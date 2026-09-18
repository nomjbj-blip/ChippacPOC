namespace DACrux.Framework
{
    partial class frmFunctionManagement
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFunctionManagement));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.PictureBox();
            this.btnUpdate = new System.Windows.Forms.PictureBox();
            this.btnInsert = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.PictureBox();
            this.pnlFunction = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.PictureBox();
            this.txtHelpUrl = new System.Windows.Forms.TextBox();
            this.txtFunctionName = new System.Windows.Forms.TextBox();
            this.txtFunctionCode = new System.Windows.Forms.TextBox();
            this.lstFunction = new System.Windows.Forms.ListBox();
            this.lblCurrFuncList = new System.Windows.Forms.Label();
            this.lblHelpUrl = new System.Windows.Forms.Label();
            this.lblFunctionName = new System.Windows.Forms.Label();
            this.lblFuctionCode = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInsert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).BeginInit();
            this.pnlFunction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnReset)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(101)))), ((int)(((byte)(126)))));
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Controls.Add(this.btnDelete);
            this.pnlTop.Controls.Add(this.btnUpdate);
            this.pnlTop.Controls.Add(this.btnInsert);
            this.pnlTop.Controls.Add(this.btnSave);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(866, 32);
            this.pnlTop.TabIndex = 121;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(329, 32);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "   ● Function Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(786, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 22);
            this.btnClose.TabIndex = 63;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(710, 5);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 22);
            this.btnDelete.TabIndex = 62;
            this.btnDelete.TabStop = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.Location = new System.Drawing.Point(635, 5);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(70, 22);
            this.btnUpdate.TabIndex = 64;
            this.btnUpdate.TabStop = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInsert.Image = ((System.Drawing.Image)(resources.GetObject("btnInsert.Image")));
            this.btnInsert.Location = new System.Drawing.Point(559, 5);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(70, 22);
            this.btnInsert.TabIndex = 65;
            this.btnInsert.TabStop = false;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Enabled = false;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(483, 5);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 22);
            this.btnSave.TabIndex = 66;
            this.btnSave.TabStop = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlFunction
            // 
            this.pnlFunction.BackColor = System.Drawing.Color.White;
            this.pnlFunction.Controls.Add(this.lblCurrFuncList);
            this.pnlFunction.Controls.Add(this.lblHelpUrl);
            this.pnlFunction.Controls.Add(this.lblFunctionName);
            this.pnlFunction.Controls.Add(this.lblFuctionCode);
            this.pnlFunction.Controls.Add(this.btnReset);
            this.pnlFunction.Controls.Add(this.txtHelpUrl);
            this.pnlFunction.Controls.Add(this.txtFunctionName);
            this.pnlFunction.Controls.Add(this.txtFunctionCode);
            this.pnlFunction.Controls.Add(this.lstFunction);
            this.pnlFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFunction.Location = new System.Drawing.Point(0, 32);
            this.pnlFunction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlFunction.Name = "pnlFunction";
            this.pnlFunction.Size = new System.Drawing.Size(866, 477);
            this.pnlFunction.TabIndex = 122;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.Enabled = false;
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.Location = new System.Drawing.Point(784, 47);
            this.btnReset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(70, 22);
            this.btnReset.TabIndex = 118;
            this.btnReset.TabStop = false;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // txtHelpUrl
            // 
            this.txtHelpUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHelpUrl.BackColor = System.Drawing.Color.PowderBlue;
            this.txtHelpUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHelpUrl.Location = new System.Drawing.Point(512, 147);
            this.txtHelpUrl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHelpUrl.Multiline = true;
            this.txtHelpUrl.Name = "txtHelpUrl";
            this.txtHelpUrl.ReadOnly = true;
            this.txtHelpUrl.Size = new System.Drawing.Size(342, 89);
            this.txtHelpUrl.TabIndex = 74;
            // 
            // txtFunctionName
            // 
            this.txtFunctionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFunctionName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtFunctionName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFunctionName.Location = new System.Drawing.Point(512, 97);
            this.txtFunctionName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFunctionName.Name = "txtFunctionName";
            this.txtFunctionName.ReadOnly = true;
            this.txtFunctionName.Size = new System.Drawing.Size(342, 22);
            this.txtFunctionName.TabIndex = 73;
            // 
            // txtFunctionCode
            // 
            this.txtFunctionCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtFunctionCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFunctionCode.Location = new System.Drawing.Point(512, 48);
            this.txtFunctionCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFunctionCode.Name = "txtFunctionCode";
            this.txtFunctionCode.ReadOnly = true;
            this.txtFunctionCode.Size = new System.Drawing.Size(99, 22);
            this.txtFunctionCode.TabIndex = 72;
            // 
            // lstFunction
            // 
            this.lstFunction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstFunction.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstFunction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstFunction.FormattingEnabled = true;
            this.lstFunction.ItemHeight = 14;
            this.lstFunction.Location = new System.Drawing.Point(16, 30);
            this.lstFunction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstFunction.Name = "lstFunction";
            this.lstFunction.Size = new System.Drawing.Size(485, 436);
            this.lstFunction.TabIndex = 71;
            this.lstFunction.SelectedIndexChanged += new System.EventHandler(this.lstFunction_SelectedIndexChanged);
            // 
            // lblCurrFuncList
            // 
            this.lblCurrFuncList.AutoSize = true;
            this.lblCurrFuncList.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrFuncList.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrFuncList.Image = ((System.Drawing.Image)(resources.GetObject("lblCurrFuncList.Image")));
            this.lblCurrFuncList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrFuncList.Location = new System.Drawing.Point(14, 12);
            this.lblCurrFuncList.Name = "lblCurrFuncList";
            this.lblCurrFuncList.Size = new System.Drawing.Size(141, 14);
            this.lblCurrFuncList.TabIndex = 125;
            this.lblCurrFuncList.Text = "     Current Function List";
            // 
            // lblHelpUrl
            // 
            this.lblHelpUrl.AutoSize = true;
            this.lblHelpUrl.BackColor = System.Drawing.Color.Transparent;
            this.lblHelpUrl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelpUrl.Image = ((System.Drawing.Image)(resources.GetObject("lblHelpUrl.Image")));
            this.lblHelpUrl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblHelpUrl.Location = new System.Drawing.Point(510, 129);
            this.lblHelpUrl.Name = "lblHelpUrl";
            this.lblHelpUrl.Size = new System.Drawing.Size(69, 14);
            this.lblHelpUrl.TabIndex = 124;
            this.lblHelpUrl.Text = "     Help Url";
            // 
            // lblFunctionName
            // 
            this.lblFunctionName.AutoSize = true;
            this.lblFunctionName.BackColor = System.Drawing.Color.Transparent;
            this.lblFunctionName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFunctionName.Image = ((System.Drawing.Image)(resources.GetObject("lblFunctionName.Image")));
            this.lblFunctionName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFunctionName.Location = new System.Drawing.Point(510, 79);
            this.lblFunctionName.Name = "lblFunctionName";
            this.lblFunctionName.Size = new System.Drawing.Size(109, 14);
            this.lblFunctionName.TabIndex = 123;
            this.lblFunctionName.Text = "     Function Name";
            // 
            // lblFuctionCode
            // 
            this.lblFuctionCode.AutoSize = true;
            this.lblFuctionCode.BackColor = System.Drawing.Color.Transparent;
            this.lblFuctionCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFuctionCode.Image = ((System.Drawing.Image)(resources.GetObject("lblFuctionCode.Image")));
            this.lblFuctionCode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFuctionCode.Location = new System.Drawing.Point(510, 30);
            this.lblFuctionCode.Name = "lblFuctionCode";
            this.lblFuctionCode.Size = new System.Drawing.Size(106, 14);
            this.lblFuctionCode.TabIndex = 122;
            this.lblFuctionCode.Text = "     Function Code";
            // 
            // frmFunctionManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 509);
            this.Controls.Add(this.pnlFunction);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmFunctionManagement";
            this.Text = "Function Management";
            this.Load += new System.EventHandler(this.frmFunctionManagement_Load);
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInsert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).EndInit();
            this.pnlFunction.ResumeLayout(false);
            this.pnlFunction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnReset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox btnDelete;
        private System.Windows.Forms.PictureBox btnUpdate;
        private System.Windows.Forms.PictureBox btnInsert;
        private System.Windows.Forms.PictureBox btnSave;
        private System.Windows.Forms.Panel pnlFunction;
        private System.Windows.Forms.TextBox txtHelpUrl;
        private System.Windows.Forms.TextBox txtFunctionName;
        private System.Windows.Forms.TextBox txtFunctionCode;
        private System.Windows.Forms.ListBox lstFunction;
        private System.Windows.Forms.PictureBox btnReset;
        private System.Windows.Forms.Label lblHelpUrl;
        private System.Windows.Forms.Label lblFunctionName;
        private System.Windows.Forms.Label lblFuctionCode;
        private System.Windows.Forms.Label lblCurrFuncList;



    }
}