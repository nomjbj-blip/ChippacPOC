namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>DACrux MDI 화면에서 GDS Map 조회 Form을 표시할 영역을 디자이너에 정의한다.</summary>
    partial class frmGdsMapView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlGdsMap;

        /// <summary>호스트 화면이 닫힐 때 자식 GDS Form과 컨트롤을 함께 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>GDS 조회 Form이 DACrux 작업 영역을 채우도록 Panel을 배치한다.</summary>
        private void InitializeComponent()
        {
            this.pnlGdsMap = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            //
            // pnlGdsMap
            //
            this.pnlGdsMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGdsMap.Location = new System.Drawing.Point(0, 0);
            this.pnlGdsMap.Name = "pnlGdsMap";
            this.pnlGdsMap.Size = new System.Drawing.Size(1000, 700);
            this.pnlGdsMap.TabIndex = 0;
            //
            // frmGdsMapView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.pnlGdsMap);
            this.Name = "frmGdsMapView";
            this.Text = "GDS Map 조회";
            this.ResumeLayout(false);
        }
    }
}
