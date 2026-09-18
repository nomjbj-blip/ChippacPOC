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
    /// <summary>
    /// Defect을 선택했을때 발생하는 Event의 대리자
    /// </summary>
    /// <param name="sender">현재 Map Object</param>
    /// <param name="oDefect">Selection된 Defect의 Object를 Array형태로 반환</param>
    public delegate void MMGSelectedDefect(object sender, Defect[] oDefect);

    public partial class MMGDefectMap : MMGBinMap
    {

        private MAP_TYPE m_mapType = MAP_TYPE.CLASS;  //현재 Defect Map의 Type을 정의 하는 변수임

        public struct ColorBySize
        {
            public int from;
            public int to;
            public Color color;
        }

        public static readonly string DEFECT_ALL = "ALL";

        public static float DEFECT_LEN = 8f;
        public static float DEFECT_SEL_LEN = 12f;

        private ColorBySize[] m_SizeColor = null;

        protected List<Defect> m_arrDefect = null;
        protected ArrayList m_arrSelDefectNumber = null;
        protected ArrayList m_DefectIndexer = null;
        protected ArrayList m_DefectClickIndexer = null;

        private Color[] m_DefectColor = null;
        private bool m_bRealSize = false;
        private bool m_bImageMark = true;
        private bool m_bVisibleShape = false;
        private Point m_pCurrent = Point.Empty;
        private Color m_FPDPanelColor = Color.White;
        private List<Defect> m_SelectedDefect = null;
        private System.Windows.Forms.MenuItem mnuREALSIZEDEFECT;
        private System.Windows.Forms.MenuItem mnuitemMOUSEDRAGMODE_DEFECT;

        public event MMGSelectedDefect OnMMGSelectedDefect;
        protected string[] m_strArrShape = null;

        new protected Color[] m_ColorSet = null;

        private bool m_bVisibleDefectInRange = false;
        protected double m_dMaxSize = 1000;
        protected double m_dMinSize = 0;
        private Font m_fntGID = null;

        /// <summary>
        /// Defect Map의 생성자 임
        /// </summary>
        public MMGDefectMap()
        {
            InitializeComponent();
            ResetSelDefectNumber();
            SetDefaultColor();
            Reset();

            ctxmGlassMap.MenuItems.Add(mnuitemMOUSEDRAGMODE_DEFECT);
            ctxmGlassMap.MenuItems.Add(mnuREALSIZEDEFECT);

            DrawDefects = "ALL";
            m_strArrShape = new string[1000]; // Max Defect Type : 하드코딩 ㅠㅠ

            DisplayDefectList = new List<Defect>();

            // rotate 기능 hidden 처리
            mnuitemMOUSEDRAGMODE_ROTATE.Visible = false;
            BackColor = Color.White;
            ShapeItems = new List<ShapeItem>();
            AddedClassNumberList = new List<ClassNumberInfo>();
        }

        #region ◈ FPDPanel 속성

        [Category("FPDPanel 속성"), Description("Probe(점등검사) 결과의 Bin 값과 Map Overaly를 하는지 여부를 설정하거나 가져옵니다.")]
        public bool VisibleProbeOverlay
        {
            set
            {
                base.m_bVisibleFPDPanelValue = value;
            }
            get
            {
                return base.m_bVisibleFPDPanelValue;
            }
        }

        [Category("FPDPanel 속성"), Description("FPDPanel의 Border Color를 설정하거나 가져옵니다.")]
        public Color FPDPanelBackgroundColor
        {
            set { m_FPDPanelColor = value; }
            get { return m_FPDPanelColor; }
        }
        #endregion

        #region ■ Map Type을 정해준다(color by CLASS, SIZE, ROUGHBIN, FINEBIN, CLUSTER)
        [Category("FPDPanel 속성"), Description("Map Type을 정해준다(color by CLASS, SIZE, ROUGHBIN, FINEBIN, CLUSTER)")]
        public MAP_TYPE MapType
        {
            get { return m_mapType; }
            set { m_mapType = value; }
        }
        #endregion

        #region ■ Defect Size 에 따라 Defect Color을 정한다
        public DataTable SizeColor
        {
            set
            {
                if (value == null) return;
                if (value.Rows.Count == 0) return;
                if (value.Columns.Count == 0) return;
                if (!value.Columns[0].Caption.Equals("SIZE_FROM")) return;
                if (!value.Columns[1].Caption.Equals("SIZE_TO")) return;
                if (!value.Columns[2].Caption.Equals("COLOR")) return;

                m_SizeColor = null;
                m_SizeColor = new ColorBySize[value.Rows.Count];
                string color = string.Empty;
                for (int i = 0; i < value.Rows.Count; i++)
                {
                    m_SizeColor[i].from = (int)(decimal)value.Rows[i]["SIZE_FROM"];
                    m_SizeColor[i].to = (int)(decimal)value.Rows[i]["SIZE_TO"];
                    color = (string)value.Rows[i]["COLOR"];
                    m_SizeColor[i].color = ColorTranslator.FromHtml(color);
                }
            }
        }
        #endregion

        #region ■ Defect Type, Roughbin, Cluster, Finebin 에 따라 Defect Color을 정한다
        public DataTable TypeColor
        {
            set
            {
                if (value == null) return;
                if (value.Rows.Count == 0) return;
                if (value.Columns.Count == 0) return;
                if (!value.Columns[0].Caption.Equals("CLASSNUMBER")) return;
                if (!value.Columns[1].Caption.Equals("COLOR")) return;

                if (m_DefectColor == null) return;
                if (m_DefectColor.Length == 0) return;
                for (int i = 0; i < m_DefectColor.Length; i++) m_DefectColor[i] = Color.Black;

                string color = string.Empty;
                int classNumber = 0;
                for (int i = 0; i < value.Rows.Count; i++)
                {
                    color = (string)value.Rows[i]["COLOR"];
                    classNumber = (int)(decimal)value.Rows[i]["CLASSNUMBER"];
                    m_DefectColor[classNumber] = ColorTranslator.FromHtml(color);
                }
            }
        }

        public void SetDefectColor(int index, string desc, Color color)
        {
            m_DefectColor[index] = color;
            AddedClassNumberList.Add(new ClassNumberInfo() { ClassNumber = index, Info = desc, Color = color });
        }

        public void SetDefectColor(int Index, Color oCol)
        {
            m_DefectColor[Index] = oCol;
        }

        public void ClearDefectColor()
        {
            AddedClassNumberList.Clear();
        }
        #endregion

        public List<Defect> Defects
        {
            get { return m_arrDefect; }
        }

        #region ■ Defect Array Handling 함수 ( FPDPanelClear / AddDefect(Defect NewDefect) )
        /// <summary>
        /// 전체 Defect을 삭제한다.
        /// </summary>
        public void DefectClear()
        {
            m_arrDefect.Clear();
            m_DefectIndexer.Clear();
        }
        /// <summary>
        /// Defect을 추가한다.
        /// </summary>
        /// <param name="NewDefect">Defect 구조체</param>
        public void AddDefect(Defect NewDefect)
        {
            m_arrDefect.Add(NewDefect);
            m_DefectIndexer.Add(new PointF((float)NewDefect.X, (float)NewDefect.Y));
            m_DefectClickIndexer.Add(new Point((int)(NewDefect.X), (int)(NewDefect.Y)));
        }
        #endregion

        /// <summary>
        /// 컨트롤의 배경색을 가져오거나 설정합니다.
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        [Description("구성 요소의 배경색 입니다.")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        public bool VisibleShape
        {
            set
            {
                m_bVisibleShape = value;
            }
            get
            {
                return m_bVisibleShape;
            }
        }


        public bool VisibleImageMark
        {
            set
            {
                m_bImageMark = value;
                Redraw();
            }
            get
            {
                return m_bImageMark;
            }
        }

        public bool RealSizeDefectDrawing
        {
            set
            {
                m_bRealSize = value;
                Redraw();
            }
            get
            {
                return m_bRealSize;
            }
        }

        public bool VisibleDefectInRange
        {
            set
            {
                m_bVisibleDefectInRange = value;
                Redraw();
            }
            get
            {
                return m_bVisibleDefectInRange;
            }
        }

        public void AddSelDefectNumber(int iDefect)
        {
            if (m_arrSelDefectNumber.IndexOf(iDefect) < 0)
                m_arrSelDefectNumber.Add(iDefect);
        }

        public void DeleteSelDefectNumber(int iDefect)
        {
            m_arrSelDefectNumber.Remove(iDefect);
        }

        public void ResetSelDefectNumber()
        {
            m_arrSelDefectNumber = new ArrayList();
        }

        public void VisibleSizeChange(double dMax, double dMin)
        {
            m_dMaxSize = dMax;
            m_dMinSize = dMin;
            Redraw();
        }

        public void SetShape(int DefectType, string Shape)
        {
            if (DefectType >= m_strArrShape.Length) return;
            m_strArrShape[DefectType] = Shape;
        }


        #region ■ Draw Glass
        protected override void DrawGlass()
        {
            base.DrawGlass();

            if (m_gdiTempMap != null)
                m_gdiTempMap.SmoothingMode = SmoothingMode.AntiAlias;

            DrawGrid(m_gdiTempMap);
            DrawShapeItems(m_gdiTempMap);
            DrawDefect(m_gdiTempMap);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DefectHighlight();
        }

        public override void Redraw()
        {
            base.Redraw();
            Invalidate();
            DefectHighlight();
        }

        #endregion

        #region ■ Draw Defect
        /// <summary>
        /// Defect을 주어진 Graphics object에 Drawing합니다.
        /// </summary>
        /// <param name="g">Graphics Object</param>
        private void DrawDefect(Graphics g)
        {
            if (g == null)
                return;

            if (m_arrDefect == null || m_arrDefect.Count <= 0) return;
            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];
            SizeF sfDSize = SizeF.Empty;

            try
            {
                RectangleF clipBounds = g.VisibleClipBounds;
                DisplayDefectList.Clear();

                foreach (Defect InDefect in m_arrDefect)
                {
                    if (DrawDefects != "ALL" && !DrawDefects.Contains(InDefect.CLASSNUMBER.ToString()))
                        continue;

                    if (m_arrSelDefectNumber.Count != 0)
                        if (m_arrSelDefectNumber.IndexOf(InDefect.CLASSNUMBER) < 0) continue;

                    if (m_bVisibleDefectInRange)
                    {
                        if (m_dMaxSize < InDefect.DSIZE || m_dMinSize > InDefect.DSIZE) continue;
                    }

                    if (m_bRealSize)
                    {
                        pdPoint[0].X = InDefect.X;
                        pdPoint[0].Y = InDefect.Y;

                        pdPoint[1].X = InDefect.X + InDefect.XSIZE;
                        pdPoint[1].Y = InDefect.Y;

                        pdPoint[2].X = InDefect.X + InDefect.XSIZE;
                        pdPoint[2].Y = InDefect.Y + InDefect.YSIZE;

                        pdPoint[3].X = InDefect.X;
                        pdPoint[3].Y = InDefect.Y + InDefect.YSIZE;
                    }
                    else
                    {
                        pdPoint[0].X = InDefect.X;
                        pdPoint[0].Y = InDefect.Y;

                        pdPoint[1].X = InDefect.X + InDefect.XSIZE * 50;
                        pdPoint[1].Y = InDefect.Y;

                        pdPoint[2].X = InDefect.X + InDefect.XSIZE * 50;
                        pdPoint[2].Y = InDefect.Y + InDefect.YSIZE * 50;

                        pdPoint[3].X = InDefect.X;
                        pdPoint[3].Y = InDefect.Y + InDefect.YSIZE * 50;
                    }

                    RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                    if (ZeroPointLocation == ZeroPoint.Center)
                    {
                        pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));
                    }
                    else if (ZeroPointLocation == ZeroPoint.LowerLeft)
                    {
                        pfPoint[0].X = (float)((-m_rectdGlassArea.X + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[1].X = (float)((-m_rectdGlassArea.X + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[2].X = (float)((-m_rectdGlassArea.X + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                        pfPoint[3].X = (float)((-m_rectdGlassArea.X + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                        pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));
                    }


                    if (!clipBounds.Contains(pfPoint[0].X - DEFECT_LEN / 2, pfPoint[0].Y - DEFECT_LEN / 2))
                        continue;

                    DisplayDefectList.Add(InDefect);

                    if (m_bVisibleShape)
                    {
                        g.DrawString(m_strArrShape[InDefect.CLASSNUMBER], m_fntGID, new SolidBrush(DefectColor(InDefect)), pfPoint[0]);
                    }
                    else
                    {
                        if (m_bRealSize)
                        {
                            g.FillPolygon(new SolidBrush(DefectColor(InDefect)), pfPoint);
                        }
                        else
                        {
                            if (DefaultDefectShape == Shape.Rectangle)
                            {
                                if (!InDefect.EmptyShape)
                                    g.FillRectangle(new SolidBrush(DefectColor(InDefect)), new RectangleF(pfPoint[0].X - DEFECT_LEN / 2, pfPoint[0].Y - DEFECT_LEN / 2, DEFECT_LEN, DEFECT_LEN));
                                else
                                    g.DrawRectangle(new Pen(DefectColor(InDefect)), pfPoint[0].X - DEFECT_LEN / 2, pfPoint[0].Y - DEFECT_LEN / 2, DEFECT_LEN, DEFECT_LEN);
                            }
                            else if (DefaultDefectShape == Shape.Circle)
                            {
                                if (!InDefect.EmptyShape)
                                    g.FillEllipse(new SolidBrush(DefectColor(InDefect)), new RectangleF(pfPoint[0].X - DEFECT_LEN / 2, pfPoint[0].Y - DEFECT_LEN / 2, DEFECT_LEN, DEFECT_LEN));
                                else
                                    g.DrawEllipse(new Pen(DefectColor(InDefect)), pfPoint[0].X - DEFECT_LEN / 2, pfPoint[0].Y - DEFECT_LEN / 2, DEFECT_LEN, DEFECT_LEN);
                            }
                        }

                        if (InDefect.IMAGECOUNT > 0 && m_bImageMark)
                        {
                            g.DrawArc(new Pen(Color.Red), pfPoint[0].X - 2, pfPoint[0].Y - 2, 8, 8, 0, 360);
                        }
                    }
                }
            }
            catch { }
        }

        private void  DrawGrid(Graphics g)
        {
            if (g == null || !ShowGridLine)
                return;

            Pen pen = new Pen(Color.Gray, 1) { DashPattern = new float[] { 4, 2 } };
            StringFormat sfX = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
            StringFormat sfY = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            try
            {
                double unit = Glass.GLASS_Y / (GridLineYCount + 1);

                // Y축 눈금선
                for (int i = 1; i <= GridLineYCount + 1; i++)
                {
                    double x1 = 0;
                    double y1 = i * unit;
                    double x2 = Glass.GLASS_X;
                    double y2 = i * unit;

                    RotatePoint(ref x1, ref y1, m_iViewAngle);
                    RotatePoint(ref x2, ref y2, m_iViewAngle);

                    x1 = (-m_rectdGlassArea.X + x1) * m_dZoomRatio * m_dScale;
                    y1 = (-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - y1) * m_dZoomRatio * m_dScale;
                    x2 = (-m_rectdGlassArea.X + x2) * m_dZoomRatio * m_dScale;
                    y2 = (-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - y2) * m_dZoomRatio * m_dScale;

                    if (i <= GridLineYCount)
                        g.DrawLine(pen, (float)x1, (float)y1, (float)x2, (float)y2);

                    string val = Math.Round(i * unit, 0).ToString();
                    float offset = g.MeasureString(val, Font).Width;
                    g.DrawString(val, Font, Brushes.Gray, new PointF((float)x1 - offset, (float)y1), sfX);
                }

                unit = Glass.GLASS_X / (GridLineXCount + 1);

                // X축 눈금선
                for (int i = 1; i <= GridLineXCount + 1; i++)
                {
                    double x1 = i * unit;
                    double y1 = 0;
                    double x2 = i * unit;
                    double y2 = Glass.GLASS_Y;

                    RotatePoint(ref x1, ref y1, m_iViewAngle);
                    RotatePoint(ref x2, ref y2, m_iViewAngle);

                    x1 = (-m_rectdGlassArea.X + x1) * m_dZoomRatio * m_dScale;
                    y1 = (-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - y1) * m_dZoomRatio * m_dScale;
                    x2 = (-m_rectdGlassArea.X + x2) * m_dZoomRatio * m_dScale;
                    y2 = (-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - y2) * m_dZoomRatio * m_dScale;

                    if (i <= GridLineXCount)
                        g.DrawLine(pen, (float)x1, (float)y1, (float)x2, (float)y2);

                    string val = Math.Round(i * unit, 0).ToString();
                    float offset = g.MeasureString(val, Font).Height;
                    g.DrawString(val, Font, Brushes.Gray, new PointF((float)x1, (float)y1 + offset), sfY);
                }

                // 원점 아래에 단위 표시
                if (!String.IsNullOrEmpty(DisplayUnit))
                {
                    double x0 = 0;
                    double y0 = 0;

                    RotatePoint(ref x0, ref y0, m_iViewAngle);

                    x0 = (-m_rectdGlassArea.X + x0) * m_dZoomRatio * m_dScale;
                    y0 = (-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - y0) * m_dZoomRatio * m_dScale;

                    SizeF offset = g.MeasureString(DisplayUnit, Font);
                    g.DrawString(DisplayUnit, Font, Brushes.Gray, new PointF((float)x0 - offset.Width, (float)y0 + offset.Height), sfY);

                    // 군정보(CLASSNUMBER) 표시
                    if (ShowClassNumberInfo)
                    {
                        y0 += 2 *offset.Height;

                        foreach (ClassNumberInfo item in AddedClassNumberList)
                        {
                            g.FillEllipse(new SolidBrush(item.Color), (float)x0, (float)y0, DEFECT_LEN, DEFECT_LEN);
                            x0 += DEFECT_LEN;
                            offset = g.MeasureString(item.Info, Font);
                            g.DrawString(item.Info, Font, new SolidBrush(item.Color), (float)x0, (float)y0);
                            x0 += offset.Width;
                            x0 += DEFECT_LEN;
                        }
                    }
                }
            }
            catch { }
            finally
            {
                if (pen != null)
                    pen.Dispose();

                if (sfX != null)
                    sfX.Dispose();

                if (sfY != null)
                    sfY.Dispose();
            }
        }

        private void DrawShapeItems(Graphics g)
        {
            if (g == null || ShapeItems.Count == 0)
                return;

            foreach (ShapeItem item in ShapeItems)
            {
                double x1 = item.Rectangle.X;
                double y1 = item.Rectangle.Y;
                double x2 = item.Rectangle.X + item.Rectangle.Width;
                double y2 = item.Rectangle.Y + item.Rectangle.Height;

                RotatePoint(ref x1, ref y1, m_iViewAngle);
                RotatePoint(ref x2, ref y2, m_iViewAngle);

                x1 = (-m_rectdGlassArea.X + x1) * m_dZoomRatio * m_dScale;
                y1 = (-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - y1) * m_dZoomRatio * m_dScale;
                x2 = (-m_rectdGlassArea.X + x2) * m_dZoomRatio * m_dScale;
                y2 = (-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - y2) * m_dZoomRatio * m_dScale;

                using (Pen pen = new Pen(item.Color, 2))
                {
                    if (item.Shape == Shape.Rectangle)
                        g.DrawRectangle(pen, (float)x1, (float)y1, (float)x2 - (float)x1, (float)y1 - (float)y2);
                    else if (item.Shape == Shape.Circle)
                        g.DrawEllipse(pen, (float)x1, (float)y1, (float)x2 - (float)x1, (float)y1 - (float)y2);
                }
            }
        }
        
        #endregion
        
        #region ■ Defect 색을 정한다
        Color DefectColor(Defect InDefect)
        {
            switch (m_mapType)
            {
                case MAP_TYPE.CLASS:
                    return m_DefectColor[InDefect.CLASSNUMBER];
                case MAP_TYPE.CLUSTER:
                    return m_DefectColor[InDefect.CLUSTERNUMBER];
                case MAP_TYPE.FINEBIN:
                    return m_DefectColor[InDefect.FINEBINNUMBER];
                case MAP_TYPE.ROUGHBIN:
                    return m_DefectColor[InDefect.ROUGHBINNUMBER];
                case MAP_TYPE.SIZE:
                    if (m_SizeColor == null) return Color.Red;
                    for (int i = 0; i < m_SizeColor.Length; i++)
                    {
                        if (InDefect.DSIZE * 1000000 >= m_SizeColor[i].from && InDefect.DSIZE * 1000000 <= m_SizeColor[i].to) return m_SizeColor[i].color;
                    }
                    return Color.Red;
                default:
                    return m_DefectColor[InDefect.CLASSNUMBER];
            }
        }
        #endregion

        Color GetDensityColor(DACrux.Base.CPanel pan)
        {
            int min = int.MaxValue;
            int max = int.MinValue;

            foreach (CQPanel InQPanel in m_GlassRecipe.QPanels)
                foreach (CPanel InFPDPanel in InQPanel.Panels)
                {
                    if (min > InFPDPanel.BIN) min = InFPDPanel.BIN;
                    if (max < InFPDPanel.BIN) max = InFPDPanel.BIN;
                }

            int gap = max - min;
            float term = 255.0f / gap;

            return Color.FromArgb(255, 255 - (int)(term * pan.BIN), 255);
        }

        public override void Reset()
        {
            base.Reset();
            if (m_arrDefect != null)
            {
                m_arrDefect.Clear();
                m_arrDefect = null;
            }

            if (m_DefectIndexer != null)
            {
                m_DefectIndexer.Clear();
                m_DefectIndexer = null;
            }

            if (AddedClassNumberList != null)
                AddedClassNumberList.Clear();

            if (m_DefectClickIndexer != null)
            {
                m_DefectClickIndexer.Clear();
                m_DefectClickIndexer = null;
            }

            if (Information != null)
                Information.Clear();

            if (ShapeItems != null)
                ShapeItems.Clear();


            m_arrDefect = new List<Defect>();
            m_DefectIndexer = new ArrayList();
            m_DefectClickIndexer = new ArrayList();
            m_SelectedDefect = new List<Defect>();

            m_fntGID = new Font("굴림", Math.Max(8, Math.Min((float)(m_dZoomRatio * 1.5f), 12)));
        }

        protected override void SetDefaultColor()
        {
            base.SetDefaultColor();
            int r, g, b;//4337915

            int[] iColor = new int[]	{16777215,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
												65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
												16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
												10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
												8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
												6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
												12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
												10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
												2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
												10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686,
												
												16766976,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
												65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
												16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
												10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
												8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
												6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
												12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
												10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
												2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
												10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686,
												
												4227136,12632256,9868950,6795178,4446555,
												6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
												12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
												10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
												2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
												10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686
												,2521304};

            if (m_ColorSet != null) m_ColorSet = null;
            m_DefectColor = new Color[iColor.Length];
            for (int i = 0; i < iColor.Length; i++)
            {
                r = (iColor[i] >> 16);
                g = (iColor[i] >> 8) - (r * 256);
                b = iColor[i] - (r * 65536) - (g * 256);
                m_DefectColor[i] = Color.FromArgb(r, g, b);
            }
        }

        private void mnuREALSIZEDEFECT_Click(object sender, System.EventArgs e)
        {
            m_bRealSize = mnuREALSIZEDEFECT.Checked = !mnuREALSIZEDEFECT.Checked;
            this.Redraw();
        }


        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            List<Defect> arrDefect = new List<Defect>();

            PointD pdSelectPoint = GetRealPoint(m_pCurrent.X, m_pCurrent.Y);


            foreach (Defect InDefect in m_arrDefect)
            {
                if (Math.Pow((InDefect.X - pdSelectPoint.X), 2.0d) + Math.Pow((InDefect.Y - pdSelectPoint.Y), 2.0d) < Math.Pow((7.0f / m_dZoomRatio), 2))
                {
                    DefectHighlight(InDefect);
                    arrDefect.Add(InDefect);
                }
            }

            if (arrDefect.Count > 0)
            {
                if (OnMMGSelectedDefect != null) OnMMGSelectedDefect(this, arrDefect.ToArray());
            }
        }

        private void DefectHighlight()
        {
            foreach (Defect defect in m_SelectedDefect)
                DefectHighlight(defect);
        }

        private void DefectHighlight(Defect InDefect)
        {
            if (m_arrDefect == null || m_arrDefect.Count <= 0) return;
            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];
            SizeF sfDSize = SizeF.Empty;

            if (m_bRealSize)
            {
                pdPoint[0].X = InDefect.X;
                pdPoint[0].Y = InDefect.Y;

                pdPoint[1].X = InDefect.X + InDefect.XSIZE;
                pdPoint[1].Y = InDefect.Y;

                pdPoint[2].X = InDefect.X + InDefect.XSIZE;
                pdPoint[2].Y = InDefect.Y + InDefect.YSIZE;

                pdPoint[3].X = InDefect.X;
                pdPoint[3].Y = InDefect.Y + InDefect.YSIZE;
            }
            else
            {
                pdPoint[0].X = InDefect.X;
                pdPoint[0].Y = InDefect.Y;

                pdPoint[1].X = InDefect.X + InDefect.XSIZE * 50;
                pdPoint[1].Y = InDefect.Y;

                pdPoint[2].X = InDefect.X + InDefect.XSIZE * 50;
                pdPoint[2].Y = InDefect.Y + InDefect.YSIZE * 50;

                pdPoint[3].X = InDefect.X;
                pdPoint[3].Y = InDefect.Y + InDefect.YSIZE * 50;
            }


            RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
            RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
            RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
            RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

            if (ZeroPointLocation == ZeroPoint.Center)
            {
                pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));
            }
            else if (ZeroPointLocation == ZeroPoint.LowerLeft)
            {
                pfPoint[0].X = (float)((-m_rectdGlassArea.X + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[1].X = (float)((-m_rectdGlassArea.X + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[2].X = (float)((-m_rectdGlassArea.X + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[3].X = (float)((-m_rectdGlassArea.X + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));
            }


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (m_bRealSize) 
                m_gdiMain.FillPolygon(new SolidBrush(m_DefectColor[InDefect.CLASSNUMBER]), pfPoint);

            if (DefaultDefectShape == Shape.Rectangle)
            {
                if (!InDefect.EmptyShape)
                    m_gdiMain.FillRectangle(new SolidBrush(DefectColor(InDefect)), new RectangleF(pfPoint[0].X - DEFECT_LEN / 2, pfPoint[0].Y - DEFECT_LEN / 2, DEFECT_LEN, DEFECT_LEN));
                else
                    m_gdiMain.DrawRectangle(new Pen(DefectColor(InDefect)), pfPoint[0].X - DEFECT_LEN / 2, pfPoint[0].Y - DEFECT_LEN / 2, DEFECT_LEN, DEFECT_LEN);
            }
            else if (DefaultDefectShape == Shape.Circle)
            {
                if (!InDefect.EmptyShape)
                    m_gdiMain.FillEllipse(new SolidBrush(DefectColor(InDefect)), new RectangleF(pfPoint[0].X - DEFECT_LEN / 2, pfPoint[0].Y - DEFECT_LEN / 2, DEFECT_LEN, DEFECT_LEN));
                else
                    m_gdiMain.DrawEllipse(new Pen(DefectColor(InDefect)), pfPoint[0].X - DEFECT_LEN / 2, pfPoint[0].Y - DEFECT_LEN / 2, DEFECT_LEN, DEFECT_LEN);
            }

            m_gdiMain.DrawArc(new Pen(Color.Blue), pfPoint[0].X - DEFECT_SEL_LEN / 2, pfPoint[0].Y - DEFECT_SEL_LEN / 2, DEFECT_SEL_LEN, DEFECT_SEL_LEN, 0, 360);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_pCurrent.X = e.X;
            m_pCurrent.Y = e.Y;
        }

        private void mnuitemMOUSEDRAGMODE_DEFECT_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Defect;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_DEFECT.Checked = true;
            MenuManagment();
        }

        protected override void mnuitemMAPMODE_FREEZOOM_Click(object sender, System.EventArgs e)
        {
            m_eoMapMode = MapMode.Free;
            m_eoMouseDragMode = MouseDragMode.Zoom;

            mnuitemMAPMODE_FREEZOOM.Checked = true;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_DEFECT.Checked = false;
            MenuManagment();
        }

        protected override void mnuitemMOUSEDRAGMODE_ROTATE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Rotate;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = true;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_DEFECT.Checked = false;
            MenuManagment();
        }

        protected override void mnuitemMOUSEDRAGMODE_MOVE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Move;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = true;
            mnuitemMOUSEDRAGMODE_DEFECT.Checked = false;
            MenuManagment();
        }

        protected new void MenuManagment()
        {
            menuitemMAPMODE_ZOOMIN.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;
            menuitemMAPMODE_ZOOMOUT.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;

            //mnuitem_SPRIT2.Visible = true;
            //mnuitemMOUSEDRAGMODE_ZOOM.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;
            //mnuitemMOUSEDRAGMODE_ROTATE.Enabled = true;
            //mnuitemMOUSEDRAGMODE_ZONE.Enabled = true;
            mnuitemMOUSEDRAGMODE_MOVE.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;

            mnuitem_SPRIT3.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked || mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            mnuitemMAPSELECTSTYLE_CIRCLE.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked || mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            mnuitemMAPSELECTSTYLE_PIE.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked || mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked || mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            mnuitemMAPSELECTSTYLE_FREEHAND.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked || mnuitemMOUSEDRAGMODE_DEFECT.Checked;
            mnuitemMAPSELECTSTYLE_BAND.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked || mnuitemMOUSEDRAGMODE_DEFECT.Checked;
        }
        
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == 0)
                m_SelectedDefect.Clear();

            if (m_eoMouseDragMode != MouseDragMode.Defect)
            {
                base.OnMouseUp(e);
                return;
            }
            Invalidate();
            CalSelectedDefect();
            
            if (OnMMGSelectedDefect != null)
                OnMMGSelectedDefect(this, m_SelectedDefect.ToArray());
        }

        private void CalSelectedDefect()
        {
            if (m_SelectPath == null) 
                return;

            PointD pdDefectCenter;
            
            if (m_eoMapSelectStyle == MapSelectStyle.FreeHand) 
                m_SelectPath.CloseFigure();

            foreach (Defect InDefect in m_arrDefect)
            {
                if (m_arrSelDefectNumber.Count != 0 && m_arrSelDefectNumber.IndexOf(InDefect.CLASSNUMBER) < 0)
                    continue;

                pdDefectCenter.X = InDefect.X;
                pdDefectCenter.Y = InDefect.Y;

                RotatePoint(ref pdDefectCenter.X, ref pdDefectCenter.Y, m_iViewAngle);

                PointF pfPoint = PointF.Empty;

                if (ZeroPointLocation == ZeroPoint.Center)
                {
                    pfPoint.X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_X / 2 + pdDefectCenter.X) * (m_dZoomRatio * m_dScale));
                    pfPoint.Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y / 2 - pdDefectCenter.Y) * (m_dZoomRatio * m_dScale));
                }
                else if (ZeroPointLocation == ZeroPoint.LowerLeft)
                {
                    pfPoint.X = (float)((-m_rectdGlassArea.X + pdDefectCenter.X) * (m_dZoomRatio * m_dScale));
                    pfPoint.Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_Y - pdDefectCenter.Y) * (m_dZoomRatio * m_dScale));
                }

                if (m_SelectPath.IsVisible(pfPoint))
                {
                    DefectHighlight(InDefect);
                    m_SelectedDefect.Add(InDefect);
                }
            }
        }

        private void MMGDefectMap_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            SetDefaultColor();
            Reset();
            base.GlassMap_Resize(null, null);
        }

        protected override void OnResize(EventArgs e)
        {
            if (DesignMode) return;
            base.OnResize(e);
        }

        protected override void OnPrint(PaintEventArgs e)
        {
            if (m_gdiTempMap == null) return;
            base.OnPrint(e);
        }

        public override void Refresh()
        {
            base.Refresh();
            Redraw();
        }

        public void SetSelected(Defect d)
        {
            m_SelectedDefect.Add(d);
            Redraw();
            DefectHighlight();
        }

        public void ClearSelectedDefect()
        {
            m_SelectedDefect.Clear();
            Redraw();
            DefectHighlight();
        }

        /// <summary>
        /// 추가된 CLASSNUMBER에 대한 값을 가져옵니다.
        /// </summary>
        public List<ClassNumberInfo> AddedClassNumberList
        {
            get;
            private set;
        }

        private string m_drawDefects;

        /// <summary>
        /// Map에 그릴 Defect을 지정합니다. ALL인 경우 모든 Defect을 그리고, 특정 Defect만 그릴 경우 CLASSNUMBER를 쉼표를 이용해서 (예. 1,2,4) 적어줍니다.
        /// </summary>
        [DefaultValue("ALL")]
        [Description("Map에 그릴 Defect을 지정합니다. ALL인 경우 모든 Defect을 그리고, 특정 Defect만 그릴 경우 CLASSNUMBER를 쉼표를 이용해서 (예. 1,2,4) 적어줍니다.")]
        public string DrawDefects
        {
            get { return m_drawDefects; }
            set { m_drawDefects = value; Redraw(); OnViewChanged(EventArgs.Empty); }
        }

        /// <summary>
        /// 눈금선을 그릴지를 설정하거나 가져옵니다.
        /// </summary>
        [DefaultValue(false)]
        [Description("Map에 눈금선을 그릴지를 지정합니다.")]
        public bool ShowGridLine
        {
            get;
            set;
        }

        /// <summary>
        /// X 방향 눈금선의 갯수를 설정하거나 가져옵니다.
        /// </summary>
        [DefaultValue(0)]
        [Description("X 방향 눈금선의 갯수를 설정하거나 가져옵니다.")]
        public int GridLineXCount
        {
            get;
            set;
        }

        /// <summary>
        /// Y 방향 눈금선의 갯수를 설정하거나 가져옵니다.
        /// </summary>
        [DefaultValue(0)]
        [Description("Y 방향 눈금선의 갯수를 설정하거나 가져옵니다.")]
        public int GridLineYCount
        {
            get;
            set;
        }

        /// <summary>
        /// X 방향 눈금의 단위가 되는 값을 설정하거나 가져옵니다.
        /// </summary>
        [DefaultValue(0)]
        [Description("X 방향 눈금의 단위가 되는 값을 나타냅니다.")]
        public int GridLineXUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Y 방향 눈금의 단위가 되는 값을 설정하거나 가져옵니다.
        /// </summary>
        [DefaultValue(0)]
        [Description("Y 방향 눈금의 단위가 되는 값을 나타냅니다.")]
        public int GridLineYUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Map에 표시되는 Defect의 모양을 설정하거나 가져옵니다.
        /// </summary>
        [DefaultValue(typeof(Shape), "Rectangle")]
        [Description("Map에 표시되는 Defect의 모양을 나타냅니다.")]
        public Shape DefaultDefectShape
        {
            get;
            set;
        }

        /// <summary>
        /// X,Y축의 0점이 되는 지점을 나타냅니다. Center인 경우 Map의 한가운데가 0점이 되며, Edge인 경우 좌하단이 0점이 됩니다.
        /// </summary>
        [DefaultValue(typeof(ZeroPoint), "Center")]
        [Description("X,Y축의 0점이 되는 지점을 나타냅니다. Center인 경우 Map의 한가운데가 0점이 되며, LowerLeft인 경우 좌하단이 0점이 됩니다.")]
        public ZeroPoint ZeroPointLocation
        {
            get;
            set;
        }

        /// <summary>
        /// 좌하단에 보여줄 단위 정보를 설정하거나 가져옵니다.
        /// </summary>
        [Description("좌하단에 보여줄 단위 정보를 설정하거나 가져옵니다.")]
        public string DisplayUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 화면에 보여지는 Defect List를 가져옵니다.
        /// </summary>
        [Browsable(false)]
        public List<Defect> DisplayDefectList
        {
            get;
            private set;
        }

        /// <summary>
        /// Rotate Notch 기능을 표시할지를 설정하거나 가져옵니다.
        /// </summary>
        [Browsable(false)]
        public bool Enabled_RotateNotch
        {
            get;
            set;
        }

        /// <summary>
        /// CLASSNUMBER에 대한 정보를 MAP 하단에 보여줄지를 설정하거나 가져옵니다.
        /// </summary>
        [DefaultValue(true)]
        [Description("CLASSNUMBER에 대한 정보를 MAP 하단에 보여줄지를 나타냅니다.")]
        public bool ShowClassNumberInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Map에 추가적으로 보여줄 도형 리스트를 가져옵니다.
        /// </summary>
        [Browsable(false)]
        public List<ShapeItem> ShapeItems
        {
            get;
            private set;
        }

        /// <summary>
        /// 도형의 모양을 나타냅니다.
        /// </summary>
        public enum Shape {
            /// <summary>네모 모양을 나타냅니다.</summary>
            Rectangle,
            /// <summary>원 모양을 나타냅니다.</summary>
            Circle 
        };
        
        /// <summary>
        /// 기준이 되는 0점을 나타냅니다.
        /// </summary>
        public enum ZeroPoint
        {
            /// <summary>Map의 한가운데 입니다.</summary>
            Center,
            /// <summary>Map의 좌하단 입니다.</summary>
            LowerLeft
        };

        public class ShapeItem
        {
            public ShapeItem(Rectangle rectangle, Color color, Shape shape)
            {
                Rectangle = rectangle;
                Color = color;
                Shape = shape;
            }

            public Rectangle Rectangle
            {
                get;
                private set;
            }

            public Color Color
            {
                get;
                private set;
            }

            public Shape Shape
            {
                get;
                private set;
            }
        }

        public class ClassNumberInfo
        {
            public int ClassNumber { get; set; }
            public string Info { get; set; }
            public Color Color { get; set; }
        }
    }
}
