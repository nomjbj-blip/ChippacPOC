namespace SmartParser.Designer
{
    partial class dlgTaskEntityValue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgTaskEntityValue));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlCom = new System.Windows.Forms.Panel();
            this.lvEntity = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.uclTitleEntity = new DACrux.SP.Controls.uclTitle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cboResultUseType = new DACrux.SP.Controls.EnumComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.uclTitleGeneral = new DACrux.SP.Controls.uclTitle();
            this.panel1.SuspendLayout();
            this.pnlCom.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(338, 328);
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
            this.btnCancel.Location = new System.Drawing.Point(419, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = DACrux.SP.Common.MultiLang.SelectLang["Cancel"];
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlCom);
            this.panel1.Controls.Add(this.uclTitleEntity);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panel1.Location = new System.Drawing.Point(0, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 258);
            this.panel1.TabIndex = 21;
            // 
            // pnlCom
            // 
            this.pnlCom.BackColor = System.Drawing.Color.White;
            this.pnlCom.Controls.Add(this.lvEntity);
            this.pnlCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCom.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pnlCom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.pnlCom.Location = new System.Drawing.Point(0, 20);
            this.pnlCom.Name = "pnlCom";
            this.pnlCom.Size = new System.Drawing.Size(494, 238);
            this.pnlCom.TabIndex = 17;
            // 
            // lvEntity
            // 
            this.lvEntity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvEntity.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvEntity.FullRowSelect = true;
            this.lvEntity.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvEntity.Location = new System.Drawing.Point(0, 0);
            this.lvEntity.MultiSelect = false;
            this.lvEntity.Name = "lvEntity";
            this.lvEntity.Size = new System.Drawing.Size(494, 238);
            this.lvEntity.TabIndex = 2;
            this.lvEntity.UseCompatibleStateImageBehavior = false;
            this.lvEntity.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = DACrux.SP.Common.MultiLang.SelectLang["Entity"];
            this.columnHeader3.Width = 200;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = DACrux.SP.Common.MultiLang.SelectLang["Parent Section"];
            this.columnHeader4.Width = 200;
            // 
            // uclTitleEntity
            // 
            this.uclTitleEntity.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleEntity.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleEntity.BackgroundImage")));
            this.uclTitleEntity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleEntity.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleEntity.EndColor = System.Drawing.Color.White;
            this.uclTitleEntity.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleEntity.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleEntity.Location = new System.Drawing.Point(0, 0);
            this.uclTitleEntity.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleEntity.Name = "uclTitleEntity";
            this.uclTitleEntity.Size = new System.Drawing.Size(494, 20);
            this.uclTitleEntity.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleEntity.TabIndex = 16;
            this.uclTitleEntity.Title = DACrux.SP.Common.MultiLang.SelectLang["Entity List"];
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 21;
            this.label2.Text = DACrux.SP.Common.MultiLang.SelectLang["Result Usage"];
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
            // dlgTaskEntityValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 356);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dlgTaskEntityValue";
            this.ShowInTaskbar = false;
            this.Text = DACrux.SP.Common.MultiLang.SelectLang["Entity Value Task"];
            this.panel1.ResumeLayout(false);
            this.pnlCom.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlCom;
        private System.Windows.Forms.ListView lvEntity;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private DACrux.SP.Controls.uclTitle uclTitleEntity;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel2;
        private DACrux.SP.Controls.uclTitle uclTitleGeneral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTaskName;
        private DACrux.SP.Controls.EnumComboBox cboResultUseType;
        private System.Windows.Forms.Label label2;

    }
}