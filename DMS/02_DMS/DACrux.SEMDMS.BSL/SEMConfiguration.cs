using System;
using System.Data;
using DACrux.SEMDMS.DSL;
using DACrux.SEMDMS.Interface;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DACrux.SEMDMS.BSL
{
    public class SEMConfiguration
        : Miracom.Middleware.BaseComponent, iSEMConfiguration
    {
        public SEMConfiguration(
            )
        {
        }

        public DataTable GetInfo(
            string Device
            )
        {
            TQD_PRODUCT oProduct = null;
            try
            {
                oProduct = new TQD_PRODUCT();
                return oProduct.GetInfo(Device);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetZone(
            string userId,
            string product
            )
        {
            TQD_ZONE oZone = null;
            try
            {
                oZone = new TQD_ZONE();
                return oZone.GetZone(userId, product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetProduct(
            )
        {
            TQD_PRODUCT oProduct = null;
            try
            {
                oProduct = new TQD_PRODUCT();
                return oProduct.GetProduct();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //---------------------------------------------------------------------------------------

        #region [ TQD_DEFECT_GROUP ]

        //--

        // TQD_DEFECT_GROUP    SELECT
        public DataTable SelectGroupList(
            )
        {
            TQD_DEFECT_GROUP oGroupList = new TQD_DEFECT_GROUP();
            return oGroupList.SelectGroupList();
        }


        public DataTable GetGroup(
            string groupID
            )
        {
            TQD_DEFECT_GROUP oGetGroup = new TQD_DEFECT_GROUP();
            return oGetGroup.SelectGroupID(groupID);
        }

        //--

        // TQD_DEFECT_GROUP    GET
        public bool ExistsDefectGroup(
            string groupId
            )
        {
            TQD_DEFECT_GROUP oGetGroup = new TQD_DEFECT_GROUP();
            object obj = oGetGroup.ExistsDefectGroup(groupId);

            if (obj == null || obj == DBNull.Value)
                return false;

            return Int32.Parse(obj.ToString()) > 0;
        }

        //--

        // TQD_DEFECT_GROUP    CREATE
        public int InsertDefectGroup(
            Dictionary<string, string> dicValues,
            string userid
            )
        {
            TQD_DEFECT_GROUP oInsertGroup = new TQD_DEFECT_GROUP();
            return oInsertGroup.InsertGroup(
                dicValues["GROUP_ID"],
                dicValues["GROUP_NAME"],
                dicValues["DELETE_FLAG"],
                userid
                );
        }

        //--

        // TQD_DEFECT_GROUP    UPDATE
        public int UpdateDefectGroup(
            Dictionary<string, string> dicValues,
            string userid
            )
        {
            TQD_DEFECT_GROUP oUpdateGroup = new TQD_DEFECT_GROUP();
            return oUpdateGroup.UpdateGroup(
                dicValues["GROUP_ID"],
                dicValues["GROUP_NAME"],
                dicValues["DELETE_FLAG"],
                userid
                );
        }

        //--

        // TQD_DEFECT_GROUP    DELETE
        public int DeleteDefectGroup(
            string groupId
            )
        {
            TQD_DEFECT_GROUP oDeleteGroup = new TQD_DEFECT_GROUP();
            return oDeleteGroup.DeleteGroup(groupId);
        }

        //--

        #endregion  [ TQD_DEFECT_GROUP ]

        //---------------------------------------------------------------------------------------

        #region [ TQD_DEFECT_TYPE ]

        public Dictionary<int, String> GetDefectTypeAll(
            )
        {
            Dictionary<int, String> dic = new Dictionary<int, string>();
            using (TQD_DEFECT_TYPE oDefectType = new TQD_DEFECT_TYPE())
            {
                DataTable dt = oDefectType.SelectDefectList();

                int classnumber = -1;
                foreach (DataRow r in dt.Rows)
                {
                    classnumber = Base.Convert.intParse(r["CLASSNUMBER"].ToString());
                    if (!dic.ContainsKey(classnumber))
                        dic.Add(classnumber, r["NAME"].ToString());
                }
            }
            return dic;
        }

        public DataTable GetDefectTypeList(
            string DefectGroupId = "ALL"
            )
        {
            TQD_DEFECT_TYPE oDefectType = new TQD_DEFECT_TYPE();
            DataTable dt = oDefectType.SelectDefectList();
            dt.TableName = "TQD_DEFECT_TYPE";
            return dt;
        }

        public bool ExistsDefectType(
            string classnumber
            )
        {
            TQD_DEFECT_TYPE oDefectType = new TQD_DEFECT_TYPE();
            object obj = oDefectType.ExistsDefectType(
                classnumber
                );

            if (obj == null || obj == DBNull.Value)
                return false;

            return Int32.Parse(obj.ToString()) > 0;
        }

        public bool DeleteDefectType(
            string classnumber
            )
        {
            TQD_DEFECT_TYPE oDefectType = new TQD_DEFECT_TYPE();
            int result = oDefectType.DeleteDefectType(classnumber);
            return result > 0;
        }

        public bool InsertDefectType(
            Dictionary<string, string> dicValues,
            string cid,
            string uid
            )
        {
            TQD_DEFECT_TYPE oDefectType = new TQD_DEFECT_TYPE();
            // Killer Defect or Non Killer Defect에  대한 Grouping 
            string groupId = dicValues["GROUP_ID"].Substring(
                dicValues["GROUP_ID"].IndexOf('(') + 1,
                dicValues["GROUP_ID"].Length - (dicValues["GROUP_ID"].IndexOf('(') + 2)
                );
            int result = oDefectType.InsertDefectByType(
                dicValues["CLASSNUMBER"],
                dicValues["NAME"],
                dicValues["DESCRIPTION"],
                groupId,
                dicValues["DEFECT_COLOR"],
                cid,
                uid
                );
            return result > 0;
        }
        public bool UpdateDefectType(
            Dictionary<string, string> dicValues,
            string cid,
            string uid
            )
        {
            TQD_DEFECT_TYPE oDefectType = new TQD_DEFECT_TYPE();
            // Killer Defect or Non Killer Defect에  대한 Grouping 
            string groupId = dicValues["GROUP_ID"].Substring(
                dicValues["GROUP_ID"].IndexOf('(') + 1,
                dicValues["GROUP_ID"].Length - (dicValues["GROUP_ID"].IndexOf('(') + 2)
                );
            int result = oDefectType.UpdateDefectByType(
                dicValues["NAME"],
                dicValues["DESCRIPTION"],
                groupId,
                dicValues["DEFECT_COLOR"],
                dicValues["CLASSNUMBER"],
                uid
                );
            return result > 0;
        }

        #endregion [ TQD_DEFECT_TYPE ]

        //---------------------------------------------------------------------------------------


        //
        //
        // TQD_COLORBYSIZE     SELECT
        public DataTable SelectColorListByDftSize(
            string userId
            )
        {
            TQD_COLORBYSIZE oColorListS = null;
            DataTable dt = null;
            try
            {
                oColorListS = new TQD_COLORBYSIZE();
                dt = oColorListS.SelectColorListByDefectSize(userId);
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

        // TQD_COLORBYSIZE    CREATE
        public void InsertColorByDftSize(
            string user,
            int[] sizeSeq,
            int[] sizeFrom,
            int[] sizeTo,
            System.Drawing.Color[] color
            )
        {
            TQD_COLORBYSIZE oInsertColorS = null;

            try
            {
                oInsertColorS = new TQD_COLORBYSIZE();
                oInsertColorS.InsertColorByDefectSize(user, sizeSeq, sizeFrom, sizeTo, color);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // TQD_COLORBYSIZE    DELETE
        public void DeleteColorByDftSize(
            string userId
            )
        {
            TQD_COLORBYSIZE oDeleteColorS = null;

            try
            {
                oDeleteColorS = new TQD_COLORBYSIZE();
                oDeleteColorS.DeleteColorByDefectSize(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteDefectSizeByColor(
            string userid,
            string sizeSeq
            )
        {
            TQD_COLORBYSIZE oSizeByColor = new TQD_COLORBYSIZE();
            int iResult = oSizeByColor.DeleteDefectSizeByColor(
                userid,
                sizeSeq
                );

            return iResult > 0;
        }

        public bool ExistsDefectSizeByColor(
            string userid,
            string sizeSeq
            )
        {
            TQD_COLORBYSIZE oSizeByColor = new TQD_COLORBYSIZE();
            object obj = oSizeByColor.ExistsDefectSizeByColor(
                userid,
                sizeSeq
                );

            if (obj == null || obj == DBNull.Value)
                return false;

            return Int64.Parse(obj.ToString()) > 0;
        }

        public bool InsertDefectSizeByColor(
            string userid,
            string sizeSeq,
            string sizeFrom,
            string sizeTo,
            string color
            )
        {
            TQD_COLORBYSIZE oSizeByColor = new TQD_COLORBYSIZE();
            int iResult = oSizeByColor.InsertDefectSizeByColor(
                userid,
                sizeSeq,
                sizeFrom,
                sizeTo,
                color
                );

            return iResult > 0;
        }

        public bool UpdateDefectSizeByColor(
            string userid,
            string sizeSeq,
            string sizeFrom,
            string sizeTo,
            string color
            )
        {
            TQD_COLORBYSIZE oSizeByColor = new TQD_COLORBYSIZE();
            int iResult = oSizeByColor.UpdateDefectSizeByColor(
                userid,
                sizeSeq,
                sizeFrom,
                sizeTo,
                color
                );

            return iResult > 0;
        }

        #region [ TQD_STEP ]


        public DataTable GetProduct(
            string fromDate,
            string toDate
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetConditionProduct(fromDate, toDate);
        }

        public DataTable GetConditionStep(
            string fromDate,
            string toDate,
            string product
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetConditionStep(fromDate, toDate, product);
        }

        public DataTable GetConditionStep(
            string fromDate,
            string toDate,
            string[] products
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetConditionStep(fromDate, toDate, products);
        }


        public DataTable GetConditionLot(
            string fromDate,
            string toDate,
            string product,
            string stepid
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetConditionLot(fromDate, toDate, product, stepid);
        }

        public DataTable GetConditionLot(
            string fromDate,
            string toDate,
            string product,
            string[] stepids
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetConditionLot(fromDate, toDate, product, stepids);
        }


        public DataTable GetConditionLot(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetConditionLot(fromDate, toDate, products, stepids);
        }


        public DataTable GetConditionWafer(
            string fromDate,
            string toDate,
            string product,
            string stepid,
            string[] lots
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetConditionWafer(fromDate, toDate, product, stepid, lots);
        }

        public DataTable GetConditionWafer(
            string fromDate,
            string toDate,
            string product,
            string[] stepids,
            string[] lots
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetConditionWafer(fromDate, toDate, product, stepids, lots);
        }

        public DataTable GetConditionWafer(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids,
            string[] lots,
            bool reviewOnly
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetConditionWafer(fromDate, toDate, products, stepids, lots, reviewOnly);
        }

        #endregion [ TQD_STEP ]

        #region [ TQD_INSP_INFO ]
        public DataTable GetLotInfo(string fromDate, string toDate, string lotid)
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.FindLot(fromDate, toDate, lotid);
        }
        #endregion [ TQD_INSP_INFO ]


        #region [ Defect Eq Trend ]

        public DataTable GetClassList(bool bClass)
        {
            TQD_DEFECT_TYPE oType = new TQD_DEFECT_TYPE();
            return oType.GetClassList(bClass);
        }

        public DataTable GetItemList(string strItemName, string strFilter, string[] strEquip, string[] strRoute, string[] LotID, string[] strStep, string strStartTime, string strEndTime)
        {
            DataTable dtItems = new DataTable();

            DACrux.SEMDMS.DSL.TQC_LOT_HIS oHis = new TQC_LOT_HIS();
            System.Text.StringBuilder sQuery = new System.Text.StringBuilder();

            try
            {
                strFilter = strFilter.Replace("*", "%").Trim();

                if (strItemName == "STEP_ID")
                {
                    if (string.IsNullOrEmpty(strFilter) == false)
                    {
                        sQuery.AppendLine(string.Format(" AND STEP_ID LIKE ('{0}')", strFilter));
                    }

                    if (strEquip != null && strEquip.Length > 0)
                    {
                        sQuery.AppendLine(string.Format(" AND EQUIP IN ('{0}')", string.Join("','", strEquip)));
                    }

                    if (strRoute != null && strRoute.Length > 0)
                    {
                        sQuery.AppendLine(string.Format(" AND LPT IN ('{0}')", string.Join("','", strRoute)));
                    }

                    if (LotID != null && LotID.Length > 0)
                    {
                        sQuery.AppendLine(string.Format(" AND LOT_ID IN ('{0}')", string.Join("','", LotID)));
                    }

                    dtItems = oHis.GetTrendItemListStep(sQuery.ToString(), strStartTime, strEndTime);
                }
                else
                {
                    if (string.IsNullOrEmpty(strFilter) == false)
                    {
                        sQuery.AppendLine(string.Format(" AND {0} LIKE ('{1}')", strItemName, strFilter));
                    }

                    if (strEquip != null && strEquip.Length > 0)
                    {
                        sQuery.AppendLine(string.Format(" AND EQUIP IN ('{0}')", string.Join("','", strEquip)));
                    }

                    if (strRoute != null && strRoute.Length > 0)
                    {
                        sQuery.AppendLine(string.Format(" AND LPT IN ('{0}')", string.Join("','", strRoute)));
                    }

                    if (LotID != null && LotID.Length > 0)
                    {
                        sQuery.AppendLine(string.Format(" AND LOT_ID IN ('{0}')", string.Join("','", LotID)));
                    }

                    if (strItemName == "LPT")
                        strItemName = "LPT , DESCRIPTION";

                    dtItems = oHis.GetTrendItemList(strItemName, sQuery.ToString(), strStartTime, strEndTime);
                }
                return dtItems;
            }
            finally
            {
            }
        }


        public DataSet GetTrendReportReview(
            bool bClassMode, 
            bool bDefectMode,
            bool bNormalized,
            string strModeName, 
            string strParaName, 
            string[] strClass, 
            string strEquip, 
            string[] strRoute, 
            string[] strLotID, 
            string[] strStep, 
            string strStartTime, 
            string strEndTime
            )
        {
            DataSet ds = null;
            DataTable dtData = null;

            DACrux.SEMDMS.DSL.TQC_LOT_HIS oHis = new TQC_LOT_HIS();
            TQD_DEFECT_TYPE oDefecType = new TQD_DEFECT_TYPE();
            DataTable dtDefectType = oDefecType.SelectDefectList();

            StringBuilder sGroup = new StringBuilder();
            StringBuilder sFilter = new StringBuilder();
            StringBuilder sGroupBy = new StringBuilder();
            StringBuilder sOrderBy = new StringBuilder();
            String[] pivotValues = null;
            String pivotValue = String.Empty;

            try
            {
                ds = new DataSet();

                if (bClassMode)
                {
                    pivotValues = new String[strClass.Length];
                    for (int iPara = 0; iPara < strClass.Length; iPara++)
                    {
                        pivotValues[iPara] = String.Format("{0} AS \"CAT{0}\"", strClass[iPara]);
                        strClass[iPara] = String.Format("nvl(CAT{0}, 0)", strClass[iPara]);
                    }

                    pivotValue = String.Format("{0}", String.Join(", ", pivotValues));
                }

                //=================================================================================================================
                //Group Header
                if (strModeName == "TRAN_TIME")
                    sGroup.AppendLine("TO_CHAR(TRAN_TIME, 'YYYY/MM/DD HH24:MI:SS') AS TRAN_TIME, ");
                else if (strModeName == "LOT_ID")
                    sGroup.AppendLine(string.Format(" {0}, ", strModeName));
                else if (strModeName == "WAFER_ID")
                    sGroup.AppendLine(string.Format(" {0}, ", strModeName));
                else
                    throw new Exception("선택된 Item이 정확 하지 않음.");

                if (string.IsNullOrEmpty(strParaName) == false)
                    sGroup.AppendLine(string.Format(" {0}, ", strParaName));

                sGroup.Append(string.Format("SUM({0}) AS DATA ", string.Join(" + ", strClass)));

                //=================================================================================================================
                //AND 조건

                if (strRoute != null && strRoute.Length > 0)
                    sFilter.AppendLine(string.Format(" AND T1.LPT IN ('{0}')", string.Join("','", strRoute)));

                if (strLotID != null && strLotID.Length > 0)
                    sFilter.AppendLine(string.Format(" AND T1.LOT_ID IN ('{0}')", string.Join("','", strLotID)));

                if (strStep != null && strStep.Length > 0)
                    sFilter.AppendLine(string.Format(" AND T2.STEP_ID IN ('{0}')", string.Join("','", strStep)));

                //=================================================================================================================
                //Group By 조건
                if (strModeName == "TRAN_TIME")
                    sGroupBy.Append("TO_CHAR(TRAN_TIME, 'YYYY/MM/DD HH24:MI:SS') ");
                else if (strModeName == "LOT_ID")
                    sGroupBy.Append(string.Format(" {0} ", strModeName));
                else if (strModeName == "WAFER_ID")
                    sGroupBy.Append(string.Format(" {0} ", strModeName));
                else
                {
                    throw new Exception("선택된 Item이 정확 하지 않음.");
                }

                if (string.IsNullOrEmpty(strParaName) == false)
                    sGroupBy.Append(string.Format(", {0} ", strParaName));

                //=================================================================================================================
                //Order By 조건
                if (strModeName == "TRAN_TIME")
                    sOrderBy.Append("TRAN_TIME  ");
                else if (strModeName == "LOT_ID")
                    sOrderBy.Append(string.Format(" {0} ", strModeName));
                else if (strModeName == "WAFER_ID")
                    sOrderBy.Append(string.Format(" {0} ", strModeName));
                else
                {
                    throw new Exception("선택된 Item이 정확 하지 않음.");
                }

                if (string.IsNullOrEmpty(strParaName) == false)
                    sOrderBy.Append(string.Format(", {0} ", strParaName));

                if (bClassMode && bDefectMode)
                {
                    dtData = oHis.GetTrendReportReviewToDefect(sGroup.ToString(), strEquip, sFilter.ToString(), pivotValue, sGroupBy.ToString(), sOrderBy.ToString(), strStartTime, strEndTime);
                    if (dtData == null || dtData.Rows.Count <= 0)
                        throw new Exception("Not Found Data");

                    if (bNormalized)
                        ChartDataNormalized(dtData, "DEFECTS");

                    dtData.TableName = "CHART";
                    ds.Tables.Add(dtData.Copy());

                    dtData = oHis.GetTrendReportReviewToDefectRaw(strEquip, sFilter.ToString(), pivotValue, strStartTime, strEndTime);
                    if (dtData == null || dtData.Rows.Count <= 0)
                        throw new Exception("Not Found Data");

                    if (bNormalized)
                        RawDataNormalized(dtData, "DEFECTS");

                    RawDataTableColumnsChanged(dtData, dtDefectType);
                    dtData.TableName = "RAW";
                    ds.Tables.Add(dtData.Copy());

                    ds.AcceptChanges();
                }
                else if (bClassMode && !bDefectMode)
                {
                    dtData = oHis.GetTrendReportReviewToDefectiveDie(sGroup.ToString(), strEquip, sFilter.ToString(), pivotValue, sGroupBy.ToString(), sOrderBy.ToString(), strStartTime, strEndTime);
                    if (dtData == null || dtData.Rows.Count <= 0)
                        throw new Exception("Not Found Data");

                    if (bNormalized)
                        ChartDataNormalized(dtData, "DEFECTIVE_DIE");

                    dtData.TableName = "CHART";
                    ds.Tables.Add(dtData.Copy());

                    dtData = oHis.GetTrendReportReviewToDefectiveDieRaw(strEquip, sFilter.ToString(), pivotValue, strStartTime, strEndTime);
                    if (dtData == null || dtData.Rows.Count <= 0)
                        throw new Exception("Not Found Data");

                    if (bNormalized)
                        RawDataNormalized(dtData, "DEFECTIVE_DIE");

                    RawDataTableColumnsChanged(dtData, dtDefectType);
                    dtData.TableName = "RAW";
                    ds.Tables.Add(dtData.Copy());

                    ds.AcceptChanges();
                }
                else
                {
                    dtData = oHis.GetTrendReportInspect(sGroup.ToString(), strEquip, sFilter.ToString(), sGroupBy.ToString(), sOrderBy.ToString(), strStartTime, strEndTime);
                    if (dtData == null || dtData.Rows.Count <= 0)
                        throw new Exception("Not Found Data");

                    dtData.TableName = "CHART";
                    ds.Tables.Add(dtData.Copy());

                    dtData = oHis.GetTrendReportInspectRaw(strEquip, sFilter.ToString(), strStartTime, strEndTime);
                    if (dtData == null || dtData.Rows.Count <= 0)
                        throw new Exception("Not Found Data");

                    dtData.TableName = "RAW";
                    ds.Tables.Add(dtData.Copy());

                    ds.AcceptChanges();
                }


                return ds;
            }
            finally
            {
                if (sGroup != null)
                    sGroup.Clear();

                if (sFilter != null)
                    sFilter.Clear();

                if (sGroupBy != null)
                    sGroupBy.Clear();

                if (sOrderBy != null)
                    sOrderBy.Clear();

                sGroup = null;
                sFilter = null;
                sGroupBy = null;
                sOrderBy = null;

                if (dtData != null)
                    dtData.Dispose();
                dtData = null;

                if (ds != null)
                    ds.Dispose();
                ds = null;
            }
        }

        private void ChartDataNormalized(
            DataTable dtData, 
            string TotalField
            )
        {
            double data = double.NaN;
            double reviewSum = double.NaN;
            double total = double.NaN;
            foreach (DataRow row in dtData.Rows)
            {
                if (!double.TryParse(row["DATA"].ToString(), out data))
                    data = double.NaN;

                if (!double.TryParse(row["REVIEW_SUM"].ToString(), out reviewSum))
                    reviewSum = double.NaN;

                if (!double.TryParse(row[TotalField].ToString(), out total))
                    total = double.NaN;

                row["DATA"] = total * (data / reviewSum);
            }
            dtData.AcceptChanges();
        }

        private void RawDataNormalized(
            DataTable dtData,
            string totalField
            )
        {
            double data = double.NaN;
            double reviewSum = double.NaN;
            double total = double.NaN;
            int stIndex = dtData.Columns["REVIEW_SUM"].Ordinal;
            foreach (DataRow row in dtData.Rows)
            {
                if (!double.TryParse(row["REVIEW_SUM"].ToString(), out reviewSum))
                    reviewSum = double.NaN;

                if (!double.TryParse(row[totalField].ToString(), out total))
                    total = double.NaN;

                for (int colIndex = stIndex + 1; colIndex < dtData.Columns.Count; colIndex++)
                {
                    if (!double.TryParse(row[dtData.Columns[colIndex].ColumnName].ToString(), out data))
                        data = 0;

                    row[dtData.Columns[colIndex].ColumnName] = total * (data / reviewSum);
                }
            }
            dtData.AcceptChanges();
        }

        private void RawDataTableColumnsChanged(
            DataTable dtData, 
            DataTable dtDefectType
            )
        {
            if (dtData == null || dtData.Rows.Count <= 0)
                return;

            if (dtDefectType == null || dtDefectType.Rows.Count <= 0)
                return;

            // dtData 에 생성되어 있는 이름 Defect Class에 대한 이름 변경
            int stIndex = dtData.Columns.IndexOf("REVIEW_SUM");
            for (int colIdx = stIndex + 1; colIdx < dtData.Columns.Count; colIdx++)
            {
                DataColumn col = dtData.Columns[colIdx];
                string classNumber = col.ColumnName.Substring(3);
                DataRow[] rows = dtDefectType.Select(String.Format("[CLASSNUMBER] = '{0}'", classNumber));
                if (rows == null || rows.Length <= 0)
                    continue;

                col.ColumnName = rows[0]["NAME"].ToString();
            }
        }

        #endregion [ Defect Eq Trend ]


        #region [ Yield Prospect ]

        public DataTable GetLossProspectFilterDevice(string strStartTime, string strEndTime)
        {
            TQC_LOSS_YIELD_SUM oLoss = new TQC_LOSS_YIELD_SUM();
            return oLoss.GetLossProspectFilterDevice(strStartTime, strEndTime);
        }

        public DataTable GetLossProspectFilter(string strStartTime, string strEndTime, string strTestArea, string strDevice)
        {
            TQC_LOSS_YIELD_SUM oLoss = new TQC_LOSS_YIELD_SUM();
            return oLoss.GetLossProspectFilter(strStartTime, strEndTime, strTestArea, strDevice);
        }

        public DataSet GetProspectYieldReport(string strStartTime, string strEndTime, string strTestArea, string strDevice, string[] strProduct, string[] strLotID, string[] strWaferID, string[] strStep)
        {
            DataSet ds = null;
            DataTable dtData = null;

            TQC_LOSS_YIELD_SUM oLoss = new TQC_LOSS_YIELD_SUM();

            System.Text.StringBuilder sFilter = new System.Text.StringBuilder();

            try
            {
                ds = new DataSet();
                if (strProduct != null && strProduct.Length > 0)
                {
                    sFilter.AppendLine(string.Format(" AND PRODUCT IN ('{0}')", string.Join("','", strProduct)));
                }

                if (strLotID != null && strLotID.Length > 0)
                {
                    sFilter.AppendLine(string.Format(" AND LOT_ID IN ('{0}')", string.Join("','", strLotID)));
                }

                if (strWaferID != null && strWaferID.Length > 0)
                {
                    sFilter.AppendLine(string.Format(" AND WAFER_ID IN ('{0}')", string.Join("','", strWaferID)));
                }

                if (strStep != null && strStep.Length > 0)
                {
                    sFilter.AppendLine(string.Format(" AND STEP_ID IN ('{0}')", string.Join("','", strStep)));
                }

                dtData = oLoss.GetProtectLossMainData(strStartTime, strEndTime, strTestArea, strDevice, sFilter.ToString());
                if (dtData == null || dtData.Rows.Count <= 0)
                    throw new Exception("Not Found Data");

                dtData.TableName = "MAIN";
                ds.Tables.Add(dtData.Copy());

                dtData = oLoss.GetProtectLossSubData(strTestArea, strDevice);
                if (dtData != null && dtData.Rows.Count > 0)
                {
                    dtData.TableName = "SUB";
                    ds.Tables.Add(dtData.Copy());
                }

                ds.AcceptChanges();
                return ds;
            }
            finally
            {
                if (sFilter != null)
                    sFilter.Clear();

                sFilter = null;

                if (dtData != null)
                    dtData.Dispose();
                dtData = null;

                if (ds != null)
                    ds.Dispose();
                ds = null;
            }
        }


        #endregion [ Yield Prospect ]
    }
}
