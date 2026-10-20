namespace NexplantQMS.GdsMap
{
    partial class MapCopyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapCopyForm));
            this.lblSource = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressCopy = new System.Windows.Forms.ProgressBar();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblSource
            //
            this.lblSource.Location = new System.Drawing.Point(16, 18);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(110, 28);
            this.lblSource.TabIndex = 0;
            this.lblSource.Text = "원본 Device";
            this.lblSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtSource
            //
            this.txtSource.Location = new System.Drawing.Point(132, 18);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(312, 28);
            this.txtSource.TabIndex = 1;
            this.txtSource.TabStop = false;
            //
            // lblTarget
            //
            this.lblTarget.Location = new System.Drawing.Point(16, 60);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(110, 28);
            this.lblTarget.TabIndex = 2;
            this.lblTarget.Text = "새 Device";
            this.lblTarget.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtTarget
            //
            this.txtTarget.Location = new System.Drawing.Point(132, 60);
            this.txtTarget.MaxLength = 50;
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(312, 28);
            this.txtTarget.TabIndex = 3;
            this.txtTarget.TextChanged += new System.EventHandler(this.txtTarget_TextChanged);
            //
            // lblStatus
            //
            this.lblStatus.Location = new System.Drawing.Point(16, 98);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(428, 46);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // progressCopy
            //
            this.progressCopy.Location = new System.Drawing.Point(16, 148);
            this.progressCopy.MarqueeAnimationSpeed = 30;
            this.progressCopy.Name = "progressCopy";
            this.progressCopy.Size = new System.Drawing.Size(428, 10);
            this.progressCopy.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressCopy.TabIndex = 5;
            this.progressCopy.Visible = false;
            //
            // btnCopy
            //
            this.btnCopy.Location = new System.Drawing.Point(188, 160);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.Size = new System.Drawing.Size(126, 42);
            this.btnCopy.TabIndex = 6;
            this.btnCopy.Text = "복사";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(320, 160);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Size = new System.Drawing.Size(126, 42);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // MapCopyForm
            //
            this.AcceptButton = this.btnCopy;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(460, 222);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressCopy);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapCopyForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GDS Map 복사";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapCopyForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressCopy;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnCancel;
    }
}
