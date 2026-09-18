using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DACrux.SEMDMS.RO;
using System.Collections.Generic;
using DACrux.Common.RO;


namespace DACrux.SEMDMS.Control
{
	/// <summary>
	/// DPUCDetailAnalysis에 대한 요약 설명입니다.
	/// </summary>
	public class DPUCRepeatedDefect : System.Windows.Forms.UserControl
	{
		string userId = "hhmstill";
		string selectedStepSeq = string.Empty;
		DataTable dtSizeColor = null;
		DataTable dtTypeColor = null;
		private FarPoint.Win.Spread.FpSpread fpSpreadWaferInfo;
		private FarPoint.Win.Spread.SheetView fpSpreadWaferInfo_Sheet;
		private System.Windows.Forms.RichTextBox richTextBoxInfo;
		//private SoftwareFX.ChartFX.Chart chart;
        private Chart chart;
		private FarPoint.Win.Spread.FpSpread fpSpreadDefect;
		private FarPoint.Win.Spread.SheetView fpSpreadDefect_Sheet;
		private DACrux.Map.DefectMap map;
		private FarPoint.Win.Spread.FpSpread fpSpreadColor;
		private FarPoint.Win.Spread.SheetView fpSpreadColor_Sheet;
		private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDetailAnalysisUnpinnedTabAreaLeft;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDetailAnalysisUnpinnedTabAreaRight;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDetailAnalysisUnpinnedTabAreaTop;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCDetailAnalysisUnpinnedTabAreaBottom;
		private Infragistics.Win.UltraWinDock.AutoHideControl _DPUCDetailAnalysisAutoHideControl;
		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow2;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow3;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow4;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow5;
		private System.Windows.Forms.Panel panelRepeatOption;
		private System.Windows.Forms.CheckBox checkBoxShot;
		private System.Windows.Forms.Button buttonRedraw;
		private System.Windows.Forms.TextBox textBoxRepeatCnt;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxTolerance;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components;

        private List<Color> RepeatDefectColor = null;
        private DataPoint selectedDataPoint = null;
        private List<string> RepeatDefectRel = null;

        private DataTable m_dtDefect = null;

		public DPUCRepeatedDefect()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

		}

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("74a429a9-0b55-49a0-bc72-4d1c2f1b14d6"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("dedf4593-2f1d-496f-a0b4-5d4bfd5328d6"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("74a429a9-0b55-49a0-bc72-4d1c2f1b14d6"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane2 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("ce86e187-e67b-47bf-bd24-ff74bdfc5eb5"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("74a429a9-0b55-49a0-bc72-4d1c2f1b14d6"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane3 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("1defe157-844f-4ce2-aca0-4825913156a7"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("74a429a9-0b55-49a0-bc72-4d1c2f1b14d6"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane4 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("6a43e0a8-5363-4862-abaf-0ea7742be8d5"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("74a429a9-0b55-49a0-bc72-4d1c2f1b14d6"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane5 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("90a7f01a-d6a3-412d-9386-f282f2279a65"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("74a429a9-0b55-49a0-bc72-4d1c2f1b14d6"), -1);
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.fpSpreadColor = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadColor_Sheet = new FarPoint.Win.Spread.SheetView();
            this.fpSpreadDefect = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadDefect_Sheet = new FarPoint.Win.Spread.SheetView();
            this.fpSpreadWaferInfo = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadWaferInfo_Sheet = new FarPoint.Win.Spread.SheetView();
            this.richTextBoxInfo = new System.Windows.Forms.RichTextBox();
            this.map = new DACrux.Map.DefectMap();
            this.ultraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDetailAnalysisUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDetailAnalysisUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCDetailAnalysisAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow2 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow3 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow4 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow5 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.panelRepeatOption = new System.Windows.Forms.Panel();
            this.checkBoxShot = new System.Windows.Forms.CheckBox();
            this.buttonRedraw = new System.Windows.Forms.Button();
            this.textBoxRepeatCnt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTolerance = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadColor_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadWaferInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadWaferInfo_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).BeginInit();
            this.windowDockingArea1.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            this.dockableWindow2.SuspendLayout();
            this.dockableWindow3.SuspendLayout();
            this.dockableWindow4.SuspendLayout();
            this.dockableWindow5.SuspendLayout();
            this.panelRepeatOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Location = new System.Drawing.Point(0, 18);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(197, 536);
            this.chart.TabIndex = 0;
            this.chart.Click += new System.EventHandler(this.chart_Click);
            this.chart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart_MouseClick);
            this.chart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart_MouseDown);
            this.chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart_MouseMove);
            this.chart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
            // 
            // fpSpreadColor
            // 
            this.fpSpreadColor.AccessibleDescription = "";
            this.fpSpreadColor.Location = new System.Drawing.Point(0, 18);
            this.fpSpreadColor.Name = "fpSpreadColor";
            this.fpSpreadColor.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadColor_Sheet});
            this.fpSpreadColor.Size = new System.Drawing.Size(260, 536);
            this.fpSpreadColor.TabIndex = 4;
            // 
            // fpSpreadColor_Sheet
            // 
            this.fpSpreadColor_Sheet.Reset();
            fpSpreadColor_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadColor_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadColor_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadColor_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadColor_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadColor_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadColor_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadColor_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadColor_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadColor_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadColor_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadColor_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadColor_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadColor_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadColor_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // fpSpreadDefect
            // 
            this.fpSpreadDefect.AccessibleDescription = "";
            this.fpSpreadDefect.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadDefect.Location = new System.Drawing.Point(0, 18);
            this.fpSpreadDefect.Name = "fpSpreadDefect";
            this.fpSpreadDefect.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadDefect.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadDefect_Sheet});
            this.fpSpreadDefect.Size = new System.Drawing.Size(95, 556);
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
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadDefect_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadDefect_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // fpSpreadWaferInfo
            // 
            this.fpSpreadWaferInfo.AccessibleDescription = "";
            this.fpSpreadWaferInfo.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadWaferInfo.Location = new System.Drawing.Point(0, 18);
            this.fpSpreadWaferInfo.Name = "fpSpreadWaferInfo";
            this.fpSpreadWaferInfo.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadWaferInfo.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadWaferInfo_Sheet});
            this.fpSpreadWaferInfo.Size = new System.Drawing.Size(95, 556);
            this.fpSpreadWaferInfo.TabIndex = 0;
            this.fpSpreadWaferInfo.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpSpreadWaferInfo_Sheet
            // 
            this.fpSpreadWaferInfo_Sheet.Reset();
            fpSpreadWaferInfo_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadWaferInfo_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadWaferInfo_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadWaferInfo_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWaferInfo_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWaferInfo_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWaferInfo_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadWaferInfo_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWaferInfo_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWaferInfo_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWaferInfo_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadWaferInfo_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWaferInfo_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWaferInfo_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWaferInfo_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadWaferInfo_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadWaferInfo_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadWaferInfo_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWaferInfo_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadWaferInfo_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.fpSpreadWaferInfo_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWaferInfo_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWaferInfo_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWaferInfo_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadWaferInfo_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpSpreadWaferInfo_Sheet.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSpreadWaferInfo_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWaferInfo_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWaferInfo_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWaferInfo_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadWaferInfo_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWaferInfo_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // richTextBoxInfo
            // 
            this.richTextBoxInfo.Location = new System.Drawing.Point(0, 18);
            this.richTextBoxInfo.Name = "richTextBoxInfo";
            this.richTextBoxInfo.ReadOnly = true;
            this.richTextBoxInfo.Size = new System.Drawing.Size(95, 556);
            this.richTextBoxInfo.TabIndex = 3;
            this.richTextBoxInfo.Text = "richTextBox1";
            this.richTextBoxInfo.WordWrap = false;
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DieBackgroundColor = System.Drawing.Color.White;
            this.map.DieBorderColor = System.Drawing.Color.LightGray;
            this.map.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.map.DieMaxX = 0;
            this.map.DieMaxY = 0;
            this.map.DieMinX = 0;
            this.map.DieMinY = 0;
            this.map.DieSizeX = 0.01D;
            this.map.DieSizeY = 0.01D;
            this.map.DisplayValue = "BIN";
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.map.Location = new System.Drawing.Point(202, 0);
            this.map.MapType = DACrux.Base.MAP_TYPE.REPEAT;
            this.map.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.map.Name = "map";
            this.map.NotchAngle = 0;
            this.map.NotchType = DACrux.Base.Notch.Flat;
            this.map.OriginDieBorder = System.Drawing.Color.Red;
            this.map.OriginIndexX = 0;
            this.map.OriginIndexY = 0;
            this.map.OriginX = 0D;
            this.map.OriginY = 0D;
            this.map.PickupDieAlpha = 96;
            this.map.PickupedDieColor = System.Drawing.Color.Transparent;
            this.map.PopupMenu = true;
            this.map.ReferenceDieSetting = 0;
            this.map.ScaleMark = false;
            this.map.SelecetedBin = "ALL";
            this.map.Size = new System.Drawing.Size(662, 555);
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 3;
            this.map.TransParent = 255;
            this.map.ViewAngle = 0;
            this.map.VisibleDieBorder = true;
            this.map.VisibleDieValue = false;
            this.map.VisibleFocusDie = false;
            this.map.VisibleInfomation = true;
            this.map.VisibleOffDie = false;
            this.map.VisibleProbeOverlay = false;
            this.map.VisibleStringBin = false;
            this.map.VisibleVIFail = false;
            this.map.VisibleXY = false;
            this.map.WaferBorderColor = System.Drawing.Color.LightGray;
            this.map.WaferColor = System.Drawing.Color.Gray;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.map.WaferMargin = 0.95D;
            this.map.WaferSize = 200000D;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            // 
            // ultraDockManager
            // 
            dockAreaPane1.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            dockableControlPane1.Control = this.chart;
            dockableControlPane1.FlyoutSize = new System.Drawing.Size(260, -1);
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(184, 24, 152, 136);
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "Chart";
            dockableControlPane2.Control = this.fpSpreadColor;
            dockableControlPane2.OriginalControlBounds = new System.Drawing.Rectangle(8, 288, 144, 100);
            dockableControlPane2.Size = new System.Drawing.Size(100, 100);
            dockableControlPane2.Text = "Color";
            dockableControlPane3.Control = this.fpSpreadDefect;
            dockableControlPane3.OriginalControlBounds = new System.Drawing.Rectangle(8, 152, 144, 128);
            dockableControlPane3.Size = new System.Drawing.Size(100, 100);
            dockableControlPane3.Text = "Defect";
            dockableControlPane4.Control = this.fpSpreadWaferInfo;
            dockableControlPane4.OriginalControlBounds = new System.Drawing.Rectangle(8, 8, 144, 136);
            dockableControlPane4.Size = new System.Drawing.Size(100, 100);
            dockableControlPane4.Text = "Wafer";
            dockableControlPane5.Control = this.richTextBoxInfo;
            dockableControlPane5.OriginalControlBounds = new System.Drawing.Rectangle(160, 152, 152, 128);
            dockableControlPane5.Size = new System.Drawing.Size(100, 100);
            dockableControlPane5.Text = "Info";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1,
            dockableControlPane2,
            dockableControlPane3,
            dockableControlPane4,
            dockableControlPane5});
            dockAreaPane1.Size = new System.Drawing.Size(197, 576);
            this.ultraDockManager.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.ultraDockManager.HostControl = this;
            // 
            // _DPUCDetailAnalysisUnpinnedTabAreaLeft
            // 
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Name = "_DPUCDetailAnalysisUnpinnedTabAreaLeft";
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 576);
            this._DPUCDetailAnalysisUnpinnedTabAreaLeft.TabIndex = 4;
            // 
            // _DPUCDetailAnalysisUnpinnedTabAreaRight
            // 
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Location = new System.Drawing.Point(864, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Name = "_DPUCDetailAnalysisUnpinnedTabAreaRight";
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 576);
            this._DPUCDetailAnalysisUnpinnedTabAreaRight.TabIndex = 5;
            // 
            // _DPUCDetailAnalysisUnpinnedTabAreaTop
            // 
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Name = "_DPUCDetailAnalysisUnpinnedTabAreaTop";
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.Size = new System.Drawing.Size(864, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaTop.TabIndex = 6;
            // 
            // _DPUCDetailAnalysisUnpinnedTabAreaBottom
            // 
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 576);
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Name = "_DPUCDetailAnalysisUnpinnedTabAreaBottom";
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.Size = new System.Drawing.Size(864, 0);
            this._DPUCDetailAnalysisUnpinnedTabAreaBottom.TabIndex = 7;
            // 
            // _DPUCDetailAnalysisAutoHideControl
            // 
            this._DPUCDetailAnalysisAutoHideControl.Location = new System.Drawing.Point(21, 0);
            this._DPUCDetailAnalysisAutoHideControl.Name = "_DPUCDetailAnalysisAutoHideControl";
            this._DPUCDetailAnalysisAutoHideControl.Owner = this.ultraDockManager;
            this._DPUCDetailAnalysisAutoHideControl.Size = new System.Drawing.Size(265, 576);
            this._DPUCDetailAnalysisAutoHideControl.TabIndex = 8;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Controls.Add(this.dockableWindow1);
            this.windowDockingArea1.Controls.Add(this.dockableWindow2);
            this.windowDockingArea1.Controls.Add(this.dockableWindow3);
            this.windowDockingArea1.Controls.Add(this.dockableWindow4);
            this.windowDockingArea1.Controls.Add(this.dockableWindow5);
            this.windowDockingArea1.Location = new System.Drawing.Point(0, 0);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.ultraDockManager;
            this.windowDockingArea1.Size = new System.Drawing.Size(202, 576);
            this.windowDockingArea1.TabIndex = 9;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.chart);
            this.dockableWindow1.Location = new System.Drawing.Point(0, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager;
            this.dockableWindow1.Size = new System.Drawing.Size(197, 556);
            this.dockableWindow1.TabIndex = 12;
            // 
            // dockableWindow2
            // 
            this.dockableWindow2.Controls.Add(this.fpSpreadColor);
            this.dockableWindow2.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow2.Name = "dockableWindow2";
            this.dockableWindow2.Owner = this.ultraDockManager;
            this.dockableWindow2.Size = new System.Drawing.Size(260, 556);
            this.dockableWindow2.TabIndex = 13;
            // 
            // dockableWindow3
            // 
            this.dockableWindow3.Controls.Add(this.fpSpreadDefect);
            this.dockableWindow3.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow3.Name = "dockableWindow3";
            this.dockableWindow3.Owner = this.ultraDockManager;
            this.dockableWindow3.Size = new System.Drawing.Size(95, 576);
            this.dockableWindow3.TabIndex = 14;
            // 
            // dockableWindow4
            // 
            this.dockableWindow4.Controls.Add(this.fpSpreadWaferInfo);
            this.dockableWindow4.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow4.Name = "dockableWindow4";
            this.dockableWindow4.Owner = this.ultraDockManager;
            this.dockableWindow4.Size = new System.Drawing.Size(95, 576);
            this.dockableWindow4.TabIndex = 15;
            // 
            // dockableWindow5
            // 
            this.dockableWindow5.Controls.Add(this.richTextBoxInfo);
            this.dockableWindow5.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow5.Name = "dockableWindow5";
            this.dockableWindow5.Owner = this.ultraDockManager;
            this.dockableWindow5.Size = new System.Drawing.Size(95, 576);
            this.dockableWindow5.TabIndex = 16;
            // 
            // panelRepeatOption
            // 
            this.panelRepeatOption.Controls.Add(this.checkBoxShot);
            this.panelRepeatOption.Controls.Add(this.buttonRedraw);
            this.panelRepeatOption.Controls.Add(this.textBoxRepeatCnt);
            this.panelRepeatOption.Controls.Add(this.label2);
            this.panelRepeatOption.Controls.Add(this.textBoxTolerance);
            this.panelRepeatOption.Controls.Add(this.label1);
            this.panelRepeatOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelRepeatOption.Location = new System.Drawing.Point(202, 555);
            this.panelRepeatOption.Name = "panelRepeatOption";
            this.panelRepeatOption.Size = new System.Drawing.Size(662, 21);
            this.panelRepeatOption.TabIndex = 11;
            // 
            // checkBoxShot
            // 
            this.checkBoxShot.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxShot.Location = new System.Drawing.Point(288, 0);
            this.checkBoxShot.Name = "checkBoxShot";
            this.checkBoxShot.Size = new System.Drawing.Size(56, 21);
            this.checkBoxShot.TabIndex = 5;
            this.checkBoxShot.Text = "Shot";
            this.checkBoxShot.CheckedChanged += new System.EventHandler(this.checkBoxShot_CheckedChanged);
            // 
            // buttonRedraw
            // 
            this.buttonRedraw.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRedraw.Location = new System.Drawing.Point(232, 0);
            this.buttonRedraw.Name = "buttonRedraw";
            this.buttonRedraw.Size = new System.Drawing.Size(56, 21);
            this.buttonRedraw.TabIndex = 4;
            this.buttonRedraw.Text = "Redraw";
            this.buttonRedraw.Click += new System.EventHandler(this.buttonRedraw_Click);
            // 
            // textBoxRepeatCnt
            // 
            this.textBoxRepeatCnt.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxRepeatCnt.Location = new System.Drawing.Point(208, 0);
            this.textBoxRepeatCnt.Name = "textBoxRepeatCnt";
            this.textBoxRepeatCnt.Size = new System.Drawing.Size(24, 21);
            this.textBoxRepeatCnt.TabIndex = 3;
            this.textBoxRepeatCnt.Text = "2";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(112, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Repeat Cnt >=";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxTolerance
            // 
            this.textBoxTolerance.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxTolerance.Location = new System.Drawing.Point(64, 0);
            this.textBoxTolerance.Name = "textBoxTolerance";
            this.textBoxTolerance.Size = new System.Drawing.Size(48, 21);
            this.textBoxTolerance.TabIndex = 1;
            this.textBoxTolerance.Text = "100000";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tolerance";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DPUCRepeatedDefect
            // 
            this.Controls.Add(this._DPUCDetailAnalysisAutoHideControl);
            this.Controls.Add(this.map);
            this.Controls.Add(this.panelRepeatOption);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._DPUCDetailAnalysisUnpinnedTabAreaTop);
            this.Controls.Add(this._DPUCDetailAnalysisUnpinnedTabAreaBottom);
            this.Controls.Add(this._DPUCDetailAnalysisUnpinnedTabAreaRight);
            this.Controls.Add(this._DPUCDetailAnalysisUnpinnedTabAreaLeft);
            this.Name = "DPUCRepeatedDefect";
            this.Size = new System.Drawing.Size(864, 576);
            this.Load += new System.EventHandler(this.DPUCDetailAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadColor_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadWaferInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadWaferInfo_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).EndInit();
            this.windowDockingArea1.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            this.dockableWindow2.ResumeLayout(false);
            this.dockableWindow3.ResumeLayout(false);
            this.dockableWindow4.ResumeLayout(false);
            this.dockableWindow5.ResumeLayout(false);
            this.panelRepeatOption.ResumeLayout(false);
            this.panelRepeatOption.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Map에 사용자 별로 defect color을 설정한다.
		void DefectColorSet()
		{
            DACrux.SEMDMS.RO.SEMConfiguration o = null;
			try
			{
				if(this.userId.Length == 0) this.userId = "Admin";
                o = new SEMConfiguration();
                dtSizeColor = o.SelectColorListByDftSize(this.userId);
				map.SizeColor = dtSizeColor;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		public void WaferInfo(string [] stepSeq)
		{
            DefectMapAnalysis o = null;
            DataTable dt = null;
            long[] arrSelectedStepSeq = null;
            try
            {
                o = new DefectMapAnalysis();

                arrSelectedStepSeq = new long[stepSeq.Length];

                for (int i = 0; i < stepSeq.Length; i++)
                {
                    arrSelectedStepSeq[i] = Convert.ToInt32(stepSeq[i]);
                }

                dt = o.GetStepInfo(arrSelectedStepSeq);
                
                // Column Order Change
                dt.Columns["WAFER_ID"].SetOrdinal(0);
                dt.Columns["LOT_ID"].SetOrdinal(0);

                fpSpreadWaferInfo_Sheet.DataSource = dt;
                DACrux.Utility.FPSpreadUtil.SetCellTypeToIntByDataTable(ref fpSpreadWaferInfo_Sheet, dt);
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadWaferInfo_Sheet);

                //FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
                //numberCellType.FixedPoint = false;
                //this.fpSpreadWaferInfo_Sheet.Columns.Get(0).CellType = numberCellType;
                //this.fpSpreadWaferInfo_Sheet.Columns.Get(1).CellType = numberCellType;

                fpSpreadWaferInfo_Sheet.Columns[2].Visible = false;
                fpSpreadWaferInfo_Sheet.Columns[3].Visible = false;

                //fpSpreadWaferInfo_Sheet.Columns.

				//DACrux.Utility.FPSpreadUtil.SpreadColumnFitSize(fpSpreadWaferInfo_Sheet);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
				dt = null;
				o = null;
			}
		}

		private void DPUCDetailAnalysis_Load(object sender, System.EventArgs e)
		{
			if(DesignMode) return;
			try
			{
				fpSpreadDefect_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode | FarPoint.Win.Spread.OperationMode.ReadOnly;
				fpSpreadColor_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode | FarPoint.Win.Spread.OperationMode.ReadOnly;

				DefectColorSet();

				fpSpreadWaferInfo_Sheet.AddSelection(0, 0, 1, fpSpreadWaferInfo_Sheet.ColumnCount);
                map.DefectSize = GetDefaultDefectSize();
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, this.Name);
			}
		}

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();

            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : DACrux.Map.DefectMap.DEFAULT_DEFECT_SIZE;
        }

		void StepSum(string stepSeq)
		{
            DefectMapAnalysis o = null;
			DataTable dt = null;
			System.Text.StringBuilder sb = null;
			try
			{
				if(stepSeq == null) return;
				if(stepSeq.Length == 0) return;

                o = new DefectMapAnalysis();
				dt = o.GetStepSum(DACrux.Base.Convert.longParse(stepSeq));
				sb = new System.Text.StringBuilder();
				for(int a = 0; a < dt.Columns.Count; a++)
				{
					sb.AppendFormat("{0}:{1}\r\n", dt.Columns[a].ColumnName, dt.Rows[0][a]);
				}

				richTextBoxInfo.Text = sb.ToString();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				o = null;
				if(dt != null) dt.Dispose();
				dt = null;
				sb = null;
			}
		}

		public void Function(string function)
		{
			try
			{
				string []selStep = SelectedWafer();

				StepSum(selStep[0]);

				switch(function)
				{
					case "radioButtonSize":
						DrawSize(selStep[0]);
						break;
					case "radioButtonClass":
						break;
					case "radioButtonCluster":
						break;
					case "radioButtonFineBin":
						break;
					case "radioButtonRoughbin":
						break;
					case "radioButtonZone":
						break;
					case "radioButtonDensity":
						break;
					case "radioButtonDieCum":
						break;
					case "radioButtonRepeater":
						DrawRepeater(selStep[0]);
						break;
				}
			}
			catch(Exception ex)
			{
				throw ex;				
			}
		}

		void DrawRepeater(string selStep)
		{
			string product = SelectedProduct();
			if(product == null) return;

			DataTable dtProductInfo = null;
			DataTable dtRepeatList = null;
			m_dtDefect = null;

			try
			{
                map.MapType = DACrux.Base.MAP_TYPE.REPEAT;
                DefectMapAnalysis o = new DefectMapAnalysis();
                SEMConfiguration oConfig = new SEMConfiguration();
                dtProductInfo = oConfig.GetInfo(product);
				int dieSizeX = DACrux.Base.Convert.intParse(string.Format("{0}", dtProductInfo.Rows[0]["DIE_PITCH_X"]));
				int dieSizeY = DACrux.Base.Convert.intParse(string.Format("{0}", dtProductInfo.Rows[0]["DIE_PITCH_Y"]));
				if(checkBoxShot.Checked)
					dtRepeatList = o.GetRepeatListByShot(dieSizeX, dieSizeY, DACrux.Base.Convert.intParse(textBoxTolerance.Text), DACrux.Base.Convert.intParse(selStep), product, DACrux.Base.Convert.intParse(textBoxRepeatCnt.Text));
				else
					dtRepeatList = o.GetRepeatListByDie(DACrux.Base.Convert.intParse(textBoxTolerance.Text), DACrux.Base.Convert.intParse(selStep), DACrux.Base.Convert.intParse(textBoxRepeatCnt.Text));
				DrawRepeaterType(dtRepeatList);
				DrawRepeaterChart(dtRepeatList);
                if (checkBoxShot.Checked)
                    DefectMapDraw.Draw(map, DACrux.Base.Convert.longParse(selStep), DACrux.Base.Convert.intParse(textBoxTolerance.Text), ref m_dtDefect, true, product, dieSizeX, dieSizeY);
                else
                    DefectMapDraw.Draw(map, DACrux.Base.Convert.longParse(selStep), DACrux.Base.Convert.intParse(textBoxTolerance.Text), ref m_dtDefect, false, product, dieSizeX, dieSizeY);
				fpSpreadDefect_Sheet.DataSource = m_dtDefect;
                DACrux.Utility.FPSpreadUtil.SetCellTypeToIntByDataTable(ref fpSpreadDefect_Sheet, m_dtDefect);
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadDefect_Sheet);

                fpSpreadDefect_Sheet.Columns[0].Visible = false;
                fpSpreadDefect_Sheet.Columns[2].Visible = false;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dtProductInfo != null) dtProductInfo.Dispose();
				dtProductInfo = null;
				if(dtRepeatList != null) dtRepeatList.Dispose();
				dtRepeatList = null;
                //if(m_dtDefect != null) m_dtDefect.Dispose();
                //m_dtDefect = null;
			}
		}

		void DrawRepeaterChart(DataTable dtRepeatList)
		{
			DataTable dt = null;
			try
			{
				dt = new DataTable("CHART");
				dt.Columns.Add("REPEAT_REL", typeof(string));
				dt.Columns.Add("CNT", typeof(int));
				for(int a = 0; a < dtRepeatList.Rows.Count; a++)
				{
					object [] obj = new object[2];
					obj[0] = string.Format("X:{0}\nY:{1}", dtRepeatList.Rows[a]["REPEAT_XREL"], dtRepeatList.Rows[a]["REPEAT_YREL"]);
					obj[1] = dtRepeatList.Rows[a]["CNT"];
					dt.Rows.Add(obj);
				}
                chart.Series.Clear();
                chart.Series.Add("Series" + 0);
                int[] cnt = new int[dt.Rows.Count];
                string[] str = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cnt[i] = DACrux.Base.Convert.intParse(dt.Rows[i][1].ToString());
                    str[i] = dt.Rows[i][0].ToString();
                }
                chart.Series["Series" + 0].Points.DataBindXY(str, cnt);
                chart.Series["Series" + 0].IsValueShownAsLabel = true;
                //chart.ChartAreas["ChartArea1"].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;
                chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
				//chart.DataSource = dt;
				ChartColor();
                DefectColor();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
				dt = null;
			}
		}

		void DrawRepeaterType(DataTable dtType)
		{
			DataTable dt = null;
            SEMConfiguration o = null;
			DataTable dtColor = null;

			try
			{
				dt = new DataTable("REPEAT TYPE");
				for(int a = 0; a < dtType.Columns.Count; a++)
				{
					dt.Columns.Add(dtType.Columns[a].ColumnName, dtType.Columns[a].DataType);
				}
				dt.Columns.Add("COLOR", typeof(string));

                o = new SEMConfiguration();
                dtColor = o.SelectColorListByDftSize(this.userId);
				if(dtColor.Rows.Count < dtType.Rows.Count)
				{
					dtColor.Dispose();
					dtColor = null;
                    dtColor = o.SelectColorListByDftSize("admin");
				}

				for(int a = 0; a < dtType.Rows.Count; a++)
				{
					object [] obj = new object[dtType.Columns.Count + 1];
					for(int b = 0; b < dtType.Columns.Count; b++)
					{
						obj[b] = dtType.Rows[a][b];
					}
					obj[dtType.Columns.Count] = dtColor.Rows[a % dtColor.Rows.Count]["COLOR"];
					dt.Rows.Add(obj);
				}

				fpSpreadColor_Sheet.Reset();
				fpSpreadColor_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
				fpSpreadColor_Sheet.DataSource = dt;
                DACrux.Utility.FPSpreadUtil.SetCellTypeToIntByDataTable(ref fpSpreadColor_Sheet, dt);
				SpreadBackColor(fpSpreadColor_Sheet, 3);
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadColor_Sheet);

				//map.RepeatColor = dt;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
				dt = null;
				if(dtColor != null) dtColor.Dispose();
				dtColor = null;
				o = null;
			}
		}

		int SpreadColumnNo(FarPoint.Win.Spread.SheetView sv, string columnName)
		{
			for(int a = 0; a < sv.Columns.Count; a++)
			{
				if(sv.Columns[a].Label.Equals(columnName)) return a;
			}
			return -1;
		}

		string SelectedProduct()
		{
			FarPoint.Win.Spread.Model.CellRange [] cr = null;
			try
			{
				cr = fpSpreadWaferInfo_Sheet.GetSelections();
				if(cr == null || cr.Length == 0) return null;
				int row = cr[0].Row;
				int col = SpreadColumnNo(fpSpreadWaferInfo_Sheet, "PRODUCT");

				if(row == -1 || col == -1) return null;

				return string.Format("{0}", fpSpreadWaferInfo_Sheet.Cells[row, col].Value);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cr = null;
			}
		}

		string[] SelectedWafer()
		{
			FarPoint.Win.Spread.Model.CellRange [] cr = null;
			
			try
			{
				cr = fpSpreadWaferInfo_Sheet.GetSelections();
				if(cr == null || cr.Length == 0) return null;

				int selectedRowCnt = 0;
				for(int a = 0; a < cr.Length; a++) selectedRowCnt += cr[a].RowCount;

				string [] stepSeq = new string[selectedRowCnt];

				selectedRowCnt = 0;
				for(int i = 0; i < cr.Length; i++)
				{
					for(int a = cr[i].Row; a < cr[i].Row + cr[i].RowCount; a++)
					{
						stepSeq[selectedRowCnt++] = string.Format("{0}", fpSpreadWaferInfo_Sheet.Cells[a, 2].Value);
					}
				}

				return stepSeq;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cr = null;
			}
		}

		void DrawSize(string selStep)
		{
			DataTable defectDt = null;
			try
			{
				if(!this.selectedStepSeq.Equals(selStep))
				{
					DefectMapDraw.DrawNoRedraw(map, DACrux.Base.Convert.longParse(selStep), ref defectDt);
					fpSpreadDefect_Sheet.DataSource = defectDt;
					this.selectedStepSeq = selStep;
				}

                map.MapType = DACrux.Base.MAP_TYPE.SIZE;
				map.Redraw();

				fpSpreadColor_Sheet.Reset();
				fpSpreadColor_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
				fpSpreadColor_Sheet.DataSource = dtSizeColor;
                DACrux.Utility.FPSpreadUtil.SetCellTypeToIntByDataTable(ref fpSpreadColor_Sheet, dtSizeColor);
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadColor_Sheet);
				SpreadBackColor(fpSpreadColor_Sheet, 2);
                //DACrux.Utility.FPSpreadUtil.SpreadColumnFitSize(fpSpreadColor_Sheet);

				ChartDrawBySize();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(defectDt != null) defectDt.Dispose();
				defectDt = null;
			}
		}

		void ChartDraw(string column)
		{
            DefectMapAnalysis o = null;
			DataTable dt = null;
            string[] arrSplit = null;
            long[] arrSelectedStepSeq = null;
			try
			{
                o = new DefectMapAnalysis();
				//dt = o.GetDefectDataCnt(this.selectedStepSeq.Split(','), column);
                arrSplit = selectedStepSeq.Split(',');
                arrSelectedStepSeq = new long[arrSplit.Length];

                for(int i = 0 ; i < arrSplit.Length; i++)
                {
                    arrSelectedStepSeq[i] = Convert.ToInt32(arrSplit[i]);
                }

                dt = o.GetDefectDataCnt(arrSelectedStepSeq, column);
				chart.DataSource = dt;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
				dt = null;
			}
		}

		void ChartDrawBySize()
		{
			DataTable dt = null;
			try
			{
                if (map.Defects.Count == 0) return;

				dt = new DataTable();
				dt.Columns.Add("SIZE", System.Type.GetType("System.String"));
				dt.Columns.Add("COUNT", System.Type.GetType("System.Int32"));

				int [] defectCnt = new int[this.dtSizeColor.Rows.Count + 1];
				for(int i = 0; i < defectCnt.Length; i++) defectCnt[i] = 0;

				int sizeIdx = -1;
                for (int i = 0; i < map.Defects.Count; i++)
				{
                    sizeIdx = SizeIndex(map.Defects[i].DSIZE);
					if(sizeIdx == -1) defectCnt[defectCnt.Length - 1]++;
					else defectCnt[sizeIdx]++;
				}

				int from = 0;
				int to = 0;
				for(int i = 0; i < this.dtSizeColor.Rows.Count; i++)
				{
                    from = System.Convert.ToInt32(dtSizeColor.Rows[i]["SIZE_FROM"]);
                    to = System.Convert.ToInt32(dtSizeColor.Rows[i]["SIZE_TO"]);
                    //from = (int)(decimal)dtSizeColor.Rows[i]["SIZE_FROM"];
                    //to = (int)(decimal)dtSizeColor.Rows[i]["SIZE_TO"];
					dt.Rows.Add(new object [] {
												  string.Format("{0}~{1}", from, to)
												  , defectCnt[i]
											  });
				}
				dt.Rows.Add(new object [] {
											  "Undefine"
											  , defectCnt[defectCnt.Length - 1]
										  });
				chart.DataSource = dt;
				ChartColor();
                DefectColor();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
				dt = null;
			}
		}

        void ChartColor()
        {
            try
            {
                RepeatDefectColor = new List<Color>();
                RepeatDefectRel = new List<string>();

                int col = SpreadColumnNo(fpSpreadColor_Sheet, "COLOR");
                for (int i = 0; i < fpSpreadColor_Sheet.RowCount; i++)
                {
                    //chart.Point[0, i].Color = fpSpreadColor_Sheet.Cells[i, col].BackColor;
                    chart.Series[0].Points[i].Color = fpSpreadColor_Sheet.Cells[i, col].BackColor;
                    
                    // Repeat Defect Adding
                    RepeatDefectRel.Add(fpSpreadColor_Sheet.Cells[i, 0].Value.ToString() + "_" + fpSpreadColor_Sheet.Cells[i, 1].Value.ToString());
                    RepeatDefectColor.Add(fpSpreadColor_Sheet.Cells[i, col].BackColor);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void DefectColor()
        {
            try
            {
                map.SetRepeatDefectRel(RepeatDefectRel);
                map.SetRepeatDefectColor(RepeatDefectColor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		int SizeIndex(double dsize)
		{
			dsize *= 1000000;
			int from = 0;
			int to = 0;
			for(int i = 0; i < this.dtSizeColor.Rows.Count; i++)
			{
                from = System.Convert.ToInt32(dtSizeColor.Rows[i]["SIZE_FROM"]);
                to = System.Convert.ToInt32(dtSizeColor.Rows[i]["SIZE_TO"]);
                //from = (int)(decimal)dtSizeColor.Rows[i]["SIZE_FROM"];
                //to = (int)(decimal)dtSizeColor.Rows[i]["SIZE_TO"];
				if(dsize >= from && dsize <= to) return i;
			}
			return this.dtSizeColor.Rows.Count;
		}

		void SpreadBackColor(FarPoint.Win.Spread.SheetView sv, int colNo)
		{
			string color = string.Empty;
			int r = 0;
			int g = 0;
			int b = 0;
			for(int i = 0; i < sv.RowCount; i++)
			{
				color = (string)sv.Cells[i, colNo].Value;
				r = DACrux.Base.Convert.intParse(color.Substring(0, 3));
				g = DACrux.Base.Convert.intParse(color.Substring(3, 3));
				b = DACrux.Base.Convert.intParse(color.Substring(6, 3));
				sv.Cells[i, colNo].BackColor = Color.FromArgb(r, g, b);
			}
		}

		private void checkBoxShot_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				string[] stepSeq = SelectedWafer();
				if(stepSeq == null) return;
				if(stepSeq.Length == 0) return;
				DrawRepeater(stepSeq[0]);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void buttonRedraw_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				Function("radioButtonRepeater");
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, this.Name);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

        
        private void chart_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (selectedDataPoint != null)
                {
                    selectedDataPoint.IsValueShownAsLabel = false;

                    selectedDataPoint = null;

                    chart.Invalidate();

                    chart.Cursor = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }


        private void chart_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                HitTestResult hitresult = chart.HitTest(e.X, e.Y);
                selectedDataPoint = null;
                if (hitresult.ChartElementType == ChartElementType.DataPoint)
                {
                    selectedDataPoint = (DataPoint)hitresult.Object;
                    selectedDataPoint.IsValueShownAsLabel = true;
                    chart.Cursor = Cursors.SizeNS;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                HitTestResult result = chart.HitTest(e.X, e.Y);
                string SeriesPoint = string.Empty;

                if (result.ChartElementType == ChartElementType.PlottingArea)
                {
                    for (int p = 0; p < chart.Series.Count; p++)
                    {

                        for (int s = 0; s < chart.Series[p].Points.Count; s++)
                        {
                            chart.Series[p].Points[s].BackHatchStyle = ChartHatchStyle.None;
                        }
                    }
                }

                else if
                (result.ChartElementType == ChartElementType.DataPoint ||
                result.ChartElementType == ChartElementType.LegendItem)
                {
                    SeriesPoint = result.Series.Name.ToString();
                    DataPoint tooltipPoint = chart.Series[SeriesPoint].Points[result.PointIndex];

                    tooltipPoint.ToolTip =
                               "Repeat Rel : " + chart.Series[SeriesPoint].Points[result.PointIndex].AxisLabel.Replace("\n","/") + "\n"
                               
                               + "Cnt: " + chart.Series[SeriesPoint].Points[result.PointIndex].YValues[0].ToString();

                    for (int k = 0; k < chart.Series.Count; k++)
                    {
                        if (!chart.Series[k].Name.Equals(SeriesPoint))
                        {

                            for (int s = 0; s < chart.Series[k].Points.Count; s++)
                            {
                                chart.Series[k].Points[s].BackHatchStyle = ChartHatchStyle.Percent70;
                            }
                        }
                        else
                        {
                            for (int s = 0; s < chart.Series[k].Points.Count; s++)
                            {
                                chart.Series[k].Points[s].BackHatchStyle = ChartHatchStyle.None;
                                chart.Series[k].Points[s].BorderWidth = 1;
                                chart.Series[k].Points[s].MarkerSize = 20;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void chart_Click(object sender, EventArgs e)
        {
            
        }

        private void chart_MouseClick(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //if (!mode.Equals("BIN")) return;
                HitTestResult result = chart.HitTest(e.X, e.Y);
                string SeriesPoint = string.Empty;

                for (int i = 0; i < chart.Series.Count; i++)
                {
                    if
                    (result.ChartElementType == ChartElementType.DataPoint ||
                    result.ChartElementType == ChartElementType.LegendItem)
                    {
                        SeriesPoint = result.Series.Name.ToString();
                        DataPoint point2 = chart.Series[i].Points[result.PointIndex];
                        DataPoint tooltipPoint = chart.Series[SeriesPoint].Points[result.PointIndex];

                        tooltipPoint.ToolTip = chart.Series[SeriesPoint].YValueMembers.ToString() + "\n" +
                            chart.Series[SeriesPoint].Points[result.PointIndex].AxisLabel + "\n" +
                            Math.Round(chart.Series[SeriesPoint].Points[result.PointIndex].YValues[0], 0);
                    }
                }

                string[] selStep = SelectedWafer();


                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    if (result.PointIndex > -1)
                    {
                        string XRel = string.Empty;
                        string YRel = string.Empty;
                        XRel = chart.Series[SeriesPoint].Points[result.PointIndex].AxisLabel.Replace("X:","").Replace("Y:","").Split('\n')[0];
                        YRel = chart.Series[SeriesPoint].Points[result.PointIndex].AxisLabel.Replace("X:","").Replace("Y:","").Split('\n')[1];
                        DataRow[] arrDr = m_dtDefect.Select(string.Format("REPEAT_XREL = {0} AND REPEAT_YREL = {1}", XRel, YRel));

                        DataTable dt = m_dtDefect.Clone();
                        
                        foreach(DataRow dr in arrDr)
                        {
                            dt.Rows.Add(dr.ItemArray);
                        }
                        
                        DefectColor();
                        DefectMapDraw.DrawNoRedraw(map, DACrux.Base.Convert.longParse(selStep[0]), ref dt);

                        map.Redraw();
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    StepSum(selStep[0]);
                    DrawRepeater(selStep[0]);
                }
                //Redraw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.Source);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
	}
}
