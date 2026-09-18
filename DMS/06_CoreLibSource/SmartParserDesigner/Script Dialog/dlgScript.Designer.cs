namespace SmartParser.Designer
{
    partial class dlgScript
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgScript));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlArgument = new System.Windows.Forms.Panel();
            this.pnlArgsLeft = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lvTaskArgument = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddTask = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtData = new System.Windows.Forms.TextBox();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlArgsRight = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lvArgument = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.uclTitleArgument = new DACrux.SP.Controls.uclTitle();
            this.pnlTarget = new System.Windows.Forms.Panel();
            this.lvTaskTarget = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uclTitleTarget = new DACrux.SP.Controls.uclTitle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboCommand = new DACrux.SP.Controls.EnumComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboRunningMode = new DACrux.SP.Controls.EnumComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.uclTitleGeneral = new DACrux.SP.Controls.uclTitle();
            this.panel1.SuspendLayout();
            this.pnlArgument.SuspendLayout();
            this.pnlArgsLeft.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlArgsRight.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.pnlTarget.SuspendLayout();
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
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlArgument);
            this.panel1.Controls.Add(this.uclTitleArgument);
            this.panel1.Controls.Add(this.pnlTarget);
            this.panel1.Controls.Add(this.uclTitleTarget);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panel1.Location = new System.Drawing.Point(0, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 422);
            this.panel1.TabIndex = 21;
            // 
            // pnlArgument
            // 
            this.pnlArgument.BackColor = System.Drawing.Color.White;
            this.pnlArgument.Controls.Add(this.pnlArgsLeft);
            this.pnlArgument.Controls.Add(this.splitter2);
            this.pnlArgument.Controls.Add(this.pnlArgsRight);
            this.pnlArgument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlArgument.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pnlArgument.Location = new System.Drawing.Point(0, 159);
            this.pnlArgument.Name = "pnlArgument";
            this.pnlArgument.Size = new System.Drawing.Size(494, 263);
            this.pnlArgument.TabIndex = 20;
            // 
            // pnlArgsLeft
            // 
            this.pnlArgsLeft.Controls.Add(this.panel3);
            this.pnlArgsLeft.Controls.Add(this.splitter1);
            this.pnlArgsLeft.Controls.Add(this.panel4);
            this.pnlArgsLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlArgsLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlArgsLeft.Name = "pnlArgsLeft";
            this.pnlArgsLeft.Size = new System.Drawing.Size(276, 263);
            this.pnlArgsLeft.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lvTaskArgument);
            this.panel3.Controls.Add(this.btnAddTask);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panel3.Size = new System.Drawing.Size(276, 176);
            this.panel3.TabIndex = 6;
            // 
            // lvTaskArgument
            // 
            this.lvTaskArgument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvTaskArgument.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvTaskArgument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTaskArgument.FullRowSelect = true;
            this.lvTaskArgument.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTaskArgument.Location = new System.Drawing.Point(0, 16);
            this.lvTaskArgument.MultiSelect = false;
            this.lvTaskArgument.Name = "lvTaskArgument";
            this.lvTaskArgument.Size = new System.Drawing.Size(276, 141);
            this.lvTaskArgument.TabIndex = 4;
            this.lvTaskArgument.UseCompatibleStateImageBehavior = false;
            this.lvTaskArgument.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "";
            this.columnHeader2.Width = 100;
            // 
            // btnAddTask
            // 
            this.btnAddTask.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddTask.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAddTask.Location = new System.Drawing.Point(0, 157);
            this.btnAddTask.Name = "btnAddTask";
            this.btnAddTask.Size = new System.Drawing.Size(276, 19);
            this.btnAddTask.TabIndex = 5;
            this.btnAddTask.UseCompatibleTextRendering = true;
            this.btnAddTask.UseVisualStyleBackColor = false;
            this.btnAddTask.Click += new System.EventHandler(this.btnAddTask_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.Gainsboro;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 176);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(276, 7);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtData);
            this.panel4.Controls.Add(this.btnSaveData);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 183);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(276, 80);
            this.panel4.TabIndex = 10;
            // 
            // txtData
            // 
            this.txtData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtData.Location = new System.Drawing.Point(0, 16);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(276, 45);
            this.txtData.TabIndex = 2;
            // 
            // btnSaveData
            // 
            this.btnSaveData.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSaveData.Location = new System.Drawing.Point(0, 61);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(276, 19);
            this.btnSaveData.TabIndex = 6;
            this.btnSaveData.UseCompatibleTextRendering = true;
            this.btnSaveData.UseVisualStyleBackColor = false;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label4.Size = new System.Drawing.Size(77, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Constant Data";
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.Color.Gainsboro;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(276, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(7, 263);
            this.splitter2.TabIndex = 5;
            this.splitter2.TabStop = false;
            // 
            // pnlArgsRight
            // 
            this.pnlArgsRight.Controls.Add(this.panel6);
            this.pnlArgsRight.Controls.Add(this.panel5);
            this.pnlArgsRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlArgsRight.Location = new System.Drawing.Point(283, 0);
            this.pnlArgsRight.Name = "pnlArgsRight";
            this.pnlArgsRight.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pnlArgsRight.Size = new System.Drawing.Size(211, 263);
            this.pnlArgsRight.TabIndex = 3;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnRemove);
            this.panel6.Controls.Add(this.btnDown);
            this.panel6.Controls.Add(this.btnUp);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 230);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(211, 33);
            this.panel6.TabIndex = 5;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemove.Location = new System.Drawing.Point(147, 7);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(58, 19);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.UseCompatibleTextRendering = true;
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDown.BackColor = System.Drawing.SystemColors.Control;
            this.btnDown.Location = new System.Drawing.Point(28, 7);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(20, 19);
            this.btnDown.TabIndex = 1;
            this.btnDown.Text = "▼";
            this.btnDown.UseCompatibleTextRendering = true;
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUp.BackColor = System.Drawing.SystemColors.Control;
            this.btnUp.Location = new System.Drawing.Point(3, 7);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(20, 19);
            this.btnUp.TabIndex = 0;
            this.btnUp.Text = "▲";
            this.btnUp.UseCompatibleTextRendering = true;
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lvArgument);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(211, 260);
            this.panel5.TabIndex = 5;
            // 
            // lvArgument
            // 
            this.lvArgument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvArgument.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7});
            this.lvArgument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvArgument.FullRowSelect = true;
            this.lvArgument.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvArgument.Location = new System.Drawing.Point(0, 13);
            this.lvArgument.MultiSelect = false;
            this.lvArgument.Name = "lvArgument";
            this.lvArgument.Size = new System.Drawing.Size(211, 247);
            this.lvArgument.TabIndex = 4;
            this.lvArgument.UseCompatibleStateImageBehavior = false;
            this.lvArgument.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "";
            this.columnHeader7.Width = 120;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 1;
            // 
            // uclTitleArgument
            // 
            this.uclTitleArgument.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleArgument.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleArgument.BackgroundImage")));
            this.uclTitleArgument.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleArgument.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleArgument.EndColor = System.Drawing.Color.White;
            this.uclTitleArgument.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleArgument.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleArgument.Location = new System.Drawing.Point(0, 139);
            this.uclTitleArgument.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleArgument.Name = "uclTitleArgument";
            this.uclTitleArgument.Size = new System.Drawing.Size(494, 20);
            this.uclTitleArgument.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleArgument.TabIndex = 18;
            this.uclTitleArgument.Title = "";
            // 
            // pnlTarget
            // 
            this.pnlTarget.BackColor = System.Drawing.Color.White;
            this.pnlTarget.Controls.Add(this.lvTaskTarget);
            this.pnlTarget.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTarget.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pnlTarget.Location = new System.Drawing.Point(0, 20);
            this.pnlTarget.Name = "pnlTarget";
            this.pnlTarget.Size = new System.Drawing.Size(494, 119);
            this.pnlTarget.TabIndex = 17;
            // 
            // lvTaskTarget
            // 
            this.lvTaskTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvTaskTarget.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvTaskTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTaskTarget.FullRowSelect = true;
            this.lvTaskTarget.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTaskTarget.Location = new System.Drawing.Point(0, 0);
            this.lvTaskTarget.MultiSelect = false;
            this.lvTaskTarget.Name = "lvTaskTarget";
            this.lvTaskTarget.Size = new System.Drawing.Size(494, 119);
            this.lvTaskTarget.TabIndex = 2;
            this.lvTaskTarget.UseCompatibleStateImageBehavior = false;
            this.lvTaskTarget.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "";
            this.columnHeader3.Width = 300;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "";
            this.columnHeader4.Width = 150;
            // 
            // uclTitleTarget
            // 
            this.uclTitleTarget.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleTarget.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleTarget.BackgroundImage")));
            this.uclTitleTarget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleTarget.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleTarget.EndColor = System.Drawing.Color.White;
            this.uclTitleTarget.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleTarget.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleTarget.Location = new System.Drawing.Point(0, 0);
            this.uclTitleTarget.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleTarget.Name = "uclTitleTarget";
            this.uclTitleTarget.Size = new System.Drawing.Size(494, 20);
            this.uclTitleTarget.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleTarget.TabIndex = 16;
            this.uclTitleTarget.Title = "";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.cboCommand);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cboRunningMode);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.uclTitleGeneral);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(494, 63);
            this.panel2.TabIndex = 22;
            // 
            // cboCommand
            // 
            this.cboCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCommand.EnumType = null;
            this.cboCommand.Font = new System.Drawing.Font("Tahoma", 8F);
            this.cboCommand.FormattingEnabled = true;
            this.cboCommand.Location = new System.Drawing.Point(79, 33);
            this.cboCommand.Name = "cboCommand";
            this.cboCommand.Size = new System.Drawing.Size(137, 21);
            this.cboCommand.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 14);
            this.label2.TabIndex = 21;
            // 
            // cboRunningMode
            // 
            this.cboRunningMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRunningMode.EnumType = null;
            this.cboRunningMode.Font = new System.Drawing.Font("Tahoma", 8F);
            this.cboRunningMode.FormattingEnabled = true;
            this.cboRunningMode.Location = new System.Drawing.Point(327, 33);
            this.cboRunningMode.Name = "cboRunningMode";
            this.cboRunningMode.Size = new System.Drawing.Size(155, 21);
            this.cboRunningMode.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 14);
            this.label1.TabIndex = 19;
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
            this.uclTitleGeneral.Title = "";
            // 
            // dlgScript
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
            this.Name = "dlgScript";
            this.ShowInTaskbar = false;
            this.Text = "Script";
            this.panel1.ResumeLayout(false);
            this.pnlArgument.ResumeLayout(false);
            this.pnlArgsLeft.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlArgsRight.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.pnlTarget.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlArgument;
        private System.Windows.Forms.Panel pnlTarget;
        private System.Windows.Forms.ListView lvTaskTarget;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private DACrux.SP.Controls.uclTitle uclTitleTarget;
        private DACrux.SP.Controls.uclTitle uclTitleArgument;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel2;
        private DACrux.SP.Controls.uclTitle uclTitleGeneral;
        private System.Windows.Forms.Label label1;
        private DACrux.SP.Controls.EnumComboBox cboRunningMode;
        private System.Windows.Forms.Label label2;
        private DACrux.SP.Controls.EnumComboBox cboCommand;
        private System.Windows.Forms.Panel pnlArgsRight;
        private System.Windows.Forms.Panel pnlArgsLeft;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListView lvTaskArgument;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ListView lvArgument;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnAddTask;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.Splitter splitter2;

    }
}