namespace DACrux.TEST.ENGUI
{
    partial class frmSetupBin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetupBin));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNewProgram = new System.Windows.Forms.Button();
            this.pbBinColor = new System.Windows.Forms.PictureBox();
            this.duclbProgram = new DACrux.Framework.Controls.DUCListBox();
            this.duclbTestArea = new DACrux.Framework.Controls.DUCListBox();
            this.fpsBinInfo = new FarPoint.Win.Spread.FpSpread();
            this.fpsBinInfo_Sheet = new FarPoint.Win.Spread.SheetView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBinColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsBinInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsBinInfo_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnNewProgram);
            this.panel1.Controls.Add(this.pbBinColor);
            this.panel1.Controls.Add(this.duclbProgram);
            this.panel1.Controls.Add(this.duclbTestArea);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(955, 157);
            this.panel1.TabIndex = 0;
            // 
            // btnNewProgram
            // 
            this.btnNewProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewProgram.Location = new System.Drawing.Point(762, 12);
            this.btnNewProgram.Name = "btnNewProgram";
            this.btnNewProgram.Size = new System.Drawing.Size(92, 23);
            this.btnNewProgram.TabIndex = 6;
            this.btnNewProgram.Text = "New Program";
            this.btnNewProgram.UseVisualStyleBackColor = true;
            this.btnNewProgram.Click += new System.EventHandler(this.btnNewProgram_Click);
            // 
            // pbBinColor
            // 
            this.pbBinColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbBinColor.Location = new System.Drawing.Point(762, 83);
            this.pbBinColor.Name = "pbBinColor";
            this.pbBinColor.Size = new System.Drawing.Size(190, 71);
            this.pbBinColor.TabIndex = 4;
            this.pbBinColor.TabStop = false;
            this.pbBinColor.Visible = false;
            this.pbBinColor.Click += new System.EventHandler(this.pbBinColor_Click);
            // 
            // duclbProgram
            // 
            this.duclbProgram.DataSource = null;
            this.duclbProgram.DisplayMember = "";
            this.duclbProgram.Dock = System.Windows.Forms.DockStyle.Left;
            this.duclbProgram.Location = new System.Drawing.Point(150, 0);
            this.duclbProgram.Name = "duclbProgram";
            this.duclbProgram.SearchText = "";
            this.duclbProgram.SearchTitle = "PROGRAM";
            this.duclbProgram.SelectedIndex = -1;
            this.duclbProgram.SelectedItem = null;
            this.duclbProgram.SelectedValue = null;
            this.duclbProgram.Size = new System.Drawing.Size(174, 157);
            this.duclbProgram.TabIndex = 2;
            this.duclbProgram.ValueMember = "";
            this.duclbProgram.OnSelectedIndexChanged += new System.EventHandler(this.duclbProgram_OnSelectedIndexChanged);
            // 
            // duclbTestArea
            // 
            this.duclbTestArea.DataSource = null;
            this.duclbTestArea.DisplayMember = "";
            this.duclbTestArea.Dock = System.Windows.Forms.DockStyle.Left;
            this.duclbTestArea.Location = new System.Drawing.Point(0, 0);
            this.duclbTestArea.Name = "duclbTestArea";
            this.duclbTestArea.SearchText = "";
            this.duclbTestArea.SearchTitle = "TESTAREA";
            this.duclbTestArea.SelectedIndex = -1;
            this.duclbTestArea.SelectedItem = null;
            this.duclbTestArea.SelectedValue = null;
            this.duclbTestArea.Size = new System.Drawing.Size(150, 157);
            this.duclbTestArea.TabIndex = 0;
            this.duclbTestArea.ValueMember = "";
            this.duclbTestArea.OnSelectedIndexChanged += new System.EventHandler(this.duclbTestArea_OnSelectedIndexChanged);
            // 
            // fpsBinInfo
            // 
            this.fpsBinInfo.AccessibleDescription = "";
            this.fpsBinInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsBinInfo.Location = new System.Drawing.Point(0, 157);
            this.fpsBinInfo.Name = "fpsBinInfo";
            this.fpsBinInfo.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsBinInfo_Sheet});
            this.fpsBinInfo.Size = new System.Drawing.Size(955, 362);
            this.fpsBinInfo.TabIndex = 1;
            this.fpsBinInfo.EditModeOn += new System.EventHandler(this.fpsBinInfo_EditModeOn);
            this.fpsBinInfo.EditModeOff += new System.EventHandler(this.fpsBinInfo_EditModeOff);
            this.fpsBinInfo.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpsBinInfo_SelectionChanged);
            this.fpsBinInfo.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpsBinInfo_CellClick);
            this.fpsBinInfo.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpsBinInfo_CellDoubleClick);
            this.fpsBinInfo.EditChange += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.fpsBinInfo_EditChange);
            // 
            // fpsBinInfo_Sheet
            // 
            this.fpsBinInfo_Sheet.Reset();
            fpsBinInfo_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpsBinInfo_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpsBinInfo_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.fpsBinInfo_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpsBinInfo_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(860, 42);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 24);
            this.btnSave.TabIndex = 214;
            this.btnSave.Text = "   Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(860, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(92, 24);
            this.btnSearch.TabIndex = 215;
            this.btnSearch.Text = "   Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.BackColor = System.Drawing.Color.White;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(762, 42);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(92, 24);
            this.btnCopy.TabIndex = 216;
            this.btnCopy.Text = "   Copy";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // frmSetupBin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 519);
            this.Controls.Add(this.fpsBinInfo);
            this.Controls.Add(this.panel1);
            this.Name = "frmSetupBin";
            this.Text = "Setup by Bin Info";
            this.Load += new System.EventHandler(this.frmSetupBin_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbBinColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsBinInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsBinInfo_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Framework.Controls.DUCListBox duclbTestArea;
        private Framework.Controls.DUCListBox duclbProgram;
        private FarPoint.Win.Spread.FpSpread fpsBinInfo;
        private FarPoint.Win.Spread.SheetView fpsBinInfo_Sheet;
        private System.Windows.Forms.PictureBox pbBinColor;
        private System.Windows.Forms.Button btnNewProgram;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnCopy;

    }
}