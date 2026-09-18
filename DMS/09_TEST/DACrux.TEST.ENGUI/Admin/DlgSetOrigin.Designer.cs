namespace DACrux.TEST.ENGUI
{
    partial class DlgSetOrigin
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgSetOrigin));
            this.butUP = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.butDown = new System.Windows.Forms.Button();
            this.butLeft = new System.Windows.Forms.Button();
            this.butRight = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtOriginX = new System.Windows.Forms.TextBox();
            this.txtOriginY = new System.Windows.Forms.TextBox();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.butCancel = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.butApply = new System.Windows.Forms.Button();
            this.tmrReclick = new System.Windows.Forms.Timer(this.components);
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // butUP
            // 
            this.butUP.BackColor = System.Drawing.SystemColors.Control;
            this.butUP.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butUP.BackgroundImage")));
            this.butUP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butUP.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.butUP.Location = new System.Drawing.Point(87, 12);
            this.butUP.Name = "butUP";
            this.butUP.Size = new System.Drawing.Size(32, 32);
            this.butUP.TabIndex = 9;
            this.butUP.Tag = "2";
            this.butUP.UseVisualStyleBackColor = false;
            this.butUP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollBut_MouseDown);
            this.butUP.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollBut_MouseUp);
            // 
            // button2
            // 
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.button2.Location = new System.Drawing.Point(79, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 48);
            this.button2.TabIndex = 32;
            this.button2.Tag = "0";
            // 
            // butDown
            // 
            this.butDown.BackColor = System.Drawing.SystemColors.Control;
            this.butDown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butDown.BackgroundImage")));
            this.butDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butDown.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.butDown.Location = new System.Drawing.Point(87, 92);
            this.butDown.Name = "butDown";
            this.butDown.Size = new System.Drawing.Size(32, 32);
            this.butDown.TabIndex = 9;
            this.butDown.Tag = "0";
            this.butDown.UseVisualStyleBackColor = false;
            this.butDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollBut_MouseDown);
            this.butDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollBut_MouseUp);
            // 
            // butLeft
            // 
            this.butLeft.BackColor = System.Drawing.SystemColors.Control;
            this.butLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butLeft.BackgroundImage")));
            this.butLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butLeft.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.butLeft.Location = new System.Drawing.Point(47, 52);
            this.butLeft.Name = "butLeft";
            this.butLeft.Size = new System.Drawing.Size(32, 32);
            this.butLeft.TabIndex = 9;
            this.butLeft.Tag = "1";
            this.butLeft.UseVisualStyleBackColor = false;
            this.butLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollBut_MouseDown);
            this.butLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollBut_MouseUp);
            // 
            // butRight
            // 
            this.butRight.BackColor = System.Drawing.SystemColors.Control;
            this.butRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butRight.BackgroundImage")));
            this.butRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butRight.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.butRight.Location = new System.Drawing.Point(127, 52);
            this.butRight.Name = "butRight";
            this.butRight.Size = new System.Drawing.Size(32, 32);
            this.butRight.TabIndex = 9;
            this.butRight.Tag = "3";
            this.butRight.UseVisualStyleBackColor = false;
            this.butRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollBut_MouseDown);
            this.butRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollBut_MouseUp);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(18, 127);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 24);
            this.label11.TabIndex = 35;
            this.label11.Text = "Origin X";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(18, 150);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(51, 24);
            this.label14.TabIndex = 36;
            this.label14.Text = "Origin Y";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOriginX
            // 
            this.txtOriginX.Location = new System.Drawing.Point(75, 127);
            this.txtOriginX.Name = "txtOriginX";
            this.txtOriginX.Size = new System.Drawing.Size(84, 21);
            this.txtOriginX.TabIndex = 33;
            // 
            // txtOriginY
            // 
            this.txtOriginY.Location = new System.Drawing.Point(75, 150);
            this.txtOriginY.Name = "txtOriginY";
            this.txtOriginY.Size = new System.Drawing.Size(84, 21);
            this.txtOriginY.TabIndex = 34;
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Location = new System.Drawing.Point(8, 12);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(71, 16);
            this.chkPreview.TabIndex = 37;
            this.chkPreview.Text = "PreView";
            this.chkPreview.UseVisualStyleBackColor = true;
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(131, 182);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(58, 23);
            this.butCancel.TabIndex = 40;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(67, 182);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(58, 23);
            this.butOK.TabIndex = 39;
            this.butOK.Text = "Ok";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butApply
            // 
            this.butApply.Location = new System.Drawing.Point(3, 182);
            this.butApply.Name = "butApply";
            this.butApply.Size = new System.Drawing.Size(58, 23);
            this.butApply.TabIndex = 38;
            this.butApply.Text = "Apply";
            this.butApply.UseVisualStyleBackColor = true;
            this.butApply.Click += new System.EventHandler(this.butApply_Click);
            // 
            // tmrReclick
            // 
            this.tmrReclick.Tick += new System.EventHandler(this.tmrReclick_Tick);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(38, 32);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(42, 16);
            this.radioButton1.TabIndex = 41;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "L,T";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton2.Location = new System.Drawing.Point(38, 88);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(42, 16);
            this.radioButton2.TabIndex = 41;
            this.radioButton2.Text = "L,B";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(124, 90);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(43, 16);
            this.radioButton3.TabIndex = 41;
            this.radioButton3.Text = "R,B";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(126, 32);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(43, 16);
            this.radioButton4.TabIndex = 41;
            this.radioButton4.Text = "R,T";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // DlgSetOrigin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(191, 211);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butApply);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtOriginX);
            this.Controls.Add(this.txtOriginY);
            this.Controls.Add(this.butUP);
            this.Controls.Add(this.butRight);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.butDown);
            this.Controls.Add(this.butLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgSetOrigin";
            this.Text = "DlgSetOrigin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butUP;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button butDown;
        private System.Windows.Forms.Button butLeft;
        private System.Windows.Forms.Button butRight;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtOriginX;
        private System.Windows.Forms.TextBox txtOriginY;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butApply;
        private System.Windows.Forms.Timer tmrReclick;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
    }
}