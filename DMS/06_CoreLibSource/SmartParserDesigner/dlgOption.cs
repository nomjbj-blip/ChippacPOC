using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DACrux.SP.Common;

namespace SmartParser.Designer
{
    public partial class dlgOption : Form
    {
        #region " Member Field & Property "

        public Analysis analysis = null;
        private OpenFileDialog dlg = null;


        #endregion

        #region " Creator "

        public dlgOption(Analysis analysis)
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();
            checklang();
            this.analysis = analysis;

            uclRegex.ShowIgnoreCase = false;


            dlg = new OpenFileDialog();
            dlg.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
            dlg.Filter = "Text encoding File (*.*)|*.*|Microsoft Excel File (*.xls)|*.xls";
            dlg.FileOk += new CancelEventHandler(dlg_FileOk);
            dlg.Multiselect = true;

            KeyPreview = true;
            this.KeyDown += dlgOption_KeyDown;
        }
        void checklang()
        {
            uclRegex.dec = DACrux.SP.Common.MultiLang.SelectLang["\\d"];
            uclRegex.endline = DACrux.SP.Common.MultiLang.SelectLang["$"];
            uclRegex.etc = DACrux.SP.Common.MultiLang.SelectLang["Etc"];
            uclRegex.grouping = DACrux.SP.Common.MultiLang.SelectLang["Grouping"];
            uclRegex.groupname = DACrux.SP.Common.MultiLang.SelectLang["(?<name>subexpression)"];
            uclRegex.groupequal = DACrux.SP.Common.MultiLang.SelectLang["k<name>"];
            uclRegex.Ignorecase = DACrux.SP.Common.MultiLang.SelectLang["Ignore Case"];
            uclRegex.newline = DACrux.SP.Common.MultiLang.SelectLang["\\n"];
            uclRegex.nfromtime = DACrux.SP.Common.MultiLang.SelectLang["{n,m}"];
            uclRegex.nleasttime = DACrux.SP.Common.MultiLang.SelectLang["{n, }"];
            uclRegex.nondec = DACrux.SP.Common.MultiLang.SelectLang["\\D "];
            uclRegex.nonspace = DACrux.SP.Common.MultiLang.SelectLang["\\S"];
            uclRegex.nonword = DACrux.SP.Common.MultiLang.SelectLang["\\W"];
            uclRegex.ntime = DACrux.SP.Common.MultiLang.SelectLang["{n}"];
            uclRegex.one = DACrux.SP.Common.MultiLang.SelectLang["+"];
            uclRegex.quanti = DACrux.SP.Common.MultiLang.SelectLang["Quantifiers"];
            uclRegex.regular = DACrux.SP.Common.MultiLang.SelectLang["Regular Expression"];
            uclRegex.returnhome = DACrux.SP.Common.MultiLang.SelectLang["\\r"];
            uclRegex.space = DACrux.SP.Common.MultiLang.SelectLang["\\s"];
            uclRegex.startline = DACrux.SP.Common.MultiLang.SelectLang["^"];
            uclRegex.tab = DACrux.SP.Common.MultiLang.SelectLang["\\t"];
            uclRegex.word = DACrux.SP.Common.MultiLang.SelectLang["\\s"];
            uclRegex.zero = DACrux.SP.Common.MultiLang.SelectLang["*"];
            uclRegex.zerone = DACrux.SP.Common.MultiLang.SelectLang["?"];
        }
        void dlgOption_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region " Event Handler "

        private void dlgOption_Load(object sender, EventArgs e)
        {
            rbtSingle.Checked = (analysis.PatternMode == PatternModeItem.Single);
            if (analysis.PatternMode == PatternModeItem.Single)
                rbtSingle.Checked = true;
            else
                radioButton2.Checked = true;

            uclRegex.RegexString = analysis.TargetFileNamingRule;
            //uclRegex.
            ListViewItem item = null;
            foreach (KeyValuePair<string, string> kv in analysis.SampleFiles)
            {
                item = new ListViewItem(kv.Key);
                lvFileList.Items.Add(item);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvFileList.Items.Count < 1)
                {
                    MessageBox.Show("Please select one or more Sample file(s).", "Information"
                        , MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                analysis.PatternMode = rbtSingle.Checked ? PatternModeItem.Single : PatternModeItem.Multi;
                analysis.TargetFileNamingRule = uclRegex.RegexString;
                analysis.SampleFiles.Clear();
                analysis.HanaSampleFiles.Clear();
                ListViewItem it = lvFileList.Items[0];
                FileInfo fi = (FileInfo)it.Tag;
                foreach (ListViewItem item in lvFileList.Items)
                {
                    switch (Path.GetExtension(item.Text).ToLower())
                    {
                        case ".xls":
                            analysis.SampleFiles.Add(item.Text + ".txt", DACrux.SP.Common.Utility.ReadStringFromFile(DACrux.SP.Common.Utility.CombinedPath(Application.StartupPath, "TEMP", item.Text), encoding.ASCII));
                            break;
                        case ".csv":
                        default:
                            analysis.SampleFiles.Add(item.Text, DACrux.SP.Common.Utility.ReadStringFromFile(DACrux.SP.Common.Utility.CombinedPath(Application.StartupPath, "TEMP", item.Text), encoding.ASCII));
                            analysis.HanaSampleFiles.Add(item.Text + "_HANA", DACrux.SP.Common.Utility.ReadStringFromFile(DACrux.SP.Common.Utility.CombinedPath(Application.StartupPath, "TEMP", item.Text), encoding.ASCII));
                            break;
                    }
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string tempFolderPath = DACrux.SP.Common.Utility.CombinedPath(Application.StartupPath, "TEMP");

                if (!Directory.Exists(tempFolderPath))
                    Directory.CreateDirectory(tempFolderPath);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fullFileName in dlg.FileNames)
                    {
                        FileInfo fi = new FileInfo(fullFileName);
                        string strTargetPath = DACrux.SP.Common.Utility.CombinedPath(tempFolderPath, fi.Name);
                        analysis.FilePath = fi.FullName;
                        fi.CopyTo(strTargetPath, true);

                        if ((from ListViewItem item in lvFileList.Items
                             where item.Text.Equals(fi.Name)
                             select item.Text).Count() < 1)
                        {
                            lvFileList.Items.Add(fi.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void dlg_FileOk(object sender, CancelEventArgs e)
        {
            //if (string.IsNullOrEmpty(uclRegex.RegexString))
            //    return;

            StringBuilder sb = new StringBuilder();
            // Regex regex = new Regex(uclRegex.RegexString, RegexOptions.IgnoreCase);

            try
            {
                foreach (string fileFullName in dlg.FileNames)
                {
                    string strFileName = Path.GetFileName(fileFullName);

                    //if (!regex.IsMatch(strFileName))
                    //    sb.AppendFormat(", {0}", strFileName);
                }

                if (sb.Length > 0)
                {
                    MessageBox.Show(string.Format("Following files are not matched. ({0})", sb.ToString(2, sb.Length - 2)), "Information"
                        , MessageBoxButtons.OK, MessageBoxIcon.Information);

                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvFileList.SelectedItems.Count < 1)
                return;

            foreach (ListViewItem item in lvFileList.SelectedItems)
                item.Remove();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            //if (lvFileList.Items.Count < 1)
            //{
            //    MessageBox.Show("Please select one or more Sample file(s).", "Information"
            //        , MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
        }

        #endregion
    }
}
