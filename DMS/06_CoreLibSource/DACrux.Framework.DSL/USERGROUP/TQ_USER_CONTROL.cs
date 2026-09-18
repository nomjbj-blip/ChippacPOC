using System;
using System.Data;

namespace DACrux.Framework.DSL
{
    /// <summary>
    /// Class Name : TQ_USER_CONTROL<br/>
    /// Summary    : Access for Common DataTable For Control<br/>
    /// Author     : Miracom AndyKuo<br/>
    /// First Date : 2009-03-25<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQC_USER_CONTROL : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_USER_CONTROL()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "EMS.PKG.TQ_COM_USER_CONTROL.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        #region GetFacilityList

        /// <summary>
        /// Get Facility List
        /// </summary>
        /// <returns>Result DataTable</returns>
        public DataTable GetFacilityList()
        {
            try
            {
                return this.GetDataTable("GetFacilityList", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFacility(string strMeasureEQ)
        {
            try
            {
                return this.GetDataTable("GetFacilityByEQ", null, new string[] { strMeasureEQ });
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
            try
            {
                return this.GetDataTable("GetDeviceList", null, new string[] { strFacility });
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                return this.GetDataTable("GetProductList", null, new string[] { strFacility, strPartID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Product List
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strPartID">,로 구분된 string(ex. 'Device1', 'Device2')</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetProductList2(string strFacility, string strPartID)
        {
            try
            {
                return this.GetDataTable("GetProductList2", new string[] { strPartID }, new string[] { strFacility });
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                return this.GetDataTable("GetProgramList", null, new string[] { strFacility, strPartID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProgramList(string strFacility, string strPartID)
        {
            try
            {
                return this.GetDataTable("GetProgramList2", null, new string[] { strFacility, strPartID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProgramList(string strFacility)
        {
            try
            {
                return this.GetDataTable("GetProgramList3", null, new string[] { strFacility });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region GetEquipList
        public DataTable GetEquipList()
        {
            try
            {
                return this.GetDataTable("GetEquipList", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetHbinList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFacility"></param>
        /// <param name="strPartID"></param>
        /// <param name="strProduct"></param>
        /// <param name="strProgram"></param>
        /// <returns></returns>
        public DataTable GetHbinList(string strFacility, string strPartID, string strFullPartID)
        {
            try
            {
                return this.GetDataTable("GetHbinList", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetBinList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFacility"></param>
        /// <param name="strPartID"></param>
        /// <param name="strProduct"></param>
        /// <param name="strProgram"></param>
        /// <param name="strHbin"></param>
        /// <returns></returns>
        public DataTable GetBinList(string strFacility, string strPartID, string strFullPartID, string strHbin)
        {
            try
            {
                return this.GetDataTable("GetBinList", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                if (strFacility.Trim() != string.Empty && strProduct.Trim() != string.Empty)
                    return this.GetDataTable("GetFlowList", null, new string[] { strFacility, strProduct });
                else
                    return this.GetDataTable("GetFlowListByFacility", null, new string[] { strFacility });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region GetFlowListByRes

        /// <summary>
        /// Get Flow List By Equipment
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strRes">Res (Equipment)</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetFlowListByRes(string strFacility, string strRes)
        {
            try
            {
                return this.GetDataTable("GetFlowListByRes", null, new string[] { strFacility, strRes });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

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
            try
            {
                //if (strProduct.Trim() == string.Empty)
                //    return this.GetDataTable("GetOperListByFacilityFlow", null, new string[] { strFacility, strFlow });
                //else
                return this.GetDataTable("GetOperList", null, new string[] { strFacility, strFlow });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Oper List2
        /// </summary>
        /// <param name="strFacility">Facility</param>
        /// <param name="strProduct">Product</param>
        /// <param name="strFlow">,로 구분된 string(ex. 'Flow1', 'Flow2')</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetOperList2(string strFacility, string strProduct, string strFlow)
        {
            try
            {
                return this.GetDataTable("GetOperListByFacilityFlow2", new string[] { strFlow }, new string[] { strFacility });
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                return this.GetDataTable("GetOperListByResFlow", null, new string[] { strFacility, strRes, strFlow });
            }
            catch (Exception ex)
            {
                throw ex;
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
            string strType = "P";

            try
            {
                if (!bProcess) strType = "M";

                if (strProduct.Trim() == string.Empty)
                    return this.GetDataTable("GetResListByFacility", null, new string[] { strFacility, strType });
                else
                {
                    if(strOper == string.Empty)
                        return this.GetDataTable("GetResList", new string[] { string.Empty }, new string[] { strFacility, strFlow, strType });
                    else
                    {
                        if (strOper.Contains(","))
                            strOper = "AND A.OPER IN ('" + strOper.Replace(",", "','") + "') ";
                        else
                            strOper = "AND A.OPER = '" + strOper + "' ";
                    }

                    return this.GetDataTable("GetResList", new string[] { strOper }, new string[] { strFacility, strFlow, strType });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Equipment list
        /// </summary>
        /// <param name="strSummaryTbName">Part of Summary Table Name</param>
        /// <returns>Equip List by summary table name</returns>
        public DataTable GetResList(string strSummaryTbName)
        {
            try
            {
                return this.GetDataTable("GetResListBySummaryType", null, new string[] { strSummaryTbName });
            }
            catch (Exception ex)
            {
                throw ex;
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
            int iProduct;
            string[] strTemp = null;
            string strDynamic = string.Empty;
            string strSplit = string.Empty;
            try
            {
                if (strRes != string.Empty)
                {
                    iProduct = strProduct.IndexOf(",");

                    if (iProduct > 0)
                    {
                        strTemp = strProduct.Split(',');

                        for (int i = 0; i < strTemp.Length; i++)
                        {
                            if (i == strTemp.Length - 1)
                                strSplit = strSplit + "'" + strTemp[i].ToString().Trim() + "'";
                            else
                                strSplit = strSplit + "'" + strTemp[i].ToString().Trim() + "',";
                        }

                        strDynamic = "AND PRODUCT IN (" + strSplit + ")";

                        return this.GetDataTable("GetParaListByProcessEQ1", new string[] { strDynamic }, new string[] { strFacility, strFlow, strOper, strRes });
                    }
                    else
                    {
                        //if (strProduct == "All" || strProduct == string.Empty)
                        //    return this.GetDataTable("GetParaListByProcessEQ2", null, new string[] { strFacility, strFlow, strOper, strRes });
                        //else
                        //    return this.GetDataTable("GetParaListByProcessEQ", null, new string[] { strFacility, strProduct, strFlow, strOper, strRes });

                        return this.GetDataTable("GetParaByFacility", null, new string[] { strFacility });
                    }
                }
                else if(strTestProgram != string.Empty)
                    return this.GetDataTable("GetParaByProduct", null, new string[] { strFacility, strProduct, strTestProgram });
                else
                    return this.GetDataTable("GetParaList", null, new string[] { strFacility, strProduct, strFlow, strOper, string.Empty });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetParaListByNothing
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetParaListByNothing()
        {
            try
            {
                return GetDataTable("GetParaListByNothing", null, null);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetYmsLotList
        /// <summary>
        /// GetYmsLotList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strStartDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="strFacility"></param>
        /// <param name="strPartID"></param>
        /// <param name="strProduct"></param>
        /// <param name="strFlow"></param>
        /// <param name="strOper"></param>
        /// <param name="strEq"></param>
        /// <param name="strLotType"></param>
        /// <param name="bGlassOnly"></param>
        /// <returns></returns>
        public DataTable GetYmsLotList(string strStartDate, string strEndDate, string strFacility, string strPartID, string strProduct, string strFlow, string strOper, string strEq, string strLotType, bool bGlassOnly)
        {
            DataTable dt = null;
            string strDynamic = string.Empty;

            try
            {
                strDynamic += "   AND PRC.FACILITY = '" + strFacility + "' " + Environment.NewLine;

                if(strPartID != string.Empty)
                    strDynamic += "   AND PRC.DEVICE = '" + strPartID + "' " + Environment.NewLine;

                if(strProduct != string.Empty)
                    strDynamic += "   AND PRC.PRODUCT = '" + strProduct + "' " + Environment.NewLine;

                if(strFlow != string.Empty)
                    strDynamic += "   AND PRC.FLOW = '" + strFlow + "' " + Environment.NewLine;

                if(strOper != string.Empty)
                {
                    if(strOper.Contains(","))
                        strDynamic += "   AND PRC.OPER IN ('" + strOper.Replace(",", "','") + "') " + Environment.NewLine;
                    else
                        strDynamic += "   AND PRC.OPER = '" + strOper + "' " + Environment.NewLine;
                }

                if (strEq != string.Empty)
                {
                    if(strEq.Contains(","))
                        strDynamic += "   AND PRC.RES_ID IN ('" + strEq.Replace(",", "','") + "') " + Environment.NewLine;
                    else
                        strDynamic += "   AND PRC.RES_ID = '" + strEq + "' " + Environment.NewLine;
                }

                if(strLotType != string.Empty)
                {
                    if (strLotType.Contains(","))
                        strDynamic += "   AND LOT.LOT_TYPE IN ('" + strLotType.Replace(",", "','") + "') " + Environment.NewLine;
                    else
                        strDynamic += "   AND LOT.LOT_TYPE IN ('" + strLotType + "') " + Environment.NewLine;
                }

                if (bGlassOnly)
                    strDynamic += "   AND LOT.LOT_MODE = 'GLS' " + Environment.NewLine;

                dt = GetDataTable("GetYmsLotList", new string[] { strDynamic }, new string[]{strStartDate, strEndDate});

                return dt;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetYmsUnitList
        /// <summary>
        /// GetYmsUnitList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strLotSeq"></param>
        /// <returns></returns>
        public DataTable GetYmsUnitList(string strLotSeq)
        {
            if (strLotSeq.Trim() == string.Empty)
                return null;

            try
            {
                if (strLotSeq.Contains(","))
                {
                    return GetDataTable("GetYmsUnitListMulti", new string[] { strLotSeq.Replace(",","','") }, null);
                }
                else
                {
                    return GetDataTable("GetYmsUnitListSingle", null, new string[] { strLotSeq });
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GetLotTypeList
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetLotTypeList(string strFacility)
        {
            if (strFacility.Trim() == string.Empty)
                return null;

            try
            {
                return GetDataTable("GetLotTypeList", null, new string[] { strFacility });
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetEWSMatList
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSMatList(string strFacility)
        {
            try
            {
                return GetDataTable("GetEWSMatList", null, new string[] { strFacility });
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetUserList
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetUserList()
        {
            try
            {
                return GetDataTable("GetUserList", null, null);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetUserNameList
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetUserNameList()
        {
            try
            {
                return GetDataTable("GetUserNameList", null, null);
            }
            catch
            {
                throw;
            }
        }
        #endregion
        
        #region GetEWSTestMode
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSTestMode(string strFacility)
        {
            try
            {
                return GetDataTable("GetEWSTestMode", null, new string[] { strFacility });
            }
            catch
            {
                throw;
            }
        }
        #endregion
        
        #region GetEWSBoardType
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSBoardType(string strFacility)
        {
            try
            {
                return GetDataTable("GetEWSBoardType", null, new string[] { strFacility });
            }
            catch
            {
                throw;
            }
        }
        #endregion
        
        #region GetEWSStep
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSStep(string strFacility)
        {
            try
            {
                return GetDataTable("GetEWSStep", null, new string[] { strFacility });
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetEWSTestGroup
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSTestGroup(string strFacility)
        {
            try
            {
                return GetDataTable("GetEWSTestGroup", null, new string[] { strFacility });
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetEWSUse
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetEWSUse(string strFacility)
        {
            try
            {
                return GetDataTable("GetEWSUse", null, new string[] { strFacility });
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetProductListByNothing
        /// <summary>
        /// GetLotTypeList
        /// </summary>
        /// <remarks>Added by Hyungsuk Yang.</remarks>
        /// <param name="strFacility"></param>
        /// <returns></returns>
        public DataTable GetProductListByNothing(string strFacility)
        {
            try
            {
                return GetDataTable("GetProductListByNothing", null, new string[] { strFacility });
            }
            catch
            {
                throw;
            }
        }
        #endregion
        
        #region GetCustomerList

        public DataTable GetCustomerList()
        {
            try
            {
                return GetDataTable("GetCustomerList", null, null);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetYMSProductList

        public DataTable GetYMSProductList(string strCustomer)
        {
            try
            {
                return GetDataTable("GetYMSProductList", null, new string[] { strCustomer });
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetYMSDeviceList

        public DataTable GetYMSDeviceList(string strCustomer)
        {
            try
            {
                return GetDataTable("GetYMSDeviceList", null, new string[] { strCustomer });
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetYMSOperList

        public DataTable GetYMSOperList()
        {
            try
            {
                return GetDataTable("GetYMSOperList", null, null);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetBinDesc

        public DataTable GetBinDesc(string strPartID)
        {
            try
            {
                return GetDataTable("GetBinDesc", null, new string[] { strPartID });
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetHandlerList

        public DataTable GetHandlerList()
        {
            try
            {
                return GetDataTable("GetHandlerList", null, null);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
