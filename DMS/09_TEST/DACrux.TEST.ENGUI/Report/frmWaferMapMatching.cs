using System;
using System.Data;
using System.Windows.Forms;
using DACrux.Framework.Base;
using DACrux.Base;
using System.Drawing;
using DACrux.Utility;
using System.Collections.Generic;
using System.Diagnostics;

namespace DACrux.TEST.ENGUI
{
    public partial class frmWaferMapMatching
        : DACruxUXBasic01, IExportExcel, DACrux.TEST.Interface.iTESTControl
    {
        #region [ Constructor ]

        private DataTable dtTestMapData = null;    //전제 Test Bin
        private DataTable dtBinDefectList = null;  //Bin 별 Defect 정의
        private DataTable dtWaferIndexList = null; //Filter 된 Test Bin
        //private DataTable dtDefectList = null;     //전체 Defect List
        private DataSet dsTestWaferMap = null;
        private DataSet dsDefectWafer = null;
        private DefectList defectList = null;

        private string LOT_ID = string.Empty;
        private string PROGRAM = string.Empty;
        private string DEVICE = string.Empty;
        private string TESTAREA = string.Empty;

        private int shiftX = 0;
        private int shiftY = 0;
        private int iAlterAngle = 0;

        private List<int> oGoodBin = new List<int>();

        private DACrux.Base.WaferRecipe oWaferRecipe = new DACrux.Base.WaferRecipe();
        private List<BinColor> m_lstBinColor = null;

        private enum MAIN_TAB { SEARCH = 0, FILTER, INFORMATION }
        private enum INFORMATION_TAB { DM = 0, CP, AVI, FOI }
        private Color dieBackColor;
        private int DisplayFlatAngle = -1;

        public struct BinColor
        {
            public int bin;
            public Color color;

            public BinColor(int inBin, Color inColor)
            {
                bin = inBin;
                color = inColor;
            }
        }

        //--

        public frmWaferMapMatching(
            )
        {
            InitializeComponent();
        }

        //--

        #endregion [ Constructor ]

        //----------------------------------------------------------------

        #region [ Event Handler ]

        //--

        private void frmWaferMapMatching_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            Application.DoEvents();

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWaferRecipe(WaferList);
            }

            //--

            Utility.FPSpreadUtil.InitSpread(fpsDefect);
            Utility.FPSpreadUtil.InitSpread(fpsCp);
            Utility.FPSpreadUtil.InitSpread(fpsAvi);
            Utility.FPSpreadUtil.InitSpread(fpsFoi);
        }

        private void lsvBin_MouseClick(object sender, MouseEventArgs e)
        {
            DrawMap();
        }

        private void lsvLayer_MouseClick(object sender, MouseEventArgs e)
        {
            /// Defect 에 대한 정보를 따로 가져오기 위해서 변경
            /// 전체 Defect List 를 가져오는 경우 System.OutofMemory 발생
            SEMDMS.RO.DefectMapAnalysis obj = new SEMDMS.RO.DefectMapAnalysis();
            long[] steps = new long[lsvLayer.SelectedItems.Count];
            for (int idx = 0; idx < lsvLayer.SelectedItems.Count; idx++)
            {
                ListViewItem item = lsvLayer.SelectedItems[idx];
                if (item == null || item.SubItems.Count <= 0)
                    continue;

                steps[idx] = Base.Convert.longParse(item.SubItems[1].Text);
            }
            defectList = obj.GetDefectData(steps);
            List<int> classNumbers = new List<int>();
            List<double> defectSize = new List<double>();
            foreach (Defect d in defectList)
            {
                int idx = classNumbers.BinarySearch(d.CLASSNUMBER);
                if (idx < 0)
                    classNumbers.Insert(~idx, d.CLASSNUMBER);

                idx = defectSize.BinarySearch(d.DSIZE);
                if (idx < 0)
                    defectSize.Insert(~idx, d.DSIZE);
            }

            DataTable dtDefectType = dsDefectWafer.Tables["DEFECT_COLOR"];
            lsvDefectType.Items.Clear();
            lsvDefectType.Items.Add("ALL");
            for (int idx = 0; idx < classNumbers.Count; idx++)
            {
                ListViewItem item = lsvDefectType.Items.Add(classNumbers[idx].ToString());
                DataRow[] rows = dtDefectType.Select(String.Format("[CLASSNUMBER] = '{0}'", classNumbers[idx]));
                if (rows == null || rows.Length <= 0)
                    item.SubItems.Add(classNumbers[idx].ToString());
                else item.SubItems.Add(rows[0]["NAME"].ToString());
            }

            double dMin = defectSize[0];
            double dMax = defectSize[defectSize.Count - 1];
            double dDiff = (dMax - dMin) / 10;
            lsvDefectSize.Items.Clear();
            lsvDefectSize.Items.Add("ALL");
            for (int idx = 0; idx < 10; idx++)
            {
                ListViewItem item = lsvDefectSize.Items.Add(dMin.ToString());
                double dValue = dMin + dDiff;
                item.SubItems.Add(dValue.ToString());
                dMin = dValue;
            }

            DrawMap();
        }

        private void lsvDefectType_MouseClick(object sender, MouseEventArgs e)
        {
            DrawMap();
        }

        private void lsvDefectSize_MouseClick(object sender, MouseEventArgs e)
        {
            DrawMap();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            shiftX++;
            switch (matchMap.ViewAngle)
            {
                case 0:
                    if (!int.TryParse(txtChangeXIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeXIndex.Text = (iIndex + 1).ToString();
                    break;
                case 90:
                    if (!int.TryParse(txtChangeYIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeYIndex.Text = (iIndex + 1).ToString();
                    break;
                case 180:
                    if (!int.TryParse(txtChangeXIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeXIndex.Text = (iIndex - 1).ToString();
                    break;
                case 270:
                    if (!int.TryParse(txtChangeYIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeYIndex.Text = (iIndex - 1).ToString();
                    break;

            }
            DrawMap();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            shiftY++;
            switch (matchMap.ViewAngle)
            {
                case 0:
                    if (!int.TryParse(txtChangeYIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeYIndex.Text = (iIndex + 1).ToString();
                    break;
                case 90:
                    if (!int.TryParse(txtChangeXIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeXIndex.Text = (iIndex - 1).ToString();
                    break;
                case 180:
                    if (!int.TryParse(txtChangeYIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeYIndex.Text = (iIndex - 1).ToString();
                    break;
                case 270:
                    if (!int.TryParse(txtChangeXIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeXIndex.Text = (iIndex + 1).ToString();
                    break;

            }

            DrawMap();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            shiftX--;
            switch (matchMap.ViewAngle)
            {
                case 0:
                    if (!int.TryParse(txtChangeXIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeXIndex.Text = (iIndex - 1).ToString();
                    break;
                case 90:
                    if (!int.TryParse(txtChangeYIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeYIndex.Text = (iIndex - 1).ToString();
                    break;
                case 180:
                    if (!int.TryParse(txtChangeXIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeXIndex.Text = (iIndex + 1).ToString();
                    break;
                case 270:
                    if (!int.TryParse(txtChangeYIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeYIndex.Text = (iIndex + 1).ToString();
                    break;

            }

            DrawMap();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            shiftY--;
            switch (matchMap.ViewAngle)
            {
                case 0:
                    if (!int.TryParse(txtChangeYIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeYIndex.Text = (iIndex - 1).ToString();
                    break;
                case 90:
                    if (!int.TryParse(txtChangeXIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeXIndex.Text = (iIndex + 1).ToString();
                    break;
                case 180:
                    if (!int.TryParse(txtChangeYIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeYIndex.Text = (iIndex + 1).ToString();
                    break;
                case 270:
                    if (!int.TryParse(txtChangeXIndex.Text, out iIndex))
                        iIndex = 0;

                    txtChangeXIndex.Text = (iIndex - 1).ToString();
                    break;
            }

            DrawMap();
        }

        private void PicWafer_Click(object sender, EventArgs e)
        {
            MatchWaferDraw(this.TPWaferList);
        }

        private void ImageList_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                ImageList.SuspendLayout();
                foreach (System.Windows.Forms.Control ctrl in ImageList.Controls)
                {

                    ctrl.Height = ctrl.Height + (ctrl.Margin.Top + ctrl.Margin.Bottom);
                    ctrl.Width = ImageList.Width - 30;
                }
                ImageList.ResumeLayout();
            }
            catch (Exception) { }
        }

        private void MatchMap_OnSelectedDefect(object sender, Defect[] oDefect)
        {
            DACrux.SEMDMS.Control.DPUCDefectInfo oForm = null;

            try
            {
                if (chkDefectImage.Checked == false)
                    return;

                if (ImageList.Controls.Count > 0)
                    ImageList.Controls.Clear();

                for (int i = 0; i < oDefect.Length; i++)
                {
                    if (oDefect[i].Images != null && oDefect[i].Images.Count > 0)
                    {
                        System.Threading.Thread.Sleep(10);


                        foreach (DefectImage image in oDefect[i].Images)
                        {
                            oForm = new DACrux.SEMDMS.Control.DPUCDefectInfo(
                                oDefect[i].DEFECTID.ToString(),
                                image.IMAGEPATH,
                                oDefect[i].XINDEX.ToString(),
                                oDefect[i].YINDEX.ToString(),
                                oDefect[i].CLASSNUMBER.ToString(),
                                oDefect[i].CLUSTERNUMBER.ToString(),
                                oDefect[i].XREL.ToString(),
                                oDefect[i].YREL.ToString(),
                                oDefect[i].XSIZE.ToString(),
                                oDefect[i].YSIZE.ToString());

                            ControlAdd(oForm);
                        }

                        MainForm.SetStatusMessage(string.Format("Defect Image 확인중..({0}/{1})", i, oDefect.Length));
                    }
                }

                ImageList.Refresh();
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void MatchMap_OnChangeCurrentDie(object sender, Die NewDie)
        {
            ToolWaferDieLocation.Text = string.Format("X : {0} Y : {1}", NewDie.IndexX, NewDie.IndexY);

            Defect[] defects = matchMap.Defects.GetDefectInDie(NewDie.IndexX, NewDie.IndexY);
            ToolParticles.Text = string.Format("Particles : {0}", defects.Length);
        }

        private void chkFailBinDefect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFailBinDefect.Checked)
            {
            }

        }

        private void BtnLeftRotation_Click(object sender, EventArgs e)
        {
            switch (TxtChangeFlatZone.Text)
            {
                case "270":
                    TxtChangeFlatZone.Text = "0";
                    break;
                case "90":
                    TxtChangeFlatZone.Text = "180";
                    break;
                case "180":
                    TxtChangeFlatZone.Text = "270";
                    break;
                default: TxtChangeFlatZone.Text = "90";
                    break;
            }

            iAlterAngle = int.Parse(TxtChangeFlatZone.Text);

            DrawMap();
        }

        private void cmbDir_ValueChanged(object sender, EventArgs e)
        {
            DrawMap();
        }


        #endregion [ Event Handler ]

        //----------------------------------------------------------------

        #region [ Method ]

        //--

        public void ExportExcel()
        {
            ExcelSheet sheet1 = new ExcelSheet();
            sheet1.Add(matchMap);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet1);

            ExcelExportManager.Export(e);
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

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            try
            {
                if (wafer.Length > 1)
                {
                    throw new Exception("Only One Wafer");
                }

                this.TPWaferList = wafer;

                MatchWaferDraw(wafer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void MatchWaferDraw(DACrux.Base.TPWafer[] wafer)
        {

            DACrux.TEST.RO.TestCommon oTestCommon = null;

            string strLotID = string.Empty;
            string strSlotID = string.Empty;

            DataTable dtTemp = null;

            try
            {
                if (dsTestWaferMap != null)
                    dsTestWaferMap.Dispose();

                if (dsDefectWafer != null)
                    dsDefectWafer.Dispose();

                txtChangeXIndex.Text = "0";
                txtChangeYIndex.Text = "0";
                TxtChangeFlatZone.Text = "0";

                shiftX = 0;
                shiftY = 0;
                iAlterAngle = 0;

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                oTestCommon = new DACrux.TEST.RO.TestCommon();

                //Service 상에서 Test Wafer Map Gathering
                dsTestWaferMap = oTestCommon.SelectWaferMapBasic(long.Parse(wafer[0].WaferSeq));

                strLotID = dsTestWaferMap.Tables["WAFER_INFO"].Rows[0]["LOT_ID"].ToString();
                strSlotID = dsTestWaferMap.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString().Split(new char[] { '-', '_', '@' }, StringSplitOptions.RemoveEmptyEntries)[1];

                //Service 상에서 DM Wafer Map Gathering
                dsDefectWafer = oTestCommon.GetMatchDefectData(strLotID, strSlotID, DACrux.Base.GlobalVariable.UserID);

                dtTestMapData = dsTestWaferMap.Tables["MAPDATA"].Copy();

                //Bin 기준 정보 List Up
                dsTestWaferMap.Tables["MAPDATA"].DefaultView.Sort = "BIN ASC";
                dtTemp = dsTestWaferMap.Tables["MAPDATA"].DefaultView.ToTable(true, "BIN");

                FillListViewItem(
                    lsvBin,
                    dtTemp,
                    true,
                    "BIN",
                    "BIN"
                    );

                //Step List Up
                dtTemp = dsDefectWafer.Tables["STEP_INFO"];
                dtTemp = dtTemp.DefaultView.ToTable(true, "STEP_ID", "STEP_SEQ");

                FillListViewItem(
                    lsvLayer,
                    dtTemp,
                    false,
                    "STEP_ID",
                    "STEP_SEQ"
                    );

                DrawMap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void DrawMap()
        {
            List<string> oList = new List<string>();
            DataTable dtWaferIndexListShift = null;
            //DataTable dtTestMapDataShift = null;

            try
            {
                matchMap.DieClear();
                matchMap.DefectClear();
                oGoodBin = new List<int>();

                if (ImageList.Controls.Count > 0)
                    ImageList.Controls.Clear();

                if (dsTestWaferMap == null || dsDefectWafer == null)
                    return;

                DrawDescription(0, 0, 0);

                MainForm.SetStatusMessage("Map을 그리는 중입니다.");

                //Bin Filter
                if (lsvBin.SelectedItems.Count > 0 && !FindItem(lsvBin.SelectedItems, "ALL"))
                {
                    oList = new List<string>();
                    foreach (ListViewItem Item in lsvBin.SelectedItems)
                    {
                        oList.Add(Item.SubItems[0].Text.ToString());
                    }

                    if (dsTestWaferMap.Tables["MAPDATA"].Select(string.Format("BIN IN ({0})", string.Join(",", oList.ToArray()))).Length <= 0)
                        return;

                    dtWaferIndexList = dsTestWaferMap.Tables["MAPDATA"].Select(string.Format("BIN IN ({0})", string.Join(",", oList.ToArray()))).CopyToDataTable<DataRow>();
                }
                else
                    dtWaferIndexList = dsTestWaferMap.Tables["MAPDATA"].Copy();

                dtWaferIndexListShift = dtWaferIndexList.Copy();
                dtTestMapData = dsTestWaferMap.Tables["MAPDATA"].Copy();

                string dir = cmbDir.Text;
                //Wafer Die 를 Update 한다.
                if (shiftX != 0 || shiftY != 0 || iAlterAngle != 0 || !string.Equals(dir, "LL"))
                {
                    int iXMax = System.Convert.ToInt32(dtTestMapData.Compute("MAX(X)", ""));
                    int iXMin = System.Convert.ToInt32(dtTestMapData.Compute("MIN(X)", ""));
                    int iYMax = System.Convert.ToInt32(dtTestMapData.Compute("MAX(Y)", ""));
                    int iYMin = System.Convert.ToInt32(dtTestMapData.Compute("MIN(Y)", ""));

                    int indexX = 0;
                    int indexY = 0;

                    foreach (DataRow drAlter in dtWaferIndexListShift.Rows)
                    {
                        //변경 시키는 Map 관련 정보 기반 좌표 이동
                        if (!int.TryParse(drAlter["X"].ToString(), out indexX))
                            continue;

                        if (!int.TryParse(drAlter["Y"].ToString(), out indexY))
                            continue;

                        // 1) Swap
                        switch (dir)
                        {
                            case "LL":
                                break;
                            case "TL":
                                indexY = (iYMax + iYMin - indexY);
                                break;
                            case "TR":
                                indexX = (iXMax + iXMin - indexX);
                                indexY = (iYMax + iYMin - indexY);
                                break;
                            case "LR":
                                indexX = (iXMax + iXMin - indexX);
                                break;
                        }


                        Rotate(iAlterAngle, iXMin, iXMax, iYMin, iYMax, ref indexX, ref indexY);

                        drAlter["X"] = indexX + shiftX;
                        drAlter["Y"] = indexY + shiftY;

                    }

                    dtWaferIndexListShift.AcceptChanges();
                    dtWaferIndexList = dtWaferIndexListShift.Copy();

                    //=====================================================================================================================================

                    iXMax = System.Convert.ToInt32(dtTestMapData.Compute("MAX(X)", ""));
                    iXMin = System.Convert.ToInt32(dtTestMapData.Compute("MIN(X)", ""));
                    iYMax = System.Convert.ToInt32(dtTestMapData.Compute("MAX(Y)", ""));
                    iYMin = System.Convert.ToInt32(dtTestMapData.Compute("MIN(Y)", ""));

                    indexX = 0;
                    indexY = 0;

                    foreach (DataRow drAlter in dtTestMapData.Rows)
                    {
                        //변경 시키는 Map 관련 정보 기반 좌표 이동
                        if (!int.TryParse(drAlter["X"].ToString(), out indexX))
                            continue;

                        if (!int.TryParse(drAlter["Y"].ToString(), out indexY))
                            continue;

                        Rotate(iAlterAngle, iXMin, iXMax, iYMin, iYMax, ref indexX, ref indexY);

                        drAlter["X"] = indexX + shiftX;
                        drAlter["Y"] = indexY + shiftY;
                    }

                    dtTestMapData.AcceptChanges();
                }

                //Test Wafer Map Draw
                TestMapDraw(dsTestWaferMap);

                ////Defect Type Filter
                DefectList filterDefectList = new DefectList();
                if (lsvDefectType.SelectedItems.Count > 0 && !FindItem(lsvDefectType.SelectedItems, "ALL"))
                {
                    oList = new List<string>();
                    foreach (ListViewItem Item in lsvDefectType.SelectedItems)
                    {
                        oList.Add(Item.SubItems[0].Text.ToString());
                        int classNumber = Base.Convert.intParse(Item.SubItems[0].Text);
                        foreach (Defect d in defectList)
                        {
                            if (d.CLASSNUMBER != classNumber)
                                continue;

                            filterDefectList.Add(d);
                        }
                    }
                }
                else
                {
                    filterDefectList = defectList;
                }

                //Defect Size Filter
                if (lsvDefectSize.SelectedItems.Count > 0 && !FindItem(lsvDefectSize.SelectedItems, "ALL"))
                {
                    double dMin = double.NaN;
                    double dMax = double.NaN;
                    List<Defect> result = new List<Defect>();
                    DefectList filterSize = new DefectList();
                    foreach (ListViewItem item in lsvDefectSize.SelectedItems)
                    {
                        if (!double.TryParse(item.SubItems[0].Text, out dMin))
                            continue;
                        if (!double.TryParse(item.SubItems[1].Text, out dMax))
                            continue;

                        result = filterDefectList.FindAll(delegate(Defect d) { return d.DSIZE >= dMin && d.DSIZE < dMax; });
                        filterSize.AddRange(result);
                    }

                    filterDefectList = filterSize;
                }

                ////Defect Data Draw, Recipe를 참고하여 그린다.
                DrawDefect(filterDefectList, dsTestWaferMap.Tables["RECIPE"], dsDefectWafer.Tables["SETUP_INFO"]);

                if (dsDefectWafer.Tables.IndexOf("COLOR_SIZE") > -1)
                {
                    matchMap.SizeColor = dsDefectWafer.Tables["COLOR_SIZE"];
                }

                if (dsDefectWafer.Tables.IndexOf("DEFECT_COLOR") > -1)
                {
                    matchMap.TypeColor = dsDefectWafer.Tables["DEFECT_COLOR"];
                }

                if (dsTestWaferMap.Tables.IndexOf("MASTER_BIN") > -1 && dsTestWaferMap.Tables["MASTER_BIN"].Rows.Count > 0)
                {
                    foreach (DataRow drBin in dsTestWaferMap.Tables["MASTER_BIN"].Rows)
                    {
                        if (drBin["HIGH_GEC"].ToString() == "Y")
                            oGoodBin.Add(int.Parse(drBin["BIN"].ToString()));
                    }
                }


                //정의된 Bin 이 없을 경우 AVI 는 0, 그외는 1번 Bin 이 good bin 이다.
                if (oGoodBin.Count <= 0)
                {
                    if (TESTAREA == "AVI")
                        oGoodBin.Add(0);
                    else
                        oGoodBin.Add(1);
                }

                double defectiveDie = 0d;
                double failBinDie = 0d;
                double dKillRate = CalcuationKillingRate(
                    oGoodBin.ToArray(),
                    out defectiveDie,
                    out failBinDie
                    );

                DrawDescription(
                    defectiveDie,
                    failBinDie,
                    dKillRate
                    );
            }
            finally
            {
                if (dtWaferIndexListShift != null)
                    dtWaferIndexListShift.Dispose();
                dtWaferIndexListShift = null;

                MainForm.SetStatusMessage(null);
                matchMap.Redraw();
                matchMap.WaferDrawMode = Map.MapMode.Fit;
                matchMap.Focus();
            }
        }


        /// <summary>
        /// Wafer를 시계방향으로 회전합니다.
        /// </summary>
        /// <param name="clockwiseAngle">시계방향회전각도 : 0,90,180,270</param>
        public void Rotate(int clockwiseAngle, int xmin, int xmax, int ymin, int ymax, ref int x, ref int y)
        {
            if (clockwiseAngle == 0)
                return;

            while (clockwiseAngle < 0)
                clockwiseAngle += 360;

            int nx, ny;

            // 계산횟수를 줄이기 위해 각도별로 계산식 구성
            if (clockwiseAngle == 180)
            {
                nx = xmax + xmin - x;
                ny = ymax + ymin - y;
            }
            else if (clockwiseAngle == 90)
            {
                nx = y;
                ny = xmax + xmin - x;
            }
            else if (clockwiseAngle == 270)
            {
                nx = ymax + ymin - y;
                ny = x;
            }
            else
            {
                throw new Exception(String.Format("처리할 수 없는 각도({0}) 입니다.", clockwiseAngle));
            }

            x = nx;
            y = ny;
        }

        private bool FindItem(ListView.SelectedListViewItemCollection oList, string keyword)
        {
            foreach (ListViewItem Item in oList)
            {
                if (Item.SubItems[0].Text.ToString() == keyword)
                    return true;
            }
            return false;
        }

        private void FillListViewItem(
            ListView lsv,
            DataTable dt,
            bool bUseAll,
            string mainCol,
            params string[] subCols
            )
        {
            lsv.Items.Clear();
            if (bUseAll) lsv.Items.Add("ALL");

            foreach (DataRow dr in dt.Rows)
            {
                ListViewItem item = lsv.Items.Add(dr[mainCol].ToString());
                if (subCols != null && subCols.Length > 0)
                {
                    for (int idx = 0; idx < subCols.Length; idx++)
                    {
                        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, dr[subCols[idx]].ToString()));
                    }
                }
            }
        }


        private void TestMapDraw(DataSet ds)
        {
            if (MakeWaferRecipe(ds, ref oWaferRecipe) == true)
            {
                SetMapBinColor(ds);
                DrawWaferMap(ds, oWaferRecipe);
            }
        }

        private bool MakeWaferRecipe(DataSet dsMap, ref DACrux.Base.WaferRecipe oWaferRecipe)
        {
            DataTable dtWaferInfo = null;

            try
            {
                int nIdx = dsMap.Tables.IndexOf("RECIPE");
                if (nIdx > -1 && dsMap.Tables[nIdx].Rows.Count > 0)
                    dtWaferInfo = dsMap.Tables[nIdx];

                oWaferRecipe = new DACrux.Base.WaferRecipe();

                double m_dMargin = 0.95D;

                if (dtWaferInfo == null || dtWaferInfo.Rows.Count < 1)
                {
                    throw new Exception("정의된 Map Define 정보가 없습니다. ");
                }
                else
                {
                    if (int.TryParse(dtWaferInfo.Rows[0]["FIRST_INDEX_X"].ToString(), out oWaferRecipe.FIRST_DIE_X) == false)
                        oWaferRecipe.FIRST_DIE_X = DACrux.Base.Convert.intParse(dtTestMapData.Compute("MIN([X])", "1=1").ToString());

                    if (int.TryParse(dtWaferInfo.Rows[0]["FIRST_INDEX_Y"].ToString(), out oWaferRecipe.FIRST_DIE_Y) == false)
                        oWaferRecipe.FIRST_DIE_Y = DACrux.Base.Convert.intParse(dtTestMapData.Compute("MIN([Y])", "1=1").ToString());

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MIN_X"].ToString(), out oWaferRecipe.DIE_INDEX_MIN_X) == false)
                        oWaferRecipe.DIE_INDEX_MIN_X = oWaferRecipe.FIRST_DIE_X;

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MIN_Y"].ToString(), out oWaferRecipe.DIE_INDEX_MIN_Y) == false)
                        oWaferRecipe.DIE_INDEX_MIN_Y = oWaferRecipe.FIRST_DIE_Y;

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MAX_X"].ToString(), out oWaferRecipe.DIE_INDEX_MAX_X) == false)
                        oWaferRecipe.DIE_INDEX_MAX_X = DACrux.Base.Convert.intParse(dtTestMapData.Compute("MAX([X])", "1=1").ToString());

                    if (int.TryParse(dtWaferInfo.Rows[0]["DIE_INDEX_MAX_Y"].ToString(), out oWaferRecipe.DIE_INDEX_MAX_Y) == false)
                        oWaferRecipe.DIE_INDEX_MAX_Y = DACrux.Base.Convert.intParse(dtTestMapData.Compute("MAX([Y])", "1=1").ToString());

                    if (Enum.TryParse(dtWaferInfo.Rows[0]["NOTCH_TYPE"].ToString(), out oWaferRecipe.NOTCH_TYPE) == false)
                        oWaferRecipe.NOTCH_TYPE = DACrux.Base.Notch.Flat;

                    if (int.TryParse(dtWaferInfo.Rows[0]["ANGLE"].ToString(), out oWaferRecipe.ANGLE) == false)
                        oWaferRecipe.ANGLE = 0;

                    if (Enum.TryParse(dtWaferInfo.Rows[0]["XY_DIRECTION"].ToString(), out oWaferRecipe.XYDIR) == false)
                        oWaferRecipe.XYDIR = DACrux.Base.XYDirection.LeftBottom;

                    oWaferRecipe.WAFER_SIZE = DACrux.Base.Util.GetValue(dtWaferInfo.Rows[0]["WAFER_SIZE"].ToString(), 200000);

                    if (double.TryParse(dtWaferInfo.Rows[0]["EDGE_SIZE"].ToString(), out oWaferRecipe.EDGE_SIZE) == false)
                        oWaferRecipe.EDGE_SIZE = 3d;

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_X"].ToString(), out oWaferRecipe.DIE_SIZE_X) == false)
                    {
                        oWaferRecipe.DIE_SIZE_X = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_X - oWaferRecipe.DIE_INDEX_MIN_X) * m_dMargin;
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_Y"].ToString(), out oWaferRecipe.DIE_SIZE_Y) == false)
                    {
                        oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y) * m_dMargin;
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_Y"].ToString(), out oWaferRecipe.DIE_SIZE_Y) == false)
                    {
                        oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y) * m_dMargin;
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["CHIP_SIZE_Y"].ToString(), out oWaferRecipe.DIE_SIZE_Y) == false)
                    {
                        oWaferRecipe.DIE_SIZE_Y = (oWaferRecipe.WAFER_SIZE) / (double)(oWaferRecipe.DIE_INDEX_MAX_Y - oWaferRecipe.DIE_INDEX_MIN_Y) * m_dMargin;
                    }

                    if (int.TryParse(dtWaferInfo.Rows[0]["ORIGIN_INDEX_X"].ToString(), out oWaferRecipe.ORIGIN_DIE_X) == false)
                    {
                        oWaferRecipe.ORIGIN_DIE_X = oWaferRecipe.DIE_INDEX_MIN_X + (int)Math.Floor((double)oWaferRecipe.XDIES / 2.0d);
                    }

                    if (int.TryParse(dtWaferInfo.Rows[0]["ORIGIN_INDEX_Y"].ToString(), out oWaferRecipe.ORIGIN_DIE_Y) == false)
                    {
                        oWaferRecipe.ORIGIN_DIE_Y = oWaferRecipe.DIE_INDEX_MIN_Y + (int)Math.Floor((double)oWaferRecipe.YDIES / 2.0d);
                    }

                    if (double.TryParse(dtWaferInfo.Rows[0]["ORIGIN_MICRO_X"].ToString(), out oWaferRecipe.ORIGIN_X) == false)
                    {
                        if (oWaferRecipe.XDIES < 20)
                        {
                            if (oWaferRecipe.XDIES % 2 == 0)
                                oWaferRecipe.ORIGIN_X = 0;
                            else
                                oWaferRecipe.ORIGIN_X = oWaferRecipe.DIE_SIZE_X / 2.0d;
                        }
                        else
                        {
                            if (oWaferRecipe.ANGLE == 90)
                                oWaferRecipe.ORIGIN_X = oWaferRecipe.DIE_SIZE_X / 2.0d;
                            else if (oWaferRecipe.ANGLE == 270)
                                oWaferRecipe.ORIGIN_X = -oWaferRecipe.DIE_SIZE_X / 2.0d;
                            else
                                oWaferRecipe.ORIGIN_X = 0;
                        }
                    }


                    if (double.TryParse(dtWaferInfo.Rows[0]["ORIGIN_MICRO_Y"].ToString(), out oWaferRecipe.ORIGIN_Y) == false)
                    {
                        if (oWaferRecipe.YDIES < 20)
                        {
                            if (oWaferRecipe.YDIES % 2 == 0)
                                oWaferRecipe.ORIGIN_Y = 0;
                            else
                                oWaferRecipe.ORIGIN_Y = oWaferRecipe.DIE_SIZE_Y / 2.0d;
                        }
                        else
                        {
                            if (oWaferRecipe.ANGLE == 90 || oWaferRecipe.ANGLE == 270)
                                oWaferRecipe.ORIGIN_Y = 0;
                            else if (oWaferRecipe.ANGLE == 0)
                                oWaferRecipe.ORIGIN_Y = -oWaferRecipe.DIE_SIZE_Y / 2.0d;
                            else if (oWaferRecipe.ANGLE == 180)
                                oWaferRecipe.ORIGIN_Y = oWaferRecipe.DIE_SIZE_Y / 2.0d;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetMapBinColor(DataSet dsMap)
        {
            try
            {
                if (dsMap.Tables.IndexOf("BINSUM") < 0 || dsMap.Tables["BINSUM"] == null)
                    return;

                m_lstBinColor = new List<BinColor>();
                DataTable dtBinSum = dsMap.Tables["BINSUM"];

                for (int i = 0; i < dtBinSum.Rows.Count; i++)
                {
                    int nBin = -1;

                    if (int.TryParse(dtBinSum.Rows[i]["BIN"].ToString(), out nBin))
                    {
                        if (string.IsNullOrEmpty(dtBinSum.Rows[i]["COLOR"].ToString().Trim()) == false
                            && dtBinSum.Rows[i]["COLOR"].ToString() != "#FFFFFF")
                        {
                            if (nBin > -1)
                            {
                                matchMap.SetColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["COLOR"].ToString()));
                            }

                            m_lstBinColor.Add(new BinColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["COLOR"].ToString())));
                        }
                        else
                        {
                            m_lstBinColor.Add(new BinColor(nBin, DACrux.TEST.Control.Util.GetColor(nBin)));
                        }
                    }
                    else
                        m_lstBinColor.Add(new BinColor(nBin, DACrux.TEST.Control.Util.GetColor(nBin)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void DrawWaferMap(DataSet dsMap, DACrux.Base.WaferRecipe oWaferRecipe)
        {
            int iTemp = 0;

            try
            {
                matchMap.ResetSelectedDie();
                matchMap.DieClear();

                if (dtWaferIndexList == null || dtWaferIndexList.Rows.Count <= 0)
                    return;

                matchMap.SetWaferRecipe(oWaferRecipe);
                matchMap.DataSource = dtWaferIndexList;

                //Shot 관련 정보확인 및 Draw
                matchMap.VisibleShot = false;

                if (dsMap.Tables.IndexOf("WAFER_INFO") > -1 && dsMap.Tables["WAFER_INFO"] != null && dsMap.Tables["WAFER_INFO"].Rows.Count > 0)
                {
                    LOT_ID = dsMap.Tables["WAFER_INFO"].Rows[0]["LOT_ID"].ToString();
                    PROGRAM = dsMap.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString();
                    DEVICE = dsMap.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString();
                    TESTAREA = dsMap.Tables["WAFER_INFO"].Rows[0]["TESTAREA"].ToString();
                }

                if (dsMap.Tables.IndexOf("SHOT_DEF") >= 0 && dsMap.Tables["SHOT_DEF"] != null && dsMap.Tables["SHOT_DEF"].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsMap.Tables["SHOT_DEF"].Rows)
                    {
                        if (int.TryParse(dr["ST_XCNT"].ToString(), out iTemp) == true)
                            matchMap.ShotArrayX = iTemp;

                        if (int.TryParse(dr["ST_YCNT"].ToString(), out iTemp) == true)
                            matchMap.ShotArrayY = iTemp;

                        if (int.TryParse(dr["ST_START_X"].ToString(), out iTemp) == true)
                            matchMap.ShotStartX = iTemp;

                        if (int.TryParse(dr["ST_START_Y"].ToString(), out iTemp) == true)
                            matchMap.ShotStartY = iTemp;
                    }
                }

                matchMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                matchMap.Focus();
            }
            catch (Exception ex)
            {
                matchMap.DieClear();
                matchMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                throw ex;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Defect Data 기준 Map 상에 Draw 한다.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtTestMapRecipe"></param>
        /// <param name="dtDMMapRecipe"></param>
        private void DrawDefect(DefectList defects, DataTable dtTestMapRecipe, DataTable dtDMMapRecipe)
        {
            matchMap.DefectClear();

            if (defects == null)
                return;

            if (dtBinDefectList != null)
                dtBinDefectList.Dispose();

            List<string> oList = null;

            try
            {
                dtTestMapData.DefaultView.Sort = "X ASC, Y ASC";

                dtBinDefectList = new DataTable();
                dtBinDefectList.Columns.Add(new DataColumn("INDEX_X", typeof(int)));
                dtBinDefectList.Columns.Add(new DataColumn("INDEX_Y", typeof(int)));
                dtBinDefectList.Columns.Add(new DataColumn("BIN", typeof(int)));
                dtBinDefectList.Columns.Add(new DataColumn("CLASSNUMBER", typeof(int)));
                dtBinDefectList.Columns.Add(new DataColumn("CLUSTERNUMBER", typeof(int)));
                dtBinDefectList.Columns.Add(new DataColumn("XSIZE", typeof(double)));
                dtBinDefectList.Columns.Add(new DataColumn("YSIZE", typeof(double)));
                dtBinDefectList.Columns.Add(new DataColumn("DSIZE", typeof(double)));
                dtBinDefectList.AcceptChanges();

                //Test Wafer Shot Map 기준 정보
                double dTestChipSizeX = double.Parse(dtTestMapRecipe.Rows[0]["CHIP_SIZE_X"].ToString());
                double dTestChipSizeY = double.Parse(dtTestMapRecipe.Rows[0]["CHIP_SIZE_Y"].ToString());
                double dTestCenterRELX = double.Parse(dtTestMapRecipe.Rows[0]["ORIGIN_MICRO_X"].ToString());
                double dTestCenterRELY = double.Parse(dtTestMapRecipe.Rows[0]["ORIGIN_MICRO_Y"].ToString());
                int iTestCenterIndexX = int.Parse(dtTestMapRecipe.Rows[0]["ORIGIN_INDEX_X"].ToString());
                int iTestCenterIndexY = int.Parse(dtTestMapRecipe.Rows[0]["ORIGIN_INDEX_Y"].ToString());

                //DM Setup 정보 기준정보
                double dDMChipSizeX = double.Parse(dtDMMapRecipe.Rows[0]["DIE_PITCH_X"].ToString());
                double dDMChipSizeY = double.Parse(dtDMMapRecipe.Rows[0]["DIE_PITCH_Y"].ToString());
                double dDMCenterRELX = double.Parse(dtDMMapRecipe.Rows[0]["ORIGIN_X"].ToString());
                double dDMCenterRELY = double.Parse(dtDMMapRecipe.Rows[0]["ORIGIN_Y"].ToString());
                int iDMCenterIndexX = int.Parse(dtDMMapRecipe.Rows[0]["DIE_ORIGIN_X"].ToString());
                int iDMCenterIndexY = int.Parse(dtDMMapRecipe.Rows[0]["DIE_ORIGIN_Y"].ToString());

                //Test Shot Map 기준으로 그린다.
                double dMatchMapCenterX = (iTestCenterIndexX * dTestChipSizeX) + dTestCenterRELX;
                double dMatchMapCenterY = (iTestCenterIndexY * dTestChipSizeY) + dTestCenterRELY;

                //Bin 선택
                if (lsvBin.SelectedItems.Count > 0 && !FindItem(lsvBin.SelectedItems, "ALL"))
                {
                    oList = new List<string>();
                    foreach (ListViewItem Item in lsvBin.SelectedItems)
                    {
                        oList.Add(Item.SubItems[0].Text.ToString());
                    }
                }

                long[] steps = defects.GetStepSeqArray();
                SEMDMS.RO.DefectMapAnalysis obj = new SEMDMS.RO.DefectMapAnalysis();
                DataTable dtImages = null;
                if (steps != null && steps.Length > 0)
                    dtImages = obj.GetDefectImagePath01(steps);

                foreach (Defect d in defects)
                {
                    d.XINDEX = (int)((dMatchMapCenterX + d.X) / dTestChipSizeX);
                    d.YINDEX = (int)((dMatchMapCenterY + d.Y) / dTestChipSizeY);

                    /// Defect Image mapping
                    d.Images.Clear();
                    if (dtImages != null && dtImages.Rows.Count > 0)
                    {
                        DataRow[] rows = dtImages.Select(String.Format("[STEP_SEQ] = '{0}' AND [DEFECTID] = '{1}'", d.STEP_SEQ, d.DEFECTID));
                        if (rows != null && rows.Length > 0)
                        {
                            foreach (DataRow row in rows)
                            {
                                d.Images.Add(new DefectImage()
                                {
                                    IMAGESEQ = Base.Convert.intParse(row["IMAGE_ID"].ToString()),
                                    IMAGEPATH = String.Format(@"{0}{1}", row["THUMB_PATH"], row["THUMB_FILENAME"])
                                });
                            }
                        }
                    }
                    d.IMAGECOUNT = d.Images.Count;

                    matchMap.AddDefect(d);

                    //Bin Filtering 이 있을 경우 와 없을 경우로하여 구성
                    DataRow[] drDefectBin = null;
                    if (oList != null && oList.Count > 0)
                    {
                        //dtTestMapData 는 DB에서 조회한 데이터, dtWaferIndexList 는 Shfit 정보
                        //drDefectBin = dtTestMapData.Select(string.Format("X = {0} AND Y = {1} AND BIN IN ({2})", d.XINDEX, d.YINDEX, string.Join(",", oList.ToArray())));
                        drDefectBin = dtWaferIndexList.Select(string.Format("X = {0} AND Y = {1} AND BIN IN ({2})", d.XINDEX, d.YINDEX, string.Join(",", oList.ToArray())));
                    }
                    else
                    {
                        //dtTestMapData 는 DB에서 조회한 데이터, dtWaferIndexList 는 Shfit 정보
                        //drDefectBin = dtTestMapData.Select(string.Format("X = {0} AND Y = {1}", d.XINDEX, d.YINDEX));
                        drDefectBin = dtWaferIndexList.Select(string.Format("X = {0} AND Y = {1}", d.XINDEX, d.YINDEX));
                    }

                    if (drDefectBin.Length > 0)
                    {
                        DataRow drNew = dtBinDefectList.NewRow();
                        drNew["INDEX_X"] = drDefectBin[0]["X"];
                        drNew["INDEX_Y"] = drDefectBin[0]["Y"];
                        drNew["BIN"] = drDefectBin[0]["BIN"];
                        drNew["CLASSNUMBER"] = d.CLASSNUMBER;
                        drNew["CLUSTERNUMBER"] = d.CLUSTERNUMBER;
                        drNew["XSIZE"] = d.XSIZE;
                        drNew["YSIZE"] = d.YSIZE;
                        drNew["DSIZE"] = d.DSIZE;
                        dtBinDefectList.Rows.Add(drNew);
                    }
                }
                dtBinDefectList.AcceptChanges();
            }
            finally
            {
            }
        }

        private double CalcuationKillingRate(
            int[] iGoodBin,
            out double defectviedie,
            out double failBinDie
            )
        {
            DataRow[] drsDefects = null;

            //전체 Wafer Bin 중 Good Bin 이 아닌 Data
            //if (dtWaferIndexList.Select(string.Format("BIN NOT IN ({0})", string.Join(",", iGoodBin))).Length <= 0)
            if (dtWaferIndexList == null || dtWaferIndexList.Rows.Count <= 0)
            {
                defectviedie = double.NaN;
                failBinDie = double.NaN;
                return double.NaN;
            }

            //defectviedie = dtWaferIndexList.Select(string.Format("BIN NOT IN ({0})", string.Join(",", iGoodBin))).Length;

            //Fail Bin 중  Defect 이 있는 Die

            //defectviedie : Defect 이 있는 Die 수
            if (dtBinDefectList == null || dtBinDefectList.Rows.Count <= 0)
            {
                defectviedie = double.NaN;
                failBinDie = double.NaN;
                return double.NaN;
            }
            defectviedie = dtBinDefectList.DefaultView.ToTable(true, "INDEX_X", "INDEX_Y", "BIN").Rows.Count;

            //failBinDie : Defect 이 있고 FailBin 은 die 수
            drsDefects = dtBinDefectList.DefaultView.ToTable(true, "INDEX_X", "INDEX_Y", "BIN").Select(string.Format("BIN NOT IN ({0})", string.Join(",", iGoodBin)));

            //failBinDie 에 대해서 die Line 상에 색상 변경
            matchMap.VisibleSignDies = true;
            matchMap.ClearSignDie();
            foreach (DataRow dr in drsDefects)
            {
                matchMap.AddSignDie(int.Parse(dr["INDEX_X"].ToString()), int.Parse(dr["INDEX_Y"].ToString()));
            }

            failBinDie = drsDefects.Length;

            return Math.Round(((double)failBinDie / (double)defectviedie) * 100, 2);

        }


        private void DrawDescription(
            double defectiveDie,
            double failBinDie,
            double killRate
            )
        {
            string[] mapInformation = new string[3];
            mapInformation[0] = string.Format("Kill Rate: {0}%", double.IsNaN(killRate) ? 0d : killRate);
            mapInformation[1] = string.Format("Bad Die Count: {0}", double.IsNaN(failBinDie) ? 0d : failBinDie);
            mapInformation[2] = string.Format("Defective Die Count: {0}", double.IsNaN(defectiveDie) ? 0d : defectiveDie);
            matchMap.SetInfomation(mapInformation);
        }

        private void ControlAdd(DACrux.SEMDMS.Control.DPUCDefectInfo container)
        {

            if (ImageList.InvokeRequired)
            {
                ImageList.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        ControlAdd(container);
                    }
                ));
            }
            else
            {
                if (container == null) return;

                //container.Height = container.Height + (container.Margin.Top + container.Margin.Bottom);
                container.Height = container.Height + container.Margin.Top;
                container.Width = ImageList.Width - SystemInformation.VerticalScrollBarWidth - container.Margin.Left - container.Margin.Right;
                ImageList.Controls.Add(container);
                ImageList.ResumeLayout();
            }
        }

        #endregion [ Method ]

        //----------------------------------------------------------------

        #region [ Properties ]

        public object DataSource
        {
            get;
            set;
        }

        #endregion [ Properties ]
    }
}
