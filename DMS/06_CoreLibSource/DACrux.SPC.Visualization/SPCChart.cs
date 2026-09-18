/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : SPCChart.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.29
--  Description     : DACrux/SPC Chart 
--  History         : Created by YSIM at 2014.11.29
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2014.11.29 : YSIM
 *             - MS Chart를 이용하여 SPC를 Drawing 할 수 있도록 함
 *             - VS 외에 별도 License가 없으나 강력하게 사용 가능
----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace DACrux.SPC.Visualization
{
    public delegate void SelectedPoint(object sender, int[] idx, long[] DataSeq);
    public delegate void ChangePointVisible(object sender, int idx, long DataSeq,bool Visible);
    public delegate void SelectData(object sender);

    [Designer(typeof(SPCChartDesigner))]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(SPCChart),"SPCChart.bmp")]
    public partial class SPCChart : UserControl
    {

        public event SelectData OnSelectData = null;

        const string XBAR = "XBAR";
        const string SIGMA = "SIGMA";
        const string RANGE = "RANGE";
        const string RAW = "RAW";
        const string EWMA_MV = "EWMA_MV";
        const string EWMA_R = "EWMA_R";
        const string EWMA_S = "EWMA_S";
        const string MA = "MA";
        const string MS = "MS";
        const string I = "I";
        const string MR = "MR";


        int m_iChartMinHeight = 300;
        bool m_FitAllChart = false;
        bool m_HiddenPointVisible = true;
        bool m_ResultVisible = true;

        bool m_bXAxisStartZero = false;
        bool m_bXAxisMargin = true;

        /// <summary>
        /// DATA
        /// </summary>
        DataTable m_dt = null;

        List<bool> m_VAL_VISIBLE = null;
        List<long> m_VAL_DSEQ = null;
        List<long> m_ALL_DSEQ_IDX = null;
        List<string> m_VAL_RESULT = null;

        List<double> m_VAL_XBAR = null;
        List<double> m_VAL_SIGMA = null;
        List<double> m_VAL_RANGE = null;
        List<DataPoint> m_VAL_RAW = null;
        List<double> m_VAL_EWMA_MV = null;
        List<double> m_VAL_EWMA_S = null;
        List<double> m_VAL_EWMA_R = null;
        List<double> m_VAL_MA = null;
        List<double> m_VAL_MS = null;
        List<double> m_VAL_I = null;
        List<double> m_VAL_MR = null;

        List<DataPoint>[] m_CMB_VAL_XBAR = null;
        List<DataPoint>[] m_CMB_VAL_SIGMA = null;
        List<DataPoint>[] m_CMB_VAL_RANGE = null;
        List<DataPoint>[] m_CMB_VAL_RAW = null;
        List<DataPoint>[] m_CMB_VAL_EWMA_MV = null;
        List<DataPoint>[] m_CMB_VAL_EWMA_S = null;
        List<DataPoint>[] m_CMB_VAL_EWMA_R = null;
        List<DataPoint>[] m_CMB_VAL_MA = null;
        List<DataPoint>[] m_CMB_VAL_MS = null;
        List<string> m_VAL_RESULT_IMR = null;

        Color[] m_COL_Combination = null;

        string m_STR_LABEL_MEMBER = "TRAN_TIME";
        string m_STR_LABEL_FORMAT = "MM-dd HH:mm";

        int m_iSampleCount = 0;

        int m_iOOC = 0;
        int m_iOOS = 0;

        double m_dSUM = 0;
        double m_dAVG = double.NaN;
        double m_dSTD = double.NaN;
        double m_dVAR = double.NaN;
        int m_iCNT = 0;

        double m_dRANGE_SUM = 0;
        double m_dRANGE_AVG = double.NaN;
        double m_dRANGE_STD = double.NaN;
        double m_dRANGE_VAR = double.NaN;

        double m_dSIGMA_SUM = 0;
        double m_dSIGMA_AVG = double.NaN;
        double m_dSIGMA_STD = double.NaN;
        double m_dSIGMA_VAR = double.NaN;

        double m_dRAW_SUM = 0;
        double m_dRAW_AVG = double.NaN;
        double m_dRAW_STD = double.NaN;
        double m_dRAW_VAR = double.NaN;
        int m_iRAW_CNT = 0;

        /// <summary>
        /// Cursor
        /// </summary>
        int m_csLineWidth = 1;
        System.Windows.Forms.DataVisualization.Charting.ChartDashStyle m_csDashStyle = ChartDashStyle.Solid;
        System.Drawing.Color m_csLineColor = Color.Green;
        System.Drawing.Color m_csSelectionColor = Color.Yellow;
        bool m_csUserEnabled = true;

        /// <summary>
        /// Spec Line
        /// </summary>
        List<double> m_USL = null;
        List<double> m_LSL = null;
        double m_MaxUSL = double.NaN;
        double m_MinLSL = double.NaN;
        LineConfig m_oSPEC_CFG = new LineConfig(Color.Red);
        LineConfig m_oXBAR_CFG = new LineConfig(Color.Gray);

        /// <summary>
        /// XBAR Control Line
        /// </summary>
        /// 
        SPCChartConfig m_XBAR_CHART = new SPCChartConfig(Color.Black, XBAR, XBAR);
        List<double> m_XBAR_UCL = null;
        List<double> m_XBAR_LCL = null;
        double m_MaxUCL = double.NaN;
        double m_MinLCL = double.NaN;

        List<double> m_XBAR_3S_UCL = null;
        List<double> m_XBAR_3S_LCL = null;
        LineConfig m_oXBAR_CTRL_CFG = new LineConfig(Color.Pink);
        LineConfig m_oXBAR_3SIGMA_CFG = new LineConfig(Color.Blue);


        /// <summary>
        /// SIGMA Control Line
        /// </summary>
        SPCChartConfig m_SIGMA_CHART = new SPCChartConfig(Color.Red, SIGMA, SIGMA);
        List<double> m_SIGMA_UCL = null;
        List<double> m_SIGMA_LCL = null;
        double m_Max_SIGMA_UCL = double.NaN;
        double m_Min_SIGMA_LCL = double.NaN;
        LineConfig m_oSIGMA_CFG = new LineConfig(Color.Pink);

        /// <summary>
        /// RANGE Control Line
        /// </summary>
        SPCChartConfig m_RANGE_CHART = new SPCChartConfig(Color.Brown, RANGE, RANGE);
        List<double> m_RANGE_UCL = null;
        List<double> m_RANGE_LCL = null;
        double m_Max_RANGE_UCL = double.NaN;
        double m_Min_RANGE_LCL = double.NaN;
        LineConfig m_oRANGE_CFG = new LineConfig(Color.Pink);

        /// <summary>
        /// RANGE Control Line
        /// </summary>
        /// XBAR와 동일한 Control Limit 값을 사용
        LineConfig m_oRAW_CFG = new LineConfig(Color.Pink);
        SPCChartConfig m_RAW_CHART = new SPCChartConfig(Color.Gray, RAW, RAW);

        /// <summary>
        /// EWMA_MV Control Line
        /// </summary>
        SPCChartConfig m_EWMA_MV_CHART = new SPCChartConfig(Color.Black, EWMA_MV, EWMA_MV);
        List<double> m_EWMA_MV_UCL = null;
        List<double> m_EWMA_MV_LCL = null;
        LineConfig m_oEWMA_MV_CFG = new LineConfig(Color.Pink);

        /// <summary>
        /// EWMA_R Control Line
        /// </summary>
        SPCChartConfig m_EWMA_R_CHART = new SPCChartConfig(Color.Black, EWMA_R, EWMA_R);
        List<double> m_EWMA_R_UCL = null;
        List<double> m_EWMA_R_LCL = null;
        LineConfig m_oEWMA_R_CFG = new LineConfig(Color.Pink);

        /// <summary>
        /// EWMA_S Control Line
        /// </summary>
        SPCChartConfig m_EWMA_S_CHART = new SPCChartConfig(Color.Black, EWMA_S, EWMA_S);
        List<double> m_EWMA_S_UCL = null;
        List<double> m_EWMA_S_LCL = null;
        LineConfig m_oEWMA_S_CFG = new LineConfig(Color.Pink);

        /// <summary>
        /// MA Control Line
        /// </summary>
        SPCChartConfig m_MA_CHART = new SPCChartConfig(Color.Black, MA, MA);
        List<double> m_MA_UCL = null;
        List<double> m_MA_LCL = null;
        LineConfig m_oMA_CFG = new LineConfig(Color.Pink);

        /// <summary>
        /// MS Control Line
        /// </summary>
        SPCChartConfig m_MS_CHART = new SPCChartConfig(Color.Black, MS, MS);
        List<double> m_MS_UCL = null;
        List<double> m_MS_LCL = null;
        LineConfig m_oMS_CFG = new LineConfig(Color.Pink);


        /// <summary>
        /// IMR Chart
        /// </summary>
        SPCChartConfig m_I_CHART = new SPCChartConfig(Color.Black, MS, MS);
        List<double> m_IMR_USL = null;
        List<double> m_IMR_LSL = null;
        List<double> m_IMR_UCL = null;
        List<double> m_IMR_LCL = null;
        LineConfig m_oISPEC_CFG = new LineConfig(Color.Red);
        LineConfig m_oICONTROL_CFG = new LineConfig(Color.Pink);

        SPCChartConfig m_MR_CHART = new SPCChartConfig(Color.Black, MS, MS);
        LineConfig m_oMR_CFG = new LineConfig(Color.Pink);


        public event SelectedPoint OnSelectedPoint = null;
        public event ChangePointVisible OnChangePointVisible = null;

        #region SPCChart Config
        public Color[] CombinationColor
        {
            get
            {
                return m_COL_Combination;
            }
            set
            {
                m_COL_Combination = value;
            }
        }

        public SPCChartConfig XBAR_CHART_CFG
        {
            get
            {
                return m_XBAR_CHART;
            }
        }

        public SPCChartConfig SIGMA_CHART_CFG
        {
            get
            {
                return m_SIGMA_CHART;
            }
        }

        public SPCChartConfig RANGE_CHART_CFG
        {
            get
            {
                return m_RANGE_CHART;
            }
        }

        public SPCChartConfig RAW_CHART_CFG
        {
            get
            {
                return m_RAW_CHART;
            }
        }

        public SPCChartConfig EWMA_MV_CHART_CFG
        {
            get
            {
                return m_EWMA_MV_CHART;
            }
        }

        public SPCChartConfig EWMA_R_CHART_CFG
        {
            get
            {
                return m_EWMA_R_CHART;
            }
        }

        public SPCChartConfig EWMA_S_CHART_CFG
        {
            get
            {
                return m_EWMA_S_CHART;
            }
        }


        public SPCChartConfig MA_CHART_CFG
        {
            get
            {
                return m_MA_CHART;
            }
        }

        public SPCChartConfig MS_CHART_CFG
        {
            get
            {
                return m_MS_CHART;
            }
        }

        public SPCChartConfig I_CHART_CFG
        {
            get
            {
                return m_I_CHART;
            }
        }

        public SPCChartConfig MR_CHART_CFG
        {
            get
            {
                return m_MR_CHART;
            }
        }

        #endregion

        public SPCChart()
        {
            InitializeComponent();

            m_XBAR_CHART = new SPCChartConfig(Color.Green, XBAR, XBAR);
            m_SIGMA_CHART = new SPCChartConfig(Color.Gray, SIGMA, SIGMA);
            m_RANGE_CHART = new SPCChartConfig(Color.DarkCyan, RANGE, RANGE);
            m_RAW_CHART = new SPCChartConfig(Color.DarkGray, RAW, RAW);
            m_EWMA_MV_CHART = new SPCChartConfig(Color.DarkKhaki, EWMA_MV, EWMA_MV);
            m_EWMA_R_CHART = new SPCChartConfig(Color.DarkKhaki, EWMA_R, EWMA_R);
            m_EWMA_S_CHART = new SPCChartConfig(Color.DarkKhaki, EWMA_S, EWMA_S);
            m_MA_CHART = new SPCChartConfig(Color.DarkKhaki, MA, MA);
            m_MS_CHART = new SPCChartConfig(Color.DarkKhaki, MS, MS);
            m_I_CHART = new SPCChartConfig(Color.Green, XBAR, XBAR);
            m_MR_CHART = new SPCChartConfig(Color.Green, XBAR, XBAR);

            m_oSPEC_CFG.LinePen.DashStyle = DashStyle.Dot;
            m_oSPEC_CFG.LinePen.DashPattern = new float[] { 5, 5 };

            m_oISPEC_CFG.LinePen.DashStyle = DashStyle.Dot;
            m_oISPEC_CFG.LinePen.DashPattern = new float[] { 5, 5 };
            //Init_Sample();
        }

        #region ★ 속성
        public DataTable DataSource
        {
            set
            {
                m_dt = value;
                DataCal();
            }
            get
            {
                return m_dt;
            }
        }

        public string LabelMember
        {
            set
            {
                m_STR_LABEL_MEMBER = value;
            }
            get
            {
                return m_STR_LABEL_MEMBER;
            }
        }

        public string LabelFormat
        {
            set
            {
                m_STR_LABEL_FORMAT = value;
            }
            get
            {
                return m_STR_LABEL_FORMAT;
            }
        }

        public int ChartMinHeight
        {
            set
            {
                m_iChartMinHeight = value;
            }
            get
            {
                return m_iChartMinHeight;
            }
        }

        public bool FitAllChart
        {
            set
            {
                m_FitAllChart = value;
            }
            get
            {
                return m_FitAllChart;
            }
        }

        public bool DisplayHiddenPoint
        {
            set
            {
                m_HiddenPointVisible = value;
                // 값이 바뀌면 다시 그려야 함
                DataCal();
                DrawSPC();
            }
            get
            {
                return m_HiddenPointVisible;
            }
        }

        public List<double> XBAR_VALUES
        {
            get
            {
                return m_VAL_XBAR;
            }
        }

        public List<double> RANGE_VALUES
        {
            get
            {
                return m_VAL_RANGE;
            }
        }

        public List<DataPoint> RAW_VALUES
        {
            get
            {
                return m_VAL_RAW;
            }
        }

        public List<long> DSEQ_VALUES
        {
            get
            {
                return m_VAL_DSEQ;
            }
        }

        public List<double> XBAR_I
        {
            get
            {
                return m_VAL_I;
            }
        }

        public List<double> XBAR_MR
        {
            get
            {
                return m_VAL_MR;
            }
        }
        #endregion

        #region Line Define

        public LineConfig SPECLine
        {
            get
            {
                return m_oSPEC_CFG;
            }
        }

        public LineConfig XBARLine
        {
            get
            {
                return m_oXBAR_CTRL_CFG;
            }
        }

        public LineConfig XBAR_3SIGMALine
        {
            get
            {
                return m_oXBAR_3SIGMA_CFG;
            }
        }

        public LineConfig SIGMALine
        {
            get
            {
                return m_oSIGMA_CFG;
            }
        }

        public LineConfig RANGELine
        {
            get
            {
                return m_oRANGE_CFG;
            }
        }

        public LineConfig EWMA_MVLine
        {
            get
            {
                return m_oEWMA_MV_CFG;
            }
        }

        public LineConfig EWMA_RLine
        {
            get
            {
                return m_oEWMA_R_CFG;
            }
        }

        public LineConfig EWMA_SLine
        {
            get
            {
                return m_oEWMA_S_CFG;
            }
        }

        public LineConfig MALine
        {
            get
            {
                return m_oMA_CFG;
            }
        }

        public LineConfig MSLine
        {
            get
            {
                return m_oMS_CFG;
            }
        }
        #endregion

        #region STAT VALUE
        public int OOC
        {
            get
            {
                return m_iOOC;
            }
        }

        public int OOS
        {
            get
            {
                return m_iOOS;
            }
        }

        public double RAW_STD
        {
            get
            {
                return m_dRAW_STD;
            }
        }

        public double RAW_AVG
        {
            get
            {
                return m_dRAW_AVG;
            }
        }

        public double RAW_VAR
        {
            get
            {
                return m_dRAW_VAR;
            }
        }

        public double RAW_SUM
        {
            get
            {
                return m_dRAW_SUM;
            }
        }

        public double STD
        {
            get
            {
                return m_dSTD;
            }
        }

        public double AVG
        {
            get
            {
                return m_dAVG;
            }
        }

        public double VAR
        {
            get
            {
                return m_dVAR;
            }
        }

        public double SUM
        {
            get
            {
                return m_dSUM;
            }
        }
        #endregion

        #region Chart Init & Sample Data
        private void Init_Sample()
        {
            try
            {
                if (m_dt == null)
                {
                    m_dt = new DataTable();
                    m_dt.Columns.Add("SPEC_SEQ", typeof(Int32));
                    m_dt.Columns.Add("DATA_SEQ", typeof(Int32));
                    m_dt.Columns.Add("SPEC_ID", typeof(string));
                    m_dt.Columns.Add("PARA_ID", typeof(string));
                    m_dt.Columns.Add("TRAN_TIME", typeof(DateTime));
                    m_dt.Columns.Add("FACTORY", typeof(string));
                    m_dt.Columns.Add("PROCESS", typeof(string));
                    m_dt.Columns.Add("STEP_ID", typeof(string));
                    m_dt.Columns.Add("AREA_ID", typeof(string));
                    m_dt.Columns.Add("RES_TYPE", typeof(string));
                    m_dt.Columns.Add("RES_ID", typeof(string));
                    m_dt.Columns.Add("CUSTOMER_ID", typeof(string));
                    m_dt.Columns.Add("PACKAGE", typeof(string));
                    m_dt.Columns.Add("MAT_ID", typeof(string));
                    m_dt.Columns.Add("LOT_ID", typeof(string));

                    m_dt.Columns.Add("USL", typeof(double));
                    m_dt.Columns.Add("TARGET", typeof(double));
                    m_dt.Columns.Add("LSL", typeof(double));

                    // XBAR
                    m_dt.Columns.Add("MEAN_UCL", typeof(double));
                    m_dt.Columns.Add("MEAN_CL", typeof(double));
                    m_dt.Columns.Add("MEAN_LCL", typeof(double));
                    m_dt.Columns.Add("EXT_MV", typeof(double));


                    //SIGMA
                    m_dt.Columns.Add("SIGMA_UCL", typeof(double));
                    m_dt.Columns.Add("SIGMA_CL", typeof(double));
                    m_dt.Columns.Add("SIGMA_LCL", typeof(double));
                    m_dt.Columns.Add("EXT_SIGMA", typeof(double));

                    //RANGE
                    m_dt.Columns.Add("RANGE_UCL", typeof(double));
                    m_dt.Columns.Add("RANGE_CL", typeof(double));
                    m_dt.Columns.Add("RANGE_LCL", typeof(double));
                    m_dt.Columns.Add("EXT_RANGE", typeof(double));

                    //RAW DATA
                    m_dt.Columns.Add("VALUE_1", typeof(double));
                    m_dt.Columns.Add("VALUE_2", typeof(double));
                    m_dt.Columns.Add("VALUE_3", typeof(double));
                    m_dt.Columns.Add("VALUE_4", typeof(double));
                    m_dt.Columns.Add("VALUE_5", typeof(double));
                    m_dt.Columns.Add("VALUE_6", typeof(double));
                    m_dt.Columns.Add("VALUE_7", typeof(double));
                    m_dt.Columns.Add("VALUE_8", typeof(double));
                    m_dt.Columns.Add("VALUE_9", typeof(double));
                    m_dt.Columns.Add("VALUE_10", typeof(double));

                    #region SAMPLE DATA
m_dt.Rows.Add(new object[] {"3243","195245","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 00:36:44","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT001","ACC","","EQUIPMENT","GGG259.1F1",700,500,300,600.82,455.414,310,451,0,0,0,39.64214929,150,75,0,91,449,490,456,472,438,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195246","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 00:37:23","MIRACOM","CSP_A_Wplasma-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","RY41-01",700,500,300,600.82,455.414,310,459.8,0,0,0,50.98725331,150,75,0,129,436,464,486,458,512,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195247","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 00:37:49","MIRACOM","CSP_A_QFN-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","1252AT1810",700,500,300,600.82,455.414,310,483.2,0,0,0,62.05400873,150,75,0,143,499,515,498,506,438,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195248","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 00:55:37","MIRACOM","CSP_A_QFNPUNCH-LARGE-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","P-A-3FTNB00000.H",700,500,300,600.82,455.414,310,446.6,0,0,0,33.56039332,150,75,0,77,480,470,424,439,457,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195249","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 01:22:52","MIRACOM","CSP_A_PUNCH-LARGE-Plasma","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","PLAK1252AF",700,500,300,600.82,455.414,310,429,0,0,0,23.65375235,150,75,0,57,428,528,449,450,415,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195250","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 01:23:21","MIRACOM","CSP_A_QFNPUNCH-LARGE-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","P-A-3C1TB00000.C",700,500,300,600.82,455.414,310,466.8,0,0,0,43.98522479,150,75,0,112,429,523,418,529,398,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195251","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 02:49:47","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1250034",700,500,300,600.82,455.414,310,461.4,0,0,0,48.08638061,150,75,0,130,510,417,392,504,411,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195252","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 02:50:11","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDP1251001",700,500,300,600.82,455.414,310,478.2,0,0,0,51.16346353,150,75,0,134,418,506,379,468,503,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195253","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 02:50:37","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1250004",700,500,300,600.82,455.414,310,444.8,0,0,0,51.41206084,150,75,0,126,452,500,425,410,432,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195254","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 03:16:21","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1250070",700,500,300,600.82,455.414,310,488.2,0,0,0,51.80443996,150,75,0,111,452,521,411,403,433,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195255","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 03:29:28","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAP682.1F4",700,500,300,600.82,455.414,310,437.8,0,0,0,32.01093563,150,75,0,79,452,522,432,410,423,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195256","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 04:57:25","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","3R2A066.1F2",700,500,300,600.82,455.414,310,499,0,0,0,21.30727575,150,75,0,53,428,463,435,441,432,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195257","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 05:23:56","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCM1251004",700,500,300,600.82,455.414,310,442.6,0,0,0,38.16149892,150,75,0,103,394,402,438,483,399,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195258","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 05:24:17","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMBA1250031",700,500,300,600.82,455.414,310,439,0,0,0,38.50324662,150,75,0,102,422,431,463,401,428,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195259","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 06:40:27","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","XJ2Y51AAC0",700,500,300,600.82,455.414,310,496,0,0,0,21.41261311,150,75,0,52,446,452,401,526,458,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195260","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 09:11:15","MIRACOM","CSP_A_Wplasma-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","QBT480.1C",700,500,300,600.82,455.414,310,473.4,0,0,0,34.63812928,150,75,0,89,432,413,484,420,452,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195261","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 09:11:29","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1249087",700,500,300,600.82,455.414,310,449.8,0,0,0,31.23619695,150,75,0,73,398,494,502,515,445,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195262","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 10:26:01","MIRACOM","CSP_A_QFN-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","1252AT1812",700,500,300,600.82,455.414,310,474.6,0,0,0,36.88224505,150,75,0,98,401,421,400,463,435,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195263","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 13:43:22","MIRACOM","CSP_A_Wplasma-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","RPAHC4F196125201",700,500,300,600.82,455.414,310,455,0,0,0,48.1300322,150,75,0,125,389,412,399,432,421,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195264","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 14:28:06","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1250023",700,500,300,600.82,455.414,310,446.2,0,0,0,21.6032405,150,75,0,46,499,530,472,523,440,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195265","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 14:53:28","MIRACOM","CSP_A_KIONIX-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","KN1251AAM",700,500,300,600.82,455.414,310,448.4,0,0,0,20.23116408,150,75,0,46,402,396,385,456,497,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195266","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 14:53:49","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGC900.1F1",700,500,300,600.82,455.414,310,438.2,0,0,0,23.02607218,150,75,0,61,381,484,395,417,475,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195267","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 16:16:44","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1250023",700,500,300,600.82,455.414,310,430,0,0,0,19.06567596,150,75,0,52,488,402,469,492,502,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195268","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 16:26:50","MIRACOM","ACX_A_XYLATERAL-3","","","Plating_ALL","PLT001","APW","","EQUIPMENT","K2A52W001",700,500,300,600.82,455.414,310,441.6,0,0,0,40.96705994,150,75,0,88,481,502,414,436,499,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195269","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 17:05:51","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","APW","","EQUIPMENT","HUSH-S866",700,500,300,600.82,455.414,310,441.2,0,0,0,26.73387364,150,75,0,65,450,521,411,423,430,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195270","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 17:06:16","MIRACOM","CSP_A_QFNPUNCH-LARGE-NEW","","","Plating_ALL","PLT002","APW","","EQUIPMENT","P-A-3EHSB00000.F",700,500,300,600.82,455.414,310,452.8,0,0,0,20.36418425,150,75,0,51,463,419,429,528,512,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195271","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 17:28:28","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","APW","","EQUIPMENT","XJ2D50AAA0",700,500,300,600.82,455.414,310,447.8,0,0,0,22.4432618,150,75,0,60,431,414,412,463,458,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195272","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 17:28:53","MIRACOM","CSP_A_PUNCH-LARGE-Plasma","","","Plating_ALL","PLT002","APW","","EQUIPMENT","LOWELL13-C001",700,500,300,600.82,455.414,310,453.2,0,0,0,23.18835915,150,75,0,57,460,489,408,456,481,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195273","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 17:29:16","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","APW","","EQUIPMENT","FAP654.1F3",700,500,300,600.82,455.414,310,446,0,0,0,27.47726333,150,75,0,68,461,409,418,411,495,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195274","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 18:13:03","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAP1250088",700,500,300,600.82,455.414,310,516.4,0,0,0,25.66709956,150,75,0,64,455,410,482,469,528,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195275","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 20:05:56","MIRACOM","CSP_A_QFNAMS-3NEW-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","AMC1251XAJ-B",700,500,300,600.82,455.414,310,509.8,0,0,0,12.35718415,150,75,0,29,430,402,421,476,504,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195276","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 22:26:03","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGG276.1F2",700,500,300,600.82,455.414,310,460,0,0,0,40.29267924,150,75,0,87,440,495,502,517,422,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195277","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 22:52:38","MIRACOM","CSP_A_2DIECURE-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","HA252AAA11",700,500,300,600.82,455.414,310,489.8,0,0,0,64.28219038,150,75,0,146,481,402,436,495,500,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195278","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 23:15:44","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMEZ1250031",700,500,300,600.82,455.414,310,497.4,0,0,0,41.51264867,150,75,0,95,455,481,426,436,457,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195279","PLT_300~700_THK","PLT_300~700_THK","2014-05-20 23:52:26","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCD1250002",700,500,300,600.82,455.414,310,489.8,0,0,0,40.39430653,150,75,0,111,400,458,495,502,436,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195280","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 00:18:12","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAM987.1F2",700,500,300,600.82,455.414,310,421.4,0,0,0,21.12581359,150,75,0,48,433,428,445,503,537,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195281","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 00:23:35","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDP1251012",700,500,300,600.82,455.414,310,481.2,0,0,0,63.65689279,150,75,0,131,466,431,513,524,487,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195282","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 00:23:54","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDP1251012",700,500,300,600.82,455.414,310,481.2,0,0,0,63.65689279,150,75,0,131,504,408,514,420,481,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195283","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 00:24:15","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1250021",700,500,300,600.82,455.414,310,483.8,0,0,0,55.13347441,150,75,0,133,535,523,436,423,517,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195284","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 00:54:57","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S872",700,500,300,600.82,455.414,310,467.2,0,0,0,38.05522303,150,75,0,94,396,385,456,520,504,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195285","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 03:44:41","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T205473AA",700,500,300,600.82,455.414,310,450,0,0,0,40.34228551,150,75,0,89,443,369,387,405,421,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195286","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 04:11:29","MIRACOM","CSP_A_PUNCH-LARGE-Plasma","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","PLAK1252AD",700,500,300,600.82,455.414,310,439,0,0,0,29.26602125,150,75,0,75,430,546,420,440,498,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195287","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 05:01:03","MIRACOM","CSP_A_QFNSLAB-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","585992.1",700,500,300,600.82,455.414,310,424.6,0,0,0,33.23100961,150,75,0,81,444,532,523,425,511,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195288","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 05:10:33","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1250013",700,500,300,600.82,455.414,310,469.6,0,0,0,40.89987775,150,75,0,102,452,520,433,425,410,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195289","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 05:10:52","MIRACOM","CSP_A_Wplasma-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","QBT480.1H",700,500,300,600.82,455.414,310,477.4,0,0,0,44.06018611,150,75,0,116,462,511,432,421,403,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195290","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 10:28:04","MIRACOM","CSP_A_Wplasma-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","P-A-3D11B00000.C",700,500,300,600.82,455.414,310,442.2,0,0,0,36.25879204,150,75,0,92,450,512,432,411,425,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195291","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 11:35:02","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T206149AE",700,500,300,600.82,455.414,310,473.4,0,0,0,42.74692971,150,75,0,89,450,511,432,422,410,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195292","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 14:16:32","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAZ1251002",700,500,300,600.82,455.414,310,453.6,0,0,0,34.86115317,150,75,0,79,445,460,427,523,512,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195293","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 14:32:18","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","ODINII-T306",700,500,300,600.82,455.414,310,442,0,0,0,18.78829423,150,75,0,46,506,464,416,474,455,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195294","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 14:32:43","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAP183.1F2",700,500,300,600.82,455.414,310,450.2,0,0,0,18.89973545,150,75,0,48,434,489,453,461,472,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195295","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 14:33:08","MIRACOM","CSP_A_PUNCH-LARGE-Plasma","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","PLAK1252AB",700,500,300,600.82,455.414,310,444.8,0,0,0,22.46552915,150,75,0,59,423,498,512,479,469,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195296","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 14:36:26","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAP1250049",700,500,300,600.82,455.414,310,416.2,0,0,0,16.51363073,150,75,0,40,389,463,492,451,455,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195297","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 14:36:46","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1249098",700,500,300,600.82,455.414,310,487.2,0,0,0,12.23519514,150,75,0,30,423,485,463,451,477,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195298","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 15:02:02","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCM1252011",700,500,300,600.82,455.414,310,459.2,0,0,0,36.52670256,150,75,0,79,423,469,482,517,477,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195299","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 15:10:24","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCD1252001",700,500,300,600.82,455.414,310,419.4,0,0,0,12.97304899,150,75,0,35,452,500,432,422,410,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195300","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 15:27:46","MIRACOM","CSP_A_QFNSLAB-LARGE-QUAL","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","586904.1",700,500,300,600.82,455.414,310,445.2,0,0,0,42.52881376,150,75,0,93,452,532,411,425,430,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195301","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 15:28:11","MIRACOM","CSP_A_QFNSLAB-LARGE-QUAL","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","585035.1",700,500,300,600.82,455.414,310,490,0,0,0,15.16575089,150,75,0,38,462,532,411,432,421,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195302","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 15:28:30","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1250060",700,500,300,600.82,455.414,310,423.4,0,0,0,16.63730747,150,75,0,38,452,522,431,422,410,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195303","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 16:06:32","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T200593.1BFTJ",700,500,300,600.82,455.414,310,450.8,0,0,0,25.11374126,150,75,0,65,482,422,488,513,522,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195304","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 16:13:04","MIRACOM","CSP_A_QFNAMS-3NEW-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","AMC1251XAJ-K",700,500,300,600.82,455.414,310,451.8,0,0,0,20.11715686,150,75,0,47,409,453,513,507,466,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195305","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 16:36:25","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","ADAM1252AAC",700,500,300,600.82,455.414,310,447,0,0,0,21.36586062,150,75,0,57,485,529,518,568,493,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195306","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 18:44:09","MIRACOM","CSP_A_Wplasma-NEW","","","Plating_ALL","PLT002","APW","","EQUIPMENT","P-A-3D11B00000.A",700,500,300,600.82,455.414,310,449.4,0,0,0,18.79627623,150,75,0,51,500,432,400,532,584,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195307","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 18:44:30","MIRACOM","CSP_A_KIONIX-2DIE","","","Plating_ALL","PLT002","APW","","EQUIPMENT","KN1251AAN",700,500,300,600.82,455.414,310,465,0,0,0,23.94786003,150,75,0,59,435,510,488,494,598,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195308","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 18:49:28","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","APW","","EQUIPMENT","GGG329.1F1",700,500,300,600.82,455.414,310,446.8,0,0,0,31.87789203,150,75,0,75,480,445,388,503,490,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195309","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 18:57:14","MIRACOM","CSP_A_QFNSLAB-LARGE-NEW","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","583252.1",700,500,300,600.82,455.414,310,445.8,0,0,0,39.9837467,150,75,0,93,493,478,402,517,467,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195310","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 18:57:41","MIRACOM","CSP_A_QFNSLAB-LARGE-QUAL","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","584706.1",700,500,300,600.82,455.414,310,493.2,0,0,0,13.08434179,150,75,0,35,507,431,474,475,434,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195311","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 18:58:08","MIRACOM","CSP_A_QFNAMS-3NEW-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","AMC1251XAJ-G",700,500,300,600.82,455.414,310,428.2,0,0,0,15.54670383,150,75,0,41,402,416,511,504,507,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195312","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 19:10:18","MIRACOM","CSP_A_QFNSLAB-LARGE-QUAL","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","583858.1",700,500,300,600.82,455.414,310,489.2,0,0,0,20.38872237,150,75,0,52,455,482,402,469,495,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195313","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 19:16:25","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCJ1252021",700,500,300,600.82,455.414,310,424.8,0,0,0,14.80540442,150,75,0,32,462,532,422,415,433,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195314","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 22:21:57","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T206149AF",700,500,300,600.82,455.414,310,470.6,0,0,0,47.60567193,150,75,0,117,462,532,425,411,433,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195315","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 22:43:58","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","ODINII-T305",700,500,300,600.82,455.414,310,417.2,0,0,0,12.0913192,150,75,0,32,450,523,421,433,462,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195316","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 22:57:55","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAP1249049",700,500,300,600.82,455.414,310,455.4,0,0,0,47.37404353,150,75,0,116,516,462,455,519,523,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195317","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 22:58:14","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAI1252001",700,500,300,600.82,455.414,310,430.2,0,0,0,40.7394158,150,75,0,107,492,407,415,465,466,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195318","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 22:58:36","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMEZ1250039",700,500,300,600.82,455.414,310,478,0,0,0,50.70995957,150,75,0,132,524,415,464,433,512,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195319","PLT_300~700_THK","PLT_300~700_THK","2014-05-21 23:08:16","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAP732.1F2",700,500,300,600.82,455.414,310,435.8,0,0,0,32.53767048,150,75,0,78,514,497,442,522,467,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195320","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 00:19:18","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCM1252015",700,500,300,600.82,455.414,310,486.6,0,0,0,38.56552865,150,75,0,103,482,430,419,442,504,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195321","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 00:20:07","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMBO1249010",700,500,300,600.82,455.414,310,480.8,0,0,0,41.60168266,150,75,0,106,431,502,399,453,511,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195322","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 00:20:39","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1249013",700,500,300,600.82,455.414,310,491.2,0,0,0,41.66773332,150,75,0,101,458,493,491,460,508,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195323","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 00:25:53","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1250026",700,500,300,600.82,455.414,310,456.8,0,0,0,51.9056837,150,75,0,132,410,435,490,389,346,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195324","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 00:26:13","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDR1252020",700,500,300,600.82,455.414,310,477.6,0,0,0,50.16273517,150,75,0,132,496,432,481,465,402,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195325","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 00:43:39","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1249060",700,500,300,600.82,455.414,310,480.6,0,0,0,46.70438952,150,75,0,107,479,398,393,472,434,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195326","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 01:28:27","MIRACOM","CSP_A_QFNAMS-3PUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","AME1251XAA-V",700,500,300,600.82,455.414,310,436.2,0,0,0,25.52841554,150,75,0,62,511,493,423,504,446,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195327","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 01:46:23","MIRACOM","CSP_A_QFNAMS-3NEW_C","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","AME1252XAE-A",700,500,300,600.82,455.414,310,432.2,0,0,0,33.7446292,150,75,0,86,452,532,421,411,403,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195328","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 02:58:20","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGG321.1F4",700,500,300,600.82,455.414,310,430.2,0,0,0,23.44568191,150,75,0,62,462,523,422,410,432,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195329","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 02:59:35","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAZ1252005",700,500,300,600.82,455.414,310,420.6,0,0,0,21.62868466,150,75,0,52,462,522,432,425,401,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195330","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 04:53:55","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCJ1252007",700,500,300,600.82,455.414,310,439,0,0,0,41.59927884,150,75,0,104,442,520,425,433,415,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195331","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 04:54:24","MIRACOM","CSP_A_2DIECURE-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","HA252AAA06",700,500,300,600.82,455.414,310,517,0,0,0,53.03772242,150,75,0,130,453,520,425,411,403,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195332","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 04:54:58","MIRACOM","CSP_A_QFNSLAB-LARGE-QUAL","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","583868.1",700,500,300,600.82,455.414,310,520.2,0,0,0,33.26710087,150,75,0,86,462,520,422,430,412,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195333","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 05:40:06","MIRACOM","CSP_A_QFNAMS-3NEW-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","AMB1251XAH-B",700,500,300,600.82,455.414,310,466,0,0,0,39.26194086,150,75,0,100,450,510,432,411,403,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195334","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 05:40:25","MIRACOM","CSP_A_Wplasma-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","RY42-01",700,500,300,600.82,455.414,310,455,0,0,0,53.53036521,150,75,0,125,449,406,459,501,480,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195335","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 09:14:54","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","ODINII-T314",700,500,300,600.82,455.414,310,464.8,0,0,0,51.14391459,150,75,0,114,432,451,463,455,438,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195336","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 09:18:16","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","XJ2Y52AAD0",700,500,300,600.82,455.414,310,451.2,0,0,0,54.44905876,150,75,0,133,453,406,458,468,428,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195337","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 09:18:43","MIRACOM","CSP_A_QFNAMS-3PUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","AME1251XAA-W",700,500,300,600.82,455.414,310,467.6,0,0,0,61.7438256,150,75,0,129,466,412,482,404,436,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195338","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 09:19:50","MIRACOM","CSP_A_QFNAMS-3PUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","AME1251XAA-W",700,500,300,600.82,455.414,310,440.6,0,0,0,52.11813504,150,75,0,128,482,448,467,454,512,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195339","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 14:23:52","MIRACOM","CSP_A_QFN_RFMD-4PMC-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","R252BT001",700,500,300,600.82,455.414,310,442.6,0,0,0,21.47789561,150,75,0,54,445,521,503,398,395,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195340","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 14:37:54","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1249062",700,500,300,600.82,455.414,310,453.6,0,0,0,52.36697433,150,75,0,130,427,402,387,396,405,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195341","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 14:48:51","MIRACOM","CSP_A_SW-LARGE-3DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SWALG203210P",700,500,300,600.82,455.414,310,460,0,0,0,41.00609711,150,75,0,110,389,395,402,415,463,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195342","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 15:08:31","MIRACOM","CSP_A_QFNADS-2NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","ADAH1248AAJ",700,500,300,600.82,455.414,310,434.6,0,0,0,23.96455716,150,75,0,61,404,385,396,377,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195343","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 15:09:03","MIRACOM","CSP_A_QFNAMS-4NEW_C","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","AME1252XAC03",700,500,300,600.82,455.414,310,445.6,0,0,0,26.07297451,150,75,0,70,488,495,502,517,528,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195344","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 15:47:48","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","LOWELL-R963",700,500,300,600.82,455.414,310,434.8,0,0,0,25.27251471,150,75,0,62,402,432,415,455,501,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195345","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 15:48:15","MIRACOM","CSP_A_PUNCH-LARGE-Plasma","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH13-R033",700,500,300,600.82,455.414,310,450.4,0,0,0,24.3372143,150,75,0,61,450,520,415,433,425,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195346","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 15:48:38","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","SWAEPCMA901E",700,500,300,600.82,455.414,310,441.4,0,0,0,25.42243104,150,75,0,61,505,498,453,553,537,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195347","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 15:49:01","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T205116AU",700,500,300,600.82,455.414,310,444.4,0,0,0,24.06865181,150,75,0,61,465,440,493,411,446,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195348","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 15:49:25","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAN261.1F1",700,500,300,600.82,455.414,310,444.6,0,0,0,24.48060457,150,75,0,61,515,446,470,435,457,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195349","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 19:58:16","MIRACOM","CSP_A_KIONIX-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","KN1250AAE",700,500,300,600.82,455.414,310,455.8,0,0,0,26.2144998,150,75,0,66,526,522,505,492,482,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195350","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 20:19:26","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCJ1252003",700,500,300,600.82,455.414,310,447.6,0,0,0,40.37697364,150,75,0,100,485,416,476,447,418,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195351","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 22:45:07","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T200593.1BETJ",700,500,300,600.82,455.414,310,446,0,0,0,31.06444913,150,75,0,79,526,513,507,498,465,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195352","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 22:56:27","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1250047",700,500,300,600.82,455.414,310,472.8,0,0,0,41.8592881,150,75,0,104,405,426,503,522,510,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195353","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 22:56:47","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1252053",700,500,300,600.82,455.414,310,453.2,0,0,0,39.42968425,150,75,0,96,406,416,455,502,513,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195354","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 23:02:16","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T202940.7ADB",700,500,300,600.82,455.414,310,441.6,0,0,0,39.44996831,150,75,0,93,405,481,436,495,455,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195355","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 23:09:07","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T206149AS",700,500,300,600.82,455.414,310,444.2,0,0,0,42.16870878,150,75,0,87,481,402,436,495,457,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195356","PLT_300~700_THK","PLT_300~700_THK","2014-05-22 23:10:46","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1249005",700,500,300,600.82,455.414,310,481,0,0,0,56.00446411,150,75,0,145,497,438,456,518,530,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195357","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 00:00:59","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCZ1252009",700,500,300,600.82,455.414,310,465.6,0,0,0,41.57884077,150,75,0,111,488,463,455,447,452,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195358","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 00:09:47","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAZ1252003",700,500,300,600.82,455.414,310,430.2,0,0,0,23.12358104,150,75,0,53,430,497,429,431,469,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195359","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 00:47:30","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAN261.1F3",700,500,300,600.82,455.414,310,438.8,0,0,0,48.0229112,150,75,0,116,499,494,483,524,531,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195360","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 01:11:37","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S862",700,500,300,600.82,455.414,310,441.8,0,0,0,43.0313839,150,75,0,90,400,434,437,513,504,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195361","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 02:29:48","MIRACOM","CSP_A_QFNAMS-3NEW-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","AMC1251XAJ-N",700,500,300,600.82,455.414,310,435,0,0,0,25.38700455,150,75,0,65,489,448,422,516,533,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195362","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 03:20:21","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCZ1252006",700,500,300,600.82,455.414,310,480,0,0,0,36.73554137,150,75,0,97,475,460,411,459,510,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195363","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 03:38:53","MIRACOM","CSP_A_QFNAMS-3_C","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","AMA1251XAM01",700,500,300,600.82,455.414,310,461.6,0,0,0,49.36901863,150,75,0,108,421,448,463,401,519,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195364","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 04:22:21","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCD1252007",700,500,300,600.82,455.414,310,482.6,0,0,0,39.13182848,150,75,0,98,501,461,409,507,416,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195365","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 04:23:15","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAX1252005",700,500,300,600.82,455.414,310,460.4,0,0,0,35.57808314,150,75,0,79,523,511,467,476,499,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195366","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 04:55:23","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","ODINII-T316",700,500,300,600.82,455.414,310,437.4,0,0,0,23.45847395,150,75,0,59,509,511,523,533,470,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195367","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 08:59:02","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","ADAM1252AAB",700,500,300,600.82,455.414,310,501.8,0,0,0,41.19101844,150,75,0,88,450,500,509,511,478,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195368","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 09:43:30","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCM1252023",700,500,300,600.82,455.414,310,456,0,0,0,33.00757489,150,75,0,78,476,480,455,435,440,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195369","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 09:43:53","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDR1252096",700,500,300,600.82,455.414,310,453.6,0,0,0,36.77363186,150,75,0,95,534,405,435,433,444,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195370","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 14:28:43","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAZ1252010",700,500,300,600.82,455.414,310,450.8,0,0,0,43.37856614,150,75,0,100,469,455,503,463,502,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195371","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 14:29:26","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAA1250013",700,500,300,600.82,455.414,310,461.6,0,0,0,58.47478089,150,75,0,130,537,468,476,527,559,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195372","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 14:29:42","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1250030",700,500,300,600.82,455.414,310,477,0,0,0,43.8805196,150,75,0,101,450,511,432,411,450,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195373","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 14:30:34","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252001",700,500,300,600.82,455.414,310,453.8,0,0,0,38.51233569,150,75,0,100,462,523,421,403,421,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195374","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 14:36:36","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAR318.1F1",700,500,300,600.82,455.414,310,449.4,0,0,0,23.06078923,150,75,0,63,450,510,421,411,403,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195375","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 14:38:46","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T206149BC",700,500,300,600.82,455.414,310,448.8,0,0,0,29.44825971,150,75,0,70,509,454,413,487,429,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195376","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 14:39:23","MIRACOM","CSP_A_PUNCH-LARGE-Plasma","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH13-R031",700,500,300,600.82,455.414,310,449,0,0,0,31.08858311,150,75,0,72,510,469,431,441,489,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195377","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 15:46:50","MIRACOM","CSP_A_QFNInctech-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","ICAD1250AH",700,500,300,600.82,455.414,310,447,0,0,0,22.54994457,150,75,0,61,453,476,413,485,462,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195378","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 15:47:19","MIRACOM","CSP_A_QFNSLAB-LARGE-QUAL","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","586926.1",700,500,300,600.82,455.414,310,448.8,0,0,0,26.56501459,150,75,0,68,404,442,416,443,501,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195379","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 16:07:16","MIRACOM","CSP_A_Wplasma-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","2528922.1",700,500,300,600.82,455.414,310,446.4,0,0,0,17.03819239,150,75,0,45,426,458,493,511,466,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195380","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 19:54:08","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","6Z07Z2R1",700,500,300,600.82,455.414,310,454.8,0,0,0,50.5242516,150,75,0,120,488,421,402,495,503,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195381","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 22:42:36","MIRACOM","CSP_A_PUNCH-LARGE-Plasma","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH13-R030",700,500,300,600.82,455.414,310,453.4,0,0,0,43.71269838,150,75,0,100,406,522,401,432,418,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195382","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 23:12:55","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S922",700,500,300,600.82,455.414,310,457.8,0,0,0,51.35854359,150,75,0,124,455,453,402,495,481,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195383","PLT_300~700_THK","PLT_300~700_THK","2014-05-23 23:52:36","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAX1252009",700,500,300,600.82,455.414,310,464.6,0,0,0,42.27055713,150,75,0,108,425,401,431,415,408,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195384","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 00:13:07","MIRACOM","CSP_A_SM-LARGE-Plasma-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCV1249074",700,500,300,600.82,455.414,310,477.8,0,0,0,51.08032889,150,75,0,124,481,402,436,495,455,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195385","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 00:14:24","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAT1252014",700,500,300,600.82,455.414,310,483.8,0,0,0,40.80073529,150,75,0,105,506,480,423,507,447,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195386","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 00:14:33","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAP635.1F1",700,500,300,600.82,455.414,310,463.4,0,0,0,36.98378023,150,75,0,85,422,500,438,457,441,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195387","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 00:22:08","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T205116BC",700,500,300,600.82,455.414,310,437.4,0,0,0,40.07243442,150,75,0,97,458,539,476,538,510,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195388","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 00:33:22","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T202940.7AAB",700,500,300,600.82,455.414,310,444.4,0,0,0,43.35089388,150,75,0,95,511,548,542,498,467,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195389","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 00:46:27","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252002",700,500,300,600.82,455.414,310,480.8,0,0,0,40.69029368,150,75,0,105,495,419,442,558,520,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195390","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 01:21:44","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGG270.1F1",700,500,300,600.82,455.414,310,454.6,0,0,0,40.61773012,150,75,0,90,403,497,437,400,468,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195391","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 02:03:22","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAP1252001",700,500,300,600.82,455.414,310,482.8,0,0,0,42.27528829,150,75,0,106,506,442,453,419,533,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195392","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 02:32:11","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDB1252003",700,500,300,600.82,455.414,310,475,0,0,0,40.74923312,150,75,0,106,492,562,463,540,445,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195393","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 02:32:28","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDP1252041",700,500,300,600.82,455.414,310,443.4,0,0,0,28.8669361,150,75,0,79,422,481,402,436,458,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195394","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 04:06:42","MIRACOM","CSP_A_KIONIX-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","KN1250AAH",700,500,300,600.82,455.414,310,452.8,0,0,0,41.03900584,150,75,0,84,495,502,514,500,416,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195395","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 04:25:01","MIRACOM","CSP_A_QFNPUNCH-LARGE-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","P-A-3EHSB00000.D",700,500,300,600.82,455.414,310,456,0,0,0,31.81194744,150,75,0,70,485,503,527,514,500,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195396","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 07:09:04","MIRACOM","","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","",700,500,300,600.82,455.414,310,471.2,0,0,0,38.00920941,150,75,0,96,485,466,524,415,402,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195397","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 09:14:02","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","ADAM1252AAK",700,500,300,600.82,455.414,310,469.2,0,0,0,39.9149095,150,75,0,98,485,502,516,503,517,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195398","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 09:14:27","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T205116AZ",700,500,300,600.82,455.414,310,445.2,0,0,0,40.13975585,150,75,0,100,402,432,415,428,455,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195399","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 09:53:03","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAN323.1F1",700,500,300,600.82,455.414,310,476,0,0,0,35.40480193,150,75,0,88,422,481,402,436,495,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195400","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 10:27:59","MIRACOM","CSP_A_QFNAMS-3NEW_C","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","AME1252XAE-G",700,500,300,600.82,455.414,310,454.6,0,0,0,47.53209442,150,75,0,117,440,460,400,469,501,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195401","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 12:14:38","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S899",700,500,300,600.82,455.414,310,456.8,0,0,0,41.44514447,150,75,0,109,458,504,429,433,468,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195402","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 13:22:15","MIRACOM","CSP_A_PUNCH-LARGE-Plasma","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH13-R037",700,500,300,600.82,455.414,310,450.2,0,0,0,29.86134625,150,75,0,70,430,538,476,542,510,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195403","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 14:46:02","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","APW","","EQUIPMENT","SMDR1252060",700,500,300,600.82,455.414,310,442.4,0,0,0,27.6459762,150,75,0,74,497,441,496,488,533,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195404","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 14:46:29","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","APW","","EQUIPMENT","SMCO1252013",700,500,300,600.82,455.414,310,453.8,0,0,0,20.51097267,150,75,0,51,460,521,438,544,439,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195405","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 14:46:59","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","APW","","EQUIPMENT","SMDP1252049",700,500,300,600.82,455.414,310,454,0,0,0,20.21138293,150,75,0,49,463,422,456,559,502,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195406","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 14:47:03","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","APW","","EQUIPMENT","SMDP1252049",700,500,300,600.82,455.414,310,454,0,0,0,20.21138293,150,75,0,49,430,462,456,450,551,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195407","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 14:47:56","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","APW","","EQUIPMENT","SMEJ1252011",700,500,300,600.82,455.414,310,452.8,0,0,0,21.15892247,150,75,0,52,483,445,444,558,572,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195408","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 14:49:44","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","APW","","EQUIPMENT","HUSH-S898",700,500,300,600.82,455.414,310,452.2,0,0,0,11.30044247,150,75,0,30,413,457,398,379,491,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195409","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 15:00:55","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDB1252024",700,500,300,600.82,455.414,310,449.6,0,0,0,46.45750747,150,75,0,110,494,448,464,402,544,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195410","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 15:01:20","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAX1252011",700,500,300,600.82,455.414,310,459.6,0,0,0,45.97064281,150,75,0,100,423,406,453,395,387,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195411","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 15:01:59","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDR1252074",700,500,300,600.82,455.414,310,461.6,0,0,0,40.90598978,150,75,0,111,409,390,509,489,451,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195412","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 15:02:35","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMEJ1252011",700,500,300,600.82,455.414,310,457.4,0,0,0,38.97178467,150,75,0,100,450,510,422,446,412,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195413","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 16:34:36","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","APW","","EQUIPMENT","HUSH-S924",700,500,300,600.82,455.414,310,434.6,0,0,0,23.86000838,150,75,0,60,455,486,472,468,458,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195414","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 16:34:57","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","APW","","EQUIPMENT","FAN323.1F3",700,500,300,600.82,455.414,310,445.6,0,0,0,24.75479751,150,75,0,66,407,419,405,428,456,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195415","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 17:40:17","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCZ1301001",700,500,300,600.82,455.414,310,469.4,0,0,0,46.76323342,150,75,0,120,442,470,410,436,411,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195416","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 18:25:51","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1252058",700,500,300,600.82,455.414,310,449.2,0,0,0,32.98029715,150,75,0,84,470,395,499,399,408,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195417","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 18:27:29","MIRACOM","CSP_A_QFNAMS-3NEW-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","AMG1252XAA-G",700,500,300,600.82,455.414,310,456.2,0,0,0,31.46744349,150,75,0,80,411,423,469,522,501,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195418","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 22:15:11","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T205116AQ",700,500,300,600.82,455.414,310,425.6,0,0,0,17.34358671,150,75,0,46,399,456,421,502,511,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195419","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 22:23:36","MIRACOM","ACX_A_HIGHG-3","","","Plating_ALL","PLT001","ACC","","EQUIPMENT","K2C524418",700,500,300,600.82,455.414,310,448,0,0,0,39.14715826,150,75,0,85,389,420,455,497,503,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195420","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 22:38:48","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDB1252009",700,500,300,600.82,455.414,310,477,0,0,0,24.22808288,150,75,0,63,435,454,499,523,518,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195421","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 22:39:09","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMEJ1252006",700,500,300,600.82,455.414,310,479.6,0,0,0,47.81004079,150,75,0,112,468,421,399,387,512,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195422","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 22:39:34","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1252008",700,500,300,600.82,455.414,310,484.4,0,0,0,31.1897419,150,75,0,75,444,421,398,375,465,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195423","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 22:39:44","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1252008",700,500,300,600.82,455.414,310,484.4,0,0,0,31.1897419,150,75,0,75,512,498,487,462,481,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195424","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 22:40:14","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMEJ1252005",700,500,300,600.82,455.414,310,478.4,0,0,0,31.42133033,150,75,0,81,488,465,502,527,418,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195425","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 22:59:19","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAR449.1F2",700,500,300,600.82,455.414,310,446.2,0,0,0,30.4253184,150,75,0,79,421,403,435,431,417,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195426","PLT_300~700_THK","PLT_300~700_THK","2014-05-24 23:35:57","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","XJ3Y01ABC0",700,500,300,600.82,455.414,310,449.8,0,0,0,36.0652187,150,75,0,82,492,462,438,481,502,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195427","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 00:13:16","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGG278.1F1",700,500,300,600.82,455.414,310,479.4,0,0,0,46.84335599,150,75,0,121,481,402,433,495,502,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195428","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 01:16:56","MIRACOM","APW_A_PQFN_STM12X12","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HA251CBC91",700,500,300,600.82,455.414,310,440.8,0,0,0,31.32411212,150,75,0,79,425,399,411,386,488,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195429","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 02:41:12","MIRACOM","CSP_A_Wplasma-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","RY43-01",700,500,300,600.82,455.414,310,495,0,0,0,59.60285228,150,75,0,149,527,495,500,501,485,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195430","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 02:56:41","MIRACOM","CSP_A_QFNSLAB-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","585998.1",700,500,300,600.82,455.414,310,435.6,0,0,0,37.30013405,150,75,0,83,400,412,421,408,416,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195431","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 03:33:26","MIRACOM","CSP_A_QFNAMS-4NEW_C","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","AMC1251XAB20",700,500,300,600.82,455.414,310,436.8,0,0,0,27.68031792,150,75,0,66,418,422,402,436,491,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195432","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 03:33:52","MIRACOM","CSP_A_QFNAMS-3NEW_C","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","AMC1252XAD-B",700,500,300,600.82,455.414,310,454.6,0,0,0,48.56233108,150,75,0,120,450,521,421,466,402,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195433","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 04:25:03","MIRACOM","CSP_A_KIONIX-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","KN1301AAR",700,500,300,600.82,455.414,310,435.6,0,0,0,29.4923719,150,75,0,70,462,510,432,411,462,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195434","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 05:04:15","MIRACOM","CSP_A_QFN-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","1301AT1873",700,500,300,600.82,455.414,310,483.4,0,0,0,46.15517306,150,75,0,121,450,510,425,433,475,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195435","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 06:34:55","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S926",700,500,300,600.82,455.414,310,483.8,0,0,0,32.63740186,150,75,0,86,453,419,468,433,472,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195436","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 07:07:43","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAP635.1F4",700,500,300,600.82,455.414,310,479,0,0,0,59.79130372,150,75,0,128,498,401,450,489,460,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195437","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 14:26:40","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S880",700,500,300,600.82,455.414,310,406.4,0,0,0,24.79516082,150,75,0,64,486,400,412,466,458,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195438","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 14:30:51","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1252063",700,500,300,600.82,455.414,310,444.2,0,0,0,42.92668168,150,75,0,100,494,503,420,444,475,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195439","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 14:31:30","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252085",700,500,300,600.82,455.414,310,435.4,0,0,0,47.6266732,150,75,0,105,432,454,505,523,444,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195440","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 14:32:25","MIRACOM","CSP_A_Wplasma-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","RPAHC4F196125226",700,500,300,600.82,455.414,310,460,0,0,0,45.16082373,150,75,0,100,433,456,428,472,419,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195441","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 14:32:58","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDR1252045",700,500,300,600.82,455.414,310,445,0,0,0,42.94182111,150,75,0,100,511,402,386,377,526,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195442","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 14:40:58","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAT1252013",700,500,300,600.82,455.414,310,449.2,0,0,0,48.18402225,150,75,0,111,405,423,396,387,392,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195443","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 15:26:36","MIRACOM","CSP_A_QFN-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","1301AT1876",700,500,300,600.82,455.414,310,466.2,0,0,0,41.09987835,150,75,0,100,510,502,436,478,390,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195444","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 15:42:44","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T204985AQ",700,500,300,600.82,455.414,310,507.8,0,0,0,38.95125158,150,75,0,100,504,510,399,387,379,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195445","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 16:08:09","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAN359.1F1",700,500,300,600.82,455.414,310,572.8,0,0,0,52.95469762,150,75,0,121,405,423,533,512,489,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195446","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 16:09:06","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMOD1301001",700,500,300,600.82,455.414,310,453.2,0,0,0,38.88058642,150,75,0,100,455,388,479,512,503,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195447","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 16:40:39","MIRACOM","CSP_A_QFNPUNCH-LARGE-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","P-A-3FPXB00000.B",700,500,300,600.82,455.414,310,457.6,0,0,0,44.7694092,150,75,0,107,407,523,511,395,466,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195448","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 20:01:10","MIRACOM","CSP_A_PUNCH-LARGE-Plasma","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","P252AH002",700,500,300,600.82,455.414,310,446.4,0,0,0,37.50066666,150,75,0,93,436,462,454,442,428,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195449","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 22:31:05","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S892",700,500,300,600.82,455.414,310,450.2,0,0,0,38.54477915,150,75,0,83,485,500,495,501,495,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195450","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 22:50:58","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1252094",700,500,300,600.82,455.414,310,434.4,0,0,0,32.00468716,150,75,0,83,405,436,411,427,438,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195451","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 22:56:09","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAP613.1F2",700,500,300,600.82,455.414,310,455.6,0,0,0,32.95906552,150,75,0,67,522,412,432,441,447,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195452","PLT_300~700_THK","PLT_300~700_THK","2014-05-25 23:05:28","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDR1252065",700,500,300,600.82,455.414,310,447.6,0,0,0,48.08118135,150,75,0,96,495,502,511,503,517,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195453","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 00:48:46","MIRACOM","CSP_A_QFN-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","1301AT1870",700,500,300,600.82,455.414,310,448.6,0,0,0,27.61883415,150,75,0,72,485,492,468,475,448,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195454","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 00:49:12","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMFJ1301001",700,500,300,600.82,455.414,310,446.4,0,0,0,30.59901959,150,75,0,79,454,495,408,423,499,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195455","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 01:42:41","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252099",700,500,300,600.82,455.414,310,480.6,0,0,0,43.80981625,150,75,0,115,508,425,411,435,512,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195456","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 03:11:51","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMEJ1301001",700,500,300,600.82,455.414,310,472,0,0,0,39.62322551,150,75,0,98,460,436,502,475,533,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195457","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 03:23:21","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T205116AW",700,500,300,600.82,455.414,310,454.6,0,0,0,39.36749929,150,75,0,90,544,427,463,499,507,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195458","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 03:52:42","MIRACOM","CSP_A_QFNSTM-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","HA301FAA01",700,500,300,600.82,455.414,310,434.4,0,0,0,23.95412282,150,75,0,58,489,543,502,426,513,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195459","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 04:18:15","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGG280.1F2",700,500,300,600.82,455.414,310,442,0,0,0,37.05401463,150,75,0,86,450,549,538,462,531,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195460","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 08:30:34","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T204985BH",700,500,300,600.82,455.414,310,486.4,0,0,0,45.67055944,150,75,0,110,489,543,502,426,516,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195461","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 11:24:04","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","XJ3Y01ABM0",700,500,300,600.82,455.414,310,495,0,0,0,28.21347196,150,75,0,68,385,425,512,523,399,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195462","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 11:59:57","MIRACOM","CSP_A_Full-LARGE-BG","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","ADAW1252AAA",700,500,300,600.82,455.414,310,446.4,0,0,0,37.28002146,150,75,0,91,450,395,387,441,402,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195463","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 12:00:21","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1252075",700,500,300,600.82,455.414,310,466.8,0,0,0,21.42895238,150,75,0,48,522,467,493,502,513,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195464","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 12:15:21","MIRACOM","CSP_A_QFNSLAB-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","585993.1",700,500,300,600.82,455.414,310,501,0,0,0,42.67317659,150,75,0,104,457,447,402,387,401,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195465","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 13:02:47","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGH949.1F2",700,500,300,600.82,455.414,310,416,0,0,0,32.01562119,150,75,0,81,497,529,404,493,488,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195466","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 14:20:42","MIRACOM","CSP_A_KIONIX-2DIE","","","Plating_ALL","PLT002","APW","","EQUIPMENT","KN1301AAY",700,500,300,600.82,455.414,310,438.4,0,0,0,23.09328907,150,75,0,59,406,437,444,536,495,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195467","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 14:21:11","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGH866.1F3",700,500,300,600.82,455.414,310,447.2,0,0,0,21.85634919,150,75,0,59,381,476,427,435,429,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195468","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 14:33:31","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDR1252107",700,500,300,600.82,455.414,310,487.6,0,0,0,45.23604757,150,75,0,113,447,467,428,523,533,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195469","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 14:57:13","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","XJ3Y01ABN0",700,500,300,600.82,455.414,310,446,0,0,0,29.2489316,150,75,0,66,420,502,460,513,509,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195470","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 20:21:43","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252032",700,500,300,600.82,455.414,310,444,0,0,0,28.24004249,150,75,0,74,516,433,493,480,459,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195471","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 20:22:25","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDR1252076",700,500,300,600.82,455.414,310,455.8,0,0,0,30.40065789,150,75,0,72,449,429,467,532,546,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195472","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 20:23:31","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAB1301002",700,500,300,600.82,455.414,310,459.8,0,0,0,28.27012557,150,75,0,72,401,491,478,432,451,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195473","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 22:37:00","MIRACOM","CSP_A_QFNPUNCH-LARGE-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","P-A-3FPYB00000.E",700,500,300,600.82,455.414,310,442.6,0,0,0,24.94594155,150,75,0,59,426,415,402,489,522,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195474","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 22:44:07","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252024",700,500,300,600.82,455.414,310,496.2,0,0,0,42.71650735,150,75,0,111,423,463,498,492,474,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195475","PLT_300~700_THK","PLT_300~700_THK","2014-05-26 22:58:40","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","0JK32001BM-03",700,500,300,600.82,455.414,310,432,0,0,0,27.24885319,150,75,0,71,489,463,451,471,412,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195476","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 00:48:47","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S953",700,500,300,600.82,455.414,310,423.4,0,0,0,24.60284536,150,75,0,61,396,452,496,512,521,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195477","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 01:30:49","MIRACOM","CSP_A_KIONIX-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","KN1301AAP",700,500,300,600.82,455.414,310,491.4,0,0,0,31.43723906,150,75,0,76,421,436,465,481,419,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195478","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 03:02:30","MIRACOM","CSP_A_QFNSLAB-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","585221.1",700,500,300,600.82,455.414,310,434,0,0,0,24.72852604,150,75,0,59,423,463,498,487,446,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195479","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 03:17:40","MIRACOM","CSP_A_QFN-Neptune","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","XJ3G01AAC0",700,500,300,600.82,455.414,310,426.8,0,0,0,23.3173755,150,75,0,60,432,469,511,393,488,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195480","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 03:29:47","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAB1301017",700,500,300,600.82,455.414,310,452.4,0,0,0,45.42356217,150,75,0,107,482,431,491,512,467,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195481","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 03:30:19","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDR1252015",700,500,300,600.82,455.414,310,446,0,0,0,28.87040007,150,75,0,73,421,452,416,494,473,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195482","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 04:14:45","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGG281.1F2",700,500,300,600.82,455.414,310,442.4,0,0,0,29.54318872,150,75,0,76,462,448,472,458,469,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195483","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 04:51:25","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","T204985AS",700,500,300,600.82,455.414,310,443,0,0,0,46.44351408,150,75,0,110,422,415,432,408,427,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195484","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 05:43:33","MIRACOM","CSP_A_QFNPUNCH-LARGE-NEW","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","P-A-3AG8B00000.I",700,500,300,600.82,455.414,310,446.6,0,0,0,61.05980675,150,75,0,149,395,512,412,405,422,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195485","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 06:44:26","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","FAN364.1F2",700,500,300,600.82,455.414,310,462.6,0,0,0,48.36631059,150,75,0,115,443,519,430,499,527,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195486","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 06:44:41","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","XJ3Y01ABF0",700,500,300,600.82,455.414,310,505.4,0,0,0,14.82565344,150,75,0,38,414,491,459,410,455,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195487","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 08:58:06","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","6Z08A6M",700,500,300,600.82,455.414,310,464.6,0,0,0,26.50094338,150,75,0,65,425,428,450,506,514,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195488","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 08:58:20","MIRACOM","CSP_A_Wplasma-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","2529972.1",700,500,300,600.82,455.414,310,465,0,0,0,18.09696107,150,75,0,47,489,476,502,550,548,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195489","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 09:20:45","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S909",700,500,300,600.82,455.414,310,442.6,0,0,0,63.48464381,150,75,0,143,412,431,502,515,521,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195490","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 09:42:54","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T205529AL",700,500,300,600.82,455.414,310,470.6,0,0,0,58.14034744,150,75,0,137,516,511,429,425,500,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195491","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 09:45:41","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252088",700,500,300,600.82,455.414,310,434.2,0,0,0,30.6300506,150,75,0,76,380,452,521,503,416,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195492","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 09:46:31","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDR1252087",700,500,300,600.82,455.414,310,469.8,0,0,0,22.56546033,150,75,0,58,425,495,410,510,432,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195493","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 09:46:59","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDP1252047",700,500,300,600.82,455.414,310,453.8,0,0,0,45.79519625,150,75,0,116,432,475,425,495,412,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195494","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 10:26:19","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAZ1301001",700,500,300,600.82,455.414,310,441,0,0,0,29.97498957,150,75,0,73,432,495,410,455,475,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195495","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 11:10:51","MIRACOM","CSP_A_QFN-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","1301AT1868",700,500,300,600.82,455.414,310,456.2,0,0,0,25.03397691,150,75,0,65,450,500,432,410,466,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195496","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 14:50:16","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAZ1301003",700,500,300,600.82,455.414,310,451.6,0,0,0,43.36242613,150,75,0,100,433,520,410,425,470,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195497","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 14:50:31","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAB1301010",700,500,300,600.82,455.414,310,444.4,0,0,0,42.69426191,150,75,0,100,452,462,475,531,470,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195498","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 14:51:42","MIRACOM","CSP_A_Wplasma-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","QBU934.1B",700,500,300,600.82,455.414,310,451.6,0,0,0,43.74128485,150,75,0,100,414,405,486,436,505,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195499","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 14:54:08","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252082",700,500,300,600.82,455.414,310,456.8,0,0,0,47.69381511,150,75,0,100,453,497,499,504,511,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195500","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 15:49:40","MIRACOM","CSP_A_QFN-Neptune","","","Plating_ALL","PLT002","APW","","EQUIPMENT","XJ3G01ABC0",700,500,300,600.82,455.414,310,445.6,0,0,0,22.30022421,150,75,0,59,461,422,428,516,498,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195501","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 16:15:32","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","FAP203.1F2",700,500,300,600.82,455.414,310,460,0,0,0,45.16082373,150,75,0,100,414,471,450,445,492,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195502","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 16:16:11","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCJ1301014",700,500,300,600.82,455.414,310,453.6,0,0,0,45.26919482,150,75,0,109,455,440,429,510,529,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195503","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 16:17:01","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1252022",700,500,300,600.82,455.414,310,473,0,0,0,35.44714375,150,75,0,89,415,475,462,433,410,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195504","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 18:08:47","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","FAN313.1F3",700,500,300,600.82,455.414,310,490.8,0,0,0,54.96999181,150,75,0,123,425,412,433,415,476,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195505","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 18:34:48","MIRACOM","CSP_A_QFN-Neptune","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","XJ3G01ABK0",700,500,300,600.82,455.414,310,469.2,0,0,0,18.6868938,150,75,0,48,425,465,415,432,466,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195506","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 18:35:12","MIRACOM","","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","XJ3Y01AAD0402",700,500,300,600.82,455.414,310,422,0,0,0,28.86173938,150,75,0,60,425,475,415,431,400,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195507","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 22:34:16","MIRACOM","CSP_A_QFNPUNCH-LARGE-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","P-A-3G5NB00000.F",700,500,300,600.82,455.414,310,423.4,0,0,0,26.59511233,150,75,0,67,425,475,412,462,430,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195508","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 23:05:08","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAZ1301002",700,500,300,600.82,455.414,310,438,0,0,0,40.07492982,150,75,0,88,425,475,413,422,403,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195509","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 23:05:50","MIRACOM","CSP_A_QFN-LARGE-DB","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","1301AT1872",700,500,300,600.82,455.414,310,480.4,0,0,0,48.96223034,150,75,0,132,425,475,431,410,406,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195510","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 23:11:02","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAP1301043",700,500,300,600.82,455.414,310,449,0,0,0,33.67491648,150,75,0,74,413,465,511,489,432,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195511","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 23:12:14","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAT1301027",700,500,300,600.82,455.414,310,441.4,0,0,0,33.93081196,150,75,0,82,483,405,460,453,486,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195512","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 23:12:41","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAT1301027",700,500,300,600.82,455.414,310,450,0,0,0,33.63034344,150,75,0,89,448,413,449,499,452,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195513","PLT_300~700_THK","PLT_300~700_THK","2014-05-27 23:36:00","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S948",700,500,300,600.82,455.414,310,447.4,0,0,0,30.36939249,150,75,0,80,392,423,386,492,402,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195514","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 00:05:04","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAB1301018",700,500,300,600.82,455.414,310,492.2,0,0,0,38.12086043,150,75,0,91,455,462,402,427,418,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195515","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 00:15:30","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252052",700,500,300,600.82,455.414,310,491.2,0,0,0,46.75147057,150,75,0,112,455,481,402,469,492,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195516","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 00:31:03","MIRACOM","ACX_A_HERA","","","Plating_ALL","PLT001","ACC","","EQUIPMENT","XJ3K01XAF0",700,500,300,600.82,455.414,310,416,0,0,0,33.05298776,150,75,0,85,481,453,503,517,446,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195517","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 00:33:51","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCO1301013",700,500,300,600.82,455.414,310,484.4,0,0,0,37.46731909,150,75,0,94,499,527,418,408,436,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195518","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 02:02:28","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","FAP658.1F1",700,500,300,600.82,455.414,310,466,0,0,0,63.65139433,150,75,0,141,488,462,402,517,403,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195519","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 02:36:48","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","T204985BD",700,500,300,600.82,455.414,310,445.6,0,0,0,39.31030399,150,75,0,99,489,522,544,582,555,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195520","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 02:54:57","MIRACOM","CSP_A_KIONIX-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","KN1301AAH",700,500,300,600.82,455.414,310,447.6,0,0,0,38.62382684,150,75,0,88,464,444,423,446,515,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195521","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 03:41:53","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","XJ3Y01AAB0",700,500,300,600.82,455.414,310,438.8,0,0,0,37.59255245,150,75,0,92,551,586,598,584,611,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195522","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 04:11:57","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGG318.1F3",700,500,300,600.82,455.414,310,462.6,0,0,0,41.84853641,150,75,0,99,484,465,416,500,517,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195523","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 05:17:25","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCJ1301017",700,500,300,600.82,455.414,310,478,0,0,0,48.16637832,150,75,0,122,422,403,436,502,520,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195524","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 08:52:45","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","FAP180.1F2",700,500,300,600.82,455.414,310,511,0,0,0,19.07878403,150,75,0,46,465,571,545,533,563,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195525","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 09:16:39","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDB1301006",700,500,300,600.82,455.414,310,452.2,0,0,0,33.07113545,150,75,0,71,485,433,402,427,471,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195526","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 09:16:53","MIRACOM","CSP_A_SM-LARGE-2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMAB1301022",700,500,300,600.82,455.414,310,459.6,0,0,0,27.06104211,150,75,0,67,466,485,457,447,419,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195527","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 09:17:09","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1301016",700,500,300,600.82,455.414,310,466.4,0,0,0,28.41302518,150,75,0,75,445,462,402,435,481,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195528","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 09:21:38","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","SWAEPCMA901G",700,500,300,600.82,455.414,310,516.6,0,0,0,111.4621012,150,75,0,307,402,481,436,495,500,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195529","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 10:13:58","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","XJ3Y01AAF0",700,500,300,600.82,455.414,310,450,0,0,0,41.76122604,150,75,0,94,455,482,502,514,436,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195530","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 14:34:23","MIRACOM","CSP_A_QFNPUNCH-QCT","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","GGH864.1F3",700,500,300,600.82,455.414,310,444.2,0,0,0,22.39866067,150,75,0,60,533,542,562,502,513,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195531","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 14:40:23","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMDB1301020",700,500,300,600.82,455.414,310,451.2,0,0,0,45.48846008,150,75,0,100,413,400,453,438,470,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195532","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 14:40:47","MIRACOM","CSP_A_QFNSM-LARGE-NEW-1DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCJ1301025",700,500,300,600.82,455.414,310,459.4,0,0,0,55.44186144,150,75,0,120,443,427,422,501,487,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195533","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 14:41:28","MIRACOM","CSP_A_Wplasma-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","NCX608301AA06",700,500,300,600.82,455.414,310,456.6,0,0,0,47.58466139,150,75,0,100,435,432,475,461,403,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195534","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 15:19:42","MIRACOM","CSP_A_QFNPUNCH-LARGE-NEW","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","P-A-3FPYB00000.C",700,500,300,600.82,455.414,310,453.2,0,0,0,26.80858072,150,75,0,70,425,431,405,444,432,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195535","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 15:20:11","MIRACOM","CSP_A_QFNPUNCH-LARGE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HUSH-S912",700,500,300,600.82,455.414,310,458.8,0,0,0,29.52456604,150,75,0,81,425,475,431,411,406,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195536","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 17:03:58","MIRACOM","CSP_A_QFNPUNCH-LARGE-QCT","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","FAP196.1F3",700,500,300,600.82,455.414,310,459.8,0,0,0,44.45447109,150,75,0,100,425,431,411,475,430,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195537","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 17:04:14","MIRACOM","CSP_A_QFNPUNCH-LARGE_D","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","XJ3Y01AAP0",700,500,300,600.82,455.414,310,452.2,0,0,0,52.76078089,150,75,0,121,425,431,475,425,400,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195538","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 17:04:29","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-3DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","T204985AY",700,500,300,600.82,455.414,310,461,0,0,0,48.05205511,150,75,0,110,415,432,475,455,406,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195539","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 17:08:19","MIRACOM","EBR_A_QFN1","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","6Z08T0M",700,500,300,600.82,455.414,310,462.4,0,0,0,43.3451266,150,75,0,100,452,461,475,422,410,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195540","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 18:27:21","MIRACOM","APW_A_PQFN_STM12X12","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","HA251CBC86",700,500,300,600.82,455.414,310,440.4,0,0,0,25.3929124,150,75,0,64,451,462,475,423,411,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195541","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 18:54:24","MIRACOM","CSP_A_SM-LARGE-P2DIE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","SMCP1252048",700,500,300,600.82,455.414,310,449,0,0,0,44.48595284,150,75,0,110,425,415,430,475,433,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195542","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 18:54:44","MIRACOM","CSP_A_Wplasma-LARGE","","","Plating_ALL","PLT004","ACC","","EQUIPMENT","QBU934.1A",700,500,300,600.82,455.414,310,463.2,0,0,0,49.12433206,150,75,0,128,425,461,406,432,477,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195543","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 22:17:32","MIRACOM","CSP_A_PUNCH-LARGE-Plasma-2DIE","","","Plating_ALL","PLT002","ACC","","EQUIPMENT","T205529AF",700,500,300,600.82,455.414,310,439.2,0,0,0,48.63332191,150,75,0,115,428,431,495,425,433,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});
m_dt.Rows.Add(new object[] {"3243","195544","PLT_300~700_THK","PLT_300~700_THK","2014-05-28 22:36:37","MIRACOM","ACX_A_XYLATERAL-3","","","Plating_ALL","PLT001","ACC","","EQUIPMENT","K3A01G025",700,500,300,600.82,455.414,310,405.6,0,0,0,14.99333185,150,75,0,40,425,415,475,462,433,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN});

#endregion

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Data Calculate

        private void DrawLabel()
        {
            try
            {
                if (chart1.Series[0] != null && m_I_CHART.ChartVisible == false)
                for (int i = 0; i < m_iCNT; i++)
                {
                    if (m_dt.Rows[i]["DISPLAY_FLAG"].ToString() == "N" && m_HiddenPointVisible == false) continue;

                    if (m_STR_LABEL_MEMBER == "TRAN_TIME")
                    {
                        if (m_dt.Columns[m_STR_LABEL_MEMBER].DataType == typeof(DateTime))
                        {
                            DateTime dtTime = (DateTime)m_dt.Rows[i][m_STR_LABEL_MEMBER];
                            chart1.Series[0].Points[i].AxisLabel = dtTime.ToString(m_STR_LABEL_FORMAT);
                        }
                        else
                        {
                            chart1.Series[0].Points[i].AxisLabel = m_dt.Rows[i][m_STR_LABEL_MEMBER].ToString();
                        }
                    }
                    else
                    {
                        chart1.Series[0].AxisLabel = m_dt.Rows[i][m_STR_LABEL_MEMBER].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DataCal()
        {
            if (m_dt == null) return;
            try
            {
                int iRowsCount = m_dt.Rows.Count;
                //1. Data Reset
                m_VAL_VISIBLE = new List<bool>();
                m_VAL_DSEQ = new List<long>();
                m_ALL_DSEQ_IDX = new List<long>();
                m_VAL_RESULT = new List<string>();
                m_VAL_RESULT_IMR = new List<string>();

                //m_VAL_AXIS_LABLE = new List<string>();

                m_VAL_XBAR = new List<double>();
                m_VAL_SIGMA = new List<double>();
                m_VAL_RANGE = new List<double>();
                m_VAL_RAW = new List<DataPoint>();
                m_VAL_EWMA_MV = new List<double>();
                m_VAL_EWMA_S = new List<double>();
                m_VAL_EWMA_R = new List<double>();
                m_VAL_MA = new List<double>();
                m_VAL_MS = new List<double>();
                m_VAL_I = new List<double>();
                m_VAL_MR = new List<double>();

                double tmp_XBAR = double.NaN;
                double tmp_SIGMA = double.NaN;
                //double tmp_RANGE = double.NaN;
                //double tmp_RAW = double.NaN;
                double tmp_EWMA_MV = double.NaN;
                double tmp_EWMA_S = double.NaN;
                double tmp_EWMA_R = double.NaN;
                double tmp_MA = double.NaN;
                double tmp_MS = double.NaN;

                //2. SPEC Reset
                m_USL = new List<double>();
                m_LSL = new List<double>();

                double tmp_USL = double.NaN;
                double tmp_LSL = double.NaN;

                //3. Control Reset
                m_XBAR_UCL = new List<double>();
                m_XBAR_LCL = new List<double>();

                m_XBAR_3S_UCL = new List<double>();
                m_XBAR_3S_LCL = new List<double>();

                m_SIGMA_UCL = new List<double>();
                m_SIGMA_LCL = new List<double>();

                m_RANGE_UCL = new List<double>();
                m_RANGE_LCL = new List<double>();

                m_EWMA_MV_UCL = new List<double>();
                m_EWMA_MV_LCL = new List<double>();

                m_EWMA_R_UCL = new List<double>();
                m_EWMA_R_LCL = new List<double>();

                m_EWMA_S_UCL = new List<double>();
                m_EWMA_S_LCL = new List<double>();

                m_MA_UCL = new List<double>();
                m_MA_LCL = new List<double>();

                m_MS_UCL = new List<double>();
                m_MS_LCL = new List<double>();

                m_IMR_USL = new List<double>();
                m_IMR_LSL = new List<double>();
                m_IMR_UCL = new List<double>();
                m_IMR_LCL = new List<double>();

                double tmp_XBAR_UCL = double.NaN;
                double tmp_XBAR_LCL = double.NaN;

                double tmp_SIGMA_UCL = double.NaN;
                double tmp_SIGMA_LCL = double.NaN;

                double tmp_RANGE_UCL = double.NaN;
                double tmp_RANGE_LCL = double.NaN;

                double tmp_EWMA_MV_UCL = double.NaN;
                double tmp_EWMA_MV_LCL = double.NaN;

                double tmp_EWMA_R_UCL = double.NaN;
                double tmp_EWMA_R_LCL = double.NaN;

                double tmp_EWMA_S_UCL = double.NaN;
                double tmp_EWMA_S_LCL = double.NaN;

                double tmp_MA_UCL = double.NaN;
                double tmp_MA_LCL = double.NaN;

                double tmp_MS_UCL = double.NaN;
                double tmp_MS_LCL = double.NaN;

                /// XBAR
                /// 
                double dSumOfSq = 0;
                double dRaw_SumOfSq = 0;
                double dRange_SumOfSq = 0;
                double dSigma_SumOfSq = 0;

                m_dRAW_SUM = 0;
                m_dSUM = 0;
                m_dRANGE_SUM = 0;
                m_dSIGMA_SUM = 0;

                double dMin = double.MinValue;
                double dMax = double.MinValue;
                double tmp = double.NaN;

                long tmp_DSEQ = -1;
                List<double> rValues = null;

                m_iSampleCount = 0;
                m_iCNT = 0;
                for (int ic = 0; ic < m_dt.Columns.Count; ic++)
                {
                    if (m_dt.Columns[ic].ColumnName.StartsWith("VALUE_") && m_dt.Columns[ic].ColumnName.StartsWith("VALUE_COUNT") == false) m_iSampleCount++;
                }

                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    if (long.TryParse(m_dt.Rows[i]["DATA_SEQ"].ToString(), out tmp_DSEQ) == false) tmp_DSEQ = -1;
                    m_ALL_DSEQ_IDX.Add(tmp_DSEQ);

                    if (m_dt.Rows[i]["DISPLAY_FLAG"].ToString() == "N" && m_HiddenPointVisible == false) continue;
                    m_VAL_VISIBLE.Add(m_dt.Rows[i]["DISPLAY_FLAG"].ToString() != "N");
                    m_VAL_DSEQ.Add(tmp_DSEQ);
                    m_VAL_RESULT.Add(m_dt.Rows[i]["RESULT"].ToString());
                    //m_VAL_AXIS_LABLE.Add(m_dt.Rows[i]["LOT_ID"].ToString());
                    m_iCNT++;

                    //XBAR
                    if (double.TryParse(m_dt.Rows[i]["EXT_MV"].ToString(), out tmp_XBAR) == false) tmp_XBAR = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["USL"].ToString(), out tmp_USL) == false) tmp_USL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["LSL"].ToString(), out tmp_LSL) == false) tmp_LSL = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_MV_UCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MV_UCL"].ToString(), out tmp_XBAR_UCL) == false) tmp_XBAR_UCL = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_MV_LCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MV_LCL"].ToString(), out tmp_XBAR_LCL) == false) tmp_XBAR_LCL = double.NaN;

                    m_VAL_XBAR.Add(tmp_XBAR);
                    m_USL.Add(tmp_USL);
                    m_LSL.Add(tmp_LSL);
                    m_XBAR_UCL.Add(tmp_XBAR_UCL);
                    m_XBAR_LCL.Add(tmp_XBAR_LCL);
                    if (double.IsNaN(tmp_XBAR) == false)
                    {
                        m_dSUM += tmp_XBAR;
                        dSumOfSq += Math.Pow(tmp_XBAR, 2);
                    }

                    ///////////////////////
                    /// OOC
                    ///////////////////////
                    if (tmp_XBAR_UCL != double.NaN && tmp_XBAR > tmp_XBAR_UCL) m_iOOC++;
                    if (tmp_XBAR_LCL != double.NaN && tmp_XBAR < tmp_XBAR_LCL) m_iOOC++;

                    ///////////////////////
                    /// OOS
                    ///////////////////////
                    if (tmp_USL != double.NaN && tmp_XBAR > tmp_USL) m_iOOS++;
                    if (tmp_LSL != double.NaN && tmp_XBAR < tmp_LSL) m_iOOS++;

                    //SIGMA
                    if (m_dt.Columns.IndexOf("EXT_SIGMA") > -1 && double.TryParse(m_dt.Rows[i]["EXT_SIGMA"].ToString(), out tmp_SIGMA) == false) tmp_SIGMA = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_SIGMA_UCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_SIGMA_UCL"].ToString(), out tmp_SIGMA_UCL) == false) tmp_SIGMA_UCL = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_SIGMA_LCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_SIGMA_LCL"].ToString(), out tmp_SIGMA_LCL) == false) tmp_SIGMA_LCL = double.NaN;

                    m_VAL_SIGMA.Add(tmp_SIGMA);
                    m_SIGMA_UCL.Add(tmp_SIGMA_UCL);
                    m_SIGMA_LCL.Add(tmp_SIGMA_LCL);
                    if (double.IsNaN(tmp_SIGMA) == false)
                    {
                        m_dSIGMA_SUM += tmp_SIGMA;
                        dSigma_SumOfSq += Math.Pow(tmp_SIGMA, 2);
                    }

                    //RANGE
                    if (m_dt.Columns.IndexOf("EXT_MIN") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MIN"].ToString(), out dMin) == false) dMin = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_MAX") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MAX"].ToString(), out dMax) == false) dMax = double.NaN;

                    if (m_dt.Columns.IndexOf("EXT_RANGE_UCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_RANGE_UCL"].ToString(), out tmp_RANGE_UCL) == false) tmp_RANGE_UCL = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_RANGE_LCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_RANGE_LCL"].ToString(), out tmp_RANGE_LCL) == false) tmp_RANGE_LCL = double.NaN;

                    m_dRANGE_SUM += (dMax - dMin);
                    dRange_SumOfSq += Math.Pow((dMax - dMin), 2);
                    m_VAL_RANGE.Add(dMax - dMin);
                    m_RANGE_UCL.Add(tmp_RANGE_UCL);
                    m_RANGE_LCL.Add(tmp_RANGE_LCL);


                    //RAW 
                    rValues = new List<double>();
                    for (int ir = 0; ir < m_iSampleCount; ir++)
                    {
                        string fieldName = string.Format("VALUE_{0}", ir + 1);
                        if (m_dt.Columns.IndexOf(fieldName) < 0) continue;
                        if (double.TryParse(m_dt.Rows[i][fieldName].ToString(), out tmp) == false) tmp = double.NaN;
                        if (double.IsNaN(tmp)) continue;
                        rValues.Add(tmp);
                        dRaw_SumOfSq += Math.Pow(tmp, 2);
                        m_dRAW_SUM += tmp;


                        //2015-05-06-정병주 : IMR 관련 Chart 추가 
                        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                        m_VAL_I.Add(tmp);

                        m_IMR_USL.Add(tmp_USL);
                        m_IMR_LSL.Add(tmp_LSL);
                        m_IMR_UCL.Add(tmp_XBAR_UCL);
                        m_IMR_LCL.Add(tmp_XBAR_LCL);

                        m_VAL_RESULT_IMR.Add(m_dt.Rows[i]["RESULT"].ToString());

                        if (m_VAL_MR.Count == 0)
                            m_VAL_MR.Add(double.NaN);

                        if (m_VAL_I.Count > 1)
                        {
                            //MR Data Adding
                            double MRtemp = double.NaN;
                            MRtemp = Math.Abs(m_VAL_I[m_VAL_I.Count - 1] - m_VAL_I[m_VAL_I.Count - 2]);
                            m_VAL_MR.Add(MRtemp);
                        }

                        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
                    }
                    m_iRAW_CNT += rValues.Count;

                    m_VAL_RAW.Add(new DataPoint((double)(i + 1), rValues.ToArray()));

                    //EWMA_MV
                    if (m_dt.Columns.IndexOf("EXT_EWMA_M") > -1 && double.TryParse(m_dt.Rows[i]["EXT_EWMA_M"].ToString(), out tmp_EWMA_MV) == false) tmp_EWMA_MV = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_EWMA_MV_UCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_EWMA_MV_UCL"].ToString(), out tmp_EWMA_MV_UCL) == false) tmp_EWMA_MV_UCL = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_EWMA_MV_LCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_EWMA_MV_LCL"].ToString(), out tmp_EWMA_MV_LCL) == false) tmp_EWMA_MV_LCL = double.NaN;
                    m_VAL_EWMA_MV.Add(tmp_EWMA_MV);
                    m_EWMA_MV_UCL.Add(tmp_EWMA_MV_UCL);
                    m_EWMA_MV_LCL.Add(tmp_EWMA_MV_LCL);

                    //EWMA_R
                    if (m_dt.Columns.IndexOf("EXT_EWMA_R") > -1 && double.TryParse(m_dt.Rows[i]["EXT_EWMA_R"].ToString(), out tmp_EWMA_R) == false) tmp_EWMA_R = double.NaN;
                    if (m_dt.Columns.IndexOf("EWMA_R_UCL") > -1 && double.TryParse(m_dt.Rows[i]["EWMA_R_UCL"].ToString(), out tmp_EWMA_R_UCL) == false) tmp_EWMA_R_UCL = double.NaN;
                    if (m_dt.Columns.IndexOf("EWMA_R_LCL") > -1 && double.TryParse(m_dt.Rows[i]["EWMA_R_LCL"].ToString(), out tmp_EWMA_R_LCL) == false) tmp_EWMA_R_LCL = double.NaN;
                    m_VAL_EWMA_R.Add(tmp_EWMA_R);
                    m_EWMA_R_UCL.Add(tmp_EWMA_R_UCL);
                    m_EWMA_R_LCL.Add(tmp_EWMA_R_LCL);

                    //EWMA_S
                    if (m_dt.Columns.IndexOf("EXT_EWMA_S") > -1 && double.TryParse(m_dt.Rows[i]["EXT_EWMA_S"].ToString(), out tmp_EWMA_S) == false) tmp_EWMA_S = double.NaN;
                    if (m_dt.Columns.IndexOf("EWMA_S_UCL") > -1 && double.TryParse(m_dt.Rows[i]["EWMA_S_UCL"].ToString(), out tmp_EWMA_S_UCL) == false) tmp_EWMA_S_UCL = double.NaN;
                    if (m_dt.Columns.IndexOf("EWMA_S_LCL") > -1 && double.TryParse(m_dt.Rows[i]["EWMA_S_LCL"].ToString(), out tmp_EWMA_S_LCL) == false) tmp_EWMA_S_LCL = double.NaN;
                    m_VAL_EWMA_S.Add(tmp_EWMA_S);
                    m_EWMA_S_UCL.Add(tmp_EWMA_S_UCL);
                    m_EWMA_S_LCL.Add(tmp_EWMA_S_LCL);

                    //MA
                    if (m_dt.Columns.IndexOf("EXT_MA") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MA"].ToString(), out tmp_MA) == false) tmp_MA = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_MA_MV_UCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MA_MV_UCL"].ToString(), out tmp_MA_UCL) == false) tmp_MA_UCL = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_MA_MV_LCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MA_MV_LCL"].ToString(), out tmp_MA_LCL) == false) tmp_MA_LCL = double.NaN;
                    m_VAL_MA.Add(tmp_MA);
                    m_MA_UCL.Add(tmp_MA_UCL);
                    m_MA_LCL.Add(tmp_MA_LCL);

                    //MS
                    if (m_dt.Columns.IndexOf("EXT_MS") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MS"].ToString(), out tmp_MS) == false) tmp_MS = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_MS_MV_UCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MS_MV_UCL"].ToString(), out tmp_MS_UCL) == false) tmp_MS_UCL = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_MS_MV_LCL") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MS_MV_LCL"].ToString(), out tmp_MS_LCL) == false) tmp_MS_LCL = double.NaN;
                    m_VAL_MS.Add(tmp_MS);
                    m_MS_UCL.Add(tmp_MS_UCL);
                    m_MS_LCL.Add(tmp_MS_LCL);
                }


                if (m_USL.Count > 0)
                {
                    double[] tmpArrUSL = m_USL.ToArray();
                    Array.Sort(tmpArrUSL);
                    m_MaxUSL = tmpArrUSL[tmpArrUSL.Length - 1];
                }

                if (m_LSL.Count > 0)
                {
                    double[] tmpArrLSL = m_LSL.ToArray();
                    Array.Sort(tmpArrLSL);
                    m_MinLSL = tmpArrLSL[0];
                }


                if (m_XBAR_UCL.Count > 0)
                {
                    double[] tmpArrUCL = m_XBAR_UCL.ToArray();
                    Array.Sort(tmpArrUCL);
                    m_MaxUCL = tmpArrUCL[tmpArrUCL.Length - 1];
                }

                if (m_XBAR_LCL.Count > 0)
                {
                    double[] tmpArrLCL = m_XBAR_LCL.ToArray();
                    Array.Sort(tmpArrLCL);
                    m_MinLCL = tmpArrLCL[0];
                }

                if (m_RANGE_UCL.Count > 0)
                {
                    double[] tmpArrUCL = m_RANGE_UCL.ToArray();
                    Array.Sort(tmpArrUCL);
                    m_Max_RANGE_UCL = tmpArrUCL[tmpArrUCL.Length - 1];
                }

                if (m_RANGE_LCL.Count > 0)
                {
                    double[] tmpArrLCL = m_RANGE_LCL.ToArray();
                    Array.Sort(tmpArrLCL);
                    m_Min_RANGE_LCL = tmpArrLCL[0];
                }

                if (m_SIGMA_UCL.Count > 0)
                {
                    double[] tmpArrUCL = m_SIGMA_UCL.ToArray();
                    Array.Sort(tmpArrUCL);
                    m_Max_SIGMA_UCL = tmpArrUCL[tmpArrUCL.Length - 1];
                }

                if (m_SIGMA_LCL.Count > 0)
                {
                    double[] tmpArrLCL = m_SIGMA_LCL.ToArray();
                    Array.Sort(tmpArrLCL);
                    m_Min_SIGMA_LCL = tmpArrLCL[0];
                }

                /// 1. XBAR 통계
                ///////////////////////////////////////////////////////////
                ///// 1.1 평균
                m_dAVG = m_dSUM / m_iCNT;
                ///// 1.2 분산
                m_dVAR = (dSumOfSq / m_iCNT) - Math.Pow(m_dAVG, 2);
                ///// 1.3 표준편차
                m_dSTD = Math.Sqrt(m_dVAR);
                ///// 1.4 3Sigma Line을 위해 계산
                for (int i = 0; i < m_iCNT; i++)
                {
                    m_XBAR_3S_UCL.Add(m_dAVG + 3 * m_dSTD);
                    m_XBAR_3S_LCL.Add(m_dAVG - 3 * m_dSTD);
                }
                ///// 1.5 XBAR에 대한 통계량으로 XBAR Chart 상하 크기 계산
                m_XBAR_CHART.SPCAxisY.Maximum = Math.Max(
                                                Math.Max(double.IsNaN(m_MaxUSL) ? double.MinValue : m_MaxUSL
                                                       , double.IsNaN(m_MaxUCL) ? double.MinValue : m_MaxUCL)
                                              , m_dAVG + 3 * m_dSTD
                                              ) + 3 * m_dSTD;
                m_XBAR_CHART.SPCAxisY.Minimum = Math.Min(
                                                Math.Min(double.IsNaN(m_MinLSL) ? double.MaxValue : m_MinLSL
                                                        , double.IsNaN(m_MinLCL) ? double.MaxValue : m_MinLCL)
                                                , m_dAVG - 3 * m_dSTD
                                                ) - 3 * m_dSTD;

                ///2. RAW 통계
                /////////////////////////////////////////////////////////
                /////2.1. 평균
                m_dRAW_AVG = m_dRAW_SUM / (double)m_iRAW_CNT;
                /////2.2. 분산(카이제곱 나누기 갯수에서 평균의 제곱을 빼기..쓰블)
                m_dRAW_VAR = (dRaw_SumOfSq / (double)m_iRAW_CNT) - Math.Pow(m_dRAW_AVG, 2);
                /////2.3. 표준편차
                m_dRAW_STD = Math.Sqrt(m_dRAW_VAR);
                /////3.4 RAW에 대한 통계량으로 XBAR Chart 상하 크기 계산
                m_RAW_CHART.SPCAxisY.Maximum = m_XBAR_CHART.SPCAxisY.Maximum;
                m_RAW_CHART.SPCAxisY.Minimum = m_XBAR_CHART.SPCAxisY.Minimum;

                ///3. RANGE 통계
                ///////////////////////////////////////////////////////////
                /////3.1. 평균
                m_dRANGE_AVG = m_dRANGE_SUM / (double)m_iCNT;
                /////3.2. 분산(카이제곱 나누기 갯수에서 평균의 제곱을 빼기..쓰블)
                m_dRANGE_VAR = (dRange_SumOfSq / (double)m_iCNT) - Math.Pow(m_dRANGE_AVG, 2);
                /////3.3. 표준편차
                m_dRANGE_STD = Math.Sqrt(m_dRANGE_VAR);
                /////3.4 XBAR에 대한 통계량으로 XBAR Chart 상하 크기 계산
                m_RANGE_CHART.SPCAxisY.Maximum = Math.Max(double.IsNaN(m_Max_RANGE_UCL) ? double.MinValue : m_Max_RANGE_UCL
                                       , m_dRANGE_AVG + 3 * m_dRANGE_STD) + 3 * m_dRANGE_STD;
                m_RANGE_CHART.SPCAxisY.Minimum = Math.Min(double.IsNaN(m_Min_RANGE_LCL) ? double.MaxValue : m_Min_RANGE_LCL
                                       , m_dRANGE_AVG - 3 * m_dRANGE_STD) - 3 * m_dRANGE_STD;

                ///4. SIGMA 통계
                ////////////////////////////////////////////////////////////
                /////4.1. 평균
                m_dSIGMA_AVG = m_dSIGMA_SUM / (double)m_iCNT;
                /////3.2. 분산(카이제곱 나누기 갯수에서 평균의 제곱을 빼기..쓰블)
                m_dSIGMA_VAR = (dSigma_SumOfSq / (double)m_iCNT) - Math.Pow(m_dSIGMA_AVG, 2);
                /////3.3. 표준편차
                m_dSIGMA_STD = Math.Sqrt(m_dSIGMA_VAR);
                /////3.4 SIGMA에 대한 통계량으로 SIGMA Chart 상하 크기 계산
                m_SIGMA_CHART.SPCAxisY.Maximum = Math.Max(double.IsNaN(m_Max_SIGMA_UCL) ? double.MinValue : m_Max_SIGMA_UCL
                                       , m_dSIGMA_AVG + 3 * m_dSIGMA_STD) + 3 * m_dSIGMA_STD;
                m_SIGMA_CHART.SPCAxisY.Minimum = Math.Min(double.IsNaN(m_Min_SIGMA_LCL) ? double.MaxValue : m_Min_SIGMA_LCL
                                       , m_dSIGMA_AVG - 3 * m_dSIGMA_STD) - 3 * m_dSIGMA_STD;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CombinationDataCal(string CombinationColumn, string[] CombinationList)
        {
            if (m_dt == null) return;
            try
            {
                int iRowsCount = m_dt.Rows.Count;
                double tmp = double.NaN;
                double dMin = double.NaN;
                double dMax = double.NaN;
                //1. Data Reset
                List<double> rValues = null;

                m_CMB_VAL_XBAR = new List<DataPoint>[CombinationList.Length + 1];
                m_CMB_VAL_SIGMA = new List<DataPoint>[CombinationList.Length + 1];
                m_CMB_VAL_RANGE = new List<DataPoint>[CombinationList.Length + 1];
                m_CMB_VAL_RAW = new List<DataPoint>[CombinationList.Length + 1];
                m_CMB_VAL_EWMA_MV = new List<DataPoint>[CombinationList.Length + 1];
                m_CMB_VAL_EWMA_S = new List<DataPoint>[CombinationList.Length + 1];
                m_CMB_VAL_EWMA_R = new List<DataPoint>[CombinationList.Length + 1];
                m_CMB_VAL_MA = new List<DataPoint>[CombinationList.Length + 1];
                m_CMB_VAL_MS = new List<DataPoint>[CombinationList.Length + 1];

                double tmp_XBAR = double.NaN;
                double tmp_SIGMA = double.NaN;
                //double tmp_RANGE = double.NaN;
                //double tmp_RAW = double.NaN;
                double tmp_EWMA_MV = double.NaN;
                double tmp_EWMA_S = double.NaN;
                double tmp_EWMA_R = double.NaN;
                double tmp_MA = double.NaN;
                double tmp_MS = double.NaN;

                //2. SPEC Reset
                //3. Control Reset
                /// XBAR
                /// 
                int IDX_VAL = 0;
                int iValueIdx = 0;
                string strCombination = string.Empty;
                string strAxisLabel = string.Empty;

                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    if (m_dt.Rows[i]["DISPLAY_FLAG"].ToString() == "N" && m_HiddenPointVisible == false) continue;

                    if (m_STR_LABEL_MEMBER == "TRAN_TIME")
                    {
                        if (m_dt.Columns[m_STR_LABEL_MEMBER].DataType == typeof(DateTime))
                        {
                            DateTime dtTime = (DateTime)m_dt.Rows[i][m_STR_LABEL_MEMBER];
                            strAxisLabel = dtTime.ToString(m_STR_LABEL_FORMAT);
                        }
                        else
                        {
                            strAxisLabel = m_dt.Rows[i][m_STR_LABEL_MEMBER].ToString();
                        }
                    }
                    else
                    {
                        strAxisLabel = m_dt.Rows[i][m_STR_LABEL_MEMBER].ToString();
                    }

                    strCombination = m_dt.Rows[i][CombinationColumn].ToString();
                    IDX_VAL = Array.IndexOf(CombinationList, strCombination);
                    if (IDX_VAL < 0) IDX_VAL = CombinationList.Length;
                    iValueIdx++; //1부터 시작

                    //XBAR
                    if (double.TryParse(m_dt.Rows[i]["EXT_MV"].ToString(), out tmp_XBAR) == false) tmp_XBAR = double.NaN;
                    if (m_CMB_VAL_XBAR[IDX_VAL] == null) m_CMB_VAL_XBAR[IDX_VAL] = new List<DataPoint>();
                    DataPoint tmpPointXBAR = new DataPoint(iValueIdx, tmp_XBAR);
                    tmpPointXBAR.AxisLabel = strAxisLabel;
                    m_CMB_VAL_XBAR[IDX_VAL].Add(tmpPointXBAR);


                    //SIGMA
                    if (m_dt.Columns.IndexOf("EXT_SIGMA") > -1 && double.TryParse(m_dt.Rows[i]["EXT_SIGMA"].ToString(), out tmp_SIGMA) == false) tmp_SIGMA = double.NaN;
                    if (m_CMB_VAL_SIGMA[IDX_VAL] == null) m_CMB_VAL_SIGMA[IDX_VAL] = new List<DataPoint>();
                    DataPoint tmpPointSIGMA = new DataPoint(iValueIdx, tmp_SIGMA);
                    tmpPointSIGMA.AxisLabel = strAxisLabel;
                    m_CMB_VAL_SIGMA[IDX_VAL].Add(tmpPointSIGMA);

                    //RANGE
                    if (m_dt.Columns.IndexOf("EXT_MIN") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MIN"].ToString(), out dMin) == false) dMin = double.NaN;
                    if (m_dt.Columns.IndexOf("EXT_MAX") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MAX"].ToString(), out dMax) == false) dMax = double.NaN;

                    if (m_CMB_VAL_RANGE[IDX_VAL] == null) m_CMB_VAL_RANGE[IDX_VAL] = new List<DataPoint>();
                    DataPoint tmpPointRANGE = new DataPoint(iValueIdx, dMax - dMin);
                    tmpPointRANGE.AxisLabel = strAxisLabel;
                    m_CMB_VAL_RANGE[IDX_VAL].Add(tmpPointRANGE);

                    //RAW 
                    rValues = new List<double>();
                    for (int ir = 0; ir < m_iSampleCount; ir++)
                    {
                        string fieldName = string.Format("VALUE_{0}", ir + 1);
                        if (m_dt.Columns.IndexOf(fieldName) < 0) continue;
                        if (double.TryParse(m_dt.Rows[i][fieldName].ToString(), out tmp) == false) tmp = double.NaN;
                        if (double.IsNaN(tmp)) continue;
                        rValues.Add(tmp);
                    }
                    m_iRAW_CNT += rValues.Count;
                    if (m_CMB_VAL_RAW[IDX_VAL] == null) m_CMB_VAL_RAW[IDX_VAL] = new List<DataPoint>();
                    DataPoint tmpPointRAW = new DataPoint(iValueIdx, rValues.ToArray());
                    tmpPointRAW.AxisLabel = strAxisLabel;
                    m_CMB_VAL_RAW[IDX_VAL].Add(tmpPointRAW);

                    //EWMA_MV
                    if (m_dt.Columns.IndexOf("EXT_EWMA_M") > -1 && double.TryParse(m_dt.Rows[i]["EXT_EWMA_M"].ToString(), out tmp_EWMA_MV) == false) tmp_EWMA_MV = double.NaN;
                    if (m_CMB_VAL_EWMA_MV[IDX_VAL] == null) m_CMB_VAL_EWMA_MV[IDX_VAL] = new List<DataPoint>();
                    DataPoint tmpPointEWMA_MV = new DataPoint(iValueIdx, tmp_EWMA_MV);
                    tmpPointEWMA_MV.AxisLabel = strAxisLabel;
                    m_CMB_VAL_EWMA_MV[IDX_VAL].Add(tmpPointEWMA_MV);

                    //EWMA_R
                    if (m_dt.Columns.IndexOf("EXT_EWMA_R") > -1 && double.TryParse(m_dt.Rows[i]["EXT_EWMA_R"].ToString(), out tmp_EWMA_R) == false) tmp_EWMA_R = double.NaN;
                    if (m_CMB_VAL_EWMA_R[IDX_VAL] == null) m_CMB_VAL_EWMA_R[IDX_VAL] = new List<DataPoint>();
                    DataPoint tmpPointEWMA_R = new DataPoint(iValueIdx, tmp_EWMA_R);
                    tmpPointEWMA_R.AxisLabel = strAxisLabel;
                    m_CMB_VAL_EWMA_R[IDX_VAL].Add(tmpPointEWMA_R);

                    //EWMA_S
                    if (m_dt.Columns.IndexOf("EXT_EWMA_S") > -1 && double.TryParse(m_dt.Rows[i]["EXT_EWMA_S"].ToString(), out tmp_EWMA_S) == false) tmp_EWMA_S = double.NaN;
                    if (m_CMB_VAL_EWMA_S[IDX_VAL] == null) m_CMB_VAL_EWMA_S[IDX_VAL] = new List<DataPoint>();
                    DataPoint tmpPointEWMA_S = new DataPoint(iValueIdx, tmp_EWMA_S);
                    tmpPointEWMA_S.AxisLabel = strAxisLabel;
                    m_CMB_VAL_EWMA_S[IDX_VAL].Add(tmpPointEWMA_S);

                    //MA
                    if (m_dt.Columns.IndexOf("EXT_MA") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MA"].ToString(), out tmp_MA) == false) tmp_MA = double.NaN;
                    if (m_CMB_VAL_MA[IDX_VAL] == null) m_CMB_VAL_MA[IDX_VAL] = new List<DataPoint>();
                    DataPoint tmpPointMA = new DataPoint(iValueIdx, tmp_MA);
                    tmpPointMA.AxisLabel = strAxisLabel;
                    m_CMB_VAL_MA[IDX_VAL].Add(tmpPointMA);

                    //MS
                    if (m_dt.Columns.IndexOf("EXT_MS") > -1 && double.TryParse(m_dt.Rows[i]["EXT_MS"].ToString(), out tmp_MS) == false) tmp_MS = double.NaN;
                    if (m_CMB_VAL_MS[IDX_VAL] == null) m_CMB_VAL_MS[IDX_VAL] = new List<DataPoint>();
                    DataPoint tmpPointMS = new DataPoint(iValueIdx, tmp_MS);
                    tmpPointMS.AxisLabel = strAxisLabel;
                    m_CMB_VAL_MS[IDX_VAL].Add(tmpPointMS);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Redraw Chart Function : 값을 다시 대입하고 다시 Chart를 그린다.
        private void RedrawXBAR(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(XBAR) < 0) chart1.ChartAreas.Add(XBAR);
                if (chart1.Series.IndexOf(XBAR) < 0) chart1.Series.Add(XBAR);
                chart1.Series[XBAR].ChartArea = XBAR;

                m_VAL_XBAR = new List<double>();
                m_USL = new List<double>();
                m_LSL = new List<double>();
                m_XBAR_UCL = new List<double>();
                m_XBAR_LCL = new List<double>();

                double tmp_XBAR = double.NaN;
                double tmp_USL = double.NaN;
                double tmp_LSL = double.NaN;
                double tmp_XBAR_UCL = double.NaN;
                double tmp_XBAR_LCL = double.NaN;

                double dSum = 0;
                double dSumOfSq = 0;
                double dAvg = 0;
                double dVar = 0;
                double dStd = 0;

                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    if (double.TryParse(m_dt.Rows[i]["EXT_MV"].ToString(), out tmp_XBAR) == false) tmp_XBAR = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["USL"].ToString(), out tmp_USL) == false) tmp_USL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["LSL"].ToString(), out tmp_LSL) == false) tmp_LSL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_MV_UCL"].ToString(), out tmp_XBAR_UCL) == false) tmp_XBAR_UCL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_MV_LCL"].ToString(), out tmp_XBAR_LCL) == false) tmp_XBAR_LCL = double.NaN;

                    if (double.IsNaN(tmp_XBAR) == false)
                    {
                        dSum += tmp_XBAR;
                        dSumOfSq += Math.Pow(tmp_XBAR, 2);
                    }

                    m_VAL_XBAR.Add(tmp_XBAR);
                    m_USL.Add(tmp_USL);
                    m_LSL.Add(tmp_LSL);
                    m_XBAR_UCL.Add(tmp_XBAR_UCL);
                    m_XBAR_LCL.Add(tmp_XBAR_LCL);
                }

                dAvg = dSum / m_dt.Rows.Count;
                dVar = (dSumOfSq / m_dt.Rows.Count) - Math.Pow(dAvg, 2);
                dStd = Math.Sqrt(dVar);


                chart1.Series[XBAR].Points.DataBindY(m_VAL_XBAR);
                chart1.Series[XBAR].ChartType = SeriesChartType.Line;

                //3.1 Marker
                chart1.Series[XBAR].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[XBAR].MarkerBorderColor = Color.Red;
                chart1.Series[XBAR].MarkerBorderWidth = 2;
                chart1.Series[XBAR].MarkerStep = 1;
                chart1.Series[XBAR].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[XBAR].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.

                if (pos == null)
                {
                    pos = new ElementPosition(0, 0, 100, 25);
                }
                chart1.ChartAreas[XBAR].Position = pos;

                if (inner_pos != null) chart1.ChartAreas[XBAR].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[XBAR].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[XBAR].AxisY.MajorGrid.Enabled = false;
                
                chart1.ChartAreas[XBAR].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[XBAR].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[XBAR].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[XBAR].AxisX.MinorGrid.Interval = 10;

                chart1.ChartAreas[XBAR].AxisY.IsStartedFromZero = false;
                chart1.ChartAreas[XBAR].AxisY.Maximum = dAvg + dStd * 12;
                chart1.ChartAreas[XBAR].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[XBAR].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                //chart1.ChartAreas[XBAR].AlignWithChartArea = XBAR; //첫번째것으로 통일

                chart1.ChartAreas[XBAR].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[XBAR].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[XBAR].Position.Auto = false;
                chart1.ChartAreas[XBAR].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일

                chart1.ChartAreas[XBAR].AlignmentStyle = AreaAlignmentStyles.All;
                chart1.ChartAreas[XBAR].AxisX.IsMarginVisible = m_bXAxisMargin;
                chart1.ChartAreas[XBAR].AxisX.IsStartedFromZero = m_bXAxisStartZero;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[XBAR].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RedrawSIGMA(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(SIGMA) < 0) chart1.ChartAreas.Add(SIGMA);
                if (chart1.Series.IndexOf(SIGMA) < 0) chart1.Series.Add(SIGMA);
                chart1.Series[SIGMA].ChartArea = SIGMA;


                m_VAL_SIGMA = new List<double>();
                m_SIGMA_UCL = new List<double>();
                m_SIGMA_LCL = new List<double>();

                double tmp_SIGMA = double.NaN;
                double tmp_SIGMA_UCL = double.NaN;
                double tmp_SIGMA_LCL = double.NaN;

                double dSum = 0;
                double dSumOfSq = 0;
                double dAvg = 0;
                double dVar = 0;
                double dStd = 0;

                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    if (double.TryParse(m_dt.Rows[i]["EXT_SIGMA"].ToString(), out tmp_SIGMA) == false) tmp_SIGMA = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_SIGMA_UCL"].ToString(), out tmp_SIGMA_UCL) == false) tmp_SIGMA_UCL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_SIGMA_LCL"].ToString(), out tmp_SIGMA_LCL) == false) tmp_SIGMA_LCL = double.NaN;

                    if (double.IsNaN(tmp_SIGMA) == false)
                    {
                        dSum += tmp_SIGMA;
                        dSumOfSq += Math.Pow(tmp_SIGMA, 2);
                    }
                    m_VAL_SIGMA.Add(tmp_SIGMA);
                    m_SIGMA_UCL.Add(tmp_SIGMA_UCL);
                    m_SIGMA_LCL.Add(tmp_SIGMA_LCL);
                }

                dAvg = dSum / m_dt.Rows.Count;
                dVar = (dSumOfSq / m_dt.Rows.Count) - Math.Pow(dAvg, 2);
                dStd = Math.Sqrt(dVar);

                chart1.Series[SIGMA].Points.DataBindY(m_VAL_SIGMA);
                chart1.Series[SIGMA].ChartType = SeriesChartType.Line;

                //3.1 Marker
                chart1.Series[SIGMA].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[SIGMA].MarkerBorderColor = Color.Red;
                chart1.Series[SIGMA].MarkerBorderWidth = 2;
                chart1.Series[SIGMA].MarkerStep = 1;
                chart1.Series[SIGMA].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[SIGMA].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.

                if (pos == null)
                {
                    pos = new ElementPosition(0, 25, 100, 25);
                }
                chart1.ChartAreas[SIGMA].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[SIGMA].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[SIGMA].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[SIGMA].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[SIGMA].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[SIGMA].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[SIGMA].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[SIGMA].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[SIGMA].AxisY.IsStartedFromZero = false;
                chart1.ChartAreas[SIGMA].AxisY.Maximum = dAvg + dStd * 12;
                chart1.ChartAreas[SIGMA].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[SIGMA].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[SIGMA].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[SIGMA].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[SIGMA].Position.Auto = false ;
                chart1.ChartAreas[SIGMA].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[SIGMA].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[SIGMA].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RedrawRANGE(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(RANGE) < 0) chart1.ChartAreas.Add(RANGE);
                if (chart1.Series.IndexOf(RANGE) < 0) chart1.Series.Add(RANGE);
                chart1.Series[RANGE].ChartArea = RANGE;

                m_VAL_RANGE = new List<double>();
                m_RANGE_UCL = new List<double>();
                m_RANGE_LCL = new List<double>();

                double tmp_RANGE = double.NaN;
                double tmp_RANGE_UCL = double.NaN;
                double tmp_RANGE_LCL = double.NaN;

                double dSum = 0;
                double dSumOfSq = 0;
                double dAvg = 0;
                double dVar = 0;
                double dStd = 0;

                double dMin = double.MinValue;
                double dMax = double.MinValue;
                for (int i = 0; i < m_dt.Rows.Count; i++)
                {

                    if (double.TryParse(m_dt.Rows[i]["EXT_MIN"].ToString(), out dMin) == false) dMin = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_MAX"].ToString(), out dMax) == false) dMax = double.NaN;
                    tmp_RANGE = dMax - dMin;
                    if (double.TryParse(m_dt.Rows[i]["EXT_RANGE_UCL"].ToString(), out tmp_RANGE_UCL) == false) tmp_RANGE_UCL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_RANGE_LCL"].ToString(), out tmp_RANGE_LCL) == false) tmp_RANGE_LCL = double.NaN;

                    if (double.IsNaN(tmp_RANGE) == false)
                    {
                        dSum += tmp_RANGE;
                        dSumOfSq += Math.Pow(tmp_RANGE, 2);
                    }
                    m_VAL_RANGE.Add(tmp_RANGE);
                    m_RANGE_UCL.Add(tmp_RANGE_UCL);
                    m_RANGE_LCL.Add(tmp_RANGE_LCL);
                }

                dAvg = dSum / m_dt.Rows.Count;
                dVar = (dSumOfSq / m_dt.Rows.Count) - Math.Pow(dAvg, 2);
                dStd = Math.Sqrt(dVar);

                chart1.Series[RANGE].Points.DataBindY(m_VAL_RANGE);
                chart1.Series[RANGE].ChartType = SeriesChartType.Line;

                //3.1 Marker
                chart1.Series[RANGE].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[RANGE].MarkerBorderColor = Color.Red;
                chart1.Series[RANGE].MarkerBorderWidth = 2;
                chart1.Series[RANGE].MarkerStep = 1;
                chart1.Series[RANGE].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[RANGE].Points[3].MarkerColor = Color.Blue;

                if (pos == null)
                {
                    pos = new ElementPosition(0, 50, 100, 25);
                }
                chart1.ChartAreas[RANGE].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[RANGE].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[RANGE].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[RANGE].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[RANGE].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[RANGE].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[RANGE].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[RANGE].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[RANGE].AxisY.IsStartedFromZero = false;
                chart1.ChartAreas[RANGE].AxisY.Maximum = dAvg + dStd * 12;
                chart1.ChartAreas[RANGE].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[RANGE].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[RANGE].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[RANGE].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[RANGE].Position.Auto = false;
                chart1.ChartAreas[RANGE].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[RANGE].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[RANGE].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RedrawRAW(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            string RAW_BOX = RAW + "_BOX";
            try
            {
                if (chart1.ChartAreas.IndexOf(RAW) < 0) chart1.ChartAreas.Add(RAW);
                if (chart1.Series.IndexOf(RAW) < 0) chart1.Series.Add(RAW);
                if (chart1.Series.IndexOf(RAW_BOX) < 0) chart1.Series.Add(RAW_BOX);

                chart1.Series[RAW].ChartArea = RAW;
                chart1.Series[RAW_BOX].ChartArea = RAW;


                m_VAL_RAW = new List<DataPoint>();
                //m_XBAR_UCL = new List<double>();
                //m_XBAR_LCL = new List<double>();


                DataPoint tmp_RAW = null;
                //double tmp_XBAR_UCL = double.NaN;
                //double tmp_XBAR_LCL = double.NaN;

                double dSum = 0;
                double dSumOfSq = 0;
                double dAvg = 0;
                double dVar = 0;
                double dStd = 0;
                double tmp = double.NaN;

                int iValidCnt = 0;

                List<double> yValues = null;
                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    yValues = new List<double>();
                    for(int ir = 0;ir<10;ir++)
                    {
                        string fieldName = string.Format("VALUE_{0}",ir+1);
                        if (m_dt.Columns.IndexOf(fieldName) < 0) continue;
                        if (double.TryParse(m_dt.Rows[i][fieldName].ToString(),out tmp) == false) tmp = double.NaN;
                        if(double.IsNaN(tmp)) continue;
                        yValues.Add(tmp);
                        dSum += tmp;
                        dSumOfSq += Math.Pow(tmp, 2);
                        iValidCnt++;
                    }
                    tmp_RAW = new DataPoint((double)(i + 1), yValues.ToArray());
                    m_VAL_RAW.Add(tmp_RAW);
                    chart1.Series[RAW].Points.Add(m_VAL_RAW[i]);

                    // RAW는 XBAR의 SPEC, CONTROL을 같이 사용

                    //if (double.TryParse(m_dt.Rows[i]["EXT_RAW_UCL"].ToString(), out tmp_XBAR_UCL) == false) tmp_XBAR_UCL = double.NaN;
                    //if (double.TryParse(m_dt.Rows[i]["EXT_RAW_LCL"].ToString(), out tmp_XBAR_LCL) == false) tmp_XBAR_LCL = double.NaN;
                    //double dXBar = (double)m_dt.Rows[i]["EXT_MV"];

                }

                chart1.Series[RAW].Points.AddY(m_VAL_RAW);

                dAvg = dSum / iValidCnt;
                dVar = (dSumOfSq / iValidCnt) - Math.Pow(dAvg, 2);
                dStd = Math.Sqrt(dVar);

                chart1.Series[RAW].ChartType = SeriesChartType.Point;
                chart1.Series[RAW_BOX].ChartType = SeriesChartType.BoxPlot;
                chart1.DataManipulator.CopySeriesValues(RAW, RAW_BOX);
                

                //3.1 Marker
                chart1.Series[RAW].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[RAW].MarkerBorderColor = Color.Red;
                chart1.Series[RAW].MarkerBorderWidth = 1;
                //chart1.Series[RAW].MarkerStep = 1;
                chart1.Series[RAW].MarkerSize = 2;
                //3.2 Marker Point
                //chart1.Series[RAW].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.
                if (pos == null)
                {
                    pos = new ElementPosition(0, 75, 100, 25);
                }
                chart1.ChartAreas[RAW].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[RAW].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[RAW].AxisY.LabelStyle.Format = "{#.#00}";


                ////4. Grid
                chart1.ChartAreas[RAW].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[RAW].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[RAW].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[RAW].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[RAW].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[RAW].AxisY.IsStartedFromZero = false;
                chart1.ChartAreas[RAW].AxisY.Maximum = dAvg + dStd * 6;
                chart1.ChartAreas[RAW].AxisY.Minimum = dAvg - dStd * 6;
                chart1.Series[RAW].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[RAW].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[RAW].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[RAW].Position.Auto = false;
                chart1.ChartAreas[RAW].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[RAW].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[RAW].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RedrawEWMA_MV(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(EWMA_MV) < 0) chart1.ChartAreas.Add(EWMA_MV);
                if (chart1.Series.IndexOf(EWMA_MV) < 0) chart1.Series.Add(EWMA_MV);

                chart1.Series[EWMA_MV].ChartArea = EWMA_MV;

                m_VAL_EWMA_MV = new List<double>();
                m_EWMA_MV_UCL = new List<double>();
                m_EWMA_MV_LCL = new List<double>();

                double tmp_EWMA_MV = double.NaN;
                double tmp_EWMA_MV_UCL = double.NaN;
                double tmp_EWMA_MV_LCL = double.NaN;

                double dSum = 0;
                double dSumOfSq = 0;
                double dAvg = 0;
                double dVar = 0;
                double dStd = 0;

                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    if (double.TryParse(m_dt.Rows[i]["EXT_EWMA_M"].ToString(), out tmp_EWMA_MV) == false) tmp_EWMA_MV = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_EWMA_MV_UCL"].ToString(), out tmp_EWMA_MV_UCL) == false) tmp_EWMA_MV_UCL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_EWMA_MV_LCL"].ToString(), out tmp_EWMA_MV_LCL) == false) tmp_EWMA_MV_LCL = double.NaN;

                    if (double.IsNaN(tmp_EWMA_MV) == false)
                    {
                        dSum += tmp_EWMA_MV;
                        dSumOfSq += Math.Pow(tmp_EWMA_MV, 2);
                    }
                    m_VAL_EWMA_MV.Add(tmp_EWMA_MV);
                    m_EWMA_MV_UCL.Add(tmp_EWMA_MV_UCL);
                    m_EWMA_MV_LCL.Add(tmp_EWMA_MV_LCL);
                }

                dAvg = dSum / m_dt.Rows.Count;
                dVar = (dSumOfSq / m_dt.Rows.Count) - Math.Pow(dAvg, 2);
                dStd = Math.Sqrt(dVar);

                chart1.Series[EWMA_MV].Points.DataBindY(m_VAL_EWMA_MV);
                chart1.Series[EWMA_MV].ChartType = SeriesChartType.Line;

                //3.1 Marker
                chart1.Series[EWMA_MV].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[EWMA_MV].MarkerBorderColor = Color.Red;
                chart1.Series[EWMA_MV].MarkerBorderWidth = 2;
                chart1.Series[EWMA_MV].MarkerStep = 1;
                chart1.Series[EWMA_MV].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[EWMA_MV].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.

                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[EWMA_MV].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[EWMA_MV].InnerPlotPosition = inner_pos;

                chart1.ChartAreas[EWMA_MV].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[EWMA_MV].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[EWMA_MV].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_MV].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[EWMA_MV].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_MV].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[EWMA_MV].AxisY.IsStartedFromZero = false;
                chart1.ChartAreas[EWMA_MV].AxisY.Maximum = dAvg + dStd * 12;
                chart1.ChartAreas[EWMA_MV].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[EWMA_MV].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[EWMA_MV].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[EWMA_MV].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[EWMA_MV].Position.Auto = false;
                chart1.ChartAreas[EWMA_MV].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[EWMA_MV].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[EWMA_MV].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RedrawEWMA_R(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(EWMA_R) < 0) chart1.ChartAreas.Add(EWMA_MV);
                if (chart1.Series.IndexOf(EWMA_R) < 0) chart1.Series.Add(EWMA_MV);

                chart1.Series[EWMA_R].ChartArea = EWMA_R;

                m_VAL_EWMA_R = new List<double>();
                m_EWMA_R_UCL = new List<double>();
                m_EWMA_R_LCL = new List<double>();

                double tmp_EWMA_R = double.NaN;
                double tmp_EWMA_R_UCL = double.NaN;
                double tmp_EWMA_R_LCL = double.NaN;

                double dSum = 0;
                double dSumOfSq = 0;
                double dAvg = 0;
                double dVar = 0;
                double dStd = 0;

                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    if (double.TryParse(m_dt.Rows[i]["EXT_EWMA_R"].ToString(), out tmp_EWMA_R) == false) tmp_EWMA_R = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EWMA_R_UCL"].ToString(), out tmp_EWMA_R_UCL) == false) tmp_EWMA_R_UCL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EWMA_R_LCL"].ToString(), out tmp_EWMA_R_LCL) == false) tmp_EWMA_R_LCL = double.NaN;

                    if (double.IsNaN(tmp_EWMA_R) == false)
                    {
                        dSum += tmp_EWMA_R;
                        dSumOfSq += Math.Pow(tmp_EWMA_R, 2);
                    }
                    m_VAL_EWMA_R.Add(tmp_EWMA_R);
                    m_EWMA_R_UCL.Add(tmp_EWMA_R_UCL);
                    m_EWMA_R_LCL.Add(tmp_EWMA_R_LCL);
                }

                dAvg = dSum / m_dt.Rows.Count;
                dVar = (dSumOfSq / m_dt.Rows.Count) - Math.Pow(dAvg, 2);
                dStd = Math.Sqrt(dVar);

                chart1.Series[EWMA_R].Points.DataBindY(m_VAL_EWMA_R);
                chart1.Series[EWMA_R].ChartType = SeriesChartType.Line;

                //3.1 Marker
                chart1.Series[EWMA_R].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[EWMA_R].MarkerBorderColor = Color.Red;
                chart1.Series[EWMA_R].MarkerBorderWidth = 2;
                chart1.Series[EWMA_R].MarkerStep = 1;
                chart1.Series[EWMA_R].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[EWMA_R].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.

                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[EWMA_R].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[EWMA_R].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[EWMA_R].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[EWMA_R].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[EWMA_R].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_R].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[EWMA_R].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_R].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[EWMA_R].AxisY.IsStartedFromZero = false;
                chart1.ChartAreas[EWMA_R].AxisY.Maximum = dAvg + dStd * 12;
                chart1.ChartAreas[EWMA_R].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[EWMA_R].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[EWMA_R].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[EWMA_R].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[EWMA_R].Position.Auto = false;
                chart1.ChartAreas[EWMA_R].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[EWMA_R].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[EWMA_R].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RedrawEWMA_S(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(EWMA_S) < 0) chart1.ChartAreas.Add(EWMA_S);
                if (chart1.Series.IndexOf(EWMA_S) < 0) chart1.Series.Add(EWMA_S);
                chart1.Series[EWMA_S].ChartArea = EWMA_S;
                
                m_VAL_EWMA_S = new List<double>();
                m_EWMA_S_UCL = new List<double>();
                m_EWMA_S_LCL = new List<double>();

                double tmp_EWMA_S = double.NaN;
                double tmp_EWMA_S_UCL = double.NaN;
                double tmp_EWMA_S_LCL = double.NaN;

                double dSum = 0;
                double dSumOfSq = 0;
                double dAvg = 0;
                double dVar = 0;
                double dStd = 0;

                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    if (double.TryParse(m_dt.Rows[i]["EXT_EWMA_S"].ToString(), out tmp_EWMA_S) == false) tmp_EWMA_S = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EWMA_S_UCL"].ToString(), out tmp_EWMA_S_UCL) == false) tmp_EWMA_S_UCL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EWMA_S_LCL"].ToString(), out tmp_EWMA_S_LCL) == false) tmp_EWMA_S_LCL = double.NaN;

                    if (double.IsNaN(tmp_EWMA_S) == false)
                    {
                        dSum += tmp_EWMA_S;
                        dSumOfSq += Math.Pow(tmp_EWMA_S, 2);
                    }
                    m_VAL_EWMA_S.Add(tmp_EWMA_S);
                    m_EWMA_S_UCL.Add(tmp_EWMA_S_UCL);
                    m_EWMA_S_LCL.Add(tmp_EWMA_S_LCL);
                }

                dAvg = dSum / m_dt.Rows.Count;
                dVar = (dSumOfSq / m_dt.Rows.Count) - Math.Pow(dAvg, 2);
                dStd = Math.Sqrt(dVar);

                chart1.Series[EWMA_S].Points.DataBindY(m_VAL_EWMA_S);
                chart1.Series[EWMA_S].ChartType = SeriesChartType.Line;

                //3.1 Marker
                chart1.Series[EWMA_S].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[EWMA_S].MarkerBorderColor = Color.Red;
                chart1.Series[EWMA_S].MarkerBorderWidth = 2;
                chart1.Series[EWMA_S].MarkerStep = 1;
                chart1.Series[EWMA_S].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[EWMA_S].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.

                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[EWMA_S].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[EWMA_S].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[EWMA_S].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[EWMA_S].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[EWMA_S].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_S].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[EWMA_S].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_S].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[EWMA_S].AxisY.IsStartedFromZero = false;
                chart1.ChartAreas[EWMA_S].AxisY.Maximum = dAvg + dStd * 12;
                chart1.ChartAreas[EWMA_S].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[EWMA_S].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[EWMA_S].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[EWMA_S].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[EWMA_S].Position.Auto = false;
                chart1.ChartAreas[EWMA_S].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[EWMA_S].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[EWMA_S].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RedrawMA(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(MA) < 0) chart1.ChartAreas.Add(MA);
                if (chart1.Series.IndexOf(MA) < 0) chart1.Series.Add(MA);
                chart1.Series[MA].ChartArea = MA;


                m_VAL_MA = new List<double>();
                m_MA_UCL = new List<double>();
                m_MA_LCL = new List<double>();

                double tmp_MA = double.NaN;
                double tmp_MA_UCL = double.NaN;
                double tmp_MA_LCL = double.NaN;

                double dSum = 0;
                double dSumOfSq = 0;
                double dAvg = 0;
                double dVar = 0;
                double dStd = 0;

                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    if (double.TryParse(m_dt.Rows[i]["EXT_MA"].ToString(), out tmp_MA) == false) tmp_MA = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_MA_MV_UCL"].ToString(), out tmp_MA_UCL) == false) tmp_MA_UCL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_MA_MV_LCL"].ToString(), out tmp_MA_LCL) == false) tmp_MA_LCL = double.NaN;

                    if (double.IsNaN(tmp_MA) == false)
                    {
                        dSum += tmp_MA;
                        dSumOfSq += Math.Pow(tmp_MA, 2);
                    }
                    m_VAL_MA.Add(tmp_MA);
                    m_MA_UCL.Add(tmp_MA_UCL);
                    m_MA_LCL.Add(tmp_MA_LCL);
                }

                dAvg = dSum / m_dt.Rows.Count;
                dVar = (dSumOfSq / m_dt.Rows.Count) - Math.Pow(dAvg, 2);
                dStd = Math.Sqrt(dVar);

                chart1.Series[MA].Points.DataBindY(m_VAL_MA);
                chart1.Series[MA].ChartType = SeriesChartType.Line;

                //3.1 Marker
                chart1.Series[MA].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[MA].MarkerBorderColor = Color.Red;
                chart1.Series[MA].MarkerBorderWidth = 2;
                chart1.Series[MA].MarkerStep = 1;
                chart1.Series[MA].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[MA].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.
                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[MA].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[MA].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[MA].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[MA].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[MA].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[MA].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[MA].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[MA].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[MA].AxisY.IsStartedFromZero = false;
                chart1.ChartAreas[MA].AxisY.Maximum = dAvg + dStd * 12;
                chart1.ChartAreas[MA].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[MA].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[MA].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[MA].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[MA].Position.Auto = false;
                chart1.ChartAreas[MA].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[MA].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[MA].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RedrawMS(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(MS) < 0) chart1.ChartAreas.Add(MS);
                if (chart1.Series.IndexOf(MS) < 0) chart1.Series.Add(MS);
                chart1.Series[MS].ChartArea = MS;


                m_VAL_MS = new List<double>();
                m_MS_UCL = new List<double>();
                m_MS_LCL = new List<double>();

                double tmp_MS = double.NaN;
                double tmp_MS_UCL = double.NaN;
                double tmp_MS_LCL = double.NaN;

                double dSum = 0;
                double dSumOfSq = 0;
                double dAvg = 0;
                double dVar = 0;
                double dStd = 0;

                for (int i = 0; i < m_dt.Rows.Count; i++)
                {
                    if (double.TryParse(m_dt.Rows[i]["EXT_MS"].ToString(), out tmp_MS) == false) tmp_MS = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_MS_MV_UCL"].ToString(), out tmp_MS_UCL) == false) tmp_MS_UCL = double.NaN;
                    if (double.TryParse(m_dt.Rows[i]["EXT_MS_MV_LCL"].ToString(), out tmp_MS_LCL) == false) tmp_MS_LCL = double.NaN;

                    if (double.IsNaN(tmp_MS) == false)
                    {
                        dSum += tmp_MS;
                        dSumOfSq += Math.Pow(tmp_MS, 2);
                    }

                    m_VAL_MS.Add(tmp_MS);
                    m_MS_UCL.Add(tmp_MS_UCL);
                    m_MS_LCL.Add(tmp_MS_LCL);
                }

                dAvg = dSum / m_dt.Rows.Count;
                dVar = (dSumOfSq / m_dt.Rows.Count) - Math.Pow(dAvg, 2);
                dStd = Math.Sqrt(dVar);

                chart1.Series[MS].Points.DataBindY(m_VAL_MS);
                chart1.Series[MS].ChartType = SeriesChartType.Line;

                //3.1 Marker
                chart1.Series[MS].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[MS].MarkerBorderColor = Color.Red;
                chart1.Series[MS].MarkerBorderWidth = 2;
                chart1.Series[MS].MarkerStep = 1;
                chart1.Series[MS].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[MS].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.
                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[MS].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[MS].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[MS].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[MS].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[MS].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[MS].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[MS].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[MS].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[MS].AxisY.IsStartedFromZero = false;
                chart1.ChartAreas[MS].AxisY.Maximum = dAvg + dStd * 12;
                chart1.ChartAreas[MS].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[MS].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[MS].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[MS].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[MS].Position.Auto = false;
                chart1.ChartAreas[MS].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[MS].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[MS].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void DrawXBAR(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(XBAR)<0) chart1.ChartAreas.Add(XBAR);
                if (chart1.Series.IndexOf(XBAR) < 0) chart1.Series.Add(XBAR);
                if (chart1.Titles.IndexOf(XBAR) < 0) chart1.Titles.Add(XBAR).Name = XBAR;

                /// 1. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 1.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 0, 100, 25);
                chart1.ChartAreas[XBAR].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[XBAR].InnerPlotPosition = inner_pos;

                /// 1.2 Chart영역의 속성처리
                chart1.ChartAreas[XBAR].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[XBAR].Position.Auto = false;

                /// 1.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[XBAR].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[XBAR].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[XBAR].AlignmentStyle = AreaAlignmentStyles.All;

                /// 1.4 Asix X 축 설정
                chart1.ChartAreas[XBAR].AxisX = m_XBAR_CHART.SPCAxisX;

                /// 1.5 Asix Y 축 설정
                chart1.ChartAreas[XBAR].AxisY = m_XBAR_CHART.SPCAxisY;

                chart1.ChartAreas[XBAR].AxisY.LabelStyle.Format = m_XBAR_CHART.SPCAxisY.LabelStyle.Format;
                chart1.ChartAreas[XBAR].AxisY.MajorGrid.Enabled = m_XBAR_CHART.SPCAxisY.MajorGrid.Enabled;
                chart1.ChartAreas[XBAR].AxisY.MajorGrid.Interval = m_XBAR_CHART.SPCAxisY.MajorGrid.Interval;
                chart1.ChartAreas[XBAR].AxisY.MajorGrid.LineColor = m_XBAR_CHART.SPCAxisY.MajorGrid.LineColor;
                chart1.ChartAreas[XBAR].AxisY.MajorGrid.LineDashStyle = m_XBAR_CHART.SPCAxisY.MajorGrid.LineDashStyle;

                chart1.ChartAreas[XBAR].AxisY.MinorGrid.Enabled = m_XBAR_CHART.SPCAxisY.MinorGrid.Enabled;
                chart1.ChartAreas[XBAR].AxisY.MinorGrid.Interval = m_XBAR_CHART.SPCAxisY.MajorGrid.Interval;
                chart1.ChartAreas[XBAR].AxisY.MinorGrid.LineColor = m_XBAR_CHART.SPCAxisY.MajorGrid.LineColor;
                chart1.ChartAreas[XBAR].AxisY.MinorGrid.LineDashStyle = m_XBAR_CHART.SPCAxisY.MajorGrid.LineDashStyle;
                chart1.ChartAreas[XBAR].AxisY.IsStartedFromZero = m_XBAR_CHART.SPCAxisY.IsStartedFromZero;

                /// 1.6 Set cursor properties 
                chart1.ChartAreas[XBAR].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[XBAR].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[XBAR].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[XBAR].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[XBAR].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Series[XBAR].ChartArea = XBAR;
                chart1.Series[XBAR].Points.DataBindY(m_VAL_XBAR);
                chart1.Series[XBAR].ChartType = SeriesChartType.Line;
                chart1.Series[XBAR].Color = m_XBAR_CHART.TrendColor;

                /// 3. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;
                chart1.Series[XBAR].IsVisibleInLegend = false;

                /// 4. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[XBAR] = m_XBAR_CHART.SPCTitle;

                /// 5 Marker
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Series[XBAR].MarkerBorderColor = m_XBAR_CHART.SPCMarker.MarkerBorderColor;
                chart1.Series[XBAR].MarkerStyle = m_XBAR_CHART.SPCMarker.MarkerStyle;
                chart1.Series[XBAR].MarkerBorderColor = m_XBAR_CHART.SPCMarker.MarkerBorderColor;
                chart1.Series[XBAR].MarkerBorderWidth = m_XBAR_CHART.SPCMarker.MarkerBorderWidth;
                chart1.Series[XBAR].MarkerSize = m_XBAR_CHART.SPCMarker.MarkerSize;
                chart1.Series[XBAR].MarkerColor = m_XBAR_CHART.SPCMarker.MarkerColor;
                chart1.Series[XBAR].MarkerStep = 1;

                chart1.ChartAreas[XBAR].AxisX.Interval = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawSIGMA(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(SIGMA) < 0) chart1.ChartAreas.Add(SIGMA);
                if (chart1.Series.IndexOf(SIGMA) < 0) chart1.Series.Add(SIGMA);
                if (chart1.Titles.IndexOf(SIGMA) < 0) chart1.Titles.Add(SIGMA).Name = SIGMA;


                /// 1. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 1.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 25, 100, 25);
                chart1.ChartAreas[SIGMA].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[SIGMA].InnerPlotPosition = inner_pos;

                /// 1.2 Chart영역의 속성처리
                chart1.ChartAreas[SIGMA].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[SIGMA].Position.Auto = false;

                /// 1.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[SIGMA].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[SIGMA].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[SIGMA].AlignmentStyle = AreaAlignmentStyles.All;

                /// 1.4 Asix X 축 설정
                chart1.ChartAreas[SIGMA].AxisX = m_SIGMA_CHART.SPCAxisX;

                /// 1.5 Asix Y 축 설정
                chart1.ChartAreas[SIGMA].AxisY = m_SIGMA_CHART.SPCAxisY;

                /// 1.6 Set cursor properties 
                chart1.ChartAreas[SIGMA].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[SIGMA].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[SIGMA].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[SIGMA].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[SIGMA].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Series[SIGMA].ChartArea = SIGMA;
                chart1.Series[SIGMA].Points.DataBindY(m_VAL_SIGMA);
                chart1.Series[SIGMA].ChartType = SeriesChartType.Line;
                chart1.Series[SIGMA].Color = m_SIGMA_CHART.TrendColor;

                /// 3. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;
                chart1.Series[SIGMA].IsVisibleInLegend = false;

                /// 4. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[SIGMA] = m_SIGMA_CHART.SPCTitle;

                /// 5 Marker
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Series[SIGMA].MarkerBorderColor = m_SIGMA_CHART.SPCMarker.MarkerBorderColor;
                chart1.Series[SIGMA].MarkerStyle = m_SIGMA_CHART.SPCMarker.MarkerStyle;
                chart1.Series[SIGMA].MarkerBorderColor = m_SIGMA_CHART.SPCMarker.MarkerBorderColor;
                chart1.Series[SIGMA].MarkerBorderWidth = m_SIGMA_CHART.SPCMarker.MarkerBorderWidth;
                chart1.Series[SIGMA].MarkerSize = m_SIGMA_CHART.SPCMarker.MarkerSize;
                chart1.Series[SIGMA].MarkerColor = m_SIGMA_CHART.SPCMarker.MarkerColor;
                chart1.Series[SIGMA].MarkerStep = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawRANGE(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(RANGE) < 0) chart1.ChartAreas.Add(RANGE);
                if (chart1.Series.IndexOf(RANGE) < 0) chart1.Series.Add(RANGE);
                if (chart1.Titles.IndexOf(RANGE) < 0) chart1.Titles.Add(RANGE).Name = RANGE;


                /// 1. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 1.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 50, 100, 25);
                chart1.ChartAreas[RANGE].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[RANGE].InnerPlotPosition = inner_pos;

                /// 1.2 Chart영역의 속성처리
                chart1.ChartAreas[RANGE].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[RANGE].Position.Auto = false;

                /// 1.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[RANGE].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[RANGE].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[RANGE].AlignmentStyle = AreaAlignmentStyles.All;

                /// 1.4 Asix X 축 설정
                chart1.ChartAreas[RANGE].AxisX = m_RANGE_CHART.SPCAxisX;

                /// 1.5 Asix Y 축 설정
                chart1.ChartAreas[RANGE].AxisY = m_RANGE_CHART.SPCAxisY;

                /// 1.6 Set cursor properties 
                chart1.ChartAreas[RANGE].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[RANGE].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[RANGE].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[RANGE].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[RANGE].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Series[RANGE].ChartArea = RANGE;
                chart1.Series[RANGE].Points.DataBindY(m_VAL_RANGE);
                chart1.Series[RANGE].ChartType = SeriesChartType.Line;
                chart1.Series[RANGE].Color = m_RANGE_CHART.TrendColor;

                /// 3. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;
                chart1.Series[RANGE].IsVisibleInLegend = false;

                /// 4. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[RANGE] = m_RANGE_CHART.SPCTitle;

                /// 5 Marker
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Series[RANGE].MarkerBorderColor = m_RANGE_CHART.SPCMarker.MarkerBorderColor;
                chart1.Series[RANGE].MarkerStyle = m_RANGE_CHART.SPCMarker.MarkerStyle;
                chart1.Series[RANGE].MarkerBorderColor = m_RANGE_CHART.SPCMarker.MarkerBorderColor;
                chart1.Series[RANGE].MarkerBorderWidth = m_RANGE_CHART.SPCMarker.MarkerBorderWidth;
                chart1.Series[RANGE].MarkerSize = m_RANGE_CHART.SPCMarker.MarkerSize;
                chart1.Series[RANGE].MarkerColor = m_RANGE_CHART.SPCMarker.MarkerColor;
                chart1.Series[RANGE].MarkerStep = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawRAW(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            string RAW_BOX = RAW + "_BOX";
            try
            {
                if (chart1.ChartAreas.IndexOf(RAW) < 0) chart1.ChartAreas.Add(RAW);
                if (chart1.Series.IndexOf(RAW) < 0) chart1.Series.Add(RAW);
                if (chart1.Titles.IndexOf(RAW) < 0) chart1.Titles.Add(RAW).Name = RAW;

                /// 1. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 1.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 75, 100, 25);
                chart1.ChartAreas[RAW].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[RAW].InnerPlotPosition = inner_pos;

                /// 1.2 Chart영역의 속성처리
                chart1.ChartAreas[RAW].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[RAW].Position.Auto = false;

                /// 1.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[RAW].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[RAW].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[RAW].AlignmentStyle = AreaAlignmentStyles.All;

                /// 1.4 Asix X 축 설정
                chart1.ChartAreas[RAW].AxisX = m_RAW_CHART.SPCAxisX;

                /// 1.5 Asix Y 축 설정
                chart1.ChartAreas[RAW].AxisY = m_RAW_CHART.SPCAxisY;

                /// 1.6 Set cursor properties 
                chart1.ChartAreas[RAW].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[RAW].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[RAW].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[RAW].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[RAW].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 

                chart1.Series[RAW].ChartArea = RAW;
                chart1.Series[RAW].Points.Clear();
                for (int i = 0; i < m_VAL_RAW.Count; i++)
                {
                    chart1.Series[RAW].Points.Add(m_VAL_RAW[i].YValues);
                }
                chart1.Series[RAW].ChartType = SeriesChartType.Point;

                if (m_iSampleCount > 3)
                {
                    if (chart1.Series.IndexOf(RAW_BOX) < 0) chart1.Series.Add(RAW_BOX);
                    chart1.Series[RAW_BOX].ChartArea = RAW;
                    chart1.Series[RAW_BOX].ChartType = SeriesChartType.BoxPlot;
                    chart1.DataManipulator.CopySeriesValues(RAW, RAW_BOX);
                }

                chart1.Series[RAW].Color = m_RAW_CHART.TrendColor;

                /// 3. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;
                ///chart1.Series[RAW].IsVisibleInLegend = false;

                /// 4. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[RAW] = m_RAW_CHART.SPCTitle;


                /// 5 Marker
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Series[RAW].MarkerBorderColor = m_RAW_CHART.SPCMarker.MarkerBorderColor;
                chart1.Series[RAW].MarkerStyle = m_RAW_CHART.SPCMarker.MarkerStyle;
                chart1.Series[RAW].MarkerBorderColor = m_RAW_CHART.SPCMarker.MarkerBorderColor;
                chart1.Series[RAW].MarkerBorderWidth = m_RAW_CHART.SPCMarker.MarkerBorderWidth;
                chart1.Series[RAW].MarkerSize = m_RAW_CHART.SPCMarker.MarkerSize;
                chart1.Series[RAW].MarkerColor = m_RAW_CHART.SPCMarker.MarkerColor;
                chart1.Series[RAW].MarkerStep = 1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawEWMA_MV(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(EWMA_MV) < 0) chart1.ChartAreas.Add(EWMA_MV);
                if (chart1.Series.IndexOf(EWMA_MV) < 0) chart1.Series.Add(EWMA_MV);
                if (chart1.Titles.IndexOf(EWMA_MV) < 0) chart1.Titles.Add(EWMA_MV).Name = EWMA_MV;

                //1. Series
                chart1.Series[EWMA_MV].ChartArea = EWMA_MV;
                chart1.Series[EWMA_MV].Points.DataBindY(m_VAL_EWMA_MV);
                chart1.Series[EWMA_MV].ChartType = SeriesChartType.Line;

                //2. Title
                chart1.Titles[EWMA_MV].Alignment = System.Drawing.ContentAlignment.BottomCenter;
                chart1.Titles[EWMA_MV].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
                chart1.Titles[EWMA_MV].DockingOffset = 2;
                chart1.Titles[EWMA_MV].IsDockedInsideChartArea = false;
                chart1.Titles[EWMA_MV].DockedToChartArea = EWMA_MV;

                //3.1 Marker
                chart1.Series[EWMA_MV].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[EWMA_MV].MarkerBorderColor = Color.Red;
                chart1.Series[EWMA_MV].MarkerBorderWidth = 2;
                chart1.Series[EWMA_MV].MarkerStep = 1;
                chart1.Series[EWMA_MV].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[EWMA_MV].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.

                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[EWMA_MV].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[EWMA_MV].InnerPlotPosition = inner_pos;

                chart1.ChartAreas[EWMA_MV].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[EWMA_MV].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[EWMA_MV].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_MV].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[EWMA_MV].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_MV].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[EWMA_MV].AxisY.IsStartedFromZero = false;
                //chart1.ChartAreas[EWMA_MV].AxisY.Maximum = dAvg + dStd * 12;
                //chart1.ChartAreas[EWMA_MV].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[EWMA_MV].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[EWMA_MV].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[EWMA_MV].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[EWMA_MV].Position.Auto = false;
                chart1.ChartAreas[EWMA_MV].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[EWMA_MV].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[EWMA_MV].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawEWMA_R(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(EWMA_R) < 0) chart1.ChartAreas.Add(EWMA_R);
                if (chart1.Series.IndexOf(EWMA_R) < 0) chart1.Series.Add(EWMA_R);
                if (chart1.Titles.IndexOf(EWMA_R) < 0) chart1.Titles.Add(EWMA_R).Name = EWMA_R;

                //1. Series
                chart1.Series[EWMA_R].ChartArea = EWMA_R;
                chart1.Series[EWMA_R].Points.DataBindY(m_VAL_EWMA_R);
                chart1.Series[EWMA_R].ChartType = SeriesChartType.Line;
                
                //2. Title
                chart1.Titles[EWMA_R].Alignment = System.Drawing.ContentAlignment.BottomCenter;
                chart1.Titles[EWMA_R].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
                chart1.Titles[EWMA_R].DockingOffset = 2;
                chart1.Titles[EWMA_R].IsDockedInsideChartArea = false;
                chart1.Titles[EWMA_R].DockedToChartArea = EWMA_R;

                //3.1 Marker
                chart1.Series[EWMA_R].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[EWMA_R].MarkerBorderColor = Color.Red;
                chart1.Series[EWMA_R].MarkerBorderWidth = 2;
                chart1.Series[EWMA_R].MarkerStep = 1;
                chart1.Series[EWMA_R].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[EWMA_R].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.

                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[EWMA_R].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[EWMA_R].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[EWMA_R].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[EWMA_R].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[EWMA_R].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_R].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[EWMA_R].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_R].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[EWMA_R].AxisY.IsStartedFromZero = false;
                //chart1.ChartAreas[EWMA_R].AxisY.Maximum = dAvg + dStd * 12;
                //chart1.ChartAreas[EWMA_R].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[EWMA_R].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[EWMA_R].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[EWMA_R].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[EWMA_R].Position.Auto = false;
                chart1.ChartAreas[EWMA_R].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[EWMA_R].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[EWMA_R].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawEWMA_S(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(EWMA_S) < 0) chart1.ChartAreas.Add(EWMA_S);
                if (chart1.Series.IndexOf(EWMA_S) < 0) chart1.Series.Add(EWMA_S);
                if (chart1.Titles.IndexOf(EWMA_S) < 0) chart1.Titles.Add(EWMA_S).Name = EWMA_S;

                //1. Series
                chart1.Series[EWMA_S].ChartArea = EWMA_S;
                chart1.Series[EWMA_S].Points.DataBindY(m_VAL_EWMA_S);
                chart1.Series[EWMA_S].ChartType = SeriesChartType.Line;

                //2. Title
                chart1.Titles[EWMA_S].Alignment = System.Drawing.ContentAlignment.BottomCenter;
                chart1.Titles[EWMA_S].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
                chart1.Titles[EWMA_S].DockingOffset = 2;
                chart1.Titles[EWMA_S].IsDockedInsideChartArea = false;
                chart1.Titles[EWMA_S].DockedToChartArea = EWMA_S;

                //3.1 Marker
                chart1.Series[EWMA_S].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[EWMA_S].MarkerBorderColor = Color.Red;
                chart1.Series[EWMA_S].MarkerBorderWidth = 2;
                chart1.Series[EWMA_S].MarkerStep = 1;
                chart1.Series[EWMA_S].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[EWMA_S].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.

                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[EWMA_S].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[EWMA_S].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[EWMA_S].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[EWMA_S].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[EWMA_S].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_S].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[EWMA_S].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[EWMA_S].AxisX.MinorGrid.Interval = 100;

                //chart1.ChartAreas[EWMA_S].AxisY.Maximum = dAvg + dStd * 12;
                //chart1.ChartAreas[EWMA_S].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[EWMA_S].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[EWMA_S].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[EWMA_S].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[EWMA_S].Position.Auto = false;
                chart1.ChartAreas[EWMA_S].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[EWMA_S].AlignmentStyle = AreaAlignmentStyles.All;
                chart1.ChartAreas[EWMA_S].AxisX.IsMarginVisible = true;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[EWMA_S].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawMA(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(MA) < 0) chart1.ChartAreas.Add(MA);
                if (chart1.Series.IndexOf(MA) < 0) chart1.Series.Add(MA);
                if (chart1.Titles.IndexOf(MA) < 0) chart1.Titles.Add(MA).Name = MA;

                //1. Series
                chart1.Series[MA].ChartArea = MA;
                chart1.Series[MA].Points.DataBindY(m_VAL_MA);
                chart1.Series[MA].ChartType = SeriesChartType.Line;

                //2. Title
                chart1.Titles[MA].Alignment = System.Drawing.ContentAlignment.BottomCenter;
                chart1.Titles[MA].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
                chart1.Titles[MA].DockingOffset = 2;
                chart1.Titles[MA].IsDockedInsideChartArea = false;
                chart1.Titles[MA].DockedToChartArea = MA;

                //3.1 Marker
                chart1.Series[MA].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[MA].MarkerBorderColor = Color.Red;
                chart1.Series[MA].MarkerBorderWidth = 2;
                chart1.Series[MA].MarkerStep = 1;
                chart1.Series[MA].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[MA].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.
                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[MA].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[MA].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[MA].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[MA].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[MA].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[MA].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[MA].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[MA].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[MA].AxisY.IsStartedFromZero = false;
                //chart1.ChartAreas[MA].AxisY.Maximum = dAvg + dStd * 12;
                //chart1.ChartAreas[MA].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[MA].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[MA].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[MA].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[MA].Position.Auto = false;
                chart1.ChartAreas[MA].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[MA].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[MA].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawMS(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(MS) < 0) chart1.ChartAreas.Add(MS);
                if (chart1.Series.IndexOf(MS) < 0) chart1.Series.Add(MS);
                if (chart1.Titles.IndexOf(MS) < 0) chart1.Titles.Add(MS).Name = MS;

                chart1.Series[MS].ChartArea = MS;
                chart1.Series[MS].Points.DataBindY(m_VAL_MS);
                chart1.Series[MS].ChartType = SeriesChartType.Line;

                //2. Title
                chart1.Titles[MS].Alignment = System.Drawing.ContentAlignment.BottomCenter;
                chart1.Titles[MS].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
                chart1.Titles[MS].DockingOffset = 2;
                chart1.Titles[MS].IsDockedInsideChartArea = false;
                chart1.Titles[MS].DockedToChartArea = MS;

                //3.1 Marker
                chart1.Series[MS].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[MS].MarkerBorderColor = Color.Red;
                chart1.Series[MS].MarkerBorderWidth = 2;
                chart1.Series[MS].MarkerStep = 1;
                chart1.Series[MS].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[MS].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.
                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[MS].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[MS].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[MS].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[MS].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[MS].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[MS].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[MS].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[MS].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[MS].AxisY.IsStartedFromZero = false;
                //chart1.ChartAreas[MS].AxisY.Maximum = dAvg + dStd * 12;
                //chart1.ChartAreas[MS].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[MS].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[MS].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[MS].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[MS].Position.Auto = false;
                chart1.ChartAreas[MS].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[MS].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[MS].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void __DrawRAW(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            string RAW_BOX = RAW + "_BOX";
            try
            {
                if (chart1.ChartAreas.IndexOf(RAW) < 0) chart1.ChartAreas.Add(RAW);
                if (chart1.Series.IndexOf(RAW) < 0) chart1.Series.Add(RAW);
                if (chart1.Titles.IndexOf(RAW) < 0) chart1.Titles.Add(RAW).Name = RAW;

                //1. Series
                chart1.Series[RAW].ChartArea = RAW;
                chart1.Series[RAW].Points.Clear();
                for (int i = 0; i < m_VAL_RAW.Count; i++)
                {
                    chart1.Series[RAW].Points.Add(m_VAL_RAW[i].YValues);
                }
                chart1.Series[RAW].ChartType = SeriesChartType.Point;

                if (m_iSampleCount > 3)
                {
                    if (chart1.Series.IndexOf(RAW_BOX) < 0) chart1.Series.Add(RAW_BOX);
                    chart1.Series[RAW_BOX].ChartArea = RAW;
                    chart1.Series[RAW_BOX].ChartType = SeriesChartType.BoxPlot;
                    chart1.DataManipulator.CopySeriesValues(RAW, RAW_BOX);
                }

                //2. Title
                chart1.Titles[RAW].Alignment = System.Drawing.ContentAlignment.BottomCenter;
                chart1.Titles[RAW].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
                chart1.Titles[RAW].DockingOffset = 2;
                chart1.Titles[RAW].IsDockedInsideChartArea = false;
                chart1.Titles[RAW].DockedToChartArea = RAW;

                //3.1 Marker
                chart1.Series[RAW].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[RAW].MarkerBorderColor = Color.Red;
                chart1.Series[RAW].MarkerBorderWidth = 2;
                chart1.Series[RAW].MarkerStep = 1;
                chart1.Series[RAW].MarkerSize = 3;

                //3.2 Marker Point
                // Set the chart area position for the first chart area.
                if (pos == null)
                {
                    pos = new ElementPosition(0, 75, 100, 25);
                }
                chart1.ChartAreas[RAW].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[RAW].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[RAW].AxisY.LabelStyle.Format = "{#.#00}";


                ////4. Grid
                chart1.ChartAreas[RAW].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[RAW].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[RAW].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[RAW].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[RAW].AxisX.MinorGrid.Interval = 10;

                //chart1.ChartAreas[RAW].AxisY.Maximum = dAvg + dStd * 6;
                //chart1.ChartAreas[RAW].AxisY.Minimum = dAvg - dStd * 6;
                chart1.Series[RAW].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[RAW].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[RAW].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[RAW].Position.Auto = false;
                chart1.ChartAreas[RAW].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일

                chart1.ChartAreas[RAW].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[RAW].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void DrawI(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(I) < 0) chart1.ChartAreas.Add(I);
                if (chart1.Series.IndexOf(I) < 0) chart1.Series.Add(I);
                if (chart1.Titles.IndexOf(I) < 0) chart1.Titles.Add(I).Name = I;
                chart1.Series[I].ChartArea = I;
                chart1.Series[I].Points.DataBindY(m_VAL_I);
                chart1.Series[I].ChartType = SeriesChartType.Line;

                //2. Title
                chart1.Titles[I].Alignment = System.Drawing.ContentAlignment.BottomCenter;
                chart1.Titles[I].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
                chart1.Titles[I].DockingOffset = 2;
                chart1.Titles[I].IsDockedInsideChartArea = false;
                chart1.Titles[I].DockedToChartArea = I;

                //3.1 Marker
                chart1.Series[I].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[I].MarkerBorderColor = Color.Red;
                chart1.Series[I].MarkerBorderWidth = 2;
                chart1.Series[I].MarkerStep = 1;
                chart1.Series[I].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[I].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.
                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[I].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[I].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[I].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[I].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[I].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[I].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[I].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[I].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[I].AxisY.Maximum = Math.Max(
                                                    Math.Max(double.IsNaN(m_MaxUSL) ? double.MinValue : m_MaxUSL
                                                           , double.IsNaN(m_MaxUCL) ? double.MinValue : m_MaxUCL)
                                                           , m_dAVG + 3 * m_dSTD
                                                           ) + 3 * m_dSTD;
                chart1.ChartAreas[I].AxisY.Minimum = Math.Min(
                                                Math.Min(double.IsNaN(m_MinLSL) ? double.MaxValue : m_MinLSL
                                                        , double.IsNaN(m_MinLCL) ? double.MaxValue : m_MinLCL)
                                                        , m_dAVG - 3 * m_dSTD
                                                        ) - 3 * m_dSTD;


                chart1.ChartAreas[I].AxisY.IsStartedFromZero = false;
                chart1.Series[I].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[I].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[I].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[I].Position.Auto = false;
                chart1.ChartAreas[I].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[I].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[I].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawMR(ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(MR) < 0) chart1.ChartAreas.Add(MR);
                if (chart1.Series.IndexOf(MR) < 0) chart1.Series.Add(MR);
                if (chart1.Titles.IndexOf(MR) < 0) chart1.Titles.Add(MR).Name = MR;

                chart1.Series[MR].ChartArea = MR;
                chart1.Series[MR].Points.DataBindY(m_VAL_MR);
                chart1.Series[MR].ChartType = SeriesChartType.Line;

                //2. Title
                chart1.Titles[MR].Alignment = System.Drawing.ContentAlignment.BottomCenter;
                chart1.Titles[MR].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
                chart1.Titles[MR].DockingOffset = 2;
                chart1.Titles[MR].IsDockedInsideChartArea = false;
                chart1.Titles[MR].DockedToChartArea = MR;

                //3.1 Marker
                chart1.Series[MR].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[MR].MarkerBorderColor = Color.Red;
                chart1.Series[MR].MarkerBorderWidth = 2;
                chart1.Series[MR].MarkerStep = 1;
                chart1.Series[MR].MarkerSize = 3;
                //3.2 Marker Point
                chart1.Series[MR].Points[3].MarkerColor = Color.Blue;

                // Set the chart area position for the first chart area.
                if (pos == null)
                {
                    pos = new ElementPosition(10, 75, 100, 25);
                }
                chart1.ChartAreas[MR].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[MR].InnerPlotPosition = inner_pos;
                chart1.ChartAreas[MR].AxisY.LabelStyle.Format = "{#.#00}";

                ////4. Grid
                chart1.ChartAreas[MR].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[MR].AxisX.MajorGrid.Enabled = true;
                chart1.ChartAreas[MR].AxisX.MajorGrid.LineColor = Color.LightGray;

                chart1.ChartAreas[MR].AxisX.MinorGrid.Enabled = true;
                chart1.ChartAreas[MR].AxisX.MinorGrid.Interval = 100;

                chart1.ChartAreas[MR].AxisY.IsStartedFromZero = false;
                //chart1.ChartAreas[MR].AxisY.Maximum = dAvg + dStd * 12;
                //chart1.ChartAreas[MR].AxisY.Minimum = dAvg - dStd * 12;
                chart1.Series[MR].IsVisibleInLegend = false;
                chart1.Legends[0].Enabled = false;

                // Set the alignment type
                chart1.ChartAreas[MR].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[MR].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[MR].Position.Auto = false;
                chart1.ChartAreas[MR].AlignWithChartArea = chart1.ChartAreas[0].Name; //첫번째것으로 통일
                chart1.ChartAreas[MR].AlignmentStyle = AreaAlignmentStyles.All;

                // Create cursor object
                System.Windows.Forms.DataVisualization.Charting.Cursor cursorx = null;

                // Set cursor object
                cursorx = chart1.ChartAreas[MR].CursorX;

                // Set cursor properties 
                cursorx.LineWidth = m_csLineWidth;
                cursorx.LineDashStyle = m_csDashStyle;
                cursorx.LineColor = m_csLineColor;
                cursorx.SelectionColor = m_csSelectionColor;
                cursorx.IsUserEnabled = m_csUserEnabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawXBAR(string MatchingCombination ,string[] CombinationList, ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                /// Data Split
                /// 
                if (chart1.ChartAreas.IndexOf(XBAR) < 0) chart1.ChartAreas.Add(XBAR);

                /// 1. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                for (int i = 0; i < CombinationList.Length + 1; i++)
                {
                    string serName = string.Format("{0}_{1}", XBAR, i);
                    if (chart1.Series.IndexOf(serName) < 0) chart1.Series.Add(serName);
                    chart1.Series[serName].ChartArea = XBAR;

                    if (m_CMB_VAL_XBAR[i] == null) continue;
                    for(int j=0;j<m_CMB_VAL_XBAR[i].Count;j++)  chart1.Series[serName].Points.Add(m_CMB_VAL_XBAR[i][j]);
                    chart1.Series[serName].ChartType = SeriesChartType.Line;
                    //chart1.Series[serName].Color = m_XBAR_CHART.TrendColor;

                    /// 1.1 Marker
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (i < CombinationList.Length)
                    {
                        chart1.Series[serName].Color = m_COL_Combination[i];
                        chart1.Series[serName].MarkerBorderColor = m_XBAR_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerStyle = m_XBAR_CHART.SPCMarker.MarkerStyle;
                        chart1.Series[serName].MarkerBorderColor = m_XBAR_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerBorderWidth = m_XBAR_CHART.SPCMarker.MarkerBorderWidth;
                        chart1.Series[serName].MarkerSize = m_XBAR_CHART.SPCMarker.MarkerSize;
                        chart1.Series[serName].MarkerColor = m_XBAR_CHART.SPCMarker.MarkerColor;
                        chart1.Series[serName].MarkerStep = 1;
                    }
                    else
                    {
                        chart1.Series[serName].Enabled = false;
                    }
                }

                if (chart1.Titles.IndexOf(XBAR) < 0) chart1.Titles.Add(XBAR).Name = XBAR;

                /// 2. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 2.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 0, 100, 25);
                chart1.ChartAreas[XBAR].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[XBAR].InnerPlotPosition = inner_pos;

                /// 2.2 Chart영역의 속성처리
                chart1.ChartAreas[XBAR].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[XBAR].Position.Auto = false;

                /// 2.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[XBAR].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[XBAR].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[XBAR].AlignmentStyle = AreaAlignmentStyles.All;

                /// 2.4 Asix X 축 설정
                chart1.ChartAreas[XBAR].AxisX = m_XBAR_CHART.SPCAxisX;

                /// 2.5 Asix Y 축 설정
                chart1.ChartAreas[XBAR].AxisY = m_XBAR_CHART.SPCAxisY;

                chart1.ChartAreas[XBAR].AxisY.LabelStyle.Format = m_XBAR_CHART.SPCAxisY.LabelStyle.Format;
                chart1.ChartAreas[XBAR].AxisY.MajorGrid.Enabled = m_XBAR_CHART.SPCAxisY.MajorGrid.Enabled;
                chart1.ChartAreas[XBAR].AxisY.MajorGrid.Interval = m_XBAR_CHART.SPCAxisY.MajorGrid.Interval;
                chart1.ChartAreas[XBAR].AxisY.MajorGrid.LineColor = m_XBAR_CHART.SPCAxisY.MajorGrid.LineColor;
                chart1.ChartAreas[XBAR].AxisY.MajorGrid.LineDashStyle = m_XBAR_CHART.SPCAxisY.MajorGrid.LineDashStyle;

                chart1.ChartAreas[XBAR].AxisY.MinorGrid.Enabled = m_XBAR_CHART.SPCAxisY.MinorGrid.Enabled;
                chart1.ChartAreas[XBAR].AxisY.MinorGrid.Interval = m_XBAR_CHART.SPCAxisY.MajorGrid.Interval;
                chart1.ChartAreas[XBAR].AxisY.MinorGrid.LineColor = m_XBAR_CHART.SPCAxisY.MajorGrid.LineColor;
                chart1.ChartAreas[XBAR].AxisY.MinorGrid.LineDashStyle = m_XBAR_CHART.SPCAxisY.MajorGrid.LineDashStyle;
                chart1.ChartAreas[XBAR].AxisY.IsStartedFromZero = m_XBAR_CHART.SPCAxisY.IsStartedFromZero;

                /// 2.6 Set cursor properties 
                chart1.ChartAreas[XBAR].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[XBAR].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[XBAR].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[XBAR].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[XBAR].CursorX.IsUserEnabled = m_csUserEnabled;


                /// 3. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;
                //chart1.Series[XBAR].IsVisibleInLegend = false;

                /// 4. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[XBAR] = m_XBAR_CHART.SPCTitle;
                chart1.ChartAreas[XBAR].AxisX.Interval = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawSIGMA(string MatchingCombination, string[] CombinationList, ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(SIGMA) < 0) chart1.ChartAreas.Add(SIGMA);
                /// 1. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                for (int i = 0; i < CombinationList.Length+1; i++)
                {
                    string serName = string.Format("{0}_{1}", SIGMA, i);
                    if (chart1.Series.IndexOf(serName) < 0) chart1.Series.Add(serName);
                    chart1.Series[serName].ChartArea = SIGMA;

                    if (m_CMB_VAL_SIGMA[i] == null) continue;
                    for (int j = 0; j < m_CMB_VAL_SIGMA[i].Count; j++) chart1.Series[serName].Points.Add(m_CMB_VAL_SIGMA[i][j]);
                    chart1.Series[serName].ChartType = SeriesChartType.Line;

                    /// 1.1 Marker
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (i < CombinationList.Length)
                    {
                        chart1.Series[serName].Color = m_COL_Combination[i];
                        chart1.Series[serName].MarkerBorderColor = m_SIGMA_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerStyle = m_SIGMA_CHART.SPCMarker.MarkerStyle;
                        chart1.Series[serName].MarkerBorderColor = m_SIGMA_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerBorderWidth = m_SIGMA_CHART.SPCMarker.MarkerBorderWidth;
                        chart1.Series[serName].MarkerSize = m_SIGMA_CHART.SPCMarker.MarkerSize;
                        chart1.Series[serName].MarkerColor = m_SIGMA_CHART.SPCMarker.MarkerColor;
                        chart1.Series[serName].MarkerStep = 1;
                    }
                    else
                    {
                        chart1.Series[serName].Enabled = false;
                    }
                }

                if (chart1.Titles.IndexOf(SIGMA) < 0) chart1.Titles.Add(SIGMA).Name = SIGMA;


                /// 2. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 2.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 25, 100, 25);
                chart1.ChartAreas[SIGMA].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[SIGMA].InnerPlotPosition = inner_pos;

                /// 2.2 Chart영역의 속성처리
                chart1.ChartAreas[SIGMA].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[SIGMA].Position.Auto = false;

                /// 2.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[SIGMA].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[SIGMA].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[SIGMA].AlignmentStyle = AreaAlignmentStyles.All;

                /// 2.4 Asix X 축 설정
                chart1.ChartAreas[SIGMA].AxisX = m_SIGMA_CHART.SPCAxisX;

                /// 2.5 Asix Y 축 설정
                chart1.ChartAreas[SIGMA].AxisY = m_SIGMA_CHART.SPCAxisY;

                /// 2.6 Set cursor properties 
                chart1.ChartAreas[SIGMA].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[SIGMA].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[SIGMA].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[SIGMA].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[SIGMA].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;

                /// 3. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[SIGMA] = m_SIGMA_CHART.SPCTitle;

  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawRANGE(string MatchingCombination, string[] CombinationList, ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(RANGE) < 0) chart1.ChartAreas.Add(RANGE);
                /// 1. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                for (int i = 0; i < CombinationList.Length + 1; i++)
                {
                    string serName = string.Format("{0}_{1}", RANGE, i);
                    if (chart1.Series.IndexOf(serName) < 0) chart1.Series.Add(serName);
                    chart1.Series[serName].ChartArea = RANGE;

                    if (m_CMB_VAL_RANGE[i] == null) continue;
                    for (int j = 0; j < m_CMB_VAL_RANGE[i].Count; j++) chart1.Series[serName].Points.Add(m_CMB_VAL_RANGE[i][j]);
                    chart1.Series[serName].ChartType = SeriesChartType.Line;

                    /// 1.1 Marker
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (i < CombinationList.Length)
                    {
                        chart1.Series[serName].Color = m_COL_Combination[i];
                        chart1.Series[serName].MarkerBorderColor = m_RANGE_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerStyle = m_RANGE_CHART.SPCMarker.MarkerStyle;
                        chart1.Series[serName].MarkerBorderColor = m_RANGE_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerBorderWidth = m_RANGE_CHART.SPCMarker.MarkerBorderWidth;
                        chart1.Series[serName].MarkerSize = m_RANGE_CHART.SPCMarker.MarkerSize;
                        chart1.Series[serName].MarkerColor = m_RANGE_CHART.SPCMarker.MarkerColor;
                        chart1.Series[serName].MarkerStep = 1;
                    }
                    else
                    {
                        chart1.Series[serName].Enabled = false;
                    }
                }
                if (chart1.Titles.IndexOf(RANGE) < 0) chart1.Titles.Add(RANGE).Name = RANGE;


                /// 2. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 2.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 50, 100, 25);
                chart1.ChartAreas[RANGE].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[RANGE].InnerPlotPosition = inner_pos;

                /// 2.2 Chart영역의 속성처리
                chart1.ChartAreas[RANGE].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[RANGE].Position.Auto = false;

                /// 2.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[RANGE].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[RANGE].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[RANGE].AlignmentStyle = AreaAlignmentStyles.All;

                /// 2.4 Asix X 축 설정
                chart1.ChartAreas[RANGE].AxisX = m_RANGE_CHART.SPCAxisX;

                /// 2.5 Asix Y 축 설정
                chart1.ChartAreas[RANGE].AxisY = m_RANGE_CHART.SPCAxisY;

                /// 2.6 Set cursor properties 
                chart1.ChartAreas[RANGE].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[RANGE].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[RANGE].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[RANGE].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[RANGE].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 3. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;

                /// 4. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[RANGE] = m_RANGE_CHART.SPCTitle;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawRAW(string MatchingCombination, string[] CombinationList, ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            string RAW_BOX = RAW + "_BOX";
            try
            {
                if (chart1.ChartAreas.IndexOf(RAW) < 0) chart1.ChartAreas.Add(RAW);
                /// 1. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                for (int i = 0; i < CombinationList.Length + 1; i++)
                {
                    string serNameRAW = string.Format("{0}_{1}", RAW, i);
                    string serNameRAW_BOX = string.Format("{0}_{1}", RAW_BOX, i);
                    if (chart1.Series.IndexOf(serNameRAW) < 0) chart1.Series.Add(serNameRAW);
                    chart1.Series[serNameRAW].ChartArea = RAW;

                    /// 1.1 Series 설정
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (m_CMB_VAL_RAW[i] == null) continue;
                    chart1.Series[serNameRAW].Points.Clear();
                    for (int j = 0; j < m_CMB_VAL_RAW[i].Count; j++)
                    {
                        chart1.Series[serNameRAW].Points.AddXY(m_CMB_VAL_RAW[i][j].XValue, m_CMB_VAL_RAW[i][j].YValues);
                    }
                    chart1.Series[serNameRAW].ChartType = SeriesChartType.Point;

                    if (m_iSampleCount > 3)
                    {
                        if (chart1.Series.IndexOf(serNameRAW_BOX) < 0) chart1.Series.Add(serNameRAW_BOX);
                        chart1.Series[serNameRAW_BOX].ChartArea = RAW;
                        chart1.Series[serNameRAW_BOX].ChartType = SeriesChartType.BoxPlot;
                        chart1.DataManipulator.CopySeriesValues(serNameRAW, serNameRAW_BOX);
                    }


                    chart1.Series[serNameRAW_BOX].Color = m_RAW_CHART.TrendColor;

                    /// 1.2 Marker
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (i < CombinationList.Length)
                    {
                        chart1.Series[serNameRAW_BOX].Color = m_COL_Combination[i];
                        chart1.Series[serNameRAW_BOX].MarkerBorderColor = m_RAW_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serNameRAW_BOX].MarkerStyle = m_RAW_CHART.SPCMarker.MarkerStyle;
                        chart1.Series[serNameRAW_BOX].MarkerBorderColor = m_RAW_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serNameRAW_BOX].MarkerBorderWidth = m_RAW_CHART.SPCMarker.MarkerBorderWidth;
                        chart1.Series[serNameRAW_BOX].MarkerSize = m_RAW_CHART.SPCMarker.MarkerSize;
                        chart1.Series[serNameRAW_BOX].MarkerColor = m_RAW_CHART.SPCMarker.MarkerColor;
                        chart1.Series[serNameRAW_BOX].MarkerStep = 1;
                    }
                    else
                    {
                        chart1.Series[serNameRAW_BOX].Enabled = false;
                    }
                }
                if (chart1.Titles.IndexOf(RAW) < 0) chart1.Titles.Add(RAW).Name = RAW;

                /// 2. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 2.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 75, 100, 25);
                chart1.ChartAreas[RAW].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[RAW].InnerPlotPosition = inner_pos;

                /// 2.2 Chart영역의 속성처리
                chart1.ChartAreas[RAW].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[RAW].Position.Auto = false;

                /// 2.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[RAW].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[RAW].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[RAW].AlignmentStyle = AreaAlignmentStyles.All;

                /// 2.4 Asix X 축 설정
                chart1.ChartAreas[RAW].AxisX = m_RAW_CHART.SPCAxisX;

                /// 2.5 Asix Y 축 설정
                chart1.ChartAreas[RAW].AxisY = m_RAW_CHART.SPCAxisY;

                /// 2.6 Set cursor properties 
                chart1.ChartAreas[RAW].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[RAW].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[RAW].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[RAW].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[RAW].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 3. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;
                ///chart1.Series[RAW].IsVisibleInLegend = false;

                /// 4. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[RAW] = m_RAW_CHART.SPCTitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawEWMA_MV(string MatchingCombination, string[] CombinationList, ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(EWMA_MV) < 0) chart1.ChartAreas.Add(EWMA_MV);
                /// 1. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                for (int i = 0; i < CombinationList.Length+1; i++)
                {
                    string serName = string.Format("{0}_{1}", EWMA_MV, i);
                    if (chart1.Series.IndexOf(serName) < 0) chart1.Series.Add(serName);
                    chart1.Series[serName].ChartArea = EWMA_MV;

                    if (m_CMB_VAL_EWMA_MV[i] == null) continue;
                    for (int j = 0; j < m_CMB_VAL_EWMA_MV[i].Count; j++) chart1.Series[serName].Points.Add(m_CMB_VAL_EWMA_MV[i][j]);
                    chart1.Series[serName].ChartType = SeriesChartType.Line;

                    /// 1.1 Marker
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (i < CombinationList.Length)
                    {
                        chart1.Series[serName].Color = m_COL_Combination[i];
                        chart1.Series[serName].MarkerBorderColor = m_EWMA_MV_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerStyle = m_EWMA_MV_CHART.SPCMarker.MarkerStyle;
                        chart1.Series[serName].MarkerBorderColor = m_EWMA_MV_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerBorderWidth = m_EWMA_MV_CHART.SPCMarker.MarkerBorderWidth;
                        chart1.Series[serName].MarkerSize = m_EWMA_MV_CHART.SPCMarker.MarkerSize;
                        chart1.Series[serName].MarkerColor = m_EWMA_MV_CHART.SPCMarker.MarkerColor;
                        chart1.Series[serName].MarkerStep = 1;
                    }
                    else
                    {
                        chart1.Series[serName].Enabled = false;
                    }
                }
                if (chart1.Titles.IndexOf(EWMA_MV) < 0) chart1.Titles.Add(EWMA_MV).Name = EWMA_MV;

                /// 2. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 2.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 25, 100, 25);
                chart1.ChartAreas[EWMA_MV].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[EWMA_MV].InnerPlotPosition = inner_pos;

                /// 2.2 Chart영역의 속성처리
                chart1.ChartAreas[EWMA_MV].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[EWMA_MV].Position.Auto = false;

                /// 2.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[EWMA_MV].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[EWMA_MV].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[EWMA_MV].AlignmentStyle = AreaAlignmentStyles.All;

                /// 2.4 Asix X 축 설정
                chart1.ChartAreas[EWMA_MV].AxisX = m_EWMA_MV_CHART.SPCAxisX;

                /// 2.5 Asix Y 축 설정
                chart1.ChartAreas[EWMA_MV].AxisY = m_EWMA_MV_CHART.SPCAxisY;

                /// 2.6 Set cursor properties 
                chart1.ChartAreas[EWMA_MV].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[EWMA_MV].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[EWMA_MV].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[EWMA_MV].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[EWMA_MV].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;

                /// 3. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[EWMA_MV] = m_EWMA_MV_CHART.SPCTitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawEWMA_R(string MatchingCombination, string[] CombinationList, ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(EWMA_R) < 0) chart1.ChartAreas.Add(EWMA_R);
                /// 1. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                for (int i = 0; i < CombinationList.Length + 1; i++)
                {
                    string serName = string.Format("{0}_{1}", EWMA_R, i);
                    if (chart1.Series.IndexOf(serName) < 0) chart1.Series.Add(serName);
                    chart1.Series[serName].ChartArea = EWMA_R;

                    if (m_CMB_VAL_EWMA_R[i] == null) continue;
                    for (int j = 0; j < m_CMB_VAL_EWMA_R[i].Count; j++) chart1.Series[serName].Points.Add(m_CMB_VAL_EWMA_R[i][j]);
                    chart1.Series[serName].ChartType = SeriesChartType.Line;

                    /// 1.1 Marker
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (i < CombinationList.Length)
                    {
                        chart1.Series[serName].Color = m_COL_Combination[i];
                        chart1.Series[serName].MarkerBorderColor = m_EWMA_R_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerStyle = m_EWMA_R_CHART.SPCMarker.MarkerStyle;
                        chart1.Series[serName].MarkerBorderColor = m_EWMA_R_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerBorderWidth = m_EWMA_R_CHART.SPCMarker.MarkerBorderWidth;
                        chart1.Series[serName].MarkerSize = m_EWMA_R_CHART.SPCMarker.MarkerSize;
                        chart1.Series[serName].MarkerColor = m_EWMA_R_CHART.SPCMarker.MarkerColor;
                        chart1.Series[serName].MarkerStep = 1;
                    }
                    else
                    {
                        chart1.Series[serName].Enabled = false;
                    }
                }
                if (chart1.Titles.IndexOf(EWMA_R) < 0) chart1.Titles.Add(EWMA_R).Name = EWMA_R;

                /// 2. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 2.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 25, 100, 25);
                chart1.ChartAreas[EWMA_R].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[EWMA_R].InnerPlotPosition = inner_pos;

                /// 2.2 Chart영역의 속성처리
                chart1.ChartAreas[EWMA_R].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[EWMA_R].Position.Auto = false;

                /// 2.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[EWMA_R].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[EWMA_R].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[EWMA_R].AlignmentStyle = AreaAlignmentStyles.All;

                /// 2.4 Asix X 축 설정
                chart1.ChartAreas[EWMA_R].AxisX = m_EWMA_R_CHART.SPCAxisX;

                /// 2.5 Asix Y 축 설정
                chart1.ChartAreas[EWMA_R].AxisY = m_EWMA_R_CHART.SPCAxisY;

                /// 2.6 Set cursor properties 
                chart1.ChartAreas[EWMA_R].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[EWMA_R].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[EWMA_R].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[EWMA_R].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[EWMA_R].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;

                /// 3. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[EWMA_R] = m_EWMA_R_CHART.SPCTitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawEWMA_S(string MatchingCombination, string[] CombinationList, ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(EWMA_S) < 0) chart1.ChartAreas.Add(EWMA_S);
                /// 1. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                for (int i = 0; i < CombinationList.Length + 1; i++)
                {
                    string serName = string.Format("{0}_{1}", EWMA_S, i);
                    if (chart1.Series.IndexOf(serName) < 0) chart1.Series.Add(serName);
                    chart1.Series[serName].ChartArea = EWMA_S;

                    if (m_CMB_VAL_EWMA_S[i] == null) continue;
                    for (int j = 0; j < m_CMB_VAL_EWMA_S[i].Count; j++) chart1.Series[serName].Points.Add(m_CMB_VAL_EWMA_S[i][j]);
                    chart1.Series[serName].ChartType = SeriesChartType.Line;

                    /// 1.1 Marker
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (i < CombinationList.Length)
                    {
                        chart1.Series[serName].Color = m_COL_Combination[i];
                        chart1.Series[serName].MarkerBorderColor = m_EWMA_S_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerStyle = m_EWMA_S_CHART.SPCMarker.MarkerStyle;
                        chart1.Series[serName].MarkerBorderColor = m_EWMA_S_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerBorderWidth = m_EWMA_S_CHART.SPCMarker.MarkerBorderWidth;
                        chart1.Series[serName].MarkerSize = m_EWMA_S_CHART.SPCMarker.MarkerSize;
                        chart1.Series[serName].MarkerColor = m_EWMA_S_CHART.SPCMarker.MarkerColor;
                        chart1.Series[serName].MarkerStep = 1;
                    }
                    else
                    {
                        chart1.Series[serName].Enabled = false;
                    }
                }
                if (chart1.Titles.IndexOf(EWMA_S) < 0) chart1.Titles.Add(EWMA_S).Name = EWMA_S;

                /// 2. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 2.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 25, 100, 25);
                chart1.ChartAreas[EWMA_S].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[EWMA_S].InnerPlotPosition = inner_pos;

                /// 2.2 Chart영역의 속성처리
                chart1.ChartAreas[EWMA_S].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[EWMA_S].Position.Auto = false;

                /// 2.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[EWMA_S].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[EWMA_S].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[EWMA_S].AlignmentStyle = AreaAlignmentStyles.All;

                /// 2.4 Asix X 축 설정
                chart1.ChartAreas[EWMA_S].AxisX = m_EWMA_S_CHART.SPCAxisX;

                /// 2.5 Asix Y 축 설정
                chart1.ChartAreas[EWMA_S].AxisY = m_EWMA_S_CHART.SPCAxisY;

                /// 2.6 Set cursor properties 
                chart1.ChartAreas[EWMA_S].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[EWMA_S].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[EWMA_S].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[EWMA_S].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[EWMA_S].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;

                /// 3. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[EWMA_S] = m_EWMA_S_CHART.SPCTitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawMA(string MatchingCombination, string[] CombinationList, ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(MA) < 0) chart1.ChartAreas.Add(MA);
                /// 1. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                for (int i = 0; i < CombinationList.Length + 1; i++)
                {
                    string serName = string.Format("{0}_{1}", MA, i);
                    if (chart1.Series.IndexOf(serName) < 0) chart1.Series.Add(serName);
                    chart1.Series[serName].ChartArea = MA;

                    if (m_CMB_VAL_MA[i] == null) continue;
                    for (int j = 0; j < m_CMB_VAL_MA[i].Count; j++) chart1.Series[serName].Points.Add(m_CMB_VAL_MA[i][j]);
                    chart1.Series[serName].ChartType = SeriesChartType.Line;

                    /// 1.1 Marker
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (i < CombinationList.Length)
                    {
                        chart1.Series[serName].Color = m_COL_Combination[i];
                        chart1.Series[serName].MarkerBorderColor = m_MA_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerStyle = m_MA_CHART.SPCMarker.MarkerStyle;
                        chart1.Series[serName].MarkerBorderColor = m_MA_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerBorderWidth = m_MA_CHART.SPCMarker.MarkerBorderWidth;
                        chart1.Series[serName].MarkerSize = m_MA_CHART.SPCMarker.MarkerSize;
                        chart1.Series[serName].MarkerColor = m_MA_CHART.SPCMarker.MarkerColor;
                        chart1.Series[serName].MarkerStep = 1;
                    }
                    else
                    {
                        chart1.Series[serName].Enabled = false;
                    }
                }
                if (chart1.Titles.IndexOf(MA) < 0) chart1.Titles.Add(MA).Name = MA;

                /// 2. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 2.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 25, 100, 25);
                chart1.ChartAreas[MA].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[MA].InnerPlotPosition = inner_pos;

                /// 2.2 Chart영역의 속성처리
                chart1.ChartAreas[MA].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[MA].Position.Auto = false;

                /// 2.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[MA].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[MA].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[MA].AlignmentStyle = AreaAlignmentStyles.All;

                /// 2.4 Asix X 축 설정
                chart1.ChartAreas[MA].AxisX = m_MA_CHART.SPCAxisX;

                /// 2.5 Asix Y 축 설정
                chart1.ChartAreas[MA].AxisY = m_MA_CHART.SPCAxisY;

                /// 2.6 Set cursor properties 
                chart1.ChartAreas[MA].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[MA].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[MA].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[MA].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[MA].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;

                /// 3. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[MA] = m_MA_CHART.SPCTitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DrawMS(string MatchingCombination, string[] CombinationList, ElementPosition pos = null, ElementPosition inner_pos = null)
        {
            try
            {
                if (chart1.ChartAreas.IndexOf(MS) < 0) chart1.ChartAreas.Add(MS);
                /// 1. Series 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                for (int i = 0; i < CombinationList.Length + 1; i++)
                {
                    string serName = string.Format("{0}_{1}", MS, i);
                    if (chart1.Series.IndexOf(serName) < 0) chart1.Series.Add(serName);
                    chart1.Series[serName].ChartArea = MS;

                    if (m_CMB_VAL_MS[i] == null) continue;
                    for (int j = 0; j < m_CMB_VAL_MS[i].Count; j++) chart1.Series[serName].Points.Add(m_CMB_VAL_MS[i][j]);
                    chart1.Series[serName].ChartType = SeriesChartType.Line;

                    /// 1.1 Marker
                    /////////////////////////////////////////////////////////////////////////////////
                    /// 
                    if (i < CombinationList.Length)
                    {
                        chart1.Series[serName].Color = m_COL_Combination[i];
                        chart1.Series[serName].MarkerBorderColor = m_MS_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerStyle = m_MS_CHART.SPCMarker.MarkerStyle;
                        chart1.Series[serName].MarkerBorderColor = m_MS_CHART.SPCMarker.MarkerBorderColor;
                        chart1.Series[serName].MarkerBorderWidth = m_MS_CHART.SPCMarker.MarkerBorderWidth;
                        chart1.Series[serName].MarkerSize = m_MS_CHART.SPCMarker.MarkerSize;
                        chart1.Series[serName].MarkerColor = m_MS_CHART.SPCMarker.MarkerColor;
                        chart1.Series[serName].MarkerStep = 1;
                    }
                    else
                    {
                        chart1.Series[serName].Enabled = false;
                    }
                }
                if (chart1.Titles.IndexOf(MS) < 0) chart1.Titles.Add(MS).Name = MS;

                /// 2. ChartArea
                /////////////////////////////////////////////////////////////////////////////////
                /// 2.1 Position이 지정되지 않을시 Default로 지정
                if (pos == null) pos = new ElementPosition(0, 25, 100, 25);
                chart1.ChartAreas[MS].Position = pos;
                if (inner_pos != null) chart1.ChartAreas[MS].InnerPlotPosition = inner_pos;

                /// 2.2 Chart영역의 속성처리
                chart1.ChartAreas[MS].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[MS].Position.Auto = false;

                /// 2.3 Chart가 다른 ChartArea와 Align 형성하여 Syn를 맞춘다
                chart1.ChartAreas[MS].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                chart1.ChartAreas[MS].AlignWithChartArea = chart1.ChartAreas[0].Name; //무조건 첫번째것으로 통일
                chart1.ChartAreas[MS].AlignmentStyle = AreaAlignmentStyles.All;

                /// 2.4 Asix X 축 설정
                chart1.ChartAreas[MS].AxisX = m_MS_CHART.SPCAxisX;

                /// 2.5 Asix Y 축 설정
                chart1.ChartAreas[MS].AxisY = m_MS_CHART.SPCAxisY;

                /// 2.6 Set cursor properties 
                chart1.ChartAreas[MS].CursorX.LineWidth = m_csLineWidth;
                chart1.ChartAreas[MS].CursorX.LineDashStyle = m_csDashStyle;
                chart1.ChartAreas[MS].CursorX.LineColor = m_csLineColor;
                chart1.ChartAreas[MS].CursorX.SelectionColor = m_csSelectionColor;
                chart1.ChartAreas[MS].CursorX.IsUserEnabled = m_csUserEnabled;

                /// 2. Legends 설정... 안보이게 설정
                /////////////////////////////////////////////////////////////////////////////////
                chart1.Legends[0].Enabled = false;

                /// 3. Title 설정
                /////////////////////////////////////////////////////////////////////////////////
                /// 
                chart1.Titles[MS] = m_MS_CHART.SPCTitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawSPC()
        {
            try
            {
                DrawSPC(string.Empty, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawSPC(string CombinationColumn, string[] CombinationList)
        {
            if (m_dt == null) return;
            int height = 0;
            int iChartCnt = 0;
            try
            {
                if (m_XBAR_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_SIGMA_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_RANGE_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_RAW_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_EWMA_MV_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_EWMA_R_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_EWMA_S_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_MA_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_MS_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_I_CHART.ChartVisible)
                {
                    //chart1.ContextMenuStrip = null;
                    toggleInternalFlagToolStripMenuItem.Visible = false;
                    propertiesToolStripMenuItem.Visible = false;

                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (m_MR_CHART.ChartVisible)
                {
                    iChartCnt++;
                    height += m_iChartMinHeight;
                }

                if (iChartCnt == 0) return;
                if (height == 0) this.Height = this.Parent.Height;

                if (m_FitAllChart)
                {
                    this.Dock = DockStyle.Fill;
                }
                else
                {
                    this.Dock = DockStyle.Top;
                }

                this.Height = Math.Max(this.Height, height);

                chart1.ChartAreas.Clear();
                chart1.Series.Clear();
                chart1.Titles.Clear();


                int poX = 0;
                int poY = 0;
                int poW = 100;
                int poH = 100 / iChartCnt;

                if (CombinationColumn != null && CombinationList != null && string.IsNullOrEmpty(CombinationColumn) == false && CombinationList.Length > 0)
                {
                    #region Combination Draw chart
                    CombinationDataCal(CombinationColumn, CombinationList);
                    
                    if (m_XBAR_CHART.ChartVisible)
                    {
                        DrawXBAR(CombinationColumn, CombinationList, new ElementPosition(poX, poY, poW, poH));
                        DrawInformation();

                        if (m_oSPEC_CFG.ZoneEnable) DrawRange("SPEC", m_USL, m_LSL);
                        if (m_oXBAR_CTRL_CFG.ZoneEnable) DrawRange(XBAR, m_XBAR_UCL, m_XBAR_LCL);
                        if (m_oXBAR_3SIGMA_CFG.ZoneEnable) DrawRange("3SIGMA", m_XBAR_3S_UCL, m_XBAR_3S_LCL);

                        poY += poH;
                    }

                    if (m_SIGMA_CHART.ChartVisible)
                    {
                        DrawSIGMA(CombinationColumn, CombinationList,new ElementPosition(poX, poY, poW, poH));
                        if (m_oSIGMA_CFG.ZoneEnable) DrawRange(SIGMA, m_SIGMA_UCL, m_SIGMA_LCL);
                        poY += poH;
                    }

                    if (m_RANGE_CHART.ChartVisible)
                    {
                        DrawRANGE(CombinationColumn, CombinationList,new ElementPosition(poX, poY, poW, poH));
                        if (m_oRANGE_CFG.ZoneEnable) DrawRange(RANGE, m_RANGE_UCL, m_RANGE_LCL);
                        poY += poH;
                    }

                    if (m_RAW_CHART.ChartVisible)
                    {
                        DrawRAW(CombinationColumn, CombinationList,new ElementPosition(poX, poY, poW, poH));
                        if (m_oRAW_CFG.ZoneEnable) DrawRange(RAW, m_XBAR_UCL, m_XBAR_LCL);
                        poY += poH;
                    }

                    if (m_EWMA_MV_CHART.ChartVisible)
                    {
                        DrawEWMA_MV(CombinationColumn, CombinationList,new ElementPosition(poX, poY, poW, poH));
                        if (m_oEWMA_MV_CFG.ZoneEnable) DrawRange(EWMA_MV, m_EWMA_MV_UCL, m_EWMA_MV_LCL);
                        poY += poH;
                    }

                    if (m_EWMA_R_CHART.ChartVisible)
                    {
                        DrawEWMA_R(CombinationColumn, CombinationList,new ElementPosition(poX, poY, poW, poH));
                        if (m_oEWMA_R_CFG.ZoneEnable) DrawRange(EWMA_R, m_EWMA_R_UCL, m_EWMA_R_LCL);
                        poY += poH;
                    }

                    if (m_EWMA_S_CHART.ChartVisible)
                    {
                        DrawEWMA_S(CombinationColumn, CombinationList,new ElementPosition(poX, poY, poW, poH));
                        if (m_oEWMA_S_CFG.ZoneEnable) DrawRange(EWMA_S, m_EWMA_S_UCL, m_EWMA_S_LCL);
                        poY += poH;
                    }

                    if (m_MA_CHART.ChartVisible)
                    {
                        DrawMA(CombinationColumn, CombinationList,new ElementPosition(poX, poY, poW, poH));
                        if (m_oMA_CFG.ZoneEnable) DrawRange(MA, m_MA_UCL, m_MA_LCL);
                        poY += poH;
                    }

                    if (m_MS_CHART.ChartVisible)
                    {
                        DrawMS(CombinationColumn, CombinationList,new ElementPosition(poX, poY, poW, poH));
                        if (m_oMS_CFG.ZoneEnable) DrawRange(MS, m_MS_UCL, m_MS_LCL);
                        poY += poH;
                    }

                    if (chart1.ChartAreas.Count > 0)
                    {
                        chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                        chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

                        chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                        chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

                        chart1.ChartAreas[0].AxisX.ScaleView.Size = Math.Min(m_iCNT, 30);
                        chart1.ChartAreas[0].AxisX.ScaleView.Position = 0;
                        chart1.ChartAreas[0].AxisX.ScaleView.MinSize = 1;
                        chart1.ChartAreas[0].AxisY.ScaleView.MinSize = 1;
                    }
                    #endregion
                }
                else
                {
                    if (m_XBAR_CHART.ChartVisible)
                    {
                        DrawXBAR(new ElementPosition(poX, poY, poW, poH));
                        DrawInformation();

                        if (m_oSPEC_CFG.ZoneEnable) DrawRange("SPEC", m_USL, m_LSL);
                        if (m_oXBAR_CTRL_CFG.ZoneEnable) DrawRange(XBAR, m_XBAR_UCL, m_XBAR_LCL);
                        if (m_oXBAR_3SIGMA_CFG.ZoneEnable) DrawRange("3SIGMA", m_XBAR_3S_UCL, m_XBAR_3S_LCL);

                        poY += poH;
                    }

                    if (m_SIGMA_CHART.ChartVisible)
                    {
                        DrawSIGMA(new ElementPosition(poX, poY, poW, poH));
                        if (m_oSIGMA_CFG.ZoneEnable) DrawRange(SIGMA, m_SIGMA_UCL, m_SIGMA_LCL);
                        poY += poH;
                    }

                    if (m_RANGE_CHART.ChartVisible)
                    {
                        DrawRANGE(new ElementPosition(poX, poY, poW, poH));
                        if (m_oRANGE_CFG.ZoneEnable) DrawRange(RANGE, m_RANGE_UCL, m_RANGE_LCL);
                        poY += poH;
                    }

                    if (m_RAW_CHART.ChartVisible)
                    {
                        DrawRAW(new ElementPosition(poX, poY, poW, poH));
                        if (m_oRAW_CFG.ZoneEnable) DrawRange(RAW, m_XBAR_UCL, m_XBAR_LCL);
                        poY += poH;
                    }

                    if (m_EWMA_MV_CHART.ChartVisible)
                    {
                        DrawEWMA_MV(new ElementPosition(poX, poY, poW, poH));
                        if (m_oEWMA_MV_CFG.ZoneEnable) DrawRange(EWMA_MV, m_EWMA_MV_UCL, m_EWMA_MV_LCL);
                        poY += poH;
                    }

                    if (m_EWMA_R_CHART.ChartVisible)
                    {
                        DrawEWMA_R(new ElementPosition(poX, poY, poW, poH));
                        if (m_oEWMA_R_CFG.ZoneEnable) DrawRange(EWMA_R, m_EWMA_R_UCL, m_EWMA_R_LCL);
                        poY += poH;
                    }

                    if (m_EWMA_S_CHART.ChartVisible)
                    {
                        DrawEWMA_S(new ElementPosition(poX, poY, poW, poH));
                        if (m_oEWMA_S_CFG.ZoneEnable) DrawRange(EWMA_S, m_EWMA_S_UCL, m_EWMA_S_LCL);
                        poY += poH;
                    }

                    if (m_MA_CHART.ChartVisible)
                    {
                        DrawMA(new ElementPosition(poX, poY, poW, poH));
                        if (m_oMA_CFG.ZoneEnable) DrawRange(MA, m_MA_UCL, m_MA_LCL);
                        poY += poH;
                    }

                    if (m_MS_CHART.ChartVisible)
                    {
                        DrawMS(new ElementPosition(poX, poY, poW, poH));
                        if (m_oMS_CFG.ZoneEnable) DrawRange(MS, m_MS_UCL, m_MS_LCL);
                        poY += poH;
                    }

                    if (m_I_CHART.ChartVisible)
                    {
                        DrawI(new ElementPosition(poX, poY, poW, poH));
                        DrawInformationIMR();
                        // DrawRange(I, m_IMR_USL, m_IMR_LSL);
                        poY += poH;
                    }

                    if (m_MR_CHART.ChartVisible)
                    {
                        DrawMR(new ElementPosition(poX, poY, poW, poH));
                        poY += poH;
                    }

                    if (chart1.ChartAreas.Count > 0)
                    {
                        DrawLabel();

                        chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                        chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

                        chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                        chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

                        chart1.ChartAreas[0].AxisX.ScaleView.Size = Math.Min(m_iCNT, 30);
                        chart1.ChartAreas[0].AxisX.ScaleView.Position = 0;
                        chart1.ChartAreas[0].AxisX.ScaleView.MinSize = 1;
                        chart1.ChartAreas[0].AxisY.ScaleView.MinSize = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetHiddenPoint(int idx, bool Hidden = true)
        {
            try
            {
                long iDataSeq = m_VAL_DSEQ[idx - 1];
                int idxAll = m_ALL_DSEQ_IDX.IndexOf(iDataSeq);

                if (Hidden)
                {
                    m_dt.Rows[idxAll]["DISPLAY_FLAG"] = "N";
                }else{
                    m_dt.Rows[idxAll]["DISPLAY_FLAG"] = "Y";
                }
                m_dt.AcceptChanges();

                DataCal();
                DrawSPC();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetPointVisibleState(int idx)
        {
            try
            {
                long iDataSeq = m_VAL_DSEQ[idx - 1];
                int idxAll = m_ALL_DSEQ_IDX.IndexOf(iDataSeq);

                return (m_dt.Rows[idxAll]["DISPLAY_FLAG"].ToString() == "N");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Data Point의 Index 값으로 DataRow를 Return한다.
        /// </summary>
        /// <param name="idx">Data Point Index</param>
        /// <param name="IsAllData">Index를 전체 Data에서 Search 한다. </param>
        /// <returns></returns>
        public DataRow GetData(int idx,bool IsAllData = true)
        {
            try
            {
                long iDataSeq = m_VAL_DSEQ[idx-1];
                return GetData(iDataSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// DataSeq에 해당하는 DataRow를 Return한다.
        /// </summary>
        /// <param name="DataSeq"></param>
        /// <returns></returns>
        public DataRow GetData(long DataSeq)
        {
            try
            {
                int idxAll = m_ALL_DSEQ_IDX.IndexOf(DataSeq);
                return m_dt.Rows[idxAll];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Display된 Point에서의 Index를 Return한다.
        /// </summary>
        /// <returns></returns>
        public int GetSelectedIndex()
        {
            int i = 0;
            try
            {
                if (m_I_CHART.ChartVisible == true)
                {
                    if (double.IsNaN(chart1.ChartAreas[I].CursorX.Position)) return -1;
                    i = (int)chart1.ChartAreas[I].CursorX.Position;
                    return i;
                }

                if (double.IsNaN(chart1.ChartAreas[XBAR].CursorX.Position)) return -1;
                i = (int)chart1.ChartAreas[XBAR].CursorX.Position;
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 선택된 Point의 Data Seq를 Return한다.
        /// </summary>
        /// <returns></returns>
        public long GetSelectedDataSeq()
        {
            try
            {
                int idx = GetSelectedIndex();
                if (idx == -1) return -1;
                return m_VAL_DSEQ[idx-1];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 각 Point에 Information을 Display한다.
        /// </summary>
        /// <param name="TargetChartSeries"></param>
        private void DrawInformation(string TargetChartSeries = XBAR)
        {
            try
            {
                if(chart1.Series.IndexOf(TargetChartSeries) < 0) return;
                for (int i = 0; i < m_VAL_RESULT.Count; i++)
                {
                    /// 1. 판정결과를 Point에 표시하는 Option
                    if (m_ResultVisible) 
                    if (m_VAL_RESULT[i].Length > 0)
                    {
                        /// 1-1. 판정결과가 있는 Point는 별도의 Option으로 Drawing
                        chart1.Series[TargetChartSeries].Points[i].MarkerStyle = MarkerStyle.Star5;
                        chart1.Series[TargetChartSeries].Points[i].MarkerColor = Color.Red;
                        chart1.Series[TargetChartSeries].Points[i].MarkerBorderColor = Color.Red;
                        chart1.Series[TargetChartSeries].Points[i].Label = m_VAL_RESULT[i];
                        chart1.Series[TargetChartSeries].Points[i].LabelForeColor = Color.Red;
                    }

                    /// 2. Display Flag가 F로 되어 있는것도 표시하는 Option
                    if(m_HiddenPointVisible) 
                    if (m_VAL_VISIBLE[i] == false) 
                    {
                        /// 2-2. Display Flag가 있는  Point는 별도의 Marker Option으로 Drawing
                        chart1.Series[TargetChartSeries].Points[i].MarkerBorderColor = Color.Black;

                        if(chart1.Series[TargetChartSeries].Points[i].Label.Length>0)
                            chart1.Series[TargetChartSeries].Points[i].Label = chart1.Series[TargetChartSeries].Points[i].Label + "\n\r";

                        chart1.Series[TargetChartSeries].Points[i].Label = chart1.Series[TargetChartSeries].Points[i].Label + "[F]";
                        chart1.Series[TargetChartSeries].Points[i].LabelForeColor = Color.DarkCyan;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawInformationIMR(string TargetChartSeries = I)
        {
            try
            {
                if (chart1.Series.IndexOf(TargetChartSeries) < 0) return;
                for (int i = 0; i < m_VAL_RESULT_IMR.Count; i++)
                {
                    /// 1. 판정결과를 Point에 표시하는 Option
                    if (m_ResultVisible)
                        if (m_VAL_RESULT_IMR[i].Length > 0)
                        {
                            /// 1-1. 판정결과가 있는 Point는 별도의 Option으로 Drawing
                            chart1.Series[TargetChartSeries].Points[i].MarkerStyle = MarkerStyle.Star5;
                            chart1.Series[TargetChartSeries].Points[i].MarkerColor = Color.Red;
                            chart1.Series[TargetChartSeries].Points[i].MarkerBorderColor = Color.Red;
                            chart1.Series[TargetChartSeries].Points[i].Label = m_VAL_RESULT_IMR[i];
                            chart1.Series[TargetChartSeries].Points[i].LabelForeColor = Color.Red;
                        }

                    chart1.Series[TargetChartSeries].Points[i].ToolTip = chart1.Series[TargetChartSeries].Points[i].YValues[0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawRange(string TargetChartArea, List<double> Upper, List<double> Lower)
        {
            if (TargetChartArea != "SPEC" && TargetChartArea != "3SIGMA" && chart1.ChartAreas.IndexOf(TargetChartArea) < 0) return;

            string str_RNG_NAME = TargetChartArea + "RNG";

            chart1.Series.Insert(0, new Series(str_RNG_NAME));

            chart1.Series[str_RNG_NAME].ChartType = SeriesChartType.RangeColumn;
            chart1.Series[str_RNG_NAME].Points.DataBindY(Upper, Lower);

            if (TargetChartArea == "SPEC" || TargetChartArea == "3SIGMA")
            {
                if (chart1.ChartAreas.IndexOf(XBAR) > -1)
                {
                    chart1.Series[str_RNG_NAME].ChartArea = XBAR;
                    chart1.ChartAreas[XBAR].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                }
            }
            else
            {
                chart1.Series[str_RNG_NAME].ChartArea = TargetChartArea;
                chart1.ChartAreas[TargetChartArea].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            }

            chart1.Series[str_RNG_NAME]["PointWidth"] = "1";
            chart1.Series[str_RNG_NAME]["DrawSideBySide"] = "false";
        }


        private void chart1_PostPaint(object sender, ChartPaintEventArgs e)
        {
            try
            {
                Series series = (Series)e.ChartElement;
                if (series.Name == XBAR)
                {
                    if (m_oSPEC_CFG.Enable) DrawLimitLine(e, m_oSPEC_CFG, m_USL, m_LSL);
                    if (m_oXBAR_CTRL_CFG.Enable) DrawLimitLine(e, m_oXBAR_CTRL_CFG, m_XBAR_UCL, m_XBAR_LCL);
                    if (m_oXBAR_3SIGMA_CFG.Enable) DrawLimitLine(e, m_oXBAR_3SIGMA_CFG, m_XBAR_3S_UCL, m_XBAR_3S_LCL);
                }

                if (m_oSIGMA_CFG.Enable && series.Name == SIGMA) DrawLimitLine(e, m_oSIGMA_CFG, m_SIGMA_UCL, m_SIGMA_LCL);
                if (m_oRAW_CFG.Enable && series.Name == RAW) DrawLimitLine(e, m_oRAW_CFG, m_XBAR_UCL, m_XBAR_LCL);
                if (m_oRANGE_CFG.Enable && series.Name == RANGE) DrawLimitLine(e, m_oRANGE_CFG, m_RANGE_UCL, m_RANGE_LCL);
                if (m_oEWMA_MV_CFG.Enable && series.Name == EWMA_MV) DrawLimitLine(e, m_oEWMA_MV_CFG, m_EWMA_MV_UCL, m_EWMA_MV_LCL);
                if (m_oEWMA_R_CFG.Enable && series.Name == EWMA_R) DrawLimitLine(e, m_oEWMA_R_CFG, m_EWMA_R_UCL, m_EWMA_R_LCL);
                if (m_oEWMA_S_CFG.Enable && series.Name == EWMA_S) DrawLimitLine(e, m_oEWMA_S_CFG, m_EWMA_S_UCL, m_EWMA_S_LCL);
                if (m_oMA_CFG.Enable && series.Name == MA) DrawLimitLine(e, m_oMA_CFG, m_MA_UCL, m_MA_LCL);
                if (m_oMS_CFG.Enable && series.Name == MS) DrawLimitLine(e, m_oMS_CFG, m_MS_UCL, m_MS_UCL);

                if (m_oISPEC_CFG.Enable && series.Name == I) DrawLimitLine(e, m_oISPEC_CFG, m_IMR_USL, m_IMR_LSL);
                if (m_oICONTROL_CFG.Enable && series.Name == I) DrawLimitLine(e, m_oICONTROL_CFG, m_IMR_UCL, m_IMR_LCL);

            }
            catch
            {
                ///오류 무시
                ///  e.ChartElement 가 Series외에 ChartArea등으로 나올 수 있음
            }
        }

        private void ORG_DrawLimitLine(ChartPaintEventArgs e, LineConfig LCfg, double[] UValue, double[] LValue,double[] tValue = null)
        {
             
            if (e.ChartElement is Series)
            {
                if (UValue == null && LValue == null) return;
                GraphicsPath gpUSL = new GraphicsPath();
                GraphicsPath gpLSL = new GraphicsPath();
                Series series = (Series)e.ChartElement;
                System.Drawing.PointF posUSL = System.Drawing.PointF.Empty;
                System.Drawing.PointF posLSL = System.Drawing.PointF.Empty;
                System.Drawing.PointF tmpPosition = System.Drawing.PointF.Empty;
                float offset = 0f;
                // Find data point with label "Product F".
                int i = 0;
                bool isFirst_USL = true;
                bool isFirst_LSL = true;
                foreach (DataPoint point in series.Points)
                {
                    if (chart1.ChartAreas[0].AxisX.ScaleView.Position > i+1)
                    {
                        i++;
                        continue;
                    }

                    // Get relative coordinates of the data point values found.
                    if (double.IsNaN(UValue[i]) == false)
                    {
                        posUSL.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, i);
                        posUSL.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, UValue[i]);
                        posUSL = e.ChartGraphics.GetAbsolutePoint(posUSL);
                    }

                    if (double.IsNaN(LValue[i]) == false)
                    {
                        posLSL.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, i);
                        posLSL.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, LValue[i]);
                        posLSL = e.ChartGraphics.GetAbsolutePoint(posLSL);
                    }

                    if (double.IsNaN(UValue[i]) == false)
                    {
                        if (isFirst_USL)
                        {
                            // 처음 값일때
                            tmpPosition.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, i + 1);
                            tmpPosition = e.ChartGraphics.GetAbsolutePoint(tmpPosition);
                                offset = (tmpPosition.X - posUSL.X) / 2f;
                                gpUSL.AddLine(posUSL.X, posUSL.Y, posUSL.X + offset * 3, posUSL.Y);
                            isFirst_USL = false;
                        }
                        else
                        {
                            // 중간 값일때
                            gpUSL.AddLine(posUSL.X + offset, posUSL.Y, posUSL.X + offset * 3, posUSL.Y);

                            // 마지막 값일때
                            if (i == series.Points.Count - 1)
                            {
                                gpUSL.AddLine(posUSL.X + offset * 3, posUSL.Y, posUSL.X + offset * 4, posUSL.Y);
                            }
                        }
                    }


                    if (double.IsNaN(LValue[i]) == false)
                    {
                        if (isFirst_LSL)
                        {
                            // 처음 값일때
                            tmpPosition.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, i + 1);
                            tmpPosition = e.ChartGraphics.GetAbsolutePoint(tmpPosition);

                            offset = (tmpPosition.X - posLSL.X) / 2f;
                            gpLSL.AddLine(posLSL.X, posLSL.Y, posLSL.X + offset * 3, posLSL.Y);
                            isFirst_LSL = false;
                        }
                        else
                        {
                            // 중간 값일때
                            gpLSL.AddLine(posLSL.X + offset, posLSL.Y, posLSL.X + offset * 3, posLSL.Y);

                            // 마지막 값일때
                            if (i == series.Points.Count - 1)
                            {
                                gpLSL.AddLine(posLSL.X + offset * 3, posLSL.Y, posLSL.X + offset * 4, posLSL.Y);
                            }
                        }
                    }
                    i++;
                }

                e.ChartGraphics.Graphics.DrawPath(LCfg.LinePen, gpUSL);
                e.ChartGraphics.Graphics.DrawPath(LCfg.LinePen, gpLSL);
            }
        }

        private void DrawLimitLine(ChartPaintEventArgs e, LineConfig LCfg, List<double> UValue, List<double> LValue, List<double> tValue = null)
        {
            try
            {
                if (e.ChartElement is Series)
                {
                    if (UValue == null && LValue == null) return;
                    GraphicsPath gpUSL = new GraphicsPath();
                    GraphicsPath gpLSL = new GraphicsPath();
                    Series series = (Series)e.ChartElement;
                    System.Drawing.PointF posUSL = System.Drawing.PointF.Empty;
                    System.Drawing.PointF posLSL = System.Drawing.PointF.Empty;
                    System.Drawing.PointF LastPosUSL = System.Drawing.PointF.Empty;
                    System.Drawing.PointF LastPosLSL = System.Drawing.PointF.Empty;
                    float offset = 0f;
                    // Find data point with label "Product F".
                    //int i = 0;
                    bool isFirst_USL = true;
                    bool isFirst_LSL = true;
                    double dUVal = double.NaN;
                    double dLVal = double.NaN;

                    int st = (int)chart1.ChartAreas[series.Name].AxisX.ScaleView.ViewMinimum; 
                    int end = (int)chart1.ChartAreas[series.Name].AxisX.ScaleView.ViewMaximum;

                    for (int idx = st; idx < end-1; idx++)
                    {
                        dUVal = UValue[idx];
                        dLVal = LValue[idx];

                        // Get relative coordinates of the data point values found.
                        if (double.IsNaN(dUVal) == false)
                        {
                            posUSL.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, idx + 1);
                            posUSL.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, dUVal);
                            posUSL = e.ChartGraphics.GetAbsolutePoint(posUSL);
                            if (isFirst_USL)
                            {
                                //a. 처음 값일때
                                if (idx <= 0)
                                {
                                    //a-1. 화면에 Drawing할 첫값이고, 전체 Data에서도 첫값이기에 이값 앞의 값이 존재하지 않아 현재값을 앞에까지 연장하여 Drawing 해야 함
                                    LastPosUSL.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, idx);
                                    LastPosUSL.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, dUVal);
                                }
                                else
                                {
                                    //a-2. 화면에 Drawing할 첫값이고, 이값 앞에 값이 존재하므로 그(보이지 않는 앞) 값을 이어서 Spec으로 Drawing 해야 함
                                    LastPosUSL.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, idx);
                                    LastPosUSL.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, UValue[idx - 1]);
                                }
                                LastPosUSL = e.ChartGraphics.GetAbsolutePoint(LastPosUSL);
                                offset = (posUSL.X - LastPosUSL.X) / 2f;
                                gpUSL.AddLine(LastPosUSL.X, LastPosUSL.Y, LastPosUSL.X + offset, LastPosUSL.Y);
                                isFirst_USL = false;
                            }
                            //b. 중간 값일때
                            gpUSL.AddLine(LastPosUSL.X + offset, LastPosUSL.Y, posUSL.X - offset, posUSL.Y);
                            gpUSL.AddLine(posUSL.X - offset, posUSL.Y, posUSL.X + offset, posUSL.Y);
                            LastPosUSL = posUSL;

                        }

                        if (double.IsNaN(dLVal) == false)
                        {
                            posLSL.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, idx + 1);
                            posLSL.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, dLVal);
                            posLSL = e.ChartGraphics.GetAbsolutePoint(posLSL);

                            if (isFirst_LSL)
                            {
                                //a. 처음 값일때
                                if (idx <= 0)
                                {
                                    //a-1. 화면에 Drawing할 첫값이고, 전체 Data에서도 첫값이기에 이값 앞의 값이 존재하지 않아 현재값을 앞에까지 연장하여 Drawing 해야 함
                                    LastPosLSL.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, idx);
                                    LastPosLSL.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, dLVal);
                                }
                                else
                                {
                                    //a-2. 화면에 Drawing할 첫값이고, 이값 앞에 값이 존재하므로 그(보이지 않는 앞) 값을 이어서 Spec으로 Drawing 해야 함
                                    LastPosLSL.X = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, idx);
                                    LastPosLSL.Y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, LValue[idx - 1]);
                                }
                                LastPosLSL = e.ChartGraphics.GetAbsolutePoint(LastPosLSL);
                                offset = (posLSL.X - LastPosLSL.X) / 2f;
                                gpLSL.AddLine(LastPosLSL.X, LastPosLSL.Y, LastPosLSL.X + offset, LastPosLSL.Y);
                                isFirst_LSL = false;
                            }
                            //b. 중간 값일때
                            gpLSL.AddLine(LastPosLSL.X + offset, LastPosLSL.Y, posLSL.X - offset, posLSL.Y);
                            gpLSL.AddLine(posLSL.X - offset, posLSL.Y, posLSL.X + offset, posLSL.Y);
                            LastPosLSL = posLSL;
                        }
                    }

                    if(LastPosUSL.X != 0 || LastPosUSL.Y != 0)
                        gpUSL.AddLine(LastPosUSL.X + offset, LastPosUSL.Y, LastPosUSL.X + offset * 2, LastPosUSL.Y);

                    if (LastPosLSL.X != 0 || LastPosLSL.Y != 0)
                        gpLSL.AddLine(LastPosLSL.X + offset, LastPosLSL.Y, LastPosLSL.X + offset * 2, LastPosLSL.Y);

                  
                    e.ChartGraphics.Graphics.DrawPath(LCfg.LinePen, gpUSL);
                    e.ChartGraphics.Graphics.DrawPath(LCfg.LinePen, gpLSL);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawBoxPlot()
        {
            chart1.ChartAreas["Default"].Visible = true;

            chart1.ChartAreas.Add("Box");
            chart1.Series.Add("BoxPlotSeries");
            chart1.ChartAreas["Box"].AlignmentOrientation = AreaAlignmentOrientations.Vertical;

            // Set the chart area position for the first chart area.
            chart1.ChartAreas["Default"].Position.Auto = false;
            //chart1.ChartAreas["Default"].Position.X = 5;
            //chart1.ChartAreas["Default"].Position.Y = 10;
            chart1.ChartAreas["Default"].Position.Width = 90;
            chart1.ChartAreas["Default"].Position.Height = 100;

            // Set the chart area position for the second chart area.
            chart1.ChartAreas["Box"].Position.Auto = false;
            chart1.ChartAreas["Box"].Position.X = 91;
            chart1.ChartAreas["Box"].Position.Y = chart1.ChartAreas["Default"].Position.Y;
            chart1.ChartAreas["Box"].Position.Width = 9;
            chart1.ChartAreas["Box"].Position.Height = 100;
            chart1.ChartAreas["Box"].AxisY.IsStartedFromZero = false;

            //double[] yValues = {55.62, 45.54, 73.45, 9.73, 88.42, 45.9, 63.6, 85.1, 67.2, 23.6}; 
            //chart1.Series["DataSeries"].Points.DataBindY(yValues);



            chart1.Series["BoxPlotSeries"].ChartArea = "Box";

            // Set Box Plot chart type
            chart1.Series["BoxPlotSeries"].ChartType = SeriesChartType.BoxPlot;

            // Specify data series name for the Box Plot
            chart1.Series["BoxPlotSeries"]["BoxPlotSeries"] = "Series1";

            //chart1.Series["나"]["BoxPlotSeries"] = "Series1";
            //Double[] data = new Double[10];
            //Random random = new Random();
            //for (int i = 0; i < 10; i++)
            //{
            //    data[i] = random.Next(45, 95);
            //}
            //chart1.Series["나"].Points.DataBindY(data);

            // Set whiskers percentile
            chart1.Series["BoxPlotSeries"]["BoxPlotWhiskerPercentile"] = "7.5";

            // Set box percentile
            chart1.Series["BoxPlotSeries"]["BoxPlotPercentile"] = "25";

            // Hide Average line
            chart1.Series["BoxPlotSeries"]["BoxPlotShowAverage"] = "false";

            // Show/Hide Median line
            chart1.Series["BoxPlotSeries"]["BoxPlotShowMedian"] = "true";
            chart1.Series["BoxPlotSeries"]["BoxSize"] = "50";

            // Show Unusual points
            chart1.Series["BoxPlotSeries"]["BoxPlotShowUnusualValues"] = "true";

            //double Q1 = chart1.Series["나"].Points[2].XValue;
            //double Q3 = chart1.Series["나"].Points[3].YValues[0];
            //double innF = Q1 - 1.5 * (Q3 - Q1);
            //double innF2 = Q3 + 1.5 * (Q3 - Q1);
            chart1.Update();

        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Right)
            {
                return false;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void chart1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Left))
            {
                ProcessSelect(e);
            }
        }

        private void ProcessSelect(System.Windows.Forms.KeyEventArgs e)
        {
            // Process keyboard keys
            if (e.KeyCode == Keys.Right)
            {
                // Set the new cursor position 
                chart1.ChartAreas[0].CursorX.Position += chart1.ChartAreas[0].CursorX.Interval;
            }
            else if (e.KeyCode == Keys.Left)
            {
                // Set the new cursor position 
                chart1.ChartAreas[0].CursorX.Position -= chart1.ChartAreas[0].CursorX.Interval;
            }

            if (chart1.ChartAreas[0].Name == "I")
                SetViewIMR();
            else
                SetView();
        }

        private void SetView()
        {
            // keep the cursor from leaving the max and min a axis points
            if (chart1.ChartAreas[0].CursorX.Position < 1)
                chart1.ChartAreas[0].CursorX.Position = 1;

            else if (chart1.ChartAreas[0].CursorX.Position > m_VAL_XBAR.Count)
                chart1.ChartAreas[0].CursorX.Position = m_VAL_XBAR.Count;


            // move the view to keep the cursor visible
            if (chart1.ChartAreas[0].CursorX.Position < chart1.ChartAreas[0].AxisX.ScaleView.Position)
                chart1.ChartAreas[0].AxisX.ScaleView.Position = chart1.ChartAreas[0].CursorX.Position;

            else if ((chart1.ChartAreas[0].CursorX.Position >
                (chart1.ChartAreas[0].AxisX.ScaleView.Position + chart1.ChartAreas[0].AxisX.ScaleView.Size)))
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Position =
                    (chart1.ChartAreas[0].CursorX.Position - chart1.ChartAreas[0].AxisX.ScaleView.Size);
            }

            if(OnSelectedPoint != null)
            {
                int idx =  GetSelectedIndex(); 
                OnSelectedPoint(this, new int[] {idx}, new long[] {m_VAL_DSEQ[idx-1]});
            }
        }

        private void SetViewIMR()
        {
            // keep the cursor from leaving the max and min a axis points
            if (chart1.ChartAreas[0].CursorX.Position < 1)
                chart1.ChartAreas[0].CursorX.Position = 1;

            else if (chart1.ChartAreas[0].CursorX.Position > m_VAL_I.Count)
                chart1.ChartAreas[0].CursorX.Position = m_VAL_I.Count;


            // move the view to keep the cursor visible
            if (chart1.ChartAreas[0].CursorX.Position < chart1.ChartAreas[0].AxisX.ScaleView.Position)
                chart1.ChartAreas[0].AxisX.ScaleView.Position = chart1.ChartAreas[0].CursorX.Position;

            else if ((chart1.ChartAreas[0].CursorX.Position >
                (chart1.ChartAreas[0].AxisX.ScaleView.Position + chart1.ChartAreas[0].AxisX.ScaleView.Size)))
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Position =
                    (chart1.ChartAreas[0].CursorX.Position - chart1.ChartAreas[0].AxisX.ScaleView.Size);
            }

            if (OnSelectedPoint != null)
            {
                int idx = GetSelectedIndex();
                OnSelectedPoint(this, new int[] { idx }, new long[] { m_VAL_DSEQ[idx - 1] });
            }
        }

        private void ProcessScroll(System.Windows.Forms.KeyEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {
            try
            {
                chart1.Focus();
                if (OnSelectedPoint != null)
                {
                    int idx = GetSelectedIndex();
                    if (idx > 0 && idx < m_VAL_DSEQ.Count)
                        OnSelectedPoint(this, new int[] { idx }, new long[] { m_VAL_DSEQ[idx - 1] });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toggleInternalFlagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool bVisible = false;
                int idx = GetSelectedIndex();

                if (idx < 0 || idx >= m_VAL_VISIBLE.Count) return;
                
                if (m_VAL_VISIBLE[idx-1] == false)
                {
                    bVisible = true;
                }
                else
                {
                    bVisible = false;
                }

                SetHiddenPoint(idx, !(bVisible) );
                if (OnChangePointVisible != null) OnChangePointVisible(this, idx, m_VAL_DSEQ[idx-1], bVisible);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSPCChartConfig oCfgForm = new frmSPCChartConfig(true);
            oCfgForm.OnApplyReflash += new ApplyReflash(oCfgForm_OnApplyReflash);

            oCfgForm.XBAR_CHART_CFG = XBAR_CHART_CFG;
            oCfgForm.SIGMA_CHART_CFG = SIGMA_CHART_CFG;
            oCfgForm.RANGE_CHART_CFG = RANGE_CHART_CFG;
            oCfgForm.RAW_CHART_CFG = RAW_CHART_CFG;

            oCfgForm.EWMA_MV_CHART_CFG = EWMA_MV_CHART_CFG;
            oCfgForm.EWMA_S_CHART_CFG = EWMA_S_CHART_CFG;
            oCfgForm.EWMA_R_CHART_CFG = EWMA_R_CHART_CFG;
            oCfgForm.MA_CHART_CFG = MA_CHART_CFG;
            oCfgForm.MS_CHART_CFG = MS_CHART_CFG;


            if (oCfgForm.ShowDialog() == DialogResult.OK)
            {
                m_XBAR_CHART = oCfgForm.XBAR_CHART_CFG;

                m_SIGMA_CHART = oCfgForm.SIGMA_CHART_CFG;
                m_RANGE_CHART = oCfgForm.RANGE_CHART_CFG;
                m_RAW_CHART = oCfgForm.RAW_CHART_CFG;

                m_EWMA_MV_CHART = oCfgForm.EWMA_MV_CHART_CFG;
                m_EWMA_S_CHART = oCfgForm.EWMA_S_CHART_CFG;
                m_EWMA_R_CHART = oCfgForm.EWMA_R_CHART_CFG;
                m_MA_CHART = oCfgForm.MA_CHART_CFG;
                m_MS_CHART = oCfgForm.MS_CHART_CFG;

                DrawSPC();
            }
        }

        void oCfgForm_OnApplyReflash(object sender)
        {
            frmSPCChartConfig oCfgForm = (frmSPCChartConfig)sender;
            m_XBAR_CHART = oCfgForm.XBAR_CHART_CFG;

            m_SIGMA_CHART = oCfgForm.SIGMA_CHART_CFG;
            m_RANGE_CHART = oCfgForm.RANGE_CHART_CFG;
            m_RAW_CHART = oCfgForm.RAW_CHART_CFG;

            m_EWMA_MV_CHART = oCfgForm.EWMA_MV_CHART_CFG;
            m_EWMA_S_CHART = oCfgForm.EWMA_S_CHART_CFG;
            m_EWMA_R_CHART = oCfgForm.EWMA_R_CHART_CFG;
            m_MA_CHART = oCfgForm.MA_CHART_CFG;
            m_MS_CHART = oCfgForm.MS_CHART_CFG;

            DrawSPC();
        }

        private void fitSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FitAllChart = true;
            DrawSPC();
        }

        private void selectDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OnSelectData != null) OnSelectData(this);
        }

        private void zoomBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();
        }

        private void copyToImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = null;
            try{
                bmp = new Bitmap(this.Width, this.Height);
                this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
                Clipboard.Clear();
                Clipboard.SetImage(bmp);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    internal class SPCChartDesigner : ControlDesigner
    {
        public SPCChartDesigner()
        {
            this.AddVerbs();
        }

        private void AddVerbs()
        {
            this.Verbs.Clear();
            this.Verbs.Add(new DesignerVerb("SPC Chart Design", new EventHandler(this.OnEdit)));
        }

        private void OnEdit(object sender, EventArgs e)
        {
            EditorServiceContext.EditValue(this, this.Control);

            //frmSPCChartConfig oCfg = new frmSPCChartConfig();
            //oCfg.XBAR_CHART_CFG = ((SPCChart)this.Control).XBAR_CHART_CFG;
            //oCfg.ShowDialog();
            //((SPCChart)this.Control).XBAR_CHART_CFG = oCfg.XBAR_CHART_CFG;

        }

        internal class EditorServiceContext : IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
        {
            private IComponentChangeService componentChangeSvc;
            private ComponentDesigner designer;
            private PropertyDescriptor targetProperty;

            private EditorServiceContext(ComponentDesigner designer, PropertyDescriptor targetProperty)
            {
                this.designer = designer;
                this.targetProperty = targetProperty;
            }

            public static void EditValue(ComponentDesigner designer, object control)
            {
                /// 1. PropertyDescriptor를 이용해 지정 속성의 값을 가져오거나 설정합니다.
                PropertyDescriptorCollection prop = TypeDescriptor.GetProperties(control);

                /// 2. SPCChartDesigner 의 EditValue를 호출할때 필요한 인터페이스를 구현한 클래스 입니다.
                ///    여기서는 본클래스에 모든 구현이 있으므로 자기 자신 입니다.
                //EditorServiceContext context = new EditorServiceContext(designer, prop);


                ///// Items의 편집기를 얻어 옵니다.
                ///// 
                //UITypeEditor editor = prop.GetEditor(typeof(UITypeEditor)) as UITypeEditor;

                ////Items를 얻어 옵니다.
                //object origin = prop.GetValue(control);

                //////UITypeEditor를 통해 편집 합니다.
                //object change = editor.EditValue(context, context, origin);

                //////변경 되었을 경우만 반영 합니다.
                //if (change != origin)
                //    prop.SetValue(control, change);

                frmSPCChartConfig oCfgForm = new frmSPCChartConfig();

                oCfgForm.XBAR_CHART_CFG = ((SPCChart)control).XBAR_CHART_CFG;
                oCfgForm.SIGMA_CHART_CFG = ((SPCChart)control).SIGMA_CHART_CFG;
                oCfgForm.RANGE_CHART_CFG = ((SPCChart)control).RANGE_CHART_CFG;
                oCfgForm.RAW_CHART_CFG = ((SPCChart)control).RAW_CHART_CFG;

                oCfgForm.EWMA_MV_CHART_CFG = ((SPCChart)control).EWMA_MV_CHART_CFG;
                oCfgForm.EWMA_S_CHART_CFG = ((SPCChart)control).EWMA_S_CHART_CFG;
                oCfgForm.EWMA_R_CHART_CFG = ((SPCChart)control).EWMA_R_CHART_CFG;
                oCfgForm.MA_CHART_CFG = ((SPCChart)control).MA_CHART_CFG;
                oCfgForm.MS_CHART_CFG = ((SPCChart)control).MS_CHART_CFG;


                if (oCfgForm.ShowDialog() == DialogResult.OK)
                {
                    //prop.SetValue(((SPCChart)control).XBAR_CHART_CFG, oCfgForm.XBAR_CHART_CFG);
                    //prop.SetValue(((SPCChart)control).SIGMA_CHART_CFG, oCfgForm.SIGMA_CHART_CFG);
                    //prop.SetValue(((SPCChart)control).RANGE_CHART_CFG, oCfgForm.RANGE_CHART_CFG);
                    //prop.SetValue(((SPCChart)control).RAW_CHART_CFG, oCfgForm.RAW_CHART_CFG);

                    //prop.SetValue(((SPCChart)control).EWMA_MV_CHART_CFG, oCfgForm.EWMA_MV_CHART_CFG);
                    //prop.SetValue(((SPCChart)control).EWMA_S_CHART_CFG, oCfgForm.EWMA_S_CHART_CFG);
                    //prop.SetValue(((SPCChart)control).EWMA_R_CHART_CFG, oCfgForm.EWMA_R_CHART_CFG);
                    //prop.SetValue(((SPCChart)control).MA_CHART_CFG, oCfgForm.MA_CHART_CFG);
                    //prop.SetValue(((SPCChart)control).MS_CHART_CFG, oCfgForm.MS_CHART_CFG);

                    //((SPCChart)control).XBAR_CHART_CFG = oCfgForm.XBAR_CHART_CFG;
                    //((SPCChart)control).SIGMA_CHART_CFG = oCfgForm.SIGMA_CHART_CFG;
                    //((SPCChart)control).RANGE_CHART_CFG = oCfgForm.RANGE_CHART_CFG;
                    //((SPCChart)control).RAW_CHART_CFG = oCfgForm.RAW_CHART_CFG;

                    //((SPCChart)control).EWMA_MV_CHART_CFG = oCfgForm.EWMA_MV_CHART_CFG;
                    //((SPCChart)control).EWMA_S_CHART_CFG = oCfgForm.EWMA_S_CHART_CFG;
                    //((SPCChart)control).EWMA_R_CHART_CFG = oCfgForm.EWMA_R_CHART_CFG;
                    //((SPCChart)control).MA_CHART_CFG = oCfgForm.MA_CHART_CFG;
                    //((SPCChart)control).MS_CHART_CFG = oCfgForm.MS_CHART_CFG;

                    prop["XBAR_CHART_CFG"].SetValue(control, oCfgForm.XBAR_CHART_CFG);

                    prop["SIGMA_CHART_CFG"].SetValue(control, oCfgForm.SIGMA_CHART_CFG);
                    prop["RANGE_CHART_CFG"].SetValue(control, oCfgForm.RANGE_CHART_CFG);
                    prop["RAW_CHART_CFG"].SetValue(control, oCfgForm.RAW_CHART_CFG);

                    prop["EWMA_MV_CHART_CFG"].SetValue(control, oCfgForm.EWMA_MV_CHART_CFG);
                    prop["EWMA_S_CHART_CFG"].SetValue(control, oCfgForm.EWMA_S_CHART_CFG);
                    prop["EWMA_R_CHART_CFG"].SetValue(control, oCfgForm.EWMA_R_CHART_CFG);
                    prop["MA_CHART_CFG"].SetValue(control, oCfgForm.MA_CHART_CFG);
                    prop["MS_CHART_CFG"].SetValue(control, oCfgForm.MS_CHART_CFG);
                }
            }

            public void OnComponentChanged()
            {
                this.ChangeService.OnComponentChanged(this.designer.Component, this.targetProperty, null, null);
            }

            public bool OnComponentChanging()
            {
                try
                {
                    this.ChangeService.OnComponentChanging(this.designer.Component, this.targetProperty);
                }
                catch (CheckoutException exception)
                {
                    if (exception != CheckoutException.Canceled)
                    {
                        throw;
                    }
                    return false;
                }
                return true;

            }


            public object GetService(Type serviceType)
            {
                if ((serviceType == typeof(ITypeDescriptorContext)) || (serviceType == typeof(IWindowsFormsEditorService)))
                {
                    return this;
                }
                if (this.designer.Component.Site != null)
                {
                    return this.designer.Component.Site.GetService(serviceType);
                }
                return null;
            }

            public void CloseDropDown()
            {
            }

            public void DropDownControl(Control control)
            {
            }

            public DialogResult ShowDialog(Form dialog)
            {
                IUIService service = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));
                if (service != null)
                {
                    return service.ShowDialog(dialog);
                }
                return dialog.ShowDialog(this.designer.Component as IWin32Window);
            }

            private IComponentChangeService ChangeService
            {
                get
                {
                    if (this.componentChangeSvc == null)
                    {
                        this.componentChangeSvc = (IComponentChangeService)((IServiceProvider)this).GetService(typeof(IComponentChangeService));
                    }
                    return this.componentChangeSvc;
                }
            }


            public IContainer Container
            {
                get
                {
                    if (this.designer.Component.Site != null)
                    {
                        return this.designer.Component.Site.Container;
                    }
                    return null;
                }

            }

            public object Instance
            {
                get
                {
                    return this.designer.Component;
                }
            }


            public PropertyDescriptor PropertyDescriptor
            {
                get
                {
                    return this.targetProperty;
                }
            }




        }
    }
}
