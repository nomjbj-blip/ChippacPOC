using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;
using DACrux.Base;
using DACrux.Base.Zone;

namespace DACrux.Map
{
    public delegate void SelectedDefect(object sender, Defect[] oDefect);

    public partial class DefectMap : DACrux.Map.WaferMap, IDefectMap
    {
        public const string DEFAULT_ZONE_LINE_COLOR = "Blue";
        public const int DEFAULT_ZONE_LINE_WIDTH = 2;

        public static readonly string[] RD_ARRAY = { "NonRD", "RD" };
        public static readonly string VIEW_IMAGE_DEFECT_ONLY = "VIEW_IMAGE_DEFECT_ONLY";
        public static readonly string VIEW_NON_IMAGE_DEFECT_ONLY = "VIEW_NON_IMAGE_DEFECT_ONLY";
        public static readonly string VIEW_ALL = "ALL";
        public const float DEFAULT_DEFECT_SIZE = 2;

        private MAP_TYPE m_mapType = MAP_TYPE.CLASS;
        private float m_DensityTerm = 1;

        public struct ColorBySize
        {
            public int No;
            public int from;
            public int to;
            public Color color;
            public string Label;
        }

        private ColorBySize[] m_SizeColor = null;
        private DataTable m_dtSizeColor;
        private DataTable m_typeColor;

        protected DefectList m_arrDefect = new DefectList();
        protected List<PointF> m_DefectIndexer = new List<PointF>();
        protected List<int> m_DefectDieIndexer = new List<int>();
        protected Dictionary<int, int> m_DefectDieIndexerCount = new Dictionary<int, int>();

        private Color[] m_DefectColor = null;
        private bool m_bRealSize = false;
        private Point m_pCurrent = Point.Empty;
        private Color m_DieColor = Color.Black;
        private Color m_DieDefectColor = Color.Empty;
        private DefectList m_SelectedDefect = new DefectList();
        private System.Windows.Forms.MenuItem mnuREALSIZEDEFECT;
        private System.Windows.Forms.MenuItem mnuitemMOUSEDRAGMODE_DIE;
        private System.Windows.Forms.MenuItem mnuitemMOUSEDRAGMODE_DEFECT;
        private System.Windows.Forms.MenuItem mnuDefectImageMark;

        public event SelectedDefect OnSelectedDefect;
        private string m_drawDefect = "ALL";
        new protected Color[] m_ColorSet = null;

        private List<Color> m_RepeatColor = null;
        private List<string> m_RepeatDefectRel = null;
        private string[] m_strDensity = new string[] { "ALL" };
        private int m_iDensityMax = -1;
        private int m_iCutCount = -1;
        private bool m_bDensity = false;

        public event ChangeDieProperty OnChangeDieProperty;

        static DefectMap()
        {
            NonRDColor = Color.Green;
            RDColor = Color.Red;
        }

        public static Color GetRDColor(int index)
        {
            return new Color[] { NonRDColor, RDColor }[index];
        }

        public DefectMap()
        {
            // 이 호출은 Windows Form 디자이너에 필요합니다.
            InitializeComponent();
            SetDefaultColor();
            Reset();
            this.MarkDieMenuVisible = false;
            DefectSize = DEFAULT_DEFECT_SIZE;

            VisibleZone = false;
            VisibleZoneID = false;
            ZoneLineColor = Color.FromName(DEFAULT_ZONE_LINE_COLOR);
            ZoneLineWidth = DEFAULT_ZONE_LINE_WIDTH;

            ZoneConfig = new Base.Zone.ZoneConfig();
            ZoneConfig.Read();
            ZoneConfig.CalculateZoneItem();
            UseZoneIDSelectControlPopup = true;

            //mnuitem_VISIBLE_ZONE.Visible = false;
            //mnuitem_ZONE_OPTION.Visible = false;

            m_ItemCountColorList = new ItemCountColorList();
            VisibleDie = true;
        }

        #region ◈ Die 속성

        [DefaultValue(true)]
        public bool VisibleDie
        {
            get;
            set;
        }

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

        public Color DieDefectColor
        {
            set { m_DieDefectColor = value; }
            get { return m_DieDefectColor; }
        }

        public string SetDensity
        {
            set
            {
                m_strDensity = value.Split(',');
            }
        }

        public int SetDensityMax
        {
            set
            {
                m_iDensityMax = value;
            }
        }

        public int SetCutCount
        {
            set
            {
                m_iCutCount = value;
            }
        }

        public bool SetDensityEnable
        {
            set
            {
                m_bDensity = value;
            }
        }

        /// <summary>
        /// Defect Count 별로 색상 표현 추가
        /// </summary>
        [Browsable(false)]
        public ItemCountColorList SetItemCountColorList
        {
            get { return m_ItemCountColorList; }
        }

        public void DefectSelectMode()
        {
            mnuitemMOUSEDRAGMODE_DEFECT_Click(null, null);
        }

        public void DieSelectMode()
        {
            mnuitemMOUSEDRAGMODE_DIE_Click(null, null);
        }

        #endregion

        #region ■ Repeat Color 설정

        public void SetRepeatDefectColor(List<Color> repeatColor)
        {
            m_RepeatColor = new List<Color>();
            m_RepeatColor = repeatColor;
        }

        public void SetRepeatDefectRel(List<string> repeatDefectRel)
        {
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
            get
            {
                return m_dtSizeColor;
            }

            set
            {
                if (value == null)
                    return;

                m_dtSizeColor = value;

                if (value.Columns.Count <= 0 || value.Rows.Count <= 0)
                    return;

                if (m_DefectColor == null) return;
                if (m_DefectColor.Length == 0) return;

                int iNo = value.Columns.IndexOf("NO");
                int iSizeFromCol = value.Columns.IndexOf("SIZE_FROM");
                int iSizeToCol = value.Columns.IndexOf("SIZE_TO");
                int iColorCol = value.Columns.IndexOf("COLOR");
                int iLabel = value.Columns.IndexOf("LABEL");

                if (iNo < 0 || iSizeFromCol < 0 || iSizeToCol < 0 || iColorCol < 0 || iLabel < 0)
                    return;

                m_SizeColor = null;
                m_SizeColor = new ColorBySize[value.Rows.Count];
                for (int i = 0; i < value.Rows.Count; i++)
                {
                    try
                    {
                        m_SizeColor[i].No = System.Convert.ToInt32(value.Rows[i][iNo]);
                        m_SizeColor[i].from = System.Convert.ToInt32(value.Rows[i][iSizeFromCol]);
                        m_SizeColor[i].to = System.Convert.ToInt32(value.Rows[i][iSizeToCol]);
                        m_SizeColor[i].color = ColorTranslator.FromHtml(value.Rows[i][iColorCol].ToString());
                        m_SizeColor[i].Label = value.Rows[i][iLabel].ToString();
                    }
                    catch { }
                }
            }
        }
        #endregion

        public void ChangeTypeColor(int classNumber, string color)
        {
            if (m_typeColor == null)
                return;

            DataRow[] rows = m_typeColor.Select(String.Format("CLASSNUMBER = {0}", classNumber));

            if (rows != null && rows.Length > 0)
            {
                rows[0]["COLOR"] = color;
                TypeColor = m_typeColor;
            }
        }

        #region ■ Defect Type, Roughbin, Cluster, Finebin 에 따라 Defect Color을 정한다
        public DataTable TypeColor
        {
            get
            {
                return m_typeColor;
            }

            set
            {
                m_typeColor = value;

                if (value == null)
                    return;

                if (value.Columns.Count <= 0 || value.Rows.Count <= 0)
                    return;

                if (m_DefectColor == null) return;
                if (m_DefectColor.Length == 0) return;

                int iClassCol = value.Columns.IndexOf("CLASSNUMBER");
                int iColorCol = value.Columns.IndexOf("COLOR");
                int classNumber = 0;
                Color ClassColor = Color.Empty;

                if (iClassCol < 0 || iColorCol < 0) 
                    return;

                for (int i = 0; i < m_DefectColor.Length; i++)
                    m_DefectColor[i] = Color.Black;

                for (int ir = 0; ir < value.Rows.Count; ir++)
                {
                    try
                    {
                        classNumber = DACrux.Base.Convert.intParse(value.Rows[ir][iClassCol].ToString());

                        ClassColor = ColorTranslator.FromHtml(value.Rows[ir][iColorCol].ToString());
                        m_DefectColor[classNumber] = ClassColor;
                    }
                    catch { }

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

        /// <summary>
        /// 이미지 포함 여부 Defect만 그릴것인지를 설정합니다.
        /// </summary>
        public string DrawDefectImage
        {
            get;
            set;
        }
        #endregion

        public DefectList Defects
        {
            get { return m_arrDefect; }
        }

        public DefectList SelectedDefect
        {
            get { return m_SelectedDefect; }
        }

        [DefaultValue(DEFAULT_DEFECT_SIZE)]
        public float DefectSize
        {
            get;
            set;
        }

        #region ■ Defect Array Handling 함수 ( DieClear / AddDefect(Defect NewDefect) )
        public void DefectClear()
        {
            m_arrDefect.Clear();
            m_DefectIndexer.Clear();
            m_DefectDieIndexer.Clear();
            m_DefectDieIndexerCount.Clear();

            m_SelectedDefect.Clear();
            m_SelectedDies.Clear();
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
#if DEBUG
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            w.Start();
#endif
            if (this.Width * this.Height == 0) return;

            m_dZoomRatio_X_m_dScale = m_dZoomRatio * m_dScale;

            PointD dNotchStart;
            PointD dNotchEnd;
            GraphicsPath gpWafer = null;

            m_gdiTempMap.Clear(Color.White);
            SolidBrush sbrshEdge = null;
            SolidBrush sbrshWafer = null;
            Pen pEdge = null;
            Pen pWafer = null;
            Font fntWID = new Font("굴림", Math.Max(9, (float)(m_dZoomRatio_X_m_dScale * 2.0f)));
            m_fntBin = new Font("굴림", Math.Max(5, (float)(m_dZoomRatio_X_m_dScale * 1.5f)));
            int iRealAngle = (m_WaferRecipe.ANGLE + m_iAngleOffset) % 360;
            
            try
            {
                if (m_WaferRecipe.WAFER_SIZE == 0)
                    return;

                gpWafer = new GraphicsPath();

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

                    gpWafer.AddLine((float)((-m_rectdWaferArea.X + dNotchStart.X + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - dNotchStart.Y) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.X + dNotchEnd.X + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - dNotchEnd.Y) * m_dZoomRatio_X_m_dScale));
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

                dNotchStart.X = -(float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * m_WaferRecipe.WAFER_RADIUS;
                dNotchStart.Y = -(float)Math.Cos((m_dNotchSize / 2) / 180 * Math.PI) * m_WaferRecipe.WAFER_RADIUS;
                dNotchEnd.X = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * m_WaferRecipe.WAFER_RADIUS;
                dNotchEnd.Y = dNotchStart.Y;

                RotatePoint(ref dNotchStart.X, ref dNotchStart.Y, (iRealAngle + m_iViewAngle) % 360);
                RotatePoint(ref dNotchEnd.X, ref dNotchEnd.Y, (iRealAngle + m_iViewAngle) % 360);

                if (m_WaferRecipe.NOTCH_TYPE == Notch.Notch)
                {
                    PointD dNotchRStart;
                    PointD dNotchREnd;
                    double dNotchR = (float)Math.Sin((m_dNotchSize / 2) / 180 * Math.PI) * m_WaferRecipe.WAFER_RADIUS;
                    dNotchRStart.X = (dNotchEnd.X + dNotchStart.X) / 2 - dNotchR;
                    dNotchRStart.Y = (dNotchEnd.Y + dNotchStart.Y) / 2;
                    dNotchREnd.X = (dNotchEnd.X + dNotchStart.X) / 2 + dNotchR;
                    dNotchREnd.Y = (dNotchEnd.Y + dNotchStart.Y) / 2;

                    gpWafer.AddArc((float)((-m_rectdWaferArea.X + dNotchRStart.X + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y - dNotchRStart.Y + m_WaferRecipe.WAFER_RADIUS - (dNotchREnd.X - dNotchRStart.X) / 2) * m_dZoomRatio_X_m_dScale)
                        , (float)((dNotchREnd.X - dNotchRStart.X) * m_dZoomRatio_X_m_dScale)
                        , (float)((dNotchREnd.X - dNotchRStart.X) * m_dZoomRatio_X_m_dScale)
                        , (float)((m_iViewAngle + iRealAngle + 180 + m_dNotchSize / 2) % 360)
                        , (float)(180));
                }
                else
                {
                    gpWafer.AddLine((float)((-m_rectdWaferArea.X + dNotchStart.X + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - dNotchStart.Y) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.X + dNotchEnd.X + m_WaferRecipe.WAFER_RADIUS) * m_dZoomRatio_X_m_dScale)
                        , (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - dNotchEnd.Y) * m_dZoomRatio_X_m_dScale));
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
                pdPoint[2].Y = -m_WaferRecipe.WAFER_RADIUS;
                pdPoint[3].X = pdPoint[0].X;
                pdPoint[3].Y = -m_WaferRecipe.WAFER_RADIUS;

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
#if DEBUG
                w.Stop();
                System.Diagnostics.Debug.WriteLine("Wafer 형상 : " + w.ElapsedMilliseconds);
                w = new System.Diagnostics.Stopwatch();
                w.Start();
#endif
                MakeDensityColor();  //나중에 
#if DEBUG
                w.Stop();
                System.Diagnostics.Debug.WriteLine("MakeDensityColor : " + w.ElapsedMilliseconds);
                w = new System.Diagnostics.Stopwatch();
                w.Start();
#endif
                DefectVisibleCheck();
#if DEBUG
                w.Stop();
                System.Diagnostics.Debug.WriteLine("DefectVisibleCheck : " + w.ElapsedMilliseconds);
                w = new System.Diagnostics.Stopwatch();
                w.Start();
#endif
                if (VisibleDie)
                    DrawDies(m_gdiTempMap);
#if DEBUG
                w.Stop();
                System.Diagnostics.Debug.WriteLine("DrawDies : " + w.ElapsedMilliseconds);
                w = new System.Diagnostics.Stopwatch();
                w.Start();
#endif
                DrawDefect(m_gdiTempMap);
#if DEBUG
                w.Stop();
                System.Diagnostics.Debug.WriteLine("DrawDefect : " + w.ElapsedMilliseconds);
                w = new System.Diagnostics.Stopwatch();
                w.Start();
#endif
                if (VisibleDie)
                    DrawDieBorder(m_gdiTempMap);
#if DEBUG
                w.Stop();
                System.Diagnostics.Debug.WriteLine("DrawDieBorder : " + w.ElapsedMilliseconds);
                w = new System.Diagnostics.Stopwatch();
                w.Start();
#endif
                DrawShot(m_gdiTempMap);
#if DEBUG
                w.Stop();
                System.Diagnostics.Debug.WriteLine("DrawShot : " + w.ElapsedMilliseconds);
                w = new System.Diagnostics.Stopwatch();
                w.Start();
#endif
                DrawZone(m_gdiTempMap);

                /// Wafer ID Draw
                ///========================================================================================================================
                ///
                if (m_strInfomation != null && m_bVisibleInfo)
                {
                    for (int i = 0; i < m_strInfomation.Count; i++)
                    {
                        m_gdiTempMap.DrawString(m_strInfomation[i].ToString(), fntWID, Brushes.Black,// new SolidBrush(Color.DarkGray), 
                            //(float)((-m_rectdWaferArea.X + 4.0) * m_dZoomRatio_X_m_dScale), (float)((-m_rectdWaferArea.Y + 4.0) * m_dZoomRatio_X_m_dScale + (fntWID.Height * i)));
                            2, Height - fntWID.Height * m_strInfomation.Count + fntWID.Height * i);
                    }
                }
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (m_eoMouseDragMode == MouseDragMode.Defect)
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    m_SelectedDefect.Clear();

                    foreach (Defect defect in m_arrDefect)
                    {
                        if (!defect.Visible || (VisibleZone && !defect.IncludeInZone))
                            continue;

                        m_SelectedDefect.Add(defect);
                    }

                    Redraw();

                    if (OnSelectedDefect != null)
                        OnSelectedDefect(this, m_SelectedDefect.ToArray());

                    return;
                }
            }

            base.OnKeyDown(e);
        }

        private void DrawDefect(Graphics g)
        {
            if (m_arrDefect == null || m_arrDefect.Count == 0)
                return;

            ZoneConfig.ZoneItemList.CalculateRegion((float)(-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS), (float)(-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS), (float)m_WaferRecipe.WAFER_SIZE, (float)m_dZoomRatio_X_m_dScale);

            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];
            SizeF sfDSize = SizeF.Empty;

            SolidBrush sbrshDie = null;

            try
            {
                sbrshDie = new SolidBrush(Color.LightPink);
                //m_SelectedDefect.Clear();

                if(Color.Empty != m_DieDefectColor)
                    sbrshDie.Color = m_DieDefectColor;

                foreach (Defect defect in m_arrDefect)
                {
                    if (!defect.Visible)
                        continue;

                    if (m_bRealSize)
                    {
                        pdPoint[0].X = defect.X;
                        pdPoint[0].Y = defect.Y;

                        pdPoint[1].X = defect.X + defect.XSIZE;
                        pdPoint[1].Y = defect.Y;

                        pdPoint[2].X = defect.X + defect.XSIZE;
                        pdPoint[2].Y = defect.Y + defect.YSIZE;

                        pdPoint[3].X = defect.X;
                        pdPoint[3].Y = defect.Y + defect.YSIZE;
                    }
                    else
                    {
                        pdPoint[0].X = defect.X;
                        pdPoint[0].Y = defect.Y;

                        pdPoint[1].X = defect.X + defect.XSIZE * 50;
                        pdPoint[1].Y = defect.Y;

                        pdPoint[2].X = defect.X + defect.XSIZE * 50;
                        pdPoint[2].Y = defect.Y + defect.YSIZE * 50;

                        pdPoint[3].X = defect.X;
                        pdPoint[3].Y = defect.Y + defect.YSIZE * 50;
                    }

                    if (m_iViewAngle > 0)
                    {
                        RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);
                    }

                    pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[0].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[0].Y) * m_dZoomRatio_X_m_dScale);

                    pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[1].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[1].Y) * m_dZoomRatio_X_m_dScale);

                    pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[2].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[2].Y) * m_dZoomRatio_X_m_dScale);

                    pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[3].X) * m_dZoomRatio_X_m_dScale);
                    pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[3].Y) * m_dZoomRatio_X_m_dScale);

                    if (VisibleZone)
                    {
                        defect.IncludeInZone = false;

                        foreach (ZoneItem item in ZoneConfig.ZoneItemList)
                        {
                            if (item.Visible && item.Region.IsVisible(pfPoint[0], g))
                            {
                                defect.IncludeInZone = true;
                                break;
                            }
                        }

                        if (!defect.IncludeInZone)
                            continue;
                    }

                    if (m_bRealSize)
                        g.FillPolygon(new SolidBrush(DefectColor(defect)), pfPoint);
                    else
                        g.FillRectangle(new SolidBrush(DefectColor(defect)), new RectangleF((float)(pfPoint[0].X - 0.5 * DefectSize), (float)(pfPoint[0].Y - 0.5 * DefectSize), DefectSize, DefectSize));

                    if (defect.IMAGECOUNT > 0 && mnuDefectImageMark.Checked)
                        g.DrawArc(new Pen(Color.Red), (float)(pfPoint[0].X - 0.5 * DefectSize - 2), (float)(pfPoint[0].Y - 0.5 * DefectSize - 2), DefectSize + 4, DefectSize + 4, 0, 360);
                }

                for (int i = 0; i < m_SelectedDefect.Count; i++)
                {
                    DefectHilight(g, m_SelectedDefect[i]);
                }
            }
            finally
            {
                
            }
        }

        private void DefectVisibleCheck()
        {
            m_DefectDieIndexer.Clear();
            m_DefectDieIndexerCount.Clear();

            foreach (Defect defect in m_arrDefect)
            {
                // 이미지 있는 Defect만 조회
                if (DrawDefectImage == VIEW_IMAGE_DEFECT_ONLY && defect.IMAGECOUNT == 0)
                {
                    defect.Visible = false;
                }
                // 이미지 없는 Defect만 조회
                else if (DrawDefectImage == VIEW_NON_IMAGE_DEFECT_ONLY && defect.IMAGECOUNT > 0)
                {
                    defect.Visible = false;
                }
                // DSA 체크
                else if (m_drawDefect != "ALL" && this.MapType == MAP_TYPE.DSA)
                {
                    string[] strVal = m_drawDefect.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    defect.Visible = false;

                    if (!String.IsNullOrEmpty(defect.DSA))
                    {
                        string[] dsaVal = defect.DSA.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string dsa in dsaVal)
                        {
                            if (Array.IndexOf(strVal, dsa) >= 0)
                            {
                                defect.Visible = true;
                                break;
                            }
                        }
                    }
                }
                // RD 체크
                else if (m_drawDefect != "ALL" && MapType == MAP_TYPE.RD)
                {
                    defect.Visible = (Array.IndexOf<string>(RD_ARRAY, DrawDefects) == defect.RD);
                }
                //Size 관련 체크
                else if (m_drawDefect != "ALL" && MapType == MAP_TYPE.SIZE && m_SizeColor != null && m_SizeColor.Length > 0)
                {
                    defect.Visible = false;
                    for (int i = 0; i < m_SizeColor.Length; i++)
                    {
                        if (m_drawDefect == m_SizeColor[i].No.ToString() && defect.DSIZE >= m_SizeColor[i].from && defect.DSIZE <= m_SizeColor[i].to)
                        {
                            defect.Visible = true;
                            break;
                        }
                    }
                }
                // CLASSNUMBER 체크
                else if (m_drawDefect != "ALL" && !m_drawDefect.Contains(defect.CLASSNUMBER.ToString()))
                {
                    defect.Visible = false;
                }
                else if (!CustomDefectVisibleCheck)
                {
                    defect.Visible = true;
                }

                // X,Y 인덱스 별 Defect 개수 계산
                if (defect.Visible)// && SetItemCountColorList.Count > 0)
                {
                    int xy = Die.XYToInt(defect.XINDEX, defect.YINDEX);                  
                    int idx = m_DefectDieIndexer.BinarySearch(xy);

                    if (idx < 0)
                    {
                        m_DefectDieIndexer.Insert(~idx, xy);
                        m_DefectDieIndexerCount.Add(xy, 0);
                    }
                        
                    m_DefectDieIndexerCount[xy]++;
                }
            }

            // Defect Count 별 Die Visible 시 포함되지 않는 Defect은 보여주지 않도록 처리
            if (SetItemCountColorList.Count > 0)
            {
                List<int> countList = SetItemCountColorList.GetAllCount();

                foreach (Defect defect in m_arrDefect)
                {
                    // Defect 갯수 별 Die에 포함되는 Die의 Defect만 보여준다.
                    int xy = Die.XYToInt(defect.XINDEX, defect.YINDEX);

                    defect.Visible = (m_DefectDieIndexer.BinarySearch(xy) >= 0 && countList.BinarySearch(m_DefectDieIndexerCount[xy]) >= 0);
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
                        if (InDefect.DSIZE >= m_SizeColor[i].from && InDefect.DSIZE <= m_SizeColor[i].to) return m_SizeColor[i].color;
                    }
                    return Color.Red;
                case MAP_TYPE.DSA:
                    if (!String.IsNullOrEmpty(InDefect.DSA))
                    {
                        string[] dsaVal = InDefect.DSA.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var val in dsaVal)
                        {
                            int dsa;
                            if (Int32.TryParse(val, out dsa))
                                return m_DefectColor[dsa];
                        }
                    }

                    return m_DefectColor[InDefect.CLASSNUMBER];
                case MAP_TYPE.REPEAT:
                    int index = m_RepeatDefectRel.IndexOf(InDefect.REPEAT_XREL + "_" + InDefect.REPEAT_YREL);

                    if (index > -1)
                        return m_RepeatColor[index];
                    else
                        return m_DefectColor[InDefect.CLASSNUMBER];
                case MAP_TYPE.RD:
                    return DACrux.Map.DefectMap.GetRDColor(InDefect.RD);
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

            if (m_arrDies.Count == 0) return;
            
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

                    if (!bOnWafer) continue;

                    /// 0 Skip / 1 Probing / 2 Mark / 3 Density
                    /// Die를 설정값에 맞춰서 Drawing한다. 
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
                            //Density 사용에 따른 분기 처리
                            if (m_bDensity == true)
                            {
                                int nTempValue = 0;
                                nTempValue = (int)((InDie.BinNumber) / ((m_iDensityMax) / (double)m_iCutCount));

                                if (m_iDensityMax == InDie.BinNumber)
                                    nTempValue = m_iCutCount - 1;

                                if (Array.IndexOf(m_strDensity, "ALL") > -1 || Array.IndexOf(m_strDensity, nTempValue.ToString()) > -1)
                                {
                                    sbrshDie.Color = GetDensityColor(InDie);
                                }
                                else
                                    continue;
                            }
                            else
                            {
                                sbrshDie.Color = GetDensityColor(InDie);
                            }
                            break;
                        case DIE_PROP_VIRTUAL_DIE:
                            sbrshDie.Color = VirtualDieColor;
                            break;
                    }
                    //pDieBorder.Color = Color.Gray;

                    m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);

                    //Defect 이 있는 Die 의 경우 생상 표현을 정의된 색으로 출력 한다.
                    if (m_DefectDieIndexer.BinarySearch(Die.XYToInt(InDie.IndexX, InDie.IndexY)) >= 0)
                    {
                        sbrshDie.Color = m_DieDefectColor;
                        m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                    }

                    //Defect Count 별 색상 표현 List 값이 있을 경우 Count 별로 색상을 변경 해준다.
                    if ((InDie.DieProp == 1 || InDie.DieProp == 999) && m_ItemCountColorList.Count > 0)
                    {
                        int xy = Die.XYToInt(InDie.IndexX, InDie.IndexY);
                        int idx = m_DefectDieIndexer.BinarySearch(xy);
                        int defectCount = 0;

                        if (idx >= 0)
                            defectCount = m_DefectDieIndexerCount[xy];

                        Color color = m_ItemCountColorList.GetColor(defectCount);

                        if (color == Color.Empty)
                            color = WaferColor;

                        sbrshDie.Color = color;
                        m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                    }

                    if (m_dZoomRatio > 0.6)
                    {
                        if (m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) > -1)
                        {
                            g.DrawPolygon(pSelDieBorder, pfPoint);
                        }
                        else
                        {
                            if(m_bVisibleDieBorder)
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
                    if (m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) > -1)
                    {
                        g.DrawPolygon(pSelDieBorder, pfPoint);
                    }
                    else
                    {
                        if(m_bVisibleDieBorder)
                            g.DrawPolygon(pDieBorder, pfPoint);
                    }
                }

                if (m_bDrawOriginDie && pfOrigin != null)
                {
                    g.DrawPolygon(pOriginDieBorder, pfOrigin);
                }

                if (VisibleZone)
                {
                    ZoneConfig.ZoneItemList.CalculateRegion((float)(-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS), (float)(-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS), (float)m_WaferRecipe.WAFER_SIZE, (float)m_dZoomRatio_X_m_dScale);//m_WaferRecipe.WAFER_SIZE);

                    foreach (ZoneItem item in ZoneConfig.ZoneItemList)
                    {
                        if (item.Visible && item.Region != null)
                            g.FillRegion(new SolidBrush(Color.FromArgb(64, Color.Yellow)), item.Region);
                    }
                }

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

        // Defect가 Die Border Line을 덮지 말아달라는 요청에 따라 DrawDieBorder 메서드 생성. 2019.10.30 Taihi,Kim.
        // 필요없는 경우 삭제 처리해도 무방 (Die Border를 그리는 로직은 DrawDies 메서드에도 그대로 있음)
        protected void DrawDieBorder(Graphics g)
        {
            if (m_arrDies.Count == 0) return;

            PointF[] pfOrigin = null;

            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];

            /// Pen / Brush는 Dispose해야 하므로 Try밖에 선언하고 Finally에서 Dispose를 ...
            /// 메모리 확인해 본결과 효과 만점...
            Pen pSelDieBorder = null;
            Pen pDieBorder = null;
            Pen pOriginDieBorder = null;
            Pen pFirstDieBorder = null;

            bool bOnWafer = false;
            try
            {
                pSelDieBorder = new Pen(Color.Red, 3);
                pDieBorder = new Pen(m_colDieBorder);
                pOriginDieBorder = new Pen(m_colOriginDieBorder);
                pFirstDieBorder = new Pen(m_colFirstDieBorder);

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

                    if (!bOnWafer) continue;
                    
                    if (m_dZoomRatio > 0.6)
                    {
                        if (m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) > -1)
                        {
                            g.DrawPolygon(pSelDieBorder, pfPoint);
                        }
                        else
                        {
                            if (m_bVisibleDieBorder)
                                g.DrawPolygon(pDieBorder, pfPoint);
                        }
                    }

                    /// Origin / First Border를 설정값에 맞춰서 Drawing한다.
                    if (InDie.IndexX == m_WaferRecipe.ORIGIN_DIE_X && InDie.IndexY == m_WaferRecipe.ORIGIN_DIE_Y)
                    {
                        pfOrigin = new PointF[4];
                        Array.Copy(pfPoint, pfOrigin, 4);
                    }

                    if (m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) > -1)
                    {
                        g.DrawPolygon(pSelDieBorder, pfPoint);
                    }
                    else
                    {
                        if (m_bVisibleDieBorder)
                            g.DrawPolygon(pDieBorder, pfPoint);
                    }
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

            m_arrDefect.Clear();
            m_DefectIndexer.Clear();
            m_DefectDieIndexer.Clear();
            m_DefectDieIndexerCount.Clear();
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
                //PointD pdSelectPoint = GetRealPoint(m_pCurrent.X, m_pCurrent.Y);

                foreach (Defect InDefect in m_arrDefect)
                {
                    if (InDefect.XINDEX == m_iCurrentX && InDefect.YINDEX == m_iCurrentY)
                    {
                        arrDefect.Add(InDefect);
                    }

                    //if (Math.Pow((InDefect.X - m_WaferRecipe.ORIGIN_X - pdSelectPoint.X), 2.0d) + Math.Pow((InDefect.Y - m_WaferRecipe.ORIGIN_Y - pdSelectPoint.Y), 2.0d) < Math.Pow((7.0f / m_dZoomRatio), 2))
                    //{
                    //    DefectHilight(m_gdiMain, InDefect);
                    //    arrDefect.Add(InDefect);
                    //}
                }

                Defect[] oDefect = new Defect[arrDefect.Count];

                arrDefect.CopyTo(oDefect);

                if (OnSelectedDefect != null) OnSelectedDefect(this, oDefect);
                
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

        private void DefectHilight(Graphics g, Defect InDefect)
        {
            if (m_arrDefect == null || m_arrDefect.Count == 0) return;

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

                pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[0].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[0].Y) * m_dZoomRatio_X_m_dScale);

                pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[1].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[1].Y) * m_dZoomRatio_X_m_dScale);

                pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[2].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[2].Y) * m_dZoomRatio_X_m_dScale);

                pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdPoint[3].X) * m_dZoomRatio_X_m_dScale);
                pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdPoint[3].Y) * m_dZoomRatio_X_m_dScale);


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (m_bRealSize) g.FillPolygon(new SolidBrush(m_DefectColor[InDefect.CLASSNUMBER]), pfPoint);

                g.FillRectangle(new SolidBrush(m_DefectColor[InDefect.CLASSNUMBER]), new RectangleF((float)(pfPoint[0].X - 0.5 * DefectSize), (float)(pfPoint[0].Y - 0.5 * DefectSize), DefectSize, DefectSize));

                g.DrawArc(new Pen(Color.Blue), (float)(pfPoint[0].X - 0.5 * DefectSize - 2), (float)(pfPoint[0].Y - 0.5 * DefectSize - 2), DefectSize + 4, DefectSize + 4, 0, 360);

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

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_DEFECT.Checked = true;
            mnuitemMOUSEDRAGMODE_DIE.Checked = false;
            MenuManagment();
        }

        private void mnuitemMOUSEDRAGMODE_DIE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Zone;

            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_DEFECT.Checked = false;
            mnuitemMOUSEDRAGMODE_DIE.Checked = true;
            MenuManagment();
        }

        protected new void MenuManagment()
        {
            menuitemMAPMODE_ZOOMIN.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;
            menuitemMAPMODE_ZOOMOUT.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;
            mnuitemMOUSEDRAGMODE_MOVE.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;

            mnuitem_SPRIT3.Enabled = 
                mnuitemMAPSELECTSTYLE_CIRCLE.Enabled = 
                mnuitemMAPSELECTSTYLE_PIE.Enabled = 
                mnuitemMAPSELECTSTYLE_RECTANGLE.Enabled = 
                mnuitemMAPSELECTSTYLE_FREEHAND.Enabled = 
                mnuitemMAPSELECTSTYLE_BAND.Enabled = 
                (mnuitemMOUSEDRAGMODE_ZONE.Checked || mnuitemMOUSEDRAGMODE_DEFECT.Checked) || mnuitemMOUSEDRAGMODE_DIE.Checked;
        }

        public void ClearSelectedDefect()
        {
            SelectedDefect.Clear();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
#if DEBUG
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            w.Start();
#endif
            CalSelectedDie();
#if DEBUG
            w.Stop();
            System.Diagnostics.Debug.WriteLine("CalSelectedDie : " + w.ElapsedMilliseconds);
            w = new System.Diagnostics.Stopwatch();
            w.Start();
#endif
            CalSelectedDefect();
#if DEBUG
            w.Stop();
            System.Diagnostics.Debug.WriteLine("CalSelectedDefect : " + w.ElapsedMilliseconds);
            w = new System.Diagnostics.Stopwatch();
            w.Start();
#endif
            Redraw();

            //if (m_SelectedDefect.Count > 0)
            //{
                if (OnSelectedDefect != null) OnSelectedDefect(this, m_SelectedDefect.ToArray());
            //}
        }

        private void CalSelectedDefect()
        {

            if (m_SelectPath == null) return;

            PointD pdDefectCenter;

            if (m_eoMapSelectStyle == MapSelectStyle.FreeHand) m_SelectPath.CloseFigure();

            if (ModifierKeys != Keys.Shift && ModifierKeys != Keys.ShiftKey &&
                ModifierKeys != Keys.Control && ModifierKeys != Keys.ControlKey)
                m_SelectedDefect.Clear();

            if (m_eoMouseDragMode != MouseDragMode.Defect)
                return;

            foreach (Defect defect in m_arrDefect)
            {
                if (!defect.Visible || (VisibleZone && !defect.IncludeInZone))
                    continue;

                pdDefectCenter.X = defect.X;
                pdDefectCenter.Y = defect.Y;

                RotatePoint(ref pdDefectCenter.X, ref pdDefectCenter.Y, m_iViewAngle);

                PointF pfPoint = PointF.Empty;
                pfPoint.X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + pdDefectCenter.X) * m_dZoomRatio_X_m_dScale);
                pfPoint.Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - pdDefectCenter.Y) * m_dZoomRatio_X_m_dScale);

                if (m_SelectPath.IsVisible(pfPoint))
                {
                    //DefectHilight(InDefect);
                    //Control Key 를 누르면 Defect 제외

                    if (ModifierKeys == Keys.Control || ModifierKeys == Keys.ControlKey)
                    {
                        int idx = m_SelectedDefect.BinarySearch(defect);

                        if (idx >= 0)
                            m_SelectedDefect.RemoveAt(idx);

                        //if (m_SelectedDefect.IndexOf(defect) > -1)
                        //    m_SelectedDefect.Remove(defect);
                    }
                    else
                    {
                        int idx = m_SelectedDefect.BinarySearch(defect);

                        if (idx < 0)
                            m_SelectedDefect.Insert(~idx, defect);

                        //if (m_SelectedDefect.IndexOf(defect) < 0)
                        //    m_SelectedDefect.Add(defect);
                    }
                }
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

        public int GetDefectCount(params Die[] dies)
        {
            if (dies == null || dies.Length == 0)
                return 0;

            int count = 0;

            foreach (Die die in dies)
            {
                foreach (Defect defect in m_arrDefect)
                {
                    if (defect.XINDEX == die.IndexX && defect.YINDEX == die.IndexY)
                        count++;
                }
            }

            return count;
        }

        private void ctxmWaferMap_Popup(object sender, EventArgs e)
        {
            menuItemDefectView_All.Checked = menuItemDefectView_ImageDefectOnly.Checked = menuItemDefectView_NonImageDefectOnly.Checked = false;

            menuItemDefectView_All.Checked = String.IsNullOrEmpty(DrawDefectImage);
            menuItemDefectView_ImageDefectOnly.Checked = (DrawDefectImage == VIEW_IMAGE_DEFECT_ONLY);
            menuItemDefectView_NonImageDefectOnly.Checked = (DrawDefectImage == VIEW_NON_IMAGE_DEFECT_ONLY);

            mnuitem_VISIBLE_ZONE.Checked = VisibleZone;

            if (mnuitemMAPMODE_FREEZOOM.Checked)
                mnuitemMOUSEDRAGMODE_DEFECT.Checked = mnuitemMOUSEDRAGMODE_DIE.Checked = false;
        }

        private void menuItemDefectView_Click(object sender, EventArgs e)
        {
            if (sender == menuItemDefectView_All)
                DrawDefectImage = null;
            else if (sender == menuItemDefectView_ImageDefectOnly)
                DrawDefectImage = VIEW_IMAGE_DEFECT_ONLY;
            else if (sender == menuItemDefectView_NonImageDefectOnly)
                DrawDefectImage = VIEW_NON_IMAGE_DEFECT_ONLY;

            menuItemDefectView_All.Checked = menuItemDefectView_ImageDefectOnly.Checked = menuItemDefectView_NonImageDefectOnly.Checked = false;
            (sender as MenuItem).Checked = true;

            m_SelectedDefect.Clear();

            Redraw();
        }

        public static Color NonRDColor
        {
            get;
            set;
        }

        public static Color RDColor
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public bool CustomDefectVisibleCheck
        {
            get;
            set;
        }

        #region Zone 표현 관련 로직

        public event EventHandler ZoneVisibleChanged;
        private bool _visibleZone;
        private ZoneIDSelectControlPopup _zoneIDSelect;

        protected virtual void OnZoneVisibleChanged(EventArgs e)
        {
            if (ZoneVisibleChanged != null)
                ZoneVisibleChanged(this, e);
        }

        // Zone Darw 2019.11.01 Taihi,Kim.
        protected virtual void DrawZone(Graphics g)
        {
            if (!VisibleZone)
                return;
            
            float waferSize = (float)(m_WaferRecipe.WAFER_SIZE * m_dZoomRatio * m_dScale);
            float radius = waferSize / 2f;
            float radiusEx = radius - (float)(ZoneConfig.WaferEdgeSize * m_dZoomRatio * m_dScale);
            
            PointF center = new PointF(
                (float)(-m_rectdWaferArea.X * m_dZoomRatio * m_dScale + radius),
                (float)(-m_rectdWaferArea.Y * m_dZoomRatio * m_dScale + radius));
            
            Pen pen = null;
            StringFormat sf = null;

            try
            {
                pen = new Pen(ZoneLineColor, ZoneLineWidth);
                
                sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                foreach (ZoneItem item in ZoneConfig.ZoneItemList)
                {
                    if (VisibleZoneID)
                    {
                        PointF pt = AngleToPointF(center, (float)(radiusEx * (item.EndRadius + item.StartRadius) / 2f), (float)((item.EndAngle + item.StartAngle) / 2f));
                        g.DrawString(item.ZoneID, Font, Brushes.Blue, pt, sf);
                    }

                    g.DrawEllipse(pen, (float)(center.X - radiusEx * item.EndRadius), (float)(center.Y - radiusEx * item.EndRadius), (float)(radiusEx * item.EndRadius * 2), (float)(radiusEx * item.EndRadius * 2));

                    g.DrawLine(pen, center, AngleToPointF(center, radius, item.StartAngle));
                }

                g.DrawEllipse(pen, center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
            }
            finally
            {
                if (pen != null)
                    pen.Dispose();

                if (sf != null)
                    sf.Dispose();
            }
        }

        private PointF AngleToPointF(PointF center, float radius, float angle)
        {
            float radian = (float)(angle / 180f * Math.PI);
            float x = center.X - (float)(radius * Math.Sin(angle * Math.PI / 180));
            float y = center.Y + (float)(radius * Math.Cos(angle * Math.PI / 180));
            return new PointF(x, y);
        }

        private void mnuitem_ZONE_OPTION_Click(object sender, EventArgs e)
        {
            using (WaferMapZoneOption option = new WaferMapZoneOption())
            {   
                if (VisibleZone)
                    GetZoneOption().Visible = false;

                option.ZoneConfig = ZoneConfig;
                
                if (option.ShowDialog() == DialogResult.OK)
                {
                    Redraw();
                    VisibleZone = true;
                }
                
                if (VisibleZone)
                    GetZoneOption().Visible = true;
            }
        }

        private void mnuitem_VISIBLE_ZONE_Click(object sender, EventArgs e)
        {
            VisibleZone = !mnuitem_VISIBLE_ZONE.Checked;
            Redraw();
        }

        private ZoneIDSelectControlPopup GetZoneOption()
        {
            if (_zoneIDSelect == null)
            {
                _zoneIDSelect = new ZoneIDSelectControlPopup();
                _zoneIDSelect.DesktopLocation = PointToScreen(new Point(Right, Top));
                _zoneIDSelect.Control.Map = this;
                _zoneIDSelect.Control.BindingList();
            }

            return _zoneIDSelect;
        }

        private void ShowZoneIDSelectControlPopup()
        {
            if (DesignMode) return;

            if (!UseZoneIDSelectControlPopup)
                return;

            GetZoneOption().Visible = VisibleZone;
        }

        [DefaultValue(true)]
        [Category("Zone 속성"), Description("ZoneID 선택 Popup을 보여줄지를 나타냅니다.")]
        public bool UseZoneIDSelectControlPopup
        {
            get;
            set;
        }

        [DefaultValue(false)]
        [Category("Zone 속성"), Description("Zone을 보여줄지를 나타냅니다.")]
        public bool VisibleZone
        {
            get { return _visibleZone; }
            set { _visibleZone = value; ShowZoneIDSelectControlPopup(); OnZoneVisibleChanged(EventArgs.Empty); }
        }

        [DefaultValue(false)]
        [Category("Zone 속성"), Description("ZoneID을 보여줄지를 나타냅니다.")]
        public bool VisibleZoneID
        {
            get;
            set;
        }

        [DefaultValue(typeof(Color), DEFAULT_ZONE_LINE_COLOR)]
        [Category("Zone 속성"), Description("Zone의 선 색상을 나타냅니다.")]
        public Color ZoneLineColor
        {
            get;
            set;
        }

        [DefaultValue(DEFAULT_ZONE_LINE_WIDTH)]
        [Category("Zone 속성"), Description("Zone의 선 두께를 나타냅니다.")]
        public int ZoneLineWidth
        {
            get;
            set;
        }

        [Category("Zone 속성"), Description("Zone 설정값을 나타냅니다.")]
        public DACrux.Base.Zone.ZoneConfig ZoneConfig
        {
            get;
            private set;
        }

        #endregion
    }
}
