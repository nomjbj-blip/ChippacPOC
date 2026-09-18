using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.StatBase
{
    [Serializable]
    public class DAData
    {
        public string EDC_TYPE = string.Empty;
        public string EDC_PLAN = string.Empty;
        public string EDC_SPEC = string.Empty;
        public string PROCESS = string.Empty;
        public string STEP = string.Empty;
        public string DEVICE = string.Empty;
        public string PACKAGE = string.Empty;
        public string PRODFAMILY = string.Empty;
        public string UNIT = string.Empty;
        public string SAMPLE_SIZE = string.Empty;
        public string EQP_ID = string.Empty;
        public string LOT_ID = string.Empty;
        public string USER = string.Empty;
        public string VALUE_COUNT = string.Empty;
        public string DATA_TIME = string.Empty;
        public string MEAN = string.Empty;
        public string COMMENTS = string.Empty;


        int m_iCount = 0;
        int m_iNCount = 0;
        double m_dSIGMA = double.NaN;
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
        double m_dSUM = double.NaN;
        double m_dSOS = double.NaN;

        double m_dUSL = double.NaN;
        double m_dTARGET = double.NaN;
        double m_dLSL = double.NaN;

        double m_dFixedUCL = double.NaN;
        double m_dFixedCL = double.NaN;
        double m_dFixedLCL = double.NaN;

        string m_strResult = "N";
        string m_strDataLable = "NONE";
        string m_strUserTag = "";
        bool m_bVisible = true;
        long m_iDataSeq = -1;

        List<double> m_fRawData = null;

        bool m_bCalculated = false;

        public DAData()
        {
            m_fRawData = new List<double>();
        }

        public DAData(int capacity)
        {
            m_fRawData = new List<double>(capacity);
        }

        public void DataOpen()
        {
            if (m_fRawData != null && m_fRawData.Count != 0)
            {
                m_fRawData = new List<double>();
                m_bCalculated = false;
            }
        }

        public void Add(double Value)
        {
            if (m_fRawData == null)
            {
                m_fRawData = new List<double>();
            }

            m_fRawData.Add(Value);
            m_bCalculated = false;
        }

        public void AddRange(double[] values)
        {
            if (m_fRawData == null)
            {
                m_fRawData = new List<double>();
            }

            m_fRawData.AddRange(values);
            m_bCalculated = false;
        }

        public void DataClose()
        {
            // Stat Method 실행
            CalDescriptive();
        }

        public void SetSpec(double USL, double LSL)
        {
            m_dUSL = USL;
            m_dLSL = LSL;
            //m_bCalculated = false;
        }

        public void SetSpec(double USL, double LSL, double Target)
        {
            m_dUSL = USL;
            m_dLSL = LSL;
            m_dTARGET = Target;
            //m_bCalculated = false;
        }


        public void SetControl(double UCL, double CL, double LCL)
        {
            m_dFixedUCL = UCL;
            m_dFixedCL = CL;
            m_dFixedLCL = LCL;
            //m_bCalculated = false;
        }

        public long DataSeq
        {
            set
            {
                m_iDataSeq = value;
            }
            get
            {
                return m_iDataSeq;
            }
        }

        public bool Visible
        {
            set
            {
                m_bVisible = value;
            }
            get
            {
                return m_bVisible;
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
                if (m_fRawData == null) return 0;
                return m_fRawData.Count;
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
                return m_dK;
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

        public double FixedUCL
        {
            get
            {
                return m_dFixedUCL;
            }
        }

        public double FixedLCL
        {
            get
            {
                return m_dFixedLCL;
            }
        }

        public double FixedCL
        {
            get
            {
                return m_dFixedCL;
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

        public double RANGE
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return (m_dMAX - m_dMIN);
            }
        }

        public double SOS
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dSOS;
            }
        }

        public double SUM
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dSUM;
            }
        }

        public List<double> DataValue
        {
            get
            {
                return m_fRawData;
            }
        }

        public string RESULT
        {
            set
            {
                m_strResult = value;
            }
            get
            {
                return m_strResult;
            }
        }

        public string Label
        {
            set
            {
                m_strDataLable = value; 
            }
            get
            {
                return m_strDataLable;
            }
        }

        public string UserTag
        {
            set
            {
                m_strUserTag = value;
            }
            get
            {
                return m_strUserTag;
            }
        }

        protected void InitResult()
        {
            m_iCount = 0;
            m_iNCount = 0;
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
            m_dSUM = double.NaN;
            m_dSOS = double.NaN;
            //m_bCalculated = false;
        }

        /// <summary>
        /// 2013-01-30 : YS Lim Create
        /// 통계량을 계산해주는 메소드(Target이 중심이 아닐경우 Cpm만 돌려줌.)
        /// </summary>
        /// <param name="daData"> 1차원(선형)으로 된 그룹구분 없는 Data Array</param>
        protected void CalDescriptive()
        {
            double dblMax = double.MinValue;
            double dblMin = double.MaxValue;


            decimal dblAcc = 0;
            decimal dblSum = 0;

            decimal dblAccTmp = 0;
            decimal dblTemp = 0;
            double dblM;
            decimal dblSigma_;

            try
            {
                if (m_bCalculated) return;
                InitResult();
                dblAccTmp = 0;

                for (int j = 0; j < m_fRawData.Count; j++)
                {
                    if (m_fRawData[j] != double.NaN)
                    {
                        if (m_fRawData[j] > dblMax) dblMax = m_fRawData[j];
                        if (m_fRawData[j] < dblMin) dblMin = m_fRawData[j];

                        dblAcc += (decimal)m_fRawData[j];
                        dblSum += (decimal)Math.Pow(m_fRawData[j], 2);

                        dblAccTmp += (decimal)m_fRawData[j];
                        m_iNCount++;
                    }
                }

                if (m_iNCount > 0)
                    dblTemp = dblAccTmp / m_iNCount;

                if (dblMin > 1000000 || m_fRawData.Count == 0)
                {
                    m_dMIN = dblMin;
                }


                m_dSOS = (double)dblSum;

                m_iCount = m_fRawData.Count;
                m_dMIN = dblMin;
                m_dMAX = dblMax;

                m_dSUM = (double)dblAcc;

                if (m_iNCount > 0)
                    m_dAVG = (double)dblAcc / m_iNCount;
                
                if (m_fRawData.Count < 2 || m_iNCount < 2)  //자료수 부족
                {
                    m_dMedian = Calculate.Median(m_fRawData.ToArray());
                    return;
                }

                m_dSIGMA = (double)Math.Sqrt((double)((dblSum / (decimal)(m_iNCount - 1)) - (decimal)(m_dAVG * m_dAVG * m_iNCount / (m_iNCount - 1))));
                m_dUCL = (double)m_dAVG + (3 * (double)m_dSIGMA);
                m_dLCL = (double)m_dAVG - (3 * (double)m_dSIGMA);

                if (!double.IsNaN(m_dUSL) && !double.IsNaN(m_dLSL))
                {
                    m_dCP = (m_dUSL - m_dLSL) / (6 * (double)m_dSIGMA);
                    m_dCPU = (m_dUSL - (double)m_dAVG) / (3 * (double)m_dSIGMA);
                    m_dCPL = ((double)m_dAVG - m_dLSL) / (3 * (double)m_dSIGMA);
                    dblM = Math.Round(((m_dUSL + m_dLSL) / 2), 4);
                    m_dK = Math.Abs(dblM - (double)m_dAVG) / ((m_dUSL - m_dLSL) / 2);
                    m_dTARGET = (m_dUSL + m_dLSL) / 2;

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
                        dblSigma_ = (decimal)Math.Sqrt(Math.Pow((double)m_dSIGMA, 2) + Math.Pow(((double)m_dAVG - m_dTARGET), 2));
                        m_dCPM = (m_dUSL - m_dLSL) / (6 * (double)dblSigma_);
                    }
                }

                m_dMedian = Calculate.Median(m_fRawData.ToArray());
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
    }
}
