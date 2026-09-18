using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace DACrux.SP.Common
{
    public class Utility
    {
        #region [ DataTable ]
        
        /// <summary>
        /// Sorting type
        /// </summary>
        public enum SortTypes { None, Asc, Desc };

        #region IsNormalDataTable

        /// <summary>
        /// Check whether Input DataTable(s) is(are) usable.
        /// </summary>
        /// <param name="dtList"></param>
        /// <returns>결과</returns>
        public static bool IsNormalDataTable(params DataTable[] dtList)
        {
            foreach (DataTable dt in dtList)
            {
                if (dt == null || dt.Rows.Count < 1)
                    return false;
            }

            return true;
        }

        #endregion

        #region GetColumnValues

        /// <summary>
        /// Get string array from the column of the datatable.
        /// </summary>
        /// <param name="dtSource">datatable</param>
        /// <param name="columnName">column name</param>
        /// <returns>result</returns>
        public static string[] GetColumnValues(DataTable dtSource, string columnName)
        {
            return GetColumnValues(dtSource, columnName, true, SortTypes.None);
        }

        /// <summary>
        /// Get string array from the column of the datatable.
        /// </summary>
        /// <param name="dtSource">datatable</param>
        /// <param name="columnName">column name</param>
        /// <param name="distinct">distinct flag</param>
        /// <returns>result</returns>
        public static string[] GetColumnValues(DataTable dtSource, string columnName, bool distinct)
        {
            return GetColumnValues(dtSource, columnName, distinct, SortTypes.None);
        }

        /// <summary>
        /// Get string array from the column of the datatable.
        /// </summary>
        /// <param name="dtSource">datatable</param>
        /// <param name="columnName">column name</param>
        /// <param name="sortType">sorting type</param>
        /// <returns>result</returns>
        public static string[] GetColumnValues(DataTable dtSource, string columnName, SortTypes sortType)
        {
            return GetColumnValues(dtSource, columnName, true, sortType);
        }

        /// <summary>
        /// Get string array from the column of the datatable.
        /// </summary>
        /// <param name="dtSource">datatable</param>
        /// <param name="columnName">column name</param>
        /// <param name="distinct">distinct flag</param>
        /// <param name="sortType">sorting type</param>
        /// <returns>result</returns>
        public static string[] GetColumnValues(DataTable dtSource, string columnName, bool distinct, SortTypes sortType)
        {
            string[] arrReturn = null;

            try
            {
                if (!IsNormalDataTable(dtSource))
                    return null;

                DataView dv = dtSource.DefaultView;

                if (sortType != SortTypes.None)
                    dv.Sort = string.Format("{0} {1}", columnName, sortType.ToString().ToUpper());

                DataTable dtTemp = dtSource.DefaultView.ToTable(distinct, columnName);

                arrReturn = new string[dtTemp.Rows.Count];

                for (int i = 0; i < arrReturn.Length; i++)
                    arrReturn[i] = dtTemp.Rows[i][0].ToString();

                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetDistinctTable

        /// <summary>
        /// Gets DistinctTable
        /// </summary>
        /// <param name="dtSource">datatable</param>
        /// <param name="columnName">column name</param>
        /// <param name="distinct">distinct flag</param>
        /// <param name="sortType">sorting type</param>
        /// <returns>result</returns>
        public static DataTable GetDistinctTable(DataTable dtSource, SortTypes sortType, params string[] columnNames)
        {
            DataTable dtTemp = null;

            try
            {
                if (!IsNormalDataTable(dtSource))
                    return null;

                DataView dv = dtSource.DefaultView;

                if (sortType != SortTypes.None)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    foreach (string columnName in columnNames)
                        sb.AppendFormat(", {0} {1}", columnName, sortType.ToString().ToUpper());

                    if (sb.Length > 0)
                        dv.Sort = sb.ToString(2, sb.Length - 2);
                }

                dtTemp = dtSource.DefaultView.ToTable(true, columnNames);

                return dtTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ShowDataTable
        /// <summary>
        /// Shows the dataTable data on a MessageBox.
        /// </summary>
        /// <param name="name">table name</param>
        /// <param name="dtData">datatable</param>
        public static void ShowDataTable(string name, DataTable dtData)
        {
            string strDtContent = string.Empty;

            for (int j = 0; j < dtData.Columns.Count; j++)
            {
                strDtContent += "\t" + dtData.Columns[j].ColumnName;
            }
            strDtContent += "\r\n1";

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                for (int j = 0; j < dtData.Columns.Count; j++)
                {
                    strDtContent += "\t" + dtData.Rows[i][j].ToString();
                }
                strDtContent += "\r\n" + (i + 2).ToString();
            }

            System.Windows.Forms.MessageBox.Show(strDtContent.Substring(0, strDtContent.LastIndexOf("\r\n")), name, System.Windows.Forms.MessageBoxButtons.OK);
        }
        #endregion

        #region [ GetStringFromDataTable ]

        public static string GetStringFromDataTable(DataTable dtSource, string columnSeperator, string rowSeperator)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in dtSource.Rows)
            {
                var a = (from object o in row.ItemArray
                         select o.ToString()).ToArray();

                sb.Append(string.Join(columnSeperator, a) + rowSeperator);
            }

            return sb.ToString();
        }

        #endregion

        #endregion

        #region [ ListView Util ]

        public static void MoveListViewItem(ListView lvSource, ListView lvTarget)
        {
            MoveListViewItem(lvSource, lvTarget, null);
        }

        public static void MoveListViewItem(ListView lvSource, ListView lvTarget, ListViewItem lvSourceItem)
        {
            ListViewItem lvItem = lvSourceItem;
            int index;

            if (lvItem != null)
            {
                index = lvItem.Index;
                lvSource.Items.Remove(lvItem);
                lvTarget.Items.Add(lvItem);
                lvItem.Selected = false;

                if (lvSource.Items.Count > 0)
                {
                    if (index == lvSource.Items.Count)
                        index--;

                    lvSource.Focus();
                    lvSource.Items[index].Selected = true;
                    lvSource.Items[index].Focused = true;
                }
            }
            else
            {
                if (lvSource.SelectedItems.Count > 0)
                {
                    lvItem = lvSource.SelectedItems[0];
                    index = lvItem.Index;
                    lvSource.Items.Remove(lvItem);
                    lvTarget.Items.Add(lvItem);
                    lvItem.Selected = false;

                    if (lvSource.Items.Count > 0)
                    {
                        if (index == lvSource.Items.Count)
                            index--;

                        lvSource.Focus();
                        lvSource.Items[index].Selected = true;
                        lvSource.Items[index].Focused = true;
                    }
                }
            }
        }

        public static void MoveListViewItems(ListView lvSource, ListView lvTarget, IList lvSourceItems)
        {
            ListViewItem lvItem;
            int index;

            if (lvSourceItems != null)
            {
                foreach (object obj in lvSourceItems)
                {
                    lvItem = (ListViewItem)obj;

                    index = lvItem.Index;
                    lvSource.Items.Remove(lvItem);
                    lvTarget.Items.Add(lvItem);
                    lvItem.Selected = false;

                    if (lvSource.Items.Count > 0)
                    {
                        if (index == lvSource.Items.Count)
                            index--;

                        lvSource.Focus();
                        lvSource.Items[index].Selected = true;
                        lvSource.Items[index].Focused = true;
                    }
                }
            }
        }

        #endregion

        #region [ Serialization ]

        public static byte[] GetSerializedData(object source)
        {
            BinaryFormatter bf;
            MemoryStream ms = null;
            byte[] arrByte = null;

            try
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream();

                if (source == null)
                    source = "NULL";
                bf.Serialize(ms, source);
                arrByte = ms.ToArray();

                ms.Seek(0, 0);
                object temp = bf.Deserialize(ms);

                return arrByte;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                bf = null;

                if (ms != null)
                {
                    ms.Close();
                    ms = null;
                }
            }
        }

        public static object GetDeserializedData(byte[] arrByte)
        {
            BinaryFormatter bf;
            MemoryStream ms = null;
            object objReturn = null;

            try
            {
                if (arrByte == null || arrByte.Length < 1)
                    return null;

                bf = new BinaryFormatter();
                ms = new MemoryStream();
                ms.Write(arrByte, 0, arrByte.Length);
                ms.Seek(0, 0);

                objReturn = bf.Deserialize(ms);

                return objReturn;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                bf = null;

                if (ms != null)
                {
                    ms.Close();
                    ms = null;
                }
            }
        }

        public static byte[] StrToByteArray(string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        #endregion

        #region [ ETC ]

        public static void SetEnable(Control ctl, bool isEnable)
        {
            ctl.Enabled = isEnable;

            foreach (Control control in ctl.Controls)
                SetEnable(control, isEnable);
        }

        public static string CombinedPath(params string[] pathList)
        {
            string strReturnPath = string.Empty;

            foreach (string path in pathList)
                strReturnPath = System.IO.Path.Combine(strReturnPath, path);

            return strReturnPath;
        }

        public static DialogResult ShowMessageBox(string message, MessageBoxIcon type)
        {
            return ShowMessageBox(message, type, MessageBoxButtons.OK);
        }

        public static DialogResult ShowMessageBox(string message, MessageBoxIcon type, MessageBoxButtons buttons)
        {
            if (string.IsNullOrEmpty(message))
                return DialogResult.None;

            return System.Windows.Forms.MessageBox.Show(message, Enum.GetName(typeof(MessageBoxIcon), type), buttons, type);
        }

        public static string ReadStringFromFile(string fileName,encoding enco)
        {
            Encoding en = null;
            switch (enco)
            {
                case encoding.ASCII:
                    en = Encoding.ASCII;
                    break;
                case encoding.BigEndianUnicode:
                    en = Encoding.BigEndianUnicode;
                    break;
                case encoding.Default:
                    en = Encoding.Default;
                    break;
                case encoding.Unicode:
                    en = Encoding.Unicode;
                    break;
                case encoding.UTF32:
                    en = Encoding.UTF32;
                    break;
                case encoding.UTF7:
                    en = Encoding.GetEncoding("us-ascii");
                    break;
                case encoding.UTF8:
                    en = Encoding.UTF8;
                    break;

            }
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".xls":
                    return Utility.LoadExcel(fileName);
                //case ".csv":
                default:
                    {
                        string data =null;
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, en))
                        {
                            data = sr.ReadToEnd().Replace('\0', ' ').Replace(Environment.NewLine, "\n");

                            if (enco == encoding.UTF7)
                                data = data.Replace("?", " ");
                        }
                        return data;

                        // 기존 코드 정리
                        //string data =null;
                        //if (enco == encoding.UTF7)
                        //{
                        //    using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, en))
                        //        data = sr.ReadToEnd().Replace(Environment.NewLine, "\n");

                        //    return data.Replace("?", " ");
                        //}
                        //else
                        //{
                        //    using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, en))
                        //        return sr.ReadToEnd().Replace(Environment.NewLine, "\n");
                        //}
                    }
            }
            //if (enco == encoding.ASCII)
            //{
            //    using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, Encoding.ASCII))
            //        return sr.ReadToEnd().Replace(Environment.NewLine, "\n");
            //}
            //else if (enco == encoding.BigEndianUnicode)
            //{
            //    using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, Encoding.BigEndianUnicode))
            //        return sr.ReadToEnd().Replace(Environment.NewLine, "\n");
            //}
            //else if (enco == encoding.Default)
            //{
            //    using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, Encoding.Default))
            //        return sr.ReadToEnd().Replace(Environment.NewLine, "\n");
            //}
            //else if (enco == encoding.Unicode)
            //{
            //    using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, Encoding.Unicode))
            //        return sr.ReadToEnd().Replace(Environment.NewLine, "\n");
            //}
            //else if (enco == encoding.UTF32)
            //{
            //    using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, Encoding.UTF32))
            //        return sr.ReadToEnd().Replace(Environment.NewLine, "\n");
            //}
            //else if (enco == encoding.UTF7)
            //{
            //    using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, Encoding.UTF7))
            //        return sr.ReadToEnd().Replace(Environment.NewLine, "\n");
            //}
            //else if (enco == encoding.UTF8)
            //{
            //    using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, Encoding.UTF8))
            //        return sr.ReadToEnd().Replace(Environment.NewLine, "\n");
            //}

        }

        #endregion

        #region [ Excel ]

        public static string LoadExcel(string fileName)
        {
            string connectString = string.Empty;
            string sheetName = string.Empty;

            StringBuilder sb = new StringBuilder(65536);

            DataTable dt = null;

            OleDbConnection oleDbConnection = null;
            OleDbDataAdapter oleDbAdapter = null;

            try
            {
                connectString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=" + (char)34 + "Excel 12.0 Xml;HDR=No;IMEX=1" + (char)34;
                //connectString = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HDR=No;IMEX=1'";

                oleDbConnection = new OleDbConnection(connectString);
                oleDbConnection.Open();

                DataTable dtSheet = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow sheet in dtSheet.Rows)
                {
                    sheetName = sheet["TABLE_NAME"].ToString();

                    if (sheetName.Contains("FilterDatabase"))
                        continue;

                    //if (sheetName.LastIndexOf("$'") == sheetName.Length - 2 && sheetName.LastIndexOf("$") == sheetName.Length - 1)
                    //    continue;

                    oleDbAdapter = new OleDbDataAdapter("SELECT * FROM [" + sheetName + "]", oleDbConnection);

                    dt = new DataTable();
                    oleDbAdapter.Fill(dt);
                    
                    sb.AppendLine("Excel Sheet [" + sheetName + "] Begin");
                    sb.Append(GetStringFromDataTable(dt, "\t", "\n"));
                    sb.AppendLine("Excel Sheet [" + sheetName + "] End\n");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oleDbConnection != null)
                    oleDbConnection.Close();
            }
        }

        #endregion

        public class ARGUMENT_TAG
        {
            #region Members

            public string RootPath;
            public string EquipPath;
            public string DataPath;

            public string FullFileName;
            public string DestinationPath;

            public string ResultFile;
            public string ResultPath;

            public string BackupFile;
            public string BackupPath;

            public string ProcessFile;
            public string ProcessPath;

            public string ErrorFile;
            public string ErrorPath;

            public string ServicePath;
            public string ServiceFile;

            public string LOGPath;
            public string LOGName;

            public string CNVPath;

            public string UploadComand;

            public int LogLevel;

            public string ResultType;

            public string DateTime;
            public string PathDateTime;
            public string DBData;

            public string ArchivePath;

            public string FileName;
            public string UnitType;
            public string FileExtension;

            public DateTime ParsingStartTime;

            public string strMapseq;
            public string strCustCode;
            public string strSPCode;

            public bool DeleteFlag;

            public string QueuePath;


            public int X = 0;
            public int Y = 0;
            public List<string> HanaBin = new List<string>();
            public List<string> CusBin = new List<string>();

            #endregion

            #region Reset

            public void Reset()
            {
                RootPath = string.Empty;
                EquipPath = string.Empty;
                DataPath = string.Empty;

                FullFileName = string.Empty;
                DestinationPath = string.Empty;

                ResultFile = string.Empty;
                ResultPath = string.Empty;

                BackupFile = string.Empty;
                BackupPath = string.Empty;

                ProcessFile = string.Empty;
                ProcessPath = string.Empty;

                ErrorFile = string.Empty;
                ErrorPath = string.Empty;

                ServicePath = string.Empty;
                ServiceFile = string.Empty;

                LOGPath = string.Empty;
                LOGName = string.Empty;

                CNVPath = string.Empty;
                QueuePath = string.Empty;

                UploadComand = string.Empty;

                LogLevel = 5;

                ResultType = string.Empty;

                DateTime = string.Empty;
                PathDateTime = string.Empty;
                DBData = string.Empty;

                ArchivePath = string.Empty;

                FileName = string.Empty;
                UnitType = string.Empty;
                FileExtension = string.Empty;

                ParsingStartTime = System.DateTime.Now;

                strCustCode = string.Empty;
                strMapseq = string.Empty;
                strSPCode = string.Empty;

                DeleteFlag = true;

                X = 0;
                Y = 0;

                HanaBin.Clear();
                CusBin.Clear();
            }

            #endregion
        }

        public sealed class Token
        {
            public int Index { get; set; }
            public int Length { get; set; }
        }


        #region[method]
        /// <summary>
        /// 고객사맵 -> 하나맵 변환 && 파일 이동
        /// </summary>
        public static void ConvertMap(string strProduct, string strWafer, string strX,
            string strY, string strBin, string strFlat, string strTotal, string strCNVPath, List<string> strCbin, List<string> strHbin)
        {
            StringBuilder sb = null;
            StreamWriter sw = null;
            try
            {
                sb = new StringBuilder();
                string CNVPath = strCNVPath + "\\" + strWafer + ".ASC";
                if (!Directory.Exists(strCNVPath))
                    Directory.CreateDirectory(strCNVPath);
                sw = new StreamWriter(CNVPath, false, Encoding.Default);

                sb.Append("DEVICE:" + strProduct + "\n");
                sb.Append("WAFERID:" + strWafer + "\n");
                sb.Append("X:" + strX + "\n");
                sb.Append("Y:" + strY + "\n");
                sb.Append("REFDIE:" + "\n");
                //if (strCbin.Count > 0) 
                //    strBin = ConvertMap(strBin, strCbin, strHbin);
                sb.Append(strBin);
                sb.Append("#" + strWafer + ":    " + "\n");
                sb.Append("FLAT ZONE : " + strFlat + "\n");
                sw.Write(sb.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                    sw.Dispose();
            }
        }

        public static string ReturnConvertMap(string strProduct, string strWafer, string strX,
            string strY, string strBin, string strFlat, string strTotal)
        {
            StringBuilder sb = null;
            StreamWriter sw = null;
            try
            {
                sb = new StringBuilder();
                //string CNVPath = strCNVPath + "\\" + strWafer + ".ASC";
                //if (!Directory.Exists(strCNVPath))
                //    Directory.CreateDirectory(strCNVPath);
                //sw = new StreamWriter(CNVPath, false, Encoding.Default);

                sb.Append("DEVICE:" + strProduct + "\n");
                sb.Append("WAFERID:" + strWafer + "\n");
                sb.Append("X:" + strX + "\n");
                sb.Append("Y:" + strY + "\n");
                sb.Append("REFDIE:" + "\n");
                //if (strCbin.Count > 0) 
                //    strBin = ConvertMap(strBin, strCbin, strHbin);
                sb.Append(strBin);
                sb.Append("#" + strWafer + ":    " + "\n");
                sb.Append("FLAT ZONE : " + strFlat + "\n");
                //sw.Write(sb.ToString());
                return sb.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sw != null)
                    sw.Dispose();
            }
        }
        //고객사 Bin -> 하나 Bin
        public static string CusBinToHanaBin(string oldbin, List<string> newbin, List<string> custombin)
        {
            try
            {
                // DB에 정의된 값이 있을 경우 해당 값을 반환
                if (newbin.Count > 0 && custombin.Count > 0)
                {
                    for (int a = 0; a < custombin.Count; a++)
                    {
                        if (oldbin == custombin[a])
                            return newbin[a];
                    }
                }

                // DB에 정의되지 않은 값은 '1' 과 '.' 을 제외하고는 모두 'D' 로 반환
                if (oldbin.Equals("1") || oldbin.Equals("."))
                    return oldbin;

                return "D";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string CusBinToHanaBin(string oldbin, DataTable dtConvertBinInfo)
        {
            try
            {
                DataRow[] drHBin = dtConvertBinInfo.Select(string.Format("[FROMBIN] = '{0}'", oldbin));

                if (drHBin.Length > 0)
                    return drHBin[0]["HBIN"].ToString();
                
                // DB에 정의되지 않은 값은 '1' 과 '.' 을 제외하고는 모두 'D' 로 반환
                if (oldbin.Equals("1") || oldbin.Equals("."))
                    return oldbin;

                return "D";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Bin -> 숫자로 변환
        public static int BinToInt(string oldbin)
        {
            try
            {
                int returnvalue = -1;

                if (oldbin.Contains(".") || oldbin.Contains("_")) // '.'일 경우 
                    returnvalue = 0;
                else
                {
                    if (!int.TryParse(oldbin, out returnvalue)) // 숫자일 경우
                    {
                        returnvalue = Convert.ToChar(oldbin);   // 문자일 경우 

                        if (returnvalue > 64 && returnvalue < 91) // A(65) ~ Z(90)
                        {
                            returnvalue = returnvalue - 55;
                        }
                    }
                }

                return returnvalue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //고객사 Bin -> 하나 Bin
        public static string ConvertBin(string oldbin, List<string> newbin, List<string> custombin)
        {
            try
            {
                if (custombin.Count > 0 && newbin.Count > 0)
                {
                    int len = -1;
                    for (int a = 0; a < custombin.Count; a++)
                    {
                        if (oldbin == custombin[a])
                        {
                            len = a;
                            break;
                        }
                    }
                    if (len >= 0)
                        return newbin[len];
                    else
                        return oldbin;
                }
                else return oldbin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //하나 Bin -> 숫자로 변환
        public static int ConvertHBin(string oldbin)
        {
            try
            {
                int returnvalue = -1;
                switch (oldbin.ToUpper())
                {
                    case "A":
                        returnvalue = 10;
                        break;
                    case "B":
                        returnvalue = 11;
                        break;
                    case "C":
                        returnvalue = 12;
                        break;
                    case "D":
                        returnvalue = 13;
                        break;
                    case "E":
                        returnvalue = 14;
                        break;
                    case "F":
                        returnvalue = 15;
                        break;
                    case "G":
                        returnvalue = 16;
                        break;
                    case "H":
                        returnvalue = 17;
                        break;
                    case "I":
                        returnvalue = 18;
                        break;
                    case "J":
                        returnvalue = 19;
                        break;
                    case "K":
                        returnvalue = 20;
                        break;
                    case "L":
                        returnvalue = 21;
                        break;
                    case "M":
                        returnvalue = 22;
                        break;
                    case "N":
                        returnvalue = 23;
                        break;
                    case "O":
                        returnvalue = 24;
                        break;
                    case "P":
                        returnvalue = 25;
                        break;
                    case "Q":
                        returnvalue = 26;
                        break;
                    case "R":
                        returnvalue = 27;
                        break;
                    case "S":
                        returnvalue = 28;
                        break;
                    case "T":
                        returnvalue = 29;
                        break;
                    case "U":
                        returnvalue = 30;
                        break;
                    case "V":
                        returnvalue = 31;
                        break;
                    case "W":
                        returnvalue = 32;
                        break;
                    case "X":
                        returnvalue = 33;
                        break;
                    case "Y":
                        returnvalue = 35;
                        break;
                    case "Z":
                        returnvalue = 36;
                        break;

                    case ".":
                        returnvalue = 0;
                        break;
                    default: returnvalue = int.Parse(oldbin); break;
                }
                return returnvalue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Cus Map -> Hana Map
        private static string ConvertMap(string strMap, List<string> cusbin, List<string> Hanabin)
        {
            try
            {
                if (cusbin[0].Length <= 1)
                {
                    for (int i = 0; i < cusbin.Count; i++)
                        strMap = strMap.Replace(cusbin[i], Hanabin[i]);

                }
                else
                {

                }
                return strMap;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Ref Die
        public static string[] ReferencePoint(string strCurrentPageText, string strFlat, Dictionary<string, string> dicBin)
        {
            int firChk = 0;
            int ReferX = 0;
            int ReferY = 0;
            int dataCount = 0;
            int DataRowp = 0;
            string[] DRArr = null;
            string[] newDRArr = null;
            string[] ArrTempXYs = null;
            string DRArrString = null;
            string DRArrBU = null;
            string DRArrStr = null;
            string ReferXY = null;
            string tempStr = null;

            try
            {
                ArrTempXYs = new string[1];
                for (int DataRow = 0; DataRow < dicBin.Count; )
                {
                    DRArrBU = dicBin[(DataRow).ToString()];

                    newDRArr = DRArrBU.Split('\n');
                    for (DataRowp = 0; DataRowp < newDRArr.Length; DataRowp++)
                    {
                        if (!newDRArr[DataRowp].Length.Equals(0))
                        {
                            dataCount++;
                            DRArrString = newDRArr[DataRowp];
                            if (dataCount.Equals(1))
                            {
                                tempStr += DRArrString;
                            }
                            else if (dataCount > 1)
                            {
                                tempStr += '\n' + DRArrString;
                            }
                        }
                    }
                    DRArr = tempStr.Split('\n');

                    //---------------------  Bottom 일 경우 START FlatZone : 0도  ----------------------
                    if (strFlat.ToUpper().Equals("BOTTOM") || strFlat.ToUpper().Equals("DOWN") || strFlat.ToUpper().Equals("0"))
                    {
                        for (int DRArri = 0; DRArri < DRArr.Length; DRArri++)
                        {
                            for (int DRArrj = 0; DRArrj < DRArr[DRArri].Length; DRArrj++)
                            {
                                DRArrStr = DRArr[DRArri].Substring(DRArrj, 1);

                                if (firChk < 1)
                                {
                                    if (!DRArrStr.Equals(".") && !DRArrStr.Equals(" "))
                                    {
                                        ReferX = DRArrj;
                                        ReferY = DRArri;
                                        firChk++;

                                    }
                                }
                                else
                                    break;
                            }
                        }
                    }

                    //---------------------  Left 일 경우 START FlatZone : 90도  ----------------------
                    if (strFlat.ToUpper().Equals("LEFT") || strFlat.ToUpper().Equals("90"))
                    {
                        for (int DRArri = DRArr.Length - 1; DRArri >= 0; DRArri--)
                        {
                            DRArrStr = DRArr[DRArri].Substring(0, 1);
                            if (firChk < 1)
                            {
                                if (!DRArrStr.Equals(".") && !DRArrStr.Equals(" "))
                                {
                                    ReferX = 0;
                                    ReferY = DRArri;
                                    firChk++;
                                    break;
                                }
                            }
                        }
                    }
                    //---------------------  UP 일 경우 START FlatZone : 180도  ----------------------
                    if (strFlat.ToUpper().Equals("UP") || strFlat.ToUpper().Equals("180") || strFlat.ToUpper().Equals("TOP"))
                    {
                        for (int DRArri = DRArr.Length - 1; DRArri >= 0; DRArri--)
                        {
                            for (int DRArrj = int.Parse(DRArr[DRArri].Length.ToString()) - 1; DRArrj >= 0; DRArrj--)
                            {
                                DRArrStr = DRArr[DRArri].Substring(DRArrj, 1);

                                if (firChk < 1)
                                {
                                    if (!DRArrStr.Equals(".") && !DRArrStr.Equals(" "))
                                    {
                                        ReferX = DRArrj;
                                        ReferY = DRArri;
                                        firChk++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    //---------------------  RIGHT 일 경우 START FlatZone : 270도  ----------------------
                    if (strFlat.ToUpper().Equals("RIGHT") || strFlat.ToUpper().Equals("270"))
                    {
                        for (int DRArri = 0; DRArri < DRArr.Length; DRArri++)
                        {
                            for (int DRArrj = int.Parse(DRArr[DRArri].Length.ToString()) - 1; DRArrj >= int.Parse(DRArr[DRArri].Length.ToString()) - 1; DRArrj--)
                            {
                                DRArrStr = DRArr[DRArri].Substring(DRArrj, 1);
                                if (firChk < 1)
                                {
                                    if (!DRArrStr.Equals(".") && !DRArrStr.Equals(" "))
                                    {
                                        ReferX = DRArrj;
                                        ReferY = DRArri;
                                        firChk++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    ReferXY = ReferX + " " + ReferY;

                    ArrTempXYs[0] = ReferXY; //레퍼런스 포인트 저장 배열

                    firChk = 0;
                    DataRowp = 0;
                    dataCount = 0;
                    DRArr = null;
                    tempStr = null;
                    break;
                }
                return ArrTempXYs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string[] ReferencePoint(string strFlat, string strMap)
        {
            string[] referencePoint = new string[1];
            string strTargetX = string.Empty;
            string strTargetY = string.Empty;
            string strTargetX_2 = string.Empty;
            string strTargetY_2 = string.Empty;
            try
            {
                strMap = strMap.Substring(0, strMap.LastIndexOf("\n"));
                string[] strMapArray = strMap.Split('\n');
                switch (strFlat.ToLower())
                {
                    case "bottom":
                        strTargetY = "1";
                        for (int i = 0; i < strMapArray[0].Length; i++)
                        {
                            if (!strMapArray[0][i].ToString().Equals("."))
                            {
                                strTargetX = (i + 1).ToString();
                                break;
                            }
                        }
                        strTargetY_2 = (strMapArray.Length).ToString();
                        for (int i = strMapArray[0].Length - 1; i > 0; i--)
                        {
                            if (string.IsNullOrEmpty(strMapArray[strMapArray.Length - 1].ToString()))
                                continue;
                            if (!strMapArray[strMapArray.Length - 1][i].ToString().Equals("."))
                            {
                                strTargetX_2 = (i + 1).ToString();
                                break;
                            }
                        }

                        break;
                    case "top":
                        strTargetY = (strMapArray.Length).ToString();
                        for (int i = strMapArray[0].Length - 1; i > 0; i--)
                        {
                            if (string.IsNullOrEmpty(strMapArray[strMapArray.Length - 1].ToString()))
                                continue;
                            if (!strMapArray[strMapArray.Length - 1][i].ToString().Equals("."))
                            {
                                strTargetX = (i + 1).ToString();
                                break;
                            }
                        }
                        strTargetY_2 = "1";
                        for (int i = 0; i < strMapArray[0].Length; i++)
                        {
                            if (!strMapArray[0][i].ToString().Equals("."))
                            {
                                strTargetX_2 = (i + 1).ToString();
                                break;
                            }
                        }
                        break;
                    case "left":
                        strTargetX = (strMapArray[0].Length).ToString();
                        for (int i = 0; i < strMapArray.Length; i++)
                        {
                            if (string.IsNullOrEmpty(strMapArray[i].ToString()))
                                continue;
                            if (!strMapArray[i][strMapArray[0].Length - 1].ToString().Equals("."))
                            {
                                strTargetY = (i + 1).ToString();
                                break;
                            }
                        }
                        strTargetX_2 = "1";
                        for (int i = strMapArray.Length - 1; i > 0; i--)
                        {
                            if (string.IsNullOrEmpty(strMapArray[i].ToString()))
                                continue;
                            if (!strMapArray[i][0].ToString().Equals("."))
                            {
                                strTargetY_2 = (i + 1).ToString();
                                break;
                            }
                        }
                        break;
                    case "right":
                        strTargetX = "1";
                        for (int i = strMapArray.Length - 1; i > 0; i--)
                        {
                            if (string.IsNullOrEmpty(strMapArray[i].ToString()))
                                continue;
                            if (!strMapArray[i][0].ToString().Equals("."))
                            {
                                strTargetY = (i + 1).ToString();
                                break;
                            }
                        }
                        strTargetX_2 = (strMapArray[0].Length).ToString();
                        for (int i = 0; i < strMapArray.Length; i++)
                        {
                            if (string.IsNullOrEmpty(strMapArray[i].ToString()))
                                continue;
                            if (!strMapArray[i][strMapArray[0].Length - 1].ToString().Equals("."))
                            {
                                strTargetY_2 = (i + 1).ToString();
                                break;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            referencePoint[0] = strTargetX + " " + strTargetY + " " + strTargetX_2 + " " + strTargetY_2;
            return referencePoint;
        }
        private struct KeyValue
        {
            public string strKey;
            public int iRowNum;
        }
        public static string ValidationMap(string strFileName, ref Analysis oAnalysis)
        {

            try
            {
                int iRowCNT = 0;
                int iOneWaferRowCNT = 0;
                List<KeyValue> strValidationKey = new List<KeyValue>();
                KeyValue key = new KeyValue();
                StreamReader sr = new StreamReader(strFileName);
                string strTotalValue = sr.ReadToEnd();
                strTotalValue = strTotalValue.Replace(Environment.NewLine, "\n").TrimStart('\n').TrimEnd('\n').Replace("\r", "");
                string[] strData = strTotalValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                sr.Dispose();

                int x, y;
                if (oAnalysis.Entities["X"].DefaultValue == string.Empty)
                {
                    if (oAnalysis.values["X"].Count < 1)
                        return string.Format("ERROR : oAnalysis.values[\"X\"].Count == 0 이므로 X값을 가져올 수 없습니다.");

                    if (int.TryParse(strTotalValue.Substring(oAnalysis.values["X"][0].Index, oAnalysis.values["X"][0].Length), out x) == false)
                        return string.Format("ERROR : Can not be converted to a number. [X:{0}]", strTotalValue.Substring(oAnalysis.values["X"][0].Index, oAnalysis.values["X"][0].Length));
                }
                else
                {
                    if(int.TryParse(oAnalysis.Entities["X"].DefaultValue, out x) == false)
                        return string.Format("ERROR : Can not be converted to a number. [X:{0}]", oAnalysis.Entities["X"].DefaultValue);
                }

                if (oAnalysis.Entities["Y"].DefaultValue == string.Empty)
                {
                    if (oAnalysis.values["Y"].Count < 1)
                        return string.Format("ERROR : oAnalysis.values[\"Y\"].Count == 0 이므로 Y값을 가져올 수 없습니다.");

                    if (int.TryParse(strTotalValue.Substring(oAnalysis.values["Y"][0].Index, oAnalysis.values["Y"][0].Length), out y) == false)
                        return string.Format("ERROR : Can not be converted to a number. [Y:{0}]", strTotalValue.Substring(oAnalysis.values["Y"][0].Index, oAnalysis.values["Y"][0].Length));
                }
                else
                {
                    if(int.TryParse(oAnalysis.Entities["Y"].DefaultValue, out y) == false)
                        return string.Format("ERROR : Can not be converted to a number. [Y:{0}]", oAnalysis.Entities["Y"].DefaultValue);
                }

                //int x = oAnalysis.Entities["X"].DefaultValue == "" ? int.Parse(strTotalValue.Substring(oAnalysis.values["X"][0].Index, oAnalysis.values["X"][0].Length)) : int.Parse(oAnalysis.Entities["X"].DefaultValue);
                //int y = oAnalysis.Entities["Y"].DefaultValue == "" ? int.Parse(strTotalValue.Substring(oAnalysis.values["Y"][0].Index, oAnalysis.values["Y"][0].Length)) : int.Parse(oAnalysis.Entities["Y"].DefaultValue);

                string sWafer = string.Empty;

                foreach (string tempKey in oAnalysis.values.Keys)
                {
                    if (tempKey.Equals(EntityString.Wafer_ID.ToString()))
                        sWafer = "Wafer_ID";
                    else if (tempKey.Equals(EntityString.Wafer_Num.ToString()))
                        sWafer = "Wafer_Num";
                }
                
                //if ((strData.Length % oAnalysis.values["Wafer_ID"].Count) > 1)
                if ((strData.Length % oAnalysis.values[sWafer].Count) > 1)
                    return string.Format("ERROR : (Line Count({0}) % {1} Count({2})) > 1", strData.Length, sWafer, oAnalysis.values[sWafer].Count);
                if ((oAnalysis.values[EntityString.bin.ToString()].Count % y > 0) && (oAnalysis.Entities[EntityString.Y.ToString()].RegexString != string.Empty))
                    return string.Format("ERROR : (Bin Count({0}) % Y({1})) > 0", oAnalysis.values[EntityString.bin.ToString()].Count, y);
                //iOneWaferRowCNT = strData.Length / oAnalysis.values["Wafer_ID"].Count;
                iOneWaferRowCNT = strData.Length / oAnalysis.values[sWafer].Count;
                if (oAnalysis.Entities[EntityString.X.ToString()].RegexString != string.Empty)
                {
                    for (int i = 0; i < oAnalysis.values[EntityString.bin.ToString()].Count; i++)
                    {
                        if (oAnalysis.values[EntityString.bin.ToString()][i].Length != x)
                            return string.Format("ERROR : Bin Length({0}) != X({1})", oAnalysis.values[EntityString.bin.ToString()][i].Length, x);
                    }
                }
                for (int row = 0; row < oAnalysis.Entities.Count; row++)
                {
                    if (string.IsNullOrEmpty(oAnalysis.Entities[row].DefaultValue))
                    {
                        iRowCNT++;
                        if (!string.IsNullOrEmpty(oAnalysis.Entities[row].strKey) && !string.IsNullOrEmpty(oAnalysis.Entities[row].RowNum))
                        {
                            key.strKey = oAnalysis.Entities[row].strKey;
                            key.iRowNum = int.Parse(oAnalysis.Entities[row].RowNum);
                            strValidationKey.Add(key);
                        }
                    }
                }
                for (int keylen = 0; keylen < strValidationKey.Count; keylen++)
                {
                    for (int datalen = 0; datalen < oAnalysis.values[sWafer].Count; datalen++)
                    {
                        if (!strData[(strValidationKey[keylen].iRowNum) - 1 + (iOneWaferRowCNT * datalen)].StartsWith(strValidationKey[keylen].strKey))
                        {
                            return string.Format("ERROR : Key Mismatch ({0} = {1})", strData[(strValidationKey[keylen].iRowNum) - 1 + (iOneWaferRowCNT * datalen)], strValidationKey[keylen].strKey);
                        }
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


        #region [ 신규 정의 ]

        public class SP_ARGUMENT_TAG
        {
            #region Members

            public string ResultFile;
            public string ResultPath;
            public string BackupFile;
            public string BackupPath;
            public string ErrorFile;
            public string ErrorPath;
            public string LOGPath;
            public string LOGMsg;
            public string CNVPath;
            public string SPRPath;
            public string SPRFile;
            public int LogLevel;
            public string DBData;
            public string strMapseq;
            public string strCustCode;
            public string strSPCode;
            public DataTable BinConvertInfo;
            public DateTime CreateTime;
            public DateTime StartTime;
            public DateTime EndTime;

            #endregion

            #region Reset

            public void Reset()
            {
                ResultFile = string.Empty;
                ResultPath = string.Empty;
                BackupFile = string.Empty;
                BackupPath = string.Empty;
                ErrorFile = string.Empty;
                ErrorPath = string.Empty;
                LOGPath = string.Empty;
                LOGMsg = string.Empty;
                CNVPath = string.Empty;
                SPRPath = string.Empty;
                SPRFile = string.Empty;
                LogLevel = 4;
                DBData = string.Empty;
                strCustCode = string.Empty;
                strMapseq = string.Empty;
                strSPCode = string.Empty;
                if (BinConvertInfo != null) BinConvertInfo.Reset();
                BinConvertInfo = null;
                CreateTime = new DateTime();
                StartTime = new DateTime();
                EndTime = new DateTime();
            }

            #endregion
        }
        
        #endregion
    }
}
