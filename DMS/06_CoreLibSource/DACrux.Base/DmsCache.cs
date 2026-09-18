using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;

namespace DACrux.Base
{
    public class DmsCache
    {
        #region 멤버 변수

        public delegate DefectClassDictionary ClassLookupDelegate();
        public delegate DmsWaferDieInfo WaferDieInfoDelegate(long stepSeq);

        private DefectClassDictionary _classLookup;

        public ClassLookupDelegate ClassLookupDel;
        public WaferDieInfoDelegate WaferDieInfoDel;

        #endregion

        #region 메서드

        static DmsCache()
        {
            Instance = new DmsCache();
        }

        private DmsCache()
        {
            WaferDieInfoList = new List<DmsWaferDieInfo>();
        }

        #endregion

        #region 프로퍼티

        public static DmsCache Instance
        {
            get;
            private set;
        }

        public List<DmsWaferDieInfo> WaferDieInfoList
        {
            get;
            private set;
        }

        public DefectClassDictionary ClassLookup
        {
            get
            {
                if (_classLookup == null)
                    _classLookup = ClassLookupDel.Invoke();

                return _classLookup;
            }
        }

        public DmsWaferDieInfo this[long stepSeq]
        {
            get
            {
                foreach (DmsWaferDieInfo obj in WaferDieInfoList)
                {
                    if (obj == null)
                        continue;

                    if (obj.StepSeq == stepSeq)
                        return obj;
                }

                WaferDieInfoList.Add(WaferDieInfoDel.Invoke((stepSeq)));
                return WaferDieInfoList[WaferDieInfoList.Count - 1];
            }
        }

        #endregion
    }

    /// <summary>
    /// Defect Class를 나타내는 클래스. classnumber가 정의되지 않은 경우에도 데이터 리턴
    /// </summary>
    public class DefectClassDictionary : Dictionary<int, string>
    {
        public new string this[int key]
        {
            get
            {
                if (ContainsKey(key))
                    return base[key];
                else
                    return key.ToString();
            }

            set
            {
                base[key] = value;
            }
        }

        public int GetKey(string name)
        {
            foreach (var item in this)
            {
                if (item.Value == name)
                    return item.Key;
            }

            int key;

            if (!Int32.TryParse(name, out key))
                key = Int32.MaxValue;

            return key;
        }
    }
}
