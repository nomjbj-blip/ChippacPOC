using System;

namespace DACrux.Framework.DSL
{
    /// <summary>
    /// Class Name : TQC_CONFIG<br/>
    /// Summary    : Access for TQC_CONFIG Table<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-06-29<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQC_CONFIG : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_CONFIG()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_CONFIG.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetConfigValue

        /// <summary>
        /// Get Config Value 
        /// </summary>
        /// <param name="strCategory">Category String</param>
        /// <param name="strName">Item Name</param>
        /// <returns>Result Data Table</returns>
        public string GetConfigValue(string strCategory, string strName)
        {
            string strReturn = string.Empty;

            try
            {
                strReturn = this.GetDataTable("GetConfigValue", null, new string[] { strCategory, strName }).Rows[0][0].ToString();
                return strReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
