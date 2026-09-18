namespace DACrux.Framework
{
    partial class frmSystemOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSystemOption));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblLaguage = new System.Windows.Forms.Label();
            this.cmbLaguage = new System.Windows.Forms.ComboBox();
            this.grpAppServer = new System.Windows.Forms.GroupBox();
            this.lblAppServerIP = new System.Windows.Forms.Label();
            this.txtAppServer = new System.Windows.Forms.TextBox();
            this.grpH101 = new System.Windows.Forms.GroupBox();
            this.lblServerChannel = new System.Windows.Forms.Label();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.txtH101ServerIP = new System.Windows.Forms.TextBox();
            this.txtH101ServerChannel = new System.Windows.Forms.TextBox();
            this.txtH101ServerPort = new System.Windows.Forms.TextBox();
            this.picLine = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.pnlTop.SuspendLayout();
            this.grpAppServer.SuspendLayout();
            this.grpH101.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.Controls.Add(this.lblLaguage);
            this.pnlTop.Controls.Add(this.cmbLaguage);
            this.pnlTop.Controls.Add(this.grpAppServer);
            this.pnlTop.Controls.Add(this.grpH101);
            this.pnlTop.Controls.Add(this.picLine);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(409, 230);
            this.pnlTop.TabIndex = 7;
            // 
            // lblLaguage
            // 
            this.lblLaguage.BackColor = System.Drawing.Color.Transparent;
            this.lblLaguage.Location = new System.Drawing.Point(14, 54);
            this.lblLaguage.Name = "lblLaguage";
            this.lblLaguage.Size = new System.Drawing.Size(116, 19);
            this.lblLaguage.TabIndex = 77;
            this.lblLaguage.Text = "Language";
            this.lblLaguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbLaguage
            // 
            this.cmbLaguage.FormattingEnabled = true;
            this.cmbLaguage.Location = new System.Drawing.Point(138, 54);
            this.cmbLaguage.Name = "cmbLaguage";
            this.cmbLaguage.Size = new System.Drawing.Size(259, 22);
            this.cmbLaguage.TabIndex = 135;
            this.cmbLaguage.SelectedIndexChanged += new System.EventHandler(this.cmbLaguage_SelectedIndexChanged);
            this.cmbLaguage.SelectionChangeCommitted += new System.EventHandler(this.cmbLaguage_SelectionChangeCommitted);
            // 
            // grpAppServer
            // 
            this.grpAppServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAppServer.BackColor = System.Drawing.Color.Transparent;
            this.grpAppServer.Controls.Add(this.lblAppServerIP);
            this.grpAppServer.Controls.Add(this.txtAppServer);
            this.grpAppServer.Location = new System.Drawing.Point(7, 80);
            this.grpAppServer.Name = "grpAppServer";
            this.grpAppServer.Size = new System.Drawing.Size(394, 60);
            this.grpAppServer.TabIndex = 134;
            this.grpAppServer.TabStop = false;
            this.grpAppServer.Text = "Application Server";
            // 
            // lblAppServerIP
            // 
            this.lblAppServerIP.Location = new System.Drawing.Point(6, 24);
            this.lblAppServerIP.Name = "lblAppServerIP";
            this.lblAppServerIP.Size = new System.Drawing.Size(117, 22);
            this.lblAppServerIP.TabIndex = 77;
            this.lblAppServerIP.Text = "Server IP";
            this.lblAppServerIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAppServer
            // 
            this.txtAppServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAppServer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtAppServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAppServer.Location = new System.Drawing.Point(129, 24);
            this.txtAppServer.Name = "txtAppServer";
            this.txtAppServer.Size = new System.Drawing.Size(259, 22);
            this.txtAppServer.TabIndex = 72;
            // 
            // grpH101
            // 
            this.grpH101.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpH101.BackColor = System.Drawing.Color.Transparent;
            this.grpH101.Controls.Add(this.lblServerChannel);
            this.grpH101.Controls.Add(this.lblServerPort);
            this.grpH101.Controls.Add(this.lblServerIP);
            this.grpH101.Controls.Add(this.txtH101ServerIP);
            this.grpH101.Controls.Add(this.txtH101ServerChannel);
            this.grpH101.Controls.Add(this.txtH101ServerPort);
            this.grpH101.Location = new System.Drawing.Point(7, 146);
            this.grpH101.Name = "grpH101";
            this.grpH101.Size = new System.Drawing.Size(394, 74);
            this.grpH101.TabIndex = 133;
            this.grpH101.TabStop = false;
            this.grpH101.Text = "EAI";
            // 
            // lblServerChannel
            // 
            this.lblServerChannel.Location = new System.Drawing.Point(8, 44);
            this.lblServerChannel.Name = "lblServerChannel";
            this.lblServerChannel.Size = new System.Drawing.Size(115, 22);
            this.lblServerChannel.TabIndex = 77;
            this.lblServerChannel.Text = "Channel";
            this.lblServerChannel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblServerPort
            // 
            this.lblServerPort.Location = new System.Drawing.Point(225, 16);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(94, 22);
            this.lblServerPort.TabIndex = 77;
            this.lblServerPort.Text = "Server Port";
            this.lblServerPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblServerIP
            // 
            this.lblServerIP.Location = new System.Drawing.Point(8, 16);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(115, 22);
            this.lblServerIP.TabIndex = 77;
            this.lblServerIP.Text = "Server IP";
            this.lblServerIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtH101ServerIP
            // 
            this.txtH101ServerIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtH101ServerIP.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtH101ServerIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtH101ServerIP.Location = new System.Drawing.Point(129, 16);
            this.txtH101ServerIP.Name = "txtH101ServerIP";
            this.txtH101ServerIP.Size = new System.Drawing.Size(90, 22);
            this.txtH101ServerIP.TabIndex = 72;
            // 
            // txtH101ServerChannel
            // 
            this.txtH101ServerChannel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtH101ServerChannel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtH101ServerChannel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtH101ServerChannel.Location = new System.Drawing.Point(129, 44);
            this.txtH101ServerChannel.Name = "txtH101ServerChannel";
            this.txtH101ServerChannel.Size = new System.Drawing.Size(259, 22);
            this.txtH101ServerChannel.TabIndex = 76;
            // 
            // txtH101ServerPort
            // 
            this.txtH101ServerPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtH101ServerPort.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtH101ServerPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtH101ServerPort.Location = new System.Drawing.Point(324, 16);
            this.txtH101ServerPort.Name = "txtH101ServerPort";
            this.txtH101ServerPort.Size = new System.Drawing.Size(55, 22);
            this.txtH101ServerPort.TabIndex = 76;
            // 
            // picLine
            // 
            this.picLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picLine.Image = ((System.Drawing.Image)(resources.GetObject("picLine.Image")));
            this.picLine.Location = new System.Drawing.Point(7, 40);
            this.picLine.Name = "picLine";
            this.picLine.Size = new System.Drawing.Size(394, 5);
            this.picLine.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLine.TabIndex = 34;
            this.picLine.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Image")));
            this.lblTitle.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblTitle.Location = new System.Drawing.Point(7, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(236, 28);
            this.lblTitle.TabIndex = 33;
            this.lblTitle.Text = "     System Option";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(252, 234);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 22);
            this.btnSave.TabIndex = 66;
            this.btnSave.TabStop = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(327, 234);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 22);
            this.btnClose.TabIndex = 63;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmSystemOption
            // 
            this.ClientSize = new System.Drawing.Size(409, 268);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSystemOption";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " System Option";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmSystemOption_Load);
            this.pnlTop.ResumeLayout(false);
            this.grpAppServer.ResumeLayout(false);
            this.grpAppServer.PerformLayout();
            this.grpH101.ResumeLayout(false);
            this.grpH101.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtH101ServerIP;
        private System.Windows.Forms.PictureBox btnSave;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox picLine;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtH101ServerPort;
        private System.Windows.Forms.GroupBox grpH101;
        private System.Windows.Forms.TextBox txtH101ServerChannel;
        private System.Windows.Forms.GroupBox grpAppServer;
        private System.Windows.Forms.TextBox txtAppServer;
        private System.Windows.Forms.ComboBox cmbLaguage;
        private System.Windows.Forms.Label lblLaguage;
        private System.Windows.Forms.Label lblAppServerIP;
        private System.Windows.Forms.Label lblServerChannel;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.Label lblServerIP;


    }
}