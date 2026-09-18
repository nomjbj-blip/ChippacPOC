using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DACrux.SP.Controls;
using DACrux.SP.Common;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartParser.Designer
{

    public partial class frmMain : Form, IStatusBar
    {
        #region " Member Field "
        public static Analysis oAnalysis = null;
        private static string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // string.Empty;
        private string LangPath = Application.StartupPath + "\\" + "lang";
        #endregion

        #region " Creator "

        public frmMain()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();

            InitializeComponent();

            this.Load += new System.EventHandler(this.frmMain_Load);
        }

        #endregion

        #region " Event Handler "

        private void frmMain_Load(object sender, EventArgs e)
        {
            // [분석]
            // 새파일
            newToolStripMenuItem.Click += (s, a) => RunMenu(newToolStripMenuItem);  
            // 열기
            openToolStripMenuItem.Click += (s, a) => RunMenu(openToolStripMenuItem);
            // 저장
            saveToolStripMenuItem.Click += (s, a) => RunMenu(saveToolStripMenuItem);
            // 다른이름으로
            saveAsToolStripMenuItem.Click += (s, a) => RunMenu(saveAsToolStripMenuItem);
            // XML 저장
            sprSaveToolStripMenuItem.Click += (s, a) => RunMenu(sprSaveToolStripMenuItem);
            // 옵션
            optionToolStripMenuItem.Click += (s, a) => RunMenu(optionToolStripMenuItem);
            // 종료
            closeToolStripMenuItem.Click += (s, a) => RunMenu(closeToolStripMenuItem);

            // [설정]
            // 구성요소 등록
            entityRegistrationToolStripMenuItem.Click += (s, a) => RunMenu(entityRegistrationToolStripMenuItem);
            // 구역 등록
            sectionRegistrationToolStripMenuItem.Click += (s, a) => RunMenu(sectionRegistrationToolStripMenuItem);
            // DB 연결
            dataBaseConnectionToolStripMenuItem.Click += (s, a) => RunMenu(dataBaseConnectionToolStripMenuItem);
            // 동작 관리
            actionManagementToolStripMenuItem.Click += (s, a) => RunMenu(actionManagementToolStripMenuItem);

            // [테스트]
            // 배치 테스트
            runBatchTestToolStripMenuItem.Click += (s, a) => RunMenu(runBatchTestToolStripMenuItem);

            // [추출]
            // To Format File
            toFormatFileToolStripMenuItem.Click += (s, a) => RunMenu(toFormatFileToolStripMenuItem);

            // [인코딩]
            aboutSmartParserToolStripMenuItem.Click += (s, a) => RunMenu(aboutSmartParserToolStripMenuItem);
            asciiToolStripMenuItem.Click += (s, a) => RunMenu(asciiToolStripMenuItem);
            bigEndianUniToolStripMenuItem.Click += (s, a) => RunMenu(bigEndianUniToolStripMenuItem);
            defaultToolStripMenuItem.Click += (s, a) => RunMenu(defaultToolStripMenuItem);
            unicodeToolStripMenuItem.Click += (s, a) => RunMenu(unicodeToolStripMenuItem);
            uTF32ToolStripMenuItem.Click += (s, a) => RunMenu(uTF32ToolStripMenuItem);
            uTF7ToolStripMenuItem.Click += (s, a) => RunMenu(uTF7ToolStripMenuItem);
            uTF8ToolStripMenuItem.Click += (s, a) => RunMenu(uTF8ToolStripMenuItem);

            // [언어]
            optionToolStripMenuItem1.Click += (s, a) => RunMenu(optionToolStripMenuItem1);
            korToolStripMenuItem.Click += (s, a) => RunMenu(korToolStripMenuItem);
            engToolStripMenuItem.Click += (s, a) => RunMenu(engToolStripMenuItem);
            otherToolStripMenuItem.Click += (s, a) => RunMenu(otherToolStripMenuItem);
            // 인코딩/랭귀지 프로세서를 위해 따로 빼논 이벤트
            encodingToolStripMenuItem.Click += new EventHandler(encodingToolStripMenuItem_Click);
            selectLangToolStripMenuItem.Click += new EventHandler(selectLangToolStripMenuItem_Click);
            langToolStripMenuItem.Click += new EventHandler(langToolStripMenuItem_Click);

            string tempFolderPath = DACrux.SP.Common.Utility.CombinedPath(Application.StartupPath, "TEMP");

            if (!Directory.Exists(tempFolderPath))
                Directory.CreateDirectory(tempFolderPath);


            try
            {
                string[] args = Environment.GetCommandLineArgs();

                if (args != null && args.Length > 1)
                    ProcessOpenAnalysis(new FileInfo(args[1]));
            }
            catch
            {
            }

            #region [ 연구대상 - 루프 내에서 람다식에 전달되는 인자를 컴파일러가 어느 시점 기준으로 전달하는 지... ]
            /// 외부 루프 하나당 핸들러 대리자가 하나씩만 생성됨...
            //foreach (ToolStripMenuItem toolItem in menuStrip1.Items)
            //    foreach (ToolStripItem mnuItem in toolItem.DropDownItems)
            //        if (mnuItem is ToolStripMenuItem)
            //            mnuItem.Click += (s, a) => RunMenu(mnuItem);
            #endregion
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (oAnalysis != null)
            {
                switch (MessageBox.Show("Do you want to save current analysis before closing?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
                {
                    case DialogResult.Yes:
                        SaveAnalysis();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }

            base.OnClosing(e);
        }


        #endregion

        #region " Method "

        private void RunMenu(ToolStripItem toolStripItem)
        {
            try
            {
                if (toolStripItem.Tag == null)
                    return;

                string strMenuTag = toolStripItem.Tag.ToString();
                DACrux.SP.Common.MultiLang multilang = new MultiLang();
                StreamWriter sw = null;
                switch (strMenuTag)
                {
                    case "ANALYSIS_NEW":
                        if (oAnalysis == null)
                            NewAnalysis();
                        else
                        {
                            //2015-09-18
                            //switch (MessageBox.Show("Do you want to save this analysis before closing?", "Information"
                            //    , MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
                            //{
                            //    case DialogResult.Yes:
                            //        {
                            //            SaveAnalysis();
                            //            CloseAnalysis();
                            //            NewAnalysis();
                            //        }
                            //        break;
                            //    case DialogResult.No:
                            //        {
                            //            CloseAnalysis();
                            //            NewAnalysis();
                            //        }
                            //        break;
                            //}
                            //

                            ///
                            ///
                            CloseAnalysis();
                            NewAnalysis();
                        }
                        break;
                    case "ANALYSIS_OPEN":
                        OpenAnalysis();
                        break;
                    case "ANALYSIS_SAVE":
                        SaveAnalysis();
                        break;
                    case "ANALYSIS_SAVE_AS":
                        SaveAnalysisAs(true);
                        break;
                    case "XML_Save":
                        XMLSaveAnalysis();
                        break;
                    case "ANALYSIS_OPTION":
                        OptionAnalysis();
                        break;
                    case "ANALYSIS_CLOSE":
                        CloseAnalysis();
                        break;

                    case "SETUP_ENTITY_REGISTRATION":
                        RegisterEntry();
                        break;
                    case "SETUP_SECTION_REGISTRATION":
                        RegisterSection();
                        break;
                    case "SETUP_DATABASE_CONNECTION":
                        DBConnectionTest();
                        break;
                    case "SETUP_ACTION_MANAGEMENT":
                        ActionManagement();
                        break;
                    case "TEST_RUN_BATCH_TEST":
                        RunBatchTest();
                        break;

                    case "Ascii":
                        EncodingData(encoding.ASCII);
                        checkEncoding(asciiToolStripMenuItem);

                        break;
                    case "BigEndianUni":
                        EncodingData(encoding.BigEndianUnicode);
                        checkEncoding(bigEndianUniToolStripMenuItem);
                        break;
                    case "Default":
                        EncodingData(encoding.Default);
                        checkEncoding(defaultToolStripMenuItem);
                        break;
                    case "Unicode":
                        EncodingData(encoding.Unicode);
                        checkEncoding(unicodeToolStripMenuItem);
                        break;
                    case "UTF32":
                        EncodingData(encoding.UTF32);
                        checkEncoding(uTF32ToolStripMenuItem);
                        break;
                    case "UTF7":
                        EncodingData(encoding.UTF7);
                        checkEncoding(uTF7ToolStripMenuItem);
                        break;
                    case "UTF8":
                        EncodingData(encoding.UTF8);
                        checkEncoding(uTF8ToolStripMenuItem);
                        break;

                    case "LanguageOption":
                        RunLang();
                        break;
                    case "Kor":
                        checklang(korToolStripMenuItem);
                        sw = new StreamWriter(LangPath);
                        sw.Write("Kor");
                        sw.Dispose();
                        multilang.CheckLang();
                        sw = null;
                        MessageBox.Show("언어는 프로세스 재시작후 적용됩니다.", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;
                    case "Eng":
                        checklang(engToolStripMenuItem);
                        sw = new StreamWriter(LangPath);
                        sw.Write("Eng");
                        sw.Dispose();
                        multilang.CheckLang();
                        sw = null;
                        MessageBox.Show("language are applied after restart process", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;

                    case "other":
                        checklang(otherToolStripMenuItem);
                        sw = new StreamWriter(LangPath);
                        sw.Write("Other");
                        sw.Dispose();
                        multilang.CheckLang();
                        sw = null;
                        MessageBox.Show("language are applied after restart process", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;

                    case "EXPORT_TO_FORMAT_FILE":
                        ExportToFormatFile();
                        break;

                    case "HELP_ABOUT_SMART_PARSER":
                        AboutSmartParser();
                        break;
                }
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }
        #region[Encoding]
        private void checkEncoding(ToolStripMenuItem item)
        {
            bigEndianUniToolStripMenuItem.Checked = false;
            defaultToolStripMenuItem.Checked = false;
            unicodeToolStripMenuItem.Checked = false;
            uTF32ToolStripMenuItem.Checked = false;
            uTF7ToolStripMenuItem.Checked = false;
            uTF8ToolStripMenuItem.Checked = false;
            asciiToolStripMenuItem.Checked = false;
            item.Checked = true;
        }
        private void EncodingData(encoding en)
        {
            string strSample = null;
            string strDataPath = null;
            //  byte[] byteTempData = null;
            //StringBuilder builder = new StringBuilder();
            //StringBuilder builder1 = new StringBuilder();
            //foreach (KeyValuePair<string, string> pair in oAnalysis.SampleFiles)
            //{
            strSample = oAnalysis.CurrentFile;
            strDataPath = oAnalysis.FilePath.Substring(0, oAnalysis.FilePath.LastIndexOf("\\") + 1) + strSample;

            switch (en)
            {
                case encoding.ASCII:

                    en = encoding.ASCII;
                    break;
                case encoding.BigEndianUnicode:
                    en = encoding.BigEndianUnicode;
                    break;
                case encoding.Default:
                    en = encoding.Default;
                    break;
                case encoding.Unicode:
                    en = encoding.Unicode;
                    break;
                case encoding.UTF32:
                    en = encoding.UTF32;
                    break;
                case encoding.UTF7:
                    en = encoding.UTF7;
                    break;
                case encoding.UTF8:
                    en = encoding.UTF8;
                    break;

            }
            foreach (KeyValuePair<string, string> pair in oAnalysis.SampleFiles)
            {
                if (pair.Key == strSample)
                {
                    oAnalysis.SampleFiles.Remove(strSample);
                    break;
                }
                //oAnalysis.SampleFiles.Clear();
            }
            oAnalysis.encoding = en;

            oAnalysis.SampleFiles.Add(strSample, DACrux.SP.Common.Utility.ReadStringFromFile(strDataPath, en));
            RegisterEntryEncoding();
        }
        void encodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oAnalysis != null)
            {

                switch (oAnalysis.encoding)
                {
                    case encoding.ASCII:
                        checkEncoding(asciiToolStripMenuItem);
                        break;
                    case encoding.BigEndianUnicode:
                        checkEncoding(bigEndianUniToolStripMenuItem);
                        break;
                    case encoding.Default:
                        checkEncoding(defaultToolStripMenuItem);
                        break;
                    case encoding.Unicode:
                        checkEncoding(unicodeToolStripMenuItem);
                        break;
                    case encoding.UTF32:
                        checkEncoding(uTF32ToolStripMenuItem);
                        break;
                    case encoding.UTF7:
                        checkEncoding(uTF7ToolStripMenuItem);
                        break;
                    case encoding.UTF8:
                        checkEncoding(uTF8ToolStripMenuItem);
                        break;
                    default: break;
                }
            }
        }
        #endregion
        #region[Lang]
        private void RunLang()
        {
            frmLang frmlang = new frmLang();
            frmlang.MdiParent = this;
            //frmlang.WindowState = FormWindowState.Maximized;
            frmlang.StartPosition = FormStartPosition.CenterParent;
            frmlang.Show();

        }
        private void checklang(ToolStripMenuItem item)
        {

            korToolStripMenuItem.Checked = false;
            engToolStripMenuItem.Checked = false;
            otherToolStripMenuItem.Checked = false;
            item.Checked = true;

            Form[] f = this.MdiChildren;
            foreach (Form f1 in f)
            {

            }

        }
        void selectLangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DACrux.SP.Common.MultiLang.SelectLang["Lang"].Equals("Kor"))
            {
                otherToolStripMenuItem.Checked = false;
                engToolStripMenuItem.Checked = false;
                korToolStripMenuItem.Checked = true;

            }
            else if (DACrux.SP.Common.MultiLang.SelectLang["Lang"].Equals("Eng"))
            {
                otherToolStripMenuItem.Checked = false;
                korToolStripMenuItem.Checked = false;
                engToolStripMenuItem.Checked = true;
            }
            else
            {

                korToolStripMenuItem.Checked = false;
                engToolStripMenuItem.Checked = false;
                otherToolStripMenuItem.Checked = true;
            }
        }
        void langToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DACrux.SP.Common.MultiLang.SelectLang["Lang"].Equals("Kor"))
            {
                otherToolStripMenuItem.Checked = false;
                engToolStripMenuItem.Checked = false;
                korToolStripMenuItem.Checked = true;

            }
            else if (DACrux.SP.Common.MultiLang.SelectLang["Lang"].Equals("Eng"))
            {
                otherToolStripMenuItem.Checked = false;
                korToolStripMenuItem.Checked = false;
                engToolStripMenuItem.Checked = true;
            }
            else
            {

                korToolStripMenuItem.Checked = false;
                engToolStripMenuItem.Checked = false;
                otherToolStripMenuItem.Checked = true;
            }
        }

        #endregion
        #region [ Analysis ]

        private void NewAnalysis()
        {
            try
            {
                oAnalysis = Analysis.GetInstance();
                oAnalysis.PatternMode = PatternModeItem.Multi;
                if (!ProcessOption())
                {
                    oAnalysis = null;
                    return;
                }

                RegisterEntry();
            }
            catch (Exception ex)
            {
                oAnalysis = null;
                throw (new Exception(ex.Message + " \r\n\t: AnalysisManager.NewAnalysis()"));
            }
        }

        private void OpenAnalysis()
        {
            OpenFileDialog dlgOpenAnalysis = null;

            try
            {
                dlgOpenAnalysis = new OpenFileDialog();
                dlgOpenAnalysis.InitialDirectory = savePath;
                dlgOpenAnalysis.AddExtension = true;
                dlgOpenAnalysis.Filter = "SmartParser File (*.spr)|*.spr";

                if (dlgOpenAnalysis.ShowDialog() == DialogResult.OK)
                {
                    if (oAnalysis != null)
                        CloseAnalysis();

                    oAnalysis = Analysis.GetInstance();
                    ProcessOpenAnalysis(new FileInfo(dlgOpenAnalysis.FileName));
                }
            }
            catch
            {
                MessageBox.Show("An error occured while opening the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dlgOpenAnalysis != null)
                    dlgOpenAnalysis.Dispose();
            }
        }

        private void SaveAnalysis()
        {
            if (oAnalysis == null)
            {
                MessageBox.Show("No Analysis exist.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < oAnalysis.Entities.Count; i++)
            {
                if (!oAnalysis.Entities[i].Name.Equals("X") && !oAnalysis.Entities[i].Name.Equals("Y") && !oAnalysis.Entities[i].Name.Equals("Wafer_Num"))
                {
                    if (oAnalysis.Entities[i].RegexString.Length == 0 && oAnalysis.Entities[i].DefaultValue.Length == 0)
                    {
                        MessageBox.Show("Please Input regex or default value of components.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }

            if (!oAnalysis.Saved)
            {
                SaveAnalysisAs(true);
            }
            else
            {
                ProcessSaveAnalysis(true);
            }
        }
        private void XMLSaveAnalysis()
        {
            if (oAnalysis == null)
            {
                MessageBox.Show("No Analysis exist.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < oAnalysis.Entities.Count; i++)
            {
                if (!oAnalysis.Entities[i].Name.Equals("X") && !oAnalysis.Entities[i].Name.Equals("Y") && !oAnalysis.Entities[i].Name.Equals("Wafer_Num"))
                {
                    if (oAnalysis.Entities[i].RegexString.Length == 0 && oAnalysis.Entities[i].DefaultValue.Length == 0)
                    {
                        MessageBox.Show("Please Input regex or default value of components.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }

            if (!oAnalysis.XMLSaved)
            {
                SaveAnalysisAs(false);
            }
            else
            {
                ProcessSaveAnalysis(false);
            }
        }
        private void SaveAnalysisAs(bool flag)
        {
            SaveFileDialog dlgSaveAnalysis = null;

            try
            {

                for (int i = 0; i < oAnalysis.Entities.Count; i++)
                {
                    if (!oAnalysis.Entities[i].Name.Equals("X") && !oAnalysis.Entities[i].Name.Equals("Y") && !oAnalysis.Entities[i].Name.Equals("Wafer_Num"))
                    {
                        if (oAnalysis.Entities[i].RegexString.Length == 0 && oAnalysis.Entities[i].DefaultValue.Length == 0)
                        {
                            MessageBox.Show("Please Input regex or default value of components.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                dlgSaveAnalysis = new SaveFileDialog();
                dlgSaveAnalysis.InitialDirectory = savePath;
                dlgSaveAnalysis.AddExtension = true;
                dlgSaveAnalysis.Filter = "Smart Parser File (*.spr)|*.spr";

                if (dlgSaveAnalysis.ShowDialog() == DialogResult.OK)
                {
                    oAnalysis.FilePath = dlgSaveAnalysis.FileName;
                    ProcessSaveAnalysis(flag);
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (dlgSaveAnalysis != null)
                {
                    dlgSaveAnalysis.Dispose();
                    dlgSaveAnalysis = null;
                }
            }
        }

        private void OptionAnalysis()
        {
            try
            {
                if (!ProcessOption())
                    return;

                RegisterEntry();
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: AnalysisManager.NewAnalysis()"));
            }
        }

        private void CloseAnalysis()
        {
            foreach (Form frm in MdiChildren)
            {
                if (frm.Name.Equals("frmRegistration"))
                    frm.Close();
            }
            Analysis.Close();
            oAnalysis = null;

            //this.Close();
        }

        private bool ProcessOption()
        {
            try
            {
                if (oAnalysis == null)
                    return false;

                dlgOption dlg = new dlgOption(oAnalysis);

                dlg.StartPosition = FormStartPosition.CenterParent;

                return dlg.ShowDialog() == DialogResult.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessOpenAnalysis(FileInfo fileInfo)
        {
            try
            {
                oAnalysis = Analysis.GetInstance();

                oAnalysis.OpenFormatFile(FormatFileType.Formatter, fileInfo.FullName);

                RegisterEntry();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessSaveAnalysis(bool flag)
        {
            try
            {
                if (oAnalysis.FilePath == null || oAnalysis.FilePath.Trim() == string.Empty)
                    throw new Exception("The save path hasn't defined.");// oAnalysis.FilePath = savePath + @"\" + oAnalysis.Name + ".spr";

                oAnalysis.SaveFormatFile(FormatFileType.Formatter, oAnalysis.FilePath, flag);
                MessageBox.Show("Analysis successfully saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region [ Setup ]

        private void RegisterEntry()
        {
            if (oAnalysis == null)
                return;

            if (oAnalysis.SampleFiles.Count < 1)
                return;

            frmRegistration frm = (frmRegistration)Array.Find<Form>(this.MdiChildren
                , form => form.Name.Equals("SmartParser.Designer.frmRegistration"));

            if (frm == null)
            {
                frm = new SmartParser.Designer.frmRegistration(ActiveTabItems.Entity);
                frm.MdiParent = this;

                frm.WindowState = FormWindowState.Maximized;

                frm.Show();
            }
            else
                frm.Select();
        }
        private void RegisterEntryEncoding()
        {
            if (oAnalysis == null)
                return;

            if (oAnalysis.SampleFiles.Count < 1)
                return;

            //List<Form> frm = new List<Form>();
            Form[] frms = this.MdiChildren;
            foreach (Form fm in frms)
            {
                if (fm.Name.Equals("frmRegistration"))
                {
                    fm.Close();
                    break;
                }
            }
            frmRegistration frm = (frmRegistration)Array.Find<Form>(this.MdiChildren
                , form => form.Name.Equals("SmartParser.Designer.frmRegistration"));

            if (frm == null)
            {
                frm = new SmartParser.Designer.frmRegistration(ActiveTabItems.Entity);
                frm.MdiParent = this;

                frm.WindowState = FormWindowState.Maximized;

                frm.Show();
            }
            else
                frm.Select();
        }
        private void RegisterSection()
        {
            if (oAnalysis == null)
                return;

            if (oAnalysis.SampleFiles.Count < 1)
                return;

            frmRegistration frm = (frmRegistration)Array.Find<Form>(this.MdiChildren
                , form => form.Name.Equals("SmartParser.Designer.frmRegistration"));

            if (frm == null)
            {
                frm = new SmartParser.Designer.frmRegistration(ActiveTabItems.Section);
                frm.MdiParent = this;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
            }
            else
                frm.Select();
        }

        private void DBConnectionTest()
        {
            if (oAnalysis == null)
                return;

            dlgDatabaseConnection dlg = new dlgDatabaseConnection();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                oAnalysis.ConnectionInfo = dlg.ConnectionInfo;
            }
        }

        private void ActionManagement()
        {
            if (oAnalysis == null)
                return;

            frmActionManagement frm = (frmActionManagement)Array.Find<Form>(this.MdiChildren
                , form => form.Name.Equals("SmartParser.Designer.frmActionManagement"));

            if (frm == null)
            {
                frm = new SmartParser.Designer.frmActionManagement();
                frm.MdiParent = this;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
            }
            else
                frm.Select();
        }

        #endregion

        #region [ Database ]

        private void DBTableDataMatching()
        {
            if (oAnalysis == null)
                return;

            frmTableDataMatching frm = (frmTableDataMatching)Array.Find<Form>(this.MdiChildren
                , form => form.Name.Equals("SmartParser.Designer.frmTableDataMatching"));

            if (frm == null)
            {
                frm = new SmartParser.Designer.frmTableDataMatching();
                frm.MdiParent = this;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
            }
            else
                frm.Select();
        }

        #endregion

        #region [ Test ]

        private void RunBatchTest()
        {
            new ScriptRunner(oAnalysis.Tasks, oAnalysis.Scripts, new FileLogger(@"c:\CFF_LOG.txt")).Run();
        }

        #endregion

        #region [ Export ]

        private void ExportToFormatFile()
        {
            SaveFileDialog dlgSave = null;

            try
            {
                //    dlgSave = new SaveFileDialog();
                //    dlgSave.InitialDirectory = savePath;
                //    dlgSave.AddExtension = true;
                //    dlgSave.Filter = "Common Formatter File (*.mcf)|*.mcf";

                //    if (dlgSave.ShowDialog() == DialogResult.OK)
                //    {
                //        oAnalysis.SaveFormatFile(FormatFileType.Parser, dlgSave.FileName,true);
                //        MessageBox.Show("Format file successfully exported.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //Form[] frms = this.MdiChildren;
                //if (frms.Length > 0)
                //{
                //    foreach (Form fm in frms)
                //    {
                //        if (fm.Name.Equals("frmConvert"))
                //        {
                //            fm.Close();
                //            break;
                //        }
                //    }
                //    frmConvert frm = new frmConvert();
                //    frm.MdiParent = this;
                //    frm.WindowState = FormWindowState.Normal;
                //    frm.Show();
                //}
                //else
                //{
                //}
                
                //frm.Size = new Size(337, 207);

                Form[] frms = this.MdiChildren;
                foreach (Form fm in frms)
                {
                    if (fm.Name.Equals("frmConvert"))
                    {
                        fm.Close();
                        break;
                    }
                }
                frmConvert frm = new frmConvert();
                frm.MdiParent = this;
                frm.WindowState = FormWindowState.Normal;
                frm.Show();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (dlgSave != null)
                {
                    dlgSave.Dispose();
                    dlgSave = null;
                }
            }
        }



        #endregion

        #region [ Help ]

        private void AboutSmartParser()
        {

        }

        #endregion

        #endregion

        #region IStatusBar Members

        public void SetMessage(string message)
        {
            statusBar.Text = message;
        }

        #endregion
    }
}
