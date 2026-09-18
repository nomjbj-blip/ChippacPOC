namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectTypeDefine_VerTwo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefectTypeDefine_VerTwo));
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTypeColor = new System.Windows.Forms.Label();
            this.picBoxTypeColor = new System.Windows.Forms.PictureBox();
            this.BtnReSearch = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnItemAdd = new System.Windows.Forms.Button();
            this.btnItemRemove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.fpsDefectType = new FarPoint.Win.Spread.FpSpread();
            this.fpsDefectType_Sheet = new FarPoint.Win.Spread.SheetView();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTypeColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectType_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblTypeColor);
            this.panel2.Controls.Add(this.picBoxTypeColor);
            this.panel2.Controls.Add(this.BtnReSearch);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnItemAdd);
            this.panel2.Controls.Add(this.btnItemRemove);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(896, 61);
            this.panel2.TabIndex = 58;
            // 
            // lblTypeColor
            // 
            this.lblTypeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTypeColor.AutoSize = true;
            this.lblTypeColor.Location = new System.Drawing.Point(603, 28);
            this.lblTypeColor.Name = "lblTypeColor";
            this.lblTypeColor.Size = new System.Drawing.Size(41, 15);
            this.lblTypeColor.TabIndex = 63;
            this.lblTypeColor.Text = "Color";
            // 
            // picBoxTypeColor
            // 
            this.picBoxTypeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picBoxTypeColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBoxTypeColor.Location = new System.Drawing.Point(659, 26);
            this.picBoxTypeColor.Margin = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.picBoxTypeColor.Name = "picBoxTypeColor";
            this.picBoxTypeColor.Size = new System.Drawing.Size(107, 23);
            this.picBoxTypeColor.TabIndex = 64;
            this.picBoxTypeColor.TabStop = false;
            this.picBoxTypeColor.Click += new System.EventHandler(this.picBoxTypeColor_Click);
            // 
            // BtnReSearch
            // 
            this.BtnReSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnReSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnReSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnReSearch.Image")));
            this.BtnReSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnReSearch.Location = new System.Drawing.Point(781, 26);
            this.BtnReSearch.Name = "BtnReSearch";
            this.BtnReSearch.Size = new System.Drawing.Size(103, 23);
            this.BtnReSearch.TabIndex = 62;
            this.BtnReSearch.Text = "Search";
            this.BtnReSearch.UseVisualStyleBackColor = true;
            this.BtnReSearch.Click += new System.EventHandler(this.BtnReSearch_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.Menu;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(284, 25);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(25, 23);
            this.btnSearch.TabIndex = 61;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Visible = false;
            this.btnSearch.Click += new System.EventHandler(this.Button_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(109, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(169, 21);
            this.txtSearch.TabIndex = 60;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Menu;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(12, 25);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(25, 23);
            this.btnSave.TabIndex = 59;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnItemAdd
            // 
            this.btnItemAdd.BackColor = System.Drawing.SystemColors.Menu;
            this.btnItemAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnItemAdd.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnItemAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnItemAdd.Image")));
            this.btnItemAdd.Location = new System.Drawing.Point(43, 25);
            this.btnItemAdd.Name = "btnItemAdd";
            this.btnItemAdd.Size = new System.Drawing.Size(29, 23);
            this.btnItemAdd.TabIndex = 58;
            this.btnItemAdd.UseVisualStyleBackColor = false;
            this.btnItemAdd.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnItemRemove
            // 
            this.btnItemRemove.BackColor = System.Drawing.SystemColors.Menu;
            this.btnItemRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnItemRemove.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnItemRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnItemRemove.Image")));
            this.btnItemRemove.Location = new System.Drawing.Point(78, 25);
            this.btnItemRemove.Name = "btnItemRemove";
            this.btnItemRemove.Size = new System.Drawing.Size(25, 23);
            this.btnItemRemove.TabIndex = 57;
            this.btnItemRemove.UseVisualStyleBackColor = false;
            this.btnItemRemove.Click += new System.EventHandler(this.Button_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSlateGray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(896, 20);
            this.label1.TabIndex = 56;
            this.label1.Text = "    Defect Type Item";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fpsDefectType
            // 
            this.fpsDefectType.AccessibleDescription = "";
            this.fpsDefectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsDefectType.Location = new System.Drawing.Point(0, 61);
            this.fpsDefectType.Name = "fpsDefectType";
            this.fpsDefectType.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsDefectType_Sheet});
            this.fpsDefectType.Size = new System.Drawing.Size(896, 466);
            this.fpsDefectType.TabIndex = 59;
            this.fpsDefectType.EditModeStarting += new FarPoint.Win.Spread.EditModeStartingEventHandler(this.fpsDefectType_EditModeStarting);
            this.fpsDefectType.EditModeOn += new System.EventHandler(this.fpsDefectType_EditModeOn);
            this.fpsDefectType.EditModeOff += new System.EventHandler(this.fpsDefectType_EditModeOff);
            this.fpsDefectType.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpsDefectType_SelectionChanged);
            this.fpsDefectType.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpsDefectType_CellClick);
            this.fpsDefectType.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpsDefectType_CellDoubleClick);
            // 
            // fpsDefectType_Sheet
            // 
            this.fpsDefectType_Sheet.Reset();
            fpsDefectType_Sheet.SheetName = "Sheet1";
            // 
            // frmDefectTypeDefine_VerTwo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 527);
            this.Controls.Add(this.fpsDefectType);
            this.Controls.Add(this.panel2);
            this.Name = "frmDefectTypeDefine_VerTwo";
            this.Text = "Define By Defect Type";
            this.Load += new System.EventHandler(this.frmDefectTypeDefine_VerTwo_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTypeColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectType_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnItemAdd;
        private System.Windows.Forms.Button btnItemRemove;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread fpsDefectType;
        private FarPoint.Win.Spread.SheetView fpsDefectType_Sheet;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button BtnReSearch;
        private System.Windows.Forms.Label lblTypeColor;
        private System.Windows.Forms.PictureBox picBoxTypeColor;
    }
}