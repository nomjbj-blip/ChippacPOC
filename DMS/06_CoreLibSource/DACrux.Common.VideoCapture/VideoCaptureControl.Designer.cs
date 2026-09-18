namespace DACrux.Common.VideoCapture
{
    partial class VideoCaptureControl
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
                if (videoSourcePlayer != null)
                {
                    Disconnect();
                    videoSourcePlayer.Dispose();
                }

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoCaptureControl));
            this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.crossLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblDisplayResolution = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblImageResolution = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblImageFormat = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblModel = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnOption = new System.Windows.Forms.ToolStripStatusLabel();
            this.picCross = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCross)).BeginInit();
            this.SuspendLayout();
            // 
            // videoSourcePlayer
            // 
            this.videoSourcePlayer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.videoSourcePlayer.ContextMenuStrip = this.contextMenuStrip1;
            this.videoSourcePlayer.ForeColor = System.Drawing.Color.DarkRed;
            this.videoSourcePlayer.Location = new System.Drawing.Point(0, 0);
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new System.Drawing.Size(10, 10);
            this.videoSourcePlayer.TabIndex = 1;
            this.videoSourcePlayer.VideoSource = null;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crossLineToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(130, 26);
            // 
            // crossLineToolStripMenuItem
            // 
            this.crossLineToolStripMenuItem.Checked = true;
            this.crossLineToolStripMenuItem.CheckOnClick = true;
            this.crossLineToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.crossLineToolStripMenuItem.Name = "crossLineToolStripMenuItem";
            this.crossLineToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.crossLineToolStripMenuItem.Text = "Cross Line";
            this.crossLineToolStripMenuItem.CheckedChanged += new System.EventHandler(this.crossLineToolStripMenuItem_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblDisplayResolution,
            this.lblImageResolution,
            this.lblImageFormat,
            this.lblModel,
            this.btnOption});
            this.statusStrip1.Location = new System.Drawing.Point(0, 240);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(320, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            // 
            // lblDisplayResolution
            // 
            this.lblDisplayResolution.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblDisplayResolution.Name = "lblDisplayResolution";
            this.lblDisplayResolution.Size = new System.Drawing.Size(55, 17);
            this.lblDisplayResolution.Text = "320x240";
            // 
            // lblImageResolution
            // 
            this.lblImageResolution.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblImageResolution.Name = "lblImageResolution";
            this.lblImageResolution.Size = new System.Drawing.Size(62, 17);
            this.lblImageResolution.Text = "1024x768";
            // 
            // lblImageFormat
            // 
            this.lblImageFormat.Name = "lblImageFormat";
            this.lblImageFormat.Size = new System.Drawing.Size(31, 17);
            this.lblImageFormat.Text = "Jpeg";
            // 
            // lblModel
            // 
            this.lblModel.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.lblModel.Name = "lblModel";
            this.lblModel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.lblModel.Size = new System.Drawing.Size(110, 17);
            this.lblModel.Spring = true;
            this.lblModel.Text = "Model";
            // 
            // btnOption
            // 
            this.btnOption.Image = ((System.Drawing.Image)(resources.GetObject("btnOption.Image")));
            this.btnOption.IsLink = true;
            this.btnOption.Name = "btnOption";
            this.btnOption.Size = new System.Drawing.Size(16, 17);
            this.btnOption.Click += new System.EventHandler(this.btnOption_Click);
            // 
            // picCross
            // 
            this.picCross.BackColor = System.Drawing.Color.Transparent;
            this.picCross.ContextMenuStrip = this.contextMenuStrip1;
            this.picCross.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCross.ImageLocation = "";
            this.picCross.Location = new System.Drawing.Point(0, 0);
            this.picCross.Name = "picCross";
            this.picCross.Size = new System.Drawing.Size(320, 240);
            this.picCross.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCross.TabIndex = 3;
            this.picCross.TabStop = false;
            this.picCross.Paint += new System.Windows.Forms.PaintEventHandler(this.picCross_Paint);
            // 
            // VideoCaptureControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.picCross);
            this.Controls.Add(this.videoSourcePlayer);
            this.Controls.Add(this.statusStrip1);
            this.Name = "VideoCaptureControl";
            this.Size = new System.Drawing.Size(320, 262);
            this.Load += new System.EventHandler(this.VideoCaptureControl_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCross)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AForge.Controls.VideoSourcePlayer videoSourcePlayer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblDisplayResolution;
        private System.Windows.Forms.ToolStripStatusLabel lblImageResolution;
        private System.Windows.Forms.ToolStripStatusLabel lblModel;
        private System.Windows.Forms.ToolStripStatusLabel btnOption;
        private System.Windows.Forms.ToolStripStatusLabel lblImageFormat;
        private System.Windows.Forms.PictureBox picCross;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem crossLineToolStripMenuItem;
    }
}
