namespace SmartParser.Designer
{
    partial class frmTableDataMatching
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTableDataMatching));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDB = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnlTableList = new System.Windows.Forms.Panel();
            this.lvTable = new System.Windows.Forms.ListView();
            this.uclTitle1 = new DACrux.SP.Controls.uclTitle();
            this.tabControl1.SuspendLayout();
            this.tabPageDB.SuspendLayout();
            this.pnlTableList.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPageDB);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(214, 543);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageDB
            // 
            this.tabPageDB.Controls.Add(this.pnlTableList);
            this.tabPageDB.Location = new System.Drawing.Point(4, 4);
            this.tabPageDB.Name = "tabPageDB";
            this.tabPageDB.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDB.Size = new System.Drawing.Size(206, 516);
            this.tabPageDB.TabIndex = 0;
            this.tabPageDB.Text = DACrux.SP.Common.MultiLang.SelectLang["Database"];
            this.tabPageDB.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(206, 516);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pnlTableList
            // 
            this.pnlTableList.Controls.Add(this.lvTable);
            this.pnlTableList.Controls.Add(this.uclTitle1);
            this.pnlTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTableList.Location = new System.Drawing.Point(3, 3);
            this.pnlTableList.Name = "pnlTableList";
            this.pnlTableList.Size = new System.Drawing.Size(200, 510);
            this.pnlTableList.TabIndex = 1;
            // 
            // lvTable
            // 
            this.lvTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTable.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTable.HideSelection = false;
            this.lvTable.Location = new System.Drawing.Point(0, 20);
            this.lvTable.MultiSelect = false;
            this.lvTable.Name = "lvTable";
            this.lvTable.Size = new System.Drawing.Size(200, 490);
            this.lvTable.TabIndex = 22;
            this.lvTable.UseCompatibleStateImageBehavior = false;
            this.lvTable.View = System.Windows.Forms.View.Details;
            // 
            // uclTitle1
            // 
            this.uclTitle1.BackColor = System.Drawing.Color.Transparent;
            this.uclTitle1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitle1.BackgroundImage")));
            this.uclTitle1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitle1.EndColor = System.Drawing.Color.White;
            this.uclTitle1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitle1.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitle1.Location = new System.Drawing.Point(0, 0);
            this.uclTitle1.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitle1.Name = "uclTitle1";
            this.uclTitle1.Size = new System.Drawing.Size(200, 20);
            this.uclTitle1.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitle1.TabIndex = 16;
            this.uclTitle1.Title = DACrux.SP.Common.MultiLang.SelectLang["Table List"];
            // 
            // frmTableDataMatching
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 543);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmTableDataMatching";
            this.Text = "Database Table Data Matching";
            this.tabControl1.ResumeLayout(false);
            this.tabPageDB.ResumeLayout(false);
            this.pnlTableList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDB;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel pnlTableList;
        private System.Windows.Forms.ListView lvTable;
        private DACrux.SP.Controls.uclTitle uclTitle1;


    }
}