using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Miracom.DMS.Win;
using Excel = Microsoft.Office.Interop.Excel; 

namespace Miracom.DMS.LIB.Report
{
	/// <Summary>
	/// <b>■ Defect Report </b><br>
	/// - 작  성  자 : 미라콤 임영신<br>
	/// - 최초작성일 : 2005년 02월 1일<br>
	/// - 최종수정자 : <br>
	/// - 최종수정일 : <br>
	/// - 주요변경로그<br>
	///   2005년 02월 1일 생성<br>
	/// </Summary>
	/// <Remarks>없음</Remarks>
	public class CrossOne : System.Windows.Forms.UserControl
	{
		#region  ★ 시스템 변수
		private System.Windows.Forms.Panel pnlDataRight;
		private System.Windows.Forms.Panel pnlDataCenter;
		private System.Windows.Forms.Panel pnlDataCenterTop;
		private System.Windows.Forms.Splitter splDataCenterTopCenter;
		private System.Windows.Forms.Panel pnlDataCenterCenter;
		private System.Windows.Forms.Panel pnlLeftBottomTop;
		private System.Windows.Forms.ListView lvEQList;
		private System.Windows.Forms.Label lblInspectionEQ;
		private System.Windows.Forms.Button btnI_EQ_Apply;
		private System.Windows.Forms.Panel pnlLeftBottomTopCenter;
		private System.Windows.Forms.Button btnI_EQ_Save;
		private System.Windows.Forms.TabControl tabChart;
		private System.Windows.Forms.Button btnSheetDefectSelector;
		private System.Windows.Forms.Button btnChartSelector;
		private System.Windows.Forms.Panel pnlLeftBottomTopTop;
		private System.Windows.Forms.Panel pnlLeftBottomTopTop2;
		private System.Windows.Forms.Panel pnlLeftBottomTopTop3;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnStepRefresh;
		private System.Windows.Forms.Label lblProcessFlow;
		private System.Windows.Forms.Button btnEQLSelectNone;
		private System.Windows.Forms.Button btnEQLSelectAll;
		private System.Windows.Forms.Button btnSelectData;
		private System.Windows.Forms.Label lblStep;
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Panel pnlCenter;
		private System.Windows.Forms.Panel pnlLeft;
		private System.Windows.Forms.Splitter splLeftInCenter;
		private System.Windows.Forms.Panel pnlInCenter;
		private System.Windows.Forms.Panel pnlLeftTop;
		private System.Windows.Forms.Splitter splLeftTopCenter;
		private System.Windows.Forms.Panel pnlLeftBottom;
		private System.Windows.Forms.Splitter splLeftCenterBottom;
		private System.Windows.Forms.Panel pnlLeftCenter;
		private System.Windows.Forms.Panel pnlLeftTopTop;
		private System.Windows.Forms.Panel pnlLeftCenterTop;
		private FarPoint.Win.Spread.FpSpread fpsCrossOne;
		private System.Windows.Forms.Label lblProd;
		private System.Windows.Forms.Label lblRoute;
		private System.Windows.Forms.TextBox txtProd;
		private System.Windows.Forms.TextBox txtRoute;
		private System.Windows.Forms.Label lblOper;
		private System.Windows.Forms.Label lblStepID;
		private System.Windows.Forms.TextBox txtOper;
		private System.Windows.Forms.TextBox txtStepID;
		private System.Windows.Forms.CheckBox chkInspectedOnly;
		private System.Windows.Forms.CheckBox chkClassifiedOnly;
		private System.Windows.Forms.ComboBox cboSampleCls;
		private System.Windows.Forms.Label lblSampleCls;
		private System.Windows.Forms.ComboBox cboWafer;
		private System.Windows.Forms.Label lblWafer;
		private System.Windows.Forms.TreeView tvProcessFlow;
		private System.Windows.Forms.Button btnProcessFlowRefresh;
		private System.Windows.Forms.ImageList imgList;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TreeView tvInspectionStep;
		private System.Windows.Forms.Button btnAddSheet;
		private System.Windows.Forms.Button btnRemoveSheet;
		#endregion

		#region  ★ 사용자 정의 멤버 변수
		string sFAB = string.Empty;
		string[] arrUserConfiguredDynamicField = null;
		string[] arrUserConfiguredDynamicChartItem = null;
		ArrayList arrFilteredEQList = null;
		public string strUserID=String.Empty;
		public bool bInspectedOnlyChecked;
		public SheetDataCollection oSheetDataCollection;

		public string[] arrHeaderText;
		int[] arrHeaderIndex;
		
		public DataSet dsDynamicField = null;
		public DataSet dsDefectConfig = null;

		public bool bChartDynamicFieldSelector = false;
		public bool bSheetDynamicFieldSelector = false;
		private DynamicFieldSelector oChartDynamicFieldSelector = null;
		private DynamicFieldSelector oSheetDynamicFieldSelector = null;

		#endregion

		string[] strDeviceList = new String[11];

		#region  ★ 사용자 정의 상수
		private const int CST_FPS_ROWIDX_HEADER				= 0;
		private const int CST_FPS_ROWIDX_GRANDTOTAL			= 1;
		private const int CST_FPS_ROWBOF_SUMMARY			= 2;
		private const int CST_FPS_DECIMALPOINT_1			= 1;
		private const int CST_FPS_DECIMALPOINT_2			= 2;
		private const int CST_FPS_STATIC_FIELD_COUNT		= 13;
		
		// STATIC SHEET FIELD
		private const int CST_FPS_COLIDX_EQ_ID				= 0;
		private const int CST_FPS_COLIDX_EVENT_STARTDT		= 1;
		private const int CST_FPS_COLIDX_JOB				= 2;
		private const int CST_FPS_COLIDX_LOT_ID				= 3;
		private const int CST_FPS_COLIDX_WF					= 4;
		private const int CST_FPS_COLIDX_I_EQUIP			= 5;
		private const int CST_FPS_COLIDX_I_DATE				= 6;
		private const int CST_FPS_COLIDX_GY					= 7;
		private const int CST_FPS_COLIDX_TOT				= 8;
		private const int CST_FPS_COLIDX_DI					= 9;
		private const int CST_FPS_COLIDX_DR					= 10;
		private const int CST_FPS_COLIDX_DENSITY			= 11;
		private const int CST_FPS_COLIDX_NORV				= 12;
		// END STATIC SHEET FIELD

		// DYNAMIC SHEET FIELD
		private const int CST_FPS_COLIDX_SPOT				= 13;
		private const int CST_FPS_COLIDX_SBPT				= 14;
		private const int CST_FPS_COLIDX_BRGE				= 15;
		private const int CST_FPS_COLIDX_LSAC				= 16;
		private const int CST_FPS_COLIDX_BFBG				= 17;
		private const int CST_FPS_COLIDX_NOGR				= 18;
		private const int CST_FPS_COLIDX_DFNC				= 19;
		private const int CST_FPS_COLIDX_PLTH				= 20;
		private const int CST_FPS_COLIDX_WSPT				= 21;
		private const int CST_FPS_COLIDX_DISC				= 22;
		private const int CST_FPS_COLIDX_SFLD				= 23;
		private const int CST_FPS_COLIDX_SCRA				= 24;
		private const int CST_FPS_COLIDX_CMSC				= 25;
		private const int CST_FPS_COLIDX_WHIS				= 26;
		private const int CST_FPS_COLIDX_BUNC				= 27;
		private const int CST_FPS_COLIDX_VOID				= 28;
		private const int CST_FPS_COLIDX_BBGG				= 29;
		private const int CST_FPS_COLIDX_VOLC				= 30;
		private const int CST_FPS_COLIDX_ARCI				= 31;
		private const int CST_FPS_COLIDX_RDBG				= 32;
		private const int CST_FPS_COLIDX_USR1				= 33;
		private const int CST_FPS_COLIDX_BOMB				= 34;
		private const int CST_FPS_COLIDX_NOIS				= 35;
		// END DYNAMIC SHEET FIELD

		private const string CST_STRING_SPACE = " ";
		private const string CST_STRING_UNDERBAR = "_";
		private const string CST_STRING_EMPTY = "";
		private const string CST_STRING_COMMA_SEPARATOR = ",";
		private System.Windows.Forms.ContextMenu ctxSpread;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private FarPoint.Win.Spread.SheetView fpsCrossOne_Sheet1;
		private const string CST_STRING_NULL = "";

		#endregion

		#region  ★ 클래스 생성자,디스포저
		public CrossOne()
		{
			try
			{
				InitializeComponent();
				//테스트 코드 : 배포시 삭제
//				this.strUserID = "SYSTEM";


//				SetInitialize();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.

			// TODO: InitComponent를 호출한 다음 초기화 작업을 추가합니다.
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
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
		
		#region ★ 구성 요소 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CrossOne));
			this.pnlTop = new System.Windows.Forms.Panel();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnChartSelector = new System.Windows.Forms.Button();
			this.btnSheetDefectSelector = new System.Windows.Forms.Button();
			this.btnRemoveSheet = new System.Windows.Forms.Button();
			this.btnAddSheet = new System.Windows.Forms.Button();
			this.btnSelectData = new System.Windows.Forms.Button();
			this.lblWafer = new System.Windows.Forms.Label();
			this.cboWafer = new System.Windows.Forms.ComboBox();
			this.lblSampleCls = new System.Windows.Forms.Label();
			this.cboSampleCls = new System.Windows.Forms.ComboBox();
			this.chkClassifiedOnly = new System.Windows.Forms.CheckBox();
			this.chkInspectedOnly = new System.Windows.Forms.CheckBox();
			this.txtStepID = new System.Windows.Forms.TextBox();
			this.txtOper = new System.Windows.Forms.TextBox();
			this.lblStepID = new System.Windows.Forms.Label();
			this.lblOper = new System.Windows.Forms.Label();
			this.txtRoute = new System.Windows.Forms.TextBox();
			this.txtProd = new System.Windows.Forms.TextBox();
			this.lblRoute = new System.Windows.Forms.Label();
			this.lblProd = new System.Windows.Forms.Label();
			this.dtpToDate = new System.Windows.Forms.DateTimePicker();
			this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
			this.lblTo = new System.Windows.Forms.Label();
			this.lblFrom = new System.Windows.Forms.Label();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.pnlCenter = new System.Windows.Forms.Panel();
			this.pnlInCenter = new System.Windows.Forms.Panel();
			this.pnlDataCenter = new System.Windows.Forms.Panel();
			this.tabChart = new System.Windows.Forms.TabControl();
			this.splDataCenterTopCenter = new System.Windows.Forms.Splitter();
			this.pnlDataCenterTop = new System.Windows.Forms.Panel();
			this.fpsCrossOne = new FarPoint.Win.Spread.FpSpread();
			this.ctxSpread = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.fpsCrossOne_Sheet1 = new FarPoint.Win.Spread.SheetView();
			this.pnlDataRight = new System.Windows.Forms.Panel();
			this.pnlDataCenterCenter = new System.Windows.Forms.Panel();
			this.splLeftInCenter = new System.Windows.Forms.Splitter();
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.pnlLeftCenter = new System.Windows.Forms.Panel();
			this.tvInspectionStep = new System.Windows.Forms.TreeView();
			this.pnlLeftCenterTop = new System.Windows.Forms.Panel();
			this.lblStep = new System.Windows.Forms.Label();
			this.btnStepRefresh = new System.Windows.Forms.Button();
			this.splLeftCenterBottom = new System.Windows.Forms.Splitter();
			this.pnlLeftBottom = new System.Windows.Forms.Panel();
			this.pnlLeftBottomTop = new System.Windows.Forms.Panel();
			this.pnlLeftBottomTopTop3 = new System.Windows.Forms.Panel();
			this.lvEQList = new System.Windows.Forms.ListView();
			this.pnlLeftBottomTopTop2 = new System.Windows.Forms.Panel();
			this.btnEQLSelectAll = new System.Windows.Forms.Button();
			this.btnI_EQ_Save = new System.Windows.Forms.Button();
			this.btnI_EQ_Apply = new System.Windows.Forms.Button();
			this.btnEQLSelectNone = new System.Windows.Forms.Button();
			this.pnlLeftBottomTopTop = new System.Windows.Forms.Panel();
			this.lblInspectionEQ = new System.Windows.Forms.Label();
			this.splLeftTopCenter = new System.Windows.Forms.Splitter();
			this.pnlLeftTop = new System.Windows.Forms.Panel();
			this.tvProcessFlow = new System.Windows.Forms.TreeView();
			this.pnlLeftTopTop = new System.Windows.Forms.Panel();
			this.lblProcessFlow = new System.Windows.Forms.Label();
			this.btnProcessFlowRefresh = new System.Windows.Forms.Button();
			this.pnlLeftBottomTopCenter = new System.Windows.Forms.Panel();
			this.imgList = new System.Windows.Forms.ImageList(this.components);
			this.pnlTop.SuspendLayout();
			this.pnlCenter.SuspendLayout();
			this.pnlInCenter.SuspendLayout();
			this.pnlDataCenter.SuspendLayout();
			this.pnlDataCenterTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fpsCrossOne)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpsCrossOne_Sheet1)).BeginInit();
			this.pnlLeft.SuspendLayout();
			this.pnlLeftCenter.SuspendLayout();
			this.pnlLeftCenterTop.SuspendLayout();
			this.pnlLeftBottom.SuspendLayout();
			this.pnlLeftBottomTop.SuspendLayout();
			this.pnlLeftBottomTopTop3.SuspendLayout();
			this.pnlLeftBottomTopTop2.SuspendLayout();
			this.pnlLeftBottomTopTop.SuspendLayout();
			this.pnlLeftTop.SuspendLayout();
			this.pnlLeftTopTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlTop
			// 
			this.pnlTop.BackColor = System.Drawing.SystemColors.Window;
			this.pnlTop.Controls.Add(this.btnExport);
			this.pnlTop.Controls.Add(this.btnChartSelector);
			this.pnlTop.Controls.Add(this.btnSheetDefectSelector);
			this.pnlTop.Controls.Add(this.btnRemoveSheet);
			this.pnlTop.Controls.Add(this.btnAddSheet);
			this.pnlTop.Controls.Add(this.btnSelectData);
			this.pnlTop.Controls.Add(this.lblWafer);
			this.pnlTop.Controls.Add(this.cboWafer);
			this.pnlTop.Controls.Add(this.lblSampleCls);
			this.pnlTop.Controls.Add(this.cboSampleCls);
			this.pnlTop.Controls.Add(this.chkClassifiedOnly);
			this.pnlTop.Controls.Add(this.chkInspectedOnly);
			this.pnlTop.Controls.Add(this.txtStepID);
			this.pnlTop.Controls.Add(this.txtOper);
			this.pnlTop.Controls.Add(this.lblStepID);
			this.pnlTop.Controls.Add(this.lblOper);
			this.pnlTop.Controls.Add(this.txtRoute);
			this.pnlTop.Controls.Add(this.txtProd);
			this.pnlTop.Controls.Add(this.lblRoute);
			this.pnlTop.Controls.Add(this.lblProd);
			this.pnlTop.Controls.Add(this.dtpToDate);
			this.pnlTop.Controls.Add(this.dtpFromDate);
			this.pnlTop.Controls.Add(this.lblTo);
			this.pnlTop.Controls.Add(this.lblFrom);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(1000, 104);
			this.pnlTop.TabIndex = 0;
			// 
			// btnExport
			// 
			this.btnExport.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnExport.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnExport.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnExport.Location = new System.Drawing.Point(608, 72);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(72, 24);
			this.btnExport.TabIndex = 45;
			this.btnExport.Text = "To Excel";
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnChartSelector
			// 
			this.btnChartSelector.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnChartSelector.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnChartSelector.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnChartSelector.Location = new System.Drawing.Point(504, 72);
			this.btnChartSelector.Name = "btnChartSelector";
			this.btnChartSelector.Size = new System.Drawing.Size(96, 24);
			this.btnChartSelector.TabIndex = 44;
			this.btnChartSelector.Text = "차트 항목구성";
			this.btnChartSelector.Click += new System.EventHandler(this.btnChartSelector_Click);
			// 
			// btnSheetDefectSelector
			// 
			this.btnSheetDefectSelector.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnSheetDefectSelector.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnSheetDefectSelector.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnSheetDefectSelector.Location = new System.Drawing.Point(392, 72);
			this.btnSheetDefectSelector.Name = "btnSheetDefectSelector";
			this.btnSheetDefectSelector.Size = new System.Drawing.Size(104, 24);
			this.btnSheetDefectSelector.TabIndex = 43;
			this.btnSheetDefectSelector.Text = "그리드 필드구성";
			this.btnSheetDefectSelector.Click += new System.EventHandler(this.btnDynamicSelector_Click);
			// 
			// btnRemoveSheet
			// 
			this.btnRemoveSheet.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnRemoveSheet.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnRemoveSheet.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnRemoveSheet.Location = new System.Drawing.Point(312, 72);
			this.btnRemoveSheet.Name = "btnRemoveSheet";
			this.btnRemoveSheet.Size = new System.Drawing.Size(72, 24);
			this.btnRemoveSheet.TabIndex = 42;
			this.btnRemoveSheet.Text = "시트삭제";
			this.btnRemoveSheet.Click += new System.EventHandler(this.btnRemoveSheet_Click);
			// 
			// btnAddSheet
			// 
			this.btnAddSheet.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnAddSheet.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnAddSheet.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnAddSheet.Location = new System.Drawing.Point(232, 72);
			this.btnAddSheet.Name = "btnAddSheet";
			this.btnAddSheet.Size = new System.Drawing.Size(72, 24);
			this.btnAddSheet.TabIndex = 41;
			this.btnAddSheet.Text = "시트 추가";
			this.btnAddSheet.Click += new System.EventHandler(this.btnAddSheet_Click);
			// 
			// btnSelectData
			// 
			this.btnSelectData.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnSelectData.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnSelectData.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnSelectData.Location = new System.Drawing.Point(904, 72);
			this.btnSelectData.Name = "btnSelectData";
			this.btnSelectData.Size = new System.Drawing.Size(80, 24);
			this.btnSelectData.TabIndex = 40;
			this.btnSelectData.Text = "조회";
			this.btnSelectData.Click += new System.EventHandler(this.btnSelectData_Click);
			// 
			// lblWafer
			// 
			this.lblWafer.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.lblWafer.Location = new System.Drawing.Point(824, 32);
			this.lblWafer.Name = "lblWafer";
			this.lblWafer.Size = new System.Drawing.Size(64, 16);
			this.lblWafer.TabIndex = 38;
			this.lblWafer.Text = "Wafer";
			this.lblWafer.Visible = false;
			// 
			// cboWafer
			// 
			this.cboWafer.Items.AddRange(new object[] {
														  "ALL"});
			this.cboWafer.Location = new System.Drawing.Point(760, 32);
			this.cboWafer.Name = "cboWafer";
			this.cboWafer.Size = new System.Drawing.Size(56, 20);
			this.cboWafer.TabIndex = 37;
			this.cboWafer.Text = "ALL";
			this.cboWafer.Visible = false;
			// 
			// lblSampleCls
			// 
			this.lblSampleCls.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.lblSampleCls.Location = new System.Drawing.Point(824, 8);
			this.lblSampleCls.Name = "lblSampleCls";
			this.lblSampleCls.Size = new System.Drawing.Size(80, 24);
			this.lblSampleCls.TabIndex = 36;
			this.lblSampleCls.Text = "Sample CLS";
			this.lblSampleCls.Visible = false;
			// 
			// cboSampleCls
			// 
			this.cboSampleCls.Items.AddRange(new object[] {
															  "ALL"});
			this.cboSampleCls.Location = new System.Drawing.Point(760, 8);
			this.cboSampleCls.Name = "cboSampleCls";
			this.cboSampleCls.Size = new System.Drawing.Size(56, 20);
			this.cboSampleCls.TabIndex = 35;
			this.cboSampleCls.Text = "ALL";
			this.cboSampleCls.Visible = false;
			// 
			// chkClassifiedOnly
			// 
			this.chkClassifiedOnly.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.chkClassifiedOnly.Location = new System.Drawing.Point(640, 32);
			this.chkClassifiedOnly.Name = "chkClassifiedOnly";
			this.chkClassifiedOnly.Size = new System.Drawing.Size(112, 16);
			this.chkClassifiedOnly.TabIndex = 34;
			this.chkClassifiedOnly.Text = "Classified Only";
			this.chkClassifiedOnly.CheckedChanged += new System.EventHandler(this.chkClassifiedOnly_CheckedChanged);
			// 
			// chkInspectedOnly
			// 
			this.chkInspectedOnly.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.chkInspectedOnly.Location = new System.Drawing.Point(640, 8);
			this.chkInspectedOnly.Name = "chkInspectedOnly";
			this.chkInspectedOnly.Size = new System.Drawing.Size(112, 16);
			this.chkInspectedOnly.TabIndex = 33;
			this.chkInspectedOnly.Text = "Inspected Only";
			this.chkInspectedOnly.CheckedChanged += new System.EventHandler(this.chkInspectedOnly_CheckedChanged);
			// 
			// txtStepID
			// 
			this.txtStepID.BackColor = System.Drawing.Color.Green;
			this.txtStepID.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtStepID.ForeColor = System.Drawing.Color.Yellow;
			this.txtStepID.Location = new System.Drawing.Point(488, 32);
			this.txtStepID.Name = "txtStepID";
			this.txtStepID.Size = new System.Drawing.Size(144, 21);
			this.txtStepID.TabIndex = 32;
			this.txtStepID.Text = "";
			// 
			// txtOper
			// 
			this.txtOper.BackColor = System.Drawing.Color.Green;
			this.txtOper.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtOper.ForeColor = System.Drawing.Color.Yellow;
			this.txtOper.Location = new System.Drawing.Point(488, 8);
			this.txtOper.Name = "txtOper";
			this.txtOper.Size = new System.Drawing.Size(144, 21);
			this.txtOper.TabIndex = 31;
			this.txtOper.Text = "";
			// 
			// lblStepID
			// 
			this.lblStepID.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.lblStepID.Location = new System.Drawing.Point(424, 32);
			this.lblStepID.Name = "lblStepID";
			this.lblStepID.Size = new System.Drawing.Size(56, 24);
			this.lblStepID.TabIndex = 30;
			this.lblStepID.Text = "Step ID :";
			this.lblStepID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblOper
			// 
			this.lblOper.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.lblOper.Location = new System.Drawing.Point(424, 8);
			this.lblOper.Name = "lblOper";
			this.lblOper.Size = new System.Drawing.Size(56, 24);
			this.lblOper.TabIndex = 29;
			this.lblOper.Text = "Oper :";
			this.lblOper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtRoute
			// 
			this.txtRoute.BackColor = System.Drawing.Color.Green;
			this.txtRoute.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtRoute.ForeColor = System.Drawing.Color.Yellow;
			this.txtRoute.Location = new System.Drawing.Point(272, 32);
			this.txtRoute.Name = "txtRoute";
			this.txtRoute.Size = new System.Drawing.Size(144, 21);
			this.txtRoute.TabIndex = 28;
			this.txtRoute.Text = "";
			// 
			// txtProd
			// 
			this.txtProd.BackColor = System.Drawing.Color.Green;
			this.txtProd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtProd.ForeColor = System.Drawing.Color.Yellow;
			this.txtProd.Location = new System.Drawing.Point(272, 8);
			this.txtProd.Name = "txtProd";
			this.txtProd.Size = new System.Drawing.Size(144, 21);
			this.txtProd.TabIndex = 27;
			this.txtProd.Text = "";
			// 
			// lblRoute
			// 
			this.lblRoute.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.lblRoute.Location = new System.Drawing.Point(216, 32);
			this.lblRoute.Name = "lblRoute";
			this.lblRoute.Size = new System.Drawing.Size(48, 24);
			this.lblRoute.TabIndex = 26;
			this.lblRoute.Text = "Route :";
			this.lblRoute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblProd
			// 
			this.lblProd.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.lblProd.Location = new System.Drawing.Point(216, 8);
			this.lblProd.Name = "lblProd";
			this.lblProd.Size = new System.Drawing.Size(48, 16);
			this.lblProd.TabIndex = 25;
			this.lblProd.Text = "Prod :";
			this.lblProd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dtpToDate
			// 
			this.dtpToDate.CustomFormat = "";
			this.dtpToDate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.dtpToDate.Location = new System.Drawing.Point(56, 32);
			this.dtpToDate.Name = "dtpToDate";
			this.dtpToDate.Size = new System.Drawing.Size(152, 21);
			this.dtpToDate.TabIndex = 24;
			this.dtpToDate.Value = new System.DateTime(2005, 2, 15, 0, 0, 0, 0);
			// 
			// dtpFromDate
			// 
			this.dtpFromDate.CustomFormat = "";
			this.dtpFromDate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.dtpFromDate.Location = new System.Drawing.Point(56, 8);
			this.dtpFromDate.Name = "dtpFromDate";
			this.dtpFromDate.Size = new System.Drawing.Size(152, 21);
			this.dtpFromDate.TabIndex = 23;
			this.dtpFromDate.Value = new System.DateTime(2005, 1, 4, 0, 0, 0, 0);
			// 
			// lblTo
			// 
			this.lblTo.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.lblTo.Location = new System.Drawing.Point(16, 32);
			this.lblTo.Name = "lblTo";
			this.lblTo.Size = new System.Drawing.Size(32, 24);
			this.lblTo.TabIndex = 1;
			this.lblTo.Text = "To :";
			this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblFrom
			// 
			this.lblFrom.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
			this.lblFrom.Location = new System.Drawing.Point(0, 8);
			this.lblFrom.Name = "lblFrom";
			this.lblFrom.Size = new System.Drawing.Size(48, 24);
			this.lblFrom.TabIndex = 0;
			this.lblFrom.Text = "From :";
			this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pnlBottom
			// 
			this.pnlBottom.BackColor = System.Drawing.SystemColors.Window;
			this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBottom.Location = new System.Drawing.Point(0, 853);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(1000, 24);
			this.pnlBottom.TabIndex = 4;
			// 
			// pnlCenter
			// 
			this.pnlCenter.Controls.Add(this.pnlInCenter);
			this.pnlCenter.Controls.Add(this.splLeftInCenter);
			this.pnlCenter.Controls.Add(this.pnlLeft);
			this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlCenter.Location = new System.Drawing.Point(0, 104);
			this.pnlCenter.Name = "pnlCenter";
			this.pnlCenter.Size = new System.Drawing.Size(1000, 749);
			this.pnlCenter.TabIndex = 5;
			// 
			// pnlInCenter
			// 
			this.pnlInCenter.Controls.Add(this.pnlDataCenter);
			this.pnlInCenter.Controls.Add(this.pnlDataRight);
			this.pnlInCenter.Controls.Add(this.pnlDataCenterCenter);
			this.pnlInCenter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlInCenter.Location = new System.Drawing.Point(232, 0);
			this.pnlInCenter.Name = "pnlInCenter";
			this.pnlInCenter.Size = new System.Drawing.Size(768, 749);
			this.pnlInCenter.TabIndex = 2;
			// 
			// pnlDataCenter
			// 
			this.pnlDataCenter.Controls.Add(this.tabChart);
			this.pnlDataCenter.Controls.Add(this.splDataCenterTopCenter);
			this.pnlDataCenter.Controls.Add(this.pnlDataCenterTop);
			this.pnlDataCenter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlDataCenter.Location = new System.Drawing.Point(0, 0);
			this.pnlDataCenter.Name = "pnlDataCenter";
			this.pnlDataCenter.Size = new System.Drawing.Size(760, 749);
			this.pnlDataCenter.TabIndex = 2;
			// 
			// tabChart
			// 
			this.tabChart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabChart.Location = new System.Drawing.Point(0, 328);
			this.tabChart.Name = "tabChart";
			this.tabChart.SelectedIndex = 0;
			this.tabChart.Size = new System.Drawing.Size(760, 421);
			this.tabChart.TabIndex = 2;
			this.tabChart.SelectedIndexChanged += new System.EventHandler(this.tabChart_SelectedIndexChanged);
			// 
			// splDataCenterTopCenter
			// 
			this.splDataCenterTopCenter.BackColor = System.Drawing.SystemColors.Window;
			this.splDataCenterTopCenter.Dock = System.Windows.Forms.DockStyle.Top;
			this.splDataCenterTopCenter.Location = new System.Drawing.Point(0, 320);
			this.splDataCenterTopCenter.Name = "splDataCenterTopCenter";
			this.splDataCenterTopCenter.Size = new System.Drawing.Size(760, 8);
			this.splDataCenterTopCenter.TabIndex = 1;
			this.splDataCenterTopCenter.TabStop = false;
			// 
			// pnlDataCenterTop
			// 
			this.pnlDataCenterTop.Controls.Add(this.fpsCrossOne);
			this.pnlDataCenterTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDataCenterTop.Location = new System.Drawing.Point(0, 0);
			this.pnlDataCenterTop.Name = "pnlDataCenterTop";
			this.pnlDataCenterTop.Size = new System.Drawing.Size(760, 320);
			this.pnlDataCenterTop.TabIndex = 0;
			// 
			// fpsCrossOne
			// 
			this.fpsCrossOne.AllowUserZoom = false;
			this.fpsCrossOne.ContextMenu = this.ctxSpread;
			this.fpsCrossOne.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fpsCrossOne.Location = new System.Drawing.Point(0, 0);
			this.fpsCrossOne.Name = "fpsCrossOne";
			this.fpsCrossOne.Sheets.Add(this.fpsCrossOne_Sheet1);
			this.fpsCrossOne.Size = new System.Drawing.Size(760, 320);
			this.fpsCrossOne.TabIndex = 0;
			this.fpsCrossOne.ActiveSheetChanged += new System.EventHandler(this.fpsCrossOne_ActiveSheetChanged);
			// 
			// ctxSpread
			// 
			this.ctxSpread.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "복사";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "MapView";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// fpsCrossOne_Sheet1
			// 
			this.fpsCrossOne_Sheet1.SheetName = "Sheet1";
			// 
			// pnlDataRight
			// 
			this.pnlDataRight.BackColor = System.Drawing.SystemColors.Window;
			this.pnlDataRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlDataRight.Location = new System.Drawing.Point(760, 0);
			this.pnlDataRight.Name = "pnlDataRight";
			this.pnlDataRight.Size = new System.Drawing.Size(8, 749);
			this.pnlDataRight.TabIndex = 1;
			// 
			// pnlDataCenterCenter
			// 
			this.pnlDataCenterCenter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlDataCenterCenter.Location = new System.Drawing.Point(0, 0);
			this.pnlDataCenterCenter.Name = "pnlDataCenterCenter";
			this.pnlDataCenterCenter.Size = new System.Drawing.Size(768, 749);
			this.pnlDataCenterCenter.TabIndex = 2;
			// 
			// splLeftInCenter
			// 
			this.splLeftInCenter.BackColor = System.Drawing.SystemColors.Window;
			this.splLeftInCenter.Location = new System.Drawing.Point(224, 0);
			this.splLeftInCenter.Name = "splLeftInCenter";
			this.splLeftInCenter.Size = new System.Drawing.Size(8, 749);
			this.splLeftInCenter.TabIndex = 1;
			this.splLeftInCenter.TabStop = false;
			// 
			// pnlLeft
			// 
			this.pnlLeft.Controls.Add(this.pnlLeftCenter);
			this.pnlLeft.Controls.Add(this.splLeftCenterBottom);
			this.pnlLeft.Controls.Add(this.pnlLeftBottom);
			this.pnlLeft.Controls.Add(this.splLeftTopCenter);
			this.pnlLeft.Controls.Add(this.pnlLeftTop);
			this.pnlLeft.Controls.Add(this.pnlLeftBottomTopCenter);
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeft.Location = new System.Drawing.Point(0, 0);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Size = new System.Drawing.Size(224, 749);
			this.pnlLeft.TabIndex = 0;
			// 
			// pnlLeftCenter
			// 
			this.pnlLeftCenter.Controls.Add(this.tvInspectionStep);
			this.pnlLeftCenter.Controls.Add(this.pnlLeftCenterTop);
			this.pnlLeftCenter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLeftCenter.Location = new System.Drawing.Point(0, 240);
			this.pnlLeftCenter.Name = "pnlLeftCenter";
			this.pnlLeftCenter.Size = new System.Drawing.Size(224, 229);
			this.pnlLeftCenter.TabIndex = 4;
			// 
			// tvInspectionStep
			// 
			this.tvInspectionStep.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvInspectionStep.ImageIndex = -1;
			this.tvInspectionStep.Location = new System.Drawing.Point(0, 24);
			this.tvInspectionStep.Name = "tvInspectionStep";
			this.tvInspectionStep.SelectedImageIndex = -1;
			this.tvInspectionStep.Size = new System.Drawing.Size(224, 205);
			this.tvInspectionStep.TabIndex = 1;
			this.tvInspectionStep.DoubleClick += new System.EventHandler(this.tvInspectionStep_DoubleClick);
			// 
			// pnlLeftCenterTop
			// 
			this.pnlLeftCenterTop.BackColor = System.Drawing.SystemColors.Window;
			this.pnlLeftCenterTop.Controls.Add(this.lblStep);
			this.pnlLeftCenterTop.Controls.Add(this.btnStepRefresh);
			this.pnlLeftCenterTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLeftCenterTop.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftCenterTop.Name = "pnlLeftCenterTop";
			this.pnlLeftCenterTop.Size = new System.Drawing.Size(224, 24);
			this.pnlLeftCenterTop.TabIndex = 0;
			// 
			// lblStep
			// 
			this.lblStep.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblStep.Location = new System.Drawing.Point(16, 8);
			this.lblStep.Name = "lblStep";
			this.lblStep.Size = new System.Drawing.Size(112, 16);
			this.lblStep.TabIndex = 3;
			this.lblStep.Text = "Step";
			// 
			// btnStepRefresh
			// 
			this.btnStepRefresh.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnStepRefresh.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnStepRefresh.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnStepRefresh.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnStepRefresh.Location = new System.Drawing.Point(152, 0);
			this.btnStepRefresh.Name = "btnStepRefresh";
			this.btnStepRefresh.Size = new System.Drawing.Size(72, 24);
			this.btnStepRefresh.TabIndex = 2;
			this.btnStepRefresh.Text = "Refresh";
			this.btnStepRefresh.Click += new System.EventHandler(this.btnStepRefresh_Click);
			// 
			// splLeftCenterBottom
			// 
			this.splLeftCenterBottom.BackColor = System.Drawing.SystemColors.Window;
			this.splLeftCenterBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splLeftCenterBottom.Location = new System.Drawing.Point(0, 469);
			this.splLeftCenterBottom.Name = "splLeftCenterBottom";
			this.splLeftCenterBottom.Size = new System.Drawing.Size(224, 8);
			this.splLeftCenterBottom.TabIndex = 3;
			this.splLeftCenterBottom.TabStop = false;
			// 
			// pnlLeftBottom
			// 
			this.pnlLeftBottom.Controls.Add(this.pnlLeftBottomTop);
			this.pnlLeftBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlLeftBottom.Location = new System.Drawing.Point(0, 477);
			this.pnlLeftBottom.Name = "pnlLeftBottom";
			this.pnlLeftBottom.Size = new System.Drawing.Size(224, 272);
			this.pnlLeftBottom.TabIndex = 2;
			// 
			// pnlLeftBottomTop
			// 
			this.pnlLeftBottomTop.Controls.Add(this.pnlLeftBottomTopTop3);
			this.pnlLeftBottomTop.Controls.Add(this.pnlLeftBottomTopTop2);
			this.pnlLeftBottomTop.Controls.Add(this.pnlLeftBottomTopTop);
			this.pnlLeftBottomTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLeftBottomTop.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftBottomTop.Name = "pnlLeftBottomTop";
			this.pnlLeftBottomTop.Size = new System.Drawing.Size(224, 272);
			this.pnlLeftBottomTop.TabIndex = 1;
			// 
			// pnlLeftBottomTopTop3
			// 
			this.pnlLeftBottomTopTop3.Controls.Add(this.lvEQList);
			this.pnlLeftBottomTopTop3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLeftBottomTopTop3.Location = new System.Drawing.Point(0, 48);
			this.pnlLeftBottomTopTop3.Name = "pnlLeftBottomTopTop3";
			this.pnlLeftBottomTopTop3.Size = new System.Drawing.Size(224, 224);
			this.pnlLeftBottomTopTop3.TabIndex = 2;
			// 
			// lvEQList
			// 
			this.lvEQList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvEQList.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lvEQList.Location = new System.Drawing.Point(0, 0);
			this.lvEQList.Name = "lvEQList";
			this.lvEQList.Size = new System.Drawing.Size(224, 224);
			this.lvEQList.TabIndex = 0;
			this.lvEQList.View = System.Windows.Forms.View.Details;
			// 
			// pnlLeftBottomTopTop2
			// 
			this.pnlLeftBottomTopTop2.BackColor = System.Drawing.SystemColors.Window;
			this.pnlLeftBottomTopTop2.Controls.Add(this.btnEQLSelectAll);
			this.pnlLeftBottomTopTop2.Controls.Add(this.btnI_EQ_Save);
			this.pnlLeftBottomTopTop2.Controls.Add(this.btnI_EQ_Apply);
			this.pnlLeftBottomTopTop2.Controls.Add(this.btnEQLSelectNone);
			this.pnlLeftBottomTopTop2.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLeftBottomTopTop2.Location = new System.Drawing.Point(0, 24);
			this.pnlLeftBottomTopTop2.Name = "pnlLeftBottomTopTop2";
			this.pnlLeftBottomTopTop2.Size = new System.Drawing.Size(224, 24);
			this.pnlLeftBottomTopTop2.TabIndex = 1;
			// 
			// btnEQLSelectAll
			// 
			this.btnEQLSelectAll.BackColor = System.Drawing.Color.LightPink;
			this.btnEQLSelectAll.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnEQLSelectAll.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnEQLSelectAll.Location = new System.Drawing.Point(144, 0);
			this.btnEQLSelectAll.Name = "btnEQLSelectAll";
			this.btnEQLSelectAll.Size = new System.Drawing.Size(40, 24);
			this.btnEQLSelectAll.TabIndex = 3;
			this.btnEQLSelectAll.Text = "All";
			this.btnEQLSelectAll.Click += new System.EventHandler(this.btnEQLSelectAll_Click);
			// 
			// btnI_EQ_Save
			// 
			this.btnI_EQ_Save.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnI_EQ_Save.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnI_EQ_Save.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnI_EQ_Save.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnI_EQ_Save.Location = new System.Drawing.Point(56, 0);
			this.btnI_EQ_Save.Name = "btnI_EQ_Save";
			this.btnI_EQ_Save.Size = new System.Drawing.Size(56, 24);
			this.btnI_EQ_Save.TabIndex = 1;
			this.btnI_EQ_Save.Text = "저장";
			this.btnI_EQ_Save.Click += new System.EventHandler(this.btnI_EQ_Save_Click);
			// 
			// btnI_EQ_Apply
			// 
			this.btnI_EQ_Apply.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnI_EQ_Apply.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnI_EQ_Apply.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnI_EQ_Apply.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnI_EQ_Apply.Location = new System.Drawing.Point(0, 0);
			this.btnI_EQ_Apply.Name = "btnI_EQ_Apply";
			this.btnI_EQ_Apply.Size = new System.Drawing.Size(56, 24);
			this.btnI_EQ_Apply.TabIndex = 0;
			this.btnI_EQ_Apply.Text = "적용";
			this.btnI_EQ_Apply.Click += new System.EventHandler(this.btnI_EQ_Apply_Click);
			// 
			// btnEQLSelectNone
			// 
			this.btnEQLSelectNone.BackColor = System.Drawing.Color.LightPink;
			this.btnEQLSelectNone.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnEQLSelectNone.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnEQLSelectNone.Location = new System.Drawing.Point(184, 0);
			this.btnEQLSelectNone.Name = "btnEQLSelectNone";
			this.btnEQLSelectNone.Size = new System.Drawing.Size(40, 24);
			this.btnEQLSelectNone.TabIndex = 2;
			this.btnEQLSelectNone.Text = "None";
			this.btnEQLSelectNone.Click += new System.EventHandler(this.btnEQLSelectNone_Click);
			// 
			// pnlLeftBottomTopTop
			// 
			this.pnlLeftBottomTopTop.BackColor = System.Drawing.SystemColors.Window;
			this.pnlLeftBottomTopTop.Controls.Add(this.lblInspectionEQ);
			this.pnlLeftBottomTopTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLeftBottomTopTop.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftBottomTopTop.Name = "pnlLeftBottomTopTop";
			this.pnlLeftBottomTopTop.Size = new System.Drawing.Size(224, 24);
			this.pnlLeftBottomTopTop.TabIndex = 0;
			// 
			// lblInspectionEQ
			// 
			this.lblInspectionEQ.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblInspectionEQ.Location = new System.Drawing.Point(8, 8);
			this.lblInspectionEQ.Name = "lblInspectionEQ";
			this.lblInspectionEQ.Size = new System.Drawing.Size(112, 16);
			this.lblInspectionEQ.TabIndex = 0;
			this.lblInspectionEQ.Text = "INSPECTION";
			// 
			// splLeftTopCenter
			// 
			this.splLeftTopCenter.BackColor = System.Drawing.SystemColors.Window;
			this.splLeftTopCenter.Dock = System.Windows.Forms.DockStyle.Top;
			this.splLeftTopCenter.Location = new System.Drawing.Point(0, 232);
			this.splLeftTopCenter.Name = "splLeftTopCenter";
			this.splLeftTopCenter.Size = new System.Drawing.Size(224, 8);
			this.splLeftTopCenter.TabIndex = 1;
			this.splLeftTopCenter.TabStop = false;
			// 
			// pnlLeftTop
			// 
			this.pnlLeftTop.Controls.Add(this.tvProcessFlow);
			this.pnlLeftTop.Controls.Add(this.pnlLeftTopTop);
			this.pnlLeftTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLeftTop.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftTop.Name = "pnlLeftTop";
			this.pnlLeftTop.Size = new System.Drawing.Size(224, 232);
			this.pnlLeftTop.TabIndex = 0;
			// 
			// tvProcessFlow
			// 
			this.tvProcessFlow.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvProcessFlow.ImageIndex = -1;
			this.tvProcessFlow.Location = new System.Drawing.Point(0, 24);
			this.tvProcessFlow.Name = "tvProcessFlow";
			this.tvProcessFlow.SelectedImageIndex = -1;
			this.tvProcessFlow.Size = new System.Drawing.Size(224, 208);
			this.tvProcessFlow.TabIndex = 1;
			this.tvProcessFlow.DoubleClick += new System.EventHandler(this.tvProcessFlow_DoubleClick);
			// 
			// pnlLeftTopTop
			// 
			this.pnlLeftTopTop.BackColor = System.Drawing.SystemColors.Window;
			this.pnlLeftTopTop.Controls.Add(this.lblProcessFlow);
			this.pnlLeftTopTop.Controls.Add(this.btnProcessFlowRefresh);
			this.pnlLeftTopTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLeftTopTop.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftTopTop.Name = "pnlLeftTopTop";
			this.pnlLeftTopTop.Size = new System.Drawing.Size(224, 24);
			this.pnlLeftTopTop.TabIndex = 0;
			// 
			// lblProcessFlow
			// 
			this.lblProcessFlow.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.lblProcessFlow.Location = new System.Drawing.Point(16, 8);
			this.lblProcessFlow.Name = "lblProcessFlow";
			this.lblProcessFlow.Size = new System.Drawing.Size(112, 16);
			this.lblProcessFlow.TabIndex = 1;
			this.lblProcessFlow.Text = "ProcessFlow";
			// 
			// btnProcessFlowRefresh
			// 
			this.btnProcessFlowRefresh.BackColor = System.Drawing.SystemColors.Desktop;
			this.btnProcessFlowRefresh.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnProcessFlowRefresh.Font = new System.Drawing.Font("돋움", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.btnProcessFlowRefresh.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnProcessFlowRefresh.Location = new System.Drawing.Point(152, 0);
			this.btnProcessFlowRefresh.Name = "btnProcessFlowRefresh";
			this.btnProcessFlowRefresh.Size = new System.Drawing.Size(72, 24);
			this.btnProcessFlowRefresh.TabIndex = 0;
			this.btnProcessFlowRefresh.Text = "Refresh";
			this.btnProcessFlowRefresh.Click += new System.EventHandler(this.btnProcessFlowRefresh_Click);
			// 
			// pnlLeftBottomTopCenter
			// 
			this.pnlLeftBottomTopCenter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLeftBottomTopCenter.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftBottomTopCenter.Name = "pnlLeftBottomTopCenter";
			this.pnlLeftBottomTopCenter.Size = new System.Drawing.Size(224, 749);
			this.pnlLeftBottomTopCenter.TabIndex = 1;
			// 
			// imgList
			// 
			this.imgList.ImageSize = new System.Drawing.Size(16, 16);
			this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
			this.imgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// CrossOne
			// 
			this.Controls.Add(this.pnlCenter);
			this.Controls.Add(this.pnlBottom);
			this.Controls.Add(this.pnlTop);
			this.Name = "CrossOne";
			this.Size = new System.Drawing.Size(1000, 877);
			this.Load += new System.EventHandler(this.CrossOne_Load);
			this.pnlTop.ResumeLayout(false);
			this.pnlCenter.ResumeLayout(false);
			this.pnlInCenter.ResumeLayout(false);
			this.pnlDataCenter.ResumeLayout(false);
			this.pnlDataCenterTop.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fpsCrossOne)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpsCrossOne_Sheet1)).EndInit();
			this.pnlLeft.ResumeLayout(false);
			this.pnlLeftCenter.ResumeLayout(false);
			this.pnlLeftCenterTop.ResumeLayout(false);
			this.pnlLeftBottom.ResumeLayout(false);
			this.pnlLeftBottomTop.ResumeLayout(false);
			this.pnlLeftBottomTopTop3.ResumeLayout(false);
			this.pnlLeftBottomTopTop2.ResumeLayout(false);
			this.pnlLeftBottomTopTop.ResumeLayout(false);
			this.pnlLeftTop.ResumeLayout(false);
			this.pnlLeftTopTop.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region  ★ 프로그램 초기화 영역 메소드 모음

		#region  ■ User PUBLIC 프로퍼티
		/// <summary>
		/// CrossOne.Aspx에서 ASP.NET 워크프로세스의 USER세션 정보를 WINFORM(CROSSONE)으로 넘겨줌,
		/// WINFORM 인스턴싱후 최초 호출되는 프로그램 진입점
		/// 1) 초기화 프로시져를 호출함
		/// </summary>
		public string User
		{
			get { return strUserID; }
			set
			{
				strUserID =  value;
				strUserID = strUserID.ToUpper();
				SetInitialize();
			}
		}
		#endregion

		#region  ■ SetChartItemArrange(CHARTVIEW_INITYPE eCHARTVIEW_INITYPE)
		/// <summary>
		/// 차트 사용자설정정보를 셋팅하기 위한 초기화 프로시져
		/// 1) 차트항목 사용자정의 셋팅을 T_DMS_USER_CONFIG 테이블로 부터 가져와서 반영함.
		/// 2) 차트 사용자정의 셋팅을 InMemory변수인 arrUserConfiguredDynamicChartItem에 저장함
		/// 3) 차트 드로잉시 arrUserConfiguredDynamicChartItem변수를 기반으로 다이나믹 차트 드로잉을 수행
		/// </summary>
		private void SetChartItemArrange(CHARTVIEW_INITYPE eCHARTVIEW_INITYPE)
		{
			DataSet dsUserChartConfig = null;
			dsUserChartConfig = GetUserChartConfig();
			if(eCHARTVIEW_INITYPE == CHARTVIEW_INITYPE.TYPE_INITIALIZE)
			{
				if(dsUserChartConfig != null && dsUserChartConfig.Tables[0].Rows.Count > 0)
				{
					arrUserConfiguredDynamicChartItem = dsUserChartConfig.Tables[0].Rows[0]["VALUE_A"].ToString().Split(Convert.ToChar(";"));
				}
			}
		}
		#endregion

		#region  ■ SetEQListViewArrange(EQLISTVIEW_INITYPE eEQLISTVIEW_INITYPE)
		/// <summary>
		/// 장비 사용자설정정보를 셋팅하기 위한 초기화 프로시져
		/// 1) 장비 사용자정의 셋팅을 T_DMS_USER_CONFIG 테이블로 부터 가져와서 반영함.
		/// 2) 장비 사용자정의 셋팅을 InMemory변수인 strFilteredEQList에 저장함
		/// 3) 장비리스트업시 strFilteredEQList변수를 기반으로 사용자 설정항목을 장비리스트에 반영
		/// </summary>
		private void SetEQListViewArrange(EQLISTVIEW_INITYPE eEQLISTVIEW_INITYPE)
		{
			DataSet dsUserEQList = null;
			string[] strFilteredEQList = null;
			dsUserEQList = GetUserEQConfig();
			if(dsUserEQList != null && dsUserEQList.Tables[0].Rows.Count > 0)
			{
				strFilteredEQList = dsUserEQList.Tables[0].Rows[0]["VALUE_A"].ToString().Split(Convert.ToChar(";"));
			}
			if(strFilteredEQList == null){return;}
			for(int i=0;i<strFilteredEQList.Length;i++)
			{
				arrFilteredEQList.Add(strFilteredEQList[i]);
			}
		}
		#endregion

		#region  ■ SetSheetHeaderArrange(SHEETVIEW_INITYPE eSHEETVIEW_INITYPE)
		/// <summary>
		/// 시트 사용자설정정보를 셋팅하기 위한 초기화 프로시져
		/// 1) 시트 사용자정의 셋팅을 T_DMS_USER_CONFIG 테이블로 부터 가져와서 반영함.
		/// 2) 시트 사용자정의 셋팅을 InMemory변수인 arrUserConfiguredDynamicField에 저장함
		/// 3) 다이나믹 시트 구성시 arrUserConfiguredDynamicField변수를 기반으로 사용자 설정항목을 시트컬럼에 반영함
		/// </summary>
		private void SetSheetHeaderArrange(SHEETVIEW_INITYPE eSHEETVIEW_INITYPE)
		{
			DataSet dsUserSheetConfig = null;
			int intDynamicFieldCnt = 0;
			DataSet dsTmpDynamicField = null;
			DataTable dtCopier = null;
			DataRow[] drTmpDynamicField = null;

			dsUserSheetConfig = GetUserSheetConfig();

			if(eSHEETVIEW_INITYPE == SHEETVIEW_INITYPE.TYPE_INITIALIZE)
			{
				if(dsUserSheetConfig != null && dsUserSheetConfig.Tables[0].Rows.Count > 0)
				{
					arrUserConfiguredDynamicField = dsUserSheetConfig.Tables[0].Rows[0]["VALUE_A"].ToString().Split(Convert.ToChar(";"));
				}
			}
			dsTmpDynamicField = GetConfig();
			if(arrUserConfiguredDynamicField != null)
			{
				if(arrUserConfiguredDynamicField.Length == 1 && arrUserConfiguredDynamicField[0] == "")
				{
					intDynamicFieldCnt = 0;
				}
				else
				{
					intDynamicFieldCnt = arrUserConfiguredDynamicField.Length;
				
					for(int i=0;i<dsTmpDynamicField.Tables[0].Rows.Count;i++)
					{
						for(int j=0;j<arrUserConfiguredDynamicField.Length;j++)
						{
							if(dsTmpDynamicField.Tables[0].Rows[i]["NAME"].ToString() == arrUserConfiguredDynamicField[j])
							{
								dsTmpDynamicField.Tables[0].Rows[i]["COMMENT"] = "Y";
								break;
							}
						}
					}			
				}
			}

			this.fpsCrossOne.ActiveSheet.Columns.Count  = CST_FPS_STATIC_FIELD_COUNT+intDynamicFieldCnt;
			arrHeaderIndex = new int[CST_FPS_STATIC_FIELD_COUNT+intDynamicFieldCnt];
			arrHeaderText = new string[CST_FPS_STATIC_FIELD_COUNT+intDynamicFieldCnt];
			//STATIC FIELD
			arrHeaderText[CST_FPS_COLIDX_EQ_ID]				= "EQ_ID";
			arrHeaderIndex[CST_FPS_COLIDX_EQ_ID]			= CST_FPS_COLIDX_EQ_ID;
			arrHeaderText[CST_FPS_COLIDX_EVENT_STARTDT]		= "EVENT_TM";
			arrHeaderIndex[CST_FPS_COLIDX_EVENT_STARTDT]	= CST_FPS_COLIDX_EVENT_STARTDT;
			arrHeaderText[CST_FPS_COLIDX_JOB]				= "EVENT";
			arrHeaderIndex[CST_FPS_COLIDX_JOB]				= CST_FPS_COLIDX_JOB;
			arrHeaderText[CST_FPS_COLIDX_LOT_ID]			= "LOT_ID";
			arrHeaderIndex[CST_FPS_COLIDX_LOT_ID]			= CST_FPS_COLIDX_LOT_ID;
			arrHeaderText[CST_FPS_COLIDX_WF]				= "WF";
			arrHeaderIndex[CST_FPS_COLIDX_WF]				= CST_FPS_COLIDX_WF;
			arrHeaderText[CST_FPS_COLIDX_I_EQUIP]			= "I_EQUIP";
			arrHeaderIndex[CST_FPS_COLIDX_I_EQUIP]			= CST_FPS_COLIDX_I_EQUIP;
			arrHeaderText[CST_FPS_COLIDX_I_DATE]			= "I_DATE";
			arrHeaderIndex[CST_FPS_COLIDX_I_DATE]			= CST_FPS_COLIDX_I_DATE;
			arrHeaderText[CST_FPS_COLIDX_GY]				= "GY";
			arrHeaderIndex[CST_FPS_COLIDX_GY]				= CST_FPS_COLIDX_GY;
			arrHeaderText[CST_FPS_COLIDX_TOT]				= "TOT";
			arrHeaderIndex[CST_FPS_COLIDX_TOT]				= CST_FPS_COLIDX_TOT;
			arrHeaderText[CST_FPS_COLIDX_DI]				= "DI";
			arrHeaderIndex[CST_FPS_COLIDX_DI]				= CST_FPS_COLIDX_DI;
			arrHeaderText[CST_FPS_COLIDX_DR]				= "DR";
			arrHeaderIndex[CST_FPS_COLIDX_DR]				= CST_FPS_COLIDX_DR;
			arrHeaderText[CST_FPS_COLIDX_DENSITY]			= "DENSITY";
			arrHeaderIndex[CST_FPS_COLIDX_DENSITY]			= CST_FPS_COLIDX_DENSITY;
			arrHeaderText[CST_FPS_COLIDX_NORV]				= "NORV";
			arrHeaderIndex[CST_FPS_COLIDX_NORV]				= CST_FPS_COLIDX_NORV;

			//DYNAMIC FIELD 
			dtCopier = dsTmpDynamicField.Tables[0].Clone();
			drTmpDynamicField = dsTmpDynamicField.Tables[0].Select("COMMENT = 'Y'");
				
			for(int k=0;k<drTmpDynamicField.Length;k++)
			{
				dtCopier.ImportRow(drTmpDynamicField[k]);
			}
			dsDynamicField = new DataSet();
			dsDynamicField.Tables.Add(dtCopier);
			for(int j=CST_FPS_STATIC_FIELD_COUNT;j<CST_FPS_STATIC_FIELD_COUNT+intDynamicFieldCnt;j++)
			{
				arrHeaderText[j] = dsDynamicField.Tables[0].Rows[j-CST_FPS_STATIC_FIELD_COUNT]["NAME"].ToString();
				arrHeaderIndex[j] = j;		
			}			
		}
		#endregion

		#region  ■ SetInitialize()
		/// <summary>
		/// 프로그램 초기화 프로시져
		/// 1) 윈폼  인스턴싱후 JAVA 스크립트에서 사용자세션정보를 윈폼에 전달하기위해 윈폼의 USER속성입력시 호출됨
		/// 2) UI및 사용자 설정항목에 대한 초기화 작업을 수행한다.
		/// </summary>
		private void SetInitialize()
		{
			string strInitialSheetName = string.Empty;
			
			TabPage oTabPage = null;
			Panel oPanel = null;
			
			
			try
			{
				sFAB = Miracom.DMS.Config.Config.GetFABInfo();
				if(strUserID == "")
				{
					MessageBox.Show("사용자가 존재하지 않음");
					return;
				}
				this.fpsCrossOne.Sheets.Clear();
				this.tabChart.TabPages.Clear();
				//SET USER ID FROM ASP.NET SESSION
				//this.strUserID = "";
				//SHEET DATA COLLECTION INSTANCING
				oSheetDataCollection = new SheetDataCollection();
				//CARENDAR INITIALIZE
				dtpFromDate.Value = System.DateTime.Now.AddDays(-1);
				dtpToDate.Value = System.DateTime.Now;	
				//SPREAD SHEET INITIALIZE
				FarPoint.Win.Spread.SheetView oShv  = FPSpreadUtil.AddSheet(this.fpsCrossOne,"UNKNOWN_0");

				//oShv.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Cell;
				this.fpsCrossOne.ActiveSheet = oShv;
				//strInitialSheetName = this.GetSheetName();
				//this.fpsCrossOne.ActiveSheet.SheetName = strInitialSheetName + CST_STRING_UNDERBAR + this.GetSheetIndex(strInitialSheetName);
				//CHART TAB 추가 
				oTabPage = new TabPage("UNKNOWN_0");
				oPanel = new Panel();
				oPanel.AutoScroll = true;
				oPanel.Dock = System.Windows.Forms.DockStyle.Fill;
				oTabPage.Controls.Add(oPanel);
				this.tabChart.TabPages.Add(oTabPage);
				
				FPSpreadUtil.ClearAll(this.fpsCrossOne.ActiveSheet);
				this.fpsCrossOne.ActiveSheet.Rows.Count		=1;
				
				arrFilteredEQList = new ArrayList();
				// TREE,LISTVIEW 초기화
				SetProcessFlowTree();
				SetInspectionStepTree();
				SetInspectionEQList();
				//현재 다이나믹 필드로 셋팅했으나 향후 다이나믹 유저 셋팅 필드로 해야 하며 데이타가 없다면
				//다이나믹 필드로 한다.
				dsDefectConfig = GetConfig();
				SetSheetHeaderArrange(SHEETVIEW_INITYPE.TYPE_INITIALIZE);
				SetChartItemArrange(CHARTVIEW_INITYPE.TYPE_INITIALIZE);
				SetEQListViewArrange(EQLISTVIEW_INITYPE.TYPE_INITIALIZE);
				//INSPECTION EQUIPMENT FILTERING ARRAY LIST 변수 초기화.
				
				SetInitializeGrid(this.fpsCrossOne.ActiveSheet,arrHeaderIndex,arrHeaderText);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		#endregion

		#region  ■ SetInitializeGrid(FarPoint.Win.Spread.SheetView oShv,int[] arrHIdxs,string[] arrHTexts)
		/// <summary>
		/// 스프레드 그리드 컬럼 레이아웃 초기화 함수
		/// </summary>
		private void SetInitializeGrid(FarPoint.Win.Spread.SheetView oShv,int[] arrHIdxs,string[] arrHTexts)
		{
			if(oShv.Rows.Count == 0){oShv.Rows.Count = 1;}
			oShv.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
			oShv.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
			oShv.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Cell;
			oShv.FrozenColumnCount=5;
			FPSpreadUtil.SetCellText(oShv,CST_FPS_ROWIDX_HEADER,arrHIdxs,arrHTexts);
			FPSpreadUtil.SetRowBackGroundColor(oShv,CST_FPS_ROWIDX_HEADER,Color.AntiqueWhite);
			FPSpreadUtil.SetColDefaultAlign(oShv);
			FPSpreadUtil.SetCellFontBold(this.fpsCrossOne.ActiveSheet,0,0,this.fpsCrossOne.ActiveSheet.Columns.Count,1);	
			FPSpreadUtil.SetColWidth(this.fpsCrossOne.ActiveSheet
				,arrHIdxs
				,new int[]{70,90,50,70,30,70,120,100,50,50
							  ,50,50,50,50,50,50,50,50,50,50
							  ,50,50,50,50,50,50,50,50,50,50
							  ,50,50,50,50,50,50});
			FPSpreadUtil.SetColSizeFix(this.fpsCrossOne.ActiveSheet,new int[]{0,1,2,3,4});
		}
		#endregion
		
		#region  ■ SetInspectionEQList()
		/// <summary>
		/// 장비정보를 리스트뷰에 바인딩함
		/// 1) 장비테이블로부터 장비리스트 조회하여 리스트 항목 설정함
		/// 2) 장비 유저셋팅테이블로부터 리스트 조회하여 리스트항목에 체크박스의 체크여부 반영함
		/// </summary>
		private void SetInspectionEQList()
		{
			DataSet dsInspectionEQList = null;
			DataSet dsUserEQList = null;
			Remoting oRemoting = null;
			string[] strFilteredEQList = null;
			
			//LIST BOX 초기화
			lvEQList.View = View.Details;
			lvEQList.CheckBoxes = true;
			lvEQList.FullRowSelect = true;
			lvEQList.GridLines = true;

			ListViewItem oListViewItem = null;

			try
			{
				oRemoting = new Remoting();
				
				dsInspectionEQList = oRemoting.GetInspectionEquipList(strUserID,"GetInspectionEquipList");
				this.lvEQList.Columns.Add("EQ_ID",100, HorizontalAlignment.Center);
				this.lvEQList.Columns.Add("Description",100, HorizontalAlignment.Center);
				for(int i=0;i<dsInspectionEQList.Tables[0].Rows.Count;i++)
				{
					oListViewItem = new ListViewItem(dsInspectionEQList.Tables[0].Rows[i]["EQUIP_ID"].ToString());
					oListViewItem.Checked = false;
					oListViewItem.SubItems.Add(dsInspectionEQList.Tables[0].Rows[i]["EQUIP_DESC"].ToString());
					lvEQList.Items.Add(oListViewItem);
				}
				dsUserEQList = GetUserEQConfig();
				if(dsUserEQList == null)
				{return;}
				if(dsUserEQList != null && dsUserEQList.Tables[0].Rows.Count > 0)
				{
					strFilteredEQList = dsUserEQList.Tables[0].Rows[0]["VALUE_A"].ToString().Split(Convert.ToChar(";"));
				}
				if(strFilteredEQList == null){return;}
			
				for(int k=0;k<lvEQList.Items.Count;k++)
				{
					for(int j=0;j<strFilteredEQList.Length;j++)
					{
						if(strFilteredEQList[j] != "" && strFilteredEQList[j] == lvEQList.Items[k].Text)
						{
							lvEQList.Items[k].Checked = true;
						}
					}
				}			
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#endregion

		#region ★ UI 이벤트 핸들링 프로시져 모음

		#region  ■ tvProcessFlow_DoubleClick(object sender, System.EventArgs e)
		/// <summary>
		///	프로세스 트리 더블클릭시 발생이벤트
		/// </summary>
		private void tvProcessFlow_DoubleClick(object sender, System.EventArgs e)
		{
			TreeNode oNode = this.tvProcessFlow.SelectedNode;
			if(sFAB == "M10")
			{
				if(oNode != null && oNode.Parent !=null )
				{
					this.txtOper.Text = this.tvProcessFlow.SelectedNode.Text;
					this.txtProd.Text = this.tvProcessFlow.SelectedNode.Parent.Tag.ToString();
				}
			}
			else if(sFAB == "M6")
			{
				if(oNode != null && oNode.Parent !=null && oNode.Parent.Parent !=null)
				{
					this.txtOper.Text = this.tvProcessFlow.SelectedNode.Text;
					this.txtRoute.Text = this.tvProcessFlow.SelectedNode.Parent.Text;
					this.txtProd.Text = this.tvProcessFlow.SelectedNode.Parent.Parent.Tag.ToString();
				}
			}
		}
		#endregion

		#region  ■ tvInspectionStep_DoubleClick(object sender, System.EventArgs e)
		/// <summary>
		/// 스텝트리 더블클릭시 발생이벤트
		/// </summary>
		private void tvInspectionStep_DoubleClick(object sender, System.EventArgs e)
		{
			TreeNode oNode = this.tvProcessFlow.SelectedNode;
			if(this.txtOper.Text.Trim() != CST_STRING_NULL)
			{
				if(this.tvInspectionStep.SelectedNode.ImageIndex == 2)
				{
					//현재는 디바이스만 트리에출력되나 향후 PROD 코드도 TAG속성에 넣을것이므로
					//스플릿해서 비교해야함.
					if(this.txtProd.Text == this.tvInspectionStep.SelectedNode.Parent.Text)
					{
						this.txtStepID.Text = this.tvInspectionStep.SelectedNode.Text;
					}
					else
					{
						MessageBox.Show("선택한 스텝의 디바이스가 선택된 프로세스의 디바이스와 틀립니다.다시 선택하세요!");
					}		
				}
			}
			else
			{
				MessageBox.Show("프로세스를 선택하세요!");
			}
		}
		#endregion
		
		#region  ■ chkInspectedOnly_CheckedChanged(object sender, System.EventArgs e)
		/// <summary>
		/// INSPECTION ONLY 체크박스 선택시 발생하는 이벤트 프로시져
		/// 1) 백엔드 데이타셋을 사용하여 데이타를 필터링한다.
		/// 2) 시트의 헤더를 재초기화 한다.
		/// 3) 필터링된 데이타를 출력한다.
		/// </summary>			
		private void chkInspectedOnly_CheckedChanged(object sender, System.EventArgs e)
		{
			SetSheetHeaderArrange(SHEETVIEW_INITYPE.TYPE_DYNAMIC);
			SetDataFilteredDisplay();
		}
		#endregion
		
		#region  ■ chkClassifiedOnly_CheckedChanged(object sender, System.EventArgs e)
		/// <summary>
		/// CLASSIFIED ONLY 체크박스 선택시 발생하는 이벤트 프로시져
		/// 1) 백엔드 데이타셋을 사용하여 데이타를 필터링한다.
		/// 2) 시트의 헤더를 재초기화 한다.
		/// 3) 필터링된 데이타를 출력한다.
		/// </summary>				
		private void chkClassifiedOnly_CheckedChanged(object sender, System.EventArgs e)
		{
			SetSheetHeaderArrange(SHEETVIEW_INITYPE.TYPE_DYNAMIC);
			SetDataFilteredDisplay();
		}
		#endregion
		
		#region  ■ btnAddSheet_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 시트추가 버튼을 클릭했을때 발생하는 이벤트 프로시져
		/// 1) 시트 네이밍룰을 적용한다.
		/// 2) 시트를 추가한다. 
		/// 3) 탭을 추가한다. (시트와 탭은 항상 동기화 되어 있어야 한다.)
		/// </summary>
		private void btnAddSheet_Click(object sender, System.EventArgs e)
		{
			//시트 추가시 차트탭 추가됨
			FarPoint.Win.Spread.SheetView shv = null;
			TabPage oTabPage = null;
			Panel oPanel = null;
			string strSheetName = string.Empty;
			string strSheetInstanceName = string.Empty;
			string strNewSheetName = string.Empty;
			bool bRet = false;
			strSheetName = GetSheetName();
			
			for(int i=0;i<fpsCrossOne.Sheets.Count;i++)
			{
				strSheetInstanceName = fpsCrossOne.Sheets[i].SheetName;
				strSheetInstanceName =strSheetInstanceName.Substring(0, strSheetInstanceName.LastIndexOf(CST_STRING_UNDERBAR));
				
				//같은 시트 이름이 존재하는지 체크 하고 있을경우 인덱스 채번을 한다.
				if(strSheetName == strSheetInstanceName)
				{
					strSheetName = strSheetName + CST_STRING_UNDERBAR + GetSheetIndex(strSheetName).ToString();
					bRet = true;
					break;
				}
			}
			//만일 기존에 같은 이름의 시트가 없을경우  시트이름을 초기값으로 사용하며 같은이름이 있을경우 인덱스 넘버링을 한다.
			if(bRet)
			{
				//SHEET 추가
				shv = FPSpreadUtil.AddSheet(this.fpsCrossOne,strSheetName);
				//shv.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
				//CHART TAB 추가
				oTabPage = new TabPage(strSheetName);
				oPanel = new Panel();
				oPanel.AutoScroll = true;
				oPanel.Dock = System.Windows.Forms.DockStyle.Fill;
				oTabPage.Controls.Add(oPanel);
				this.tabChart.TabPages.Add(oTabPage);
			}
			else
			{
				strNewSheetName = strSheetName+ CST_STRING_UNDERBAR + GetSheetIndex(strSheetName).ToString();
				shv = FPSpreadUtil.AddSheet(this.fpsCrossOne,strNewSheetName);
				//shv.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
				oTabPage = new TabPage(strNewSheetName);
				oPanel = new Panel();
				oPanel.AutoScroll = true;
				oPanel.Dock = System.Windows.Forms.DockStyle.Fill;
				oTabPage.Controls.Add(oPanel);
				this.tabChart.TabPages.Add(oTabPage);
			}
			shv.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
			shv.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
			shv.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Cell;

			shv.FrozenColumnCount = 5;
			this.fpsCrossOne.ActiveSheet = shv;
			this.tabChart.SelectedTab = oTabPage;
			this.fpsCrossOne.ActiveSheet.Rows.Count		= 1;
			this.fpsCrossOne.ActiveSheet.Columns.Count  = arrHeaderIndex.Length;
			SetInitializeGrid(shv,arrHeaderIndex,arrHeaderText);
		}
		#endregion

		#region  ■ btnRemoveSheet_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 시트제거 버튼을 클릭했을때 발생하는 이벤트 프로시져
		/// 1) 시트와 탭을 동시에 삭제한다.
		/// </summary>			
		private void btnRemoveSheet_Click(object sender, System.EventArgs e)
		{
			if(this.fpsCrossOne.Sheets.Count == 1)
			{
				MessageBox.Show("시트가 하나만 존재하므로 삭제할수 없습니다.");
				return;
			}
			for(int i=0;i<this.tabChart.TabPages.Count;i++)
			{
				if(this.tabChart.TabPages[i].Text == this.fpsCrossOne.ActiveSheet.SheetName)
				{
					FPSpreadUtil.RemoveSheet(this.fpsCrossOne);
					this.tabChart.TabPages.RemoveAt(i);
				}
			}
		}
		#endregion

		#region  ■ btnSelectData_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 조회 버튼을 클릭했을때 발생하는 이벤트 프로시져
		/// 1) 쿼리 진입 함수를 호출한다.
		/// </summary>			
		private void btnSelectData_Click(object sender, System.EventArgs e)
		{
			StartQuery();
			fpsCrossOne.AutoClipboard = true;
			
			
//			FarPoint.Win.Spread.SpreadView sv = this.fpsCrossOne.GetRootWorkbook();
//			sv.ClipboardOptions = FarPoint.Win.Spread.ClipboardOptions.AllHeaders;
//			sv.AutoClipboard = true;
		}
		#endregion

		#region ■ btnExport_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 엑셀출력버튼을 클릭했을때 발생하는 이벤트 프로시져
		/// 1) EXCEL EXPORTER를 로드한다.
		/// 2) 현재 존재하는 시트의 이름을 EXCEL EXPORTER로 넘겨준다.
		/// </summary>
		private void btnExport_Click(object sender, System.EventArgs e)
		{
			ExcelExporter oDExcelExporter = null;

			oDExcelExporter = new ExcelExporter();
			oDExcelExporter.iCallerFormPtr = this.Handle;
			for(int i=0;i<this.fpsCrossOne.Sheets.Count;i++)
			{
				oDExcelExporter.arrSheetList.Add(fpsCrossOne.Sheets[i].SheetName);
			}
			oDExcelExporter.SetSheetList();
			oDExcelExporter.ShowDialog();
		}
		#endregion

		#region ■ btnI_EQ_Save_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 장비 목록의 저장버튼을 클릭했을때 발생하는 이벤트 프로시져
		/// 1) 장비리스트에 대한 사용자 설정 정보를 저장,업데이트 한다.
		/// </summary>
		private void btnI_EQ_Save_Click(object sender, System.EventArgs e)
		{
			//장비 저장 시 체크된 장비를 저장한다.(T_DMS_USER_CONFIG)
			DataSet dsUserEQList = null;

			dsUserEQList = GetUserEQConfig();
			if(dsUserEQList != null && dsUserEQList.Tables[0].Rows.Count > 0)
			{
				// UPDATE
				SetUserEQConfig(USER_CONFIG_QUERY_TYPE.UPDATE);
			}
			else
			{
				//INSERT
				SetUserEQConfig(USER_CONFIG_QUERY_TYPE.INSERT);
			}
		}
		#endregion

		#region ■ btnI_EQ_Apply_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 장비 목록의 적용버튼을 클릭했을때 발생하는 이벤트 프로시져
		/// 1) 데이타를 시트에 출력시 선택된 장비에 해당하는 인스펙션 정보만을 출력한다.
		/// </summary>
		private void btnI_EQ_Apply_Click(object sender, System.EventArgs e)
		{
			// 적용 버튼 클릭시 또 다시 데이타 필터링이 일어난다.
			// 체크된 장비만 YES DATA를 뿌려준다.
			if(this.lvEQList.Items.Count<1){return;}
			arrFilteredEQList.Clear();
			for(int i=0;i<this.lvEQList.Items.Count;i++)
			{
				if(this.lvEQList.Items[i].Checked == true)
				{
					arrFilteredEQList.Add(lvEQList.Items[i].Text);
				}
			}
		}
		#endregion

		#region ■ btnEQLSelectAll_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 장비 목록의 ALL버튼을 클릭했을때 발생하는 이벤트 프로시져
		/// 1) 전체 목록을 체크한다.
		/// </summary>
		private void btnEQLSelectAll_Click(object sender, System.EventArgs e)
		{
			for(int i=0;i<lvEQList.Items.Count;i++)
			{
				lvEQList.Items[i].Checked = true;
			}
		}
		#endregion

		#region ■ btnEQLSelectNone_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 장비 목록의 NONE버튼을 클릭했을때 발생하는 이벤트 프로시져
		/// 1) 전체 목록을 체크하지 않는다.
		/// </summary>
		private void btnEQLSelectNone_Click(object sender, System.EventArgs e)
		{
			for(int i=0;i<lvEQList.Items.Count;i++)
			{
				lvEQList.Items[i].Checked = false;
			}
		}
		#endregion

		#region ■ fpsCrossOne_ActiveSheetChanged(object sender, System.EventArgs e)
		/// <summary>
		/// 시트가 다중으로 있을때 시트선택을 변경할경우 발생하는 이벤트 프로시져
		/// 1) 액티브 탭을 같은 인덱스로 동기화 시킨다.
		/// </summary>
		private void fpsCrossOne_ActiveSheetChanged(object sender, System.EventArgs e)
		{
			for(int i=0;i<this.tabChart.TabPages.Count;i++)
			{
				if(this.tabChart.TabPages[i].Text.Trim() == this.fpsCrossOne.ActiveSheet.SheetName)
				{
					this.tabChart.SelectedIndex = i;
				}
			}
			
		}
		#endregion

		#region ■ tabChart_SelectedIndexChanged(object sender, System.EventArgs e)
		/// <summary>
		/// 탭선택이 변경될때 발생하는 이벤트 프로시져
		/// 1) 시트탭도 같이 변경된다.
		/// </summary>
		private void tabChart_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i=0;i<this.fpsCrossOne.Sheets.Count;i++)
			{
				if(this.fpsCrossOne.Sheets[i].SheetName == tabChart.TabPages[tabChart.SelectedIndex].Text)
				{
					this.fpsCrossOne.ActiveSheet = this.fpsCrossOne.Sheets[i];
				}
			}
		}
		#endregion

		#region ■ btnDynamicSelector_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 그리드 컬럼항목 선택버튼을 클릭시에 발생하는 이벤트 프로시져
		/// 1) 다이나믹 설렉터를 로딩한다.
		/// 2) 다이나믹 설렉터가 로딩될때 eSELECTOR_TYPE에 따라 그리드항목 설정을 선택하여 로딩한다.
		/// </summary>
		private void btnDynamicSelector_Click(object sender, System.EventArgs e)
		{
			if(!bSheetDynamicFieldSelector) 
			{
				oSheetDynamicFieldSelector = new DynamicFieldSelector();
				oSheetDynamicFieldSelector.eSELECTOR_TYPE = SELECTOR_TYPE.DEFECT_FIELD_SHEET;
				oSheetDynamicFieldSelector.strUserID = this.strUserID;
				oSheetDynamicFieldSelector.iCallerFormPtr = this.Handle;
				oSheetDynamicFieldSelector.Show();
				bSheetDynamicFieldSelector = true;
			}
			else
			{
				oSheetDynamicFieldSelector.Activate();
			}
		}
		#endregion

		#region ■ btnChartSelector_Click(object sender, System.EventArgs e)
		/// <summary>
		/// 차트항목 선택버튼을 클릭시에 발생하는 이벤트 프로시져
		/// 1) 다이나믹 설렉터를 로딩한다.
		/// 2) 다이나믹 설렉터가 로딩될때 eSELECTOR_TYPE에 따라 차트 설정을 선택하여 로딩한다.
		/// </summary>
		private void btnChartSelector_Click(object sender, System.EventArgs e)
		{
			if(!bChartDynamicFieldSelector)
			{
				oChartDynamicFieldSelector = new DynamicFieldSelector();
				oChartDynamicFieldSelector.eSELECTOR_TYPE = SELECTOR_TYPE.DEFECT_FIELD_CHART;
				oChartDynamicFieldSelector.strUserID = this.strUserID;
				oChartDynamicFieldSelector.iCallerFormPtr = this.Handle;
				oChartDynamicFieldSelector.Show();	
				bChartDynamicFieldSelector = true;
			}
			else
			{
				oChartDynamicFieldSelector.Activate();
			}
		}
		#endregion

		#endregion

		#region ★ 메뉴트리 관련 메소드

		#region  ■ SetProcessFlowTree()
		/// <summary>
		/// 프로세스 트리데이타를 조회하고 설정한다.
		/// 1) 현재는 디바이스를 TDA,TEA로 고정하고 향후 라우트정보를 해석시에 동적으로 구성한다.
		/// 2) 오퍼레이션 정보도 현재는 쿼리에 하드코딩 되어있으나 향후 라우트정보 해석시 동적으로 구성한다.
		/// </summary>
		private void SetProcessFlowTree()
		{
			//Clear All Nodes
			tvProcessFlow.Nodes.Clear();
			//변수 초기화 섹션
			DataSet dsReturn=null;
			Remoting oRemoting=null;
			DataTable dtProcessFlowList;
			TreeNode oNode;

			//2005.2.4(금) : 추정연 대리님과의 협의;
			//현재 TDA만 셋팅하도록 하드코딩함 추후 PRODUCT PROD로 노드태그값을 설정함
			tvProcessFlow.ImageList = this.imgList;

			strDeviceList[0]  = "6BG";
			strDeviceList[1]  = "6CA";
			strDeviceList[2]  = "6CB";
			strDeviceList[3]  = "6CC";
			strDeviceList[4]  = "6CD";
			strDeviceList[5]  = "6CP";
			strDeviceList[6]  = "6DA";
			strDeviceList[7]  = "6DG";
			strDeviceList[8]  = "6DL";
			strDeviceList[9]  = "6ND";
			strDeviceList[10] = "6NH";


			/*2005.06.30 CHOO

			 */


			string[] paramValues = new string[1];

			//2005.06.30 CHOO
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			try
			{
				if(sFAB == "M10")
				{
					TreeUtil.AddNode(this.tvProcessFlow.Nodes,TreeUtil.SetNode("TDA","TDA",0));
					TreeUtil.AddNode(this.tvProcessFlow.Nodes,TreeUtil.SetNode("TEA","TEA",0));
				}
				else if(sFAB == "M6")
				{
					for (int nn=0; nn<11; nn++)
					{
						TreeNode aNode = TreeUtil.SetNode(strDeviceList[nn], strDeviceList[nn], 0);
						TreeUtil.AddNode(this.tvProcessFlow.Nodes, aNode);
					}
				}

				if(sFAB == "M10")
				{
					//TDA TREE NODE SETTING
					oNode = TreeUtil.FindNode(tvProcessFlow.Nodes,"TDA");
					oRemoting = new Remoting();
					dsReturn = oRemoting.GetProcessFlowList(strUserID, "GetProcessFlowListTDA");
					dtProcessFlowList = dsReturn.Tables[0];
					for(int i=0;i<dtProcessFlowList.Rows.Count;i++)
					{
						TreeUtil.AddNode(oNode.Nodes,TreeUtil.SetNode(dtProcessFlowList.Rows[i]["OPER"] + " " + dtProcessFlowList.Rows[i]["DESCRIPTION"],
							dtProcessFlowList.Rows[i]["OPER"] + " " + dtProcessFlowList.Rows[i]["DESCRIPTION"],2));
					}
					//TEA TREE NODE SETTING
					oNode = TreeUtil.FindNode(tvProcessFlow.Nodes,"TEA");
					oRemoting = new Remoting();
					dsReturn = oRemoting.GetProcessFlowList(strUserID, "GetProcessFlowListTEA");
					dtProcessFlowList = dsReturn.Tables[0];
					for(int i=0;i<dtProcessFlowList.Rows.Count;i++)
					{
						TreeUtil.AddNode(oNode.Nodes,TreeUtil.SetNode(dtProcessFlowList.Rows[i]["OPER"] + " " + dtProcessFlowList.Rows[i]["DESCRIPTION"],
							dtProcessFlowList.Rows[i]["OPER"] + " " + dtProcessFlowList.Rows[i]["DESCRIPTION"],2));
					}


				}
				else if(sFAB == "M6")
				{
					oRemoting = new Remoting();
					for (int nn=0; nn<11; nn++)
					{
						oNode = TreeUtil.FindNode(tvProcessFlow.Nodes, strDeviceList[nn]);
						paramValues[0] = strDeviceList[nn]+"         ";
						dsReturn = oRemoting.GetProcessFlowListHiCIM(strUserID, "GetProcessFlowListHiCIM", paramValues);
						dtProcessFlowList = dsReturn.Tables[0];
						for(int i=0;i<dtProcessFlowList.Rows.Count;i++)
						{
							TreeNode bNode = TreeUtil.SetNode(dtProcessFlowList.Rows[i]["RTE"].ToString().Trim() + " " + dtProcessFlowList.Rows[i]["RTE_DESC"].ToString().Trim(),
								dtProcessFlowList.Rows[i]["RTE"].ToString().Trim() + " " + dtProcessFlowList.Rows[i]["RTE_DESC"].ToString().Trim(),2);
							TreeUtil.AddNode(oNode.Nodes, bNode);

							TreeNode cNode = TreeUtil.SetNode(dtProcessFlowList.Rows[i]["OPER"].ToString().Trim() + " " + dtProcessFlowList.Rows[i]["OPER_DESC"].ToString().Trim(),
								dtProcessFlowList.Rows[i]["OPER"].ToString().Trim() + " " + dtProcessFlowList.Rows[i]["OPER_DESC"].ToString().Trim(),2);
							TreeUtil.AddNode(bNode.Nodes, cNode);
						}
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				//2005.06.30 CHOO
				this.Cursor = System.Windows.Forms.Cursors.Default;

				if(dsReturn != null) dsReturn.Dispose();
			}
		}
		#endregion

		#region  ■ SetInspectionStepTree()
		/// <summary>
		/// 스텝 트리데이타를 조회하고 설정한다.
		/// 1) 현재는 디바이스를 TDA,TEA로 고정하고 향후 라우트정보를 해석시에 동적으로 구성한다.
		/// </summary>
		private void SetInspectionStepTree()
		{
			//CLEAR ALL NODES
			tvInspectionStep.Nodes.Clear();

			//INITIALIZE VARIABLES
			DataSet dsReturn=null;
			Remoting oRemoting=null;
			DataTable dtInspectionStepList;
			string[] paramValues = new string[1];
			//2005.2.4(금) : 추정연 대리님과의 협의;
			//현재 TDA,TEA만 셋팅하도록 하드코딩함,
			tvInspectionStep.ImageList = this.imgList;
			
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			try
			{
				if(sFAB == "M10")
				{
					TreeUtil.AddNode(this.tvInspectionStep.Nodes,TreeUtil.SetNode("TDA","TDA",0));
					TreeUtil.AddNode(this.tvInspectionStep.Nodes,TreeUtil.SetNode("TEA","TEA",0));
				}
				else if(sFAB == "M6")
				{
					for (int nn=0; nn<11; nn++)
					{
						TreeUtil.AddNode(this.tvInspectionStep.Nodes,TreeUtil.SetNode(strDeviceList[nn], strDeviceList[nn], 0));
					}
				}
				oRemoting = new Remoting();
				foreach(TreeNode oNode in tvInspectionStep.Nodes)
				{
					paramValues[0] = oNode.Text;
					dsReturn = oRemoting.GetInspectionStepList(strUserID, "GetInspectionStepList",paramValues);
					dtInspectionStepList = dsReturn.Tables[0];
					for(int i=0;i<dtInspectionStepList.Rows.Count;i++)
					{
						TreeUtil.AddNode(oNode.Nodes,TreeUtil.SetNode(dtInspectionStepList.Rows[i]["STEP_ID"].ToString(),
							dtInspectionStepList.Rows[i]["STEP_ID"].ToString(),2));					
					}		
					dtInspectionStepList.Dispose();
					dsReturn.Dispose();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				//2005.06.30 CHOO
				this.Cursor = System.Windows.Forms.Cursors.Default;

				if(dsReturn != null) dsReturn.Dispose();
			}			
		}
		#endregion

		#endregion

		#region ★ DATA FILTERING 
		#region  ■ SetMainProcessFlowInfoCopyByViewFilter(ref STSheetData oSTSheetData,VIEWFILTER eViewFilter)
		/// <summary>
		/// UI 시트에 데이타 출력시 INSPECTED,CLASSIFIED체크여부의 조합에 따라
		/// 원본데이타로로부터 데이타를 해당 조건으로  필터링하여 데이타 테이블로 리턴함
		/// Param 1) 원본데이타를 가지고 있는 스트럭쳐
		/// Param 2) 파라미터로 필터 타입을 받음 [VIEWFILTER]
		/// </summary>
		private DataTable SetMainProcessFlowInfoCopyByViewFilter(ref STSheetData oSTSheetData,VIEWFILTER eViewFilter)
		{
			DataTable dtFiltered = null;
			DataRow[] drFiltered = null;

			
			GetSheetDataSchemaAwareDataSet(ref dtFiltered);
			// 필터링 조건에 따라 필터링된 데이타 테이블을 만든다.
			// 필터링은 데이타셋의 SELECT 필터조건 메소드를 사용한다.
			switch(eViewFilter)
			{
				case VIEWFILTER.Inspected_Classified:
					drFiltered = oSTSheetData.dtAll.Select("WF IS NOT NULL AND TOT <> NORV AND TOT IS NOT NULL AND NORV IS NOT NULL","EQ_ID,EVENT_TM,LOT_ID");
					//Import Inspected Rows
					for(int i=0;i<drFiltered.Length;i++)
					{
						dtFiltered.ImportRow(drFiltered[i]);
					}
					break;
				case VIEWFILTER.Inspected_NotClassified:	//INSPECTED ONLY
					drFiltered = oSTSheetData.dtAll.Select("WF IS NOT NULL","EQ_ID,EVENT_TM,LOT_ID");
					//Import Inspected Rows
					for(int i=0;i<drFiltered.Length;i++)
					{
						dtFiltered.ImportRow(drFiltered[i]);
					}
					break;
				case VIEWFILTER.NotInspected_Classified:	//CLASSIFIED ONLY
					drFiltered = oSTSheetData.dtAll.Select("TOT <> NORV AND TOT IS NOT NULL AND NORV IS NOT NULL","EQ_ID,EVENT_TM,LOT_ID");
					//Import Inspected Rows
					for(int i=0;i<drFiltered.Length;i++)
					{
						dtFiltered.ImportRow(drFiltered[i]);
					}
					break;
				case VIEWFILTER.NotInspected_NotClassified:	//ALL
					drFiltered = oSTSheetData.dtAll.Select("LOT_ID IS NOT NULL","EQ_ID,EVENT_TM,LOT_ID");
					//Import Inspected Rows
					for(int i=0;i<drFiltered.Length;i++)
					{
						dtFiltered.ImportRow(drFiltered[i]);
					}
					break;
			}
			return dtFiltered;
		}
		#endregion
		#endregion

		#region ★ CROSSONE 데이타 출력 메소드 모음

		#region  ■ StartQuery()
		/// <summary>
		/// UI 에서 조회 버튼을 클릭했을때 호출되는 프로시져. 
		/// 조회를 시작하기 위해 초기 셋팅및 후속 메소드를 호출함.
		/// 1) 백앤드 데이타셋을 저장하기위한 스트럭쳐를 생성하고 스트럭쳐 어레이에 할당한다.
		/// 2) 프로세스데이타,인스펙션데이타,디펙데이타를 순차적으로 가져와서 백앤드 데이타셋에 저장한다.
		/// 3) 데이타를 출력한다.모든 출력은 SetDataFilteredDisplay 메소드를 이용한다.
		/// </summary>
		private void StartQuery()
		{

			if(this.txtOper.Text.Trim() == "" || this.txtProd.Text.Trim() == "" || this.txtStepID.Text.Trim() =="")
			{
				MessageBox.Show("PRODUCT,OPERATION 또는 STEP_ID 가 없습니다.해당항목을 선택한후 조회하세요.!");
				return;
			}
			//---------------------------------------------------------------------------
			// 마우스 커서 모래시계
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			//---------------------------------------------------------------------------
			if(this.fpsCrossOne.Sheets.Count < 1)
			{
				MessageBox.Show("사용할 시트가 없습니다.시트를 추가하세요.");
				return;
			}

			if(this.fpsCrossOne.ActiveSheet.Rows.Count > 0)
			{this.fpsCrossOne.ActiveSheet.RowCount = 0;}
			//인스펙션 로우 인덱스를 시트 데이터 컬렉션에 저장함
			STSheetData oSTSheetData = new STSheetData();
			oSTSheetData.SheetName = string.Empty;
			oSTSheetData.dtAll = null;
			oSTSheetData.bExecuted = false;
			oSTSheetData.arrChartCollection = null;
			oSTSheetData.intSummaryRowCnt = 0;
			
			string strActiveSheetName = this.fpsCrossOne.ActiveSheet.SheetName;
			//기 저장된 SheetData가 있으면 컬렉션에서 삭제함.
			for(int i=0;i<oSheetDataCollection.Count;i++)
			{
				if(oSheetDataCollection[i].SheetName == strActiveSheetName)
				{
					oSheetDataCollection.RemoveAt(i);
				}
			}
			oSTSheetData.SheetName = strActiveSheetName;

			//Fill Data Set From Remoting
			//2005.06.29 CHOO

			if(sFAB == "M10")
			{
				SetMainProcessFlowInfoToDataSet(ref oSTSheetData);
			}
			else if(sFAB == "M6")
			{
				SetMainProcessFlowInfoToDataSetHiCIM(ref oSTSheetData);
			}
			
			if (oSTSheetData.dtAll == null)
			{ 
				//그리드 헤더추가및 스타일 지정로직.
				SetInitializeGrid(this.fpsCrossOne.ActiveSheet,arrHeaderIndex,arrHeaderText);
				this.Cursor = System.Windows.Forms.Cursors.Default;
				MessageBox.Show("데이타가 존재하지 않습니다.!");
				return;
			}
			SetMainProcessFlowWaferInfoToDataSet(ref oSTSheetData);
			oSTSheetData.bExecuted = true;
			oSheetDataCollection.Add(oSTSheetData);
			SetDataFilteredDisplay();
			//---------------------------------------------------------------------------
			// 마우스 커서 디폴트
			this.Cursor = System.Windows.Forms.Cursors.Default;
			//---------------------------------------------------------------------------
			MessageBox.Show("조회완료");
		}
		#endregion

		#region ■ GetSheetDataSchemaAwareDataSet(ref DataTable dtSheetData)
		/// <summary>
		/// 시트에 출력할 데이타를 저장하고 있는 백앤드 데이타셋의 스키마를 설정하는 기능 함수
		/// 1) 정적필드에 대해서는 하드코딩되어 스키마를 설정한다.
		/// 2) 동적필드는 CONFIG테이블로 부터 데이타를 가져와서 스키마를 설정한다.
		/// </summary>
		private void GetSheetDataSchemaAwareDataSet(ref DataTable dtSheetData)
		{
			try
			{
				dtSheetData = new DataTable();
				// STATIC FIELD DISPLAY
				dtSheetData.Columns.Add("LOT_ID", Type.GetType("System.String"));
				dtSheetData.Columns.Add("EQ_ID", Type.GetType("System.String"));
				dtSheetData.Columns.Add("EVENT_TM", Type.GetType("System.String"));
				dtSheetData.Columns.Add("EVENT", Type.GetType("System.String"));
				dtSheetData.Columns.Add("WF",Type.GetType("System.String"));
				dtSheetData.Columns.Add("STEP_SEQ", Type.GetType("System.String"));
				dtSheetData.Columns.Add("I_EQUIP", Type.GetType("System.String"));
				dtSheetData.Columns.Add("I_DATE", Type.GetType("System.String"));
				dtSheetData.Columns.Add("GY", Type.GetType("System.String"));
				dtSheetData.Columns.Add("TOT", Type.GetType("System.String"));
				dtSheetData.Columns.Add("DI", Type.GetType("System.String"));
				dtSheetData.Columns.Add("DR", Type.GetType("System.String"));
				dtSheetData.Columns.Add("DENSITY", Type.GetType("System.String"));
				dtSheetData.Columns.Add("NORV", Type.GetType("System.String"));
				// END STATIC FIELD DISPLAY

				// DYNAMIC FIELD DISPLAY
				for(int i=0;i<dsDefectConfig.Tables[0].Rows.Count;i++)
				{
					dtSheetData.Columns.Add(dsDefectConfig.Tables[0].Rows[i]["NAME"].ToString(),Type.GetType("System.String"));
				}
				// END DYNAMIC FIELD DISPLAY
			}
			catch(Exception ex)
			{throw ex;}
		}

		private void GetSheetDataSchemaAwareDataSetHiCIM(ref DataTable dtSheetData)
		{
			try
			{
				dtSheetData = new DataTable();
				// STATIC FIELD DISPLAY
				dtSheetData.Columns.Add("LOT_ID", Type.GetType("System.String"));
				dtSheetData.Columns.Add("EQ_ID", Type.GetType("System.String"));
				dtSheetData.Columns.Add("EVENT_TM", Type.GetType("System.String"));
				dtSheetData.Columns.Add("EVENT", Type.GetType("System.String"));
				dtSheetData.Columns.Add("WF",Type.GetType("System.String"));
				dtSheetData.Columns.Add("STEP_SEQ", Type.GetType("System.String"));
				dtSheetData.Columns.Add("I_EQUIP", Type.GetType("System.String"));
				dtSheetData.Columns.Add("I_DATE", Type.GetType("System.String"));
				dtSheetData.Columns.Add("GY", Type.GetType("System.String"));
				dtSheetData.Columns.Add("TOT", Type.GetType("System.String"));
				dtSheetData.Columns.Add("DI", Type.GetType("System.String"));
				dtSheetData.Columns.Add("DR", Type.GetType("System.String"));
				dtSheetData.Columns.Add("DENSITY", Type.GetType("System.String"));
				dtSheetData.Columns.Add("NORV", Type.GetType("System.String"));
				// END STATIC FIELD DISPLAY

				// DYNAMIC FIELD DISPLAY
				for(int i=0;i<dsDefectConfig.Tables[0].Rows.Count;i++)
				{
					dtSheetData.Columns.Add(dsDefectConfig.Tables[0].Rows[i]["NAME"].ToString(),Type.GetType("System.String"));
				}
				// END DYNAMIC FIELD DISPLAY
			}
			catch(Exception ex)
			{throw ex;}
		}
		#endregion

		#region ■ GetSheetDataSchemaAwareRowInstance(ref DataTable dtSheetData,DataRow rwMainProcessFlow)
		/// <summary>
		/// 시트에 출력할 데이타를 저장하고 있는 백앤드 데이타셋의 스키마를 설정하는 기능 함수
		/// 1) 정적필드에 대해서는 하드코딩되어 스키마를 설정한다.
		/// 2) 동적필드는 CONFIG테이블로 부터 데이타를 가져와서 스키마를 설정한다.
		/// </summary>
		private DataRow GetSheetDataSchemaAwareRowInstance(ref DataTable dtSheetData,DataRow rwMainProcessFlow)
		{
			DataRow drSheetDataRow = dtSheetData.NewRow();
			drSheetDataRow["LOT_ID"] = rwMainProcessFlow["LOT_ID"].ToString();
			drSheetDataRow["EQ_ID"] = rwMainProcessFlow["EQ_ID"].ToString();
			drSheetDataRow["EVENT_TM"] = rwMainProcessFlow["EVENT_TM"].ToString();
			drSheetDataRow["EVENT"] = rwMainProcessFlow["EVENT"].ToString();
			return drSheetDataRow;
		}
		#endregion

		#region ■ SetMainProcessFlowInfoToDataSet(ref STSheetData oSTSheetData)

		private void SetMainProcessFlowInfoToDataSet(ref STSheetData oSTSheetData)
		{
			//변수 초기화 섹션
			DataSet dsReturn=null;
			Remoting oRemoting=null;
			string[] strOperSplits = null;
			DataTable dtAllSheetData = null;
			DataRow drAllSheetData = null;
			/// <summary>
			/// 2005.06.23 CHOO
			/// Update Query Logic... to refer Index (Enhancing Query Performance)
			/// </summary>
			string strStartDT = dtpFromDate.Value.Year.ToString("0000")+ dtpFromDate.Value.Month.ToString("00") +dtpFromDate.Value.Day.ToString("00");
			string strEndDT = dtpToDate.Value.Year.ToString("0000")+dtpToDate.Value.Month.ToString("00")+dtpToDate.Value.Day.ToString("00");
			string strStartClock = "060000000000";
			string strEndClock = "060000000000";

			strOperSplits = this.txtOper.Text.Split(Convert.ToChar(CST_STRING_SPACE));

			string[] paramValues = new string[4];
			try
			{
				paramValues[0] = txtProd.Text +"%";
				paramValues[1] = strStartDT + "" + strStartClock;
				paramValues[2] = strEndDT + "" + strEndClock;
				paramValues[3] = strOperSplits[0];
		
				oRemoting = new Remoting();
				dsReturn = oRemoting.GetMainProcessFlowInfo(strUserID, "GetMainProcessFlowInfo2",paramValues);
				if (dsReturn == null || dsReturn.Tables[0].Rows.Count == 0)
				{return;}
				//DataSet을 시트라고 가정하고 데이터를 인서트 한다.
				//Table Schema Aware
				GetSheetDataSchemaAwareDataSet(ref dtAllSheetData);
				for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
				{
					drAllSheetData = GetSheetDataSchemaAwareRowInstance(ref dtAllSheetData,dsReturn.Tables[0].Rows[i]);
					dtAllSheetData.Rows.Add(drAllSheetData);
					oSTSheetData.dtAll = dtAllSheetData;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
			}	
		}
		#endregion

		#region ■ SetMainProcessFlowInfoToDataSetHiCIM(ref STSheetData oSTSheetData)
		private void SetMainProcessFlowInfoToDataSetHiCIM(ref STSheetData oSTSheetData)
		{
			//변수 초기화 섹션
			DataSet dsReturn=null;
			Remoting oRemoting=null;
			string[] strRteSplits = null;
			string[] strOperSplits = null;
			DataTable dtAllSheetData = null;
			DataRow drAllSheetData = null;

			/// <summary>
			/// 2005.06.23 CHOO
			/// Update Query Logic... to refer Index (Enhancing Query Performance)
			/// </summary>
			//	string strStartDT = dtpFromDate.Value.Year+"."+dtpFromDate.Value.Month+"."+dtpFromDate.Value.Day;
			//	string strEndDT = dtpToDate.Value.Year+"."+dtpToDate.Value.Month+"."+dtpToDate.Value.Day;
			//	string strStartClock = "06";
			//	string strEndClock = "06";

			string strStartDT = dtpFromDate.Value.Year.ToString("0000")+ dtpFromDate.Value.Month.ToString("00") +dtpFromDate.Value.Day.ToString("00");
			string strEndDT = dtpToDate.Value.Year.ToString("0000")+dtpToDate.Value.Month.ToString("00")+dtpToDate.Value.Day.ToString("00");
			string strStartClock = "060000";
			string strEndClock = "060000";

			strRteSplits = this.txtRoute.Text.Split(Convert.ToChar(CST_STRING_SPACE));
			strOperSplits = this.txtOper.Text.Split(Convert.ToChar(CST_STRING_SPACE));

			string[] paramValues = new string[5];
			try
			{
				/* 2005.06.23 CHOO
				paramValues[0] = strOperSplits[0];
				paramValues[1] = strStartDT + " " + strStartClock;
				paramValues[2] = strEndDT + " " + strEndClock;
				paramValues[3] = txtProd.Text +"%";
				 */	
				paramValues[0] = txtProd.Text +"%";
				paramValues[1] = strStartDT + "" + strStartClock;
				paramValues[2] = strEndDT + "" + strEndClock;
				paramValues[3] = strRteSplits[0] + "     ";
				paramValues[4] = strOperSplits[0];

				oRemoting = new Remoting();
				dsReturn = oRemoting.GetMainProcessFlowInfo(strUserID, "GetMainProcessFlowInfoFromHiCIM", paramValues);
				if (dsReturn == null || dsReturn.Tables[0].Rows.Count == 0)
				{return;}
				//DataSet을 시트라고 가정하고 데이터를 인서트 한다.
				//Table Schema Aware
				GetSheetDataSchemaAwareDataSetHiCIM(ref dtAllSheetData);
				for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
				{
					drAllSheetData = GetSheetDataSchemaAwareRowInstance(ref dtAllSheetData, dsReturn.Tables[0].Rows[i]);
					dtAllSheetData.Rows.Add(drAllSheetData);
					oSTSheetData.dtAll = dtAllSheetData;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if(dsReturn != null) dsReturn.Dispose();
			}	
		}
		#endregion

		#region ■ SetMainProcessFlowWaferInfoToDataSet(ref STSheetData oSTSheetData)
		private void SetMainProcessFlowWaferInfoToDataSet(ref STSheetData oSTSheetData)
		{
			//변수 초기화 섹션
			DataSet dsReturn = null;
			DataSet dsDieCnt = null;
			DataSet dsDefectClassifiedCnt = null;

			DataRow drTmp = null;//(wafefinfo 가 1개 이상일때 사용되는 변수
			DataTable dtCopier = null;
			string strTmp = string.Empty;
			bool bRet = false;
			bool bExists = false;
			Remoting oRemoting=null;
			string strEQID = string.Empty;
			string strLotID = string.Empty;
			string strEQIDChecked = string.Empty;

			double dInspectiedDie = 0.0;
			double dDefectiveDie = 0.0;
			double dGYValue = 0.0;

			string strStartDT = dtpFromDate.Value.Year+"-"+dtpFromDate.Value.Month+"-"+dtpFromDate.Value.Day;
			string strEndDT = dtpToDate.Value.Year+"-"+dtpToDate.Value.Month+"-"+dtpToDate.Value.Day;
			string strStartClock = "06:00:00";
			string strEndClock = "06:00:00";

			string[] paramValues = new string[4];
			string[] paramValuesStepSeq = new string[1];
			string[] paramValuesDefectClassifiedCnt = new string[2];
			string[] dynamicQueryElements = new string[1];


			string[] Event_tm = new string[1];
 
			try
			{
				dtCopier = oSTSheetData.dtAll.Clone();
				paramValues[0] = this.txtStepID.Text;
				paramValues[2] = strStartDT + " " + strStartClock;
				paramValues[3] = strEndDT + " " + strEndClock;
				
				oRemoting = new Remoting();
				
				for(int i=0;i<oSTSheetData.dtAll.Rows.Count;i++)
				{
					strLotID = oSTSheetData.dtAll.Rows[i]["LOT_ID"].ToString();
					paramValues[1] = strLotID;
					dsReturn = oRemoting.GetInspectionWaferInfo(strUserID, "GetInspectionWaferInfo",paramValues);
					if(dsReturn.Tables[0].Rows.Count>1){bRet = true;}else{bRet= false;}
					//INSPECTION 정보가 있을경우 다음 조건문 수행
					if(dsReturn != null && dsReturn.Tables[0].Rows.Count > 0)
					{
						for(int j=0;j<dsReturn.Tables[0].Rows.Count;j++)
						{
							dynamicQueryElements[0] = "";
							if(bRet) //waferinfo가 1개이상일때
							{
								bExists = false;
								strEQIDChecked = dsReturn.Tables[0].Rows[j]["I_EQUIP"].ToString().Trim();

								if(arrFilteredEQList != null && arrFilteredEQList.Count > 0)
								{
									for(int y=0;y<arrFilteredEQList.Count;y++)
									{
										if(strEQIDChecked == arrFilteredEQList[y].ToString().Trim())
										{
											bExists = true;
											break;
										}
									}
								}
								else{bExists = true;}

								if(bExists)
								{
									drTmp = dtCopier.NewRow();
									drTmp["EQ_ID"] = oSTSheetData.dtAll.Rows[i]["EQ_ID"].ToString();
									//2005.06.30 CHOO
									//drTmp["EVENT_TM"] = oSTSheetData.dtAll.Rows[i]["EVENT_TM"].ToString();
									Event_tm[0] = oSTSheetData.dtAll.Rows[i]["EVENT_TM"].ToString();
									drTmp["EVENT_TM"] = Event_tm[0].Substring(2,2) + "/"+
										Event_tm[0].Substring(4,2) + "/"+
										Event_tm[0].Substring(6,2) + " "+
										Event_tm[0].Substring(8,2) + ":"+
										Event_tm[0].Substring(10,2);
										drTmp["EVENT"] = oSTSheetData.dtAll.Rows[i]["EVENT"].ToString();
									drTmp["LOT_ID"] = oSTSheetData.dtAll.Rows[i]["LOT_ID"].ToString();

									//Set Parameter For Remoting Call
									paramValuesStepSeq[0] = dsReturn.Tables[0].Rows[j]["STEP_SEQ"].ToString();
									paramValuesDefectClassifiedCnt[0] = dsReturn.Tables[0].Rows[j]["WAFER_SEQ"].ToString();
									paramValuesDefectClassifiedCnt[1] = dsReturn.Tables[0].Rows[j]["STEP_SEQ"].ToString();
	
									dsDieCnt = oRemoting.GetInspectedDieCount(strUserID,"GetInspectedDieCount",paramValuesStepSeq);
									
									// DYNAMIC QUERY DEFINITION
									if(dsDefectConfig.Tables[0].Rows.Count > 0)
									{
										for(int k=0;k<dsDefectConfig.Tables[0].Rows.Count;k++)
										{
											strTmp += dsDefectConfig.Tables[0].Rows[k]["VALUE"].ToString() + CST_STRING_SPACE
												+ dsDefectConfig.Tables[0].Rows[k]["NAME"].ToString()+CST_STRING_COMMA_SEPARATOR;
										}		
										strTmp = strTmp.Substring(0,strTmp.Length-1);//끝 쉼표 제거
										dynamicQueryElements[0] = strTmp;
										strTmp = "";
									}
									// END DYNAMIC QUERY DEFINITION
									dsDefectClassifiedCnt = oRemoting.GetDefectClassifiedCounts(strUserID,"GetDefectClassifiedCounts",dynamicQueryElements,paramValuesDefectClassifiedCnt);
			
									drTmp["WF"] = dsReturn.Tables[0].Rows[j]["WF"].ToString();
									drTmp["STEP_SEQ"] = dsReturn.Tables[0].Rows[j]["STEP_SEQ"].ToString();
									drTmp["I_EQUIP"] = dsReturn.Tables[0].Rows[j]["I_EQUIP"].ToString();
									drTmp["I_DATE"] = dsReturn.Tables[0].Rows[j]["I_DATE"].ToString();
									drTmp["DI"] = dsDieCnt.Tables[0].Rows[0][0].ToString();
									drTmp["DR"] = dsReturn.Tables[0].Rows[j]["DEFECTIVE_DIE"].ToString();
									drTmp["DENSITY"] = dsReturn.Tables[0].Rows[j]["DEFECT_DD"].ToString();
									drTmp["TOT"] = dsReturn.Tables[0].Rows[j]["DEFECTS"].ToString();
			
									dInspectiedDie = Convert.ToDouble(dsDieCnt.Tables[0].Rows[0][0].ToString());
									dDefectiveDie = Convert.ToDouble(dsReturn.Tables[0].Rows[j]["DEFECTIVE_DIE"].ToString());
									if(dInspectiedDie != 0.0 && dDefectiveDie != 0.0)
									{
										dGYValue = ((dInspectiedDie - dDefectiveDie) / dInspectiedDie) * 100;
										drTmp["GY"] = dGYValue.ToString();		
									}
									if(dsDefectClassifiedCnt != null && dsDefectClassifiedCnt.Tables[0].Rows.Count>0)
									{
										drTmp["NORV"] = dsDefectClassifiedCnt.Tables[0].Rows[0]["NORV"].ToString();
										// DYNAMIC FIELDS DISPLAY
										for(int m=0;m<dsDefectConfig.Tables[0].Rows.Count;m++)
										{
											drTmp[dsDefectConfig.Tables[0].Rows[m]["NAME"].ToString()] = dsDefectClassifiedCnt.Tables[0].Rows[0][dsDefectConfig.Tables[0].Rows[m]["NAME"].ToString()].ToString();
										}	
									}
									else
									{
										drTmp["NORV"] = "0";
										// DYNAMIC FIELDS DISPLAY
										for(int m=0;m<dsDefectConfig.Tables[0].Rows.Count;m++)
										{
											drTmp[dsDefectConfig.Tables[0].Rows[m]["NAME"].ToString()] = "0";
										}
									}
										
									// END DYNAMIC FIELDS DISPLAY
									dtCopier.Rows.Add(drTmp);
								}							
								bExists = false;
							}
							else //waferinfo가 1개일때
							{
								// 장비 리스트뷰의 체크박스 선택후 적용 또는 저장된 데이터를 불러와
								// 인스펙션 데이타를 보여줄지 말지 필터링한다.
								bExists = false;
								strEQIDChecked = dsReturn.Tables[0].Rows[j]["I_EQUIP"].ToString().Trim();
								if(arrFilteredEQList != null && arrFilteredEQList.Count > 0)
								{
									for(int y=0;y<arrFilteredEQList.Count;y++)
									{
										if(strEQIDChecked == arrFilteredEQList[y].ToString().Trim())
										{
											bExists = true;
											break;
										}
									}
								}
								else{bExists = true;}	
								if(bExists)
								{
									//Set Parameter For Remoting Call
									paramValuesStepSeq[0] = dsReturn.Tables[0].Rows[j]["STEP_SEQ"].ToString();
									paramValuesDefectClassifiedCnt[0] = dsReturn.Tables[0].Rows[j]["WAFER_SEQ"].ToString();
									paramValuesDefectClassifiedCnt[1] = dsReturn.Tables[0].Rows[j]["STEP_SEQ"].ToString();
						
									dsDieCnt = oRemoting.GetInspectedDieCount(strUserID,"GetInspectedDieCount",paramValuesStepSeq);

									// DYNAMIC QUERY DEFINITION
									if(dsDefectConfig.Tables[0].Rows.Count > 0)
									{
										for(int k=0;k<dsDefectConfig.Tables[0].Rows.Count;k++)
										{
											strTmp += dsDefectConfig.Tables[0].Rows[k]["VALUE"].ToString() + CST_STRING_SPACE
												+ dsDefectConfig.Tables[0].Rows[k]["NAME"].ToString()+CST_STRING_COMMA_SEPARATOR;
										}		
										strTmp = strTmp.Substring(0,strTmp.Length-1);//끝 쉼표 제거
										dynamicQueryElements[0] = strTmp;
										strTmp = "";
									}

									dsDefectClassifiedCnt = oRemoting.GetDefectClassifiedCounts(strUserID,"GetDefectClassifiedCounts",dynamicQueryElements,paramValuesDefectClassifiedCnt);
									oSTSheetData.dtAll.Rows[i]["WF"] = dsReturn.Tables[0].Rows[j]["WF"].ToString();
									oSTSheetData.dtAll.Rows[i]["STEP_SEQ"] = dsReturn.Tables[0].Rows[j]["STEP_SEQ"].ToString();
									oSTSheetData.dtAll.Rows[i]["I_EQUIP"] = dsReturn.Tables[0].Rows[j]["I_EQUIP"].ToString();
									oSTSheetData.dtAll.Rows[i]["I_DATE"] = dsReturn.Tables[0].Rows[j]["I_DATE"].ToString();
									oSTSheetData.dtAll.Rows[i]["DI"] = dsDieCnt.Tables[0].Rows[0][0].ToString();
									oSTSheetData.dtAll.Rows[i]["DR"] = dsReturn.Tables[0].Rows[j]["DEFECTIVE_DIE"].ToString();
									oSTSheetData.dtAll.Rows[i]["DENSITY"] = dsReturn.Tables[0].Rows[j]["DEFECT_DD"].ToString();
									oSTSheetData.dtAll.Rows[i]["TOT"] = dsReturn.Tables[0].Rows[j]["DEFECTS"].ToString();
						
									dInspectiedDie = Convert.ToDouble(dsDieCnt.Tables[0].Rows[0][0].ToString());
									dDefectiveDie = Convert.ToDouble(dsReturn.Tables[0].Rows[j]["DEFECTIVE_DIE"].ToString());
									if(dInspectiedDie != 0.0 && dDefectiveDie != 0.0)
									{
										dGYValue = ((dInspectiedDie - dDefectiveDie) / dInspectiedDie) * 100;
										oSTSheetData.dtAll.Rows[i]["GY"] = dGYValue.ToString();		
									}
									if(dsDefectClassifiedCnt != null && dsDefectClassifiedCnt.Tables[0].Rows.Count>0)
									{
										oSTSheetData.dtAll.Rows[i]["NORV"]  = dsDefectClassifiedCnt.Tables[0].Rows[0]["NORV"].ToString();
										// DYNAMIC FIELDS DISPLAY
										//DYNAMIC FIELD DISPLAY
										for(int m=0;m<dsDefectConfig.Tables[0].Rows.Count;m++)
										{
											oSTSheetData.dtAll.Rows[i][dsDefectConfig.Tables[0].Rows[m]["NAME"].ToString()] = dsDefectClassifiedCnt.Tables[0].Rows[0][dsDefectConfig.Tables[0].Rows[m]["NAME"].ToString()].ToString();
										}	
									}
									else
									{
										oSTSheetData.dtAll.Rows[i]["NORV"]  = "0";
										// DYNAMIC FIELDS DISPLAY
										for(int m=0;m<dsDefectConfig.Tables[0].Rows.Count;m++)
										{
											oSTSheetData.dtAll.Rows[i][dsDefectConfig.Tables[0].Rows[m]["NAME"].ToString()] = "0";
										}	
									}
									//END DYNAMIC FIELD DISPLAY
									bRet = true;
								}
								bExists = false;
							}//end if
						}//end for
					}//end if
				}//end for

				for(int m=0;m<dtCopier.Rows.Count;m++)
				{
					oSTSheetData.dtAll.ImportRow(dtCopier.Rows[m]);
				}
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
		#endregion
		
		#region ■ SetMainProcessFlowGrandTotalToSheet(DataTable dtSummary)
		// GRAND TOTAL
		private void SetMainProcessFlowGrandTotalToSheet(DataTable dtSummary)
		{
			// STATIC FIELD SUM VARIABLE
			double dSumGY	= 0.0;
			double dSumTOT	= 0.0;
			double dSumDI	= 0.0;
			double dSumDR	= 0.0;
			double dSumDENSITY	= 0.0;
			double dSumNORV  = 0.0;
			// END STATIC FIELD SUM VARIABLE

			// DYNAMIC FIELD SUM VARIABLE
			double[] dSumTmp = null;
			// END DYNAMIC FIELD SUM VARIABLE
			if(dtSummary.Rows.Count < 1)
			{
				return;
			}
			for(int i=0;i<dtSummary.Rows.Count;i++)
			{
				dSumGY	+= Convert.ToDouble(dtSummary.Rows[i]["GY"] == DBNull.Value ? "0.0" : dtSummary.Rows[i]["GY"].ToString());
				dSumTOT += Convert.ToDouble(dtSummary.Rows[i]["TOT"] == DBNull.Value ? "0.0" : dtSummary.Rows[i]["TOT"].ToString());
				dSumDI += Convert.ToDouble(dtSummary.Rows[i]["DI"] == DBNull.Value ? "0.0" : dtSummary.Rows[i]["DI"].ToString());
				dSumDR += Convert.ToDouble(dtSummary.Rows[i]["DR"] == DBNull.Value ? "0.0" : dtSummary.Rows[i]["DR"].ToString());
				dSumDENSITY += Convert.ToDouble(dtSummary.Rows[i]["DENSITY"] == DBNull.Value ? "0.0" : dtSummary.Rows[i]["DENSITY"].ToString());
				dSumNORV += Convert.ToDouble(dtSummary.Rows[i]["NORV"] == DBNull.Value ? "0.0" : dtSummary.Rows[i]["NORV"].ToString());

				dSumTmp = new double[dsDefectConfig.Tables[0].Rows.Count];
				// DYNAMIC FIELD 합계 변수 초기화
				for(int j=0;j<dsDefectConfig.Tables[0].Rows.Count;j++)
				{
					dSumTmp[j] = 0.0;
				}
				// DYNAMIC FIELD 합계
				for(int k=0;k<dsDefectConfig.Tables[0].Rows.Count;k++)
				{
					dSumTmp[k] += Convert.ToDouble(dtSummary.Rows[i][dsDefectConfig.Tables[0].Rows[k]["NAME"].ToString()] == DBNull.Value ? "0.0" : dtSummary.Rows[i][dsDefectConfig.Tables[0].Rows[k]["NAME"].ToString()].ToString());
				}
			}

			// STATIC SUM
			dSumGY = dSumGY/dtSummary.Rows.Count;
			dSumTOT = dSumTOT/dtSummary.Rows.Count;
			dSumDI = dSumDI/dtSummary.Rows.Count;
			dSumDR = dSumDR/dtSummary.Rows.Count;
			dSumDENSITY = dSumDENSITY/dtSummary.Rows.Count;
			dSumNORV = dSumNORV/dtSummary.Rows.Count;
			// END STATIC SUM
			
			// DYNAMIC SUM
			for(int m=0;m<dSumTmp.Length;m++)
			{
				dSumTmp[m] = dSumTmp[m]/dtSummary.Rows.Count;
			}
			// END DYNAMIC SUM

			// STATIC SHEET EDITING
			FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,CST_FPS_COLIDX_EQ_ID,"TOTAL");
			FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,CST_FPS_COLIDX_GY,dSumGY.ToString());
			FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,CST_FPS_COLIDX_TOT,dSumTOT.ToString());
			FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,CST_FPS_COLIDX_DI,dSumDI.ToString());
			FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,CST_FPS_COLIDX_DR,dSumDR.ToString());
			FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,CST_FPS_COLIDX_DENSITY,dSumDENSITY.ToString());
			FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,CST_FPS_COLIDX_NORV,dSumNORV.ToString());
			// END STATIC SHEET EDITING

			// DYNAMIC SHEET EDITING
			for(int n=0;n<dsDynamicField.Tables[0].Rows.Count;n++)
			{
				FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,n+CST_FPS_STATIC_FIELD_COUNT,dSumTmp[n].ToString());
			}
			// END DYNAMIC SHEET EDITING
		}
		#endregion

		#region ■ SetMainProcessFlowLOT_IDGrandTotalToSheet(int intSummaryRowEOF)
		// LOT_ID GRAND TOTAL
		private void SetMainProcessFlowLOT_IDGrandTotalToSheet(int intSummaryRowEOF)
		{
			int intSumLOT_ID_LEFT = 0;
			int intSumLOT_ID_RIGHT = 0;
			string[] arrLOT_ID = null;
			string strLOT_ID = string.Empty;
			for(int i=2;i<intSummaryRowEOF+2;i++)
			{
				strLOT_ID = FPSpreadUtil.GetCellText(this.fpsCrossOne.ActiveSheet,i,CST_FPS_COLIDX_LOT_ID);
				arrLOT_ID = strLOT_ID.Split(Convert.ToChar(CST_STRING_UNDERBAR));
				intSumLOT_ID_LEFT += Convert.ToInt16(arrLOT_ID[0].ToString());
				intSumLOT_ID_RIGHT += Convert.ToInt16(arrLOT_ID[1].ToString());
			}
			strLOT_ID = intSumLOT_ID_LEFT.ToString() + CST_STRING_UNDERBAR + intSumLOT_ID_RIGHT.ToString();
			FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,CST_FPS_COLIDX_LOT_ID,strLOT_ID);
		}
		#endregion

		#region ■ SetMainProcessFlowWFGrandTotal(int intSummaryRowEOF)
		// WAFER GRANDTOTAL
		private void SetMainProcessFlowWFGrandTotal(int intSummaryRowEOF)
		{
			int intSumWF = 0;
			string strWF = string.Empty;

			for(int i=2;i<intSummaryRowEOF+2;i++)
			{
				strWF = FPSpreadUtil.GetCellText(this.fpsCrossOne.ActiveSheet,i,CST_FPS_COLIDX_WF);
				intSumWF += Convert.ToInt16(strWF);
			}
			strWF = intSumWF.ToString();
			FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,CST_FPS_ROWIDX_GRANDTOTAL,CST_FPS_COLIDX_WF,strWF);
		}
		#endregion

		#region ■ SetMainProcessFlowLOT_IDAndWFSummaryToSheet(DataTable dtAll,DataTable dtInspected,DataTable dtFiltered,int intSummaryRowEOF)
		// LOT_ID And Wafer SUMMARY
		private void SetMainProcessFlowLOT_IDAndWFSummaryToSheet(DataTable dtAll,DataTable dtInspected,DataTable dtFiltered,int intSummaryRowEOF)
		{
			string strSumLOT_ID = string.Empty;
			int intSumLOT_ID_LEFT = 0;
			int intSumLOT_ID_RIGHT = 0;
			int intSumWafer = 0;
			string strOldEQ_ID = string.Empty;
			string strNewEQ_ID = string.Empty;
			string strOldLOT_ID = string.Empty;
			string strNewLOT_ID = string.Empty;
			string strOldInspectedLOT_ID = string.Empty;
			string strNewInspectedLOT_ID = string.Empty;
			string strOldWafer_ID = string.Empty;
			string strNewWafer_ID = string.Empty;
			string strEQ_ID = string.Empty;
			string strEQNAME = string.Empty;
			ArrayList arrEQLIST = new ArrayList();
			ArrayList arrLOTLIST = new ArrayList();
			ArrayList arrInspectedLOTLIST = new ArrayList();
			ArrayList arrWAFER_IDs = new ArrayList();
			DataRow[] drEQFiltered = null;
			//EQ LIST EXTRACT
			strOldEQ_ID = CST_STRING_EMPTY;
			for(int i=0;i<dtAll.Rows.Count;i++)
			{
				strNewEQ_ID = dtAll.Rows[i]["EQ_ID"].ToString();
				if(strNewEQ_ID != strOldEQ_ID)
				{
					arrEQLIST.Add(strNewEQ_ID);
				}
				strOldEQ_ID = strNewEQ_ID;
			}
			for(int j=0;j<arrEQLIST.Count;j++)
			{
				strEQ_ID = arrEQLIST[j].ToString();
				// 전체 유니크 LOT_ID 리스트 구함
				drEQFiltered = dtAll.Select("EQ_ID ='"+strEQ_ID+"'","LOT_ID");
				strOldLOT_ID = CST_STRING_EMPTY;
				for(int k=0;k<drEQFiltered.Length;k++)
				{
					strNewLOT_ID = drEQFiltered[k]["LOT_ID"].ToString();
					if(strNewLOT_ID != strOldLOT_ID)
					{
						arrLOTLIST.Add(strNewLOT_ID);
					}
					strOldLOT_ID = strNewLOT_ID;
				}

				// INSPECTED 유니크 LOT_ID 리스트 구함
				drEQFiltered = dtInspected.Select("EQ_ID ='"+strEQ_ID+"'","LOT_ID");
				strOldInspectedLOT_ID = CST_STRING_EMPTY;
				for(int n=0;n<drEQFiltered.Length;n++)
				{
					strNewInspectedLOT_ID = drEQFiltered[n]["LOT_ID"].ToString();
					if(strNewInspectedLOT_ID != strOldInspectedLOT_ID)
					{
						arrInspectedLOTLIST.Add(strNewInspectedLOT_ID);
					}
					strOldInspectedLOT_ID = strNewInspectedLOT_ID;
				}
				// EQ_ID와 유니크 LOT_ID를 기준으로 유니크 WAFER_ID의 수를 구함.
				strOldWafer_ID = CST_STRING_EMPTY;

				for(int p=0;p<arrInspectedLOTLIST.Count;p++)
				{
					for(int q=0;q<dtFiltered.Rows.Count;q++)
					{
						if(dtFiltered.Rows[q]["EQ_ID"].ToString() == strEQ_ID && dtFiltered.Rows[q]["LOT_ID"].ToString() == arrInspectedLOTLIST[p].ToString())
						{
							strNewWafer_ID = dtFiltered.Rows[q]["WF"].ToString();
							if(strNewWafer_ID != strOldWafer_ID)
							{
								arrWAFER_IDs.Add(strNewWafer_ID);
							}
						}
					}
				}
				//ADD DATA TO SHEET
				for(int m=2;m<intSummaryRowEOF+2;m++)
				{
					strEQNAME = FPSpreadUtil.GetCellText(this.fpsCrossOne.ActiveSheet,m,CST_FPS_COLIDX_EQ_ID);
					if(strEQNAME == strEQ_ID)
					{
						intSumLOT_ID_LEFT =	arrInspectedLOTLIST.Count;
						intSumLOT_ID_RIGHT =  arrLOTLIST.Count;
						intSumWafer = arrWAFER_IDs.Count;
						strSumLOT_ID = intSumLOT_ID_LEFT.ToString() + CST_STRING_UNDERBAR + intSumLOT_ID_RIGHT.ToString();
						FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,m,CST_FPS_COLIDX_LOT_ID,strSumLOT_ID);
						FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,m,CST_FPS_COLIDX_WF,intSumWafer.ToString());
					}
				}
				arrLOTLIST.Clear();
				arrInspectedLOTLIST.Clear();
				arrWAFER_IDs.Clear();
			}
		}
		#endregion
		
		#region ■ SetMainProcessFlowSummaryToSheet(DataTable dtSummary)
		private void SetMainProcessFlowSummaryToSheet(DataTable dtSummary)
		{
			//Add Summary Rows in Sheet
			FPSpreadUtil.AddRow(this.fpsCrossOne.ActiveSheet,0,dtSummary.Rows.Count+CST_FPS_ROWBOF_SUMMARY);
			//Manual DataBinding
		
			for(int i=0;i<dtSummary.Rows.Count;i++)
			{
				// STATIC SHEET FIELD DISPLAY
				FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,i+CST_FPS_ROWBOF_SUMMARY,CST_FPS_COLIDX_EQ_ID,dtSummary.Rows[i]["EQ_ID"].ToString());
				FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,i+CST_FPS_ROWBOF_SUMMARY,CST_FPS_COLIDX_GY,dtSummary.Rows[i]["GY"].ToString());
				FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,i+CST_FPS_ROWBOF_SUMMARY,CST_FPS_COLIDX_TOT,dtSummary.Rows[i]["TOT"].ToString());
				FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,i+CST_FPS_ROWBOF_SUMMARY,CST_FPS_COLIDX_DI,dtSummary.Rows[i]["DI"].ToString());
				FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,i+CST_FPS_ROWBOF_SUMMARY,CST_FPS_COLIDX_DR,dtSummary.Rows[i]["DR"].ToString());
				FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,i+CST_FPS_ROWBOF_SUMMARY,CST_FPS_COLIDX_DENSITY,dtSummary.Rows[i]["DENSITY"].ToString());
				FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,i+CST_FPS_ROWBOF_SUMMARY,CST_FPS_COLIDX_NORV,dtSummary.Rows[i]["NORV"].ToString());
				// END STATIC SHEET FIELD DISPLAY

				// DYNAMIC SHEET FIELD DISPLAY
				for(int j=0;j<dsDynamicField.Tables[0].Rows.Count;j++)
				{
					FPSpreadUtil.SetCellText(this.fpsCrossOne.ActiveSheet,i+CST_FPS_ROWBOF_SUMMARY,j+CST_FPS_STATIC_FIELD_COUNT
						,dtSummary.Rows[i][dsDynamicField.Tables[0].Rows[j]["NAME"].ToString()].ToString());
				}
				// END DYNAMIC SHEET FIELD DISPLAY
			}
			//Calcurate Summary Grand Total And DataBinding

		}
		#endregion
		
		#region ■ GetMainProcessFlowSummary(ref STSheetData oSTSheetData,VIEWFILTER eViewFilter)
		private DataTable GetMainProcessFlowSummary(ref STSheetData oSTSheetData,VIEWFILTER eViewFilter)
		{
			//변수 선언및 초기화
			DataTable dtSummaryData = null;
			DataTable dtSummaryBaseData = null;
			DataRow drSummaryRecord = null;
			DataRow[] drFilteredSummaryBaseData = null;
			DataRow[] drSortedAllData = null;
			string strNew = string.Empty;
			string strOld = string.Empty;
			double dSumGY	= 0.0;
			double dSumTOT	= 0.0;
			int intSumDI	= 0;
			int intSumDR	= 0;
			double dSumDENSITY	= 0.0;
			int intSumNORV  = 0;

			// DYNAMIC FIELD VARIABLES
			int[] intSumTmp = null;
			// END DYNAMIC FIELD VARIABLES
		
			//필터링된 데이타 테이블 선택
			dtSummaryBaseData = SetMainProcessFlowInfoCopyByViewFilter(ref oSTSheetData,eViewFilter);
		
			//서머리 자료구조를 저장할 데이타테이블 초기화
			dtSummaryData = new DataTable();
			// STATIC FIELD
			dtSummaryData.Columns.Add("LOT_ID", Type.GetType("System.String"));
			dtSummaryData.Columns.Add("EQ_ID", Type.GetType("System.String"));
			dtSummaryData.Columns.Add("WF",Type.GetType("System.String"));
			dtSummaryData.Columns.Add("GY", Type.GetType("System.String"));
			dtSummaryData.Columns.Add("TOT", Type.GetType("System.String"));
			dtSummaryData.Columns.Add("DI", Type.GetType("System.String"));
			dtSummaryData.Columns.Add("DR", Type.GetType("System.String"));
			dtSummaryData.Columns.Add("DENSITY", Type.GetType("System.String"));
			dtSummaryData.Columns.Add("NORV", Type.GetType("System.String"));
			// END STATIC FIELD

			// DYNAMIC FIELD 
			for(int b=0;b<dsDefectConfig.Tables[0].Rows.Count;b++)
			{
				dtSummaryData.Columns.Add(dsDefectConfig.Tables[0].Rows[b]["NAME"].ToString(), Type.GetType("System.String"));
			}	
			// END DYNAMIC FIELD
			//장비 유니크 리스트 추출
			strOld = CST_STRING_EMPTY;
			drSortedAllData = oSTSheetData.dtAll.Select("EQ_ID IS NOT NULL","EQ_ID");
			for(int i=0;i<drSortedAllData.Length;i++)
			{
				strNew = drSortedAllData[i]["EQ_ID"].ToString();
				if(strNew != strOld)
				{
					drSummaryRecord = dtSummaryData.NewRow();
					drSummaryRecord["EQ_ID"] = strNew;
					dtSummaryData.Rows.Add(drSummaryRecord);
					strOld = strNew;
				}
			}
			//장비항목별 집계
			for(int j=0;j<dtSummaryData.Rows.Count;j++)
			{
				// STATIC VARIABLE 초기화
				dSumGY		=0.0;
				dSumTOT		=0.0;
				intSumDI	=0;
				intSumDR	=0;
				dSumDENSITY	=0.0;
				intSumNORV	=0;
				// END STATIC VARIABLE 초기화
				// DYNAMIC FIELD VARIABLES 초기화
				intSumTmp = new int[dsDefectConfig.Tables[0].Rows.Count];
				for(int a=0;a<dsDefectConfig.Tables[0].Rows.Count;a++)
				{
					intSumTmp[a] = 0;
				}
				// END DYNAMIC FIELD VARIABLES 초기화
				//Data Filtering
				drFilteredSummaryBaseData = dtSummaryBaseData.Select("EQ_ID = '"+dtSummaryData.Rows[j]["EQ_ID"].ToString()+"'");
				if(drFilteredSummaryBaseData.Length>0)
				{
					for(int k=0;k<drFilteredSummaryBaseData.Length;k++)
					{
						dSumGY	+= Convert.ToDouble(drFilteredSummaryBaseData[k]["GY"] == DBNull.Value ? "0.0" : drFilteredSummaryBaseData[k]["GY"].ToString());
						dSumTOT += Convert.ToDouble(drFilteredSummaryBaseData[k]["TOT"] == DBNull.Value ? "0.0" : drFilteredSummaryBaseData[k]["TOT"].ToString());
						intSumDI += Convert.ToInt32(drFilteredSummaryBaseData[k]["DI"] == DBNull.Value ? "0" : drFilteredSummaryBaseData[k]["DI"].ToString());
						intSumDR += Convert.ToInt32(drFilteredSummaryBaseData[k]["DR"] == DBNull.Value ? "0" : drFilteredSummaryBaseData[k]["DR"].ToString());
						dSumDENSITY += Convert.ToDouble(drFilteredSummaryBaseData[k]["DENSITY"] == DBNull.Value ? "0.0" : drFilteredSummaryBaseData[k]["DENSITY"].ToString());
						intSumNORV += Convert.ToInt32(drFilteredSummaryBaseData[k]["NORV"] == DBNull.Value ? "0" : drFilteredSummaryBaseData[k]["NORV"].ToString());

						for(int c=0;c<dsDynamicField.Tables[0].Rows.Count;c++)
						{
							intSumTmp[c] += Convert.ToInt32(drFilteredSummaryBaseData[k][dsDynamicField.Tables[0].Rows[c]["NAME"].ToString()] == DBNull.Value ? "0" : drFilteredSummaryBaseData[k][dsDynamicField.Tables[0].Rows[c]["NAME"].ToString()].ToString());
						}
						//항목별 집계저장
						// STATIC FIELDS
						dtSummaryData.Rows[j]["GY"] = Convert.ToString(dSumGY/drFilteredSummaryBaseData.Length);
						dtSummaryData.Rows[j]["TOT"] = Convert.ToString(dSumTOT/drFilteredSummaryBaseData.Length);
						dtSummaryData.Rows[j]["DI"] = Convert.ToString(intSumDI/drFilteredSummaryBaseData.Length);
						dtSummaryData.Rows[j]["DR"] = Convert.ToString(intSumDR/drFilteredSummaryBaseData.Length);
						dtSummaryData.Rows[j]["DENSITY"] = Convert.ToString(dSumDENSITY/drFilteredSummaryBaseData.Length);
						dtSummaryData.Rows[j]["NORV"] = Convert.ToString(intSumNORV/drFilteredSummaryBaseData.Length);
						// END STATIC FIELDS
						// DYNAMIC FIELDS
						for(int d=0;d<dsDefectConfig.Tables[0].Rows.Count;d++)
						{
							dtSummaryData.Rows[j][dsDefectConfig.Tables[0].Rows[d]["NAME"].ToString()] = Convert.ToString(intSumTmp[d]/drFilteredSummaryBaseData.Length);
						}
						// END DYNAMIC FIELDS
					}
				}
				else
				{
					//항목별 집계저장
					// STATIC FIELDS
					dtSummaryData.Rows[j]["GY"] = Convert.ToString(dSumGY);
					dtSummaryData.Rows[j]["TOT"] = Convert.ToString(dSumTOT);
					dtSummaryData.Rows[j]["DI"] = Convert.ToString(intSumDI);
					dtSummaryData.Rows[j]["DR"] = Convert.ToString(intSumDR);
					dtSummaryData.Rows[j]["DENSITY"] = Convert.ToString(dSumDENSITY);
					dtSummaryData.Rows[j]["NORV"] = Convert.ToString(intSumNORV);
					// END STATIC FIELDS
					// DYNAMIC FIELDS
					for(int d=0;d<dsDefectConfig.Tables[0].Rows.Count;d++)
					{
						dtSummaryData.Rows[j][dsDefectConfig.Tables[0].Rows[d]["NAME"].ToString()] = Convert.ToString(intSumTmp[d]);
					}
					// END DYNAMIC FIELDS
				}
			}
			return dtSummaryData;
		}
		#endregion

		#region ■ SetDataFilteredDisplay()
		private void SetDataFilteredDisplay()
		{
			VIEWFILTER eViewFilter;
			STSheetData oSTSheetData;
			DataTable dtFiltered = null;
			DataTable dtSummary = null;
			int intSummaryRowCnt = 0;
			//Excel Chart 계산을 위한 SHEET HEADER ROW 포지션초기화
			this.fpsCrossOne.ActiveSheet.Tag = "0";

			if(this.fpsCrossOne.ActiveSheet.Rows.Count == 1)
			{return;}

			this.fpsCrossOne.ActiveSheet.Rows.Count = 0;
			//SET VIEW FILTERING 
			if(this.chkInspectedOnly.Checked == true && this.chkClassifiedOnly.Checked == true) 
			{eViewFilter = VIEWFILTER.Inspected_Classified;}
			else if(this.chkInspectedOnly.Checked == true && this.chkClassifiedOnly.Checked == false) //INSPECTED ONLY
			{eViewFilter = VIEWFILTER.Inspected_NotClassified;}
			else if(this.chkInspectedOnly.Checked == false && this.chkClassifiedOnly.Checked == true) //CLASSIFIED ONLY
			{eViewFilter = VIEWFILTER.NotInspected_Classified;}
			else{eViewFilter = VIEWFILTER.NotInspected_NotClassified;} //ALL DATA
			for(int i=0;i<oSheetDataCollection.Count;i++)
			{
				if(oSheetDataCollection[i].SheetName == this.fpsCrossOne.ActiveSheet.SheetName)
				{
					oSTSheetData = oSheetDataCollection[i];
					dtFiltered = SetMainProcessFlowInfoCopyByViewFilter(ref oSTSheetData,eViewFilter);
					FPSpreadUtil.DataBind(this.fpsCrossOne.ActiveSheet,dtFiltered,0,arrHeaderIndex,arrHeaderText);
					//서머리 데이타 바인딩.
					dtSummary = GetMainProcessFlowSummary(ref oSTSheetData,eViewFilter);
					intSummaryRowCnt = dtSummary.Rows.Count;
					//Excel Chart 계산을 위한 SHEET HEADER ROW 포지션을 SHEET TAG에 설정
					this.fpsCrossOne.ActiveSheet.Tag = Convert.ToString(intSummaryRowCnt+1);

					SetMainProcessFlowSummaryToSheet(dtSummary);
					SetMainProcessFlowGrandTotalToSheet(dtSummary);
					//LOT_ID And WAFER TOTAL 
					SetMainProcessFlowLOT_IDAndWFSummaryToSheet(oSTSheetData.dtAll,SetMainProcessFlowInfoCopyByViewFilter(ref oSTSheetData,VIEWFILTER.Inspected_NotClassified)
						,SetMainProcessFlowInfoCopyByViewFilter(ref oSTSheetData,eViewFilter),intSummaryRowCnt);
					//LOT_ID GRAND TOTAL
					SetMainProcessFlowLOT_IDGrandTotalToSheet(intSummaryRowCnt);
					//WAFER GRAND TOTAL
					SetMainProcessFlowWFGrandTotal(intSummaryRowCnt);
					//그리드 헤더추가및 스타일 지정로직.
					SetInitializeGrid(this.fpsCrossOne.ActiveSheet,arrHeaderIndex,arrHeaderText);
					//GRID LINNING.
					if(intSummaryRowCnt > 0)
					{
						if(this.fpsCrossOne.ActiveSheet.Rows.Count >intSummaryRowCnt+2)
						{
							FPSpreadUtil.SetRowColBolder(this.fpsCrossOne.ActiveSheet,intSummaryRowCnt+CST_FPS_ROWBOF_SUMMARY,CST_FPS_COLIDX_WF+1);
						}
					}
					//CHART DATA DISPLAY
					oSTSheetData.intSummaryRowCnt = intSummaryRowCnt;
					SetChartData(ref oSTSheetData,dtFiltered);
					break;
				}
			}
			// SHEET COLUMN FORMAT & CHART FX SETTING
			SetSheetColumnFormatting(intSummaryRowCnt);
		}
		#endregion

		#region ■ SetSheetColumnFormatting(int intSummaryEOF)
		private void SetSheetColumnFormatting(int intSummaryEOF)
		{
			string strOldEQ_ID = string.Empty;
			string strNewEQ_ID = string.Empty;
			string strTemp = string.Empty;
			for(int i=CST_FPS_ROWBOF_SUMMARY-1;i<intSummaryEOF+CST_FPS_ROWBOF_SUMMARY;i++)
			{
				// STATIC SHEET FIELD NUMERIC FORMAT SETTING 
				FPSpreadUtil.SetCellTypeNumeric(this.fpsCrossOne.ActiveSheet,CST_FPS_COLIDX_GY,i,CST_FPS_DECIMALPOINT_1);
				FPSpreadUtil.SetCellTypeNumeric(this.fpsCrossOne.ActiveSheet,CST_FPS_COLIDX_TOT,i,CST_FPS_DECIMALPOINT_2);
				FPSpreadUtil.SetCellTypeNumeric(this.fpsCrossOne.ActiveSheet,CST_FPS_COLIDX_DI,i,CST_FPS_DECIMALPOINT_1);
				FPSpreadUtil.SetCellTypeNumeric(this.fpsCrossOne.ActiveSheet,CST_FPS_COLIDX_DR,i,CST_FPS_DECIMALPOINT_1);
				FPSpreadUtil.SetCellTypeNumeric(this.fpsCrossOne.ActiveSheet,CST_FPS_COLIDX_DENSITY,i,CST_FPS_DECIMALPOINT_1);
				FPSpreadUtil.SetCellTypeNumeric(this.fpsCrossOne.ActiveSheet,CST_FPS_COLIDX_NORV,i,CST_FPS_DECIMALPOINT_1);
				// END STATIC SHEET FIELD NUMERIC FORMAT SETTING 
				// DYNAMIC SHEET FIELD NUMERIC FORMAT SETTING 
				for(int h=0;h<dsDynamicField.Tables[0].Rows.Count;h++)
				{
					FPSpreadUtil.SetCellTypeNumeric(this.fpsCrossOne.ActiveSheet,h+CST_FPS_STATIC_FIELD_COUNT,i,CST_FPS_DECIMALPOINT_1);
				}
				// END DYNAMIC SHEET FIELD NUMERIC FORMAT SETTING 
			}
			//FONT SETTING
			FPSpreadUtil.SetCellFontBold(this.fpsCrossOne.ActiveSheet,0,0,this.fpsCrossOne.ActiveSheet.Columns.Count,intSummaryEOF+2);	
			//COLOR SETTING
			FPSpreadUtil.SetCellFontColor(this.fpsCrossOne.ActiveSheet,0,1,this.fpsCrossOne.ActiveSheet.Columns.Count-1,intSummaryEOF+1,Color.Blue);
			//장비별 시작지점 COLOR SETTING

			//데이타가 있을경우만.
			if(this.fpsCrossOne.ActiveSheet.Rows.Count > CST_FPS_ROWBOF_SUMMARY+2)
			{
				strOldEQ_ID = CST_STRING_EMPTY;
				for(int j=intSummaryEOF+2;j<this.fpsCrossOne.ActiveSheet.Rows.Count;j++)
				{
					strNewEQ_ID = FPSpreadUtil.GetCellText(this.fpsCrossOne.ActiveSheet,j,CST_FPS_COLIDX_EQ_ID);
					if(strNewEQ_ID != strOldEQ_ID)
					{
						FPSpreadUtil.SetCellFontColor(this.fpsCrossOne.ActiveSheet,0,j,this.fpsCrossOne.ActiveSheet.Columns.Count-1
							,j,Color.Pink);
						strOldEQ_ID = strNewEQ_ID;
					}
				}		
			}
		}
		#endregion

		#region ■ SetChartData(ref STSheetData oSTSheetData,DataTable dtFiltered)
		private void SetChartData(ref STSheetData oSTSheetData,DataTable dtFiltered)
		{
			string strOld = string.Empty;
			string strNew = string.Empty;
			string strValue = string.Empty;
			string strTmpColName = string.Empty;
			DataRow[] drFiltered = null;

			int intSummaryRowCount = 0;
			int intTmpColIdx = 0;
			ArrayList arrEQ = null;
			SoftwareFX.ChartFX.Chart oChartDEFECTIVEDIE= null;	
			SoftwareFX.ChartFX.Chart oChartDEFECT= null;
			SoftwareFX.ChartFX.Chart oChart = null;

			try
			{
				drFiltered = dtFiltered.Select("EQ_ID IS NOT NULL","EVENT_TM,LOT_ID");
				
				intSummaryRowCount = oSTSheetData.intSummaryRowCnt;
				arrEQ = new ArrayList();
				oSTSheetData.arrChartCollection = new ArrayList();
				
				//장비 유니크 리스트 추출
				strOld = CST_STRING_EMPTY;
				for(int i=intSummaryRowCount+2;i<this.fpsCrossOne.ActiveSheet.Rows.Count;i++)
				{
					strNew  = FPSpreadUtil.GetCellText(this.fpsCrossOne.ActiveSheet,i,0);
					if(strNew != strOld)
					{
						arrEQ.Add(strNew);
						strOld = strNew;
					}
				}
				//1.DEFECTIVE DIE COUNT(DEFAULT)
				oChartDEFECTIVEDIE = new SoftwareFX.ChartFX.Chart(); 
				oChartDEFECTIVEDIE.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
				oChartDEFECTIVEDIE.OpenData(SoftwareFX.ChartFX.COD.Values, arrEQ.Count,0);
				oChartDEFECTIVEDIE.Gallery = SoftwareFX.ChartFX.Gallery.Lines;
				oChartDEFECTIVEDIE.SerLegBox = true;
				oChartDEFECTIVEDIE.Titles[0].Text = "Defect Die Count";
				oChartDEFECTIVEDIE.Dock = System.Windows.Forms.DockStyle.Top;
				//2.DEFEC COUNT(DEFAULT)
				oChartDEFECT = new SoftwareFX.ChartFX.Chart(); 
				oChartDEFECT.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
				oChartDEFECT.OpenData(SoftwareFX.ChartFX.COD.Values, arrEQ.Count,0);
				oChartDEFECT.Gallery = SoftwareFX.ChartFX.Gallery.Lines;
				oChartDEFECT.SerLegBox = true;
				oChartDEFECT.Titles[0].Text = "Defect Count";
				oChartDEFECT.Dock = System.Windows.Forms.DockStyle.Top;

				//SERIZE LAVEL SETTING
				for(int y=0;y<arrEQ.Count;y++)
				{
					oChartDEFECTIVEDIE.Series[y].Legend = arrEQ[y].ToString();
					oChartDEFECT.Series[y].Legend = arrEQ[y].ToString();
				}
				//DATA SETTING
				for(int j=0;j<arrEQ.Count;j++)
				{

					for(int i=0;i<drFiltered.Length;i++)
					{
						oChartDEFECTIVEDIE.Legend[i] = drFiltered[i]["EVENT_TM"].ToString();
						oChartDEFECT.Legend[i] = drFiltered[i]["EVENT_TM"].ToString();

						if(arrEQ[j].ToString() == drFiltered[i]["EQ_ID"].ToString())
						{
							oChartDEFECTIVEDIE.Value[j,i] = Convert.ToDouble(drFiltered[i][CST_FPS_COLIDX_DR+1].ToString() == "" ? "0.0" : drFiltered[i][CST_FPS_COLIDX_DR+1].ToString());
							oChartDEFECT.Value[j,i] = Convert.ToDouble(drFiltered[i][CST_FPS_COLIDX_TOT+1].ToString() == "" ? "0.0" : drFiltered[i][CST_FPS_COLIDX_TOT+1].ToString());	
						}
					}
				}
				// DYNAMIC COLUMN CHART SETTING
				if(arrUserConfiguredDynamicChartItem != null)
				{
					for(int z=arrUserConfiguredDynamicChartItem.Length;z>0;z--)
					{
						for(int w=0;w<this.fpsCrossOne.ActiveSheet.Columns.Count;w++)
						{
							if(FPSpreadUtil.GetCellText(this.fpsCrossOne.ActiveSheet,0,w) == arrUserConfiguredDynamicChartItem[z-1])
							{
								// w is field index
								intTmpColIdx = w;
								strTmpColName = arrUserConfiguredDynamicChartItem[z-1];
								break;
							}
						}
						if(strTmpColName != string.Empty)
						{
							oChart = new SoftwareFX.ChartFX.Chart(); 
							oChart.ClearData(SoftwareFX.ChartFX.ClearDataFlag.XValues);
							oChart.OpenData(SoftwareFX.ChartFX.COD.Values, arrEQ.Count, 0);
							oChart.Gallery = SoftwareFX.ChartFX.Gallery.Lines;
							oChart.SerLegBox = true;
							oChart.Titles[0].Text = strTmpColName + " Count";
							oChart.Dock = System.Windows.Forms.DockStyle.Top;
							for(int y=0;y<arrEQ.Count;y++)
							{
								oChart.Series[y].Legend = arrEQ[y].ToString();
							}
							for(int j=0;j<arrEQ.Count;j++)
							{

								for(int i=0;i<drFiltered.Length;i++)
								{
									oChart.Legend[i] = drFiltered[i]["EVENT_TM"].ToString();
									if(arrEQ[j].ToString() == drFiltered[i]["EQ_ID"].ToString())
									{
										oChart.Value[j,i] = Convert.ToDouble(drFiltered[i][intTmpColIdx+1].ToString() == "" ? "0.0" : drFiltered[i][intTmpColIdx+1].ToString());
									}
								}
							}
							oSTSheetData.arrChartCollection.Add(oChart);
							strTmpColName = string.Empty;	
						}
					}
				}
				oSTSheetData.arrChartCollection.Add(oChartDEFECT);
				oSTSheetData.arrChartCollection.Add(oChartDEFECTIVEDIE);
				// ADD CHART TO TAP PANNER
				for(int k=0;k<this.tabChart.TabPages.Count;k++)
				{
					if(this.tabChart.TabPages[k].Text == oSTSheetData.SheetName)
					{
						// CLEAR TAB
						this.tabChart.TabPages[k].Controls[0].Controls.Clear();
						// LOOPING arrChartCollection
						for(int m=0;m<oSTSheetData.arrChartCollection.Count;m++)
						{
							this.tabChart.TabPages[k].Controls[0].Controls.Add((SoftwareFX.ChartFX.Chart)oSTSheetData.arrChartCollection[m]);
						}
						break;
					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		#endregion

		#endregion

		#region ★ 구성기준정보및 사용자 설정 정보 관련 메소드 모음

		#region ■  GetConfig()
		private DataSet GetConfig()
		{
			DataSet dsDfClsConfig = null;
			Remoting oRemoting = null;
			string[] paramValues = null;

			//DEFECT CLASS FIELD DYNAMIC CONFIGURATION INFO QUERY
			paramValues = new string[1];
			paramValues[0] = "CROSSRPT_ONE_DEFECT_FIELD_SHEET";
			oRemoting = new Remoting();
			dsDfClsConfig = oRemoting.GetConfig(strUserID,"GetConfig",paramValues);

			return dsDfClsConfig;
		}
		#endregion

		#region ■ GetUserSheetConfig()
		private DataSet GetUserSheetConfig()
		{
			DataSet dsDfClsConfig = null;
			Remoting oRemoting = null;
			string[] paramValues = null;

			//DEFECT CLASS FIELD DYNAMIC CONFIGURATION INFO QUERY
			paramValues = new string[3];
			paramValues[0] = "CROSSRPT_ONE";
			paramValues[1] = strUserID;
			paramValues[2] = "DF_CLASS";	
			oRemoting = new Remoting();
			dsDfClsConfig = oRemoting.GetConfig(strUserID,"GetUserConfig",paramValues);

			return dsDfClsConfig;
		}
		#endregion

		#region ■ GetUserChartConfig()
		private DataSet GetUserChartConfig()
		{
			DataSet dsDfClsConfig = null;
			Remoting oRemoting = null;
			string[] paramValues = null;

			//DEFECT CLASS FIELD DYNAMIC CONFIGURATION INFO QUERY
			paramValues = new string[3];
			paramValues[0] = "CROSSRPT_ONE";
			paramValues[1] = strUserID;
			paramValues[2] = "CHART_CLASS";
			oRemoting = new Remoting();
			dsDfClsConfig = oRemoting.GetConfig(strUserID,"GetUserConfig",paramValues);

			return dsDfClsConfig;
		}
		#endregion

		#region ■ GetUserEQConfig()
		private DataSet GetUserEQConfig()
		{
			DataSet dsDfClsConfig = null;
			Remoting oRemoting = null;
			string[] paramValues = null;

			//DEFECT CLASS FIELD DYNAMIC CONFIGURATION INFO QUERY
			paramValues = new string[3];
			paramValues[0] = "CROSSRPT_ONE";
			paramValues[1] = strUserID;
			paramValues[2] = "EQLIST";
			oRemoting = new Remoting();
			dsDfClsConfig = oRemoting.GetConfig(strUserID,"GetUserConfig",paramValues);

			return dsDfClsConfig;
		}
		#endregion

		#region ■ SetUserEQConfig(USER_CONFIG_QUERY_TYPE eUSER_CONFIG_QUERY_TYPE)
		private void SetUserEQConfig(USER_CONFIG_QUERY_TYPE eUSER_CONFIG_QUERY_TYPE)
		{
			Remoting oRemoting = null;
			string[] paramValues = null;
			string strDynamicElements = string.Empty;
			// INSERT = 1.CATEGORY 2.USER_ID 3.NAME 4.VALUE_A 5.VALUE_B
			// UPDATE = 1.VALUE_A  2.CATEGORY 3.USER_ID 4.NAME 
			try
			{
				for(int i=0;i<this.lvEQList.Items.Count;i++)
				{
					if(lvEQList.Items[i].Checked == true)
					{
						strDynamicElements += lvEQList.Items[i].Text + ";";
					}
				}
				if(strDynamicElements.Length >0)
				{
					strDynamicElements = strDynamicElements.Substring(0,strDynamicElements.Length -1);
				}
				switch(eUSER_CONFIG_QUERY_TYPE)
				{
					case USER_CONFIG_QUERY_TYPE.INSERT:

						paramValues = new string[5];
						paramValues[0] = "CROSSRPT_ONE";
						paramValues[1] = strUserID;
						paramValues[2] = "EQLIST";
						paramValues[3] = strDynamicElements;
						paramValues[4] = " ";
						oRemoting = new Remoting();
						oRemoting.InsertUserConfig(strUserID,"InsertUserConfig",paramValues);
						break;
					case USER_CONFIG_QUERY_TYPE.UPDATE:
						paramValues = new string[4];
						paramValues[0] = strDynamicElements;
						paramValues[1] = "CROSSRPT_ONE";
						paramValues[2] = strUserID;
						paramValues[3] = "EQLIST";
						oRemoting = new Remoting();
						oRemoting.InsertUserConfig(strUserID,"UpdateUserConfig",paramValues);
						break;
				}
			}
			catch(Exception ex)
			{MessageBox.Show(ex.Message);}
		}
		#endregion

		#region ■ SetSheetDynamicConfiguration(string[] arrDynamicConfiguration)
		public void SetSheetDynamicConfiguration(string[] arrDynamicConfiguration)
		{
			arrUserConfiguredDynamicField = arrDynamicConfiguration;
			SetSheetHeaderArrange(SHEETVIEW_INITYPE.TYPE_DYNAMIC);
			SetDataFilteredDisplay();
		}
		#endregion

		#region ■ SetChartDynamicConfiguration(string[] arrDynamicConfiguration)
		public void SetChartDynamicConfiguration(string[] arrDynamicConfiguration)
		{
			//Chart ReDrawing
			//어레이 데이타를 차트 드로잉시에 반영한다.
			arrUserConfiguredDynamicChartItem = arrDynamicConfiguration;
			SetDataFilteredDisplay();
		}
		#endregion

		#endregion

		#region ★ 엑셀 출력 메소드 모음

		#region ■ ExportSheetToExcel(ArrayList arrSheetList,bool bExportChart,bool bIsExcelChart)
		public void ExportSheetToExcel(ArrayList arrSheetList,bool bExportChart,bool bIsExcelChart)
		{
			//0.객체 선언 , 인스턴싱
			string[,] strExportList = null;
			int[,] intExportList = null;
			double[,] dblExportList = null;
			DateTime[,] dteExportList = null;
			string strTmp = string.Empty;
			int intSheetDataRowBOF = 0;
			int intSheetFullRowCnt = 0;
			int intSheetFullColCnt = 0;
			int intChartCnt = 0;
			string strOld = string.Empty;
			string strNew = string.Empty;
			Miracom.DMS.Win.ExcelUtil oXL = null;
			Excel._Workbook oWB = null;
			string[] arrSplits = null;
			string[] arrSplitsDetail = null;
			int iYear = 0;
			int iMonth = 0;
			int iDay = 0;
			int iHH = 0;
			int iMM = 0;
			DateTime dtTmp;
			string strTmpColName = string.Empty;
			int intTmpColIdx = 0;
			int iExcelChartPosition = 0;

			try
			{
				//1.엑셀 오브젝트 얻어 오기	
				oXL = new Miracom.DMS.Win.ExcelUtil();
				oWB = oXL.fnGetExcelWorkbook(true,arrSheetList.Count);
			
				for(int k=0;k<arrSheetList.Count;k++)
				{
					for(int m=0;m<fpsCrossOne.Sheets.Count;m++)
					{
						if(arrSheetList[k].ToString() == fpsCrossOne.Sheets[m].SheetName && fpsCrossOne.Sheets[m].Rows.Count > 2)
						{
							intSheetDataRowBOF = Convert.ToInt16(fpsCrossOne.Sheets[m].Tag.ToString());
							intSheetDataRowBOF += 2;//헤더 토탈 포함.
							intSheetFullRowCnt = fpsCrossOne.Sheets[m].RowCount;
							intSheetFullColCnt = fpsCrossOne.Sheets[m].ColumnCount;
							if(intSheetFullRowCnt >1)
							{
								for(int i=0;i<intSheetDataRowBOF-1;i++)
								{
									if(i==0) //헤더 설정
									{
										strExportList=new string[1,intSheetFullColCnt];
										for(int j=0;j<intSheetFullColCnt;j++)
										{
											strExportList[0,j]=fpsCrossOne.Sheets[m].GetText(0,j).ToString();
										}
										oXL.fnSetValue(oWB,k+1,3,1,3,intSheetFullColCnt,strExportList,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.gray);
									}
									else if(i>0 && i<intSheetDataRowBOF-1) //서머리 설정
									{
										strExportList=new string[1,intSheetFullColCnt];
										for(int j=0;j<intSheetFullColCnt;j++)
										{
											strExportList[0,j]=fpsCrossOne.Sheets[m].GetText(i,j).ToString();
										}
										oXL.fnSetValue(oWB,k+1,3+i,1,3+i,intSheetFullColCnt,strExportList,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.yellow);
									}
								}
								for(int x=0;x<intSheetFullColCnt;x++) //데이터 설정 ,컬럼으로 설정
								{
									//INT 타입으로 바꿔야할 컬럼들
									if( x >= CST_FPS_COLIDX_TOT  && x != CST_FPS_COLIDX_DENSITY)
									{
										intExportList = new int[intSheetFullRowCnt-intSheetDataRowBOF+1,1];
										for(int y=intSheetDataRowBOF;y<intSheetFullRowCnt+1;y++)
										{
											strTmp = fpsCrossOne.Sheets[m].GetText(y-1,x).ToString();
											if(strTmp == null)strTmp = "";
											strTmp = strTmp == "" ? "0" : strTmp;
											intExportList[y-intSheetDataRowBOF,0] = Convert.ToInt32(strTmp);
										}
										oXL.fnSetValue(oWB,k+1,intSheetDataRowBOF+2,x+1,intSheetFullRowCnt+2,x+1,intExportList,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);
									}
									else if(x == CST_FPS_COLIDX_DENSITY || x == CST_FPS_COLIDX_GY)
									{
										dblExportList = new double[intSheetFullRowCnt-intSheetDataRowBOF+1,1];
										for(int y=intSheetDataRowBOF;y<intSheetFullRowCnt+1;y++)
										{
											strTmp = fpsCrossOne.Sheets[m].GetText(y-1,x).ToString();
											if(strTmp == null)strTmp = "";
											strTmp = strTmp == "" ? "0.0" : strTmp;
											dblExportList[y-intSheetDataRowBOF,0] = Convert.ToDouble(strTmp);
										}
										oXL.fnSetValue(oWB,k+1,intSheetDataRowBOF+2,x+1,intSheetFullRowCnt+2,x+1,dblExportList,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);									
									}
									else if(x == CST_FPS_COLIDX_EVENT_STARTDT)
									{
										dteExportList = new DateTime[intSheetFullRowCnt-intSheetDataRowBOF+1,1];
										for(int y=intSheetDataRowBOF;y<intSheetFullRowCnt+1;y++)
										{
											strTmp = fpsCrossOne.Sheets[m].GetText(y-1,x).ToString();

											arrSplits = strTmp.Split(Convert.ToChar(" "));
											arrSplitsDetail = arrSplits[0].Split(Convert.ToChar("/"));
											iYear = Convert.ToInt16("20"+arrSplitsDetail[0]);
											iMonth = Convert.ToInt16(arrSplitsDetail[1]);
											iDay = Convert.ToInt16(arrSplitsDetail[2]);

											arrSplitsDetail = arrSplits[1].Split(Convert.ToChar(":"));
											iHH = Convert.ToInt16(arrSplitsDetail[0]);
											iMM = Convert.ToInt16(arrSplitsDetail[1]);
											dtTmp = new DateTime(iYear,iMonth,iDay,iHH,iMM,0);

											dteExportList[y-intSheetDataRowBOF,0] = dtTmp;
										}
										oXL.fnSetValue(oWB,k+1,intSheetDataRowBOF+2,x+1,intSheetFullRowCnt+2,x+1,dteExportList,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);									
									}
									else
									{
										strExportList = new string[intSheetFullRowCnt-intSheetDataRowBOF+1,1];
										for(int y=intSheetDataRowBOF;y<intSheetFullRowCnt+1;y++)
										{
											strTmp = fpsCrossOne.Sheets[m].GetText(y-1,x).ToString();
											strExportList[y-intSheetDataRowBOF,0] = strTmp == null ? "" : strTmp;
										}
										oXL.fnSetValue(oWB,k+1,intSheetDataRowBOF+2,x+1,intSheetFullRowCnt+2,x+1,strExportList,false,"굴림체",10,Miracom.DMS.Win.ExcelUtil.Color.black,Miracom.DMS.Win.ExcelUtil.Alignment.xlVAlignCenter, Miracom.DMS.Win.ExcelUtil.HAlignment.xlHAlignLeft, true, Miracom.DMS.Win.ExcelUtil.Color.white);
									}
							
								}
								// 차트 포함여부 체크
								if(bExportChart)
								{
									//엑셀차트인지 차트 FX 인지 구분하여 EXPORT함.
									if(bIsExcelChart)
									{
										//Defect Count
										iExcelChartPosition = 1;
										ExportExcelChart(oXL,intSheetDataRowBOF,intSheetFullRowCnt,m,k,oWB,CST_FPS_COLIDX_TOT,iExcelChartPosition,"Defect Count");
										//Defective Die Count
										iExcelChartPosition = 2;
										ExportExcelChart(oXL,intSheetDataRowBOF,intSheetFullRowCnt,m,k,oWB,CST_FPS_COLIDX_DR,iExcelChartPosition,"Defective Die Count");
										//Dynamic Excel Chart
										if(arrUserConfiguredDynamicChartItem != null)
										{
											for(int z=0;z<arrUserConfiguredDynamicChartItem.Length;z++)
											{
												iExcelChartPosition += 1;
												for(int w=0;w<this.fpsCrossOne.ActiveSheet.Columns.Count;w++)
												{
													if(FPSpreadUtil.GetCellText(fpsCrossOne.Sheets[m],0,w) == arrUserConfiguredDynamicChartItem[z])
													{
														// w is field index
														intTmpColIdx = w;
														strTmpColName = arrUserConfiguredDynamicChartItem[z];
														break;
													}
												}
												if(strTmpColName != string.Empty)
												{
													//존재하면 차트를 그리면 된다.
													ExportExcelChart(oXL,intSheetDataRowBOF,intSheetFullRowCnt,m,k,oWB,intTmpColIdx,iExcelChartPosition,strTmpColName);
													strTmpColName = string.Empty;	
												}
											}
										}								
									}
									else
									{
										//CHART FX 익스포트							
										intChartCnt = this.tabChart.TabPages[k].Controls[0].Controls.Count;
										for(int n=0;n<intChartCnt;n++)
										{
											oXL.fnSetChartFx(oWB,k+1,(SoftwareFX.ChartFX.Chart)this.tabChart.TabPages[k].Controls[0].Controls[intChartCnt-n-1],
												fpsCrossOne.Sheets[k].RowCount+3+(n*10),3,1.0f, 1.0f);
										}				
									}
								}
								oXL.fnSetSheetName(oWB,k+1,fpsCrossOne.Sheets[k].SheetName);
								break;
							}
						}
					}
				}
			}
			catch(Exception Ex)
			{
				MessageBox.Show(Ex.Message);
			}
			finally
			{
				oXL = null;
				Miracom.DMS.Win.ExcelUtil.fnExcelProcessExit();	
			}
		}
		#endregion

		#region ■ ExportExcelChart(Miracom.DMS.Win.ExcelUtil oXL,int intSheetDataRowBOF,int intSheetFullRowCnt,int m,int k,Excel._Workbook oWB,int iDataColumn,int iPositionCnt,string strChartName)
		private void ExportExcelChart(Miracom.DMS.Win.ExcelUtil oXL,int intSheetDataRowBOF,int intSheetFullRowCnt,int m,int k,Excel._Workbook oWB,int iDataColumn,int iPositionCnt,string strChartName)
		{
			ArrayList arrEQIndex = null;
			ArrayList arrEQName = null;
			string strOld = string.Empty;
			string strNew = string.Empty;
			Excel.ChartObjects oChartObjects = null;
			Excel.ChartObject oChartObj = null;
			Excel._Chart oChartInst = null;
			Excel.Worksheet thisWorksheet  = null;
			Excel.Range oRangeValue = null;
			Excel.Range oRangeDate = null;
			Excel.SeriesCollection oSeriesCollection = null;
			Excel.Series oSeries = null;

			arrEQIndex = new ArrayList();
			arrEQName = new ArrayList();
								
			strOld = CST_STRING_EMPTY;
			for(int z=intSheetDataRowBOF;z<intSheetFullRowCnt+1;z++)
			{
				strNew  = fpsCrossOne.Sheets[m].GetText(z-1,0).ToString();
				if(strNew != strOld)
				{
					arrEQIndex.Add(z);
					arrEQName.Add(strNew);
					strOld = strNew;
				}
			}
			thisWorksheet = (Excel.Worksheet)oWB.Sheets[k+1] ;
			oChartObjects = (Excel.ChartObjects)thisWorksheet.ChartObjects(Type.Missing);

			//oChartInst.Name = "Defect Cnt"; 반영시 엑셀시스템 오류 에러남
			oChartObj = oChartObjects.Add(50,((intSheetFullRowCnt/10)*135)+(iPositionCnt*250),500,200);
			oChartInst = oChartObj.Chart;
		
			for(int w=0;w<arrEQIndex.Count;w++)
			{
				if(w == 0)
				{
					if(arrEQIndex.Count == 1)
					{
						oRangeValue = oXL.fnGetRange(oWB,k+1,Convert.ToInt16(arrEQIndex[w])+2,iDataColumn+1,intSheetFullRowCnt+2,iDataColumn+1);
						oRangeDate = oXL.fnGetRange(oWB,k+1,Convert.ToInt16(arrEQIndex[w])+2,CST_FPS_COLIDX_EVENT_STARTDT+1,intSheetFullRowCnt+2,CST_FPS_COLIDX_EVENT_STARTDT+1);					
					}
					else
					{
						oRangeValue = oXL.fnGetRange(oWB,k+1,Convert.ToInt16(arrEQIndex[w])+2,iDataColumn+1,Convert.ToInt16(arrEQIndex[w+1])-1+2,iDataColumn+1);
						oRangeDate = oXL.fnGetRange(oWB,k+1,Convert.ToInt16(arrEQIndex[w])+2,CST_FPS_COLIDX_EVENT_STARTDT+1,Convert.ToInt16(arrEQIndex[w+1])-1+2,CST_FPS_COLIDX_EVENT_STARTDT+1);
					}
					oRangeDate.NumberFormat = "YY/MM/DD HH:MM";

					oChartInst.ChartWizard(oRangeValue,Excel.XlChartType.xlXYScatter,Type.Missing,
						Excel.XlRowCol.xlColumns,Type.Missing,Type.Missing,Type.Missing,strChartName,Type.Missing,Type.Missing,Type.Missing);
					oSeries = (Excel.Series)oChartInst.SeriesCollection(1);
					oSeries.XValues = oRangeDate;
					oSeries.Name = arrEQName[w].ToString();		
				}
				else
				{
					try
					{
						oSeriesCollection = (Excel.SeriesCollection)oChartInst.SeriesCollection(Type.Missing);
						if(w != arrEQIndex.Count-1)
						{
							try
							{
								oRangeValue =oXL.fnGetRange(oWB,k+1,Convert.ToInt16(arrEQIndex[w])+2,iDataColumn+1,Convert.ToInt16(arrEQIndex[w+1])-1+2,iDataColumn+1);
								oSeriesCollection.Add(oRangeValue,Excel.XlRowCol.xlColumns,Type.Missing,Type.Missing,Type.Missing);	
							}
							catch(Exception ex)
							{if(ex.GetType().Name.ToString() != "InvalidCastException"){throw ex;}}
							try
							{
								oRangeDate = oXL.fnGetRange(oWB,k+1,Convert.ToInt16(arrEQIndex[w])+2,CST_FPS_COLIDX_EVENT_STARTDT+1,Convert.ToInt16(arrEQIndex[w+1])-1+2,CST_FPS_COLIDX_EVENT_STARTDT+1);
								oRangeDate.NumberFormat = "YY/MM/DD HH:MM";
								oSeries = (Excel.Series)oChartInst.SeriesCollection(w+1);
								oSeries.XValues = oRangeDate;
								oSeries.Name = arrEQName[w].ToString();
							}
							catch(Exception ex)
							{if(ex.GetType().Name.ToString() != "InvalidCastException"){throw ex;}}
						}
						else
						{
							try
							{
								oRangeValue =oXL.fnGetRange(oWB,k+1,Convert.ToInt16(arrEQIndex[w])+2,iDataColumn+1,intSheetFullRowCnt+2,iDataColumn+1);
								oSeriesCollection.Add(oRangeValue,Excel.XlRowCol.xlColumns,Type.Missing,Type.Missing,Type.Missing);	
							}
							catch(Exception ex)
							{if(ex.GetType().Name.ToString() != "InvalidCastException"){throw ex;}}
							try
							{
								oRangeDate = oXL.fnGetRange(oWB,k+1,Convert.ToInt16(arrEQIndex[w])+2,CST_FPS_COLIDX_EVENT_STARTDT+1,intSheetFullRowCnt+2,CST_FPS_COLIDX_EVENT_STARTDT+1);
								oRangeDate.NumberFormat = "YY/MM/DD HH:MM";
								oSeries = (Excel.Series)oChartInst.SeriesCollection(w+1);
								oSeries.XValues = oRangeDate;
								oSeries.Name = arrEQName[w].ToString();
							}
							catch(Exception ex)
							{if(ex.GetType().Name.ToString() != "InvalidCastException"){throw ex;}}			
						}
					}
					catch(Exception Ex)
					{
						if(Ex.GetType().Name.ToString() != "InvalidCastException")
						{throw Ex;}
					}	
				}
			}
		}
		#endregion
		
		#endregion
		
		#region ★ 유틸리티 메소드 모음

		#region ■ GetCurrentSTSheetDataIndex()
		private int GetCurrentSTSheetDataIndex()
		{
			int intCurrentSTSheetDataIdx = 0;
			string strActiveSheetName = this.fpsCrossOne.ActiveSheet.SheetName;
			//기 저장된 SheetData가 있으면 컬렉션에서 삭제함.
			for(int i=0;i<oSheetDataCollection.Count;i++)
			{
				if(oSheetDataCollection[i].SheetName == strActiveSheetName)
				{
					intCurrentSTSheetDataIdx = i;
					break;
				}
			}
			return intCurrentSTSheetDataIdx;
		}
		#endregion

		#region ■ GetStepSeqListSelected()
		public string GetStepSeqListSelected()
		{
			ArrayList arrStepSeqs = null;
			STSheetData oSTSheetData;
			DataTable dtOriginalData = null;
			string strEQ_ID = string.Empty;
			string strLOT_ID = string.Empty;
			string strWF = string.Empty;
			string strStepSeq = string.Empty;
			string strParamDetailAnalysis = string.Empty;
			FarPoint.Win.Spread.Model.CellRange[] oSelectedRange = null;

			arrStepSeqs = new ArrayList();
			oSelectedRange = this.fpsCrossOne.ActiveSheet.GetSelections();
			oSTSheetData = oSheetDataCollection[GetCurrentSTSheetDataIndex()];
			dtOriginalData = oSTSheetData.dtAll;

			for(int i=0;i<dtOriginalData.Rows.Count;i++)
			{		
				for(int j=0;j<oSelectedRange.Length;j++)
				{	
					
					for(int k=oSelectedRange[j].Row;k<oSelectedRange[j].Row+oSelectedRange[j].RowCount;k++)
					{
						strEQ_ID = FPSpreadUtil.GetCellText(this.fpsCrossOne.ActiveSheet,k,CST_FPS_COLIDX_EQ_ID);
						strLOT_ID = FPSpreadUtil.GetCellText(this.fpsCrossOne.ActiveSheet,k,CST_FPS_COLIDX_LOT_ID);
						strWF = FPSpreadUtil.GetCellText(this.fpsCrossOne.ActiveSheet,k,CST_FPS_COLIDX_WF);
					
						if(strEQ_ID == dtOriginalData.Rows[i]["EQ_ID"].ToString() && 
							strLOT_ID == dtOriginalData.Rows[i]["LOT_ID"].ToString() &&
							strWF == dtOriginalData.Rows[i]["WF"].ToString() && 
							strWF != null && strWF != "" &&
							dtOriginalData.Rows[i]["STEP_SEQ"].ToString() != null)
						{
							strStepSeq = dtOriginalData.Rows[i]["STEP_SEQ"].ToString();
							arrStepSeqs.Add(strStepSeq);
						}		
					}
				}	
			}
			for(int k=0;k<arrStepSeqs.Count;k++)
			{
				strParamDetailAnalysis += arrStepSeqs[k] + ", ";
			}
			if(arrStepSeqs.Count >0 &&  strParamDetailAnalysis.Length > 0)
			{
				strParamDetailAnalysis = strParamDetailAnalysis.Substring(0,strParamDetailAnalysis.Length-2);
			}
			else
			{strParamDetailAnalysis = "";}
			
			return strParamDetailAnalysis;
		}
		#endregion

		#region ■ LoadDefectMap()
		private void LoadDefectMap()
		{
			// Search Stepseq for Selected wafer : EQ_ID,LOT_ID,WAFER_ID
		}
		#endregion

		#region  ■ GetSheetName()
		private string GetSheetName()
		{
			string strSheetName = string.Empty;
			string strOperDescription = string.Empty;
			string strSheetInstanceName = string.Empty;
			if(this.txtOper.Text.Trim() != CST_STRING_EMPTY && this.txtOper.Text.Trim() != CST_STRING_EMPTY)
			{
				strOperDescription = this.txtOper.Text.Trim().Substring(this.txtOper.Text.IndexOf(CST_STRING_SPACE)+1,this.txtOper.Text.Length -this.txtOper.Text.IndexOf(CST_STRING_SPACE)-1);
				strSheetName = strOperDescription + CST_STRING_UNDERBAR + this.txtStepID.Text.Trim();	
			}
			else
			{
				strSheetName = "UNKNOWN";
			}
			return strSheetName;
		}
		#endregion

		#region  ■ GetSheetIndex(string strSheetName)
		private int GetSheetIndex(string strSheetName)
		{
			string strSheetInstanceName = string.Empty;
			string strSheetInstanceFullName = string.Empty;
			string[] arrTmp = null;
			int intOldIndex = -1;
			int intNewIndex = -1;
			int intReturn = -1;
			ArrayList arrSheetIndexs = new ArrayList();

			for(int i=0;i<fpsCrossOne.Sheets.Count;i++)
			{
				strSheetInstanceFullName = fpsCrossOne.Sheets[i].SheetName;
				strSheetInstanceName = strSheetInstanceFullName.Substring(0, strSheetInstanceFullName.LastIndexOf(CST_STRING_UNDERBAR));
				if(strSheetName == strSheetInstanceName)
				{
					arrTmp = strSheetInstanceFullName.Split(Convert.ToChar(CST_STRING_UNDERBAR));
					arrSheetIndexs.Add(Convert.ToInt16(arrTmp[arrTmp.Length-1]));
				}

			}
			if(arrSheetIndexs.Count >0)
			{
				for(int j=0;j<arrSheetIndexs.Count;j++)
				{
					intNewIndex = Convert.ToInt16(arrSheetIndexs[j].ToString());
					if(intNewIndex > intOldIndex)
					{
						intOldIndex = intNewIndex;
					}
				}
				intReturn = intOldIndex+1;
			}
			else
			{intReturn=0;}
			return intReturn;
		}
		#endregion

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			this.fpsCrossOne.ActiveSheet.ClipboardCopy();
		}

		#endregion



		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			//mapview 보기
//			MessageBox.Show(GetStepSeqListSelected());

			if(DesignMode) return;

			string strStepSeqList = string.Empty;
			Miracom.DMS.LIB.MapAnalysisComm.DefectMapGalleryList mgList;
			strStepSeqList = GetStepSeqListSelected();
			if(strStepSeqList == "")
			{
				Miracom.DMS.Win.FormUtil.DisplayInfoMsg(System.Windows.Forms.MessageBoxIcon.Information,
					"List is empty.");
				return;
			}
			mgList = new Miracom.DMS.LIB.MapAnalysisComm.DefectMapGalleryList();
			mgList.Show();
			mgList.gstrStepSeqList = strStepSeqList;
			mgList.gtmr.Enabled = true;		
		}

		private void CrossOne_Load(object sender, System.EventArgs e)
		{
		
		}

		private void btnStepRefresh_Click(object sender, System.EventArgs e)
		{
			//2005.06.30 CHOO
			SetInspectionStepTree();		
		}

		private void btnProcessFlowRefresh_Click(object sender, System.EventArgs e)
		{
			//2005.06.30 CHOO
			SetProcessFlowTree();
		}
	}

	#region  ★ Class Utility & Data Structure
	public struct STSheetData
	{
		public int intSummaryRowCnt;
		public string SheetName; 
		public DataTable dtAll;
		public bool bExecuted;
		public ArrayList arrChartCollection;
	}
	public class SheetDataCollection : CollectionBase
	{
		public virtual void Add(STSheetData NewSTSheetData)
		{
			this.List.Add(NewSTSheetData);
		}
		public virtual STSheetData this[int Index]
		{
			get
			{
				return (STSheetData)this.List[Index];
			}
		}
	}
	public enum USER_CONFIG_QUERY_TYPE
	{
		INSERT,
		UPDATE
	}
	enum VIEWFILTER
	{
		Inspected_Classified,
		NotInspected_Classified,
		Inspected_NotClassified,
		NotInspected_NotClassified
	}
	enum SHEETVIEW_INITYPE
	{
		TYPE_INITIALIZE,
		TYPE_DYNAMIC,
		TYPE_UNKNOWN
	}
	enum CHARTVIEW_INITYPE
	{
		TYPE_INITIALIZE,
		TYPE_DYNAMIC,
		TYPE_UNKNOWN
	}
	enum EQLISTVIEW_INITYPE
	{
		TYPE_INITIALIZE,
		TYPE_DYNAMIC,
		TYPE_UNKNOWN
	}
	#endregion
}
