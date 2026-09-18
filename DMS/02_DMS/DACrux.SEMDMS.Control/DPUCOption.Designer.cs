namespace DACrux.SEMDMS.Control
{
    partial class DPUCOption
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.txtInfor = new System.Windows.Forms.TextBox();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.fpsDefects = new FarPoint.Win.Spread.FpSpread();
            this.fpsDefects_Sheet = new FarPoint.Win.Spread.SheetView();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.rdbClassName = new System.Windows.Forms.RadioButton();
            this.rdbClassCode = new System.Windows.Forms.RadioButton();
            this.btnApply = new Infragistics.Win.Misc.UltraButton();
            this.uceClassNew = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.fpsDisplayClass = new FarPoint.Win.Spread.FpSpread();
            this.fpsDisplayClass_Sheet = new FarPoint.Win.Spread.SheetView();
            this.ultraTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefects_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uceClassNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDisplayClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDisplayClass_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl)).BeginInit();
            this.ultraTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.txtInfor);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(297, 789);
            // 
            // txtInfor
            // 
            this.txtInfor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfor.Location = new System.Drawing.Point(0, 0);
            this.txtInfor.Multiline = true;
            this.txtInfor.Name = "txtInfor";
            this.txtInfor.Size = new System.Drawing.Size(297, 789);
            this.txtInfor.TabIndex = 0;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ultraGroupBox2);
            this.ultraTabPageControl2.Controls.Add(this.ultraGroupBox3);
            this.ultraTabPageControl2.Controls.Add(this.ultraGroupBox1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(297, 789);
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.fpsDefects);
            this.ultraGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox2.Location = new System.Drawing.Point(0, 165);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(297, 363);
            this.ultraGroupBox2.TabIndex = 1;
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
            this.fpsDefects.Size = new System.Drawing.Size(293, 343);
            this.fpsDefects.TabIndex = 0;
            // 
            // fpsDefects_Sheet
            // 
            this.fpsDefects_Sheet.Reset();
            fpsDefects_Sheet.SheetName = "Sheet1";
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.rdbClassName);
            this.ultraGroupBox3.Controls.Add(this.rdbClassCode);
            this.ultraGroupBox3.Controls.Add(this.btnApply);
            this.ultraGroupBox3.Controls.Add(this.uceClassNew);
            this.ultraGroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraGroupBox3.Location = new System.Drawing.Point(0, 528);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(297, 261);
            this.ultraGroupBox3.TabIndex = 2;
            this.ultraGroupBox3.Text = "NEW CLASS";
            this.ultraGroupBox3.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
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
            this.btnApply.Location = new System.Drawing.Point(208, 70);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(84, 30);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // uceClassNew
            // 
            this.uceClassNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uceClassNew.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.uceClassNew.Location = new System.Drawing.Point(7, 43);
            this.uceClassNew.Name = "uceClassNew";
            this.uceClassNew.Size = new System.Drawing.Size(287, 21);
            this.uceClassNew.TabIndex = 0;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.fpsDisplayClass);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(297, 165);
            this.ultraGroupBox1.TabIndex = 0;
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
            this.fpsDisplayClass.Size = new System.Drawing.Size(293, 145);
            this.fpsDisplayClass.TabIndex = 0;
            // 
            // fpsDisplayClass_Sheet
            // 
            this.fpsDisplayClass_Sheet.Reset();
            fpsDisplayClass_Sheet.SheetName = "Sheet1";
            // 
            // ultraTabControl
            // 
            this.ultraTabControl.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl.Name = "ultraTabControl";
            this.ultraTabControl.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl.Size = new System.Drawing.Size(299, 810);
            this.ultraTabControl.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.ultraTabControl.TabIndex = 0;
            ultraTab1.Key = "INFORMATION";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Information";
            ultraTab1.ToolTipText = "Information";
            ultraTab2.Key = "RECLASSIFY";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Reclassify";
            ultraTab2.ToolTipText = "Reclassify";
            ultraTab3.Key = "DEFECT_INFO";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Defect Infor.";
            this.ultraTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            this.ultraTabControl.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(297, 789);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(297, 789);
            // 
            // DPUCOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraTabControl);
            this.Name = "DPUCOption";
            this.Size = new System.Drawing.Size(299, 810);
            this.Load += new System.EventHandler(this.DPUCOption_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefects_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ultraGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uceClassNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpsDisplayClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDisplayClass_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl)).EndInit();
            this.ultraTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private Infragistics.Win.Misc.UltraButton btnApply;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor uceClassNew;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private FarPoint.Win.Spread.FpSpread fpsDefects;
        private FarPoint.Win.Spread.SheetView fpsDefects_Sheet;
        private FarPoint.Win.Spread.FpSpread fpsDisplayClass;
        private FarPoint.Win.Spread.SheetView fpsDisplayClass_Sheet;
        private System.Windows.Forms.RadioButton rdbClassName;
        private System.Windows.Forms.RadioButton rdbClassCode;
        private System.Windows.Forms.TextBox txtInfor;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
    }
}
