namespace DACrux.TEST.Control
{
    partial class TPUCQCInspection
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPUCQCInspection));
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.ImageCellType imageCellType3 = new FarPoint.Win.Spread.CellType.ImageCellType();
            FarPoint.Win.Spread.CellType.ImageCellType imageCellType4 = new FarPoint.Win.Spread.CellType.ImageCellType();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.txtYIndex = new System.Windows.Forms.TextBox();
            this.txtXIndex = new System.Windows.Forms.TextBox();
            this.lblX = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.txtVI = new System.Windows.Forms.TextBox();
            this.txtEDS = new System.Windows.Forms.TextBox();
            this.lblBin = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblVI = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.plnCat = new System.Windows.Forms.Panel();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.m_ewMap = new DACrux.Map.EditWaferMap();
            this.pnlInfo.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.plnCat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.SuspendLayout();
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
            // txtYIndex
            // 
            this.txtYIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYIndex.Location = new System.Drawing.Point(104, 2);
            this.txtYIndex.Name = "txtYIndex";
            this.txtYIndex.ReadOnly = true;
            this.txtYIndex.Size = new System.Drawing.Size(48, 20);
            this.txtYIndex.TabIndex = 0;
            // 
            // txtXIndex
            // 
            this.txtXIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXIndex.Location = new System.Drawing.Point(24, 2);
            this.txtXIndex.Name = "txtXIndex";
            this.txtXIndex.ReadOnly = true;
            this.txtXIndex.Size = new System.Drawing.Size(48, 20);
            this.txtXIndex.TabIndex = 0;
            // 
            // lblX
            // 
            this.lblX.ForeColor = System.Drawing.Color.White;
            this.lblX.Location = new System.Drawing.Point(0, 1);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(23, 23);
            this.lblX.TabIndex = 12;
            this.lblX.Text = "X:";
            this.lblX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblY
            // 
            this.lblY.ForeColor = System.Drawing.Color.White;
            this.lblY.Location = new System.Drawing.Point(81, 1);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(23, 23);
            this.lblY.TabIndex = 11;
            this.lblY.Text = "Y:";
            this.lblY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtVI
            // 
            this.txtVI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVI.Location = new System.Drawing.Point(284, 2);
            this.txtVI.Name = "txtVI";
            this.txtVI.ReadOnly = true;
            this.txtVI.Size = new System.Drawing.Size(48, 20);
            this.txtVI.TabIndex = 0;
            // 
            // txtEDS
            // 
            this.txtEDS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEDS.Location = new System.Drawing.Point(196, 2);
            this.txtEDS.Name = "txtEDS";
            this.txtEDS.ReadOnly = true;
            this.txtEDS.Size = new System.Drawing.Size(48, 20);
            this.txtEDS.TabIndex = 0;
            // 
            // lblBin
            // 
            this.lblBin.ForeColor = System.Drawing.Color.White;
            this.lblBin.Location = new System.Drawing.Point(164, 0);
            this.lblBin.Name = "lblBin";
            this.lblBin.Size = new System.Drawing.Size(32, 23);
            this.lblBin.TabIndex = 11;
            this.lblBin.Text = "BIN:";
            this.lblBin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.DarkOrange;
            this.pnlInfo.Controls.Add(this.lblX);
            this.pnlInfo.Controls.Add(this.lblY);
            this.pnlInfo.Controls.Add(this.txtXIndex);
            this.pnlInfo.Controls.Add(this.txtYIndex);
            this.pnlInfo.Controls.Add(this.txtEDS);
            this.pnlInfo.Controls.Add(this.lblBin);
            this.pnlInfo.Controls.Add(this.txtVI);
            this.pnlInfo.Controls.Add(this.lblVI);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(670, 24);
            this.pnlInfo.TabIndex = 0;
            // 
            // lblVI
            // 
            this.lblVI.ForeColor = System.Drawing.Color.White;
            this.lblVI.Location = new System.Drawing.Point(260, 0);
            this.lblVI.Name = "lblVI";
            this.lblVI.Size = new System.Drawing.Size(24, 23);
            this.lblVI.TabIndex = 11;
            this.lblVI.Text = "VI:";
            this.lblVI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.m_ewMap);
            this.pnlBody.Controls.Add(this.splitter1);
            this.pnlBody.Controls.Add(this.plnCat);
            this.pnlBody.Controls.Add(this.pnlInfo);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 0);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(670, 514);
            this.pnlBody.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(252, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 490);
            this.splitter1.TabIndex = 41;
            this.splitter1.TabStop = false;
            // 
            // plnCat
            // 
            this.plnCat.Controls.Add(this.fpSpread1);
            this.plnCat.Dock = System.Windows.Forms.DockStyle.Left;
            this.plnCat.Location = new System.Drawing.Point(0, 24);
            this.plnCat.Name = "plnCat";
            this.plnCat.Size = new System.Drawing.Size(252, 490);
            this.plnCat.TabIndex = 42;
            // 
            // fpSpread1
            // 
            this.fpSpread1.About = "2.5.2008.2005";
            this.fpSpread1.AccessibleDescription = "fpSpread1";
            this.fpSpread1.BackColor = System.Drawing.SystemColors.ControlText;
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpSpread1.Location = new System.Drawing.Point(0, 0);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(252, 490);
            this.fpSpread1.TabIndex = 40;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.fpSpread1.TextTipAppearance = tipAppearance2;
            this.fpSpread1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            this.fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread1_Sheet1.ColumnCount = 4;
            this.fpSpread1_Sheet1.RowCount = 0;
            this.fpSpread1_Sheet1.RowHeader.ColumnCount = 0;
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
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpSpread1_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            this.fpSpread1.SetActiveViewport(1, 0);
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
            this.m_ewMap.ImeMode = System.Windows.Forms.ImeMode.On;
            this.m_ewMap.IsVIEdit = true;
            this.m_ewMap.Location = new System.Drawing.Point(257, 24);
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
            this.m_ewMap.PickupDieAlpha = 96;
            this.m_ewMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.m_ewMap.PopupMenu = true;
            this.m_ewMap.ReferenceDieSetting = 0;
            this.m_ewMap.ScaleMark = false;
            this.m_ewMap.SelecetedBin = "ALL";
            this.m_ewMap.SelectedVI = "ALL";
            this.m_ewMap.Size = new System.Drawing.Size(413, 490);
            this.m_ewMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_ewMap.TabIndex = 1;
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
            // TPUCQCInspection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBody);
            this.Name = "TPUCQCInspection";
            this.Size = new System.Drawing.Size(670, 514);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.plnCat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TextBox txtYIndex;
        private System.Windows.Forms.TextBox txtXIndex;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.TextBox txtVI;
        private System.Windows.Forms.TextBox txtEDS;
        private System.Windows.Forms.Label lblBin;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblVI;
        private System.Windows.Forms.Panel pnlBody;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel plnCat;
        private Map.EditWaferMap m_ewMap;
    }
}
