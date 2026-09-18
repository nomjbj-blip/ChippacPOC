namespace DACrux.SP.Controls
{
    partial class RegexSectionControl
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lvEntity = new System.Windows.Forms.ListView();
            this.lvSection = new System.Windows.Forms.ListView();
            this.lvItems = new System.Windows.Forms.ListView();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboFirstEntity = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboLastEntity = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkMustExists = new System.Windows.Forms.CheckBox();
            this.txtParent = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblName.Location = new System.Drawing.Point(3, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(34, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Font = new System.Drawing.Font("Tahoma", 8F);
            this.txtName.Location = new System.Drawing.Point(86, 7);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(196, 20);
            this.txtName.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnSave.Location = new System.Drawing.Point(6, 323);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 25);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnFind.Location = new System.Drawing.Point(217, 323);
            this.btnFind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(64, 25);
            this.btnFind.TabIndex = 10;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnRemove.Location = new System.Drawing.Point(76, 323);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(64, 25);
            this.btnRemove.TabIndex = 9;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // lvEntity
            // 
            this.lvEntity.Location = new System.Drawing.Point(6, 76);
            this.lvEntity.Name = "lvEntity";
            this.lvEntity.Size = new System.Drawing.Size(128, 64);
            this.lvEntity.TabIndex = 13;
            this.lvEntity.UseCompatibleStateImageBehavior = false;
            this.lvEntity.View = System.Windows.Forms.View.Details;
            // 
            // lvSection
            // 
            this.lvSection.Location = new System.Drawing.Point(6, 159);
            this.lvSection.Name = "lvSection";
            this.lvSection.Size = new System.Drawing.Size(128, 61);
            this.lvSection.TabIndex = 14;
            this.lvSection.UseCompatibleStateImageBehavior = false;
            this.lvSection.View = System.Windows.Forms.View.Details;
            // 
            // lvItems
            // 
            this.lvItems.BackColor = System.Drawing.Color.AliceBlue;
            this.lvItems.Location = new System.Drawing.Point(148, 76);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(108, 144);
            this.lvItems.TabIndex = 15;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(259, 98);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(22, 23);
            this.button5.TabIndex = 21;
            this.button5.Text = "▼";
            this.button5.UseCompatibleTextRendering = true;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(259, 76);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(22, 22);
            this.button6.TabIndex = 20;
            this.button6.Text = "▲";
            this.button6.UseCompatibleTextRendering = true;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label1.Location = new System.Drawing.Point(3, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Entity List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label2.Location = new System.Drawing.Point(3, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Section List";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label3.Location = new System.Drawing.Point(145, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Item List";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label4.Location = new System.Drawing.Point(3, 226);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(201, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "[Info] Drag and Drop list items to move. ";
            // 
            // cboFirstEntity
            // 
            this.cboFirstEntity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFirstEntity.Font = new System.Drawing.Font("Tahoma", 8F);
            this.cboFirstEntity.FormattingEnabled = true;
            this.cboFirstEntity.Items.AddRange(new object[] {
            "String",
            "Number",
            "DateTime"});
            this.cboFirstEntity.Location = new System.Drawing.Point(76, 244);
            this.cboFirstEntity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboFirstEntity.Name = "cboFirstEntity";
            this.cboFirstEntity.Size = new System.Drawing.Size(182, 21);
            this.cboFirstEntity.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label6.Location = new System.Drawing.Point(3, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "First Entity";
            // 
            // cboLastEntity
            // 
            this.cboLastEntity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLastEntity.Font = new System.Drawing.Font("Tahoma", 8F);
            this.cboLastEntity.FormattingEnabled = true;
            this.cboLastEntity.Items.AddRange(new object[] {
            "String",
            "Number",
            "DateTime"});
            this.cboLastEntity.Location = new System.Drawing.Point(76, 268);
            this.cboLastEntity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboLastEntity.Name = "cboLastEntity";
            this.cboLastEntity.Size = new System.Drawing.Size(182, 21);
            this.cboLastEntity.TabIndex = 29;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label7.Location = new System.Drawing.Point(3, 272);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Last Entity";
            // 
            // chkMustExists
            // 
            this.chkMustExists.AutoSize = true;
            this.chkMustExists.Font = new System.Drawing.Font("Tahoma", 8F);
            this.chkMustExists.Location = new System.Drawing.Point(6, 297);
            this.chkMustExists.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkMustExists.Name = "chkMustExists";
            this.chkMustExists.Size = new System.Drawing.Size(143, 17);
            this.chkMustExists.TabIndex = 31;
            this.chkMustExists.Text = "This section must exists.";
            this.chkMustExists.UseVisualStyleBackColor = true;
            // 
            // txtParent
            // 
            this.txtParent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParent.Font = new System.Drawing.Font("Tahoma", 8F);
            this.txtParent.Location = new System.Drawing.Point(86, 30);
            this.txtParent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtParent.Name = "txtParent";
            this.txtParent.ReadOnly = true;
            this.txtParent.Size = new System.Drawing.Size(196, 20);
            this.txtParent.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label5.Location = new System.Drawing.Point(3, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Parent Section";
            // 
            // RegexSectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtParent);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkMustExists);
            this.Controls.Add(this.cboLastEntity);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboFirstEntity);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.lvSection);
            this.Controls.Add(this.lvEntity);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Font = new System.Drawing.Font("Tahoma", 8F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RegexSectionControl";
            this.Size = new System.Drawing.Size(284, 357);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ListView lvEntity;
        private System.Windows.Forms.ListView lvSection;
        private System.Windows.Forms.ListView lvItems;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboFirstEntity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboLastEntity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkMustExists;
        private System.Windows.Forms.TextBox txtParent;
        private System.Windows.Forms.Label label5;

    }
}
