namespace DACrux.ProjectManager
{
    partial class frmWorkSheet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWorkSheet));
            this.splTop = new System.Windows.Forms.Splitter();
            this.uctlDataView = new DACrux.ProjectManager.UI.DataView();
            this.uctlAnalysisView = new DACrux.ProjectManager.UI.StatView();
            this.uctlGraphView = new DACrux.ProjectManager.UI.GraphView();
            this.pnlTop = new DACrux.ProjectManager.UI.GradientPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnGraphView = new System.Windows.Forms.Button();
            this.btnAnalysisView = new System.Windows.Forms.Button();
            this.btnDataView = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // splTop
            // 
            this.splTop.BackColor = System.Drawing.Color.OliveDrab;
            this.splTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.splTop.Location = new System.Drawing.Point(0, 36);
            this.splTop.Name = "splTop";
            this.splTop.Size = new System.Drawing.Size(907, 4);
            this.splTop.TabIndex = 3;
            this.splTop.TabStop = false;
            // 
            // uctlDataView
            // 
            this.uctlDataView.DataSource = null;
            this.uctlDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctlDataView.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uctlDataView.IsSplited = false;
            this.uctlDataView.Location = new System.Drawing.Point(0, 40);
            this.uctlDataView.Name = "uctlDataView";
            this.uctlDataView.Size = new System.Drawing.Size(907, 480);
            this.uctlDataView.TabIndex = 4;
            // 
            // uctlAnalysisView
            // 
            this.uctlAnalysisView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctlAnalysisView.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uctlAnalysisView.Location = new System.Drawing.Point(0, 40);
            this.uctlAnalysisView.Name = "uctlAnalysisView";
            this.uctlAnalysisView.Size = new System.Drawing.Size(907, 480);
            this.uctlAnalysisView.TabIndex = 5;
            // 
            // uctlGraphView
            // 
            this.uctlGraphView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctlGraphView.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uctlGraphView.Location = new System.Drawing.Point(0, 40);
            this.uctlGraphView.Name = "uctlGraphView";
            this.uctlGraphView.Size = new System.Drawing.Size(907, 480);
            this.uctlGraphView.TabIndex = 6;
            // 
            // pnlTop
            // 
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Controls.Add(this.btnGraphView);
            this.pnlTop.Controls.Add(this.btnAnalysisView);
            this.pnlTop.Controls.Add(this.btnDataView);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.EndColor = System.Drawing.Color.Transparent;
            this.pnlTop.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTop.GradientStyle = DACrux.ProjectManager.UI.GradientPanel.GradientMode.Horizontal;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(907, 36);
            this.pnlTop.StartColor = System.Drawing.Color.AliceBlue;
            this.pnlTop.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(833, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(62, 22);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "   Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseCompatibleTextRendering = true;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGraphView
            // 
            this.btnGraphView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGraphView.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnGraphView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGraphView.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnGraphView.Image = ((System.Drawing.Image)(resources.GetObject("btnGraphView.Image")));
            this.btnGraphView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGraphView.Location = new System.Drawing.Point(738, 7);
            this.btnGraphView.Name = "btnGraphView";
            this.btnGraphView.Size = new System.Drawing.Size(89, 22);
            this.btnGraphView.TabIndex = 27;
            this.btnGraphView.Text = "   Graph View";
            this.btnGraphView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGraphView.UseCompatibleTextRendering = true;
            this.btnGraphView.UseVisualStyleBackColor = true;
            this.btnGraphView.Click += new System.EventHandler(this.btnGraphView_Click);
            // 
            // btnAnalysisView
            // 
            this.btnAnalysisView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalysisView.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnAnalysisView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalysisView.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnAnalysisView.Image = ((System.Drawing.Image)(resources.GetObject("btnAnalysisView.Image")));
            this.btnAnalysisView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAnalysisView.Location = new System.Drawing.Point(634, 7);
            this.btnAnalysisView.Name = "btnAnalysisView";
            this.btnAnalysisView.Size = new System.Drawing.Size(98, 22);
            this.btnAnalysisView.TabIndex = 26;
            this.btnAnalysisView.Text = "   Analysis View";
            this.btnAnalysisView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAnalysisView.UseCompatibleTextRendering = true;
            this.btnAnalysisView.UseVisualStyleBackColor = true;
            this.btnAnalysisView.Click += new System.EventHandler(this.btnAnalysisView_Click);
            // 
            // btnDataView
            // 
            this.btnDataView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDataView.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnDataView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDataView.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnDataView.Image = ((System.Drawing.Image)(resources.GetObject("btnDataView.Image")));
            this.btnDataView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDataView.Location = new System.Drawing.Point(546, 7);
            this.btnDataView.Name = "btnDataView";
            this.btnDataView.Size = new System.Drawing.Size(82, 22);
            this.btnDataView.TabIndex = 25;
            this.btnDataView.Text = " Data View";
            this.btnDataView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDataView.UseCompatibleTextRendering = true;
            this.btnDataView.UseVisualStyleBackColor = true;
            this.btnDataView.Click += new System.EventHandler(this.btnDataView_Click);
            // 
            // frmWorkSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(907, 520);
            this.Controls.Add(this.uctlDataView);
            this.Controls.Add(this.uctlAnalysisView);
            this.Controls.Add(this.uctlGraphView);
            this.Controls.Add(this.splTop);
            this.Controls.Add(this.pnlTop);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmWorkSheet";
            this.Text = "WorkSheet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmWorkSheet_Activated);
            this.Load += new System.EventHandler(this.frmWorkSheet_Load);
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public DACrux.ProjectManager.UI.GradientPanel pnlTop;
        public System.Windows.Forms.Splitter splTop;
        private System.Windows.Forms.Button btnDataView;
        private System.Windows.Forms.Button btnAnalysisView;
        private System.Windows.Forms.Button btnGraphView;
        private System.Windows.Forms.Button btnClose;
        private DACrux.ProjectManager.UI.DataView uctlDataView;
        private DACrux.ProjectManager.UI.StatView uctlAnalysisView;
        private DACrux.ProjectManager.UI.GraphView uctlGraphView;
    }
}