/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : Descriptive.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2015.02.10
--  Description     : 기초 통계량 계산
--  History         : Created by YSIM at 2015.02.10
 * ********************************************************************************************************
  * 2015 년 DACrux V5 History Start
 ----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Stat.Core
{
    [Serializable]
    public class Descriptive
    {
        double m_dSum = double.NaN;
        double m_dAvg = double.NaN;
        double m_dStd = double.NaN;
        double m_dVar = double.NaN;
        double m_dSumOfSq = double.NaN;
        double m_dNSumOfSq = double.NaN;
        double m_dMin = double.NaN;
        double m_dMax = double.NaN;
        double m_dMedian = double.NaN;
        double m_dQ1 = double.NaN;
        double m_dQ3 = double.NaN;

        /// <summary>
        /// USL / LSL 
        /// </summary>
        double m_dUSL = double.NaN;
        double m_dLSL = double.NaN;
        double m_dTAR = double.NaN;
        int m_iOOS = 0;

        /// <summary>
        /// UCL / LCL
        /// </summary>
        double m_dUCL = double.NaN;
        double m_dLCL = double.NaN;
        double m_dCL = double.NaN;
        int m_iOOC = 0;

        int m_dCntAll = 0;
        int m_dCntAva = 0;
        List<Double> m_dRawData = null;
        List<Double> m_dRawData_Clean = null;

        public double SUM { get { return m_dSum; } }
        public double AVG { get { return m_dAvg; } }
        public double STD { get { return m_dStd; } }
        public double VAR { get { return m_dVar; } }
        public double SOS { get { return m_dSumOfSq; } }
        public double NOS { get { return m_dNSumOfSq; } }
        public double MIN { get { return m_dMin; } }
        public double MAX { get { return m_dMax; } }
        public double MEDIAN { get { return m_dMedian; } }

        public int N { get { return m_dCntAll; } }
        public int MissingN { get { return m_dCntAll - m_dCntAva; } }


        /// <summary>
        /// Min
        /// </summary>
        public double Q0 { get { return m_dMin; } }

        public double Q1 { get { return m_dQ1; } }

        /// <summary>
        /// Median
        /// </summary>
        public double Q2 { get { return m_dMedian; } }

        public double Q3 { get { return m_dQ3; } }

        /// <summary>
        /// Max
        /// </summary>
        public double Q4 { get { return m_dMax; } }

        public double ALLCount { get { return m_dCntAll; } }
        public double AVACount { get { return m_dCntAva; } }
        public double[] RAW { get { return m_dRawData.ToArray(); } }
        public double[] RAW_CLEAN { get { return m_dRawData_Clean.ToArray(); } }

        public Descriptive(List<Double> dRawData)
        {
            try
            {
                m_dRawData = dRawData;
                m_dRawData_Clean = new List<double>();
                Process();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Process()
        {
            m_dCntAll = m_dRawData.Count;
            for (int i = 0; i < m_dCntAll; i++)
            {
                if (m_dRawData[i] != double.NaN)
                {
                    m_dRawData_Clean.Add(m_dRawData[i]);
                    if (double.IsNaN(m_dSumOfSq)) m_dSumOfSq = 0;
                    if (double.IsNaN(m_dNSumOfSq)) m_dNSumOfSq = 0;
                    if (double.IsNaN(m_dSum)) m_dSum = 0;
                    m_dCntAva++;
                    m_dSumOfSq += Math.Pow(m_dRawData[i], 2);
                    m_dNSumOfSq += 1 / Math.Pow(m_dRawData[i], 2);
                    m_dSum += m_dRawData[i];

                    #region Check OOS
                    if (!double.IsNaN(m_dUSL) && !double.IsNaN(m_dLSL))  //상하한 모두 존재
                    {
                        if (m_dRawData[i] > m_dUSL)  m_iOOS++;
                        if (m_dRawData[i] < m_dLSL)  m_iOOS++;
                    }
                    else if (!double.IsNaN(m_dUSL) && double.IsNaN(m_dLSL))  // 상한만 존재
                    {
                        if (m_dRawData[i] > m_dUSL) m_iOOS++;
                    }
                    else if (double.IsNaN(m_dUSL) && !double.IsNaN(m_dLSL)) // 하한만 존재
                    {
                        if (m_dRawData[i] < m_dLSL) m_iOOS++;
                    }
                    #endregion

                    #region Check OOC
                    if (!double.IsNaN(m_dUCL) && !double.IsNaN(m_dLCL))  //상하한 모두 존재
                    {
                        if (m_dRawData[i] > m_dUCL)  m_iOOC++;
                        if (m_dRawData[i] < m_dLCL)  m_iOOC++;
                    }
                    else if (!double.IsNaN(m_dUCL) && double.IsNaN(m_dLCL))  // 상한만 존재
                    {
                        if (m_dRawData[i] > m_dUCL) m_iOOC++;
                    }
                    else if (double.IsNaN(m_dUCL) && !double.IsNaN(m_dLCL)) // 하한만 존재
                    {
                        if (m_dRawData[i] < m_dLCL) m_iOOC++;
                    }
                    #endregion
                }
            }

            //1. Clean Data Count
            m_dCntAva = m_dRawData_Clean.Count;

            //2. 평균
            m_dAvg = m_dSum / (double)m_dCntAva;

            //3. 분산
            #region OLD , Loop를 한번 더돌게되어서 안돌수 있도록 수식 수정 (YSIM 2015-02-09)
            //m_ddSum = 0;
            //for (int ir = 0; ir < dCntAll; ir++)
            //{
            //    if (m_dRawData[ir] != double.NaN)
            //    {
            //        m_ddRawData_avg.Add(m_dRawData[ir] - m_ddAvg);
            //        m_ddSum += Math.Pow(m_dRawData[ir] - m_ddAvg, 2);
            //    }
            //}

            //m_ddVar = m_ddSum / (m_ddCntAva - 1);
            //if (m_ddVar == 0) m_ddVar = double.NaN;
            #endregion
            m_dVar = (m_dSumOfSq / m_dCntAva) - Math.Pow(m_dAvg, 2);

            //4. 표준편차
            m_dStd = Math.Sqrt(m_dVar);

            //5. 중심값
            m_dMedian = Median(m_dRawData_Clean.ToArray(),ref m_dMax, ref m_dMin);



            int count = -1;
            if ((m_dCntAva % 2) == 0)
            {
                count = (int)m_dCntAva / 2;
            }
            else
            {
                // 홀수개였을때 중앙값은 아무데도 속하지 않음
                count = (int)m_dCntAva / 2;
            }

            //6. Q1
            double[] tmpQ1 = new double[count];
            Array.Copy(m_dRawData_Clean.ToArray(), tmpQ1, count);
            m_dMedian = Median(tmpQ1);

            //7. Q3
            double[] tmpQ3 = new double[count];
            Array.Copy(m_dRawData_Clean.ToArray(), count + (m_dCntAva % 2), tmpQ3, 0, count);
            m_dMedian = Median(tmpQ3);
        }


        public Descriptive(DataTable dt, int[] ValueColumnIndexes, string USL = null, string TAR = null, string LSL = null, string UCL = null, string LCL = null, string CL = null)
        {
            try
            {
                if (!double.TryParse(USL, out m_dUSL))
                    m_dUSL = double.NaN;
                if (!double.TryParse(TAR, out m_dTAR))
                    m_dTAR = double.NaN;
                if (!double.TryParse(LSL, out m_dLSL))
                    m_dLSL = double.NaN;
                if (!double.TryParse(UCL, out m_dUCL))
                    m_dUCL = double.NaN;
                if (!double.TryParse(LCL, out m_dLCL))
                    m_dLCL = double.NaN;
                if (!double.TryParse(CL, out m_dCL))
                    m_dCL = double.NaN;

                m_dRawData = new List<double>();
                m_dRawData_Clean = new List<double>();
                double[] tmp = new double[dt.Rows.Count];
                for (int i = 0; i < ValueColumnIndexes.Length; i++)
                {
                    dt.Columns.CopyTo(tmp, 0);
                    m_dRawData.AddRange(tmp);
                }
                Process();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double Median(double[] data)
        {
            double dMedian = double.NaN;
            double dMax = double.NaN;
            double dMin = double.NaN;
            try
            {
                dMedian = Median(data, ref dMax, ref dMin);
                return dMedian;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double Median(double[] data,ref double Max, ref double Min)
        {
            double dMedian = double.NaN;
            try
            {
                if (data == null || data.Length == 0) return double.NaN;
                Array.Sort(data);

                Max = data[data.Length - 1];
                Min = data[0];

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
