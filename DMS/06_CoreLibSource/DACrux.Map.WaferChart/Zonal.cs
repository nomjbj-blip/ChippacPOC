using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Text;
using DACrux.Map;
using DACrux.Base;

namespace DACrux.Map.WaferChart
{
	public class Zonal : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		protected double m_dScale = 1.0d;
		protected Rectangle m_rectSelect = Rectangle.Empty;
		protected Rectangle m_rectSelect2 = Rectangle.Empty;

		protected Point m_poStart = Point.Empty;
		protected Point m_poEnd = Point.Empty;
		protected Bitmap m_bmpWaferMap;
		protected Bitmap m_bmpTemp;
		protected Bitmap m_bmpWaferID;
		protected RectangleD  m_rectdWaferArea;

		protected double m_dZoomRatio = 1.0d;
		protected int m_iViewAngle = 0;
		protected int m_iAngleOffset = 180;

		protected Color[] m_ColorSet;
		protected ArrayList m_arrDies;
		protected ArrayList m_DieIndexer = null;

		protected Graphics m_gdiMain = null;
		protected Graphics m_gdiTempMap = null;

		protected Position m_poInformation = Position.LeftTop;
		protected bool m_bVisibleInfo = true;
		protected ArrayList m_SelectedDies = null;
		protected ArrayList m_strInfomation = null;
		protected bool m_bVisibleDieValue = false;
		protected bool m_bVisibleVIFaile = false;

		#region Wafer속성 Member변수
		/// <summary>
		/// Region 속성
		/// </summary>
		protected double m_dNotchSize = 30.0d;


		// Draw속성
		protected Color m_colWafer = Color.Gray;
		protected Color m_colWaferBorder = Color.LightGray;
		protected Color m_colEdge = Color.White;
		protected bool m_bCenterMark = false;
		protected bool m_bScale = false;
		protected bool m_bVIVisible = false;
		#endregion

		#region Die속성 Member 변수
		protected Color m_colDieBorder = Color.LightGray;

		protected bool m_bDrawOriginDie = true;
		protected bool m_bDrawFirstDie = true;
		protected bool m_bDrawMarkDie = true;
		protected bool m_bDrawSkipDie = true;

		protected Color m_colOriginDieBorder = Color.Red;
		protected Color m_colFirstDieBorder = Color.SkyBlue;
		protected Color m_colMarkDieColor = Color.LightSkyBlue;
		protected Color m_colSkipDieColor = Color.Yellow;

		#endregion

		protected WaferRecipe m_WaferRecipe;
		protected HatchBrush m_hbSelectBrush = null;
		protected DataTable m_DT = null;
		
		protected string[] m_strSelectedBin = new string[] {"1"};
		public int m_iSelectedBinAlpha = 70;
		public int m_iNotSelectedBinAlpha = 20;
			
		protected int m_iCurrentX = -1;
		protected int m_iCurrentY = -1;
		protected int m_iCurrentDieIndex = -1;

		protected RectangleF m_rectNotchArea = RectangleF.Empty;
		protected string m_strDisplayDieValue = "BIN";
		protected bool m_bPopupMenu = true;
		protected bool m_bVisibleDieBorder = true;
		protected Font m_fntBin = new Font("굴림",6);

//		private float[] m_fZoneYield = new float[] {80.0f,69.0f,75.3f,90.0f,68.5f,79.0f,69.0f,70.0f,82.0f,
//													   69.0f,75.3f,90.0f,80.0f,72.8f,68.7f,78.0f,82.0f,100.0f};

		private float[] m_fZoneYield = new float[] {0,0,0,0,0,0,0,0,0,
													   0,0,0,0,0,0,0,0,0};
		public Zonal()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();
			SetDefaultColor();
			Reset();
			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.
		}
		

		public int AngleOffSet
		{
			set
			{
				m_iAngleOffset = value;
			}
			get
			{
				return m_iAngleOffset;
			}
		}

		public object Dies
		{
			get{ return (object)m_arrDies.ToArray(typeof(Die));	}
		}

		public object DiesIndex
		{
			get{ return (object)m_DieIndexer.ToArray(typeof(Point));	}
		}

		public virtual object DataSource
		{
			set
			{
				if(value == null) return;
				m_DT = (DataTable)value;
				this.DieClear();

				for(int i=0;i<m_DT.Rows.Count;i++)
				{
					Die oNewDie = new Die(DACrux.Base.Convert.intParse(m_DT.Rows[i]["X"].ToString())
						,DACrux.Base.Convert.intParse(m_DT.Rows[i]["Y"].ToString()),0,1);
					if(m_DT.Columns.IndexOf("VI")>-1) oNewDie.VIFail = DACrux.Base.Convert.intParse(m_DT.Rows[i]["VI"].ToString());
                    if(m_DT.Columns.IndexOf("VISUALINSP")>-1) oNewDie.VIFail = DACrux.Base.Convert.intParse(m_DT.Rows[i]["VISUALINSP"].ToString());
					if(m_DT.Columns.IndexOf("AVI")>-1) oNewDie.AVIFailNumber = DACrux.Base.Convert.intParse(m_DT.Rows[i]["AVI"].ToString());
                    if(m_DT.Columns.IndexOf("BIN") > -1) oNewDie.BinNumber = m_DT.Rows[i]["BIN"].ToString().Length == 0 ? 0 : DACrux.Base.Convert.intParse(m_DT.Rows[i]["BIN"].ToString());
					if(m_DT.Columns.IndexOf("PCMVALUE")>-1) oNewDie.ParametricValue = DACrux.Base.Convert.doubleParse(m_DT.Rows[i]["PCMVALUE"].ToString());
					oNewDie.DiePassFail = oNewDie.BinNumber + oNewDie.AVIFailNumber + oNewDie.VIFail;

					this.AddDie(oNewDie);
				}
				for(int i=0;i<18;i++)
					m_fZoneYield[i] = GetPieYield(i);
			}get
			 {
				 return m_DT;
			 }
		}

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// Zonal
			// 
			Cursor = System.Windows.Forms.Cursors.Cross;
			this.ForeColor = System.Drawing.Color.Red;
			this.Name = "Zonal";
			this.Size = new System.Drawing.Size(336, 320);
			this.Resize += new System.EventHandler(this.WaferMap_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.WaferMap_Paint);

		}
		#endregion

		#region ◈ 소멸자 , Dispose
		~Zonal()
		{
			if(m_arrDies != null) 
			{
				m_arrDies.Clear();
				m_arrDies = null;
			}

			if(m_DieIndexer != null)
			{
				m_DieIndexer.Clear();
				m_DieIndexer = null;
			}

			if(m_hbSelectBrush != null) m_hbSelectBrush.Dispose();

			if(m_gdiMain != null) m_gdiMain.Dispose();
			if(m_gdiTempMap != null) m_gdiTempMap.Dispose();
			if(m_bmpWaferID != null) m_bmpWaferID.Dispose();
			if(m_bmpTemp != null) m_bmpTemp.Dispose();
			if(m_bmpWaferMap != null) m_bmpWaferMap.Dispose();
			//if(ctxmWaferMap != null) ctxmWaferMap.Dispose();
			if(m_DT != null) m_DT.Dispose();
			m_strInfomation = null;
			m_ColorSet = null;
			GC.Collect();
		}


		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					if(m_arrDies != null) 
					{
						m_arrDies.Clear();
						m_arrDies = null;
					}

					if(m_DieIndexer != null)
					{
						m_DieIndexer.Clear();
						m_DieIndexer = null;
					}

					if(m_hbSelectBrush != null) m_hbSelectBrush.Dispose();

					if(m_gdiMain != null) m_gdiMain.Dispose();
					if(m_gdiTempMap != null) m_gdiTempMap.Dispose();
					if(m_bmpTemp != null) m_bmpTemp.Dispose();
					if(m_bmpWaferMap != null) m_bmpWaferMap.Dispose();
					if(m_DT != null) m_DT.Dispose();
					m_ColorSet = null;

					components.Dispose();
					GC.Collect();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region ◈ Wafer
		public virtual void Reset()
		{
			/// Data가 존재 했을경우 초기화//////////////////////////////////////////////////
			if(m_arrDies != null) 
			{
				m_arrDies.Clear();
				m_arrDies = null;
			}

			if(m_DieIndexer != null)
			{
				m_DieIndexer.Clear();
				m_DieIndexer = null;
			}

			if(m_hbSelectBrush != null) m_hbSelectBrush.Dispose();
			//////////////////////////////////////////////////////////////////////////////
		
			m_WaferRecipe = new WaferRecipe(300.0d);
			if(m_WaferRecipe.NOTCH_TYPE == Notch.Notch)
			{
				m_dNotchSize = 2.0d;
			}
			else
			{
				m_dNotchSize = 30.0d;
			}
			
			double dMinCanvers = Math.Min(this.Width,this.Height);
			m_dZoomRatio = dMinCanvers/m_WaferRecipe.WAFER_SIZE;
			m_rectdWaferArea.X = ((dMinCanvers - this.Width)/2.0f)/m_dZoomRatio;
			m_rectdWaferArea.Y = ((dMinCanvers - this.Height)/2.0f)/m_dZoomRatio;
			m_rectdWaferArea.Width = m_WaferRecipe.WAFER_SIZE;
			m_rectdWaferArea.Height = m_WaferRecipe.WAFER_SIZE;

			m_arrDies = new ArrayList();
			m_DieIndexer = new ArrayList();
			m_SelectedDies = new ArrayList();
			m_hbSelectBrush = new HatchBrush(HatchStyle.WideUpwardDiagonal,Color.White,Color.PowderBlue    );
		}
		#endregion

		#region ◈ First Die 속성
		/// <summary>
		/// Probe Test시 First Die의 Drawing에 대한 속성을 정의한다.
		/// </summary>
		/// 
		[Category("First Die"), Description("First Die를 표시할지 여부를 가져오거나 설정합니다.")]
		public bool DrawFirstDie
		{
			set{m_bDrawFirstDie = value;}
			get{return m_bDrawFirstDie;}
		}

		[Category("First Die"), Description("First Die의 X좌표 값을 가져오거나 설정합니다.")]
		public int FirstDieX
		{
			set{ m_WaferRecipe.FIRST_DIE_X = value;	}
			get{return m_WaferRecipe.FIRST_DIE_X;}
		}

		[Category("First Die"), Description("First Die의 Y좌표 값을 가져오거나 설정합니다.")]
		public int FirstDieY
		{
			set{m_WaferRecipe.FIRST_DIE_Y = value;}
			get{return m_WaferRecipe.FIRST_DIE_Y;}
		}
		
		[Category("First Die"), Description("First Die의 Border Color 값을 가져오거나 설정합니다.")]
		public Color FirstDieBorderColor
		{
			set{m_colFirstDieBorder = value;}
			get{return m_colFirstDieBorder;}
		}
		#endregion

		#region ◈ Origin Die 속성
		
		[Category("Origin Die"), Description("Origin Die를 표시할지 여부를 가져오거나 설정합니다.")]
		public bool DrawOriginDie
		{
			set{m_bDrawOriginDie = value;}
			get{return m_bDrawOriginDie;}
		}

		[Category("Origin Die"), Description("Origin Die의 X축 절대좌표를 가져오거나 설정합니다.")]
		public double OriginX
		{
			set{m_WaferRecipe.ORIGIN_X = value;}
			get{return m_WaferRecipe.ORIGIN_X;}
		}

		[Category("Origin Die"), Description("Origin Die의 Y축 절대좌표를 가져오거나 설정합니다.")]
		public double OriginY
		{
			set{m_WaferRecipe.ORIGIN_Y = value;}
			get{return m_WaferRecipe.ORIGIN_Y;}
		}

		[Category("Origin Die"), Description("Origin Die의 X축 Index 좌표를 가져오거나 설정합니다.")]
		public int OriginIndexX
		{
			set{m_WaferRecipe.ORIGIN_DIE_X = value;}
			get{return m_WaferRecipe.ORIGIN_DIE_X;}
		}

		[Category("Origin Die"), Description("Origin Die의 Y축 Index 좌표를 가져오거나 설정합니다.")]
		public int OriginIndexY
		{
			set{m_WaferRecipe.ORIGIN_DIE_Y = value;}
			get{return m_WaferRecipe.ORIGIN_DIE_Y;}
		}

		[Category("Origin Die"), Description("Origin Die의 Border Color값을 가져오거나 설정합니다.")]
		public Color OriginDieBorder
		{
			set{m_colOriginDieBorder = value;}
			get{return m_colOriginDieBorder;}
		}

		#endregion

		#region ◈ Mark Die 속성
		
		[Category("Mark Die"), Description("Mark Die를 표시할지 여부를 가져오거나 설정합니다.")]
		public bool DrawMarkDie
		{
			set{m_bDrawMarkDie = value;}
			get{return m_bDrawMarkDie;}
		}

		[Category("Mark Die"), Description("Mark Die의 Border Color값을 가져오거나 설정합니다.")]
		public Color MarkDieColor
		{
			set{m_colMarkDieColor = value;}
			get{return m_colMarkDieColor;}
		}

		#endregion

		#region ◈ Skip Die 속성
		
		[Category("Mark Die"), Description("Skip Die를 표시할지 여부를 가져오거나 설정합니다.")]
		public bool DrawSkipDie
		{
			set{m_bDrawSkipDie = value;}
			get{return m_bDrawSkipDie;}
		}

		[Category("Skip Die"), Description("Skip Die의 Border Color값을 가져오거나 설정합니다.")]
		public Color SkipDieColor
		{
			set{m_colSkipDieColor = value;}
			get{return m_colSkipDieColor;}
		}

		#endregion

		#region ◈ Die 속성
		
		[Category("Die 속성"), Description("Die의 Border Color를 설정하거나 가져옵니다.")]
		public Color DieBorderColor
		{
			set{ m_colDieBorder = value;}
			get{return m_colDieBorder;}
		}

		[Category("Die 속성"), Description("Die의 Border를 표시할지 여부를 설정하거나 가져옵니다.")]
		public bool VisibleDieBorder
		{
			set{ m_bVisibleDieBorder = value;}
			get{return m_bVisibleDieBorder;}
		}

		[Category("Die 속성"), Description("Die 의 Width를 mm로 설정하거나 가져옵니다.")]
		public double DieSizeX
		{
			set{m_WaferRecipe.DIE_SIZE_X = value;}
			get{return m_WaferRecipe.DIE_SIZE_X;}
		}

		[Category("Die 속성"), Description("Die 의 Height를 mm로 설정하거나 가져옵니다.")]
		public double DieSizeY
		{
			set{m_WaferRecipe.DIE_SIZE_Y = value;}
			get{return m_WaferRecipe.DIE_SIZE_Y;}
		}

		[Category("Die 속성"), Description("Wafer에 존재한는 Die의 X축 Count를 설정하거나 가져옵니다.")]
		public int XDies
		{
			get{return m_WaferRecipe.XDIES;}
		}

		[Category("Die 속성"), Description("Wafer에 존재한는 Die의 Y축 Count를 설정하거나 가져옵니다.")]
		public int YDies
		{
			get{return m_WaferRecipe.YDIES;}
		}

		[Category("Die 속성"), Description("Wafer에 존재한는 Die의 X축의 최소 Index를 설정하거나 가져옵니다.")]
		public int DieMinX
		{
			set{ m_WaferRecipe.DIE_INDEX_MIN_X = value; }
			get{ return m_WaferRecipe.DIE_INDEX_MIN_X; }
		}

		[Category("Die 속성"), Description("Wafer에 존재한는 Die의 Y축의 최소 Index를 설정하거나 가져옵니다.")]
		public int DieMinY
		{
			set{ m_WaferRecipe.DIE_INDEX_MIN_Y = value; }
			get{ return m_WaferRecipe.DIE_INDEX_MIN_Y; }
		}

		[Category("Die 속성"), Description("Wafer에 존재한는 Die의 X축의 최대 Index를 설정하거나 가져옵니다.")]
		public int DieMaxX
		{
			set{ m_WaferRecipe.DIE_INDEX_MAX_X = value; }
			get{ return m_WaferRecipe.DIE_INDEX_MAX_X; }
		}

		[Category("Die 속성"), Description("Wafer에 존재한는 Die의 Y축의 최대 Index를 설정하거나 가져옵니다.")]
		public int DieMaxY
		{
			set{ m_WaferRecipe.DIE_INDEX_MAX_Y = value; }
			get{ return m_WaferRecipe.DIE_INDEX_MAX_Y; }
		}
		#endregion

		#region ◈ Wafer 속성
		[Category("Wafer 속성"), Description("Wafer의 Size를 mm로 설정하거나 가져옵니다.")]
		public double WaferSize
		{
			set{m_WaferRecipe.WAFER_SIZE = value;}
			get{return m_WaferRecipe.WAFER_SIZE;}
		}
		
		[Category("Wafer 속성"), Description("Wafer의 Background Color를 설정하거나 가져옵니다.")]
		public Color WaferColor
		{
			set{m_colWafer = value;}
			get{return m_colWafer;}
		}

		[Category("Wafer 속성"), Description("Wafer의 Border Color를 설정하거나 가져옵니다.")]
		public Color WaferBorderColor
		{
			set{m_colWaferBorder = value;}
			get{return m_colWaferBorder;}
		}

		[Category("Wafer 속성"), Description("Edge의 Size를 mm로 설정하거나 가져옵니다.")]
		public double EdgeSize
		{
			set{m_WaferRecipe.EDGE_SIZE = value;}
			get{return m_WaferRecipe.EDGE_SIZE;}
		}
		
		[Category("Wafer 속성"), Description("Edge의 Background Color를 설정하거나 가져옵니다.")]
		public Color EdgeColor
		{
			set{m_colEdge = value;}
			get{return m_colEdge;}
		}

		[Category("Wafer 속성"), Description("Net Die의 개수를 가져옵니다.")]
		public int NetDie
		{
			//set{m_WaferRecipe.NETDIE = value;}
			get{return m_WaferRecipe.NETDIE;}
		}

		[Category("Wafer 속성"), Description("Notch의 각도를 설정하거나 가져옵니다.")]
		public int NotchAngle
		{
			set
			{
				m_WaferRecipe.ANGLE = value;
			}
			get
			{
				return m_WaferRecipe.ANGLE;
			}
		}

		[Category("Wafer 속성"), Description("Notch의 Type을 설정하거나 가져옵니다.")]
		public Notch NotchType
		{
			set
			{
				m_WaferRecipe.NOTCH_TYPE = value;

				if(m_WaferRecipe.NOTCH_TYPE == Notch.Notch)
				{
					m_dNotchSize = 2.0d;
				}
				else
				{
					m_dNotchSize = 30.0d;
				}
			}
			get
			{
				return m_WaferRecipe.NOTCH_TYPE;
			}
		}
		#endregion

		#region ◈ Option 속성

		
		[Category("Wafer Option"), Description("Wafer Infomation의 표시 여부를 설정하거나 가져옵니다.")]
		public bool VisibleInfomation
		{
			set
			{
				m_bVisibleInfo = value;
			}
			get
			{
				return m_bVisibleInfo;
			}
		}

		[Category("Wafer Option"), Description("REFERENCEDIE_SETTING")]
		public int ReferenceDieSetting
		{
			set
			{
				m_WaferRecipe.REFERENCEDIE_SETTING = value;
			}
			get
			{
				return m_WaferRecipe.REFERENCEDIE_SETTING;
			}
		}

		[Category("Wafer Option"), Description("X,Y 좌표의 시작점을 설정하거나 가져옵니다.")]
		public XYDirection XYDirect
		{
			set
			{
				m_WaferRecipe.XYDIR = value;
			}
			get
			{
				return m_WaferRecipe.XYDIR;
			}
		}

		[Category("Wafer Option"), Description("Visual Inspection Code의 Display여부를 설정하거나 가져옵니다.")]
		public bool VIVisible
		{
			set
			{
				m_bVIVisible = value;
			}
			get
			{
				return m_bVIVisible;
			}
		}

		[Category("Wafer Option"), Description("Draw 대상 Bin을 설정하거나 가져옵니다.")]
		public virtual string SelecetedBin
		{
			set
			{
				m_strSelectedBin = value.Split(',');

				for(int i=0;i<18;i++)
					m_fZoneYield[i] = GetPieYield(i);

				System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(FadeIn));
				t.IsBackground = true;
				t.Start();
			}
			get
			{
				return string.Join(",",m_strSelectedBin);
			}
		}

		private void FadeIn()
		{
			if(m_DieIndexer.Count>0)
			{
				for(int i=10; i>0; i--)
				{
					m_iNotSelectedBinAlpha =  i * m_iSelectedBinAlpha / 10;		

					this.Redraw();
				}
			}
		}

		#endregion

		#region ■ Real Position 계산 관련 (Rotate,GetRealPoint,GetViewPoint)
		public void Rotate(double Angle)
		{
			Angle = Angle % 360;
			m_iViewAngle = (int)(Angle + (double)m_iViewAngle);

			Redraw();
			if(m_iViewAngle >= 0)
			{
				m_iViewAngle = m_iViewAngle % 360;
			}
			else
			{
				m_iViewAngle = (int)(360.0d + ((double)m_iViewAngle % 360));
			}
		}

		protected PointD GetRealPoint(int piX,int piY)
		{
			PointD pdCenter;
			PointF pfWaferCenterToPixel = GetViewPoint(0.0d,0.0d);
			PointD pdBefor;

			pdBefor.X = (double)(pfWaferCenterToPixel.X - piX);
			pdBefor.Y = (double)(pfWaferCenterToPixel.Y - piY);

			RotatePoint(ref pdBefor.X ,ref pdBefor.Y ,m_iViewAngle);
			piX = (int)(pdBefor.X + pfWaferCenterToPixel.X);
			piY = (int)(pdBefor.Y + pfWaferCenterToPixel.Y);

			pdCenter.X = m_WaferRecipe.WAFER_SIZE/2 - (m_rectdWaferArea.X + piX/(m_dZoomRatio * m_dScale));
			pdCenter.Y = - m_WaferRecipe.WAFER_SIZE/2 + (m_rectdWaferArea.Y + piY/(m_dZoomRatio * m_dScale));
			return pdCenter;
		}

		private PointF GetViewPoint(double dX,double dY)
		{
			PointF pfCenter = PointF.Empty;
			pfCenter.X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + dX ) * (m_dZoomRatio * m_dScale));
			pfCenter.Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - dY ) * (m_dZoomRatio * m_dScale));
			return pfCenter;
		}
		#endregion

		#region ■ Draw Wafer
		protected virtual void DrawWafer()
		{
			if(this.Width * this.Height == 0 || m_gdiTempMap == null) return;

			PointD dNotchStart;
			PointD dNotchEnd;
			GraphicsPath gpWafer = null;
			GraphicsPath gpYield = null;
			GraphicsPath gpYieldValue = null;

			m_gdiTempMap.Clear(Color.White);
			SolidBrush sbrshEdge = null;
			SolidBrush sbrshWafer = null;
			Pen pEdge = null;
			Pen pWafer = null;
			Font fntWID = new Font("굴림",Math.Max(8,(float)((m_dZoomRatio * m_dScale) * 2.3f)) );
			m_fntBin = new Font("굴림",9 );
			int iRealAngle = (m_WaferRecipe.ANGLE + m_iAngleOffset) % 360;
			try
			{
				gpWafer = new GraphicsPath();

				///=======Wafer Edge Drawing ========================================================================
				gpWafer.AddArc((float)((- m_rectdWaferArea.X + m_WaferRecipe.EDGE_SIZE) * (m_dZoomRatio * m_dScale))
					, (float)((- m_rectdWaferArea.Y + m_WaferRecipe.EDGE_SIZE) * (m_dZoomRatio * m_dScale))
					, (float)((m_WaferRecipe.WAFER_SIZE-m_WaferRecipe.EDGE_SIZE*2) * (m_dZoomRatio * m_dScale))
					, (float)((m_WaferRecipe.WAFER_SIZE-m_WaferRecipe.EDGE_SIZE*2) * (m_dZoomRatio * m_dScale))
					, (float)((m_iViewAngle + iRealAngle + 90 + m_dNotchSize/2) % 360)
					, (float)(360 - m_dNotchSize));

				if(m_WaferRecipe.NOTCH_TYPE != Notch.Notch)
				{
					dNotchStart.X = - (float)Math.Sin((m_dNotchSize/2)/180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE-m_WaferRecipe.EDGE_SIZE*2)/2);
					dNotchStart.Y = - (float)Math.Cos((m_dNotchSize/2)/180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE-m_WaferRecipe.EDGE_SIZE*2)/2);
					dNotchEnd.X =  (float)Math.Sin((m_dNotchSize/2)/180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE-m_WaferRecipe.EDGE_SIZE*2)/2);
					dNotchEnd.Y = dNotchStart.Y;

					RotatePoint(ref dNotchStart.X , ref dNotchStart.Y, (iRealAngle + m_iViewAngle) % 360);
					RotatePoint(ref dNotchEnd.X , ref dNotchEnd.Y,  (iRealAngle + m_iViewAngle) % 360);

					gpWafer.AddLine((float)((- m_rectdWaferArea.X + dNotchStart.X + (m_WaferRecipe.WAFER_SIZE/2)) * (m_dZoomRatio * m_dScale))
						, (float)((- m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE/2) - dNotchStart.Y) * (m_dZoomRatio * m_dScale))
						, (float)((- m_rectdWaferArea.X + dNotchEnd.X + (m_WaferRecipe.WAFER_SIZE/2)) * (m_dZoomRatio * m_dScale))
						, (float)((- m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE/2) - dNotchEnd.Y) * (m_dZoomRatio * m_dScale)));
				}
				///==================================================================================================
				///
				sbrshWafer	=	new SolidBrush(m_colWafer);
				pWafer		=	new Pen(m_colWafer);
				m_gdiTempMap.FillPath(sbrshWafer,gpWafer);
				m_gdiTempMap.DrawPath(pWafer,gpWafer);

				gpWafer.StartFigure();

				///=======Wafer Line Drawing ========================================================================
				///
				gpWafer.AddArc((float)(- m_rectdWaferArea.X * (m_dZoomRatio * m_dScale))
					, (float)(- m_rectdWaferArea.Y * (m_dZoomRatio * m_dScale))
					, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale))
					, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale))
					, (float)((m_iViewAngle + iRealAngle + 90 + m_dNotchSize/2) % 360)
					, (float)(360 - m_dNotchSize));




				dNotchStart.X = - (float)Math.Sin((m_dNotchSize/2)/180 * Math.PI) * (m_WaferRecipe.WAFER_SIZE/2);
				dNotchStart.Y = - (float)Math.Cos((m_dNotchSize/2)/180 * Math.PI) * (m_WaferRecipe.WAFER_SIZE/2);
				dNotchEnd.X = (float)Math.Sin((m_dNotchSize/2)/180 * Math.PI) * (m_WaferRecipe.WAFER_SIZE/2);
				dNotchEnd.Y = dNotchStart.Y;

				RotatePoint(ref dNotchStart.X , ref dNotchStart.Y, (iRealAngle + m_iViewAngle) % 360);
				RotatePoint(ref dNotchEnd.X , ref dNotchEnd.Y,(iRealAngle + m_iViewAngle) % 360);

				if(m_WaferRecipe.NOTCH_TYPE == Notch.Notch)
				{
					PointD dNotchRStart;
					PointD dNotchREnd;
					double dNotchR = (float)Math.Sin((m_dNotchSize/2)/180 * Math.PI) * (m_WaferRecipe.WAFER_SIZE/2);
					dNotchRStart.X = (dNotchEnd.X + dNotchStart.X)/2 - dNotchR;
					dNotchRStart.Y = (dNotchEnd.Y + dNotchStart.Y)/2;
					dNotchREnd.X = (dNotchEnd.X + dNotchStart.X)/2 + dNotchR;
					dNotchREnd.Y = (dNotchEnd.Y + dNotchStart.Y)/2;
				
					gpWafer.AddArc((float)((- m_rectdWaferArea.X + dNotchRStart.X + (m_WaferRecipe.WAFER_SIZE/2)) * (m_dZoomRatio * m_dScale))
						, (float)((- m_rectdWaferArea.Y - dNotchRStart.Y + (m_WaferRecipe.WAFER_SIZE/2) - (dNotchREnd.X - dNotchRStart.X)/2) * (m_dZoomRatio * m_dScale))
						, (float)((dNotchREnd.X - dNotchRStart.X) * (m_dZoomRatio * m_dScale))
						, (float)((dNotchREnd.X - dNotchRStart.X) * (m_dZoomRatio * m_dScale))
						, (float)((m_iViewAngle + iRealAngle + 180 + m_dNotchSize/2) % 360)
						, (float)(180));
				}
				else
				{
					gpWafer.AddLine((float)((- m_rectdWaferArea.X + dNotchStart.X + (m_WaferRecipe.WAFER_SIZE/2)) * (m_dZoomRatio * m_dScale))
						, (float)((- m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE/2) - dNotchStart.Y) * (m_dZoomRatio * m_dScale))
						, (float)((- m_rectdWaferArea.X + dNotchEnd.X + (m_WaferRecipe.WAFER_SIZE/2)) * (m_dZoomRatio * m_dScale))
						, (float)((- m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE/2) - dNotchEnd.Y) * (m_dZoomRatio * m_dScale)));
				}


				///========================================================================================================================
				///
				sbrshEdge = new SolidBrush(m_colEdge);
				pEdge		= new Pen(m_colEdge);
				m_gdiTempMap.FillPath(sbrshEdge,gpWafer);
				m_gdiTempMap.DrawPath(pEdge,gpWafer);

				///

				pEdge.Width = 3;
				m_gdiTempMap.DrawArc(pEdge
					, (float)((-m_rectdWaferArea.X + (m_WaferRecipe.WAFER_SIZE/2) - (m_WaferRecipe.WAFER_SIZE/2) * 1/4) * (m_dZoomRatio * m_dScale))
					, (float)((-m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE/2) - (m_WaferRecipe.WAFER_SIZE/2) * 1/4) * (m_dZoomRatio * m_dScale))
					, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale)) * 1/4
					, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale)) * 1/4
					, 0.0f
					, 360.0f);
				///

				m_gdiTempMap.DrawArc(pEdge
					, (float)((- m_rectdWaferArea.X + (m_WaferRecipe.WAFER_SIZE/2) - (m_WaferRecipe.WAFER_SIZE/2) * 2/4) * (m_dZoomRatio * m_dScale))
					, (float)((- m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE/2) - (m_WaferRecipe.WAFER_SIZE/2) * 2/4) * (m_dZoomRatio * m_dScale))
					, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale)) * 2/4
					, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale)) * 2/4
					, 0.0f
					, 360.0f);
				///
				m_gdiTempMap.DrawArc(pEdge
					, (float)((- m_rectdWaferArea.X + (m_WaferRecipe.WAFER_SIZE/2) - (m_WaferRecipe.WAFER_SIZE/2) * 3/4) * (m_dZoomRatio * m_dScale))
					, (float)((- m_rectdWaferArea.Y + (m_WaferRecipe.WAFER_SIZE/2) - (m_WaferRecipe.WAFER_SIZE/2) * 3/4) * (m_dZoomRatio * m_dScale))
					, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale)) * 3/4
					, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale)) * 3/4
					, 0.0f
					, 360.0f);


				/// Notch부분의 사각형을 정의한다.
				///======================================================================================================================== 
				PointD[] pdPoint = new PointD[4];

				pdPoint[0].X = - Math.Sin((m_dNotchSize/2)/180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE-m_WaferRecipe.EDGE_SIZE*2)/2);
				pdPoint[0].Y = - Math.Cos((m_dNotchSize/2)/180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE-m_WaferRecipe.EDGE_SIZE*2)/2);
				pdPoint[1].X = Math.Sin((m_dNotchSize/2)/180 * Math.PI) * ((m_WaferRecipe.WAFER_SIZE-m_WaferRecipe.EDGE_SIZE*2)/2);
				pdPoint[1].Y = pdPoint[0].Y;

				pdPoint[2].X = pdPoint[1].X;
				pdPoint[2].Y = -(m_WaferRecipe.WAFER_SIZE/2);
				pdPoint[3].X = pdPoint[0].X;
				pdPoint[3].Y = -(m_WaferRecipe.WAFER_SIZE/2);
			
				RotatePoint(ref pdPoint[0].X,ref pdPoint[0].Y,(iRealAngle) % 360);
				RotatePoint(ref pdPoint[1].X,ref pdPoint[1].Y,(iRealAngle) % 360);
				RotatePoint(ref pdPoint[2].X,ref pdPoint[2].Y,(iRealAngle) % 360);
				RotatePoint(ref pdPoint[3].X,ref pdPoint[3].Y,(iRealAngle) % 360);

				double dMinX = Math.Min(Math.Min(pdPoint[0].X,pdPoint[1].X),Math.Min(pdPoint[2].X,pdPoint[3].X));
				double dMaxX = Math.Max(Math.Max(pdPoint[0].X,pdPoint[1].X),Math.Max(pdPoint[2].X,pdPoint[3].X));
				double dMinY = Math.Min(Math.Min(pdPoint[0].Y,pdPoint[1].Y),Math.Min(pdPoint[2].Y,pdPoint[3].Y));
				double dMaxY = Math.Max(Math.Max(pdPoint[0].Y,pdPoint[1].Y),Math.Max(pdPoint[2].Y,pdPoint[3].Y));

				m_rectNotchArea.X = (float)dMinX;
				m_rectNotchArea.Y = (float)dMaxY;
				m_rectNotchArea.Width = (float)(dMaxX-dMinX);
				m_rectNotchArea.Height = (float)(dMaxY-dMinY);
				///========================================================================================================================
				/// 

				DrawDies(m_gdiTempMap);

				gpYield = new GraphicsPath();
				gpYieldValue = new GraphicsPath();

				PointD pntFirst;
				PointD pntSecond;
				

				for(int i=0;i<18;i++)
				{
					m_gdiTempMap.DrawPie(pEdge
									, (float)(- m_rectdWaferArea.X * (m_dZoomRatio * m_dScale))
									, (float)(- m_rectdWaferArea.Y * (m_dZoomRatio * m_dScale))
									, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale))
									, (float)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale))
									, (float)i*20
									, 20.0f);

					
					if(i>0)
					{

						pntFirst.X = Math.Sin( (double)((i-1) * 20 + 10)/180  * Math.PI ) * ((m_WaferRecipe.WAFER_SIZE/2.0f) * (m_fZoneYield[i-1]/100.0f));
						pntFirst.Y = Math.Cos( (double)((i-1) * 20 + 10)/180  * Math.PI ) * ((m_WaferRecipe.WAFER_SIZE/2.0f) * (m_fZoneYield[i-1]/100.0f));

						pntSecond.X = Math.Sin( (double)(i * 20 + 10)/180  * Math.PI ) * ((m_WaferRecipe.WAFER_SIZE/2.0f) * (m_fZoneYield[i]/100.0f));
						pntSecond.Y = Math.Cos( (double)(i * 20 + 10)/180  * Math.PI ) * ((m_WaferRecipe.WAFER_SIZE/2.0f) * (m_fZoneYield[i]/100.0f));
						
						RotatePoint(ref pntFirst.X ,ref pntFirst.Y ,(iRealAngle) % 360);
						RotatePoint(ref pntSecond.X ,ref pntSecond.Y ,(iRealAngle) % 360);

						pntFirst.X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2.0f) - pntFirst.X);
						pntFirst.Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2.0f) + pntFirst.Y);

						pntSecond.X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2.0f) - pntSecond.X);
						pntSecond.Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2.0f) + pntSecond.Y);
						
						gpYield.AddLine(  (float)pntFirst.X * (float)(m_dZoomRatio * m_dScale)
							, (float)pntFirst.Y * (float)(m_dZoomRatio * m_dScale)
							, (float)pntSecond.X * (float)(m_dZoomRatio * m_dScale)
							, (float)pntSecond.Y * (float)(m_dZoomRatio * m_dScale));

						if(i==1)
						{
							gpYieldValue.AddString(string.Format("{0:#0.00}",m_fZoneYield[i-1])
								,m_fntBin.FontFamily 
								,(int)FontStyle.Regular
								,12
								,new PointF((float)pntFirst.X * (float)(m_dZoomRatio * m_dScale),(float)pntFirst.Y * (float)(m_dZoomRatio * m_dScale))
								,StringFormat.GenericDefault );
						}

						gpYieldValue.AddString(string.Format("{0:#0.00}",m_fZoneYield[i])
							,m_fntBin.FontFamily
							,(int)FontStyle.Regular
							,12
							,new PointF((float)pntSecond.X * (float)(m_dZoomRatio * m_dScale),(float)pntSecond.Y * (float)(m_dZoomRatio * m_dScale))
							,StringFormat.GenericDefault );

					}
				}

				gpYield.CloseFigure();
				pEdge.Width = 4;
				pEdge.Color = Color.Red;
				m_gdiTempMap.DrawPath(pEdge,gpYield);

				pEdge.Width = 1;
				pEdge.Color = Color.Black;
				m_gdiTempMap.DrawPath(pEdge,gpYieldValue);

				/// Wafer ID Draw
				///========================================================================================================================
				///
				if(m_strInfomation !=null && m_bVisibleInfo )
				{
					for(int i=0;i<m_strInfomation.Count;i++)
					{
						m_gdiTempMap.DrawString(m_strInfomation[i].ToString(),fntWID,new SolidBrush(Color.Black),(float)((-m_rectdWaferArea.X + 2.0) * (m_dZoomRatio * m_dScale))
							,(float)((-m_rectdWaferArea.Y + 2.0) * (m_dZoomRatio * m_dScale) + (fntWID.Height * i) ) );
					}
				}

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(gpWafer != null) gpWafer.Dispose();
				if(sbrshEdge != null) sbrshEdge.Dispose();
				if(pEdge != null) pEdge.Dispose();
				if(sbrshWafer != null) sbrshWafer.Dispose();
				if(pWafer != null) pWafer.Dispose();
				if(fntWID != null) fntWID.Dispose();
			}
		}

		#endregion

		#region ■ Point좌표를 주어진 각도로 Rotation하는 함수
		protected void RotatePoint(ref double dx, ref double dy, double RAngle)
		{
			double tX = dx * Math.Cos(RAngle/180 * Math.PI) + dy * Math.Sin(RAngle/180 * Math.PI);
			double tY = - dx * Math.Sin(RAngle/180 * Math.PI) + dy * Math.Cos(RAngle/180 * Math.PI);

			dx = tX;
			dy = tY;
		}

		protected void RotatePoint(ref float dx, ref float dy, double RAngle)
		{
			float tX = dx * (float)Math.Cos(RAngle/180 * Math.PI) + dy * (float)Math.Sin(RAngle/180 * Math.PI);
			float tY = - dx * (float)Math.Sin(RAngle/180 * Math.PI) + dy * (float)Math.Cos(RAngle/180 * Math.PI);

			dx = tX;
			dy = tY;
		}

		#endregion

		#region ■ 주어진 조건으로 Wafer 안에 들어갈 Die들을 구하는 함수
		public virtual void DieCalculation()
		{
			double dDiePitchX = (m_WaferRecipe.WAFER_SIZE * 0.90) / m_WaferRecipe.XDIES ;
			double dDiePitchY = (m_WaferRecipe.WAFER_SIZE * 0.90) / m_WaferRecipe.YDIES;
			double dOriginX = (m_WaferRecipe.WAFER_SIZE * 0.90)/2;
			double dOriginY = (m_WaferRecipe.WAFER_SIZE * 0.90)/2;

			double dX = 0.0d;
			double dY = 0.0d;


			m_WaferRecipe.ORIGIN_X = dOriginX + dDiePitchX;
			m_WaferRecipe.ORIGIN_Y = dOriginY + dDiePitchY;
			m_WaferRecipe.DIE_SIZE_X = dDiePitchX;
			m_WaferRecipe.DIE_SIZE_Y = dDiePitchY;
			this.m_WaferRecipe.NETDIE = 0;

			int iBin = 0;
			m_arrDies.Clear();
			Die oDie = new Die(0,0,0,0,0,0,0);
			try
			{
				for(int idx=0;idx<m_WaferRecipe.XDIES;idx++)
				{
					dX = idx * dDiePitchX - dOriginX;

					for(int idy=0;idy<m_WaferRecipe.YDIES;idy++)
					{				
						dY = idy * dDiePitchY - dOriginY;

						if(UseDie(dX,dY,dDiePitchX,dDiePitchY))
						{
							oDie.IndexX = idx;
							oDie.IndexY = idy;
							oDie.BinNumber = iBin;
							oDie.DieCood.X = dX;
							oDie.DieCood.Y = dY;
							oDie.DieCood.Width = dDiePitchX;
							oDie.DieCood.Height = dDiePitchY;
							oDie.DieProp = 1;
							oDie.ZoneNumber = 0;
							m_arrDies.Add(oDie);
							this.m_WaferRecipe.NETDIE++;
						}
					}
				}
			}
			catch
			{
			}
		}
		#endregion

		#region ■ 주어진 Real Size 조건으로 Wafer안에 들어갈 Die들을 구하는 함수 [ DieCalculation(bool isReal) / UseDie(double X,double Y,double Width,double Height) ]
		public virtual void DieCalculation(bool isReal)
		{
			if(!isReal)
			{
				this.DieCalculation();
				return;
			}

			double dWaferR = m_WaferRecipe.WAFER_SIZE / 2.0d;

			this.m_WaferRecipe.DIE_INDEX_MIN_X = (int)((- dWaferR + this.m_WaferRecipe.ORIGIN_X) / this.m_WaferRecipe.DIE_SIZE_X) + this.m_WaferRecipe.ORIGIN_DIE_X -1;
			this.m_WaferRecipe.DIE_INDEX_MIN_Y = (int)((- dWaferR + this.m_WaferRecipe.ORIGIN_Y) / this.m_WaferRecipe.DIE_SIZE_Y) + this.m_WaferRecipe.ORIGIN_DIE_Y -1;
			this.m_WaferRecipe.DIE_INDEX_MAX_X = (int)((dWaferR + this.m_WaferRecipe.ORIGIN_X) / this.m_WaferRecipe.DIE_SIZE_X) + this.m_WaferRecipe.ORIGIN_DIE_X;
			this.m_WaferRecipe.DIE_INDEX_MAX_Y = (int)((dWaferR + this.m_WaferRecipe.ORIGIN_Y) / this.m_WaferRecipe.DIE_SIZE_Y) + this.m_WaferRecipe.ORIGIN_DIE_Y;
			
			int iDieX = 0;
			int iDieY = 0;

			double dX = 0.0d;
			double dY = 0.0d;

			int iBin = 0;
			m_arrDies.Clear();

			this.m_WaferRecipe.NETDIE = 0;
			Die oDie = new Die(0,0,0,0,0,0,0);			
			for(int idx=this.m_WaferRecipe.DIE_INDEX_MIN_X;idx<=this.m_WaferRecipe.DIE_INDEX_MAX_X;idx++)
			{
				for(int idy=m_WaferRecipe.DIE_INDEX_MIN_Y;idy<=this.m_WaferRecipe.DIE_INDEX_MAX_Y;idy++)
				{				
					switch(m_WaferRecipe.XYDIR)
					{
						case XYDirection.LeftTop:
							iDieX = idx;
							iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) - 1;
							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) * this.m_WaferRecipe.DIE_SIZE_Y;
							break;
						case XYDirection.LeftBottom:
							iDieX = idx;
							iDieY = idy;
							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) * this.m_WaferRecipe.DIE_SIZE_Y;
							break;
						case XYDirection.RightBottom:
							iDieX = this.m_WaferRecipe.ORIGIN_DIE_X - (idx - this.m_WaferRecipe.ORIGIN_DIE_X);
							iDieY = idy;
							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X-1) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) * this.m_WaferRecipe.DIE_SIZE_Y;
							break;
						case XYDirection.RightTop:
							iDieX = this.m_WaferRecipe.ORIGIN_DIE_X - (idx - this.m_WaferRecipe.ORIGIN_DIE_X);
							iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) - 1;
							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X - 1) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y - 1) * this.m_WaferRecipe.DIE_SIZE_Y;
							break;
					}
				
					if(UseDie(dX - m_WaferRecipe.ORIGIN_X,dY + this.m_WaferRecipe.DIE_SIZE_Y - m_WaferRecipe.ORIGIN_Y,this.m_WaferRecipe.DIE_SIZE_X,this.m_WaferRecipe.DIE_SIZE_Y))
					{
						oDie.IndexX = iDieX;
						oDie.IndexY = iDieY;
						oDie.BinNumber = iBin;
						oDie.DieCood.X = dX;
						oDie.DieCood.Y = dY;
						oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
						oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
						oDie.DieProp = 1;
						oDie.ZoneNumber = 0;
						m_arrDies.Add(oDie);
						this.m_WaferRecipe.NETDIE++;
					}
					else
					{
						oDie.IndexX = iDieX;
						oDie.IndexY = iDieY;
						oDie.BinNumber = iBin;
						oDie.DieCood.X = dX;
						oDie.DieCood.Y = dY;
						oDie.DieCood.Width = this.m_WaferRecipe.DIE_SIZE_X;
						oDie.DieCood.Height = this.m_WaferRecipe.DIE_SIZE_Y;
						oDie.DieProp = 1;
						oDie.ZoneNumber = 0;
						m_arrDies.Add(oDie);
					}
				}
			}
		}

		protected bool UseDie(double X,double Y,double Width,double Height)
		{
			int iRealAngle = (m_WaferRecipe.ANGLE + m_iAngleOffset) % 360;
			switch(iRealAngle)
			{
				case 0:
					if(m_rectNotchArea.Top > Y || m_rectNotchArea.Top > Y+Height) return false;
					break;
				case 90:
					if(m_rectNotchArea.Right > X || m_rectNotchArea.Right > X+Width) return false;
					break;
				case 180:
					if(m_rectNotchArea.Bottom < Y || m_rectNotchArea.Bottom < Y+Height) return false;
					break;
				case 270:
					if(m_rectNotchArea.Left < X || m_rectNotchArea.Left < X+Width) return false;
					break;
			}

			if( (m_WaferRecipe.WAFER_SIZE/2 - m_WaferRecipe.EDGE_SIZE) < Math.Sqrt(Math.Pow(X,2) + Math.Pow(Y+Height,2))  ) return false;
			if( (m_WaferRecipe.WAFER_SIZE/2 - m_WaferRecipe.EDGE_SIZE) < Math.Sqrt(Math.Pow(X+Width,2) + Math.Pow(Y+Height,2)) ) return false;
			if( (m_WaferRecipe.WAFER_SIZE/2 - m_WaferRecipe.EDGE_SIZE) < Math.Sqrt(Math.Pow(X,2) + Math.Pow(Y,2))  ) return false;
			if( (m_WaferRecipe.WAFER_SIZE/2 - m_WaferRecipe.EDGE_SIZE) < Math.Sqrt(Math.Pow(X+Width,2) + Math.Pow(Y,2)) ) return false;

			return true;
		}

		#endregion

		#region ■ Die Array Handling 함수 ( DieClear / AddDie(Die NewDie) )
		public virtual void DieClear()
		{
			m_arrDies.Clear();
			m_DieIndexer.Clear();
		}

		public void AddDie(Die NewDie)
		{

			int iDieX = 0;
			int iDieY = 0;

			switch(m_WaferRecipe.XYDIR)
			{
				case XYDirection.LeftTop:
					iDieX = NewDie.IndexX;
					iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y + (this.m_WaferRecipe.ORIGIN_DIE_Y - NewDie.IndexY) - 1;
					break;
				case XYDirection.LeftBottom:
					iDieX = NewDie.IndexX;
					iDieY = NewDie.IndexY;
					break;
				case XYDirection.RightBottom:
					iDieX = this.m_WaferRecipe.ORIGIN_DIE_X + (this.m_WaferRecipe.ORIGIN_DIE_X - NewDie.IndexX) - 1;
					iDieY = NewDie.IndexY;
					break;
				case XYDirection.RightTop:
					iDieX = this.m_WaferRecipe.ORIGIN_DIE_X + (this.m_WaferRecipe.ORIGIN_DIE_X - NewDie.IndexX) - 1;
					iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y + (this.m_WaferRecipe.ORIGIN_DIE_Y - NewDie.IndexY) - 1;
					break;
			}

			NewDie.DieCood.X = (iDieX - m_WaferRecipe.ORIGIN_DIE_X) * m_WaferRecipe.DIE_SIZE_X;
			NewDie.DieCood.Y = (iDieY - m_WaferRecipe.ORIGIN_DIE_Y) * m_WaferRecipe.DIE_SIZE_Y;
			NewDie.DieCood.Width = m_WaferRecipe.DIE_SIZE_X;
			NewDie.DieCood.Height = m_WaferRecipe.DIE_SIZE_Y;

			if(m_arrDies.Count==0) 
			{
				m_WaferRecipe.DIE_INDEX_MAX_X = m_WaferRecipe.DIE_INDEX_MIN_X = NewDie.IndexX;
				m_WaferRecipe.DIE_INDEX_MAX_Y = m_WaferRecipe.DIE_INDEX_MIN_Y = NewDie.IndexY;
			}
			else
			{
				if(NewDie.IndexX < m_WaferRecipe.DIE_INDEX_MIN_X) m_WaferRecipe.DIE_INDEX_MIN_X = NewDie.IndexX;
				if(NewDie.IndexY < m_WaferRecipe.DIE_INDEX_MIN_Y) m_WaferRecipe.DIE_INDEX_MIN_Y = NewDie.IndexY;
				if(NewDie.IndexX > m_WaferRecipe.DIE_INDEX_MAX_X) m_WaferRecipe.DIE_INDEX_MAX_X = NewDie.IndexX;
				if(NewDie.IndexY > m_WaferRecipe.DIE_INDEX_MAX_Y) m_WaferRecipe.DIE_INDEX_MAX_Y = NewDie.IndexY;
			}

			m_arrDies.Add(NewDie);
			m_DieIndexer.Add(new Point(NewDie.IndexX,NewDie.IndexY));
		}
		#endregion
		
		#region ▣ Redraw / Paint / Resize Event처리 함수
		protected virtual void WaferMap_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			try
			{
				if(m_gdiTempMap == null) return;
				m_gdiMain.DrawImageUnscaled(m_bmpWaferMap,0,0);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}


		protected void WaferMap_Resize(object sender, System.EventArgs e)
		{
			if(this.Width * this.Height == 0)
			{
				return;
			}

			float dMinCanvers = Math.Min(this.Width,this.Height);
			m_dZoomRatio = dMinCanvers/m_WaferRecipe.WAFER_SIZE;
			m_rectdWaferArea = new RectangleD(((dMinCanvers - this.Width)/2.0f)/(m_dZoomRatio * m_dScale),((dMinCanvers - this.Height)/2.0f)/(m_dZoomRatio * m_dScale),m_WaferRecipe.WAFER_SIZE,m_WaferRecipe.WAFER_SIZE);
		
			if(m_bmpWaferMap != null) m_bmpWaferMap.Dispose();
			if(m_gdiTempMap != null) m_gdiTempMap.Dispose();

			m_gdiMain = this.CreateGraphics();
			m_bmpWaferMap = new Bitmap(this.Width,this.Height);
			m_bmpTemp = new Bitmap(m_bmpWaferMap);
			m_gdiTempMap = Graphics.FromImage(m_bmpWaferMap);
			Redraw();
			GC.Collect();
		}

		
		public virtual void Redraw()
		{
			DrawWafer();
			WaferMap_Paint(null,null);
		}

		#endregion

		#region ■ Information Drawing 함수

		public void AddInfomation(string strInfo)
		{
			if(m_strInfomation == null) m_strInfomation = new ArrayList();
			else
			{
				if(m_strInfomation.IndexOf(strInfo)>-1) return;
			}

			m_strInfomation.Add(strInfo);
		}

		public void SetInfomation(string[] strInfos)
		{
			if(m_strInfomation == null) m_strInfomation = new ArrayList();
			m_strInfomation.Clear();
			for(int i=0;i<strInfos.Length;i++) m_strInfomation.Add(strInfos[i]);

		}

		#endregion

		#region ■ Die Drawing
		protected virtual void DrawDies(Graphics g)
		{
			if(m_arrDies.Count<=0) return;
			m_WaferRecipe.NETDIE = 0;

			PointD[] pdPoint = new PointD[4];
			PointF[] pfPoint = new PointF[4];

			/// Pen / Brush는 Dispose해야 하므로 Try밖에 선언하고 Finally에서 Dispose를 ...
			/// 메모리 확인해 본결과 효과 만점...
			Pen pSelDieBorder = null;
			Pen pDieBorder = null;
			Pen pOriginDieBorder = null;
			Pen pFirstDieBorder = null;

			SolidBrush sbrshDie = null;

			bool bOnWafer = false;
			try
			{
				pSelDieBorder = new Pen(Color.Red,3);
				pDieBorder = new Pen(m_colDieBorder);
				pOriginDieBorder = new Pen(m_colOriginDieBorder);
				pFirstDieBorder = new Pen(m_colFirstDieBorder);
				sbrshDie	= new SolidBrush(Color.White);

				foreach(Die InDie in m_arrDies)
				{
					if(InDie.BinNumber>100) continue;
					/// Die의 4 점의 좌표를 계산한다. ////////////////////////////////////////////////////////////////////////////////////////
					pdPoint[0].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
					pdPoint[0].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

					pdPoint[1].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
					pdPoint[1].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;
				
					pdPoint[2].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
					pdPoint[2].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;
				
					pdPoint[3].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
					pdPoint[3].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

					RotatePoint(ref pdPoint[0].X,ref pdPoint[0].Y,m_iViewAngle);
					RotatePoint(ref pdPoint[1].X,ref pdPoint[1].Y,m_iViewAngle);
					RotatePoint(ref pdPoint[2].X,ref pdPoint[2].Y,m_iViewAngle);
					RotatePoint(ref pdPoint[3].X,ref pdPoint[3].Y,m_iViewAngle);
 
					pfPoint[0].X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + pdPoint[0].X ) * (m_dZoomRatio * m_dScale));
					pfPoint[0].Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - pdPoint[0].Y ) * (m_dZoomRatio * m_dScale));

					pfPoint[1].X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + pdPoint[1].X ) * (m_dZoomRatio * m_dScale));
					pfPoint[1].Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - pdPoint[1].Y ) * (m_dZoomRatio * m_dScale));

					pfPoint[2].X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + pdPoint[2].X ) * (m_dZoomRatio * m_dScale));
					pfPoint[2].Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - pdPoint[2].Y ) * (m_dZoomRatio * m_dScale));

					pfPoint[3].X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + pdPoint[3].X ) * (m_dZoomRatio * m_dScale));
					pfPoint[3].Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - pdPoint[3].Y ) * (m_dZoomRatio * m_dScale));
					////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				
					bOnWafer = this.UseDie(InDie.DieCood.X - m_WaferRecipe.ORIGIN_X
						,InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y
						,this.m_WaferRecipe.DIE_SIZE_X
						,this.m_WaferRecipe.DIE_SIZE_Y);

					/// 0 Skip / 1 Probing / 2 Mark Die를 설정값에 맞춰서 Drawing한다. 
					/// 
					if(InDie.DieProp == 1)
					{
						if(Array.IndexOf(m_strSelectedBin,InDie.BinNumber.ToString()) > -1)
						{
							sbrshDie.Color = Color.FromArgb(m_iSelectedBinAlpha,m_ColorSet[InDie.BinNumber]);
						}
						else
						{
							sbrshDie.Color = Color.FromArgb(m_iNotSelectedBinAlpha,m_ColorSet[InDie.BinNumber]);
						}

						m_gdiTempMap.FillPolygon(sbrshDie,pfPoint);
						if(m_dZoomRatio>0.6 && m_bVisibleDieBorder)
						{
							g.DrawPolygon(pDieBorder,pfPoint);
						}

						m_WaferRecipe.NETDIE++;
					}
				}

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				pdPoint = null;
				pfPoint = null;

				if(pSelDieBorder != null) pSelDieBorder.Dispose();
				if(pDieBorder != null)	pDieBorder.Dispose();
				if(pOriginDieBorder != null)	pOriginDieBorder.Dispose();
				if(pFirstDieBorder != null)	pFirstDieBorder.Dispose();
				if(sbrshDie != null) sbrshDie.Dispose();
			}
		}

	
		protected void RedrawDie(Die argDie,Color colDie,Color colBorder)
		{
			if(m_gdiMain == null || m_bmpWaferMap == null) return;
			try
			{

				PointD[] pdPoint = new PointD[4];

				pdPoint[0].X = argDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
				pdPoint[0].Y = argDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

				pdPoint[1].X = argDie.DieCood.X + argDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
				pdPoint[1].Y = argDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;
			
				pdPoint[2].X = argDie.DieCood.X + argDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
				pdPoint[2].Y = argDie.DieCood.Y + argDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;
			
				pdPoint[3].X = argDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
				pdPoint[3].Y = argDie.DieCood.Y + argDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

				RotatePoint(ref pdPoint[0].X,ref pdPoint[0].Y,m_iViewAngle);
				RotatePoint(ref pdPoint[1].X,ref pdPoint[1].Y,m_iViewAngle);
				RotatePoint(ref pdPoint[2].X,ref pdPoint[2].Y,m_iViewAngle);
				RotatePoint(ref pdPoint[3].X,ref pdPoint[3].Y,m_iViewAngle);

				PointF[] pfPoint = new PointF[4];
				pfPoint[0].X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + pdPoint[0].X ) * (m_dZoomRatio * m_dScale));
				pfPoint[0].Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - pdPoint[0].Y ) * (m_dZoomRatio * m_dScale));

				pfPoint[1].X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + pdPoint[1].X ) * (m_dZoomRatio * m_dScale));
				pfPoint[1].Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - pdPoint[1].Y ) * (m_dZoomRatio * m_dScale));

				pfPoint[2].X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + pdPoint[2].X ) * (m_dZoomRatio * m_dScale));
				pfPoint[2].Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - pdPoint[2].Y ) * (m_dZoomRatio * m_dScale));

				pfPoint[3].X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + pdPoint[3].X ) * (m_dZoomRatio * m_dScale));
				pfPoint[3].Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - pdPoint[3].Y ) * (m_dZoomRatio * m_dScale));

				m_gdiMain.DrawImageUnscaled(m_bmpWaferMap,0,0);
				m_gdiMain.DrawPolygon(new Pen(colBorder),pfPoint);

			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		#region ■ Default Color
		protected virtual void SetDefaultColor()
		{
			int r,g,b;//4337915
			//16777215
			try
			{
                int[] iColor = new int[]	{16766976,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
												65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
												16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
												10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
												8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
												6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
												12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
												10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
												2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
												10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686
												,2521304,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
												65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
												16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
												10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
												8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
												6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
												12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
												10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
												2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
												10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686,
												6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
												12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
												10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
												2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
												10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686,
                                                8421504,4227136,12632256,9868950,6795178,4446555};
				
				if(m_ColorSet != null) m_ColorSet = null;
				m_ColorSet = new Color[iColor.Length];
				for(int i=0; i< iColor.Length;i++)
				{
					r = (iColor[i]>>16);
					g = (iColor[i]>>8) - (r * 256);
					b = iColor[i] - (r * 65536) - (g * 256);
					m_ColorSet[i] = Color.FromArgb(r,g,b);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
		}

		#endregion

		public void SetColor(int Index,Color BinColor)
		{
			m_ColorSet[Index] = BinColor;
		}

		public void SetColor(int Index,Color BinColor,int Alpha)
		{
			m_ColorSet[Index] = Color.FromArgb(Alpha,BinColor);;
		}

		public Color GetColor(int Index)
		{
			return m_ColorSet[Index];
		}

		public Bitmap GetMapImage()
		{
			return m_bmpWaferMap;
		}

		public float GetPieYield(int Slice)
		{
			GraphicsPath SelectPath = null;
			Rectangle rectSelect = Rectangle.Empty;
			int iGec = 0;
			int iTotalCount = 0;
			try
			{
				SelectPath = new GraphicsPath();
				rectSelect.X = (int)(-m_rectdWaferArea.X * (m_dZoomRatio * m_dScale));
				rectSelect.Y = (int)(-m_rectdWaferArea.Y * (m_dZoomRatio * m_dScale));
				rectSelect.Width = (int)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale));
				rectSelect.Height = (int)(m_WaferRecipe.WAFER_SIZE * (m_dZoomRatio * m_dScale));
				SelectPath.AddPie(rectSelect,(float)Slice * 20.0f,20.0f);

				CalSelectedDie(SelectPath,ref iGec,ref iTotalCount);

				return (float)iGec/(float)iTotalCount * 100.0f;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(SelectPath != null) SelectPath.Dispose();
				SelectPath = null;
			}
		}

		private void CalSelectedDie(GraphicsPath SelectPath,ref int GecCount,ref int TotalCount)
		{
			PointD pdDieCenter;
			GecCount = 0;
			TotalCount = 0;
			try
			{
				foreach(Die InDie in m_arrDies)
				{
					pdDieCenter.X = (InDie.DieCood.X - m_WaferRecipe.ORIGIN_X) + InDie.DieCood.Width/2;
					pdDieCenter.Y = (InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y) + InDie.DieCood.Height/2;

					RotatePoint(ref pdDieCenter.X,ref pdDieCenter.Y,m_iViewAngle);
 
					PointF pfPoint = PointF.Empty;
					pfPoint.X = (float)((- m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE/2 + pdDieCenter.X ) * (m_dZoomRatio * m_dScale));
					pfPoint.Y = (float)((- m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE/2 - pdDieCenter.Y ) * (m_dZoomRatio * m_dScale));
				
					if(SelectPath.IsVisible(pfPoint))
					{
						if(Array.IndexOf(m_strSelectedBin,InDie.BinNumber.ToString()) > -1) GecCount++;
						TotalCount++;
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		////////////////////END
	}
}
