namespace DACrux.BStats.StatDialog.DlgTaguchi_
{
    partial class DlgCloseEffect
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
            this.lstColList = new System.Windows.Forms.ListBox();
            this.lstSelCol = new System.Windows.Forms.ListBox();
            this.butSel = new System.Windows.Forms.Button();
            this.butSelAll = new System.Windows.Forms.Button();
            this.butDeSel = new System.Windows.Forms.Button();
            this.butDeSelAll = new System.Windows.Forms.Button();
            this.lstFactor = new System.Windows.Forms.ListBox();
            this.butOk = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstColList
            // 
            this.lstColList.FormattingEnabled = true;
            this.lstColList.ItemHeight = 12;
            this.lstColList.Location = new System.Drawing.Point(25, 28);
            this.lstColList.Name = "lstColList";
            this.lstColList.Size = new System.Drawing.Size(127, 172);
            this.lstColList.Sorted = true;
            this.lstColList.TabIndex = 0;
            this.lstColList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstColList_MouseDoubleClick);
            // 
            // lstSelCol
            // 
            this.lstSelCol.FormattingEnabled = true;
            this.lstSelCol.ItemHeight = 12;
            this.lstSelCol.Location = new System.Drawing.Point(201, 28);
            this.lstSelCol.Name = "lstSelCol";
            this.lstSelCol.Size = new System.Drawing.Size(127, 172);
            this.lstSelCol.Sorted = true;
            this.lstSelCol.TabIndex = 0;
            this.lstSelCol.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstSelCol_MouseDoubleClick);
            // 
            // butSel
            // 
            this.butSel.Location = new System.Drawing.Point(161, 48);
            this.butSel.Name = "butSel";
            this.butSel.Size = new System.Drawing.Size(32, 23);
            this.butSel.TabIndex = 1;
            this.butSel.Text = ">";
            this.butSel.UseVisualStyleBackColor = true;
            this.butSel.Click += new System.EventHandler(this.butSel_Click);
            // 
            // butSelAll
            // 
            this.butSelAll.Location = new System.Drawing.Point(161, 77);
            this.butSelAll.Name = "butSelAll";
            this.butSelAll.Size = new System.Drawing.Size(32, 23);
            this.butSelAll.TabIndex = 1;
            this.butSelAll.Text = ">>";
            this.butSelAll.UseVisualStyleBackColor = true;
            this.butSelAll.Click += new System.EventHandler(this.butSelAll_Click);
            // 
            // butDeSel
            // 
            this.butDeSel.Location = new System.Drawing.Point(161, 120);
            this.butDeSel.Name = "butDeSel";
            this.butDeSel.Size = new System.Drawing.Size(32, 23);
            this.butDeSel.TabIndex = 1;
            this.butDeSel.Text = "<";
            this.butDeSel.UseVisualStyleBackColor = true;
            this.butDeSel.Click += new System.EventHandler(this.butDeSel_Click);
            // 
            // butDeSelAll
            // 
            this.butDeSelAll.Location = new System.Drawing.Point(161, 149);
            this.butDeSelAll.Name = "butDeSelAll";
            this.butDeSelAll.Size = new System.Drawing.Size(32, 23);
            this.butDeSelAll.TabIndex = 1;
            this.butDeSelAll.Text = "<<";
            this.butDeSelAll.UseVisualStyleBackColor = true;
            this.butDeSelAll.Click += new System.EventHandler(this.butDeSelAll_Click);
            // 
            // lstFactor
            // 
            this.lstFactor.FormattingEnabled = true;
            this.lstFactor.ItemHeight = 12;
            this.lstFactor.Location = new System.Drawing.Point(25, 233);
            this.lstFactor.Name = "lstFactor";
            this.lstFactor.Size = new System.Drawing.Size(303, 88);
            this.lstFactor.TabIndex = 2;
            // 
            // butOk
            // 
            this.butOk.Location = new System.Drawing.Point(122, 327);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(100, 38);
            this.butOk.TabIndex = 1;
            this.butOk.Text = "OK";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(228, 327);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(100, 38);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "사용 가능 항(A)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "선택 항(A)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = " 요인 (F) :";
            // 
            // DlgCloseEffect
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(359, 387);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstFactor);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.butDeSelAll);
            this.Controls.Add(this.butSelAll);
            this.Controls.Add(this.butDeSel);
            this.Controls.Add(this.butSel);
            this.Controls.Add(this.lstSelCol);
            this.Controls.Add(this.lstColList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgCloseEffect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DlgCloseEffect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstColList;
        private System.Windows.Forms.ListBox lstSelCol;
        private System.Windows.Forms.Button butSel;
        private System.Windows.Forms.Button butSelAll;
        private System.Windows.Forms.Button butDeSel;
        private System.Windows.Forms.Button butDeSelAll;
        private System.Windows.Forms.ListBox lstFactor;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}