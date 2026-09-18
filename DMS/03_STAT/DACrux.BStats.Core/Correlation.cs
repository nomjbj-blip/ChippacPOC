using System;
using System.Data;
using CenterSpace.NMath.Stats;

namespace DACrux.BStats.Core
{
    public class Correlation
    {
        #region " MEMBER FIELD "

        private DataFrame m_Source;
        private DataFrame m_CleanSource;
        private int m_X;  // X변수의 Column Index
        private int m_Y;  // Y변수의 Column Index
        int m_iDecimalInner = 4;
        int m_iDecimalOuter = 4;

        private ResultCorrelation m_Result;
        private SelectedCorrelation m_InputOption;

        #endregion

        #region " CREATOR "
        public Correlation(DataTable Source, int iX, int iY, SelectedCorrelation InputOption)
        {
            try
            {
                m_Source = new DataFrame(Source);
                m_X = iX;
                m_Y = iY;
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
        /// Can Get Correlation Result
        /// </summary>
        public ResultCorrelation Result
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
        #endregion

        #region " METHOD "

        private void Analysis()
        {

            double dblCoefficientCorrelation;
            double dblSpearman;
            try
            {
                m_Result = new ResultCorrelation();
                m_Result.Xvar = m_Source.ColumnHeaders[m_X];
                m_Result.Yvar = m_Source.ColumnHeaders[m_Y];
                m_CleanSource = m_Source.CleanRows(m_X, m_Y);
                CenterSpace.NMath.Stats.GammaDistribution oGamma = new GammaDistribution();

                dblCoefficientCorrelation = StatsFunctions.Correlation(m_CleanSource[m_X], m_CleanSource[m_Y]);
                m_Result.CoefficientCorrelation = Math.Round(dblCoefficientCorrelation, m_iDecimalOuter);
                if (m_InputOption.IsPvalue)
                    m_Result.Pvalue = Math.Round(GetPvalue(dblCoefficientCorrelation), m_iDecimalOuter);
                if (m_InputOption.IsSpearman)
                {
                    dblSpearman = StatsFunctions.Spearmans(m_CleanSource[m_X], m_CleanSource[m_Y]);
                    if (m_InputOption.IsPvalue)
                        m_Result.SpearmanPvalue = Math.Round(GetPvalue(dblSpearman), m_iDecimalOuter);
                    m_Result.Spearman = Math.Round(dblSpearman, m_iDecimalOuter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetPvalue(double r)
        {
            double dblReturn;
            int iN;
            try
            {
                iN = m_CleanSource.Rows;
                double t = (Math.Sqrt(iN - 2) * r) / Math.Sqrt(1 - Math.Pow(r, 2));
                TDistribution td = new TDistribution(iN - 2);

                if (r < 0)
                    dblReturn = td.CDF(t);
                else
                    dblReturn = 1 - td.CDF(t);
                dblReturn = dblReturn * 2.0;
                return Math.Round(dblReturn, m_iDecimalOuter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
