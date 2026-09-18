using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_PARASPEC : Miracom.Middleware.QueryComponent
    {
        public TQP_PARASPEC()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_PARASPEC.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [Common Select]
        public DataTable GetData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Insert]
        public void InsertData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertExecuteMultiple(string sqlName, string[,] paras)
        {
            try
            {
                this.ExecuteMultiple(sqlName, null, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Update]
        public void UpdateData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Delete]
        public void DeleteData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public void CreateParaSpec(string[,] ParaInfo)
        {
            try
            {
                this.ExecuteMultiple("CREATE_PARASPEC", ParaInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetParaSpecListEditable(
            string factory,
            string Program
            )
        {

            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_PARASPEC_EDITABLE", null, new string[] { factory, Program });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetParameterNotEmpty(
            string[] items, 
            string query
            )
        {
            // GetDataTable 호출시 "산술 연산으로 인해 오버클로가 발생 되었습니다." 메시지 발생
            // GetDataTable_Numeric_IgnoreException로 수정
            string[] pivotIn = new string[items.Length];
            for (int idx = 0; idx < items.Length; idx++)
            {
                pivotIn[idx] = items[idx].Replace("A.", "");
            }
            return GetDataTable_Numeric_IgnoreException("SELECT_PARA_NOT_EMPTY", ConvertBy.String, new string[] { string.Join(",", items), query, string.Join(",", pivotIn) }, null);
        }


        public DataTable SelectProgramRevMaxValue(
            string factory,
            string program
            )
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_PROGRAM_REV_MAX", null, new string[] { factory, program });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProgramParaSpec(string Program, string[,] ParaInfo)
        {
            try
            {

                this.Execute("DELETE_TEMP_PROGRAM", null, null);						//혹시 있을.. "UPDATE_TEMP"를 삭제한다.
                this.Execute("UPDATE_PARASPEC_TEMP", null, new string[] { Program });	//기존것을 "UPDATE_TEMP" 로 바꾼다
                if (ParaInfo.Length > 0) this.ExecuteMultiple("CREATE_PARASPEC", ParaInfo);		//새로운것을 Insert한다.
                this.Execute("DELETE_TEMP_PROGRAM", null, null);						//"UPDATE_TEMP"를 삭제한다.
            }
            catch (Exception ex)
            {
                this.Execute("DELETE_PROGRAM_PARASPEC", null, new string[] { Program });	//Insert하던 새로운것을 삭제한다.
                this.Execute("ROLLBACK_PARASPEC_TEMP", null, new string[] { Program });	//"UPDATE_TEMP"를 원래의 Program Name으로 바꾼다
                throw ex;
            }
        }

        public void DeleteProgramParaSpec(string Program)
        {
            try
            {
                this.Execute("DELETE_PROGRAM_PARASPEC", null, new string[] { Program });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// PARAM_NAME 배열을 가져옵니다.
        /// </summary>
        public string[] GetParamNames(string factory, string program, string tableName)
        {
            DataTable dt = GetDataTable("SELECT_PARAM_NAME", null, new string[] { factory, program, tableName });

            if (dt == null || dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        /// <summary>
        /// 최대 PARAM_INDEX 값을 가져옵니다.
        /// </summary>
        public int GetMaxParamIndex(string factory, string program, string tableName)
        {
            object obj = ExecuteScalar("SELECT_MAX_PARAM_INDEX", null, new string[] { factory, program, tableName });
            return Int32.Parse(obj.ToString());
        }


        /// <summary>
        /// 최대 PROGRAM_REV 값을 가져옵니다.
        /// </summary>
        public int GetMaxProgramRev(string factory, string program)
        {
            object obj = ExecuteScalar("SELECT_MAX_PROGRAM_REV", null, new string[] { factory, program });
            return Int32.Parse(obj.ToString());
        }

        public DataTable GetProgramParamsData(
            string factory,
            string program,
            string[] programRev
            )
        {
            return GetDataTable(
                "SELECT_PARA_DATA",
                new string[] { string.Format("'{0}'", string.Join("','", programRev)) },
                new string[] { factory, program }
                );
        }

        public DataTable GetParameterItems(
            string factory,
            string program,
            string programRev
            )
        {
            return this.GetDataTable(
                "GET_ITEM_01",
                null,
                new string[] { factory, program, programRev }
                );
        }

        public string GetBinTTable(
            decimal waferSeq
            )
        {
            DataTable dt = this.GetDataTable(
                "GET_BIN_TTABLE",
                null,
                new string[] { waferSeq.ToString() }
                );

            if (dt == null || dt.Rows.Count == 0)
                return null;

            return (string)dt.Rows[0][0];
        }

        /// <summary>
        /// PROGRAM에 해당하는 TABLE_NAME, PARAM_NAME 테이블을 가져옵니다.
        /// </summary>
        public DataTable GetParaSpec(string program)
        {
            return this.GetDataTable("SELECT_PARA_SPEC_02", null, new string[] { program });
        }

        public DataTable GetParaSpec(string factory, string program, string tableName)
        {
            return this.GetDataTable("SELECT_PARA_SPEC_03", null, new string[] { factory, program, tableName });
        }

        public DataTable GetParameterItemByMultiWafers(
            string program,
            long[] wafers
            )
        {
            return this.GetDataTable("SELECT_PARA_ITEM_MULTI", new string[] { string.Format("'{0}'", string.Join("','", wafers)) }, new string[] { program });
        }


        /// <summary>
        /// PROGRAM에 해당하는 모든 데이터를 가져옵니다. 
        /// USL, LSL, UTL, LTL에 해당하는 정보 필요
        /// </summary>
        public DataTable GetParamSepc(
            string factory,
            string programName,
            string programRev
            )
        {
            return this.GetDataTable(
                "SELECT_PARA_SPEC_04",
                null,
                new string[] { factory, programName, programRev }
                );
        }

        /// <summary>
        /// 해당 테이블 명의 파리미터를 모두 삭제합니다.
        /// </summary>
        public void DeleteDataByTableName(string tableName)
        {
            ExecuteNonQuery("DELETE_BY_TABLE_NAME", null, new string[] { tableName });
        }
    }
}
