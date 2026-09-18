using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using DACrux.Base;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDMDataMaint : DACrux.Framework.Base.DACruxUXBasic01, DACrux.SEMDMS.Interface.iSEMControl
    {
        //쿼리랑 순서가 달라지면 골치 아픔...
        enum WAFER { LOT_ID, PRODUCT, STEP_ID, SLOT_ID, WAFER_ID, RESULTTIMESTAMP, DEFECTS, CLASSIFIED_DEFECTS, RANDOM_DEFECTS, ADDER_RANDOM_DEFECT, ADDER_CLUSTERS, DEFECT_DD, INSPECTED_DIE, IMAGES, SCAN_AREA, SETUP_SEQ, STEP_SEQ, WAFER_SEQ }
        enum LOT { LOT_ID, PRODUCT, STEP_ID, SLOT_ID, WAFER_ID, RESULTTIMESTAMP, DEFECTS, CLASSIFIED_DEFECTS, RANDOM_DEFECTS, ADDER_RANDOM_DEFECT, ADDER_CLUSTERS, DEFECT_DD, INSPECTED_DIE, IMAGES, SCAN_AREA, SETUP_SEQ, STEP_SEQ, WAFER_SEQ, LOT_SEQ }
        enum LOT_GRP { LOT_ID, STEP_ID, LOT_SEQ, PRODUCT }

        private DataSet dsDefect = null;

        public frmDMDataMaint()
        {
            InitializeComponent();
        }

        private void rbtType_CheckedChanged(object sender, EventArgs e)
        {

        }


        #region [ Method ]

        public void DrawWafer(DACrux.Base.DPWafer[] wafer)
        {
            try
            {
                this.DPWaferList = wafer;
                WaferDataView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }


        #endregion [ Method ]

        /// <summary>
        /// wafer 또는 Lot Base 로 Data 를 가져온다.
        /// </summary>
        private void WaferDataView()
        {
            if (this.DPWaferList == null || this.DPWaferList.Length <= 0)
                return;

            long[] lStepSeq = null;

            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;

            DataTable dtLotGrp = null;

            try
            {
                Utility.FPSpreadUtil.InitSpread(fpSpreadWafer);
                Utility.FPSpreadUtil.InitSpread(fpSpreadLot);
                Utility.FPSpreadUtil.InitSpread(fpSpreadLotDetail);
                Utility.FPSpreadUtil.InitSpread(fpSpreadHistory);

                oDMapAnalysis = new DACrux.SEMDMS.RO.DefectMapAnalysis();

                lStepSeq = new long[this.DPWaferList.Length];

                for (int iw = 0; iw < this.DPWaferList.Length; iw++)
                {
                    lStepSeq[iw] = DACrux.Base.Convert.longParse(this.DPWaferList[iw].StepSeq);
                }

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                dsDefect = oDMapAnalysis.GetDMMaintDataList(lStepSeq);
                if (dsDefect == null || dsDefect.Tables.Contains("WAFER") == false || dsDefect.Tables["WAFER"].Rows.Count <= 0)
                    throw new Exception("Wafer 의 정보가 없습니다.");

                InitializeWafer(dsDefect.Tables["WAFER"]);

                if (dsDefect == null || dsDefect.Tables.Contains("LOT") == false || dsDefect.Tables["LOT"].Rows.Count <= 0)
                    throw new Exception("Wafer 의 정보가 없습니다.");

                dtLotGrp = dsDefect.Tables["LOT"].DefaultView.ToTable(true, "LOT_ID", "STEP_ID", "LOT_SEQ", "PRODUCT");

                InitializeLot(dtLotGrp);

            }
            finally
            {
            }
        }

        /// <summary>
        /// Wafer 전체 List Up
        /// </summary>
        /// <param name="dtWafer"></param>
        public void InitializeWafer(DataTable dtWafer)
        {
            try
            {
                Utility.FPSpreadUtil.InitSpread(fpSpreadWafer);
                fpSpreadWafer.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

                fpSpreadWafer.ActiveSheet.Columns.Add(0, dtWafer.Columns.Count);
                fpSpreadWafer.ActiveSheet.Rows.Add(0, dtWafer.Rows.Count);

                for (int ic = 0; ic < dtWafer.Columns.Count; ic++)
                {
                    fpSpreadWafer.ActiveSheet.SetColumnLabel(0, ic, dtWafer.Columns[ic].ColumnName);
                    fpSpreadWafer.ActiveSheet.Columns[ic].BackColor = Color.LightGray;
                }

                //Cell 속성
                FarPoint.Win.Spread.CellType.TextCellType TxtCell = new FarPoint.Win.Spread.CellType.TextCellType();
                FarPoint.Win.Spread.CellType.NumberCellType NumCellInt = new FarPoint.Win.Spread.CellType.NumberCellType();
                NumCellInt.DecimalPlaces = 0;
                FarPoint.Win.Spread.CellType.NumberCellType NumCellDouble = new FarPoint.Win.Spread.CellType.NumberCellType();
                NumCellDouble.DecimalPlaces = 3;

                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.LOT_ID].CellType = TxtCell;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.WAFER_ID].CellType = TxtCell;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.PRODUCT].CellType = TxtCell;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.STEP_ID].CellType = TxtCell;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SLOT_ID].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.RESULTTIMESTAMP].CellType = TxtCell;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.DEFECTS].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.CLASSIFIED_DEFECTS].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.RANDOM_DEFECTS].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.ADDER_RANDOM_DEFECT].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.ADDER_CLUSTERS].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.DEFECT_DD].CellType = NumCellDouble;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.INSPECTED_DIE].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.IMAGES].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SCAN_AREA].CellType = NumCellDouble;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SETUP_SEQ].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.STEP_SEQ].CellType = NumCellInt;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.WAFER_SEQ].CellType = NumCellInt;

                //컬럼 사이즈
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.LOT_ID].Width = 80;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.WAFER_ID].Width = 100;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.PRODUCT].Width = 120;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.STEP_ID].Width = 120;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SLOT_ID].Width = 80;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.RESULTTIMESTAMP].Width = 160;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.DEFECTS].Width = 80;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.CLASSIFIED_DEFECTS].Width = 150;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.RANDOM_DEFECTS].Width = 150;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.ADDER_RANDOM_DEFECT].Width = 150;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.ADDER_CLUSTERS].Width = 150;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.DEFECT_DD].Width = 80;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.INSPECTED_DIE].Width = 120;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.IMAGES].Width = 80;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SCAN_AREA].Width = 80;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SETUP_SEQ].Width = 80;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.STEP_SEQ].Width = 80;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.WAFER_SEQ].Width = 80;


                //변경 가능한 항목에 대해 색상을 안넣는다.
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.LOT_ID].BackColor = Color.Empty;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.PRODUCT].BackColor = Color.Empty;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.STEP_ID].BackColor = Color.Empty;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SLOT_ID].BackColor = Color.Empty;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.WAFER_ID].BackColor = Color.Empty;

                //변경 항목 외에는 수정 가능 하지 못하게 한다.
                //fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.WAFER_ID].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.RESULTTIMESTAMP].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.DEFECTS].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.CLASSIFIED_DEFECTS].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.RANDOM_DEFECTS].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.ADDER_RANDOM_DEFECT].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.ADDER_CLUSTERS].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.DEFECT_DD].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.INSPECTED_DIE].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.IMAGES].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SCAN_AREA].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SETUP_SEQ].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.STEP_SEQ].Locked = true;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.WAFER_SEQ].Locked = true;


                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.SETUP_SEQ].Visible = false;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.STEP_SEQ].Visible = false;
                fpSpreadWafer.ActiveSheet.Columns[(int)WAFER.WAFER_SEQ].Visible = false;

                for (int ir = 0; ir < dtWafer.Rows.Count; ir++)
                {
                    DataRow drRow = dtWafer.Rows[ir];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.LOT_ID].Value = drRow[(int)WAFER.LOT_ID];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.WAFER_ID].Value = drRow[(int)WAFER.WAFER_ID];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.RESULTTIMESTAMP].Value = drRow[(int)WAFER.RESULTTIMESTAMP];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.PRODUCT].Value = drRow[(int)WAFER.PRODUCT];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.STEP_ID].Value = drRow[(int)WAFER.STEP_ID];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.SLOT_ID].Value = drRow[(int)WAFER.SLOT_ID];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.DEFECTS].Value = drRow[(int)WAFER.DEFECTS];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.CLASSIFIED_DEFECTS].Value = drRow[(int)WAFER.CLASSIFIED_DEFECTS];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.RANDOM_DEFECTS].Value = drRow[(int)WAFER.RANDOM_DEFECTS];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.ADDER_RANDOM_DEFECT].Value = drRow[(int)WAFER.ADDER_RANDOM_DEFECT];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.ADDER_CLUSTERS].Value = drRow[(int)WAFER.ADDER_CLUSTERS];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.DEFECT_DD].Value = drRow[(int)WAFER.DEFECT_DD];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.INSPECTED_DIE].Value = drRow[(int)WAFER.INSPECTED_DIE];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.IMAGES].Value = drRow[(int)WAFER.IMAGES];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.SCAN_AREA].Value = drRow[(int)WAFER.SCAN_AREA];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.SETUP_SEQ].Value = drRow[(int)WAFER.SETUP_SEQ];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.STEP_SEQ].Value = drRow[(int)WAFER.STEP_SEQ];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.WAFER_SEQ].Value = drRow[(int)WAFER.WAFER_SEQ];

                    //수정 여부 핀딘을 위해 Tag에 값을 넣는다.
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.LOT_ID].Tag = drRow[(int)WAFER.LOT_ID];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.WAFER_ID].Tag = drRow[(int)WAFER.WAFER_ID];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.PRODUCT].Tag = drRow[(int)WAFER.PRODUCT];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.STEP_ID].Tag = drRow[(int)WAFER.STEP_ID];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.SLOT_ID].Tag = drRow[(int)WAFER.SLOT_ID];
                    fpSpreadWafer.ActiveSheet.Cells[ir, (int)WAFER.DEFECTS].Tag = drRow[(int)WAFER.DEFECTS];
                }

                
            }
            finally
            {

            }
        }

        /// <summary>
        /// Lot / Step / Seq 기준 Grouping
        /// </summary>
        /// <param name="dtLotGrp"></param>
        public void InitializeLot(DataTable dtLotGrp)
        {
            try
            {
                Utility.FPSpreadUtil.InitSpread(fpSpreadLot);
                fpSpreadLot.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

                fpSpreadLot.ActiveSheet.Columns.Add(0, dtLotGrp.Columns.Count);
                fpSpreadLot.ActiveSheet.Rows.Add(0, dtLotGrp.Rows.Count);

                for (int ic = 0; ic < dtLotGrp.Columns.Count; ic++)
                {
                    fpSpreadLot.ActiveSheet.SetColumnLabel(0, ic, dtLotGrp.Columns[ic].ColumnName);
                    fpSpreadLot.ActiveSheet.Columns[ic].BackColor = Color.LightGray;
                }

                //Cell 속성
                FarPoint.Win.Spread.CellType.TextCellType TxtCell = new FarPoint.Win.Spread.CellType.TextCellType();
                FarPoint.Win.Spread.CellType.NumberCellType NumCellInt = new FarPoint.Win.Spread.CellType.NumberCellType();
                NumCellInt.DecimalPlaces = 0;

                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.LOT_ID].CellType = TxtCell;
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.STEP_ID].CellType = TxtCell;
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.LOT_SEQ].CellType = NumCellInt;
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.PRODUCT].CellType = TxtCell;


                //컬럼 사이즈
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.LOT_ID].Width = 80;
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.STEP_ID].Width = 120;
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.LOT_SEQ].Width = 80;
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.PRODUCT].Width = 100;

                //변경 가능한 항목에 대해 색상을 안넣는다.
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.LOT_ID].BackColor = Color.Empty;

                //변경 항목 외에는 수정 가능 하지 못하게 한다.
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.STEP_ID].Locked = true;
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.LOT_SEQ].Locked = true;
                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.PRODUCT].Locked = true;

                fpSpreadLot.ActiveSheet.Columns[(int)LOT_GRP.LOT_SEQ].Visible = false;

                for (int ir = 0; ir < dtLotGrp.Rows.Count; ir++)
                {
                    DataRow drRow = dtLotGrp.Rows[ir];
                    fpSpreadLot.ActiveSheet.Cells[ir, (int)LOT_GRP.LOT_ID].Value = drRow[(int)LOT_GRP.LOT_ID];
                    fpSpreadLot.ActiveSheet.Cells[ir, (int)LOT_GRP.STEP_ID].Value = drRow[(int)LOT_GRP.STEP_ID];
                    fpSpreadLot.ActiveSheet.Cells[ir, (int)LOT_GRP.LOT_SEQ].Value = drRow[(int)LOT_GRP.LOT_SEQ];
                    fpSpreadLot.ActiveSheet.Cells[ir, (int)LOT_GRP.PRODUCT].Value = drRow[(int)LOT_GRP.PRODUCT];

                    //수정 여부 핀딘을 위해 Tag에 값을 넣는다.
                    fpSpreadLot.ActiveSheet.Cells[ir, (int)LOT_GRP.LOT_ID].Tag = drRow[(int)LOT_GRP.LOT_ID];
                }
            }
            finally
            {

            }

        }

        /// <summary>
        /// Lot 선택 시 Detail 정보 표시
        /// </summary>
        /// <param name="dtLotDetail"></param>
        public void InitializeLotDetail(DataTable dtLotDetail)
        {
            try
            {
                Utility.FPSpreadUtil.InitSpread(fpSpreadLotDetail);
                fpSpreadLotDetail.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;

                fpSpreadLotDetail.ActiveSheet.Columns.Add(0, dtLotDetail.Columns.Count);
                fpSpreadLotDetail.ActiveSheet.Rows.Add(0, dtLotDetail.Rows.Count);

                for (int ic = 0; ic < dtLotDetail.Columns.Count; ic++)
                {
                    fpSpreadLotDetail.ActiveSheet.SetColumnLabel(0, ic, dtLotDetail.Columns[ic].ColumnName);
                    fpSpreadLotDetail.ActiveSheet.Columns[ic].BackColor = Color.LightGray;
                }

                //Cell 속성
                FarPoint.Win.Spread.CellType.TextCellType TxtCell = new FarPoint.Win.Spread.CellType.TextCellType();
                FarPoint.Win.Spread.CellType.NumberCellType NumCellInt = new FarPoint.Win.Spread.CellType.NumberCellType();
                NumCellInt.DecimalPlaces = 0;
                FarPoint.Win.Spread.CellType.NumberCellType NumCellDouble = new FarPoint.Win.Spread.CellType.NumberCellType();
                NumCellDouble.DecimalPlaces = 3;

                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.LOT_ID].CellType = TxtCell;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.WAFER_ID].CellType = TxtCell;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.PRODUCT].CellType = TxtCell;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.STEP_ID].CellType = TxtCell;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.SLOT_ID].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.RESULTTIMESTAMP].CellType = TxtCell;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.DEFECTS].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.CLASSIFIED_DEFECTS].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.RANDOM_DEFECTS].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.ADDER_RANDOM_DEFECT].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.ADDER_CLUSTERS].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.DEFECT_DD].CellType = NumCellDouble;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.INSPECTED_DIE].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.IMAGES].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.SCAN_AREA].CellType = NumCellDouble;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.SETUP_SEQ].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.STEP_SEQ].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.WAFER_SEQ].CellType = NumCellInt;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.LOT_SEQ].CellType = NumCellInt;

                //컬럼 사이즈
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.LOT_ID].Width = 80;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.WAFER_ID].Width = 100;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.PRODUCT].Width = 120;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.STEP_ID].Width = 120;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.SLOT_ID].Width = 80;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.RESULTTIMESTAMP].Width = 160;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.DEFECTS].Width = 80;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.CLASSIFIED_DEFECTS].Width = 150;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.RANDOM_DEFECTS].Width = 150;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.ADDER_RANDOM_DEFECT].Width = 150;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.ADDER_CLUSTERS].Width = 120;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.DEFECT_DD].Width = 80;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.INSPECTED_DIE].Width = 120;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.IMAGES].Width = 80;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.SCAN_AREA].Width = 80;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.SETUP_SEQ].Width = 80;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.STEP_SEQ].Width = 80;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.WAFER_SEQ].Width = 80;


                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.SETUP_SEQ].Visible = false;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.STEP_SEQ].Visible = false;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.WAFER_SEQ].Visible = false;
                fpSpreadLotDetail.ActiveSheet.Columns[(int)LOT.LOT_SEQ].Visible = false;

                for (int ir = 0; ir < dtLotDetail.Rows.Count; ir++)
                {
                    DataRow drRow = dtLotDetail.Rows[ir];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.LOT_ID].Value = drRow[(int)LOT.LOT_ID];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.WAFER_ID].Value = drRow[(int)LOT.WAFER_ID];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.RESULTTIMESTAMP].Value = drRow[(int)LOT.RESULTTIMESTAMP];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.PRODUCT].Value = drRow[(int)LOT.PRODUCT];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.STEP_ID].Value = drRow[(int)LOT.STEP_ID];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.SLOT_ID].Value = drRow[(int)LOT.SLOT_ID];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.DEFECTS].Value = drRow[(int)LOT.DEFECTS];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.CLASSIFIED_DEFECTS].Value = drRow[(int)LOT.CLASSIFIED_DEFECTS];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.RANDOM_DEFECTS].Value = drRow[(int)LOT.RANDOM_DEFECTS];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.ADDER_RANDOM_DEFECT].Value = drRow[(int)LOT.ADDER_RANDOM_DEFECT];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.ADDER_CLUSTERS].Value = drRow[(int)LOT.ADDER_CLUSTERS];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.DEFECT_DD].Value = drRow[(int)LOT.DEFECT_DD];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.INSPECTED_DIE].Value = drRow[(int)LOT.INSPECTED_DIE];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.IMAGES].Value = drRow[(int)LOT.IMAGES];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.SCAN_AREA].Value = drRow[(int)LOT.SCAN_AREA];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.SETUP_SEQ].Value = drRow[(int)LOT.SETUP_SEQ];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.STEP_SEQ].Value = drRow[(int)LOT.STEP_SEQ];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.WAFER_SEQ].Value = drRow[(int)LOT.WAFER_SEQ];
                    fpSpreadLotDetail.ActiveSheet.Cells[ir, (int)LOT.LOT_SEQ].Value = drRow[(int)LOT.LOT_SEQ];
                }
            }
            finally
            {

            }
        }

        #region [ Event Handler ]

        private void frmDMDataMaint_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now.AddDays(-7);
            dtpEnd.Value = DateTime.Now;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;

            try
            {
                oDMapAnalysis = new DACrux.SEMDMS.RO.DefectMapAnalysis();

                if(string.IsNullOrEmpty(TxtComment.Text.Trim()) == true)
                    throw new Exception("Comment 를 입력 바랍니다.");

                if(tcMain.SelectedIndex == 0)
                {
                    //Wafer Base 저장
                    if (MessageBox.Show("수정된 Wafer 정보들을 저장 하시겠습니까?", "생성", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    string strStepSeq = string.Empty;
                    string strLotID = string.Empty;
                    string strSlotID = string.Empty;
                    string strStepID = string.Empty;
                    string strProduct = string.Empty;
                    string strWaferID = string.Empty;

                    MainForm.SetStatusMessage("Data 를 저장 중입니다.");
                    //Validation 용도로 한번 확인 한다.
                    for(int ir = 0; ir < fpSpreadWafer.ActiveSheet.Rows.Count; ir++)
                    {
                        if (fpSpreadWafer.ActiveSheet.Rows[ir].Tag == null || fpSpreadWafer.ActiveSheet.Rows[ir].Tag.ToString() != "EDIT")
                            continue;

                        strStepSeq = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.STEP_SEQ).ToString().ToUpper();
                        strLotID = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.LOT_ID).ToString().ToUpper();
                        strSlotID = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.SLOT_ID).ToString().ToUpper();
                        strStepID = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.STEP_ID).ToString().ToUpper();
                        strProduct = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.PRODUCT).ToString().ToUpper();
                        strWaferID = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.WAFER_ID).ToString().ToUpper();

                        if(string.IsNullOrEmpty(strStepSeq) || string.IsNullOrEmpty(strLotID) 
                            || string.IsNullOrEmpty(strSlotID) || string.IsNullOrEmpty(strStepID)
                            || string.IsNullOrEmpty(strProduct) || string.IsNullOrEmpty(strWaferID))
                            throw new Exception("수정된 Wafer 중에 입력이 안된 정보가 있습니다. 다시 확인 바랍니다.");
                    }

                    for(int ir = 0; ir < fpSpreadWafer.ActiveSheet.Rows.Count; ir++)
                    {
                        if (fpSpreadWafer.ActiveSheet.Rows[ir].Tag == null || fpSpreadWafer.ActiveSheet.Rows[ir].Tag.ToString() != "EDIT")
                            continue;

                        strStepSeq = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.STEP_SEQ).ToString().ToUpper();
                        strLotID = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.LOT_ID).ToString().ToUpper();
                        strSlotID = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.SLOT_ID).ToString().ToUpper();
                        strStepID = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.STEP_ID).ToString().ToUpper();
                        strProduct = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.PRODUCT).ToString().ToUpper();
                        strWaferID = fpSpreadWafer.ActiveSheet.GetValue(ir, (int)WAFER.WAFER_ID).ToString().ToUpper();

                        MainForm.SetStatusMessage(string.Format("Data 를 저장 중입니다. Lot ID : {0}, Slot ID : {1}", strLotID, strSlotID));
                        oDMapAnalysis.SetDMMaintDataUpdateWafer(strStepSeq, strWaferID, strLotID, int.Parse(strSlotID), strStepID, strProduct, DACrux.Base.GlobalVariable.UserID, TxtComment.Text, DACrux.Base.GlobalVariable.LocalIP);
                        fpSpreadWafer.ActiveSheet.Rows[ir].BackColor = Color.Gray;
                        fpSpreadWafer.ActiveSheet.Rows[ir].ForeColor = Color.Black;
                    }

                    MessageBox.Show("정상 처리 하였습니다. 다시 조회 바랍니다.");

                    Utility.FPSpreadUtil.InitSpread(fpSpreadWafer);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadLot);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadLotDetail);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadHistory);
                }
                else if(tcMain.SelectedIndex == 1)
                {
                    //Lot Base 저장
                    if (MessageBox.Show("수정된 Wafer 정보들을 저장 하시겠습니까?", "생성", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    MainForm.SetStatusMessage("Data 를 저장 중입니다.");
                    string strLotSeq = string.Empty;
                    string strStepID = string.Empty;
                    string strLotID= string.Empty;
                    string strOriLotID = string.Empty;
                    string strProduct = string.Empty;
                    List<string> lsStepSeq = new List<string>();


                    //Validation 용도로 한번 확인 한다.
                    for (int ir = 0; ir < fpSpreadLot.ActiveSheet.Rows.Count; ir++)
                    {
                        if (fpSpreadLot.ActiveSheet.Rows[ir].Tag == null || fpSpreadLot.ActiveSheet.Rows[ir].Tag.ToString() != "EDIT")
                            continue;

                        strLotSeq = fpSpreadLot.ActiveSheet.GetValue(ir, (int)LOT_GRP.LOT_SEQ).ToString().ToUpper();
                        strLotID = fpSpreadLot.ActiveSheet.GetValue(ir, (int)LOT_GRP.LOT_ID).ToString().ToUpper();
                        strStepID = fpSpreadLot.ActiveSheet.GetValue(ir, (int)LOT_GRP.STEP_ID).ToString().ToUpper();
                        strProduct = fpSpreadLot.ActiveSheet.GetValue(ir, (int)LOT_GRP.PRODUCT).ToString().ToUpper();

                        if (string.IsNullOrEmpty(strLotSeq) || string.IsNullOrEmpty(strLotID))
                            throw new Exception("수정된 Wafer 중에 입력이 안된 정보가 있습니다. 다시 확인 바랍니다.");
                    }

                    for (int ir = 0; ir < fpSpreadLot.ActiveSheet.Rows.Count; ir++)
                    {
                        if (fpSpreadLot.ActiveSheet.Rows[ir].Tag == null || fpSpreadLot.ActiveSheet.Rows[ir].Tag.ToString() != "EDIT")
                            continue;

                        strLotSeq = fpSpreadLot.ActiveSheet.GetValue(ir, (int)LOT_GRP.LOT_SEQ).ToString().ToUpper();
                        strLotID = fpSpreadLot.ActiveSheet.GetValue(ir, (int)LOT_GRP.LOT_ID).ToString().ToUpper();
                        strStepID = fpSpreadLot.ActiveSheet.GetValue(ir, (int)LOT_GRP.STEP_ID).ToString().ToUpper();
                        strProduct = fpSpreadLot.ActiveSheet.GetValue(ir, (int)LOT_GRP.PRODUCT).ToString().ToUpper();

                        if(fpSpreadLot.ActiveSheet.Cells[ir, (int)WAFER.LOT_ID].Tag != null)
                            strOriLotID = fpSpreadLot.ActiveSheet.Cells[ir, (int)WAFER.LOT_ID].Tag.ToString();

                        DataTable dtLotList = dsDefect.Tables["LOT"].Select(string.Format("LOT_SEQ = '{0}' AND STEP_ID = '{1}'", strLotSeq, strStepID)).CopyToDataTable<DataRow>();

                        foreach (DataRow dr in dtLotList.Rows)
                        {
                            lsStepSeq.Add(dr["STEP_SEQ"].ToString());
                        }

                        MainForm.SetStatusMessage(string.Format("Data 를 저장 중입니다. Lot ID : {0}", strLotID));
                        oDMapAnalysis.SetDMMaintDataUpdateLot(lsStepSeq.ToArray(), strLotSeq, strLotID, strOriLotID, strStepID, strProduct, DACrux.Base.GlobalVariable.UserID, TxtComment.Text, DACrux.Base.GlobalVariable.LocalIP);
                        fpSpreadLot.ActiveSheet.Rows[ir].BackColor = Color.Gray;
                        fpSpreadLot.ActiveSheet.Rows[ir].ForeColor = Color.Black;
                    }

                    MessageBox.Show("정상 처리 하였습니다. 다시 조회 바랍니다.");

                    Utility.FPSpreadUtil.InitSpread(fpSpreadWafer);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadLot);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadLotDetail);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadHistory);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            FarPoint.Win.Spread.Model.CellRange[] oSelectItem = null;
            string strLotID = string.Empty;
            string strSlotID = string.Empty;
            string strStepSeq = string.Empty;
            string strLotSeq = string.Empty;
            string strStepID = string.Empty;
            int idx = -1;

            try
            {
                oDMapAnalysis = new DACrux.SEMDMS.RO.DefectMapAnalysis();
                
                if (string.IsNullOrEmpty(TxtComment.Text.Trim()) == true)
                    throw new Exception("Comment 를 입력 바랍니다.");

                if (tcMain.SelectedIndex == 0)
                {
                    //Wafer Base 삭제
                    if (MessageBox.Show("선택한 Wafer 정보들을 삭제 하시겠습니까?\n삭제 후 원복 할 수 없습니다.", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    oSelectItem = fpSpreadWafer.ActiveSheet.GetSelections();

                    for (int i = 0; i < oSelectItem.Length; i++)
                    {
                        for (int cr = 0; cr < oSelectItem[i].RowCount; cr++)
                        {
                            idx = oSelectItem[i].Row + cr;
                            strLotID = fpSpreadWafer.ActiveSheet.GetValue(idx, (int)WAFER.LOT_ID).ToString().ToUpper();
                            strSlotID = fpSpreadWafer.ActiveSheet.GetValue(idx, (int)WAFER.SLOT_ID).ToString().ToUpper();
                            strStepSeq = fpSpreadWafer.ActiveSheet.GetValue(idx, (int)WAFER.STEP_SEQ).ToString().ToUpper();

                            MainForm.SetStatusMessage(string.Format("Data 를 삭제 중입니다. Lot ID : {0}, Slot ID : {1}", strLotID, strSlotID));

                            oDMapAnalysis.SetDMMaintDataDeleteWafer(strStepSeq, DACrux.Base.GlobalVariable.UserID, TxtComment.Text, DACrux.Base.GlobalVariable.LocalIP);
                            fpSpreadWafer.ActiveSheet.Rows[idx].BackColor = Color.Gray;
                            fpSpreadWafer.ActiveSheet.Rows[idx].ForeColor = Color.Black;
                        }
                    }

                    MessageBox.Show("정상 처리 하였습니다. 다시 조회 바랍니다.");

                    Utility.FPSpreadUtil.InitSpread(fpSpreadWafer);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadLot);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadLotDetail);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadHistory);
                }
                else if (tcMain.SelectedIndex == 1)
                {
                    //Lot Base 삭제
                    if (MessageBox.Show("선택한 Lot 정보들을 삭제 하시겠습니까?\n삭제 후 원복 할 수 없습니다.", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    oSelectItem = fpSpreadLot.ActiveSheet.GetSelections();

                    List<string> lsStepSeq = new List<string>();
                    for (int i = 0; i < oSelectItem.Length; i++)
                    {
                        for (int cr = 0; cr < oSelectItem[i].RowCount; cr++)
                        {
                            idx = oSelectItem[i].Row + cr;
                            strLotSeq = fpSpreadLot.ActiveSheet.GetValue(idx, (int)LOT_GRP.LOT_SEQ).ToString().ToUpper();
                            strStepID = fpSpreadLot.ActiveSheet.GetValue(idx, (int)LOT_GRP.STEP_ID).ToString().ToUpper();
                            strLotID = fpSpreadLot.ActiveSheet.GetValue(idx, (int)LOT_GRP.LOT_ID).ToString().ToUpper();

                            DataTable dtLotList = dsDefect.Tables["LOT"].Select(string.Format("LOT_SEQ = '{0}' AND STEP_ID = '{1}'", strLotSeq, strStepID)).CopyToDataTable<DataRow>();

                            foreach (DataRow dr in dtLotList.Rows)
                            {
                                lsStepSeq.Add(dr["STEP_SEQ"].ToString());
                            }

                            MainForm.SetStatusMessage(string.Format("Data 를 삭제 중입니다. Lot ID : {0}, Step ID : {1}", strLotID, strStepID));

                            oDMapAnalysis.SetDMMaintDataDeleteLot(lsStepSeq.ToArray(), DACrux.Base.GlobalVariable.UserID, TxtComment.Text, DACrux.Base.GlobalVariable.LocalIP);
                            fpSpreadLot.ActiveSheet.Rows[idx].BackColor = Color.Gray;
                            fpSpreadLot.ActiveSheet.Rows[idx].ForeColor = Color.Black;
                        }
                    }

                    MessageBox.Show("정상 처리 하였습니다. 다시 조회 바랍니다.");

                    Utility.FPSpreadUtil.InitSpread(fpSpreadWafer);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadLot);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadLotDetail);
                    Utility.FPSpreadUtil.InitSpread(fpSpreadHistory);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void fpSpreadWafer_EditModeOff(object sender, EventArgs e)
        {
            int iRow = fpSpreadWafer.ActiveSheet.ActiveRowIndex;
            int iCol =  fpSpreadWafer.ActiveSheet.ActiveColumnIndex;
           
            if (string.IsNullOrEmpty(fpSpreadWafer.ActiveSheet.Cells[iRow, iCol].Text.Trim()))
                return;

            //수정 된것에 대한 Flag
            if (fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.LOT_ID].Value.ToString() != fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.LOT_ID].Tag.ToString()
                || fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.STEP_ID].Value.ToString() != fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.STEP_ID].Tag.ToString()
                || fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.PRODUCT].Value.ToString() != fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.PRODUCT].Tag.ToString()
                || fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.SLOT_ID].Value.ToString() != fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.SLOT_ID].Tag.ToString()
                || fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.WAFER_ID].Value.ToString() != fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.WAFER_ID].Tag.ToString())
            {
                fpSpreadWafer.ActiveSheet.Rows[iRow].Tag = "EDIT";
                fpSpreadWafer.ActiveSheet.Rows[iRow].ForeColor = Color.Red;

                if (iCol == (int)WAFER.LOT_ID || iCol == (int)WAFER.SLOT_ID)
                {
                    fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.WAFER_ID].Value = string.Format("{0}-{1:00}", fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.LOT_ID].Value, fpSpreadWafer.ActiveSheet.Cells[iRow, (int)WAFER.SLOT_ID].Value);
                }
            }
            else
            {
                fpSpreadWafer.ActiveSheet.Rows[iRow].Tag = "";
                fpSpreadWafer.ActiveSheet.Rows[iRow].ForeColor = Color.Black;
            }


        }

        private void fpSpreadLot_EditModeOff(object sender, EventArgs e)
        {
            int iRow = fpSpreadLot.ActiveSheet.ActiveRowIndex;
            int iCol = fpSpreadLot.ActiveSheet.ActiveColumnIndex;

            if (string.IsNullOrEmpty(fpSpreadLot.ActiveSheet.Cells[fpSpreadLot.ActiveSheet.ActiveRowIndex, fpSpreadLot.ActiveSheet.ActiveColumnIndex].Text.Trim()))
                return;

            //수정 된것에 대한 Flag
            if (fpSpreadLot.ActiveSheet.Cells[iRow, (int)WAFER.LOT_ID].Value.ToString() != fpSpreadLot.ActiveSheet.Cells[iRow, (int)WAFER.LOT_ID].Tag.ToString())
            {
                fpSpreadLot.ActiveSheet.Rows[iRow].Tag = "EDIT";
                fpSpreadLot.ActiveSheet.Rows[iRow].ForeColor = Color.Red;
            }
            else
            {
                fpSpreadLot.ActiveSheet.Rows[iRow].Tag = "";
                fpSpreadLot.ActiveSheet.Rows[iRow].ForeColor = Color.Black;
            }
        }


        private void fpSpreadLot_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            Utility.FPSpreadUtil.InitSpread(fpSpreadLotDetail);

            if (dsDefect == null || dsDefect.Tables.Contains("LOT") == false || dsDefect.Tables["LOT"].Rows.Count <= 0)
                return;

            DataTable dtSelectLot = null;

            try
            {
                string strLotSeq = fpSpreadLot.ActiveSheet.Cells[e.Row, (int)LOT_GRP.LOT_SEQ].Value.ToString();
                string strStepID = fpSpreadLot.ActiveSheet.Cells[e.Row, (int)LOT_GRP.STEP_ID].Value.ToString();

                if (dsDefect.Tables["LOT"].Select(string.Format("LOT_SEQ = {0} AND STEP_ID = '{1}'", strLotSeq, strStepID)).Length > 0)
                {
                    dtSelectLot = dsDefect.Tables["LOT"].Select(string.Format("LOT_SEQ = {0} AND STEP_ID = '{1}'", strLotSeq, strStepID)).CopyToDataTable<DataRow>();
                    InitializeLotDetail(dtSelectLot);
                }
            }
            finally
            {
            }
        }


        private void BtnView_Click(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;

            DataTable dtMaintHis = null;

            try
            {
                oDMapAnalysis = new DefectMapAnalysis();

                Utility.FPSpreadUtil.InitSpread(fpSpreadHistory);

                if(string.IsNullOrEmpty(TxtLotID.Text.Trim()) == true)
                    dtMaintHis = oDMapAnalysis.GetMaintHisTime(dtpStart.Value.ToString("yyyyMMdd00000000"), dtpEnd.Value.ToString("yyyyMMdd23595900"));
                else
                    dtMaintHis = oDMapAnalysis.GetMaintHisLotID(TxtLotID.Text.Trim().ToUpper().Replace("*", "%"));

                if(dtMaintHis == null || dtMaintHis.Rows.Count <= 0)
                {
                    MessageBox.Show("Not Found Data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                fpSpreadHistory.DataSource = dtMaintHis;
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadHistory.ActiveSheet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }

        }


        private void TxtLotID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
                BtnView_Click(null, null);
        }
        #endregion [ Event Handler ]













    }
}
