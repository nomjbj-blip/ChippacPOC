using System;
using System.Text;
using System.Data;
using DACrux.TEST.DSL;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace DACrux.TEST.BSL
{
    /// <summary>
    /// TEST 데이터 처리를 위한 클래스
    /// </summary>
    public class TestDataManager
    {
        /// <summary>
        /// 테이블 당 허용하는 최대 컬럼 갯수
        /// </summary>
        public static readonly int MAX_COLUMN_COUNT = 900;
        private static readonly Dictionary<String, String> dicParameterReplaceValues = null;

        static TestDataManager()
        {
            dicParameterReplaceValues = new Dictionary<String, String>();
            dicParameterReplaceValues.Add("#1", "");
            dicParameterReplaceValues.Add("#2", "");
            dicParameterReplaceValues.Add("#3", "");
            dicParameterReplaceValues.Add("#4", "");
            dicParameterReplaceValues.Add("#5", "");
            dicParameterReplaceValues.Add("#6", "");
            dicParameterReplaceValues.Add("#7", "");
            dicParameterReplaceValues.Add("#8", "");
            dicParameterReplaceValues.Add("#9", "");
        }

        /// <summary>
        /// TEST 데이터를 저장합니다.
        /// </summary>
        public static void SaveData(
            string factory,
            string programName,
            DataTable rawDt,
            string waferSeq
            )
        {
            List<string> columnList = new List<string>();
            List<string> tableList = new List<string>();

            foreach (DataColumn col in rawDt.Columns)
                columnList.Add(col.ColumnName.ToUpper());

            // 프로그램명으로 테이블명 가져오기
            TQP_DATA_TABLES obj = new TQP_DATA_TABLES();
            string[] tableArr = obj.GetTableNames(factory, programName);

            TQP_PARASPEC par = new TQP_PARASPEC();
            int programRev = par.GetMaxProgramRev(factory, programName);

            if (tableArr != null && tableArr.Length > 0)
                tableList.AddRange(tableArr);

            if (tableList.Count == 0)
            {
                // 해당 프로그램에 대해 신규 입력 인 경우
                tableArr = CreateNewTable(factory, programName, programRev, columnList);
                tableList.AddRange(tableArr);
            }
            else
            {
                // 테이블의 컬럼과 비교하여 매핑 안되는 필드 추가 (필요시 테이블도 추가)
                CheckColumns(factory, programName, programRev, tableList, columnList);
            }

            TD_TABLE tbl = new TD_TABLE();

            // 해당 WAFER_SEQ 로 데이터가 존재하는 경우 삭제 처리
            tbl.DeleteData(waferSeq, tableList);

            // 데이터 추가
            // bulk insert 
            DataSet ds = tbl.GetTableSchema(factory, programName);

            for (int i = 0; i < rawDt.Rows.Count; i++)
            {
                SetData(ds, rawDt.Rows[i], waferSeq, i + 1);
            }

            foreach (DataTable dt in ds.Tables)
            {
                // 한건이라도 데이터가 있으면 추가한다.
                if (dt.Rows.Count > 0)
                {
                    tbl.InsertData(dt);
                }
            }
        }

        public static void SaveData(
            string factory,
            string programName,
            DataTable rawDt,
            string waferSeq,
            bool bIncreaseProbeCnt
            )
        {
            /// Fab1 PCM 데이터 처리 관련
            /// Probe Count 증가시에 해당 로직 적용
            if (bIncreaseProbeCnt)
            {
                SaveData(factory, programName, rawDt, waferSeq);
                return;
            }

            /// Fab2 PCM Data 관련 데이터 처리
            List<string> columnList = new List<string>();
            List<string> tableList = new List<string>();

            String columnName = String.Empty;
            foreach (DataColumn col in rawDt.Columns)
            {
                columnName = col.ColumnName.ToUpper();
                /// Parameter Name 에 "#1", "#2", "#3" 가 존재하는 경우
                /// 해당 Parameter를 치환 후 기존 값과 비교하여 처리
                if (columnName.Contains("#"))
                {
                    foreach (KeyValuePair<String, String> pv in dicParameterReplaceValues)
                        columnName = columnName.Replace(pv.Key, pv.Value);
                }

                if (columnList.Contains(columnName))
                    continue;

                columnList.Add(columnName);
            }

            TQP_DATA_TABLES obj = new TQP_DATA_TABLES();
            string[] tableArr = obj.GetTableNames(factory, programName);

            TQP_PARASPEC par = new TQP_PARASPEC();
            int programRev = par.GetMaxProgramRev(factory, programName);
            DataTable dtParamSpec = par.GetParamSepc(factory, programName, programRev.ToString());

            if (tableArr != null && tableArr.Length > 0)
                tableList.AddRange(tableArr);

            if (tableList.Count == 0)
            {
                tableArr = CreateNewTable(factory, programName, programRev, columnList);
                tableList.AddRange(tableArr);
            }
            else
            {
                CheckColumns(factory, programName, programRev, tableList, columnList);
            }

            TD_TABLE tbl = new TD_TABLE();
            //DataTable dtRaw = tbl.GetAllRawData(new string[] { waferSeq });
            DataSet ds = tbl.GetTableSchema(factory, programName, waferSeq);

            // 해당 WAFER_SEQ 로 데이터가 존재하는 경우 삭제 처리
            tbl.DeleteData(waferSeq, tableList);

            // Retest에 대한 데이터 처리
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < rawDt.Rows.Count; i++)
                {
                    SetData(ds, dtParamSpec, rawDt.Rows[i], waferSeq);
                }
            }
            else // 최초 등록시 
            {
                for (int i = 0; i < rawDt.Rows.Count; i++)
                {
                    SetData(ds, dtParamSpec, rawDt.Rows[i], waferSeq, i + 1);
                }
            }

            foreach (DataTable dt in ds.Tables)
            {
                // 한건이라도 데이터가 있으면 추가한다.
                if (dt.Rows.Count > 0)
                {
                    tbl.InsertData(dt);
                }
            }

        }

        /// <summary>
        /// default
        /// </summary>
        private static void SetData(
            DataSet ds,
            DataRow row,
            string waferSeq,
            int order
            )
        {
            // 추가할 Row 리스트
            List<DataRow> newRowList = new List<DataRow>();

            foreach (DataTable dt in ds.Tables)
                newRowList.Add(dt.NewRow());

            foreach (DataColumn col in row.Table.Columns)
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Columns.Contains(col.ColumnName))
                    {
                        // 필수 컬럼 3개
                        DataColumn waferCol = ds.Tables[i].Columns["WAFER_SEQ"];
                        DataColumn dieNumCol = ds.Tables[i].Columns["DIE_NUM"];
                        DataColumn dieProbeCol = ds.Tables[i].Columns["DIEPROBE_CNT"];

                        DataRow newRow = newRowList[i];

                        // 해당 테이블에 처음 추가되는 데이터인 경우
                        if (newRowList[i][waferCol] == null || newRowList[i][waferCol] == DBNull.Value)
                        {
                            newRow[waferCol] = Convert.ChangeType(waferSeq, waferCol.DataType);
                            newRow[dieNumCol] = order;
                            newRow[dieProbeCol] = 0;
                        }

                        object val = row[col.ColumnName];

                        if (val == null || val == DBNull.Value || String.IsNullOrWhiteSpace(val.ToString()))
                            newRow[col.ColumnName] = DBNull.Value;
                        else if (ds.Tables[i].Columns[col.ColumnName].DataType == typeof(decimal))
                            newRow[col.ColumnName] = GetDecimalValue(val.ToString());
                        else
                            newRow[col.ColumnName] = Convert.ChangeType(val, ds.Tables[i].Columns[col.ColumnName].DataType);
                    }
                }
            }

            // NewRow에 데이터가 추가되었으면 INSERT
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                object seq = newRowList[i]["WAFER_SEQ"];

                if (seq != null && seq != DBNull.Value)
                    ds.Tables[i].Rows.Add(newRowList[i]);
            }
        }

        /// <summary>
        /// FAB2 PCM 최초 데이터 처리시.
        /// Parameter name에 "#"이 존재하는 경우
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="dtParam"></param>
        /// <param name="row"></param>
        /// <param name="waferSeq"></param>
        /// <param name="order"></param>
        private static void SetData(
            DataSet ds,
            DataTable dtParam,
            DataRow row,
            string waferSeq,
            int order
            )
        {
            // 추가할 Row 리스트
            List<DataRow> newRowList = new List<DataRow>();

            foreach (DataTable dt in ds.Tables)
                newRowList.Add(dt.NewRow());

            DataRow[] drsParam = null;
            String newColumnName = String.Empty;
            decimal sourceVal = decimal.Zero;
            decimal targetVal = decimal.Zero;

            foreach (DataColumn col in row.Table.Columns)
            {
                newColumnName = col.ColumnName;
                targetVal = decimal.Zero;
                /// PARAMETER NAME 에 "#1", "#2", "#3" ... 와 같이 등록된 경우
                /// EX> BV_CV_P_24V#1, BV_CV_P_24V#2 -> BV_CV_P_24V로 변경하여 저장
                /// BV_CV_P_24V 에 해당하는 Value 값이랑 Spec 체크하여 Spec out 에 한해서
                /// BV_CV_P_24V#1, BV_CV_P_24V#2 값으로 치환한다.
                if (col.ColumnName.Contains("#"))
                {
                    foreach (KeyValuePair<String, String> pv in dicParameterReplaceValues)
                        newColumnName = newColumnName.Replace(pv.Key, pv.Value);
                    drsParam = dtParam.Select(String.Format("[PARAM_NAME] = '{0}'", newColumnName));
                    targetVal = GetDecimalValue(row[col.ColumnName].ToString());
                }

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Columns.Contains(newColumnName))
                    {
                        // 필수 컬럼 3개
                        DataColumn waferCol = ds.Tables[i].Columns["WAFER_SEQ"];
                        DataColumn dieNumCol = ds.Tables[i].Columns["DIE_NUM"];
                        DataColumn dieProbeCol = ds.Tables[i].Columns["DIEPROBE_CNT"];

                        DataRow newRow = newRowList[i];

                        // 해당 테이블에 처음 추가되는 데이터인 경우
                        if (newRowList[i][waferCol] == null || newRowList[i][waferCol] == DBNull.Value)
                        {
                            newRow[waferCol] = Convert.ChangeType(waferSeq, waferCol.DataType);
                            newRow[dieNumCol] = order;
                            newRow[dieProbeCol] = 0;
                        }

                        object val = null;
                        // 파일에 BV_CV_P_24V 해당하는 Item 없는 경우 발생
                        // BV_CV_P_24V#1 해당 Item만 올라오는 경우가 존재하여 로직 추가
                        if (row.Table.Columns.Contains(newColumnName))
                            val = row[newColumnName];
                        else
                            val = row[col.ColumnName];

                        if (val == null || val == DBNull.Value || String.IsNullOrWhiteSpace(val.ToString()))
                        {
                            newRow[newColumnName] = DBNull.Value;
                        }
                        else if (ds.Tables[i].Columns[newColumnName].DataType == typeof(decimal))
                        {
                            sourceVal = GetDecimalValue(val.ToString());
                            newRow[newColumnName] = GetSpecCheckedValue(drsParam, sourceVal, targetVal);
                        }
                        else
                        {
                            newRow[newColumnName] = Convert.ChangeType(val, ds.Tables[i].Columns[newColumnName].DataType);
                        }
                    }
                }
            }

            // NewRow에 데이터가 추가되었으면 INSERT
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                object seq = newRowList[i]["WAFER_SEQ"];

                if (seq != null && seq != DBNull.Value)
                    ds.Tables[i].Rows.Add(newRowList[i]);
            }
        }


        /// <summary>
        /// retest 한 결과 파라미터에 대해서
        /// 해당 값이 Spec 값을 벗어난 경우에 대해서 
        /// DB의 값을 Retest 값으로 치환한다.
        /// </summary>
        private static void SetData(
            DataSet ds,
            DataTable dtPara,
            DataRow row,
            string waferSeq
            )
        {
            decimal sourceVal = decimal.Zero;
            decimal targetVal = decimal.Zero;
            DataRow[] drsParam = null;

            int iDieNum = GetDieNumber(
                ds,
                dtPara,
                row
                );

            // DIE NUM는 X, Y 좌표를 가지고 찾는다.
            // DIE NUM를 찾지 못하는 경우 기존 데이터 저장
            if (iDieNum == -1)
                return;

            String newColumnName = String.Empty;
            foreach (DataColumn col in row.Table.Columns)
            {
                if (string.Equals(col.ColumnName, "X")
                    || string.Equals(col.ColumnName, "Y")
                    || string.Equals(col.ColumnName, "BIN"))
                    continue;

                newColumnName = col.ColumnName;
                /// PARAMETER NAME 에 "#1", "#2", "#3" ... 와 같이 등록된 경우
                /// EX> BV_CV_P_24V#1, BV_CV_P_24V#2 -> BV_CV_P_24V로 변경하여 저장
                /// BV_CV_P_24V 에 해당하는 Value 값이랑 Spec 체크하여 Spec out 에 한해서
                /// BV_CV_P_24V#1, BV_CV_P_24V#2 값으로 치환한다.
                foreach (KeyValuePair<String, String> pv in dicParameterReplaceValues)
                    newColumnName = newColumnName.Replace(pv.Key, pv.Value);

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Columns.Contains(newColumnName))
                    {
                        // DB Data
                        sourceVal = GetDecimalValue(ds.Tables[i].Rows[iDieNum][newColumnName].ToString());

                        // Upload File RawData
                        targetVal = GetDecimalValue(row[col.ColumnName].ToString());
                        drsParam = dtPara.Select(String.Format("[PARAM_NAME] = '{0}'", newColumnName));
                        if (drsParam != null && drsParam.Length > 0)
                        {
                            ds.Tables[i].Rows[iDieNum][newColumnName] = GetSpecCheckedValue(
                                drsParam,
                                sourceVal,
                                targetVal
                                );
                        }
                    }
                }
            }
        }

        private static int GetDieNumber(
            DataSet ds,
            DataTable dtPara,
            DataRow row
            )
        {
            object objX = null;
            object objY = null;
            foreach (DataColumn col in row.Table.Columns)
            {
                if (string.Equals(col.ColumnName, "X"))
                    objX = row[col.ColumnName];
                else if (string.Equals(col.ColumnName, "Y"))
                    objY = row[col.ColumnName];

                if (objX != null && objY != null)
                    break;
            }

            DataRow[] drs = dtPara.Select(String.Format("[PARAM_NAME] IN ('X', 'Y')"));
            string tableName = drs[0]["TABLE_NAME"].ToString();
            drs = ds.Tables[tableName].Select(String.Format("[X] = '{0}' AND [Y] = '{1}'", objX, objY));

            if (drs == null || drs.Length <= 0)
                return -1;
            else
                return int.Parse(drs[0]["DIE_NUM"].ToString()) - 1;
        }

        private static decimal GetDecimalValue(
            string sValue
            )
        {
            string digit = string.Empty;
            int iValue = 0;
            decimal decValue = decimal.Zero;

            if (!Decimal.TryParse(sValue, NumberStyles.Float, CultureInfo.InvariantCulture, out decValue))
            {
                if (string.IsNullOrEmpty(sValue))
                    return decimal.Zero;

                digit = sValue.Substring(0, 1);
                if (int.TryParse(digit, out iValue))
                    return decimal.MaxValue;

                if (string.Equals(digit, "+"))
                    return decimal.MaxValue;
                else
                    return decimal.MinValue;
            }
            return decValue;
        }

        private static object GetSpecCheckedValue(
            DataRow[] drParamSpec,
            decimal sourceVal,
            decimal targetVal
            )
        {
            if (drParamSpec == null || drParamSpec.Length <= 0)
                return sourceVal;

            double dUsl = double.NaN;
            double dLsl = double.NaN;
            double dUtl = double.NaN;
            double dLtl = double.NaN;

            if (!double.TryParse(drParamSpec[0]["USL"].ToString(), out dUsl))
                dUsl = double.NaN;
            if (!double.TryParse(drParamSpec[0]["LSL"].ToString(), out dLsl))
                dLsl = double.NaN;
            if (!double.TryParse(drParamSpec[0]["UTL"].ToString(), out dUtl))
                dUtl = double.NaN;
            if (!double.TryParse(drParamSpec[0]["LTL"].ToString(), out dLtl))
                dLtl = double.NaN;

            if (decimal.Equals(sourceVal, decimal.Zero))
            {
                if (decimal.Equals(targetVal, decimal.Zero))
                    return DBNull.Value;
                else
                    return targetVal;
            }

            // 둘다 없는 경우
            if (double.IsNaN(dUsl) && double.IsNaN(dLsl))
            {
                return sourceVal;
            }
            // LSL 존재하는 경우
            else if (double.IsNaN(dUsl) && !double.IsNaN(dLsl))
            {
                if ((decimal)dLsl < sourceVal)
                    return sourceVal;

                if (decimal.Equals(targetVal, decimal.Zero))
                    return DBNull.Value;
                else
                    return targetVal;
            }
            // USL 존재하는 경우
            else if (!double.IsNaN(dUsl) && double.IsNaN(dLsl))
            {
                if (sourceVal < (decimal)dUsl)
                    return sourceVal;

                if (decimal.Equals(targetVal, decimal.Zero))
                    return DBNull.Value;
                else
                    return targetVal;
            }
            else if (!double.IsNaN(dUsl) && !double.IsNaN(dLsl))
            {
                /// lsl의 값보다 usl의 값이 더 큰 경우
                if (dUsl < dLsl)
                    return sourceVal;

                /// utl / ltl에 대한 값이 존재하는 경우
                if (!double.IsNaN(dUtl) && !double.IsNaN(dLtl))
                {
                    if ((decimal)dLtl < sourceVal && sourceVal < (decimal)dUtl)
                        return sourceVal;

                    if (decimal.Equals(sourceVal, decimal.Zero) && decimal.Equals(targetVal, decimal.Zero))
                        return DBNull.Value;
                    else if (decimal.Equals(targetVal, decimal.Zero))
                        return sourceVal;
                    else
                        return targetVal;
                }
                else
                {
                    if ((decimal)dLsl < sourceVal && sourceVal < (decimal)dUsl)
                        return sourceVal;

                    if (decimal.Equals(sourceVal, decimal.Zero) && decimal.Equals(targetVal, decimal.Zero))
                        return DBNull.Value;
                    else if (decimal.Equals(targetVal, decimal.Zero))
                        return sourceVal;
                    else
                        return targetVal;
                }
            }

            return sourceVal;
        }

        //----------------------------------------------------------------------------------------------------

        /// <summary>
        /// 새 테이블을 생성하고 새 테이블명 배열을 리턴합니다.
        /// </summary>
        private static string[] CreateNewTable(
            string factory,
            string programName,
            int programRev,
            List<string> columnList
            )
        {
            List<string> tableList = new List<string>();

            // MAX_COLUMN_COUNT 갯수만큼씩 테이블 생성
            for (int i = 0; ; i++)
            {
                int count = Math.Min(MAX_COLUMN_COUNT, columnList.Count - MAX_COLUMN_COUNT * i);

                string[] arr = new string[count];
                columnList.CopyTo(MAX_COLUMN_COUNT * i, arr, 0, count);
                List<string> newTableColumn = new List<string>(arr);

                TD_TABLE obj = new TD_TABLE();
                string tableName = obj.CreateNewTable(factory, programName, programRev, i, newTableColumn);
                tableList.Add(tableName);

                if (count != MAX_COLUMN_COUNT)
                    break;
            }

            return tableList.ToArray();
        }

        /// <summary>
        /// 컬럼을 체크하여 테이블에 해당 컬럼이 있는지 확인하여 없으면 추가합니다. 컬럼의 최대 갯수를 초과할 경우 새 테이블을 만듭니다.
        /// </summary>
        private static void CheckColumns(
            string factory,
            string programName,
            int programRev,
            List<string> tableList,
            List<string> columnList
            )
        {
            TQP_PARASPEC par = new TQP_PARASPEC();
            TD_TABLE tbl = new TD_TABLE();

            List<string> dbColumnList = new List<string>();

            // 해당 프로그램에 대한 모든 테이블의 컬럼 수집
            foreach (string tableName in tableList)
            {
                string[] colArr = par.GetParamNames(factory, programName, tableName);
                dbColumnList.AddRange(colArr);
            }

            List<string> newColumnList = new List<string>();

            // 테이블에 없는 컬럼만 수집
            foreach (string column in columnList)
            {
                if (!dbColumnList.Contains(column))
                    newColumnList.Add(column);
            }

            if (newColumnList.Count == 0)
                return;

            // 마지막 테이블 명
            string lastTableName = tableList[tableList.Count - 1];

            string[] paramArr = par.GetParamNames(factory, programName, lastTableName);

            // 추가 가능한 여유 컬럼 갯수
            int emptyCount = MAX_COLUMN_COUNT - paramArr.Length;

            // 마지막 테이블에 컬럼을 추가할 여유가 있는 경우
            if (emptyCount > 0)
            {
                List<string> list;

                // 컬럼에 여유가 있는 경우
                if (emptyCount > newColumnList.Count)
                {
                    list = new List<string>(newColumnList);
                    newColumnList.Clear();
                }
                else // 추가 가능한 여유 컬럼 갯수 보다 추가할 컬럼이 많은 경우
                {
                    string[] arr = new string[emptyCount];
                    newColumnList.CopyTo(0, arr, 0, emptyCount);
                    list = new List<string>(arr);
                    // 추가된 데이터를 리스트에서 삭제
                    newColumnList.RemoveRange(0, emptyCount);
                }

                // 기존 테이블에 컬럼 추가
                tbl.AppendColumn(factory, programName, programRev, tableList.Count - 1, list);
            }

            // 신규 테이블 추가가 필요한 경우
            if (newColumnList.Count > 0)
            {
                string newTableName = tbl.CreateNewTable(factory, programName, programRev, tableList.Count, newColumnList);
                tableList.Add(newTableName);
            }
        }

        /// <summary>
        /// TD 테이블이 비어있는지 확인 후 비어있으면 DROP 시키고 PARAMETER 및 TABLE 정보를 삭제합니다.
        /// </summary>
        public void CheckTableEmpty(string tableName)
        {
            TD_TABLE td = new TD_TABLE();

            // 빈 테이블인지 확인
            if (td.ExistsData(tableName))
                return;

            // TABLE 리스트를 삭제
            TQP_DATA_TABLES tbl = new TQP_DATA_TABLES();
            tbl.DeleteDataByTableName(tableName);

            // PARAMETER 리스트 삭제
            TQP_PARASPEC par = new TQP_PARASPEC();
            par.DeleteDataByTableName(tableName);

            // TABLE 삭제
            td.DropTable(tableName);
        }
    }
}
