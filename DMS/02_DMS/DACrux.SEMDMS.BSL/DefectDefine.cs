using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DACrux.SEMDMS.DSL;
using System.Drawing;

namespace DACrux.SEMDMS.BSL
{
    public class DefectDefine : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iDefectDefine
    {
        public DefectDefine()
        {
        }

        #region [TQD_SETUP]

        public long GetSetupDuplicate(string SetupID, string strStepID, string strSetupTime)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP oTQD_SETUP = null;
            DataTable dt = null;
            try
            {
                oTQD_SETUP = new DACrux.SEMDMS.DSL.TQD_SETUP();
                dt = oTQD_SETUP.GetData("SELECT_SETUP_SEQ", null, new string[] { SetupID, strStepID, strSetupTime });
                if (dt.Rows.Count == 0) return -1;
                return DACrux.Base.Convert.longParse(dt.Rows[0][0].ToString());
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        public long GetSetupInfo(string SetupID)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP oTQD_SETUP = null;
            DataTable dt = null;
            try
            {
                oTQD_SETUP = new DACrux.SEMDMS.DSL.TQD_SETUP();
                dt = oTQD_SETUP.GetData("SELECT_SETUP_SEQ_2", null, new string[] { SetupID });
                if (dt.Rows.Count == 0) return -1;
                return DACrux.Base.Convert.longParse(dt.Rows[0][0].ToString());
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

        }

        public long GetSetupInfo(string SetupID, string strX, string strY)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP oTQD_SETUP = null;
            DataTable dt = null;
            try
            {
                oTQD_SETUP = new DACrux.SEMDMS.DSL.TQD_SETUP();
                dt = oTQD_SETUP.GetData("FAB1_SETUP_SUPLE", null, new string[] { SetupID, strX, strY });
                if (dt.Rows.Count == 0) return -1;
                return DACrux.Base.Convert.longParse(dt.Rows[0][0].ToString());
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

        }

        public void CreateSetupInfo(string[] SetupInfo)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP oTQD_SETUP = null;
            oTQD_SETUP = new DACrux.SEMDMS.DSL.TQD_SETUP();
            oTQD_SETUP.InsertData("CREATE_SETUP", null, SetupInfo);
        }

        public void CreateSetupData(string[] SetupInfo)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP oTQD_SETUP = null;
            oTQD_SETUP = new DACrux.SEMDMS.DSL.TQD_SETUP();
            oTQD_SETUP.InsertData("CREATE_SETUP_INFO", null, SetupInfo);
        }

        public void UpdateSetupData(string strSetupSeq, string strStreetX, string strStreetY)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP oTQD_SETUP = null;
            oTQD_SETUP = new DACrux.SEMDMS.DSL.TQD_SETUP();
            oTQD_SETUP.UpdateData("UPDATE_SETUP_STREET", null, new string[] { strStreetX, strStreetY, strSetupSeq });
        }

        #endregion

        #region [TQD_LOT]


        public long GetLotSeq(string[] LotInfo)
        {
            DACrux.SEMDMS.DSL.TQD_LOT oTQD_LOT = null;
            DataTable dt = null;
            try
            {
                oTQD_LOT = new DACrux.SEMDMS.DSL.TQD_LOT();
                dt = oTQD_LOT.GetData("SELECT_LOT_SEQ", null, new string[] { LotInfo[1], LotInfo[0] });
                if (dt.Rows.Count == 0) return -1;
                return DACrux.Base.Convert.longParse(dt.Rows[0]["LOT_SEQ"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        public void CreateLotInfo(string[] LotInfo)
        {
            DACrux.SEMDMS.DSL.TQD_LOT oTQD_LOT = null;
            try
            {
                oTQD_LOT = new DACrux.SEMDMS.DSL.TQD_LOT();
                oTQD_LOT.InsertData("CREATE_LOT", null, LotInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_WAFER]

        public long GetWaferSeq(string[] WaferInfo)
        {
            DACrux.SEMDMS.DSL.TQD_WAFER oTQD_WAFER = null;
            DataTable dt = null;
            try
            {
                oTQD_WAFER = new DACrux.SEMDMS.DSL.TQD_WAFER();
                dt = oTQD_WAFER.GetData("SELECT_WAFER_SEQ", null, new string[] { WaferInfo[0], WaferInfo[1] });
                if (dt.Rows.Count == 0) return -1;
                return DACrux.Base.Convert.longParse(dt.Rows[0]["WAFER_SEQ"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        public void CreateWaferInfo(string[] WaferInfo)
        {
            DACrux.SEMDMS.DSL.TQD_WAFER oTQD_WAFER = null;
            try
            {
                oTQD_WAFER = new DACrux.SEMDMS.DSL.TQD_WAFER();
                oTQD_WAFER.InsertData("CREATE_WAFER", null, WaferInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_STEP]

        /// <summary>
        /// FAB1 의 Migration 용으로 사용.
        /// </summary>
        /// <param name="StepInfo"></param>
        /// <returns></returns>
        public long GetStepSeq(string[] StepInfo)
        {
            DACrux.SEMDMS.DSL.TQD_STEP oTQD_STEP = null;
            DataTable dt = null;
            try
            {
                oTQD_STEP = new DACrux.SEMDMS.DSL.TQD_STEP();
                dt = oTQD_STEP.GetData("SELECT_STEP_SEQ", null, StepInfo); //StepInfo[1],StepInfo[5],StepInfo[2],StepInfo[8]
                if (dt.Rows.Count == 0) return -1;
                return DACrux.Base.Convert.longParse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }


        public long GetStepInfo(string[] StepInfo)
        {
            DACrux.SEMDMS.DSL.TQD_STEP oTQD_STEP = null;
            DataTable dt = null;
            try
            {
                oTQD_STEP = new DACrux.SEMDMS.DSL.TQD_STEP();
                dt = oTQD_STEP.GetData("SELECT_STEP_INFO", null, StepInfo); //StepInfo[1],StepInfo[5],StepInfo[2],StepInfo[8]
                if (dt.Rows.Count == 0) return -1;
                return DACrux.Base.Convert.longParse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        /// <summary>
        ///  Step 상의 TestNo 값을 +1 해준다.
        /// </summary>
        /// <param name="strWaferSeq"></param>
        /// <param name="strStepID"></param>
        public void UpdateStepTestNo(string strWaferSeq, string strStepID)
        {
            DACrux.SEMDMS.DSL.TQD_STEP oTQD_STEP = null;
            oTQD_STEP = new DACrux.SEMDMS.DSL.TQD_STEP();
            oTQD_STEP.UpdateStepTestNo(strWaferSeq, strStepID);
        }

        /// <summary>
        /// FAB1 의 Migration 용으로 사용.
        /// Migration의 경우 TEST_ORDER 를 0으로 저장 한다.
        /// </summary>
        /// <param name="StepInfo"></param>
        public void CreateStepInfo(string[] StepInfo)
        {
            DACrux.SEMDMS.DSL.TQD_STEP oTQD_STEP = null;
            try
            {
                oTQD_STEP = new DACrux.SEMDMS.DSL.TQD_STEP();
                oTQD_STEP.InsertData("CREATE_STEP", null, StepInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateStepSeq(string[] StepInfo)
        {
            DACrux.SEMDMS.DSL.TQD_STEP oTQD_STEP = null;
            try
            {
                oTQD_STEP = new DACrux.SEMDMS.DSL.TQD_STEP();
                oTQD_STEP.InsertData("CREATE_STEP_INFO", null, StepInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateInspSum(string stepSeq)
        {
            TQD_INSP_SUM obj = new TQD_INSP_SUM();
            obj.InsertData(stepSeq);
        }

        public DataTable GetMigrationData(string strStarttime, string strEndTime)
        {
            DACrux.SEMDMS.DSL.TQD_STEP oTQD_STEP = null;
            try
            {
                oTQD_STEP = new DACrux.SEMDMS.DSL.TQD_STEP();
                return oTQD_STEP.GetMigrationData(strStarttime, strEndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 저장된 Map의 ORGIN 인덱스 정보를 가져옵니다.
        /// </summary>
        public Point GetOriginDieIndex(string setupID, string stepID, DateTime setupTime)
        {
            TQD_SETUP obj = new TQD_SETUP();
            DataTable dt = obj.GetOriginDieIndex(setupID, stepID, setupTime);

            if (dt == null || dt.Rows.Count == 0)
                return Point.Empty;

            return new Point(Int32.Parse(dt.Rows[0]["DIE_ORIGIN_X"].ToString()), Int32.Parse(dt.Rows[0]["DIE_ORIGIN_Y"].ToString()));
        }

        #endregion

        #region [TQD_INSP_INFO]

        /// <summary>
        /// FAB1 의 Migration 용으로 사용.
        /// </summary>
        /// <param name="InspInfo"></param>
        public void CreateInspInfo(string[] InspInfo)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oTQD_INSP_INFO = null;
            try
            {
                oTQD_INSP_INFO = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oTQD_INSP_INFO.InsertData("CREATE_INSP_INFO_MIG", null, InspInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateInspData(string[] InspInfo)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oTQD_INSP_INFO = null;
            try
            {
                oTQD_INSP_INFO = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oTQD_INSP_INFO.InsertData("CREATE_INSP_INFO_PARSE", null, InspInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetInspInfo(string strStepSeq)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oTQD_INSP_INFO = null;
            try
            {
                oTQD_INSP_INFO = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                return oTQD_INSP_INFO.GetData("SELECT_TQD_INSP_INFO_DUPL", null, new string[] { strStepSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetInspInfoDuple(string strStepSeq, string strTestNo)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oTQD_INSP_INFO = null;
            try
            {
                oTQD_INSP_INFO = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                return oTQD_INSP_INFO.GetData("SELECT_TQD_INSP_INFO_TEST_NO", null, new string[] { strStepSeq, strTestNo });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #endregion

        #region [TQD_DEFECT]

        public int GetDefectCount(string strWaferSeq, string strStep)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oTQD_DEFECT = null;
            DataTable dt = null;
            int idefectcount = 0;
            try
            {
                oTQD_DEFECT = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                dt = oTQD_DEFECT.GetData("SELECT_DEFECT_COUNT", null, new string[2] { strWaferSeq, strStep });
                idefectcount = DACrux.Base.Convert.intParse(dt.Rows[0][0].ToString());
                return idefectcount;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        public DataTable GetDefectDataCount(string strWaferSeq, string strStep)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oTQD_DEFECT = null;
            try
            {
                oTQD_DEFECT = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                return oTQD_DEFECT.GetData("SELECT_DEFECT_DATA", null, new string[2] { strWaferSeq, strStep });
            }
            finally
            {
            }
        }

        public void CreateDefect(string[,] aParas)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oTQD_DEFECT = new DACrux.SEMDMS.DSL.TQD_DEFECT();
            oTQD_DEFECT.InsertExecuteMultiple("CREATE_DEFECT", aParas);
        }

        /// <summary>
        /// result time 기준 최근의 STEP의 Defect X,Y를 가져옵니다.
        /// </summary>
        public DataTable GetLastestStepDefectXY(string waferID, string stepID, string resultTime)
        {
            TQD_DEFECT obj = new TQD_DEFECT();
            return obj.GetLastestStepDefectXY(waferID, stepID, resultTime);
        }

        public void DeleteDefectInfo(string strWaferSeq, string strStep)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oTQD_DEFECT = new DACrux.SEMDMS.DSL.TQD_DEFECT();
            try
            {
                oTQD_DEFECT.DeleteData("DELETE_DEFECT", null, new string[] { strWaferSeq, strStep });
                return;
            }
            finally
            {

            }
        }

        public void SetMigrationData(string[,] strData)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oTQD_DEFECT = null;
            try
            {
                oTQD_DEFECT = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                oTQD_DEFECT.SetMigrationData(strData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [TQD_IMAGES]

        //public void CreateImages(string[] ImageInfo)
        //{
        //    DACrux.SEMDMS.DSL.TQD_IMAGES oTQD_IMAGES = null;
        //    try
        //    {
        //        oTQD_IMAGES = new DACrux.SEMDMS.DSL.TQD_IMAGES();
        //        oTQD_IMAGES.InsertData("CREATE_IMAGES", null, ImageInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void CreateImagesMulti(string[,] ImageInfo)
        {
            DACrux.SEMDMS.DSL.TQD_IMAGES oTQD_IMAGES = null;
            try
            {
                oTQD_IMAGES = new DACrux.SEMDMS.DSL.TQD_IMAGES();
                oTQD_IMAGES.InsertExecuteMultiple("CREATE_IMAGES", ImageInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteImages(string strStepSeq, string strWaferSeq)
        {
            DACrux.SEMDMS.DSL.TQD_IMAGES oTQD_IMAGES = null;
            try
            {
                oTQD_IMAGES = new DACrux.SEMDMS.DSL.TQD_IMAGES();
                oTQD_IMAGES.DeleteData("DELETE_IMAGES", null, new string[] { strStepSeq, strWaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_PRODUCT]

        public DataTable GetProductInfo(string strProduct)
        {
            DACrux.SEMDMS.DSL.TQD_PRODUCT oTQD_PRODUCT = null;

            oTQD_PRODUCT = new DACrux.SEMDMS.DSL.TQD_PRODUCT();
            return oTQD_PRODUCT.GetData("SELECT_PRODUCT", null, new string[] { strProduct });
        }

        public void CreateProduct(string[] ProductInfo)
        {
            DACrux.SEMDMS.DSL.TQD_PRODUCT oTQD_PRODUCT = null;
            oTQD_PRODUCT = new DACrux.SEMDMS.DSL.TQD_PRODUCT();
            oTQD_PRODUCT.InsertData("CREATE_PRODUCT", null, ProductInfo);
        }

        #endregion

        #region [TQD_SETUP_MAP]

        public int GetSetupMapCount(long setup_seq)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP_MAP oTQD_SETUP_MAP = null;
            DataTable dt = null;
            int isamplecount = 0;
            try
            {
                oTQD_SETUP_MAP = new DACrux.SEMDMS.DSL.TQD_SETUP_MAP();
                dt = oTQD_SETUP_MAP.GetData("SELECT_MAP_COUNT", null, new string[] { setup_seq.ToString() });
                isamplecount = DACrux.Base.Convert.intParse(dt.Rows[0][0].ToString());
                return isamplecount;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        public DataTable GetSetupTestData(string strsetup_seq, string strTEST)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP_MAP oTQD_SETUP_MAP = null;
            try
            {
                oTQD_SETUP_MAP = new DACrux.SEMDMS.DSL.TQD_SETUP_MAP();
                return oTQD_SETUP_MAP.GetData("SELECT_MAP_TEST_DATA", null, new string[] { strsetup_seq, strTEST });
            }
            finally
            {
            }
        }

        public void CreateSetupMap(string[,] aParas)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP_MAP oTQD_SETUP_MAP = null;
            oTQD_SETUP_MAP = new DACrux.SEMDMS.DSL.TQD_SETUP_MAP();
            oTQD_SETUP_MAP.InsertExecuteMultiple("CREATE_TQD_SETUP_MAP", aParas);
        }

        #endregion

        /// <summary>
        /// IMAGECOUNT 값을 일괄 업데이트 합니다.
        /// </summary>
        public void UpdateImageCount(long StepSeq)
        {
            TQD_DEFECT obj = new TQD_DEFECT();
            obj.UpdateImageCount(StepSeq);
        }

        public long[] GetStepSeqBy(string lotID, string equipID, string resultTimestamp)
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            DataTable dt = obj.GetStepSeqBy(lotID, equipID, resultTimestamp);

            long[] stepSeqArr = new long[dt.Rows.Count];

            for (int i = 0; i < stepSeqArr.Length; i++)
                stepSeqArr[i] = Int64.Parse(dt.Rows[i][0].ToString());

            return stepSeqArr;
        }
    }
}
