using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Reflection; 
using Excel = Microsoft.Office.Interop.Excel; 
using System.Diagnostics;

namespace Miracom.DMS.LIB.Report
{
	/// <Summary>
	/// <b>■ Trend Report </b><br>
	/// - 작  성  자 : 미라콤 곽동일<br>
	/// - 최초작성일 : 2004년 09월 14일<br>
	/// - 최종수정자 : <br>
	/// - 최종수정일 : <br>
	/// - 주요변경로그<br>
	///   2004.09.14 생성<br>
	/// </Summary>
	/// <Remarks>없음</Remarks>
	/// 
	public class TrendReport : System.Windows.Forms.UserControl
	{
		public string strUserID="";
		int iCateFlag;

		private static string strFromDate="";
		private static string strToDate="";
		private static string strFromTime="";
		private static string strToTime="";
		private static int intSelectedRowCount=0;
		private static int intLayerSelectedRowCount=0;
		private static int intExcelRow=1;
		private static int intExcelCol=1;

		private static int intDefaultRowCount=0;
		private static int intSelectedSelectedRowCount=0;

		private string[,] strStepSeqList;

		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.GroupBox grpPeriod;
		private System.Windows.Forms.ComboBox cmbSample;
		private System.Windows.Forms.Label lblSample;
		private System.Windows.Forms.Label lblMark1;
		private System.Windows.Forms.DateTimePicker dtpToTime;
		private System.Windows.Forms.DateTimePicker dtpFromTime;
		private System.Windows.Forms.Label lblShift;
		private System.Windows.Forms.ComboBox cmbShift;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.GroupBox grpDefault;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.ComboBox cmbGroup;
		private System.Windows.Forms.Button btnDefaultClear;
		private System.Windows.Forms.Button btnGetLayer;
		private System.Windows.Forms.Button btnSelectLayer;
		private System.Windows.Forms.ListBox lstDevice;
		private System.Windows.Forms.Button btnClearLayer;
		private System.Windows.Forms.Button btnClearSelected;
		private System.Windows.Forms.Button btnGetDevice;
		private System.Windows.Forms.Button btnToDefault;
		private System.Windows.Forms.Label lblStartDate;
		private System.Windows.Forms.Label lblLayerCount;
		private System.Windows.Forms.TextBox txtLayerCount;
		private System.Windows.Forms.Button btnSelectAll;
		private System.Windows.Forms.Label lblSelectedCount;
		private System.Windows.Forms.TextBox txtSelectedCount;
		private System.Windows.Forms.Label lblDefaultAddedCounter;
		private System.Windows.Forms.TextBox txtDefaultAddCount;
		private C1.Win.C1FlexGrid.C1FlexGrid cdgDefault;
		private C1.Win.C1FlexGrid.C1FlexGrid cdgSpec;
		private System.Windows.Forms.Button btnDefaultRefresh;
		private System.Windows.Forms.ComboBox cmbDevice;
		private System.Windows.Forms.Label lblEndDate;
		private System.Windows.Forms.Label lblTime;
		private System.Windows.Forms.Button btnDefaultSave;
		private System.Windows.Forms.Label lblDefaultCount;
		private System.Windows.Forms.TextBox txtDefaultCount;
		private System.Windows.Forms.Label lblDefaultChangedCounter;
		private System.Windows.Forms.TextBox txtDefaultChangedCount;
		private System.Windows.Forms.Button btnDefaultSaveAs;
		private System.Windows.Forms.GroupBox grpLayer;
		private System.Windows.Forms.GroupBox grpSelected;
		private System.Windows.Forms.GroupBox grpSpecOption;
		private System.Windows.Forms.Label lblGroup;
		private System.Windows.Forms.Label lblDevice;
		private System.Windows.Forms.GroupBox grpDevice;
		private System.Windows.Forms.GroupBox grpOption;
		private System.Windows.Forms.Button btnToExcel;
		private C1.Win.C1FlexGrid.C1FlexGrid cdgLayer;
		private C1.Win.C1FlexGrid.C1FlexGrid cdgSelected;
		private System.Windows.Forms.Button btnSelectLayerAll;
		private System.Windows.Forms.GroupBox grpOption3;
		private System.Windows.Forms.GroupBox grpOption2;
		private System.Windows.Forms.GroupBox grpOption1;
		private System.Windows.Forms.CheckBox chkLimit;
		private System.Windows.Forms.GroupBox grpOption4;
		private System.Windows.Forms.Label lblDay;
		private System.Windows.Forms.Label lblMapPeriod;
		private System.Windows.Forms.Label lblSpecOptionCount;
		private System.Windows.Forms.TextBox txtSpecOptionCount;
		private System.Windows.Forms.NumericUpDown nudMapPeriod;
		private System.Windows.Forms.GroupBox grpEdit;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.NumericUpDown nudUnit;
		private System.Windows.Forms.NumericUpDown nudMin;
		private System.Windows.Forms.NumericUpDown nudMax;
		private System.Windows.Forms.Label lblUnit;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label lblMax;
		private System.Windows.Forms.Button btnDefaultEdit;
		private System.Windows.Forms.GroupBox grpEditS;
		private System.Windows.Forms.Button btnDeleteS;
		private System.Windows.Forms.NumericUpDown nudUnitS;
		private System.Windows.Forms.NumericUpDown nudMinS;
		private System.Windows.Forms.NumericUpDown nudMaxS;
		private System.Windows.Forms.Label lblUnitS;
		private System.Windows.Forms.Label lblMinS;
		private System.Windows.Forms.Label lblMaxS;
		private System.Windows.Forms.Button btnEditS;
		private System.Windows.Forms.Label lblEng;
		private System.Windows.Forms.Label lblCtrl;
		private System.Windows.Forms.NumericUpDown nudEng;
		private System.Windows.Forms.NumericUpDown nudCtrl;
		private System.Windows.Forms.NumericUpDown nudCtrlS;
		private System.Windows.Forms.NumericUpDown nudEngS;
		private System.Windows.Forms.Label lblCtrlS;
		private System.Windows.Forms.Label lblEngS;
		private System.Windows.Forms.Button btnDefaultSelectAll;
		private System.Windows.Forms.TextBox txtSelectedChangedCount;
		private System.Windows.Forms.Label lblSelectedChangedCounter;
		private System.Windows.Forms.GroupBox grpSchedule;
		private System.Windows.Forms.RadioButton rdoLog;
		private System.Windows.Forms.RadioButton rdoLinear;
		private C1.Win.C1FlexGrid.C1FlexGrid cdgSchedule;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnDeleteSchedule;
		private System.Windows.Forms.Label lblScheduleTime;
		private System.Windows.Forms.DateTimePicker dtpScheduleDate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker dtpScheduleTime;
		private System.Windows.Forms.ComboBox cmbScheduleGroup;
		private System.Windows.Forms.Label lblScheduleGroup;
		private System.Windows.Forms.Button btnDeleteGroup;
		private System.Windows.Forms.TextBox txtNewGroup;
		private System.Windows.Forms.Button btnRegistSchedule;
		private System.Windows.Forms.RadioButton rdoSelected;
		private System.Windows.Forms.RadioButton rdoDefault;
		private System.Windows.Forms.RadioButton rdoSpec3;
		private System.Windows.Forms.RadioButton rdoSpec2;
		private System.Windows.Forms.RadioButton rdoSpec1;
		private SoftwareFX.ChartFX.Chart chtChart;
		private System.Windows.Forms.NumericUpDown nudLogUnit;
		private System.Windows.Forms.NumericUpDown nudLogMax;
		private System.Windows.Forms.CheckBox chkTot;
		private System.Windows.Forms.CheckBox chkDR;
		private System.Windows.Forms.CheckBox chkDR5;
		private System.Windows.Forms.CheckBox chkTot5;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;

		private System.ComponentModel.Container components = null;

		#region ■ GetUserID

		/// <summary>
		/// 1) HTML 단에서 세션변수를 받아 UserID 변수로 저장한다.
		/// 2) 해당 사용자가 저장해둔 Custom Group List 를 가져오는 서비스 호출
		/// 3) 해당 사용자가 지정한 Schedule List 를 가져오는 서비스 호출
		/// </summary>
		/// <param name="strUser">사용자 ID</param>
		/// 
		public void GetUserID(string strUser)
		{
			strUserID=strUser;
			GetGroupList();
			GetScheduleList();
		}
		#endregion

		#region ■ 초기 생성자

		/// <summary>
		/// 1) Shift 관련 콤보 초기화
		/// 2) 날짜선택 콤보 초기화
		/// 3) 각 Grid 초기화 서비스 호출
		/// </summary>
		/// 
		public TrendReport()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

			cmbShift.Items.Clear();

			cmbShift.Items.Add("ALL");
			cmbShift.Items.Add("A");
			cmbShift.Items.Add("B");
			cmbShift.Items.Add("C");
			cmbShift.Items.Add("Custom");
 

			dtpFromDate.Value = System.DateTime.Now.AddDays(-1);
			dtpToDate.Value = System.DateTime.Now;
			
			dtpFromTime.Text="06:00";
			dtpToTime.Text="06:00";

			ResetDefaultLayerGrid();
			ResetSelectedLayerGrid();
			ResetLayerGrid();
			ResetSpecGrid();
			ResetScheduleGrid();

			// cmbGroup 콤보설정
			cmbGroup.Items.Add("Select Group");
			cmbScheduleGroup.Items.Add("Select Group");

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
				if(components != null)
				{
					components.Dispose();
				}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TrendReport));
			SoftwareFX.ChartFX.SeriesAttributes seriesAttributes1 = new SoftwareFX.ChartFX.SeriesAttributes();
			SoftwareFX.ChartFX.SeriesAttributes seriesAttributes2 = new SoftwareFX.ChartFX.SeriesAttributes();
			SoftwareFX.ChartFX.TitleDockable titleDockable1 = new SoftwareFX.ChartFX.TitleDockable();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.grpPeriod = new System.Windows.Forms.GroupBox();
			this.lblTime = new System.Windows.Forms.Label();
			this.cmbSample = new System.Windows.Forms.ComboBox();
			this.lblSample = new System.Windows.Forms.Label();
			this.dtpToTime = new System.Windows.Forms.DateTimePicker();
			this.dtpFromTime = new System.Windows.Forms.DateTimePicker();
			this.cmbShift = new System.Windows.Forms.ComboBox();
			this.dtpToDate = new System.Windows.Forms.DateTimePicker();
			this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
			this.lblStartDate = new System.Windows.Forms.Label();
			this.lblEndDate = new System.Windows.Forms.Label();
			this.lblShift = new System.Windows.Forms.Label();
			this.lblMark1 = new System.Windows.Forms.Label();
			this.grpLayer = new System.Windows.Forms.GroupBox();
			this.btnSelectLayerAll = new System.Windows.Forms.Button();
			this.cdgLayer = new C1.Win.C1FlexGrid.C1FlexGrid();
			this.btnClearLayer = new System.Windows.Forms.Button();
			this.lblLayerCount = new System.Windows.Forms.Label();
			this.txtLayerCount = new System.Windows.Forms.TextBox();
			this.grpSelected = new System.Windows.Forms.GroupBox();
			this.txtSelectedChangedCount = new System.Windows.Forms.TextBox();
			this.lblSelectedChangedCounter = new System.Windows.Forms.Label();
			this.grpEditS = new System.Windows.Forms.GroupBox();
			this.nudCtrlS = new System.Windows.Forms.NumericUpDown();
			this.nudEngS = new System.Windows.Forms.NumericUpDown();
			this.lblCtrlS = new System.Windows.Forms.Label();
			this.lblEngS = new System.Windows.Forms.Label();
			this.btnDeleteS = new System.Windows.Forms.Button();
			this.nudUnitS = new System.Windows.Forms.NumericUpDown();
			this.nudMinS = new System.Windows.Forms.NumericUpDown();
			this.nudMaxS = new System.Windows.Forms.NumericUpDown();
			this.lblUnitS = new System.Windows.Forms.Label();
			this.lblMinS = new System.Windows.Forms.Label();
			this.lblMaxS = new System.Windows.Forms.Label();
			this.btnEditS = new System.Windows.Forms.Button();
			this.cdgSelected = new C1.Win.C1FlexGrid.C1FlexGrid();
			this.btnClearSelected = new System.Windows.Forms.Button();
			this.btnSelectAll = new System.Windows.Forms.Button();
			this.txtSelectedCount = new System.Windows.Forms.TextBox();
			this.lblSelectedCount = new System.Windows.Forms.Label();
			this.grpDefault = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnDown = new System.Windows.Forms.Button();
			this.btnUp = new System.Windows.Forms.Button();
			this.txtNewGroup = new System.Windows.Forms.TextBox();
			this.btnDefaultSelectAll = new System.Windows.Forms.Button();
			this.grpEdit = new System.Windows.Forms.GroupBox();
			this.btnDeleteGroup = new System.Windows.Forms.Button();
			this.nudCtrl = new System.Windows.Forms.NumericUpDown();
			this.nudEng = new System.Windows.Forms.NumericUpDown();
			this.lblCtrl = new System.Windows.Forms.Label();
			this.lblEng = new System.Windows.Forms.Label();
			this.btnDelete = new System.Windows.Forms.Button();
			this.nudUnit = new System.Windows.Forms.NumericUpDown();
			this.nudMin = new System.Windows.Forms.NumericUpDown();
			this.nudMax = new System.Windows.Forms.NumericUpDown();
			this.lblUnit = new System.Windows.Forms.Label();
			this.lblMin = new System.Windows.Forms.Label();
			this.lblMax = new System.Windows.Forms.Label();
			this.btnDefaultEdit = new System.Windows.Forms.Button();
			this.btnDefaultSaveAs = new System.Windows.Forms.Button();
			this.txtDefaultChangedCount = new System.Windows.Forms.TextBox();
			this.lblDefaultCount = new System.Windows.Forms.Label();
			this.txtDefaultCount = new System.Windows.Forms.TextBox();
			this.btnDefaultSave = new System.Windows.Forms.Button();
			this.btnDefaultRefresh = new System.Windows.Forms.Button();
			this.btnDefaultClear = new System.Windows.Forms.Button();
			this.cmbGroup = new System.Windows.Forms.ComboBox();
			this.lblGroup = new System.Windows.Forms.Label();
			this.txtDefaultAddCount = new System.Windows.Forms.TextBox();
			this.cdgDefault = new C1.Win.C1FlexGrid.C1FlexGrid();
			this.lblDefaultChangedCounter = new System.Windows.Forms.Label();
			this.lblDefaultAddedCounter = new System.Windows.Forms.Label();
			this.grpSpecOption = new System.Windows.Forms.GroupBox();
			this.lblSpecOptionCount = new System.Windows.Forms.Label();
			this.txtSpecOptionCount = new System.Windows.Forms.TextBox();
			this.cmbDevice = new System.Windows.Forms.ComboBox();
			this.lblDevice = new System.Windows.Forms.Label();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.cdgSpec = new C1.Win.C1FlexGrid.C1FlexGrid();
			this.btnGetDevice = new System.Windows.Forms.Button();
			this.grpDevice = new System.Windows.Forms.GroupBox();
			this.lstDevice = new System.Windows.Forms.ListBox();
			this.btnGetLayer = new System.Windows.Forms.Button();
			this.btnToDefault = new System.Windows.Forms.Button();
			this.btnSelectLayer = new System.Windows.Forms.Button();
			this.grpOption = new System.Windows.Forms.GroupBox();
			this.grpOption2 = new System.Windows.Forms.GroupBox();
			this.chkDR5 = new System.Windows.Forms.CheckBox();
			this.chkTot5 = new System.Windows.Forms.CheckBox();
			this.chkDR = new System.Windows.Forms.CheckBox();
			this.chkTot = new System.Windows.Forms.CheckBox();
			this.grpSchedule = new System.Windows.Forms.GroupBox();
			this.cmbScheduleGroup = new System.Windows.Forms.ComboBox();
			this.lblScheduleGroup = new System.Windows.Forms.Label();
			this.dtpScheduleTime = new System.Windows.Forms.DateTimePicker();
			this.label3 = new System.Windows.Forms.Label();
			this.dtpScheduleDate = new System.Windows.Forms.DateTimePicker();
			this.lblScheduleTime = new System.Windows.Forms.Label();
			this.btnDeleteSchedule = new System.Windows.Forms.Button();
			this.btnRegistSchedule = new System.Windows.Forms.Button();
			this.cdgSchedule = new C1.Win.C1FlexGrid.C1FlexGrid();
			this.nudMapPeriod = new System.Windows.Forms.NumericUpDown();
			this.lblDay = new System.Windows.Forms.Label();
			this.chkLimit = new System.Windows.Forms.CheckBox();
			this.grpOption3 = new System.Windows.Forms.GroupBox();
			this.rdoSelected = new System.Windows.Forms.RadioButton();
			this.rdoDefault = new System.Windows.Forms.RadioButton();
			this.grpOption1 = new System.Windows.Forms.GroupBox();
			this.rdoSpec3 = new System.Windows.Forms.RadioButton();
			this.rdoSpec2 = new System.Windows.Forms.RadioButton();
			this.rdoSpec1 = new System.Windows.Forms.RadioButton();
			this.btnToExcel = new System.Windows.Forms.Button();
			this.grpOption4 = new System.Windows.Forms.GroupBox();
			this.nudLogUnit = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.nudLogMax = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.rdoLinear = new System.Windows.Forms.RadioButton();
			this.rdoLog = new System.Windows.Forms.RadioButton();
			this.lblMapPeriod = new System.Windows.Forms.Label();
			this.chtChart = new SoftwareFX.ChartFX.Chart();
			this.grpPeriod.SuspendLayout();
			this.grpLayer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cdgLayer)).BeginInit();
			this.grpSelected.SuspendLayout();
			this.grpEditS.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCtrlS)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEngS)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudUnitS)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinS)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxS)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cdgSelected)).BeginInit();
			this.grpDefault.SuspendLayout();
			this.panel1.SuspendLayout();
			this.grpEdit.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCtrl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEng)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudUnit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cdgDefault)).BeginInit();
			this.grpSpecOption.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cdgSpec)).BeginInit();
			this.grpDevice.SuspendLayout();
			this.grpOption.SuspendLayout();
			this.grpOption2.SuspendLayout();
			this.grpSchedule.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cdgSchedule)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMapPeriod)).BeginInit();
			this.grpOption3.SuspendLayout();
			this.grpOption1.SuspendLayout();
			this.grpOption4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudLogUnit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudLogMax)).BeginInit();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(225, 58);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(87, 20);
			this.comboBox1.TabIndex = 0;
			// 
			// grpPeriod
			// 
			this.grpPeriod.Controls.Add(this.lblTime);
			this.grpPeriod.Controls.Add(this.cmbSample);
			this.grpPeriod.Controls.Add(this.lblSample);
			this.grpPeriod.Controls.Add(this.dtpToTime);
			this.grpPeriod.Controls.Add(this.dtpFromTime);
			this.grpPeriod.Controls.Add(this.cmbShift);
			this.grpPeriod.Controls.Add(this.dtpToDate);
			this.grpPeriod.Controls.Add(this.dtpFromDate);
			this.grpPeriod.Controls.Add(this.lblStartDate);
			this.grpPeriod.Controls.Add(this.lblEndDate);
			this.grpPeriod.Controls.Add(this.lblShift);
			this.grpPeriod.Controls.Add(this.lblMark1);
			this.grpPeriod.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpPeriod.Location = new System.Drawing.Point(4, 4);
			this.grpPeriod.Name = "grpPeriod";
			this.grpPeriod.Size = new System.Drawing.Size(281, 190);
			this.grpPeriod.TabIndex = 24;
			this.grpPeriod.TabStop = false;
			this.grpPeriod.Text = "Select Period    ";
			// 
			// lblTime
			// 
			this.lblTime.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblTime.Location = new System.Drawing.Point(40, 120);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(33, 12);
			this.lblTime.TabIndex = 32;
			this.lblTime.Text = "Time";
			// 
			// cmbSample
			// 
			this.cmbSample.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.cmbSample.Location = new System.Drawing.Point(83, 160);
			this.cmbSample.Name = "cmbSample";
			this.cmbSample.Size = new System.Drawing.Size(78, 20);
			this.cmbSample.TabIndex = 30;
			this.cmbSample.Text = "ALL";
			// 
			// lblSample
			// 
			this.lblSample.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblSample.Location = new System.Drawing.Point(27, 165);
			this.lblSample.Name = "lblSample";
			this.lblSample.Size = new System.Drawing.Size(59, 12);
			this.lblSample.TabIndex = 29;
			this.lblSample.Text = "Sample";
			// 
			// dtpToTime
			// 
			this.dtpToTime.CustomFormat = "HH:mm";
			this.dtpToTime.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.dtpToTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpToTime.Location = new System.Drawing.Point(155, 115);
			this.dtpToTime.Name = "dtpToTime";
			this.dtpToTime.ShowUpDown = true;
			this.dtpToTime.Size = new System.Drawing.Size(56, 21);
			this.dtpToTime.TabIndex = 27;
			this.dtpToTime.ValueChanged += new System.EventHandler(this.dtpToTime_ValueChanged);
			// 
			// dtpFromTime
			// 
			this.dtpFromTime.CustomFormat = "HH:mm";
			this.dtpFromTime.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.dtpFromTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpFromTime.Location = new System.Drawing.Point(83, 115);
			this.dtpFromTime.Name = "dtpFromTime";
			this.dtpFromTime.ShowUpDown = true;
			this.dtpFromTime.Size = new System.Drawing.Size(56, 21);
			this.dtpFromTime.TabIndex = 26;
			this.dtpFromTime.ValueChanged += new System.EventHandler(this.dtpFromTime_ValueChanged);
			// 
			// cmbShift
			// 
			this.cmbShift.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.cmbShift.Location = new System.Drawing.Point(83, 88);
			this.cmbShift.Name = "cmbShift";
			this.cmbShift.Size = new System.Drawing.Size(68, 20);
			this.cmbShift.TabIndex = 23;
			this.cmbShift.Text = "ALL";
			this.cmbShift.SelectedIndexChanged += new System.EventHandler(this.cmbShift_SelectedIndexChanged);
			// 
			// dtpToDate
			// 
			this.dtpToDate.CustomFormat = "";
			this.dtpToDate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.dtpToDate.Location = new System.Drawing.Point(83, 46);
			this.dtpToDate.Name = "dtpToDate";
			this.dtpToDate.Size = new System.Drawing.Size(159, 21);
			this.dtpToDate.TabIndex = 19;
			this.dtpToDate.Value = new System.DateTime(2004, 10, 4, 0, 0, 0, 0);
			// 
			// dtpFromDate
			// 
			this.dtpFromDate.CustomFormat = "";
			this.dtpFromDate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.dtpFromDate.Location = new System.Drawing.Point(83, 19);
			this.dtpFromDate.Name = "dtpFromDate";
			this.dtpFromDate.Size = new System.Drawing.Size(159, 21);
			this.dtpFromDate.TabIndex = 22;
			this.dtpFromDate.Value = new System.DateTime(2004, 10, 4, 0, 0, 0, 0);
			// 
			// lblStartDate
			// 
			this.lblStartDate.Font = new System.Drawing.Font("굴림", 9F);
			this.lblStartDate.Location = new System.Drawing.Point(15, 25);
			this.lblStartDate.Name = "lblStartDate";
			this.lblStartDate.Size = new System.Drawing.Size(62, 16);
			this.lblStartDate.TabIndex = 20;
			this.lblStartDate.Text = "Start Date";
			// 
			// lblEndDate
			// 
			this.lblEndDate.Font = new System.Drawing.Font("굴림", 9F);
			this.lblEndDate.Location = new System.Drawing.Point(18, 52);
			this.lblEndDate.Name = "lblEndDate";
			this.lblEndDate.Size = new System.Drawing.Size(59, 16);
			this.lblEndDate.TabIndex = 31;
			this.lblEndDate.Text = "End Date";
			// 
			// lblShift
			// 
			this.lblShift.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblShift.Location = new System.Drawing.Point(44, 95);
			this.lblShift.Name = "lblShift";
			this.lblShift.Size = new System.Drawing.Size(33, 12);
			this.lblShift.TabIndex = 24;
			this.lblShift.Text = "Shift";
			// 
			// lblMark1
			// 
			this.lblMark1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblMark1.Location = new System.Drawing.Point(140, 120);
			this.lblMark1.Name = "lblMark1";
			this.lblMark1.Size = new System.Drawing.Size(16, 12);
			this.lblMark1.TabIndex = 28;
			this.lblMark1.Text = "~";
			// 
			// grpLayer
			// 
			this.grpLayer.Controls.Add(this.btnSelectLayerAll);
			this.grpLayer.Controls.Add(this.cdgLayer);
			this.grpLayer.Controls.Add(this.btnClearLayer);
			this.grpLayer.Controls.Add(this.lblLayerCount);
			this.grpLayer.Controls.Add(this.txtLayerCount);
			this.grpLayer.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpLayer.Location = new System.Drawing.Point(519, 3);
			this.grpLayer.Name = "grpLayer";
			this.grpLayer.Size = new System.Drawing.Size(476, 190);
			this.grpLayer.TabIndex = 25;
			this.grpLayer.TabStop = false;
			this.grpLayer.Text = "Layer(Step) List";
			// 
			// btnSelectLayerAll
			// 
			this.btnSelectLayerAll.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSelectLayerAll.BackgroundImage")));
			this.btnSelectLayerAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSelectLayerAll.Font = new System.Drawing.Font("굴림", 9F);
			this.btnSelectLayerAll.Location = new System.Drawing.Point(305, 14);
			this.btnSelectLayerAll.Name = "btnSelectLayerAll";
			this.btnSelectLayerAll.Size = new System.Drawing.Size(91, 21);
			this.btnSelectLayerAll.TabIndex = 30;
			this.btnSelectLayerAll.Click += new System.EventHandler(this.btnSelectLayerAll_Click);
			// 
			// cdgLayer
			// 
			this.cdgLayer.BackColor = System.Drawing.SystemColors.Window;
			this.cdgLayer.ColumnInfo = "10,0,0,0,0,75,Columns:";
			this.cdgLayer.DragMode = C1.Win.C1FlexGrid.DragModeEnum.Automatic;
			this.cdgLayer.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Automatic;
			this.cdgLayer.Font = new System.Drawing.Font("굴림", 9F);
			this.cdgLayer.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cdgLayer.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
			this.cdgLayer.Location = new System.Drawing.Point(7, 40);
			this.cdgLayer.Name = "cdgLayer";
			this.cdgLayer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.cdgLayer.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
			this.cdgLayer.Size = new System.Drawing.Size(461, 143);
			this.cdgLayer.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(@"Fixed{BackColor:Control;ForeColor:ControlText;Border:Flat,1,ControlDark,Both;}	Highlight{BackColor:Highlight;ForeColor:HighlightText;}	Search{BackColor:Highlight;ForeColor:HighlightText;}	Frozen{BackColor:Beige;}	EmptyArea{BackColor:AppWorkspace;Border:Flat,1,ControlDarkDark,Both;}	GrandTotal{BackColor:Black;ForeColor:White;}	Subtotal0{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal1{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal2{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal3{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal4{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal5{BackColor:ControlDarkDark;ForeColor:White;}	");
			this.cdgLayer.TabIndex = 29;
			this.cdgLayer.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.cdgLayer_GiveFeedback);
			this.cdgLayer.DragLeave += new System.EventHandler(this.cdgLayer_DragLeave);
			// 
			// btnClearLayer
			// 
			this.btnClearLayer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClearLayer.BackgroundImage")));
			this.btnClearLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClearLayer.Font = new System.Drawing.Font("굴림", 9F);
			this.btnClearLayer.Location = new System.Drawing.Point(402, 14);
			this.btnClearLayer.Name = "btnClearLayer";
			this.btnClearLayer.Size = new System.Drawing.Size(67, 21);
			this.btnClearLayer.TabIndex = 4;
			this.btnClearLayer.Click += new System.EventHandler(this.btnClearLayer_Click);
			// 
			// lblLayerCount
			// 
			this.lblLayerCount.Font = new System.Drawing.Font("굴림", 9F);
			this.lblLayerCount.Location = new System.Drawing.Point(9, 19);
			this.lblLayerCount.Name = "lblLayerCount";
			this.lblLayerCount.Size = new System.Drawing.Size(39, 12);
			this.lblLayerCount.TabIndex = 3;
			this.lblLayerCount.Text = "Count";
			// 
			// txtLayerCount
			// 
			this.txtLayerCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLayerCount.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtLayerCount.Location = new System.Drawing.Point(51, 14);
			this.txtLayerCount.Name = "txtLayerCount";
			this.txtLayerCount.ReadOnly = true;
			this.txtLayerCount.Size = new System.Drawing.Size(66, 21);
			this.txtLayerCount.TabIndex = 2;
			this.txtLayerCount.Text = "0";
			this.txtLayerCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// grpSelected
			// 
			this.grpSelected.Controls.Add(this.txtSelectedChangedCount);
			this.grpSelected.Controls.Add(this.lblSelectedChangedCounter);
			this.grpSelected.Controls.Add(this.grpEditS);
			this.grpSelected.Controls.Add(this.cdgSelected);
			this.grpSelected.Controls.Add(this.btnClearSelected);
			this.grpSelected.Controls.Add(this.btnSelectAll);
			this.grpSelected.Controls.Add(this.txtSelectedCount);
			this.grpSelected.Controls.Add(this.lblSelectedCount);
			this.grpSelected.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpSelected.Location = new System.Drawing.Point(519, 214);
			this.grpSelected.Name = "grpSelected";
			this.grpSelected.Size = new System.Drawing.Size(475, 373);
			this.grpSelected.TabIndex = 26;
			this.grpSelected.TabStop = false;
			this.grpSelected.Text = "Selected Layer";
			// 
			// txtSelectedChangedCount
			// 
			this.txtSelectedChangedCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSelectedChangedCount.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtSelectedChangedCount.Location = new System.Drawing.Point(173, 14);
			this.txtSelectedChangedCount.Name = "txtSelectedChangedCount";
			this.txtSelectedChangedCount.ReadOnly = true;
			this.txtSelectedChangedCount.Size = new System.Drawing.Size(34, 21);
			this.txtSelectedChangedCount.TabIndex = 72;
			this.txtSelectedChangedCount.Text = "0";
			this.txtSelectedChangedCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblSelectedChangedCounter
			// 
			this.lblSelectedChangedCounter.Font = new System.Drawing.Font("굴림", 9F);
			this.lblSelectedChangedCounter.Location = new System.Drawing.Point(115, 19);
			this.lblSelectedChangedCounter.Name = "lblSelectedChangedCounter";
			this.lblSelectedChangedCounter.Size = new System.Drawing.Size(58, 12);
			this.lblSelectedChangedCounter.TabIndex = 73;
			this.lblSelectedChangedCounter.Text = "Changed";
			// 
			// grpEditS
			// 
			this.grpEditS.Controls.Add(this.nudCtrlS);
			this.grpEditS.Controls.Add(this.nudEngS);
			this.grpEditS.Controls.Add(this.lblCtrlS);
			this.grpEditS.Controls.Add(this.lblEngS);
			this.grpEditS.Controls.Add(this.btnDeleteS);
			this.grpEditS.Controls.Add(this.nudUnitS);
			this.grpEditS.Controls.Add(this.nudMinS);
			this.grpEditS.Controls.Add(this.nudMaxS);
			this.grpEditS.Controls.Add(this.lblUnitS);
			this.grpEditS.Controls.Add(this.lblMinS);
			this.grpEditS.Controls.Add(this.lblMaxS);
			this.grpEditS.Controls.Add(this.btnEditS);
			this.grpEditS.Font = new System.Drawing.Font("굴림", 9F);
			this.grpEditS.Location = new System.Drawing.Point(6, 277);
			this.grpEditS.Name = "grpEditS";
			this.grpEditS.Size = new System.Drawing.Size(462, 87);
			this.grpEditS.TabIndex = 71;
			this.grpEditS.TabStop = false;
			this.grpEditS.Text = "Edit Selected Row";
			// 
			// nudCtrlS
			// 
			this.nudCtrlS.Location = new System.Drawing.Point(233, 21);
			this.nudCtrlS.Maximum = new System.Decimal(new int[] {
																	 2000,
																	 0,
																	 0,
																	 0});
			this.nudCtrlS.Name = "nudCtrlS";
			this.nudCtrlS.Size = new System.Drawing.Size(70, 21);
			this.nudCtrlS.TabIndex = 78;
			this.nudCtrlS.Value = new System.Decimal(new int[] {
																   20,
																   0,
																   0,
																   0});
			// 
			// nudEngS
			// 
			this.nudEngS.Location = new System.Drawing.Point(72, 21);
			this.nudEngS.Maximum = new System.Decimal(new int[] {
																	2000,
																	0,
																	0,
																	0});
			this.nudEngS.Name = "nudEngS";
			this.nudEngS.Size = new System.Drawing.Size(73, 21);
			this.nudEngS.TabIndex = 77;
			this.nudEngS.Value = new System.Decimal(new int[] {
																  20,
																  0,
																  0,
																  0});
			// 
			// lblCtrlS
			// 
			this.lblCtrlS.Font = new System.Drawing.Font("굴림", 9F);
			this.lblCtrlS.Location = new System.Drawing.Point(171, 25);
			this.lblCtrlS.Name = "lblCtrlS";
			this.lblCtrlS.Size = new System.Drawing.Size(63, 13);
			this.lblCtrlS.TabIndex = 76;
			this.lblCtrlS.Text = "Ctrl Limit";
			// 
			// lblEngS
			// 
			this.lblEngS.Font = new System.Drawing.Font("굴림", 9F);
			this.lblEngS.Location = new System.Drawing.Point(7, 26);
			this.lblEngS.Name = "lblEngS";
			this.lblEngS.Size = new System.Drawing.Size(72, 13);
			this.lblEngS.TabIndex = 75;
			this.lblEngS.Text = "Eng Limit";
			// 
			// btnDeleteS
			// 
			this.btnDeleteS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteS.BackgroundImage")));
			this.btnDeleteS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDeleteS.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDeleteS.Location = new System.Drawing.Point(382, 54);
			this.btnDeleteS.Name = "btnDeleteS";
			this.btnDeleteS.Size = new System.Drawing.Size(73, 21);
			this.btnDeleteS.TabIndex = 70;
			this.btnDeleteS.Click += new System.EventHandler(this.btnDeleteS_Click);
			// 
			// nudUnitS
			// 
			this.nudUnitS.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.nudUnitS.Increment = new System.Decimal(new int[] {
																	   5,
																	   0,
																	   0,
																	   0});
			this.nudUnitS.Location = new System.Drawing.Point(248, 54);
			this.nudUnitS.Maximum = new System.Decimal(new int[] {
																	 995,
																	 0,
																	 0,
																	 0});
			this.nudUnitS.Name = "nudUnitS";
			this.nudUnitS.Size = new System.Drawing.Size(55, 21);
			this.nudUnitS.TabIndex = 69;
			this.nudUnitS.ThousandsSeparator = true;
			this.nudUnitS.Value = new System.Decimal(new int[] {
																   20,
																   0,
																   0,
																   0});
			// 
			// nudMinS
			// 
			this.nudMinS.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.nudMinS.Increment = new System.Decimal(new int[] {
																	  10,
																	  0,
																	  0,
																	  0});
			this.nudMinS.Location = new System.Drawing.Point(150, 54);
			this.nudMinS.Maximum = new System.Decimal(new int[] {
																	1000,
																	0,
																	0,
																	0});
			this.nudMinS.Name = "nudMinS";
			this.nudMinS.Size = new System.Drawing.Size(57, 21);
			this.nudMinS.TabIndex = 68;
			this.nudMinS.ThousandsSeparator = true;
			// 
			// nudMaxS
			// 
			this.nudMaxS.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.nudMaxS.Increment = new System.Decimal(new int[] {
																	  10,
																	  0,
																	  0,
																	  0});
			this.nudMaxS.Location = new System.Drawing.Point(41, 54);
			this.nudMaxS.Maximum = new System.Decimal(new int[] {
																	10000,
																	0,
																	0,
																	0});
			this.nudMaxS.Name = "nudMaxS";
			this.nudMaxS.Size = new System.Drawing.Size(72, 21);
			this.nudMaxS.TabIndex = 67;
			this.nudMaxS.Tag = "";
			this.nudMaxS.ThousandsSeparator = true;
			this.nudMaxS.Value = new System.Decimal(new int[] {
																  100,
																  0,
																  0,
																  0});
			// 
			// lblUnitS
			// 
			this.lblUnitS.Font = new System.Drawing.Font("굴림", 9F);
			this.lblUnitS.Location = new System.Drawing.Point(220, 59);
			this.lblUnitS.Name = "lblUnitS";
			this.lblUnitS.Size = new System.Drawing.Size(34, 16);
			this.lblUnitS.TabIndex = 66;
			this.lblUnitS.Text = "Unit";
			// 
			// lblMinS
			// 
			this.lblMinS.Font = new System.Drawing.Font("굴림", 9F);
			this.lblMinS.Location = new System.Drawing.Point(123, 59);
			this.lblMinS.Name = "lblMinS";
			this.lblMinS.Size = new System.Drawing.Size(34, 16);
			this.lblMinS.TabIndex = 65;
			this.lblMinS.Text = "Min";
			// 
			// lblMaxS
			// 
			this.lblMaxS.Font = new System.Drawing.Font("굴림", 9F);
			this.lblMaxS.Location = new System.Drawing.Point(7, 59);
			this.lblMaxS.Name = "lblMaxS";
			this.lblMaxS.Size = new System.Drawing.Size(34, 16);
			this.lblMaxS.TabIndex = 64;
			this.lblMaxS.Text = "Max";
			// 
			// btnEditS
			// 
			this.btnEditS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditS.BackgroundImage")));
			this.btnEditS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEditS.Font = new System.Drawing.Font("굴림", 9F);
			this.btnEditS.Location = new System.Drawing.Point(318, 54);
			this.btnEditS.Name = "btnEditS";
			this.btnEditS.Size = new System.Drawing.Size(59, 21);
			this.btnEditS.TabIndex = 63;
			this.btnEditS.Click += new System.EventHandler(this.btnEditS_Click);
			// 
			// cdgSelected
			// 
			this.cdgSelected.BackColor = System.Drawing.SystemColors.Window;
			this.cdgSelected.ColumnInfo = "10,0,0,0,0,75,Columns:";
			this.cdgSelected.DragMode = C1.Win.C1FlexGrid.DragModeEnum.Automatic;
			this.cdgSelected.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Automatic;
			this.cdgSelected.Font = new System.Drawing.Font("굴림", 9F);
			this.cdgSelected.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cdgSelected.Location = new System.Drawing.Point(7, 39);
			this.cdgSelected.Name = "cdgSelected";
			this.cdgSelected.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
			this.cdgSelected.Size = new System.Drawing.Size(460, 207);
			this.cdgSelected.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(@"Fixed{BackColor:Control;ForeColor:ControlText;Border:Flat,1,ControlDark,Both;}	Highlight{BackColor:Highlight;ForeColor:HighlightText;}	Search{BackColor:Highlight;ForeColor:HighlightText;}	Frozen{BackColor:Beige;}	EmptyArea{BackColor:AppWorkspace;Border:Flat,1,ControlDarkDark,Both;}	GrandTotal{BackColor:Black;ForeColor:White;}	Subtotal0{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal1{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal2{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal3{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal4{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal5{BackColor:ControlDarkDark;ForeColor:White;}	");
			this.cdgSelected.TabIndex = 9;
			this.cdgSelected.DragEnter += new System.Windows.Forms.DragEventHandler(this.cdgSelected_DragEnter);
			// 
			// btnClearSelected
			// 
			this.btnClearSelected.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClearSelected.BackgroundImage")));
			this.btnClearSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClearSelected.Font = new System.Drawing.Font("굴림", 9F);
			this.btnClearSelected.Location = new System.Drawing.Point(400, 251);
			this.btnClearSelected.Name = "btnClearSelected";
			this.btnClearSelected.Size = new System.Drawing.Size(67, 21);
			this.btnClearSelected.TabIndex = 8;
			this.btnClearSelected.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.btnClearSelected.Click += new System.EventHandler(this.btnClearSelected_Click);
			// 
			// btnSelectAll
			// 
			this.btnSelectAll.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSelectAll.BackgroundImage")));
			this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSelectAll.Font = new System.Drawing.Font("굴림", 9F);
			this.btnSelectAll.Location = new System.Drawing.Point(304, 251);
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Size = new System.Drawing.Size(91, 21);
			this.btnSelectAll.TabIndex = 6;
			this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
			// 
			// txtSelectedCount
			// 
			this.txtSelectedCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSelectedCount.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtSelectedCount.Location = new System.Drawing.Point(44, 15);
			this.txtSelectedCount.Name = "txtSelectedCount";
			this.txtSelectedCount.ReadOnly = true;
			this.txtSelectedCount.Size = new System.Drawing.Size(55, 21);
			this.txtSelectedCount.TabIndex = 4;
			this.txtSelectedCount.Text = "0";
			this.txtSelectedCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblSelectedCount
			// 
			this.lblSelectedCount.Font = new System.Drawing.Font("굴림", 9F);
			this.lblSelectedCount.Location = new System.Drawing.Point(6, 20);
			this.lblSelectedCount.Name = "lblSelectedCount";
			this.lblSelectedCount.Size = new System.Drawing.Size(39, 12);
			this.lblSelectedCount.TabIndex = 5;
			this.lblSelectedCount.Text = "Total";
			// 
			// grpDefault
			// 
			this.grpDefault.Controls.Add(this.panel1);
			this.grpDefault.Controls.Add(this.txtNewGroup);
			this.grpDefault.Controls.Add(this.btnDefaultSelectAll);
			this.grpDefault.Controls.Add(this.grpEdit);
			this.grpDefault.Controls.Add(this.btnDefaultSaveAs);
			this.grpDefault.Controls.Add(this.txtDefaultChangedCount);
			this.grpDefault.Controls.Add(this.lblDefaultCount);
			this.grpDefault.Controls.Add(this.txtDefaultCount);
			this.grpDefault.Controls.Add(this.btnDefaultSave);
			this.grpDefault.Controls.Add(this.btnDefaultRefresh);
			this.grpDefault.Controls.Add(this.btnDefaultClear);
			this.grpDefault.Controls.Add(this.cmbGroup);
			this.grpDefault.Controls.Add(this.lblGroup);
			this.grpDefault.Controls.Add(this.txtDefaultAddCount);
			this.grpDefault.Controls.Add(this.cdgDefault);
			this.grpDefault.Controls.Add(this.lblDefaultChangedCounter);
			this.grpDefault.Controls.Add(this.lblDefaultAddedCounter);
			this.grpDefault.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpDefault.Location = new System.Drawing.Point(4, 213);
			this.grpDefault.Name = "grpDefault";
			this.grpDefault.Size = new System.Drawing.Size(482, 374);
			this.grpDefault.TabIndex = 28;
			this.grpDefault.TabStop = false;
			this.grpDefault.Text = "Default";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.btnDown);
			this.panel1.Controls.Add(this.btnUp);
			this.panel1.Location = new System.Drawing.Point(459, 39);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(19, 208);
			this.panel1.TabIndex = 67;
			// 
			// btnDown
			// 
			this.btnDown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDown.BackgroundImage")));
			this.btnDown.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnDown.Location = new System.Drawing.Point(0, 190);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(15, 14);
			this.btnDown.TabIndex = 1;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// btnUp
			// 
			this.btnUp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUp.BackgroundImage")));
			this.btnUp.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnUp.Location = new System.Drawing.Point(0, 0);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(15, 15);
			this.btnUp.TabIndex = 0;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// txtNewGroup
			// 
			this.txtNewGroup.Font = new System.Drawing.Font("굴림", 9F);
			this.txtNewGroup.Location = new System.Drawing.Point(171, 251);
			this.txtNewGroup.Name = "txtNewGroup";
			this.txtNewGroup.Size = new System.Drawing.Size(72, 21);
			this.txtNewGroup.TabIndex = 66;
			this.txtNewGroup.Text = "NewGroup";
			// 
			// btnDefaultSelectAll
			// 
			this.btnDefaultSelectAll.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDefaultSelectAll.BackgroundImage")));
			this.btnDefaultSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDefaultSelectAll.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDefaultSelectAll.Location = new System.Drawing.Point(310, 250);
			this.btnDefaultSelectAll.Name = "btnDefaultSelectAll";
			this.btnDefaultSelectAll.Size = new System.Drawing.Size(91, 21);
			this.btnDefaultSelectAll.TabIndex = 65;
			this.btnDefaultSelectAll.Click += new System.EventHandler(this.btnDefaultSelectAll_Click);
			// 
			// grpEdit
			// 
			this.grpEdit.Controls.Add(this.btnDeleteGroup);
			this.grpEdit.Controls.Add(this.nudCtrl);
			this.grpEdit.Controls.Add(this.nudEng);
			this.grpEdit.Controls.Add(this.lblCtrl);
			this.grpEdit.Controls.Add(this.lblEng);
			this.grpEdit.Controls.Add(this.btnDelete);
			this.grpEdit.Controls.Add(this.nudUnit);
			this.grpEdit.Controls.Add(this.nudMin);
			this.grpEdit.Controls.Add(this.nudMax);
			this.grpEdit.Controls.Add(this.lblUnit);
			this.grpEdit.Controls.Add(this.lblMin);
			this.grpEdit.Controls.Add(this.lblMax);
			this.grpEdit.Controls.Add(this.btnDefaultEdit);
			this.grpEdit.Font = new System.Drawing.Font("굴림", 9F);
			this.grpEdit.Location = new System.Drawing.Point(8, 279);
			this.grpEdit.Name = "grpEdit";
			this.grpEdit.Size = new System.Drawing.Size(464, 86);
			this.grpEdit.TabIndex = 64;
			this.grpEdit.TabStop = false;
			this.grpEdit.Text = "Edit Selected Row";
			// 
			// btnDeleteGroup
			// 
			this.btnDeleteGroup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteGroup.BackgroundImage")));
			this.btnDeleteGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDeleteGroup.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDeleteGroup.Location = new System.Drawing.Point(346, 21);
			this.btnDeleteGroup.Name = "btnDeleteGroup";
			this.btnDeleteGroup.Size = new System.Drawing.Size(111, 22);
			this.btnDeleteGroup.TabIndex = 75;
			this.btnDeleteGroup.Click += new System.EventHandler(this.btnDeleteGroup_Click);
			// 
			// nudCtrl
			// 
			this.nudCtrl.Location = new System.Drawing.Point(215, 21);
			this.nudCtrl.Maximum = new System.Decimal(new int[] {
																	2000,
																	0,
																	0,
																	0});
			this.nudCtrl.Name = "nudCtrl";
			this.nudCtrl.Size = new System.Drawing.Size(88, 21);
			this.nudCtrl.TabIndex = 74;
			this.nudCtrl.Value = new System.Decimal(new int[] {
																  20,
																  0,
																  0,
																  0});
			// 
			// nudEng
			// 
			this.nudEng.Location = new System.Drawing.Point(69, 21);
			this.nudEng.Maximum = new System.Decimal(new int[] {
																   2000,
																   0,
																   0,
																   0});
			this.nudEng.Name = "nudEng";
			this.nudEng.Size = new System.Drawing.Size(73, 21);
			this.nudEng.TabIndex = 73;
			this.nudEng.Value = new System.Decimal(new int[] {
																 20,
																 0,
																 0,
																 0});
			// 
			// lblCtrl
			// 
			this.lblCtrl.Font = new System.Drawing.Font("굴림", 9F);
			this.lblCtrl.Location = new System.Drawing.Point(159, 26);
			this.lblCtrl.Name = "lblCtrl";
			this.lblCtrl.Size = new System.Drawing.Size(61, 13);
			this.lblCtrl.TabIndex = 72;
			this.lblCtrl.Text = "Ctrl Limit";
			// 
			// lblEng
			// 
			this.lblEng.Font = new System.Drawing.Font("굴림", 9F);
			this.lblEng.Location = new System.Drawing.Point(6, 26);
			this.lblEng.Name = "lblEng";
			this.lblEng.Size = new System.Drawing.Size(61, 13);
			this.lblEng.TabIndex = 71;
			this.lblEng.Text = "Eng Limit";
			// 
			// btnDelete
			// 
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDelete.Location = new System.Drawing.Point(385, 54);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(73, 21);
			this.btnDelete.TabIndex = 70;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// nudUnit
			// 
			this.nudUnit.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.nudUnit.Increment = new System.Decimal(new int[] {
																	  5,
																	  0,
																	  0,
																	  0});
			this.nudUnit.Location = new System.Drawing.Point(248, 54);
			this.nudUnit.Maximum = new System.Decimal(new int[] {
																	995,
																	0,
																	0,
																	0});
			this.nudUnit.Name = "nudUnit";
			this.nudUnit.Size = new System.Drawing.Size(55, 21);
			this.nudUnit.TabIndex = 69;
			this.nudUnit.ThousandsSeparator = true;
			this.nudUnit.Value = new System.Decimal(new int[] {
																  20,
																  0,
																  0,
																  0});
			// 
			// nudMin
			// 
			this.nudMin.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.nudMin.Increment = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
			this.nudMin.Location = new System.Drawing.Point(150, 54);
			this.nudMin.Maximum = new System.Decimal(new int[] {
																   1000,
																   0,
																   0,
																   0});
			this.nudMin.Name = "nudMin";
			this.nudMin.Size = new System.Drawing.Size(57, 21);
			this.nudMin.TabIndex = 68;
			this.nudMin.ThousandsSeparator = true;
			// 
			// nudMax
			// 
			this.nudMax.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.nudMax.Increment = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
			this.nudMax.Location = new System.Drawing.Point(41, 54);
			this.nudMax.Maximum = new System.Decimal(new int[] {
																   10000,
																   0,
																   0,
																   0});
			this.nudMax.Name = "nudMax";
			this.nudMax.Size = new System.Drawing.Size(72, 21);
			this.nudMax.TabIndex = 67;
			this.nudMax.Tag = "";
			this.nudMax.ThousandsSeparator = true;
			this.nudMax.Value = new System.Decimal(new int[] {
																 100,
																 0,
																 0,
																 0});
			// 
			// lblUnit
			// 
			this.lblUnit.Font = new System.Drawing.Font("굴림", 9F);
			this.lblUnit.Location = new System.Drawing.Point(220, 59);
			this.lblUnit.Name = "lblUnit";
			this.lblUnit.Size = new System.Drawing.Size(34, 13);
			this.lblUnit.TabIndex = 66;
			this.lblUnit.Text = "Unit";
			// 
			// lblMin
			// 
			this.lblMin.Font = new System.Drawing.Font("굴림", 9F);
			this.lblMin.Location = new System.Drawing.Point(123, 59);
			this.lblMin.Name = "lblMin";
			this.lblMin.Size = new System.Drawing.Size(34, 13);
			this.lblMin.TabIndex = 65;
			this.lblMin.Text = "Min";
			// 
			// lblMax
			// 
			this.lblMax.Font = new System.Drawing.Font("굴림", 9F);
			this.lblMax.Location = new System.Drawing.Point(7, 59);
			this.lblMax.Name = "lblMax";
			this.lblMax.Size = new System.Drawing.Size(34, 13);
			this.lblMax.TabIndex = 64;
			this.lblMax.Text = "Max";
			// 
			// btnDefaultEdit
			// 
			this.btnDefaultEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDefaultEdit.BackgroundImage")));
			this.btnDefaultEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDefaultEdit.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDefaultEdit.Location = new System.Drawing.Point(321, 54);
			this.btnDefaultEdit.Name = "btnDefaultEdit";
			this.btnDefaultEdit.Size = new System.Drawing.Size(59, 21);
			this.btnDefaultEdit.TabIndex = 63;
			this.btnDefaultEdit.Click += new System.EventHandler(this.btnDefaultEdit_Click);
			// 
			// btnDefaultSaveAs
			// 
			this.btnDefaultSaveAs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDefaultSaveAs.BackgroundImage")));
			this.btnDefaultSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDefaultSaveAs.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDefaultSaveAs.Location = new System.Drawing.Point(78, 250);
			this.btnDefaultSaveAs.Name = "btnDefaultSaveAs";
			this.btnDefaultSaveAs.Size = new System.Drawing.Size(84, 22);
			this.btnDefaultSaveAs.TabIndex = 53;
			this.btnDefaultSaveAs.Click += new System.EventHandler(this.btnDefaultSaveAs_Click);
			// 
			// txtDefaultChangedCount
			// 
			this.txtDefaultChangedCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtDefaultChangedCount.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtDefaultChangedCount.Location = new System.Drawing.Point(346, 15);
			this.txtDefaultChangedCount.Name = "txtDefaultChangedCount";
			this.txtDefaultChangedCount.ReadOnly = true;
			this.txtDefaultChangedCount.Size = new System.Drawing.Size(32, 21);
			this.txtDefaultChangedCount.TabIndex = 51;
			this.txtDefaultChangedCount.Text = "0";
			this.txtDefaultChangedCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblDefaultCount
			// 
			this.lblDefaultCount.Font = new System.Drawing.Font("굴림", 9F);
			this.lblDefaultCount.Location = new System.Drawing.Point(144, 21);
			this.lblDefaultCount.Name = "lblDefaultCount";
			this.lblDefaultCount.Size = new System.Drawing.Size(33, 12);
			this.lblDefaultCount.TabIndex = 50;
			this.lblDefaultCount.Text = "Total";
			// 
			// txtDefaultCount
			// 
			this.txtDefaultCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtDefaultCount.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtDefaultCount.Location = new System.Drawing.Point(178, 15);
			this.txtDefaultCount.Name = "txtDefaultCount";
			this.txtDefaultCount.ReadOnly = true;
			this.txtDefaultCount.Size = new System.Drawing.Size(38, 21);
			this.txtDefaultCount.TabIndex = 49;
			this.txtDefaultCount.Text = "0";
			this.txtDefaultCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnDefaultSave
			// 
			this.btnDefaultSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDefaultSave.BackgroundImage")));
			this.btnDefaultSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDefaultSave.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDefaultSave.Location = new System.Drawing.Point(8, 250);
			this.btnDefaultSave.Name = "btnDefaultSave";
			this.btnDefaultSave.Size = new System.Drawing.Size(65, 22);
			this.btnDefaultSave.TabIndex = 48;
			this.btnDefaultSave.Click += new System.EventHandler(this.btnDefaultSave_Click);
			// 
			// btnDefaultRefresh
			// 
			this.btnDefaultRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDefaultRefresh.BackgroundImage")));
			this.btnDefaultRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDefaultRefresh.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDefaultRefresh.Location = new System.Drawing.Point(394, 14);
			this.btnDefaultRefresh.Name = "btnDefaultRefresh";
			this.btnDefaultRefresh.Size = new System.Drawing.Size(81, 22);
			this.btnDefaultRefresh.TabIndex = 36;
			this.btnDefaultRefresh.Click += new System.EventHandler(this.btnDefaultRefresh_Click);
			// 
			// btnDefaultClear
			// 
			this.btnDefaultClear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDefaultClear.BackgroundImage")));
			this.btnDefaultClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDefaultClear.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDefaultClear.Location = new System.Drawing.Point(405, 250);
			this.btnDefaultClear.Name = "btnDefaultClear";
			this.btnDefaultClear.Size = new System.Drawing.Size(67, 21);
			this.btnDefaultClear.TabIndex = 35;
			this.btnDefaultClear.Click += new System.EventHandler(this.btnDefaultClear_Click);
			// 
			// cmbGroup
			// 
			this.cmbGroup.Font = new System.Drawing.Font("굴림", 9F);
			this.cmbGroup.Location = new System.Drawing.Point(46, 16);
			this.cmbGroup.Name = "cmbGroup";
			this.cmbGroup.Size = new System.Drawing.Size(97, 20);
			this.cmbGroup.TabIndex = 32;
			this.cmbGroup.Text = "Select Group";
			this.cmbGroup.SelectedIndexChanged += new System.EventHandler(this.cmbGroup_SelectedIndexChanged);
			// 
			// lblGroup
			// 
			this.lblGroup.Font = new System.Drawing.Font("굴림", 9F);
			this.lblGroup.Location = new System.Drawing.Point(6, 21);
			this.lblGroup.Name = "lblGroup";
			this.lblGroup.Size = new System.Drawing.Size(46, 14);
			this.lblGroup.TabIndex = 31;
			this.lblGroup.Text = "Group";
			// 
			// txtDefaultAddCount
			// 
			this.txtDefaultAddCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtDefaultAddCount.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtDefaultAddCount.Location = new System.Drawing.Point(250, 15);
			this.txtDefaultAddCount.Name = "txtDefaultAddCount";
			this.txtDefaultAddCount.ReadOnly = true;
			this.txtDefaultAddCount.Size = new System.Drawing.Size(37, 21);
			this.txtDefaultAddCount.TabIndex = 29;
			this.txtDefaultAddCount.Text = "0";
			this.txtDefaultAddCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// cdgDefault
			// 
			this.cdgDefault.BackColor = System.Drawing.SystemColors.Window;
			this.cdgDefault.ColumnInfo = "10,0,0,0,0,75,Columns:";
			this.cdgDefault.Font = new System.Drawing.Font("굴림", 9F);
			this.cdgDefault.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cdgDefault.Location = new System.Drawing.Point(8, 40);
			this.cdgDefault.Name = "cdgDefault";
			this.cdgDefault.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
			this.cdgDefault.Size = new System.Drawing.Size(451, 206);
			this.cdgDefault.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(@"Fixed{BackColor:Control;ForeColor:ControlText;Border:Flat,1,ControlDark,Both;}	Highlight{BackColor:Highlight;ForeColor:HighlightText;}	Search{BackColor:Highlight;ForeColor:HighlightText;}	Frozen{BackColor:Beige;}	EmptyArea{BackColor:AppWorkspace;Border:Flat,1,ControlDarkDark,Both;}	GrandTotal{BackColor:Black;ForeColor:White;}	Subtotal0{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal1{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal2{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal3{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal4{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal5{BackColor:ControlDarkDark;ForeColor:White;}	");
			this.cdgDefault.TabIndex = 28;
			// 
			// lblDefaultChangedCounter
			// 
			this.lblDefaultChangedCounter.Font = new System.Drawing.Font("굴림", 9F);
			this.lblDefaultChangedCounter.Location = new System.Drawing.Point(289, 20);
			this.lblDefaultChangedCounter.Name = "lblDefaultChangedCounter";
			this.lblDefaultChangedCounter.Size = new System.Drawing.Size(58, 12);
			this.lblDefaultChangedCounter.TabIndex = 52;
			this.lblDefaultChangedCounter.Text = "Changed";
			// 
			// lblDefaultAddedCounter
			// 
			this.lblDefaultAddedCounter.Font = new System.Drawing.Font("굴림", 9F);
			this.lblDefaultAddedCounter.Location = new System.Drawing.Point(221, 20);
			this.lblDefaultAddedCounter.Name = "lblDefaultAddedCounter";
			this.lblDefaultAddedCounter.Size = new System.Drawing.Size(42, 12);
			this.lblDefaultAddedCounter.TabIndex = 30;
			this.lblDefaultAddedCounter.Text = "Add";
			// 
			// grpSpecOption
			// 
			this.grpSpecOption.Controls.Add(this.lblSpecOptionCount);
			this.grpSpecOption.Controls.Add(this.txtSpecOptionCount);
			this.grpSpecOption.Controls.Add(this.cmbDevice);
			this.grpSpecOption.Controls.Add(this.lblDevice);
			this.grpSpecOption.Controls.Add(this.btnUpdate);
			this.grpSpecOption.Controls.Add(this.cdgSpec);
			this.grpSpecOption.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpSpecOption.Location = new System.Drawing.Point(4, 597);
			this.grpSpecOption.Name = "grpSpecOption";
			this.grpSpecOption.Size = new System.Drawing.Size(482, 275);
			this.grpSpecOption.TabIndex = 29;
			this.grpSpecOption.TabStop = false;
			this.grpSpecOption.Text = "Spec Option";
			// 
			// lblSpecOptionCount
			// 
			this.lblSpecOptionCount.Font = new System.Drawing.Font("굴림", 9F);
			this.lblSpecOptionCount.Location = new System.Drawing.Point(379, 22);
			this.lblSpecOptionCount.Name = "lblSpecOptionCount";
			this.lblSpecOptionCount.Size = new System.Drawing.Size(33, 12);
			this.lblSpecOptionCount.TabIndex = 52;
			this.lblSpecOptionCount.Text = "Total";
			// 
			// txtSpecOptionCount
			// 
			this.txtSpecOptionCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSpecOptionCount.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtSpecOptionCount.Location = new System.Drawing.Point(419, 17);
			this.txtSpecOptionCount.Name = "txtSpecOptionCount";
			this.txtSpecOptionCount.ReadOnly = true;
			this.txtSpecOptionCount.Size = new System.Drawing.Size(53, 21);
			this.txtSpecOptionCount.TabIndex = 51;
			this.txtSpecOptionCount.Text = "0";
			this.txtSpecOptionCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// cmbDevice
			// 
			this.cmbDevice.Font = new System.Drawing.Font("굴림", 9F);
			this.cmbDevice.Location = new System.Drawing.Point(57, 16);
			this.cmbDevice.Name = "cmbDevice";
			this.cmbDevice.Size = new System.Drawing.Size(129, 20);
			this.cmbDevice.TabIndex = 34;
			this.cmbDevice.Text = "Select Device";
			this.cmbDevice.SelectedIndexChanged += new System.EventHandler(this.cmbDevice_SelectedIndexChanged);
			// 
			// lblDevice
			// 
			this.lblDevice.Font = new System.Drawing.Font("굴림", 9F);
			this.lblDevice.Location = new System.Drawing.Point(8, 20);
			this.lblDevice.Name = "lblDevice";
			this.lblDevice.Size = new System.Drawing.Size(46, 14);
			this.lblDevice.TabIndex = 33;
			this.lblDevice.Text = "Device";
			// 
			// btnUpdate
			// 
			this.btnUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.BackgroundImage")));
			this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnUpdate.Font = new System.Drawing.Font("굴림", 9F);
			this.btnUpdate.Location = new System.Drawing.Point(303, 17);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(67, 21);
			this.btnUpdate.TabIndex = 5;
			this.btnUpdate.Visible = false;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// cdgSpec
			// 
			this.cdgSpec.BackColor = System.Drawing.SystemColors.Window;
			this.cdgSpec.ColumnInfo = "10,0,0,0,0,75,Columns:";
			this.cdgSpec.Font = new System.Drawing.Font("굴림", 9F);
			this.cdgSpec.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cdgSpec.Location = new System.Drawing.Point(7, 44);
			this.cdgSpec.Name = "cdgSpec";
			this.cdgSpec.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
			this.cdgSpec.Size = new System.Drawing.Size(466, 222);
			this.cdgSpec.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(@"Fixed{BackColor:Control;ForeColor:ControlText;Border:Flat,1,ControlDark,Both;}	Highlight{BackColor:Highlight;ForeColor:HighlightText;}	Search{BackColor:Highlight;ForeColor:HighlightText;}	Frozen{BackColor:Beige;}	EmptyArea{BackColor:AppWorkspace;Border:Flat,1,ControlDarkDark,Both;}	GrandTotal{BackColor:Black;ForeColor:White;}	Subtotal0{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal1{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal2{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal3{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal4{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal5{BackColor:ControlDarkDark;ForeColor:White;}	");
			this.cdgSpec.TabIndex = 0;
			// 
			// btnGetDevice
			// 
			this.btnGetDevice.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetDevice.BackgroundImage")));
			this.btnGetDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnGetDevice.Location = new System.Drawing.Point(293, 77);
			this.btnGetDevice.Name = "btnGetDevice";
			this.btnGetDevice.Size = new System.Drawing.Size(20, 40);
			this.btnGetDevice.TabIndex = 30;
			this.btnGetDevice.Click += new System.EventHandler(this.btnGetDevice_Click);
			// 
			// grpDevice
			// 
			this.grpDevice.Controls.Add(this.lstDevice);
			this.grpDevice.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpDevice.Location = new System.Drawing.Point(320, 4);
			this.grpDevice.Name = "grpDevice";
			this.grpDevice.Size = new System.Drawing.Size(165, 190);
			this.grpDevice.TabIndex = 31;
			this.grpDevice.TabStop = false;
			this.grpDevice.Text = "Device List";
			// 
			// lstDevice
			// 
			this.lstDevice.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lstDevice.ItemHeight = 12;
			this.lstDevice.Location = new System.Drawing.Point(7, 20);
			this.lstDevice.Name = "lstDevice";
			this.lstDevice.ScrollAlwaysVisible = true;
			this.lstDevice.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstDevice.Size = new System.Drawing.Size(150, 160);
			this.lstDevice.TabIndex = 0;
			// 
			// btnGetLayer
			// 
			this.btnGetLayer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetLayer.BackgroundImage")));
			this.btnGetLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnGetLayer.Location = new System.Drawing.Point(492, 77);
			this.btnGetLayer.Name = "btnGetLayer";
			this.btnGetLayer.Size = new System.Drawing.Size(20, 40);
			this.btnGetLayer.TabIndex = 32;
			this.btnGetLayer.Click += new System.EventHandler(this.btnGetLayer_Click);
			// 
			// btnToDefault
			// 
			this.btnToDefault.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnToDefault.BackgroundImage")));
			this.btnToDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnToDefault.Location = new System.Drawing.Point(492, 400);
			this.btnToDefault.Name = "btnToDefault";
			this.btnToDefault.Size = new System.Drawing.Size(20, 40);
			this.btnToDefault.TabIndex = 33;
			this.btnToDefault.Click += new System.EventHandler(this.btnToDefault_Click);
			// 
			// btnSelectLayer
			// 
			this.btnSelectLayer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSelectLayer.BackgroundImage")));
			this.btnSelectLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSelectLayer.Location = new System.Drawing.Point(743, 196);
			this.btnSelectLayer.Name = "btnSelectLayer";
			this.btnSelectLayer.Size = new System.Drawing.Size(40, 20);
			this.btnSelectLayer.TabIndex = 34;
			this.btnSelectLayer.Click += new System.EventHandler(this.btnSelectLayer_Click);
			// 
			// grpOption
			// 
			this.grpOption.Controls.Add(this.grpOption2);
			this.grpOption.Controls.Add(this.grpSchedule);
			this.grpOption.Controls.Add(this.nudMapPeriod);
			this.grpOption.Controls.Add(this.lblDay);
			this.grpOption.Controls.Add(this.chkLimit);
			this.grpOption.Controls.Add(this.grpOption3);
			this.grpOption.Controls.Add(this.grpOption1);
			this.grpOption.Controls.Add(this.btnToExcel);
			this.grpOption.Controls.Add(this.grpOption4);
			this.grpOption.Controls.Add(this.lblMapPeriod);
			this.grpOption.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpOption.Location = new System.Drawing.Point(519, 597);
			this.grpOption.Name = "grpOption";
			this.grpOption.Size = new System.Drawing.Size(475, 275);
			this.grpOption.TabIndex = 35;
			this.grpOption.TabStop = false;
			this.grpOption.Text = "Report Options";
			// 
			// grpOption2
			// 
			this.grpOption2.Controls.Add(this.chkDR5);
			this.grpOption2.Controls.Add(this.chkTot5);
			this.grpOption2.Controls.Add(this.chkDR);
			this.grpOption2.Controls.Add(this.chkTot);
			this.grpOption2.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpOption2.Location = new System.Drawing.Point(113, 15);
			this.grpOption2.Name = "grpOption2";
			this.grpOption2.Size = new System.Drawing.Size(111, 78);
			this.grpOption2.TabIndex = 2;
			this.grpOption2.TabStop = false;
			this.grpOption2.Text = "Category";
			// 
			// chkDR5
			// 
			this.chkDR5.Location = new System.Drawing.Point(56, 50);
			this.chkDR5.Name = "chkDR5";
			this.chkDR5.Size = new System.Drawing.Size(53, 17);
			this.chkDR5.TabIndex = 5;
			this.chkDR5.Text = "DR.5";
			this.chkDR5.CheckedChanged += new System.EventHandler(this.chkDR5_CheckedChanged);
			// 
			// chkTot5
			// 
			this.chkTot5.Location = new System.Drawing.Point(5, 49);
			this.chkTot5.Name = "chkTot5";
			this.chkTot5.Size = new System.Drawing.Size(54, 18);
			this.chkTot5.TabIndex = 4;
			this.chkTot5.Text = "Tot.5";
			this.chkTot5.CheckedChanged += new System.EventHandler(this.chkTot5_CheckedChanged);
			// 
			// chkDR
			// 
			this.chkDR.Checked = true;
			this.chkDR.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDR.Location = new System.Drawing.Point(56, 22);
			this.chkDR.Name = "chkDR";
			this.chkDR.Size = new System.Drawing.Size(41, 17);
			this.chkDR.TabIndex = 3;
			this.chkDR.Text = "DR";
			this.chkDR.CheckedChanged += new System.EventHandler(this.chkDR_CheckedChanged);
			// 
			// chkTot
			// 
			this.chkTot.Checked = true;
			this.chkTot.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTot.Location = new System.Drawing.Point(5, 21);
			this.chkTot.Name = "chkTot";
			this.chkTot.Size = new System.Drawing.Size(43, 18);
			this.chkTot.TabIndex = 2;
			this.chkTot.Text = "Tot";
			this.chkTot.CheckedChanged += new System.EventHandler(this.chkTot_CheckedChanged);
			// 
			// grpSchedule
			// 
			this.grpSchedule.Controls.Add(this.cmbScheduleGroup);
			this.grpSchedule.Controls.Add(this.lblScheduleGroup);
			this.grpSchedule.Controls.Add(this.dtpScheduleTime);
			this.grpSchedule.Controls.Add(this.label3);
			this.grpSchedule.Controls.Add(this.dtpScheduleDate);
			this.grpSchedule.Controls.Add(this.lblScheduleTime);
			this.grpSchedule.Controls.Add(this.btnDeleteSchedule);
			this.grpSchedule.Controls.Add(this.btnRegistSchedule);
			this.grpSchedule.Controls.Add(this.cdgSchedule);
			this.grpSchedule.Font = new System.Drawing.Font("굴림", 9F);
			this.grpSchedule.Location = new System.Drawing.Point(13, 134);
			this.grpSchedule.Name = "grpSchedule";
			this.grpSchedule.Size = new System.Drawing.Size(450, 133);
			this.grpSchedule.TabIndex = 10;
			this.grpSchedule.TabStop = false;
			this.grpSchedule.Text = "Scheduling";
			// 
			// cmbScheduleGroup
			// 
			this.cmbScheduleGroup.Font = new System.Drawing.Font("굴림", 9F);
			this.cmbScheduleGroup.Location = new System.Drawing.Point(337, 19);
			this.cmbScheduleGroup.Name = "cmbScheduleGroup";
			this.cmbScheduleGroup.Size = new System.Drawing.Size(107, 20);
			this.cmbScheduleGroup.TabIndex = 77;
			this.cmbScheduleGroup.Text = "Select Group";
			// 
			// lblScheduleGroup
			// 
			this.lblScheduleGroup.Font = new System.Drawing.Font("굴림", 9F);
			this.lblScheduleGroup.Location = new System.Drawing.Point(296, 24);
			this.lblScheduleGroup.Name = "lblScheduleGroup";
			this.lblScheduleGroup.Size = new System.Drawing.Size(46, 14);
			this.lblScheduleGroup.TabIndex = 76;
			this.lblScheduleGroup.Text = "Group";
			// 
			// dtpScheduleTime
			// 
			this.dtpScheduleTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dtpScheduleTime.Location = new System.Drawing.Point(337, 75);
			this.dtpScheduleTime.Name = "dtpScheduleTime";
			this.dtpScheduleTime.ShowUpDown = true;
			this.dtpScheduleTime.Size = new System.Drawing.Size(107, 21);
			this.dtpScheduleTime.TabIndex = 75;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(296, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(39, 12);
			this.label3.TabIndex = 74;
			this.label3.Text = "Date";
			// 
			// dtpScheduleDate
			// 
			this.dtpScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpScheduleDate.Location = new System.Drawing.Point(337, 47);
			this.dtpScheduleDate.Name = "dtpScheduleDate";
			this.dtpScheduleDate.Size = new System.Drawing.Size(107, 21);
			this.dtpScheduleDate.TabIndex = 73;
			// 
			// lblScheduleTime
			// 
			this.lblScheduleTime.Location = new System.Drawing.Point(295, 80);
			this.lblScheduleTime.Name = "lblScheduleTime";
			this.lblScheduleTime.Size = new System.Drawing.Size(39, 12);
			this.lblScheduleTime.TabIndex = 72;
			this.lblScheduleTime.Text = "Time";
			// 
			// btnDeleteSchedule
			// 
			this.btnDeleteSchedule.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteSchedule.BackgroundImage")));
			this.btnDeleteSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDeleteSchedule.Font = new System.Drawing.Font("굴림", 9F);
			this.btnDeleteSchedule.Location = new System.Drawing.Point(371, 106);
			this.btnDeleteSchedule.Name = "btnDeleteSchedule";
			this.btnDeleteSchedule.Size = new System.Drawing.Size(73, 21);
			this.btnDeleteSchedule.TabIndex = 71;
			this.btnDeleteSchedule.Click += new System.EventHandler(this.btnDeleteSchedule_Click);
			// 
			// btnRegistSchedule
			// 
			this.btnRegistSchedule.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRegistSchedule.BackgroundImage")));
			this.btnRegistSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRegistSchedule.Font = new System.Drawing.Font("굴림", 9F);
			this.btnRegistSchedule.Location = new System.Drawing.Point(292, 106);
			this.btnRegistSchedule.Name = "btnRegistSchedule";
			this.btnRegistSchedule.Size = new System.Drawing.Size(74, 21);
			this.btnRegistSchedule.TabIndex = 6;
			this.btnRegistSchedule.Click += new System.EventHandler(this.btnRegistSchedule_Click);
			// 
			// cdgSchedule
			// 
			this.cdgSchedule.BackColor = System.Drawing.SystemColors.Window;
			this.cdgSchedule.ColumnInfo = "10,0,0,0,0,75,Columns:";
			this.cdgSchedule.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cdgSchedule.Location = new System.Drawing.Point(9, 17);
			this.cdgSchedule.Name = "cdgSchedule";
			this.cdgSchedule.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
			this.cdgSchedule.Size = new System.Drawing.Size(275, 109);
			this.cdgSchedule.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(@"Normal{Font:굴림, 9pt;}	Fixed{BackColor:Control;ForeColor:ControlText;Border:Flat,1,ControlDark,Both;}	Highlight{BackColor:Highlight;ForeColor:HighlightText;}	Search{BackColor:Highlight;ForeColor:HighlightText;}	Frozen{BackColor:Beige;}	EmptyArea{BackColor:AppWorkspace;Border:Flat,1,ControlDarkDark,Both;}	GrandTotal{BackColor:Black;ForeColor:White;}	Subtotal0{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal1{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal2{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal3{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal4{BackColor:ControlDarkDark;ForeColor:White;}	Subtotal5{BackColor:ControlDarkDark;ForeColor:White;}	");
			this.cdgSchedule.TabIndex = 0;
			// 
			// nudMapPeriod
			// 
			this.nudMapPeriod.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.nudMapPeriod.Location = new System.Drawing.Point(180, 108);
			this.nudMapPeriod.Maximum = new System.Decimal(new int[] {
																		 3,
																		 0,
																		 0,
																		 0});
			this.nudMapPeriod.Minimum = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.nudMapPeriod.Name = "nudMapPeriod";
			this.nudMapPeriod.Size = new System.Drawing.Size(35, 21);
			this.nudMapPeriod.TabIndex = 9;
			this.nudMapPeriod.Value = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   0});
			// 
			// lblDay
			// 
			this.lblDay.Font = new System.Drawing.Font("굴림", 9F);
			this.lblDay.Location = new System.Drawing.Point(216, 113);
			this.lblDay.Name = "lblDay";
			this.lblDay.Size = new System.Drawing.Size(35, 16);
			this.lblDay.TabIndex = 7;
			this.lblDay.Text = "Days";
			// 
			// chkLimit
			// 
			this.chkLimit.Font = new System.Drawing.Font("굴림", 9F);
			this.chkLimit.Location = new System.Drawing.Point(14, 111);
			this.chkLimit.Name = "chkLimit";
			this.chkLimit.Size = new System.Drawing.Size(109, 17);
			this.chkLimit.TabIndex = 4;
			this.chkLimit.Text = "Use Ctrl Limit";
			// 
			// grpOption3
			// 
			this.grpOption3.Controls.Add(this.rdoSelected);
			this.grpOption3.Controls.Add(this.rdoDefault);
			this.grpOption3.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpOption3.Location = new System.Drawing.Point(230, 15);
			this.grpOption3.Name = "grpOption3";
			this.grpOption3.Size = new System.Drawing.Size(117, 78);
			this.grpOption3.TabIndex = 3;
			this.grpOption3.TabStop = false;
			this.grpOption3.Text = "Reporting";
			// 
			// rdoSelected
			// 
			this.rdoSelected.Location = new System.Drawing.Point(6, 45);
			this.rdoSelected.Name = "rdoSelected";
			this.rdoSelected.Size = new System.Drawing.Size(108, 24);
			this.rdoSelected.TabIndex = 1;
			this.rdoSelected.Text = "Selected Layer";
			// 
			// rdoDefault
			// 
			this.rdoDefault.Checked = true;
			this.rdoDefault.Location = new System.Drawing.Point(6, 17);
			this.rdoDefault.Name = "rdoDefault";
			this.rdoDefault.TabIndex = 0;
			this.rdoDefault.TabStop = true;
			this.rdoDefault.Text = "Default Layer";
			// 
			// grpOption1
			// 
			this.grpOption1.Controls.Add(this.rdoSpec3);
			this.grpOption1.Controls.Add(this.rdoSpec2);
			this.grpOption1.Controls.Add(this.rdoSpec1);
			this.grpOption1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.grpOption1.Location = new System.Drawing.Point(13, 15);
			this.grpOption1.Name = "grpOption1";
			this.grpOption1.Size = new System.Drawing.Size(94, 78);
			this.grpOption1.TabIndex = 1;
			this.grpOption1.TabStop = false;
			this.grpOption1.Text = "Spec Option";
			// 
			// rdoSpec3
			// 
			this.rdoSpec3.Location = new System.Drawing.Point(9, 51);
			this.rdoSpec3.Name = "rdoSpec3";
			this.rdoSpec3.Size = new System.Drawing.Size(74, 20);
			this.rdoSpec3.TabIndex = 2;
			this.rdoSpec3.Text = "No Limit";
			// 
			// rdoSpec2
			// 
			this.rdoSpec2.Location = new System.Drawing.Point(9, 33);
			this.rdoSpec2.Name = "rdoSpec2";
			this.rdoSpec2.Size = new System.Drawing.Size(74, 20);
			this.rdoSpec2.TabIndex = 1;
			this.rdoSpec2.Text = "Limit In";
			// 
			// rdoSpec1
			// 
			this.rdoSpec1.Checked = true;
			this.rdoSpec1.Location = new System.Drawing.Point(9, 15);
			this.rdoSpec1.Name = "rdoSpec1";
			this.rdoSpec1.Size = new System.Drawing.Size(77, 20);
			this.rdoSpec1.TabIndex = 0;
			this.rdoSpec1.TabStop = true;
			this.rdoSpec1.Text = "Limit Out";
			// 
			// btnToExcel
			// 
			this.btnToExcel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnToExcel.BackgroundImage")));
			this.btnToExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnToExcel.Location = new System.Drawing.Point(281, 107);
			this.btnToExcel.Name = "btnToExcel";
			this.btnToExcel.Size = new System.Drawing.Size(67, 21);
			this.btnToExcel.TabIndex = 0;
			this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
			// 
			// grpOption4
			// 
			this.grpOption4.Controls.Add(this.nudLogUnit);
			this.grpOption4.Controls.Add(this.label2);
			this.grpOption4.Controls.Add(this.nudLogMax);
			this.grpOption4.Controls.Add(this.label1);
			this.grpOption4.Controls.Add(this.rdoLinear);
			this.grpOption4.Controls.Add(this.rdoLog);
			this.grpOption4.Font = new System.Drawing.Font("굴림", 9F);
			this.grpOption4.Location = new System.Drawing.Point(355, 15);
			this.grpOption4.Name = "grpOption4";
			this.grpOption4.Size = new System.Drawing.Size(107, 113);
			this.grpOption4.TabIndex = 5;
			this.grpOption4.TabStop = false;
			this.grpOption4.Text = "Y Axis Style";
			// 
			// nudLogUnit
			// 
			this.nudLogUnit.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.nudLogUnit.Increment = new System.Decimal(new int[] {
																		 5,
																		 0,
																		 0,
																		 0});
			this.nudLogUnit.Location = new System.Drawing.Point(39, 83);
			this.nudLogUnit.Maximum = new System.Decimal(new int[] {
																	   995,
																	   0,
																	   0,
																	   0});
			this.nudLogUnit.Name = "nudLogUnit";
			this.nudLogUnit.ReadOnly = true;
			this.nudLogUnit.Size = new System.Drawing.Size(60, 21);
			this.nudLogUnit.TabIndex = 71;
			this.nudLogUnit.ThousandsSeparator = true;
			this.nudLogUnit.Value = new System.Decimal(new int[] {
																	 20,
																	 0,
																	 0,
																	 0});
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("굴림", 9F);
			this.label2.Location = new System.Drawing.Point(9, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 16);
			this.label2.TabIndex = 70;
			this.label2.Text = "Unit";
			// 
			// nudLogMax
			// 
			this.nudLogMax.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.nudLogMax.Increment = new System.Decimal(new int[] {
																		100,
																		0,
																		0,
																		0});
			this.nudLogMax.Location = new System.Drawing.Point(39, 59);
			this.nudLogMax.Maximum = new System.Decimal(new int[] {
																	  10000,
																	  0,
																	  0,
																	  0});
			this.nudLogMax.Name = "nudLogMax";
			this.nudLogMax.ReadOnly = true;
			this.nudLogMax.Size = new System.Drawing.Size(60, 21);
			this.nudLogMax.TabIndex = 69;
			this.nudLogMax.Tag = "";
			this.nudLogMax.ThousandsSeparator = true;
			this.nudLogMax.Value = new System.Decimal(new int[] {
																	1000,
																	0,
																	0,
																	0});
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("굴림", 9F);
			this.label1.Location = new System.Drawing.Point(5, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 16);
			this.label1.TabIndex = 68;
			this.label1.Text = "Max";
			// 
			// rdoLinear
			// 
			this.rdoLinear.Checked = true;
			this.rdoLinear.Location = new System.Drawing.Point(7, 17);
			this.rdoLinear.Name = "rdoLinear";
			this.rdoLinear.Size = new System.Drawing.Size(82, 16);
			this.rdoLinear.TabIndex = 1;
			this.rdoLinear.TabStop = true;
			this.rdoLinear.Text = "Linear";
			this.rdoLinear.CheckedChanged += new System.EventHandler(this.rdoLinear_CheckedChanged);
			// 
			// rdoLog
			// 
			this.rdoLog.Location = new System.Drawing.Point(7, 34);
			this.rdoLog.Name = "rdoLog";
			this.rdoLog.Size = new System.Drawing.Size(86, 24);
			this.rdoLog.TabIndex = 0;
			this.rdoLog.Text = "Log";
			this.rdoLog.CheckedChanged += new System.EventHandler(this.rdoLog_CheckedChanged);
			// 
			// lblMapPeriod
			// 
			this.lblMapPeriod.Font = new System.Drawing.Font("굴림", 9F);
			this.lblMapPeriod.Location = new System.Drawing.Point(136, 113);
			this.lblMapPeriod.Name = "lblMapPeriod";
			this.lblMapPeriod.Size = new System.Drawing.Size(43, 16);
			this.lblMapPeriod.TabIndex = 8;
			this.lblMapPeriod.Text = "Period";
			// 
			// chtChart
			// 
			this.chtChart.AxisY.Gridlines = true;
			this.chtChart.AxisY.LabelsFormat.Decimals = 0;
			this.chtChart.DataEditorObj.ShowHeader = false;
			this.chtChart.DesignTimeData = "C:\\Program Files\\ChartFX for .NET 6.2\\Wizard\\Scatter.txt";
			this.chtChart.Gallery = SoftwareFX.ChartFX.Gallery.Scatter;
			this.chtChart.Location = new System.Drawing.Point(5, 224);
			this.chtChart.MarkerSize = ((short)(4));
			this.chtChart.Name = "chtChart";
			this.chtChart.NValues = 27;
			this.chtChart.Palette = "DarkPastels.AltPastels";
			seriesAttributes1.MarkerShape = SoftwareFX.ChartFX.MarkerShape.Cross;
			this.chtChart.Series.AddRange(new SoftwareFX.ChartFX.SeriesAttributes[] {
																						seriesAttributes1,
																						seriesAttributes2});
			this.chtChart.SerLegBox = true;
			this.chtChart.Size = new System.Drawing.Size(480, 250);
			this.chtChart.TabIndex = 36;
			this.chtChart.Titles.AddRange(new SoftwareFX.ChartFX.TitleDockable[] {
																					 titleDockable1});
			// 
			// TrendReport
			// 
			this.Controls.Add(this.grpOption);
			this.Controls.Add(this.btnSelectLayer);
			this.Controls.Add(this.btnToDefault);
			this.Controls.Add(this.btnGetLayer);
			this.Controls.Add(this.grpDevice);
			this.Controls.Add(this.btnGetDevice);
			this.Controls.Add(this.grpSpecOption);
			this.Controls.Add(this.grpDefault);
			this.Controls.Add(this.grpSelected);
			this.Controls.Add(this.grpLayer);
			this.Controls.Add(this.grpPeriod);
			this.Controls.Add(this.chtChart);
			this.Name = "TrendReport";
			this.Size = new System.Drawing.Size(1000, 877);
			this.Load += new System.EventHandler(this.TrendReport_Load);
			this.grpPeriod.ResumeLayout(false);
			this.grpLayer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cdgLayer)).EndInit();
			this.grpSelected.ResumeLayout(false);
			this.grpEditS.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudCtrlS)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEngS)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudUnitS)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinS)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxS)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cdgSelected)).EndInit();
			this.grpDefault.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.grpEdit.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudCtrl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEng)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudUnit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cdgDefault)).EndInit();
			this.grpSpecOption.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cdgSpec)).EndInit();
			this.grpDevice.ResumeLayout(false);
			this.grpOption.ResumeLayout(false);
			this.grpOption2.ResumeLayout(false);
			this.grpSchedule.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cdgSchedule)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMapPeriod)).EndInit();
			this.grpOption3.ResumeLayout(false);
			this.grpOption1.ResumeLayout(false);
			this.grpOption4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudLogUnit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudLogMax)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region ■ TrendReport_Load 이벤트

		/// <summary>
		/// 1) Shift 선택콤보에 ALL Item 추가
		/// 2) GetDeviceList 서비스 호출
		/// </summary>
		/// 
		private void TrendReport_Load(object sender, System.EventArgs e)
		{
			if(DesignMode) return;

			cmbShift.Text="ALL";
			GetDeviceList();
		}
		#endregion

		#region ■cmbShift_SelectedIndexChanged 변경 이벤트

		/// <summary>
		/// Shift 설정에 따라서 From ~ To Time 값 설정
		/// </summary>
		/// 
		private void cmbShift_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (cmbShift.Text)
			{
				case "ALL":
				{
					dtpFromTime.Text="06:00";
					dtpToTime.Text="06:00";
					break;
				}
				case "A":
				{
					dtpFromTime.Text="06:00";
					dtpToTime.Text="14:00";
					break;
				}
				case "B":
				{
					dtpFromTime.Text="14:00";
					dtpToTime.Text="22:00";
					break;
				}
				case "C":
				{
					dtpFromTime.Text="22:00";
					dtpToTime.Text="06:00";
					break;
				}
			}
		}
		#endregion

		#region ■ dtpFromTime_ValueChanged 변경 이벤트

		/// <summary>
		/// From Date Time 값 변경시 ShiftSet 서비스 호출
		/// </summary>
		/// 
		private void dtpFromTime_ValueChanged(object sender, System.EventArgs e)
		{
			ShiftSet();
		}
		#endregion

		#region ■ dtpToTime_ValueChanged 변경 이벤트

		/// <summary>
		/// To Date Time 값 변경시 ShiftSet 서비스 호출
		/// </summary>
		/// 
		private void dtpToTime_ValueChanged(object sender, System.EventArgs e)
		{
			ShiftSet();
		}
		#endregion

		#region ■ ShiftSet

		/// <summary>
		/// 시간설정에 따른 Shift 설정.
		/// </summary>
		/// 
		private void ShiftSet()
		{
			if(dtpFromTime.Text=="06:00" && dtpToTime.Text=="06:00")
			{
				cmbShift.SelectedIndex=0;
			}
			else if(dtpFromTime.Text=="06:00" && dtpToTime.Text=="14:00")
			{
				cmbShift.SelectedIndex=1;
			}
			else if(dtpFromTime.Text=="14:00" && dtpToTime.Text=="22:00")
			{
				cmbShift.SelectedIndex=2;
			}
			else if(dtpFromTime.Text=="22:00" && dtpToTime.Text=="06:00")
			{
				cmbShift.SelectedIndex=3;
			}
			else 
			{
				cmbShift.SelectedIndex=4;
			}
		}
		#endregion

		#region ■ GetPeriod

		/// <summary>
		/// Report 기간변수 설정
		/// </summary>
		/// 
		private void GetPeriod()
		{

			strFromDate = dtpFromDate.Value.Year.ToString().PadLeft(4,'0') + 
				dtpFromDate.Value.Month.ToString().PadLeft(2,'0') + 
				dtpFromDate.Value.Day.ToString().PadLeft(2,'0'); 

			strFromTime = dtpFromTime.Value.Hour.ToString().PadLeft(2,'0') +  
				dtpFromTime.Value.Minute.ToString().PadLeft(2,'0') + 
				dtpFromTime.Value.Second.ToString().PadLeft(2,'0'); 

			strToTime = dtpToTime.Value.Hour.ToString().PadLeft(2,'0') + 
				dtpToTime.Value.Minute.ToString().PadLeft(2,'0') + 
				dtpToTime.Value.Second.ToString().PadLeft(2,'0');

			// 선택한 Shift가 All 이나 C(Night) 일 경우는 End-Date를 +1 하여 다음날로 변경한다.
			if(cmbShift.SelectedIndex==0 || cmbShift.SelectedIndex==3)
			{
				strToDate = dtpToDate.Value.AddDays(1).Year.ToString().PadLeft(4,'0') +  
					dtpToDate.Value.AddDays(1).Month.ToString().PadLeft(2,'0') + 
					dtpToDate.Value.AddDays(1).Day.ToString().PadLeft(2,'0');
			}
			else // Custom Time 과 그 외의 Shift 경우는 선택한 날짜를 그대로, 해당 시간대를 그대로 적용
			{
				strToDate = dtpToDate.Value.Year.ToString().PadLeft(4,'0') + 
					dtpToDate.Value.Month.ToString().PadLeft(2,'0') + 
					dtpToDate.Value.Day.ToString().PadLeft(2,'0');
			}

		}
		#endregion

		#region ■ PeriodCheck

		/// <summary>
		/// 설정한 기간이 올바른지 체크한다.
		/// </summary>
		/// 
		private void PeriodCheck()
		{
			GetPeriod();

			bool bPeriodFlag=true;

			if(Int32.Parse(strFromDate.Substring(0,8)) > Int32.Parse(strToDate.Substring(0,8)))
			{
				bPeriodFlag=false;
			}

			if(Int32.Parse(strFromDate) > Int32.Parse(strToDate))
			{
				if(Int32.Parse(strFromTime) > Int32.Parse(strToDate))
				{
					bPeriodFlag=false;
				}
			}
			if(!bPeriodFlag)
			{
				MessageBox.Show("기간설정이 올바르지 않습니다.");
				dtpFromDate.Value=dtpToDate.Value;
			}

		}
		#endregion

		#region ■ btnGetDevice_Click 클릭 이벤트

		/// <summary>
		/// 1) Product 목록 상자를 Clear 한다.
		/// 2) 목록상자에 설정한 기간의 Device 목록을 가져오는 서비스를 호출한다.
		/// </summary>
		/// 
		private void btnGetDevice_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			lstDevice.Items.Clear();
			GetDevice();
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ GetDevice

		/// <summary>
		/// Product 목록상자에 Device List 를 가져와 바인딩한다.
		/// </summary>
		/// <param name="strUserID">사용자 ID</param>
		/// <param name="strQueryID">쿼리 ID</param>
		/// <param name="strParam">파라메터</param>
		/// 
		private void GetDevice()
		{
			GetPeriod();

			DataSet dsReturn=null;
			Remoting oDeviceList=null;

			try
			{
				oDeviceList=new Remoting();

				dsReturn=oDeviceList.GetDevice(strUserID, "GetDeviceList", strFromDate, strToDate, strFromTime, strToTime, cmbShift.SelectedItem.ToString());

				for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
				{
					lstDevice.Items.Add(dsReturn.Tables[0].Rows[i][0]);
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

		#region ■ GetGroupList

		/// <summary>
		/// 사용자 Default Group 목록을 가져와 해당 콤보에 바인딩한다.
		/// </summary>
		/// 
		private void GetGroupList()
		{
			DataSet dsReturn=null;
			Remoting oGroupList=null;

			try
			{
				oGroupList=new Remoting();

				dsReturn=oGroupList.GetGroupList(strUserID, "GetGroupList");

				cmbGroup.Items.Clear();
				cmbScheduleGroup.Items.Clear();
				cmbGroup.Items.Add("Select Group");
				cmbScheduleGroup.Items.Add("Select Group");

				for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
				{
					cmbGroup.Items.Add(dsReturn.Tables[0].Rows[i][0]);
					cmbScheduleGroup.Items.Add(dsReturn.Tables[0].Rows[i][0]);
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

		#region ■ btnGetLayer_Click

		/// <summary>
		/// 1) 선택한 Product 이 1개 이상 존재하는지 체크
		/// 2) 해당하는 Step List 를 가져오는 서비스 를 호출한다.
		/// </summary>
		/// 
		private void btnGetLayer_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			if(lstDevice.SelectedItems.Count==0)
			{
				MessageBox.Show("Device를 선택하여야만 추가할 수 있습니다.");
			}
			else
			{
				GetLayer();
				txtLayerCount.Text=(cdgLayer.Rows.Count-1).ToString();
			}
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ GetLayer 

		/// <summary>
		/// 1) 선택한 Product 에 해당하는 Layer 목록을 가져온다.
		/// </summary>
		/// 
		private void GetLayer()
		{
			DataSet dsReturn=null;
			Remoting oLayer=null;

			string[] strDeviceList;

			try
			{
				strDeviceList=new string[lstDevice.SelectedItems.Count];

				for(int i=0;i<lstDevice.SelectedItems.Count;i++)
				{
					strDeviceList[i]=lstDevice.SelectedItems[i].ToString();
				}

				oLayer=new Remoting();
				dsReturn=oLayer.GetLayer(strUserID, "GetLayer", strFromDate, strToDate, strFromTime, strToTime, cmbShift.SelectedItem.ToString(), strDeviceList);
				cdgLayer.DataSource=dsReturn.Tables[0];
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

		#region ■ GetDeviceList

		/// <summary>
		/// Device 목록을 가져와 Device 선택 콤보상자에 바인딩한다.
		/// </summary>
		/// 
		private void GetDeviceList()
		{

			DataSet dsReturn=null;
			Remoting oDeviceList=null;

			try
			{
				oDeviceList=new Remoting();

				dsReturn=oDeviceList.GetDevice(strUserID, "GetProduct");

				for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
				{
					cmbDevice.Items.Add(dsReturn.Tables[0].Rows[i][0]);
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

		#region ■ btnClearLayer_Click 클릭 이벤트

		/// <summary>
		/// 1) Layer Grid 초기화 서비스 호출
		/// </summary>
		/// 
		private void btnClearLayer_Click(object sender, System.EventArgs e)
		{
			ResetLayerGrid();
			txtLayerCount.Text="0";
		}
		#endregion

		#region ■ btnSelectLayer_Click

		/// <summary>
		/// 선택한 Step 을 Selected Layer Grid 로 복사하는 서비스 호출
		/// </summary>
		/// 
		private void btnSelectLayer_Click(object sender, System.EventArgs e)
		{
			// User 가 선택한 Step 을 Collection.
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			AddToSelected();

			this.Cursor = System.Windows.Forms.Cursors.Default;

			txtSelectedCount.Text=(cdgSelected.Rows.Count-1).ToString();
		}
		#endregion

		#region ■ cmbDevice_SelectedIndexChanged 변경이벤트

		/// <summary>
		/// 선택한 Device 에 해당하는 Spect List 를 가져오는 서비스 호출.
		/// </summary>
		/// 
		private void cmbDevice_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			GetSpecList(cmbDevice.SelectedItem.ToString());
			txtSpecOptionCount.Text=(cdgSpec.Rows.Count-1).ToString();

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ GetSpecList

		/// <summary>
		/// 기본 Spec 목록을 가져와 Spec Grid 에 바인딩한다.
		/// </summary>
		/// <param name="strUserID">사용자 ID</param>
		/// <param name="strQueryID">쿼리 ID</param>
		/// <param name="strParam">파라메터</param>
		/// 
		private void GetSpecList(string strDevice)
		{
			DataSet dsReturn=null;
			Remoting oSpecList=null;

			try
			{
				oSpecList=new Remoting();
				dsReturn=oSpecList.GetSpecList(strUserID, "GetSpecList", strDevice);
				cdgSpec.DataSource=dsReturn.Tables[0];
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

		#region ■ btnUpdate_Click (현재 사용되지 않음 - 관리자 메뉴에 포함예정)

		/// <summary>
		/// 기본 Spec 목록인 T_DMS_SPEC 테이블의 내용중 선택한 항목을
		/// 모든 User Setting 값에 일괄적용(T_DMS_USERSPEC)하는 서비스 호출
		/// </summary>
		/// 
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			UpdateUserSpec();
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ UpdateUserSpec (현재 사용되지 않음 - 관리자 메뉴에 포함예정)

		/// <summary>
		/// 기본 Spec 목록인 T_DMS_SPEC 테이블의 내용중 선택한 항목을 모든 User Setting 값에 일괄적용(T_DMS_USERSPEC)한다.
		/// </summary>
		/// 
		private void UpdateUserSpec()
		{
			Remoting oSpec = null;
			StringBuilder builder=new StringBuilder();
			string[,] strStepID;

			try
			{
				strStepID=new string[cdgSpec.Rows.Selected.Count,3];

				for(int i=0;i<cdgSpec.Rows.Selected.Count;i++)
				{
					strStepID[i,0]=cdgSpec.GetData(cdgSpec.Rows.Selected[i].Index, "STEP_ID").ToString();
					strStepID[i,1]=cdgSpec.GetData(cdgSpec.Rows.Selected[i].Index, "ENG_LIMIT").ToString();
					strStepID[i,2]=cdgSpec.GetData(cdgSpec.Rows.Selected[i].Index, "CTRL_LIMIT").ToString();
				}

				//비지니스 컴포넌트 호출
				oSpec = new Remoting(); 

				oSpec.UpdateUserSpec(strUserID, "UpdateUserSpec", cmbDevice.SelectedItem.ToString(), strStepID);
				MessageBox.Show("성공적으로 적용하였습니다.");

			}
			catch(Exception ex)
			{

				throw ex;
				//Miracom.DMS.Config.
			}

			finally
			{
			}

		}
		#endregion

		#region ■ AddToSelected

		/// <summary>
		/// 1) 선택한 Layer List 를 Selected Grid 로 옮긴다.
		/// 2) 중복된 행이 있는지 체크하여 복사하지 않는다.
		/// </summary>
		/// 
		private void AddToSelected()
		{

			intSelectedRowCount=cdgSpec.Rows.Count;
			intLayerSelectedRowCount=cdgLayer.Rows.Selected.Count;

			string strDevice="";
			string strStep="";

			for(int i=1;i<=intLayerSelectedRowCount;i++)
			{
				strDevice=cdgLayer.Rows.Selected[i-1][0].ToString();
				strStep=cdgLayer.Rows.Selected[i-1][1].ToString();

				if(!SelectionCheck(strDevice, strStep))
				{
					cdgSelected.Rows.Add();
					cdgSelected.SetData(cdgSelected.Rows.Count-1,0,strDevice);
					cdgSelected.SetData(cdgSelected.Rows.Count-1,1,"Unknown");
					cdgSelected.SetData(cdgSelected.Rows.Count-1,2,strStep);
					cdgSelected.SetData(cdgSelected.Rows.Count-1,3,20);
					cdgSelected.SetData(cdgSelected.Rows.Count-1,4,20);
					cdgSelected.SetData(cdgSelected.Rows.Count-1,5,100);
					cdgSelected.SetData(cdgSelected.Rows.Count-1,6,0);
					cdgSelected.SetData(cdgSelected.Rows.Count-1,7,20);
				}
			}

		}
		#endregion

		#region ■ SelectionCheck

		/// <summary>
		/// Selected Grid 에 중복된 행이 있는지 체크하여 결과를 반환한다.
		/// </summary>
		/// <param name="strDevice">Product</param>
		/// <param name="strStep">Step ID</param>
		/// 
		private bool SelectionCheck(string strDevice, string strStep)
		{
			bool bSelected=false;

			for(int i=1;i<cdgSelected.Rows.Count;i++)
			{
				if(cdgSelected.GetData(i,0).ToString()==strDevice && cdgSelected.GetData(i,2).ToString()==strStep)
				{
					bSelected=true;
					break;
				}
			}
			return bSelected;
		}
		#endregion

		#region ■ SelectionCheck2

		/// <summary>
		/// Default Grid 에 중복된 행이 있는지 체크하여 결과를 반환한다.
		/// </summary>
		/// <param name="strDevice">Product</param>
		/// <param name="strStep">Step ID</param>
		/// 
		private bool SelectionCheck2(string strDevice, string strStep)
		{
			bool bSelected=false;

			for(int i=1;i<cdgDefault.Rows.Count;i++)
			{
				if(cdgDefault.GetData(i,0).ToString()==strDevice && cdgDefault.GetData(i,2).ToString()==strStep)
				{
					bSelected=true;
					break;
				}
			}
			return bSelected;
		}
		#endregion

		#region ■ cdgLayer_DragLeave 이벤트 (현재 사용하지 않음)

		/// <summary>
		/// 드래그 관련 이벤트 
		/// </summary>
		/// 
		private void cdgLayer_DragLeave(object sender, System.EventArgs e)
		{

		}
		#endregion

		#region ■ cdgLayer_ItemDrag 이벤트 (현재 사용하지 않음)

		/// <summary>
		/// 드래그 관련 이벤트
		/// </summary>
		/// 
		private void cdgLayer_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
		}
		#endregion

		#region ■ cdgSelected_DragDrop 이벤트 (현재 사용하지 않음)

		/// <summary>
		/// 드래그 관련 이벤트
		/// </summary>
		/// 
		private void cdgSelected_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//			AdapterListItem temp = (AdapterListItem)e.Data.GetData("transDoc.createRuleUI+AdapterListItem");
			//			this.listView2.Items.Insert(0, new AdapterListItem( temp.m_adapter, temp.m_imageIndex));
			//
			//			// 원래 커서로 변환
			//			this.Cursor = Cursors.Default;
		}
		#endregion

		#region ■ btnClearSelected_Click 클릭 이벤트

		/// <summary>
		/// Selected Grid 초기화 (호출)
		/// </summary>
		/// 
		private void btnClearSelected_Click(object sender, System.EventArgs e)
		{
			ResetSelectedLayerGrid();
			txtSelectedCount.Text="0";
			txtSelectedChangedCount.Text="0";
		}
		#endregion

		#region ■ cdgSelected_DragEnter 이벤트 (현재 사용하지 않음)

		/// <summary>
		/// 드래그 관련 이벤트
		/// </summary>
		/// 
		private void cdgSelected_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//			if( e.Data.GetDataPresent("transDoc.createRuleUI+AdapterListItem") )
			//			{
			//				// 원하는 객체가 들어왔으므로 마우스를 Hand로 변경
			//				this.Cursor = Cursors.Hand;
			//				e.Effect = DragDropEffects.Copy;
			//			}

		}
		#endregion

		#region ■ cdgLayer_GiveFeedback 이벤트 (현재 사용하지 않음)

		/// <summary>
		/// 드래그 관련 이벤트 
		/// </summary>
		/// 
		private void cdgLayer_GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
			//			e.UseDefaultCursors = false;
			//			e.Effect = DragDropEffects.Copy;
		}
		#endregion

		#region ■ btnSelectLayerAll_Click 클릭 이벤트

		/// <summary>
		/// Selected Layer의 Select All 버튼을 클릭하면 전체 행을 선택한다.
		/// </summary>
		/// 
		private void btnSelectAll_Click(object sender, System.EventArgs e)
		{
			if(cdgSelected.Rows.Count>1)
			{
				cdgSelected.Select(1,0,cdgSelected.Rows.Count-1,cdgSelected.Cols.Count-1,true);
			}
		}
		#endregion

		#region ■ btnSelectLayerAll_Click 클릭 이벤트

		/// <summary>
		/// cdgLayer의 Select All 버튼을 클릭하면 전체 행을 선택한다.
		/// </summary>
		/// 
		private void btnSelectLayerAll_Click(object sender, System.EventArgs e)
		{
			if(cdgLayer.Rows.Count>1)
			{
				cdgLayer.Select(1,0,cdgLayer.Rows.Count-1,cdgLayer.Cols.Count-1,true);
			}
		}
		#endregion

		#region ■ btnToDefault_Click

		/// <summary>
		/// 1) Selected Grid 의 내용을 Default Grid 로 복사하는 서비스 호출
		/// 2) Count 재 설정.
		/// </summary>
		/// 
		private void btnToDefault_Click(object sender, System.EventArgs e)
		{
			// User 가 선택한 Step 을 Collection.
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			AddToDefault();
			txtDefaultCount.Text=(cdgDefault.Rows.Count-1).ToString();

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ AddToDefault

		/// <summary>
		/// 1) Selected Grid 의 내용이 Default Grid 에 존재하는지 Check 
		/// 2) Default Grid 로 복사한다.
		/// </summary>
		/// 
		private void AddToDefault()
		{
			// Default Group으로 Add한다. 
			intDefaultRowCount=cdgDefault.Rows.Count;
			intSelectedSelectedRowCount=cdgSelected.Rows.Selected.Count;

			string strDevice="";
			string strStep="";

			for(int i=1;i<=intSelectedSelectedRowCount;i++)
			{
				strDevice=cdgSelected.Rows.Selected[i-1][0].ToString();
				strStep=cdgSelected.Rows.Selected[i-1][2].ToString();

				if(!SelectionCheck2(strDevice, strStep))
				{
					cdgDefault.Rows.Add();
					cdgDefault.SetData(cdgDefault.Rows.Count-1,0,cdgSelected.Rows.Selected[i-1][0].ToString());
					cdgDefault.SetData(cdgDefault.Rows.Count-1,1,cdgSelected.Rows.Selected[i-1][1].ToString());
					cdgDefault.SetData(cdgDefault.Rows.Count-1,2,cdgSelected.Rows.Selected[i-1][2].ToString());
					cdgDefault.SetData(cdgDefault.Rows.Count-1,3,Int16.Parse(cdgSelected.Rows.Selected[i-1][3].ToString()));
					cdgDefault.SetData(cdgDefault.Rows.Count-1,4,Int16.Parse(cdgSelected.Rows.Selected[i-1][4].ToString()));
					cdgDefault.SetData(cdgDefault.Rows.Count-1,5,Int16.Parse(cdgSelected.Rows.Selected[i-1][5].ToString()));
					cdgDefault.SetData(cdgDefault.Rows.Count-1,6,Int16.Parse(cdgSelected.Rows.Selected[i-1][6].ToString()));
					cdgDefault.SetData(cdgDefault.Rows.Count-1,7,Int16.Parse(cdgSelected.Rows.Selected[i-1][7].ToString()));

					cdgDefault.GetCellRange(cdgDefault.Rows.Count-1,0,cdgDefault.Rows.Count-1,7).StyleNew.BackColor=System.Drawing.Color.LemonChiffon;

					txtDefaultAddCount.Text=(Int16.Parse(txtDefaultAddCount.Text)+1).ToString();
				}
			}
		}
		#endregion

		#region ■ btnDefaultSave_Click 

		/// <summary>
		/// 1) 현재 그룹을 DB 에서 삭제한다.
		/// 2) 해당 그룹명으로 다시 Insert 한다.
		/// </summary>
		/// 
		private void btnDefaultSave_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			// 선택된 그룹의 변경된 내용을 저장한다. (cmdDevice 에 Group 이 선택되어 있는 경우...)
			if(cmbGroup.SelectedIndex == -1 ||cmbGroup.SelectedItem.ToString() == "Select Group")
			{
				Miracom.DMS.Win.FormUtil.DisplayInfoMsg(MessageBoxIcon.Information,"새로운 그룹을 등록할경우 Save As 버튼을 클릭하세요.");
			}
			else
			{
				DeleteDefault();
				SaveDefault();			
			}

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ btnDefaultSaveAs_Click 클릭 이벤트

		/// <summary>
		/// Default Group 을 새 이름으로 저장하는 서비스 호출
		/// </summary>
		/// 
		private void btnDefaultSaveAs_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			bool bRet = false;
			string strGroupID=txtNewGroup.Text;
			// 신규 혹은 새 그룹명으로 저장 (Group 이 선택되어 있지 않은 경우만...)
			bRet = SaveAsDefault(strGroupID);
			if(bRet)
			{
				GetGroupList();
				cmbGroup.Text=strGroupID;
				MessageBox.Show("성공적으로 저장하였습니다.");
			}
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ SaveAsDefault

		/// <summary>
		/// Default Group 을 새로운 이름으로 저장하거나 새로 생성한 Group 을 저장한다.
		/// </summary>
		/// <param name="strGroupID">저장할 Group 명</param>
		/// 
		private bool SaveAsDefault(string strGroupID)
		{
			bool bRet = false;
			if(CheckGroupID(strGroupID))
			{
				MessageBox.Show("입력하신 Group명이 이미 존재합니다.다른 Group명을 입력하여 주십시오.");
				bRet = false;
			}
			else
			{
				Remoting oSpec = null;
				StringBuilder builder=new StringBuilder();
				string[,] strStepList;

				try
				{
					strStepList=new string[cdgDefault.Rows.Count-1,8];

					for(int i=1;i<cdgDefault.Rows.Count;i++)
					{
						for(int j=0;j<cdgDefault.Cols.Count;j++)
						{
							strStepList[i-1,j]=cdgDefault.GetData(i,j).ToString();
						}
					}

					//비지니스 컴포넌트 호출
					oSpec = new Remoting();
					oSpec.InsertUserSpec(strUserID, "InsertUserSpec", strGroupID, strStepList);
					bRet = true;
					//cdgDefault.GetCellRange(1,0,cdgDefault.Rows.Count-1,7).StyleNew.BackColor=System.Drawing.Color.White;
					
				}
				catch(Exception ex)
				{
					throw ex;
				}

				finally
				{
				}
			}
			return bRet;
		}
		#endregion

		#region ■ DeleteDefault

		/// <summary>
		/// 현재 선택된 그룹을 삭제한다.
		/// </summary>
		/// 
		private void DeleteDefault()
		{
			Remoting oSpec = null;
			StringBuilder builder=new StringBuilder();

			string strGroupID=cmbGroup.SelectedItem.ToString();

			try
			{
				//비지니스 컴포넌트 호출
				oSpec = new Remoting();
				oSpec.DeleteUserSpec(strUserID, "DeleteUserSpec", strGroupID);
			}
			catch(Exception ex)
			{
				throw ex;
			}

			finally
			{
			}

		}
		#endregion

		#region ■ SaveDefault

		/// <summary>
		/// 현재 그룹의 내용을 저장한다.
		/// </summary>
		/// 
		private void SaveDefault()
		{
			Remoting oSpec = null;
			StringBuilder builder=new StringBuilder();
			string[,] strStepList;

			string strGroupID=cmbGroup.SelectedItem.ToString();

			try
			{
				strStepList=new string[cdgDefault.Rows.Count-1,8];

				for(int i=1;i<cdgDefault.Rows.Count;i++)
				{
					for(int j=0;j<cdgDefault.Cols.Count;j++)
					{
						strStepList[i-1,j]=cdgDefault.GetData(i,j).ToString();
					}
				}

				//비지니스 컴포넌트 호출
				oSpec = new Remoting();
				oSpec.InsertUserSpec(strUserID, "InsertUserSpec", strGroupID, strStepList);

//				cdgDefault.GetCellRange(1,0,cdgDefault.Rows.Count-1,7).StyleNew.BackColor=System.Drawing.Color.White;

				MessageBox.Show("성공적으로 저장하였습니다.");

			}
			catch(Exception ex)
			{
				throw ex;
			}

			finally
			{
			}

		}
		#endregion

		#region ■ CheckGroupID

		/// <summary>
		/// 새로 저장할 Custom Group 명이 이미 존재하는지의 여부를 반환한다.
		/// </summary>
		/// <param name="strGroupID">저장할 Group 명</param>
		/// 
		private bool CheckGroupID(string strGroupID)
		{
			bool bGroup=false;

			if(cmbGroup.Items.Contains(strGroupID))
			{
				bGroup=true;
			}
			return bGroup;
		}
		#endregion

		#region ■ btnDefaultClear_Click 클릭이벤트

		/// <summary>
		/// Default Layer Grid의 내용을 Clear 한다.
		/// </summary>
		/// 
		private void btnDefaultClear_Click(object sender, System.EventArgs e)
		{
			ResetDefaultLayerGrid();
			txtDefaultCount.Text="0";
			txtDefaultAddCount.Text="0";
			txtDefaultChangedCount.Text="0";
			cmbGroup.SelectedItem=cmbGroup.Items[0];
		}
		#endregion

		#region ■ Default Group 콤보 변경 이벤트 : cmbGroup_SelectedIndexChanged

		/// <summary>
		/// 1) Default Layer관련 컨트롤 초기화하는 서비스 호출.
		/// 2) 
		/// </summary>
		/// 
		private void cmbGroup_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			if(cmbGroup.SelectedIndex==0)
			{
				btnDefaultClear_Click(null,null);
			}
			else
			{
				GetSelectedGroup();
			}

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ 선택한 Group 에 해당하는 Default Layer 목록을 가져옴.

		/// <summary>
		/// 선택한 Group 에 해당하는 Default Layer 목록을 가져와 cdgDefault에 바인딩한다.
		/// </summary>
		/// 
		private void GetSelectedGroup()
		{
			DataSet dsReturn=null;
			Remoting oList=null;

			string strGroupID=cmbGroup.SelectedItem.ToString();

			try
			{
				oList=new Remoting();
				
				dsReturn=oList.GetSelectedGroup(strUserID, "GetSelectedGroup", strGroupID);
				
				txtDefaultCount.Text="0";
				txtDefaultAddCount.Text="0";
				txtDefaultChangedCount.Text="0";
				//cdgDefault.BackColor=System.Drawing.Color.White;
				//cdgDefault.Clear(C1.Win.C1FlexGrid.ClearFlags.Content);
				ResetDefaultLayerGrid();

//				cdgDefault.DataSource=dsReturn.Tables[0];
				string strDevice=string.Empty;
				string strStep=string.Empty;

				//Product, Route, STEP_ID, ENG_LIMIT, CTRL_LIMIT, MAX_VALUE, MIN_VALUE, MAIN_UNIT
				for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
				{
					strDevice = dsReturn.Tables[0].Rows[i]["Product"].ToString();
					strStep = dsReturn.Tables[0].Rows[i]["STEP_ID"].ToString();
					cdgDefault.Rows.Add();
					cdgDefault.SetData(cdgDefault.Rows.Count-1,0,dsReturn.Tables[0].Rows[i][0].ToString());
					cdgDefault.SetData(cdgDefault.Rows.Count-1,1,dsReturn.Tables[0].Rows[i][1].ToString());
					cdgDefault.SetData(cdgDefault.Rows.Count-1,2,dsReturn.Tables[0].Rows[i][2].ToString());
					cdgDefault.SetData(cdgDefault.Rows.Count-1,3,Int16.Parse(dsReturn.Tables[0].Rows[i][3].ToString()));
					cdgDefault.SetData(cdgDefault.Rows.Count-1,4,Int16.Parse(dsReturn.Tables[0].Rows[i][4].ToString()));
					cdgDefault.SetData(cdgDefault.Rows.Count-1,5,Int16.Parse(dsReturn.Tables[0].Rows[i][5].ToString()));
					cdgDefault.SetData(cdgDefault.Rows.Count-1,6,Int16.Parse(dsReturn.Tables[0].Rows[i][6].ToString()));
					cdgDefault.SetData(cdgDefault.Rows.Count-1,7,Int16.Parse(dsReturn.Tables[0].Rows[i][7].ToString()));

					cdgDefault.GetCellRange(cdgDefault.Rows.Count-1,0,cdgDefault.Rows.Count-1,7).StyleNew.BackColor=System.Drawing.Color.LemonChiffon;

					//txtDefaultAddCount.Text=(Int16.Parse(txtDefaultAddCount.Text)+1).ToString();
				}

				
				txtDefaultCount.Text=(cdgDefault.Rows.Count-1).ToString();
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

		#region ■ btnDefaultRefresh_Click 클릭 이벤트

		/// <summary>
		/// Default Layer를 DB 에 있는 내용으로 Refresh 하는 서비스 호출.
		/// </summary>
		/// 
		private void btnDefaultRefresh_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			if(cmbGroup.SelectedIndex>0)
			{
				GetSelectedGroup();
			}

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ btnDefaultSelectAll_Click 클릭 이벤트

		/// <summary>
		/// Default Layer의 Select All 버튼을 클릭하면 전체 행을 선택한다.
		/// </summary>
		/// 
		private void btnDefaultSelectAll_Click(object sender, System.EventArgs e)
		{
			if(cdgDefault.Rows.Count>1)
			{
				cdgDefault.Select(1,0,cdgDefault.Rows.Count-1,cdgDefault.Cols.Count-1,true);
			}
		}
		#endregion

		#region ■ btnDefaultEdit_Click 클릭 이벤트

		/// <summary>
		/// Default Layer Grid의 선택된 Row 에 대해서 수정사항을 적용하고 수정된 필드의 배경색상을 변경한다.
		/// </summary>
		/// 
		private void btnDefaultEdit_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			int intEditCount=0;
			bool bEdit;

			for(int i=0;i<cdgDefault.Rows.Selected.Count;i++)
			{
				bEdit=false;

				if(cdgDefault.GetData(cdgDefault.Rows.Selected[i].Index,3).ToString()!=nudEng.Value.ToString())
				{
					cdgDefault.GetCellRange(cdgDefault.Rows.Selected[i].Index,3,cdgDefault.Rows.Selected[i].Index,3).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgDefault.SetData(cdgDefault.Rows.Selected[i].Index,3,nudEng.Value.ToString());
					bEdit=true;
				}


				if(cdgDefault.GetData(cdgDefault.Rows.Selected[i].Index,4).ToString()!=nudCtrl.Value.ToString())
				{
					cdgDefault.GetCellRange(cdgDefault.Rows.Selected[i].Index,4,cdgDefault.Rows.Selected[i].Index,4).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgDefault.SetData(cdgDefault.Rows.Selected[i].Index,4,nudCtrl.Value.ToString());
					bEdit=true;
				}


				if(cdgDefault.GetData(cdgDefault.Rows.Selected[i].Index,5).ToString()!=nudMax.Value.ToString())
				{
					cdgDefault.GetCellRange(cdgDefault.Rows.Selected[i].Index,5,cdgDefault.Rows.Selected[i].Index,5).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgDefault.SetData(cdgDefault.Rows.Selected[i].Index,5,nudMax.Value.ToString());
					bEdit=true;
				}


				if(cdgDefault.GetData(cdgDefault.Rows.Selected[i].Index,6).ToString()!=nudMin.Value.ToString())
				{
					cdgDefault.GetCellRange(cdgDefault.Rows.Selected[i].Index,6,cdgDefault.Rows.Selected[i].Index,6).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgDefault.SetData(cdgDefault.Rows.Selected[i].Index,6,nudMin.Value.ToString());
					bEdit=true;
				}


				if(cdgDefault.GetData(cdgDefault.Rows.Selected[i].Index,7).ToString()!=nudUnit.Value.ToString())
				{
					cdgDefault.GetCellRange(cdgDefault.Rows.Selected[i].Index,7,cdgDefault.Rows.Selected[i].Index,7).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgDefault.SetData(cdgDefault.Rows.Selected[i].Index,7,nudUnit.Value.ToString());
					bEdit=true;
				}

				if(bEdit)
				{
					intEditCount+=1;
				}

			}
			txtDefaultChangedCount.Text=(Int16.Parse(txtDefaultChangedCount.Text)+intEditCount).ToString();

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ btnEditS_Click 클릭 이벤트

		/// <summary>
		/// Selected Layer Grid의 선택된 Row 에 대해서 수정사항을 적용하고 수정된 필드의 배경색상을 변경한다.
		/// </summary>
		/// 
		private void btnEditS_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			int intEditCount=0;

			bool bEdit;

			for(int i=0;i<cdgSelected.Rows.Selected.Count;i++)
			{
				bEdit=false;

				if(cdgSelected.GetData(cdgSelected.Rows.Selected[i].Index,3).ToString()!=nudEngS.Value.ToString())
				{
					cdgSelected.GetCellRange(cdgSelected.Rows.Selected[i].Index,3,cdgSelected.Rows.Selected[i].Index,3).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgSelected.SetData(cdgSelected.Rows.Selected[i].Index,3,nudEngS.Value.ToString());
					bEdit=true;
				}

				if(cdgSelected.GetData(cdgSelected.Rows.Selected[i].Index,4).ToString()!=nudCtrlS.Value.ToString())
				{
					cdgSelected.GetCellRange(cdgSelected.Rows.Selected[i].Index,4,cdgSelected.Rows.Selected[i].Index,4).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgSelected.SetData(cdgSelected.Rows.Selected[i].Index,4,nudCtrlS.Value.ToString());
					bEdit=true;
				}

				if(cdgSelected.GetData(cdgSelected.Rows.Selected[i].Index,5).ToString()!=nudMaxS.Value.ToString())
				{
					cdgSelected.GetCellRange(cdgSelected.Rows.Selected[i].Index,5,cdgSelected.Rows.Selected[i].Index,5).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgSelected.SetData(cdgSelected.Rows.Selected[i].Index,5,nudMaxS.Value.ToString());
					bEdit=true;
				}

				if(cdgSelected.GetData(cdgSelected.Rows.Selected[i].Index,6).ToString()!=nudMinS.Value.ToString())
				{
					cdgSelected.GetCellRange(cdgSelected.Rows.Selected[i].Index,6,cdgSelected.Rows.Selected[i].Index,6).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgSelected.SetData(cdgSelected.Rows.Selected[i].Index,6,nudMinS.Value.ToString());
					bEdit=true;
				}

				if(cdgSelected.GetData(cdgSelected.Rows.Selected[i].Index,7).ToString()!=nudUnitS.Value.ToString())
				{
					cdgSelected.GetCellRange(cdgSelected.Rows.Selected[i].Index,7,cdgSelected.Rows.Selected[i].Index,7).StyleNew.BackColor=System.Drawing.Color.Yellow;
					cdgSelected.SetData(cdgSelected.Rows.Selected[i].Index,7,nudUnitS.Value.ToString());
					bEdit=true;
				}

				if(bEdit)
				{
					intEditCount+=1;
				}
			}
			txtSelectedChangedCount.Text=(Int16.Parse(txtSelectedChangedCount.Text)+intEditCount).ToString();

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ btnDelete_Click 클릭 이벤트

		/// <summary>
		/// Default Layer Grid의 선택된 Row를 화면에서 삭제함.
		/// </summary>
		/// 
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			int intSelectedCount=cdgDefault.Rows.Selected.Count;

			for(int i=intSelectedCount-1;i>=0;i--)
			{
				cdgDefault.Rows.Remove(cdgDefault.Rows.Selected[i].Index);
				txtDefaultChangedCount.Text=(Int16.Parse(txtDefaultChangedCount.Text)+1).ToString();
				txtDefaultCount.Text=(Int16.Parse(txtDefaultCount.Text)-1).ToString();
			}

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ btnDeleteS_Click 클릭 이벤트

		/// <summary>
		/// Selected Layer Grid의 선택된 Row를 화면에서 삭제함.
		/// </summary>
		/// 
		private void btnDeleteS_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			int intSelectedCount=cdgSelected.Rows.Selected.Count;

			for(int i=intSelectedCount-1;i>=0;i--)
			{
				cdgSelected.Rows.Remove(cdgSelected.Rows.Selected[i].Index);
				txtSelectedChangedCount.Text=(Int16.Parse(txtSelectedChangedCount.Text)+1).ToString();
				txtSelectedCount.Text=(Int16.Parse(txtSelectedCount.Text)-1).ToString();
			}

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ btnDeleteGroup_Click 클릭 이벤트

		/// <summary>
		/// 1) 선택된 Default Group 을 삭제하는 서비스 호출
		/// 2) Default Layer Grid 초기화하는 서비스 호출
		/// </summary>
		/// 
		private void btnDeleteGroup_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			DeleteDefault();
			btnDefaultClear_Click(null,null);

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ GetScheduleTime

		/// <summary>
		/// 스케쥴을 등록할 Date Time을 Formatting 한다.
		/// </summary>
		/// 
		private string GetScheduleTime()
		{
			string strScheduleTime="";

			strScheduleTime = dtpScheduleDate.Value.Year.ToString().PadLeft(4,'0') + "-" +
				dtpScheduleDate.Value.Month.ToString().PadLeft(2,'0') + "-" +
				dtpScheduleDate.Value.Day.ToString().PadLeft(2,'0') + " " +
				dtpScheduleTime.Value.Hour.ToString().PadLeft(2,'0') + ":" +
				dtpScheduleTime.Value.Minute.ToString().PadLeft(2,'0') + ":" +
				dtpScheduleTime.Value.Second.ToString().PadLeft(2,'0'); 

			return strScheduleTime;
		}
		#endregion

		#region ■ btnRegistSchedule_Click 클릭이벤트

		/// <summary>
		/// 스케쥴을 등록할 Default Layer 가 선택되어 있는지 확인하여 선택되어 있으면 RegistSchedule 서비스 호출.
		/// </summary>
		/// 
		private void btnRegistSchedule_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			if(cmbScheduleGroup.SelectedIndex<1)
			{
				MessageBox.Show("실행할 Default Layer Group을 선택하여주십시오.");
				cmbScheduleGroup.Focus();
			}
			else
			{
				RegistSchedule();
			}

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ RegistSchedule

		/// <summary>
		/// 새로운 스케쥴을 등록한다.
		/// </summary>
		/// 
		private void RegistSchedule()
		{
			Remoting oSchedule = null;
			StringBuilder builder=new StringBuilder();

			try
			{
				//비지니스 컴포넌트 호출
				oSchedule = new Remoting();
				oSchedule.InsertSchedule(strUserID, "InsertSchedule", GetScheduleTime(), "TrendReport" , cmbScheduleGroup.Text);

				MessageBox.Show("성공적으로 등록하였습니다.");
				GetScheduleList();
			}
			catch(Exception ex)
			{
				throw ex;
			}

			finally
			{
			}
		}
		#endregion

		#region ■ 사용자 Reporting Schedule 목록을 가져옴.

		/// <summary>
		/// DB 에 저장된 Report 스케쥴 목록을 가져와 Schedule Grid에 바인딩한다.
		/// </summary>
		/// 
		private void GetScheduleList()
		{
			DataSet dsReturn=null;
			Remoting oScheduleList=null;

			try
			{
				oScheduleList=new Remoting();

				dsReturn=oScheduleList.GetScheduleList(strUserID, "GetScheduleList","TrendReport");

				cdgSchedule.DataSource=dsReturn.Tables[0];

			}

			catch(Exception ex)
			{
				throw ex;
				//Miracom.DMS.Config.
			}

			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
				//if(oScheduleList != null) oScheduleList.Dispose();
			}
		}
		#endregion

		#region ■ btnDeleteSchedule_Click 클릭이벤트

		/// <summary>
		/// 1) Schedule Grid 상의 선택된 스케쥴을 삭제한다.
		/// 2) Grid를 정리하기 위하여 GetScheduleList 를 호출하여 초기화 한다. (현재 남아 있는 스케쥴 목록을 화면에 Display)
		/// </summary>
		/// 
		private void btnDeleteSchedule_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			DeleteSchedule();
			GetScheduleList();

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ DeleteSchedule

		/// <summary>
		/// Schedule Grid 상의 선택된 스케쥴을 삭제한다.
		/// </summary>
		/// 
		private void DeleteSchedule()
		{
			Remoting oSchedule = null;
			StringBuilder builder=new StringBuilder();

			try
			{
				oSchedule = new Remoting();
				for(int i=0;i<cdgSchedule.Rows.Selected.Count;i++)
				{
					oSchedule.DeleteSchedule(strUserID, "DeleteSchedule","TrendReport", cdgSchedule.GetData(cdgSchedule.Rows.Selected[i].Index,1).ToString());
				}
				MessageBox.Show("성공적으로 삭제하였습니다.");

				//비지니스 컴포넌트 호출
			}
			catch(Exception ex)
			{
				throw ex;
			}

			finally
			{
				//if(oSchedule != null) oSchedule.Dispose();
			}

		}
		#endregion

		#region ■ btnToExcel_Click 클릭 이벤트

		/// <summary>
		/// 선택한 Report Source (Default/Selected) 를 가지고 ExportToExcel 서비스를 호출한다.
		/// </summary>
		/// 
		private void btnToExcel_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			MessageBox.Show("Excel Reporting을 시작합니다. 작업이 완료될때까지 마우스로 클릭하거나 키보드입력등의 작업을 중지하여 주십시오.");

			if((chkTot.Checked==false) && (chkDR.Checked==false) && (chkTot5.Checked==false) && (chkDR5.Checked==false))
			{
				MessageBox.Show("Category가 하나도 선택되지 않았습니다. 적어도 한개 이상의 Category를 선택하여 주십시오.");
			}
			else
			{
				if(rdoDefault.Checked)
				{
					ExportToExcel(cdgDefault);
				}
				else
				{
					ExportToExcel(cdgSelected);
				}
			}


			this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		#endregion

		#region ■ 선택한 조건으로 Report Data 를 쿼리

		/// <summary>
		/// 선택한 조건으로 Report Data 를 쿼리하여 가져온다.
		/// 변경 --> displayOption 추가
		/// </summary>
		/// <param name="strStep">Step ID</param>
		/// <param name="intLimit">Limit 수치</param></param>
		/// <param name="bTot5"> 0.5 이상이면 true 전체 Defect 조건이면 false </param>
		/// <param name="displayOption">Chart OR Map Display Option</param>
		/// <returns></returns>
		//private bool GetReport(string strStep, int intLimit, bool bTot5)
		private bool GetReport(string strStep, int intLimit, bool bTot5 , string displayOption)
		{

			DataSet dsReturn=null;
			Remoting oReport=null;
			bool bData=false;

			try
			{
				GetPeriod();

				oReport=new Remoting();

				string strSpecLimit="NoLimit";

				// Chart 는 NoLimit으로, Map 은 선택된 옵션에 따라 데이터를 조회한다.
				if(displayOption.Equals("Map"))
				{
					if(rdoSpec1.Checked)
					{
						strSpecLimit="LimitOut";
					}
					else if(rdoSpec2.Checked)
					{
						strSpecLimit="LimitIn";
					}
				}

				dsReturn=oReport.GetReport(strUserID, "GetDefectDRCount",strFromDate, strToDate, strFromTime, strToTime, cmbShift.SelectedItem.ToString(), strStep, intLimit, strSpecLimit, bTot5);

				if(dsReturn.Tables[0].Rows.Count>0)
				{
					bData=true;

					strStepSeqList=new string[dsReturn.Tables[0].Rows.Count,8];

					for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
					{
						chtChart.MarkerSize = 2;

						chtChart.Legend[i]=dsReturn.Tables[0].Rows[i][4].ToString().Substring(5,2) + "/" + dsReturn.Tables[0].Rows[i][4].ToString().Substring(8,2);

						strStepSeqList[i,0]=dsReturn.Tables[0].Rows[i][0].ToString();
						strStepSeqList[i,1]=dsReturn.Tables[0].Rows[i][1].ToString();
						strStepSeqList[i,2]=dsReturn.Tables[0].Rows[i][2].ToString();
						strStepSeqList[i,3]=dsReturn.Tables[0].Rows[i][3].ToString();
						strStepSeqList[i,4]=dsReturn.Tables[0].Rows[i][4].ToString();
						if(!bTot5)
						{
							strStepSeqList[i,5]=dsReturn.Tables[0].Rows[i][5].ToString();
							strStepSeqList[i,6]=dsReturn.Tables[0].Rows[i][6].ToString();
						}
						else
						{
							strStepSeqList[i,5]=dsReturn.Tables[0].Rows[i][7].ToString();
							strStepSeqList[i,6]=dsReturn.Tables[0].Rows[i][8].ToString();
						}
						strStepSeqList[i,7]=dsReturn.Tables[0].Rows[i][9].ToString();


						switch(iCateFlag)
						{
							case 1:
								chtChart.Value[0, i]=Int32.Parse(strStepSeqList[i,5]);
								chtChart.Value[1, i]=Int32.Parse(strStepSeqList[i,6]);
								break;
							case 2:
								chtChart.Value[0, i]=Int32.Parse(strStepSeqList[i,5]);
								break;
							case 3:
								chtChart.Value[0, i]=Int32.Parse(strStepSeqList[i,6]);
								break;
						}
					}

					chtChart.CloseData(SoftwareFX.ChartFX.COD.Values);

					dsReturn.Tables[0].Columns.Remove("PRODUCT");
					dsReturn.Tables[0].Columns.Remove("LOT"); 
					dsReturn.Tables[0].Columns.Remove("STEP");
					dsReturn.Tables[0].Columns.Remove("STEP_SEQ"); 

					if(!bTot5)
					{
						dsReturn.Tables[0].Columns.Remove("Def0_5");
						dsReturn.Tables[0].Columns.Remove("DR0_5"); 
					}
					else
					{
						dsReturn.Tables[0].Columns.Remove("Def");
						dsReturn.Tables[0].Columns.Remove("DR"); 
					}
					//chtChart.DataSource=dsReturn.Tables[0];
				}

				return bData;

			}

			catch(Exception ex)
			{
				throw ex;
				//Miracom.DMS.Config.
			}

			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
				//if(oReport != null) oReport.Dispose();
			}

		}
		#endregion

		#region ■ ResetDefaultLayerGrid

		/// <summary>
		/// Default Grid 초기화
		/// </summary>
		/// 
		private void ResetDefaultLayerGrid()
		{

			cdgDefault.Clear();
			cdgDefault.DataSource=null;
			cdgDefault.Rows.Count=1;
			cdgDefault.Cols.Count=8;
			cdgDefault.Cols[2].Width = 200;

			cdgDefault.SetData(0,0,"PRODUCT");
			cdgDefault.SetData(0,1,"ROUTE");
			cdgDefault.SetData(0,2,"STEP");
			cdgDefault.SetData(0,3,"ENG_LIMIT");
			cdgDefault.SetData(0,4,"CTRL_LIMIT");
			cdgDefault.SetData(0,5,"MAX");
			cdgDefault.SetData(0,6,"MIN");
			cdgDefault.SetData(0,7,"Unit");
		}
		#endregion

		#region ■ ResetSelectedLayerGrid

		/// <summary>
		/// Selected Grid 초기화
		/// </summary>
		/// 
		private void ResetSelectedLayerGrid()
		{
			//Selected Layer Grid 설정
			cdgSelected.DataSource=null;
			cdgSelected.Rows.Count=1;
			cdgSelected.Cols.Count=8;

			cdgSelected.SetData(0,0,"PRODUCT");
			cdgSelected.SetData(0,1,"ROUTE");
			cdgSelected.SetData(0,2,"STEP");
			cdgSelected.SetData(0,3,"ENG_LIMIT");
			cdgSelected.SetData(0,4,"CTRL_LIMIT");
			cdgSelected.SetData(0,5,"MAX");
			cdgSelected.SetData(0,6,"MIN");
			cdgSelected.SetData(0,7,"Unit");
		}
		#endregion

		#region ■ ResetLayerGrid

		/// <summary>
		/// Layer 선택 Grid 초기화
		/// </summary>
		/// 
		private void ResetLayerGrid()
		{
			//Layer Grid 설정
			cdgLayer.DataSource=null;
			cdgLayer.Rows.Count=1;
			cdgLayer.Cols.Count=3;

			cdgLayer.SetData(0,0,"PRODUCT");
			cdgLayer.SetData(0,1,"STEP_ID");
			cdgLayer.SetData(0,2,"TEST No");
		}
		#endregion

		#region ■ ResetSpecGrid

		/// <summary>
		/// 기준 Spec Grid 초기화
		/// </summary>
		/// 
		private void ResetSpecGrid()
		{
			//Spec Option Grid 설정
			cdgSpec.DataSource=null;
			cdgSpec.Rows.Count=1;
			cdgSpec.Cols.Count=4;

			cdgSpec.SetData(0,0,"ROUTE");
			cdgSpec.SetData(0,1,"STEP");
			cdgSpec.SetData(0,2,"ENG_LIMIT");
			cdgSpec.SetData(0,3,"CTRL_LIMIT");
		}
		#endregion

		#region ■ ResetScheduleGrid
		/// <summary>
		/// Schedule 목록 그리드 초기화
		/// </summary>
		/// 
		private void ResetScheduleGrid()
		{
			//Schedule Grid 설정
			cdgSchedule.DataSource=null;
			cdgSchedule.Rows.Count=1;
			cdgSchedule.Cols.Count=3;

			cdgSchedule.SetData(0,0,"Time");
			cdgSchedule.SetData(0,1,"Group");
			cdgSchedule.SetData(0,2,"Remark");
		}
		#endregion

		#region ■ 엑셀로 Export
		private DataSet GetChartData(string strStep, int intLimit, bool bTot5)
		{
			DataSet dsReturn=null;
			Remoting oReport=null;
			try
			{
				GetPeriod();

				oReport=new Remoting();

				string strSpecLimit="NoLimit";

//				if(rdoSpec1.Checked)
//				{
//					strSpecLimit="LimitOut";
//				}
//				else if(rdoSpec2.Checked)
//				{
//					strSpecLimit="LimitIn";
//				}

				dsReturn=oReport.GetReport(strUserID, "GetDefectDRCount",strFromDate, strToDate, strFromTime, strToTime, cmbShift.SelectedItem.ToString(), strStep, intLimit, strSpecLimit, bTot5);
				return dsReturn;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
			}
		}
		/// <summary>
		/// 선택한 그리드의 내용과 옵션사항을 기준으로 엑셀 Reporting.
		/// </summary>
		/// <param name="cdgGrid">그리드 오브젝트 (cdgDefault 혹은 cdgSelected)</param>
		/// 
		private void ExportToExcel(C1.Win.C1FlexGrid.C1FlexGrid cdgGrid)
		{
			bool bResult=false;
			string sChartRsndA = string.Empty;
			string sChartRsndB = string.Empty;
			string sChartEngLimit = string.Empty;
			string sColumnDR = string.Empty;
			string sColumnDef = string.Empty;
			int iLoop = 0;
			DataSet dsChartData = null;
			DataTable dtChartData = null;
			string sChartTitle = string.Empty;
			double dFirstScanDate = 0;
			double dMaxScaleLinearView = 0;

			DateTime[,] dteTimeAxis = null;
			double[,] dbDefData = null;
			double[,] dbDRData = null;
			double[,] dbEngLimit = null;
			string[,] sExcelTitle = new string[1,1];
			bool bIsOneXValue = false;
			string sChartTrendName = string.Empty;
			int iEngLimit = 0;
			int iXlDataLastRow = 0;

			if(cdgGrid.Rows.Count<2)
			{
				MessageBox.Show("Default Group을 선택하여주십시오.");
			}
			else
			{
				//Chart Export
				//0.객체 선언 , 인스턴싱
				Miracom.DMS.Win.ExcelUtil oXL = new Miracom.DMS.Win.ExcelUtil();

				//1.엑셀 오브젝트 얻어 오기
				Excel._Workbook oWB = oXL.fnGetExcelWorkbook(true,3);

				//2.엑셀 시트명 세팅
				oXL.fnSetSheetName(oWB,1,"Defect Trend Report");

				string[,] strHeader;
				strHeader=new string[1,1];

				strHeader[0,0]="Defect Trend Report";
				oXL.fnSetValue(oWB,1,1,1,1,1,strHeader, true,"Times New Roman", 16, Miracom.DMS.Win.ExcelUtil.Color.black, Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, false, Miracom.DMS.Win.ExcelUtil.Color.white);

				intExcelRow=3;
				intExcelCol=1;

				int intMapPeriod=-(Int32.Parse(nudMapPeriod.Value.ToString()));

				strHeader=new string[4,1];
				strHeader[0,0]="* Chart Period : " + dtpFromDate.Value.ToShortDateString() + " " + dtpFromTime.Value.ToShortTimeString() + " ~ " + dtpToDate.Value.ToShortDateString() + " " + dtpToTime.Value.ToShortTimeString();
				strHeader[1,0]="* Map Period : " + Convert.ToDateTime(dtpToDate.Value.ToShortDateString() + " " + dtpToTime.Value.Hour.ToString().PadLeft(2,'0') + ":" + dtpToTime.Value.Minute.ToString().PadLeft(2,'0')).AddDays(intMapPeriod) + " ~ " + dtpToDate.Value.ToShortDateString() + " " + dtpToTime.Value.ToShortTimeString();
				strHeader[2,0]="* Shift : " + cmbShift.SelectedItem;

				oXL.fnSetValue(oWB,1,intExcelRow,intExcelCol,intExcelRow+3,1,strHeader, false,"Times New Roman", 12, Miracom.DMS.Win.ExcelUtil.Color.black, Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, false, Miracom.DMS.Win.ExcelUtil.Color.white);

				intExcelRow=8;
				intExcelCol=1;

				// Linear View MaxScale 값 설정
				if(rdoDefault.Checked)
				{
					dMaxScaleLinearView = Convert.ToDouble(nudMax.Value.ToString());
				}
				else
				{
					dMaxScaleLinearView = Convert.ToDouble(nudMaxS.Value.ToString());
				}

				for(int i=1;i<cdgGrid.Rows.Count;i++)
				{
					int intLimit=Int32.Parse(cdgGrid.GetData(i,3).ToString());

					if(chkLimit.Checked)
					{
						intLimit=Int32.Parse(cdgGrid.GetData(i,4).ToString());
					}				

					bool bTot5;
					iEngLimit = intLimit;
					// 0.5 가 아닌 Total Defect Count 나 DR 을 선택했을 경우...
					if((chkTot.Checked==true) || (chkDR.Checked==true))
					{
						sChartRsndA = "Def Cnt";
						sChartRsndB = "DR Cnt";
						sColumnDR = "DR";
						sColumnDef = "DEF";
						bTot5=false;
						if((chkTot.Checked==true) && (chkDR.Checked==true))
						{
							iCateFlag=1;
						}
						else if((chkTot.Checked==true) && (chkDR.Checked==false))
						{
							iCateFlag=2;
						}
						else
						{
							iCateFlag=3;
						}

					}
						// 0.5 이상인 Total Defect Count 나 DR 을 선택했을 경우...
					else
					{
						sChartRsndA = "Def.5 Cnt";
						sChartRsndB = "DR.5 Cnt";
						sColumnDR = "DR0_5";
						sColumnDef = "DEF0_5";
						bTot5=true;
						if((chkTot5.Checked==true) && (chkDR5.Checked==true))
						{
							iCateFlag=1;
						}
						else if((chkTot5.Checked==true) && (chkDR5.Checked==false))
						{
							iCateFlag=2;
						}
						else
						{
							iCateFlag=3;
						}
					}

					sChartEngLimit = "ENGLIMIT ";
					dsChartData = GetChartData(cdgGrid.GetData(i,2).ToString(),intLimit,bTot5);

//					if(GetReport(cdgGrid.GetData(i,2).ToString(),intLimit,bTot5))
					if(GetReport(cdgGrid.GetData(i,2).ToString(),intLimit,bTot5,"Chart"))
					{

						iLoop += 1;
						bResult=true;

						strHeader[0,0]=cdgGrid.GetData(i,2).ToString();
						oXL.fnSetValue(oWB,1,intExcelRow,intExcelCol,intExcelRow,intExcelCol,strHeader, true,"Times New Roman", 11, Miracom.DMS.Win.ExcelUtil.Color.black, Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, false, Miracom.DMS.Win.ExcelUtil.Color.white);
						intExcelRow+=1;
						//엑셀에 차트 이미지 인서트
						if(dsChartData != null && dsChartData.Tables[0].Rows.Count > 0)
						{

							dtChartData = dsChartData.Tables[0];	
							dteTimeAxis = new DateTime[dtChartData.Rows.Count,1];
							dbDefData = new double[dtChartData.Rows.Count,1];
							dbDRData = new double[dtChartData.Rows.Count,1];
							dbEngLimit = new double[dtChartData.Rows.Count,1];
							int iColPos = 0;
//							iColPos = 4*i;


							for(int k=0;k<dtChartData.Rows.Count;k++)
							{
								dteTimeAxis[k,0] = DateTime.Parse(dtChartData.Rows[k]["SCAN_DATE"].ToString());
								dbDefData[k,0] = Double.Parse(dtChartData.Rows[k][sColumnDef].ToString());
								dbDRData[k,0] = Double.Parse(dtChartData.Rows[k][sColumnDR].ToString());
								dbEngLimit[k,0] = iEngLimit;
							}
							if(dtChartData.Rows.Count == 1)
							{
								bIsOneXValue = true;
								dFirstScanDate = dteTimeAxis[0,0].ToOADate();
							}
							else
							{
								bIsOneXValue = false;
							}
							
							

							//데이타 타이틀 설정
							oXL.fnSetValue(oWB,2,iXlDataLastRow+1,iColPos+1,iXlDataLastRow+1,iColPos+1,strHeader,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,
								Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.darkyellow);  
							//시간축 데이타 
							oXL.fnSetValue(oWB,2,iXlDataLastRow+2,iColPos+1,iXlDataLastRow+dtChartData.Rows.Count+1,iColPos+1,dteTimeAxis,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,
								Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);         
							Excel.Range oRangeXDateValue = oXL.fnGetRange(oWB,2,iXlDataLastRow+2,iColPos+1,iXlDataLastRow+dtChartData.Rows.Count+1,iColPos+1);
							//oRangeXDateValue.NumberFormat = "YY/MM/DD HH:MM";
							oRangeXDateValue.NumberFormat = "MM/DD";

							//데이타 Def 타이틀 설정
							sExcelTitle[0,0] = sChartRsndA;
							oXL.fnSetValue(oWB,2,iXlDataLastRow+1,iColPos+2,iXlDataLastRow+1,iColPos+2,sExcelTitle,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,
								Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);  
							//Def DATA
							oXL.fnSetValue(oWB,2,iXlDataLastRow+2,iColPos+2,iXlDataLastRow+dtChartData.Rows.Count+1,iColPos+2,dbDefData,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, 
								Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);     
							Excel.Range oRangeDefValue = oXL.fnGetRange(oWB,2,iXlDataLastRow+2,iColPos+2,iXlDataLastRow+dtChartData.Rows.Count+1,iColPos+2);
							
							//데이타 DR 타이틀 설정
							sExcelTitle[0,0] = sChartRsndB;
							oXL.fnSetValue(oWB,2,iXlDataLastRow+1,iColPos+3,iXlDataLastRow+1,iColPos+3,sExcelTitle,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,
								Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);  
							//DR DATA
							oXL.fnSetValue(oWB,2,iXlDataLastRow+2,iColPos+3,iXlDataLastRow+dtChartData.Rows.Count+1,iColPos+3,dbDRData,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, 
								Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);     
							Excel.Range oRangeDRValue = oXL.fnGetRange(oWB,2,iXlDataLastRow+2,iColPos+3,iXlDataLastRow+dtChartData.Rows.Count+1,iColPos+3);

							//Engineering Limit 설정
							sExcelTitle[0,0] = sChartEngLimit;
							oXL.fnSetValue(oWB,2,iXlDataLastRow+1,iColPos+4,iXlDataLastRow+1,iColPos+4,sExcelTitle,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,
								Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);  
							//ENGLIMIT DATA
							oXL.fnSetValue(oWB,2,iXlDataLastRow+2,iColPos+4,iXlDataLastRow+dtChartData.Rows.Count+1,iColPos+4,dbEngLimit,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, 
								Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);     
							Excel.Range oRangeENGLValue = oXL.fnGetRange(oWB,2,iXlDataLastRow+2,iColPos+4,iXlDataLastRow+dtChartData.Rows.Count+1,iColPos+4);


							iXlDataLastRow += dtChartData.Rows.Count + 2;
							//차트 설정 코드 : 차트 추가시마다 반복적으로 호출함.
							Excel._Chart oChartInst = null; 
							oChartInst = oXL.fnGetChartObject((Excel.Worksheet)oWB.Worksheets[1],5,(iLoop*230.25)-80,360,180);  //현재 워크시트의 차트오브젝트를 바인딩
							
							switch(iCateFlag)
							{
								case 1://def && dr
									if(rdoLog.Checked)
									{
										if(bIsOneXValue)
										{
											oXL.fnChardWizardLogViewOneTimeXValue(oChartInst,oRangeXDateValue,oRangeDefValue,strHeader[0,0],sChartRsndA,double.Parse(nudLogMax.Value.ToString()),dFirstScanDate,false); 
										}
										else
										{
											oXL.fnChardWizardLogView(oChartInst,oRangeXDateValue,oRangeDefValue,strHeader[0,0],sChartRsndA,double.Parse(nudLogMax.Value.ToString()),false); 
										}
									}
									else
									{
										if(bIsOneXValue)
										{
											oXL.fnChardWizardOneTimeXValue(oChartInst,oRangeXDateValue,oRangeDefValue,strHeader[0,0],sChartRsndA,dFirstScanDate,false,dMaxScaleLinearView);
										}
										else
										{
											oXL.fnChardWizard(oChartInst,oRangeXDateValue,oRangeDefValue,strHeader[0,0],sChartRsndA,false,dMaxScaleLinearView);
										}		
									}
									oXL.fnSetChardAddSeries(oChartInst,oRangeXDateValue,oRangeDRValue,sChartRsndB,2); 
									oXL.fnSetChardAddSeriesLimitLine(oChartInst,oRangeXDateValue,oRangeENGLValue,sChartEngLimit,3,iEngLimit,bIsOneXValue); 
									break;
								case 2://def
									if(rdoLog.Checked)
									{
										if(bIsOneXValue)
										{
											oXL.fnChardWizardLogViewOneTimeXValue(oChartInst,oRangeXDateValue,oRangeDefValue,strHeader[0,0],sChartRsndA,double.Parse(nudLogMax.Value.ToString()),dFirstScanDate,false);  
										}
										else
										{
											oXL.fnChardWizardLogView(oChartInst,oRangeXDateValue,oRangeDefValue,strHeader[0,0],sChartRsndA,double.Parse(nudLogMax.Value.ToString()),false);  
										}		
									}
									else
									{
										if(bIsOneXValue)
										{
											oXL.fnChardWizardOneTimeXValue(oChartInst,oRangeXDateValue,oRangeDefValue,strHeader[0,0],sChartRsndA,dFirstScanDate,false,dMaxScaleLinearView); 
										}
										else
										{
											oXL.fnChardWizard(oChartInst,oRangeXDateValue,oRangeDefValue,strHeader[0,0],sChartRsndA,false,dMaxScaleLinearView); 
										}		
									}
									oXL.fnSetChardAddSeriesLimitLine(oChartInst,oRangeXDateValue,oRangeENGLValue,sChartEngLimit,2,iEngLimit,bIsOneXValue); 
									break;
								case 3://dr
									if(rdoLog.Checked)
									{
										if(bIsOneXValue)
										{
											oXL.fnChardWizardLogViewOneTimeXValue(oChartInst,oRangeXDateValue,oRangeDRValue,strHeader[0,0],sChartRsndB,double.Parse(nudLogMax.Value.ToString()),dFirstScanDate,false);
										}
										else
										{
											oXL.fnChardWizardLogView(oChartInst,oRangeXDateValue,oRangeDRValue,strHeader[0,0],sChartRsndB,double.Parse(nudLogMax.Value.ToString()),false);
										}		
									}
									else
									{
										if(bIsOneXValue)
										{
											oXL.fnChardWizardOneTimeXValue(oChartInst,oRangeXDateValue,oRangeDRValue,strHeader[0,0],sChartRsndB,dFirstScanDate,false,dMaxScaleLinearView);
										}
										else
										{
											oXL.fnChardWizard(oChartInst,oRangeXDateValue,oRangeDRValue,strHeader[0,0],sChartRsndB,false,dMaxScaleLinearView);
										}		
										
									}
									oXL.fnSetChardAddSeriesLimitLine(oChartInst,oRangeXDateValue,oRangeENGLValue,sChartEngLimit,2,iEngLimit,bIsOneXValue); 
									break;
							}



						}
						// 날짜 체크해서 맵을 그릴 것인지 확인
						string strMapInfoPath = @"c:\temp.png";
						Bitmap imgMapInfo = null;
						StringBuilder sb = new StringBuilder();

						if(GetReport(cdgGrid.GetData(i,2).ToString(),intLimit,bTot5,"Map"))
						{
							for (int j=0;j<strStepSeqList.GetLongLength(0);j++)
							{								
								if((Convert.ToDateTime(dtpToDate.Value.ToShortDateString() + " " + dtpToTime.Value.Hour.ToString().PadLeft(2,'0') + ":" + dtpToTime.Value.Minute.ToString().PadLeft(2,'0')).AddDays(intMapPeriod)) <= Convert.ToDateTime(strStepSeqList[j,4]))
								{
									if(bTot5)
									{
										Miracom.DMS.LIB.MapGen.MapGenerate.Generate(strUserID, @"C:",strStepSeqList[j,3], 150,500,300000000);
									}
									else
									{
										Miracom.DMS.LIB.MapGen.MapGenerate.Generate(strUserID, @"C:",strStepSeqList[j,3], 150);
									}

									oXL.fnSetImage(oWB,1,@"C:\"+strStepSeqList[j,3].ToString()+".png",intExcelRow,intExcelCol+7,1.0f,1.0f);

									/// ====================================================================
									///	- 수정자 : 미라콤 임영신
									///	- 수정일 : 2005-05-17
									///	- 변경로그 : 맵 정보를 텍스트에서 이미지로 변경하여 엑셀로 보냄
	
									//								strHeader[0,0]="Dev: "+ strStepSeqList[j,0];// Product
									//								oXL.fnSetValue(oWB,1,intExcelRow+9,intExcelCol+7,intExcelRow+9,intExcelCol+8,strHeader,false,"돋움체",9,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white,true);
									//
									//								strHeader[0,0]="Lot: "+ strStepSeqList[j,1];// Lot
									//								oXL.fnSetValue(oWB,1,intExcelRow+10,intExcelCol+7,intExcelRow+10,intExcelCol+8,strHeader,false,"돋움체",9,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white,true);
									//
									//								strHeader[0,0]="Step: "+ strStepSeqList[j,2];// Step ID
									//								oXL.fnSetValue(oWB,1,intExcelRow+11,intExcelCol+7,intExcelRow+11,intExcelCol+8,strHeader,false,"돋움체",9,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white,true);
									//
									//								strHeader[0,0]="Wafer: "+ strStepSeqList[j,7];// Wafer ID
									//								oXL.fnSetValue(oWB,1,intExcelRow+12,intExcelCol+7,intExcelRow+12,intExcelCol+8,strHeader,false,"돋움체",9,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white, true);
									//
									//								strHeader[0,0]="Def: "+ strStepSeqList[j,5];// Defect Count / 0.5 Defect Count
									//								oXL.fnSetValue(oWB,1,intExcelRow+13,intExcelCol+7,intExcelRow+13,intExcelCol+7,strHeader,false,"돋움체",9,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white,false);
									//
									//								strHeader[0,0]="DR: "+ strStepSeqList[j,6];// DR Count / 0.5 DR Count
									//								oXL.fnSetValue(oWB,1,intExcelRow+13,intExcelCol+8,intExcelRow+13,intExcelCol+8,strHeader,false,"돋움체",9,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white,false);
									//
									//								strHeader[0,0]="Date: "+ strStepSeqList[j,4];// Date
									//								oXL.fnSetValue(oWB,1,intExcelRow+14,intExcelCol+7,intExcelRow+14,intExcelCol+8,strHeader,false,"돋움체",9,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white, true);
								
									imgMapInfo = new Bitmap(190, 100);
									Graphics g = Graphics.FromImage(imgMapInfo);
									sb.Append("Dev: "+ strStepSeqList[j,0] + "\r\n");
									sb.Append("Lot: "+ strStepSeqList[j,1] + "\r\n");
									sb.Append("Step: "+ strStepSeqList[j,2] + "\r\n");
									sb.Append("Wafer: "+ strStepSeqList[j,7] + "\r\n");
									sb.Append("Def: "+ strStepSeqList[j,5] + "\r\n");
									sb.Append("DR: "+ strStepSeqList[j,6] + "\r\n");
									sb.Append("Date: "+ strStepSeqList[j,4] + "\r\n");

									g.FillRectangle(new System.Drawing.SolidBrush(Color.White), 0, 0, imgMapInfo.Width, imgMapInfo.Height);
									g.DrawString(sb.ToString(),
										new System.Drawing.Font("Arial", 9),
										new System.Drawing.SolidBrush(System.Drawing.Color.Black),
										0.0f, 0.0f);
									g.Dispose();
									sb.Remove(0,sb.Length);								
									imgMapInfo.Save(strMapInfoPath, System.Drawing.Imaging.ImageFormat.Png);
									oXL.fnSetImage(oWB,1,strMapInfoPath,intExcelRow  + 9,intExcelCol+7,1.0f,1.0f);
									System.IO.File.Delete(strMapInfoPath);
									/// =========================================================================================

									intExcelCol+=2;

									if(intExcelCol>250)
									{
										intExcelCol=1;
										intExcelRow+=18;
									}	
									System.IO.File.Delete(@"C:\"+strStepSeqList[j,3].ToString()+".png");
								}
							}
						}
						intExcelCol=1;
						intExcelRow+=16;
					}
				}
				if(!bResult)
				{
					strHeader[0,0]="No Data !!";
					oXL.fnSetValue(oWB,1,intExcelRow+2,1,intExcelRow+2,1,strHeader, false,"Times New Roman", 12, Miracom.DMS.Win.ExcelUtil.Color.black, Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, false, Miracom.DMS.Win.ExcelUtil.Color.white);
				}
				strHeader[0,0]="-------------------------------------------------- End of Report ----------------------------------------------------";
				oXL.fnSetValue(oWB,1,intExcelRow+4,1,intExcelRow+4,1,strHeader, true,"Times New Roman", 12, Miracom.DMS.Win.ExcelUtil.Color.black, Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter,Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, false, Miracom.DMS.Win.ExcelUtil.Color.white);

				oXL = null;
				Miracom.DMS.Win.ExcelUtil.fnExcelProcessExit();
			}


		}
		#endregion

		#region ■ ChartReset

		/// <summary>
		/// Chart Reset
		/// </summary>
		/// 
		private void ChartReset()
		{
			chtChart.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
			chtChart.ClearData(SoftwareFX.ChartFX.ClearDataFlag.Labels);

			chtChart.OpenData(SoftwareFX.ChartFX.COD.Values, 1, 1);
			chtChart.Value[0, 0] = 0;
			chtChart.CloseData(SoftwareFX.ChartFX.COD.Values);

		}
		#endregion

		#region ■ rdoLinear_CheckedChanged 변경 이벤트

		/// <summary>
		/// 차트옵션 선택에서 Linear 를 선택하면 최대값, 단위의 Custom 설정이 불가능하게 설정한다.
		/// </summary>
		/// 
		private void rdoLinear_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rdoLinear.Checked)
			{
				nudLogMax.ReadOnly=true;
				nudLogUnit.ReadOnly=true;
			}
		}
		#endregion

		#region ■ rdoLog_CheckedChanged 변경 이벤트

		/// <summary>
		/// 차트옵션 선택에서 Log 를 선택하면 최대값, 단위의 Custom 설정이 가능하게 설정한다.
		/// </summary>
		/// 
		private void rdoLog_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rdoLog.Checked)
			{
				nudLogMax.ReadOnly=false;
				nudLogUnit.ReadOnly=false;
			}
		}
		#endregion

		private void chkTot_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkTot.Checked)
			{
				chkTot5.Checked=false;
				chkDR5.Checked=false;
			}
		}

		private void chkDR_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkDR.Checked)
			{
				chkTot5.Checked=false;
				chkDR5.Checked=false;
			}
		}

		private void chkTot5_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkTot5.Checked)
			{
				chkTot.Checked=false;
				chkDR.Checked=false;
			}
		
		}

		private void chkDR5_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkDR5.Checked)
			{
				chkTot.Checked=false;
				chkDR.Checked=false;
			}
		}

		private void btnUp_Click(object sender, System.EventArgs e)
		{
			int iSelectedRow = 0;
			iSelectedRow = cdgDefault.RowSel;

			
			if(iSelectedRow == 0 || iSelectedRow == 1)return;
			cdgDefault.Rows[iSelectedRow].Selected = false;
			if(iSelectedRow != 0 && iSelectedRow != 1)
			{
	
				cdgDefault.Rows[iSelectedRow].Move(iSelectedRow-1);
				cdgDefault.Refresh();
				cdgDefault.RowSel = iSelectedRow-1;
				cdgDefault.Refresh();
				for(int i=1;i<cdgDefault.Rows.Count;i++)
				{
					cdgDefault.Rows[i].Selected = false;				
				}
				cdgDefault.Rows[iSelectedRow-1].Selected = true;
				cdgDefault.Refresh();
			}		
		}

		private void btnDown_Click(object sender, System.EventArgs e)
		{
			
			int iSelectedRow = 0;
			iSelectedRow = cdgDefault.RowSel;
			if(iSelectedRow == cdgDefault.Rows.Count-1)return;

			cdgDefault.Rows[iSelectedRow].Selected = false;
			
			if(iSelectedRow != 0 && iSelectedRow != cdgDefault.Rows.Count)
			{
	
				cdgDefault.Rows[iSelectedRow].Move(iSelectedRow+1);
				cdgDefault.Refresh();
				cdgDefault.RowSel = iSelectedRow+1;
				cdgDefault.Refresh();
				for(int i=1;i<cdgDefault.Rows.Count;i++)
				{
					cdgDefault.Rows[i].Selected = false;				
				}
				cdgDefault.Rows[iSelectedRow+1].Selected = true;
				cdgDefault.Refresh();
			}	
		}
	
	}
}

