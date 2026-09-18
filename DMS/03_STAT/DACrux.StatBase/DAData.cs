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
        public string CUSTOMER = string.Empty;
        public string UNIT = string.Empty;
        public string SAMPLE_SIZE = string.Empty;
        public string EQP_ID = string.Empty;
        public string LOT_ID = string.Empty;
        public string USER = string.Empty;
        public string VALUE_COUNT = string.Empty;
        public string DATA_TIME = string.Empty;
        public string COMMENTS = string.Empty;
        public string INTCOMMENTS = string.Empty;

        public double MEAN = double.NaN;
        public double SIGMA = double.NaN;
        public double RANGE = double.NaN;

        public double EWMA_M = double.NaN;
        public double EWMA_S = double.NaN;
        public double EWMA_R = double.NaN;

        public double MS = double.NaN;
        public double MA = double.NaN;

        public double USL = double.NaN;
        public double TARGET = double.NaN;
        public double LSL = double.NaN;

        public double UCL = double.NaN;
        public double CL = double.NaN;
        public double LCL = double.NaN;

        public double SigmaUCL = double.NaN;
        public double SigmaCL = double.NaN;
        public double SigmaLCL = double.NaN;

        public double RangeUCL = double.NaN;
        public double RangeCL = double.NaN;
        public double RangeLCL = double.NaN;

        public double EWMA_M_UCL = double.NaN;
        public double EWMA_M_CL = double.NaN;
        public double EWMA_M_LCL = double.NaN;

        public double EWMA_S_UCL = double.NaN;
        public double EWMA_S_CL = double.NaN;
        public double EWMA_S_LCL = double.NaN;

        public double EWMA_R_UCL = double.NaN;
        public double EWMA_R_CL = double.NaN;
        public double EWMA_R_LCL = double.NaN;

        public double MS_UCL = double.NaN;
        public double MS_CL = double.NaN;
        public double MS_LCL = double.NaN;

        public double MA_UCL = double.NaN;
        public double MA_CL = double.NaN;
        public double MA_LCL = double.NaN;

        public double RAW_UCL = double.NaN;
        public double RAW_CL = double.NaN;
        public double RAW_LCL = double.NaN;

        public long DataSeq = -1;

        public string Result = "";
        public string DataLable = "NONE";
        public bool Visible = true;



        //int m_iCount = 0;
        //int m_iNCount = 0;
        double m_dMIN = double.NaN;
        double m_dMAX = double.NaN;
        double m_dSUM = 0d;
        List<double> m_fRawData = null;

        public DAData()
        {
            m_fRawData = new List<double>();
        }

        public void Add(double Value)
        {
            if (m_fRawData == null)
            {
                m_fRawData = new List<double>();
            }
            m_fRawData.Add(Value);

            if (!double.IsNaN(Value))
            {
                m_dSUM += Value;
                if (m_dMAX < Value) m_dMAX = Value;
                if (m_dMIN > Value) m_dMIN = Value;
            }
        }
        public void AddRange(double[] values)
        {
            if (m_fRawData == null)
            {
                m_fRawData = new List<double>();
            }

            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
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

        public double MIN
        {
            get
            {
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
                return m_dMAX;
            }
            set
            {
                m_dMAX = value;
            }
        }

        // Deleted By James Kwon 14/09/24
        //public double RANGE
        //{
        //    get
        //    {
        //        return (m_dMAX - m_dMIN);
        //    }
        //}

        public double SUM
        {
            get
            {
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

        protected void InitResult()
        {
            EDC_TYPE = string.Empty;
            EDC_PLAN = string.Empty;
            EDC_SPEC = string.Empty;
            PROCESS = string.Empty;
            STEP = string.Empty;
            DEVICE = string.Empty;
            PACKAGE = string.Empty;
            PRODFAMILY = string.Empty;
            UNIT = string.Empty;
            SAMPLE_SIZE = string.Empty;
            EQP_ID = string.Empty;
            LOT_ID = string.Empty;
            USER = string.Empty;
            VALUE_COUNT = string.Empty;
            DATA_TIME = string.Empty;
            COMMENTS = string.Empty;

            MEAN = double.NaN;
            SIGMA = double.NaN;

            EWMA_M = double.NaN;
            EWMA_S = double.NaN;
            EWMA_R = double.NaN;

            USL = double.NaN;
            TARGET = double.NaN;
            LSL = double.NaN;

            UCL = double.NaN;
            CL = double.NaN;
            LCL = double.NaN;

            SigmaUCL = double.NaN;
            SigmaCL = double.NaN;
            SigmaLCL = double.NaN;

            RangeUCL = double.NaN;
            RangeCL = double.NaN;
            RangeLCL = double.NaN;

            EWMA_M_UCL = double.NaN;
            EWMA_M_CL = double.NaN;
            EWMA_M_LCL = double.NaN;

            EWMA_S_UCL = double.NaN;
            EWMA_S_CL = double.NaN;
            EWMA_S_LCL = double.NaN;

            EWMA_R_UCL = double.NaN;
            EWMA_R_CL = double.NaN;
            EWMA_R_LCL = double.NaN;

            RAW_UCL = double.NaN;
            RAW_CL = double.NaN;
            RAW_LCL = double.NaN;

            DataSeq = -1;

            Result = "";
            DataLable = "NONE";
            Visible = true;

            //m_iCount = 0;
            //m_iNCount = 0;
            m_dMIN = double.NaN;
            m_dMAX = double.NaN;
            m_dSUM = double.NaN;
            m_fRawData = null;
        }

        ///// <summary>
        ///// 2013-01-30 : YS Lim Create
        ///// 통계량을 계산해주는 메소드(Target이 중심이 아닐경우 Cpm만 돌려줌.)
        ///// </summary>
        ///// <param name="daData"> 1차원(선형)으로 된 그룹구분 없는 Data Array</param>
        //protected void CalDescriptive()
        //{
        //    double dblMax = double.MinValue;
        //    double dblMin = double.MaxValue;


        //    decimal dblAcc = 0;
        //    decimal dblSum = 0;

        //    decimal dblAccTmp = 0;
        //    decimal dblTemp = 0;
        //    double dblM;
        //    decimal dblSigma_;

        //    try
        //    {
        //        if (m_bCalculated) return;
        //        InitResult();
        //        dblAccTmp = 0;

        //        for (int j = 0; j < m_fRawData.Count; j++)
        //        {
        //            if (m_fRawData[j] != double.NaN)
        //            {
        //                if (m_fRawData[j] > dblMax) dblMax = m_fRawData[j];
        //                if (m_fRawData[j] < dblMin) dblMin = m_fRawData[j];

        //                dblAcc += (decimal)m_fRawData[j];
        //                dblSum += (decimal)Math.Pow(m_fRawData[j], 2);

        //                dblAccTmp += (decimal)m_fRawData[j];
        //                m_iNCount++;
        //            }
        //        }

        //        if (m_iNCount > 0)
        //            dblTemp = dblAccTmp / m_iNCount;

        //        if (dblMin > 1000000 || m_fRawData.Count == 0)
        //        {
        //            m_dMIN = dblMin;
        //        }


        //        m_dSOS = (double)dblSum;

        //        m_iCount = m_fRawData.Count;
        //        m_dMIN = dblMin;
        //        m_dMAX = dblMax;

        //        m_dSUM = (double)dblAcc;

        //        if (m_iNCount > 0)
        //            m_dAVG = (double)dblAcc / m_iNCount;

        //        if (m_fRawData.Count < 2 || m_iNCount < 2)  //자료수 부족
        //        {
        //            m_dMedian = Calculate.Median(m_fRawData.ToArray());
        //            return;
        //        }

        //        m_dSIGMA = (double)Math.Sqrt((double)((dblSum / (decimal)(m_iNCount - 1)) - (decimal)(m_dAVG * m_dAVG * m_iNCount / (m_iNCount - 1))));
        //        m_dUCL = (double)m_dAVG + (3 * (double)m_dSIGMA);
        //        m_dLCL = (double)m_dAVG - (3 * (double)m_dSIGMA);

        //        if (!double.IsNaN(m_dUSL) && !double.IsNaN(m_dLSL))
        //        {
        //            m_dCP = (m_dUSL - m_dLSL) / (6 * (double)m_dSIGMA);
        //            m_dCPU = (m_dUSL - (double)m_dAVG) / (3 * (double)m_dSIGMA);
        //            m_dCPL = ((double)m_dAVG - m_dLSL) / (3 * (double)m_dSIGMA);
        //            dblM = Math.Round(((m_dUSL + m_dLSL) / 2), 4);
        //            m_dK = Math.Abs(dblM - (double)m_dAVG) / ((m_dUSL - m_dLSL) / 2);
        //            m_dTARGET = (m_dUSL + m_dLSL) / 2;

        //            if (dblM == m_dTARGET) //목표치가 스펙의 중심일 때
        //            {
        //                if (m_dCPL < m_dCPU)
        //                {
        //                    m_dCPK = m_dCPL;
        //                }
        //                else
        //                {
        //                    m_dCPK = m_dCPU;
        //                }
        //            }
        //            else
        //            {
        //                dblSigma_ = (decimal)Math.Sqrt(Math.Pow((double)m_dSIGMA, 2) + Math.Pow(((double)m_dAVG - m_dTARGET), 2));
        //                m_dCPM = (m_dUSL - m_dLSL) / (6 * (double)dblSigma_);
        //            }
        //        }

        //        m_dMedian = Calculate.Median(m_fRawData.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        m_bCalculated = true;
        //    }
        //}
    }
}
