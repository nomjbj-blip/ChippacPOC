using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.ENGUI
{
    public partial class frmVisualInspDefine : DACrux.Framework.Base.DACruxUXBasic01
    {
        DataTable m_dt = null;
        public frmVisualInspDefine()
        {
            InitializeComponent();
        }

        private void frmVisualInspDefine_Load(object sender, EventArgs e)
        {
            try
            {
                FillData();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void FillData()
        {
            DACrux.TEST.RO.VisulalInspection oVisualInsp = null;
            try
            {
                oVisualInsp = new RO.VisulalInspection();
                m_dt = oVisualInsp.GetAVISpec();

                ListUpType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ListUpType()
        {
            try
            {
                DataTable dtSpecType = DACrux.Base.FilterEx.SelectDistinct("TYPE", m_dt, "INSPTYPE");
                lstSpecType.Items.Clear();
                for (int i = 0; i < dtSpecType.Rows.Count; i++)
                {
                    lstSpecType.Items.Add(dtSpecType.Rows[i]["INSPTYPE"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void lstSpecType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dv = null;
            try
            {
                dv = new DataView(m_dt, string.Format("INSPTYPE='{0}'", lstSpecType.Text), "BIN", DataViewRowState.CurrentRows);
                fpBin_Sheet1.DataSource = dv;

                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                FarPoint.Win.Spread.CellType.NumberCellType numCell_INT = new FarPoint.Win.Spread.CellType.NumberCellType();
                cmbCell.Items = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9" };
                numCell_INT.DecimalPlaces = 0;
                fpBin_Sheet1.Columns[0].CellType = numCell_INT;
                fpBin_Sheet1.Columns[fpBin_Sheet1.Columns.Count - 1].CellType = cmbCell;
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                m_dt.AcceptChanges();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }


    }
}
