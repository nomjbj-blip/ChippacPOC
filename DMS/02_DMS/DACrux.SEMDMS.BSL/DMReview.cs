using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DACrux.Base;

namespace DACrux.SEMDMS.BSL
{
    public class DMReview
    {
        public DMReview() { }

        public bool GetInspectionInfo(
            string factory,
            DateTime resulttimestamp,
            string product,
            string lot,
            string wafer,
            string step,
            out long waferseq,
            out long stepseq
            )
        {
            DSL.TQD_INSP_INFO objInspInfo = new DSL.TQD_INSP_INFO();
            DataTable dt = objInspInfo.GetStepInfoByStepSeqAndWaferSeq(
                factory,
                resulttimestamp,
                product,
                lot,
                wafer,
                step
                );

            if (dt == null || dt.Rows.Count <= 0)
            {
                waferseq = long.MinValue;
                stepseq = long.MinValue;
                return false;
            }

            waferseq = Int64.Parse(dt.Rows[0]["WAFER_SEQ"].ToString());
            stepseq = Int64.Parse(dt.Rows[0]["STEP_SEQ"].ToString());
            return true;
        }

        public bool GetInspectionInfo_01(
            string factory,
            DateTime resulttimestamp,
            string lot,
            string slot,
            string step,
            out long waferseq,
            out long stepseq
            )
        {
            DSL.TQD_INSP_INFO objInspInfo = new DSL.TQD_INSP_INFO();
            DataTable dt = objInspInfo.GetStepInfoByStepSeqAndWaferSeq_01(
                factory,
                resulttimestamp,
                lot,
                slot,
                step
                );

            if (dt == null || dt.Rows.Count <= 0)
            {
                waferseq = long.MinValue;
                stepseq = long.MinValue;
                return false;
            }

            waferseq = Int64.Parse(dt.Rows[0]["WAFER_SEQ"].ToString());
            stepseq = Int64.Parse(dt.Rows[0]["STEP_SEQ"].ToString());
            return true;
        }

        public bool GetInspectionInfo_02(
            string factory,
            DateTime resulttimestamp,
            string lot,
            string slot,
            string step,
            out long waferseq,
            out long stepseq
            )
        {
            DSL.TQD_INSP_INFO objInspInfo = new DSL.TQD_INSP_INFO();
            DataTable dt = objInspInfo.GetStepInfoByStepSeqAndWaferSeq_02(
                factory,
                resulttimestamp,
                lot,
                slot,
                step
                );

            if (dt == null || dt.Rows.Count <= 0)
            {
                waferseq = long.MinValue;
                stepseq = long.MinValue;
                return false;
            }

            waferseq = Int64.Parse(dt.Rows[0]["WAFER_SEQ"].ToString());
            stepseq = Int64.Parse(dt.Rows[0]["STEP_SEQ"].ToString());
            return true;
        }

        //--

        public int UpdateDefectList(string[,] defectParams)
        {
            DSL.TQD_DEFECT oDefect = new DSL.TQD_DEFECT();
            return oDefect.UpdateDefect(
                defectParams
                );
        }

        //--

        public int InsertDefectImage(string[,] imageParams)
        {
            DSL.TQD_IMAGES oImages = new DSL.TQD_IMAGES();
            return oImages.CreateImages(
                imageParams
                );
        }

        //--

        public int DeleteDefectImage(
            long stepseq, 
            long waferseq
            )
        {
            DSL.TQD_IMAGES oImage = new DSL.TQD_IMAGES();
            return oImage.DeleteDefectImage(
                stepseq, 
                waferseq
                );
        }

        //--

        public int SaveDefect(
            long stepseq,
            long waferseq,
            int imageCnt,
            string reviewEQ,
            string reviewFile,
            string reviewPath,
            string[,] Params
            )
        {
            using (DSL.TQD_STEP obj = new DSL.TQD_STEP())
            {
                obj.UpdateReviewInfo(
                    stepseq,
                    waferseq,
                    imageCnt.ToString(),
                    reviewEQ,
                    reviewFile,
                    reviewPath
                    );
            }

            using (DSL.TQD_DEFECT oDefect = new DSL.TQD_DEFECT())
            {
                if (Params.GetLength(0) > 0)
                    oDefect.UpdateDefect(Params);
            }

            using (DSL.TQD_INSP_INFO oInspInfo = new DSL.TQD_INSP_INFO())
            {
                oInspInfo.UpdateClassifiedCnt(
                    waferseq,
                    stepseq
                    );
            }

            return 1;
        }

        //--

        public int SaveDefect(
            long stepseq,
            long waferseq,
            string[,] Params
            )
        {
            int iResult = 0;

            //--

            using (DSL.TQD_DEFECT oDefect = new DSL.TQD_DEFECT())
            {
                if (Params.GetLength(0) > 0)
                    iResult = oDefect.UpdateDefect01(Params);
            }

            using (DSL.TQD_INSP_INFO oInspInfo = new DSL.TQD_INSP_INFO())
            {
                oInspInfo.UpdateClassifiedCnt(
                    waferseq,
                    stepseq
                    );
            }
            return iResult;
        }

        /// <summary>
        /// TQD_REVIEW_SUM 에 데이터 추가
        /// </summary>
        public void InsertReviewSum(long stepSeq)
        {
            DSL.TQD_REVIEW_SUM obj = new DSL.TQD_REVIEW_SUM();
            obj.DeleteData(stepSeq.ToString());
            obj.InsertData(stepSeq.ToString());
        }

        /// <summary>
        /// IMAGECOUNT 값을 일괄 업데이트 합니다.
        /// </summary>
        public void UpdateImageCount(long stepSeq)
        {
            DSL.TQD_DEFECT obj = new DSL.TQD_DEFECT();
            obj.UpdateImageCount(stepSeq);
        }
    }
}
