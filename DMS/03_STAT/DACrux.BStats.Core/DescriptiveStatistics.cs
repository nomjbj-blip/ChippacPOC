using System;
using System.Data;
using CenterSpace.NMath.Stats;

namespace DACrux.BStats.Core
{
    /// <summary>
    ///
    /// </summary>
    public class DescriptiveStatistics
    {
        #region " MEMBER FIELD "

        private DataFrame m_Source;
        private int m_ColumnIndex;
        int m_iDecimalInner = 4;
        int m_iDecimalOuter = 4;
        double m_Trim = 0.1;
        private ResultStatistics m_Result;
        private SelectedStatistics m_InputOption;

        private double m_Mean = double.NaN;
        private double m_Std = double.NaN;
        private double m_Q1 = double.NaN;
        private double m_Q3 = double.NaN;
        private double m_Max = double.NaN;
        private double m_Min = double.NaN;
        private double m_SumofSquares = double.NaN;


        #endregion

        #region " CREATOR "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="ColumnIndex"></param>
        public DescriptiveStatistics(DataTable Source, int ColumnIndex, SelectedStatistics InputOption)
        {
            try
            {
                m_Source = new DataFrame(Source);
                m_ColumnIndex = ColumnIndex;
                m_Result = new ResultStatistics();
                m_InputOption = InputOption;
                Analysis();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region " PROPERTY "
        /// <summary>
        /// Can Get Descriptive Statistics Result
        /// </summary>
        public ResultStatistics Result
        {
            get
            {
                return m_Result;
            }
        }

        public int DecimalInner
        {
            get { return m_iDecimalInner; }
            set { m_iDecimalInner = value; }
        }

        public int DecimalOuter
        {
            get { return m_iDecimalOuter; }
            set { m_iDecimalOuter = value; }
        }

        public double TrimOfTrimmedMean
        {
            get { return m_Trim; }
            set { m_Trim = value; }
        }
        #endregion

        #region " METHOD "

        #region [ Coefficient of Variation ]
        /// <summary>
        /// 변동계수(100*표준편차 / 평균 )
        /// </summary>
        /// <param name="idfCleanColumn">Clean IDFColumn</param>
        /// <returns>Coefficient of Variation</returns>
        private double GetCoefficientofvariation(IDFColumn idfCleanColumn)
        {
            double dblReturn;
            double dblStd;
            double dblMean;
            try
            {
                if (double.IsNaN(m_Std))
                {
                    if (m_InputOption.IsBiased)
                        m_Std = StatsFunctions.StandardDeviation(idfCleanColumn, BiasType.Biased);
                    else
                        m_Std = StatsFunctions.StandardDeviation(idfCleanColumn, BiasType.Unbiased);
                }
                dblStd = m_Std;
                if (double.IsNaN(m_Mean))
                    m_Mean = StatsFunctions.Mean(idfCleanColumn);
                dblMean = m_Mean;

                dblReturn = (double)100 * Math.Round(dblStd, m_iDecimalInner) / Math.Round(dblMean, m_iDecimalInner);
                return Math.Round(dblReturn, m_iDecimalOuter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ MSSD ]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idfCleanColumn">Clean IDFColumn</param>
        /// <returns></returns>
        private double GetMSSD(IDFColumn idfCleanColumn)
        {
            double dblReturn;
            double dblCumulative;
            int iRowCnt;
            try
            {
                dblReturn = double.NaN;
                iRowCnt = idfCleanColumn.Count;
                dblCumulative = 0;

                for (int i = 1; i < iRowCnt; i++)
                {
                    dblCumulative += Math.Pow((double)idfCleanColumn[i] - (double)idfCleanColumn[i - 1], 2);
                }
                if (iRowCnt > 2)
                    dblReturn = Math.Round(dblCumulative / (double)(2 * ((double)iRowCnt - 1)), m_iDecimalOuter);
                return dblReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ Method For Count of Mode Value ]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private bool GetMode(double Value)
        {
            try
            {
                return (Value == m_Result.Mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ Standard Error of Mean ]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idfCleanColumn"></param>
        /// <returns></returns>
        private double GetMeanSE(IDFColumn idfCleanColumn)
        {
            double dblReturn;
            double dblS;
            int iRowCnt;
            try
            {
                dblReturn = double.NaN;
                iRowCnt = idfCleanColumn.Count;
                if (double.IsNaN(m_Std))
                {
                    if (m_InputOption.IsBiased)
                        m_Std = StatsFunctions.StandardDeviation(idfCleanColumn, BiasType.Biased);
                    else
                        m_Std = StatsFunctions.StandardDeviation(idfCleanColumn, BiasType.Unbiased);
                }
                dblS = m_Std;
                dblReturn = dblS / Math.Sqrt((double)iRowCnt);
                dblReturn = Math.Round(dblReturn, m_iDecimalOuter);
                return dblReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ Sum of Squares ]
        private double GetSS(IDFColumn idfCleanColumn)
        {
            double dblReturn;
            int iRowCnt;
            double dblCumulative;
            try
            {
                dblReturn = double.NaN;
                iRowCnt = idfCleanColumn.Count;
                dblCumulative = 0;
                for (int i = 0; i < iRowCnt; i++)
                {
                    dblCumulative += Math.Pow((double)idfCleanColumn[i], 2);
                }
                dblReturn = Math.Round(dblCumulative, m_iDecimalOuter);
                return dblReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ Sum ]
        private double GetSum(IDFColumn idfCleanColumn)
        {
            int iRowCnt;
            double dblAccumulator;
            int iDecimal;
            int iMax;
            try
            {
                iRowCnt = idfCleanColumn.Count;
                dblAccumulator = 0;
                iMax = 0;
                for (int i = 0; i < iRowCnt; i++)
                {
                    dblAccumulator += (double)idfCleanColumn[i];
                    iDecimal = ((double)idfCleanColumn[i]).ToString().Length - ((double)idfCleanColumn[i]).ToString().IndexOf('.');
                    if (iMax < iDecimal)
                        iMax = iDecimal;
                }
                return Math.Round(dblAccumulator, iMax);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ Analysis ]

        private void Analysis()
        {
            IDFColumn idfCol = null;
            IDFColumn idfCleanCol = null;
            int iRowCnt;
            try
            {

                idfCol = m_Source[m_ColumnIndex];
                idfCleanCol = idfCol.Clean();
                iRowCnt = idfCleanCol.Count;

                if (m_InputOption.IsMean)
                {
                    m_Mean = StatsFunctions.Mean(idfCleanCol);
                    m_Result.Mean = Math.Round(m_Mean, m_iDecimalOuter);
                }

                if (m_InputOption.IsStandarddeviation)
                {
                    if (m_InputOption.IsBiased)
                    {
                        m_Std = StatsFunctions.StandardDeviation(idfCleanCol, BiasType.Biased);
                        m_Result.Standarddeviation = Math.Round(m_Std, m_iDecimalOuter);
                    }
                    else
                    {
                        m_Std = StatsFunctions.StandardDeviation(idfCleanCol, BiasType.Unbiased);
                        m_Result.Standarddeviation = Math.Round(m_Std, m_iDecimalOuter);
                    }
                }

                if (m_InputOption.IsVariance)
                {
                    if (m_InputOption.IsBiased)
                    {
                        m_Result.Variance = Math.Round(StatsFunctions.Variance(idfCleanCol, BiasType.Biased), m_iDecimalOuter);
                    }
                    else
                    {
                        m_Result.Variance = Math.Round(StatsFunctions.Variance(idfCleanCol, BiasType.Unbiased), m_iDecimalOuter);
                    }
                }


                if (m_InputOption.IsCoefficientofvariation)
                {
                    //100*표준편차 / 평균 
                    m_Result.Coefficientofvariation = GetCoefficientofvariation(idfCleanCol);
                }

                if (m_InputOption.IsCumulativeN)  //누적 비결측자료수
                {
                    m_Result.CumulativeN = StatsFunctions.Count(idfCleanCol);
                }
                if (m_InputOption.IsCumulativePercent)  //누적 퍼센트
                {
                    m_Result.CumulativePercent = Math.Round((double)100 * StatsFunctions.Count(idfCleanCol) / StatsFunctions.Count(idfCol), m_iDecimalOuter);
                }


                if (m_InputOption.IsQ1)
                {
                    m_Q1 = StatsFunctions.Quartile(idfCleanCol, 1);
                    m_Result.Q1 = m_Q1;
                }
                if (m_InputOption.IsQ3)
                {
                    m_Q3 = StatsFunctions.Quartile(idfCleanCol, 3);
                    m_Result.Q3 = m_Q3;
                }

                if (m_InputOption.IsInterquartileRange)  //IQR
                {
                    if (double.IsNaN(m_Q1))
                        m_Q1 = StatsFunctions.Quartile(idfCleanCol, 1);
                    if (double.IsNaN(m_Q3))
                        m_Q3 = StatsFunctions.Quartile(idfCleanCol, 3);

                    m_Result.InterquartileRange = Math.Round(m_Q3 - m_Q1, m_iDecimalOuter);
                }

                if (m_InputOption.IsMin)
                {
                    m_Min = StatsFunctions.MinValue(idfCleanCol);
                    m_Result.Min = m_Min;
                }
                if (m_InputOption.IsMax)
                {
                    m_Max = StatsFunctions.MaxValue(idfCleanCol);
                    m_Result.Max = m_Max;
                }

                if (m_InputOption.IsRange)
                {
                    if (double.IsNaN(m_Min))
                        m_Min = StatsFunctions.MinValue(idfCleanCol);
                    if (double.IsNaN(m_Max))
                        m_Max = StatsFunctions.MaxValue(idfCleanCol);

                    m_Result.Range = Math.Round(m_Max - m_Min, m_iDecimalOuter);
                }


                if (m_InputOption.IsKurtosis)
                {
                    if (m_InputOption.IsBiased)
                        m_Result.Kurtosis = Math.Round(StatsFunctions.Kurtosis(idfCleanCol, BiasType.Unbiased), m_iDecimalOuter);
                    else
                        m_Result.Kurtosis = Math.Round(StatsFunctions.Kurtosis(idfCleanCol, BiasType.Biased), m_iDecimalOuter);
                }



                if (m_InputOption.IsMedian)
                {
                    m_Result.Median = StatsFunctions.Median(idfCleanCol);
                }

                if (m_InputOption.IsMode)
                {
                    if (iRowCnt > 1)
                    {
                        m_Result.Mode = StatsFunctions.Mode(idfCleanCol);
                        m_Result.ModeN = StatsFunctions.CountIf(idfCleanCol, new StatsFunctions.LogicalDoubleFunction(GetMode));
                        if (m_Result.ModeN == 1)
                        {
                            m_Result.Mode = double.NaN;
                            m_Result.ModeN = 0;
                        }
                    }
                }

                if (m_InputOption.IsSumofSquares)
                {
                    m_SumofSquares = GetSS(idfCleanCol);
                    m_Result.SumofSquares = m_SumofSquares;
                }


                if (m_InputOption.IsMeanSE)
                {
                    m_Result.MeanSe = GetMeanSE(idfCleanCol);
                }

                if (m_InputOption.IsMSSD)
                {
                    m_Result.MSSD = GetMSSD(idfCleanCol);
                }
                if (m_InputOption.IsNmissing)
                {
                    m_Result.Nmissing = StatsFunctions.Count(idfCol) - StatsFunctions.Count(idfCleanCol);
                }
                //if (m_InputOption.IsNnonmissing)   //누적 수량 및 퍼센트, 누적 퍼센트를 밖에서 계산하기 위해서 수정했음
                //{
                m_Result.Nnonmissing = StatsFunctions.Count(idfCleanCol);
                //}
                //if (m_InputOption.IsNtotal)   //누적 수량 및 퍼센트, 누적 퍼센트를 밖에서 계산하기 위해서 수정했음
                //{
                m_Result.Ntotal = StatsFunctions.Count(idfCol);
                //}
                if (m_InputOption.IsPercent)
                {
                    m_Result.Percent = Math.Round((double)100 * StatsFunctions.Count(idfCleanCol) / StatsFunctions.Count(idfCol), m_iDecimalOuter);
                }

                if (m_InputOption.IsSkewness)
                {
                    if (m_InputOption.IsBiased)
                        m_Result.Skewness = Math.Round(StatsFunctions.Skewness(idfCleanCol, BiasType.Biased), m_iDecimalOuter);
                    else
                        m_Result.Skewness = Math.Round(StatsFunctions.Skewness(idfCleanCol, BiasType.Unbiased), m_iDecimalOuter);
                }

                if (m_InputOption.IsSum)
                {
                    //m_Result.Sum = StatsFunctions.Sum(idfCleanCol);  //소수점 문제로 인해 변경(있잖아요 왜..2진수로 표현할때 에러...)
                    m_Result.Sum = GetSum(idfCleanCol);
                }

                if (m_InputOption.IsTrimmedMean)
                {
                    m_Result.TrimmedMean = Math.Round(StatsFunctions.TrimmedMean(idfCleanCol, m_Trim), m_iDecimalOuter);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion
    }
}
