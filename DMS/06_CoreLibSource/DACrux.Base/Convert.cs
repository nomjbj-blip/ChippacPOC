using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DACrux.Base
{
    public static class Convert
    {
        public static object Serialize(object InTag)
        {
            IFormatter formatter = null;
            MemoryStream stream = null;
            try
            {
                formatter = new BinaryFormatter();
                stream = new MemoryStream();

                formatter.Serialize(stream, InTag);
                stream.Position = 0;

                byte[] buffer = new byte[stream.Length];
                buffer = stream.GetBuffer();
                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        public static Object Deserialize(Object InObj)
        {
            IFormatter formatter = null;
            MemoryStream tstream = null;
            object obj = null;
            try
            {
                // byte 배열을 structure로 복원
                formatter = new BinaryFormatter();
                obj = new object();
                tstream = new MemoryStream((byte[])InObj);
                obj = formatter.Deserialize(tstream);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (tstream != null)
                    tstream.Dispose();
            }
        }

        public static byte[] ConvertToByte(string val)
        {
            string[] sVal = val.Split(' ');
            byte[] body = new byte[sVal.Length];
            for (int a = 0; a < body.Length; a++)
                if (sVal[a] != null && sVal[a].Length > 0)
                    body[a] = (byte)int.Parse(sVal[a]);
            return body;
        }

        public static byte[] Convert16ToByte(string val)
        {
            string[] sVal = val.Split(' ');
            byte[] body = new byte[sVal.Length];
            for (int a = 0; a < body.Length; a++)
            {
                if (sVal[a] != null && sVal[a].Length > 0)
                    body[a] = (byte)int.Parse(sVal[a], System.Globalization.NumberStyles.HexNumber);
            }
            return body;
        }

        public static Image StringToImage(string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;
            else
                return ByteArrayToImage(Convert16ToByte(val));
        }

        public static Image ByteArrayToImage(byte[] b)
        {
            try
            {
                ImageConverter ic = new ImageConverter();
                Image img = (Image)ic.ConvertFrom(b);
                return img;
            }
            catch
            {
                return null;
            }
        }

        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter ic = new ImageConverter();
            byte[] b = (byte[])ic.ConvertTo(img, typeof(byte[]));
            return b;
        }

        public static int intParse(string strValue, int iDefault = 0)
        {
            int iResult = 0;

            if (int.TryParse(strValue, out iResult) == false)
                iResult = iDefault;

            return iResult;
        }

        public static double doubleParse(string strValue, double dDefault = 0)
        {
            double dResult = 0;

            if (double.TryParse(strValue, out dResult) == false)
                dResult = dDefault;

            return dResult;
        }

        public static long longParse(string strValue, long lDefault = 0)
        {
            long lResult = 0;

            if (long.TryParse(strValue, out lResult) == false)
                lResult = lDefault;

            return lResult;
        }
    }
}
