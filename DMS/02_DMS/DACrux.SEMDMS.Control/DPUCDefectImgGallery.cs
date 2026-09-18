using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// DPUCDefectImgGallery에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCDefectImgGallery : System.Windows.Forms.UserControl
    {
        long stepSeq = -1;
        private FarPoint.Win.Spread.FpSpread fpSpread;
        private FarPoint.Win.Spread.SheetView fpSpread_Sheet;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Splitter splitter1;
        private System.ComponentModel.IContainer components;

        public DPUCDefectImgGallery()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();

            // TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

        }

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드
        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.fpSpread = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Sheet = new FarPoint.Win.Spread.SheetView();
            this.listView = new System.Windows.Forms.ListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // fpSpread
            // 
            this.fpSpread.AccessibleDescription = "";
            this.fpSpread.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpread.Dock = System.Windows.Forms.DockStyle.Left;
            this.fpSpread.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread.Location = new System.Drawing.Point(0, 0);
            this.fpSpread.Name = "fpSpread";
            this.fpSpread.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpread.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Sheet});
            this.fpSpread.Size = new System.Drawing.Size(328, 419);
            this.fpSpread.TabIndex = 0;
            this.fpSpread.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpSpread_Sheet
            // 
            this.fpSpread_Sheet.Reset();
            fpSpread_Sheet.SheetName = "Sheet1";
            fpSpread_Sheet.ColumnHeader.RowCount = 0;
            fpSpread_Sheet.RowHeader.ColumnCount = 0;
            // 
            // listView
            // 
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.LargeImageList = this.imageList;
            this.listView.Location = new System.Drawing.Point(334, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(346, 419);
            this.listView.TabIndex = 1;
            this.listView.UseCompatibleStateImageBehavior = false;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList.ImageSize = new System.Drawing.Size(96, 96);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(328, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 419);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // DPUCDefectImgGallery
            // 
            this.Controls.Add(this.listView);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.fpSpread);
            this.Name = "DPUCDefectImgGallery";
            this.Size = new System.Drawing.Size(680, 419);
            this.Load += new System.EventHandler(this.DPUCDefectImgGallery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        public long StepSeq
        {
            get { return this.stepSeq; }
            set { this.stepSeq = value; }
        }

        public Image[] DefectImages
        {
            get
            {
                Image[] img = new Image[this.imageList.Images.Count];
                for (int a = 0; a < this.imageList.Images.Count; a++) img[a] = this.imageList.Images[a];
                return img;
            }
        }

        public DataView StepInformation
        {
            get
            {
                return this.fpSpread_Sheet.GetDataView(true);
            }
        }

        void StepInfo()
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            DataTable dtInfo = null;
            DataTable dtBind = null;
            try
            {
                oDMapAnalysis = new DACrux.SEMDMS.RO.DefectMapAnalysis();
                dtInfo = oDMapAnalysis.GetImageGallery(new long[] { this.stepSeq });

                dtBind = new DataTable("INFO");
                dtBind.Columns.Add("ITEM", typeof(string));
                dtBind.Columns.Add("VALUE", typeof(string));
                for (int a = 0; a < dtInfo.Columns.Count; a++)
                {
                    if (dtInfo.Columns[a].ColumnName.Equals("STEP_SEQ")
                        || dtInfo.Columns[a].ColumnName.Equals("WAFER_SEQ")
                        || dtInfo.Columns[a].ColumnName.Equals("SETUP_SEQ")) continue;

                    object[] obj = new object[2];
                    obj[0] = dtInfo.Columns[a].ColumnName;
                    obj[1] = string.Format("{0}", dtInfo.Rows[0][dtInfo.Columns[a].ColumnName]);
                    dtBind.Rows.Add(obj);
                }

                fpSpread_Sheet.DataSource = dtBind;
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread_Sheet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDMapAnalysis = null;
                if (dtInfo != null) dtInfo.Dispose();
                dtInfo = null;
                if (dtBind != null) dtBind.Dispose();
                dtBind = null;
            }
        }

        void ImageAdd()
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            DataTable dtImage = null;

            FileInfo oImage = null;

            string ServerIP = string.Empty;

            try
            {
                oDMapAnalysis = new DACrux.SEMDMS.RO.DefectMapAnalysis();
                dtImage = oDMapAnalysis.GetDefectImageInfo(this.stepSeq);

                string label;
                for (int a = 0; a < dtImage.Rows.Count; a++)
                {

                    if (string.IsNullOrEmpty(dtImage.Rows[a]["THUMB_PATH"].ToString()) == false)
                        oImage = DefectMapDraw.fnFTPImageDownload(string.Format("{0}/{1}", dtImage.Rows[a]["THUMB_PATH"].ToString().Trim(), dtImage.Rows[a]["THUMB_FILENAME"]).Trim());
                    else
                        oImage = DefectMapDraw.fnFTPImageDownload(string.Format("{0}/{1}", dtImage.Rows[a]["IMAGE_PATH"].ToString().Trim(), dtImage.Rows[a]["IMAGE_FILENAME"]).Trim());

                    if (oImage != null && oImage.Exists)
                    {
                        label = string.Format("{0}_{1}_{2}",
                            dtImage.Rows[a]["XINDEX"], dtImage.Rows[a]["YINDEX"],
                            dtImage.Rows[a]["DEFECTID"]);
                        ImageAdd(oImage, label);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDMapAnalysis = null;
                if (dtImage != null) dtImage.Dispose();
                dtImage = null;
            }
        }

        private void DPUCDefectImgGallery_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;

            try
            {
                ImageAdd();
                StepInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.Source);
            }
        }

        void ImageAdd(string imageURL, string label)
        {
            System.Net.WebClient myWebClient = null;
            System.IO.Stream myStream = null;
            Image oImg = null;

            try
            {
                myWebClient = new System.Net.WebClient();
                myStream = myWebClient.OpenRead(imageURL);
                oImg = Image.FromStream(myStream);
                imageList.Images.Add(oImg);
                listView.Items.Add(label, imageList.Images.Count - 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (myStream != null) myStream.Close();
                myStream = null;
                if (myWebClient != null) myWebClient.Dispose();
                myWebClient = null;
                if (oImg != null) oImg.Dispose();
                oImg = null;
            }
        }

        void ImageAdd(FileInfo oFile, string label)
        {
            System.IO.Stream myStream = null;
            Image oImg = null;

            try
            {
                myStream = oFile.OpenRead();
                oImg = Image.FromStream(myStream);
                imageList.Images.Add(oImg);
                listView.Items.Add(label, imageList.Images.Count - 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (myStream != null) myStream.Close();
                myStream = null;
                if (oImg != null) oImg.Dispose();
                oImg = null;
            }
        }
    }
}
