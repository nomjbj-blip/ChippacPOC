using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.SPC.Visualization
{
    [Serializable]
    public class LineConfig
    {
        System.Drawing.Color m_LineColor = System.Drawing.Color.Red;
        System.Drawing.SolidBrush m_LineBrush = null;
        System.Drawing.Pen m_LinePen = null;
        bool m_ZoneEnable = false;
        bool m_LineEnable = false;
        int m_LineWidth = 1;

        /// <summary>
        /// Line의 Color를 가져오거나 설정합니다.
        /// </summary>
        public System.Drawing.Color LineColor
        {
            set
            {
                m_LineColor = value;
            }
            get
            {
                return m_LineColor;
            }
        }

        /// <summary>
        /// Line을 채울 속성을 가져오거나 설정합니다.
        /// </summary>
        public System.Drawing.SolidBrush LineBrush
        {
            set
            {
                m_LineBrush = value;
            }
            get
            {
                return m_LineBrush;
            }
        }

        /// <summary>
        /// Line Drawing시 사용될 Pen 속성을 가져오거나 설정합니다.
        /// </summary>
        public System.Drawing.Pen LinePen
        {
            set
            {
                m_LinePen = value;
            }
            get
            {
                return m_LinePen;
            }
        }

        /// <summary>
        /// Line Drawing시 사용될 Pen 두께를 가져오거나 설정합니다.
        /// </summary>
        public int Width
        {
            set
            {
                m_LineWidth = value;
            }
            get
            {
                return m_LineWidth;
            }
        }

        /// <summary>
        /// Line 사용여부를 가져오거나 설정합니다.
        /// </summary>
        public bool Enable
        {
            set
            {
                m_LineEnable = value;
            }
            get
            {
                return m_LineEnable;
            }
        }

        public bool ZoneEnable
        {
            set
            {
                m_ZoneEnable = value;
            }
            get
            {
                return m_ZoneEnable;
            }
        }

        public LineConfig()
        {
            m_LineBrush = new System.Drawing.SolidBrush(m_LineColor);
            m_LinePen = new System.Drawing.Pen(m_LineBrush);
        }

        public LineConfig(System.Drawing.Color PenColor,bool Enable = true, bool ZoneEnable = false , int LineWidth = 1)
        {
            m_LineColor = PenColor;
            m_LineWidth = LineWidth;
            m_LineEnable = Enable;
            m_ZoneEnable = ZoneEnable;

            m_LineBrush = new System.Drawing.SolidBrush(m_LineColor);
            m_LinePen = new System.Drawing.Pen(m_LineBrush);
            m_LinePen.Width = m_LineWidth;

        }

    }
}
