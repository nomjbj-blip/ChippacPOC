namespace DACrux.Framework.Controls
{
    partial class DUCItemSelector
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DUCItemSelector));
            this.txtSelect = new System.Windows.Forms.TextBox();
            this.butSearch = new System.Windows.Forms.Button();
            this.lbl_title = new System.Windows.Forms.Label();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSelect
            // 
            this.txtSelect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSelect.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSelect.Location = new System.Drawing.Point(0, 0);
            this.txtSelect.Multiline = true;
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.Size = new System.Drawing.Size(177, 19);
            this.txtSelect.TabIndex = 0;
            this.txtSelect.WordWrap = false;
            this.txtSelect.TextChanged += new System.EventHandler(this.txtSelect_TextChanged);
            this.txtSelect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSelect_KeyPress);
            // 
            // butSearch
            // 
            this.butSearch.BackColor = System.Drawing.SystemColors.Window;
            this.butSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butSearch.BackgroundImage")));
            this.butSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.butSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.butSearch.FlatAppearance.BorderSize = 0;
            this.butSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butSearch.ForeColor = System.Drawing.Color.White;
            this.butSearch.Location = new System.Drawing.Point(177, 0);
            this.butSearch.Margin = new System.Windows.Forms.Padding(0);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(19, 19);
            this.butSearch.TabIndex = 1;
            this.butSearch.TabStop = false;
            this.butSearch.UseVisualStyleBackColor = false;
            this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
            // 
            // lbl_title
            // 
            this.lbl_title.BackColor = System.Drawing.Color.Transparent;
            this.lbl_title.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbl_title.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.Image = global::DACrux.Framework.Controls.Properties.Resources.StopHS;
            this.lbl_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_title.Location = new System.Drawing.Point(0, 0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(78, 21);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "     Sample";
            this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.txtSelect);
            this.pnlSearch.Controls.Add(this.butSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(78, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(198, 21);
            this.pnlSearch.TabIndex = 2;
            // 
            // DUCItemSelector
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.lbl_title);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DUCItemSelector";
            this.Size = new System.Drawing.Size(276, 21);
            this.Load += new System.EventHandler(this.DUCItemSelector_Load);
            this.VisibleChanged += new System.EventHandler(this.DUCItemSelector_VisibleChanged);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSelect;
        private System.Windows.Forms.Button butSearch;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Panel pnlSearch;
    }
}
