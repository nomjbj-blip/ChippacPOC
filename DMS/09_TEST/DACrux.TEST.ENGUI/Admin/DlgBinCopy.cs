using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.TEST.RO;

namespace DACrux.TEST.ENGUI
{
    public partial class DlgBinCopy : Form
    {
        #region [ Constructor ]
        public DlgBinCopy()
        {
            InitializeComponent();
        }

        public DlgBinCopy(
            string testarea,
            string sourceProgram
            )
            : this()
        {
            TestArea = testarea;
            SourceProgram = sourceProgram;
        }
        #endregion [ Constructor ]

        #region [ Event Handler ]
        private void DlgBinCopy_Load(
            object sender,
            EventArgs e
            )
        {
        }
        #endregion [ Event Handler ]

        #region [ Method ]
        #endregion [ Method ]

        #region [ Property ]
        public string TestArea
        {
            get;
            private set;
        }

        public string SourceProgram
        {
            get { return txtSourceProgram.Text; }
            private set { txtSourceProgram.Text = value; }
        }
        public string TargetProgram
        {
            get { return txtTargetProgram.Text; }
            private set { txtTargetProgram.Text = value; }
        }
        #endregion [ Property ]
    }
}
