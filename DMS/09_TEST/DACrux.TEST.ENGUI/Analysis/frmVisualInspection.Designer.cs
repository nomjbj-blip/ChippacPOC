namespace DACrux.TEST.ENGUI
{
    partial class frmVisualInspection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVisualInspection));
            FarPoint.Win.Spread.CellType.ImageCellType imageCellType3 = new FarPoint.Win.Spread.CellType.ImageCellType();
            FarPoint.Win.Spread.CellType.ImageCellType imageCellType4 = new FarPoint.Win.Spread.CellType.ImageCellType();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.lblX = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.txtXIndex = new System.Windows.Forms.TextBox();
            this.txtYIndex = new System.Windows.Forms.TextBox();
            this.txtEDS = new System.Windows.Forms.TextBox();
            this.lblBin = new System.Windows.Forms.Label();
            this.txtVI = new System.Windows.Forms.TextBox();
            this.lblVI = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_ewMap = new DACrux.Map.EditWaferMap();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.butClose = new System.Windows.Forms.Button();
            this.butReset = new System.Windows.Forms.Button();
            this.butSave = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanel2
            // 
            appearance2.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance2.ImageBackground")));
            appearance2.ImageBackgroundStretchMargins = new Infragistics.Win.ImageBackgroundStretchMargins(35, 0, 600, 0);
            this.ultraPanel2.Appearance = appearance2;
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.lblX);
            this.ultraPanel2.ClientArea.Controls.Add(this.lblY);
            this.ultraPanel2.ClientArea.Controls.Add(this.txtXIndex);
            this.ultraPanel2.ClientArea.Controls.Add(this.txtYIndex);
            this.ultraPanel2.ClientArea.Controls.Add(this.txtEDS);
            this.ultraPanel2.ClientArea.Controls.Add(this.lblBin);
            this.ultraPanel2.ClientArea.Controls.Add(this.txtVI);
            this.ultraPanel2.ClientArea.Controls.Add(this.lblVI);
            this.ultraPanel2.ClientArea.Controls.Add(this.label1);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(964, 62);
            this.ultraPanel2.TabIndex = 25;
            // 
            // lblX
            // 
            this.lblX.BackColor = System.Drawing.Color.Transparent;
            this.lblX.ForeColor = System.Drawing.Color.White;
            this.lblX.Location = new System.Drawing.Point(13, 37);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(23, 23);
            this.lblX.TabIndex = 27;
            this.lblX.Text = "X:";
            this.lblX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblY
            // 
            this.lblY.BackColor = System.Drawing.Color.Transparent;
            this.lblY.ForeColor = System.Drawing.Color.White;
            this.lblY.Location = new System.Drawing.Point(94, 37);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(23, 23);
            this.lblY.TabIndex = 25;
            this.lblY.Text = "Y:";
            this.lblY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtXIndex
            // 
            this.txtXIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXIndex.Location = new System.Drawing.Point(37, 38);
            this.txtXIndex.Name = "txtXIndex";
            this.txtXIndex.ReadOnly = true;
            this.txtXIndex.Size = new System.Drawing.Size(48, 20);
            this.txtXIndex.TabIndex = 20;
            // 
            // txtYIndex
            // 
            this.txtYIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYIndex.Location = new System.Drawing.Point(117, 38);
            this.txtYIndex.Name = "txtYIndex";
            this.txtYIndex.ReadOnly = true;
            this.txtYIndex.Size = new System.Drawing.Size(48, 20);
            this.txtYIndex.TabIndex = 22;
            // 
            // txtEDS
            // 
            this.txtEDS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEDS.Location = new System.Drawing.Point(209, 38);
            this.txtEDS.Name = "txtEDS";
            this.txtEDS.ReadOnly = true;
            this.txtEDS.Size = new System.Drawing.Size(48, 20);
            this.txtEDS.TabIndex = 21;
            // 
            // lblBin
            // 
            this.lblBin.BackColor = System.Drawing.Color.Transparent;
            this.lblBin.ForeColor = System.Drawing.Color.White;
            this.lblBin.Location = new System.Drawing.Point(177, 36);
            this.lblBin.Name = "lblBin";
            this.lblBin.Size = new System.Drawing.Size(32, 23);
            this.lblBin.TabIndex = 24;
            this.lblBin.Text = "BIN:";
            this.lblBin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtVI
            // 
            this.txtVI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVI.Location = new System.Drawing.Point(297, 38);
            this.txtVI.Name = "txtVI";
            this.txtVI.ReadOnly = true;
            this.txtVI.Size = new System.Drawing.Size(48, 20);
            this.txtVI.TabIndex = 23;
            // 
            // lblVI
            // 
            this.lblVI.BackColor = System.Drawing.Color.Transparent;
            this.lblVI.ForeColor = System.Drawing.Color.White;
            this.lblVI.Location = new System.Drawing.Point(273, 36);
            this.lblVI.Name = "lblVI";
            this.lblVI.Size = new System.Drawing.Size(24, 23);
            this.lblVI.TabIndex = 26;
            this.lblVI.Text = "VI:";
            this.lblVI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(23, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 36);
            this.label1.TabIndex = 19;
            this.label1.Text = "Visual Inspection";
            // 
            // m_ewMap
            // 
            this.m_ewMap.AllowDrop = true;
            this.m_ewMap.AngleOffSet = 0;
            this.m_ewMap.AutoFocus = true;
            this.m_ewMap.AutoScroll = true;
            this.m_ewMap.CenterMark = false;
            this.m_ewMap.CurrentFailNumber = 0;
            this.m_ewMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.m_ewMap.DataSource = null;
            this.m_ewMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.m_ewMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.m_ewMap.DieMaxX = 0;
            this.m_ewMap.DieMaxY = 0;
            this.m_ewMap.DieMinX = 0;
            this.m_ewMap.DieMinY = 0;
            this.m_ewMap.DieSizeX = 0.01D;
            this.m_ewMap.DieSizeY = 0.01D;
            this.m_ewMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.m_ewMap.DisplayValue = "BIN";
            this.m_ewMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ewMap.DrawFirstDie = true;
            this.m_ewMap.DrawMarkDie = false;
            this.m_ewMap.DrawOriginDie = true;
            this.m_ewMap.DrawSkipDie = true;
            this.m_ewMap.EdgeColor = System.Drawing.Color.LightGray;
            this.m_ewMap.EdgeSize = 1D;
            this.m_ewMap.EditMenuVisible = true;
            this.m_ewMap.EditMethod = DACrux.Map.EditMode.KILL;
            this.m_ewMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.m_ewMap.FirstDieX = 0;
            this.m_ewMap.FirstDieY = 0;
            this.m_ewMap.ForeColor = System.Drawing.Color.Red;
            this.m_ewMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.m_ewMap.GradationInterval = 5;
            this.m_ewMap.GradationMaxValue = double.NaN;
            this.m_ewMap.GradationMinValue = double.NaN;
            this.m_ewMap.ImeMode = System.Windows.Forms.ImeMode.On;
            this.m_ewMap.IsVIEdit = true;
            this.m_ewMap.Location = new System.Drawing.Point(258, 62);
            this.m_ewMap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_ewMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.m_ewMap.Name = "m_ewMap";
            this.m_ewMap.NotchAngle = 0;
            this.m_ewMap.NotchType = DACrux.Base.Notch.Notch;
            this.m_ewMap.OriginDieBorder = System.Drawing.Color.Red;
            this.m_ewMap.OriginIndexX = 0;
            this.m_ewMap.OriginIndexY = 0;
            this.m_ewMap.OriginX = 0D;
            this.m_ewMap.OriginY = 0D;
            this.m_ewMap.ParametricColumn = "PCMVALUE";
            this.m_ewMap.PickupDieAlpha = 96;
            this.m_ewMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.m_ewMap.PopupMenu = true;
            this.m_ewMap.ReferenceDieSetting = 0;
            this.m_ewMap.ScaleMark = false;
            this.m_ewMap.SelecetedBin = "ALL";
            this.m_ewMap.SelectedVI = "ALL";
            this.m_ewMap.Size = new System.Drawing.Size(706, 428);
            this.m_ewMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_ewMap.TabIndex = 26;
            this.m_ewMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.m_ewMap.TransParent = 255;
            this.m_ewMap.ViewAngle = 0;
            this.m_ewMap.VIMember = "VI";
            this.m_ewMap.VisibleDieBorder = true;
            this.m_ewMap.VisibleDieValue = false;
            this.m_ewMap.VisibleFocusDie = false;
            this.m_ewMap.VisibleInfomation = true;
            this.m_ewMap.VisibleOffDie = false;
            this.m_ewMap.VisibleStringBin = false;
            this.m_ewMap.VisibleVIFail = true;
            this.m_ewMap.VisibleXY = false;
            this.m_ewMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.m_ewMap.WaferColor = System.Drawing.Color.Gray;
            this.m_ewMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.m_ewMap.WaferMargin = 0.95D;
            this.m_ewMap.WaferSize = 200000D;
            this.m_ewMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
            this.m_ewMap.OnChangeDieProperty += new DACrux.Map.ChangeDieProperty(this.m_ewMap_OnChangeDieProperty);
            this.m_ewMap.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.m_ewMap_OnChangeCurrentDie);
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "fpSpread1";
            this.fpSpread1.BackColor = System.Drawing.SystemColors.ControlText;
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Left;
            this.fpSpread1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpSpread1.Location = new System.Drawing.Point(0, 62);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(253, 428);
            this.fpSpread1.TabIndex = 41;
            this.fpSpread1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpread1_Sheet1.ColumnCount = 4;
            fpSpread1_Sheet1.RowCount = 0;
            fpSpread1_Sheet1.RowHeader.ColumnCount = 0;
            this.fpSpread1_Sheet1.ActiveColumnIndex = -1;
            this.fpSpread1_Sheet1.ActiveRowIndex = -1;
            this.fpSpread1_Sheet1.AllowNoteEdit = false;
            this.fpSpread1_Sheet1.AutoCalculation = false;
            this.fpSpread1_Sheet1.AutoGenerateColumns = false;
            this.fpSpread1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "Key";
            this.fpSpread1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "Cat";
            this.fpSpread1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "Count";
            this.fpSpread1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "Name";
            this.fpSpread1_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.White;
            imageCellType3.Style = FarPoint.Win.RenderStyle.Normal;
            imageCellType3.TransparencyColor = System.Drawing.Color.Empty;
            imageCellType3.TransparencyTolerance = 0;
            this.fpSpread1_Sheet1.Columns.Get(0).CellType = imageCellType3;
            this.fpSpread1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpSpread1_Sheet1.Columns.Get(0).Label = "Key";
            this.fpSpread1_Sheet1.Columns.Get(0).Width = 38F;
            this.fpSpread1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpSpread1_Sheet1.Columns.Get(1).Label = "Cat";
            this.fpSpread1_Sheet1.Columns.Get(1).Width = 36F;
            this.fpSpread1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpSpread1_Sheet1.Columns.Get(2).Label = "Count";
            this.fpSpread1_Sheet1.Columns.Get(2).Width = 50F;
            this.fpSpread1_Sheet1.Columns.Get(3).Label = "Name";
            this.fpSpread1_Sheet1.Columns.Get(3).Width = 102F;
            this.fpSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.fpSpread1_Sheet1.RowHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
            this.fpSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            imageCellType4.Style = FarPoint.Win.RenderStyle.Normal;
            imageCellType4.TransparencyColor = System.Drawing.Color.Empty;
            imageCellType4.TransparencyTolerance = 0;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.CellType = imageCellType4;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.Locked = true;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.Renderer = imageCellType4;
            this.fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpSpread1_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(253, 62);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 428);
            this.splitter1.TabIndex = 42;
            this.splitter1.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.butClose);
            this.pnlBottom.Controls.Add(this.butReset);
            this.pnlBottom.Controls.Add(this.butSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 490);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(964, 35);
            this.pnlBottom.TabIndex = 43;
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.Location = new System.Drawing.Point(886, 7);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(75, 23);
            this.butClose.TabIndex = 0;
            this.butClose.Text = "Close";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // butReset
            // 
            this.butReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butReset.Location = new System.Drawing.Point(724, 7);
            this.butReset.Name = "butReset";
            this.butReset.Size = new System.Drawing.Size(75, 23);
            this.butReset.TabIndex = 0;
            this.butReset.Text = "Reset";
            this.butReset.UseVisualStyleBackColor = true;
            this.butReset.Click += new System.EventHandler(this.butReset_Click);
            // 
            // butSave
            // 
            this.butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butSave.Location = new System.Drawing.Point(805, 7);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(75, 23);
            this.butSave.TabIndex = 0;
            this.butSave.Text = "Save";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Erase.png");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            this.imageList.Images.SetKeyName(8, "");
            this.imageList.Images.SetKeyName(9, "");
            this.imageList.Images.SetKeyName(10, "");
            this.imageList.Images.SetKeyName(11, "");
            this.imageList.Images.SetKeyName(12, "");
            this.imageList.Images.SetKeyName(13, "");
            this.imageList.Images.SetKeyName(14, "");
            this.imageList.Images.SetKeyName(15, "");
            this.imageList.Images.SetKeyName(16, "");
            this.imageList.Images.SetKeyName(17, "");
            this.imageList.Images.SetKeyName(18, "");
            this.imageList.Images.SetKeyName(19, "");
            this.imageList.Images.SetKeyName(20, "");
            // 
            // frmVisualInspection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 525);
            this.Controls.Add(this.m_ewMap);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.fpSpread1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.ultraPanel2);
            this.Name = "frmVisualInspection";
            this.Text = "Visual Inspection";
            this.Load += new System.EventHandler(this.frmVisualInspection_Load);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ClientArea.PerformLayout();
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.TextBox txtXIndex;
        private System.Windows.Forms.TextBox txtYIndex;
        private System.Windows.Forms.TextBox txtEDS;
        private System.Windows.Forms.Label lblBin;
        private System.Windows.Forms.TextBox txtVI;
        private System.Windows.Forms.Label lblVI;
        private Map.EditWaferMap m_ewMap;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Button butReset;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.ImageList imageList;
    }
}