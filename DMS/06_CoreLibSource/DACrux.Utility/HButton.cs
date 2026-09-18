using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace DACrux.Utility
{
	[DefaultEvent("Click")]
	public partial class HButton : UserControl
	{
		Bitmap m_button = null;
		Graphics m_g = null;
		string m_text = null;
		Color m_textColor = Color.Black;
		bool m_mouseOver = false;
		bool m_mouseDown = false;
		public enum GRADIENT_FOWORD { LEFT_TO_RIGHT, RIGHT_TO_LEFT, TOP_TO_BOTTOM, BOTTOM_TO_TOP };
		GRADIENT_FOWORD m_gradientFoword = GRADIENT_FOWORD.TOP_TO_BOTTOM;
		Color m_gradientFrom = Color.FromArgb(249, 255, 200);
		Color m_gradientTo = Color.FromArgb(188, 200, 217);
		Font m_font = null;

		public Font BtnFont
		{
			get { return m_font; }
			set
			{
				m_font = value;
				Draw();
			}
		}
		public Color GradientFrom
		{
			get { return m_gradientFrom; }
			set
			{
				m_gradientFrom = value;
				Draw();
			}
		}
		public Color GradientTo
		{
			get { return m_gradientTo; }
			set
			{
				m_gradientTo = value;
				Draw();
			}
		}
		public GRADIENT_FOWORD GradientFoword
		{
			get { return m_gradientFoword; }
			set
			{
				m_gradientFoword = value;
				Draw();
			}
		}
		public Image BtnIcon
		{
			get { return pictureBoxIcon.Image; }
			set
			{
				if (pictureBoxIcon.Image != null)
				{
					pictureBoxIcon.Image.Dispose();
					pictureBoxIcon.Image = null;
				}

				pictureBoxIcon.Image = value;
			}
		}
		public Color BtnTextColor
		{
			get { return m_textColor; }
			set
			{
				m_textColor = value;
				Draw();
			}
		}
		public string BtnText
		{
			get { return m_text; }
			set { m_text = value;
			Draw();
			}
		}
		
		public HButton()
		{
			InitializeComponent();
			m_font = this.Font;
		}

		void CreateObj()
		{
			if (m_button != null)
			{
				m_button.Dispose();
				m_button = null;
			}
			m_button = new Bitmap(this.Width, this.Height);

			if (m_g != null)
			{
				m_g.Dispose();
				m_g = null;
			}
			m_g = Graphics.FromImage(m_button);
		}

		void DrawText()
		{
			int margin = pictureBoxIcon.Width / 2;
			SolidBrush brush = new SolidBrush(m_textColor);
			RectangleF rect = new RectangleF(margin, 0, this.Width - margin, this.Height);
			RectangleF shRect = new RectangleF(margin + 1, 1, this.Width - margin, this.Height);
			StringFormat sf = new StringFormat();
			SolidBrush sh = new SolidBrush(Color.White);
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			m_g.DrawString(m_text, m_font, sh, shRect, sf);
			m_g.DrawString(m_text, m_font, brush, rect, sf);
			sf.Dispose();
			sf = null;
			brush.Dispose();
			brush = null;
			sh.Dispose();
			sh = null;
		}

		void Draw()
		{
			if (this.Width < 5) return;
			if (this.Height < 5) return;

			CreateObj();
			DrawGrad();
			DrawText();
			DrawOut();
			DrawRound();
			DrawMouseOver();

			if(this.BackgroundImage != null)
			{
			this.BackgroundImage.Dispose();
			this.BackgroundImage = null;
			}
			this.BackgroundImage = (Image)m_button.Clone();
		}

		private void HButton_SizeChanged(object sender, EventArgs e)
		{
			Draw();
		}

		void DrawGrad()
		{
			if (!m_mouseDown)
			{
				LinearGradientBrush linGrBrush = null;
				switch (m_gradientFoword)
				{
					case GRADIENT_FOWORD.TOP_TO_BOTTOM:
						linGrBrush = new LinearGradientBrush(
							new Point(0, 0)
							, new Point(0, this.Height)
							, m_gradientFrom
							, m_gradientTo
							);
						break;
					case GRADIENT_FOWORD.BOTTOM_TO_TOP:
						linGrBrush = new LinearGradientBrush(
							new Point(0, this.Height)
							, new Point(0, 0)
							, m_gradientFrom
							, m_gradientTo
							);
						break;
					case GRADIENT_FOWORD.LEFT_TO_RIGHT:
						linGrBrush = new LinearGradientBrush(
							new Point(0, 0)
							, new Point(this.Width, 0)
							, m_gradientFrom
							, m_gradientTo
							);
						break;
					case GRADIENT_FOWORD.RIGHT_TO_LEFT:
						linGrBrush = new LinearGradientBrush(
							new Point(this.Width, 0)
							, new Point(0, 0)
							, m_gradientFrom
							, m_gradientTo
							);
						break;
				}

				Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
				m_g.FillRectangle(linGrBrush, rect);
				linGrBrush.Dispose();
				linGrBrush = null;
			}
			else
			{
				SolidBrush brush = new SolidBrush(Color.LightGray);
				Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
				m_g.FillRectangle(brush, rect);
				brush.Dispose();
				brush = null;
			}
		}

		void DrawOut()
		{
			Pen pen = new Pen(Color.FromArgb(2, 57, 121));
			Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
			m_g.DrawRectangle(pen, rect);
			pen.Dispose();
			pen = null;
		}

		void DrawRound()
		{
			Pen pen = new Pen(SystemColors.Control);

			m_g.DrawLine(pen, 0, 0, 1, 0);
			m_g.DrawLine(pen, 0, 0, 0, 1);

			m_g.DrawLine(pen, this.Width - 2, 0, this.Width-1, 0);
			m_g.DrawLine(pen, this.Width -1, 0, this.Width -1 , 1);

			m_g.DrawLine(pen, 0, this.Height - 2, 0, this.Height - 1);
			m_g.DrawLine(pen, 0, this.Height -1, 1, this.Height -1);

			m_g.DrawLine(pen, this.Width - 1, this.Height - 2, this.Width - 1, this.Height);
			m_g.DrawLine(pen, this.Width - 2, this.Height - 1, this.Width, this.Height - 1);

			pen.Dispose();
			pen = null;

			Color outColor = Color.FromArgb(2, 57, 121);
			m_button.SetPixel(1, 1, outColor);
			m_button.SetPixel(this.Width - 2, 1, outColor);
			m_button.SetPixel(1, this.Height - 2, outColor);
			m_button.SetPixel(this.Width - 2, this.Height - 2, outColor);
		}

		private void pictureBoxIcon_Click(object sender, EventArgs e)
		{
			if (DesignMode) return;
			OnClick(e);
		}

		void DrawMouseOver()
		{
			if (!m_mouseOver) return;
			if (m_mouseDown) return;

			Pen pen = new Pen(Color.Orange, 2);
			Rectangle rect = new Rectangle(2, 2, this.Width - 4, this.Height - 4);
			m_g.DrawRectangle(pen, rect);
			pen.Dispose();
			pen = null;
		}

		private void HButton_MouseEnter(object sender, EventArgs e)
		{
			try
			{
				m_mouseOver = true;
				Draw();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void pictureBoxIcon_MouseEnter(object sender, EventArgs e)
		{
			try
			{
				OnMouseEnter(e);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void HButton_MouseLeave(object sender, EventArgs e)
		{
			try
			{
				m_mouseOver = false;
				Draw();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void pictureBoxIcon_MouseLeave(object sender, EventArgs e)
		{
			try
			{
				OnMouseLeave(e);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void HButton_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				m_mouseDown = true;
				Draw();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void pictureBoxIcon_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				OnMouseDown(e);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void HButton_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				m_mouseDown = false;
				Draw();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void pictureBoxIcon_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				OnMouseUp(e);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
