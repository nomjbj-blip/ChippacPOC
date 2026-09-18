using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.Base;
using System.Drawing;

namespace DACrux.SEMDMS.RO
{
    public class SEMConfiguration
    {
        DACrux.SEMDMS.Interface.iSEMConfiguration m_OBJ;

        public SEMConfiguration()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iSEMConfiguration),
                        strUrl + "/DACrux.SEMDMS.BSL.SEMConfiguration.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iSEMConfiguration;

        }

        public DataTable GetInfo(
            string Device
            )
        {
            try
            {
                return m_OBJ.GetInfo(Device);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [ TQD_DEFECT_GROUP ]
        //TQD_DEFECT_GROUP     SELECT
        public DataTable SelectGroupList(
            )
        {
            return m_OBJ.SelectGroupList();
        }

        public DataTable GetGroup(string groupId)
        {
            return m_OBJ.GetGroup(groupId);
        }

        //TQD_DEFECT_GROUP     GET
        public bool ExistsDefectGroup(
            string groupId
            )
        {
            return m_OBJ.ExistsDefectGroup(groupId);
        }

        public int InsertDefectGroup(
            Dictionary<string, string> dictionary,
            string userid
            )
        {
            return m_OBJ.InsertDefectGroup(dictionary, userid);
        }

        public int UpdateDefectGroup(
            Dictionary<string, string> dictionary,
            string userid
            )
        {
            return m_OBJ.UpdateDefectGroup(dictionary, userid);
        }

        public int DeleteDefectGroup(
            string groupId
            )
        {
            return m_OBJ.DeleteDefectGroup(groupId);
        }

        #endregion [TQD_DEFECT_GROUP]

        //TQD_ZONE    SELECT
        public DataTable GetZone(string userId, string product)
        {
            try
            {
                return m_OBJ.GetZone(userId, product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //TQD_PRODUCT SELECT
        public DataTable GetProduct()
        {
            try
            {
                return m_OBJ.GetProduct();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [ TQD_DEFECT_TYPE ]

        //--

        public Dictionary<int, String> GetDefectTypeAll()
        {
            return m_OBJ.GetDefectTypeAll();
        }

        public DataTable GetDefectTypeList(
            string DefectGroupId = "ALL"
            )

        {
            return m_OBJ.GetDefectTypeList(DefectGroupId);
        }

        //--

        public bool InsertDefectByType(
            Dictionary<string, string> dicValues,
            string cid,
            string uid
            )
        {
            return m_OBJ.InsertDefectType(
                dicValues,
                cid,
                uid
                );
        }

        //--

        public bool DeleteDefectByType(
            string classnumber
            )
        {
            return m_OBJ.DeleteDefectType(
                classnumber
                );
        }

        //--

        public bool ExistsDefectType(
            string classnumber
            )
        {
            return m_OBJ.ExistsDefectType(
                classnumber
                );
        }

        //--

        public bool UpdateDefectByType(
            Dictionary<string, string> dicValues,
            string cid,
            string uid
            )
        {
            return m_OBJ.UpdateDefectType(
                dicValues,
                cid,
                uid
                );
        }

        #endregion [ TQD_DEFECT_TYPE ]

        #region [ TQD_DEFECT_SIZE ]
        //
        //
        //TQD_COLORBYSIZE     SELECT
        public DataTable SelectColorListByDftSize(string userId)
        {
            try
            {
                return m_OBJ.SelectColorListByDftSize(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //TQD_COLORBYSIZE     CREATE
        public void CreateColorByDftSize(string user, int[] sizeSeq, int[] sizeFrom, int[] sizeTo, System.Drawing.Color[] color)
        {
            try
            {
                m_OBJ.InsertColorByDftSize(user, sizeSeq, sizeFrom, sizeTo, color);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //TQD_COLORBYSIZE     DELETE
        public void DeleteColorByDftSize(string userId)
        {
            try
            {
                m_OBJ.DeleteColorByDftSize(
                    userId
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExistsDefectSizeByColor(
            string userid,
            string sizeSeq
            )
        {
            return m_OBJ.ExistsDefectSizeByColor(
                userid,
                sizeSeq
                );
        }

        public bool InsertDefectSizeByColor(
            string userid,
            string sizeSeq,
            string sizeFrom,
            string sizeTo,
            string color
            )
        {
            return m_OBJ.InsertDefectSizeByColor(
                userid,
                sizeSeq,
                sizeFrom,
                sizeTo,
                color
                );
        }

        public bool UpdateDefectSizeByColor(
            string userid,
            string sizeSeq,
            string sizeFrom,
            string sizeTo,
            string color
            )
        {
            return m_OBJ.UpdateDefectSizeByColor(
                userid,
                sizeSeq,
                sizeFrom,
                sizeTo,
                color
                );
        }

        public bool DeleteDefectSizeByColor(
            string userid,
            string sizeSeq
            )
        {
            return m_OBJ.DeleteDefectSizeByColor(
                userid,
                sizeSeq
                );
        }

        #endregion [TQD_DEFECT_SIZE]


        #region [ TQD_STEP ]
        public DataTable GetProduct(string fromDate, string toDate)
        {
            return m_OBJ.GetProduct(fromDate, toDate);
        }

        public DataTable GetConditionStep(string fromDate, string toDate, string product)
        {
            return m_OBJ.GetConditionStep(fromDate, toDate, product);
        }

        public DataTable GetConditionStep(string fromDate, string toDate, string[] products)
        {
            return m_OBJ.GetConditionStep(fromDate, toDate, products);
        }

        public DataTable GetConditionLot(string fromDate, string toDate, string product, string stepid)
        {
            return m_OBJ.GetConditionLot(fromDate, toDate, product, stepid);
        }

        public DataTable GetConditionLot(string fromDate, string toDate, string product, string[] stepids)
        {
            return m_OBJ.GetConditionLot(fromDate, toDate, product, stepids);
        }

        public DataTable GetConditionLot(string fromDate, string toDate, string[] products, string[] stepids)
        {
            return m_OBJ.GetConditionLot(fromDate, toDate, products, stepids);
        }

        public DataTable GetConditionWafer(string fromDate, string toDate, string product, string stepid, string[] lots)
        {
            return m_OBJ.GetConditionWafer(fromDate, toDate, product, stepid, lots);
        }

        public DataTable GetConditionWafer(string fromDate, string toDate, string product, string[] stepids, string[] lots)
        {
            return m_OBJ.GetConditionWafer(fromDate, toDate, product, stepids, lots);
        }

        public DataTable GetConditionWafer(string fromDate, string toDate, string[] products, string[] stepids, string[] lots, bool reviewOnly = false)
        {
            return m_OBJ.GetConditionWafer(fromDate, toDate, products, stepids, lots, reviewOnly);
        }

        #endregion  [ TQD_STEP ]

        public DataTable GetLotInfo(string fromDate, string toDate, string lotid)
        {
            return m_OBJ.GetLotInfo(fromDate, toDate, lotid);
        }

        #region [ Defect Eq Trend ]

        public DataTable GetClassList(bool bClass)
        {
            return m_OBJ.GetClassList(bClass);
        }

        public DataTable GetItemList(string strItemName, string strFilter, string[] strEquip, string[] strRoute, string[] LotID, string[] strStep, string strStartTime, string strEndTime)
        {
            return m_OBJ.GetItemList(strItemName, strFilter, strEquip, strRoute, LotID, strStep, strStartTime, strEndTime);
        }

        public DataSet GetTrendReportReview(bool bClassMode, bool bDefectMode, bool bNormalized, string strModeName, string strParaName, string[] strClass, string strEquip, string[] strRoute, string[] strLotID, string[] strStep, string strStartTime, string strEndTime)
        {
            return m_OBJ.GetTrendReportReview(bClassMode, bDefectMode, bNormalized, strModeName, strParaName, strClass, strEquip, strRoute, strLotID, strStep, strStartTime, strEndTime);
        }

        #endregion [ Defect Eq Trend ]

        #region [ Yield Prospect ]

        public DataTable GetLossProspectFilterDevice(string strStartTime, string strEndTime)
        {
            return m_OBJ.GetLossProspectFilterDevice(strStartTime, strEndTime);
        }

        public DataTable GetLossProspectFilter(string strStartTime, string strEndTime, string strTestArea, string strDevice)
        {
            return m_OBJ.GetLossProspectFilter(strStartTime, strEndTime, strTestArea, strDevice);
        }

        public DataSet GetProspectYieldReport(string strStartTime, string strEndTime, string strTestArea, string strDevice, string[] strProduct, string[] strLotID, string[] strWaferID, string[] strStep)
        {
            return m_OBJ.GetProspectYieldReport(strStartTime, strEndTime, strTestArea, strDevice, strProduct, strLotID, strWaferID, strStep);
        }

        #endregion [ Yield Prospect ]
    }
}
