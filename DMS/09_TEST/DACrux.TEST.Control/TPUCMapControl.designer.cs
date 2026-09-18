using System.Windows.Forms;
namespace DACrux.MapAnalysis.Control
{
    partial class TPUCMapControl : UserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPUCMapControl));
            this.pnlWaferMapInfo = new System.Windows.Forms.Panel();
            this.chkboxSelBin = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.txtDieNum = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.lbY = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.lbX = new System.Windows.Forms.Label();
            this.map = new DACrux.Map.WaferMap();
            this.pnlWaferMap = new System.Windows.Forms.Panel();
            this.txtBin = new System.Windows.Forms.TextBox();
            this.lbBin = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlWaferMapInfo.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.pnlWaferMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlWaferMapInfo
            // 
            this.pnlWaferMapInfo.Controls.Add(this.chkboxSelBin);
            this.pnlWaferMapInfo.Controls.Add(this.label1);
            this.pnlWaferMapInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWaferMapInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlWaferMapInfo.Name = "pnlWaferMapInfo";
            this.pnlWaferMapInfo.Padding = new System.Windows.Forms.Padding(2);
            this.pnlWaferMapInfo.Size = new System.Drawing.Size(602, 616);
            this.pnlWaferMapInfo.TabIndex = 12;
            // 
            // chkboxSelBin
            // 
            this.chkboxSelBin.CheckOnClick = true;
            this.chkboxSelBin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkboxSelBin.IntegralHeight = false;
            this.chkboxSelBin.Location = new System.Drawing.Point(2, 22);
            this.chkboxSelBin.Name = "chkboxSelBin";
            this.chkboxSelBin.Size = new System.Drawing.Size(598, 592);
            this.chkboxSelBin.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSlateGray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(598, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "    Bin Select";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlInfo.Controls.Add(this.txtValue);
            this.pnlInfo.Controls.Add(this.label2);
            this.pnlInfo.Controls.Add(this.txtBin);
            this.pnlInfo.Controls.Add(this.lbBin);
            this.pnlInfo.Controls.Add(this.txtDieNum);
            this.pnlInfo.Controls.Add(this.label6);
            this.pnlInfo.Controls.Add(this.txtY);
            this.pnlInfo.Controls.Add(this.lbY);
            this.pnlInfo.Controls.Add(this.txtX);
            this.pnlInfo.Controls.Add(this.lbX);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(602, 21);
            this.pnlInfo.TabIndex = 3;
            // 
            // txtDieNum
            // 
            this.txtDieNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDieNum.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtDieNum.Location = new System.Drawing.Point(143, 0);
            this.txtDieNum.Name = "txtDieNum";
            this.txtDieNum.ReadOnly = true;
            this.txtDieNum.Size = new System.Drawing.Size(40, 21);
            this.txtDieNum.TabIndex = 20;
            // 
            // txtY
            // 
            this.txtY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtY.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtY.Location = new System.Drawing.Point(70, 0);
            this.txtY.Name = "txtY";
            this.txtY.ReadOnly = true;
            this.txtY.Size = new System.Drawing.Size(40, 21);
            this.txtY.TabIndex = 0;
            // 
            // lbY
            // 
            this.lbY.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbY.ForeColor = System.Drawing.Color.White;
            this.lbY.Location = new System.Drawing.Point(55, 0);
            this.lbY.Name = "lbY";
            this.lbY.Size = new System.Drawing.Size(15, 21);
            this.lbY.TabIndex = 11;
            this.lbY.Text = "Y";
            this.lbY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtX
            // 
            this.txtX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtX.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtX.Location = new System.Drawing.Point(15, 0);
            this.txtX.Name = "txtX";
            this.txtX.ReadOnly = true;
            this.txtX.Size = new System.Drawing.Size(40, 21);
            this.txtX.TabIndex = 0;
            // 
            // lbX
            // 
            this.lbX.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbX.ForeColor = System.Drawing.Color.White;
            this.lbX.Location = new System.Drawing.Point(0, 0);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(15, 21);
            this.lbX.TabIndex = 12;
            this.lbX.Text = "X";
            this.lbX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.BackColor = System.Drawing.SystemColors.Control;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DieBorderColor = System.Drawing.Color.LightGray;
            this.map.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.map.DieMaxX = 0;
            this.map.DieMaxY = 0;
            this.map.DieMinX = 0;
            this.map.DieMinY = 0;
            this.map.DieSizeX = 0.01D;
            this.map.DieSizeY = 0.01D;
            this.map.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.map.DisplayValue = "BIN";
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.DrawFirstDie = true;
            this.map.DrawMarkDie = false;
            this.map.DrawOriginDie = true;
            this.map.DrawSkipDie = true;
            this.map.EdgeColor = System.Drawing.Color.LightGray;
            this.map.EdgeSize = 1D;
            this.map.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.map.FirstDieX = 0;
            this.map.FirstDieY = 0;
            this.map.ForeColor = System.Drawing.Color.Red;
            this.map.FromGradationDieColor = System.Drawing.Color.Lime;
            this.map.GradationInterval = 5;
            this.map.GradationMaxValue = double.NaN;
            this.map.GradationMinValue = double.NaN;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.map.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.map.Name = "map";
            this.map.NotchAngle = 0;
            this.map.NotchType = DACrux.Base.Notch.Notch;
            this.map.OriginDieBorder = System.Drawing.Color.Red;
            this.map.OriginIndexX = 0;
            this.map.OriginIndexY = 0;
            this.map.OriginX = 0D;
            this.map.OriginY = 0D;
            this.map.ParametricColumn = "PCMVALUE";
            this.map.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.map.PickupDieAlpha = 96;
            this.map.PickupedDieColor = System.Drawing.Color.Transparent;
            this.map.PopupMenu = true;
            this.map.ReferenceDieSetting = 0;
            this.map.ScaleMark = false;
            this.map.SelecetedBin = "ALL";
            this.map.SelectedVI = "ALL";
            this.map.ShotLineWidth = 2;
            this.map.Size = new System.Drawing.Size(602, 595);
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 4;
            this.map.ToGradationDieColor = System.Drawing.Color.Red;
            this.map.TransParent = 255;
            this.map.ViewAngle = 0;
            this.map.VIMember = "VIFAIL";
            this.map.VisibleDieBorder = true;
            this.map.VisibleDieValue = false;
            this.map.VisibleFocusDie = false;
            this.map.VisibleInfomation = true;
            this.map.VisibleOffDie = false;
            this.map.VisibleStringBin = false;
            this.map.VisibleVIFail = true;
            this.map.VisibleXY = false;
            this.map.WaferBorderColor = System.Drawing.Color.LightGray;
            this.map.WaferColor = System.Drawing.Color.Gray;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.map.WaferMargin = 0.95D;
            this.map.WaferSize = 200D;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.map.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.m_wMap_OnChangeCurrentDie);
            // 
            // pnlWaferMap
            // 
            this.pnlWaferMap.Controls.Add(this.map);
            this.pnlWaferMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWaferMap.Location = new System.Drawing.Point(0, 21);
            this.pnlWaferMap.Name = "pnlWaferMap";
            this.pnlWaferMap.Size = new System.Drawing.Size(602, 595);
            this.pnlWaferMap.TabIndex = 15;
            // 
            // txtBin
            // 
            this.txtBin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtBin.Location = new System.Drawing.Point(216, 0);
            this.txtBin.Name = "txtBin";
            this.txtBin.ReadOnly = true;
            this.txtBin.Size = new System.Drawing.Size(40, 21);
            this.txtBin.TabIndex = 25;
            // 
            // lbBin
            // 
            this.lbBin.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBin.ForeColor = System.Drawing.Color.White;
            this.lbBin.Location = new System.Drawing.Point(183, 0);
            this.lbBin.Name = "lbBin";
            this.lbBin.Size = new System.Drawing.Size(33, 21);
            this.lbBin.TabIndex = 26;
            this.lbBin.Text = "BIN";
            this.lbBin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(110, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 21);
            this.label6.TabIndex = 21;
            this.label6.Text = "NUM";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtValue
            // 
            this.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtValue.Location = new System.Drawing.Point(319, 0);
            this.txtValue.Name = "txtValue";
            this.txtValue.ReadOnly = true;
            this.txtValue.Size = new System.Drawing.Size(60, 21);
            this.txtValue.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(256, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 21);
            this.label2.TabIndex = 28;
            this.label2.Text = "VALUE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TPUCMapControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlWaferMap);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.pnlWaferMapInfo);
            this.Name = "TPUCMapControl";
            this.Size = new System.Drawing.Size(602, 616);
            this.pnlWaferMapInfo.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.pnlWaferMap.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lbX;
        private System.Windows.Forms.Label lbY;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.TextBox txtY;
        private DACrux.Map.WaferMap map;
        private System.Windows.Forms.Panel pnlWaferMapInfo;
        private System.Windows.Forms.CheckedListBox chkboxSelBin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlWaferMap;
        private System.Windows.Forms.TextBox txtDieNum;
        private TextBox txtBin;
        private Label lbBin;
        private TextBox txtValue;
        private Label label2;
        private Label label6;
    }
}
