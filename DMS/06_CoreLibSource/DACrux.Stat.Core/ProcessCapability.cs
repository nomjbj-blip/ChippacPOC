/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : ProcessCapability.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2015.02.10
--  Description     : 공정능력 지수 계산
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


// 2 sample t-test에서 Welch t-test의 분산과 자유도를 사용하지 않고 pooled variance를 사용하기 위해 등분산 가정을 하게 되는데, 
// (n-1)sigma^2 / s^2 ~ chisquare (n) 이고, 카이제곱 확률변수의 비는 F 분포를 따릅니다. 
// 따라서 sigma1^2 / sigma2^2 ~ F (nu1, nu2) (단, nu1, nu2는 자유도) 로 표현할 수 있고, 
// 이때 표본으로부터 구해진 F통계량은 
// 귀무가설 H0 : sigma1^2=sigma2^2   ( (sigma1^2 / sigma2^2 )= 1 )
// 대립가설 H1 : not H0 
// 의 가설을 검정하는 데에 사용될 수 있습니다.
// F통계량의 값이 클수록 두 표본분산의 비가 차이가 난다고 볼 수 있으므로, F값이 유의하게 크게 나올 경우 p-value가 유의수준 (일반적으로 5%=0.05) 작게 나와 귀무가설을 기각하게 되고, 등분산 가정이 성립하지 않는다는 결론을 내립니다. 

    
    public class ProcessCapability
    {
        /// <summary>
        /// UCL / LCL
        /// </summary>
        //double m_dUCL = double.NaN;
        //double m_dLCL = double.NaN;
        //double m_dCL = double.NaN;
        //int m_iOOC = 0;


        List<double[]> m_dRawData = null;
        List<double> m_dRawData_XBar = null;
        List<double> m_dRawData_Clean = null;

        #region 클래스 변수

        /// <summary>
        /// USL / LSL 
        /// </summary>
        double m_dUSL = double.NaN;
        double m_dLSL = double.NaN;
        double m_dTAR = double.NaN;

        #region 결과값
        /// <summary>
        /// 평균
        /// </summary>
        double m_dMEAN = double.NaN;
        /// <summary>
        /// Sigma
        /// </summary>
        double m_dSIGMA = double.NaN;
        /// <summary>
        /// CP
        /// </summary>
        double m_dCP = double.NaN;
        /// <summary>
        /// K
        /// </summary>
        double m_dK = double.NaN;
        /// <summary>
        /// CPU
        /// </summary>
        double m_dCPU = double.NaN;
        /// <summary>
        /// CPL
        /// </summary>
        double m_dCPL = double.NaN;
        /// <summary>
        /// CPK
        /// </summary>
        double m_dCPK = double.NaN;
        /// <summary>
        /// CPM
        /// </summary>
        double m_dCPM = double.NaN;


        #endregion

        #region DataSource & Etc

        /// <summary>
        /// DataSource
        /// </summary>
        DataTable m_dtData;

        /// <summary>
        /// Value Indexes
        /// </summary>
        int[] m_iValueColumns = null;

        /// <summary>
        /// 소수점 표시 자리수
        /// </summary>
        int m_iDecimalPlaces = 4;

        /// <summary>
        /// 예외 발생 유무
        /// </summary>
        bool m_Exception = true;

        bool m_IsSameSampleSize = true;

        /// <summary>
        /// 모든 Raw Data전체 (군내)
        /// </summary>
        double m_dSum = double.NaN;
        double m_dStd = double.NaN;
        double m_dVar = double.NaN;
        double m_dSumOfSq = double.NaN;
        double m_dNSumOfSq = double.NaN;
        double m_dMin = double.NaN;
        double m_dMax = double.NaN;
        double m_dMedian = double.NaN;
        double m_dQ1 = double.NaN;
        double m_dQ3 = double.NaN;
        int m_dCntAll = 0;
        int m_dCntAva = 0;

        double m_dSP = double.NaN;
        double m_dSP_Unbiased = double.NaN;
        int m_iDegree = 0;

        public double SUM { get { return m_dSum; } }
        public double AVG { get { return m_dMEAN; } }
        public double STD { get { return m_dStd; } }
        public double VAR { get { return m_dVar; } }
        public double SOS { get { return m_dSumOfSq; } }
        public double NOS { get { return m_dNSumOfSq; } }
        public double MIN { get { return m_dMin; } }
        public double MAX { get { return m_dMax; } }
        public double MEDIAN { get { return m_dMedian; } }

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


        /// <summary>
        /// 각 군의 평균간 (군간)
        /// </summary>
        double m_dpSum = double.NaN;
        double m_dpAvg = double.NaN;
        double m_dpStd = double.NaN;
        double m_dpVar = double.NaN;
        double m_dpSumOfSq = double.NaN;
        double m_dpNSumOfSq = double.NaN;
        double m_dpMin = double.NaN;
        double m_dpMax = double.NaN;
        double m_dpMedian = double.NaN;
        double m_dpQ1 = double.NaN;
        double m_dpQ3 = double.NaN;
        int m_dpCntAll = 0;
        int m_dpCntAva = 0;

        public double P_SUM { get { return m_dpSum; } }
        public double P_AVG { get { return m_dpAvg; } }
        public double P_STD { get { return m_dpStd; } }
        public double P_VAR { get { return m_dpVar; } }
        public double P_SOS { get { return m_dpSumOfSq; } }
        public double P_NOS { get { return m_dpNSumOfSq; } }
        public double P_MIN { get { return m_dpMin; } }
        public double P_MAX { get { return m_dpMax; } }
        public double P_MEDIAN { get { return m_dpMedian; } }

        /// <summary>
        /// Min
        /// </summary>
        public double P_Q0 { get { return m_dpMin; } }

        public double P_Q1 { get { return m_dpQ1; } }

        /// <summary>
        /// Median
        /// </summary>
        public double P_Q2 { get { return m_dpMedian; } }

        public double P_Q3 { get { return m_dpQ3; } }

        /// <summary>
        /// Max
        /// </summary>
        public double P_Q4 { get { return m_dpMax; } }

        public double P_ALLCount { get { return m_dpCntAll; } }
        public double P_AVACount { get { return m_dpCntAva; } }
        #endregion

        #endregion


        #region 생성자
        public ProcessCapability(DataTable dt, int[] ValueColumnIndexes, string USL, string TAR, string LSL, string Sigma = null)
        {
            try
            {
                if (!double.TryParse(USL, out m_dUSL))
                    m_dUSL = double.NaN;
                if (!double.TryParse(TAR, out m_dTAR))
                    m_dTAR = double.NaN;
                if (!double.TryParse(LSL, out m_dLSL))
                    m_dLSL = double.NaN;

                m_iValueColumns = ValueColumnIndexes;


                m_dRawData = new List<double[]>();          // Null 포함한 전체값을 각 군별로 저장
                m_dRawData_Clean = new List<double>();      // Null 을 제외한 전체값을 전체로 저장

                m_dRawData_XBar = new List<double>();       // Null 제외한 군내 평균 List 저장


                double[] tmp = new double[m_iValueColumns.Length];
                /// Sample Count가 2보다 작을때 관리도용 계수 사용 못함
                if (m_iValueColumns.Length < 2) m_IsSameSampleSize = false;
                m_iDegree = 0;                              // 관측치 누적 합
                for (int ir = 0; ir < dt.Rows.Count; ir++)
                {
                    double tmpXBar = double.NaN;
                    double tmpSubSum = double.NaN;
                    int tmpSubCnt = 0;

                    for (int idxCol = 0; idxCol < m_iValueColumns.Length; idxCol++)
                    {
                        int ic = m_iValueColumns[idxCol];
                        if (double.TryParse(dt.Rows[ir][ic].ToString(), out tmp[idxCol]) == false)
                        {
                            /// 1개의 Data라도 빈값이 있다면 Sample Size가 동일하지 않은것임
                            m_IsSameSampleSize = false;
                            tmp[idxCol] = double.NaN;
                        }
                        else
                        {
                            /// Sub Sum
                            if (double.IsNaN(tmpSubSum)) tmpSubSum = 0;
                            tmpSubSum += tmp[idxCol];
                            tmpSubCnt++;


                            /// All Data Sum
                            if (double.IsNaN(m_dSumOfSq)) m_dSumOfSq = 0;
                            if (double.IsNaN(m_dNSumOfSq)) m_dNSumOfSq = 0;
                            if (double.IsNaN(m_dSum)) m_dSum = 0;
                            m_dCntAva++;
                            m_dSumOfSq += Math.Pow(tmp[idxCol], 2);
                            m_dNSumOfSq += 1 / Math.Pow(tmp[idxCol], 2);
                            m_dSum += tmp[idxCol];

                            m_dRawData_Clean.Add(tmp[idxCol]);          // 전체 Data의 Array
                        }
                    }
                    m_dRawData.Add(tmp);                                // 모든 Raw Data 집합의 Array

                    tmpXBar = tmpSubSum/tmpSubCnt;
                    m_dRawData_XBar.Add(tmpXBar);                       // 각 구별 평균 값
                    m_iDegree += (tmpSubCnt - 1);                       // 관측치 누적 합

                    if (double.IsNaN(m_dpSumOfSq)) m_dpSumOfSq = 0;
                    if (double.IsNaN(m_dpNSumOfSq)) m_dpNSumOfSq = 0;
                    if (double.IsNaN(m_dpSum)) m_dpSum = 0;
                    m_dpCntAva++;
                    m_dpSumOfSq += Math.Pow(tmpXBar, 2);
                    m_dpNSumOfSq += 1 / Math.Pow(tmpXBar, 2);
                    m_dpSum += tmpXBar;
                }


                //1. Clean Data Count
                m_dCntAva = m_dRawData_Clean.Count;
                m_dpCntAva = m_dRawData_XBar.Count;

                //2. 평균
                m_dMEAN = m_dSum / (double)m_dCntAva;
                m_dpAvg = m_dpSum / (double)m_dpCntAva;

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
                m_dVar = (m_dSumOfSq / m_dCntAva) - Math.Pow(m_dMEAN, 2);
                m_dpVar = (m_dpSumOfSq / m_dpCntAva) - Math.Pow(m_dpAvg, 2);

                //4. 표준편차
                m_dStd = Math.Sqrt(m_dVar);
                m_dpStd = Math.Sqrt(m_dpVar);

                m_dSP = m_dSumOfSq / m_iDegree;
                m_dSP_Unbiased = m_dSP / GetC4(1 + m_iDegree);

                //5. 중심값
                m_dMedian = Descriptive.Median(m_dRawData_Clean.ToArray(), ref m_dMax, ref m_dMin);
                m_dpMedian = Descriptive.Median(m_dRawData_XBar.ToArray(), ref m_dpMax, ref m_dpMin);


                //ALL
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

                //XBAR
                int pcount = -1;
                if ((m_dpCntAva % 2) == 0)
                {
                    pcount = (int)m_dpCntAva / 2;
                }
                else
                {
                    // 홀수개였을때 중앙값은 아무데도 속하지 않음
                    pcount = (int)m_dpCntAva / 2;
                }

                //6. Q1 (All Raw Data)
                double[] tmpQ1 = new double[count];
                Array.Copy(m_dRawData_Clean.ToArray(), tmpQ1, count);
                m_dMedian = Descriptive.Median(tmpQ1);

                //6. Q1 (XBAR)
                double[] tmppQ1 = new double[pcount];
                Array.Copy(m_dRawData_XBar.ToArray(), tmppQ1, pcount);
                m_dpMedian = Descriptive.Median(tmppQ1);

                //7. Q3 (All Raw Data)
                double[] tmpQ3 = new double[count];
                Array.Copy(m_dRawData_Clean.ToArray(), count + (m_dCntAva % 2), tmpQ3, 0, count);
                m_dMedian = Descriptive.Median(tmpQ3);

                //7. Q3 (XBAR)
                double[] tmppQ3 = new double[pcount];
                Array.Copy(m_dRawData_XBar.ToArray(), pcount + (m_dpCntAva % 2), tmppQ3, 0, pcount);
                m_dpMedian = Descriptive.Median(tmppQ3);

                Process();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 접근자

        #region Is Exception

        public bool IsException
        {
            get
            {
                return m_Exception;
            }
            set
            {
                m_Exception = value;
            }
        }
        #endregion

        #region 소수점 자리수
        /// <summary>
        /// 소수점 자리수
        /// </summary>
        public int DecimalPlaces
        {
            set
            {
                m_iDecimalPlaces = value;
            }
        }
        #endregion

        #region string으로 된 Spec
        /// <summary>
        /// USL, TARGET, LSL순서로 된 string Array, 값이 없는것은 string.Empty로 입력
        /// </summary>
        public string[] Spec
        {
            set
            {
                string[] arrSpec = value;
                if (arrSpec[0].Trim() == string.Empty)
                    m_dUSL = double.NaN;
                else
                {
                    if (!double.TryParse(arrSpec[0].Trim(), out m_dUSL))
                    {
                        m_dUSL = double.NaN;
                    }
                }
                if (arrSpec[1].Trim() == string.Empty)
                    m_dTAR = double.NaN;
                else
                {
                    if (!double.TryParse(arrSpec[1].Trim(), out m_dTAR))
                    {
                        m_dTAR = double.NaN;
                    }
                }
                if (arrSpec[2].Trim() == string.Empty)
                    m_dLSL = double.NaN;
                else
                {
                    if (!double.TryParse(arrSpec[2].Trim(), out m_dLSL))
                    {
                        m_dLSL = double.NaN;
                    }
                }
            }
            get
            {
                string[] arrSpec = new string[3];
                if (double.IsNaN(m_dUSL))
                    arrSpec[0] = string.Empty;
                else
                    arrSpec[0] = m_dUSL.ToString();
                if (double.IsNaN(m_dTAR))
                    arrSpec[1] = string.Empty;
                else
                    arrSpec[1] = m_dTAR.ToString();
                if (double.IsNaN(m_dLSL))
                    arrSpec[2] = string.Empty;
                else
                    arrSpec[2] = m_dLSL.ToString();
                return arrSpec;
            }
        }
        public string USLstring
        {
            set
            {
                if (value.Trim() == string.Empty)
                    m_dUSL = double.NaN;
                else
                {
                    if (!double.TryParse(value.Trim(), out m_dUSL))
                    {
                        m_dUSL = double.NaN;
                    }
                }
            }
            get
            {
                if (double.IsNaN(m_dUSL))
                    return string.Empty;
                else
                    return m_dUSL.ToString();
            }
        }
        public string TARGETstring
        {
            set
            {
                if (value.Trim() == string.Empty)
                    m_dTAR = double.NaN;
                else
                {
                    if (!double.TryParse(value.Trim(), out m_dTAR))
                    {
                        m_dTAR = double.NaN;
                    }
                }
            }
            get
            {
                if (double.IsNaN(m_dTAR))
                    return string.Empty;
                else
                    return m_dTAR.ToString();
            }
        }
        public string LSLstring
        {
            set
            {
                if (value.Trim() == string.Empty)
                    m_dLSL = double.NaN;
                else
                {
                    if (!double.TryParse(value.Trim(), out m_dLSL))
                    {
                        m_dLSL = double.NaN;
                    }
                }
            }
            get
            {
                if (double.IsNaN(m_dLSL))
                    return string.Empty;
                else
                    return m_dLSL.ToString();
            }
        }
        #endregion

        #region double로 된 Spec
        public double USL
        {
            set
            {
                m_dUSL = value;
            }
            get
            {
                return m_dUSL;
            }
        }
        public double TARGET
        {
            set
            {
                m_dTAR = value;
            }
            get
            {
                return m_dTAR;
            }
        }
        public double LSL
        {
            set
            {
                m_dLSL = value;
            }
            get
            {
                return m_dLSL;
            }
        }
        #endregion

        #region DataSource

        public DataTable DataSource
        {
            set
            {
                //m_dtData = value.Copy();
                m_dtData = value;
            }
            get
            {
                //return m_dtData.Copy();
                return m_dtData;
            }
        }

        /// <summary>
        /// DataSource에서 Value가 존재하는 Column의 Index
        /// </summary>
        public int[] ValueColumns
        {
            set
            {
                m_iValueColumns = value;
            }
            get
            {
                return m_iValueColumns;
            }
        }

        #endregion

        #region CP
        public string CPstring
        {
            get
            {
                if (double.IsNaN(m_dCP))
                    return string.Empty;
                else
                    return ((double)Math.Round(m_dCP, m_iDecimalPlaces)).ToString();
            }
        }
        public double CP
        {
            get
            {
                return m_dCP;
            }
        }
        #endregion

        #region CPK
        public string CPKstring
        {
            get
            {
                if (double.IsNaN(m_dCPK))
                    return string.Empty;
                else
                    return ((double)Math.Round(m_dCPK, m_iDecimalPlaces)).ToString();
            }
        }

        public double CPK
        {
            get
            {
                return m_dCPK;
            }
        }
        #endregion

        #region CPU
        public string CPUstring
        {
            get
            {
                if (double.IsNaN(m_dCPU))
                    return string.Empty;
                else
                    return ((double)Math.Round(m_dCPU, m_iDecimalPlaces)).ToString();
            }
        }
        public double Cpu
        {
            get
            {
                return m_dCPU;
            }
        }
        #endregion

        #region CPL
        public string CPLstring
        {
            get
            {
                if (double.IsNaN(m_dCPL))
                    return string.Empty;
                else
                    return ((double)Math.Round(m_dCPL, m_iDecimalPlaces)).ToString();
            }
        }
        public double Cpl
        {
            get
            {
                return m_dCPL;
            }
        }
        #endregion

        #region K
        public string Kstring
        {
            get
            {
                if (double.IsNaN(m_dK))
                    return string.Empty;
                else
                    return ((double)Math.Round(m_dK, m_iDecimalPlaces)).ToString();
            }
        }
        public double K
        {
            get
            {
                return m_dK;
            }
        }
        #endregion

        #region CPM
        public string CPMstring
        {
            get
            {
                if (double.IsNaN(m_dCPM))
                    return string.Empty;
                else
                    return ((double)Math.Round(m_dCPM, m_iDecimalPlaces)).ToString();
            }
        }
        public double Cpm
        {
            get
            {
                return m_dCPM;
            }
        }
        #endregion

        #endregion


        private void Process()
        {

            double dblM;
            try
            {
                if (m_IsSameSampleSize)
                {
                    m_dSIGMA = GetSigmaWithin_Pooled(m_dtData, m_iValueColumns, false);
                }
                else
                {
                    m_dSIGMA = m_dStd;
                }


                if (m_dSIGMA == 0)
                {
                    if (IsException)
                        throw new Exception("Cannot proceed calculation, because the deviation value is 0. All values are same.");
                }
                else if (double.IsNaN(m_dSIGMA))
                {
                    if (IsException)
                        throw new Exception("Cannot proceed calculation, because the deviation value is 0. All values are same.");
                }
                else
                {
                    if (!double.IsNaN(m_dUSL) && !double.IsNaN(m_dLSL)) // 상한과 하한이 존재
                    {
                        dblM = (m_dUSL + m_dLSL) / 2;
                        if (m_dUSL > m_dLSL)
                        {
                            m_dCP = GetCp(m_dUSL, m_dLSL, 6, m_dSIGMA);
                            m_dCPU = GetCPU(m_dMEAN, m_dUSL, 6, m_dSIGMA);
                            m_dCPL = GetCPL(m_dMEAN, m_dLSL, 6, m_dSIGMA);
                            m_dK = Math.Abs(dblM - m_dMEAN) / ((m_dUSL - m_dLSL) / 2);
                        }
                        else
                        {
                            if (IsException)
                                throw new Exception("Wrong spec value. Upper limit must be bigger than lower limit.");
                        }
                    }
                    else if (!double.IsNaN(m_dUSL) && double.IsNaN(m_dLSL)) // USL만 존재하는 경우
                    {
                        m_dCPU = GetCPU(m_dMEAN, m_dUSL, 6, m_dSIGMA);
                    }
                    else if (double.IsNaN(m_dUSL) && !double.IsNaN(m_dLSL)) // LSL만 존재하는 경우
                    {
                        m_dCPL = GetCPL(m_dMEAN, m_dLSL, 6, m_dSIGMA);
                    }
                    else
                    {
                        return;
                    }
                    m_dCPK = GetCpk(m_dCPL, m_dCPU);
                    if (!double.IsNaN(m_dTAR)) //목표치가 있을 경우
                        m_dCPM = GetCpm(m_dLSL, m_dUSL, m_dTAR, 6, m_dRawData_Clean);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [ CAPABILITY ]
        private bool IsSameSampleSize(DataTable dt, int[] arrColumns)
        {
            int iRowCnt;
            int iColCnt;
            int iFirstCnt;
            int iCnt;
            try
            {
                iRowCnt = dt.Rows.Count;
                iColCnt = arrColumns.Length;

                if (iRowCnt < 2 || iColCnt < 2 || dt.Columns.Count < 2)
                {
                    return false;
                }
                iFirstCnt = 0;
                for (int j = 0; j < iColCnt; j++)  //첫번째 줄의 sample size
                {
                    if (dt.Rows[0][arrColumns[j]] != DBNull.Value && dt.Rows[0][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[0][arrColumns[j]])))
                    {
                        iFirstCnt++;
                    }
                }
                if (iFirstCnt < 2)
                {
                    return false;
                }

                for (int i = 1; i < iRowCnt; i++)
                {
                    iCnt = 0;
                    for (int j = 0; j < iColCnt; j++)
                    {
                        if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                        {
                            iCnt++;
                        }
                    }
                    if (iFirstCnt != iCnt)
                    {
                        return false;
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Pooled Within Std
        private double GetSigmaWithin_Pooled(DataTable dt, int[] arrColumns, bool Unbiased)
        {
            double dblSp;
            double dblC4d;
            int iRowCnt;
            int iColCnt;

            try
            {
                if (dt == null)
                {
                    if (m_Exception)
                        throw new ArgumentException(ErrorNullParameter("dt"));
                    else
                        return double.NaN;
                }
                if (arrColumns == null)
                {
                    if (m_Exception)
                        throw new ArgumentException(ErrorNullParameter("arrColumns"));
                    else
                        return double.NaN;
                }

                iColCnt = arrColumns.Length;
                iRowCnt = dt.Rows.Count;
                if (iColCnt < 2)
                {
                    if (m_Exception)
                        throw new ArgumentException(ErrorWrongParameter("arrColumns"));
                    else
                        return double.NaN;
                }

                if (iRowCnt == 0)
                {
                    if (m_Exception)
                        throw new ArgumentException(ErrorWrongParameter("dt"));
                    else
                        return double.NaN;
                }

                dblSp = Math.Sqrt(m_dpSumOfSq / (m_dRawData_XBar.Count -1));
                if (Unbiased)
                {
                    dblC4d = GetC4(m_dRawData_XBar.Count);
                    return dblSp / dblC4d;
                }
                else
                    return dblSp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private string ErrorNotEqualParameterLength(string strParameter1, string strParameter2)
        {
            try
            {
                return string.Format("Length of {0} doesn't equal length of {1}.", strParameter1, strParameter2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ErrorNullParameter(string strParameter)
        {
            try
            {
                if (strParameter == string.Empty)
                    strParameter = "parameter";

                return string.Format("Argument({0}) is null.", strParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ErrorWrongParameter(string strParameter)
        {
            try
            {
                if (strParameter == string.Empty)
                    strParameter = "parameter";

                return string.Format("Argument({0}) is wrong.", strParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCp(double USL, double LSL, double Ksigma, double Sigma)
        {
            try
            {
                return (USL - LSL) / (Ksigma * Sigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCPL(double Mean, double LSL, double Ksigma, double Sigma)
        {
            try
            {
                return (Mean - LSL) / (Ksigma / 2 * Sigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCPU(double Mean, double USL, double Ksigma, double Sigma)
        {
            try
            {
                return (USL - Mean) / (Ksigma / 2 * Sigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCpk(double CPL, double CPU)
        {
            try
            {
                if (double.IsNaN(CPU) && double.IsNaN(CPL))
                {
                    if (m_Exception)
                        throw new ArgumentException("Argument(Cpl, Cpu) is  null.");
                    else
                        return double.NaN;
                }
                if (double.IsNaN(CPU))
                    return CPL;
                if (double.IsNaN(CPL))
                    return CPU;
                else
                    return Math.Min(CPU, CPL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCCpk(double LSL, double USL, double Target, double Mean, double Ksigma, double Sigma)
        {
            double Mu;
            try
            {
                if (double.IsNaN(USL) && double.IsNaN(LSL))
                {
                    if (m_Exception)
                        throw new ArgumentException("Argument(USL, LSL) is double.NaN");
                    else
                        return double.NaN;
                }
                if (double.IsNaN(Ksigma))
                {
                    if (m_Exception)
                        throw new ArgumentException("Argument(Ksigma) is double.NaN");
                    else
                        return double.NaN;
                }

                if (double.IsNaN(Sigma))
                {
                    if (m_Exception)
                        throw new ArgumentException("Argument(Sigma) is double.NaN");
                    else
                        return double.NaN;
                }


                if (!double.IsNaN(Target))
                    Mu = Target;
                else
                {
                    if (!double.IsNaN(USL) && !double.IsNaN(LSL))
                        Mu = (USL + LSL) / 2;
                    else
                    {
                        if (!double.IsNaN(Mean))
                            Mu = Mean;
                        else
                        {
                            if (m_Exception)
                                throw new ArgumentException("Argument(Mean) is double.NaN");
                            else
                                return double.NaN;
                        }

                    }
                }

                if (!double.IsNaN(USL) && !double.IsNaN(LSL))
                    return Math.Min(USL - Mu, Mu - LSL) / (Ksigma / 2 * Sigma);
                else if (double.IsNaN(USL) && !double.IsNaN(LSL))
                    return (Mu - LSL) / (Ksigma / 2 * Sigma);
                else //if (!double.IsNaN(USL) && double.IsNaN(LSL))
                    return (USL - Mu) / (Ksigma / 2 * Sigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="LSL"></param>
        /// <param name="USL"></param>
        /// <param name="Target"></param>
        /// <param name="Ksigma"></param>
        /// <param name="arrCleanData">double.Nan이 없는 배열</param>
        /// <returns></returns>
        private double GetCpm(double LSL, double USL, double Target, double Ksigma, List<double> arrCleanData)
        {
            double dblm;
            int iDataCnt;
            double dblSigma_;
            double dblTemp;
            double dblReturn;
            try
            {
                if (double.IsNaN(USL) && double.IsNaN(LSL))
                {
                    if (m_Exception)
                        throw new ArgumentException("Argument(USL, LSL) is double.NaN");
                    else
                        return double.NaN;
                }
                if (double.IsNaN(Ksigma))
                {
                    if (m_Exception)
                        throw new ArgumentException("Argument(Ksigma) is double.NaN");
                    else
                        return double.NaN;
                }

                if (arrCleanData == null)
                {
                    if (m_Exception)
                        throw new ArgumentException("Argument(Data) is null");
                    else
                        return double.NaN;
                }

                if (double.IsNaN(Target))
                {
                    if (m_Exception)
                        throw new ArgumentException("Argument(Target) is double.NaN");
                    else
                        return double.NaN;
                }


                iDataCnt = arrCleanData.Count;
                if (iDataCnt == 0)
                {
                    if (m_Exception)
                        throw new ArgumentException("Length of Argument(Data) is 0");
                    else
                        return double.NaN;
                }


                dblTemp = 0;
                for (int i = 0; i < iDataCnt; i++)
                    dblTemp += Math.Pow((arrCleanData[i] - Target), 2);
                dblSigma_ = Math.Sqrt(dblTemp / (iDataCnt - 1));

                if (!double.IsNaN(USL) && !double.IsNaN(LSL))
                {
                    dblm = (USL + LSL) / 2;
                    if (dblm == Target)
                    {
                        dblReturn = (USL - LSL) / (Ksigma * dblSigma_);
                    }
                    else
                    {
                        dblReturn = Math.Min((Target - LSL), (USL - Target)) / (Ksigma / 2 * dblSigma_);
                    }

                }
                else if (double.IsNaN(USL) && !double.IsNaN(LSL))
                {
                    dblReturn = (USL - Target) / (Ksigma / 2 * dblSigma_);
                }
                else //if (!double.IsNaN(USL) && double.IsNaN(LSL))
                {
                    dblReturn = (Target - LSL) / (Ksigma / 2 * dblSigma_);
                }
                return dblReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetC4(double N)
        {
            double dblGamma1;
            double dblGamma2;
            try
            {
                dblGamma1 = Math.Log(N / 2);
                dblGamma2 = Math.Log((N - 1) / 2);
                if (double.IsInfinity(dblGamma1) || double.IsInfinity(dblGamma2))
                    return 1;
                return Math.Sqrt(2 / (N - 1)) * dblGamma1 / dblGamma2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}


//*A3 = 3 / (c4 √ n)

//*B4 = 1 + 3 / {c4 √ 2(n-1)}

//*B3 = 1 - 3 / {c4 √ 2(n-1)}

//*c4 = 4(n-1) / (4n-3)

