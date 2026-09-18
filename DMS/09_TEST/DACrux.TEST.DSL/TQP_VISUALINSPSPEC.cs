using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_VISUALINSPSPEC : Miracom.Middleware.QueryComponent
    {
        public TQP_VISUALINSPSPEC()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_VISUALINSPSPEC.xml");
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

        public DataTable GetVisualInspTypeList()
		{
			DataTable dt = null;
			try
			{
				dt = this.GetDataTable("SELECT_VISUALINSPSPEC_LIST",null,null);
				return dt;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
			}
		}

		public DataTable GetVisualInspSpecOfType(string strInspType)
		{
			DataTable dt = null;
			try
			{
				dt = this.GetDataTable("SELECT_SEPC_OF_TYPE",null,new string[] {strInspType});
				return dt;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
			}
		}

		public void CreateNewInspType(string strInspType)
		{
			string[,] strPara = new string[100,6];
			try
			{
				for(int i=0;i<100;i++)
				{
					strPara[i,0] = strInspType;
					strPara[i,1] = i.ToString();
					if(i==0)
					{
						strPara[i,2] = "Good";
						strPara[i,4] = string.Format("Pass",i);
					}
					else
					{
						strPara[i,2] = string.Format("BIN{0:0#}",i);
						strPara[i,4] = string.Format("Visual Fail - {0} ",i);
					}
					strPara[i,3] = ColorString(i);
                    strPara[i, 5] = string.Empty;
				}

				this.ExecuteMultiple("CREATE_VISUALINSPSPECTYPE",strPara);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        public void DeleteInspType(string strInspType)
        {
            //DELETE_INSPTYPE

            try
            {
                this.Execute("DELETE_INSPTYPE", null, new string[] { strInspType });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public void UpdateSpec(string[,] strPara)
		{
			try
			{
				this.ExecuteMultiple("UPDATE_SEPC",strPara);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        public DataTable GetAVISpec(string VIType)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_SPECLIST", null, new string[] {VIType});
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }

        }

        public void UpdateRawdata(string TABLE, string VISUALINSP, string WAFER_SEQ, string X, string Y)
        {
            try
            {
                this.Execute("UPDATE_RAWDATA", new string[] { TABLE }, new string[] { VISUALINSP, WAFER_SEQ, X, Y });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRawdataDMS(string VISUALINSP, string STEP_SEQ, string X, string Y)
        {
            try
            {
                this.Execute("UPDATE_RAWDATA_DMS", new string[] { X, Y }, new string[] { VISUALINSP, STEP_SEQ});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateReturnRawdata(string TABLE, string WAFER_SEQ)
        {
            try
            {
                this.Execute("UPDATE_RETURN_RAWDATA", new string[] { TABLE }, new string[] { WAFER_SEQ });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateReturnRawdataDMS(string STEP_SEQ)
        {
            try
            {
                this.Execute("UPDATE_RETURN_RAWDATA_DMS", null, new string[] { STEP_SEQ });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEDSBINDesc(string PROGRAM)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_EDSBINDESC", null, new string[] { PROGRAM });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal int[] m_iColor = {16766976,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
											 65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
											 16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
											 10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
											 8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
											 6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
											 12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
											 10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
											 2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
											 10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686,
											 
											 16766976,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
											 65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
											 16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
											 10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
											 8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
											 6069641,0,4259584,7242348,7552844,2613922,11377144,8723452,3745060,6005922,
											 12720820,2615727,6475744,8781431,13882444,273280,13391644,13317614,15138683,13399885,
											 10731647,8333305,7428478,5658018,12090672,6983060,5689642,1055945,1918105,5796598,
											 2192459,10882320,267359,14939131,8427326,16100701,14189729,8599505,148945,3303408,
											 10243695,14070241,1305178,5595511,8657805,2921728,3620885,13344342,13733736,5609686,

											 16766976,65281,4194432,32896,8421631,8404992,16744703,64,65535,32768,
											 65280,16448,12615680,16711808,16776960,4210816,4227327,12615808,14817052,8454016,
											 16744448,4194432,16744576,12615935,8388863,16777088,33023,65408,4227072,16711680,
											 10485760,8388736,255,8454143,16512,16384,4210688,8388608,4194304,4194368,
											 8388672,8453888,32896,4227200,8421504,4227136,12632256,9868950,6795178,4446555,
											 6069641,0,4259584,7242348,7552844,2613922};
        public string ColorString(int iBinNumber)
        {
            string strColor = string.Empty;
            int r = (m_iColor[iBinNumber] >> 16);
            int g = (m_iColor[iBinNumber] >> 8) - (r * 256);
            int b = m_iColor[iBinNumber] - (r * 65536) - (g * 256);

            strColor = string.Format("{0:00#}{1:00#}{2:00#}", r, g, b);
            return strColor;
        }

        
    }
}
