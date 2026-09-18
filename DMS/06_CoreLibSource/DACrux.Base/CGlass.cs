using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DACrux.Base
{
    /// <summary>
    /// L2R_B2U : 왼쪽에서 오른쪽으로 우선진행하여 아래에서 위로</br>
    /// B2U_L2R : 아래에서 위로 우선진행하여 왼쪽에서 오른쪽으로</br>
    /// L2R_U2B : 왼쪽에서 오른쪽으로 우선진행하여 위에서 아래로</br>
    /// U2B_L2R : 위에서 아래로 우선진행하여 왼쪽에서 오른쪽으로</br>
    /// R2L_B2U : 오른쪽에서 왼쪽으로 우선진행하여 아래에서 위로</br>
    /// B2U_R2L : 아래에서 위로 우선진행하여 오른쪽에서 왼쪽으로</br>
    /// R2L_U2B : 오른쪽에서 왼쪽으로 우선진행하여 위에서 아래로</br>
    /// U2B_R2L : 위에서 아래로 우선진행하여 오른쪽에서 왼쪽으로</br>

    /// </summary>
    public enum IDMakeType { L2R_B2T
                            ,B2T_L2R
                            ,L2R_T2B
                            ,T2B_L2R
                            ,R2L_B2T
                            ,B2T_R2L
                            ,R2L_T2B
                            ,T2B_R2L};
    /// <summary>
    /// TQ_DMS_SETUP_GLASS 
    /// //////////////////////////////////////////////////////////////////////
    /// SETUP_SEQ  :Product / Flow / Oper 명에 따른 Setup Seq. No
    /// SETUP_ID   :Recipe ID or Product Code
    /// STEP_ID    :Flow (Route) Code
    /// SETUP_TIME :Recipe 를 생성 또는 수정한 시간
    /// ANGLE      :Angle 위치 (좌측상단이 0), 시계방향 회전기준으로 증가
    /// QPNL_COUNT :Glass 내의 Cell 의 개수
    /// NOTCH_TYPE :FPD 에서는 사용하지 않음 무조건 '0'
    /// GLASS_X    :GLASS X Size (단위 nm)
    /// GLASS_Y    :GLASS Y Size (단위 nm)
    /// ORIGIN_X   :기준 Point의 X 좌표이며 nm 단위임
    /// ORIGIN_Y   :기준 Point의 Y 좌표이며 nm 단위임
    /// STREET_X   :Q-PANEL간 X 축 간격 값
    /// STREET_Y   :Q-PANEL간 Y 축 간격 값
    /// QPNL_X_CNT :Glass 내의 가로 Cell 수 
    /// QPNL_Y_CNT :Glass 내의 세로 Cell 수 
    /// </summary>
    [Serializable]
    public class CGlass
    {
        #region [ 정보 표시를 위한 프로퍼티 ]

        private string facility = string.Empty;

        [Browsable(false)]
        public string Facility
        {
            get { return facility; }
            set { facility = value; }
        }
        private string product = string.Empty;

        [Browsable(false)]
        public string Product
        {
            get { return product; }
            set { product = value; }
        }
        private string flow = string.Empty;

        [Browsable(false)]
        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }
        private string oper = string.Empty;

        [Browsable(false)]
        public string Oper
        {
            get { return oper; }
            set { oper = value; }
        }
        private string lotID = string.Empty;

        [Browsable(false)]
        public string LotID
        {
            get { return lotID; }
            set { lotID = value; }
        }
        private string unitID = string.Empty;

        [Browsable(false)]
        public string UnitID
        {
            get { return unitID; }
            set { unitID = value; }
        }
        private string inspTime = string.Empty;

        [Browsable(false)]
        public string InspTime
        {
            get { return inspTime; }
            set { inspTime = value; }
        }
        private string judge = string.Empty;

        [Browsable(false)]
        public string Judge
        {
            get { return judge; }
            set { judge = value; }
        }
        private string grade = string.Empty;

        [Browsable(false)]
        public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        private string resId = string.Empty;

        [Browsable(false)]
        public string ResId
        {
            get { return resId; }
            set { resId = value; }
        }

        private string defects = string.Empty;

        [Browsable(false)]
        public string DefectCount
        {
            get { return defects; }
            set { defects = value; }
        }

        #endregion

        private long m_lSETUP_SEQ = -1;
        private string m_strSETUP_ID = string.Empty;
        private string m_strSTEP_ID = string.Empty;
        private DateTime m_oSETUP_TIME = DateTime.Now;
        private int m_iANGLE = 0;
        private string m_strNOTCH_TYPE = "0";
        private double m_dORIGIN_X = 0;
        private double m_dORIGIN_Y = 0;
        private double m_dQPNL_SPOINT_X = 0;
        private double m_dQPNL_SPOINT_Y = 0;
        private double m_dQPNL_PITCH_X = 0;
        private double m_dQPNL_PITCH_Y = 0;
        private double m_dSTREET_X = 0;
        private double m_dSTREET_Y = 0;
        private double m_dGLASS_X = 0;
        private double m_dGLASS_Y = 0;
        private int m_iQPNL_X_CNT = 0;
        private int m_iQPNL_Y_CNT = 0;
        private int m_iPIXEL_X = 0;
        private int m_iPIXEL_Y = 0;
        private List<CQPanel> m_oQPanels = null;
        private List<CModel> m_oModels = null;
        private double m_dNotchSize = 2d;
        private bool m_QPanelAutoGen = false;
        private IDMakeType m_eoQPnlIDMake = IDMakeType.B2T_R2L;
        string[] m_charIdx = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA" }; //,"AA","AB","AC",


        #region PUBLIC Property
        [ReadOnly(true), Category("DB Data"), Description("Represents a unique SetupID(DEVICE,Recipe) Key value.")]
        public long SETUP_SEQ
        {
            get
            {
                return m_lSETUP_SEQ;
            }
            set
            {
                m_lSETUP_SEQ = value;
            }
        }

        [ReadOnly(true), Category("DB Data"), Description("Gets the last updated time.")]
        public DateTime SETUP_TIME
        {
            set
            {
                m_oSETUP_TIME = value;
            }
            get
            {
                return m_oSETUP_TIME;
            }
        }

        [ReadOnly(true), Category("Infomation"), Description("Gets or define a SetupID")]
        public string SETUP_ID
        {
            get
            {
                return m_strSETUP_ID;
            }
            set
            {
                m_strSETUP_ID = value;
            }
        }

        [ReadOnly(true), Category("Infomation"), Description("Gets or Define an oper code which is related to this Recipe.")]
        public string STEP_ID
        {
            get
            {
                return m_strSTEP_ID;
            }
            set
            {
                m_strSTEP_ID = value;
            }
        }

        [Category("MAP Cood"), Description("Gets or define the angle of the MAP.")]
        public int ANGLE
        {
            get
            {
                return m_iANGLE;
            }
            set
            {
                m_iANGLE = value;
            }
        }

        [ReadOnly(true), Category("MAP Cood"), Description("Gets the number of Q-Panel of the map.")]
        public int QPNL_COUNT
        {
            get
            {
                if (m_oQPanels == null) return 0;
                return m_oQPanels.Count;
            }
        }

        [ReadOnly(true), Category("Etc"), Description("Notch Type of the MAP. (FPD Module does not use this field.)")]
        public string NOTCH_TYPE
        {
            get
            {
                return m_strNOTCH_TYPE;
            }
            set
            {
                m_strNOTCH_TYPE = value;
            }
        }

        [Browsable(false), Category("MAP Cood"), Description("Gets or define the origin coordinate(mm) on the MAP and X Offset value from Map Center.")]
        public double ORIGIN_X
        {
            get
            {
                return m_dORIGIN_X;
            }
            set
            {
                m_dORIGIN_X = value;
            }
        }

        [Browsable(false), Category("MAP Cood"), Description("Gets or define the origin coordinate(mm) on the MAP and Y Offset value from Map Center.")]
        public double ORIGIN_Y
        {
            get
            {
                return m_dORIGIN_Y;
            }
            set
            {
                m_dORIGIN_Y = value;
            }
        }

        [Category("QPanel"), Description("Gets or define the width(mm) of Scribe Line. Prior to the Q-Panel Street value and the default is 0.")]
        public double STREET_X
        {
            get
            {
                return m_dSTREET_X;
            }
            set
            {
                m_dSTREET_X = value;
            }
        }

        [Category("QPanel"), Description("Gets or define the height(mm) of Scribe Line. Prior to the Q-Panel Street value and the default is 0.")]
        public double STREET_Y
        {
            get
            {
                return m_dSTREET_Y;
            }
            set
            {
                m_dSTREET_Y = value;
            }
        }

        [Category("MAP Cood"), Description("Gets or sets width(mm) of Glass.")]
        public double GLASS_X
        {
            get
            {
                return m_dGLASS_X;
            }
            set
            {
                m_dGLASS_X = value;
            }
        }

        [Category("MAP Cood"), Description("Gets or sets height(mm) of Glass.")]
        public double GLASS_Y
        {
            get
            {
                return m_dGLASS_Y;
            }
            set
            {
                m_dGLASS_Y = value;
            }
        }

        [Category("QPanel"), Description("Gets or sets the count of Q-Panel on Axis X.")]
        public int QPNL_X_CNT
        {
            get
            {
                return m_iQPNL_X_CNT;
            }
            set
            {
                m_iQPNL_X_CNT = value;
            }
        }

        [Category("QPanel"), Description("Gets or sets the count of Q-Panel on Axis Y.")]
        public int QPNL_Y_CNT
        {
            get
            {
                return m_iQPNL_Y_CNT;
            }
            set
            {
                m_iQPNL_Y_CNT = value;
            }
        }

        [Category("QPanel"), Description("Gets or sets the width of Q-Panel.")]
        public double QPNL_PITCH_X
        {
            get
            {
                return m_dQPNL_PITCH_X;
            }
            set
            {
                m_dQPNL_PITCH_X = value;
            }
        }

        [Category("QPanel"), Description("Gets or sets the height of Q-Panel.")]
        public double QPNL_PITCH_Y
        {
            get
            {
                return m_dQPNL_PITCH_Y;
            }
            set
            {
                m_dQPNL_PITCH_Y = value;
            }
        }

        [Category("QPanel"), Description("Gets or sets the starting position(mm) of the left bottom of Q-Panel.")]
        public double QPNL_SPOINT_X
        {
            get
            {
                return m_dQPNL_SPOINT_X;
            }
            set
            {
                m_dQPNL_SPOINT_X = value;
            }
        }

        [Category("QPanel"), Description("Gets or sets the starting position(mm) of the left bottom of Q-Panel.")]
        public double QPNL_SPOINT_Y
        {
            get
            {
                return m_dQPNL_SPOINT_Y;
            }
            set
            {
                m_dQPNL_SPOINT_Y = value;
            }
        }

        [Category("Etc"), Description("Gets or sets the size of Notch.")]
        public double NOTCH_SIZE
        {
            get
            {
                return m_dNotchSize;
            }
            set
            {
                m_dNotchSize = value;
            }
        }


        [Category("MAP Cood"), Description("Gets or sets the count of Pixel on Axis X.")]
        public int PIXEL_X
        {
            get
            {
                return m_iPIXEL_X;
            }
            set
            {
                m_iPIXEL_X = value;
            }
        }

        [Category("MAP Cood"), Description("Gets or sets the count of Pixel on Axis Y.")]
        public int PIXEL_Y
        {
            get
            {
                return m_iPIXEL_Y;
            }
            set
            {
                m_iPIXEL_Y = value;
            }
        }

        [Category("QPanel"), Description("QPanel ID Make Rule")]
        public IDMakeType QPNL_DRAW_RULE
        {
            get
            {
                return m_eoQPnlIDMake;
            }
            set
            {
                m_eoQPnlIDMake = value;
            }
        }
        #endregion

        public CGlass()
        {
            m_oQPanels = new List<CQPanel>();
            m_oModels = new List<CModel>();
        }

        public CGlass(double Width, double Height,bool AutoQPanel)
        {
            m_oQPanels = new List<CQPanel>();
            m_oModels = new List<CModel>();
            m_dGLASS_X = Width;
            m_dGLASS_Y = Height;

            if (AutoQPanel)
            {
                QPNL_X_CNT = 2;
                QPNL_Y_CNT = 2;
                QPanelAutogen = true;
            }
        }

        public void AutoQPanel(IDMakeType oIDMakeTYpe)
        {
            this.QPNL_DRAW_RULE = oIDMakeTYpe;

            if (m_oQPanels == null)
                m_oQPanels = new List<CQPanel>();
            else
                m_oQPanels.Clear();

            int idx = 0;

            switch (m_eoQPnlIDMake)
            {
                /// L2R_B2T : 왼쪽에서 오른쪽, 아래에서 위 , 좌우 진행 우선
                case IDMakeType.L2R_B2T:
                    for (int y = 0; y < m_iQPNL_Y_CNT; y++)
                    {
                        for (int x = 0; x < m_iQPNL_X_CNT; x++)
                        {
                            m_oQPanels.Add(MakeQPanel(x, y, idx++));
                        }
                    }

                    break;
                /// B2T_L2R : 왼쪽에서 오른쪽, 아래에서 위 , 위아래 진행 우선
                case IDMakeType.B2T_L2R:
                    for (int x = 0; x < m_iQPNL_X_CNT; x++)
                    {
                        for (int y = 0; y < m_iQPNL_Y_CNT; y++)
                        {
                            m_oQPanels.Add(MakeQPanel(x, y, idx++));
                        }
                    }
                    break;

                /// L2R_T2B : 왼쪽에서 오른쪽, 위에서 아래 , 좌우 진행 우선
                case IDMakeType.L2R_T2B:
                    for (int y = m_iQPNL_Y_CNT -1; y >=0 ; y--)
                    {
                        for (int x = 0; x < m_iQPNL_X_CNT; x++)
                        {
                            m_oQPanels.Add(MakeQPanel(x, y, idx++));
                        }
                    }
                    break;
                /// T2B_L2R : 왼쪽에서 오른쪽, 위에서 아래 , 위아래 진행 우선
                case IDMakeType.T2B_L2R:
                    for (int x = 0; x < m_iQPNL_X_CNT; x++)
                    {
                        for (int y = m_iQPNL_Y_CNT - 1; y >= 0; y--)
                        {
                            m_oQPanels.Add(MakeQPanel(x, y, idx++));
                        }
                    }
                    break;


                /// R2L_B2T : 오른쪽에서 왼쪽, 아래에서 위 , 좌우 진행 우선
                case IDMakeType.R2L_B2T:
                    for (int y = 0; y < m_iQPNL_Y_CNT; y++)
                    {
                        for (int x = m_iQPNL_X_CNT - 1; x >= 0; x--)
                        {
                            m_oQPanels.Add(MakeQPanel(x, y, idx++));
                        }
                    }
                    break;
                /// B2T_R2L : 오른쪽에서 왼쪽, 아래에서 위 , 위아래 진행 우선
                case IDMakeType.B2T_R2L:
                    for (int x = m_iQPNL_X_CNT - 1; x >= 0; x--)
                    {
                        for (int y = 0; y < m_iQPNL_Y_CNT; y++)
                        {
                            m_oQPanels.Add(MakeQPanel(x, y, idx++));
                        }
                    }
                    break;


                /// R2L_T2B : 오른쪽에서 왼쪽, 위에서 아래 , 좌우 진행 우선
                case IDMakeType.R2L_T2B:
                    for (int y = m_iQPNL_Y_CNT - 1; y >= 0; y--)
                    {
                        for (int x = m_iQPNL_X_CNT - 1; x >= 0; x--)
                        {
                            m_oQPanels.Add(MakeQPanel(x, y, idx++));
                        }
                    }
                    break;
                /// T2B_R2L : 오른쪽에서 왼쪽, 위에서 아래 , 위아래 진행 우선
                case IDMakeType.T2B_R2L:
                    for (int x = m_iQPNL_X_CNT - 1; x >= 0; x--)
                    {
                        for (int y = m_iQPNL_Y_CNT - 1; y >= 0; y--)
                        {
                            m_oQPanels.Add(MakeQPanel(x, y, idx++));
                        }
                    }
                    break;

                default:
                    for (int x = 0; x < m_iQPNL_X_CNT; x++)
                    {
                        for (int y = 0; y < m_iQPNL_Y_CNT; y++)
                        {
                            m_oQPanels.Add(MakeQPanel(x, y, idx++));
                        }
                    }
                    break;
            }

            AutoPanel();
        }

        private CQPanel MakeQPanel(int x, int y, int idx)
        {

            CQPanel oQPanel = new CQPanel();
            oQPanel.QPanelID = idx;
            oQPanel.QPNL_ALIAS = string.Format("{0}", m_charIdx[idx]);
            oQPanel.QPNL_INDEX_X = x + 1;
            oQPanel.QPNL_INDEX_Y = y + 1;
            oQPanel.QPNL_PITCH_X = m_dQPNL_PITCH_X; // (m_dGLASS_X - (m_dSTREET_X * (m_iQPNL_X_CNT - 1))) / m_iQPNL_X_CNT;
            oQPanel.QPNL_PITCH_Y = m_dQPNL_PITCH_Y; // (m_dGLASS_Y - (m_dSTREET_Y * (m_iQPNL_Y_CNT-1))) / m_iQPNL_Y_CNT;
            oQPanel.QPNL_SPOINT_X = m_dQPNL_SPOINT_X + (m_dORIGIN_X - m_dGLASS_X / 2) + x * oQPanel.QPNL_PITCH_X + m_dSTREET_X * x;
            oQPanel.QPNL_SPOINT_Y = m_dQPNL_SPOINT_Y + (m_dORIGIN_Y - m_dGLASS_Y / 2) + y * oQPanel.QPNL_PITCH_Y + m_dSTREET_Y * y;

            return oQPanel;
        }

        public void AutoPanel()
        {
            int iModelIdx = 0;
            int iPanNum = 0;
            int chr_idx = 0;
            int iNum = 0;

            foreach (CQPanel InQPanel in m_oQPanels)
            {
                InQPanel.Panels.Clear();
                iModelIdx = 0;
                foreach (CModel InModel in m_oModels)
                {
                    switch (InModel.PNL_DRAW_RULE)
                    {
                        /// L2R_B2T : 왼쪽에서 오른쪽, 아래에서 위 , 좌우 진행 우선
                        case IDMakeType.L2R_B2T:
                            for (int y = 0; y < InModel.GRID_Y; y++)
                            {
                                for (int x = 0; x < InModel.GRID_X; x++)
                                {
                                    InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
                                }
                                chr_idx++;
                                iNum = 0;
                            }
                            break;
                        /// B2T_L2R : 왼쪽에서 오른쪽, 아래에서 위 , 위아래 진행 우선
                        case IDMakeType.B2T_L2R:
                            for (int x = 0; x < InModel.GRID_X; x++)
                            {
                                for (int y = 0; y < InModel.GRID_Y; y++)
                                {
                                    InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
                                }
                                chr_idx++;
                                iNum = 0;
                            }
                            break;

                        /// L2R_T2B : 왼쪽에서 오른쪽, 위에서 아래 , 좌우 진행 우선
                        case IDMakeType.L2R_T2B:
                            for (int y = InModel.GRID_Y - 1; y >= 0; y--)
                            {
                                for (int x = 0; x < InModel.GRID_X; x++)
                                {
                                    InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
                                }
                                chr_idx++;
                                iNum = 0;
                            }
                            break;
                        /// T2B_L2R : 왼쪽에서 오른쪽, 위에서 아래 , 위아래 진행 우선
                        case IDMakeType.T2B_L2R:
                            for (int x = 0; x < InModel.GRID_X; x++)
                            {
                                for (int y = InModel.GRID_Y - 1; y >= 0; y--)
                                {
                                    InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
                                }
                                chr_idx++;
                                iNum = 0;
                            }
                            break;


                        /// R2L_B2T : 오른쪽에서 왼쪽, 아래에서 위 , 좌우 진행 우선
                        case IDMakeType.R2L_B2T:
                            for (int y = 0; y < InModel.GRID_Y; y++)
                            {
                                for (int x = InModel.GRID_X - 1; x >= 0; x--)
                                {
                                    InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
                                }
                                chr_idx++;
                                iNum = 0;
                            }
                            break;
                        /// B2T_R2L : 오른쪽에서 왼쪽, 아래에서 위 , 위아래 진행 우선
                        case IDMakeType.B2T_R2L:
                            for (int x = InModel.GRID_X - 1; x >= 0; x--)
                            {
                                for (int y = 0; y < InModel.GRID_Y; y++)
                                {
                                    InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
                                }
                                chr_idx++;
                                iNum = 0;
                            }
                            break;


                        /// R2L_T2B : 오른쪽에서 왼쪽, 위에서 아래 , 좌우 진행 우선
                        case IDMakeType.R2L_T2B:
                            for (int y = InModel.GRID_Y - 1; y >= 0; y--)
                            {
                                for (int x = InModel.GRID_X - 1; x >= 0; x--)
                                {
                                    InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
                                }
                                chr_idx++;
                                iNum = 0;
                            }
                            break;
                        /// T2B_R2L : 오른쪽에서 왼쪽, 위에서 아래 , 위아래 진행 우선
                        case IDMakeType.T2B_R2L:
                            for (int x = InModel.GRID_X - 1; x >= 0; x--)
                            {
                                for (int y = InModel.GRID_Y - 1; y >= 0; y--)
                                {
                                    InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
                                }
                                chr_idx++;
                                iNum = 0;
                            }
                            break;

                        default:
                            for (int x = 0; x < InModel.GRID_X; x++)
                            {
                                for (int y = 0; y < InModel.GRID_Y; y++)
                                {
                                    InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
                                }
                                chr_idx++;
                                iNum = 0;
                            }
                            break;
                    }
                    chr_idx = 0;
                    iNum = 0;
                    iModelIdx++;
                }
            }
        }

        private CPanel MakePanel(int x, int y, int idx, int chr_idx, int iNum, CQPanel InQPanel, CModel InModel)
        {
            CPanel oPanel = new CPanel(idx);
            oPanel.INDEX_X = x + 1;
            oPanel.INDEX_Y = y + 1;
            oPanel.PITCH_X = InModel.PITCH_X;
            oPanel.PITCH_Y = InModel.PITCH_Y;
            oPanel.SPOINT_X = InModel.ORIGIN_X + (InModel.PITCH_X * x) + (InModel.STREET_X * x);
            oPanel.SPOINT_Y = InModel.ORIGIN_Y + (InModel.PITCH_Y * y) + (InModel.STREET_Y * y);
            oPanel.MODEL_ID = InModel.MODEL_ID;

            if (m_oModels.IndexOf(InModel) == 0)
            {
                oPanel.PNL_ALIAS = string.Format("{0}-{1}{2}", InQPanel.QPNL_ALIAS, m_charIdx[chr_idx], iNum + 1);
            }
            else
            {
                oPanel.PNL_ALIAS = string.Format("{0}-{1}{2}-{3}", InQPanel.QPNL_ALIAS, m_charIdx[chr_idx], iNum + 1, InModel.MODEL_ID);
            }

            oPanel.QPNL_ALIAS = InQPanel.QPNL_ALIAS;
            oPanel.PanelProperty = 0;
            oPanel.BIN = 1;

            return oPanel;
        }

        // '09.8.28 GLASS SETUP 작업 중 추가(by 양형석)
        public void ChangeModelID(string oldID, string newID)
        {
            foreach(CQPanel qpnl in m_oQPanels)
            {
                foreach(CPanel pnl in qpnl.Panels)
                {
                    if (pnl.MODEL_ID == oldID)
                        pnl.MODEL_ID = newID;
                }
            }
        }

        public void SetSetupSeq(long seq)
        {
            m_lSETUP_SEQ = seq;
        }

        public void SetSetupTime(DateTime datetime)
        {
            m_oSETUP_TIME = datetime;
        }

        [Category("MAP Cood"), Description("Gets the collection of Q-Panel.")]
        [Browsable(false)]
        public List<CQPanel> QPanels
        {
            get
            {
                return m_oQPanels;
            }
        }

        [Category("MMG Setup"), Description("Gets the collection of Model.")]
        [Browsable(false)]
        public List<CModel> Models
        {
            get
            {
                return m_oModels;
            }
        }

        [Category("QPanel"), Description("Gets or sets whether create Q-Panels in the Glass automatically.")]
        public bool QPanelAutogen
        {
            set
            {
                if (value)
                {
                    AutoQPanel(m_eoQPnlIDMake);
                }
                else
                {
                    QPanels.Clear();
                }
                m_QPanelAutoGen = value;
            }
            get
            {
                return m_QPanelAutoGen;
            }
        }

        public CPanel GetPanel(int iPanelNum)
        {
            if (iPanelNum < 0)
                return null;

            CPanel panel = null;

            for (int i = 0; i < m_oQPanels.Count; i++)
            {
                panel = m_oQPanels[i].Panels.Find(delegate(CPanel pnl) { return pnl.PANEL_NUM.Equals(iPanelNum); });

                if (panel != null)
                    return panel;
            }

            return null;
        }

        public CPanel GetPanel(string strGlasslID)
        {
            if (strGlasslID.Trim() == string.Empty)
                return null;

            CPanel panel = null;

            for (int i = 0; i < m_oQPanels.Count; i++)
            {
                panel = m_oQPanels[i].Panels.Find(delegate(CPanel pnl) { return pnl.PNL_ALIAS.Equals(strGlasslID); });

                if (panel != null)
                    return panel;
            }

            return null;
        }
    }

    /// <summary>
    /// TQ_DMS_SETUP_QPNL 
    /// //////////////////////////////////////////////////////////////////////
    /// SETUP_SEQ     : Setup Seq. No (TQ_DMS_SETUP_QPNL 의 SETUP_SEQ)
    /// QPNL_ALIAS    : Glass 내에서 Cell(Q-Panel) 식별자
    /// QPNL_INDEX_X  : Glass에서 Cell(Q-Panel)의 X Index
    /// QPNL_INDEX_Y  : Glass에서 Cell(Q-Panel)의 Y Index
    /// QPNL_SPOINT_X : Glass에서 Cell(Q-Panel)의 X Start Position 위치
    /// QPNL_SPOINT_Y : Glass에서 Cell(Q-Panel)의 Y Start Position 위치
    /// QPNL_PITCH_X  : Cell(Q-Panel)의 X 사이즈이며 nm 단위임
    /// QPNL_PITCH_Y  : Cell(Q-Panel)의 Y 사이즈이며 nm 단위임
    /// QPNL_STREET_X : Cell(Q-Panel)간 Street Size X
    /// QPNL_STREET_Y : Cell(Q-Panel)간 Street Size Y
    /// </summary>
    [Serializable]
    public class CQPanel
    {
        private long m_iSETUP_SEQ;
        private int m_iQPNLID = -1;
        private string m_strQPNL_ALIAS;
        private int m_iQPNL_INDEX_X;
        private int m_iQPNL_INDEX_Y;
        private double m_dQPNL_SPOINT_X;
        private double m_dQPNL_SPOINT_Y;
        private double m_dQPNL_PITCH_X;
        private double m_dQPNL_PITCH_Y;
        private double m_dQPNL_STREET_X;
        private double m_dQPNL_STREET_Y;
        private List<CPanel> m_oCPanel = null;
        private int m_iBIN;

        #region PUBLIC Property

        [Category("DB Data"), Description("Represents a unique SetupID(DEVICE,Recipe) Key value.")]
        public long SETUP_SEQ
        {
            set
            {
                m_iSETUP_SEQ = value;
            }
            get
            {
                return m_iSETUP_SEQ;
            }
        }

        [Category("DB Data"), Description("Gets or sets Q-Panel ID.")]
        public string QPNL_ALIAS
        {
            set
            {
                m_strQPNL_ALIAS = value;
            }
            get
            {
                return m_strQPNL_ALIAS;
            }
        }

        [Category("MAP Cood"), Description("Gets or sets the ordinal number of a Q-Panel on the Axis X.")]
        public int QPNL_INDEX_X
        {
            set
            {
                m_iQPNL_INDEX_X = value;
            }
            get
            {
                return m_iQPNL_INDEX_X;
            }
        }

        [Category("Map Cood"), Description("Gets or sets the ordinal number of a Q-Panel on the Axis Y.")]
        public int QPNL_INDEX_Y
        {
            set
            {
                m_iQPNL_INDEX_Y = value;
            }
            get
            {
                return m_iQPNL_INDEX_Y;
            }
        }

        [Category("Map Cood"), Description("Gets or sets the starting position(mm) of the left bottom of Q-Panel on the Axis X.")]
        public double QPNL_SPOINT_X
        {
            set
            {
                m_dQPNL_SPOINT_X = value;
            }
            get
            {
                return m_dQPNL_SPOINT_X;
            }
        }

        [Category("Map Cood"), Description("Gets or sets the starting position(mm) of the left bottom of Q-Panel on the Axis Y.")]
        public double QPNL_SPOINT_Y
        {
            set
            {
                m_dQPNL_SPOINT_Y = value;
            }
            get
            {
                return m_dQPNL_SPOINT_Y;
            }
        }

        [Category("Map Cood"), Description("Gets or sets the width(mm) of a Q-Panel.")]
        public double QPNL_PITCH_X
        {
            set
            {
                m_dQPNL_PITCH_X = value;
            }
            get
            {
                return m_dQPNL_PITCH_X;
            }
        }

        [Category("Map Cood"), Description("Gets or sets the height(mm) of a Q-Panel.")]
        public double QPNL_PITCH_Y
        {
            set
            {
                m_dQPNL_PITCH_Y = value;
            }
            get
            {
                return m_dQPNL_PITCH_Y;
            }
        }

        [Category("Map Cood"), Description("Gets or sets the width(mm) of the scribe line between Q-Panels.")]
        public double QPNL_STREET_X
        {
            set
            {
                m_dQPNL_STREET_X = value;
            }
            get
            {
                return m_dQPNL_STREET_X;
            }
        }

        [Category("Map Cood"), Description("Gets or sets the height(mm) of the scribe line between Q-Panels.")]
        public double QPNL_STREET_Y
        {
            set
            {
                m_dQPNL_STREET_Y = value;
            }
            get
            {
                return m_dQPNL_STREET_Y;
            }
        }
        #endregion

        public CQPanel()
        {
            m_oCPanel = new List<CPanel>();
        }

        public CQPanel(double dX, double dY, double Width, double Height)
        {
            m_oCPanel = new List<CPanel>();
            QPNL_PITCH_X = Width;
            QPNL_PITCH_Y = Height;
            QPNL_SPOINT_X = dX;
            QPNL_SPOINT_Y = dY;
        }

        [Category("MAP Cood"), Description("QPanel Integer Serial ID")]
        public int QPanelID
        {
            set
            {
                m_iQPNLID = value;
            }
            get
            {
                return m_iQPNLID;
            }
        }


        [Category("MAP Cood"), Description("Gets the panel collection.")]
        public List<CPanel> Panels
        {
            get
            {
                return m_oCPanel;
            }
        }


        public int BIN
        {
            set
            {
                m_iBIN = value;
            }
            get
            {
                return m_iBIN;
            }
        }

    }

    [Serializable]
    public class CPanel
    {
        private int m_iSETUP_SEQ;
        private int m_iPANEL_NUM;
        private int m_iQPNLID;
        private string m_strQPNL_ALIAS;
        private string m_strMODEL_ID;
        private string m_strPNL_ALIAS;
        private int m_iINDEX_X;
        private int m_iINDEX_Y;
        private double m_dSPOINT_X;
        private double m_dSPOINT_Y;
        private double m_dPITCH_X;
        private double m_dPITCH_Y;

        private string m_strJudge;

        private string m_strGrade;

        
        /// <summary>
        /// 판정값에 대한 속성
        /// </summary>
        /// //////////////////////////////////////////////////
        private int m_iBIN;
        private int m_iPassFail;
        private int m_iZoneNumber;
        private double m_dParaValue;
        private string m_strPanelID;
        private int m_intPanelProperty;

        #region PUBLIC Property
        public int SETUP_SEQ
        {
            set
            {
                m_iSETUP_SEQ = value;
            }
            get
            {
                return m_iSETUP_SEQ;
            }
        }

        public int QPanelID
        {
            set
            {
                m_iQPNLID = value;
            }
            get
            {
                return m_iQPNLID;
            }
        }

        [Category("DB Data"), Description("Represents the unique ID of a panel in a Glass.")]
        public int PANEL_NUM
        {
            set
            {
                m_iPANEL_NUM = value;
            }
            get
            {
                return m_iPANEL_NUM;
            }
        }

        public string QPNL_ALIAS
        {
            set
            {
                m_strQPNL_ALIAS = value;
            }
            get
            {
                return m_strQPNL_ALIAS;
            }
        }
        public string MODEL_ID
        {
            set
            {
                m_strMODEL_ID = value;
            }
            get
            {
                return m_strMODEL_ID;
            }
        }
        public string PNL_ALIAS
        {
            set
            {
                m_strPNL_ALIAS = value;
            }
            get
            {
                return m_strPNL_ALIAS;
            }
        }
        public int INDEX_X
        {
            set
            {
                m_iINDEX_X = value;
            }
            get
            {
                return m_iINDEX_X;
            }
        }
        public int INDEX_Y
        {
            set
            {
                m_iINDEX_Y = value;
            }
            get
            {
                return m_iINDEX_Y;
            }
        }
        public double SPOINT_X
        {
            set
            {
                m_dSPOINT_X = value;
            }
            get
            {
                return m_dSPOINT_X;
            }
        }
        public double SPOINT_Y
        {
            set
            {
                m_dSPOINT_Y = value;
            }
            get
            {
                return m_dSPOINT_Y;
            }
        }
        public double PITCH_X
        {
            set
            {
                m_dPITCH_X = value;
            }
            get
            {
                return m_dPITCH_X;
            }
        }
        public double PITCH_Y
        {
            set
            {
                m_dPITCH_Y = value;
            }
            get
            {
                return m_dPITCH_Y;
            }
        }

        #endregion

        #region 판정값에 대한 PUBLIC Property
        public int BIN
        {
            set
            {
                m_iBIN = value;
            }
            get
            {
                return m_iBIN;
            }
        }

        public int PassFail
        {
            set
            {
                m_iPassFail = value;
            }
            get
            {
                return m_iPassFail;
            }
        }

        public int ZoneNumber
        {
            set
            {
                m_iZoneNumber = value;
            }
            get
            {
                return m_iZoneNumber;
            }
        }

        public double ParaValue
        {
            set
            {
                m_dParaValue = value;
            }
            get
            {
                return m_dParaValue;
            }
        }

        public string PanelID
        {
            set
            {
                m_strPanelID = value;
            }
            get
            {
                return m_strPanelID;
            }
        }

        public int PanelProperty
        {
            set
            {
                m_intPanelProperty = value;
            }
            get
            {
                return m_intPanelProperty;
            }
        }

        public string Judge
        {
            get { return m_strJudge; }
            set { m_strJudge = value; }
        }

        public string Grade
        {
            get { return m_strGrade; }
            set { m_strGrade = value; }
        }

     
        #endregion

        public CPanel()
        {

        }

        public CPanel(int PanelNumber)
        {
            m_iPANEL_NUM = PanelNumber;
        }

        public void SetPanelNumber(int PanelNumber)
        {
            m_iPANEL_NUM = PanelNumber;
        }

    }

    [Serializable]
    public class CModel
    {
        private long m_lSETUP_SEQ;
        private string m_strMODEL_ID = string.Empty;
        private double m_dPITCH_X;
        private double m_dPITCH_Y;
        private double m_dORIGIN_X;
        private double m_dORIGIN_Y;
        private int m_iGRID_X;
        private int m_iGRID_Y;
        private double m_dSTREET_X;
        private double m_dSTREET_Y;
        private IDMakeType m_eoIDMakeRule = IDMakeType.R2L_B2T;

        #region PUBLIC Property
        [Category("DB Data"), Description("Represents a unique SetupID(DEVICE,Recipe) Key value.")]
        [Browsable(false)]
        public long SETUP_SEQ
        {
            set
            {
                m_lSETUP_SEQ = value;
            }
            get
            {
                return m_lSETUP_SEQ;
            }
        }

        [Category("Information"), Description("Gets or sets the Model ID.")]
        public string MODEL_ID
        {
            set
            {
                m_strMODEL_ID = value;
            }
            get
            {
                return m_strMODEL_ID;
            }
        }

        [Category("Information"), Description("Gets or sets the width(mm) of a Panel.")]
        public double PITCH_X
        {
            set
            {
                m_dPITCH_X = value;
            }
            get
            {
                return m_dPITCH_X;
            }
        }

        [Category("Information"), Description("Gets or sets the width(mm) of a Panel.")]
        public double PITCH_Y
        {
            set
            {
                m_dPITCH_Y = value;
            }
            get
            {
                return m_dPITCH_Y;
            }
        }

        [Category("Information"), Description("Gets or sets the starting position(mm) of the left bottom of Panel on the Axis X.")]
        public double ORIGIN_X
        {
            set
            {
                m_dORIGIN_X = value;
            }
            get
            {
                return m_dORIGIN_X;
            }
        }

        [Category("Information"), Description("Gets or sets the starting position(mm) of the left bottom of Q-Panel on the Axis Y.")]
        public double ORIGIN_Y
        {
            set
            {
                m_dORIGIN_Y = value;
            }
            get
            {
                return m_dORIGIN_Y;
            }
        }

        [Category("Information"), Description("Gets or sets the Grid X of a Model.")]
        public int GRID_X
        {
            set
            {
                m_iGRID_X = value;
            }
            get
            {
                return m_iGRID_X;
            }
        }

        [Category("Information"), Description("Gets or sets the Grid Y of a Model.")]
        public int GRID_Y
        {
            set
            {
                m_iGRID_Y = value;
            }
            get
            {
                return m_iGRID_Y;
            }
        }

        [Category("Information"), Description("Gets or sets the width(mm) of the scribe line between Panels.")]
        public double STREET_X
        {
            set
            {
                m_dSTREET_X = value;
            }
            get
            {
                return m_dSTREET_X;
            }
        }

        [Category("Information"), Description("Gets or sets the height(mm) of the scribe line between Panels.")]
        public double STREET_Y
        {
            set
            {
                m_dSTREET_Y = value;
            }
            get
            {
                return m_dSTREET_Y;
            }
        }

        [Category("Information"), Description("Panel Alias Setting Rule")]
        public IDMakeType PNL_DRAW_RULE
        {
            set
            {
                m_eoIDMakeRule = value;
            }
            get
            {
                return m_eoIDMakeRule;
            }
        }

        #endregion

        public CModel()
        {
        }


    }
}
