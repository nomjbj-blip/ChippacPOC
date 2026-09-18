using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.Data.Parser.Klarf
{
    public class SummaryList : List<Summary>
    {
        public void Add(List<string> headers, List<double> dataArr)
        {
            if (headers == null || headers.Count == 0 || dataArr == null || dataArr.Count == 0)
                return;

            int length = dataArr.Count / headers.Count;

            for (int i = 0; i < length; i++)
            {
                Add(new Summary(headers, dataArr.GetRange(i * headers.Count, headers.Count)));
            }
        }

        public Summary GetSummary(int testNo)
        {
            foreach (Summary summ in this)
            {
                if (summ.TESTNO == testNo)
                    return summ;
            }

            return null;
        }
    }

    public class Summary : ICloneable
    {
        private Summary()
        {
        }

        public Summary(List<string> headers, List<double> data)
        {
            ParsingUtil.SetPropertyData(this, headers, data);
        }

        public object Clone()
        {
            Summary obj = new Summary();
            obj.TESTNO = TESTNO;
            obj.NDEFECT = NDEFECT;
            obj.DEFDENSITY = DEFDENSITY;
            obj.NDIE = NDIE;
            obj.NDEFDIE = NDEFDIE;
            return obj;
        }

        public int TESTNO { get; internal set; }
        public int NDEFECT { get; internal set; }
        public double DEFDENSITY { get; internal set; }
        public int NDIE { get; internal set; }
        public int NDEFDIE { get; internal set; }
    }
}
