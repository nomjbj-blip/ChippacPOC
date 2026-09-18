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
using System.Diagnostics;


namespace DACrux.DMSVFPD.Map
{
    public partial class FPDCellLayout : UserControl
    {
        public struct ColorBySize
        {
            public int from;
            public int to;
            public Color color;
        }

        public ArrayList m_arrInfomation = null;
      
        private MAP_TYPE m_mapType = MAP_TYPE.CLASS;
        private ColorBySize[] m_SizeColor = null;
        protected Color[] m_ColorSet;
        protected Bitmap m_bmpCellMap;
        protected Bitmap m_bmpTemp;
        protected Bitmap m_bmpGlassID;
        protected Bitmap m_bmpCellID;
		protected MapMode m_oDrawMode = MapMode.Fit;
        protected MouseDragMode m_eoMouseDragMode = MouseDragMode.Zoom;
        protected Rectangle m_rectSelect = Rectangle.Empty;
        protected Rectangle m_rectSelect2 = Rectangle.Empty;
        protected RectangleD m_rectdCellArea;
        protected Point m_poStart = Point.Empty;
        protected Point m_poEnd = Point.Empty;
        protected MapSelectStyle m_eoMapSelectStyle = MapSelectStyle.Circle;
        protected GraphicsPath m_SelectPath = null;

        protected ArrayList m_arrArrays = null;
        protected ArrayList m_ArrayIndexer = null;

        protected Cell m_CellRecipe;
        protected ArrayList m_SelectedCells = null;
        protected ArrayList m_SelectedDefect = null;
        protected ArrayList m_arrDefect = null;
        protected ArrayList m_DefectIndexer = null;
        protected ArrayList m_DefectClickIndexer = null;
        protected ArrayList m_arrSelDefectNumber = null;
        //public event SelectCells OnSelectCells;
        protected HatchBrush m_hbSelectBrush = null;
        private Color[] m_DefectColor = null;

        private Point m_pCurrent = Point.Empty;

        protected Graphics m_gdiMain = null;
        protected Graphics m_gdiTempMap = null;
        protected double m_dScale = 1.0d;
        private double m_DrawRatio = 0.5f;
        Font m_fntGID = null;
        protected Color m_colCellBorder = Color.Silver;
        protected Color m_colArrayBorder = Color.DarkGray;
        protected int m_iViewAngle = 0;
        protected int m_iAngleOffset = 0;
        protected bool m_bVisibleCellBorder = true;
        protected bool m_bVisibleArrayBorder = true;
        protected bool m_bVisibleCellValue = false;
        protected bool m_bVisibleArrayValue = false;
        protected bool m_bPopupMenu = true;
        protected bool m_bRealSize = false;

        protected long m_lMaxDefectCount = 1;
        public event SelectedDefect OnSelectedDefect;

        private double m_ARRAY_SIZE_X = 0;
        private double m_ARRAY_SIZE_Y = 0;

        private double m_GLASS_ORIGIN_X = 0;
        private double m_GLASS_ORIGIN_Y = 0;
        protected bool m_bVisibleDefectInRange = false;

        protected double m_dMaxSize = 100;
        protected double m_dMinSize = 0;


        private bool m_bImageMark = false;



        public FPDCellLayout()
        {
            InitializeComponent();
            ResetSelDefectNumber();
            SetDefaultColor();
            SetDefaultBinColor();
			m_hbSelectBrush = new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.White, Color.PowderBlue);
        }

        public void VisibleSizeChange(double dMax, double dMin)
        {
            try
            {
                m_dMaxSize = dMax;
                m_dMinSize = dMin;
                Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
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

        public void SetGlassOrigin(double x, double y)
        {
            try
            {
                m_GLASS_ORIGIN_X = x;
                m_GLASS_ORIGIN_Y = y;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetArraySize(double x, double y)
        {
            try
            {
                m_ARRAY_SIZE_X = x;
                m_ARRAY_SIZE_Y = y;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddSelDefectNumber(int iDefect)
        {
            try
            {
                if(m_arrSelDefectNumber.IndexOf(iDefect)<0)
                m_arrSelDefectNumber.Add(iDefect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetDefectColor(int i)
        {
            try
            {
                return (m_DefectColor[i].R * 256 * 256) + (m_DefectColor[i].G * 256) + (m_DefectColor[i].B);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void DeleteSelDefectNumber(int iDefect)
        {
            try
            {
                m_arrSelDefectNumber.Remove(iDefect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ResetSelDefectNumber()
        {
            try
            {
                m_arrSelDefectNumber = new ArrayList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ResetSelectedCell()
        {
            m_SelectedCells.Clear();
            this.Redraw();
        }

        public void AddInfo(string Info)
        {
            try
            {
                if (m_arrInfomation == null)
                {
                    m_arrInfomation = new ArrayList();
                }
                m_arrInfomation.Add(Info);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region ◈ Popup Menu Click Event 처리
        private void mnuitemMAPMODE_FREEZOOM_Click(object sender, System.EventArgs e)
        {
            m_oDrawMode = MapMode.Free;
            mnuitemMAPMODE_FREEZOOM.Checked = true;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            FPDCellLayout_Resize(null, null);

            MenuManagment();
            //mnuitemMOUSEDRAGMODE_ZOOM_Click(null,null);
        }

        private void mnuitemMAPMODE_FITSIZE_Click(object sender, System.EventArgs e)
        {
            m_oDrawMode = MapMode.Fit;
            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = true;
            FPDCellLayout_Resize(null, null);

            MenuManagment();

        }

        protected void mnuitemMOUSEDRAGMODE_ZOOM_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Zoom;

            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZOOM.Checked = true;
            MenuManagment();
        }

        private void mnuitemMOUSEDRAGMODE_ROTATE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Rotate;

            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = true;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZOOM.Checked = false;
            MenuManagment();
        }

        private void mnuitemMOUSEDRAGMODE_ZONE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Zone;

            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = true;
            mnuitemMOUSEDRAGMODE_ZOOM.Checked = false;
            MenuManagment();

        }

        private void mnuitemMOUSEDRAGMODE_MOVE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Move;

            mnuitemMOUSEDRAGMODE_MOVE.Checked = true;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZOOM.Checked = false;
            MenuManagment();
        }

        protected void mnuitemMAPSELECTSTYLE_CIRCLE_Click(object sender, System.EventArgs e)
        {
            m_eoMapSelectStyle = MapSelectStyle.Circle;

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
                m_eoMapSelectStyle = MapSelectStyle.Pie;

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
            m_eoMapSelectStyle = MapSelectStyle.Rectangle;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            mnuitemMAPSELECTSTYLE_BAND.Checked = false;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = false;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = true;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
        }

        protected void mnuitemMAPSELECTSTYLE_FREEHAND_Click(object sender, System.EventArgs e)
        {
            m_eoMapSelectStyle = MapSelectStyle.FreeHand;

            mnuitemMAPSELECTSTYLE_CIRCLE.Checked = false;
            mnuitemMAPSELECTSTYLE_BAND.Checked = false;
            mnuitemMAPSELECTSTYLE_FREEHAND.Checked = true;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = false;
            mnuitemMAPSELECTSTYLE_PIE.Checked = false;
        }

        protected void mnuitemMAPSELECTSTYLE_BAND_Click(object sender, System.EventArgs e)
        {
            m_eoMapSelectStyle = MapSelectStyle.Band;

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

            //mnuitem_SPRIT2.Visible = true;
            mnuitemMOUSEDRAGMODE_ZOOM.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;
            //mnuitemMOUSEDRAGMODE_ROTATE.Enabled = true;
            //mnuitemMOUSEDRAGMODE_ZONE.Enabled = true;
            mnuitemMOUSEDRAGMODE_MOVE.Enabled = !mnuitemMAPMODE_FITSIZE.Checked;

            mnuitemMAPSELECTSTYLE_CIRCLE.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked;
            mnuitemMAPSELECTSTYLE_PIE.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked;
            mnuitemMAPSELECTSTYLE_RECTANGLE.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked;
            mnuitemMAPSELECTSTYLE_FREEHAND.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked;
            mnuitemMAPSELECTSTYLE_BAND.Enabled = mnuitemMOUSEDRAGMODE_ZONE.Checked;
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
            Clipboard.SetDataObject(m_bmpCellMap);
        }
		public Bitmap GetImage()
		{
			return m_bmpCellMap;
		}
        #endregion
        
        #region ■ Rotate 시각적 표현
        private void DrawRotate()
        {
            if (this.Height / 2 > m_poStart.Y)
            {
                for (int i = 0; i < 6; i++)
                {
                    Rotate(15);
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    Rotate(-15);
                }
            }
        }
        #endregion

        public void SetRecipe(Cell oCellRecipe)
        {
            try
            {
                m_CellRecipe = oCellRecipe;
                DrawReset();
                Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FPDCellLayout_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            m_CellRecipe.CellCood.Width = 1100;//1926,2328
            m_CellRecipe.CellCood.Height = 660d;


            double dWidthHeightRatio = (this.Width * m_CellRecipe.CellCood.Height) / (this.Height * m_CellRecipe.CellCood.Width);
            double DrawSizeX = this.Width;
            double DrawSizeY = this.Height;

            m_SelectedCells = new ArrayList();
            m_SelectedDefect = new ArrayList();

            if (dWidthHeightRatio > 1)
            {
                DrawSizeX = (DrawSizeY * m_CellRecipe.CellCood.Width) / m_CellRecipe.CellCood.Height;
                m_DrawRatio = (DrawSizeX / m_CellRecipe.CellCood.Width) * 0.9d;
            }
            else
            {
                DrawSizeY = (DrawSizeX * m_CellRecipe.CellCood.Height) / m_CellRecipe.CellCood.Width;
                m_DrawRatio = (DrawSizeY / m_CellRecipe.CellCood.Height) * 0.9d;
            }

            m_arrDefect = new ArrayList();
            DrawReset();
            Redraw();
        }

        public void AddDefect(DACrux.Base.DEFECT_TAG oAddDefect)
        {
            try
            {
                m_arrDefect.Add(oAddDefect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ResetDefect()
        {
            try
            {
                m_arrDefect = new ArrayList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            Redraw();
        }

        public void DrawReset()
        {
            if (this.Width * this.Height == 0)
            {
                return;
            }
            
            try
            {
                if (m_bmpCellMap != null) m_bmpCellMap.Dispose();
                if (m_gdiTempMap != null) m_gdiTempMap.Dispose();

                m_gdiMain = this.CreateGraphics();
                m_bmpCellMap = new Bitmap(this.Width, this.Height);
                m_bmpTemp = new Bitmap(m_bmpCellMap);
                m_gdiTempMap = Graphics.FromImage(m_bmpCellMap);
                

                float dMinCanvers_x = 0f;
                float dMinCanvers_y = 0f;

                if ((m_CellRecipe.CellCood.Width / this.Width) > (m_CellRecipe.CellCood.Height / this.Height))
                {
                    dMinCanvers_x = this.Width * 0.9f;
                    m_DrawRatio = dMinCanvers_x / m_CellRecipe.CellCood.Width;
                    dMinCanvers_y = (float)(m_CellRecipe.CellCood.Height * m_DrawRatio);
                }
                else
                {
                    dMinCanvers_y = this.Height * 0.9f;
                    m_DrawRatio = dMinCanvers_y / m_CellRecipe.CellCood.Height;
                    dMinCanvers_x = (float)(m_CellRecipe.CellCood.Width * m_DrawRatio);
                }

                m_rectdCellArea = new RectangleD(((dMinCanvers_x - this.Width) / 2.0d) / (m_DrawRatio * m_dScale)
                    , ((dMinCanvers_y - this.Height) / 2.0d) / (m_DrawRatio * m_dScale)
                    , this.Width / m_DrawRatio
                    , this.Height / m_DrawRatio);

                m_fntGID = new Font("굴림", Math.Max(8, Math.Min((float)(m_DrawRatio * 1.5f), 12)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
        public void AddArray(int X, int Y, int BIN)
        {
            CellArray NewArray = new CellArray(X, Y, BIN);
            NewArray.ParametricValue = BIN;

            NewArray.ArrayCood.X = (NewArray.IndexX-1) * m_ARRAY_SIZE_X;
            NewArray.ArrayCood.Y = (NewArray.IndexY-1) * m_ARRAY_SIZE_Y;

            NewArray.ArrayCood.Width = m_ARRAY_SIZE_X;
            NewArray.ArrayCood.Height = m_ARRAY_SIZE_Y;

            m_arrArrays.Add(NewArray);
            m_ArrayIndexer.Add(new Point(X, Y));
            if (BIN > m_lMaxDefectCount) m_lMaxDefectCount = BIN;
        }

        public void ResetArray()
        {
            try
            {
                if (m_arrArrays != null) m_arrArrays = null;
                m_arrArrays = new ArrayList();

                if (m_ArrayIndexer != null) m_ArrayIndexer = null;
                m_ArrayIndexer = new ArrayList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual void DrawCell()
        {
            if (this.Width * this.Height == 0 || m_gdiTempMap == null) return;
            GraphicsPath gpCell = null;

            try
            {
                PointD[] pdPoint = new PointD[4];
                PointF[] pfPoint = new PointF[4];

                Pen pSelCellBorder = null;
                Pen pCellBorder = null;

                SolidBrush sbrshCell = null;

                try
                {
                    m_gdiTempMap.Clear(Color.SlateGray);
                    gpCell = new GraphicsPath();

                    pSelCellBorder = new Pen(Color.Red, 3);
                    pCellBorder = new Pen(m_colCellBorder,3);
                    sbrshCell = new SolidBrush(Color.White);

                    pdPoint[0].X = m_CellRecipe.CellCood.X - m_GLASS_ORIGIN_X;
                    pdPoint[0].Y = m_CellRecipe.CellCood.Y - m_GLASS_ORIGIN_Y;

                    pdPoint[1].X = m_CellRecipe.CellCood.X + m_CellRecipe.CellCood.Width - m_GLASS_ORIGIN_X;
                    pdPoint[1].Y = m_CellRecipe.CellCood.Y - m_GLASS_ORIGIN_Y;

                    pdPoint[2].X = m_CellRecipe.CellCood.X + m_CellRecipe.CellCood.Width - m_GLASS_ORIGIN_X;
                    pdPoint[2].Y = m_CellRecipe.CellCood.Y + m_CellRecipe.CellCood.Height - m_GLASS_ORIGIN_Y;

                    pdPoint[3].X = m_CellRecipe.CellCood.X - m_GLASS_ORIGIN_X;
                    pdPoint[3].Y = m_CellRecipe.CellCood.Y + m_CellRecipe.CellCood.Height - m_GLASS_ORIGIN_Y;

                    RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                    pfPoint[0].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[0].X) * (m_DrawRatio * m_dScale));
                    pfPoint[0].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[0].Y) * (m_DrawRatio * m_dScale));

                    pfPoint[1].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[1].X) * (m_DrawRatio * m_dScale));
                    pfPoint[1].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[1].Y) * (m_DrawRatio * m_dScale));

                    pfPoint[2].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[2].X) * (m_DrawRatio * m_dScale));
                    pfPoint[2].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[2].Y) * (m_DrawRatio * m_dScale));

                    pfPoint[3].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[3].X) * (m_DrawRatio * m_dScale));
                    pfPoint[3].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[3].Y) * (m_DrawRatio * m_dScale));

                    try
                    {
                        sbrshCell.Color = m_ColorSet[m_CellRecipe.BinNumber];
                        pCellBorder.Color = m_colCellBorder;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (m_CellRecipe.CellProp > -1) m_gdiTempMap.FillPolygon(sbrshCell, pfPoint);
                    if (m_DrawRatio > 0.05 && m_bVisibleCellBorder)
                    {
                        if (m_SelectedCells.IndexOf(new Point(m_CellRecipe.IndexX, m_CellRecipe.IndexY)) > -1)
                        {
                            m_gdiTempMap.DrawPolygon(pSelCellBorder, pfPoint);
                        }
                        else
                        {
                            m_gdiTempMap.DrawPolygon(pCellBorder, pfPoint);
                        }
                    }
                    DrawArray(m_gdiTempMap);
                    DrawDefect(m_gdiTempMap);
                    DrawInfo(m_gdiTempMap);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    pdPoint = null;
                    pfPoint = null;

                    if (pSelCellBorder != null) pSelCellBorder.Dispose();
                    if (pCellBorder != null) pCellBorder.Dispose();
                    if (sbrshCell != null) sbrshCell.Dispose();
                }
            }
            catch (Exception ex)
            {
                DACrux.Framework.DCMH.DspError(ex);
            }
        }

        private void DrawInfo(Graphics g)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawArray(Graphics g)
        {
            if (m_arrArrays == null) return;
            try
            {
                if (m_arrArrays == null || m_arrArrays.Count <= 0) return;

                PointD[] pdPoint = new PointD[4];
                PointF[] pfPoint = new PointF[4];

                Pen pSelArrayBorder = null;
                Pen pArrayBorder = null;

                SolidBrush sbrshArray = null;
                int iDensity = 0;

                try
                {
                    pSelArrayBorder = new Pen(Color.Red, 3);
                    pArrayBorder = new Pen(m_colArrayBorder);
                    sbrshArray = new SolidBrush(Color.Gray);

                    foreach (CellArray InArray in m_arrArrays)
                    {
                        pdPoint[0].X = InArray.ArrayCood.X - m_GLASS_ORIGIN_X;
                        pdPoint[0].Y = InArray.ArrayCood.Y - m_GLASS_ORIGIN_Y;

                        pdPoint[1].X = InArray.ArrayCood.X + InArray.ArrayCood.Width - m_GLASS_ORIGIN_X;
                        pdPoint[1].Y = InArray.ArrayCood.Y - m_GLASS_ORIGIN_Y;

                        pdPoint[2].X = InArray.ArrayCood.X + InArray.ArrayCood.Width - m_GLASS_ORIGIN_X;
                        pdPoint[2].Y = InArray.ArrayCood.Y + InArray.ArrayCood.Height - m_GLASS_ORIGIN_Y;

                        pdPoint[3].X = InArray.ArrayCood.X - m_GLASS_ORIGIN_X;
                        pdPoint[3].Y = InArray.ArrayCood.Y + InArray.ArrayCood.Height - m_GLASS_ORIGIN_Y;

                        RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                        pfPoint[0].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[0].X) * (m_DrawRatio * m_dScale));
                        pfPoint[0].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[0].Y) * (m_DrawRatio * m_dScale));

                        pfPoint[1].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[1].X) * (m_DrawRatio * m_dScale));
                        pfPoint[1].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[1].Y) * (m_DrawRatio * m_dScale));

                        pfPoint[2].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[2].X) * (m_DrawRatio * m_dScale));
                        pfPoint[2].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[2].Y) * (m_DrawRatio * m_dScale));

                        pfPoint[3].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[3].X) * (m_DrawRatio * m_dScale));
                        pfPoint[3].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[3].Y) * (m_DrawRatio * m_dScale));

                        try
                        {
                            iDensity = (int)((InArray.ParametricValue * 255)/m_lMaxDefectCount);
                            sbrshArray.Color = Color.FromArgb(iDensity,Color.FromArgb( 255, 0, 0));
                            pArrayBorder.Color = Color.FromArgb(50,m_colArrayBorder);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        g.FillPolygon(sbrshArray, pfPoint);
                        if (m_DrawRatio > 0.05 && m_bVisibleCellBorder)
                        {
                            if (m_SelectedCells.IndexOf(new Point(InArray.IndexX, InArray.IndexY)) > -1)
                            {
                                g.DrawPolygon(pSelArrayBorder, pfPoint);
                            }
                            else
                            {
                                g.DrawPolygon(pArrayBorder, pfPoint);
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
                    pdPoint = null;
                    pfPoint = null;

                    if (pSelArrayBorder != null) pSelArrayBorder.Dispose();
                    if (pArrayBorder != null) pArrayBorder.Dispose();
                    if (sbrshArray != null) sbrshArray.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region ■ Draw Defect
        private void DrawDefect(Graphics g)
        {
            if (m_arrDefect == null || m_arrDefect.Count <= 0) return;
            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];
            SizeF sfDSize = SizeF.Empty;
            try
            {
                foreach (DACrux.Base.DEFECT_TAG InDefect in m_arrDefect)
                {

                    if (m_bVisibleDefectInRange)
                    {
                        if (m_dMaxSize < InDefect.dsize || m_dMinSize > InDefect.dsize) continue;
                    }
                    if (m_arrSelDefectNumber.Count != 0)
                        if (m_arrSelDefectNumber.IndexOf(InDefect.classnumber) < 0) continue;

                    if (m_bRealSize)
                    {
                        pdPoint[0].X = InDefect.x;
                        pdPoint[0].Y = InDefect.y;

                        pdPoint[1].X = InDefect.x + InDefect.xsize;
                        pdPoint[1].Y = InDefect.y;

                        pdPoint[2].X = InDefect.x + InDefect.xsize;
                        pdPoint[2].Y = InDefect.y + InDefect.ysize;

                        pdPoint[3].X = InDefect.x;
                        pdPoint[3].Y = InDefect.y + InDefect.ysize;
                    }
                    else
                    {
                        pdPoint[0].X = InDefect.x;
                        pdPoint[0].Y = InDefect.y;

                        pdPoint[1].X = InDefect.x + InDefect.xsize * 50;
                        pdPoint[1].Y = InDefect.y;

                        pdPoint[2].X = InDefect.x + InDefect.xsize * 50;
                        pdPoint[2].Y = InDefect.y + InDefect.ysize * 50;

                        pdPoint[3].X = InDefect.x;
                        pdPoint[3].Y = InDefect.y + InDefect.ysize * 50;
                    }

                    RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                    RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);


                    pfPoint[0].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[0].X) * (m_DrawRatio * m_dScale));
                    pfPoint[0].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[0].Y) * (m_DrawRatio * m_dScale));

                    pfPoint[1].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[1].X) * (m_DrawRatio * m_dScale));
                    pfPoint[1].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[1].Y) * (m_DrawRatio * m_dScale));

                    pfPoint[2].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[2].X) * (m_DrawRatio * m_dScale));
                    pfPoint[2].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[2].Y) * (m_DrawRatio * m_dScale));

                    pfPoint[3].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[3].X) * (m_DrawRatio * m_dScale));
                    pfPoint[3].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[3].Y) * (m_DrawRatio * m_dScale));

                    if (m_bRealSize)
                    {
                        g.FillPolygon(new SolidBrush(DefectColor(InDefect)), pfPoint);
                    }else{
                        g.FillRectangle(new SolidBrush(DefectColor(InDefect)), new RectangleF(pfPoint[0].X, pfPoint[0].Y, 4.0f, 4.0f));
                    }

                    if (InDefect.imagecount > 0 && m_bImageMark)
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
        Color DefectColor(DACrux.Base.DEFECT_TAG InDefect)
        {
            switch (m_mapType)
            {
                case MAP_TYPE.CLASS:
                    return m_DefectColor[InDefect.classnumber];
                case MAP_TYPE.CLUSTER:
                    return m_DefectColor[InDefect.clusternumber];
                case MAP_TYPE.FINEBIN:
                    return m_DefectColor[InDefect.finebinnumber];
                case MAP_TYPE.ROUGHBIN:
                    return m_DefectColor[InDefect.roughbinnumber];
                case MAP_TYPE.SIZE:
                    if (m_SizeColor == null) return Color.Red;
                    for (int i = 0; i < m_SizeColor.Length; i++)
                    {
                        if (InDefect.dsize * 1000000 >= m_SizeColor[i].from && InDefect.dsize * 1000000 <= m_SizeColor[i].to) return m_SizeColor[i].color;
                    }
                    return Color.Red;
                default:
                    return m_DefectColor[InDefect.classnumber];
            }
        }
        #endregion

        private void FPDCellLayout_Resize(object sender, EventArgs e)
        {

            if (DesignMode) return;
            if (this.Width * this.Height == 0)
            {
                return;
            }

            float dMinCanvers_x = 0f;
            float dMinCanvers_y = 0f;

            try
            {
                if (m_oDrawMode == MapMode.Free)
                {
                    RectangleD rectdBefore = m_rectdCellArea;
                    PointD pfBeforeCenter = new PointD(m_rectdCellArea.X + m_rectdCellArea.Width / 2, m_rectdCellArea.Y + m_rectdCellArea.Height / 2);

                    m_rectdCellArea.Width = this.Width / (m_DrawRatio * m_dScale);
                    m_rectdCellArea.Height = this.Height / (m_DrawRatio * m_dScale);

                    m_rectdCellArea.X = pfBeforeCenter.X - m_rectdCellArea.Width / 2.0d;
                    m_rectdCellArea.Y = pfBeforeCenter.Y - m_rectdCellArea.Height / 2.0d;
                }
                else if (m_oDrawMode == MapMode.Fit)
                {
                    if ((m_CellRecipe.CellCood.Width / this.Width) > (m_CellRecipe.CellCood.Height / this.Height))
                    {
                        dMinCanvers_x = this.Width * 0.9f;
                        m_DrawRatio = dMinCanvers_x / m_CellRecipe.CellCood.Width;
                        dMinCanvers_y = (float)(m_CellRecipe.CellCood.Height * m_DrawRatio);
                    }
                    else
                    {
                        dMinCanvers_y = this.Height * 0.9f;
                        m_DrawRatio = dMinCanvers_y / m_CellRecipe.CellCood.Height;
                        dMinCanvers_x = (float)(m_CellRecipe.CellCood.Width * m_DrawRatio);
                    }

                    m_rectdCellArea = new RectangleD(((dMinCanvers_x - this.Width) / 2.0d) / (m_DrawRatio * m_dScale)
                        , ((dMinCanvers_y - this.Height) / 2.0d) / (m_DrawRatio * m_dScale)
                        , this.Width / m_DrawRatio
                        , this.Height / m_DrawRatio);
                }

                if (m_bmpCellMap != null) m_bmpCellMap.Dispose();
                if (m_gdiTempMap != null) m_gdiTempMap.Dispose();

                m_gdiMain = this.CreateGraphics();
                m_bmpCellMap = new Bitmap(this.Width, this.Height);
                m_bmpTemp = new Bitmap(m_bmpCellMap);
                m_gdiTempMap = Graphics.FromImage(m_bmpCellMap);
                Redraw();
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
            }
        }

        public virtual void Redraw()
        {
            DrawCell();
            FPDCellLayout_Paint(this, null);
        }

        private void FPDCellLayout_Paint(object sender, PaintEventArgs e)
        {
            if (m_gdiTempMap == null) return;
            m_gdiMain.DrawImageUnscaled(m_bmpCellMap, 0, 0);
        }

        
        #region ■ Point좌표를 주어진 각도로 Rotation하는 함수
        protected void RotatePoint(ref double dx, ref double dy, double RAngle)
        {
            double tX = (-m_CellRecipe.CellCood.Width/2+dx) * Math.Cos(RAngle / 180 * Math.PI) + (-m_CellRecipe.CellCood.Height/2+dy) * Math.Sin(RAngle / 180 * Math.PI);
            double tY = -(-m_CellRecipe.CellCood.Width/2+dx) * Math.Sin(RAngle / 180 * Math.PI) + (-m_CellRecipe.CellCood.Height/2+dy) * Math.Cos(RAngle / 180 * Math.PI);

            dx = tX;
            dy = tY;

        }
        #endregion
        #region ■ Default Color
        protected virtual void SetDefaultBinColor()
        {
            int r, g, b;//4337915
            //16777215
            try
            {
                int[] iColor = new int[]	{0,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        protected virtual void SetDefaultColor()
        {
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
        #endregion

        
        #region ■ Zoom 관련
        public void ZoomIn()
		{
			if (m_oDrawMode == MapMode.Fit) return;
            if (m_DrawRatio > 3500.0f) return;
            m_DrawRatio = m_DrawRatio * 1.05f;
            RectangleD rectdBefore = m_rectdCellArea;
            PointD pfBeforeCenter = new PointD(m_rectdCellArea.X + m_rectdCellArea.Width / 2, m_rectdCellArea.Y + m_rectdCellArea.Height / 2);

            m_rectdCellArea.Width = this.Width / m_DrawRatio;
            m_rectdCellArea.Height = this.Height / m_DrawRatio;
            m_rectdCellArea.X = pfBeforeCenter.X - m_rectdCellArea.Width / 2;
            m_rectdCellArea.Y = pfBeforeCenter.Y - m_rectdCellArea.Height / 2;

            Redraw();

		}

		public void ZoomOut()
		{
			if (m_oDrawMode == MapMode.Fit) return;
            m_DrawRatio = m_DrawRatio * 0.95f;
            RectangleD rectdBefore = m_rectdCellArea;
            PointD pfBeforeCenter = new PointD(m_rectdCellArea.X + m_rectdCellArea.Width / 2, m_rectdCellArea.Y + m_rectdCellArea.Height / 2);

            m_rectdCellArea.Width = this.Width / m_DrawRatio;
            m_rectdCellArea.Height = this.Height / m_DrawRatio;
            m_rectdCellArea.X = pfBeforeCenter.X - m_rectdCellArea.Width / 2;
            m_rectdCellArea.Y = pfBeforeCenter.Y - m_rectdCellArea.Height / 2;

			Redraw();
		}
		#endregion

        private void FPDCellLayout_MouseMove(object sender, MouseEventArgs e)
        {
            Bitmap bmpTemp = null;
            Graphics gdiOverWrite = null;
            try
            {

                bmpTemp = (Bitmap)m_bmpCellMap.Clone();
                gdiOverWrite = Graphics.FromImage(bmpTemp);

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
                else
                {

                    //for (int i = 0; i < m_arrDrawCell.Count ; i++)
                    //{
                    //    if (m_arrDrawCell[i].Contains((float)e.X, (float)e.Y))
                    //    {
                    //        Rectangle rectDrawSelCell = Rectangle.Truncate(m_arrDrawCell[i]);
                    //        rectDrawSelCell.Offset(1, 1);
                    //        gdiOverWrite.DrawRectangle(new Pen(Color.OrangeRed, 3), rectDrawSelCell);
                    //        break;
                    //    }
                    //}
                    //dMpoint = GetRealPoint(e.X,e.Y);
                    //gdiOverWrite.DrawString(string.Format("X={0},Y={1},Xa={2},Ya={3},ex={4},ey={5}", dMpoint.X, dMpoint.Y, m_rectdCellArea.X, m_rectdCellArea.Y,e.X,e.Y), m_fntGID, new SolidBrush(Color.Black), 10, 10);
                    //m_gdiMain.DrawImageUnscaled(bmpTemp, 0, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (bmpTemp != null) bmpTemp.Dispose();
                if (gdiOverWrite != null) gdiOverWrite.Dispose();

                bmpTemp = null;
                gdiOverWrite = null;
            }
        }

        private void ZoomEffect(int startX,int startY)
        {
            Bitmap bmpTemp = (Bitmap)m_bmpCellMap.Clone();
            Graphics gdiOverWrite = Graphics.FromImage(bmpTemp);
            Rectangle rectDrawSelCell = new Rectangle(startX-25, startY-25, 50, 50);
            Pen oPen = new Pen(Color.Gray, 2);
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    bmpTemp = (Bitmap)m_bmpCellMap.Clone();
                    gdiOverWrite = Graphics.FromImage(bmpTemp);
                    rectDrawSelCell.X = (startX - 25) - (i * 25);
                    rectDrawSelCell.Y = startY - 25 - (i * 25);
                    rectDrawSelCell.Width = 50 + (i * 50);
                    rectDrawSelCell.Height = 50 + (i * 50);
                    gdiOverWrite.DrawRectangle(oPen, rectDrawSelCell);
                    m_gdiMain.DrawImageUnscaled(bmpTemp, 0, 0);

                    bmpTemp.Dispose();
                    gdiOverWrite.Dispose();

                    System.Threading.Thread.Sleep(0);
                }
            }
            catch
            {
            }
            finally
            {
                if (bmpTemp != null) bmpTemp.Dispose();
                if (gdiOverWrite != null) gdiOverWrite.Dispose();
                if (oPen != null) oPen.Dispose();
  
                bmpTemp = null;
                gdiOverWrite = null;
                oPen = null;
            }
        }

        private void FPDCellLayout_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //ZoomEffect(e.X ,e.Y );
            ArrayList arrDefect = new ArrayList();
            try
            {
                PointD pdSelectPoint = GetRealPoint(m_poStart.X, m_poStart.Y);
                foreach (DACrux.Base.DEFECT_TAG InDefect in m_arrDefect)
                {
                    if (Math.Pow((InDefect.x - pdSelectPoint.X), 2.0d) + Math.Pow((InDefect.y - pdSelectPoint.Y), 2.0d) < Math.Pow((7.0f / m_DrawRatio), 2))
                    {
                        DefectHilight(InDefect);
                        arrDefect.Add(InDefect);
                    }
                }

                if (arrDefect.Count > 0)
                {
                    DACrux.Base.DEFECT_TAG[] oDefect = new DACrux.Base.DEFECT_TAG[arrDefect.Count];

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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_SelectPath = new GraphicsPath();
            m_poStart.X = e.X;
            m_poStart.Y = e.Y;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                switch (m_eoMouseDragMode)
                {
                    case MouseDragMode.Zoom:
                        if (m_oDrawMode == MapMode.Fit) break;
                        if (m_rectSelect.Width * m_rectSelect.Height == 0) return;

                        RectangleD rectdBefore = m_rectdCellArea;
                        double fBeforeZoomRatio = m_DrawRatio;

                        float fRatioWidth = (float)this.Width / m_rectSelect.Width;
                        float fRatioHeight = (float)this.Height / m_rectSelect.Height;

                        PointD pdCenter = new PointD((float)((m_rectSelect.X + m_rectSelect.Width / 2) / fBeforeZoomRatio + m_rectdCellArea.X)
                            , (float)((m_rectSelect.Y + m_rectSelect.Height / 2) / fBeforeZoomRatio + m_rectdCellArea.Y));

                        m_DrawRatio = Math.Min(fRatioHeight, fRatioWidth) * fBeforeZoomRatio;

                        m_rectdCellArea.Width = this.Width / m_DrawRatio;
                        m_rectdCellArea.Height = this.Height / m_DrawRatio;

                        m_rectdCellArea.X = pdCenter.X - m_rectdCellArea.Width / 2;
                        m_rectdCellArea.Y = pdCenter.Y - m_rectdCellArea.Height / 2;
                        //CalSelectedCell();
                        break;
                    case MouseDragMode.Move:
                        if (m_oDrawMode == MapMode.Fit) break;
                        m_rectdCellArea.X = m_rectdCellArea.X - (m_poEnd.X - m_poStart.X) / m_DrawRatio;
                        m_rectdCellArea.Y = m_rectdCellArea.Y - (m_poEnd.Y - m_poStart.Y) / m_DrawRatio;
                        break;
                    case MouseDragMode.Zone:
                        CalSelectedDefect();
                        //CalSelectedCell();
                        break;
                    case MouseDragMode.Rotate:
                        DrawRotate();
                        break;
                }

                if (m_SelectPath != null) m_SelectPath.Dispose();

                this.Redraw();
            }
        }

        //private void CalSelectedCell()
        //{
        //    if (m_SelectPath == null) return;
            
        //    PointF pdCellCenter = PointF.Empty;
        //    if (m_eoMapSelectStyle == MapSelectStyle.FreeHand) m_SelectPath.CloseFigure();
        //    try
        //    {
        //        foreach (Cell m_CellRecipe in m_arrCells)
        //        {
        //            pdCellCenter.X = (float)((m_CellRecipe.CellCood.X - m_GLASS_ORIGIN_X) + m_CellRecipe.CellCood.Width / 2);
        //            pdCellCenter.Y = (float)((m_CellRecipe.CellCood.Y - m_GLASS_ORIGIN_Y) + m_CellRecipe.CellCood.Height / 2);

        //            PointF pfPoint = PointF.Empty;
        //            pfPoint.X = (float)((-m_rectdCellArea.X + m_GlassRecipe.CELL_SIZE_X / 2 + pdCellCenter.X) * m_DrawRatio);
        //            pfPoint.Y = (float)((-m_rectdCellArea.Y + m_GlassRecipe.CELL_SIZE_Y / 2 - pdCellCenter.Y) * m_DrawRatio);

        //            if (m_SelectPath.IsVisible(pfPoint))
        //            {
        //                if (m_SelectedCells.IndexOf(new Point(m_CellRecipe.IndexX, m_CellRecipe.IndexY)) < 0)
        //                    m_SelectedCells.Add(new Point(m_CellRecipe.IndexX, m_CellRecipe.IndexY));
        //            }
        //        }
        //        if (OnSelectCells != null) OnSelectCells(this, (object)m_SelectedCells.ToArray(typeof(Point)));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void CalSelectedDefect()
        {

            if (m_SelectPath == null) return;
            PointD pdPoint;
            PointF pfPoint = PointF.Empty;
            if (m_eoMapSelectStyle == MapSelectStyle.FreeHand) m_SelectPath.CloseFigure();
            try
            {
                m_SelectedDefect.Clear();
                foreach (DACrux.Base.DEFECT_TAG InDefect in m_arrDefect)
                {
                    if (m_arrSelDefectNumber.Count != 0)
                        if (m_arrSelDefectNumber.IndexOf(InDefect.classnumber) < 0) continue;

                    pdPoint.X = InDefect.x;
                    pdPoint.Y = InDefect.y;

                    RotatePoint(ref pdPoint.X, ref pdPoint.Y, m_iViewAngle);

                    pfPoint.X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint.X) * (m_DrawRatio * m_dScale));
                    pfPoint.Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint.Y) * (m_DrawRatio * m_dScale));

                    if (m_SelectPath.IsVisible(pfPoint))
                    {
                        DefectHilight(InDefect);
                        m_SelectedDefect.Add(InDefect);
                    }
                }

                if (m_SelectedDefect.Count > 0)
                {
                    DACrux.Base.DEFECT_TAG[] oDefect = new DACrux.Base.DEFECT_TAG[m_SelectedDefect.Count];

                    m_SelectedDefect.CopyTo(oDefect);

                    if (OnSelectedDefect != null) OnSelectedDefect(this, oDefect);
                }
            }
            catch
            {
                // Error 무시
            }
        }

        private void DefectHilight(DACrux.Base.DEFECT_TAG InDefect)
        {
            if (m_arrDefect == null || m_arrDefect.Count <= 0) return;
            PointD[] pdPoint = new PointD[4];
            PointF[] pfPoint = new PointF[4];
            SizeF sfDSize = SizeF.Empty;
            try
            {
                if (m_bRealSize)
                {
                    pdPoint[0].X = InDefect.x;
                    pdPoint[0].Y = InDefect.y;

                    pdPoint[1].X = InDefect.x + InDefect.xsize;
                    pdPoint[1].Y = InDefect.y;

                    pdPoint[2].X = InDefect.x + InDefect.xsize;
                    pdPoint[2].Y = InDefect.y + InDefect.ysize;

                    pdPoint[3].X = InDefect.x;
                    pdPoint[3].Y = InDefect.y + InDefect.ysize;
                }
                else
                {
                    pdPoint[0].X = InDefect.x;
                    pdPoint[0].Y = InDefect.y;

                    pdPoint[1].X = InDefect.x + InDefect.xsize * 50;
                    pdPoint[1].Y = InDefect.y;

                    pdPoint[2].X = InDefect.x + InDefect.xsize * 50;
                    pdPoint[2].Y = InDefect.y + InDefect.ysize * 50;

                    pdPoint[3].X = InDefect.x;
                    pdPoint[3].Y = InDefect.y + InDefect.ysize * 50;
                }


                RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                pfPoint[0].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[0].X) * m_DrawRatio);
                pfPoint[0].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[0].Y) * m_DrawRatio);

                pfPoint[1].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[1].X) * m_DrawRatio);
                pfPoint[1].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[1].Y) * m_DrawRatio);

                pfPoint[2].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[2].X) * m_DrawRatio);
                pfPoint[2].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[2].Y) * m_DrawRatio);

                pfPoint[3].X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + pdPoint[3].X) * m_DrawRatio);
                pfPoint[3].Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - pdPoint[3].Y) * m_DrawRatio);


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (m_bRealSize) m_gdiTempMap.FillPolygon(new SolidBrush(m_DefectColor[InDefect.classnumber]), pfPoint);
                m_gdiTempMap.FillRectangle(new SolidBrush(m_DefectColor[InDefect.classnumber]), new RectangleF(pfPoint[0].X, pfPoint[0].Y, 4.0f, 4.0f));
                m_gdiTempMap.DrawArc(new Pen(Color.Blue), pfPoint[0].X - 2, pfPoint[0].Y - 2, 8, 8, 0, 360);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DrawSelectRect()
        {
            if (m_bmpCellMap == null) return;

            Graphics gdiTemp = null;
            Pen penPath = null;
            
            try
            {
                gdiTemp = Graphics.FromImage(m_bmpTemp);
                gdiTemp.DrawImageUnscaled(m_bmpCellMap, 0, 0);

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
                gdiTemp.DrawImageUnscaled(m_bmpCellMap, 0, 0);

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

        private void FPDCellLayout_MouseDown(object sender, MouseEventArgs e)
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
                if (m_bPopupMenu) ctxmFPDMap.Show(this, new Point(e.X, e.Y));
            }
        }

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
                gdiTemp.DrawImageUnscaled(m_bmpCellMap, 0, 0);
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

                        m_rectSelect.X = (int)((-m_rectdCellArea.X - dStartValue / 2 + m_CellRecipe.CellCood.Width / 2) * (m_DrawRatio * m_dScale));
                        m_rectSelect.Y = (int)((-m_rectdCellArea.Y - dStartValue / 2 + m_CellRecipe.CellCood.Height / 2) * (m_DrawRatio * m_dScale));
                        m_rectSelect.Width = (int)(dStartValue * (m_DrawRatio * m_dScale));
                        m_rectSelect.Height = (int)(dStartValue * (m_DrawRatio * m_dScale));

                        pdEnd = GetRealPoint(m_poEnd.X, m_poEnd.Y);
                        dEndValue = Math.Sqrt(Math.Pow(pdEnd.X, 2) + Math.Pow(pdEnd.Y, 2)) * 2;

                        m_rectSelect2.X = (int)((-m_rectdCellArea.X - dEndValue / 2 + m_CellRecipe.CellCood.Width / 2) * (m_DrawRatio * m_dScale));
                        m_rectSelect2.Y = (int)((-m_rectdCellArea.Y - dEndValue / 2 + m_CellRecipe.CellCood.Height / 2) * (m_DrawRatio * m_dScale));
                        m_rectSelect2.Width = (int)(dEndValue * (m_DrawRatio * m_dScale));
                        m_rectSelect2.Height = (int)(dEndValue * (m_DrawRatio * m_dScale));

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


                        m_rectSelect.X = (int)(-m_rectdCellArea.X * (m_DrawRatio * m_dScale));
                        m_rectSelect.Y = (int)(-m_rectdCellArea.Y * (m_DrawRatio * m_dScale));
                        m_rectSelect.Width = (int)(m_CellRecipe.CellCood.Width * (m_DrawRatio * m_dScale));
                        m_rectSelect.Height = (int)(m_CellRecipe.CellCood.Height * (m_DrawRatio * m_dScale));
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

            pdCenter.X = m_rectdCellArea.X + piX / (m_DrawRatio * m_dScale);
            pdCenter.Y = m_CellRecipe.CellCood.Height - (piY / (m_DrawRatio * m_dScale) + m_rectdCellArea.Y );
            
            Debug.Write(string.Format("X:{0},Y:{1},piX:{2},piY:{3},CenterX:{4},CenterY:{5}\n\r"
                , pdCenter.X, pdCenter.Y, piX, piY, pfGlassCenterToPixel.X, pfGlassCenterToPixel.Y));
            
            return pdCenter;
        }

        private PointF GetViewPoint(double dX, double dY)
        {
            PointF pfCenter = PointF.Empty;
            pfCenter.X = (float)((-m_rectdCellArea.X + m_CellRecipe.CellCood.Width / 2 + dX) * (m_DrawRatio * m_dScale));
            pfCenter.Y = (float)((-m_rectdCellArea.Y + m_CellRecipe.CellCood.Height / 2 - dY) * (m_DrawRatio * m_dScale));
            return pfCenter;
        }

        #endregion
    }
}