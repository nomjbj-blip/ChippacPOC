using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Reflection; 
using Excel = Microsoft.Office.Interop.Excel; 
using System.Net;
using System.Net.Sockets;

namespace DMSPlus.DefectMapAnalysis.Control
{
	/// <Summary>
	/// <b>■ Defect Report </b><br>
	/// - 작  성  자 : 미라콤 곽동일<br>
	/// - 최초작성일 : 2004년 08월 27일<br>
	/// - 최종수정자 : <br>
	/// - 최종수정일 : <br>
	/// - 주요변경로그<br>
	///   2004.08.27 생성<br>
	/// </Summary>
	/// <Remarks>없음</Remarks>
	/// 
	public class ReportDefectReport : System.Windows.Forms.UserControl
	{
		public string strUserID="";

		private System.Windows.Forms.Label lblDateFrom;
		private System.Windows.Forms.Label lblDateTo;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private ReportQuery uctlQuery;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.GroupBox grpChartOption;
		private System.Windows.Forms.RadioButton rdoExcel;
		private System.Windows.Forms.RadioButton rdoImage;

		private FarPoint.Win.Spread.FpSpread fpsDefectReport;

		private System.Windows.Forms.Button btnToExcel;
		private System.Windows.Forms.Button btnReset;
		private static string[] strEquip;

		private string[,] strAxisY;

		private int intMaxStep;
		private int intMinStep;

		private string[,] strExportList;
		private string strLastEQ="";

		private static string strFromDate="";
		private static string strToDate="";
		private System.Windows.Forms.TabControl tabChart;
		private System.Windows.Forms.TabPage tbpDefect;
		private System.Windows.Forms.TabPage tbpDie;

		private Miracom.Common.Win.ChartUtil oChart = new Miracom.Common.Win.ChartUtil();
		private SoftwareFX.ChartFX.Chart chtDefCount;
		private SoftwareFX.ChartFX.Chart chtDieCount;
		private System.Windows.Forms.TabPage tbpClass;

		private DataTable[] dtClassCount = null;
		private DataTable[] dtRoughBinCount = null;
		private SoftwareFX.ChartFX.Chart chtClass;
		private System.Windows.Forms.TabPage tbpRoughBin;
		private SoftwareFX.ChartFX.Chart chtRoughBin;
		private System.Windows.Forms.CheckedListBox lstColumns;

		private System.ComponentModel.Container components = null;
		private string[,] strColumnList = null; 
		private int iStaticColumn = 15;
		int intColumnCount = 0;
		int iDfFieldCnt = 0;
		private System.Windows.Forms.CheckBox chkAllColumn;
		private System.Windows.Forms.Label label1;
		ArrayList arrDFClsType = null;
		Hashtable htChartMaxScale = null;

		double dDefectChartOldMaxYScale = 0;
		double dDefectChartNewMaxYScale = 0;
		double dDieChartOldMaxYScale = 0;
		double dDieChartNewMaxYScale = 0;
		double dClassChartOldMaxYScale = 0;
		double dClassChartNewMaxYScale = 0;
		double dRBChartOldMaxYScale = 0;
		double dRBChartNewMaxYScale = 0;


		#region ■ DefectReport

		/// <summary>
		/// DefectReport 생성자
		/// </summary>
		/// 
		public ReportDefectReport()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();
			// TODO: InitComponent를 호출한 다음 초기화 작업을 추가합니다.
		}
		#endregion

		#region ■ GetUserID

		/// <summary>
		/// 사용자 ID를 ASPX의 세션에서 넘겨받는다.
		/// </summary>
		/// <param name="strUser">사용자 ID</param>
		/// 
		public void GetUserID(string strUser)
		{
			strUserID=strUser;
			// uctlQuery 사용자정의컨트롤에 사용자 ID를 할당.
			uctlQuery.strUserID=strUserID;
		}
		#endregion

		#region Dispose

		/// <summary>
		/// Dispose
		/// </summary>
		/// 
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}
		#endregion

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReportDefectReport));
			SoftwareFX.ChartFX.TitleDockable titleDockable1 = new SoftwareFX.ChartFX.TitleDockable();
			SoftwareFX.ChartFX.TitleDockable titleDockable2 = new SoftwareFX.ChartFX.TitleDockable();
			SoftwareFX.ChartFX.SeriesAttributes seriesAttributes1 = new SoftwareFX.ChartFX.SeriesAttributes();
			SoftwareFX.ChartFX.SeriesAttributes seriesAttributes2 = new SoftwareFX.ChartFX.SeriesAttributes();
			SoftwareFX.ChartFX.TitleDockable titleDockable3 = new SoftwareFX.ChartFX.TitleDockable();
			SoftwareFX.ChartFX.TitleDockable titleDockable4 = new SoftwareFX.ChartFX.TitleDockable();
			this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
			this.dtpToDate = new System.Windows.Forms.DateTimePicker();
			this.lblDateFrom = new System.Windows.Forms.Label();
			this.lblDateTo = new System.Windows.Forms.Label();
			this.uctlQuery = new ReportQuery();
			this.btnStart = new System.Windows.Forms.Button();
			this.grpChartOption = new System.Windows.Forms.GroupBox();
			this.btnToExcel = new System.Windows.Forms.Button();
			this.rdoImage = new System.Windows.Forms.RadioButton();
			this.rdoExcel = new System.Windows.Forms.RadioButton();
			this.fpsDefectReport = new FarPoint.Win.Spread.FpSpread();
			this.btnReset = new System.Windows.Forms.Button();
			this.tabChart = new System.Windows.Forms.TabControl();
			this.tbpDefect = new System.Windows.Forms.TabPage();
			this.chtDefCount = new SoftwareFX.ChartFX.Chart();
			this.tbpDie = new System.Windows.Forms.TabPage();
			this.chtDieCount = new SoftwareFX.ChartFX.Chart();
			this.tbpClass = new System.Windows.Forms.TabPage();
			this.chtClass = new SoftwareFX.ChartFX.Chart();
			this.tbpRoughBin = new System.Windows.Forms.TabPage();
			this.chtRoughBin = new SoftwareFX.ChartFX.Chart();
			this.lstColumns = new System.Windows.Forms.CheckedListBox();
			this.chkAllColumn = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.grpChartOption.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fpsDefectReport)).BeginInit();
			this.tabChart.SuspendLayout();
			this.tbpDefect.SuspendLayout();
			this.tbpDie.SuspendLayout();
			this.tbpClass.SuspendLayout();
			this.tbpRoughBin.SuspendLayout();
			this.SuspendLayout();
			// 
			// dtpFromDate
			// 
			this.dtpFromDate.Location = new System.Drawing.Point(8, 19);
			this.dtpFromDate.Name = "dtpFromDate";
			this.dtpFromDate.Size = new System.Drawing.Size(180, 21);
			this.dtpFromDate.TabIndex = 5;
			this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
			// 
			// dtpToDate
			// 
			this.dtpToDate.CustomFormat = "";
			this.dtpToDate.Location = new System.Drawing.Point(206, 19);
			this.dtpToDate.Name = "dtpToDate";
			this.dtpToDate.Size = new System.Drawing.Size(184, 21);
			this.dtpToDate.TabIndex = 1;
			this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
			// 
			// lblDateFrom
			// 
			this.lblDateFrom.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblDateFrom.Location = new System.Drawing.Point(6, 5);
			this.lblDateFrom.Name = "lblDateFrom";
			this.lblDateFrom.Size = new System.Drawing.Size(118, 16);
			this.lblDateFrom.TabIndex = 2;
			this.lblDateFrom.Text = "Start Date";
			// 
			// lblDateTo
			// 
			this.lblDateTo.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblDateTo.Location = new System.Drawing.Point(203, 5);
			this.lblDateTo.Name = "lblDateTo";
			this.lblDateTo.Size = new System.Drawing.Size(81, 16);
			this.lblDateTo.TabIndex = 3;
			this.lblDateTo.Text = "End Date";
			// 
			// uctlQuery
			// 
			this.uctlQuery.Location = new System.Drawing.Point(2, 48);
			this.uctlQuery.Name = "uctlQuery";
			this.uctlQuery.Size = new System.Drawing.Size(396, 200);
			this.uctlQuery.TabIndex = 6;
			// 
			// btnStart
			// 
			this.btnStart.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStart.BackgroundImage")));
			this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStart.Location = new System.Drawing.Point(264, 304);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(65, 21);
			this.btnStart.TabIndex = 8;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// grpChartOption
			// 
			this.grpChartOption.Controls.Add(this.btnToExcel);
			this.grpChartOption.Controls.Add(this.rdoImage);
			this.grpChartOption.Controls.Add(this.rdoExcel);
			this.grpChartOption.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpChartOption.Location = new System.Drawing.Point(224, 400);
			this.grpChartOption.Name = "grpChartOption";
			this.grpChartOption.Size = new System.Drawing.Size(144, 112);
			this.grpChartOption.TabIndex = 13;
			this.grpChartOption.TabStop = false;
			this.grpChartOption.Text = "To Excel";
			// 
			// btnToExcel
			// 
			this.btnToExcel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnToExcel.BackgroundImage")));
			this.btnToExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnToExcel.Location = new System.Drawing.Point(40, 80);
			this.btnToExcel.Name = "btnToExcel";
			this.btnToExcel.Size = new System.Drawing.Size(65, 21);
			this.btnToExcel.TabIndex = 10;
			this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
			// 
			// rdoImage
			// 
			this.rdoImage.Checked = true;
			this.rdoImage.Font = new System.Drawing.Font("돋움", 9F);
			this.rdoImage.Location = new System.Drawing.Point(27, 47);
			this.rdoImage.Name = "rdoImage";
			this.rdoImage.Size = new System.Drawing.Size(100, 24);
			this.rdoImage.TabIndex = 1;
			this.rdoImage.TabStop = true;
			this.rdoImage.Text = "Image Chart";
			// 
			// rdoExcel
			// 
			this.rdoExcel.Enabled = false;
			this.rdoExcel.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.rdoExcel.Location = new System.Drawing.Point(27, 21);
			this.rdoExcel.Name = "rdoExcel";
			this.rdoExcel.Size = new System.Drawing.Size(92, 24);
			this.rdoExcel.TabIndex = 0;
			this.rdoExcel.Text = "Excel Chart";
			// 
			// fpsDefectReport
			// 
			this.fpsDefectReport.Location = new System.Drawing.Point(401, 0);
			this.fpsDefectReport.Name = "fpsDefectReport";
			this.fpsDefectReport.Size = new System.Drawing.Size(598, 264);
			this.fpsDefectReport.TabIndex = 15;
			this.fpsDefectReport.SheetTabClick += new FarPoint.Win.Spread.SheetTabClickEventHandler(this.fpsDefectReport_SheetTabClick);
			// 
			// btnReset
			// 
			this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReset.Location = new System.Drawing.Point(264, 344);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(65, 21);
			this.btnReset.TabIndex = 16;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// tabChart
			// 
			this.tabChart.Controls.Add(this.tbpDefect);
			this.tabChart.Controls.Add(this.tbpDie);
			this.tabChart.Controls.Add(this.tbpClass);
			this.tabChart.Controls.Add(this.tbpRoughBin);
			this.tabChart.Location = new System.Drawing.Point(400, 272);
			this.tabChart.Name = "tabChart";
			this.tabChart.SelectedIndex = 0;
			this.tabChart.Size = new System.Drawing.Size(600, 240);
			this.tabChart.TabIndex = 17;
			// 
			// tbpDefect
			// 
			this.tbpDefect.Controls.Add(this.chtDefCount);
			this.tbpDefect.Location = new System.Drawing.Point(4, 21);
			this.tbpDefect.Name = "tbpDefect";
			this.tbpDefect.Size = new System.Drawing.Size(592, 215);
			this.tbpDefect.TabIndex = 0;
			this.tbpDefect.Text = "Defect Count";
			// 
			// chtDefCount
			// 
			this.chtDefCount.AxisX.Gridlines = true;
			this.chtDefCount.AxisX.Title.Text = "Inspection Data (YY-MM HH)";
			this.chtDefCount.AxisY.Gridlines = true;
			this.chtDefCount.AxisY.LabelsFormat.Decimals = 0;
			this.chtDefCount.AxisY.Title.Text = "Defect Count";
			this.chtDefCount.BorderObject = new SoftwareFX.ChartFX.DefaultBorder(SoftwareFX.ChartFX.BorderType.Color, System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0))));
			this.chtDefCount.DataEditorObj.ShowHeader = false;
			this.chtDefCount.DesignTimeData = "C:\\Program Files\\ChartFX for .NET 6.2\\Wizard\\MulltiSeries.txt";
			this.chtDefCount.Gallery = SoftwareFX.ChartFX.Gallery.Lines;
			this.chtDefCount.Location = new System.Drawing.Point(0, 0);
			this.chtDefCount.MarkerSize = ((short)(4));
			this.chtDefCount.Name = "chtDefCount";
			this.chtDefCount.NSeries = 3;
			this.chtDefCount.NValues = 10;
			this.chtDefCount.Palette = "DarkPastels.Pastels";
			this.chtDefCount.Size = new System.Drawing.Size(592, 215);
			this.chtDefCount.TabIndex = 0;
			titleDockable1.Alignment = System.Drawing.StringAlignment.Near;
			this.chtDefCount.Titles.AddRange(new SoftwareFX.ChartFX.TitleDockable[] {
																						titleDockable1});
			this.chtDefCount.InternalCommand += new SoftwareFX.ChartFX.CommandUIEventHandler(this.chtDefCount_InternalCommand);
			// 
			// tbpDie
			// 
			this.tbpDie.Controls.Add(this.chtDieCount);
			this.tbpDie.Location = new System.Drawing.Point(4, 21);
			this.tbpDie.Name = "tbpDie";
			this.tbpDie.Size = new System.Drawing.Size(592, 215);
			this.tbpDie.TabIndex = 1;
			this.tbpDie.Text = "Die Count";
			// 
			// chtDieCount
			// 
			this.chtDieCount.AxisX.Gridlines = true;
			this.chtDieCount.AxisX.Title.Text = "Inspection Data (YY-MM HH)";
			this.chtDieCount.AxisY.Gridlines = true;
			this.chtDieCount.AxisY.LabelsFormat.Decimals = 0;
			this.chtDieCount.AxisY.Title.Text = "Depective Die Count";
			this.chtDieCount.DesignTimeData = "C:\\Program Files\\ChartFX for .NET 6.2\\Wizard\\MulltiSeries.txt";
			this.chtDieCount.Gallery = SoftwareFX.ChartFX.Gallery.Lines;
			this.chtDieCount.Location = new System.Drawing.Point(0, 0);
			this.chtDieCount.MarkerSize = ((short)(4));
			this.chtDieCount.Name = "chtDieCount";
			this.chtDieCount.NSeries = 3;
			this.chtDieCount.NValues = 10;
			this.chtDieCount.Palette = "Nature.Sky";
			this.chtDieCount.Size = new System.Drawing.Size(592, 215);
			this.chtDieCount.TabIndex = 0;
			titleDockable2.Font = new System.Drawing.Font("Arial", 8.25F);
			this.chtDieCount.Titles.AddRange(new SoftwareFX.ChartFX.TitleDockable[] {
																						titleDockable2});
			this.chtDieCount.InternalCommand += new SoftwareFX.ChartFX.CommandUIEventHandler(this.chtDieCount_InternalCommand);
			// 
			// tbpClass
			// 
			this.tbpClass.Controls.Add(this.chtClass);
			this.tbpClass.Location = new System.Drawing.Point(4, 21);
			this.tbpClass.Name = "tbpClass";
			this.tbpClass.Size = new System.Drawing.Size(592, 215);
			this.tbpClass.TabIndex = 2;
			this.tbpClass.Text = "Class";
			// 
			// chtClass
			// 
			this.chtClass.AxisX.Title.Text = "Inspection Data (YY-MM HH)";
			this.chtClass.AxisY.LabelsFormat.Decimals = 0;
			this.chtClass.AxisY.Title.Text = "Class Count";
			this.chtClass.DesignTimeData = "C:\\Program Files\\ChartFX for .NET 6.2\\Wizard\\Scatter.txt";
			this.chtClass.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chtClass.Gallery = SoftwareFX.ChartFX.Gallery.Scatter;
			this.chtClass.Location = new System.Drawing.Point(0, 0);
			this.chtClass.MarkerSize = ((short)(4));
			this.chtClass.Name = "chtClass";
			this.chtClass.NValues = 27;
			seriesAttributes1.MarkerShape = SoftwareFX.ChartFX.MarkerShape.Cross;
			this.chtClass.Series.AddRange(new SoftwareFX.ChartFX.SeriesAttributes[] {
																						seriesAttributes1,
																						seriesAttributes2});
			this.chtClass.SerLegBoxObj.ToolBorder = SoftwareFX.ChartFX.ToolBorder.External;
			this.chtClass.Size = new System.Drawing.Size(592, 215);
			this.chtClass.TabIndex = 0;
			this.chtClass.Titles.AddRange(new SoftwareFX.ChartFX.TitleDockable[] {
																					 titleDockable3});
			this.chtClass.InternalCommand += new SoftwareFX.ChartFX.CommandUIEventHandler(this.chtClass_InternalCommand);
			// 
			// tbpRoughBin
			// 
			this.tbpRoughBin.Controls.Add(this.chtRoughBin);
			this.tbpRoughBin.Location = new System.Drawing.Point(4, 21);
			this.tbpRoughBin.Name = "tbpRoughBin";
			this.tbpRoughBin.Size = new System.Drawing.Size(592, 215);
			this.tbpRoughBin.TabIndex = 3;
			this.tbpRoughBin.Text = "RoughBin";
			// 
			// chtRoughBin
			// 
			this.chtRoughBin.AxisX.Title.Text = "Inspection Data (YY-MM HH)";
			this.chtRoughBin.AxisY.LabelsFormat.Decimals = 0;
			this.chtRoughBin.AxisY.Title.Text = "RoughBin Count";
			this.chtRoughBin.DesignTimeData = "C:\\Program Files\\ChartFX for .NET 6.2\\Wizard\\Scatter.txt";
			this.chtRoughBin.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chtRoughBin.Gallery = SoftwareFX.ChartFX.Gallery.Scatter;
			this.chtRoughBin.LegendBoxObj.AutoSize = false;
			this.chtRoughBin.LegendBoxObj.Height = 0;
			this.chtRoughBin.LegendBoxObj.Width = 0;
			this.chtRoughBin.Location = new System.Drawing.Point(0, 0);
			this.chtRoughBin.MarkerSize = ((short)(4));
			this.chtRoughBin.Name = "chtRoughBin";
			this.chtRoughBin.NValues = 27;
			this.chtRoughBin.Palette = "Nature.Sky";
			this.chtRoughBin.Size = new System.Drawing.Size(592, 215);
			this.chtRoughBin.TabIndex = 0;
			this.chtRoughBin.Titles.AddRange(new SoftwareFX.ChartFX.TitleDockable[] {
																						titleDockable4});
			this.chtRoughBin.InternalCommand += new SoftwareFX.ChartFX.CommandUIEventHandler(this.chtRoughBin_InternalCommand);
			// 
			// lstColumns
			// 
			this.lstColumns.CheckOnClick = true;
			this.lstColumns.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lstColumns.Location = new System.Drawing.Point(8, 304);
			this.lstColumns.Name = "lstColumns";
			this.lstColumns.Size = new System.Drawing.Size(208, 196);
			this.lstColumns.TabIndex = 18;
			// 
			// chkAllColumn
			// 
			this.chkAllColumn.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.chkAllColumn.Location = new System.Drawing.Point(16, 272);
			this.chkAllColumn.Name = "chkAllColumn";
			this.chkAllColumn.Size = new System.Drawing.Size(136, 24);
			this.chkAllColumn.TabIndex = 19;
			this.chkAllColumn.Text = "Check All";
			this.chkAllColumn.CheckedChanged += new System.EventHandler(this.chkAllColumn_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.label1.Location = new System.Drawing.Point(8, 256);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 16);
			this.label1.TabIndex = 20;
			this.label1.Text = "Column Select";
			// 
			// DefectReport
			// 
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chkAllColumn);
			this.Controls.Add(this.lstColumns);
			this.Controls.Add(this.tabChart);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.fpsDefectReport);
			this.Controls.Add(this.grpChartOption);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.dtpToDate);
			this.Controls.Add(this.dtpFromDate);
			this.Controls.Add(this.lblDateTo);
			this.Controls.Add(this.lblDateFrom);
			this.Controls.Add(this.uctlQuery);
			this.Name = "DefectReport";
			this.Size = new System.Drawing.Size(1000, 520);
			this.Load += new System.EventHandler(this.DefectReport_Load);
			this.grpChartOption.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fpsDefectReport)).EndInit();
			this.tabChart.ResumeLayout(false);
			this.tbpDefect.ResumeLayout(false);
			this.tbpDie.ResumeLayout(false);
			this.tbpClass.ResumeLayout(false);
			this.tbpRoughBin.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region ■ Form_Load

		/// <summary>
		/// Form Load 이벤트
		/// 1) 날짜선택상자의 날짜를 기본값으로 설정한다. (어제 ~ 오늘)
		/// 2) 해당날짜에 Inspection이 진행된 장비목록을 가져오는 함수 호출.
		/// 3) 챠트를 초기화하는 함수 호출.
		/// </summary>
		/// 
		private void DefectReport_Load(object sender, System.EventArgs e)
		{
			if(DesignMode) return;

			dtpFromDate.Value = System.DateTime.Now.AddDays(-1);
			dtpToDate.Value = System.DateTime.Now;

			arrDFClsType = new ArrayList();
			GetEQList();
			SetColumnList();

			ChartReset();
			htChartMaxScale = new Hashtable();
		}
		#endregion

		private void SetColumnList()
		{
			DataSet dsDfType = null;
			
			Remoting oRemoting = new Remoting();
			dsDfType = oRemoting.Get(strUserID,"GetDefectTypeDefinition");
			intColumnCount = iStaticColumn + dsDfType.Tables[0].Rows.Count;
			strColumnList=new string[intColumnCount,2];

			strColumnList[0,0]="Scan Date";
			strColumnList[0,1]="TO_CHAR(T_DMS_STEP.RESULTTIMESTAMP,'YYYY-MM-DD HH24:MI:SS') AS Scan_Date";

			strColumnList[1,0]="Lot ID";
			strColumnList[1,1]="T_DMS_LOT.LOT_ID AS LOT_ID";

			strColumnList[2,0]="Wafer ID";
			strColumnList[2,1]="T_DMS_WAFER.WAFER_ID AS WAFER_ID";

			strColumnList[3,0]="Setup ID";
			strColumnList[3,1]="T_DMS_SETUP.SETUP_ID AS SETUP_ID";

			strColumnList[4,0]="Step ID";
			strColumnList[4,1]="T_DMS_STEP.STEP_ID AS STEP_ID";

			strColumnList[5,0]="S Cls";
			strColumnList[5,1]="substr(T_DMS_LOT.LOT_ID,3,1) AS S_Cls";

			strColumnList[6,0]="Main_Eq";
			strColumnList[6,1]="T_DMS_STEP.MAIN_EQ AS Main_Eq";

			strColumnList[7,0]="Insp-EQ";
			strColumnList[7,1]="T_DMS_STEP.INSPECTION_EQ AS EQ_ID";

			strColumnList[8,0]="Tot_Def_Cnt";
			strColumnList[8,1]="T_DMS_STEP_SUM.TOTALDEFECT AS Tot_Def_Cnt";

			strColumnList[9,0]="Tot_Def_0.5";
			strColumnList[9,1]="(T_DMS_STEP_SUM.TOTALDEFECT-T_DMS_STEP_SUM.TOTAL5UM) AS Tot_Def_05";

			strColumnList[10,0]="DI";
			strColumnList[10,1]="' ' AS DI";

			strColumnList[11,0]="DR";
			strColumnList[11,1]="T_DMS_STEP_SUM.TOTALDEFECTDIE AS DR";

			strColumnList[12,0]="DR_0.5";
			strColumnList[12,1]="(T_DMS_STEP_SUM.TOTALDEFECTDD-T_DMS_STEP_SUM.TOTALRANDOMDEFDIE5UM) AS DR_05";

			strColumnList[13,0]="Density";
			strColumnList[13,1]="T_DMS_STEP_SUM.TOTALDEFECTDD AS Density";

			strColumnList[14,0]="REMARK";
			strColumnList[14,1]="'' AS Remark";



			string sDfName = string.Empty;
			for(int i=0;i<dsDfType.Tables[0].Rows.Count;i++)
			{
				sDfName = dsDfType.Tables[0].Rows[i]["NAME"].ToString();
				strColumnList[15+i,0] = sDfName;
				strColumnList[15+i,1] = "'' AS " +  sDfName;
			}

			for(int i=0;i<intColumnCount;i++)
			{
				if(strColumnList[i,1].Substring(0,2)=="''")
				{
					lstColumns.Items.Add(strColumnList[i,0],CheckState.Unchecked);
				}
				else
				{
					lstColumns.Items.Add(strColumnList[i,0],CheckState.Checked);
				}
			}
			//ADD Defect Type List
		}
		#region ■ 장비목록 ListUp

		/// <summary>
		/// 1) 장비목록 쿼리에 필요한 날짜를 Formating 한다.
		/// 2) Formating 한 날짜를 가지고 uctlQuery 사용자 컨트롤의 GetEQList를 호출한다.
		/// </summary>
		/// 
		private void GetEQList()
		{
			SetDate();
			uctlQuery.GetEQList(strFromDate,strToDate);
		}
		#endregion

		#region ■ btnStart_Click 클릭이벤트

		/// <summary>
		/// 1) 스프레드시트 초기화
		/// 2) 챠트 초기화
		/// 3) 레포트 시작 서비스 호출
		/// </summary>
		/// 
		private void btnStart_Click(object sender, System.EventArgs e)
		{
			htChartMaxScale.Clear();
			if(strAxisY != null && strAxisY.Length>0) strAxisY = null;
			strAxisY=new string[uctlQuery.clbEQ.CheckedItems.Count,3];

			for(int y=0;y<uctlQuery.clbEQ.CheckedItems.Count;y++)
			{
				strAxisY[y,0]=uctlQuery.clbEQ.CheckedItems[y].ToString();
				strAxisY[y,1]="";
				strAxisY[y,2]="";
			}

			SheetReset();
			ChartReset();
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			StartReporting();
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ End Date 변경시 이벤트

		/// <summary>
		/// 장비목록을 새로 불러오는 서비스 호출
		/// </summary>
		/// 
		private void dtpToDate_ValueChanged(object sender, System.EventArgs e)
		{
			GetEQList();
		}
		#endregion

		#region ■ Start Date 변경시 이벤트

		/// <summary>
		/// 장비목록을 새로 불러오는 서비스 호출
		/// </summary>
		/// 
		private void dtpFromDate_ValueChanged(object sender, System.EventArgs e)
		{
			GetEQList();
		}
		#endregion

		#region ■ StartReporting

		/// <summary>
		/// 1) 장비목록에서 체크된 장비수*2 개의 시트를 생성하고 초기화.
		/// 2) 각 장비ID를 인자로 넘겨 Defect Count / DR 등 필요한 Report Column을 가져오는 서비스 호출  
		/// </summary>
		/// 
		private void StartReporting()
		{
			try
			{
				if(this.lstColumns.CheckedItems.Count>0)
				{
					arrDFClsType.Clear();
					// 시트가 장비별로 생성되므로 장비별로 Query Action을 일으킨다.
					strEquip=new string[uctlQuery.clbEQ.CheckedItems.Count];
					dtClassCount = new DataTable[uctlQuery.clbEQ.CheckedItems.Count];
					dtRoughBinCount =  new DataTable[uctlQuery.clbEQ.CheckedItems.Count];

					for(int i=iStaticColumn;i<lstColumns.Items.Count;i++)
					{
						if (lstColumns.GetItemChecked(i) ==true)
						{
							arrDFClsType.Add(strColumnList[i,0]);
						}
					}

					for(int i=0;i<uctlQuery.clbEQ.CheckedItems.Count;++i)
					{
						strEquip[i]=uctlQuery.clbEQ.CheckedItems[i].ToString();
					}

					int intArrCnt=0;

					for(int i=0;i<uctlQuery.clbEQ.CheckedItems.Count*2;i+=2)
					{

						FarPoint.Win.Spread.SheetView Sheet = new FarPoint.Win.Spread.SheetView();

						fpsDefectReport.Sheets.Add(Sheet);
						fpsDefectReport.Sheets[i].SheetName=strEquip[intArrCnt];

						FarPoint.Win.Spread.SheetView Sheet_CNT = new FarPoint.Win.Spread.SheetView();

						fpsDefectReport.Sheets.Add(Sheet_CNT);
						fpsDefectReport.Sheets[i+1].SheetName=strEquip[intArrCnt]+"_CNT";

						GetDefectData(strEquip[intArrCnt],i);
						intArrCnt++;
					}


					//arrDFClsType
				}
				else
				{
					MessageBox.Show("Report에 표시할 필드가 하나도 선택되지 않았습니다."
						+ "1개 이상의 필드를 선택하신 후에 다시 시도하여 주십시오.");
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region ■ GetDefectData

		/// <summary>
		/// 1) StartReporting 에서 넘겨받은 장비 ID및 설정기간에 해당하는 Defect 정보를 Listup 한다.
		/// 2) 가져온 정보를 각 Sheet에 바인딩한다.
		/// 3) 본 Report 에는 1쌍의 (Defect/DR Chart) 챠트만 존재하므로 첫번째 장비에 해당하는 챠트만 Display 시킨다.
		/// </summary>
		/// 
		private void GetDefectData(string strEQID, int intEQIndex)
		{

			DataSet dsReturn=null;
			DataSet dsDInspected = null;
			Remoting oList=null;
			StringBuilder builder=new StringBuilder();
			string strColumn="";
			string sColumnTmp = string.Empty;
			string[] strStepList;
			string[] strDeviceList;

			bool bDisplay=false;
			bool bDIExists = false;

			try
			{
				oList=new Remoting();

				
				//  Dynamic SQL 컬럼 생성
				for(int i=0;i<lstColumns.Items.Count;i++)
				{
					if (lstColumns.GetItemChecked(i) ==true)
					{
						if(bDisplay!=false)
						{
							builder.Append(", ");
						}
						
						//SQ_SCAN_SAMPLE.DI
						sColumnTmp = strColumnList[i,1];
						if(sColumnTmp == "'' AS DI" || sColumnTmp == "0 AS DI")
						{
							bDIExists = true;
						}
						if(sColumnTmp == "'' AS Remark")
						{
							builder.Append(sColumnTmp);
						}
						else
						{
							builder.Append(sColumnTmp.Replace("'' AS","0 AS"));
						}
						
						
						bDisplay=true;
					}
				}
				iDfFieldCnt = 0;
				for(int i=iStaticColumn;i<lstColumns.Items.Count;i++)
				{
					if (lstColumns.GetItemChecked(i) ==true)
					{
						builder.Append(", ");
						//SQT_SCAN_SAMPLE.DI
						sColumnTmp = "0 AS " + strColumnList[i,0] + "#";
						builder.Append(sColumnTmp);
						iDfFieldCnt += 1;
					}
				}

				builder.Append(",T_DMS_STEP.STEP_SEQ");

				strColumn=builder.ToString();
				builder.Remove(0,builder.Length);

				strDeviceList= new string [uctlQuery.clbDevice.CheckedItems.Count];

				for(int k=0;k< uctlQuery.clbDevice.CheckedItems.Count;k++)
				{
					strDeviceList[k]=uctlQuery.clbDevice.CheckedItems[k].ToString();
				}

				strStepList= new string [uctlQuery.clbStep.CheckedItems.Count];

				for(int k=0;k< uctlQuery.clbStep.CheckedItems.Count;k++)
				{
					strStepList[k]=uctlQuery.clbStep.CheckedItems[k].ToString();
				}

				dsReturn=oList.GetDefectData(strUserID, "GetDefectReport", strFromDate, strToDate, strColumn, strEQID, strDeviceList, strStepList,strColumnList);
				///////////////////////////////////////////////////////////////////////////////
				/// SHEET DATA INSERTION
				///////////////////////////////////////////////////////////////////////////////
				DataRow[] drFilteredRows = null;
				DataTable dtDfReport = null;
				ArrayList arrUniqueDefClassList = new ArrayList();
				string sOldDefClassName = string.Empty;
				string sNewDefClassName = string.Empty;
				ArrayList arrDefDataColumn = new ArrayList();
				string sStepSeqs = string.Empty;
				
				drFilteredRows = dsReturn.Tables[2].Select("","NAME");
				
				for(int i=0;i<drFilteredRows.Length;i++)
				{
					sNewDefClassName = drFilteredRows[i]["NAME"].ToString();
					if(sNewDefClassName != sOldDefClassName)
					{
						arrUniqueDefClassList.Add(sNewDefClassName);
					}
					sOldDefClassName = sNewDefClassName;
				}
				drFilteredRows = null;
				dtDfReport = dsReturn.Tables[0];
				for(int i=0;i<dtDfReport.Rows.Count;i++)
				{
					sStepSeqs += dtDfReport.Rows[i]["STEP_SEQ"].ToString() + ",";

				}
				for(int j=0;j<iDfFieldCnt;j++)
				{
					dtDfReport.Columns[dtDfReport.Columns.Count-j-2].ColumnName  += "(RB)"; 
				}	
				if(sStepSeqs.Length > 0)
				{
					sStepSeqs = sStepSeqs.Substring(0,sStepSeqs.Length-1);
				
					//DI 값을 가져옴
					dsDInspected = oList.GetDynamic(strUserID,"GetInspectedDieCountGroupBy",new string[]{sStepSeqs});
				}
				else
				{
					dsDInspected = null;
				}

				
				for(int i=0;i<dtDfReport.Columns.Count;i++)
				{
					arrDefDataColumn.Add(dtDfReport.Columns[i].ColumnName);
				}
				
				for(int i=0;i<arrUniqueDefClassList.Count;i++)
				{
					for(int j=0;j<arrDefDataColumn.Count;j++)
					{
						if(arrUniqueDefClassList[i].ToString() == arrDefDataColumn[j].ToString())
						{
							drFilteredRows = dsReturn.Tables[2].Select("NAME='"+arrUniqueDefClassList[i].ToString()+"'","STEP_SEQ");
							for(int k=0;k<dtDfReport.Rows.Count;k++)
							{
								for(int m=0;m<drFilteredRows.Length;m++)
								{
									if(dtDfReport.Rows[k]["STEP_SEQ"].ToString() == drFilteredRows[m]["STEP_SEQ"].ToString())
									{
										dtDfReport.Rows[k][arrUniqueDefClassList[i].ToString()] = drFilteredRows[m]["CNT"];
										break;
									}
								}
							}
						}
					}
				}
				// ROUGHBIN DATA INSERTION
				sNewDefClassName = "";
				sOldDefClassName = "";
				drFilteredRows = null;
				arrUniqueDefClassList.Clear();

				drFilteredRows = dsReturn.Tables[3].Select("","NAME");
				
				
				for(int i=0;i<drFilteredRows.Length;i++)
				{
					sNewDefClassName = drFilteredRows[i]["NAME"].ToString();
					if(sNewDefClassName != sOldDefClassName)
					{
						arrUniqueDefClassList.Add(sNewDefClassName);
					}
					sOldDefClassName = sNewDefClassName;
				}
				string sTmpColName = string.Empty;

				for(int i=0;i<arrUniqueDefClassList.Count;i++)
				{
					for(int j=0;j<arrDefDataColumn.Count;j++)
					{
						sTmpColName = arrUniqueDefClassList[i].ToString() + "#(RB)";
						if(sTmpColName == arrDefDataColumn[j].ToString())
						{
							drFilteredRows = dsReturn.Tables[3].Select("NAME='"+arrUniqueDefClassList[i].ToString()+"'","STEP_SEQ");
							for(int k=0;k<dtDfReport.Rows.Count;k++)
							{
								for(int m=0;m<drFilteredRows.Length;m++)
								{
									if(dtDfReport.Rows[k]["STEP_SEQ"].ToString() == drFilteredRows[m]["STEP_SEQ"].ToString())
									{
										dtDfReport.Rows[k][sTmpColName] = drFilteredRows[m]["CNT"];
										break;
									}
								}
							}
						}
					}
				}

				if(bDIExists)
				{
					for(int i=0;i<dtDfReport.Rows.Count;i++)
					{
						if(dsDInspected != null)
						{
							for(int j=0;j<dsDInspected.Tables[0].Rows.Count;j++)
							{
								if(dtDfReport.Rows[i]["STEP_SEQ"].ToString() == dsDInspected.Tables[0].Rows[j]["STEP_SEQ"].ToString())
								{
									dtDfReport.Rows[i]["DI"] = dsDInspected.Tables[0].Rows[j]["DICnt"];
									break;
								}
							}
						}
					}
				}

//				dtDfReport.Columns.Remove("STEP_SEQ");
				fpsDefectReport.Sheets[intEQIndex].DataSource=dtDfReport;
				fpsDefectReport.Sheets[intEQIndex+1].DataSource=dsReturn.Tables[1];
				dtClassCount[intEQIndex/2] = dsReturn.Tables[2];
				dtRoughBinCount[intEQIndex/2] = dsReturn.Tables[3];

				if(intEQIndex==0)
				{
					strLastEQ = fpsDefectReport.Sheets[0].SheetName.ToString().Substring(0,6);
					ShowChart(strEQID,1);
				}
			}

			catch(Exception ex)
			{
				throw ex;
				//Miracom.DMS.Config.
			}

			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
			}
		}
		#endregion

		#region ■ btnReset_Click 클릭이벤트

		/// <summary>
		/// 1) 스프레드 시트가 1개이상 있으면 스프레드시트를 초기화 하는 서비스 호출.
		/// </summary>
		/// 
		private void btnReset_Click(object sender, System.EventArgs e)
		{
			if(fpsDefectReport.Sheets.Count>0)
				SheetReset();
		}
		#endregion

		#region ■ 스프레드 시트 초기화

		/// <summary>
		/// 스프레드 시트의 Count 대로 Loop 를 돌면서 각 시트를 초기화 한다.
		/// </summary>
		/// 
		private void SheetReset()
		{
			for (int i=fpsDefectReport.Sheets.Count-1;i>=0;i--)
			{
				fpsDefectReport.Sheets.Remove(fpsDefectReport.Sheets[i]);
			}
		}
		#endregion

		#region ■ 스프레드 시트탭 클릭 이벤트

		/// <summary>
		/// 스프레드 시트의 탭을 클릭하면 해당장비에 대한 차트가 보여질 수 있도록 한다.
		/// </summary>
		/// 
		private void fpsDefectReport_SheetTabClick(object sender, FarPoint.Win.Spread.SheetTabClickEventArgs e)
		{
			try
			{
				// 선택한 시트가 '장비명_CNT' 시트가 아니면 +1 하여 현재 시트를 '장비명_CNT' 시트로 설정한다. 

				int intSheetIndex;
				if ((e.SheetTabIndex%2)==0)

				{
					intSheetIndex = e.SheetTabIndex + 1;
				}
				else
				{
					intSheetIndex = e.SheetTabIndex;
				}

				for(int k=0;k<strAxisY.GetLength(0);k++)
				{
					if(strAxisY[k,0]==strLastEQ)
					{
						strAxisY[k,1]=chtDefCount.AxisY.Max.ToString();
						strAxisY[k,2]=chtDieCount.AxisY.Max.ToString();
					}
				}
			
				ChartReset();
				ShowChart(fpsDefectReport.Sheets[intSheetIndex-1].SheetName.ToString(), intSheetIndex);

				int iDEFAxisY=300;
				int iDRAxisY=300;

				for(int y=0;y<strAxisY.GetLength(0);y++)
				{
					if(fpsDefectReport.Sheets[e.SheetTabIndex].SheetName.ToString().Substring(0,6)==strAxisY[y,0])
					{
						if(strAxisY[y,1]=="")
							iDEFAxisY=0;
						else
							iDEFAxisY=Int32.Parse(strAxisY[y,1]);

						if(strAxisY[y,2]=="")
							iDRAxisY=0;
						else
							iDRAxisY=Int32.Parse(strAxisY[y,2]);

						if(iDEFAxisY!=0)
							chtDefCount.AxisY.Max=iDEFAxisY;

						if(iDRAxisY!=0)
							chtDieCount.AxisY.Max=iDRAxisY;

					}
				}
				chtDefCount.Refresh();
				chtDieCount.Refresh();
			}
			catch
			{
			}
			finally
			{
				strLastEQ = fpsDefectReport.Sheets[e.SheetTabIndex].SheetName.ToString().Substring(0,6);
			}
		}
		#endregion

		#region ■ ChartReset

		/// <summary>
		/// 2개의 차트를 초기화 한다.
		/// </summary>
		/// 
		private void ChartReset()
		{
			
			chtDefCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.AllData);
			chtDieCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.AllData);
			chtClass.ClearData(SoftwareFX.ChartFX.ClearDataFlag.AllData);
			chtRoughBin.ClearData(SoftwareFX.ChartFX.ClearDataFlag.AllData);

			chtDefCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.Data);
			chtDieCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.Data);
			chtClass.ClearData(SoftwareFX.ChartFX.ClearDataFlag.Data);
			chtRoughBin.ClearData(SoftwareFX.ChartFX.ClearDataFlag.Data);

			chtDefCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.Labels);
			chtDieCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.Labels);			
			chtClass.ClearData(SoftwareFX.ChartFX.ClearDataFlag.Labels);
			chtRoughBin.ClearData(SoftwareFX.ChartFX.ClearDataFlag.Labels);

			chtDefCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
			chtDieCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
			chtClass.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
			chtRoughBin.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);

		}
		#endregion

		#region ■ GetStepSeqList (삭제예정-확인요망)

		/// <summary>
		/// 호출하는 서비스 없음 (확인 요망)
		/// </summary>
		/// 
		private void GetStepSeqList()
		{
			DataSet dsReturn=null;
			Remoting oList=null;
			StringBuilder builder=new StringBuilder();

			string strWhere="";

			try
			{
				oList=new Remoting();

				//  Dynamic SQL 조건 생성
				
				builder.Append(" AND T_DMS_PRODUCT.FACILITY='M10'");
				builder.Append(" AND T_DMS_STEP.RESULTTIMESTAMP BETWEEN '" + strFromDate + "' AND '" + strToDate + "'");

				// 선택한 장비를 Device 조회 쿼리에 반영
				if(uctlQuery.clbEQ.CheckedItems.Count>0)
				{
					builder.Append(" AND T_DMS_STEP.INSPECTION_EQ IN(");
					builder.Append("'" + uctlQuery.clbEQ.CheckedItems[0] + "'");

					for(int k=1;k< uctlQuery.clbEQ.CheckedItems.Count;k++)
					{
						builder.Append(",'" + uctlQuery.clbEQ.CheckedItems[k] + "'");
					}
					builder.Append(")");
				}

				// 선택한 Device를 STEP 조회 쿼리에 반영
				if(uctlQuery.clbDevice.CheckedItems.Count>0)
				{
					builder.Append(" AND T_DMS_PRODUCT.PRODUCT in (");

					builder.Append("'" + uctlQuery.clbDevice.CheckedItems[0] + "'");
					for(int i=1;i< uctlQuery.clbDevice.CheckedItems.Count;i++)
					{
						builder.Append(",'" + uctlQuery.clbDevice.CheckedItems[i] + "'");
					}
					builder.Append(")");
				}

				// 선택한 STEP을 조회 쿼리에 반영
				if(uctlQuery.clbStep.CheckedItems.Count>0)
				{
					builder.Append(" AND T_DMS_STEP.STEP_ID in (");

					builder.Append("'" + uctlQuery.clbStep.CheckedItems[0] + "'");
					for(int i=1;i< uctlQuery.clbStep.CheckedItems.Count;i++)
					{
						builder.Append(",'" + uctlQuery.clbStep.CheckedItems[i] + "'");
					}
					builder.Append(")");
				}

				strWhere=builder.ToString();
				builder.Remove(0,builder.Length);

				string[] strParam={strWhere};

				dsReturn=oList.GetDynamic(strUserID, "GetStepSeqList", strParam);

				if(dsReturn.Tables[0].Rows.Count>0)
				{
					intMinStep=Int32.Parse(dsReturn.Tables[0].Rows[0][0].ToString());
					intMaxStep=Int32.Parse(dsReturn.Tables[0].Rows[0][1].ToString());
				}
			}

			catch(Exception ex)
			{
				throw ex;
				//Miracom.DMS.Config.
			}

			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
			}
		}
		#endregion

		#region ■ 날짜변수 생성

		/// <summary>
		/// SQL 문에 사용하기 위한 날짜변수를 생성한다.
		/// </summary>
		/// 
		private void SetDate()
		{
			strFromDate = string.Format("{0, 4}{1, 2}{2, 2}{3, 2}{4, 2}{5, 2}",
				dtpFromDate.Value.Year.ToString(),
				dtpFromDate.Value.Month.ToString().PadLeft(2, '0'),
				dtpFromDate.Value.Day.ToString().PadLeft(2, '0'),
				"06","00","00");

			strToDate =  string.Format("{0, 4}{1, 2}{2, 2}{3, 2}{4, 2}{5, 2}",
				dtpToDate.Value.AddDays(1).Year.ToString(),
				dtpToDate.Value.AddDays(1).Month.ToString().PadLeft(2, '0'),
				dtpToDate.Value.AddDays(1).Day.ToString().PadLeft(2, '0'),
				"05","59","59");
		}
		#endregion

		#region ■ btnToExcel_Click 클릭이벤트

		/// <summary>
		/// 시트가 존재하는지 체크하고 존재하면 ExportToExcel 서비스를 호출한다.
		/// </summary>
		/// 
		private void btnToExcel_Click(object sender, System.EventArgs e)
		{

			int intSheetIndex = 0;
			if(fpsDefectReport.Sheets.Count==0)
			{
				MessageBox.Show("There is none data to export","Notice");
			}
			else
			{
				ExportToExcel();
				//
				for(int i=0;i<this.fpsDefectReport.Sheets.Count;i++)
				{
					if(fpsDefectReport.Sheets[i].SheetName == fpsDefectReport.ActiveSheet.SheetName)
					{
						intSheetIndex = i;
						break;
					}
				}

				if ((intSheetIndex%2)==0)
				{
					intSheetIndex += 1;
				}
				ShowChart(fpsDefectReport.Sheets[intSheetIndex-1].SheetName.ToString(), intSheetIndex);

			}
		}
		#endregion

		#region ■ 챠트생성

		/// <summary>
		/// 선택된 시트에 해당하는 장비의 챠트를 생성한다.
		/// </summary>
		/// <param name="strEquipName">장비명</param>
		/// <param name="intSheetTab">선택된 스프레드 시트 Index</param></param>
		/// 
		private void ShowChart(string strEquipName, int intSheetTab)
		{
			DataRow[] drFilteredRows = null;
			string sOldDefClassName = string.Empty;
			string sNewDefClassName = string.Empty;
			string sOldRofBinClassName = string.Empty;
			string sNewRofBinClassName = string.Empty;
			int intRowCount = 0;
			int iDtCnt = 0;
			DataTable dtDefectRpt = null;
			bool bMatch = false;
			double dChartYScaleChangedValue = 0;
			object oTmp = null;

			try
			{
				intRowCount=fpsDefectReport.Sheets[intSheetTab].RowCount;

				ChartReset();


				chtDefCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
				chtDieCount.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
				chtClass.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
				chtRoughBin.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);

				chtDefCount.OpenData(SoftwareFX.ChartFX.COD.Values, 1, 0);
				chtDieCount.OpenData(SoftwareFX.ChartFX.COD.Values, 1, 0);
				chtClass.OpenData(SoftwareFX.ChartFX.COD.Values, 1, 0);
				chtRoughBin.OpenData(SoftwareFX.ChartFX.COD.Values, 1, 0);
			
				chtDefCount.Titles[0].Text =  strEquipName+" Defect Count";
				chtDieCount.Titles[0].Text =  strEquipName+" Defective Die Count";
				chtClass.Titles[0].Text = strEquipName+" Class Count";
				chtRoughBin.Titles[0].Text = strEquipName+" RoughBin Count";

				for(int i=0;i<intRowCount;i++)
				{
					chtDefCount.Legend[i]=fpsDefectReport.Sheets[intSheetTab].GetText(i,0).Substring(5,8);
					chtDefCount.Value[0, i]=double.Parse(fpsDefectReport.Sheets[intSheetTab].GetText(i,1));

					chtDieCount.Legend[i]=fpsDefectReport.Sheets[intSheetTab].GetText(i,0).Substring(5,8);
					chtDieCount.Value[0, i]=double.Parse(fpsDefectReport.Sheets[intSheetTab].GetText(i,3));
				}

				/// ====================================================================
				///	- 수정자 : 미라콤 임영신
				///	- 수정일 : 2005-05-16 
				///	- 변경로그 : Class Chart, RoughBin Chart 추가
				iDtCnt = intSheetTab/2;
				// Logic 
				// 1.Unique Defect Class List를 찾는다.
				// 2.Unique Defect Class 별로 필터링하여 chart데이타를 정의한다.
				
				//차트 XValue Legend 셋팅
				dtDefectRpt = (DataTable)fpsDefectReport.Sheets[intSheetTab].DataSource;
				for(int i=0;i<dtDefectRpt.Rows.Count;i++)
				{
					chtClass.Legend[i] = dtDefectRpt.Rows[i]["SCAN_DATE"].ToString().Substring(5,8);
					chtRoughBin.Legend[i]  = dtDefectRpt.Rows[i]["SCAN_DATE"].ToString().Substring(5,8);
						
				}
				/////////////////////////////////////////////////////////////////////////
				/// Defect CLASS TREND
				////////////////////////////////////////////////////////////////////////////

				drFilteredRows = null;
				chtClass.SerLegBoxObj.Font = new Font("Arial", 6);
				chtClass.SerLegBox = true;


				for(int i=0;i<arrDFClsType.Count;i++)
				{
					chtClass.Series[i].Legend = arrDFClsType[i].ToString();
					drFilteredRows = dtClassCount[iDtCnt].Select("NAME='"+arrDFClsType[i].ToString()+"'","SCAN_DATE ASC");
					if(drFilteredRows != null && drFilteredRows.Length > 0)
					{
						for(int k=0;k<dtDefectRpt.Rows.Count;k++)
						{
							for(int j=0;j<drFilteredRows.Length;j++)
							{
								if(dtDefectRpt.Rows[k]["SCAN_DATE"].ToString() == drFilteredRows[j]["SCAN_DATE"].ToString())
								{
									chtClass.Value[i,k] = Convert.ToDouble(drFilteredRows[j]["CNT"].ToString() == "" ? "0.0" : drFilteredRows[j]["CNT"].ToString());
									bMatch = true;
									break;
								}
							}
							if(!bMatch)
							{
								chtClass.Value[i,k] = 0;
							}

						}		
					}
					else
					{
						for(int k=0;k<dtDefectRpt.Rows.Count;k++)
						{
							chtClass.Value[i,k] = 0;
						}	
					}
				}

				/////////////////////////////////////////////////////////////////////////
				/// ROUGHBIN TREND
				/////////////////////////////////////////////////////////////////////////
				drFilteredRows = null;
				chtRoughBin.SerLegBoxObj.Font = new Font("Arial", 6);
				chtRoughBin.SerLegBox = true;

				for(int i=0;i<arrDFClsType.Count;i++)
				{
					chtRoughBin.Series[i].Legend = arrDFClsType[i].ToString();
					drFilteredRows = dtRoughBinCount[iDtCnt].Select("NAME='"+arrDFClsType[i].ToString()+"'","SCAN_DATE");
					if(drFilteredRows != null && drFilteredRows.Length > 0)
					{
						for(int k=0;k<dtDefectRpt.Rows.Count;k++)
						{
							for(int j=0;j<drFilteredRows.Length;j++)
							{
								if(dtDefectRpt.Rows[k]["SCAN_DATE"].ToString() == drFilteredRows[j]["SCAN_DATE"].ToString())
								{
									chtRoughBin.Value[i,k] = Convert.ToDouble(drFilteredRows[j]["CNT"].ToString() == "" ? "0.0" : drFilteredRows[j]["CNT"].ToString());
									bMatch = true;
									break;
								}
							}
							if(!bMatch)
							{
								chtRoughBin.Value[i,k] = 0;
							}

						}		
					}
					else
					{
						for(int k=0;k<dtDefectRpt.Rows.Count;k++)
						{
							chtRoughBin.Value[i,k] = 0;
						}	
					}
				}

				chtClass.CloseData(SoftwareFX.ChartFX.COD.Values);
				chtRoughBin.CloseData(SoftwareFX.ChartFX.COD.Values);
				chtDefCount.CloseData(SoftwareFX.ChartFX.COD.Values);
				chtDieCount.CloseData(SoftwareFX.ChartFX.COD.Values);

				oTmp = htChartMaxScale[intSheetTab+"DF"];
				if(oTmp != null)
				{
					dChartYScaleChangedValue = Convert.ToDouble(oTmp);
					chtDefCount.AxisY.Max = dChartYScaleChangedValue;
					oTmp = null;
				}
				oTmp = htChartMaxScale[intSheetTab+"DC"];
				if(oTmp != null)
				{
					dChartYScaleChangedValue = Convert.ToDouble(oTmp);
					chtDieCount.AxisY.Max = dChartYScaleChangedValue;
					oTmp = null;
				}
				oTmp = htChartMaxScale[intSheetTab+"CS"];
				if(oTmp != null)
				{
					dChartYScaleChangedValue = Convert.ToDouble(oTmp);
					chtClass.AxisY.Max = dChartYScaleChangedValue;
					oTmp = null;
				}
				oTmp = htChartMaxScale[intSheetTab+"RB"];
				if(oTmp != null)
				{
					dChartYScaleChangedValue = Convert.ToDouble(oTmp);
					chtRoughBin.AxisY.Max = dChartYScaleChangedValue;
					oTmp = null;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region ■ Excel 로 Export

		/// <summary>
		/// 1) 스프레드시트의 내용을 엑셀로 보낸다.
		/// 2) 각 장비별로 챠트를 생성하여 이미지생성후 엑셀에 포함시킨다.
		/// </summary>
		/// 
		private void ExportToExcel()
		{

			//0.객체 선언 , 인스턴싱
			Miracom.Common.Win.ExcelUtil oXL = new Miracom.Common.Win.ExcelUtil();
			
			//1.엑셀 오브젝트 얻어 오기
			Excel._Workbook oWB = oXL.fnGetExcelWorkbook(true,fpsDefectReport.Sheets.Count+1);

			// k = 그리드상의 Sheet Count (0부터시작함)
			// i,j 엑셀에 삽입할 배열의 가로 세로 인덱스 (0부터시작)

			//엑셀시트에 각 그리드시트 삽입.
			string[,] strHeader;

			strHeader=new string[1,lstColumns.CheckedItems.Count+iDfFieldCnt+1];

			int intHeaderCol;

			for(int k=0;k<fpsDefectReport.Sheets.Count;k++)
			{
				//  헤더 Display를 위해 선택된 컬럼을 가져온다.
				intHeaderCol=0;

				strHeader[0,0]=fpsDefectReport.Sheets[k].SheetName;
				oXL.fnSetValue(oWB,k+2,1,1,1,1,strHeader, true,"Times New Roman",16,Miracom.Common.Win.ExcelUtil.Color.black, Miracom.Common.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.Common.Win.ExcelUtil.HAlignment.xlHAlignLeft, false, Miracom.Common.Win.ExcelUtil.Color.white);

				if((k%2)>0)
				{
					strHeader[0,0]="Scan Date";
					strHeader[0,1]="Def Count";
					strHeader[0,2]="Scan Date";
					strHeader[0,3]="DR Count";
					oXL.fnSetValue(oWB,k+2,2,1,2,4, strHeader, true,"굴림체",10,Miracom.Common.Win.ExcelUtil.Color.black,Miracom.Common.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.Common.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.Common.Win.ExcelUtil.Color.pastelorange);
				}
				else
				{
					for(int i=0;i<this.lstColumns.Items.Count;i++)
					{
						if (lstColumns.GetItemChecked(i)==true)
						{

							strHeader[0,intHeaderCol]=strColumnList[i,0];
							intHeaderCol+=1;
						}
					}

					for(int i=iStaticColumn;i<this.lstColumns.Items.Count;i++)
					{
						if (lstColumns.GetItemChecked(i)==true)
						{
							strHeader[0,intHeaderCol]=strColumnList[i,0]+"#(RB)";
							intHeaderCol+=1;
						}
					}
					strHeader[0,lstColumns.CheckedItems.Count+iDfFieldCnt] = "STEP_SEQ";
					oXL.fnSetValue(oWB,k+2,2,1,2,this.lstColumns.CheckedItems.Count+iDfFieldCnt+1, strHeader, true,"굴림체",10,Miracom.Common.Win.ExcelUtil.Color.white,Miracom.Common.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.Common.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.Common.Win.ExcelUtil.Color.pastelsky);
				}

				if(fpsDefectReport.Sheets[k].RowCount>0)
				{
					strExportList=new string[fpsDefectReport.Sheets[k].RowCount,fpsDefectReport.Sheets[k].ColumnCount];
					for(int i=0;i<fpsDefectReport.Sheets[k].RowCount;i++)
					{
						for(int j=0;j<fpsDefectReport.Sheets[k].ColumnCount;j++)
						{
							strExportList[i,j]=fpsDefectReport.Sheets[k].GetText(i,j).ToString();
						}
					}
					oXL.fnSetValue(oWB,k+2,3,1,fpsDefectReport.Sheets[k].RowCount+2,fpsDefectReport.Sheets[k].ColumnCount,strExportList,false,"굴림체",10,Miracom.Common.Win.ExcelUtil.Color.black,Miracom.Common.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.Common.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.Common.Win.ExcelUtil.Color.white);
				}
				oXL.fnSetSheetName(oWB,k+2,fpsDefectReport.Sheets[k].SheetName);
			}

			//Chart Export
			int intXIndex=3;
			oXL.fnSetSheetName(oWB,1,"Chart");
			strHeader[0,0]="Defect Count / Defective Die Count Chart";
			oXL.fnSetValue(oWB,1,1,1,1,1,strHeader, true,"Times New Roman", 16, Miracom.Common.Win.ExcelUtil.Color.black, Miracom.Common.Win.ExcelUtil.Alignment.xlVAlignCenter,Miracom.Common.Win.ExcelUtil.HAlignment.xlHAlignLeft, false, Miracom.Common.Win.ExcelUtil.Color.white);

			for(int i=1;i<=fpsDefectReport.Sheets.Count;i+=2)
			{
				//해당 ('EQ_CNT') 시트의 챠트를 생성
				ShowChart(fpsDefectReport.Sheets[i-1].SheetName.ToString(),i);

				//생성된 챠트를 Excel 로 Export 
				oXL.fnSetChartFx(oWB,1,chtDefCount,intXIndex,2,1.0f, 1.0f);
				intXIndex+=15;
				oXL.fnSetChartFx(oWB,1,chtDieCount,intXIndex,2,1.0f, 1.0f);
				intXIndex+=15;
				//Class Chart, RoughBin Chart 추가
				oXL.fnSetChartFx(oWB,1,chtClass,intXIndex,2,1.0f, 1.0f);
				intXIndex+=15;
				oXL.fnSetChartFx(oWB,1,chtRoughBin,intXIndex,2,1.0f, 1.0f);
				intXIndex+=15;
			}

			oXL = null;
			Miracom.Common.Win.ExcelUtil.fnExcelProcessExit();	

		}
		#endregion

		private void chkLegBox_CheckedChanged(object sender, System.EventArgs e)
		{
		}

		private void chkLegBoxClass_CheckedChanged(object sender, System.EventArgs e)
		{
		}

		private void chkAllColumn_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.lstColumns.Items.Count==0)
			{
				chkAllColumn.Checked=false;
			}
			else
			{
				if(chkAllColumn.Checked==true)
				{
					for(int i=0;i<lstColumns.Items.Count;i++)
					{
						lstColumns.SetItemCheckState(i,CheckState.Checked);
					}
				}
				else
				{
					for(int i=0;i<lstColumns.Items.Count;i++)
					{
						lstColumns.SetItemCheckState(i,CheckState.Unchecked);
					}
				}
			}	
		}

		private void chtDefCount_InternalCommand(object sender, SoftwareFX.ChartFX.CommandUIEventArgs e)
		{

			int iSheetTabCalc = 0;
			int iSheetCalcTmp = 0;
			object oTmp = null;


			try
			{
				if(e.ID == 29449)
				{
					dDefectChartOldMaxYScale = chtDefCount.AxisY.Max;
				}
				else if(e.ID == 29696)
				{
					dDefectChartNewMaxYScale = chtDefCount.AxisY.Max;
					if(dDefectChartOldMaxYScale != dDefectChartNewMaxYScale)
					{
						for(int i=0;i<this.fpsDefectReport.Sheets.Count;i++)
						{
							if(fpsDefectReport.Sheets[i].SheetName == fpsDefectReport.ActiveSheet.SheetName)
							{
								if(i==0 || i==1)
								{
									iSheetTabCalc = 1;
								}
								else
								{
									iSheetCalcTmp = i%2;
									if(iSheetCalcTmp == 0)
									{
										iSheetTabCalc = i+1;
									}
									else
									{
										iSheetTabCalc = i;
									}
								}
							
								oTmp = htChartMaxScale[iSheetTabCalc.ToString()+"DF"];
								if(oTmp == null)
								{
									htChartMaxScale.Add(iSheetTabCalc.ToString()+"DF",dDefectChartNewMaxYScale);
								}
								else
								{
									htChartMaxScale.Remove(iSheetTabCalc.ToString()+"DF");
									htChartMaxScale.Add(iSheetTabCalc.ToString()+"DF",dDefectChartNewMaxYScale);
								}
								break;
							}
						}
					}
				}			
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
		}

		private void chtDieCount_InternalCommand(object sender, SoftwareFX.ChartFX.CommandUIEventArgs e)
		{

			int iSheetTabCalc = 0;
			int iSheetCalcTmp = 0;
			object oTmp = null;


			try
			{
				if(e.ID == 29449)
				{
					dDieChartOldMaxYScale = chtDieCount.AxisY.Max;
				}
				else if(e.ID == 29696)
				{
					dDieChartNewMaxYScale = chtDieCount.AxisY.Max;
					if(dDieChartOldMaxYScale != dDieChartNewMaxYScale)
					{
						for(int i=0;i<this.fpsDefectReport.Sheets.Count;i++)
						{
							if(fpsDefectReport.Sheets[i].SheetName == fpsDefectReport.ActiveSheet.SheetName)
							{
								if(i==0 || i==1)
								{
									iSheetTabCalc = 1;
								}
								else
								{
									iSheetCalcTmp = i%2;
									if(iSheetCalcTmp == 0)
									{
										iSheetTabCalc = i+1;
									}
									else
									{
										iSheetTabCalc = i;
									}
								}
							
								oTmp = htChartMaxScale[iSheetTabCalc.ToString()+"DC"];
								if(oTmp == null)
								{
									htChartMaxScale.Add(iSheetTabCalc.ToString()+"DC",dDieChartNewMaxYScale);
								}
								else
								{
									htChartMaxScale.Remove(iSheetTabCalc.ToString()+"DC");
									htChartMaxScale.Add(iSheetTabCalc.ToString()+"DC",dDieChartNewMaxYScale);
								}
								break;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
		}

		private void chtClass_InternalCommand(object sender, SoftwareFX.ChartFX.CommandUIEventArgs e)
		{

			int iSheetTabCalc = 0;
			int iSheetCalcTmp = 0;
			object oTmp = null;

			try
			{
				if(e.ID == 29449)
				{
					dClassChartOldMaxYScale = chtClass.AxisY.Max;
				}
				else if(e.ID == 29696)
				{
					dClassChartNewMaxYScale = chtClass.AxisY.Max;
					if(dClassChartOldMaxYScale != dClassChartNewMaxYScale)
					{
						for(int i=0;i<this.fpsDefectReport.Sheets.Count;i++)
						{
							if(fpsDefectReport.Sheets[i].SheetName == fpsDefectReport.ActiveSheet.SheetName)
							{
								if(i==0 || i==1)
								{
									iSheetTabCalc = 1;
								}
								else
								{
									iSheetCalcTmp = i%2;
									if(iSheetCalcTmp == 0)
									{
										iSheetTabCalc = i+1;
									}
									else
									{
										iSheetTabCalc = i;
									}
								}
							
								oTmp = htChartMaxScale[iSheetTabCalc.ToString()+"CS"];
								if(oTmp == null)
								{
									htChartMaxScale.Add(iSheetTabCalc.ToString()+"CS",dClassChartNewMaxYScale);
								}
								else
								{
									htChartMaxScale.Remove(iSheetTabCalc.ToString()+"CS");
									htChartMaxScale.Add(iSheetTabCalc.ToString()+"CS",dClassChartNewMaxYScale);
								}
								break;
							}
						}
					}
				}	
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
		}

		private void chtRoughBin_InternalCommand(object sender, SoftwareFX.ChartFX.CommandUIEventArgs e)
		{

			int iSheetTabCalc = 0;
			int iSheetCalcTmp = 0;
			object oTmp = null;

			try
			{
				if(e.ID == 29449)
				{
					dRBChartOldMaxYScale = chtRoughBin.AxisY.Max;
				}
				else if(e.ID == 29696)
				{
					dRBChartNewMaxYScale = chtRoughBin.AxisY.Max;
					if(dRBChartOldMaxYScale != dRBChartNewMaxYScale)
					{
						for(int i=0;i<this.fpsDefectReport.Sheets.Count;i++)
						{
							if(fpsDefectReport.Sheets[i].SheetName == fpsDefectReport.ActiveSheet.SheetName)
							{

								if(i==0 || i==1)
								{
									iSheetTabCalc = 1;
								}
								else
								{
									iSheetCalcTmp = i%2;
									MessageBox.Show(iSheetCalcTmp.ToString());
									if(iSheetCalcTmp == 0)
									{
										iSheetTabCalc = i+1;
									}
									else
									{
										iSheetTabCalc = i;
									}
								}
								oTmp = htChartMaxScale[iSheetTabCalc.ToString()+"RB"];
								if(oTmp == null)
								{
									htChartMaxScale.Add(iSheetTabCalc.ToString()+"RB",dRBChartNewMaxYScale);
								}
								else
								{
									htChartMaxScale.Remove(iSheetTabCalc.ToString()+"RB");
									htChartMaxScale.Add(iSheetTabCalc.ToString()+"RB",dRBChartNewMaxYScale);
								}
								break;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
	
		}
	}
}
