using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;

namespace MenuFileEditor
{
    /// <summary>
    /// 속성 창(PropertyGrid)에 보이는 메뉴 한 개 (2026-10-15).
    /// 목적: Table1 행의 30개 열을 분류(General / View Option / Caption / Icon / Assembly / Etc)로 나눠 보이고 바로 고치게 한다.
    /// 처리: 값을 읽을 때는 행에서 바로 읽고, 고칠 때는 MenuDocument 의 함수로만 바꾼다(Key 변경 시 하위 Parent 같이 변경,
    ///       Parent 변경 시 순서 / Level 정리). 잘못된 값이면 예외를 던지고, 속성 창이 오류 창을 띄운 뒤 원래 값으로 돌린다.
    /// 순서(MNU_ORDER)와 Level(MNU_LEVEL)은 읽기 전용이다. 순서는 위 / 아래 이동 버튼으로, Level 은 부모에 맞춰 자동으로 바뀐다.
    /// </summary>
    [DefaultProperty("MenuKey")]
    public sealed class MenuItemView
    {
        private const string General = "[01] General";
        private const string ViewOption = "[02] View Option";
        private const string Caption = "[03] Caption";
        private const string Icon = "[04] Icon";
        private const string AssemblyInfo = "[05] Assembly";
        private const string Etc = "[06] Etc";

        private readonly MenuDocument document;
        private readonly DataRow row;

        public MenuItemView(MenuDocument document, DataRow row)
        {
            this.document = document;
            this.row = row;
        }

        [Browsable(false)]
        public MenuDocument Document
        {
            get { return document; }
        }

        [Browsable(false)]
        public DataRow Row
        {
            get { return row; }
        }

        private string Get(string column)
        {
            return MenuDocument.Text(row, column);
        }

        private void Set(string column, object value)
        {
            document.SetValue(row, column, value);
        }

        #region [01] General

        [Category(General), DisplayName("Menu Key"), Description("메뉴 고유 이름(MENU_KEY). 바꾸면 하위 메뉴의 Popup(Parent) Menu 도 같이 바뀐다. 공백 / 중복 불가.")]
        public string MenuKey
        {
            get { return Get("MENU_KEY"); }
            set { document.RenameKey(row, value); }
        }

        [Category(General), DisplayName("Menu ID"), Description("MENU_ID. 보통 Menu Key 와 같다.")]
        public string MenuId
        {
            get { return Get("MENU_ID"); }
            set { Set("MENU_ID", value); }
        }

        [Category(General), DisplayName("Popup(Parent) Menu"), Description("부모 메뉴(POPUP_MENU). 최상위는 ROOT. 바꾸면 새 부모의 마지막 순서로 옮긴다.")]
        [TypeConverter(typeof(ParentMenuConverter))]
        public string PopupMenu
        {
            get { return Get("POPUP_MENU"); }
            set { document.ChangeParent(row, value); }
        }

        [Category(General), DisplayName("Stock Toolbar"), Description("메뉴 아래 도구 막대에 넣을 때의 도구 막대 이름(STOCK_TOOLBAR). 비우면 도구 막대에 넣지 않는다.")]
        [TypeConverter(typeof(StockToolbarConverter))]
        public string StockToolbar
        {
            get { return Get("STOCK_TOOLBAR"); }
            set { Set("STOCK_TOOLBAR", value); }
        }

        #endregion

        #region [02] View Option

        [Category(ViewOption), DisplayName("Enabled"), Description("ENABLE. False 면 메뉴가 회색으로 눌리지 않는다.")]
        public bool Enabled
        {
            get { return MenuDocument.IsTrue(row, "ENABLE"); }
            set { Set("ENABLE", value); }
        }

        [Category(ViewOption), DisplayName("Visible"), Description("VISIBLE. False 면 메뉴가 보이지 않는다(하위 메뉴도 함께 안 보임).")]
        public bool Visible
        {
            get { return MenuDocument.IsTrue(row, "VISIBLE"); }
            set { Set("VISIBLE", value); }
        }

        [Category(ViewOption), DisplayName("Start"), Description("IS_START. True 면 이 메뉴 위에 구분선을 넣는다.")]
        public bool Start
        {
            get { return MenuDocument.IsTrue(row, "IS_START"); }
            set { Set("IS_START", value); }
        }

        [Category(ViewOption), DisplayName("Multiple Dialog"), Description("MULTI. True 면 같은 화면을 여러 개 열 수 있다.")]
        public bool Multiple
        {
            get { return MenuDocument.IsTrue(row, "MULTI"); }
            set { Set("MULTI", value); }
        }

        #endregion

        #region [03] Caption

        [Category(Caption), DisplayName("Caption 1"), Description("CAPTION001 (English)")]
        public string Caption1
        {
            get { return Get("CAPTION001"); }
            set { Set("CAPTION001", value); }
        }

        [Category(Caption), DisplayName("Caption 2"), Description("CAPTION002 (한국어)")]
        public string Caption2
        {
            get { return Get("CAPTION002"); }
            set { Set("CAPTION002", value); }
        }

        [Category(Caption), DisplayName("Caption 3"), Description("CAPTION003 (中文)")]
        public string Caption3
        {
            get { return Get("CAPTION003"); }
            set { Set("CAPTION003", value); }
        }

        [Category(Caption), DisplayName("Caption 4"), Description("CAPTION004")]
        public string Caption4
        {
            get { return Get("CAPTION004"); }
            set { Set("CAPTION004", value); }
        }

        [Category(Caption), DisplayName("Caption 5"), Description("CAPTION005")]
        public string Caption5
        {
            get { return Get("CAPTION005"); }
            set { Set("CAPTION005", value); }
        }

        [Category(Caption), DisplayName("Tool Tip"), Description("TOOL_TIP")]
        public string ToolTip
        {
            get { return Get("TOOL_TIP"); }
            set { Set("TOOL_TIP", value); }
        }

        #endregion

        #region [04] Icon

        [Category(Icon), DisplayName("ICon [32X32]"), Description("ICON32. [...] 를 눌러 그림 파일을 고르면 32 x 32 PNG 로 바꿔 넣는다.")]
        [Editor(typeof(IconValueEditor), typeof(UITypeEditor)), TypeConverter(typeof(IconTextConverter))]
        public string Icon32
        {
            get { return Get("ICON32"); }
            set { Set("ICON32", value); }
        }

        [Category(Icon), DisplayName("ICon [16X16]"), Description("ICON16. [...] 를 눌러 그림 파일을 고르면 16 x 16 PNG 로 바꿔 넣는다.")]
        [Editor(typeof(IconValueEditor), typeof(UITypeEditor)), TypeConverter(typeof(IconTextConverter))]
        public string Icon16
        {
            get { return Get("ICON16"); }
            set { Set("ICON16", value); }
        }

        #endregion

        #region [05] Assembly

        [Category(AssemblyInfo), DisplayName("Form Type"), Description("FORM_TYPE. BASIC_FORM = 화면 탭 / MODAL_FORM = 대화 상자.")]
        [TypeConverter(typeof(FormTypeConverter))]
        public string FormType
        {
            get { return Get("FORM_TYPE"); }
            set { Set("FORM_TYPE", value); }
        }

        [Category(AssemblyInfo), DisplayName("Module"), Description("MODULE. 화면이 있는 DLL 파일 이름(예: DACrux.SEMDMS.ENGUI.dll).")]
        public string Module
        {
            get { return Get("MODULE"); }
            set { Set("MODULE", value); }
        }

        [Category(AssemblyInfo), DisplayName("Assembly"), Description("ASSEMBLY. 화면 클래스 전체 이름(예: DACrux.SEMDMS.ENGUI.frmKlarfUpload).")]
        public string Assembly
        {
            get { return Get("ASSEMBLY"); }
            set { Set("ASSEMBLY", value); }
        }

        [Category(AssemblyInfo), DisplayName("Function Code"), Description("FUNCTION_CODE. 권한 확인에 쓰는 기능 코드.")]
        public string FunctionCode
        {
            get { return Get("FUNCTION_CODE"); }
            set { Set("FUNCTION_CODE", value); }
        }

        [Category(AssemblyInfo), DisplayName("Help URL"), Description("HELP_URL")]
        public string HelpUrl
        {
            get { return Get("HELP_URL"); }
            set { Set("HELP_URL", value); }
        }

        [Category(AssemblyInfo), DisplayName("Menu Order"), ReadOnly(true), Description("MNU_ORDER. 형제 안 순서(0부터). 위 / 아래 이동 버튼으로 바꾼다.")]
        public int MenuOrder
        {
            get { int order = MenuDocument.Order(row); return order == int.MaxValue ? 0 : order; }
        }

        [Category(AssemblyInfo), DisplayName("Menu Level"), ReadOnly(true), Description("MNU_LEVEL. 최상위 = 1. 부모를 바꾸면 자동으로 맞춘다.")]
        public int MenuLevel
        {
            get { object value = row["MNU_LEVEL"]; return value == DBNull.Value ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture); }
        }

        #endregion

        #region [06] Etc

        [Category(Etc), DisplayName("Ribbon Tab"), Description("RIBBON_TAB")]
        public string RibbonTab
        {
            get { return Get("RIBBON_TAB"); }
            set { Set("RIBBON_TAB", value); }
        }

        [Category(Etc), DisplayName("Ribbon Group"), Description("RIBBON_GROUP")]
        public string RibbonGroup
        {
            get { return Get("RIBBON_GROUP"); }
            set { Set("RIBBON_GROUP", value); }
        }

        [Category(Etc), DisplayName("RESV 01"), Description("RESV_01. DMS Link 메뉴는 화면 생성자 인자로 쓴다.")]
        public string Resv01
        {
            get { return Get("RESV_01"); }
            set { Set("RESV_01", value); }
        }

        [Category(Etc), DisplayName("RESV 02"), Description("RESV_02")]
        public string Resv02
        {
            get { return Get("RESV_02"); }
            set { Set("RESV_02", value); }
        }

        [Category(Etc), DisplayName("RESV 03"), Description("RESV_03")]
        public string Resv03
        {
            get { return Get("RESV_03"); }
            set { Set("RESV_03", value); }
        }

        [Category(Etc), DisplayName("RESV 04"), Description("RESV_04. DM / TEST 면 왼쪽 조회 창(DMS / TEST 검색)을 연다.")]
        public string Resv04
        {
            get { return Get("RESV_04"); }
            set { Set("RESV_04", value); }
        }

        [Category(Etc), DisplayName("RESV 05"), Description("RESV_05")]
        public string Resv05
        {
            get { return Get("RESV_05"); }
            set { Set("RESV_05", value); }
        }

        #endregion
    }

    /// <summary>부모 메뉴 고르기: ROOT + 자기 자신과 하위를 뺀 모든 Menu Key(목록에서만 고름).</summary>
    public sealed class ParentMenuConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var view = context == null ? null : context.Instance as MenuItemView;
            if (view == null) return new StandardValuesCollection(new[] { MenuDocument.Root });
            return new StandardValuesCollection(view.Document.ParentCandidates(view.Row));
        }
    }

    /// <summary>Stock Toolbar 고르기: 빈 값 / MNU_DMS_LINK / MNU_EXPORT (직접 입력도 가능).</summary>
    public sealed class StockToolbarConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new[] { string.Empty, "MNU_DMS_LINK", "MNU_EXPORT" });
        }
    }

    /// <summary>Form Type 고르기: BASIC_FORM / MODAL_FORM / 빈 값 (직접 입력도 가능).</summary>
    public sealed class FormTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new[] { "BASIC_FORM", "MODAL_FORM", string.Empty });
        }
    }

    /// <summary>
    /// 아이콘 값(긴 16진수 문자열) 대신 "PNG 32 x 32 (1,234 byte)" 처럼 짧게 보인다.
    /// 글자로 직접 고치지 못하게(CanConvertFrom = false) 하고, [...] 편집 창(IconValueEditor)으로만 바꾼다.
    /// </summary>
    public sealed class IconTextConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return false;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string)) return base.ConvertTo(context, culture, value, destinationType);
            string hex = value as string;
            if (string.IsNullOrEmpty(hex) || hex.Trim().Length == 0) return "(없음)";
            using (System.Drawing.Image image = MenuDocument.HexToImage(hex))
            {
                if (image == null) return "(그림으로 읽을 수 없음)";
                int bytes = hex.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
                return string.Format(CultureInfo.InvariantCulture, "{0} x {1} ({2:N0} byte)", image.Width, image.Height, bytes);
            }
        }
    }
}
