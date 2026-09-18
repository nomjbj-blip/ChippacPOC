namespace DACrux.Framework
{
    partial class frmGroupManagement
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroupManagement));
            this.pnlGroup = new System.Windows.Forms.Panel();
            this.lblGroupDesc = new System.Windows.Forms.Label();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.lblGroupCode = new System.Windows.Forms.Label();
            this.lblCurrentGroup = new System.Windows.Forms.Label();
            this.txtGrpCaption = new System.Windows.Forms.TextBox();
            this.txtGrpName = new System.Windows.Forms.TextBox();
            this.txtGrpCode = new System.Windows.Forms.TextBox();
            this.lstGroup = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.PictureBox();
            this.btnInsert = new System.Windows.Forms.PictureBox();
            this.btnEdit = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.PictureBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlFunction = new System.Windows.Forms.Panel();
            this.lblSelectedFuncList = new System.Windows.Forms.Label();
            this.lblFunctionList = new System.Windows.Forms.Label();
            this.btnOut = new System.Windows.Forms.PictureBox();
            this.btnIn = new System.Windows.Forms.PictureBox();
            this.lstUseFunc = new System.Windows.Forms.ListBox();
            this.lstFunc = new System.Windows.Forms.ListBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlGroup.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.lblGroupDesc)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.lblGroupName)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.lblGroupCode)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.lblCurrentGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInsert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).BeginInit();
            this.pnlFunction.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.lblSelectedFuncList)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.lblFunctionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnIn)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlGroup
            // 
            this.pnlGroup.BackColor = System.Drawing.Color.White;
            this.pnlGroup.Controls.Add(this.lblGroupDesc);
            this.pnlGroup.Controls.Add(this.lblGroupName);
            this.pnlGroup.Controls.Add(this.lblGroupCode);
            this.pnlGroup.Controls.Add(this.lblCurrentGroup);
            this.pnlGroup.Controls.Add(this.txtGrpCaption);
            this.pnlGroup.Controls.Add(this.txtGrpName);
            this.pnlGroup.Controls.Add(this.txtGrpCode);
            this.pnlGroup.Controls.Add(this.lstGroup);
            this.pnlGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGroup.Location = new System.Drawing.Point(0, 32);
            this.pnlGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Size = new System.Drawing.Size(951, 210);
            this.pnlGroup.TabIndex = 6;
            // 
            // lblGroupDesc
            // 
            this.lblGroupDesc.AutoSize = true;
            this.lblGroupDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroupDesc.Image = ((System.Drawing.Image)(resources.GetObject("lblGroupDesc.Image")));
            this.lblGroupDesc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGroupDesc.Location = new System.Drawing.Point(507, 113);
            this.lblGroupDesc.Name = "lblGroupDesc";
            this.lblGroupDesc.Size = new System.Drawing.Size(124, 14);
            this.lblGroupDesc.TabIndex = 130;
            this.lblGroupDesc.Text = "     Group Description";
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            this.lblGroupName.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroupName.Image = ((System.Drawing.Image)(resources.GetObject("lblGroupName.Image")));
            this.lblGroupName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGroupName.Location = new System.Drawing.Point(507, 64);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(95, 14);
            this.lblGroupName.TabIndex = 129;
            this.lblGroupName.Text = "     Group Name";
            // 
            // lblGroupCode
            // 
            this.lblGroupCode.AutoSize = true;
            this.lblGroupCode.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroupCode.Image = ((System.Drawing.Image)(resources.GetObject("lblGroupCode.Image")));
            this.lblGroupCode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGroupCode.Location = new System.Drawing.Point(507, 12);
            this.lblGroupCode.Name = "lblGroupCode";
            this.lblGroupCode.Size = new System.Drawing.Size(92, 14);
            this.lblGroupCode.TabIndex = 128;
            this.lblGroupCode.Text = "     Group Code";
            // 
            // lblCurrentGroup
            // 
            this.lblCurrentGroup.AutoSize = true;
            this.lblCurrentGroup.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentGroup.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentGroup.Image = ((System.Drawing.Image)(resources.GetObject("lblCurrentGroup.Image")));
            this.lblCurrentGroup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrentGroup.Location = new System.Drawing.Point(14, 12);
            this.lblCurrentGroup.Name = "lblCurrentGroup";
            this.lblCurrentGroup.Size = new System.Drawing.Size(127, 14);
            this.lblCurrentGroup.TabIndex = 126;
            this.lblCurrentGroup.Text = "     Current Group List";
            // 
            // txtGrpCaption
            // 
            this.txtGrpCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGrpCaption.BackColor = System.Drawing.Color.PowderBlue;
            this.txtGrpCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrpCaption.Location = new System.Drawing.Point(509, 131);
            this.txtGrpCaption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGrpCaption.Multiline = true;
            this.txtGrpCaption.Name = "txtGrpCaption";
            this.txtGrpCaption.Size = new System.Drawing.Size(430, 69);
            this.txtGrpCaption.TabIndex = 74;
            // 
            // txtGrpName
            // 
            this.txtGrpName.BackColor = System.Drawing.Color.PowderBlue;
            this.txtGrpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrpName.Location = new System.Drawing.Point(509, 82);
            this.txtGrpName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGrpName.Name = "txtGrpName";
            this.txtGrpName.Size = new System.Drawing.Size(205, 22);
            this.txtGrpName.TabIndex = 73;
            // 
            // txtGrpCode
            // 
            this.txtGrpCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtGrpCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrpCode.Location = new System.Drawing.Point(509, 30);
            this.txtGrpCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGrpCode.Name = "txtGrpCode";
            this.txtGrpCode.ReadOnly = true;
            this.txtGrpCode.Size = new System.Drawing.Size(88, 22);
            this.txtGrpCode.TabIndex = 72;
            // 
            // lstGroup
            // 
            this.lstGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstGroup.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstGroup.FormattingEnabled = true;
            this.lstGroup.ItemHeight = 14;
            this.lstGroup.Location = new System.Drawing.Point(16, 30);
            this.lstGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstGroup.Name = "lstGroup";
            this.lstGroup.Size = new System.Drawing.Size(485, 170);
            this.lstGroup.TabIndex = 0;
            this.lstGroup.SelectedIndexChanged += new System.EventHandler(this.lstGroup_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(569, 5);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 22);
            this.btnSave.TabIndex = 66;
            this.btnSave.TabStop = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInsert.Image = ((System.Drawing.Image)(resources.GetObject("btnInsert.Image")));
            this.btnInsert.Location = new System.Drawing.Point(645, 5);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(70, 22);
            this.btnInsert.TabIndex = 65;
            this.btnInsert.TabStop = false;
            this.btnInsert.Click += new System.EventHandler(this.brnInsert_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.Location = new System.Drawing.Point(721, 5);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(70, 22);
            this.btnEdit.TabIndex = 64;
            this.btnEdit.TabStop = false;
            this.btnEdit.Click += new System.EventHandler(this.brnEdit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(873, 5);
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
            this.btnDelete.Location = new System.Drawing.Point(797, 5);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 22);
            this.btnDelete.TabIndex = 62;
            this.btnDelete.TabStop = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 242);
            this.splitter1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(951, 8);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // pnlFunction
            // 
            this.pnlFunction.BackColor = System.Drawing.Color.White;
            this.pnlFunction.Controls.Add(this.lblSelectedFuncList);
            this.pnlFunction.Controls.Add(this.lblFunctionList);
            this.pnlFunction.Controls.Add(this.btnOut);
            this.pnlFunction.Controls.Add(this.btnIn);
            this.pnlFunction.Controls.Add(this.lstUseFunc);
            this.pnlFunction.Controls.Add(this.lstFunc);
            this.pnlFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFunction.Location = new System.Drawing.Point(0, 250);
            this.pnlFunction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlFunction.Name = "pnlFunction";
            this.pnlFunction.Size = new System.Drawing.Size(951, 204);
            this.pnlFunction.TabIndex = 2;
            // 
            // lblSelectedFuncList
            // 
            this.lblSelectedFuncList.AutoSize = true;
            this.lblSelectedFuncList.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectedFuncList.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedFuncList.Image = ((System.Drawing.Image)(resources.GetObject("lblSelectedFuncList.Image")));
            this.lblSelectedFuncList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSelectedFuncList.Location = new System.Drawing.Point(507, 16);
            this.lblSelectedFuncList.Name = "lblSelectedFuncList";
            this.lblSelectedFuncList.Size = new System.Drawing.Size(148, 14);
            this.lblSelectedFuncList.TabIndex = 131;
            this.lblSelectedFuncList.Text = "     Selected Function List";
            // 
            // lblFunctionList
            // 
            this.lblFunctionList.AutoSize = true;
            this.lblFunctionList.BackColor = System.Drawing.Color.Transparent;
            this.lblFunctionList.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFunctionList.Image = ((System.Drawing.Image)(resources.GetObject("lblFunctionList.Image")));
            this.lblFunctionList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFunctionList.Location = new System.Drawing.Point(14, 16);
            this.lblFunctionList.Name = "lblFunctionList";
            this.lblFunctionList.Size = new System.Drawing.Size(146, 14);
            this.lblFunctionList.TabIndex = 127;
            this.lblFunctionList.Text = "     AvaiLabel Function List";
            // 
            // btnOut
            // 
            this.btnOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOut.Image = ((System.Drawing.Image)(resources.GetObject("btnOut.Image")));
            this.btnOut.Location = new System.Drawing.Point(448, 6);
            this.btnOut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(24, 24);
            this.btnOut.TabIndex = 79;
            this.btnOut.TabStop = false;
            this.btnOut.Visible = false;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // btnIn
            // 
            this.btnIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIn.Image = ((System.Drawing.Image)(resources.GetObject("btnIn.Image")));
            this.btnIn.Location = new System.Drawing.Point(477, 6);
            this.btnIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(24, 24);
            this.btnIn.TabIndex = 78;
            this.btnIn.TabStop = false;
            this.btnIn.Visible = false;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // lstUseFunc
            // 
            this.lstUseFunc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstUseFunc.BackColor = System.Drawing.Color.PowderBlue;
            this.lstUseFunc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstUseFunc.FormattingEnabled = true;
            this.lstUseFunc.ItemHeight = 14;
            this.lstUseFunc.Location = new System.Drawing.Point(509, 34);
            this.lstUseFunc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstUseFunc.Name = "lstUseFunc";
            this.lstUseFunc.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstUseFunc.Size = new System.Drawing.Size(430, 156);
            this.lstUseFunc.TabIndex = 76;
            // 
            // lstFunc
            // 
            this.lstFunc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstFunc.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstFunc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstFunc.FormattingEnabled = true;
            this.lstFunc.ItemHeight = 14;
            this.lstFunc.Location = new System.Drawing.Point(16, 34);
            this.lstFunc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstFunc.Name = "lstFunc";
            this.lstFunc.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFunc.Size = new System.Drawing.Size(485, 156);
            this.lstFunc.TabIndex = 75;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(101)))), ((int)(((byte)(126)))));
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Controls.Add(this.btnDelete);
            this.pnlTop.Controls.Add(this.btnEdit);
            this.pnlTop.Controls.Add(this.btnInsert);
            this.pnlTop.Controls.Add(this.btnSave);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(951, 32);
            this.pnlTop.TabIndex = 0;
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
            this.lblTitle.Text = "  ● Security Group Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmGroupManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 454);
            this.Controls.Add(this.pnlFunction);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlGroup);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmGroupManagement";
            this.Text = "Security Group Management";
            this.Load += new System.EventHandler(this.frmGroupManagement_Load);
            this.pnlGroup.ResumeLayout(false);
            this.pnlGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInsert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).EndInit();
            this.pnlFunction.ResumeLayout(false);
            this.pnlFunction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnIn)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlGroup;
        private System.Windows.Forms.PictureBox btnInsert;
        private System.Windows.Forms.PictureBox btnEdit;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox btnDelete;
        private System.Windows.Forms.PictureBox btnSave;
        private System.Windows.Forms.TextBox txtGrpCaption;
        private System.Windows.Forms.TextBox txtGrpName;
        private System.Windows.Forms.TextBox txtGrpCode;
        private System.Windows.Forms.ListBox lstGroup;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnlFunction;
        private System.Windows.Forms.PictureBox btnOut;
        private System.Windows.Forms.PictureBox btnIn;
        private System.Windows.Forms.ListBox lstUseFunc;
        private System.Windows.Forms.ListBox lstFunc;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblGroupDesc;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.Label lblGroupCode;
        private System.Windows.Forms.Label lblCurrentGroup;
        private System.Windows.Forms.Label lblSelectedFuncList;
        private System.Windows.Forms.Label lblFunctionList;
    }
}