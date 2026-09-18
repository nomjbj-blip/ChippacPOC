namespace DACrux.SEMDMS.ENGUI
{
    partial class frmCommonalityLotList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCommonalityLotList));
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.BtnSearchLotList = new System.Windows.Forms.Button();
            this.dlbRECIPE = new DACrux.Framework.Controls.DUCListBox();
            this.dlbFLOW = new DACrux.Framework.Controls.DUCListBox();
            this.dlbOPER = new DACrux.Framework.Controls.DUCListBox();
            this.dlbResID = new DACrux.Framework.Controls.DUCListBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lsLotCount = new System.Windows.Forms.Label();
            this.dlbModel = new DACrux.Framework.Controls.DUCListBox();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(166, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "~ To";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "From";
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy-MM-dd";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(50, 9);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(105, 21);
            this.dtpStart.TabIndex = 6;
            this.dtpStart.Value = new System.DateTime(2019, 11, 1, 11, 53, 20, 0);
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy-MM-dd";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(211, 9);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(105, 21);
            this.dtpEnd.TabIndex = 7;
            this.dtpEnd.Value = new System.DateTime(2019, 11, 1, 11, 53, 26, 0);
            // 
            // BtnSearchLotList
            // 
            this.BtnSearchLotList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSearchLotList.Image = ((System.Drawing.Image)(resources.GetObject("BtnSearchLotList.Image")));
            this.BtnSearchLotList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSearchLotList.Location = new System.Drawing.Point(323, 8);
            this.BtnSearchLotList.Name = "BtnSearchLotList";
            this.BtnSearchLotList.Size = new System.Drawing.Size(97, 22);
            this.BtnSearchLotList.TabIndex = 10;
            this.BtnSearchLotList.Text = "    Query";
            this.BtnSearchLotList.UseVisualStyleBackColor = true;
            this.BtnSearchLotList.Click += new System.EventHandler(this.BtnSearchLotList_Click);
            // 
            // dlbRECIPE
            // 
            this.dlbRECIPE.AutoScroll = true;
            this.dlbRECIPE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dlbRECIPE.DataSource = null;
            this.dlbRECIPE.DisplayMember = "";
            this.dlbRECIPE.Location = new System.Drawing.Point(290, 49);
            this.dlbRECIPE.Name = "dlbRECIPE";
            this.dlbRECIPE.SearchText = "";
            this.dlbRECIPE.SearchTitle = "Recipe";
            this.dlbRECIPE.SelectedIndex = -1;
            this.dlbRECIPE.SelectedItem = null;
            this.dlbRECIPE.SelectedValue = null;
            this.dlbRECIPE.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbRECIPE.Size = new System.Drawing.Size(142, 159);
            this.dlbRECIPE.TabIndex = 104;
            this.dlbRECIPE.ValueMember = "";
            this.dlbRECIPE.OnSelectedIndexChanged += new System.EventHandler(this.dlbRECIPE_OnSelectedIndexChanged);
            // 
            // dlbFLOW
            // 
            this.dlbFLOW.AutoScroll = true;
            this.dlbFLOW.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dlbFLOW.DataSource = null;
            this.dlbFLOW.DisplayMember = "";
            this.dlbFLOW.Location = new System.Drawing.Point(2, 49);
            this.dlbFLOW.Name = "dlbFLOW";
            this.dlbFLOW.SearchText = "";
            this.dlbFLOW.SearchTitle = "Flow";
            this.dlbFLOW.SelectedIndex = -1;
            this.dlbFLOW.SelectedItem = null;
            this.dlbFLOW.SelectedValue = null;
            this.dlbFLOW.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbFLOW.Size = new System.Drawing.Size(142, 159);
            this.dlbFLOW.TabIndex = 104;
            this.dlbFLOW.ValueMember = "";
            this.dlbFLOW.OnSelectedIndexChanged += new System.EventHandler(this.dlbFLOW_OnSelectedIndexChanged);
            // 
            // dlbOPER
            // 
            this.dlbOPER.AutoScroll = true;
            this.dlbOPER.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dlbOPER.DataSource = null;
            this.dlbOPER.DisplayMember = "";
            this.dlbOPER.Location = new System.Drawing.Point(146, 49);
            this.dlbOPER.Name = "dlbOPER";
            this.dlbOPER.SearchText = "";
            this.dlbOPER.SearchTitle = "Oper";
            this.dlbOPER.SelectedIndex = -1;
            this.dlbOPER.SelectedItem = null;
            this.dlbOPER.SelectedValue = null;
            this.dlbOPER.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbOPER.Size = new System.Drawing.Size(142, 159);
            this.dlbOPER.TabIndex = 104;
            this.dlbOPER.ValueMember = "";
            this.dlbOPER.OnSelectedIndexChanged += new System.EventHandler(this.dlbOPER_OnSelectedIndexChanged);
            // 
            // dlbResID
            // 
            this.dlbResID.AutoScroll = true;
            this.dlbResID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dlbResID.DataSource = null;
            this.dlbResID.DisplayMember = "";
            this.dlbResID.Location = new System.Drawing.Point(578, 49);
            this.dlbResID.Name = "dlbResID";
            this.dlbResID.SearchText = "";
            this.dlbResID.SearchTitle = "Res ID";
            this.dlbResID.SelectedIndex = -1;
            this.dlbResID.SelectedItem = null;
            this.dlbResID.SelectedValue = null;
            this.dlbResID.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbResID.Size = new System.Drawing.Size(142, 159);
            this.dlbResID.TabIndex = 104;
            this.dlbResID.ValueMember = "";
            this.dlbResID.OnSelectedIndexChanged += new System.EventHandler(this.dlbResID_OnSelectedIndexChanged);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApply.Location = new System.Drawing.Point(519, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(97, 22);
            this.btnApply.TabIndex = 10;
            this.btnApply.Text = "    Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(622, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 22);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "    Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lsLotCount
            // 
            this.lsLotCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lsLotCount.AutoSize = true;
            this.lsLotCount.Location = new System.Drawing.Point(522, 32);
            this.lsLotCount.Name = "lsLotCount";
            this.lsLotCount.Size = new System.Drawing.Size(96, 12);
            this.lsLotCount.TabIndex = 105;
            this.lsLotCount.Text = "Total Lot List : 0";
            // 
            // dlbModel
            // 
            this.dlbModel.AutoScroll = true;
            this.dlbModel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dlbModel.DataSource = null;
            this.dlbModel.DisplayMember = "";
            this.dlbModel.Location = new System.Drawing.Point(434, 49);
            this.dlbModel.Name = "dlbModel";
            this.dlbModel.SearchText = "";
            this.dlbModel.SearchTitle = "Res Model";
            this.dlbModel.SelectedIndex = -1;
            this.dlbModel.SelectedItem = null;
            this.dlbModel.SelectedValue = null;
            this.dlbModel.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbModel.Size = new System.Drawing.Size(142, 159);
            this.dlbModel.TabIndex = 104;
            this.dlbModel.ValueMember = "";
            this.dlbModel.OnSelectedIndexChanged += new System.EventHandler(this.dlbModel_OnSelectedIndexChanged);
            // 
            // frmCommonalityLotList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 209);
            this.Controls.Add(this.lsLotCount);
            this.Controls.Add(this.dlbOPER);
            this.Controls.Add(this.dlbFLOW);
            this.Controls.Add(this.dlbModel);
            this.Controls.Add(this.dlbResID);
            this.Controls.Add(this.dlbRECIPE);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.BtnSearchLotList);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.dtpEnd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(729, 233);
            this.MinimumSize = new System.Drawing.Size(729, 233);
            this.Name = "frmCommonalityLotList";
            this.Text = "Lot List";
            this.Load += new System.EventHandler(this.frmCommonalityLotList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button BtnSearchLotList;
        private Framework.Controls.DUCListBox dlbRECIPE;
        private Framework.Controls.DUCListBox dlbFLOW;
        private Framework.Controls.DUCListBox dlbOPER;
        private Framework.Controls.DUCListBox dlbResID;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lsLotCount;
        private Framework.Controls.DUCListBox dlbModel;
    }
}