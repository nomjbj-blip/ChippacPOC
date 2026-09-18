using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace DACrux.Utility
{
	public class HFocus
	{
		delegate void DelgDisposPictureBox(PictureBox pb);

		int m_alpha = 50;
		int m_time = 50;
		Control m_parent = null;
		Control[] m_ctl = null;

		delegate Point DelgParentPoint(int ctlIdx);
		delegate void DelgCtlVIsiable(int ctlIdx, int inCtlIdx, bool visible);
		delegate void DelgAddPicture(int ctlIdx, PictureBox pb);
		public delegate void DelgEndAni();
		public event DelgEndAni OnEndAni;

		Control m_noticeCtl = null;
		Control m_noticeParent = null;
		int m_noticeDelayTime = 0;

		delegate Point DelgParentPointCtl(Control ctl);
		delegate void DelgCtlVIsiableCtl(Control ctl, int inCtlIdx, bool visible);
		delegate void DelgAddPictureCtl(Control ctl, PictureBox pb);

		[DllImport("user32.dll")]
		static extern IntPtr GetDC(IntPtr hwnd);
		[DllImport("gdi32.dll")]
		extern static IntPtr GetCurrentObject(IntPtr hdc, ushort objectType);
		[DllImport("user32.dll")]
		extern static void ReleaseDC(IntPtr hdc);

		Point ParentPoint(int ctlIdx)
		{
			return m_parent.PointToScreen(m_ctl[ctlIdx].Location);
		}

		Point ParentPointCtl(Control ctl)
		{
			return m_noticeParent.PointToScreen(ctl.Location);
		}

		void CtlVisible(int ctlIdx, int inCtlIdx, bool visible)
		{
			if (m_ctl[ctlIdx].Controls[inCtlIdx].Dock != DockStyle.None)
				m_ctl[ctlIdx].Controls[inCtlIdx].Visible = visible;
		}

		void CtlVisibleCtl(Control ctl, int inCtlIdx, bool visible)
		{
			if (ctl.Controls[inCtlIdx].Dock != DockStyle.None)
				ctl.Controls[inCtlIdx].Visible = visible;
		}

		void AddPictureBox(int ctlIdx, PictureBox pb)
		{
			m_ctl[ctlIdx].Controls.Add(pb);
			pb.BringToFront();
		}

		void AddPictureBoxCtl(Control ctl, PictureBox pb)
		{
			ctl.Controls.Add(pb);
			pb.BringToFront();
		}

		void DisposePictureBox(PictureBox pb)
		{
			pb.Dispose();
			pb = null;
		}

		void AniStart()
		{
			IntPtr ptrDc = IntPtr.Zero;
			IntPtr ptrBitmaps = IntPtr.Zero;
			Image image = null;
			Brush brush = null;
			try
			{
				DelgParentPoint delgParentPoint = new DelgParentPoint(ParentPoint);
				DelgCtlVIsiable delgCtlVisible = new DelgCtlVIsiable(CtlVisible);
				DelgAddPicture delgAddPictureBox = new DelgAddPicture(AddPictureBox);
				DelgDisposPictureBox delgDisposePictureBox = new DelgDisposPictureBox(DisposePictureBox);

				ptrDc = GetDC(IntPtr.Zero);
				ptrBitmaps = GetCurrentObject(ptrDc, 7);
				image = Image.FromHbitmap(ptrBitmaps);

				brush = new SolidBrush(Color.FromArgb(m_alpha, 255, 0, 0));

				for (int a = 0; a < m_ctl.Length; a++)
				{
					if (m_ctl[a].GetType().Name.Equals("TabControl"))
					{
						throw new Exception(string.Format("Can not support {0}.", m_ctl[a].GetType().Name));
					}

					//m_ctl[a].SuspendLayout();

					Point p = (Point)m_parent.Invoke(delgParentPoint, new object[] {a });
					Rectangle rect1 = new Rectangle(0, 0, m_ctl[a].Width, m_ctl[a].Height);
					Rectangle rect2 = new Rectangle(p.X, p.Y, m_ctl[a].Width, m_ctl[a].Height);

					Bitmap btm = new Bitmap(m_ctl[a].Width, m_ctl[a].Height);
					Graphics g = Graphics.FromImage(btm);

					g.DrawImage(image, rect1, rect2, GraphicsUnit.Pixel);
					g.FillRectangle(brush, rect1);

					g.Dispose();
					g = null;

					bool[] ctlVisible = new bool[m_ctl[a].Controls.Count];
					for (int b = 0; b < m_ctl[a].Controls.Count; b++)
					{
						ctlVisible[b] = m_ctl[a].Controls[b].Visible;
						m_ctl[a].Invoke(delgCtlVisible, new object[] {a, b, false });
					}

					PictureBox pb = new PictureBox();
					pb.Image = btm;
					pb.Dock = DockStyle.Fill;
					m_ctl[a].Invoke(delgAddPictureBox, new object[] { a, pb});

					//m_ctl[a].ResumeLayout(false);

					System.Threading.Thread.Sleep(m_time);

					btm.Dispose();
					btm = null;
					pb.Invoke(delgDisposePictureBox, new object[] { pb});

					for (int b = 0; b < m_ctl[a].Controls.Count; b++)
						m_ctl[a].Invoke(delgCtlVisible, new object[] {a, b, ctlVisible[b] });
				}

				if (OnEndAni != null) OnEndAni();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				ReleaseDC(ptrDc);
				ReleaseDC(ptrBitmaps);
				if (image != null)
				{
					image.Dispose();
					image = null;
				}
				if (brush != null)
				{
					brush.Dispose();
					brush = null;
				}
			}
		}

		void NoticeStart()
		{
			IntPtr ptrDc = IntPtr.Zero;
			IntPtr ptrBitmaps = IntPtr.Zero;
			Image image = null;
			Brush brush = null;

			try
			{
				DelgParentPointCtl delgParentPointCtl = new DelgParentPointCtl(ParentPointCtl);
				DelgCtlVIsiableCtl delgCtlVisibleCtl = new DelgCtlVIsiableCtl(CtlVisibleCtl);
				DelgAddPictureCtl delgAddPictureBoxCtl = new DelgAddPictureCtl(AddPictureBoxCtl);
				DelgDisposPictureBox delgDisposePictureBox = new DelgDisposPictureBox(DisposePictureBox);

				ptrDc = GetDC(IntPtr.Zero);
				ptrBitmaps = GetCurrentObject(ptrDc, 7);
				image = Image.FromHbitmap(ptrBitmaps);

				brush = new SolidBrush(Color.FromArgb(m_alpha, 255, 0, 0));

				if (m_noticeCtl.GetType().Name.Equals("TabControl"))
				{
					throw new Exception(string.Format("Can not support {0}.", m_noticeCtl.GetType().Name));
				}

				Point p = (Point)m_noticeParent.Invoke(delgParentPointCtl, new object[] { m_noticeCtl });
				Rectangle rect1 = new Rectangle(0, 0, m_noticeCtl.Width, m_noticeCtl.Height);
				Rectangle rect2 = new Rectangle(p.X, p.Y, m_noticeCtl.Width, m_noticeCtl.Height);

				Bitmap btm = new Bitmap(m_noticeCtl.Width, m_noticeCtl.Height);
				Graphics g = Graphics.FromImage(btm);

				g.DrawImage(image, rect1, rect2, GraphicsUnit.Pixel);
				g.FillRectangle(brush, rect1);

				g.Dispose();
				g = null;

				bool[] ctlVisible = new bool[m_noticeCtl.Controls.Count];
				for (int b = 0; b < m_noticeCtl.Controls.Count; b++)
				{
					ctlVisible[b] = m_noticeCtl.Controls[b].Visible;
					m_noticeCtl.Invoke(delgCtlVisibleCtl, new object[] { m_noticeCtl, b, false });
				}

				PictureBox pb = new PictureBox();
				pb.Image = btm;
				pb.Dock = DockStyle.Fill;
				m_noticeCtl.Invoke(delgAddPictureBoxCtl, new object[] { m_noticeCtl, pb });

				System.Threading.Thread.Sleep(m_time);

				btm.Dispose();
				btm = null;
				pb.Invoke(delgDisposePictureBox, new object[] { pb });

				for (int b = 0; b < m_noticeCtl.Controls.Count; b++)
					m_noticeCtl.Invoke(delgCtlVisibleCtl, new object[] { m_noticeCtl, b, ctlVisible[b] });
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				ReleaseDC(ptrDc);
				ReleaseDC(ptrBitmaps);
				if (image != null)
				{
					image.Dispose();
					image = null;
				}
				if (brush != null)
				{
					brush.Dispose();
					brush = null;
				}
			}
		}

		/// <summary>
		/// 순서대로 깜빡여준다. 투명도는 50, 붉은색 유지 시간은시간은 50 ms
		/// </summary>
		/// <param name="parent">깜빡이려는 컨트롤의 부모 컨트롤</param>
		/// <param name="ctl">깜빡이려는 컨트롤들</param>
		public void AniStart(Control parent, Control[] ctl)
		{
			m_parent = parent;
			m_ctl = ctl;
			if (!m_parent.Visible) return;

			m_alpha = 50;
			m_time = 50;

			System.Threading.Thread trd = new System.Threading.Thread(
				new System.Threading.ThreadStart(AniStart));
			trd.Start();
		}

		/// <summary>
		/// 컨트롤을 두번 깜빡인다. 붉은색 알파값은 100. 깜빡임 유지 시간은 100ms
		/// </summary>
		/// <param name="ctl">깜빡거릴 컨트롤</param>
		public void Notice(Control ctl)
		{
			m_noticeParent = ctl.Parent;
			m_noticeCtl =  ctl;
			m_noticeDelayTime = 0;
			if (!m_noticeParent.Visible) return;

			m_alpha = 100;
			m_time = 100;

			System.Threading.Thread trd = new System.Threading.Thread(
				new System.Threading.ThreadStart(Notice));
			trd.Start();
		}

		/// <summary>
		/// 컨트롤을 두번 깜빡인다. 붉은색 알파값은 100. 깜빡임 유지 시간은 100ms
		/// delay time 설정가능
		/// </summary>
		/// <param name="ctl">깜빡거릴 컨트롤</param>
		/// <param name="noticeDelayTime">이 함수를 호출 한 후 noticeDelayTime 이 지난 후에 두번 깜빡인다</param>
		public void Notice(Control ctl, int noticeDelayTime)
		{
			m_noticeParent = ctl.Parent;
			m_noticeCtl = ctl;
			m_noticeDelayTime = noticeDelayTime;
			if (!m_noticeParent.Visible) return;

			m_alpha = 100;
			m_time = 100;

			System.Threading.Thread trd = new System.Threading.Thread(
				new System.Threading.ThreadStart(Notice));
			trd.Start();
		}

		void Notice()
		{
			System.Threading.Thread.Sleep(m_noticeDelayTime);

			NoticeStart();
			System.Threading.Thread.Sleep(100);
			NoticeStart();
		}
	}
}
