using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DACrux.Data.Parser.Klarf
{
    /*
    InspectionTest 1;
    SampleTestPlan 3363
      9 -38 
      8 -38 
      7 -38 
      ...
    AreaPerTest 6.3087e+009;
     */
    public class InspectionTestList : List<InspectionTest>, ICloneable
    {
        public object Clone()
        {
            InspectionTestList obj = new InspectionTestList();

            foreach (InspectionTest test in this)
            {
                obj.Add(test.Clone() as InspectionTest);
            }

            return obj;
        }

        public InspectionTest GetTest(int testNo)
        {
            foreach (var test in this)
            {
                if (test.TestNo == testNo)
                    return test;
            }

            throw new Exception(String.Format("해당하는 TEST 번호({0})를 찾을 수 없습니다.", testNo));
        }
    }

    public class InspectionTest : ICloneable
    {
        public void GetMinMaxDiePoint(out int xmin, out int xmax, out int ymin, out int ymax)
        {
            xmin = Int32.MaxValue;
            xmax = Int32.MinValue;

            ymin = Int32.MaxValue;
            ymax = Int32.MinValue;

            for (int i = 0; i < SampleTestPlan.Length; i++)
            {
                xmin = Math.Min(xmin, SampleTestPlan[i].X);
                xmax = Math.Max(xmax, SampleTestPlan[i].X);

                ymin = Math.Min(ymin, SampleTestPlan[i].Y);
                ymax = Math.Max(ymax, SampleTestPlan[i].Y);
            }
        }

        public int TestNo { get; internal set; }
        public double AreaPerTest { get; internal set; }
        public Point[] SampleTestPlan { get; internal set; }
        public TestParameterSpec TestParameterSpec { get; internal set; }
        public Summary Summary { get; internal set; }

        public object Clone()
        {
            InspectionTest obj = new InspectionTest();
            obj.TestNo = TestNo;
            obj.AreaPerTest = AreaPerTest;

            if (SampleTestPlan != null)
                obj.SampleTestPlan = SampleTestPlan.Clone() as Point[];

            if (TestParameterSpec != null)
                obj.TestParameterSpec = TestParameterSpec.Clone() as TestParameterSpec;

            if (Summary != null)
                obj.Summary = Summary.Clone() as Summary;

            return obj;
        }
    }
}
