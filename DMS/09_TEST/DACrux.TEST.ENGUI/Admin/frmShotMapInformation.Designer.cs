namespace DACrux.TEST.ENGUI
{
    partial class frmShotMapInformation
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
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem1 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem2 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem3 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem4 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem5 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem6 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem7 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem8 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem9 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem10 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem11 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem12 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem13 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem14 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem15 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem16 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem17 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem18 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem19 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem20 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem21 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem22 = new DACrux.Framework.PropertyGrid.PropertyItem();
            this.dlbDeviceList = new DACrux.Framework.Controls.DUCListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.map = new DACrux.Map.WaferMap();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.txtYIndex = new System.Windows.Forms.TextBox();
            this.lbY = new System.Windows.Forms.Label();
            this.txtXIndex = new System.Windows.Forms.TextBox();
            this.lbX = new System.Windows.Forms.Label();
            this.grid = new DACrux.Framework.PropertyGrid.DucPropertyGrid();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraSplitter2 = new Infragistics.Win.Misc.UltraSplitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkScopePoint = new System.Windows.Forms.CheckBox();
            this.chkCenterLine = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlbDeviceList
            // 
            this.dlbDeviceList.AutoScroll = true;
            this.dlbDeviceList.DataSource = null;
            this.dlbDeviceList.DisplayMember = "";
            this.dlbDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dlbDeviceList.Location = new System.Drawing.Point(0, 0);
            this.dlbDeviceList.Name = "dlbDeviceList";
            this.dlbDeviceList.SearchText = "";
            this.dlbDeviceList.SearchTitle = "Device";
            this.dlbDeviceList.SelectedIndex = -1;
            this.dlbDeviceList.SelectedItem = null;
            this.dlbDeviceList.SelectedValue = null;
            this.dlbDeviceList.Size = new System.Drawing.Size(160, 459);
            this.dlbDeviceList.TabIndex = 30;
            this.dlbDeviceList.ValueMember = "";
            this.dlbDeviceList.OnSelectedIndexChanged += new System.EventHandler(this.dlbDeviceList_OnSelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.map);
            this.panel1.Controls.Add(this.pnlInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(166, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 511);
            this.panel1.TabIndex = 31;
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.BackColor = System.Drawing.SystemColors.Control;
            this.map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DieBorderColor = System.Drawing.Color.LightGray;
            this.map.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.map.DieMaxX = 0;
            this.map.DieMaxY = 0;
            this.map.DieMinX = 0;
            this.map.DieMinY = 0;
            this.map.DieSizeX = 0.01D;
            this.map.DieSizeY = 0.01D;
            this.map.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.map.DisplayValue = "BIN";
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.DrawFirstDie = true;
            this.map.DrawMarkDie = false;
            this.map.DrawOriginDie = true;
            this.map.DrawSkipDie = true;
            this.map.EdgeColor = System.Drawing.Color.LightGray;
            this.map.EdgeSize = 1D;
            this.map.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.map.FirstDieX = 0;
            this.map.FirstDieY = 0;
            this.map.ForeColor = System.Drawing.Color.Red;
            this.map.FromGradationDieColor = System.Drawing.Color.Lime;
            this.map.GradationInterval = 5;
            this.map.GradationMaxValue = double.NaN;
            this.map.GradationMinValue = double.NaN;
            this.map.Location = new System.Drawing.Point(0, 21);
            this.map.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.map.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.map.Name = "map";
            this.map.NotchAngle = 0;
            this.map.NotchType = DACrux.Base.Notch.Notch;
            this.map.OriginDieBorder = System.Drawing.Color.Red;
            this.map.OriginIndexX = 0;
            this.map.OriginIndexY = 0;
            this.map.OriginX = 0D;
            this.map.OriginY = 0D;
            this.map.ParaLimit = false;
            this.map.ParametricColumn = "PCMVALUE";
            this.map.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.map.PickupDieAlpha = 96;
            this.map.PickupedDieColor = System.Drawing.Color.Transparent;
            this.map.PopupMenu = true;
            this.map.ReferenceDieSetting = 0;
            this.map.ScaleMark = false;
            this.map.SelecetedBin = "ALL";
            this.map.SelectedVI = "ALL";
            this.map.ShotLineWidth = 2;
            this.map.Size = new System.Drawing.Size(655, 490);
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 5;
            this.map.ToGradationDieColor = System.Drawing.Color.Red;
            this.map.TransParent = 255;
            this.map.ViewAngle = 0;
            this.map.VIMember = "VIFAIL";
            this.map.VisibleDieBorder = true;
            this.map.VisibleDieValue = false;
            this.map.VisibleFocusDie = false;
            this.map.VisibleInfomation = true;
            this.map.VisibleOffDie = false;
            this.map.VisibleShotAlignPoint = false;
            this.map.VisibleSignDies = false;
            this.map.VisibleStringBin = false;
            this.map.VisibleVIFail = true;
            this.map.VisibleXY = false;
            this.map.WaferBorderColor = System.Drawing.Color.LightGray;
            this.map.WaferColor = System.Drawing.Color.Gray;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.map.WaferID = "";
            this.map.WaferMargin = 0.95D;
            this.map.WaferSize = 200D;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.map.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.m_wMap_OnChangeCurrentDie);
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlInfo.Controls.Add(this.txtYIndex);
            this.pnlInfo.Controls.Add(this.lbY);
            this.pnlInfo.Controls.Add(this.txtXIndex);
            this.pnlInfo.Controls.Add(this.lbX);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(655, 21);
            this.pnlInfo.TabIndex = 5;
            // 
            // txtYIndex
            // 
            this.txtYIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtYIndex.Location = new System.Drawing.Point(85, 0);
            this.txtYIndex.Name = "txtYIndex";
            this.txtYIndex.ReadOnly = true;
            this.txtYIndex.Size = new System.Drawing.Size(61, 21);
            this.txtYIndex.TabIndex = 0;
            // 
            // lbY
            // 
            this.lbY.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbY.ForeColor = System.Drawing.Color.White;
            this.lbY.Location = new System.Drawing.Point(70, 0);
            this.lbY.Name = "lbY";
            this.lbY.Size = new System.Drawing.Size(15, 21);
            this.lbY.TabIndex = 11;
            this.lbY.Text = "Y";
            this.lbY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtXIndex
            // 
            this.txtXIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtXIndex.Location = new System.Drawing.Point(15, 0);
            this.txtXIndex.Name = "txtXIndex";
            this.txtXIndex.ReadOnly = true;
            this.txtXIndex.Size = new System.Drawing.Size(55, 21);
            this.txtXIndex.TabIndex = 0;
            // 
            // lbX
            // 
            this.lbX.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbX.ForeColor = System.Drawing.Color.White;
            this.lbX.Location = new System.Drawing.Point(0, 0);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(15, 21);
            this.lbX.TabIndex = 12;
            this.lbX.Text = "X";
            this.lbX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Right;
            this.grid.Location = new System.Drawing.Point(827, 0);
            this.grid.Name = "grid";
            propertyItem1.ColumnName = "FACTORY";
            propertyItem1.IsRequiredField = true;
            propertyItem2.ColumnName = "MAPID";
            propertyItem2.IsRequiredField = true;
            propertyItem3.ColumnName = "WAFER_SIZE";
            propertyItem3.DefaultVauleAtInsertMode = "200000";
            propertyItem3.DisplayName = "Wafer Size (um)";
            propertyItem3.IsRequiredField = true;
            propertyItem4.ColumnName = "CHIP_SIZE_X";
            propertyItem4.DisplayName = "Chip Size X (um)";
            propertyItem4.IsRequiredField = true;
            propertyItem5.ColumnName = "CHIP_SIZE_Y";
            propertyItem5.DisplayName = "Chip Size Y (um)";
            propertyItem5.IsRequiredField = true;
            propertyItem6.ColumnName = "ORIGIN_MICRO_X";
            propertyItem6.DefaultVauleAtInsertMode = "0";
            propertyItem6.Description = "Wafer Center와 Center Die 간 X 거리(um)";
            propertyItem6.DisplayName = "Center Distance X (um)";
            propertyItem6.IsRequiredField = true;
            propertyItem7.ColumnName = "ORIGIN_MICRO_Y";
            propertyItem7.DefaultVauleAtInsertMode = "0";
            propertyItem7.Description = "Wafer Center와 Center Die 간 Y 거리(um)";
            propertyItem7.DisplayName = "Center Distance Y (um)";
            propertyItem7.IsRequiredField = true;
            propertyItem8.ColumnName = "EDGE_SIZE";
            propertyItem8.DefaultVauleAtInsertMode = "0";
            propertyItem8.DisplayName = "Edge Size (mm)";
            propertyItem8.IsRequiredField = true;
            propertyItem9.ColumnName = "ORIGIN_INDEX_X";
            propertyItem9.DisplayName = "Center Die Index X";
            propertyItem9.IsReadOnly = true;
            propertyItem9.IsReadOnlyAtInsertMode = true;
            propertyItem9.IsRequiredField = true;
            propertyItem10.ColumnName = "ORIGIN_INDEX_Y";
            propertyItem10.DisplayName = "Center Die Index Y";
            propertyItem10.IsReadOnly = true;
            propertyItem10.IsReadOnlyAtInsertMode = true;
            propertyItem10.IsRequiredField = true;
            propertyItem11.ColumnName = "DIE_INDEX_MIN_X";
            propertyItem11.DefaultVauleAtInsertMode = "1";
            propertyItem11.DisplayName = "Die Index X Min";
            propertyItem11.IsReadOnly = true;
            propertyItem11.IsReadOnlyAtInsertMode = true;
            propertyItem12.ColumnName = "DIE_INDEX_MIN_Y";
            propertyItem12.DefaultVauleAtInsertMode = "1";
            propertyItem12.DisplayName = "Die Index Y Min";
            propertyItem12.IsReadOnly = true;
            propertyItem12.IsReadOnlyAtInsertMode = true;
            propertyItem13.ColumnName = "DIE_INDEX_MAX_X";
            propertyItem13.DisplayName = "Die Index X Max";
            propertyItem13.IsReadOnly = true;
            propertyItem13.IsReadOnlyAtInsertMode = true;
            propertyItem14.ColumnName = "DIE_INDEX_MAX_Y";
            propertyItem14.DisplayName = "Die Index Y Max";
            propertyItem14.IsReadOnly = true;
            propertyItem14.IsReadOnlyAtInsertMode = true;
            propertyItem15.ColumnName = "NETDIE";
            propertyItem15.DisplayName = "Die Count";
            propertyItem15.IsReadOnly = true;
            propertyItem15.IsReadOnlyAtInsertMode = true;
            propertyItem16.ColumnName = "NETSHOT";
            propertyItem16.DisplayName = "Shot Count";
            propertyItem16.IsReadOnly = true;
            propertyItem16.IsReadOnlyAtInsertMode = true;
            propertyItem17.ColumnName = "ST_XCNT";
            propertyItem17.DisplayName = "Shot Array X";
            propertyItem17.IsReadOnly = true;
            propertyItem17.IsReadOnlyAtInsertMode = true;
            propertyItem18.ColumnName = "ST_YCNT";
            propertyItem18.DisplayName = "Shot Array Y";
            propertyItem18.IsReadOnly = true;
            propertyItem18.IsReadOnlyAtInsertMode = true;
            propertyItem19.ColumnName = "ST_START_X";
            propertyItem19.Description = "";
            propertyItem19.DisplayName = "Shot Start X";
            propertyItem19.IsReadOnly = true;
            propertyItem19.IsReadOnlyAtInsertMode = true;
            propertyItem20.ColumnName = "ST_START_Y";
            propertyItem20.Description = "";
            propertyItem20.DisplayName = "Shot Start Y";
            propertyItem20.IsReadOnly = true;
            propertyItem20.IsReadOnlyAtInsertMode = true;
            propertyItem21.ColumnName = "DELETE_FLAG";
            propertyItem21.DefaultVauleAtInsertMode = "Y";
            propertyItem21.DisplayName = "사용자 등록 여부";
            propertyItem21.IsReadOnly = true;
            propertyItem21.IsReadOnlyAtInsertMode = true;
            propertyItem22.ColumnName = "INSERT_TIME";
            propertyItem22.DisplayName = "생성 시간";
            this.grid.PropertyList.Add(propertyItem1);
            this.grid.PropertyList.Add(propertyItem2);
            this.grid.PropertyList.Add(propertyItem3);
            this.grid.PropertyList.Add(propertyItem4);
            this.grid.PropertyList.Add(propertyItem5);
            this.grid.PropertyList.Add(propertyItem6);
            this.grid.PropertyList.Add(propertyItem7);
            this.grid.PropertyList.Add(propertyItem8);
            this.grid.PropertyList.Add(propertyItem9);
            this.grid.PropertyList.Add(propertyItem10);
            this.grid.PropertyList.Add(propertyItem11);
            this.grid.PropertyList.Add(propertyItem12);
            this.grid.PropertyList.Add(propertyItem13);
            this.grid.PropertyList.Add(propertyItem14);
            this.grid.PropertyList.Add(propertyItem15);
            this.grid.PropertyList.Add(propertyItem16);
            this.grid.PropertyList.Add(propertyItem17);
            this.grid.PropertyList.Add(propertyItem18);
            this.grid.PropertyList.Add(propertyItem19);
            this.grid.PropertyList.Add(propertyItem20);
            this.grid.PropertyList.Add(propertyItem21);
            this.grid.PropertyList.Add(propertyItem22);
            this.grid.Size = new System.Drawing.Size(271, 511);
            this.grid.TabIndex = 32;
            this.grid.CommandButtonClick += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.gridProperty_CommandButtonClick);
            this.grid.CommandComplete += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.gridProperty_CommandComplete);
            this.grid.ValueChanged += new DACrux.Framework.PropertyGrid.DucPropertyGrid.ValueChangedEventHandler(this.grid_ValueChanged);
            this.grid.PropertyDataChanged += new System.EventHandler(this.gridProperty_PropertyDataChanged);
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter1.Location = new System.Drawing.Point(160, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 0;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 511);
            this.ultraSplitter1.TabIndex = 33;
            // 
            // ultraSplitter2
            // 
            this.ultraSplitter2.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter2.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraSplitter2.Location = new System.Drawing.Point(821, 0);
            this.ultraSplitter2.Name = "ultraSplitter2";
            this.ultraSplitter2.RestoreExtent = 271;
            this.ultraSplitter2.Size = new System.Drawing.Size(6, 511);
            this.ultraSplitter2.TabIndex = 34;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dlbDeviceList);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(160, 511);
            this.panel2.TabIndex = 35;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkScopePoint);
            this.panel3.Controls.Add(this.chkCenterLine);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 459);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(160, 52);
            this.panel3.TabIndex = 0;
            // 
            // chkScopePoint
            // 
            this.chkScopePoint.AutoSize = true;
            this.chkScopePoint.Location = new System.Drawing.Point(12, 28);
            this.chkScopePoint.Name = "chkScopePoint";
            this.chkScopePoint.Size = new System.Drawing.Size(128, 16);
            this.chkScopePoint.TabIndex = 0;
            this.chkScopePoint.Text = "Show Scope Point";
            this.chkScopePoint.UseVisualStyleBackColor = true;
            this.chkScopePoint.CheckedChanged += new System.EventHandler(this.chkScopePoint_CheckedChanged);
            // 
            // chkCenterLine
            // 
            this.chkCenterLine.AutoSize = true;
            this.chkCenterLine.Location = new System.Drawing.Point(12, 6);
            this.chkCenterLine.Name = "chkCenterLine";
            this.chkCenterLine.Size = new System.Drawing.Size(125, 16);
            this.chkCenterLine.TabIndex = 0;
            this.chkCenterLine.Text = "Show Center Line";
            this.chkCenterLine.UseVisualStyleBackColor = true;
            this.chkCenterLine.CheckedChanged += new System.EventHandler(this.chkCenterLine_CheckedChanged);
            // 
            // frmShotMapInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 511);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ultraSplitter2);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel2);
            this.Name = "frmShotMapInformation";
            this.Text = "Shot Map Define";
            this.Load += new System.EventHandler(this.frmShotMapInformation_Load);
            this.panel1.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.Controls.DUCListBox dlbDeviceList;
        private System.Windows.Forms.Panel panel1;
        private Map.WaferMap map;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.TextBox txtYIndex;
        private System.Windows.Forms.Label lbY;
        private System.Windows.Forms.TextBox txtXIndex;
        private System.Windows.Forms.Label lbX;
        private Framework.PropertyGrid.DucPropertyGrid grid;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chkScopePoint;
        private System.Windows.Forms.CheckBox chkCenterLine;

    }
}