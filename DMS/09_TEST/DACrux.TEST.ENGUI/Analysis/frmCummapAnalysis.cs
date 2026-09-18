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

namespace DACrux.TEST.ENGUI
{
    public partial class frmCummapAnalysis : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl
    {
        private string[] m_strSelBin = null;
        private string m_strPrduct = string.Empty;
        private string m_strProgram = string.Empty;
        private long[] m_strWaferSeqs = null;
        private DataSet m_ds = null;

        public frmCummapAnalysis()
        {
            InitializeComponent();
        }

        private void frmCummapAnalysis_Load(object sender, EventArgs e)
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

        public void DrawCummap(long[] WaferSeqs)
        {
            try
            {
                m_strWaferSeqs = WaferSeqs;

                DrawCummap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Data를 Load할 수 없습니다.[Err:{0}]", ex.Message));
            }
        }


        private void DrawCummap()
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            try
            {
                if (m_ds != null)
                {
                    m_ds.Dispose();
                    m_ds = null;
                }

                m_wMap.WaferColor = Color.White;
                oPRBMapAnalysis = new RO.ProbeMapAnalysis();
                m_ds = oPRBMapAnalysis.GetCumMapReport(m_strWaferSeqs);

                /// Color Depth ====================================================================
                /// 
                for (int i = 0; i <= m_strWaferSeqs.Length; i++)
                {
                    //m_wMap.SetColor(i, Color.Red, (255 / (m_strWaferSeqs.Length + 1)) * i);

                    Color oColor = Color.FromArgb((255 / (m_strWaferSeqs.Length + 1)) * i
                                   , 255
                                   , (255 / (i + 2))
                                   , (255 / (i + 2)));


                    m_wMap.SetColor(i, oColor);
                }

                /// BIN Description & Summary========================================================
                m_strSelBin = new string[] { "1" };

                chkboxSelBin.Items.Clear();

                for (int i = 0; i < m_ds.Tables["BINDESC"].Rows.Count; i++)
                {
                    chkboxSelBin.Items.Add(string.Format("{0}-{1} [{2}]", m_ds.Tables["BINDESC"].Rows[i]["BIN"].ToString()
                        , m_ds.Tables["BINDESC"].Rows[i]["BIN_NAME"].ToString()
                        , m_ds.Tables["BINDESC"].Rows[i]["CNT"].ToString()));
                    if (Array.IndexOf(m_strSelBin, m_ds.Tables["BINDESC"].Rows[i]["BIN"].ToString()) > -1) chkboxSelBin.SetItemChecked(i, true);
                }

                m_strPrduct = m_ds.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString();
                m_strProgram = m_ds.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString();


                /// Recipe===============================================================================
                m_wMap.WaferSize = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());

                m_wMap.DieSizeX = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                m_wMap.DieSizeY = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());

                m_wMap.OriginIndexX = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_wMap.OriginIndexY = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());

                m_wMap.OriginX = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_wMap.OriginY = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());

                m_wMap.FirstDieX = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                m_wMap.FirstDieY = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());

                m_wMap.NotchAngle = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                m_wMap.EdgeSize = DACrux.Base.Convert.doubleParse(m_ds.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                m_wMap.NotchType = DACrux.Base.Notch.Notch; //m_ds.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Notch : DACrux.Base.Notch.Flat;

                int iXYDir = DACrux.Base.Convert.intParse(m_ds.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }

                GetRawData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Can't load map information[Err:{0}]", ex.Message));
            }
        }

        public void GetRawData()
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            DataTable dt = null;
            try
            {
                if (m_strSelBin.Length == 0) return;
                oPRBMapAnalysis = new RO.ProbeMapAnalysis();
                dt = oPRBMapAnalysis.GetCumRawData(m_strProgram, m_strWaferSeqs, m_strSelBin);
                if (m_ds.Tables.IndexOf("MAPDATA") > 0)
                {
                    m_ds.Tables.Remove("MAPDATA");
                }
                m_ds.Tables.Add(dt.Copy());


                m_wMap.DataSource = utgRawData.DataSource = m_ds.Tables["MAPDATA"];
                DataTable chartData = new DataTable();
                string[] strBinDesc = new string[m_ds.Tables["BINDESC"].Rows.Count];
                string[] strBinColumn = new string[m_ds.Tables["BINDESC"].Rows.Count];
                int[] iBinSum = new int[m_ds.Tables["BINDESC"].Rows.Count];
                chartData.Columns.Add(new DataColumn("WAFER_ID",typeof(string)));
                // BINDESC - "BIN, BIN_NAME, COLOR, CNT"

                for (int i = 0; i < m_ds.Tables["BINDESC"].Rows.Count; i++)
                {
                    strBinColumn[i] = string.Format("BIN{0}",m_ds.Tables["BINDESC"].Rows[i]["BIN"].ToString());
                    chartData.Columns.Add(new DataColumn(strBinColumn[i],typeof(Int32)));
                    strBinDesc[i] = m_ds.Tables["BINDESC"].Rows[i]["BIN_NAME"].ToString();
                    iBinSum[i] = DACrux.Base.Convert.intParse(m_ds.Tables["BINDESC"].Rows[i]["CNT"].ToString());
                }

                for (int i = 0; i < m_ds.Tables["WAFER_INFO"].Rows.Count; i++)
                {
                    object[] oRaw = new object[strBinDesc.Length + 1];
                    oRaw[0] = string.Format("{0}-{1}"
                         ,m_ds.Tables["WAFER_INFO"].Rows[i]["LOT_ID"].ToString()
                         ,m_ds.Tables["WAFER_INFO"].Rows[i]["WAFER_ID"].ToString().Replace(m_ds.Tables["WAFER_INFO"].Rows[i]["LOT_ID"].ToString(),""));
                    for (int c = 0; c < strBinColumn.Length; c++)
                    {
                        oRaw[c + 1] = m_ds.Tables["WAFER_INFO"].Rows[i][strBinColumn[c]];
                    }
                    chartData.Rows.Add(oRaw);
                }

                ultraChart1.Transform3D.Perspective = 0;
                ultraChart1.Transform3D.Scale = 85;
                ultraChart1.Transform3D.XRotation = 115;
                ultraChart1.Transform3D.YRotation = 15;

                ultraChart1.Data.DataSource = chartData;
                ultraChart1.Data.DataBind();
                ultraChart1.Data.SetColumnLabels(strBinDesc);

                DataRow[] drs = m_ds.Tables["WAFER_INFO"].Select();
                string[] strInfo = new string[2];
                strInfo[0] = string.Format("DEVICE  :{0}", m_ds.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString());
                strInfo[1] = string.Format("PROGRAM :{0}", m_ds.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString());

                m_wMap.SetInfomation(strInfo);
                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                m_wMap.Focus();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void GetRawData2()
        {
            RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            DataTable dt = null;
            try
            {
                oPRBMapAnalysis = new RO.ProbeMapAnalysis();

                /// Raw Data=======================================================================
                dt = oPRBMapAnalysis.GetCumRawData(m_strProgram, m_strWaferSeqs, m_strSelBin);
                dt.TableName = "MAPDATA";

                if (m_ds.Tables.IndexOf("MAPDATA") > -1) m_ds.Tables.Remove("MAPDATA");
                m_ds.Tables.Add(dt.Copy());
                m_wMap.DataSource = m_ds.Tables["MAPDATA"];
                DataRow[] drs = m_ds.Tables["WAFER_INFO"].Select();

                int[] nCnt = new int[m_ds.Tables["BINDESC"].Rows.Count];
                double[] dblYield = new double[m_ds.Tables["BINDESC"].Rows.Count];
                string[] str = new string[m_ds.Tables["BINDESC"].Rows.Count];
                double dblTotal = Convert.ToDouble(m_ds.Tables["BINDESC"].Compute("SUM(CNT)", null));

                for (int j = 0; j < m_ds.Tables["BINDESC"].Rows.Count; j++)
                {
                    nCnt[j] = DACrux.Base.Convert.intParse(m_ds.Tables["BINDESC"].Rows[j]["CNT"].ToString());
                    str[j] = m_ds.Tables["BINDESC"].Rows[j]["BIN"].ToString();

                    string strConvert = string.Empty;
                    strConvert = ((System.Convert.ToDouble(nCnt[j]) / dblTotal) * 100.0).ToString("###.##");

                    if (strConvert == string.Empty) dblYield[j] = 0.0;
                    else dblYield[j] = System.Convert.ToDouble(strConvert);
                }

                /// Map Info Set=================================================================
                /// 
                string[] strInfo = new string[3];
                strInfo[0] = string.Format("DEVICE  :{0}", m_ds.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString());
                strInfo[1] = string.Format("PROGRAM :{0}", m_ds.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString());
                strInfo[2] = string.Format("LOT ID  :{0}", m_ds.Tables["WAFER_INFO"].Rows[0]["LOT_ID"].ToString());

                m_wMap.SetInfomation(strInfo);
                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                m_wMap.Focus();
                m_wMap.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ToExcel()
        {

            //0.객체 선언 , 인스턴싱

            ExcelUtil oXL = new ExcelUtil();

            //1.엑셀 오브젝트 얻어 오기
            Microsoft.Office.Interop.Excel._Workbook oWB = oXL.fnGetExcelWorkbook(true, 3);

            string[,] strHeader;

            /// RawData=TITLE===========================================================================================================
            strHeader = new string[1, 1];
            strHeader[0, 0] = string.Format("Raw Data : {0}-{1}", m_ds.Tables["WAFER_INFO"].Rows[0]["LOT_ID"].ToString(), m_ds.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString());
            oXL.fnSetSheetName(oWB, 1, "Raw Data");
            oXL.fnSetValue(oWB, 1, 1, 1, 1, 1, strHeader, true, "Times New Roman", 16, ExcelUtil.Color.black, ExcelUtil.Alignment.xlVAlignCenter, ExcelUtil.HAlignment.xlHAlignLeft, false, ExcelUtil.Color.white);

            DataTable dtSheet = new DataTable();
            dtSheet = (DataTable)utgRawData.DataSource;

            /// RawData=HEADER===========================================================================================================
            strHeader = new string[1, dtSheet.Columns.Count];
            for (int ic = 0; ic < dtSheet.Columns.Count; ic++)
            {
                strHeader[0, ic] = dtSheet.Columns[ic].ToString();
            }
            oXL.fnSetValue(oWB, 1, 2, 1, dtSheet.Rows.Count, dtSheet.Columns.Count, strHeader);

            /// RawData=DATA===========================================================================================================
            object[,] strRawData = new object[dtSheet.Rows.Count, dtSheet.Columns.Count];

            for (int row = 0; row < dtSheet.Rows.Count; row++)
            {
                for (int col = 0; col < dtSheet.Columns.Count; col++)
                {
                    strRawData[row, col] = dtSheet.Rows[row][col].ToString();
                }
            }

            oXL.fnSetValue(oWB, 1, 3, 1, dtSheet.Rows.Count + 2, dtSheet.Columns.Count, strRawData);

            /// MAP============================================================================================================
            /// 
            strHeader[0, 0] = "Wafer Map & Chart";
            oXL.fnSetSheetName(oWB, 2, "Wafer Map");
            Bitmap bmpWaferMap = m_wMap.GetMapImage();
            string tmpFile = string.Format(@"{0}\tmpMap.bmp", Application.StartupPath);
            bmpWaferMap.Save(tmpFile);
            oXL.fnSetImage(oWB, 2, tmpFile, 1, 1);

            //Chart Export=====================================================================================================
            //oXL.fnSetChartFx(oWB,2,chart1,30,2,1.0f, 1.0f);

            oXL = null;
            File.Delete(tmpFile);
            strHeader = null;
            strRawData = null;
            ExcelUtil.fnExcelProcessExit();
        }

        private void chkboxSelBin_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_strSelBin = new string[chkboxSelBin.CheckedItems.Count];
            chkboxSelBin.CheckedItems.CopyTo(m_strSelBin, 0);

            if (m_strSelBin.Length == 0) return;

            for (int i = 0; i < m_strSelBin.Length; i++)
            {
                m_strSelBin[i] = m_strSelBin[i].Substring(0, m_strSelBin[i].IndexOf("-"));
            }

            GetRawData2();
        }

        private void chkBinCnt_CheckedChanged(object sender, EventArgs e)
        {
            m_wMap.VisibleDieValue = chkBinCnt.Checked;
        }

        private void m_wMap_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            utlDieInfo.Text = string.Format("X,Y=[{0},{1}]\nCOUNT=[{2}]"
                                , NewDie.IndexX.ToString()
                                , NewDie.IndexY.ToString()
                                , NewDie.BinNumber.ToString());
            if (m_ds != null)
            {
                int i = m_wMap.Dies.IndexOf(NewDie);
                if (i < utgRawData.Rows.Count)
                    utgRawData.Rows[i].Activate();
            }
        }


        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            try
            {

                long[] Wafers = new long[wafer.Length];

                for (int i = 0; i < wafer.Length; i++)
                {
                    Wafers[i] = Convert.ToInt32(wafer[i].WaferSeq);
                }

                DrawCummap(Wafers);

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
