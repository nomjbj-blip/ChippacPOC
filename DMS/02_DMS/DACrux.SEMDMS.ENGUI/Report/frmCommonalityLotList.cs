using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.Controls;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmCommonalityLotList : Form
    {
        public delegate void Selected(object sender, string[] strLotList);
        public event Selected OnSelected = null;

        DataTable dtHis;
        DataTable dtResult;

        enum TQC_LOT_HIS { FACTORY, LOT_ID, LOT_TYPE, TRAN_TIME, RECIPE, FLOW, FLOW_DESC, FLOW_NO, OPER, OPER_DESC, OPER_LONG_DESC, FLOW_OPER_SEQ, FLOW_AREA, RES_ID, RES_MODEL, RES_AREA }

        public frmCommonalityLotList()
        {
            InitializeComponent();
        }

        private void frmCommonalityLotList_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now;
            dtpEnd.Value = DateTime.Now;
        }

        private void BtnSearchLotList_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                fnClearControl();

                DACrux.Common.RO.ComConfiguration oCom = new Common.RO.ComConfiguration();
                dtHis = oCom.GetLotHisData(dtpStart.Value.ToString("yyyyMMdd00000000"), dtpEnd.Value.ToString("yyyyMMdd23595900"));
                if (dtHis == null || dtHis.Rows.Count <= 0)
                {
                    MessageBox.Show("데이터가 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lsLotCount.Text = "Total Lot List : 0";
                }

                dtResult = dtHis.Copy();
                lsLotCount.Text = string.Format("Total Lot List : {0}", dtHis.Rows.Count);

                fnDataGroup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            List<string> lsLotList = new List<string>();

            if (dtResult == null)
                return;

            foreach (DataRow dr in dtResult.Rows)
            {
                lsLotList.Add(dr["LOT_ID"].ToString());
            }

            if (lsLotList.Count > 0)
            {
                Array.Sort(lsLotList.ToArray(), StringComparer.InvariantCulture);
            }

            if (OnSelected != null) 
                OnSelected(this, lsLotList.ToArray());

            this.Close();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dlbFLOW_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilterData();
            fnDataGroupReFilter(TQC_LOT_HIS.FLOW.ToString(), dtResult);
        }

        private void dlbOPER_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilterData();
            fnDataGroupReFilter(TQC_LOT_HIS.OPER.ToString(), dtResult);
        }

        private void dlbRECIPE_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilterData();
            fnDataGroupReFilter(TQC_LOT_HIS.RECIPE.ToString(), dtResult);
        }

        private void dlbModel_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilterData();
            fnDataGroupReFilter(TQC_LOT_HIS.RES_MODEL.ToString(), dtResult);
        }

        private void dlbResID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilterData();
            fnDataGroupReFilter(TQC_LOT_HIS.RES_ID.ToString(), dtResult);
        }

        /// <summary>
        /// Data 기준으로 Grouping 하여 Filtering Control 에 넣는다.
        /// </summary>
        private void fnDataGroup()
        {
            DACrux.Common.RO.ComConfiguration oCom = new Common.RO.ComConfiguration();
            DataTable dtGroup = null;
            try
            {
                dtGroup = oCom.GetLotHisDataGroup(TQC_LOT_HIS.FLOW.ToString(), dtpStart.Value.ToString("yyyyMMdd00000000"), dtpEnd.Value.ToString("yyyyMMdd23595900"));
                SetBinding(dlbFLOW, dtGroup);

                dtGroup = oCom.GetLotHisDataGroup(TQC_LOT_HIS.OPER.ToString(), dtpStart.Value.ToString("yyyyMMdd00000000"), dtpEnd.Value.ToString("yyyyMMdd23595900"));
                SetBinding(dlbOPER, dtGroup);

                dtGroup = oCom.GetLotHisDataGroup(TQC_LOT_HIS.RECIPE.ToString(), dtpStart.Value.ToString("yyyyMMdd00000000"), dtpEnd.Value.ToString("yyyyMMdd23595900"));
                SetBinding(dlbRECIPE, dtGroup);

                dtGroup = oCom.GetLotHisDataGroup(TQC_LOT_HIS.RES_MODEL.ToString(), dtpStart.Value.ToString("yyyyMMdd00000000"), dtpEnd.Value.ToString("yyyyMMdd23595900"));
                SetBinding(dlbModel, dtGroup);

                dtGroup = oCom.GetLotHisDataGroup(TQC_LOT_HIS.RES_ID.ToString(), dtpStart.Value.ToString("yyyyMMdd00000000"), dtpEnd.Value.ToString("yyyyMMdd23595900"));
                SetBinding(dlbResID, dtGroup);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fnDataGroupReFilter(string strFilterItem, DataTable dtFilter)
        {
            if (dtFilter == null || dtFilter.Rows.Count <= 0)
                return;

            DataTable dtGroup = null;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (strFilterItem == "FLOW")
                {
                    dtGroup = dtFilter.DefaultView.ToTable(true, "OPER").Select("1 = 1", "OPER").CopyToDataTable<DataRow>();
                    SetBinding(dlbOPER, dtGroup);

                    dtGroup = dtFilter.DefaultView.ToTable(true, "RECIPE").Select("1 = 1", "RECIPE").CopyToDataTable<DataRow>();
                    SetBinding(dlbRECIPE, dtGroup);

                    dtGroup = dtFilter.DefaultView.ToTable(true, "RES_MODEL").Select("1 = 1", "RES_MODEL").CopyToDataTable<DataRow>();
                    SetBinding(dlbModel, dtGroup);

                    dtGroup = dtFilter.DefaultView.ToTable(true, "RES_ID").Select("1 = 1", "RES_ID").CopyToDataTable<DataRow>();
                    SetBinding(dlbResID, dtGroup);
                }

                if (strFilterItem == "OPER")
                {
                    dtGroup = dtFilter.DefaultView.ToTable(true, "RECIPE").Select("1 = 1", "RECIPE").CopyToDataTable<DataRow>();
                    SetBinding(dlbRECIPE, dtGroup);

                    dtGroup = dtFilter.DefaultView.ToTable(true, "RES_MODEL").Select("1 = 1", "RES_MODEL").CopyToDataTable<DataRow>();
                    SetBinding(dlbModel, dtGroup);

                    dtGroup = dtFilter.DefaultView.ToTable(true, "RES_ID").Select("1 = 1", "RES_ID").CopyToDataTable<DataRow>();
                    SetBinding(dlbResID, dtGroup);
                }

                if (strFilterItem == "RECIPE")
                {
                    dtGroup = dtFilter.DefaultView.ToTable(true, "RES_MODEL").Select("1 = 1", "RES_MODEL").CopyToDataTable<DataRow>();
                    SetBinding(dlbModel, dtGroup);

                    dtGroup = dtFilter.DefaultView.ToTable(true, "RES_ID").Select("1 = 1", "RES_ID").CopyToDataTable<DataRow>();
                    SetBinding(dlbResID, dtGroup);
                }

                if (strFilterItem == "RES_MODEL")
                {
                    dtGroup = dtFilter.DefaultView.ToTable(true, "RES_ID").Select("1 = 1", "RES_ID").CopyToDataTable<DataRow>();
                    SetBinding(dlbResID, dtGroup);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dtGroup != null)
                    dtGroup.Dispose();
                dtGroup = null;

                this.Cursor = Cursors.Default;
            }
        }


        /// <summary>
        /// Filter Control 기준으로 Data 를 Filtering 한다.
        /// </summary>
        private void SetFilterData()
        {
            if (dtHis == null || dtHis.Rows.Count <= 0)
                return;

            this.Cursor = Cursors.WaitCursor;

            string[] strItem = null;

            try
            {
                dtResult = dtHis.Copy();

                strItem = GetSelectedValues(dlbFLOW);
                if (strItem != null && strItem.Length> 0 && dtResult.Rows.Count > 0 && dtResult.Select(string.Format("FLOW IN ('{0}')", string.Join("','", strItem) )).Length > 0)
                {
                    dtResult = dtResult.Select(string.Format("FLOW IN ('{0}')", string.Join("','", strItem))).CopyToDataTable<DataRow>();
                }

                strItem = GetSelectedValues(dlbOPER);
                if (strItem != null && strItem.Length > 0 && dtResult.Rows.Count > 0 && dtResult.Select(string.Format("OPER IN ('{0}')", string.Join("','", strItem))).Length > 0)
                {
                    dtResult = dtResult.Select(string.Format("OPER IN ('{0}')", string.Join("','", strItem))).CopyToDataTable<DataRow>();
                }

                strItem = GetSelectedValues(dlbRECIPE);
                if (strItem != null && strItem.Length > 0 && dtResult.Rows.Count > 0 && dtResult.Select(string.Format("RECIPE IN ('{0}')", string.Join("','", strItem))).Length > 0)
                {
                    dtResult = dtResult.Select(string.Format("RECIPE IN ('{0}')", string.Join("','", strItem))).CopyToDataTable<DataRow>();
                }

                strItem = GetSelectedValues(dlbModel);
                if (strItem != null && strItem.Length > 0 && dtResult.Rows.Count > 0 && dtResult.Select(string.Format("RES_MODEL IN ('{0}')", string.Join("','", strItem))).Length > 0)
                {
                    dtResult = dtResult.Select(string.Format("RES_MODEL IN ('{0}')", string.Join("','", strItem))).CopyToDataTable<DataRow>();
                }

                strItem = GetSelectedValues(dlbResID);
                if (strItem != null && strItem.Length > 0 && dtResult.Rows.Count > 0 && dtResult.Select(string.Format("RES_ID IN ('{0}')", string.Join("','", strItem))).Length > 0)
                {
                    dtResult = dtResult.Select(string.Format("RES_ID IN ('{0}')", string.Join("','", strItem))).CopyToDataTable<DataRow>();
                }

                lsLotCount.Text = string.Format("Total Lot List : {0}", dtResult.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                 this.Cursor = Cursors.Default;
            }
        }

        private void SetBinding(
            DUCListBox listbox,
            DataTable source,
            int dispIndex = 0,
            int valueIndex = 0
            )
        {
            if (source != null)
            {
                listbox.DisplayMember = source.Columns[dispIndex].ColumnName;
                listbox.ValueMember = source.Columns[valueIndex].ColumnName;
            }

            listbox.DataSource = source;
        }

        private string[] GetSelectedValues(
            DUCListBox listBox
            )
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

        private void fnClearControl()
        {
            dlbFLOW.ClearDataSource();
            dlbOPER.ClearDataSource();
            dlbRECIPE.ClearDataSource();
            dlbModel.ClearDataSource();
            dlbResID.ClearDataSource();
        }

    }
}
