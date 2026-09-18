namespace DACrux.ProjectManager.UI.Dialog
{
    partial class DlgDataViewMoveColumn
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
            this.grbMoveColumn = new System.Windows.Forms.GroupBox();
            this.lblSelectedColumnID = new System.Windows.Forms.Label();
            this.lvColumnList = new System.Windows.Forms.ListView();
            this.colColId = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.rbtPrevious = new System.Windows.Forms.RadioButton();
            this.rbtLast = new System.Windows.Forms.RadioButton();
            this.rbtFirst = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grbMoveColumn.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbMoveColumn
            // 
            this.grbMoveColumn.Controls.Add(this.lblSelectedColumnID);
            this.grbMoveColumn.Controls.Add(this.lvColumnList);
            this.grbMoveColumn.Controls.Add(this.rbtPrevious);
            this.grbMoveColumn.Controls.Add(this.rbtLast);
            this.grbMoveColumn.Controls.Add(this.rbtFirst);
            this.grbMoveColumn.Location = new System.Drawing.Point(10, 12);
            this.grbMoveColumn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grbMoveColumn.Name = "grbMoveColumn";
            this.grbMoveColumn.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grbMoveColumn.Size = new System.Drawing.Size(275, 220);
            this.grbMoveColumn.TabIndex = 3;
            this.grbMoveColumn.TabStop = false;
            this.grbMoveColumn.Text = "Move Selected Column";
            // 
            // lblSelectedColumnID
            // 
            this.lblSelectedColumnID.AutoSize = true;
            this.lblSelectedColumnID.Location = new System.Drawing.Point(89, 71);
            this.lblSelectedColumnID.Name = "lblSelectedColumnID";
            this.lblSelectedColumnID.Size = new System.Drawing.Size(0, 13);
            this.lblSelectedColumnID.TabIndex = 7;
            // 
            // lvColumnList
            // 
            this.lvColumnList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colColId,
            this.colName});
            this.lvColumnList.FullRowSelect = true;
            this.lvColumnList.Location = new System.Drawing.Point(29, 94);
            this.lvColumnList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lvColumnList.MultiSelect = false;
            this.lvColumnList.Name = "lvColumnList";
            this.lvColumnList.Size = new System.Drawing.Size(232, 113);
            this.lvColumnList.TabIndex = 6;
            this.lvColumnList.UseCompatibleStateImageBehavior = false;
            this.lvColumnList.View = System.Windows.Forms.View.Details;
            this.lvColumnList.SelectedIndexChanged += new System.EventHandler(this.lvColumnList_SelectedIndexChanged);
            // 
            // colColId
            // 
            this.colColId.Text = "Column ID";
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 150;
            // 
            // rbtPrevious
            // 
            this.rbtPrevious.AutoSize = true;
            this.rbtPrevious.Location = new System.Drawing.Point(13, 70);
            this.rbtPrevious.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbtPrevious.Name = "rbtPrevious";
            this.rbtPrevious.Size = new System.Drawing.Size(79, 17);
            this.rbtPrevious.TabIndex = 5;
            this.rbtPrevious.TabStop = true;
            this.rbtPrevious.Text = "previous to";
            this.rbtPrevious.UseVisualStyleBackColor = true;
            this.rbtPrevious.CheckedChanged += new System.EventHandler(this.rbtPrevious_CheckedChanged);
            // 
            // rbtLast
            // 
            this.rbtLast.AutoSize = true;
            this.rbtLast.Location = new System.Drawing.Point(13, 46);
            this.rbtLast.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbtLast.Name = "rbtLast";
            this.rbtLast.Size = new System.Drawing.Size(110, 17);
            this.rbtLast.TabIndex = 4;
            this.rbtLast.TabStop = true;
            this.rbtLast.Text = "to the last column";
            this.rbtLast.UseVisualStyleBackColor = true;
            // 
            // rbtFirst
            // 
            this.rbtFirst.AutoSize = true;
            this.rbtFirst.Location = new System.Drawing.Point(13, 21);
            this.rbtFirst.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbtFirst.Name = "rbtFirst";
            this.rbtFirst.Size = new System.Drawing.Size(112, 17);
            this.rbtFirst.TabIndex = 3;
            this.rbtFirst.TabStop = true;
            this.rbtFirst.Text = "to the first column";
            this.rbtFirst.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(151, 240);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(221, 240);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DlgDataViewMoveColumn
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 270);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grbMoveColumn);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgDataViewMoveColumn";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Move Column";
            this.Load += new System.EventHandler(this.DlgDataViewMoveColumn_Load);
            this.grbMoveColumn.ResumeLayout(false);
            this.grbMoveColumn.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbMoveColumn;
        private System.Windows.Forms.ListView lvColumnList;
        private System.Windows.Forms.RadioButton rbtPrevious;
        private System.Windows.Forms.RadioButton rbtLast;
        private System.Windows.Forms.RadioButton rbtFirst;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader colColId;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Label lblSelectedColumnID;
    }
}