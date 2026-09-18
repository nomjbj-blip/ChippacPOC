using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.TEST.DSL;

namespace DACrux.TEST.BSL
{
    public class DieDefectMatching
    {
        /// <summary>
        /// TQD_DEFECT_DIE 테이블에 CP DIE 와 DEFECT 매칭 데이터 추가
        /// </summary>
        public static int InsertDefectDieData(string testWaferSeq, string program)
        {
            TQD_DEFECT_DIE obj = new TQD_DEFECT_DIE();

            // X,Y,BIN 필드가 있는 테이블명 가져오기
            string tableName = obj.GetProgramBinTable(program);

            if (String.IsNullOrEmpty(tableName))
                return 0;

            obj.DeleteData(testWaferSeq);

            return obj.InsertData(tableName, testWaferSeq);
        }
    }
}