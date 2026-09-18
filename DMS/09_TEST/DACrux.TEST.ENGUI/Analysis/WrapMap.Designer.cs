namespace DACrux.TEST.ENGUI
{
    partial class WrapMap
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
            this.waferMap = new DACrux.Map.WaferMap();
            this.SuspendLayout();
            // 
            // waferMap
            // 
            this.waferMap.AngleOffSet = 0;
            this.waferMap.BackColor = System.Drawing.SystemColors.Control;
            this.waferMap.CenterMark = false;
            this.waferMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.waferMap.DataSource = null;
            this.waferMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.waferMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.waferMap.DieMaxX = 0;
            this.waferMap.DieMaxY = 0;
            this.waferMap.DieMinX = 0;
            this.waferMap.DieMinY = 0;
            this.waferMap.DieSizeX = 0.01D;
            this.waferMap.DieSizeY = 0.01D;
            this.waferMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.waferMap.DisplayValue = "BIN";
            this.waferMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.waferMap.DrawFirstDie = true;
            this.waferMap.DrawMarkDie = false;
            this.waferMap.DrawOriginDie = true;
            this.waferMap.DrawSkipDie = true;
            this.waferMap.EdgeColor = System.Drawing.Color.LightGray;
            this.waferMap.EdgeSize = 1D;
            this.waferMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.waferMap.FirstDieX = 0;
            this.waferMap.FirstDieY = 0;
            this.waferMap.ForeColor = System.Drawing.Color.Red;
            this.waferMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.waferMap.GradationInterval = 5;
            this.waferMap.GradationMaxValue = double.NaN;
            this.waferMap.GradationMinValue = double.NaN;
            this.waferMap.Location = new System.Drawing.Point(0, 0);
            this.waferMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.waferMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.waferMap.Name = "waferMap";
            this.waferMap.NotchAngle = 0;
            this.waferMap.NotchType = DACrux.Base.Notch.Notch;
            this.waferMap.OriginDieBorder = System.Drawing.Color.Red;
            this.waferMap.OriginIndexX = 0;
            this.waferMap.OriginIndexY = 0;
            this.waferMap.OriginX = 0D;
            this.waferMap.OriginY = 0D;
            this.waferMap.ParaLimit = false;
            this.waferMap.ParametricColumn = "PCMVALUE";
            this.waferMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.waferMap.PickupDieAlpha = 96;
            this.waferMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.waferMap.PopupMenu = true;
            this.waferMap.ReferenceDieSetting = 0;
            this.waferMap.ScaleMark = false;
            this.waferMap.SelecetedBin = "ALL";
            this.waferMap.SelectedVI = "ALL";
            this.waferMap.Size = new System.Drawing.Size(544, 459);
            this.waferMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.waferMap.TabIndex = 5;
            this.waferMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.waferMap.TransParent = 255;
            this.waferMap.ViewAngle = 0;
            this.waferMap.VIMember = "VIFAIL";
            this.waferMap.VisibleDieBorder = true;
            this.waferMap.VisibleDieValue = false;
            this.waferMap.VisibleFocusDie = false;
            this.waferMap.VisibleInfomation = true;
            this.waferMap.VisibleOffDie = false;
            this.waferMap.VisibleShotAlignPoint = false;
            this.waferMap.VisibleSignDies = false;
            this.waferMap.VisibleStringBin = false;
            this.waferMap.VisibleVIFail = true;
            this.waferMap.VisibleXY = false;
            this.waferMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.waferMap.WaferColor = System.Drawing.Color.Gray;
            this.waferMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.waferMap.WaferID = "";
            this.waferMap.WaferMargin = 0.95D;
            this.waferMap.WaferSize = 200D;
            this.waferMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.waferMap.Load += new System.EventHandler(this.waferMap_Load);
            this.waferMap.DoubleClick += new System.EventHandler(this.waferMap_DoubleClick);
            this.waferMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.waferMap_MouseDown);
            this.waferMap.MouseEnter += new System.EventHandler(this.waferMap_MouseEnter);
            this.waferMap.MouseLeave += new System.EventHandler(this.waferMap_MouseLeave);
            // 
            // WrapMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.waferMap);
            this.Name = "WrapMap";
            this.Size = new System.Drawing.Size(544, 459);
            this.MouseEnter += new System.EventHandler(this.Map_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Map_MouseLeave);
            this.ResumeLayout(false);

        }

        #endregion

        private Map.WaferMap waferMap;

    }
}
