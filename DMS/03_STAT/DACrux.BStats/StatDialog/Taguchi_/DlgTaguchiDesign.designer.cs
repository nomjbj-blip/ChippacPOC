namespace DACrux.BStats.StatDialog.DlgTaguchi_
{
    partial class DlgTaguchiDesign
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
            this.butOk = new System.Windows.Forms.Button();
            this.dgDesignList = new System.Windows.Forms.DataGridView();
            this.butCancel = new System.Windows.Forms.Button();
            this.chkActiveFactor = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgDesignList)).BeginInit();
            this.SuspendLayout();
            // 
            // butOk
            // 
            this.butOk.Location = new System.Drawing.Point(82, 200);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(134, 34);
            this.butOk.TabIndex = 1;
            this.butOk.Text = "OK";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgDesignList
            // 
            this.dgDesignList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDesignList.Location = new System.Drawing.Point(12, 12);
            this.dgDesignList.MultiSelect = false;
            this.dgDesignList.Name = "dgDesignList";
            this.dgDesignList.ReadOnly = true;
            this.dgDesignList.RowHeadersVisible = false;
            this.dgDesignList.RowTemplate.Height = 23;
            this.dgDesignList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDesignList.Size = new System.Drawing.Size(303, 157);
            this.dgDesignList.TabIndex = 3;
            this.dgDesignList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDesignList_CellClick);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(222, 200);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(93, 34);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkActiveFactor
            // 
            this.chkActiveFactor.AutoSize = true;
            this.chkActiveFactor.Location = new System.Drawing.Point(12, 175);
            this.chkActiveFactor.Name = "chkActiveFactor";
            this.chkActiveFactor.Size = new System.Drawing.Size(200, 16);
            this.chkActiveFactor.TabIndex = 4;
            this.chkActiveFactor.Text = "동적 특성을 위한 신호 요인 추가";
            this.chkActiveFactor.UseVisualStyleBackColor = true;
            // 
            // DlgTaguchiDesign
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(329, 248);
            this.Controls.Add(this.chkActiveFactor);
            this.Controls.Add(this.dgDesignList);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgTaguchiDesign";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Taguchi Design - 사용가능한 설계";
            ((System.ComponentModel.ISupportInitialize)(this.dgDesignList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.DataGridView dgDesignList;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox chkActiveFactor;
    }
}