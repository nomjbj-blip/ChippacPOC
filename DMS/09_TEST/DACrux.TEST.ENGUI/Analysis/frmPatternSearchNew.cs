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
using DACrux.TEST.Interface;
using DACrux.TEST.Control;
using DACrux.Utility;
using DACrux.Map;

namespace DACrux.TEST.ENGUI
{
    public partial class frmPatternSearchNew : DACruxUXBasic01, IExportExcel, iTESTControl
    {
        #region 멤버 변수

        enum Col { XINDEX, YINDEX, BIN };
        enum Wafer { WAFER_SEQ, MATCH_COUNT, MATCHING_RATE }

        public const int DEFAULT_TOLERANCE = 100;
        public const int DEFAULT_MATCHING_RATE = 80;

        DataSet m_dsDefect;
        long[] m_stepSeqArr;
        bool m_isStop;

        #endregion

        #region 생성자 및 Load/Closing 이벤트

        public frmPatternSearchNew()
        {
            InitializeComponent();
        }

        private void frmPatternSearchNew_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            if (WaferList != null && WaferList.Length > 0)
            {
                DACrux.Base.TPWafer[] oWafer = new Base.TPWafer[WaferList.Length];

                for (int iWafer = 0; iWafer < WaferList.Length; iWafer++)
                {
                    oWafer[iWafer].WaferSeq = WaferList[iWafer];
                }

                DrawWafer(oWafer);
            }

            UpdateProgress();

            SettingData setting = new SettingData(GetType());
            numMatchingRate.Value = setting.GetValue<int>(numMatchingRate.Name, DEFAULT_MATCHING_RATE);
        }

        private void frmPatternSearchNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingData setting = new SettingData(GetType());
            setting.SetValue(numMatchingRate.Name, numMatchingRate.Value);
            setting.Save();
        }

        #endregion

        public void DrawWafer(TPWafer[] waferArr)
        {
            if (waferArr == null || waferArr.Length == 0)
                return;

            long[] waferSeqArr = new long[waferArr.Length];

                for (int iw = 0; iw < waferArr.Length; iw++)
                    waferSeqArr[iw] = DACrux.Base.Convert.longParse(waferArr[iw].WaferSeq);

            // Source Wafer
            if (tabControl1.SelectedTab == tabSource)
            {
                sourceMap.ResetSelectedDie();
                DrawWafer(sourceMap, waferSeqArr);
                sourceMap.SetZoneSelect(ZoneSelectMode.Rectangle);
            }
            // Target Wafers
            else if (tabControl1.SelectedTab == tabTarget)
            {
                m_stepSeqArr = waferSeqArr;
                AddSelectedWaferList(waferSeqArr);
            }
        }

        private void DrawWafer(WaferMap waferMap, long[] waferSeqArr)
        {
            try
            {
                StatusMessage("데이터를 조회 중입니다.");

                if (m_dsDefect != null)
                    m_dsDefect.Dispose();

                RO.ProbeMapAnalysis obj = new RO.ProbeMapAnalysis();
                m_dsDefect = obj.SelectWaferMapDrawData(waferSeqArr);

                DataTable binTable = m_dsDefect.Tables["BINSUM"];

                for (int i = 0; i < binTable.Rows.Count; i++)
                {
                    if (binTable.Rows[i]["COLOR"].ToString() != "#FFFFFF")
                    {
                        Color tmpColor = ColorTranslator.FromHtml(m_dsDefect.Tables["BINSUM"].Rows[i]["COLOR"].ToString());
                        waferMap.SetColor(DACrux.Base.Convert.intParse(m_dsDefect.Tables["BINSUM"].Rows[i]["BIN"].ToString()), tmpColor);
                    }
                }

                DataTable recipeTable = m_dsDefect.Tables["RECIPE"];

                waferMap.WaferSize = DACrux.Base.Convert.doubleParse(recipeTable.Rows[0]["WAFER_SIZE"].ToString());
                waferMap.DieSizeX = DACrux.Base.Convert.doubleParse(recipeTable.Rows[0]["CHIP_SIZE_X"].ToString());
                waferMap.DieSizeY = DACrux.Base.Convert.doubleParse(recipeTable.Rows[0]["CHIP_SIZE_Y"].ToString());
                waferMap.OriginIndexX = DACrux.Base.Convert.intParse(recipeTable.Rows[0]["ORIGIN_INDEX_X"].ToString());
                waferMap.OriginIndexY = DACrux.Base.Convert.intParse(recipeTable.Rows[0]["ORIGIN_INDEX_Y"].ToString());
                waferMap.OriginX = DACrux.Base.Convert.doubleParse(recipeTable.Rows[0]["ORIGIN_MICRO_X"].ToString());
                waferMap.OriginY = DACrux.Base.Convert.doubleParse(recipeTable.Rows[0]["ORIGIN_MICRO_Y"].ToString());
                waferMap.FirstDieX = DACrux.Base.Convert.intParse(recipeTable.Rows[0]["FIRST_INDEX_X"].ToString());
                waferMap.FirstDieY = DACrux.Base.Convert.intParse(recipeTable.Rows[0]["FIRST_INDEX_Y"].ToString());
                waferMap.NotchAngle = DACrux.Base.Convert.intParse(recipeTable.Rows[0]["ANGLE"].ToString());
                waferMap.EdgeSize = DACrux.Base.Convert.doubleParse(recipeTable.Rows[0]["EDGE_SIZE"].ToString());
                waferMap.NotchType = DACrux.Base.Notch.Notch; //ds.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                int iXYDir = DACrux.Base.Convert.intParse(recipeTable.Rows[0]["XY_DIRECTION"].ToString());
                
                switch (iXYDir)
                {
                    case 0:
                        waferMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        waferMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        waferMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        waferMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }

                waferMap.DataSource = m_dsDefect.Tables["MAPDATA"];

                DataTable infoTable = m_dsDefect.Tables["WAFER_INFO"];

                string[] strInfo = new string[5];
                strInfo[0] = string.Format("DEVICE  :{0}", infoTable.Rows[0]["PRODUCT"].ToString());
                strInfo[1] = string.Format("PROGRAM :{0}", infoTable.Rows[0]["PROGRAM"].ToString());
                strInfo[2] = string.Format("WAFER_ID:{0}-{1}", infoTable.Rows[0]["LOT_ID"].ToString(), infoTable.Rows[0]["WAFER_ID"].ToString());
                strInfo[3] = string.Format("YIELD   :{0:0.0#} %", infoTable.Rows[0]["YIELD"].ToString());
                strInfo[4] = string.Format("TESTER  :{0}", infoTable.Rows[0]["TESTER"].ToString());

                waferMap.SetInfomation(strInfo);
                waferMap.WaferDrawMode = DACrux.Map.MapMode.Fit;

                toolTotalDie.Text = String.Format("Total Dies : {0:N0}", waferMap.Dies.Count);
            }
            finally
            {
                StatusMessage(null);
            }
        }

        private void AddSelectedWaferList(long[] waferSeqArr)
        {
            if (waferSeqArr == null || waferSeqArr.Length == 0)
                return;

            UpdateProgress();

            RO.ProbeMapAnalysis obj = new RO.ProbeMapAnalysis();
            DataTable dt = obj.GetPatternSearch_WaferList(waferSeqArr);
            
            fpSelectedWafer_Sheet1.DataSource = dt;
            FPSpreadUtil.SetAutoColumnWidth(fpSelectedWafer_Sheet1);
            FPSpreadUtil.SetDecimalLength(fpSelectedWafer_Sheet1, 0, Wafer.WAFER_SEQ, Wafer.MATCH_COUNT);
            FPSpreadUtil.SetDecimalLength(fpSelectedWafer_Sheet1, 1, Wafer.MATCHING_RATE);
            fpSelectedWafer_Sheet1.Columns[(int)Wafer.WAFER_SEQ].Visible = false;

            fpMatchedWafer_Sheet1.DataSource = dt.Clone();
            FPSpreadUtil.SetDecimalLength(fpMatchedWafer_Sheet1, 0, Wafer.WAFER_SEQ, Wafer.MATCH_COUNT);
            FPSpreadUtil.SetDecimalLength(fpMatchedWafer_Sheet1, 1, Wafer.MATCHING_RATE);
            fpMatchedWafer_Sheet1.Columns[(int)Wafer.WAFER_SEQ].Visible = false;

            for (int i = 0; i < fpSelectedWafer_Sheet1.ColumnCount; i++)
                fpMatchedWafer_Sheet1.Columns[i].Width = fpSelectedWafer_Sheet1.Columns[i].Width;
        }

        private DataTable ConvertDies(List<Point> dieList)
        {
            if (dieList == null)
                return null;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("XINDEX", typeof(int)));
            dt.Columns.Add(new DataColumn("YINDEX", typeof(int)));
            dt.Columns.Add(new DataColumn("BIN", typeof(int)));

            foreach (Point point in dieList)
            {
                DataRow dr = dt.NewRow();
                dr["XINDEX"] = point.X;
                dr["YINDEX"] = point.Y;
                dr["BIN"] = sourceMap.Dies[sourceMap.Dies.IndexOf(point.X, point.Y)].BinNumber;
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

        private void DeleteSelectedDie()
        {
            List<int> list = GetRowIndexList(fpSelectedDie_Sheet1.GetSelections());

            if (list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    int idx = list[i];
                    int x = DACrux.Base.Convert.intParse(fpSelectedDie_Sheet1.Cells[idx, (int)Col.XINDEX].Text);
                    int y = DACrux.Base.Convert.intParse(fpSelectedDie_Sheet1.Cells[idx, (int)Col.YINDEX].Text);

                    int index = sourceMap.SelectedDies.IndexOf(new Point(x, y));

                    if (index >= 0)
                    {
                        sourceMap.SelectedDies.RemoveAt(index);
                        fpSelectedDie_Sheet1.Rows.Remove(idx, 1);
                    }
                }
            }

            sourceMap.Redraw();
            UpdateSelectedDies(sourceMap.SelectedDies.Count);
        }

        private void UpdateSelectedDies(int count)
        {
            toolSelectedDefect.Text = String.Format("Selected Dies : {0:N0}", count);
            txtDefectCount.Text = String.Format("{0:N0}", count);
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

        private List<int> GetSelectedDie()
        {
            if (sourceMap.SelectedDies == null || sourceMap.SelectedDies.Count == 0)
                return null;

            List<int> list = new List<int>();

            for (int i = 0; i < sourceMap.SelectedDies.Count; i++)
            {
                Point pt = sourceMap.SelectedDies[i];
                list.Add(Die.XYToInt(pt.X, pt.Y));
            }

            list.Sort();
            return list;
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

        private void sourceMap_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            toolWaferDieLocation.Text = string.Format("X : {0:N0}, Y : {1:N0}", NewDie.IndexX, NewDie.IndexY);
        }

        private void sourceMap_OnSelectDies(object sender, List<Point> selectedDies)
        {
            UpdateSelectedDies(selectedDies.Count);

            fpSelectedDie_Sheet1.DataSource = ConvertDies(selectedDies);
            FPSpreadUtil.SetDecimalLength(fpSelectedDie_Sheet1, 0, Col.XINDEX, Col.YINDEX);
            FPSpreadUtil.SetAutoColumnWidth(fpSelectedDie_Sheet1);
        }

        private void fpSelectedDefect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedDie();
                e.Handled = true;
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            DeleteSelectedDie();
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
                RO.ProbeMapAnalysis obj = new RO.ProbeMapAnalysis();

                for (int r = 0; r < fpSelectedWafer_Sheet1.RowCount; r++)
                {
                    if (m_isStop)
                        break;

                    List<int> dieList = GetSelectedDie();
                    if (dieList == null || dieList.Count <= 0)
                        continue;

                    UpdateProgress(fpSelectedWafer_Sheet1.RowCount, r + 1);

                    long waferSeq = long.Parse(fpSelectedWafer_Sheet1.Cells[r, (int)Wafer.WAFER_SEQ].Text);
                    int matchCount = 0;

                    List<int> list = obj.GetPatternSearch_DieList(waferSeq);

                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (dieList.BinarySearch(list[i]) >= 0)
                                matchCount++;
                        }
                    }

                    float matchingRate = (float)Math.Min(Math.Round(100 * matchCount / (double)dieList.Count, 1), 100f);

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
                    stepSeqArr[i] = long.Parse(fpMatchedWafer_Sheet1.Cells[list[i], (int)Wafer.WAFER_SEQ].Text);

                DrawWafer(targetMap, stepSeqArr);
            }
        }

        private void btnSendTo_Click(object sender, EventArgs e)
        {
            if (fpMatchedWafer_Sheet1.RowCount == 0)
                return;

            TPWafer[] waferArr = new TPWafer[fpMatchedWafer_Sheet1.RowCount];

            for (int i = 0; i < fpMatchedWafer_Sheet1.RowCount; i++)
            {
                waferArr[i] = new TPWafer();
                waferArr[i].WaferSeq = fpMatchedWafer_Sheet1.Cells[i, (int)Wafer.WAFER_SEQ].Text;
            }

            ShowForm("MNU_BIN_MAP_GALLERY", waferArr);
        }
    }
}
