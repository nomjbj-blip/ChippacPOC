using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DACrux.Base;
using DACrux.Utility;

namespace DACrux.TEST.ENGUI
{
    public partial class frmTestDataMaint : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region 멤버변수

        public static readonly int LOT_LENGTH = 6;
        public static readonly int WAFER_LENGTH = 9;
        public static readonly int PROGRAM_LENGTH = 4;

        public static readonly string DATE_FORMAT = "yyyyMMdd";

        // 주의. WAFER_SEQ 이후는 임의로 컬럼을 추가하지 말것.
        enum Col
        {
            CHECK,
            WAFER_ID,
            NUM,
            WAFER_SEQ,
            DIE_NUM,
            X,
            Y,
            DATA
        }

        enum HisCol
        {
            TRAN_TIME,
            FACTORY,
            TESTAREA,
            PRODUCT,
            PROGRAM,
            LOT_ID,
            WAFER_ID,
            USER_ID,
            IP_ADDRESS,
            CATEGORY,
            TYPE,
            COUNT,
            PREV,
            CURR,
            COMMENT
        }

        enum TranCategory
        {
            RAWDATA, WAFER, LOT, FILE_UPLOAD
        }

        enum TranType
        {
            INSERT, UPDATE, DELETE, CHANGE_PROGRAM
        }
        
        #endregion

        #region 생성자 및 Load 이벤트

        public frmTestDataMaint()
        {
            InitializeComponent();
        }

        private void frmTestDataMaint_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            dtpStart.Value = DateTime.Now.Date.AddDays(-30);
            dtpEnd.Value = DateTime.Now.Date;

            dtpStartHis.Value = DateTime.Now.Date.AddDays(-30);
            dtpEndHis.Value = DateTime.Now.Date;

            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetConditionTestArea();
            SetBinding(lstTestArea, dt);

            lstWafer_OnSelectedIndexChanged(lstWafer, EventArgs.Empty);
            lstWafer.ListBox.MouseDoubleClick += lstWafer_MouseDoubleClick;
            FPSpreadUtil.InitSpread(fpSpread2);

            pnlUpload.Enabled = false;
        }
        
        #endregion

        #region 사용자 정의 메서드

        private void SetBinding(DACrux.Framework.Controls.DUCListBox listBox, DataTable source, int dispIndex = 0, int valueIndex = 0)
        {
            if (source != null)
            {
                listBox.DisplayMember = source.Columns[dispIndex].ColumnName;
                listBox.ValueMember = source.Columns[valueIndex].ColumnName;
            }

            listBox.DataSource = source;
        }

        private string[] GetSelectedValues(DACrux.Framework.Controls.DUCListBox listBox)
        {
            DataTable dt = listBox.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return null;

            object[] arr = listBox.SelectedValues;

            if (arr == null || arr.Length == 0)
                return null;

            string[] results = new string[arr.Length];

            for (int i = 0; i < arr.Length; i++)
                results[i] = arr[i].ToString();

            return results;
        }

        private string GetSelectedValue(DACrux.Framework.Controls.DUCListBox listBox)
        {
            string[] arr = GetSelectedValues(listBox);

            if (arr == null || arr.Length == 0)
                return null;

            return arr[0];
        }

        private bool ShowUserCommentPopup(string description, out string userComment)
        {
            using (frmTestDataUserComment popup = new frmTestDataUserComment())
            {
                popup.Description = description;

                if (popup.ShowDialog() == DialogResult.OK)
                {
                    userComment = popup.UserComment;
                    return true;
                }
            }

            userComment = null;
            return false;
        }

        private void InsertChangeHis(TranCategory category, TranType ttype, int tranCount, string prevValue, string currValue, string userComment)
        {
            RO.TestCommon obj = new RO.TestCommon();
            obj.InsertChangeHis(GlobalVariable.Factory, lstTestArea.Text, lstDevice.Text, lstProgram.Text, lstLot.Text, lstWafer.Text,
                GlobalVariable.UserID, GlobalVariable.LocalIP, category.ToString(), ttype.ToString(), tranCount, prevValue, currValue, userComment);
        }

        private string GetStartDate(DateTimePicker picker)
        {
            return picker.Value.ToString(DATE_FORMAT);
        }

        private string GetEndDate(DateTimePicker picker)
        {
            return picker.Value.AddDays(1).ToString(DATE_FORMAT);
        }

        private void RefreshControl()
        {
            ClearAll();
            lstDevice_OnSelectedIndexChanged(lstDevice, EventArgs.Empty);
        }

        private void ClearAll()
        {
            fpSpread1_Sheet1.RowCount = 0;

            txtCurrWaferID.Clear();
            txtCurrWaferSeq.Clear();
            txtCurrWafProgram.Clear();

            txtNewWaferID.Clear();
            txtNewWafProgram.Clear();

            txtCurrLotID.Clear();
            txtCurrLotSeq.Clear();
            txtCurrLotProgram.Clear();

            txtNewLotID.Clear();
            txtNewLotProgram.Clear();
        }

        private void SetDecimalLength()
        {
            int decimalLen = (int)numDecimalLength.Value;
            int start = (int)Col.DATA;
            // 데이터 시작 필드가 BIN 인 경우 DECIMAL LENGTH = 0 으로 설정
            if (fpSpread1_Sheet1.Columns[(int)Col.DATA].Label == "BIN" || fpSpread1_Sheet1.ColumnHeader.Cells[0, (int)Col.DATA].Text == "BIN")
            {
                DACrux.Utility.FPSpreadUtil.SetDecimalLength(fpSpread1_Sheet1, 0, Col.DATA);
                start++;
            }

            for (int i = (int)Col.DATA + 1; i < fpSpread1_Sheet1.ColumnCount; i++)
            {
                DACrux.Utility.FPSpreadUtil.SetDecimalLength(fpSpread1_Sheet1, decimalLen, new int[] { i });
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1, false, 1, new int[] { i });
            }

            fpSpread1.Refresh();
        }

        #endregion

        #region 조회 조건 관련

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            lstTestArea_OnSelectedIndexChanged(null, EventArgs.Empty);
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            lstTestArea_OnSelectedIndexChanged(null, EventArgs.Empty);
        }

        private void lstTestArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTestArea.SelectedIndex < 0)
                return;

            pnlUpload.Enabled = true;
            lstDevice.ClearDataSource();
            lstProgram.ClearDataSource();
            lstLot.ClearDataSource();
            lstWafer.ClearDataSource();

            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetConditionDevice(GetStartDate(dtpStart), GetEndDate(dtpEnd), GetSelectedValues(lstTestArea));
            SetBinding(lstDevice, dt);
        }

        private void lstDevice_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDevice.SelectedIndex < 0)
                return;

            lstProgram.ClearDataSource();
            lstLot.ClearDataSource();
            lstWafer.ClearDataSource();

            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetConditionProgram(GetStartDate(dtpStart), GetEndDate(dtpEnd), GetSelectedValues(lstTestArea), GetSelectedValues(lstDevice));
            SetBinding(lstProgram, dt);
        }

        private void lstProgram_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtCurrLotProgram.Clear();
            txtCurrWafProgram.Clear();
            txtNewWafProgram.Clear();
            txtNewLotProgram.Clear();

            if (lstProgram.SelectedIndex < 0)
                return;

            lstLot.ClearDataSource();
            lstWafer.ClearDataSource();

            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetConditionLot(GetStartDate(dtpStart), GetEndDate(dtpEnd), GetSelectedValues(lstTestArea), GetSelectedValues(lstDevice), GetSelectedValues(lstProgram));
            SetBinding(lstLot, dt, 0, 1);

            txtNewLotProgram.Text = txtCurrLotProgram.Text = txtNewWafProgram.Text = txtCurrWafProgram.Text = GetSelectedValue(lstProgram);
        }

        private void lstLot_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtCurrLotID.Clear();
            txtNewLotID.Clear();
            pnlLot.Enabled = false;

            if (lstLot.SelectedIndex < 0)
                return;

            pnlLot.Enabled = true;
            lstWafer.ClearDataSource();

            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetConditionWafer(GetSelectedValue(lstProgram), GetSelectedValues(lstLot));
            SetBinding(lstWafer, dt, 0, 1);

            txtNewLotID.Text = txtCurrLotID.Text = lstLot.Text;
            txtCurrLotSeq.Text = lstLot.SelectedValue.ToString();

            if (lstTestArea.Text == "AVI")
            {
                txtNewLotProgram.Enabled = false;
                txtNewWafProgram.Enabled = false;
            }
            else
            {
                txtNewLotProgram.Enabled = true;
                txtNewWafProgram.Enabled = true;
            }
        }

        private void lstWafer_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtCurrWaferID.Clear();
            txtNewWaferID.Clear();
            txtCurrWaferLotID.Clear();
            pnlWafer.Enabled = pnlRawdata.Enabled = false;
            fpSpread1_Sheet1.RowCount = fpSpread1_Sheet1.ColumnCount = 0;

            if (lstWafer.SelectedIndex < 0)
                return;

            pnlWafer.Enabled = pnlRawdata.Enabled = true;
            int probeCnt = -1;
            txtNewWaferID.Text = txtCurrWaferID.Text = GetWaferID(lstWafer.Text, out probeCnt);
            txtProbeCnt.Text = probeCnt.ToString();
            nudProbeCnt.Value = probeCnt;
            txtNewWaferLotID.Text = txtCurrWaferLotID.Text = lstLot.Text;
            txtCurrWaferSeq.Text = lstWafer.SelectedValue.ToString();
            txtCurrWaferLotSeq.Text = lstLot.SelectedValue.ToString();

            if (lstTestArea.Text == "AVI")
            {
                txtNewLotProgram.Enabled = false;
                txtNewWafProgram.Enabled = false;
            }
            else
            {
                txtNewLotProgram.Enabled = true;
                txtNewWafProgram.Enabled = true;
            }
        }

        private void lstWafer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void lstLot_EnterTextBox(object sender, EventArgs e)
        {
            // TextBox에서 Enter 입력 시 PRODUCT, PROGRAM 을 재조회한 후 선택한다.
            // PRODUCT의 경우 0번째 항목으로 선택한다.
            // PROGRAM의 경우 해당 LOT에 대해 조회된 PROGRAM 이 여러개인 경우 선택하지 않고 1개인 경우만 선택한다.
            string search = lstLot.SearchText;

            if (String.IsNullOrEmpty(lstTestArea.Text))
            {
                ShowErrorMessage("TEST AREA 를 선택하세요.");
                return;
            }

            if (String.IsNullOrEmpty(search))
                return;

            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetLotInfo(lstTestArea.Text, GetStartDate(dtpStart), GetEndDate(dtpEnd), search);

            if (dt == null || dt.Rows.Count == 0)
            {
                ShowMessage("데이터를 찾을 수 없습니다.");
                return;
            }

            // PRODUCT의 경우 0번째 항목으로 선택한다.
            lstDevice.Text = dt.Rows[0]["DEVICE"].ToString();

            // PROGRAM이 1개인 경우 선택해준다.
            if (dt.Rows.Count > 0)
                lstProgram.Text = dt.Rows[0]["PROGRAM"].ToString();
            lstLot.SearchText = search;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //AVI Data 변경은 제외
            if (lstProgram.SelectedIndex < 0 || lstWafer.SelectedIndex < 0)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                 
                MainForm.SetStatusMessage("데이터 조회중입니다...");
                RO.DataSelect obj = new RO.DataSelect();
                DataTable dt = null;

                if (lstTestArea.Text == "AVI")
                    dt = obj.GetAVIRawData(GetSelectedValues(lstWafer));
                else
                    dt = obj.GetAllRawData_Comp(GetSelectedValues(lstWafer));

                dt.AcceptChanges();
                fpSpread1_Sheet1.RowCount = fpSpread1_Sheet1.ColumnCount = 0;
                fpSpread1_Sheet1.DataSource = dt;

                MainForm.SetStatusMessage(String.Format("{0:N0}건의 데이터를 바인딩 중입니다...", dt.Rows.Count));

                fpSpread1_Sheet1.Columns[(int)Col.WAFER_SEQ].Visible = false;
                fpSpread1_Sheet1.Columns[(int)Col.DIE_NUM].Visible = false;

                DACrux.Utility.FPSpreadUtil.SetDecimalLength(fpSpread1_Sheet1, 0, Col.NUM, Col.X, Col.Y);
                
                // 컬럼 너비 자동 조정
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1, 5);//5row 까지만 길이 체크

                // CHECKBOX 컬럼
                fpSpread1_Sheet1.Columns[(int)Col.CHECK].Label = " ";
                fpSpread1_Sheet1.Columns[(int)Col.CHECK].Width = 20;
                fpSpread1_Sheet1.Columns[(int)Col.CHECK].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

                fpSpread1_Sheet1.Columns[(int)Col.X].Width = fpSpread1_Sheet1.Columns[(int)Col.Y].Width = fpSpread1_Sheet1.Columns[(int)Col.NUM].Width = 50;

                SetDecimalLength();

                for (int i = 0; i < (int)Col.DATA; i++)
                {
                    fpSpread1_Sheet1.Columns[i].Locked = true;
                    fpSpread1_Sheet1.Columns[i].BackColor = Color.LightGray;
                }

                int iCol = dt.Columns.IndexOf("BIN");
                if (iCol > -1 && lstTestArea.Text != "AVI")
                {
                    fpSpread1_Sheet1.Columns[iCol].Locked = true;
                    fpSpread1_Sheet1.Columns[iCol].BackColor = Color.LightGray;
                }

                iCol = dt.Columns.IndexOf("SOFTBIN");
                if (iCol > -1 && lstTestArea.Text != "AVI")
                {
                    fpSpread1_Sheet1.Columns[iCol].Locked = true;
                    fpSpread1_Sheet1.Columns[iCol].BackColor = Color.LightGray;
                }

                fpSpread1_Sheet1.FrozenColumnCount = (int)Col.DATA;
                pnlRawdata.Enabled = fpSpread1_Sheet1.RowCount > 0;
            }
            finally
            {
                MainForm.SetStatusMessage(null);
                Cursor = Cursors.Default;
            }
        }

        private void fpSpread1_Sheet1_CellChanged(object sender, FarPoint.Win.Spread.SheetViewEventArgs e)
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return;

            fpSpread1_Sheet1.Cells[e.Row, (int)Col.CHECK].Value = (dt.Rows[e.Row].RowState == DataRowState.Modified);
        }

        #endregion

        #region RAWDATA

        private void btnRawdataUpdate_Click(object sender, EventArgs e)
        {
            DataTable dt = fpSpread1_Sheet1.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
            {
                ShowErrorMessage("업데이트할 데이터가 없습니다.");
                return;
            }

            DataTable changed = dt.GetChanges();

            if (changed == null || changed.Rows.Count == 0)
            {
                ShowErrorMessage("변경된 데이터가 없습니다.");
                return;
            }
            
            // 변경 사유 입력
            string userComment;
            
            if (!ShowUserCommentPopup(String.Format("{0}건의 데이터를 업데이트 하시겠습니까?", changed.Rows.Count), out userComment))
                return;

            // BULK COPY를 위해 WAFER_SEQ 앞의 데이터는 모두 삭제 처리
            for (int i = 0; i < (int)Col.WAFER_SEQ; i++)
                changed.Columns.RemoveAt(0);

            RO.TestCommon obj = new RO.TestCommon();
            if (lstTestArea.Text == "AVI")
            {
                obj.MergeAVITableData(GlobalVariable.Factory, changed);
            }
            else
            {
                obj.MergeTdTableData(GlobalVariable.Factory, changed);
            }

            InsertChangeHis(TranCategory.RAWDATA, TranType.UPDATE, changed.Rows.Count, null, null, userComment);

            ShowMessage("업데이트를 완료하였습니다.");
            btnSearch.PerformClick();
        }
        
        #endregion

        #region FILE UPLOAD

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtUploadFile.Clear();
                    txtUploadFile.Text = String.Join(Environment.NewLine, dlg.FileNames);
                }
            }
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUploadFile.Text))
            {
                ShowErrorMessage("파일을 선택하세요.");
                return;
            }

            foreach (string file in txtUploadFile.Lines)
            {
                if (!File.Exists(file))
                {
                    ShowErrorMessage("파일을 찾을 수 없습니다.");
                    return;
                }
            }

            using (frmTestDataUpload_Pcm popup = new frmTestDataUpload_Pcm())
            {
                popup.Oper = lstTestArea.Text;
                popup.FileNames = txtUploadFile.Lines;

                if (popup.ShowDialog() == DialogResult.OK)
                {
                    InsertChangeHis(TranCategory.FILE_UPLOAD, TranType.INSERT, 1, null, null, popup.UserComment);
                    txtUploadFile.Clear();
                }
            }
        }

        #endregion

        #region WAFER

        private void btnUpdateWaferLotID_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCurrWaferLotID.Text))
            {
                ShowErrorMessage("실행할 수 없습니다.");
                return;
            }

            txtNewWaferLotID.Text = txtNewWaferLotID.Text.Trim();

            if (String.IsNullOrEmpty(txtNewWaferLotID.Text))
            {
                ShowErrorMessage("새 Lot ID 가 없습니다.");
                return;
            }

            if (txtCurrWaferLotID.Text == txtNewWaferLotID.Text)
            {
                ShowErrorMessage("현재 Lot ID와 새 Lot ID가 같습니다.");
                return;
            }

            if (txtNewWaferLotID.TextLength < LOT_LENGTH)
            {
                ShowErrorMessage(String.Format("Lot ID 길이는 최소 {0} 이상 이어야 합니다.", LOT_LENGTH));
                return;
            }

            // 변경 사유 입력
            string userComment;

            if (!ShowUserCommentPopup(String.Format("해당 Lot ID를 '{0}' 로 변경하시겠습니까?", txtNewWaferLotID.Text), out userComment))
                return;

            // TODO
            RO.TestDataMaint obj = new RO.TestDataMaint();
            obj.UpdateWaferLotID(GlobalVariable.Factory, txtCurrWaferLotSeq.Text, txtCurrWaferSeq.Text, txtNewWaferLotID.Text);

            // change history
            InsertChangeHis(TranCategory.WAFER, TranType.UPDATE, 1, txtCurrWaferLotID.Text, txtNewWaferLotID.Text, userComment);

            ShowMessage("업데이트를 완료하였습니다.");
            RefreshControl();
        }

        private void btnUpdateWaferID_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCurrWaferID.Text))
            {
                ShowErrorMessage("실행할 수 없습니다.");
                return;
            }

            txtNewWaferID.Text = txtNewWaferID.Text.Trim();

            if (String.IsNullOrEmpty(txtNewWaferID.Text))
            {
                ShowErrorMessage("새 Wafer ID 가 없습니다.");
                return;
            }

            if (String.Equals(txtProbeCnt.Text, nudProbeCnt.Value.ToString())
                && String.Equals(txtNewWaferID.Text, txtCurrWaferID.Text))
            {
                ShowErrorMessage("현재 Wafer ID와 새 Wafer ID가 같습니다.");
                return;
            }

            if (!String.Equals(txtProbeCnt.Text, nudProbeCnt.Value.ToString())
                && !String.Equals(txtNewWaferID.Text, txtCurrWaferID.Text))
            {
                ShowErrorMessage(String.Format("{0} -> {1} 로 Probe Count를 변경할 수 없습니다.", txtProbeCnt.Text, ((int)nudProbeCnt.Value).ToString()));
                return;
            }

            if (txtNewWaferID.TextLength < WAFER_LENGTH)
            {
                ShowErrorMessage(String.Format("Wafer ID 길이는 최소 {0} 이상 이어야 합니다.", WAFER_LENGTH));
                return;
            }

            // 변경 사유 입력
            string userComment;

            if (!ShowUserCommentPopup(String.Format("해당 Wafer ID를 '{0}' 로 변경하시겠습니까?", txtNewWaferID.Text), out userComment))
                return;

            // TODO
            RO.TestDataMaint obj = new RO.TestDataMaint();
            obj.UpdateWaferID(txtCurrWaferSeq.Text, txtNewWaferID.Text, (int)nudProbeCnt.Value);

            // change history
            String newWaferId = String.Format("{0} ({1})", txtNewWaferID.Text, (int)nudProbeCnt.Value);
            //InsertChangeHis(TranCategory.WAFER, TranType.UPDATE, 1, txtCurrWaferID.Text, txtNewWaferID.Text, userComment);
            InsertChangeHis(TranCategory.WAFER, TranType.UPDATE, 1, txtCurrWaferID.Text, newWaferId, userComment);

            ShowMessage("업데이트를 완료하였습니다.");
            RefreshControl();
        }

        private void btnUpdateWafProgram_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCurrWafProgram.Text))
            {
                ShowErrorMessage("실행할 수 없습니다.");
                return;
            }

            txtNewWafProgram.Text = txtNewWafProgram.Text.Trim();

            if (String.IsNullOrEmpty(txtNewWafProgram.Text))
            {
                ShowErrorMessage("새 PROGRAM 명이 없습니다.");
                return;
            }

            if (txtCurrWafProgram.Text == txtNewWafProgram.Text)
            {
                ShowErrorMessage("현재 PROGRAM 명과 새 PROGRAM 명이 같습니다.");
                return;
            }

            if (txtNewWafProgram.TextLength < PROGRAM_LENGTH)
            {
                ShowErrorMessage(String.Format("프로그램 길이는 최소 {0} 이상 이어야 합니다.", PROGRAM_LENGTH));
                return;
            }

            // 변경 사유 입력
            string userComment;

            if (!ShowUserCommentPopup(String.Format("해당 WAFER 에 대한 PROGRAM 명을 '{0}' 로 변경하시겠습니까?", txtNewWafProgram.Text), out userComment))
                return;

            // TODO
            RO.TestDataMaint obj = new RO.TestDataMaint();
            obj.UpdateWaferProgram(GlobalVariable.Factory, txtCurrLotSeq.Text, txtCurrWaferSeq.Text, txtNewWafProgram.Text);

            // change history
            InsertChangeHis(TranCategory.WAFER, TranType.CHANGE_PROGRAM, 1, txtCurrWafProgram.Text, txtNewWafProgram.Text, userComment);

            ShowMessage("PROGRAM 명 변경을 완료하였습니다.");
            RefreshControl();
        }

        private void btnDeleteWaferData_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCurrWaferID.Text))
            {
                ShowErrorMessage("선택된 Wafer ID가 없습니다.");
                return;
            }

            if (!ShowQuestionMessage(String.Format("'{0}' WAFER 데이터를 삭제하시겠습니까?", txtCurrWaferID.Text)))
                return;

            // 변경 사유 입력
            string userComment;

            if (!ShowUserCommentPopup("이 작업은 되돌릴 수 없습니다. 정말 삭제하시겠습니까?", out userComment))
                return;

            // ID/PASSWORD 입력 확인

            // 삭제 후 해당 테이블에 대한 데이터 건수가 0인 경우 TQP_DATA_TABLES, TQP_PARASPEC, TD_{테이블} 모두 삭제 처리한다.
            RO.TestDataMaint obj = new RO.TestDataMaint();
            obj.DeleteWaferData(txtCurrWaferSeq.Text);

            // change history
            InsertChangeHis(TranCategory.WAFER, TranType.DELETE, 1, txtCurrWaferID.Text, null, userComment);

            ShowMessage("삭제를 완료하였습니다.");
            RefreshControl();
        }

        private void numDecimalLength_ValueChanged(object sender, EventArgs e)
        {
            SetDecimalLength();
        }

        private string GetWaferID(
            string waferID,
            out int probeCnt
            )
        {
            String tmp = String.Empty;
            if (waferID.Contains(" "))
            {
                tmp = waferID.Substring(waferID.IndexOf('(') + 1, waferID.LastIndexOf(')') - (waferID.IndexOf('(') + 1));
                probeCnt = Base.Convert.intParse(tmp);
                return waferID.Substring(0, waferID.LastIndexOf(' '));
            }
            else
            {
                probeCnt = 0;
                return waferID;
            }
        }

        #endregion

        #region LOT

        private void btnUpdateLotID_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCurrLotID.Text))
            {
                ShowErrorMessage("실행할 수 없습니다.");
                return;
            }

            txtNewLotID.Text = txtNewLotID.Text.Trim();

            if (String.IsNullOrEmpty(txtNewLotID.Text))
            {
                ShowErrorMessage("새 Lot ID 가 없습니다.");
                return;
            }

            if (txtCurrLotID.Text == txtNewLotID.Text)
            {
                ShowErrorMessage("현재 Lot ID와 새 Lot ID가 같습니다.");
                return;
            }

            if (txtNewLotID.TextLength < LOT_LENGTH)
            {
                ShowErrorMessage(String.Format("Lot ID 길이는 최소 {0} 이상 이어야 합니다.", LOT_LENGTH));
                return;
            }

            // 변경 사유 입력
            string userComment;

            if (!ShowUserCommentPopup(String.Format("Lot ID를 '{0}' 로 변경하시겠습니까?", txtNewLotID.Text), out userComment))
                return;

            // TODO
            RO.TestDataMaint obj = new RO.TestDataMaint();
            obj.UpdateLotID(txtCurrLotSeq.Text, txtNewLotID.Text);

            // change history
            InsertChangeHis(TranCategory.LOT, TranType.UPDATE, 1, txtCurrLotID.Text, txtNewLotID.Text, userComment);

            ShowMessage("Lot ID 변경을 완료하였습니다.");
            RefreshControl();
        }

        private void btnUpdateLotProgram_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCurrLotProgram.Text))
            {
                ShowErrorMessage("실행할 수 없습니다.");
                return;
            }

            txtNewLotProgram.Text = txtNewLotProgram.Text.Trim();

            if (String.IsNullOrEmpty(txtNewLotProgram.Text))
            {
                ShowErrorMessage("새 PROGRAM 명이 없습니다.");
                return;
            }

            if (txtCurrLotProgram.Text == txtNewLotProgram.Text)
            {
                ShowErrorMessage("현재 PROGRAM 명과 새 PROGRAM 명이 같습니다.");
                return;
            }

            if (txtNewLotProgram.TextLength < PROGRAM_LENGTH)
            {
                ShowErrorMessage(String.Format("프로그램 길이는 최소 {0} 이상 이어야 합니다.", PROGRAM_LENGTH));
                return;
            }

            // 변경 사유 입력
            string userComment;

            if (!ShowUserCommentPopup(String.Format("PROGRAM 명을 '{0}' 로 변경하시겠습니까?", txtNewLotProgram.Text), out userComment))
                return;

            // TODO
            RO.TestDataMaint obj = new RO.TestDataMaint();
            obj.UpdateLotProgram(GlobalVariable.Factory, txtCurrLotSeq.Text, txtCurrLotProgram.Text, txtNewLotProgram.Text);

            // change history
            InsertChangeHis(TranCategory.LOT, TranType.CHANGE_PROGRAM, 1, txtCurrLotProgram.Text, txtNewLotProgram.Text, userComment);

            ShowMessage("PROGRAM 명 변경을 완료하였습니다.");
            RefreshControl();
        }

        private void btnDeleteLotData_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCurrLotID.Text))
            {
                ShowErrorMessage("선택된 Lot ID가 없습니다.");
                return;
            }

            if (!ShowQuestionMessage(String.Format("'{0}' LOT 데이터를 삭제하시겠습니까?", txtCurrLotID.Text)))
                return;

            // 변경 사유 입력
            string userComment;

            if (!ShowUserCommentPopup("이 작업은 되돌릴 수 없습니다. 정말 삭제하시겠습니까?", out userComment))
                return;
            
            // 삭제 후 해당 테이블에 대한 데이터 건수가 0인 경우 TQP_DATA_TABLES, TQP_PARASPEC, TD_{테이블} 모두 삭제 처리한다.
            RO.TestDataMaint obj = new RO.TestDataMaint();
            obj.DeleteLotData(txtCurrLotSeq.Text, txtCurrLotProgram.Text);

            // change history
            InsertChangeHis(TranCategory.LOT, TranType.DELETE, 1, txtCurrLotID.Text, null, userComment);

            ShowMessage("삭제를 완료하였습니다.");
            RefreshControl();
        }
        
        #endregion

        #region HISTORY

        private void btnSearchHis_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.SetStatusMessage("데이터 조회중입니다...");

                RO.TestCommon obj = new RO.TestCommon();
                DataTable dt = obj.GetChangeHisData(GlobalVariable.Factory, GetSelectedValue(lstTestArea), dtpStartHis.Value.ToString("yyyyMMdd"), dtpEndHis.Value.AddDays(1).ToString("yyyyMMdd"));
                fpSpread2_Sheet1.DataSource = dt;

                FPSpreadUtil.SetAutoColumnSort(fpSpread2_Sheet1);
                FPSpreadUtil.SetAutoColumnFilter(fpSpread2_Sheet1);
                FPSpreadUtil.SetAutoColumnWidth(fpSpread2_Sheet1);
                FPSpreadUtil.SetDecimalLength(fpSpread2_Sheet1, 0, HisCol.COUNT);
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        } 

        #endregion
    }
}
