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
    public partial class MMGBinMap_Simple : UserControl
    {
        protected DelayTimer _timer = new DelayTimer();
        /// <summary>m_dScale : Glass Drawing 계산이 끝난 후 화면에 Drawing하는 비율</summary>
        protected PointD m_dZoomRatio = new PointD(1.0, 1.0);
        protected Rectangle m_rectSelect = Rectangle.Empty;
        protected Point m_poStart = Point.Empty;
        protected Point m_poEnd = Point.Empty;
        protected MapSelectionMode m_eoMapSelectionMode = MapSelectionMode.ALL;
        protected MapMode m_eoMapMode = MapMode.Free;
        protected MapSelectStyle m_eoMapSelectStyle = MapSelectStyle.Rectangle;
        protected bool m_bVisibleInfo = true;
        protected List<string> m_strInfomation = new List<string>();
        protected Color m_colGlass = Color.Gray;
        protected Color m_colGlassBorder = Color.Black;
        protected Color m_colEdge = Color.Black;
        protected string[] m_strSelectedBin = new string[1]
    {
      "ALL"
    };
        public int m_iNotSelectedBinAlpha = 20;
        protected int m_iCurrentX = -1;
        protected int m_iCurrentY = -1;
        protected int m_iCurrentFPDPanelIndex = -1;
        protected Font m_fntAlias = new Font("Tahoma", 10f);
        protected Font m_fntLabel = new Font("Tahoma", 8f);
        protected RectangleF m_rectNotchArea = RectangleF.Empty;
        protected RectangleF m_rectOCRIDArea = RectangleF.Empty;
        protected bool m_bPopupMenu = true;
        public const int DRAG_RANGE = 5;
        public const int LEFT_MARGIN = 160;
        public const int BOTTOM_MARGIN = 50;
        public const int RIGHT_MARGIN = 20;
        public const int TOP_MARGIN = 120;

        protected bool m_isDrag;
        protected RectangleD m_rectdGlassArea;
        protected MouseDragMode m_eoMouseDragMode;
        protected GraphicsPath m_SelectPath = new GraphicsPath();
        protected Color[] m_ColorSet;
        protected Position m_poInformation;
        protected bool m_bCenterMark;
        protected bool m_bScale;
        protected bool m_bVisibleSelectedModel;
        protected CGlass m_GlassRecipe;
        protected HatchBrush m_hbSelectBrush;
        protected DataTable m_DT;
        protected MenuItem mnuCOPYTOCLIP;
        private bool _isDisposed;
        protected bool m_isClick;

        public bool PopupMenu
        {
            get
            {
                return this.m_bPopupMenu;
            }
            set
            {
                this.m_bPopupMenu = value;
            }
        }

        [Description("Glass의 Background Color를 설정하거나 가져옵니다.")]
        [Category("Glass 속성")]
        public Color GlassColor
        {
            get
            {
                return this.m_colGlass;
            }
            set
            {
                this.m_colGlass = value;
            }
        }

        [Description("Glass의 Border Color를 설정하거나 가져옵니다.")]
        [Category("Glass 속성")]
        public Color GlassBorderColor
        {
            get
            {
                return this.m_colGlassBorder;
            }
            set
            {
                this.m_colGlassBorder = value;
            }
        }

        [Category("Glass 속성")]
        [Description("Edge의 Background Color를 설정하거나 가져옵니다.")]
        public Color EdgeColor
        {
            get
            {
                return this.m_colEdge;
            }
            set
            {
                this.m_colEdge = value;
            }
        }

        [Category("Glass 속성")]
        [Description("Net FPDPanel의 개수를 가져옵니다.")]
        public int NetFPDPanel
        {
            get
            {
                return this.m_GlassRecipe.QPNL_COUNT;
            }
        }

        [Category("Glass Option")]
        [Description("Glass Infomation의 표시 여부를 설정하거나 가져옵니다.")]
        public bool VisibleInfomation
        {
            get
            {
                return this.m_bVisibleInfo;
            }
            set
            {
                this.m_bVisibleInfo = value;
            }
        }

        [Category("Glass Option")]
        [Description("Visual Inspection Code의 Display여부를 설정하거나 가져옵니다.")]
        public bool VisibleSelectedModel
        {
            get
            {
                return this.m_bVisibleSelectedModel;
            }
            set
            {
                this.m_bVisibleSelectedModel = value;
            }
        }

        [Description("FPDPanel X, Y의 좌표 눈금 표현 여부를 설정하거나 가져옵니다.")]
        [Category("Glass Option")]
        public bool ScaleMark
        {
            get
            {
                return this.m_bScale;
            }
            set
            {
                this.m_bScale = value;
                this.Invalidate();
            }
        }

        [Category("Glass Option")]
        [Description("Glass의 Drawing Mode을 설정하거나 가져옵니다.")]
        public virtual MapMode GlassDrawMode
        {
            get
            {
                return this.m_eoMapMode;
            }
            set
            {
                this.m_eoMapMode = value;
                switch (this.m_eoMapMode)
                {
                    case MapMode.Fit:
                        this.mnuitemMAPMODE_FITSIZE_Click(null, (EventArgs)null);
                        break;
                    case MapMode.Free:
                        this.mnuitemMAPMODE_FREEZOOM_Click(null, (EventArgs)null);
                        break;
                }
            }
        }

        public List<string> Information
        {
            get
            {
                return this.m_strInfomation;
            }
        }

        [Browsable(false)]
        public CGlass Glass
        {
            get
            {
                return this.m_GlassRecipe;
            }
        }

        [DefaultValue(typeof(MapFitMode), "DisplayAll")]
        public MapFitMode MapFitMode { get; set; }

        /// <summary>눈금선을 그릴지를 설정하거나 가져옵니다.</summary>
        [Description("Map에 눈금선을 그릴지를 지정합니다.")]
        [DefaultValue(false)]
        public bool ShowGridLine { get; set; }

        protected string StatusTotal
        {
            get
            {
                return this.tslTotal.Text;
            }
            set
            {
                this.tslTotal.Text = "Total Count: " + value;
            }
        }

        protected string StatusCurrent
        {
            get
            {
                return this.tslCurrent.Text;
            }
            set
            {
                this.tslCurrent.Text = "Current Count: " + value;
            }
        }

        protected string StatusMessage
        {
            get
            {
                return this.tslMessage.Text;
            }
            set
            {
                this.tslMessage.Text = value;
            }
        }

        protected string StatusCoordinate
        {
            get
            {
                return this.tslCoord.Text;
            }
            set
            {
                this.tslCoord.Text = value;
            }
        }

        protected StatusStrip StatusBar
        {
            get
            {
                return this.statusStrip;
            }
        }

        public int AngleOffSet
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public int ViewAngle
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        [DefaultValue(true)]
        public bool VisibleStatusBar
        {
            get { return statusStrip.Visible; }
            set { statusStrip.Visible = value; }
        }

        /// <summary>Map이 확대 되거나 축소되는 등의 View가 변경되는 경우 발생하는 이벤트 입니다.</summary>
        public event EventHandler ViewChanged;

        public MMGBinMap_Simple()
        {
            this.SetStyle(ControlStyles.UserMouse | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.m_GlassRecipe = new CGlass();
            this.InitializeComponent();
            this.SetDefaultColor();
            this.Reset();
            this.menuItem1.Visible = this.menuItem2.Visible = false;
            this._timer.Tick += new EventHandler(this.DelayTimer_Tick);
            this._timer.Owner = (Control)this;
        }

        ~MMGBinMap_Simple()
        {
            this.Dispose(false);
        }

        protected void ResetViewChanged()
        {
            this._timer.Reset();
        }

        protected void SetDelayViewChanged()
        {
            this._timer.Set();
        }

        protected int GetWidth()
        {
            return this.Width;
        }

        protected int GetHeight()
        {
            int height = Height;

            if (StatusBar.Visible)
                height -= StatusBar.Height;

            if (Information.Count > 0)
                height -= Font.Height * Information.Count;

            return height;
        }

        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            this.OnViewChanged(e);
        }

        protected virtual void OnViewChanged(EventArgs e)
        {
            if (this.ViewChanged == null)
                return;
            this.ViewChanged(this, e);
        }

        public void SetGlass(CGlass oClass)
        {
            this.m_GlassRecipe = oClass;
        }

        public void Copy(MMGBinMap_Simple TagetMap)
        {
            if (TagetMap == null)
                TagetMap = new MMGBinMap_Simple();
            TagetMap.m_GlassRecipe = this.m_GlassRecipe;
            TagetMap.m_strInfomation = this.m_strInfomation;
            TagetMap.m_ColorSet = this.m_ColorSet;
            TagetMap.Redraw();
        }

        public new void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>사용 중인 모든 리소스를 정리합니다.</summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (this.m_hbSelectBrush != null)
                    this.m_hbSelectBrush.Dispose();
                //if (this.ctxmGlassMap != null)
                //    this.ctxmGlassMap.Dispose();
                if (this.m_DT != null)
                    this.m_DT.Dispose();
                this._isDisposed = true;
            }
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>Glass Map을 초기화 합니다. Recipe가 초기값으로 임의설정됩니다.</summary>
        protected virtual void Reset()
        {
            this.m_GlassRecipe = (CGlass)null;
            if (this.m_hbSelectBrush != null)
                this.m_hbSelectBrush.Dispose();
            this.m_GlassRecipe = new CGlass(1000.0, 1000.0, false);
            this.m_GlassRecipe.NOTCH_SIZE = 2.0;
            double num1;
            double num2;
            if (this.m_GlassRecipe.GLASS_X / (double)this.GetWidth() > this.m_GlassRecipe.GLASS_Y / (double)this.GetHeight())
            {
                num1 = (double)this.GetWidth();
                this.m_dZoomRatio.X = num1 / this.m_GlassRecipe.GLASS_X;
                num2 = this.m_GlassRecipe.GLASS_Y * this.m_dZoomRatio.X;
                this.m_dZoomRatio.Y = num2 / this.m_GlassRecipe.GLASS_Y;
            }
            else
            {
                num2 = (double)this.GetHeight();
                this.m_dZoomRatio.Y = num2 / this.m_GlassRecipe.GLASS_Y;
                num1 = this.m_GlassRecipe.GLASS_X * this.m_dZoomRatio.X;
                this.m_dZoomRatio.X = num1 / this.m_GlassRecipe.GLASS_X;
            }
            this.m_rectdGlassArea.X = (num1 - (double)this.GetWidth()) / 2.0 / this.m_dZoomRatio.X;
            this.m_rectdGlassArea.Y = (num2 - (double)this.GetHeight()) / 2.0 / this.m_dZoomRatio.Y;
            this.m_rectdGlassArea.Width = (double)this.GetWidth() / this.m_dZoomRatio.X;
            this.m_rectdGlassArea.Height = (double)this.GetHeight() / this.m_dZoomRatio.Y;
            this.m_hbSelectBrush = new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.White, Color.PowderBlue);
            this.SetDelayViewChanged();
        }

        private void FadeIn()
        {
            if (this.m_GlassRecipe == null || this.m_GlassRecipe.QPanels == null || this.m_GlassRecipe.QPanels.Count <= 0)
                return;
            for (int index = 10; index > 0; --index)
            {
                this.m_iNotSelectedBinAlpha = index * (int)byte.MaxValue / 10;
                this.Redraw();
            }
        }

        public void ZoomIn()
        {
            this.ZoomIn(Point.Empty);
        }

        public void ZoomIn(Point center)
        {
            if (this.m_eoMapMode == MapMode.Fit)
                return;
            this.m_dZoomRatio.X = this.m_dZoomRatio.X * 1.05;
            this.m_dZoomRatio.Y = this.m_dZoomRatio.Y * 1.05;
            PointD pointD = new PointD(this.m_rectdGlassArea.X + this.m_rectdGlassArea.Width / 2.0, this.m_rectdGlassArea.Y + this.m_rectdGlassArea.Height / 2.0);
            this.m_rectdGlassArea.Width = (double)this.GetWidth() / this.m_dZoomRatio.X;
            this.m_rectdGlassArea.Height = (double)this.GetHeight() / this.m_dZoomRatio.Y;
            this.m_rectdGlassArea.X = pointD.X - this.m_rectdGlassArea.Width / 2.0;
            this.m_rectdGlassArea.Y = pointD.Y - this.m_rectdGlassArea.Height / 2.0;
            this.Invalidate();
            this.SetDelayViewChanged();
        }

        public void ZoomOut()
        {
            this.ZoomOut(Point.Empty);
        }

        public void ZoomOut(Point center)
        {
            if (this.m_eoMapMode == MapMode.Fit)
                return;
            this.m_dZoomRatio.X = this.m_dZoomRatio.X * 0.949999988079071;
            this.m_dZoomRatio.Y = this.m_dZoomRatio.Y * 0.949999988079071;
            PointD pointD = new PointD(this.m_rectdGlassArea.X + this.m_rectdGlassArea.Width / 2.0, this.m_rectdGlassArea.Y + this.m_rectdGlassArea.Height / 2.0);
            this.m_rectdGlassArea.Width = (double)this.GetWidth() / this.m_dZoomRatio.X;
            this.m_rectdGlassArea.Height = (double)this.GetHeight() / this.m_dZoomRatio.Y;
            this.m_rectdGlassArea.X = pointD.X - this.m_rectdGlassArea.Width / 2.0;
            this.m_rectdGlassArea.Y = pointD.Y - this.m_rectdGlassArea.Height / 2.0;
            this.Invalidate();
            this.SetDelayViewChanged();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.m_poStart.X = e.X;
                this.m_poStart.Y = e.Y;
                this.m_poEnd.X = e.X;
                this.m_poEnd.Y = e.Y;
                this.m_rectSelect.Width = Math.Max(this.m_poStart.X, this.m_poEnd.X) - Math.Min(this.m_poStart.X, this.m_poEnd.X);
                this.m_rectSelect.Height = Math.Max(this.m_poStart.Y, this.m_poEnd.Y) - Math.Min(this.m_poStart.Y, this.m_poEnd.Y);
                this.m_SelectPath = new GraphicsPath();
                this.m_isClick = true;
            }
            else if (e.Button == MouseButtons.Right && this.m_bPopupMenu)
                this.ctxmGlassMap.Show((Control)this, new Point(e.X, e.Y));
            this.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            this.GetRealPoint(this.m_poStart.X, this.m_poStart.Y);
            this.m_rectSelect.X = Math.Min(this.m_poStart.X, e.X);
            this.m_rectSelect.Y = Math.Min(this.m_poStart.Y, e.Y);
            this.m_rectSelect.Width = Math.Max(this.m_poStart.X, e.X) - Math.Min(this.m_poStart.X, e.X);
            this.m_rectSelect.Height = Math.Max(this.m_poStart.Y, e.Y) - Math.Min(this.m_poStart.Y, e.Y);
            this.m_isDrag = this.m_rectSelect.Width > 5 || this.m_rectSelect.Height > 5;
            this.m_isClick = !this.m_isDrag;
            if (this.m_eoMouseDragMode == MouseDragMode.Move && this.m_eoMapMode != MapMode.Fit)
            {
                this.m_rectdGlassArea.X = this.m_rectdGlassArea.X - (double)(e.X - this.m_poEnd.X) / this.m_dZoomRatio.X;
                this.m_rectdGlassArea.Y = this.m_rectdGlassArea.Y - (double)(e.Y - this.m_poEnd.Y) / this.m_dZoomRatio.Y;
            }
            this.m_poEnd.X = e.X;
            this.m_poEnd.Y = e.Y;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            switch (this.m_eoMouseDragMode)
            {
                case MouseDragMode.Zoom:
                    if (this.m_eoMapMode != MapMode.Fit)
                    {
                        if (this.m_rectSelect.Width * this.m_rectSelect.Height < 50)
                            return;
                        PointD dZoomRatio = this.m_dZoomRatio;
                        double val2 = (double)this.GetWidth() / (double)this.m_rectSelect.Width;
                        double val1 = (double)this.GetHeight() / (double)this.m_rectSelect.Height;
                        PointD pointD = new PointD((double)(this.m_rectSelect.X + this.m_rectSelect.Width / 2) / dZoomRatio.X + this.m_rectdGlassArea.X, (double)(this.m_rectSelect.Y + this.m_rectSelect.Height / 2) / dZoomRatio.Y + this.m_rectdGlassArea.Y);
                        this.m_dZoomRatio.X = Math.Min(val1, val2) * dZoomRatio.X;
                        this.m_dZoomRatio.Y = Math.Min(val1, val2) * dZoomRatio.Y;
                        this.m_rectdGlassArea.Width = (double)this.GetWidth() / this.m_dZoomRatio.X;
                        this.m_rectdGlassArea.Height = (double)this.GetHeight() / this.m_dZoomRatio.Y;
                        this.m_rectdGlassArea.X = pointD.X - this.m_rectdGlassArea.Width / 2.0;
                        this.m_rectdGlassArea.Y = pointD.Y - this.m_rectdGlassArea.Height / 2.0;
                        break;
                    }
                    break;
            }
            this.m_isDrag = false;
            this.Invalidate();
            this.SetDelayViewChanged();
        }

        protected PointD GetRealPoint(int piX, int piY)
        {
            PointD viewPoint = this.GetViewPoint(0.0, 0.0);
            PointD pointD1;
            pointD1.X = viewPoint.X - (double)piX;
            pointD1.Y = viewPoint.Y - (double)piY;
            piX = (int)(pointD1.X + viewPoint.X);
            piY = (int)(pointD1.Y + viewPoint.Y);
            PointD pointD2;
            pointD2.X = this.m_GlassRecipe.GLASS_X / 2.0 - (this.m_rectdGlassArea.X + (double)piX / this.m_dZoomRatio.X);
            pointD2.Y = -this.m_GlassRecipe.GLASS_Y / 2.0 + (this.m_rectdGlassArea.Y + (double)piY / this.m_dZoomRatio.Y);
            return pointD2;
        }

        private PointD GetViewPoint(double dX, double dY)
        {
            PointD empty = PointD.Empty;
            empty.X = (-this.m_rectdGlassArea.X + this.m_GlassRecipe.GLASS_X / 2.0 + dX) * this.m_dZoomRatio.X;
            empty.Y = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y / 2.0 - dY) * this.m_dZoomRatio.Y;
            return empty;
        }

        protected void DrawSelectRect(Graphics g)
        {
            if (this.m_rectSelect.Width * this.m_rectSelect.Height <= 0)
                return;
            Pen pen = new Pen((Brush)this.m_hbSelectBrush, 3f);
            g.DrawRectangle(pen, this.m_rectSelect.X, this.m_rectSelect.Y, this.m_rectSelect.Width - 1, this.m_rectSelect.Height - 1);
        }

        private void DrawMoveRect(Graphics g)
        {
            this.m_rectSelect = new Rectangle(0, 0, this.GetWidth(), this.GetHeight());
            this.m_rectSelect.Offset(this.m_poEnd.X - this.m_poStart.X, this.m_poEnd.Y - this.m_poStart.Y);
            if (this.m_rectSelect.Width * this.m_rectSelect.Height <= 0)
                return;
            Pen pen = new Pen((Brush)this.m_hbSelectBrush, 3f);
            g.DrawRectangle(pen, this.m_rectSelect);
        }

        private void DrawZoneGuid(Graphics g)
        {
            Pen pen = (Pen)null;
            SolidBrush solidBrush = (SolidBrush)null;
            try
            {
                pen = new Pen((Brush)this.m_hbSelectBrush, 3f);
                if (this.m_eoMapSelectStyle == MapSelectStyle.FreeHand)
                {
                    this.m_SelectPath.AddLine(this.m_poStart, this.m_poEnd);
                    this.m_poStart = this.m_poEnd;
                    g.DrawPath(pen, this.m_SelectPath);
                }
                else
                {
                    this.m_SelectPath.Reset();
                    switch (this.m_eoMapSelectStyle)
                    {
                        case MapSelectStyle.Circle:
                            if (this.m_rectSelect.Width * this.m_rectSelect.Height > 0)
                            {
                                this.m_SelectPath.AddEllipse(this.m_rectSelect.X, this.m_rectSelect.Y, this.m_rectSelect.Width - 1, this.m_rectSelect.Height - 1);
                                break;
                            }
                            break;
                        case MapSelectStyle.Band:
                            PointD realPoint1 = this.GetRealPoint(this.m_poStart.X, this.m_poStart.Y);
                            double num1 = Math.Sqrt(Math.Pow(realPoint1.X, 2.0) + Math.Pow(realPoint1.Y, 2.0)) * 2.0;
                            this.m_rectSelect.X = (int)((-this.m_rectdGlassArea.X - num1 / 2.0 + this.m_GlassRecipe.GLASS_X / 2.0) * this.m_dZoomRatio.X);
                            this.m_rectSelect.Y = (int)((-this.m_rectdGlassArea.Y - num1 / 2.0 + this.m_GlassRecipe.GLASS_Y / 2.0) * this.m_dZoomRatio.Y);
                            this.m_rectSelect.Width = (int)(num1 * this.m_dZoomRatio.X);
                            this.m_rectSelect.Height = (int)(num1 * this.m_dZoomRatio.Y);
                            PointD realPoint2 = this.GetRealPoint(this.m_poEnd.X, this.m_poEnd.Y);
                            double num2 = Math.Sqrt(Math.Pow(realPoint2.X, 2.0) + Math.Pow(realPoint2.Y, 2.0)) * 2.0;
                            Rectangle rect = new Rectangle();
                            rect.X = (int)((-this.m_rectdGlassArea.X - num2 / 2.0 + this.m_GlassRecipe.GLASS_X / 2.0) * this.m_dZoomRatio.X);
                            rect.Y = (int)((-this.m_rectdGlassArea.Y - num2 / 2.0 + this.m_GlassRecipe.GLASS_Y / 2.0) * this.m_dZoomRatio.Y);
                            rect.Width = (int)(num2 * this.m_dZoomRatio.X);
                            rect.Height = (int)(num2 * this.m_dZoomRatio.Y);
                            if (this.m_rectSelect != rect)
                            {
                                this.m_SelectPath.AddEllipse(this.m_rectSelect);
                                this.m_SelectPath.AddEllipse(rect);
                                break;
                            }
                            break;
                        case MapSelectStyle.Pie:
                            PointD realPoint3 = this.GetRealPoint(this.m_poStart.X, this.m_poStart.Y);
                            PointD realPoint4 = this.GetRealPoint(this.m_poEnd.X, this.m_poEnd.Y);
                            double num3 = realPoint3.Y <= 0.0 ? Math.Acos(realPoint3.X / Math.Sqrt(Math.Pow(realPoint3.X, 2.0) + Math.Pow(realPoint3.Y, 2.0))) * (180.0 / Math.PI) % 360.0 : -Math.Acos(realPoint3.X / Math.Sqrt(Math.Pow(realPoint3.X, 2.0) + Math.Pow(realPoint3.Y, 2.0))) * (180.0 / Math.PI) % 360.0;
                            double num4 = realPoint4.Y <= 0.0 ? Math.Acos(realPoint4.X / Math.Sqrt(Math.Pow(realPoint4.X, 2.0) + Math.Pow(realPoint4.Y, 2.0))) * (180.0 / Math.PI) % 360.0 : -Math.Acos(realPoint4.X / Math.Sqrt(Math.Pow(realPoint4.X, 2.0) + Math.Pow(realPoint4.Y, 2.0))) * (180.0 / Math.PI) % 360.0;
                            this.m_rectSelect.X = (int)(-this.m_rectdGlassArea.X * this.m_dZoomRatio.X);
                            this.m_rectSelect.Y = (int)(-this.m_rectdGlassArea.Y * this.m_dZoomRatio.Y);
                            this.m_rectSelect.Width = (int)(this.m_GlassRecipe.GLASS_X * this.m_dZoomRatio.X);
                            this.m_rectSelect.Height = (int)(this.m_GlassRecipe.GLASS_Y * this.m_dZoomRatio.Y);
                            this.m_SelectPath.AddPie(this.m_rectSelect, (float)num3, (float)((num4 - num3 + 720.0) % 360.0));
                            break;
                        case MapSelectStyle.Rectangle:
                            if (this.m_rectSelect.Width * this.m_rectSelect.Height > 0)
                            {
                                this.m_SelectPath.AddRectangle(this.m_rectSelect);
                                break;
                            }
                            break;
                    }
                    solidBrush = new SolidBrush(Color.FromArgb(80, Color.Tomato));
                    g.FillPath((Brush)solidBrush, this.m_SelectPath);
                    g.DrawPath(pen, this.m_SelectPath);
                }
            }
            finally
            {
                if (pen != null)
                    pen.Dispose();
                if (solidBrush != null)
                    solidBrush.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicsPath path = (GraphicsPath)null;
            SolidBrush solidBrush1 = (SolidBrush)null;
            SolidBrush solidBrush2 = (SolidBrush)null;
            Pen pen1 = (Pen)null;
            Pen pen2 = (Pen)null;
            Font font = new Font("Tahoma", Math.Max(8f, (float)(this.m_dZoomRatio.X * 2.29999995231628)));
            PointD[] pointDArray = new PointD[4]
      {
        new PointD()
        {
          X = -this.m_GlassRecipe.GLASS_X / 2.0,
          Y = -this.m_GlassRecipe.GLASS_Y / 2.0
        },
        new PointD()
        {
          X = this.m_GlassRecipe.GLASS_X / 2.0,
          Y = -this.m_GlassRecipe.GLASS_Y / 2.0
        },
        new PointD()
        {
          X = this.m_GlassRecipe.GLASS_X / 2.0,
          Y = this.m_GlassRecipe.GLASS_Y / 2.0
        },
        new PointD()
        {
          X = -this.m_GlassRecipe.GLASS_X / 2.0,
          Y = this.m_GlassRecipe.GLASS_Y / 2.0
        }
      };
            try
            {
                path = new GraphicsPath();
                PointF[] points = new PointF[pointDArray.Length];
                for (int index = 0; index < points.Length; ++index)
                {
                    points[index].X = (float)((-this.m_rectdGlassArea.X + this.m_GlassRecipe.GLASS_X / 2.0 + pointDArray[index].X) * this.m_dZoomRatio.X);
                    points[index].Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y / 2.0 - pointDArray[index].Y) * this.m_dZoomRatio.Y);
                }
                path.AddLines(points);
                path.CloseFigure();
                solidBrush2 = new SolidBrush(this.m_colGlass);
                pen2 = new Pen(this.m_colGlassBorder);
                graphics.FillPath((Brush)solidBrush2, path);
                graphics.DrawPath(pen2, path);
                if (this.m_strInfomation != null && this.m_bVisibleInfo)
                {
                    for (int index = 0; index < this.m_strInfomation.Count; ++index)
                        graphics.DrawString(this.m_strInfomation[index], font, (Brush)new SolidBrush(Color.Black), //(float)((-this.m_rectdGlassArea.X + 2.0) * this.m_dZoomRatio.X), (float)((-this.m_rectdGlassArea.Y + 2.0) * this.m_dZoomRatio.Y) + (float)(font.Height * index));
                            2, Height - font.Height * m_strInfomation.Count + (font.Height * index) - 2 - (StatusBar.Visible ? StatusBar.Height : 0));
                }
                if (!this.m_isDrag)
                    return;
                switch (this.m_eoMouseDragMode)
                {
                    case MouseDragMode.Zoom:
                        this.DrawSelectRect(graphics);
                        break;
                    case MouseDragMode.Move:
                        this.DrawMoveRect(graphics);
                        break;
                    case MouseDragMode.Zone:
                    case MouseDragMode.Defect:
                        this.DrawZoneGuid(graphics);
                        break;
                }
            }
            finally
            {
                if (path != null)
                    path.Dispose();
                if (solidBrush1 != null)
                    solidBrush1.Dispose();
                if (pen1 != null)
                    pen1.Dispose();
                if (solidBrush2 != null)
                    solidBrush2.Dispose();
                if (pen2 != null)
                    pen2.Dispose();
                if (font != null)
                    font.Dispose();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.DesignMode)
                return;

            if (this.m_eoMapMode == MapMode.Free)
            {
                PointD pointD = new PointD(this.m_rectdGlassArea.X + this.m_rectdGlassArea.Width / 2.0, this.m_rectdGlassArea.Y + this.m_rectdGlassArea.Height / 2.0);
                this.m_rectdGlassArea.Width = (double)this.GetWidth() / this.m_dZoomRatio.X;
                this.m_rectdGlassArea.Height = (double)this.GetHeight() / this.m_dZoomRatio.Y;
                this.m_rectdGlassArea.X = pointD.X - this.m_rectdGlassArea.Width / 2.0;
                this.m_rectdGlassArea.Y = pointD.Y - this.m_rectdGlassArea.Height / 2.0;
            }
            else if (this.m_eoMapMode == MapMode.Fit)
            {
                double num1 = 0.0;
                double num2 = 0.0;
                int width = this.Width;
                int height = this.Height;
                if (this.MapFitMode == MapFitMode.DisplayAll)
                {
                    if (this.m_GlassRecipe.GLASS_X / (double)this.GetWidth() > this.m_GlassRecipe.GLASS_Y / (double)this.GetHeight())
                    {
                        num1 = (double)this.GetWidth() * 0.85;
                        this.m_dZoomRatio.X = num1 / this.m_GlassRecipe.GLASS_X;
                        num2 = this.m_GlassRecipe.GLASS_Y * this.m_dZoomRatio.X;
                        this.m_dZoomRatio.Y = num2 / this.m_GlassRecipe.GLASS_Y;
                    }
                    else
                    {
                        num2 = (double)this.GetHeight() * 0.85;
                        this.m_dZoomRatio.Y = num2 / this.m_GlassRecipe.GLASS_Y;
                        num1 = this.m_GlassRecipe.GLASS_X * this.m_dZoomRatio.Y;
                        this.m_dZoomRatio.X = num1 / this.m_GlassRecipe.GLASS_X;
                    }
                }
                else if (this.MapFitMode == MapFitMode.Expand)
                {
                    num1 = (double)this.GetWidth() * 0.85;
                    num2 = (double)this.GetHeight() * 0.85;
                    this.m_dZoomRatio.X = num1 / this.m_GlassRecipe.GLASS_X;
                    this.m_dZoomRatio.Y = num2 / this.m_GlassRecipe.GLASS_Y;
                }
                this.m_rectdGlassArea = new RectangleD((num1 - (double)this.GetWidth()) / 2.0 / this.m_dZoomRatio.X - (this.ShowGridLine ? 20.0 : 0.0) / this.m_dZoomRatio.X, (num2 - (double)this.GetHeight()) / 2.0 / this.m_dZoomRatio.Y + (this.ShowGridLine ? 25.0 : 0.0) / this.m_dZoomRatio.Y, (double)this.GetWidth() / this.m_dZoomRatio.X, (double)this.GetHeight() / this.m_dZoomRatio.Y);
            }
            this.Invalidate();
            if (this.m_eoMapMode == MapMode.Free)
            {
                this.SetDelayViewChanged();
            }
            else
            {
                if (this.m_eoMapMode != MapMode.Fit)
                    return;
                this.SetDelayViewChanged();
            }
        }

        /// <summary>Galss를 다시 Drawing 합니다.</summary>
        public virtual void Redraw()
        {
            this.Refresh();
        }

        protected virtual void SetDefaultColor()
        {
            int[] numArray = new int[101]
      {
        16766976,
        65281,
        4194432,
        32896,
        8421631,
        8404992,
        16744703,
        64,
        (int) ushort.MaxValue,
        32768,
        65280,
        16448,
        12615680,
        16711808,
        16776960,
        4210816,
        4227327,
        12615808,
        14817052,
        8454016,
        16744448,
        4194432,
        16744576,
        12615935,
        8388863,
        16777088,
        33023,
        65408,
        4227072,
        16711680,
        10485760,
        8388736,
        (int) byte.MaxValue,
        8454143,
        16512,
        16384,
        4210688,
        8388608,
        4194304,
        4194368,
        8388672,
        8453888,
        32896,
        4227200,
        8421504,
        4227136,
        12632256,
        9868950,
        6795178,
        4446555,
        6069641,
        0,
        4259584,
        7242348,
        7552844,
        2613922,
        11377144,
        8723452,
        3745060,
        6005922,
        12720820,
        2615727,
        6475744,
        8781431,
        13882444,
        273280,
        13391644,
        13317614,
        15138683,
        13399885,
        10731647,
        8333305,
        7428478,
        5658018,
        12090672,
        6983060,
        5689642,
        1055945,
        1918105,
        5796598,
        2192459,
        10882320,
        267359,
        14939131,
        8427326,
        16100701,
        14189729,
        8599505,
        148945,
        3303408,
        10243695,
        14070241,
        1305178,
        5595511,
        8657805,
        2921728,
        3620885,
        13344342,
        13733736,
        5609686,
        2521304
      };
            if (this.m_ColorSet != null)
                this.m_ColorSet = (Color[])null;
            this.m_ColorSet = new Color[numArray.Length];
            for (int index = 0; index < numArray.Length; ++index)
            {
                int red = numArray[index] >> 16;
                int green = (numArray[index] >> 8) - red * 256;
                int blue = numArray[index] - red * 65536 - green * 256;
                this.m_ColorSet[index] = Color.FromArgb(red, green, blue);
            }
        }

        /// <summary>Bin별 Color를 재정의합니다.</summary>
        /// <param name="Index">BIN Index</param>
        /// <param name="BinColor">Color object</param>
        public void SetColor(int Index, Color BinColor)
        {
            this.m_ColorSet[Index] = BinColor;
        }

        /// <summary>Bin별 Color 및 투명도를 재정의합니다.</summary>
        /// <param name="Index">BIN Index</param>
        /// <param name="BinColor">Color object</param>
        /// <param name="Alpha">투명도</param>
        public void SetColor(int Index, Color BinColor, int Alpha)
        {
            this.m_ColorSet[Index] = Color.FromArgb(Alpha, BinColor);
        }

        /// <summary>Bin Index의 Color을 가져옵니다.</summary>
        /// <param name="Index">BIN Index</param>
        /// <returns></returns>
        public Color GetColor(int Index)
        {
            return this.m_ColorSet[Index];
        }

        protected virtual void mnuitemMAPMODE_FITSIZE_Click(object sender, EventArgs e)
        {
            MapMode eoMapMode = this.m_eoMapMode;
            this.m_eoMapMode = MapMode.Fit;
            this.OnResize(EventArgs.Empty);
            this.MenuManagment();
            this.m_eoMapMode = eoMapMode;
        }

        protected virtual void mnuitemMAPMODE_FREEZOOM_Click(object sender, EventArgs e)
        {
            this.m_eoMapMode = MapMode.Free;
            this.m_eoMouseDragMode = MouseDragMode.Zoom;
            this.mnuitemMAPMODE_FREEZOOM.Checked = true;
            this.mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            this.mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            this.MenuManagment();
        }

        protected virtual void mnuitemMOUSEDRAGMODE_ROTATE_Click(object sender, EventArgs e)
        {
            this.m_eoMouseDragMode = MouseDragMode.Rotate;
            this.mnuitemMAPMODE_FREEZOOM.Checked = false;
            this.mnuitemMOUSEDRAGMODE_ROTATE.Checked = true;
            this.mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            this.MenuManagment();
        }

        protected virtual void mnuitemMOUSEDRAGMODE_MOVE_Click(object sender, EventArgs e)
        {
            this.m_eoMouseDragMode = MouseDragMode.Move;
            this.mnuitemMAPMODE_FREEZOOM.Checked = false;
            this.mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            this.mnuitemMOUSEDRAGMODE_MOVE.Checked = true;
            this.MenuManagment();
        }

        protected virtual void mnuitemMAPSELECTSTYLE_CIRCLE_Click(object sender, EventArgs e)
        {
            this.m_eoMapSelectStyle = MapSelectStyle.Circle;
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Checked = true;
            this.mnuitemMAPSELECTSTYLE_PIE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            this.mnuitemMAPSELECTSTYLE_BAND.Checked = false;
        }

        protected virtual void mnuitemMAPSELECTSTYLE_PIE_Click(object sender, EventArgs e)
        {
            this.m_eoMapSelectStyle = MapSelectStyle.Pie;
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_PIE.Checked = true;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            this.mnuitemMAPSELECTSTYLE_BAND.Checked = false;
        }

        protected virtual void mnuitemMAPSELECTSTYLE_RECTANGLE_Click(object sender, EventArgs e)
        {
            this.m_eoMapSelectStyle = MapSelectStyle.Rectangle;
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_PIE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = true;
            this.mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            this.mnuitemMAPSELECTSTYLE_BAND.Checked = false;
        }

        protected virtual void mnuitemMAPSELECTSTYLE_FREEHAND_Click(object sender, EventArgs e)
        {
            this.m_eoMapSelectStyle = MapSelectStyle.FreeHand;
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_PIE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_FREEHAND.Checked = true;
            this.mnuitemMAPSELECTSTYLE_BAND.Checked = false;
        }

        protected virtual void mnuitemMAPSELECTSTYLE_BAND_Click(object sender, EventArgs e)
        {
            this.m_eoMapSelectStyle = MapSelectStyle.Band;
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_PIE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            this.mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            this.mnuitemMAPSELECTSTYLE_BAND.Checked = true;
        }

        protected void MenuManagment()
        {
        }

        private void menuitemMAPMODE_ZOOMIN_Click(object sender, EventArgs e)
        {
            this.ZoomIn();
        }

        private void menuitemMAPMODE_ZOOMOUT_Click(object sender, EventArgs e)
        {
            this.ZoomOut();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta < 0)
                this.ZoomOut(e.Location);
            else
                this.ZoomIn(e.Location);
        }

        private void mnuCOPYTOCLIP_Click(object sender, EventArgs e)
        {
        }

        public Bitmap GetMapImage()
        {
            return (Bitmap)null;
        }

        private void mnuitemCLEAR_SELECTEDDIE_Click(object sender, EventArgs e)
        {
        }

        private void MMGBinMap_Simple_Load(object sender, EventArgs e)
        {
            int num = this.DesignMode ? 1 : 0;
        }

        public void SetSelectedModel(string ModelID)
        {
            foreach (CQPanel qpanel in this.m_GlassRecipe.QPanels)
            {
                foreach (CPanel panel in qpanel.Panels)
                    panel.PanelProperty = !(panel.MODEL_ID == ModelID) ? panel.PanelProperty & 254 : panel.PanelProperty | 1;
            }
            this.m_bVisibleSelectedModel = true;
            this.Redraw();
        }

        public void DeleteModel(string ModelID)
        {
            foreach (CQPanel qpanel in this.m_GlassRecipe.QPanels)
            {
                List<CPanel> cpanelList = new List<CPanel>();
                foreach (CPanel panel in qpanel.Panels)
                {
                    if (panel.MODEL_ID == ModelID)
                        cpanelList.Add(panel);
                }
                for (int index = 0; index < cpanelList.Count; ++index)
                    qpanel.Panels.Remove(cpanelList[index]);
            }
            foreach (CModel model in this.m_GlassRecipe.Models)
            {
                if (model.MODEL_ID == ModelID)
                {
                    this.m_GlassRecipe.Models.Remove(model);
                    break;
                }
            }
            this.Redraw();
        }

        protected Rectangle GetBounds()
        {
            return new Rectangle(160, 120, this.Width - 160 - 20, this.Height - 120 - 50 - (this.StatusBar.Visible ? this.StatusBar.Height : 0));
        }

        private void mnuitemSELECTIONMODE_ALL_Click(object sender, EventArgs e)
        {
            this.mnuitemSELECTIONMODE_ALL.Checked = true;
            this.mnuitemSELECTIONMODE_QMODE.Checked = false;
        }

        private void mnuitemSELECTIONMODE_QMODE_Click(object sender, EventArgs e)
        {
            this.mnuitemSELECTIONMODE_ALL.Checked = false;
            this.mnuitemSELECTIONMODE_QMODE.Checked = true;
        }

        public void FitSize()
        {
            this.mnuitemMAPMODE_FITSIZE_Click(null, (EventArgs)null);
        }

        /// <summary>
        /// Load 이벤트가 강제로 실행되도록 합니다.
        /// </summary>
        public void ForceLoadEvent()
        {
            Bitmap bmp = new Bitmap(Width, Height);
            DrawToBitmap(bmp, ClientRectangle);
        }

        public void AddContextMenuItem(string itemName, EventHandler handler)
        {
            ctxmGlassMap.MenuItems.Add(itemName, handler);
        }
    }
}
