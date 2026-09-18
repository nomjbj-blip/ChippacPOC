/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : SPCChartConfig.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2015.01.01
--  Description     : DACrux SPC Chart Configuration
--  History         : Created by YSIM at 2015.01.01
 * ********************************************************************************************************
 * 2015 년 DACrux V5 History Init
----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace DACrux.SPC.Visualization
{
    public class SPCChartConfig
    {
        bool m_ChartVisible = false;
        bool m_AutoInterval = true;
        System.Drawing.Color m_TrendColor = System.Drawing.Color.Red;
        int m_TrendWidth = 1;
        ChartDashStyle m_TrendStyle = ChartDashStyle.Solid;

        public Title SPCTitle = null;
        public DataPointCustomProperties SPCMarker = null;
        public Axis SPCAxisX = null;
        public Axis SPCAxisY = null;
        public string ChartName = string.Empty;
        
        /// <summary>
        /// Line의 Color를 가져오거나 설정합니다.
        /// </summary>
        public System.Drawing.Color TrendColor
        {
            set
            {
                m_TrendColor = value;
            }
            get
            {
                return m_TrendColor;
            }
        }

        /// <summary>
        /// Line의 Dash Style 속성을 가져오거나 설정합니다.
        /// </summary>
        public ChartDashStyle TrendStyle
        {
            set
            {
                m_TrendStyle = value;
            }
            get
            {
                return m_TrendStyle;
            }
        }


        /// <summary>
        /// Line Drawing시 사용될 Pen 두께를 가져오거나 설정합니다.
        /// </summary>
        public int TrendWidth
        {
            set
            {
                m_TrendWidth = value;
            }
            get
            {
                return m_TrendWidth;
            }
        }

        /// <summary>
        /// Line 사용여부를 가져오거나 설정합니다.
        /// </summary>
        public bool ChartVisible
        {
            set
            {
                m_ChartVisible = value;
            }
            get
            {
                return m_ChartVisible;
            }
        }

        public bool ChartAutoInterval
        {
            set
            {
                m_AutoInterval = value;
            }
            get
            {
                return m_AutoInterval;
            }
        }

        //public SPCChartConfig()
        //{
        //    SPCTitle = new Title();
        //    SPCMarker = new DataPointCustomProperties();
        //    SPCAxisX = new Axis();
        //    SPCAxisY = new Axis();
        //}

        public SPCChartConfig(System.Drawing.Color ChartColor,string ChartTitle,string ChartArea, bool Enable = true, bool ZoneEnable = false, int LineWidth = 1)
        {

            /// 1. SPC Title
            ///===========================================
            SPCTitle = new Title(ChartTitle);
            ///Default Value
            ///
            SPCTitle.Name = ChartTitle;
            SPCTitle.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            SPCTitle.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            SPCTitle.DockingOffset = 2;
            SPCTitle.IsDockedInsideChartArea = false;
            SPCTitle.DockedToChartArea = ChartArea;
            ChartName = ChartArea;  // 고정불변 {XBAR,RANGE,SIGMA,RAW,EWMA_MV,EWMA_S,EWMA_R,MA,MS}

            /// 2. SPC Marker
            ///===========================================
            SPCMarker = new DataPointCustomProperties();
            ///Default Value
            SPCMarker.MarkerStyle = MarkerStyle.Circle;
            SPCMarker.MarkerBorderColor = Color.DarkGray;
            SPCMarker.MarkerBorderWidth = 2;
            SPCMarker.MarkerSize = 7;
            SPCMarker.MarkerColor = Color.DarkGray;

            /// 3. SPC Axis X
            ///===========================================
            SPCAxisX = new Axis();
            ///Default Value
            SPCAxisX.MajorGrid.Enabled = true;
            SPCAxisX.MajorGrid.Interval = 10;
            SPCAxisX.MajorGrid.LineColor = Color.Gray;
            SPCAxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;

            SPCAxisX.MinorGrid.Enabled = false;
            SPCAxisX.MinorGrid.Interval = 1;
            SPCAxisX.MinorGrid.LineColor = Color.LightGray;
            SPCAxisX.MinorGrid.LineDashStyle = ChartDashStyle.Solid;
                    
            ///4. SPC Axis Y
            ///===========================================
            SPCAxisY = new Axis(null,AxisName.Y);
            ///Default Value
            SPCAxisY.LabelStyle.Format = "{#.#00}";
            SPCAxisY.MajorGrid.Enabled = false;
            SPCAxisY.MajorGrid.Interval = 10;
            SPCAxisY.MajorGrid.LineColor = Color.Gray;
            SPCAxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;

            SPCAxisY.MinorGrid.Enabled = false;
            SPCAxisY.MinorGrid.Interval = 1;
            SPCAxisY.MinorGrid.LineColor = Color.LightGray;
            SPCAxisY.MinorGrid.LineDashStyle = ChartDashStyle.Solid;
            SPCAxisY.IsStartedFromZero = false;

            
            m_TrendColor = ChartColor;
            m_TrendWidth = LineWidth;
            m_ChartVisible = Enable;

            m_TrendStyle = ChartDashStyle.Solid;
        }
    }
}
