/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : FilterEx.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Core::DataSet에서 GroupBy나 Distinct등을 할 수 있는 Utility를 제공.
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Base
{
    /// <summary>
    /// 클래스명	:FilterEx
    /// 클래스요약	:DataSet에서 GroupBy나 Distinct등을 할 수 있는 Utility를 제공한다.
    /// 작 성 자	:미라콤 임영신
    /// 버    전	:V1.0.0.0
    /// 상세설명	:
    /// </summary>
    public class FilterEx
    {
        #region ▣내부 Function
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

                return bResult;  // value type standard comparison
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ▣SelectDistinct
        /// <summary>
        /// SourceTable에서 FieldNames을 기준으로 유일한 값을 구하여 주어진 TableName의 Table을 리턴한다.
        /// </summary>
        /// <param name="TableName">Return될 Table의 Name이다.</param>
        /// <param name="SourceTable">Distinct 대상이 될 DataTable이며 FieldNames를 반드시 포함하고 있어야 한다.</param>
        /// <param name="FieldNames">Distinct 대상이 될 Column이름이며 여러개일 경우 ","로 구분하여 명시하면 된다.</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldNames)
        {
            string[] FieldName = null;
            object[] LastValue = null;
            object[] CurrValue = null;
            DataTable dt = null;
            try
            {
                FieldName = FieldNames.Split(',');
                dt = new DataTable(TableName);
                LastValue = new object[FieldName.Length];
                CurrValue = new object[FieldName.Length];

                for (int i = 0; i < FieldName.Length; i++)
                {
                    dt.Columns.Add(FieldName[i], SourceTable.Columns[FieldName[i]].DataType);
                }

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
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (FieldName != null) FieldName = null;
                if (LastValue != null) LastValue = null;
                if (CurrValue != null) CurrValue = null;
                if (dt != null) dt.Dispose();

            }
        }
        #endregion

        #region ▣SelectGroupBy
        /// <summary>
        /// SourceTable에서 FieldNames을 기준으로 유일한 값에 대한 Count를 구하여 주어진 TableName의 Table을 리턴한다.
        /// </summary>
        /// <param name="TableName">Return될 Table의 Name이다.</param>
        /// <param name="SourceTable">Distinct 대상이 될 DataTable이며 FieldNames를 반드시 포함하고 있어야 한다.</param>
        /// <param name="FieldNames">Distinct 대상이 될 Column이름이며 여러개일 경우 ","로 구분하여 명시하면 된다.</param>
        /// <param name="Filter">Table에 Where조건을 명시한다.</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectGroupBy(string TableName, DataTable SourceTable, string FieldNames, string Filter)
        {
            string[] FieldName = null;
            object[] LastValue = null;
            object[] CurrValue = null;
            DataTable dt = null;

            try
            {
                FieldName = FieldNames.Split(',');
                dt = new DataTable(TableName);
                LastValue = new object[FieldName.Length];
                CurrValue = new object[FieldName.Length];

                for (int i = 0; i < FieldName.Length; i++)
                {
                    dt.Columns.Add(FieldName[i], SourceTable.Columns[FieldName[i]].DataType);
                }

                dt.Columns.Add("COUNT", System.Type.GetType("System.Int32"));

                foreach (DataRow dr in SourceTable.Select(Filter, FieldNames))
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

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (FieldName != null) FieldName = null;
                if (LastValue != null) LastValue = null;
                if (CurrValue != null) CurrValue = null;
                if (dt != null) dt.Dispose();

            }
        }
        #endregion

        #region ▣SelectGroupBy
        /// <summary>
        /// SourceTable에서 FieldNames을 기준으로 유일한 값에 대한 Count를 구하여 주어진 TableName의 Table을 리턴한다.
        /// </summary>
        /// <param name="TableName">Return될 Table의 Name이다.</param>
        /// <param name="SourceTable">Distinct 대상이 될 DataTable이며 FieldNames를 반드시 포함하고 있어야 한다.</param>
        /// <param name="FieldNames">Distinct 대상이 될 Column이름이며 여러개일 경우 ","로 구분하여 명시하면 된다.</param>
        /// <param name="Filter">Table에 Where조건을 명시한다.</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectGroupByNoSort(string TableName, DataTable SourceTable, string FieldNames, string Filter)
        {
            string[] FieldName = null;
            object[] LastValue = null;
            object[] CurrValue = null;
            DataTable dt = null;
            DataView dv = null;
            try
            {

                FieldName = FieldNames.Split(',');
                dt = new DataTable(TableName);
                dv = new DataView(SourceTable);

                LastValue = new object[FieldName.Length];
                CurrValue = new object[FieldName.Length];

                for (int i = 0; i < FieldName.Length; i++)
                {
                    dt.Columns.Add(FieldName[i], SourceTable.Columns[FieldName[i]].DataType);
                }

                dt.Columns.Add("COUNT", System.Type.GetType("System.Int32"));

                dv.RowFilter = Filter;
                dv.Sort = FieldNames;

                for (int i = 0; i < dv.Count; i++)
                {
                    for (int col = 0; col < FieldName.Length; col++)
                    {
                        CurrValue[col] = dv[i][FieldName[col]];
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

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (FieldName != null) FieldName = null;
                if (LastValue != null) LastValue = null;
                if (CurrValue != null) CurrValue = null;
                if (dt != null) dt.Dispose();

            }
        }
        #endregion

        #region ▣SelectGroupBy
        /// <summary>
        /// SourceTable에서 FieldNames을 기준으로 유일한 값에 대한 Count를 구하여 주어진 TableName의 Table을 리턴한다.
        /// </summary>
        /// <param name="TableName">Return될 Table의 Name이다.</param>
        /// <param name="SourceTable">Distinct 대상이 될 DataTable이며 FieldNames를 반드시 포함하고 있어야 한다.</param>
        /// <param name="FieldNames">Distinct 대상이 될 Column이름이며 여러개일 경우 ","로 구분하여 명시하면 된다.</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectGroupBy(string TableName, DataTable SourceTable, string FieldNames)
        {
            string[] FieldName = null;
            object[] LastValue = null;
            object[] CurrValue = null;
            DataTable dt = null;
            try
            {
                FieldName = FieldNames.Split(',');
                dt = new DataTable(TableName);
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

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (FieldName != null) FieldName = null;
                if (LastValue != null) LastValue = null;
                if (CurrValue != null) CurrValue = null;
                if (dt != null) dt.Dispose();
            }
        }
        #endregion

        #region ▣SelectMinMax
        /// <summary>
        /// SourceTable에서 FieldName의 Column Value들중 Max값과 Min값을 Return한다.
        /// </summary>
        /// <param name="SourceTable">Return될 Table의 Name이다.</param>
        /// <param name="FieldName">Min, Max를 구할 Column Name</param>
        /// <param name="Min">Min Value의 Out Parameter</param>
        /// <param name="Max">Max Value의 Out Parameter</param>
        public static void SelectMinMax(DataTable SourceTable, string FieldName, out double Min, out double Max)
        {
            try
            {
                DataRow[] dr = SourceTable.Select("", FieldName);

                Min = DACrux.Base.Convert.doubleParse(dr[0][FieldName].ToString());
                Max = DACrux.Base.Convert.doubleParse(dr[dr.Length - 1][FieldName].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SelectMinMax(DataTable SourceTable, string FieldName, out float Min, out float Max)
        {
            try
            {
                DataRow[] dr = SourceTable.Select("", FieldName);

                Min = float.Parse(dr[0][FieldName].ToString());
                Max = float.Parse(dr[dr.Length - 1][FieldName].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SelectMinMax(DataTable SourceTable, string FieldName, out int Min, out int Max)
        {
            try
            {
                DataRow[] dr = SourceTable.Select("", FieldName);

                Min = DACrux.Base.Convert.intParse(dr[0][FieldName].ToString());
                Max = DACrux.Base.Convert.intParse(dr[dr.Length - 1][FieldName].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
