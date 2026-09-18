using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.TEST.RO;

namespace DACrux.TEST.ENGUI
{
    public partial class frmVisualInspection : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl
    {

        private DataSet m_DS = null;
        DataTable m_dtVICnt = null;
        DACrux.Base.TPWafer OCurrentWafer;

        private string m_TestAreaGroup = string.Empty;
        private string m_TestArea = string.Empty;
        private string m_Product = string.Empty;
        private string m_Program = string.Empty;
        private string m_LotID = string.Empty;
        private string m_WaferID = string.Empty;

        public frmVisualInspection()
        {
            InitializeComponent();
            m_ewMap.EditMenuVisible = false;
        }

        public void Initialize()
        {
            fpSpread1_Sheet1.Rows.Count = 0;
            SetKeymap(null);
        }


        public void DrawTESTMap(long WaferSeq)
        {
            DataTable dt = null;
            DataSet dsData = null;
            DACrux.TEST.RO.ProbeMapAnalysis oTestMap = null;

            try
            {
                if (m_DS != null) m_DS.Dispose();
                m_DS = new DataSet();

                oTestMap = new ProbeMapAnalysis();

                // Bin Color Get =================================================
                //SetMapColor(m_LotID, m_WaferID, m_TestArea, m_Program);
                m_ewMap.SetDefaultColors();

                dsData = oTestMap.SelectWaferMapBasic(WaferSeq);


                m_ewMap.WaferSize = DACrux.Base.Convert.doubleParse(dsData.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());

                m_ewMap.DieSizeX = DACrux.Base.Convert.doubleParse(dsData.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                m_ewMap.DieSizeY = DACrux.Base.Convert.doubleParse(dsData.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());

                m_ewMap.OriginIndexX = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_ewMap.OriginIndexY = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());

                m_ewMap.OriginX = DACrux.Base.Convert.doubleParse(dsData.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_ewMap.OriginY = DACrux.Base.Convert.doubleParse(dsData.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());

                m_ewMap.FirstDieX = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                m_ewMap.FirstDieY = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());

                m_ewMap.NotchAngle = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                m_ewMap.EdgeSize = DACrux.Base.Convert.doubleParse(dsData.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                m_ewMap.NotchType = DACrux.Base.Notch.Notch; //dsData.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                m_ewMap.DieMinX = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_X"].ToString());
                m_ewMap.DieMinY = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_Y"].ToString());
                m_ewMap.DieMaxX = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_X"].ToString());
                m_ewMap.DieMaxY = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_Y"].ToString());

                int iXYDir = DACrux.Base.Convert.intParse(dsData.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }
                m_ewMap.DieCalculation(true);


                // Map Bin Row Data ===================================================================================================
                dt = dsData.Tables["MAPDATA"].Copy();
                dt.TableName = "MAP";
                m_DS.Tables.Add(dt.Copy());
                m_ewMap.DataSource = m_DS.Tables["MAP"];
                m_ewMap.VisibleVIFail = true;
                m_ewMap.VisibleFocusDie = true;
                m_ewMap.VIMember = "BIN";
                m_ewMap.DieFocusingType = Map.FocusType.Arraw;

                // Wafer Infomation====================================================================================================
                dt = dsData.Tables["MAPDATA"].Copy();
                dt.TableName = "WAFER_INFO";
                m_DS.Tables.Add(dt.Copy());

                string[] strInfo = new string[3];
                strInfo[0] = string.Format("DEVICE  :   {0}", m_Product);
                strInfo[1] = string.Format("LOT_ID  :   {0}", m_LotID);
                strInfo[2] = string.Format("WAFER_ID:   {0}", m_WaferID);

                m_ewMap.SetInfomation(strInfo);



                FarPoint.Win.Spread.CellType.NumberCellType ct = new FarPoint.Win.Spread.CellType.NumberCellType();
                ct.DecimalPlaces = 0;
                m_ewMap.Redraw();
                m_ewMap.Refresh();
                m_ewMap.Width = m_ewMap.Width + 1; /// Size를 자동으로 조절하게 하는 Trip

                // Key Map Get ============================================================================
                SetKeymap(m_TestArea);


                // Count
                BinCounting();

            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Wafer 정보를 전달하여 Edit할 Wafer를 Drawing한다.
        /// </summary>
        /// <param name="Step">Testarea or 공정</param>
        /// <param name="Product">제품코드</param>
        /// <param name="LotID">Lot ID</param>
        /// <param name="WaferID">Full Wafer ID</param>
        /// <param name="Program">Test Program (필요시)</param>
        public void DrawWaferMap(string Step, string Product, string LotID, string WaferID, string Program = null)
        {
            DataTable dt = null;
            DACrux.TEST.RO.ProbeAdmin oMapDef = null;
            DACrux.TEST.RO.ProbeMapAnalysis oAviTable = null;
            DACrux.TEST.RO.VisulalInspection oVisual = null;

            m_TestArea = Step;
            m_Product = Product;
            m_LotID = LotID;
            m_WaferID = WaferID;
            m_Program = Program;

            try
            {
                if (m_DS != null) m_DS.Dispose();
                m_DS = new DataSet();

                oAviTable = new RO.ProbeMapAnalysis();
                oMapDef = new RO.ProbeAdmin();
                oVisual = new VisulalInspection();

                // Bin Color Get =================================================
                SetMapColor(m_LotID, m_WaferID, m_TestArea, m_Program);

                dt = oMapDef.GetMapDef(m_Product);
                if (dt == null || dt.Rows.Count < 1)
                {
                    dt = oAviTable.GetMapDefByLotID(m_Product);
                    if (dt == null || dt.Rows.Count < 1)
                    {
                        throw new Exception(string.Format("Product : {0} is undefine as Golden Map", Product));
                    }
                }

                m_ewMap.WaferSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["WAFER_SIZE"].ToString());

                m_ewMap.DieSizeX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["CHIP_SIZE_X"].ToString());
                m_ewMap.DieSizeY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["CHIP_SIZE_Y"].ToString());

                m_ewMap.OriginIndexX = DACrux.Base.Convert.intParse(dt.Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_ewMap.OriginIndexY = DACrux.Base.Convert.intParse(dt.Rows[0]["ORIGIN_INDEX_Y"].ToString());

                m_ewMap.OriginX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_ewMap.OriginY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_MICRO_Y"].ToString());

                m_ewMap.FirstDieX = DACrux.Base.Convert.intParse(dt.Rows[0]["FIRST_INDEX_X"].ToString());
                m_ewMap.FirstDieY = DACrux.Base.Convert.intParse(dt.Rows[0]["FIRST_INDEX_Y"].ToString());

                m_ewMap.NotchAngle = DACrux.Base.Convert.intParse(dt.Rows[0]["ANGLE"].ToString());
                m_ewMap.EdgeSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["EDGE_SIZE"].ToString());
                m_ewMap.NotchType = DACrux.Base.Notch.Notch; //dt.Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                m_ewMap.DieMinX = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MIN_X"].ToString());
                m_ewMap.DieMinY = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MIN_Y"].ToString());
                m_ewMap.DieMaxX = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MAX_X"].ToString());
                m_ewMap.DieMaxY = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MAX_Y"].ToString());

                int iXYDir = DACrux.Base.Convert.intParse(dt.Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        m_ewMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }
                m_ewMap.DieCalculation(true);

                // Map Bin Row Data ===================================================================================================
                dt = oVisual.GetAVIMapData(m_TestArea, m_Product, m_LotID, m_WaferID, m_Program);

                dt.TableName = "MAP";
                m_DS.Tables.Add(dt.Copy());
                m_ewMap.DataSource = m_DS.Tables["MAP"];
                m_ewMap.VisibleVIFail = true;
                m_ewMap.VisibleFocusDie = true;
                m_ewMap.VIMember = "BIN";
                m_ewMap.DieFocusingType = Map.FocusType.Arraw;

                // Wafer Infomation====================================================================================================
                dt = oAviTable.GetWaferInfo(m_TestArea, m_Product, m_LotID, m_WaferID);
                dt.TableName = "WAFER_INFO";
                m_DS.Tables.Add(dt.Copy());

                string[] strInfo = new string[3];
                strInfo[0] = string.Format("DEVICE  :   {0}", m_Product);
                strInfo[1] = string.Format("LOT_ID  :   {0}", m_LotID);
                strInfo[2] = string.Format("WAFER_ID:   {0}", m_WaferID);

                m_ewMap.SetInfomation(strInfo);



                FarPoint.Win.Spread.CellType.NumberCellType ct = new FarPoint.Win.Spread.CellType.NumberCellType();
                ct.DecimalPlaces = 0;
                m_ewMap.Redraw();
                m_ewMap.Refresh();
                m_ewMap.Width = m_ewMap.Width + 1; /// Size를 자동으로 조절하게 하는 Trip

                // Key Map Get ============================================================================
                SetKeymap(m_TestArea);


                // Count
                BinCounting();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                oMapDef = null;
                oAviTable = null;
            }
        }

        private void TPUCVisualInspection_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (DesignMode) return;

                Initialize();

                if (this.WaferList != null && this.WaferList.Length > 0)
                {
                    DrawWaferRecipe(WaferList);
                }

            }
            catch(Exception){}
        }

        private void SetMapColor(string LotID, string WaferID, String TestArea, String Product)
        {
            VisulalInspection oVISpec = null;
            ProbeMapAnalysis oMapData = null;
            DataTable dt = null;

            string strWaferSeq = null;

            try
            {
                oVISpec = new VisulalInspection();
                oMapData = new ProbeMapAnalysis();

                dt = oMapData.GetWaferInfo(TestArea, LotID, WaferID, false);
                if (dt == null || dt.Rows.Count < 1)
                    throw new Exception("Data is not Exist");
                strWaferSeq = dt.Rows[0]["WAFER_SEQ"].ToString();

                dt = oMapData.GetBinDistribution(Product, new string[] { strWaferSeq }, false, true);

                dt.TableName = "BINSUM";
                m_DS.Tables.Add(dt.Copy());

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    m_ewMap.SetColor(DACrux.Base.Convert.intParse(dt.Rows[i]["BIN"].ToString()), ColorTranslator.FromHtml(dt.Rows[i]["COLOR"].ToString()));
                }
            }
            catch
            {
            }
        }

        private void SetKeymap(string TestArea)
        {
            DataTable dt = null;
            DACrux.TEST.RO.VisulalInspection oVI = null;
            try
            {
                /// 1; Default
                for (int i = 0; i < 20; i++)
                {
                    m_ewMap.SetVIValue(i, -1);
                }

                /// 1.1 Select Key Map
                /// ////////////////////////////////////////////////////////////////////
                oVI = new VisulalInspection();
                dt = oVI.GetAVISpec(TestArea);

                /// ////////////////////////////////////////////////////////////////////

                // 1.2 VI Input Undefined
                if (dt == null || dt.Rows.Count < 1)
                {
                    // 1.2.1 Table Create
                    dt = new DataTable();

                    // 1.2.2 Column Create
                    dt.Columns.Add(new DataColumn("INSPTYPE", typeof(string)));
                    dt.Columns.Add(new DataColumn("BIN", typeof(int)));
                    dt.Columns.Add(new DataColumn("BIN_NAME", typeof(string)));
                    dt.Columns.Add(new DataColumn("COLOR", typeof(string)));
                    dt.Columns.Add(new DataColumn("DESCRIPTION", typeof(string)));
                    dt.Columns.Add(new DataColumn("KEYMAP", typeof(string)));

                    // 1.2.3 Rowa Add
                    dt.Rows.Add(new object[] { "IQC", 0, "Good", "255216000", "Pass", "0" });
                    dt.Rows.Add(new object[] { "IQC", 1, "Scratch", "000255064", "Visual Fail - 1", "1" });
                    dt.Rows.Add(new object[] { "IQC", 2, "BIN02", "064000128", "Visual Fail - 2", "2" });
                    dt.Rows.Add(new object[] { "IQC", 3, "BIN03", "000128128", "Visual Fail - 3", "3" });
                    dt.Rows.Add(new object[] { "IQC", 4, "BIN04", "128128255", "Visual Fail - 4", "4" });
                    dt.Rows.Add(new object[] { "IQC", 5, "BIN05", "128064000", "Visual Fail - 5", "" });
                    dt.Rows.Add(new object[] { "IQC", 6, "BIN06", "255128255", "Visual Fail - 6", "" });
                    dt.Rows.Add(new object[] { "IQC", 7, "BIN07", "000000064", "Visual Fail - 7", "" });
                    dt.Rows.Add(new object[] { "IQC", 8, "BIN08", "000255255", "Visual Fail - 8", "" });
                    dt.Rows.Add(new object[] { "IQC", 9, "BIN09", "000128000", "Visual Fail - 9", "" });
                    dt.Rows.Add(new object[] { "IQC", 10, "BIN10", "000255000", "Visual Fail - 10", "" });
                    dt.Rows.Add(new object[] { "IQC", 11, "BIN11", "000064064", "Visual Fail - 11", "" });
                    dt.Rows.Add(new object[] { "IQC", 12, "BIN12", "192128000", "Visual Fail - 12", "" });
                    dt.Rows.Add(new object[] { "IQC", 13, "BIN13", "255000128", "Visual Fail - 13", "" });
                    dt.Rows.Add(new object[] { "IQC", 14, "BIN14", "255255000", "Visual Fail - 14", "" });
                    dt.Rows.Add(new object[] { "IQC", 15, "BIN15", "064064128", "Visual Fail - 15", "" });
                    dt.Rows.Add(new object[] { "IQC", 16, "BIN16", "064128255", "Visual Fail - 16", "" });
                    dt.Rows.Add(new object[] { "IQC", 17, "BIN17", "192128128", "Visual Fail - 17", "" });
                    dt.Rows.Add(new object[] { "IQC", 18, "BIN18", "226023028", "Visual Fail - 18", "" });
                    dt.Rows.Add(new object[] { "IQC", 19, "BIN19", "128255128", "Visual Fail - 19", "" });
                    dt.Rows.Add(new object[] { "IQC", 20, "BIN20", "255128000", "Visual Fail - 20", "" });
                    dt.Rows.Add(new object[] { "IQC", 21, "BIN21", "064000128", "Visual Fail - 21", "" });
                    dt.Rows.Add(new object[] { "IQC", 22, "BIN22", "255128128", "Visual Fail - 22", "" });
                    dt.Rows.Add(new object[] { "IQC", 23, "BIN23", "192128255", "Visual Fail - 23", "" });
                    dt.Rows.Add(new object[] { "IQC", 24, "BIN24", "128000255", "Visual Fail - 24", "" });
                    dt.Rows.Add(new object[] { "IQC", 25, "BIN25", "255255128", "Visual Fail - 25", "" });
                    dt.Rows.Add(new object[] { "IQC", 26, "BIN26", "000128255", "Visual Fail - 26", "" });
                    dt.Rows.Add(new object[] { "IQC", 27, "BIN27", "000255128", "Visual Fail - 27", "" });
                    dt.Rows.Add(new object[] { "IQC", 28, "BIN28", "064128000", "Visual Fail - 28", "" });
                }
                fpSpread1_Sheet1.Rows.Count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string VICnt = "0";
                    string Bin = dt.Rows[i]["BIN"].ToString();


                    // VI Cnt ===============================
                    if (m_dtVICnt != null)
                    {
                        if (m_dtVICnt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in m_dtVICnt.Rows)
                            {
                                if (dr[0].ToString() == Bin)
                                    VICnt = dr[1].ToString();
                            }
                        }
                    }

                    FarPoint.Win.Spread.CellType.ImageCellType ic = new FarPoint.Win.Spread.CellType.ImageCellType();

                    int iKey = -1;
                    if (int.TryParse(dt.Rows[i]["KEYMAP"].ToString(), out iKey) == false) continue;

                    m_ewMap.SetVIValue(iKey, DACrux.Base.Convert.intParse(dt.Rows[i]["BIN"].ToString()));

                    fpSpread1_Sheet1.Rows.Add(fpSpread1_Sheet1.Rows.Count, 1);
                    int iRow = fpSpread1_Sheet1.Rows.Count - 1;
                    fpSpread1_Sheet1.Cells[iRow, 0].CellType = ic;
                    fpSpread1_Sheet1.Cells[iRow, 0].Value = imageList.Images[iKey];
                    fpSpread1_Sheet1.Cells[iRow, 1].Value = dt.Rows[i]["BIN"];
                    fpSpread1_Sheet1.Cells[iRow, 2].Value = 0;
                    fpSpread1_Sheet1.Cells[iRow, 3].Value = dt.Rows[i]["BIN_NAME"];

                    fpSpread1_Sheet1.Cells[iRow, 0].BackColor = ColorTranslator.FromHtml(dt.Rows[i]["COLOR"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveData()
        {
            SaveData(m_TestArea, m_Product, m_LotID, m_WaferID);
        }

        public void SaveData(string Step, string product, string lot_Id, string wafer_id)
        {
            DACrux.TEST.RO.VisulalInspection oVIINSP = null;
            try
            {
                /// AVI TABLE SAVE
                oVIINSP = new DACrux.TEST.RO.VisulalInspection();

                List<DACrux.Base.Die> oUpdateDie = new List<DACrux.Base.Die>();
                foreach (DACrux.Base.Die oDie in m_ewMap.Dies)
                {
                    if (oDie.VIFail > 0 && oDie.DieProp == 1)
                    {
                        oUpdateDie.Add(oDie);
                    }
                }

                oVIINSP.UpdateVI(DACrux.Base.GlobalVariable.Factory, m_TestArea, m_Product, m_Program, m_LotID, m_WaferID, oUpdateDie);

                MessageBox.Show("Upload completed");

            }
            catch (Exception ex)
            {
                DspMessage(string.Format("Data Upload Error.[Err:{0}]", ex.Message));
            }
        }

        private void m_ewMap_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            try
            {
                txtXIndex.Text = NewDie.IndexX.ToString();
                txtYIndex.Text = NewDie.IndexY.ToString();
                txtEDS.Text = NewDie.BinNumber.ToString();
                txtVI.Text = NewDie.VIFail.ToString();
            }
            catch
            {
            }
        }

        private void m_ewMap_OnChangeDieProperty(object sender, Base.Die NewDie)
        {
            try
            {
                BinCounting();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void BinCounting()
        {
            // 이함수 속도가 빨라야 할텐데... 
            try
            {
                DataTable dtSource = (DataTable)m_ewMap.DataSource;
                DataTable dt = DACrux.Base.FilterEx.SelectGroupBy("BINCNT", dtSource, m_ewMap.VIMember);
                DataRow[] drs = null;
                for (int i = 0; i < fpSpread1_Sheet1.Rows.Count; i++)
                {
                    string strCatValue = fpSpread1_Sheet1.Cells[i, 1].Value.ToString();
                    int iCnt = 0;
                    if (strCatValue == "0" || strCatValue == "")
                    {
                        drs = dt.Select(string.Format("{0} = '{1}' or {2} IS NULL", m_ewMap.VIMember, strCatValue, m_ewMap.VIMember));
                        foreach (DataRow dr in drs)
                        {
                            iCnt += DACrux.Base.Convert.intParse(dr["COUNT"].ToString());
                        }
                    }
                    else
                    {
                        drs = dt.Select(string.Format("{0}='{1}'", m_ewMap.VIMember, strCatValue));
                        if (drs != null && drs.Length > 0)
                        {
                            int.TryParse(drs[0]["COUNT"].ToString(), out iCnt);
                        }
                    }
                    fpSpread1_Sheet1.Cells[i, 2].Value = iCnt;
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveData();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butReset_Click(object sender, EventArgs e)
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void frmVisualInspection_Load(object sender, EventArgs e)
        {

            try
            {
                if (DesignMode) return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            try
            {
                if (wafer.Length != 1)
                {
                    MessageBox.Show(this, "Set only One wafer.", this.Name);
                    return;
                }

                OCurrentWafer = wafer[0];
                m_TestArea = OCurrentWafer.Testarea;
                m_Product = OCurrentWafer.Product;
                m_LotID = OCurrentWafer.LotID;
                m_WaferID = OCurrentWafer.WaferID;
                m_Program = OCurrentWafer.Program;

                DrawTESTMap(DACrux.Base.Convert.longParse(wafer[0].WaferSeq));
                //DrawWaferMap(OCurrentWafer.Testarea, OCurrentWafer.Product, OCurrentWafer.LotID, OCurrentWafer.WaferID, OCurrentWafer.Program);

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
