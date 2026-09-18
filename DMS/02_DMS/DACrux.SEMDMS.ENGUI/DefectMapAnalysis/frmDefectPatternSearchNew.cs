using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.Base;
using DACrux.Base;
using DACrux.SEMDMS.Interface;
using DACrux.SEMDMS.Control;
using DACrux.Utility;
using DACrux.Map;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectPatternSearchNew : DACruxUXBasicDefectLink, iSEMControl, ISendDefect, IExportExcel, iFileControl
    {
        #region 멤버 변수

        enum Col { STEP_SEQ, ID, CLASS, CLUSTER };
        enum Wafer { STEP_SEQ, MATCH_COUNT, MATCHING_RATE, LOT_ID, WAFER_ID }

        public const int DEFAULT_TOLERANCE = 100;
        public const int DEFAULT_MATCHING_RATE = 80;

        DataSet m_dsDefect;
        long[] m_stepSeqArr;
        bool m_isStop;

        #endregion

        #region 생성자 및 Load/Closing 이벤트

        public frmDefectPatternSearchNew()
        {
            InitializeComponent();
        }

        private void frmDefectPatternSearchNew_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            if (WaferList != null && WaferList.Length > 0)
            {
                DACrux.Base.DPWafer[] oWafer = new Base.DPWafer[WaferList.Length];

                for (int iWafer = 0; iWafer < WaferList.Length; iWafer++)
                {
                    oWafer[iWafer].StepSeq = WaferList[iWafer];
                }

                DrawWafer(oWafer);
            }
            else if (ExistsDefectArray)
            {
                DrawWafer(DefectList);
            }

            UpdateProgress();

            SettingData setting = new SettingData(GetType());
            numTolerance.Value = setting.GetValue<int>(numTolerance.Name, DEFAULT_TOLERANCE);
            numMatchingRate.Value = setting.GetValue<int>(numMatchingRate.Name, DEFAULT_MATCHING_RATE);
        }

        private void frmDefectPatternSearchNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingData setting = new SettingData(GetType());
            setting.SetValue(numTolerance.Name, numTolerance.Value);
            setting.SetValue(numMatchingRate.Name, numMatchingRate.Value);
            setting.Save();
        }

        #endregion

        #region 사용자 정의 메서드

        public void DrawWafer(DPWafer[] waferArr)
        {
            if (waferArr == null || waferArr.Length == 0)
                return;

            long[] stepSeqArr = new long[waferArr.Length];

                for (int iw = 0; iw < waferArr.Length; iw++)
                    stepSeqArr[iw] = DACrux.Base.Convert.longParse(waferArr[iw].StepSeq);

            // Source Wafer
            if (tabControl1.SelectedTab == tabSource)
            {
                DrawWafer(sourceMap, stepSeqArr);
            }
            // Target Wafers
            else if (tabControl1.SelectedTab == tabTarget)
            {
                m_stepSeqArr = stepSeqArr;
                AddSelectedWaferList(stepSeqArr);
            }
        }

        public void DrawWafer(DefectList defectList)
        {
            DrawWafer(sourceMap, defectList);
            fpSelectedDefect_Sheet1.DataSource = null;
        }

        private void DrawWafer(DefectMap defectMap, long[] stepSeqArr)
        {
            try
            {
                StatusMessage("Defect 데이터를 조회 중입니다.");

                RO.DefectMapAnalysis oDMapAnalysis = new RO.DefectMapAnalysis();
                DefectList defectList = new DefectList();
                defectList.AddRange(oDMapAnalysis.GetDefectMapViewer_DefectArray(stepSeqArr));

                DrawWafer(defectMap, defectList);
            }
            finally
            {
                StatusMessage(null);
                this.Cursor = Cursors.Default;
            }
        }

        private void DrawWafer(DefectMap defectMap, DefectList defectList)
        {
            try
            {
                defectMap.DefectClear();

                if (defectList == null || defectList.Count == 0)
                    return;

                defectMap.Defects.AddRange(defectList);

                long[] lStepSeq = defectList.GetStepSeqArray();

                // 이전 조회한 DataSet이 있는 경우 Dispose
                if (m_dsDefect != null)
                    m_dsDefect.Dispose();

                StatusMessage("기준 정보를 조회 중입니다.");

                RO.DefectMapAnalysis oDMapAnalysis = new RO.DefectMapAnalysis();
                m_dsDefect = oDMapAnalysis.GetDefectMapViewer_Info(lStepSeq);

                if (!m_dsDefect.Tables.Contains("STEP_INFO") || m_dsDefect.Tables["STEP_INFO"].Rows.Count <= 0)
                    throw new Exception("정의된 Step 정보가 없습니다.");

                if (!m_dsDefect.Tables.Contains("SETUP_INFO"))
                    throw new Exception("정의된 Setup 정보가 없습니다.");

                string[] strSetupArr = new string[m_dsDefect.Tables["STEP_INFO"].Rows.Count];
                string[] strTestArr = new string[m_dsDefect.Tables["STEP_INFO"].Rows.Count];

                for (int i = 0; i < m_dsDefect.Tables["STEP_INFO"].Rows.Count; i++)
                {
                    strSetupArr[i] = m_dsDefect.Tables["STEP_INFO"].Rows[i]["SETUP_SEQ"].ToString();
                    strTestArr[i] = m_dsDefect.Tables["STEP_INFO"].Rows[i]["TEST"].ToString();
                }

                DataTable dtWaferMap = oDMapAnalysis.GetDefectMapViewer_Map(lStepSeq, strSetupArr, strTestArr);

                StatusMessage("Map을 그리고 있습니다.");

                defectMap.Rotate(0);
                defectMap.WaferSize = DACrux.Base.Convert.doubleParse(m_dsDefect.Tables["SETUP_INFO"].Rows[0]["WAFER_SIZE"].ToString());
                defectMap.NotchType = DACrux.Base.Notch.Notch; // dsDefect.Tables["SETUP_INFO"].Rows[0]["NOTCH_TYPE"].ToString().Equals("F") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;
                defectMap.AngleOffSet = 0;
                defectMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                defectMap.NotchAngle = 0;// DOWN으로 저장하여 보여주므로 0으로 설정 DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ANGLE"].ToString());

                defectMap.DieSizeX = DACrux.Base.Convert.doubleParse(m_dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_X"].ToString());
                defectMap.DieSizeY = DACrux.Base.Convert.doubleParse(m_dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_Y"].ToString());
                defectMap.OriginIndexX = DACrux.Base.Convert.intParse(m_dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_X"].ToString());
                defectMap.OriginIndexY = DACrux.Base.Convert.intParse(m_dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_Y"].ToString());
                defectMap.OriginX = DACrux.Base.Convert.doubleParse(m_dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_X"].ToString());
                defectMap.OriginY = DACrux.Base.Convert.doubleParse(m_dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_Y"].ToString());
                
                defectMap.DieCalculation(true);
                defectMap.DrawDefects = "ALL";
                defectMap.DieClear();

                UserConfiguration.SetVirtualDie(defectMap);

                foreach (DataRow dr in dtWaferMap.Rows)
                {
                    defectMap.AddDie(new DACrux.Base.Die(
                        DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString()),
                        DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString()),
                        DACrux.Base.Convert.intParse(dr["TEST"].ToString()),
                        1
                        ));
                }

                defectMap.SetInfomation(DefectMapDraw.GetMapDescription(defectMap.Defects));

                UserConfiguration.SetMapInformation(defectMap, m_dsDefect);
                UserConfiguration.SetMapColor(defectMap);

                defectMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                defectMap.DefectSelectMode();

                toolTotalDie.Text = String.Format("Total Dies : {0:N0}", defectMap.Dies.Count);
                toolDefectCount.Text = String.Format("Total Defects : {0:N0}", defectMap.Defects.Count);
            }
            finally
            {
                StatusMessage(null);
            }
        }

        private void AddSelectedWaferList(long[] stepSeqArr)
        {
            if (stepSeqArr == null || stepSeqArr.Length == 0)
                return;

            UpdateProgress();

            RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
            DataTable dt = obj.GetPatternSearch_WaferList(stepSeqArr);
            
            fpSelectedWafer_Sheet1.DataSource = dt;
            FPSpreadUtil.SetAutoColumnWidth(fpSelectedWafer_Sheet1);
            FPSpreadUtil.SetDecimalLength(fpSelectedWafer_Sheet1, 0, Wafer.STEP_SEQ, Wafer.MATCH_COUNT);
            FPSpreadUtil.SetDecimalLength(fpSelectedWafer_Sheet1, 1, Wafer.MATCHING_RATE);
            fpSelectedWafer_Sheet1.Columns[(int)Wafer.STEP_SEQ].Visible = false;

            fpMatchedWafer_Sheet1.DataSource = dt.Clone();
            FPSpreadUtil.SetDecimalLength(fpMatchedWafer_Sheet1, 0, Wafer.STEP_SEQ, Wafer.MATCH_COUNT);
            FPSpreadUtil.SetDecimalLength(fpMatchedWafer_Sheet1, 1, Wafer.MATCHING_RATE);
            fpMatchedWafer_Sheet1.Columns[(int)Wafer.STEP_SEQ].Visible = false;

            for (int i = 0; i < fpSelectedWafer_Sheet1.ColumnCount; i++)
                fpMatchedWafer_Sheet1.Columns[i].Width = fpSelectedWafer_Sheet1.Columns[i].Width;
        }

        private DataTable ConvertDefects(Defect[] oDefect)
        {
            if (oDefect == null)
                return null;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SEQ", typeof(decimal)));
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("CLASS", typeof(int)));
            dt.Columns.Add(new DataColumn("CLUSTER", typeof(int)));
            dt.Columns.Add(new DataColumn("XINDEX", typeof(int)));
            dt.Columns.Add(new DataColumn("YINDEX", typeof(int)));
            dt.Columns.Add(new DataColumn("XREL", typeof(double)));
            dt.Columns.Add(new DataColumn("YREL", typeof(double)));
            dt.Columns.Add(new DataColumn("X", typeof(double)));
            dt.Columns.Add(new DataColumn("Y", typeof(double)));
            dt.Columns.Add(new DataColumn("XSIZE", typeof(double)));
            dt.Columns.Add(new DataColumn("YSIZE", typeof(double)));

            foreach (Defect defect in oDefect)
            {
                DataRow dr = dt.NewRow();
                dr["SEQ"] = defect.STEP_SEQ;
                dr["ID"] = defect.DEFECTID;
                dr["CLASS"] = defect.CLASSNUMBER;
                dr["CLUSTER"] = defect.CLUSTERNUMBER;
                dr["XINDEX"] = defect.XINDEX;
                dr["YINDEX"] = defect.YINDEX;
                dr["XREL"] = defect.XREL;
                dr["YREL"] = defect.YREL;
                dr["X"] = defect.X;
                dr["Y"] = defect.Y;
                dr["XSIZE"] = defect.XSIZE;
                dr["YSIZE"] = defect.YSIZE;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private List<int> GetRowIndexList(FarPoint.Win.Spread.Model.CellRange[] rangeArr)
        {
            List<int> list = new List<int>();

            foreach (FarPoint.Win.Spread.Model.CellRange range in rangeArr)
            {
                for (int i = 0; i < range.RowCount; i++)
                {
                    int val = range.Row + i;
                    int idx = list.BinarySearch(val);

                    if (idx < 0)
                        list.Insert(~idx, val);
                }
            }

            return list;
        }

        private void DeleteSelectedDefect()
        {
            List<int> list = GetRowIndexList(fpSelectedDefect_Sheet1.GetSelections());

            if (list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    int idx = list[i];
                    long stepSeq = DACrux.Base.Convert.longParse(fpSelectedDefect_Sheet1.Cells[idx, (int)Col.STEP_SEQ].Text);
                    int defectId = DACrux.Base.Convert.intParse(fpSelectedDefect_Sheet1.Cells[idx, (int)Col.ID].Text);

                    sourceMap.SelectedDefect.RemoveAt(sourceMap.SelectedDefect.BinarySearch(stepSeq, defectId));
                    fpSelectedDefect_Sheet1.Rows.Remove(idx, 1);
                }
            }

            sourceMap.Redraw();
            UpdateSelectedDefects(sourceMap.SelectedDefect.Count);
        }

        private void UpdateSelectedDefects(int count)
        {
            toolSelectedDefect.Text = String.Format("Selected Defects : {0:N0}", count);
            txtDefectCount.Text = String.Format("{0:N0}", count);
        }
                
        private List<RectangleF> GetDefectRectangles()
        {
            List<RectangleF> list = new List<RectangleF>();

            int tolerance = (int)numTolerance.Value;

            foreach (Defect defect in sourceMap.SelectedDefect)
            {
                RectangleF rect = new RectangleF(
                    (float)(defect.X - defect.XSIZE * 0.5 - tolerance),
                    (float)(defect.Y - defect.YSIZE * 0.5 - tolerance),
                    (float)(defect.XSIZE + 2 * tolerance),
                    (float)(defect.YSIZE + 2 * tolerance));

                list.Add(rect);
            }

            return list;
        }

        private bool IsVisible(List<RectangleF> rectList, PointF point)
        {
            foreach (RectangleF rect in rectList)
            {
                if (rect.Contains(point))
                    return true;
            }

            return false;
        }

        private void UpdateProgress(int total = 0, int curr = 0)
        {
            if (total == 0)
            {
                lblProgress.Text = String.Empty;
                progressBar1.Maximum = progressBar1.Value = 0;
            }
            else
            {
                lblProgress.Text = String.Format("Calculating... {0:N0}/{1:N0}", curr, total);
                progressBar1.Maximum = total;
                progressBar1.Value = curr;
            }

            Application.DoEvents();
        }

        public Defect[] GetSelectedDefect()
        {
            if (tabControl1.SelectedTab == tabSource)
            {
                if (sourceMap.SelectedDefect.Count > 0)
                    return sourceMap.SelectedDefect.ToArray();
                else if (sourceMap.Defects.Count > 0)
                    return sourceMap.Defects.ToArray();
                else
                    return null;
            }
            else
            {
                if (targetMap.Defects.Count == 0)
                    return null;

                return targetMap.Defects.ToArray();
            }
        }

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();

            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet("Source Map");
            sheet.Add(sourceMap);
            e.SheetList.Add(sheet);

            sheet = new ExcelSheet("Selected Wafer List");
            sheet.Add(fpSelectedWafer);
            e.SheetList.Add(sheet);

            sheet = new ExcelSheet("Matched Wafer List");
            sheet.Add(fpMatchedWafer);
            e.SheetList.Add(sheet);

            ExcelExportManager.Export(e);
        }

        #endregion

        #region 이벤트 처리 메서드

        private void map_OnSelectedDefect(object sender, DACrux.Base.Defect[] oDefect)
        {
            UpdateSelectedDefects(oDefect.Length);

            fpSelectedDefect_Sheet1.DataSource = ConvertDefects(oDefect);
            FPSpreadUtil.SetDecimalLength(fpSelectedDefect_Sheet1, 0, Col.STEP_SEQ);
            FPSpreadUtil.SetAutoColumnWidth(fpSelectedDefect_Sheet1);
            fpSelectedDefect_Sheet1.Columns[(int)Col.STEP_SEQ].Visible = false;
        }

        private void map_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            toolWaferDieLocation.Text = string.Format("X : {0:N0}, Y : {1:N0}", NewDie.IndexX, NewDie.IndexY);
        }

        private void fpSelectedDefect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedDefect();
                e.Handled = true;
            }
        }

        private void fpMatchedWafer_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Application.DoEvents();

                List<int> list = GetRowIndexList(fpMatchedWafer_Sheet1.GetSelections());

                if (list == null || list.Count == 0)
                    return;

                long[] stepSeqArr = new long[list.Count];

                for (int i = 0; i < list.Count; i++)
                    stepSeqArr[i] = long.Parse(fpMatchedWafer_Sheet1.Cells[list[i], (int)Wafer.STEP_SEQ].Text);

                DrawWafer(targetMap, stepSeqArr);
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            DeleteSelectedDefect();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (m_stepSeqArr == null || m_stepSeqArr.Length == 0)
                return;

            try
            {
                btnSearch.Enabled = false;
                btnStop.Enabled = true;
                Application.DoEvents();

                fpMatchedWafer_Sheet1.RowCount = 0;

                StatusMessage("Pattern Search를 시작합니다.");
                RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();

                List<RectangleF> rectList = GetDefectRectangles();

                for (int r = 0; r < fpSelectedWafer_Sheet1.RowCount; r++)
                {
                    if (m_isStop)
                        break;

                    UpdateProgress(fpSelectedWafer_Sheet1.RowCount, r + 1);

                    long stepSeq = long.Parse(fpSelectedWafer_Sheet1.Cells[r, (int)Wafer.STEP_SEQ].Text);
                    int matchCount = 0;

                    List<PointF> list = obj.GetPatternSearch_XYList(stepSeq);

                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (IsVisible(rectList, list[i]))
                                matchCount++;
                        }
                    }

                    float matchingRate = (float)Math.Min(Math.Round(100 * matchCount / (double)sourceMap.SelectedDefect.Count, 1), 100f);

                    // 결과 업데이트
                    fpSelectedWafer_Sheet1.Cells[r, (int)Wafer.MATCH_COUNT].Value = matchCount;
                    fpSelectedWafer_Sheet1.Cells[r, (int)Wafer.MATCHING_RATE].Value = matchingRate;

                    if (matchingRate >= (float)numMatchingRate.Value)
                    {
                        fpMatchedWafer_Sheet1.RowCount++;

                        for (int c = 0; c < fpSelectedWafer_Sheet1.ColumnCount; c++)
                            fpMatchedWafer_Sheet1.Cells[fpMatchedWafer_Sheet1.RowCount - 1, c].Value = fpSelectedWafer_Sheet1.Cells[r, c].Value;
                    }

                    System.Threading.Thread.Sleep(20);
                }

                MessageBox.Show(String.Format("Pattern Search를 완료하였습니다.\n대상 Wafer : {0}, 찾은 Wafer : {1}", fpSelectedWafer_Sheet1.RowCount, fpMatchedWafer_Sheet1.RowCount));
            }
            finally
            {
                StatusMessage(null);
                btnSearch.Enabled = true;
                btnStop.Enabled = false;
                m_isStop = false;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_isStop = true;
        }

        #endregion
    }
}
