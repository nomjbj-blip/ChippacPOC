using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
	/// <summary>
	/// DPUCDefectImgGalleryManyStep에 대한 요약 설명입니다.
	/// </summary>
	public class DPUCDefectImgGalleryManyStep : System.Windows.Forms.UserControl
	{
		System.Threading.Thread thread = null;
        DACrux.Base.DPWafer[] m_stepSeq = null;
		bool threadRun = true;
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDown;
		private System.Windows.Forms.Button buttonToExcel;
		private System.Windows.Forms.Button buttonRedraw;
	
		delegate void GalleryDelegate();

		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DPUCDefectImgGalleryManyStep()
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
				threadRun = false;
				if(thread != null)
				{
					while(thread.ThreadState == System.Threading.ThreadState.Running) System.Threading.Thread.Sleep(100);
				}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DPUCDefectImgGalleryManyStep));
			this.panel = new System.Windows.Forms.Panel();
			this.numericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonToExcel = new System.Windows.Forms.Button();
			this.buttonRedraw = new System.Windows.Forms.Button();
			this.panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.Controls.Add(this.buttonToExcel);
			this.panel.Controls.Add(this.buttonRedraw);
			this.panel.Controls.Add(this.numericUpDown);
			this.panel.Controls.Add(this.label1);
			this.panel.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel.Location = new System.Drawing.Point(0, 0);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(624, 24);
			this.panel.TabIndex = 0;
			// 
			// numericUpDown
			// 
			this.numericUpDown.Increment = new System.Decimal(new int[] {
																			10,
																			0,
																			0,
																			0});
			this.numericUpDown.Location = new System.Drawing.Point(88, 0);
			this.numericUpDown.Maximum = new System.Decimal(new int[] {
																		  1000,
																		  0,
																		  0,
																		  0});
			this.numericUpDown.Minimum = new System.Decimal(new int[] {
																		  10,
																		  0,
																		  0,
																		  0});
			this.numericUpDown.Name = "numericUpDown";
			this.numericUpDown.Size = new System.Drawing.Size(72, 21);
			this.numericUpDown.TabIndex = 2;
			this.numericUpDown.Value = new System.Decimal(new int[] {
																		10,
																		0,
																		0,
																		0});
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "Gallery Height";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonToExcel
			// 
			this.buttonToExcel.Dock = System.Windows.Forms.DockStyle.Right;
			this.buttonToExcel.Image = ((System.Drawing.Image)(resources.GetObject("buttonToExcel.Image")));
			this.buttonToExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonToExcel.Location = new System.Drawing.Point(544, 0);
			this.buttonToExcel.Name = "buttonToExcel";
			this.buttonToExcel.Size = new System.Drawing.Size(80, 24);
			this.buttonToExcel.TabIndex = 13;
			this.buttonToExcel.Text = "    ToExcel";
			this.buttonToExcel.Click += new System.EventHandler(this.buttonToExcel_Click);
			// 
			// buttonRedraw
			// 
			this.buttonRedraw.Image = ((System.Drawing.Image)(resources.GetObject("buttonRedraw.Image")));
			this.buttonRedraw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonRedraw.Location = new System.Drawing.Point(160, 0);
			this.buttonRedraw.Name = "buttonRedraw";
			this.buttonRedraw.Size = new System.Drawing.Size(88, 24);
			this.buttonRedraw.TabIndex = 12;
			this.buttonRedraw.Text = " Redraw";
			this.buttonRedraw.Click += new System.EventHandler(this.buttonRedraw_Click);
			// 
			// DPUCDefectImgGalleryManyStep
			// 
			this.AutoScroll = true;
			this.Controls.Add(this.panel);
			this.Name = "DPUCDefectImgGalleryManyStep";
			this.Size = new System.Drawing.Size(624, 384);
			this.panel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

        public void Gallery(DACrux.Base.DPWafer[] stepSeq)
		{
			try
			{
                this.Controls.Clear();

				this.m_stepSeq = stepSeq;

				thread = new System.Threading.Thread(
					new System.Threading.ThreadStart(GalleryThread));
				thread.Name = "Defect image gallery";
				thread.Start();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		void GalleryThread()
		{
			GalleryDelegate gallery = new GalleryDelegate(GalleryD);
			this.Invoke(gallery);
		}

		void GalleryD()
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				for(int a = 0; a < m_stepSeq.Length; a++)
				{
					if(!threadRun) return;

					DPUCDefectImgGallery gallery = new DPUCDefectImgGallery();
					if(a == 0)
					{
						numericUpDown.Value = gallery.Height;
					}

					gallery.StepSeq = DACrux.Base.Convert.longParse(m_stepSeq[a].StepSeq);
					gallery.Dock = DockStyle.Top;
					this.Controls.Add(gallery);
					gallery.BringToFront();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void buttonRedraw_Click(object sender, System.EventArgs e)
		{
			try
			{
				int height = (int)numericUpDown.Value;
				string type = null;
				for(int a = 0; a < this.Controls.Count; a++)
				{
					type = this.Controls[a].GetType().Name;
					if(type.Equals("DPUCDefectImgGallery"))
					{
						this.Controls[a].Height = height;
					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
		}

		string [,] StepInfo(DataView dv)
		{
			DataTable dt = dv.Table;
			string [,] info = new string[dt.Rows.Count, 1];
			for(int a = 0; a < dt.Rows.Count; a++)
			{
				info[a, 0] = string.Format("{0}:{1}", dt.Rows[a][0], dt.Rows[a][1]);
			}
			return info;
		}

		private void buttonToExcel_Click(object sender, System.EventArgs e)
		{
			DACrux.Utility.ExcelUtil excel = null;
			try
			{
                excel = new DACrux.Utility.ExcelUtil();
				Microsoft.Office.Interop.Excel._Workbook workBook = excel.fnGetExcelWorkbook(true, 1);
				excel.fnAllCellResize(workBook, 1, 100, 22);

				int rowCnt = 0;
				int colCnt = 0;

				int curRow = 1;
				int curCol = 1;

				DPUCDefectImgGallery gallery = null;
				string[,] stepInfo = null;
				string tempImgFile = string.Format(@"{0}\{1}", Application.StartupPath, "temp");

                if (!System.IO.Directory.Exists(tempImgFile)) System.IO.Directory.CreateDirectory(tempImgFile);
				Image[] defectImg = null;
				for(int a = 0; a < this.Controls.Count; a++)
				{
					if(!this.Controls[a].GetType().Name.Equals("DPUCDefectImgGallery")) continue;
					gallery = (DPUCDefectImgGallery)this.Controls[a];

					stepInfo = StepInfo(gallery.StepInformation);
					rowCnt = gallery.StepInformation.Table.Rows.Count;
					colCnt = gallery.StepInformation.Table.Columns.Count;
					excel.fnSetValue(workBook, 1, curRow, 1, curRow + rowCnt - 1, 1, stepInfo);
					curRow = curRow + rowCnt;

					defectImg = gallery.DefectImages;
					curCol = 1;
                    string imgFile = string.Empty;
					for(int b = 0; b < defectImg.Length; b++)
					{
                        imgFile = tempImgFile + @"\" + b + ".jpg";
                        defectImg[b].Save(imgFile);
                        excel.fnSetImage(workBook, 1, imgFile, curRow, curCol++);
					}
					curRow += 6;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
			finally
			{
				DACrux.Utility.ExcelUtil.fnExcelProcessExit();
			}
		}
	}
}
