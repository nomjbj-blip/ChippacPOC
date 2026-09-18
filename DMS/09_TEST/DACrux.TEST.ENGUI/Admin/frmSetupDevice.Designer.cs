namespace DACrux.TEST.ENGUI
{
    partial class frmSetupDevice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetupDevice));
            this.panel1 = new System.Windows.Forms.Panel();
            this.butDelete = new System.Windows.Forms.Button();
            this.butUpdate = new System.Windows.Forms.Button();
            this.butCreate = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.txtWaferSize = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDieSizeX = new System.Windows.Forms.TextBox();
            this.txtDieSizeY = new System.Windows.Forms.TextBox();
            this.txtOriginX = new System.Windows.Forms.TextBox();
            this.txtEdgeSize = new System.Windows.Forms.TextBox();
            this.txtOriginY = new System.Windows.Forms.TextBox();
            this.txtNotchAngle = new System.Windows.Forms.TextBox();
            this.txtNetDie = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDiesX = new System.Windows.Forms.TextBox();
            this.txtDiesY = new System.Windows.Forms.TextBox();
            this.txtOrgingIndexY = new System.Windows.Forms.TextBox();
            this.txtOrgingIndexX = new System.Windows.Forms.TextBox();
            this.txtNotchType = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ducDeviceName = new DACrux.Framework.Controls.DUCItemSelector();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFacility = new System.Windows.Forms.TextBox();
            this.cmbMapIDs = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtCustDevice = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.chkCenterMark = new System.Windows.Forms.CheckBox();
            this.chkScale = new System.Windows.Forms.CheckBox();
            this.m_wMap = new DACrux.Map.WaferMap();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.butDelete);
            this.panel1.Controls.Add(this.butUpdate);
            this.panel1.Controls.Add(this.butCreate);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(600, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 502);
            this.panel1.TabIndex = 37;
            // 
            // butDelete
            // 
            this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butDelete.Location = new System.Drawing.Point(179, 469);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(75, 28);
            this.butDelete.TabIndex = 57;
            this.butDelete.Text = "Delete";
            this.butDelete.UseVisualStyleBackColor = true;
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // butUpdate
            // 
            this.butUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butUpdate.Location = new System.Drawing.Point(104, 469);
            this.butUpdate.Name = "butUpdate";
            this.butUpdate.Size = new System.Drawing.Size(75, 28);
            this.butUpdate.TabIndex = 57;
            this.butUpdate.Text = "Update";
            this.butUpdate.UseVisualStyleBackColor = true;
            this.butUpdate.Click += new System.EventHandler(this.butUpdate_Click);
            // 
            // butCreate
            // 
            this.butCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCreate.Location = new System.Drawing.Point(29, 469);
            this.butCreate.Name = "butCreate";
            this.butCreate.Size = new System.Drawing.Size(75, 28);
            this.butCreate.TabIndex = 57;
            this.butCreate.Text = "Create";
            this.butCreate.UseVisualStyleBackColor = true;
            this.butCreate.Click += new System.EventHandler(this.butCreate_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.label23);
            this.panel4.Controls.Add(this.label24);
            this.panel4.Controls.Add(this.label18);
            this.panel4.Controls.Add(this.label19);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Controls.Add(this.label26);
            this.panel4.Controls.Add(this.txtWaferSize);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.txtDieSizeX);
            this.panel4.Controls.Add(this.txtDieSizeY);
            this.panel4.Controls.Add(this.txtOriginX);
            this.panel4.Controls.Add(this.txtEdgeSize);
            this.panel4.Controls.Add(this.txtOriginY);
            this.panel4.Controls.Add(this.txtNotchAngle);
            this.panel4.Controls.Add(this.txtNetDie);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.txtDiesX);
            this.panel4.Controls.Add(this.txtDiesY);
            this.panel4.Controls.Add(this.txtOrgingIndexY);
            this.panel4.Controls.Add(this.txtOrgingIndexX);
            this.panel4.Controls.Add(this.txtNotchType);
            this.panel4.Location = new System.Drawing.Point(8, 160);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(272, 216);
            this.panel4.TabIndex = 38;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Image = ((System.Drawing.Image)(resources.GetObject("label12.Image")));
            this.label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.Location = new System.Drawing.Point(10, 192);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(98, 20);
            this.label12.TabIndex = 31;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(240, 24);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(24, 20);
            this.label23.TabIndex = 30;
            this.label23.Text = "mm";
            this.label23.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(176, 48);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(24, 20);
            this.label24.TabIndex = 29;
            this.label24.Text = "mm";
            this.label24.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.White;
            this.label18.Image = ((System.Drawing.Image)(resources.GetObject("label18.Image")));
            this.label18.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label18.Location = new System.Drawing.Point(10, 144);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(98, 20);
            this.label18.TabIndex = 28;
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.White;
            this.label19.Image = ((System.Drawing.Image)(resources.GetObject("label19.Image")));
            this.label19.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label19.Location = new System.Drawing.Point(10, 168);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(98, 20);
            this.label19.TabIndex = 27;
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.Image = ((System.Drawing.Image)(resources.GetObject("label16.Image")));
            this.label16.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label16.Location = new System.Drawing.Point(8, 96);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 20);
            this.label16.TabIndex = 25;
            // 
            // label17
            // 
            this.label17.Image = ((System.Drawing.Image)(resources.GetObject("label17.Image")));
            this.label17.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label17.Location = new System.Drawing.Point(8, 120);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(100, 20);
            this.label17.TabIndex = 26;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.White;
            this.label14.Image = ((System.Drawing.Image)(resources.GetObject("label14.Image")));
            this.label14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label14.Location = new System.Drawing.Point(10, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 20);
            this.label14.TabIndex = 23;
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.White;
            this.label15.Image = ((System.Drawing.Image)(resources.GetObject("label15.Image")));
            this.label15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label15.Location = new System.Drawing.Point(10, 72);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 20);
            this.label15.TabIndex = 24;
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.Image = ((System.Drawing.Image)(resources.GetObject("label26.Image")));
            this.label26.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label26.Location = new System.Drawing.Point(8, 24);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(100, 20);
            this.label26.TabIndex = 22;
            // 
            // txtWaferSize
            // 
            this.txtWaferSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWaferSize.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtWaferSize.Location = new System.Drawing.Point(112, 24);
            this.txtWaferSize.Name = "txtWaferSize";
            this.txtWaferSize.ReadOnly = true;
            this.txtWaferSize.Size = new System.Drawing.Size(128, 21);
            this.txtWaferSize.TabIndex = 21;
            this.txtWaferSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.LightSlateGray;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(272, 20);
            this.label7.TabIndex = 20;
            this.label7.Text = "    Map Infomation";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDieSizeX
            // 
            this.txtDieSizeX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDieSizeX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDieSizeX.Location = new System.Drawing.Point(112, 48);
            this.txtDieSizeX.Name = "txtDieSizeX";
            this.txtDieSizeX.ReadOnly = true;
            this.txtDieSizeX.Size = new System.Drawing.Size(64, 21);
            this.txtDieSizeX.TabIndex = 21;
            this.txtDieSizeX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDieSizeY
            // 
            this.txtDieSizeY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDieSizeY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDieSizeY.Location = new System.Drawing.Point(112, 72);
            this.txtDieSizeY.Name = "txtDieSizeY";
            this.txtDieSizeY.ReadOnly = true;
            this.txtDieSizeY.Size = new System.Drawing.Size(64, 21);
            this.txtDieSizeY.TabIndex = 21;
            this.txtDieSizeY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtOriginX
            // 
            this.txtOriginX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOriginX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOriginX.Location = new System.Drawing.Point(112, 96);
            this.txtOriginX.Name = "txtOriginX";
            this.txtOriginX.ReadOnly = true;
            this.txtOriginX.Size = new System.Drawing.Size(64, 21);
            this.txtOriginX.TabIndex = 21;
            this.txtOriginX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtEdgeSize
            // 
            this.txtEdgeSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEdgeSize.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtEdgeSize.Location = new System.Drawing.Point(112, 144);
            this.txtEdgeSize.Name = "txtEdgeSize";
            this.txtEdgeSize.ReadOnly = true;
            this.txtEdgeSize.Size = new System.Drawing.Size(128, 21);
            this.txtEdgeSize.TabIndex = 21;
            this.txtEdgeSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtOriginY
            // 
            this.txtOriginY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOriginY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOriginY.Location = new System.Drawing.Point(112, 120);
            this.txtOriginY.Name = "txtOriginY";
            this.txtOriginY.ReadOnly = true;
            this.txtOriginY.Size = new System.Drawing.Size(64, 21);
            this.txtOriginY.TabIndex = 21;
            this.txtOriginY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtNotchAngle
            // 
            this.txtNotchAngle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotchAngle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtNotchAngle.Location = new System.Drawing.Point(112, 192);
            this.txtNotchAngle.Name = "txtNotchAngle";
            this.txtNotchAngle.ReadOnly = true;
            this.txtNotchAngle.Size = new System.Drawing.Size(56, 21);
            this.txtNotchAngle.TabIndex = 21;
            this.txtNotchAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtNetDie
            // 
            this.txtNetDie.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNetDie.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtNetDie.Location = new System.Drawing.Point(112, 168);
            this.txtNetDie.Name = "txtNetDie";
            this.txtNetDie.ReadOnly = true;
            this.txtNetDie.Size = new System.Drawing.Size(128, 21);
            this.txtNetDie.TabIndex = 21;
            this.txtNetDie.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(176, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 20);
            this.label5.TabIndex = 29;
            this.label5.Text = "mm";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(176, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 20);
            this.label9.TabIndex = 30;
            this.label9.Text = "mm";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(176, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 20);
            this.label10.TabIndex = 30;
            this.label10.Text = "mm";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(240, 144);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 20);
            this.label11.TabIndex = 29;
            this.label11.Text = "mm";
            this.label11.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtDiesX
            // 
            this.txtDiesX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDiesX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDiesX.Location = new System.Drawing.Point(200, 48);
            this.txtDiesX.Name = "txtDiesX";
            this.txtDiesX.ReadOnly = true;
            this.txtDiesX.Size = new System.Drawing.Size(40, 21);
            this.txtDiesX.TabIndex = 21;
            this.txtDiesX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDiesY
            // 
            this.txtDiesY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDiesY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDiesY.Location = new System.Drawing.Point(200, 72);
            this.txtDiesY.Name = "txtDiesY";
            this.txtDiesY.ReadOnly = true;
            this.txtDiesY.Size = new System.Drawing.Size(40, 21);
            this.txtDiesY.TabIndex = 21;
            this.txtDiesY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtOrgingIndexY
            // 
            this.txtOrgingIndexY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrgingIndexY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOrgingIndexY.Location = new System.Drawing.Point(200, 120);
            this.txtOrgingIndexY.Name = "txtOrgingIndexY";
            this.txtOrgingIndexY.ReadOnly = true;
            this.txtOrgingIndexY.Size = new System.Drawing.Size(40, 21);
            this.txtOrgingIndexY.TabIndex = 21;
            this.txtOrgingIndexY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtOrgingIndexX
            // 
            this.txtOrgingIndexX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrgingIndexX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOrgingIndexX.Location = new System.Drawing.Point(200, 96);
            this.txtOrgingIndexX.Name = "txtOrgingIndexX";
            this.txtOrgingIndexX.ReadOnly = true;
            this.txtOrgingIndexX.Size = new System.Drawing.Size(40, 21);
            this.txtOrgingIndexX.TabIndex = 21;
            this.txtOrgingIndexX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtNotchType
            // 
            this.txtNotchType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotchType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtNotchType.Location = new System.Drawing.Point(176, 192);
            this.txtNotchType.Name = "txtNotchType";
            this.txtNotchType.ReadOnly = true;
            this.txtNotchType.Size = new System.Drawing.Size(64, 21);
            this.txtNotchType.TabIndex = 21;
            this.txtNotchType.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel3.Controls.Add(this.ducDeviceName);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.txtFacility);
            this.panel3.Controls.Add(this.cmbMapIDs);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.cmbCustomer);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.txtCustDevice);
            this.panel3.Location = new System.Drawing.Point(8, 8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 144);
            this.panel3.TabIndex = 37;
            // 
            // ducDeviceName
            // 
            this.ducDeviceName.AutoSize = true;
            this.ducDeviceName.BackColor = System.Drawing.SystemColors.Control;
            this.ducDeviceName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ducDeviceName.BackgroundImage")));
            this.ducDeviceName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.ducDeviceName.DataSource = null;
            this.ducDeviceName.DefaultText = "";
            this.ducDeviceName.DisplayColumn = "";
            this.ducDeviceName.EntryMode = DACrux.Framework.Controls.EntryModeCollection.Normal;
            this.ducDeviceName.Label = "Device";
            this.ducDeviceName.LabelWidth = 103;
            this.ducDeviceName.Location = new System.Drawing.Point(8, 22);
            this.ducDeviceName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ducDeviceName.MultiSelect = false;
            this.ducDeviceName.Name = "ducDeviceName";
            this.ducDeviceName.ReadOnly = false;
            this.ducDeviceName.SearchVisible = true;
            this.ducDeviceName.Size = new System.Drawing.Size(256, 24);
            this.ducDeviceName.TabIndex = 38;
            this.ducDeviceName.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.ducDeviceName.UseAll = true;
            this.ducDeviceName.ValueColumn = "";
            this.ducDeviceName.ValueTextColumn = "";
            this.ducDeviceName.DropDown += new System.EventHandler(this.ducDeviceName_DropDown);
            this.ducDeviceName.OnItemSelected += new DACrux.Framework.Controls.ItemSelected(this.ducDeviceName_OnItemSelected);
            this.ducDeviceName.OnTextChanged += new System.EventHandler(this.ducDeviceName_OnTextChanged);
            // 
            // label3
            // 
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.Location = new System.Drawing.Point(8, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 44;
            // 
            // txtFacility
            // 
            this.txtFacility.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFacility.Location = new System.Drawing.Point(112, 72);
            this.txtFacility.Name = "txtFacility";
            this.txtFacility.Size = new System.Drawing.Size(152, 21);
            this.txtFacility.TabIndex = 45;
            // 
            // cmbMapIDs
            // 
            this.cmbMapIDs.Location = new System.Drawing.Point(112, 120);
            this.cmbMapIDs.Name = "cmbMapIDs";
            this.cmbMapIDs.Size = new System.Drawing.Size(152, 20);
            this.cmbMapIDs.TabIndex = 43;
            this.cmbMapIDs.SelectedIndexChanged += new System.EventHandler(this.cmbMapIDs_SelectedIndexChanged);
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
            this.label6.Size = new System.Drawing.Size(272, 20);
            this.label6.TabIndex = 37;
            this.label6.Text = "    New Wafer Map Creation";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.Location = new System.Drawing.Point(112, 96);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(152, 20);
            this.cmbCustomer.TabIndex = 43;
            // 
            // label2
            // 
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.Location = new System.Drawing.Point(8, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.TabIndex = 48;
            // 
            // label4
            // 
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.Location = new System.Drawing.Point(8, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 20);
            this.label4.TabIndex = 48;
            // 
            // label13
            // 
            this.label13.Image = ((System.Drawing.Image)(resources.GetObject("label13.Image")));
            this.label13.Location = new System.Drawing.Point(8, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 20);
            this.label13.TabIndex = 48;
            // 
            // txtCustDevice
            // 
            this.txtCustDevice.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCustDevice.Location = new System.Drawing.Point(112, 48);
            this.txtCustDevice.Name = "txtCustDevice";
            this.txtCustDevice.Size = new System.Drawing.Size(152, 21);
            this.txtCustDevice.TabIndex = 38;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Menu;
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.chkCenterMark);
            this.panel5.Controls.Add(this.chkScale);
            this.panel5.Location = new System.Drawing.Point(8, 384);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(272, 48);
            this.panel5.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.LightSlateGray;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Image = ((System.Drawing.Image)(resources.GetObject("label8.Image")));
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(272, 20);
            this.label8.TabIndex = 21;
            this.label8.Text = "    Option";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkCenterMark
            // 
            this.chkCenterMark.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkCenterMark.Location = new System.Drawing.Point(8, 24);
            this.chkCenterMark.Name = "chkCenterMark";
            this.chkCenterMark.Size = new System.Drawing.Size(104, 16);
            this.chkCenterMark.TabIndex = 18;
            this.chkCenterMark.Text = "Center Mark";
            this.chkCenterMark.CheckedChanged += new System.EventHandler(this.chkCenterMark_CheckedChanged);
            // 
            // chkScale
            // 
            this.chkScale.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkScale.Location = new System.Drawing.Point(160, 24);
            this.chkScale.Name = "chkScale";
            this.chkScale.Size = new System.Drawing.Size(104, 16);
            this.chkScale.TabIndex = 18;
            this.chkScale.Text = "Scale";
            this.chkScale.CheckedChanged += new System.EventHandler(this.chkScale_CheckedChanged);
            // 
            // m_wMap
            // 
            this.m_wMap.AngleOffSet = 0;
            this.m_wMap.CenterMark = false;
            this.m_wMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.m_wMap.DataSource = null;
            this.m_wMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.m_wMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.m_wMap.DieMaxX = 0;
            this.m_wMap.DieMaxY = 0;
            this.m_wMap.DieMinX = 0;
            this.m_wMap.DieMinY = 0;
            this.m_wMap.DieSizeX = 0.01D;
            this.m_wMap.DieSizeY = 0.01D;
            this.m_wMap.DisplayValue = "BIN";
            this.m_wMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_wMap.DrawFirstDie = true;
            this.m_wMap.DrawMarkDie = false;
            this.m_wMap.DrawOriginDie = true;
            this.m_wMap.DrawSkipDie = true;
            this.m_wMap.EdgeColor = System.Drawing.Color.LightGray;
            this.m_wMap.EdgeSize = 1D;
            this.m_wMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.m_wMap.FirstDieX = 0;
            this.m_wMap.FirstDieY = 0;
            this.m_wMap.ForeColor = System.Drawing.Color.Red;
            this.m_wMap.Location = new System.Drawing.Point(0, 0);
            this.m_wMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.m_wMap.Name = "m_wMap";
            this.m_wMap.NotchAngle = 0;
            this.m_wMap.NotchType = DACrux.Base.Notch.Flat;
            this.m_wMap.OriginDieBorder = System.Drawing.Color.Red;
            this.m_wMap.OriginIndexX = 0;
            this.m_wMap.OriginIndexY = 0;
            this.m_wMap.OriginX = 0D;
            this.m_wMap.OriginY = 0D;
            this.m_wMap.PickupDieAlpha = 96;
            this.m_wMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.m_wMap.PopupMenu = true;
            this.m_wMap.ReferenceDieSetting = 0;
            this.m_wMap.ScaleMark = false;
            this.m_wMap.SelecetedBin = "ALL";
            this.m_wMap.Size = new System.Drawing.Size(600, 502);
            this.m_wMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_wMap.TabIndex = 6;
            this.m_wMap.TransParent = 255;
            this.m_wMap.ViewAngle = 0;
            this.m_wMap.VisibleDieBorder = true;
            this.m_wMap.VisibleDieValue = false;
            this.m_wMap.VisibleFocusDie = false;
            this.m_wMap.VisibleInfomation = true;
            this.m_wMap.VisibleOffDie = false;
            this.m_wMap.VisibleStringBin = false;
            this.m_wMap.VisibleVIFail = false;
            this.m_wMap.VisibleXY = false;
            this.m_wMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.m_wMap.WaferColor = System.Drawing.Color.Gainsboro;
            this.m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.m_wMap.WaferMargin = 0.95D;
            this.m_wMap.WaferSize = 200000D;
            this.m_wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
            // 
            // frmSetupDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 502);
            this.Controls.Add(this.m_wMap);
            this.Controls.Add(this.panel1);
            this.Name = "frmSetupDevice";
            this.Text = "Device Define";
            this.Load += new System.EventHandler(this.TPUCDevice_Load);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtWaferSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDieSizeX;
        private System.Windows.Forms.TextBox txtDieSizeY;
        private System.Windows.Forms.TextBox txtOriginX;
        private System.Windows.Forms.TextBox txtEdgeSize;
        private System.Windows.Forms.TextBox txtOriginY;
        private System.Windows.Forms.TextBox txtNotchAngle;
        private System.Windows.Forms.TextBox txtNetDie;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDiesX;
        private System.Windows.Forms.TextBox txtDiesY;
        private System.Windows.Forms.TextBox txtOrgingIndexY;
        private System.Windows.Forms.TextBox txtOrgingIndexX;
        private System.Windows.Forms.TextBox txtNotchType;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFacility;
        private System.Windows.Forms.ComboBox cmbMapIDs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtCustDevice;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkCenterMark;
        private System.Windows.Forms.CheckBox chkScale;
        private Map.WaferMap m_wMap;
        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.Button butUpdate;
        private System.Windows.Forms.Button butCreate;
        private Framework.Controls.DUCItemSelector ducDeviceName;

    }
}