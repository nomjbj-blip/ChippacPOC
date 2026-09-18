using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using DACrux.Base;
using System.Threading;

namespace DACrux.Map
{
    /// <summary>
    /// WaferMap에 대한 요약 설명입니다.
    /// </summary>
    /// 
    public enum FocusType { Arraw, Close };
    public enum MapMode { Fit, Free, Edit };
    public enum MouseDragMode { Normal, Zoom, Move, Rotate, Zone, Shot, Defect };
    public enum MapSelectStyle { FreeHand, Circle, Band, Pie, Rectangle };
    public delegate void SelectDies(object sender, List<Point> selectedDies);

    /// <summary>
    /// Mouse의 현재 위치한 Die가 이전 Die의 Index와 바뀌었을때 호출되는 Event의 대리자
    /// </summary>
    public delegate void ChangeCurrentDie(object sender, Die NewDie);
    /// <summary>
    /// Mouse의 Real Position과 절대 Position을 전달하는 Event의 대리자
    /// </summary>
    public delegate void ChangePosition(object sender, PointD Currpoint, PointD RealPoint);

    public partial class WaferMap : UserControl, IDataGrid, IShot
    {
        public const int DIE_PROP_VIRTUAL_DIE = 999;

        public event EventHandler ShotOptionChanged;

        private bool m_isDrawing = false;
        protected double m_dScale = 1.0d;
        protected Rectangle m_rectSelect = Rectangle.Empty;
        protected Rectangle m_rectSelect2 = Rectangle.Empty;

        protected Point m_poStart = Point.Empty;
        protected Point m_poEnd = Point.Empty;
        protected Bitmap m_bmpWaferMap;
        protected Bitmap m_bmpTemp;
        protected Bitmap m_bmpWaferID;
        protected Bitmap m_bmpFocus;
        protected RectangleD m_rectdWaferArea;
        protected MapMode m_eoMapMode = MapMode.Free;
        protected MouseDragMode m_eoMouseDragMode = MouseDragMode.Zoom;
        protected MapSelectStyle m_eoMapSelectStyle = MapSelectStyle.Rectangle;

        protected GraphicsPath m_SelectPath = null;
        protected double m_dZoomRatio = 1.0d;
        protected int m_iViewAngle = 0;
        protected int m_iAngleOffset = 0;

        protected Color[] m_ColorSet;
        protected Color[] m_VIColorSet = null;
        protected DieList m_arrDies;
        protected List<Point> m_DieIndexer = null;

        //2019-06-07-추가
        protected DieList m_arrModifyDies;
        protected List<Point> m_DieModifyIndexer = null;

        protected Graphics m_gdiMain = null;
        protected Graphics m_gdiTempMap = null;

        protected Position m_poInformation = Position.LeftTop;
        protected bool m_bVisibleInfo = true;
        protected List<Point> m_SelectedDies = null;
        protected bool m_bVisibleSignDies = false;
        protected List<Point> m_SignDies = new List<Point>();
        protected Pen m_bVisibleSignLine = new Pen(Color.White, 5);
        protected List<string> m_strInfomation = null;
        protected Point m_FocusDie = new Point(99999, 99999);

        protected bool m_bVisibleDieValue = false;
        protected bool m_bVisibleVIFaile = false;
        protected bool m_bVisibleXY = false;
        protected bool m_bVisibleFocusDie = false;

        protected FocusType m_ftFocus = FocusType.Arraw;
        protected string m_strVIMember = "VIFAIL";

        //Defect Count 별로 생상 표현 추가
        protected ItemCountColorList m_ItemCountColorList = null;

        #region Wafer속성 Member변수

        /// <summary>
        /// Region 속성
        /// </summary>
        protected double m_dNotchSize = 30.0d;
        protected double m_dMargin = 0.95D;


        // Draw속성
        protected Color m_colWafer = Color.Gray;
        protected Color m_colWaferBorder = Color.LightGray;
        protected Color m_colEdge = Color.LightGray;
        protected bool m_bCenterMark = false;
        protected bool m_bScale = false;
        #endregion

        #region Die속성 Member 변수

        protected Color m_colDieBorder = Color.LightGray;

        protected bool m_bDrawOriginDie = true;
        protected bool m_bDrawFirstDie = true;
        protected bool m_bDrawMarkDie = true;
        protected bool m_bDrawSkipDie = true;
        protected bool m_bVisibleOffDie = false;
        protected bool m_bStringBin = false;
        protected bool m_bDrawGradationDie = false;

        protected Color m_colOriginDieBorder = Color.Red;
        protected Color m_colFirstDieBorder = Color.SkyBlue;
        protected Color m_colMarkDieColor = Color.LightSkyBlue;
        protected Color m_colSkipDieColor = Color.Yellow;
        protected Color m_colFromGradationDieColor = Color.Lime;
        protected Color m_colToGradationDieColor = Color.Red;

        protected int m_nGradationInterval = 5;
        protected double m_dbGradationMaxValue = double.NaN;
        protected double m_dbGradationMinValue = double.NaN;
        protected string m_sParametricColumn = "PCMVALUE";

        #endregion

        protected WaferRecipe m_WaferRecipe;
        protected System.Windows.Forms.ContextMenu ctxmWaferMap;
        protected System.Windows.Forms.MenuItem mnuitemMAPSELECTSTYLE_CIRCLE;
        protected System.Windows.Forms.MenuItem mnuitemMAPSELECTSTYLE_PIE;
        protected System.Windows.Forms.MenuItem mnuitemMAPSELECTSTYLE_RECTANGLE;
        protected System.Windows.Forms.MenuItem mnuitemMAPSELECTSTYLE_FREEHAND;
        protected System.Windows.Forms.MenuItem mnuitemMAPSELECTSTYLE_BAND;
        protected HatchBrush m_hbSelectBrush = null;
        protected DataTable m_DT = null;

        protected string[] m_strSelectedBin = new string[] { "ALL" };
        protected string[] m_strSelectedVI = new string[] { "ALL" };
        public event SelectDies OnSelectDies;
        public event ChangeCurrentDie OnChangeCurrentDie;
        public event ChangePosition OnChangePosition;
        public int m_iNotSelectedBinAlpha = 20;
        public int m_iPickupedDieAlpha = 96;
        public Color m_colPickupedDie = Color.Transparent;

        protected int m_iCurrentX = -1;
        protected int m_iCurrentY = -1;
        protected int m_iCurrentDieIndex = -1;

        protected Font m_fntBin = new Font("굴림", 6);
        protected Font m_fntWBin = new Font("굴림", 5, FontStyle.Bold);
        protected Font m_fntPara = new Font("굴림", 9);

        protected RectangleF m_rectNotchArea = RectangleF.Empty;
        protected RectangleF m_rectOCRIDArea = RectangleF.Empty;
        protected string m_strDisplayDieValue = "BIN";
        protected DieDisplayValue m_DisplayDieValue = DieDisplayValue.Bin;
        protected bool m_bPopupMenu = true;
        protected bool m_bVisibleDieBorder = true;
        protected int m_iTransParent = 255;
        protected bool m_ParaLimit = false;
        protected string m_WaferID = string.Empty;

        public int REL_INDEX_MIN_X = 0;
        public int REL_INDEX_MAX_X = 0;
        public int REL_INDEX_MIN_Y = 0;
        public int REL_INDEX_MAX_Y = 0;

        /// <summary>Shot 교차점 알림 영역 크기</summary>
        public static int SHOT_NOTI_SIZE = 12;
        private bool _visibleShotAlignPoint;

        public const string DEFAULT_SHOT_LINE_COLOR = "Blue";
        public const string DEFAULT_SHOT_NOTIFY_COLOR = "Yellow";
        public const int DEFAULT_SHOT_LINE_WIDTH = 2;

        public WaferMap()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();
            SetDefaultColor();
            Reset();
            // TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

            ShotLineColor = Color.FromName(DEFAULT_SHOT_LINE_COLOR);
            ShotNofityColor = Color.FromName(DEFAULT_SHOT_NOTIFY_COLOR);
            ShotLineWidth = DEFAULT_SHOT_LINE_WIDTH;

            m_DieIndexer = new List<Point>();

            DrawVirtualDie = true;
            VirtualDieColor = Color.DarkGray;

            m_dZoomRatio_X_m_dScale = m_dZoomRatio * m_dScale;
        }

        public static void AppendVirtualDie(WaferMap map)
        {
            WaferDieCalculator calc = new WaferDieCalculator();
            calc.WaferSize = map.WaferSize;
            calc.DiePitchX = map.DieSizeX;
            calc.DiePitchY = map.DieSizeY;
            calc.OriginX = map.OriginX;
            calc.OriginY = map.OriginY;
            calc.Calculate();

            foreach (Point pt in calc.GetDiePointArray())
            {
                map.AddDie(new Base.Die(pt.X, pt.Y, 0, WaferMap.DIE_PROP_VIRTUAL_DIE));
            }
        }

        [Category("WAFER"), Description("Wafer Map의 Angle 각도를 가져오거나 설정합니다.")]
        public int ViewAngle
        {
            get
            {
                return m_iViewAngle;
            }
            set
            {
                m_iViewAngle = value % 360;
            }
        }



        [Category("동작속성"), Description("Wafer에서 Mouse 오른쪽 Button에 대한 Menu를 Display 할지에 대한 속성을 가져오거나 설정합니다.")]
        public bool PopupMenu
        {
            set
            {
                m_bPopupMenu = value;
            }
            get
            {
                return m_bPopupMenu;
            }
        }

        [Category("VALUE 속성"), Description("Parameter value의 Font 속성을 가져오거나 설정합니다.")]
        public Font ParaValueFont
        {
            set
            {
                m_fntPara = value;
            }
            get
            {
                return m_fntPara;
            }
        }


        [Category("VALUE 속성"), Description("Focus를 Display 할지에 대한 속성을 가져오거나 설정합니다.")]
        public bool VisibleFocusDie
        {
            set
            {
                m_bVisibleFocusDie = value;
            }
            get
            {
                return m_bVisibleFocusDie;
            }
        }

        [Category("VALUE 속성"), Description("Focus를 Type에 대한 속성을 가져오거나 설정합니다.")]
        public FocusType DieFocusingType
        {
            set
            {
                m_ftFocus = value;
            }
            get
            {
                return m_ftFocus;
            }
        }

        [Category("VALUE 속성"), Description("Wafer에서 Mouse 오른쪽 Button에 대한 Menu를 Display 할지에 대한 속성을 가져오거나 설정합니다.")]
        public bool VisibleDieValue
        {
            set
            {
                m_bVisibleDieValue = value;
                //this.Redraw();
            }
            get
            {
                return m_bVisibleDieValue;
            }
        }

        [Category("VALUE 속성"), Description("각 Die의 X,Y 좌표를 Enable 하거나 Disable 합니다.")]
        public bool VisibleXY
        {
            set
            {
                m_bVisibleXY = value;
                //this.Redraw();
            }
            get
            {
                return m_bVisibleXY;
            }
        }

        [Category("VALUE 속성"), Description("Visual Inspection 결과를 Drawing합니다.")]
        public bool VisibleVIFail
        {
            set
            {
                m_bVisibleVIFaile = value;
                this.Redraw();
            }
            get
            {
                return m_bVisibleVIFaile;
            }
        }

        [Category("DIE 속성"), Description("Pickup Die의 투명도를 지정하거나 가져옵니다.")]
        public int PickupDieAlpha
        {
            set
            {
                if (value > 255) value = 255;
                m_iPickupedDieAlpha = value;
            }
            get
            {
                return m_iPickupedDieAlpha;
            }
        }

        [Category("DIE 속성"), Description("Wafer영역을 벋어난 Die에 대한 표시 여부를 지정하거나 가져옵니다.")]
        public bool VisibleOffDie
        {
            set
            {
                m_bVisibleOffDie = value;
                this.Redraw();
            }
            get
            {
                return m_bVisibleOffDie;
            }
        }

        public Color PickupedDieColor
        {
            set
            {
                m_colPickupedDie = value;
            }
            get
            {
                return m_colPickupedDie;
            }
        }

        [Category("Wafer Option"), Description("Visual Fail에 대한 Data Member를 설정하거나 가져옵니다.[Default = 'VI']")]
        public string VIMember
        {
            set
            {
                m_strVIMember = value;
            }
            get
            {
                return m_strVIMember;
            }
        }


        [Category("DIE 속성"), Description("String Bin을 표시하기를 지정하거나 가져옵니다.")]
        public bool VisibleStringBin
        {
            set
            {
                m_bStringBin = value;
            }
            get
            {
                return m_bStringBin;
            }
        }

        [Category("WAFER 속성"), Description("Wafer의 Angle에 대한 Offset을 지정하거나 가져옵니다.")]
        public int AngleOffSet
        {
            set
            {
                m_iAngleOffset = value;
            }
            get
            {
                return m_iAngleOffset;
            }
        }

        [Category("DIE 속성"), Description("Wafer를 벋어나는 Die의 투명도를 지정하거나 설정합니다.")]
        public int TransParent
        {
            set
            {
                if (value > 255)
                {
                    m_iTransParent = 255;
                }
                else
                {
                    m_iTransParent = value;
                }
            }
            get
            {
                return m_iTransParent;
            }
        }

        [Category("DIE 속성"), Description("선택된 Die의 개수를 가져옵니다.")]
        public int SelectedDiesCount
        {
            get { return m_SelectedDies.Count; }
        }

        public void SetSelectedDie(int x, int y)
        {
            m_SelectedDies.Clear();
            m_SelectedDies.Add(new Point(x, y));
        }

        public void AddSelectedDie(int x, int y)
        {
            m_SelectedDies.Add(new Point(x, y));
        }

        public void ClearSelectedDie()
        {
            m_SelectedDies.Clear();
        }

        public void ResetSelectedDie()
        {
            m_SelectedDies.Clear();
            this.Redraw();
        }

        public bool VisibleSignDies
        {
            set
            {
                m_bVisibleSignDies = value;
            }
            get
            {
                return m_bVisibleSignDies;
            }
        }

        public Pen SignDieColor
        {
            set
            {
                m_bVisibleSignLine = value;
            }
            get
            {
                return m_bVisibleSignLine;
            }
        }

        public void AddSignDie(int x, int y)
        {
            m_SignDies.Add(new Point(x, y));
        }

        public void ClearSignDie()
        {
            m_SignDies.Clear();
        }

        public void SetFocusDie(int x, int y)
        {
            m_FocusDie.X = x;
            m_FocusDie.Y = y;
            DrawDies(x, y);
            if (OnChangeCurrentDie != null)
            {
                int idx = m_DieIndexer.IndexOf(new Point(x, y));
                if (idx < 0) return;
                OnChangeCurrentDie(this, m_arrDies[idx]);
            }

            m_iCurrentX = x;
            m_iCurrentY = y;
        }

        public Point GetFocusDie()
        {
            return m_FocusDie;
        }

        public void ClearFocusDie()
        {
            m_FocusDie.X = 99999;
            m_FocusDie.Y = 99999;
        }

        public void Copy(WaferMap TagetMap)
        {
            if (TagetMap == null) TagetMap = new WaferMap();

            TagetMap.m_WaferRecipe = this.m_WaferRecipe;
            TagetMap.DataSource = this.DataSource;
            TagetMap.m_strInfomation = this.m_strInfomation;
            TagetMap.m_ColorSet = this.m_ColorSet;
            TagetMap.Redraw();
        }

        public double WaferMargin
        {
            set
            {
                m_dMargin = value;
            }
            get
            {
                return m_dMargin;
            }
        }

        public string DisplayValue
        {
            set
            {
                m_strDisplayDieValue = value;
            }
            get
            {
                return m_strDisplayDieValue;
            }
        }

        /// <summary>
        /// PCM Value 출력 시 Limit 별로 색상을 표시.
        /// </summary>
        public bool ParaLimit
        {
            set
            {
                m_ParaLimit = value;
            }
            get
            {
                return m_ParaLimit;
            }
        }

        public DieDisplayValue DisplayDieValue
        {
            set
            {
                m_DisplayDieValue = value;
                m_strDisplayDieValue = m_DisplayDieValue.ToString().ToUpper();
            }
            get { return m_DisplayDieValue; }
        }

        public DieList Dies
        {
            get
            {
                return m_arrDies;
            }
        }

        public DieList ModifyDies
        {
            get { return m_arrModifyDies; }
        }

        public DieList DieInfomation
        {
            get { return m_arrDies; }
        }

        public Point[] DiesIndex
        {
            get { return m_DieIndexer.ToArray(); }
        }

        public List<Point> DiesIndexList
        {
            get { return m_DieIndexer; }
        }

        public List<Point> SelectedDies
        {
            get { return m_SelectedDies; }
        }

        public int IndexOf(int x, int y)
        {
            return m_DieIndexer.IndexOf(new Point(x, y));
        }

        //현재 Wafer 의 Wafer ID
        public string WaferID
        {
            set
            {
                m_WaferID = value;
            }
            get
            {
                return m_WaferID;
            }
        }


        public virtual object DataSource
        {
            set
            {
                m_WaferRecipe.SHOT_ARRAY_X = m_WaferRecipe.SHOT_ARRAY_Y = 1;

                if (value == null) return;
                m_DT = (DataTable)value;
                this.DieClear();

                int iDieProp = 1;

                for (int i = 0; i < m_DT.Rows.Count; i++)
                {
                    try
                    {
                        if (m_DT.Columns.IndexOf("DIEPROP") > -1) iDieProp = DACrux.Base.Convert.intParse(m_DT.Rows[i]["DIEPROP"].ToString());

                        Die oNewDie = new Die(DACrux.Base.Convert.intParse(m_DT.Rows[i]["X"].ToString())
                            , DACrux.Base.Convert.intParse(m_DT.Rows[i]["Y"].ToString()), 0, iDieProp);

                        if (m_DT.Columns.IndexOf("VI") > -1) if (int.TryParse(m_DT.Rows[i]["VI"].ToString(), out oNewDie.VIFail) == false) oNewDie.VIFail = 0;
                        if (m_DT.Columns.IndexOf("INSPECT") > -1) if (int.TryParse(m_DT.Rows[i]["INSPECT"].ToString(), out oNewDie.VIFail) == false) oNewDie.VIFail = 0;
                        if (m_DT.Columns.IndexOf("VISUALINSP") > -1) oNewDie.VIFail = DACrux.Base.Convert.intParse(m_DT.Rows[i]["VISUALINSP"].ToString());
                        if (m_DT.Columns.IndexOf("AVI") > -1) oNewDie.AVIFailNumber = DACrux.Base.Convert.intParse(m_DT.Rows[i]["AVI"].ToString());
                        if (m_DT.Columns.IndexOf("BIN") > -1) oNewDie.BinNumber = m_DT.Rows[i]["BIN"].ToString().Length == 0 ? 0 : DACrux.Base.Convert.intParse(m_DT.Rows[i]["BIN"].ToString());
                        if (m_DT.Columns.IndexOf("COUNT") > -1) oNewDie.BinNumber = m_DT.Rows[i]["COUNT"].ToString().Length == 0 ? 0 : DACrux.Base.Convert.intParse(m_DT.Rows[i]["COUNT"].ToString());
                        if (m_DT.Columns.IndexOf("PCMVALUE") > -1) oNewDie.ParametricValue = m_DT.Rows[i]["PCMVALUE"].ToString().Length == 0 ? -9999999999d : DACrux.Base.Convert.doubleParse(m_DT.Rows[i]["PCMVALUE"].ToString());
                        oNewDie.DiePassFail = oNewDie.BinNumber + oNewDie.AVIFailNumber + oNewDie.VIFail;

                        if (m_bDrawGradationDie == true && m_DT.Columns.IndexOf(m_sParametricColumn) > -1)
                        {
                            if (m_DT.Rows[i][m_sParametricColumn].ToString() != string.Empty)
                                oNewDie.ParametricValue = DACrux.Base.Convert.doubleParse(m_DT.Rows[i][m_sParametricColumn.ToUpper()].ToString());
                            else
                                oNewDie.ParametricValue = double.NaN;
                        }

                        this.AddDie(oNewDie);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            get
            {
                return m_DT;
            }
        }

        /// <summary>
        /// DM, TEST 와는 상관없이 Die List 를 Data Table 형태로 변환 해준다.
        /// </summary>
        public DataTable ToDiesTable
        {
            get
            {
                if (m_arrDies == null || m_arrDies.Count <= 0)
                    return null;

                DataTable dtIndexRow = new DataTable();

                try
                {
                    dtIndexRow.Columns.Add(new DataColumn("X", typeof(double)));
                    dtIndexRow.Columns.Add(new DataColumn("Y", typeof(double)));
                    dtIndexRow.Columns.Add(new DataColumn("BIN", typeof(double)));
                    dtIndexRow.Columns.Add(new DataColumn("PARA", typeof(double)));
                    dtIndexRow.AcceptChanges();

                    for (int nDieIdx = 0; nDieIdx < m_arrDies.Count; nDieIdx++)
                    {
                        Die InDie = m_arrDies[nDieIdx];
                        DataRow dtNew = dtIndexRow.NewRow();
                        dtNew["X"] = InDie.IndexX;
                        dtNew["Y"] = InDie.IndexY;
                        dtNew["BIN"] = InDie.BinNumber;
                        dtNew["PARA"] = InDie.ParametricValue;

                        dtIndexRow.Rows.Add(dtNew);
                    }

                    dtIndexRow.AcceptChanges();
                    return dtIndexRow;
                }
                finally
                {
                    if (dtIndexRow != null)
                        dtIndexRow.Dispose();
                }
            }
        }


        /// <summary>
        /// DataTable 형식으로 Wafer Map 을 Draw 하여 Return 한다.
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            DataTable dtMap = new DataTable();
            DataTable dtRowTemp = ToDiesTable.Copy();

            try
            {
                if (dtRowTemp == null || dtRowTemp.Rows.Count <= 0)
                    return null;

                int iXMax = int.Parse(dtRowTemp.Compute("MAX([X])", "1=1").ToString());
                int iYMax = int.Parse(dtRowTemp.Compute("MAX([Y])", "1=1").ToString());
                int iXMin = int.Parse(dtRowTemp.Compute("MIN([X])", "1=1").ToString());
                int iYMin = int.Parse(dtRowTemp.Compute("MIN([Y])", "1=1").ToString());

                //X 축 Header 를 만든다.
                for (int ic = 0; ic < iXMax - iXMin + 2; ic++)
                {
                    if (ic == 0)
                    {
                        dtMap.Columns.Add(new DataColumn("*", typeof(double)));
                    }
                    else
                    {
                        int ColIndex = (iXMin + ic) - 1;
                        dtMap.Columns.Add(new DataColumn(ColIndex.ToString(), typeof(double)));
                    }
                }

                //상위 Max 부터 순차적으로 내려 간다.
                for (int ir = iYMax; ir >= iYMin; ir--)
                {
                    DataRow drNewRow = dtMap.NewRow();
                    DataRow[] drRow = dtRowTemp.Select(string.Format("Y = {0}", ir));

                    drNewRow["*"] = ir;
                    foreach (DataRow dr in drRow)
                    {
                        if (m_strDisplayDieValue == "BIN")
                            drNewRow[dr["X"].ToString()] = dr["BIN"];
                        else
                            drNewRow[dr["X"].ToString()] = dr["PARA"];

                    }
                    dtMap.Rows.Add(drNewRow);
                }

                dtMap.AcceptChanges();

                return dtMap;
            }
            finally
            {
                if (dtMap != null)
                    dtMap.Dispose();

                if (dtRowTemp != null)
                    dtRowTemp.Dispose();
            }

        }

        public DataGridView ToDataGridView()
        {
            double dContrast = m_WaferRecipe.DIE_SIZE_Y / m_WaferRecipe.DIE_SIZE_X;

            int Height = 30;
            int iWidth = (int)Math.Max(10, (Height * dContrast));
            iWidth = (int)Math.Min(300, iWidth);


            //DataGridView 를 form 에 넣지 않으면 Draw 되지 않는다.
            Form TempForm = new Form();
            DataGridView dgMap = new DataGridView();

            //Wafer 정보가 있을 경우 해당 정보를 Tab 정보로 넣는다.
            if (string.IsNullOrEmpty(m_WaferID) == false)
            {
                dgMap.Name = m_WaferID;
            }
            else
            {
                if (m_strInfomation != null && m_strInfomation.Count > 0)
                {
                    for (int i = 0; i < m_strInfomation.Count; i++)
                    {
                        string strWaferInfo = m_strInfomation[i].ToString().ToUpper();
                        if (strWaferInfo.Contains("WAFER") == true && strWaferInfo.Contains(":") == true)
                        {
                            string[] strWaferVal = strWaferInfo.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            if (strWaferVal.Length == 2)
                                dgMap.Name = strWaferVal[1].Trim();
                        }
                    }
                }
            }

            TempForm.Controls.Add(dgMap);
            dgMap.AllowUserToAddRows = false;
            dgMap.RowHeadersVisible = true;

            DataTable dtWaferMap = ToDataTable().Copy();
            if (dtWaferMap == null)
                return null;

            if (dtWaferMap.Columns.IndexOf("*") > -1)
            {
                dtWaferMap.Columns.Remove("*");
                dtWaferMap.AcceptChanges();
            }

            dgMap.DataSource = dtWaferMap;

            for (int ic = 0; ic < dgMap.Columns.Count; ic++)
            {
                dgMap.Columns[ic].HeaderText = dtWaferMap.Columns[ic].ColumnName;
                dgMap.Columns[ic].Width = iWidth;
            }


            //DataGridView 의 마지막 Row 는 
            int idgRow = dgMap.Rows.Count;
            for (int ir = 0; ir < dgMap.Rows.Count; ir++)
            {
                for (int c = 0; c < dgMap.Columns.Count; c++)
                {
                    int iBinBal = -1;
                    double dPara = 0;

                    if (m_strDisplayDieValue == "BIN" && dgMap.Rows[ir].Cells[c].Value != null && int.TryParse(dgMap.Rows[ir].Cells[c].Value.ToString(), out iBinBal) == true)
                    {
                        dgMap.Rows[ir].Cells[c].Style.BackColor = m_ColorSet[iBinBal];
                    }
                    else if (dgMap.Rows[ir].Cells[c].Value != null && double.TryParse(dgMap.Rows[ir].Cells[c].Value.ToString(), out dPara) == true)
                        dgMap.Rows[ir].Cells[c].Style.BackColor = Color.Gray;
                }

                dgMap.Rows[ir].Height = Height;
                dgMap.Rows[ir].HeaderCell.Value = idgRow;
                idgRow--;
            }

            dgMap.Refresh();
            Application.DoEvents();
            return dgMap;

        }



        #region ◈ 소멸자 , Dispose
        ~WaferMap()
        {
            if (m_arrDies != null)
            {
                m_arrDies.Clear();
                m_arrDies = null;
            }

            if (m_DieIndexer != null)
            {
                m_DieIndexer.Clear();
                m_DieIndexer = null;
            }

            if (m_hbSelectBrush != null) m_hbSelectBrush.Dispose();

            if (m_gdiMain != null) m_gdiMain.Dispose();
            if (m_gdiTempMap != null) m_gdiTempMap.Dispose();
            if (m_bmpWaferID != null) m_bmpWaferID.Dispose();
            if (m_bmpTemp != null) m_bmpTemp.Dispose();
            if (m_bmpWaferMap != null) m_bmpWaferMap.Dispose();
            //if(ctxmWaferMap != null) ctxmWaferMap.Dispose();
            if (m_DT != null) m_DT.Dispose();
            m_strInfomation = null;
            m_ColorSet = null;
            GC.Collect();
        }

        #endregion

        #region ◈ Wafer
        public virtual void Reset()
        {
            /// Data가 존재 했을경우 초기화//////////////////////////////////////////////////
            if (m_arrDies != null)
            {
                m_arrDies.Clear();
                m_arrDies = null;
            }

            if (m_DieIndexer != null)
            {
                m_DieIndexer.Clear();
                m_DieIndexer = null;
            }

            if (m_hbSelectBrush != null) m_hbSelectBrush.Dispose();
            //////////////////////////////////////////////////////////////////////////////

            m_WaferRecipe = new WaferRecipe(200.0d);
            if (m_WaferRecipe.NOTCH_TYPE == Notch.Notch)
            {
                m_dNotchSize = 2.0d;
            }
            else
            {
                m_dNotchSize = 30.0d;
            }

            double dMinCanvers = Math.Min(this.Width, this.Height);
            m_dZoomRatio = dMinCanvers / m_WaferRecipe.WAFER_SIZE;
            m_dZoomRatio_X_m_dScale = m_dZoomRatio * m_dScale;
            m_rectdWaferArea.X = ((dMinCanvers - this.Width) / 2.0f) / m_dZoomRatio;
            m_rectdWaferArea.Y = ((dMinCanvers - this.Height) / 2.0f) / m_dZoomRatio;
            m_rectdWaferArea.Width = m_WaferRecipe.WAFER_SIZE;
            m_rectdWaferArea.Height = m_WaferRecipe.WAFER_SIZE;

            m_arrDies = new DieList();
            m_DieIndexer = new List<Point>();
            m_arrModifyDies = new DieList();
            m_DieModifyIndexer = new List<Point>();
            m_SelectedDies = new List<Point>();
            m_hbSelectBrush = new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.White, Color.PowderBlue);

            m_bmpFocus = new Bitmap(DACrux.Map.Resource.Pointer);

            m_ItemCountColorList = new ItemCountColorList();

        }
        #endregion

        #region ◈ First Die 속성
        /// <summary>
        /// Probe Test시 First Die의 Drawing에 대한 속성을 정의한다.
        /// </summary>
        /// 
        [Category("First Die"), Description("First Die를 표시할지 여부를 가져오거나 설정합니다.")]
        public bool DrawFirstDie
        {
            set { m_bDrawFirstDie = value; }
            get { return m_bDrawFirstDie; }
        }

        [Category("First Die"), Description("First Die의 X좌표 값을 가져오거나 설정합니다.")]
        public int FirstDieX
        {
            set { m_WaferRecipe.FIRST_DIE_X = value; }
            get { return m_WaferRecipe.FIRST_DIE_X; }
        }

        [Category("First Die"), Description("First Die의 Y좌표 값을 가져오거나 설정합니다.")]
        public int FirstDieY
        {
            set { m_WaferRecipe.FIRST_DIE_Y = value; }
            get { return m_WaferRecipe.FIRST_DIE_Y; }
        }

        [Category("First Die"), Description("First Die의 Border Color 값을 가져오거나 설정합니다.")]
        public Color FirstDieBorderColor
        {
            set { m_colFirstDieBorder = value; }
            get { return m_colFirstDieBorder; }
        }
        #endregion

        #region ◈ Origin Die 속성

        [Category("Origin Die"), Description("Origin Die를 표시할지 여부를 가져오거나 설정합니다.")]
        public bool DrawOriginDie
        {
            set { m_bDrawOriginDie = value; }
            get { return m_bDrawOriginDie; }
        }

        [Category("Origin Die"), Description("Origin Die의 X축 절대좌표를 가져오거나 설정합니다.")]
        public double OriginX
        {
            set { m_WaferRecipe.ORIGIN_X = value; }
            get { return m_WaferRecipe.ORIGIN_X; }
        }

        [Category("Origin Die"), Description("Origin Die의 Y축 절대좌표를 가져오거나 설정합니다.")]
        public double OriginY
        {
            set { m_WaferRecipe.ORIGIN_Y = value; }
            get { return m_WaferRecipe.ORIGIN_Y; }
        }

        [Category("Origin Die"), Description("Origin Die의 X축 Index 좌표를 가져오거나 설정합니다.")]
        public int OriginIndexX
        {
            set { m_WaferRecipe.ORIGIN_DIE_X = value; }
            get { return m_WaferRecipe.ORIGIN_DIE_X; }
        }

        [Category("Origin Die"), Description("Origin Die의 Y축 Index 좌표를 가져오거나 설정합니다.")]
        public int OriginIndexY
        {
            set { m_WaferRecipe.ORIGIN_DIE_Y = value; }
            get { return m_WaferRecipe.ORIGIN_DIE_Y; }
        }

        [Category("Origin Die"), Description("Origin Die의 Border Color값을 가져오거나 설정합니다.")]
        public Color OriginDieBorder
        {
            set { m_colOriginDieBorder = value; }
            get { return m_colOriginDieBorder; }
        }

        #endregion

        #region ◈ Mark Die 속성

        [Category("Mark Die"), Description("Mark Die를 표시할지 여부를 가져오거나 설정합니다.")]
        public bool DrawMarkDie
        {
            set
            {
                m_bDrawMarkDie = value;
                mnuitemVISIBLE_MARKDIE.Checked = m_bDrawMarkDie;
            }
            get { return m_bDrawMarkDie; }
        }

        [Category("Mark Die"), Description("Mark Die의 Border Color값을 가져오거나 설정합니다.")]
        public Color MarkDieColor
        {
            set { m_colMarkDieColor = value; }
            get { return m_colMarkDieColor; }
        }


        [Category("Mark Die"), Description("Mark Die의 Border Color값을 가져오거나 설정합니다.")]
        public bool MarkDieMenuVisible
        {
            set
            {
                mnuitemVISIBLE_MARKDIE.Checked = value;
                mnuitemVISIBLE_MARKDIE.Visible = value;
            }
        }
        #endregion

        #region ◈ Skip Die 속성

        [Category("Mark Die"), Description("Skip Die를 표시할지 여부를 가져오거나 설정합니다.")]
        public bool DrawSkipDie
        {
            set { m_bDrawSkipDie = value; }
            get { return m_bDrawSkipDie; }
        }

        [Category("Skip Die"), Description("Skip Die의 Border Color값을 가져오거나 설정합니다.")]
        public Color SkipDieColor
        {
            set { m_colSkipDieColor = value; }
            get { return m_colSkipDieColor; }
        }

        #endregion

        #region ◈ Virtual Die 속성

        [DefaultValue(true)]
        [Category("Virtual Die"), Description("Virtual Die를 표시할지 여부를 가져오거나 설정합니다.")]
        public bool DrawVirtualDie
        {
            get;
            set;
        }

        [DefaultValue(typeof(Color), "DarkGray")]
        [Category("Virtual Die"), Description("Virtual Die의 BackColor를 설정하거나 가져옵니다.")]
        public Color VirtualDieColor
        {
            get;
            set;
        }

        #endregion

        #region ◈ Die 속성

        [Category("Die 속성"), Description("Die의 Border Color를 설정하거나 가져옵니다.")]
        public Color DieBorderColor
        {
            set { m_colDieBorder = value; }
            get { return m_colDieBorder; }
        }

        [Category("Die 속성"), Description("Die의 Border를 표시할지 여부를 설정하거나 가져옵니다.")]
        public bool VisibleDieBorder
        {
            set { m_bVisibleDieBorder = value; }
            get { return m_bVisibleDieBorder; }
        }

        [Category("Die 속성"), Description("Die 의 Width를 mm로 설정하거나 가져옵니다.")]
        public double DieSizeX
        {
            set { m_WaferRecipe.DIE_SIZE_X = value; }
            get { return m_WaferRecipe.DIE_SIZE_X; }
        }

        [Category("Die 속성"), Description("Die 의 Height를 mm로 설정하거나 가져옵니다.")]
        public double DieSizeY
        {
            set { m_WaferRecipe.DIE_SIZE_Y = value; }
            get { return m_WaferRecipe.DIE_SIZE_Y; }
        }

        [Category("Die 속성"), Description("Wafer에 존재한는 Die의 X축 Count를 설정하거나 가져옵니다.")]
        public int XDies
        {
            get { return m_WaferRecipe.XDIES; }
        }

        [Category("Die 속성"), Description("Wafer에 존재한는 Die의 Y축 Count를 설정하거나 가져옵니다.")]
        public int YDies
        {
            get { return m_WaferRecipe.YDIES; }
        }

        [Category("Die 속성"), Description("Wafer에 존재한는 Die의 X축의 최소 Index를 설정하거나 가져옵니다.")]
        public int DieMinX
        {
            set { m_WaferRecipe.DIE_INDEX_MIN_X = value; }
            get { return m_WaferRecipe.DIE_INDEX_MIN_X; }
        }

        [Category("Die 속성"), Description("Wafer에 존재한는 Die의 Y축의 최소 Index를 설정하거나 가져옵니다.")]
        public int DieMinY
        {
            set { m_WaferRecipe.DIE_INDEX_MIN_Y = value; }
            get { return m_WaferRecipe.DIE_INDEX_MIN_Y; }
        }

        [Category("Die 속성"), Description("Wafer에 존재한는 Die의 X축의 최대 Index를 설정하거나 가져옵니다.")]
        public int DieMaxX
        {
            set { m_WaferRecipe.DIE_INDEX_MAX_X = value; }
            get { return m_WaferRecipe.DIE_INDEX_MAX_X; }
        }

        [Category("Die 속성"), Description("Wafer에 존재한는 Die의 Y축의 최대 Index를 설정하거나 가져옵니다.")]
        public int DieMaxY
        {
            set { m_WaferRecipe.DIE_INDEX_MAX_Y = value; }
            get { return m_WaferRecipe.DIE_INDEX_MAX_Y; }
        }

        [Category("Die 속성"), DefaultValue(false), Description("Die 색상을 Parametric Value를 기준으로 그라데이션으로 표시할지 여부를 설정하거나 가져옵니다.")]
        public bool DrawGradationDie
        {
            set { m_bDrawGradationDie = value; }
            get { return m_bDrawGradationDie; }
        }

        [Category("Die 속성"), Description("그라데이션되는 시작 색상을 설정하거나 가져옵니다.")]
        public Color FromGradationDieColor
        {
            set { m_colFromGradationDieColor = value; }
            get { return m_colFromGradationDieColor; }
        }

        [Category("Die 속성"), Description("그라데이션되는 종료 색상을 설정하거나 가져옵니다.")]
        public Color ToGradationDieColor
        {
            set { m_colToGradationDieColor = value; }
            get { return m_colToGradationDieColor; }
        }

        [Category("Die 속성"), Description("그라데이션 단계값을 설정하거나 가져옵니다.")]
        public int GradationInterval
        {
            set { m_nGradationInterval = value; }
            get { return m_nGradationInterval; }
        }

        [Category("Die 속성"), Description("그라데이션 색상 지정을 위한 Parametric Value의 최대값을 설정하거나 가져옵니다.")]
        public double GradationMaxValue
        {
            set { m_dbGradationMaxValue = value; }
            get { return m_dbGradationMaxValue; }
        }

        [Category("Die 속성"), Description("그라데이션 색상 지정을 위한 Parametric Value의 최소값을 설정하거나 가져옵니다.")]
        public double GradationMinValue
        {
            set { m_dbGradationMinValue = value; }
            get { return m_dbGradationMinValue; }
        }

        [Category("Die 속성"), Description("Parametric Value의 컬럼명을 설정하거나 가져옵니다.")]
        public string ParametricColumn
        {
            set { m_sParametricColumn = value; }
            get { return m_sParametricColumn; }
        }


        #endregion

        #region ◈ Shot 속성

        [DefaultValue(1)]
        [Category("Shot 속성"), Description("Shot에 대한 X 방향의 Die 갯수를 나타냅니다.")]
        public int ShotArrayX
        {
            set { m_WaferRecipe.SHOT_ARRAY_X = value; }
            get { return m_WaferRecipe.SHOT_ARRAY_X; }
        }

        [DefaultValue(1)]
        [Category("Shot 속성"), Description("Shot에 대한 Y 방향의 Die 갯수를 나타냅니다.")]
        public int ShotArrayY
        {
            set { m_WaferRecipe.SHOT_ARRAY_Y = value; }
            get { return m_WaferRecipe.SHOT_ARRAY_Y; }
        }

        [DefaultValue(1)]
        [Category("Shot 속성"), Description("Shot의 좌하단에 매치되는 Die 에 대한 X 방향 인덱스를 나타냅니다.")]
        public int ShotStartX
        {
            set { m_WaferRecipe.SHOT_START_X = value; }
            get { return m_WaferRecipe.SHOT_START_X; }
        }

        [DefaultValue(1)]
        [Category("Shot 속성"), Description("Shot의 좌하단에 매치되는 Die 에 대한 Y 방향 인덱스를 나타냅니다.")]
        public int ShotStartY
        {
            set { m_WaferRecipe.SHOT_START_Y = value; }
            get { return m_WaferRecipe.SHOT_START_Y; }
        }

        [DefaultValue(false)]
        [Category("Shot 속성"), Description("Shot이 Map에 보여지는지를 나타냅니다.")]
        public bool VisibleShot
        {
            get;
            set;
        }

        [DefaultValue(typeof(Color), DEFAULT_SHOT_LINE_COLOR)]
        [Category("Shot 속성"), Description("Shot의 선 색상을 나타냅니다.")]
        public Color ShotLineColor
        {
            get;
            set;
        }

        [DefaultValue(DEFAULT_SHOT_LINE_WIDTH)]
        [Category("Shot 속성"), Description("Shot의 선 두께를 나타냅니다.")]
        public int ShotLineWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Shot Notify 영역 색상
        /// </summary>
        [DefaultValue(typeof(Color), DEFAULT_SHOT_NOTIFY_COLOR)]
        public Color ShotNofityColor { get; set; }

        /// <summary>
        /// Shot 정보가 설정되었는지를 가져옵니다.
        /// </summary>
        [Browsable(false)]
        public bool ExistsShotInfo
        {
            get { return m_WaferRecipe.SHOT_ARRAY_X > 0 || m_WaferRecipe.SHOT_ARRAY_Y > 0; }
        }

        #endregion

        #region ◈ Wafer 속성
        [Category("Wafer 속성"), Description("Wafer의 Size를 mm로 설정하거나 가져옵니다.")]
        public double WaferSize
        {
            set { m_WaferRecipe.WAFER_SIZE = value; }
            get { return m_WaferRecipe.WAFER_SIZE; }
        }

        [Category("Wafer 속성"), Description("Wafer의 Background Color를 설정하거나 가져옵니다.")]
        public Color WaferColor
        {
            set { m_colWafer = value; }
            get { return m_colWafer; }
        }

        [Category("Wafer 속성"), Description("Wafer의 Border Color를 설정하거나 가져옵니다.")]
        public Color WaferBorderColor
        {
            set { m_colWaferBorder = value; }
            get { return m_colWaferBorder; }
        }

        [Category("Wafer 속성"), Description("Edge의 Size를 mm로 설정하거나 가져옵니다.")]
        public double EdgeSize
        {
            set { m_WaferRecipe.EDGE_SIZE = value; }
            get { return m_WaferRecipe.EDGE_SIZE; }
        }

        [Category("Wafer 속성"), Description("Edge의 Background Color를 설정하거나 가져옵니다.")]
        public Color EdgeColor
        {
            set { m_colEdge = value; }
            get { return m_colEdge; }
        }

        [Category("Wafer 속성"), Description("Net Die의 개수를 가져옵니다.")]
        public int NetDie
        {
            //set{m_WaferRecipe.NETDIE = value;}
            get { return m_WaferRecipe.NETDIE; }
        }

        [Category("Wafer 속성"), Description("Notch의 각도를 설정하거나 가져옵니다.")]
        public int NotchAngle
        {
            set
            {
                m_WaferRecipe.ANGLE = value;
            }
            get
            {
                return m_WaferRecipe.ANGLE;
            }
        }

        [Category("Wafer 속성"), Description("Notch의 Type을 설정하거나 가져옵니다.")]
        public Notch NotchType
        {
            set
            {
                m_WaferRecipe.NOTCH_TYPE = value;

                if (m_WaferRecipe.NOTCH_TYPE == Notch.Notch)
                {
                    m_dNotchSize = 2.0d;
                }
                else
                {
                    m_dNotchSize = 30.0d;
                }
            }
            get
            {
                return m_WaferRecipe.NOTCH_TYPE;
            }
        }
        #endregion

        #region ◈ Option 속성


        [Category("Wafer Option"), Description("Wafer Infomation의 표시 여부를 설정하거나 가져옵니다.")]
        public bool VisibleInfomation
        {
            set
            {
                m_bVisibleInfo = value;
            }
            get
            {
                return m_bVisibleInfo;
            }
        }

        [Category("Wafer Option"), Description("REFERENCEDIE_SETTING")]
        public int ReferenceDieSetting
        {
            set
            {
                m_WaferRecipe.REFERENCEDIE_SETTING = value;
            }
            get
            {
                return m_WaferRecipe.REFERENCEDIE_SETTING;
            }
        }

        [Category("Wafer Option"), Description("X,Y 좌표의 시작점을 설정하거나 가져옵니다.")]
        public XYDirection XYDirect
        {
            set
            {
                m_WaferRecipe.XYDIR = value;
            }
            get
            {
                return m_WaferRecipe.XYDIR;
            }
        }


        [Category("Wafer Option"), Description("Center Guide 표현 여부를 설정하거나 가져옵니다.")]
        public bool CenterMark
        {
            set
            {
                m_bCenterMark = value;
                if (m_gdiMain == null || m_bmpWaferMap == null) return;
                DrawCenteGrid();
            }
            get { return m_bCenterMark; }
        }

        [Category("Wafer Option"), Description("Die X, Y의 좌표 눈금 표현 여부를 설정하거나 가져옵니다.")]
        public bool ScaleMark
        {
            set
            {
                m_bScale = value;
                if (value) m_dScale = 0.97d;
                else m_dScale = 1.0d;
                if (m_gdiMain != null && m_bmpWaferMap != null) this.Redraw();
                m_dZoomRatio_X_m_dScale = m_dZoomRatio * m_dScale;
            }
            get { return m_bScale; }
        }

        [Category("Wafer Option"), Description("Wafer의 Drawing Mode을 설정하거나 가져옵니다.")]
        public virtual MapMode WaferDrawMode
        {
            set
            {
                m_eoMapMode = value;
                switch (m_eoMapMode)
                {
                    case MapMode.Fit:
                        mnuitemMAPMODE_FITSIZE_Click(null, null);
                        break;
                    case MapMode.Free:
                        mnuitemMAPMODE_FREEZOOM_Click(null, null);
                        break;
                }
            }
            get { return m_eoMapMode; }
        }

        [Category("Wafer Option"), Description("Draw 대상 Bin을 설정하거나 가져옵니다.")]
        public virtual string SelecetedBin
        {
            set
            {
                m_strSelectedBin = value.Split(',');
                FadeIn();
                //System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(FadeIn));
                //t.IsBackground = true;
                //t.Start();
            }
            get
            {
                return string.Join(",", m_strSelectedBin);
            }
        }


        [Category("Wafer Option"), Description("Draw 대상 VI Fail Code를 설정하거나 가져옵니다.")]
        public virtual string SelectedVI
        {
            set
            {
                m_strSelectedVI = value.Split(',');
                FadeIn();
            }
            get
            {
                return string.Join(",", m_strSelectedVI);
            }
        }

        private void FadeIn()
        {
            /// DIE 수가 많으면 FadeIn 회수를 줄인다.
            int iFadeIn_Step = 5;


            if (m_DieIndexer == null) return;

            iFadeIn_Step = Math.Max(5 - m_DieIndexer.Count / 400, 1);

            m_iNotSelectedBinAlpha = 20;
            if (m_DieIndexer.Count > 0)
            {
                m_iNotSelectedBinAlpha = 0;
                this.Redraw();

                //for (int i = iFadeIn_Step; i > 0; i--)
                //{
                //    m_iNotSelectedBinAlpha = i * 255 / iFadeIn_Step;
                //    this.Redraw();
                //}

                //m_iNotSelectedBinAlpha = 50;
                //this.Redraw();
            }
        }

        #endregion

        #region ■ Zoom 관련

        protected double m_dZoomRatio_X_m_dScale;

        public void ZoomIn()
        {
            if (m_eoMapMode == MapMode.Fit) return;
            m_dZoomRatio = m_dZoomRatio * 1.05d;
            m_dZoomRatio_X_m_dScale = m_dZoomRatio * m_dScale;
            RectangleD rectdBefore = m_rectdWaferArea;
            PointD pdBeforeCenter = new PointD(m_rectdWaferArea.X + m_rectdWaferArea.Width / 2d, m_rectdWaferArea.Y + m_rectdWaferArea.Height / 2d);

            //m_rectdWaferArea.Width = this.Width / (m_dZoomRatio);
            //m_rectdWaferArea.Height = this.Height / (m_dZoomRatio);
            m_rectdWaferArea.Width = this.Width / m_dZoomRatio_X_m_dScale;
            m_rectdWaferArea.Height = this.Height / m_dZoomRatio_X_m_dScale;
            m_rectdWaferArea.X = pdBeforeCenter.X - m_rectdWaferArea.Width / 2d;
            m_rectdWaferArea.Y = pdBeforeCenter.Y - m_rectdWaferArea.Height / 2d;
            Redraw();

        }

        public void ZoomOut()
        {
            if (m_eoMapMode == MapMode.Fit) return;
            m_dZoomRatio = m_dZoomRatio * 0.95f;
            m_dZoomRatio_X_m_dScale = m_dZoomRatio * m_dScale;
            RectangleD rectdBefore = m_rectdWaferArea;
            PointD pdBeforeCenter = new PointD(m_rectdWaferArea.X + m_rectdWaferArea.Width / 2d, m_rectdWaferArea.Y + m_rectdWaferArea.Height / 2d);

            m_rectdWaferArea.Width = this.Width / m_dZoomRatio_X_m_dScale;
            m_rectdWaferArea.Height = this.Height / m_dZoomRatio_X_m_dScale;
            m_rectdWaferArea.X = pdBeforeCenter.X - m_rectdWaferArea.Width / 2d;
            m_rectdWaferArea.Y = pdBeforeCenter.Y - m_rectdWaferArea.Height / 2d;
            Redraw();
        }
        #endregion

        #region ▣ Mouse Up/Down/Move Event
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                m_poStart.X = e.X;
                m_poStart.Y = e.Y;

                m_poEnd.X = e.X;
                m_poEnd.Y = e.Y;

                m_rectSelect.Width = Math.Max(m_poStart.X, m_poEnd.X) - Math.Min(m_poStart.X, m_poEnd.X);
                m_rectSelect.Height = Math.Max(m_poStart.Y, m_poEnd.Y) - Math.Min(m_poStart.Y, m_poEnd.Y);

                ///  Mouse Down할때 생성되고 Up할때 Dispose 된다.
                ///  //////////////////////////////////////////////////////////////////////////////////////
                m_SelectPath = new GraphicsPath();
                ///////////////////////////////////////////////////////////////////////////////////////////
                ///

                if (m_eoMouseDragMode == MouseDragMode.Zone)
                {
                    DrawZoneGuid();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (m_bPopupMenu) ctxmWaferMap.Show(this, new Point(e.X, e.Y));
            }
        }

        public void SelDie(int IndexX, int indexY)
        {
            int iDieIdx = m_DieIndexer.IndexOf(new Point(IndexX, indexY));

            if (iDieIdx < 0)
            {
                IndexX = m_iCurrentX;
                indexY = m_iCurrentY;
                iDieIdx = m_DieIndexer.IndexOf(new Point(IndexX, indexY));
            }

            if (iDieIdx < 0) return;
            Die oNextDie = (Die)this.m_arrDies[iDieIdx];

            if (m_iCurrentX != IndexX || m_iCurrentY != indexY)
            {

                this.RedrawDie(oNextDie, Color.Pink, Color.Red);

                if (OnChangeCurrentDie != null)
                {
                    OnChangeCurrentDie(this, oNextDie);
                }
            }

            m_iCurrentX = IndexX;
            m_iCurrentY = indexY;

        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            try
            {
                if (e.Button == MouseButtons.Left && m_eoMapMode != MapMode.Edit)
                {
                    m_poEnd.X = e.X;
                    m_poEnd.Y = e.Y;

                    m_rectSelect.X = Math.Min(m_poStart.X, m_poEnd.X);
                    m_rectSelect.Y = Math.Min(m_poStart.Y, m_poEnd.Y);
                    m_rectSelect.Width = Math.Max(m_poStart.X, m_poEnd.X) - Math.Min(m_poStart.X, m_poEnd.X);
                    m_rectSelect.Height = Math.Max(m_poStart.Y, m_poEnd.Y) - Math.Min(m_poStart.Y, m_poEnd.Y);

                    switch (m_eoMouseDragMode)
                    {
                        case MouseDragMode.Zoom:
                            DrawSelectRect();
                            break;
                        case MouseDragMode.Move:
                            DrawMoveRect();
                            break;
                        case MouseDragMode.Rotate:
                            break;
                        case MouseDragMode.Zone:
                        case MouseDragMode.Defect:
                            DrawZoneGuid();
                            break;
                    }
                }
                else
                {
                    PointD pdSelectPoint = GetRealPoint(e.X, e.Y);

                    int iDieIndexX = 0;
                    int iDieIndexY = 0;

                    if (pdSelectPoint.X + (m_WaferRecipe.ORIGIN_X % m_WaferRecipe.DIE_SIZE_X) < 0)
                    {
                        iDieIndexX = (int)((pdSelectPoint.X + (m_WaferRecipe.ORIGIN_X % m_WaferRecipe.DIE_SIZE_X)) / m_WaferRecipe.DIE_SIZE_X) - 1 + m_WaferRecipe.ORIGIN_DIE_X + (int)(m_WaferRecipe.ORIGIN_X / m_WaferRecipe.DIE_SIZE_X);
                    }
                    else
                    {
                        iDieIndexX = (int)((pdSelectPoint.X + (m_WaferRecipe.ORIGIN_X % m_WaferRecipe.DIE_SIZE_X)) / m_WaferRecipe.DIE_SIZE_X) + m_WaferRecipe.ORIGIN_DIE_X + (int)(m_WaferRecipe.ORIGIN_X / m_WaferRecipe.DIE_SIZE_X);
                    }

                    if (pdSelectPoint.Y + (m_WaferRecipe.ORIGIN_Y % m_WaferRecipe.DIE_SIZE_Y) < 0)
                    {
                        iDieIndexY = (int)((pdSelectPoint.Y + (m_WaferRecipe.ORIGIN_Y % m_WaferRecipe.DIE_SIZE_Y)) / m_WaferRecipe.DIE_SIZE_Y) - 1 + m_WaferRecipe.ORIGIN_DIE_Y + (int)(m_WaferRecipe.ORIGIN_Y / m_WaferRecipe.DIE_SIZE_Y);
                    }
                    else
                    {
                        iDieIndexY = (int)((pdSelectPoint.Y + (m_WaferRecipe.ORIGIN_Y % m_WaferRecipe.DIE_SIZE_Y)) / m_WaferRecipe.DIE_SIZE_Y) + m_WaferRecipe.ORIGIN_DIE_Y + (int)(m_WaferRecipe.ORIGIN_Y / m_WaferRecipe.DIE_SIZE_Y);
                    }

                    switch (m_WaferRecipe.XYDIR)
                    {
                        case XYDirection.LeftTop:
                            //iDieIndexX = iDieIndexX;
                            iDieIndexY = this.m_WaferRecipe.ORIGIN_DIE_Y - (iDieIndexY - m_WaferRecipe.ORIGIN_DIE_Y) - 1;
                            break;
                        case XYDirection.LeftBottom:
                            //iDieIndexX = iDieIndexX;
                            //iDieIndexY = iDieIndexY;
                            break;
                        case XYDirection.RightBottom:
                            iDieIndexX = this.m_WaferRecipe.ORIGIN_DIE_X - (iDieIndexX - m_WaferRecipe.ORIGIN_DIE_X) - 1;
                            break;
                        case XYDirection.RightTop:
                            iDieIndexX = this.m_WaferRecipe.ORIGIN_DIE_X - (iDieIndexX - m_WaferRecipe.ORIGIN_DIE_X) - 1;
                            iDieIndexY = this.m_WaferRecipe.ORIGIN_DIE_Y - (iDieIndexY - m_WaferRecipe.ORIGIN_DIE_Y) - 1;
                            break;
                    }

                    m_iCurrentDieIndex = m_DieIndexer.IndexOf(new Point(iDieIndexX, iDieIndexY));

                    if (m_iCurrentDieIndex < 0)
                    {
                        this.ClearMarkDie();
                        m_iCurrentX = -1;
                        m_iCurrentY = -1;
                        return;
                    }

                    Die oDie = (Die)this.m_arrDies[m_iCurrentDieIndex];

                    if (m_eoMapMode == MapMode.Edit) this.RedrawDie(oDie, Color.Pink, Color.Red);

                    if (m_iCurrentX != iDieIndexX || m_iCurrentY != iDieIndexY)
                    {
                        if (OnChangeCurrentDie != null)
                        {
                            OnChangeCurrentDie(this, oDie);
                        }
                    }

                    m_iCurrentX = iDieIndexX;
                    m_iCurrentY = iDieIndexY;

                    if (OnChangePosition != null)
                    {
                        PointD pdReal = new PointD(pdSelectPoint.X + m_WaferRecipe.ORIGIN_X, pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y);
                        OnChangePosition(this, pdSelectPoint, pdReal);
                    }
                }
            }
            catch (Exception) { }
            //if (e.Button == MouseButtons.Left && m_eoMouseDragMode != MouseDragMode.Normal)
            //{
            //    m_poEnd.X = e.X;
            //    m_poEnd.Y = e.Y;

            //    m_rectSelect.X = Math.Min(m_poStart.X, m_poEnd.X);
            //    m_rectSelect.Y = Math.Min(m_poStart.Y, m_poEnd.Y);
            //    m_rectSelect.Width = Math.Max(m_poStart.X, m_poEnd.X) - Math.Min(m_poStart.X, m_poEnd.X);
            //    m_rectSelect.Height = Math.Max(m_poStart.Y, m_poEnd.Y) - Math.Min(m_poStart.Y, m_poEnd.Y);

            //    switch (m_eoMouseDragMode)
            //    {
            //        case MouseDragMode.Zoom:
            //            DrawSelectRect();
            //            break;
            //        case MouseDragMode.Move:
            //            DrawMoveRect();
            //            break;
            //        case MouseDragMode.Rotate:
            //            break;
            //        case MouseDragMode.Zone:
            //        case MouseDragMode.Defect:
            //            DrawZoneGuid();
            //            break;
            //    }
            //}
            //else
            //{
            //    PointD pdSelectPoint = GetRealPoint(e.X, e.Y);

            //    int iDieIndexX = 0;
            //    int iDieIndexY = 0;

            //    switch (m_WaferRecipe.XYDIR)
            //    {
            //        case XYDirection.LeftTop:
            //            if ((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X )< 0d)
            //            {
            //                iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X) / m_WaferRecipe.DIE_SIZE_X);
            //            }
            //            else
            //            {
            //                iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X) / m_WaferRecipe.DIE_SIZE_X);
            //                //iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X + m_WaferRecipe.DIE_SIZE_X) / m_WaferRecipe.DIE_SIZE_X);
            //            }

            //            if ((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y) < 0d)
            //            {
            //                iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y - m_WaferRecipe.ORIGIN_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //            }
            //            else
            //            {
            //                iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y - m_WaferRecipe.ORIGIN_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //                //iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y - m_WaferRecipe.ORIGIN_Y + m_WaferRecipe.DIE_SIZE_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //            }
            //            /// Y는 역수 취함
            //            iDieIndexY = m_WaferRecipe.DIE_INDEX_MIN_Y + (m_WaferRecipe.DIE_INDEX_MAX_Y - iDieIndexY);

            //            break;
            //        case XYDirection.RightTop:
            //            if ((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X) < 0d)
            //            {
            //                iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X - m_WaferRecipe.ORIGIN_X) / m_WaferRecipe.DIE_SIZE_X);
            //            }
            //            else
            //            {
            //                iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X - m_WaferRecipe.ORIGIN_X) / m_WaferRecipe.DIE_SIZE_X);
            //                //iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X - m_WaferRecipe.ORIGIN_X + m_WaferRecipe.DIE_SIZE_X) / m_WaferRecipe.DIE_SIZE_X);
            //            }

            //            if ((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y) < 0d)
            //            {
            //                iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y - m_WaferRecipe.ORIGIN_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //            }
            //            else
            //            {
            //                iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y - m_WaferRecipe.ORIGIN_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //                //iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y - m_WaferRecipe.ORIGIN_Y + m_WaferRecipe.DIE_SIZE_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //            }

            //            /// X,Y는 역수 취함
            //            iDieIndexX = m_WaferRecipe.DIE_INDEX_MIN_X + (m_WaferRecipe.DIE_INDEX_MAX_X - iDieIndexX);
            //            iDieIndexY = m_WaferRecipe.DIE_INDEX_MIN_Y + (m_WaferRecipe.DIE_INDEX_MAX_Y - iDieIndexY);
            //            break;
            //        case XYDirection.LeftBottom:
            //            if ((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X) < 0d)
            //            {
            //                iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X) / m_WaferRecipe.DIE_SIZE_X);
            //            }
            //            else
            //            {
            //                iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X) / m_WaferRecipe.DIE_SIZE_X);
            //                //iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X + m_WaferRecipe.DIE_SIZE_X) / m_WaferRecipe.DIE_SIZE_X);

            //            }

            //            if ((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y) < 0d)
            //            {
            //                iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //            }
            //            else
            //            {
            //                iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //                //iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y + m_WaferRecipe.DIE_SIZE_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //            }
            //            break;

            //        case XYDirection.RightBottom:
            //            if ((pdSelectPoint.X + m_WaferRecipe.ORIGIN_X) < 0d)
            //            {
            //                iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X - m_WaferRecipe.ORIGIN_X) / m_WaferRecipe.DIE_SIZE_X);
            //            }
            //            else
            //            {
            //                iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X - m_WaferRecipe.ORIGIN_X) / m_WaferRecipe.DIE_SIZE_X);
            //                //iDieIndexX = m_WaferRecipe.ORIGIN_DIE_X + (int)((pdSelectPoint.X - m_WaferRecipe.ORIGIN_X + m_WaferRecipe.DIE_SIZE_X) / m_WaferRecipe.DIE_SIZE_X);
            //            }
            //            if ((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y) < 0d)
            //            {
            //                iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //            }
            //            else
            //            {
            //                iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //                //iDieIndexY = m_WaferRecipe.ORIGIN_DIE_Y + (int)((pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y + m_WaferRecipe.DIE_SIZE_Y) / m_WaferRecipe.DIE_SIZE_Y);
            //            }

            //            /// X는 역수 취함
            //            iDieIndexX = m_WaferRecipe.DIE_INDEX_MIN_X + (m_WaferRecipe.DIE_INDEX_MAX_X - iDieIndexX);
            //            break;

            //    }

            //    //switch (m_WaferRecipe.XYDIR)
            //    //{
            //    //    case XYDirection.LeftBottom:
            //    //        //iDieIndexX = iDieIndexX;
            //    //        //iDieIndexY = iDieIndexY;
            //    //        break;
            //    //    case XYDirection.LeftTop:
            //    //        //iDieIndexX = iDieIndexX;
            //    //        //iDieIndexY = m_WaferRecipe.DIE_INDEX_MIN_Y + (m_WaferRecipe.DIE_INDEX_MAX_Y - iDieIndexY);
            //    //        break;

            //    //    case XYDirection.RightBottom:
            //    //        iDieIndexX = m_WaferRecipe.DIE_INDEX_MIN_X + (m_WaferRecipe.DIE_INDEX_MAX_X - iDieIndexX + 1);
            //    //        //iDieIndexY = iDieIndexY;
            //    //        break;
            //    //    case XYDirection.RightTop:
            //    //        iDieIndexX = m_WaferRecipe.DIE_INDEX_MIN_X + (m_WaferRecipe.DIE_INDEX_MAX_X - iDieIndexX + 1);
            //    //        iDieIndexY = m_WaferRecipe.DIE_INDEX_MIN_Y + (m_WaferRecipe.DIE_INDEX_MAX_Y - iDieIndexY + 1);
            //    //        //iDieIndexY = m_WaferRecipe.DIE_INDEX_MIN_Y + (m_WaferRecipe.DIE_INDEX_MAX_Y - iDieIndexY);
            //    //        break;
            //    //}

            //    m_iCurrentDieIndex = m_DieIndexer.IndexOf(new Point(iDieIndexX, iDieIndexY));

            //    if (m_iCurrentDieIndex < 0)
            //    {
            //        this.ClearMarkDie();
            //        return;
            //    }

            //    Die oDie = (Die)this.m_arrDies[m_iCurrentDieIndex];

            //    //if (m_eoMapMode == MapMode.Edit) this.RedrawDie(oDie, Color.Pink, Color.Red); ;

            //    if (m_iCurrentX != iDieIndexX || m_iCurrentY != iDieIndexY)
            //    {
            //        if (OnChangeCurrentDie != null)
            //        {
            //            oDie.IndexX = iDieIndexX;
            //            oDie.IndexY = iDieIndexY;
            //            OnChangeCurrentDie(this, oDie);
            //        }
            //    }

            //    m_iCurrentX = iDieIndexX;
            //    m_iCurrentY = iDieIndexY;

            //    if (OnChangePosition != null)
            //    {
            //        PointD pdReal = new PointD(pdSelectPoint.X + m_WaferRecipe.ORIGIN_X, pdSelectPoint.Y + m_WaferRecipe.ORIGIN_Y);
            //        OnChangePosition(this, pdSelectPoint, pdReal);
            //    }
            //}
        }


        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);

            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    switch (m_eoMouseDragMode)
                    {
                        case MouseDragMode.Zoom:
                            if (m_eoMapMode == MapMode.Fit) break;
                            if (m_rectSelect.Width * m_rectSelect.Height == 0) return;

                            RectangleD rectdBefore = m_rectdWaferArea;
                            double fBeforeZoomRatio = m_dZoomRatio;

                            double fRatioWidth = (double)this.Width / m_rectSelect.Width;
                            double fRatioHeight = (double)this.Height / m_rectSelect.Height;

                            PointD pdCenter = new PointD((m_rectSelect.X + m_rectSelect.Width / 2) / fBeforeZoomRatio + m_rectdWaferArea.X
                                , (m_rectSelect.Y + m_rectSelect.Height / 2) / fBeforeZoomRatio + m_rectdWaferArea.Y);

                            m_dZoomRatio = Math.Min(fRatioHeight, fRatioWidth) * fBeforeZoomRatio;
                            m_dZoomRatio_X_m_dScale = m_dZoomRatio * m_dScale;

                            m_rectdWaferArea.Width = this.Width / m_dZoomRatio;
                            m_rectdWaferArea.Height = this.Height / m_dZoomRatio;

                            m_rectdWaferArea.X = pdCenter.X - m_rectdWaferArea.Width / 2;
                            m_rectdWaferArea.Y = pdCenter.Y - m_rectdWaferArea.Height / 2;
                            break;
                        case MouseDragMode.Move:
                            if (m_eoMapMode == MapMode.Fit) break;
                            m_rectdWaferArea.X = m_rectdWaferArea.X - (m_poEnd.X - m_poStart.X) / m_dZoomRatio;
                            m_rectdWaferArea.Y = m_rectdWaferArea.Y - (m_poEnd.Y - m_poStart.Y) / m_dZoomRatio;
                            break;
                        case MouseDragMode.Rotate:
                            DrawRotate();
                            break;
                        case MouseDragMode.Zone:
                            CalSelectedDie();
                            break;
                    }

                    //if (m_SelectPath != null) m_SelectPath.Dispose();
                }
            }
            finally
            {
                Redraw();
            }
        }
        #endregion

        #region ■ Real Position 계산 관련 (Rotate,GetRealPoint,GetViewPoint)
        public void Rotate(double Angle)
        {
            try
            {
                Angle = Angle % 360;
                m_iViewAngle = (int)(Angle + (double)m_iViewAngle);

                Redraw();
                if (m_iViewAngle >= 0)
                {
                    m_iViewAngle = m_iViewAngle % 360;
                }
                else
                {
                    m_iViewAngle = (int)(360.0d + ((double)m_iViewAngle % 360));
                }
            }
            catch (Exception) { }
        }

        protected PointD GetRealPoint(int piX, int piY)
        {
            PointD pdCenter;
            PointF pfWaferCenterToPixel = GetViewPoint(0.0d, 0.0d);
            PointD pdBefor;

            pdBefor.X = (double)(pfWaferCenterToPixel.X - piX);
            pdBefor.Y = (double)(pfWaferCenterToPixel.Y - piY);

            RotatePoint(ref pdBefor.X, ref pdBefor.Y, m_iViewAngle);
            piX = (int)(pdBefor.X + pfWaferCenterToPixel.X);
            piY = (int)(pdBefor.Y + pfWaferCenterToPixel.Y);

            pdCenter.X = m_WaferRecipe.WAFER_RADIUS - (m_rectdWaferArea.X + piX / m_dZoomRatio_X_m_dScale);
            pdCenter.Y = -m_WaferRecipe.WAFER_RADIUS + (m_rectdWaferArea.Y + piY / m_dZoomRatio_X_m_dScale);
            return pdCenter;
        }

        private PointF GetViewPoint(double dX, double dY)
        {
            PointF pfCenter = PointF.Empty;
            pfCenter.X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + dX) * m_dZoomRatio_X_m_dScale);
            pfCenter.Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - dY) * m_dZoomRatio_X_m_dScale);
            return pfCenter;
        }
        #endregion

        #region ■ Edit 및 선택 영역 표시 함수[ DrawSelectRect() / DrawMoveRect() ]
        protected void DrawSelectRect()
        {
            if (m_bmpWaferMap == null) return;

            Graphics gdiTemp = null;
            Pen penPath = null;
            try
            {
                gdiTemp = Graphics.FromImage(m_bmpTemp);
                gdiTemp.DrawImageUnscaled(m_bmpWaferMap, 0, 0);

                if (m_rectSelect.Width * m_rectSelect.Height > 0)
                {
                    penPath = new Pen(m_hbSelectBrush, 3);
                    gdiTemp.DrawRectangle(penPath, m_rectSelect.X, m_rectSelect.Y, m_rectSelect.Width - 1, m_rectSelect.Height - 1);
                }
                m_gdiMain.DrawImageUnscaled(m_bmpTemp, 0, 0);
            }
            catch
            {
            }
            finally
            {
                if (gdiTemp != null) gdiTemp.Dispose();
                if (penPath != null) penPath.Dispose();
            }
        }

        private void DrawMoveRect()
        {
            Graphics gdiTemp = null;
            Pen penPath = null;
            try
            {
                gdiTemp = Graphics.FromImage(m_bmpTemp);
                gdiTemp.DrawImageUnscaled(m_bmpWaferMap, 0, 0);

                m_rectSelect = new Rectangle(0, 0, this.Width, this.Height);
                m_rectSelect.Offset(m_poEnd.X - m_poStart.X, m_poEnd.Y - m_poStart.Y);


                if (m_rectSelect.Width * m_rectSelect.Height > 0)
                {
                    penPath = new Pen(m_hbSelectBrush, 3);
                    gdiTemp.DrawRectangle(penPath, m_rectSelect);
                }
                m_gdiMain.DrawImageUnscaled(m_bmpTemp, 0, 0);
            }
            catch
            {
            }
            finally
            {
                if (gdiTemp != null) gdiTemp.Dispose();
                if (penPath != null) penPath.Dispose();
            }
        }

        #endregion

        #region ■ Zone 선택 모양을 그리는 함수
        protected void DrawZoneGuid()
        {
            PointD pdStart;
            PointD pdEnd;
            double dStartValue = 0.0d;
            double dEndValue = 0.0d;
            Graphics gdiTemp = null;
            Pen penPath = null;
            SolidBrush sbrshPath = null;
            try
            {
                gdiTemp = Graphics.FromImage(m_bmpTemp);
                gdiTemp.DrawImageUnscaled(m_bmpWaferMap, 0, 0);
                penPath = new Pen(m_hbSelectBrush, 3);

                if (m_eoMapSelectStyle == MapSelectStyle.FreeHand)
                {
                    m_SelectPath.AddLine(m_poStart, m_poEnd);
                    m_poStart = m_poEnd;

                    gdiTemp.DrawPath(penPath, m_SelectPath);

                    m_gdiMain.DrawImageUnscaled(m_bmpTemp, 0, 0);
                    return;
                }

                if (m_SelectPath == null)
                    return;

                m_SelectPath.Reset();
                switch (m_eoMapSelectStyle)
                {
                    case MapSelectStyle.Band:
                        pdStart = GetRealPoint(m_poStart.X, m_poStart.Y);
                        dStartValue = Math.Sqrt(Math.Pow(pdStart.X, 2) + Math.Pow(pdStart.Y, 2)) * 2;

                        m_rectSelect.X = (int)((-m_rectdWaferArea.X - dStartValue / 2 + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale);
                        m_rectSelect.Y = (int)((-m_rectdWaferArea.Y - dStartValue / 2 + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale);
                        m_rectSelect.Width = (int)(dStartValue * m_dZoomRatio_X_m_dScale);
                        m_rectSelect.Height = (int)(dStartValue * m_dZoomRatio_X_m_dScale);

                        pdEnd = GetRealPoint(m_poEnd.X, m_poEnd.Y);
                        dEndValue = Math.Sqrt(Math.Pow(pdEnd.X, 2) + Math.Pow(pdEnd.Y, 2)) * 2;

                        m_rectSelect2.X = (int)((-m_rectdWaferArea.X - dEndValue / 2 + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale);
                        m_rectSelect2.Y = (int)((-m_rectdWaferArea.Y - dEndValue / 2 + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale);
                        m_rectSelect2.Width = (int)(dEndValue * m_dZoomRatio_X_m_dScale);
                        m_rectSelect2.Height = (int)(dEndValue * m_dZoomRatio_X_m_dScale);

                        m_SelectPath.AddEllipse(m_rectSelect);
                        m_SelectPath.AddEllipse(m_rectSelect2);

                        break;
                    case MapSelectStyle.Circle:
                        if (m_rectSelect.Width * m_rectSelect.Height > 0)
                        {
                            m_SelectPath.AddEllipse(m_rectSelect.X, m_rectSelect.Y, m_rectSelect.Width - 1, m_rectSelect.Height - 1);
                        }
                        break;
                    case MapSelectStyle.Pie:
                        pdStart = GetRealPoint(m_poStart.X, m_poStart.Y);
                        pdEnd = GetRealPoint(m_poEnd.X, m_poEnd.Y);

                        if (pdStart.Y > 0)
                        {
                            dStartValue = -Math.Acos(pdStart.X / Math.Sqrt(Math.Pow(pdStart.X, 2) + Math.Pow(pdStart.Y, 2))) * (180 / Math.PI) % 360;
                        }
                        else
                        {
                            dStartValue = Math.Acos(pdStart.X / Math.Sqrt(Math.Pow(pdStart.X, 2) + Math.Pow(pdStart.Y, 2))) * (180 / Math.PI) % 360;
                        }

                        if (pdEnd.Y > 0)
                        {
                            dEndValue = -Math.Acos(pdEnd.X / Math.Sqrt(Math.Pow(pdEnd.X, 2) + Math.Pow(pdEnd.Y, 2))) * (180 / Math.PI) % 360;
                        }
                        else
                        {
                            dEndValue = Math.Acos(pdEnd.X / Math.Sqrt(Math.Pow(pdEnd.X, 2) + Math.Pow(pdEnd.Y, 2))) * (180 / Math.PI) % 360;
                        }


                        m_rectSelect.X = (int)(-m_rectdWaferArea.X * m_dZoomRatio_X_m_dScale);
                        m_rectSelect.Y = (int)(-m_rectdWaferArea.Y * m_dZoomRatio_X_m_dScale);
                        m_rectSelect.Width = (int)(m_WaferRecipe.WAFER_SIZE * m_dZoomRatio_X_m_dScale);
                        m_rectSelect.Height = (int)(m_WaferRecipe.WAFER_SIZE * m_dZoomRatio_X_m_dScale);
                        m_SelectPath.AddPie(m_rectSelect, (float)dStartValue, (float)((dEndValue - dStartValue + 360) % 360));
                        break;
                    case MapSelectStyle.Rectangle:
                        if (m_rectSelect.Width * m_rectSelect.Height > 0)
                        {
                            m_SelectPath.AddRectangle(m_rectSelect);
                        }
                        break;
                }
                sbrshPath = new SolidBrush(Color.FromArgb(80, Color.Tomato));
                gdiTemp.FillPath(sbrshPath, m_SelectPath);
                gdiTemp.DrawPath(penPath, m_SelectPath);
                m_gdiMain.DrawImageUnscaled(m_bmpTemp, 0, 0);
            }
            catch
            {

            }
            finally
            {
                if (gdiTemp != null) gdiTemp.Dispose();
                if (penPath != null) penPath.Dispose();
                if (sbrshPath != null) sbrshPath.Dispose();
            }
        }
        #endregion

        #region ▣ Die선택시 선택된 Die들 계산하여 Event 발생
        protected void CalSelectedDie()
        {
            PointD pdDieCenter;

            if (m_eoMapSelectStyle == MapSelectStyle.FreeHand)
                m_SelectPath.CloseFigure();

            int iTmp = -1;

            try
            {
                if (ModifierKeys != Keys.Shift && ModifierKeys != Keys.ShiftKey &&
                    ModifierKeys != Keys.Control && ModifierKeys != Keys.ControlKey)
                    m_SelectedDies.Clear();

                if (m_eoMouseDragMode != MouseDragMode.Zone)
                    return;

                if (m_SelectPath == null)
                    return;

                foreach (Die InDie in m_arrDies)
                {
                    if (InDie.DieProp == DIE_PROP_VIRTUAL_DIE)
                        continue;

                    pdDieCenter.X = (InDie.DieCood.X - m_WaferRecipe.ORIGIN_X) + InDie.DieCood.Width / 2;
                    pdDieCenter.Y = (InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y) + InDie.DieCood.Height / 2;

                    RotatePoint(ref pdDieCenter.X, ref pdDieCenter.Y, m_iViewAngle);

                    PointF pfPoint = PointF.Empty;
                    pfPoint.X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdDieCenter.X) * m_dZoomRatio_X_m_dScale);
                    pfPoint.Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdDieCenter.Y) * m_dZoomRatio_X_m_dScale);

                    if (m_SelectPath.IsVisible(pfPoint))
                    {
                        iTmp = m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY));

                        if (ModifierKeys == Keys.Control)
                        {
                            if (iTmp > -1)
                                m_SelectedDies.RemoveAt(iTmp);
                        }
                        else
                        {
                            if (iTmp < 0)
                                m_SelectedDies.Add(new Point(InDie.IndexX, InDie.IndexY));
                        }
                    }
                }
                if (OnSelectDies != null) OnSelectDies(this, m_SelectedDies);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ■ Rotate 시각적 표현
        private void DrawRotate()
        {
            if (this.Width / 2 > m_poStart.Y)
            {
                for (int i = 0; i < 6; i++)
                {
                    Rotate(-15);
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    Rotate(15);
                }
            }
        }

        #endregion

        #region ■ Scale 그리는 함수
        private void DrawScale(Graphics g)
        {
            double dStartX = -m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.ORIGIN_X;
            double dStartY = -m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS + m_WaferRecipe.ORIGIN_Y;
            Pen penScale = new Pen(Color.LightGray);
            SolidBrush brsScale = new SolidBrush(Color.LightGray);
            Font fontScale = new Font("굴림", 9);
            int iDieX = 0;
            int iDieY = 0;
            try
            {
                for (int ix = m_WaferRecipe.DIE_INDEX_MIN_X; ix <= m_WaferRecipe.DIE_INDEX_MAX_X; ix++)
                {
                    switch (m_WaferRecipe.XYDIR)
                    {
                        case XYDirection.LeftTop:
                            iDieX = ix;
                            break;
                        case XYDirection.LeftBottom:
                            iDieX = ix;
                            break;
                        case XYDirection.RightBottom:
                            iDieX = this.m_WaferRecipe.ORIGIN_DIE_X - (ix - this.m_WaferRecipe.ORIGIN_DIE_X);
                            break;
                        case XYDirection.RightTop:
                            iDieX = this.m_WaferRecipe.ORIGIN_DIE_X - (ix - this.m_WaferRecipe.ORIGIN_DIE_X);
                            break;
                    }

                    if ((Math.Abs(ix) % 10) < 5)
                    {
                        g.DrawRectangle(penScale
                            , (float)((dStartX + (ix - m_WaferRecipe.ORIGIN_DIE_X) * m_WaferRecipe.DIE_SIZE_X) * m_dZoomRatio_X_m_dScale)
                            , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025))
                            , (float)(m_WaferRecipe.DIE_SIZE_X * m_dZoomRatio_X_m_dScale)
                            , (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale));
                        switch ((Math.Abs(ix) % 10))
                        {
                            case 0:
                                g.DrawString(string.Format("{0}", ix), fontScale, brsScale
                                    , (float)((dStartX + (ix - m_WaferRecipe.ORIGIN_DIE_X) * m_WaferRecipe.DIE_SIZE_X) * m_dZoomRatio_X_m_dScale - 7)
                                    , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025)) + (float)(m_WaferRecipe.WAFER_SIZE * 0.01d * m_dZoomRatio_X_m_dScale));

                                break;
                            case 4:
                                if (ix > 0)
                                {
                                    g.DrawString(string.Format("{0}", ix + 1), fontScale, brsScale
                                        , (float)((dStartX + ((ix - m_WaferRecipe.ORIGIN_DIE_X) + 1) * m_WaferRecipe.DIE_SIZE_X) * m_dZoomRatio_X_m_dScale - 7)
                                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025)) + (float)(m_WaferRecipe.WAFER_SIZE * 0.01d * m_dZoomRatio_X_m_dScale));
                                }
                                if (ix < 0)
                                {
                                    g.DrawString(string.Format("{0}", ix - 1), fontScale, brsScale
                                        , (float)((dStartX + ((ix - m_WaferRecipe.ORIGIN_DIE_X) - 1) * m_WaferRecipe.DIE_SIZE_X) * m_dZoomRatio_X_m_dScale - 7)
                                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025)) + (float)(m_WaferRecipe.WAFER_SIZE * 0.01d * m_dZoomRatio_X_m_dScale));

                                    g.DrawRectangle(penScale
                                        , (float)((dStartX + ((ix - 1) - m_WaferRecipe.ORIGIN_DIE_X) * m_WaferRecipe.DIE_SIZE_X) * m_dZoomRatio_X_m_dScale)
                                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025))
                                        , (float)(m_WaferRecipe.DIE_SIZE_X * m_dZoomRatio_X_m_dScale)
                                        , (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale));

                                    g.FillRectangle(brsScale, (float)((dStartX + ((ix - 1) - m_WaferRecipe.ORIGIN_DIE_X) * m_WaferRecipe.DIE_SIZE_X) * m_dZoomRatio_X_m_dScale)
                                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025))
                                        , (float)(m_WaferRecipe.DIE_SIZE_X * m_dZoomRatio_X_m_dScale)
                                        , (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale));
                                }
                                break;
                            case 1:
                            case 3:
                                g.FillRectangle(brsScale, (float)((dStartX + (ix - m_WaferRecipe.ORIGIN_DIE_X) * m_WaferRecipe.DIE_SIZE_X) * m_dZoomRatio_X_m_dScale)
                                    , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025))
                                    , (float)(m_WaferRecipe.DIE_SIZE_X * m_dZoomRatio_X_m_dScale)
                                    , (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale));
                                break;
                        }
                    }
                }



                for (int iy = m_WaferRecipe.DIE_INDEX_MIN_Y; iy <= m_WaferRecipe.DIE_INDEX_MAX_Y; iy++)
                {
                    switch (m_WaferRecipe.XYDIR)
                    {
                        case XYDirection.LeftTop:
                            iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (iy - this.m_WaferRecipe.ORIGIN_DIE_Y) + 1;
                            break;
                        case XYDirection.LeftBottom:
                            iDieY = iy;
                            break;
                        case XYDirection.RightBottom:
                            iDieY = iy;
                            break;
                        case XYDirection.RightTop:
                            iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (iy - this.m_WaferRecipe.ORIGIN_DIE_Y) - 1;
                            break;
                    }

                    if (Math.Abs(iDieY % 10) < 5)
                    {
                        g.DrawRectangle(penScale, (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025))
                            , (float)((dStartY - ((iDieY - m_WaferRecipe.ORIGIN_DIE_Y) + 1) * m_WaferRecipe.DIE_SIZE_Y) * m_dZoomRatio_X_m_dScale)
                            , (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale)
                            , (float)(m_WaferRecipe.DIE_SIZE_Y * m_dZoomRatio_X_m_dScale));
                        switch (Math.Abs(iDieY % 10))
                        {
                            case 0:
                                g.DrawString(string.Format("{0}", iDieY), fontScale, brsScale
                                    , (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025)) + (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale)
                                    , (float)((dStartY - (iDieY - m_WaferRecipe.ORIGIN_DIE_Y) * m_WaferRecipe.DIE_SIZE_Y) * m_dZoomRatio_X_m_dScale) - 4);
                                break;
                            case 4:
                                if (iDieY > 0)
                                {
                                    g.DrawString(string.Format("{0}", iDieY + 1), fontScale, brsScale
                                        , (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025)) + (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale)
                                        , (float)((dStartY - ((iDieY - m_WaferRecipe.ORIGIN_DIE_Y) + 1) * m_WaferRecipe.DIE_SIZE_Y) * m_dZoomRatio_X_m_dScale) - 4);
                                }
                                if (iDieY < 0)
                                {
                                    g.DrawString(string.Format("{0}", iDieY - 1), fontScale, brsScale
                                        , (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025)) + (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale)
                                        , (float)((dStartY - ((iDieY - m_WaferRecipe.ORIGIN_DIE_Y) + 1) * m_WaferRecipe.DIE_SIZE_Y) * m_dZoomRatio_X_m_dScale) - 4);

                                    g.FillRectangle(brsScale
                                        , (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025))
                                        , (float)((dStartY - (((iDieY - 1) - m_WaferRecipe.ORIGIN_DIE_Y) + 1) * m_WaferRecipe.DIE_SIZE_Y) * m_dZoomRatio_X_m_dScale)
                                        , (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale)
                                        , (float)(m_WaferRecipe.DIE_SIZE_Y * m_dZoomRatio_X_m_dScale));

                                    g.DrawRectangle(penScale, (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025))
                                        , (float)((dStartY - (((iDieY - 1) - m_WaferRecipe.ORIGIN_DIE_Y) + 1) * m_WaferRecipe.DIE_SIZE_Y) * m_dZoomRatio_X_m_dScale)
                                        , (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale)
                                        , (float)(m_WaferRecipe.DIE_SIZE_Y * m_dZoomRatio_X_m_dScale));
                                }

                                break;
                            case 1:
                            case 3:
                                g.FillRectangle(brsScale
                                    , (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio_X_m_dScale * 1.025))
                                    , (float)((dStartY - ((iDieY - m_WaferRecipe.ORIGIN_DIE_Y) + 1) * m_WaferRecipe.DIE_SIZE_Y) * m_dZoomRatio_X_m_dScale)
                                    , (float)(m_WaferRecipe.WAFER_SIZE * 0.005d * m_dZoomRatio_X_m_dScale)
                                    , (float)(m_WaferRecipe.DIE_SIZE_Y * m_dZoomRatio_X_m_dScale));
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                penScale.Dispose();
                brsScale.Dispose();
                fontScale.Dispose();
            }

        }
        #endregion

        #region ■ Draw Wafer
        protected virtual void DrawWafer()
        {
            if (this.Width * this.Height == 0 || m_gdiTempMap == null) return;

            PointD dNotchStart;
            PointD dNotchEnd;
            GraphicsPath gpWafer = null;

            m_dZoomRatio_X_m_dScale = m_dZoomRatio * m_dScale;

            m_gdiTempMap.Clear(Color.White);
            SolidBrush sbrshEdge = null;
            SolidBrush sbrshWafer = null;
            Pen pEdge = null;
            Pen pWafer = null;
            //Font fntWID = new Font("굴림", Math.Max(8, (float)((m_dZoomRatio * m_dScale) * 2.3f)));
            Font fntWID = new Font("굴림", Math.Max(9, (float)(m_dZoomRatio_X_m_dScale * 1.5f)));
            m_fntBin = new Font("굴림", Math.Max(9, (float)(m_dZoomRatio_X_m_dScale * 1.5f)));
            m_fntWBin = new Font("굴림", Math.Max(9, (float)(m_dZoomRatio_X_m_dScale * 1.5f)));
            int iRealAngle = (m_WaferRecipe.ANGLE + m_iAngleOffset) % 360;
            try
            {
                if (m_WaferRecipe.WAFER_SIZE == 0)
                    return;

                gpWafer = new GraphicsPath();

                if (m_bScale) DrawScale(m_gdiTempMap);

                ///=======Wafer Edge Drawing ========================================================================
                gpWafer.AddArc((float)((-m_rectdWaferArea.X + m_WaferRecipe.EDGE_SIZE) * m_dZoomRatio_X_m_dScale)
                    , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.EDGE_SIZE) * m_dZoomRatio_X_m_dScale)
                    , (float)((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) * m_dZoomRatio_X_m_dScale)
                    , (float)((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) * m_dZoomRatio_X_m_dScale)
                    , (float)((m_iViewAngle + iRealAngle + 90 + m_dNotchSize / 2) % 360)
                    , (float)(360 - m_dNotchSize));

                if (m_WaferRecipe.NOTCH_TYPE != Notch.Notch)
                {
                    dNotchStart.X = -(float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                    dNotchStart.Y = -(float)Math.Cos((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                    dNotchEnd.X = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                    dNotchEnd.Y = dNotchStart.Y;

                    RotatePoint(ref dNotchStart.X, ref dNotchStart.Y, (iRealAngle + m_iViewAngle) % 360);
                    RotatePoint(ref dNotchEnd.X, ref dNotchEnd.Y, (iRealAngle + m_iViewAngle) % 360);

                    gpWafer.AddLine((float)((-m_rectdWaferArea.X + dNotchStart.X + (m_WaferRecipe.WAFER_RADIUS)) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y +m_WaferRecipe.WAFER_RADIUS- dNotchStart.Y) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.X + dNotchEnd.X + (m_WaferRecipe.WAFER_RADIUS)) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y +m_WaferRecipe.WAFER_RADIUS- dNotchEnd.Y) * m_dZoomRatio_X_m_dScale));
                }
                ///==================================================================================================
                ///
                sbrshWafer = new SolidBrush(m_colWafer);
                pWafer = new Pen(m_colWafer);
                m_gdiTempMap.FillPath(sbrshWafer, gpWafer);
                m_gdiTempMap.DrawPath(pWafer, gpWafer);

                gpWafer.StartFigure();

                ///=======Wafer Line Drawing ========================================================================
                ///
                gpWafer.AddArc((float)(-m_rectdWaferArea.X * m_dZoomRatio_X_m_dScale)
                    , (float)(-m_rectdWaferArea.Y * m_dZoomRatio_X_m_dScale)
                    , (float)(m_WaferRecipe.WAFER_SIZE * m_dZoomRatio_X_m_dScale)
                    , (float)(m_WaferRecipe.WAFER_SIZE * m_dZoomRatio_X_m_dScale)
                    , (float)((m_iViewAngle + iRealAngle + 90 + m_dNotchSize / 2) % 360)
                    , (float)(360 - m_dNotchSize));

                dNotchStart.X = -(float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_RADIUS);
                dNotchStart.Y = -(float)Math.Cos((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_RADIUS);
                dNotchEnd.X = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_RADIUS);
                dNotchEnd.Y = dNotchStart.Y;

                RotatePoint(ref dNotchStart.X, ref dNotchStart.Y, (iRealAngle + m_iViewAngle) % 360);
                RotatePoint(ref dNotchEnd.X, ref dNotchEnd.Y, (iRealAngle + m_iViewAngle) % 360);

                if (m_WaferRecipe.NOTCH_TYPE == Notch.Notch)
                {
                    PointD dNotchRStart;
                    PointD dNotchREnd;
                    double dNotchR = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_RADIUS);
                    dNotchRStart.X = (dNotchEnd.X + dNotchStart.X) / 2 - dNotchR;
                    dNotchRStart.Y = (dNotchEnd.Y + dNotchStart.Y) / 2;
                    dNotchREnd.X = (dNotchEnd.X + dNotchStart.X) / 2 + dNotchR;
                    dNotchREnd.Y = (dNotchEnd.Y + dNotchStart.Y) / 2;

                    gpWafer.AddArc((float)((-m_rectdWaferArea.X + dNotchRStart.X + (m_WaferRecipe.WAFER_RADIUS)) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y - dNotchRStart.Y +m_WaferRecipe.WAFER_RADIUS- (dNotchREnd.X - dNotchRStart.X) / 2) * m_dZoomRatio_X_m_dScale)
                        , (float)((dNotchREnd.X - dNotchRStart.X) * m_dZoomRatio_X_m_dScale)
                        , (float)((dNotchREnd.X - dNotchRStart.X) * m_dZoomRatio_X_m_dScale)
                        , (float)((m_iViewAngle + iRealAngle + 180 + m_dNotchSize / 2) % 360)
                        , (float)(180));
                }
                else
                {
                    gpWafer.AddLine((float)((-m_rectdWaferArea.X + dNotchStart.X + (m_WaferRecipe.WAFER_RADIUS)) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y +m_WaferRecipe.WAFER_RADIUS- dNotchStart.Y) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.X + dNotchEnd.X + (m_WaferRecipe.WAFER_RADIUS)) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y +m_WaferRecipe.WAFER_RADIUS- dNotchEnd.Y) * m_dZoomRatio_X_m_dScale));
                }


                ///========================================================================================================================
                ///
                sbrshEdge = new SolidBrush(m_colEdge);
                pEdge = new Pen(m_colEdge);
                m_gdiTempMap.FillPath(sbrshEdge, gpWafer);
                m_gdiTempMap.DrawPath(pEdge, gpWafer);


                /// Notch부분의 사각형을 정의한다.
                ///======================================================================================================================== 
                PointD[] pdPoint = new PointD[4];

                pdPoint[0].X = -Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdPoint[0].Y = -Math.Cos((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdPoint[1].X = Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdPoint[1].Y = pdPoint[0].Y;

                pdPoint[2].X = pdPoint[1].X;
                pdPoint[2].Y = -(m_WaferRecipe.WAFER_RADIUS);
                pdPoint[3].X = pdPoint[0].X;
                pdPoint[3].Y = -(m_WaferRecipe.WAFER_RADIUS);

                RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, (iRealAngle) % 360);
                RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, (iRealAngle) % 360);
                RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, (iRealAngle) % 360);
                RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, (iRealAngle) % 360);

                double dMinX = Math.Min(Math.Min(pdPoint[0].X, pdPoint[1].X), Math.Min(pdPoint[2].X, pdPoint[3].X));
                double dMaxX = Math.Max(Math.Max(pdPoint[0].X, pdPoint[1].X), Math.Max(pdPoint[2].X, pdPoint[3].X));
                double dMinY = Math.Min(Math.Min(pdPoint[0].Y, pdPoint[1].Y), Math.Min(pdPoint[2].Y, pdPoint[3].Y));
                double dMaxY = Math.Max(Math.Max(pdPoint[0].Y, pdPoint[1].Y), Math.Max(pdPoint[2].Y, pdPoint[3].Y));

                m_rectNotchArea.X = (float)dMinX;
                m_rectNotchArea.Y = (float)dMaxY;
                m_rectNotchArea.Width = (float)(dMaxX - dMinX);
                m_rectNotchArea.Height = (float)(dMaxY - dMinY);
                ///========================================================================================================================
                /// 
                /// 
                /// OCRID부분의 사각형을 정의한다.
                ///======================================================================================================================== 
                PointD[] pdOCRPoint = new PointD[4];

                pdOCRPoint[0].X = -Math.Sin(8.0d / 180.0d * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdOCRPoint[0].Y = -Math.Cos(8.0d / 180.0d * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdOCRPoint[1].X = Math.Sin(8.0d / 180.0d * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdOCRPoint[1].Y = pdOCRPoint[0].Y;

                pdOCRPoint[2].X = pdOCRPoint[1].X;
                pdOCRPoint[2].Y = -(m_WaferRecipe.WAFER_RADIUS);
                pdOCRPoint[3].X = pdOCRPoint[0].X;
                pdOCRPoint[3].Y = -(m_WaferRecipe.WAFER_RADIUS);

                RotatePoint(ref pdOCRPoint[0].X, ref pdOCRPoint[0].Y, (iRealAngle) % 360);
                RotatePoint(ref pdOCRPoint[1].X, ref pdOCRPoint[1].Y, (iRealAngle) % 360);
                RotatePoint(ref pdOCRPoint[2].X, ref pdOCRPoint[2].Y, (iRealAngle) % 360);
                RotatePoint(ref pdOCRPoint[3].X, ref pdOCRPoint[3].Y, (iRealAngle) % 360);

                double dOCRMinX = Math.Min(Math.Min(pdOCRPoint[0].X, pdOCRPoint[1].X), Math.Min(pdOCRPoint[2].X, pdOCRPoint[3].X));
                double dOCRMaxX = Math.Max(Math.Max(pdOCRPoint[0].X, pdOCRPoint[1].X), Math.Max(pdOCRPoint[2].X, pdOCRPoint[3].X));
                double dOCRMinY = Math.Min(Math.Min(pdOCRPoint[0].Y, pdOCRPoint[1].Y), Math.Min(pdOCRPoint[2].Y, pdOCRPoint[3].Y));
                double dOCRMaxY = Math.Max(Math.Max(pdOCRPoint[0].Y, pdOCRPoint[1].Y), Math.Max(pdOCRPoint[2].Y, pdOCRPoint[3].Y));

                m_rectOCRIDArea.X = (float)dOCRMinX;
                m_rectOCRIDArea.Y = (float)dOCRMaxY;
                m_rectOCRIDArea.Width = (float)(dOCRMaxX - dOCRMinX);
                m_rectOCRIDArea.Height = (float)(dOCRMaxY - dOCRMinY);
                ///========================================================================================================================

                DrawDies(m_gdiTempMap);

                DrawShot(m_gdiTempMap);

                DrawCenteGrid(m_gdiTempMap);

                /// Wafer ID Draw
                ///========================================================================================================================
                ///
                if (m_strInfomation != null && m_bVisibleInfo)
                {
                    for (int i = 0; i < m_strInfomation.Count; i++)
                    {
                        m_gdiTempMap.DrawString(m_strInfomation[i].ToString(), fntWID, Brushes.Black,
                            2, Height - fntWID.Height * m_strInfomation.Count + fntWID.Height * i);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (gpWafer != null) gpWafer.Dispose();
                if (sbrshEdge != null) sbrshEdge.Dispose();
                if (pEdge != null) pEdge.Dispose();
                if (sbrshWafer != null) sbrshWafer.Dispose();
                if (pWafer != null) pWafer.Dispose();
                if (fntWID != null) fntWID.Dispose();
            }
        }

        #endregion

        #region ■ Point좌표를 주어진 각도로 Rotation하는 함수
        protected void RotatePoint(ref double dx, ref double dy, double RAngle)
        {
            if (RAngle == 0)
                return;

            double a = RAngle / 180d * Math.PI;

            double tmpX = dx * Math.Cos(a) + dy * Math.Sin(a);
            double tmpY = -dx * Math.Sin(a) + dy * Math.Cos(a);

            dx = tmpX;
            dy = tmpY;
        }

        #endregion

        #region ■ [사용안함] 주어진 조건으로 Wafer 안에 들어갈 Die들을 구하는 함수
        //public virtual void DieCalculation()
        //{
        //    double dDiePitchX = (m_WaferRecipe.WAFER_SIZE * m_dMargin) / m_WaferRecipe.XDIES;
        //    double dDiePitchY = (m_WaferRecipe.WAFER_SIZE * m_dMargin) / m_WaferRecipe.YDIES;
        //    double dBeginX = (m_WaferRecipe.WAFER_SIZE * m_dMargin) / 2d;
        //    double dBeginY = (m_WaferRecipe.WAFER_SIZE * m_dMargin) / 2d;

        //    double dX = 0.0d;
        //    double dY = 0.0d;

        //    m_WaferRecipe.DIE_SIZE_X = dDiePitchX;
        //    m_WaferRecipe.DIE_SIZE_Y = dDiePitchY;
        //    this.m_WaferRecipe.NETDIE = 0;

        //    if ((m_WaferRecipe.XDIES % 2) == 1)
        //    {
        //        m_WaferRecipe.ORIGIN_X = dDiePitchX / 2;
        //    }

        //    if ((m_WaferRecipe.YDIES % 2) == 1)
        //    {
        //        m_WaferRecipe.ORIGIN_Y = dDiePitchY / 2;
        //    }

        //    int iBin = 0;
        //    DieClear();
        //    Die oDie = new Die(0, 0, 0, 0, 0, 0, 0);
        //    try
        //    {
        //        switch (XYDirect)
        //        {
        //            case XYDirection.LeftBottom:
        //                #region LEFT-BOTTOM
        //                dBeginX = -dBeginX + m_WaferRecipe.ORIGIN_X;
        //                dBeginY = -dBeginY + m_WaferRecipe.ORIGIN_Y;

        //                this.OriginIndexX = (int)((-dBeginX + m_WaferRecipe.ORIGIN_X) / dDiePitchX) + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                this.OriginIndexY = (int)((-dBeginY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY) + m_WaferRecipe.DIE_INDEX_MIN_Y;

        //                for (int idx = 0; idx < XDies; idx++)
        //                {
        //                    dX = idx * dDiePitchX + dBeginX;

        //                    for (int idy = 0; idy < YDies; idy++)
        //                    {
        //                        dY = idy * dDiePitchY + dBeginY;

        //                        if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, dDiePitchX, dDiePitchY))
        //                        {
        //                            oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                            oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
        //                            oDie.BinNumber = iBin;
        //                            oDie.DieCood.X = dX;
        //                            oDie.DieCood.Y = dY;
        //                            oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
        //                            oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
        //                            oDie.DieProp = 1;
        //                            oDie.ZoneNumber = 0;
        //                            m_arrDies.Add(oDie);
        //                            this.m_WaferRecipe.NETDIE++;
        //                        }
        //                        else
        //                        {
        //                            oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                            oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
        //                            oDie.BinNumber = iBin;
        //                            oDie.DieCood.X = dX;
        //                            oDie.DieCood.Y = dY;
        //                            oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
        //                            oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
        //                            oDie.DieProp = -1;
        //                            oDie.ZoneNumber = 0;
        //                            m_arrDies.Add(oDie);
        //                        }
        //                        m_DieIndexer.Add(new Point(oDie.IndexX, oDie.IndexY));
        //                    }
        //                }
        //                #endregion
        //                break;
        //            case XYDirection.LeftTop:
        //                #region LEFT-TOP
        //                dBeginX = -dBeginX + m_WaferRecipe.ORIGIN_X;
        //                dBeginY = dBeginY + m_WaferRecipe.ORIGIN_Y;

        //                this.OriginIndexX = (int)((-dBeginX + m_WaferRecipe.ORIGIN_X) / dDiePitchX) + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                this.OriginIndexY = (int)((dBeginY - dDiePitchY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY) + m_WaferRecipe.DIE_INDEX_MIN_Y;

        //                for (int idx = 0; idx < XDies; idx++)
        //                {
        //                    dX = idx * dDiePitchX + dBeginX;

        //                    for (int idy = 0; idy < YDies; idy++)
        //                    {
        //                        dY = -((idy + 1) * dDiePitchY) + dBeginY;

        //                        if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, dDiePitchX, dDiePitchY))
        //                        {
        //                            oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                            oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
        //                            oDie.BinNumber = iBin;
        //                            oDie.DieCood.X = dX;
        //                            oDie.DieCood.Y = dY;
        //                            oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
        //                            oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
        //                            oDie.DieProp = 1;
        //                            oDie.ZoneNumber = 0;
        //                            m_arrDies.Add(oDie);
        //                            this.m_WaferRecipe.NETDIE++;
        //                        }
        //                        else
        //                        {
        //                            oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                            oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
        //                            oDie.BinNumber = iBin;
        //                            oDie.DieCood.X = dX;
        //                            oDie.DieCood.Y = dY;
        //                            oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
        //                            oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
        //                            oDie.DieProp = -1;
        //                            oDie.ZoneNumber = 0;
        //                            m_arrDies.Add(oDie);
        //                        }
        //                        m_DieIndexer.Add(new Point(oDie.IndexX, oDie.IndexY));
        //                    }
        //                }
        //                #endregion
        //                break;
        //            case XYDirection.RightBottom:
        //                #region RIGHT-BOTTOM
        //                dBeginX = dBeginX + m_WaferRecipe.ORIGIN_X;
        //                dBeginY = -dBeginY + m_WaferRecipe.ORIGIN_Y;

        //                this.OriginIndexX = (int)((dBeginX - dDiePitchX + m_WaferRecipe.ORIGIN_X) / dDiePitchX) + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                this.OriginIndexY = (int)((-dBeginY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY) + m_WaferRecipe.DIE_INDEX_MIN_Y;

        //                for (int idx = 0; idx < XDies; idx++)
        //                {
        //                    dX = -((idx + 1) * dDiePitchX) + dBeginX;
        //                    for (int idy = 0; idy < YDies; idy++)
        //                    {
        //                        dY = idy * dDiePitchY + dBeginY;
        //                        if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, dDiePitchX, dDiePitchY))
        //                        {
        //                            oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                            oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
        //                            oDie.BinNumber = iBin;
        //                            oDie.DieCood.X = dX;
        //                            oDie.DieCood.Y = dY;
        //                            oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
        //                            oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
        //                            oDie.DieProp = 1;
        //                            oDie.ZoneNumber = 0;
        //                            m_arrDies.Add(oDie);
        //                            this.m_WaferRecipe.NETDIE++;
        //                        }
        //                        else
        //                        {
        //                            oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                            oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
        //                            oDie.BinNumber = iBin;
        //                            oDie.DieCood.X = dX;
        //                            oDie.DieCood.Y = dY;
        //                            oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
        //                            oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
        //                            oDie.DieProp = -1;
        //                            oDie.ZoneNumber = 0;
        //                            m_arrDies.Add(oDie);
        //                        }
        //                        m_DieIndexer.Add(new Point(oDie.IndexX, oDie.IndexY));
        //                    }
        //                }
        //                #endregion
        //                break;
        //            case XYDirection.RightTop:
        //                #region RIGHT-TOP
        //                dBeginX = dBeginX + m_WaferRecipe.ORIGIN_X;
        //                dBeginY = dBeginY + m_WaferRecipe.ORIGIN_Y;

        //                this.OriginIndexX = (int)((dBeginX - dDiePitchX + m_WaferRecipe.ORIGIN_X) / dDiePitchX) + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                this.OriginIndexY = (int)((dBeginY - dDiePitchY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY) + m_WaferRecipe.DIE_INDEX_MIN_Y;

        //                for (int idx = 0; idx < XDies; idx++)
        //                {
        //                    dX = -((idx + 1) * dDiePitchX) + dBeginX;

        //                    for (int idy = 0; idy < YDies; idy++)
        //                    {
        //                        dY = -((idy + 1) * dDiePitchY) + dBeginY;
        //                        if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, dDiePitchX, dDiePitchY))
        //                        {
        //                            oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                            oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
        //                            oDie.BinNumber = iBin;
        //                            oDie.DieCood.X = dX;
        //                            oDie.DieCood.Y = dY;
        //                            oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
        //                            oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
        //                            oDie.DieProp = 1;
        //                            oDie.ZoneNumber = 0;
        //                            m_arrDies.Add(oDie);
        //                            this.m_WaferRecipe.NETDIE++;
        //                        }
        //                        else
        //                        {
        //                            oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
        //                            oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
        //                            oDie.BinNumber = iBin;
        //                            oDie.DieCood.X = dX;
        //                            oDie.DieCood.Y = dY;
        //                            oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
        //                            oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
        //                            oDie.DieProp = -1;
        //                            oDie.ZoneNumber = 0;
        //                            m_arrDies.Add(oDie);
        //                        }
        //                        m_DieIndexer.Add(new Point(oDie.IndexX, oDie.IndexY));
        //                    }
        //                }
        //                #endregion
        //                break;
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        #endregion

        #region ■ 주어진 Real Size 조건으로 Wafer안에 들어갈 Die들을 구하는 함수 [ DieCalculation(bool isReal) / UseDie(double X,double Y,double Width,double Height) ]
        public virtual void DieCalculation()
        {
            double edgeHight = 150 - Math.Cos((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);

            double dDiePitchX = (m_WaferRecipe.WAFER_SIZE * m_dMargin) / m_WaferRecipe.XDIES;
            double dDiePitchY = ((m_WaferRecipe.WAFER_SIZE - edgeHight) * m_dMargin) / m_WaferRecipe.YDIES;
            double dBeginX = (m_WaferRecipe.WAFER_SIZE * m_dMargin) / 2d;
            double dBeginY = (m_WaferRecipe.WAFER_SIZE * m_dMargin) / 2d;

            //double dBeginX = (m_WaferRecipe.WAFER_SIZE) / 2d;
            //double dBeginY = (m_WaferRecipe.WAFER_SIZE) / 2d;

            m_WaferRecipe.DIE_SIZE_X = dDiePitchX;
            m_WaferRecipe.DIE_SIZE_Y = dDiePitchY;
            this.m_WaferRecipe.NETDIE = 0;

            if ((m_WaferRecipe.XDIES % 2) == 1)
            {
                m_WaferRecipe.ORIGIN_X = dDiePitchX / 2;
            }
            else
            {
                m_WaferRecipe.ORIGIN_X = 0;
            }

            if ((m_WaferRecipe.YDIES % 2) == 1)
            {
                m_WaferRecipe.ORIGIN_Y = dDiePitchY / 2;
            }
            else
            {
                m_WaferRecipe.ORIGIN_Y = 0;
            }


            DieClear();
            Die oDie = new Die(0, 0, 0, 0, 0, 0, 0);
            try
            {
                switch (XYDirect)
                {
                    case XYDirection.LeftBottom:
                        #region LEFT-BOTTOM
                        dBeginX = -dBeginX + m_WaferRecipe.ORIGIN_X;
                        dBeginY = -dBeginY + m_WaferRecipe.ORIGIN_Y;

                        this.OriginIndexX = (int)Math.Round((-dBeginX + m_WaferRecipe.ORIGIN_X) / dDiePitchX, 2) + m_WaferRecipe.DIE_INDEX_MIN_X;
                        this.OriginIndexY = (int)Math.Round((-dBeginY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY, 2) + m_WaferRecipe.DIE_INDEX_MIN_Y;
                        #endregion
                        break;
                    case XYDirection.LeftTop:
                        #region LEFT-TOP
                        dBeginX = -dBeginX + m_WaferRecipe.ORIGIN_X;
                        dBeginY = dBeginY + m_WaferRecipe.ORIGIN_Y;

                        this.OriginIndexX = (int)Math.Round((-dBeginX + m_WaferRecipe.ORIGIN_X) / dDiePitchX, 2) + m_WaferRecipe.DIE_INDEX_MIN_X;
                        this.OriginIndexY = (int)Math.Round((dBeginY - dDiePitchY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY, 2) + m_WaferRecipe.DIE_INDEX_MIN_Y;

                        #endregion
                        break;
                    case XYDirection.RightBottom:
                        #region RIGHT-BOTTOM
                        dBeginX = dBeginX + m_WaferRecipe.ORIGIN_X;
                        dBeginY = -dBeginY + m_WaferRecipe.ORIGIN_Y;

                        this.OriginIndexX = (int)Math.Round((dBeginX - dDiePitchX + m_WaferRecipe.ORIGIN_X) / dDiePitchX, 2) + m_WaferRecipe.DIE_INDEX_MIN_X;
                        this.OriginIndexY = (int)Math.Round((-dBeginY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY, 2) + m_WaferRecipe.DIE_INDEX_MIN_Y;

                        #endregion
                        break;
                    case XYDirection.RightTop:
                        #region RIGHT-TOP
                        dBeginX = dBeginX + m_WaferRecipe.ORIGIN_X;
                        dBeginY = dBeginY + m_WaferRecipe.ORIGIN_Y;

                        this.OriginIndexX = (int)Math.Round((dBeginX - dDiePitchX + m_WaferRecipe.ORIGIN_X) / dDiePitchX, 2) + m_WaferRecipe.DIE_INDEX_MIN_X;
                        this.OriginIndexY = (int)Math.Round((dBeginY - dDiePitchY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY, 2) + m_WaferRecipe.DIE_INDEX_MIN_Y;

                        #endregion
                        break;
                }
            }
            catch//(Exception ex)
            {
                //Console.WriteLine(string.Format("DieCalculation():{0}",ex.Message));
            }
        }
        #endregion

        #region ■ 주어진 Real Size 조건으로 Wafer안에 들어갈 Die들을 구하는 함수 [ DieCalculation(bool isReal) / UseDie(double X,double Y,double Width,double Height) ]
        public virtual void DieCalculation(bool isReal)
        {
            if (!isReal)
            {
                this.DieCalculation();
            }

            double dDiePitchX = m_WaferRecipe.DIE_SIZE_X;
            double dDiePitchY = m_WaferRecipe.DIE_SIZE_Y;

            double dBeginX = 0;
            double dBeginY = 0;

            double dX = 0.0d;
            double dY = 0.0d;

            this.m_WaferRecipe.NETDIE = 0;


            int iBin = 1;
            DieClear();
            Die oDie = new Die(0, 0, 0, 0, 0, 0, 0);
            try
            {
                switch (XYDirect)
                {
                    case XYDirection.LeftTop:
                        #region LEFT-TOP
                        dBeginX = -m_WaferRecipe.DIE_SIZE_X * (m_WaferRecipe.ORIGIN_DIE_X - m_WaferRecipe.DIE_INDEX_MIN_X + 1);
                        dBeginY = m_WaferRecipe.DIE_SIZE_Y * (m_WaferRecipe.ORIGIN_DIE_Y - m_WaferRecipe.DIE_INDEX_MIN_Y + 1);

                        for (int idx = 0; idx < XDies; idx++)
                        {
                            dX = idx * dDiePitchX + dBeginX;

                            for (int idy = 0; idy < YDies; idy++)
                            {
                                dY = -((idy + 1) * dDiePitchY) + dBeginY;

                                if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, dDiePitchX, dDiePitchY))
                                {
                                    oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
                                    oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
                                    oDie.BinNumber = iBin;
                                    oDie.DieCood.X = dX;
                                    oDie.DieCood.Y = dY;
                                    oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
                                    oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
                                    oDie.DieProp = 1;
                                    oDie.ZoneNumber = 0;
                                    m_arrDies.Add(oDie);
                                    this.m_WaferRecipe.NETDIE++;
                                }
                                else
                                {
                                    oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
                                    oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
                                    oDie.BinNumber = iBin;
                                    oDie.DieCood.X = dX;
                                    oDie.DieCood.Y = dY;
                                    oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
                                    oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
                                    oDie.DieProp = -1;
                                    oDie.ZoneNumber = 0;
                                    m_arrDies.Add(oDie);
                                }
                                m_DieIndexer.Add(new Point(oDie.IndexX, oDie.IndexY));
                            }
                        }
                        #endregion
                        break;
                    case XYDirection.RightTop:
                        #region RIGHT-TOP
                        dBeginX = m_WaferRecipe.DIE_SIZE_X * (m_WaferRecipe.ORIGIN_DIE_X - m_WaferRecipe.DIE_INDEX_MIN_X + 1);
                        dBeginY = m_WaferRecipe.DIE_SIZE_Y * (m_WaferRecipe.ORIGIN_DIE_Y - m_WaferRecipe.DIE_INDEX_MIN_Y + 1);

                        //this.OriginIndexX = (int)((dBeginX - dDiePitchX + m_WaferRecipe.ORIGIN_X) / dDiePitchX) + m_WaferRecipe.DIE_INDEX_MIN_X;
                        //this.OriginIndexY = (int)((dBeginY - dDiePitchY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY) + m_WaferRecipe.DIE_INDEX_MIN_Y;

                        for (int idx = 0; idx < XDies; idx++)
                        {
                            dX = -((idx + 1) * dDiePitchX) + dBeginX;

                            for (int idy = 0; idy < YDies; idy++)
                            {
                                dY = -((idy + 1) * dDiePitchY) + dBeginY;
                                if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, dDiePitchX, dDiePitchY))
                                {
                                    oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
                                    oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
                                    oDie.BinNumber = iBin;
                                    oDie.DieCood.X = dX;
                                    oDie.DieCood.Y = dY;
                                    oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
                                    oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
                                    oDie.DieProp = 1;
                                    oDie.ZoneNumber = 0;
                                    m_arrDies.Add(oDie);
                                    this.m_WaferRecipe.NETDIE++;
                                }
                                else
                                {
                                    oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
                                    oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
                                    oDie.BinNumber = iBin;
                                    oDie.DieCood.X = dX;
                                    oDie.DieCood.Y = dY;
                                    oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
                                    oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
                                    oDie.DieProp = -1;
                                    oDie.ZoneNumber = 0;
                                    m_arrDies.Add(oDie);
                                }
                                m_DieIndexer.Add(new Point(oDie.IndexX, oDie.IndexY));
                            }
                        }
                        #endregion
                        break;
                    case XYDirection.LeftBottom:
                        #region LEFT-BOTTOM
                        dBeginX = -m_WaferRecipe.DIE_SIZE_X * (m_WaferRecipe.ORIGIN_DIE_X - m_WaferRecipe.DIE_INDEX_MIN_X + 1);
                        dBeginY = -m_WaferRecipe.DIE_SIZE_Y * (m_WaferRecipe.ORIGIN_DIE_Y - m_WaferRecipe.DIE_INDEX_MIN_Y + 1);

                        //dBeginX = -dBeginX + m_WaferRecipe.ORIGIN_X;
                        //dBeginY = -dBeginY + m_WaferRecipe.ORIGIN_Y;

                        //this.OriginIndexX = (int)((-dBeginX + m_WaferRecipe.ORIGIN_X) / dDiePitchX) + m_WaferRecipe.DIE_INDEX_MIN_X;
                        //this.OriginIndexY = (int)((-dBeginY + m_WaferRecipe.ORIGIN_Y) / dDiePitchY) + m_WaferRecipe.DIE_INDEX_MIN_Y;

                        for (int idx = 0; idx < XDies; idx++)
                        {
                            dX = idx * dDiePitchX + dBeginX;

                            for (int idy = 0; idy < YDies; idy++)
                            {
                                dY = idy * dDiePitchY + dBeginY;

                                if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, dDiePitchX, dDiePitchY))
                                {
                                    oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
                                    oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
                                    oDie.BinNumber = iBin;
                                    oDie.DieCood.X = dX;
                                    oDie.DieCood.Y = dY;
                                    oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
                                    oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
                                    oDie.DieProp = 1;
                                    oDie.ZoneNumber = 0;
                                    m_arrDies.Add(oDie);
                                    this.m_WaferRecipe.NETDIE++;
                                }
                                else
                                {
                                    oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
                                    oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
                                    oDie.BinNumber = iBin;
                                    oDie.DieCood.X = dX;
                                    oDie.DieCood.Y = dY;
                                    oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
                                    oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
                                    oDie.DieProp = -1;
                                    oDie.ZoneNumber = 0;
                                    m_arrDies.Add(oDie);
                                }
                                m_DieIndexer.Add(new Point(oDie.IndexX, oDie.IndexY));
                            }
                        }
                        #endregion
                        break;
                    case XYDirection.RightBottom:
                        #region RIGHT-BOTTOM
                        dBeginX = m_WaferRecipe.DIE_SIZE_X * (m_WaferRecipe.ORIGIN_DIE_X - m_WaferRecipe.DIE_INDEX_MIN_X + 1);
                        dBeginY = -m_WaferRecipe.DIE_SIZE_Y * (m_WaferRecipe.ORIGIN_DIE_Y - m_WaferRecipe.DIE_INDEX_MIN_Y + 1);

                        for (int idx = 0; idx < XDies; idx++)
                        {
                            dX = -((idx + 1) * dDiePitchX) + dBeginX;
                            for (int idy = 0; idy < YDies; idy++)
                            {
                                dY = idy * dDiePitchY + dBeginY;
                                if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, dDiePitchX, dDiePitchY))
                                {
                                    oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
                                    oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
                                    oDie.BinNumber = iBin;
                                    oDie.DieCood.X = dX;
                                    oDie.DieCood.Y = dY;
                                    oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
                                    oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
                                    oDie.DieProp = 1;
                                    oDie.ZoneNumber = 0;
                                    m_arrDies.Add(oDie);
                                    this.m_WaferRecipe.NETDIE++;
                                }
                                else
                                {
                                    oDie.IndexX = idx + m_WaferRecipe.DIE_INDEX_MIN_X;
                                    oDie.IndexY = idy + m_WaferRecipe.DIE_INDEX_MIN_Y;
                                    oDie.BinNumber = iBin;
                                    oDie.DieCood.X = dX;
                                    oDie.DieCood.Y = dY;
                                    oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
                                    oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
                                    oDie.DieProp = -1;
                                    oDie.ZoneNumber = 0;
                                    m_arrDies.Add(oDie);
                                }
                                m_DieIndexer.Add(new Point(oDie.IndexX, oDie.IndexY));
                            }
                        }
                        #endregion
                        break;

                }
            }
            catch//(Exception ex)
            {
                //Console.WriteLine(string.Format("DieCalculation(bool isReal):{0}", ex.Message));
            }
        }

        protected bool UseDie(double X, double Y, double Width, double Height)
        {
            int iRealAngle = (m_WaferRecipe.ANGLE + m_iAngleOffset) % 360;
            //			X = X + m_WaferRecipe.ORIGIN_X;
            //			Y = Y - m_WaferRecipe.ORIGIN_Y;
            switch (iRealAngle)
            {
                case 0:
                    if (m_rectNotchArea.Top > Y || m_rectNotchArea.Top > Y + Height) return false;
                    break;
                case 90:
                    if (m_rectNotchArea.Right > X || m_rectNotchArea.Right > X + Width) return false;
                    break;
                case 180:
                    if (m_rectNotchArea.Bottom < Y || m_rectNotchArea.Bottom < Y + Height) return false;
                    break;
                case 270:
                    if (m_rectNotchArea.Left < X || m_rectNotchArea.Left < X + Width) return false;
                    break;
            }

            if ((m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.EDGE_SIZE) < Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y + Height, 2))) return false;
            if ((m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.EDGE_SIZE) < Math.Sqrt(Math.Pow(X + Width, 2) + Math.Pow(Y + Height, 2))) return false;
            if ((m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.EDGE_SIZE) < Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2))) return false;
            if ((m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.EDGE_SIZE) < Math.Sqrt(Math.Pow(X + Width, 2) + Math.Pow(Y, 2))) return false;

            return true;
        }

        #endregion

        #region ■ Die Array Handling 함수 ( DieClear / AddDie(Die NewDie) )
        public virtual void DieClear()
        {
            m_arrDies.Clear();
            m_DieIndexer.Clear();
            m_arrModifyDies.Clear();
            m_DieModifyIndexer.Clear();
            m_ItemCountColorList.Clear();

            m_WaferRecipe.DIE_INDEX_MIN_X = m_WaferRecipe.DIE_INDEX_MIN_Y = 0;
            m_WaferRecipe.DIE_INDEX_MAX_X = m_WaferRecipe.DIE_INDEX_MAX_Y = 0;
        }

        public void AddDie(Die NewDie)
        {
            //int currIndex = m_arrDies.IndexOf(NewDie.IndexX, NewDie.IndexY);

            //// 같은 인덱스에 Die 가 존재하는 경우
            //if (currIndex >= 0)
            //{
            //    Die die = m_arrDies[currIndex];
            //    die.BinNumber = NewDie.BinNumber;
            //    die.DieProp = NewDie.DieProp;
            //    m_arrDies.UpdateDie(currIndex, die);
            //    return;
            //}

            double dDiePitchX = m_WaferRecipe.DIE_SIZE_X;
            double dDiePitchY = m_WaferRecipe.DIE_SIZE_Y;

            double dBeginX = 0;
            double dBeginY = 0;

            double dX = 0.0d;
            double dY = 0.0d;
            try
            {
                if (NewDie.IndexX < m_WaferRecipe.DIE_INDEX_MIN_X) m_WaferRecipe.DIE_INDEX_MIN_X = NewDie.IndexX;
                if (NewDie.IndexY < m_WaferRecipe.DIE_INDEX_MIN_Y) m_WaferRecipe.DIE_INDEX_MIN_Y = NewDie.IndexY;
                if (NewDie.IndexX > m_WaferRecipe.DIE_INDEX_MAX_X) m_WaferRecipe.DIE_INDEX_MAX_X = NewDie.IndexX;
                if (NewDie.IndexY > m_WaferRecipe.DIE_INDEX_MAX_Y) m_WaferRecipe.DIE_INDEX_MAX_Y = NewDie.IndexY;

                switch (XYDirect)
                {
                    case XYDirection.LeftTop:
                        #region LEFT-TOP
                        dBeginX = -m_WaferRecipe.DIE_SIZE_X * (m_WaferRecipe.ORIGIN_DIE_X - m_WaferRecipe.DIE_INDEX_MIN_X + 1);
                        dBeginY = m_WaferRecipe.DIE_SIZE_Y * (m_WaferRecipe.ORIGIN_DIE_Y - m_WaferRecipe.DIE_INDEX_MIN_Y + 1);

                        dX = (NewDie.IndexX - m_WaferRecipe.DIE_INDEX_MIN_X) * dDiePitchX + dBeginX;
                        dY = -(((NewDie.IndexY - m_WaferRecipe.DIE_INDEX_MIN_Y) + 1) * dDiePitchY) + dBeginY;
                        #endregion
                        break;
                    case XYDirection.RightTop:
                        #region RIGHT-TOP
                        dBeginX = m_WaferRecipe.DIE_SIZE_X * (m_WaferRecipe.ORIGIN_DIE_X - m_WaferRecipe.DIE_INDEX_MIN_X + 1);
                        dBeginY = m_WaferRecipe.DIE_SIZE_Y * (m_WaferRecipe.ORIGIN_DIE_Y - m_WaferRecipe.DIE_INDEX_MIN_Y + 1);

                        dX = -(((NewDie.IndexX - m_WaferRecipe.DIE_INDEX_MIN_X) + 1) * dDiePitchX) + dBeginX;
                        dY = -(((NewDie.IndexY - m_WaferRecipe.DIE_INDEX_MIN_Y) + 1) * dDiePitchY) + dBeginY;
                        #endregion
                        break;
                    case XYDirection.LeftBottom:
                        #region LEFT-BOTTOM
                        dBeginX = -m_WaferRecipe.DIE_SIZE_X * (m_WaferRecipe.ORIGIN_DIE_X - m_WaferRecipe.DIE_INDEX_MIN_X);
                        dBeginY = -m_WaferRecipe.DIE_SIZE_Y * (m_WaferRecipe.ORIGIN_DIE_Y - m_WaferRecipe.DIE_INDEX_MIN_Y);

                        dX = (NewDie.IndexX - m_WaferRecipe.DIE_INDEX_MIN_X) * dDiePitchX + dBeginX;
                        dY = (NewDie.IndexY - m_WaferRecipe.DIE_INDEX_MIN_Y) * dDiePitchY + dBeginY;
                        #endregion
                        break;

                    case XYDirection.RightBottom:
                        #region RIGHT-BOTTOM
                        dBeginX = m_WaferRecipe.DIE_SIZE_X * (m_WaferRecipe.ORIGIN_DIE_X - m_WaferRecipe.DIE_INDEX_MIN_X + 1);
                        dBeginY = -m_WaferRecipe.DIE_SIZE_Y * (m_WaferRecipe.ORIGIN_DIE_Y - m_WaferRecipe.DIE_INDEX_MIN_Y);

                        dX = -(((NewDie.IndexX - m_WaferRecipe.DIE_INDEX_MIN_X) + 1) * dDiePitchX) + dBeginX;
                        dY = (NewDie.IndexY - m_WaferRecipe.DIE_INDEX_MIN_Y) * dDiePitchY + dBeginY;
                        #endregion
                        break;

                }

                NewDie.DieCood.X = dX;
                NewDie.DieCood.Y = dY;
                NewDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
                NewDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
                m_arrDies.Add(NewDie);
                if (NewDie.DieProp == 1) this.m_WaferRecipe.NETDIE++;
                m_DieIndexer.Add(new Point(NewDie.IndexX, NewDie.IndexY));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void AddDie(int X, int Y, int BIN)
        {
            Die NewDie = new Die(X, Y, BIN, 0);
            NewDie.DieCood.X = (NewDie.IndexX - m_WaferRecipe.ORIGIN_DIE_X) * m_WaferRecipe.DIE_SIZE_X;
            NewDie.DieCood.Y = (NewDie.IndexY - m_WaferRecipe.ORIGIN_DIE_Y) * m_WaferRecipe.DIE_SIZE_Y;
            NewDie.DieCood.Width = m_WaferRecipe.DIE_SIZE_X;
            NewDie.DieCood.Height = m_WaferRecipe.DIE_SIZE_Y;

            NewDie.DieProp = DieProperty(NewDie.DieCood.X, NewDie.DieCood.Y, NewDie.DieCood.Width, NewDie.DieCood.Height);
            if (NewDie.DieProp == -1) return;

            if (m_arrDies.Count == 0)
            {
                m_WaferRecipe.DIE_INDEX_MAX_X = m_WaferRecipe.DIE_INDEX_MIN_X = NewDie.IndexX;
                m_WaferRecipe.DIE_INDEX_MAX_Y = m_WaferRecipe.DIE_INDEX_MIN_Y = NewDie.IndexY;

                REL_INDEX_MAX_X = REL_INDEX_MIN_X = NewDie.IndexX;
                REL_INDEX_MAX_Y = REL_INDEX_MIN_Y = NewDie.IndexY;
            }
            else
            {
                if (NewDie.IndexX < m_WaferRecipe.DIE_INDEX_MIN_X) m_WaferRecipe.DIE_INDEX_MIN_X = NewDie.IndexX;
                if (NewDie.IndexY < m_WaferRecipe.DIE_INDEX_MIN_Y) m_WaferRecipe.DIE_INDEX_MIN_Y = NewDie.IndexY;
                if (NewDie.IndexX > m_WaferRecipe.DIE_INDEX_MAX_X) m_WaferRecipe.DIE_INDEX_MAX_X = NewDie.IndexX;
                if (NewDie.IndexY > m_WaferRecipe.DIE_INDEX_MAX_Y) m_WaferRecipe.DIE_INDEX_MAX_Y = NewDie.IndexY;
                if (NewDie.DieProp == 1)
                {
                    if (NewDie.IndexX < REL_INDEX_MIN_X) REL_INDEX_MIN_X = NewDie.IndexX;
                    if (NewDie.IndexY < REL_INDEX_MIN_Y) REL_INDEX_MIN_Y = NewDie.IndexY;
                    if (NewDie.IndexX > REL_INDEX_MAX_X) REL_INDEX_MAX_X = NewDie.IndexX;
                    if (NewDie.IndexY > REL_INDEX_MAX_Y) REL_INDEX_MAX_Y = NewDie.IndexY;
                }
            }

            m_arrDies.Add(NewDie);
            m_DieIndexer.Add(new Point(X, Y));
        }

        #endregion

        #region ▣ Redraw / Paint / Resize Event처리 함수
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            WaferMap_Paint(this, e);
        }

        public void ForceLoadEvent()
        {
            using (Bitmap bmp = new Bitmap(Width, Height))
            {
                base.DrawToBitmap(bmp, ClientRectangle);
            }
        }

        protected virtual void WaferMap_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            try
            {
                if (m_gdiTempMap == null) return;
                m_gdiMain.DrawImageUnscaled(m_bmpWaferMap, 0, 0);
                DrawCenteGrid();

                if (e != null)
                    e.Graphics.DrawImageUnscaled(m_bmpWaferMap, 0, 0);
            }
            catch//(Exception ex)
            {
                //Console.WriteLine(string.Format("WaferMap_Paint(object sender, System.Windows.Forms.PaintEventArgs e):{0}", ex.Message));
            }
        }


        protected override void OnResize(System.EventArgs e)
        {
            if (this.Width * this.Height == 0)
            {
                return;
            }

            try
            {
                if (m_eoMapMode == MapMode.Free)
                {
                    RectangleD rectdBefore = m_rectdWaferArea;
                    PointD pfBeforeCenter = new PointD(m_rectdWaferArea.X + m_rectdWaferArea.Width / 2, m_rectdWaferArea.Y + m_rectdWaferArea.Height / 2);

                    m_rectdWaferArea.Width = this.Width / m_dZoomRatio_X_m_dScale;
                    m_rectdWaferArea.Height = this.Height / m_dZoomRatio_X_m_dScale;

                    m_rectdWaferArea.X = pfBeforeCenter.X - m_rectdWaferArea.Width / 2;
                    m_rectdWaferArea.Y = pfBeforeCenter.Y - m_rectdWaferArea.Height / 2;
                }
                else if (m_eoMapMode == MapMode.Fit)
                {
                    float dMinCanvers = Math.Min(this.Width, this.Height);
                    m_dZoomRatio = dMinCanvers / m_WaferRecipe.WAFER_SIZE;
                    m_dZoomRatio_X_m_dScale = m_dZoomRatio * m_dScale;
                    //m_rectdWaferArea = new RectangleD(((dMinCanvers - this.Width) / 2.0f) / m_dZoomRatio_X_m_dScale, ((dMinCanvers - this.Height) / 2.0f) / m_dZoomRatio_X_m_dScale, m_WaferRecipe.WAFER_SIZE, m_WaferRecipe.WAFER_SIZE);
                    m_rectdWaferArea = new RectangleD(((dMinCanvers - this.Width) / 2.0f) / m_dZoomRatio_X_m_dScale
                                                    , ((dMinCanvers - this.Height) / 2.0f) / m_dZoomRatio_X_m_dScale
                                                    , this.Width / m_dZoomRatio_X_m_dScale
                                                    , this.Height / m_dZoomRatio_X_m_dScale);
                }

                if (m_bmpWaferMap != null) m_bmpWaferMap.Dispose();
                if (m_gdiTempMap != null) m_gdiTempMap.Dispose();

                m_gdiMain = this.CreateGraphics();
                m_bmpWaferMap = new Bitmap(this.Width, this.Height);
                m_bmpTemp = new Bitmap(m_bmpWaferMap);
                m_gdiTempMap = Graphics.FromImage(m_bmpWaferMap);
                Redraw();
            }
            catch//(Exception ex)
            {
                //Console.WriteLine(string.Format("WaferMap_Resize(object sender, System.EventArgs e):{0}", ex.Message));
            }
            finally
            {
                GC.Collect();
            }
        }


        public virtual void Redraw()
        {
            DrawWafer();
            WaferMap_Paint(null, null);
        }

        #endregion

        #region ■ Information Drawing 함수

        public void AddInfomation(string strInfo)
        {
            if (m_strInfomation == null) m_strInfomation = new List<string>();
            else
            {
                if (m_strInfomation.IndexOf(strInfo) > -1) return;
            }

            m_strInfomation.Add(strInfo);
        }

        public void SetInfomation(string[] strInfos)
        {
            if (m_strInfomation == null)
                m_strInfomation = new List<string>();

            m_strInfomation.Clear();

            if (strInfos == null || strInfos.Length == 0)
                return;

            for (int i = 0; i < strInfos.Length; i++)
                m_strInfomation.Add(strInfos[i]);
        }

        #endregion

        #region ■ Die Drawing
        protected virtual void DrawDies(Graphics g)
        {
            if (m_arrDies.Count == 0) return;
            m_WaferRecipe.NETDIE = 0;

            PointF[] pfFirst = null;
            PointF[] pfOrigin = null;

            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];

            /// Pen / Brush는 Dispose해야 하므로 Try밖에 선언하고 Finally에서 Dispose를 ...
            /// 메모리 확인해 본결과 효과 만점... <--jiral
            Pen pSelDieBorder = null;
            Pen pDieBorder = null;
            Pen pOriginDieBorder = null;
            Pen pFirstDieBorder = null;

            SolidBrush sbrshDie = null;

            bool bOnWafer = false;
            try
            {
                pSelDieBorder = new Pen(Color.Red, 1);
                pDieBorder = new Pen(m_colDieBorder);
                pOriginDieBorder = new Pen(m_colOriginDieBorder);
                pFirstDieBorder = new Pen(m_colFirstDieBorder);
                sbrshDie = new SolidBrush(Color.White);

                //foreach(Die InDie in m_arrDies)

                for (int nDieIdx = 0; nDieIdx < m_arrDies.Count; nDieIdx++)
                {
                    Die InDie = m_arrDies[nDieIdx];

                    //주석_DIE.COLOR
                    //if(InDie.BinNumber>100) continue;
                    /// Die의 4 점의 좌표를 계산한다. ////////////////////////////////////////////////////////////////////////////////////////
                    pdPoint[0].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
                    pdPoint[0].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

                    pdPoint[1].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
                    pdPoint[1].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

                    pdPoint[2].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
                    pdPoint[2].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

                    pdPoint[3].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
                    pdPoint[3].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

                    RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                    pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[0].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[0].Y) * m_dZoomRatio_X_m_dScale);

                    pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[1].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[1].Y) * m_dZoomRatio_X_m_dScale);

                    pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[2].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[2].Y) * m_dZoomRatio_X_m_dScale);

                    pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[3].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[3].Y) * m_dZoomRatio_X_m_dScale);
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    bOnWafer = this.UseDie(InDie.DieCood.X - m_WaferRecipe.ORIGIN_X
                        , InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y
                        , this.m_WaferRecipe.DIE_SIZE_X
                        , this.m_WaferRecipe.DIE_SIZE_Y);

                    /// 0 Skip / 1 Probing / 2 Mark Die를 설정값에 맞춰서 Drawing한다. 
                    switch (InDie.DieProp)
                    {
                        case 0:
                            if (m_bDrawSkipDie)
                            {
                                sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colSkipDieColor);
                            }
                            else continue;
                            break;
                        case 1:
                            if (m_bDrawGradationDie == true && double.IsNaN(m_dbGradationMinValue) == false && double.IsNaN(m_dbGradationMaxValue) == false && m_ParaLimit == false)
                            {
                                int nTempValue = 0;
                                if (double.IsNaN(InDie.ParametricValue) == true)
                                {
                                    sbrshDie.Color = Color.FromArgb(255, 255, 255);
                                    continue;
                                }
                                else if (InDie.ParametricValue <= m_dbGradationMinValue)
                                    nTempValue = 0;
                                else if (InDie.ParametricValue >= m_dbGradationMaxValue)
                                    nTempValue = m_nGradationInterval - 1;
                                else
                                    nTempValue = (int)((InDie.ParametricValue - m_dbGradationMinValue) / ((m_dbGradationMaxValue - m_dbGradationMinValue) / (double)m_nGradationInterval));

                                if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, nTempValue.ToString()) > -1)
                                {
                                    int nGapR = Math.Abs(m_colToGradationDieColor.R - m_colFromGradationDieColor.R);
                                    int nGapG = Math.Abs(m_colToGradationDieColor.G - m_colFromGradationDieColor.G);
                                    int nGapB = Math.Abs(m_colToGradationDieColor.B - m_colFromGradationDieColor.B);

                                    string[] sColorName = new string[3];
                                    if (nGapR >= nGapG)
                                    {
                                        if (nGapR >= nGapB)
                                        {
                                            sColorName[0] = "R";
                                            if (nGapG >= nGapB)
                                            {
                                                sColorName[1] = "G";
                                                sColorName[2] = "B";
                                            }
                                            else
                                            {
                                                sColorName[1] = "B";
                                                sColorName[2] = "G";
                                            }
                                        }
                                        else
                                        {
                                            sColorName[0] = "B";
                                            sColorName[1] = "R";
                                            sColorName[2] = "G";
                                        }
                                    }
                                    else
                                    {
                                        if (nGapG >= nGapB)
                                        {
                                            sColorName[0] = "G";
                                            if (nGapR >= nGapB)
                                            {
                                                sColorName[1] = "R";
                                                sColorName[2] = "B";
                                            }
                                            else
                                            {
                                                sColorName[1] = "B";
                                                sColorName[2] = "R";
                                            }
                                        }
                                        else
                                        {
                                            sColorName[0] = "B";
                                            sColorName[1] = "G";
                                            sColorName[2] = "R";
                                        }
                                    }

                                    int nColorStep = (nGapR + nGapG + nGapB) / m_nGradationInterval;

                                    int R = 0;
                                    int G = 0;
                                    int B = 0;

                                    int nOffsetColor = nColorStep * nTempValue;
                                    for (int i = 0; i < sColorName.Length; i++)
                                    {
                                        switch (sColorName[i])
                                        {
                                            case "R":
                                                if (nOffsetColor > nGapR)
                                                {
                                                    R = m_colToGradationDieColor.R;
                                                    nOffsetColor -= nGapR;
                                                }
                                                else
                                                {
                                                    if (m_colToGradationDieColor.R - m_colFromGradationDieColor.R >= 0)
                                                        R = m_colFromGradationDieColor.R + nOffsetColor;
                                                    else
                                                        R = m_colFromGradationDieColor.R - nOffsetColor;

                                                    nOffsetColor = 0;
                                                }
                                                break;
                                            case "G":
                                                if (nOffsetColor > nGapG)
                                                {
                                                    G = m_colToGradationDieColor.G;
                                                    nOffsetColor -= nGapG;
                                                }
                                                else
                                                {
                                                    if (m_colToGradationDieColor.G - m_colFromGradationDieColor.G >= 0)
                                                        G = m_colFromGradationDieColor.G + nOffsetColor;
                                                    else
                                                        G = m_colFromGradationDieColor.G - nOffsetColor;

                                                    nOffsetColor = 0;
                                                }
                                                break;
                                            case "B":
                                                if (nOffsetColor > nGapB)
                                                {
                                                    B = m_colToGradationDieColor.B;
                                                    nOffsetColor -= nGapB;
                                                }
                                                else
                                                {
                                                    if (m_colToGradationDieColor.B - m_colFromGradationDieColor.B >= 0)
                                                        B = m_colFromGradationDieColor.B + nOffsetColor;
                                                    else
                                                        B = m_colFromGradationDieColor.B - nOffsetColor;

                                                    nOffsetColor = 0;
                                                }
                                                break;
                                        }
                                    }

                                    sbrshDie.Color = Color.FromArgb(R, G, B);
                                }
                                else
                                {
                                    sbrshDie.Color = Color.FromArgb(m_iNotSelectedBinAlpha, m_ColorSet[InDie.BinNumber]);
                                    pDieBorder.Color = Color.FromArgb(0, 0, 0, 0);
                                }
                            }
                            else if (m_bDrawGradationDie == true && (double.IsNaN(m_dbGradationMinValue) == false || double.IsNaN(m_dbGradationMaxValue) == false) && m_ParaLimit == true)
                            {
                                //Gration Draw 시 Para Limit 에 대한 정보를 출력 하기 위해 추가함.
                                //Limit 에서 벗어나면 RED 아니면 WHITE

                                Color LimitSuccess = Color.White;
                                Color LimitFail = Color.Red;

                                if (double.IsNaN(InDie.ParametricValue) == true)
                                {
                                    sbrshDie.Color = LimitSuccess;
                                    continue;
                                }

                                sbrshDie.Color = LimitSuccess;

                                if (double.IsNaN(m_dbGradationMaxValue) == false && InDie.ParametricValue > m_dbGradationMaxValue)
                                {
                                    sbrshDie.Color = LimitFail;
                                }

                                if (double.IsNaN(m_dbGradationMinValue) == false && InDie.ParametricValue < m_dbGradationMinValue)
                                {
                                    sbrshDie.Color = LimitFail;
                                }

                            }
                            else if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, InDie.BinNumber.ToString()) > -1)
                            {
                                try
                                {
                                    int nColorNumber = -1;
                                    nColorNumber = InDie.BinNumber;

                                    if (nColorNumber < 0)
                                        sbrshDie.Color = Color.Black;
                                    else
                                        sbrshDie.Color = m_ColorSet[nColorNumber];

                                    pDieBorder.Color = m_colDieBorder;

                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            else
                            {
                                sbrshDie.Color = Color.FromArgb(m_iNotSelectedBinAlpha, m_ColorSet[InDie.BinNumber]);
                                pDieBorder.Color = Color.FromArgb(0, 0, 0, 0);
                            }

                            m_WaferRecipe.NETDIE++;
                            break;
                        case 2:
                            if (m_bDrawMarkDie)
                            {
                                sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colMarkDieColor);
                            }
                            else continue;
                            break;
                        case DIE_PROP_VIRTUAL_DIE:
                            sbrshDie.Color = VirtualDieColor;
                            break;
                    }

                    if (InDie.DieProp > -1)
                    {
                        m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                    }

                    if (/*m_dZoomRatio > 0.6 &&*/ m_bVisibleDieBorder)
                    {
                        if (m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) > -1)
                        {
                            g.DrawPolygon(pSelDieBorder, pfPoint);
                        }
                        else
                        {
                            g.DrawPolygon(pDieBorder, pfPoint);
                        }
                    }

                    //Die Line 을 특정 기준에 의해 그려 준다.
                    if (VisibleSignDies == true && m_SignDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) > -1)
                    {
                        g.DrawPolygon(m_bVisibleSignLine, pfPoint);
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////


                    /// Origin / First Border를 설정값에 맞춰서 Drawing한다.
                    if (InDie.IndexX == m_WaferRecipe.ORIGIN_DIE_X && InDie.IndexY == m_WaferRecipe.ORIGIN_DIE_Y)
                    {
                        pfOrigin = new PointF[4];
                        Array.Copy(pfPoint, pfOrigin, 4);
                    }

                    if (InDie.IndexX == m_WaferRecipe.FIRST_DIE_X && InDie.IndexY == m_WaferRecipe.FIRST_DIE_Y)
                    {
                        pfFirst = new PointF[4];
                        Array.Copy(pfPoint, pfFirst, 4);
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////
                    ///

                    if (m_bVisibleVIFaile && InDie.VIFail > 0)
                    {
                        float cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X) / 4;
                        float cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y) / 4;
                        float minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
                        float maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
                        float minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
                        float maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));

                        //	float cirR = Math.Min(Math.Abs(minX - maxX),Math.Abs(minY - maxY));
                        float cirR = Math.Min((float)(m_WaferRecipe.DIE_SIZE_X * m_dZoomRatio_X_m_dScale), (float)(m_WaferRecipe.DIE_SIZE_Y * m_dZoomRatio_X_m_dScale));
                        // 10% 원을 작게 그린다.
                        cirR = cirR - (cirR / 10);

                        g.FillEllipse(new SolidBrush(m_VIColorSet[InDie.VIFail]), cirX - cirR / 2, cirY - cirR / 2, cirR, cirR);
                        g.DrawEllipse(new Pen(m_colDieBorder), cirX - cirR / 2, cirY - cirR / 2, cirR, cirR);
                        //g.DrawEllipse(new Pen(Color.Red),cirX - cirR/2,cirY - cirR/2,cirR,cirR);
                    }

                    if (m_bVisibleVIFaile && InDie.AVIFailNumber > 0)
                    {
                        float cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X) / 4;
                        float cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y) / 4;
                        float minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
                        float maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
                        float minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
                        float maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));

                        //	float cirR = Math.Min(Math.Abs(minX - maxX),Math.Abs(minY - maxY));
                        float cirR = Math.Min((float)(m_WaferRecipe.DIE_SIZE_X * m_dZoomRatio_X_m_dScale), (float)(m_WaferRecipe.DIE_SIZE_Y * m_dZoomRatio_X_m_dScale));
                        // 10% 원을 작게 그린다.
                        cirR = cirR - (cirR / 10);

                        g.FillEllipse(new SolidBrush(m_ColorSet[InDie.VIFail]), cirX - cirR / 2, cirY - cirR / 2, cirR, cirR);
                        g.DrawEllipse(new Pen(m_colDieBorder), cirX - cirR / 2, cirY - cirR / 2, cirR, cirR);
                        //g.DrawEllipse(new Pen(Color.Red), cirX - cirR / 2, cirY - cirR / 2, cirR, cirR);

                    }

                    //if (m_bVisibleDieValue && m_dZoomRatio > 0.8)
                    if (m_bVisibleDieValue)
                    {
                        StringFormat format = new StringFormat();
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;

                        float minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
                        float minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
                        float maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
                        float maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));

                        RectangleF textRect = new RectangleF(minX, minY, maxX - minX, maxY - minY);

                        if (m_strDisplayDieValue == "BIN")
                        {
                            if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, InDie.BinNumber.ToString()) > -1)
                            {
                                g.DrawString(InDie.BinNumber.ToString(), m_fntWBin, new SolidBrush(Color.LightGray), textRect, format);
                                g.DrawString(InDie.BinNumber.ToString(), m_fntBin, new SolidBrush(Color.Black), textRect, format);
                            }
                        }
                        else if (m_strDisplayDieValue == "PCMVALUE")
                        {
                            if (double.IsNaN(InDie.ParametricValue) == true)
                                g.DrawString("F", m_fntPara, new SolidBrush(Util.GetIdealTextColor(sbrshDie.Color)), textRect, format);
                            else
                                g.DrawString(((double)InDie.ParametricValue).ToString(), m_fntPara, new SolidBrush(Util.GetIdealTextColor(sbrshDie.Color)), textRect, format);
                        }
                        else if (m_strDisplayDieValue == "SHOT")
                        {
                            g.DrawString(InDie.ShotID.ToString(), m_fntWBin, new SolidBrush(Color.LightGray), textRect, format);
                            g.DrawString(InDie.ShotID.ToString(), m_fntBin, new SolidBrush(Color.Black), textRect, format);

                            // 테스나 전용 - Shot 번호 표시시 1번 Dut를 Selection한다.
                            if (InDie.SiteNumber == 1)
                                g.DrawPolygon(pSelDieBorder, pfPoint);
                        }
                        else if (m_strDisplayDieValue == "SITE")
                        {
                            if (InDie.DiePassFail > 0)
                                sbrshDie.Color = m_ColorSet[InDie.SiteNumber];
                            else
                                sbrshDie.Color = Color.FromArgb(255, 255, 255);

                            if (InDie.DieProp > -1) m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                            //if (m_dZoomRatio > 0.6 && m_bVisibleDieBorder)
                            if (m_bVisibleDieBorder)
                            {
                                if (m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) > -1)
                                {
                                    g.DrawPolygon(pSelDieBorder, pfPoint);
                                }
                                else
                                {
                                    g.DrawPolygon(pDieBorder, pfPoint);
                                }
                            }

                            g.DrawString(InDie.SiteNumber.ToString(), m_fntWBin, new SolidBrush(Color.LightGray), textRect, format);
                            g.DrawString(InDie.SiteNumber.ToString(), m_fntBin, new SolidBrush(Color.Black), textRect, format);
                        }
                        else if (m_strDisplayDieValue == "ZONE")
                        {
                            g.DrawString(InDie.ZoneNumber.ToString(), m_fntWBin, new SolidBrush(Color.LightGray), textRect, format);
                            g.DrawString(InDie.ZoneNumber.ToString(), m_fntBin, new SolidBrush(Color.Black), textRect, format);
                        }
                        else if (m_strDisplayDieValue == "BLOCK")
                        {
                            g.DrawString(InDie.BlockArea.ToString(), m_fntWBin, new SolidBrush(Color.LightGray), textRect, format);
                            g.DrawString(InDie.BlockArea.ToString(), m_fntBin, new SolidBrush(Color.Black), textRect, format);
                        }
                        else if (m_strDisplayDieValue == "REPROB")
                        {
                            g.DrawString(InDie.ReProbing.ToString(), m_fntWBin, new SolidBrush(Color.LightGray), textRect, format);
                            g.DrawString(InDie.ReProbing.ToString(), m_fntBin, new SolidBrush(Color.Black), textRect, format);
                        }
                        //else if (m_strDisplayDieValue == "REPROBCHAR")
                        //{
                        //    g.DrawString(InDie.ReProbingChar, m_fntWBin, new SolidBrush(Color.LightGray), textRect, format);
                        //    g.DrawString(InDie.ReProbingChar, m_fntBin, new SolidBrush(Color.Black), textRect, format);
                        //}
                    }

                    if (m_bDrawMarkDie && InDie.DieProp == 2)
                    {
                        float cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X) / 4;
                        float cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y) / 4;
                        float minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
                        float maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
                        float minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
                        float maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));

                        //	float cirR = Math.Min(Math.Abs(minX - maxX),Math.Abs(minY - maxY));
                        float cirR = Math.Min((float)(m_WaferRecipe.DIE_SIZE_X * m_dZoomRatio_X_m_dScale), (float)(m_WaferRecipe.DIE_SIZE_Y * m_dZoomRatio_X_m_dScale));

                        g.FillEllipse(new SolidBrush(Color.Black), cirX - 3, cirY - 3, 3, 3);
                    }
                }
                if (m_bDrawFirstDie && pfFirst != null)
                {
                    g.DrawPolygon(pFirstDieBorder, pfFirst);
                }

                if (m_bDrawOriginDie && pfOrigin != null)
                {
                    g.DrawPolygon(pOriginDieBorder, pfOrigin);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pdPoint = null;
                pfPoint = null;

                if (pSelDieBorder != null) pSelDieBorder.Dispose();
                if (pDieBorder != null) pDieBorder.Dispose();
                if (pOriginDieBorder != null) pOriginDieBorder.Dispose();
                if (pFirstDieBorder != null) pFirstDieBorder.Dispose();
                if (sbrshDie != null) sbrshDie.Dispose();
            }
        }

        protected virtual void DrawDies(int x, int y)
        {
            if (m_arrDies.Count == 0) return;
            //m_WaferRecipe.NETDIE = 0;

            PointF[] pfFirst = null;
            PointF[] pfOrigin = null;

            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];

            /// Pen / Brush는 Dispose해야 하므로 Try밖에 선언하고 Finally에서 Dispose를 ...
            /// 메모리 확인해 본결과 효과 만점...
            Pen pSelDieBorder = null;
            Pen pDieBorder = null;
            Pen pOriginDieBorder = null;
            Pen pFirstDieBorder = null;

            // ForcusDie관련
            PointF[] pfFocusPoint = null;

            SolidBrush sbrshDie = null;

            float cirX = 0f;
            float cirY = 0f;
            float minX = 0f;
            float maxX = 0f;
            float minY = 0f;
            float maxY = 0f;
            float cirR = 0f;

            bool bOnWafer = false;
            //Graphics g = m_gdiTempMap;
            Graphics g = Graphics.FromImage(m_bmpTemp);
            g.DrawImageUnscaled(m_bmpWaferMap, 0, 0);

            try
            {
                pSelDieBorder = new Pen(Color.Red, 1);
                pDieBorder = new Pen(m_colDieBorder);
                pOriginDieBorder = new Pen(m_colOriginDieBorder);
                pFirstDieBorder = new Pen(m_colFirstDieBorder, 2);
                sbrshDie = new SolidBrush(Color.White);

                int idx = m_DieIndexer.IndexOf(new Point(x, y));
                if (idx < 0) return;
                Die InDie = m_arrDies[idx];

                if (InDie.Dummy == 1) return;
                if (InDie.BinNumber > 100) return;

                #region 좌표계산
                /// Die의 4 점의 좌표를 계산한다. ////////////////////////////////////////////////////////////////////////////////////////
                /// 

                pdPoint[0].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
                pdPoint[0].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

                pdPoint[1].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
                pdPoint[1].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

                pdPoint[2].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
                pdPoint[2].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

                pdPoint[3].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
                pdPoint[3].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

                RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[0].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[0].Y) * m_dZoomRatio_X_m_dScale);

                pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[1].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[1].Y) * m_dZoomRatio_X_m_dScale);

                pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[2].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[2].Y) * m_dZoomRatio_X_m_dScale);

                pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[3].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[3].Y) * m_dZoomRatio_X_m_dScale);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                bOnWafer = this.UseDie(InDie.DieCood.X - m_WaferRecipe.ORIGIN_X
                    , InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y
                    , this.m_WaferRecipe.DIE_SIZE_X
                    , this.m_WaferRecipe.DIE_SIZE_Y);

                if (bOnWafer == false && m_bVisibleOffDie == false) return;

                /// 0 Skip / 1 Probing / 2 Mark Die / 3 Pickuped 를 설정값에 맞춰서 Drawing한다. 
                switch (InDie.DieProp)
                {
                    case 0:
                        if (m_bDrawSkipDie)
                        {
                            sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colSkipDieColor);
                        }
                        //else return;
                        break;
                    case 1:
                        if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, InDie.BinNumber.ToString()) > -1)
                        {
                            try
                            {
                                sbrshDie.Color = m_ColorSet[InDie.BinNumber];
                                pDieBorder.Color = m_colDieBorder;
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        else
                        {
                            sbrshDie.Color = Color.FromArgb(m_iNotSelectedBinAlpha, m_ColorSet[InDie.BinNumber]);
                            pDieBorder.Color = Color.FromArgb(0, 0, 0, 0);
                        }
                        break;
                    case 2:
                        if (m_bDrawMarkDie)
                        {
                            sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colMarkDieColor);
                        }
                        else return;
                        break;
                    case 3:
                        if (m_colPickupedDie == Color.Transparent)
                        {
                            sbrshDie.Color = Color.FromArgb(m_iPickupedDieAlpha, m_ColorSet[InDie.BinNumber]);
                        }
                        else
                        {
                            sbrshDie.Color = Color.FromArgb(255, m_colPickupedDie);
                        }
                        pDieBorder.Color = m_colDieBorder;
                        break;
                    case 4:
                        if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, InDie.BinNumber.ToString()) > -1)
                        {
                            try
                            {
                                sbrshDie.Color = m_ColorSet[InDie.BinNumber];
                                pDieBorder.Color = Color.Yellow;
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        else
                        {
                            sbrshDie.Color = Color.FromArgb(m_iNotSelectedBinAlpha, m_ColorSet[InDie.BinNumber]);
                            pDieBorder.Color = Color.FromArgb(0, 0, 0, 0);
                        }
                        break;
                }

                if (InDie.DieProp > -1) g.FillPolygon(sbrshDie, pfPoint);
                //if (m_dZoomRatio > 0.6 && m_bVisibleDieBorder)
                if (m_bVisibleDieBorder)
                {
                    if (m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) > -1)
                    {
                        g.DrawPolygon(pSelDieBorder, pfPoint);
                    }
                    else
                    {
                        g.DrawPolygon(pDieBorder, pfPoint);
                    }
                }

                /// FoucusDie 관련////////////////////////////////////////////////////////////////////////
                if (m_FocusDie == new Point(InDie.IndexX, InDie.IndexY))
                {
                    pfFocusPoint = (PointF[])pfPoint.Clone();
                }
                //////////////////////////////////////////////////////////////////////////////////////////


                /// Origin / First Border를 설정값에 맞춰서 Drawing한다.
                if (InDie.IndexX == m_WaferRecipe.ORIGIN_DIE_X && InDie.IndexY == m_WaferRecipe.ORIGIN_DIE_Y)
                {
                    pfOrigin = new PointF[4];
                    Array.Copy(pfPoint, pfOrigin, 4);
                }

                if (InDie.IndexX == m_WaferRecipe.FIRST_DIE_X && InDie.IndexY == m_WaferRecipe.FIRST_DIE_Y)
                {
                    pfFirst = new PointF[4];
                    Array.Copy(pfPoint, pfFirst, 4);
                }

                //////////////////////////////////////////////////////////////////////////////////////////
                ///
                cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X) / 4;
                cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y) / 4;
                minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
                maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
                minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
                maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));
                cirR = Math.Min((float)(m_WaferRecipe.DIE_SIZE_X * m_dZoomRatio_X_m_dScale), (float)(m_WaferRecipe.DIE_SIZE_Y * m_dZoomRatio_X_m_dScale));

                if (m_bVisibleVIFaile && InDie.VIFail > 0)
                {
                    g.FillEllipse(new SolidBrush(m_ColorSet[InDie.VIFail]), cirX - cirR / 3f, cirY - cirR / 3f, cirR * (2f / 3f), cirR * (2f / 3f));
                    g.DrawEllipse(new Pen(Color.Red), cirX - cirR / 3f, cirY - cirR / 3f, cirR * (2f / 3f), cirR * (2f / 3f));
                }

                //if (m_bVisibleDieValue && m_dZoomRatio > 0.8)
                if (m_bVisibleDieValue)
                {

                    if (m_strDisplayDieValue == "BIN")
                    {
                        g.DrawString(InDie.BinNumber.ToString(), m_fntBin, new SolidBrush(Color.DarkGray), minX, minY);
                    }
                    else if (m_strDisplayDieValue == "PCMVALUE")
                    {
                        g.DrawString(InDie.ParametricValue.ToString(), m_fntPara, new SolidBrush(Util.GetIdealTextColor(sbrshDie.Color)), minX, minY);
                    }
                }

                //if (m_bVisibleXY && m_dZoomRatio > 0.8)
                if (m_bVisibleXY)
                {
                    g.DrawString(string.Format("{0},{1}", InDie.IndexX, InDie.IndexY), m_fntBin, new SolidBrush(Color.DarkGray), minX, maxY - m_fntBin.GetHeight(g));
                }

                /// Draw Skip Die
                /// 
                if (m_bDrawSkipDie && InDie.DieProp == 0)
                {
                    g.DrawLine(pDieBorder, minX, minY, maxX, maxY);
                    g.DrawLine(pDieBorder, maxX, minY, minX, maxY);
                }
                /// Draw Mark Die
                if (m_bDrawMarkDie && InDie.DieProp == 2)
                {
                    g.FillEllipse(new SolidBrush(Color.Black), cirX - 3, cirY - 3, 3, 3);
                }

                /// FoucusDie 관련////////////////////////////////////////////////////////////////////////
                if (m_bVisibleFocusDie && pfFocusPoint != null)
                {
                    float fx1 = Math.Min(Math.Min(pfFocusPoint[0].X, pfFocusPoint[1].X), Math.Min(pfFocusPoint[2].X, pfFocusPoint[3].X));
                    float fx2 = Math.Max(Math.Max(pfFocusPoint[0].X, pfFocusPoint[1].X), Math.Max(pfFocusPoint[2].X, pfFocusPoint[3].X));
                    float fy1 = Math.Min(Math.Min(pfFocusPoint[0].Y, pfFocusPoint[1].Y), Math.Min(pfFocusPoint[2].Y, pfFocusPoint[3].Y));
                    float fy2 = Math.Max(Math.Max(pfFocusPoint[0].Y, pfFocusPoint[1].Y), Math.Max(pfFocusPoint[2].Y, pfFocusPoint[3].Y));

                    Pen fcPen = new Pen(Color.Black, 4);
                    g.DrawPolygon(fcPen, pfFocusPoint);

                    if (m_ftFocus == FocusType.Close)
                    {
                        fcPen.Color = Color.Red;
                        fcPen.Width = 1;
                        g.DrawLine(fcPen, new PointF(0f, (fy1 + fy2) / 2f), new PointF(this.Width, (fy1 + fy2) / 2f));
                        g.DrawLine(fcPen, new PointF((fx1 + fx2) / 2f, 0f), new PointF((fx1 + fx2) / 2f, this.Height));

                        /// 그림자 (있으나 마나)
                        fcPen.Color = Color.DarkGray;
                        g.DrawLine(fcPen, new PointF(0f, (fy1 + fy2) / 2f + 1), new PointF(this.Width, (fy1 + fy2) / 2f + 1));
                        g.DrawLine(fcPen, new PointF((fx1 + fx2) / 2f + 1, 0f), new PointF((fx1 + fx2) / 2f + 1, this.Height));
                    }
                    else
                    {
                        g.DrawImage(m_bmpFocus, (fx1 + fx2) / 2f, (fy1 + fy2) / 2f);
                    }

                    fcPen.Dispose();
                    fcPen = null;

                    m_gdiMain.DrawImageUnscaled(m_bmpTemp, 0, 0);
                    //this.Invalidate(new Rectangle(new Point((int)fx1 - 5, (int)fy1 - 5), new Size((int)(fx2 - fx1) + 30, (int)(fy2 - fy1) + 30)));

                }
                ///////////////////////////////////////////////////////////////////////////////////////////


                if (m_bDrawFirstDie && pfFirst != null)
                {
                    g.DrawPolygon(pFirstDieBorder, pfFirst);
                }

                if (m_bDrawOriginDie && pfOrigin != null)
                {
                    g.DrawPolygon(pOriginDieBorder, pfOrigin);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pdPoint = null;
                pfPoint = null;

                if (pSelDieBorder != null) pSelDieBorder.Dispose();
                if (pDieBorder != null) pDieBorder.Dispose();
                if (pOriginDieBorder != null) pOriginDieBorder.Dispose();
                if (pFirstDieBorder != null) pFirstDieBorder.Dispose();
                if (sbrshDie != null) sbrshDie.Dispose();
                //this.Focus();
            }
        }

        /// <summary>
        /// 설정된 Shot 에 대한 X, Y 시작/끝 인덱스를 가져옵니다.
        /// </summary>
        public static void CalculateShotStartEndIndex(Point[] dies, int shotStartX, int shotStartY, int shotArrayX, int shotArrayY,
            out int xStart, out int yStart, out int xEnd, out int yEnd)
        {
            // Shot을 그리기 위한 DIE의 X, Y 시작 인덱스, 끝 인덱스
            xStart = shotStartX;
            yStart = shotStartY;
            xEnd = shotStartX;
            yEnd = shotStartY;

            if (dies == null || dies.Length == 0)
                return;

            int minX = Int32.MaxValue;
            int minY = Int32.MaxValue;
            int maxX = Int32.MinValue;
            int maxY = Int32.MinValue;

            foreach (Point pt in dies)
            {
                minX = Math.Min(minX, pt.X);
                minY = Math.Min(minY, pt.Y);
                maxX = Math.Max(maxX, pt.X);
                maxY = Math.Max(maxY, pt.Y);
            }

            while (xStart > minX)
                xStart -= shotArrayX;

            while (yStart > minY)
                yStart -= shotArrayY;

            while (xEnd <= maxX)
                xEnd += shotArrayX;

            while (yEnd <= maxY)
                yEnd += shotArrayY;
        }

        protected virtual void DrawShot(Graphics g)
        {
            if (!VisibleShot || !ExistsShotInfo)
                return;

            List<PointD> shotList = new List<PointD>();
            List<PointD> notiList = new List<PointD>();

            // Wafer 영역에만 Shot Grid가 그려 지도록 Clipping 한다. 2019.10.04 Taihi,Kim.
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(
                (float)(-m_rectdWaferArea.X * m_dZoomRatio_X_m_dScale),
                (float)(-m_rectdWaferArea.Y * m_dZoomRatio_X_m_dScale),
                (float)(m_WaferRecipe.WAFER_SIZE * m_dZoomRatio_X_m_dScale),
                (float)(m_WaferRecipe.WAFER_SIZE * m_dZoomRatio_X_m_dScale));

            g.SetClip(path, CombineMode.Intersect);

            // 시작/끝 인덱스 계산
            int xStart, yStart, xEnd, yEnd;
            CalculateShotStartEndIndex(DiesIndex, m_WaferRecipe.SHOT_START_X, m_WaferRecipe.SHOT_START_Y, m_WaferRecipe.SHOT_ARRAY_X, m_WaferRecipe.SHOT_ARRAY_Y,
                out xStart, out yStart, out xEnd, out yEnd);

            // Shot의 시작점
            double ptX = -m_WaferRecipe.ORIGIN_X - m_WaferRecipe.DIE_SIZE_X * (m_WaferRecipe.ORIGIN_DIE_X - m_WaferRecipe.SHOT_START_X);
            double ptY = -m_WaferRecipe.ORIGIN_Y - m_WaferRecipe.DIE_SIZE_Y * (m_WaferRecipe.ORIGIN_DIE_Y - m_WaferRecipe.SHOT_START_Y);

            // 각각의 Shot이 그려지는 시작점에 대한 Point 계산
            for (int x = xStart; x < xEnd; x += m_WaferRecipe.SHOT_ARRAY_X)
            {
                for (int y = yStart; y < yEnd; y += m_WaferRecipe.SHOT_ARRAY_Y)
                {
                    shotList.Add(new PointD(
                        ptX + (x - m_WaferRecipe.SHOT_START_X) * m_WaferRecipe.DIE_SIZE_X,
                        ptY + (y - m_WaferRecipe.SHOT_START_Y) * m_WaferRecipe.DIE_SIZE_Y));
                }
            }

            // Wafer 정렬을 위한 Shot 교차점 Point 계산
            if (_visibleShotAlignPoint && !DieOfLeftShotNotify.IsEmpty() && !DieOfRightShotNotify.IsEmpty())
            {
                notiList.Add(new PointD(
                    ptX + (DieOfLeftShotNotify.IndexX - m_WaferRecipe.SHOT_START_X) * m_WaferRecipe.DIE_SIZE_X,
                    ptY + (DieOfLeftShotNotify.IndexY - m_WaferRecipe.SHOT_START_Y) * m_WaferRecipe.DIE_SIZE_Y));

                notiList.Add(new PointD(
                    ptX + (DieOfRightShotNotify.IndexX - m_WaferRecipe.SHOT_START_X) * m_WaferRecipe.DIE_SIZE_X,
                    ptY + (DieOfRightShotNotify.IndexY - m_WaferRecipe.SHOT_START_Y) * m_WaferRecipe.DIE_SIZE_Y));
            }

            double width = m_WaferRecipe.DIE_SIZE_X * m_WaferRecipe.SHOT_ARRAY_X;
            double height = m_WaferRecipe.DIE_SIZE_Y * m_WaferRecipe.SHOT_ARRAY_Y;

            Pen pen = new Pen(ShotLineColor, ShotLineWidth);

            try
            {
                foreach (PointD point in shotList)
                {
                    PointD[] pdPoint = new PointD[]
                    {
                        point,
                        new PointD(point.X + width, point.Y),
                        new PointD(point.X + width, point.Y + height),
                        new PointD(point.X, point.Y + height)
                    };

                    RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                    PointF[] pfPoint = new PointF[4];

                    pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[0].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[0].Y) * m_dZoomRatio_X_m_dScale);

                    pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[1].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[1].Y) * m_dZoomRatio_X_m_dScale);

                    pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[2].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[2].Y) * m_dZoomRatio_X_m_dScale);

                    pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[3].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[3].Y) * m_dZoomRatio_X_m_dScale);

                    g.DrawPolygon(pen, pfPoint);
                }

                if (_visibleShotAlignPoint && !DieOfLeftShotNotify.IsEmpty() && !DieOfRightShotNotify.IsEmpty() && notiList.Count > 0)
                {
                    for (int i = 0; i < notiList.Count; i++)
                    {
                        PointD point = notiList[i];
                        RotatePoint(ref point.X, ref point.Y, m_iViewAngle);

                        PointF fpPoint = new PointF(
                            (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + point.X) * m_dZoomRatio_X_m_dScale),
                            (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - point.Y) * m_dZoomRatio_X_m_dScale));

                        using (Brush br = new SolidBrush(ShotNofityColor))
                            g.FillRectangle(br, new RectangleF(fpPoint.X - SHOT_NOTI_SIZE / 2, fpPoint.Y - SHOT_NOTI_SIZE / 2, SHOT_NOTI_SIZE, SHOT_NOTI_SIZE));
                    }
                }
            }
            finally
            {
                g.ResetClip();

                if (pen != null)
                    pen.Dispose();
            }
        }

        public void DrawCenteGrid()
        {
            DrawCenteGrid(m_gdiMain);
        }

        public void DrawCenteGrid(Graphics g)
        {
            Pen pCenterMark = null;

            try
            {
                if (m_bCenterMark)
                {
                    pCenterMark = new Pen(Color.Red);
                    g.DrawLine(pCenterMark, (float)(-m_rectdWaferArea.X * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale));

                    g.DrawLine(pCenterMark, (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale)
                        , (float)(-m_rectdWaferArea.Y * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE) * m_dZoomRatio_X_m_dScale));
                }
                //				else
                //				{
                //					if(m_gdiMain != null && m_bmpWaferMap != null) m_gdiMain.DrawImageUnscaled(m_bmpWaferMap,0,0);
                //				}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pCenterMark != null) pCenterMark.Dispose();
            }
        }

        protected void ClearMarkDie()
        {
            if (m_gdiMain == null || m_bmpWaferMap == null) return;
            try
            {
                m_gdiMain.DrawImageUnscaled(m_bmpWaferMap, 0, 0);
                DrawCenteGrid();
            }
            catch
            {
            }
        }

        public void RedrawDie(Die argDie, Color colDie, Color colBorder)
        {
            if (m_gdiMain == null || m_bmpWaferMap == null) return;
            try
            {
                m_gdiMain.DrawImageUnscaled(m_bmpWaferMap, 0, 0);
                DrawCenteGrid();

                PointD[] pdPoint = new PointD[4];

                pdPoint[0].X = argDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
                pdPoint[0].Y = argDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

                pdPoint[1].X = argDie.DieCood.X + argDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
                pdPoint[1].Y = argDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

                pdPoint[2].X = argDie.DieCood.X + argDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
                pdPoint[2].Y = argDie.DieCood.Y + argDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

                pdPoint[3].X = argDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
                pdPoint[3].Y = argDie.DieCood.Y + argDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

                RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                PointF[] pfPoint = new PointF[4];
                pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[0].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[0].Y) * m_dZoomRatio_X_m_dScale);

                pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[1].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[1].Y) * m_dZoomRatio_X_m_dScale);

                pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[2].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[2].Y) * m_dZoomRatio_X_m_dScale);

                pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[3].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[3].Y) * m_dZoomRatio_X_m_dScale);

                PointF[] pfFocusPoint = (PointF[])pfPoint.Clone();

                float fx1 = Math.Min(Math.Min(pfFocusPoint[0].X, pfFocusPoint[1].X), Math.Min(pfFocusPoint[2].X, pfFocusPoint[3].X));
                float fx2 = Math.Max(Math.Max(pfFocusPoint[0].X, pfFocusPoint[1].X), Math.Max(pfFocusPoint[2].X, pfFocusPoint[3].X));
                float fy1 = Math.Min(Math.Min(pfFocusPoint[0].Y, pfFocusPoint[1].Y), Math.Min(pfFocusPoint[2].Y, pfFocusPoint[3].Y));
                float fy2 = Math.Max(Math.Max(pfFocusPoint[0].Y, pfFocusPoint[1].Y), Math.Max(pfFocusPoint[2].Y, pfFocusPoint[3].Y));

                Pen fcPen = new Pen(Color.Black, 4);
                m_gdiMain.DrawPolygon(fcPen, pfFocusPoint);

                if (m_ftFocus == FocusType.Close)
                {
                    m_gdiMain.DrawImageUnscaled(m_bmpWaferMap, 0, 0);
                    m_gdiMain.DrawPolygon(new Pen(colBorder), pfPoint);
                }
                else
                {
                    m_gdiMain.DrawImage(m_bmpFocus, (fx1 + fx2) / 2f, (fy1 + fy2) / 2f);
                }

                DrawCenteGrid();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //if (m_gdiMain == null || m_bmpWaferMap == null) return;

            //bool bOnWafer = false;

            //Pen pSelDieBorder = null;
            //Pen pDieBorder = null;
            //Pen pOriginDieBorder = null;
            //Pen pFirstDieBorder = null;
            //SolidBrush sbrshDie = null;
            //float cirX = 0f;
            //float cirY = 0f;
            //float minX = 0f;
            //float maxX = 0f;
            //float minY = 0f;
            //float maxY = 0f;
            //float cirR = 0f;
            //try
            //{
            //    pSelDieBorder = new Pen(Color.Red, 3);
            //    pDieBorder = new Pen(m_colDieBorder);
            //    pOriginDieBorder = new Pen(m_colOriginDieBorder);
            //    pFirstDieBorder = new Pen(m_colFirstDieBorder);
            //    sbrshDie = new SolidBrush(Color.White);

            //    PointD[] pdPoint = new PointD[4];

            //    pdPoint[0].X = argDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
            //    pdPoint[0].Y = argDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

            //    pdPoint[1].X = argDie.DieCood.X + argDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
            //    pdPoint[1].Y = argDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

            //    pdPoint[2].X = argDie.DieCood.X + argDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
            //    pdPoint[2].Y = argDie.DieCood.Y + argDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

            //    pdPoint[3].X = argDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
            //    pdPoint[3].Y = argDie.DieCood.Y + argDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

            //    RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
            //    RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
            //    RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
            //    RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

            //    PointF[] pfPoint = new PointF[4];
            //    pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
            //    pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

            //    pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
            //    pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

            //    pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
            //    pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

            //    pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
            //    pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));


            //    bOnWafer = this.UseDie(argDie.DieCood.X - m_WaferRecipe.ORIGIN_X
            //     , argDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y
            //     , this.m_WaferRecipe.DIE_SIZE_X
            //     , this.m_WaferRecipe.DIE_SIZE_Y);

            //    if (bOnWafer == false && m_bVisibleOffDie == false) return;

            //    /// 0 Skip / 1 Probing / 2 Mark Die / 3 Pickuped 를 설정값에 맞춰서 Drawing한다. 
            //    switch (argDie.DieProp)
            //    {
            //        case 0:
            //            if (m_bDrawSkipDie)
            //            {
            //                sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colSkipDieColor);
            //            }
            //            //else return;
            //            break;
            //        case 1:
            //            if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, argDie.BinNumber.ToString()) > -1)
            //            {
            //                try
            //                {
            //                    sbrshDie.Color = m_ColorSet[argDie.BinNumber];
            //                    pDieBorder.Color = m_colDieBorder;
            //                }
            //                catch (Exception ex)
            //                {
            //                    throw ex;
            //                }
            //            }
            //            else
            //            {
            //                sbrshDie.Color = Color.FromArgb(m_iNotSelectedBinAlpha, m_ColorSet[argDie.BinNumber]);
            //                pDieBorder.Color = Color.FromArgb(0, 0, 0, 0);
            //            }
            //            break;
            //        case 2:
            //            if (m_bDrawMarkDie)
            //            {
            //                sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colMarkDieColor);
            //            }
            //            else return;
            //            break;
            //        case 3:
            //            if (m_colPickupedDie == Color.Transparent)
            //            {
            //                sbrshDie.Color = Color.FromArgb(m_iPickupedDieAlpha, m_ColorSet[argDie.BinNumber]);
            //            }
            //            else
            //            {
            //                sbrshDie.Color = Color.FromArgb(255, m_colPickupedDie);
            //            }
            //            pDieBorder.Color = m_colDieBorder;
            //            break;
            //        case 4:
            //            if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, argDie.BinNumber.ToString()) > -1)
            //            {
            //                try
            //                {
            //                    sbrshDie.Color = m_ColorSet[argDie.BinNumber];
            //                    pDieBorder.Color = m_colDieBorder;
            //                }
            //                catch (Exception ex)
            //                {
            //                    throw ex;
            //                }
            //            }
            //            else
            //            {
            //                sbrshDie.Color = Color.FromArgb(m_iNotSelectedBinAlpha, m_ColorSet[argDie.BinNumber]);
            //                pDieBorder.Color = Color.FromArgb(0, 0, 0, 0);
            //            }
            //            break;
            //    }

            //    if (argDie.DieProp > -1) m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
            //    //////////////////////////////////////////////////////////////////////////////////////////
            //    ///
            //    cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X) / 4;
            //    cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y) / 4;
            //    minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
            //    maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
            //    minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
            //    maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));
            //    cirR = Math.Min((float)(m_WaferRecipe.DIE_SIZE_X * (m_dZoomRatio * m_dScale)), (float)(m_WaferRecipe.DIE_SIZE_Y * (m_dZoomRatio * m_dScale)));


            //    if (m_bVisibleDieValue && m_dZoomRatio > 0.8)
            //    {

            //        if (m_strDisplayDieValue == "BIN")
            //        {
            //            m_gdiTempMap.DrawString(argDie.BinNumber.ToString(), m_fntBin, new SolidBrush(Color.DarkGray), minX, minY);
            //        }
            //        else if (m_strDisplayDieValue == "PCMVALUE")
            //        {
            //            m_gdiTempMap.DrawString(argDie.ParametricValue.ToString(), m_fntPara, new SolidBrush(Color.DarkGray), minX, minY);
            //        }
            //    }

            //    m_gdiTempMap.DrawPolygon(new Pen(colBorder), pfPoint);
            //    //m_gdiMain.DrawPolygon(new Pen(colBorder), pfPoint);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        #endregion

        public void SetDefaultColors()
        {
            SetDefaultColor();
        }

        #region ■ Default Color
        protected virtual void SetDefaultColor()
        {
            int r, g, b;//4337915
            //16777215
            try
            {
                int[] iColor = new int[] {16766976,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
										65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
										16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
										10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
										8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
										6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
										12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
										10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
										2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
										10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686
										,2521304,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
										65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
										16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
										10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
										8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
										6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
										12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
										10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
										2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
										10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686,
										6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
										12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
										10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
										2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
										10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686,
                                        8421504,4227136,12632256,9868950,6795178,4446555};

                if (m_ColorSet != null) m_ColorSet = null;
                m_ColorSet = new Color[iColor.Length];
                for (int i = 0; i < iColor.Length; i++)
                {
                    r = (iColor[i] >> 16);
                    g = (iColor[i] >> 8) - (r * 256);
                    b = iColor[i] - (r * 65536) - (g * 256);
                    m_ColorSet[i] = Color.FromArgb(r, g, b);
                }

                m_VIColorSet = m_ColorSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        #endregion

        public void SetColor(int Index, Color BinColor)
        {
            m_ColorSet[Index] = BinColor;
        }

        public void SetColor(int Index, Color BinColor, int Alpha)
        {
            m_ColorSet[Index] = Color.FromArgb(Alpha, BinColor); ;
        }

        public Color GetColor(int Index)
        {
            return m_ColorSet[Index];
        }

        /// <summary>
        /// Shot Option 변경을 알립니다.
        /// </summary>
        protected virtual void OnShotOptionChanged(EventArgs e)
        {
            if (ShotOptionChanged != null)
                ShotOptionChanged(this, e);
        }

        public void SetVIColor(int Index, Color BinColor)
        {
            m_VIColorSet[Index] = BinColor;
        }

        public Color GetVIColor(int Index)
        {
            return m_VIColorSet[Index];
        }

        #region ◈ Popup Menu Click Event 처리
        protected void mnuitemMAPMODE_FREEZOOM_Click(object sender, System.EventArgs e)
        {
            m_eoMapMode = MapMode.Free;
            m_eoMouseDragMode = MouseDragMode.Zoom;

            mnuitemMAPMODE_FREEZOOM.Checked = true;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;

            MenuManagment();
        }

        protected void mnuitemMAPMODE_FITSIZE_Click(object sender, System.EventArgs e)
        {
            MapMode Last = m_eoMapMode;
            try
            {
                m_eoMapMode = MapMode.Fit;
                //m_eoMouseDragMode = MouseDragMode.Normal;

                //mnuitemMAPMODE_FREEZOOM.Checked = false;
                //mnuitemMAPMODE_FITSIZE.Checked = true;
                //mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
                //mnuitemMOUSEDRAGMODE_ZONE.Checked = false;
                //mnuitemMOUSEDRAGMODE_MOVE.Checked = false;

                OnResize(EventArgs.Empty);

                MenuManagment();
            }
            catch
            {
            }
            finally
            {
                m_eoMapMode = Last;
            }

        }

        protected void mnuitemMOUSEDRAGMODE_ROTATE_Click(object sender, System.EventArgs e)
        {
            //m_eoMapMode = MapMode.Fit; 변화없음
            m_eoMouseDragMode = MouseDragMode.Rotate;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = true;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;

            MenuManagment();
        }

        protected void mnuitemMOUSEDRAGMODE_ZONE_Click(object sender, System.EventArgs e)
        {
            //m_eoMapMode = MapMode.Fit; 변화없음
            m_eoMouseDragMode = MouseDragMode.Zone;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = true;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;

            MenuManagment();

        }

        protected void mnuitemMOUSEDRAGMODE_MOVE_Click(object sender, System.EventArgs e)
        {
            m_eoMapMode = MapMode.Free;
            m_eoMouseDragMode = MouseDragMode.Move;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = true;

            MenuManagment();
        }

        protected void mnuitemMAPSELECTSTYLE_CIRCLE_Click(object sender, System.EventArgs e)
        {
            if (m_eoMouseDragMode != MouseDragMode.Defect)
                m_eoMouseDragMode = MouseDragMode.Zone;

            m_eoMapSelectStyle = MapSelectStyle.Circle;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = true;
            mnuitemMAPSELECTSTYLE_BAND.Checked = false;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
        }

        protected void mnuitemMAPSELECTSTYLE_PIE_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (m_eoMouseDragMode != MouseDragMode.Defect)
                    m_eoMouseDragMode = MouseDragMode.Zone;

                m_eoMapSelectStyle = MapSelectStyle.Pie;

                mnuitemMAPMODE_FREEZOOM.Checked = false;
                mnuitemMAPMODE_FITSIZE.Checked = false;
                mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
                mnuitemMOUSEDRAGMODE_MOVE.Checked = false;

                mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
                mnuitemMAPSELECTSTYLE_BAND.Checked = false;
                mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
                mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
                mnuitemMAPSELECTSTYLE_PIE.Checked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void mnuitemMAPSELECTSTYLE_RECTANGLE_Click(object sender, System.EventArgs e)
        {
            if (m_eoMouseDragMode != MouseDragMode.Defect)
                m_eoMouseDragMode = MouseDragMode.Zone;

            m_eoMapSelectStyle = MapSelectStyle.Rectangle;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            mnuitemMAPSELECTSTYLE_BAND.Checked = false;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = true;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
        }

        protected void mnuitemMAPSELECTSTYLE_FREEHAND_Click(object sender, System.EventArgs e)
        {
            if (m_eoMouseDragMode != MouseDragMode.Defect)
                m_eoMouseDragMode = MouseDragMode.Zone;

            m_eoMapSelectStyle = MapSelectStyle.FreeHand;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            mnuitemMAPSELECTSTYLE_BAND.Checked = false;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = true;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
        }

        protected void mnuitemMAPSELECTSTYLE_BAND_Click(object sender, System.EventArgs e)
        {
            if (m_eoMouseDragMode != MouseDragMode.Defect)
                m_eoMouseDragMode = MouseDragMode.Zone;

            m_eoMapSelectStyle = MapSelectStyle.Band;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            mnuitemMAPSELECTSTYLE_BAND.Checked = true;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
        }

        protected void MenuManagment()
        {
            menuitemMAPMODE_ZOOMIN.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;
            menuitemMAPMODE_ZOOMOUT.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;
        }

        protected void menuitemMAPMODE_ZOOMIN_Click(object sender, System.EventArgs e)
        {
            ZoomIn();
        }

        protected void menuitemMAPMODE_ZOOMOUT_Click(object sender, System.EventArgs e)
        {
            ZoomOut();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0) ZoomIn();
            if (e.Delta < 0) ZoomOut();
            base.OnMouseWheel(e);
        }

        private void mnuCOPYTOCLIP_Click(object sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(m_bmpWaferMap);
        }


        private void mnuitemVISIBLE_MARKDIE_Click(object sender, EventArgs e)
        {
            mnuitemVISIBLE_MARKDIE.Checked = !mnuitemVISIBLE_MARKDIE.Checked;
            m_bDrawMarkDie = mnuitemVISIBLE_MARKDIE.Checked;
            this.Redraw();
        }

        private void mnuitemVISIBLE_SHOT_Click(object sender, EventArgs e)
        {
            mnuitemVISIBLE_SHOT.Checked = !mnuitemVISIBLE_SHOT.Checked;
            VisibleShot = mnuitemVISIBLE_SHOT.Checked;
            this.Redraw();
        }

        private void ctxmWaferMap_Popup(object sender, EventArgs e)
        {
            mnuitemVISIBLE_SHOT.Enabled = ExistsShotInfo;
            mnuitemVISIBLE_SHOT.Checked = ExistsShotInfo && VisibleShot;
        }

        #endregion


        public Bitmap GetMapImage()
        {
            return m_bmpWaferMap;
        }

        public WaferRecipe GetWaferRecipe()
        {
            return m_WaferRecipe;
        }

        public void SetWaferRecipe(WaferRecipe wr)
        {
            m_WaferRecipe = wr;
        }

        private void mnuitemCLEAR_SELECTEDDIE_Click(object sender, System.EventArgs e)
        {
            ResetSelectedDie();
        }

        public void InkingDieReset()
        {
            for (int i = m_arrDies.Count - 1; i >= 0; i--)
            {
                Die oDie = m_arrDies[i];
                if (oDie.DieProp == 2)
                {
                    m_arrDies.RemoveAt(i);
                    m_DieIndexer.RemoveAt(i);
                }
            }
        }

        public void InkingDieAdd()
        {
            int iAddDiesX = (int)((m_WaferRecipe.WAFER_SIZE - (m_WaferRecipe.XDIES * m_WaferRecipe.DIE_SIZE_X)) / m_WaferRecipe.DIE_SIZE_X);
            int iAddDiesY = (int)((m_WaferRecipe.WAFER_SIZE - (m_WaferRecipe.YDIES * m_WaferRecipe.DIE_SIZE_Y)) / m_WaferRecipe.DIE_SIZE_Y);

            int iStartX = m_WaferRecipe.DIE_INDEX_MIN_X - ((iAddDiesX / 2) + 1);
            int iEndX = m_WaferRecipe.DIE_INDEX_MAX_X + ((iAddDiesX / 2) + 1);
            int iStartY = m_WaferRecipe.DIE_INDEX_MIN_Y - ((iAddDiesY / 2) + 1);
            int iEndY = m_WaferRecipe.DIE_INDEX_MAX_Y + ((iAddDiesY / 2) + 1);
            for (int ix = iStartX; ix < iEndX; ix++)
            {
                for (int iy = iStartY; iy < iEndY; iy++)
                {
                    if (m_DieIndexer.IndexOf(new Point(ix, iy)) < 0)
                    {
                        AddDie(ix, iy, 0);
                    }
                }
            }
        }


        private int DieProperty(double X, double Y, double Width, double Height)
        {
            double Xr = X - m_WaferRecipe.ORIGIN_X;
            double Yr = Y - m_WaferRecipe.ORIGIN_Y;

            double Xc = (Xr + (Width / 2.0d));
            double Yc = (Yr + (Height / 2.0d));

            int[] iEdge = new int[] { 0, 0, 0, 0 };
            bool iCenter = false;
            int iRealAngle = (m_WaferRecipe.ANGLE + m_iAngleOffset) % 360;
            RectangleF rectDie = new RectangleF((float)Xr, (float)Yr, (float)Width, (float)Height);

            if (((m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.EDGE_SIZE)) > Math.Sqrt(Math.Pow(Xr, 2) + Math.Pow(Yr + Height, 2))) iEdge[0] = 1;
            if (((m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.EDGE_SIZE)) > Math.Sqrt(Math.Pow(Xr + Width, 2) + Math.Pow(Yr + Height, 2))) iEdge[1] = 1;
            if (((m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.EDGE_SIZE)) > Math.Sqrt(Math.Pow(Xr, 2) + Math.Pow(Yr, 2))) iEdge[2] = 1;
            if (((m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.EDGE_SIZE)) > Math.Sqrt(Math.Pow(Xr + Width, 2) + Math.Pow(Yr, 2))) iEdge[3] = 1;
            if (((m_WaferRecipe.WAFER_RADIUS - m_WaferRecipe.EDGE_SIZE)) > Math.Sqrt(Math.Pow(Xc, 2) + Math.Pow(Yc, 2))) iCenter = true;

            //if (!iCenter)
            //{
            //    //if((iEdge[0] + iEdge[1] + iEdge[2] + iEdge[3]) >= 2) return 2;

            //    return -1;
            //}

            if (m_rectOCRIDArea.IntersectsWith(rectDie)) return -1;

            return 2;

        }

        public Die GetDie(int x, int y)
        {
            int idx = m_DieIndexer.IndexOf(new Point(x, y));
            if (idx < 0) return new Die(-99999, -99999, 0, new RectangleD());
            return (Die)this.m_arrDies[idx];
        }

        #region ■ Print Wafer
        public void Print(Graphics g, Rectangle rect)
        {
            if (rect.Width * rect.Height == 0 || g == null) return;

            PointD dNotchStart;
            PointD dNotchEnd;
            GraphicsPath gpWafer = null;

            SolidBrush sbrshEdge = null;
            SolidBrush sbrshWafer = null;
            Pen pEdge = null;
            Pen pWafer = null;

            double dMinCanvers = Math.Min(rect.Width, rect.Height);
            double dZoomRatio = dMinCanvers / m_WaferRecipe.WAFER_SIZE;
            RectangleD rectdWaferArea = new RectangleD();
            rectdWaferArea.X = ((dMinCanvers - rect.Width) / 2.0f) / dZoomRatio;
            rectdWaferArea.Y = ((dMinCanvers - rect.Height) / 2.0f) / dZoomRatio;
            rectdWaferArea.Width = m_WaferRecipe.WAFER_SIZE;
            rectdWaferArea.Height = m_WaferRecipe.WAFER_SIZE;

            Font fntWID = new Font("굴림", Math.Max(9, (float)((dZoomRatio * m_dScale) * 1.5f)));
            m_fntBin = new Font("굴림", Math.Max(9, (float)((dZoomRatio * m_dScale) * 0.9f)));
            int iRealAngle = (m_WaferRecipe.ANGLE + m_iAngleOffset) % 360;

            try
            {
                gpWafer = new GraphicsPath();

                if (m_bScale) DrawScale(g);

                ///=======Wafer Edge Drawing ========================================================================
                gpWafer.AddArc((float)((-rectdWaferArea.X + m_WaferRecipe.EDGE_SIZE) * (dZoomRatio * m_dScale))
                    , (float)((-rectdWaferArea.Y + m_WaferRecipe.EDGE_SIZE) * (dZoomRatio * m_dScale))
                    , (float)((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) * (dZoomRatio * m_dScale))
                    , (float)((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) * (dZoomRatio * m_dScale))
                    , (float)((m_iViewAngle + iRealAngle + 90 + m_dNotchSize / 2) % 360)
                    , (float)(360 - m_dNotchSize));

                if (m_WaferRecipe.NOTCH_TYPE != Notch.Notch)
                {
                    dNotchStart.X = -(float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                    dNotchStart.Y = -(float)Math.Cos((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                    dNotchEnd.X = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                    dNotchEnd.Y = dNotchStart.Y;

                    RotatePoint(ref dNotchStart.X, ref dNotchStart.Y, (iRealAngle + m_iViewAngle) % 360);
                    RotatePoint(ref dNotchEnd.X, ref dNotchEnd.Y, (iRealAngle + m_iViewAngle) % 360);

                    gpWafer.AddLine((float)((-rectdWaferArea.X + dNotchStart.X + (m_WaferRecipe.WAFER_RADIUS)) * (dZoomRatio * m_dScale))
                        , (float)((-rectdWaferArea.Y +m_WaferRecipe.WAFER_RADIUS- dNotchStart.Y) * (dZoomRatio * m_dScale))
                        , (float)((-rectdWaferArea.X + dNotchEnd.X + (m_WaferRecipe.WAFER_RADIUS)) * (dZoomRatio * m_dScale))
                        , (float)((-rectdWaferArea.Y +m_WaferRecipe.WAFER_RADIUS- dNotchEnd.Y) * (dZoomRatio * m_dScale)));
                }
                ///==================================================================================================
                ///
                sbrshWafer = new SolidBrush(m_colWafer);
                pWafer = new Pen(m_colWafer);
                g.FillPath(sbrshWafer, gpWafer);
                g.DrawPath(pWafer, gpWafer);

                gpWafer.StartFigure();

                ///=======Wafer Line Drawing ========================================================================
                ///
                gpWafer.AddArc((float)(-rectdWaferArea.X * (dZoomRatio * m_dScale))
                    , (float)(-rectdWaferArea.Y * (dZoomRatio * m_dScale))
                    , (float)(m_WaferRecipe.WAFER_SIZE * (dZoomRatio * m_dScale))
                    , (float)(m_WaferRecipe.WAFER_SIZE * (dZoomRatio * m_dScale))
                    , (float)((m_iViewAngle + iRealAngle + 90 + m_dNotchSize / 2) % 360)
                    , (float)(360 - m_dNotchSize));

                dNotchStart.X = -(float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_RADIUS);
                dNotchStart.Y = -(float)Math.Cos((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_RADIUS);
                dNotchEnd.X = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_RADIUS);
                dNotchEnd.Y = dNotchStart.Y;

                RotatePoint(ref dNotchStart.X, ref dNotchStart.Y, (iRealAngle + m_iViewAngle) % 360);
                RotatePoint(ref dNotchEnd.X, ref dNotchEnd.Y, (iRealAngle + m_iViewAngle) % 360);

                if (m_WaferRecipe.NOTCH_TYPE == Notch.Notch)
                {
                    PointD dNotchRStart;
                    PointD dNotchREnd;
                    double dNotchR = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_RADIUS);
                    dNotchRStart.X = (dNotchEnd.X + dNotchStart.X) / 2 - dNotchR;
                    dNotchRStart.Y = (dNotchEnd.Y + dNotchStart.Y) / 2;
                    dNotchREnd.X = (dNotchEnd.X + dNotchStart.X) / 2 + dNotchR;
                    dNotchREnd.Y = (dNotchEnd.Y + dNotchStart.Y) / 2;

                    gpWafer.AddArc((float)((-rectdWaferArea.X + dNotchRStart.X + (m_WaferRecipe.WAFER_RADIUS)) * (dZoomRatio * m_dScale))
                        , (float)((-rectdWaferArea.Y - dNotchRStart.Y +m_WaferRecipe.WAFER_RADIUS- (dNotchREnd.X - dNotchRStart.X) / 2) * (dZoomRatio * m_dScale))
                        , (float)((dNotchREnd.X - dNotchRStart.X) * (dZoomRatio * m_dScale))
                        , (float)((dNotchREnd.X - dNotchRStart.X) * (dZoomRatio * m_dScale))
                        , (float)((m_iViewAngle + iRealAngle + 180 + m_dNotchSize / 2) % 360)
                        , (float)(180));
                }
                else
                {
                    gpWafer.AddLine((float)((-rectdWaferArea.X + dNotchStart.X + (m_WaferRecipe.WAFER_RADIUS)) * (dZoomRatio * m_dScale))
                        , (float)((-rectdWaferArea.Y +m_WaferRecipe.WAFER_RADIUS- dNotchStart.Y) * (dZoomRatio * m_dScale))
                        , (float)((-rectdWaferArea.X + dNotchEnd.X + (m_WaferRecipe.WAFER_RADIUS)) * (dZoomRatio * m_dScale))
                        , (float)((-rectdWaferArea.Y +m_WaferRecipe.WAFER_RADIUS- dNotchEnd.Y) * (dZoomRatio * m_dScale)));
                }


                ///========================================================================================================================
                ///
                sbrshEdge = new SolidBrush(m_colEdge);
                pEdge = new Pen(m_colEdge);
                g.FillPath(sbrshEdge, gpWafer);
                g.DrawPath(pEdge, gpWafer);


                /// Notch부분의 사각형을 정의한다.
                ///======================================================================================================================== 
                PointD[] pdPoint = new PointD[4];

                pdPoint[0].X = -Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdPoint[0].Y = -Math.Cos((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdPoint[1].X = Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdPoint[1].Y = pdPoint[0].Y;

                pdPoint[2].X = pdPoint[1].X;
                pdPoint[2].Y = -(m_WaferRecipe.WAFER_RADIUS);
                pdPoint[3].X = pdPoint[0].X;
                pdPoint[3].Y = -(m_WaferRecipe.WAFER_RADIUS);

                RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, (iRealAngle) % 360);
                RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, (iRealAngle) % 360);
                RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, (iRealAngle) % 360);
                RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, (iRealAngle) % 360);

                double dMinX = Math.Min(Math.Min(pdPoint[0].X, pdPoint[1].X), Math.Min(pdPoint[2].X, pdPoint[3].X));
                double dMaxX = Math.Max(Math.Max(pdPoint[0].X, pdPoint[1].X), Math.Max(pdPoint[2].X, pdPoint[3].X));
                double dMinY = Math.Min(Math.Min(pdPoint[0].Y, pdPoint[1].Y), Math.Min(pdPoint[2].Y, pdPoint[3].Y));
                double dMaxY = Math.Max(Math.Max(pdPoint[0].Y, pdPoint[1].Y), Math.Max(pdPoint[2].Y, pdPoint[3].Y));

                m_rectNotchArea.X = (float)dMinX;
                m_rectNotchArea.Y = (float)dMaxY;
                m_rectNotchArea.Width = (float)(dMaxX - dMinX);
                m_rectNotchArea.Height = (float)(dMaxY - dMinY);
                ///========================================================================================================================
                /// 
                /// 
                /// OCRID부분의 사각형을 정의한다.
                ///======================================================================================================================== 
                PointD[] pdOCRPoint = new PointD[4];

                pdOCRPoint[0].X = -Math.Sin(8.0d / 180.0d * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdOCRPoint[0].Y = -Math.Cos(8.0d / 180.0d * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdOCRPoint[1].X = Math.Sin(8.0d / 180.0d * Math.PI) * ((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) / 2);
                pdOCRPoint[1].Y = pdOCRPoint[0].Y;

                pdOCRPoint[2].X = pdOCRPoint[1].X;
                pdOCRPoint[2].Y = -(m_WaferRecipe.WAFER_RADIUS);
                pdOCRPoint[3].X = pdOCRPoint[0].X;
                pdOCRPoint[3].Y = -(m_WaferRecipe.WAFER_RADIUS);

                RotatePoint(ref pdOCRPoint[0].X, ref pdOCRPoint[0].Y, (iRealAngle) % 360);
                RotatePoint(ref pdOCRPoint[1].X, ref pdOCRPoint[1].Y, (iRealAngle) % 360);
                RotatePoint(ref pdOCRPoint[2].X, ref pdOCRPoint[2].Y, (iRealAngle) % 360);
                RotatePoint(ref pdOCRPoint[3].X, ref pdOCRPoint[3].Y, (iRealAngle) % 360);

                double dOCRMinX = Math.Min(Math.Min(pdOCRPoint[0].X, pdOCRPoint[1].X), Math.Min(pdOCRPoint[2].X, pdOCRPoint[3].X));
                double dOCRMaxX = Math.Max(Math.Max(pdOCRPoint[0].X, pdOCRPoint[1].X), Math.Max(pdOCRPoint[2].X, pdOCRPoint[3].X));
                double dOCRMinY = Math.Min(Math.Min(pdOCRPoint[0].Y, pdOCRPoint[1].Y), Math.Min(pdOCRPoint[2].Y, pdOCRPoint[3].Y));
                double dOCRMaxY = Math.Max(Math.Max(pdOCRPoint[0].Y, pdOCRPoint[1].Y), Math.Max(pdOCRPoint[2].Y, pdOCRPoint[3].Y));

                m_rectOCRIDArea.X = (float)dOCRMinX;
                m_rectOCRIDArea.Y = (float)dOCRMaxY;
                m_rectOCRIDArea.Width = (float)(dOCRMaxX - dOCRMinX);
                m_rectOCRIDArea.Height = (float)(dOCRMaxY - dOCRMinY);
                ///========================================================================================================================


                PrintDrawDies(g, rect);


                /// Wafer ID Draw
                ///========================================================================================================================
                ///
                if (m_strInfomation != null && m_bVisibleInfo)
                {
                    for (int i = 0; i < m_strInfomation.Count; i++)
                    {
                        g.DrawString(m_strInfomation[i].ToString(), fntWID, Brushes.Black,
                            //(float)((-rectdWaferArea.X + 2.0) * (dZoomRatio * m_dScale)), (float)((-rectdWaferArea.Y + 2.0) * (dZoomRatio * m_dScale) + (fntWID.Height * i)));
                            2, Height - fntWID.Height * m_strInfomation.Count + fntWID.Height * i);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (gpWafer != null) gpWafer.Dispose();
                if (sbrshEdge != null) sbrshEdge.Dispose();
                if (pEdge != null) pEdge.Dispose();
                if (sbrshWafer != null) sbrshWafer.Dispose();
                if (pWafer != null) pWafer.Dispose();
                if (fntWID != null) fntWID.Dispose();
            }
        }
        #endregion

        #region ■ Die Drawing for Print
        protected virtual void PrintDrawDies(Graphics g, Rectangle rect)
        {
            if (m_arrDies.Count == 0) return;
            m_WaferRecipe.NETDIE = 0;

            PointF[] pfFirst = null;
            PointF[] pfOrigin = null;

            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];

            /// Pen / Brush는 Dispose해야 하므로 Try밖에 선언하고 Finally에서 Dispose를 ...
            /// 메모리 확인해 본결과 효과 만점...
            Pen pSelDieBorder = null;
            Pen pDieBorder = null;
            Pen pOriginDieBorder = null;
            Pen pFirstDieBorder = null;

            SolidBrush sbrshDie = null;

            float cirX = 0f;
            float cirY = 0f;
            float minX = 0f;
            float maxX = 0f;
            float minY = 0f;
            float maxY = 0f;
            float cirR = 0f;

            bool bOnWafer = false;
            try
            {
                double dMinCanvers = Math.Min(rect.Width, rect.Height);
                double dZoomRatio = dMinCanvers / m_WaferRecipe.WAFER_SIZE;
                RectangleD rectdWaferArea = new RectangleD();
                rectdWaferArea.X = ((dMinCanvers - rect.Width) / 2.0f) / dZoomRatio;
                rectdWaferArea.Y = ((dMinCanvers - rect.Height) / 2.0f) / dZoomRatio;
                rectdWaferArea.Width = m_WaferRecipe.WAFER_SIZE;
                rectdWaferArea.Height = m_WaferRecipe.WAFER_SIZE;

                pSelDieBorder = new Pen(Color.Red, 1);
                pDieBorder = new Pen(m_colDieBorder);
                pOriginDieBorder = new Pen(m_colOriginDieBorder);
                pFirstDieBorder = new Pen(m_colFirstDieBorder);
                sbrshDie = new SolidBrush(Color.White);

                foreach (Die InDie in m_arrDies)
                {
                    if (InDie.Dummy == 1) continue;
                    if (InDie.BinNumber > 100) continue;
                    /// Die의 4 점의 좌표를 계산한다. ////////////////////////////////////////////////////////////////////////////////////////
                    pdPoint[0].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
                    pdPoint[0].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

                    pdPoint[1].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
                    pdPoint[1].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

                    pdPoint[2].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
                    pdPoint[2].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

                    pdPoint[3].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
                    pdPoint[3].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

                    RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                    pfPoint[0].X = (float)((-rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[0].X) * (dZoomRatio * m_dScale));
                    pfPoint[0].Y = (float)((-rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[0].Y) * (dZoomRatio * m_dScale));

                    pfPoint[1].X = (float)((-rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[1].X) * (dZoomRatio * m_dScale));
                    pfPoint[1].Y = (float)((-rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[1].Y) * (dZoomRatio * m_dScale));

                    pfPoint[2].X = (float)((-rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[2].X) * (dZoomRatio * m_dScale));
                    pfPoint[2].Y = (float)((-rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[2].Y) * (dZoomRatio * m_dScale));

                    pfPoint[3].X = (float)((-rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[3].X) * (dZoomRatio * m_dScale));
                    pfPoint[3].Y = (float)((-rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[3].Y) * (dZoomRatio * m_dScale));
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    bOnWafer = this.UseDie(InDie.DieCood.X - m_WaferRecipe.ORIGIN_X
                        , InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y
                        , this.m_WaferRecipe.DIE_SIZE_X
                        , this.m_WaferRecipe.DIE_SIZE_Y);

                    /// 0 Skip / 1 Probing / 2 Mark Die를 설정값에 맞춰서 Drawing한다. 
                    switch (InDie.DieProp)
                    {
                        case 0:
                            if (m_bDrawSkipDie)
                            {
                                sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colSkipDieColor);
                            }
                            else continue;
                            break;
                        case 1:
                            if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, InDie.BinNumber.ToString()) > -1)
                            {
                                try
                                {
                                    sbrshDie.Color = m_ColorSet[InDie.BinNumber];
                                    pDieBorder.Color = m_colDieBorder;
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            else
                            {
                                sbrshDie.Color = Color.FromArgb(m_iNotSelectedBinAlpha, m_ColorSet[InDie.BinNumber]);
                                pDieBorder.Color = Color.FromArgb(0, 0, 0, 0);
                            }

                            m_WaferRecipe.NETDIE++;
                            break;
                        case 2:
                            if (m_bDrawMarkDie)
                            {
                                sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colMarkDieColor);
                            }
                            else continue;
                            break;
                        case 3:
                            if (m_colPickupedDie == Color.Transparent)
                            {
                                sbrshDie.Color = Color.FromArgb(m_iPickupedDieAlpha, m_ColorSet[InDie.BinNumber]);
                            }
                            else
                            {
                                sbrshDie.Color = Color.FromArgb(255, m_colPickupedDie);
                            }
                            pDieBorder.Color = m_colDieBorder;
                            break;
                    }

                    if (InDie.DieProp > -1) g.FillPolygon(sbrshDie, pfPoint);
                    if (dZoomRatio > 0.6 && m_bVisibleDieBorder)
                    {
                        if (m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) > -1)
                        {
                            g.DrawPolygon(pSelDieBorder, pfPoint);
                        }
                        else
                        {
                            g.DrawPolygon(pDieBorder, pfPoint);
                        }
                    }


                    //////////////////////////////////////////////////////////////////////////////////////////


                    /// Origin / First Border를 설정값에 맞춰서 Drawing한다.
                    if (InDie.IndexX == m_WaferRecipe.ORIGIN_DIE_X && InDie.IndexY == m_WaferRecipe.ORIGIN_DIE_Y)
                    {
                        pfOrigin = new PointF[4];
                        Array.Copy(pfPoint, pfOrigin, 4);
                    }

                    if (InDie.IndexX == m_WaferRecipe.FIRST_DIE_X && InDie.IndexY == m_WaferRecipe.FIRST_DIE_Y)
                    {
                        pfFirst = new PointF[4];
                        Array.Copy(pfPoint, pfFirst, 4);
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////
                    ///
                    cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X) / 4;
                    cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y) / 4;
                    minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
                    maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
                    minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
                    maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));
                    cirR = Math.Min((float)(m_WaferRecipe.DIE_SIZE_X * (dZoomRatio * m_dScale)), (float)(m_WaferRecipe.DIE_SIZE_Y * (dZoomRatio * m_dScale)));

                    if (m_bVisibleVIFaile && InDie.VIFail > 0)
                    {
                        g.FillEllipse(new SolidBrush(m_ColorSet[InDie.VIFail]), cirX - cirR / 3f, cirY - cirR / 3f, cirR * (2f / 3f), cirR * (2f / 3f));
                        g.DrawEllipse(new Pen(Color.Red), cirX - cirR / 3f, cirY - cirR / 3f, cirR * (2f / 3f), cirR * (2f / 3f));
                    }

                    if (m_bVisibleDieValue && dZoomRatio > 0.8)
                    {

                        if (m_strDisplayDieValue == "BIN")
                        {
                            g.DrawString(InDie.BinNumber.ToString(), m_fntBin, new SolidBrush(Color.DarkGray), minX, minY);
                        }
                        else if (m_strDisplayDieValue == "PCMVALUE")
                        {
                            g.DrawString(InDie.ParametricValue.ToString(), m_fntPara, new SolidBrush(Util.GetIdealTextColor(sbrshDie.Color)), minX, minY);
                        }
                    }

                    /// Draw Skip Die
                    /// 
                    if (m_bDrawSkipDie && InDie.DieProp == 0)
                    {
                        g.DrawLine(pDieBorder, minX, minY, maxX, maxY);
                        g.DrawLine(pDieBorder, maxX, minY, minX, maxY);
                    }
                    /// Draw Mark Die
                    if (m_bDrawMarkDie && InDie.DieProp == 2)
                    {
                        g.FillEllipse(new SolidBrush(Color.Black), cirX - 3, cirY - 3, 3, 3);
                    }
                }

                if (m_bDrawFirstDie && pfFirst != null)
                {
                    g.DrawPolygon(pFirstDieBorder, pfFirst);
                }

                if (m_bDrawOriginDie && pfOrigin != null)
                {
                    g.DrawPolygon(pOriginDieBorder, pfOrigin);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pdPoint = null;
                pfPoint = null;

                if (pSelDieBorder != null) pSelDieBorder.Dispose();
                if (pDieBorder != null) pDieBorder.Dispose();
                if (pOriginDieBorder != null) pOriginDieBorder.Dispose();
                if (pFirstDieBorder != null) pFirstDieBorder.Dispose();
                if (sbrshDie != null) sbrshDie.Dispose();
            }
        }
        #endregion

        public bool SetDieProperty(int x, int y, int iProperty, int iBin)
        {
            if (m_bmpWaferMap == null) return false;
            if (m_gdiTempMap == null) return false;
            try
            {
                int idx = m_DieIndexer.IndexOf(new Point(x, y));
                if (idx < 0) return false;

                Die InDie = m_arrDies[idx];
                InDie.DieProp = iProperty;
                InDie.BinNumber = iBin;
                m_arrDies[idx] = InDie;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetDieProperty(int x, int y, int iProperty)
        {
            if (m_bmpWaferMap == null) return false;
            if (m_gdiTempMap == null) return false;
            try
            {
                int idx = m_DieIndexer.IndexOf(new Point(x, y));
                if (idx < 0) return false;

                Die InDie = m_arrDies[idx];
                InDie.DieProp = iProperty;
                m_arrDies[idx] = InDie;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetPickupedDie(int x, int y)
        {
            if (m_bmpWaferMap == null) return false;
            if (m_gdiTempMap == null) return false;
            try
            {

                if (SetDieProperty(x, y, 3))
                {
                    int idx = m_DieIndexer.IndexOf(new Point(x, y));
                    Die InDie = m_arrDies[idx];
                    RedrawDie(InDie, m_colPickupedDie, m_colDieBorder);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private void WaferMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //더블 클릭시 Zoom In 되는 기능 인데 다른 Event 와 겹쳐서 우선 주석 처리함.
            //if (m_eoMapMode == MapMode.Fit) return;
            //if (m_eoMouseDragMode == MouseDragMode.Zoom)
            //{
            //    m_rectSelect.X = e.X - 200;
            //    m_rectSelect.Y = e.Y - 200;
            //    m_rectSelect.Width = 400;
            //    m_rectSelect.Height = 400;
            //    DrawSelectRect();

            //    RectangleD rectdBefore = m_rectdWaferArea;
            //    double fBeforeZoomRatio = m_dZoomRatio;

            //    double fRatioWidth = (double)this.Width / m_rectSelect.Width;
            //    double fRatioHeight = (double)this.Height / m_rectSelect.Height;

            //    PointD pdCenter = new PointD((m_rectSelect.X + m_rectSelect.Width / 2) / fBeforeZoomRatio + m_rectdWaferArea.X
            //        , (m_rectSelect.Y + m_rectSelect.Height / 2) / fBeforeZoomRatio + m_rectdWaferArea.Y);

            //    m_dZoomRatio = Math.Min(fRatioHeight, fRatioWidth) * fBeforeZoomRatio;

            //    m_rectdWaferArea.Width = this.Width / m_dZoomRatio;
            //    m_rectdWaferArea.Height = this.Height / m_dZoomRatio;

            //    m_rectdWaferArea.X = pdCenter.X - m_rectdWaferArea.Width / 2;
            //    m_rectdWaferArea.Y = pdCenter.Y - m_rectdWaferArea.Height / 2;

            //    if (m_SelectPath != null) m_SelectPath.Dispose();

            //    this.Redraw();

            //}
        }

        public void AddContextMenuItem(string itemName, EventHandler handler)
        {
            ctxmWaferMap.MenuItems.Add(itemName, handler);
        }

        #region [Shot Notify] Wafer 정렬을 위한 Shot 교차점 표시 관련 로직 (2019.07.16)

        /// <summary>
        /// Wafer 정렬을 위한 왼쪽 Shot Notify Die 입니다. 기준점은 좌하단입니다.
        /// </summary>
        public Die DieOfLeftShotNotify { get; private set; }

        /// <summary>
        /// Wafer 정렬을 위한 오른쪽 Shot Notify Die 입니다. 기준점은 좌하단입니다.
        /// </summary>
        public Die DieOfRightShotNotify { get; private set; }

        /// <summary>
        /// Shot Notify 모드인지를 가져옵니다.
        /// </summary>
        public bool IsShotNotifyMode()
        {
            return !DieOfLeftShotNotify.IsEmpty() && !DieOfRightShotNotify.IsEmpty();
        }

        /// <summary>
        /// Wafer가 얼마만큼 회전되어 있는지를 나타내는 값(radian)입니다.
        /// </summary>
        public double ShotNotifyAlignAngle { get; private set; }

        private void CalcShotNotifyAlignPoint()
        {
            if (m_arrDies.Count == 0 || m_WaferRecipe.SHOT_ARRAY_X <= 0 || m_WaferRecipe.SHOT_ARRAY_Y <= 0)
                return;

            DieOfLeftShotNotify = DieOfRightShotNotify = Die.Empty;

            int xStart = m_WaferRecipe.SHOT_START_X;
            int yStart = m_WaferRecipe.SHOT_START_Y;
            int xEnd = m_WaferRecipe.SHOT_START_X;
            int yEnd = m_WaferRecipe.SHOT_START_Y;

            while (xStart > m_WaferRecipe.DIE_INDEX_MIN_X + 1)
                xStart -= m_WaferRecipe.SHOT_ARRAY_X;

            while (yStart > m_WaferRecipe.DIE_INDEX_MIN_Y + 1)
                yStart -= m_WaferRecipe.SHOT_ARRAY_Y;

            while (xEnd < m_WaferRecipe.DIE_INDEX_MAX_X)
                xEnd += m_WaferRecipe.SHOT_ARRAY_X;

            while (yEnd < m_WaferRecipe.DIE_INDEX_MAX_Y)
                yEnd += m_WaferRecipe.SHOT_ARRAY_Y;

            int refX = m_WaferRecipe.ORIGIN_DIE_Y;
            int temp = Int32.MaxValue;
            int indexY = -1;

            // y방향 가운데 인덱스 찾기
            for (int y = yStart; y <= yEnd; y += ShotArrayY)
            {
                int val = Math.Abs(y - refX - 1);

                if (temp > val)
                {
                    temp = val;
                    indexY = y;
                }
            }

            int xMin = Int32.MaxValue, xMax = Int32.MinValue;
            int indexXMin = -1, indexXMax = -1;

            // x방향 좌우 인덱스 찾기
            for (int x = xStart; x <= xEnd; x += ShotArrayX)
            {
                if (x <= m_WaferRecipe.DIE_INDEX_MAX_X)
                {
                    if (xMin > x && m_arrDies.IndexOf(x, indexY) >= 0 && m_arrDies.IndexOf(x, indexY) >= 0)
                    {
                        xMin = x;
                        indexXMin = x;
                    }

                    if (xMax < x && m_arrDies.IndexOf(x, indexY) >= 0 && m_arrDies.IndexOf(x, indexY) >= 0)
                    {
                        xMax = x;
                        indexXMax = x;
                    }
                }
            }

            if (indexY > 0 && indexXMin > 0 && indexXMax > 0)
            {
                foreach (Die d in m_arrDies)
                {
                    if (d.IndexX == indexXMin && d.IndexY == indexY)
                    {
                        DieOfLeftShotNotify = d;
                        break;
                    }
                }

                foreach (Die d in m_arrDies)
                {
                    if (d.IndexX == indexXMax && d.IndexY == indexY)
                    {
                        DieOfRightShotNotify = d;
                        break;
                    }
                }
            }

            if (DieOfLeftShotNotify.IsEmpty() || DieOfRightShotNotify.IsEmpty() || DieOfLeftShotNotify == DieOfRightShotNotify)
            {
                //throw new Exception("계산을 실패 하였습니다.");
                _visibleShotAlignPoint = false;
            }
        }

        /// <summary>
        /// Wafer 정렬을 위한 Shot 기준점 표시 위치를 나타냅니다.
        /// </summary>
        public bool VisibleShotAlignPoint
        {
            get
            {
                return _visibleShotAlignPoint;
            }

            set
            {
                _visibleShotAlignPoint = value;

                if (!_visibleShotAlignPoint)
                {
                    Redraw();
                }
                else
                {
                    CalcShotNotifyAlignPoint();
                    Redraw();
                }
            }
        }

        /// <summary>
        /// Wafer 정렬을 위한 Shot 기준점을 계산합니다.
        /// </summary>
        /// <param name="dx">두 기준점 사이의 X 거리</param>
        /// <param name="dy">두 기준점 사이의 Y 거리</param>
        public void CalcShotNotifyAlignPoint(double dx, double dy)
        {
            if (dx == 0 && dy == 0)
                ShotNotifyAlignAngle = 0;
            else
                ShotNotifyAlignAngle = Math.Atan2(dy, dx);

            // 회전한 각도만큼 회전 처리
            //Rotate(-ShotNotifyAlignAngle * 180 / Math.PI);
        }

        private int _prevDieX;
        private int _prevDieY;
        private int _passingCount;

        /// <summary>
        /// Die에 포커스를 설정합니다. 이 메서드는 Wafer가 얼마나 회전하였는지를 나타내는 ShotNotifyAlignAngle 값을 고려하여 Die에 포커스를 설정합니다.
        /// </summary>
        /// <param name="dx">DieOfLeftShotNotify Die를 0으로 한 X 값</param>
        /// <param name="dy">DieOfLeftShotNotify Die를 0으로 한 Y 값</param>
        public void SetFocusDieByShotNotify(double dx, double dy)
        {
            if (_passingCount < FocusDiePassingCount)
            {
                _passingCount++;
                return;
            }
            else
            {
                _passingCount = 0;
            }

            if (IsShotNotifyMode() && ShotNotifyAlignAngle != 0)
            {
                double theta = Math.Atan2(dy, dx);
                double r = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                dx = r * Math.Cos(theta - ShotNotifyAlignAngle);
                dy = r * Math.Sin(theta - ShotNotifyAlignAngle);
            }

            int x = -1;
            int y = -1;

            if (dx > 0)
                x = DieOfLeftShotNotify.IndexX + (int)(dx / m_WaferRecipe.DIE_SIZE_X);
            else
                x = DieOfLeftShotNotify.IndexX + (int)(dx / m_WaferRecipe.DIE_SIZE_X) - 1;

            if (dy > 0)
                y = DieOfLeftShotNotify.IndexY + (int)(dy / m_WaferRecipe.DIE_SIZE_Y);
            else
                y = DieOfLeftShotNotify.IndexY + (int)(dy / m_WaferRecipe.DIE_SIZE_Y) - 1;

            //x = DieOfLeftShotNotify.IndexX + (int)(dx / m_WaferRecipe.DIE_SIZE_X);
            //y = DieOfLeftShotNotify.IndexY + (int)(dy / m_WaferRecipe.DIE_SIZE_Y);

            // 같은 Die인 경우 포커스를 업데이트 하지 않는다.
            //if (_prevDieX == x && _prevDieY == y)
            //    return;

            _prevDieX = x;
            _prevDieY = y;

            SetFocusDie(x, y);
        }

        private void menuItemShotOption_Click(object sender, EventArgs e)
        {
            using (WaferMapShotOption frm = new WaferMapShotOption())
            {
                frm.Map = this;
                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                OnShotOptionChanged(EventArgs.Empty);

                if (ShotArrayX > 0 && ShotArrayY > 0 && !mnuitemVISIBLE_SHOT.Checked)
                    mnuitemVISIBLE_SHOT.PerformClick();

                Redraw();
            }
        }

        /// <summary>
        /// SetFocusDieByShotNotify() 메서드 호출 시 데이터를 처리하지 않고 Passing 할 수량을 설정합니다.<para/>
        /// 예를 들어 0인 경우 모든 데이터를 처리하고, 10인 경우 10개 데이터는 처리하지 않고 11번째 데이터는 처리합니다.<para/>
        /// </summary>
        [DefaultValue(0)]
        public int FocusDiePassingCount
        {
            get;
            set;
        }

        #endregion

        public Bitmap Bitmap
        {
            get
            {
                m_gdiMain.DrawImageUnscaled(m_bmpWaferMap, 0, 0);
                return m_bmpWaferMap;
            }
        }

        public void SetZoneSelect(ZoneSelectMode mode)
        {
            if (mode == ZoneSelectMode.Rectangle)
                mnuitemMAPSELECTSTYLE_RECTANGLE.PerformClick();
            else if (mode == ZoneSelectMode.Circle)
                mnuitemMAPSELECTSTYLE_CIRCLE.PerformClick();
            else if (mode == ZoneSelectMode.Pie)
                mnuitemMAPSELECTSTYLE_PIE.PerformClick();
            else if (mode == ZoneSelectMode.FreeHand)
                mnuitemMAPSELECTSTYLE_FREEHAND.PerformClick();
            else if (mode == ZoneSelectMode.Band)
                mnuitemMAPSELECTSTYLE_BAND.PerformClick();
            else
                mnuitemMAPSELECTSTYLE_RECTANGLE.PerformClick();
        }
    }

    public enum ZoneSelectMode
    {
        Rectangle,
        Circle,
        Pie,
        FreeHand,
        Band
    }
}
