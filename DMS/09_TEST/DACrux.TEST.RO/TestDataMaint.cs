using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.TEST.RO
{
    public class TestDataMaint
    {
        DACrux.TEST.Interface.iTestDataMaint m_OBJ;

        public TestDataMaint()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_PRB);
            object obj = Activator.GetObject(typeof(DACrux.TEST.Interface.iTestDataMaint)
                , strUrl + "/DACrux.TEST.BSL.TestDataMaint.bin");
            m_OBJ = obj as DACrux.TEST.Interface.iTestDataMaint;
        }

        /// <summary>
        /// WAFER의 LOT ID를 업데이트 합니다.
        /// </summary>
        public void UpdateWaferLotID(string factory, string lotSeq, string waferSeq, string lotID)
        {
            m_OBJ.UpdateWaferLotID(factory, lotSeq, waferSeq, lotID);
        }

        /// <summary>
        /// WAFER ID를 업데이트 합니다.
        /// </summary>
        public void UpdateWaferID(string waferSeq, string waferID, int probeCnt)
        {
            m_OBJ.UpdateWaferID(waferSeq, waferID, probeCnt);
        }

        /// <summary>
        /// WAFER의 PROGRAM 명을 변경 합니다.
        /// </summary>
        public void UpdateWaferProgram(string factory, string lotSeq, string waferSeq, string program)
        {
            m_OBJ.UpdateWaferProgram(factory, lotSeq, waferSeq, program);
        }

        /// <summary>
        /// WAFER 데이터를 삭제 합니다.
        /// </summary>
        public void DeleteWaferData(string waferSeq)
        {
            m_OBJ.DeleteWaferData(waferSeq);
        }

        /// <summary>
        /// LOT ID를 업데이트 합니다.
        /// </summary>
        public void UpdateLotID(string lotSeq, string lotID)
        {
            m_OBJ.UpdateLotID(lotSeq, lotID);
        }

        /// <summary>
        /// LOT의 PROGRAM 명을 변경 합니다.
        /// </summary>
        public void UpdateLotProgram(string factory, string lotSeq, string currProgram, string newProgram)
        {
            m_OBJ.UpdateLotProgram(factory, lotSeq, currProgram, newProgram);
        }

        /// <summary>
        /// LOT 데이터를 삭제 합니다.
        /// </summary>
        public void DeleteLotData(string lotSeq, string program)
        {
            m_OBJ.DeleteLotData(lotSeq, program);
        }
    }
}
