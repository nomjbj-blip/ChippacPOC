using System;
namespace DACrux.Map
{
	partial class WaferMap
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
		if (disposing)
			{
				if (components != null)
				{
					if (m_arrDies != null)
					{
						m_arrDies.Clear();
						m_arrDies = null;
					}

					if (m_DieIndexer != null)
					{
						m_DieIndexer.Clear();
						m_DieIndexer = null;
					}

					if (m_hbSelectBrush != null) m_hbSelectBrush.Dispose();

					if (m_gdiMain != null) m_gdiMain.Dispose();
					if (m_gdiTempMap != null) m_gdiTempMap.Dispose();
					if (m_bmpTemp != null) m_bmpTemp.Dispose();
					if (m_bmpWaferMap != null) m_bmpWaferMap.Dispose();
					if (ctxmWaferMap != null) ctxmWaferMap.Dispose();
					if (m_DT != null) m_DT.Dispose();
					m_ColorSet = null;

					components.Dispose();
					GC.Collect();
				}
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
            this.ctxmWaferMap = new System.Windows.Forms.ContextMenu();
            this.mnuitemMAPMODE_FREEZOOM = new System.Windows.Forms.MenuItem();
            this.mnuitemMAPMODE_FITSIZE = new System.Windows.Forms.MenuItem();
            this.mnuitemMOUSEDRAGMODE_ROTATE = new System.Windows.Forms.MenuItem();
            this.mnuitemMOUSEDRAGMODE_ZONE = new System.Windows.Forms.MenuItem();
            this.mnuitemMAPSELECTSTYLE_CIRCLE = new System.Windows.Forms.MenuItem();
            this.mnuitemMAPSELECTSTYLE_PIE = new System.Windows.Forms.MenuItem();
            this.mnuitemMAPSELECTSTYLE_RECTANGLE = new System.Windows.Forms.MenuItem();
            this.mnuitemMAPSELECTSTYLE_FREEHAND = new System.Windows.Forms.MenuItem();
            this.mnuitemMAPSELECTSTYLE_BAND = new System.Windows.Forms.MenuItem();
            this.mnuitemMOUSEDRAGMODE_MOVE = new System.Windows.Forms.MenuItem();
            this.mnuitemVISIBLE_MARKDIE = new System.Windows.Forms.MenuItem();
            this.mnuitemVISIBLE_SHOT = new System.Windows.Forms.MenuItem();
            this.mnuitem_SPRIT1 = new System.Windows.Forms.MenuItem();
            this.menuitemMAPMODE_ZOOMIN = new System.Windows.Forms.MenuItem();
            this.menuitemMAPMODE_ZOOMOUT = new System.Windows.Forms.MenuItem();
            this.mnuitem_SPRIT2 = new System.Windows.Forms.MenuItem();
            this.mnuitemCLEAR_SELECTEDDIE = new System.Windows.Forms.MenuItem();
            this.menuItemShotOption = new System.Windows.Forms.MenuItem();
            this.mnuitem_SPRIT3 = new System.Windows.Forms.MenuItem();
            this.mnuCOPYTOCLIP = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // ctxmWaferMap
            // 
            this.ctxmWaferMap.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuitemMAPMODE_FREEZOOM,
            this.mnuitemMAPMODE_FITSIZE,
            this.mnuitemMOUSEDRAGMODE_ROTATE,
            this.mnuitemMOUSEDRAGMODE_ZONE,
            this.mnuitemMOUSEDRAGMODE_MOVE,
            this.mnuitemVISIBLE_MARKDIE,
            this.mnuitemVISIBLE_SHOT,
            this.mnuitem_SPRIT1,
            this.menuitemMAPMODE_ZOOMIN,
            this.menuitemMAPMODE_ZOOMOUT,
            this.mnuitem_SPRIT2,
            this.mnuitemCLEAR_SELECTEDDIE,
            this.menuItemShotOption,
            this.mnuitem_SPRIT3,
            this.mnuCOPYTOCLIP});
            this.ctxmWaferMap.Popup += new System.EventHandler(this.ctxmWaferMap_Popup);
            // 
            // mnuitemMAPMODE_FREEZOOM
            // 
            this.mnuitemMAPMODE_FREEZOOM.Index = 0;
            this.mnuitemMAPMODE_FREEZOOM.Text = "Free Zoom";
            this.mnuitemMAPMODE_FREEZOOM.Click += new System.EventHandler(this.mnuitemMAPMODE_FREEZOOM_Click);
            // 
            // mnuitemMAPMODE_FITSIZE
            // 
            this.mnuitemMAPMODE_FITSIZE.Index = 1;
            this.mnuitemMAPMODE_FITSIZE.Text = "Fit Size";
            this.mnuitemMAPMODE_FITSIZE.Click += new System.EventHandler(this.mnuitemMAPMODE_FITSIZE_Click);
            // 
            // mnuitemMOUSEDRAGMODE_ROTATE
            // 
            this.mnuitemMOUSEDRAGMODE_ROTATE.Index = 2;
            this.mnuitemMOUSEDRAGMODE_ROTATE.Text = "Rotate Notch";
            this.mnuitemMOUSEDRAGMODE_ROTATE.Click += new System.EventHandler(this.mnuitemMOUSEDRAGMODE_ROTATE_Click);
            // 
            // mnuitemMOUSEDRAGMODE_ZONE
            // 
            this.mnuitemMOUSEDRAGMODE_ZONE.Index = 3;
            this.mnuitemMOUSEDRAGMODE_ZONE.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuitemMAPSELECTSTYLE_CIRCLE,
            this.mnuitemMAPSELECTSTYLE_PIE,
            this.mnuitemMAPSELECTSTYLE_RECTANGLE,
            this.mnuitemMAPSELECTSTYLE_FREEHAND,
            this.mnuitemMAPSELECTSTYLE_BAND});
            this.mnuitemMOUSEDRAGMODE_ZONE.Text = "Zone Select";
            this.mnuitemMOUSEDRAGMODE_ZONE.Click += new System.EventHandler(this.mnuitemMOUSEDRAGMODE_ZONE_Click);
            // 
            // mnuitemMAPSELECTSTYLE_CIRCLE
            // 
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Index = 0;
            this.mnuitemMAPSELECTSTYLE_CIRCLE.MergeOrder = 31;
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Text = "Circle";
            this.mnuitemMAPSELECTSTYLE_CIRCLE.Click += new System.EventHandler(this.mnuitemMAPSELECTSTYLE_CIRCLE_Click);
            // 
            // mnuitemMAPSELECTSTYLE_PIE
            // 
            this.mnuitemMAPSELECTSTYLE_PIE.Index = 1;
            this.mnuitemMAPSELECTSTYLE_PIE.MergeOrder = 32;
            this.mnuitemMAPSELECTSTYLE_PIE.Text = "Pie";
            this.mnuitemMAPSELECTSTYLE_PIE.Click += new System.EventHandler(this.mnuitemMAPSELECTSTYLE_PIE_Click);
            // 
            // mnuitemMAPSELECTSTYLE_RECTANGLE
            // 
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Checked = true;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Index = 2;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.MergeOrder = 33;
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Text = "Rectangle";
            this.mnuitemMAPSELECTSTYLE_RECTANGLE.Click += new System.EventHandler(this.mnuitemMAPSELECTSTYLE_RECTANGLE_Click);
            // 
            // mnuitemMAPSELECTSTYLE_FREEHAND
            // 
            this.mnuitemMAPSELECTSTYLE_FREEHAND.Index = 3;
            this.mnuitemMAPSELECTSTYLE_FREEHAND.MergeOrder = 34;
            this.mnuitemMAPSELECTSTYLE_FREEHAND.Text = "Free Hand";
            this.mnuitemMAPSELECTSTYLE_FREEHAND.Click += new System.EventHandler(this.mnuitemMAPSELECTSTYLE_FREEHAND_Click);
            // 
            // mnuitemMAPSELECTSTYLE_BAND
            // 
            this.mnuitemMAPSELECTSTYLE_BAND.Index = 4;
            this.mnuitemMAPSELECTSTYLE_BAND.MergeOrder = 35;
            this.mnuitemMAPSELECTSTYLE_BAND.Text = "Band";
            this.mnuitemMAPSELECTSTYLE_BAND.Click += new System.EventHandler(this.mnuitemMAPSELECTSTYLE_BAND_Click);
            // 
            // mnuitemMOUSEDRAGMODE_MOVE
            // 
            this.mnuitemMOUSEDRAGMODE_MOVE.Index = 4;
            this.mnuitemMOUSEDRAGMODE_MOVE.Text = "Move View Center";
            this.mnuitemMOUSEDRAGMODE_MOVE.Click += new System.EventHandler(this.mnuitemMOUSEDRAGMODE_MOVE_Click);
            // 
            // mnuitemVISIBLE_MARKDIE
            // 
            this.mnuitemVISIBLE_MARKDIE.Index = 5;
            this.mnuitemVISIBLE_MARKDIE.Text = "Visible Mark Die";
            this.mnuitemVISIBLE_MARKDIE.Click += new System.EventHandler(this.mnuitemVISIBLE_MARKDIE_Click);
            // 
            // mnuitemVISIBLE_SHOT
            // 
            this.mnuitemVISIBLE_SHOT.Index = 6;
            this.mnuitemVISIBLE_SHOT.Text = "Visible Shot";
            this.mnuitemVISIBLE_SHOT.Click += new System.EventHandler(this.mnuitemVISIBLE_SHOT_Click);
            // 
            // mnuitem_SPRIT1
            // 
            this.mnuitem_SPRIT1.Index = 7;
            this.mnuitem_SPRIT1.Text = "-";
            // 
            // menuitemMAPMODE_ZOOMIN
            // 
            this.menuitemMAPMODE_ZOOMIN.Index = 8;
            this.menuitemMAPMODE_ZOOMIN.Text = "5% Zoom In";
            this.menuitemMAPMODE_ZOOMIN.Click += new System.EventHandler(this.menuitemMAPMODE_ZOOMIN_Click);
            // 
            // menuitemMAPMODE_ZOOMOUT
            // 
            this.menuitemMAPMODE_ZOOMOUT.Index = 9;
            this.menuitemMAPMODE_ZOOMOUT.Text = "5% Zoom Out";
            this.menuitemMAPMODE_ZOOMOUT.Click += new System.EventHandler(this.menuitemMAPMODE_ZOOMOUT_Click);
            // 
            // mnuitem_SPRIT2
            // 
            this.mnuitem_SPRIT2.Index = 10;
            this.mnuitem_SPRIT2.Text = "-";
            // 
            // mnuitemCLEAR_SELECTEDDIE
            // 
            this.mnuitemCLEAR_SELECTEDDIE.Index = 11;
            this.mnuitemCLEAR_SELECTEDDIE.Text = "Clear Selected Die";
            this.mnuitemCLEAR_SELECTEDDIE.Click += new System.EventHandler(this.mnuitemCLEAR_SELECTEDDIE_Click);
            // 
            // menuItemShotOption
            // 
            this.menuItemShotOption.Index = 12;
            this.menuItemShotOption.Text = "Shot Option...";
            this.menuItemShotOption.Click += new System.EventHandler(this.menuItemShotOption_Click);
            // 
            // mnuitem_SPRIT3
            // 
            this.mnuitem_SPRIT3.Index = 13;
            this.mnuitem_SPRIT3.Text = "-";
            // 
            // mnuCOPYTOCLIP
            // 
            this.mnuCOPYTOCLIP.Index = 14;
            this.mnuCOPYTOCLIP.Text = "Copy To Clipboard";
            this.mnuCOPYTOCLIP.Click += new System.EventHandler(this.mnuCOPYTOCLIP_Click);
            // 
            // WaferMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.ForeColor = System.Drawing.Color.Red;
            this.Name = "WaferMap";
            this.Size = new System.Drawing.Size(403, 375);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.WaferMap_Paint);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.WaferMap_MouseDoubleClick);
            this.ResumeLayout(false);

		}

		#endregion

        public System.Windows.Forms.MenuItem mnuitemMAPMODE_FREEZOOM;
        public System.Windows.Forms.MenuItem mnuitemMAPMODE_FITSIZE;
        public System.Windows.Forms.MenuItem mnuitemMOUSEDRAGMODE_ROTATE;
        public System.Windows.Forms.MenuItem mnuitemMOUSEDRAGMODE_ZONE;
        public System.Windows.Forms.MenuItem mnuitemMOUSEDRAGMODE_MOVE;
        public System.Windows.Forms.MenuItem menuitemMAPMODE_ZOOMIN;
        public System.Windows.Forms.MenuItem menuitemMAPMODE_ZOOMOUT;
        public System.Windows.Forms.MenuItem mnuitem_SPRIT1;
        public System.Windows.Forms.MenuItem mnuitem_SPRIT2;
        public System.Windows.Forms.MenuItem mnuitem_SPRIT3;
        public System.Windows.Forms.MenuItem mnuCOPYTOCLIP;
        public System.Windows.Forms.MenuItem mnuitemCLEAR_SELECTEDDIE;
        public System.Windows.Forms.MenuItem mnuitemVISIBLE_MARKDIE;
        public System.Windows.Forms.MenuItem mnuitemVISIBLE_SHOT;
        public System.Windows.Forms.MenuItem menuItemShotOption;

    }
}
