using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Utility;
using System.Collections;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using DACrux.Common.RO;

namespace DACrux.TEST.ENGUI
{
    public partial class frmAVICumMapView : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl, DACrux.Framework.Base.IExportExcel
    {
        enum CUM_CNT { FROM, TO, COLOR }

        private string m_strPrduct = string.Empty;
        private string m_strProgram = string.Empty;
        private long[] m_strWaferSeqs = null;
        private DataSet m_ds = null;
        private string strConfigFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "DM", "AviCumCount");

        private string[] strHeaderBase = new string[] { "DEVICE_ALIAS", "PRODUCT", "TESTAREA", "PROGRAM", "LOT_ID", "WAFER_ID", "START_TIME", "END_TIME", "YIELD", "TESTED_DIE", "DEFECT_RATE", "DEFECT_CNT" };

        public frmAVICumMapView()
        {
            InitializeComponent();
        }

        private void frmAVICumMapView_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;


            FileInfo oConfigFile = new FileInfo(strConfigFullPath);
            if (oConfigFile.Exists)
            {
                DataSet dsFile = new DataSet();
                dsFile.ReadXml(oConfigFile.FullName);
                if (dsFile.Tables.Count > 0)
                    SetCumCountColor(dsFile.Tables[0]);
            }
            else
            {
                SetCumCountColor(GetInitCumColor());
            }
        }

        #region User Method

        private void ControlAdd(DACrux.TEST.Control.TPUImageInfo container)
        {
            if (AVIImageList.InvokeRequired)
            {
                AVIImageList.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        ControlAdd(container);
                    }
                ));
            }
            else
            {
                if (container == null) return;

                container.OnSelectDieIndex += new TEST.Control.SelectDieIndex(SelectDieIndex);

                container.Height = container.Height + container.Margin.Top;
                container.Width = AVIImageList.Width - SystemInformation.VerticalScrollBarWidth - container.Margin.Left - container.Margin.Right;
                AVIImageList.Controls.Add(container);
                AVIImageList.ResumeLayout();
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

                m_wMap.Reset();
                m_wMap.WaferColor = Color.White;
                m_wMap.Redraw();

                oPRBMapAnalysis = new RO.ProbeMapAnalysis();

                m_ds = oPRBMapAnalysis.SelectAVICumData(m_strWaferSeqs);
                if (m_ds == null || m_ds.Tables.Count <= 0)
                    throw new Exception("Not Found Data");

                fnCumSheetSet();

            }
            finally
            {
            }
        }

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            bool bFlag = true;
            long[] Wafers = new long[wafer.Length];
            for (int i = 0; i < wafer.Length; i++)
            {
                if (!base.RESV_03.Contains(wafer[i].Testarea))
                {
                    bFlag = false;
                    break;
                }
                Wafers[i] = Convert.ToInt32(wafer[i].WaferSeq);
            }

            if (!bFlag)
                return;

            DrawCummap(Wafers);
            this.TPWaferList = wafer;
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



        private void fnCumSheetSet()
        {
            List<string> lsHeader = new List<string>();
            try
            {
                Utility.FPSpreadUtil.InitSpread(fpSpread1);

                if (m_ds.Tables.IndexOf("BIN_INFO") < 0 || m_ds.Tables["BIN_INFO"].Rows.Count <= 0)
                    return;

                if (m_ds.Tables.IndexOf("WAFER_INFO") < 0 || m_ds.Tables["WAFER_INFO"].Rows.Count <= 0)
                    return;

                fnWaferMapDraw();

                lsHeader.AddRange(strHeaderBase);
                for (int iBin = 0; iBin < m_ds.Tables["BIN_INFO"].Rows.Count; iBin++)
                {
                    lsHeader.Add(string.Format("BIN{0}", m_ds.Tables["BIN_INFO"].Rows[iBin]["BIN"]));
                }

                DataTable dtRowData = m_ds.Tables["WAFER_INFO"].DefaultView.ToTable(false, lsHeader.ToArray()).Copy();

                //Sheet 상에 Data Bind
                DACrux.Utility.FPSpreadUtil.InitSpread(fpSpread1);
                fpSpread1.ActiveSheet.DataSource = dtRowData;

                //Header의 정보를 Bin Desc 정보로 치환해준다.
                FarPoint.Win.Spread.CellType.NumberCellType BinCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
                BinCellType.DecimalPlaces = 0;

                BinCellType.Separator = ",";

                //Sheet 상의 Columns 명을 DESC 로 변경 한다.
                for (int ic = 0; ic < fpSpread1.ActiveSheet.Columns.Count; ic++)
                {
                    string strColumnsName = fpSpread1.ActiveSheet.Columns[ic].Label;

                    if (strColumnsName.StartsWith("BIN") == true || strColumnsName.StartsWith("YLD_BIN") == true)
                    {
                        string strBinNo = string.Empty;

                        if (strColumnsName.StartsWith("YLD_BIN") == true)
                            strBinNo = strColumnsName.Replace("YLD_BIN", "");
                        else
                            strBinNo = strColumnsName.Replace("BIN", "");

                        if (m_ds.Tables.IndexOf("BIN_INFO") > -1)
                        {
                            //정의된 Bin 정보가 없을 경우 보여 주지 않는다.
                            DataRow[] drHeader = m_ds.Tables["BIN_INFO"].Select(string.Format("BIN = '{0}'", strBinNo));

                            if (drHeader.Length > 0)
                            {
                                strColumnsName = string.Format("{0}({1})", drHeader[0]["BIN"], drHeader[0]["BIN_NAME"]);
                                fpSpread1.ActiveSheet.Columns[ic].Label = strColumnsName;
                                fpSpread1.ActiveSheet.Columns[ic].CellType = BinCellType;
                            }
                            else
                                fpSpread1.ActiveSheet.Columns[ic].Visible = false;
                        }
                    }
                    else if (strColumnsName == "DEFECT_CNT" || strColumnsName == "TESTED_DIE")
                        fpSpread1.ActiveSheet.Columns[ic].CellType = BinCellType;
                }

                //Column 을 Remove 해야 Excel Export 시 Data 가 없는 Column 이 안보인다.
                for (int ic = fpSpread1.ActiveSheet.Columns.Count; ic <= 0; ic--)
                {
                    if (fpSpread1.ActiveSheet.Columns[ic].Visible == false)
                        fpSpread1.ActiveSheet.Columns[ic].Remove();
                }

                fpSpread1.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;
                fpSpread1.ActiveSheet.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
                fpSpread1.ActiveSheet.Models.Selection.ClearSelection();
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1.ActiveSheet);

            }
            finally
            {

            }
        }

        private void fnWaferMapDraw()
        {
            int iFrom = -1;
            int iTo = -1;
            Color crColor = Color.Empty;
            MainForm.SetStatusMessage("조회를 시작 합니다.");
            try
            {
                m_wMap.Reset();
                m_wMap.WaferColor = Color.White;
                m_wMap.Redraw();

                if (m_ds.Tables.IndexOf("MAPDATA") < 0 || m_ds.Tables["MAPDATA"].Rows.Count <= 0)
                    return;

                //Wafer Map Set
                /// Color Depth ====================================================================
                for (int i = 0; i <= m_strWaferSeqs.Length; i++)
                {
                    Color oColor = Color.FromArgb((255 / (m_strWaferSeqs.Length + 1)) * i
                                   , 255
                                   , (255 / (i + 2))
                                   , (255 / (i + 2)));


                    m_wMap.SetColor(i, oColor);
                }

                // dgCumColor에 설정되어 있는 색상으로 표시
                foreach (DataGridViewRow dRow in dgCumCount.Rows)
                {
                    if (dRow.IsNewRow)
                        continue;

                    if (int.TryParse(dRow.Cells[(int)CUM_CNT.FROM].Value.ToString(), out iFrom) == false)
                        continue;

                    if (int.TryParse(dRow.Cells[(int)CUM_CNT.TO].Value.ToString(), out iTo) == false)
                        continue;

                    crColor = ColorTranslator.FromHtml(dRow.Cells[(int)CUM_CNT.COLOR].Value.ToString());

                    if (iFrom > iTo)
                        throw new Exception("Form 값이 To 값보다 큽니다.");

                    //Bin(갯수) 별로 색상을 재정의 한다.
                    for (int ic = iFrom; ic <= iTo; ic++)
                    {
                        m_wMap.SetColor(ic, crColor);
                    }
                }

                //Redraw 시 마다 File 상에 저장 해놓고 Form Load 시 불러 온다.
                if (dgCumCount.Rows.Count > 1)
                {
                    FileInfo oConfigFile = new FileInfo(strConfigFullPath);
                    if (oConfigFile != null && oConfigFile.Exists)
                    {
                        oConfigFile.Delete();
                    }

                    DirectoryInfo oDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "DM"));
                    if (oDir.Exists == false)
                        oDir.Create();

                    DataTable dtWrite = (DataTable)dgCumCount.DataSource;
                    dtWrite.WriteXml(oConfigFile.FullName);
                }

                /// Recipe===============================================================================

                m_wMap.VisibleDieValue = true;
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

                m_wMap.VisibleDieBorder = true;
                m_wMap.DieBorderColor = Color.Black;
                m_wMap.WaferColor = Color.DimGray;

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

                m_wMap.DataSource = m_ds.Tables["MAPDATA"].Copy();
                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                m_wMap.Focus();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                MainForm.SetStatusMessage(null);
            }
        }

        /// <summary>
        /// AVI Image 정보를 기준으로 가져온다.
        /// </summary>
        /// <param name="selectedDies">null 일 경우 전체 조회</param>
        private void fnAVIMapGathering(List<Point> selectedDies)
        {
            ComConfiguration obj = null;

            DirectoryInfo oDirectory = null;

            string strFTPIP = string.Empty;
            string strFTPPort = string.Empty;
            string strFTPID = string.Empty;
            string strFTPPass = string.Empty;

            string strFTPFullPath = string.Empty;
            string strLocalPathName = string.Empty;
            string strLocalFullPath = string.Empty;

            string strFileName = string.Empty;
            string strFilePath = string.Empty;

            string strWaferID = string.Empty;
            string strProgram = string.Empty;
            string strDevice = string.Empty;

            int iCount = 0;

            DataTable dtImages = null;
            try
            {
                AVIImageList.Controls.Clear();
                AVIImageList.Refresh();

                if (m_ds.Tables.Contains("IMAGES") == true && m_ds.Tables["IMAGES"].Rows.Count > 0)
                    dtImages = m_ds.Tables["IMAGES"].Copy();
                else if (m_ds.Tables.Contains("MAPDATA_IMAGES") == true && m_ds.Tables["MAPDATA_IMAGES"].Rows.Count > 0)
                    dtImages = m_ds.Tables["MAPDATA_IMAGES"].Copy();
                else
                    return;

                if (dtImages.Rows.Count <= 0 || dtImages.Columns.Contains("IMAGE_PATH") == false)
                    return;

                MainForm.SetStatusMessage("AVI Image를 Download 받는 중입니다.");
                obj = new ComConfiguration();
                DataTable dtConfig = obj.GetAVIImageFTPInfo();
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                    return;

                strFTPIP = dtConfig.Rows[0]["IP"].ToString();
                strFTPPort = dtConfig.Rows[0]["PORT"].ToString();
                strFTPID = dtConfig.Rows[0]["ID"].ToString();
                strFTPPass = dtConfig.Rows[0]["PASS"].ToString();

                strLocalPathName = System.IO.Path.Combine(Environment.CurrentDirectory, "AVI", this.Name);
                oDirectory = new DirectoryInfo(strLocalPathName);
                if (oDirectory.Exists)
                    oDirectory.Delete(true);

                oDirectory.Create();

                // selectedDies 가 null 인 경우 전체 View 
                if (selectedDies == null)
                {
                    try
                    {
                        DataRow[] odies = dtImages.Select("IMAGE_PATH <> '' ");
                        foreach (DataRow dr in odies)
                        {
                            strFileName = dr["IMAGE_FILE_NAME"].ToString();
                            strFilePath = dr["IMAGE_PATH"].ToString();
                            strWaferID = dr["WAFER_ID"].ToString();
                            strProgram = dr["PROGRAM"].ToString();
                            strDevice = dr["DEVICE_ALIAS"].ToString();

                            strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFileName);

                            if (strFilePath.StartsWith("BACKUP") == true)
                                strFTPFullPath = System.IO.Path.Combine(strFilePath, strFileName).Replace("\\", "/");
                            else
                                strFTPFullPath = System.IO.Path.Combine("BACKUP/", strFilePath, strFileName).Replace("\\", "/");

                            DACrux.TEST.Control.TPUImageInfo oForm = new TEST.Control.TPUImageInfo(
                                  strFTPFullPath,
                                  strLocalFullPath,
                                  dr["X"].ToString(),
                                  dr["Y"].ToString(),
                                  dr["BIN"].ToString(),
                                  strWaferID,
                                  strProgram,
                                  strDevice,
                                  strFTPIP,
                                  strFTPPort,
                                  strFTPID,
                                  strFTPPass);

                            ControlAdd(oForm);

                            MainForm.SetStatusMessage(string.Format("AVI Image를 Download 받는 중입니다. ({0}/{1})", iCount, odies.Length));
                            iCount++;
                        }
                    }
                    catch { }
                }
                else
                {
                    foreach (Point inPt in selectedDies)
                    {
                        //Map Data 에 IMAGE_PATH Columns 정보가 있을 경우 AVI Map 으로 처리 한다.
                        try
                        {
                            DataRow[] odies = dtImages.Select(string.Format("X = {0} AND Y = {1} AND IMAGE_PATH <> '' ", inPt.X, inPt.Y));
                            foreach (DataRow dr in odies)
                            {
                                strFileName = dr["IMAGE_FILE_NAME"].ToString();
                                strFilePath = dr["IMAGE_PATH"].ToString();
                                strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFileName);
                                strWaferID = dr["WAFER_ID"].ToString();
                                strProgram = dr["PROGRAM"].ToString();
                                strDevice = dr["DEVICE_ALIAS"].ToString();

                                if (strFilePath.StartsWith("BACKUP") == true)
                                    strFTPFullPath = System.IO.Path.Combine(strFilePath, strFileName).Replace("\\", "/");
                                else
                                    strFTPFullPath = System.IO.Path.Combine("BACKUP/", strFilePath, strFileName).Replace("\\", "/");

                                DACrux.TEST.Control.TPUImageInfo oForm = new TEST.Control.TPUImageInfo(
                                      strFTPFullPath,
                                      strLocalFullPath,
                                      dr["X"].ToString(),
                                      dr["Y"].ToString(),
                                      dr["BIN"].ToString(),
                                      strWaferID,
                                      strProgram,
                                      strDevice,
                                      strFTPIP,
                                      strFTPPort,
                                      strFTPID,
                                      strFTPPass);

                                ControlAdd(oForm);
                            }
                        }
                        catch { }

                        MainForm.SetStatusMessage(string.Format("AVI Image를 Download 받는 중입니다. ({0}/{1})", iCount, selectedDies.Count));
                        iCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtImages != null)
                    dtImages.Dispose();
                dtImages = null;

                AVIImageList.Refresh();
                MainForm.SetStatusMessage(null);
            }
        }

        private DataTable GetInitCumColor()
        {
            DataTable dtCumConfig = new DataTable("CUM_CNT");
            dtCumConfig.Columns.Add(new DataColumn(CUM_CNT.FROM.ToString(), typeof(int)));
            dtCumConfig.Columns.Add(new DataColumn(CUM_CNT.TO.ToString(), typeof(int)));
            dtCumConfig.Columns.Add(new DataColumn(CUM_CNT.COLOR.ToString(), typeof(string)));

            DataRow drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 1;
            drItem[(int)CUM_CNT.TO] = 2;
            drItem[(int)CUM_CNT.COLOR] = "#FFCAE5";
            dtCumConfig.Rows.Add(drItem);

            drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 3;
            drItem[(int)CUM_CNT.TO] = 4;
            drItem[(int)CUM_CNT.COLOR] = "#FFAED7";
            dtCumConfig.Rows.Add(drItem);

            drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 5;
            drItem[(int)CUM_CNT.TO] = 6;
            drItem[(int)CUM_CNT.COLOR] = "#FF77BC";
            dtCumConfig.Rows.Add(drItem);

            drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 7;
            drItem[(int)CUM_CNT.TO] = 8;
            drItem[(int)CUM_CNT.COLOR] = " #FF48A5";
            dtCumConfig.Rows.Add(drItem);

            drItem = dtCumConfig.NewRow();
            drItem[(int)CUM_CNT.FROM] = 9;
            drItem[(int)CUM_CNT.TO] = 100;
            drItem[(int)CUM_CNT.COLOR] = "#FF0081";
            dtCumConfig.Rows.Add(drItem);
            dtCumConfig.AcceptChanges();

            return dtCumConfig;
        }


        private void SelectDieIndex(Point oIndex)
        {
            try
            {
                m_wMap.SelDie(oIndex.X, oIndex.Y);
                //m_wMap.SetSelectedDie(oIndex.X, oIndex.Y);
                //m_wMap.Focus();
                //m_wMap.SetFocusDie(oIndex.X, oIndex.Y);
                //m_wMap.Focus();
                m_wMap.Focus();
            }
            finally
            {
            }
        }

        private void SetCumCountColor(DataTable dtConfig)
        {
            dgCumCount.DataSource = dtConfig;

            dgCumCount.Columns[0].HeaderText = CUM_CNT.FROM.ToString();
            dgCumCount.Columns[1].HeaderText = CUM_CNT.TO.ToString();
            dgCumCount.Columns[2].HeaderText = CUM_CNT.COLOR.ToString();

            dgCumCount.Columns[0].Width = 60;
            dgCumCount.Columns[1].Width = 60;
            dgCumCount.Columns[2].Width = 80;

            dgCumCount.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgCumCount.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgCumCount.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgCumCount.Columns[0].ReadOnly = false;
            dgCumCount.Columns[1].ReadOnly = false;
            dgCumCount.Columns[2].ReadOnly = true;

            foreach (DataGridViewRow dRow in dgCumCount.Rows)
            {
                if (dRow.IsNewRow == true)
                    continue;

                Color DefectCNTColor = ColorTranslator.FromHtml(dRow.Cells[(int)CUM_CNT.COLOR].Value.ToString());
                dRow.DefaultCellStyle.BackColor = DefectCNTColor;
            }

            dgCumCount.Refresh();

            Application.DoEvents();
        }



        #endregion User Method

        #region Event Handler

        private void aVIImageAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("AVI 전체 Image 를 가져오겠습니까?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                fnAVIMapGathering(null);

            }
        }

        private void btnRedraw_Click(
            object sender, 
            EventArgs e
            )
        {
            fnWaferMapDraw();
        }

        private void dgCumCount_CellDoubleClick(
            object sender, 
            DataGridViewCellEventArgs e
            )
        {
            if (e.ColumnIndex == (int)CUM_CNT.COLOR)
            {
                ColorDialog ColorDLG = new ColorDialog();

                if (ColorDLG.ShowDialog() == DialogResult.OK)
                {
                    dgCumCount.Rows[e.RowIndex].Cells[2].Value = ColorTranslator.ToHtml(ColorDLG.Color);
                    dgCumCount.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorDLG.Color;
                }
            }
        }

        private void m_wMap_OnSelectDies(
            object sender,
            List<Point> selectedDies
            )
        {
            try
            {
                fnAVIMapGathering(selectedDies);
            }
            finally
            {
                AVIImageList.Refresh();
            }
        }

        private void m_wMap_OnChangeCurrentDie(
            object sender,
            Base.Die NewDie
            )
        {
            try
            {
                txtXIndex.Text = NewDie.IndexX.ToString();
                txtYIndex.Text = NewDie.IndexY.ToString();
                txtBin.Text = NewDie.BinNumber.ToString();
            }
            finally
            {
            }
        }

        private void m_wMap_OnSelectDies(object sender)
        {
        }

        private void rbtType_Click(object sender, EventArgs e)
        {
            fnCumSheetSet();
        }

        #endregion Event Handler

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add((DataTable)fpSpread1.DataSource);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "DataRow";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(m_wMap);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Map n Chart";

            sheet = new DACrux.Utility.ExcelSheet();
            List<System.Windows.Forms.Control> lsImages = new List<System.Windows.Forms.Control>();
            foreach (System.Windows.Forms.Control cr in AVIImageList.Controls)
            {
                if (cr.Name == "TPUImageInfo")
                    lsImages.Add(cr);
            }

            sheet.Add();
            if (lsImages.Count > 0)
                sheet.Add(lsImages.ToArray());

            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "AVI Image";

            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion
    }
}
