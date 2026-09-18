using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace DACruxV5
{
    static class Util
    {
        public static Image GetImage(Image source, int size)
        {
            if (source == null)
                return null;

            return new Bitmap(source, size, size);
        }

        public static byte[] ImageToBytes(System.Drawing.Image image)
        {
            return (new ImageConverter()).ConvertTo(image, typeof(byte[])) as byte[];
        }

        public static void GetImageFormat(System.Drawing.Image img, out ImageFormat format, out string ext)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                format = ImageFormat.Jpeg;
            else if (img.RawFormat.Equals(ImageFormat.Bmp))
                format = ImageFormat.Bmp;
            else if (img.RawFormat.Equals(ImageFormat.Emf))
                format = ImageFormat.Emf;
            else if (img.RawFormat.Equals(ImageFormat.Exif))
                format = ImageFormat.Exif;
            else if (img.RawFormat.Equals(ImageFormat.Gif))
                format = ImageFormat.Gif;
            else if (img.RawFormat.Equals(ImageFormat.Icon))
                format = ImageFormat.Icon;
            else if (img.RawFormat.Equals(ImageFormat.Tiff))
                format = ImageFormat.Tiff;
            else
                format = ImageFormat.Png;

            ext = GetImageExt(format);
        }

        private static string GetImageExt(ImageFormat format)
        {
            string ext = format.ToString();

            if (format == ImageFormat.Icon)
                ext = "ico";
            else if (format == ImageFormat.Jpeg)
                ext = "jpg";
            else if (format == ImageFormat.Tiff)
                ext = "tif";

            return "." + ext.ToLower();
        }

        public static string BytesToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 3);

            for (int i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 1)
                    sb.AppendFormat("{0:X2} ", bytes[i]);
                else
                    sb.AppendFormat("{0:X2}", bytes[i]);
            }

            return sb.ToString();
        }

        public static byte[] StringToBytes(string val)
        {
            if (String.IsNullOrEmpty(val))
                return null;

            try
            {
                string[] sVal = val.Split(' ');
                byte[] body = new byte[sVal.Length];
                for (int a = 0; a < body.Length; a++) body[a] = byte.Parse(sVal[a], System.Globalization.NumberStyles.AllowHexSpecifier);
                return body;
            }
            catch
            {
                return null;
            }
        }

        public static Image StringToImage(string val)
        {
            if (String.IsNullOrEmpty(val))
                return null;

            return BytesToImage(StringToBytes(val));
        }

        public static Image BytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
