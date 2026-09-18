namespace DACrux.ProjectManager.UI.Dialog
{
    partial class DlgDataViewInsertRemove
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
            this.rbtRow = new System.Windows.Forms.RadioButton();
            this.rbtColumn = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.grbGroupBox = new System.Windows.Forms.GroupBox();
            this.rbtCells = new System.Windows.Forms.RadioButton();
            this.grbGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbtRow
            // 
            this.rbtRow.AutoSize = true;
            this.rbtRow.Location = new System.Drawing.Point(15, 40);
            this.rbtRow.Name = "rbtRow";
            this.rbtRow.Size = new System.Drawing.Size(46, 17);
            this.rbtRow.TabIndex = 1;
            this.rbtRow.Text = "&Row";
            this.rbtRow.UseVisualStyleBackColor = true;
            // 
            // rbtColumn
            // 
            this.rbtColumn.AutoSize = true;
            this.rbtColumn.Location = new System.Drawing.Point(15, 62);
            this.rbtColumn.Name = "rbtColumn";
            this.rbtColumn.Size = new System.Drawing.Size(60, 17);
            this.rbtColumn.TabIndex = 2;
            this.rbtColumn.Text = "&Column";
            this.rbtColumn.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(81, 107);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOk.Location = new System.Drawing.Point(11, 107);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 25);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grbGroupBox
            // 
            this.grbGroupBox.Controls.Add(this.rbtCells);
            this.grbGroupBox.Controls.Add(this.rbtRow);
            this.grbGroupBox.Controls.Add(this.rbtColumn);
            this.grbGroupBox.Location = new System.Drawing.Point(12, 7);
            this.grbGroupBox.Name = "grbGroupBox";
            this.grbGroupBox.Size = new System.Drawing.Size(134, 94);
            this.grbGroupBox.TabIndex = 5;
            this.grbGroupBox.TabStop = false;
            this.grbGroupBox.Text = "Insert / Remove";
            // 
            // rbtCells
            // 
            this.rbtCells.AutoSize = true;
            this.rbtCells.Checked = true;
            this.rbtCells.Location = new System.Drawing.Point(15, 18);
            this.rbtCells.Name = "rbtCells";
            this.rbtCells.Size = new System.Drawing.Size(47, 17);
            this.rbtCells.TabIndex = 0;
            this.rbtCells.TabStop = true;
            this.rbtCells.Text = "Ce&lls";
            this.rbtCells.UseVisualStyleBackColor = true;
            // 
            // DlgDataViewInsertRemove
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(157, 139);
            this.Controls.Add(this.grbGroupBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgDataViewInsertRemove";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert / Remove";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DlgDataViewInsertRemove_KeyDown);
            this.grbGroupBox.ResumeLayout(false);
            this.grbGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtRow;
        private System.Windows.Forms.RadioButton rbtColumn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox grbGroupBox;
        private System.Windows.Forms.RadioButton rbtCells;
    }
}