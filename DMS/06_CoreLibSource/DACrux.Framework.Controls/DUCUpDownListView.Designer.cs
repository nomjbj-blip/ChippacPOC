namespace DACrux.Framework.Controls
{
    partial class DUCUpDownListView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DUCUpDownListView));
            this.pnlControl = new System.Windows.Forms.Panel();
            this.butLast = new System.Windows.Forms.Button();
            this.butDown = new System.Windows.Forms.Button();
            this.butUp = new System.Windows.Forms.Button();
            this.butFirst = new System.Windows.Forms.Button();
            this.lstView = new System.Windows.Forms.ListView();
            this.pnlControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControl
            // 
            this.pnlControl.BackColor = System.Drawing.Color.White;
            this.pnlControl.Controls.Add(this.butLast);
            this.pnlControl.Controls.Add(this.butDown);
            this.pnlControl.Controls.Add(this.butUp);
            this.pnlControl.Controls.Add(this.butFirst);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlControl.Location = new System.Drawing.Point(206, 0);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(27, 104);
            this.pnlControl.TabIndex = 0;
            // 
            // butLast
            // 
            this.butLast.Image = ((System.Drawing.Image)(resources.GetObject("butLast.Image")));
            this.butLast.Location = new System.Drawing.Point(3, 74);
            this.butLast.Name = "butLast";
            this.butLast.Size = new System.Drawing.Size(22, 23);
            this.butLast.TabIndex = 0;
            this.butLast.UseVisualStyleBackColor = true;
            this.butLast.Click += new System.EventHandler(this.butLast_Click);
            // 
            // butDown
            // 
            this.butDown.Image = ((System.Drawing.Image)(resources.GetObject("butDown.Image")));
            this.butDown.Location = new System.Drawing.Point(3, 52);
            this.butDown.Name = "butDown";
            this.butDown.Size = new System.Drawing.Size(22, 23);
            this.butDown.TabIndex = 0;
            this.butDown.UseVisualStyleBackColor = true;
            this.butDown.Click += new System.EventHandler(this.butDown_Click);
            // 
            // butUp
            // 
            this.butUp.Image = ((System.Drawing.Image)(resources.GetObject("butUp.Image")));
            this.butUp.Location = new System.Drawing.Point(3, 25);
            this.butUp.Name = "butUp";
            this.butUp.Size = new System.Drawing.Size(22, 23);
            this.butUp.TabIndex = 0;
            this.butUp.UseVisualStyleBackColor = true;
            this.butUp.Click += new System.EventHandler(this.butUp_Click);
            // 
            // butFirst
            // 
            this.butFirst.Image = ((System.Drawing.Image)(resources.GetObject("butFirst.Image")));
            this.butFirst.Location = new System.Drawing.Point(3, 3);
            this.butFirst.Name = "butFirst";
            this.butFirst.Size = new System.Drawing.Size(22, 23);
            this.butFirst.TabIndex = 0;
            this.butFirst.UseVisualStyleBackColor = true;
            this.butFirst.Click += new System.EventHandler(this.butFirst_Click);
            // 
            // lstView
            // 
            this.lstView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstView.FullRowSelect = true;
            this.lstView.HideSelection = false;
            this.lstView.Location = new System.Drawing.Point(0, 0);
            this.lstView.MultiSelect = false;
            this.lstView.Name = "lstView";
            this.lstView.Size = new System.Drawing.Size(206, 104);
            this.lstView.TabIndex = 1;
            this.lstView.UseCompatibleStateImageBehavior = false;
            this.lstView.View = System.Windows.Forms.View.Details;
            // 
            // DUCUpDownListView
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstView);
            this.Controls.Add(this.pnlControl);
            this.Name = "DUCUpDownListView";
            this.Size = new System.Drawing.Size(233, 104);
            this.pnlControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.Button butLast;
        private System.Windows.Forms.Button butDown;
        private System.Windows.Forms.Button butUp;
        private System.Windows.Forms.Button butFirst;
        private System.Windows.Forms.ListView lstView;
    }
}
