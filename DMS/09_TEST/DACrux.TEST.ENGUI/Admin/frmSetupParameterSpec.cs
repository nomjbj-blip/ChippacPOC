using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DACrux.TEST.RO;
using DACrux.Framework.Controls;
using FarPoint.Win.Spread.CellType;

namespace DACrux.TEST.ENGUI
{
    public partial class frmSetupParameterSpec : DACrux.Framework.Base.DACruxUXBasic01
    {
        private enum ColumnIndex
        {
            PROGRAM = 0,
            PARAM_INDEX,
            PARAM_NAME,
            USER_PROGRAM_NAME,
            PARAM_TYPE,
            PARAM_DESC,
            TABLE_NAME,
            DECIMAL_PLACES,
            RUNTIME_DEFIEND,
            USE_FLAG,
            UOM
        }

        //--

        public frmSetupParameterSpec()
        {
            InitializeComponent();
        }

        #region Event Handler

        private void frmSetupParameterSpec_Load(
            object sender,
            EventArgs e
            )
        {
            ProbeAdmin oProbeAdmin = new ProbeAdmin();
            DataTable dt = oProbeAdmin.GetTestAreaList();
            FillControlData(
                duclbTestArea,
                dt,
                "TESTAREA",
                "TESTAREA"
                );

            dt = oProbeAdmin.GetProductList();
            FillControlData(
                duclbProduct,
                dt,
                "PRODUCT",
                "PRODUCT"
                );
        }

        //--

        private void btnSearch_Click(
            object sender,
            EventArgs e
            )
        {
            if (duclbProgram.SelectedValue == null)
                return;

            GetProgramParameterSpec(
                    duclbProgram.SelectedValue.ToString()
                    );
        }

        //--

        private void duclbTestArea_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (duclbTestArea.SelectedItems == null)
                return;

            //--

            ProbeAdmin oProbeAdmin = new ProbeAdmin();
            DataTable dt = oProbeAdmin.GetTestProgramList(
                Array.ConvertAll(duclbTestArea.SelectedValues, x => x.ToString()),
                null
                );

            FillControlData(
                duclbProgram,
                dt,
                "PROGRAM",
                "PROGRAM"
                );
        }

        //--

        private void duclbProduct_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (duclbTestArea.SelectedItems == null
                || duclbProduct.SelectedItems == null)
                return;

            //--

            ProbeAdmin oProbeAdmin = new ProbeAdmin();
            DataTable dt = oProbeAdmin.GetTestProgramList(
                Array.ConvertAll(duclbTestArea.SelectedValues, x => x.ToString()),
                Array.ConvertAll(duclbProduct.SelectedValues, x => x.ToString())
                );

            FillControlData(
                duclbProgram,
                dt,
                "PROGRAM",
                "PROGRAM"
                );
        }

        //--

        private void duclbProgram_OnSelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            if (duclbProgram.SelectedItem == null)
                return;

            GetProgramParameterSpec(
                duclbProgram.SelectedValue.ToString()
                );
        }

        //--

        private void grid_CommandButtonClick(
            object sender, 
            Framework.PropertyGrid.CommandEventArgs e
            )
        {
            ProbeAdmin oProbeAdmin = null;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                oProbeAdmin = new ProbeAdmin();
                if (e.Mode == Framework.PropertyGrid.CommandMode.Insert) 
                { 
                }
                else if (e.Mode == Framework.PropertyGrid.CommandMode.Update)
                { 
                }
                else if (e.Mode == Framework.PropertyGrid.CommandMode.Delete)
                { 
                }

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        //--

        private void grid_CommandComplete(
            object sender, 
            Framework.PropertyGrid.CommandEventArgs e
            )
        {
            btnSearch.PerformClick();
        }

        //--

        private void grid_DropDownComboBox(
            object sender, 
            Framework.PropertyGrid.DropDownComboBoxEventArgs e
            )
        {
            if (String.Equals(e.PropertyName, "USE_FLAG"))
            {
                e.ComboBoxValueList.Add(bool.FalseString);
                e.ComboBoxValueList.Add(bool.TrueString);
            }
            else if (String.Equals(e.PropertyName, "PARAM_TYPE"))
            {
                e.ComboBoxValueList.Add("INT");
                e.ComboBoxValueList.Add("FLOAT");
                e.ComboBoxValueList.Add("NUMBER");
                e.ComboBoxValueList.Add("STRING");
            }
            else if (String.Equals(e.PropertyName, "UOM"))
            {
                string[] uoms = new string[] { " ", "㎀", "㎁", "㎂", "㎃", "A", "㎴", "㎵", "㎷", "V", "㎱", "㎲", "㎳", "s", "Ω", "㏁", "㎐", "㎑", "㎒", "㎓", "㎊", "㎋", "㎌" };
                for (int idx = 0; idx < uoms.Length; idx++)
                {
                    e.ComboBoxValueList.Add(uoms[idx]);
                }
            }
        }

        //--

        #endregion Event Handler

        #region Method

        //--

        private void FillControlData(
            DUCListBox dlb,
            DataTable dt,
            String displayMember,
            String valueMember
            )
        {
            dlb.DisplayMember = displayMember;
            dlb.ValueMember = valueMember;
            dlb.DataSource = dt;
            dlb.DataBinding();
        }

        //--

        private void GetProgramParameterSpec(
            string program
            )
        {
            ProbeAdmin oProbeAdmin = new ProbeAdmin();
            DataTable dt = oProbeAdmin.GetParaSpecListEditable(
                DACrux.Base.GlobalVariable.Factory,
                program
                );

            //--

            dt.TableName = program;
            Utility.FPSpreadUtil.InitSpread(fpsParamSpec);
            Utility.FPSpreadUtil.SetSpreadData(dt, fpsParamSpec_Sheet);
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpsParamSpec_Sheet);
            //SetColumnStyle();
            grid.DataSource = dt;
        }

        //private void SetColumnStyle()
        //{
        //    CheckBoxCellType chkCellType = new CheckBoxCellType();
        //    chkCellType.ThreeState = true;

        //    //--

        //    ComboBoxCellType combDataTypeCellType = new ComboBoxCellType();
        //    combDataTypeCellType.Items = new string[] { "NUMBER", "STRING" };
        //    //--

        //    ComboBoxCellType combUomCellType = new ComboBoxCellType();
        //    combUomCellType.Items = new string[] { " ", "㎀", "㎁", "㎂", "㎃", "A", "㎴", "㎵", "㎷", "V", "㎱", "㎲", "㎳", "s", "Ω", "㏁", "㎐", "㎑", "㎒", "㎓", "㎊", "㎋", "㎌" };

        //    //--

        //    NumberCellType numCellType = new NumberCellType();
        //    numCellType.DecimalPlaces = 0;

        //    //--

        //    TextCellType txtCellType = new TextCellType();

        //    //--

        //    int itmp = 0;

        //    fpsParamSpec_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
        //    fpsParamSpec_Sheet.Columns[(int)ColumnIndex.PARAM_NAME].CellType = txtCellType;
        //    fpsParamSpec_Sheet.Columns[(int)ColumnIndex.USER_PROGRAM_NAME].CellType = txtCellType;
        //    fpsParamSpec_Sheet.Columns[(int)ColumnIndex.PARAM_TYPE].CellType = combDataTypeCellType;
        //    fpsParamSpec_Sheet.Columns[(int)ColumnIndex.PARAM_DESC].CellType = txtCellType;
        //    fpsParamSpec_Sheet.Columns[(int)ColumnIndex.DECIMAL_PLACES].CellType = numCellType;
        //    fpsParamSpec_Sheet.Columns[(int)ColumnIndex.RUNTIME_DEFIEND].CellType = numCellType;
        //    fpsParamSpec_Sheet.Columns[(int)ColumnIndex.USE_FLAG].CellType = chkCellType;
        //    fpsParamSpec_Sheet.Columns[(int)ColumnIndex.UOM].CellType = combUomCellType;
        //}

        #endregion Method

    }
}
