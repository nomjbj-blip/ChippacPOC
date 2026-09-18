using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
	/// <summary>
	/// UserControl1에 대한 요약 설명입니다.
	/// </summary>
	public class DPUCImageGallery : System.Windows.Forms.UserControl
	{
		public struct ImgInfo
		{
			public long strStepSeq;
			public int iDefectId;
			public int iImageId;
			public string strServer;
			public string strPath;
		};
		public string gstrLotId;
		public string gstrWaferId;
		public string gstrStepId;
		public System.Windows.Forms.TextBox gtxtImageUrl;

		public System.Collections.ArrayList galImg;
		public System.Windows.Forms.ListView glv;
		public System.Windows.Forms.ImageList giml;
		private System.Windows.Forms.ColumnHeader columnHeader;
		public System.Windows.Forms.ContextMenu gcmn;
		private System.Windows.Forms.MenuItem menuItemRemove;
		private System.Windows.Forms.MenuItem menuItemSli;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.MenuItem menuItemClass;
		private System.Windows.Forms.MenuItem menuItemMapGallery;
		private System.Windows.Forms.MenuItem menuItemImageGallery;
		private System.ComponentModel.IContainer components;

		public DPUCImageGallery()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitComponent를 호출한 다음 초기화 작업을 추가합니다.
			galImg = new ArrayList();
			gtxtImageUrl = null;
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
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
			this.components = new System.ComponentModel.Container();
			this.glv = new System.Windows.Forms.ListView();
			this.columnHeader = new System.Windows.Forms.ColumnHeader();
			this.giml = new System.Windows.Forms.ImageList(this.components);
			this.gcmn = new System.Windows.Forms.ContextMenu();
			this.menuItemRemove = new System.Windows.Forms.MenuItem();
			this.menuItemSli = new System.Windows.Forms.MenuItem();
			this.menuItemClass = new System.Windows.Forms.MenuItem();
			this.menuItemMapGallery = new System.Windows.Forms.MenuItem();
			this.menuItemImageGallery = new System.Windows.Forms.MenuItem();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// glv
			// 
			this.glv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.glv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																				  this.columnHeader});
			this.glv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glv.HideSelection = false;
			this.glv.LargeImageList = this.giml;
			this.glv.Location = new System.Drawing.Point(0, 0);
			this.glv.Name = "glv";
			this.glv.Size = new System.Drawing.Size(192, 240);
			this.glv.TabIndex = 0;
			this.glv.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
			this.glv.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView_MouseUp);
			// 
			// columnHeader
			// 
			this.columnHeader.Text = "Thumb List";
			this.columnHeader.Width = 77;
			// 
			// giml
			// 
			this.giml.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.giml.ImageSize = new System.Drawing.Size(82, 82);
			this.giml.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// gcmn
			// 
			this.gcmn.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				 this.menuItemRemove,
																				 this.menuItemSli,
																				 this.menuItemClass,
																				 this.menuItemMapGallery,
																				 this.menuItemImageGallery});
			// 
			// menuItemRemove
			// 
			this.menuItemRemove.Index = 0;
			this.menuItemRemove.Text = "Remove";
			this.menuItemRemove.Click += new System.EventHandler(this.menuItemRemove_Click);
			// 
			// menuItemSli
			// 
			this.menuItemSli.Index = 1;
			this.menuItemSli.Text = "Save Large Image";
			this.menuItemSli.Click += new System.EventHandler(this.menuItemSli_Click);
			// 
			// menuItemClass
			// 
			this.menuItemClass.Index = 2;
			this.menuItemClass.Text = "Class 입력";
			// 
			// menuItemMapGallery
			// 
			this.menuItemMapGallery.Index = 3;
			this.menuItemMapGallery.Text = "Map Gallery";
			// 
			// menuItemImageGallery
			// 
			this.menuItemImageGallery.Index = 4;
			this.menuItemImageGallery.Text = "Image Gallery";
			// 
			// DPUCImageGallery
			// 
			this.AutoScroll = true;
			this.Controls.Add(this.glv);
			this.Name = "DPUCImageGallery";
			this.Size = new System.Drawing.Size(192, 240);
			this.ResumeLayout(false);

		}
		#endregion

		public void Add(string id, System.Drawing.Image image)
		{
			int i;
			for(i = 0; i < glv.Items.Count; i++)
			{
				if(id == glv.Items[i].Text)
				{
					System.Windows.Forms.MessageBox.Show(this, "Exist map.", "DMSPlus");
					return;
				}
			}
			giml.Images.Add(image);
			glv.Items.Add(id, giml.Images.Count - 1);
		}

		public void Add(string id, System.Drawing.Image image,
			long strStepSeq, int iDefectId, int iImageId, string strServer, string strPath)
		{
			int i;
			for(i = 0; i < glv.Items.Count; i++)
			{
				if(id == glv.Items[i].Text)
				{
					System.Windows.Forms.MessageBox.Show(this, "Exist map.", "DMSPlus");
					return;
				}
			}
			giml.Images.Add(image);
			glv.Items.Add(id, giml.Images.Count - 1);

			ImgInfo imgInfo;
			imgInfo.strStepSeq = strStepSeq;
			imgInfo.iDefectId = iDefectId;
			imgInfo.iImageId = iImageId;
			imgInfo.strServer = strServer;
			imgInfo.strPath = strPath;

			galImg.Add(imgInfo);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
		}

		private void listView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			/*
			if(e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				gcmn.Show(this, new System.Drawing.Point(e.X, e.Y));
			}
			*/
			this.glv.ContextMenu = this.gcmn;

			if(glv.SelectedItems.Count == 1 && gcmn.MenuItems[1].Enabled == true && gtxtImageUrl != null)
			{
				int itemNo = glv.SelectedItems[0].Index;
				string imagePath = GetImagePath(itemNo);
				imagePath = imagePath.Replace("\\", "/");
				if(gtxtImageUrl != null) gtxtImageUrl.Text = imagePath;
			}
		}

		private void menuItemRemove_Click(object sender, System.EventArgs e)
		{
			int i;
			for(i = glv.SelectedItems.Count - 1; i >= 0; i--)
			{
				glv.SelectedItems[i].Remove();
			}
		}

		public string GetStepSeqList()
		{
			string stepSeqList = "";
			for(int i = 0; i < glv.Items.Count; i++)
			{
				if(stepSeqList == "") stepSeqList = glv.Items[i].Text;
				else stepSeqList += ", " + glv.Items[i].Text;
			}

			return stepSeqList;
		}

		private void menuItemSli_Click(object sender, System.EventArgs e)
		{
			SaveImage();
		}

		void SaveLargeImage(int no, string strFileName, int formatIndex)
		{
			ImgInfo imgInfo = (ImgInfo)galImg[no];
			string imagePath = GetImagePath(no);
			System.Net.WebClient myWebClient = new System.Net.WebClient();
			System.IO.Stream myStream = myWebClient.OpenRead(imagePath);
			Image btm = Image.FromStream(myStream);

			switch(formatIndex)
			{
				case 1:
					btm.Save(strFileName, System.Drawing.Imaging.ImageFormat.Png);
					break;
				case 2:
					btm.Save(strFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
					break;
				case 3:
					btm.Save(strFileName, System.Drawing.Imaging.ImageFormat.Bmp);
					break;
				default:
					btm.Save(strFileName, System.Drawing.Imaging.ImageFormat.Gif);
					break;
			}
		}

		public void SaveImage()
		{
			Cursor = System.Windows.Forms.Cursors.WaitCursor;

			if(glv.SelectedItems.Count == 1)
			{
				saveFileDialog = new SaveFileDialog();
				saveFileDialog.Filter = "Png files (*.png)|*.png|Jpeg files (*.jpg)|*.jpg|Bitmap files (*.bmp)|*.bmp|Gif files (*.gif)|*.gif";
				saveFileDialog.DefaultExt = "png";
				ImgInfo imgInfo = (ImgInfo)galImg[glv.SelectedItems[0].Index];
				saveFileDialog.FileName = string.Format("{0}_{1}_{2}_{3}_{4}",
					this.gstrLotId, this.gstrWaferId, this.gstrStepId, imgInfo.iDefectId.ToString().PadLeft(4, '0'), imgInfo.iImageId.ToString().PadLeft(2, '0'));

				if(saveFileDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK) return;
				SaveLargeImage(glv.SelectedItems[0].Index, saveFileDialog.FileName, saveFileDialog.FilterIndex);
			}
			else
			{
				string strFileName;
				if(folderBrowserDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK) return;
				System.Windows.Forms.MessageBox.Show(this, "Path : " + folderBrowserDialog.SelectedPath + "\r\nSave file format : LotID_WaferID_StepID_DefectID_ImageID.png", "DMSPlus");
				for(int i = 0; i < glv.SelectedItems.Count; i++)
				{
					ImgInfo imgInfo = (ImgInfo)galImg[glv.SelectedItems[i].Index];
					strFileName = string.Format("{0}_{1}_{2}_{3}_{4}",
						this.gstrLotId, this.gstrWaferId, this.gstrStepId, imgInfo.iDefectId.ToString().PadLeft(4, '0'), imgInfo.iImageId.ToString().PadLeft(2, '0'));
					strFileName = folderBrowserDialog.SelectedPath + "\\" + strFileName + ".png";
					SaveLargeImage(glv.SelectedItems[i].Index, strFileName, 1);
				}
			}

			Cursor = System.Windows.Forms.Cursors.Default;
			System.Windows.Forms.MessageBox.Show(this, "Save completion.", "DMSPlus");
		}

		private void listView_DoubleClick(object sender, System.EventArgs e)
		{
			System.Net.WebClient myWebClient = null;
			LargeImage largeImg = null;
			try
			{
				if(gcmn.MenuItems[1].Enabled == false) return;
				Cursor = System.Windows.Forms.Cursors.WaitCursor;

				int itemNo = glv.SelectedItems[0].Index;

				myWebClient = new System.Net.WebClient();
				System.IO.Stream myStream = myWebClient.OpenRead(GetImagePath(itemNo));
				Image btm = Image.FromStream(myStream);

				largeImg = new LargeImage();
				//largeImg.gimg.Size = new System.Drawing.Size(btm.Width + 10, btm.Height + 25);
				largeImg.Text = glv.Items[itemNo].Text;
				largeImg.gimg.Image = btm;
				largeImg.Show();

				Cursor = System.Windows.Forms.Cursors.Default;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public string GetImagePath(int iItemNo)
		{
//			Remoting o = null;
//			DataSet ds = null;
//			DataSet ds2 = null;
//			string strDefectImageUrl = "";
			string imagePath = "";
//			string strImageRoot;
//			try
//			{
//				o = new Remoting();
//				ds = o.GetConfig("DMSPlusUser", "GetConfig", "MAPANALYSIS", "DEFECTIMAGEURL");
//				ds2 = o.GetConfig("DMSPlusUser", "GetConfig", "INTERFACE", "IMAGEPATH");
//				if(ds.Tables[0].Rows.Count > 0) strDefectImageUrl = ds.Tables[0].Rows[0]["VALUE"].ToString();
//
//				ImgInfo imgInfo = (ImgInfo)galImg[iItemNo];
//				System.IO.FileInfo fileInfo = new System.IO.FileInfo(imgInfo.strPath);
//				//string imagePath = "http://"+imgInfo.strServer+"/image/" + fileInfo.FullName.Remove(0, 8);
//				strImageRoot = ds2.Tables[0].Rows[0]["VALUE"].ToString().Trim();
//				imagePath = strDefectImageUrl + fileInfo.FullName.Remove(0,strImageRoot.Length);
//			}
//			catch(Exception ex)
//			{
//				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//			}
//			finally
//			{
//				if(ds != null) { ds.Dispose(); ds = null; }
//				if(ds2 != null) { ds2.Dispose(); ds2 = null; }
//			}
			return imagePath;
		}

		public string GetImagePathLocal(int iItemNo)
		{
			ImgInfo imgInfo = (ImgInfo)galImg[iItemNo];
			System.IO.FileInfo fileInfo = new System.IO.FileInfo(imgInfo.strPath);
			//string imagePath = "http://"+imgInfo.strServer+"/image/" + fileInfo.FullName.Remove(0, 8);
			string imagePath = fileInfo.FullName;
			return imagePath;
		}

		public void RemoveAll()
		{
			galImg.Clear();
			glv.Items.Clear();
		}

		public void SelectAll()
		{
			int i;
			for(i = 0; i < glv.Items.Count; i++)
			{
				glv.Items[i].Selected = true;
			}
		}




	}
}
