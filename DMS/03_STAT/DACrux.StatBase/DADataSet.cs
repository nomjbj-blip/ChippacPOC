using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.Statistics
{
    [Serializable]
    public class DADataSet
    {
        List<DAData> m_oDatas = null;
        bool m_bCalculated = false;

        int m_iCount = 0;
        int m_iNCount = 0;
        int m_iVisibleCount = 0;

        double m_dSIGMA = double.NaN;
        double m_dSIGMA2 = double.NaN;
        double m_dMIN = double.NaN;
        double m_dMAX = double.NaN;
        double m_dAVG = double.NaN;
        double m_dK = double.NaN;
        double m_dCP = double.NaN;
        double m_dCPK = double.NaN;
        double m_dCPU = double.NaN;
        double m_dCPL = double.NaN;
        double m_dCPM = double.NaN;
        double m_dUCL = double.NaN;
        double m_dLCL = double.NaN;
        double m_dMedian = double.NaN;

        double m_dRMIN = double.NaN;
        double m_dRMAX = double.NaN;
        double m_dRAVG = double.NaN;

        double m_dUSL = double.NaN;
        double m_dTARGET = double.NaN;
        double m_dLSL = double.NaN;


        double m_dMAXUSL = double.NaN;
        double m_dMINLSL = double.NaN;


        public DADataSet()
        {
            m_oDatas = new List<DAData>();
            m_bCalculated = false;
        }

        public List<DAData> Datas
        {
            get
            {
                return m_oDatas;
            }
        }


        public double MAXUSL
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMAXUSL;
            }
        }

        public double MINLSL
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMINLSL;
            }
        }

        public double USL
        {
            get
            {
                return m_dUSL;
            }
        }

        public double LSL
        {
            get
            {
                return m_dLSL;
            }
        }

        public double Target
        {
            get
            {
                return m_dTARGET;
            }
        }

        public int DataCount
        {
            get
            {
                if (m_oDatas == null) return 0;
                return m_oDatas.Count;
            }
        }

        public int NCount
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_iNCount;
            }
        }

        public double SIGMA
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dSIGMA;
            }
        }

        public double MIN
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMIN;
            }
            set
            {
                m_dMIN = value;
            }
        }

        public double MAX
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMAX;
            }
        }

        public double RMIN
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dRMIN;
            }
            set
            {
                m_dRMIN = value;
            }
        }

        public double RMAX
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dRMAX;
            }
        }

        public double RAVG
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dRAVG;
            }
        }

        public double AVG
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dAVG;
            }
            set
            {
                m_dAVG = value;
            }
        }

        public double K
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return K;
            }
        }

        public double CP
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dCP;
            }
        }

        public double CPK
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dCPK;
            }
        }

        public double CPU
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dCPU;
            }
        }

        public double CPL
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dCPL;
            }
        }

        public double CPM
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dCPM;
            }
        }

        public double UCL
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dUCL;
            }
        }

        public double LCL
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dLCL;
            }
        }

        public double Median
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMedian;
            }
        }

        public int VisibleDataCount
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_iVisibleCount;
            }
        }

        protected void InitResult()
        {
            m_iCount = 0;
            m_iNCount = 0;
            m_iVisibleCount = 0;
            m_dSIGMA = double.NaN;
            m_dMIN = double.NaN;
            m_dMAX = double.NaN;
            m_dAVG = double.NaN;
            m_dK = double.NaN;
            m_dCP = double.NaN;
            m_dCPK = double.NaN;
            m_dCPU = double.NaN;
            m_dCPL = double.NaN;
            m_dCPM = double.NaN;
            m_dUCL = double.NaN;
            m_dLCL = double.NaN;
            m_dMedian = double.NaN;

            m_dMAXUSL = double.MinValue;
            m_dMINLSL = double.MaxValue;

            m_bCalculated = false;
        }

        /// <summary>
        /// 통계량을 계산해주는 메소드(Target이 중심이 아닐경우 Cpm만 돌려줌.)
        /// 상세  설명: Data dt와 [USL, Target, LSL]string array을 받아서 [sigma, TotalCount , n, Min, Max, Avg, Cp, K,   Cpk, Cpu, Cpl, Cpm, Ucl, Lcl]string array 반환<br/>
        /// </summary>
        /// <param name="dr"> 2차원(직사각형)으로 된 그룹구분 없는 Data Rows</param>
        /// <param name="arrInput">[USL, Target, LSL]의 string Array</param>
        public void CalDescriptive()
        {
            int iRowsCNT = 0;
            int iDataCNT = 0;
            int iTotalCnt = 0;

            string[] arrReturn = new string[14];

            decimal dblAcc = 0;
            double dblMax = double.MinValue;
            double dblMin = double.MaxValue;

            double dblMax_R = double.MinValue;
            double dblMin_R = double.MaxValue;

            //decimal dblSigma;

            decimal dblAcc2 = 0;
            decimal dblSum2 = 0;
            int iSampleCNT = 0;

            double dblM;
            decimal dblSigma_;


            try
            {
                if (m_bCalculated) return;
                InitResult();
                iRowsCNT = m_oDatas.Count;


                foreach (DAData oData in m_oDatas)
                {
                    if (oData.MAX > dblMax) dblMax = oData.MAX;
                    if (oData.MIN > dblMin) dblMin = oData.MIN;

                    if (oData.RANGE > dblMax_R) dblMax_R = oData.RANGE;
                    if (oData.RANGE < dblMin_R) dblMin_R = oData.RANGE;

                    dblAcc += (decimal)oData.SUM;
                    dblAcc2 += (decimal)oData.AVG;
                    dblSum2 += (decimal)Math.Pow(System.Convert.ToDouble(oData.AVG), 2);
                    iDataCNT += oData.NCount;
                    iTotalCnt += oData.DataCount;
                    if (oData.NCount > iSampleCNT) iSampleCNT = oData.NCount;

                    if (!double.IsNaN(oData.LSL) && oData.LSL < m_dMINLSL) m_dMINLSL = oData.LSL;
                    if (!double.IsNaN(oData.USL) && oData.USL > m_dMAXUSL) m_dMAXUSL = oData.USL;
                    if (oData.Visible) m_iVisibleCount++;
                }

                if (iRowsCNT < 2)  //자료수 부족
                {
                    return;
                }
                if (iDataCNT < 2) //자료수 부족
                {
                    return;
                }

                m_iCount = iTotalCnt;
                m_iNCount = iDataCNT;
                m_dMIN = dblMin;
                m_dMAX = dblMax;
                m_dAVG = (double)(dblAcc / iDataCNT);

                m_dRMAX = dblMax_R;
                m_dRMIN = dblMin_R;

                m_dSIGMA = Math.Sqrt(((double)(dblSum2 / (iRowsCNT - 1)) - (double)(m_dAVG * m_dAVG * iRowsCNT / (iRowsCNT - 1))));
                m_dSIGMA2 = (double)(m_dSIGMA / Math.Sqrt(iSampleCNT));

                m_dUCL = (double)(m_dAVG + (double)(3 * m_dSIGMA2));
                m_dLCL = (double)(m_dAVG - (double)(3 * m_dSIGMA2));


                if (!double.IsNaN(m_dUSL) && !double.IsNaN(m_dLCL)) // 상한 하한 모두 존재
                {
                    //먼저 상한과 하한이 있는 경우 CP를 구함.
                    m_dCP = (m_dUSL - m_dLSL) / (6 * (double)m_dSIGMA);

                    m_dCPU = (m_dUSL - (double)m_dAVG) / (3 * (double)m_dSIGMA);
                    m_dCPL = ((double)m_dAVG - m_dLSL) / (3 * (double)m_dSIGMA);

                    dblM = Math.Round(((m_dUSL + m_dLSL) / 2), 4);

                    m_dK = Math.Abs(dblM - (double)m_dAVG) / ((m_dUSL - m_dLSL) / 2);
                    if (double.IsNaN(m_dTARGET)) //Target없을 때
                    {
                        m_dTARGET = (m_dUSL + m_dLSL) / 2;
                    }

                    if (dblM == m_dTARGET) //목표치가 스펙의 중심일 때
                    {
                        if (m_dCPL < m_dCPU)
                        {
                            m_dCPK = m_dCPL;
                        }
                        else
                        {
                            m_dCPK = m_dCPU;
                        }
                    }
                    else
                    {
                        dblSigma_ = (decimal)Math.Sqrt(Math.Pow((double)m_dSIGMA, 2) + Math.Pow(((double)m_dAVG - m_dSIGMA), 2));
                        m_dCPM = (m_dUSL - m_dLSL) / (6 * (double)dblSigma_);
                        arrReturn[11] = System.Convert.ToString(m_dCPM);
                    }
                }
                else if (!double.IsNaN(m_dUSL)) // USL만 존재하는 경우
                {
                    m_dCPU = (m_dUSL - (double)m_dAVG) / (3 * (double)m_dSIGMA);
                }
                else if (!double.IsNaN(m_dLSL)) // LSL만 존재하는 경우
                {
                    m_dCPL = ((double)m_dAVG - m_dLSL) / (3 * (double)m_dSIGMA);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_bCalculated = true;
            }
        }

        public void Add(DAData data)
        {
            m_oDatas.Add(data);
            m_bCalculated = false;
        }
    }
}
