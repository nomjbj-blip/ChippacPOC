using System;

namespace DACrux.Framework.DSL
{
    /// <summary>
    /// Class Name : TQC_GROUP_FUNCTION<br/>
    /// Summary    : Access for TQ_GROUP_FUNCTION Table<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQC_GROUP_FUNCTION : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_GROUP_FUNCTION()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_GROUP_FUNCTION.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region InsertGroupFunction

        /// <summary>
        /// Insert Group Function
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <param name="strFunCode">Function Code</param>
        /// <returns>Success Code</returns>
        public bool InsertGroupFunction(string strGroupCode, string strFunctionCode)
        {
            try
            {
                this.GetDataTable("INSERTGROUPFUNCTION", null, new string[] { strGroupCode, strFunctionCode });

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteGroupAllFunction

        /// <summary>
        /// Delete All AvaiLabel Function on Selected Group
        /// </summary>
        /// <param name="strGrpCode">Selected Group Code</param>
        /// <returns>Success Bool</returns>
        public bool DeleteGroupAllFunction(string strGroupCode)
        {
            try
            {
                this.GetDataTable("DELETEGROUPALLFUNCTION", null, new string[] { strGroupCode });

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
