using System;
using System.Data;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using Infragistics.Win.Misc;
using System.Collections.Generic;
using DACrux.Common.RO;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmGlobalConfiguration : DACrux.Framework.Base.DACruxUXBasic01
    {
        private enum TQC_CONFIG { CATEGORY, NAME, VALUE, TYPE, COMMENT }
        private DataTable dtCategoryList = null;
        private DataTable dtCategoryGroup = null;

        public frmGlobalConfiguration(
            )
        {
            InitializeComponent();
        }

        #region [ Event Hadler ]

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetConfigData();
        }

        private void frmGlobalConfiguration_Load(object sender, EventArgs e)
        {
            GetConfigData();
            InitProperty();
        }

        private void grid_CommandButtonClick(object sender, Framework.PropertyGrid.CommandEventArgs e)
        {
            DataTable dtTemp = null;

            DACrux.Common.RO.ComConfiguration oCom = new DACrux.Common.RO.ComConfiguration();

            string strCategory = gridProperty.GetValue("CATEGORY").ToString();
            string strName = gridProperty.GetValue("NAME").ToString();
            string strVALUE = gridProperty.GetValue("VALUE").ToString();
            string strTYPE = gridProperty.GetValue("TYPE").ToString();
            string strDESC = gridProperty.GetValue("COMMENT").ToString();

            DialogResult result;
            switch (e.Mode)
            {
                case Framework.PropertyGrid.CommandMode.Insert:


                    dtTemp = oCom.GetConfigListDuple(strCategory, strName);

                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        MessageBox.Show("해당 데이터가 이미 존재합니다.");
                        e.Cancel = true;
                        return;
                    }

                    //--

                    result = MessageBox.Show("저장하시겠습니까?", "입력", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        oCom.InsertConfiagData(strCategory, strName, strVALUE, strTYPE, strDESC);
                        MessageBox.Show("저장이 완료 되었습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GetConfigData();
                    }
                    else
                    {
                        e.Cancel = true;
                    }

                    break;

                case Framework.PropertyGrid.CommandMode.Update:

                    result = MessageBox.Show("변경하시겠습니까", "수정", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        oCom.UpdateConfiagData(strCategory, strName, strVALUE, strTYPE, strDESC);
                        MessageBox.Show("저장이 완료 되었습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GetConfigData();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;

                case Framework.PropertyGrid.CommandMode.Delete:

                    result = MessageBox.Show("삭제하시겠습니까", "삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        oCom.DeleteConfiagData(strCategory, strName);
                        MessageBox.Show("저장이 완료 되었습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GetConfigData();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;

                case Framework.PropertyGrid.CommandMode.None:
                default:
                    break;
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetConfigDataList(cmbCategory.SelectedItem.ToString());
        }

        private void fpSpreadConfig_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            try
            {
                //string strCategory = fpSpreadConfig.ActiveSheet.Cells[e.Row, (int)TQC_CONFIG.CATEGORY].Text;
                //string strNAME = fpSpreadConfig.ActiveSheet.Cells[e.Row, (int)TQC_CONFIG.NAME].Text;

                //gridProperty.DataSource = null;

                //if(dtCategoryList == null || dtCategoryList.Rows.Count <=0 || dtCategoryList.Select(string.Format("CATEGORY = '{0}' AND NAME = '{1}'", strCategory, strNAME)).Length <= 0)
                //    return;

                //gridProperty.DataSource = dtCategoryList.Select(string.Format("CATEGORY = '{0}' AND NAME = '{1}'", strCategory, strNAME)).CopyToDataTable<DataRow>();
            }
            finally
            {

            }

        }


        #endregion [ Event Hadler ]

        //--

        #region [ Method ]

        private void InitProperty()
        {
            gridProperty.PropertyList.Clear();

            DACrux.Framework.PropertyGrid.PropertyItem pi = null;

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "CATEGORY";
            pi.DisplayName = "Category";
            gridProperty.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "NAME";
            pi.DisplayName = "Name";
            gridProperty.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "VALUE";
            pi.DisplayName = "Value";
            gridProperty.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "TYPE";
            pi.DisplayName = "Type";
            gridProperty.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "COMMENT";
            pi.DisplayName = "Comment";
            gridProperty.PropertyList.Add(pi);

        }

        private void GetConfigData()
        {
            DACrux.Common.RO.ComConfiguration oCom = new DACrux.Common.RO.ComConfiguration();

            cmbCategory.Items.Clear();
            cmbCategory.Text = "";

            Utility.FPSpreadUtil.InitSpread(fpSpreadConfig);

            if (dtCategoryList != null)
                dtCategoryList.Dispose();

            dtCategoryList = null;

            if (dtCategoryGroup != null)
                dtCategoryGroup.Dispose();

            dtCategoryGroup = null;

            dtCategoryList = oCom.GetConfigListAll();
            if (dtCategoryList != null && dtCategoryList.Rows.Count > 0)
            {
                dtCategoryGroup = dtCategoryList.DefaultView.ToTable(true, "CATEGORY").Select("1 = 1", "CATEGORY").CopyToDataTable<DataRow>();

                foreach(DataRow dr in dtCategoryGroup.Rows)
                {
                    cmbCategory.Items.Add(dr["CATEGORY"].ToString());
                }
            }
        }

        private void SetConfigDataList(string strCategory)
        {
            Utility.FPSpreadUtil.InitSpread(fpSpreadConfig);

            if (dtCategoryList == null || dtCategoryList.Rows.Count <= 0 || dtCategoryList.Select(string.Format("CATEGORY = '{0}'", strCategory)).Length <= 0)
                return;

            DataTable dtRow = dtCategoryList.Select(string.Format("CATEGORY = '{0}'", strCategory)).CopyToDataTable<DataRow>();

            Utility.FPSpreadUtil.SetSpreadData(dtRow, fpSpreadConfig.ActiveSheet);
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadConfig.ActiveSheet);

            gridProperty.DataSource = dtRow;
        }

        #endregion [ Method ]

    }
}
