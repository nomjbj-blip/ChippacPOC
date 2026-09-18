namespace DACrux.TEST.ENGUI
{
    partial class TPUCGallery
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonLast = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonFirst = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonRedraw = new System.Windows.Forms.Button();
            this.comboBoxCol = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.ultraProgressBar = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.labelStatus = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panelMap = new System.Windows.Forms.Panel();
            this.panelSingle = new System.Windows.Forms.Panel();
            this.chMapView = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.OldLace;
            this.panel1.Controls.Add(this.chMapView);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.buttonClose);
            this.panel1.Controls.Add(this.buttonLast);
            this.panel1.Controls.Add(this.buttonRight);
            this.panel1.Controls.Add(this.buttonLeft);
            this.panel1.Controls.Add(this.buttonFirst);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.buttonRedraw);
            this.panel1.Controls.Add(this.comboBoxCol);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(659, 20);
            this.panel1.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(288, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(8, 20);
            this.panel4.TabIndex = 9;
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.SystemColors.Control;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Location = new System.Drawing.Point(639, 0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(20, 20);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "x";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonLast
            // 
            this.buttonLast.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLast.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonLast.Enabled = false;
            this.buttonLast.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonLast.Location = new System.Drawing.Point(256, 0);
            this.buttonLast.Name = "buttonLast";
            this.buttonLast.Size = new System.Drawing.Size(32, 20);
            this.buttonLast.TabIndex = 6;
            this.buttonLast.Text = ">>";
            this.buttonLast.UseVisualStyleBackColor = false;
            this.buttonLast.Click += new System.EventHandler(this.buttonLast_Click);
            // 
            // buttonRight
            // 
            this.buttonRight.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRight.Enabled = false;
            this.buttonRight.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonRight.Location = new System.Drawing.Point(224, 0);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(32, 20);
            this.buttonRight.TabIndex = 5;
            this.buttonRight.Text = ">";
            this.buttonRight.UseVisualStyleBackColor = false;
            this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
            // 
            // buttonLeft
            // 
            this.buttonLeft.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonLeft.Enabled = false;
            this.buttonLeft.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonLeft.Location = new System.Drawing.Point(192, 0);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(32, 20);
            this.buttonLeft.TabIndex = 4;
            this.buttonLeft.Text = "<";
            this.buttonLeft.UseVisualStyleBackColor = false;
            this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
            // 
            // buttonFirst
            // 
            this.buttonFirst.BackColor = System.Drawing.SystemColors.Control;
            this.buttonFirst.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonFirst.Enabled = false;
            this.buttonFirst.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonFirst.Location = new System.Drawing.Point(160, 0);
            this.buttonFirst.Name = "buttonFirst";
            this.buttonFirst.Size = new System.Drawing.Size(32, 20);
            this.buttonFirst.TabIndex = 3;
            this.buttonFirst.Text = "<<";
            this.buttonFirst.UseVisualStyleBackColor = false;
            this.buttonFirst.Click += new System.EventHandler(this.buttonFirst_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(152, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(8, 20);
            this.panel3.TabIndex = 2;
            // 
            // buttonRedraw
            // 
            this.buttonRedraw.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRedraw.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRedraw.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonRedraw.Location = new System.Drawing.Point(96, 0);
            this.buttonRedraw.Name = "buttonRedraw";
            this.buttonRedraw.Size = new System.Drawing.Size(56, 20);
            this.buttonRedraw.TabIndex = 1;
            this.buttonRedraw.Text = "Redraw";
            this.buttonRedraw.UseVisualStyleBackColor = false;
            this.buttonRedraw.Click += new System.EventHandler(this.buttonRedraw_Click);
            // 
            // comboBoxCol
            // 
            this.comboBoxCol.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBoxCol.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.comboBoxCol.Location = new System.Drawing.Point(56, 0);
            this.comboBoxCol.Name = "comboBoxCol";
            this.comboBoxCol.Size = new System.Drawing.Size(40, 20);
            this.comboBoxCol.TabIndex = 0;
            this.comboBoxCol.Text = "5";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Col Cnt";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonStop
            // 
            this.buttonStop.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonStop.Location = new System.Drawing.Point(611, 0);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(48, 24);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "중지";
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // ultraProgressBar
            // 
            this.ultraProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraProgressBar.Location = new System.Drawing.Point(144, 0);
            this.ultraProgressBar.Name = "ultraProgressBar";
            this.ultraProgressBar.Size = new System.Drawing.Size(467, 24);
            this.ultraProgressBar.TabIndex = 0;
            this.ultraProgressBar.Text = "[Formatted]";
            // 
            // labelStatus
            // 
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStatus.Location = new System.Drawing.Point(0, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(144, 24);
            this.labelStatus.TabIndex = 1;
            this.labelStatus.Text = "labelStatus";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ultraProgressBar);
            this.panel2.Controls.Add(this.buttonStop);
            this.panel2.Controls.Add(this.labelStatus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 363);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(659, 24);
            this.panel2.TabIndex = 6;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // panelMap
            // 
            this.panelMap.AutoScroll = true;
            this.panelMap.BackColor = System.Drawing.Color.White;
            this.panelMap.Location = new System.Drawing.Point(16, 32);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(120, 104);
            this.panelMap.TabIndex = 5;
            this.panelMap.StyleChanged += new System.EventHandler(this.panelMap_SizeChanged);
            // 
            // panelSingle
            // 
            this.panelSingle.AutoScroll = true;
            this.panelSingle.BackColor = System.Drawing.Color.White;
            this.panelSingle.Location = new System.Drawing.Point(144, 32);
            this.panelSingle.Name = "panelSingle";
            this.panelSingle.Size = new System.Drawing.Size(120, 104);
            this.panelSingle.TabIndex = 7;
            // 
            // chMapView
            // 
            this.chMapView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chMapView.AutoSize = true;
            this.chMapView.Checked = true;
            this.chMapView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chMapView.Location = new System.Drawing.Point(518, 2);
            this.chMapView.Name = "chMapView";
            this.chMapView.Size = new System.Drawing.Size(117, 16);
            this.chMapView.TabIndex = 10;
            this.chMapView.Text = "Map View Mode";
            this.chMapView.UseVisualStyleBackColor = true;
            // 
            // TPUCGallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.panelSingle);
            this.Name = "TPUCGallery";
            this.Size = new System.Drawing.Size(659, 387);
            this.Load += new System.EventHandler(this.Gallery_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chMapView;

        
    }
}
