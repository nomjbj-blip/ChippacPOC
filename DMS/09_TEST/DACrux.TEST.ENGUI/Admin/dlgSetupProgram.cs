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
    public partial class dlgSetupProgram : Form
    {
        #region [ Data Field ]
        private string TestArea = string.Empty;
        #endregion [ Data Field ]

        #region [ Constrator ]
        public dlgSetupProgram()
        {
            InitializeComponent();
        }

        public dlgSetupProgram(
            string testarea
            )
            : this()
        {
            TestArea = testarea;
        }
        #endregion [ Constrator ]

        #region [ Event Handler ]

        private void dlgSetupProgram_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            FillTestArea();
        }

        private void btnSave_Click(
            object sender,
            EventArgs e
            )
        {
            SaveData();
        }
        #endregion [ Event Handler ]

        #region [ Method ]
        private void FillTestArea()
        {
            RO.DataSelect obj = new RO.DataSelect();
            DataTable dt = obj.GetConditionTestArea();
            foreach (DataRow row in dt.Rows)
            {
                cmbTestArea.Items.Add(row["TESTAREA"].ToString());
            }

            if (!String.IsNullOrEmpty(TestArea))
                cmbTestArea.Text = TestArea;
        }

        private void SaveData()
        {
            RO.ProbeAdmin obj = new RO.ProbeAdmin();
            obj.CreateProgram(
                Base.GlobalVariable.Factory, 
                txtProgram.Text.ToUpper(),
                cmbTestArea.Text.ToUpper(),
                txtDevice.Text.ToUpper(), 
                Base.GlobalVariable.UserID
                );
        }
        #endregion [ Method ]

        #region [ Property ]
        public string ProgramName
        {
            get { return this.txtProgram.Text; }
            private set { this.txtProgram.Text = value; }
        }
        #endregion [ Property ]
    }
}
