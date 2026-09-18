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
    public partial class frmCommonalityWaferNo : Form
    {
        public delegate void WaferNoSelected(object sender, string[] strWaferNo);
        public event WaferNoSelected OnWaferNoSelected = null;

        public frmCommonalityWaferNo()
        {
            InitializeComponent();
        }

        private void frmCommonalityWaferNo_Load(object sender, EventArgs e)
        {

        }

        private void chkWaferNoAll_CheckedChanged(object sender, EventArgs e)
        {
            chkWaferNo01.Checked = chkWaferNoAll.Checked;
            chkWaferNo02.Checked = chkWaferNoAll.Checked;
            chkWaferNo03.Checked = chkWaferNoAll.Checked;
            chkWaferNo04.Checked = chkWaferNoAll.Checked;
            chkWaferNo05.Checked = chkWaferNoAll.Checked;
            chkWaferNo06.Checked = chkWaferNoAll.Checked;
            chkWaferNo07.Checked = chkWaferNoAll.Checked;
            chkWaferNo08.Checked = chkWaferNoAll.Checked;
            chkWaferNo09.Checked = chkWaferNoAll.Checked;
            chkWaferNo10.Checked = chkWaferNoAll.Checked;
            chkWaferNo11.Checked = chkWaferNoAll.Checked;
            chkWaferNo12.Checked = chkWaferNoAll.Checked;
            chkWaferNo13.Checked = chkWaferNoAll.Checked;
            chkWaferNo14.Checked = chkWaferNoAll.Checked;
            chkWaferNo15.Checked = chkWaferNoAll.Checked;
            chkWaferNo16.Checked = chkWaferNoAll.Checked;
            chkWaferNo17.Checked = chkWaferNoAll.Checked;
            chkWaferNo18.Checked = chkWaferNoAll.Checked;
            chkWaferNo19.Checked = chkWaferNoAll.Checked;
            chkWaferNo20.Checked = chkWaferNoAll.Checked;
            chkWaferNo21.Checked = chkWaferNoAll.Checked;
            chkWaferNo22.Checked = chkWaferNoAll.Checked;
            chkWaferNo23.Checked = chkWaferNoAll.Checked;
            chkWaferNo24.Checked = chkWaferNoAll.Checked;
            chkWaferNo25.Checked = chkWaferNoAll.Checked;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            List<string> lsWafer = new List<string>();

            if (chkWaferNo01.Checked)
                lsWafer.Add("1");

            if (chkWaferNo02.Checked)
                lsWafer.Add("2");

            if (chkWaferNo03.Checked)
                lsWafer.Add("3");

            if (chkWaferNo04.Checked)
                lsWafer.Add("4");

            if (chkWaferNo05.Checked)
                lsWafer.Add("5");

            if (chkWaferNo06.Checked)
                lsWafer.Add("6");

            if (chkWaferNo07.Checked)
                lsWafer.Add("7");

            if (chkWaferNo08.Checked)
                lsWafer.Add("8");

            if (chkWaferNo09.Checked)
                lsWafer.Add("9");

            if (chkWaferNo10.Checked)
                lsWafer.Add("10");

            if (chkWaferNo11.Checked)
                lsWafer.Add("11");

            if (chkWaferNo12.Checked)
                lsWafer.Add("12");

            if (chkWaferNo13.Checked)
                lsWafer.Add("13");

            if (chkWaferNo14.Checked)
                lsWafer.Add("14");

            if (chkWaferNo15.Checked)
                lsWafer.Add("15");

            if (chkWaferNo16.Checked)
                lsWafer.Add("16");

            if (chkWaferNo17.Checked)
                lsWafer.Add("17");

            if (chkWaferNo18.Checked)
                lsWafer.Add("18");

            if (chkWaferNo19.Checked)
                lsWafer.Add("19");

            if (chkWaferNo20.Checked)
                lsWafer.Add("20");

            if (chkWaferNo21.Checked)
                lsWafer.Add("21");

            if (chkWaferNo22.Checked)
                lsWafer.Add("22");

            if (chkWaferNo23.Checked)
                lsWafer.Add("23");

            if (chkWaferNo24.Checked)
                lsWafer.Add("24");

            if (chkWaferNo25.Checked)
                lsWafer.Add("25");

            if (lsWafer.Count <= 0)
            {
                MessageBox.Show("선택된 Wafer No 가 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (OnWaferNoSelected != null)
                OnWaferNoSelected(this, lsWafer.ToArray());

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
    }
}
