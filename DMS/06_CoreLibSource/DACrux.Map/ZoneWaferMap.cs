using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.Map
{
	public partial class ZoneWaferMap : DACrux.Map.WaferMap
	{
		private Color m_colCurrentSelZone = Color.White;

		public ZoneWaferMap()
		{
			// 이 호출은 Windows Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.
		}

		public void SetDieZone(int x, int y, int ZoneNumber)
		{
			try
			{
				int iDieIdx = m_DieIndexer.IndexOf(new Point(x, y));
				if (iDieIdx < 0) return;
				Die oDie = m_arrDies[iDieIdx];
				oDie.ZoneNumber = ZoneNumber;

				this.m_arrDies.RemoveAt(iDieIdx);
				this.m_arrDies.Insert(iDieIdx, oDie);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#region ◈ Die Drawing
		protected override void DrawDies(Graphics g)
		{
			m_WaferRecipe.NETDIE = 0;
			PointF[] pfOriginDiePoints = null;
			PointF[] pfFirstDiePoints = null;

			PointD[] pdPoint = new PointD[4];
			PointF[] pfPoint = new PointF[4];

			Pen pDieBorder = null;
			Pen pOriginDieBorder = null;
			Pen pFirstDieBorder = null;

			Pen pCenterMark = null;

			SolidBrush sbrshDie = null;

			try
			{
				pDieBorder = new Pen(m_colDieBorder);
				pOriginDieBorder = new Pen(m_colOriginDieBorder);
				pFirstDieBorder = new Pen(m_colFirstDieBorder);
				sbrshDie = new SolidBrush(Color.White);

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

					
                    // 2014.04.21 : 주영진 수정
                    // Color 로 Data 가 들어있기 때문
                    #region 기존
                    //if (InDie.ZoneNumber > -1)
                    //{
                    //    switch (InDie.DieProp)
                    //    {
                    //        case 0:
                    //            sbrshDie.Color = Color.FromArgb(30, m_ColorSet[InDie.ZoneNumber]);
                    //            m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                    //            break;
                    //        case 1:
                    //            sbrshDie.Color = Color.FromArgb(160, m_ColorSet[InDie.ZoneNumber]);
                    //            m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                    //            m_WaferRecipe.NETDIE++;
                    //            break;
                    //        case 2:
                    //            sbrshDie.Color = Color.FromArgb(160, m_colMarkDieColor);
                    //            m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                    //            break;
                    //        case 3:
                    //            sbrshDie.Color = Color.FromArgb(160, m_ColorSet[InDie.ZoneNumber]);
                    //            m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                    //            sbrshDie.Color = Color.FromArgb(160, m_colMarkDieColor);
                    //            m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
                    //            m_WaferRecipe.NETDIE++;
                    //            break;
                    //    }
                    //}
                    #endregion
                    if (InDie.ZoneNumber > -1)
					{
                        string color = InDie.ZoneNumber.ToString().PadLeft(9, '0');
                        int R = DACrux.Base.Convert.intParse(color.Substring(0, 3));
                        int G = DACrux.Base.Convert.intParse(color.Substring(3, 3));
                        int B = DACrux.Base.Convert.intParse(color.Substring(6, 3));

						switch (InDie.DieProp)
						{
							case 0:
                                sbrshDie.Color = Color.FromArgb(30, Color.FromArgb(R, G, B));
								m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
								break;
							case 1:
                                sbrshDie.Color = Color.FromArgb(160, Color.FromArgb(R, G, B));
								m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
								m_WaferRecipe.NETDIE++;
								break;
							case 2:
                                sbrshDie.Color = Color.FromArgb(160, Color.FromArgb(R, G, B));
								m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
								break;
							case 3:
                                sbrshDie.Color = Color.FromArgb(160, Color.FromArgb(R, G, B));
								m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
								sbrshDie.Color = Color.FromArgb(160, m_colMarkDieColor);
								m_gdiTempMap.FillPolygon(sbrshDie, pfPoint);
								m_WaferRecipe.NETDIE++;
								break;
						}

						if (InDie.IndexX == m_WaferRecipe.ORIGIN_DIE_X && InDie.IndexY == m_WaferRecipe.ORIGIN_DIE_Y)
						{
							pfOriginDiePoints = new PointF[pfPoint.Length];
							Array.Copy(pfPoint, pfOriginDiePoints, pfPoint.Length);
						}

						if (InDie.IndexX == m_WaferRecipe.FIRST_DIE_X && InDie.IndexY == m_WaferRecipe.FIRST_DIE_Y)
						{
							pfFirstDiePoints = new PointF[pfPoint.Length];
							Array.Copy(pfPoint, pfFirstDiePoints, pfPoint.Length);
						}

						g.DrawPolygon(pDieBorder, pfPoint);
					}

				}

				if (m_bDrawOriginDie && pfOriginDiePoints != null)
				{
					g.DrawPolygon(pOriginDieBorder, pfOriginDiePoints);
				}

				if (m_bDrawFirstDie && pfFirstDiePoints != null)
				{
					g.DrawPolygon(pFirstDieBorder, pfFirstDiePoints);
				}

				if (m_bCenterMark)
				{
					pCenterMark = new Pen(Color.Red);
					g.DrawLine(pCenterMark, (float)(-m_rectdWaferArea.X * (m_dZoomRatio * m_dScale))
						, (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2) * (m_dZoomRatio * m_dScale))
						, (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio * m_dScale))
						, (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE / 2) * (m_dZoomRatio * m_dScale)));

					g.DrawLine(pCenterMark, (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2) * (m_dZoomRatio * m_dScale))
						, (float)(-m_rectdWaferArea.Y * (m_dZoomRatio * m_dScale))
						, (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_SIZE / 2) * (m_dZoomRatio * m_dScale))
						, (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_SIZE) * (m_dZoomRatio * m_dScale)));
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				pdPoint = null;
				pfPoint = null;

				if (pDieBorder != null) pDieBorder.Dispose();
				if (pOriginDieBorder != null) pOriginDieBorder.Dispose();
				if (pFirstDieBorder != null) pFirstDieBorder.Dispose();
				if (pCenterMark != null) pCenterMark.Dispose();
				if (sbrshDie != null) sbrshDie.Dispose();
			}
		}
		#endregion
	}
}
