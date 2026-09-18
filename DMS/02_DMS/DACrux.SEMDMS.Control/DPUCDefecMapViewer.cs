using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DACrux.Common.RO;
using DACrux.Map;
using DACrux.Base;
using DACrux.SEMDMS.RO;
using DACrux.Framework.Base;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// frmDefecMapAnalysis에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCDefecMapViewer : DACruxCTLBasic01, ISendDefect, IExportExcel
    {
        #region 멤버 변수

        public const string Chart_DefectClass = "CLASSNUMBER";
        public const string Chart_InspectedWafer = "WAFER_ID";
        public const string Chart_StepID = "STEP_ID";
        public const string Chart_InspectionTime = "RESULTTIMESTAMP";
        public const string Chart_DefectSize = "DSIZE";
        public const string Chart_DefectType = "DEFECT_TYPE";
        public const string DATETIME_FORMAT = "yy-MM-dd HH:mm:ss";

        enum DEFECT_CNT { FROM, TO, COLOR }
        enum DEFECT_COLOR { CLASSNUMBER, NAME, COLOR }
        enum DEFECT_SIZE { NO, SIZE_FROM, SIZE_TO, COLOR, LABEL, DSIZE }
        enum ReclassifiedNewDefectColIndex { STEP_SEQ = 0, DEFECTID, WAFER_SEQ, CLASSNUMBER, NEW_DEFECT_CLASS }

        private string strConfigFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "DM", "DefectCount");
        private string strConfigInfo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "DM", "AppConfig.ini");

        DataTable dtSizeColor = null;

        DataSet dsDefect = null;
        //DataTable dtDefectData = null;
        DataTable dtDefectCnt = null;
        DataTable dtIndexDefectsGroup = null;
        DataTable dtChartdata = null;
        DataTable dtImageList = null;
        MAP_TYPE m_prevMapType;

        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDefecMapViewerUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDefecMapViewerUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDefecMapViewerUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDefecMapViewerUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.AutoHideControl _DPUCDefecMapViewerAutoHideControl;
        private System.Windows.Forms.Panel pnlChart;
        private System.Windows.Forms.Panel panelData;
        private DACrux.Map.DefectMap map;
        private FarPoint.Win.Spread.FpSpread fpSpreadColor;
        private FarPoint.Win.Spread.SheetView fpSpreadColor_Sheet;
        private FarPoint.Win.Spread.FpSpread fpSpreadDefect;
        private FarPoint.Win.Spread.SheetView fpSpreadDefect_Sheet;
        private Panel panel1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Panel MapPanel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolWaferDieLocation;
        private ToolStripStatusLabel toolSelectedDefect;
        private ToolStripStatusLabel toolParticles;
        private ToolStripStatusLabel toolTotalDie;
        private FlowLayoutPanel imageList;
        private GroupBox groupBox2;
        private RadioButton rbtYDescend;
        private RadioButton rbtYAscend;
        private GroupBox groupBox1;
        private RadioButton rbtXChrono;
        private RadioButton rbtXLabel;
        private Label label2;
        private Label label1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbYAxisItem;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbXAxisItem;
        private Chart ChartDefect;
        private Panel panel2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbChartType;
        private Label label3;
        private Button btnRedraw;
        private Panel plDensity;
        private Button BtnDefectCountReDraw;
        private DPUCRDSetup rdSetup;
        private CheckBox chkDefectCount;
        private DataGridView dgDefectCount;
        private CheckBox chkNormalize;
        private ContextMenuStrip contextMenuColor;
        private ToolStripMenuItem colorResetToolStripMenuItem;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Panel pnGridView;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem sizeResetToolStripMenuItem;
        private Panel pnlInformation;
        private DPUCInformation dpucInformation1;
        private CheckBox chkScaleBreaks;
        private Panel Reclassify;
        private DPUCReclassify dpucReclassify1;
        private ToolStripStatusLabel toolDefectStat;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea4;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow2;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow3;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow4;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow5;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow6;
        private System.ComponentModel.IContainer components;

        #endregion

        #region 생성자 및 Load 및 Closing 이벤트

        public DPUCDefecMapViewer()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

            //
            // TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
            //
            m_prevMapType = MAP_TYPE.CLASS;
        }

        private void DPUCDefecMapViewer_Load(object sender, System.EventArgs e)
        {
            if (DesignMode)
                return;

            UserConfiguration.SetDefectColor(map);

            fpSpreadColor_Sheet.DataSource = map.TypeColor;
            DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadColor_Sheet);
            fpSpreadColor_Sheet.Columns[(int)DEFECT_COLOR.CLASSNUMBER].Width = 20;

            fpSpreadColor_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode | FarPoint.Win.Spread.OperationMode.ReadOnly;
            fpSpreadDefect_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode | FarPoint.Win.Spread.OperationMode.ReadOnly;

            //Chart 관련 Data 입력
            cmbXAxisItem.Items.Clear();
            cmbXAxisItem.DataMember = "";
            cmbXAxisItem.DisplayMember = "VALUE";
            cmbXAxisItem.ValueMember = "KEY";
            cmbXAxisItem.DataSource = GetChartXAxis();
            cmbXAxisItem.SelectedIndex = 0;

            cmbYAxisItem.Items.Clear();
            cmbYAxisItem.DataMember = "";
            cmbYAxisItem.DisplayMember = "VALUE";
            cmbYAxisItem.ValueMember = "KEY";
            cmbYAxisItem.DataSource = GetChartYAxis();
            cmbYAxisItem.SelectedIndex = 0;

            cmbChartType.DataMember = "";
            cmbChartType.DisplayMember = "VALUE";
            cmbChartType.ValueMember = "KEY";
            cmbChartType.DataSource = GetChartType();
            cmbChartType.SelectedIndex = 0;

            FileInfo oConfigFile = new FileInfo(strConfigFullPath);
            if (oConfigFile.Exists == true)
            {
                DataSet dsFile = new DataSet();
                dsFile.ReadXml(oConfigFile.FullName);
                if (dsFile.Tables.Count > 0)
                {
                    dtDefectCnt = dsFile.Tables[0];
                    SetDefectCountColor();
                }
            }
            else
            {
                dtDefectCnt = GetInitDefectConfig();
                SetDefectCountColor();
            }

            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(strConfigInfo)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(strConfigInfo));

            if (ParentForm != null)
                ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDefectCountColor();
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

        #endregion

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPUCDefecMapViewer));
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("bab611f7-b2c4-4891-b38a-d94ef53725f8"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("f6133f07-1a87-4377-b2b8-0f8c6a5b0094"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("bab611f7-b2c4-4891-b38a-d94ef53725f8"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane2 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("1d2371d6-307a-4457-9059-c6efcb9d3977"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("bab611f7-b2c4-4891-b38a-d94ef53725f8"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane3 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("28a733a5-1e5a-44b6-a974-7d56045dd157"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("bab611f7-b2c4-4891-b38a-d94ef53725f8"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane4 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("03aa1a84-3893-4912-a276-96b4f336c5d2"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("bab611f7-b2c4-4891-b38a-d94ef53725f8"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane5 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("6dccf462-d001-470d-81b4-73f5634bde94"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("bab611f7-b2c4-4891-b38a-d94ef53725f8"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane6 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("28a733a5-1e5a-44b6-a974-7d56045dd158"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("bab611f7-b2c4-4891-b38a-d94ef53725f8"), -1);
            this.pnlChart = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ChartDefect = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sizeResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRedraw = new System.Windows.Forms.Button();
            this.chkScaleBreaks = new System.Windows.Forms.CheckBox();
            this.chkNormalize = new System.Windows.Forms.CheckBox();
            this.cmbChartType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbXAxisItem = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.fpSpreadColor = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadColor_Sheet = new FarPoint.Win.Spread.SheetView();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.plDensity = new System.Windows.Forms.Panel();
            this.pnGridView = new System.Windows.Forms.Panel();
            this.dgDefectCount = new System.Windows.Forms.DataGridView();
            this.contextMenuColor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.colorResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnDefectCountReDraw = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtYDescend = new System.Windows.Forms.RadioButton();
            this.rbtYAscend = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtXChrono = new System.Windows.Forms.RadioButton();
            this.rbtXLabel = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYAxisItem = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.chkDefectCount = new System.Windows.Forms.CheckBox();
            this.imageList = new System.Windows.Forms.FlowLayoutPanel();
            this.panelData = new System.Windows.Forms.Panel();
            this.fpSpreadDefect = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadDefect_Sheet = new FarPoint.Win.Spread.SheetView();
            this.pnlInformation = new System.Windows.Forms.Panel();
            this.dpucInformation1 = new DACrux.SEMDMS.Control.DPUCInformation();
            this.Reclassify = new System.Windows.Forms.Panel();
            this.dpucReclassify1 = new DACrux.SEMDMS.Control.DPUCReclassify();
            this.rdSetup = new DACrux.SEMDMS.Control.DPUCRDSetup();
            this.map = new DACrux.Map.DefectMap();
            this.ultraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._DPUCDefecMapViewerUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDefecMapViewerUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDefecMapViewerUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDefecMapViewerUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDefecMapViewerAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.MapPanel = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolWaferDieLocation = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolParticles = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTotalDie = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolSelectedDefect = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolDefectStat = new System.Windows.Forms.ToolStripStatusLabel();
            this.windowDockingArea4 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow2 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow3 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow4 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow5 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow6 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.pnlChart.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChartDefect)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbChartType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbXAxisItem)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadColor_Sheet)).BeginInit();
            this.plDensity.SuspendLayout();
            this.pnGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDefectCount)).BeginInit();
            this.contextMenuColor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbYAxisItem)).BeginInit();
            this.panelData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect_Sheet)).BeginInit();
            this.pnlInformation.SuspendLayout();
            this.Reclassify.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).BeginInit();
            this.MapPanel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.windowDockingArea4.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            this.dockableWindow2.SuspendLayout();
            this.dockableWindow3.SuspendLayout();
            this.dockableWindow4.SuspendLayout();
            this.dockableWindow5.SuspendLayout();
            this.dockableWindow6.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlChart
            // 
            this.pnlChart.Controls.Add(this.tabControl1);
            this.pnlChart.Controls.Add(this.ultraSplitter1);
            this.pnlChart.Controls.Add(this.plDensity);
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Location = new System.Drawing.Point(0, 20);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(327, 558);
            this.pnlChart.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 223);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(327, 335);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ChartDefect);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(319, 309);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Chart";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ChartDefect
            // 
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.ChartDefect.ChartAreas.Add(chartArea1);
            this.ChartDefect.ContextMenuStrip = this.contextMenuStrip;
            this.ChartDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.ChartDefect.Legends.Add(legend1);
            this.ChartDefect.Location = new System.Drawing.Point(3, 98);
            this.ChartDefect.Name = "ChartDefect";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.ChartDefect.Series.Add(series1);
            this.ChartDefect.Size = new System.Drawing.Size(313, 208);
            this.ChartDefect.TabIndex = 3;
            this.ChartDefect.Text = "chart1";
            this.ChartDefect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChartDefect_MouseClick);
            this.ChartDefect.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ChartDefect_MouseMove);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sizeResetToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(129, 26);
            // 
            // sizeResetToolStripMenuItem
            // 
            this.sizeResetToolStripMenuItem.Name = "sizeResetToolStripMenuItem";
            this.sizeResetToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.sizeResetToolStripMenuItem.Text = "Size Reset";
            this.sizeResetToolStripMenuItem.Click += new System.EventHandler(this.sizeResetToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnRedraw);
            this.panel2.Controls.Add(this.chkScaleBreaks);
            this.panel2.Controls.Add(this.chkNormalize);
            this.panel2.Controls.Add(this.cmbChartType);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.cmbXAxisItem);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(313, 95);
            this.panel2.TabIndex = 4;
            // 
            // btnRedraw
            // 
            this.btnRedraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedraw.Image = ((System.Drawing.Image)(resources.GetObject("btnRedraw.Image")));
            this.btnRedraw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRedraw.Location = new System.Drawing.Point(188, 55);
            this.btnRedraw.Name = "btnRedraw";
            this.btnRedraw.Size = new System.Drawing.Size(121, 31);
            this.btnRedraw.TabIndex = 9;
            this.btnRedraw.Text = "Redraw";
            this.btnRedraw.UseVisualStyleBackColor = true;
            this.btnRedraw.Click += new System.EventHandler(this.BtnRedraw_Click);
            // 
            // chkScaleBreaks
            // 
            this.chkScaleBreaks.Location = new System.Drawing.Point(8, 73);
            this.chkScaleBreaks.Name = "chkScaleBreaks";
            this.chkScaleBreaks.Size = new System.Drawing.Size(121, 21);
            this.chkScaleBreaks.TabIndex = 0;
            this.chkScaleBreaks.Text = "Scale Breaks";
            this.chkScaleBreaks.CheckedChanged += new System.EventHandler(this.chkScaleBreaks_CheckedChanged);
            // 
            // chkNormalize
            // 
            this.chkNormalize.Location = new System.Drawing.Point(8, 52);
            this.chkNormalize.Name = "chkNormalize";
            this.chkNormalize.Size = new System.Drawing.Size(91, 21);
            this.chkNormalize.TabIndex = 0;
            this.chkNormalize.Text = "Normalize";
            this.chkNormalize.CheckedChanged += new System.EventHandler(this.chkNormalize_CheckedChanged);
            // 
            // cmbChartType
            // 
            this.cmbChartType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbChartType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "ValueListItem0";
            valueListItem1.DisplayText = "Class";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "Size";
            this.cmbChartType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.cmbChartType.Location = new System.Drawing.Point(130, 28);
            this.cmbChartType.Name = "cmbChartType";
            this.cmbChartType.Size = new System.Drawing.Size(178, 19);
            this.cmbChartType.TabIndex = 7;
            this.cmbChartType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Chart Type";
            // 
            // cmbXAxisItem
            // 
            this.cmbXAxisItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbXAxisItem.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem4.DataValue = "ValueListItem0";
            valueListItem4.DisplayText = "Class";
            valueListItem5.DataValue = "ValueListItem1";
            valueListItem5.DisplayText = "Size";
            this.cmbXAxisItem.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,
            valueListItem5});
            this.cmbXAxisItem.Location = new System.Drawing.Point(130, 3);
            this.cmbXAxisItem.Name = "cmbXAxisItem";
            this.cmbXAxisItem.Size = new System.Drawing.Size(178, 19);
            this.cmbXAxisItem.TabIndex = 7;
            this.cmbXAxisItem.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbXAxisItem.ValueChanged += new System.EventHandler(this.cmbXAxisItem_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "X-Axis";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.fpSpreadColor);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(319, 309);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Defect Color";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // fpSpreadColor
            // 
            this.fpSpreadColor.AccessibleDescription = "";
            this.fpSpreadColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadColor.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadColor.Location = new System.Drawing.Point(3, 3);
            this.fpSpreadColor.Name = "fpSpreadColor";
            this.fpSpreadColor.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadColor.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadColor_Sheet});
            this.fpSpreadColor.Size = new System.Drawing.Size(313, 303);
            this.fpSpreadColor.TabIndex = 2;
            this.fpSpreadColor.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadColor.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpreadColor_CellDoubleClick);
            // 
            // fpSpreadColor_Sheet
            // 
            this.fpSpreadColor_Sheet.Reset();
            fpSpreadColor_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadColor_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadColor_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadColor_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadColor_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadColor_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.SheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSpreadColor_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 213);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 264;
            this.ultraSplitter1.Size = new System.Drawing.Size(327, 10);
            this.ultraSplitter1.TabIndex = 8;
            // 
            // plDensity
            // 
            this.plDensity.Controls.Add(this.pnGridView);
            this.plDensity.Controls.Add(this.panel1);
            this.plDensity.Dock = System.Windows.Forms.DockStyle.Top;
            this.plDensity.Location = new System.Drawing.Point(0, 0);
            this.plDensity.Name = "plDensity";
            this.plDensity.Size = new System.Drawing.Size(327, 213);
            this.plDensity.TabIndex = 7;
            // 
            // pnGridView
            // 
            this.pnGridView.Controls.Add(this.dgDefectCount);
            this.pnGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnGridView.Location = new System.Drawing.Point(0, 39);
            this.pnGridView.Name = "pnGridView";
            this.pnGridView.Size = new System.Drawing.Size(327, 174);
            this.pnGridView.TabIndex = 7;
            // 
            // dgDefectCount
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDefectCount.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDefectCount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDefectCount.ContextMenuStrip = this.contextMenuColor;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDefectCount.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgDefectCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDefectCount.Location = new System.Drawing.Point(0, 0);
            this.dgDefectCount.Name = "dgDefectCount";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDefectCount.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgDefectCount.RowTemplate.Height = 23;
            this.dgDefectCount.Size = new System.Drawing.Size(327, 174);
            this.dgDefectCount.TabIndex = 36;
            this.dgDefectCount.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDefectCount_CellDoubleClick);
            this.dgDefectCount.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDefectCount_CellEndEdit);
            // 
            // contextMenuColor
            // 
            this.contextMenuColor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorResetToolStripMenuItem});
            this.contextMenuColor.Name = "contextMenuColor";
            this.contextMenuColor.Size = new System.Drawing.Size(136, 26);
            // 
            // colorResetToolStripMenuItem
            // 
            this.colorResetToolStripMenuItem.Name = "colorResetToolStripMenuItem";
            this.colorResetToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.colorResetToolStripMenuItem.Text = "Color Reset";
            this.colorResetToolStripMenuItem.Click += new System.EventHandler(this.colorResetToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnDefectCountReDraw);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbYAxisItem);
            this.panel1.Controls.Add(this.chkDefectCount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 39);
            this.panel1.TabIndex = 6;
            // 
            // BtnDefectCountReDraw
            // 
            this.BtnDefectCountReDraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDefectCountReDraw.BackColor = System.Drawing.Color.White;
            this.BtnDefectCountReDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDefectCountReDraw.Image = ((System.Drawing.Image)(resources.GetObject("BtnDefectCountReDraw.Image")));
            this.BtnDefectCountReDraw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDefectCountReDraw.Location = new System.Drawing.Point(196, 3);
            this.BtnDefectCountReDraw.Name = "BtnDefectCountReDraw";
            this.BtnDefectCountReDraw.Size = new System.Drawing.Size(121, 31);
            this.BtnDefectCountReDraw.TabIndex = 9;
            this.BtnDefectCountReDraw.Text = "Redraw";
            this.BtnDefectCountReDraw.UseVisualStyleBackColor = false;
            this.BtnDefectCountReDraw.Click += new System.EventHandler(this.BtnDefectCountReDraw_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rbtYDescend);
            this.groupBox2.Controls.Add(this.rbtYAscend);
            this.groupBox2.Location = new System.Drawing.Point(18, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(316, 44);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "X 축 정렬";
            this.groupBox2.Visible = false;
            // 
            // rbtYDescend
            // 
            this.rbtYDescend.AutoSize = true;
            this.rbtYDescend.Location = new System.Drawing.Point(95, 21);
            this.rbtYDescend.Name = "rbtYDescend";
            this.rbtYDescend.Size = new System.Drawing.Size(90, 16);
            this.rbtYDescend.TabIndex = 0;
            this.rbtYDescend.Text = "Descending";
            this.rbtYDescend.UseVisualStyleBackColor = true;
            // 
            // rbtYAscend
            // 
            this.rbtYAscend.AutoSize = true;
            this.rbtYAscend.Checked = true;
            this.rbtYAscend.Location = new System.Drawing.Point(10, 21);
            this.rbtYAscend.Name = "rbtYAscend";
            this.rbtYAscend.Size = new System.Drawing.Size(83, 16);
            this.rbtYAscend.TabIndex = 0;
            this.rbtYAscend.TabStop = true;
            this.rbtYAscend.Text = "Ascending";
            this.rbtYAscend.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rbtXChrono);
            this.groupBox1.Controls.Add(this.rbtXLabel);
            this.groupBox1.Location = new System.Drawing.Point(18, 224);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 44);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X 축 정렬 기준";
            this.groupBox1.Visible = false;
            // 
            // rbtXChrono
            // 
            this.rbtXChrono.AutoSize = true;
            this.rbtXChrono.Location = new System.Drawing.Point(95, 21);
            this.rbtXChrono.Name = "rbtXChrono";
            this.rbtXChrono.Size = new System.Drawing.Size(113, 16);
            this.rbtXChrono.TabIndex = 0;
            this.rbtXChrono.Text = "X Chronological";
            this.rbtXChrono.UseVisualStyleBackColor = true;
            // 
            // rbtXLabel
            // 
            this.rbtXLabel.AutoSize = true;
            this.rbtXLabel.Checked = true;
            this.rbtXLabel.Location = new System.Drawing.Point(10, 21);
            this.rbtXLabel.Name = "rbtXLabel";
            this.rbtXLabel.Size = new System.Drawing.Size(66, 16);
            this.rbtXLabel.TabIndex = 0;
            this.rbtXLabel.TabStop = true;
            this.rbtXLabel.Text = "X Label";
            this.rbtXLabel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 312);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Y-Axis";
            this.label2.Visible = false;
            // 
            // cmbYAxisItem
            // 
            this.cmbYAxisItem.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Standard;
            valueListItem3.DataValue = "ValueListItem0";
            valueListItem3.DisplayText = "Defect Count";
            this.cmbYAxisItem.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3});
            this.cmbYAxisItem.Location = new System.Drawing.Point(83, 311);
            this.cmbYAxisItem.Name = "cmbYAxisItem";
            this.cmbYAxisItem.Size = new System.Drawing.Size(135, 19);
            this.cmbYAxisItem.TabIndex = 7;
            this.cmbYAxisItem.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbYAxisItem.Visible = false;
            // 
            // chkDefectCount
            // 
            this.chkDefectCount.Location = new System.Drawing.Point(7, 9);
            this.chkDefectCount.Name = "chkDefectCount";
            this.chkDefectCount.Size = new System.Drawing.Size(148, 21);
            this.chkDefectCount.TabIndex = 0;
            this.chkDefectCount.Text = "Defect Count Color";
            // 
            // imageList
            // 
            this.imageList.AutoScroll = true;
            this.imageList.AutoSize = true;
            this.imageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.imageList.Location = new System.Drawing.Point(0, 20);
            this.imageList.Name = "imageList";
            this.imageList.Size = new System.Drawing.Size(327, 558);
            this.imageList.TabIndex = 10;
            this.imageList.WrapContents = false;
            this.imageList.SizeChanged += new System.EventHandler(this.imageList_SizeChanged);
            this.imageList.MouseEnter += new System.EventHandler(this.ImageList_MouseEnter);
            // 
            // panelData
            // 
            this.panelData.Controls.Add(this.fpSpreadDefect);
            this.panelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelData.Location = new System.Drawing.Point(616, 128);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(200, 344);
            this.panelData.TabIndex = 7;
            // 
            // fpSpreadDefect
            // 
            this.fpSpreadDefect.AccessibleDescription = "";
            this.fpSpreadDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadDefect.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadDefect.Location = new System.Drawing.Point(0, 0);
            this.fpSpreadDefect.Name = "fpSpreadDefect";
            this.fpSpreadDefect.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadDefect.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadDefect_Sheet});
            this.fpSpreadDefect.Size = new System.Drawing.Size(200, 344);
            this.fpSpreadDefect.TabIndex = 0;
            this.fpSpreadDefect.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadDefect.SetActiveViewport(0, -1, -1);
            // 
            // fpSpreadDefect_Sheet
            // 
            this.fpSpreadDefect_Sheet.Reset();
            fpSpreadDefect_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadDefect_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpreadDefect_Sheet.ColumnCount = 0;
            fpSpreadDefect_Sheet.RowCount = 0;
            this.fpSpreadDefect_Sheet.ActiveColumnIndex = -1;
            this.fpSpreadDefect_Sheet.ActiveRowIndex = -1;
            this.fpSpreadDefect_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadDefect_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadDefect_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpreadDefect_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDefect_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSpreadDefect_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlInformation
            // 
            this.pnlInformation.Controls.Add(this.dpucInformation1);
            this.pnlInformation.Location = new System.Drawing.Point(0, 20);
            this.pnlInformation.Name = "pnlInformation";
            this.pnlInformation.Size = new System.Drawing.Size(327, 558);
            this.pnlInformation.TabIndex = 10;
            // 
            // dpucInformation1
            // 
            this.dpucInformation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucInformation1.Location = new System.Drawing.Point(0, 0);
            this.dpucInformation1.Name = "dpucInformation1";
            this.dpucInformation1.Size = new System.Drawing.Size(327, 558);
            this.dpucInformation1.TabIndex = 0;
            // 
            // Reclassify
            // 
            this.Reclassify.Controls.Add(this.dpucReclassify1);
            this.Reclassify.Location = new System.Drawing.Point(0, 20);
            this.Reclassify.Name = "Reclassify";
            this.Reclassify.Size = new System.Drawing.Size(327, 558);
            this.Reclassify.TabIndex = 10;
            // 
            // dpucReclassify1
            // 
            this.dpucReclassify1.DataSource = null;
            this.dpucReclassify1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpucReclassify1.IsDisplayApply = true;
            this.dpucReclassify1.Location = new System.Drawing.Point(0, 0);
            this.dpucReclassify1.Name = "dpucReclassify1";
            this.dpucReclassify1.Size = new System.Drawing.Size(327, 558);
            this.dpucReclassify1.TabIndex = 0;
            this.dpucReclassify1.OnReclassifyApply += new System.EventHandler(this.dpucReclassify1_OnReclassifyApply);
            // 
            // rdSetup
            // 
            this.rdSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdSetup.Location = new System.Drawing.Point(0, 20);
            this.rdSetup.Name = "rdSetup";
            this.rdSetup.ShotArrayX = 1;
            this.rdSetup.ShotArrayY = 1;
            this.rdSetup.ShotStartX = 1;
            this.rdSetup.ShotStartY = 1;
            this.rdSetup.Size = new System.Drawing.Size(327, 558);
            this.rdSetup.TabIndex = 17;
            this.rdSetup.TargetControl = this.map;
            this.rdSetup.ApplyButtonClick += new System.EventHandler(this.shot_ApplyButtonClick);
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
            this.map.Size = new System.Drawing.Size(747, 576);
            this.map.SizeColor = null;
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 9;
            this.map.ToGradationDieColor = System.Drawing.Color.Red;
            this.map.TransParent = 255;
            this.map.TypeColor = null;
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
            // ultraDockManager
            // 
            dockAreaPane1.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            dockableControlPane1.Control = this.pnlChart;
            dockableControlPane1.Key = "DD";
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(96, 24, 256, 480);
            dockableControlPane1.Size = new System.Drawing.Size(376, 278);
            dockableControlPane1.Text = "Defect Distribution";
            dockableControlPane2.Control = this.imageList;
            dockableControlPane2.Key = "IL";
            dockableControlPane2.OriginalControlBounds = new System.Drawing.Rectangle(243, 151, 200, 100);
            dockableControlPane2.Size = new System.Drawing.Size(100, 100);
            dockableControlPane2.Text = "ImageList";
            dockableControlPane3.Control = this.panelData;
            dockableControlPane3.Key = "RA";
            dockableControlPane3.OriginalControlBounds = new System.Drawing.Rectangle(616, 128, 200, 344);
            dockableControlPane3.Size = new System.Drawing.Size(376, 278);
            dockableControlPane3.Text = "Raw Data";
            dockableControlPane4.Control = this.pnlInformation;
            dockableControlPane4.Key = "IM";
            dockableControlPane4.OriginalControlBounds = new System.Drawing.Rectangle(342, 215, 200, 100);
            dockableControlPane4.Size = new System.Drawing.Size(100, 100);
            dockableControlPane4.Text = "Information";
            dockableControlPane5.Control = this.Reclassify;
            dockableControlPane5.Key = "RC";
            dockableControlPane5.OriginalControlBounds = new System.Drawing.Rectangle(393, 198, 200, 100);
            dockableControlPane5.Size = new System.Drawing.Size(100, 100);
            dockableControlPane5.Text = "Reclassify";
            dockableControlPane6.Control = this.rdSetup;
            dockableControlPane6.Key = "RD";
            dockableControlPane6.OriginalControlBounds = new System.Drawing.Rectangle(616, 128, 200, 344);
            dockableControlPane6.Size = new System.Drawing.Size(376, 278);
            dockableControlPane6.Text = "RD";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1,
            dockableControlPane2,
            dockableControlPane3,
            dockableControlPane4,
            dockableControlPane5,
            dockableControlPane6});
            dockAreaPane1.SelectedTabIndex = 5;
            dockAreaPane1.Size = new System.Drawing.Size(327, 600);
            this.ultraDockManager.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.ultraDockManager.HostControl = this;
            this.ultraDockManager.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.VisualStudio2005;
            this.ultraDockManager.PaneDisplayed += new Infragistics.Win.UltraWinDock.PaneDisplayedEventHandler(this.ultraDockManager_PaneDisplayed);
            // 
            // _DPUCDefecMapViewerUnpinnedTabAreaLeft
            // 
            this._DPUCDefecMapViewerUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._DPUCDefecMapViewerUnpinnedTabAreaLeft.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDefecMapViewerUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._DPUCDefecMapViewerUnpinnedTabAreaLeft.Name = "_DPUCDefecMapViewerUnpinnedTabAreaLeft";
            this._DPUCDefecMapViewerUnpinnedTabAreaLeft.Owner = this.ultraDockManager;
            this._DPUCDefecMapViewerUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 600);
            this._DPUCDefecMapViewerUnpinnedTabAreaLeft.TabIndex = 0;
            // 
            // _DPUCDefecMapViewerUnpinnedTabAreaRight
            // 
            this._DPUCDefecMapViewerUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._DPUCDefecMapViewerUnpinnedTabAreaRight.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDefecMapViewerUnpinnedTabAreaRight.Location = new System.Drawing.Point(1079, 0);
            this._DPUCDefecMapViewerUnpinnedTabAreaRight.Name = "_DPUCDefecMapViewerUnpinnedTabAreaRight";
            this._DPUCDefecMapViewerUnpinnedTabAreaRight.Owner = this.ultraDockManager;
            this._DPUCDefecMapViewerUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 600);
            this._DPUCDefecMapViewerUnpinnedTabAreaRight.TabIndex = 1;
            // 
            // _DPUCDefecMapViewerUnpinnedTabAreaTop
            // 
            this._DPUCDefecMapViewerUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._DPUCDefecMapViewerUnpinnedTabAreaTop.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDefecMapViewerUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._DPUCDefecMapViewerUnpinnedTabAreaTop.Name = "_DPUCDefecMapViewerUnpinnedTabAreaTop";
            this._DPUCDefecMapViewerUnpinnedTabAreaTop.Owner = this.ultraDockManager;
            this._DPUCDefecMapViewerUnpinnedTabAreaTop.Size = new System.Drawing.Size(1079, 0);
            this._DPUCDefecMapViewerUnpinnedTabAreaTop.TabIndex = 2;
            // 
            // _DPUCDefecMapViewerUnpinnedTabAreaBottom
            // 
            this._DPUCDefecMapViewerUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._DPUCDefecMapViewerUnpinnedTabAreaBottom.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDefecMapViewerUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 600);
            this._DPUCDefecMapViewerUnpinnedTabAreaBottom.Name = "_DPUCDefecMapViewerUnpinnedTabAreaBottom";
            this._DPUCDefecMapViewerUnpinnedTabAreaBottom.Owner = this.ultraDockManager;
            this._DPUCDefecMapViewerUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1079, 0);
            this._DPUCDefecMapViewerUnpinnedTabAreaBottom.TabIndex = 3;
            // 
            // _DPUCDefecMapViewerAutoHideControl
            // 
            this._DPUCDefecMapViewerAutoHideControl.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDefecMapViewerAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._DPUCDefecMapViewerAutoHideControl.Name = "_DPUCDefecMapViewerAutoHideControl";
            this._DPUCDefecMapViewerAutoHideControl.Owner = this.ultraDockManager;
            this._DPUCDefecMapViewerAutoHideControl.Size = new System.Drawing.Size(0, 0);
            this._DPUCDefecMapViewerAutoHideControl.TabIndex = 4;
            // 
            // MapPanel
            // 
            this.MapPanel.Controls.Add(this.map);
            this.MapPanel.Controls.Add(this.statusStrip1);
            this.MapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapPanel.Location = new System.Drawing.Point(0, 0);
            this.MapPanel.Name = "MapPanel";
            this.MapPanel.Size = new System.Drawing.Size(747, 600);
            this.MapPanel.TabIndex = 15;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolWaferDieLocation,
            this.toolParticles,
            this.toolTotalDie,
            this.toolSelectedDefect,
            this.toolDefectStat});
            this.statusStrip1.Location = new System.Drawing.Point(0, 576);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(747, 24);
            this.statusStrip1.TabIndex = 0;
            // 
            // toolWaferDieLocation
            // 
            this.toolWaferDieLocation.AutoSize = false;
            this.toolWaferDieLocation.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolWaferDieLocation.Name = "toolWaferDieLocation";
            this.toolWaferDieLocation.Size = new System.Drawing.Size(120, 19);
            this.toolWaferDieLocation.Text = "X : 0, Y : 0";
            // 
            // toolParticles
            // 
            this.toolParticles.AutoSize = false;
            this.toolParticles.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolParticles.Name = "toolParticles";
            this.toolParticles.Size = new System.Drawing.Size(90, 19);
            this.toolParticles.Text = "Particles : 0";
            // 
            // toolTotalDie
            // 
            this.toolTotalDie.AutoSize = false;
            this.toolTotalDie.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolTotalDie.Name = "toolTotalDie";
            this.toolTotalDie.Size = new System.Drawing.Size(140, 19);
            this.toolTotalDie.Text = "Total Die : 0";
            // 
            // toolSelectedDefect
            // 
            this.toolSelectedDefect.AutoSize = false;
            this.toolSelectedDefect.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolSelectedDefect.Name = "toolSelectedDefect";
            this.toolSelectedDefect.Size = new System.Drawing.Size(140, 19);
            this.toolSelectedDefect.Text = "Selected Defect : 0";
            // 
            // toolDefectStat
            // 
            this.toolDefectStat.AutoSize = false;
            this.toolDefectStat.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolDefectStat.Name = "toolDefectStat";
            this.toolDefectStat.Size = new System.Drawing.Size(242, 19);
            this.toolDefectStat.Spring = true;
            this.toolDefectStat.Text = "Total Defect : 0";
            // 
            // windowDockingArea4
            // 
            this.windowDockingArea4.Controls.Add(this.dockableWindow1);
            this.windowDockingArea4.Controls.Add(this.dockableWindow2);
            this.windowDockingArea4.Controls.Add(this.dockableWindow3);
            this.windowDockingArea4.Controls.Add(this.dockableWindow4);
            this.windowDockingArea4.Controls.Add(this.dockableWindow5);
            this.windowDockingArea4.Controls.Add(this.dockableWindow6);
            this.windowDockingArea4.Dock = System.Windows.Forms.DockStyle.Right;
            this.windowDockingArea4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.windowDockingArea4.Location = new System.Drawing.Point(747, 0);
            this.windowDockingArea4.Name = "windowDockingArea4";
            this.windowDockingArea4.Owner = this.ultraDockManager;
            this.windowDockingArea4.Size = new System.Drawing.Size(332, 600);
            this.windowDockingArea4.TabIndex = 14;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.pnlChart);
            this.dockableWindow1.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager;
            this.dockableWindow1.Size = new System.Drawing.Size(327, 580);
            this.dockableWindow1.TabIndex = 16;
            // 
            // dockableWindow2
            // 
            this.dockableWindow2.Controls.Add(this.imageList);
            this.dockableWindow2.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow2.Name = "dockableWindow2";
            this.dockableWindow2.Owner = this.ultraDockManager;
            this.dockableWindow2.Size = new System.Drawing.Size(327, 580);
            this.dockableWindow2.TabIndex = 17;
            // 
            // dockableWindow3
            // 
            this.dockableWindow3.Controls.Add(this.panelData);
            this.dockableWindow3.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow3.Name = "dockableWindow3";
            this.dockableWindow3.Owner = this.ultraDockManager;
            this.dockableWindow3.Size = new System.Drawing.Size(0, 0);
            this.dockableWindow3.TabIndex = 18;
            // 
            // dockableWindow4
            // 
            this.dockableWindow4.Controls.Add(this.pnlInformation);
            this.dockableWindow4.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow4.Name = "dockableWindow4";
            this.dockableWindow4.Owner = this.ultraDockManager;
            this.dockableWindow4.Size = new System.Drawing.Size(327, 580);
            this.dockableWindow4.TabIndex = 19;
            // 
            // dockableWindow5
            // 
            this.dockableWindow5.Controls.Add(this.Reclassify);
            this.dockableWindow5.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow5.Name = "dockableWindow5";
            this.dockableWindow5.Owner = this.ultraDockManager;
            this.dockableWindow5.Size = new System.Drawing.Size(327, 580);
            this.dockableWindow5.TabIndex = 20;
            // 
            // dockableWindow6
            // 
            this.dockableWindow6.Controls.Add(this.rdSetup);
            this.dockableWindow6.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow6.Name = "dockableWindow6";
            this.dockableWindow6.Owner = this.ultraDockManager;
            this.dockableWindow6.Size = new System.Drawing.Size(327, 580);
            this.dockableWindow6.TabIndex = 21;
            // 
            // DPUCDefecMapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.Controls.Add(this._DPUCDefecMapViewerAutoHideControl);
            this.Controls.Add(this.MapPanel);
            this.Controls.Add(this.windowDockingArea4);
            this.Controls.Add(this._DPUCDefecMapViewerUnpinnedTabAreaTop);
            this.Controls.Add(this._DPUCDefecMapViewerUnpinnedTabAreaBottom);
            this.Controls.Add(this._DPUCDefecMapViewerUnpinnedTabAreaLeft);
            this.Controls.Add(this._DPUCDefecMapViewerUnpinnedTabAreaRight);
            this.Name = "DPUCDefecMapViewer";
            this.Size = new System.Drawing.Size(1079, 600);
            this.Load += new System.EventHandler(this.DPUCDefecMapViewer_Load);
            this.pnlChart.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChartDefect)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbChartType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbXAxisItem)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadColor_Sheet)).EndInit();
            this.plDensity.ResumeLayout(false);
            this.pnGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDefectCount)).EndInit();
            this.contextMenuColor.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbYAxisItem)).EndInit();
            this.panelData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect_Sheet)).EndInit();
            this.pnlInformation.ResumeLayout(false);
            this.Reclassify.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).EndInit();
            this.MapPanel.ResumeLayout(false);
            this.MapPanel.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.windowDockingArea4.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            this.dockableWindow2.ResumeLayout(false);
            this.dockableWindow2.PerformLayout();
            this.dockableWindow3.ResumeLayout(false);
            this.dockableWindow4.ResumeLayout(false);
            this.dockableWindow5.ResumeLayout(false);
            this.dockableWindow6.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region [ Datatable is chart option ]

        //--

        private DataTable GetChartType()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] { 
                    new DataColumn("KEY", typeof(String)), 
                    new DataColumn("VALUE", typeof(String)), 
                    new DataColumn("TYPE", typeof(Boolean)) 
                });

            //--

            DataRow row = dt.NewRow();
            row["KEY"] = "COLUMN";
            row["VALUE"] = "Bar";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "LINE";
            row["VALUE"] = "Line";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "PIE";
            row["VALUE"] = "Pie";
            dt.Rows.Add(row);

            //--

            row = dt.NewRow();
            row["KEY"] = "PYRAMID";
            row["VALUE"] = "Pyramid";
            dt.Rows.Add(row);

            return dt;
        }

        //--

        private DataTable GetChartXAxis()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] { 
                    new DataColumn("KEY", typeof(String)), 
                    new DataColumn("VALUE", typeof(String)), 
                    new DataColumn("TYPE", typeof(Boolean)) 
                });

            //--

            DataRow row = dt.NewRow();
            row["KEY"] = Chart_DefectClass;
            row["VALUE"] = "Defect Class";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = Chart_InspectedWafer;
            row["VALUE"] = "Inspected Wafer";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = Chart_StepID;
            row["VALUE"] = "Step ID";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = Chart_InspectionTime;
            row["VALUE"] = "Inspection Time";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = Chart_DefectSize;
            row["VALUE"] = "Defect Size";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["KEY"] = Chart_DefectType;
            row["VALUE"] = "Defect Type";
            dt.Rows.Add(row);

            return dt;
        }

        //--

        private DataTable GetChartYAxis()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] { 
                    new DataColumn("KEY", typeof(String)), 
                    new DataColumn("VALUE", typeof(String)), 
                    new DataColumn("TYPE", typeof(Boolean)) 
                });

            //--

            DataRow row = dt.NewRow();
            row["KEY"] = "DEFECTS";
            row["VALUE"] = "Defect Count";
            dt.Rows.Add(row);

            return dt;
        }

        #endregion [ Datatable is chart option ]

        #region [ Event Handler ]

        private void imageList_SizeChanged(object sender, EventArgs e)
        {
            imageList.SuspendLayout();

            foreach (System.Windows.Forms.Control ctrl in imageList.Controls)
            {
                ctrl.Height = ctrl.Height + (ctrl.Margin.Top + ctrl.Margin.Bottom);
                ctrl.Width = imageList.Width - 30;
            }

            imageList.ResumeLayout();
        }

        private List<DataPoint> m_selectedPoint = new List<DataPoint>();

        private void ChartDefect_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                map.CustomDefectVisibleCheck = true;
                map.SelectedDefect.Clear();

                HitTestResult result = ChartDefect.HitTest(e.X, e.Y);
                DataPoint point = null;

                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    point = ChartDefect.Series[0].Points[result.PointIndex];
                }
                else if (result.ChartElementType == ChartElementType.AxisLabels && result.Axis.AxisName == AxisName.X && result.Object is CustomLabel)
                {
                    point = ChartDefect.Series[0].Points[(int)(result.Object as CustomLabel).FromPosition];
                }

                // Shift:더하기, Ctrl:빼기
                if (System.Windows.Forms.Control.ModifierKeys == Keys.Shift)
                {
                    if (!m_selectedPoint.Contains(point) && point != null)
                        m_selectedPoint.Add(point);
                }
                else if (System.Windows.Forms.Control.ModifierKeys == Keys.Control)
                {
                    if (m_selectedPoint.Contains(point))
                        m_selectedPoint.Remove(point);
                }
                else
                {
                    m_selectedPoint.Clear();

                    if (point != null)
                        m_selectedPoint.Add(point);
                }

                if (m_selectedPoint.Count == 0)
                {
                    map.Defects.SetVisibleAll(true);
                }
                else
                {
                    String axis = cmbXAxisItem.Value.ToString();

                    if (axis == Chart_DefectClass)
                    {
                        List<int> list = new List<int>();

                        foreach (DataPoint pt in m_selectedPoint)
                            list.Add((int)pt.Tag);

                        map.Defects.SetVisible<int>(axis, list);
                    }
                    else if (axis == Chart_InspectedWafer || axis == Chart_StepID)
                    {
                        List<string> list = new List<string>();

                        foreach (DataPoint pt in m_selectedPoint)
                            list.Add(pt.Tag.ToString());

                        map.Defects.SetVisible<string>(axis, list);
                    }
                    else if (axis == Chart_InspectionTime)
                    {
                        List<DateTime> list = new List<DateTime>();

                        foreach (DataPoint pt in m_selectedPoint)
                            list.Add((DateTime)pt.Tag);

                        map.Defects.SetVisible<DateTime>(axis, list);
                    }
                    else if (axis == Chart_DefectSize)
                    {
                        List<Tuple<double, double>> list = new List<Tuple<double, double>>();

                        foreach (DataPoint pt in m_selectedPoint)
                            list.Add((Tuple<double, double>)pt.Tag);

                        map.Defects.SetVisibleSize(list);
                    }
                    else if (axis == Chart_DefectType)
                    {
                        List<string> list = new List<string>();

                        foreach (DataPoint pt in m_selectedPoint)
                            list.Add(pt.Tag.ToString());

                        DefectType defectType = DefectType.None;

                        foreach (string item in list)
                        {
                            if (item == DefectList.DEFECT_TYPE_NEW)
                                defectType |= DefectType.New;
                            else if (item == DefectList.DEFECT_TYPE_RANDOM)
                                defectType |= DefectType.Random;
                            else if (item == DefectList.DEFECT_TYPE_CLUSTER)
                                defectType |= DefectType.Cluster;
                        }

                        map.Defects.SetVisibleType(defectType);
                    }
                }

                map.Redraw();
            }
            finally
            {
                ChartDefect.Cursor = Cursors.Hand;
            }

            /*
                else if (String.Equals(strChartXAxis, "DSIZE"))
                {
                    string[] strSize = ChartDefect.Series[0].Points[result.PointIndex].Tag.ToString().Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
                    if (strSize.Length == 2)
                    {
                        double dFrom = double.Parse(strSize[0].Trim());
                        double dto = double.Parse(strSize[1].Trim());

                        if (dtDefectData.Select(string.Format("DSIZE >= {0} AND DSIZE <= {1}", dFrom, dto)).Length > 0)
                        {
                            //Filter된 Defect 만 Draw
                            defects = DefectMapAnalysis.DataTableToDefectList(dtDefectData.Select(string.Format("DSIZE >= {0} AND DSIZE <= {1}", dFrom, dto), "DEFECTID").CopyToDataTable<DataRow>());
                        }
                    }
                }
            //*/
        }

        private void ChartDefect_MouseMove(object sender, MouseEventArgs e)
        {
            // Call Hit Test Method
            HitTestResult result = ChartDefect.HitTest(e.X, e.Y);

            try
            {
                if (ModifierKeys == Keys.Shift || ModifierKeys == Keys.ShiftKey || result.PointIndex < 0)
                    return;

                //SelectedDefects.Clear();
                // Reset Data Point Attributes
                foreach (DataPoint point in ChartDefect.Series[0].Points)
                {
                    point.BackSecondaryColor = Color.Black;
                    point.BackHatchStyle = ChartHatchStyle.None;
                    point.BorderWidth = 1;
                }

                // If a Data Point or a Legend item is selected.
                if (result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.LegendItem)
                {
                    // Set cursor type 
                    //Cursor = Cursors.Hand;

                    // Find selected data point
                    DataPoint point = ChartDefect.Series[0].Points[result.PointIndex];

                    // Set End Gradient Color to White
                    point.BackSecondaryColor = Color.Blue;

                    // Set selected hatch style
                    point.BackHatchStyle = ChartHatchStyle.Percent50;

                    // Increase border width
                    point.BorderWidth = 3;
                }
            }
            finally
            { }
        }

        private DataTable GetInitDefectConfig()
        {
            DataTable dtDefectConfig = new DataTable("DEFECT_CNT");
            dtDefectConfig.Columns.Add(new DataColumn(DEFECT_CNT.FROM.ToString(), typeof(int)));
            dtDefectConfig.Columns.Add(new DataColumn(DEFECT_CNT.TO.ToString(), typeof(int)));
            dtDefectConfig.Columns.Add(new DataColumn(DEFECT_CNT.COLOR.ToString(), typeof(string)));
            dtDefectConfig.AcceptChanges();

            DataRow drItem = dtDefectConfig.NewRow();
            drItem[(int)DEFECT_CNT.FROM] = 0;
            drItem[(int)DEFECT_CNT.TO] = 0;
            drItem[(int)DEFECT_CNT.COLOR] = "#FFFFFF";
            dtDefectConfig.Rows.Add(drItem);

            drItem = dtDefectConfig.NewRow();
            drItem[(int)DEFECT_CNT.FROM] = 1;
            drItem[(int)DEFECT_CNT.TO] = 5;
            drItem[(int)DEFECT_CNT.COLOR] = "#DAF7A6";
            dtDefectConfig.Rows.Add(drItem);

            drItem = dtDefectConfig.NewRow();
            drItem[(int)DEFECT_CNT.FROM] = 6;
            drItem[(int)DEFECT_CNT.TO] = 10;
            drItem[(int)DEFECT_CNT.COLOR] = "#FFC300";
            dtDefectConfig.Rows.Add(drItem);

            drItem = dtDefectConfig.NewRow();
            drItem[(int)DEFECT_CNT.FROM] = 11;
            drItem[(int)DEFECT_CNT.TO] = 15;
            drItem[(int)DEFECT_CNT.COLOR] = "#FF5733";
            dtDefectConfig.Rows.Add(drItem);

            drItem = dtDefectConfig.NewRow();
            drItem[(int)DEFECT_CNT.FROM] = 16;
            drItem[(int)DEFECT_CNT.TO] = 20;
            drItem[(int)DEFECT_CNT.COLOR] = "#C70039";
            dtDefectConfig.Rows.Add(drItem);

            drItem = dtDefectConfig.NewRow();
            drItem[(int)DEFECT_CNT.FROM] = 21;
            drItem[(int)DEFECT_CNT.TO] = 30;
            drItem[(int)DEFECT_CNT.COLOR] = "#900C3F";
            dtDefectConfig.Rows.Add(drItem);

            drItem = dtDefectConfig.NewRow();
            drItem[(int)DEFECT_CNT.FROM] = 31;
            drItem[(int)DEFECT_CNT.TO] = 1000;
            drItem[(int)DEFECT_CNT.COLOR] = "#581845";
            dtDefectConfig.Rows.Add(drItem);

            dtDefectConfig.AcceptChanges();

            return dtDefectConfig;
        }

        private void SetDefectCountColor()
        {
            if (dtDefectCnt == null || dtDefectCnt.Rows.Count <= 0)
                return;

            dgDefectCount.DataSource = dtDefectCnt;

            dgDefectCount.Columns[0].HeaderText = DEFECT_CNT.FROM.ToString();
            dgDefectCount.Columns[1].HeaderText = DEFECT_CNT.TO.ToString();
            dgDefectCount.Columns[2].HeaderText = DEFECT_CNT.COLOR.ToString();

            dgDefectCount.Columns[0].Width = 60;
            dgDefectCount.Columns[1].Width = 60;
            dgDefectCount.Columns[2].Width = 80;

            dgDefectCount.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgDefectCount.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgDefectCount.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgDefectCount.Columns[0].ReadOnly = false;
            dgDefectCount.Columns[1].ReadOnly = false;
            dgDefectCount.Columns[2].ReadOnly = true;

            foreach (DataGridViewRow dRow in dgDefectCount.Rows)
            {
                if (dRow.IsNewRow == true)
                    continue;

                Color DefectCNTColor = ColorTranslator.FromHtml(dRow.Cells[(int)DEFECT_CNT.COLOR].Value.ToString());
                dRow.Cells[(int)DEFECT_CNT.COLOR].Style.BackColor = DefectCNTColor;
            }

            dgDefectCount.Refresh();
        }

        private void map_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            toolWaferDieLocation.Text = null;
            toolParticles.Text = null;

            if (map.Defects.Count == 0)
                return;

            toolWaferDieLocation.Text = string.Format("X : {0} Y : {1}", NewDie.IndexX, NewDie.IndexY);

            Defect[] defectArr = map.Defects.GetDefectInDie(NewDie.IndexX, NewDie.IndexY);
            toolParticles.Text = string.Format("Particles : {0}", defectArr.Length);
        }

        private void BtnRedraw_Click(object sender, EventArgs e)
        {
            DrawChart();
        }

        private void cmbXAxisItem_ValueChanged(object sender, EventArgs e)
        {
            chkNormalize.Checked = false;
            chkNormalize.Enabled = false;

            ChartDefect.Cursor = Cursors.Hand;
            switch (cmbXAxisItem.SelectedItem.DataValue.ToString())
            {
                case Chart_DefectClass:
                    this.map.MapType = DACrux.Base.MAP_TYPE.CLASS;
                    fpSpreadColor_Sheet.DataSource = map.TypeColor;
                    chkNormalize.Enabled = true;
                    break;
                case Chart_DefectSize:
                    this.map.MapType = DACrux.Base.MAP_TYPE.SIZE;
                    break;
                default:
                    this.map.MapType = DACrux.Base.MAP_TYPE.CLASS;
                    break;
            }

            btnRedraw.PerformClick();
        }

        private void dgDefectCount_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)DEFECT_CNT.COLOR)
            {
                ColorDialog ColorDLG = new ColorDialog();

                if (ColorDLG.ShowDialog() == DialogResult.OK)
                {
                    dgDefectCount.Rows[e.RowIndex].Cells[2].Value = ColorTranslator.ToHtml(ColorDLG.Color);
                    dgDefectCount.Rows[e.RowIndex].Cells[2].Style.BackColor = ColorDLG.Color;
                    //dgDefectCount.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorDLG.Color;
                }
            }
        }

        /// <summary>
        /// Defect Count 별로 색상을 표시 한다.
        /// </summary>
        private void BtnDefectCountReDraw_Click(object sender, EventArgs e)
        {
            try
            {
                map.CustomDefectVisibleCheck = false;

                if (map.Defects.Count == 0)
                    return;

                StatusMessage("Map 을 Redraw 중입니다.");

                if (chkDefectCount.Checked)
                    FilterDefectCountColor();
                else
                    map.SetItemCountColorList.Clear();

                map.Redraw();
            }
            finally
            {
                StatusMessage(null);
            }

        }

        private void chkNormalize_CheckedChanged(object sender, EventArgs e)
        {
            btnRedraw.PerformClick();
        }

        private void chkScaleBreaks_CheckedChanged(object sender, EventArgs e)
        {
            btnRedraw.PerformClick();
        }

        private void colorResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dtDefectCnt = GetInitDefectConfig();
            SetDefectCountColor();
        }

        private void fpSpreadColor_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            try
            {
                //Color 관련된 Column 일 경우 색상을 다시 표현 할 수 있다.
                if (e.Column == (int)DEFECT_COLOR.COLOR && map.TypeColor != null && map.TypeColor.Rows.Count > 0)
                {
                    ColorDialog ColorDLG = new ColorDialog();
                    if (ColorDLG.ShowDialog() == DialogResult.OK)
                    {
                        StatusMessage("Defect Class Color 변경 중..");

                        int classNumber = DACrux.Base.Convert.intParse(fpSpreadColor_Sheet.Cells[e.Row, (int)DEFECT_COLOR.CLASSNUMBER].Text);
                        map.ChangeTypeColor(classNumber, ColorTranslator.ToHtml(ColorDLG.Color));

                        fpSpreadColor.ActiveSheet.Cells[e.Row, e.Column].BackColor = ColorDLG.Color;
                        DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadColor_Sheet);
                        fpSpreadColor_Sheet.Columns[(int)DEFECT_COLOR.CLASSNUMBER].Width = 20;

                        //Redraw 한다.
                        BtnDefectCountReDraw.PerformClick();
                        BtnRedraw_Click(null, null);

                    }
                }
            }
            finally
            {
                StatusMessage(null);
            }
        }

        private void sizeResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartDefect.ChartAreas["Default"].AxisX.ScaleView.ZoomReset(0);
            ChartDefect.ChartAreas["Default"].AxisY.ScaleView.ZoomReset(0);
        }

        private void dgDefectCount_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)DEFECT_CNT.TO && dgDefectCount.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                int ibVal = DACrux.Base.Convert.intParse(dgDefectCount.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                ibVal++;

                if (dgDefectCount.Rows.Count > e.RowIndex + 1)
                {
                    if (dgDefectCount.Rows[e.RowIndex + 1].IsNewRow == true)
                    {
                        dtDefectCnt = (DataTable)dgDefectCount.DataSource;
                        DataRow dr = dtDefectCnt.NewRow();
                        dr[(int)DEFECT_CNT.FROM] = ibVal;
                        dr[(int)DEFECT_CNT.TO] = DBNull.Value;
                        dr[(int)DEFECT_CNT.COLOR] = DBNull.Value;
                        dtDefectCnt.Rows.Add(dr);
                        dtDefectCnt.AcceptChanges();

                        SetDefectCountColor();
                    }
                    else
                    {
                        dgDefectCount.Rows[e.RowIndex + 1].Cells[(int)DEFECT_CNT.FROM].Value = ibVal;
                    }

                }
            }
        }

        private void map_OnSelectedDefect(object sender, DACrux.Base.Defect[] oDefect)
        {
            DPUCDefectInfo oForm = null;

            try
            {
                toolSelectedDefect.Text = string.Format("Selected Defect : {0}", (oDefect == null) ? 0 : oDefect.Length);

                if (ultraDockManager.ControlPanes["RA"].IsInView)
                    fpSpreadDefect.ActiveSheet.DataSource = ConvertDefects(oDefect);

                if (ultraDockManager.ControlPanes["IM"].IsInView) // 선택된 Defect가 있으면 DefectList 정보를, 없으면 Map 정보를 보여준다. 2020.01.14 Taihi,Kim.
                {
                    if (oDefect != null && oDefect.Length > 0)
                        dpucInformation1.DataSource = oDefect;
                    else
                        dpucInformation1.DataSource = map;
                }

                if (ultraDockManager.ControlPanes["RC"].IsInView)
                {
                    DefectList defects = new DefectList();
                    defects.AddRange(oDefect);
                    dpucReclassify1.DataSource = defects;
                }

                if (!ultraDockManager.ControlPanes["IL"].IsInView)
                    return;

                if (imageList.Controls.Count > 0)
                    imageList.Controls.Clear();

                for (int i = 0; i < oDefect.Length; i++)
                {
                    foreach (DefectImage img in oDefect[i].Images)
                    {
                        System.Threading.Thread.Sleep(10);

                        try
                        {
                            oForm = new DPUCDefectInfo(oDefect[i].DEFECTID.ToString(), img.IMAGEPATH, oDefect[i].XINDEX.ToString(), oDefect[i].YINDEX.ToString(),
                                oDefect[i].CLASSNUMBER.ToString(), oDefect[i].CLUSTERNUMBER.ToString(), oDefect[i].XREL.ToString(),
                                oDefect[i].YREL.ToString(), oDefect[i].XSIZE.ToString(), oDefect[i].YSIZE.ToString());

                            ControlAdd(oForm);
                        }
                        catch { }

                        StatusMessage(string.Format("Defect Image 확인중..({0}/{1})", i, oDefect.Length));
                    }

                    dpucInformation1.DataSource = oDefect[i];
                }

                imageList.Refresh();
            }
            finally
            {
                StatusMessage(null);
            }

        }

        private void dpucReclassify1_OnReclassifyApply(
            object sender,
            EventArgs e
            )
        {
            if (dpucReclassify1.DataSource == null || dpucReclassify1.DataSource.Count <= 0)
                return;

            DefectList defects = dpucReclassify1.DataSource as DefectList;
            string[,] Params = new string[defects.Count, 5];
            object item = dpucReclassify1.SelectedNewDefectClass;
            if (item == null) return;

            if (MessageBox.Show("Reclassify를 진행하시겠습니까?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                return;

            for (int idx = 0; idx < defects.Count; idx++)
            {
                Params[idx, (int)ReclassifiedNewDefectColIndex.STEP_SEQ] = defects[idx].STEP_SEQ.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.DEFECTID] = defects[idx].DEFECTID.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.WAFER_SEQ] = defects[idx].WAFER_SEQ.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.CLASSNUMBER] = defects[idx].CLASSNUMBER.ToString();
                Params[idx, (int)ReclassifiedNewDefectColIndex.NEW_DEFECT_CLASS] = item.ToString();
            }

            DefectMapAnalysis obj = new DefectMapAnalysis();
            long[] steps = new long[Wafer.Length];
            for (int idx = 0; idx < Wafer.Length; idx++)
            {
                steps[idx] = Base.Convert.longParse(Wafer[idx].StepSeq);
            }

            obj.SetReclassifyDefectList_Comp(
                steps,
                Params,
                Base.GlobalVariable.UserID,
                Base.GlobalVariable.LocalIP
                );

            Draw();
        }

        #endregion [ Event Handler ]

        #region [ Method ]

        public void Draw()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (Wafer == null || Wafer.Length == 0)
                    return;

                var lStepSeq = new long[Wafer.Length];

                for (int iw = 0; iw < Wafer.Length; iw++)
                    lStepSeq[iw] = DACrux.Base.Convert.longParse(Wafer[iw].StepSeq);


#if DEBUG
                map.CenterMark = true;
#endif
                StatusMessage("Defect 데이터를 조회 중입니다.");

                RO.DefectMapAnalysis oDMapAnalysis = new RO.DefectMapAnalysis();
                DefectList defectList = new DefectList();
                defectList.AddRange(oDMapAnalysis.GetDefectMapViewer_DefectArray(lStepSeq));

                Draw(defectList);
            }
            finally
            {
                StatusMessage(null);
                this.Cursor = Cursors.Default;
            }
        }

        public void Draw(DefectList defectList)
        {
            try
            {
                map.DefectClear();

                if (defectList == null || defectList.Count == 0)
                    return;

                map.Defects.AddRange(defectList);

                long[] lStepSeq = defectList.GetStepSeqArray();

                dpucReclassify1.DataSource = null;

                // 이전 조회한 DataSet이 있는 경우 Dispose
                if (dsDefect != null)
                    dsDefect.Dispose();

                StatusMessage("Map 을 조회 중입니다.");

                StatusMessage("기준 정보를 조회 중입니다.");

                RO.DefectMapAnalysis oDMapAnalysis = new RO.DefectMapAnalysis();
                dsDefect = oDMapAnalysis.GetDefectMapViewer_Info(lStepSeq);

                if (!dsDefect.Tables.Contains("STEP_INFO") || dsDefect.Tables["STEP_INFO"].Rows.Count <= 0)
                    throw new Exception("정의된 Step 정보가 없습니다.");

                if (!dsDefect.Tables.Contains("SETUP_INFO"))
                    throw new Exception("정의된 Setup 정보가 없습니다.");

                string[] strSetupArr = new string[dsDefect.Tables["STEP_INFO"].Rows.Count];
                string[] strTestArr = new string[dsDefect.Tables["STEP_INFO"].Rows.Count];

                for (int i = 0; i < dsDefect.Tables["STEP_INFO"].Rows.Count; i++)
                {
                    strSetupArr[i] = dsDefect.Tables["STEP_INFO"].Rows[i]["SETUP_SEQ"].ToString();
                    strTestArr[i] = dsDefect.Tables["STEP_INFO"].Rows[i]["TEST"].ToString();
                }

                DataTable dtWaferMap = oDMapAnalysis.GetDefectMapViewer_Map(lStepSeq, strSetupArr, strTestArr);

                if (dtChartdata != null)
                    dtChartdata.Dispose();

                // DEFECT IMAGE 경로를 비동기 조회
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GetDefectImagePathAsync), lStepSeq);

                StatusMessage("Data를 처리 중입니다.");

                map.Rotate(0);
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

                // SHOT 정보 설정
                if (dsDefect.Tables.Contains("STEP_INFO") && dsDefect.Tables["STEP_INFO"].Rows.Count > 0 && dsDefect.Tables["STEP_INFO"].Columns.Contains("ST_XCNT"))
                {
                    DataRow stepRow = dsDefect.Tables["STEP_INFO"].Rows[0];
                    rdSetup.ShotArrayX = map.ShotArrayX = DACrux.Base.Convert.intParse(stepRow["ST_XCNT"].ToString(), 1);
                    rdSetup.ShotArrayY = map.ShotArrayY = DACrux.Base.Convert.intParse(stepRow["ST_YCNT"].ToString(), 1);
                    rdSetup.ShotStartX = map.ShotStartX = DACrux.Base.Convert.intParse(stepRow["ST_START_X"].ToString(), 1);
                    rdSetup.ShotStartY = map.ShotStartY = DACrux.Base.Convert.intParse(stepRow["ST_START_Y"].ToString(), 1);
                }

                map.DieCalculation(true);
                map.DrawDefects = "ALL";
                map.DieClear();

                UserConfiguration.SetVirtualDie(map);

                foreach (DataRow dr in dtWaferMap.Rows)
                {
                    map.AddDie(new DACrux.Base.Die(
                        DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString()),
                        DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString()),
                        DACrux.Base.Convert.intParse(dr["TEST"].ToString()),
                        1
                        ));
                }

                // Shot Start Index 재조정 2019.12.26 Taihi,Kim.
                DefectMapDraw.ReadjustShotStartIndex(map);

                SetDefectClassVisible();

                if (chkDefectCount.Checked)
                {
                    FilterDefectCountColor();
                }

                //Index 별 Defect Count
                if (dsDefect.Tables.IndexOf("DEFECT_GRP") >= 0 && dsDefect.Tables["DEFECT_GRP"].Rows.Count > 0)
                    dtIndexDefectsGroup = dsDefect.Tables["DEFECT_GRP"];

                map.SetInfomation(DefectMapDraw.GetMapDescription(map.Defects));

                UserConfiguration.SetMapInformation(map, dsDefect);
                UserConfiguration.SetMapColor(map);

                map.WaferDrawMode = DACrux.Map.MapMode.Fit;
                map.DefectSelectMode();

                fpSpreadDefect_Sheet.DataSource = null;
                fpSpreadDefect_Sheet.Rows.Clear();
                fpSpreadDefect_Sheet.Columns.Clear();

                toolTotalDie.Text = string.Format("Total Die Count : {0}", map.Dies.Count);

                var stat = map.Defects.GetDefectStat();
                toolDefectStat.Text = String.Format("Total Defect : {0}, New Random : {1}, Cluster Group : {2}", map.Defects.Count, stat.NewRandom, stat.ClusterGroup);

                imageList.Controls.Clear();

                DrawChart();

                // Information 설정
                dpucInformation1.DataSource = map;

                rdSetup.ClearChartData();
            }
            finally
            {
                StatusMessage(null);
            }
        }

        /// <summary>
        /// 데이터에 따라 Defect Class 항목이 보이거나 보여지지 않도록 합니다.
        /// </summary>
        private void SetDefectClassVisible()
        {
            if (map.Defects.Count > 0 && fpSpreadColor_Sheet.RowCount > 0)
            {
                int[] classNumberArr = map.Defects.GetAllClassNumber();

                for (int ir = 0; ir < fpSpreadColor_Sheet.RowCount; ir++)
                {
                    fpSpreadColor_Sheet.Cells[ir, (int)DEFECT_COLOR.COLOR].BackColor = ColorTranslator.FromHtml(fpSpreadColor_Sheet.Cells[ir, (int)DEFECT_COLOR.COLOR].Value.ToString());

                    int classNumber = DACrux.Base.Convert.intParse(fpSpreadColor_Sheet.Cells[ir, (int)DEFECT_COLOR.CLASSNUMBER].Value.ToString());

                    fpSpreadColor_Sheet.Rows[ir].Visible = Array.BinarySearch(classNumberArr, classNumber) >= 0;
                }
            }
        }

        private int GetInt(object value)
        {
            if (value == null || value == DBNull.Value)
                throw new Exception("값이 비어있습니다.");

            int val;

            if (!Int32.TryParse(value.ToString(), out val))
                throw new Exception(String.Format("값('{0}')을 숫자로 변환할 수 없습니다.", value));

            return val;
        }

        private Color GetColor(object value)
        {
            if (value == null || value == DBNull.Value)
                throw new Exception("색상값이 비어있습니다.");

            try
            {
                return ColorTranslator.FromHtml(value.ToString());
            }
            catch
            {
                throw new Exception(String.Format("색상값('{0}')을 변환할 수 없습니다.", value));
            }
        }

        private void FilterDefectCountColor()
        {
            List<DataGridViewRow> rowList = new List<DataGridViewRow>();

            if (dgDefectCount.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgDefectCount.SelectedRows)
                    rowList.Add(row);
            }
            else
            {
                foreach (DataGridViewRow row in dgDefectCount.Rows)
                    rowList.Add(row);
            }

            map.SetItemCountColorList.Clear();

            foreach (DataGridViewRow row in rowList)
            {
                if (row.IsNewRow)
                    continue;

                map.SetItemCountColorList.Add(new ItemCountColor()
                {
                    From = GetInt(row.Cells[(int)DEFECT_CNT.FROM].Value),
                    To = GetInt(row.Cells[(int)DEFECT_CNT.TO].Value),
                    Color = GetColor(row.Cells[(int)DEFECT_CNT.COLOR].Value)
                });
            }
        }

        private void SaveDefectCountColor()
        {
            //Redraw 시 마다 File 상에 저장 해놓고 Form Load 시 불러 온다.
            if (dgDefectCount.Rows.Count > 1)
            {
                FileInfo oConfigFile = new FileInfo(strConfigFullPath);
                if (oConfigFile != null && oConfigFile.Exists)
                {
                    oConfigFile.Delete();
                }

                DirectoryInfo oDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "DM"));
                if (oDir.Exists == false)
                    oDir.Create();

                DataTable dtWrite = (DataTable)dgDefectCount.DataSource;
                dtWrite.WriteXml(oConfigFile.FullName);
            }
        }

        private void GetDefectImagePathAsync(object state)
        {
            long[] stepSeqArr = state as long[];

            DefectMapAnalysis obj = new DefectMapAnalysis();
            dtImageList = obj.GetDefectImagePath01(stepSeqArr);
            UpdateDefectImageInfo(dtImageList);
        }

        private void UpdateDefectImageInfo(DataTable dt)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<DataTable>(UpdateDefectImageInfo), dt);
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                decimal stepSeq = DACrux.Base.Convert.intParse(row["STEP_SEQ"].ToString());
                decimal defectid = DACrux.Base.Convert.intParse(row["DEFECTID"].ToString());

                int idx = map.Defects.BinarySearch((long)stepSeq, (int)defectid);

                if (idx < 0)
                    continue;

                Defect defect = map.Defects[idx];

                DefectImage img = new DefectImage();
                img.IMAGESEQ = DACrux.Base.Convert.intParse(row["IMAGE_ID"].ToString());
                img.IMAGEPATH = String.Format("{0}/{1}", row["IMAGE_PATH"].ToString().Trim(), row["IMAGE_FILENAME"].ToString().Trim());
                defect.Images.Add(img);
            }
        }

        private void UpdateDefectImageInfoFilter(object state)
        {
            if (dtImageList == null || dtImageList.Rows.Count <= 0)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            UpdateDefectImageInfoFilter(null);
                        }
                        ));

                return;
            }

            foreach (DataRow row in dtImageList.Rows)
            {
                decimal stepSeq = DACrux.Base.Convert.intParse(row["STEP_SEQ"].ToString());
                decimal defectid = DACrux.Base.Convert.intParse(row["DEFECTID"].ToString());

                int idx = map.Defects.BinarySearch((long)stepSeq, (int)defectid);

                if (idx < 0)
                    continue;

                Defect defect = map.Defects[idx];

                DefectImage img = new DefectImage();
                img.IMAGESEQ = DACrux.Base.Convert.intParse(row["IMAGE_ID"].ToString());
                img.IMAGEPATH = String.Format("{0}/{1}", row["IMAGE_PATH"].ToString().Trim(), row["IMAGE_FILENAME"].ToString().Trim());
                defect.Images.Add(img);
            }
        }

        private void ControlAdd(DPUCDefectInfo container)
        {
            if (imageList.InvokeRequired)
            {
                imageList.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        ControlAdd(container);
                    }
                ));
            }
            else
            {
                if (container == null) return;

                //container.Height = container.Height + (container.Margin.Top + container.Margin.Bottom);
                container.Height = container.Height + container.Margin.Top;
                container.Width = imageList.Width - SystemInformation.VerticalScrollBarWidth - container.Margin.Left - container.Margin.Right;
                imageList.Controls.Add(container);
                imageList.ResumeLayout();
            }
        }

        private void ImageList_MouseEnter(object sender, EventArgs e)
        {
            imageList.Focus();
        }

        private void DrawChart()
        {
            try
            {
                map.CustomDefectVisibleCheck = true;
                m_selectedPoint.Clear();

                //chart 관련 Initialize
                ChartDefect.Legends.Clear();
                ChartDefect.ChartAreas.Clear();
                ChartDefect.ChartAreas.Add("Default");
                ChartDefect.Series.Clear();
                ChartDefect.Series.Add("Default");

                if (map.Defects.Count == 0)
                    return;

                StatusMessage("Chart 를 ReDraw 중입니다.");

                if (cmbXAxisItem.Value == null || cmbYAxisItem.Value == null)
                    return;

                String sXAxis = cmbXAxisItem.Value.ToString();
                String sYAxis = cmbYAxisItem.Value.ToString();

                if (sXAxis == Chart_DefectClass)
                {
                    Dictionary<int, int> dic = map.Defects.GroupByCount<int>(sXAxis, true, true);

                    int total = dic.Values.Sum();
                    int categorized = 0;
                    /// uncategorized 존재 시 
                    if (dic.ContainsKey(0))
                        categorized = total - dic[0];
                    else
                        categorized = dic.Values.Sum();

                    // Normalize 처리
                    if (dic.ContainsKey(0) && chkNormalize.Checked)
                        dic.Remove(0);

                    foreach (var item in dic)
                    {
                        string className = DmsCache.Instance.ClassLookup[item.Key];
                        Color color;

                        string colorStr = Select(map.TypeColor, Chart_DefectClass, item.Key.ToString(), "COLOR");

                        if (!String.IsNullOrEmpty(colorStr))
                            color = ColorTranslator.FromHtml(colorStr);
                        else
                            color = Color.Gray;

                        DataPoint pt = new DataPoint(ChartDefect.Series["Default"]);

                        double value;

                        if (!chkNormalize.Checked)
                            value = item.Value;
                        else
                            value = Math.Round((double)item.Value / categorized * total, 3);

                        pt.Tag = item.Key;
                        pt.ToolTip = String.Format("{0} : {1}", className, value);
                        pt.BorderWidth = 2;
                        pt.SetValueXY(className, value);
                        pt.Color = color;
                        ChartDefect.Series["Default"].Points.Add(pt);
                    }
                }
                else if (sXAxis == Chart_InspectedWafer || sXAxis == Chart_StepID)
                {
                    Dictionary<string, int> dic = map.Defects.GroupByCount<string>(sXAxis, true);

                    foreach (var item in dic)
                    {
                        DataPoint pt = new DataPoint(ChartDefect.Series["Default"]);

                        pt.Tag = item.Key;
                        pt.ToolTip = String.Format("{0} : {1}", item.Key, item.Value);
                        pt.BorderWidth = 2;
                        pt.SetValueXY(item.Key, item.Value);
                        ChartDefect.Series["Default"].Points.Add(pt);
                        ChartDefect.ApplyPaletteColors();
                    }
                }
                else if (sXAxis == Chart_InspectionTime)
                {
                    Dictionary<DateTime, int> dic = map.Defects.GroupByCount<DateTime>(sXAxis, true);

                    foreach (var item in dic)
                    {
                        string key = item.Key.ToString(DATETIME_FORMAT);

                        DataPoint pt = new DataPoint(ChartDefect.Series["Default"]);

                        pt.Tag = item.Key;
                        pt.ToolTip = String.Format("{0} : {1}", key, item.Value);
                        pt.BorderWidth = 2;
                        pt.SetValueXY(key, item.Value);
                        ChartDefect.Series["Default"].Points.Add(pt);
                        ChartDefect.ApplyPaletteColors();
                    }
                }
                else if (sXAxis == Chart_DefectSize)
                {
                    DataTable dt = map.SizeColor;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        List<Tuple<double, double>> fromToList = new List<Tuple<double, double>>();
                        List<Color> colorList = new List<Color>();
                        List<string> labelList = new List<string>();

                        foreach (DataRow row in dt.Rows)
                        {
                            double from = DACrux.Base.Convert.doubleParse(row["SIZE_FROM"].ToString());
                            double to = DACrux.Base.Convert.doubleParse(row["SIZE_TO"].ToString());

                            fromToList.Add(new Tuple<double, double>(from, to));
                            colorList.Add(ColorTranslator.FromHtml(row["COLOR"].ToString()));
                            labelList.Add(row["LABEL"].ToString());
                        }

                        var dic = map.Defects.GroupBySizeCount(fromToList);
                        int i = 0;

                        foreach (var item in dic)
                        {
                            string key = labelList[i];

                            DataPoint pt = new DataPoint(ChartDefect.Series["Default"]);

                            pt.Tag = item.Key;
                            pt.ToolTip = String.Format("{0} : {1}", key, item.Value);
                            pt.BorderWidth = 2;
                            pt.Color = colorList[i];
                            pt.SetValueXY(key, item.Value);
                            ChartDefect.Series["Default"].Points.Add(pt);
                            i++;
                        }
                    }
                }
                else if (sXAxis == Chart_DefectType)
                {
                    Dictionary<string, int> dic = map.Defects.GroupByType();

                    foreach (var item in dic)
                    {
                        DataPoint pt = new DataPoint(ChartDefect.Series["Default"]);

                        pt.Tag = item.Key;
                        pt.ToolTip = String.Format("{0} : {1}", item.Key, item.Value);
                        pt.BorderWidth = 2;
                        pt.SetValueXY(item.Key, item.Value);
                        ChartDefect.Series["Default"].Points.Add(pt);
                        ChartDefect.ApplyPaletteColors();
                    }
                }

                //Label 을 보여줄때 Size 에 맞게 보여 준다.
                ChartDefect.Series["Default"].SmartLabelStyle.Enabled = true;
                ChartDefect.Series["Default"].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                ChartDefect.Series["Default"].SmartLabelStyle.CalloutLineAnchorCapStyle = LineAnchorCapStyle.Arrow;
                ChartDefect.Series["Default"].SmartLabelStyle.CalloutLineColor = Color.Black;
                ChartDefect.Series["Default"].SmartLabelStyle.CalloutLineWidth = 1;
                ChartDefect.Series["Default"].SmartLabelStyle.CalloutStyle = LabelCalloutStyle.None;
                ChartDefect.Series["Default"].MarkerSize = 2;
                ChartDefect.Series["Default"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                ChartDefect.Series["Default"].BorderWidth = 2;
                ChartDefect.Series["Default"].ChartType = ChartTypeChanged();

                /*//chart Point 입력
                for (int ir = 0; ir < dtChart.Rows.Count; ir++)
                {

                        case "DSIZE":
                            strSeries = dtChart.Rows[ir]["LABEL"].ToString();
                            strSeriesDESC = dtChart.Rows[ir]["LABEL"].ToString();

                            TempColor = ColorTranslator.FromHtml(dtChart.Rows[ir]["COLOR"].ToString());

                            if (double.TryParse(dtChart.Rows[ir][sXAxis].ToString(), out dValue) == false)
                                throw new Exception("Data 가 숫자 형식이 아닙니다.");

                            pt = new DataPoint(ChartDefect.Series["Default"]);

                            pt.Tag = strSeriesDESC;
                            pt.ToolTip = String.Format("{0} : {1}", strSeriesDESC, dValue);
                            pt.BorderWidth = 2;
                            pt.SetValueXY(strSeriesDESC, dValue);
                            pt.Color = TempColor;
                            ChartDefect.Series["Default"].Points.Add(pt);
                            break;

                    }
                }
                //*/

                //Pyramid 및 Pie Chart 의 경우 속성을 변경 해준다.
                if (ChartDefect.Series["Default"].ChartType == SeriesChartType.Pyramid)
                {
                    ChartDefect.Series["Default"].IsValueShownAsLabel = true;
                    ChartDefect.Series["Default"]["PyramidLabelStyle"] = "OutsideInColumn";
                    ChartDefect.Series["Default"]["PyramidOutsideLabelPlacement"] = "Right";
                    ChartDefect.Series["Default"]["PyramidPointGap"] = "2";
                    ChartDefect.Series["Default"]["PyramidMinPointHeight"] = "0";
                    ChartDefect.ChartAreas["Default"].Area3DStyle.Enable3D = true;
                    ChartDefect.Series["Default"]["Pyramid3DRotationAngle"] = "5";
                    ChartDefect.Series["Default"]["Pyramid3DDrawingStyle"] = "SquareBase";
                    ChartDefect.Series["Default"]["PyramidValueType"] = "Linear";
                    ChartDefect.Series["Default"].BorderWidth = 1;
                    ChartDefect.Series["Default"].BorderColor = Color.Black;
                    ChartDefect.Legends.Add("Legend1");
                    ChartDefect.Legends[0].Enabled = true;
                    ChartDefect.Legends[0].Docking = Docking.Bottom;
                    ChartDefect.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                }
                else if (ChartDefect.Series["Default"].ChartType == SeriesChartType.Pie)
                {
                    ChartDefect.ChartAreas["Default"].Area3DStyle.Enable3D = true;
                    ChartDefect.Series["Default"].IsValueShownAsLabel = true;
                    ChartDefect.Series["Default"]["PieLabelStyle"] = "Outside";
                    ChartDefect.Series["Default"].BorderWidth = 1;
                    ChartDefect.Series["Default"].BorderColor = Color.Black;
                    ChartDefect.Legends.Add("Legend1");
                    ChartDefect.Legends[0].Enabled = true;
                    ChartDefect.Legends[0].Docking = Docking.Bottom;
                    ChartDefect.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                }
                else
                {
                    ChartDefect.Series["Default"].IsValueShownAsLabel = true;
                }

                //Zoom 관련 속성
                ChartDefect.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;
                ChartDefect.ChartAreas["Default"].CursorX.AutoScroll = true;
                ChartDefect.ChartAreas["Default"].AxisX.ScrollBar.LineColor = Color.Black;
                ChartDefect.ChartAreas["Default"].AxisX.ScrollBar.Size = 17;
                ChartDefect.ChartAreas["Default"].AxisX.ScaleView.SmallScrollSize = double.NaN;
                ChartDefect.ChartAreas["Default"].AxisY.ScaleView.Zoomable = true;
                ChartDefect.ChartAreas["Default"].AxisX.ScaleView.ZoomReset(0);
                ChartDefect.ChartAreas["Default"].AxisY.ScaleView.ZoomReset(0);

                ChartDefect.ChartAreas["Default"].CursorX.IsUserSelectionEnabled = true;
                ChartDefect.ChartAreas["Default"].CursorY.IsUserSelectionEnabled = true;

                //X 축 관련 속성
                ChartDefect.ChartAreas["Default"].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                ChartDefect.ChartAreas["Default"].AxisX.Interval = 1;
                ChartDefect.ChartAreas["Default"].AxisX.IntervalOffset = 1;
                ChartDefect.ChartAreas["Default"].AxisX.IsLabelAutoFit = true;
                ChartDefect.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                    //| System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                    //| System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                                                                    | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep90;

                //ChartDefect.ChartAreas["Default"].AxisX.LabelStyle.Angle = 90;
                //ChartDefect.ChartAreas["Default"].AxisX.LabelStyle.Font = new System.Drawing.Font("굴림", 9);
                ChartDefect.AntiAliasing = AntiAliasingStyles.All;

                ChartDefect.ChartAreas["Default"].AxisX.MajorGrid.Enabled = false;
                ChartDefect.ChartAreas["Default"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                ChartDefect.ChartAreas["Default"].AxisX.LabelStyle.IsEndLabelVisible = true;
                ChartDefect.ChartAreas["Default"].AxisY.MajorGrid.Enabled = true;
                ChartDefect.ChartAreas["Default"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                ChartDefect.ChartAreas["Default"].BackColor = Color.WhiteSmoke;

                //Scale 관련 너무 큰 값에 대한 대안
                ChartDefect.ChartAreas["Default"].AxisY.ScaleBreakStyle.Enabled = false;
                if (map.Defects.Count > 100)
                {
                    ChartDefect.ChartAreas["Default"].AxisY.ScaleBreakStyle.Enabled = chkScaleBreaks.Checked;
                    ChartDefect.ChartAreas["Default"].AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
                    ChartDefect.ChartAreas["Default"].AxisY.ScaleBreakStyle.Spacing = 2;
                    ChartDefect.ChartAreas["Default"].AxisY.ScaleBreakStyle.LineWidth = 2;
                    ChartDefect.ChartAreas["Default"].AxisY.ScaleBreakStyle.LineColor = Color.Red;
                    ChartDefect.ChartAreas["Default"].AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 25;
                    ChartDefect.ChartAreas["Default"].AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Auto;
                }

            }
            finally
            {
                StatusMessage(null);
            }
        }

        private string Select(DataTable dt, string column, string value, string returnColumn = null)
        {
            if (dt == null || dt.Rows.Count == 0)
                return String.Empty;

            DataRow[] rows = dt.Select(String.Format("{0} = '{1}'", column, value));

            if (rows == null || rows.Length == 0)
                return String.Empty;

            if (!String.IsNullOrEmpty(returnColumn))
                return rows[0][returnColumn].ToString();

            return rows[0][0].ToString();
        }

        private SeriesChartType ChartTypeChanged(
           )
        {
            SeriesChartType charttype = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), cmbChartType.Value.ToString(), true);

            return charttype;
        }

        public Base.Defect[] GetSelectedDefect()
        {
            if (map.SelectedDefect.Count > 0)
                return map.SelectedDefect.ToArray();
            else
                return map.GetVisibleDefect().ToArray();
        }

        #endregion[ Method ]

        #region RD 관련 코드

        private void shot_ApplyButtonClick(object sender, EventArgs e)
        {
            long[] stepSeqArr = map.Defects.GetStepSeqArray();

            if (stepSeqArr == null || stepSeqArr.Length == 0)
                return;

            map.ShotArrayX = rdSetup.ShotArrayX;
            map.ShotArrayY = rdSetup.ShotArrayY;
            map.ShotStartX = rdSetup.ShotStartX;
            map.ShotStartY = rdSetup.ShotStartY;

            RepeatedDefect rd = new RepeatedDefect();
            rd.DieXSize = map.DieSizeX;
            rd.DieYSize = map.DieSizeY;
            rd.Tolerance = rdSetup.Tolerance;
            rd.RepeatCount = rdSetup.RepeatCount;
            rd.ShotStartX = rdSetup.ShotStartX;
            rd.ShotStartY = rdSetup.ShotStartY;
            rd.ShotArrayX = rdSetup.ShotArrayX;
            rd.ShotArrayY = rdSetup.ShotArrayY;
            rd.CalcBy = rdSetup.IsPointMode ? CalcBy.Point : CalcBy.Size;
            rd.Calculate(DmsCache.Instance[stepSeqArr[0]].Dies, map.Defects);

            map.Redraw();

            int rdCount = map.Defects.GetRDCount();
            int nonRd = map.Defects.Count - rdCount;

            rdSetup.SetChartData(nonRd, rdCount);
        }

        private void ultraDockManager_PaneDisplayed(object sender, Infragistics.Win.UltraWinDock.PaneDisplayedEventArgs e)
        {
            if (e.Pane.Control == Reclassify)
                return;

            if (e.Pane.Control == rdSetup)
            {
                if (map.MapType != MAP_TYPE.RD)
                    m_prevMapType = map.MapType;

                map.MapType = MAP_TYPE.RD;
            }
            else
            {
                map.MapType = m_prevMapType;
                map.DrawDefects = DefectMap.VIEW_ALL;
            }

            map.VisibleShot = (e.Pane.Control == rdSetup);
            map.Redraw();
        }

        #endregion

        #region [ Property ]

        public DACrux.Base.DPWafer[] Wafer
        {
            get;
            set;
        }

        #endregion [ Property ]

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet("Map");
            sheet.Add(map);
            e.SheetList.Add(sheet);

            sheet = new DACrux.Utility.ExcelSheet("Defect Chart and Images");
            sheet.Add(ChartDefect);

            List<System.Windows.Forms.Control> lsImages = new List<System.Windows.Forms.Control>();
            foreach (System.Windows.Forms.Control cr in imageList.Controls)
            {
                if (cr.Name == "DPUCDefectInfo")
                    lsImages.Add(cr);
            }

            sheet.Add();
            if (lsImages.Count > 0)
                sheet.Add(lsImages.ToArray());

            e.SheetList.Add(sheet);

            sheet = new DACrux.Utility.ExcelSheet("DataRow");
            sheet.SheetName = "DataRow";
            sheet.Add(map.Defects.ToDataTable());
            e.SheetList.Add(sheet);

            DACrux.Utility.ExcelExportManager.Export(e);
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
                dt.Columns.Add(new DataColumn("CLASS", typeof(int)));
                dt.Columns.Add(new DataColumn("CLUSTER", typeof(int)));
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

                foreach (Defect df in oDefect)
                {
                    DataRow dr = dt.NewRow();
                    dr["DEFECTID"] = df.DEFECTID;
                    dr["CLASS"] = df.CLASSNUMBER;
                    dr["CLUSTER"] = df.CLUSTERNUMBER;
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

        #endregion
    }
}
