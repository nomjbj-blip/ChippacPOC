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
    public delegate void SelectCells(object sender, object SelDies);
    public delegate void SelectedDefect(object sender, DACrux.Base.DEFECT_TAG[] oDefect);
    public delegate void CreateDefect(object sender,DACrux.Base.PointD oDpoint);

    public partial class FPDLayout : UserControl
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
        protected Bitmap m_bmpGlassMap;
        protected Bitmap m_bmpTemp;
        protected Bitmap m_bmpGlassID;
        protected Bitmap m_bmpCellID;
		protected MapMode m_oDrawMode = MapMode.Fit;
        protected MouseDragMode m_eoMouseDragMode = MouseDragMode.Zoom;
        protected Rectangle m_rectSelect = Rectangle.Empty;
        protected Rectangle m_rectSelect2 = Rectangle.Empty;
        protected RectangleD m_rectdGlassArea;
        protected Point m_poStart = Point.Empty;
        protected Point m_poEnd = Point.Empty;
        protected MapSelectStyle m_eoMapSelectStyle = MapSelectStyle.Circle;
        protected GraphicsPath m_SelectPath = null;

        protected ArrayList m_arrCells = null;
        protected ArrayList m_CellIndexer = null;
        protected ArrayList[] m_arrArrays = null;
        protected ArrayList[] m_ArrayIndexer = null;

        protected GlassRecipe m_GlassRecipe;
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
        private PointD[] m_pdGlassOutLine = new PointD[] { new PointD(0, 10), new PointD(10, 0), new PointD(150, 0), new PointD(150, 100), new PointD(0, 100) };
        protected ArrayList m_arrDrawCell = null;
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

        protected bool m_bVisibleDensity = false;
        protected bool m_bVisibleDefectInRange = false;

        protected long m_lMaxDefectCount = 1;
        public event SelectedDefect OnSelectedDefect;
        public event CreateDefect OnCreateDefect;

        protected double m_dMaxSize = 1000;
        protected double m_dMinSize = 0;

        protected bool m_bEdit = false;
        protected bool m_bImageMark = true;
        protected bool m_bVisibleShape = false;

        protected string[] m_strArrShape = null;

        public FPDLayout()
        {
            InitializeComponent();
            ResetSelDefectNumber();
            SetDefaultColor();
            SetDefaultBinColor();
			m_hbSelectBrush = new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.White, Color.PowderBlue);
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

        public bool VisibleDensity
        {
            set
            {
                m_bVisibleDensity = value;
                Redraw();
            }
            get
            {
                return m_bVisibleDensity;
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

        public void SetSelectedCell(int x, int y)
        {
            m_SelectedCells.Add(new Point(x, y));
        }

        public void AddSelectedCell(int x, int y)
        {
            m_SelectedCells.Add(new Point(x, y));
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

        public void ResetShapeList(int Shapes)
        {
            try
            {
                m_strArrShape = new string[Shapes];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetShape(int DefectType, string Shape)
        {
            try
            {
                if (DefectType >= m_strArrShape.Length) return;
                m_strArrShape[DefectType] = Shape;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetShape(int DefectType)
        {
            try
            {
                if (DefectType >= m_strArrShape.Length) return m_strArrShape[0];
                return m_strArrShape[DefectType];
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
        
        public object SelectedCells
        {
            get { return (object)m_SelectedCells.ToArray(typeof(Point)); }
        }

        public MapMode MapControlMode
        {
            set
            {
                m_oDrawMode = value;
            }
            get
            {
                return m_oDrawMode;
            }
        }

        public bool EditMap
        {
            set
            {
                m_bEdit = value;
            }
            get
            {
                return m_bEdit;
            }
        }

        #region ◈ Popup Menu Click Event 처리
        private void mnuitemMAPMODE_FREEZOOM_Click(object sender, System.EventArgs e)
        {
            m_oDrawMode = MapMode.Free;
       
            mnuitemMAPMODE_FREEZOOM.Checked = true;
            mnuitemMAPMODE_FITSIZE.Checked = false;
            FPDLayout_Resize(null, null);

            MenuManagment();
            //mnuitemMOUSEDRAGMODE_ZOOM_Click(null,null);
        }

        private void mnuitemMAPMODE_FITSIZE_Click(object sender, System.EventArgs e)
        {
            m_oDrawMode = MapMode.Fit;
            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = true;
            FPDLayout_Resize(null, null);

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
            Clipboard.SetDataObject(m_bmpGlassMap);
        }
		public Bitmap GetImage()
		{
			return m_bmpGlassMap;
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

        public void AddInfo(string Info)
        {
            try
            {
                if (m_arrInfomation == null)
                {
                    ResetInfo();
                }
                m_arrInfomation.Add(Info);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ResetInfo()
        {
            try
            {
               m_arrInfomation = new ArrayList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetRecipe(GlassRecipe oGlassRecipe)
        {
            try
            {
                m_GlassRecipe = oGlassRecipe;
                DrawReset();
                Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GlassRecipe GetRecipe()
        {
            try
            {
                return m_GlassRecipe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FPDLayout_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            m_GlassRecipe.GLASS_SIZE_X = 2228;//1926,2328
            m_GlassRecipe.GLASS_SIZE_Y = 2010;
            m_GlassRecipe.CELL_INDEX_MAX_X = 2;
            m_GlassRecipe.CELL_INDEX_MAX_Y = 3;

            m_GlassRecipe.CELL_SIZE_X = 1100; // 2000
            m_GlassRecipe.CELL_SIZE_Y = 660d; // 2300


            m_pdGlassOutLine = new PointD[] { new PointD(0, 48.0f)
                                                     , new PointD(36.0f, 0)
                                                     , new PointD(m_GlassRecipe.GLASS_SIZE_X, 0)
                                                     , new PointD(m_GlassRecipe.GLASS_SIZE_X, m_GlassRecipe.GLASS_SIZE_Y)
                                                     , new PointD(0, m_GlassRecipe.GLASS_SIZE_Y) };


            double dGapX = (m_GlassRecipe.GLASS_SIZE_X - (double)(m_GlassRecipe.CELL_INDEX_MAX_X * m_GlassRecipe.CELL_SIZE_X)) / m_GlassRecipe.CELL_INDEX_MAX_X;
            double dGapY = (m_GlassRecipe.GLASS_SIZE_Y - (double)(m_GlassRecipe.CELL_INDEX_MAX_Y * m_GlassRecipe.CELL_SIZE_Y)) / m_GlassRecipe.CELL_INDEX_MAX_Y;

            double dWidthHeightRatio = (this.Width * m_GlassRecipe.GLASS_SIZE_Y) / (this.Height * m_GlassRecipe.GLASS_SIZE_X);
            double DrawSizeX = this.Width;
            double DrawSizeY = this.Height;

            m_arrCells = new ArrayList();
            m_CellIndexer = new ArrayList();
            m_SelectedCells = new ArrayList();
            m_SelectedDefect = new ArrayList();
            for (int y = 0; y < m_GlassRecipe.CELL_INDEX_MAX_Y; y++)
            {
                for (int x = 0; x < m_GlassRecipe.CELL_INDEX_MAX_X; x++)
                {
                    AddCell(x, y, 0);
                }
            }

            if (dWidthHeightRatio > 1)
            {
                DrawSizeX = (DrawSizeY * m_GlassRecipe.GLASS_SIZE_X) / m_GlassRecipe.GLASS_SIZE_Y;
                m_DrawRatio = (DrawSizeX / m_GlassRecipe.GLASS_SIZE_X) * 0.9d;
            }
            else
            {
                DrawSizeY = (DrawSizeX * m_GlassRecipe.GLASS_SIZE_Y) / m_GlassRecipe.GLASS_SIZE_X;
                m_DrawRatio = (DrawSizeY / m_GlassRecipe.GLASS_SIZE_Y) * 0.9d;
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
                if (m_bmpGlassMap != null) m_bmpGlassMap.Dispose();
                if (m_gdiTempMap != null) m_gdiTempMap.Dispose();

                m_gdiMain = this.CreateGraphics();
                m_bmpGlassMap = new Bitmap(this.Width, this.Height);
                m_bmpTemp = new Bitmap(m_bmpGlassMap);
                m_gdiTempMap = Graphics.FromImage(m_bmpGlassMap);
                

                float dMinCanvers_x = 0f;
                float dMinCanvers_y = 0f;

                if ((m_GlassRecipe.GLASS_SIZE_X / this.Width) > (m_GlassRecipe.GLASS_SIZE_Y / this.Height))
                {
                    dMinCanvers_x = this.Width * 0.9f;
                    m_DrawRatio = dMinCanvers_x / m_GlassRecipe.GLASS_SIZE_X;
                    dMinCanvers_y = (float)(m_GlassRecipe.GLASS_SIZE_Y * m_DrawRatio);
                }
                else
                {
                    dMinCanvers_y = this.Height * 0.9f;
                    m_DrawRatio = dMinCanvers_y / m_GlassRecipe.GLASS_SIZE_Y;
                    dMinCanvers_x = (float)(m_GlassRecipe.GLASS_SIZE_X * m_DrawRatio);
                }

                m_rectdGlassArea = new RectangleD(((dMinCanvers_x - this.Width) / 2.0d) / (m_DrawRatio * m_dScale)
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
 
        public void AddCell(int X, int Y,double oX , double oY ,int BIN)
        {
            Cell NewCell = new Cell(X, Y, BIN, 0);
            double dGapX = (m_GlassRecipe.GLASS_SIZE_X - (m_GlassRecipe.CELL_INDEX_MAX_X * m_GlassRecipe.CELL_SIZE_X)) / m_GlassRecipe.CELL_INDEX_MAX_X;
            double dGapY = (m_GlassRecipe.GLASS_SIZE_Y - (m_GlassRecipe.CELL_INDEX_MAX_Y * m_GlassRecipe.CELL_SIZE_Y)) / m_GlassRecipe.CELL_INDEX_MAX_Y;

            //NewCell.CellCood.X = ((NewCell.IndexX * dGapX) + dGapX / 2.0d) + (NewCell.IndexX * m_GlassRecipe.CELL_SIZE_X);
            //NewCell.CellCood.Y = ((NewCell.IndexY * dGapY) + dGapY / 2.0d) + (NewCell.IndexY * m_GlassRecipe.CELL_SIZE_Y);
            NewCell.CellCood.X = oX;
            NewCell.CellCood.Y = oY;
            NewCell.CellCood.Width = m_GlassRecipe.CELL_SIZE_X;
            NewCell.CellCood.Height = m_GlassRecipe.CELL_SIZE_Y;

            m_arrCells.Add(NewCell);
            m_CellIndexer.Add(new Point(X, Y));
        }

        public void AddCell(int X, int Y, int BIN)
        {
            Cell NewCell = new Cell(X, Y, BIN, 0);
            double dGapX = (m_GlassRecipe.GLASS_SIZE_X - (m_GlassRecipe.CELL_INDEX_MAX_X * m_GlassRecipe.CELL_SIZE_X)) / m_GlassRecipe.CELL_INDEX_MAX_X;
            double dGapY = (m_GlassRecipe.GLASS_SIZE_Y - (m_GlassRecipe.CELL_INDEX_MAX_Y * m_GlassRecipe.CELL_SIZE_Y)) / m_GlassRecipe.CELL_INDEX_MAX_Y;

            NewCell.CellCood.X = ((NewCell.IndexX * dGapX) + dGapX / 2.0d) + (NewCell.IndexX * m_GlassRecipe.CELL_SIZE_X);
            NewCell.CellCood.Y = ((NewCell.IndexY * dGapY) + dGapY / 2.0d) + (NewCell.IndexY * m_GlassRecipe.CELL_SIZE_Y);
            NewCell.CellCood.Width = m_GlassRecipe.CELL_SIZE_X;
            NewCell.CellCood.Height = m_GlassRecipe.CELL_SIZE_Y;

            m_arrCells.Add(NewCell);
            m_CellIndexer.Add(new Point(X, Y));
        }

        public void ResetCells()
        {
            try
            {
                if (m_arrCells != null) m_arrCells = null;
                m_arrCells = new ArrayList();

                if (m_CellIndexer != null) m_CellIndexer = null;
                m_CellIndexer = new ArrayList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddArray(int X, int Y, int CellIdx, int BIN)
        {
            Cell thisCell = (Cell)m_arrCells[CellIdx-1];
            CellArray NewArray = new CellArray(X, Y, BIN);
            NewArray.ParametricValue = BIN;
            double dGapX = (m_GlassRecipe.GLASS_SIZE_X - (m_GlassRecipe.CELL_INDEX_MAX_X * m_GlassRecipe.CELL_SIZE_X)) / m_GlassRecipe.CELL_INDEX_MAX_X;
            double dGapY = (m_GlassRecipe.GLASS_SIZE_Y - (m_GlassRecipe.CELL_INDEX_MAX_Y * m_GlassRecipe.CELL_SIZE_Y)) / m_GlassRecipe.CELL_INDEX_MAX_Y;

            //NewArray.ArrayCood.X = ((thisCell.CellCood.X * dGapX) + dGapX / 2.0d) + (thisCell.CellCood.X * m_GlassRecipe.CELL_SIZE_X) + (NewArray.IndexX * m_GlassRecipe.ARRAY_SIZE_X);
            //NewArray.ArrayCood.Y = ((thisCell.CellCood.Y * dGapY) + dGapY / 2.0d) + (thisCell.CellCood.Y * m_GlassRecipe.CELL_SIZE_Y) + (NewArray.IndexY * m_GlassRecipe.ARRAY_SIZE_Y);

            NewArray.ArrayCood.X = ((thisCell.CellCood.X * dGapX) + dGapX / 2.0d) + thisCell.CellCood.X + ((NewArray.IndexX-1) * m_GlassRecipe.ARRAY_SIZE_X);
            NewArray.ArrayCood.Y = ((thisCell.CellCood.Y * dGapY) + dGapY / 2.0d) + thisCell.CellCood.Y + ((NewArray.IndexY-1) * m_GlassRecipe.ARRAY_SIZE_Y);
            
            NewArray.ArrayCood.Width = m_GlassRecipe.ARRAY_SIZE_X;
            NewArray.ArrayCood.Height = m_GlassRecipe.ARRAY_SIZE_Y;

            m_arrArrays[CellIdx-1].Add(NewArray);
            m_ArrayIndexer[CellIdx-1].Add(new Point(X, Y));
            if (BIN > m_lMaxDefectCount) m_lMaxDefectCount = BIN;
        }

        public void ResetArray()
        {
            try
            {
                if (m_arrArrays != null) m_arrArrays = null;
                if (m_ArrayIndexer != null) m_ArrayIndexer = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ResetArray(int Cells)
        {
            try
            {
                if (m_arrArrays != null) m_arrArrays = null;
                m_arrArrays = new ArrayList[Cells];

                if (m_ArrayIndexer != null) m_ArrayIndexer = null;
                m_ArrayIndexer = new ArrayList[Cells];

                for(int i = 0;i<Cells;i++)
                {
                    m_arrArrays[i] = new ArrayList();
                    m_ArrayIndexer[i] = new ArrayList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DrawGlass()
        {
            if (this.Width * this.Height == 0 || m_gdiTempMap == null) return;

            GraphicsPath gpGlass = null;
            GraphicsPath gpShadow = null;
            SolidBrush sbrshGlass = null;

            Pen pGlass = null;
            Pen pShadow = null;

            try
            {
                m_gdiTempMap.Clear(Color.SlateGray);
                gpGlass = new GraphicsPath();

                ///=======Glass Edge Drawing ========================================================
                gpGlass.StartFigure();
                
                //PointD[] pdGlassOutLine = new PointD[m_pdGlassOutLine.Length];
                //PointF[] pfGlassOutLine = new PointF[m_pdGlassOutLine.Length];
                //for (int i = 0; i < m_pdGlassOutLine.Length; i++)
                //{
                //    pdGlassOutLine[i] = new PointD(m_pdGlassOutLine[i].X ,m_pdGlassOutLine[i].Y);
                //    RotatePoint(ref pdGlassOutLine[i].X, ref pdGlassOutLine[i].Y, m_iViewAngle);

                //    pfGlassOutLine[i].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdGlassOutLine[i].X) * (m_DrawRatio * m_dScale));
                //    pfGlassOutLine[i].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 + pdGlassOutLine[i].Y) * (m_DrawRatio * m_dScale));
                //}

                //gpGlass.AddPolygon(pfGlassOutLine);
                
                sbrshGlass = new SolidBrush(Color.DimGray);
                pGlass = new Pen(Color.Black);

                ///=======Shadow Drawing ============================================================
                ///
                int iShadowDepth = Math.Min(5, (int)(m_DrawRatio * 100));

                gpShadow = (GraphicsPath)gpGlass.Clone();
                Matrix translateMatrix = new Matrix();
                Matrix translateMatrix2 = new Matrix();
                translateMatrix2.Translate(iShadowDepth + 2, iShadowDepth + 2);
                gpShadow.Transform(translateMatrix2);

                translateMatrix.Translate(-1, -1);
                for (int i = iShadowDepth; i >= 0 ; i--)
                {
                    gpShadow.Transform(translateMatrix);
                    pShadow = new Pen(Color.FromArgb(205 + 10 * i, 205 + 10 * i, 205 + 10 * i));
                    m_gdiTempMap.DrawPath(pShadow, gpShadow);
                }

                m_gdiTempMap.FillPath(sbrshGlass, gpGlass);
                m_gdiTempMap.DrawPath(pGlass, gpGlass);


                DrawCells(m_gdiTempMap);
                if (m_arrArrays != null && m_bVisibleDensity)
                {
                    for (int i = 0; i < m_arrArrays.Length; i++)
                    {
                        DrawArray(m_gdiTempMap, i);
                    }
                }
                DrawDefect(m_gdiTempMap);


                DrawInfo(m_gdiTempMap);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(gpGlass != null) gpGlass.Dispose();
                if (gpShadow != null) gpShadow.Dispose();
                if (sbrshGlass != null) sbrshGlass.Dispose();
            }
        }

        private void DrawInfo(Graphics g)
        {
            if (m_arrInfomation == null || m_arrInfomation.Count == 0) return;
            SolidBrush sbrshGlass = null;
            try
            {
                sbrshGlass = new SolidBrush(Color.WhiteSmoke);
                for (int i = 0; i < m_arrInfomation.Count; i++)
                {
                    g.DrawString((string)m_arrInfomation[i], m_fntGID, sbrshGlass, 10, 25 + i*15);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual void DrawCells(Graphics g)
        {
            try
            {
                if (m_arrCells == null || m_arrCells.Count <= 0) return;
                m_GlassRecipe.NETCELL = 0;

                PointD[] pdPoint = new PointD[4];
                PointF[] pfPoint = new PointF[4];

                Pen pSelCellBorder = null;
                Pen pCellBorder = null;

                SolidBrush sbrshCell = null;

                try
                {
                    pSelCellBorder = new Pen(Color.Red, 3);
                    pCellBorder = new Pen(m_colCellBorder,3);
                    sbrshCell = new SolidBrush(Color.White);

                    foreach (Cell InCell in m_arrCells)
                    {
                        pdPoint[0].X = InCell.CellCood.X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[0].Y = InCell.CellCood.Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[1].X = InCell.CellCood.X + InCell.CellCood.Width - m_GlassRecipe.ORIGIN_X;
                        pdPoint[1].Y = InCell.CellCood.Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[2].X = InCell.CellCood.X + InCell.CellCood.Width - m_GlassRecipe.ORIGIN_X;
                        pdPoint[2].Y = InCell.CellCood.Y + InCell.CellCood.Height - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[3].X = InCell.CellCood.X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[3].Y = InCell.CellCood.Y + InCell.CellCood.Height - m_GlassRecipe.ORIGIN_Y;

                        RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                        pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[0].X) * (m_DrawRatio * m_dScale));
                        pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[0].Y) * (m_DrawRatio * m_dScale));

                        pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[1].X) * (m_DrawRatio * m_dScale));
                        pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[1].Y) * (m_DrawRatio * m_dScale));

                        pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[2].X) * (m_DrawRatio * m_dScale));
                        pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[2].Y) * (m_DrawRatio * m_dScale));

                        pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[3].X) * (m_DrawRatio * m_dScale));
                        pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[3].Y) * (m_DrawRatio * m_dScale));

                        m_GlassRecipe.NETCELL++;

                        try
                        {
                            if (m_ColorSet == null) return;
                            sbrshCell.Color = m_ColorSet[InCell.BinNumber];
                            pCellBorder.Color = m_colCellBorder;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        if (InCell.CellProp > -1) g.FillPolygon(sbrshCell, pfPoint);
                        if (m_DrawRatio > 0.05 && m_bVisibleCellBorder)
                        {
                            if (m_SelectedCells.IndexOf(new Point(InCell.IndexX, InCell.IndexY)) > -1)
                            {
                                g.DrawPolygon(pSelCellBorder, pfPoint);
                            }
                            else
                            {
                                g.DrawPolygon(pCellBorder, pfPoint);
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

                    if (pSelCellBorder != null) pSelCellBorder.Dispose();
                    if (pCellBorder != null) pCellBorder.Dispose();
                    if (sbrshCell != null) sbrshCell.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawArray(Graphics g,int CellIdx)
        {
            if (m_arrArrays == null) return;
            try
            {
                if (m_arrArrays[CellIdx] == null || m_arrArrays[CellIdx].Count <= 0) return;

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

                    foreach (CellArray InArray in m_arrArrays[CellIdx])
                    {
                        pdPoint[0].X = InArray.ArrayCood.X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[0].Y = InArray.ArrayCood.Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[1].X = InArray.ArrayCood.X + InArray.ArrayCood.Width - m_GlassRecipe.ORIGIN_X;
                        pdPoint[1].Y = InArray.ArrayCood.Y - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[2].X = InArray.ArrayCood.X + InArray.ArrayCood.Width - m_GlassRecipe.ORIGIN_X;
                        pdPoint[2].Y = InArray.ArrayCood.Y + InArray.ArrayCood.Height - m_GlassRecipe.ORIGIN_Y;

                        pdPoint[3].X = InArray.ArrayCood.X - m_GlassRecipe.ORIGIN_X;
                        pdPoint[3].Y = InArray.ArrayCood.Y + InArray.ArrayCood.Height - m_GlassRecipe.ORIGIN_Y;

                        RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
                        RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

                        pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[0].X) * (m_DrawRatio * m_dScale));
                        pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[0].Y) * (m_DrawRatio * m_dScale));

                        pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[1].X) * (m_DrawRatio * m_dScale));
                        pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[1].Y) * (m_DrawRatio * m_dScale));

                        pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[2].X) * (m_DrawRatio * m_dScale));
                        pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[2].Y) * (m_DrawRatio * m_dScale));

                        pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[3].X) * (m_DrawRatio * m_dScale));
                        pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[3].Y) * (m_DrawRatio * m_dScale));



                        m_GlassRecipe.NETARRAY++;

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

                    if(m_arrSelDefectNumber.Count != 0)
                    if(m_arrSelDefectNumber.IndexOf(InDefect.classnumber)<0) continue;

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


                    pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[0].X) * (m_DrawRatio * m_dScale));
                    pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[0].Y) * (m_DrawRatio * m_dScale));

                    pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[1].X) * (m_DrawRatio * m_dScale));
                    pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[1].Y) * (m_DrawRatio * m_dScale));

                    pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[2].X) * (m_DrawRatio * m_dScale));
                    pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[2].Y) * (m_DrawRatio * m_dScale));

                    pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[3].X) * (m_DrawRatio * m_dScale));
                    pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[3].Y) * (m_DrawRatio * m_dScale));

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (m_bVisibleShape)
                    {
                        g.DrawString(m_strArrShape[InDefect.classnumber],m_fntGID,new SolidBrush(DefectColor(InDefect)), new RectangleF(pfPoint[0].X, pfPoint[0].Y, 4.0f, 4.0f));
                    }
                    else
                    {
                        if (m_bRealSize)
                        {
                            g.FillPolygon(new SolidBrush(DefectColor(InDefect)), pfPoint);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(DefectColor(InDefect)), new RectangleF(pfPoint[0].X, pfPoint[0].Y, 4.0f, 4.0f));
                        }
                        if (InDefect.imagecount > 0 && m_bImageMark)
                        {
                            g.DrawArc(new Pen(Color.Red), pfPoint[0].X - 2, pfPoint[0].Y - 2, 8, 8, 0, 360);
                        }
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

        private void FPDLayout_Resize(object sender, EventArgs e)
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
                    RectangleD rectdBefore = m_rectdGlassArea;
                    PointD pfBeforeCenter = new PointD(m_rectdGlassArea.X + m_rectdGlassArea.Width / 2, m_rectdGlassArea.Y + m_rectdGlassArea.Height / 2);

                    m_rectdGlassArea.Width = this.Width / (m_DrawRatio * m_dScale);
                    m_rectdGlassArea.Height = this.Height / (m_DrawRatio * m_dScale);

                    m_rectdGlassArea.X = pfBeforeCenter.X - m_rectdGlassArea.Width / 2.0d;
                    m_rectdGlassArea.Y = pfBeforeCenter.Y - m_rectdGlassArea.Height / 2.0d;
                }
                //else if (m_oDrawMode == MapMode.Edit)
                //{
                //    if ((m_GlassRecipe.GLASS_SIZE_X / this.Width) > (m_GlassRecipe.GLASS_SIZE_Y / this.Height))
                //    {
                //        dMinCanvers_x = this.Width * 0.9f;
                //        m_DrawRatio = dMinCanvers_x / m_GlassRecipe.GLASS_SIZE_X;
                //        dMinCanvers_y = (float)(m_GlassRecipe.GLASS_SIZE_Y * m_DrawRatio);
                //    }
                //    else
                //    {
                //        dMinCanvers_y = this.Height * 0.9f;
                //        m_DrawRatio = dMinCanvers_y / m_GlassRecipe.GLASS_SIZE_Y;
                //        dMinCanvers_x = (float)(m_GlassRecipe.GLASS_SIZE_X * m_DrawRatio);
                //    }

                //    m_rectdGlassArea = new RectangleD(((this.Width - dMinCanvers_x) / 2.0d) / (m_DrawRatio * m_dScale)
                //        , ((this.Height - dMinCanvers_y) / 2.0d) / (m_DrawRatio * m_dScale)
                //        , this.Width / m_DrawRatio
                //        , this.Height / m_DrawRatio);
                //}
                else if (m_oDrawMode == MapMode.Fit)
                {
                    if ((m_GlassRecipe.GLASS_SIZE_X / this.Width) > (m_GlassRecipe.GLASS_SIZE_Y / this.Height))
                    {
                        dMinCanvers_x = this.Width * 0.9f;
                        m_DrawRatio = dMinCanvers_x / m_GlassRecipe.GLASS_SIZE_X;
                        dMinCanvers_y = (float)(m_GlassRecipe.GLASS_SIZE_Y * m_DrawRatio);
                    }
                    else
                    {
                        dMinCanvers_y = this.Height * 0.9f;
                        m_DrawRatio = dMinCanvers_y / m_GlassRecipe.GLASS_SIZE_Y;
                        dMinCanvers_x = (float)(m_GlassRecipe.GLASS_SIZE_X * m_DrawRatio);
                    }

                    m_rectdGlassArea = new RectangleD(((dMinCanvers_x - this.Width) / 2.0d) / (m_DrawRatio * m_dScale)
                        , ((dMinCanvers_y - this.Height) / 2.0d) / (m_DrawRatio * m_dScale)
                        , this.Width / m_DrawRatio
                        , this.Height / m_DrawRatio);
                }

                if (m_bmpGlassMap != null) m_bmpGlassMap.Dispose();
                if (m_gdiTempMap != null) m_gdiTempMap.Dispose();

                m_gdiMain = this.CreateGraphics();
                m_bmpGlassMap = new Bitmap(this.Width, this.Height);
                m_bmpTemp = new Bitmap(m_bmpGlassMap);
                m_gdiTempMap = Graphics.FromImage(m_bmpGlassMap);
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
            if (DesignMode) return;
            DrawGlass();
            FPDLayout_Paint(this, null);
        }

        private void FPDLayout_Paint(object sender, PaintEventArgs e)
        {
            if (m_gdiTempMap == null) return;
            m_gdiMain.DrawImageUnscaled(m_bmpGlassMap, 0, 0);
        }

        
        #region ■ Point좌표를 주어진 각도로 Rotation하는 함수
        protected void RotatePoint(ref double dx, ref double dy, double RAngle)
        {
            double tX = (-m_GlassRecipe.GLASS_SIZE_X/2+dx) * Math.Cos(RAngle / 180 * Math.PI) + (-m_GlassRecipe.GLASS_SIZE_Y/2+dy) * Math.Sin(RAngle / 180 * Math.PI);
            double tY = -(-m_GlassRecipe.GLASS_SIZE_X/2+dx) * Math.Sin(RAngle / 180 * Math.PI) + (-m_GlassRecipe.GLASS_SIZE_Y/2+dy) * Math.Cos(RAngle / 180 * Math.PI);

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
            RectangleD rectdBefore = m_rectdGlassArea;
            PointD pfBeforeCenter = new PointD(m_rectdGlassArea.X + m_rectdGlassArea.Width / 2, m_rectdGlassArea.Y + m_rectdGlassArea.Height / 2);

            m_rectdGlassArea.Width = this.Width / m_DrawRatio;
            m_rectdGlassArea.Height = this.Height / m_DrawRatio;
            m_rectdGlassArea.X = pfBeforeCenter.X - m_rectdGlassArea.Width / 2;
            m_rectdGlassArea.Y = pfBeforeCenter.Y - m_rectdGlassArea.Height / 2;

            Redraw();

		}

		public void ZoomOut()
		{
			if (m_oDrawMode == MapMode.Fit) return;
            m_DrawRatio = m_DrawRatio * 0.95f;
            RectangleD rectdBefore = m_rectdGlassArea;
            PointD pfBeforeCenter = new PointD(m_rectdGlassArea.X + m_rectdGlassArea.Width / 2, m_rectdGlassArea.Y + m_rectdGlassArea.Height / 2);

            m_rectdGlassArea.Width = this.Width / m_DrawRatio;
            m_rectdGlassArea.Height = this.Height / m_DrawRatio;
            m_rectdGlassArea.X = pfBeforeCenter.X - m_rectdGlassArea.Width / 2;
            m_rectdGlassArea.Y = pfBeforeCenter.Y - m_rectdGlassArea.Height / 2;

			Redraw();
		}
		#endregion

        private void FPDLayout_MouseMove(object sender, MouseEventArgs e)
        {
            Bitmap bmpTemp = null;
            Graphics gdiOverWrite = null;
            try
            {

                bmpTemp = (Bitmap)m_bmpGlassMap.Clone();
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
                    PointD dMpoint = GetRealPoint(e.X, e.Y);
                    
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
                    gdiOverWrite.DrawString(string.Format("X={0:F2},Y={1:F2}", dMpoint.X, dMpoint.Y), m_fntGID, new SolidBrush(Color.White), 10, 10);
                    m_gdiMain.DrawImageUnscaled(bmpTemp, 0, 0);
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
            Bitmap bmpTemp = (Bitmap)m_bmpGlassMap.Clone();
            Graphics gdiOverWrite = Graphics.FromImage(bmpTemp);
            Rectangle rectDrawSelCell = new Rectangle(startX-25, startY-25, 50, 50);
            Pen oPen = new Pen(Color.Gray, 2);
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    bmpTemp = (Bitmap)m_bmpGlassMap.Clone();
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

        private void FPDLayout_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ArrayList arrDefect = new ArrayList();
            try
            {
                PointD pdSelectPoint = GetRealPoint(m_poStart.X, m_poStart.Y);

                if (m_bEdit)
                {
                    PointD oDPoint = new PointD(pdSelectPoint.X ,pdSelectPoint.Y);
                    if (OnCreateDefect != null) OnCreateDefect(this, oDPoint);                    
                }else{
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

            if (e.Button == MouseButtons.Left && !m_bEdit)
            {
                switch (m_eoMouseDragMode)
                {
                    case MouseDragMode.Zoom:
                        if (m_oDrawMode == MapMode.Fit) break;
                        if (m_rectSelect.Width * m_rectSelect.Height == 0) return;

                        RectangleD rectdBefore = m_rectdGlassArea;
                        double fBeforeZoomRatio = m_DrawRatio;

                        float fRatioWidth = (float)this.Width / m_rectSelect.Width;
                        float fRatioHeight = (float)this.Height / m_rectSelect.Height;

                        PointD pdCenter = new PointD((float)((m_rectSelect.X + m_rectSelect.Width / 2) / fBeforeZoomRatio + m_rectdGlassArea.X)
                            , (float)((m_rectSelect.Y + m_rectSelect.Height / 2) / fBeforeZoomRatio + m_rectdGlassArea.Y));

                        m_DrawRatio = Math.Min(fRatioHeight, fRatioWidth) * fBeforeZoomRatio;

                        m_rectdGlassArea.Width = this.Width / m_DrawRatio;
                        m_rectdGlassArea.Height = this.Height / m_DrawRatio;

                        m_rectdGlassArea.X = pdCenter.X - m_rectdGlassArea.Width / 2;
                        m_rectdGlassArea.Y = pdCenter.Y - m_rectdGlassArea.Height / 2;
                        //CalSelectedCell();
                        break;
                    case MouseDragMode.Move:
                        if (m_oDrawMode == MapMode.Fit) break;
                        m_rectdGlassArea.X = m_rectdGlassArea.X - (m_poEnd.X - m_poStart.X) / m_DrawRatio;
                        m_rectdGlassArea.Y = m_rectdGlassArea.Y - (m_poEnd.Y - m_poStart.Y) / m_DrawRatio;
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
        //        foreach (Cell InCell in m_arrCells)
        //        {
        //            pdCellCenter.X = (float)((InCell.CellCood.X - m_GlassRecipe.ORIGIN_X) + InCell.CellCood.Width / 2);
        //            pdCellCenter.Y = (float)((InCell.CellCood.Y - m_GlassRecipe.ORIGIN_Y) + InCell.CellCood.Height / 2);

        //            PointF pfPoint = PointF.Empty;
        //            pfPoint.X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.CELL_SIZE_X / 2 + pdCellCenter.X) * m_DrawRatio);
        //            pfPoint.Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.CELL_SIZE_Y / 2 - pdCellCenter.Y) * m_DrawRatio);

        //            if (m_SelectPath.IsVisible(pfPoint))
        //            {
        //                if (m_SelectedCells.IndexOf(new Point(InCell.IndexX, InCell.IndexY)) < 0)
        //                    m_SelectedCells.Add(new Point(InCell.IndexX, InCell.IndexY));
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

                    pfPoint.X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint.X) * (m_DrawRatio * m_dScale));
                    pfPoint.Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint.Y) * (m_DrawRatio * m_dScale));

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

                pfPoint[0].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[0].X) * m_DrawRatio);
                pfPoint[0].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[0].Y) * m_DrawRatio);

                pfPoint[1].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[1].X) * m_DrawRatio);
                pfPoint[1].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[1].Y) * m_DrawRatio);

                pfPoint[2].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[2].X) * m_DrawRatio);
                pfPoint[2].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[2].Y) * m_DrawRatio);

                pfPoint[3].X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + pdPoint[3].X) * m_DrawRatio);
                pfPoint[3].Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - pdPoint[3].Y) * m_DrawRatio);


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
            catch
            {
            }
            finally
            {
                if (gdiTemp != null) gdiTemp.Dispose();
                if (penPath != null) penPath.Dispose();
            }
        }

        private void FPDLayout_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !m_bEdit)
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

                        m_rectSelect.X = (int)((-m_rectdGlassArea.X - dStartValue / 2 + m_GlassRecipe.GLASS_SIZE_X / 2) * (m_DrawRatio * m_dScale));
                        m_rectSelect.Y = (int)((-m_rectdGlassArea.Y - dStartValue / 2 + m_GlassRecipe.GLASS_SIZE_Y / 2) * (m_DrawRatio * m_dScale));
                        m_rectSelect.Width = (int)(dStartValue * (m_DrawRatio * m_dScale));
                        m_rectSelect.Height = (int)(dStartValue * (m_DrawRatio * m_dScale));

                        pdEnd = GetRealPoint(m_poEnd.X, m_poEnd.Y);
                        dEndValue = Math.Sqrt(Math.Pow(pdEnd.X, 2) + Math.Pow(pdEnd.Y, 2)) * 2;

                        m_rectSelect2.X = (int)((-m_rectdGlassArea.X - dEndValue / 2 + m_GlassRecipe.GLASS_SIZE_X / 2) * (m_DrawRatio * m_dScale));
                        m_rectSelect2.Y = (int)((-m_rectdGlassArea.Y - dEndValue / 2 + m_GlassRecipe.GLASS_SIZE_Y / 2) * (m_DrawRatio * m_dScale));
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


                        m_rectSelect.X = (int)(-m_rectdGlassArea.X * (m_DrawRatio * m_dScale));
                        m_rectSelect.Y = (int)(-m_rectdGlassArea.Y * (m_DrawRatio * m_dScale));
                        m_rectSelect.Width = (int)(m_GlassRecipe.GLASS_SIZE_X * (m_DrawRatio * m_dScale));
                        m_rectSelect.Height = (int)(m_GlassRecipe.GLASS_SIZE_Y * (m_DrawRatio * m_dScale));
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


            pdCenter.X = m_rectdGlassArea.X + piX / (m_DrawRatio * m_dScale);
            pdCenter.Y = m_GlassRecipe.GLASS_SIZE_Y - (piY / (m_DrawRatio * m_dScale) + m_rectdGlassArea.Y );

            Debug.Write(string.Format("X:{0},Y:{1},piX:{2},piY:{3},CenterX:{4},CenterY:{5}\n\r"
                , pdCenter.X, pdCenter.Y, piX, piY, pfGlassCenterToPixel.X, pfGlassCenterToPixel.Y));
            
            return pdCenter;
        }

        private PointF GetViewPoint(double dX, double dY)
        {
            PointF pfCenter = PointF.Empty;
            pfCenter.X = (float)((-m_rectdGlassArea.X + m_GlassRecipe.GLASS_SIZE_X / 2 + dX) * (m_DrawRatio * m_dScale));
            pfCenter.Y = (float)((-m_rectdGlassArea.Y + m_GlassRecipe.GLASS_SIZE_Y / 2 - dY) * (m_DrawRatio * m_dScale));
            return pfCenter;
        }

        #endregion
    }
}