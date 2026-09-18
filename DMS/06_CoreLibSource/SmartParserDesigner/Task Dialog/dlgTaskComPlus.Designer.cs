namespace SmartParser.Designer
{
    partial class dlgTaskComPlus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgTaskComPlus));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlParameter = new System.Windows.Forms.Panel();
            this.lvParameter = new System.Windows.Forms.ListView();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.pnlMethod = new System.Windows.Forms.Panel();
            this.lvMethod = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.pnlCom = new System.Windows.Forms.Panel();
            this.lvComponent = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uclTitleParameter = new DACrux.SP.Controls.uclTitle();
            this.uclTitleMethod = new DACrux.SP.Controls.uclTitle();
            this.uclTitleCom = new DACrux.SP.Controls.uclTitle();
            this.cboResultUseType = new DACrux.SP.Controls.EnumComboBox();
            this.uclTitleGeneral = new DACrux.SP.Controls.uclTitle();
            this.panel1.SuspendLayout();
            this.pnlParameter.SuspendLayout();
            this.pnlMethod.SuspendLayout();
            this.pnlCom.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(338, 490);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = DACrux.SP.Common.MultiLang.SelectLang["Save"];
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(419, 490);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = DACrux.SP.Common.MultiLang.SelectLang["Cancel"];
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlParameter);
            this.panel1.Controls.Add(this.uclTitleParameter);
            this.panel1.Controls.Add(this.pnlMethod);
            this.panel1.Controls.Add(this.uclTitleMethod);
            this.panel1.Controls.Add(this.pnlCom);
            this.panel1.Controls.Add(this.uclTitleCom);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panel1.Location = new System.Drawing.Point(0, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 422);
            this.panel1.TabIndex = 21;
            // 
            // pnlParameter
            // 
            this.pnlParameter.BackColor = System.Drawing.Color.White;
            this.pnlParameter.Controls.Add(this.lvParameter);
            this.pnlParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParameter.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pnlParameter.Location = new System.Drawing.Point(0, 326);
            this.pnlParameter.Name = "pnlParameter";
            this.pnlParameter.Size = new System.Drawing.Size(494, 96);
            this.pnlParameter.TabIndex = 22;
            // 
            // lvParameter
            // 
            this.lvParameter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvParameter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8});
            this.lvParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvParameter.FullRowSelect = true;
            this.lvParameter.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvParameter.Location = new System.Drawing.Point(0, 0);
            this.lvParameter.MultiSelect = false;
            this.lvParameter.Name = "lvParameter";
            this.lvParameter.Size = new System.Drawing.Size(494, 96);
            this.lvParameter.TabIndex = 2;
            this.lvParameter.UseCompatibleStateImageBehavior = false;
            this.lvParameter.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = DACrux.SP.Common.MultiLang.SelectLang["Name"];
            this.columnHeader7.Width = 120;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = DACrux.SP.Common.MultiLang.SelectLang["Type"];
            this.columnHeader8.Width = 150;
            // 
            // pnlMethod
            // 
            this.pnlMethod.BackColor = System.Drawing.Color.White;
            this.pnlMethod.Controls.Add(this.lvMethod);
            this.pnlMethod.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMethod.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pnlMethod.Location = new System.Drawing.Point(0, 159);
            this.pnlMethod.Name = "pnlMethod";
            this.pnlMethod.Size = new System.Drawing.Size(494, 147);
            this.pnlMethod.TabIndex = 20;
            // 
            // lvMethod
            // 
            this.lvMethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvMethod.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader6});
            this.lvMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMethod.FullRowSelect = true;
            this.lvMethod.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvMethod.Location = new System.Drawing.Point(0, 0);
            this.lvMethod.MultiSelect = false;
            this.lvMethod.Name = "lvMethod";
            this.lvMethod.Size = new System.Drawing.Size(494, 147);
            this.lvMethod.TabIndex = 1;
            this.lvMethod.UseCompatibleStateImageBehavior = false;
            this.lvMethod.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = DACrux.SP.Common.MultiLang.SelectLang["Name"];
            this.columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = DACrux.SP.Common.MultiLang.SelectLang["Parameters"];
            this.columnHeader2.Width = 70;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = DACrux.SP.Common.MultiLang.SelectLang["Return Type"];
            this.columnHeader6.Width = 130;
            // 
            // pnlCom
            // 
            this.pnlCom.BackColor = System.Drawing.Color.White;
            this.pnlCom.Controls.Add(this.lvComponent);
            this.pnlCom.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCom.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pnlCom.Location = new System.Drawing.Point(0, 20);
            this.pnlCom.Name = "pnlCom";
            this.pnlCom.Size = new System.Drawing.Size(494, 119);
            this.pnlCom.TabIndex = 17;
            // 
            // lvComponent
            // 
            this.lvComponent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvComponent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvComponent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvComponent.FullRowSelect = true;
            this.lvComponent.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvComponent.Location = new System.Drawing.Point(0, 0);
            this.lvComponent.MultiSelect = false;
            this.lvComponent.Name = "lvComponent";
            this.lvComponent.Size = new System.Drawing.Size(494, 119);
            this.lvComponent.TabIndex = 2;
            this.lvComponent.UseCompatibleStateImageBehavior = false;
            this.lvComponent.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = DACrux.SP.Common.MultiLang.SelectLang["Component"];
            this.columnHeader3.Width = 300;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = DACrux.SP.Common.MultiLang.SelectLang["Application"];
            this.columnHeader4.Width = 150;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cboResultUseType);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtTaskName);
            this.panel2.Controls.Add(this.uclTitleGeneral);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(494, 63);
            this.panel2.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 19;
            this.label1.Text = DACrux.SP.Common.MultiLang.SelectLang["Task Name"];
            // 
            // txtTaskName
            // 
            this.txtTaskName.Location = new System.Drawing.Point(85, 32);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(138, 22);
            this.txtTaskName.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 21;
            this.label2.Text = DACrux.SP.Common.MultiLang.SelectLang["Result Usage"];
            // 
            // uclTitleParameter
            // 
            this.uclTitleParameter.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleParameter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleParameter.BackgroundImage")));
            this.uclTitleParameter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleParameter.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleParameter.EndColor = System.Drawing.Color.White;
            this.uclTitleParameter.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleParameter.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleParameter.Location = new System.Drawing.Point(0, 306);
            this.uclTitleParameter.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleParameter.Name = "uclTitleParameter";
            this.uclTitleParameter.Size = new System.Drawing.Size(494, 20);
            this.uclTitleParameter.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleParameter.TabIndex = 25;
            this.uclTitleParameter.Title = DACrux.SP.Common.MultiLang.SelectLang["Parameter List"];
            // 
            // uclTitleMethod
            // 
            this.uclTitleMethod.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleMethod.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleMethod.BackgroundImage")));
            this.uclTitleMethod.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleMethod.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleMethod.EndColor = System.Drawing.Color.White;
            this.uclTitleMethod.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleMethod.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleMethod.Location = new System.Drawing.Point(0, 139);
            this.uclTitleMethod.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleMethod.Name = "uclTitleMethod";
            this.uclTitleMethod.Size = new System.Drawing.Size(494, 20);
            this.uclTitleMethod.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleMethod.TabIndex = 18;
            this.uclTitleMethod.Title = DACrux.SP.Common.MultiLang.SelectLang["Method List"];
            // 
            // uclTitleCom
            // 
            this.uclTitleCom.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleCom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleCom.BackgroundImage")));
            this.uclTitleCom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleCom.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleCom.EndColor = System.Drawing.Color.White;
            this.uclTitleCom.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleCom.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleCom.Location = new System.Drawing.Point(0, 0);
            this.uclTitleCom.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleCom.Name = "uclTitleCom";
            this.uclTitleCom.Size = new System.Drawing.Size(494, 20);
            this.uclTitleCom.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleCom.TabIndex = 16;
            this.uclTitleCom.Title = DACrux.SP.Common.MultiLang.SelectLang["Component List"];
            // 
            // cboResultUseType
            // 
            this.cboResultUseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboResultUseType.EnumType = null;
            this.cboResultUseType.Font = new System.Drawing.Font("Tahoma", 8F);
            this.cboResultUseType.FormattingEnabled = true;
            this.cboResultUseType.Location = new System.Drawing.Point(327, 33);
            this.cboResultUseType.Name = "cboResultUseType";
            this.cboResultUseType.Size = new System.Drawing.Size(155, 21);
            this.cboResultUseType.TabIndex = 20;
            // 
            // uclTitleGeneral
            // 
            this.uclTitleGeneral.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleGeneral.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleGeneral.BackgroundImage")));
            this.uclTitleGeneral.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleGeneral.EndColor = System.Drawing.Color.White;
            this.uclTitleGeneral.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleGeneral.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleGeneral.Location = new System.Drawing.Point(0, 0);
            this.uclTitleGeneral.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleGeneral.Name = "uclTitleGeneral";
            this.uclTitleGeneral.Size = new System.Drawing.Size(494, 20);
            this.uclTitleGeneral.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleGeneral.TabIndex = 17;
            this.uclTitleGeneral.Title = DACrux.SP.Common.MultiLang.SelectLang["General"];
            // 
            // dlgTaskComPlus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 518);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dlgTaskComPlus";
            this.ShowInTaskbar = false;
            this.Text = DACrux.SP.Common.MultiLang.SelectLang["COM+ Task"];
            this.panel1.ResumeLayout(false);
            this.pnlParameter.ResumeLayout(false);
            this.pnlMethod.ResumeLayout(false);
            this.pnlCom.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlMethod;
        private System.Windows.Forms.ListView lvMethod;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel pnlCom;
        private System.Windows.Forms.ListView lvComponent;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private DACrux.SP.Controls.uclTitle uclTitleCom;
        private System.Windows.Forms.Panel pnlParameter;
        private System.Windows.Forms.ListView lvParameter;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private DACrux.SP.Controls.uclTitle uclTitleMethod;
        private DACrux.SP.Controls.uclTitle uclTitleParameter;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel2;
        private DACrux.SP.Controls.uclTitle uclTitleGeneral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTaskName;
        private DACrux.SP.Controls.EnumComboBox cboResultUseType;
        private System.Windows.Forms.Label label2;

    }
}