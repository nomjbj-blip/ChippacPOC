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

    }
}
