namespace DACrux.SP.Controls
{
    partial class SmartListbox2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmartListbox2));
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.imlCheckbox = new System.Windows.Forms.ImageList(this.components);
            this.btnCancel = new System.Windows.Forms.PictureBox();
            this.btnChecked = new System.Windows.Forms.PictureBox();
            this.btnAllItem = new System.Windows.Forms.PictureBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlBack = new System.Windows.Forms.Panel();
            this.lvList = new DACrux.SP.Controls.SmartListView2(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnChecked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAllItem)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BackColor = System.Drawing.Color.Lavender;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.Teal;
            this.txtSearch.Location = new System.Drawing.Point(4, 30);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(191, 21);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.WordWrap = false;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Location = new System.Drawing.Point(198, 30);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(22, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.TabStop = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(6, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(141, 22);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imlCheckbox
            // 
            this.imlCheckbox.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlCheckbox.ImageStream")));
            this.imlCheckbox.TransparentColor = System.Drawing.Color.Transparent;
            this.imlCheckbox.Images.SetKeyName(0, "Unchecked");
            this.imlCheckbox.Images.SetKeyName(1, "Checked");
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(199, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(19, 20);
            this.btnCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnCancel.TabIndex = 9;
            this.btnCancel.TabStop = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnChecked
            // 
            this.btnChecked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChecked.BackColor = System.Drawing.Color.White;
            this.btnChecked.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnChecked.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChecked.Image = ((System.Drawing.Image)(resources.GetObject("btnChecked.Image")));
            this.btnChecked.Location = new System.Drawing.Point(152, 5);
            this.btnChecked.Name = "btnChecked";
            this.btnChecked.Size = new System.Drawing.Size(19, 20);
            this.btnChecked.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnChecked.TabIndex = 10;
            this.btnChecked.TabStop = false;
            this.btnChecked.Click += new System.EventHandler(this.btnChecked_Click);
            // 
            // btnAllItem
            // 
            this.btnAllItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllItem.BackColor = System.Drawing.Color.White;
            this.btnAllItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnAllItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAllItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAllItem.Image")));
            this.btnAllItem.Location = new System.Drawing.Point(176, 5);
            this.btnAllItem.Name = "btnAllItem";
            this.btnAllItem.Size = new System.Drawing.Size(19, 20);
            this.btnAllItem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnAllItem.TabIndex = 11;
            this.btnAllItem.TabStop = false;
            this.btnAllItem.Click += new System.EventHandler(this.btnAllItem_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTop.Controls.Add(this.btnChecked);
            this.pnlTop.Controls.Add(this.btnAllItem);
            this.pnlTop.Controls.Add(this.btnCancel);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(223, 30);
            this.pnlTop.TabIndex = 9;
            // 
            // pnlBack
            // 
            this.pnlBack.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBack.Controls.Add(this.pnlTop);
            this.pnlBack.Controls.Add(this.lvList);
            this.pnlBack.Controls.Add(this.txtSearch);
            this.pnlBack.Controls.Add(this.btnSearch);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Location = new System.Drawing.Point(0, 0);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Size = new System.Drawing.Size(225, 225);
            this.pnlBack.TabIndex = 10;
            // 
            // lvList
            // 
            this.lvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvList.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvList.FullRowSelect = true;
            this.lvList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvList.HideSelection = false;
            this.lvList.Location = new System.Drawing.Point(4, 57);
            this.lvList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lvList.MultiSelect = false;
            this.lvList.Name = "lvList";
            this.lvList.Size = new System.Drawing.Size(215, 162);
            this.lvList.TabIndex = 8;
            this.lvList.UseCompatibleStateImageBehavior = false;
            this.lvList.View = System.Windows.Forms.View.Details;
            // 
            // SmartListbox2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(225, 225);
            this.Controls.Add(this.pnlBack);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SmartListbox2";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Deactivate += new System.EventHandler(this.SmartListbox_Deactivate);
            this.Load += new System.EventHandler(this.SmartListbox_Load);
            this.VisibleChanged += new System.EventHandler(this.SmartListbox_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnChecked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAllItem)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlBack.ResumeLayout(false);
            this.pnlBack.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblTitle;
        private SmartListView2 lvList;
        private System.Windows.Forms.ImageList imlCheckbox;
        private System.Windows.Forms.PictureBox btnSearch;
        private System.Windows.Forms.PictureBox btnCancel;
        private System.Windows.Forms.PictureBox btnAllItem;
        private System.Windows.Forms.PictureBox btnChecked;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBack;
    }
}
