using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DACrux.Common.RO;

namespace DACrux.SEMDMS.Control
{
	/// <summary>
	/// DPUCZoneMap에 대한 요약 설명입니다.
	/// </summary>
	public class DPUCZoneMap : System.Windows.Forms.UserControl
	{
		private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCZoneMapUnpinnedTabAreaLeft;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCZoneMapUnpinnedTabAreaRight;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCZoneMapUnpinnedTabAreaTop;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DPUCZoneMapUnpinnedTabAreaBottom;
		private Infragistics.Win.UltraWinDock.AutoHideControl _DPUCZoneMapAutoHideControl;
		//private SoftwareFX.ChartFX.Chart chart1;

        private Chart chart1;
		private FarPoint.Win.Spread.FpSpread fpSpreadZone;
		private FarPoint.Win.Spread.SheetView fpSpreadZone_Sheet;
		private FarPoint.Win.Spread.FpSpread fpSpreadDefect;
		private FarPoint.Win.Spread.SheetView fpSpreadDefect_Sheet;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow2;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow3;
		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea4;
		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea2;
		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
		private DACrux.Map.DefectMap map;
		private System.ComponentModel.IContainer components;

		public DPUCZoneMap()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.
            map.DefectSize = GetDefaultDefectSize();
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
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("1b610dee-e32b-498f-925b-b4d473bf7c3a"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("815f8010-dce8-485d-a37d-b57f2275f0bd"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("1b610dee-e32b-498f-925b-b4d473bf7c3a"), -1);
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane2 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("d99a095f-ee7c-4962-bb1e-b22a109e04b3"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane2 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("599575a1-92f1-4112-ba64-c161d8b05f4f"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("d99a095f-ee7c-4962-bb1e-b22a109e04b3"), -1);
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane3 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("0b129316-9b68-4256-994e-79c19b5c7564"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane3 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("a8da6c9c-5488-41ec-8775-d98c872d2352"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("0b129316-9b68-4256-994e-79c19b5c7564"), -1);
            this.fpSpreadDefect = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadDefect_Sheet = new FarPoint.Win.Spread.SheetView();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.fpSpreadZone = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadZone_Sheet = new FarPoint.Win.Spread.SheetView();
            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._DPUCZoneMapUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCZoneMapUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCZoneMapUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCZoneMapUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DPUCZoneMapAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow2 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow3 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.windowDockingArea4 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.windowDockingArea2 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.map = new DACrux.Map.DefectMap();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadZone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadZone_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
            this.dockableWindow1.SuspendLayout();
            this.dockableWindow2.SuspendLayout();
            this.dockableWindow3.SuspendLayout();
            this.windowDockingArea4.SuspendLayout();
            this.windowDockingArea2.SuspendLayout();
            this.windowDockingArea1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fpSpreadDefect
            // 
            this.fpSpreadDefect.About = "4.0.2001.2005";
            this.fpSpreadDefect.AccessibleDescription = "";
            this.fpSpreadDefect.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadDefect.Location = new System.Drawing.Point(0, 18);
            this.fpSpreadDefect.Name = "fpSpreadDefect";
            this.fpSpreadDefect.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadDefect.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadDefect_Sheet});
            this.fpSpreadDefect.Size = new System.Drawing.Size(95, 502);
            this.fpSpreadDefect.TabIndex = 7;
            this.fpSpreadDefect.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpSpreadDefect_Sheet
            // 
            this.fpSpreadDefect_Sheet.Reset();
            this.fpSpreadDefect_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadDefect_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadDefect_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadDefect_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadDefect_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadDefect_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDefect_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDefect_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadDefect_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDefect_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(0, 18);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(95, 502);
            this.chart1.TabIndex = 0;
            // 
            // fpSpreadZone
            // 
            this.fpSpreadZone.About = "4.0.2001.2005";
            this.fpSpreadZone.AccessibleDescription = "";
            this.fpSpreadZone.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadZone.Location = new System.Drawing.Point(0, 18);
            this.fpSpreadZone.Name = "fpSpreadZone";
            this.fpSpreadZone.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadZone.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadZone_Sheet});
            this.fpSpreadZone.Size = new System.Drawing.Size(113, 502);
            this.fpSpreadZone.TabIndex = 6;
            this.fpSpreadZone.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpSpreadZone_Sheet
            // 
            this.fpSpreadZone_Sheet.Reset();
            this.fpSpreadZone_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadZone_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadZone_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadZone_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadZone_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadZone_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadZone_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadZone_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadZone_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadZone_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadZone_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadZone_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadZone_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadZone_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadZone_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadZone_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadZone_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadZone_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadZone_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadZone_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadZone_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadZone_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadZone_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ultraDockManager1
            // 
            dockAreaPane1.DockedBefore = new System.Guid("d99a095f-ee7c-4962-bb1e-b22a109e04b3");
            dockableControlPane1.Control = this.fpSpreadDefect;
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(96, 232, 200, 100);
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "fpSpread2";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(95, 520);
            dockAreaPane2.DockedBefore = new System.Guid("0b129316-9b68-4256-994e-79c19b5c7564");
            dockableControlPane2.Control = this.chart1;
            dockableControlPane2.OriginalControlBounds = new System.Drawing.Rectangle(472, 56, 144, 224);
            dockableControlPane2.Size = new System.Drawing.Size(100, 100);
            dockableControlPane2.Text = "chart1";
            dockAreaPane2.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane2});
            dockAreaPane2.Size = new System.Drawing.Size(95, 520);
            dockableControlPane3.Control = this.fpSpreadZone;
            dockableControlPane3.OriginalControlBounds = new System.Drawing.Rectangle(56, 48, 200, 100);
            dockableControlPane3.Size = new System.Drawing.Size(100, 100);
            dockableControlPane3.Text = "fpSpread1";
            dockAreaPane3.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane3});
            dockAreaPane3.Size = new System.Drawing.Size(113, 520);
            this.ultraDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1,
            dockAreaPane2,
            dockAreaPane3});
            this.ultraDockManager1.HostControl = this;
            // 
            // _DPUCZoneMapUnpinnedTabAreaLeft
            // 
            this._DPUCZoneMapUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._DPUCZoneMapUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._DPUCZoneMapUnpinnedTabAreaLeft.Name = "_DPUCZoneMapUnpinnedTabAreaLeft";
            this._DPUCZoneMapUnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
            this._DPUCZoneMapUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 520);
            this._DPUCZoneMapUnpinnedTabAreaLeft.TabIndex = 0;
            // 
            // _DPUCZoneMapUnpinnedTabAreaRight
            // 
            this._DPUCZoneMapUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._DPUCZoneMapUnpinnedTabAreaRight.Location = new System.Drawing.Point(800, 0);
            this._DPUCZoneMapUnpinnedTabAreaRight.Name = "_DPUCZoneMapUnpinnedTabAreaRight";
            this._DPUCZoneMapUnpinnedTabAreaRight.Owner = this.ultraDockManager1;
            this._DPUCZoneMapUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 520);
            this._DPUCZoneMapUnpinnedTabAreaRight.TabIndex = 1;
            // 
            // _DPUCZoneMapUnpinnedTabAreaTop
            // 
            this._DPUCZoneMapUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._DPUCZoneMapUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._DPUCZoneMapUnpinnedTabAreaTop.Name = "_DPUCZoneMapUnpinnedTabAreaTop";
            this._DPUCZoneMapUnpinnedTabAreaTop.Owner = this.ultraDockManager1;
            this._DPUCZoneMapUnpinnedTabAreaTop.Size = new System.Drawing.Size(800, 0);
            this._DPUCZoneMapUnpinnedTabAreaTop.TabIndex = 2;
            // 
            // _DPUCZoneMapUnpinnedTabAreaBottom
            // 
            this._DPUCZoneMapUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._DPUCZoneMapUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 520);
            this._DPUCZoneMapUnpinnedTabAreaBottom.Name = "_DPUCZoneMapUnpinnedTabAreaBottom";
            this._DPUCZoneMapUnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
            this._DPUCZoneMapUnpinnedTabAreaBottom.Size = new System.Drawing.Size(800, 0);
            this._DPUCZoneMapUnpinnedTabAreaBottom.TabIndex = 3;
            // 
            // _DPUCZoneMapAutoHideControl
            // 
            this._DPUCZoneMapAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._DPUCZoneMapAutoHideControl.Name = "_DPUCZoneMapAutoHideControl";
            this._DPUCZoneMapAutoHideControl.Owner = this.ultraDockManager1;
            this._DPUCZoneMapAutoHideControl.Size = new System.Drawing.Size(0, 0);
            this._DPUCZoneMapAutoHideControl.TabIndex = 4;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.fpSpreadDefect);
            this.dockableWindow1.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(95, 520);
            this.dockableWindow1.TabIndex = 16;
            // 
            // dockableWindow2
            // 
            this.dockableWindow2.Controls.Add(this.chart1);
            this.dockableWindow2.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow2.Name = "dockableWindow2";
            this.dockableWindow2.Owner = this.ultraDockManager1;
            this.dockableWindow2.Size = new System.Drawing.Size(95, 520);
            this.dockableWindow2.TabIndex = 17;
            // 
            // dockableWindow3
            // 
            this.dockableWindow3.Controls.Add(this.fpSpreadZone);
            this.dockableWindow3.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow3.Name = "dockableWindow3";
            this.dockableWindow3.Owner = this.ultraDockManager1;
            this.dockableWindow3.Size = new System.Drawing.Size(113, 520);
            this.dockableWindow3.TabIndex = 18;
            // 
            // windowDockingArea4
            // 
            this.windowDockingArea4.Controls.Add(this.dockableWindow1);
            this.windowDockingArea4.Dock = System.Windows.Forms.DockStyle.Right;
            this.windowDockingArea4.Location = new System.Drawing.Point(700, 0);
            this.windowDockingArea4.Name = "windowDockingArea4";
            this.windowDockingArea4.Owner = this.ultraDockManager1;
            this.windowDockingArea4.Size = new System.Drawing.Size(100, 520);
            this.windowDockingArea4.TabIndex = 11;
            // 
            // windowDockingArea2
            // 
            this.windowDockingArea2.Controls.Add(this.dockableWindow2);
            this.windowDockingArea2.Dock = System.Windows.Forms.DockStyle.Right;
            this.windowDockingArea2.Location = new System.Drawing.Point(600, 0);
            this.windowDockingArea2.Name = "windowDockingArea2";
            this.windowDockingArea2.Owner = this.ultraDockManager1;
            this.windowDockingArea2.Size = new System.Drawing.Size(100, 520);
            this.windowDockingArea2.TabIndex = 13;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Controls.Add(this.dockableWindow3);
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Right;
            this.windowDockingArea1.Location = new System.Drawing.Point(482, 0);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.ultraDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(118, 520);
            this.windowDockingArea1.TabIndex = 14;
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DieBackgroundColor = System.Drawing.Color.Black;
            this.map.DieBorderColor = System.Drawing.Color.LightGray;
            this.map.DieMaxX = 0;
            this.map.DieMaxY = 0;
            this.map.DieMinX = 0;
            this.map.DieMinY = 0;
            this.map.DieSizeX = 0.01;
            this.map.DieSizeY = 0.01;
            this.map.DisplayValue = "BIN";
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.DrawFirstDie = true;
            this.map.DrawMarkDie = false;
            this.map.DrawOriginDie = true;
            this.map.DrawSkipDie = true;
            this.map.EdgeColor = System.Drawing.Color.LightGray;
            this.map.EdgeSize = 1;
            this.map.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.map.FirstDieX = 0;
            this.map.FirstDieY = 0;
            this.map.ForeColor = System.Drawing.Color.Red;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.map.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.map.Name = "map";
            this.map.NotchAngle = 0;
            this.map.NotchType = DACrux.Base.Notch.Flat;
            this.map.OriginDieBorder = System.Drawing.Color.Red;
            this.map.OriginIndexX = 0;
            this.map.OriginIndexY = 0;
            this.map.OriginX = 0;
            this.map.OriginY = 0;
            this.map.PopupMenu = true;
            this.map.ReferenceDieSetting = 0;
            this.map.ScaleMark = false;
            this.map.SelecetedBin = "ALL";
            this.map.Size = new System.Drawing.Size(482, 520);
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 15;
            this.map.VisibleDieBorder = true;
            this.map.VisibleDieValue = false;
            this.map.VisibleInfomation = true;
            this.map.VisibleProbeOverlay = false;
            this.map.VisibleVIFail = false;
            this.map.WaferBorderColor = System.Drawing.Color.LightGray;
            this.map.WaferColor = System.Drawing.Color.Gray;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.map.WaferSize = 200000;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftTop;
            // 
            // DPUCZoneMap
            // 
            this.Controls.Add(this._DPUCZoneMapAutoHideControl);
            this.Controls.Add(this.map);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this.windowDockingArea2);
            this.Controls.Add(this.windowDockingArea4);
            this.Controls.Add(this._DPUCZoneMapUnpinnedTabAreaTop);
            this.Controls.Add(this._DPUCZoneMapUnpinnedTabAreaBottom);
            this.Controls.Add(this._DPUCZoneMapUnpinnedTabAreaLeft);
            this.Controls.Add(this._DPUCZoneMapUnpinnedTabAreaRight);
            this.Name = "DPUCZoneMap";
            this.Size = new System.Drawing.Size(800, 520);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefect_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadZone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadZone_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
            this.dockableWindow1.ResumeLayout(false);
            this.dockableWindow2.ResumeLayout(false);
            this.dockableWindow3.ResumeLayout(false);
            this.windowDockingArea4.ResumeLayout(false);
            this.windowDockingArea2.ResumeLayout(false);
            this.windowDockingArea1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();

            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : DACrux.Map.DefectMap.DEFAULT_DEFECT_SIZE;
        }
	}
}
