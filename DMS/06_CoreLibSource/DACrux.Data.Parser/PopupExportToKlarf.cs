using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Utility;
using System.IO;

namespace DACrux.Data.Parser
{
    public partial class PopupExportToKlarf : Form
    {
        public static readonly char SEPERATOR = '=';
        public static readonly string OPTION_FILENAME = "DACrux\\ExportToKlarf";
        public static readonly string LOCAL_PATH = "LOCAL_PATH";
        public static readonly string SERVER_PATH = "SERVER_PATH";
        public static readonly string FILE_NAME = "FILE_NAME";

        private SettingData _setting;
        private FtpFolderBrowserDialog _ftpDlg = new FtpFolderBrowserDialog();

        public PopupExportToKlarf()
        {
            InitializeComponent();

            DefectList = new DefectList();
            WaferList = new List<DmsWaferDieInfo>();
            rdoServer.Checked = true;
            _setting = new SettingData(OPTION_FILENAME);
        }

        private void PopupExportToKlarf_Load(object sender, EventArgs e)
        {
            if (DefectList.Count == 0 || WaferList.Count == 0)
                return;

            foreach (DmsWaferDieInfo wafer in WaferList)
            {
                Klarf.ParserKlarf parser = new Klarf.ParserKlarf(DefectList.GetDefectArray(wafer.StepSeq), wafer, ClassLookup);

                PopupExportToKlarf_Sampling ctl = new PopupExportToKlarf_Sampling();
                ctl.StepSeq = wafer.StepSeq;
                ctl.Parser = parser;
                ctl.EnabledSampling = chkDataSampling.Checked;

                flowLayoutPanel1.Controls.Add(ctl);
            }

            txtLocalPath.Text = _setting.GetValue(LOCAL_PATH);
            txtServerPath.Text = _ftpDlg.SelectedPath = _setting.GetValue(SERVER_PATH);
            txtFileName.Text = _setting.GetValue(FILE_NAME);
            
            Text += String.Format(" - {0}", flowLayoutPanel1.Controls.Count);
        }

        /// <summary>
        /// List로부터 지정된 갯수만큼 추출하여 가져옵니다.
        /// </summary>
        public static Defect[] GetSampleList(Defect[] list, int sampleCount)
        {
            if (list == null || list.Length == 0 || sampleCount == 0)
                return null;

            if (list.Length == sampleCount)
                return list;

            Random rnd = new Random(DateTime.Now.Millisecond);
            Defect[] arr = list.OrderBy(item => rnd.Next()).Skip(list.Length - sampleCount).ToArray<Defect>();
            
            Array.Sort<Defect>(arr);
            return arr;
        }

        private void chkDataSampling_CheckedChanged(object sender, EventArgs e)
        {
            foreach (PopupExportToKlarf_Sampling ctl in flowLayoutPanel1.Controls)
            {
                ctl.EnabledSampling = chkDataSampling.Checked;
            }
        }

        private void ButtonShortcut_Click(object sender, EventArgs e)
        {
            string text = (sender as Button).Text;
            
            int index = text.IndexOf(SEPERATOR);

            txtFileName.Focus();
            Application.DoEvents();

            if (index < 0)
                SendKeys.SendWait(text);
            else
                SendKeys.SendWait(text.Substring(0, index).Replace("%","{%}"));
        }

        private string GetFileName(Klarf.ParserKlarf parser)
        {
            string fileName = txtFileName.Text;

            foreach (Button button in new Button[] { btnShortcutDeviceID, btnShortcutInspectionTime, btnShortcutLotID, btnShortcutSlotID, btnShortcutStepID, btnShortcutWaferID })
            {
                string shortcut = button.Text.Substring(0, 2);

                if (fileName.Contains(shortcut))
                {
                    string replace = null;

                    switch (shortcut)
                    {
                        case "%L": replace = parser.LotID; break;
                        case "%W": replace = parser.Wafers[0].WaferID; break;
                        case "%D": replace = parser.DeviceID; break;
                        case "%S": replace = parser.StepID; break;
                        case "%I": replace = parser.ResultTimestamp.ToString("yyyyMMddHHmmss"); break;
                        case "%O": replace = String.Format("{0:00}", parser.Wafers[0].Slot); break;
                    }

                    fileName = fileName.Replace(shortcut, replace);
                }
            }

            // 확장자 없는 경우 붙여주기
            if (String.IsNullOrWhiteSpace(System.IO.Path.GetExtension(fileName)))
                fileName += ".000";

            return fileName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count == 0)
            {
                MessageBox.Show("저장할 데이터가 없습니다.");
                return;
            }

            if (String.IsNullOrWhiteSpace(txtServerPath.Text))
            {
                MessageBox.Show("경로를 입력하세요.");
                return;
            }

            if (String.IsNullOrWhiteSpace(txtFileName.Text))
            {
                MessageBox.Show("파일명을 입력하세요.");
                return;
            }

            string[] fileNames = new string[flowLayoutPanel1.Controls.Count];

            // 파일명 중복 체크
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                PopupExportToKlarf_Sampling ctl = flowLayoutPanel1.Controls[i] as PopupExportToKlarf_Sampling;
                string fileName = GetFileName(ctl.Parser);

                if (Array.IndexOf<string>(fileNames, fileName) >= 0)
                {
                    MessageBox.Show("파일 이름이 중복됩니다. " + fileName);
                    return;
                }

                fileNames[i] = fileName;
            }

            // Validation 체크
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                PopupExportToKlarf_Sampling ctl = flowLayoutPanel1.Controls[i] as PopupExportToKlarf_Sampling;

                if (!ctl.CheckValid())
                    return;
            }

            string savePath;

            if (rdoLocal.Checked)
            {
                savePath = txtLocalPath.Text;
            }
            else
            {
                // server 인 경우 임시 폴더 생성
                savePath = Path.GetTempFileName();
                File.Delete(savePath);
                Directory.CreateDirectory(savePath);
            }

            // 파일 저장
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                PopupExportToKlarf_Sampling ctl = flowLayoutPanel1.Controls[i] as PopupExportToKlarf_Sampling;
                
                Defect[] sampleDefectArr = GetSampleList(DefectList.GetDefectArray(ctl.StepSeq), ctl.Count);
                ctl.Parser.Wafers[0].SetDefect(sampleDefectArr);

                // server 편의를 위해 경로도 포함
                fileNames[i] = Path.Combine(savePath, fileNames[i]);
                ctl.Parser.SaveFile(fileNames[i]);
            }

            if (rdoServer.Checked)
            {
                _ftpDlg.FtpInfo.SetTop();
                _ftpDlg.FtpInfo.SetCurrentDirectory(_ftpDlg.SelectedFullPath);
                
                // server 인 경우 ftp로 업로드 후 삭제
                for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                {
                    _ftpDlg.FtpInfo.SendFile(fileNames[i]);
                }

                // 로컬 temp 폴더 삭제
                Directory.Delete(savePath, true);
            }

            MessageBox.Show(String.Format("{0}개의 파일을 저장하였습니다.", flowLayoutPanel1.Controls.Count));

            _setting.SetValue(LOCAL_PATH, txtLocalPath.Text);
            _setting.SetValue(SERVER_PATH, txtServerPath.Text);
            _setting.SetValue(FILE_NAME, txtFileName.Text);
            _setting.Save();

            Close();
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (rdoServer.Checked)
            {
                _ftpDlg.SelectedPath = txtServerPath.Text;

                if (_ftpDlg.ShowDialog() == DialogResult.OK)
                {
                    txtServerPath.Text = _ftpDlg.SelectedPath;
                }
            }
            else
            {
                using (FolderBrowserDialog dlg = new FolderBrowserDialog())
                {
                    dlg.SelectedPath = txtServerPath.Text;

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        txtLocalPath.Text = dlg.SelectedPath;
                    }
                }
            }
        }

        private void rdoLocation_CheckedChanged(object sender, EventArgs e)
        {
            txtServerPath.Visible = rdoServer.Checked;
            txtLocalPath.Visible = rdoLocal.Checked;
        }

        public DefectList DefectList
        {
            get;
            private set;
        }

        public List<DmsWaferDieInfo> WaferList
        {
            get;
            private set;
        }

        public Dictionary<int, string> ClassLookup
        {
            get;
            set;
        }

        public ServerCommunicationFtp FtpInfo
        {
            get { return _ftpDlg.FtpInfo; }
        }

        public string FtpRoot
        {
            get { return _ftpDlg.RootPath; }
            set { _ftpDlg.RootPath = value; }
        }
    }

    internal class ButtonEx : Button
    {
        public ButtonEx()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}
