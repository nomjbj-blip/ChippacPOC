namespace SmartParser.Designer
{
    partial class dlgTaskDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgTaskDB));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlParameter = new System.Windows.Forms.Panel();
            this.lvParameter = new System.Windows.Forms.ListView();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.uclTitleParameter = new DACrux.SP.Controls.uclTitle();
            this.pnlQuery = new System.Windows.Forms.Panel();
            this.txtQuery = new DACrux.SP.Controls.SyntaxRichTextBox();
            this.uclTitleQuery = new DACrux.SP.Controls.uclTitle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTargetColumn = new System.Windows.Forms.Label();
            this.txtTargetColumn = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboQueryType = new DACrux.SP.Controls.EnumComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboResultUseType = new DACrux.SP.Controls.EnumComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.uclTitleGeneral = new DACrux.SP.Controls.uclTitle();
            this.panel1.SuspendLayout();
            this.pnlParameter.SuspendLayout();
            this.pnlQuery.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(338, 467);
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
            this.btnCancel.Location = new System.Drawing.Point(419, 467);
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
            this.panel1.Controls.Add(this.pnlQuery);
            this.panel1.Controls.Add(this.uclTitleQuery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panel1.Location = new System.Drawing.Point(0, 97);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 361);
            this.panel1.TabIndex = 21;
            // 
            // pnlParameter
            // 
            this.pnlParameter.BackColor = System.Drawing.Color.White;
            this.pnlParameter.Controls.Add(this.lvParameter);
            this.pnlParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParameter.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pnlParameter.Location = new System.Drawing.Point(0, 159);
            this.pnlParameter.Name = "pnlParameter";
            this.pnlParameter.Size = new System.Drawing.Size(494, 202);
            this.pnlParameter.TabIndex = 22;
            // 
            // lvParameter
            // 
            this.lvParameter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvParameter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7});
            this.lvParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvParameter.FullRowSelect = true;
            this.lvParameter.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvParameter.Location = new System.Drawing.Point(0, 0);
            this.lvParameter.MultiSelect = false;
            this.lvParameter.Name = "lvParameter";
            this.lvParameter.Size = new System.Drawing.Size(494, 202);
            this.lvParameter.TabIndex = 2;
            this.lvParameter.UseCompatibleStateImageBehavior = false;
            this.lvParameter.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = DACrux.SP.Common.MultiLang.SelectLang["Name"];
            this.columnHeader7.Width = 200;
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
            this.uclTitleParameter.Location = new System.Drawing.Point(0, 139);
            this.uclTitleParameter.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleParameter.Name = "uclTitleParameter";
            this.uclTitleParameter.Size = new System.Drawing.Size(494, 20);
            this.uclTitleParameter.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleParameter.TabIndex = 25;
            this.uclTitleParameter.Title = DACrux.SP.Common.MultiLang.SelectLang["Parameter List"];
            // 
            // pnlQuery
            // 
            this.pnlQuery.BackColor = System.Drawing.Color.White;
            this.pnlQuery.Controls.Add(this.txtQuery);
            this.pnlQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuery.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pnlQuery.Location = new System.Drawing.Point(0, 20);
            this.pnlQuery.Name = "pnlQuery";
            this.pnlQuery.Size = new System.Drawing.Size(494, 119);
            this.pnlQuery.TabIndex = 17;
            // 
            // txtQuery
            // 
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.Font = new System.Drawing.Font("Consolas", 8F);
            this.txtQuery.Location = new System.Drawing.Point(0, 0);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(494, 119);
            this.txtQuery.TabIndex = 0;
            this.txtQuery.Text = "";
            // 
            // uclTitleQuery
            // 
            this.uclTitleQuery.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleQuery.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleQuery.BackgroundImage")));
            this.uclTitleQuery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleQuery.EndColor = System.Drawing.Color.White;
            this.uclTitleQuery.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleQuery.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleQuery.Location = new System.Drawing.Point(0, 0);
            this.uclTitleQuery.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleQuery.Name = "uclTitleQuery";
            this.uclTitleQuery.Size = new System.Drawing.Size(494, 20);
            this.uclTitleQuery.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleQuery.TabIndex = 16;
            this.uclTitleQuery.Title = "Query";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.lblTargetColumn);
            this.panel2.Controls.Add(this.txtTargetColumn);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.cboQueryType);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cboResultUseType);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtTaskName);
            this.panel2.Controls.Add(this.uclTitleGeneral);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(494, 97);
            this.panel2.TabIndex = 22;
            // 
            // lblTargetColumn
            // 
            this.lblTargetColumn.AutoSize = true;
            this.lblTargetColumn.Location = new System.Drawing.Point(234, 62);
            this.lblTargetColumn.Name = "lblTargetColumn";
            this.lblTargetColumn.Size = new System.Drawing.Size(88, 14);
            this.lblTargetColumn.TabIndex = 25;
            this.lblTargetColumn.Text = DACrux.SP.Common.MultiLang.SelectLang["Target Column"];
            this.lblTargetColumn.Visible = false;
            // 
            // txtTargetColumn
            // 
            this.txtTargetColumn.Location = new System.Drawing.Point(327, 59);
            this.txtTargetColumn.Name = "txtTargetColumn";
            this.txtTargetColumn.Size = new System.Drawing.Size(155, 22);
            this.txtTargetColumn.TabIndex = 24;
            this.txtTargetColumn.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 14);
            this.label3.TabIndex = 23;
            this.label3.Text = DACrux.SP.Common.MultiLang.SelectLang["Query Type"];
            // 
            // cboQueryType
            // 
            this.cboQueryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQueryType.EnumType = null;
            this.cboQueryType.Font = new System.Drawing.Font("Tahoma", 8F);
            this.cboQueryType.FormattingEnabled = true;
            this.cboQueryType.Location = new System.Drawing.Point(85, 60);
            this.cboQueryType.Name = "cboQueryType";
            this.cboQueryType.Size = new System.Drawing.Size(138, 21);
            this.cboQueryType.TabIndex = 22;
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
            // dlgTaskDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 495);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dlgTaskDB";
            this.ShowInTaskbar = false;
            this.Text = "Database Task";
            this.panel1.ResumeLayout(false);
            this.pnlParameter.ResumeLayout(false);
            this.pnlQuery.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlQuery;
        private DACrux.SP.Controls.uclTitle uclTitleQuery;
        private System.Windows.Forms.Panel pnlParameter;
        private System.Windows.Forms.ListView lvParameter;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private DACrux.SP.Controls.uclTitle uclTitleParameter;
        private System.Windows.Forms.Panel panel2;
        private DACrux.SP.Controls.uclTitle uclTitleGeneral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTaskName;
        private DACrux.SP.Controls.EnumComboBox cboResultUseType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DACrux.SP.Controls.EnumComboBox cboQueryType;
        private DACrux.SP.Controls.SyntaxRichTextBox txtQuery;
        private System.Windows.Forms.Label lblTargetColumn;
        private System.Windows.Forms.TextBox txtTargetColumn;

    }
}