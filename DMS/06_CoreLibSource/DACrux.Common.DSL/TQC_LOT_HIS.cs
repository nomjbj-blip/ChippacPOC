using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Common.DSL
{
    public class TQC_LOT_HIS : Miracom.Middleware.QueryComponent
    {
        #region 생성자

        public TQC_LOT_HIS()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];

            if (connectID.Equals(string.Empty))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "TQC_LOT_HIS.xml");
        }

        #endregion


        public DataTable GetLotHisData(string strStartTime, string strEndTime)
        {
            return GetDataTable("SELECT_LOT_HIS_TIME", null, new string[] { strStartTime, strEndTime });
        }

        public DataTable GetLotHisDataGroup(string strItem, string strStartTime, string strEndTime)
        {
            return GetDataTable("SELECT_LOT_HIS_GROUP", new string[] { strItem, strItem }, new string[] { strStartTime, strEndTime });
        }

        public DataTable GetCommonalityLot(string strGoodLot, string strBadLot, string strTestArea)
        {
            return GetDataTable("SELECT_LOT_HIS_LOT", new string[] { strGoodLot, strTestArea, strBadLot, strTestArea }, null);
        }

        public DataTable GetCommonalityLotGrp(string strGoodLot, string strBadLot, string strGrp, string strTestArea)
        {
            return GetDataTable("SELECT_LOT_HIS_LOT_GRP", new string[] { strGrp, strGoodLot, strTestArea, strBadLot, strTestArea, strGrp, strGrp }, null);
        }

        public DataTable GetCommonalityWafer(string goodLot, string goodWafer, string badLot, string badWafer, string strTestArea)
        {
            return GetDataTable("SELECT_LOT_HIS_WAFER", new string[] { goodLot, goodWafer, strTestArea, badLot, badWafer, strTestArea }, null);
        }

        public DataTable GetCommonalityWaferGrp(string goodLot, string goodWafer, string badLot, string badWafer, string strGrp, string strTestArea)
        {
            return GetDataTable("SELECT_LOT_HIS_WAFER_GRP", new string[] { strGrp, goodLot, goodWafer, strTestArea, badLot, badWafer, strTestArea, strGrp, strGrp }, null);
        }

    }
}
