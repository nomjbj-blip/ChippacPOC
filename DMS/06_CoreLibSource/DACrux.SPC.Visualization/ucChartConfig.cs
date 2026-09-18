using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DACrux.SPC.Visualization
{

    internal partial class ucChartConfig : UserControl
    {
        SPCChartConfig m_oChartConfig = null;

        public void SetChartConfig(SPCChartConfig Cfg)
        {
            m_oChartConfig = Cfg;
            ReflashControl();
        }

        public SPCChartConfig GetChartConfig()
        {
            UpdateValue();
            return m_oChartConfig;
        }

        private void UpdateValue()
        {
            double dYMax = double.NaN;
            double dYMin = double.NaN;
            double dYInterval = double.NaN;
            try
            {
                /// >> Trend
                /////////////////////////////////////////////////////////////////////////
                /// 0. Common
                /// 
                m_oChartConfig.ChartVisible = chkVisible.Checked;

                /// 1. Title
                /// 
                m_oChartConfig.SPCTitle.Name = txtChartTitle.Text;
                m_oChartConfig.SPCTitle.Alignment = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), cmbTitleAlignment.Text);
                m_oChartConfig.SPCTitle.Docking = (System.Windows.Forms.DataVisualization.Charting.Docking)Enum.Parse(typeof(System.Windows.Forms.DataVisualization.Charting.Docking), cmbTitleDocking.Text);

                /// 2. Trend Line
                /// 
                m_oChartConfig.TrendColor = butTrendColor.BackColor;
                m_oChartConfig.TrendWidth = (int)nudTrendWidth.Value;
                m_oChartConfig.TrendStyle = (ChartDashStyle)Enum.Parse(typeof(ChartDashStyle), lbl_DashTypeTrend.Tag.ToString());

                /// 3. Maarker
                /// 
                m_oChartConfig.SPCMarker.MarkerStyle = (System.Windows.Forms.DataVisualization.Charting.MarkerStyle)Enum.Parse(typeof(System.Windows.Forms.DataVisualization.Charting.MarkerStyle), cmbMarkerStyle.Text);

                m_oChartConfig.SPCMarker.BorderColor = butMarkerBorderColor.BackColor;
                m_oChartConfig.SPCMarker.Color = butMarkerColor.BackColor;
                m_oChartConfig.SPCMarker.MarkerBorderWidth = (int)nudMarkerBorderWidth.Value;
                m_oChartConfig.SPCMarker.MarkerSize = (int)nudMarkerSize.Value;


                /// >> Axis
                /////////////////////////////////////////////////////////////////////////
                /// 
                /// 1. Axis X Major
                /// 
                m_oChartConfig.SPCAxisX.MajorGrid.LineColor = but_AxisX_MajorGrid_Color.BackColor;
                m_oChartConfig.SPCAxisX.MajorGrid.Enabled = chk_AxisX_MajorGrid_Visible.Checked;
                m_oChartConfig.SPCAxisX.MajorGrid.Interval = double.Parse(txt_AxisX_MajorGrid_Interval.Text);
                m_oChartConfig.SPCAxisX.MajorGrid.LineDashStyle = (ChartDashStyle)Enum.Parse(typeof(ChartDashStyle), cmb_AxisX_MajorGrid_DashStyle.Text);
                m_oChartConfig.SPCAxisX.MajorGrid.LineWidth = (int)nud_AxisX_MajorGrid_Width.Value;

                /// 2. Axis X Minor
                /// 
                m_oChartConfig.SPCAxisX.MinorGrid.LineColor = but_AxisX_MinorGrid_Color.BackColor;
                m_oChartConfig.SPCAxisX.MinorGrid.Enabled = chk_AxisX_MinorGrid_Visible.Checked;
                m_oChartConfig.SPCAxisX.MinorGrid.Interval = double.Parse(txt_AxisX_MinorGrid_Interval.Text);
                m_oChartConfig.SPCAxisX.MinorGrid.LineDashStyle = (ChartDashStyle)Enum.Parse(typeof(ChartDashStyle), cmb_AxisX_MinorGrid_DashStyle.Text);
                m_oChartConfig.SPCAxisX.MinorGrid.LineWidth = (int)nud_AxisX_MinorGrid_Width.Value;

                /// 
                /// 3. Axis Y Major
                /// 
                m_oChartConfig.SPCAxisY.MajorGrid.LineColor = but_AxisY_MajorGrid_Color.BackColor;
                m_oChartConfig.SPCAxisY.MajorGrid.Enabled = chk_AxisY_MajorGrid_Visible.Checked;
                m_oChartConfig.SPCAxisY.MajorGrid.Interval = double.Parse(txt_AxisY_MajorGrid_Interval.Text);
                m_oChartConfig.SPCAxisY.MajorGrid.LineDashStyle = (ChartDashStyle)Enum.Parse(typeof(ChartDashStyle), cmb_AxisY_MajorGrid_DashStyle.Text);
                m_oChartConfig.SPCAxisY.MajorGrid.LineWidth = (int)nud_AxisY_MajorGrid_Width.Value;

                /// 4. Axis Y Minor
                /// 
                m_oChartConfig.SPCAxisY.MinorGrid.LineColor = but_AxisY_MinorGrid_Color.BackColor;
                m_oChartConfig.SPCAxisY.MinorGrid.Enabled = chk_AxisY_MinorGrid_Visible.Checked;
                m_oChartConfig.SPCAxisY.MinorGrid.Interval = double.Parse(txt_AxisY_MinorGrid_Interval.Text);
                m_oChartConfig.SPCAxisY.MinorGrid.LineDashStyle = (ChartDashStyle)Enum.Parse(typeof(ChartDashStyle), cmb_AxisY_MinorGrid_DashStyle.Text);
                m_oChartConfig.SPCAxisY.MinorGrid.LineWidth = (int)nud_AxisY_MinorGrid_Width.Value;

                /// 5. Axis Y Max/Min Interval
                /// 2015-04-27-정병주 : YAxis Max, Min, Interval 을 사용자가 Update 한다.
                /// 
                if (!double.TryParse(txtMax.Text, out dYMax))
                {
                    txtMax.Focus();
                    throw new Exception("Y Max is Not Number Type");
                }

                m_oChartConfig.SPCAxisY.Maximum = dYMax;

                if (!double.TryParse(txtMin.Text, out dYMin))
                {
                    txtMin.Focus();
                    throw new Exception("Y Min is Not Number Type");
                }

                m_oChartConfig.SPCAxisY.Minimum = dYMin;

                if (!double.TryParse(txtRange.Text, out dYInterval))
                {
                    txtRange.Focus();
                    throw new Exception("Interval is Not Number Type");
                }

                m_oChartConfig.SPCAxisY.Interval = dYInterval;
                m_oChartConfig.ChartAutoInterval = chkAutoRange.Checked;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReflashControl()
        {
            try
            {
                /// >> Trend
                /////////////////////////////////////////////////////////////////////////
                /// 0. Common
                /// 
                chkVisible.Checked = m_oChartConfig.ChartVisible;

                lbl_ChartName.Text = m_oChartConfig.ChartName;
                
                /// 1. Title
                /// 
                txtChartTitle.Text = m_oChartConfig.SPCTitle.Name;
                cmbTitleAlignment.Text = Enum.GetName(typeof(ContentAlignment), m_oChartConfig.SPCTitle.Alignment);
                cmbTitleDocking.Text = Enum.GetName(typeof(System.Windows.Forms.DataVisualization.Charting.Docking), m_oChartConfig.SPCTitle.Docking);

                /// 2. Trend Line
                /// 
                butTrendColor.BackColor = m_oChartConfig.TrendColor;
                nudTrendWidth.Value = m_oChartConfig.TrendWidth;
                lbl_DashTypeTrend.Tag = m_oChartConfig.TrendStyle;
                switch(m_oChartConfig.TrendStyle)
                {
                    case ChartDashStyle.Solid:
                        lbl_DashTypeTrend.Image = imgListDash.Images[0];
                        break;
                    case ChartDashStyle.Dash:
                        lbl_DashTypeTrend.Image = imgListDash.Images[1];
                        break;
                    case ChartDashStyle.Dot:
                        lbl_DashTypeTrend.Image = imgListDash.Images[2];
                        break;
                    case ChartDashStyle.DashDot:
                        lbl_DashTypeTrend.Image = imgListDash.Images[3];
                        break;
                    case ChartDashStyle.DashDotDot:
                        lbl_DashTypeTrend.Image = imgListDash.Images[4];
                        break;
                    case ChartDashStyle.NotSet:
                        lbl_DashTypeTrend.Image = null;
                        break;
                }

                /// 3. Maarker
                /// 
                cmbMarkerStyle.Text = Enum.GetName(typeof(System.Windows.Forms.DataVisualization.Charting.MarkerStyle), m_oChartConfig.SPCMarker.MarkerStyle);
                butMarkerBorderColor.BackColor = m_oChartConfig.SPCMarker.BorderColor;
                butMarkerColor.BackColor = m_oChartConfig.SPCMarker.Color;
                nudMarkerBorderWidth.Value = m_oChartConfig.SPCMarker.MarkerBorderWidth;
                nudMarkerSize.Value = m_oChartConfig.SPCMarker.MarkerSize;


                /// >> Axis
                /////////////////////////////////////////////////////////////////////////
                /// 
                /// 1. Axis X Major
                /// 
                but_AxisX_MajorGrid_Color.BackColor = m_oChartConfig.SPCAxisX.MajorGrid.LineColor;
                chk_AxisX_MajorGrid_Visible.Checked = m_oChartConfig.SPCAxisX.MajorGrid.Enabled;
                txt_AxisX_MajorGrid_Interval.Text = m_oChartConfig.SPCAxisX.MajorGrid.Interval.ToString();
                cmb_AxisX_MajorGrid_DashStyle.Text = Enum.GetName(typeof(ChartDashStyle), m_oChartConfig.SPCAxisX.MajorGrid.LineDashStyle);
                nud_AxisX_MajorGrid_Width.Value = m_oChartConfig.SPCAxisX.MajorGrid.LineWidth;

                /// 2. Axis X Minor
                /// 
                but_AxisX_MinorGrid_Color.BackColor = m_oChartConfig.SPCAxisX.MinorGrid.LineColor;
                chk_AxisX_MinorGrid_Visible.Checked = m_oChartConfig.SPCAxisX.MinorGrid.Enabled;
                txt_AxisX_MinorGrid_Interval.Text = m_oChartConfig.SPCAxisX.MinorGrid.Interval.ToString();
                cmb_AxisX_MinorGrid_DashStyle.Text = Enum.GetName(typeof(ChartDashStyle), m_oChartConfig.SPCAxisX.MinorGrid.LineDashStyle);
                nud_AxisX_MinorGrid_Width.Value = m_oChartConfig.SPCAxisX.MinorGrid.LineWidth;

                /// 
                /// 3. Axis Y Major
                /// 
                but_AxisY_MajorGrid_Color.BackColor = m_oChartConfig.SPCAxisY.MajorGrid.LineColor;
                chk_AxisY_MajorGrid_Visible.Checked = m_oChartConfig.SPCAxisY.MajorGrid.Enabled;
                txt_AxisY_MajorGrid_Interval.Text = m_oChartConfig.SPCAxisY.MajorGrid.Interval.ToString();
                cmb_AxisY_MajorGrid_DashStyle.Text = Enum.GetName(typeof(ChartDashStyle), m_oChartConfig.SPCAxisY.MajorGrid.LineDashStyle);
                nud_AxisY_MajorGrid_Width.Value = m_oChartConfig.SPCAxisY.MajorGrid.LineWidth;

                /// 4. Axis Y Minor
                /// 
                but_AxisY_MinorGrid_Color.BackColor = m_oChartConfig.SPCAxisY.MinorGrid.LineColor;
                chk_AxisY_MinorGrid_Visible.Checked = m_oChartConfig.SPCAxisY.MinorGrid.Enabled;
                txt_AxisY_MinorGrid_Interval.Text = m_oChartConfig.SPCAxisY.MinorGrid.Interval.ToString();
                cmb_AxisY_MinorGrid_DashStyle.Text = Enum.GetName(typeof(ChartDashStyle), m_oChartConfig.SPCAxisY.MinorGrid.LineDashStyle);
                nud_AxisY_MinorGrid_Width.Value = m_oChartConfig.SPCAxisY.MinorGrid.LineWidth;

                /// 5. Axis Y Minor
                /// 2015-04-27-정병주 : YAxis Max, Min, Interval 을 가져온다.
                /// 

                txtMax.Text = m_oChartConfig.SPCAxisY.Maximum.ToString();
                txtMin.Text = m_oChartConfig.SPCAxisY.Minimum.ToString();
                txtRange.Text = m_oChartConfig.SPCAxisY.Interval.ToString();
                chkAutoRange.Checked = m_oChartConfig.ChartAutoInterval;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ucChartConfig()
        {
            InitializeComponent();
            m_oChartConfig = new SPCChartConfig(Color.Blue,"XBAR","XBAR");
        }

        private void cmLineType_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            ContextMenuStrip otsMnu = (ContextMenuStrip)ts.GetCurrentParent();
            Label target = (Label)otsMnu.SourceControl;

            target.Image = ts.BackgroundImage;
            target.Tag = ts.Text;
        }

        private void ucChartConfig_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            cmbMarkerStyle.Items.AddRange(Enum.GetNames(typeof(MarkerStyle)));
            cmbTitleAlignment.Items.AddRange(Enum.GetNames(typeof(System.Drawing.ContentAlignment)));
            cmbTitleDocking.Items.AddRange(Enum.GetNames(typeof(System.Windows.Forms.DataVisualization.Charting.Docking)));
            cmb_AxisX_MajorGrid_DashStyle.Items.AddRange(Enum.GetNames(typeof(ChartDashStyle)));
            cmb_AxisX_MinorGrid_DashStyle.Items.AddRange(Enum.GetNames(typeof(ChartDashStyle)));
            cmb_AxisY_MajorGrid_DashStyle.Items.AddRange(Enum.GetNames(typeof(ChartDashStyle)));
            cmb_AxisY_MinorGrid_DashStyle.Items.AddRange(Enum.GetNames(typeof(ChartDashStyle)));
            ReflashControl();
        }

        private void ColorPickup_Click(object sender, EventArgs e)
        {
            try
            {
                Button butTemp = (Button)sender;
                dlgColorPickup.Color = butTemp.BackColor;
                if (dlgColorPickup.ShowDialog() == DialogResult.OK)
                {
                    butTemp.BackColor = dlgColorPickup.Color;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void chkAutoRange_CheckedChanged(object sender, EventArgs e)
        {
            grpValue.Enabled = !chkAutoRange.Checked;
        }
    }
}
