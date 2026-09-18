using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DACrux.Utility
{
    // Excel Export Argument 2019.08.04 Taihi,Kim.
    // 이 클래스는 DACrux.Utility.ExcelExportManager 클래스의 매개변수로 사용됩니다.
    public class ExcelExportArgs : EventArgs
    {
        public ExcelExportArgs()
        {
            SheetList = new ExcelSheetList();
        }

        public ExcelSheetList SheetList
        {
            get;
            private set;
        }

        public string FileName
        {
            get;
            set;
        }
    }

    // 텍스트 표현을 위한 클래스. 문자열, 폰트, 컬러로 구성 2019.08.04 Taihi,Kim.
    public class TextObject
    {
        public static readonly Font DefaultFont = Control.DefaultFont;
        public static readonly Color DefaultColor = Control.DefaultForeColor;

        public TextObject(string text, Font font, Color fontColor)
        {
            Text = text;
            Font = font;
            FontColor = fontColor;
        }

        public TextObject(string text, Font font)
            : this(text, font, DefaultColor)
        {
        }

        public TextObject(string text, float fontSize, bool bold, Color fontColor)
            : this(text, new Font(DefaultFont.Name, fontSize, (bold ? FontStyle.Bold : FontStyle.Regular)), fontColor)
        {
        }

        public TextObject(string text, float fontSize, bool bold)
            : this(text, fontSize, bold, DefaultColor)
        {
        }

        public TextObject(string text, float fontSize, Color fontColor)
            : this(text, fontSize, false, fontColor)
        {
        }

        public TextObject(string text, float fontSize)
            : this(text, fontSize, false, DefaultColor)
        {
        }

        public TextObject(string text)
            : this(text, DefaultFont, DefaultColor)
        {
        }

        public string Text { get; private set; }
        public Font Font { get; private set; }
        public Color FontColor { get; private set; }
    }

    // 엑셀의 여러 Sheet를 표현 2019.08.04 Taihi,Kim.
    public class ExcelSheetList : List<ExcelSheet>
    {
        public void Add(string sheetName)
        {
            Add(new ExcelSheet(sheetName));
        }

        public void Add()
        {
            Add(GetUniqueName());
        }

        public new void Add(ExcelSheet sheet)
        {
            base.Add(sheet);

            if (String.IsNullOrEmpty(sheet.SheetName))
                sheet.SheetName = GetUniqueName();
        }

        public int GetTotalItemCount()
        {
            int count = 0;

            foreach (ExcelSheet sheet in this)
                count += sheet.ItemList.Count;

            return count;
        }

        public string GetUniqueName()
        {
            int i = 1;

            while (true)
            {
                string name = String.Format("Sheet{0}", i++);
                bool exists = false;

                foreach (ExcelSheet sheet in this)
                {
                    if (sheet.SheetName == name)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    return name;
            }
        }

        public ExcelSheet this[string sheetName]
        {
            get
            {
                foreach (ExcelSheet sheet in this)
                {
                    if (sheet.SheetName == sheetName)
                        return sheet;
                }

                throw new Exception(String.Format("시트'{0}'를 찾을 수 없습니다.", sheetName));
            }
        }
    }

    // 엑셀의 Sheet를 표현 2019.08.04 Taihi,Kim.
    public class ExcelSheet
    {
        public static readonly object EmptyItem = new object();

        public ExcelSheet()
        {
            ImageScale = 1.0f;
            ItemList = new System.Collections.ArrayList();
        }

        public ExcelSheet(string sheetName)
            : this()
        {
            SheetName = sheetName;
        }

        // 빈 row 추가
        public void Add()
        {
            ItemList.Add(EmptyItem);
        }

        public void Add(params Control[] controls)
        {
            if (controls == null || controls.Length == 0)
                return;

            ItemList.Add(controls);
        }

        public void Add(params DataTable[] dataTables)
        {
            if (dataTables == null || dataTables.Length == 0)
                return;

            ItemList.Add(dataTables);
        }

        public void Add(params Image[] images)
        {
            if (images == null || images.Length == 0)
                return;

            ItemList.Add(images);
        }

        public void Add(params string[] imagePaths)
        {
            if (imagePaths == null || imagePaths.Length == 0)
                return;

            ItemList.Add(imagePaths);
        }

        public void Add(params TextObject[] textObjects)
        {
            if (textObjects == null || textObjects.Length == 0)
                return;

            ItemList.Add(textObjects);
        }

        public string SheetName
        {
            get;
            set;
        }

        // image scale factor 기본값은 1
        public float ImageScale
        {
            get;
            set;
        }

        internal ArrayList ItemList
        {
            get;
            private set;
        }
    }
}
