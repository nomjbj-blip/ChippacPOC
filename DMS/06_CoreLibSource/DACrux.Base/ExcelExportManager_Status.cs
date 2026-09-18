using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Utility
{
    // ExcelExcportManager status form 2019.08.07 Taihi,Kim.
    public partial class ExcelExportManager_Status : Form
    {
        #region 멤버 변수

        public static string FormatMessage = "엑셀 파일을 저장합니다... ({0}/{1})";

        private int _currentCount;
        private int _totalCount;
        private System.Windows.Forms.Label label1;
        
        #endregion

        #region 메서드

        public ExcelExportManager_Status()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = "엑셀 파일을 저장합니다... ({0}/{0})";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExcelExportManager_Status
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 55);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExcelExportManager_Status";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.TopMost = true;
            this.ResumeLayout(false);
        }

        #endregion

        public new void Show()
        {
            base.Show();
            Application.DoEvents();
        }

        private void UpdateUI()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Update));
            }
            else
            {
                label1.Text = String.Format(FormatMessage, _currentCount, _totalCount);
                Application.DoEvents();
            }
        }
        
        #endregion

        #region 프로퍼티

        public int CurrentCount
        {
            get { return _currentCount; }
            set { _currentCount = value; UpdateUI(); }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; UpdateUI(); }
        }

        #endregion
    }
}
