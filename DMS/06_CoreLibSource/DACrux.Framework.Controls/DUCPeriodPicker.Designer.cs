namespace DACrux.Framework.Controls
{
    partial class DUCPeriodPicker
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoByDay = new System.Windows.Forms.RadioButton();
            this.rdoByWeek = new System.Windows.Forms.RadioButton();
            this.rdoByMonth = new System.Windows.Forms.RadioButton();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rdoByDay);
            this.panel1.Controls.Add(this.rdoByWeek);
            this.panel1.Controls.Add(this.rdoByMonth);
            this.panel1.Controls.Add(this.dtpStart);
            this.panel1.Controls.Add(this.dtpEnd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 66);
            this.panel1.TabIndex = 1;
            // 
            // rdoByDay
            // 
            this.rdoByDay.AutoSize = true;
            this.rdoByDay.Checked = true;
            this.rdoByDay.Location = new System.Drawing.Point(5, 9);
            this.rdoByDay.Name = "rdoByDay";
            this.rdoByDay.Size = new System.Drawing.Size(45, 18);
            this.rdoByDay.TabIndex = 5;
            this.rdoByDay.TabStop = true;
            this.rdoByDay.Text = "Day";
            this.rdoByDay.UseVisualStyleBackColor = true;
            this.rdoByDay.Click += new System.EventHandler(this.rdoButton_Click);
            // 
            // rdoByWeek
            // 
            this.rdoByWeek.AutoSize = true;
            this.rdoByWeek.Location = new System.Drawing.Point(56, 9);
            this.rdoByWeek.Name = "rdoByWeek";
            this.rdoByWeek.Size = new System.Drawing.Size(57, 18);
            this.rdoByWeek.TabIndex = 6;
            this.rdoByWeek.Text = "Week";
            this.rdoByWeek.UseVisualStyleBackColor = true;
            this.rdoByWeek.Click += new System.EventHandler(this.rdoButton_Click);
            // 
            // rdoByMonth
            // 
            this.rdoByMonth.AutoSize = true;
            this.rdoByMonth.Location = new System.Drawing.Point(119, 9);
            this.rdoByMonth.Name = "rdoByMonth";
            this.rdoByMonth.Size = new System.Drawing.Size(60, 18);
            this.rdoByMonth.TabIndex = 7;
            this.rdoByMonth.Text = "Month";
            this.rdoByMonth.UseVisualStyleBackColor = true;
            this.rdoByMonth.Click += new System.EventHandler(this.rdoButton_Click);
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(5, 34);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(84, 22);
            this.dtpStart.TabIndex = 8;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(95, 34);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(84, 22);
            this.dtpEnd.TabIndex = 9;
            // 
            // DUCPeriodPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DUCPeriodPicker";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(194, 74);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoByDay;
        private System.Windows.Forms.RadioButton rdoByWeek;
        private System.Windows.Forms.RadioButton rdoByMonth;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;

    }
}
