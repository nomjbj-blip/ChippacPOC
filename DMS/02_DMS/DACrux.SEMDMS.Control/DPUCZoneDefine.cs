using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
//using TESTPlus.Admin.RO;
using DACrux.Base;
using DACrux.SEMDMS.RO;
using DACrux.Map;
using System.Collections.Generic;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// TPUCZoneDefine에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCZoneDefine : System.Windows.Forms.UserControl
    {
        string userId = "hhmstill";

        private System.Windows.Forms.Panel plnLeft;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUserID;
        private DACrux.Map.ZoneWaferMap m_zwMap;
        private System.Windows.Forms.Button butRemoveZoneID;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button butAddZoneID;
        private DataSet m_DS = null;
        private FarPoint.Win.Spread.FpSpread fpMap;
        private FarPoint.Win.Spread.SheetView fpMap_Sheet1;

        private System.Windows.Forms.Splitter splitter1;

        private string m_selDevice = string.Empty;
        private Color m_selColor = Color.White;
        private string m_selZoneID = string.Empty;
        private System.Windows.Forms.TextBox txtNewZone;
        private FarPoint.Win.Spread.FpSpread fpZoneID;
        private FarPoint.Win.Spread.SheetView fpZoneID_Sheet1;
        private int m_iZoneIdx = 0;
        private System.Windows.Forms.RadioButton rdoAdd;
        private System.Windows.Forms.RadioButton rdoOverWrite;
        private System.Windows.Forms.RadioButton rdoDelete;
        private ArrayList m_UndoArray = null;
        private System.Windows.Forms.Button butUpdate;
        private System.Windows.Forms.Button butCancel;
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.Container components = null;
        public DPUCZoneDefine()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();
            // TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

        }

        public void Initialize()
        {
            FillDeviceList();

            if (m_DS != null) m_DS.Dispose();
            m_DS = new DataSet();
            m_DS.Tables.Add("MAP");
            m_DS.Tables["MAP"].Columns.Add(new DataColumn("X", System.Type.GetType("System.Int32")));
            m_DS.Tables["MAP"].Columns.Add(new DataColumn("Y", System.Type.GetType("System.Int32")));
            m_DS.Tables["MAP"].Columns.Add(new DataColumn("ZONEID", System.Type.GetType("System.String")));
            m_DS.Tables["MAP"].Columns.Add(new DataColumn("COLOR", System.Type.GetType("System.String")));

            m_DS.Tables.Add("ZONEIDS");
            m_DS.Tables["ZONEIDS"].Columns.Add(new DataColumn("ZONEID", System.Type.GetType("System.String")));
            m_DS.Tables["ZONEIDS"].Columns.Add(new DataColumn("COLOR", System.Type.GetType("System.String")));

            fpMap_Sheet1.DataSource = m_DS.Tables["MAP"];
            fpZoneID_Sheet1.DataAutoSizeColumns = false;
            fpZoneID_Sheet1.DataSource = m_DS.Tables["ZONEIDS"];
            fpZoneID_Sheet1.Columns[0].Width = (int)(fpZoneID.Width * 0.6f);
            fpZoneID_Sheet1.Columns[1].Width = (int)(fpZoneID.Width * 0.2f);
            if (m_UndoArray != null)
            {
                m_UndoArray.Clear();
                m_UndoArray = null;
            }
            m_UndoArray = new ArrayList();
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
                    m_zwMap.Dispose();
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPUCZoneDefine));
            this.m_zwMap = new DACrux.Map.ZoneWaferMap();
            this.plnLeft = new System.Windows.Forms.Panel();
            this.fpMap = new FarPoint.Win.Spread.FpSpread();
            this.fpMap_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.butUpdate = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoAdd = new System.Windows.Forms.RadioButton();
            this.fpZoneID = new FarPoint.Win.Spread.FpSpread();
            this.fpZoneID_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.butRemoveZoneID = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDevice = new System.Windows.Forms.Label();
            this.txtNewZone = new System.Windows.Forms.TextBox();
            this.butAddZoneID = new System.Windows.Forms.Button();
            this.rdoOverWrite = new System.Windows.Forms.RadioButton();
            this.rdoDelete = new System.Windows.Forms.RadioButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.plnLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpMap_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpZoneID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpZoneID_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_zwMap
            // 
            this.m_zwMap.AngleOffSet = 0;
            this.m_zwMap.CenterMark = false;
            this.m_zwMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.m_zwMap.DataSource = null;
            this.m_zwMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.m_zwMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.m_zwMap.DieMaxX = 0;
            this.m_zwMap.DieMaxY = 0;
            this.m_zwMap.DieMinX = 0;
            this.m_zwMap.DieMinY = 0;
            this.m_zwMap.DieSizeX = 0.01D;
            this.m_zwMap.DieSizeY = 0.01D;
            this.m_zwMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.m_zwMap.DisplayValue = "BIN";
            this.m_zwMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_zwMap.DrawFirstDie = true;
            this.m_zwMap.DrawMarkDie = false;
            this.m_zwMap.DrawOriginDie = true;
            this.m_zwMap.DrawSkipDie = true;
            this.m_zwMap.EdgeColor = System.Drawing.Color.LightGray;
            this.m_zwMap.EdgeSize = 1D;
            this.m_zwMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.m_zwMap.FirstDieX = 0;
            this.m_zwMap.FirstDieY = 0;
            this.m_zwMap.ForeColor = System.Drawing.Color.Red;
            this.m_zwMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.m_zwMap.GradationInterval = 5;
            this.m_zwMap.GradationMaxValue = double.NaN;
            this.m_zwMap.GradationMinValue = double.NaN;
            this.m_zwMap.Location = new System.Drawing.Point(0, 0);
            this.m_zwMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.m_zwMap.Name = "m_zwMap";
            this.m_zwMap.NotchAngle = 0;
            this.m_zwMap.NotchType = DACrux.Base.Notch.Flat;
            this.m_zwMap.OriginDieBorder = System.Drawing.Color.Red;
            this.m_zwMap.OriginIndexX = 0;
            this.m_zwMap.OriginIndexY = 0;
            this.m_zwMap.OriginX = 0D;
            this.m_zwMap.OriginY = 0D;
            this.m_zwMap.ParametricColumn = "PCMVALUE";
            this.m_zwMap.PickupDieAlpha = 96;
            this.m_zwMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.m_zwMap.PopupMenu = true;
            this.m_zwMap.ReferenceDieSetting = 0;
            this.m_zwMap.ScaleMark = false;
            this.m_zwMap.SelecetedBin = "ALL";
            this.m_zwMap.SelectedVI = "ALL";
            this.m_zwMap.Size = new System.Drawing.Size(540, 540);
            this.m_zwMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_zwMap.TabIndex = 0;
            this.m_zwMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.m_zwMap.TransParent = 255;
            this.m_zwMap.ViewAngle = 0;
            this.m_zwMap.VIMember = "VIFAIL";
            this.m_zwMap.VisibleDieBorder = true;
            this.m_zwMap.VisibleDieValue = false;
            this.m_zwMap.VisibleFocusDie = false;
            this.m_zwMap.VisibleInfomation = true;
            this.m_zwMap.VisibleOffDie = false;
            this.m_zwMap.VisibleStringBin = false;
            this.m_zwMap.VisibleVIFail = false;
            this.m_zwMap.VisibleXY = false;
            this.m_zwMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.m_zwMap.WaferColor = System.Drawing.Color.Gray;
            this.m_zwMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.m_zwMap.WaferMargin = 0.95D;
            this.m_zwMap.WaferSize = 200000D;
            this.m_zwMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
            this.m_zwMap.OnSelectDies += new DACrux.Map.SelectDies(this.m_zwMap_OnSelectDies);
            // 
            // plnLeft
            // 
            this.plnLeft.Controls.Add(this.fpMap);
            this.plnLeft.Controls.Add(this.panel3);
            this.plnLeft.Controls.Add(this.panel1);
            this.plnLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.plnLeft.Location = new System.Drawing.Point(548, 0);
            this.plnLeft.Name = "plnLeft";
            this.plnLeft.Size = new System.Drawing.Size(272, 540);
            this.plnLeft.TabIndex = 1;
            // 
            // fpMap
            // 
            this.fpMap.AccessibleDescription = "";
            this.fpMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpMap.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpMap.Location = new System.Drawing.Point(0, 256);
            this.fpMap.Name = "fpMap";
            this.fpMap.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpMap.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpMap_Sheet1});
            this.fpMap.Size = new System.Drawing.Size(272, 252);
            this.fpMap.TabIndex = 55;
            this.fpMap.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpMap_Sheet1
            // 
            this.fpMap_Sheet1.Reset();
            fpMap_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpMap_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpMap_Sheet1.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpMap_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpMap_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpMap_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpMap_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpMap_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpMap_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpMap_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpMap_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpMap_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpMap_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpMap_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpMap_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpMap_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpMap_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpMap_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpMap_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpMap_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.fpMap_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpMap_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpMap_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpMap_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpMap_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpMap_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpMap_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpMap_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpMap_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpMap_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpMap_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpMap_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMap_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.butUpdate);
            this.panel3.Controls.Add(this.butCancel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 508);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 32);
            this.panel3.TabIndex = 56;
            // 
            // butUpdate
            // 
            this.butUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butUpdate.BackgroundImage")));
            this.butUpdate.Location = new System.Drawing.Point(160, 8);
            this.butUpdate.Name = "butUpdate";
            this.butUpdate.Size = new System.Drawing.Size(46, 19);
            this.butUpdate.TabIndex = 0;
            this.butUpdate.Click += new System.EventHandler(this.butUpdate_Click);
            // 
            // butCancel
            // 
            this.butCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butCancel.BackgroundImage")));
            this.butCancel.Location = new System.Drawing.Point(216, 8);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(46, 19);
            this.butCancel.TabIndex = 0;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(272, 256);
            this.panel1.TabIndex = 54;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel2.Controls.Add(this.rdoAdd);
            this.panel2.Controls.Add(this.fpZoneID);
            this.panel2.Controls.Add(this.txtUserID);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.cmbDevice);
            this.panel2.Controls.Add(this.butRemoveZoneID);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblDevice);
            this.panel2.Controls.Add(this.txtNewZone);
            this.panel2.Controls.Add(this.butAddZoneID);
            this.panel2.Controls.Add(this.rdoOverWrite);
            this.panel2.Controls.Add(this.rdoDelete);
            this.panel2.Location = new System.Drawing.Point(8, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(256, 240);
            this.panel2.TabIndex = 54;
            // 
            // rdoAdd
            // 
            this.rdoAdd.Checked = true;
            this.rdoAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoAdd.Location = new System.Drawing.Point(8, 192);
            this.rdoAdd.Name = "rdoAdd";
            this.rdoAdd.Size = new System.Drawing.Size(48, 16);
            this.rdoAdd.TabIndex = 58;
            this.rdoAdd.TabStop = true;
            this.rdoAdd.Text = "Add";
            // 
            // fpZoneID
            // 
            this.fpZoneID.AccessibleDescription = "";
            this.fpZoneID.BackColor = System.Drawing.Color.LightSteelBlue;
            this.fpZoneID.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpZoneID.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpZoneID.Location = new System.Drawing.Point(112, 80);
            this.fpZoneID.Name = "fpZoneID";
            this.fpZoneID.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpZoneID.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpZoneID_Sheet1});
            this.fpZoneID.Size = new System.Drawing.Size(136, 104);
            this.fpZoneID.TabIndex = 57;
            this.fpZoneID.TabStripPolicy = FarPoint.Win.Spread.TabStripPolicy.Never;
            this.fpZoneID.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpZoneID_Sheet1
            // 
            this.fpZoneID_Sheet1.Reset();
            fpZoneID_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpZoneID_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpZoneID_Sheet1.ColumnHeader.RowCount = 0;
            fpZoneID_Sheet1.RowHeader.ColumnCount = 0;
            this.fpZoneID_Sheet1.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic1;
            this.fpZoneID_Sheet1.ColumnFooter.Columns.Default.Resizable = false;
            this.fpZoneID_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.fpZoneID_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpZoneID_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpZoneID_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpZoneID_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.fpZoneID_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpZoneID_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpZoneID_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpZoneID_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.ColumnHeader.Columns.Default.Resizable = false;
            this.fpZoneID_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.fpZoneID_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpZoneID_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpZoneID_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpZoneID_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.Columns.Default.Resizable = false;
            this.fpZoneID_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpZoneID_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpZoneID_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpZoneID_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpZoneID_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.fpZoneID_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpZoneID_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.fpZoneID_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpZoneID_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpZoneID_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpZoneID_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpZoneID_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.fpZoneID_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpZoneID_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpZoneID_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpZoneID_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpZoneID_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(112, 32);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(136, 20);
            this.txtUserID.TabIndex = 55;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightSlateGray;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(256, 20);
            this.label6.TabIndex = 54;
            this.label6.Text = "    Zone Define";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDevice
            // 
            this.cmbDevice.Location = new System.Drawing.Point(112, 56);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(136, 19);
            this.cmbDevice.TabIndex = 50;
            this.cmbDevice.SelectedIndexChanged += new System.EventHandler(this.cmbDevice_SelectedIndexChanged);
            // 
            // butRemoveZoneID
            // 
            this.butRemoveZoneID.BackColor = System.Drawing.SystemColors.Control;
            this.butRemoveZoneID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butRemoveZoneID.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.butRemoveZoneID.Image = ((System.Drawing.Image)(resources.GetObject("butRemoveZoneID.Image")));
            this.butRemoveZoneID.Location = new System.Drawing.Point(48, 160);
            this.butRemoveZoneID.Name = "butRemoveZoneID";
            this.butRemoveZoneID.Size = new System.Drawing.Size(65, 21);
            this.butRemoveZoneID.TabIndex = 52;
            this.butRemoveZoneID.UseVisualStyleBackColor = false;
            this.butRemoveZoneID.Click += new System.EventHandler(this.butRemoveZoneID_Click);
            // 
            // label2
            // 
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.Location = new System.Drawing.Point(8, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.TabIndex = 49;
            // 
            // label1
            // 
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 49;
            // 
            // lblDevice
            // 
            this.lblDevice.Image = ((System.Drawing.Image)(resources.GetObject("lblDevice.Image")));
            this.lblDevice.Location = new System.Drawing.Point(8, 56);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(100, 20);
            this.lblDevice.TabIndex = 49;
            // 
            // txtNewZone
            // 
            this.txtNewZone.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNewZone.Location = new System.Drawing.Point(48, 112);
            this.txtNewZone.Name = "txtNewZone";
            this.txtNewZone.Size = new System.Drawing.Size(64, 20);
            this.txtNewZone.TabIndex = 53;
            // 
            // butAddZoneID
            // 
            this.butAddZoneID.BackColor = System.Drawing.SystemColors.Control;
            this.butAddZoneID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butAddZoneID.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.butAddZoneID.Image = ((System.Drawing.Image)(resources.GetObject("butAddZoneID.Image")));
            this.butAddZoneID.Location = new System.Drawing.Point(48, 136);
            this.butAddZoneID.Name = "butAddZoneID";
            this.butAddZoneID.Size = new System.Drawing.Size(65, 21);
            this.butAddZoneID.TabIndex = 52;
            this.butAddZoneID.UseVisualStyleBackColor = false;
            this.butAddZoneID.Click += new System.EventHandler(this.butAddZoneID_Click);
            // 
            // rdoOverWrite
            // 
            this.rdoOverWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoOverWrite.Location = new System.Drawing.Point(72, 192);
            this.rdoOverWrite.Name = "rdoOverWrite";
            this.rdoOverWrite.Size = new System.Drawing.Size(80, 16);
            this.rdoOverWrite.TabIndex = 58;
            this.rdoOverWrite.Text = "Over Write";
            // 
            // rdoDelete
            // 
            this.rdoDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoDelete.Location = new System.Drawing.Point(168, 192);
            this.rdoDelete.Name = "rdoDelete";
            this.rdoDelete.Size = new System.Drawing.Size(56, 16);
            this.rdoDelete.TabIndex = 58;
            this.rdoDelete.Text = "Delete";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(540, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(8, 540);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // DPUCZoneDefine
            // 
            this.Controls.Add(this.m_zwMap);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.plnLeft);
            this.Name = "DPUCZoneDefine";
            this.Size = new System.Drawing.Size(820, 540);
            this.Load += new System.EventHandler(this.TPUCZoneDefine_Load);
            this.plnLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpMap_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpZoneID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpZoneID_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void TPUCZoneDefine_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;
            txtUserID.Text = this.userId;

            Initialize();
        }

        private void FillDeviceList()
        {
            DataTable dt = null;
            try
            {
                SEMConfiguration o = new SEMConfiguration();
                dt = o.GetProduct();
                string product = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    product = string.Format("{0}", dt.Rows[i]["PRODUCT"]);
                    cmbDevice.Items.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmbDevice_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DataTable dt = null;
            try
            {
                SEMConfiguration o = new SEMConfiguration();
                dt = o.GetZone(this.userId.ToUpper(), cmbDevice.Text);
                fpMap_Sheet1.DataSource = dt;

                SetMap(ref dt);


                //ProductInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        void ProductInfo()
        {
            DataTable dt = null;
            try
            {
                SEMConfiguration o = new SEMConfiguration();
                dt = o.GetInfo(cmbDevice.Text);
                if (dt.Rows.Count == 0) return;

                m_zwMap.WaferSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["WAFER_SIZE"].ToString());
                m_zwMap.NotchType = DACrux.Base.Notch.Notch; //dt.Rows[0]["NOTCH_TYPE"].ToString().Equals("F") ? Notch.Flat : Notch.Notch;
                m_zwMap.AngleOffSet = 0;
                m_zwMap.NotchAngle = DACrux.Base.Convert.intParse(dt.Rows[0]["ANGLE"].ToString());
                m_zwMap.WaferDrawMode = DACrux.Map.MapMode.Fit;

                m_zwMap.DieSizeX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["DIE_PITCH_X"].ToString());
                m_zwMap.DieSizeY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["DIE_PITCH_Y"].ToString()) ;
                m_zwMap.OriginIndexX = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_ORIGIN_X"].ToString());
                m_zwMap.OriginIndexY = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_ORIGIN_Y"].ToString());
                m_zwMap.OriginX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_X"].ToString());
                m_zwMap.OriginY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_Y"].ToString());
                m_zwMap.DieCalculation(true);
                m_zwMap.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetMap(ref DataTable dtDies)
        {
            DACrux.Common.RO.ComConfiguration oComConfig = new DACrux.Common.RO.ComConfiguration();

            DataTable dt = null;

            try
            {
                SEMConfiguration o = new SEMConfiguration();
                dt = o.GetInfo(cmbDevice.Text);

                //ds = oMapDef.GetMapDef(cmbDevice.SelectedValue.ToString());

                //m_zwMap.Reset();
                m_zwMap.WaferSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["WAFER_SIZE"].ToString());
                m_zwMap.NotchType = DACrux.Base.Notch.Notch; //dt.Rows[0]["NOTCH_TYPE"].ToString().Equals("F") ? Notch.Flat : Notch.Notch;
                m_zwMap.AngleOffSet = 0;
                m_zwMap.NotchAngle = DACrux.Base.Convert.intParse(dt.Rows[0]["ANGLE"].ToString());
                m_zwMap.WaferDrawMode = DACrux.Map.MapMode.Fit;

                m_zwMap.DieSizeX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["DIE_PITCH_X"].ToString());
                m_zwMap.DieSizeY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["DIE_PITCH_Y"].ToString());
                m_zwMap.OriginIndexX = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_ORIGIN_X"].ToString());
                m_zwMap.OriginIndexY = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_ORIGIN_Y"].ToString());
                m_zwMap.OriginX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_X"].ToString());
                m_zwMap.OriginY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_Y"].ToString());

                m_zwMap.DieClear();

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
                            m_zwMap.VirtualDieColor = System.Drawing.ColorTranslator.FromHtml(dr["VALUE"].ToString());
                        }

                        if (dr["NAME"].ToString() == "VISIBLE")
                        {
                            if (dr["VALUE"].ToString() == "Y")
                                WaferMap.AppendVirtualDie(m_zwMap);
                        }
                    }
                }

                for (int i = 0; i < dtDies.Rows.Count; i++)
                {

                    m_zwMap.AddDie(new DACrux.Base.Die(DACrux.Base.Convert.intParse(dtDies.Rows[i]["INDEX_X"].ToString())
                        , DACrux.Base.Convert.intParse(dtDies.Rows[i]["INDEX_Y"].ToString())
                        , 0
                        , 0, 0, 0, 0, 1));
                }

                m_iZoneIdx = 0;
                for (int i = 0; i < dtDies.Rows.Count; i++)
                {

                    m_zwMap.SetDieZone(DACrux.Base.Convert.intParse(dtDies.Rows[i]["INDEX_X"].ToString())
                            , DACrux.Base.Convert.intParse(dtDies.Rows[i]["INDEX_Y"].ToString())
                        //, m_arrZoneID.IndexOf(dtDies.Rows[i]["ZONEID"].ToString()) + 1);
                            , DACrux.Base.Convert.intParse(dtDies.Rows[i]["COLOR"].ToString()));

                }

                m_zwMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                m_zwMap.Redraw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("MapID [{0}] 정보를 가져올 수 없습니다[Err:{1}]", cmbDevice.SelectedValue.ToString(), ex.Message));
            }
        }

        private void butAddZoneID_Click(object sender, System.EventArgs e)
        {
            //TEST.RO.ProbeAdmin oColor = new TEST.RO.ProbeAdmin();


            //if(txtNewZone.Text.Trim().Length==0) return;
            //DataRow[] drs = m_DS.Tables["ZONEIDS"].Select(string.Format("ZONEID='{0}'",txtNewZone.Text.Trim()));
            //if(drs.Length>0)
            //{
            //    MessageBox.Show("This Zone Id is already in use");
            //    return;
            //}

            //string strColor = oColor.ColorString(m_iZoneIdx + 1);

            //m_DS.Tables["ZONEIDS"].Rows.Add(new object[] {txtNewZone.Text.Trim(),strColor});
            //m_DS.Tables["ZONEIDS"].AcceptChanges();
            //txtNewZone.Text = "";


            //fpZoneID_Sheet1.Cells[m_DS.Tables["ZONEIDS"].Rows.Count - 1,1].BackColor 
            //    = Color.FromArgb(DACrux.Base.Convert.intParse(strColor.Substring(0,3)),
            //                    DACrux.Base.Convert.intParse(strColor.Substring(3,3)),
            //                    DACrux.Base.Convert.intParse(strColor.Substring(6,3)) );

            //fpZoneID_Sheet1.ActiveRowIndex = m_DS.Tables["ZONEIDS"].Rows.Count - 1;

            //m_iZoneIdx++;

        }

        private void butRemoveZoneID_Click(object sender, System.EventArgs e)
        {
            DeleteZoneList();
        }

        private void DeleteZoneList()
        {
            if (fpZoneID_Sheet1.Rows.Count == 0)
                return;

            string strDelZoneID = fpZoneID_Sheet1.GetValue(fpZoneID_Sheet1.ActiveRowIndex, 0).ToString();

            DataRow[] drs = m_DS.Tables["MAP"].Select(string.Format("ZONEID='{0}'", strDelZoneID));
            foreach (DataRow dr in drs)
            {
                dr["ZONEID"] = "NONE";
                dr["COLOR"] = "255255255";
                m_zwMap.SetDieZone(DACrux.Base.Convert.intParse(dr["X"].ToString()), DACrux.Base.Convert.intParse(dr["Y"].ToString()), 0);
            }

            m_DS.Tables["ZONEIDS"].Rows.RemoveAt(fpZoneID_Sheet1.ActiveRowIndex);
            m_DS.AcceptChanges();
            m_zwMap.Redraw();
        }

        private void m_zwMap_OnSelectDies(object sender, List<Point> selectedDies)
        {
            int iMode = 0;
            if (rdoOverWrite.Checked) iMode = 1;
            if (rdoDelete.Checked) iMode = 2;

            m_UndoArray.Clear();

            //for(int i=0;i<alstDies.Count;i++)
            for (int i = 0; i < selectedDies.Count; i++)
            {
                //Point oDie = (Point)alstDies[i];
                Point oDie = selectedDies[i];
                DataRow[] drs = m_DS.Tables["MAP"].Select(string.Format("X={0} AND Y={1}", oDie.X, oDie.Y));
                if (iMode == 2)
                {
                    drs[0]["ZONEID"] = "NONE";
                    drs[0]["COLOR"] = "255255255";
                    m_zwMap.SetDieZone(oDie.X, oDie.Y, 0);
                }
                else if (iMode == 1)
                {
                    drs[0]["ZONEID"] = fpZoneID_Sheet1.GetValue(fpZoneID_Sheet1.ActiveRowIndex, 0).ToString();
                    drs[0]["COLOR"] = fpZoneID_Sheet1.GetValue(fpZoneID_Sheet1.ActiveRowIndex, 1).ToString();
                    m_zwMap.SetDieZone(oDie.X, oDie.Y, fpZoneID_Sheet1.ActiveRowIndex + 1);
                }
                else
                {
                    if (drs[0]["ZONEID"].ToString() == "NONE")
                    {
                        drs[0]["ZONEID"] = fpZoneID_Sheet1.GetValue(fpZoneID_Sheet1.ActiveRowIndex, 0).ToString();
                        drs[0]["COLOR"] = fpZoneID_Sheet1.GetValue(fpZoneID_Sheet1.ActiveRowIndex, 1).ToString();
                        m_zwMap.SetDieZone(oDie.X, oDie.Y, fpZoneID_Sheet1.ActiveRowIndex + 1);
                    }
                }
            }

            m_zwMap.Redraw();
            m_DS.Tables["MAP"].AcceptChanges();
            m_zwMap.ResetSelectedDie();
        }

        private void butCancel_Click(object sender, System.EventArgs e)
        {
            Initialize();
        }

        private void butUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                UpdateZone();
            }
            catch (Exception ex)
            {
                MessageBox.Show("수정사항을 저장하지 못했습니다.Err[{0}]", ex.Message);
            }
            finally
            {
            }
        }

        private void UpdateZone()
        {
            /*
            ZoneDefine oZoneDef = new ZoneDefine();
            try
            {
                string UserID = (string)LoadResist("USERID");
                oZoneDef.DeleteZoneMap(UserID,cmbDevice.Text);
                oZoneDef.CreateZoneMap(UserID,cmbDevice.Text,cmbDevice.SelectedValue.ToString(),ref m_DS);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                oZoneDef = null;
            }
            */
        }

        #region Resist Value 처리
        private object LoadResist(string strKey)
        {
            object objValue = null;
            try
            {
                Microsoft.Win32.RegistryKey SoftwareKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software", true);
                Microsoft.Win32.RegistryKey MiracomKey = SoftwareKey.OpenSubKey("Miracom", true);
                if (MiracomKey == null) MiracomKey = SoftwareKey.CreateSubKey("Miracom");
                Microsoft.Win32.RegistryKey TPlusKey = MiracomKey.OpenSubKey("TESTPlus", true);
                if (TPlusKey == null) TPlusKey = MiracomKey.CreateSubKey("TESTPlus");

                objValue = TPlusKey.GetValue(strKey);

                TPlusKey.Close();
                MiracomKey.Close();
                SoftwareKey.Close();
                return objValue;
            }
            catch
            {
                MessageBox.Show("이 시스템에는 Miracom Middleware에 대한 정보가 없습니다.");
            }
            return null;
        }

        private void WriteRegist(string strKey, object objValue)
        {
            string sCon = string.Empty;
            try
            {

                Microsoft.Win32.RegistryKey SoftwareKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software", true);

                Microsoft.Win32.RegistryKey MiracomKey = SoftwareKey.OpenSubKey("Miracom", true);
                if (MiracomKey == null)
                {
                    MiracomKey = SoftwareKey.CreateSubKey("Miracom");
                }

                Microsoft.Win32.RegistryKey TPlusKey = MiracomKey.OpenSubKey("TESTPlus", true);
                if (TPlusKey == null)
                {
                    TPlusKey = MiracomKey.CreateSubKey("TESTPlus");
                }

                TPlusKey.SetValue(strKey, objValue);

                TPlusKey.Close();
                MiracomKey.Close();
                SoftwareKey.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
