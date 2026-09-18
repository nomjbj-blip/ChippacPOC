using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;
using DACrux.Utility;
using System.Collections;
using DACrux.Common.RO;

namespace DACrux.TEST.ENGUI
{
    public partial class frmPatternSearch : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl
    {

        private string m_selBin = string.Empty;
        bool changeFindMap = false;
        DataSet m_ds = null;

        public frmPatternSearch()
        {
            InitializeComponent();
        }

        public struct BinColor
        {
            public int bin;
            public Color col;
        }
        ArrayList alBinColor = null;

        #region MakeTragetMap
        //void MakeTragetMap()
        //{
        //    string[] Product = {
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M",
        //                            "S7SP800X01-51M"
        //                        };
        //    string[] Program = {
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1",
        //                            "K39VAGSL13_PT1"
        //                        };
        //    string[] WaferSeqs = {
        //                              "49883.00",
        //                              "49859.00",
        //                              "49860.00",
        //                              "49862.00",
        //                              "49865.00",
        //                              "49864.00",
        //                              "49863.00",
        //                              "49861.00",
        //                              "49866.00",
        //                              "49875.00",
        //                              "49874.00",
        //                              "49873.00",
        //                              "49872.00",
        //                              "49871.00",
        //                              "49870.00",
        //                              "49869.00",
        //                              "49868.00",
        //                              "49867.00",
        //                              "49882.00",
        //                              "49881.00",
        //                              "49880.00",
        //                              "49879.00",
        //                              "49878.00",
        //                              "49877.00",
        //                              "49876.00"
        //                          };

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("product", System.Type.GetType("System.String"));
        //    dt.Columns.Add("program", System.Type.GetType("System.String"));
        //    dt.Columns.Add("waferseq", System.Type.GetType("System.String"));

        //    fpSpreadTargetMap_Sheet.DataSource = dt;

        //    for (int i = 0; i < Product.Length; i++)
        //    {
        //        dt.Rows.Add(new object[] { Product[i], Program[i], WaferSeqs[i] });
        //    }
        //}
        #endregion

        private void PatternSearch_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (DesignMode) return;

                if (this.WaferList != null && this.WaferList.Length > 0)
                {
                    DrawWaferRecipe(WaferList);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }


        private void ApplyData(DACrux.Base.TPWafer[] wafer)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                long[] Wafers = new long[wafer.Length];

                for (int i = 0; i < wafer.Length; i++)
                {
                    Wafers[i] = Convert.ToInt64(wafer[i].WaferSeq);
                }

                SetData(wafer);

                panelSourceMap.Dock = DockStyle.Fill;
                gallery.Dock = DockStyle.Fill;
                panelSourceMap.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #region ' Draw Method '

        public void DrawWaferMapWithThread(DACrux.Map.WaferMap wMap, long WaferSeq)
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            DataSet ds = null;
            try
            {
                if (this.Disposing)
                {
                    return;
                }

                if (wMap.InvokeRequired)
                {
                    wMap.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            DrawWaferMapWithThread(wMap, WaferSeq);
                        }
                        ));
                }
                else
                {
                    oPRBMapAnalysis = new RO.ProbeMapAnalysis();
                    ds = oPRBMapAnalysis.SelectWaferMapDrawData(WaferSeq);
                    if (ds == null) return;

                    Color tmpColor = Color.White;
                    for (int i = 0; i < ds.Tables["BINSUM"].Rows.Count; i++)
                    {
                        if (!ds.Tables["BINSUM"].Rows[i]["COLOR"].ToString().Equals("#FFFFFF"))
                        {
                            tmpColor = ColorTranslator.FromHtml(ds.Tables["BINSUM"].Rows[i]["COLOR"].ToString());

                            wMap.SetColor(DACrux.Base.Convert.intParse(ds.Tables["BINSUM"].Rows[i]["BIN"].ToString()), tmpColor);
                            //m_wcZonal.SetColor(DACrux.Base.Convert.intParse(ds.Tables[0].Rows[i]["BIN"].ToString()), chart1.Series[0].Points[i].Color);
                        }
                    }

                    wMap.WaferSize = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());
                    wMap.DieSizeX = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                    wMap.DieSizeY = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());
                    wMap.OriginIndexX = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                    wMap.OriginIndexY = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());
                    wMap.OriginX = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                    wMap.OriginY = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());
                    wMap.FirstDieX = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                    wMap.FirstDieY = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());
                    wMap.NotchAngle = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                    wMap.EdgeSize = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                    wMap.NotchType = DACrux.Base.Notch.Notch; //ds.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                    int iXYDir = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                    switch (iXYDir)
                    {
                        case 0:
                            wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                            break;
                        case 1:
                            wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                            break;
                        case 2:
                            wMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                            break;
                        case 3:
                            wMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                            break;
                    }

                    wMap.DataSource = ds.Tables["MAPDATA"];

                    /// Map Info Set=================================================================
                    /// 
                    string[] strInfo = new string[5];
                    strInfo[0] = string.Format("DEVICE  :{0}", ds.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString());
                    strInfo[1] = string.Format("PROGRAM :{0}", ds.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString());
                    strInfo[2] = string.Format("WAFER_ID:{0}-{1}", ds.Tables["WAFER_INFO"].Rows[0]["LOT_ID"].ToString(), ds.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString());
                    strInfo[3] = string.Format("YIELD   :{0:0.0#} %", ds.Tables["WAFER_INFO"].Rows[0]["YIELD"].ToString());
                    strInfo[4] = string.Format("TESTER  :{0}", ds.Tables["WAFER_INFO"].Rows[0]["TESTER"].ToString());

                    wMap.SetInfomation(strInfo);
                    wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                    wMap.Focus();
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void DrawWaferMap(DACrux.Map.WaferMap wMap, long WaferSeqs)
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            ComConfiguration oComConfig = null;
            DataSet ds = null;
            try
            {
                oPRBMapAnalysis = new RO.ProbeMapAnalysis();
                ds = oPRBMapAnalysis.SelectWaferMapDrawData(WaferSeqs);
                if (ds == null) return;

                Color tmpColor = Color.White;
                for (int i = 0; i < ds.Tables["BINSUM"].Rows.Count; i++)
                {
                    if (!ds.Tables["BINSUM"].Rows[i]["COLOR"].ToString().Equals("#FFFFFF"))
                    {
                        tmpColor = ColorTranslator.FromHtml(ds.Tables["BINSUM"].Rows[i]["COLOR"].ToString());
                        wMap.SetColor(DACrux.Base.Convert.intParse(ds.Tables["BINSUM"].Rows[i]["BIN"].ToString()), tmpColor);
                        //m_wcZonal.SetColor(DACrux.Base.Convert.intParse(ds.Tables[0].Rows[i]["BIN"].ToString()), chart1.Series[0].Points[i].Color);
                    }
                }

                wMap.WaferSize = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());
                wMap.DieSizeX = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                wMap.DieSizeY = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());
                wMap.OriginIndexX = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                wMap.OriginIndexY = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());
                wMap.OriginX = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                wMap.OriginY = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());
                wMap.FirstDieX = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                wMap.FirstDieY = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());
                wMap.NotchAngle = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                wMap.EdgeSize = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                wMap.NotchType = DACrux.Base.Notch.Notch; //ds.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                int iXYDir = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        wMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        wMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }

                wMap.DataSource = ds.Tables["MAPDATA"];

                /// Map Info Set=================================================================
                /// 
                string[] strInfo = new string[5];
                strInfo[0] = string.Format("DEVICE  :{0}", ds.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString());
                strInfo[1] = string.Format("PROGRAM :{0}", ds.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString());
                strInfo[2] = string.Format("WAFER_ID:{0}", ds.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString());
                strInfo[3] = string.Format("YIELD   :{0:0.0#} %", ds.Tables["WAFER_INFO"].Rows[0]["YIELD"].ToString());
                strInfo[4] = string.Format("TESTER  :{0}", ds.Tables["WAFER_INFO"].Rows[0]["TESTER"].ToString());

                wMap.WaferID = ds.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString();
                wMap.SetInfomation(strInfo);

                //=================================================================================================================================
                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                //=================================================================================================================================
                oComConfig = new ComConfiguration();
                DataTable dtInfo = oComConfig.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "TEST_OPTION", DACrux.Base.GlobalVariable.UserID);
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    List<string> sWaferInfo = new List<string>();

                    foreach (DataRow drInfo in dtInfo.Rows)
                    {
                        if (ds.Tables["WAFER_INFO"].Columns.IndexOf(drInfo["NAME"].ToString()) > -1)
                            sWaferInfo.Add(string.Format("{0}  :{1}", drInfo["VALUE"], ds.Tables["WAFER_INFO"].Rows[0][drInfo["NAME"].ToString()].ToString()));
                    }

                    wMap.SetInfomation(sWaferInfo.ToArray());
                }

                //Wafer Information 사용 여부
                dtInfo = oComConfig.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "TEST_OPTION_ENABLE", DACrux.Base.GlobalVariable.UserID);
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    if (dtInfo.Rows[0]["VALUE"].ToString() == "N")
                        wMap.SetInfomation(null);
                }

                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                DataTable dtMapOption = oComConfig.SelectDefectMapConfig(DACrux.Base.GlobalVariable.Factory, DACrux.Base.GlobalVariable.UserID);
                if (dtMapOption != null && dtMapOption.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMapOption.Rows)
                    {
                        string strType = dr["NAME"].ToString();
                        Color crType = ColorTranslator.FromHtml(dr["VALUE"].ToString());

                        switch (strType)
                        {
                            //Wafer Base Color
                            case "WAFER_TEST_MAP_BG":
                                wMap.WaferColor = crType;
                                break;
                            //Wafer Border Line Color
                            case "WAFER_TEST_MAP_LINE":
                                wMap.DieBorderColor = crType;
                                break;

                        }
                    }
                }

                wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                wMap.Focus();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void Redraw()
        {
            try
            {
                sWaferMap.ResetSelectedDie();

                sWaferMap.SelecetedBin = string.Join(",", GetSelectedBin());
                sWaferMap.Redraw();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(this, ex.Message, ex.Source);
            }
        }

        #endregion ========================================================================================

        #region ' Copare Method '

        private void CompareStart()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("LOT_ID", System.Type.GetType("System.String"));
                dt.Columns.Add("WAFER_ID", System.Type.GetType("System.String"));
                dt.Columns.Add("PRODUCT", System.Type.GetType("System.String"));
                dt.Columns.Add("PROGRAM", System.Type.GetType("System.String"));
                dt.Columns.Add("WAFER_SEQ", System.Type.GetType("System.String"));
                dt.Columns.Add("MATCHING_RATE", System.Type.GetType("System.Double"));

                ultraProgressBar.Value = 0;
                ultraProgressBar.Maximum = fpSpreadTargetMap_Sheet.Rows.Count;

                Compare(dt);

                buttonStart.Text = "Start";
                buttonPauseResume.Text = "Pause";
                buttonPauseResume.Enabled = false;
                buttonShowMap.Enabled = true;

                changeFindMap = true;
                //SetSpreadWithThread(fpSpreadFindMap, dt);
                fpSpreadFindMap_Sheet.DataSource = dt;
            }
            catch (Exception ex)
            {
                 MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void Compare(DataTable dt)
        {
            int[] selectedBin = GetSelectedBin();
            Point[] selectedDie = GetSelectedDie();

            if (selectedBin == null)
            {
                //MessageBox.Show(this, "Bin 을 선택하세요.");
                ultraDockManager.ControlPanes[1].Activate();
                throw new Exception("Please select bin");
            }
            if (selectedDie == null)
            {
                throw new Exception("Please select area or die");
            }

            string LotId = string.Empty;
            string WaferId = string.Empty;
            string product = string.Empty;
            string program = string.Empty;
            string waferSeq = string.Empty;
            float[] matchingRate = new float[fpSpreadTargetMap_Sheet.Rows.Count];
            float userMatchingRate = float.Parse(textBoxMatchingRate.Text);

            for (int i = 0; i < fpSpreadTargetMap_Sheet.Rows.Count; i++)
            {
                //SetSpreadSetSelectionWithThread(fpSpreadTargetMap, i);
                fpSpreadTargetMap_Sheet.ActiveRowIndex = i;
                fpSpreadTargetMap_Sheet.Models.Selection.SetSelection(i, 0, 1, fpSpreadTargetMap_Sheet.ColumnCount);
                fpSpreadTargetMap.Refresh();

                LotId = fpSpreadTargetMap_Sheet.Cells[i, 0].Text;
                WaferId = fpSpreadTargetMap_Sheet.Cells[i, 1].Text;
                product = fpSpreadTargetMap_Sheet.Cells[i, 2].Text;
                program = fpSpreadTargetMap_Sheet.Cells[i, 3].Text;
                waferSeq = fpSpreadTargetMap_Sheet.Cells[i, 4].Text;
                //DrawWaferMapWithThread(tWaferMap, DACrux.Base.Convert.longParse(waferSeq));
                DrawWaferMap(tWaferMap, DACrux.Base.Convert.longParse(waferSeq));
                matchingRate[i] = MatchingRate(selectedBin, selectedDie);
                if (userMatchingRate <= matchingRate[i])
                {
                    dt.Rows.Add(new object[] { LotId, WaferId, product, program, waferSeq, matchingRate[i] });
                }

                while (buttonPauseResume.Text.Equals("Resume"))
                {
                    System.Threading.Thread.Sleep(100);
                }

                if (buttonStart.Text.Equals("Start"))
                {
                    buttonPauseResume.Enabled = false;
                    buttonPauseResume.Text = "Pause";
                    buttonShowMap.Enabled = true;
                    break;
                }
                //SetMainStatusBarProgress(fpSpreadTargetMap_Sheet.Rows.Count, i);
                ultraProgressBar.Value = i + 1;
            }
        }

        #endregion ==============================================================

        private void SetSelectedDie(List<Point> pt)
        {
            fpSpreadDie_Sheet.Reset();

            DataTable dt = new DataTable();
            dt.Columns.Add("DIE_X", System.Type.GetType("System.Int32"));
            dt.Columns.Add("DIE_Y", System.Type.GetType("System.Int32"));

            for (int i = 0; i < pt.Count; i++)
            {
                dt.Rows.Add(new object[] { pt[i].X, pt[i].Y });
            }

            fpSpreadDie_Sheet.DataSource = dt;
        }

        private int[] GetSelectedBin()
        {
            int cnt = 0;
            for (int i = 0; i < fpSpreadBin_Sheet.Rows.Count; i++)
            {
                if (fpSpreadBin_Sheet.Cells[i, 0].Value != null)
                {
                    if ((bool)fpSpreadBin_Sheet.Cells[i, 0].Value) cnt++;
                }
            }

            if (cnt == 0) return null;

            int[] bin = new int[cnt];
            cnt = 0;
            for (int i = 0; i < fpSpreadBin_Sheet.Rows.Count; i++)
            {
                if (fpSpreadBin_Sheet.Cells[i, 0].Value != null)
                {
                    if ((bool)fpSpreadBin_Sheet.Cells[i, 0].Value) bin[cnt++] = (int)(decimal)fpSpreadBin_Sheet.Cells[i, 2].Value;
                }
            }
            return bin;
        }

        private Point[] GetSelectedDie()
        {
            if (fpSpreadDie_Sheet.Cells[0, 0].Value == null) return null;

            Point[] p = new Point[fpSpreadDie_Sheet.Rows.Count];
            for (int i = 0; i < fpSpreadDie_Sheet.Rows.Count; i++)
            {
                p[i].X = (int)fpSpreadDie_Sheet.Cells[i, 0].Value;
                p[i].Y = (int)fpSpreadDie_Sheet.Cells[i, 1].Value;
            }
            return p;
        }

        public void SetSpreadSetSelectionWithThread(FarPoint.Win.Spread.FpSpread obj, int i)
        {
            if (this.Disposing)
            {
                return;
            }

            if (obj.InvokeRequired)
            {
                obj.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SetSpreadSetSelectionWithThread(obj, i);
                    }
                    ));
            }
            else
            {
                obj.ActiveSheet.ActiveRowIndex = i;
                obj.ActiveSheet.Models.Selection.SetSelection(i, 0, 1, obj.ActiveSheet.ColumnCount);
                obj.Refresh();
            }
        }

        public void SetSpreadWithThread(FarPoint.Win.Spread.FpSpread obj, DataTable dt)
        {
            if (this.Disposing)
            {
                return;
            }

            if (obj.InvokeRequired)
            {
                obj.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SetSpreadWithThread(obj, dt);
                    }
                    ));
            }
            else
            {
                obj.DataSource = dt;
                
            }
            fpSpreadFindMap_Sheet.DataSource = dt;
            //DACrux.Utility.FPSpreadUtil.SpreadColumnFitSizeDataOrHeader(fpSpreadFindMap_Sheet);
        }

        float MatchingRate(int[] selectedBin, Point[] selectedDie)
        {
            int sameCnt = 0;
            for (int i = 0; i < selectedDie.Length; i++)
            {
                if (IsExistDie(selectedDie[i], selectedBin)) sameCnt++;
            }

            return (float)sameCnt / (float)selectedDie.Length * 100.0f;
        }

        // p가 target map에 있느냐?
        private bool IsExistDie(Point p, int[] bin)
        {
            Point[] tPoint = (Point[])tWaferMap.DiesIndex;
            int iIdx = Array.IndexOf(tPoint, p);
            if (iIdx > -1)
            {
                if (Array.IndexOf(bin, tWaferMap.Dies[iIdx].BinNumber) > -1) 
                    return true;

                return false;
            }

            return false;
        }

        private void BinSpread(string program, long waferSeq)
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            BinColor binColor;
            Color tmpColor = Color.White;

            try
            {
                oPRBMapAnalysis = new RO.ProbeMapAnalysis();
                this.alBinColor = new ArrayList();

                fpSpreadBin_Sheet.Reset();

                DataTable dt = oPRBMapAnalysis.GetBinDistribution(program, waferSeq);
                fpSpreadBin_Sheet.DataSource = dt;

                fpSpreadBin_Sheet.Columns[0].Width = 30;
                fpSpreadBin_Sheet.Columns.Add(0, 1);
                fpSpreadBin_Sheet.Columns[0].Label = "S";
                fpSpreadBin_Sheet.Columns[0].Width = 20;

                fpSpreadBin_Sheet.Columns.Add(1, 1);
                fpSpreadBin_Sheet.Columns[1].Label = "C";
                fpSpreadBin_Sheet.Columns[1].Width = 20;

                // Cell Type Define
                FarPoint.Win.Spread.CellType.CheckBoxCellType chkCell = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                FarPoint.Win.Spread.CellType.NumberCellType numberCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                numberCell.DecimalPlaces = 0;

                fpSpreadBin_Sheet.Columns[0].CellType = chkCell;
                fpSpreadBin_Sheet.Columns[2].CellType = numberCell;
                fpSpreadBin_Sheet.Columns[4].CellType = numberCell;
                fpSpreadBin_Sheet.Columns[6].CellType = numberCell;
                fpSpreadBin_Sheet.Columns[7].CellType = numberCell;

                // Bin Coloring
                string color = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    color = fpSpreadBin_Sheet.Cells[i, 5].Text;

                    if (fpSpreadBin_Sheet.Cells[i, 5].Text.Equals("#FFFFFF"))
                    {
                        binColor.bin = DACrux.Base.Convert.intParse(dt.Rows[i]["BIN"].ToString());
                        tmpColor = sWaferMap.GetColor(binColor.bin);
                        binColor.col = tmpColor;
                        this.alBinColor.Add(binColor);

                        fpSpreadBin_Sheet.Cells[i, 1].BackColor = tmpColor;

                        sWaferMap.SetColor(DACrux.Base.Convert.intParse(dt.Rows[i]["BIN"].ToString()), tmpColor);
                        tWaferMap.SetColor(DACrux.Base.Convert.intParse(dt.Rows[i]["BIN"].ToString()), tmpColor);
                    }
                    else
                    {
                        fpSpreadBin_Sheet.Cells[i, 1].BackColor = ColorTranslator.FromHtml(color);

                        sWaferMap.SetColor(DACrux.Base.Convert.intParse(dt.Rows[i]["BIN"].ToString()), fpSpreadBin_Sheet.Cells[i, 1].BackColor);
                        tWaferMap.SetColor(DACrux.Base.Convert.intParse(dt.Rows[i]["BIN"].ToString()), fpSpreadBin_Sheet.Cells[i, 1].BackColor);
                    }
                }

                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadBin_Sheet);
            }
            catch (Exception ex)
            {
                 MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        public void SetData(DACrux.Base.TPWafer[] oWaferKey)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LOT_ID", System.Type.GetType("System.String"));
            dt.Columns.Add("WAFER_ID", System.Type.GetType("System.String"));            
            dt.Columns.Add("PRODUCT", System.Type.GetType("System.String"));
            dt.Columns.Add("PROGRAM", System.Type.GetType("System.String"));
            dt.Columns.Add("WAFER_SEQ", System.Type.GetType("System.String"));

            for (int i = 0; i < oWaferKey.Length; i++)
            {
                if (oWaferKey[i].Testarea == "MULTIPROBE" || oWaferKey[i].Testarea == "PRELASER" || oWaferKey[i].Testarea == "POSTLASER")
                    dt.Rows.Add(new object[] { oWaferKey[i].LotID, oWaferKey[i].WaferID, oWaferKey[i].Product, oWaferKey[i].Program, oWaferKey[i].WaferSeq });
            }

            fpSpreadTargetMap_Sheet.DataSource = dt;

            DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadTargetMap_Sheet);

            ultraDockManager.ControlPanes[1].Activate();
            BringToFrontMapSourcePanel();
            fpSpreadDie_Sheet.Reset();

            sWaferMap.ClearFocusDie();
            //sWaferMap.ResetSelectedDie();
            //sWaferMap.Redraw();
        }

        private void gallery_OnClose()
        {
            BringToFrontMapSourcePanel();
        }

        private void BringToFrontMapSourcePanel()
        {
            //gallery.SuspendLayout();
            //panelSourceMap.ResumeLayout();
            panelSourceMap.BringToFront();
        }

        private void BringToFrontGallery()
        {
            //panelSourceMap.SuspendLayout();
            //gallery.ResumeLayout();
            gallery.BringToFront();
        }


        #region ' Click Event '

        private void sWaferMap_OnSelectDies(object sender, List<Point> selectedDies)
        {
            SetSelectedDie(selectedDies);

            ultraDockManager.ControlPanes[2].Activate();
        }

        private void buttonShowMap_Click(object sender, System.EventArgs e)
        {
            if (fpSpreadFindMap_Sheet.Rows.Count == 0) return;
            long[] waferSeqs = new long[fpSpreadFindMap_Sheet.Rows.Count];

            for (int i = 0; i < fpSpreadFindMap_Sheet.Rows.Count; i++)
            {
                waferSeqs[i] = DACrux.Base.Convert.longParse(fpSpreadFindMap_Sheet.Cells[i, 4].Text);
            }

            BringToFrontGallery();
            if (changeFindMap)
            {
                gallery.isFromMapView = true;
                gallery.Draw(waferSeqs);
                changeFindMap = false;
            }
        }

        private void buttonClear_Click(object sender, System.EventArgs e)
        {
            sWaferMap.ResetSelectedDie();
            sWaferMap.Redraw();
            fpSpreadDie_Sheet.Reset();
        }

        private void buttonStart_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (buttonStart.Text.Equals("Start"))
                {

                    if (GetSelectedBin() == null || GetSelectedBin().Length < 1) throw new Exception("Please Select Bin");

                    buttonStart.Text = "Stop";
                    buttonShowMap.Enabled = false;
                    buttonPauseResume.Enabled = true;

                    CheckForIllegalCrossThreadCalls = false;

                    System.Threading.Thread thread = new System.Threading.Thread(
                        new System.Threading.ThreadStart(CompareStart));
                    thread.Name = "pattern search";
                    thread.Start();
                    


                    //Thread.Sleep(1);

                    //thread.Join();
                }
                else
                {
                    buttonStart.Text = "Start";
                }
            }
            catch (Exception ex)
            {
                DACrux.Framework.DCMH.DspError(ex);
            }
        }

        private void buttonPauseResume_Click(object sender, System.EventArgs e)
        {
            if (buttonPauseResume.Text.Equals("Pause"))
            {
                buttonStart.Enabled = false;
                buttonPauseResume.Text = "Resume";
            }
            else
            {
                buttonStart.Enabled = true;
                buttonPauseResume.Text = "Pause";
            }
        }

        private void fpSpreadBin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ctxMnu.Show(fpSpreadBin, new Point(e.X, e.Y));
            }
        }

        private void mnuAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fpSpreadBin_Sheet.Rows.Count; i++)
            {
                fpSpreadBin_Sheet.SetValue(i, 0, true);
            }
        }

        private void mnuNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fpSpreadBin_Sheet.Rows.Count; i++)
            {
                fpSpreadBin_Sheet.SetValue(i, 0, false);
            }
        }

        private void mnuInvert_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fpSpreadBin_Sheet.Rows.Count; i++)
            {

                if (fpSpreadBin_Sheet.Cells[i, 0].Value == null || (bool)fpSpreadBin_Sheet.Cells[i, 0].Value == false)
                {
                    fpSpreadBin_Sheet.SetValue(i, 0, true); // .GetValue(i, 0)
                }
                else
                {
                    fpSpreadBin_Sheet.SetValue(i, 0, false);
                }
            }
        }

        private void butApply_Click(object sender, System.EventArgs e)
        {
            sWaferMap.SelecetedBin = m_selBin;
            Redraw();
        }

        private void fpSpreadBin_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.Column == 0)
                {
                    int[] bin = GetSelectedBin();
                    if (bin == null)
                    {
                        sWaferMap.SelecetedBin = "ALL";
                        return;
                    }

                    string[] strBin = new string[bin.Length];
                    for (int i = 0; i < strBin.Length; i++) strBin[i] = string.Format("{0}", bin[i]);
                    string bins = string.Join(",", strBin);
                    BringToFrontMapSourcePanel();
                    sWaferMap.SelecetedBin = bins;
                    m_selBin = bins;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void fpSpreadTargetMap_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            try
            {
                sWaferMap.ResetSelectedDie();
                sWaferMap.Redraw();
                fpSpreadDie_Sheet.Reset();

                FarPoint.Win.Spread.FpSpread spread = (FarPoint.Win.Spread.FpSpread)sender;
                int row = spread.ActiveSheet.Models.Selection[0].Row;

                if (row == -1) return;

                this.Cursor = Cursors.WaitCursor;
                BringToFrontMapSourcePanel();

                string product = spread.ActiveSheet.Cells[row, 2].Text;
                string program = spread.ActiveSheet.Cells[row, 3].Text;
                string waferSeq = spread.ActiveSheet.Cells[row, 4].Text;

                sWaferMap.SelecetedBin = "ALL";
                BinSpread(program, DACrux.Base.Convert.longParse(waferSeq));
                DrawWaferMap(sWaferMap, DACrux.Base.Convert.longParse(waferSeq));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion ===================================================================================================

        #region ' Method ' 

        private void toExcel()
        {
            //0.객체 선언 , 인스턴싱
            ExcelUtil oXL = new ExcelUtil();

            //1.엑셀 오브젝트 얻어 오기
            Excel._Workbook oWB = oXL.fnGetExcelWorkbook(true, 3);
            Bitmap bmpWaferMap = null;
            string[,] strHeader;

            /// RawData=TITLE===========================================================================================================
            strHeader = new string[1, 1];
            strHeader[0, 0] = string.Format("Raw Data : {0}-{1}", m_ds.Tables["WAFER_INFO"].Rows[0]["LOT_ID"].ToString(), m_ds.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString());
            oXL.fnSetSheetName(oWB, 1, "Raw Data");
            oXL.fnSetValue(oWB, 1, 1, 1, 1, 1, strHeader, true, "Times New Roman", 16, ExcelUtil.Color.black, ExcelUtil.Alignment.xlVAlignCenter, ExcelUtil.HAlignment.xlHAlignLeft, false, ExcelUtil.Color.white);

            /// RawData=HEADER===========================================================================================================
            strHeader = new string[1, m_ds.Tables["MAPDATA"].Columns.Count];
            for (int ic = 0; ic < m_ds.Tables["MAPDATA"].Columns.Count; ic++)
            {
                strHeader[0, ic] = m_ds.Tables["MAPDATA"].Columns[ic].ColumnName;
            }
            oXL.fnSetValue(oWB, 1, 2, 1, m_ds.Tables["MAPDATA"].Rows.Count, m_ds.Tables["MAPDATA"].Columns.Count, strHeader);

            /// RawData=DATA===========================================================================================================
            object[,] strRawData = new object[m_ds.Tables["MAPDATA"].Rows.Count, m_ds.Tables["MAPDATA"].Columns.Count];
            for (int ir = 0; ir < m_ds.Tables["MAPDATA"].Rows.Count; ir++)
            {
                for (int ic = 0; ic < m_ds.Tables["MAPDATA"].Columns.Count; ic++)
                {
                    strRawData[ir, ic] = m_ds.Tables["MAPDATA"].Rows[ir][ic];
                }
            }
            oXL.fnSetValue(oWB, 1, 3, 1, m_ds.Tables["MAPDATA"].Rows.Count + 2, m_ds.Tables["MAPDATA"].Columns.Count, strRawData);

            /// MAP============================================================================================================
            /// 
            strHeader[0, 0] = "Wafer Map & Chart";
            oXL.fnSetSheetName(oWB, 2, "Wafer Map");
            //Bitmap bmpWaferMap = m_wMap.GetMapImage();
            string tmpFile = string.Format(@"{0}\tmpMap.bmp", Application.StartupPath);
            //bmpWaferMap = sWaferMap.MapObj
            bmpWaferMap.Save(tmpFile);
            oXL.fnSetImage(oWB, 2, tmpFile, 1, 1);
            System.IO.File.Delete(tmpFile);

            // Gallery =========================================================
            if (!changeFindMap)
            {
                oXL.fnSetSheetName(oWB, 1, "Find Map Gallery");

                string[] arrTempMapPath = gallery.SaveTempBmp();
                int maxCol = 5;
                int col = 1;
                int row = 1;
                for (int i = 0; i < arrTempMapPath.Length; i++)
                {
                    if (col == maxCol)
                    {
                        row++;
                        col = 1;
                    }
                    oXL.fnSetImage(oWB, 1, arrTempMapPath[i], row, col);
                    System.IO.File.Delete(arrTempMapPath[i]);

                    col++;
                }
            }
        }

        #endregion ==========================================================================================


        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            try
            {
                ApplyData(wafer);

                this.TPWaferList = wafer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        public void DrawWaferRecipe(string[] strWafer)
        {
            try
            {
                DACrux.Base.TPWafer[] oWafer = new Base.TPWafer[strWafer.Length];
                for (int iWafer = 0; iWafer < strWafer.Length; iWafer++)
                {
                    oWafer[iWafer].WaferSeq = strWafer[iWafer];
                }

                DrawWafer(oWafer);

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }
    }
}
