using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Common.RO;
using DACrux.Map;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// frmDefecMapAnalysis에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCDefecMap : System.Windows.Forms.UserControl, DACrux.Framework.Base.ISendDefect
    {
        DACrux.Base.DPWafer[] wafer = null;
        int RK = 1;

        private DACrux.Map.DefectMap map;
        private Panel MapPanel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ToolWaferDieLocation;
        private ToolStripStatusLabel ToolDefectCount;
        private ToolStripStatusLabel ToolParticles;
        private ToolStripStatusLabel ToolTotalDie;
        private DPUCRDSetup rdSetup;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView dgSelectDefects;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private System.ComponentModel.IContainer components;

        #region 생성자 및 초기화 코드

        public DPUCDefecMap()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();
            //
            // TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
            //
        }

        private void DPUCDefecMap_Load(object sender, System.EventArgs e)
        {
            map.DefectSize = GetDefaultDefectSize();
            DefectColorSet();
        }

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();

            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : DefectMap.DEFAULT_DEFECT_SIZE;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPUCDefecMap));
            this.MapPanel = new System.Windows.Forms.Panel();
            this.map = new DACrux.Map.DefectMap();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolWaferDieLocation = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolParticles = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolTotalDie = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolDefectCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rdSetup = new DACrux.SEMDMS.Control.DPUCRDSetup();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgSelectDefects = new System.Windows.Forms.DataGridView();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.MapPanel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSelectDefects)).BeginInit();
            this.SuspendLayout();
            // 
            // MapPanel
            // 
            this.MapPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapPanel.Controls.Add(this.map);
            this.MapPanel.Controls.Add(this.statusStrip1);
            this.MapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapPanel.Location = new System.Drawing.Point(0, 0);
            this.MapPanel.Name = "MapPanel";
            this.MapPanel.Size = new System.Drawing.Size(692, 600);
            this.MapPanel.TabIndex = 15;
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
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
            this.map.ShotLineWidth = 2;
            this.map.Size = new System.Drawing.Size(690, 574);
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
            this.map.OnSelectedDefect += new DACrux.Map.SelectedDefect(this.map_OnSelectedDefect);
            this.map.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.map_OnChangeCurrentDie);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolWaferDieLocation,
            this.ToolParticles,
            this.ToolTotalDie,
            this.ToolDefectCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 574);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(690, 24);
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
            // ToolParticles
            // 
            this.ToolParticles.AutoSize = false;
            this.ToolParticles.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolParticles.Name = "ToolParticles";
            this.ToolParticles.Size = new System.Drawing.Size(80, 19);
            this.ToolParticles.Text = "Particles : 0";
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(698, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(315, 600);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.rdSetup);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(307, 574);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "RD";
            // 
            // rdSetup
            // 
            this.rdSetup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rdSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdSetup.Location = new System.Drawing.Point(3, 3);
            this.rdSetup.Name = "rdSetup";
            this.rdSetup.ShotArrayX = 1;
            this.rdSetup.ShotArrayY = 1;
            this.rdSetup.ShotStartX = 1;
            this.rdSetup.ShotStartY = 1;
            this.rdSetup.Size = new System.Drawing.Size(301, 568);
            this.rdSetup.TabIndex = 17;
            this.rdSetup.TargetControl = this.map;
            this.rdSetup.ApplyButtonClick += new System.EventHandler(this.rdSetup_ApplyButtonClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgSelectDefects);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(307, 574);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SelectDefect";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgSelectDefects
            // 
            this.dgSelectDefects.AllowUserToAddRows = false;
            this.dgSelectDefects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSelectDefects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSelectDefects.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgSelectDefects.Location = new System.Drawing.Point(3, 3);
            this.dgSelectDefects.Name = "dgSelectDefects";
            this.dgSelectDefects.RowTemplate.Height = 23;
            this.dgSelectDefects.Size = new System.Drawing.Size(301, 568);
            this.dgSelectDefects.TabIndex = 0;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraSplitter1.Location = new System.Drawing.Point(692, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 315;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 600);
            this.ultraSplitter1.TabIndex = 16;
            // 
            // DPUCDefecMap
            // 
            this.Controls.Add(this.MapPanel);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.tabControl1);
            this.Name = "DPUCDefecMap";
            this.Size = new System.Drawing.Size(1013, 600);
            this.Load += new System.EventHandler(this.DPUCDefecMap_Load);
            this.MapPanel.ResumeLayout(false);
            this.MapPanel.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSelectDefects)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        private void map_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            ToolWaferDieLocation.Text = string.Format("X : {0} Y : {1}", NewDie.IndexX, NewDie.IndexY);
            ToolParticles.Text = string.Format("Particles : {0}", map.GetDefectCount(NewDie));
        }

        public void Draw()
        {
            if (this.wafer == null) return;

            long[] lStepSeq = null;

            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;

            this.Cursor = Cursors.WaitCursor;

            try
            {
                DataSet dsDefect = new DataSet();

                oDMapAnalysis = new RO.DefectMapAnalysis();

                lStepSeq = new long[this.wafer.Length];

                for (int iw = 0; iw < this.wafer.Length; iw++)
                {
                    lStepSeq[iw] = DACrux.Base.Convert.longParse(this.wafer[iw].StepSeq);
                }

                //=================================================================================================================================
                //Setup Seq 정보를 가져 온다.
                //=================================================================================================================================
                dsDefect = oDMapAnalysis.GetDefectMapViewer_Info(lStepSeq);
                if (dsDefect.Tables.IndexOf("SETUP_INFO") < 0)
                    throw new Exception("정의된 Setup 정보가 없습니다. TQD_SETUP Empty");

                map.WaferSize = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["WAFER_SIZE"].ToString());
                map.NotchType = DACrux.Base.Notch.Notch; //dsDefect.Tables["SETUP_INFO"].Rows[0]["NOTCH_TYPE"].ToString().Equals("F") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;
                map.AngleOffSet = 0;
                map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                map.NotchAngle = 0;// DOWN으로 저장하여 보여주므로 0으로 설정 DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ANGLE"].ToString());

                map.DieSizeX = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_X"].ToString());
                map.DieSizeY = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_Y"].ToString());
                map.OriginIndexX = DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_X"].ToString());
                map.OriginIndexY = DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_Y"].ToString());
                map.OriginX = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_X"].ToString());
                map.OriginY = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_Y"].ToString());

                // SHOT 정보 설정
                if (dsDefect.Tables.Contains("STEP_INFO") && dsDefect.Tables["STEP_INFO"].Rows.Count > 0 && dsDefect.Tables["STEP_INFO"].Columns.Contains("ST_XCNT"))
                {
                    DataRow stepRow = dsDefect.Tables["STEP_INFO"].Rows[0];
                    rdSetup.ShotArrayX = map.ShotArrayX = DACrux.Base.Convert.intParse(stepRow["ST_XCNT"].ToString(), 1);
                    rdSetup.ShotArrayY = map.ShotArrayY = DACrux.Base.Convert.intParse(stepRow["ST_YCNT"].ToString(), 1);
                    rdSetup.ShotStartX = map.ShotStartX = DACrux.Base.Convert.intParse(stepRow["ST_START_X"].ToString(), 1);
                    rdSetup.ShotStartY = map.ShotStartY = DACrux.Base.Convert.intParse(stepRow["ST_START_Y"].ToString(), 1);
                }

                if (dsDefect.Tables.Contains("STEP_INFO") && dsDefect.Tables["STEP_INFO"].Rows.Count > 0 && dsDefect.Tables["STEP_INFO"].Columns.Contains("WAFER_ID"))
                {
                    map.WaferID = dsDefect.Tables["STEP_INFO"].Rows[0]["WAFER_ID"].ToString();
                }

                map.DrawDefects = "ALL";


                //=================================================================================================================================
                //Setup Map 정보를 가져 온다.
                //=================================================================================================================================
                if (dsDefect.Tables.IndexOf("SETUP_MAP") < 0)
                    throw new Exception("정의된 Setup 정보가 없습니다. TQD_SETUP Empty");

                map.DieClear();

                foreach (DataRow dr in dsDefect.Tables["SETUP_MAP"].Rows)
                {
                    map.AddDie(new DACrux.Base.Die(DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString())
                        , DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString())
                        , DACrux.Base.Convert.intParse(dr["TEST"].ToString())
                        , 1));
                }

                // Shot Start Index 재조정 2019.12.26 Taihi,Kim.
                DefectMapDraw.ReadjustShotStartIndex(map);

                //=================================================================================================================================
                //Defect 정보를 가져 온다.
                //=================================================================================================================================

                map.DefectClear();

                DataTable defectDt = oDMapAnalysis.GetDefectMapViewer_Defects(lStepSeq);

                foreach (DataRow dr in defectDt.Rows)
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
                    df.IMAGECOUNT = DACrux.Base.Convert.intParse(dr["IMAGECOUNT"].ToString());
                    df.ADDER = DACrux.Base.Convert.intParse(dr["ADDER"].ToString());
                    //df.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(dr["ROUGHBINNUMBER"].ToString());
                    //df.FINEBINNUMBER = DACrux.Base.Convert.intParse(dr["FINEBINNUMBER"].ToString());
                    //df.REVIEWSAMPLE = DACrux.Base.Convert.intParse(dr["REVIEWSAMPLE"].ToString());
                    //df.FIRST_STEP = dr["FIRST_STEP"].ToString();
                    //df.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["RETICLE_REPEAT_ID"].ToString());
                    //df.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["DIE_REPEAT_ID"].ToString());
                    //df.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(dr["MAN_OPT_CLASS"].ToString());
                    //df.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_OPT_CLASS"].ToString());
                    //df.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(dr["MAN_SEM_CLASS"].ToString());
                    //df.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_SEM_CLASS"].ToString());

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

                map.SetInfomation(DefectMapDraw.GetMapDescription(map.Defects));

                //=================================================================================================================================
                //Data 기준으로 Map 을 Draw 한다. 
                //=================================================================================================================================
                map.WaferDrawMode = DACrux.Map.MapMode.Fit;

                //Map 선택이 Defect Select 가 최우선 순위로 바꿔 준다.
                map.DefectSelectMode();

                // Change Column Order
                dsDefect.Tables["DEFECT_INFO"].Columns["WAFER_SEQ"].SetOrdinal(dsDefect.Tables["DEFECT_INFO"].Columns.Count - 1);
                dsDefect.Tables["DEFECT_INFO"].Columns["STEP_SEQ"].SetOrdinal(dsDefect.Tables["DEFECT_INFO"].Columns.Count - 1);
                FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
                numberCellType.DecimalPlaces = 0;

                ToolTotalDie.Text = string.Format("Total Die Count : {0}", map.Dies.Count);
                ToolDefectCount.Text = string.Format("Total Defect Count : {0}", map.Defects.Count);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public void Draw(DefectList defectList)
        {
            this.Cursor = Cursors.WaitCursor;

            ComConfiguration oComConfig = null;
            try
            {
                oComConfig = new ComConfiguration();
                long[] lStepSeq = defectList.GetStepSeqArray();
                
                //=================================================================================================================================
                //Setup Seq 정보를 가져 온다.
                //=================================================================================================================================

                // 첫번째 Map 기준정보로 설정
                DmsWaferDieInfo info = DmsCache.Instance[lStepSeq[0]];

                //map.WaferSize = (double)info.StepInfo.SampleSize * 1000 * 1000; // mm -> nm
                map.WaferSize = (double)info.StepInfo.WaferSize; // Wafer Size 를 um 로 변경
                map.NotchType = DACrux.Base.Notch.Notch;
                map.AngleOffSet = 0;
                map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                map.NotchAngle = 0;

                map.DieSizeX = info.StepInfo.DiePitchX;
                map.DieSizeY = info.StepInfo.DiePitchY;
                map.OriginIndexX = (int)info.StepInfo.DieOriginX;
                map.OriginIndexY = (int)info.StepInfo.DieOriginY;
                map.OriginX = info.StepInfo.SampleCenterLocationX;
                map.OriginY = info.StepInfo.SampleCenterLocationY;

                //map.ShotArrayX = rdSetup.ShotArrayX = info.StepInfo.ShotArrayX;
                //map.ShotArrayY = rdSetup.ShotArrayY = info.StepInfo.ShotArrayY;
                //map.ShotStartX = rdSetup.ShotStartX = info.StepInfo.ShotStartX;
                //map.ShotStartY = rdSetup.ShotStartY = info.StepInfo.ShotStartY;

                map.DrawDefects = "ALL";

                map.DieClear();

                //Virture Die 에 대한 Information Set
                DataTable dtVir = oComConfig.GetConfigUser(
                  DACrux.Base.GlobalVariable.Factory,
                  "VIRTUAL_OPTION",
                  DACrux.Base.GlobalVariable.UserID
                  );

                if (dtVir != null && dtVir.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVir.Rows)
                    {
                        if (dr["NAME"].ToString() == "COLOR")
                        {
                            map.VirtualDieColor = System.Drawing.ColorTranslator.FromHtml(dr["VALUE"].ToString());
                        }

                        if (dr["NAME"].ToString() == "VISIBLE")
                        {
                            if (dr["VALUE"].ToString() == "Y")
                                WaferMap.AppendVirtualDie(map);
                        }
                    }
                }

                foreach (Point pt in info.Dies)
                {
                    map.AddDie(new Die(pt.X, pt.Y, 1, 1));
                }

                map.DefectClear();

                foreach (Defect d in defectList)
                {
                    map.AddDefect(d);
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
                                map.DieDefectColor = crType;
                                break;
                            //Wafer Border Line Color
                            case "WAFER_MAP_LINE":
                                map.DieBorderColor = crType;
                                break;

                        }
                    }
                }

                map.WaferDrawMode = DACrux.Map.MapMode.Fit;

                //Map 선택이 Defect Select 가 최우선 순위로 바꿔 준다.
                map.DefectSelectMode();

                ToolTotalDie.Text = string.Format("Total Die Count : {0}", info.Dies.Length);
                ToolDefectCount.Text = string.Format("Total Defect Count : {0}", defectList.Count);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        void DefectColorSet()
        {
            RO.DefectMapAnalysis oDMapAnalysis = new RO.DefectMapAnalysis();
            map.TypeColor = oDMapAnalysis.GetColorByDefectType();
        }

        public void SetData(DACrux.Base.DPWafer[] wafer)
        {
            this.wafer = wafer;
        }

        public Base.Defect[] GetSelectedDefect()
        {
            if (map.SelectedDefect.Count > 0)
                return map.SelectedDefect.ToArray();
            else
                return map.GetVisibleDefect().ToArray();
        }

        private void rdSetup_ApplyButtonClick(object sender, EventArgs e)
        {
            long[] stepSeqArr = map.Defects.GetStepSeqArray();

            if (stepSeqArr == null || stepSeqArr.Length == 0)
                return;

            map.ShotArrayX = rdSetup.ShotArrayX;
            map.ShotArrayY = rdSetup.ShotArrayY;
            map.ShotStartX = rdSetup.ShotStartX;
            map.ShotStartY = rdSetup.ShotStartY;

            RepeatedDefect rd = new RepeatedDefect();
            rd.Tolerance = rdSetup.Tolerance;
            rd.RepeatCount = rdSetup.RepeatCount;
            rd.ShotStartX = rdSetup.ShotStartX;
            rd.ShotStartY = rdSetup.ShotStartY;
            rd.ShotArrayX = rdSetup.ShotArrayX;
            rd.ShotArrayY = rdSetup.ShotArrayY;
            rd.DieXSize = map.DieSizeX;
            rd.DieYSize = map.DieSizeY;
            rd.CalcBy = rdSetup.IsPointMode ? CalcBy.Point : CalcBy.Size;
            rd.Calculate(DmsCache.Instance[stepSeqArr[0]].Dies, map.Defects);

            map.MapType = MAP_TYPE.RD;
            map.Redraw();

            int rdCount = map.Defects.GetRDCount();
            int nonRd = map.Defects.Count - rdCount;

            rdSetup.SetChartData(nonRd, rdCount);
        }

        public DefectMap DefectMap
        {
            get { return map; }
        }

        private void map_OnSelectedDefect(object sender, Defect[] oDefect)
        {
            DataTable dtDefectList = ConvertDefects(oDefect);

            dgSelectDefects.DataSource = null;

            if (dtDefectList != null && dtDefectList.Rows.Count > 0)
            {
                dgSelectDefects.DataSource = dtDefectList;
            }

            dgSelectDefects.Refresh();
        }

        private DataTable ConvertDefects(Defect[] oDefect)
        {
            if (oDefect == null || oDefect.Length <= 0)
                return null;

            DataTable dt = null;

            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("DEFECTID", typeof(int)));
                dt.Columns.Add(new DataColumn("CLASSNUMBER", typeof(int)));
                dt.Columns.Add(new DataColumn("CLUSTERNUMBER", typeof(int)));
                dt.Columns.Add(new DataColumn("XINDEX", typeof(int)));
                dt.Columns.Add(new DataColumn("YINDEX", typeof(int)));
                dt.Columns.Add(new DataColumn("XREL", typeof(double)));
                dt.Columns.Add(new DataColumn("YREL", typeof(double)));
                dt.Columns.Add(new DataColumn("X", typeof(double)));
                dt.Columns.Add(new DataColumn("Y", typeof(double)));
                dt.Columns.Add(new DataColumn("XSIZE", typeof(double)));
                dt.Columns.Add(new DataColumn("YSIZE", typeof(double)));
                dt.Columns.Add(new DataColumn("IMAGECOUNT", typeof(double)));
                dt.AcceptChanges();

                foreach(Defect df in oDefect)
                {
                    DataRow dr = dt.NewRow();
                    dr["DEFECTID"] = df.DEFECTID;
                    dr["CLASSNUMBER"] = df.CLASSNUMBER;
                    dr["CLUSTERNUMBER"] = df.CLUSTERNUMBER;
                    dr["XINDEX"] = df.XINDEX;
                    dr["YINDEX"] = df.YINDEX;
                    dr["XREL"] = df.XREL;
                    dr["YREL"] = df.YREL;
                    dr["X"] = df.X;
                    dr["Y"] = df.Y;
                    dr["XSIZE"] = df.XSIZE;
                    dr["YSIZE"] = df.YSIZE;
                    dr["IMAGECOUNT"] = df.IMAGECOUNT;
                    dt.Rows.Add(dr);
                }

                dt.AcceptChanges();
                return dt;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();

                dt = null;
            }

        }
    }
}
