using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using DACrux.Common.RO;
using DACrux.Map;
using DACrux.SEMDMS.RO;
using DACrux.Base;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// DPUCPatternSearch에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCPatternSearch : DACrux.Framework.Base.DACruxCTLBasic01, DACrux.Framework.Base.IExportExcel
    {
        public delegate void EventHandlerPattenrSearch(DACrux.Base.DPWafer[] wafer);
        public event EventHandlerPattenrSearch OnGallery;

        DACrux.Base.DPWafer[] wafer = null;
        System.Threading.Thread thread = null;
        DataTable dtWaferMap = null;
        DataTable dtDefectData = null;

        private DACrux.Map.DefectMap sDefectMap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelSearchButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel6;
        private DACrux.Map.DefectMap tDefectMap;
        private System.Windows.Forms.Splitter splitter1;
        private FarPoint.Win.Spread.FpSpread fpSpreadTarget;
        private FarPoint.Win.Spread.SheetView fpSpreadTarget_Sheet;
        private FarPoint.Win.Spread.FpSpread fpSpreadFind;
        private FarPoint.Win.Spread.SheetView fpSpreadFind_Sheet;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxDefectCntOnDie;
        private System.Windows.Forms.TextBox textBoxMatchingRate;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonSearch;
        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Button buttonGallery;
        private System.Windows.Forms.Splitter splitter4;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ToolWaferDieLocation;
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public DPUCPatternSearch()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPUCPatternSearch));
            this.sDefectMap = new DACrux.Map.DefectMap();
            this.fpSpreadTarget = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadTarget_Sheet = new FarPoint.Win.Spread.SheetView();
            this.fpSpreadFind = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadFind_Sheet = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ultraProgressBar = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBoxMatchingRate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelSearchButton = new System.Windows.Forms.Panel();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxDefectCntOnDie = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tDefectMap = new DACrux.Map.DefectMap();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.buttonGallery = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.buttonClear = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolWaferDieLocation = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadTarget_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadFind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadFind_Sheet)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelSearchButton.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sDefectMap
            // 
            this.sDefectMap.AngleOffSet = 0;
            this.sDefectMap.CenterMark = false;
            this.sDefectMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.sDefectMap.DataSource = null;
            this.sDefectMap.DieBackgroundColor = System.Drawing.Color.Black;
            this.sDefectMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.sDefectMap.DieDefectColor = System.Drawing.Color.Empty;
            this.sDefectMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.sDefectMap.DieMaxX = 0;
            this.sDefectMap.DieMaxY = 0;
            this.sDefectMap.DieMinX = 0;
            this.sDefectMap.DieMinY = 0;
            this.sDefectMap.DieSizeX = 0.01D;
            this.sDefectMap.DieSizeY = 0.01D;
            this.sDefectMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.sDefectMap.DisplayValue = "BIN";
            this.sDefectMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sDefectMap.DrawDefectImage = null;
            this.sDefectMap.DrawDefects = "ALL";
            this.sDefectMap.DrawFirstDie = true;
            this.sDefectMap.DrawMarkDie = false;
            this.sDefectMap.DrawOriginDie = true;
            this.sDefectMap.DrawSkipDie = true;
            this.sDefectMap.EdgeColor = System.Drawing.Color.LightGray;
            this.sDefectMap.EdgeSize = 1D;
            this.sDefectMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.sDefectMap.FirstDieX = 0;
            this.sDefectMap.FirstDieY = 0;
            this.sDefectMap.ForeColor = System.Drawing.Color.Red;
            this.sDefectMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.sDefectMap.GradationInterval = 5;
            this.sDefectMap.GradationMaxValue = double.NaN;
            this.sDefectMap.GradationMinValue = double.NaN;
            this.sDefectMap.Location = new System.Drawing.Point(0, 0);
            this.sDefectMap.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.sDefectMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.sDefectMap.Name = "sDefectMap";
            this.sDefectMap.NotchAngle = 0;
            this.sDefectMap.NotchType = DACrux.Base.Notch.Flat;
            this.sDefectMap.OriginDieBorder = System.Drawing.Color.Red;
            this.sDefectMap.OriginIndexX = 0;
            this.sDefectMap.OriginIndexY = 0;
            this.sDefectMap.OriginX = 0D;
            this.sDefectMap.OriginY = 0D;
            this.sDefectMap.ParaLimit = false;
            this.sDefectMap.ParametricColumn = "PCMVALUE";
            this.sDefectMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.sDefectMap.PickupDieAlpha = 96;
            this.sDefectMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.sDefectMap.PopupMenu = true;
            this.sDefectMap.ReferenceDieSetting = 0;
            this.sDefectMap.ScaleMark = false;
            this.sDefectMap.SelecetedBin = "ALL";
            this.sDefectMap.SelectedVI = "ALL";
            this.sDefectMap.Size = new System.Drawing.Size(466, 560);
            this.sDefectMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.sDefectMap.TabIndex = 0;
            this.sDefectMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.sDefectMap.TransParent = 255;
            this.sDefectMap.ViewAngle = 0;
            this.sDefectMap.VIMember = "VIFAIL";
            this.sDefectMap.VisibleDieBorder = true;
            this.sDefectMap.VisibleDieValue = false;
            this.sDefectMap.VisibleFocusDie = false;
            this.sDefectMap.VisibleImageMark = true;
            this.sDefectMap.VisibleInfomation = true;
            this.sDefectMap.VisibleOffDie = false;
            this.sDefectMap.VisibleProbeOverlay = false;
            this.sDefectMap.VisibleShotAlignPoint = false;
            this.sDefectMap.VisibleSignDies = false;
            this.sDefectMap.VisibleStringBin = false;
            this.sDefectMap.VisibleVIFail = false;
            this.sDefectMap.VisibleXY = false;
            this.sDefectMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.sDefectMap.WaferColor = System.Drawing.Color.Gray;
            this.sDefectMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.sDefectMap.WaferID = "";
            this.sDefectMap.WaferMargin = 0.95D;
            this.sDefectMap.WaferSize = 200000D;
            this.sDefectMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.sDefectMap.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.sDefectMap_OnChangeCurrentDie);
            // 
            // fpSpreadTarget
            // 
            this.fpSpreadTarget.AccessibleDescription = "";
            this.fpSpreadTarget.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fpSpreadTarget.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadTarget.Location = new System.Drawing.Point(0, 154);
            this.fpSpreadTarget.Name = "fpSpreadTarget";
            this.fpSpreadTarget.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadTarget.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadTarget_Sheet});
            this.fpSpreadTarget.Size = new System.Drawing.Size(427, 184);
            this.fpSpreadTarget.TabIndex = 1;
            this.fpSpreadTarget.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fpSpreadTarget_MouseUp);
            // 
            // fpSpreadTarget_Sheet
            // 
            this.fpSpreadTarget_Sheet.Reset();
            fpSpreadTarget_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadTarget_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadTarget_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadTarget_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTarget_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTarget_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTarget_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTarget_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadTarget_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTarget_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTarget_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTarget_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTarget_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadTarget_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTarget_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTarget_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTarget_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTarget_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadTarget_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadTarget_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadTarget_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTarget_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadTarget_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTarget_Sheet.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTarget_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTarget_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTarget_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadTarget_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTarget_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTarget_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTarget_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTarget_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadTarget_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTarget_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // fpSpreadFind
            // 
            this.fpSpreadFind.AccessibleDescription = "";
            this.fpSpreadFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadFind.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpreadFind.Location = new System.Drawing.Point(0, 23);
            this.fpSpreadFind.Name = "fpSpreadFind";
            this.fpSpreadFind.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadFind.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadFind_Sheet});
            this.fpSpreadFind.Size = new System.Drawing.Size(427, 81);
            this.fpSpreadFind.TabIndex = 2;
            this.fpSpreadFind.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpSpreadFind_Sheet
            // 
            this.fpSpreadFind_Sheet.Reset();
            fpSpreadFind_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadFind_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadFind_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadFind_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFind_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFind_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFind_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFind_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadFind_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFind_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFind_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFind_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFind_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadFind_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFind_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFind_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFind_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFind_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadFind_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadFind_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadFind_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFind_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadFind_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFind_Sheet.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFind_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFind_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFind_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadFind_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFind_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFind_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFind_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFind_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadFind_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFind_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(427, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "     Target Wafer";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ultraProgressBar);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panelSearchButton);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 341);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 136);
            this.panel1.TabIndex = 4;
            // 
            // ultraProgressBar
            // 
            this.ultraProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraProgressBar.Location = new System.Drawing.Point(0, 71);
            this.ultraProgressBar.Name = "ultraProgressBar";
            this.ultraProgressBar.Size = new System.Drawing.Size(427, 25);
            this.ultraProgressBar.TabIndex = 3;
            this.ultraProgressBar.Text = "[Formatted]";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textBoxMatchingRate);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 47);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(427, 24);
            this.panel3.TabIndex = 2;
            // 
            // textBoxMatchingRate
            // 
            this.textBoxMatchingRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMatchingRate.Location = new System.Drawing.Point(112, 0);
            this.textBoxMatchingRate.Name = "textBoxMatchingRate";
            this.textBoxMatchingRate.Size = new System.Drawing.Size(315, 21);
            this.textBoxMatchingRate.TabIndex = 1;
            this.textBoxMatchingRate.Text = "50";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Matching Rate";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelSearchButton
            // 
            this.panelSearchButton.Controls.Add(this.buttonStop);
            this.panelSearchButton.Controls.Add(this.buttonPause);
            this.panelSearchButton.Controls.Add(this.buttonSearch);
            this.panelSearchButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSearchButton.Location = new System.Drawing.Point(0, 96);
            this.panelSearchButton.Name = "panelSearchButton";
            this.panelSearchButton.Size = new System.Drawing.Size(427, 40);
            this.panelSearchButton.TabIndex = 1;
            this.panelSearchButton.SizeChanged += new System.EventHandler(this.panelSearchButton_SizeChanged);
            // 
            // buttonStop
            // 
            this.buttonStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(144, 0);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(283, 40);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonPause.Enabled = false;
            this.buttonPause.Location = new System.Drawing.Point(72, 0);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(72, 40);
            this.buttonPause.TabIndex = 1;
            this.buttonPause.Text = "Pause";
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSearch.Location = new System.Drawing.Point(0, 0);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(72, 40);
            this.buttonSearch.TabIndex = 0;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxDefectCntOnDie);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 23);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(427, 24);
            this.panel2.TabIndex = 3;
            // 
            // textBoxDefectCntOnDie
            // 
            this.textBoxDefectCntOnDie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDefectCntOnDie.Location = new System.Drawing.Point(112, 0);
            this.textBoxDefectCntOnDie.Name = "textBoxDefectCntOnDie";
            this.textBoxDefectCntOnDie.Size = new System.Drawing.Size(315, 21);
            this.textBoxDefectCntOnDie.TabIndex = 1;
            this.textBoxDefectCntOnDie.Text = "1";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = "Def. Cnt On Die >";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(427, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "     Search Condition";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tDefectMap);
            this.panel4.Controls.Add(this.splitter2);
            this.panel4.Controls.Add(this.fpSpreadTarget);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(427, 338);
            this.panel4.TabIndex = 5;
            // 
            // tDefectMap
            // 
            this.tDefectMap.AngleOffSet = 0;
            this.tDefectMap.CenterMark = false;
            this.tDefectMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.tDefectMap.DataSource = null;
            this.tDefectMap.DieBackgroundColor = System.Drawing.Color.Black;
            this.tDefectMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.tDefectMap.DieDefectColor = System.Drawing.Color.Empty;
            this.tDefectMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.tDefectMap.DieMaxX = 0;
            this.tDefectMap.DieMaxY = 0;
            this.tDefectMap.DieMinX = 0;
            this.tDefectMap.DieMinY = 0;
            this.tDefectMap.DieSizeX = 0.01D;
            this.tDefectMap.DieSizeY = 0.01D;
            this.tDefectMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.tDefectMap.DisplayValue = "BIN";
            this.tDefectMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tDefectMap.DrawDefectImage = null;
            this.tDefectMap.DrawDefects = "ALL";
            this.tDefectMap.DrawFirstDie = true;
            this.tDefectMap.DrawMarkDie = false;
            this.tDefectMap.DrawOriginDie = true;
            this.tDefectMap.DrawSkipDie = true;
            this.tDefectMap.EdgeColor = System.Drawing.Color.LightGray;
            this.tDefectMap.EdgeSize = 1D;
            this.tDefectMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.tDefectMap.FirstDieX = 0;
            this.tDefectMap.FirstDieY = 0;
            this.tDefectMap.ForeColor = System.Drawing.Color.Red;
            this.tDefectMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.tDefectMap.GradationInterval = 5;
            this.tDefectMap.GradationMaxValue = double.NaN;
            this.tDefectMap.GradationMinValue = double.NaN;
            this.tDefectMap.Location = new System.Drawing.Point(0, 23);
            this.tDefectMap.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.tDefectMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.tDefectMap.Name = "tDefectMap";
            this.tDefectMap.NotchAngle = 0;
            this.tDefectMap.NotchType = DACrux.Base.Notch.Flat;
            this.tDefectMap.OriginDieBorder = System.Drawing.Color.Red;
            this.tDefectMap.OriginIndexX = 0;
            this.tDefectMap.OriginIndexY = 0;
            this.tDefectMap.OriginX = 0D;
            this.tDefectMap.OriginY = 0D;
            this.tDefectMap.ParaLimit = false;
            this.tDefectMap.ParametricColumn = "PCMVALUE";
            this.tDefectMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.tDefectMap.PickupDieAlpha = 96;
            this.tDefectMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.tDefectMap.PopupMenu = true;
            this.tDefectMap.ReferenceDieSetting = 0;
            this.tDefectMap.ScaleMark = false;
            this.tDefectMap.SelecetedBin = "ALL";
            this.tDefectMap.SelectedVI = "ALL";
            this.tDefectMap.Size = new System.Drawing.Size(427, 128);
            this.tDefectMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.tDefectMap.TabIndex = 4;
            this.tDefectMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.tDefectMap.TransParent = 255;
            this.tDefectMap.ViewAngle = 0;
            this.tDefectMap.VIMember = "VIFAIL";
            this.tDefectMap.VisibleDieBorder = true;
            this.tDefectMap.VisibleDieValue = false;
            this.tDefectMap.VisibleFocusDie = false;
            this.tDefectMap.VisibleImageMark = true;
            this.tDefectMap.VisibleInfomation = true;
            this.tDefectMap.VisibleOffDie = false;
            this.tDefectMap.VisibleProbeOverlay = false;
            this.tDefectMap.VisibleShotAlignPoint = false;
            this.tDefectMap.VisibleSignDies = false;
            this.tDefectMap.VisibleStringBin = false;
            this.tDefectMap.VisibleVIFail = false;
            this.tDefectMap.VisibleXY = false;
            this.tDefectMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.tDefectMap.WaferColor = System.Drawing.Color.Gray;
            this.tDefectMap.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.tDefectMap.WaferID = "";
            this.tDefectMap.WaferMargin = 0.95D;
            this.tDefectMap.WaferSize = 200000D;
            this.tDefectMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 151);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(427, 3);
            this.splitter2.TabIndex = 5;
            this.splitter2.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.fpSpreadFind);
            this.panel5.Controls.Add(this.buttonGallery);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 480);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(427, 128);
            this.panel5.TabIndex = 6;
            // 
            // buttonGallery
            // 
            this.buttonGallery.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonGallery.Enabled = false;
            this.buttonGallery.Location = new System.Drawing.Point(0, 104);
            this.buttonGallery.Name = "buttonGallery";
            this.buttonGallery.Size = new System.Drawing.Size(427, 24);
            this.buttonGallery.TabIndex = 5;
            this.buttonGallery.Text = "Gallery";
            this.buttonGallery.Click += new System.EventHandler(this.buttonGallery_Click);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(427, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "     Find Wafer";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel4);
            this.panel6.Controls.Add(this.splitter4);
            this.panel6.Controls.Add(this.panel1);
            this.panel6.Controls.Add(this.splitter3);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(469, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(427, 608);
            this.panel6.TabIndex = 7;
            // 
            // splitter4
            // 
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter4.Location = new System.Drawing.Point(0, 338);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(427, 3);
            this.splitter4.TabIndex = 8;
            this.splitter4.TabStop = false;
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter3.Location = new System.Drawing.Point(0, 477);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(427, 3);
            this.splitter3.TabIndex = 7;
            this.splitter3.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(466, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 608);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // buttonClear
            // 
            this.buttonClear.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonClear.Location = new System.Drawing.Point(0, 584);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(466, 24);
            this.buttonClear.TabIndex = 9;
            this.buttonClear.Text = "Clear";
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolWaferDieLocation});
            this.statusStrip1.Location = new System.Drawing.Point(0, 560);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(466, 24);
            this.statusStrip1.TabIndex = 10;
            // 
            // ToolWaferDieLocation
            // 
            this.ToolWaferDieLocation.AutoSize = false;
            this.ToolWaferDieLocation.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolWaferDieLocation.Name = "ToolWaferDieLocation";
            this.ToolWaferDieLocation.Size = new System.Drawing.Size(150, 19);
            this.ToolWaferDieLocation.Text = "X : 0, Y : 0";
            // 
            // DPUCPatternSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.Controls.Add(this.sDefectMap);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel6);
            this.Name = "DPUCPatternSearch";
            this.Size = new System.Drawing.Size(896, 608);
            this.Load += new System.EventHandler(this.DPUCPatternSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadTarget_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadFind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadFind_Sheet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelSearchButton.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public DACrux.Base.DPWafer[] Wafer
        {
            get
            {
                return this.wafer;
            }
            set
            {
                this.wafer = value;
            }
        }

        private void DPUCPatternSearch_Load(object sender, System.EventArgs e)
        {
            //TQD_INSP_INFO o = null;
            DataSet ds = null;
            try
            {
                /*
#if DEBUG
                o = new TQD_INSP_INFO();
                ds = o.GetStepInfo(new string [] {"20151", "20170", "20190", "20210"});
                fpSpreadTarget_Sheet.DataSource = ds.Tables[0];
#endif
*/
                fpSpreadTarget_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode | FarPoint.Win.Spread.OperationMode.ReadOnly;
                fpSpreadFind_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode | FarPoint.Win.Spread.OperationMode.ReadOnly;
                sDefectMap.DefectSize = tDefectMap.DefectSize = GetDefaultDefectSize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
                if (ds != null) ds.Dispose();
                ds = null;

                //o = null;
            }
        }

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();

            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : DACrux.Map.DefectMap.DEFAULT_DEFECT_SIZE;
        }

        public void ListUpStepInfo()
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            DataTable dt = null;
            try
            {

                long[] stepSeq = new long[this.wafer.Length];
                for (int i = 0; i < stepSeq.Length; i++) stepSeq[i] = DACrux.Base.Convert.longParse(this.wafer[i].StepSeq);
                oDMapAnalysis = new DACrux.SEMDMS.RO.DefectMapAnalysis();
                dt = oDMapAnalysis.GetStepInfo(stepSeq);

                fpSpreadTarget_Sheet.DataSource = dt;

                DACrux.Utility.FPSpreadUtil.SetCellTypeToIntByDataTable(ref fpSpreadTarget_Sheet, dt);

                //FarPoint.Win.Spread.CellType.NumberCellType numberCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                //numberCell.DecimalPlaces = 0;

                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    try
                //    {
                //        long num = DACrux.Base.Convert.longParse(dt.Rows[0][i].ToString());
                //        fpSpreadTarget_Sheet.Columns[i].CellType = numberCell;
                //    }
                //    catch
                //    {
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;

                oDMapAnalysis = null;
            }
        }

        private void fpSpreadTarget_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                FarPoint.Win.Spread.Model.CellRange cr = fpSpreadTarget_Sheet.GetSelection(0);
                if (cr.Row == -1) return;
                long stepSeq = DACrux.Base.Convert.longParse(fpSpreadTarget_Sheet.Cells[cr.Row, 0].Value.ToString());
                DrawDefect(this.sDefectMap, stepSeq);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        public void DrawDefect(Map.DefectMap oMap, long stepSeq, bool ImageMark = false)
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            ComConfiguration oComConfig = null;
            DataSet dsDefect = null;
            DataTable dt = null;

            try
            {
                dsDefect = new DataSet();

                oDMapAnalysis = new RO.DefectMapAnalysis();
                oComConfig = new ComConfiguration();

                StatusMessage("기준 정보를 조회 중입니다.");
                dsDefect = oDMapAnalysis.GetDefectMapViewer_Info(new long[] { stepSeq });
                if (dsDefect.Tables.IndexOf("STEP_INFO") < 0 || dsDefect.Tables["STEP_INFO"].Rows.Count <= 0)
                    throw new Exception("정의된 Step 정보가 없습니다.");

                if (dsDefect.Tables.IndexOf("SETUP_INFO") < 0)
                    throw new Exception("정의된 Setup 정보가 없습니다.");

                string[] strSetupArr = new string[dsDefect.Tables["STEP_INFO"].Rows.Count];
                string[] strTestArr = new string[dsDefect.Tables["STEP_INFO"].Rows.Count];

                for (int iStep = 0; iStep < dsDefect.Tables["STEP_INFO"].Rows.Count; iStep++)
                {
                    strSetupArr[iStep] = dsDefect.Tables["STEP_INFO"].Rows[iStep]["SETUP_SEQ"].ToString();
                    strTestArr[iStep] = dsDefect.Tables["STEP_INFO"].Rows[iStep]["TEST"].ToString();
                }

                StatusMessage("Map 을 조회 중입니다.");
                dtWaferMap = oDMapAnalysis.GetDefectMapViewer_Map(new long[] { stepSeq }, strSetupArr, strTestArr);

                if (dtDefectData != null)
                    dtDefectData.Dispose();

                dtDefectData = null;

                StatusMessage("Defect 을 조회 중입니다.");
                dtDefectData = oDMapAnalysis.GetDefectMapViewer_Defects(new long[] { stepSeq });

                StatusMessage("Data를 처리 중입니다.");
                //map.SetDensityEnable = checkBoxDensity.Checked;
                //map.SetDensity = "ALL";
                sDefectMap.WaferSize = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["WAFER_SIZE"].ToString());
                sDefectMap.NotchType = DACrux.Base.Notch.Notch; // dsDefect.Tables["SETUP_INFO"].Rows[0]["NOTCH_TYPE"].ToString().Equals("F") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;
                sDefectMap.AngleOffSet = 0;
                sDefectMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                sDefectMap.NotchAngle = 0;// DOWN으로 저장하여 보여주므로 0으로 설정 DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ANGLE"].ToString());

                sDefectMap.DieSizeX = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_X"].ToString());
                sDefectMap.DieSizeY = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_Y"].ToString());
                sDefectMap.OriginIndexX = DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_X"].ToString());
                sDefectMap.OriginIndexY = DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_Y"].ToString());
                sDefectMap.OriginX = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_X"].ToString());
                sDefectMap.OriginY = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_Y"].ToString());

                // SHOT 정보 설정
                if (dsDefect.Tables.Contains("STEP_INFO") && dsDefect.Tables["STEP_INFO"].Rows.Count > 0 && dsDefect.Tables["STEP_INFO"].Columns.Contains("ST_XCNT"))
                {
                    DataRow stepRow = dsDefect.Tables["STEP_INFO"].Rows[0];
                    sDefectMap.ShotArrayX = DACrux.Base.Convert.intParse(stepRow["ST_XCNT"].ToString(), 1);
                    sDefectMap.ShotArrayY = DACrux.Base.Convert.intParse(stepRow["ST_YCNT"].ToString(), 1);
                    sDefectMap.ShotStartX = DACrux.Base.Convert.intParse(stepRow["ST_START_X"].ToString(), 1);
                    sDefectMap.ShotStartY = DACrux.Base.Convert.intParse(stepRow["ST_START_Y"].ToString(), 1);
                }

                sDefectMap.DieCalculation(true);
                sDefectMap.DrawDefects = "ALL";

                //=================================================================================================================================
                //Setup Map 정보를 가져 온다.
                //=================================================================================================================================
                sDefectMap.DieClear();

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
                            sDefectMap.VirtualDieColor = System.Drawing.ColorTranslator.FromHtml(dr["VALUE"].ToString());
                        }

                        if (dr["NAME"].ToString() == "VISIBLE")
                        {
                            if (dr["VALUE"].ToString() == "Y")
                                WaferMap.AppendVirtualDie(sDefectMap);
                        }
                    }
                }

                foreach (DataRow dr in dtWaferMap.Rows)
                {
                    sDefectMap.AddDie(new DACrux.Base.Die(
                        DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString()),
                        DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString()),
                        DACrux.Base.Convert.intParse(dr["TEST"].ToString()),
                        1
                        ));
                }

                // Shot Start Index 재조정 2019.12.26 Taihi,Kim.
                DefectMapDraw.ReadjustShotStartIndex(sDefectMap);

                DefectList defects = null;
                defects = DefectMapAnalysis.DataTableToDefectList(dtDefectData);
                sDefectMap.DefectClear();
                sDefectMap.Defects.AddRange(defects);
                sDefectMap.SetInfomation(DefectMapDraw.GetMapDescription(defects));

                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                DataTable dtMapOption = oComConfig.SelectDefectMapConfig(DACrux.Base.GlobalVariable.Factory, DACrux.Base.GlobalVariable.UserID);
                if (dtMapOption != null && dtMapOption.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMapOption.Rows)
                    {
                        string strType = dr["NAME"].ToString();
                        Color crType = ColorTranslator.FromHtml(dr["VALUE"].ToString());

                        switch (strType)
                        {
                            //Wafer Base Color
                            case "WAFER_MAP_BG":
                                sDefectMap.WaferColor = crType;
                                break;
                            //Inspection Die Color
                            case "WAFER_MAP_INP":
                                sDefectMap.DieBackgroundColor = crType;
                                break;
                            //Inspection Defect Die Color
                            case "WAFER_MAP_DEFECT":
                                sDefectMap.DieDefectColor = crType;
                                break;
                            //Wafer Border Line Color
                            case "WAFER_MAP_LINE":
                                sDefectMap.DieBorderColor = crType;
                                break;

                        }
                    }
                }

                dt = oComConfig.GetConfigurationUser(
                    DACrux.Base.GlobalVariable.Factory,
                    "WAFER_OPTION",
                    DACrux.Base.GlobalVariable.UserID
                    );

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> sWaferInfo = new List<string>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dsDefect.Tables["STEP_INFO"].Columns.Contains(dr["NAME"].ToString()))
                        {
                            DataTable dtList = dsDefect.Tables["STEP_INFO"].DefaultView.ToTable(true, dr["NAME"].ToString());
                            List<string> lsList = new List<string>();

                            foreach (DataRow drList in dtList.Rows)
                            {
                                lsList.Add(drList[0].ToString());
                            }

                            if (lsList.Count > 0)
                                sWaferInfo.Add(string.Format("{0}: {1}", dr["VALUE"], string.Join(",", lsList.ToArray())));
                        }
                    }
                    sDefectMap.SetInfomation(sWaferInfo.ToArray());
                }

                if (dsDefect.Tables.Contains("STEP_INFO") && dsDefect.Tables["STEP_INFO"].Rows.Count > 0 && dsDefect.Tables["STEP_INFO"].Columns.Contains("WAFER_ID"))
                {
                    sDefectMap.WaferID = dsDefect.Tables["STEP_INFO"].Rows[0]["WAFER_ID"].ToString();
                }

                //Wafer Information 사용 여부
                dt = oComConfig.GetConfigurationUser(
                    DACrux.Base.GlobalVariable.Factory,
                    "WAFER_OPTION_ENABLE",
                    DACrux.Base.GlobalVariable.UserID
                    );

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["VALUE"].ToString() == "N")
                        sDefectMap.SetInfomation(null);
                }

                sDefectMap.DefectSize = GetDefaultDefectSize();

                //=================================================================================================================================
                //Data 기준으로 Map 을 Draw 한다. 
                //=================================================================================================================================
                sDefectMap.WaferDrawMode = DACrux.Map.MapMode.Fit;

                //Map 선택이 Defect Select 가 최우선 순위로 바꿔 준다.
                sDefectMap.DieSelectMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StatusMessage(null);
            }
        }

        private void buttonClear_Click(object sender, System.EventArgs e)
        {
            sDefectMap.ResetSelectedDie();
        }

        private void panelSearchButton_SizeChanged(object sender, System.EventArgs e)
        {
            if (DesignMode) return;
            try
            {
                int ctlCnt = panelSearchButton.Controls.Count;
                int w = panelSearchButton.Width / ctlCnt;
                for (int i = 1; i < panelSearchButton.Controls.Count; i++) panelSearchButton.Controls[i].Width = w;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        void Compare()
        {
            List<Point> sSelectDie = null;
            List<DACrux.Base.Defect> tDefect = null;
            DataTable dt = null;

            try
            {
                if (this.thread.ThreadState == System.Threading.ThreadState.AbortRequested)
                    return;

                dt = ((DataTable)fpSpreadTarget_Sheet.DataSource).Clone();

                sSelectDie = sDefectMap.SelectedDies;
                int matchingRate = DACrux.Base.Convert.intParse(textBoxMatchingRate.Text);

                long stepSeq = -1;
                for (int i = 0; i < fpSpreadTarget_Sheet.RowCount; i++)
                {
                    stepSeq = DACrux.Base.Convert.longParse(fpSpreadTarget_Sheet.Cells[i, 0].Value.ToString());
                    DefectMapDraw.DrawDefect(this.tDefectMap, stepSeq, ref tDefect);

                    if (tDefect != null && MatchingRate(sSelectDie, tDefect.ToArray()) >= matchingRate)
                    {
                        dt.Rows.Add(DACrux.Utility.FPSpreadUtil.GetRowToObject(fpSpreadTarget_Sheet, i));
                    }
                    SetProgressValue(i + 1);
                }

                SetSpreadDataSource(dt);
            }
            finally
            {
                sSelectDie = null;
                tDefect = null;

                //if(dt != null) dt.Dispose();
                //dt = null;

                fnBtnVisible(buttonStop, false);
                fnBtnVisible(buttonPause, false);
                fnBtnVisible(buttonSearch, true);
                //DACrux.Base.WithThread.EnableButton(buttonStop, false);
                //DACrux.Base.WithThread.EnableButton(buttonPause, false);
                //DACrux.Base.WithThread.EnableButton(buttonSearch, true);
                SetProgressValue(0);
            }
        }

        public void fnBtnVisible(Button objBtn, bool bVisible)
        {
            if (objBtn.InvokeRequired)
            {
                objBtn.BeginInvoke(new MethodInvoker(delegate { fnBtnVisible(objBtn, bVisible); }));

                return;
            }

            objBtn.Enabled = bVisible;
        }

        private void SetSpreadDataSource(DataTable dt)
        {
            if (this.Disposing)
            {
                return;
            }

            if (fpSpreadFind.InvokeRequired)
            {
                fpSpreadFind.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SetSpreadDataSource(dt);
                    }
                ));
            }
            else
            {
                if (dt != null && dt.Rows.Count > 0)
                    fnBtnVisible(buttonGallery, true);
                else fnBtnVisible(buttonGallery, false);

                fpSpreadFind_Sheet.DataSource = dt;
                DACrux.Utility.FPSpreadUtil.SetCellTypeToIntByDataTable(ref fpSpreadFind_Sheet, dt);

                //FarPoint.Win.Spread.CellType.NumberCellType numberCell = new FarPoint.Win.Spread.CellType.NumberCellType();
                //numberCell.DecimalPlaces = 0;

                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    try
                //    {
                //        long num = DACrux.Base.Convert.longParse(dt.Rows[0][i].ToString());
                //        fpSpreadFind_Sheet.Columns[i].CellType = numberCell;
                //    }
                //    catch
                //    {
                //    }
                //}
            }
        }

        private void SetProgressValue(int value)
        {
            if (this.Disposing)
            {
                return;
            }

            if (ultraProgressBar.InvokeRequired)
            {
                ultraProgressBar.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        SetProgressValue(value);
                    }
                ));
            }
            else
            {
                ultraProgressBar.Value = value;
            }
        }

        int MatchingRate(List<Point> sSelectDie, DACrux.Base.Defect[] tDefect)
        {
            int selectCnt = sSelectDie.Count;
            int matchingDieCnt = 0;
            for (int i = 0; i < sSelectDie.Count; i++)
            {
                if (OverDefectCnt(sSelectDie[i], tDefect)) matchingDieCnt++;
            }

            return (int)((float)matchingDieCnt / (float)selectCnt * 100.0f);
        }

        bool OverDefectCnt(Point sDie, DACrux.Base.Defect[] tDefect)
        {
            int defectCntOnDie = DACrux.Base.Convert.intParse(textBoxDefectCntOnDie.Text);
            int cnt = 0;
            DACrux.Base.Defect defect;
            for (int i = 0; i < tDefect.Length; i++)
            {
                defect = tDefect[i];
                if (defect.XINDEX == sDie.X && defect.YINDEX == sDie.Y)
                {
                    cnt++;
                    if (cnt >= defectCntOnDie) return true;
                }
            }
            return false;
        }

        private void buttonSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                buttonStop.Enabled = true;
                buttonPause.Enabled = true;
                buttonSearch.Enabled = false;

                ultraProgressBar.Maximum = fpSpreadTarget_Sheet.RowCount;

                this.thread = new System.Threading.Thread(new System.Threading.ThreadStart(Compare));
                this.thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.thread.Abort();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void buttonPause_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (buttonPause.Text.Equals("Pause"))
                {
                    //this.thread.Suspend();
                    buttonPause.Text = "Resume";
                    buttonStop.Enabled = false;
                }
                else
                {
                    //this.thread.Resume();
                    buttonPause.Text = "Pause";
                    buttonStop.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        DACrux.Base.DPWafer[] SearchMapCollection()
        {
            try
            {
                if (fpSpreadFind_Sheet.RowCount == 0)
                    return null;

                DACrux.Base.DPWafer[] wafer = new DACrux.Base.DPWafer[fpSpreadFind_Sheet.RowCount];
                DataTable dt = (DataTable)fpSpreadFind_Sheet.DataSource;
                for (int i = 0; i < wafer.Length; i++)
                {
                    wafer[i].LotID = string.Format("{0}", dt.Rows[i]["LOT_ID"]);
                    wafer[i].Product = string.Format("{0}", dt.Rows[i]["PRODUCT"]);
                    wafer[i].StepSeq = string.Format("{0}", dt.Rows[i]["STEP_SEQ"]);
                }
                return wafer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void buttonGallery_Click(object sender, System.EventArgs e)
        {
            try
            {
                DACrux.Base.DPWafer[] wafer = SearchMapCollection();
                OnGallery(wafer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        private void sDefectMap_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            try
            {
                ToolWaferDieLocation.Text = string.Format("X : {0} Y : {1}", NewDie.IndexX, NewDie.IndexY);
            }
            catch (Exception)
            {
            }
        }

        public Base.Defect[] GetSelectedDefect()
        {
            if (sDefectMap.SelectedDefect.Count > 0)
                return sDefectMap.SelectedDefect.ToArray();
            else
                return sDefectMap.Defects.ToArray();
        }

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add(sDefectMap);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Map";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add((DataTable)fpSpreadTarget.DataSource);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "DataRow";

            sheet = new DACrux.Utility.ExcelSheet();
            sheet.Add((DataTable)fpSpreadFind.DataSource);
            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Result";

            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion
    }
}
