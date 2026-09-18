namespace DACrux.SP.Controls
{
    partial class uclPeriodPicker
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
            this.grpPeriod = new System.Windows.Forms.GroupBox();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.rdoByPeriod = new System.Windows.Forms.RadioButton();
            this.rdoByMonth = new System.Windows.Forms.RadioButton();
            this.rdoByWeek = new System.Windows.Forms.RadioButton();
            this.rdoByDay = new System.Windows.Forms.RadioButton();
            this.lblTild = new System.Windows.Forms.Label();
            this.uclMonthPicker1 = new DACrux.SP.Controls.uclMonthPicker();
            this.uclWeekPicker1 = new DACrux.SP.Controls.uclWeekPicker();
            this.grpPeriod.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPeriod
            // 
            this.grpPeriod.Controls.Add(this.dtpEnd);
            this.grpPeriod.Controls.Add(this.dtpStart);
            this.grpPeriod.Controls.Add(this.rdoByPeriod);
            this.grpPeriod.Controls.Add(this.rdoByMonth);
            this.grpPeriod.Controls.Add(this.rdoByWeek);
            this.grpPeriod.Controls.Add(this.rdoByDay);
            this.grpPeriod.Controls.Add(this.lblTild);
            this.grpPeriod.Controls.Add(this.uclMonthPicker1);
            this.grpPeriod.Controls.Add(this.uclWeekPicker1);
            this.grpPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPeriod.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPeriod.Location = new System.Drawing.Point(0, 0);
            this.grpPeriod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpPeriod.Name = "grpPeriod";
            this.grpPeriod.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpPeriod.Size = new System.Drawing.Size(260, 80);
            this.grpPeriod.TabIndex = 2;
            this.grpPeriod.TabStop = false;
            this.grpPeriod.Text = "Select Period";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(116, 47);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(101, 21);
            this.dtpEnd.TabIndex = 5;
            this.dtpEnd.ValueChanged += new System.EventHandler(this.dtpEnd_ValueChanged);
            // 
            // dtpStart
            // 
            this.dtpStart.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(12, 47);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(90, 21);
            this.dtpStart.TabIndex = 4;
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtpStart_ValueChanged);
            // 
            // rdoByPeriod
            // 
            this.rdoByPeriod.AutoSize = true;
            this.rdoByPeriod.Checked = true;
            this.rdoByPeriod.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoByPeriod.Location = new System.Drawing.Point(192, 22);
            this.rdoByPeriod.Name = "rdoByPeriod";
            this.rdoByPeriod.Size = new System.Drawing.Size(61, 19);
            this.rdoByPeriod.TabIndex = 3;
            this.rdoByPeriod.TabStop = true;
            this.rdoByPeriod.Text = "Period";
            this.rdoByPeriod.UseVisualStyleBackColor = true;
            this.rdoByPeriod.CheckedChanged += new System.EventHandler(this.rdoByPeriod_CheckedChanged);
            // 
            // rdoByMonth
            // 
            this.rdoByMonth.AutoSize = true;
            this.rdoByMonth.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoByMonth.Location = new System.Drawing.Point(126, 22);
            this.rdoByMonth.Name = "rdoByMonth";
            this.rdoByMonth.Size = new System.Drawing.Size(58, 19);
            this.rdoByMonth.TabIndex = 2;
            this.rdoByMonth.Text = "Month";
            this.rdoByMonth.UseVisualStyleBackColor = true;
            this.rdoByMonth.CheckedChanged += new System.EventHandler(this.rdoByMonth_CheckedChanged);
            // 
            // rdoByWeek
            // 
            this.rdoByWeek.AutoSize = true;
            this.rdoByWeek.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoByWeek.Location = new System.Drawing.Point(63, 22);
            this.rdoByWeek.Name = "rdoByWeek";
            this.rdoByWeek.Size = new System.Drawing.Size(56, 19);
            this.rdoByWeek.TabIndex = 1;
            this.rdoByWeek.Text = "Week";
            this.rdoByWeek.UseVisualStyleBackColor = true;
            this.rdoByWeek.CheckedChanged += new System.EventHandler(this.rdoByWeek_CheckedChanged);
            // 
            // rdoByDay
            // 
            this.rdoByDay.AutoSize = true;
            this.rdoByDay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoByDay.Location = new System.Drawing.Point(12, 22);
            this.rdoByDay.Name = "rdoByDay";
            this.rdoByDay.Size = new System.Drawing.Size(46, 19);
            this.rdoByDay.TabIndex = 0;
            this.rdoByDay.Text = "Day";
            this.rdoByDay.UseVisualStyleBackColor = true;
            this.rdoByDay.CheckedChanged += new System.EventHandler(this.rdoByDay_CheckedChanged);
            // 
            // lblTild
            // 
            this.lblTild.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTild.Location = new System.Drawing.Point(101, 48);
            this.lblTild.Name = "lblTild";
            this.lblTild.Size = new System.Drawing.Size(16, 22);
            this.lblTild.TabIndex = 7;
            this.lblTild.Text = "~";
            this.lblTild.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uclMonthPicker1
            // 
            this.uclMonthPicker1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uclMonthPicker1.Location = new System.Drawing.Point(12, 47);
            this.uclMonthPicker1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.uclMonthPicker1.Name = "uclMonthPicker1";
            this.uclMonthPicker1.Size = new System.Drawing.Size(183, 21);
            this.uclMonthPicker1.TabIndex = 0;
            this.uclMonthPicker1.Visible = false;
            this.uclMonthPicker1.MonthValueChanged += new System.EventHandler(this.uclMonthPicker1_MonthValueChanged);
            // 
            // uclWeekPicker1
            // 
            this.uclWeekPicker1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uclWeekPicker1.Location = new System.Drawing.Point(12, 47);
            this.uclWeekPicker1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.uclWeekPicker1.Name = "uclWeekPicker1";
            this.uclWeekPicker1.Size = new System.Drawing.Size(196, 25);
            this.uclWeekPicker1.TabIndex = 1;
            this.uclWeekPicker1.Visible = false;
            this.uclWeekPicker1.WeekValueChanged += new System.EventHandler(this.uclWeekPicker1_WeekValueChanged);
            // 
            // uclPeriodPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.grpPeriod);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "uclPeriodPicker";
            this.Size = new System.Drawing.Size(260, 80);
            this.grpPeriod.ResumeLayout(false);
            this.grpPeriod.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private uclMonthPicker uclMonthPicker1;
        private uclWeekPicker uclWeekPicker1;
        private System.Windows.Forms.GroupBox grpPeriod;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.RadioButton rdoByPeriod;
        private System.Windows.Forms.RadioButton rdoByMonth;
        private System.Windows.Forms.RadioButton rdoByWeek;
        private System.Windows.Forms.RadioButton rdoByDay;
        private System.Windows.Forms.Label lblTild;
    }
}
