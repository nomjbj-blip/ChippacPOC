namespace DACrux.SEMDMS.Test
{
    partial class Form1
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
            this.BtnParseStart = new System.Windows.Forms.Button();
            this.TxtPath = new System.Windows.Forms.RichTextBox();
            this.BtnOpen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnParseStart
            // 
            this.BtnParseStart.Location = new System.Drawing.Point(242, 105);
            this.BtnParseStart.Name = "BtnParseStart";
            this.BtnParseStart.Size = new System.Drawing.Size(110, 28);
            this.BtnParseStart.TabIndex = 0;
            this.BtnParseStart.Text = "Start";
            this.BtnParseStart.UseVisualStyleBackColor = true;
            this.BtnParseStart.Click += new System.EventHandler(this.BtnParseStart_Click);
            // 
            // TxtPath
            // 
            this.TxtPath.Location = new System.Drawing.Point(12, 3);
            this.TxtPath.Name = "TxtPath";
            this.TxtPath.Size = new System.Drawing.Size(340, 54);
            this.TxtPath.TabIndex = 1;
            this.TxtPath.Text = "";
            // 
            // BtnOpen
            // 
            this.BtnOpen.Location = new System.Drawing.Point(12, 63);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(55, 23);
            this.BtnOpen.TabIndex = 2;
            this.BtnOpen.Text = "Open";
            this.BtnOpen.UseVisualStyleBackColor = true;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 140);
            this.Controls.Add(this.BtnOpen);
            this.Controls.Add(this.TxtPath);
            this.Controls.Add(this.BtnParseStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "ParsingTest";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnParseStart;
        private System.Windows.Forms.RichTextBox TxtPath;
        private System.Windows.Forms.Button BtnOpen;
    }
}

