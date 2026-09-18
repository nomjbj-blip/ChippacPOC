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
    public partial class frmWaferDefine : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region [ Data Field ]
        private string m_sCurrentDrawWaferID = string.Empty;
        private DACrux.Base.TPWafer[] mExcelSelDatas;
        #endregion

        #region [ Create & Close ]
        public frmWaferDefine()
        {
            InitializeComponent();
        }
        #endregion

    }

}
