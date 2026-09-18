using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDMSStepSelect : Form
    {
        DACrux.Base.DPWafer[] wafer = null;

        public frmDMSStepSelect()
        {
            InitializeComponent();
        }

        public frmDMSStepSelect(bool multiSelect)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//
			dpucStepSelect.MultiSelect = multiSelect;
		}

        public DACrux.Base.DPWafer[] Wafer
        {
            get
            {
                return this.wafer;
            }
            set
            {
                this.wafer = value;
            }
        }

        public bool MultiSelect
        {
            get { return dpucStepSelect.MultiSelect; }
            set { dpucStepSelect.MultiSelect = value; }
        }

        public string[] WAFERID_LIST
        {
            get { return dpucStepSelect.WAFERID_LIST; }
        }

        private void dpucStepSelect1_OnSelected(object sender, DACrux.Base.DPWafer[] wafer)
        {
            this.wafer = wafer;
            this.DialogResult = DialogResult.OK;
        }
    }
}
