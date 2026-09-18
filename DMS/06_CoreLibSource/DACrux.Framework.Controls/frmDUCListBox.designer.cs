namespace DACrux.Framework.Controls
{
    partial class frmDUCListBox
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "rrr",
            "a",
            "b"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("ggg");
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.butClear = new System.Windows.Forms.Button();
            this.butChecked = new System.Windows.Forms.Button();
            this.butInvert = new System.Windows.Forms.Button();
            this.butAll = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.pnlBack = new System.Windows.Forms.Panel();
            this.tBar = new System.Windows.Forms.TrackBar();
            this.butSearch = new System.Windows.Forms.Button();
            this.lvList = new System.Windows.Forms.ListView();
            this.pnlTop.SuspendLayout();
            this.pnlBack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBar)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.Teal;
            this.txtSearch.Location = new System.Drawing.Point(-1, 31);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(208, 21);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.WordWrap = false;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(2, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(121, 22);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.butClear);
            this.pnlTop.Controls.Add(this.butChecked);
            this.pnlTop.Controls.Add(this.butInvert);
            this.pnlTop.Controls.Add(this.butAll);
            this.pnlTop.Controls.Add(this.butCancel);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(2, 2);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(229, 27);
            this.pnlTop.TabIndex = 0;
            // 
            // butClear
            // 
            this.butClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butClear.BackColor = System.Drawing.Color.White;
            this.butClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butClear.Image = global::DACrux.Framework.Controls.Properties.Resources.Erase;
            this.butClear.Location = new System.Drawing.Point(189, 3);
            this.butClear.Margin = new System.Windows.Forms.Padding(0);
            this.butClear.Name = "butClear";
            this.butClear.Size = new System.Drawing.Size(20, 20);
            this.butClear.TabIndex = 1;
            this.butClear.UseVisualStyleBackColor = false;
            this.butClear.Click += new System.EventHandler(this.butClear_Click);
            // 
            // butChecked
            // 
            this.butChecked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butChecked.BackColor = System.Drawing.Color.White;
            this.butChecked.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butChecked.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butChecked.Image = global::DACrux.Framework.Controls.Properties.Resources.CheckGrammar;
            this.butChecked.Location = new System.Drawing.Point(124, 3);
            this.butChecked.Margin = new System.Windows.Forms.Padding(0);
            this.butChecked.Name = "butChecked";
            this.butChecked.Size = new System.Drawing.Size(20, 20);
            this.butChecked.TabIndex = 1;
            this.butChecked.UseVisualStyleBackColor = false;
            this.butChecked.Click += new System.EventHandler(this.butChecked_Click);
            // 
            // butInvert
            // 
            this.butInvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butInvert.BackColor = System.Drawing.Color.White;
            this.butInvert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butInvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butInvert.Image = global::DACrux.Framework.Controls.Properties.Resources._2_6_13_Check_Type_DefinitionGroup1_16;
            this.butInvert.Location = new System.Drawing.Point(145, 3);
            this.butInvert.Margin = new System.Windows.Forms.Padding(0);
            this.butInvert.Name = "butInvert";
            this.butInvert.Size = new System.Drawing.Size(20, 20);
            this.butInvert.TabIndex = 2;
            this.butInvert.UseVisualStyleBackColor = false;
            this.butInvert.Click += new System.EventHandler(this.butInvert_Click);
            // 
            // butAll
            // 
            this.butAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butAll.BackColor = System.Drawing.Color.White;
            this.butAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butAll.Image = global::DACrux.Framework.Controls.Properties.Resources.CheckBoxHS;
            this.butAll.Location = new System.Drawing.Point(166, 3);
            this.butAll.Margin = new System.Windows.Forms.Padding(0);
            this.butAll.Name = "butAll";
            this.butAll.Size = new System.Drawing.Size(20, 20);
            this.butAll.TabIndex = 3;
            this.butAll.UseVisualStyleBackColor = false;
            this.butAll.Click += new System.EventHandler(this.butAll_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.BackColor = System.Drawing.Color.White;
            this.butCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butCancel.Image = global::DACrux.Framework.Controls.Properties.Resources.Close;
            this.butCancel.Location = new System.Drawing.Point(209, 3);
            this.butCancel.Margin = new System.Windows.Forms.Padding(0);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(20, 20);
            this.butCancel.TabIndex = 4;
            this.butCancel.UseVisualStyleBackColor = false;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // pnlBack
            // 
            this.pnlBack.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBack.Controls.Add(this.tBar);
            this.pnlBack.Controls.Add(this.pnlTop);
            this.pnlBack.Controls.Add(this.txtSearch);
            this.pnlBack.Controls.Add(this.butSearch);
            this.pnlBack.Controls.Add(this.lvList);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Location = new System.Drawing.Point(0, 0);
            this.pnlBack.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Padding = new System.Windows.Forms.Padding(2);
            this.pnlBack.Size = new System.Drawing.Size(235, 225);
            this.pnlBack.TabIndex = 0;
            // 
            // tBar
            // 
            this.tBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tBar.AutoSize = false;
            this.tBar.BackColor = System.Drawing.SystemColors.ControlText;
            this.tBar.Location = new System.Drawing.Point(2, 157);
            this.tBar.Maximum = 100;
            this.tBar.Minimum = 30;
            this.tBar.Name = "tBar";
            this.tBar.Size = new System.Drawing.Size(229, 22);
            this.tBar.SmallChange = 10;
            this.tBar.TabIndex = 3;
            this.tBar.TickFrequency = 10;
            this.tBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tBar.Value = 100;
            this.tBar.Visible = false;
            this.tBar.Scroll += new System.EventHandler(this.tBar_Scroll);
            // 
            // butSearch
            // 
            this.butSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSearch.BackColor = System.Drawing.Color.White;
            this.butSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butSearch.Image = global::DACrux.Framework.Controls.Properties.Resources.Filter;
            this.butSearch.Location = new System.Drawing.Point(211, 30);
            this.butSearch.Margin = new System.Windows.Forms.Padding(0);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(20, 21);
            this.butSearch.TabIndex = 2;
            this.butSearch.UseVisualStyleBackColor = false;
            this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
            // 
            // lvList
            // 
            this.lvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvList.FullRowSelect = true;
            this.lvList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvList.HideSelection = false;
            this.lvList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lvList.Location = new System.Drawing.Point(2, 53);
            this.lvList.Name = "lvList";
            this.lvList.Size = new System.Drawing.Size(229, 168);
            this.lvList.TabIndex = 4;
            this.lvList.UseCompatibleStateImageBehavior = false;
            this.lvList.View = System.Windows.Forms.View.Details;
            this.lvList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvList_MouseClick);
            this.lvList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvList_MouseDoubleClick);
            this.lvList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvList_MouseUp);
            // 
            // frmDUCListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(235, 225);
            this.Controls.Add(this.pnlBack);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmDUCListBox";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.frmDUCListBox_Activated);
            this.Deactivate += new System.EventHandler(this.frmDUCListBox_Deactivate);
            this.Shown += new System.EventHandler(this.frmDUCListBox_Shown);
            this.pnlTop.ResumeLayout(false);
            this.pnlBack.ResumeLayout(false);
            this.pnlBack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBack;
        private System.Windows.Forms.Button butAll;
        private System.Windows.Forms.Button butChecked;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butInvert;
        private System.Windows.Forms.Button butSearch;
        private System.Windows.Forms.TrackBar tBar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.ListView lvList;
        private System.Windows.Forms.Button butClear;
    }
}
