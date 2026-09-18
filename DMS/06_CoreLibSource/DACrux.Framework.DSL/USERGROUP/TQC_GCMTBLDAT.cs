using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Framework.DSL
{
    public class TQC_GCMTBLDAT : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_GCMTBLDAT()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_GCMTBLDAT.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public DataTable GetGcm()
        {
            try
            {
                return this.GetDataTable("GET_GCM_DATA", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertGcmDataSearchRule(string FACTORY, string SEARCH_ID)
        {
            try
            {
                this.Execute("INSERT_GCM_SEARCH_RULE", null, new string[] { FACTORY, SEARCH_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void InsertGcmDataFTPOption(string FACTORY, string FTP_IP, string FPT_PORT, string FTP_USER_ID, string FTP_PASSWORD, string GOLDEN, string EQUIPMENT_CMP, string MHS)
        {
            try
            {
                this.Execute("INSERT_GCM_FTP_OPTION", null, new string[] { FACTORY, FTP_IP, FPT_PORT, FTP_USER_ID, FTP_PASSWORD, GOLDEN, EQUIPMENT_CMP, MHS });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void InsertGcmDataFTPOption(string FACTORY, string DAY, string SWING, string NIGHT)
        {
            try
            {
                this.Execute("INSERT_GCM_SHIFT_ZONE", null, new string[] { FACTORY, DAY, SWING, NIGHT });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void InsertGcmDataCompareOption(string FACTORY, int INTERVAL, string START_TIME)
        {
            try
            {
                this.Execute("INSERT_GCM_COMPARE_OPTION", null, new string[] { FACTORY, INTERVAL.ToString(), START_TIME });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void InsertGcmDataH101Config(string FACTORY, string H101SERVER_IP, string H101COM, string H101PORT, string H101CHANEL)
        {
            try
            {
                this.Execute("INSERT_GCM_H101_CONFIG", null, new string[] { FACTORY, H101SERVER_IP, H101COM, H101PORT, H101CHANEL });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void UpdateGcmDataCompareOption(string FACTORY, int INTERVAL, string START_TIME)
        {
            try
            {
                this.Execute("UPDATE_GCM_COMPARE_OPTION", null, new string[] { INTERVAL.ToString(), START_TIME, FACTORY });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void UpdateGcmDataH101Config(string FACTORY, string H101SERVER_IP, string H101COM, string H101PORT, string H101CHANEL)
        {
            try
            {
                this.Execute("UPDATE_GCM_H101_CONFIG", null, new string[] { H101SERVER_IP, H101COM, H101PORT, H101CHANEL, FACTORY });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void UpdateGcmDataFTPOption(string FACTORY, string FTP_IP, string FPT_PORT, string FTP_USER_ID, string FTP_PASSWORD, string GOLDEN, string EQUIPMENT_CMP, string MHS)
        {
            try
            {
                this.Execute("UPDATE_GCM_FTP_OPTION", null, new string[] { FTP_IP, FPT_PORT, FTP_USER_ID, FTP_PASSWORD, GOLDEN, EQUIPMENT_CMP, MHS, FACTORY });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void UpdateGcmDataSearchRule(string FACTORY, string SEARCH_ID)
        {
            try
            {
                this.Execute("UPDATE_GCM_SEACH_RULE", null, new string[] { SEARCH_ID, FACTORY });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void UpdateGcmDataShiftZone(string FACTORY, string DAY, string SWING, string NIGHT)
        {
            try
            {
                this.Execute("UPDATE_GCM_SHIFT_ZONE", null, new string[] { DAY, SWING, NIGHT, FACTORY });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
    }
}
