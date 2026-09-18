using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DACrux.TEST.Control
{
	/// <summary>
	/// Map에 대한 요약 설명입니다.
	/// </summary>
	public class WrapMap : System.Windows.Forms.UserControl
	{
		bool single = false;

		public delegate void EventHandlerSelected(WrapMap map);

		public event EventHandlerSelected OnSelected;
		public event EventHandlerSelected OnDblClick; 

		bool bSelected = false;
		int mapId = -1;

		private DACrux.Map.WaferMap waferMap;
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WrapMap()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.
		}

		public bool Single
		{
			get
			{
				return single;
			}
			set
			{
				single = value;
			}
		}

		public bool Selected
		{
			get
			{
				return bSelected;
			}
			set
			{
				MapSelected(value);
			}
		}

		public int MapId
		{
			get
			{
				return mapId;
			}
			set
			{
				mapId = value;
			}
		}

		public DACrux.Map.WaferMap MapObj
		{
			get
			{
				return waferMap;
			}
		}

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			waferMap.Dispose();

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
			this.waferMap.CenterMark = false;
			this.waferMap.Cursor = System.Windows.Forms.Cursors.Cross;
			this.waferMap.DataSource = null;
			this.waferMap.DieMaxX = 0;
			this.waferMap.DieMaxY = 0;
			this.waferMap.DieMinX = 0;
			this.waferMap.DieMinY = 0;
			this.waferMap.DieSizeX = 0.01;
			this.waferMap.DieSizeY = 0.01;
			this.waferMap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.waferMap.DrawFirstDie = true;
            this.waferMap.DrawMarkDie = false;
			this.waferMap.DrawOriginDie = true;
			this.waferMap.DrawSkipDie = true;
			this.waferMap.EdgeColor = System.Drawing.Color.LightGray;
			this.waferMap.EdgeSize = 1;
			this.waferMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
			this.waferMap.FirstDieX = 0;
			this.waferMap.FirstDieY = 0;
			this.waferMap.ForeColor = System.Drawing.Color.Red;
			this.waferMap.Location = new System.Drawing.Point(2, 2);
			this.waferMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
			this.waferMap.Name = "waferMap";
			this.waferMap.NotchAngle = 0;
			this.waferMap.NotchType = DACrux.Base.Notch.Flat;
			this.waferMap.OriginDieBorder = System.Drawing.Color.Red;
			this.waferMap.OriginIndexX = 0;
			this.waferMap.OriginIndexY = 0;
			this.waferMap.OriginX = 0;
			this.waferMap.OriginY = 0;
			this.waferMap.ScaleMark = false;
			this.waferMap.SelecetedBin = "ALL";
			this.waferMap.Size = new System.Drawing.Size(284, 260);
			this.waferMap.SkipDieColor = System.Drawing.Color.Yellow;
			this.waferMap.TabIndex = 0;
			this.waferMap.WaferBorderColor = System.Drawing.Color.LightGray;
			this.waferMap.WaferColor = System.Drawing.Color.Gray;
			this.waferMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
			this.waferMap.WaferSize = 200000;
			this.waferMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
			this.waferMap.MouseEnter += new System.EventHandler(this.waferMap_MouseEnter);
			this.waferMap.DoubleClick += new System.EventHandler(this.waferMap_DoubleClick);
			this.waferMap.MouseLeave += new System.EventHandler(this.waferMap_MouseLeave);
			this.waferMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.waferMap_MouseDown);
			// 
			// Map
			// 
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.waferMap);
			this.DockPadding.All = 2;
			this.Name = "Map";
			this.Size = new System.Drawing.Size(288, 264);
			this.MouseEnter += new System.EventHandler(this.Map_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.Map_MouseLeave);
			this.ResumeLayout(false);

		}
		#endregion

		private void waferMap_MouseEnter(object sender, System.EventArgs e)
		{
			if(Single) return;
			if(!bSelected) this.BackColor = Color.Pink;
		}

		private void waferMap_MouseLeave(object sender, System.EventArgs e)
		{
			if(Single) return;
			if(!bSelected) this.BackColor = Color.White;
		}

		private void Map_MouseEnter(object sender, System.EventArgs e)
		{
			if(Single) return;
			if(!bSelected) this.BackColor = Color.Pink;
		}

		private void Map_MouseLeave(object sender, System.EventArgs e)
		{
			if(Single) return;
			if(!bSelected) this.BackColor = Color.White;
		}

		void MapSelected()
		{
			if(Single) return;
			if(!bSelected)
			{
				this.BackColor = Color.Blue;
			}
			else
			{
				this.BackColor = Color.White;
			}

			bSelected = !bSelected;
		}

		void MapSelected(bool selected)
		{
			if(Single) return;
			if(selected)
			{
				this.BackColor = Color.Blue;
			}
			else
			{
				this.BackColor = Color.White;
			}

			bSelected = selected;
		}

		private void waferMap_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) if(OnSelected != null) OnSelected(this);
		}

		private void waferMap_DoubleClick(object sender, System.EventArgs e)
		{
			if(OnDblClick != null) OnDblClick(this);
		}
	}
}
