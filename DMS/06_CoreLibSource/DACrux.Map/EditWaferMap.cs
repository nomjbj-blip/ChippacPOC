using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DACrux.Base;

namespace DACrux.Map
{


	/// <summary>
	/// First Die를 재 정의 했을때 재정의된 Die의 정보를 전달하는 Event의 대리자
	/// </summary>
	public delegate void RedefineFirstDie(object sender, Die NewDie);

	/// <summary>
	/// Die의 정보를 재 정의 했을때 재정의된 Die의 정보를 전달하는 Event의 대리자
	/// </summary>
	public delegate void ChangeDieProperty(object sender, Die NewDie);

	/// <summary>
	/// EditWaferMap의 Die 추가 및 삭제 Mode의 열거자
	/// </summary>
	public enum EditMode { SKIP, ADD, DELETE, MARK, FIRSTMARK, KILL, UNKILL };


	public partial class EditWaferMap : DACrux.Map.WaferMap
	{

		private EditMode m_eoEditMode = EditMode.ADD;
		public event RedefineFirstDie OnRedefineFirstDie;
		public event ChangeDieProperty OnChangeDieProperty;

        //private double m_dMargin = 0.95D;
		private int m_iCurrentFailNumber = 0;
        private bool m_bEditMenuVisible = true;

        private System.Windows.Forms.MenuItem mnuitemMAPMODE_EDIT;
		private System.Windows.Forms.MenuItem mnuitemEDITMODE_SKIP;
		private System.Windows.Forms.MenuItem mnuitemEDITMODE_ADD;
		private System.Windows.Forms.MenuItem mnuitemEDITMODE_DELETE;
		private System.Windows.Forms.MenuItem mnuitemEDITMODE_MARKDIE;
		private System.Windows.Forms.MenuItem mnuitemEDITMODE_MARKFIRST;


		private int[] m_iKeyValue = new int[] { 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49 };

		private bool m_bAutoFocus = false;
		private bool m_bVIMode = false;
        private int m_iChangeBin = -1;

		public EditWaferMap()
		{
			InitializeComponent();
		}

        public bool EditMenuVisible
        {
            set {
                m_bEditMenuVisible = value;
                mnuitemMAPMODE_EDIT.Visible = value;
            }
            get { return m_bEditMenuVisible; }
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
		public EditMode EditMethod
		{
			set
			{
				m_eoEditMode = value;
				mnuitemEDITMODE_SKIP.Checked = false;
				mnuitemEDITMODE_ADD.Checked = false;
				mnuitemEDITMODE_DELETE.Checked = false;
				mnuitemEDITMODE_MARKDIE.Checked = false;
				mnuitemEDITMODE_MARKFIRST.Checked = false;
				switch (m_eoEditMode)
				{
					case EditMode.ADD:
						mnuitemEDITMODE_ADD.Checked = true;
						break;
					case EditMode.DELETE:
						mnuitemEDITMODE_DELETE.Checked = true;
						break;
					case EditMode.FIRSTMARK:
						mnuitemEDITMODE_MARKFIRST.Checked = true;
						break;
					case EditMode.MARK:
						mnuitemEDITMODE_MARKDIE.Checked = true;
						break;
					case EditMode.SKIP:
						mnuitemEDITMODE_SKIP.Checked = true;
						break;
				}
			}
			get { return m_eoEditMode; }
		}

		public int CurrentFailNumber
		{
			set { m_iCurrentFailNumber = value; }
			get { return m_iCurrentFailNumber; }
		}

		public override object DataSource
		{
			set
			{
				if (value == null) return;
				m_DT = (DataTable)value;
				this.DieClear();
                if (m_DT.Columns.IndexOf("VI") > -1)
                {
                    m_strVIMember = "VI"; //Default "VIFAIL" => "VI"
                }

                for (int i = 0; i < m_DT.Rows.Count; i++)
				{
                    Die oNewDie = new Die(DACrux.Base.Convert.intParse(m_DT.Rows[i]["X"].ToString())
                        , DACrux.Base.Convert.intParse(m_DT.Rows[i]["Y"].ToString()), 0, 1);

                    //Die oNewDie = new Die(DACrux.Base.Convert.intParse(m_DT.Rows[i]["X"].ToString())
                    //    , DACrux.Base.Convert.intParse(m_DT.Rows[i]["Y"].ToString()), 0, DACrux.Base.Convert.intParse(m_DT.Rows[i]["USECODE"].ToString()));

                    //int.TryParse(m_DT.Rows[i][m_strVIMember].ToString(), out oNewDie.VIFail);
                    //int.TryParse(m_DT.Rows[i][m_strVIMember].ToString(), out oNewDie.DiePassFail);

                    if (m_DT.Columns.IndexOf("BIN") > -1) int.TryParse(m_DT.Rows[i]["BIN"].ToString(), out oNewDie.BinNumber);
                    if (m_DT.Columns.IndexOf("AVI") > -1) int.TryParse(m_DT.Rows[i]["AVI"].ToString(), out oNewDie.AVIFailNumber);

					this.AddDie(oNewDie);

				}
			}
			get
			{
				return m_DT;
			}
		}


		public void DieMinMaxCalculation()
		{
			double dWaferR = m_WaferRecipe.WAFER_SIZE / 2.0d;

			this.m_WaferRecipe.DIE_INDEX_MIN_X = (int)((-dWaferR + this.m_WaferRecipe.ORIGIN_X) / this.m_WaferRecipe.DIE_SIZE_X) + this.m_WaferRecipe.ORIGIN_DIE_X - 1;
			this.m_WaferRecipe.DIE_INDEX_MIN_Y = (int)((-dWaferR + this.m_WaferRecipe.ORIGIN_Y) / this.m_WaferRecipe.DIE_SIZE_Y) + this.m_WaferRecipe.ORIGIN_DIE_Y;
			this.m_WaferRecipe.DIE_INDEX_MAX_X = (int)((dWaferR + this.m_WaferRecipe.ORIGIN_X) / this.m_WaferRecipe.DIE_SIZE_X) + this.m_WaferRecipe.ORIGIN_DIE_X;
			this.m_WaferRecipe.DIE_INDEX_MAX_Y = (int)((dWaferR + this.m_WaferRecipe.ORIGIN_Y) / this.m_WaferRecipe.DIE_SIZE_Y) + this.m_WaferRecipe.ORIGIN_DIE_Y + 1;
		}

		public void SetVIValue(int Index, int Val)
		{
			m_iKeyValue[Index] = Val;
		}

		public int GetVIValue(int Index)
		{
			return m_iKeyValue[Index];
		}

        public void SetChangeBin(int iBin)
        {
            m_iChangeBin = iBin;
        }

		private void AutoDevDef()
		{
			int iDieX = 0;
			int iDieY = 0;

			double dX = 0.0d;
			double dY = 0.0d;

			int iBin = 0;
			m_arrDies.Clear();
			m_DieIndexer.Clear();


			this.m_WaferRecipe.NETDIE = 0;
			this.m_WaferRecipe.DIE_SIZE_X = (this.m_WaferRecipe.WAFER_SIZE * m_dMargin) / (this.m_WaferRecipe.DIE_INDEX_MAX_X - this.m_WaferRecipe.DIE_INDEX_MIN_X + 1);
			this.m_WaferRecipe.DIE_SIZE_Y = (this.m_WaferRecipe.WAFER_SIZE * m_dMargin) / (this.m_WaferRecipe.DIE_INDEX_MAX_Y - this.m_WaferRecipe.DIE_INDEX_MIN_Y + 1);

			if ((this.m_WaferRecipe.XDIES % 2) == 0)
			{
				this.m_WaferRecipe.ORIGIN_X = 0;
			}
			else
			{
				this.m_WaferRecipe.ORIGIN_X = this.m_WaferRecipe.DIE_SIZE_X - this.m_WaferRecipe.DIE_SIZE_X / 2.0d;
			}

			if ((this.m_WaferRecipe.YDIES % 2) == 0)
			{
				this.m_WaferRecipe.ORIGIN_Y = 0;
			}
			else
			{
				this.m_WaferRecipe.ORIGIN_Y = -this.m_WaferRecipe.DIE_SIZE_Y / 2.0d;
			}


			this.m_WaferRecipe.ORIGIN_DIE_X = this.m_WaferRecipe.DIE_INDEX_MIN_X + (int)Math.Floor((double)m_WaferRecipe.XDIES / 2.0d);
			this.m_WaferRecipe.ORIGIN_DIE_Y = this.m_WaferRecipe.DIE_INDEX_MIN_Y + (int)Math.Floor((double)m_WaferRecipe.YDIES / 2.0d);


			for (int idx = m_WaferRecipe.DIE_INDEX_MIN_X; idx <= m_WaferRecipe.DIE_INDEX_MAX_X; idx++)
			{
				for (int idy = m_WaferRecipe.DIE_INDEX_MIN_Y; idy <= m_WaferRecipe.DIE_INDEX_MAX_Y; idy++)
				{
					switch (m_WaferRecipe.XYDIR)
					{
						case XYDirection.LeftTop:
							iDieX = idx;
							iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) - ((this.m_WaferRecipe.YDIES + 1) % 2);

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
							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X - 1) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) * this.m_WaferRecipe.DIE_SIZE_Y;
							break;
						case XYDirection.RightTop:
							iDieX = this.m_WaferRecipe.ORIGIN_DIE_X - (idx - this.m_WaferRecipe.ORIGIN_DIE_X);
							iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y - (idy - this.m_WaferRecipe.ORIGIN_DIE_Y) - ((this.m_WaferRecipe.YDIES + 1) % 2);
							dX = (idx - this.m_WaferRecipe.ORIGIN_DIE_X - 1) * this.m_WaferRecipe.DIE_SIZE_X;
							dY = (idy - this.m_WaferRecipe.ORIGIN_DIE_Y - ((this.m_WaferRecipe.YDIES) % 2)) * this.m_WaferRecipe.DIE_SIZE_Y;
							break;
					}

					if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, this.m_WaferRecipe.DIE_SIZE_X, this.m_WaferRecipe.DIE_SIZE_Y))
					{
						m_arrDies.Add(new Die(iDieX, iDieY, iBin, dX, dY, this.m_WaferRecipe.DIE_SIZE_X, this.m_WaferRecipe.DIE_SIZE_Y));
						this.m_WaferRecipe.NETDIE++;
					}
					else
					{
						m_arrDies.Add(new Die(iDieX, iDieY, iBin, dX, dY, this.m_WaferRecipe.DIE_SIZE_X, this.m_WaferRecipe.DIE_SIZE_Y, -1));
					}
					m_DieIndexer.Add(new Point(iDieX, iDieY));
				}
			}
		}


		private void ReUseDie()
		{
			Die[] tmpDie = new Die[m_arrDies.Count];
			m_arrDies.CopyTo(tmpDie);
			m_arrDies.Clear();

			double dX = 0.0d;
			double dY = 0.0d;

			for (int i = 0; i < tmpDie.Length; i++)
			{
				dX = tmpDie[i].IndexX * this.m_WaferRecipe.DIE_SIZE_X;
				dY = tmpDie[i].IndexY * this.m_WaferRecipe.DIE_SIZE_Y;

				if (tmpDie[i].DieProp != 2)
				{
					if (UseDie(dX - m_WaferRecipe.ORIGIN_X, dY - m_WaferRecipe.ORIGIN_Y, this.m_WaferRecipe.DIE_SIZE_X, this.m_WaferRecipe.DIE_SIZE_Y))
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
			if (iDieIdx < 0) return;
			Die oDie;
			try
			{
				poKey.X = m_iCurrentX;
				poKey.Y = m_iCurrentY;


				iDieIdx = m_DieIndexer.IndexOf(poKey);
                if (e.Button == System.Windows.Forms.MouseButtons.Left && m_eoMouseDragMode == MouseDragMode.Normal)
				{
					oDie = (Die)this.m_arrDies[iDieIdx];
                    int iOldDieProperty = oDie.DieProp;
					switch (m_eoEditMode)
					{
						case EditMode.SKIP:
							oDie.DieProp = 0;
							break;
						case EditMode.ADD:
							oDie.DieProp = 1;
							break;
						case EditMode.MARK:
							oDie.DieProp = 2;
							break;
						case EditMode.DELETE:
							oDie.DieProp = -1;
							break;
						case EditMode.FIRSTMARK:
                            m_WaferRecipe.FIRST_DIE_X = oDie.IndexX;
                            m_WaferRecipe.FIRST_DIE_Y = oDie.IndexY;
							if (OnRedefineFirstDie != null) OnRedefineFirstDie(this, oDie);
							break;
						case EditMode.KILL:
							oDie.VIFail = m_iCurrentFailNumber;
							this.m_arrDies.RemoveAt(iDieIdx);
							this.m_arrDies.Insert(iDieIdx, oDie);
							break;
						case EditMode.UNKILL:
							oDie.VIFail = 0;
							this.m_arrDies.RemoveAt(iDieIdx);
							this.m_arrDies.Insert(iDieIdx, oDie);
							break;
					}

					if (m_eoEditMode != EditMode.FIRSTMARK)
					{
						this.m_arrDies.RemoveAt(iDieIdx);
						this.m_arrDies.Insert(iDieIdx, oDie);
					}
                    this.Redraw();
                    if (iOldDieProperty != oDie.DieProp && OnChangeDieProperty != null)
                    {
                        OnChangeDieProperty(this, oDie);
                    }
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
			base.OnMouseMove(e);
            Die oDie;
            if (m_iCurrentDieIndex > -1)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && m_eoMouseDragMode == MouseDragMode.Normal)
                {
                    if (m_iCurrentDieIndex < 0) return;
                    oDie = (Die)this.m_arrDies[m_iCurrentDieIndex];
                    switch (m_eoEditMode)
                    {
                        case EditMode.DELETE:
                            oDie.DieProp = -1;
                            break;
                        case EditMode.SKIP:
                            oDie.DieProp = 0;
                            break;
                        case EditMode.ADD:
                            oDie.DieProp = 1;
                            break;
                        case EditMode.MARK:
                            oDie.DieProp = 2;
                            break;
                        default:
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
            }
			if (m_bAutoFocus) this.Focus();
		}

		protected override void WaferMap_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			base.WaferMap_Paint(null, null);
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
            int iDieIdx = -1;
            int icurrX = m_iCurrentX;
            int icurrY = m_iCurrentY;
			switch (e.KeyCode)
			{
				case Keys.Up:
                    icurrY = m_iCurrentY - 1;
					break;
				case Keys.Down:
                    icurrY = m_iCurrentY + 1;
					break;
				case Keys.Left:
                    icurrX = m_iCurrentX - 1;
					break;
				case Keys.Right:
                    icurrX = m_iCurrentX + 1;
					break;
			}
            iDieIdx = m_DieIndexer.IndexOf(new Point(icurrX, icurrY));
            if (iDieIdx > -1)
            {
                m_iCurrentX = icurrX;
                m_iCurrentY = icurrY;
                SetFocusDie(m_iCurrentX, m_iCurrentY);
            }
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (!m_bVIMode) return;
			int iDieIdx = -1;
            int iFailNum = -1;
            int iKeyNum = -1;
			//base.OnKeyDown(e);

			switch (e.KeyCode)
			{
				case Keys.D1:
                case Keys.NumPad1:
					iKeyNum = 1; //m_iKeyValue[0];
					break;
				case Keys.D2:
                case Keys.NumPad2:
					iKeyNum = 2; //m_iKeyValue[1];
					break;
				case Keys.D3:
                case Keys.NumPad3:
					iKeyNum = 3; //m_iKeyValue[2];
					break;
				case Keys.D4:
                case Keys.NumPad4:
					iKeyNum = 4; //m_iKeyValue[3];
					break;
				case Keys.D5:
                case Keys.NumPad5:
					iKeyNum = 5; //m_iKeyValue[4];
					break;
				case Keys.D6:
                case Keys.NumPad6:
					iKeyNum = 6; //m_iKeyValue[5];
					break;
				case Keys.D7:
                case Keys.NumPad7:
					iKeyNum = 7; //m_iKeyValue[6];
					break;
				case Keys.D8:
                case Keys.NumPad8:
					iKeyNum = 8; //m_iKeyValue[7];
					break;
				case Keys.D9:
                case Keys.NumPad9:
					iKeyNum = 9; //m_iKeyValue[8];
					break;
				case Keys.D0:
                case Keys.NumPad0:
					iKeyNum = 10; //m_iKeyValue[9];
					break;
				case Keys.F1:
					iKeyNum = 11; //m_iKeyValue[10];
					break;
				case Keys.F2:
					iKeyNum = 12; //m_iKeyValue[11];
					break;
				case Keys.F3:
					iKeyNum = 13; //m_iKeyValue[12];
					break;
				case Keys.F4:
					iKeyNum = 14; //m_iKeyValue[13];
					break;
				case Keys.F5:
					iKeyNum = 15; //m_iKeyValue[14];
					break;
				case Keys.F6:
					iKeyNum = 16; //m_iKeyValue[15];
					break;
				case Keys.F7:
					iKeyNum = 17; //m_iKeyValue[16];
					break;
				case Keys.F8:
					iKeyNum = 18; //m_iKeyValue[17];
					break;
				case Keys.F9:
					iKeyNum = 19; //m_iKeyValue[18];
					break;
				case Keys.F10:
					iKeyNum = 20; //m_iKeyValue[19];
					break;
				case Keys.Delete:
					iKeyNum = 999;
					break;
			}

			if (iKeyNum < 0)
			{
				return;
            }
            else if (iKeyNum == 999)
            {
                iFailNum = 0;
            }
            else
            {
                iFailNum = m_iKeyValue[iKeyNum];
            }

            if (iFailNum < 0) return;

            // 1. Current Die가 없으면 
			iDieIdx = m_DieIndexer.IndexOf(new Point(m_iCurrentX, m_iCurrentY));
			if (iDieIdx < 0) return;
			Die oDie = m_arrDies[iDieIdx];


            oDie.VIFail = iFailNum;
			m_arrDies.RemoveAt(iDieIdx);
			m_arrDies.Insert(iDieIdx, oDie);

			if (m_DT != null)
			{
				DataRow[] drs = m_DT.Select(string.Format("X = {0} AND Y={1}", m_iCurrentX, m_iCurrentY));
				if (m_DT.Columns[m_strVIMember] != null)
				{
                    drs[0][m_strVIMember] = iFailNum;
				}
				m_DT.AcceptChanges();
                if (OnChangeDieProperty != null) OnChangeDieProperty(this, oDie);
            }
            this.KillDie(oDie, m_VIColorSet[oDie.VIFail], m_colDieBorder);
		}


		protected void KillDie(Die argDie, Color colDie, Color colBorder)
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

				RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
				RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
				RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
				RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

				PointF[] pfPoint = new PointF[4];
				pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
				pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

				pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
				pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

				pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
				pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

				pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
				pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));

				float cirX = (pfPoint[0].X + pfPoint[1].X + pfPoint[2].X + pfPoint[3].X) / 4;
				float cirY = (pfPoint[0].Y + pfPoint[1].Y + pfPoint[2].Y + pfPoint[3].Y) / 4;
				float minX = (float)Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
				float maxX = (float)Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X));
				float minY = (float)Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
				float maxY = (float)Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y));

				float cirR = Math.Min(Math.Abs(minX - maxX), Math.Abs(minY - maxY));

				if (argDie.VIFail == 0)
				{
					pDieBorder = new Pen(m_colDieBorder);
					pOriginDieBorder = new Pen(m_colOriginDieBorder);
					pFirstDieBorder = new Pen(m_colFirstDieBorder);
					sbrshDie = new SolidBrush(Color.White);
					bool bOnWafer = this.UseDie(argDie.DieCood.X - m_WaferRecipe.ORIGIN_X
						, argDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y
						, this.m_WaferRecipe.DIE_SIZE_X
						, this.m_WaferRecipe.DIE_SIZE_Y);

					/// 0 Skip / 1 Probing / 2 Mark Die를 설정값에 맞춰서 Drawing한다. 
					switch (argDie.DieProp)
					{
						case 0:
							if (m_bDrawSkipDie)
							{
								sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colSkipDieColor);
							}
							break;
						case 1:
							sbrshDie.Color = m_ColorSet[argDie.BinNumber];
							m_WaferRecipe.NETDIE++;
							break;
						case 2:
							if (m_bDrawMarkDie)
							{
								sbrshDie.Color = Color.FromArgb(bOnWafer ? 255 : 128, m_colMarkDieColor);
							}
							break;
					}

					if (argDie.DieProp > -1) m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
					//////////////////////////////////////////////////////////////////////////////////////////

					m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
					m_gdiTempMap.DrawPolygon(pDieBorder, pfPoint);
				}
				else
				{
					m_gdiTempMap.FillEllipse(new SolidBrush(m_ColorSet[argDie.VIFail]), cirX - cirR / 2, cirY - cirR / 2, cirR, cirR);
					m_gdiTempMap.DrawEllipse(new Pen(m_colDieBorder), cirX - cirR / 2, cirY - cirR / 2, cirR, cirR);
				}


				m_gdiMain.DrawImageUnscaled(m_bmpWaferMap, 0, 0);
				m_gdiMain.DrawPolygon(new Pen(Color.Red), pfPoint);

				DrawCenteGrid();

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        private void mnuitemEDITMODE_ADD_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Normal;
            this.m_eoEditMode = EditMode.ADD;

            mnuitemEDITMODE_SKIP.Checked = false;
            mnuitemEDITMODE_ADD.Checked = true;
            mnuitemEDITMODE_DELETE.Checked = false;
            mnuitemEDITMODE_MARKDIE.Checked = false;
            mnuitemEDITMODE_MARKFIRST.Checked = false;
        }

        private void mnuitemEDITMODE_DELETE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Normal;
            this.m_eoEditMode = EditMode.DELETE;

            mnuitemEDITMODE_SKIP.Checked = false;
            mnuitemEDITMODE_ADD.Checked = false;
            mnuitemEDITMODE_DELETE.Checked = true;
            mnuitemEDITMODE_MARKDIE.Checked = false;
            mnuitemEDITMODE_MARKFIRST.Checked = false;
        }

        private void mnuitemEDITMODE_MARKDIE_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Normal;
            this.m_eoEditMode = EditMode.MARK;

            mnuitemEDITMODE_SKIP.Checked = false;
            mnuitemEDITMODE_ADD.Checked = false;
            mnuitemEDITMODE_DELETE.Checked = false;
            mnuitemEDITMODE_MARKDIE.Checked = true;
            mnuitemEDITMODE_MARKFIRST.Checked = false;
        }

        private void mnuitemEDITMODE_MARKFIRST_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Normal;
            this.m_eoEditMode = EditMode.FIRSTMARK;

            mnuitemEDITMODE_SKIP.Checked = false;
            mnuitemEDITMODE_ADD.Checked = false;
            mnuitemEDITMODE_DELETE.Checked = false;
            mnuitemEDITMODE_MARKDIE.Checked = false;
            mnuitemEDITMODE_MARKFIRST.Checked = true;
        }

        private void mnuitemEDITMODE_SKIP_Click(object sender, System.EventArgs e)
        {
            m_eoMouseDragMode = MouseDragMode.Normal;
            this.m_eoEditMode = EditMode.SKIP;

            mnuitemEDITMODE_SKIP.Checked = true;
            mnuitemEDITMODE_ADD.Checked = false;
            mnuitemEDITMODE_DELETE.Checked = false;
            mnuitemEDITMODE_MARKDIE.Checked = false;
            mnuitemEDITMODE_MARKFIRST.Checked = false;
        }

	}
}
