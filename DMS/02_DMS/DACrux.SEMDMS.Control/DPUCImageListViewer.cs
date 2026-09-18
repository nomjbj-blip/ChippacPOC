using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;

using DACrux.Base;

namespace DACrux.SEMDMS.Control
{
	/// <summary>
	/// DPUCImageListViewer에 대한 요약 설명입니다.
	/// </summary>
	public class DPUCImageListViewer : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel pnlImageList;
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		ArrayList m_arrImgList = null;

		public DPUCImageListViewer()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

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
            this.pnlImageList = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlImageList
            // 
            this.pnlImageList.AutoScroll = true;
            this.pnlImageList.BackColor = System.Drawing.Color.White;
            this.pnlImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImageList.Location = new System.Drawing.Point(0, 0);
            this.pnlImageList.Name = "pnlImageList";
            this.pnlImageList.Padding = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.pnlImageList.Size = new System.Drawing.Size(504, 392);
            this.pnlImageList.TabIndex = 0;
            // 
            // DPUCImageListViewer
            // 
            this.Controls.Add(this.pnlImageList);
            this.Name = "DPUCImageListViewer";
            this.Size = new System.Drawing.Size(504, 392);
            this.ResumeLayout(false);

		}
		#endregion

		public void Reset()
		{
			if(m_arrImgList == null)
				m_arrImgList = new ArrayList();
			pnlImageList.Controls.Clear();
		}

        public void AddImage(Image oImg, DACrux.Base.IMAGES_TAG ImgInfo)
		{
			PictureBox oPicBox = null;
			try
			{
				if(m_arrImgList == null)
					m_arrImgList = new ArrayList();

				oPicBox = new System.Windows.Forms.PictureBox();
				oPicBox.Location = new System.Drawing.Point(0, 0);
				oPicBox.Name = ImgInfo.image_filename;
				oPicBox.Size = new System.Drawing.Size(376, 257);
				oPicBox.TabIndex = 0;
				oPicBox.TabStop = false;
				oPicBox.Dock = DockStyle.Top;
				oPicBox.SizeMode = PictureBoxSizeMode.StretchImage;
				oPicBox.Image = oImg;
				oPicBox.Visible = true;
				oPicBox.BorderStyle = BorderStyle.FixedSingle;
				pnlImageList.Controls.Add(oPicBox);
				m_arrImgList.Add(ImgInfo);

				Panel p = new Panel();
				p.Height = 6;
				p.Dock = DockStyle.Top;
				pnlImageList.Controls.Add(p);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void AddImage(Stream oImgStream,DACrux.Base.IMAGES_TAG ImgInfo)
		{
			Image oImg = null;
			try
			{
				oImg = Image.FromStream(oImgStream);
				AddImage(oImg,ImgInfo);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(oImg != null) oImg.Dispose();
				oImg = null;
			}
		}

        public void AddImage(string ImageURL, DACrux.Base.IMAGES_TAG ImgInfo)
		{
			System.Net.WebClient myWebClient = null;
			System.IO.Stream myStream = null;
			Image oImg = null;
			try
			{
				myWebClient = new System.Net.WebClient();
				myStream = myWebClient.OpenRead(ImageURL);
				oImg = Image.FromStream(myStream);
				AddImage(oImg,ImgInfo);;

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(myWebClient != null) myWebClient.Dispose();
				myWebClient = null;

				if(myStream != null) myStream.Close();


			}
		}

	}
}
