using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miracom.Middleware;
using System.Data;

namespace DACrux.TEST.DSL
{
    public class TQP_BINDESC : Miracom.Middleware.QueryComponent
    {
        public TQP_BINDESC()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_BINDESC.xml");
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

        public void CreateBins(string[,] BinInf)
        {
            try
            {
                this.ExecuteMultiple("CREATE_BIN", BinInf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProgramBin(string Program)
        {
            try
            {
                this.Execute("DELETE_PROGRAM_BIN_ALL", null, new string[] { Program });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void UpdateProgramBin(string Program, string[,] BinInf)
        //{
        //    //DELETE_PROGRAM_BIN
        //    try
        //    {
        //        this.Execute("UPDATE_PROGRAM_TEMP", null, new string[] { Program });
        //        if (BinInf.Length > 0) this.ExecuteMultiple("CREATE_BIN", BinInf);
        //        this.Execute("DELETE_TEMP_PROGRAM", null, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Execute("DELETE_PROGRAM_BIN", null, new string[] { Program });	//Insert하던 새로운것을 삭제한다.
        //        this.Execute("ROLLBACK_BINDESC_TEMP", null, new string[] { Program });	//"UPDATE_TEMP"를 원래의 Program Name으로 바꾼다
        //        throw ex;
        //    }
        //}
        public int UpdateProgramBin(
            string program,
            string[,] sParam
            )
        {
            return this.ExecuteMultipleEx(
                "UPDATE_PROGRAM_BIN",
                null,
                sParam);
        }

        public DataTable GetBinListEditable(string Program)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_BIN_EDITABLE", null, new string[] { Program });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBinInfoByAll(string Product, string TestArea, string Format)
        {
            try
            {
                return this.GetDataTable("SELECT_BININFO_BY_ALL_1", null, new string[] { Product, TestArea, Format });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinListEditableByTestArea(string Program, string TestArea)
        {

            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_BIN_EDITABLE_BY_TESTAREA", null, new string[] { Program, TestArea });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBinEditableByAll(string Product, string TestArea, string format)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_BIN_EDITABLE_BY_ALL", null, new string[] { Product, TestArea, format });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProgramBin(string Product, string TestArea, string Format, string[,] BinInf)
        {
            //DELETE_PROGRAM_BIN
            try
            {
                this.Execute("UPDATE_PRODUCT_TEMP", null, new string[] { Product, TestArea, Format });
                if (BinInf.Length > 0) this.ExecuteMultiple("CREATE_BIN", BinInf);
                this.Execute("DELETE_TEMP_PRODUCT", null, null);
            }
            catch (Exception ex)
            {
                this.Execute("DELETE_BIN_BY_ALL", null, new string[] { Product, TestArea, Format });	//Insert하던 새로운것을 삭제한다.
                this.Execute("ROLLBACK_BINDESC_TEMP", null, new string[] { Product });	//"UPDATE_TEMP"를 원래의 Product Name으로 바꾼다
                throw ex;
            }
        }

        public void UpdateProgramBinByTestArea(string Program, string TestArea, string[,] BinInf)
        {
            //DELETE_PROGRAM_BIN
            try
            {
                this.Execute("UPDATE_PROGRAM_TEMP_BY_TESTAREA", null, new string[] { Program, TestArea });
                if (BinInf.Length > 0) this.ExecuteMultiple("CREATE_BIN", BinInf);
                this.Execute("DELETE_TEMP_PROGRAM", null, null);
            }
            catch (Exception ex)
            {
                this.Execute("DELETE_PROGRAM_BIN_BY_TESTAREA", null, new string[] { Program, TestArea });	//Insert하던 새로운것을 삭제한다.
                this.Execute("ROLLBACK_BINDESC_TEMP", null, new string[] { Program });	//"UPDATE_TEMP"를 원래의 Program Name으로 바꾼다
                throw ex;
            }
        }

        public void DeleteBinByAll(string Product, string TestArea, string Format)
        {
            try
            {
                this.Execute("DELETE_BIN_BY_ALL", null, new string[] { Product, TestArea, Format });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProgramBinByTestArea(string Program, string TestArea)
        {
            try
            {
                this.Execute("DELETE_PROGRAM_BIN_BY_TESTAREA", null, new string[] { Program, TestArea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinInfo(
            string program
            )
        {
            return this.GetDataTable(
                "SELECT_BIN_DATA",
                null,
                new string[] { program }
                );
        }


        public DataTable GetBinList(string PRODUCT, string PROGRAM)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BINLIST", null, new string[] { PRODUCT, PROGRAM });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBindescByElement(string Contents)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_BINDESC_BY_ELEMENT", new string[] { Contents }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGecBinByAll(string Product, string TestArea, string Format)
        {
            try
            {
                return this.GetDataTable("SELECT_GECBIN_BY_ALL", null, new string[] { Product, TestArea, Format });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGecBinByAllAtOnce(string Product, string TestArea, string Format)
        {
            try
            {
                return this.GetDataTable("SELECT_GEC_BIN_BY_ALL_AT_ONCE", null, new string[] { Product, TestArea, Format });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBinByAllAtOnce(string Product, string TestArea, string Format)
        {
            try
            {
                return this.GetDataTable("SELECT_BININFO_BY_ALL_AT_ONCE", null, new string[] { Product, TestArea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBinfoByAll(string Product, string TestArea, string Format)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_BININFO_BY_ALL", null, new string[] { Product, TestArea });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Map Parser 사용 DSL-------------------------------------------------------

        public DataTable SelectBinInfo(string Program, string Product)
        {
            try
            {
                return this.GetDataTable("SELECT_BININFO", null, new string[] { Program, Product });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBinInfoByTestArea(string Program, string Product, string TestArea)
        {
            try
            {
                return this.GetDataTable("SELECT_BININFO_BY_TESTAREA", null, new string[] { Program, Product, TestArea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGecBin(string Program)
        {
            try
            {
                return this.GetDataTable("SELECT_GECBIN", null, new string[] { Program });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBinInforByCharBin(string program, string char_bin)
        {
            try
            {
                return this.GetDataTable("SELECT_BIN_BY_CHARBIN", null, new string[] { program, char_bin });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDescDistinct(string Entity)
        {
            try
            {
                return this.GetDataTable("SELECT_BINDESC_DISTINCT", new string[] { Entity, Entity, Entity }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion ------------------------------------------------------------------------

        public DataTable SelectBinRuleList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_BIN_RULE_LIST", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        public DataTable SelectProgramBin(string strProgram, string strBin)
        {
            return this.GetDataTable("SELECT_PGM_BIN", null, new string[] { strProgram, strBin });
        }

        public void SetBinDESC(string strProgram, string strBin, string strCharBin, string strDESC)
        {
            this.Execute("INSERT_BIN_DESC", null, new string[] { strProgram, strBin, strCharBin, strDESC });
        }

        public object CreateCopyBin(string sourceProgram, string targetProgram)
        {
            return ExecuteNonQuery("CREATE_COPY_BIN", null, new string[] { targetProgram, sourceProgram });
        }

        public object ExistsBinInfo(string program, string bin)
        {
            return ExecuteScalar("EXISTS_BIN_INFO", null, new string[] { program, bin });
        }

        public int InsertBinInfo(string program, string bin, string binname, string charbin, string highgec, string display, string upperlimitcnt, string lowerlimitcnt, string color, string description, string userid)
        {
            return ExecuteNonQuery("CREATE_BIN", null, new string[] { program, bin, binname, charbin, highgec, display, upperlimitcnt, lowerlimitcnt, color, description });
        }

        public int MergeBinInfo(string program, string bin, string binname, string charbin, string highgec, string display, string upperlimitcnt, string lowerlimitcnt, string color, string description, string userid)
        {
            return ExecuteNonQuery("MERGE_BIN", null, new string[] { program, bin, binname, charbin, highgec, display, upperlimitcnt, lowerlimitcnt, color, description });
        }

        public int MergeBinInfo(string[] value)
        {
            return ExecuteNonQuery("MERGE_BIN", null, value);
        }

        public int UpadateBinInfo(string program, string bin, string binname, string charbin, string highgec, string display, string upperlimitcnt, string lowerlimitcnt, string color, string description, string userid)
        {
            return ExecuteNonQuery("UPDATE_PROGRAM_BIN", null, new string[] { program, bin, binname, charbin, highgec, display, upperlimitcnt, lowerlimitcnt, color, description });
        }

        public int DeleteBinInfo(string program, string bin, string userid)
        {
            return ExecuteNonQuery("DELETE_PROGRAM_BIN", null, new string[] { program, bin });
        }
    }
}
