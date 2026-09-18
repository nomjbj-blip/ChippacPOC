#define WINDOWS2003
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using DACrux.Base;
using DACrux.Framework.Interface;
using DACrux.Framework.DSL;
using System.Transactions;

namespace DACrux.Framework.BSL
{
    public class UserControl : Miracom.Middleware.BaseComponent, iDACruxUserControl
    {
        #region GetFacilityList

        /// <summary>
        /// Get Total Facility List
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetFacilityList()
        {
            DataTable dt = null;
            DACrux.Framework.DSL.TQC_USER_CONTROL oUserControl = null;
            
            try
            {
                oUserControl = new TQC_USER_CONTROL(); 

                dt = oUserControl.GetFacilityList();
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        public string GetFacility(string strMeasureEQ)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();

                dt = oUserControl.GetFacility(strMeasureEQ);
                if (dt == null || dt.Rows.Count < 0)
                    return string.Empty;
                else
                    return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
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
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetDeviceList(strFacility);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
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
            TQC_USER_CONTROL oUserControl = null;
            string[] arrTemp = null;
            try
            {
                oUserControl = new TQC_USER_CONTROL();
                if (strPartID.IndexOf(',') == -1)
                    dt = oUserControl.GetProductList(strFacility, strPartID);
                else
                {
                    arrTemp = strPartID.Split(',');
                    for (int i = 0; i < arrTemp.Length; i++)
                    {
                        arrTemp[i].Replace("'", "");
                        arrTemp[i] = "'" + arrTemp[i].Trim() + "'";
                    }
                    strPartID = string.Join(",", arrTemp);
                    dt = oUserControl.GetProductList2(strFacility, strPartID);
                }

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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetProgramList

        public DataTable GetProgramList(string strFacility, string strPartID, string strProduct)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;
            try
            {
                oUserControl = new TQC_USER_CONTROL();
                if (strFacility != string.Empty && strPartID == string.Empty && strProduct == string.Empty)
                    dt = oUserControl.GetProgramList(strFacility);
                else if (strFacility != string.Empty && strPartID != string.Empty && strProduct == string.Empty)
                    dt = oUserControl.GetProgramList(strFacility, strPartID);
                else if (strFacility != string.Empty && strPartID != string.Empty && strProduct != string.Empty)
                    dt = oUserControl.GetProgramList(strFacility, strPartID, strProduct);
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
                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetEquipList
        public DataTable GetEquipList()
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;
            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetEquipList();
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
        #endregion

        #region GetHbinList

        public DataTable GetHbinList(string strFacility, string strPartID, string strFullPartID)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;
            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetHbinList(strFacility, strPartID, strFullPartID);
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
                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetBinList

        public DataTable GetBinList(string strFacility, string strPartID, string strFullPartID, string strHbin)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;
            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetBinList(strFacility, strPartID, strFullPartID, strHbin);
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
                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion


        #region GetFlowList

        /// <summary>
        /// Get Flow List
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strProduct">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetFlowList(string strFacility, string strProduct)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetFlowList(strFacility, strProduct);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
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
            TQC_USER_CONTROL oUserControl = null;
            string[] arrTemp = null;
            try
            {
                oUserControl = new TQC_USER_CONTROL();
                if (strFlow.IndexOf(',') == -1)
                    dt = oUserControl.GetOperList(strFacility, strProduct, strFlow);
                else
                {
                    arrTemp = strFlow.Split(',');
                    for (int i = 0; i < arrTemp.Length; i++)
                    {
                        arrTemp[i].Replace("'", "");
                        arrTemp[i] = "'" + arrTemp[i].Trim() + "'";
                    }
                    strFlow = string.Join(",", arrTemp);
                    dt = oUserControl.GetOperList2(strFacility, strProduct, strFlow);
                }
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
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
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetResList(strFacility, strProduct, strFlow, strOper, bProcess);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        public DataTable GetResList(string strSummaryType)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetResList("TQC_DMS_" + strSummaryType + "_SUM_%");
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
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
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();

                dt = oUserControl.GetFlowListByRes(strFacility, strRes);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
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
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();

                dt = oUserControl.GetOperListByResFlow(strFacility, strRes, strFlow);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetParaList

        /// <summary>
        /// Get Para List By Process Equipment
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
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();

                dt = oUserControl.GetParaList(strFacility, strProduct, strFlow, strOper, strRes, strTestProgram);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
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
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();

                dt = oUserControl.GetLotTypeList(strFacility);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetYmsLotList

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFacility"></param>
        /// <param name="strPartID"></param>
        /// <param name="strProduct"></param>
        /// <param name="strFlow"></param>
        /// <param name="strOper"></param>
        /// <param name="strEq"></param>
        /// <param name="strLotType"></param>
        /// <returns></returns>
        public DataTable GetYmsLotList(string strStartDate, string strEndDate, string strFacility, string strPartID, string strProduct, string strFlow, string strOper, string strEq, string strLotType, bool bOnlyGlass)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();

                dt = oUserControl.GetYmsLotList(strStartDate, strEndDate, strFacility, strPartID, strProduct, strFlow, strOper, strEq, strLotType, bOnlyGlass);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetYmsUnitList

        public DataTable GetYmsUnitList(string strLotSeq)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();

                dt = oUserControl.GetYmsUnitList(strLotSeq);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetEWSMatList

        /// <summary>
        /// GetEWSMatList
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetEWSMatList(string strFacility)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetEWSMatList(strFacility);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetUserList
        /// <summary>
        /// GetUserList
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetUserList()
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetUserList();
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetUserNameList
        /// <summary>
        /// GetUserNameList
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetUserNameList()
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetUserNameList();
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion


        #region GetEWSTestMode

        /// <summary>
        /// GetEWSTestMode
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetEWSTestMode(string strFacility)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetEWSTestMode(strFacility);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetEWSStep

        /// <summary>
        /// GetEWSStep
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetEWSStep(string strFacility)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetEWSStep(strFacility);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetEWSTestGroup

        /// <summary>
        /// GetEWSStep
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetEWSTestGroup(string strFacility)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetEWSTestGroup(strFacility);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetEWSUse

        /// <summary>
        /// GetEWSStep
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetEWSUse(string strFacility)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetEWSUse(strFacility);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetEWSBoardType

        /// <summary>
        /// GetEWSBoardType
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetEWSBoardType(string strFacility)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetEWSBoardType(strFacility);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetProductListByNothing

        /// <summary>
        /// GetEWSStep
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetProductListByNothing(string strFacility)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetProductListByNothing(strFacility);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion


        #region GetParaListByNothing

        /// <summary>
        /// GetEWSStep
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetParaListByNothing()
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetParaListByNothing();
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion


        #region GetCustomerList
        public DataTable GetCustomerList()
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = new TQC_USER_CONTROL();
            try
            {
                //oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetCustomerList();
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }

        }

        #endregion

        #region GetYMSProductList

        public DataTable GetYMSProductList(string strCustomer)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetYMSProductList(strCustomer);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetYMSDeviceList

        public DataTable GetYMSDeviceList(string strCustomer)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetYMSDeviceList(strCustomer);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetYMSOperList

        public DataTable GetYMSOperList()
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetYMSOperList();
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetBinDesc

        public DataTable GetBinDesc(string strPartID)
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetBinDesc(strPartID);
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion

        #region GetHandlerList

        public DataTable GetHandlerList()
        {
            DataTable dt = null;
            TQC_USER_CONTROL oUserControl = null;

            try
            {
                oUserControl = new TQC_USER_CONTROL();
                dt = oUserControl.GetHandlerList();
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

                if (oUserControl != null) oUserControl.Dispose();
                oUserControl = null;
            }
        }

        #endregion  
    }
}
