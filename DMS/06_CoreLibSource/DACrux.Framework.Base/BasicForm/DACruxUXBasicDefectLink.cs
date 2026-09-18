using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Base;

namespace DACrux.Framework.Base
{
    public class DACruxUXBasicDefectLink : DACruxUXBasic01
    {
        public DACruxUXBasicDefectLink()
        {
            DefectList = new DefectList();
        }

        /// <summary>
        /// DefectArray 데이터가 존재하는지를 가져옵니다.
        /// </summary>
        protected bool ExistsDefectArray
        {
            get { return DefectList.Count > 0; }
        }

        public DefectList DefectList
        {
            get;
            private set;
        }
    }

    public interface ISendDefect
    {
        /// <summary>
        /// 선택된 Defect을 가져옵니다.
        /// </summary>
        Defect[] GetSelectedDefect();
    }
}
