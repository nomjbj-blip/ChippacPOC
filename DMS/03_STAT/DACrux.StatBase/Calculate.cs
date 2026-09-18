using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.StatBase
{
    public static class Calculate
    {
        public static double Median(double[] data)
        {

            double dMedian = double.NaN;
            try
            {
                if (data == null || data.Length == 0) return double.NaN;
                Array.Sort(data);
                if (data.Length % 2 == 0)
                {
                    dMedian = (data[data.Length / 2] + data[(data.Length / 2) - 1]) / 2d;
                }
                else
                {
                    dMedian = data[data.Length / 2];
                }

                return dMedian;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
