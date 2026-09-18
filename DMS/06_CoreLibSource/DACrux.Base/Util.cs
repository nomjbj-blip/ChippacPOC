using System;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

namespace DACrux.Base
{
    public static class Util
    {
        private static char[] m_chrs = new char[] {'0','1','2','3','4','5','6','7','8','9'
                                    ,'A','B','C','D','E','F','G','H','I','J'
                                    ,'K','L','M','N','O','P','Q','R','S','T'
                                    ,'U','V','W','X','Y','Z'
                                    ,')','!','@','#','$','%','^','&','*','('};

        public static Color Int2Color(int value)
        {
            int r, g, b;
            try
            {
                r = (value >> 16);
                g = (value >> 8) - (r * 256);
                b = value - (r * 65536) - (g * 256);
                return Color.FromArgb(r, g, b);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int Color2Int(Color value)
        {
            int iColorValue = 0;
            try
            {
                iColorValue += value.R << 16;
                iColorValue += value.G << 8;
                iColorValue += value.B;
                return iColorValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int NumericBin(char str)
        {
            try
            {
                return Array.IndexOf(m_chrs, str);
            }
            catch
            {
                return 0;
            }
        }

        public static string StringBin(int i)
        {
            try
            {
                return m_chrs[i].ToString();
            }
            catch
            {
                return "?";
            }
        }

        public static byte[] StringToByte(string val)
        {
            string[] sVal = val.Split(' ');
            byte[] body = new byte[sVal.Length];
            for (int a = 0; a < body.Length; a++) body[a] = (byte)DACrux.Base.Convert.intParse(sVal[a]);
            return body;
        }

        #region SendMail

        /// <summary>
        /// 메일을 전송합니다.
        /// </summary>
        public static string[] SendMail(string mailServer, int port, string fromMail, string[] toMailArr, string subject, string body)
        {
            using (SmtpClient smtp = new SmtpClient(mailServer, port))
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromMail);

                foreach (string toMail in toMailArr)
                {
                    if (String.IsNullOrEmpty(toMail))
                        continue;

                    string addr = toMail.ToLower();

                    if (!addr.Contains("@"))
                        continue;

                    try
                    {
                        mail.To.Add(toMail);
                    }
                    catch
                    {
                    }
                }

                mail.Subject = subject;
                mail.Body = body;

                smtp.Send(mail);

                string[] results = new string[mail.To.Count];

                for (int i = 0; i < mail.To.Count; i++)
                    results[i] = mail.To[i].Address;

                return results;
            }
        }

        static private void SendMail(string strFileName, DateTime dt, string strErrMessage)
        {
            // DACrux.Common.RO.UserGroup oUserGroup = null;
            string strMailServer = "smtp.samsung.com";
            int iMailPort = 25;
            DataTable dtSecUsr = new DataTable();
            string strMailSenderID = "louis0723.lim@miracom.co.kr";
            string strMailSenderPW = "louis0723.lim";
            string strMailSenderName = "ehdco8593";
            string strMailSubject = "Engineering UI에서 Error가 보고 되었습니다";
            string strMailContents = string.Empty;
            StringBuilder strMailBody;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());

            //string strMailFlag = "N";

            //string[] strEmailList = null;
            try
            {
                //oUserGroup = new DACrux.Common.RO.UserGroup();
                //dtSecUsr = oUserGroup.LoadSecurityUser().Tables[0];
                //dtSecUsr = oUserGroup.SearchGroupUser("ADMIN").Tables[0];
                strMailBody = new StringBuilder();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(strMailServer);
                mail.From = new MailAddress(strMailSenderID, strMailSenderName, System.Text.Encoding.UTF8);
                mail.To.Add("louis0723.lim@miracom.co.kr");
                mail.Subject = strMailSubject;
                strMailBody = strMailBody.AppendFormat("Event Time      : {0} \n", dt.ToString());
                strMailBody = strMailBody.AppendFormat("Computer Name   : {0} \n", Environment.MachineName);
                strMailBody = strMailBody.AppendFormat("Computer IP     : {0} \n", ipEntry.AddressList[4].ToString());
                strMailBody = strMailBody.AppendFormat("Windows Version : {0} \n", Environment.OSVersion);
                //strMailBody = strMailBody.AppendFormat("Program Name    : {0} \n", Application.ProductName);
                //strMailBody = strMailBody.AppendFormat("Server Version  : 2011072909\n");
                //strMailBody = strMailBody.AppendFormat("Client Version  : 2011072909\n");
                strMailBody = strMailBody.AppendFormat("User ID         :  {0}\n", DACrux.Base.GlobalVariable.UserID);
                //strMailBody = strMailBody.AppendFormat("User Name       : {0} \n", DACrux.Base.Regedit.Server);
                //strMailBody = strMailBody.AppendFormat("User Group      : QA_GERNERAL\n");
                strMailBody = strMailBody.AppendFormat("Server Name     : DAServer\n");
                //strMailBody = strMailBody.AppendFormat("Site ID         : HMMS\n");
                strMailBody = strMailBody.AppendFormat("Server Address  : {0}\n", DACrux.Base.GlobalVariable.ServerIP);
                //strMailBody = strMailBody.AppendFormat("OI Name         : HMC0570\n");
                //strMailBody = strMailBody.AppendFormat("Form Name         : {0} \n", Application.OpenForms[0].Name);
                strMailBody = strMailBody.AppendFormat("Error Message     : {0} \n", strErrMessage);
                strMailBody = strMailBody.AppendFormat("ClipBoard Data  : \n");
                mail.Body = strMailBody.ToString();
                System.Net.Mail.Attachment a;
                a = new Attachment(strFileName);
                mail.Attachments.Add(a);
                SmtpServer.Port = iMailPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(strMailSenderID, strMailSenderPW);

                //SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public static string[] ASEMailSend(string sSubject, string sBody, string[] sAddressee, string sAttachFile = "")
        {
            try
            {
                System.Collections.Generic.List<string> lsSendFailUser = new System.Collections.Generic.List<string>();

                for (int i = 0; i < sAddressee.Length; i++)
                {
                    try
                    {
                        ASEMailSend(sSubject, sBody, sAddressee[i], sAttachFile);
                    }
                    catch (Exception ex)
                    {
                        lsSendFailUser.Add(string.Format("{0} : {1}", sAddressee[i], ex.Message));
                    }
                }
                return lsSendFailUser.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ASEMailMultiSend(string sSubject, string sBody, string[] sAddressee, string sAttachFile = "")
        {
            try
            {
                try
                {
                    ASEMailSend2(sSubject, sBody, sAddressee, sAttachFile);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ASEMailSend(string sSubject, string sBody, string sAddressee, string sAttachFile = "")
        {
            string strMailServer = "asekrmail2.asekr.com";
            string strMailSenderID = "Alliance@asekr.com";
            string strMailSenderPW = "Alliance";
            string strMailSenderName = "Alliance";

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(strMailServer);
                mail.From = new MailAddress(strMailSenderID, strMailSenderName, System.Text.Encoding.UTF8);
                mail.To.Add(sAddressee);
                mail.Subject = sSubject;
                mail.Body = sBody;

                if (!string.IsNullOrEmpty(sAttachFile) && System.IO.File.Exists(sAttachFile))
                {
                    System.Net.Mail.Attachment a;
                    a = new Attachment(sAttachFile);
                    mail.Attachments.Add(a);
                }
                //SmtpServer.Port = iMailPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(strMailSenderID, strMailSenderPW);

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ASEMailSend2(string sSubject, string sBody, string[] sAddressee, string sAttachFile = "")
        {
            string strMailServer = "asekrmail2.asekr.com";
            //int iMailPort = 25;
            string strMailSenderID = "Alliance@asekr.com";
            string strMailSenderPW = "Alliance";
            string strMailSenderName = "Alliance";

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(strMailServer);
                mail.From = new MailAddress(strMailSenderID, strMailSenderName, System.Text.Encoding.UTF8);
                for (int i = 0; i < sAddressee.Length; i++)
                    mail.To.Add(sAddressee[i]);
                mail.Subject = sSubject;
                mail.Body = sBody;

                if (!string.IsNullOrEmpty(sAttachFile) && System.IO.File.Exists(sAttachFile))
                {
                    System.Net.Mail.Attachment a;
                    a = new Attachment(sAttachFile);
                    mail.Attachments.Add(a);
                }
                //SmtpServer.Port = iMailPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(strMailSenderID, strMailSenderPW);

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ScreenCapture(int intBitWidth, int intBitHeight, Point ptSource, DateTime dt, string strErrMessage)
        {
            try
            {
                Bitmap bitmap = new System.Drawing.Bitmap(intBitWidth, intBitHeight);
                Graphics g = Graphics.FromImage(bitmap);
                string strFileName = string.Empty;
                strFileName = System.IO.Directory.GetCurrentDirectory() + "\\" + "Error_" + dt.ToString("yyyyMMddHHmmss") + ".PNG";
                //g.CopyFromScreen(intBitWidth, intBitHeight, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceAnd);
                g.CopyFromScreen(ptSource, new Point(0, 0), new Size(intBitWidth, intBitHeight));
                bitmap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Png);
                //picInfo.Image=bitmap;
                //picInfo.SizeMode = PictureBoxSizeMode.StretchImage;
                // picCapImage.
                SendMail(strFileName, dt, strErrMessage);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region 데이터 압축 전송

        /// <summary>
        /// object를 압축하여 byte 배열로 변환합니다.
        /// </summary>
        public static byte[] ObjectToCompressedBytes(object obj)
        {
            if (obj == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(gzip, obj);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 압축된 byte 배열을 object 로 리턴합니다.
        /// </summary>
        public static object CompressedBytesToObject(byte[] arr)
        {
            if (arr == null || arr.Length == 0)
                return null;

            using (MemoryStream ms = new MemoryStream(arr))
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(gzip);
                }
            }
        }

        #endregion

        public static DataTable GetDataWithJoin(DataTable parentDt, DataTable childDt, string[] parentPrimaryColumns, string[] parentJoinColumns, string[] childJoinColumns, string parentSelectText)
        {
            // 기존 DataSet 관계 저장
            DataSet parentDs = parentDt.DataSet;
            DataSet childDs = childDt.DataSet;

            if (parentDs != null)
                parentDs.Tables.Remove(parentDt);

            if (childDs != null)
                childDs.Tables.Remove(childDt);

            DataSet ds = new DataSet();
            ds.Tables.Add(parentDt);
            ds.Tables.Add(childDt);

            // Key 추가
            DataColumn[] parentColArr = new DataColumn[parentJoinColumns.Length];

            for (int i = 0; i < parentJoinColumns.Length; i++)
                parentColArr[i] = parentDt.Columns[parentJoinColumns[i]];

            DataColumn[] parentKeyArr = new DataColumn[parentPrimaryColumns.Length];

            for (int i = 0; i < parentPrimaryColumns.Length; i++)
                parentKeyArr[i] = parentDt.Columns[parentPrimaryColumns[i]];

            DataColumn[] childColArr = new DataColumn[childJoinColumns.Length];

            for (int i = 0; i < childJoinColumns.Length; i++)
                childColArr[i] = childDt.Columns[childJoinColumns[i]];

            parentDt.PrimaryKey = parentKeyArr;

            // Relation 추가
            DataRelation rel = new DataRelation("relation", parentColArr, childColArr);
            ds.Relations.Add(rel);

            DataRow[] rows = parentDt.Select(parentSelectText);

            DataTable dt = null;

            foreach (DataRow row in rows)
            {
                DataRow[] resultRows = row.GetChildRows(ds.Relations[0]);

                if (dt == null)
                    dt = resultRows.CopyToDataTable();
                else
                    resultRows.CopyToDataTable(dt, LoadOption.OverwriteChanges);
            }

            // 제약조건 모두 삭제
            ds.Relations.Remove(rel);

            for (int i = childDt.Constraints.Count - 1; i >= 0; i--)
                childDt.Constraints.Remove(childDt.Constraints[i]);

            parentDt.PrimaryKey = null;

            ds.Tables.Clear();
            ds.Dispose();

            if (parentDs != null)
                parentDs.Tables.Add(parentDt);

            if (childDs != null)
                childDs.Tables.Add(childDt);

            return dt;
        }

        private static bool TryConvert<T>(string value, out T returnValue) where T : struct
        {
            returnValue = default(T);

            if (String.IsNullOrEmpty(value))
                return false;

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));

                if (converter != null)
                    returnValue = (T)converter.ConvertFromString(value);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 값을 가져옵니다.
        /// </summary>
        public static T GetValue<T>(string name) where T : struct
        {
            T t;
            if (TryConvert<T>(name, out t))
                return t;

            return default(T);
        }

        /// <summary>
        /// 값을 가져옵니다. 값이 비어있거나 변환에 실패하는 경우 전달된 값이 리턴됩니다.
        /// </summary>
        public static T GetValue<T>(string name, T defaultValue) where T : struct
        {
            T t;
            if (TryConvert<T>(name, out t))
                return t;

            return defaultValue;
        }

        /// <summary>
        /// 배경 색상에 따라 가장 잘 보이는 텍스트 Color 색상을 가져옵니다.
        /// </summary>
        public static Color GetIdealTextColor(Color backColor)
        {
            int nThreshold = 115;
            int bgDelta = System.Convert.ToInt32((backColor.R * 0.299) + (backColor.G * 0.587) + (backColor.B * 0.114));

            Color foreColor = (255 - bgDelta <= nThreshold) ? Color.Black : Color.White;
            return foreColor;
        }
    }
}
