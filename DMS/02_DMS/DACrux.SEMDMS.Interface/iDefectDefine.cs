using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    public interface iDefectDefine
    {
        #region [TQD_SETUP]

        long GetSetupInfo(string SetupID);
        long GetSetupInfo(string SetupID, string strX, string strY);
        void CreateSetupInfo(string[] SetupInfo);
        void UpdateSetupData(string strSetupSeq, string strStreetX, string strStreetY);

        #endregion

        #region [TQD_LOT]

        long GetLotSeq(string[] LotInfo);
        void CreateLotInfo(string[] LotInfo);

        #endregion

        #region [TQD_WAFER]

        long GetWaferSeq(string[] WaferInfo);
        void CreateWaferInfo(string[] WaferInfo);

        #endregion

        #region [TQD_STEP]

        long GetStepSeq(string[] StepInfo);
        void CreateStepInfo(string[] StepInfo);
        DataTable GetMigrationData(string strStarttime, string strEndTime);

        #endregion

        #region [TQD_INSP_INFO]

        void CreateInspInfo(string[] InspInfo);
        DataTable GetInspInfo(string strStepSeq);

        #endregion

        #region [TQD_DEFECT]

        int GetDefectCount(string strWaferSeq, string strStep);
        void CreateDefect(string[,] aParas);
        void DeleteDefectInfo(string strWaferSeq, string strStep);
        void SetMigrationData(string[,] strData);

        #endregion

        #region [TQD_IMAGES]

        //void CreateImages(string[] ImageInfo);
        void CreateImagesMulti(string[,] ImageInfo);
        void DeleteImages(string strStepSeq, string strWaferSeq);

        #endregion

        #region [TQD_PRODUCT]

        DataTable GetProductInfo(string strProduct);
        void CreateProduct(string[] ProductInfo);

        #endregion

        #region [TQD_SETUP_MAP]

        int GetSetupMapCount(long setup_seq);
        void CreateSetupMap(string[,] aParas);

        #endregion

    }
}
