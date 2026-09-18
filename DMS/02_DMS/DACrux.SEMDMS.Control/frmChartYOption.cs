using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
    public partial class frmChartYOption : Form
    {
        public frmChartYOption(
            )
        {
            InitializeComponent();
        }

        public frmChartYOption(
            DefectGubun gubun
            )
            : this()
        {
            New = gubun.New;
            CarrayOver = gubun.CarryOver;
            Cluster = gubun.Cluster;
            Random = gubun.Random;
        }

        //-----------------------------------------------------------------------------

        #region [ Event Handler ]

        //--

        //private void btnClose_Click(
        //    object sender,
        //    EventArgs e
        //    )
        //{
        //    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //    this.Close();
        //}

        //--

        //private void btnOk_Click(
        //    object sender,
        //    EventArgs e
        //    )
        //{
        //    this.DialogResult = System.Windows.Forms.DialogResult.OK;
        //    this.Close();
        //}

        #endregion [ Event Handler ]

        //-----------------------------------------------------------------------------

        #region [ Properties ]

        /// <summary>
        /// TQD_DEFECT 테이블에서 Adder Column 의 값이 1이상인 것
        /// </summary>
        public bool New
        {
            get { return chkNew.Checked; }
            private set { chkNew.Checked = value; }
        }

        /// <summary>
        /// TQD_DEFECT 테이블에서 Adder Column 의 값이 0 인 것
        /// </summary>
        public bool CarrayOver
        {
            get { return chkCarryOver.Checked; }
            private set { chkCarryOver.Checked = value; }
        }

        /// <summary>
        /// TQD_DEFECT 테이블에서 Cluster Column 의 값이 1 이상 인 것
        /// </summary>
        public bool Cluster
        {
            get { return chkCluster.Checked; }
            private set { chkCluster.Checked = value; }
        }

        /// <summary>
        /// TQD_DEFECT 테이블에서 Cluster Column 의 값이 0 인 것
        /// </summary>
        public bool Random
        {
            get { return chkRandom.Checked; }
            private set { chkRandom.Checked = value; }
        }

        public DefectGubun Gubun
        {
            get
            {
                return new DefectGubun()
                {
                    New = chkNew.Checked,
                    CarryOver = chkCarryOver.Checked,
                    Cluster = chkCluster.Checked,
                    Random = chkRandom.Checked
                };
            }
        }

        #endregion [ Properties ]

        //-----------------------------------------------------------------------------
    }

    //-----------------------------------------------------------------------------

    public class DefectGubun
    {
        public bool New { get; set; }
        public bool CarryOver { get; set; }
        public bool Cluster { get; set; }
        public bool Random { get; set; }
    }
}
