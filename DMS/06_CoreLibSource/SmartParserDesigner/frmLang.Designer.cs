namespace SmartParser.Designer
{
    partial class frmLang
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLang));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.uclTitleGeneral = new DACrux.SP.Controls.uclTitle();
            this.rowDel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.colDel = new System.Windows.Forms.Button();
            this.ColAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.uclTitleGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(8, 63);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(615, 480);
            this.dataGridView1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.uclTitleGeneral);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(626, 63);
            this.panel2.TabIndex = 23;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(558, 37);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 26);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = DACrux.SP.Common.MultiLang.SelectLang["Save"];
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // uclTitleGeneral
            // 
            this.uclTitleGeneral.BackColor = System.Drawing.Color.Transparent;
            this.uclTitleGeneral.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uclTitleGeneral.BackgroundImage")));
            this.uclTitleGeneral.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uclTitleGeneral.Controls.Add(this.rowDel);
            this.uclTitleGeneral.Controls.Add(this.textBox1);
            this.uclTitleGeneral.Controls.Add(this.colDel);
            this.uclTitleGeneral.Controls.Add(this.ColAdd);
            this.uclTitleGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.uclTitleGeneral.EndColor = System.Drawing.Color.White;
            this.uclTitleGeneral.Font = new System.Drawing.Font("Tahoma", 8F);
            this.uclTitleGeneral.GradientStyle = DACrux.SP.Controls.GradientMode.Horizontal;
            this.uclTitleGeneral.Location = new System.Drawing.Point(0, 0);
            this.uclTitleGeneral.Margin = new System.Windows.Forms.Padding(0);
            this.uclTitleGeneral.Name = "uclTitleGeneral";
            this.uclTitleGeneral.Size = new System.Drawing.Size(626, 20);
            this.uclTitleGeneral.StartColor = System.Drawing.Color.LightSteelBlue;
            this.uclTitleGeneral.TabIndex = 17;
            this.uclTitleGeneral.Title = "";
            // 
            // rowDel
            // 
            this.rowDel.Location = new System.Drawing.Point(188, 37);
            this.rowDel.Name = "rowDel";
            this.rowDel.Size = new System.Drawing.Size(63, 26);
            this.rowDel.TabIndex = 21;
            this.rowDel.Text = "rowDel";
            this.rowDel.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(108, -5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(143, 20);
            this.textBox1.TabIndex = 18;
            // 
            // colDel
            // 
            this.colDel.Location = new System.Drawing.Point(261, 37);
            this.colDel.Name = "colDel";
            this.colDel.Size = new System.Drawing.Size(76, 26);
            this.colDel.TabIndex = 20;
            this.colDel.Text = "colDel";
            this.colDel.UseVisualStyleBackColor = true;
            // 
            // ColAdd
            // 
            this.ColAdd.Location = new System.Drawing.Point(257, -7);
            this.ColAdd.Name = "ColAdd";
            this.ColAdd.Size = new System.Drawing.Size(80, 26);
            this.ColAdd.TabIndex = 19;
            this.ColAdd.Text = "Lang Add";
            this.ColAdd.UseVisualStyleBackColor = true;
            // 
            // frmLang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 543);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Name = "frmLang";
            this.Text = "Language";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.uclTitleGeneral.ResumeLayout(false);
            this.uclTitleGeneral.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel2;
        private DACrux.SP.Controls.uclTitle uclTitleGeneral;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button rowDel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button colDel;
        private System.Windows.Forms.Button ColAdd;



    }
}