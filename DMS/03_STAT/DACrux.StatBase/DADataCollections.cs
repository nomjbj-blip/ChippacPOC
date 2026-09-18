using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.StatBase
{
    public class DADataCollections
    {
        public delegate void AddData(int count);
        public event AddData OnAddData = null;
        private List<string> m_DataID = null;
        private List<DACrux.StatBase.DAData> m_Datas = null;
        private List<int> m_idxVisible = null;

        bool m_bCalculated = false;

        int m_iCount = 0;
        int m_iNCount = 0;

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


        double m_dUSL = double.NaN;
        double m_dTARGET = double.NaN;
        double m_dLSL = double.NaN;

        double m_dRMIN = double.NaN;
        double m_dRMAX = double.NaN;
        double m_dRAVG = double.NaN;

        double m_d3SMIN = double.NaN;
        double m_d3SMAX = double.NaN;
        double m_d3SAVG = double.NaN;

        double m_dEWMA_M_MIN = double.NaN;
        double m_dEWMA_M_MAX = double.NaN;
        double m_dEWMA_M_AVG = double.NaN;

        double m_dEWMA_R_MIN = double.NaN;
        double m_dEWMA_R_MAX = double.NaN;
        double m_dEWMA_R_AVG = double.NaN;

        double m_dEWMA_S_MIN = double.NaN;
        double m_dEWMA_S_MAX = double.NaN;
        double m_dEWMA_S_AVG = double.NaN;

        double m_dMS_MIN = double.NaN;
        double m_dMS_MAX = double.NaN;
        double m_dMS_AVG = double.NaN;

        double m_dMA_MIN = double.NaN;
        double m_dMA_MAX = double.NaN;
        double m_dMA_AVG = double.NaN;


        double m_dMAXUSL = double.MinValue;
        double m_dMINLSL = double.MaxValue;

        double m_dUCLMAX = double.MinValue;
        double m_dLCLMIN = double.MaxValue;

        double m_dR_UCL = double.MinValue;
        double m_dR_LCL = double.MaxValue;

        double m_d3S_UCL = double.MinValue;
        double m_d3S_LCL = double.MaxValue;

        double m_dEWMA_M_UCL = double.MinValue;
        double m_dEWMA_M_LCL = double.MaxValue;

        double m_dEWMA_R_UCL = double.MinValue;
        double m_dEWMA_R_LCL = double.MaxValue;

        double m_dEWMA_S_UCL = double.MinValue;
        double m_dEWMA_S_LCL = double.MaxValue;

        double m_dMS_UCL = double.MinValue;
        double m_dMS_LCL = double.MaxValue;

        double m_dMA_UCL = double.MinValue;
        double m_dMA_LCL = double.MaxValue;

        double m_dRAW_UCL = double.MinValue;
        double m_dRAW_LCL = double.MaxValue;


        public double R_UCL{ get{return m_dR_UCL;}}
        public double R_LCL{ get{return m_dR_LCL;}}

        public double S3_UCL{ get{return m_d3S_UCL;}}
        public double S3_LCL{ get{return m_d3S_LCL;}}

        public double EWMA_M_UCL{ get{return m_dEWMA_M_UCL;}}
        public double EWMA_M_LCL{ get{return m_dEWMA_M_LCL;}}

        public double EWMA_R_UCL{ get{return m_dEWMA_R_UCL;}}
        public double EWMA_R_LCL{ get{return m_dEWMA_R_LCL;}}

        public double EWMA_S_UCL{ get{return m_dEWMA_S_UCL;}}
        public double EWMA_S_LCL{ get{return m_dEWMA_S_LCL;}}

        public double MS_UCL{ get{return m_dMS_UCL;}}
        public double MS_LCL{ get{return m_dMS_LCL;}}

        public double MA_UCL{ get{return m_dMA_UCL;}}
        public double MA_LCL { get { return m_dMA_LCL; } }

        public double RAW_UCL { get { return m_dRAW_UCL; } }
        public double RAW_LCL { get { return m_dRAW_LCL; } }

        public DADataCollections()
        {
            m_DataID = new List<string>();
            m_Datas = new List<DACrux.StatBase.DAData>();
            m_bCalculated = false;
        }

        public void Add(DACrux.StatBase.DAData value)
        {
            m_bCalculated = false;
            m_Datas.Add(value);
            m_DataID.Add(value.DataLable);
            if (OnAddData != null) OnAddData(1);
        }

        public void Add(string dataid, DACrux.StatBase.DAData value)
        {
            m_bCalculated = false;
            m_Datas.Add(value);
            m_DataID.Add(dataid);
            if (OnAddData != null) OnAddData(1);
        }

        public void Clear()
        {
            m_bCalculated = false;
            m_Datas.Clear();
            m_DataID.Clear();
        }

        public bool Contains(DACrux.StatBase.DAData value)
        {
            return m_Datas.Contains(value);
        }

        public int IndexOf(DACrux.StatBase.DAData value)
        {
            return m_Datas.IndexOf((DACrux.StatBase.DAData)value);
        }

        public void Insert(int index, object value)
        {
            m_bCalculated = false;
            m_Datas.Insert(index, (DACrux.StatBase.DAData)value);
        }

        public void Remove(object value)
        {
            m_bCalculated = false;
            m_Datas.Remove((DACrux.StatBase.DAData)value);
        }

        public void RemoveAt(int index)
        {
            m_bCalculated = false;
            m_Datas.RemoveAt(index);
        }

        public DACrux.StatBase.DAData this[int index]
        {
            get
            {
                return m_Datas[index];
            }
            set
            {
                m_bCalculated = false;
                m_Datas[index] = value;
            }
        }

        public DACrux.StatBase.DAData this[string dataid]
        {
            get
            {
                int index = m_DataID.IndexOf(dataid);
                if (index < 0) return null;
                return m_Datas[index];
            }
            set
            {
                int index = m_DataID.IndexOf(dataid);
                if (index < 0)
                {
                    this.Add(dataid, value);
                }
                else
                {
                    m_bCalculated = false;
                    m_Datas[index] = value;
                }
            }
        }


        public void CopyTo(DACrux.StatBase.DAData[] array, int index)
        {
            m_Datas.CopyTo(array, index);
        }

        public int Count
        {
            get { return m_Datas.Count; }
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
            set
            {
                m_bCalculated = false;
                m_dUSL = value;
            }
        }

        public double LSL
        {
            get
            {
                return m_dLSL;
            }
            set
            {
                m_bCalculated = false;
                m_dLSL = value;
            }
        }

        public double Target
        {
            get
            {
                return m_dTARGET;
            }
            set
            {
                m_bCalculated = false;
                m_dTARGET = value;
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

        public double RMIN
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dRMIN;
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

        public double SMIN
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_d3SMIN;
            }
        }

        public double SMAX
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_d3SMAX;
            }
        }

        public double SAVG
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_d3SAVG;
            }
        }

        public double EWMA_M_MIN
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dEWMA_M_MIN;
            }
        }

        public double EWMA_M_MAX
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dEWMA_M_MAX;
            }
        }

        public double EWMA_M_AVG
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dEWMA_M_AVG;
            }
        }

        public double EWMA_R_MIN
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dEWMA_R_MIN;
            }
        }

        public double EWMA_R_MAX
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dEWMA_R_MAX;
            }
        }

        public double EWMA_R_AVG
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dEWMA_R_AVG;
            }
        }


        public double EWMA_S_MIN
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dEWMA_S_MIN;
            }
        }

        public double EWMA_S_MAX
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dEWMA_S_MAX;
            }
        }

        public double EWMA_S_AVG
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dEWMA_S_AVG;
            }
        }

        public double MS_MIN
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMS_MIN;
            }
        }

        public double MS_MAX
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMS_MAX;
            }
        }

        public double MS_AVG
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMS_AVG;
            }
        }

        public double MA_MIN
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMA_MIN;
            }
        }

        public double MA_MAX
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMA_MAX;
            }
        }

        public double MA_AVG
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_dMA_AVG;
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
                return m_idxVisible.Count;
            }
        }

        public List<int> VisibleDatas
        {
            get
            {
                if (!m_bCalculated) CalDescriptive();
                return m_idxVisible;
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

            m_dMAXUSL = double.MinValue;
            m_dMINLSL = double.MaxValue;

            m_bCalculated = false;
            m_idxVisible = new List<int>();
        }

        /// <summary>
        /// 통계량을 계산해주는 메소드(Target이 중심이 아닐경우 Cpm만 돌려줌.)
        /// 상세  설명: Data dt와 [USL, Target, LSL]string array을 받아서 [sigma, TotalCount , n, Min, Max, Avg, Cp, K,   Cpk, Cpu, Cpl, Cpm, Ucl, Lcl]string array 반환<br/>
        /// </summary>
        /// <param name="dr"> 2차원(직사각형)으로 된 그룹구분 없는 Data Rows</param>
        /// <param name="arrInput">[USL, Target, LSL]의 string Array</param>
        public void CalDescriptive(bool ReCal = false)
        {
            int iRowsCNT = 0;
            int iDataCNT = 0;
            int iTotalCnt = 0;

            string[] arrReturn = new string[14];

            decimal dblAcc = 0;


            //decimal dblSigma;

            //decimal dblAcc2 = 0;
            decimal dblSum2 = 0;
            int iSampleCNT = 0;

            double dblM;
            decimal dblSigma_;

            decimal dblSumM = 0;
            decimal dblSumR = 0;
            decimal dblSumS = 0;

            decimal dblSumEM = 0;
            decimal dblSumER = 0;
            decimal dblSumES = 0;

            decimal dblSumMS = 0;
            decimal dblSumMA = 0;

            try
            {
                if (m_bCalculated && !ReCal) return;
                InitResult();
                iRowsCNT = m_Datas.Count;

                m_dMAX = double.MinValue;
                m_dMIN = double.MaxValue;

                m_dRMAX = double.MinValue;
                m_dRMIN = double.MaxValue;

                m_d3SMAX = double.MinValue;
                m_d3SMIN = double.MaxValue;


                m_dEWMA_M_MAX = double.MinValue;
                m_dEWMA_M_MIN = double.MaxValue;

                m_dEWMA_R_MAX = double.MinValue;
                m_dEWMA_R_MIN = double.MaxValue;

                m_dEWMA_S_MAX = double.MinValue;
                m_dEWMA_S_MIN = double.MaxValue;

                m_dMS_MAX = double.MinValue;
                m_dMS_MIN = double.MaxValue;

                m_dMA_MAX = double.MinValue;
                m_dMA_MIN = double.MaxValue;


                int idx = 0;
                m_idxVisible.Clear();
                foreach (DAData oData in m_Datas)
                {
                    if (oData.Visible == false)
                    {
                        idx++;
                        continue;
                    }
                    m_idxVisible.Add(idx);
                    if (oData.DataCount == 0) continue;
                    if (oData.MAX > m_dMAX) m_dMAX = oData.MAX;
                    if (oData.MIN < m_dMIN) m_dMIN = oData.MIN;

                    if (oData.RANGE > m_dRMAX) m_dRMAX = oData.RANGE;
                    if (oData.RANGE < m_dRMIN) m_dRMIN = oData.RANGE;

                    if (oData.SIGMA > m_d3SMAX) m_d3SMAX = oData.SIGMA;
                    if (oData.SIGMA < m_d3SMIN) m_d3SMIN = oData.SIGMA;

                    if (oData.EWMA_M > m_dEWMA_M_MAX) m_dEWMA_M_MAX = oData.EWMA_M;
                    if (oData.EWMA_M < m_dEWMA_M_MIN) m_dEWMA_M_MIN = oData.EWMA_M;

                    if (oData.EWMA_R > m_dEWMA_R_MAX) m_dEWMA_R_MAX = oData.EWMA_R;
                    if (oData.EWMA_R < m_dEWMA_R_MIN) m_dEWMA_R_MIN = oData.EWMA_R;

                    if (oData.EWMA_S > m_dEWMA_S_MAX) m_dEWMA_S_MAX = oData.EWMA_S;
                    if (oData.EWMA_S < m_dEWMA_S_MIN) m_dEWMA_S_MIN = oData.EWMA_S;

                    if (oData.MS > m_dMS_MAX) m_dMS_MAX = oData.MS;
                    if (oData.MS < m_dMS_MIN) m_dMS_MIN = oData.MS;

                    if (oData.MA > m_dMA_MAX) m_dMA_MAX = oData.MA;
                    if (oData.MA < m_dMA_MIN) m_dMA_MIN = oData.MA;

                    if(!double.IsNaN(oData.SUM)) dblAcc += (decimal)oData.SUM;
                    if (!double.IsNaN(oData.MEAN)) dblSumM += (decimal)oData.MEAN;
                    if (!double.IsNaN(oData.RANGE)) dblSumR += (decimal)oData.RANGE;
                    if (!double.IsNaN(oData.SIGMA)) dblSumS += (decimal)oData.SIGMA;

                    if(!double.IsNaN(oData.EWMA_M)) dblSumEM += (decimal)oData.EWMA_M;
                    if (!double.IsNaN(oData.EWMA_R)) dblSumER += (decimal)oData.EWMA_R;
                    if (!double.IsNaN(oData.EWMA_S)) dblSumES += (decimal)oData.EWMA_S;

                    if (!double.IsNaN(oData.MS)) dblSumMS += (decimal)oData.MS;
                    if (!double.IsNaN(oData.MA)) dblSumMA += (decimal)oData.MA;

                    if (!double.IsNaN(oData.MEAN))  dblSum2 += (decimal)Math.Pow(System.Convert.ToDouble(oData.MEAN), 2);

                    iDataCNT += oData.DataCount;
                    iTotalCnt += oData.DataCount;
                    if (oData.DataCount > iSampleCNT) iSampleCNT = oData.DataCount;

                    if (!double.IsNaN(oData.LSL) && oData.LSL < m_dMINLSL) m_dMINLSL = oData.LSL;
                    if (!double.IsNaN(oData.USL) && oData.USL > m_dMAXUSL) m_dMAXUSL = oData.USL;

                    /// CONTROL MIN, MAX
                    /// 

                    if (!double.IsNaN(oData.UCL) && oData.UCL > m_dUCLMAX) m_dUCLMAX = oData.UCL;
                    if (!double.IsNaN(oData.LCL) && oData.LCL < m_dLCLMIN) m_dLCLMIN = oData.LCL;

                    if (!double.IsNaN(oData.RangeUCL) && oData.RangeUCL > m_dR_UCL) m_dR_UCL = oData.RangeUCL;
                    if (!double.IsNaN(oData.RangeLCL) && oData.RangeLCL < m_dR_LCL) m_dR_LCL = oData.RangeLCL;

                    if (!double.IsNaN(oData.SigmaUCL) && oData.SigmaUCL > m_d3S_UCL) m_d3S_UCL = oData.SigmaUCL;
                    if (!double.IsNaN(oData.SigmaLCL) && oData.SigmaLCL < m_d3S_LCL) m_d3S_LCL = oData.SigmaLCL;

                    if (!double.IsNaN(oData.EWMA_M_UCL) && oData.EWMA_M_UCL > m_dEWMA_M_UCL) m_dEWMA_M_UCL = oData.EWMA_M_UCL;
                    if (!double.IsNaN(oData.EWMA_M_LCL) && oData.EWMA_M_LCL < m_dEWMA_M_LCL) m_dEWMA_M_LCL = oData.EWMA_M_LCL;

                    if (!double.IsNaN(oData.EWMA_R_UCL) && oData.EWMA_R_UCL > m_dEWMA_R_UCL) m_dEWMA_R_UCL = oData.EWMA_R_UCL;
                    if (!double.IsNaN(oData.EWMA_R_LCL) && oData.EWMA_R_LCL < m_dEWMA_R_LCL) m_dEWMA_R_LCL = oData.EWMA_R_LCL;

                    if (!double.IsNaN(oData.EWMA_S_UCL) && oData.EWMA_S_UCL > m_dEWMA_S_UCL) m_dEWMA_S_UCL = oData.EWMA_S_UCL;
                    if (!double.IsNaN(oData.EWMA_S_LCL) && oData.EWMA_S_LCL < m_dEWMA_S_LCL) m_dEWMA_S_LCL = oData.EWMA_S_LCL;


                    if (!double.IsNaN(oData.MS_UCL) && oData.MS_UCL > m_dMS_UCL) m_dMS_UCL = oData.MS_UCL;
                    if (!double.IsNaN(oData.MS_LCL) && oData.MS_LCL < m_dMS_LCL) m_dMS_LCL = oData.MS_LCL;

     
                    if (!double.IsNaN(oData.MA_UCL) && oData.MA_UCL > m_dMA_UCL) m_dMA_UCL = oData.MA_UCL;
                    if (!double.IsNaN(oData.MA_LCL) && oData.MA_LCL < m_dMA_LCL) m_dMA_LCL = oData.MA_LCL;

                    if (!double.IsNaN(oData.RAW_UCL) && oData.RAW_UCL > m_dRAW_UCL) m_dRAW_UCL = oData.RAW_UCL;
                    if (!double.IsNaN(oData.RAW_LCL) && oData.RAW_LCL < m_dRAW_LCL) m_dRAW_LCL = oData.RAW_LCL;


                    idx++;
                }

                m_iCount = iTotalCnt;
                m_iNCount = iDataCNT;

                if(iDataCNT > 0)
                m_dAVG = (double)(dblAcc / iDataCNT);

                if (iRowsCNT < 2)  //자료수 부족
                {
                    return;
                }
                if (iDataCNT < 2) //자료수 부족
                {
                    return;
                }


                m_dSIGMA = Math.Sqrt(((double)(dblSum2 / (iRowsCNT - 1)) - (double)(m_dAVG * m_dAVG * iRowsCNT / (iRowsCNT - 1))));
                m_dSIGMA2 = (double)(m_dSIGMA / Math.Sqrt(iSampleCNT));


                m_dUCL = (double)(m_dAVG + (double)(3 * m_dSIGMA2));
                m_dLCL = (double)(m_dAVG - (double)(3 * m_dSIGMA2));


                m_dRAVG = (double)dblSumM / iRowsCNT;
                m_d3SAVG = (double)dblSumS / iRowsCNT;

                m_dEWMA_M_AVG = (double)dblSumEM / iRowsCNT;
                m_dEWMA_R_AVG = (double)dblSumER / iRowsCNT;
                m_dEWMA_S_AVG = (double)dblSumES / iRowsCNT;

                m_dMS_AVG = (double)dblSumMS / iRowsCNT;
                m_dMA_AVG = (double)dblSumMA / iRowsCNT;

                if (!double.IsNaN(m_dUSL) && !double.IsNaN(m_dLSL)) // 상한 하한 모두 존재
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

        public DACrux.StatBase.DAData[] ToArray()
        {
            return m_Datas.ToArray();
        }
    }
}
