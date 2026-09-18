namespace SmartParser.Designer
{
    partial class dlgOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgOption));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvFileList = new System.Windows.Forms.ListView();
            this.columnFileName = new System.Windows.Forms.ColumnHeader();
            this.uclTitle2 = new DACrux.SP.Controls.uclTitle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.uclRegex = new DACrux.SP.Controls.RegexBuilder();
            this.uclTitle4 = new DACrux.SP.Controls.uclTitle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.rbtSingle = new System.Windows.Forms.RadioButton();
            this.uclTitle3 = new DACrux.SP.Controls.uclTitle();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.38346F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.61654F));
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(532, 219);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.uclTitle2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panel4.Location = new System.Drawing.Point(287, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(242, 213);
            this.panel4.TabIndex = 18;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.Controls.Add(this.btnRemove);
            this.panel6.Controls.Add(this.btnAdd);
            this.panel6.Controls.Add(this.lvFileList);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panel6.Location = new System.Drawing.Point(0, 20);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(242, 193);
            this.panel6.TabIndex = 17;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(167, 6);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(67, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = DACrux.SP.Common.MultiLang.SelectLang["Remove"];
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(122, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(44, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = DACrux.SP.Common.MultiLang.SelectLang["Add"];
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvFileList
            // 
            this.lvFileList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFileName});
            this.lvFileList.FullRowSelect = true;
            this.lvFileList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFileList.Location = new System.Drawing.Point(7, 32);
            this.lvFileList.MultiSelect = false;
            this.lvFileList.Name = "lvFileList";
            this.lvFileList.Size = new System.Drawing.Size(227, 156);
            this.lvFileList.TabIndex = 0;
            this.lvFileList.UseCompatibleStateImageBehavior = false;
            this.lvFileList.View = System.Windows.Forms.View.Details;
            // 
            // columnFileName
            // 
            this.columnFileName.Text = DACrux.SP.Common.MultiLang.SelectLang["File Name"];
            this.columnFileName.Width = 100;
            // 
            // uclTitle2
            // 
            this.uclTitle2.BackColor = System.Drawing.Color.Transparent;
            this.uclTitle2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitle2.BackgroundImage")));
            this.uclTitle2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitle2.EndColor = System.Drawing.Color.White;
            this.uclTitle2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitle2.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitle2.Location = new System.Drawing.Point(0, 0);
            this.uclTitle2.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitle2.Name = "uclTitle2";
            this.uclTitle2.Size = new System.Drawing.Size(242, 20);
            this.uclTitle2.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitle2.TabIndex = 16;
            this.uclTitle2.Title = DACrux.SP.Common.MultiLang.SelectLang["Sample File"];
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.uclTitle4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.uclTitle3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 213);
            this.panel1.TabIndex = 17;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.uclRegex);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panel3.Location = new System.Drawing.Point(0, 85);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(278, 128);
            this.panel3.TabIndex = 20;
            // 
            // uclRegex
            // 
            this.uclRegex.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclRegex.Location = new System.Drawing.Point(11, 7);
            this.uclRegex.Name = "uclRegex";
            this.uclRegex.RegexOption = ((System.Text.RegularExpressions.RegexOptions)(((System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                        | System.Text.RegularExpressions.RegexOptions.Singleline)));
            this.uclRegex.RegexString = "";
            this.uclRegex.ShowIgnoreCase = true;
            this.uclRegex.Size = new System.Drawing.Size(259, 116);
            this.uclRegex.TabIndex = 20;
            // 
            // uclTitle4
            // 
            this.uclTitle4.BackColor = System.Drawing.Color.Transparent;
            this.uclTitle4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitle4.BackgroundImage")));
            this.uclTitle4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitle4.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitle4.EndColor = System.Drawing.Color.White;
            this.uclTitle4.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitle4.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitle4.Location = new System.Drawing.Point(0, 65);
            this.uclTitle4.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitle4.Name = "uclTitle4";
            this.uclTitle4.Size = new System.Drawing.Size(278, 20);
            this.uclTitle4.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitle4.TabIndex = 18;
            this.uclTitle4.Title = DACrux.SP.Common.MultiLang.SelectLang["Target File Naming Rule"];
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.radioButton2);
            this.panel2.Controls.Add(this.rbtSingle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panel2.Location = new System.Drawing.Point(0, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(278, 45);
            this.panel2.TabIndex = 17;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.radioButton2.Location = new System.Drawing.Point(121, 14);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(69, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = DACrux.SP.Common.MultiLang.SelectLang["Multi Unit"];
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // rbtSingle
            // 
            this.rbtSingle.AutoSize = true;
            this.rbtSingle.Font = new System.Drawing.Font("Tahoma", 8F);
            this.rbtSingle.Location = new System.Drawing.Point(18, 14);
            this.rbtSingle.Name = "rbtSingle";
            this.rbtSingle.Size = new System.Drawing.Size(75, 17);
            this.rbtSingle.TabIndex = 0;
            this.rbtSingle.Text = DACrux.SP.Common.MultiLang.SelectLang["Single Unit"];
            this.rbtSingle.UseVisualStyleBackColor = true;
            // 
            // uclTitle3
            // 
            this.uclTitle3.BackColor = System.Drawing.Color.Transparent;
            this.uclTitle3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitle3.BackgroundImage")));
            this.uclTitle3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitle3.EndColor = System.Drawing.Color.White;
            this.uclTitle3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitle3.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitle3.Location = new System.Drawing.Point(0, 0);
            this.uclTitle3.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitle3.Name = "uclTitle3";
            this.uclTitle3.Size = new System.Drawing.Size(278, 20);
            this.uclTitle3.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitle3.TabIndex = 16;
            this.uclTitle3.Title = DACrux.SP.Common.MultiLang.SelectLang["Analysis Unit Mode"];
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(374, 225);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = DACrux.SP.Common.MultiLang.SelectLang["Save"];
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(455, 225);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = DACrux.SP.Common.MultiLang.SelectLang["Cancel"];
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dlgOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 251);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dlgOption";
            this.ShowInTaskbar = false;
            this.Text = DACrux.SP.Common.MultiLang.SelectLang["Option"];
            this.Load += new System.EventHandler(this.dlgOption_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private DACrux.SP.Controls.uclTitle uclTitle2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private DACrux.SP.Controls.RegexBuilder uclRegex;
        private DACrux.SP.Controls.uclTitle uclTitle4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton rbtSingle;
        private DACrux.SP.Controls.uclTitle uclTitle3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListView lvFileList;
        private System.Windows.Forms.ColumnHeader columnFileName;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;

    }
}