using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using DACrux.BStats.Statistics;

namespace DACrux.BStats
{
    public class DataTableUtil
    {
        #region " MEMBER FIELD "
        private static string m_Separator = "--";//"ĦœǼҝҖ";
        private static string m_ErrorColNameNotEqual = "Length of ColumnNames doesn't equal";

        #endregion

        #region " PROPERTY "
        /// <summary>
        /// 여러 컬럼으로 그룹핑 될 경우 출력 테이블의 컬럼명에 사용될 구분자
        /// </summary>
        public static string SEPARATOR
        {
            set
            {
                m_Separator = value;
            }
            get
            {
                return m_Separator;
            }
        }
        #endregion

        #region ColumnEqual
        /// <summary>
        /// 전달받은 두 Object를 비교하여 동일한지의 여부를 반환한다.
        /// </summary>
        /// <param name="A">비교할 Object #1</param>
        /// <param name="B">비교할 Object #2</param>
        /// <returns></returns>
        private static bool ColumnEqual(object[] A, object[] B)
        {
            bool bResult = true;
            try
            {
                for (int i = 0; i < A.Length; i++)
                {
                    if (A[i] == DBNull.Value && B[i] == DBNull.Value) //  both are DBNull.Value
                        bResult = bResult && true;
                    else if (A[i] == DBNull.Value || B[i] == DBNull.Value) //  only one is DBNull.Value
                        bResult = bResult && false;
                    else if (A[i] == null && B[i] == null) //  both are DBNull.Value
                        bResult = bResult && true;
                    else if (A[i] == null || B[i] == null) //  only one is DBNull.Value
                        bResult = bResult && false;
                }

                if (!bResult) return false;

                for (int j = 0; j < A.Length; j++)
                {

                    if (A[j].Equals(B[j])) bResult = bResult && true;
                    else bResult = bResult && false;
                }

            }
            catch
            {
            }

            return bResult;  // value type standard comparison
        }

        #endregion

        #region SelectGroupBy
        /// <summary>
        /// 전달받은 테이블의 특정컬럼의 Group By 목록을 구해서 반환한다.
        /// </summary>
        /// <param name="TableName">반환할 테이블명</param>
        /// <param name="SourceTable">Source Table</param>
        /// <param name="FieldNames">Group By 를 적용할 컬럼목록 - 공백없이 쉼표(,)로 구분</param>
        /// <returns>Group BY 목록을 저장한 테이블</returns>
        public static DataTable SelectGroupBy(string TableName, DataTable SourceTable, string FieldNames)
        {
            string[] FieldName = null;
            object[] LastValue = null;
            object[] CurrValue = null;
            DataTable dt = new DataTable(TableName);
            try
            {
                FieldName = FieldNames.Split(',');

                LastValue = new object[FieldName.Length];
                CurrValue = new object[FieldName.Length];

                for (int i = 0; i < FieldName.Length; i++)
                {
                    dt.Columns.Add(FieldName[i], SourceTable.Columns[FieldName[i]].DataType);
                }

                dt.Columns.Add("COUNT", System.Type.GetType("System.Int32"));

                foreach (DataRow dr in SourceTable.Select("", FieldNames))
                {
                    for (int col = 0; col < FieldName.Length; col++)
                    {
                        CurrValue[col] = dr[FieldName[col]];
                    }

                    if (LastValue == null || !(ColumnEqual(LastValue, CurrValue)))
                    {
                        CurrValue.CopyTo(LastValue, 0);
                        dt.Rows.Add(LastValue);
                        dt.Rows[dt.Rows.Count - 1]["COUNT"] = 1;
                    }
                    else
                    {
                        dt.Rows[dt.Rows.Count - 1]["COUNT"] = (int)(dt.Rows[dt.Rows.Count - 1]["COUNT"]) + 1;
                    }
                }

            }
            catch
            {
            }
            finally
            {
                if (FieldName != null) FieldName = null;
                if (LastValue != null) LastValue = null;
                if (CurrValue != null) CurrValue = null;
                if (dt != null) dt.Dispose();
            }

            return dt;
        }

        #endregion

        #region SelectMinMax
        /// <summary>
        /// 전달받은 테이블의 특정컬럼의 Min, Max 값을 구한다.
        /// </summary>
        /// <param name="SourceTable">Source Table</param>
        /// <param name="FieldName">Min Max를 구할 컬럼</param>
        /// <param name="dblMin">내보낼 최소값 (Object[])</param>
        /// <param name="dblMax">내보낼 최대값 (Object[])</param>
        public static void SelectMinMax(DataTable SourceTable, string FieldName, out double dblMin, out double dblMax)
        {
            DataRow[] dr = SourceTable.Select("", FieldName);

            if (dr[0][FieldName] == DBNull.Value || dr[0][FieldName].ToString() == "")
                dblMin = 0;
            else
                dblMin = double.Parse(dr[0][FieldName].ToString());

            if (dr[dr.Length - 1][FieldName] == DBNull.Value || dr[dr.Length - 1][FieldName].ToString() == "")
                dblMax = double.Parse(dr[dr.Length - 1][FieldName].ToString());
            else
                dblMax = 0;
        }
        #endregion

        #region GetDecimalDigit
        /// <summary>
        /// 전달받은 테이블의 특정컬럼의 평균적인 소수점 자리수를 추측하여 구한다.
        /// </summary>
        /// <param name="SourceTable">Source Table</param>
        /// <param name="FieldName">자리수를 구할 컬럼</param>
        /// <param name="DecimalDigit">내보낼 소수점 자리수 </param>
        public static void GetDigit(DataTable SourceTable, string FieldName, out int DecimalDigit)
        {
            int RowDev;
            double TempSUM;
            int i;
            int iDev = 10;

            if (SourceTable.Rows.Count > 10)
            {
                iDev = 10;
            }
            else
            {
                iDev = SourceTable.Rows.Count;
            }

            RowDev = (int)Math.Round((double)(SourceTable.Rows.Count / iDev));
            i = 0;
            TempSUM = 0;

            while (i < SourceTable.Rows.Count)
            {
                if (SourceTable.Rows[i][FieldName] != DBNull.Value && SourceTable.Rows[i][FieldName].ToString().Trim() != "")
                    TempSUM += double.Parse(SourceTable.Rows[i][FieldName].ToString());

                i += RowDev;
            }

            if ((double)((int)TempSUM) == TempSUM)
            {
                DecimalDigit = 0;
            }
            else
            {
                DecimalDigit = TempSUM.ToString().Length - TempSUM.ToString().IndexOf(".", 0, TempSUM.ToString().Length) - 1;
            }

        }
        #endregion

        #region " NEW METHOD FOR STATISTICAL ANALYSYS"

        #region [ Common ]

        public static string MakeColumnName(object[] DataRow, string Separator)
        {
            string[] arrTemp = null;
            int iCnt;
            try
            {
                if (DataRow == null)
                    throw new ArgumentException(ErrorParameter("DataRow"));
                iCnt = DataRow.Length;
                arrTemp = new string[iCnt];
                for (int i = 0; i < iCnt; i++)
                    arrTemp[i] = DataRow[i].ToString();
                if (string.Join(Separator, arrTemp) == string.Empty)
                {
                    return "Not_Asigned ";
                }
                else
                {
                    return string.Join(Separator, arrTemp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public static string MakeString(string[] ColumnNames, object[] DataRow, string Separator)
        {
            string strReturn;
            int iColNameCnt;
            int iColCnt;
            try
            {
                if (ColumnNames == null || DataRow == null)
                    throw new ArgumentException(ErrorParameter("ColumnNames, DataRow"));
                iColNameCnt = ColumnNames.Length;
                iColCnt = DataRow.Length;
                if (iColNameCnt != iColCnt)
                    throw new ArgumentException(m_ErrorColNameNotEqual);
                if (iColNameCnt == 1)
                {   
                    if (DataRow[0].ToString().Trim() == string.Empty)
                        return string.Format("TRIM([" + ColumnNames[0] + "])" + " = '' OR [" + ColumnNames[0] + "] IS NULL");
                    else
                        return "[" + ColumnNames[0] + "] = '" + DataRow[0].ToString() + "'";
                }
                else
                {
                    if (DataRow[0].ToString().Trim() == string.Empty)
                    {
                        strReturn = string.Format("(TRIM([" + ColumnNames[0] + "])" + " = '' OR [" + ColumnNames[0] + "] IS NULL)");
                    }
                    else
                        strReturn = "[" + ColumnNames[0] + "] = '" + DataRow[0].ToString() + "'";
                    for (int i = 1; i < iColNameCnt; i++)
                    {
                        if (DataRow[i].ToString().Trim() == string.Empty)
                            strReturn += Separator + string.Format("(TRIM([" + ColumnNames[i] + "])" + " = '' OR [" + ColumnNames[i] + "] IS NULL)");
                        else
                            strReturn += Separator + "[" + ColumnNames[i] + "] = '" + DataRow[i].ToString() + "'";
                    }
                }               
                return strReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string[] GetColumnName(DataTable dt, int[] arrColumnIndex)
        {
            string[] arrReturn = null;
            int iCnt;
            try
            {
                if (arrColumnIndex == null)
                    throw new ArgumentException(ErrorParameter("arrColumnIndex"));
                if (dt == null)
                    throw new ArgumentException(ErrorParameter("dt"));
                iCnt = arrColumnIndex.Length;

                arrReturn = new string[iCnt];
                for (int i = 0; i < iCnt; i++)
                {
                    arrReturn[i] = dt.Columns[arrColumnIndex[i]].ColumnName;
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrReturn = null;
            }
        }

        public static string[] GetColumnName(DataTable dt)
        {
            string[] arrReturn = null;
            int iCnt;
            try
            {
                if (dt == null)
                    throw new ArgumentException(ErrorParameter("dt"));
                iCnt = dt.Columns.Count;

                arrReturn = new string[iCnt];
                for (int i = 0; i < iCnt; i++)
                {
                    arrReturn[i] = dt.Columns[i].ColumnName;
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrReturn = null;
            }
        }

        public static string[] GetStringArray(object[] arrObj)
        {
            int iCnt;
            string[] arrReturn = null;
            try
            {
                if (arrObj == null)
                    return null;
                iCnt = arrObj.Length;
                arrReturn = new string[iCnt];
                for (int i = 0; i < iCnt; i++)
                {
                    arrReturn[i] = arrObj[i].ToString();
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ SELECT WHERE ]



        #region New Distinct

        public static DataTable SelectDistinctNoSort(string TableName, DataTable SourceTable, int[] FieldIndex)
        {
            try
            {
                return SelectDistinctNoSort(TableName, SourceTable, FieldIndex, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SelectDistinctNoSort(string TableName, DataTable SourceTable, int[] FieldIndex, bool bSort)
        {
            string[] arrColumnName = null;
            int iColumnCnt;
            try
            {
                if (FieldIndex == null)
                    throw new ArgumentException(ErrorParameter("FieldIndex"));
                if(SourceTable == null)
                    throw new ArgumentException(ErrorParameter("SourceTable"));
                iColumnCnt = FieldIndex.Length;
                arrColumnName = GetColumnName(SourceTable, FieldIndex);                
                if (bSort)
                    SourceTable.DefaultView.Sort = string.Join(",", arrColumnName);
                return SourceTable.DefaultView.ToTable(TableName, true, arrColumnName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrColumnName = null;
            }
        }        
        #endregion

        #region New Select Where


        public static DACruxTable SelectWhere(DACruxTable Source, string Filter)
        {
            try
            {
                return SelectWhere(Source.DataTable.TableName, Source, Filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DACruxTable SelectWhere(string TableName, DACruxTable Source, string Filter)
        {
            DACruxTable dtcReturn = null;
            DataRow[] arrDr = null;
            int iDrCnt;
            try
            {                
                arrDr = Source.DataTable.Select(Filter);
                iDrCnt = arrDr.Length;
                dtcReturn = Source.Clone();
                dtcReturn.DataTable.TableName = TableName;
                dtcReturn.DataTable.AcceptChanges();
                for (int i = 0; i < iDrCnt; i++)
                {
                    dtcReturn.DataTable.Rows.Add(arrDr[i].ItemArray);
                }
                dtcReturn.DataTable.AcceptChanges();
                dtcReturn.ObjVariable = Source.ObjVariable;
                dtcReturn.ObjVariableValue = Source.ObjVariableValue;
                dtcReturn.SeriesVariable = Source.SeriesVariable;
                dtcReturn.SeriesVariableValue = Source.SeriesVariableValue;
                dtcReturn.Variable = Source.Variable;
                return dtcReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtcReturn = null;
                arrDr = null;
            }
        }

        
        public static DACruxTable SelectWhere(DACruxTable Source, int SelectedColumn, string Filter)
        {
            try
            {
                return SelectWhere(Source.DataTable.TableName, Source, SelectedColumn, Filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DACruxTable SelectWhere(string TableName, DACruxTable Source, int SelectedColumn, string Filter)
        {
            DACruxTable dtcReturn = null;
            DataRow[] arrDr = null;
            int iDrCnt;
            try
            {

                dtcReturn = new DACruxTable();
                dtcReturn.DataTable = new DataTable(TableName);
                dtcReturn.DataTable.Columns.Add(Source.DataTable.Columns[SelectedColumn].ColumnName, Source.DataTable.Columns[SelectedColumn].DataType);
                dtcReturn.DataTable.AcceptChanges();

                arrDr = Source.DataTable.Select(Filter);
                iDrCnt = arrDr.Length;

                for (int i = 0; i < iDrCnt; i++)
                {
                    dtcReturn.DataTable.Rows.Add(new object[] { arrDr[i][SelectedColumn] });
                }
                dtcReturn.DataTable.AcceptChanges();
                dtcReturn.ObjVariable = Source.ObjVariable;
                dtcReturn.ObjVariableValue = Source.ObjVariableValue;
                dtcReturn.SeriesVariable = Source.SeriesVariable;
                dtcReturn.SeriesVariableValue = Source.SeriesVariableValue;
                dtcReturn.Variable = Source.Variable;
                return dtcReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtcReturn = null;
                arrDr = null;
            }
        }

        #endregion


        #endregion

        #endregion


        #region Decimal Place
        public static int GetDecimalPlace(DataTable dt, int iCol)
        {
            int iReturn;
            int iRowCnt;
            int iColCnt;
            double dblTemp;
            int iTemp;
            try
            {
                if (dt == null)
                    throw new ArgumentException("dt is null.");
                iRowCnt = dt.Rows.Count;
                iColCnt = dt.Columns.Count;
                if (iCol >= iColCnt)
                    throw new ArgumentException("dt dont has iCol.");
                iReturn = 0;
                for (int i = 0; i < iRowCnt; i++)
                {

                    if (double.TryParse(dt.Rows[i][iCol].ToString(), out dblTemp))
                    {
                        if (dblTemp.ToString().IndexOf('.') > -1)
                        {
                            iTemp = dblTemp.ToString().Length - (dblTemp.ToString().IndexOf('.') + 1);
                            if (iReturn < iTemp)
                                iReturn = iTemp;
                        }
                    }
                }
                return iReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static int GetDecimalPlace(DataTable dt)
        {
            int iReturn;
            int iRowCnt;
            int iColCnt;
            double dblTemp;
            int iTemp;
            try
            {
                if (dt == null)
                    throw new ArgumentException("dt is null.");
                iRowCnt = dt.Rows.Count;
                iColCnt = dt.Columns.Count;
                iReturn = 0;
                for (int i = 0; i < iRowCnt; i++)
                {
                    for (int j = 0; j < iColCnt; j++)
                    {
                        if (double.TryParse(dt.Rows[i][j].ToString(), out dblTemp))
                        {
                            if (dblTemp.ToString().IndexOf('.') > -1)
                            {
                                iTemp = dblTemp.ToString().Length - (dblTemp.ToString().IndexOf('.') + 1);
                                if (iReturn < iTemp)
                                    iReturn = iTemp;
                            }
                        }
                    }
                }
                return iReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        private static string ErrorParameter(string strParameter)
        {
            try
            {
                if(strParameter == string.Empty)
                    strParameter = "parameter";

                return string.Format("Argument({0}) is null.", strParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
