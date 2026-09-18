using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using DACrux.Base;

namespace DACrux.Map
{

	/// <summary>
	/// First Die를 재 정의 했을때 재정의된 Die의 정보를 전달하는 Event의 대리자
	/// </summary>
	public delegate void TESTRedefineFirstDie(object sender,Die NewDie);
	/// <summary>
	/// Die의 정보를 재 정의 했을때 재정의된 Die의 정보를 전달하는 Event의 대리자
	/// </summary>
	public delegate void TESTChangeDieProperty(object sender,Die NewDie);

    /// <summary>
    /// Die Bin 번호를 재 정의 헸을때 기존 Bin 번호와 재 정의된 Bin 번호를 전달하는 Event 대리자
    /// </summary>
    public delegate void ChangeDieBinNumber(object sender, ChangeDieBinNumberInfo e);

	/// <summary>
	/// EditTESTMap의 Die 추가 및 삭제 Mode의 열거자
	/// </summary>
	public enum EditDieMode {SKIP,ADD,DELETE,MARK,FIRSTMARK,KILL,UNKILL,DEFECT};

    public class EditTESTMap : DACrux.Map.WaferMap
	{
		private System.ComponentModel.IContainer components = null;

		private EditDieMode m_eoEditDieMode = EditDieMode.ADD;
		public event TESTRedefineFirstDie OnTESTRedefineFirstDie;
		public event TESTChangeDieProperty OnTESTChangeDieProperty;
        public event ChangeDieBinNumber OnChangeDieBinNumber;

		private double m_dMargin = 0.95d;
		private int m_iCurrentFailNumber = 0;
		private bool m_bPopupMenuVisible = true;
        private bool m_bIqcFqcBinCheck = false;     // TESNA 전용 로직으로 IQC 또는 FQC 공정에서 Bin을 체크한다.
        private System.Windows.Forms.MenuItem mnuitem_SPRIT;
	
		private string m_strVIMember = "VIFAIL";

		private int[] m_iKeyValue = new int[] {30,31,32,33,34 ,35,36,37,38,39 ,40,41,42,43,44 ,45,46,47,48,49,50,51};
        private int m_iKeyNum = 0;
        private string m_iKeyIn = string.Empty;
        private string m_ScopeImageLocal = string.Empty;
        private string m_ScopeImage = string.Empty;
        private string m_ScopeImagePath = string.Empty;

		private bool m_bAutoFocus = false;
		private bool m_bVIMode = false;
        private bool m_bEFR = false;

        protected bool m_bDrawGradationDie = false;
        protected string m_sParametricColumn = "PCMVALUE";

        public EditTESTMap()
        {
            InitializeComponent();
            this.WaferDrawMode = MapMode.Edit;
        }

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					if(m_DieIndexer != null) m_DieIndexer = null;
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		
		public bool PopupMenuVisible
		{
			set{m_bPopupMenuVisible=value;}
			get{return m_bPopupMenuVisible;}
		}

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IqcFqcBinCheck
		{
            set { m_bIqcFqcBinCheck = value; }
            get { return m_bIqcFqcBinCheck; }
		}

		[Category("Wafer Option"), Description("Mouse가 Drag 될때 자동으로 Focus를 갖는지 여부늘 설정하거나 가져옵니다.")]
		public bool AutoFocus
		{
			set
			{
				m_bAutoFocus = value;
			}
			get
			{
				return m_bAutoFocus;
			}
		}

		[Category("Wafer Option"), Description("Visual Fail에 대한 Marking을 할 수 있는지 여부늘 설정하거나 가져옵니다.")]
		public bool IsVIEdit
		{
			set
			{
				m_bVIMode = value;
			}
			get
			{
				return m_bVIMode;
			}
		}

		[Category("Wafer Option"), Description("Wafer의 Edit Mode을 설정하거나 가져옵니다.")]
		public EditDieMode EditMethod
		{
			set
            {
				m_eoEditDieMode = value; 
			}
			get{return m_eoEditDieMode;}
		}

		[Category("Wafer Option"), Description("Wafer의 Drawing Mode을 설정하거나 가져옵니다.")]
		public override MapMode WaferDrawMode
		{
			set
			{
				m_eoMapMode = value;
				switch(m_eoMapMode)
				{
					case MapMode.Edit:
						base.WaferDrawMode = MapMode.Edit;
						//mnuitemMAPMODE_EDIT_Click(null,null);
                        mnuitemEditDieMode_SINGLE_SUBTRACTION_Click(null, null);
						break;
					case MapMode.Fit:
						mnuitemMAPMODE_FITSIZE_Click(null,null);
						break;
					case MapMode.Free:
						mnuitemMAPMODE_FREEZOOM_Click(null,null);
						break;
				}
			}
			get{return m_eoMapMode;}
		}

        public int SelectKey
        {
            set { m_iKeyNum = value; }
        }

        public string SelectKeyIn
        {
            set { m_iKeyIn = value; }
        }

        public string SelectScopeImageLocal
        {
            get { return m_ScopeImageLocal; }
            set { m_ScopeImageLocal = value; }
        }

        public string SelectScopeImage
        {
            get { return m_ScopeImage; }
            set { m_ScopeImage = value; }
        }

        public string SelectScopeImagePath
        {
            get { return m_ScopeImagePath; }
            set { m_ScopeImagePath = value; }
        }

        public bool EFR
        {
            set { m_bEFR = value; }
        }

		public double Margin
		{
			set{m_dMargin = value/100.0d;}
			get{return m_dMargin * 100.0d;}
		}

		public int CurrentFailNumber
		{
			set{m_iCurrentFailNumber = value;}
			get{return m_iCurrentFailNumber;}
		}

        [Category("Die 속성"), DefaultValue(false), Description("Die 색상을 Parametric Value를 기준으로 그라데이션으로 표시할지 여부를 설정하거나 가져옵니다.")]
        public bool EditDrawGradationDie
        {
            set { m_bDrawGradationDie = value; }
            get { return m_bDrawGradationDie; }
        }

        [Category("Die 속성"), Description("Parametric Value의 컬럼명을 설정하거나 가져옵니다.")]
        public string EditParametricColumn
        {
            set { m_sParametricColumn = value; }
            get { return m_sParametricColumn; }
        }
		
		public override object DataSource
		{
			set
			{
				if(value == null) return;
				m_DT = (DataTable)value;
				this.DieClear();

                if (m_DT.Columns.IndexOf("VI") > -1)
                {
                    m_strVIMember = "VI"; //Default "VIFAIL" => "VI"
                }

				for(int i=0;i<m_DT.Rows.Count;i++)
				{
                    Die oNewDie = new Die(DACrux.Base.Convert.intParse(m_DT.Rows[i]["X"].ToString()), DACrux.Base.Convert.intParse(m_DT.Rows[i]["Y"].ToString()), 0, 1);//DACrux.Base.Convert.intParse(m_DT.Rows[i]["USECODE"].ToString()));
					if(m_DT.Columns.IndexOf("VI")>-1)
					{
						m_strVIMember = "VI";
						oNewDie.VIFail = DACrux.Base.Convert.intParse(m_DT.Rows[i]["VI"].ToString());
					}

                    //if(m_DT.Columns.IndexOf("VI")>-1) oNewDie.DiePassFail = DACrux.Base.Convert.intParse(m_DT.Rows[i]["VI"].ToString());
                    if (m_DT.Columns.IndexOf("HIGH_GEC") > -1) oNewDie.DiePassFail = (m_DT.Rows[i]["HIGH_GEC"].ToString() == "T" ? 0 : 1);
					if(m_DT.Columns.IndexOf("BIN")>-1) oNewDie.BinNumber = DACrux.Base.Convert.intParse(m_DT.Rows[i]["BIN"].ToString());
					if(m_DT.Columns.IndexOf("AVI")>-1) oNewDie.AVIFailNumber = DACrux.Base.Convert.intParse(m_DT.Rows[i]["AVI"].ToString());

                    if (m_DT.Columns.IndexOf("PF_FLAG") > -1) oNewDie.DiePassFail = (m_DT.Rows[i]["PF_FLAG"].ToString() == "P" ? 0 : 1);
                    if (m_DT.Columns.IndexOf("PF") > -1) oNewDie.DiePassFail = (m_DT.Rows[i]["PF"].ToString() == "P" ? 0 : 1);
                    if (m_DT.Columns.IndexOf("SHOT_NO") > -1 && m_DT.Rows[i]["SHOT_NO"].ToString() != string.Empty) oNewDie.ShotID = DACrux.Base.Convert.intParse(m_DT.Rows[i]["SHOT_NO"].ToString());
                    if (m_DT.Columns.IndexOf("DUT_ID") > -1 && m_DT.Rows[i]["DUT_ID"].ToString() != string.Empty) oNewDie.SiteNumber = DACrux.Base.Convert.intParse(m_DT.Rows[i]["DUT_ID"].ToString());
                    //if (m_DT.Columns.IndexOf("CHAR_BIN") > -1 && m_DT.Rows[i]["CHAR_BIN"].ToString() != string.Empty) oNewDie.BinChar = m_DT.Rows[i]["CHAR_BIN"].ToString();
                    //if (m_DT.Columns.IndexOf("BIN_CHAR") > -1 && m_DT.Rows[i]["BIN_CHAR"].ToString() != string.Empty) oNewDie.BinChar = m_DT.Rows[i]["BIN_CHAR"].ToString();
                    //if (m_DT.Columns.IndexOf("REPROB_BIN") > -1 && m_DT.Rows[i]["REPROB_BIN"].ToString() != string.Empty) oNewDie.ReProbingChar = m_DT.Rows[i]["REPROB_BIN"].ToString();
                    if (m_bDrawGradationDie == true && m_DT.Columns.IndexOf(m_sParametricColumn) > -1)
                    {
                        if (m_DT.Rows[i][m_sParametricColumn].ToString() != string.Empty)
                            oNewDie.ParametricValue = DACrux.Base.Convert.doubleParse(m_DT.Rows[i][m_sParametricColumn.ToUpper()].ToString());
                        else
                            oNewDie.ParametricValue = double.NaN;
                    }

					this.AddDie(oNewDie);
				}
			}get
			 {
				 return m_DT;
			 }
		}

        public DataTable ModifyDieDataSource
        {
            set
            {
                DataTable dt = (DataTable)value;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Die oNewDie = new Die(DACrux.Base.Convert.intParse(dt.Rows[i]["X"].ToString())
                        , DACrux.Base.Convert.intParse(dt.Rows[i]["Y"].ToString()), 0, DACrux.Base.Convert.intParse(dt.Rows[i]["USECODE"].ToString()));
                    if (dt.Columns.IndexOf("VI") > -1)
                    {
                        oNewDie.VIFail = DACrux.Base.Convert.intParse(dt.Rows[i]["VI"].ToString());
                    }

                    if (dt.Columns.IndexOf("HIGH_GEC") > -1) oNewDie.DiePassFail = (dt.Rows[i]["HIGH_GEC"].ToString() == "T" ? 0 : 1);
                    if (dt.Columns.IndexOf("BIN") > -1) oNewDie.BinNumber = DACrux.Base.Convert.intParse(dt.Rows[i]["BIN"].ToString());
                    if (dt.Columns.IndexOf("AVI") > -1) oNewDie.AVIFailNumber = DACrux.Base.Convert.intParse(dt.Rows[i]["AVI"].ToString());

                    m_arrModifyDies.Add(oNewDie);
                    m_DieModifyIndexer.Add(new Point(DACrux.Base.Convert.intParse(dt.Rows[i]["X"].ToString()), DACrux.Base.Convert.intParse(dt.Rows[i]["Y"].ToString())));
                }                
            }
        }
        
		#region 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.mnuitem_SPRIT = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // ctxmWaferMap
            // 
            this.ctxmWaferMap.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuitem_SPRIT});
            // 
            // mnuitemMAPMODE_FREEZOOM
            // 
            this.mnuitemMAPMODE_FREEZOOM.Click += new System.EventHandler(this.mnuitemMAPMODE_FREEZOOM_Click);
            // 
            // mnuitemMAPMODE_FITSIZE
            // 
            this.mnuitemMAPMODE_FITSIZE.Click += new System.EventHandler(this.mnuitemMAPMODE_FITSIZE_Click);
            // 
            // mnuitemMOUSEDRAGMODE_ROTATE
            // 
            this.mnuitemMOUSEDRAGMODE_ROTATE.Click += new System.EventHandler(this.mnuitemMOUSEDRAGMODE_ROTATE_Click);
            // 
            // mnuitemMOUSEDRAGMODE_ZONE
            // 
            this.mnuitemMOUSEDRAGMODE_ZONE.Visible = false;
            // 
            // mnuitem_SPRIT3
            // 
            this.mnuitem_SPRIT3.Index = 9;
            // 
            // mnuitem_SPRIT
            // 
            this.mnuitem_SPRIT.Index = 12;
            this.mnuitem_SPRIT.Text = "-";
            this.mnuitem_SPRIT.Visible = false;
            // 
            // EditTESTMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "EditTESTMap";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.WaferMap_Paint);
            this.ResumeLayout(false);

		}
		#endregion

		public void DieMinMaxCalculation()
		{
			double dWaferR = m_WaferRecipe.WAFER_SIZE / 2.0d;

			this.m_WaferRecipe.DIE_INDEX_MIN_X = (int)((- dWaferR + this.m_WaferRecipe.ORIGIN_X) / this.m_WaferRecipe.DIE_SIZE_X) + this.m_WaferRecipe.ORIGIN_DIE_X -1;
			this.m_WaferRecipe.DIE_INDEX_MIN_Y = (int)((- dWaferR + this.m_WaferRecipe.ORIGIN_Y) / this.m_WaferRecipe.DIE_SIZE_Y) + this.m_WaferRecipe.ORIGIN_DIE_Y;
			this.m_WaferRecipe.DIE_INDEX_MAX_X = (int)((dWaferR + this.m_WaferRecipe.ORIGIN_X) / this.m_WaferRecipe.DIE_SIZE_X) + this.m_WaferRecipe.ORIGIN_DIE_X;
			this.m_WaferRecipe.DIE_INDEX_MAX_Y = (int)((dWaferR + this.m_WaferRecipe.ORIGIN_Y) / this.m_WaferRecipe.DIE_SIZE_Y) + this.m_WaferRecipe.ORIGIN_DIE_Y+1;
		}

		public void SetVIValue(int Index , int Val)
		{
			m_iKeyValue[Index] = Val;
		}

		public int GetVIValue(int Index)
		{
			return m_iKeyValue[Index];
		}
        
		public override void DieCalculation(bool isReal)
		{
			if(!isReal)
			{
				this.AutoDevDef();
				return;
			}

			int iDieX = 0;
			int iDieY = 0;
			
			double dX = 0.0d;
			double dY = 0.0d;

			int iBin = 0;
			m_arrDies.Clear();
            m_arrModifyDies.Clear();
			m_DieIndexer.Clear();

			this.m_WaferRecipe.NETDIE = 0;
			
			for(int idx=this.m_WaferRecipe.DIE_INDEX_MIN_X;idx<=this.m_WaferRecipe.DIE_INDEX_MAX_X;idx++)
			{
				for(int idy=this.m_WaferRecipe.DIE_INDEX_MIN_Y;idy<=this.m_WaferRecipe.DIE_INDEX_MAX_Y;idy++)
				{				
					switch(m_WaferRecipe.XYDIR)
					{
						case XYDirection.LeftTop:
							iDieX = idx;
							iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) - ((this.m_WaferRecipe.YDIES+1) % 2);

							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y - ((this.m_WaferRecipe.YDIES) % 2)) * this.m_WaferRecipe.DIE_SIZE_Y;
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
							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X - ((this.m_WaferRecipe.XDIES) % 2)) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) * this.m_WaferRecipe.DIE_SIZE_Y;
							break;
						case XYDirection.RightTop:
							iDieX = this.m_WaferRecipe.ORIGIN_DIE_X - (idx - this.m_WaferRecipe.ORIGIN_DIE_X);
							iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) - ((this.m_WaferRecipe.YDIES+1) % 2);
							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X - ((this.m_WaferRecipe.XDIES) % 2)) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y - ((this.m_WaferRecipe.YDIES) % 2)) * this.m_WaferRecipe.DIE_SIZE_Y;
							break;
					}

					if(UseDie(dX - m_WaferRecipe.ORIGIN_X,dY - m_WaferRecipe.ORIGIN_Y,this.m_WaferRecipe.DIE_SIZE_X,this.m_WaferRecipe.DIE_SIZE_Y))
					{
						m_arrDies.Add(new Die(iDieX,iDieY,iBin,dX,dY,this.m_WaferRecipe.DIE_SIZE_X,this.m_WaferRecipe.DIE_SIZE_Y,2));
						this.m_WaferRecipe.NETDIE++;
					}
					else
					{
						m_arrDies.Add(new Die(iDieX,iDieY,iBin,dX,dY,this.m_WaferRecipe.DIE_SIZE_X,this.m_WaferRecipe.DIE_SIZE_Y,-1));
					}
					m_DieIndexer.Add(new Point(iDieX,iDieY));
				}
			}
		}

        public void ReDraw_TestDie(DataTable dtDie)
        {
            Point poKey = Point.Empty;
			int iDieIdx = 0;
			if(iDieIdx < 0) return;
			Die oDie;
			try
			{
                foreach (DataRow dr in dtDie.Rows)
                {
                    poKey.X = System.Convert.ToInt32(dr["X"].ToString());
                    poKey.Y = System.Convert.ToInt32(dr["Y"].ToString());

                    iDieIdx = m_DieIndexer.IndexOf(poKey);

                    oDie = (Die)this.m_arrDies[iDieIdx];
                    oDie.DieProp = 1;

                    this.m_arrDies.RemoveAt(iDieIdx);
                    this.m_arrDies.Insert(iDieIdx, oDie);
                }

                this.Redraw();
			}
			catch(Exception)
			{
				// 여기는 너무 빈번한 Event 이므로 Error 무시해야 함
			}
			finally
			{
				poKey = Point.Empty;
			}
        }

		private void AutoDevDef()
		{
			int iDieX = 0;
			int iDieY = 0;
			
			double dX = 0.0d;
			double dY = 0.0d;

			int iBin = 0;
			m_arrDies.Clear();
            m_arrModifyDies.Clear();
			m_DieIndexer.Clear();


			this.m_WaferRecipe.NETDIE = 0;
			this.m_WaferRecipe.DIE_SIZE_X = (this.m_WaferRecipe.WAFER_SIZE * m_dMargin) / (this.m_WaferRecipe.DIE_INDEX_MAX_X - this.m_WaferRecipe.DIE_INDEX_MIN_X + 1);
			this.m_WaferRecipe.DIE_SIZE_Y = (this.m_WaferRecipe.WAFER_SIZE * m_dMargin) / (this.m_WaferRecipe.DIE_INDEX_MAX_Y - this.m_WaferRecipe.DIE_INDEX_MIN_Y + 1);
			
			if((this.m_WaferRecipe.XDIES % 2) == 0)
			{
				this.m_WaferRecipe.ORIGIN_X =  0;
			}
			else
			{
				this.m_WaferRecipe.ORIGIN_X = this.m_WaferRecipe.DIE_SIZE_X - this.m_WaferRecipe.DIE_SIZE_X / 2.0d;
			}

			if((this.m_WaferRecipe.YDIES % 2) == 0)
			{
				this.m_WaferRecipe.ORIGIN_Y = 0;
			}
			else
			{
				this.m_WaferRecipe.ORIGIN_Y = - this.m_WaferRecipe.DIE_SIZE_Y / 2.0d;
			}


			this.m_WaferRecipe.ORIGIN_DIE_X = this.m_WaferRecipe.DIE_INDEX_MIN_X + (int)Math.Floor( (double)m_WaferRecipe.XDIES / 2.0d );
			this.m_WaferRecipe.ORIGIN_DIE_Y = this.m_WaferRecipe.DIE_INDEX_MIN_Y + (int)Math.Floor( (double)m_WaferRecipe.YDIES / 2.0d );

			
			for(int idx=m_WaferRecipe.DIE_INDEX_MIN_X;idx<=m_WaferRecipe.DIE_INDEX_MAX_X;idx++)
			{
				for(int idy=m_WaferRecipe.DIE_INDEX_MIN_Y;idy<=m_WaferRecipe.DIE_INDEX_MAX_Y;idy++)
				{				
					switch(m_WaferRecipe.XYDIR)
					{
						case XYDirection.LeftTop:
							iDieX = idx;
							iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) - ((this.m_WaferRecipe.YDIES+1) % 2);

							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y - ((this.m_WaferRecipe.YDIES) % 2)) * this.m_WaferRecipe.DIE_SIZE_Y;
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
							iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) - ((this.m_WaferRecipe.YDIES+1) % 2);
							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X - 1) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y - ((this.m_WaferRecipe.YDIES) % 2)) * this.m_WaferRecipe.DIE_SIZE_Y;
							break;
					}

					if(UseDie(dX - m_WaferRecipe.ORIGIN_X,dY - m_WaferRecipe.ORIGIN_Y,this.m_WaferRecipe.DIE_SIZE_X,this.m_WaferRecipe.DIE_SIZE_Y))
					{
						m_arrDies.Add(new Die(iDieX,iDieY,iBin,dX,dY,this.m_WaferRecipe.DIE_SIZE_X,this.m_WaferRecipe.DIE_SIZE_Y,2));
						this.m_WaferRecipe.NETDIE++;
					}
					else
					{
						m_arrDies.Add(new Die(iDieX,iDieY,iBin,dX,dY,this.m_WaferRecipe.DIE_SIZE_X,this.m_WaferRecipe.DIE_SIZE_Y,-1));
					}
					m_DieIndexer.Add(new Point(iDieX,iDieY));
				}
			}
		}
        		
		private void ReUseDie()
		{
			Die[] tmpDie = new Die[m_arrDies.Count];
			m_arrDies.CopyTo(tmpDie);
			m_arrDies.Clear();
            m_arrModifyDies.Clear();
			
			double dX = 0.0d;
			double dY = 0.0d;

			for(int i=0;i<tmpDie.Length;i++)
			{
				dX = tmpDie[i].IndexX * this.m_WaferRecipe.DIE_SIZE_X;
				dY = tmpDie[i].IndexY * this.m_WaferRecipe.DIE_SIZE_Y;
				
				if(tmpDie[i].DieProp != 2)
				{
					if(UseDie(dX - m_WaferRecipe.ORIGIN_X,dY - m_WaferRecipe.ORIGIN_Y,this.m_WaferRecipe.DIE_SIZE_X,this.m_WaferRecipe.DIE_SIZE_Y))
					{
						tmpDie[i].DieProp = 1;
					}
					else
					{
						tmpDie[i].DieProp = 0;
					}
				}
				m_arrDies.Add(tmpDie[i]);
			}
			tmpDie = null;
		}

		public override void Redraw()
		{
			base.Redraw();
		}

		/// <summary>
		/// Wafer Map에서 MouseDown Event를 구현합니다.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			Point poKey = Point.Empty;
			int iDieIdx = 0;
            int iDieModifyIdx = 0;
			if(iDieIdx < 0) return;
			Die oDie;
            ChangeDieBinNumberInfo oDieInfo;
			try
			{
                if (e.Button == System.Windows.Forms.MouseButtons.Left && m_eoMapMode == MapMode.Edit && m_eoMouseDragMode == MouseDragMode.Zone)
                {
                    m_poStart.X = e.X;
                    m_poStart.Y = e.Y;

                    m_poEnd.X = e.X;
                    m_poEnd.Y = e.Y;

                    m_rectSelect.Width = Math.Max(m_poStart.X, m_poEnd.X) - Math.Min(m_poStart.X, m_poEnd.X);
                    m_rectSelect.Height = Math.Max(m_poStart.Y, m_poEnd.Y) - Math.Min(m_poStart.Y, m_poEnd.Y);

                    ///  Mouse Down할때 생성되고 Up할때 Dispose 된다.
                    ///  //////////////////////////////////////////////////////////////////////////////////////
                    m_SelectPath = new System.Drawing.Drawing2D.GraphicsPath();
                    ///////////////////////////////////////////////////////////////////////////////////////////
                    ///

                    if (m_eoMouseDragMode == MouseDragMode.Zone)
                    {
                        DrawZoneGuid();
                    }
                    return;
                }

				poKey.X = m_iCurrentX;
				poKey.Y = m_iCurrentY;
				
				iDieIdx = m_DieIndexer.IndexOf(poKey);
                if (iDieIdx < 0)
                    return;

                iDieModifyIdx = m_DieModifyIndexer.IndexOf(poKey);
				if(e.Button == System.Windows.Forms.MouseButtons.Left && m_eoMapMode == MapMode.Edit)
				{
					oDie = (Die)this.m_arrDies[iDieIdx];

                    if (m_bIqcFqcBinCheck == true && oDie.BinNumber == 2)
                    {
                        //MessageBox.Show("고객사 Fail Bin 정보는 수정할 수 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (m_bIqcFqcBinCheck == false && oDie.DiePassFail != 0)
                    {
                        //MessageBox.Show("Fail Bin 정보는 수정할 수 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (oDie.BinNumber == m_iKeyNum && oDie.VIFail == 0)
                        return;

					switch(m_eoEditDieMode)
					{
						case EditDieMode.SKIP:
							oDie.DieProp = 0;
							break;
						case EditDieMode.ADD:
							oDie.DieProp = 1;
							break;
						case EditDieMode.MARK:
							oDie.DieProp = 2;
							break;
						case EditDieMode.DELETE:
							oDie.DieProp = -1;
							break;
						case EditDieMode.FIRSTMARK:
							if(OnTESTRedefineFirstDie != null) OnTESTRedefineFirstDie(this,oDie);
							break;
						case EditDieMode.KILL:
                            if (!m_bEFR)
                            {
                                //if (oDie.DiePassFail > 0 && iDieModifyIdx < 0)
                                //{
                                //    MessageBox.Show(@"Loss 정보가 이미 존재합니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    return;
                                //}

                                if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1)
                                {
                                    oDie.DiePassFail = 0;
                                    oDie.VIFail = 0;
                                }

                                m_arrDies.RemoveAt(iDieIdx);
                                m_arrDies.Insert(iDieIdx, oDie);

                                if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1)
                                {
                                    oDie.DiePassFail = 0;
                                    m_arrModifyDies.RemoveAt(iDieModifyIdx);
                                    m_DieModifyIndexer.RemoveAt(iDieModifyIdx);
                                }
                            }
                
                            if (OnTESTChangeDieProperty != null) OnTESTChangeDieProperty(this, oDie);
							break;
						case EditDieMode.UNKILL:
							oDie.VIFail = 0;
							this.m_arrDies.RemoveAt(iDieIdx);
							this.m_arrDies.Insert(iDieIdx,oDie);
							break;
                        case EditDieMode.DEFECT:
                            //if (oDie.DiePassFail > 0 && iDieModifyIdx < 0)
                            //{
                            //    MessageBox.Show(@"Loss 정보가 이미 존재합니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //    return;
                            //}

                            if (oDie.VIFail != m_iKeyNum)
                            {
                                oDieInfo.nPreDefectNumber = oDie.VIFail;
                                oDieInfo.nDefectNumber = m_iKeyNum;

                                if (OnChangeDieBinNumber != null) OnChangeDieBinNumber(this, oDieInfo);
                            }

                            oDie.VIFail = m_iKeyNum;

                            if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1 && m_iKeyNum == 0)
                            {
                                oDie.DiePassFail = 0;
                            }

                            m_arrDies.RemoveAt(iDieIdx);
                            m_arrDies.Insert(iDieIdx, oDie);

                            if (m_iKeyNum == 0)
                            {
                                if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1)
                                {
                                    oDie.DiePassFail = 0;
                                    m_arrModifyDies.RemoveAt(iDieModifyIdx);
                                    m_DieModifyIndexer.RemoveAt(iDieModifyIdx);
                                }
                            }
                            else if (iDieModifyIdx == -1)
                            {
                                m_arrModifyDies.Add(oDie);
                                m_DieModifyIndexer.Add(new Point(m_iCurrentX, m_iCurrentY));
                            }
                            else
                            {
                                m_arrModifyDies.RemoveAt(iDieModifyIdx);
                                m_arrModifyDies.Insert(iDieModifyIdx, oDie);
                            }

                            if (OnTESTChangeDieProperty != null) OnTESTChangeDieProperty(this, oDie);
                            if (m_DT != null)
                            {
                                DataRow[] drs = m_DT.Select(string.Format("X = {0} AND Y={1}", m_iCurrentX, m_iCurrentY));

                                if (!m_bEFR)
                                {
                                    if (m_DT.Columns[m_strVIMember] != null)
                                    {
                                        drs[0][m_strVIMember] = m_iKeyNum;
                                    }
                                }
                                else
                                {
                                    drs[0]["BIN"] = m_iKeyNum;
                                }
                                // 2013/03/18 User Control 사용 시 Update 정보 파악을 위해.
                                m_DT.AcceptChanges();
                            }

			                this.Validate();

                            this.KillDie(oDie, m_VIColorSet[oDie.VIFail], m_colDieBorder);
                            return;
					}

					if(m_eoEditDieMode != EditDieMode.FIRSTMARK)
					{
						this.m_arrDies.RemoveAt(iDieIdx);
						this.m_arrDies.Insert(iDieIdx,oDie);
					}

					this.Redraw();
				}
			}
			catch
			{
				// 여기는 너무 빈번한 Event 이므로 Error 무시해야 함
			}
			finally
			{
				poKey = Point.Empty;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
            Die oDie;
            try
            {
                base.OnMouseMove(e);

                if (m_iCurrentDieIndex > -1)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left && m_eoMapMode == MapMode.Edit && m_eoMouseDragMode == MouseDragMode.Zone)
                    {
                        m_poEnd.X = e.X;
                        m_poEnd.Y = e.Y;

                        m_rectSelect.X = Math.Min(m_poStart.X, m_poEnd.X);
                        m_rectSelect.Y = Math.Min(m_poStart.Y, m_poEnd.Y);
                        m_rectSelect.Width = Math.Max(m_poStart.X, m_poEnd.X) - Math.Min(m_poStart.X, m_poEnd.X);
                        m_rectSelect.Height = Math.Max(m_poStart.Y, m_poEnd.Y) - Math.Min(m_poStart.Y, m_poEnd.Y);

                        DrawZoneGuid();
                        return;
                    }

                    if (e.Button == System.Windows.Forms.MouseButtons.Left && m_eoMouseDragMode == MouseDragMode.Normal)
                    {
                        if (m_iCurrentDieIndex < 0) return;
                        oDie = (Die)this.m_arrDies[m_iCurrentDieIndex];
                        switch (m_eoEditDieMode)
                        {
                            case EditDieMode.DELETE:
                                oDie.DieProp = -1;
                                break;
                            case EditDieMode.SKIP:
                                oDie.DieProp = 0;
                                break;
                            case EditDieMode.ADD:
                                oDie.DieProp = 1;
                                break;
                            case EditDieMode.MARK:
                                oDie.DieProp = 2;
                                break;
                            case EditDieMode.DEFECT:
                                return;
                        }

                        this.m_arrDies.RemoveAt(m_iCurrentDieIndex);
                        this.m_arrDies.Insert(m_iCurrentDieIndex, oDie);
                        this.Redraw();
                    }
                    else
                    {
                        oDie = (Die)this.m_arrDies[m_iCurrentDieIndex];
                        if (m_bVisibleFocusDie) SetFocusDie(oDie.IndexX, oDie.IndexY);
                    }

                    if (m_bAutoFocus) this.Focus();
                }
            }
            catch (Exception) { }
		}

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && m_eoMapMode == MapMode.Edit && m_eoMouseDragMode == MouseDragMode.Zone)
                {
                    switch (m_eoMouseDragMode)
                    {
                        case MouseDragMode.Zone:
                            CalMultiSubtraction();
                            break;
                    }
                }
            }
            catch
            {
            }
        }

		protected override void WaferMap_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			base.WaferMap_Paint(null,null);
		}

		private void mnuitemEditDieMode_ADD_Click(object sender, System.EventArgs e)
		{
			this.m_eoEditDieMode = EditDieMode.ADD;

		}

		private void mnuitemEditDieMode_DELETE_Click(object sender, System.EventArgs e)
		{
			this.m_eoEditDieMode = EditDieMode.DELETE;	
		}

		private void mnuitemEditDieMode_MARKDIE_Click(object sender, System.EventArgs e)
		{
			this.m_eoEditDieMode = EditDieMode.MARK;	
		}

		private void mnuitemEditDieMode_MARKFIRST_Click(object sender, System.EventArgs e)
		{
			this.m_eoEditDieMode = EditDieMode.FIRSTMARK;
		}

		private void mnuitemEditDieMode_SKIP_Click(object sender, System.EventArgs e)
		{
			this.m_eoEditDieMode = EditDieMode.SKIP;
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
            base.OnKeyPress(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
            base.OnKeyUp(e);

            if (m_iCurrentX < 0 || m_iCurrentY < 0) return;

            System.Windows.Forms.Keys InputKey = e.KeyCode;

            int nXOffset = 0;
            int nYOffset = 0;
            switch (m_iViewAngle)
            {
                case 0:
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                            nYOffset = 1;
                            break;
                        case Keys.Down:
                            nYOffset = -1;
                            break;
                        case Keys.Left:
                            nXOffset = -1;
                            break;
                        case Keys.Right:
                            nXOffset = 1;
                            break;
                    }
                    break;
                case 90:
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                            nXOffset = -1;
                            break;
                        case Keys.Down:
                            nXOffset = 1;
                            break;
                        case Keys.Left:
                            nYOffset = -1;
                            break;
                        case Keys.Right:
                            nYOffset = 1;
                            break;
                    }
                    break;
                case 180:
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                            nYOffset = -1;
                            break;
                        case Keys.Down:
                            nYOffset = 1;
                            break;
                        case Keys.Left:
                            nXOffset = 1;
                            break;
                        case Keys.Right:
                            nXOffset = -1;
                            break;
                    }
                    break;
                case 270:
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                            nXOffset = 1;
                            break;
                        case Keys.Down:
                            nXOffset = -1;
                            break;
                        case Keys.Left:
                            nYOffset = 1;
                            break;
                        case Keys.Right:
                            nYOffset = -1;
                            break;
                    }
                    break;
            }

            switch (e.KeyCode)
            {
                case Keys.Up:
                    //this.SelDie(m_iCurrentX, m_iCurrentY - 1);
                    this.SelDie(m_iCurrentX + nXOffset, m_iCurrentY + nYOffset);
                    break;
                case Keys.Down:
                    //this.SelDie(m_iCurrentX, m_iCurrentY + 1);
                    this.SelDie(m_iCurrentX + nXOffset, m_iCurrentY + nYOffset);
                    break;
                case Keys.Left:
                    //this.SelDie(m_iCurrentX - 1, m_iCurrentY);
                    this.SelDie(m_iCurrentX + nXOffset, m_iCurrentY + nYOffset);
                    break;
                case Keys.Right:
                    //this.SelDie(m_iCurrentX + 1, m_iCurrentY);
                    this.SelDie(m_iCurrentX + nXOffset, m_iCurrentY + nYOffset);
                    break;
            }
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{ 
			if(!m_bVIMode) return;
            int iDieIdx = -1;
            int iDieModifyIdx = -1;
			int iKeyNum = -1;
			base.OnKeyDown (e);
			switch(e.KeyCode)
			{
				case Keys.D1:
                case Keys.NumPad1:
					iKeyNum = m_iKeyValue[1];
					break;
				case Keys.D2:
                case Keys.NumPad2:
					iKeyNum = m_iKeyValue[2];
					break;
				case Keys.D3:
                case Keys.NumPad3:
					iKeyNum = m_iKeyValue[3];
					break;
				case Keys.D4:
                case Keys.NumPad4:
					iKeyNum = m_iKeyValue[4];
					break;
				case Keys.D5:
                case Keys.NumPad5:
					iKeyNum = m_iKeyValue[5];
					break;
				case Keys.D6:
                case Keys.NumPad6:
					iKeyNum = m_iKeyValue[6];
					break;
				case Keys.D7:
                case Keys.NumPad7:
					iKeyNum = m_iKeyValue[7];
					break;
				case Keys.D8:
                case Keys.NumPad8:
					iKeyNum = m_iKeyValue[8];
					break;
				case Keys.D9:
                case Keys.NumPad9:
					iKeyNum = m_iKeyValue[9];
					break;
				case Keys.D0:
                case Keys.NumPad0:
					iKeyNum = m_iKeyValue[10];
					break;
				case Keys.F1:
					iKeyNum = m_iKeyValue[11];
					break;
				case Keys.F2:
					iKeyNum = m_iKeyValue[12];
					break;
				case Keys.F3:
					iKeyNum = m_iKeyValue[13];
					break;
				case Keys.F4:
					iKeyNum = m_iKeyValue[14];
					break;
				case Keys.F5:
					iKeyNum = m_iKeyValue[15];
					break;
				case Keys.F6:
					iKeyNum = m_iKeyValue[16];
					break;
				case Keys.F7:
					iKeyNum = m_iKeyValue[17];
					break;
				case Keys.F8:
					iKeyNum = m_iKeyValue[18];
					break;
				case Keys.F9:
					iKeyNum = m_iKeyValue[19];
					break;
				case Keys.F10:
					iKeyNum = m_iKeyValue[20];
					break;
				case Keys.Delete:
					iKeyNum = 0;
					break;
                case Keys.B:
                    iKeyNum = m_iKeyValue[21];
                    break;
                case Keys.F:
                    iKeyNum = m_iKeyValue[22];
                    break;
			}

            if (iKeyNum < 0 || m_iCurrentX < 0 || m_iCurrentY < 0)// || e.Handled == true)
			{
				return;
			}
            
            iDieIdx = m_DieIndexer.IndexOf(new Point(m_iCurrentX, m_iCurrentY));
            iDieModifyIdx = m_DieModifyIndexer.IndexOf(new Point(m_iCurrentX, m_iCurrentY));

            Die oDie = m_arrDies[iDieIdx];

            if (!m_bEFR)
            {
                ChangeDieBinNumberInfo oDieInfo = new ChangeDieBinNumberInfo();
                oDieInfo.nPreDefectNumber = oDie.VIFail;
                oDieInfo.nDefectNumber = m_iKeyNum;

                if (OnChangeDieBinNumber != null) OnChangeDieBinNumber(this, oDieInfo);

                oDie.VIFail = iKeyNum;

                //AVI 관련 Image 정보\
                oDie.ScopeImageLocal = m_ScopeImageLocal;
                oDie.ScopeImage = m_ScopeImage;
                oDie.ScopeImagePath = m_ScopeImagePath;

                if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1 && iKeyNum == 0)
                {
                    oDie.DiePassFail = 0;
                }

                m_arrDies.RemoveAt(iDieIdx);
                m_arrDies.Insert(iDieIdx, oDie);

                //Delete 및 bin0 의 경우가 중복이 되어 m_iKeyIn 추가하여 분기 시킴.
                if (iKeyNum == 0 && m_iKeyIn == "lbDel")
                {
                    if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1)
                    {
                        oDie.DiePassFail = 0;
                        m_arrModifyDies.RemoveAt(iDieModifyIdx);
                        m_DieModifyIndexer.RemoveAt(iDieModifyIdx);
                    }
                }
                else if (iDieModifyIdx == -1)
                {
                    m_arrModifyDies.Add(oDie);
                    m_DieModifyIndexer.Add(new Point(m_iCurrentX, m_iCurrentY));
                }
                else
                {
                    m_arrModifyDies.RemoveAt(iDieModifyIdx);
                    m_arrModifyDies.Insert(iDieModifyIdx, oDie);
                }
            }
            else
            {
                Die oGoodDie = new Die();

                for (int i = 0; i < m_arrDies.Count; i++)
                {
                    oGoodDie = m_arrDies[i];
                    if (oGoodDie.DiePassFail == 0) break;
                }

                // EFR이 아닌(Good Bin으로 색상이 지정되지 않은) Bin은 Pass
                if (m_ColorSet[oGoodDie.BinNumber] == m_ColorSet[oDie.BinNumber]) return;

                oDie.BinNumber = iKeyNum;
                oDie.DiePassFail = 0;

                m_arrDies.RemoveAt(iDieIdx);
                m_arrDies.Insert(iDieIdx, oDie);
            }

            if (OnTESTChangeDieProperty != null) OnTESTChangeDieProperty(this, oDie);
            if (m_DT != null)
            {
                DataRow[] drs = m_DT.Select(string.Format("X = {0} AND Y={1}", m_iCurrentX, m_iCurrentY));

                if (!m_bEFR)
                {
                    if (m_DT.Columns[m_strVIMember] != null)
                    {
                        drs[0][m_strVIMember] = iKeyNum;
                    }
                }
                else
                {
                    drs[0]["BIN"] = iKeyNum;
                }
                // 2013/03/18 User Control 사용 시 Update 정보 파악을 위해.
                m_DT.AcceptChanges();
            }

			this.Validate();

            this.KillDie(oDie, m_VIColorSet[oDie.VIFail], m_colDieBorder);
		}


        public void ChangeDieResult(int KeyNumber, int iCurrentX, int iCurrentY)
        {

            if (!m_bVIMode) return;
            int iDieIdx = -1;
            int iDieModifyIdx = -1;
            int iKeyNum = KeyNumber;

            try
            {
                if (iKeyNum < 0 || iCurrentX < 0 || iCurrentY < 0)
                {
                    return;
                }

                iDieIdx = m_DieIndexer.IndexOf(new Point(iCurrentX, iCurrentY));
                iDieModifyIdx = m_DieModifyIndexer.IndexOf(new Point(iCurrentX, iCurrentY));

                Die oDie = m_arrDies[iDieIdx];

                if (!m_bEFR)
                {
                    if (oDie.VIFail != m_iKeyNum)
                    {
                        ChangeDieBinNumberInfo oDieInfo = new ChangeDieBinNumberInfo();
                        oDieInfo.nPreDefectNumber = oDie.VIFail;
                        oDieInfo.nDefectNumber = m_iKeyNum;

                        if (OnChangeDieBinNumber != null) OnChangeDieBinNumber(this, oDieInfo);
                    }

                    oDie.VIFail = iKeyNum;

                    if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1 && iKeyNum == 0)
                    {
                        oDie.DiePassFail = 0;
                    }

                    m_arrDies.RemoveAt(iDieIdx);
                    m_arrDies.Insert(iDieIdx, oDie);

                    if (iKeyNum == 0)
                    {
                        if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1)
                        {
                            oDie.DiePassFail = 0;
                            m_arrModifyDies.RemoveAt(iDieModifyIdx);
                            m_DieModifyIndexer.RemoveAt(iDieModifyIdx);
                        }
                    }
                    else if (iDieModifyIdx == -1)
                    {
                        m_arrModifyDies.Add(oDie);
                        m_DieModifyIndexer.Add(new Point(iCurrentX, iCurrentY));
                    }
                    else
                    {
                        m_arrModifyDies.RemoveAt(iDieModifyIdx);
                        m_arrModifyDies.Insert(iDieModifyIdx, oDie);
                    }
                }
                else
                {
                    Die oGoodDie = new Die();

                    for (int i = 0; i < m_arrDies.Count; i++)
                    {
                        oGoodDie = m_arrDies[i];
                        if (oGoodDie.DiePassFail == 0) break;
                    }

                    // EFR이 아닌(Good Bin으로 색상이 지정되지 않은) Bin은 Pass
                    if (m_ColorSet[oGoodDie.BinNumber] == m_ColorSet[oDie.BinNumber]) return;

                    oDie.BinNumber = iKeyNum;
                    oDie.DiePassFail = 0;

                    m_arrDies.RemoveAt(iDieIdx);
                    m_arrDies.Insert(iDieIdx, oDie);
                }

                if (OnTESTChangeDieProperty != null) OnTESTChangeDieProperty(this, oDie);
                if (m_DT != null)
                {
                    DataRow[] drs = m_DT.Select(string.Format("X = {0} AND Y={1}", iCurrentX, iCurrentY));

                    if (!m_bEFR)
                    {
                        if (m_DT.Columns[m_strVIMember] != null)
                        {
                            drs[0][m_strVIMember] = iKeyNum;
                        }
                    }
                    else
                    {
                        drs[0]["BIN"] = iKeyNum;
                    }
                    // 2013/03/18 User Control 사용 시 Update 정보 파악을 위해.
                    m_DT.AcceptChanges();
                }

                this.Validate();

                this.KillDie(oDie, m_VIColorSet[oDie.VIFail], m_colDieBorder);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
		protected void KillDie(Die argDie,Color colDie,Color colBorder)
		{
			Pen pDieBorder = null;
			Pen pOriginDieBorder = null;
			Pen pFirstDieBorder = null;
			SolidBrush sbrshDie = null;

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

				float cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X)/4;
				float cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y)/4;
				float minX = (float)Math.Min(Math.Min(pfPoint[0].X,pfPoint[1].X),Math.Min(pfPoint[2].X,pfPoint[3].X));
				float maxX = (float)Math.Max(Math.Max(pfPoint[0].X,pfPoint[1].X),Math.Max(pfPoint[2].X,pfPoint[3].X));
				float minY = (float)Math.Min(Math.Min(pfPoint[0].Y,pfPoint[1].Y),Math.Min(pfPoint[2].Y,pfPoint[3].Y));
				float maxY = (float)Math.Max(Math.Max(pfPoint[0].Y,pfPoint[1].Y),Math.Max(pfPoint[2].Y,pfPoint[3].Y));
					
				float cirR = Math.Min(Math.Abs(minX - maxX),Math.Abs(minY - maxY));
				// 10% 원을 작게 그린다.
                cirR = cirR - (cirR / 10);

				if(argDie.VIFail == 0)
				{
					pDieBorder = new Pen(m_colDieBorder);
					pOriginDieBorder = new Pen(m_colOriginDieBorder);
					pFirstDieBorder = new Pen(m_colFirstDieBorder);
					sbrshDie	= new SolidBrush(Color.White);
					bool bOnWafer = this.UseDie(argDie.DieCood.X - m_WaferRecipe.ORIGIN_X
						,argDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y
						,this.m_WaferRecipe.DIE_SIZE_X
						,this.m_WaferRecipe.DIE_SIZE_Y);

					/// 0 Skip / 1 Probing / 2 Mark Die를 설정값에 맞춰서 Drawing한다. 
					switch(argDie.DieProp)
					{				
						case 0:
							if(m_bDrawSkipDie)
							{
								sbrshDie.Color = Color.FromArgb(bOnWafer?255:128,m_colSkipDieColor);
							}
							break;
						case 1:
							sbrshDie.Color = m_ColorSet[argDie.BinNumber];
							m_WaferRecipe.NETDIE++;
							break;
						case 2:
							if(m_bDrawMarkDie)
							{
								sbrshDie.Color = Color.FromArgb(bOnWafer?255:128,m_colMarkDieColor);
							}
							break;
					}

					if(argDie.DieProp > -1) m_gdiTempMap.FillPolygon(sbrshDie,pfPoint);
					//////////////////////////////////////////////////////////////////////////////////////////

					m_gdiTempMap.FillPolygon(sbrshDie,pfPoint);
					m_gdiTempMap.DrawPolygon(pDieBorder,pfPoint);
				}
				else
				{
                    m_gdiTempMap.FillEllipse(new SolidBrush(colDie), cirX - cirR / 2, cirY - cirR / 2, cirR, cirR);
					m_gdiTempMap.DrawEllipse(new Pen(m_colDieBorder),cirX - cirR/2,cirY - cirR/2,cirR,cirR);
				}


				m_gdiMain.DrawImageUnscaled(m_bmpWaferMap,0,0);
				m_gdiMain.DrawPolygon(new Pen(Color.Red),pfPoint);

				DrawCenteGrid();
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private void mnuitemMAPMODE_FREEZOOM_Click(object sender, System.EventArgs e)
		{
			m_eoMapMode = MapMode.Free;
			mnuitemMAPMODE_FREEZOOM.Checked = true;
			mnuitemMAPMODE_FITSIZE.Checked = false;

            m_eoMouseDragMode = MouseDragMode.Zoom;
            mnuitemMOUSEDRAGMODE_MOVE.Checked = false;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;
            mnuitemMOUSEDRAGMODE_ZONE.Checked = false;
            MenuManagment();
		}

		private void mnuitemMAPMODE_FITSIZE_Click(object sender, System.EventArgs e)
		{
			m_eoMapMode = MapMode.Fit;
			mnuitemMAPMODE_FREEZOOM.Checked = false;
			mnuitemMAPMODE_FITSIZE.Checked = true;		

            if (mnuitemMOUSEDRAGMODE_ROTATE.Checked == true)
                m_eoMouseDragMode = MouseDragMode.Rotate;
            else
                m_eoMouseDragMode = MouseDragMode.Zoom;
			
			MenuManagment();

            m_iViewAngle = 0;
            OnResize(EventArgs.Empty);
		}

		private void mnuitemMAPMODE_EDIT_Click(object sender, System.EventArgs e)
		{
            // 미사용
			m_eoMapMode = MapMode.Edit;
			mnuitemMAPMODE_FREEZOOM.Checked = false;
			mnuitemMAPMODE_FITSIZE.Checked = false;

			MenuManagment();
		}

        private void mnuitemEditDieMode_SINGLE_SUBTRACTION_Click(object sender, EventArgs e)
        {
            m_eoMapMode = MapMode.Edit;
            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = false;

            m_eoMouseDragMode = MouseDragMode.Zoom;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;

            MenuManagment();
        }

        private void mnuitemEditDieMode_MULTI_SUBTRACTION_Click(object sender, EventArgs e)
        {
            m_eoMapMode = MapMode.Edit;
            mnuitemMAPMODE_FREEZOOM.Checked = false;
            mnuitemMAPMODE_FITSIZE.Checked = false;

            m_eoMouseDragMode = MouseDragMode.Zone;
            mnuitemMOUSEDRAGMODE_ROTATE.Checked = false;

            MenuManagment();
        }

        private void mnuitemMOUSEDRAGMODE_ROTATE_Click(object sender, EventArgs e)
        {
            if (mnuitemMAPMODE_FREEZOOM.Checked == true)
                m_eoMapMode = MapMode.Free;
            else
                m_eoMapMode = MapMode.Fit;

        }

		private new void MenuManagment()
		{
            // Zomm 관련 메뉴 설정
            menuitemMAPMODE_ZOOMIN.Visible = mnuitemMAPMODE_FREEZOOM.Checked;
            menuitemMAPMODE_ZOOMOUT.Visible = mnuitemMAPMODE_FREEZOOM.Checked;
            mnuitemMOUSEDRAGMODE_MOVE.Visible = mnuitemMAPMODE_FREEZOOM.Checked;
            mnuitem_SPRIT2.Visible = mnuitemMAPMODE_FREEZOOM.Checked;            
		}

        #region ▣ 영역 차감시 선택된 Die들 계산하여 차감
        private void CalMultiSubtraction()
        {
            if (m_iKeyNum == 0)
                return;

            PointD pdDieCenter;
            if (m_eoMapSelectStyle == MapSelectStyle.FreeHand) m_SelectPath.CloseFigure();

            try
            {
                foreach (Die InDie in m_arrDies)
                {
                    pdDieCenter.X = (InDie.DieCood.X - m_WaferRecipe.ORIGIN_X) + InDie.DieCood.Width / 2;
                    pdDieCenter.Y = (InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y) + InDie.DieCood.Height / 2;

                    RotatePoint(ref pdDieCenter.X, ref pdDieCenter.Y, m_iViewAngle);

                    PointF pfPoint = PointF.Empty;
                    pfPoint.X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdDieCenter.X) * (m_dZoomRatio * m_dScale));
                    pfPoint.Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdDieCenter.Y) * (m_dZoomRatio * m_dScale));

                    if (m_SelectPath.IsVisible(pfPoint))
                    {
                        if (m_SelectedDies.IndexOf(new Point(InDie.IndexX, InDie.IndexY)) < 0)
                            m_SelectedDies.Add(new Point(InDie.IndexX, InDie.IndexY));
                    }
                }
                
                // 선택된 Die 차감
                Point poKey = Point.Empty;

                for (int i = 0; i < m_SelectedDies.Count; i++)
                {
                    poKey.X = ((Point)m_SelectedDies[i]).X;
                    poKey.Y = ((Point)m_SelectedDies[i]).Y;

                    int iDieIdx = m_DieIndexer.IndexOf(poKey);
                    if (iDieIdx < 0)
                        return;

                    int iDieModifyIdx = m_DieModifyIndexer.IndexOf(poKey);

                    Die oDie = (Die)this.m_arrDies[iDieIdx];
                    if (m_bIqcFqcBinCheck == true && oDie.BinNumber == 2)
                    {
                        //MessageBox.Show("고객사 Fail Bin 정보는 수정할 수 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }
                    else if (m_bIqcFqcBinCheck == false && oDie.DiePassFail != 0)
                    {
                        //MessageBox.Show("Fail Bin 정보는 수정할 수 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    if (oDie.BinNumber == m_iKeyNum && oDie.VIFail == 0)
                        continue;

                    switch (m_eoEditDieMode)
                    {
                        case EditDieMode.SKIP:
                            oDie.DieProp = 0;
                            break;
                        case EditDieMode.ADD:
                            oDie.DieProp = 1;
                            break;
                        case EditDieMode.MARK:
                            oDie.DieProp = 2;
                            break;
                        case EditDieMode.DELETE:
                            oDie.DieProp = -1;
                            break;
                        case EditDieMode.FIRSTMARK:
                            if (OnTESTRedefineFirstDie != null) OnTESTRedefineFirstDie(this, oDie);
                            break;
                        case EditDieMode.KILL:
                            if (!m_bEFR)
                            {
                                //if (oDie.DiePassFail > 0 && iDieModifyIdx < 0)
                                //{
                                //    MessageBox.Show(@"Loss 정보가 이미 존재합니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    continue;
                                //}

                                if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1)
                                {
                                    oDie.DiePassFail = 0;
                                    oDie.VIFail = 0;
                                }

                                m_arrDies.RemoveAt(iDieIdx);
                                m_arrDies.Insert(iDieIdx, oDie);

                                if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1)
                                {
                                    oDie.DiePassFail = 0;
                                    m_arrModifyDies.RemoveAt(iDieModifyIdx);
                                    m_DieModifyIndexer.RemoveAt(iDieModifyIdx);
                                }
                            }

                            if (OnTESTChangeDieProperty != null) OnTESTChangeDieProperty(this, oDie);
                            break;
                        case EditDieMode.UNKILL:
                            oDie.VIFail = 0;
                            this.m_arrDies.RemoveAt(iDieIdx);
                            this.m_arrDies.Insert(iDieIdx, oDie);
                            break;
                        case EditDieMode.DEFECT:
                            //if (oDie.DiePassFail > 0 && iDieModifyIdx < 0)
                            //{
                            //    MessageBox.Show(@"Loss 정보가 이미 존재합니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //    continue;
                            //}

                            if (oDie.VIFail != m_iKeyNum)
                            {
                                ChangeDieBinNumberInfo oDieInfo;

                                oDieInfo.nPreDefectNumber = oDie.VIFail;
                                oDieInfo.nDefectNumber = m_iKeyNum;

                                if (OnChangeDieBinNumber != null) OnChangeDieBinNumber(this, oDieInfo);
                            }

                            oDie.VIFail = m_iKeyNum;

                            if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1 && m_iKeyNum == 0)
                            {
                                oDie.DiePassFail = 0;
                            }

                            m_arrDies.RemoveAt(iDieIdx);
                            m_arrDies.Insert(iDieIdx, oDie);

                            if (m_iKeyNum == 0)
                            {
                                if (m_arrModifyDies.Count > 0 && iDieModifyIdx != -1)
                                {
                                    oDie.DiePassFail = 0;
                                    m_arrModifyDies.RemoveAt(iDieModifyIdx);
                                    m_DieModifyIndexer.RemoveAt(iDieModifyIdx);
                                }
                            }
                            else if (iDieModifyIdx == -1)
                            {
                                m_arrModifyDies.Add(oDie);
                                m_DieModifyIndexer.Add(new Point(m_iCurrentX, m_iCurrentY));
                            }
                            else
                            {
                                m_arrModifyDies.RemoveAt(iDieModifyIdx);
                                m_arrModifyDies.Insert(iDieModifyIdx, oDie);
                            }

                            if (OnTESTChangeDieProperty != null) OnTESTChangeDieProperty(this, oDie);
                            if (m_DT != null)
                            {
                                DataRow[] drs = m_DT.Select(string.Format("X = {0} AND Y={1}", m_iCurrentX, m_iCurrentY));

                                if (!m_bEFR)
                                {
                                    if (m_DT.Columns[m_strVIMember] != null)
                                    {
                                        drs[0][m_strVIMember] = m_iKeyNum;
                                    }
                                }
                                else
                                {
                                    drs[0]["BIN"] = m_iKeyNum;
                                }
                                // 2013/03/18 User Control 사용 시 Update 정보 파악을 위해.
                                m_DT.AcceptChanges();
                            }

                            this.Validate();

                            this.KillDie(oDie, m_VIColorSet[oDie.VIFail], m_colDieBorder);
                            continue;
                    }

                    if (m_eoEditDieMode != EditDieMode.FIRSTMARK)
                    {
                        this.m_arrDies.RemoveAt(iDieIdx);
                        this.m_arrDies.Insert(iDieIdx, oDie);
                    }
                }

                m_SelectedDies.Clear();
                this.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
	}
}

