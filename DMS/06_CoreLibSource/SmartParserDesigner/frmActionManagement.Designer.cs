namespace SmartParser.Designer
{
    partial class frmActionManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActionManagement));
            this.panel1 = new System.Windows.Forms.Panel();
            this.uclTitleTask = new DACrux.SP.Controls.uclTitle();
            this.cboTaskType = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lvTask = new System.Windows.Forms.ListView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.uclTitleScript = new DACrux.SP.Controls.uclTitle();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnRemoveScript = new System.Windows.Forms.Button();
            this.btnEditScript = new System.Windows.Forms.Button();
            this.btnAddScript = new System.Windows.Forms.Button();
            this.lvScript = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.uclTitleTask);
            this.panel1.Controls.Add(this.cboTaskType);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lvTask);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 537);
            this.panel1.TabIndex = 0;
            // 
            // uclTitleTask
            // 
            this.uclTitleTask.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleTask.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleTask.BackgroundImage")));
            this.uclTitleTask.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleTask.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleTask.EndColor = System.Drawing.Color.White;
            this.uclTitleTask.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleTask.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleTask.Location = new System.Drawing.Point(0, 0);
            this.uclTitleTask.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleTask.Name = "uclTitleTask";
            this.uclTitleTask.Size = new System.Drawing.Size(347, 20);
            this.uclTitleTask.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleTask.TabIndex = 30;
            this.uclTitleTask.Title = DACrux.SP.Common.MultiLang.SelectLang["Task List"];
            // 
            // cboTaskType
            // 
            this.cboTaskType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTaskType.FormattingEnabled = true;
            this.cboTaskType.Location = new System.Drawing.Point(-38, 28);
            this.cboTaskType.Name = "cboTaskType";
            this.cboTaskType.Size = new System.Drawing.Size(186, 22);
            this.cboTaskType.TabIndex = 29;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnDelete.Location = new System.Drawing.Point(278, 27);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(64, 25);
            this.btnDelete.TabIndex = 28;
            this.btnDelete.Text = DACrux.SP.Common.MultiLang.SelectLang["Delete"];
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.BackColor = System.Drawing.SystemColors.Control;
            this.btnEdit.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnEdit.Location = new System.Drawing.Point(222, 27);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(50, 25);
            this.btnEdit.TabIndex = 27;
            this.btnEdit.Text = DACrux.SP.Common.MultiLang.SelectLang["Edit"];
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnAdd.Location = new System.Drawing.Point(154, 27);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(62, 25);
            this.btnAdd.TabIndex = 26;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = DACrux.SP.Common.MultiLang.SelectLang["Add"];
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label3.Location = new System.Drawing.Point(-100, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = DACrux.SP.Common.MultiLang.SelectLang["Task Type"];
            // 
            // lvTask
            // 
            this.lvTask.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTask.BackColor = System.Drawing.Color.White;
            this.lvTask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvTask.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lvTask.FullRowSelect = true;
            this.lvTask.GridLines = true;
            this.lvTask.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTask.Location = new System.Drawing.Point(6, 59);
            this.lvTask.MultiSelect = false;
            this.lvTask.Name = "lvTask";
            this.lvTask.Size = new System.Drawing.Size(336, 471);
            this.lvTask.TabIndex = 16;
            this.lvTask.UseCompatibleStateImageBehavior = false;
            this.lvTask.View = System.Windows.Forms.View.Details;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(706, 543);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.uclTitleScript);
            this.panel2.Controls.Add(this.btnDown);
            this.panel2.Controls.Add(this.btnUp);
            this.panel2.Controls.Add(this.btnRemoveScript);
            this.panel2.Controls.Add(this.btnEditScript);
            this.panel2.Controls.Add(this.btnAddScript);
            this.panel2.Controls.Add(this.lvScript);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(356, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(347, 537);
            this.panel2.TabIndex = 37;
            // 
            // uclTitleScript
            // 
            this.uclTitleScript.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleScript.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleScript.BackgroundImage")));
            this.uclTitleScript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleScript.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleScript.EndColor = System.Drawing.Color.White;
            this.uclTitleScript.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleScript.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleScript.Location = new System.Drawing.Point(0, 0);
            this.uclTitleScript.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleScript.Name = "uclTitleScript";
            this.uclTitleScript.Size = new System.Drawing.Size(347, 20);
            this.uclTitleScript.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleScript.TabIndex = 44;
            this.uclTitleScript.Title = DACrux.SP.Common.MultiLang.SelectLang["Script List"];
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.SystemColors.Control;
            this.btnDown.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnDown.Location = new System.Drawing.Point(37, 26);
            this.btnDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(25, 25);
            this.btnDown.TabIndex = 42;
            this.btnDown.TabStop = false;
            this.btnDown.Text = "▼";
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.SystemColors.Control;
            this.btnUp.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnUp.Location = new System.Drawing.Point(6, 26);
            this.btnUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(25, 25);
            this.btnUp.TabIndex = 41;
            this.btnUp.TabStop = false;
            this.btnUp.Text = "▲";
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnRemoveScript
            // 
            this.btnRemoveScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveScript.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemoveScript.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnRemoveScript.Location = new System.Drawing.Point(278, 24);
            this.btnRemoveScript.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRemoveScript.Name = "btnRemoveScript";
            this.btnRemoveScript.Size = new System.Drawing.Size(64, 25);
            this.btnRemoveScript.TabIndex = 40;
            this.btnRemoveScript.Text = DACrux.SP.Common.MultiLang.SelectLang["Remove"];
            this.btnRemoveScript.UseVisualStyleBackColor = false;
            this.btnRemoveScript.Click += new System.EventHandler(this.btnRemoveScript_Click);
            // 
            // btnEditScript
            // 
            this.btnEditScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditScript.BackColor = System.Drawing.SystemColors.Control;
            this.btnEditScript.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnEditScript.Location = new System.Drawing.Point(222, 24);
            this.btnEditScript.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEditScript.Name = "btnEditScript";
            this.btnEditScript.Size = new System.Drawing.Size(50, 25);
            this.btnEditScript.TabIndex = 39;
            this.btnEditScript.Text = DACrux.SP.Common.MultiLang.SelectLang["Edit"];
            this.btnEditScript.UseVisualStyleBackColor = false;
            this.btnEditScript.Click += new System.EventHandler(this.btnEditScript_Click);
            // 
            // btnAddScript
            // 
            this.btnAddScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddScript.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddScript.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnAddScript.Location = new System.Drawing.Point(154, 24);
            this.btnAddScript.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddScript.Name = "btnAddScript";
            this.btnAddScript.Size = new System.Drawing.Size(62, 25);
            this.btnAddScript.TabIndex = 38;
            this.btnAddScript.TabStop = false;
            this.btnAddScript.Text = DACrux.SP.Common.MultiLang.SelectLang["Add"];
            this.btnAddScript.UseVisualStyleBackColor = false;
            this.btnAddScript.Click += new System.EventHandler(this.btnAddScript_Click);
            // 
            // lvScript
            // 
            this.lvScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvScript.BackColor = System.Drawing.Color.White;
            this.lvScript.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvScript.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lvScript.FullRowSelect = true;
            this.lvScript.GridLines = true;
            this.lvScript.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvScript.Location = new System.Drawing.Point(6, 59);
            this.lvScript.MultiSelect = false;
            this.lvScript.Name = "lvScript";
            this.lvScript.Size = new System.Drawing.Size(336, 469);
            this.lvScript.TabIndex = 36;
            this.lvScript.UseCompatibleStateImageBehavior = false;
            this.lvScript.View = System.Windows.Forms.View.Details;
            // 
            // frmActionManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 543);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Name = "frmActionManagement";
            this.Text = "Action Management";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView lvTask;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.ComboBox cboTaskType;
        private DACrux.SP.Controls.uclTitle uclTitleTask;
        private System.Windows.Forms.Panel panel2;
        private DACrux.SP.Controls.uclTitle uclTitleScript;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnRemoveScript;
        private System.Windows.Forms.Button btnEditScript;
        private System.Windows.Forms.Button btnAddScript;
        private System.Windows.Forms.ListView lvScript;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;


    }
}