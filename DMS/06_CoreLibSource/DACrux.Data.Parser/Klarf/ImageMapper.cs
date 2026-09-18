using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Base;
using System.IO;

namespace DACrux.Data.Parser.Klarf
{
    class ImageMapper
    {
        public static string[] SEPARATOR_ARR = new string[] { "TiffFilename" , "TiffFileName" };
        public static string DATA_KEY = "DefectList";
        public static string[] DATA_KEY_ARR = new string[] { "DefectList", "Defectlist" };
        public static string SLOT = "Slot";

        public static ImageManagerList Mapping(ParserKlarf parser, string textData)
        {
            ImageManagerList managerList = new ImageManagerList();

            if (String.IsNullOrEmpty(textData))
                return null;

            if (!Contains(textData, SEPARATOR_ARR))
                return managerList;

            string[] splitArray = textData.Split(SEPARATOR_ARR, StringSplitOptions.RemoveEmptyEntries);
            
            Wafer currWafer = null;

            foreach (string splitText in splitArray)
            {
                if (!Contains(splitText, DATA_KEY_ARR))
                    continue;

                string imageFileName = ParserBase.GetValue(splitText, null, null, ";").Trim().Trim('\"');
                managerList.Add(Path.Combine(parser.DirectoryName, imageFileName));

                foreach (string data in ParserBase.Cut(splitText, SLOT))
                {
                    if (data.Contains(SLOT))
                        currWafer = parser.Wafers.GetWaferBySlot(GetSlot(data));

                    if (!Contains(data, DATA_KEY_ARR))
                        continue;

                    if (currWafer == null)
                        throw new Exception("currWafer 정보가 null 입니다.");

                    string defectText = ParserBase.GetValue(data, GetKey(data, DATA_KEY_ARR));

                    DefectList defectList = DefectDataHelper.GetDefectList(currWafer.DefectHeaders, new string[] { defectText });

                    foreach (Defect tmpDefect in defectList)
                    {
                        int j = 0;

                        int imageCount = Math.Min(tmpDefect.IMAGECOUNT, tmpDefect.IMAGELIST);

                        for (int i = 0; i < imageCount; i++)
                        {
                            Defect defect = currWafer.DefectList.GetDefect(tmpDefect.DEFECTID);

                            if (defect == null)
                                throw new Exception(String.Format("해당하는 DEFECTID {0} 를 찾을 수 없습니다. File={1}", tmpDefect.DEFECTID, parser.FileName));

                            DefectImage tmpImage = tmpDefect.Images.GetDefectImage(i);

                            DefectImage image;

                            while (true)
                            {
                                image = defect.Images.GetDefectImage(j);

                                if (image != null && !String.IsNullOrEmpty(image.IMAGEPATH))
                                    j++;
                                else
                                    break;
                            }

                            if (image == null)
                                break;

                            ImageManager tif = managerList[imageFileName];

                            // 이미지 파일이 없거나 일반적인 이미지 파일인 경우
                            if (tif == null || !tif.HasFrameImage)
                            {
                                image.IMAGEPATH = imageFileName;
                                image.IMAGESEQ = tmpImage.IMAGESEQ;
                                break;
                            }
                            else // 여러 이미지를 가진 TIF 파일인 경우
                            {
                                // 이미지 SEQ가 비정상적으로 1로 올라오는 경우 다음 이미지 처리를 위해 SEQ를 증가시킨다. (1,2,3,4,5,1,7,8 같은 케이스가 있음)
                                if (tmpImage.IMAGESEQ == 1 && tif.GetNextSeq() < tif.FrameCount)
                                    image.IMAGESEQ = tif.GetNextSeq();
                                else
                                    image.IMAGESEQ = tmpImage.IMAGESEQ;

                                // FRAME 이미지를 저장합니다.
                                image.IMAGEPATH = Path.GetFileName(tif.GetImageFileName(image.IMAGESEQ));

                                if (!tif.FrameDictionary.ContainsKey(image.IMAGESEQ))
                                    tif.FrameDictionary.Add(image.IMAGESEQ, image.IMAGEPATH);

                                j++;
                            }
                        }
                    }
                }
            }

            return managerList;
        }

        private static int GetSlot(string data)
        {
            int slot;

            if (Int32.TryParse(ParserBase.GetValue(data, SLOT, " ", ";"), out slot))
                return slot;

            return 0;
        }

        private static bool Contains(string text, string[] arr)
        {
            foreach (string val in arr)
            {
                if (text.Contains(val))
                    return true;
            }

            return false;
        }

        private static string GetKey(string text, string[] arr)
        {
            foreach (string val in arr)
            {
                if (text.Contains(val))
                    return val;
            }

            return null;
        }
    }

    public class ImageManagerList : IDisposable
    {
        public ImageManagerList()
        {
            SingleList = new List<ImageManager>();
            MultiList = new List<ImageManager>();
        }

        public void Dispose()
        {
            foreach (ImageManager tif in MultiList)
                tif.Dispose();

            foreach (ImageManager tif in SingleList)
                tif.Dispose();
        }

        public void Add(string fileName)
        {
            if (!File.Exists(fileName))
                return;

            if (this[fileName] != null)
                return;

            ImageManager tif = new ImageManager(fileName);

            if (tif.HasFrameImage)
                MultiList.Add(tif);
            else
                SingleList.Add(tif);
        }

        public bool HasData()
        {
            return MultiList.Count + SingleList.Count > 0;
        }

        public ImageManager this[string fileName]
        {
            get
            {
                foreach (ImageManager tif in MultiList)
                {
                    if (tif.ImageName == fileName)
                        return tif;
                }

                foreach (ImageManager tif in SingleList)
                {
                    if (tif.ImageName == fileName)
                        return tif;
                }

                return null;
            }
        }

        public List<ImageManager> SingleList
        {
            get;
            private set;
        }

        public List<ImageManager> MultiList
        {
            get;
            private set;
        }
    }

    public class ImageManager : IDisposable
    {
        private ImageLoader m_imageLoader;

        public ImageManager(string imageFileName)
        {
            FrameDictionary = new Dictionary<int, string>();
            ImageName = Path.GetFileName(imageFileName);
            ImageFullName = imageFileName;

            try
            {
                m_imageLoader = new ImageLoader(imageFileName);
                HasFrameImage = m_imageLoader.Frames > 1;
            }
            catch
            {
                m_imageLoader = null;
            }
        }

        public string SaveThumbnailImage(string targetDirectory)
        {
            if (m_imageLoader == null)
                return null;

            string thumbnailImage = ImageLoader.GetThumbnailImageName(ImageName);
            thumbnailImage = Path.Combine(targetDirectory, Path.GetFileName(thumbnailImage));
            m_imageLoader.SaveThumbnailImage(thumbnailImage);

            return thumbnailImage;
        }

        public string SaveFrameThumbnailImage(int imageSeq, string savePath)
        {
            if (m_imageLoader == null)
                return null;

            string frameImageName = m_imageLoader.GetFrameImageName(imageSeq);
            string thumbnailImage = ImageLoader.GetThumbnailImageName(frameImageName);
            thumbnailImage = Path.Combine(savePath, Path.GetFileName(thumbnailImage));

            ImageLoader.SaveThumbnailImage(frameImageName, thumbnailImage);
            
            return thumbnailImage;
        }

        public string SaveFrameImageAndThumbnail(int imageSeq, string savePath)
        {
            if (m_imageLoader == null)
                return null;

            string frameImageName = m_imageLoader.GetFrameImageName(imageSeq);
            frameImageName = Path.Combine(savePath, Path.GetFileName(frameImageName));

            m_imageLoader.SaveImageAndThumbnail(frameImageName, imageSeq - 1);
            return frameImageName;
        }

        public int GetNextSeq()
        {
            if (FrameDictionary.Count == 0)
                return 1;

            int[] arr = FrameDictionary.Keys.ToArray();

            bool decrease = arr.Length > 2 && (arr[arr.Length - 2] - arr[arr.Length - 1] > 0);
            return arr[arr.Length - 1] + (decrease ? -1 : 1); 
        }

        internal string GetImageFileName(int imageSeq)
        {
            if (m_imageLoader != null)
                return m_imageLoader.GetFrameImageName(imageSeq);

            return null;
        }

        public void Dispose()
        {
            if (m_imageLoader != null)
            {
                m_imageLoader.Dispose();
                m_imageLoader = null;
            }
        }

        public string ImageName
        {
            get;
            private set;
        }

        public string ImageFullName
        {
            get;
            private set;
        }

        public bool HasFrameImage
        {
            get;
            private set;
        }

        public int FrameCount
        {
            get
            {
                if (m_imageLoader != null)
                    return m_imageLoader.Frames;

                return 1;
            }
        }

        public Dictionary<int, string> FrameDictionary
        {
            get;
            private set;
        }
    }
}
