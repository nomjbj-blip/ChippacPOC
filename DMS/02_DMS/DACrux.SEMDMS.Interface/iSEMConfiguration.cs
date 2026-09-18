using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Drawing;

namespace DACrux.SEMDMS.Interface
{
    public interface iSEMConfiguration
    {
        //TQD_COLORBYSIZE
        DataTable SelectColorListByDftSize(string userId);
        void InsertColorByDftSize(string user, int[] sizeSeq, int[] sizeFrom, int[] sizeTo, System.Drawing.Color[] color);
        void DeleteColorByDftSize(string userId);
        bool DeleteDefectSizeByColor(string userid, string sizeSeq);
        bool ExistsDefectSizeByColor(string userid, string sizeSeq);
        bool InsertDefectSizeByColor(string userid, string sizeSeq, string sizeFrom, string sizeTo, string color);
        bool UpdateDefectSizeByColor(string userid, string sizeSeq, string sizeFrom, string sizeTo, string color);

        //TQD_DEFECT_TYPE
        DataTable GetDefectTypeList(string DefectGroupId = "ALL");
        bool ExistsDefectType(string classnumber);
        bool DeleteDefectType(string classnumber);
        bool InsertDefectType(Dictionary<string, string> dicValues, string cid, string uid);
        bool UpdateDefectType(Dictionary<string, string> dicValues, string cid, string uid);

        //TQD_PRODUCT
        DataTable GetInfo(string Device);
        DataTable GetProduct();

        //TQD_DEFECT_GROUP
        DataTable SelectGroupList();
        DataTable GetGroup(string groupId);
        bool ExistsDefectGroup(string groupId);
        int InsertDefectGroup(Dictionary<string, string> dicValues, string userid);
        int UpdateDefectGroup(Dictionary<string, string> dicValues, string userid);
        int DeleteDefectGroup(string groupId);

        //TQD_ZONE
        DataTable GetZone(string userId, string product);

        // TQD_STEP
        DataTable GetProduct(string fromDate, string toDate);
        DataTable GetConditionStep(string fromDate, string toDate, string product);
        DataTable GetConditionStep(string fromDate, string toDate, string[] products);

        DataTable GetConditionLot(string fromDate, string toDate, string product, string stepid);
        DataTable GetConditionLot(string fromDate, string toDate, string product, string[] stepids);
        DataTable GetConditionLot(string fromDate, string toDate, string[] products, string[] stepids);

        DataTable GetConditionWafer(string fromDate, string toDate, string product, string stepid, string[] lots);
        DataTable GetConditionWafer(string fromDate, string toDate, string product, string[] stepids, string[] lots);
        DataTable GetConditionWafer(string fromDate, string toDate, string[] products, string[] stepids, string[] lots, bool reviewOnly);

        DataTable GetLotInfo(string fromDate, string toDate, string lotid);


        #region [ Defect Eq Trend ]

        DataTable GetClassList(bool bClass);

        DataTable GetItemList(string strItemName, string strFilter, string[] strEquip, string[] strRoute, string[] LotID, string[] strStep, string strStartTime, string strEndTime);
        DataSet GetTrendReportReview(bool bClassMode, bool bDefectMode, bool bNormalized, string strModeName, string strParaName, string[] strClass, string strEquip, string[] strRoute, string[] strLotID, string[] strStep, string strStartTime, string strEndTime);

        #endregion [ Defect Eq Trend ]

        #region [ Yield Prospect ]

        DataTable GetLossProspectFilterDevice(string strStartTime, string strEndTime);
        DataTable GetLossProspectFilter(string strStartTime, string strEndTime, string strTestArea, string strDevice);
        DataSet GetProspectYieldReport(string strStartTime, string strEndTime, string strTestArea, string strDevice, string[] strProduct, string[] strLotID, string[] strWaferID, string[] strStep);

        #endregion [ Yield Prospect ]

        Dictionary<int, string> GetDefectTypeAll();
    }
}
