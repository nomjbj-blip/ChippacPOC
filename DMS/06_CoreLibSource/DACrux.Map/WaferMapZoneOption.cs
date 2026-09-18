using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base.Zone;

namespace DACrux.Map
{
    /// <summary>
    /// Zone Option Popup 2019.11.01 Taihi,Kim.
    /// </summary>
    public partial class WaferMapZoneOption : Form
    {
        #region 멤버 변수

        private bool _isBinding;

        #endregion

        #region 생성자 및 Load 이벤트

        public WaferMapZoneOption()
        {
            InitializeComponent();
        }

        private void WaferMapZoneOption_Load(object sender, EventArgs e)
        {
            cboOrientation.Items.AddRange(Enum.GetNames(typeof(ZoneOrientation)));

            map.ZoneConfig.Orientation = ZoneConfig.Orientation;
            map.ZoneConfig.AnnularSizeMode = ZoneConfig.AnnularSizeMode;
            map.ZoneConfig.AnnularZoneCount = ZoneConfig.AnnularZoneCount;
            map.ZoneConfig.RadialZoneCount = ZoneConfig.RadialZoneCount;
            map.ZoneConfig.Angle = ZoneConfig.Angle;
            map.ZoneConfig.WaferEdgeSize = ZoneConfig.WaferEdgeSize;

            _isBinding = true;

            try
            {
                cboOrientation.Text = ZoneConfig.Orientation.ToString();
                rdoSameArea.Checked = ZoneConfig.AnnularSizeMode == ZoneAnnularSizeMode.SameArea;
                rdoSameRadius.Checked = ZoneConfig.AnnularSizeMode == ZoneAnnularSizeMode.SameRadius;
                numAnnularZone.Value = ZoneConfig.AnnularZoneCount;
                numRadialZone.Value = ZoneConfig.RadialZoneCount;
                numAngle.Value = (decimal)ZoneConfig.Angle;
                numExcludesWaferEdge.Value = (decimal)ZoneConfig.WaferEdgeSize;
            }
            finally
            {
                _isBinding = false;
            }

            chkDisplayZoneID.Checked = true;
            map.WaferDrawMode = MapMode.Fit;
        }

        #endregion

        #region 사용자 정의 메서드

        private ZoneOrientation GetZoneOrientation()
        {
            ZoneOrientation val;

            if (!Enum.TryParse<ZoneOrientation>(cboOrientation.Text, out val))
                val = ZoneOrientation.Down;

            return val;
        }

        private void SetValue()
        {
            map.ZoneConfig.Orientation = GetZoneOrientation();
            map.ZoneConfig.AnnularSizeMode = rdoSameArea.Checked ? ZoneAnnularSizeMode.SameArea : ZoneAnnularSizeMode.SameRadius;
            map.ZoneConfig.AnnularZoneCount = (int)numAnnularZone.Value;
            map.ZoneConfig.RadialZoneCount = (int)numRadialZone.Value;
            map.ZoneConfig.Angle = (float)numAngle.Value;
            map.ZoneConfig.WaferEdgeSize = (float)numExcludesWaferEdge.Value;
            map.ZoneConfig.CalculateZoneItem();
            map.WaferDrawMode = MapMode.Fit;
        }

        #endregion

        #region 이벤트 처리 메서드

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isBinding)
            {
                SetValue();
                map.WaferDrawMode = MapMode.Fit;
            }
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!_isBinding)
            {
                SetValue();
                map.WaferDrawMode = MapMode.Fit;
            }
        }

        private void cboOrientation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isBinding)
            {
                SetValue();
                map.WaferDrawMode = MapMode.Fit;
            }
        }

        private void chkDisplayZoneID_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isBinding)
            {
                map.VisibleZoneID = chkDisplayZoneID.Checked;
                map.WaferDrawMode = MapMode.Fit;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ZoneConfig == null)
                throw new Exception("ZoneConfig is null");

            if (String.IsNullOrEmpty(cboOrientation.Text))
            {
                MessageBox.Show("Please select Orientation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ZoneConfig.CheckValid();
            ZoneConfig.Save();

            ZoneConfig.Orientation = map.ZoneConfig.Orientation;
            ZoneConfig.AnnularSizeMode = map.ZoneConfig.AnnularSizeMode;
            ZoneConfig.AnnularZoneCount = map.ZoneConfig.AnnularZoneCount;
            ZoneConfig.RadialZoneCount = map.ZoneConfig.RadialZoneCount;
            ZoneConfig.Angle = map.ZoneConfig.Angle;
            ZoneConfig.WaferEdgeSize = map.ZoneConfig.WaferEdgeSize;
            ZoneConfig.CalculateZoneItem();

            DialogResult = DialogResult.OK;
        }

        #endregion

        #region 프로퍼티

        public DACrux.Base.Zone.ZoneConfig ZoneConfig
        {
            get;
            set;
        }

        #endregion
    }
}