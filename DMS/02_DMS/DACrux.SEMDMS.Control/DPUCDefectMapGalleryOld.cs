using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DACrux.Map;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using DACrux.Base;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// DPUCDefectMapGallery에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCDefectMapGalleryOld : System.Windows.Forms.UserControl, DACrux.Framework.Base.ISendDefect
    {
        DACrux.Base.DPWafer[] wafer = null;

        System.Threading.Thread drawMapThread = null;
        int lastSelectedMapNo = -1;
        const int SCROLLBAR_WIDTH = 20;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonRedraw;
        private System.Windows.Forms.Button buttonFirst;
        private System.Windows.Forms.ComboBox comboBoxColCnt;
        private System.Windows.Forms.Button buttonPre;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonLast;
        private System.Windows.Forms.Label labelCurWaferNo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelGallery;
        private System.Windows.Forms.Panel panelSingle;
        private DACrux.Map.DefectMap defectMap;
        private System.Windows.Forms.Panel panelNavi;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonUnselectAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button buttonToExcel;
        private CheckBox chkImageMarker;
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public DPUCDefectMapGalleryOld()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();

            // TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.
        }

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveMapControl();

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드
        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPUCDefectMapGalleryOld));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkImageMarker = new System.Windows.Forms.CheckBox();
            this.buttonToExcel = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.buttonUnselectAll = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.panelNavi = new System.Windows.Forms.Panel();
            this.labelCurWaferNo = new System.Windows.Forms.Label();
            this.buttonLast = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPre = new System.Windows.Forms.Button();
            this.buttonFirst = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonRedraw = new System.Windows.Forms.Button();
            this.comboBoxColCnt = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelGallery = new System.Windows.Forms.Panel();
            this.panelSingle = new System.Windows.Forms.Panel();
            this.defectMap = new DACrux.Map.DefectMap();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelNavi.SuspendLayout();
            this.panelSingle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkImageMarker);
            this.panel1.Controls.Add(this.buttonToExcel);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panelNavi);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.buttonRedraw);
            this.panel1.Controls.Add(this.comboBoxColCnt);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(880, 24);
            this.panel1.TabIndex = 0;
            // 
            // chkImageMarker
            // 
            this.chkImageMarker.AutoSize = true;
            this.chkImageMarker.Location = new System.Drawing.Point(734, 5);
            this.chkImageMarker.Name = "chkImageMarker";
            this.chkImageMarker.Size = new System.Drawing.Size(102, 16);
            this.chkImageMarker.TabIndex = 12;
            this.chkImageMarker.Text = "Image Marker";
            this.chkImageMarker.UseVisualStyleBackColor = true;
            this.chkImageMarker.CheckedChanged += new System.EventHandler(this.chkImageMarker_CheckedChanged);
            // 
            // buttonToExcel
            // 
            this.buttonToExcel.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonToExcel.Image = ((System.Drawing.Image)(resources.GetObject("buttonToExcel.Image")));
            this.buttonToExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonToExcel.Location = new System.Drawing.Point(648, 0);
            this.buttonToExcel.Name = "buttonToExcel";
            this.buttonToExcel.Size = new System.Drawing.Size(80, 24);
            this.buttonToExcel.TabIndex = 11;
            this.buttonToExcel.Text = "    ToExcel";
            this.buttonToExcel.Click += new System.EventHandler(this.buttonToExcel_Click);
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(632, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(16, 24);
            this.panel5.TabIndex = 10;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.buttonUnselectAll);
            this.panel4.Controls.Add(this.buttonSelectAll);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(416, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(216, 24);
            this.panel4.TabIndex = 7;
            // 
            // buttonUnselectAll
            // 
            this.buttonUnselectAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonUnselectAll.Image = ((System.Drawing.Image)(resources.GetObject("buttonUnselectAll.Image")));
            this.buttonUnselectAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonUnselectAll.Location = new System.Drawing.Point(104, 0);
            this.buttonUnselectAll.Name = "buttonUnselectAll";
            this.buttonUnselectAll.Size = new System.Drawing.Size(112, 24);
            this.buttonUnselectAll.TabIndex = 1;
            this.buttonUnselectAll.Text = "   Unselect All";
            this.buttonUnselectAll.Click += new System.EventHandler(this.buttonUnselectAll_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("buttonSelectAll.Image")));
            this.buttonSelectAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSelectAll.Location = new System.Drawing.Point(0, 0);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(104, 24);
            this.buttonSelectAll.TabIndex = 0;
            this.buttonSelectAll.Text = "   Select All";
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // panelNavi
            // 
            this.panelNavi.Controls.Add(this.labelCurWaferNo);
            this.panelNavi.Controls.Add(this.buttonLast);
            this.panelNavi.Controls.Add(this.buttonNext);
            this.panelNavi.Controls.Add(this.buttonPre);
            this.panelNavi.Controls.Add(this.buttonFirst);
            this.panelNavi.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelNavi.Location = new System.Drawing.Point(208, 0);
            this.panelNavi.Name = "panelNavi";
            this.panelNavi.Size = new System.Drawing.Size(208, 24);
            this.panelNavi.TabIndex = 8;
            // 
            // labelCurWaferNo
            // 
            this.labelCurWaferNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurWaferNo.Location = new System.Drawing.Point(160, 0);
            this.labelCurWaferNo.Name = "labelCurWaferNo";
            this.labelCurWaferNo.Size = new System.Drawing.Size(48, 24);
            this.labelCurWaferNo.TabIndex = 6;
            this.labelCurWaferNo.Text = "1/10";
            this.labelCurWaferNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonLast
            // 
            this.buttonLast.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonLast.Image = ((System.Drawing.Image)(resources.GetObject("buttonLast.Image")));
            this.buttonLast.Location = new System.Drawing.Point(120, 0);
            this.buttonLast.Name = "buttonLast";
            this.buttonLast.Size = new System.Drawing.Size(40, 24);
            this.buttonLast.TabIndex = 5;
            this.buttonLast.Click += new System.EventHandler(this.buttonLast_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonNext.Image = ((System.Drawing.Image)(resources.GetObject("buttonNext.Image")));
            this.buttonNext.Location = new System.Drawing.Point(80, 0);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(40, 24);
            this.buttonNext.TabIndex = 4;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonPre
            // 
            this.buttonPre.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonPre.Image = ((System.Drawing.Image)(resources.GetObject("buttonPre.Image")));
            this.buttonPre.Location = new System.Drawing.Point(40, 0);
            this.buttonPre.Name = "buttonPre";
            this.buttonPre.Size = new System.Drawing.Size(40, 24);
            this.buttonPre.TabIndex = 3;
            this.buttonPre.Click += new System.EventHandler(this.buttonPre_Click);
            // 
            // buttonFirst
            // 
            this.buttonFirst.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonFirst.Image = ((System.Drawing.Image)(resources.GetObject("buttonFirst.Image")));
            this.buttonFirst.Location = new System.Drawing.Point(0, 0);
            this.buttonFirst.Name = "buttonFirst";
            this.buttonFirst.Size = new System.Drawing.Size(40, 24);
            this.buttonFirst.TabIndex = 1;
            this.buttonFirst.Click += new System.EventHandler(this.buttonFirst_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(192, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(16, 24);
            this.panel2.TabIndex = 7;
            // 
            // buttonRedraw
            // 
            this.buttonRedraw.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRedraw.Image = ((System.Drawing.Image)(resources.GetObject("buttonRedraw.Image")));
            this.buttonRedraw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRedraw.Location = new System.Drawing.Point(104, 0);
            this.buttonRedraw.Name = "buttonRedraw";
            this.buttonRedraw.Size = new System.Drawing.Size(88, 24);
            this.buttonRedraw.TabIndex = 0;
            this.buttonRedraw.Text = " Redraw";
            this.buttonRedraw.Click += new System.EventHandler(this.buttonRedraw_Click);
            // 
            // comboBoxColCnt
            // 
            this.comboBoxColCnt.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBoxColCnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColCnt.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.comboBoxColCnt.Location = new System.Drawing.Point(56, 0);
            this.comboBoxColCnt.Name = "comboBoxColCnt";
            this.comboBoxColCnt.Size = new System.Drawing.Size(48, 20);
            this.comboBoxColCnt.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 24);
            this.label1.TabIndex = 9;
            this.label1.Text = "Col Cnt";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelGallery
            // 
            this.panelGallery.AutoScroll = true;
            this.panelGallery.BackColor = System.Drawing.Color.White;
            this.panelGallery.Location = new System.Drawing.Point(3, 30);
            this.panelGallery.Name = "panelGallery";
            this.panelGallery.Size = new System.Drawing.Size(112, 104);
            this.panelGallery.TabIndex = 2;
            this.panelGallery.SizeChanged += new System.EventHandler(this.panelGallery_SizeChanged);
            this.panelGallery.Click += new System.EventHandler(this.panelGallery_Click);
            // 
            // panelSingle
            // 
            this.panelSingle.Controls.Add(this.defectMap);
            this.panelSingle.Location = new System.Drawing.Point(121, 30);
            this.panelSingle.Name = "panelSingle";
            this.panelSingle.Size = new System.Drawing.Size(128, 120);
            this.panelSingle.TabIndex = 3;
            // 
            // defectMap
            // 
            this.defectMap.AngleOffSet = 0;
            this.defectMap.CenterMark = false;
            this.defectMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.defectMap.DataSource = null;
            this.defectMap.DieBackgroundColor = System.Drawing.Color.Black;
            this.defectMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.defectMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.defectMap.DieMaxX = 0;
            this.defectMap.DieMaxY = 0;
            this.defectMap.DieMinX = 0;
            this.defectMap.DieMinY = 0;
            this.defectMap.DieSizeX = 0.01D;
            this.defectMap.DieSizeY = 0.01D;
            this.defectMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.defectMap.DisplayValue = "BIN";
            this.defectMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectMap.DrawDefectImage = null;
            this.defectMap.DrawDefects = "ALL";
            this.defectMap.DrawFirstDie = true;
            this.defectMap.DrawMarkDie = false;
            this.defectMap.DrawOriginDie = true;
            this.defectMap.DrawSkipDie = true;
            this.defectMap.EdgeColor = System.Drawing.Color.LightGray;
            this.defectMap.EdgeSize = 1D;
            this.defectMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.defectMap.FirstDieX = 0;
            this.defectMap.FirstDieY = 0;
            this.defectMap.ForeColor = System.Drawing.Color.Red;
            this.defectMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.defectMap.GradationInterval = 5;
            this.defectMap.GradationMaxValue = double.NaN;
            this.defectMap.GradationMinValue = double.NaN;
            this.defectMap.Location = new System.Drawing.Point(0, 0);
            this.defectMap.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.defectMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.defectMap.Name = "defectMap";
            this.defectMap.NotchAngle = 0;
            this.defectMap.NotchType = DACrux.Base.Notch.Flat;
            this.defectMap.OriginDieBorder = System.Drawing.Color.Red;
            this.defectMap.OriginIndexX = 0;
            this.defectMap.OriginIndexY = 0;
            this.defectMap.OriginX = 0D;
            this.defectMap.OriginY = 0D;
            this.defectMap.ParametricColumn = "PCMVALUE";
            this.defectMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.defectMap.PickupDieAlpha = 96;
            this.defectMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.defectMap.PopupMenu = true;
            this.defectMap.ReferenceDieSetting = 0;
            this.defectMap.ScaleMark = false;
            this.defectMap.SelecetedBin = "ALL";
            this.defectMap.SelectedVI = "ALL";
            this.defectMap.ShotLineWidth = 2;
            this.defectMap.Size = new System.Drawing.Size(128, 120);
            this.defectMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.defectMap.TabIndex = 0;
            this.defectMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.defectMap.TransParent = 255;
            this.defectMap.ViewAngle = 0;
            this.defectMap.VIMember = "VIFAIL";
            this.defectMap.VisibleDieBorder = true;
            this.defectMap.VisibleDieValue = false;
            this.defectMap.VisibleFocusDie = false;
            this.defectMap.VisibleImageMark = true;
            this.defectMap.VisibleInfomation = true;
            this.defectMap.VisibleOffDie = false;
            this.defectMap.VisibleProbeOverlay = false;
            this.defectMap.VisibleStringBin = false;
            this.defectMap.VisibleVIFail = false;
            this.defectMap.VisibleXY = false;
            this.defectMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.defectMap.WaferColor = System.Drawing.Color.Gray;
            this.defectMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.defectMap.WaferMargin = 0.95D;
            this.defectMap.WaferSize = 200000D;
            this.defectMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.defectMap.DoubleClick += new System.EventHandler(this.defectMap_DoubleClick);
            // 
            // DPUCDefectMapGalleryOld
            // 
            this.Controls.Add(this.panelSingle);
            this.Controls.Add(this.panelGallery);
            this.Controls.Add(this.panel1);
            this.Name = "DPUCDefectMapGalleryOld";
            this.Size = new System.Drawing.Size(880, 496);
            this.Load += new System.EventHandler(this.DPUCDefectMapGallery_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panelNavi.ResumeLayout(false);
            this.panelSingle.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public DACrux.Base.DPWafer[] Wafer
        {
            get
            {
                return this.wafer;
            }
            set
            {
                this.wafer = value;
            }
        }

        public void Draw()
        {
            try
            {
                if (this.wafer == null) return;

                if (drawMapThread != null)
                {
                    if (drawMapThread.IsAlive)
                    {
                        MessageBox.Show(this, "Drawing Map...", this.Name);
                        return;
                    }
                    else
                    {
                        drawMapThread = null;
                    }
                }

                if (this.wafer.Length > 100)
                    throw new Exception("현재 Wafer 100개 이상은 처리 할 수 없습니다.");

                RemoveMapControl();
                
                //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(CreateMapControl));
                //thread.Start();
                CreateMapControl();

                drawMapThread = new System.Threading.Thread(new System.Threading.ThreadStart(DrawMap));
                drawMapThread.Name = "Defect Map Gallery";
                drawMapThread.Start();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void RemoveMapControl()
        {
            try
            {
                for (int i = panelGallery.Controls.Count - 1; i >= 0; i--)
                {
                    ((DPUCWrapDefectMap)panelGallery.Controls[i]).Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CreateMapControl()
        {
            try
            {
                int colCnt = DACrux.Base.Convert.intParse(comboBoxColCnt.Text);
                int width = (panelGallery.Width - SCROLLBAR_WIDTH) / colCnt;

                int row = 0;
                int col = 0;
                for (int i = 0; i < this.wafer.Length; i++)
                {
                    DPUCWrapDefectMap map = new DPUCWrapDefectMap();
                    map.OnDblClk += new DACrux.SEMDMS.Control.DPUCWrapDefectMap.EventDefectMap(map_OnDblClk);
                    map.OnSelectedChange += new DACrux.SEMDMS.Control.DPUCWrapDefectMap.EventDefectMap(map_OnSelectedChange);
                    map.MapNo = i;

                    if (col == colCnt)
                    {
                        row++;
                        col = 0;
                    }
                    panelGallery.Controls.Add(map);
                    map.Location = new Point(col * width, row * width);
                    map.Size = new Size(width, width);
                    col++;
                }
                labelCurWaferNo.Text = string.Format("{0}/{1}", "", panelGallery.Controls.Count);

                panelGallery.ResumeLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void DrawMap()
        {
            try
            {
                DPUCWrapDefectMap map = null;
                for (int i = 0; i < this.wafer.Length; i++)
                {
                    map = (DPUCWrapDefectMap)panelGallery.Controls[i];
                    map.Draw(DACrux.Base.Convert.longParse(this.wafer[i].StepSeq), chkImageMarker.Checked);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void panelGallery_SizeChanged(object sender, System.EventArgs e)
        {
            if (DesignMode) return;

            if (panelGallery.Controls.Count == 0) return;

            try
            {
                int colCnt = (panelGallery.Width - SCROLLBAR_WIDTH) / panelGallery.Controls[0].Width;
                int curCol = 0;
                int curRow = 0;
                // map의 배열을 다시 한다
                panelGallery.SuspendLayout();
                for (int i = 0; i < panelGallery.Controls.Count; i++)
                {
                    panelGallery.Controls[i].Location =
                        new Point(curCol * panelGallery.Controls[0].Width + panelGallery.DisplayRectangle.Left,
                        curRow * panelGallery.Controls[0].Height + panelGallery.DisplayRectangle.Top);
                    curCol++;
                    if (curCol == colCnt)
                    {
                        curCol = 0;
                        curRow++;
                    }
                }

                panelGallery.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void map_OnDblClk(DPUCWrapDefectMap map)
        {
            try
            {
                panelSingle.BringToFront();
                map.Copy(this.defectMap);
                defectMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                panelNavi.Enabled = !panelNavi.Enabled;
                labelCurWaferNo.Text = string.Format("{0}/{1}", map.MapNo + 1, panelGallery.Controls.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void DPUCDefectMapGallery_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;
            panelSingle.Dock = DockStyle.Fill;
            panelGallery.Dock = DockStyle.Fill;
            panelGallery.BringToFront();
            panelNavi.Enabled = false;
            comboBoxColCnt.SelectedIndex = 4;
        }

        private void defectMap_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                panelNavi.Enabled = !panelNavi.Enabled;
                panelGallery.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void map_OnSelectedChange(DPUCWrapDefectMap map)
        {
            try
            {
                if (ModifierKeys == Keys.Control || ModifierKeys == Keys.ControlKey)
                {
                    map.Selected = !map.Selected;
                    lastSelectedMapNo = map.MapNo;
                }
                else
                {
                    DPUCWrapDefectMap mapObj = null;
                    for (int i = 0; i < panelGallery.Controls.Count; i++)
                    {
                        mapObj = (DPUCWrapDefectMap)panelGallery.Controls[i];
                        mapObj.Selected = false;
                    }

                    if (ModifierKeys == Keys.Shift || ModifierKeys == Keys.ShiftKey)
                    {
                        if (lastSelectedMapNo == map.MapNo)
                        {
                            map.Selected = true;
                            return;
                        }

                        if (lastSelectedMapNo == -1)
                        {
                            map.Selected = !map.Selected;
                        }
                        else
                        {
                            int small = -1;
                            int large = -1;
                            if (lastSelectedMapNo < map.MapNo)
                            {
                                small = lastSelectedMapNo;
                                large = map.MapNo;
                            }
                            else
                            {
                                small = map.MapNo;
                                large = lastSelectedMapNo;
                            }

                            for (int i = 0; i < panelGallery.Controls.Count; i++)
                            {
                                mapObj = (DPUCWrapDefectMap)panelGallery.Controls[i];
                                if (mapObj.MapNo >= small && mapObj.MapNo <= large) mapObj.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        map.Selected = true;
                        lastSelectedMapNo = map.MapNo;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void panelGallery_Click(object sender, System.EventArgs e)
        {
            try
            {
                DPUCWrapDefectMap mapObj = null;
                for (int i = 0; i < panelGallery.Controls.Count; i++)
                {
                    mapObj = (DPUCWrapDefectMap)panelGallery.Controls[i];
                    mapObj.Selected = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void buttonRedraw_Click(object sender, System.EventArgs e)
        {
            try
            {
                DPUCWrapDefectMap map = null;
                int colCnt = DACrux.Base.Convert.intParse(comboBoxColCnt.Text);
                int width = (panelGallery.Width - SCROLLBAR_WIDTH) / colCnt;

                int col = 0;
                int row = 0;
                for (int i = 0; i < panelGallery.Controls.Count; i++)
                {
                    map = (DPUCWrapDefectMap)panelGallery.Controls[i];
                    map.DefectMap.VisibleImageMark = chkImageMarker.Checked;
                    map.DefectMap.Redraw();
                    map.Size = new Size(width, width);
                    if (col == colCnt)
                    {
                        col = 0;
                        row++;
                    }
                    map.Location = new Point(col * width, row * width);
                    col++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void buttonSelectAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                DPUCWrapDefectMap mapObj = null;
                for (int i = 0; i < panelGallery.Controls.Count; i++)
                {
                    mapObj = (DPUCWrapDefectMap)panelGallery.Controls[i];
                    mapObj.Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void buttonUnselectAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                DPUCWrapDefectMap mapObj = null;
                for (int i = 0; i < panelGallery.Controls.Count; i++)
                {
                    mapObj = (DPUCWrapDefectMap)panelGallery.Controls[i];
                    mapObj.Selected = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void buttonToExcel_Click(object sender, System.EventArgs e)
        {
            //0.객체 선언 , 인스턴싱
            DACrux.Utility.ExcelUtil oXL = new DACrux.Utility.ExcelUtil();

            //1.엑셀 오브젝트 얻어 오기
            Excel._Workbook oWB = oXL.fnGetExcelWorkbook(true, 3);

            string[,] strHeader;
            string tmpFile = string.Empty;
            Bitmap bmpWaferMap = null;
            DefectMap tmpMapSingle = null;
            DPUCWrapDefectMap tmpMapGallery = null;

            try
            {
                /// MAP============================================================================================================
                /// 
                strHeader = new string[1, 1];
                strHeader[0, 0] = "Wafer Map & Chart";
                tmpFile = string.Format(@"{0}\tmpMap.bmp", Application.StartupPath);


                if (this.Controls[0].Name.Equals("panelSingle"))
                {
                    if (panelSingle.Controls.Count != 0)
                    {
                        oXL.fnSetSheetName(oWB, 2, "Single Map");

                        //map = (DPUCWrapDefectMap)panelSingle.Controls[0];
                        tmpMapSingle = panelSingle.Controls[0] as DefectMap;
                        if (tmpMapSingle == null) return;
                        //bmpWaferMap = map.GetMapImage();
                        bmpWaferMap = tmpMapSingle.GetMapImage();

                        bmpWaferMap.Save(tmpFile);
                        oXL.fnSetImage(oWB, 1, tmpFile, 1, 1);
                    }
                }
                else if (this.Controls[0].Name.Equals("panelGallery"))
                {
                    if (panelGallery.Controls.Count != 0)
                    {
                        oXL.fnSetSheetName(oWB, 1, "Map Gallery");
                        oXL.fnAllCellResize(oWB, 1, panelGallery.Controls[0].Width, panelGallery.Controls[0].Height);

                        int maxColCnt = DACrux.Base.Convert.intParse(comboBoxColCnt.Text);
                        int row = 1;
                        int col = 1;

                        for (int i = 0; i < panelGallery.Controls.Count; i++)
                        {
                            tmpMapGallery = panelGallery.Controls[i] as DPUCWrapDefectMap;
                            if (tmpMapGallery == null) return;
                            bmpWaferMap = tmpMapGallery.GetMapImage();

                            bmpWaferMap.Save(tmpFile);
                            oXL.fnSetImage(oWB, 1, tmpFile, row, col);

                            col++;
                            if (col == maxColCnt + 1)
                            {
                                col = 1;
                                row++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
                oXL = null;
                if (tmpMapSingle != null) tmpMapSingle.Dispose();
                tmpMapSingle = null;

                //--

                if (tmpMapGallery != null) tmpMapGallery.Dispose();
                tmpMapGallery = null;

                //--

                System.IO.File.Delete(tmpFile);
                strHeader = null;
                DACrux.Utility.ExcelUtil.fnExcelProcessExit();
            }
        }

        private void buttonFirst_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (this.lastSelectedMapNo == 0) return;
                DPUCWrapDefectMap map = (DPUCWrapDefectMap)panelGallery.Controls[0];
                map.Copy(this.defectMap);
                this.lastSelectedMapNo = map.MapNo;
                labelCurWaferNo.Text = string.Format("{0}/{1}", map.MapNo + 1, panelGallery.Controls.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void buttonLast_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (this.lastSelectedMapNo == panelGallery.Controls.Count - 1) return;
                DPUCWrapDefectMap map = (DPUCWrapDefectMap)panelGallery.Controls[panelGallery.Controls.Count - 1];
                map.Copy(this.defectMap);
                this.lastSelectedMapNo = map.MapNo;
                labelCurWaferNo.Text = string.Format("{0}/{1}", map.MapNo + 1, panelGallery.Controls.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void buttonNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                DPUCWrapDefectMap map = null;
                for (int i = 0; i < panelGallery.Controls.Count; i++)
                {
                    map = (DPUCWrapDefectMap)panelGallery.Controls[i];
                    if (this.lastSelectedMapNo + 1 == map.MapNo)
                    {
                        this.lastSelectedMapNo++;
                        map.Copy(this.defectMap);
                        labelCurWaferNo.Text = string.Format("{0}/{1}", map.MapNo + 1, panelGallery.Controls.Count);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void buttonPre_Click(object sender, System.EventArgs e)
        {
            try
            {
                DPUCWrapDefectMap map = null;
                for (int i = 0; i < panelGallery.Controls.Count; i++)
                {
                    map = (DPUCWrapDefectMap)panelGallery.Controls[i];
                    if (this.lastSelectedMapNo - 1 == map.MapNo)
                    {
                        this.lastSelectedMapNo--;
                        map.Copy(this.defectMap);
                        labelCurWaferNo.Text = string.Format("{0}/{1}", map.MapNo + 1, panelGallery.Controls.Count);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void chkImageMarker_CheckedChanged(object sender, EventArgs e)
        {

        }

        public Defect[] GetSelectedDefect()
        {
            List<Defect> list = new List<Defect>();

            foreach (DPUCWrapDefectMap map in panelGallery.Controls)
            {
                if (map.Selected)
                {
                    if (map.DefectMap.SelectedDefect.Count > 0)
                        return map.DefectMap.SelectedDefect.ToArray();
                    else
                        return map.DefectMap.Defects.ToArray();
                }
            }

            return list.ToArray();
        }
    }
}
