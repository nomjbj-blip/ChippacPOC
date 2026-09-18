using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DACrux.Map;
using DACrux.Common.RO;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// frmDefecMapAnalysis에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCBareDefecMapViewer : DACrux.Framework.Base.DACruxCTLBasic01, DACrux.Framework.Base.ISendDefect, DACrux.Framework.Base.IExportExcel
    {
        DataTable dtSizeColor = null;
        DataTable dtTypeColor = null;
        DataTable dtNoColoring = null;

        DataSet dsDefect = null;
        private DACrux.Map.DefectMap map;
        //private SoftwareFX.ChartFX.Chart chart;
        private Panel MapPanel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ToolWaferDieLocation;
        private ToolStripStatusLabel ToolDefectCount;
        private ToolStripStatusLabel ToolTotalDie;
        private Panel panel1;
        private ListBox lsDeviceList;
        private Panel panel2;
        private Label label1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private TextBox TxtStepID;
        private Label label5;
        private TextBox TxtDeviceID;
        private Label label4;
        private TextBox TxtWaferID;
        private Label label3;
        private TextBox TxtLotID;
        private Label label2;
        private Button BtnRedraw;
        private CheckBox chkNoneMap;
        private System.ComponentModel.IContainer components;

        public DPUCBareDefecMapViewer()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();
            //
            // TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
            //
        }

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPUCBareDefecMapViewer));
            this.map = new DACrux.Map.DefectMap();
            this.MapPanel = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolWaferDieLocation = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolTotalDie = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolDefectCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lsDeviceList = new System.Windows.Forms.ListBox();
            this.BtnRedraw = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TxtStepID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtDeviceID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtWaferID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtLotID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.chkNoneMap = new System.Windows.Forms.CheckBox();
            this.MapPanel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DieBackgroundColor = System.Drawing.Color.Black;
            this.map.DieBorderColor = System.Drawing.Color.LightGray;
            this.map.DieDefectColor = System.Drawing.Color.Empty;
            this.map.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.map.DieMaxX = 0;
            this.map.DieMaxY = 0;
            this.map.DieMinX = 0;
            this.map.DieMinY = 0;
            this.map.DieSizeX = 0.01D;
            this.map.DieSizeY = 0.01D;
            this.map.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.map.DisplayValue = "BIN";
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.DrawDefectImage = null;
            this.map.DrawDefects = "ALL";
            this.map.DrawFirstDie = true;
            this.map.DrawMarkDie = false;
            this.map.DrawOriginDie = true;
            this.map.DrawSkipDie = true;
            this.map.EdgeColor = System.Drawing.Color.LightGray;
            this.map.EdgeSize = 1D;
            this.map.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.map.FirstDieX = 0;
            this.map.FirstDieY = 0;
            this.map.ForeColor = System.Drawing.Color.Red;
            this.map.FromGradationDieColor = System.Drawing.Color.Lime;
            this.map.GradationInterval = 5;
            this.map.GradationMaxValue = double.NaN;
            this.map.GradationMinValue = double.NaN;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.map.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.map.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.map.Name = "map";
            this.map.NotchAngle = 0;
            this.map.NotchType = DACrux.Base.Notch.Flat;
            this.map.OriginDieBorder = System.Drawing.Color.Red;
            this.map.OriginIndexX = 0;
            this.map.OriginIndexY = 0;
            this.map.OriginX = 0D;
            this.map.OriginY = 0D;
            this.map.ParaLimit = false;
            this.map.ParametricColumn = "PCMVALUE";
            this.map.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.map.PickupDieAlpha = 96;
            this.map.PickupedDieColor = System.Drawing.Color.Transparent;
            this.map.PopupMenu = true;
            this.map.ReferenceDieSetting = 0;
            this.map.ScaleMark = false;
            this.map.SelecetedBin = "ALL";
            this.map.SelectedVI = "ALL";
            this.map.Size = new System.Drawing.Size(775, 576);
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 9;
            this.map.ToGradationDieColor = System.Drawing.Color.Red;
            this.map.TransParent = 255;
            this.map.ViewAngle = 0;
            this.map.VIMember = "VIFAIL";
            this.map.VisibleDieBorder = true;
            this.map.VisibleDieValue = false;
            this.map.VisibleFocusDie = false;
            this.map.VisibleImageMark = true;
            this.map.VisibleInfomation = true;
            this.map.VisibleOffDie = false;
            this.map.VisibleProbeOverlay = false;
            this.map.VisibleShotAlignPoint = false;
            this.map.VisibleSignDies = false;
            this.map.VisibleStringBin = false;
            this.map.VisibleVIFail = false;
            this.map.VisibleXY = false;
            this.map.WaferBorderColor = System.Drawing.Color.LightGray;
            this.map.WaferColor = System.Drawing.Color.DimGray;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.map.WaferID = "";
            this.map.WaferMargin = 0.95D;
            this.map.WaferSize = 200000D;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.map.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.map_OnChangeCurrentDie);
            // 
            // MapPanel
            // 
            this.MapPanel.Controls.Add(this.map);
            this.MapPanel.Controls.Add(this.statusStrip1);
            this.MapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapPanel.Location = new System.Drawing.Point(238, 0);
            this.MapPanel.Name = "MapPanel";
            this.MapPanel.Size = new System.Drawing.Size(775, 600);
            this.MapPanel.TabIndex = 15;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolWaferDieLocation,
            this.ToolTotalDie,
            this.ToolDefectCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 576);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(775, 24);
            this.statusStrip1.TabIndex = 0;
            // 
            // ToolWaferDieLocation
            // 
            this.ToolWaferDieLocation.AutoSize = false;
            this.ToolWaferDieLocation.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolWaferDieLocation.Name = "ToolWaferDieLocation";
            this.ToolWaferDieLocation.Size = new System.Drawing.Size(150, 19);
            this.ToolWaferDieLocation.Text = "X : 0, Y : 0";
            // 
            // ToolTotalDie
            // 
            this.ToolTotalDie.AutoSize = false;
            this.ToolTotalDie.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolTotalDie.Name = "ToolTotalDie";
            this.ToolTotalDie.Size = new System.Drawing.Size(180, 19);
            this.ToolTotalDie.Text = "Total Die Count : 0";
            // 
            // ToolDefectCount
            // 
            this.ToolDefectCount.AutoSize = false;
            this.ToolDefectCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolDefectCount.Name = "ToolDefectCount";
            this.ToolDefectCount.Size = new System.Drawing.Size(180, 19);
            this.ToolDefectCount.Text = "Total Defect Count : 0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lsDeviceList);
            this.panel1.Controls.Add(this.BtnRedraw);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(232, 600);
            this.panel1.TabIndex = 16;
            // 
            // lsDeviceList
            // 
            this.lsDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsDeviceList.FormattingEnabled = true;
            this.lsDeviceList.ItemHeight = 12;
            this.lsDeviceList.Location = new System.Drawing.Point(0, 153);
            this.lsDeviceList.Name = "lsDeviceList";
            this.lsDeviceList.Size = new System.Drawing.Size(232, 417);
            this.lsDeviceList.TabIndex = 1;
            this.lsDeviceList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsDeviceList_MouseDoubleClick);
            // 
            // BtnRedraw
            // 
            this.BtnRedraw.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BtnRedraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRedraw.Image = ((System.Drawing.Image)(resources.GetObject("BtnRedraw.Image")));
            this.BtnRedraw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRedraw.Location = new System.Drawing.Point(0, 570);
            this.BtnRedraw.Name = "BtnRedraw";
            this.BtnRedraw.Size = new System.Drawing.Size(232, 30);
            this.BtnRedraw.TabIndex = 3;
            this.BtnRedraw.Text = "ReDraw";
            this.BtnRedraw.UseVisualStyleBackColor = true;
            this.BtnRedraw.Click += new System.EventHandler(this.BtnRedraw_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device Map List";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.chkNoneMap);
            this.panel2.Controls.Add(this.TxtStepID);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.TxtDeviceID);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.TxtWaferID);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.TxtLotID);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(232, 135);
            this.panel2.TabIndex = 0;
            // 
            // TxtStepID
            // 
            this.TxtStepID.Location = new System.Drawing.Point(70, 84);
            this.TxtStepID.Name = "TxtStepID";
            this.TxtStepID.ReadOnly = true;
            this.TxtStepID.Size = new System.Drawing.Size(155, 21);
            this.TxtStepID.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "Step ID";
            // 
            // TxtDeviceID
            // 
            this.TxtDeviceID.Location = new System.Drawing.Point(70, 57);
            this.TxtDeviceID.Name = "TxtDeviceID";
            this.TxtDeviceID.ReadOnly = true;
            this.TxtDeviceID.Size = new System.Drawing.Size(155, 21);
            this.TxtDeviceID.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Device";
            // 
            // TxtWaferID
            // 
            this.TxtWaferID.Location = new System.Drawing.Point(70, 30);
            this.TxtWaferID.Name = "TxtWaferID";
            this.TxtWaferID.ReadOnly = true;
            this.TxtWaferID.Size = new System.Drawing.Size(155, 21);
            this.TxtWaferID.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "Wafer ID";
            // 
            // TxtLotID
            // 
            this.TxtLotID.Location = new System.Drawing.Point(70, 3);
            this.TxtLotID.Name = "TxtLotID";
            this.TxtLotID.ReadOnly = true;
            this.TxtLotID.Size = new System.Drawing.Size(155, 21);
            this.TxtLotID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Lot ID";
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.Location = new System.Drawing.Point(232, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 0;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 600);
            this.ultraSplitter1.TabIndex = 17;
            // 
            // chkNoneMap
            // 
            this.chkNoneMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkNoneMap.AutoSize = true;
            this.chkNoneMap.Location = new System.Drawing.Point(136, 114);
            this.chkNoneMap.Name = "chkNoneMap";
            this.chkNoneMap.Size = new System.Drawing.Size(89, 16);
            this.chkNoneMap.TabIndex = 3;
            this.chkNoneMap.Text = "Only Defect";
            this.chkNoneMap.UseVisualStyleBackColor = true;
            this.chkNoneMap.CheckedChanged += new System.EventHandler(this.chkNoneMap_CheckedChanged);
            // 
            // DPUCBareDefecMapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.Controls.Add(this.MapPanel);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.panel1);
            this.Name = "DPUCBareDefecMapViewer";
            this.Size = new System.Drawing.Size(1013, 600);
            this.Load += new System.EventHandler(this.DPUCBareDefecMapViewer_Load);
            this.MapPanel.ResumeLayout(false);
            this.MapPanel.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region [ Event Handler ]


        private void DPUCBareDefecMapViewer_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;
            try
            {
                DefectColorSet();
                NoDefectColoring();
                map.DefectSize = GetDefaultDefectSize();
                GetDeviceList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();

            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : DefectMap.DEFAULT_DEFECT_SIZE;
        }


        private void map_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            try
            {
                ToolWaferDieLocation.Text = string.Format("X : {0} Y : {1}", NewDie.IndexX, NewDie.IndexY);

                //DataRow[] dr = ((DataTable)fpSpreadDefect_Sheet.DataSource).Select(string.Format("XINDEX = '{0}' AND YINDEX = '{1}'", NewDie.IndexX, NewDie.IndexY));
                //ToolParticles.Text = string.Format("Particles : {0}", dr.Length);
            }
            catch (Exception)
            {
            }
        }

        private void BtnRedraw_Click(object sender, EventArgs e)
        {
            Draw();
        }


        private void lsDeviceList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Draw();
        }


        private void chkNoneMap_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }


        #endregion [ Event Handler ]

        #region [ Method ]

        public void GetDeviceList()
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = new RO.DefectMapAnalysis();
            DataTable dtDeviceList = oDMapAnalysis.GetShotDeviceList();

            lsDeviceList.Items.Clear();
            foreach (DataRow dr in dtDeviceList.Rows)
            {
                lsDeviceList.Items.Add(dr[0]);
            }
        }

        public void Draw()
        {
            if (this.Wafer == null) return;

            long[] lStepSeq = null;
            string strDeviceID = string.Empty;

            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            ComConfiguration oComConfig = null;

            StatusMessage("조회를 시작 합니다.");

            try
            {
                if (dsDefect != null)
                    dsDefect.Dispose();

                TxtDeviceID.Text = string.Empty;
                TxtLotID.Text = string.Empty;
                TxtWaferID.Text = string.Empty;
                TxtStepID.Text = string.Empty;

                dsDefect = new DataSet();

                oDMapAnalysis = new RO.DefectMapAnalysis();
                oComConfig = new ComConfiguration();

                lStepSeq = new long[this.Wafer.Length];

                for (int iw = 0; iw < this.Wafer.Length; iw++)
                {
                    lStepSeq[iw] = DACrux.Base.Convert.longParse(this.Wafer[iw].StepSeq);
                }


                if (lsDeviceList.SelectedItems.Count > 0 && string.IsNullOrEmpty(lsDeviceList.SelectedItem.ToString()) == false)
                    strDeviceID = lsDeviceList.SelectedItem.ToString();

                //=================================================================================================================================
                //Setup Seq 정보를 가져 온다.
                //=================================================================================================================================
                StatusMessage("Data 를 조회 중입니다.");
                dsDefect = oDMapAnalysis.GetBareDefectMapViewer(lStepSeq, strDeviceID);

                StatusMessage("Data를 처리 중입니다.");
                map.SetDensity = "ALL";
                if (dsDefect.Tables.IndexOf("SETUP_INFO") > 0 && dsDefect.Tables["SETUP_INFO"].Rows.Count > 0)
                {
                    map.WaferSize = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["WAFER_SIZE"].ToString());
                    map.NotchType = DACrux.Base.Notch.Notch; // dsDefect.Tables["SETUP_INFO"].Rows[0]["NOTCH_TYPE"].ToString().Equals("F") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;
                    map.AngleOffSet = 0;
                    map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                    map.NotchAngle = 0;// DOWN으로 저장하여 보여주므로 0으로 설정 DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ANGLE"].ToString());
                    map.DieSizeX = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_X"].ToString());
                    map.DieSizeY = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_Y"].ToString());
                    map.OriginIndexX = DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_X"].ToString());
                    map.OriginIndexY = DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_Y"].ToString());
                    map.OriginX = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_X"].ToString());
                    map.OriginY = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_Y"].ToString());
                    map.DieCalculation(true);
                    map.DrawDefects = "ALL";

                    // SHOT 정보 설정
                    if (dsDefect.Tables.Contains("STEP_INFO") && dsDefect.Tables["STEP_INFO"].Rows.Count > 0 && dsDefect.Tables["STEP_INFO"].Columns.Contains("ST_XCNT"))
                    {
                        DataRow stepRow = dsDefect.Tables["STEP_INFO"].Rows[0];
                        map.ShotArrayX = DACrux.Base.Convert.intParse(stepRow["ST_XCNT"].ToString(), 1);
                        map.ShotArrayY = DACrux.Base.Convert.intParse(stepRow["ST_YCNT"].ToString(), 1);
                        map.ShotStartX = DACrux.Base.Convert.intParse(stepRow["ST_START_X"].ToString(), 1);
                        map.ShotStartY = DACrux.Base.Convert.intParse(stepRow["ST_START_Y"].ToString(), 1);
                    }

                    TxtDeviceID.Text = dsDefect.Tables["SETUP_INFO"].Rows[0]["SETUP_ID"].ToString();
                }


                TxtLotID.Text = dsDefect.Tables["STEP_INFO"].Rows[0]["LOT_ID"].ToString();
                TxtWaferID.Text = dsDefect.Tables["STEP_INFO"].Rows[0]["WAFER_ID"].ToString();
                TxtStepID.Text = dsDefect.Tables["STEP_INFO"].Rows[0]["STEP_ID"].ToString();

                //=================================================================================================================================
                //Setup Map 정보를 가져 온다.
                //=================================================================================================================================
                map.DieClear();

                if (chkNoneMap.Checked == false)
                {
                    if (dsDefect.Tables.IndexOf("SETUP_MAP") >= 0)
                    {
                        foreach (DataRow dr in dsDefect.Tables["SETUP_MAP"].Rows)
                        {
                            map.AddDie(new DACrux.Base.Die(
                                DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString()),
                                DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString()),
                                DACrux.Base.Convert.intParse(dr["TEST"].ToString()),
                                1
                                ));
                        }
                    }
                }

                // Shot Start Index 재조정 2019.12.26 Taihi,Kim.
                DefectMapDraw.ReadjustShotStartIndex(map);

                //=================================================================================================================================
                //Defect 정보를 가져 온다.
                //=================================================================================================================================
                
                map.DefectClear();

                if (dsDefect.Tables.IndexOf("DEFECT_INFO") >= 0)
                {
                    foreach (DataRow dr in dsDefect.Tables["DEFECT_INFO"].Rows)
                    {
                        DACrux.Base.Defect df = new DACrux.Base.Defect();
                        df.STEP_SEQ = DACrux.Base.Convert.intParse(dr["STEP_SEQ"].ToString());
                        df.DEFECTID = DACrux.Base.Convert.intParse(dr["DEFECTID"].ToString());
                        df.WAFER_SEQ = DACrux.Base.Convert.intParse(dr["WAFER_SEQ"].ToString());
                        df.X = DACrux.Base.Convert.doubleParse(dr["X"].ToString());
                        df.Y = DACrux.Base.Convert.doubleParse(dr["Y"].ToString());
                        df.XREL = DACrux.Base.Convert.doubleParse(dr["XREL"].ToString());
                        df.YREL = DACrux.Base.Convert.doubleParse(dr["YREL"].ToString());
                        df.XINDEX = DACrux.Base.Convert.intParse(dr["XINDEX"].ToString());
                        df.YINDEX = DACrux.Base.Convert.intParse(dr["YINDEX"].ToString());
                        df.XSIZE = DACrux.Base.Convert.doubleParse(dr["XSIZE"].ToString());
                        df.YSIZE = DACrux.Base.Convert.doubleParse(dr["YSIZE"].ToString());
                        df.DEFECTAREA = DACrux.Base.Convert.doubleParse(dr["DEFECTAREA"].ToString());
                        df.DSIZE = DACrux.Base.Convert.doubleParse(dr["DSIZE"].ToString());
                        df.CLASSNUMBER = DACrux.Base.Convert.intParse(dr["CLASSNUMBER"].ToString());
                        df.TEST = DACrux.Base.Convert.intParse(dr["TEST"].ToString());
                        df.CLUSTERNUMBER = DACrux.Base.Convert.intParse(dr["CLUSTERNUMBER"].ToString());
                        df.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(dr["ROUGHBINNUMBER"].ToString());
                        df.FINEBINNUMBER = DACrux.Base.Convert.intParse(dr["FINEBINNUMBER"].ToString());
                        df.REVIEWSAMPLE = DACrux.Base.Convert.intParse(dr["REVIEWSAMPLE"].ToString());
                        df.IMAGECOUNT = DACrux.Base.Convert.intParse(dr["IMAGECOUNT"].ToString());
                        df.ADDER = DACrux.Base.Convert.intParse(dr["ADDER"].ToString());
                        df.FIRST_STEP = dr["FIRST_STEP"].ToString();
                        df.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["RETICLE_REPEAT_ID"].ToString());
                        df.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["DIE_REPEAT_ID"].ToString());
                        df.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(dr["MAN_OPT_CLASS"].ToString());
                        df.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_OPT_CLASS"].ToString());
                        df.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(dr["MAN_SEM_CLASS"].ToString());
                        df.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_SEM_CLASS"].ToString());

                        //try catch 로 하면 속도가 너무 느리다.. 
                        if (dr.Table.Columns.IndexOf("REPEAT_XREL") > -1)
                            df.REPEAT_XREL = DACrux.Base.Convert.intParse(dr["REPEAT_XREL"].ToString());

                        if (dr.Table.Columns.IndexOf("REPEAT_YREL") > -1)
                            df.REPEAT_YREL = DACrux.Base.Convert.intParse(dr["REPEAT_YREL"].ToString());

                        if (dr.Table.Columns.IndexOf("IMAGE_PATH") > -1 && string.IsNullOrEmpty(dr["IMAGE_PATH"].ToString()) == false)
                        {
                            df.IMAGEURL = string.Format("{0}/{1}", dr["IMAGE_PATH"].ToString().Trim(), dr["IMAGE_FILENAME"].ToString().Trim()).Trim();
                        }

                        map.AddDefect(df);
                    }

                    dsDefect.Tables["DEFECT_INFO"].Columns["WAFER_SEQ"].SetOrdinal(dsDefect.Tables["DEFECT_INFO"].Columns.Count - 1);
                    dsDefect.Tables["DEFECT_INFO"].Columns["STEP_SEQ"].SetOrdinal(dsDefect.Tables["DEFECT_INFO"].Columns.Count - 1);
                }

                map.SetInfomation(DefectMapDraw.GetMapDescription(map.Defects));

                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                DataTable dtMapOption = oComConfig.SelectDefectMapConfig(DACrux.Base.GlobalVariable.Factory, DACrux.Base.GlobalVariable.UserID);
                if (dtMapOption != null && dtMapOption.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMapOption.Rows)
                    {
                        string strType = dr["NAME"].ToString();
                        Color crType = ColorTranslator.FromHtml(dr["VALUE"].ToString());

                        switch (strType)
                        {
                            //Wafer Base Color
                            case "WAFER_MAP_BG":
                                map.WaferColor = crType;
                                break;
                            //Inspection Die Color
                            case "WAFER_MAP_INP":
                                map.DieBackgroundColor = crType;
                                break;
                            //Inspection Defect Die Color
                            case "WAFER_MAP_DEFECT":
                                //Defect 의 Index 는 bare Map 과 맞지 않아 헷갈릴수 있어 주석 처리.
                                //map.DieDefectColor = crType;
                                break;
                            //Wafer Border Line Color
                            case "WAFER_MAP_LINE":
                                map.DieBorderColor = crType;
                                break;

                        }
                    }
                }

                DataTable dt = oComConfig.GetConfigurationUser(
                    DACrux.Base.GlobalVariable.Factory,
                    "WAFER_OPTION",
                    DACrux.Base.GlobalVariable.UserID
                    );
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> sWaferInfo = new List<string>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dsDefect.Tables["STEP_INFO"].Columns.Contains(dr["NAME"].ToString()))
                        {
                            DataTable dtList = dsDefect.Tables["STEP_INFO"].DefaultView.ToTable(true, dr["NAME"].ToString());
                            List<string> lsList = new List<string>();

                            foreach (DataRow drList in dtList.Rows)
                            {
                                lsList.Add(drList[0].ToString());
                            }

                            if (lsList.Count > 0)
                                sWaferInfo.Add(string.Format("{0}: {1}", dr["VALUE"], string.Join(",", lsList.ToArray())));
                        }
                    }
                    map.SetInfomation(sWaferInfo.ToArray());
                }

                if (dsDefect.Tables.Contains("STEP_INFO") && dsDefect.Tables["STEP_INFO"].Rows.Count > 0 && dsDefect.Tables["STEP_INFO"].Columns.Contains("WAFER_ID"))
                {
                    map.WaferID = dsDefect.Tables["STEP_INFO"].Rows[0]["WAFER_ID"].ToString();
                }

                //Wafer Information 사용 여부
                dt = oComConfig.GetConfigurationUser(
                   DACrux.Base.GlobalVariable.Factory,
                   "WAFER_OPTION_ENABLE",
                   DACrux.Base.GlobalVariable.UserID
                   );

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["VALUE"].ToString() == "N")
                        map.SetInfomation(null);
                }


                //=================================================================================================================================
                //Data 기준으로 Map 을 Draw 한다. 
                //=================================================================================================================================
                map.WaferDrawMode = DACrux.Map.MapMode.Fit;

                //Map 선택이 Defect Select 가 최우선 순위로 바꿔 준다.
                map.DefectSelectMode();

                // Change Column Order

                FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
                numberCellType.DecimalPlaces = 0;

                ToolTotalDie.Text = string.Format("Total Die Count : {0}", map.Dies == null ? 0 : map.Dies.Count);
                ToolDefectCount.Text = string.Format("Total Defect Count : {0}", map.Defects == null ? 0 : map.Defects.Count);

            }
            finally
            {
                StatusMessage(null);
            }
        }

        void DefectColorSet()
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            oDMapAnalysis = new DACrux.SEMDMS.RO.DefectMapAnalysis();
            try
            {
                this.dtSizeColor = oDMapAnalysis.SelectColorByDefectSize(DACrux.Base.GlobalVariable.UserID);
                map.SizeColor = dtSizeColor;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                this.dtTypeColor = oDMapAnalysis.GetColorByDefectType();
                map.TypeColor = dtTypeColor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void NoDefectColoring()
        {
            try
            {
                Color c = Color.Red;
                if (this.dtNoColoring == null)
                {
                    this.dtNoColoring = new DataTable();
                    dtNoColoring.Columns.Add("DEFECT", System.Type.GetType("System.String"));
                    dtNoColoring.Columns.Add("COLOR", System.Type.GetType("System.String"));
                    string strC = string.Format("{0}{1}{2}"
                        , string.Format("{0}", c.R).PadLeft(3, '0')
                        , string.Format("{0}", c.G).PadLeft(3, '0')
                        , string.Format("{0}", c.B).PadLeft(3, '0')
                        );
                    dtNoColoring.Rows.Add(new object[] { "ALL", strC });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion[ Method ]

        #region [ Property ]

        public DACrux.Base.DPWafer[] Wafer
        {
            get;
            set;
        }

        public void Initialize()
        {

        }

        public void SetData(DACrux.Base.DPWafer[] wafer)
        {
            this.Wafer = wafer;
            lsDeviceList.ClearSelected();
        }

        public Base.Defect[] GetSelectedDefect()
        {
            if (map.SelectedDefect.Count > 0)
                return map.SelectedDefect.ToArray();
            else
                return map.GetVisibleDefect().ToArray();
        }
        #endregion [ Property ]


        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(map);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Map";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add();
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Defect Chart n Images";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.SheetName = "DataRow";

            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "DataRow";

            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion

    }
}
