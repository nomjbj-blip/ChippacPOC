using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization.Charting.Utilities;
using System.Collections.Generic;
using DACrux.Base;
using DACrux.Common.RO;
using DACrux.Map;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// DPUCDetailAnalysis에 대한 요약 설명입니다.
    /// </summary>
    /// 

    public class DPUCDefectSourceAnalysis : DACrux.Framework.Base.DACruxCTLBasic01, DACrux.Framework.Base.ISendDefect, DACrux.Framework.Base.IExportExcel
    {
        #region 멤버 변수

        public static readonly int DEFAULT_TOLERANCE = 50;
        public static readonly string MISSING = "Missing_";

        enum Step { Factory, StepSeq, StepID, Count }

        private FarPoint.Win.Spread.FpSpread fpSpreadDefect;
        private FarPoint.Win.Spread.SheetView fpSpreadDefect_Sheet;
        private DACrux.Map.DefectMap map;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDetailAnalysisUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDetailAnalysisUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDetailAnalysisUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDetailAnalysisUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.AutoHideControl _DPUCDetailAnalysisAutoHideControl;
        private System.Windows.Forms.Panel panelDsaControl;
        private Chart chart;
        private Label label1;
        private ListView lstStep;
        private Panel panel1;
        private Label label2;
        private Label label3;
        private Panel panel2;
        private Button BtnDown;
        private Button BtnUp;
        private System.ComponentModel.IContainer components = null;
        private Panel panel3;
        private DataTable dtDefectOri = null;
        private DataTable dtDefect = null;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private TabPage tabPage3;
        private Panel panel5;
        private Panel panel4;
        private FlowLayoutPanel ImageList;
        private Label label4;
        private CheckBox chkDefectImage;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter2;
        private RadioButton rdoSize;
        private RadioButton rdoPoint;
        private NumericUpDown numTolerance;
        private Button btnRedraw;

        List<string> lsPointName = new List<string>();
        List<Color> lsPointColor = new List<Color>();
        private CheckBox chkScaleBreaks;
        private CheckBox chkMissing;
        DefectSourceAnalysis m_dsa;

        #endregion

        #region 생성자 및 Load/Closing 이벤트 메서드

        public DPUCDefectSourceAnalysis()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();

            // TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

        }

        private void DPUCDetailAnalysis_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;

            fpSpreadDefect_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode | FarPoint.Win.Spread.OperationMode.ReadOnly;
            DefectColorSet();
            ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);

            SettingData setting = new SettingData(GetType());
            rdoPoint.Checked = setting.GetValue<bool>(rdoPoint.Name, true);
            rdoSize.Checked = setting.GetValue<bool>(rdoSize.Name, false);
            numTolerance.Value = setting.GetValue<int>(numTolerance.Name, DEFAULT_TOLERANCE);
            chkMissing.Checked = setting.GetValue<bool>(chkMissing.Name, true);
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingData setting = new SettingData(GetType());
            setting.SetValue(rdoPoint.Name, rdoPoint.Checked);
            setting.SetValue(rdoSize.Name, rdoSize.Checked);
            setting.SetValue(numTolerance.Name, numTolerance.Value);
            setting.SetValue(chkMissing.Name, chkMissing.Checked);
            setting.Save();
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

        #region 구성 요소 디자이너에서 생성한 코드
        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPUCDefectSourceAnalysis));
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.fpSpreadDefect = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadDefect_Sheet = new FarPoint.Win.Spread.SheetView();
            this.map = new DACrux.Map.DefectMap();
            this.ultraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDetailAnalysisUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDetailAnalysisUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDetailAnalysisAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.panelDsaControl = new System.Windows.Forms.Panel();
            this.btnRedraw = new System.Windows.Forms.Button();
            this.numTolerance = new System.Windows.Forms.NumericUpDown();
            this.rdoSize = new System.Windows.Forms.RadioButton();
            this.rdoPoint = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lstStep = new System.Windows.Forms.ListView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnDown = new System.Windows.Forms.Button();
            this.BtnUp = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkMissing = new System.Windows.Forms.CheckBox();
            this.chkScaleBreaks = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ultraSplitter2 = new Infragistics.Win.Misc.UltraSplitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chkDefectImage = new System.Windows.Forms.CheckBox();
            this.ImageList = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).BeginInit();
            this.panelDsaControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(223)))), ((int)(((byte)(240)))));
            this.chart.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            this.chart.BackSecondaryColor = System.Drawing.Color.White;
            this.chart.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            this.chart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chart.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea1.Area3DStyle.Inclination = 15;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.PointGapDepth = 0;
            chartArea1.Area3DStyle.Rotation = 5;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.TruncatedLabels = true;
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(165)))), ((int)(((byte)(191)))), ((int)(((byte)(228)))));
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.Name = "Default";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 75F;
            chartArea1.Position.Width = 90F;
            chartArea1.Position.X = 2F;
            chartArea1.Position.Y = 13F;
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(3, 31);
            this.chart.Name = "chart";
            this.chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            this.chart.Size = new System.Drawing.Size(1129, 202);
            this.chart.TabIndex = 11;
            this.chart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart_MouseClick);
            this.chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ChartDefect_MouseMove);
            // 
            // fpSpreadDefect
            // 
            this.fpSpreadDefect.AccessibleDescription = "";
            this.fpSpreadDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadDefect.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadDefect.Location = new System.Drawing.Point(3, 3);
            this.fpSpreadDefect.Name = "fpSpreadDefect";
            this.fpSpreadDefect.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadDefect.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadDefect_Sheet});
            this.fpSpreadDefect.Size = new System.Drawing.Size(1129, 230);
            this.fpSpreadDefect.TabIndex = 2;
            this.fpSpreadDefect.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpSpreadDefect_Sheet
            // 
            this.fpSpreadDefect_Sheet.Reset();
            fpSpreadDefect_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadDefect_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadDefect_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadDefect_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.DataAutoCellTypes = false;
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
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDefect_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DieBackgroundColor = System.Drawing.Color.White;
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
            this.map.Location = new System.Drawing.Point(0, 28);
            this.map.MapType = DACrux.Base.MAP_TYPE.DSA;
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
            this.map.Size = new System.Drawing.Size(1143, 292);
            this.map.SizeColor = null;
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 3;
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
            this.map.WaferColor = System.Drawing.Color.Gray;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.map.WaferID = "";
            this.map.WaferMargin = 0.95D;
            this.map.WaferSize = 200000D;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.map.OnSelectedDefect += new DACrux.Map.SelectedDefect(this.map_OnSelectedDefect);
            // 
            // ultraDockManager
            // 
            this.ultraDockManager.HostControl = this;
            this.ultraDockManager.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.VisualStudio2005;
            // 
            // _DPUCDetailAnalysisUnpinnedTabAreaLeft
            // 
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Name = "_DPUCDetailAnalysisUnpinnedTabAreaLeft";
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 592);
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.TabIndex = 4;
            // 
            // _DPUCDetailAnalysisUnpinnedTabAreaRight
            // 
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Location = new System.Drawing.Point(1143, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Name = "_DPUCDetailAnalysisUnpinnedTabAreaRight";
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 592);
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.TabIndex = 5;
            // 
            // _DPUCDetailAnalysisUnpinnedTabAreaTop
            // 
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Name = "_DPUCDetailAnalysisUnpinnedTabAreaTop";
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Size = new System.Drawing.Size(1143, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.TabIndex = 6;
            // 
            // _DPUCDetailAnalysisUnpinnedTabAreaBottom
            // 
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 592);
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Name = "_DPUCDetailAnalysisUnpinnedTabAreaBottom";
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1143, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.TabIndex = 7;
            // 
            // _DPUCDetailAnalysisAutoHideControl
            // 
            this._DPUCDetailAnalysisAutoHideControl.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._DPUCDetailAnalysisAutoHideControl.Location = new System.Drawing.Point(21, 0);
            this._DPUCDetailAnalysisAutoHideControl.Name = "_DPUCDetailAnalysisAutoHideControl";
            this._DPUCDetailAnalysisAutoHideControl.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisAutoHideControl.Size = new System.Drawing.Size(265, 576);
            this._DPUCDetailAnalysisAutoHideControl.TabIndex = 8;
            // 
            // panelDsaControl
            // 
            this.panelDsaControl.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelDsaControl.Controls.Add(this.btnRedraw);
            this.panelDsaControl.Controls.Add(this.numTolerance);
            this.panelDsaControl.Controls.Add(this.rdoSize);
            this.panelDsaControl.Controls.Add(this.rdoPoint);
            this.panelDsaControl.Controls.Add(this.label1);
            this.panelDsaControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDsaControl.Location = new System.Drawing.Point(0, 0);
            this.panelDsaControl.Name = "panelDsaControl";
            this.panelDsaControl.Size = new System.Drawing.Size(1143, 28);
            this.panelDsaControl.TabIndex = 10;
            // 
            // btnRedraw
            // 
            this.btnRedraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedraw.BackColor = System.Drawing.Color.White;
            this.btnRedraw.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRedraw.Image = ((System.Drawing.Image)(resources.GetObject("btnRedraw.Image")));
            this.btnRedraw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRedraw.Location = new System.Drawing.Point(1048, 2);
            this.btnRedraw.Name = "btnRedraw";
            this.btnRedraw.Size = new System.Drawing.Size(92, 24);
            this.btnRedraw.TabIndex = 218;
            this.btnRedraw.Text = "     Search";
            this.btnRedraw.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRedraw.UseVisualStyleBackColor = false;
            this.btnRedraw.Click += new System.EventHandler(this.btnRedraw_Click);
            // 
            // numTolerance
            // 
            this.numTolerance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numTolerance.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTolerance.Location = new System.Drawing.Point(946, 4);
            this.numTolerance.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTolerance.Name = "numTolerance";
            this.numTolerance.Size = new System.Drawing.Size(84, 21);
            this.numTolerance.TabIndex = 9;
            this.numTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // rdoSize
            // 
            this.rdoSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoSize.AutoSize = true;
            this.rdoSize.Location = new System.Drawing.Point(815, 6);
            this.rdoSize.Name = "rdoSize";
            this.rdoSize.Size = new System.Drawing.Size(48, 16);
            this.rdoSize.TabIndex = 7;
            this.rdoSize.TabStop = true;
            this.rdoSize.Text = "Size";
            this.rdoSize.UseVisualStyleBackColor = true;
            // 
            // rdoPoint
            // 
            this.rdoPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoPoint.AutoSize = true;
            this.rdoPoint.Checked = true;
            this.rdoPoint.Location = new System.Drawing.Point(761, 6);
            this.rdoPoint.Name = "rdoPoint";
            this.rdoPoint.Size = new System.Drawing.Size(51, 16);
            this.rdoPoint.TabIndex = 7;
            this.rdoPoint.TabStop = true;
            this.rdoPoint.Text = "Point";
            this.rdoPoint.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(876, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "Tolerance";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstStep
            // 
            this.lstStep.CheckBoxes = true;
            this.lstStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstStep.Location = new System.Drawing.Point(0, 28);
            this.lstStep.Name = "lstStep";
            this.lstStep.Size = new System.Drawing.Size(722, 202);
            this.lstStep.TabIndex = 14;
            this.lstStep.UseCompatibleStateImageBehavior = false;
            this.lstStep.View = System.Windows.Forms.View.List;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnDown);
            this.panel2.Controls.Add(this.BtnUp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(722, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(40, 202);
            this.panel2.TabIndex = 16;
            // 
            // BtnDown
            // 
            this.BtnDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDown.Image = ((System.Drawing.Image)(resources.GetObject("BtnDown.Image")));
            this.BtnDown.Location = new System.Drawing.Point(3, 107);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(34, 95);
            this.BtnDown.TabIndex = 0;
            this.BtnDown.UseVisualStyleBackColor = true;
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // BtnUp
            // 
            this.BtnUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUp.Image = ((System.Drawing.Image)(resources.GetObject("BtnUp.Image")));
            this.BtnUp.Location = new System.Drawing.Point(3, 6);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(34, 95);
            this.BtnUp.TabIndex = 0;
            this.BtnUp.UseVisualStyleBackColor = true;
            this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(762, 28);
            this.panel1.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(762, 28);
            this.label2.TabIndex = 0;
            this.label2.Text = "     Step List";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1129, 28);
            this.label3.TabIndex = 1;
            this.label3.Text = "     DSA Chart";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.map);
            this.panel3.Controls.Add(this.ultraSplitter1);
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Controls.Add(this.panelDsaControl);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1143, 592);
            this.panel3.TabIndex = 17;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 320);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 231;
            this.ultraSplitter1.Size = new System.Drawing.Size(1143, 10);
            this.ultraSplitter1.TabIndex = 14;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 330);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1143, 262);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkMissing);
            this.tabPage1.Controls.Add(this.chkScaleBreaks);
            this.tabPage1.Controls.Add(this.chart);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1135, 236);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DSA Chart";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkMissing
            // 
            this.chkMissing.AutoSize = true;
            this.chkMissing.Location = new System.Drawing.Point(136, 10);
            this.chkMissing.Name = "chkMissing";
            this.chkMissing.Size = new System.Drawing.Size(105, 16);
            this.chkMissing.TabIndex = 13;
            this.chkMissing.Text = "Show Missing";
            this.chkMissing.UseVisualStyleBackColor = true;
            this.chkMissing.CheckedChanged += new System.EventHandler(this.chkMissing_CheckedChanged);
            // 
            // chkScaleBreaks
            // 
            this.chkScaleBreaks.AutoSize = true;
            this.chkScaleBreaks.Location = new System.Drawing.Point(290, 9);
            this.chkScaleBreaks.Name = "chkScaleBreaks";
            this.chkScaleBreaks.Size = new System.Drawing.Size(99, 16);
            this.chkScaleBreaks.TabIndex = 12;
            this.chkScaleBreaks.Text = "Scale Breaks";
            this.chkScaleBreaks.UseVisualStyleBackColor = true;
            this.chkScaleBreaks.Visible = false;
            this.chkScaleBreaks.CheckedChanged += new System.EventHandler(this.chkScaleBreaks_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel5);
            this.tabPage2.Controls.Add(this.ultraSplitter2);
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1135, 236);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Step List && Image List";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lstStep);
            this.panel5.Controls.Add(this.panel2);
            this.panel5.Controls.Add(this.panel1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(762, 230);
            this.panel5.TabIndex = 15;
            // 
            // ultraSplitter2
            // 
            this.ultraSplitter2.BackColor = System.Drawing.Color.Transparent;
            this.ultraSplitter2.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraSplitter2.Location = new System.Drawing.Point(765, 3);
            this.ultraSplitter2.Name = "ultraSplitter2";
            this.ultraSplitter2.RestoreExtent = 361;
            this.ultraSplitter2.Size = new System.Drawing.Size(6, 230);
            this.ultraSplitter2.TabIndex = 17;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chkDefectImage);
            this.panel4.Controls.Add(this.ImageList);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(771, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(361, 230);
            this.panel4.TabIndex = 16;
            // 
            // chkDefectImage
            // 
            this.chkDefectImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDefectImage.Checked = true;
            this.chkDefectImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDefectImage.Location = new System.Drawing.Point(251, 3);
            this.chkDefectImage.Name = "chkDefectImage";
            this.chkDefectImage.Size = new System.Drawing.Size(105, 21);
            this.chkDefectImage.TabIndex = 13;
            this.chkDefectImage.Text = "Include Image";
            // 
            // ImageList
            // 
            this.ImageList.AutoScroll = true;
            this.ImageList.AutoSize = true;
            this.ImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ImageList.Location = new System.Drawing.Point(0, 25);
            this.ImageList.Name = "ImageList";
            this.ImageList.Size = new System.Drawing.Size(361, 205);
            this.ImageList.TabIndex = 11;
            this.ImageList.WrapContents = false;
            this.ImageList.SizeChanged += new System.EventHandler(this.ImageList_SizeChanged);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(361, 25);
            this.label4.TabIndex = 12;
            this.label4.Text = "Defect Image List";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.fpSpreadDefect);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1135, 236);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Raw Data";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // DPUCDefectSourceAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.Controls.Add(this._DPUCDetailAnalysisAutoHideControl);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this._DPUCDetailAnalysisUnpinnedTabAreaTop);
            this.Controls.Add(this._DPUCDetailAnalysisUnpinnedTabAreaBottom);
            this.Controls.Add(this._DPUCDetailAnalysisUnpinnedTabAreaRight);
            this.Controls.Add(this._DPUCDetailAnalysisUnpinnedTabAreaLeft);
            this.Name = "DPUCDefectSourceAnalysis";
            this.Size = new System.Drawing.Size(1143, 592);
            this.Load += new System.EventHandler(this.DPUCDetailAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).EndInit();
            this.panelDsaControl.ResumeLayout(false);
            this.panelDsaControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region 사용자 정의 메서드

        private List<KeyValuePair<string, long>> GetStepList()
        {
            List<KeyValuePair<string, long>> list = new List<KeyValuePair<string, long>>();

            foreach (ListViewItem item in lstStep.CheckedItems)
            {
                string stepID = item.SubItems[(int)Step.StepID].Text;
                long stepSeq = DACrux.Base.Convert.longParse(item.SubItems[(int)Step.StepSeq].Text);
                list.Add(new KeyValuePair<string, long>(stepID, stepSeq));
            }

            return list;
        }

        public void WaferInfo(long[] stepSeqArr)
        {
            try
            {
                StatusMessage("데이터를 조회중입니다.");
                // Map 바인딩
                DrawMap(stepSeqArr);
                // Step 리스트 바인딩
                BindingStepList(stepSeqArr);

                btnRedraw.PerformClick();
            }
            finally
            {
                StatusMessage(null);
            }
        }

        private void GetDefectImagePathAsync(object state)
        {
            long[] stepSeqArr = state as long[];

            RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
            DataTable dt = obj.GetDefectImagePath01(stepSeqArr);
            UpdateDefectImageInfo(dt);
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

        private void DrawMap(long[] stepSeqArr)
        {
            // Map 설정
            DACrux.SEMDMS.RO.DefectMapAnalysis obj = new DACrux.SEMDMS.RO.DefectMapAnalysis();

            // Defect 데이터 조회
            map.DefectClear();

            RO.DefectMapAnalysis oDMapAnalysis = new RO.DefectMapAnalysis();
            DataSet dsDefect = oDMapAnalysis.GetDefectMapViewer_Info(stepSeqArr);

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

            DataTable dtWaferMap = oDMapAnalysis.GetDefectMapViewer_Map(stepSeqArr, strSetupArr, strTestArr);

            // DEFECT IMAGE 경로를 비동기 조회
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GetDefectImagePathAsync), stepSeqArr);

            map.Defects.AddRange(obj.GetDefectMapViewer_DefectArray(stepSeqArr));
            map.SetDefaultColors();

            map.DieBackgroundColor = Color.Black;
            map.MapType = Base.MAP_TYPE.DSA;
            map.VisibleImageMark = false;

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
                map.ShotArrayX = DACrux.Base.Convert.intParse(stepRow["ST_XCNT"].ToString(), 1);
                map.ShotArrayY = DACrux.Base.Convert.intParse(stepRow["ST_YCNT"].ToString(), 1);
                map.ShotStartX = DACrux.Base.Convert.intParse(stepRow["ST_START_X"].ToString(), 1);
                map.ShotStartY = DACrux.Base.Convert.intParse(stepRow["ST_START_Y"].ToString(), 1);
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

            map.SetInfomation(DefectMapDraw.GetMapDescription(map.Defects));
            UserConfiguration.SetMapInformation(map, dsDefect);
            UserConfiguration.SetMapColor(map);

            map.WaferDrawMode = DACrux.Map.MapMode.Fit;
            map.DefectSelectMode();
        }

        private void BindingStepList(long[] stepSeqArr)
        {
            // Step 정보 조회 및 바인딩
            lstStep.Clear();
            lstStep.Items.Clear();
            lstStep.Columns.Clear();
            lstStep.View = View.Details;
            lstStep.FullRowSelect = true;
            lstStep.CheckBoxes = true;

            lstStep.Columns.Add("FACTORY", 80, HorizontalAlignment.Center);
            lstStep.Columns.Add("STEP_SEQ", 0, HorizontalAlignment.Center);
            lstStep.Columns.Add("STEP_ID", 100, HorizontalAlignment.Center);
            lstStep.Columns.Add("DEFECT", 100, HorizontalAlignment.Center);

            RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
            DataTable dt = obj.GetInspInfo02(stepSeqArr);

            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(new string[]
                        {
                            row[(int)Step.Factory].ToString(), 
                            row[(int)Step.StepSeq].ToString(), 
                            row[(int)Step.StepID].ToString(), 
                            row[(int)Step.Count].ToString()
                        });

                item.Checked = true;
                lstStep.Items.Add(item);
            }
        }

        private void DrawChart()
        {
            lsPointName.Clear();
            lsPointColor.Clear();

            //chart 관련 Initialize
            chart.Legends.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add("Default");
            chart.Series.Clear();

            if (m_dsa == null)
                return;

            // 전체 Step에 대한 Series 추가
            foreach (DSAResult result in m_dsa.Results)
            {
                Series series = chart.Series.Add(result.StepID);

                //series.SmartLabelStyle.Enabled = true;
                series.BorderWidth = 2;
                series.ChartType = SeriesChartType.StackedColumn;
                series.IsValueShownAsLabel = true;
            }

            // Missing Series 추가
            if (chkMissing.Checked)
            {
                foreach (DSAResult result in m_dsa.Missings)
                {
                    Series series = chart.Series.Add(MISSING + result.StepID);

                    //series.SmartLabelStyle.Enabled = true;
                    series.BorderWidth = 2;
                    series.ChartType = SeriesChartType.StackedColumn;
                    series.IsValueShownAsLabel = true;
                    series.IsVisibleInLegend = false;
                }
            }

            // 각 Series 별 DataPoint 추가
            foreach (DSAResult result in m_dsa.Results)
            {
                foreach (DSAItem item in result)
                {
                    DataPoint dp = new DataPoint(chart.Series[result.StepID]);

                    if (item.DefectList.Count > 0)
                    {
                        //Tag = STEP_ID _ STEP_ID_DUPLE
                        dp.Tag = string.Format("{0}_{1}", result.StepID, item.StepID);
                        dp.ToolTip = String.Format("{0} : {1:N0}", item.StepID, item.DefectList.Count);
                        dp.BorderWidth = 2;
                        dp.SetValueXY(item.StepID, item.DefectList.Count);
                        chart.Series[result.StepID].Points.Add(dp);
                    }
                    else
                    {
                        dp.SetValueXY(item.StepID, double.NaN);
                        chart.Series[result.StepID].Points.Add(dp);
                    }
                }
            }

            if (chkMissing.Checked)
            {
                // 각 Series 별 DataPoint 추가
                foreach (DSAResult result in m_dsa.Missings)
                {
                    foreach (DSAItem item in result)
                    {
                        DataPoint dp = new DataPoint(chart.Series[result.StepID]);

                        if (item.DefectList.Count > 0)
                        {
                            //Tag = STEP_ID _ STEP_ID_DUPLE
                            dp.Tag = string.Format("{0}_{1}", MISSING + result.StepID, item.StepID);
                            dp.ToolTip = String.Format("{0} : {1:N0}", item.StepID, -item.DefectList.Count);
                            dp.BorderWidth = 2;
                            dp.SetValueXY(item.StepID, -item.DefectList.Count);
                            chart.Series[MISSING + result.StepID].Points.Add(dp);
                        }
                        else
                        {
                            dp.SetValueXY(item.StepID, double.NaN);
                            chart.Series[MISSING + result.StepID].Points.Add(dp);
                        }
                    }
                }
            }

            chart.ApplyPaletteColors();

            foreach (Series series in chart.Series)
            {
                foreach (DataPoint dp in series.Points)
                {
                    if (dp.Tag == null)
                        continue;

                    lsPointName.Add((string)dp.Tag);
                    lsPointColor.Add(dp.Color);
                }
            }

            map.SetDSAColor(lsPointColor.ToArray());
            map.Redraw();

            if (chkMissing.Checked)
            {
                int half = chart.Series.Count / 2;
                for (int i = 0; i < half; i++)
                    chart.Series[half + i].Color = chart.Series[i].Color;
            }

            //X 축 관련 속성
            chart.ChartAreas["Default"].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chart.ChartAreas["Default"].AxisX.Interval = 1;
            chart.ChartAreas["Default"].AxisX.IntervalOffset = 1;
            chart.ChartAreas["Default"].AxisX.IsLabelAutoFit = true;
            chart.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont
                                                                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
                                                                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels
                                                                | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30;
            chart.ChartAreas["Default"].AxisX.LabelStyle.IsEndLabelVisible = true;

            chart.ChartAreas["Default"].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas["Default"].AxisY.MajorGrid.Enabled = true;
            chart.ChartAreas["Default"].AxisY.MajorGrid.LineColor = Color.Gray;
            chart.ChartAreas["Default"].AxisY.IsStartedFromZero = true;

            Legend legend = chart.Legends.Add("Legend1");
            legend.Enabled = true;
            legend.Docking = Docking.Bottom;
            legend.Alignment = System.Drawing.StringAlignment.Center;
            legend.LegendItemOrder = LegendItemOrder.SameAsSeriesOrder;

            //Zoom 관련 속성
            chart.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas["Default"].CursorX.AutoScroll = true;
            chart.ChartAreas["Default"].AxisX.ScrollBar.LineColor = Color.Black;
            chart.ChartAreas["Default"].AxisX.ScrollBar.Size = 17;
            chart.ChartAreas["Default"].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart.ChartAreas["Default"].AxisY.ScaleView.Zoomable = true;
            chart.ChartAreas["Default"].AxisX.ScaleView.ZoomReset();
            chart.ChartAreas["Default"].AxisY.ScaleView.ZoomReset();

            chart.ChartAreas["Default"].AxisY.ScaleBreakStyle.Enabled = chkScaleBreaks.Checked;
            chart.ChartAreas["Default"].AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chart.ChartAreas["Default"].AxisY.ScaleBreakStyle.Spacing = 2;
            chart.ChartAreas["Default"].AxisY.ScaleBreakStyle.LineWidth = 2;
            chart.ChartAreas["Default"].AxisY.ScaleBreakStyle.LineColor = Color.Red;
            chart.ChartAreas["Default"].AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 10;
            chart.ChartAreas["Default"].AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Auto;

            //chart.ChartAreas["Default"].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

            chart.ChartAreas["Default"].CursorX.IsUserSelectionEnabled = true;
            chart.ChartAreas["Default"].CursorY.IsUserSelectionEnabled = true;

            chart.DataBind();
            
            foreach (var result in m_dsa.Results)
            {
                foreach (var item in result)
                {
                    foreach (var defect in item.DefectList)
                    {
                        int idx = lsPointName.IndexOf(String.Format("{0}_{1}", result.StepID, item.StepID));
                        defect.DSA = String.Format("{0}", idx >= 0 ? idx : 0);
                    }
                }
            }

            foreach (var result in m_dsa.Missings)
            {
                foreach (var item in result)
                {
                    foreach (var defect in item.DefectList)
                    {
                        int idx = lsPointName.IndexOf(String.Format("{0}_{1}", MISSING + result.StepID, item.StepID));
                        string str = String.Format("{0}", idx >= 0 ? idx : 0);
                        defect.DSA = String.IsNullOrEmpty(defect.DSA) ? str : String.Join(",", new string[] { defect.DSA, str });
                    }
                }
            }
        }

        private void ControlAdd(DPUCDefectInfo container)
        {

            if (ImageList.InvokeRequired)
            {
                ImageList.BeginInvoke(new MethodInvoker(
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
                container.Width = ImageList.Width - SystemInformation.VerticalScrollBarWidth - container.Margin.Left - container.Margin.Right;
                ImageList.Controls.Add(container);
                ImageList.ResumeLayout();
            }
        }

        public Base.Defect[] GetSelectedDefect()
        {
            if (map.SelectedDefect.Count > 0)
                return map.SelectedDefect.ToArray();
            else
                return map.GetVisibleDefect().ToArray();
        }

        void DefectColorSet()
        {
            map.SetDSAColor(Color.LimeGreen, Color.Blue, Color.Red);
        }

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(lstStep);
            sheet.Add();
            sheet.Add(chart);
            sheet.Add();
            sheet.Add(map);
            sheet.Add();

            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Map n Chart";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(chart);

            List<System.Windows.Forms.Control> lsImages = new List<System.Windows.Forms.Control>();
            foreach (System.Windows.Forms.Control cr in ImageList.Controls)
            {
                if (cr.Name == "DPUCDefectInfo")
                    lsImages.Add(cr);
            }

            sheet.Add();
            if (lsImages.Count > 0)
                sheet.Add(lsImages.ToArray());

            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Defect Chart n Images";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.SheetName = "DataRow";
            sheet.Add((DataTable)fpSpreadDefect.DataSource);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "DataRow";

            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion

        #region 이벤트 처리 메서드

        private void BtnUp_Click(object sender, EventArgs e)
        {
            int dir = -1;

            foreach (ListViewItem lvi in lstStep.SelectedItems)
            {
                int index = lvi.Index + dir;
                if (index >= lstStep.Items.Count)
                    index = 0;
                else if (index < 0)
                    index = lstStep.Items.Count + dir;

                lstStep.Items.RemoveAt(lvi.Index);
                lstStep.Items.Insert(index, lvi);
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            int dir = 1;

            foreach (ListViewItem lvi in lstStep.SelectedItems)
            {
                int index = lvi.Index + dir;
                if (index >= lstStep.Items.Count)
                    index = 0;
                else if (index < 0)
                    index = lstStep.Items.Count + dir;

                lstStep.Items.RemoveAt(lvi.Index);
                lstStep.Items.Insert(index, lvi);
            }
        }

        private void btnRedraw_Click(object sender, System.EventArgs e)
        {
            try
            {
                StatusMessage("DSA 계산중입니다.");

                // 선택한 Step 리스트 가져오기
                List<KeyValuePair<string, long>> stepList = GetStepList();
                CalcBy calcBy = rdoPoint.Checked ? CalcBy.Point : CalcBy.Size;

                m_dsa = new DefectSourceAnalysis();
                m_dsa.Calculate(calcBy, (int)numTolerance.Value, map.Defects, stepList);


                DrawChart();
            }
            finally
            {
                StatusMessage(null);
            }
        }

        private void chart_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult result = chart.HitTest(e.X, e.Y);
            List<DataPoint> list = new List<DataPoint>();

            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                list.Add(result.Series.Points[result.PointIndex]);
            }
            else if (result.ChartElementType == ChartElementType.AxisLabels && result.Axis.AxisName == AxisName.X && result.Object is CustomLabel)
            {
                foreach (Series series in chart.Series)
                {
                    if (series.Points.Count > (result.Object as CustomLabel).FromPosition)
                        list.Add(series.Points[(int)(result.Object as CustomLabel).FromPosition]);
                }
            }

            if (list.Count > 0)
            {
                List<string> values = new List<string>();

                if (ModifierKeys == Keys.Shift || ModifierKeys == Keys.ShiftKey)
                {
                    string[] strVal = map.DrawDefects.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (strVal != null && strVal.Length > 0)
                        values.AddRange(strVal);

                    foreach (var pt in list)
                    {
                        if (pt.Tag == null)
                            continue;

                        int idx = lsPointName.IndexOf(pt.Tag.ToString());
                        idx = idx < 0 ? 0 : idx;
                        values.Add(idx.ToString());
                    }

                    if (!values.Contains("VAL"))
                        map.DrawDefects = String.Join(",", values);
                }
                else
                {
                    foreach (var pt in list)
                    {
                        if (pt.Tag == null)
                            continue;

                        int idx = lsPointName.IndexOf(pt.Tag.ToString());
                        idx = idx < 0 ? 0 : idx;
                        values.Add(idx.ToString());
                    }

                    map.DrawDefects = String.Join(",", values);
                }
            }
            else
            {
                map.DrawDefects = "ALL";
            }

            map.Redraw();
        }

        private void ChartDefect_MouseMove(object sender, MouseEventArgs e)
        {
            // Call Hit Test Method
            HitTestResult result = chart.HitTest(e.X, e.Y);

            try
            {
                if (chart.Legends.Count <= 0)
                    return;

                //Control Key 를 누르고 있을 경우 여러 Point 를 선택
                if (ModifierKeys == Keys.Shift || ModifierKeys == Keys.ShiftKey)
                    return;

                // Reset Data Point Attributes
                foreach (Series sVal in chart.Series)
                {
                    foreach (DataPoint point in sVal.Points)
                    {
                        point.BackSecondaryColor = Color.Black;
                        point.BackHatchStyle = ChartHatchStyle.None;
                        point.BorderWidth = 1;
                    }
                }

                List<DataPoint> list = new List<DataPoint>();

                // If a Data Point or a Legend item is selected.
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    list.Add(result.Series.Points[result.PointIndex]);
                }
                else if (result.ChartElementType == ChartElementType.AxisLabels && result.Axis.AxisName == AxisName.X && result.Object is CustomLabel)
                {
                    foreach (Series series in chart.Series)
                    {
                        if (series.Points.Count > (result.Object as CustomLabel).FromPosition)
                            list.Add(series.Points[(int)(result.Object as CustomLabel).FromPosition]);
                    }
                }

                if (list.Count > 0)
                {
                    Cursor = Cursors.Hand;

                    foreach (var point in list)
                    {
                        point.BackSecondaryColor = Color.White;
                        point.BackHatchStyle = ChartHatchStyle.Percent25;
                        point.BorderWidth = 3;
                    }
                }
                else
                {
                    // Set default cursor
                    this.Cursor = Cursors.Default;
                }
            }
            finally
            { }
        }

        private void map_OnSelectedDefect(object sender, Defect[] oDefect)
        {
            DPUCDefectInfo oForm = null;

            try
            {
                if (tabControl1.SelectedTab == tabPage3)
                {
                    DefectList list = new DefectList();
                    list.AddRange(oDefect);
                    fpSpreadDefect_Sheet.DataSource = list.ToDataTable();
                }
                else if (tabControl1.SelectedTab == tabPage2 && chkDefectImage.Checked)
                {
                    if (ImageList.Controls.Count > 0)
                        ImageList.Controls.Clear();

                    for (int i = 0; i < oDefect.Length; i++)
                    {
                        foreach (var image in oDefect[i].Images)
                        {
                            System.Threading.Thread.Sleep(10);

                            try
                            {
                                oForm = new DPUCDefectInfo(oDefect[i].DEFECTID.ToString(), image.IMAGEPATH, oDefect[i].XINDEX.ToString(), oDefect[i].YINDEX.ToString(),
                                    oDefect[i].CLASSNUMBER.ToString(), oDefect[i].CLUSTERNUMBER.ToString(), oDefect[i].XREL.ToString(),
                                    oDefect[i].YREL.ToString(), oDefect[i].XSIZE.ToString(), oDefect[i].YSIZE.ToString());

                                ControlAdd(oForm);
                            }
                            catch { }

                            StatusMessage(string.Format("Defect Image 확인중..({0}/{1})", i, oDefect.Length));
                        }
                    }

                    ImageList.Refresh();
                }
            }
            finally
            {
                StatusMessage(null);
            }
        }

        private void ImageList_SizeChanged(object sender, EventArgs e)
        {
            ImageList.SuspendLayout();
            foreach (System.Windows.Forms.Control ctrl in ImageList.Controls)
            {

                ctrl.Height = ctrl.Height + (ctrl.Margin.Top + ctrl.Margin.Bottom);
                ctrl.Width = ImageList.Width - 30;
            }
            ImageList.ResumeLayout();
        }

        private void chkScaleBreaks_CheckedChanged(object sender, EventArgs e)
        {
            DrawChart();
        }

        private void chkMissing_CheckedChanged(object sender, EventArgs e)
        {
            DrawChart();
        }

        #endregion
    }
}
