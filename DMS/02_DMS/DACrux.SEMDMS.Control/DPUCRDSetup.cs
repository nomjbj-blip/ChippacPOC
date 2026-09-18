using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DACrux.Map;
using DACrux.Base;
using DACrux.Framework.Controls;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCRDSetup : UserControl
    {
        public static readonly int DefaultTolerance = 50;
        public static readonly int DefaultRepeatCount = 5;

        public static readonly Color NonRDColor = Color.Green;
        public static readonly Color RDColor = Color.Red;

        public static readonly string DrawDefectsALL = "ALL";

        private static ShotInfo GlobalShotInfo = new ShotInfo();
        private bool _isBinding;
        
        private System.Windows.Forms.Control _targetControl;

        public event EventHandler ApplyButtonClick;

        public DPUCRDSetup()
        {
            InitializeComponent();
        }

        private void DPUCShotSetup_Load(object sender, EventArgs e)
        {
            SetVisibleImageMark();

            BindingMapValue();

            SettingData settingData = new SettingData(GetType().Name);
            rdoPoint.Checked = settingData.GetValue<bool>("Mode_Point", true);
            rdoSize.Checked = settingData.GetValue<bool>("Mode_Size", false);
            txtRDTolerance.Text = settingData.GetValue<int>("Tolerance", DefaultTolerance).ToString();
            numRDCount.Value = settingData.GetValue<int>("RepeatCount", DefaultRepeatCount);

            try
            {
                 _isBinding = true;
                numShotArrayX.Value = GlobalShotInfo.ArrayX;
                numShotArrayY.Value = GlobalShotInfo.ArrayY;
                numShotStartX.Value = GlobalShotInfo.StartX;
                numShotStartY.Value = GlobalShotInfo.StartY;
            }
            finally
            {
                _isBinding = false;
            }
        }

        protected virtual void OnApplyButtonClick(EventArgs e)
        {
            if (ApplyButtonClick != null)
                ApplyButtonClick(this, e);
        }

        public void SetChartData(int nonRd, int rd)
        {
            chart.Series[0].Points.Clear();
            chart.Series[0].Points.AddXY("NonRD", nonRd);
            chart.Series[0].Points.AddXY("RD", rd);
            chart.Series[0].Points[0].Color = NonRDColor;
            chart.Series[0].Points[1].Color = RDColor;
        }

        private void SetVisibleImageMark()
        {
            if (TargetControl is IDefectMap)
            {
                chkImageMarker.Checked = (TargetControl as IDefectMap).VisibleImageMark;
            }
            else if (TargetControl is DUCControlSpread)
            {
                DUCControlSpreadItemList list = (TargetControl as DUCControlSpread).ControlList;

                foreach (var item in list)
                {
                    if (item.Control is IDefectMap)
                        chkImageMarker.Checked = (item.Control as IDefectMap).VisibleImageMark;
                }
            }
        }

        private void SetDrawDefects(string drawDefects)
        {
            if (TargetControl is IDefectMap)
            {
                (TargetControl as IDefectMap).DrawDefects = drawDefects;
            }
            else if (TargetControl is DUCControlSpread)
            {
                DUCControlSpreadItemList list = (TargetControl as DUCControlSpread).ControlList;

                foreach (var item in list)
                {
                    if (item.Control is IDefectMap)
                        (item.Control as IDefectMap).DrawDefects = drawDefects;
                }
            }
        }

        private void MapRedraw()
        {
            if (TargetControl is IDefectMap)
            {
                (TargetControl as IDefectMap).Redraw();
            }
            else if (TargetControl is DUCControlSpread)
            {
                DUCControlSpreadItemList list = (TargetControl as DUCControlSpread).ControlList;

                foreach (var item in list)
                {
                    if (item.Control is IDefectMap)
                        (item.Control as IDefectMap).Redraw();
                }

                (TargetControl as DUCControlSpread).RedrawAll();
            }
        }

        private void ClearSelectedDefect()
        {
            if (TargetControl is IDefectMap)
            {
                (TargetControl as IDefectMap).ClearSelectedDefect();
            }
            else if (TargetControl is DUCControlSpread)
            {
                DUCControlSpreadItemList list = (TargetControl as DUCControlSpread).ControlList;

                foreach (var item in list)
                {
                    if (item.Control is IDefectMap)
                        (item.Control as IDefectMap).ClearSelectedDefect();
                }
            }
        }

        public void ClearChartData()
        {
            SetChartData(0, 0);
        }

        private void BindingMapValue()
        {
            if (TargetControl is IShot)
            {
                IShot map = TargetControl as IShot;

                _isBinding = true;
                try
                {
                    if (!chk1X1.Checked)
                    {
                        numShotArrayX.Value = map.ShotArrayX;
                        numShotArrayY.Value = map.ShotArrayY;
                    }
                    else
                    {
                        _prevArray.X = map.ShotArrayX;
                        _prevArray.Y = map.ShotArrayY;
                    }

                    numShotStartX.Value = map.ShotStartX;
                    numShotStartY.Value = map.ShotStartY;
                }
                finally
                {
                    _isBinding = false;
                }
            }
        }

        private void SetValueToMap()
        {
            if (TargetControl is IShot)
            {
                IShot map = TargetControl as IShot;

                map.ShotArrayX = (int)numShotArrayX.Value;
                map.ShotArrayY = (int)numShotArrayY.Value;
                map.ShotStartX = (int)numShotStartX.Value;
                map.ShotStartY = (int)numShotStartY.Value;
                map.Redraw();

                SetGlobalShotInfo();
            }
        }

        private void SetGlobalShotInfo()
        {
            if (!chk1X1.Checked)
            {
                GlobalShotInfo.ArrayX = (int)numShotArrayX.Value;
                GlobalShotInfo.ArrayY = (int)numShotArrayY.Value;
            }
            else
            {
                GlobalShotInfo.ArrayX = _prevArray.X;
                GlobalShotInfo.ArrayY = _prevArray.Y;
            }

            GlobalShotInfo.StartX = (int)numShotStartX.Value;
            GlobalShotInfo.StartY = (int)numShotStartY.Value;
        }

        private void chkImageMarker_CheckedChanged(object sender, System.EventArgs e)
        {
            if (TargetControl is IDefectMap)
            {
                (TargetControl as IDefectMap).VisibleImageMark = chkImageMarker.Checked;
                (TargetControl as IDefectMap).Redraw();
            }
            else if (TargetControl is DUCControlSpread)
            {
                DUCControlSpreadItemList list = (TargetControl as DUCControlSpread).ControlList;

                foreach (var item in list)
                {
                    if (item.Control is IDefectMap)
                    {
                        (item.Control as IDefectMap).VisibleImageMark = chkImageMarker.Checked;
                        (item.Control as IDefectMap).Redraw();
                    }
                }
            }
        }

        private void ChartDefect_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                SetDrawDefects(DrawDefectsALL);

                HitTestResult result = chart.HitTest(e.X, e.Y);
                string SeriesPoint = string.Empty;

                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    string label = chart.Series[0].Points[result.PointIndex].AxisLabel.Trim();

                    if (!String.IsNullOrEmpty(label))
                        SetDrawDefects(label);
                }
                else if (result.ChartElementType == ChartElementType.AxisLabels && result.Axis.AxisName == AxisName.X && result.Object is CustomLabel)
                {
                    string label = (result.Object as CustomLabel).Text.Trim();

                    if (!String.IsNullOrEmpty(label))
                        SetDrawDefects(label);
                }

                MapRedraw();
            }
            finally
            {
                chart.Cursor = Cursors.Hand;
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!_isBinding)
                SetValueToMap();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            numShotStartY.Value++;
            SetValueToMap();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            numShotStartY.Value--;
            SetValueToMap();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            numShotStartX.Value--;
            SetValueToMap();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            numShotStartX.Value++;
            SetValueToMap();
        }

        Point _prevArray;

        private void chk1X1_CheckedChanged(object sender, EventArgs e)
        {
            if (chk1X1.Checked)
            {
                _prevArray.X = (int)numShotArrayX.Value;
                _prevArray.Y = (int)numShotArrayY.Value;
                numShotArrayX.Value = numShotArrayY.Value = 1;
            }
            else
            {
                numShotArrayX.Value = _prevArray.X;
                numShotArrayY.Value = _prevArray.Y;
            }

            SetValueToMap();
            numShotArrayX.Enabled = numShotArrayY.Enabled = grpStart.Enabled = !chk1X1.Checked;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            double tolerance;

            if (!Double.TryParse(txtRDTolerance.Text, out tolerance))
            {
                MessageBox.Show("Tolerance 값에 문제가 있습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetDrawDefects(DrawDefectsALL);
            ClearSelectedDefect();
            OnApplyButtonClick(EventArgs.Empty);

            SettingData settingData = new SettingData(GetType().Name);
            settingData.SetValue("Mode_Point", rdoPoint.Checked);
            settingData.SetValue("Mode_Size", rdoSize.Checked);
            settingData.SetValue("Tolerance", tolerance);
            settingData.SetValue("RepeatCount", numRDCount.Value);

            settingData.Save();
        }

        public void ApplyClick()
        {
            btnApply.PerformClick();
        }

        [DefaultValue(null)]
        public System.Windows.Forms.Control TargetControl
        {
            get
            {
                return _targetControl;
            }
            set
            {
                if (_targetControl == value)
                    return;

                if (_targetControl != null && _targetControl is IShot)
                    (_targetControl as IShot).ShotOptionChanged -= TargetControl_ShotOptionChanged;

                _targetControl = value;

                if (_targetControl != null && _targetControl is IShot)
                    (_targetControl as IShot).ShotOptionChanged += TargetControl_ShotOptionChanged;
            }
        }

        private void TargetControl_ShotOptionChanged(object sender, EventArgs e)
        {
            BindingMapValue();
            SetGlobalShotInfo();
        }

        [Browsable(false)]
        public double Tolerance
        {
            get { return DACrux.Base.Convert.doubleParse(txtRDTolerance.Text); }
        }

        [Browsable(false)]
        public int RepeatCount
        {
            get { return (int)numRDCount.Value; }
        }

        [Browsable(false)]
        public int ShotStartX
        {
            get { return (int)numShotStartX.Value; }
            set { numShotStartX.Value = value; }
        }

        [Browsable(false)]
        public int ShotStartY
        {
            get { return (int)numShotStartY.Value; }
            set { numShotStartY.Value = value; }
        }

        [Browsable(false)]
        public int ShotArrayX
        {
            get 
            { 
                return (int)numShotArrayX.Value;
            }
            set 
            {
                if (!chk1X1.Checked)
                    numShotArrayX.Value = value;
                else
                    _prevArray.X = value;
            }
        }

        [Browsable(false)]
        public int ShotArrayY
        {
            get 
            { 
                return (int)numShotArrayY.Value;
            }
            set
            {
                if (!chk1X1.Checked)
                    numShotArrayY.Value = value;
                else
                    _prevArray.Y = value;
            }
        }

        [Browsable(false)]
        public bool VisibleImageMarker
        {
            get { return chkImageMarker.Checked; }
        }

        [Browsable(false)]
        public bool IsPointMode
        {
            get { return rdoPoint.Checked; }
        }

        class ShotInfo
        {
            public ShotInfo()
            {
                ArrayX = ArrayY = StartX = StartY = 1;
            }

            public int ArrayX;
            public int ArrayY;
            public int StartX;
            public int StartY;
        }

        [DefaultValue(false)]
        public bool UseOneByOne
        {
            get { return chk1X1.Checked; }
            set { chk1X1.Checked = value; }
        }
    }
}
