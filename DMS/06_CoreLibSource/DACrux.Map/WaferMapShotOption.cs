using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Map
{
    public partial class WaferMapShotOption : Form
    {
        public WaferMapShotOption()
        {
            InitializeComponent();
        }

        private int _oldArrayX, _oldArrayY, _oldStartX, _oldStartY;

        private void WaferMapShotOption_Load(object sender, EventArgs e)
        {
            if (Map == null)
                throw new Exception("Map 이 설정되지 않았습니다.");

            _oldArrayX = Map.ShotArrayX;
            _oldArrayY = Map.ShotArrayY;
            _oldStartX = Map.ShotStartX;
            _oldStartY = Map.ShotStartY;

            numShotArrayX.Value = _oldArrayX;
            numShotArrayY.Value = _oldArrayY;
            numShotStartX.Value = _oldStartX;
            numShotStartY.Value = _oldStartY;
        }

        private void SetValue()
        {
            Map.ShotArrayX = (int)numShotArrayX.Value;
            Map.ShotArrayY = (int)numShotArrayY.Value;
            Map.ShotStartX = (int)numShotStartX.Value;
            Map.ShotStartY = (int)numShotStartY.Value;
            Map.Redraw();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            numShotStartY.Value++;
            SetValue();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            numShotStartY.Value--;
            SetValue();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            numShotStartX.Value--;
            SetValue();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            numShotStartX.Value++;
            SetValue();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SetValue();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Map.ShotArrayX = _oldArrayX;
            Map.ShotArrayY = _oldArrayY;
            Map.ShotStartX = _oldStartX;
            Map.ShotStartY = _oldStartY;
            Map.Redraw();

            DialogResult = DialogResult.Cancel;
        }

        public DACrux.Map.IShot Map
        {
            get;
            set;
        }

        private void numericControl_ValueChanged(object sender, EventArgs e)
        {
            SetValue();
        }
    }
}
