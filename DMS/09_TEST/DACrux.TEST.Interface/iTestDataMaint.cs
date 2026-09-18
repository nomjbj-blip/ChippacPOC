using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.TEST.Interface
{
    public interface iTestDataMaint
    {
        /// <summary>
        /// WAFER의 LOT ID를 업데이트 합니다.
        /// </summary>
        void UpdateWaferLotID(string factory, string lotSeq, string waferSeq, string lotID);

        /// <summary>
        /// WAFER ID를 업데이트 합니다.
        /// </summary>
        void UpdateWaferID(string waferSeq, string waferID, int probeCnt);

        /// <summary>
        /// WAFER의 PROGRAM 명을 변경 합니다.
        /// </summary>
        void UpdateWaferProgram(string factory, string lotSeq, string waferSeq, string program);

        /// <summary>
        /// WAFER 데이터를 삭제 합니다.
        /// </summary>
        void DeleteWaferData(string waferSeq);

        /// <summary>
        /// LOT ID를 업데이트 합니다.
        /// </summary>
        void UpdateLotID(string lotSeq, string lotID);

        /// <summary>
        /// LOT의 PROGRAM 명을 변경 합니다.
        /// </summary>
        void UpdateLotProgram(string factory, string lotSeq, string currProgram, string newProgram);

        /// <summary>
        /// LOT 데이터를 삭제 합니다.
        /// </summary>
        void DeleteLotData(string lotSeq, string program);
    }
}
