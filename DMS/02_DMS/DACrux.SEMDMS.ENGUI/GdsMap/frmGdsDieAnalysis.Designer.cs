namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>DACrux MDI 화면에서 Die 상세 분석 Form(GDS EXE)을 표시할 영역을 디자이너에 정의한다.</summary>
    partial class frmGdsDieAnalysis
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlGdsMap;
        private System.Windows.Forms.Label lblShared;

        /// <summary>호스트 화면이 닫힐 때 공유 GDS 상세 Form 은 먼저 떼어 두고(다른 상세보기 창이 계속 쓴다, 2026-10-16) 컨트롤을 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing) Detach();
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>GDS 상세 Form 이 DACrux 작업 영역을 채우도록 Panel 을 배치한다. 공유 화면이 다른 창에 있을 때는 안내 문구를 보인다.</summary>
        private void InitializeComponent()
        {
            this.pnlGdsMap = new System.Windows.Forms.Panel();
            this.lblShared = new System.Windows.Forms.Label();
            this.pnlGdsMap.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlGdsMap
            //
            this.pnlGdsMap.Controls.Add(this.lblShared);
            this.pnlGdsMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGdsMap.Location = new System.Drawing.Point(0, 0);
            this.pnlGdsMap.Name = "pnlGdsMap";
            this.pnlGdsMap.Size = new System.Drawing.Size(1200, 800);
            this.pnlGdsMap.TabIndex = 0;
            //
            // lblShared
            //
            this.lblShared.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShared.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblShared.Location = new System.Drawing.Point(0, 0);
            this.lblShared.Name = "lblShared";
            this.lblShared.Size = new System.Drawing.Size(1200, 800);
            this.lblShared.TabIndex = 0;
            this.lblShared.Text = "다른 상세보기 창에 도면을 표시하고 있습니다.\r\n이 창을 누르면 여기에 표시합니다.";
            this.lblShared.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblShared.Visible = false;
            this.lblShared.Click += new System.EventHandler(this.lblShared_Click);
            //
            // frmGdsDieAnalysis
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.pnlGdsMap);
            this.Name = "frmGdsDieAnalysis";
            this.Text = "상세보기";
            this.pnlGdsMap.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
