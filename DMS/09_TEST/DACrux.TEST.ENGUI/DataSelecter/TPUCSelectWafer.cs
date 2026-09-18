using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.TEST.RO;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// TPUCSelectWafer에 대한 요약 설명입니다.
    /// </summary>
    /// 

    public delegate void Selected(object sender, TPWafer[] SelDatas);
    public delegate void SelectionCancel(object sender);

    public class TPUCSelectWafer : DACrux.Framework.Base.DACruxCTLBasic01
    {
        string conditionSaveFile = @"TEST.ini";
        string SearchOption = "TESTSearchOption";
        string strMydocPath = string.Empty;
        private System.Windows.Forms.Panel pnlSelectWafer;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlRunLoad;
        private System.Windows.Forms.Splitter splitter1;
        private FarPoint.Win.Spread.FpSpread fpSelectOption;
        private FarPoint.Win.Spread.SheetView fpSelectOption_Sheet1;
        private string[] m_strField = new string[] {"TESTAREA","DEVICE_ALIAS","PROGRAM","LOT_ID","WAFER_ID","TESTER","PROBE_CARD", "CUSTOMER", "WAFER_SEQ", "LOT_SEQ"};
        private System.Windows.Forms.Button butUp;
        private System.Windows.Forms.Button butDown;
        private System.Windows.Forms.Button butQuery;
        private DataTable dtWaferList = null;
        private System.Windows.Forms.ImageList imageList1;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button butRun;
        private System.Windows.Forms.Panel pnlControl;
        private string m_strSort = string.Empty;
        private System.Windows.Forms.ContextMenu ctxMnu;
        private System.Windows.Forms.MenuItem mnuAll;
        private System.Windows.Forms.MenuItem mnuNone;
        private System.Windows.Forms.MenuItem mnuInvert;
        private System.Windows.Forms.MenuItem mnuWhere;
        public event Selected OnSelected;
        private bool m_isMultiSelect = false;
        private Panel panel4;
        private Button BtnSave;
        private Button BtnDelete;
        private Button BtnLoad;
        private Label label8;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Splitter splitter2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbRecipe;
        private Infragistics.Win.UltraWinTree.UltraTree trvWaferList;
        private Label label7;
        private Label label6;
        private CheckBox chkDate;
        private Button BtnConfig;
        private CheckBox chkLastTest;
        //private string[] TestArea = null;

        public TPUCSelectWafer()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();

            // TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

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
                if (dtWaferList != null)
                    dtWaferList.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPUCSelectWafer));
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            this.pnlSelectWafer = new System.Windows.Forms.Panel();
            this.fpSelectOption = new FarPoint.Win.Spread.FpSpread();
            this.fpSelectOption_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlControl = new System.Windows.Forms.Panel();
            this.butQuery = new System.Windows.Forms.Button();
            this.BtnConfig = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.butUp = new System.Windows.Forms.Button();
            this.butDown = new System.Windows.Forms.Button();
            this.chkLastTest = new System.Windows.Forms.CheckBox();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.pnlRunLoad = new System.Windows.Forms.Panel();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.butRun = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.ctxMnu = new System.Windows.Forms.ContextMenu();
            this.mnuAll = new System.Windows.Forms.MenuItem();
            this.mnuNone = new System.Windows.Forms.MenuItem();
            this.mnuInvert = new System.Windows.Forms.MenuItem();
            this.mnuWhere = new System.Windows.Forms.MenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmbRecipe = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.trvWaferList = new Infragistics.Win.UltraWinTree.UltraTree();
            this.pnlSelectWafer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectOption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectOption_Sheet1)).BeginInit();
            this.pnlControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlRunLoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRecipe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvWaferList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSelectWafer
            // 
            this.pnlSelectWafer.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlSelectWafer.Controls.Add(this.fpSelectOption);
            this.pnlSelectWafer.Controls.Add(this.pnlControl);
            this.pnlSelectWafer.Controls.Add(this.panel2);
            this.pnlSelectWafer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSelectWafer.Location = new System.Drawing.Point(0, 15);
            this.pnlSelectWafer.Name = "pnlSelectWafer";
            this.pnlSelectWafer.Padding = new System.Windows.Forms.Padding(5);
            this.pnlSelectWafer.Size = new System.Drawing.Size(528, 252);
            this.pnlSelectWafer.TabIndex = 60;
            // 
            // fpSelectOption
            // 
            this.fpSelectOption.AccessibleDescription = "";
            this.fpSelectOption.BackColor = System.Drawing.Color.LightSteelBlue;
            this.fpSelectOption.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSelectOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSelectOption.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpSelectOption.Location = new System.Drawing.Point(5, 61);
            this.fpSelectOption.Name = "fpSelectOption";
            this.fpSelectOption.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSelectOption.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSelectOption_Sheet1});
            this.fpSelectOption.Size = new System.Drawing.Size(488, 186);
            this.fpSelectOption.TabIndex = 0;
            this.fpSelectOption.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSelectOption.EditModeOn += new System.EventHandler(this.fpSelectOption_EditModeOn);
            this.fpSelectOption.SizeChanged += new System.EventHandler(this.fpSelectOption_SizeChanged);
            this.fpSelectOption.KeyUp += new System.Windows.Forms.KeyEventHandler(this.fpSelectOption_KeyUp);
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
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.fpSelectOption_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSelectOption_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSelectOption_Sheet1.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
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
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.fpSelectOption_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpSelectOption_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSelectOption_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSelectOption_Sheet1.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSelectOption_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSelectOption_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSelectOption_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSelectOption_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSelectOption_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlControl
            // 
            this.pnlControl.Controls.Add(this.butQuery);
            this.pnlControl.Controls.Add(this.BtnConfig);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlControl.Location = new System.Drawing.Point(493, 61);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(30, 186);
            this.pnlControl.TabIndex = 4;
            // 
            // butQuery
            // 
            this.butQuery.BackColor = System.Drawing.Color.Wheat;
            this.butQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.butQuery.Image = ((System.Drawing.Image)(resources.GetObject("butQuery.Image")));
            this.butQuery.Location = new System.Drawing.Point(0, 74);
            this.butQuery.Name = "butQuery";
            this.butQuery.Size = new System.Drawing.Size(30, 112);
            this.butQuery.TabIndex = 0;
            this.butQuery.UseVisualStyleBackColor = false;
            this.butQuery.Click += new System.EventHandler(this.butQuery_Click);
            // 
            // BtnConfig
            // 
            this.BtnConfig.BackColor = System.Drawing.Color.Gray;
            this.BtnConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnConfig.Image = ((System.Drawing.Image)(resources.GetObject("BtnConfig.Image")));
            this.BtnConfig.Location = new System.Drawing.Point(0, 0);
            this.BtnConfig.Name = "BtnConfig";
            this.BtnConfig.Size = new System.Drawing.Size(30, 74);
            this.BtnConfig.TabIndex = 6;
            this.BtnConfig.UseVisualStyleBackColor = false;
            this.BtnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.butUp);
            this.panel2.Controls.Add(this.butDown);
            this.panel2.Controls.Add(this.chkLastTest);
            this.panel2.Controls.Add(this.chkDate);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.dtpStart);
            this.panel2.Controls.Add(this.dtpEnd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(518, 56);
            this.panel2.TabIndex = 2;
            // 
            // butUp
            // 
            this.butUp.BackColor = System.Drawing.Color.Orange;
            this.butUp.Image = ((System.Drawing.Image)(resources.GetObject("butUp.Image")));
            this.butUp.Location = new System.Drawing.Point(449, 3);
            this.butUp.Name = "butUp";
            this.butUp.Size = new System.Drawing.Size(30, 32);
            this.butUp.TabIndex = 0;
            this.butUp.UseVisualStyleBackColor = false;
            this.butUp.Visible = false;
            this.butUp.Click += new System.EventHandler(this.butUp_Click);
            // 
            // butDown
            // 
            this.butDown.BackColor = System.Drawing.Color.Orange;
            this.butDown.Image = ((System.Drawing.Image)(resources.GetObject("butDown.Image")));
            this.butDown.Location = new System.Drawing.Point(485, 3);
            this.butDown.Name = "butDown";
            this.butDown.Size = new System.Drawing.Size(30, 32);
            this.butDown.TabIndex = 0;
            this.butDown.UseVisualStyleBackColor = false;
            this.butDown.Visible = false;
            this.butDown.Click += new System.EventHandler(this.butDown_Click);
            // 
            // chkLastTest
            // 
            this.chkLastTest.AutoSize = true;
            this.chkLastTest.Location = new System.Drawing.Point(173, 13);
            this.chkLastTest.Name = "chkLastTest";
            this.chkLastTest.Size = new System.Drawing.Size(77, 16);
            this.chkLastTest.TabIndex = 8;
            this.chkLastTest.Text = "Last Test";
            this.chkLastTest.UseVisualStyleBackColor = true;
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(173, 34);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(98, 16);
            this.chkDate.TabIndex = 8;
            this.chkDate.Text = "Not Use Date";
            this.chkDate.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "To";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "From";
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy-MM-dd";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(63, 4);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(104, 21);
            this.dtpStart.TabIndex = 0;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy-MM-dd";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(63, 31);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(104, 21);
            this.dtpEnd.TabIndex = 0;
            // 
            // pnlRunLoad
            // 
            this.pnlRunLoad.Controls.Add(this.fpSpread1);
            this.pnlRunLoad.Controls.Add(this.panel3);
            this.pnlRunLoad.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlRunLoad.Location = new System.Drawing.Point(0, 330);
            this.pnlRunLoad.Name = "pnlRunLoad";
            this.pnlRunLoad.Size = new System.Drawing.Size(528, 270);
            this.pnlRunLoad.TabIndex = 61;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.AsNeeded;
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.Location = new System.Drawing.Point(0, 0);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(528, 229);
            this.fpSpread1.TabIndex = 0;
            this.fpSpread1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread1_CellDoubleClick);
            this.fpSpread1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fpSpread1_MouseDown);
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread1_Sheet1.AutoCalculation = false;
            this.fpSpread1_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
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
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpSpread1_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSpread1_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread1_Sheet1.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread1_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.fpSpread1_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.butRun);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 229);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5);
            this.panel3.Size = new System.Drawing.Size(528, 41);
            this.panel3.TabIndex = 2;
            // 
            // butRun
            // 
            this.butRun.Dock = System.Windows.Forms.DockStyle.Right;
            this.butRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butRun.Image = ((System.Drawing.Image)(resources.GetObject("butRun.Image")));
            this.butRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butRun.Location = new System.Drawing.Point(352, 5);
            this.butRun.Name = "butRun";
            this.butRun.Size = new System.Drawing.Size(171, 31);
            this.butRun.TabIndex = 0;
            this.butRun.Text = "  Search";
            this.butRun.Click += new System.EventHandler(this.butRun_Click);
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
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 322);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(528, 8);
            this.splitter1.TabIndex = 63;
            this.splitter1.TabStop = false;
            // 
            // ctxMnu
            // 
            this.ctxMnu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAll,
            this.mnuNone,
            this.mnuInvert,
            this.mnuWhere});
            // 
            // mnuAll
            // 
            this.mnuAll.Index = 0;
            this.mnuAll.Text = "Select All";
            this.mnuAll.Click += new System.EventHandler(this.mnuAll_Click);
            // 
            // mnuNone
            // 
            this.mnuNone.Index = 1;
            this.mnuNone.Text = "Select None";
            this.mnuNone.Click += new System.EventHandler(this.mnuNone_Click);
            // 
            // mnuInvert
            // 
            this.mnuInvert.Index = 2;
            this.mnuInvert.Text = "Select Invert";
            this.mnuInvert.Click += new System.EventHandler(this.mnuInvert_Click);
            // 
            // mnuWhere
            // 
            this.mnuWhere.Index = 3;
            this.mnuWhere.Text = "Select Where..";
            this.mnuWhere.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel4.Controls.Add(this.cmbRecipe);
            this.panel4.Controls.Add(this.BtnSave);
            this.panel4.Controls.Add(this.BtnDelete);
            this.panel4.Controls.Add(this.BtnLoad);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(528, 0);
            this.panel4.TabIndex = 72;
            // 
            // cmbRecipe
            // 
            this.cmbRecipe.Location = new System.Drawing.Point(72, 2);
            this.cmbRecipe.Name = "cmbRecipe";
            this.cmbRecipe.Size = new System.Drawing.Size(226, 21);
            this.cmbRecipe.TabIndex = 2;
            // 
            // BtnSave
            // 
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSave.Location = new System.Drawing.Point(220, 30);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(76, 29);
            this.BtnSave.TabIndex = 1;
            this.BtnSave.Text = "  Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("BtnDelete.Image")));
            this.BtnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDelete.Location = new System.Drawing.Point(138, 30);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(76, 29);
            this.BtnDelete.TabIndex = 1;
            this.BtnDelete.Text = "  Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLoad.Image = ((System.Drawing.Image)(resources.GetObject("BtnLoad.Image")));
            this.BtnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnLoad.Location = new System.Drawing.Point(56, 30);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(76, 29);
            this.BtnLoad.TabIndex = 1;
            this.BtnLoad.Text = "  Load";
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "Recipe";
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 63;
            this.ultraSplitter1.Size = new System.Drawing.Size(528, 15);
            this.ultraSplitter1.TabIndex = 73;
            this.ultraSplitter1.Visible = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 267);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(528, 8);
            this.splitter2.TabIndex = 74;
            this.splitter2.TabStop = false;
            // 
            // trvWaferList
            // 
            this.trvWaferList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvWaferList.ImageList = this.imageList1;
            this.trvWaferList.Location = new System.Drawing.Point(0, 275);
            this.trvWaferList.Name = "trvWaferList";
            _override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.Single;
            this.trvWaferList.Override = _override1;
            this.trvWaferList.PathSeparator = ",";
            this.trvWaferList.Size = new System.Drawing.Size(528, 47);
            this.trvWaferList.TabIndex = 75;
            this.trvWaferList.DoubleClick += new System.EventHandler(this.trvWaferList_DoubleClick);
            // 
            // TPUCSelectWafer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.Controls.Add(this.trvWaferList);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.pnlSelectWafer);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlRunLoad);
            this.Name = "TPUCSelectWafer";
            this.Size = new System.Drawing.Size(528, 600);
            this.Load += new System.EventHandler(this.TPUCSelectWafer_Load);
            this.pnlSelectWafer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectOption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectOption_Sheet1)).EndInit();
            this.pnlControl.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlRunLoad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRecipe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvWaferList)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void TPUCSelectWafer_Load(
            object sender, 
            System.EventArgs e
            )
        {
            if (DesignMode) return;
            Initialize();
#if DEBUG
            Debug.WriteLine(String.Format("Tag: {0}", this.Tag));
#endif

            strMydocPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux", "TEST");
            fnRecipeListUp();
        }

        string ConditionFile()
        {
            DirectoryInfo oDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux"));
            if (oDir.Exists == false)
                oDir.Create();

            return string.Format(@"{0}\{1}", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux"), this.conditionSaveFile);
        }

        public void Initialize(string[] strConfig = null)
        {
            string strTemp = string.Empty;

            FileInfo oFile = null;
            DataTable dtOption = null;
            DataSet ds = null;

            string[] strArrItem = null;
            try
            {
                //Default 는 "LOT_ID",  "PRODUCT", "STEP_ID",  "INSPECTION_EQ" 이며 이외의 경우 설정값 기준으로 사용된다.
                if (strConfig == null)
                    strArrItem = new string[] { "LOT_ID", "DEVICE_ALIAS", "TESTAREA", "PROGRAM" };
                else
                    strArrItem = strConfig;

                FarPoint.Win.Spread.CellType.CheckBoxCellType chkCell = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                FarPoint.Win.Spread.CellType.TextCellType TxtCell = new FarPoint.Win.Spread.CellType.TextCellType();
                cmbCell.Items = cmbCell.ItemData = new string[] { "=", "!=", "<", ">" };
                cmbCell.EditorValue = FarPoint.Win.Spread.CellType.EditorValue.ItemData;

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
                        fpSelectOption.ActiveSheet.SetValue(ir, 2, " ");
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
                        if (dr["NAME"].ToString() == "chkDate")
                            chkDate.Checked = bool.Parse(dr["VALUE"].ToString());

                        if (dr["NAME"].ToString() == "chkLastTest")
                            chkLastTest.Checked = bool.Parse(dr["VALUE"].ToString());
                    }
                }

                //입력되었던 값중 Lot ID 는 초기화 한다.
                for (int ir = 0; ir < fpSelectOption.ActiveSheet.RowCount; ir++)
                {
                    if (fpSelectOption.ActiveSheet.Cells[ir, 1].Value.ToString() == "LOT_ID")
                        fpSelectOption.ActiveSheet.Cells[ir, 3].Value = null;
                }

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

        private void fpSelectOption_SizeChanged(object sender, System.EventArgs e)
        {
            fpSelectOption.ActiveSheet.Columns[0].Width = 24;
            fpSelectOption.ActiveSheet.Columns[1].Width = 100;
            fpSelectOption.ActiveSheet.Columns[2].Width = 50;
            fpSelectOption.ActiveSheet.Columns[3].Width = Math.Max(70, fpSelectOption.Width - 26 - (fpSelectOption.ActiveSheet.Columns[0].Width
                + fpSelectOption.ActiveSheet.Columns[1].Width
                + fpSelectOption.ActiveSheet.Columns[2].Width));
        }

        private void butUp_Click(object sender, System.EventArgs e)
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

        private void butDown_Click(object sender, System.EventArgs e)
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

        private void butQuery_Click(object sender, System.EventArgs e)
        {
            DataSelect oWaferSum = new DataSelect();

            DataTable dt = null;
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
                Utility.FPSpreadUtil.InitSpread(fpSpread1);
                trvWaferList.Nodes.Clear();
                m_strSort = "";
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
                    strWhereValue = (string)fpSelectOption.ActiveSheet.GetText(i, 3).Replace("'", "").Replace("*", "%").ToUpper().Trim();

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

                if (m_strSort.Length > 0)
                {
                    m_strSort = m_strSort.Substring(1);
                }

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                if (chkDate.Checked == true)
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

                dt = oWaferSum.GetTESTWaferList(dtpStart.Value.ToString("yyyyMMdd"), dtpEnd.Value.AddDays(1).ToString("yyyyMMdd"), wherePara.ToArray(), m_strSort, chkDate.Checked, chkLastTest.Checked);
                if (dt == null || dt.Rows.Count <= 0 )
                {
                    MessageBox.Show("Not Found Data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dt.TableName = "WAFERLIST";
                dtWaferList = dt.Copy();

                if (m_strSort.Length > 0)
                {
                    FillTree(dtWaferList, m_strSort);
                }

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                fpSpread1_Sheet1.DataSource = dtWaferList;
                //Utility.FPSpreadUtil.SpreadSortingAll(fpSpread1_Sheet1);
                Utility.FPSpreadUtil.AdjustSpread(fpSpread1_Sheet1);
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
                fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;

                SaveCurrentSaveOption();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("You cannot get the data.[Err:{0}]", ex.Message));
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();

                wherePara = null;

                MainForm.SetStatusMessage(null);
            }
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
                            if (bExist == false)
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

                            if (bExist == false)
                            {
                                Infragistics.Win.UltraWinTree.UltraTreeNode tn2 = new Infragistics.Win.UltraWinTree.UltraTreeNode(string.Format("{0}_{1}_{2}", drs[i][strFields[ic]].ToString(), i, ic), drs[i][strFields[ic]].ToString());
                                tn2.Override.NodeAppearance.Image = imageIdx;
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
                string[] strPath = trvWaferList.SelectedNodes[0].FullPath.Split(',');
                string filter = "";
                DataView dv = new DataView(dtWaferList);

                for (int i = 0; i < strPath.Length; i++)
                {
                    filter = filter + string.Format(" AND {0} = '{1}' ", strFields[i], strPath[i]);
                }

                dv.RowFilter = filter.Substring(4);
                dv.Sort = m_strSort;

                if (dv.Count <= 0)
                {
                    fpSpread1_Sheet1.Rows.Clear();
                }
                else
                {
                    Utility.FPSpreadUtil.InitSpread(fpSpread1);
                    fpSpread1_Sheet1.DataSource = dv.ToTable();
                    //Utility.FPSpreadUtil.SpreadSortingAll(fpSpread1_Sheet1);
                    Utility.FPSpreadUtil.AdjustSpread(fpSpread1_Sheet1);
                    Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
                }

                fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
                fpSpread1_Sheet1.Models.Selection.ClearSelection();
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
                 fpSelectOption.ActiveSheet.Cells[fpSelectOption.ActiveSheet.ActiveRowIndex, 3].Value != null &&
                string.IsNullOrEmpty(fpSelectOption.ActiveSheet.Cells[fpSelectOption.ActiveSheet.ActiveRowIndex, 3].Value.ToString()) == false)
            {
                butQuery_Click(null, null);
            }
        }

        private void butRun_Click(
            object sender, 
            System.EventArgs e
            )
        {
            if (fpSpread1_Sheet1 == null 
                || fpSpread1_Sheet1.DataSource == null 
                || fpSpread1_Sheet1.RowCount <= 0) 
                return;

            ArrayList arrDatas = new ArrayList();

            DataView dv = ((DataTable)fpSpread1_Sheet1.DataSource).DefaultView;
            DataTable dtSheet  = null;
            TPWafer[] oTPSelDatas = null;
            TPWafer oWafer;

            try
            {
                MainForm.SetStatusMessage("조회를 시작 합니다.");
                FarPoint.Win.Spread.Model.CellRange[] cr = fpSpread1_Sheet1.GetSelections();
                
                //선택된 Row 가 없을 경우 전체 조회 한다.
                if (cr.Length <= 0)
                {
                    dtSheet = ((DataTable)fpSpread1_Sheet1.DataSource).Copy();
                    if (dtSheet == null || dtSheet.Rows.Count <= 0)
                        return;

                    for (int ir = 0; ir < dtSheet.Rows.Count; ir++)
                    {
                        oWafer = new TPWafer();
                        oWafer.Testarea = dtSheet.Rows[ir]["TESTAREA"].ToString();
                        oWafer.Product = dtSheet.Rows[ir]["DEVICE_ALIAS"].ToString();
                        oWafer.Program = dtSheet.Rows[ir]["PROGRAM"].ToString();
                        oWafer.WaferSeq = dtSheet.Rows[ir]["WAFER_SEQ"].ToString();
                        oWafer.LotID = dtSheet.Rows[ir]["LOT_ID"].ToString();
                        oWafer.WaferID = dtSheet.Rows[ir]["WAFER_ID"].ToString();
                        arrDatas.Add(oWafer);
                    }
                }
                else
                {
                    for (int i = 0; i < cr.Length; i++)
                    {
                        for (int r = 0; r < cr[i].RowCount; r++)
                        {
                            oWafer = new TPWafer();
                            oWafer.Testarea = dv[cr[i].Row + r]["TESTAREA"].ToString();
                            //oWafer.Product = dv[cr[i].Row + r]["PRODUCT"].ToString();
                            oWafer.Product = dv[cr[i].Row + r]["DEVICE_ALIAS"].ToString();
                            oWafer.Program = dv[cr[i].Row + r]["PROGRAM"].ToString();
                            oWafer.WaferSeq = dv[cr[i].Row + r]["WAFER_SEQ"].ToString();
                            oWafer.LotID = dv[cr[i].Row + r]["LOT_ID"].ToString();
                            oWafer.WaferID = dv[cr[i].Row + r]["WAFER_ID"].ToString();
                            arrDatas.Add(oWafer);
                        }
                    }
                }

                oTPSelDatas = new TPWafer[arrDatas.Count];

                if (oTPSelDatas.Length > 100)
                {
                    if (MessageBox.Show(string.Format("현재 선택한 Wafer 가 100개가 넘습니다. 계속 진행 하시겠습니까? (현: {0})", oTPSelDatas.Length), "확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        return;
                }

                arrDatas.CopyTo(oTPSelDatas);

                MainForm.SetStatusMessage("Data 를 조회 중입니다.");
                if (OnSelected != null) OnSelected(this, oTPSelDatas);

                MainForm.SetStatusMessage("Data를 처리 중입니다.");
                //Main Form 기준 Active 되어 있는 Form 으로 조회 List 를 던져 준다.
                var child = FindForm().ActiveMdiChild as DACrux.TEST.Interface.iTESTControl;
                if (child != null)
                    child.DrawWafer(oTPSelDatas);
            }
            finally
            {
                if (oTPSelDatas != null)
                    oTPSelDatas = null;

                MainForm.SetStatusMessage(null);
            }
        }

        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader || e.RowHeader)
                return;

            butRun.PerformClick();
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
                dtOption.Rows.Add(new object[] { "chkDate", chkDate.Checked.ToString() });
                dtOption.Rows.Add(new object[] { "chkLastTest", chkLastTest.Checked.ToString() });
                dtOption.AcceptChanges();

                if (dtOption != null && dtOption.Rows.Count > 0)
                {
                    dtOption.WriteXml(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DACrux"), SearchOption));
                }
            }
            catch (Exception) { }
        }

        private void fpSpread1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ctxMnu.Show(fpSpread1, new Point(e.X, e.Y));
            }
        }

        private void mnuAll_Click(object sender, System.EventArgs e)
        {
            fpSpread1_Sheet1.Models.Selection.AddSelection(0, 0, fpSpread1_Sheet1.RowCount, fpSpread1_Sheet1.ColumnCount);
        }

        private void mnuNone_Click(object sender, System.EventArgs e)
        {
            fpSpread1_Sheet1.Models.Selection.ClearSelection();
        }

        private void mnuInvert_Click(object sender, System.EventArgs e)
        {
            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
            {
                if (fpSpread1_Sheet1.Models.Selection.IsSelected(i, 0))
                {
                    fpSpread1_Sheet1.Models.Selection.RemoveSelection(i, 0, 1, fpSpread1_Sheet1.ColumnCount);
                }
                else
                {
                    fpSpread1_Sheet1.Models.Selection.AddSelection(i, 0, 1, fpSpread1_Sheet1.ColumnCount);
                }
            }
        }

        public bool MultiWafer
        {
            set
            {
                m_isMultiSelect = value;
                if (!m_isMultiSelect) fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
                else fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;

            }
            get
            {
                return m_isMultiSelect;
            }
        }

        //public void SetTestArea(string[] TestArea)
        //{
        //    try
        //    {
        //        TestArea = TestArea;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(string.Format("Testarea를 지정할 수 없습니다.[Err:{0}]", ex.Message));
        //    }
        //}


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
            string strFullPath = string.Empty;
            FileInfo oFile = null;

            Assembly assemblyMap = null;
            Type type = null;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (string.IsNullOrEmpty(cmbRecipe.Text) == true)
                    throw new Exception("Recipe ID 입력 확인.");


                strFullPath = Path.Combine(strMydocPath, cmbRecipe.Text);

                if (MessageBox.Show("해당 Recipe 를 Load 하시겠습니까? Load 시 현재 열려 있는 TEST 화면들은 사라집니다.", "확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;

                dtMenu = GetMenu();
                if (dtMenu == null || dtMenu.Rows.Count <= 0)
                    throw new Exception("Menu File 을 불러 올 수 없습니다. 관리자에 문의 하세요.");


                //현재 Open 된 화면에 대해 close 한다.
                foreach (Form fm in this.ParentForm.MdiChildren)
                {
                    if (dtMenu.Select(string.Format("RESV_04 = '{0}' AND ASSEMBLY = '{1}'", "TEST", fm.GetType().FullName)).Length > 0)
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

                    Utility.FPSpreadUtil.InitSpread(fpSpread1);
                    fpSpread1_Sheet1.DataSource = dtWaferList;
                    Utility.FPSpreadUtil.AdjustSpread(fpSpread1_Sheet1);
                    Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread1_Sheet1);
                    fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
                    fpSpread1_Sheet1.Models.Selection.ClearSelection();

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
                    if (dtMenu.Select(string.Format("RESV_04 = '{0}' AND ASSEMBLY = '{1}'", "TEST", fm.GetType().FullName)).Length > 0)
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

    }
}
