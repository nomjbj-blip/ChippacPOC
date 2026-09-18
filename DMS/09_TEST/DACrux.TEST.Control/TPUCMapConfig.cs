using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization.Charting.Utilities;

namespace DACrux.MapAnalysis.Control
{
    /// <summary>
    /// TPUCMapConfig 에 대한 요약 설명입니다.
    /// </summary>
    public partial class TPUCMapConfig : System.Windows.Forms.UserControl
    {
        private DataSet dsWaferOri = null;
        private DataTable dtRowData = null;
        private DataTable dtConfig = null;
        private enum TQP_MAPCFG { MAP_CFG_SEQ, REV_NO, SEQ_REV_NO, TESTAREA, DEVICE_ALIAS, PROGRAM, ALTER_DIRECTION, ALTER_ANGLE, ALTER_INDEX_X, ALTER_INDEX_Y, CREATE_USER, CREATE_TIME, UPDATE_USER, UPDATE_TIME, USER_COMMENT, DELETE_FLAG }

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

        private List<BinColor> m_lstBinColor = null;
        private DataSet m_dsMap = null;
        private DACrux.Base.WaferRecipe oWaferRecipe = new DACrux.Base.WaferRecipe();
        private bool m_bShowDataGrid = true;
        private bool m_bShowUserInformation = false;
        private string[] m_sUserInformation = null;
        private int m_nDisplayFaltAngle = -1;
        private string m_sZonalGoodBins = "1";
        private string strParaItem = string.Empty;

        public List<BinColor> mBinColor
        {
            get { return m_lstBinColor; }
        }

        public DACrux.Base.TPWafer[] mWaferInfo;

        #region [ Create & Close ]

        public TPUCMapConfig()
        {
            InitializeComponent();
        }

        private void TPUCMapConfig_Load(object sender, EventArgs e)
        {
            DACrux.TEST.RO.ProbeAdmin oProbeAdmin = new DACrux.TEST.RO.ProbeAdmin();

            dlbProduct.Items.Clear();
            cmbDevice.Items.Clear();
            cmbArea.Items.Clear();
            DataTable dt = oProbeAdmin.GetProductList();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int ir = 0; ir < dt.Rows.Count; ir++)
                    cmbDevice.Items.Add(dt.Rows[ir]["PRODUCT"].ToString());
            }


            dt = oProbeAdmin.GetTestAreaListNotPCM();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int ir = 0; ir < dt.Rows.Count; ir++)
                    cmbArea.Items.Add(dt.Rows[ir]["TESTAREA"].ToString());
            }

            dtStart.Value = DateTime.Now.AddMonths(-1);
            dtEnd.Value = DateTime.Now;

            BtnView_Click(null, null);
        }


        #endregion


        #region [ User Method ]

        private void FillControlData(
            DACrux.Framework.Controls.DUCListBox dlb,
            DataTable dt,
            String displayMember,
            String valueMember
            )
        {
            dlb.DisplayMember = displayMember;
            dlb.ValueMember = valueMember;
            dlb.DataSource = dt;

            dlb.SelectedIndex = -1;
        }

        private void ViewSheetSet(DataTable dtData)
        {
            fsSheet_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect | FarPoint.Win.Spread.OperationMode.ReadOnly;

            fsSheet_Sheet1.DataSource = dtData.Copy();
            Utility.FPSpreadUtil.SetAutoColumnWidth(fsSheet_Sheet1);

            FarPoint.Win.Spread.CellType.NumberCellType nmbrcell = new FarPoint.Win.Spread.CellType.NumberCellType();
            nmbrcell.DecimalPlaces = 0;

            fsSheet.ActiveSheet.Columns[(int)TQP_MAPCFG.MAP_CFG_SEQ].Visible = false;
            fsSheet.ActiveSheet.Columns[(int)TQP_MAPCFG.ALTER_ANGLE].CellType = nmbrcell;
            fsSheet.ActiveSheet.Columns[(int)TQP_MAPCFG.ALTER_INDEX_X].CellType = nmbrcell;
            fsSheet.ActiveSheet.Columns[(int)TQP_MAPCFG.ALTER_INDEX_Y].CellType = nmbrcell;
            fsSheet.ActiveSheet.Columns[(int)TQP_MAPCFG.MAP_CFG_SEQ].CellType = nmbrcell;
            fsSheet.ActiveSheet.Columns[(int)TQP_MAPCFG.REV_NO].CellType = nmbrcell;
        }

        private void fnControlClear()
        {
            TxtRevision.Text = string.Empty;
            cmbDevice.Text = string.Empty;
            TxtProgram.Text = string.Empty;
            chkProgram.Checked = false;
            cmbDir.Text = string.Empty;
            numAngle.Value = 0;
            numIndexX.Value = 0;
            numIndexY.Value = 0;
            TxtDeleteFlag.Text = string.Empty;
            TxtConfigSeq.Text = string.Empty;
            TxtLastUpdateUser.Text = string.Empty;
            TxtLastUpdateTime.Text = string.Empty;
            TxtCreateUser.Text = string.Empty;
            TxtCreateTime.Text = string.Empty;
            TxtComment.Text = string.Empty;
        }

        #endregion


        #region [ Event Handler ]

        private void BtnView_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                dlbProduct.DataSource = null;
                dlbProgram.DataSource = null;
                dlbRevision.DataSource = null;
                fsSheet_Sheet1.Rows.Clear();

                DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
                dtConfig = oProbeMapAnalysis.SelectMapConfigView(chkDeleteFlag.Checked);
                if (dtConfig != null && dtConfig.Rows.Count > 0)
                {
                    DataTable dtFilter = dtConfig.DefaultView.ToTable(true, "TESTAREA");
                    FillControlData(
                      dlbTestArea,
                      dtFilter,
                      "TESTAREA",
                      "TESTAREA"
                      );

                    ViewSheetSet(dtConfig);
                }

                fnControlClear();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void dlbTestArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;

            dlbProduct.DataSource = null;
            dlbProgram.DataSource = null;
            dlbRevision.DataSource = null;

            if (dlbTestArea.SelectedItems.Count <= 0)
                return;

            string strTestArea = dlbTestArea.SelectedValue.ToString();

            DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
            dtConfig = oProbeMapAnalysis.SelectMapConfigView(chkDeleteFlag.Checked);

            if (dtConfig != null &&
                dtConfig.Select(string.Format("TESTAREA = '{0}'", strTestArea)).Length > 0)
            {
                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}'", strTestArea), "DEVICE_ALIAS").CopyToDataTable<DataRow>().DefaultView.ToTable(true, "DEVICE_ALIAS");

                FillControlData(
                       dlbProduct,
                       dtFilter,
                       "DEVICE_ALIAS",
                       "DEVICE_ALIAS"
                       );

                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}'", strTestArea), "DEVICE_ALIAS").CopyToDataTable<DataRow>();
                ViewSheetSet(dtFilter);
               
            }
        }


        private void dlbProduct_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;

            dlbProgram.DataSource = null;
            dlbRevision.DataSource = null;

            if (dlbTestArea.SelectedItems.Count <= 0
                || dlbProduct.SelectedItems.Count <= 0)
                return;

            string strTestArea = dlbTestArea.SelectedValue.ToString();
            string strDevice = dlbProduct.SelectedValue.ToString();

            DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
            dtConfig = oProbeMapAnalysis.SelectMapConfigView(chkDeleteFlag.Checked);

            if (dtConfig != null &&
                dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}'", strTestArea, strDevice)).Length > 0)
            {
                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}'", strTestArea, strDevice), "PROGRAM").CopyToDataTable<DataRow>().DefaultView.ToTable(true, "PROGRAM");

                FillControlData(
                         dlbProgram,
                         dtFilter,
                         "PROGRAM",
                         "PROGRAM"
                         );

                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}'", strTestArea, strDevice), "PROGRAM").CopyToDataTable<DataRow>();
                ViewSheetSet(dtFilter);

            }
           
        }


        private void dlbProgram_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;

            dlbRevision.DataSource = null;

            if (dlbTestArea.SelectedItems.Count <= 0
                || dlbProduct.SelectedItems.Count <= 0
                || dlbProgram.SelectedItems.Count <= 0)
                return;

            string strTestArea = dlbTestArea.SelectedValue.ToString();
            string strDevice = dlbProduct.SelectedValue.ToString();
            string strProgram = dlbProgram.SelectedValue.ToString();

            DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
            dtConfig = oProbeMapAnalysis.SelectMapConfigView(chkDeleteFlag.Checked);

            if (dtConfig != null &&
                dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}' AND PROGRAM = '{2}'", strTestArea, strDevice, strProgram)).Length > 0)
            {
                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}' AND PROGRAM = '{2}'", strTestArea, strDevice, strProgram), "REV_NO DESC").CopyToDataTable<DataRow>();

                FillControlData(
                       dlbRevision,
                       dtFilter,
                       "SEQ_REV_NO",
                       "MAP_CFG_SEQ"
                       );

                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}' AND PROGRAM = '{2}'", strTestArea, strDevice, strProgram), "REV_NO DESC").CopyToDataTable<DataRow>();
                ViewSheetSet(dtFilter);

            }
        }



        private void dlbRevision_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtFilter = null;

            if (dlbTestArea.SelectedItems.Count <= 0
                || dlbProduct.SelectedItems.Count <= 0
                || dlbProgram.SelectedItems.Count <= 0
                || dlbRevision.SelectedItems.Count <= 0)
                return;

            string strTestArea = dlbTestArea.SelectedValue.ToString();
            string strDevice = dlbProduct.SelectedValue.ToString();
            string strProgram = dlbProgram.SelectedValue.ToString();
            string strConfigSeq = dlbRevision.SelectedValue.ToString();

            DACrux.TEST.RO.ProbeMapAnalysis oProbeMapAnalysis = new DACrux.TEST.RO.ProbeMapAnalysis();
            dtConfig = oProbeMapAnalysis.SelectMapConfigView(chkDeleteFlag.Checked);

            if (dtConfig != null &&
                dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}' AND PROGRAM = '{2}' AND MAP_CFG_SEQ = '{3}'", strTestArea, strDevice, strProgram, strConfigSeq)).Length > 0)
            {
                dtFilter = dtConfig.Select(string.Format("TESTAREA = '{0}' AND DEVICE_ALIAS = '{1}' AND PROGRAM = '{2}' AND MAP_CFG_SEQ = '{3}'", strTestArea, strDevice, strProgram, strConfigSeq), "REV_NO").CopyToDataTable<DataRow>();
                ViewSheetSet(dtFilter);

            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;

            int iAlterX = 0;
            int iAlterY = 0;
            int iAlterAngle = 0;

            try
            {
                if (string.IsNullOrEmpty(cmbArea.Text) == true)
                {
                    MessageBox.Show("Test Area 정보를 선택 해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbArea.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(cmbDevice.Text) == true)
                {
                    MessageBox.Show("Device 정보를 선택 해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbDevice.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(TxtProgram.Text) == true)
                {
                    MessageBox.Show("Program 정보를 입력 해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtProgram.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(cmbDir.Text) == true)
                {
                    MessageBox.Show("Direction 정보를 입력 해주세요. Default 는 LL 입니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbDir.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(TxtComment.Text) == true)
                {
                    MessageBox.Show("Comment 에 입력 사항이 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtComment.Focus();
                    return;
                }

                if (int.TryParse(numIndexX.Text, out iAlterX) == false)
                    iAlterX = 0;

                if (int.TryParse(numIndexY.Text, out iAlterY) == false)
                    iAlterY = 0;

                if (int.TryParse(numAngle.Text, out iAlterAngle) == false)
                    iAlterAngle = 0;

                if (MessageBox.Show(string.Format("현재 정보로 해당 Device ({0}) 에 저장 하시겠습니까?", cmbDevice.Text), "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;

                if (chkProgram.Checked == true && MessageBox.Show(string.Format("해당 Program {0} 으로 예외 처리됩니다. 계속 하시겠습니까?", TxtProgram.Text), "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;

                DACrux.TEST.RO.ProbeMapAnalysis oTest = new TEST.RO.ProbeMapAnalysis();
                oTest.UpdateMapConfig(cmbArea.Text,
                                      cmbDevice.Text,
                                      chkProgram.Checked == false ? "ALL" : TxtProgram.Text,
                                      string.IsNullOrEmpty(cmbDir.Text) ? "LL" : cmbDir.Text,
                                      iAlterAngle.ToString(),
                                      iAlterX.ToString(),
                                      iAlterY.ToString(),
                                      DACrux.Base.GlobalVariable.UserID,
                                      TxtComment.Text);

                MessageBox.Show("저장 되었습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                fsSheet_Sheet1.Rows.Clear();
                dlbRevision.DataSource = null;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(TxtConfigSeq.Text) == true)
            {
                MessageBox.Show("저장되어 있는 정보가 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(TxtComment.Text) == true)
            {
                MessageBox.Show("Comment 에 입력 사항이 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtComment.Focus();
                return;
            }

            if (MessageBox.Show(string.Format("현재 정보를 삭제 하시겠습니까?\nArea:{0}, Device:{1}, PGM:{2}", cmbArea.Text, cmbDevice.Text, TxtProgram.Text), "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            this.Cursor = Cursors.WaitCursor;

            try
            {
                DACrux.TEST.RO.ProbeMapAnalysis oTest = new TEST.RO.ProbeMapAnalysis();
                oTest.DeleteMapConfig(TxtConfigSeq.Text, DACrux.Base.GlobalVariable.UserID, TxtComment.Text);

                MessageBox.Show("삭제 되었습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                BtnView_Click(null, null);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void chkProgram_CheckedChanged(object sender, EventArgs e)
        {
            TxtProgram.ReadOnly = !chkProgram.Checked;
            if (TxtProgram.ReadOnly)
                TxtProgram.Text = "ALL";

        }

        private void fsSheet_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0)
                return;

            int iTemp = 0;
            TxtRevision.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.REV_NO].Text;
            cmbDevice.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.DEVICE_ALIAS].Text;
            TxtProgram.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.PROGRAM].Text;
            cmbArea.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.TESTAREA].Text;

            if (TxtProgram.Text == "ALL")
            {
                chkProgram.Checked = false;
                TxtProgram.ReadOnly = true;
            }

            cmbDir.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.ALTER_DIRECTION].Text;

            if (int.TryParse(fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.ALTER_ANGLE].Value.ToString(), out iTemp) == false)
                iTemp = 0;
            numAngle.Value = iTemp;

            if (int.TryParse(fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.ALTER_INDEX_X].Value.ToString(), out iTemp) == false)
                iTemp = 0;
            numIndexX.Value = iTemp;

            if (int.TryParse(fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.ALTER_INDEX_Y].Value.ToString(), out iTemp) == false)
                iTemp = 0;
            numIndexY.Value = iTemp;

            TxtDeleteFlag.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.DELETE_FLAG].Text;
            TxtConfigSeq.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.MAP_CFG_SEQ].Text;
            TxtLastUpdateUser.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.UPDATE_USER].Text;
            TxtLastUpdateTime.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.UPDATE_TIME].Text;
            TxtCreateUser.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.CREATE_USER].Text;
            TxtCreateTime.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.CREATE_TIME].Text;
            TxtComment.Text = fsSheet.ActiveSheet.Cells[e.Row, (int)TQP_MAPCFG.USER_COMMENT].Text;

            //조회되었던 내용 clear
            fpSpread1.ActiveSheet.Rows.Clear();
            lbInform.Text = "-";

            m_wMapBF.DieClear();
            m_wMapBF.Redraw();

            m_wMapAF.DieClear();
            m_wMapAF.Redraw();

        }

        private void cmbDevice_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbDir_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbArea_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void numAngle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void numIndexX_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void numIndexY_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void chkDeleteFlag_CheckedChanged(object sender, EventArgs e)
        {
            BtnView_Click(null, null);
        }

        private void cmbDir_ValueChanged(object sender, EventArgs e)
        {
            MapConfigAfterMap();
        }

        private void numAngle_ValueChanged(object sender, EventArgs e)
        {
            MapConfigAfterMap();
        }

        private void numIndexX_ValueChanged(object sender, EventArgs e)
        {
            MapConfigAfterMap();
        }

        private void numIndexY_ValueChanged(object sender, EventArgs e)
        {
            MapConfigAfterMap();
        }

        private void m_wMapBF_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            txtXIndexBF.Text = NewDie.IndexX.ToString();
            txtYIndexBF.Text = NewDie.IndexY.ToString();
            txtBinBF.Text = NewDie.BinNumber.ToString();
        }

        private void m_wMapAF_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            txtXIndexAF.Text = NewDie.IndexX.ToString();
            txtYIndexAF.Text = NewDie.IndexY.ToString();
            txtBinAF.Text = NewDie.BinNumber.ToString();
        }

        #endregion  [ Event Handler ]


        #region [ Map Control ]


        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0)
                return;

            int iColWaferSeq = -1;
            int iColTESTAREA = -1;
            int iColDEVICE = -1;
            int iColPROGRAM = -1;

            string strWaferSeq = string.Empty;
            string strTestArea = string.Empty;
            string strDevice = string.Empty;
            string strProgram = string.Empty;

            iColWaferSeq = dtRowData.Columns.IndexOf("WAFER_SEQ");
            iColTESTAREA = dtRowData.Columns.IndexOf("TESTAREA");
            iColDEVICE = dtRowData.Columns.IndexOf("DEVICE_ALIAS");
            iColPROGRAM = dtRowData.Columns.IndexOf("PROGRAM");

            if (iColWaferSeq > -1)
            {
                strWaferSeq = fpSpread1.ActiveSheet.Cells[e.Row, iColWaferSeq].Text;

                DACrux.TEST.RO.ProbeMapAnalysis oTestMap = new DACrux.TEST.RO.ProbeMapAnalysis();
                dsWaferOri = oTestMap.SelectWaferMapConfigMap(long.Parse(strWaferSeq));
                if (dsWaferOri == null || dsWaferOri.Tables.Count <= 0)
                    throw new Exception("Not Found Data");

                DrawMapBF(dsWaferOri);
                DrawMapAF(dsWaferOri);

                strTestArea = fpSpread1.ActiveSheet.Cells[e.Row, iColTESTAREA].Text;
                strDevice = fpSpread1.ActiveSheet.Cells[e.Row, iColDEVICE].Text;
                strProgram = fpSpread1.ActiveSheet.Cells[e.Row, iColPROGRAM].Text;

                lbInform.Text = string.Format("TestArea : {0}, Device : {1}, Program : {2}", strTestArea, strDevice, strProgram);

                MapConfigAfterMap();
            }

        }

        private void BtnWaferView_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            DACrux.TEST.RO.ProbeMapAnalysis oReport = new DACrux.TEST.RO.ProbeMapAnalysis();

            try
            {
                fpSpread1.ActiveSheet.Rows.Clear();

                if (dlbTestArea.SelectedItems.Count <= 0 || dlbProduct.SelectedItems.Count <= 0 || dlbProgram.SelectedItems.Count <= 0)
                {
                    MessageBox.Show("조회시 상위의 조회조건들은 선택해야 합니다.\nTest Area, Device Alias, Program", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string strTestArea = dlbTestArea.SelectedValue.ToString();
                string strDevice = dlbProduct.SelectedValue.ToString();
                string strProgram = dlbProgram.SelectedValue.ToString();

                dtRowData = oReport.GetMapConfigWaferList(dtStart.Value.ToString("yyyyMMdd"), dtEnd.Value.AddDays(1).ToString("yyyyMMdd"), strTestArea, strDevice, strProgram);
                if (dtRowData == null || dtRowData.Rows.Count <= 0)
                {
                    MessageBox.Show("Not Found Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(TxtLotID.Text) == false)
                {
                    if (dtRowData.Select(string.Format("LOT_ID = '{0}'", TxtLotID.Text.Trim().ToUpper())).Length <= 0)
                    {
                        MessageBox.Show("Not Found Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        dtRowData = dtRowData.Select(string.Format("LOT_ID = '{0}'", TxtLotID.Text.Trim().ToUpper())).CopyToDataTable<DataRow>();
                    }

                }

                fpSpread1.ActiveSheet.DataSource = dtRowData;
                fpSpread1.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect | FarPoint.Win.Spread.OperationMode.ReadOnly;

                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1.ActiveSheet);

                FarPoint.Win.Spread.CellType.NumberCellType nmbrcell = new FarPoint.Win.Spread.CellType.NumberCellType();
                nmbrcell.DecimalPlaces = 0;

                int iCol = -1;

                iCol = dtRowData.Columns.IndexOf("MAP_CFG_SEQ");
                if (iCol > -1)
                    fpSpread1.ActiveSheet.Columns[iCol].CellType = nmbrcell;

                iCol = dtRowData.Columns.IndexOf("WAFER_SEQ");
                if (iCol > -1)
                    fpSpread1.ActiveSheet.Columns[iCol].CellType = nmbrcell;

                iCol = dtRowData.Columns.IndexOf("LOT_SEQ");
                if (iCol > -1)
                    fpSpread1.ActiveSheet.Columns[iCol].CellType = nmbrcell;

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public void DrawMapBF(DataSet m_dsMap)
        {
            try
            {
                if (m_dsMap == null)
                    return;

                m_wMapBF.SetDefaultColors();

                if (m_dsMap.Tables.IndexOf("MASTER_BIN") >= 0 && m_dsMap.Tables["MASTER_BIN"] != null && m_dsMap.Tables["MASTER_BIN"].Rows.Count > 0)
                {
                    DataTable dtBinSum = m_dsMap.Tables["MASTER_BIN"];

                    for (int i = 0; i < dtBinSum.Rows.Count; i++)
                    {
                        int nBin = -1;

                        if (int.TryParse(dtBinSum.Rows[i]["BIN"].ToString(), out nBin))
                        {
                            if (string.IsNullOrEmpty(dtBinSum.Rows[i]["BIN_COLOR"].ToString().Trim()) == false && dtBinSum.Rows[i]["BIN_COLOR"].ToString() != "#FFFFFF")
                            {
                                if (nBin > -1)
                                {
                                    m_wMapBF.SetColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["BIN_COLOR"].ToString()));
                                }
                            }
                        }
                    }
                }

                m_wMapBF.WaferSize = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());

                m_wMapBF.DieSizeX = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                m_wMapBF.DieSizeY = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());

                m_wMapBF.OriginIndexX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_wMapBF.OriginIndexY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());

                m_wMapBF.OriginX = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_wMapBF.OriginY = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());

                m_wMapBF.FirstDieX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                m_wMapBF.FirstDieY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());

                m_wMapBF.NotchAngle = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                m_wMapBF.EdgeSize = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                m_wMapBF.NotchType = DACrux.Base.Notch.Notch; //m_dsMap.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                m_wMapBF.DieMinX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_X"].ToString());
                m_wMapBF.DieMinY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_Y"].ToString());
                m_wMapBF.DieMaxX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_X"].ToString());
                m_wMapBF.DieMaxY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_Y"].ToString());

                int iXYDir = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        m_wMapBF.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        m_wMapBF.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        m_wMapBF.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        m_wMapBF.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }
                m_wMapBF.DieCalculation(true);

                // Map Bin Row Data ===================================================================================================
                m_wMapBF.DieClear();
                m_wMapBF.DataSource = m_dsMap.Tables["MAPDATA"];
                m_wMapBF.VisibleVIFail = true;
                m_wMapBF.VisibleFocusDie = true;
                m_wMapBF.VIMember = "BIN";
                m_wMapBF.DieFocusingType = Map.FocusType.Arraw;

                //Shot 관련 정보확인 및 Draw
                m_wMapBF.VisibleShot = false;
                if (m_dsMap.Tables.IndexOf("SHOT_DEF") >= 0 && m_dsMap.Tables["SHOT_DEF"] != null && m_dsMap.Tables["SHOT_DEF"].Rows.Count > 0)
                {
                    int iTemp = 0;
                    m_wMapBF.VisibleShot = true;
                    foreach (DataRow dr in m_dsMap.Tables["SHOT_DEF"].Rows)
                    {
                        if (int.TryParse(dr["ST_XCNT"].ToString(), out iTemp) == true)
                            m_wMapBF.ShotArrayX = iTemp;

                        if (int.TryParse(dr["ST_YCNT"].ToString(), out iTemp) == true)
                            m_wMapBF.ShotArrayY = iTemp;

                        if (int.TryParse(dr["ST_START_X"].ToString(), out iTemp) == true)
                            m_wMapBF.ShotStartX = iTemp;

                        if (int.TryParse(dr["ST_START_Y"].ToString(), out iTemp) == true)
                            m_wMapBF.ShotStartY = iTemp;
                    }
                }

                //Mark die 관련 정보 수집
                if (m_dsMap.Tables.IndexOf("MARKDATA") >= 0 && m_dsMap.Tables["MARKDATA"] != null && m_dsMap.Tables["MARKDATA"].Rows.Count > 0)
                {
                    foreach (DataRow dr in m_dsMap.Tables["MARKDATA"].Rows)
                    {
                        int iX = 0;
                        int iY = 0;

                        if (int.TryParse(dr["INDEX_X"].ToString(), out iX) == false)
                            continue;

                        if (int.TryParse(dr["INDEX_Y"].ToString(), out iY) == false)
                            continue;

                        if (m_wMapBF.IndexOf(iX, iY) < 0)
                        {
                            Base.Die oDie = new Base.Die();
                            oDie.IndexX = iX;
                            oDie.IndexY = iY;
                            oDie.DieProp = 2; // 실제 있는 Die 는 1번 Mark Die 는 2 번
                            oDie.BinNumber = 0;
                            m_wMapBF.AddDie(oDie);
                        }
                    }
                }

                FarPoint.Win.Spread.CellType.NumberCellType ct = new FarPoint.Win.Spread.CellType.NumberCellType();
                ct.DecimalPlaces = 0;
                m_wMapBF.Redraw();
                m_wMapBF.Refresh();
                m_wMapBF.Width = m_wMapBF.Width + 1; /// Size를 자동으로 조절하게 하는 Trip
                m_wMapBF.WaferDrawMode = Map.MapMode.Fit;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }

        public void DrawMapAF(DataSet m_dsMap)
        {
            try
            {
                if (m_dsMap == null)
                    return;

                m_wMapAF.SetDefaultColors();

                if (m_dsMap.Tables.IndexOf("MASTER_BIN") >= 0 && m_dsMap.Tables["MASTER_BIN"] != null && m_dsMap.Tables["MASTER_BIN"].Rows.Count > 0)
                {
                    DataTable dtBinSum = m_dsMap.Tables["MASTER_BIN"];

                    for (int i = 0; i < dtBinSum.Rows.Count; i++)
                    {
                        int nBin = -1;

                        if (int.TryParse(dtBinSum.Rows[i]["BIN"].ToString(), out nBin))
                        {
                            if (string.IsNullOrEmpty(dtBinSum.Rows[i]["BIN_COLOR"].ToString().Trim()) == false && dtBinSum.Rows[i]["BIN_COLOR"].ToString() != "#FFFFFF")
                            {
                                if (nBin > -1)
                                {
                                    m_wMapAF.SetColor(nBin, ColorTranslator.FromHtml(dtBinSum.Rows[i]["BIN_COLOR"].ToString()));
                                }
                            }
                        }
                    }
                }

                m_wMapAF.WaferSize = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());

                m_wMapAF.DieSizeX = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                m_wMapAF.DieSizeY = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());

                m_wMapAF.OriginIndexX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_wMapAF.OriginIndexY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());

                m_wMapAF.OriginX = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_wMapAF.OriginY = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());

                m_wMapAF.FirstDieX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                m_wMapAF.FirstDieY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());

                m_wMapAF.NotchAngle = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                m_wMapAF.EdgeSize = DACrux.Base.Convert.doubleParse(m_dsMap.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                m_wMapAF.NotchType = DACrux.Base.Notch.Notch; //m_dsMap.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                m_wMapAF.DieMinX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_X"].ToString());
                m_wMapAF.DieMinY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MIN_Y"].ToString());
                m_wMapAF.DieMaxX = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_X"].ToString());
                m_wMapAF.DieMaxY = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["DIE_INDEX_MAX_Y"].ToString());

                int iXYDir = DACrux.Base.Convert.intParse(m_dsMap.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        m_wMapAF.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        m_wMapAF.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        m_wMapAF.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        m_wMapAF.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }
                m_wMapAF.DieCalculation(true);

                // Map Bin Row Data ===================================================================================================
                m_wMapAF.DieClear();
                m_wMapAF.DataSource = m_dsMap.Tables["MAPDATA"];
                m_wMapAF.VisibleVIFail = true;
                m_wMapAF.VisibleFocusDie = true;
                m_wMapAF.VIMember = "BIN";
                m_wMapAF.DieFocusingType = Map.FocusType.Arraw;

                //Shot 관련 정보확인 및 Draw
                m_wMapAF.VisibleShot = false;
                if (m_dsMap.Tables.IndexOf("SHOT_DEF") >= 0 && m_dsMap.Tables["SHOT_DEF"] != null && m_dsMap.Tables["SHOT_DEF"].Rows.Count > 0)
                {
                    int iTemp = 0;
                    m_wMapAF.VisibleShot = true;
                    foreach (DataRow dr in m_dsMap.Tables["SHOT_DEF"].Rows)
                    {
                        if (int.TryParse(dr["ST_XCNT"].ToString(), out iTemp) == true)
                            m_wMapAF.ShotArrayX = iTemp;

                        if (int.TryParse(dr["ST_YCNT"].ToString(), out iTemp) == true)
                            m_wMapAF.ShotArrayY = iTemp;

                        if (int.TryParse(dr["ST_START_X"].ToString(), out iTemp) == true)
                            m_wMapAF.ShotStartX = iTemp;

                        if (int.TryParse(dr["ST_START_Y"].ToString(), out iTemp) == true)
                            m_wMapAF.ShotStartY = iTemp;
                    }
                }

                FarPoint.Win.Spread.CellType.NumberCellType ct = new FarPoint.Win.Spread.CellType.NumberCellType();
                ct.DecimalPlaces = 0;
                m_wMapAF.Redraw();
                m_wMapAF.Refresh();
                m_wMapAF.Width = m_wMapAF.Width + 1; /// Size를 자동으로 조절하게 하는 Trip
                m_wMapAF.WaferDrawMode = Map.MapMode.Fit;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }

        private void MapConfigAfterMap()
        {
            if(tabControl1.SelectedIndex != 1 || dsWaferOri == null || dsWaferOri.Tables.Count <= 0 || cmbArea.SelectedIndex < 0 || cmbDevice.SelectedIndex < 0 || string.IsNullOrEmpty(TxtProgram.Text))
                return;

            int rotate, shiftX, shiftY = -1;

            DataSet dsAfterMap = dsWaferOri.Copy();

            DataTable dtAFMap = dsAfterMap.Tables["MAPDATA"];

            string Dir = cmbDir.Text;
            Int32.TryParse(numAngle.Value.ToString(), out rotate);
            Int32.TryParse(numIndexX.Value.ToString(), out shiftX);
            Int32.TryParse(numIndexY.Value.ToString(), out shiftY);

            int xmax = int.Parse(dtAFMap.Compute("MAX([X])", "1=1").ToString());
            int ymax = int.Parse(dtAFMap.Compute("MAX([Y])", "1=1").ToString());
            int xmin = int.Parse(dtAFMap.Compute("MIN([X])", "1=1").ToString());
            int ymin = int.Parse(dtAFMap.Compute("MIN([Y])", "1=1").ToString());

            for (int ir = 0; ir < dtAFMap.Rows.Count; ir++)
            {
                //Shift 한다.
                int IndexX = Int32.Parse(dtAFMap.Rows[ir]["X"].ToString());
                int IndexY = Int32.Parse(dtAFMap.Rows[ir]["Y"].ToString());

                //1) Swap
                switch (Dir)
                {
                    case "LL":
                        //IndexX = IndexX;
                        //IndexY = IndexY;
                        break;
                    case "TL":
                        //IndexX = IndexX;
                        IndexY = (ymax + ymin - IndexY);
                        break;
                    case "TR":
                        IndexX = (xmax + xmin - IndexX);
                        IndexY = (ymax + ymin - IndexY);
                        break;
                    case "LR":
                        IndexX = (xmax + xmin - IndexX);
                        //IndexY = IndexY;
                        break;
                }

                // 2)회전
                Rotate(rotate, xmin, xmax, ymin, ymax, ref IndexX, ref IndexY);

                // 3)시프트
                IndexX += shiftX;
                IndexY += shiftY;

                dtAFMap.Rows[ir]["X"] = IndexX;
                dtAFMap.Rows[ir]["Y"] = IndexY;
            }

            dtAFMap.AcceptChanges();

            DrawMapAF(dsAfterMap);
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

        #endregion [ Map Control ]
    }
}
