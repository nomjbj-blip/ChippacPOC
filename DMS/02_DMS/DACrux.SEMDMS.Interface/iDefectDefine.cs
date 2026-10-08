using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    public interface iDefectDefine
    {
        #region [TQD_SETUP]

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
        DataTable GetMigrationData(string strStarttime, string strEndTime);

        #endregion

        #region [TQD_INSP_INFO]

        #endregion

        #region [TQD_DEFECT]

        int GetDefectCount(string strWaferSeq, string strStep);
        void CreateDefect(string[,] aParas);
        void SetMigrationData(string[,] strData);

        #endregion

        #region [TQD_IMAGES]

        //void CreateImages(string[] ImageInfo);

        #endregion

        #region [TQD_PRODUCT]

        DataTable GetProductInfo(string strProduct);
        void CreateProduct(string[] ProductInfo);

        #endregion

        #region [TQD_SETUP_MAP]

        void CreateSetupMap(string[,] aParas);

        #endregion

    }
}
