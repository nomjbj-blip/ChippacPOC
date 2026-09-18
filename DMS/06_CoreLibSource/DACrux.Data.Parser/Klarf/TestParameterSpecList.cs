using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.Data.Parser.Klarf
{
    /*
    TestParametersSpec 2
      PIXELSIZE    SAMPLEPERCENTAGE  ;
    TestParametersList
      0.3906    0  ;
    DefectClusterSpec 3
     */
    public class TestParameterSpecList : List<TestParameterSpec>
    {
        public void Add(List<string> headers, List<double> dataArr)
        {
            if (headers == null || headers.Count == 0 || dataArr == null || dataArr.Count == 0)
                return;

            int length = dataArr.Count / headers.Count;

            for (int i = 0; i < length; i++)
            {
                Add(new TestParameterSpec(headers, dataArr.GetRange(i * headers.Count, headers.Count)));
            }
        }

        public TestParameterSpec GetParaSpec(int index)
        {
            if (Count > index)
                return this[index];

            return null;
        }
    }

    public class TestParameterSpec : ICloneable
    {
        private TestParameterSpec()
        {
        }

        public TestParameterSpec(List<string> headers, List<double> data)
        {
            ParsingUtil.SetPropertyData(this, headers, data);
        }

        public object Clone()
        {
            TestParameterSpec obj = new TestParameterSpec();
            obj.PIXELSIZE = PIXELSIZE;
            obj.SAMPLEPERCENTAGE = SAMPLEPERCENTAGE;
            return obj;
        }

        public double PIXELSIZE { get; internal set; }
        public double SAMPLEPERCENTAGE { get; internal set; }
    }
}
