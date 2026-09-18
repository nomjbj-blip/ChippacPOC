namespace DACrux.SEMDMS.Control
{
    partial class DPUCReclassify
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.fpsDefects = new FarPoint.Win.Spread.FpSpread();
            this.fpsDefects_Sheet = new FarPoint.Win.Spread.SheetView();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.dlbNewClassNumber = new DACrux.Framework.Controls.DUCListBox();
            this.rdbClassName = new System.Windows.Forms.RadioButton();
            this.rdbClassCode = new System.Windows.Forms.RadioButton();
            this.btnApply = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.fpsDisplayClass = new FarPoint.Win.Spread.FpSpread();
            this.fpsDisplayClass_Sheet = new FarPoint.Win.Spread.SheetView();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraSplitter2 = new Infragistics.Win.Misc.UltraSplitter();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefects_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDisplayClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDisplayClass_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.fpsDefects);
            this.ultraGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox2.Location = new System.Drawing.Point(0, 171);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(237, 321);
            this.ultraGroupBox2.TabIndex = 4;
            this.ultraGroupBox2.Text = "Selected Defect";
            this.ultraGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // fpsDefects
            // 
            this.fpsDefects.AccessibleDescription = "";
            this.fpsDefects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsDefects.Location = new System.Drawing.Point(2, 18);
            this.fpsDefects.Name = "fpsDefects";
            this.fpsDefects.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsDefects_Sheet});
            this.fpsDefects.Size = new System.Drawing.Size(233, 301);
            this.fpsDefects.TabIndex = 0;
            // 
            // fpsDefects_Sheet
            // 
            this.fpsDefects_Sheet.Reset();
            fpsDefects_Sheet.SheetName = "Sheet1";
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.dlbNewClassNumber);
            this.ultraGroupBox3.Controls.Add(this.rdbClassName);
            this.ultraGroupBox3.Controls.Add(this.rdbClassCode);
            this.ultraGroupBox3.Controls.Add(this.btnApply);
            this.ultraGroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraGroupBox3.Location = new System.Drawing.Point(0, 498);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(237, 261);
            this.ultraGroupBox3.TabIndex = 5;
            this.ultraGroupBox3.Text = "NEW CLASS";
            this.ultraGroupBox3.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // dlbNewClassNumber
            // 
            this.dlbNewClassNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dlbNewClassNumber.DataSource = null;
            this.dlbNewClassNumber.DisplayMember = "";
            this.dlbNewClassNumber.Location = new System.Drawing.Point(5, 43);
            this.dlbNewClassNumber.Name = "dlbNewClassNumber";
            this.dlbNewClassNumber.SearchText = "";
            this.dlbNewClassNumber.SearchTitle = "";
            this.dlbNewClassNumber.SelectedIndex = -1;
            this.dlbNewClassNumber.SelectedItem = null;
            this.dlbNewClassNumber.SelectedValue = null;
            this.dlbNewClassNumber.Size = new System.Drawing.Size(227, 177);
            this.dlbNewClassNumber.TabIndex = 4;
            this.dlbNewClassNumber.ValueMember = "";
            // 
            // rdbClassName
            // 
            this.rdbClassName.AutoSize = true;
            this.rdbClassName.Location = new System.Drawing.Point(115, 21);
            this.rdbClassName.Name = "rdbClassName";
            this.rdbClassName.Size = new System.Drawing.Size(94, 16);
            this.rdbClassName.TabIndex = 3;
            this.rdbClassName.Text = "Class Name";
            this.rdbClassName.UseVisualStyleBackColor = true;
            this.rdbClassName.CheckedChanged += new System.EventHandler(this.rdbClassCodeOrName_CheckedChanged);
            // 
            // rdbClassCode
            // 
            this.rdbClassCode.AutoSize = true;
            this.rdbClassCode.Checked = true;
            this.rdbClassCode.Location = new System.Drawing.Point(5, 21);
            this.rdbClassCode.Name = "rdbClassCode";
            this.rdbClassCode.Size = new System.Drawing.Size(90, 16);
            this.rdbClassCode.TabIndex = 2;
            this.rdbClassCode.TabStop = true;
            this.rdbClassCode.Text = "Class Code";
            this.rdbClassCode.UseVisualStyleBackColor = true;
            this.rdbClassCode.CheckedChanged += new System.EventHandler(this.rdbClassCodeOrName_CheckedChanged);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(148, 226);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(84, 30);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.fpsDisplayClass);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(237, 165);
            this.ultraGroupBox1.TabIndex = 3;
            this.ultraGroupBox1.Text = "Display Class";
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // fpsDisplayClass
            // 
            this.fpsDisplayClass.AccessibleDescription = "";
            this.fpsDisplayClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsDisplayClass.Location = new System.Drawing.Point(2, 18);
            this.fpsDisplayClass.Name = "fpsDisplayClass";
            this.fpsDisplayClass.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsDisplayClass_Sheet});
            this.fpsDisplayClass.Size = new System.Drawing.Size(233, 145);
            this.fpsDisplayClass.TabIndex = 0;
            // 
            // fpsDisplayClass_Sheet
            // 
            this.fpsDisplayClass_Sheet.Reset();
            fpsDisplayClass_Sheet.SheetName = "Sheet1";
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 165);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 165;
            this.ultraSplitter1.Size = new System.Drawing.Size(237, 6);
            this.ultraSplitter1.TabIndex = 6;
            // 
            // ultraSplitter2
            // 
            this.ultraSplitter2.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter2.Location = new System.Drawing.Point(0, 492);
            this.ultraSplitter2.Name = "ultraSplitter2";
            this.ultraSplitter2.RestoreExtent = 261;
            this.ultraSplitter2.Size = new System.Drawing.Size(237, 6);
            this.ultraSplitter2.TabIndex = 7;
            // 
            // DPUCReclassify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.ultraSplitter2);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.ultraGroupBox3);
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "DPUCReclassify";
            this.Size = new System.Drawing.Size(237, 759);
            this.Load += new System.EventHandler(this.DPUCReclassify_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefects_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ultraGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpsDisplayClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDisplayClass_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private FarPoint.Win.Spread.FpSpread fpsDefects;
        private FarPoint.Win.Spread.SheetView fpsDefects_Sheet;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private System.Windows.Forms.RadioButton rdbClassName;
        private System.Windows.Forms.RadioButton rdbClassCode;
        private Infragistics.Win.Misc.UltraButton btnApply;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private FarPoint.Win.Spread.FpSpread fpsDisplayClass;
        private FarPoint.Win.Spread.SheetView fpsDisplayClass_Sheet;
        private Framework.Controls.DUCListBox dlbNewClassNumber;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter2;
    }
}
