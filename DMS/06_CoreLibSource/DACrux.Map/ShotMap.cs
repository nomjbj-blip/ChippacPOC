using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;
using DACrux.Base;

namespace DACrux.Map
{
	public partial class ShotMap : DACrux.Map.WaferMap
	{
		private Shot m_oShot;
		private Rectangle m_rectShotSample = Rectangle.Empty;
		private ArrayList m_arrShot = null;
		private ArrayList m_arrDieIndex = null;

		public ShotMap()
		{
			InitializeComponent();
			m_arrShot = new ArrayList();
			m_arrDieIndex = new ArrayList();
			m_eoMouseDragMode = MouseDragMode.Shot;
			m_eoMapSelectStyle = MapSelectStyle.Rectangle;
			ShotClear();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.
		}


		public void ShotClear()
		{
			m_oShot.MinX = 0;
			m_oShot.MinY = 0;
			m_oShot.MaxX = 0;
			m_oShot.MaxY = 0;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				m_poStart.X = e.X;
				m_poStart.Y = e.Y;

				m_poEnd.X = e.X;
				m_poEnd.Y = e.Y;

				m_rectSelect.Width = Math.Max(m_poStart.X, m_poEnd.X) - Math.Min(m_poStart.X, m_poEnd.X);
				m_rectSelect.Height = Math.Max(m_poStart.Y, m_poEnd.Y) - Math.Min(m_poStart.Y, m_poEnd.Y);

			}
			else if (e.Button == MouseButtons.Right)
			{
				//ctxmWaferMap.Show(this,new Point(e.X,e.Y));
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			//base.OnMouseMove (e);

			if (e.Button == MouseButtons.Left)
			{
				m_poEnd.X = e.X;
				m_poEnd.Y = e.Y;

				m_rectSelect.X = Math.Min(m_poStart.X, m_poEnd.X);
				m_rectSelect.Y = Math.Min(m_poStart.Y, m_poEnd.Y);
				m_rectSelect.Width = Math.Max(m_poStart.X, m_poEnd.X) - Math.Min(m_poStart.X, m_poEnd.X);
				m_rectSelect.Height = Math.Max(m_poStart.Y, m_poEnd.Y) - Math.Min(m_poStart.Y, m_poEnd.Y);

				switch (m_eoMouseDragMode)
				{
					case MouseDragMode.Shot:
						DrawSelectedDie();
						break;
				}
			}
		}


		#region ◈ Die선택시 선택된 Die들을 찾아 다시그림
		private void DrawSelectedDie()
		{
			GraphicsPath gpathSelDie = new GraphicsPath();
			gpathSelDie.StartFigure();
			m_rectShotSample = Rectangle.Empty;
			RectangleF rectfTemp = RectangleF.Empty;
			PointD[] pdPoint = new PointD[4];
			PointF[] pfPoint = new PointF[4];
			Pen penPath = new Pen(m_hbSelectBrush, 3);
			Font fntSelect = new Font("굴림", 9, FontStyle.Bold);
			if (m_eoMapSelectStyle == MapSelectStyle.FreeHand) m_SelectPath.CloseFigure();
			try
			{
				Redraw();
				foreach (Die InDie in m_arrDies)
				{
					pdPoint[0].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
					pdPoint[0].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

					pdPoint[1].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
					pdPoint[1].Y = InDie.DieCood.Y - m_WaferRecipe.ORIGIN_Y;

					pdPoint[2].X = InDie.DieCood.X + InDie.DieCood.Width - m_WaferRecipe.ORIGIN_X;
					pdPoint[2].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

					pdPoint[3].X = InDie.DieCood.X - m_WaferRecipe.ORIGIN_X;
					pdPoint[3].Y = InDie.DieCood.Y + InDie.DieCood.Height - m_WaferRecipe.ORIGIN_Y;

					RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
					RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
					RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
					RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

					pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
					pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

					pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
					pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

					pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
					pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

					pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
					pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));

					rectfTemp.X = Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X));
					rectfTemp.Y = Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y));
					rectfTemp.Width = Math.Max(Math.Max(pfPoint[0].X, pfPoint[1].X), Math.Max(pfPoint[2].X, pfPoint[3].X)) - rectfTemp.X;
					rectfTemp.Height = Math.Max(Math.Max(pfPoint[0].Y, pfPoint[1].Y), Math.Max(pfPoint[2].Y, pfPoint[3].Y)) - rectfTemp.Y;

					if (m_rectSelect.IntersectsWith(Rectangle.Round(rectfTemp)))
					{
						if (m_rectShotSample != Rectangle.Empty)
						{
							m_rectShotSample = Rectangle.Union(m_rectShotSample, Rectangle.Round(rectfTemp));
							m_oShot.MinX = Math.Min(m_oShot.MinX, InDie.IndexX);
							m_oShot.MinY = Math.Min(m_oShot.MinY, InDie.IndexY);
							m_oShot.MaxX = Math.Max(m_oShot.MaxX, InDie.IndexX);
							m_oShot.MaxY = Math.Max(m_oShot.MaxY, InDie.IndexY);
						}
						else
						{
							m_rectShotSample = Rectangle.Round(rectfTemp);
							m_oShot.MinX = InDie.IndexX;
							m_oShot.MinY = InDie.IndexY;
							m_oShot.MaxX = InDie.IndexX;
							m_oShot.MaxY = InDie.IndexY;
						}
					}
				}

				gpathSelDie.AddRectangle(m_rectShotSample);
				gpathSelDie.CloseFigure();

				m_gdiMain.DrawPath(penPath, gpathSelDie);
				m_gdiMain.DrawString(string.Format("{0}:[{1} ~ {2}]", m_oShot.CountX, m_oShot.MinX, m_oShot.MaxX)
					, fntSelect, new SolidBrush(Color.White), m_rectShotSample.X, m_rectShotSample.Y - 13);

				m_gdiMain.DrawString(string.Format("{0}:[{1} ~ {2}]", m_oShot.CountY, m_oShot.MinY, m_oShot.MaxY)
					, fntSelect, new SolidBrush(Color.White), m_rectShotSample.X - 17, m_rectShotSample.Y, new StringFormat(StringFormatFlags.DirectionVertical));

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				gpathSelDie.Dispose();
				penPath.Dispose();
				fntSelect.Dispose();
			}
		}
		#endregion

		private void ShotMap_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ShotCalculate();
			DrawShot();
		}


		private void ShotCalculate()
		{
			int iDieX = 0;
			int iDieY = 0;

			int shIdxX = 0;
			int shIdxY = 0;

			int iDivX = 0;
			int iDivY = 0;

			string strDieIdx = string.Empty;

			RectangleF rectfShot = RectangleF.Empty;
			RectangleF rectfDie = RectangleF.Empty;
			m_arrShot.Clear();
			m_arrDieIndex.Clear();
			ArrayList arrShotIndexer = new ArrayList();
			int shGapX = (int)(m_oShot.MinX % m_oShot.CountX);
			int shGapY = (int)(m_oShot.MinY % m_oShot.CountY);

			foreach (Die InDie in m_arrDies)
			{
				shIdxX = (int)Math.DivRem((InDie.IndexX - shGapX), m_oShot.CountX, out iDivX);
				shIdxY = (int)Math.DivRem((InDie.IndexY - shGapY), m_oShot.CountY, out iDivY);

				if (iDivX != 0)
				{
					shIdxX = shIdxX + (((InDie.IndexX - shGapX) < 0) ? -1 : 0);
				}
				if (iDivY != 0)
				{
					shIdxY = shIdxY + (((InDie.IndexY - shGapY) < 0) ? -1 : 0);
				}

				if (arrShotIndexer.IndexOf(new Point(shIdxX, shIdxY)) > -1) continue;
				arrShotIndexer.Add(new Point(shIdxX, shIdxY));

				Shot oShot = new Shot();

				oShot.IndexX = shIdxX;
				oShot.IndexY = shIdxY;
				oShot.ShotIndex = arrShotIndexer.Count - 1;

				oShot.MinX = shIdxX * m_oShot.CountX + shGapX;
				oShot.MinY = shIdxY * m_oShot.CountY + shGapY;

				oShot.MaxX = oShot.MinX + m_oShot.CountX - 1;
				oShot.MaxY = oShot.MinY + m_oShot.CountY - 1;

				rectfShot = RectangleF.Empty;
				rectfDie = RectangleF.Empty;
				strDieIdx = string.Format("{0}!{1}#", oShot.IndexX, oShot.IndexY);

				for (int ix = oShot.MinX; ix <= oShot.MaxX; ix++)
				{
					for (int iy = oShot.MinY; iy <= oShot.MaxY; iy++)
					{
						if (m_DieIndexer.IndexOf(new Point(ix, iy)) > -1)
						{
							strDieIdx = string.Format("{0},{1}!{2}", strDieIdx, ix, iy);
						}

                        switch (m_WaferRecipe.XYDIR)
                        {
                            case XYDirection.LeftTop:
                                iDieX = ix - 1;
                                iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y + (this.m_WaferRecipe.ORIGIN_DIE_Y - iy) + ((this.m_WaferRecipe.YDIES + 1) % 2);
                                break;
                            case XYDirection.LeftBottom:
                                iDieX = ix - 1;
                                iDieY = iy - 1;
                                break;
                            case XYDirection.RightBottom:
                                iDieX = this.m_WaferRecipe.ORIGIN_DIE_X + (this.m_WaferRecipe.ORIGIN_DIE_X - ix);
                                iDieY = iy;
                                break;
                            case XYDirection.RightTop:
                                iDieX = this.m_WaferRecipe.ORIGIN_DIE_X + (this.m_WaferRecipe.ORIGIN_DIE_X - ix);
                                iDieY = this.m_WaferRecipe.ORIGIN_DIE_Y + (this.m_WaferRecipe.ORIGIN_DIE_Y - iy) + ((this.m_WaferRecipe.YDIES + 1) % 2);
                                break;
                        }

						rectfDie.X = (float)((iDieX - m_WaferRecipe.ORIGIN_DIE_X) * m_WaferRecipe.DIE_SIZE_X);
						rectfDie.Y = (float)((iDieY - m_WaferRecipe.ORIGIN_DIE_Y) * m_WaferRecipe.DIE_SIZE_Y);
						rectfDie.Width = (float)m_WaferRecipe.DIE_SIZE_X;
						rectfDie.Height = (float)m_WaferRecipe.DIE_SIZE_Y;

						if (rectfShot != RectangleF.Empty)
						{
							rectfShot = RectangleF.Union(rectfShot, rectfDie);
						}
						else
						{
							rectfShot = rectfDie;
						}

					}
				}
				//strDieIdx = strDieIdx.Remove(0,1);

				m_arrDieIndex.Add(strDieIdx);

				oShot.ShotCood.X = (double)rectfShot.X;
				oShot.ShotCood.Y = (double)rectfShot.Y;
				oShot.ShotCood.Width = (double)rectfShot.Width;
				oShot.ShotCood.Height = (double)rectfShot.Height;

				m_arrShot.Add(oShot);
			}
		}

		private void DrawShot()
		{
			if (DesignMode) return;
			GraphicsPath gpathShot = null;
			PointD[] pdPoint = new PointD[4];
			PointF[] pfPoint = new PointF[4];

			/// Pen / Brush는 Dispose해야 하므로 Try밖에 선언하고 Finally에서 Dispose를 ...
			/// 메모리 확인해 본결과 효과 만점...
			Pen pShotBorder = new Pen(new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.Yellow, Color.PowderBlue), 3);
			Font fntShot = new Font("굴림", 9, FontStyle.Bold);
			try
			{
				gpathShot = new GraphicsPath();
				gpathShot.StartFigure();
				foreach (Shot InShot in m_arrShot)
				{
					pdPoint[0].X = InShot.ShotCood.X - m_WaferRecipe.ORIGIN_X;
					pdPoint[0].Y = InShot.ShotCood.Y - m_WaferRecipe.ORIGIN_Y;

					pdPoint[1].X = InShot.ShotCood.X + InShot.ShotCood.Width - m_WaferRecipe.ORIGIN_X;
					pdPoint[1].Y = InShot.ShotCood.Y - m_WaferRecipe.ORIGIN_Y;

					pdPoint[2].X = InShot.ShotCood.X + InShot.ShotCood.Width - m_WaferRecipe.ORIGIN_X;
					pdPoint[2].Y = InShot.ShotCood.Y + InShot.ShotCood.Height - m_WaferRecipe.ORIGIN_Y;

					pdPoint[3].X = InShot.ShotCood.X - m_WaferRecipe.ORIGIN_X;
					pdPoint[3].Y = InShot.ShotCood.Y + InShot.ShotCood.Height - m_WaferRecipe.ORIGIN_Y;

					RotatePoint(ref pdPoint[0].X, ref pdPoint[0].Y, m_iViewAngle);
					RotatePoint(ref pdPoint[1].X, ref pdPoint[1].Y, m_iViewAngle);
					RotatePoint(ref pdPoint[2].X, ref pdPoint[2].Y, m_iViewAngle);
					RotatePoint(ref pdPoint[3].X, ref pdPoint[3].Y, m_iViewAngle);

					pfPoint[0].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[0].X) * (m_dZoomRatio * m_dScale));
					pfPoint[0].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[0].Y) * (m_dZoomRatio * m_dScale));

					pfPoint[1].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[1].X) * (m_dZoomRatio * m_dScale));
					pfPoint[1].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[1].Y) * (m_dZoomRatio * m_dScale));

					pfPoint[2].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[2].X) * (m_dZoomRatio * m_dScale));
					pfPoint[2].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[2].Y) * (m_dZoomRatio * m_dScale));

					pfPoint[3].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2 + pdPoint[3].X) * (m_dZoomRatio * m_dScale));
					pfPoint[3].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2 - pdPoint[3].Y) * (m_dZoomRatio * m_dScale));

					gpathShot.AddPolygon(pfPoint);
					gpathShot.CloseFigure();

					m_gdiMain.DrawString(string.Format("[{0},{1}]", InShot.IndexX, InShot.IndexY)
						, fntShot, new SolidBrush(Color.LightSlateGray)
						, Math.Min(Math.Min(pfPoint[0].X, pfPoint[1].X), Math.Min(pfPoint[2].X, pfPoint[3].X)) + 10
						, Math.Min(Math.Min(pfPoint[0].Y, pfPoint[1].Y), Math.Min(pfPoint[2].Y, pfPoint[3].Y)) + 10);

				}
				m_gdiMain.DrawPath(pShotBorder, gpathShot);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				pdPoint = null;
				pfPoint = null;
				pShotBorder.Dispose();
				fntShot.Dispose();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DrawShot();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (m_gdiMain != null) DrawShot();
		}

		public string[] GetShotDetail()
		{
			string[] strShotDetail = new string[m_arrDieIndex.Count];
			m_arrDieIndex.CopyTo(strShotDetail);
			return strShotDetail;
		}

		public void SetShotMap(int minx, int miny, int maxx, int maxy)
		{
			m_oShot.MinX = minx;
			m_oShot.MinY = miny;
			m_oShot.MaxX = maxx;
			m_oShot.MaxY = maxy;

			ShotCalculate();
			DrawShot();
		}

		public DataSet GetDataSource()
		{
			string[] strShotDetail = GetShotDetail();
			string[] strHeader = null;
			string[] strBody = null;
			string[] strDie = null;

			int iShotX = 0;
			int iShotY = 0;

			int iDieX = 0;
			int iDieY = 0;

			int iDieShotX = 0;
			int iDieShotY = 0;

			int shGapX = (int)(m_oShot.MinX % m_oShot.CountX);
			int shGapY = (int)(m_oShot.MinY % m_oShot.CountY);

			DataSet ds = null;
			try
			{
				ds = new DataSet();
				ds.Tables.Add();
				ds.Tables[0].Columns.Add(new DataColumn("X", System.Type.GetType("System.Int32")));
				ds.Tables[0].Columns.Add(new DataColumn("Y", System.Type.GetType("System.Int32")));
				ds.Tables[0].Columns.Add(new DataColumn("SX", System.Type.GetType("System.Int32")));
				ds.Tables[0].Columns.Add(new DataColumn("SY", System.Type.GetType("System.Int32")));
				ds.Tables[0].Columns.Add(new DataColumn("DIESX", System.Type.GetType("System.Int32")));
				ds.Tables[0].Columns.Add(new DataColumn("DIESY", System.Type.GetType("System.Int32")));
				ds.Tables[0].Columns.Add(new DataColumn("SHOTID", System.Type.GetType("System.Int32")));

				for (int i = 0; i < strShotDetail.Length; i++)
				{
					strHeader = strShotDetail[i].Split('#');
					strBody = strHeader[0].Split('!');
					iShotX = DACrux.Base.Convert.intParse(strBody[0]);
					iShotY = DACrux.Base.Convert.intParse(strBody[1]);

					strBody = strHeader[1].Remove(0, 1).Split(',');
					for (int iDies = 0; iDies < strBody.Length; iDies++)
					{
						strDie = strBody[iDies].Split('!');
						iDieX = DACrux.Base.Convert.intParse(strDie[0]);
						iDieY = DACrux.Base.Convert.intParse(strDie[1]);
						iDieShotX = (iDieX - shGapX) % m_oShot.CountX;
						iDieShotY = (iDieY - shGapY) % m_oShot.CountY;

						ds.Tables[0].Rows.Add(new object[] { iDieX, iDieY, iShotX, iShotY, iDieShotX, iDieShotY, i });
					}
				}

				return ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (ds != null) ds.Dispose();
			}
		}

	}
}
