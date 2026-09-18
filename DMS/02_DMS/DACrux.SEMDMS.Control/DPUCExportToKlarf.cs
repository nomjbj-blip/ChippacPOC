using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Utility;
using System.IO;
using DACrux.Base;
using DACrux.Data.Parser.Klarf;
using DACrux.Common.RO;
using DACrux.Framework.Base;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCExportToKlarf : DACruxCTLBasic01, ISendDefect
    {
        #region 멤버 변수

        public static readonly char SEPERATOR = '=';
        public static readonly string OPTION_FILENAME = "DACrux\\ExportToKlarf";

        public static readonly string IS_LOCAL = "IS_LOCAL"; // bool
        public static readonly string PATH_LOCAL = "PATH_LOCAL"; // string
        public static readonly string PATH_SERVER = "PATH_SERVER"; // string
        public static readonly string FILE_NAME = "FILE_NAME"; // string

        public static readonly string DEFECT_SAMPLING = "DEFECT_SAMPLING"; // bool 
        public static readonly string SAMPLE_COUNT = "SAMPLE_COUNT"; // int
        public static readonly string SELECT_DEFECT_TYPE = "SELECT_DEFECT_TYPE"; // string
        public static readonly string INCLUDE_CLUSTER = "INCLUDE_CLUSTER"; // string
        public static readonly string MERGE_LOT = "MERGE_LOT"; // string

        private enum Col
        {
            CHECK_BOX,
            STEP_SEQ,
            WAFER_SEQ,
            LOT_ID,
            WAFER_ID,
            SLOT_ID,
            STEP_ID,
            PRODUCT,
            SETUP_SEQ,
            GROUP_VAL,
            ALL_DEFECT,
            NEW_DEFECT,
            RANDOM_DEFECT,
            CLUSTER_DEFECT,
            CLUSTER_GROUP,
            RESULT_TIME,
            SAVE_COUNT,
            FILE_NAME
        }

        private SettingData _setting;
        Dictionary<int, Defect[]> defectArray;
        #endregion

        #region 생성자 및 Load, Close 이벤트

        public DPUCExportToKlarf()
        {
            InitializeComponent();

            DefectList = new DefectList();
            defectArray = new Dictionary<int, Defect[]>();
            rdoServer.Checked = true;
        }

        private void PopupExportToKlarf_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _setting = new SettingData(OPTION_FILENAME);

            ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);

            LoadUserSettings();

            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)Col.CHECK_BOX].Text = Boolean.TrueString;
        }

        void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUserSettings();
        }
        
        #endregion

        #region 사용자 정의 메서드

        private void  LoadUserSettings()
        {
            rdoLocal.Checked = _setting.GetValue<bool>(IS_LOCAL, false);
            txtLocalPath.Text = _setting.GetValue(PATH_LOCAL);
            txtServerPath.Text = _setting.GetValue(PATH_SERVER);
            txtFileName.Text = _setting.GetValue(FILE_NAME);

            rdoServer.Checked = !rdoLocal.Checked;

            chkDefectSampling.Checked = _setting.GetValue<bool>(DEFECT_SAMPLING, false);
            numSampleCount.Value = _setting.GetValue<int>(SAMPLE_COUNT);
            SetDefectType(_setting.GetValue(SELECT_DEFECT_TYPE));
            chkCluster.Checked = _setting.GetValue<bool>(INCLUDE_CLUSTER, true);
            chkMergeLot.Checked = _setting.GetValue<bool>(MERGE_LOT, true);
        }

        private void SaveUserSettings()
        {
            _setting.SetValue(IS_LOCAL, rdoLocal.Checked);
            _setting.SetValue(PATH_LOCAL, txtLocalPath.Text);
            _setting.SetValue(PATH_SERVER, txtServerPath.Text);
            _setting.SetValue(FILE_NAME, txtFileName.Text);

            _setting.SetValue(DEFECT_SAMPLING, chkDefectSampling.Checked);
            _setting.SetValue(SAMPLE_COUNT, numSampleCount.Value);
            _setting.SetValue(SELECT_DEFECT_TYPE, GetDefectType());
            _setting.SetValue(INCLUDE_CLUSTER, chkCluster.Checked);
            _setting.SetValue(MERGE_LOT, chkMergeLot.Checked);

            _setting.Save();
        }

        private string GetDefectType()
        {
            foreach (RadioButton button in new RadioButton[] { rdoAllDefect, rdoNewDefect })
            {
                if (button.Checked)
                    return button.Name;
            }

            return null;
        }

        public Defect[] GetSelectedDefect()
        {
            DefectList list = new DefectList();

            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return null;

            List<ParserKlarf> parserList = new List<ParserKlarf>();

            // 파일 저장
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                long stepSeq = System.Convert.ToInt64(dt.Rows[i][(int)Col.STEP_SEQ]);
                int count = System.Convert.ToInt32(dt.Rows[i][(int)Col.SAVE_COUNT]);

                list.AddRange(GetSampleDefectList(DefectList.GetDefectArray(stepSeq), count));
            }

            return list.ToArray();
        }
        
        private void SetDefectType(string name)
        {
            if (String.IsNullOrEmpty(name))
                return;

            foreach (RadioButton button in new RadioButton[] { rdoAllDefect, rdoNewDefect })
            {
                if (button.Name == name)
                {
                    button.Checked = true;
                    break;
                }
            }
        }

        private void Clear()
        {
            fpSpread1_Sheet1.RowCount = 0;
            fpSpread1_Sheet1.DataSource = null;
        }

        public void SetWaferInfo(long[] stepSeqArr)
        {
            if (stepSeqArr == null || stepSeqArr.Length == 0)
                return;

            RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
            DataTable dt = obj.GetWaferInfo(stepSeqArr);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                long stepSeq = long.Parse(dt.Rows[i][(int)Col.STEP_SEQ].ToString());
                defectArray[i] = DefectList.GetDefectArray(stepSeq);
            }

            fpSpread1_Sheet1.DataSource = dt;

            fpSpread1_Sheet1.ColumnHeader.Rows[0].Height = 30;

            fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)Col.CHECK_BOX].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            fpSpread1_Sheet1.Columns[(int)Col.CHECK_BOX].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            fpSpread1_Sheet1.Columns[(int)Col.CHECK_BOX].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;

            fpSpread1_Sheet1.Columns[(int)Col.STEP_SEQ].Visible = false;
            fpSpread1_Sheet1.Columns[(int)Col.WAFER_SEQ].Visible = false;
            fpSpread1_Sheet1.Columns[(int)Col.LOT_ID].Visible = false;
            fpSpread1_Sheet1.Columns[(int)Col.SLOT_ID].Visible = false;
            fpSpread1_Sheet1.Columns[(int)Col.STEP_ID].Visible = true;
            fpSpread1_Sheet1.Columns[(int)Col.PRODUCT].Visible = false;
            fpSpread1_Sheet1.Columns[(int)Col.SETUP_SEQ].Visible = false;
            fpSpread1_Sheet1.Columns[(int)Col.GROUP_VAL].Visible = false;

            fpSpread1_Sheet1.Columns[(int)Col.WAFER_ID].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            fpSpread1_Sheet1.Columns[(int)Col.RESULT_TIME].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;

            fpSpread1_Sheet1.Columns[(int)Col.SAVE_COUNT].Font = new Font(fpSpread1.Font, FontStyle.Bold);
            fpSpread1_Sheet1.Columns[(int)Col.FILE_NAME].Font = new Font(fpSpread1.Font, FontStyle.Bold);
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                long stepSeq = long.Parse(dt.Rows[i][(int)Col.STEP_SEQ].ToString());

                DefectList list = new DefectList();
                list.AddRange(defectArray[i]);

                DefectStat stat = list.GetDefectStat();

                dt.Rows[i][(int)Col.ALL_DEFECT] = list.Count;
                dt.Rows[i][(int)Col.NEW_DEFECT] = stat.New;
                dt.Rows[i][(int)Col.RANDOM_DEFECT] = stat.Random;
                dt.Rows[i][(int)Col.CLUSTER_DEFECT] = stat.Cluster;
                dt.Rows[i][(int)Col.CLUSTER_GROUP] = stat.ClusterGroup;
                dt.Rows[i][(int)Col.SAVE_COUNT] = dt.Rows[i][(int)Col.ALL_DEFECT];
            }

            DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
            fpSpread1_Sheet1.Columns[(int)Col.FILE_NAME].Width = 200;
            DACrux.Utility.FPSpreadUtil.SetDecimalLength(fpSpread1_Sheet1, 0, Col.ALL_DEFECT, Col.NEW_DEFECT, Col.RANDOM_DEFECT, Col.CLUSTER_DEFECT, Col.CLUSTER_GROUP, Col.SAVE_COUNT);

            for (int i = 1; i < fpSpread1_Sheet1.ColumnCount; i++)
            {
                fpSpread1_Sheet1.Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fpSpread1_Sheet1.Columns[i].Locked = true;
            }

            ChangeCellMerge();
            ChangeExportCount();
            ChangeExportFileName();
        }

        private void ChangeExportFileName()
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return;

            var mergeData = GetMergeData(dt, (int)Col.GROUP_VAL);

            // merge 된 단위로 이름 설정
            foreach (var data in mergeData)
            {
                dt.Rows[data.Item1][(int)Col.FILE_NAME] = String.Empty;

                for (int i = data.Item1; i < data.Item1 + data.Item2; i++)
                {
                    if (GetBoolValue(fpSpread1_Sheet1.Cells[i, (int)Col.CHECK_BOX]))
                        dt.Rows[data.Item1][(int)Col.FILE_NAME] = GetFileName(dt.Rows[data.Item1]);
                }
            }

            fpSpread1.Refresh();
            FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1, true, fpSpread1_Sheet1.RowCount, new int[] { (int)Col.FILE_NAME });
        }

        private void ChangeCellMerge()
        {
            if (!chkMergeLot.Checked)
            {
                fpSpread1_Sheet1.ClearSpanCells();
                return;
            }

            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return;

            var mergeData = GetMergeData(dt, (int)Col.GROUP_VAL);

            // GROUP_VAL 데이터를 기준으로 merge
            foreach (var data in mergeData)
            {
                fpSpread1_Sheet1.AddSpanCell(data.Item1, (int)Col.STEP_ID, data.Item2, 1);
                fpSpread1_Sheet1.AddSpanCell(data.Item1, (int)Col.FILE_NAME, data.Item2, 1);
            }
        }

        private void ChangeExportCount()
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (chkDefectSampling.Checked)
                    dt.Rows[i][(int)Col.SAVE_COUNT] = GetSampleDefectList(defectArray[i], (int)numSampleCount.Value).Length;
                else
                    dt.Rows[i][(int)Col.SAVE_COUNT] = GetSampleDefectList(defectArray[i], defectArray[i].Length).Length;
            }
        }

        public static bool GetBoolValue(FarPoint.Win.Spread.Cell cell)
        {
            string text = cell.Text;

            if (String.IsNullOrEmpty(text))
                return false;

            return text == Boolean.TrueString;
        }

        public List<Tuple<int, int>> GetMergeData(DataTable dt, int column)
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();

            if (dt == null || dt.Rows.Count == 0)
                return list;

            // Tuple<start index, row count>
            string prevVal = dt.Rows[0][column].ToString();
            int index = 0;

            for (int i = 1; i < dt.Rows.Count; i++)
            {
                string val = dt.Rows[i][column].ToString();

                if (!chkMergeLot.Checked || prevVal != val)
                {
                    list.Add(new Tuple<int, int>(index, i - index));

                    prevVal = val;
                    index = i;
                }
            }

            list.Add(new Tuple<int, int>(index, dt.Rows.Count - index));

            return list;
        }

        /// <summary>
        /// List로부터 지정된 갯수만큼 추출하여 가져옵니다.
        /// </summary>
        private Defect[] GetSampleDefectList(Defect[] defectArr, int sampleCount)
        {
            if (defectArr == null || defectArr.Length == 0)
                return new Defect[] {};

            List<Defect> list = new List<Defect>();
                        
            foreach (Defect defect in defectArr)
            {
                if (rdoNewDefect.Checked && defect.ADDER == 0)
                    continue;

                if (!chkRandom.Checked && defect.CLUSTERNUMBER == 0)
                    continue;

                if (!chkCluster.Checked && defect.CLUSTERNUMBER > 0)
                    continue;
                
                list.Add(defect);
            }

            return RandomSelect(list, sampleCount);
        }

        private Defect[] RandomSelect(List<Defect> defectList, int sampleCount)
        {
            if (defectList.Count <= sampleCount)
                return defectList.ToArray();

            System.Threading.Thread.Sleep(10);
            Random rnd = new Random(DateTime.Now.Millisecond);
            Defect[] arr = defectList.OrderBy(item => rnd.Next()).Skip(defectList.Count - sampleCount).ToArray<Defect>();

            return arr;
        }

        public void Save()
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("저장할 데이터가 없습니다.");
                return;
            }

            if ((rdoServer.Checked && String.IsNullOrWhiteSpace(txtServerPath.Text)) ||
                (rdoLocal.Checked && String.IsNullOrWhiteSpace(txtLocalPath.Text)))
            {
                MessageBox.Show("경로를 입력하세요.");
                return;
            }

            if (String.IsNullOrWhiteSpace(txtFileName.Text))
            {
                MessageBox.Show("파일명을 입력하세요.");
                return;
            }

            ChangeExportFileName();

            List<int> indexList = new List<int>();
            List<string> fileNames = new List<string>();

            // 파일명 중복 체크
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!GetBoolValue(fpSpread1_Sheet1.Cells[i, (int)Col.CHECK_BOX]))
                    continue;

                string fileName = dt.Rows[i][(int)Col.FILE_NAME].ToString();
                
                if (!String.IsNullOrWhiteSpace(fileName) && fileNames.Contains(fileName))
                {
                    MessageBox.Show("파일 이름이 중복됩니다. " + fileName);
                    return;
                }

                indexList.Add(i);
                fileNames.Add(fileName);
            }

            if (indexList.Count == 0)
            {
                MessageBox.Show("선택한 항목이 없습니다. ");
                return;
            }

            indexList.Clear();
            fileNames.Clear();

            if (MessageBox.Show("파일을 생성하시겠습니까?", "생성", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

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

            var mergeData = GetMergeData(dt, (int)Col.GROUP_VAL);
            int fileCount = 0;

            // KLARF 생성
            foreach (var data in mergeData)
            {
                ParserKlarf parser = null;

                for (int i = data.Item1; i < data.Item1 + data.Item2; i++)
                {
                    // 체크 여부 확인
                    if (!GetBoolValue(fpSpread1_Sheet1.Cells[i, (int)Col.CHECK_BOX]))
                        continue;

                    long stepSeq = System.Convert.ToInt64(dt.Rows[i][(int)Col.STEP_SEQ]);
                    int count = System.Convert.ToInt32(dt.Rows[i][(int)Col.SAVE_COUNT]);
                    DmsWaferDieInfo info = DmsCache.Instance[stepSeq];

                    // Create parser
                    ParserKlarf tmp = new ParserKlarf(DefectList.GetDefectArray(stepSeq), info, DmsCache.Instance.ClassLookup);

                    Defect[] sampleDefectArr = GetSampleDefectList(DefectList.GetDefectArray(stepSeq), count);
                    tmp.Wafers[0].SetDefect(sampleDefectArr);

                    // Setup Map의 Angle이 0이 아니면 Review 설비에서 작업이 가능하도록 원래 각도로 돌려서 데이터를 내려준다. 2019.11.25 Taihi,Kim.
                    if (info.StepInfo.Angle > 0)
                    {
                        tmp.RotateAndRecalculate(info.StepInfo.Angle, DieIndexSort.LowerLeftToCenter);
                        tmp.OrientationMarkLocation = ParserKlarf.AngleToOrientationMarkLocation(info.StepInfo.Angle);
                    }

                    // 1개 Parser에 Wafer 추가
                    if (parser == null)
                        parser = tmp;
                    else
                        parser.Wafers.Add(tmp.Wafers[0]);
                }

                if (parser != null)
                {
                    string fileName = Path.Combine(savePath, fpSpread1_Sheet1.Cells[data.Item1, (int)Col.FILE_NAME].Text);
                    parser.SaveFile(fileName);
                    fileCount++;

                    // 서버인 경우 Temp에서 파일 업로드
                    if (rdoServer.Checked)
                    {
                        using (FtpFolderBrowserDialog ftpDlg = CreateFtpDialog())
                        {
                            string serverFile = String.Format("{0}/{1}", ftpDlg.SelectedFullPath, Path.GetFileName(fileName));
                            ftpDlg.FtpInfo.SendFileNew(fileName, serverFile);
                        }
                    }
                }
            }

            // 서버인 경우 로컬 temp 폴더 삭제
            if (rdoServer.Checked)
                Directory.Delete(savePath, true);

            MessageBox.Show(String.Format("{0}개의 파일을 생성하였습니다.", fileCount));
        }

        private FtpFolderBrowserDialog CreateFtpDialog()
        {
            ComConfiguration obj = new ComConfiguration();
            var dic = obj.GetData("FTP_INFO");

            FtpFolderBrowserDialog ftpDlg = new FtpFolderBrowserDialog();
            ftpDlg.FtpInfo.Server = dic["SERVER"];
            ftpDlg.FtpInfo.Port = Int32.Parse(dic["PORT"]);
            ftpDlg.FtpInfo.UserID = dic["USER"];
            ftpDlg.FtpInfo.Password = dic["PASS"];
            ftpDlg.FtpInfo.ChmodValue = (short)DACrux.Base.Convert.intParse(dic["CHMOD_VALUE"], 0);
            ftpDlg.RootPath = dic["REVIEW_WORKING_PATH"];
            ftpDlg.SelectedPath = txtServerPath.Text;
            return ftpDlg;
        }

        #endregion

        #region 이벤트 처리 메서드

        private void ButtonShortcut_Click(object sender, EventArgs e)
        {
            string text = (sender as Button).Text;

            int index = text.IndexOf(SEPERATOR);

            txtFileName.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            txtFileName.Focus();
            Application.DoEvents();

            if (index < 0)
                SendKeys.SendWait(text);
            else
                SendKeys.SendWait(text.Substring(0, index).Replace("%", "{%}"));
        }

        private string GetFileName(DataRow row)
        {
            string fileName = txtFileName.Text;

            foreach (Button button in new Button[] { btnShortcutDeviceID, btnShortcutLotID, btnShortcutSlotID, btnShortcutStepID, btnShortcutWaferID, btnShortcutResultTime })
            {
                string shortcut = button.Text.Substring(0, 2);

                if (fileName.Contains(shortcut))
                {
                    object replace = null;

                    switch (shortcut)
                    {
                        case "%L": replace = row[(int)Col.LOT_ID]; break;
                        case "%W": replace = row[(int)Col.WAFER_ID]; break;
                        case "%D": replace = row[(int)Col.PRODUCT]; break;
                        case "%S": replace = row[(int)Col.STEP_ID]; break;
                        case "%R": replace = row[(int)Col.RESULT_TIME].ToString().Replace("-", String.Empty).Replace(":", String.Empty).Replace(" ", String.Empty); break;
                        case "%O": replace = String.Format("{0:00}", row[(int)Col.SLOT_ID]); break;
                    }

                    fileName = fileName.Replace(shortcut, replace.ToString());
                }
            }

            int dup = 0;
            DataTable dt = row.Table;

            for (int i = 0; i < dt.Rows.IndexOf(row); i++)
            {
                if (fileName == Path.GetFileNameWithoutExtension(dt.Rows[i][(int)Col.FILE_NAME].ToString()))
                    dup++;
            }

            // 확장자 없는 경우 붙여주기
            if (String.IsNullOrWhiteSpace(System.IO.Path.GetExtension(fileName)))
                fileName += String.Format(".{0:000}", dup);

            return fileName;
        }

        private void fpSpread1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader && e.Column == (int)Col.CHECK_BOX)
            {
                bool chk = (fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)Col.CHECK_BOX].Text == Boolean.TrueString);

                for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
                    fpSpread1_Sheet1.Cells[i, (int)Col.CHECK_BOX].Value = !chk;

                fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)Col.CHECK_BOX].Value = !chk;

                ChangeExportFileName();
            }
        }

        private void fpSpread1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == (int)Col.CHECK_BOX)
            {
                ChangeExportFileName();
            }
        }

        private void fpSpread1_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == (int)Col.CHECK_BOX)
            {
                ChangeExportFileName();
            }
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (rdoServer.Checked)
            {
                using (FtpFolderBrowserDialog ftpDlg = CreateFtpDialog())
                {
                    if (ftpDlg.ShowDialog() == DialogResult.OK)
                        txtServerPath.Text = ftpDlg.SelectedPath;
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

        private void chkDefectCount_CheckedChanged(object sender, EventArgs e)
        {
            numSampleCount.Enabled = chkDefectSampling.Checked;
            ChangeExportCount();
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            ChangeExportFileName();
        }

        private void DefectSample_CheckedChanged(object sender, EventArgs e)
        {
            ChangeExportCount();
        }

        private void numSampleCount_ValueChanged(object sender, EventArgs e)
        {
            ChangeExportCount();
        }

        private void chkMergeLot_CheckedChanged(object sender, EventArgs e)
        {
            ChangeCellMerge();
            ChangeExportFileName();
        }
        
        #endregion

        #region 프로퍼티

        public DefectList DefectList
        {
            get;
            private set;
        }
        
        #endregion
    }

    internal class ButtonEx : Button
    {
        public ButtonEx()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}
