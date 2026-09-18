using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using DACrux.Common.RO;

namespace DACrux.TEST.Control
{
    public static class Util
    {
        public static string ReplaceSpecialCharacter(string sData, string sReplaceChar = "_")
        {
            return sData.Replace("-", sReplaceChar).Replace(".", sReplaceChar).Replace("/", sReplaceChar).Replace("#", sReplaceChar).Replace("@", sReplaceChar).Replace("(", sReplaceChar).Replace(")", sReplaceChar).Replace(" ", sReplaceChar).Replace(":", sReplaceChar).Replace("[", sReplaceChar).Replace("]", sReplaceChar).Replace("+", sReplaceChar);
        }

        public static string MakeTableName(string sFactory, string sDevice, string sReplaceChar = "_")
        {
            string sData = string.Empty;

            if (sFactory == "PT01")
                sData = string.Format("TQ_PD_{0}", sDevice);
            else if (sFactory == "FT01")
                sData = string.Format("TQ_FD_{0}", sDevice);

            return sData.Replace("-", sReplaceChar).Replace(".", sReplaceChar).Replace("/", sReplaceChar).Replace("#", sReplaceChar).Replace("@", sReplaceChar).Replace("(", sReplaceChar).Replace(")", sReplaceChar).Replace(" ", sReplaceChar).Replace(":", sReplaceChar).Replace("[", sReplaceChar).Replace("]", sReplaceChar).Replace("+", sReplaceChar);
        }

        public static string MakeMergeTableName(string sFactory, string sDevice, string sReplaceChar = "_")
        {
            string sData = string.Empty;

            if (sFactory == "PT01")
                sData = string.Format("TQ_PM_{0}", sDevice);
            else if (sFactory == "FT01")
                sData = string.Format("TQ_FM_{0}", sDevice);

            return sData.Replace("-", sReplaceChar).Replace(".", sReplaceChar).Replace("/", sReplaceChar).Replace("#", sReplaceChar).Replace("@", sReplaceChar).Replace("(", sReplaceChar).Replace(")", sReplaceChar).Replace(" ", sReplaceChar).Replace(":", sReplaceChar).Replace("[", sReplaceChar).Replace("]", sReplaceChar).Replace("+", sReplaceChar);
        }

        public static byte[] ConvertToByte(string val)
        {
            try
            {
                string[] sVal = val.Split(' ');
                byte[] body = new byte[sVal.Length];
                for (int a = 0; a < body.Length; a++) body[a] = (byte)Convert.ToByte(sVal[a], 10);
                return body;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ConvertToString(string strFileName, int nType = 0)
        {
            switch (nType)
            {
                case 0:
                    return ConvertToString(new FileInfo(strFileName));
                case 1:
                    return ConvertToString1(new FileInfo(strFileName));
                case 2:
                    return ConvertToString2(new FileInfo(strFileName));
                default:
                    return ConvertToString(new FileInfo(strFileName));
            }
        }

        public static string ConvertToString2(FileInfo fi)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            string tempFile = Path.GetTempFileName();

            try
            {
                int BUFFER_SIZE = 1024 * 1024;

                byte[] buffer = new byte[BUFFER_SIZE];

                fs = fi.OpenRead();
                sw = new StreamWriter(tempFile);

                int count = 0;

                while (true)
                {
                    count = fs.Read(buffer, 0, BUFFER_SIZE);

                    if (count <= 0)
                        break;

                    for (int i = 0; i < count; i++)
                        sw.Write(String.Format("{0} ", buffer[i]));
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();

                if (sw != null)
                    sw.Dispose();
            }

            string converted;

            using (StreamReader sr = File.OpenText(tempFile))
            {
                converted = sr.ReadToEnd();
                sr.DiscardBufferedData();
            }

            File.Delete(tempFile);

            return converted;
        }

        public static string ConvertToString1(FileInfo fi)
        {
            int BUFFER_SIZE = 1024 * 1024;
            byte[] buffer = new byte[BUFFER_SIZE];
            StringBuilder sb = null;

            using (FileStream fs = fi.OpenRead())
            {
                int count = 0;
                sb = new StringBuilder((int)fs.Length * 4);

                while (true)
                {
                    count = fs.Read(buffer, 0, BUFFER_SIZE);

                    if (count <= 0)
                        break;

                    for (int i = 0; i < count; i++)
                        sb.AppendFormat("{0} ", buffer[i]);
                }
            }

            if (sb == null)
                return null;

            return sb.ToString();
        }

        public static string ConvertToString(FileInfo fi)
        {
            FileStream fs = null;
            try
            {
                fs = fi.OpenRead();
                byte[] body = new byte[fi.Length];
                fs.Read(body, 0, body.Length);
                string[] strBody = new string[body.Length];
                for (int a = 0; a < strBody.Length; a++) strBody[a] = string.Format("{0}", Convert.ToInt16(body[a]));
                return string.Join(" ", strBody);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
            }
        }

        public static string ConvertToString(char[] Source)
        {
            try
            {
                string[] strBody = new string[Source.Length];
                for (int a = 0; a < strBody.Length; a++) strBody[a] = string.Format("{0}", Convert.ToInt16(Source[a]));
                return string.Join(" ", strBody);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ConvertToString(long lNumber)
        {
            try
            {
                byte[] btSize = BitConverter.GetBytes(lNumber);
                string[] strBody = new string[4];
                for (int a = 0; a < strBody.Length; a++) strBody[3 - a] = string.Format("{0}", Convert.ToInt16(btSize[a]));
                return string.Join(" ", strBody);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// DataTable Pivot
        /// ------------------------------------------------------------------------------------------------------
        ///  TargetRows[0]  | TargetRows[1]..| sPivotRowsName | PivotCols[0]  | PivotCols[1]  | PivotCols[2]| ...
        /// ------------------------------------------------------------------------------------------------------
        ///                                  | PivotRows[0]   |               |               |             |
        ///                                  | PivotRows[1]   |               |               |             |
        ///                                  | PivotRows[2]   |               |               |             |
        ///                                  | PivotRows[3]   |               |               |             |
        ///                                  | ...            |               |               |             |
        /// </summary>
        /// <param name="sourceDr">Data Rows</param>
        /// <param name="sTargetRows">Pivot 기준 컬럼 데이타</param>
        /// <param name="sPivotRows">행으로 변환될 컬럼</param>
        /// <param name="sPivotRowsName">행으로 변환될 컬럼들의 컬럼명</param>
        /// <param name="sPivotCols">열로 변환될 컬럼</param>
        /// <returns></returns>
        public static DataTable TablePivot(DataTable sourceDt, string[] sTargetRows, string[] sPivotRows, string[] sPivotCols)
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, "VALUE", sPivotCols);
        }
        public static DataTable TablePivot(DataTable sourceDt, string[] sTargetRows, string[] sPivotRows, string[] sPivotCols, bool bSetDefault)
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, "VALUE", sPivotCols, bSetDefault);
        }
        public static DataTable TablePivot(DataRow[] sourceDr, string[] sTargetRows, string[] sPivotRows, string[] sPivotCols)
        {
            return TablePivot(sourceDr, sTargetRows, sPivotRows, "VALUE", sPivotCols);
        }
        public static DataTable TablePivot(DataRow[] sourceDr, string[] sTargetRows, string[] sPivotRows, string[] sPivotCols, bool bSetDefault)
        {
            return TablePivot(sourceDr, sTargetRows, sPivotRows, "VALUE", sPivotCols, bSetDefault);
        }
        public static DataTable TablePivot(DataTable sourceDt, string[] sTargetRows, string[] sPivotRows, string sPivotRowsName, string[] sPivotCols)
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, sPivotRowsName, sPivotCols);
        }
        public static DataTable TablePivot(DataRow[] sourceDr, string[] sTargetRows, string[] sPivotRows, string sPivotRowsName, string[] sPivotCols)
        {
            return TablePivot(sourceDr, sTargetRows, sPivotRows, sPivotRowsName, sPivotCols, false);
        }
        public static DataTable TablePivot(DataTable sourceDt, string[] sTargetRows, string[] sPivotRows, string sPivotRowsName, string[] sPivotCols, bool bSetDefault)
        {
            return TablePivot(sourceDt.Select(), sTargetRows, sPivotRows, sPivotRowsName, sPivotCols, bSetDefault);
        }
        public static DataTable TablePivot(DataRow[] sourceDr, string[] sTargetRows, string[] sPivotRows, string sPivotRowsName, string[] sPivotCols, bool bSetDefault)
        {
            DataTable dt = null;

            try
            {
                if (sourceDr == null || sourceDr.Length < 1)
                    return null;

                if (sPivotCols.Length > 1 && sPivotCols.Length != sPivotRows.Length)
                    return null;


                dt = new DataTable();

                // sTargetRows 컬럼 생성
                foreach (string sTargetRow in sTargetRows)
                {
                    string columnName = sourceDr[0].Table.Columns[sTargetRow].ColumnName;
                    Type type = sourceDr[0].Table.Columns[sTargetRow].DataType;
                    dt.Columns.Add(columnName, type);
                }

                // sPivotRows 컬럼 생성
                if (sPivotCols.Length == 1)
                    dt.Columns.Add(sPivotRowsName, typeof(string));

                // sPivotCols 컬럼 생성
                DataSet dsTemp = new DataSet();
                foreach (string sPivotCol in sPivotCols)
                {
                    dsTemp.Tables.Add(sPivotCol);
                }
                // 임시 DataSet에 각각의 컬럼을 넣는다.
                foreach (DataRow dr in sourceDr)
                {
                    for (int i = 0; i < sPivotCols.Length; i++)
                    {
                        string columnName = dr[sPivotCols[i]].ToString();
                        Type type = dr.Table.Columns[sPivotRows[i]].DataType;

                        if (!dsTemp.Tables[sPivotCols[i]].Columns.Contains(columnName))
                        {
                            dsTemp.Tables[sPivotCols[i]].Columns.Add(columnName, type);
                        }
                    }
                }
                // 임시 DataSet의 테이블 컬럼을 하나의 테이블 컬럼으로 연결해서 넣는다.
                foreach (string sPivotCol in sPivotCols)
                {
                    foreach (DataColumn dc in dsTemp.Tables[sPivotCol].Columns)
                    {
                        if (!dt.Columns.Contains(dc.ColumnName))
                        {
                            if (bSetDefault)
                            {
                                dc.AllowDBNull = bSetDefault;

                                //switch (dc.DataType.Name.ToUpper())
                                //{
                                //    case "STRING":
                                //        dc.DefaultValue = string.Empty;
                                //        break;
                                //    case "DECIMAL":
                                //        dc.DefaultValue = double.NaN;
                                //        break;
                                //    case "INT32":
                                //        dc.DefaultValue = 0;
                                //        break;
                                //    case "DATETIME":
                                //        dc.DefaultValue = DateTime.Today;
                                //        break;
                                //}
                            }
                            dt.Columns.Add(dc.ColumnName, dc.DataType);
                        }
                    }
                }

                // Pivot 데이터 Row 생성
                foreach (DataRow dr in sourceDr)
                {
                    bool IsInsert = true;

                    // 현재 입력된 데이터 체크
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {

                        bool IsContinue = false;
                        foreach (string sTargetRow in sTargetRows)
                        {
                            // sTargetRow 중 하나라도 다르면 다음 행 비교
                            if (dt.Rows[row][sTargetRow].ToString() != dr[sTargetRow].ToString())
                            {
                                IsContinue = true;
                                break;
                            }
                        }

                        if (IsContinue)
                            continue;

                        if (sPivotCols.Length == 1)
                        {
                            for (int i = 0; i < sPivotRows.Length; i++)
                            {
                                if (dt.Rows[row + i][sPivotRowsName].ToString() == sPivotRows[i])
                                {
                                    dt.Rows[row + i][dr[sPivotCols[0]].ToString()] = dr[sPivotRows[i]];
                                    IsInsert = false;
                                    //break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < sPivotCols.Length; i++)
                            {
                                dt.Rows[row][dr[sPivotCols[i]].ToString()] = dr[sPivotRows[i]];
                                IsInsert = false;
                            }
                        }

                        if (!IsInsert)
                            break;
                    }

                    // 새로운 행을 추가한다.
                    if (IsInsert)
                    {
                        if (sPivotCols.Length == 1)
                        {
                            for (int i = 0; i < sPivotRows.Length; i++)
                            {
                                DataRow drRow = dt.NewRow();

                                foreach (string sTargetRow in sTargetRows)
                                {
                                    drRow[sTargetRow] = dr[sTargetRow];
                                }

                                drRow[sPivotRowsName] = sPivotRows[i];
                                drRow[dr[sPivotCols[0]].ToString()] = dr[sPivotRows[i]];
                                dt.Rows.Add(drRow);
                            }
                        }
                        else
                        {
                            DataRow drRow = dt.NewRow();

                            foreach (string sTargetRow in sTargetRows)
                            {
                                drRow[sTargetRow] = dr[sTargetRow];
                            }

                            for (int i = 0; i < sPivotCols.Length; i++)
                            {
                                drRow[dr[sPivotCols[i]].ToString()] = dr[sPivotRows[i]];
                            }
                            dt.Rows.Add(drRow);
                        }
                    }
                }

                dt.AcceptChanges();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable Pivot2(DataRow[] sourceDr, string[] sBaseColumnName, string sBaseHeaderColumnName, string[] RotColumnName)
        {
            if (sourceDr.Length < 1)
                return null;

            if (sBaseColumnName.Length != RotColumnName.Length)
                return null;

            DataTable dt = new DataTable();

            int sBaseHeaderColumnIndex = sourceDr[0].Table.Columns.IndexOf(sBaseHeaderColumnName);
            if (sBaseHeaderColumnIndex != -1)
                dt.Columns.Add(sBaseHeaderColumnName, sourceDr[0].Table.Columns[sBaseHeaderColumnIndex].DataType);

            for (int Index = 0; Index < sBaseColumnName.Length; Index++)
            {
                int sBaseColumnIndex = sourceDr[0].Table.Columns.IndexOf(sBaseColumnName[Index]);
                int sRotColumnIndex = sourceDr[0].Table.Columns.IndexOf(RotColumnName[Index]);

                foreach (DataRow sBaseColumnRow in sourceDr)
                {
                    string ColName = sBaseColumnRow[sBaseColumnIndex].ToString();
                    if (!dt.Columns.Contains(ColName))
                    {
                        dt.Columns.Add(ColName, sBaseColumnRow.Table.Columns[sRotColumnIndex].DataType);
                    }
                }

                for (int rowIndex = 0; rowIndex < sourceDr.Length; rowIndex++)
                {
                    bool IsInsert = true;
                    if (sBaseHeaderColumnIndex != -1)
                    {
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            if (dt.Rows[row][0].ToString() == sourceDr[rowIndex][sBaseHeaderColumnIndex].ToString())
                            {
                                dt.Rows[row][dt.Columns.IndexOf(sourceDr[rowIndex][sBaseColumnIndex].ToString())] = sourceDr[rowIndex][sRotColumnIndex];
                                IsInsert = false;
                            }
                        }

                        if (IsInsert)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = sourceDr[rowIndex][sBaseHeaderColumnIndex];
                            dr[dt.Columns.IndexOf(sourceDr[rowIndex][sBaseColumnIndex].ToString())] = sourceDr[rowIndex][sRotColumnIndex];
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            if (dt.Rows[row][0].ToString() == RotColumnName[Index])
                            {
                                dt.Rows[row][dt.Columns.IndexOf(sourceDr[rowIndex][sBaseColumnIndex].ToString())] = sourceDr[rowIndex][sRotColumnIndex];
                                IsInsert = false;
                            }
                        }

                        if (IsInsert)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = RotColumnName[Index];
                            dr[dt.Columns.IndexOf(sourceDr[rowIndex][sBaseColumnIndex].ToString())] = sourceDr[rowIndex][sRotColumnIndex];
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// 피벗 메서드.
        /// 소스 데이타를 특정 컬럼 기준으로 컬럼으로 확장 후 데이타를 확장컬럼에 넣어서 반환해줌. 
        /// 추가 : 나머지 컬럼은 모두 삭제
        /// </summary>
        /// <param name="sourceTb"> 변환 될 데이타 테이블</param>
        /// <param name="sBaseColumnName"> 컬럼변경 데이타를 가진 컬럼명</param>
        /// <param name="sBaseHeaderColumnName"> 컬럼에 대한 헤더컬럼.</param>
        /// <param name="RotColumnName"> 옮겨질 데이타. </param>
        /// <returns></returns>
        /// EMS.Base.Util.Pivot(dtTAT.Select(), "OPER_DESC", "LOT_ID", new string[] { "TOTAL_TIME", "RES_ID" });
        public static DataTable Pivot(DataRow[] sourceDr, string sBaseColumnName, string sBaseHeaderColumnName, string[] RotColumnName)
        {
            if (sourceDr.Length < 1)
                return null;

            DataTable dt = new DataTable();
            int sBaseColumnIndex = sourceDr[0].Table.Columns.IndexOf(sBaseColumnName);
            int sBaseHeaderColumnIndex = sourceDr[0].Table.Columns.IndexOf(sBaseHeaderColumnName);
            int[] sRotColumnIndex = new int[RotColumnName.Length];
            for (int i = 0; i < RotColumnName.Length; i++)
                sRotColumnIndex[i] = sourceDr[0].Table.Columns.IndexOf(RotColumnName[i]);

            if (sBaseHeaderColumnIndex != -1)
                dt.Columns.Add(sBaseHeaderColumnName, sourceDr[0].Table.Columns[sBaseHeaderColumnIndex].DataType);

            dt.Columns.Add("VALUE", typeof(string));
            string ColName = string.Empty;

            foreach (DataRow sBaseColumnRow in sourceDr)
            {
                ColName = sBaseColumnRow[sBaseColumnIndex].ToString();
                if (!dt.Columns.Contains(ColName))
                {
                    dt.Columns.Add(ColName, sBaseColumnRow.Table.Columns[sRotColumnIndex[0]].DataType);
                }
            }

            for (int rowIndex = 0; rowIndex < sourceDr.Length; rowIndex++)
            {
                bool IsInsert = true;
                if (sBaseHeaderColumnIndex != -1)
                {
                    for (int i = 0; i < RotColumnName.Length; i++)
                    {
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            if (dt.Rows[row][0].ToString() == sourceDr[rowIndex][sBaseHeaderColumnIndex].ToString() && dt.Rows[row][1].ToString() == RotColumnName[i])
                            {
                                dt.Rows[row][dt.Columns.IndexOf(sourceDr[rowIndex][sBaseColumnIndex].ToString())] = sourceDr[rowIndex][sRotColumnIndex[i]];
                                IsInsert = false;
                            }
                        }

                        if (IsInsert)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = sourceDr[rowIndex][sBaseHeaderColumnIndex];
                            dr[1] = RotColumnName[i];
                            dr[dt.Columns.IndexOf(sourceDr[rowIndex][sBaseColumnIndex].ToString())] = sourceDr[rowIndex][sRotColumnIndex[i]];
                            dt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < RotColumnName.Length; i++)
                    {
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            if (dt.Rows[row][0].ToString() == RotColumnName[i])
                            {
                                dt.Rows[row][dt.Columns.IndexOf(sourceDr[rowIndex][sBaseColumnIndex].ToString())] = sourceDr[rowIndex][sRotColumnIndex[i]];
                                IsInsert = false;
                            }
                        }

                        if (IsInsert)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = RotColumnName[i];
                            dr[dt.Columns.IndexOf(sourceDr[rowIndex][sBaseColumnIndex].ToString())] = sourceDr[rowIndex][sRotColumnIndex[i]];
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }

            dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// 컨버트 메소드
        /// 소스 데이타를 특정 컬럼의 요소로 모두 바인딩한다.
        /// 추가 : 나머지 컬럼은 모두 삭제.
        /// </summary>
        /// <param name="sourceTb"> 변환 될 데이타 테이블</param>
        /// <param name="sBaseColumnName"> 컬럼변경 데이타를 가진 컬럼명</param>
        /// <param name="sBaseHeaderColumnName"> 컬럼에 대한 헤더컬럼.</param>
        /// <param name="RotColumnName"> 옮겨질 데이타. </param>
        /// <returns></returns>
        public static DataTable ConvertTable(DataTable sourceDt, string[] sBaseColumnName, string sTargetColumnName, string sDefultValueColumn)
        {
            DataTable dt = null;
            DataTable dtResult = null;
            List<string> lsTargetColumns = null;
            List<string>[] lsBaseColumns = null;
            try
            {
                if (sourceDt.Rows.Count < 1)
                    return sourceDt;

                int sDefultValueColumnIndex = sourceDt.Columns.IndexOf(sDefultValueColumn);
                int sTargetColumnNameIndex = sourceDt.Columns.IndexOf(sTargetColumnName);
                int[] sBaseColumnIndex = new int[sBaseColumnName.Length];

                for (int i = 0; i < sBaseColumnName.Length; i++)
                    sBaseColumnIndex[i] = sourceDt.Columns.IndexOf(sBaseColumnName[i]);

                dtResult = sourceDt.Clone();
                lsTargetColumns = new List<string>();
                lsBaseColumns = new List<string>[sBaseColumnName.Length];
                for (int i = 0; i < lsBaseColumns.Length; i++)
                    lsBaseColumns[i] = new List<string>();

                foreach (DataRow dr in sourceDt.Rows)
                {

                    if (lsBaseColumns[0].IndexOf(dr[sBaseColumnIndex[0]].ToString()) < 0)
                    {
                        lsBaseColumns[0].Add(dr[sBaseColumnIndex[0]].ToString());
                        for (int i = 1; i < sBaseColumnIndex.Length; i++)
                            lsBaseColumns[i].Add(dr[sBaseColumnIndex[i]].ToString());
                    }

                    if (lsTargetColumns.IndexOf(dr[sTargetColumnNameIndex].ToString()) < 0)
                        lsTargetColumns.Add(dr[sTargetColumnNameIndex].ToString());
                }

                for (int i = 0; i < lsTargetColumns.Count; i++)
                {
                    if (dt != null) dt.Reset();

                    System.Data.DataRow[] drSelect = sourceDt.Select(string.Format("[{0}] = '{1}'", sTargetColumnName, lsTargetColumns[i]));
                    if (drSelect.Length < 1)
                        dt = sourceDt.Clone();
                    else
                    {
                        dt = drSelect.CopyToDataTable<DataRow>();
                        foreach (DataRow row in drSelect)
                        {
                            dt.ImportRow(row);
                        }
                        dt.AcceptChanges();
                    }
                       

                    for (int j = 0; j < lsBaseColumns[0].Count; j++)
                    {
                        if (dt.Select(string.Format("[{0}] = '{1}'", sBaseColumnName[0], lsBaseColumns[0][j])).Length < 1)
                        {
                            DataRow dr = dt.NewRow();
                            dr.ItemArray = dt.Rows[0].ItemArray;
                            for (int k = 0; k < sBaseColumnIndex.Length; k++)
                            {
                                dr[sBaseColumnIndex[k]] = lsBaseColumns[k][j];
                            }

                            dr[sDefultValueColumn] = "0";
                            dt.Rows.Add(dr.ItemArray);
                            //dt.AcceptChanges();
                        }
                    }

                    dtResult.Merge(dt);
                    dtResult.AcceptChanges();
                }
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
                if (dtResult != null) dtResult.Dispose();
                dtResult = null;
                if (lsTargetColumns != null) lsTargetColumns.Clear();
                lsTargetColumns = null;
                if (lsBaseColumns != null)
                {
                    foreach (List<string> list in lsBaseColumns)
                    {
                        if (list != null) list.Clear();
                    }
                }
                lsBaseColumns = null;
            }
        }

        public static DataTable ConvertTable(DataTable sourceDt, string[] sBaseColumnName, string sTargetColumnName, string[] sDefultValueColumn)
        {
            DataTable dt = null;
            DataTable dtResult = null;
            List<string> lsTargetColumns = null;
            List<string[]> lsBaseColumns = null;
            try
            {
                if (sourceDt.Rows.Count < 1)
                    return sourceDt;

                int sTargetColumnNameIndex = sourceDt.Columns.IndexOf(sTargetColumnName);
                int[] sBaseColumnIndex = new int[sBaseColumnName.Length];

                dtResult = sourceDt.Clone();
                lsTargetColumns = new List<string>();
                lsBaseColumns = new List<string[]>();

                for (int i = 0; i < sBaseColumnName.Length; i++)
                    sBaseColumnIndex[i] = sourceDt.Columns.IndexOf(sBaseColumnName[i]);

                foreach (DataRow dr in sourceDt.Rows)
                {
                    string sRow = string.Empty;

                    bool IsInsert = true;

                    for (int i = 0; i < lsBaseColumns.Count; i++)
                    {
                        for (int j = 0; j < sBaseColumnIndex.Length; j++)
                        {
                            IsInsert = false;
                            if (lsBaseColumns[i][j] != dr[sBaseColumnIndex[j]].ToString())
                            {
                                IsInsert = true;
                                break;
                            }
                        }
                    }

                    if (IsInsert)
                    {
                        string[] sItem = new string[sBaseColumnIndex.Length];
                        for (int i = 0; i < sBaseColumnIndex.Length; i++)
                            sItem[i] = dr[sBaseColumnIndex[i]].ToString();

                        lsBaseColumns.Add(sItem);
                    }

                    if (lsTargetColumns.IndexOf(dr[sTargetColumnNameIndex].ToString()) < 0)
                        lsTargetColumns.Add(dr[sTargetColumnNameIndex].ToString());
                }

                for (int i = 0; i < lsTargetColumns.Count; i++)
                {
                    if (dt != null) dt.Reset();

                    DataRow[] drSelect = sourceDt.Select(string.Format("[{0}] = '{1}'", sTargetColumnName, lsTargetColumns[i]));
                    if (drSelect.Length < 1)
                        dt = sourceDt.Clone();
                    else
                    {
                        dt = drSelect.CopyToDataTable<DataRow>();
                        foreach (DataRow row in drSelect)
                        {
                            dt.ImportRow(row);
                        }
                        dt.AcceptChanges();
                    }
                       

                    for (int j = 0; j < lsBaseColumns.Count; j++)
                    {
                        string sCondition = string.Empty;

                        for (int k = 0; k < sBaseColumnName.Length; k++)
                        {
                            sCondition += string.Format("[{0}] = '{1}'", sBaseColumnName[k].Replace("'", "''"), lsBaseColumns[j][k].Replace("'", "''"));

                            if (k != sBaseColumnName.Length - 1)
                                sCondition += " AND ";
                        }

                        if (dt.Select(sCondition).Length < 1)
                        {
                            DataRow dr = dt.NewRow();
                            dr.ItemArray = dt.Rows[0].ItemArray;
                            for (int k = 0; k < sBaseColumnIndex.Length; k++)
                            {
                                dr[sBaseColumnIndex[k]] = lsBaseColumns[j][k];
                            }

                            for (int col = 0; col < sDefultValueColumn.Length; col++)
                                dr[sDefultValueColumn[col]] = "0";

                            dt.Rows.Add(dr.ItemArray);
                            //dt.AcceptChanges();
                        }
                    }

                    dtResult.Merge(dt);
                    dtResult.AcceptChanges();
                }
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
                if (dtResult != null) dtResult.Dispose();
                dtResult = null;
                if (lsTargetColumns != null) lsTargetColumns.Clear();
                lsTargetColumns = null;
                if (lsBaseColumns != null) lsBaseColumns.Clear();
                lsBaseColumns = null;
            }
        }

        /*
          /// <summary>
        /// 피벗 메서드.
        /// 소스 데이타를 특정 컬럼 기준으로 컬럼으로 확장 후 데이타를 확장컬럼에 넣어서 반환해줌. 
        /// 추가 : 챠트에 쓰기 위해 NO를 추가해서 넘버링 함. ( 기준 BindingX 값으로 사용함 )
        /// 추가 : 나머지 컬럼은 모두 제자리.
        /// </summary>
        /// <param name="sourceTb"> 변환 될 데이타 테이블</param>
        /// <param name="sBaseColumnName"> 컬럼변경 데이타를 가진 컬럼명</param>
        /// <param name="sBaseHeaderColumnName"> 컬럼에 대한 헤더컬럼.</param>
        /// <param name="RotColumnName"> 옮겨질 데이타. </param>
        /// <returns></returns>
        DataTable Pivot(DataTable sourceTb, string sBaseColumnName, string sBaseHeaderColumnName, string RotColumnName)
        {
            DataTable dt = new DataTable();
            int sBaseColumnIndex = sourceTb.Columns.IndexOf(sBaseColumnName);
            int sBaseHeaderColumnIndex = sourceTb.Columns.IndexOf(sBaseHeaderColumnName);
            int sRotColumnIndex = sourceTb.Columns.IndexOf(RotColumnName);

            dt.Columns.Add("_NO");
            for (int colIndex = 0; colIndex < sourceTb.Columns.Count; colIndex++)
            {
                if (sBaseColumnIndex == colIndex || sRotColumnIndex == colIndex || sBaseHeaderColumnIndex == colIndex) continue;
                dt.Columns.Add(string.Format("_{0}", sourceTb.Columns[colIndex].ColumnName));
            }

            string ColName = string.Empty;

            foreach (DataRow sBaseColumnRow in sourceTb.Rows)
            {
                ColName = sBaseColumnRow[sBaseColumnIndex].ToString();
                if (!dt.Columns.Contains(string.Format("_{0}", ColName)))
                {
                    dt.Columns.Add(string.Format("_{0}", ColName));
                    if (sBaseHeaderColumnIndex >= 0)
                        dt.Columns[dt.Columns.Count - 1].Caption = sBaseColumnRow[sBaseHeaderColumnIndex].ToString();
                }
            }
            int offsetCol = sBaseHeaderColumnIndex == -1 ? 2 : 3;

            // Row 한번에 뛰어넘을 간격을 계산함. 
            int step = dt.Columns.Count - (sourceTb.Columns.Count - offsetCol) - 1; // 1은 NO 컬럼 몫.

            if (step <= 0)
            {
                throw new Exception("RowIndex 계산증가값은 0이하가 될수 없습니다.");
            }

            List<object> values = new List<object>();
            for (int rowIndex = 0; rowIndex < sourceTb.Rows.Count; rowIndex += step)
            {
                values.Add(dt.Rows.Count + 1);
                for (int colIndex = 0; colIndex < sourceTb.Columns.Count; colIndex++)
                {
                    if (sBaseColumnIndex == colIndex || sRotColumnIndex == colIndex || sBaseHeaderColumnIndex == colIndex) continue;

                    values.Add(sourceTb.Rows[rowIndex][colIndex]); // 기존 컬럼 복사
                }

                for (int i = 0; i < step; i++) // 세로로 된 기준 컬럼데이타를 가로로 복사함. 
                {
                    if (sourceTb.Rows.Count <= rowIndex + i) break;
                    values.Add(sourceTb.Rows[rowIndex + i][sRotColumnIndex]);
                }
                dt.Rows.Add(values.ToArray<object>());
                values.Clear();
            }
            return dt;
        }
         */


        #region [ Sheeet ]

        #region [ InitSpread ]
        public static void InitSpread(params FarPoint.Win.Spread.FpSpread[] spreadList)
        {
            try
            {
                FarPoint.Win.Spread.CellType.ColumnHeaderRenderer renderer = new FarPoint.Win.Spread.CellType.ColumnHeaderRenderer();
                renderer.WordWrap = false;

                foreach (FarPoint.Win.Spread.FpSpread fp in spreadList)
                {
                    foreach (FarPoint.Win.Spread.SheetView sv in fp.Sheets)
                    {
                        sv.Reset();

                        sv.ColumnHeader.Rows[0].Renderer = renderer;
                        sv.DataAutoSizeColumns = false;
                        sv.DataAutoCellTypes = false;

                        sv.ColumnCount = 0;
                        sv.RowCount = 0;

                        sv.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                        sv.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

                        FarPoint.Win.Spread.DefaultSkins.Classic2.Apply(sv);
                    }
                    fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                    fp.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                    fp.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
                    fp.ScrollTipPolicy = FarPoint.Win.Spread.ScrollTipPolicy.Both;

                    FarPoint.Win.Spread.DefaultSkins.Classic2.Apply(fp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region [ SetSpreadData ]
        public static void SetSpreadData(DataTable dt, FarPoint.Win.Spread.SheetView sheet, int minColumnWidth, int defultPlaces, bool IsColumnFixed = false, bool IsShowSeparator = true)
        {
            try
            {
                sheet.DataSource = dt;

                for (int c = 0; c < sheet.ColumnCount; c++)
                {
                    // DataType Setting
                    switch (dt.Columns[c].DataType.Name.ToUpper())
                    {
                        case "CHAR":
                        case "STRING":
                            FarPoint.Win.Spread.CellType.TextCellType text = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[c].CellType = text;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "FLOAT":
                        case "DOUBLE":
                        case "DECIMAL":
                            FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                            num.DecimalPlaces = defultPlaces;
                            num.ShowSeparator = IsShowSeparator;
                            num.MaximumValue = 99999999999999;
                            sheet.Columns[c].CellType = num;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "INT32":
                            FarPoint.Win.Spread.CellType.NumberCellType intType = new FarPoint.Win.Spread.CellType.NumberCellType();
                            intType.DecimalPlaces = 0;
                            intType.ShowSeparator = IsShowSeparator;
                            sheet.Columns[c].CellType = intType;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "DATETIME":
                            FarPoint.Win.Spread.CellType.DateTimeCellType date = new FarPoint.Win.Spread.CellType.DateTimeCellType();

                            date.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.ShortDateWithTime;
                            sheet.Columns[c].CellType = date;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show(string.Format("Not define {0}", dt.Columns[c].DataType.Name), "SetSpreadData"
                                                                    , System.Windows.Forms.MessageBoxButtons.OK
                                                                    , System.Windows.Forms.MessageBoxIcon.Information);
                            FarPoint.Win.Spread.CellType.TextCellType type = new FarPoint.Win.Spread.CellType.TextCellType();
                            sheet.Columns[c].CellType = type;
                            sheet.Columns[c].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            sheet.Columns[c].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                    }
                    if (IsColumnFixed == true)
                        sheet.Columns[c].Width = minColumnWidth;
                    else
                        sheet.Columns[c].Width = (sheet.GetPreferredColumnWidth(c, true, false) > minColumnWidth ? (sheet.GetPreferredColumnWidth(c, true, true) + 15) : minColumnWidth);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region [ SetSheetColumnWidth ]
        public static void SetSheetColumnWidth(params FarPoint.Win.Spread.SheetView[] sheetList)
        {
            try
            {
                foreach (FarPoint.Win.Spread.SheetView sv in sheetList)
                {
                    for (int i = 0; i < sv.Columns.Count; i++)
                    {
                        sv.Columns[i].Width = sv.GetPreferredColumnWidth(i, true, true) + 15;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region [ SetDecimalPlaces ]
        public static void SetDecimalPlaces(FarPoint.Win.Spread.SheetView sheet, int[] arrColIndex, int[] arrDecimalPlaces, bool IsShowSeparator = true)
        {
            try
            {
                for (int i = 0; i < arrColIndex.Length; i++)
                {
                    if (sheet.Columns.Count <= arrColIndex[i])
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Input value is too large [{0}]", arrColIndex[i]), "Notice"
                                                                    , System.Windows.Forms.MessageBoxButtons.OK
                                                                    , System.Windows.Forms.MessageBoxIcon.Information);
                        continue;
                    }

                    FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                    num.DecimalPlaces = arrDecimalPlaces[i];
                    num.ShowSeparator = IsShowSeparator;
                    num.MaximumValue = 99999999999999;
                    sheet.Columns[arrColIndex[i]].CellType = num;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        public static void SetColumnTypeNumber(ref FarPoint.Win.Spread.FpSpread Spread,
                                                int nStartIndex, int nDecimalPlaces)
        {
            try
            {
                FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                num.DecimalPlaces = nDecimalPlaces;
                num.ShowSeparator = true;
                num.MaximumValue = 99999999999999;
                for (int i = nStartIndex - 1; i < Spread.ActiveSheet.Columns.Count; i++)
                {
                    Spread.ActiveSheet.Columns[i].CellType = num;
                    //Spread.ActiveSheet.Cells[0, i, Spread.ActiveSheet.Rows.Count - 1, i].CellType = num;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetCurrencyCellType(ref FarPoint.Win.Spread.FpSpread Spread,
                                                int nStartIndex, int nDecimalPlaces)
        {
            try
            {
                FarPoint.Win.Spread.CellType.CurrencyCellType ct = new FarPoint.Win.Spread.CellType.CurrencyCellType();
                ct.DecimalPlaces = nDecimalPlaces;
                ct.ShowSeparator = true;

                for (int i = nStartIndex - 1; i < Spread.ActiveSheet.Columns.Count; i++)
                {
                    Spread.ActiveSheet.Columns[i].CellType = ct;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetColumnTypeNumber(ref FarPoint.Win.Spread.FpSpread Spread,
                                                int nStartIndex, int nSetCount, int nDecimalPlaces)
        {
            try
            {
                FarPoint.Win.Spread.CellType.NumberCellType num = new FarPoint.Win.Spread.CellType.NumberCellType();
                num.DecimalPlaces = nDecimalPlaces;
                num.ShowSeparator = true;
                num.MaximumValue = 99999999999999;
                for (int i = 0; i < nSetCount; i++)
                {
                    if ((nStartIndex - 1) + i < Spread.ActiveSheet.ColumnCount)
                        continue;
                    Spread.ActiveSheet.Columns[(nStartIndex - 1) + i].CellType = num;
                    //Spread.ActiveSheet.Cells[0, i - (nStartIndex - 1), Spread.ActiveSheet.Rows.Count - 1, i - (nStartIndex - 1)].CellType = num;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ MS Chart ]

        public static void InitMSChart(ref Chart chart)
        {
            chart.Titles.Clear();
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            chart.BackColor = System.Drawing.Color.LightYellow;
            chart.ChartAreas.Add("Default");
            chart.ChartAreas["Default"].BackColor = System.Drawing.Color.LightSkyBlue;
            chart.ChartAreas["Default"].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chart.ChartAreas["Default"].BorderColor = System.Drawing.Color.Black;
            chart.ChartAreas["Default"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chart.ChartAreas["Default"].BorderWidth = 1;
            chart.ChartAreas["Default"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart.ChartAreas["Default"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart.ChartAreas["Default"].AxisY2.MajorGrid.LineColor = System.Drawing.Color.LightGray;

            chart.Titles.Add("Default");
            chart.Titles[0].Alignment = System.Drawing.ContentAlignment.TopCenter;
            chart.Titles[0].Text = chart.Text;

            chart.Legends.Add("Default");
            chart.Legends["Default"].Alignment = System.Drawing.StringAlignment.Center;
            chart.Legends["Default"].BackColor = System.Drawing.Color.LightYellow;
            chart.Legends["Default"].BorderColor = System.Drawing.Color.DarkKhaki;
            chart.Legends["Default"].BorderWidth = 2;
        }

        #endregion

        #region [ Color Util ]
        private static int[] m_iColor = {16766976,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
								        65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
								        16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
								        10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
								        8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
								        6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
								        12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
								        10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
								        2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
								        10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686};

        public static string GetColorString(int iBinNumber)
        {
            string strColor = string.Empty;
            int r = (m_iColor[iBinNumber % 100] >> 16);
            int g = (m_iColor[iBinNumber % 100] >> 8) - (r * 256);
            int b = m_iColor[iBinNumber % 100] - (r * 65536) - (g * 256);

            strColor = string.Format("{0:00#}{1:00#}{2:00#}", r, g, b);
            return strColor;
        }

        public static System.Drawing.Color GetColor(int iBinNumber)
        {
            int r = (m_iColor[iBinNumber % 100] >> 16);
            int g = (m_iColor[iBinNumber % 100] >> 8) - (r * 256);
            int b = m_iColor[iBinNumber % 100] - (r * 65536) - (g * 256);

            return System.Drawing.Color.FromArgb(255, r, g, b);
        }
        #endregion

        #region [ 신규 Table Pivot ]
        public enum PivotColumnForm { Sequential, Groupping };

        /// PivotColumnForm : Sequential
        /// ------------------------------------------------------------------------------------------------------
        ///  sDefaultDataColumns[0]  | sDefaultDataColumns[1]..| sPivotRowColumnName   | sPivotColumnColumns[0].Data01  | sPivotColumnColumns[0].Data02  | sPivotColumnColumns[0].Data03  | sPivotColumnColumns[1].Data01  | sPivotColumnColumns[1].Data02  | sPivotColumnColumns[1].Data03...
        /// ------------------------------------------------------------------------------------------------------
        ///                                                    | sPivotRowColumns[0]   |                                |                                |                                | ...
        ///                                                    | sPivotRowColumns[1]   |                                |                                |                                | ...
        ///                                                    | sPivotRowColumns[2]   |                                |                                |                                | ...
        ///                                                    | sPivotRowColumns[3]   |                                |                                |                                | ...
        ///                                                    | ...                   |                                |                                |                                | ...
        ///                                                    
        /// /// PivotColumnForm : Groupping
        /// ------------------------------------------------------------------------------------------------------
        ///  sDefaultDataColumns[0]  | sDefaultDataColumns[1]..| sPivotRowColumnName   | sPivotColumnColumns[0].Data01;sPivotColumnColumns[1].Data01  | sPivotColumnColumns[0].Data02;sPivotColumnColumns[1].Data02  | sPivotColumnColumns[0].Data03;sPivotColumnColumns[1].Data03...
        /// ------------------------------------------------------------------------------------------------------
        ///                                                    | sPivotRowColumns[0]   |                                                              |                                                              |                                                          | ...
        ///                                                    | sPivotRowColumns[1]   |                                                              |                                                              |                                                          | ...
        ///                                                    | sPivotRowColumns[2]   |                                                              |                                                              |                                                          | ...
        ///                                                    | sPivotRowColumns[3]   |                                                              |                                                              |                                                          | ...
        ///                                                    | ...                   |                                                              |                                                              |                                                          | ...

        /// <summary>
        /// DataTable Pivot
        /// 
        /// </summary>
        /// <param name="sourceDt">원본 데이터</param>
        /// <param name="sDefaultDataColumns">변환시 기본 테이터 컬럼s</param>
        /// <param name="sPivotRowColumns">행으로 변환될 데이터 컬럼s</param>
        /// <param name="sPivotColumnColumns">열로 변환될 데이터 컬럼s</param>
        /// <param name="emPivotType">변환시 열의 형태</param>
        /// <param name="sPivotRowColumnName">행으로 변환될 컬럼들의 컬럼이름</param>
        /// <param name="bCreatePivotRowsColumn">행으로 변환될 컬럼들의 컬럼 생성여부</param>
        /// <returns></returns>
        public static DataTable TablePivot2(DataTable sourceDt
                                          , string[] sDefaultDataColumns
                                          , string[] sPivotRowColumns
                                          , string[] sPivotColumnColumns
                                          , PivotColumnForm emPivotType
                                          , string sPivotRowColumnName = "VALUE"
                                          , bool bCreatePivotRowsColumn = true)
        {
            DataTable dt = null;

            try
            {
                // 원본 데이터가 없으면 Null 반환
                if (sourceDt == null || sourceDt.Rows.Count < 1)
                    return null;

                // 기본이 되는 TargetRow가 없으면 Null 반환
                //if (sDefaultDataColumns == null || sDefaultDataColumns.Length < 1)
                //    return null;

                // Pivot 대상 데이터가 없으면 Null 반환
                if (sPivotRowColumns == null || sPivotRowColumns.Length < 1)
                    return null;

                // ------------------------------------------
                // Pivot  시작
                dt = new DataTable();

                // sTargetRows 컬럼 생성
                foreach (string sDefaultColumns in sDefaultDataColumns)
                {
                    if (sourceDt.Columns.Contains(sDefaultColumns) == false)
                        throw new Exception(string.Format("원본 데이터에 없는 컬럼 이름을 지정하셨습니다. [{0}]", sDefaultColumns));

                    dt.Columns.Add(sDefaultColumns, sourceDt.Columns[sDefaultColumns].DataType);
                }

                // 컬럼 생성 여부 또는 sPivotRows가 1개이상 일때 sPivotRows 컬럼 생성
                if (bCreatePivotRowsColumn == true || sPivotRowColumns.Length > 1)
                    dt.Columns.Add(sPivotRowColumnName, typeof(string));

                // sPivotCols의 컬럼에서 데이터를 추출하여 정렬 후 컬럼을 생성한다.
                DataRow[] drCol = null;
                switch (emPivotType)
                {
                    case PivotColumnForm.Sequential:
                        foreach (string sPivotColumn in sPivotColumnColumns)
                        {
                            if (sourceDt.Columns.Contains(sPivotColumn) == false)
                                throw new Exception(string.Format("원본 데이터에 없는 컬럼 이름을 지정하셨습니다. [{0}]", sPivotColumn));

                            drCol = sourceDt.DefaultView.ToTable(true, sPivotColumn).Select("1=1", sPivotColumn);

                            foreach (DataRow dr in drCol)
                                dt.Columns.Add(dr[0].ToString(), sourceDt.Columns[sPivotRowColumns[0]].DataType);
                        }
                        break;
                    case PivotColumnForm.Groupping:
                        string sColumnName = string.Empty;
                        foreach (string sPivotColumn in sPivotColumnColumns)
                        {
                            if (sourceDt.Columns.Contains(sPivotColumn) == false)
                                throw new Exception(string.Format("원본 데이터에 없는 컬럼 이름을 지정하셨습니다. [{0}]", sPivotColumn));
                        }

                        drCol = sourceDt.DefaultView.ToTable(true, sPivotColumnColumns).Select("1=1", string.Join(",", sPivotColumnColumns));

                        foreach (DataRow dr in drCol)
                        {
                            string sColumName = string.Empty;

                            for (int i = 0; i < dr.ItemArray.Length; i++)
                                sColumName += ";" + dr[i].ToString();

                            dt.Columns.Add(sColumName.Substring(1), sourceDt.Columns[sPivotRowColumns[0]].DataType);
                        }
                        break;
                }

                // Pivot 데이터 Row 생성
                foreach (DataRow dr in sourceDt.Rows)
                {
                    bool IsInsert = true;

                    // 현재 입력된 데이터 체크
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        bool IsContinue = false;
                        foreach (string sDefaultColumns in sDefaultDataColumns)
                        {
                            // sTargetRow 중 하나라도 다르면 다음 행 비교
                            if (dt.Rows[row][sDefaultColumns].ToString() != dr[sDefaultColumns].ToString())
                            {
                                IsContinue = true;
                                break;
                            }
                        }

                        if (IsContinue)
                            continue;

                        for (int i = 0; i < sPivotRowColumns.Length; i++)
                        {
                            switch (emPivotType)
                            {
                                case PivotColumnForm.Sequential:
                                    for (int j = 0; j < sPivotColumnColumns.Length; j++)
                                        dt.Rows[row + i][dr[sPivotColumnColumns[j]].ToString()] = dr[sPivotRowColumns[i]];
                                    break;
                                case PivotColumnForm.Groupping:
                                    string sColumnName = string.Empty;
                                    for (int j = 0; j < sPivotColumnColumns.Length; j++)
                                        sColumnName += ";" + dr[sPivotColumnColumns[j]].ToString();
                                    dt.Rows[row + i][sColumnName.Substring(1)] = dr[sPivotRowColumns[i]];
                                    break;
                            }
                        }

                        IsInsert = false;
                        break;
                    }

                    // 새로운 행을 추가한다.
                    if (IsInsert)
                    {
                        foreach (string sPivotRowCol in sPivotRowColumns)
                        {
                            DataRow drRow = dt.NewRow();

                            foreach (string sDefaultColumns in sDefaultDataColumns)
                                drRow[sDefaultColumns] = dr[sDefaultColumns];

                            if (bCreatePivotRowsColumn == true || sPivotRowColumns.Length > 1)
                                drRow[sPivotRowColumnName] = sPivotRowCol;

                            switch (emPivotType)
                            {
                                case PivotColumnForm.Sequential:
                                    foreach (string sPivotColumn in sPivotColumnColumns)
                                        drRow[dr[sPivotColumn].ToString()] = dr[sPivotRowCol];
                                    break;
                                case PivotColumnForm.Groupping:
                                    string sColumnName = string.Empty;
                                    foreach (string sPivotColumn in sPivotColumnColumns)
                                        sColumnName += ";" + dr[sPivotColumn].ToString();
                                    drRow[sColumnName.Substring(1)] = dr[sPivotRowCol];
                                    break;
                            }

                            dt.Rows.Add(drRow);
                        }
                    }
                }

                dt.AcceptChanges();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

    public class AVIImage
    {

        #region FTP를 List 형태로 받아 Return 해준다.

        public DataTable AVIFTPFileDownload(DataTable FileList, string ModuleName)
        {
            DACrux.Utility.HFtpClient oFTP = null;
            //DACrux.Utility.ServerCommunicationFtp oFTP = null;
            string strLocalFullPath = string.Empty;

            string strFTPFullPath = string.Empty;
            string strLocalPathName = string.Empty;

            string strFTPIP = string.Empty;
            string strFTPPort = string.Empty;
            string strFTPID = string.Empty;
            string strFTPPass = string.Empty;
            //string strFTPMainPath = string.Empty;

            FileInfo oImageFile = null;
            DirectoryInfo oDirectory = null;

            DataTable dtResult = null;

            //DACrux.TEST.RO.DataSelect oData = null;
            ComConfiguration obj = null;

            try
            {
                if (FileList == null || FileList.Rows.Count <= 0)
                    return null;

                dtResult = new DataTable();
                dtResult.Columns.Add(new DataColumn("X", typeof(int)));
                dtResult.Columns.Add(new DataColumn("Y", typeof(int)));
                dtResult.Columns.Add(new DataColumn("BIN", typeof(int)));
                dtResult.Columns.Add(new DataColumn("IMAGE_PATH", typeof(string)));
                dtResult.Columns.Add(new DataColumn("THUMB_PATH", typeof(string)));
                dtResult.AcceptChanges();

                //oData = new RO.DataSelect();
                obj = new ComConfiguration();
                DataTable dtConfig = obj.GetAVIImageFTPInfo();
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                    return null;

                strFTPIP = dtConfig.Rows[0]["IP"].ToString();
                strFTPPort = dtConfig.Rows[0]["PORT"].ToString();
                strFTPID = dtConfig.Rows[0]["ID"].ToString();
                strFTPPass = dtConfig.Rows[0]["PASS"].ToString();
                //strFTPMainPath = dtConfig.Rows[0]["MAIN_PATH"].ToString();

                //Local Directory 삭제 후 생성 (쓰레기 Data 는 미리 삭제..)
                strLocalPathName = System.IO.Path.Combine(Environment.CurrentDirectory, "AVI", ModuleName);
                oDirectory = new DirectoryInfo(strLocalPathName);
                if (oDirectory.Exists)
                    oDirectory.Delete(true);

                oDirectory.Create();

                oFTP = new DACrux.Utility.HFtpClient(strFTPIP, DACrux.Base.Convert.intParse(strFTPPort), strFTPID, strFTPPass);
                bool IsFtpConnect = oFTP.LoginTest();
                if (IsFtpConnect == false)
                    throw new Exception("FTP에 접속할 수 없습니다.");

                //oFTP = new Utility.ServerCommunicationFtp();
                //oFTP.Server = strFTPIP;
                //oFTP.Port = DACrux.Base.Convert.intParse(strFTPPort);
                //oFTP.UserID = strFTPID;
                //oFTP.Password = strFTPPass;
                //oFTP.ChmodValue = 777;

                //Image 및 Thumb File 을 Download 받는다.
                string strFileName = string.Empty;
                string strFilePath = string.Empty;
                for (int f = 0; f < FileList.Rows.Count; f++)
                {
                    DataRow drNew = dtResult.NewRow();
                    drNew["X"] = FileList.Rows[f]["X"];
                    drNew["Y"] = FileList.Rows[f]["Y"];
                    drNew["BIN"] = FileList.Rows[f]["BIN"];

                    //File Name 가져 오는 부분
                    strFileName = FileList.Rows[f]["IMAGE_FILE_NAME"].ToString();
                    strFilePath = FileList.Rows[f]["IMAGE_PATH"].ToString();
                    strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFileName);

                    if (strFilePath.StartsWith("BACKUP") == true)
                        strFTPFullPath = System.IO.Path.Combine(strFilePath, strFileName).Replace("\\", "/");
                    else
                        strFTPFullPath = System.IO.Path.Combine("BACKUP/", strFilePath, strFileName).Replace("\\", "/");

                    //oFTP.ReceiveFile(strFTPFullPath, strLocalFullPath);
                    oFTP.Down(strFTPFullPath, strLocalFullPath);
                    oImageFile = new FileInfo(strLocalFullPath);
                    if (!oImageFile.Exists)
                        throw new Exception("AVI Image File을 Download 하였으나 존재 하지 않습니다.");

                    drNew["IMAGE_PATH"] = strLocalFullPath;

                    //Thumb File Name 가져 오는 부분
                    if (string.IsNullOrEmpty(FileList.Rows[f]["THUMB_FILENAME"].ToString()) == false)
                    {
                        strFileName = FileList.Rows[f]["THUMB_FILENAME"].ToString();
                        strFilePath = FileList.Rows[f]["THUMB_PATH"].ToString();
                        strLocalFullPath = System.IO.Path.Combine(strLocalPathName, strFileName);
                        strFTPFullPath = System.IO.Path.Combine(strFilePath, strFileName).Replace("\\", "/");
                        //oFTP.ReceiveFile(strFTPFullPath, strLocalFullPath); 
                        oFTP.Down(strFTPFullPath, strLocalFullPath);
                        oImageFile = new FileInfo(strLocalFullPath);
                        if (!oImageFile.Exists)
                            throw new Exception("AVI Image File을 Download 하였으나 존재 하지 않습니다.");

                        drNew["THUMB_PATH"] = strLocalFullPath;
                    }

                    dtResult.Rows.Add(drNew);
                }
                dtResult.AcceptChanges();
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        #endregion
    }

    public class MultiKeyDictionary<K1, K2, V> : Dictionary<K1, Dictionary<K2, V>>
    {
        public V this[K1 key1, K2 key2]
        {
            get
            {
                if (!ContainsKey(key1) || !this[key1].ContainsKey(key2))
                    throw new ArgumentOutOfRangeException();
                return base[key1][key2];
            }
            set
            {
                if (!ContainsKey(key1))
                    this[key1] = new Dictionary<K2, V>();
                this[key1][key2] = value;
            }
        }
        public void Add(K1 key1, K2 key2, V value)
        {
            if (!ContainsKey(key1))
                this[key1] = new Dictionary<K2, V>();
            this[key1][key2] = value;
        }
        public bool ContainsKey(K1 key1, K2 key2)
        {
            return base.ContainsKey(key1) && this[key1].ContainsKey(key2);
        }
        public new IEnumerable<V> Values
        {
            get
            {
                return from baseDict in base.Values
                       from baseKey in baseDict.Keys
                       select baseDict[baseKey];
            }
        }
    }

    public class MultiKeyDictionary<K1, K2, K3, V> : Dictionary<K1, MultiKeyDictionary<K2, K3, V>>
    {
        public V this[K1 key1, K2 key2, K3 key3]
        {
            get
            {
                return ContainsKey(key1) ? this[key1][key2, key3] : default(V);
            }
            set
            {
                if (!ContainsKey(key1))
                    this[key1] = new MultiKeyDictionary<K2, K3, V>();
                this[key1][key2, key3] = value;
            }
        }
        public bool ContainsKey(K1 key1, K2 key2, K3 key3)
        {
            return base.ContainsKey(key1) && this[key1].ContainsKey(key2, key3);
        }
        public void Add(K1 key1, K2 key2, K3 key3, V value)
        {
            if (!ContainsKey(key1))
                this[key1] = new MultiKeyDictionary<K2, K3, V>();
            this[key1][key2, key3] = value;
        }
    }

}
