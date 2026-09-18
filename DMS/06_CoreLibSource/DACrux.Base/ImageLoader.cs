using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DACrux.Base
{
    public enum IMAGEFORMAT
    {
        TIFF, JPG, BMP, GIF
    }
    
    public class ImageLoader : IDisposable
    {
        private Image m_oImage;
        private System.Drawing.Imaging.FrameDimension m_FrmDim;

        private static Image.GetThumbnailImageAbort ImageCallback = new Image.GetThumbnailImageAbort(ImageCallbackMethod);
        
        public ImageLoader(string imageName)
        {
            ImageName = imageName;
            m_oImage = Image.FromFile(imageName);
            m_FrmDim = new System.Drawing.Imaging.FrameDimension(m_oImage.FrameDimensionsList[0]);
            Frames = m_oImage.GetFrameCount(m_FrmDim);
            ImageFormat = GetImageFormat(m_oImage);
        }

        public static ImageFormat GetImageFormat(Image img)
        {
            foreach (ImageFormat format in new ImageFormat[] { ImageFormat.Tiff, ImageFormat.Jpeg, ImageFormat.Bmp, ImageFormat.Gif, ImageFormat.Png })
            {
                if (img.RawFormat.Guid == format.Guid)
                    return format;
            }

            // default
            return ImageFormat.Jpeg;
        }

        /// <summary>
        /// Index로 Active된 Image를 지정한 이름으로 저장한다.
        /// </summary>
        public void SaveImage(string ImageName, int ImageIndex)
        {
            m_oImage.SelectActiveFrame(m_FrmDim, ImageIndex);
            m_oImage.Save(ImageName, ImageFormat);
        }

        /// <summary>
        /// Index로 Active된 Image와 Thumbnail을 저장한다.
        /// </summary>
        public void SaveImageAndThumbnail(string imageName, int imageIndex)
        {
            m_oImage.SelectActiveFrame(m_FrmDim, imageIndex);
            m_oImage.Save(imageName, ImageFormat);
            
            string thumbImageName = GetThumbnailImageName(imageName);
            SaveThumbnailImage(m_oImage, thumbImageName);
        }

        public void SaveThumbnailImage(string thumbnailImageName)
        {
            SaveThumbnailImage(m_oImage, thumbnailImageName);
        }

        /// <summary>
        /// Thumbnail 이미지와 원본 이미지를 target path 에 저장합니다.
        /// </summary>
        public static void SaveThumbnailImage(string imageFile, string thumbnailImageFile)
        {
            if (!File.Exists(imageFile))
                return;

            Image image = null;

            try
            {
                image = Image.FromFile(imageFile);
                SaveThumbnailImage(image, thumbnailImageFile);
            }
            catch
            {
                File.Delete(thumbnailImageFile);
                File.Copy(imageFile, thumbnailImageFile, true);
            }
            finally
            {
                if (image != null)
                    image.Dispose();
            }
        }

        /// <summary>
        /// Thumbnail 이미지와 원본 이미지를 target path 에 저장합니다.
        /// </summary>
        public static void SaveThumbnailImage(Image image, string thumbnailImageFile)
        {
            Image thumbImage = null;

            if (image == null)
                return;

            try
            {
                int width = (int)(image.Width / 2);
                int height = (int)(image.Height / 2);
                ImageFormat format = GetImageFormat(image);

                try
                {
                    thumbImage = image.GetThumbnailImage(width, height, ImageCallback, IntPtr.Zero);
                    thumbImage.Save(thumbnailImageFile);
                }
                catch
                {
                }
            }
            finally
            {
                if (thumbImage != null)
                    thumbImage.Dispose();
            }
        }

        /// <summary>
        /// Index로 Active된 Image를 지정한 이름으로 저장한다.
        /// </summary>
        public string SaveImage(int imageIndex)
        {
            if (Frames <= imageIndex)
            {
                throw new Exception(String.Format("'{0}' 이미지의 Frame Count는 {1} 이므로 인덱스 {2} 의 이미지를 가져올 수 없습니다.",
                    Path.GetFileName(ImageName), Frames, imageIndex));
            }

            string imageName = GetFrameImageName(ImageName, imageIndex + 1);

            m_oImage.SelectActiveFrame(m_FrmDim, imageIndex);
            m_oImage.Save(imageName, ImageFormat.Tiff);
            return imageName;
        }

        private static string GetFrameImageName(string imageFileName, int imageIndex)
        {
            return Path.Combine(Path.GetDirectoryName(imageFileName),
                String.Format("{0}_{1:0000}{2}", Path.GetFileNameWithoutExtension(imageFileName), imageIndex, Path.GetExtension(imageFileName)));
        }

        public static string GetThumeNail(int x, int y, string imagename)
        {
            string strThumbFile = string.Empty;
            string strImgFile = string.Empty;
            string strPath = string.Empty;
            Image m_oImageOne = null;
            Image oThumb = null;

            try
            {
                m_oImageOne = Image.FromFile(imagename); //큰 이미지 Load
                oThumb = m_oImageOne.GetThumbnailImage(x, y, ImageCallback, IntPtr.Zero);

                //FileName Make
                strImgFile = Path.GetFileNameWithoutExtension(imagename);
                strPath = Path.GetDirectoryName(imagename);

                if (!Directory.Exists(string.Format(@"{0}\Thumb", strPath)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\Thumb", strPath));
                }

                strThumbFile = string.Format(@"{0}\Thumb\{1}_thumb.jpg", strPath, strImgFile);

                try
                {
                    oThumb.Save(strThumbFile, ImageFormat.Jpeg);
                }
                catch (Exception)
                {
                    FileInfo ThumbFile = new FileInfo(strThumbFile);
                    if (ThumbFile.Exists)
                    {
                        StreamReader oReader = new StreamReader(ThumbFile.FullName, System.Text.Encoding.Default);
                        oThumb.Save(oReader.BaseStream, ImageFormat.Jpeg);
                    }
                }

                return strThumbFile;
            }
            catch (Exception) { return ""; }
            finally
            {
                if (m_oImageOne != null)
                    m_oImageOne.Dispose();

                if (oThumb != null)
                    oThumb.Dispose();
            }
        }

        /// <summary>
        /// Thumbnail 이미지와 원본 이미지를 target path 에 저장합니다.
        /// </summary>
        public static bool SaveThumbnailAndOriginalFile(string imageName, string targetPath)
        {
            Image oImage = null;

            try
            {
                // 이미지 파일에 문제가 있는 경우 Thumbnail은 만들지 않고 원본을 백업으로 옮길 수 있도록 한다. 2019.10.14 Taihi,Kim.
                // 이렇게 처리하지 않으면 해당 Wafer의 다른 이미지들도 조회를 할 수 없는 문제가 있음.
                oImage = Image.FromFile(imageName);
            }
            catch
            {
                string backupFile = Path.Combine(targetPath, Path.GetFileName(imageName));
                File.Delete(backupFile);
                File.Copy(imageName, backupFile, true);
                return false;
            }

            try
            {
                int width = (int)(oImage.Width / 2);
                int height = (int)(oImage.Height / 2);
                ImageFormat format = GetImageFormat(oImage);

                Image oThumbImage = null;
                string targetImageName = Path.GetFileName(imageName);
                string thumbImageName = GetThumbnailImageName(targetImageName);

                try
                {
                    oThumbImage = oImage.GetThumbnailImage(width, height, ImageCallback, IntPtr.Zero);
                    oThumbImage.Save(Combine(targetPath, thumbImageName), format);
                }
                catch
                {
                    // thumbnail 생성 실패할 경우 원본 이미지를 저장
                    oImage.Save(Combine(targetPath, thumbImageName), format);
                }

                oImage.Save(Combine(targetPath, targetImageName), format);
                return true;
            }
            finally
            {
                if (oImage != null)
                    oImage.Dispose();
            }
        }

        public static string GetThumbnailImageName(string fileName)
        {
            string name = Path.GetFileName(fileName);
            name = "thumb_" + name;
            return Path.Combine(Path.GetDirectoryName(fileName), name);
        }

        public static string Combine(string path, string name)
        {
            return string.Format(@"{0}\{1}", path, name);
        }

        public string GetFrameImageName(int imageSeq)
        {
            return GetFrameImageName(ImageName, imageSeq);
        }

        private static bool ImageCallbackMethod()
        {
            return false;
        }

        public string GetThumbnail(int x, int y, string imagename, ref Byte[] ThmByte)
        {
            string strThumbFile = string.Empty;
            string strImgFile = string.Empty;
            string strPath = string.Empty;
            try
            {
                Image m_oImageOne = Image.FromFile(imagename); //큰 이미지 Load
                Image oThumb = m_oImageOne.GetThumbnailImage(x, y, ImageCallback, IntPtr.Zero);

                //FileName Make
                strImgFile = Path.GetFileNameWithoutExtension(imagename);
                strPath = Path.GetDirectoryName(imagename);

                if (!Directory.Exists(string.Format(@"{0}\Thumb", strPath)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\Thumb", strPath));
                }

                strThumbFile = string.Format(@"{0}\Thumb\{1}_thumb.jpg", strPath, strImgFile);


                oThumb.Save(strThumbFile, ImageFormat.Jpeg);

                FileStream fs = new FileStream(strThumbFile, FileMode.Open, FileAccess.Read);
                BinaryReader oBr = new BinaryReader(fs);
                ThmByte = oBr.ReadBytes((int)fs.Length);

                oBr.Close();
                fs.Close();
                oThumb.Dispose();
                return strThumbFile;
                //File.Delete(strThumbFile);
                //Directory.Delete(string.Format(@"{0}\Thumb",strPath));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        private string GetExtension()
        {
            if (ImageFormat == ImageFormat.Jpeg)
                return ".jpg";
            else
                return "." + ImageFormat.ToString().ToLower().Substring(0, 3);
        }

        public void Dispose()
        {
            if (m_oImage != null)
                m_oImage.Dispose();
        }

        /// <summary>
        /// Image 갯수
        /// </summary>
        public int Frames
        {
            get;
            private set;
        }

        public string ImageName
        {
            get;
            private set;
        }

        public ImageFormat ImageFormat
        {
            get;
            private set;
        }
    }
}
