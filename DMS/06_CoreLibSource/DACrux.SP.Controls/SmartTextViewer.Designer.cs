namespace DACrux.SP.Controls
{
    partial class SmartTextViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlPaging = new System.Windows.Forms.Panel();
            this.pageLister = new DACrux.SP.Controls.PageLister();
            this.txtContent = new DACrux.SP.Controls.RichTextBoxEx();
            this.uclLineNo = new DACrux.SP.Controls.LineNumbersPanel();
            this.pnlPaging.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPaging
            // 
            this.pnlPaging.Controls.Add(this.pageLister);
            this.pnlPaging.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPaging.Location = new System.Drawing.Point(0, 301);
            this.pnlPaging.Name = "pnlPaging";
            this.pnlPaging.Size = new System.Drawing.Size(369, 30);
            this.pnlPaging.TabIndex = 5;
            // 
            // pageLister
            // 
            this.pageLister.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageLister.Location = new System.Drawing.Point(0, 0);
            this.pageLister.Name = "pageLister";
            this.pageLister.NumPagesShown = 10;
            this.pageLister.PagesCount = 0;
            this.pageLister.Size = new System.Drawing.Size(228, 30);
            this.pageLister.TabIndex = 0;
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtContent.Location = new System.Drawing.Point(40, 0);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(329, 301);
            this.txtContent.TabIndex = 6;
            this.txtContent.Text = "";
            this.txtContent.WordWrap = false;
            // 
            // uclLineNo
            // 
            this.uclLineNo.AutoSizing = false;
            this.uclLineNo.BackgroundGradientAlphaColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.uclLineNo.BackgroundGradientBetaColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.uclLineNo.BackgroundGradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.uclLineNo.BorderLinesColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.uclLineNo.BorderLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.uclLineNo.BorderLinesThickness = 1F;
            this.uclLineNo.Dock = System.Windows.Forms.DockStyle.Left;
            this.uclLineNo.DockSide = DACrux.SP.Controls.LineNumbersPanel.LineNumberDockSide.Left;
            this.uclLineNo.GridLinesColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.uclLineNo.GridLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.uclLineNo.GridLinesThickness = 1F;
            this.uclLineNo.LineNumbersAlignment = System.Drawing.ContentAlignment.TopRight;
            this.uclLineNo.LineNumbersAntiAlias = true;
            this.uclLineNo.LineNumbersAsHexadecimal = false;
            this.uclLineNo.LineNumbersClippedByItemRectangle = false;
            this.uclLineNo.LineNumbersLeadingZeroes = false;
            this.uclLineNo.LineNumbersOffset = new System.Drawing.Size(0, 0);
            this.uclLineNo.LineOffset = 0;
            this.uclLineNo.Location = new System.Drawing.Point(-1, 0);
            this.uclLineNo.Margin = new System.Windows.Forms.Padding(0);
            this.uclLineNo.MarginLinesColor = System.Drawing.Color.SlateGray;
            this.uclLineNo.MarginLinesSide = DACrux.SP.Controls.LineNumbersPanel.LineNumberDockSide.Right;
            this.uclLineNo.MarginLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.uclLineNo.MarginLinesThickness = 1F;
            this.uclLineNo.Name = "uclLineNo";
            this.uclLineNo.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.uclLineNo.ParentRichTextBox = this.txtContent;
            this.uclLineNo.SeeThroughMode = false;
            this.uclLineNo.ShowBackgroundGradient = false;
            this.uclLineNo.ShowBorderLines = true;
            this.uclLineNo.ShowGridLines = true;
            this.uclLineNo.ShowLineNumbers = true;
            this.uclLineNo.ShowMarginLines = true;
            this.uclLineNo.Size = new System.Drawing.Size(40, 301);
            this.uclLineNo.TabIndex = 4;
            // 
            // SmartTextViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.uclLineNo);
            this.Controls.Add(this.pnlPaging);
            this.Name = "SmartTextViewer";
            this.Size = new System.Drawing.Size(369, 331);
            this.pnlPaging.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LineNumbersPanel uclLineNo;
        private System.Windows.Forms.Panel pnlPaging;
        private PageLister pageLister;
        private RichTextBoxEx txtContent;
    }
}
