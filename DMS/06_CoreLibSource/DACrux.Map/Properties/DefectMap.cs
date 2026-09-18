using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;
using DACrux.Base;

namespace DACrux.Map
{
    public delegate void SelectedDefect(object sender, Defect[] oDefect);
    public partial class DefectMap : DACrux.Map.WaferMap
    {
        private MAP_TYPE m_mapType = MAP_TYPE.CLASS;
        private float m_DensityTerm = 1;

        public struct ColorBySize
        {
            public int from;
            public int to;
            public Color color;
        }
        private ColorBySize[] m_SizeColor = null;

        protected ArrayList m_arrDefect = null;
        protected ArrayList m_DefectIndexer = null;
        //protected ArrayList m_DefectClickIndexer = null;
        private Color[] m_DefectColor = null;
        private bool m_bRealSize = false;
        private Point m_pCurrent = Point.Empty;
        private Color m_DieColor = Color.White;
        private ArrayList m_SelectedDefect = null;
        private System.Windows.Forms.MenuItem mnuREALSIZEDEFECT;
        private System.Windows.Forms.MenuItem mnuitemMOUSEDRAGMODE_DEFECT;
        private System.Windows.Forms.MenuItem mnuDefectImageMark;

        public event SelectedDefect OnSelectedDefect;
        private string m_drawDefect = "ALL";
        new protected Color[] m_ColorSet = null;

        private ArrayList m_RepeatColor = null;
        private ArrayList m_RepeatDefectRel = null;

        public event ChangeDieProperty OnChangeDieProperty;

        public DefectMap()
        {
            // 이 호출은 Windows Form 디자이너에 필요합니다.
            InitializeComponent();
            SetDefaultColor();
            Reset();
        }

        #region ◈ Die 속성

        public bool VisibleProbeOverlay
        {
            set
            {
                base.m_bVisibleDieValue = value;
            }
            get
            {
                return base.m_bVisibleDieValue;
            }
        }

        /// <summary>
        /// Image 가 있을 경우 표시되는 부분에 대한 사용 여부 확인
        /// </summary>
        public bool VisibleImageMark
        {
            set
            {
                mnuDefectImageMark.Checked = value;
            }
            get
            {
                return mnuDefectImageMark.Checked;
            }
        }
        [Category("Die 속성"), Description("Die의 Border Color를 설정하거나 가져옵니다.")]
        public Color DieBackgroundColor
        {
            set { m_DieColor = value; }
            get { return m_DieColor; }
        }
        #endregion

        #region ■ Repeat Color 설정

        public void SetRepeatDefectColor(ArrayList repeatColor)
        {
            m_RepeatColor = new ArrayList();
            m_RepeatColor = repeatColor;
        }

        public void SetRepeatDefectRel(ArrayList repeatDefectRel)
        {
            m_RepeatDefectRel = new ArrayList();
            m_RepeatDefectRel = repeatDefectRel;
        }

        #endregion

        #region ■ Map Type을 정해준다(color by CLASS, SIZE, ROUGHBIN, FINEBIN, CLUSTER)

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
                int r = 0;
                int g = 0;
                int b = 0;
                for (int i = 0; i < value.Rows.Count; i++)
                {
                    //m_SizeColor[i].from = (int)(decimal)value.Rows[i]["SIZE_FROM"];
                    //m_SizeColor[i].to = (int)(decimal)value.Rows[i]["SIZE_TO"];
                    m_SizeColor[i].from = System.Convert.ToInt32(value.Rows[i]["SIZE_FROM"]);
                    m_SizeColor[i].to = System.Convert.ToInt32(value.Rows[i]["SIZE_TO"]);
                    color = (string)value.Rows[i]["COLOR"];
                    r = int.Parse(color.Substring(0, 3));
                    g = int.Parse(color.Substring(3, 3));
                    b = int.Parse(color.Substring(6, 3));
                    m_SizeColor[i].color = Color.FromArgb(r, g, b);
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
                int r = 0;
                int g = 0;
                int b = 0;
                for (int i = 0; i < value.Rows.Count; i++)
                {
                    color = (string)value.Rows[i]["COLOR"];
                    classNumber = System.Convert.ToInt32(value.Rows[i]["CLASSNUMBER"]);

                    
                    r = int.Parse(color.Substring(0, 3));
                    g = int.Parse(color.Substring(3, 3));
                    b = int.Parse(color.Substring(6, 3));
                    m_DefectColor[classNumber] = Color.FromArgb(r, g, b);
                }
            }
        }

        public void SetDSAColor(Color New, Color CarryOver, Color Missing)
        {
            m_DefectColor[0] = New;
            m_DefectColor[1] = CarryOver;
            m_DefectColor[2] = Missing;
        }

        public void SetDSAColor(Color[] oDSAColor)
        {
            for (int ir = 0; ir < oDSAColor.Length; ir++)
            {
                m_DefectColor[ir] = oDSAColor[ir];
            }
        }

        #endregion

        #region ■ 그려야 할 defect 목록을 정한다
        public string DrawDefects
        {
            get { return m_drawDefect; }
            set { m_drawDefect = value; }
        }
        #endregion

        public object Defects
        {
            get { return (object)m_arrDefect.ToArray(typeof(Defect)); }
        }

        #region ■ Defect Array Handling 함수 ( DieClear / AddDefect(Defect NewDefect) )
        public void DefectClear()
        {
            m_arrDefect.Clear();
            m_DefectIndexer.Clear();
        }

        public void AddDefect(Defect NewDefect)
        {
            m_arrDefect.Add(NewDefect);
            m_DefectIndexer.Add(new PointF((float)NewDefect.X, (float)NewDefect.Y));
        }
        #endregion

        #region ■ Draw Wafer
        protected override void DrawWafer()
        {
            if (this.Width * this.Height == 0) return;

            PointD dNotchStart;
            PointD dNotchEnd;
            GraphicsPath gpWafer = null;

            m_gdiTempMap.Clear(Color.White);
            SolidBrush sbrshEdge = null;
            SolidBrush sbrshWafer = null;
            Pen pEdge = null;
            Pen pWafer = null;
            Font fntWID = new Font("굴림", Math.Max(6, (float)((m_dZoomRatio * m_dScale) * 2.0f)));
            m_fntBin = new Font("굴림", Math.Max(5, (float)((m_dZoomRatio * m_dScale) * 1.5f)));
            int iRealAngle = (m_WaferRecipe.ANGLE + m_iAngleOffset) % 360;
            try
            {
                gpWafer = new GraphicsPath();

                ///=======Wafer Edge Drawing ========================================================================
                gpWafer.AddArc((float)((-m_rectdWaferArea.X + m_WaferRecipe.EDGE_SIZE) * (m_dZoomRatio * m_dScale))
                    , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.EDGE_SIZE) * (m_dZoomRatio * m_dScale))
                    , (float)((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) * (m_dZoomRatio * m_dScale))
                    , (float)((m_WaferRecipe.WAFER_SIZE - m_WaferRecipe.EDGE_SIZE * 2) * (m_dZoomRatio * m_dScale))
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

                    gpWafer.AddLine((float)((-m_rectdWaferArea.X + dNotchStart.X + (m_WaferRecipe.WAFER_SIZE / 2)) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE / 2) - dNotchStart.Y) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdWaferArea.X + dNotchEnd.X + (m_WaferRecipe.WAFER_SIZE / 2)) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE / 2) - dNotchEnd.Y) * (m_dZoomRatio * m_dScale)));
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
                gpWafer.AddArc((float)(-m_rectdWaferArea.X * (m_dZoomRatio * m_dScale))
                    , (float)(-m_rectdWaferArea.Y * (m_dZoomRatio * m_dScale))
                    , (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale))
                    , (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale))
                    , (float)((m_iViewAngle + iRealAngle + 90 + m_dNotchSize / 2) % 360)
                    , (float)(360 - m_dNotchSize));

                dNotchStart.X = -(float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_SIZE / 2);
                dNotchStart.Y = -(float)Math.Cos((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_SIZE / 2);
                dNotchEnd.X = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_SIZE / 2);
                dNotchEnd.Y = dNotchStart.Y;

                RotatePoint(ref dNotchStart.X, ref dNotchStart.Y, (iRealAngle + m_iViewAngle) % 360);
                RotatePoint(ref dNotchEnd.X, ref dNotchEnd.Y, (iRealAngle + m_iViewAngle) % 360);

                if (m_WaferRecipe.NOTCH_TYPE == Notch.Notch)
                {
                    PointD dNotchRStart;
                    PointD dNotchREnd;
                    double dNotchR = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * (m_WaferRecipe.WAFER_SIZE / 2);
                    dNotchRStart.X = (dNotchEnd.X + dNotchStart.X) / 2 - dNotchR;
                    dNotchRStart.Y = (dNotchEnd.Y + dNotchStart.Y) / 2;
                    dNotchREnd.X = (dNotchEnd.X + dNotchStart.X) / 2 + dNotchR;
                    dNotchREnd.Y = (dNotchEnd.Y + dNotchStart.Y) / 2;

                    gpWafer.AddArc((float)((-m_rectdWaferArea.X + dNotchRStart.X + (m_WaferRecipe.WAFER_SIZE / 2)) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdWaferArea.Y - dNotchRStart.Y + (m_WaferRecipe.WAFER_SIZE / 2) - (dNotchREnd.X - dNotchRStart.X) / 2) * (m_dZoomRatio * m_dScale))
                        , (float)((dNotchREnd.X - dNotchRStart.X) * (m_dZoomRatio * m_dScale))
                        , (float)((dNotchREnd.X - dNotchRStart.X) * (m_dZoomRatio * m_dScale))
                        , (float)((m_iViewAngle + iRealAngle + 180 + m_dNotchSize / 2) % 360)
                        , (float)(180));
                }
                else
                {
                    gpWafer.AddLine((float)((-m_rectdWaferArea.X + dNotchStart.X + (m_WaferRecipe.WAFER_SIZE / 2)) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE / 2) - dNotchStart.Y) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdWaferArea.X + dNotchEnd.X + (m_WaferRecipe.WAFER_SIZE / 2)) * (m_dZoomRatio * m_dScale))
                        , (float)((-m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE / 2) - dNotchEnd.Y) * (m_dZoomRatio * m_dScale)));
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
                pdPoint[2].Y = -(m_WaferRecipe.WAFER_SIZE / 2);
                pdPoint[3].X = pdPoint[0].X;
                pdPoint[3].Y = -(m_WaferRecipe.WAFER_SIZE / 2);

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
                MakeDensityColor();  //나중에 

                DrawDies(m_gdiTempMap);
                DrawDefect(m_gdiTempMap);

                /// Wafer ID Draw
                ///========================================================================================================================
                ///
                if (m_strInfomation != null && m_bVisibleInfo)
                {
                    for (int i = 0; i < m_strInfomation.Count; i++)
                    {
                        m_gdiTempMap.DrawString(m_strInfomation[i].ToString(), fntWID, new SolidBrush(Color.DarkGray), (float)((-m_rectdWaferArea.X + 4.0) * (m_dZoomRatio * m_dScale))
                            , (float)((-m_rectdWaferArea.Y + 4.0) * (m_dZoomRatio * m_dScale) + (fntWID.Height * i)));
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

        #region ■ Draw Defect
        private void DrawDefect(Graphics g)
        {
            if (m_arrDefect == null || m_arrDefect.Count <= 0) return;
            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];
            SizeF sfDSize = SizeF.Empty;
            try
            {
                foreach (Defect InDefect in m_arrDefect)
                {
                    //DSA 의 경우 분기 함.
                    if (m_drawDefect != "ALL" && this.MapType == MAP_TYPE.DSA)
                    {
                        string[] strVal = m_drawDefect.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        int index = Array.IndexOf(strVal, InDefect.DSA.ToString());

                        if (index < 0) continue;

                        //if (int.Parse(m_drawDefect) != InDefect.DSA) continue;

                        pdPoint[0].X = InDefect.X;
                        pdPoint[0].Y = InDefect.Y;

                        pdPoint[1].X = InDefect.X + InDefect.XSIZE * 50;
                        pdPoint[1].Y = InDefect.Y;

                        pdPoint[2].X = InDefect.X + InDefect.XSIZE * 50;
                        pdPoint[2].Y = InDefect.Y + InDefect.YSIZE * 50;

                        pdPoint[3].X = InDefect.X;
                        pdPoint[3].Y = InDefect.Y + InDefect.YSIZE * 50;
                    }
                    else
                    {
                        if (m_drawDefect != "ALL" && m_drawDefect.Equals(InDefect.CLASSNUMBER.ToString()) == false) continue;
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
                    }

                    RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                    pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                    pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                    pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                    pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                    pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                    pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                    pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                    pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));


                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //if(m_bRealSize) g.FillPolygon(new SolidBrush(m_DefectColor[InDefect.CLASSNUMBER]),pfPoint);
                    //g.FillRectangle(new SolidBrush(m_DefectColor[InDefect.CLASSNUMBER]),new RectangleF(pfPoint[0].X,pfPoint[0].Y,4.0f,4.0f));
                    if (m_bRealSize) g.FillPolygon(new SolidBrush(DefectColor(InDefect)), pfPoint);
                    g.FillRectangle(new SolidBrush(DefectColor(InDefect)), new RectangleF(pfPoint[0].X, pfPoint[0].Y, 4.0f, 4.0f));
                    
                    if (InDefect.IMAGECOUNT > 0 && mnuDefectImageMark.Checked == true)
                    {
                        g.DrawArc(new Pen(Color.Red), pfPoint[0].X - 2, pfPoint[0].Y - 2, 8, 8, 0, 360);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                case MAP_TYPE.DSA:
                    return m_DefectColor[InDefect.DSA];

                // 2014.04.05 추가 : 주영진
                // Repeat Defect 이라면 Repeat Defect Color 사용, 아니면 CLASSNUMBER Color 사용
                case MAP_TYPE.REPEAT:
                    int index = m_RepeatDefectRel.IndexOf(InDefect.REPEAT_XREL + "_" + InDefect.REPEAT_YREL);

                    if (index > -1)
                        return (Color)m_RepeatColor[index];
                    else
                        return m_DefectColor[InDefect.CLASSNUMBER];
                default:
                    return m_DefectColor[InDefect.CLASSNUMBER];
            }
        }
        #endregion

        #region ■ Die Drawing
        protected override void DrawDies(Graphics g)
        {
            if (this.VisibleDieValue)
            {
                base.DrawDies(g);
                return;
            }

            if (m_arrDies.Count <= 0) return;
            m_WaferRecipe.NETDIE = 0;

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

            bool bOnWafer = false;
            try
            {
                pSelDieBorder = new Pen(Color.Red, 3);
                pDieBorder = new Pen(m_colDieBorder);
                pOriginDieBorder = new Pen(m_colOriginDieBorder);
                pFirstDieBorder = new Pen(m_colFirstDieBorder);
                sbrshDie = new SolidBrush(Color.White);

                foreach (Die InDie in m_arrDies)
                {
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

                    pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                    pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                    pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                    pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                    pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                    pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                    pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                    pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    bOnWafer = this.UseDie(InDie.DieCood.X - m_WaferRecipe.ORIGIN_X
                        , InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y
                        , this.m_WaferRecipe.DIE_SIZE_X
                        , this.m_WaferRecipe.DIE_SIZE_Y);

                    if (!bOnWafer) continue;

                    /// 0 Skip / 1 Probing / 2 Mark / 3 Density
                    /// Die를 설정값에 맞춰서 Drawing한다. 
                    /// 
                    switch (InDie.DieProp)
                    {
                        case 0:
                            sbrshDie.Color = Color.Gray;
                            break;
                        case 1:
                            if(m_ColorSet != null)
                            if (MapType == MAP_TYPE.OVERLAY
                                || MapType == MAP_TYPE.DSA
                                || MapType == MAP_TYPE.REPEAT)
							sbrshDie.Color = m_ColorSet[InDie.BinNumber];
                            sbrshDie.Color = m_DieColor;
                            break;
                        case 3:
                            sbrshDie.Color = GetDensityColor(InDie);
                            break;
                    }
                    pDieBorder.Color = Color.Gray;

                    m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                    if (m_dZoomRatio > 0.6)
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

                    //////////////////////////////////////////////////////////////////////////////////////////
                    ///

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

        Color GetDensityColor(Die die)
        {
            try
            {
                if (float.IsInfinity(m_DensityTerm))
                    return Color.FromArgb(255, 255, 255); 

                if ((int)(m_DensityTerm * die.BinNumber) > 255)
                    return Color.FromArgb(255, 255 - 255, 255);

                return Color.FromArgb(255, 255 - (int)(m_DensityTerm * die.BinNumber), 255);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void MakeDensityColor()
        {
            try
            {
                int min = int.MaxValue;
                int max = int.MinValue;

                foreach (Die InDie in m_arrDies)
                {
                    if (min > InDie.BinNumber) min = InDie.BinNumber;
                    if (max < InDie.BinNumber) max = InDie.BinNumber;
                }

                int gap = max - min;
                m_DensityTerm = 255.0f / gap;

            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            //if (m_DefectClickIndexer != null)
            //{
            //    m_DefectClickIndexer.Clear();
            //    m_DefectClickIndexer = null;
            //}


            m_arrDefect = new ArrayList();
            m_DefectIndexer = new ArrayList();
            //m_DefectClickIndexer = new ArrayList();
            m_SelectedDefect = new ArrayList();
        }

        protected override void SetDefaultColor()
        {
            base.SetDefaultColor();
            int r, g, b;//4337915
            try
            {
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
            catch (Exception ex)
            {
                throw ex;
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

            ArrayList arrDefect = new ArrayList();
            try
            {
                PointD pdSelectPoint = GetRealPoint(m_pCurrent.X, m_pCurrent.Y);


                foreach (Defect InDefect in m_arrDefect)
                {
                    if (Math.Pow((InDefect.X - m_WaferRecipe.ORIGIN_X - pdSelectPoint.X), 2.0d) + Math.Pow((InDefect.Y - m_WaferRecipe.ORIGIN_Y - pdSelectPoint.Y), 2.0d) < Math.Pow((7.0f / m_dZoomRatio), 2))
                    {
                        DefectHilight(InDefect);
                        arrDefect.Add(InDefect);
                    }
                }

                if (arrDefect.Count > 0)
                {
                    Defect[] oDefect = new Defect[arrDefect.Count];

                    arrDefect.CopyTo(oDefect);

                    if (OnSelectedDefect != null) OnSelectedDefect(this, oDefect);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrDefect = null;
            }
        }

        private void DefectHilight(Defect InDefect)
        {
            if (m_arrDefect == null || m_arrDefect.Count <= 0) return;
            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];
            SizeF sfDSize = SizeF.Empty;
            try
            {
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
                    pdPoint[1].Y = InDefect.Y - m_WaferRecipe.ORIGIN_Y;

                    pdPoint[2].X = InDefect.X + InDefect.XSIZE * 50;
                    pdPoint[2].Y = InDefect.Y + InDefect.YSIZE * 50;

                    pdPoint[3].X = InDefect.X;
                    pdPoint[3].Y = InDefect.Y + InDefect.YSIZE * 50;
                }


                RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
                pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
                pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
                pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

                pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
                pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (m_bRealSize) m_gdiMain.FillPolygon(new SolidBrush(m_DefectColor[InDefect.CLASSNUMBER]), pfPoint);
                m_gdiMain.FillRectangle(new SolidBrush(m_DefectColor[InDefect.CLASSNUMBER]), new RectangleF(pfPoint[0].X, pfPoint[0].Y, 4.0f, 4.0f));
                m_gdiMain.DrawArc(new Pen(Color.Blue), pfPoint[0].X - 2, pfPoint[0].Y - 2, 8, 8, 0, 360);

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_pCurrent.X = e.X;
            m_pCurrent.Y = e.Y;

            Point poKey = Point.Empty;
            int iDieIdx = 0;
            if (iDieIdx < 0) return;
            Die oDie;
            try
            {
                poKey.X = m_iCurrentX;
                poKey.Y = m_iCurrentY;


                iDieIdx = m_DieIndexer.IndexOf(poKey);
                if (e.Button == System.Windows.Forms.MouseButtons.Left && m_eoMouseDragMode == MouseDragMode.Normal)
                {
                    oDie = (Die)this.m_arrDies[iDieIdx];
                    int iOldDieProperty = oDie.DieProp;
                    
                    this.Redraw();
                    if (iOldDieProperty != oDie.DieProp && OnChangeDieProperty != null)
                    {
                        OnChangeDieProperty(this, oDie);
                    }
                }
            }
            catch
            {
                // 여기는 너무 빈번한 Event 이므로 Error 무시해야 함
            }
            finally
            {
                poKey = Point.Empty;
            }
        }

        private void mnuitemMOUSEDRAGMODE_DEFECT_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Defect;

            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = false;
            mnuitemMOUSEDRAGMODE_DEFECT.Checked = true;
            MenuManagment();
        }

        protected new void MenuManagment()
        {
            menuitemMAPMODE_ZOOMIN.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;
            menuitemMAPMODE_ZOOMOUT.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;
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
            if (m_eoMouseDragMode != MouseDragMode.Defect)
            {
                base.OnMouseUp(e);
                return;
            }

            try
            {
                CalSelectedDefect();
                if (m_SelectedDefect.Count > 0)
                {
                    Defect[] oDefect = new Defect[m_SelectedDefect.Count];
                    m_SelectedDefect.CopyTo(oDefect);
                    if (OnSelectedDefect != null) OnSelectedDefect(this, oDefect);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalSelectedDefect()
        {

            if (m_SelectPath == null) return;
            PointD pdDefectCenter;
            if (m_eoMapSelectStyle == MapSelectStyle.FreeHand) m_SelectPath.CloseFigure();
            try
            {
                if (ModifierKeys != Keys.Shift && ModifierKeys != Keys.ShiftKey &&
                    ModifierKeys != Keys.Control && ModifierKeys != Keys.ControlKey)
                    m_SelectedDefect.Clear();

                foreach (Defect InDefect in m_arrDefect)
                {
                    pdDefectCenter.X = InDefect.X;
                    pdDefectCenter.Y = InDefect.Y;

                    RotatePoint(ref pdDefectCenter.X, ref pdDefectCenter.Y, m_iViewAngle);

                    PointF pfPoint = PointF.Empty;
                    pfPoint.X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdDefectCenter.X) * (m_dZoomRatio * m_dScale));
                    pfPoint.Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdDefectCenter.Y) * (m_dZoomRatio * m_dScale));

                    if (m_SelectPath.IsVisible(pfPoint))
                    {
                        //DefectHilight(InDefect);
                        //Control Key 를 누르면 Defect 제외
                        if (ModifierKeys == Keys.Control || ModifierKeys == Keys.ControlKey)
                        {
                            if (m_SelectedDefect.IndexOf(InDefect) > -1)
                                m_SelectedDefect.Remove(InDefect);
                        }
                        else
                        {
                            if (m_SelectedDefect.IndexOf(InDefect) < 0)
                                m_SelectedDefect.Add(InDefect);
                        }
                    }
                }
                

                for (int ir = 0; ir < m_SelectedDefect.Count; ir++)
                {
                    DefectHilight((Defect)m_SelectedDefect[ir]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void mnuDefectImageMark_Click(object sender, EventArgs e)
        {
            try
            {
                mnuDefectImageMark.Checked = !mnuDefectImageMark.Checked;
                this.DrawWafer();
                this.Refresh();
                Application.DoEvents();
            }
            catch (Exception) { }
        }
    }
}
