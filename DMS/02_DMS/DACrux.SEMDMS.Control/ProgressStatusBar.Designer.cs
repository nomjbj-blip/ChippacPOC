namespace DACrux.SEMDMS.Control
{
    partial class ProgressStatusBar
    {
        /// <summary>필수 디자이너 변수입니다.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>사용 중인 모든 리소스를 정리합니다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblBusy = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            //
            // lblBusy
            //
            this.lblBusy.AutoEllipsis = true;
            this.lblBusy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBusy.ForeColor = System.Drawing.Color.DimGray;
            this.lblBusy.Location = new System.Drawing.Point(0, 3);
            this.lblBusy.Name = "lblBusy";
            this.lblBusy.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblBusy.Size = new System.Drawing.Size(234, 20);
            this.lblBusy.TabIndex = 0;
            this.lblBusy.Text = "대기";
            this.lblBusy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // progress
            //
            this.progress.Dock = System.Windows.Forms.DockStyle.Right;
            this.progress.Location = new System.Drawing.Point(234, 3);
            this.progress.MarqueeAnimationSpeed = 30;
            this.progress.Maximum = 1;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(140, 20);
            this.progress.TabIndex = 1;
            this.progress.Visible = false;
            //
            // timer
            //
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            //
            // ProgressStatusBar
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lblBusy);
            this.Controls.Add(this.progress);
            this.Name = "ProgressStatusBar";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.Size = new System.Drawing.Size(380, 26);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblBusy;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Timer timer;
    }
}
