using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using System.Drawing.Drawing2D;
using System.Collections;

namespace DACrux.DMSVFPD.Map
{
    public enum MapSelectionMode { QMode, ALL };
    public enum MapMode { Fit, Free };
    public enum MouseDragMode { Zoom, Move, Rotate, Zone, Shot, Defect };
    public enum MapSelectStyle { FreeHand, Circle, Band, Pie, Rectangle };
    /// <summary>Map이 화면에 채워질 모드를 나타냅니다.</summary>
    public enum MapFitMode {
        /// <summary>Map이 가로,세로 비율을 유지하며 컨트롤 크기에 맞게 확대 또는 축소되어 보여집니다.</summary>
        DisplayAll,
        /// <summary>Map 전체가 컨트롤에 꽉 차게 보여집니다.</summary>
        Expand,
        /// <summary>가로, 세로 길이 중 작은 길이 기준으로 화면에 보여줍니다.</summary>
        MinimumLength };



    [Flags]
    public enum DisplayFlag
    {
        NONE = 0x00,
        LOTID = 0x01,
        SLOT = 0x02,
        PANEL_BIN = 0x04,
        QPANEL_BIN = 0x08,
        QPANEL_ALIAS = 0x10,
        PANEL_ALIAS = 0x20,
        PCM = 0x40
    };

    public delegate void SelectFPDPanels(object sender, List<CPanel> SelFPDPanels);


    /// <summary>
    /// Mouse의 현재 위치한 FPDPanel가 이전 FPDPanel의 Index와 바뀌었을때 호출되는 Event의 대리자
    /// </summary>
    public delegate void ChangeCurrentFPDPanel(object sender, DACrux.Base.CPanel NewFPDPanel);
    /// <summary>
    /// Mouse의 Real Position과 절대 Position을 전달하는 Event의 대리자
    /// </summary>
    public delegate void ChangePosition(object sender, PointD Currpoint, PointD RealPoint);


    /// <summary>
    /// 생성날자 : 2009-06-24
    /// 개 발 자 : YS LIM
    /// 소스버전 : V1.0.0
    /// 내    용 : 
    ///         <2009-06-24>
    ///         1. MMG (Multi-Model Glass) 적용 Map
    ///         2. Original Point는 Glass 중심
    ///         3. Glass >> Q-Panel >> Panel Layout
    /// 
    /// </summary>
    /// 

    public partial class MMGBinMap : UserControl
    {
        public event SelectFPDPanels OnSelectFPDPanels;
        //public event ChangeCurrentFPDPanel OnChangeCurrentFPDPanel;
        //public event ChangePosition OnChangePosition;

        /// <summary>
        /// Map이 확대 되거나 축소되는 등의 View가 변경되는 경우 발생하는 이벤트 입니다.
        /// </summary>
        public event EventHandler ViewChanged;

        protected DelayTimer _timer = new DelayTimer();

        /// <summary>
        /// m_dScale : Glass Drawing 계산이 끝난 후 화면에 Drawing하는 비율
        /// </summary>
        protected double m_dZoomRatio = 1.0d;

        protected int m_iViewAngle = 0;
        protected int m_iAngleOffset = 0;
        protected double m_dScale = 0.97d;

        protected Rectangle m_rectSelect = Rectangle.Empty;
        protected Rectangle m_rectSelect2 = Rectangle.Empty;

        protected Point m_poStart = Point.Empty;
        protected Point m_poEnd = Point.Empty;
        protected Bitmap m_bmpGlassMap;
        protected Bitmap m_bmpTemp;
        protected Bitmap m_bmpGlassID;
        protected RectangleD m_rectdGlassArea;

        protected MapSelectionMode m_eoMapSelectionMode = MapSelectionMode.ALL;
        protected MapMode m_eoMapMode = MapMode.Free;
        protected MouseDragMode m_eoMouseDragMode = MouseDragMode.Zoom;
        protected MapSelectStyle m_eoMapSelectStyle = MapSelectStyle.Circle;

        protected GraphicsPath m_SelectPath = null;

        protected Color[] m_ColorSet;


        protected DisplayFlag m_eoDisplayValues = DisplayFlag.PANEL_BIN | DisplayFlag.LOTID | DisplayFlag.QPANEL_ALIAS | DisplayFlag.PANEL_ALIAS;
        protected bool m_bVisibleFPDPanelValue = false;
        protected Color m_colFPDPanelBorder = Color.LightGray;

        protected List<CPanel> m_SelectedPanels = null;
        protected List<CQPanel> m_SelectedQPanels = null;
        protected bool m_bVisibleQPanelValue = false;
        protected Color m_colQPanelBorder = Color.Gray;

        protected Graphics m_gdiMain = null;
        protected Graphics m_gdiTempMap = null;

        protected Position m_poInformation = Position.LeftTop;
        protected bool m_bVisibleInfo = true;
        protected List<string> m_strInfomation = new List<string>();

        #region Glass속성 Member변수
        // Draw속성
        protected Color m_colGlass = Color.Gray;
        protected Color m_colGlassBorder = Color.Black;
        protected Color m_colEdge = Color.Black;
        protected bool m_bCenterMark = false;
        protected bool m_bScale = false;
        protected bool m_bVisibleSelectedModel = false;
        #endregion

        protected CGlass m_GlassRecipe = null;
        protected HatchBrush m_hbSelectBrush = null;
        protected DataTable m_DT = null;

        protected string[] m_strSelectedBin = new string[] { "ALL" };
        public int m_iNotSelectedBinAlpha = 20;

        protected int m_iCurrentX = -1;
        protected int m_iCurrentY = -1;
        protected int m_iCurrentFPDPanelIndex = -1;

        protected Font m_fntBin = new Font("Tahoma", 6);
        protected Font m_fntAlias = new Font("Tahoma", 10); // Glass Setup 작업중 추가 by 양형석 ('2009.07.20)
        protected Font m_fntLabel = new Font("Tahoma", 8); // Fail Map Analysis 작업중 추가 by 양형석 ('2009.08.11)
        protected Font m_fntPara = new Font("Tahoma", 9);
        protected System.Windows.Forms.MenuItem mnuCOPYTOCLIP;

        protected RectangleF m_rectNotchArea = RectangleF.Empty;
        protected RectangleF m_rectOCRIDArea = RectangleF.Empty;

        protected bool m_bPopupMenu = true;
        protected bool m_bVisibleFPDPanelBorder = true;
        protected bool m_bVisibleQPanelBorder = true;

        public MMGBinMap()
        {
            m_GlassRecipe = new CGlass();
            InitializeComponent();
            SetDefaultColor();
            Reset();

            menuItem1.Visible = menuItem2.Visible = false;
            _timer.Tick += DelayTimer_Tick;
            _timer.Owner = this;
        }
        
        private void SetDelayViewChanged()
        {
            _timer.Set();
        }

        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            OnViewChanged(e);
        }

        protected virtual void OnViewChanged(EventArgs e)
        {
            if (ViewChanged != null)
                ViewChanged(this, e);
        }

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

        public bool VisibleFPDPanelValue
        {
            set
            {
                m_bVisibleFPDPanelValue = value;
                this.Redraw();
            }
            get
            {
                return m_bVisibleFPDPanelValue;
            }
        }


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

        public int SelectedFPDPanelsCount
        {
            get { return m_SelectedPanels.Count; }
        }

        public void SetGlass(CGlass oClass)
        {
            m_GlassRecipe = oClass;
        }

        public void SetSelectedFPDPanel(int x, int y)
        {
            foreach (CQPanel InQPanel in m_GlassRecipe.QPanels)
            {
                if (InQPanel.QPNL_INDEX_X == x && InQPanel.QPNL_INDEX_Y == y)
                {
                    m_SelectedQPanels.Add(InQPanel);
                    break;
                }
            }
        }

        public void ResetSelectedFPDPanel()
        {
            m_SelectedPanels.Clear();
            this.Redraw();
        }

        public void Copy(MMGBinMap TagetMap)
        {
            if (TagetMap == null) TagetMap = new MMGBinMap();

            TagetMap.m_GlassRecipe = this.m_GlassRecipe;
            //TagetMap.DataSource = this.DataSource;
            TagetMap.m_strInfomation = this.m_strInfomation;
            TagetMap.m_ColorSet = this.m_ColorSet;
            TagetMap.Redraw();
        }

        public DisplayFlag DisplayValue
        {
            set
            {
                m_eoDisplayValues = value;
            }
            get
            {
                return m_eoDisplayValues;
            }
        }

        public List<CPanel> SelectedFPDPanels
        {
            get { return m_SelectedPanels; }
        }

        #region ◈ 소멸자 , Dispose
        ~MMGBinMap()
        {

            if (m_hbSelectBrush != null) m_hbSelectBrush.Dispose();

            if (m_gdiMain != null) m_gdiMain.Dispose();
            if (m_gdiTempMap != null) m_gdiTempMap.Dispose();
            if (m_bmpGlassID != null) m_bmpGlassID.Dispose();
            if (m_bmpTemp != null) m_bmpTemp.Dispose();
            if (m_bmpGlassMap != null) m_bmpGlassMap.Dispose();
            if (m_DT != null) m_DT.Dispose();
            m_strInfomation = null;
            m_ColorSet = null;
            GC.Collect();
        }


        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {

                    if (m_hbSelectBrush != null) m_hbSelectBrush.Dispose();

                    if (m_gdiMain != null) m_gdiMain.Dispose();
                    if (m_gdiTempMap != null) m_gdiTempMap.Dispose();
                    if (m_bmpTemp != null) m_bmpTemp.Dispose();
                    if (m_bmpGlassMap != null) m_bmpGlassMap.Dispose();
                    if (ctxmGlassMap != null) ctxmGlassMap.Dispose();
                    if (m_DT != null) m_DT.Dispose();
                    m_ColorSet = null;

                    components.Dispose();
                    GC.Collect();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #region ◈ Glass Reset
        /// <summary>
        /// Glass Map을 초기화 합니다. Recipe가 초기값으로 임의설정됩니다.
        /// </summary>
        public virtual void Reset()
        {
            /// Data가 존재 했을경우 초기화//////////////////////////////////////////////////
            m_GlassRecipe = null;
            if (m_hbSelectBrush != null) m_hbSelectBrush.Dispose();
            //////////////////////////////////////////////////////////////////////////////

            m_GlassRecipe = new CGlass(1000.0d, 1000.0d, false);
            m_GlassRecipe.NOTCH_SIZE = 2.0d;

            double dMinCanvers_x = 0d;
            double dMinCanvers_y = 0d;

            if ((m_GlassRecipe.GLASS_X / this.Width) > (m_GlassRecipe.GLASS_Y / this.Height))
            {
                dMinCanvers_x = this.Width * m_dScale;
                m_dZoomRatio = dMinCanvers_x / m_GlassRecipe.GLASS_X;
                dMinCanvers_y = (float)(m_GlassRecipe.GLASS_Y * m_dZoomRatio);
            }
            else
            {
                dMinCanvers_y = this.Height * m_dScale;
                m_dZoomRatio = dMinCanvers_y / m_GlassRecipe.GLASS_Y;
                dMinCanvers_x = (float)(m_GlassRecipe.GLASS_X * m_dZoomRatio);
            }

            m_rectdGlassArea.X = ((dMinCanvers_x - this.Width) / 2.0f) / (m_dZoomRatio * m_dScale);
            m_rectdGlassArea.Y = ((dMinCanvers_y - this.Height) / 2.0f) / (m_dZoomRatio * m_dScale);
            m_rectdGlassArea.Width = this.Width / m_dZoomRatio;
            m_rectdGlassArea.Height = this.Height / m_dZoomRatio;

            m_SelectedPanels = new List<CPanel>();
            m_SelectedQPanels = new List<CQPanel>();

            m_hbSelectBrush = new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.White, Color.PowderBlue);

            SetDelayViewChanged(); //OnViewChanged(EventArgs.Empty);
        }
        #endregion



        #region ◈ FPDPanel 속성

        [Category("Glass 속성"), Description("FPDPanel의 Border Color를 설정하거나 가져옵니다.")]
        public Color FPDPanelBorderColor
        {
            set { m_colFPDPanelBorder = value; }
            get { return m_colFPDPanelBorder; }
        }

        [Category("Glass 속성"), Description("FPDPanel의 Border를 표시할지 여부를 설정하거나 가져옵니다.")]
        public bool VisibleFPDPanelBorder
        {
            set { m_bVisibleFPDPanelBorder = value; }
            get { return m_bVisibleFPDPanelBorder; }
        }

        [Category("Glass 속성"), Description("Glass에 존재한는 FPDPanel의 X축 Count를 설정하거나 가져옵니다.")]
        public int XFPDPanels
        {
            get { return m_GlassRecipe.QPNL_X_CNT; }
        }

        [Category("Glass 속성"), Description("Glass에 존재한는 FPDPanel의 Y축 Count를 설정하거나 가져옵니다.")]
        public int YFPDPanels
        {
            get { return m_GlassRecipe.QPNL_Y_CNT; }
        }

        #endregion

        #region ◈ Glass 속성
        [Category("Glass 속성"), Description("Glass의 Background Color를 설정하거나 가져옵니다.")]
        public Color GlassColor
        {
            set { m_colGlass = value; }
            get { return m_colGlass; }
        }

        [Category("Glass 속성"), Description("Glass의 Border Color를 설정하거나 가져옵니다.")]
        public Color GlassBorderColor
        {
            set { m_colGlassBorder = value; }
            get { return m_colGlassBorder; }
        }

        [Category("Glass 속성"), Description("Edge의 Background Color를 설정하거나 가져옵니다.")]
        public Color EdgeColor
        {
            set { m_colEdge = value; }
            get { return m_colEdge; }
        }

        [Category("Glass 속성"), Description("Net FPDPanel의 개수를 가져옵니다.")]
        public int NetFPDPanel
        {
            get { return m_GlassRecipe.QPNL_COUNT; }
        }

        #endregion

        #region ◈ Option 속성


        [Category("Glass Option"), Description("Glass Infomation의 표시 여부를 설정하거나 가져옵니다.")]
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

        [Category("Glass Option"), Description("Visual Inspection Code의 Display여부를 설정하거나 가져옵니다.")]
        public bool VisibleSelectedModel
        {
            set
            {
                m_bVisibleSelectedModel = value;
            }
            get
            {
                return m_bVisibleSelectedModel;
            }
        }

        [Category("Glass Option"), Description("Center Guide 표현 여부를 설정하거나 가져옵니다.")]
        public bool CenterMark
        {
            set
            {
                m_bCenterMark = value;
                if (m_gdiMain == null || m_bmpGlassMap == null) return;
                DrawCenterGrid();
            }
            get { return m_bCenterMark; }
        }

        [Category("Glass Option"), Description("FPDPanel X, Y의 좌표 눈금 표현 여부를 설정하거나 가져옵니다.")]
        public bool ScaleMark
        {
            set
            {
                m_bScale = value;
                if (m_gdiMain != null && m_bmpGlassMap != null) this.Redraw();
            }
            get { return m_bScale; }
        }

        [Category("Glass Option"), Description("Glass의 Drawing Mode을 설정하거나 가져옵니다.")]
        public virtual MapMode GlassDrawMode
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

        [Category("Glass Option"), Description("Glass의 Access Mode을 설정하거나 가져옵니다.")]
        public virtual MapSelectionMode SelectionMode
        {
            set { m_eoMapSelectionMode = value; }
            get { return m_eoMapSelectionMode; }
        }

        [Category("Glass Option"), Description("Draw 대상 Bin을 설정하거나 가져옵니다.")]
        public virtual string SelecetedBin
        {
            set
            {
                m_strSelectedBin = value.Split(',');
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(FadeIn));
                t.IsBackground = true;
                t.Start();
            }
            get
            {
                return string.Join(",", m_strSelectedBin);
            }
        }

        private void FadeIn()
        {
            if (m_GlassRecipe != null && m_GlassRecipe.QPanels != null && m_GlassRecipe.QPanels.Count > 0)
            {
                for (int i = 10; i > 0; i--)
                {
                    m_iNotSelectedBinAlpha = i * 255 / 10;

                    this.Redraw();
                }
            }
        }

        #endregion

        #region ■ Zoom 관련
        public void ZoomIn()
        {
            if (m_eoMapMode == MapMode.Fit) return;
            m_dZoomRatio = m_dZoomRatio * 1.05d;
            RectangleD rectdBefore = m_rectdGlassArea;
            PointD pdBeforeCenter = new PointD(m_rectdGlassArea.X + m_rectdGlassArea.Width / 2, m_rectdGlassArea.Y + m_rectdGlassArea.Height / 2);

            m_rectdGlassArea.Width = this.Width / m_dZoomRatio;
            m_rectdGlassArea.Height = this.Height / m_dZoomRatio;
            m_rectdGlassArea.X = pdBeforeCenter.X - m_rectdGlassArea.Width / 2;
            m_rectdGlassArea.Y = pdBeforeCenter.Y - m_rectdGlassArea.Height / 2;
            Redraw();
            SetDelayViewChanged();
        }

        public void ZoomOut()
        {
            if (m_eoMapMode == MapMode.Fit) return;
            m_dZoomRatio = m_dZoomRatio * 0.95f;
            RectangleD rectdBefore = m_rectdGlassArea;
            PointD pdBeforeCenter = new PointD(m_rectdGlassArea.X + m_rectdGlassArea.Width / 2, m_rectdGlassArea.Y + m_rectdGlassArea.Height / 2);

            m_rectdGlassArea.Width = this.Width / m_dZoomRatio;
            m_rectdGlassArea.Height = this.Height / m_dZoomRatio;
            m_rectdGlassArea.X = pdBeforeCenter.X - m_rectdGlassArea.Width / 2;
            m_rectdGlassArea.Y = pdBeforeCenter.Y - m_rectdGlassArea.Height / 2;
            Redraw();
            SetDelayViewChanged();
        }
        #endregion

        #region ▣ Mouse Up/Down/Move Event
        private void GlassMap_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
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
                if (m_bPopupMenu) ctxmGlassMap.Show(this, new Point(e.X, e.Y));
            }
        }



        private void GlassMap_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
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
        }

        private void GlassMap_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (m_eoMouseDragMode)
                {
                    case MouseDragMode.Zoom:
                        if (m_eoMapMode == MapMode.Fit) break;
                        if (m_rectSelect.Width * m_rectSelect.Height == 0) return;

                        RectangleD rectdBefore = m_rectdGlassArea;
                        double fBeforeZoomRatio = m_dZoomRatio;

                        double fRatioWidth = (double)this.Width / m_rectSelect.Width;
                        double fRatioHeight = (double)this.Height / m_rectSelect.Height;

                        PointD pdCenter = new PointD((m_rectSelect.X + m_rectSelect.Width / 2) / fBeforeZoomRatio + m_rectdGlassArea.X
                            , (m_rectSelect.Y + m_rectSelect.Height / 2) / fBeforeZoomRatio + m_rectdGlassArea.Y);

                        m_dZoomRatio = Math.Min(fRatioHeight, fRatioWidth) * fBeforeZoomRatio;

                        m_rectdGlassArea.Width = this.Width / m_dZoomRatio;
                        m_rectdGlassArea.Height = this.Height / m_dZoomRatio;

                        m_rectdGlassArea.X = pdCenter.X - m_rectdGlassArea.Width / 2;
                        m_rectdGlassArea.Y = pdCenter.Y - m_rectdGlassArea.Height / 2;
                        break;
                    case MouseDragMode.Move:
                        if (m_eoMapMode == MapMode.Fit) break;
                        m_rectdGlassArea.X = m_rectdGlassArea.X - (m_poEnd.X - m_poStart.X) / m_dZoomRatio;
                        m_rectdGlassArea.Y = m_rectdGlassArea.Y - (m_poEnd.Y - m_poStart.Y) / m_dZoomRatio;
                        break;
                    case MouseDragMode.Rotate:
                        DrawRotate();
                        break;
                    case MouseDragMode.Zone:
                        CalSelectedFPDPanel();
                        break;
                }

                if (m_SelectPath != null) m_SelectPath.Dispose();

                this.Redraw();
                SetDelayViewChanged();
            }
        }
        #endregion

        #region ■ Real Position 계산 관련 (Rotate,GetRealPoint,GetViewPoint)
        public void Rotate(double Angle)
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

        protected PointD GetRealPoint(int piX, int piY)
        {
            PointD pdCenter;
            PointF pfGlassCenterToPixel = GetViewPoint(0.0d, 0.0d);
            PointD pdBefor;

            pdBefor.X = (double)(pfGlassCenterToPixel.X - piX);
            pdBefor.Y = (double)(pfGlassCenterToPixel.Y - piY);

            RotatePoint(ref pdBefor.X, ref pdBefor.Y, m_iViewAngle);
            piX = (int)(pdBefor.X + pfGlassCenterToPixel.X);
            piY = (int)(pdBefor.Y + pfGlassCenterToPixel.Y);

            pdCenter.X = m_GlassRecipe.GLASS_X / 2 - (m_rectdGlassArea.X + piX / (m_dZoomRatio * m_dScale));
            pdCenter.Y = -m_GlassRecipe.GLASS_Y / 2 + (m_rectdGlassArea.Y + piY / (m_dZoomRatio * m_dScale));
            return pdCenter;
        }

        private PointF GetViewPoint(double dX, double dY)
        {
            PointF pfCenter = PointF.Empty;
            pfCenter.X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + dX) * (m_dZoomRatio * m_dScale));
            pfCenter.Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - dY) * (m_dZoomRatio * m_dScale));
            return pfCenter;
        }
        #endregion

        #region ■ Edit 및 선택 영역 표시 함수[ DrawSelectRect() / DrawMoveRect() ]
        protected void DrawSelectRect()
        {
            if (m_bmpGlassMap == null) return;

            Graphics gdiTemp = null;
            Pen penPath = null;

            try
            {
                gdiTemp = Graphics.FromImage(m_bmpTemp);
                gdiTemp.DrawImageUnscaled(m_bmpGlassMap, 0, 0);

                if (m_rectSelect.Width * m_rectSelect.Height > 0)
                {
                    penPath = new Pen(m_hbSelectBrush, 3);
                    gdiTemp.DrawRectangle(penPath, m_rectSelect.X, m_rectSelect.Y, m_rectSelect.Width - 1, m_rectSelect.Height - 1);
                }
                m_gdiMain.DrawImageUnscaled(m_bmpTemp, 0, 0);
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
                gdiTemp.DrawImageUnscaled(m_bmpGlassMap, 0, 0);

                m_rectSelect = new Rectangle(0, 0, this.Width, this.Height);
                m_rectSelect.Offset(m_poEnd.X - m_poStart.X, m_poEnd.Y - m_poStart.Y);


                if (m_rectSelect.Width * m_rectSelect.Height > 0)
                {
                    penPath = new Pen(m_hbSelectBrush, 3);
                    gdiTemp.DrawRectangle(penPath, m_rectSelect);
                }
                m_gdiMain.DrawImageUnscaled(m_bmpTemp, 0, 0);
            }
            finally
            {
                if (gdiTemp != null) gdiTemp.Dispose();
                if (penPath != null) penPath.Dispose();
            }
        }

        #endregion

        #region ■ Zone 선택 모양을 그리는 함수
        private void DrawZoneGuid()
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
                gdiTemp.DrawImageUnscaled(m_bmpGlassMap, 0, 0);
                penPath = new Pen(m_hbSelectBrush, 3);

                if (m_eoMapSelectStyle == MapSelectStyle.FreeHand)
                {
                    m_SelectPath.AddLine(m_poStart, m_poEnd);
                    m_poStart = m_poEnd;

                    gdiTemp.DrawPath(penPath, m_SelectPath);

                    m_gdiMain.DrawImageUnscaled(m_bmpTemp, 0, 0);
                    return;
                }


                m_SelectPath.Reset();
                switch (m_eoMapSelectStyle)
                {
                    case MapSelectStyle.Band:
                        pdStart = GetRealPoint(m_poStart.X, m_poStart.Y);
                        dStartValue = Math.Sqrt(Math.Pow(pdStart.X, 2) + Math.Pow(pdStart.Y, 2)) * 2;

                        m_rectSelect.X = (int)((-m_rectdGlassArea.X - dStartValue / 2 + m_GlassRecipe.GLASS_X / 2) * (m_dZoomRatio * m_dScale));
                        m_rectSelect.Y = (int)((-m_rectdGlassArea.Y - dStartValue / 2 + m_GlassRecipe.GLASS_Y / 2) * (m_dZoomRatio * m_dScale));
                        m_rectSelect.Width = (int)(dStartValue * (m_dZoomRatio * m_dScale));
                        m_rectSelect.Height = (int)(dStartValue * (m_dZoomRatio * m_dScale));

                        pdEnd = GetRealPoint(m_poEnd.X, m_poEnd.Y);
                        dEndValue = Math.Sqrt(Math.Pow(pdEnd.X, 2) + Math.Pow(pdEnd.Y, 2)) * 2;

                        m_rectSelect2.X = (int)((-m_rectdGlassArea.X - dEndValue / 2 + m_GlassRecipe.GLASS_X / 2) * (m_dZoomRatio * m_dScale));
                        m_rectSelect2.Y = (int)((-m_rectdGlassArea.Y - dEndValue / 2 + m_GlassRecipe.GLASS_Y / 2) * (m_dZoomRatio * m_dScale));
                        m_rectSelect2.Width = (int)(dEndValue * (m_dZoomRatio * m_dScale));
                        m_rectSelect2.Height = (int)(dEndValue * (m_dZoomRatio * m_dScale));

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


                        m_rectSelect.X = (int)(-m_rectdGlassArea.X * (m_dZoomRatio * m_dScale));
                        m_rectSelect.Y = (int)(-m_rectdGlassArea.Y * (m_dZoomRatio * m_dScale));
                        m_rectSelect.Width = (int)(m_GlassRecipe.GLASS_X * (m_dZoomRatio * m_dScale));
                        m_rectSelect.Height = (int)(m_GlassRecipe.GLASS_Y * (m_dZoomRatio * m_dScale));
                        m_SelectPath.AddPie(m_rectSelect, (float)dStartValue, (float)((dEndValue - dStartValue + 720) % 360));
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
            finally
            {
                if (gdiTemp != null) gdiTemp.Dispose();
                if (penPath != null) penPath.Dispose();
                if (sbrshPath != null) sbrshPath.Dispose();
            }
        }
        #endregion

        #region ▣ FPDPanel선택시 선택된 FPDPanel들 계산하여 Event 발생
        private void CalSelectedFPDPanel()
        {
            PointD pdFPDPanelCenter;
            if (m_eoMapSelectStyle == MapSelectStyle.FreeHand) m_SelectPath.CloseFigure();

            foreach (CQPanel InQPanel in m_GlassRecipe.QPanels)
            {
                foreach (CPanel InFPDPanel in InQPanel.Panels)
                {
                    pdFPDPanelCenter.X = (InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X - m_GlassRecipe.ORIGIN_X) + InFPDPanel.PITCH_X / 2;
                    pdFPDPanelCenter.Y = (InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y - m_GlassRecipe.ORIGIN_Y) + InFPDPanel.PITCH_Y / 2;

                    RotatePoint(ref pdFPDPanelCenter.X, ref pdFPDPanelCenter.Y, m_iViewAngle);

                    PointF pfPoint = PointF.Empty;
                    pfPoint.X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdFPDPanelCenter.X) * (m_dZoomRatio * m_dScale));
                    pfPoint.Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdFPDPanelCenter.Y) * (m_dZoomRatio * m_dScale));

                    if (m_SelectPath.IsVisible(pfPoint))
                    {
                        if (m_SelectedPanels.IndexOf(InFPDPanel) < 0)
                            m_SelectedPanels.Add(InFPDPanel);
                    }
                }
            }

            if (OnSelectFPDPanels != null) OnSelectFPDPanels(this, m_SelectedPanels);
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

        #region ■ Draw Glass
        /// <summary>
        /// 1. Glass 외곽 Line을 Draw 한다.
        /// 2. GDI Object를 Reset한다.
        /// 3. Notch를 Drawing한다.
        /// </summary>
        protected virtual void DrawGlass()
        {
            if (m_GlassRecipe == null) return;
            if (this.Width * this.Height == 0 || m_gdiTempMap == null) return;

            GraphicsPath gpGlass = null;
            GraphicsPath gpNotch = null;
            m_gdiTempMap.Clear(Color.White);
            SolidBrush sbrshEdge = null;
            SolidBrush sbrshGlass = null;
            Pen pEdge = null;
            Pen pGlass = null;
            Font fntWID = new Font("Tahoma", Math.Max(8, (float)((m_dZoomRatio * m_dScale) * 2.3f)));
            m_fntBin = new Font("Tahoma", Math.Max(5, (float)((m_dZoomRatio * m_dScale) * 1.5f)));
            int iRealAngle = (m_GlassRecipe.ANGLE + m_iAngleOffset) % 360;

            PointD[] pdGlass = new PointD[5];
            PointD[] pdNotch = new PointD[5];
            try
            {
                gpGlass = new GraphicsPath();
                gpNotch = new GraphicsPath();

                /// Left-Bottom : Notch
                pdGlass[0].X = -m_GlassRecipe.GLASS_X / 2d;
                pdGlass[0].Y = -m_GlassRecipe.GLASS_Y / 2d + m_GlassRecipe.NOTCH_SIZE;
                pdGlass[1].X = -m_GlassRecipe.GLASS_X / 2d + m_GlassRecipe.NOTCH_SIZE;
                pdGlass[1].Y = -m_GlassRecipe.GLASS_Y / 2d;

                /// Notch Display가 작아서 별도의 Display를 함
                /// 
                pdNotch[0].X = pdGlass[0].X - 5 - (m_GlassRecipe.GLASS_X * 0.01d);
                pdNotch[0].Y = pdGlass[0].Y + (m_GlassRecipe.GLASS_X * 0.01d);
                pdNotch[1].X = pdGlass[1].X + (m_GlassRecipe.GLASS_X * 0.01d);
                pdNotch[1].Y = pdGlass[1].Y - 5 - (m_GlassRecipe.GLASS_X * 0.01d);
                pdNotch[2].X = pdGlass[1].X - 5;
                pdNotch[2].Y = pdGlass[0].Y - 5;
                pdNotch[3].X = pdNotch[2].X - (m_GlassRecipe.GLASS_X * 0.005d);
                pdNotch[3].Y = pdNotch[2].Y;
                pdNotch[4].X = pdNotch[2].X;
                pdNotch[4].Y = pdNotch[2].Y - (m_GlassRecipe.GLASS_X * 0.005d);


                /// Right-Bottom
                pdGlass[2].X = m_GlassRecipe.GLASS_X / 2d;
                pdGlass[2].Y = -m_GlassRecipe.GLASS_Y / 2d;

                /// Right-Top
                pdGlass[3].X = m_GlassRecipe.GLASS_X / 2d;
                pdGlass[3].Y = m_GlassRecipe.GLASS_Y / 2d;

                /// Right-Top
                pdGlass[4].X = -m_GlassRecipe.GLASS_X / 2d;
                pdGlass[4].Y = m_GlassRecipe.GLASS_Y / 2d;

                PointF[] dfGlass = new PointF[5];
                for (int i = 0; i < 5; i++)
                {
                    RotatePoint(ref pdGlass[i].X, ref pdGlass[i].Y, (iRealAngle + m_iViewAngle) % 360);
                    dfGlass[i].X = (float)((-m_rectdGlassArea.X + (m_GlassRecipe.GLASS_X / 2) + pdGlass[i].X) * (m_dZoomRatio * m_dScale));
                    dfGlass[i].Y = (float)((-m_rectdGlassArea.Y + (m_GlassRecipe.GLASS_Y / 2) - pdGlass[i].Y) * (m_dZoomRatio * m_dScale));
                }

                gpGlass.AddLines(dfGlass);
                gpGlass.CloseFigure();

                sbrshGlass = new SolidBrush(m_colGlass);
                pGlass = new Pen(m_colGlassBorder);
                m_gdiTempMap.FillPath(sbrshGlass, gpGlass);
                m_gdiTempMap.DrawPath(pGlass, gpGlass);

                PointF[] dfNotch = new PointF[2];
                for (int i = 0; i < 2; i++)
                {
                    RotatePoint(ref pdNotch[i].X, ref pdNotch[i].Y, (iRealAngle + m_iViewAngle) % 360);
                    dfNotch[i].X = (float)((-m_rectdGlassArea.X + (m_GlassRecipe.GLASS_X / 2) + pdNotch[i].X) * (m_dZoomRatio * m_dScale));
                    dfNotch[i].Y = (float)((-m_rectdGlassArea.Y + (m_GlassRecipe.GLASS_Y / 2) - pdNotch[i].Y) * (m_dZoomRatio * m_dScale));
                }

                gpNotch.AddLines(dfNotch);
                gpNotch.CloseFigure();


                PointF[] dfNotch2 = new PointF[3];
                for (int i = 0; i < 3; i++)
                {
                    RotatePoint(ref pdNotch[i + 2].X, ref pdNotch[i + 2].Y, (iRealAngle + m_iViewAngle) % 360);
                    dfNotch2[i].X = (float)((-m_rectdGlassArea.X + (m_GlassRecipe.GLASS_X / 2) + pdNotch[i + 2].X) * (m_dZoomRatio * m_dScale));
                    dfNotch2[i].Y = (float)((-m_rectdGlassArea.Y + (m_GlassRecipe.GLASS_Y / 2) - pdNotch[i + 2].Y) * (m_dZoomRatio * m_dScale));
                }

                gpNotch.AddLines(dfNotch2);
                gpNotch.CloseFigure();

                if (m_GlassRecipe.NOTCH_SIZE != 0) // Fail Map Analysis 작업중 추가 by 양형석 ('2009.08.11)
                {
                    sbrshGlass = new SolidBrush(Color.Red);
                    pGlass = new Pen(Color.Red);
                    m_gdiTempMap.FillPath(sbrshGlass, gpNotch);
                    m_gdiTempMap.DrawPath(pGlass, gpNotch);
                }


                DrawQPanels(m_gdiTempMap);
                DrawFPDPanels(m_gdiTempMap);


                /// Glass ID Draw
                ///========================================================================================================================
                ///
                if (m_strInfomation != null && m_bVisibleInfo)
                {
                    for (int i = 0; i < m_strInfomation.Count; i++)
                    {
                        m_gdiTempMap.DrawString(m_strInfomation[i], fntWID, new SolidBrush(Color.Black), (float)((-m_rectdGlassArea.X + 2.0) * (m_dZoomRatio * m_dScale))
                                                                            , (float)((-m_rectdGlassArea.Y + 2.0) * (m_dZoomRatio * m_dScale) + (fntWID.Height * i)));
                    }
                }
            }
            catch { }
            finally
            {
                if (gpGlass != null) gpGlass.Dispose();
                if (sbrshEdge != null) sbrshEdge.Dispose();
                if (pEdge != null) pEdge.Dispose();
                if (sbrshGlass != null) sbrshGlass.Dispose();
                if (pGlass != null) pGlass.Dispose();
                if (fntWID != null) fntWID.Dispose();
            }
        }

        #endregion
        
        #region ■ Point좌표를 주어진 각도로 Rotation하는 함수
        protected virtual void RotatePoint(ref double dx, ref double dy, double RAngle)
        {
            double tX = dx * Math.Cos(RAngle / 180 * Math.PI) + dy * Math.Sin(RAngle / 180 * Math.PI);
            double tY = -dx * Math.Sin(RAngle / 180 * Math.PI) + dy * Math.Cos(RAngle / 180 * Math.PI);

            dx = tX;
            dy = tY;
        }

        #endregion

        #region ▣ Redraw / Paint / Resize Event처리 함수
        protected virtual void GlassMap_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (m_gdiTempMap == null) return;
            m_gdiMain.DrawImageUnscaled(m_bmpGlassMap, 0, 0);
            DrawCenterGrid();
        }

        protected void GlassMap_Resize(object sender, System.EventArgs e)
        {
            if (DesignMode) return;

            if (this.Width * this.Height == 0)
            {
                return;
            }

            if (m_eoMapMode == MapMode.Free)
            {
                RectangleD rectdBefore = m_rectdGlassArea;
                PointD pfBeforeCenter = new PointD(m_rectdGlassArea.X + m_rectdGlassArea.Width / 2, m_rectdGlassArea.Y + m_rectdGlassArea.Height / 2);

                m_rectdGlassArea.Width = this.Width / (m_dZoomRatio * m_dScale);
                m_rectdGlassArea.Height = this.Height / (m_dZoomRatio * m_dScale);

                m_rectdGlassArea.X = pfBeforeCenter.X - m_rectdGlassArea.Width / 2.0d;
                m_rectdGlassArea.Y = pfBeforeCenter.Y - m_rectdGlassArea.Height / 2.0d;
            }
            else if (m_eoMapMode == MapMode.Fit)
            {
                double dMinCanvers_x = 0d;
                double dMinCanvers_y = 0d;

                if ((m_GlassRecipe.GLASS_X / this.Width) > (m_GlassRecipe.GLASS_Y / this.Height))
                {
                    dMinCanvers_x = this.Width * 0.85;
                    m_dZoomRatio = dMinCanvers_x / m_GlassRecipe.GLASS_X;
                    dMinCanvers_y = (float)(m_GlassRecipe.GLASS_Y * m_dZoomRatio);
                }
                else
                {
                    dMinCanvers_y = this.Height * 0.85;
                    m_dZoomRatio = dMinCanvers_y / m_GlassRecipe.GLASS_Y;
                    dMinCanvers_x = (float)(m_GlassRecipe.GLASS_X * m_dZoomRatio);
                }
                m_rectdGlassArea = new RectangleD(((dMinCanvers_x - this.Width) / 2.0d) / (m_dZoomRatio * m_dScale)
                    , ((dMinCanvers_y - this.Height) / 2.0d) / (m_dZoomRatio * m_dScale)
                    , this.Width / m_dZoomRatio
                    , this.Height / m_dZoomRatio);
            }

            if (m_bmpGlassMap != null) m_bmpGlassMap.Dispose();
            if (m_gdiTempMap != null) m_gdiTempMap.Dispose();

            m_gdiMain = this.CreateGraphics();
            m_bmpGlassMap = new Bitmap(this.Width, this.Height);
            m_bmpTemp = new Bitmap(m_bmpGlassMap);
            m_gdiTempMap = Graphics.FromImage(m_bmpGlassMap);
            Redraw();
            GC.Collect();

            if (m_eoMapMode == MapMode.Free)
                SetDelayViewChanged();
            else if (m_eoMapMode == MapMode.Fit)
                SetDelayViewChanged(); //OnViewChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Galss를 다시 Drawing 합니다.
        /// </summary>
        public virtual void Redraw()
        {
            DrawGlass();
            GlassMap_Paint(null, null);
        }

        #endregion

        #region ■ Information Drawing 함수
        public List<string> Information
        {
            get
            {
                return m_strInfomation;
            }
        }
        #endregion

        #region ■ Q-Panel Drawing
        /// <summary>
        /// Q-Panel을 주어진 Graphics object에 Drawing합니다.
        /// </summary>
        /// <param name="g">Graphics Object</param>
        private void DrawQPanels(Graphics g)
        {
            if (m_GlassRecipe.QPanels.Count <= 0) return;

            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];

            /// Pen / Brush는 Dispose해야 하므로 Try밖에 선언하고 Finally에서 Dispose를 ...
            /// 메모리 확인해 본결과 효과 만점...
            Pen pSelQPanelBorder = null;
            Pen pQPanelBorder = null;

            SolidBrush sbrshQPanel = null;

            float cirX = 0f;
            float cirY = 0f;
            float minX = 0f;
            float maxX = 0f;
            float minY = 0f;
            float maxY = 0f;
            float cirR = 0f;

            pSelQPanelBorder = new Pen(Color.Red, 3);
            pQPanelBorder = new Pen(m_colFPDPanelBorder);
            sbrshQPanel = new SolidBrush(Color.White);

            foreach (CQPanel InQPanel in m_GlassRecipe.QPanels)
            {
                if (InQPanel.BIN > 100) continue;
                /// QPanel의 4 점의 좌표를 계산한다. ////////////////////////////////////////////////////////////////////////////////////////
                pdPoint[0].X = InQPanel.QPNL_SPOINT_X - m_GlassRecipe.ORIGIN_X;
                pdPoint[0].Y = InQPanel.QPNL_SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

                pdPoint[1].X = InQPanel.QPNL_SPOINT_X + InQPanel.QPNL_PITCH_X - m_GlassRecipe.ORIGIN_X;
                pdPoint[1].Y = InQPanel.QPNL_SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

                pdPoint[2].X = InQPanel.QPNL_SPOINT_X + InQPanel.QPNL_PITCH_X - m_GlassRecipe.ORIGIN_X;
                pdPoint[2].Y = InQPanel.QPNL_SPOINT_Y + InQPanel.QPNL_PITCH_Y - m_GlassRecipe.ORIGIN_Y;

                pdPoint[3].X = InQPanel.QPNL_SPOINT_X - m_GlassRecipe.ORIGIN_X;
                pdPoint[3].Y = InQPanel.QPNL_SPOINT_Y + InQPanel.QPNL_PITCH_Y - m_GlassRecipe.ORIGIN_Y;

                RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, InQPanel.BIN.ToString()) > -1)
                {
                    sbrshQPanel.Color = m_ColorSet[InQPanel.BIN];
                    pQPanelBorder.Color = m_colQPanelBorder;
                }
                else
                {
                    sbrshQPanel.Color = Color.FromArgb(m_iNotSelectedBinAlpha, m_ColorSet[InQPanel.BIN]);
                    pQPanelBorder.Color = Color.FromArgb(0, 0, 0, 0);
                }

                m_gdiTempMap.FillPolygon(sbrshQPanel, pfPoint);

                if (m_dZoomRatio > 0.03 && m_bVisibleQPanelBorder)
                {
                    if (m_SelectedQPanels.IndexOf(InQPanel) > -1)
                    {
                        g.DrawPolygon(pSelQPanelBorder, pfPoint);
                    }
                    else
                    {
                        g.DrawPolygon(pQPanelBorder, pfPoint);
                    }
                }
                //////////////////////////////////////////////////////////////////////////////////////////


                //////////////////////////////////////////////////////////////////////////////////////////
                ///
                cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X) / 4;
                cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y) / 4;
                minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
                maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
                minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
                maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));
                cirR = Math.Min((float)(InQPanel.QPNL_PITCH_X * (m_dZoomRatio * m_dScale)), (float)(InQPanel.QPNL_PITCH_Y * (m_dZoomRatio * m_dScale)));


                if (m_bVisibleQPanelValue && m_dZoomRatio > 0.8)
                {

                    if ((m_eoDisplayValues & DisplayFlag.QPANEL_BIN) > 0)
                    {
                        g.DrawString(InQPanel.BIN.ToString(), m_fntBin, new SolidBrush(Color.DarkGray), minX, minY);
                    }

                    if ((m_eoDisplayValues & DisplayFlag.QPANEL_ALIAS) > 0)
                    {
                        g.DrawString(InQPanel.QPNL_ALIAS.ToString(), m_fntPara, new SolidBrush(Color.DarkGray), minX, minY);
                    }
                }

            }
        }
        #endregion

        #region ■ FPDPanel Drawing
        /// <summary>
        /// Panel을 주어진 Graphics object에 Drawing합니다.
        /// </summary>
        /// <param name="g">Graphics Object</param>
        protected virtual void DrawFPDPanels(Graphics g)
        {
            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];

            /// Pen / Brush는 Dispose해야 하므로 Try밖에 선언하고 Finally에서 Dispose를 ...
            /// 메모리 확인해 본결과 효과 만점...
            Pen pSelFPDPanelBorder = null;
            Pen pFPDPanelBorder = null;
            Pen pOriginFPDPanelBorder = null;
            Pen pFirstFPDPanelBorder = null;

            SolidBrush sbrshFPDPanel = null;

            float cirX = 0f;
            float cirY = 0f;
            float minX = 0f;
            float maxX = 0f;
            float minY = 0f;
            float maxY = 0f;
            float cirR = 0f;

            SizeF sfText; // Glass Setup 작업중 추가 by 양형석 ('2009.07.20)

            try
            {
                pSelFPDPanelBorder = new Pen(Color.Red, 3);
                pFPDPanelBorder = new Pen(m_colFPDPanelBorder);
                sbrshFPDPanel = new SolidBrush(Color.White);

                foreach (CQPanel InQPanel in m_GlassRecipe.QPanels)
                {
                    foreach (CPanel InFPDPanel in InQPanel.Panels)
                    {
                        if (InFPDPanel.BIN > 100) continue;
                        /// FPDPanel의 4 점의 좌표를 계산한다. ////////////////////////////////////////////////////////////////////////////////////////
                        pdPoint[0].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[0].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[1].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X + InFPDPanel.PITCH_X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[1].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[2].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X + InFPDPanel.PITCH_X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[2].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y + InFPDPanel.PITCH_Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[3].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[3].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y + InFPDPanel.PITCH_Y - m_GlassRecipe.ORIGIN_Y;

                        RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                        pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                        if (Array.IndexOf(m_strSelectedBin, "ALL") > -1 || Array.IndexOf(m_strSelectedBin, InFPDPanel.BIN.ToString()) > -1)
                        {
                            sbrshFPDPanel.Color = m_ColorSet[InFPDPanel.BIN];
                            pFPDPanelBorder.Color = m_colFPDPanelBorder;
                        }
                        else
                        {
                            sbrshFPDPanel.Color = Color.FromArgb(m_iNotSelectedBinAlpha, m_ColorSet[InFPDPanel.BIN]);
                            pFPDPanelBorder.Color = Color.FromArgb(0, 0, 0, 0);
                        }

                        if (m_bVisibleSelectedModel)
                        {
                            if ((InFPDPanel.PanelProperty & 0x01) == 0)
                            {
                                sbrshFPDPanel.Color = Color.FromArgb(50, m_ColorSet[InFPDPanel.BIN]);
                            }
                            else
                            {
                                sbrshFPDPanel.Color = m_ColorSet[InFPDPanel.BIN];
                            }

                            pFPDPanelBorder.Color = m_colFPDPanelBorder;
                        }

                        m_gdiTempMap.FillPolygon(sbrshFPDPanel, pfPoint);
                        if (m_dZoomRatio > 0.001 && m_bVisibleFPDPanelBorder)
                        {
                            g.DrawPolygon(pFPDPanelBorder, pfPoint);
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////


                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///
                        cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X) / 4;
                        cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y) / 4;
                        minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
                        maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
                        minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
                        maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));
                        cirR = Math.Min((float)(InFPDPanel.PITCH_X * (m_dZoomRatio * m_dScale)), (float)(InFPDPanel.PITCH_Y * (m_dZoomRatio * m_dScale)));

                        if (m_bVisibleFPDPanelValue && m_dZoomRatio > 0.8)
                        {
                            if ((m_eoDisplayValues & DisplayFlag.PANEL_ALIAS) > 0)
                            {
                                sfText = g.MeasureString(InFPDPanel.PNL_ALIAS, m_fntAlias);
                                g.DrawString(InFPDPanel.PNL_ALIAS, m_fntAlias, new SolidBrush(Color.Red), minX + (maxX - minX) / 2 - sfText.Width / 2, minY + (maxY - minY) / 2 - sfText.Height / 2);
                            }

                            if ((m_eoDisplayValues & DisplayFlag.SLOT) > 0)
                            {
                                sfText = g.MeasureString(InFPDPanel.PNL_ALIAS, m_fntLabel);
                                g.DrawString(InFPDPanel.PNL_ALIAS, m_fntLabel, new SolidBrush(Color.DarkKhaki), minX - sfText.Width - 5, minY + (maxY - minY) / 2 - sfText.Height / 2);
                            }

                            if ((m_eoDisplayValues & DisplayFlag.PANEL_BIN) > 0)
                            {
                                g.DrawString(InFPDPanel.BIN.ToString(), m_fntBin, new SolidBrush(Color.DarkGray), minX, minY);
                            }

                            if ((m_eoDisplayValues & DisplayFlag.PCM) > 0)
                            {
                                g.DrawString(InFPDPanel.ParaValue.ToString(), m_fntPara, new SolidBrush(Color.DarkGray), minX, minY);
                            }
                        }
                    }
                }

                if (m_dZoomRatio > 0.001 && m_bVisibleFPDPanelBorder)
                {
                    DrawSelFPDPanel(g);
                }
            }
            finally
            {
                if (pSelFPDPanelBorder != null) pSelFPDPanelBorder.Dispose();
                if (pFPDPanelBorder != null) pFPDPanelBorder.Dispose();
                if (pOriginFPDPanelBorder != null) pOriginFPDPanelBorder.Dispose();
                if (pFirstFPDPanelBorder != null) pFirstFPDPanelBorder.Dispose();
                if (sbrshFPDPanel != null) sbrshFPDPanel.Dispose();
            }
        }


        /// <summary>
        /// Selected 된 Panel을 그리는 함수
        /// </summary>
        /// <param name="g"></param>
        public void DrawSelFPDPanel(Graphics g)
        {
            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];

            /// Pen / Brush는 Dispose해야 하므로 Try밖에 선언하고 Finally에서 Dispose를 ...
            /// 메모리 확인해 본결과 효과 만점...
            Pen pSelFPDPanelBorder = null;
            Pen pFPDPanelBorder = null;
            Pen pOriginFPDPanelBorder = null;
            Pen pFirstFPDPanelBorder = null;

            SolidBrush sbrshFPDPanel = null;

            try
            {
                pSelFPDPanelBorder = new Pen(Color.Red, 3);
                pFPDPanelBorder = new Pen(m_colFPDPanelBorder);
                sbrshFPDPanel = new SolidBrush(Color.White);

                foreach (CPanel InFPDPanel in m_SelectedPanels)
                {
                    if (m_eoMapSelectionMode == MapSelectionMode.QMode)
                    {
                        foreach (CQPanel InQPanel in m_GlassRecipe.QPanels)
                        {
                            /// FPDPanel의 4 점의 좌표를 계산한다. ////////////////////////////////////////////////////////////////////////////////////////
                            pdPoint[0].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X - m_GlassRecipe.ORIGIN_X;
                            pdPoint[0].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

                            pdPoint[1].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X + InFPDPanel.PITCH_X - m_GlassRecipe.ORIGIN_X;
                            pdPoint[1].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

                            pdPoint[2].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X + InFPDPanel.PITCH_X - m_GlassRecipe.ORIGIN_X;
                            pdPoint[2].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y + InFPDPanel.PITCH_Y - m_GlassRecipe.ORIGIN_Y;

                            pdPoint[3].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X - m_GlassRecipe.ORIGIN_X;
                            pdPoint[3].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y + InFPDPanel.PITCH_Y - m_GlassRecipe.ORIGIN_Y;

                            RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                            RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                            RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                            RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                            pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                            pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                            pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                            pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                            pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                            pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                            pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                            pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            if (m_dZoomRatio > 0.001 && m_bVisibleFPDPanelBorder)
                            {
                                g.DrawPolygon(pSelFPDPanelBorder, pfPoint);
                            }
                        }
                    }
                    else
                    {
                        CQPanel InQPanel = null;
                        foreach (CQPanel findQPanel in m_GlassRecipe.QPanels)
                        {
                            if (findQPanel.Panels.IndexOf(InFPDPanel) > -1)
                            {
                                InQPanel = findQPanel;
                                break;
                            }
                        }

                        /// FPDPanel의 4 점의 좌표를 계산한다. ////////////////////////////////////////////////////////////////////////////////////////
                        pdPoint[0].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[0].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[1].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X + InFPDPanel.PITCH_X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[1].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[2].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X + InFPDPanel.PITCH_X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[2].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y + InFPDPanel.PITCH_Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[3].X = InQPanel.QPNL_SPOINT_X + InFPDPanel.SPOINT_X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[3].Y = InQPanel.QPNL_SPOINT_Y + InFPDPanel.SPOINT_Y + InFPDPanel.PITCH_Y - m_GlassRecipe.ORIGIN_Y;

                        RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                        pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        if (m_dZoomRatio > 0.001 && m_bVisibleFPDPanelBorder)
                        {
                            g.DrawPolygon(pSelFPDPanelBorder, pfPoint);
                        }
                    }
                }
            }
            finally
            {
                pdPoint = null;
                pfPoint = null;

                if (pSelFPDPanelBorder != null) pSelFPDPanelBorder.Dispose();
                if (pFPDPanelBorder != null) pFPDPanelBorder.Dispose();
                if (pOriginFPDPanelBorder != null) pOriginFPDPanelBorder.Dispose();
                if (pFirstFPDPanelBorder != null) pFirstFPDPanelBorder.Dispose();
                if (sbrshFPDPanel != null) sbrshFPDPanel.Dispose();
            }
        }


        /// <summary>
        /// Glass Center
        /// </summary>
        public void DrawCenterGrid()
        {
            Pen pCenterMark = null;

            try
            {
                if (m_bCenterMark)
                {
                    pCenterMark = new Pen(Color.Red);
                    m_gdiMain.DrawLine(pCenterMark, (float)(-m_rectdGlassArea.X * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2) * (m_dZoomRatio * m_dScale)));

                    m_gdiMain.DrawLine(pCenterMark, (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2) * (m_dZoomRatio * m_dScale))
                        , (float)(-m_rectdGlassArea.Y * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y) * (m_dZoomRatio * m_dScale)));
                }
            }
            finally
            {
                if (pCenterMark != null) pCenterMark.Dispose();
            }
        }

        protected void ClearMarkFPDPanel()
        {
            if (m_gdiMain == null || m_bmpGlassMap == null) return;

            m_gdiMain.DrawImageUnscaled(m_bmpGlassMap, 0, 0);
            DrawCenterGrid();
        }

        protected void RedrawFPDPanel(CPanel argFPDPanel, Color colFPDPanel, Color colBorder)
        {
            if (m_gdiMain == null || m_bmpGlassMap == null) return;

            PointD[] pdPoint = new PointD[4];

            pdPoint[0].X = argFPDPanel.SPOINT_X - m_GlassRecipe.ORIGIN_X;
            pdPoint[0].Y = argFPDPanel.SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

            pdPoint[1].X = argFPDPanel.SPOINT_X + argFPDPanel.PITCH_X - m_GlassRecipe.ORIGIN_X;
            pdPoint[1].Y = argFPDPanel.SPOINT_Y - m_GlassRecipe.ORIGIN_Y;

            pdPoint[2].X = argFPDPanel.SPOINT_X + argFPDPanel.PITCH_X - m_GlassRecipe.ORIGIN_X;
            pdPoint[2].Y = argFPDPanel.SPOINT_Y + argFPDPanel.PITCH_Y - m_GlassRecipe.ORIGIN_Y;

            pdPoint[3].X = argFPDPanel.SPOINT_X - m_GlassRecipe.ORIGIN_X;
            pdPoint[3].Y = argFPDPanel.SPOINT_Y + argFPDPanel.PITCH_Y - m_GlassRecipe.ORIGIN_Y;

            RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
            RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
            RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
            RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

            PointF[] pfPoint = new PointF[4];
            pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
            pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

            pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
            pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

            pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
            pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

            pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
            pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));

            m_gdiMain.DrawImageUnscaled(m_bmpGlassMap, 0, 0);
            m_gdiMain.DrawPolygon(new Pen(colBorder), pfPoint);

            DrawCenterGrid();
        }

        #endregion

        #region ■ Default Color
        protected virtual void SetDefaultColor()
        {
            int r, g, b;//4337915
            //16777215

            int[] iColor = new int[]	{16766976,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
												65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
												16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
												10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
												8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
												6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
												12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
												10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
												2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
												10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686
												,2521304};

            if (m_ColorSet != null) m_ColorSet = null;
            m_ColorSet = new Color[iColor.Length];
            for (int i = 0; i < iColor.Length; i++)
            {
                r = (iColor[i] >> 16);
                g = (iColor[i] >> 8) - (r * 256);
                b = iColor[i] - (r * 65536) - (g * 256);
                m_ColorSet[i] = Color.FromArgb(r, g, b);
            }
        }

        #endregion

        /// <summary>
        /// Bin별 Color를 재정의합니다.
        /// </summary>
        /// <param name="Index">BIN Index</param>
        /// <param name="BinColor">Color object</param>
        public void SetColor(int Index, Color BinColor)
        {
            m_ColorSet[Index] = BinColor;
        }
        /// <summary>
        /// Bin별 Color 및 투명도를 재정의합니다.
        /// </summary>
        /// <param name="Index">BIN Index</param>
        /// <param name="BinColor">Color object</param>
        /// <param name="Alpha">투명도</param>
        public void SetColor(int Index, Color BinColor, int Alpha)
        {
            m_ColorSet[Index] = Color.FromArgb(Alpha, BinColor); ;
        }
        /// <summary>
        /// Bin Index의 Color을 가져옵니다.
        /// </summary>
        /// <param name="Index">BIN Index</param>
        /// <returns></returns>
        public Color GetColor(int Index)
        {
            return m_ColorSet[Index];
        }

        #region ◈ Popup Menu Click Event 처리

        protected virtual void mnuitemMAPMODE_FITSIZE_Click(object sender, System.EventArgs e)
        {
            MapMode mode = m_eoMapMode;
            m_eoMapMode = MapMode.Fit;
            GlassMap_Resize(null, null);
            MenuManagment();
            m_eoMapMode = mode;
        }

        protected virtual void mnuitemMAPMODE_FREEZOOM_Click(object sender, System.EventArgs e)
        {
            m_eoMapMode = MapMode.Free;
            m_eoMouseDragMode = MouseDragMode.Zoom;

            mnuitemMAPMODE_FREEZOOM.Checked = true;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            MenuManagment();
        }

        protected virtual void mnuitemMOUSEDRAGMODE_ROTATE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Rotate;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = true;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            MenuManagment();
        }

        protected virtual void mnuitemMOUSEDRAGMODE_MOVE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Move;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = true;
            MenuManagment();
        }

        protected virtual void mnuitemMAPSELECTSTYLE_CIRCLE_Click(object sender, System.EventArgs e)
        {
            m_eoMapSelectStyle = MapSelectStyle.Circle;
            //m_eoMouseDragMode = MouseDragMode.Zone;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = true;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            mnuitemMAPSELECTSTYLE_BAND.Checked = false;
        }

        protected virtual void mnuitemMAPSELECTSTYLE_PIE_Click(object sender, System.EventArgs e)
        {
            m_eoMapSelectStyle = MapSelectStyle.Pie;
            //m_eoMouseDragMode = MouseDragMode.Zone;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            mnuitemMAPSELECTSTYLE_PIE.Checked = true;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            mnuitemMAPSELECTSTYLE_BAND.Checked = false;
        }

        protected virtual void mnuitemMAPSELECTSTYLE_RECTANGLE_Click(object sender, System.EventArgs e)
        {
            m_eoMapSelectStyle = MapSelectStyle.Rectangle;
            //m_eoMouseDragMode = MouseDragMode.Zone;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = true;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            mnuitemMAPSELECTSTYLE_BAND.Checked = false;
        }

        protected virtual void mnuitemMAPSELECTSTYLE_FREEHAND_Click(object sender, System.EventArgs e)
        {
            m_eoMapSelectStyle = MapSelectStyle.FreeHand;
            //m_eoMouseDragMode = MouseDragMode.Zone;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = true;
            mnuitemMAPSELECTSTYLE_BAND.Checked = false;
        }

        protected virtual void mnuitemMAPSELECTSTYLE_BAND_Click(object sender, System.EventArgs e)
        {
            m_eoMapSelectStyle = MapSelectStyle.Band;
            //m_eoMouseDragMode = MouseDragMode.Zone;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            mnuitemMAPSELECTSTYLE_BAND.Checked = true;
        }

        protected void MenuManagment()
        {
        }

        private void menuitemMAPMODE_ZOOMIN_Click(object sender, System.EventArgs e)
        {
            ZoomIn();
        }

        private void menuitemMAPMODE_ZOOMOUT_Click(object sender, System.EventArgs e)
        {
            ZoomOut();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0) ZoomOut();
            if (e.Delta < 0) ZoomIn();
            base.OnMouseWheel(e);
        }

        private void mnuCOPYTOCLIP_Click(object sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(m_bmpGlassMap);
        }

        #endregion

        public Bitmap GetMapImage()
        {
            return m_bmpGlassMap;
        }

        private void mnuitemCLEAR_SELECTEDDIE_Click(object sender, System.EventArgs e)
        {
            ResetSelectedFPDPanel();
        }

        private void MMGBinMap_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            SetDefaultColor();
            Reset();
            GlassMap_Resize(null, null);
        }


        //public void CalModel(IDMakeType oIDMakeTYpe)
        //{
        //    if (m_GlassRecipe == null) return;
        //    if (m_GlassRecipe.Models.Count == 0) return;

        //    this.PanelIDRule = oIDMakeTYpe;
        //    int iModelIdx = 0;
        //    int iPanNum = 0;
        //    int chr_idx = 0;
        //    int iNum = 0;

        //    foreach (CQPanel InQPanel in m_GlassRecipe.QPanels)
        //    {
        //        InQPanel.Panels.Clear();
        //        iModelIdx = 0;
        //        foreach (CModel InModel in m_GlassRecipe.Models)
        //        {
        //            switch (m_eoPnlIDMake)
        //            {
        //                /// L2R_B2T : 왼쪽에서 오른쪽, 아래에서 위 , 좌우 진행 우선
        //                case IDMakeType.L2R_B2T:
        //                    for (int y = 0; y < InModel.GRID_Y; y++)
        //                    {
        //                        for (int x = 0; x < InModel.GRID_X; x++)
        //                        {
        //                            InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
        //                        }
        //                        chr_idx++;
        //                        iNum = 0;
        //                    }

        //                    break;
        //                /// B2T_L2R : 왼쪽에서 오른쪽, 아래에서 위 , 위아래 진행 우선
        //                case IDMakeType.B2T_L2R:
        //                    for (int x = 0; x < InModel.GRID_X; x++)
        //                    {
        //                        for (int y = 0; y < InModel.GRID_Y; y++)
        //                        {
        //                            InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
        //                        }
        //                        chr_idx++;
        //                        iNum = 0;
        //                    }
        //                    break;

        //                /// L2R_T2B : 왼쪽에서 오른쪽, 위에서 아래 , 좌우 진행 우선
        //                case IDMakeType.L2R_T2B:
        //                    for (int y = InModel.GRID_Y - 1; y >= 0; y--)
        //                    {
        //                        for (int x = 0; x < InModel.GRID_X; x++)
        //                        {
        //                            InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
        //                        }
        //                        chr_idx++;
        //                        iNum = 0;
        //                    }
        //                    break;
        //                /// T2B_L2R : 왼쪽에서 오른쪽, 위에서 아래 , 위아래 진행 우선
        //                case IDMakeType.T2B_L2R:
        //                    for (int x = 0; x < InModel.GRID_X; x++)
        //                    {
        //                        for (int y = InModel.GRID_Y - 1; y >= 0; y--)
        //                        {
        //                            InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
        //                        }
        //                        chr_idx++;
        //                        iNum = 0;
        //                    }
        //                    break;


        //                /// R2L_B2T : 오른쪽에서 왼쪽, 아래에서 위 , 좌우 진행 우선
        //                case IDMakeType.R2L_B2T:
        //                    for (int y = 0; y < InModel.GRID_Y; y++)
        //                    {
        //                        for (int x = InModel.GRID_X - 1; x >= 0; x--)
        //                        {
        //                            InQPanel.Panels.Add(MakePanel(x, y, iPanNum++,chr_idx,iNum++, InQPanel, InModel));
        //                        }
        //                        chr_idx++;
        //                        iNum = 0;
        //                    }
        //                    break;
        //                /// B2T_R2L : 오른쪽에서 왼쪽, 아래에서 위 , 위아래 진행 우선
        //                case IDMakeType.B2T_R2L:
        //                    for (int x = InModel.GRID_X - 1; x >= 0; x--)
        //                    {
        //                        for (int y = 0; y < InModel.GRID_Y; y++)
        //                        {
        //                            InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
        //                        }
        //                        chr_idx++;
        //                        iNum = 0;
        //                    }
        //                    break;


        //                /// R2L_T2B : 오른쪽에서 왼쪽, 위에서 아래 , 좌우 진행 우선
        //                case IDMakeType.R2L_T2B:
        //                    for (int y = InModel.GRID_Y - 1; y >= 0; y--)
        //                    {
        //                        for (int x = InModel.GRID_X - 1; x >= 0; x--)
        //                        {
        //                            InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
        //                        }
        //                        chr_idx++;
        //                        iNum = 0;
        //                    }
        //                    break;
        //                /// T2B_R2L : 오른쪽에서 왼쪽, 위에서 아래 , 위아래 진행 우선
        //                case IDMakeType.T2B_R2L:
        //                    for (int x = InModel.GRID_X - 1; x >= 0; x--)
        //                    {
        //                        for (int y = InModel.GRID_Y - 1; y >= 0; y--)
        //                        {
        //                            InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
        //                        }
        //                        chr_idx++;
        //                        iNum = 0;
        //                    }
        //                    break;

        //                default:
        //                    for (int x = 0; x < InModel.GRID_X; x++)
        //                    {
        //                        for (int y = 0; y < InModel.GRID_Y; y++)
        //                        {
        //                            InQPanel.Panels.Add(MakePanel(x, y, iPanNum++, chr_idx, iNum++, InQPanel, InModel));
        //                        }
        //                        chr_idx++;
        //                        iNum = 0;
        //                    }
        //                    break;
        //            }

        //            iModelIdx++;
        //        }
        //    }
        //}

        //private CPanel MakePanel(int x, int y, int idx,int chr_idx,int iNum, CQPanel InQPanel, CModel InModel)
        //{
        //    CPanel oPanel = new CPanel(idx);
        //    oPanel.INDEX_X = x + 1;
        //    oPanel.INDEX_Y = y + 1;
        //    oPanel.PITCH_X = InModel.PITCH_X;
        //    oPanel.PITCH_Y = InModel.PITCH_Y;
        //    oPanel.SPOINT_X = InModel.ORIGIN_X + (InModel.PITCH_X * x) + (InModel.STREET_X * x);
        //    oPanel.SPOINT_Y = InModel.ORIGIN_Y + (InModel.PITCH_Y * y) + (InModel.STREET_Y * y);
        //    oPanel.MODEL_ID = InModel.MODEL_ID;

        //    if (m_GlassRecipe.Models.IndexOf(InModel) == 0)
        //    {
        //        oPanel.PNL_ALIAS = string.Format("{0}-{1}{2}", InQPanel.QPNL_ALIAS, m_charIdx[chr_idx], iNum+1);
        //    }
        //    else
        //    {
        //        oPanel.PNL_ALIAS = string.Format("{0}-{1}{2}-{3}", InQPanel.QPNL_ALIAS, m_charIdx[chr_idx], iNum+1, InModel.MODEL_ID);
        //    }

        //    oPanel.QPNL_ALIAS = InQPanel.QPNL_ALIAS;
        //    oPanel.PanelProperty = 0;
        //    oPanel.BIN = 1;

        //    return oPanel;
        //}

        public void SetSelectedModel(string ModelID)
        {
            foreach (CQPanel InQPanel in m_GlassRecipe.QPanels)
            {
                foreach (CPanel InPanel in InQPanel.Panels)
                {
                    if (InPanel.MODEL_ID == ModelID)
                    {
                        InPanel.PanelProperty = InPanel.PanelProperty | 0x01;
                    }
                    else
                    {
                        InPanel.PanelProperty = InPanel.PanelProperty & 0xFE;
                    }
                }
            }
            m_bVisibleSelectedModel = true;
            Redraw();
        }

        public void DeleteModel(string ModelID)
        {
            foreach (CQPanel InQPanel in m_GlassRecipe.QPanels)
            {
                List<CPanel> oDelete = new List<CPanel>();

                foreach (CPanel InPanel in InQPanel.Panels)
                {
                    if (InPanel.MODEL_ID == ModelID)
                    {
                        oDelete.Add(InPanel);
                    }
                }

                for (int i = 0; i < oDelete.Count; i++)
                    InQPanel.Panels.Remove(oDelete[i]);

            }

            foreach (CModel InModel in m_GlassRecipe.Models)
            {
                if (InModel.MODEL_ID == ModelID)
                {
                    m_GlassRecipe.Models.Remove(InModel);
                    break;
                }
            }

            Redraw();
        }

        private void mnuitemSELECTIONMODE_ALL_Click(object sender, EventArgs e)
        {
            mnuitemSELECTIONMODE_ALL.Checked = true;
            mnuitemSELECTIONMODE_QMODE.Checked = false;

            m_eoMapSelectionMode = MapSelectionMode.ALL;
        }

        private void mnuitemSELECTIONMODE_QMODE_Click(object sender, EventArgs e)
        {
            mnuitemSELECTIONMODE_ALL.Checked = false;
            mnuitemSELECTIONMODE_QMODE.Checked = true;

            m_eoMapSelectionMode = MapSelectionMode.QMode;
        }

        public void FitSize()
        {
            mnuitemMAPMODE_FITSIZE_Click(null, null);
        }

        [Browsable(false)]
        public CGlass Glass
        {
            get { return m_GlassRecipe; }
        }
    }

    /// <summary>
    /// 지정 Interval 만큼 지연 후 Tick 이벤트를 발생.
    /// 만약 Set() 메서드가 호출되면 Interval 값은 초기화됨.
    /// </summary>
    public class DelayTimer
    {
        /// <summary>
        /// 지연 시간을 나타냅니다.
        /// </summary>
        public static int INTERVAL = 1000;

        public event EventHandler Tick;

        private int _timer = 0;

        public DelayTimer()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(RunAsnyc);
        }

        /// <summary>
        /// 타이머를 설정 Interval로 초기화 합니다.
        /// </summary>
        public void Set()
        {
            _timer = INTERVAL;
        }

        internal void Reset()
        {
            _timer = 0;
        }

        protected virtual void OnTick(EventArgs e)
        {
            if (Tick == null || Owner == null)
                return;

            if (Owner.InvokeRequired)
                Owner.BeginInvoke(new Action<EventArgs>(OnTick), e);
            else
                Tick(this, e);
        }

        private void RunAsnyc(object state)
        {
            while (true)
            {

                if (_timer > 0)
                {
                    if (_timer == 100)
                        OnTick(EventArgs.Empty);

                    _timer -= 100;
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        public Control Owner 
        { 
            get; 
            set; 
        }

        public double ZoomX
        {
            get;
            set;
        }

        public double ZoomY
        {
            get;
            set;
        }
    }
}
