using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using DACrux.Data.Parser.Klarf;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// DPUCStepSelect에 대한 요약 설명입니다.
    /// </summary>
    public delegate void Selected(object sender, DACrux.Base.DPWafer[] wafer);

    public class DPUCStepSelect : DACrux.Framework.Base.DACruxCTLBasic01
    {
        string conditionSaveFile = @"dms.ini";
        string SearchOption = "DMSearchOption";
        string strMydocPath = string.Empty;
        bool isView = false;
        bool bEndter = false;

        private System.Windows.Forms.Splitter splitter1;
        private FarPoint.Win.Spread.SheetView fpSelectOption_Sheet1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel pnlRunLoad;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button butRun;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private FarPoint.Win.Spread.FpSpread fpSelectOption;
        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.Button butQuery;
        private System.Windows.Forms.Button butDown;
        private System.Windows.Forms.Button butUp;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.ComponentModel.IContainer components;
        public event Selected OnSelected = null;
        private string m_strSort = string.Empty;
        //private bool m_isMultiSelect = false;
        private string[] m_strField = new string[] { 
                    "PRODUCT", "LOT_ID", "WAFER_ID", "STEP_ID", "INSPECTION_EQ", "RESULTTIMESTAMP", "TEST", "SLOT_ID", 
                    "SETUP_SEQ", "MAIN_EQ", "DEFECTS", "IMAGES", "STEP_SEQ", "WAFER_SEQ" 
                };
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button butStaticSearch;
        private Button butPeriodReflash;
        private Panel panel1;
        private TextBox txtFilterWafer;
        private TextBox txtFilterLot;
        private TextBox txtFilterStep;
        private TextBox txtFilterProduct;
        private TextBox txtFilterEQ;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private ListBox lstWafer;
        private ListBox lstLot;
        private ListBox lstStep;
        private ListBox lstProduct;
        private ListBox lstEQ;

        private DataTable dtStepList = null;
        private DataTable dtStepOrder = null;
        private Label label7;
        private Label label6;
        private Infragistics.Win.UltraWinTree.UltraTree trvWaferList;
        private Panel panel4;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Splitter splitter2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbRecipe;
        private Label label8;
        private Button BtnSave;
        private Button BtnDelete;
        private Button BtnLoad;
        private CheckBox chkDate;
        private Button BtnConfig;
        private CheckBox chkDefectImage;
        private Panel panel5;
        private RadioButton rbtWaferList;
        private RadioButton rbtStepList;
        private Label label9;
        protected Infragistics.Win.UltraWinEditors.UltraComboEditor cmbValueTypes;
        private CheckBox chkLsatInspect;
        DataTable m_dtStatic = null;
        private TabPage tabPage3;
        private Button btnKlarfFileOpen;
        private CheckBox chkOrdered;
        private CheckBox chkWaferID;
        private CheckBox chkLotListFilter;
        private List<string> strWaferList = new List<string>();
        private Button BtnColRefresh;
        private Button btnClear;
        private string strConfigInfo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "DM", "DMSearchAppConfig.ini");

        public DPUCStepSelect()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();

            // TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.
            DmsCacheHelper.InitDmsCache();
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

                if (dtStepList != null)
                    dtStepList.Dispose();

                if (dtStepOrder != null)
                    dtStepOrder.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPUCStepSelect));
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.fpSelectOption_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.pnlRunLoad = new System.Windows.Forms.Panel();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BtnColRefresh = new System.Windows.Forms.Button();
            this.chkLotListFilter = new System.Windows.Forms.CheckBox();
            this.cmbValueTypes = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label9 = new System.Windows.Forms.Label();
            this.rbtWaferList = new System.Windows.Forms.RadioButton();
            this.rbtStepList = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkWaferID = new System.Windows.Forms.CheckBox();
            this.chkOrdered = new System.Windows.Forms.CheckBox();
            this.butRun = new System.Windows.Forms.Button();
            this.fpSelectOption = new FarPoint.Win.Spread.FpSpread();
            this.pnlControl = new System.Windows.Forms.Panel();
            this.butQuery = new System.Windows.Forms.Button();
            this.BtnConfig = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.butDown = new System.Windows.Forms.Button();
            this.butUp = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkLsatInspect = new System.Windows.Forms.CheckBox();
            this.chkDefectImage = new System.Windows.Forms.CheckBox();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.butPeriodReflash = new System.Windows.Forms.Button();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtFilterWafer = new System.Windows.Forms.TextBox();
            this.txtFilterLot = new System.Windows.Forms.TextBox();
            this.txtFilterStep = new System.Windows.Forms.TextBox();
            this.txtFilterProduct = new System.Windows.Forms.TextBox();
            this.txtFilterEQ = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstWafer = new System.Windows.Forms.ListBox();
            this.lstLot = new System.Windows.Forms.ListBox();
            this.lstStep = new System.Windows.Forms.ListBox();
            this.lstProduct = new System.Windows.Forms.ListBox();
            this.lstEQ = new System.Windows.Forms.ListBox();
            this.butStaticSearch = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnKlarfFileOpen = new System.Windows.Forms.Button();
            this.trvWaferList = new Infragistics.Win.UltraWinTree.UltraTree();
            this.panel4 = new System.Windows.Forms.Panel();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.cmbRecipe = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label8 = new System.Windows.Forms.Label();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectOption_Sheet1)).BeginInit();
            this.pnlRunLoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbValueTypes)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectOption)).BeginInit();
            this.pnlControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trvWaferList)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRecipe)).BeginInit();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 628);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(600, 15);
            this.splitter1.TabIndex = 68;
            this.splitter1.TabStop = false;
            // 
            // fpSelectOption_Sheet1
            // 
            this.fpSelectOption_Sheet1.Reset();
            fpSelectOption_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSelectOption_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSelectOption_Sheet1.ColumnHeader.RowCount = 0;
            fpSelectOption_Sheet1.RowHeader.ColumnCount = 0;
            this.fpSelectOption_Sheet1.AutoCalculation = false;
            this.fpSelectOption_Sheet1.AutoGenerateColumns = false;
            this.fpSelectOption_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSelectOption_Sheet1.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSelectOption_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.fpSelectOption_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.DataAutoCellTypes = false;
            this.fpSelectOption_Sheet1.DataAutoHeadings = false;
            this.fpSelectOption_Sheet1.DataAutoSizeColumns = false;
            this.fpSelectOption_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSelectOption_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSelectOption_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.fpSelectOption_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpSelectOption_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpSelectOption_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSelectOption_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSelectOption_Sheet1.SheetCornerStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSelectOption_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSelectOption_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "action_delete.gif");
            // 
            // pnlRunLoad
            // 
            this.pnlRunLoad.Controls.Add(this.fpSpread1);
            this.pnlRunLoad.Controls.Add(this.panel5);
            this.pnlRunLoad.Controls.Add(this.panel3);
            this.pnlRunLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRunLoad.Location = new System.Drawing.Point(0, 643);
            this.pnlRunLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlRunLoad.Name = "pnlRunLoad";
            this.pnlRunLoad.Size = new System.Drawing.Size(600, 293);
            this.pnlRunLoad.TabIndex = 66;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.fpSpread1.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.AsNeeded;
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread1.Location = new System.Drawing.Point(0, 77);
            this.fpSpread1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpread1.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(600, 159);
            this.fpSpread1.TabIndex = 0;
            this.fpSpread1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread1_CellDoubleClick);
            this.fpSpread1.SetActiveViewport(0, -1, -1);
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpread1_Sheet1.ColumnCount = 0;
            fpSpread1_Sheet1.RowCount = 0;
            this.fpSpread1_Sheet1.ActiveColumnIndex = -1;
            this.fpSpread1_Sheet1.ActiveRowIndex = -1;
            this.fpSpread1_Sheet1.AutoCalculation = false;
            this.fpSpread1_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.DataAutoCellTypes = false;
            this.fpSpread1_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpread1_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            this.fpSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread1_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.RowHeader.Columns.Get(0).Width = 83F;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpSpread1_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSpread1_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.SheetCornerStyle.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread1_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSpread1_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.BtnColRefresh);
            this.panel5.Controls.Add(this.chkLotListFilter);
            this.panel5.Controls.Add(this.cmbValueTypes);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.rbtWaferList);
            this.panel5.Controls.Add(this.rbtStepList);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(600, 77);
            this.panel5.TabIndex = 3;
            // 
            // BtnColRefresh
            // 
            this.BtnColRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnColRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnColRefresh.Image = ((System.Drawing.Image)(resources.GetObject("BtnColRefresh.Image")));
            this.BtnColRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnColRefresh.Location = new System.Drawing.Point(4, 36);
            this.BtnColRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnColRefresh.Name = "BtnColRefresh";
            this.BtnColRefresh.Size = new System.Drawing.Size(226, 34);
            this.BtnColRefresh.TabIndex = 6;
            this.BtnColRefresh.Text = "Columns Refresh";
            this.BtnColRefresh.UseVisualStyleBackColor = true;
            this.BtnColRefresh.Visible = false;
            this.BtnColRefresh.Click += new System.EventHandler(this.BtnColRefresh_Click);
            // 
            // chkLotListFilter
            // 
            this.chkLotListFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLotListFilter.AutoSize = true;
            this.chkLotListFilter.Location = new System.Drawing.Point(457, 43);
            this.chkLotListFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkLotListFilter.Name = "chkLotListFilter";
            this.chkLotListFilter.Size = new System.Drawing.Size(133, 22);
            this.chkLotListFilter.TabIndex = 5;
            this.chkLotListFilter.Text = "Header Filter";
            this.chkLotListFilter.UseVisualStyleBackColor = true;
            this.chkLotListFilter.Visible = false;
            this.chkLotListFilter.Click += new System.EventHandler(this.chkLotListFilter_Click);
            // 
            // cmbValueTypes
            // 
            this.cmbValueTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbValueTypes.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem3.DataValue = "ValueListItem0";
            valueListItem3.DisplayText = "DEFECTS";
            valueListItem4.DataValue = "ValueListItem1";
            valueListItem4.DisplayText = "DEFECTIVE_DIE";
            this.cmbValueTypes.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.cmbValueTypes.Location = new System.Drawing.Point(404, 36);
            this.cmbValueTypes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbValueTypes.Name = "cmbValueTypes";
            this.cmbValueTypes.Size = new System.Drawing.Size(186, 28);
            this.cmbValueTypes.TabIndex = 4;
            this.cmbValueTypes.Text = "DEFECTS";
            this.cmbValueTypes.SelectionChanged += new System.EventHandler(this.cmbValueTypes_SelectionChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(295, 45);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 18);
            this.label9.TabIndex = 2;
            this.label9.Text = "Value Type";
            // 
            // rbtWaferList
            // 
            this.rbtWaferList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtWaferList.AutoSize = true;
            this.rbtWaferList.Location = new System.Drawing.Point(465, 6);
            this.rbtWaferList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbtWaferList.Name = "rbtWaferList";
            this.rbtWaferList.Size = new System.Drawing.Size(106, 22);
            this.rbtWaferList.TabIndex = 0;
            this.rbtWaferList.Text = "WaferList";
            this.rbtWaferList.UseVisualStyleBackColor = true;
            this.rbtWaferList.CheckedChanged += new System.EventHandler(this.rbtWaferList_CheckedChanged);
            // 
            // rbtStepList
            // 
            this.rbtStepList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtStepList.AutoSize = true;
            this.rbtStepList.Checked = true;
            this.rbtStepList.Location = new System.Drawing.Point(300, 6);
            this.rbtStepList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbtStepList.Name = "rbtStepList";
            this.rbtStepList.Size = new System.Drawing.Size(146, 22);
            this.rbtStepList.TabIndex = 0;
            this.rbtStepList.TabStop = true;
            this.rbtStepList.Text = "StepSummary";
            this.rbtStepList.UseVisualStyleBackColor = true;
            this.rbtStepList.CheckedChanged += new System.EventHandler(this.rbtStepList_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkWaferID);
            this.panel3.Controls.Add(this.chkOrdered);
            this.panel3.Controls.Add(this.butRun);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 236);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.panel3.Size = new System.Drawing.Size(600, 57);
            this.panel3.TabIndex = 2;
            // 
            // chkWaferID
            // 
            this.chkWaferID.AutoSize = true;
            this.chkWaferID.Checked = true;
            this.chkWaferID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWaferID.Location = new System.Drawing.Point(154, 12);
            this.chkWaferID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkWaferID.Name = "chkWaferID";
            this.chkWaferID.Size = new System.Drawing.Size(100, 22);
            this.chkWaferID.TabIndex = 5;
            this.chkWaferID.Text = "Wafer ID";
            this.chkWaferID.UseVisualStyleBackColor = true;
            // 
            // chkOrdered
            // 
            this.chkOrdered.AutoSize = true;
            this.chkOrdered.Checked = true;
            this.chkOrdered.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOrdered.Location = new System.Drawing.Point(10, 12);
            this.chkOrdered.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkOrdered.Name = "chkOrdered";
            this.chkOrdered.Size = new System.Drawing.Size(130, 22);
            this.chkOrdered.TabIndex = 5;
            this.chkOrdered.Text = "ASCENDING";
            this.chkOrdered.UseVisualStyleBackColor = true;
            this.chkOrdered.CheckedChanged += new System.EventHandler(this.chkOrdered_CheckedChanged);
            // 
            // butRun
            // 
            this.butRun.Dock = System.Windows.Forms.DockStyle.Right;
            this.butRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butRun.Image = ((System.Drawing.Image)(resources.GetObject("butRun.Image")));
            this.butRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butRun.Location = new System.Drawing.Point(344, 8);
            this.butRun.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butRun.Name = "butRun";
            this.butRun.Size = new System.Drawing.Size(249, 41);
            this.butRun.TabIndex = 0;
            this.butRun.Text = "  Search";
            this.butRun.Click += new System.EventHandler(this.butRun_Click);
            // 
            // fpSelectOption
            // 
            this.fpSelectOption.AccessibleDescription = "";
            this.fpSelectOption.BackColor = System.Drawing.Color.LightSteelBlue;
            this.fpSelectOption.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSelectOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSelectOption.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpSelectOption.Location = new System.Drawing.Point(4, 4);
            this.fpSelectOption.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fpSelectOption.Name = "fpSelectOption";
            this.fpSelectOption.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSelectOption.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSelectOption_Sheet1});
            this.fpSelectOption.Size = new System.Drawing.Size(538, 263);
            this.fpSelectOption.TabIndex = 0;
            this.fpSelectOption.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSelectOption.EditModeOn += new System.EventHandler(this.fpSelectOption_EditModeOn);
            this.fpSelectOption.SizeChanged += new System.EventHandler(this.fpSelectOption_SizeChanged);
            this.fpSelectOption.KeyUp += new System.Windows.Forms.KeyEventHandler(this.fpSelectOption_KeyUp);
            // 
            // pnlControl
            // 
            this.pnlControl.Controls.Add(this.butQuery);
            this.pnlControl.Controls.Add(this.BtnConfig);
            this.pnlControl.Controls.Add(this.btnClear);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlControl.Location = new System.Drawing.Point(542, 4);
            this.pnlControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(46, 263);
            this.pnlControl.TabIndex = 4;
            // 
            // butQuery
            // 
            this.butQuery.BackColor = System.Drawing.Color.Wheat;
            this.butQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.butQuery.Image = ((System.Drawing.Image)(resources.GetObject("butQuery.Image")));
            this.butQuery.Location = new System.Drawing.Point(0, 102);
            this.butQuery.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butQuery.Name = "butQuery";
            this.butQuery.Size = new System.Drawing.Size(46, 161);
            this.butQuery.TabIndex = 0;
            this.butQuery.UseVisualStyleBackColor = false;
            this.butQuery.Click += new System.EventHandler(this.butQuery_Click);
            // 
            // BtnConfig
            // 
            this.BtnConfig.BackColor = System.Drawing.Color.Gray;
            this.BtnConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnConfig.Image = ((System.Drawing.Image)(resources.GetObject("BtnConfig.Image")));
            this.BtnConfig.Location = new System.Drawing.Point(0, 34);
            this.BtnConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnConfig.Name = "BtnConfig";
            this.BtnConfig.Size = new System.Drawing.Size(46, 68);
            this.BtnConfig.TabIndex = 5;
            this.BtnConfig.UseVisualStyleBackColor = false;
            this.BtnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnClear.Font = new System.Drawing.Font("Gulim", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClear.ImageIndex = 11;
            this.btnClear.ImageList = this.imageList1;
            this.btnClear.Location = new System.Drawing.Point(0, 0);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(46, 34);
            this.btnClear.TabIndex = 6;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // butDown
            // 
            this.butDown.BackColor = System.Drawing.Color.Orange;
            this.butDown.Image = ((System.Drawing.Image)(resources.GetObject("butDown.Image")));
            this.butDown.Location = new System.Drawing.Point(617, 20);
            this.butDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butDown.Name = "butDown";
            this.butDown.Size = new System.Drawing.Size(46, 48);
            this.butDown.TabIndex = 0;
            this.butDown.UseVisualStyleBackColor = false;
            this.butDown.Visible = false;
            this.butDown.Click += new System.EventHandler(this.butDown_Click);
            // 
            // butUp
            // 
            this.butUp.BackColor = System.Drawing.Color.Orange;
            this.butUp.Image = ((System.Drawing.Image)(resources.GetObject("butUp.Image")));
            this.butUp.Location = new System.Drawing.Point(569, 20);
            this.butUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butUp.Name = "butUp";
            this.butUp.Size = new System.Drawing.Size(46, 48);
            this.butUp.TabIndex = 0;
            this.butUp.UseVisualStyleBackColor = false;
            this.butUp.Visible = false;
            this.butUp.Click += new System.EventHandler(this.butUp_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel2.Controls.Add(this.butUp);
            this.panel2.Controls.Add(this.butDown);
            this.panel2.Controls.Add(this.chkLsatInspect);
            this.panel2.Controls.Add(this.chkDefectImage);
            this.panel2.Controls.Add(this.chkDate);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.dtpStart);
            this.panel2.Controls.Add(this.butPeriodReflash);
            this.panel2.Controls.Add(this.dtpEnd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 22);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(600, 94);
            this.panel2.TabIndex = 2;
            // 
            // chkLsatInspect
            // 
            this.chkLsatInspect.AutoSize = true;
            this.chkLsatInspect.Location = new System.Drawing.Point(240, 4);
            this.chkLsatInspect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkLsatInspect.Name = "chkLsatInspect";
            this.chkLsatInspect.Size = new System.Drawing.Size(156, 22);
            this.chkLsatInspect.TabIndex = 6;
            this.chkLsatInspect.Text = "Last Inspection";
            this.chkLsatInspect.UseVisualStyleBackColor = true;
            // 
            // chkDefectImage
            // 
            this.chkDefectImage.AutoSize = true;
            this.chkDefectImage.Location = new System.Drawing.Point(240, 34);
            this.chkDefectImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkDefectImage.Name = "chkDefectImage";
            this.chkDefectImage.Size = new System.Drawing.Size(144, 22);
            this.chkDefectImage.TabIndex = 6;
            this.chkDefectImage.Text = "Include Image";
            this.chkDefectImage.UseVisualStyleBackColor = true;
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(240, 64);
            this.chkDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(141, 22);
            this.chkDate.TabIndex = 6;
            this.chkDate.Text = "Not Use Date";
            this.chkDate.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 56);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 18);
            this.label7.TabIndex = 5;
            this.label7.Text = "To";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 18);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 18);
            this.label6.TabIndex = 5;
            this.label6.Text = "From";
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy-MM-dd";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(81, 12);
            this.dtpStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(148, 28);
            this.dtpStart.TabIndex = 0;
            // 
            // butPeriodReflash
            // 
            this.butPeriodReflash.BackColor = System.Drawing.Color.Orange;
            this.butPeriodReflash.Image = ((System.Drawing.Image)(resources.GetObject("butPeriodReflash.Image")));
            this.butPeriodReflash.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.butPeriodReflash.Location = new System.Drawing.Point(414, 6);
            this.butPeriodReflash.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butPeriodReflash.Name = "butPeriodReflash";
            this.butPeriodReflash.Size = new System.Drawing.Size(140, 69);
            this.butPeriodReflash.TabIndex = 4;
            this.butPeriodReflash.Text = "Static List Up";
            this.butPeriodReflash.UseVisualStyleBackColor = false;
            this.butPeriodReflash.Click += new System.EventHandler(this.butPeriodReflash_Click);
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy-MM-dd";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(81, 50);
            this.dtpEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(148, 28);
            this.dtpEnd.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 116);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(600, 303);
            this.tabControl1.TabIndex = 70;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fpSelectOption);
            this.tabPage1.Controls.Add(this.pnlControl);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(592, 271);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Dynamic";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.butStaticSearch);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(592, 271);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Static";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Menu;
            this.panel1.Controls.Add(this.txtFilterWafer);
            this.panel1.Controls.Add(this.txtFilterLot);
            this.panel1.Controls.Add(this.txtFilterStep);
            this.panel1.Controls.Add(this.txtFilterProduct);
            this.panel1.Controls.Add(this.txtFilterEQ);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lstWafer);
            this.panel1.Controls.Add(this.lstLot);
            this.panel1.Controls.Add(this.lstStep);
            this.panel1.Controls.Add(this.lstProduct);
            this.panel1.Controls.Add(this.lstEQ);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 263);
            this.panel1.TabIndex = 3;
            // 
            // txtFilterWafer
            // 
            this.txtFilterWafer.Location = new System.Drawing.Point(607, 45);
            this.txtFilterWafer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFilterWafer.Name = "txtFilterWafer";
            this.txtFilterWafer.Size = new System.Drawing.Size(188, 28);
            this.txtFilterWafer.TabIndex = 2;
            // 
            // txtFilterLot
            // 
            this.txtFilterLot.Location = new System.Drawing.Point(409, 45);
            this.txtFilterLot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFilterLot.Name = "txtFilterLot";
            this.txtFilterLot.Size = new System.Drawing.Size(188, 28);
            this.txtFilterLot.TabIndex = 2;
            // 
            // txtFilterStep
            // 
            this.txtFilterStep.Location = new System.Drawing.Point(210, 45);
            this.txtFilterStep.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFilterStep.Name = "txtFilterStep";
            this.txtFilterStep.Size = new System.Drawing.Size(188, 28);
            this.txtFilterStep.TabIndex = 2;
            // 
            // txtFilterProduct
            // 
            this.txtFilterProduct.Location = new System.Drawing.Point(11, 45);
            this.txtFilterProduct.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFilterProduct.Name = "txtFilterProduct";
            this.txtFilterProduct.Size = new System.Drawing.Size(188, 28);
            this.txtFilterProduct.TabIndex = 2;
            // 
            // txtFilterEQ
            // 
            this.txtFilterEQ.Location = new System.Drawing.Point(806, 45);
            this.txtFilterEQ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFilterEQ.Name = "txtFilterEQ";
            this.txtFilterEQ.Size = new System.Drawing.Size(188, 28);
            this.txtFilterEQ.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(604, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(193, 32);
            this.label5.TabIndex = 1;
            this.label5.Text = "Wafer";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(406, 9);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 32);
            this.label4.TabIndex = 1;
            this.label4.Text = "Lot";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(210, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 32);
            this.label3.TabIndex = 1;
            this.label3.Text = "Layer";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(193, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Device";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(806, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "     InspectEQ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstWafer
            // 
            this.lstWafer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstWafer.FormattingEnabled = true;
            this.lstWafer.ItemHeight = 18;
            this.lstWafer.Location = new System.Drawing.Point(607, 78);
            this.lstWafer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstWafer.Name = "lstWafer";
            this.lstWafer.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstWafer.Size = new System.Drawing.Size(188, 4);
            this.lstWafer.TabIndex = 0;
            // 
            // lstLot
            // 
            this.lstLot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstLot.FormattingEnabled = true;
            this.lstLot.ItemHeight = 18;
            this.lstLot.Location = new System.Drawing.Point(409, 78);
            this.lstLot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstLot.Name = "lstLot";
            this.lstLot.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstLot.Size = new System.Drawing.Size(188, 4);
            this.lstLot.TabIndex = 0;
            // 
            // lstStep
            // 
            this.lstStep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstStep.FormattingEnabled = true;
            this.lstStep.ItemHeight = 18;
            this.lstStep.Location = new System.Drawing.Point(210, 78);
            this.lstStep.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstStep.Name = "lstStep";
            this.lstStep.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstStep.Size = new System.Drawing.Size(188, 4);
            this.lstStep.TabIndex = 0;
            // 
            // lstProduct
            // 
            this.lstProduct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstProduct.FormattingEnabled = true;
            this.lstProduct.ItemHeight = 18;
            this.lstProduct.Location = new System.Drawing.Point(11, 78);
            this.lstProduct.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstProduct.Name = "lstProduct";
            this.lstProduct.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstProduct.Size = new System.Drawing.Size(188, 4);
            this.lstProduct.TabIndex = 0;
            // 
            // lstEQ
            // 
            this.lstEQ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstEQ.FormattingEnabled = true;
            this.lstEQ.ItemHeight = 18;
            this.lstEQ.Location = new System.Drawing.Point(806, 78);
            this.lstEQ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstEQ.Name = "lstEQ";
            this.lstEQ.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstEQ.Size = new System.Drawing.Size(188, 4);
            this.lstEQ.TabIndex = 0;
            // 
            // butStaticSearch
            // 
            this.butStaticSearch.BackColor = System.Drawing.Color.Wheat;
            this.butStaticSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.butStaticSearch.Image = ((System.Drawing.Image)(resources.GetObject("butStaticSearch.Image")));
            this.butStaticSearch.Location = new System.Drawing.Point(538, 4);
            this.butStaticSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butStaticSearch.Name = "butStaticSearch";
            this.butStaticSearch.Size = new System.Drawing.Size(50, 263);
            this.butStaticSearch.TabIndex = 5;
            this.butStaticSearch.UseVisualStyleBackColor = false;
            this.butStaticSearch.Click += new System.EventHandler(this.butStaticSearch_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnKlarfFileOpen);
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3.Size = new System.Drawing.Size(592, 271);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Klarf File";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnKlarfFileOpen
            // 
            this.btnKlarfFileOpen.BackColor = System.Drawing.Color.White;
            this.btnKlarfFileOpen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnKlarfFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnKlarfFileOpen.Image")));
            this.btnKlarfFileOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnKlarfFileOpen.Location = new System.Drawing.Point(11, 9);
            this.btnKlarfFileOpen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKlarfFileOpen.Name = "btnKlarfFileOpen";
            this.btnKlarfFileOpen.Size = new System.Drawing.Size(214, 36);
            this.btnKlarfFileOpen.TabIndex = 215;
            this.btnKlarfFileOpen.Text = "   Read KLARF File...";
            this.btnKlarfFileOpen.UseVisualStyleBackColor = false;
            this.btnKlarfFileOpen.Click += new System.EventHandler(this.btnKlarfFileOpen_Click);
            // 
            // trvWaferList
            // 
            this.trvWaferList.Dock = System.Windows.Forms.DockStyle.Top;
            this.trvWaferList.ImageList = this.imageList1;
            this.trvWaferList.Location = new System.Drawing.Point(0, 434);
            this.trvWaferList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trvWaferList.Name = "trvWaferList";
            _override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.Single;
            this.trvWaferList.Override = _override1;
            this.trvWaferList.PathSeparator = ",";
            this.trvWaferList.Size = new System.Drawing.Size(600, 194);
            this.trvWaferList.TabIndex = 71;
            this.trvWaferList.DoubleClick += new System.EventHandler(this.trvWaferList_DoubleClick);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel4.Controls.Add(this.BtnSave);
            this.panel4.Controls.Add(this.BtnDelete);
            this.panel4.Controls.Add(this.BtnLoad);
            this.panel4.Controls.Add(this.cmbRecipe);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(600, 0);
            this.panel4.TabIndex = 71;
            // 
            // BtnSave
            // 
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSave.Location = new System.Drawing.Point(314, 45);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(109, 44);
            this.BtnSave.TabIndex = 1;
            this.BtnSave.Text = "  Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("BtnDelete.Image")));
            this.BtnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDelete.Location = new System.Drawing.Point(197, 45);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(109, 44);
            this.BtnDelete.TabIndex = 1;
            this.BtnDelete.Text = "  Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLoad.Image = ((System.Drawing.Image)(resources.GetObject("BtnLoad.Image")));
            this.BtnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnLoad.Location = new System.Drawing.Point(80, 45);
            this.BtnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(109, 44);
            this.BtnLoad.TabIndex = 1;
            this.BtnLoad.Text = "  Load";
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // cmbRecipe
            // 
            this.cmbRecipe.Location = new System.Drawing.Point(103, 3);
            this.cmbRecipe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbRecipe.Name = "cmbRecipe";
            this.cmbRecipe.Size = new System.Drawing.Size(323, 28);
            this.cmbRecipe.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 4);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 18);
            this.label8.TabIndex = 0;
            this.label8.Text = "Recipe";
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Enabled = false;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 0);
            this.ultraSplitter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 63;
            this.ultraSplitter1.Size = new System.Drawing.Size(600, 22);
            this.ultraSplitter1.TabIndex = 72;
            this.ultraSplitter1.Visible = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 419);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(600, 15);
            this.splitter2.TabIndex = 74;
            this.splitter2.TabStop = false;
            // 
            // DPUCStepSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.Controls.Add(this.pnlRunLoad);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.trvWaferList);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.panel4);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DPUCStepSelect";
            this.Size = new System.Drawing.Size(600, 936);
            this.Load += new System.EventHandler(this.DPUCStepSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectOption_Sheet1)).EndInit();
            this.pnlRunLoad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbValueTypes)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectOption)).EndInit();
            this.pnlControl.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trvWaferList)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRecipe)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        public bool MultiSelect
        {
            get
            {
                if (fpSpread1_Sheet1.SelectionPolicy == FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
                else
                    fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            }
        }

        public string[] WAFERID_LIST
        {
            get
            {
                return strWaferList.ToArray();
            }
        }

        public void Initialize(string[] strConfig = null)
        {
            FileInfo oFile = null;
            DataTable dtOption = null;
            DataSet ds = null;

            string[] strArrItem = null;
            try
            {
                //Default 는 "LOT_ID",  "PRODUCT", "STEP_ID",  "INSPECTION_EQ" 이며 이외의 경우 설정값 기준으로 사용된다.
                if (strConfig == null)
                    strArrItem = new string[] { "LOT_ID", "PRODUCT", "STEP_ID", "INSPECTION_EQ" };
                else
                    strArrItem = strConfig;

                FarPoint.Win.Spread.CellType.CheckBoxCellType chkCell = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                FarPoint.Win.Spread.CellType.TextCellType TxtCell = new FarPoint.Win.Spread.CellType.TextCellType();
                cmbCell.Items = new string[] { "=", "!=", "<", ">" };

                fpSelectOption.ActiveSheet.RowCount = strArrItem.Length;

                if (strConfig != null)
                {
                    fpSelectOption.ActiveSheet.Rows.Clear();
                    fpSelectOption.ActiveSheet.RowCount = strArrItem.Length;
                    fpSelectOption.ActiveSheet.ColumnCount = 4;
                    for (int ir = 0; ir < strArrItem.Length; ir++)
                    {
                        fpSelectOption.ActiveSheet.SetValue(ir, 1, strArrItem[ir]);
                        fpSelectOption.ActiveSheet.SetValue(ir, 2, "=");
                    }
                }
                else if (!File.Exists(ConditionFile()) || !fpSelectOption.Open(ConditionFile()))
                {
                    dtpEnd.Value = DateTime.Now;
                    dtpStart.Value = DateTime.Now.AddDays(-7);
                    fpSelectOption.ActiveSheet.ColumnCount = 4;
                    for (int ir = 0; ir < strArrItem.Length; ir++)
                    {
                        fpSelectOption.ActiveSheet.SetValue(ir, 1, strArrItem[ir]);
                        fpSelectOption.ActiveSheet.SetValue(ir, 2, "=");
                    }
                }
                else
                {
                    fpSelectOption.ActiveSheet.ColumnCount = 4;
                    fpSelectOption.ActiveSheet.RowHeaderColumnCount = 0;
                    fpSelectOption.ActiveSheet.ColumnHeaderRowCount = 0;
                    fpSelectOption.ActiveSheet.Columns[0].CellType = chkCell;
                    fpSelectOption.ActiveSheet.Columns[2].CellType = cmbCell;
                    dtpStart.Value = dtpEnd.Value.Add((TimeSpan)fpSelectOption.Tag);
                }

                //SearchOption 을 불러와서 Load 한다.
                isView = false;
                oFile = new FileInfo(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux"), SearchOption));
                if (oFile.Exists == true)
                {
                    ds = new DataSet();
                    dtOption = new DataTable();
                    ds.ReadXml(oFile.FullName);

                    if (ds.Tables.Count > 0)
                        dtOption = ds.Tables[0];

                    foreach (DataRow dr in dtOption.Rows)
                    {
                        if (String.Equals(dr["NAME"].ToString(), chkDate.Name))
                            chkDate.Checked = bool.Parse(dr["VALUE"].ToString());

                        if (String.Equals(dr["NAME"].ToString(), chkLsatInspect.Name))
                            chkLsatInspect.Checked = bool.Parse(dr["VALUE"].ToString());

                        if (String.Equals(dr["NAME"].ToString(), chkDefectImage.Name))
                            chkDefectImage.Checked = bool.Parse(dr["VALUE"].ToString());

                        if (String.Equals(dr["NAME"].ToString(), chkOrdered.Name))
                            chkOrdered.Checked = bool.Parse(dr["VALUE"].ToString());

                        if (String.Equals(dr["NAME"].ToString(), chkWaferID.Name))
                            chkWaferID.Checked = bool.Parse(dr["VALUE"].ToString());
                    }
                }

                //입력되었던 값중 Lot ID 는 초기화 한다.
                //for (int ir = 0; ir < fpSelectOption.ActiveSheet.RowCount; ir++)
                //{
                //    if (fpSelectOption.ActiveSheet.Cells[ir, 1].Value.ToString() == "LOT_ID")
                //        fpSelectOption.ActiveSheet.Cells[ir, 3].Value = null;
                //}

                chkCell.ThreeState = false;


                fpSelectOption.ActiveSheet.Columns[0].CellType = chkCell;
                fpSelectOption.ActiveSheet.Columns[1].CellType = TxtCell;
                fpSelectOption.ActiveSheet.Columns[2].CellType = cmbCell;
                fpSelectOption.ActiveSheet.Columns[3].CellType = TxtCell;

                fpSelectOption.ActiveSheet.Columns[0].Width = 20;
                fpSelectOption.ActiveSheet.Columns[1].Width = 80;
                fpSelectOption.ActiveSheet.Columns[2].Width = 60;
                fpSelectOption.ActiveSheet.Columns[3].Width = Math.Max(70, fpSelectOption.Width - 26 - (fpSelectOption.ActiveSheet.Columns[0].Width
                    + fpSelectOption.ActiveSheet.Columns[1].Width
                    + fpSelectOption.ActiveSheet.Columns[2].Width));

                fpSelectOption.ActiveSheet.Columns[1].Locked = true;
                butPeriodReflash.Visible = (tabControl1.SelectedIndex == 1);

                //입력 문자열 위치
                fpSelectOption.ActiveSheet.Columns[3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                fpSelectOption.ActiveSheet.Columns[3].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            }
            finally
            {
                if (dtOption != null)
                    dtOption.Dispose();

                if (ds != null)
                    ds.Dispose();
            }
        }

        private void DPUCStepSelect_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;
            Initialize();

            strMydocPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "DM");
            fnRecipeListUp();

            tabControl1.TabPages.Remove(tabPage2);
            //cmbValueTypes.Items.Clear();
            //cmbValueTypes.DataMember = "";
            //cmbValueTypes.DisplayMember = "VALUE";
            //cmbValueTypes.ValueMember = "KEY";
            //cmbValueTypes.DataSource = GetValueType();
            //cmbValueTypes.SelectedIndex = 0;


            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(strConfigInfo)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(strConfigInfo));
        }

        private DataTable GetValueType()
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
            row["VALUE"] = "DEFECTS";
            dt.Rows.Add(row);


            row = dt.NewRow();
            row["KEY"] = "DEFECTIVE_DIE";
            row["VALUE"] = "DEFECTIVE_DIE";
            dt.Rows.Add(row);

            return dt;
        }

        private string ConditionFile()
        {
            DirectoryInfo oDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux"));
            if (oDir.Exists == false)
                oDir.Create();

            return string.Format(@"{0}\{1}", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux"), this.conditionSaveFile);
        }

        private void butUp_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (fpSelectOption.ActiveSheet.ActiveRowIndex == 0) return;
                object[] oValue = new object[4];
                object[] oValue2 = new object[4];
                oValue[0] = fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 0);
                oValue[1] = fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 1);
                oValue[2] = fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 2);
                oValue[3] = fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 3);

                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 0, fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex - 1, 0));
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 1, fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex - 1, 1));
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 2, fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex - 1, 2));
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 3, fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex - 1, 3));

                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex - 1, 0, oValue[0]);
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex - 1, 1, oValue[1]);
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex - 1, 2, oValue[2]);
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex - 1, 3, oValue[3]);

                fpSelectOption.ActiveSheet.ActiveRowIndex = fpSelectOption.ActiveSheet.ActiveRowIndex - 1;
            }
            catch { }
        }

        private void butDown_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (fpSelectOption.ActiveSheet.ActiveRowIndex == fpSelectOption.ActiveSheet.Rows.Count - 1) return;
                object[] oValue = new object[4];
                object[] oValue2 = new object[4];
                oValue[0] = fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 0);
                oValue[1] = fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 1);
                oValue[2] = fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 2);
                oValue[3] = fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 3);

                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 0, fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex + 1, 0));
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 1, fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex + 1, 1));
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 2, fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex + 1, 2));
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex, 3, fpSelectOption.ActiveSheet.GetValue(fpSelectOption.ActiveSheet.ActiveRowIndex + 1, 3));

                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex + 1, 0, oValue[0]);
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex + 1, 1, oValue[1]);
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex + 1, 2, oValue[2]);
                fpSelectOption.ActiveSheet.SetValue(fpSelectOption.ActiveSheet.ActiveRowIndex + 1, 3, oValue[3]);

                fpSelectOption.ActiveSheet.ActiveRowIndex = fpSelectOption.ActiveSheet.ActiveRowIndex + 1;
            }
            finally
            { }
        }

        private void butQuery_Click(object sender, System.EventArgs e)
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            DataSet ds = null;
            bool bChecked = false;
            string strEqul = "";
            string strWhereValue = "";
            string strAddWhere = "";
            string strField = "";
            string[] strArrValue;
            List<string> strArrItem = new List<string>();
            List<string> wherePara = new List<string>();
            List<string> lsItem = new List<string>();

            try
            {
                MainForm.SetStatusMessage("조회를 시작 합니다.");
                oDMapAnalysis = new DACrux.SEMDMS.RO.DefectMapAnalysis();
                m_strSort = "";
                Utility.FPSpreadUtil.InitSpread(fpSpread1);
                trvWaferList.Nodes.Clear();
                isView = true;

                for (int i = 0; i < fpSelectOption.ActiveSheet.Rows.Count; i++)
                {
                    try
                    {
                        bChecked = fpSelectOption.ActiveSheet.GetText(i, 0).Equals("True");
                    }
                    catch
                    {
                        bChecked = false;
                    }
                    strField = (string)fpSelectOption.ActiveSheet.GetText(i, 1);
                    strEqul = (string)fpSelectOption.ActiveSheet.GetText(i, 2);
                    strWhereValue = (string)fpSelectOption.ActiveSheet.GetText(i, 3).Replace("'", "").Replace("*", "%").Replace("?", "_").ToUpper().Trim();

                    if (strEqul.Trim().Length > 0 && strWhereValue.Trim().Length > 0)
                    {
                        strArrItem.Add(strField);
                        //인자 값의 개수에 다라 = 또는 In 으로 Query를 진행 한다.
                        strArrValue = strWhereValue.Replace(" ", "").Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                        //최종 와일드 키가 있을 경우 적용
                        if (strWhereValue.Contains("%") == true)
                        {
                            if (strArrValue.Length > 1)
                            {

                                switch (strEqul)
                                {
                                    case "=":
                                        foreach (string strVal in strArrValue)
                                        {
                                            lsItem.Add(string.Format(" {0} LIKE '{1}' ", strField, strVal.Trim()));
                                        }

                                        strAddWhere = string.Format("( {0} )", string.Join("OR", lsItem.ToArray()));
                                        break;
                                    case "!=":
                                        foreach (string strVal in strArrValue)
                                        {
                                            lsItem.Add(string.Format(" {0} NOT LIKE '{1}' ", strField, strVal.Trim()));
                                        }

                                        strAddWhere = string.Format("( {0} )", string.Join("AND", lsItem.ToArray()));
                                        break;
                                    case "<":
                                    case ">":
                                        foreach (string strVal in strArrValue)
                                        {
                                            lsItem.Add(string.Format(" {0} {1} '{2}' ", strField, strEqul, strVal.Trim()));
                                        }

                                        strAddWhere = string.Format("( {0} )", string.Join("OR", lsItem.ToArray()));
                                        break;
                                }

                            }
                            else
                            {
                                switch (strEqul)
                                {
                                    case "=":
                                    case "IN":
                                    case "<":
                                    case ">":
                                        strEqul = "LIKE";
                                        strWhereValue = string.Format("'{0}'", strArrValue[0].Trim());
                                        strAddWhere = string.Format(" {0} {1} {2} ", strField, strEqul, strWhereValue);
                                        break;
                                    case "!=":
                                    case "NOT IN":
                                        strEqul = "NOT LIKE";
                                        strWhereValue = string.Format("'{0}'", strArrValue[0].Trim());
                                        strAddWhere = string.Format(" {0} {1} {2} ", strField, strEqul, strWhereValue);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (strArrValue.Length > 1)
                            {
                                switch (strEqul)
                                {
                                    case "=":
                                        strEqul = "IN";
                                        strWhereValue = string.Format("('{0}')", string.Join("','", strArrValue));
                                        strAddWhere = string.Format(" {0} {1} {2} ", strField, strEqul, strWhereValue);
                                        break;
                                    case "!=":
                                        strEqul = "NOT IN";
                                        strWhereValue = string.Format("('{0}')", string.Join("','", strArrValue));
                                        strAddWhere = string.Format(" {0} {1} {2} ", strField, strEqul, strWhereValue);
                                        break;
                                    case "<":
                                    case ">":
                                        foreach (string strVal in strArrValue)
                                        {
                                            lsItem.Add(string.Format(" {0} {1} '{2}' ", strField, strEqul, strVal.Trim()));
                                        }

                                        strAddWhere = string.Format("( {0} )", string.Join("OR", lsItem.ToArray()));
                                        break;
                                }

                            }
                            else
                            {
                                switch (strEqul)
                                {
                                    case "=":
                                        strWhereValue = string.Format("'{0}'", strArrValue[0]);
                                        strAddWhere = string.Format(" {0} {1} {2} ", strField, strEqul, strWhereValue);
                                        break;
                                    case "!=":
                                        strWhereValue = string.Format("'{0}'", strArrValue[0]);
                                        strAddWhere = string.Format(" {0} {1} {2} ", strField, strEqul, strWhereValue);
                                        break;
                                    case "<":
                                        strWhereValue = string.Format("'{0}'", strArrValue[0]);
                                        strAddWhere = string.Format(" {0} {1} {2} ", strField, strEqul, strWhereValue);
                                        break;
                                    case ">":
                                        strWhereValue = string.Format("'{0}'", strArrValue[0]);
                                        strAddWhere = string.Format(" {0} {1} {2} ", strField, strEqul, strWhereValue);
                                        break;
                                }
                            }
                        }

                        lsItem.Clear();
                        wherePara.Add(strAddWhere);

                    }

                    if (bChecked) m_strSort = string.Format("{0},{1}", m_strSort, strField);
                }

                //## Image 에 대한 포함 여부를 option으로 사용 한다.
                if (chkDefectImage.Checked)
                    wherePara.Add(" IMAGES > 0 ");

                if (m_strSort.Length > 0)
                {
                    m_strSort = m_strSort.Substring(1);
                }

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                if (chkDate.Checked)
                {
                    if (Array.IndexOf(strArrItem.ToArray(), "LOT_ID") < 0 && Array.IndexOf(strArrItem.ToArray(), "WAFER_ID") < 0)
                    {
                        MessageBox.Show("기간 조회 기능을 제외시 Lot ID, Wafer ID 중 하나는 필수 조건 입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    if (strArrItem.Count <= 0)
                    {
                        if (MessageBox.Show("기간만으로 조회시 오래 걸릴 수 있습니다. 계속 진행 하시겠습니까?", "확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                            return;
                    }
                }


                ds = oDMapAnalysis.GetDMSStepWaferList(dtpStart.Value.ToString("yyyy-MM-dd"), dtpEnd.Value.AddDays(1).ToString("yyyy-MM-dd"), wherePara.ToArray(), chkDate.Checked, chkLsatInspect.Checked);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables.IndexOf("STEPLIST") < 0 || ds.Tables["STEPLIST"].Rows.Count <= 0)
                {
                    MessageBox.Show("Not Found Data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (dtStepList != null)
                    dtStepList.Dispose();

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                dtStepList = ds.Tables["STEPLIST"].Copy();

                if (dtStepOrder != null)
                    dtStepOrder.Dispose();

                dtStepOrder = ds.Tables["STEP_ORDER"].Copy();

                if (m_strSort.Length > 0)
                {
                    //Check Box 가 있을 경우 Tree 에 출력 한다.
                    FillTree(dtStepList, m_strSort);
                    dtStepList = dtStepList.Select(String.Empty, m_strSort).CopyToDataTable<DataRow>();
                    fnSearchSheetSet(dtStepList);
                }
                else
                {
                    //Check Box 가 없을 경우 전체 Row 출력 한다.
                    fnSearchSheetSet(dtStepList);
                }

                SaveCurrentSaveOption();

                trvWaferList.Visible = trvWaferList.Nodes.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Data를 가져올 수 없습니다[Err:{0}", ex.Message));
            }
            finally
            {
                if (ds != null) ds.Dispose();
                wherePara = null;

                MainForm.SetStatusMessage(null);
            }
        }

        private void SaveCurrentSaveOption()
        {
            string strFullPath = string.Empty;
            DataTable dtOption = null;
            try
            {
                fpSelectOption.Tag = dtpStart.Value - dtpEnd.Value;
                //fpSelectOption.Tag = string.Format("{0};{1}", dtpStart.Value.ToString("yyyy-MM-dd HH:mm:ss"), dtpEnd.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                fpSelectOption.Save(ConditionFile(), false);

                dtOption = GetSearchOption();
                dtOption.Rows.Add(new object[] { chkDate.Name, chkDate.Checked.ToString() });
                dtOption.Rows.Add(new object[] { chkLsatInspect.Name, chkLsatInspect.Checked.ToString() });
                dtOption.Rows.Add(new object[] { chkDefectImage.Name, chkDefectImage.Checked.ToString() });
                dtOption.Rows.Add(new object[] { chkOrdered.Name, chkOrdered.Checked.ToString() });
                dtOption.Rows.Add(new object[] { chkWaferID.Name, chkWaferID.Checked.ToString() });
                dtOption.AcceptChanges();

                if (dtOption != null && dtOption.Rows.Count > 0)
                {
                    dtOption.WriteXml(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux"), SearchOption));
                }
            }
            catch (Exception) { }
        }

        private void butRun_Click(object sender, System.EventArgs e)
        {
            ArrayList arrStepSeq = new ArrayList();
            DACrux.Base.DPWafer[] wafer = null;

            DataView dv = null;
            string[] strStepSeq = null;

            try
            {
                MainForm.SetStatusMessage("조회를 시작 합니다.");
                strWaferList = new List<string>();
                FarPoint.Win.Spread.Model.CellRange[] cr = fpSpread1_Sheet1.GetSelections();

                if (rbtWaferList.Checked == true)
                {
                    if (fpSpread1_Sheet1 == null || fpSpread1_Sheet1.DataSource == null || fpSpread1_Sheet1.RowCount <= 0) return;

                    dv = ((DataTable)fpSpread1_Sheet1.DataSource).DefaultView;
                    for (int i = 0; i < cr.Length; i++)
                    {
                        for (int r = 0; r < cr[i].RowCount; r++)
                        {
                            arrStepSeq.Add(dv[cr[i].Row + r]["STEP_SEQ"].ToString());

                            if (strWaferList.IndexOf(dv[cr[i].Row + r]["WAFER_ID"].ToString()) < 0)
                                strWaferList.Add(dv[cr[i].Row + r]["WAFER_ID"].ToString());
                        }
                    }

                    //선택된 Row 가 없을 경우 전체 조회 한다.
                    int iCol = 0;
                    int iColWaferID = 0;
                    if (cr.Length <= 0)
                    {
                        for (int i = 0; i < fpSpread1_Sheet1.ColumnCount; i++)
                        {
                            if (fpSpread1_Sheet1.Columns[i].Label == "STEP_SEQ")
                            {
                                iCol = i;
                                break;
                            }
                        }

                        for (int i = 0; i < fpSpread1_Sheet1.ColumnCount; i++)
                        {
                            if (fpSpread1_Sheet1.Columns[i].Label == "WAFER_ID")
                            {
                                iColWaferID = i;
                                break;
                            }
                        }

                        for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
                        {
                            arrStepSeq.Add(fpSpread1_Sheet1.Cells[i, iCol].Value.ToString());

                            if (strWaferList.IndexOf(fpSpread1_Sheet1.Cells[i, iColWaferID].Value.ToString()) < 0)
                                strWaferList.Add(fpSpread1_Sheet1.Cells[i, iColWaferID].Value.ToString());
                        }
                    }

                    //Wafer List 의 경우 Columns 정보를 넣는다.

                    string sColumnOrder = string.Empty;
                    string sColumnWidth = string.Empty;

                    for (int i = 0; i < fpSpread1_Sheet1.ColumnCount; i++)
                    {
                        sColumnOrder += ";" + fpSpread1_Sheet1.Columns[i].Label;
                        sColumnWidth += ";" + fpSpread1_Sheet1.Columns[i].Width.ToString();
                    }

                    sColumnOrder = sColumnOrder.Substring(1);
                    sColumnWidth = sColumnWidth.Substring(1);

                    DACrux.Base.IniHandle.IniWriteValue(DACrux.Base.GlobalVariable.UserID, "WAFER_LIST_COLUMNS_ORDER", sColumnOrder, strConfigInfo);
                    DACrux.Base.IniHandle.IniWriteValue(DACrux.Base.GlobalVariable.UserID, "WAFER_LIST_COLUMNS_WIDTH", sColumnWidth, strConfigInfo);
                }
                else
                {
                    foreach (FarPoint.Win.Spread.Model.CellRange crRow in fpSpread1_Sheet1.GetSelections())
                    {
                        int iColumn = crRow.Column < 0 ? 0 : crRow.Column;
                        int iColumnCount = crRow.ColumnCount < 0 ? fpSpread1_Sheet1.Columns.Count : crRow.ColumnCount;
                        int iRow = crRow.Row < 0 ? 0 : crRow.Row;
                        int iRowCount = crRow.RowCount < 0 ? fpSpread1_Sheet1.RowCount : crRow.RowCount;

                        for (int iC = iColumn; iC < iColumn + iColumnCount; iC++)
                        {
                            for (int iR = iRow; iR < iRow + iRowCount; iR++)
                            {
                                DataRow[] drVal = dtStepList.Select(string.Format("STEP_ID = '{0}' AND WAFER_ID = '{1}'", fpSpread1_Sheet1.Columns[iC].Label, fpSpread1_Sheet1.Rows[iR].Label));
                                foreach (DataRow drow in drVal)
                                {
                                    arrStepSeq.Add(drow["STEP_SEQ"].ToString());
                                }

                                if (strWaferList.IndexOf(fpSpread1_Sheet1.Rows[iR].Label) < 0)
                                    strWaferList.Add(fpSpread1_Sheet1.Rows[iR].Label);
                            }
                        }
                    }
                }

                strStepSeq = new string[arrStepSeq.Count];

                if (strStepSeq.Length > 100)
                {
                    if (MessageBox.Show(string.Format("현재 선택한 Wafer 가 100개가 넘습니다. 계속 진행 하시겠습니까? (현: {0})", strStepSeq.Length), "확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        return;
                }

                arrStepSeq.CopyTo(strStepSeq);

                if (arrStepSeq.Count <= 0)
                    return;

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                wafer = new DACrux.Base.DPWafer[strStepSeq.Length];
                for (int i = 0; i < wafer.Length; i++) wafer[i].StepSeq = strStepSeq[i];
                if (OnSelected != null) OnSelected(this, wafer);

                //Main Form 기준 Active 되어 있는 Form 으로 조회 List 를 던져 준다.
                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                var child = FindForm().ActiveMdiChild as DACrux.SEMDMS.Interface.iSEMControl;
                if (child != null)
                    child.DrawWafer(wafer);

            }
            finally
            {
                arrStepSeq = null;
                wafer = null;

                MainForm.SetStatusMessage(null);
            }
        }

        private void fpSelectOption_SizeChanged(object sender, System.EventArgs e)
        {
            try
            {
                fpSelectOption.ActiveSheet.Columns[0].Width = 24;
                fpSelectOption.ActiveSheet.Columns[1].Width = 100;
                fpSelectOption.ActiveSheet.Columns[2].Width = 50;
                fpSelectOption.ActiveSheet.Columns[3].Width = Math.Max(70, fpSelectOption.Width - 26 - (fpSelectOption.ActiveSheet.Columns[0].Width
                    + fpSelectOption.ActiveSheet.Columns[1].Width
                    + fpSelectOption.ActiveSheet.Columns[2].Width));
            }
            catch { }
        }

        public void FillTree(DataTable dt, string strSort)
        {
            trvWaferList.Nodes.Clear();
            bool bExist = false;
            int imageIdx = 0;
            string[] strFields = strSort.Split(',');

            try
            {
                DataRow[] drs = dt.Select("", strSort);
                for (int i = 0; i < drs.Length; i++)
                {
                    Infragistics.Win.UltraWinTree.UltraTreeNode tn = null;
                    for (int ic = 0; ic < strFields.Length; ic++)
                    {
                        imageIdx = Math.Min(Array.IndexOf(m_strField, strFields[ic]), 10);

                        bExist = false;
                        if (tn == null)
                        {
                            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode inTn in trvWaferList.Nodes)
                            {
                                if (inTn.Text == drs[i][strFields[ic]].ToString())
                                {
                                    tn = inTn;
                                    bExist = true;
                                }
                            }
                            if (!bExist)
                            {
                                tn = new Infragistics.Win.UltraWinTree.UltraTreeNode(string.Format("{0}_{1}_{2}", drs[i][strFields[ic]].ToString(), i, ic), drs[i][strFields[ic]].ToString());
                                tn.Override.NodeAppearance.Image = imageIdx;
                                trvWaferList.Nodes.Add(tn);
                            }
                        }
                        else
                        {
                            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode inTn in tn.Nodes)
                            {
                                if (inTn.Text == drs[i][strFields[ic]].ToString())
                                {
                                    tn = inTn;
                                    bExist = true;
                                    break;
                                }
                            }

                            if (!bExist)
                            {
                                Infragistics.Win.UltraWinTree.UltraTreeNode tn2 = new Infragistics.Win.UltraWinTree.UltraTreeNode(string.Format("{0}_{1}_{2}", drs[i][strFields[ic]].ToString(), i, ic), drs[i][strFields[ic]].ToString());
                                tn2.Override.NodeAppearance.Image = imageIdx;
                                //if (!tn.Nodes.Contains(tn2))
                                //    tn.Nodes.Add(tn2);
                                if (!tn.Key.Contains(tn2.Key))
                                    tn.Nodes.Add(tn2);
                                tn = tn2;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void trvWaferList_DoubleClick(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (trvWaferList.SelectedNodes.Count <= 0)
                    return;

                string[] strFields = m_strSort.Split(',');
                //string[] strPath = trvWaferList.SelectedNode.FullPath.Split(',');
                object[] strPath = trvWaferList.SelectedNodes[0].FullPath.Split(',');
                string filter = "";
                DataView dv = new DataView(dtStepList);

                for (int i = 0; i < strPath.Length; i++)
                {
                    filter = filter + string.Format(" AND {0} = '{1}' ", strFields[i], strPath[i]);
                }

                dv.RowFilter = filter.Substring(4);
                //dv.Sort = "LOT_ID, WAFER_ID, " + m_strSort; //Lot ID 및 Wafer ID 가 최우선으로 Sorting 한다.

                if (dv.Count <= 0)
                {
                    fpSpread1_Sheet1.Rows.Clear();
                }
                else
                {
                    fnSearchSheetSet(dv.ToTable());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void butPeriodReflash_Click(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            try
            {
                m_strSort = "LOT_ID,WAFER_ID";
                //DataTable distinctTable = view.ToTable("DistinctTable", true, "Col1");
                oDMapAnalysis = new DACrux.SEMDMS.RO.DefectMapAnalysis();
                m_dtStatic = oDMapAnalysis.GetStepList(dtpStart.Value.ToString("yyyy-MM-dd"), dtpEnd.Value.ToString("yyyy-MM-dd")
                    , m_strField
                    , null
                    , m_strSort);

                FillEQ();
                FillProd();
                FillStep();
                FillLot();
                FillWafer();
                FillTree(m_dtStatic, m_strSort);
                FillSheet();
                m_dtStatic.TableName = "STEPLIST";

                if (dtStepList != null)
                    dtStepList.Dispose();

                dtStepList = m_dtStatic.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void chkOrdered_CheckedChanged(object sender, EventArgs e)
        {
            if(isView) butQuery.PerformClick();
        }

        #region ■ Static Fill
        private void FillEQ()
        {
            DataTable tmpDT = null;
            DataRow[] drs = null;
            string filter = string.Empty;
            try
            {
                // INSPECTION_EQ
                tmpDT = m_dtStatic.DefaultView.ToTable(true, "INSPECTION_EQ");

                if (txtFilterEQ.Text.Length > 0)
                    filter = string.Format("INSPECTION_EQ LIKE '*{0}*'", txtFilterEQ.Text);

                drs = tmpDT.Select(filter, "INSPECTION_EQ");
                lstEQ.Items.Clear();
                for (int i = 0; i < drs.Length; i++) lstEQ.Items.Add(drs[i]["INSPECTION_EQ"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillProd()
        {
            DataTable tmpDT = null;
            DataRow[] drs = null;
            string filter = string.Empty;
            try
            {
                tmpDT = m_dtStatic.DefaultView.ToTable(true, "PRODUCT");

                if (txtFilterProduct.Text.Length > 0)
                    filter = string.Format("PRODUCT LIKE '*{0}*'", txtFilterProduct.Text);

                //// PRODUCT
                drs = tmpDT.Select(null, "PRODUCT");
                lstProduct.Items.Clear();
                for (int i = 0; i < drs.Length; i++) lstProduct.Items.Add(drs[i]["PRODUCT"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void FillStep()
        {
            DataTable tmpDT = null;
            DataRow[] drs = null;
            string filter = string.Empty;
            try
            {
                tmpDT = m_dtStatic.DefaultView.ToTable(true, "STEP_ID");

                if (txtFilterStep.Text.Length > 0)
                    filter = string.Format("STEP_ID LIKE '*{0}*'", txtFilterStep.Text);

                //// STEP_ID
                tmpDT = m_dtStatic.DefaultView.ToTable(true, "STEP_ID");
                drs = tmpDT.Select(null, "STEP_ID");
                lstStep.Items.Clear();
                for (int i = 0; i < drs.Length; i++) lstStep.Items.Add(drs[i]["STEP_ID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillLot()
        {
            DataTable tmpDT = null;
            DataRow[] drs = null;
            string filter = string.Empty;
            try
            {
                tmpDT = m_dtStatic.DefaultView.ToTable(true, "LOT_ID");

                if (txtFilterLot.Text.Length > 0)
                    filter = string.Format("LOT_ID LIKE '*{0}*'", txtFilterLot.Text);

                // LOT_ID
                tmpDT = m_dtStatic.DefaultView.ToTable(true, "LOT_ID");
                drs = tmpDT.Select(null, "LOT_ID");
                lstLot.Items.Clear();
                for (int i = 0; i < drs.Length; i++) lstLot.Items.Add(drs[i]["LOT_ID"].ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillWafer()
        {
            DataTable tmpDT = null;
            DataRow[] drs = null;
            string filter = string.Empty;
            try
            {
                tmpDT = m_dtStatic.DefaultView.ToTable(true, "WAFER_ID");

                if (txtFilterWafer.Text.Length > 0)
                    filter = string.Format("WAFER_ID LIKE '*{0}*'", txtFilterWafer.Text);

                //// WAFER_ID
                tmpDT = m_dtStatic.DefaultView.ToTable(true, "WAFER_ID");
                drs = tmpDT.Select(null, "WAFER_ID");
                lstWafer.Items.Clear();
                for (int i = 0; i < drs.Length; i++) lstWafer.Items.Add(drs[i]["WAFER_ID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void FillSheet()
        {
            List<string> lsItem = new List<string>();
            try
            {
                if (m_dtStatic == null || m_dtStatic.Rows.Count <= 0)
                    return;

                DataView dv = new DataView(m_dtStatic);

                lsItem = new List<string>();
                if (lstEQ.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < lstEQ.SelectedItems.Count; i++)
                        lsItem.Add(lstEQ.SelectedItems[i].ToString());
                }
                else
                {
                    for (int i = 0; i < lstEQ.Items.Count; i++)
                        lsItem.Add(lstEQ.Items[i].ToString());
                }

                string[] strEQ = lsItem.ToArray();


                lsItem = new List<string>();
                if (lstProduct.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < lstProduct.SelectedItems.Count; i++)
                        lsItem.Add(lstProduct.SelectedItems[i].ToString());
                }
                else
                {
                    for (int i = 0; i < lstProduct.Items.Count; i++)
                        lsItem.Add(lstProduct.Items[i].ToString());
                }

                string[] strProduct = lsItem.ToArray();

                lsItem = new List<string>();
                if (lstStep.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < lstStep.SelectedItems.Count; i++)
                        lsItem.Add(lstStep.SelectedItems[i].ToString());
                }
                else
                {
                    for (int i = 0; i < lstStep.Items.Count; i++)
                        lsItem.Add(lstStep.Items[i].ToString());
                }

                string[] strStep = lsItem.ToArray();

                lsItem = new List<string>();
                if (lstLot.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < lstLot.SelectedItems.Count; i++)
                        lsItem.Add(lstLot.SelectedItems[i].ToString());
                }
                else
                {
                    for (int i = 0; i < lstLot.Items.Count; i++)
                        lsItem.Add(lstLot.Items[i].ToString());
                }

                string[] strLot = lsItem.ToArray();

                lsItem = new List<string>();
                if (lstWafer.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < lstWafer.SelectedItems.Count; i++)
                        lsItem.Add(lstWafer.SelectedItems[i].ToString());
                }
                else
                {
                    for (int i = 0; i < lstWafer.Items.Count; i++)
                        lsItem.Add(lstWafer.Items[i].ToString());
                }

                string[] strWafer = lsItem.ToArray();


                dv.RowFilter = string.Format("INSPECTION_EQ IN ('{0}') AND PRODUCT IN ('{1}') AND STEP_ID IN ('{2}') AND LOT_ID IN ('{3}')  AND WAFER_ID IN ('{4}')"
                                            , string.Join("','", strEQ)
                                            , string.Join("','", strProduct)
                                            , string.Join("','", strStep)
                                            , string.Join("','", strLot)
                                            , string.Join("','", strWafer));


                //dv.Sort = dv.Sort = "LOT_ID, WAFER_ID" + m_strSort; //Lot ID 및 Wafer ID 가 최우선으로 Sorting 한다.;

                if (dv.Count <= 0)
                {
                    fpSpread1_Sheet1.Rows.Clear();
                }
                else
                    fnSearchSheetSet(dv.ToTable());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butStaticSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillSheet();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //butPeriodReflash.Visible = (tabControl1.SelectedIndex == 1);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DACrux.Utility.ExcelUtilNoStatic excel = new DACrux.Utility.ExcelUtilNoStatic();
                excel.ToExcel(fpSpread1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            butRun_Click(null, null);
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            SetupSearchItem oForm = null;
            List<string> lsItem = new List<string>();
            try
            {
                //##Sheet 상의 정보를 전달한다.
                for (int i = 0; i < fpSelectOption.ActiveSheet.Rows.Count; i++)
                    lsItem.Add((string)fpSelectOption.ActiveSheet.GetText(i, 1));

                oForm = new SetupSearchItem(m_strField, lsItem.ToArray());
                oForm.OnSetSelection += new SetSelection(SetSelectItem);
                oForm.TopMost = true;
                oForm.StartPosition = FormStartPosition.CenterScreen;
                oForm.ShowInTaskbar = false;
                oForm.Show();
            }
            finally
            {
            }
        }

        private void SetSelectItem(string[] strSelectItem)
        {
            try
            {
                Initialize(strSelectItem);
            }
            finally
            {
            }
        }

        //Cell 수정 시 입력된 Data 초기화
        private void fpSelectOption_EditModeOn(object sender, EventArgs e)
        {
            //fpSelectOption.ActiveSheet.Cells[fpSelectOption.ActiveSheet.ActiveRowIndex, fpSelectOption.ActiveSheet.ActiveColumnIndex].ResetValue();
            //Application.DoEvents();
        }

        private void fpSelectOption_KeyUp(object sender, KeyEventArgs e)
        {
            //선택한 Cell Delete 키를 누를시 내용 삭제 한다.
            if (e.KeyCode == Keys.Delete)
            {
                fpSelectOption.ActiveSheet.Cells[fpSelectOption.ActiveSheet.ActiveRowIndex, fpSelectOption.ActiveSheet.ActiveColumnIndex].ResetValue();
                Application.DoEvents();
            }

            //Lot ID 또는 Wafer ID 입력 후 Enter 시 조회 하도록 한다.
            if (e.KeyCode == Keys.Enter &&
                (fpSelectOption.ActiveSheet.Cells[fpSelectOption.ActiveSheet.ActiveRowIndex, 1].Value.ToString() == "LOT_ID" ||
                 fpSelectOption.ActiveSheet.Cells[fpSelectOption.ActiveSheet.ActiveRowIndex, 1].Value.ToString() == "WAFER_ID") &&
                 bEndter == false &&
                 fpSelectOption.ActiveSheet.Cells[fpSelectOption.ActiveSheet.ActiveRowIndex, 3].Value != null &&
                 string.IsNullOrEmpty(fpSelectOption.ActiveSheet.Cells[fpSelectOption.ActiveSheet.ActiveRowIndex, 3].Value.ToString()) == false)
            {
                bEndter = true;
                butQuery_Click(null, null);
            }
            else
            {
                bEndter = false;
            }
        }

        private void chkLotListFilter_Click(object sender, EventArgs e)
        {
            for (int iCol = 0; iCol < fpSpread1_Sheet1.Columns.Count; iCol++)
            {
                fpSpread1_Sheet1.Columns[iCol].AllowAutoFilter = chkLotListFilter.Checked;
            }

            fpSpread1_Sheet1.Models.Selection.ClearSelection();
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // 입력 값 초기화
            for (int i = 0; i < fpSelectOption.ActiveSheet.RowCount; i++)
            {
                fpSelectOption.ActiveSheet.Cells[i, 3].Value = null;
            }
        }

        #region DM SearchControl

        public void GetForm(bool bSelectMulti)
        {
            this.MultiSelect = bSelectMulti;

        }

        private void rbtStepList_CheckedChanged(object sender, EventArgs e)
        {
            fpSpread1.AllowColumnMove = false;
            if (rbtStepList.Checked == true)
            {
                chkWaferID.Visible = true;
                label9.Visible = true;
                cmbValueTypes.Visible = true;
                chkOrdered.Visible = true;
                chkLotListFilter.Visible = false;
                BtnColRefresh.Visible = false;
                //butQuery_Click(null, null);
                butQuery.PerformClick();
            }
        }

        private void rbtWaferList_CheckedChanged(object sender, EventArgs e)
        {
            fpSpread1.AllowColumnMove = false;

            if (rbtWaferList.Checked == true)
            {
                fpSpread1.AllowColumnMove = true;
                chkWaferID.Visible = false;
                label9.Visible = false;
                cmbValueTypes.Visible = false;
                chkOrdered.Visible = false;
                chkLotListFilter.Visible = true;
                BtnColRefresh.Visible = true;
                //butQuery_Click(null, null);
                butQuery.PerformClick();

                //====================================================================

                // Column 순서 및 넓이 지정
                string sColumnOrder = DACrux.Base.IniHandle.IniReadValue(DACrux.Base.GlobalVariable.UserID, "WAFER_LIST_COLUMNS_ORDER", strConfigInfo);
                string sColumnWidth = DACrux.Base.IniHandle.IniReadValue(DACrux.Base.GlobalVariable.UserID, "WAFER_LIST_COLUMNS_WIDTH", strConfigInfo);
                string[] ColumnWidth = string.IsNullOrEmpty(sColumnWidth) ? null : sColumnWidth.Split(';');

                if (string.IsNullOrEmpty(sColumnOrder) == false)
                {
                    string[] ColumnsName = sColumnOrder.Split(';');
                    try
                    {
                        for (int i = 0; i < fpSpread1_Sheet1.ColumnCount; i++)
                        {
                            fpSpread1_Sheet1.Columns[i].DataField = ColumnsName[i];

                            if (ColumnWidth != null)
                                fpSpread1_Sheet1.Columns[i].Width = int.Parse(ColumnWidth[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        private void cmbValueTypes_SelectionChanged(object sender, EventArgs e)
        {
            butQuery_Click(null, null);
        }


        private void fnSearchSheetSet(DataTable dtRow)
        {
            Utility.FPSpreadUtil.InitSpread(fpSpread1);

            if (rbtStepList.Checked && dtRow != null && dtStepOrder != null)
            {
                FillCollectionSheet(dtRow);

                fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal | FarPoint.Win.Spread.OperationMode.ReadOnly;
                fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            }
            else
            {

                fpSpread1_Sheet1.DataSource = dtRow;
                //Utility.FPSpreadUtil.SpreadSortingAll(fpSpread1_Sheet1);
                fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
                Utility.FPSpreadUtil.AdjustSpread(fpSpread1_Sheet1);

                for (int iCol = 0; iCol < fpSpread1_Sheet1.Columns.Count; iCol++)
                {
                    if (fpSpread1_Sheet1.Columns[iCol].Label == "LOT_ID" || fpSpread1_Sheet1.Columns[iCol].Label == "PRODUCT" || fpSpread1_Sheet1.Columns[iCol].Label == "STEP_ID" || fpSpread1_Sheet1.Columns[iCol].Label == "INSPECTION_EQ")
                        fpSpread1_Sheet1.Columns[iCol].MergePolicy = FarPoint.Win.Spread.Model.MergePolicy.Always;

                    fpSpread1_Sheet1.Columns[iCol].AllowAutoFilter = chkLotListFilter.Checked;
                }
            }

            fpSpread1_Sheet1.Models.Selection.ClearSelection();
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
        }


        private void FillCollectionSheet(DataTable dtRow)
        {
            DataTable dtWaferID = null;
            DataTable dtTmp = null;
            List<String> lstStepNames = null;
            List<String> lsHeader = null;
            int iRowSpenCnt = 0;

            try
            {
                fpSpread1_Sheet1.Rows.Clear();
                fpSpread1_Sheet1.Columns.Clear();
                fpSpread1_Sheet1.RowHeader.ColumnCount = 3;

                if (chkWaferID.Checked)
                {
                    fpSpread1_Sheet1.RowHeader.Columns[1].Visible = false;
                    fpSpread1_Sheet1.RowHeader.Columns[2].Visible = true;
                }
                else
                {
                    fpSpread1_Sheet1.RowHeader.Columns[1].Visible = true;
                    fpSpread1_Sheet1.RowHeader.Columns[2].Visible = false;
                }

                if (dtRow == null || dtStepOrder == null
                    || dtRow.Rows.Count <= 0 || dtStepOrder.Rows.Count <= 0)
                {
                    throw new Exception("Not Found Data");
                }

                lstStepNames = new List<string>();
                lsHeader = new List<string>();

                //fpSpread1_Sheet1.Columns.Add(0, dtStepOrder.Rows.Count);
                //fpSpread1_Sheet1.Rows.Add(0, iDataRows);
                // Column Label 생성
                //for(int iCol = 0; iCol < dtStepOrder.Rows.Count; iCol++)
                //{
                //    fpSpread1_Sheet1.SetColumnLabel(0, iCol, dtStepOrder.Rows[iCol]["STEP_ID"].ToString());
                //    fpSpread1_Sheet1.Columns[iCol].Width = 100;
                //}
                dtTmp = dtRow.DefaultView.ToTable(true, "STEP_ID");
                foreach (DataRow dr in dtTmp.Rows)
                {
                    lstStepNames.Add(dr["STEP_ID"].ToString());
                }

                //--

                if (lstStepNames.Count <= 0)
                    return;

                //--

                DataRow[] rows = dtStepOrder.Select(
                    String.Format("[STEP_ID] IN ('{0}')", string.Join("','", lstStepNames)),
                    String.Format("RESULTTIMESTAMP {0}", chkOrdered.Checked ? "ASC" : "DESC")
                    );

                //--

                fpSpread1_Sheet1.Columns.Add(0, rows.Length);

                //--

                for (int iR = 0; iR < rows.Length; iR++)
                {
                    fpSpread1_Sheet1.SetColumnLabel(0, iR, rows[iR]["STEP_ID"].ToString());
                    fpSpread1_Sheet1.Columns[iR].Label = rows[iR]["STEP_ID"].ToString();
                    fpSpread1_Sheet1.Columns[iR].Width = 100;
                    lsHeader.Add(rows[iR]["STEP_ID"].ToString());
                }

                //--

                FarPoint.Win.Spread.CellType.NumberCellType oNumCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                oNumCell.DecimalPlaces = 0;

                dtRow = dtRow.Select("", "WAFER_ID, STEP_ID, SLOT_ID").CopyToDataTable<DataRow>();
                dtWaferID = dtRow.DefaultView.ToTable(true, "LOT_ID", "WAFER_ID", "SLOT_ID").Select("", "LOT_ID, WAFER_ID, SLOT_ID").CopyToDataTable<DataRow>();

                int ibf = 0;
                for (int ir = 0; ir < dtWaferID.Rows.Count; ir++)
                {
                    string strLotID = dtWaferID.Rows[ir]["LOT_ID"].ToString();
                    string strSlotID = dtWaferID.Rows[ir]["SLOT_ID"].ToString();
                    string strWaferID = dtWaferID.Rows[ir]["WAFER_ID"].ToString();

                    fpSpread1_Sheet1.Rows.Count++;
                    fpSpread1_Sheet1.Rows[fpSpread1_Sheet1.Rows.Count - 1].Label = strWaferID;

                    fpSpread1_Sheet1.RowHeader.Cells[ir, 0].Text = strLotID;
                    fpSpread1_Sheet1.RowHeader.Cells[ir, 1].Text = strSlotID;
                    fpSpread1_Sheet1.RowHeader.Cells[ir, 2].Text = strWaferID;

                    DataTable dtWaferStep = dtRow.Select(string.Format("WAFER_ID = '{0}'", strWaferID)).CopyToDataTable<DataRow>().DefaultView.ToTable(true, "STEP_ID");
                    foreach (DataRow drStep in dtWaferStep.Rows)
                    {
                        string strStepName = drStep["STEP_ID"].ToString();

                        int iColIndex = lsHeader.IndexOf(strStepName);
                        if (iColIndex < 0)
                            continue;

                        decimal oVal = decimal.Parse(dtRow.Compute(string.Format("SUM({0})", cmbValueTypes.Text), string.Format("WAFER_ID = '{0}' AND STEP_ID = '{1}'", strWaferID, strStepName)).ToString());
                        fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.Rows.Count - 1, iColIndex].Value = oVal;

                    }

                    if (ir > 0)
                    {
                        string strvfLotID = fpSpread1_Sheet1.RowHeader.Cells[ir - 1, 0].Text;
                        if (strvfLotID != strLotID)
                        {
                            fpSpread1_Sheet1.AddRowHeaderSpanCell(ibf, 0, iRowSpenCnt, 1);
                            ibf = ir;
                            iRowSpenCnt = 0;
                        }
                    }

                    iRowSpenCnt++;

                    //마지막 Row
                    if (ir + 1 == dtWaferID.Rows.Count)
                    {
                        fpSpread1_Sheet1.AddRowHeaderSpanCell(ibf, 0, iRowSpenCnt, 1);
                    }

                }

                fpSpread1_Sheet1.RowHeader.Columns[0].Width = 60;
                fpSpread1_Sheet1.RowHeader.Columns[1].Width = 30;
                fpSpread1_Sheet1.RowHeader.Columns[2].Width = 80;

                fpSpread1.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnColRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (System.IO.File.Exists(strConfigInfo)) 
                    System.IO.File.Delete(strConfigInfo);

                rbtWaferList_CheckedChanged(null, null);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Recipe 저장(화면 및 조회 관련 기능)

        private void fnRecipeListUp()
        {
            DirectoryInfo dr = null;
            try
            {
                dr = new DirectoryInfo(strMydocPath);
                cmbRecipe.Items.Clear();
                foreach (DirectoryInfo di in dr.GetDirectories())
                {
                    cmbRecipe.Items.Add(di.Name);
                }

            }
            catch (Exception) { }
        }

        /// <summary>
        /// 저장된 Recipe 를 불러 온다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            DataTable dtMenu = null;
            DataTable dtMenuList = null;
            DataTable dtWaferList = null;
            DataSet ds = null;
            FileInfo oFile = null;

            string strFullPath = string.Empty;

            Assembly assemblyMap = null;
            Type type = null;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (string.IsNullOrEmpty(cmbRecipe.Text) == true)
                    throw new Exception("Recipe ID 입력 확인.");


                strFullPath = Path.Combine(strMydocPath, cmbRecipe.Text);

                if (MessageBox.Show("해당 Recipe 를 Load 하시겠습니까? Load 시 현재 열려 있는 DMS 화면들은 사라집니다.", "확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;

                dtMenu = GetMenu();
                if (dtMenu == null || dtMenu.Rows.Count <= 0)
                    throw new Exception("Menu File 을 불러 올 수 없습니다. 관리자에 문의 하세요.");


                //현재 Open 된 화면에 대해 close 한다.
                foreach (Form fm in this.ParentForm.MdiChildren)
                {
                    if (dtMenu.Select(string.Format("RESV_04 = '{0}' AND ASSEMBLY = '{1}'", "DM", fm.GetType().FullName)).Length > 0)
                    {
                        fm.Close();
                        fm.Dispose();
                    }
                    Application.DoEvents();
                }

                dtMenuList = new DataTable();

                //Menu File 들을 불러 온다.
                ds = new DataSet();
                oFile = new FileInfo(Path.Combine(strFullPath, "MenuList"));
                if (oFile.Exists == true)
                {
                    ds.ReadXml(oFile.FullName);

                    if (ds.Tables.Count > 0)
                        dtMenuList = ds.Tables[0];
                }

                if (ds != null)
                    ds.Dispose();

                ds = new DataSet();
                oFile = new FileInfo(Path.Combine(strFullPath, "WaferData"));
                if (oFile.Exists == true)
                {
                    ds.ReadXml(oFile.FullName);

                    if (ds.Tables.Count > 0)
                        dtWaferList = ds.Tables[0];

                    fnSearchSheetSet(dtWaferList);
                }

                oFile = new FileInfo(Path.Combine(strFullPath, "Option"));
                if (oFile.Exists == true)
                {
                    fpSelectOption.Open(oFile.FullName);
                    string strTimeVal = System.Convert.ToString(fpSelectOption.Tag);
                    if (string.IsNullOrEmpty(strTimeVal) == false && strTimeVal.Contains(";"))
                    {
                        string[] strVal = strTimeVal.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        dtpStart.Value = DateTime.Parse(strVal[0]);
                        dtpEnd.Value = DateTime.Parse(strVal[1]);
                    }
                }

                oFile = new FileInfo(Path.Combine(strFullPath, "TreeList"));
                if (oFile.Exists == true)
                {
                    trvWaferList.LoadFromXml(oFile.FullName);
                }

                //저장된 화면 들을 재 실행 한다.
                foreach (DataRow dr in dtMenuList.Rows)
                {
                    DataRow[] drMenu = dtMenu.Select(string.Format("MODULE = '{0}.dll' AND ASSEMBLY = '{1}'", dr["NAMESPACE"], dr["FULLNAME"]));
                    if (drMenu.Length > 0)
                    {
                        assemblyMap = DACrux.Base.AssemblyUtil.LoadAssembly(string.Format(@"{0}\{1}", Application.StartupPath, drMenu[0]["MODULE"].ToString()));
                        type = assemblyMap.GetType(drMenu[0]["ASSEMBLY"].ToString());

                        /// Multi로 Open되는지 확인후 기존 열린 화면을 찾아 Focus만 준다.
                        /////////////////////////////////////////////////////////////////////////////
                        if (drMenu[0]["MULTI"].ToString().ToUpper() == "FALSE")
                        {
                            for (int i = 0; i < this.ParentForm.MdiChildren.Length; i++)
                            {
                                if (this.ParentForm.MdiChildren[i].GetType().FullName == type.FullName)
                                {
                                    this.ParentForm.MdiChildren[i].Focus();
                                    return;
                                }
                            }
                        }

                        DACrux.Framework.Base.DACruxUXBasic01 oForm = null;
                        string[] strWaferList = dr["WAFER_LIST"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        if (!string.IsNullOrEmpty(drMenu[0]["RESV_01"].ToString().Trim()))
                            oForm = (DACrux.Framework.Base.DACruxUXBasic01)Activator.CreateInstance(type, drMenu[0]["RESV_01"].ToString());
                        else
                            oForm = (DACrux.Framework.Base.DACruxUXBasic01)Activator.CreateInstance(type);

                        oForm.WaferList = strWaferList;
                        //oForm.FUNC_CODE = drMenu[0]["RESV_03"].ToString();
                        oForm.FUNC_CODE = drMenu[0]["FUNCTION_CODE"].ToString();
                        oForm.MdiParent = this.ParentForm;

                        oForm.Show();

                        Application.DoEvents();
                    }
                }

                MessageBox.Show("Load 가 완료 되었습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fnRecipeListUp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string strFullPath = string.Empty;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (MessageBox.Show(string.Format("해당 Recipe {0} 를 삭제 하시겠습니까?", cmbRecipe.Text), "확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;

                strFullPath = Path.Combine(strMydocPath, cmbRecipe.Text);

                DirectoryInfo oDir = new DirectoryInfo(strFullPath);
                if (oDir.Exists == true)
                {
                    oDir.Delete(true);
                }

                MessageBox.Show("삭제가 완료 되었습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fnRecipeListUp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Recipe 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable dtMenu = null;
            DataTable dtMenuList = null;
            DataTable dtWaferList = null;

            string strFullPath = string.Empty;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (string.IsNullOrEmpty(cmbRecipe.Text) == true)
                    throw new Exception("Recipe ID 입력 확인.");

                if (MessageBox.Show("해당 Recipe 를 저장 하시겠습니까?", "확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;

                strFullPath = Path.Combine(strMydocPath, cmbRecipe.Text);

                DirectoryInfo oDir = new DirectoryInfo(strFullPath);
                if (oDir.Exists == true)
                {
                    if (MessageBox.Show(string.Format("이미 저장된 Recipe {0} 가 있습니다. 다시 만들겠습니까?", cmbRecipe.Text), "중복 확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        return;

                    oDir.Delete(true);
                    oDir.Refresh();
                    Application.DoEvents();
                }

                oDir = new DirectoryInfo(strFullPath);
                oDir.Create();

                //메뉴 정의가 DM 으로 되어 있는 화면 온다.
                dtMenu = GetMenu();
                dtMenuList = GetMenuList();
                if (dtMenu == null || dtMenu.Rows.Count <= 0)
                    throw new Exception("Menu File 을 불러 올 수 없습니다. 관리자에 문의 하세요.");

                //현재 보고 있는 화면 저장
                foreach (DACrux.Framework.Base.DACruxUXBasic01 fm in this.ParentForm.MdiChildren)
                {
                    if (dtMenu.Select(string.Format("RESV_04 = '{0}' AND ASSEMBLY = '{1}'", "DM", fm.GetType().FullName)).Length > 0)
                        dtMenuList.Rows.Add(new object[] { fm.GetType().Name, fm.GetType().Namespace, fm.GetType().FullName, fm.WaferList == null ? "" : string.Join(",", fm.WaferList) });

                    //var vTemp = fm.GetType().GetField.GetMethod("WaferList");
                }

                if (fpSpread1.DataSource != null)
                {
                    dtWaferList = (DataTable)fpSpread1.DataSource;
                    dtWaferList.TableName = "WaferData";

                    //각각 control 마다 저장 한다.
                    dtWaferList.WriteXml(Path.Combine(strFullPath, "WaferData"));
                }

                if (dtMenuList != null && dtMenuList.Rows.Count > 0)
                {
                    dtMenuList.WriteXml(Path.Combine(strFullPath, "MenuList"));
                }

                fpSelectOption.Tag = string.Format("{0};{1}", dtpStart.Value.ToString("yyyy-MM-dd HH:mm:ss"), dtpEnd.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                fpSelectOption.Save(Path.Combine(strFullPath, "Option"), false);
                trvWaferList.SaveAsXml(Path.Combine(strFullPath, "TreeList"));

                MessageBox.Show("저장이 완료 되었습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fnRecipeListUp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private DataTable GetMenu()
        {
            DataTable dt = null;
            DataSet ds = null;
            string strMenuFile = Application.StartupPath + @"\MenuFile.mnu";

            try
            {
                if (File.Exists(strMenuFile))
                {
                    ds = new DataSet();
                    ds.ReadXml(strMenuFile);
                    return ds.Tables[0];
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        private DataTable GetMenuList()
        {
            DataTable dt = null;

            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("NAME", typeof(string)));
                dt.Columns.Add(new DataColumn("NAMESPACE", typeof(string)));
                dt.Columns.Add(new DataColumn("FULLNAME", typeof(string)));
                dt.Columns.Add(new DataColumn("WAFER_LIST", typeof(string)));
                dt.TableName = "WAFER";
                dt.AcceptChanges();

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        private DataTable GetSearchOption()
        {
            DataTable dt = null;

            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("NAME", typeof(string)));
                dt.Columns.Add(new DataColumn("VALUE", typeof(string)));
                dt.TableName = "SEARCH";
                dt.AcceptChanges();

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        #endregion

        #region Klarf 파일 로드

        private void btnKlarfFileOpen_Click(object sender, EventArgs e)
        {
            List<ParserKlarf> parserList = new List<ParserKlarf>();

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                string errorMessage = null;

                foreach (string fileName in dlg.FileNames)
                {
                    try
                    {
                        ParserKlarf parser = new ParserKlarf(fileName);

                        if (parser.Wafers.Count == 0)
                            errorMessage += String.Format("읽기 에러 : {0} | {1}\n", Path.GetFileName(fileName), "Wafer 데이터 없음");

                        parserList.Add(parser);
                    }
                    catch (Exception ex)
                    {
                        errorMessage += String.Format("읽기 에러 : {0} | {1}\n", Path.GetFileName(fileName), ex.Message);
                    }
                }

                if (!String.IsNullOrEmpty(errorMessage))
                    ShowErrorMessage(errorMessage);
            }

            DACrux.Base.DefectList defectList;
            DmsCacheHelper.LoadKlarfData(parserList.ToArray(), out defectList);

            var child = FindForm().ActiveMdiChild as DACrux.SEMDMS.Interface.iFileControl;

            if (child != null)
                child.DrawWafer(defectList);
            else
                ShowErrorMessage(String.Format("'{0}' 화면에서는 열 수 없습니다.", FindForm().ActiveMdiChild.Text));
        }

        #endregion
    }
}
