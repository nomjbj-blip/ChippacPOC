using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.Base;
using DACrux.Framework.Interface;


namespace DACrux.Framework.RO
{
    /// <summary>
    /// Class Name : UserControl<br/>
    /// Summary    : User Control Definition Remoting Object Class<br/>
    /// Author     : Miracom Andy Kuo<br/>
    /// First Date : 2009-03-06<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserControl
    {
        #region ClassMember

        DACrux.Framework.Interface.iDACruxUserControl m_OBJ = null;
        
        #endregion

        #region Create
        /// <summary>
        /// Initialize Class
        /// </summary>
        public UserControl()
        {
            try
            {
                string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
                object obj = Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxUserControl),
                    strUrl + "/DACrux.Framework.BSL.UserControl.bin");
                m_OBJ = obj as DACrux.Framework.Interface.iDACruxUserControl;
                System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion 

        #region GetFacilityList

        /// <summary>
        /// Get Total Facility List
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetFacilityList()
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetFacilityList();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        public string GetFacility(string strMeasureEQ)
        {
            try
            {
                return m_OBJ.GetFacility(strMeasureEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetDeviceList

        /// <summary>
        /// Get Device (Model) List
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetDeviceList(string strFacility)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetDeviceList(strFacility);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        #endregion

        #region GetProductList

        /// <summary>
        /// Get Product List
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strPartID">Device</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetProductList(string strFacility, string strPartID)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetProductList(strFacility, strPartID);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        #endregion

        #region GetProgramList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFacility"></param>
        /// <param name="strPartID"></param>
        /// <param name="strProduct"></param>
        /// <returns></returns>
        public DataTable GetProgramList(string strFacility, string strPartID, string strProduct)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetProgramList(strFacility, strPartID, strProduct);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region
        public DataTable GetEquipList()
        {
            DataTable dt = null;

            try
            {
                dt = m_OBJ.GetEquipList();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetHbinList

        public DataTable GetHbinList(string strFacility, string strPartID, string strFullPartID)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetHbinList(strFacility, strPartID, strFullPartID);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion        

        #region GetBinList

        public DataTable GetBinList(string strFacility, string strPartID, string strFullPartID, string strHbin)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetBinList(strFacility, strPartID, strFullPartID, strHbin);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion        

        #region GetFlowList

        /// <summary>
        /// Get Flow List
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strProduct">Product</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetFlowList(string strFacility, string strProduct)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetFlowList(strFacility, strProduct);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }      

        #endregion

        #region GetOperList

        /// <summary>
        /// Get Oper List
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strProduct">Product</param>
        /// <param name="strFlow">Flow</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetOperList(string strFacility, string strProduct, string strFlow)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetOperList(strFacility, strProduct, strFlow);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        #endregion

        #region GetResList

        /// <summary>
        /// Get Res (Equipment) List
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strProduct">Product</param>
        /// <param name="strFlow">Flow</param>
        /// <param name="strOper">Oper</param>
        /// <param name="bProcess">true: Process EQ, false: Measurement EQ</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetResList(string strFacility, string strProduct, string strFlow, string strOper, bool bProcess)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetResList(strFacility, strProduct, strFlow, strOper, bProcess);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        /// <summary>
        /// Get Res(EQ) List By Summary Type(In DB TableName List)
        /// </summary>
        /// <param name="strSummaryType">"GLS" or "LOT" or "PNL" or "QPN"</param>
        /// <returns></returns>
        public DataTable GetResList(string strSummaryType)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetResList(strSummaryType);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        #endregion

        #region GetFlowListByRes

        /// <summary>
        /// Get Flow List By Equipment
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strRes">Res (Equipment)</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetFlowListByRes(string strFacility, string strRes)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetFlowListByRes(strFacility, strRes);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        #endregion

        #region GetOperListByResFlow

        /// <summary>
        /// Get Flow List By Equipment
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strRes">Res (Equipment)</param>
        /// <param name="strFlow">Flow</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetOperListByResFlow(string strFacility, string strRes, string strFlow)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetOperListByResFlow(strFacility, strRes, strFlow);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        #endregion

        #region GetParaList

        /// <summary>
        /// Get Para List 
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strProduct">Product</param>
        /// <param name="strFlow">Flow</param>
        /// <param name="strOper">Oper</param>
        /// <param name="strRes">Process EQ</param>
        /// <param name="strTestProgram">Test Program</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetParaList(string strFacility, string strProduct, string strFlow, string strOper, string strRes, string strTestProgram)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetParaList(strFacility, strProduct, strFlow, strOper, strRes, strTestProgram);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        #endregion

        #region GetLotTypeList
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetLotTypeList(string strFacility)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetLotTypeList(strFacility);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetYmsLotList

        public DataTable GetYmsLotList(string strStartDate, string strEndDate, string strFacility, string strDevice, string strProduct, string strFlow, string strOper, string strEq, string strLotType, bool bOnlyGlass)
        {
            DataTable dt = null;

            try
            {
                dt = m_OBJ.GetYmsLotList(strStartDate, strEndDate, strFacility, strDevice, strProduct, strFlow, strOper, strEq, strLotType, bOnlyGlass);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        #endregion

        #region GetYmsUnitList

        public DataTable GetYmsUnitList(string strLotSeq)
        {
            DataTable dt = null;

            try
            {
                dt = m_OBJ.GetYmsUnitList(strLotSeq);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }

        #endregion

        #region GetEWSMatList
        /// <summary>
        /// GetEWSMatList
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSMatList(string strFacility)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetEWSMatList(strFacility);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetUserList
        /// <summary>
        /// GetUserList
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetUserList()
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetUserList();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetUserNameList
        /// <summary>
        /// GetUserList
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetUserNameList()
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetUserNameList();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetEWSTestMode
        /// <summary>
        /// GetEWSTestMode
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSTestMode(string strFacility)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetEWSTestMode(strFacility);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetEWSStep
        /// <summary>
        /// GetEWSStep
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSStep(string strFacility)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetEWSStep(strFacility);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetEWSTestGroup
        /// <summary>
        /// GetEWSStep
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSTestGroup(string strFacility)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetEWSTestGroup(strFacility);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetEWSUse
        /// <summary>
        /// GetEWSStep
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSUse(string strFacility)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetEWSUse(strFacility);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetEWSBoardType
        /// <summary>
        /// GetEWSStep
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSBoardType(string strFacility)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetEWSBoardType(strFacility);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetProductListByNothing
        /// <summary>
        /// GetProductListByNothing
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetProductListByNothing(string strFacility)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetProductListByNothing(strFacility);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion


        #region GetParaListByNothing
        /// <summary>
        /// GetProductListByNothing
        /// </summary>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetParaListByNothing()
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetParaListByNothing();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion


        public DataTable GetFlowListByHTKIM(string strMeasureEQ)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        #region GetCustomerList
        public DataTable GetCustomerList()
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetCustomerList();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetYMSProductList
        public DataTable GetYMSProductList(string strCustomer)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetYMSProductList(strCustomer);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetYMSDeviceList
        public DataTable GetYMSDeviceList(string strCustomer)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetYMSDeviceList(strCustomer);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetYMSOperList
        public DataTable GetYMSOperList()
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetYMSOperList();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetBinDesc
        public DataTable GetBinDesc(string strPartID)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetBinDesc(strPartID);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion

        #region GetBinDesc
        public DataTable GetHandlerList()
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetHandlerList();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
            }
        }
        #endregion
    }
}
