using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using System.Collections;
using System.Drawing.Drawing2D;

namespace DACrux.DMSVFPD.Map
{
    public partial class MMGDefectMap_Simple : MMGBinMap_Simple, DACrux.Map.IDefectMap, DACrux.Map.IShot
    {
        public static readonly string DEFECT_ALL = "ALL";
        public const float DEFAULT_DEFECT_SIZE = 2;

        public event EventHandler ShotOptionChanged;
        
        private MAP_TYPE m_mapType = MAP_TYPE.CLASS;
        private bool m_bImageMark = true;
        protected double m_dMaxSize = 1000.0;

        private MMGDefectMap_Simple.ColorBySize[] m_SizeColor;
        private Color[] m_DefectColor;

        protected DefectList m_arrDefect;
        protected ArrayList m_arrSelDefectNumber;
        //protected ArrayList m_DefectIndexer;
        //protected ArrayList m_DefectClickIndexer;
        
        private bool m_bRealSize;
        private bool m_bVisibleShape;
        private DefectList m_SelectedDefect;
        private MenuItem mnuREALSIZEDEFECT;
        private MenuItem mnuitemMOUSEDRAGMODE_DEFECT;
        protected Dictionary<int, string> m_strArrShape;
        protected new Color[] m_ColorSet;
        private bool m_bVisibleDefectInRange;
        protected double m_dMinSize;
        private Font m_fntGID;
        private string m_drawDefects;
        private string m_drawDefectType;

        /// <summary>Defect Map의 생성자 임</summary>
        public MMGDefectMap_Simple()
        {
            this.InitializeComponent();
            this.ResetSelDefectNumber();
            this.SetDefaultColor();
            this.Reset();
            this.ctxmGlassMap.MenuItems.Add(this.mnuitemMOUSEDRAGMODE_DEFECT);
            this.ctxmGlassMap.MenuItems.Add(this.mnuREALSIZEDEFECT);
            this.ctxmGlassMap.MenuItems.Add(this.menuItemDefectMapOption);
            this.m_drawDefects = MMGDefectMap_Simple.DEFECT_ALL;
            this.m_drawDefectType = MMGDefectMap_Simple.DEFECT_ALL;
            this.m_strArrShape = new Dictionary<int, string>();
            this.ShapeItems = new List<MMGDefectMap_Simple.ShapeItem>();
            this.DisplayDefectList = new List<Defect>();
            this.mnuitemMOUSEDRAGMODE_ROTATE.Visible = false;
            this.BackColor = Color.White;
            this.AddedClassNumberList = new List<MMGDefectMap_Simple.ClassNumberInfo>();
            this.HighlightDefectColor = Color.Blue;

            IsShotMap = false;
            ShotArrayX = ShotArrayY = 1;
            ShotStartX = ShotStartY = 1;
            DefectSize = DEFAULT_DEFECT_SIZE;

            DisplayXUnit = DisplayYUnit = DACrux.Base.LengthConverter.Unit.um.ToString();
            DensityUnit = DACrux.Base.LengthConverter.Unit.cm.ToString();
        }

        [Category("FPDPanel 속성")]
        [Description("Map Type을 정해준다(color by CLASS, SIZE, ROUGHBIN, FINEBIN, CLUSTER)")]
        public MAP_TYPE MapType
        {
            get
            {
                return this.m_mapType;
            }
            set
            {
                this.m_mapType = value;
            }
        }

        public DataTable SizeColor
        {
            set
            {
                if (value == null || value.Rows.Count == 0 || (value.Columns.Count == 0 || !value.Columns[0].Caption.Equals("SIZE_FROM")) || (!value.Columns[1].Caption.Equals("SIZE_TO") || !value.Columns[2].Caption.Equals("COLOR")))
                    return;
                this.m_SizeColor = (MMGDefectMap_Simple.ColorBySize[])null;
                this.m_SizeColor = new MMGDefectMap_Simple.ColorBySize[value.Rows.Count];
                string empty = string.Empty;
                for (int index = 0; index < value.Rows.Count; ++index)
                {
                    this.m_SizeColor[index].from = Int32.Parse(value.Rows[index]["SIZE_FROM"].ToString());
                    this.m_SizeColor[index].to = Int32.Parse(value.Rows[index]["SIZE_TO"].ToString());
                    string str = value.Rows[index]["COLOR"].ToString();
                    this.m_SizeColor[index].color = ColorTranslator.FromHtml(str);
                }
            }
        }

        public DataTable TypeColor
        {
            set
            {
                if (value == null || value.Rows.Count == 0 || (value.Columns.Count == 0 || !value.Columns[0].Caption.Equals("CLASSNUMBER")) || (!value.Columns[1].Caption.Equals("COLOR") || this.m_DefectColor == null || this.m_DefectColor.Length == 0))
                    return;
                for (int index = 0; index < this.m_DefectColor.Length; ++index)
                    this.m_DefectColor[index] = Color.Black;
                string empty = string.Empty;
                for (int index = 0; index < value.Rows.Count; ++index)
                {
                    string str = (string)value.Rows[index]["COLOR"];
                    this.m_DefectColor[Int32.Parse(value.Rows[index]["CLASSNUMBER"].ToString())] = ColorTranslator.FromHtml(str);
                }
            }
        }

        public DefectList Defects
        {
            get
            {
                return this.m_arrDefect;
            }
        }

        /// <summary>컨트롤의 배경색을 가져오거나 설정합니다.</summary>
        [Description("구성 요소의 배경색 입니다.")]
        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        public bool VisibleShape
        {
            get
            {
                return this.m_bVisibleShape;
            }
            set
            {
                this.m_bVisibleShape = value;
            }
        }

        public bool VisibleImageMark
        {
            get
            {
                return this.m_bImageMark;
            }
            set
            {
                this.m_bImageMark = value;
                this.Redraw();
            }
        }

        public bool RealSizeDefectDrawing
        {
            get
            {
                return this.m_bRealSize;
            }
            set
            {
                this.m_bRealSize = value;
                this.Redraw();
            }
        }

        public bool VisibleDefectInRange
        {
            get
            {
                return this.m_bVisibleDefectInRange;
            }
            set
            {
                this.m_bVisibleDefectInRange = value;
                this.Redraw();
            }
        }

        /// <summary>추가된 CLASSNUMBER에 대한 값을 가져옵니다.</summary>
        public List<MMGDefectMap_Simple.ClassNumberInfo> AddedClassNumberList { get; private set; }

        /// <summary>
        /// Map에 그릴 Defect을 지정합니다. ALL인 경우 모든 Defect을 그리고, 특정 Defect만 그릴 경우 CLASSNUMBER를 쉼표를 이용해서 (예. 1,2,4) 적어줍니다.
        /// </summary>
        [DefaultValue("ALL")]
        [Description("Map에 그릴 Defect을 지정합니다. ALL인 경우 모든 Defect을 그리고, 특정 Defect만 그릴 경우 CLASSNUMBER를 쉼표를 이용해서 (예. 1,2,4) 적어줍니다.")]
        public string DrawDefects
        {
            get
            {
                return this.m_drawDefects;
            }
            set
            {
                this.m_drawDefects = value;
                this.Redraw();
                this.OnViewChanged(EventArgs.Empty);
                this.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            }
        }

        /// <summary>
        /// Map에 그릴 Defect을 지정합니다. ALL인 경우 모든 Defect을 그리고, 특정 Defect만 그릴 경우 CLASSNUMBER를 쉼표를 이용해서 (예. 1,2,4) 적어줍니다.
        /// </summary>
        [DefaultValue("ALL")]
        [Description("Map에 그릴 Defect Type을 지정합니다. ALL인 경우 모든 Defect을 그리고, 특정 Defect만 그릴 경우 DEFECT_TYPE을 쉼표를 이용해서 (예. MONITORING) 적어줍니다.")]
        public string DrawDefectType
        {
            get
            {
                return this.m_drawDefectType;
            }
            set
            {
                this.m_drawDefectType = value;
                this.Redraw();
                this.OnViewChanged(EventArgs.Empty);
            }
        }

        /// <summary>X 방향 눈금선의 갯수를 설정하거나 가져옵니다.</summary>
        [Description("X 방향 눈금선의 갯수를 설정하거나 가져옵니다.")]
        [DefaultValue(0)]
        public int GridLineXCount { get; set; }

        /// <summary>Y 방향 눈금선의 갯수를 설정하거나 가져옵니다.</summary>
        [Description("Y 방향 눈금선의 갯수를 설정하거나 가져옵니다.")]
        [DefaultValue(0)]
        public int GridLineYCount { get; set; }

        /// <summary>X 방향 눈금의 단위가 되는 값을 설정하거나 가져옵니다.</summary>
        [DefaultValue(0)]
        [Description("X 방향 눈금의 단위가 되는 값을 나타냅니다.")]
        public int GridLineXUnit { get; set; }

        /// <summary>Y 방향 눈금의 단위가 되는 값을 설정하거나 가져옵니다.</summary>
        [Description("Y 방향 눈금의 단위가 되는 값을 나타냅니다.")]
        [DefaultValue(0)]
        public int GridLineYUnit { get; set; }

        /// <summary>Map에 표시되는 Defect의 모양을 설정하거나 가져옵니다.</summary>
        [DefaultValue(typeof(DefectShape), "Rectangle")]
        [Description("Map에 표시되는 Defect의 모양을 나타냅니다.")]
        public DefectShape DefaultDefectShape { get; set; }

        /// <summary>X축의 단위 정보를 설정하거나 가져옵니다.</summary>
        [Description("X축의 단위 정보를 설정하거나 가져옵니다.")]
        [DefaultValue("um")]
        public string DisplayXUnit { get; set; }

        /// <summary>Y축의 단위 정보를 설정하거나 가져옵니다.</summary>
        [Description("Y축의 단위 정보를 설정하거나 가져옵니다.")]
        [DefaultValue("um")]
        public string DisplayYUnit { get; set; }

        /// <summary>X축의 단위 정보를 설정하거나 가져옵니다.</summary>
        [Description("DPA 계산시 사용되는 단위 정보를 설정하거나 가져옵니다.")]
        [DefaultValue("cm")]
        public string DensityUnit { get; set; }

        /// <summary>화면에 보여지는 Defect List를 가져옵니다.</summary>
        [Browsable(false)]
        public List<Defect> DisplayDefectList { get; private set; }

        /// <summary>Rotate Notch 기능을 표시할지를 설정하거나 가져옵니다.</summary>
        [Browsable(false)]
        public bool Enabled_RotateNotch { get; set; }

        /// <summary>선택되거나 활성화된 Defect의 테두리 혹은 Cross Line 색상을 설정하거나 가져옵니다.</summary>
        [DefaultValue(typeof(Color), "Blue")]
        public Color HighlightDefectColor { get; set; }

        /// <summary>CLASSNUMBER에 대한 정보를 MAP 하단에 보여줄지를 설정하거나 가져옵니다.</summary>
        [DefaultValue(true)]
        [Description("CLASSNUMBER에 대한 정보를 MAP 하단에 보여줄지를 나타냅니다.")]
        public bool ShowClassNumberInfo { get; set; }

        /// <summary>Map에 추가적으로 보여줄 도형 리스트를 가져옵니다.</summary>
        [Browsable(false)]
        public List<MMGDefectMap_Simple.ShapeItem> ShapeItems { get; private set; }

        public event MMGSelectedDefect OnMMGSelectedDefect;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (m_eoMouseDragMode == MouseDragMode.Defect)
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    m_SelectedDefect.Clear();

                    foreach (Defect defect in m_arrDefect)
                    {
                        if (defect.Visible)
                            m_SelectedDefect.Add(defect);
                    }

                    Redraw();

                    if (OnMMGSelectedDefect != null)
                        OnMMGSelectedDefect(this, m_SelectedDefect.ToArray());

                    return;
                }
            }

            base.OnKeyDown(e);
        }

        public void SetFunction(MMGDefectMap_Simple.Function func)
        {
            if (func == MMGDefectMap_Simple.Function.FreeZoom)
                this.mnuitemMAPMODE_FREEZOOM_Click(null, EventArgs.Empty);
            else if (func == MMGDefectMap_Simple.Function.Move)
            {
                this.mnuitemMOUSEDRAGMODE_MOVE_Click(null, EventArgs.Empty);
            }
            else
            {
                if (func != MMGDefectMap_Simple.Function.DefectSelect)
                    return;
                this.mnuitemMOUSEDRAGMODE_DEFECT_Click(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Map Option 변경을 알립니다.
        /// </summary>
        protected virtual void OnShotOptionChanged(EventArgs e)
        {
            if (ShotOptionChanged != null)
                ShotOptionChanged(this, e);
        }

        public void SetDefectColor(int index, string desc, Color color)
        {
            this.m_DefectColor[index] = color;
            this.AddedClassNumberList.Add(new MMGDefectMap_Simple.ClassNumberInfo()
            {
                ClassNumber = index,
                Info = desc,
                Color = color
            });
        }

        public void SetDefectColor(int Index, Color oCol)
        {
            this.m_DefectColor[Index] = oCol;
        }

        public void ClearDefectColor()
        {
            this.AddedClassNumberList.Clear();
        }

        /// <summary>전체 Defect을 삭제한다.</summary>
        public void DefectClear()
        {
            this.m_arrDefect.Clear();
            //this.m_DefectIndexer.Clear();
        }

        /// <summary>Defect을 추가한다.</summary>
        /// <param name="NewDefect">Defect 구조체</param>
        public void AddDefect(Defect NewDefect)
        {
            var idx = m_arrDefect.BinarySearch(NewDefect);

            if (idx >= 0)
                m_arrDefect.RemoveAt(idx);
            else
                m_arrDefect.Insert(~idx, NewDefect);

            //this.m_DefectIndexer.Add(new PointF((float)NewDefect.X, (float)NewDefect.Y));
            //this.m_DefectClickIndexer.Add(new Point((int)NewDefect.X, (int)NewDefect.Y));
        }

        public void AddSelDefectNumber(int iDefect)
        {
            if (this.m_arrSelDefectNumber.IndexOf(iDefect) >= 0)
                return;
            this.m_arrSelDefectNumber.Add(iDefect);
        }

        public void DeleteSelDefectNumber(int iDefect)
        {
            this.m_arrSelDefectNumber.Remove(iDefect);
        }

        public void ResetSelDefectNumber()
        {
            this.m_arrSelDefectNumber = new ArrayList();
        }

        public void VisibleSizeChange(double dMax, double dMin)
        {
            this.m_dMaxSize = dMax;
            this.m_dMinSize = dMin;
            this.Redraw();
        }

        public void SetShape(int DefectType, string Shape)
        {
            this.m_strArrShape[DefectType] = Shape;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            this.DrawDefect(graphics);
            this.DrawGrid(graphics);
            this.DrawShotGrid(graphics);
            this.DrawClassNumberInfo(graphics);
            this.DrawShapeItems(graphics);
            this.DefectHighlight(graphics);
            this.DrawGridValue(graphics);
        }

        private bool IsVisibleDefect(Defect defect)
        {
            if (MapType == MAP_TYPE.CLASS)
            {
                return (this.DrawDefects == "ALL" || this.DrawDefects.Contains(defect.CLASSNUMBER.ToString())) &&
                (this.DrawDefectType == "ALL" || string.IsNullOrEmpty(defect.DEFECT_TYPE) || this.DrawDefectType.Contains(defect.DEFECT_TYPE));
            }
            else if (MapType == MAP_TYPE.RD)
            {
                return (this.DrawDefects == "ALL" || Array.IndexOf<string>(DACrux.Map.DefectMap.RD_ARRAY, DrawDefects) == defect.RD);
            }
            else
            {
                return this.DrawDefects == "ALL";
            }
        }

        /// <summary>Defect을 주어진 Graphics object에 Drawing합니다.</summary>
        /// <param name="g">Graphics Object</param>
        private void DrawDefect(Graphics g)
        {
            if (this.m_arrDefect == null || this.m_arrDefect.Count <= 0)
                return;

            PointD[] pointDArray = new PointD[4];
            PointF[] points = new PointF[4];
            SizeF empty = SizeF.Empty;
            RectangleF visibleClipBounds = g.VisibleClipBounds;
            this.DisplayDefectList.Clear();

            foreach (Defect defect in (List<Defect>)this.m_arrDefect)
            {
                if (this.IsVisibleDefect(defect) && (this.m_arrSelDefectNumber.Count == 0 || this.m_arrSelDefectNumber.IndexOf(defect.CLASSNUMBER) >= 0) && (!this.m_bVisibleDefectInRange || this.m_dMaxSize >= defect.DSIZE && this.m_dMinSize <= defect.DSIZE))
                {
                    double fx, fy;
                    GetDefectXY(defect, out fx, out fy);

                    pointDArray[0].X = fx;
                    pointDArray[0].Y = fy;
                    pointDArray[1].X = fx + defect.XSIZE;
                    pointDArray[1].Y = fy;
                    pointDArray[2].X = fx + defect.XSIZE;
                    pointDArray[2].Y = fy + defect.YSIZE;
                    pointDArray[3].X = fx;
                    pointDArray[3].Y = fy + defect.YSIZE;

                    points[0].X = (float)((-this.m_rectdGlassArea.X + pointDArray[0].X) * this.m_dZoomRatio.X);
                    points[0].Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - pointDArray[0].Y) * this.m_dZoomRatio.Y);
                    points[1].X = (float)((-this.m_rectdGlassArea.X + pointDArray[1].X) * this.m_dZoomRatio.X);
                    points[1].Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - pointDArray[1].Y) * this.m_dZoomRatio.Y);
                    points[2].X = (float)((-this.m_rectdGlassArea.X + pointDArray[2].X) * this.m_dZoomRatio.X);
                    points[2].Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - pointDArray[2].Y) * this.m_dZoomRatio.Y);
                    points[3].X = (float)((-this.m_rectdGlassArea.X + pointDArray[3].X) * this.m_dZoomRatio.X);
                    points[3].Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - pointDArray[3].Y) * this.m_dZoomRatio.Y);

                    if (visibleClipBounds.Contains(points[0].X - DefectSize / 2f, points[0].Y - DefectSize / 2f))
                    {
                        this.DisplayDefectList.Add(defect);
                        if (this.m_bVisibleShape)
                        {
                            g.DrawString(this.m_strArrShape[defect.CLASSNUMBER], this.m_fntGID, (Brush)new SolidBrush(this.DefectColor(defect)), points[0]);
                        }
                        else
                        {
                            if (this.m_bRealSize)
                            {
                                g.FillPolygon((Brush)new SolidBrush(this.DefectColor(defect)), points);
                            }
                            else if (this.DefaultDefectShape == DefectShape.Rectangle)
                            {
                                if (!defect.EmptyShape)
                                    g.FillRectangle((Brush)new SolidBrush(this.DefectColor(defect)), new RectangleF(points[0].X - DefectSize / 2f, points[0].Y - DefectSize / 2f, DefectSize, DefectSize));
                                else
                                    g.DrawRectangle(new Pen(this.DefectColor(defect)), points[0].X - DefectSize / 2f, points[0].Y - DefectSize / 2f, DefectSize, DefectSize);
                            }
                            else if (this.DefaultDefectShape == DefectShape.Circle)
                            {
                                if (!defect.EmptyShape)
                                    g.FillEllipse((Brush)new SolidBrush(this.DefectColor(defect)), new RectangleF(points[0].X - DefectSize / 2f, points[0].Y - DefectSize / 2f, DefectSize, DefectSize));
                                else
                                    g.DrawEllipse(new Pen(this.DefectColor(defect)), points[0].X - DefectSize / 2f, points[0].Y - DefectSize / 2f, DefectSize, DefectSize);
                            }

                            if (defect.IMAGECOUNT > 0 && this.m_bImageMark)
                                g.DrawArc(new Pen(Color.Red), points[0].X - 3f, points[0].Y - 3f, 6f, 6f, 0.0f, 360f);
                        }
                    }
                }
            }
        }

        private void DrawGrid(Graphics g)
        {
            if (g == null || !this.ShowGridLine)
                return;
            Pen pen = new Pen(Color.Gray, 1f)
            {
                DashPattern = new float[2] { 4f, 2f }
            };
            StringFormat stringFormat1 = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
            StringFormat stringFormat2 = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            try
            {
                double num1 = this.Glass.GLASS_Y / (double)(this.GridLineYCount + 1);
                for (int index = 1; index <= this.GridLineYCount + 1; ++index)
                {
                    double num2 = 0.0;
                    double num3 = (double)index * num1;
                    double glassX = this.Glass.GLASS_X;
                    double num4 = (double)index * num1;
                    double num5 = (-this.m_rectdGlassArea.X + num2) * this.m_dZoomRatio.X;
                    double num6 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num3) * this.m_dZoomRatio.Y;
                    double num7 = (-this.m_rectdGlassArea.X + glassX) * this.m_dZoomRatio.X;
                    double num8 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num4) * this.m_dZoomRatio.Y;
                    if (index <= this.GridLineYCount)
                        g.DrawLine(pen, (float)num5, (float)num6, (float)num7, (float)num8);
                }
                double num9 = this.Glass.GLASS_X / (double)(this.GridLineXCount + 1);
                for (int index = 1; index <= this.GridLineXCount + 1; ++index)
                {
                    double num2 = (double)index * num9;
                    double num3 = 0.0;
                    double num4 = (double)index * num9;
                    double glassY = this.Glass.GLASS_Y;
                    double num5 = (-this.m_rectdGlassArea.X + num2) * this.m_dZoomRatio.X;
                    double num6 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num3) * this.m_dZoomRatio.Y;
                    double num7 = (-this.m_rectdGlassArea.X + num4) * this.m_dZoomRatio.X;
                    double num8 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - glassY) * this.m_dZoomRatio.Y;
                    if (index <= this.GridLineXCount)
                        g.DrawLine(pen, (float)num5, (float)num6, (float)num7, (float)num8);
                }
            }
            finally
            {
                if (pen != null)
                    pen.Dispose();
                if (stringFormat1 != null)
                    stringFormat1.Dispose();
                if (stringFormat2 != null)
                    stringFormat2.Dispose();
            }
        }

        private void DrawShotGrid(Graphics g)
        {
            if (g == null || !IsShotMap || (ShotArrayX == 1 && ShotArrayY == 1))
                return;

            Pen pen = new Pen(Color.Black, 1f);

            try
            {
                double num1 = this.Glass.GLASS_Y / (double)ShotArrayY;
                for (int index = 1; index <= ShotArrayY; index++)
                {
                    double num2 = 0.0;
                    double num3 = (double)index * num1;
                    double glassX = this.Glass.GLASS_X;
                    double num4 = (double)index * num1;
                    double num5 = (-this.m_rectdGlassArea.X + num2) * this.m_dZoomRatio.X;
                    double num6 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num3) * this.m_dZoomRatio.Y;
                    double num7 = (-this.m_rectdGlassArea.X + glassX) * this.m_dZoomRatio.X;
                    double num8 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num4) * this.m_dZoomRatio.Y;
                    //if (index < this.ShotCountY)
                    g.DrawLine(pen, (float)num5, (float)num6, (float)num7, (float)num8);
                }
                double num9 = this.Glass.GLASS_X / (double)this.ShotArrayX;
                for (int index = 1; index <= this.ShotArrayX; ++index)
                {
                    double num2 = (double)index * num9;
                    double num3 = 0.0;
                    double num4 = (double)index * num9;
                    double glassY = this.Glass.GLASS_Y;
                    double num5 = (-this.m_rectdGlassArea.X + num2) * this.m_dZoomRatio.X;
                    double num6 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num3) * this.m_dZoomRatio.Y;
                    double num7 = (-this.m_rectdGlassArea.X + num4) * this.m_dZoomRatio.X;
                    double num8 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - glassY) * this.m_dZoomRatio.Y;
                    //if (index <= this.ShotCountX)
                    g.DrawLine(pen, (float)num5, (float)num6, (float)num7, (float)num8);
                }
            }
            finally
            {
                if (pen != null)
                    pen.Dispose();
            }
        }

        /// <summary>
        /// Defect의 제일 작은 X,Y 인덱스를 가져옵니다.
        /// </summary>
        private void GetMinXYIndex(out int x, out int y)
        {
            x = Int32.MaxValue;
            y = Int32.MaxValue;

            foreach (Defect d in m_arrDefect)
            {
                x = Math.Min(d.XINDEX, x);
                y = Math.Min(d.YINDEX, y);
            }
        }

        private void DrawClassNumberInfo(Graphics g)
        {
            if (!this.ShowClassNumberInfo)
                return;
            float y = (float)(this.ClientRectangle.Height - 15 - (this.StatusBar.Visible ? this.StatusBar.Height : 0));
            float x1 = 20f;
            foreach (MMGDefectMap_Simple.ClassNumberInfo addedClassNumber in this.AddedClassNumberList)
            {
                g.FillEllipse((Brush)new SolidBrush(addedClassNumber.Color), x1, y, DefectSize, DefectSize);
                float x2 = x1 + DefectSize;
                g.DrawString(addedClassNumber.Info, this.Font, (Brush)new SolidBrush(addedClassNumber.Color), x2, y);
                x1 = x2 + g.MeasureString(addedClassNumber.Info, this.Font).Width + DefectSize;
            }
        }

        private void DrawShapeItems(Graphics g)
        {
            if (g == null || this.ShapeItems == null || this.ShapeItems.Count == 0)
                return;
            foreach (MMGDefectMap_Simple.ShapeItem shapeItem in this.ShapeItems)
            {
                double x = (double)shapeItem.Rectangle.X;
                double y = (double)shapeItem.Rectangle.Y;
                double num1 = (double)(shapeItem.Rectangle.X + shapeItem.Rectangle.Width);
                double num2 = (double)(shapeItem.Rectangle.Y + shapeItem.Rectangle.Height);
                double num3 = (-this.m_rectdGlassArea.X + x) * this.m_dZoomRatio.X;
                double num4 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - y) * this.m_dZoomRatio.Y;
                double num5 = (-this.m_rectdGlassArea.X + num1) * this.m_dZoomRatio.X;
                double num6 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num2) * this.m_dZoomRatio.Y;
                using (Pen pen = new Pen(shapeItem.Color, 2f))
                {
                    if (shapeItem.Shape == DefectShape.Rectangle)
                        g.DrawRectangle(pen, (float)num3, (float)num4, (float)num5 - (float)num3, (float)num4 - (float)num6);
                    else if (shapeItem.Shape == DefectShape.Circle)
                        g.DrawEllipse(pen, (float)num3, (float)num4, (float)num5 - (float)num3, (float)num4 - (float)num6);
                }
                if (!string.IsNullOrEmpty(this.Name))
                {
                    Font font = shapeItem.Font != null ? shapeItem.Font : this.Font;
                    using (Brush brush = (Brush)new SolidBrush(shapeItem.TextColor))
                        g.DrawString(shapeItem.DisplayText, font, brush, (float)num3 + 2f, (float)num4 + 3f);
                }
            }
        }

        private void DrawGridValue(Graphics g)
        {
            if (g == null || !this.ShowGridLine)
                return;
            Pen pen = new Pen(Color.Gray, 1f)
            {
                DashPattern = new float[2] { 4f, 2f }
            };
            StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
            StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            try
            {
                double num1 = this.Glass.GLASS_Y / (double)(this.GridLineYCount + 1);
                for (int index = 1; index <= this.GridLineYCount + 1; ++index)
                {
                    double num2 = 0.0;
                    double num3 = (double)index * num1;
                    double num4 = (-this.m_rectdGlassArea.X + num2) * this.m_dZoomRatio.X;
                    double num5 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num3) * this.m_dZoomRatio.Y;
                    string str = string.Format("{0:N0}", Math.Round((double)index * num1, 0));
                    SizeF sizeF = g.MeasureString(str, this.Font, PointF.Empty, stringFormat);
                    g.FillRectangle(Brushes.White, Math.Max(1f, (float)num4 - sizeF.Width), (float)num5 - sizeF.Height / 2f, sizeF.Width - 6f, sizeF.Height);
                    g.DrawString(str, this.Font, Brushes.Gray, Math.Max(1f, (float)num4 - sizeF.Width), (float)num5 + 3f, stringFormat);
                }
                double num6 = this.Glass.GLASS_X / (double)(this.GridLineXCount + 1);
                for (int index = 1; index <= this.GridLineXCount + 1; ++index)
                {
                    double num2 = (double)index * num6;
                    double num3 = 0.0;
                    double num4 = (-this.m_rectdGlassArea.X + num2) * this.m_dZoomRatio.X;
                    double num5 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num3) * this.m_dZoomRatio.Y;
                    float val2 = (float)(this.Height - 30 - (this.StatusBar.Visible ? this.StatusBar.Height : 0));
                    string str = string.Format("{0:N0}", Math.Round((double)index * num6, 0));
                    SizeF sizeF = g.MeasureString(str, this.Font, PointF.Empty, stringFormat);
                    g.FillRectangle(Brushes.White, (float)num4 - sizeF.Width / 2f, Math.Min((float)num5 + 10f, val2), sizeF.Width, sizeF.Height);
                    g.DrawString(str, this.Font, Brushes.Gray, (float)num4, Math.Min((float)num5 + 10f, val2) + 9f, format);
                }
                double num7 = 0.0;
                double num8 = 0.0;
                double num9 = (-this.m_rectdGlassArea.X + num7) * this.m_dZoomRatio.X;
                double num10 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num8) * this.m_dZoomRatio.Y;
                if (!string.IsNullOrEmpty(this.DisplayXUnit))
                {
                    SizeF sizeF = g.MeasureString(this.DisplayXUnit, this.Font);
                    g.DrawString(this.DisplayXUnit, this.Font, Brushes.Gray, new PointF((float)num9, (float)num10 + sizeF.Height), format);
                }
                if (string.IsNullOrEmpty(this.DisplayYUnit))
                    return;
                SizeF sizeF1 = g.MeasureString(this.DisplayYUnit, this.Font);
                g.DrawString(this.DisplayYUnit, this.Font, Brushes.Gray, new PointF((float)num9 - sizeF1.Width, (float)num10), format);
            }
            finally
            {
                if (pen != null)
                    pen.Dispose();
                if (stringFormat != null)
                    stringFormat.Dispose();
                if (format != null)
                    format.Dispose();
            }
        }

        private Color DefectColor(Defect d)
        {
            switch (this.m_mapType)
            {
                case MAP_TYPE.SIZE:
                    if (this.m_SizeColor == null)
                        return Color.Red;
                    for (int index = 0; index < this.m_SizeColor.Length; ++index)
                    {
                        if (d.DSIZE * 1000000.0 >= (double)this.m_SizeColor[index].from && d.DSIZE * 1000000.0 <= (double)this.m_SizeColor[index].to)
                            return this.m_SizeColor[index].color;
                    }
                    return Color.Red;
                case MAP_TYPE.CLASS:
                    return this.m_DefectColor[d.CLASSNUMBER];
                case MAP_TYPE.CLUSTER:
                    return this.m_DefectColor[d.CLUSTERNUMBER];
                case MAP_TYPE.FINEBIN:
                    return this.m_DefectColor[d.FINEBINNUMBER];
                case MAP_TYPE.ROUGHBIN:
                    return this.m_DefectColor[d.ROUGHBINNUMBER];
                case MAP_TYPE.RD:
                    return DACrux.Map.DefectMap.GetRDColor(d.RD);
                default:
                    return this.m_DefectColor[d.CLASSNUMBER];
            }
        }

        protected override void Reset()
        {
            base.Reset();

            if (this.m_arrDefect != null)
            {
                this.m_arrDefect.Clear();
                this.m_arrDefect = (DefectList)null;
            }
            //if (this.m_DefectIndexer != null)
            //{
            //    this.m_DefectIndexer.Clear();
            //    this.m_DefectIndexer = (ArrayList)null;
            //}
            if (this.AddedClassNumberList != null)
                this.AddedClassNumberList.Clear();
            //if (this.m_DefectClickIndexer != null)
            //{
            //    this.m_DefectClickIndexer.Clear();
            //    this.m_DefectClickIndexer = (ArrayList)null;
            //}
            if (this.Information != null)
                this.Information.Clear();
            if (this.ShapeItems != null)
                this.ShapeItems.Clear();
            this.m_arrDefect = new DefectList();
            this.m_arrDefect.CountChanged += new EventHandler(this.m_arrDefect_CountChanged);
            this.m_SelectedDefect = new DefectList();
            this.m_SelectedDefect.CountChanged += new EventHandler(this.m_SelectedDefect_CountChanged);
            //this.m_DefectIndexer = new ArrayList();
            //this.m_DefectClickIndexer = new ArrayList();
            this.m_fntGID = new Font("굴림", Math.Max(8f, Math.Min((float)(this.m_dZoomRatio.X * 1.5), 12f)));
        }

        private void m_arrDefect_CountChanged(object sender, EventArgs e)
        {
            this.StatusTotal = this.m_arrDefect.Count.ToString();
        }

        private void m_SelectedDefect_CountChanged(object sender, EventArgs e)
        {
            if (this.m_SelectedDefect.Count > 0)
                this.StatusMessage = string.Format("{0} defect(s) selected.", this.m_SelectedDefect.Count);
            else
                this.StatusMessage = (string)null;
        }

        protected override void SetDefaultColor()
        {
            base.SetDefaultColor();
            int[] numArray = new int[256]
      {
        16777215,
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
            this.m_DefectColor = new Color[numArray.Length];
            for (int index = 0; index < numArray.Length; ++index)
            {
                int red = numArray[index] >> 16;
                int green = (numArray[index] >> 8) - red * 256;
                int blue = numArray[index] - red * 65536 - green * 256;
                this.m_DefectColor[index] = Color.FromArgb(red, green, blue);
            }
        }

        private void mnuREALSIZEDEFECT_Click(object sender, EventArgs e)
        {
            this.m_bRealSize = this.mnuREALSIZEDEFECT.Checked = !this.mnuREALSIZEDEFECT.Checked;
            this.Redraw();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //this.m_SelectPath =  (GraphicsPath)null;
            this.m_SelectPath.Reset();
            base.OnMouseWheel(e);
        }

        protected override void OnResize(EventArgs e)
        {
            //this.m_SelectPath = (GraphicsPath)null;
            this.m_SelectPath.Reset();
            base.OnResize(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            this.Invalidate();
        }

        private void DefectHighlight(Graphics g)
        {
            StatusCoordinate = null;
            if (this.m_SelectedDefect == null)
                return;
            foreach (Defect d in (List<Defect>)this.m_SelectedDefect)
                this.DefectHighlight(g, d);
            if (this.m_SelectedDefect.Count == 1 && this.m_isClick)
                this.DefectCrossLine(g, this.m_SelectedDefect[0]);
            if (this.m_isDrag || this.m_isClick)
                return;
            //this.DrawDefectRegion(g);
        }

        /// <summary>
        /// Shot 설정 여부를 고려한 Defect의 X,Y 값을 가져옵니다.
        /// </summary>
        private void GetDefectXY(Defect defect, out double x, out double y)
        {
            if (IsShotMap)
            {
                while (defect.XREL > _dieSize.Width)
                    defect.XREL -= _dieSize.Width;

                while (defect.YREL > _dieSize.Height)
                    defect.YREL -= _dieSize.Height;

                while (defect.XREL < 0)
                    defect.XREL += _dieSize.Width;

                while (defect.YREL < 0)
                    defect.YREL += _dieSize.Height;

                x = defect.XREL + Mod(defect.XINDEX - ShotStartX, ShotArrayX) * _dieSize.Width;
                y = defect.YREL + Mod(defect.YINDEX - ShotStartY, ShotArrayY) * _dieSize.Height;
            }
            else
            {
                x = defect.X;
                y = defect.Y;
            }
        }

        public DefectList GetVisibleDefect()
        {
            DefectList list = new DefectList();

            if (m_arrDefect == null || m_arrDefect.Count == 0)
                return list;

            foreach (Defect defect in m_arrDefect)
            {
                if (defect.Visible)
                    list.Add(defect);
            }

            return list;
        }

        public static int Mod(int num, int divisor)
        {
            int result = num % divisor;
            return result < 0 ? result + divisor : result;
        }

        private void DefectHighlight(Graphics g, Defect d)
        {
            if (this.m_arrDefect == null || this.m_arrDefect.Count <= 0)
                return;

            PointD[] pointDArray = new PointD[4];
            PointF[] points = new PointF[4];
            SizeF empty = SizeF.Empty;

            double fx, fy;
            GetDefectXY(d, out fx, out fy);

            pointDArray[0].X = fx;
            pointDArray[0].Y = fy;
            pointDArray[1].X = fx + d.XSIZE;
            pointDArray[1].Y = fy;
            pointDArray[2].X = fx + d.XSIZE;
            pointDArray[2].Y = fy + d.YSIZE;
            pointDArray[3].X = fx;
            pointDArray[3].Y = fy + d.YSIZE;

            points[0].X = (float)((-this.m_rectdGlassArea.X + pointDArray[0].X) * this.m_dZoomRatio.X);
            points[0].Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - pointDArray[0].Y) * this.m_dZoomRatio.Y);
            points[1].X = (float)((-this.m_rectdGlassArea.X + pointDArray[1].X) * this.m_dZoomRatio.X);
            points[1].Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - pointDArray[1].Y) * this.m_dZoomRatio.Y);
            points[2].X = (float)((-this.m_rectdGlassArea.X + pointDArray[2].X) * this.m_dZoomRatio.X);
            points[2].Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - pointDArray[2].Y) * this.m_dZoomRatio.Y);
            points[3].X = (float)((-this.m_rectdGlassArea.X + pointDArray[3].X) * this.m_dZoomRatio.X);
            points[3].Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - pointDArray[3].Y) * this.m_dZoomRatio.Y);

            if (this.m_bRealSize)
                g.FillPolygon((Brush)new SolidBrush(this.m_DefectColor[d.CLASSNUMBER]), points);
            if (this.DefaultDefectShape == DefectShape.Rectangle)
            {
                if (!d.EmptyShape)
                    g.FillRectangle((Brush)new SolidBrush(this.DefectColor(d)), new RectangleF(points[0].X - DefectSize / 2f, points[0].Y - DefectSize / 2f, DefectSize, DefectSize));
                else
                    g.DrawRectangle(new Pen(this.DefectColor(d)), points[0].X - DefectSize / 2f, points[0].Y - DefectSize / 2f, DefectSize, DefectSize);
            }
            else if (this.DefaultDefectShape == DefectShape.Circle)
            {
                if (!d.EmptyShape)
                    g.FillEllipse((Brush)new SolidBrush(this.DefectColor(d)), new RectangleF(points[0].X - DefectSize / 2f, points[0].Y - DefectSize / 2f, DefectSize, DefectSize));
                else
                    g.DrawEllipse(new Pen(this.DefectColor(d)), points[0].X - DefectSize / 2f, points[0].Y - DefectSize / 2f, DefectSize, DefectSize);
            }

            using (Pen pen = new Pen(this.HighlightDefectColor))
                g.DrawArc(pen, points[0].X - DefectSize / 2f - 2, points[0].Y - DefectSize / 2f - 2, DefectSize + 2, DefectSize + 2, 0, 360);
        }

        private void DefectCrossLine(Graphics g, Defect d)
        {
            /*
            double num1 = 0.0;
            double y1 = d.Y;
            double glassX = this.Glass.GLASS_X;
            double y2 = d.Y;
            double num2 = (-this.m_rectdGlassArea.X + num1) * this.m_dZoomRatio.X;
            double num3 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - y1) * this.m_dZoomRatio.Y;
            double num4 = (-this.m_rectdGlassArea.X + glassX) * this.m_dZoomRatio.X;
            double num5 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - y2) * this.m_dZoomRatio.Y;
            using (Pen pen = new Pen(this.HighlightDefectColor))
                g.DrawLine(pen, (float)num2, (float)num3, (float)num4, (float)num5);
            double x1 = d.X;
            double num6 = 0.0;
            double x2 = d.X;
            double glassY = this.Glass.GLASS_Y;
            double num7 = (-this.m_rectdGlassArea.X + x1) * this.m_dZoomRatio.X;
            double num8 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - num6) * this.m_dZoomRatio.Y;
            double num9 = (-this.m_rectdGlassArea.X + x2) * this.m_dZoomRatio.X;
            double num10 = (-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - glassY) * this.m_dZoomRatio.Y;
            using (Pen pen = new Pen(this.HighlightDefectColor))
                g.DrawLine(pen, (float)num7, (float)num8, (float)num9, (float)num10);
            PointD calculatedPoint = this.GetCalculatedPoint((PointF)this.m_poEnd);
            this.StatusCoordinate = string.Format("TD:{0:N0}, MD:{1:N0}", (int)calculatedPoint.X, (int)calculatedPoint.Y);
            */

            //this.StatusCoordinate = String.Format("INDEX:{0},{1} REL:{2},{3} SIZE:{4},{5} AREA:{6} CLASS:{7}",
            //    d.XINDEX, d.YINDEX, d.XREL, d.YREL, d.XSIZE, d.YSIZE, d.DEFECTAREA, d.CLASSNUMBER);
        }

        private void DrawDefectRegion(Graphics g)
        {
            if (this.m_SelectPath == null || this.m_SelectedDefect == null || this.m_SelectedDefect.Count == 0)
                return;
            using (Pen pen = new Pen(this.HighlightDefectColor))
                g.DrawPath(pen, this.m_SelectPath);
            RectangleF bounds = this.m_SelectPath.GetBounds();
            string td;
            string md;
            string dpa;
            this.GetSelectedRectInfo(bounds, out td, out md, out dpa);
            bounds.Offset(2f, 2f);
            string str = string.Format("{0}\n{1}\nDefect: {2}\n{3}", td, md, this.m_SelectedDefect.Count, dpa);
            SizeF sizeF = g.MeasureString(str, this.Font);
            using (Brush brush = (Brush)new SolidBrush(Color.FromArgb((int)sbyte.MaxValue, this.GlassColor)))
                g.FillRectangle(brush, bounds.X, bounds.Y, sizeF.Width, sizeF.Height);
            g.DrawString(str, this.Font, Brushes.Black, bounds.Location);
        }

        private void mnuitemMOUSEDRAGMODE_DEFECT_Click(object sender, EventArgs e)
        {
            this.m_eoMouseDragMode = MouseDragMode.Defect;
            this.mnuitemMAPMODE_FREEZOOM.Checked = false;
            this.mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            this.mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            this.mnuitemMOUSEDRAGMODE_DEFECT.Checked = true;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = true;
            this.MenuManagment();
        }

        protected override void mnuitemMAPMODE_FREEZOOM_Click(object sender, EventArgs e)
        {
            this.m_eoMapMode = MapMode.Free;
            this.m_eoMouseDragMode = MouseDragMode.Zoom;
            this.mnuitemMAPMODE_FREEZOOM.Checked = true;
            this.mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            this.mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            this.mnuitemMOUSEDRAGMODE_DEFECT.Checked = false;
            this.MenuManagment();
        }

        protected override void mnuitemMOUSEDRAGMODE_ROTATE_Click(object sender, EventArgs e)
        {
            this.m_eoMouseDragMode = MouseDragMode.Rotate;
            this.mnuitemMAPMODE_FREEZOOM.Checked = false;
            this.mnuitemMOUSEDRAGMODE_ROTATE.Checked = true;
            this.mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            this.mnuitemMOUSEDRAGMODE_DEFECT.Checked = false;
            this.MenuManagment();
        }

        protected override void mnuitemMOUSEDRAGMODE_MOVE_Click(object sender, EventArgs e)
        {
            this.m_eoMouseDragMode = MouseDragMode.Move;
            this.mnuitemMAPMODE_FREEZOOM.Checked = false;
            this.mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            this.mnuitemMOUSEDRAGMODE_MOVE.Checked = true;
            this.mnuitemMOUSEDRAGMODE_DEFECT.Checked = false;
            this.MenuManagment();
        }

        private void menuItemDefectMapOption_Click(object sender, EventArgs e)
        {
            using (DACrux.Map.WaferMapShotOption frm = new DACrux.Map.WaferMapShotOption())
            {
                frm.Map = this;

                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                OnShotOptionChanged(EventArgs.Empty);
            }
        }

        protected new void MenuManagment()
        {
            this.menuitemMAPMODE_ZOOMIN.Enabled = !this.mnuitemMAPMODE_FITSIZE.Checked;
            this.menuitemMAPMODE_ZOOMOUT.Enabled = !this.mnuitemMAPMODE_FITSIZE.Checked;
            this.mnuitemMOUSEDRAGMODE_MOVE.Enabled = !this.mnuitemMAPMODE_FITSIZE.Checked;
            this.mnuitem_SPRIT3.Enabled = this.mnuitemMOUSEDRAGMODE_ZONE.Checked || this.mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Enabled = this.mnuitemMOUSEDRAGMODE_ZONE.Checked || this.mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            this.mnuitemMAPSELECTSTYLE_PIE.Enabled = this.mnuitemMOUSEDRAGMODE_ZONE.Checked || this.mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Enabled = this.mnuitemMOUSEDRAGMODE_ZONE.Checked || this.mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            this.mnuitemMAPSELECTSTYLE_FREEHAND.Enabled = this.mnuitemMOUSEDRAGMODE_ZONE.Checked || this.mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            this.mnuitemMAPSELECTSTYLE_BAND.Enabled = this.mnuitemMOUSEDRAGMODE_ZONE.Checked || this.mnuitemMOUSEDRAGMODE_DEFECT.Checked;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (this.m_eoMouseDragMode == MouseDragMode.Defect)
            {
                this.ResetViewChanged();

                if (Control.ModifierKeys == Keys.None)
                    this.m_SelectedDefect.Clear();
                
                this.CalSelectedDefect();

                if (this.OnMMGSelectedDefect != null)
                    this.OnMMGSelectedDefect(this, this.m_SelectedDefect.ToArray());
            }
            else
            {
                this.SetDelayViewChanged();
            }
                
            this.Invalidate();
        }

        protected override void OnViewChanged(EventArgs e)
        {
            base.OnViewChanged(e);
            if (this.DisplayDefectList == null)
                return;
            this.StatusCurrent = this.DisplayDefectList.Count.ToString();
        }

        private void CalSelectedDefect()
        {
            if (this.m_SelectPath == null)
                return;

            if (this.m_eoMapSelectStyle == MapSelectStyle.FreeHand)
                this.m_SelectPath.CloseFigure();

            if (this.m_isClick)
            {
                Rectangle rect = new Rectangle(this.m_poEnd.X - (int)(0.5 * (double)DefectSize), this.m_poEnd.Y - (int)(0.5 * (double)DefectSize), (int)DefectSize, (int)DefectSize);
                this.m_SelectPath.Reset();
                this.m_SelectPath.AddRectangle(rect);
            }

            foreach (Defect defect in (List<Defect>)this.m_arrDefect)
            {
                if (this.IsVisibleDefect(defect) && (this.m_arrSelDefectNumber.Count == 0 || this.m_arrSelDefectNumber.IndexOf(defect.CLASSNUMBER) >= 0))
                {
                    PointD pointD;
                    GetDefectXY(defect, out pointD.X, out pointD.Y);

                    PointF empty = PointF.Empty;
                    empty.X = (float)((-this.m_rectdGlassArea.X + pointD.X) * this.m_dZoomRatio.X);
                    empty.Y = (float)((-this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y - pointD.Y) * this.m_dZoomRatio.Y);

                    if (this.m_SelectPath.IsVisible(empty))
                    {
                        // Shift : 선택 더하기
                        //if ((Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.None) && !this.m_SelectedDefect.Contains(defect))
                        if ((Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.None))
                        {
                            int idx = m_SelectedDefect.BinarySearch(defect);

                            if (idx < 0)
                                m_SelectedDefect.Insert(~idx, defect);

                            //m_SelectedDefect.Add(defect);
                        }
                        // Ctrl : 선택 빼기
                        //else if (Control.ModifierKeys == Keys.Control && this.m_SelectedDefect.Contains(defect))
                        else if (Control.ModifierKeys == Keys.Control)
                        {
                            int idx = m_SelectedDefect.BinarySearch(defect);

                            if (idx >= 0)
                                m_SelectedDefect.RemoveAt(idx);

                            //m_SelectedDefect.Remove(defect);
                        }

                        if (this.m_isClick)
                            break;
                    }
                }
            }

            //string td;
            //string md;
            //string dpa;
            //this.GetSelectedRectInfo(this.m_SelectPath.GetBounds(), out td, out md, out dpa);
            //this.StatusCoordinate = string.Format("{0}, {1}, {2}", td, md, dpa);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.m_eoMouseDragMode == MouseDragMode.Defect)
            {
                this.GetRealPoint(this.m_poStart.X, this.m_poStart.Y);
                this.m_rectSelect.X = Math.Min(this.m_poStart.X, e.X);
                this.m_rectSelect.Y = Math.Min(this.m_poStart.Y, e.Y);
                this.m_rectSelect.Width = Math.Max(this.m_poStart.X, e.X) - Math.Min(this.m_poStart.X, e.X);
                this.m_rectSelect.Height = Math.Max(this.m_poStart.Y, e.Y) - Math.Min(this.m_poStart.Y, e.Y);
                this.m_isDrag = this.m_rectSelect.Width > 5 || this.m_rectSelect.Height > 5;
                this.m_isClick = !this.m_isDrag;
                PointD calculatedPoint1 = this.GetCalculatedPoint((PointF)this.m_rectSelect.Location);
                PointD calculatedPoint2 = this.GetCalculatedPoint((PointF)new Point(this.m_rectSelect.Right, this.m_rectSelect.Bottom));
                double dX1 = Math.Min(Math.Max(0.0, calculatedPoint1.X), this.Glass.GLASS_X);
                double dY1 = Math.Min(Math.Max(0.0, calculatedPoint1.Y), this.Glass.GLASS_Y);
                double dX2 = Math.Min(Math.Max(0.0, calculatedPoint2.X), this.Glass.GLASS_X);
                double dY2 = Math.Min(Math.Max(0.0, calculatedPoint2.Y), this.Glass.GLASS_Y);
                Point decalculatedPoint1 = this.GetDecalculatedPoint(new PointD(dX1, dY1));
                Point decalculatedPoint2 = this.GetDecalculatedPoint(new PointD(dX2, dY2));
                this.m_rectSelect = new Rectangle(decalculatedPoint1.X, decalculatedPoint1.Y, decalculatedPoint2.X - decalculatedPoint1.X, decalculatedPoint2.Y - decalculatedPoint1.Y);
                this.m_poEnd.X = e.X;
                this.m_poEnd.Y = e.Y;
                this.Invalidate();
            }
            else
                base.OnMouseMove(e);
        }

        private void GetSelectedRectInfo(RectangleF rect, out string td, out string md, out string dpa)
        {
            td = md = dpa = (string)null;
            if (this.m_SelectedDefect == null || this.m_SelectedDefect.Count == 0)
                return;

            PointD calculatedPoint1 = this.GetCalculatedPoint(rect.Location);
            PointD calculatedPoint2 = this.GetCalculatedPoint(new PointF(rect.Left + rect.Width, rect.Top + rect.Height));
            calculatedPoint1.X = Math.Min(Math.Max(calculatedPoint1.X, 0.0), this.Glass.GLASS_X);
            calculatedPoint1.Y = Math.Min(Math.Max(calculatedPoint1.Y, 0.0), this.Glass.GLASS_Y);
            calculatedPoint2.X = Math.Min(Math.Max(calculatedPoint2.X, 0.0), this.Glass.GLASS_X);
            calculatedPoint2.Y = Math.Min(Math.Max(calculatedPoint2.Y, 0.0), this.Glass.GLASS_Y);
            td = string.Format("X:{0:N0}~{1:N0}{2}", calculatedPoint1.X, calculatedPoint2.X, this.DisplayXUnit);
            md = string.Format("Y:{0:N0}~{1:N0}{2}", calculatedPoint2.Y, calculatedPoint1.Y, this.DisplayYUnit);
            string valueString1 = (calculatedPoint2.X - calculatedPoint1.X).ToString() + this.DisplayXUnit;
            string valueString2 = (calculatedPoint1.Y - calculatedPoint2.Y).ToString() + this.DisplayYUnit;
            
            double toValue1;
            double toValue2;
            if (LengthConverter.TryConvert(valueString1, DensityUnit, out toValue1) && LengthConverter.TryConvert(valueString2, DensityUnit, out toValue2))
                dpa = string.Format("DPA: {0}", Math.Round((double)this.m_SelectedDefect.Count / (toValue1 * toValue2), 2));
            else
                dpa = "DPA: NaN";
        }

        private PointD GetCalculatedPoint(PointF pt)
        {
            PointD empty = PointD.Empty;
            empty.X = (double)pt.X / this.m_dZoomRatio.X + this.m_rectdGlassArea.X;
            empty.Y = -(double)pt.Y / this.m_dZoomRatio.Y - this.m_rectdGlassArea.Y + this.m_GlassRecipe.GLASS_Y;
            return empty;
        }

        private Point GetDecalculatedPoint(PointD pt)
        {
            Point empty = Point.Empty;
            empty.X = (int)(this.m_dZoomRatio.X * (pt.X - this.m_rectdGlassArea.X));
            empty.Y = (int)(-this.m_dZoomRatio.Y * (pt.Y + this.m_rectdGlassArea.Y - this.m_GlassRecipe.GLASS_Y));
            return empty;
        }

        private void MMGDefectMap_Simple_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            this.SetDefaultColor();
            this.Reset();
            this.SetFunction(MMGDefectMap_Simple.Function.DefectSelect);
        }

        public void SetSelected(Defect d)
        {
            this.m_SelectedDefect.Add(d);
            this.Invalidate();
        }

        public void ClearSelectedDefect()
        {
            this.m_SelectedDefect.Clear();
            Redraw();
        }

        public void SetShotMap(bool isShotMap, SizeF dieSize,  int shotCountX, int shotCountY, int shotStartX, int shotStartY)
        {
            IsShotMap = isShotMap;
            _dieSize = dieSize;
            ShotArrayX = shotCountX;
            ShotArrayY = shotCountY;
            ShotStartX = shotStartX;
            ShotStartY = shotStartY;

            if (IsShotMap)
            {
                Glass.GLASS_X = dieSize.Width * ShotArrayX;
                Glass.GLASS_Y = dieSize.Height * shotCountY;
            }
        }

        private SizeF _dieSize = new SizeF(1, 1);

        public struct ColorBySize
        {
            public int from;
            public int to;
            public Color color;
        }

        public enum Function
        {
            FreeZoom,
            Move,
            DefectSelect,
        }

        /// <summary>기준이 되는 0점을 나타냅니다.</summary>
        public enum ZeroPoint
        {
            Center,
            LowerLeft,
        }

        public class ShapeItem
        {
            public Rectangle Rectangle { get; private set; }

            public Color Color { get; private set; }

            public DefectShape Shape { get; private set; }

            public string DisplayText { get; set; }

            public Font Font { get; set; }

            public Color TextColor { get; set; }

            public ShapeItem(Rectangle rectangle, Color lineColor, DefectShape shape, string displayName, Font textFont, Color textColor)
            {
                this.Rectangle = rectangle;
                this.Color = lineColor;
                this.Shape = shape;
                this.DisplayText = displayName;
                this.Font = textFont;
                this.TextColor = textColor;
            }

            public ShapeItem(Rectangle rectangle, Color lineColor, DefectShape shape, string displayName, Font textFont)
                : this(rectangle, lineColor, shape, displayName, textFont, lineColor)
            {
            }

            public ShapeItem(Rectangle rectangle, Color lineColor, DefectShape shape, string displayName, Color textColor)
                : this(rectangle, lineColor, shape, displayName, (Font)null, textColor)
            {
            }

            public ShapeItem(Rectangle rectangle, Color lineColor, DefectShape shape, string displayName)
                : this(rectangle, lineColor, shape, displayName, (Font)null, lineColor)
            {
            }

            public ShapeItem(Rectangle rectangle, Color lineColor, DefectShape shape)
                : this(rectangle, lineColor, shape, (string)null, (Font)null, lineColor)
            {
            }
        }

        public class ClassNumberInfo
        {
            public int ClassNumber { get; set; }

            public string Info { get; set; }

            public Color Color { get; set; }
        }

        [DefaultValue(false)]
        public bool IsShotMap
        {
            get;
            private set;
        }

        int _shotArrayX = 1;
        int _shotArrayY = 1;

        [DefaultValue(1)]
        public int ShotArrayX
        {
            get { return _shotArrayX; }
            set { _shotArrayX = value; Glass.GLASS_X = _dieSize.Width * _shotArrayX; FitSize(); }
        }

        [DefaultValue(1)]
        public int ShotArrayY
        {
            get { return _shotArrayY; }
            set { _shotArrayY = value; Glass.GLASS_Y = _dieSize.Height * _shotArrayY; FitSize(); }
        }

        [DefaultValue(1)]
        public int ShotStartX
        {
            get;
            set;
        }

        [DefaultValue(1)]
        public int ShotStartY
        {
            get;
            set;
        }

        [DefaultValue(1)]
        public int DieMinX
        {
            get;
            set;
        }

        [DefaultValue(1)]
        public int DieMinY
        {
            get;
            set;
        }

        [DefaultValue(DEFAULT_DEFECT_SIZE)]
        public float DefectSize
        {
            get;
            set;
        }

        public DefectList SelectedDefect
        {
            get { return m_SelectedDefect; }
        }

        public void ClearShotInfo()
        {
            ShotArrayX = ShotArrayY = 1;
            ShotStartX = ShotStartY = 1;
        }
    }

    /// <summary>
    /// 도형의 모양을 나타냅니다.
    /// </summary>
    public enum DefectShape
    {
        /// <summary>네모 모양을 나타냅니다.</summary>
        Rectangle,
        /// <summary>원 모양을 나타냅니다.</summary>
        Circle
    };
}
