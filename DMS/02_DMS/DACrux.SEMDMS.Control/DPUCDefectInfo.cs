using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DACrux.SEMDMS.Control
{
    public partial class DPUCDefectInfo : UserControl
    {
        private Image DefectImg;
        // Image.FromStream 으로 만든 이미지는 원본 스트림이 살아 있어야 하므로 이미지와 함께 보관했다가 해제한다.
        private MemoryStream m_ImageStream;
        private string m_ImageFTPPath = string.Empty;

        public DPUCDefectInfo()
        {
            InitializeComponent();
        }

        public DPUCDefectInfo(
            string strDefectNo,
            string strImage,
            string strXindex,
            string strYindex,
            string strClass,
            string strCluster,
            string strRelX,
            string strRelY,
            string strXSize,
            string strYSize
            )
        {
            InitializeComponent();

            try
            {
                lbDefectNo.Text = strDefectNo;
                lbXindex.Text = strXindex;
                lbYindex.Text = strYindex;
                lbClass.Text = strClass;
                lbCluster.Text = strCluster;
                lbRelX.Text = strRelX;
                lbRelY.Text = strRelY;
                lbXSize.Text = strXSize;
                lbYSize.Text = strYSize;

                m_ImageFTPPath = strImage;

                imageControl1.ImageControlDraw(
                    null,
                    DefectimageList.Images[0]
                    );

                // 이미지 다운로드만 백그라운드 스레드에서 한다. 앱 종료를 막지 않도록 IsBackground 로 둔다.
                // 화면 표시는 ShowImage 에서 UI 스레드로 넘긴다. (2026-10-08)
                System.Threading.Thread.Sleep(10);
                System.Threading.Thread trd = new System.Threading.Thread(new System.Threading.ThreadStart(DrawImage));
                trd.IsBackground = true;
                trd.Start();
            }
            catch
            {
                imageControl1.ImageControlDraw(
                    null,
                    DefectimageList.Images[1]
                    );
            }
        }

        private void DrawImage()
        {
            MemoryStream ms = null;
            FileInfo oImage = null;
            try
            {
                oImage = DefectMapDraw.fnFTPImageDownload(m_ImageFTPPath);
                if (oImage == null)
                    return;

                if (oImage.Exists == false)
                    return;

                ms = new MemoryStream();
                using (FileStream fs = File.OpenRead(oImage.FullName))
                {
                    fs.CopyTo(ms);
                }

                Image image = Image.FromStream(ms);
                ShowImage(image, ms);
                ms = null; // 소유권을 ShowImage 로 넘겼다.
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Defect Image 표시 실패 ({0}): {1}", m_ImageFTPPath, ex.Message);
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();

                ms = null;

                //Image File 은 사용후 삭제 한다.
                try
                {
                    if (oImage != null && oImage.Exists)
                        oImage.Delete();
                }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }
        }

        /// <summary>
        /// 다운로드 스레드에서 만든 이미지를 UI 스레드에서 표시한다.
        /// 컨트롤이 이미 해제되었으면(한 번도 표시되지 않은 이미지) 이미지 / 스트림을 바로 해제한다.
        /// 표시된 이미지는 이미지 팝업이 계속 그릴 수 있으므로 컨트롤 해제 때 직접 해제하지 않고 GC 에 맡긴다. (2026-10-08)
        /// </summary>
        private void ShowImage(Image image, MemoryStream stream)
        {
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action<Image, MemoryStream>(ShowImage), image, stream);
                }
                catch (InvalidOperationException)
                {
                    // 창 핸들이 이미 사라졌다(목록이 지워지는 중).
                    image.Dispose();
                    stream.Dispose();
                }
                return;
            }

            if (IsDisposed)
            {
                image.Dispose();
                stream.Dispose();
                return;
            }

            DefectImg = image;
            m_ImageStream = stream;

            imageControl1.ImageControlDraw(
                null,
                DefectImg
                );
        }


    }
}
