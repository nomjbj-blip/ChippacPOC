using System;
using System.Data;
using CenterSpace.NMath.Core;
using CenterSpace.NMath.Stats;

namespace DACrux.BStats.Core
{
    public class HypothesisTesting
    {
        #region " MEMBER FIELD "

        private DataTable m_Source;
        /// <summary>
        /// 분석 컬럼의 인덱스
        /// </summary>
        private int m_iCol1 = -1;
        /// <summary>
        /// Two Sample 일경우 두번째 분석 컬럼의 인덱스
        /// </summary>
        private int m_iCol2 = -1;
        int m_iDecimalInner = 4;
        int m_iDecimalOuter = 4;
        SelectedHypothesisTesting m_InputOption;
        ResultHypothesisTesting m_Result;

        #region [ LANGUAGE ]
        string m_Statistics = "Basic statistics";
        string m_Variable = "Variable";
        string m_N = "N";
        string m_Mean = "Mean";
        string m_Standarddeviation = "StDev";
        string m_Min = "Minimum";
        string m_Max = "Maximum";
        string m_Difference = "Difference";
        #endregion
        #region Message
        //private string m_ErrorSpec_Target = "Target is out of spec.";
        //private string m_ErrorSpec_LSLmoreUSL = "LSL cannot be greater than USL";
        //private string m_ErrorLengthofMovingRange = "Length of moving range must be between 2 and 100.";
        //private string m_ErrorKsigma = "K Sigma must be positive natural number.";
        //private string m_ErrorSubgroupsize = "More than half of Subgroup should be the same size.";
        //private string m_ErrorArgumentContantForSubgroupSize = "Sub group size must be positive natural number.";
        //private string m_ErrorStd0 = "Standard deviation is 0. Cannot analysis.";
        private string m_ErrorNotEnoughData = "Not enough data in column.";

        #endregion

        #endregion

        #region " CREATOR "

        public HypothesisTesting(DataTable Source, int iCol, SelectedHypothesisTesting InputOption) : this(Source, iCol, -1, InputOption)
        {            
        }

        public HypothesisTesting(DataTable Source, int iCol1, int iCol2, SelectedHypothesisTesting InputOption)
        {
            m_Source = Source;
            m_iCol1 = iCol1;
            m_iCol2 = iCol2;
            m_InputOption = InputOption;
            m_Result = new ResultHypothesisTesting();
            m_Result = Analysis(m_Source, m_iCol1, m_iCol2, m_InputOption);
        }

        #endregion

        #region " PROPERTY "

        public ResultHypothesisTesting Result
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

        #region [ Inner methods ]

        private ResultHypothesisTesting Analysis(DataTable dtSource, int iCol1, int iCol2, SelectedHypothesisTesting oInputOption)
        {
            ResultHypothesisTesting oReturn;
            DataFrame dfClean;
            DoubleVector dv1;
            DoubleVector dv2;
            DoubleVector dvDifference;
            double dblMean_ = double.NaN;  // 검정에 사용 할 평균
            double dblStandardDeviation_ = double.NaN; // 검정에 사용 할 표준편차
            double dblDifference_ = double.NaN; // 검정에 사용 할 차이
            double dblAlpha;
            HypothesisType oType = HypothesisType.TwoSided;

            #region Var for statistics
            string strVar1= string.Empty;
            string strVar2 = string.Empty;
            int iN1;
            int iN2;
            int iNd;
            double dblMean1;
            double dblMean2;
            double dblMeanD;
            double dblStd1;
            double dblStd2;
            double dblStdD;
            double dblMin1;
            double dblMin2;
            double dblMinD;
            double dblMax1;
            double dblMax2;
            double dblMaxD;
            double dblSeMean1;
            double dblSeMean2;
            double dblSeMeanD;
            DataTable dtStatistics = null;
            #endregion

            try
            {
                #region Check Rull
                if (dtSource == null)
                    throw new Exception("dtSource is null.");
                if (iCol1 >= dtSource.Columns.Count || iCol2 >= dtSource.Columns.Count)
                    throw new Exception("out of index");
                if (oInputOption.Testing == TestingType.OneSampleT || oInputOption.Testing == TestingType.OneSampleZ)
                {
                    if (iCol1 < 0)
                        throw new Exception("Column index is -1");
                }
                else
                {
                    if (iCol1 < 0 || iCol2 < 0)
                        throw new Exception("Column index is -1");
                    if(iCol1 == iCol2)
                        throw new Exception("Column1 equal column2");
                }
                #endregion

                dtStatistics = new DataTable(m_Statistics);
                dtStatistics.Columns.Add(m_Variable);
                dtStatistics.Columns.Add(m_N);
                dtStatistics.Columns.Add(m_Mean);
                dtStatistics.Columns.Add(m_Standarddeviation);
                dtStatistics.Columns.Add(m_Min);
                dtStatistics.Columns.Add(m_Max);

                if(oInputOption.oMethods.Alternative == AlternativeType.Greater)
                    oType = HypothesisType.Right;
                else if(oInputOption.oMethods.Alternative == AlternativeType.Less)
                    oType = HypothesisType.Left;
                else
                    oType = HypothesisType.TwoSided;

                if (oInputOption.oMethods.ConfidenceLevel >= 100 || oInputOption.oMethods.ConfidenceLevel <= 0)
                    throw new Exception("ConfidenceLevel Error");
                dblAlpha = (100 - oInputOption.oMethods.ConfidenceLevel) / 100;
                oReturn = new ResultHypothesisTesting();
                
                #region Case별 Analysis

                switch (oInputOption.Testing)
                {
                    #region One Sample T test
                    case TestingType.OneSampleT:

                        #region DoubleVector Creator
                        dfClean = (new DataFrame(dtSource)).CleanRows(iCol1);
                        dv1 = ((IDFColumn)dfClean[iCol1]).ToDoubleVector();
                        if (dv1.Length < 1)
                        {
                            oReturn.ErrorInformation = m_ErrorNotEnoughData;
                            return oReturn;
                        }
                        #endregion

                        #region Descriptive Statistics
                        strVar1 = dtSource.Columns[iCol1].ColumnName;
                        iN1 = dv1.Length;
                        dblMean1 = StatsFunctions.Mean(dv1.ToArray());
                        dblStd1 = StatsFunctions.StandardDeviation(dv1, BiasType.Unbiased);
                        dblMin1 = StatsFunctions.MinValue(dv1);
                        dblMax1 = StatsFunctions.MaxValue(dv1);
                        dblSeMean1 = dblStd1 / Math.Sqrt((double)iN1);
                        dtStatistics.Rows.Add(new object[] { strVar1, iN1, Math.Round(dblMean1, m_iDecimalOuter), Math.Round(dblStd1, m_iDecimalOuter), dblMin1, dblMax1 });
                        dtStatistics.AcceptChanges();
                        #endregion

                        #region Testing Vaiables
                        if (double.IsNaN(oInputOption.oMethods.TestingMean))
                            dblMean_ = dblMean1;
                        else
                            dblMean_ = oInputOption.oMethods.TestingMean;
                        #endregion                       

                        #region Testing
                        OneSampleTTest oTest1T = new OneSampleTTest(dblMean1, dblStd1, iN1, dblMean_, dblAlpha, oType);
                        #endregion

                        #region Result Make
                        oReturn.P = Math.Round(oTest1T.P, m_iDecimalOuter);
                        oReturn.Statistic = Math.Round(oTest1T.Statistic, m_iDecimalOuter);
                        oReturn.StatisticName = "T-Value";
                        oReturn.LeftCriticalValue = Math.Round(oTest1T.LeftCriticalValue, m_iDecimalOuter);
                        oReturn.RightCriticalValue = Math.Round(oTest1T.RightCriticalValue, m_iDecimalOuter);

                        #region CI
                        if (oType == HypothesisType.Right)
                            oReturn.LowerConfidenceLimit = Math.Round(oTest1T.LowerConfidenceLimit, m_iDecimalOuter);
                        else if (oType == HypothesisType.Left)
                            oReturn.UpperConfidenceLimit = Math.Round(oTest1T.UpperConfidenceLimit, m_iDecimalOuter);
                        else
                        {
                            oReturn.LowerConfidenceLimit = Math.Round(oTest1T.LowerConfidenceLimit, m_iDecimalOuter);
                            oReturn.UpperConfidenceLimit = Math.Round(oTest1T.UpperConfidenceLimit, m_iDecimalOuter);
                        }
                        #endregion                        
                        
                        #endregion
                        break;
                    #endregion

                    #region Two Sample Paired

                    case TestingType.TwoSamplePaired:

                        #region DoubleVector Creator
                        dfClean = (new DataFrame(dtSource)).CleanRows(iCol1, iCol2);
                        dv1 = ((IDFColumn)dfClean[iCol1]).ToDoubleVector();
                        dv2 = ((IDFColumn)dfClean[iCol2]).ToDoubleVector();
                        if (dv1.Length < 1 || dv2.Length < 1)
                        {
                            oReturn.ErrorInformation = m_ErrorNotEnoughData;
                            return oReturn;
                        }
#if DEBUG
                        if (dv1.Length != dv2.Length)
                            throw new Exception("먼 경우??????");
#endif
                        dvDifference = dv1 - dv2;
                        #endregion

                        #region Descriptive Statistics
                        strVar1 = dtSource.Columns[iCol1].ColumnName;
                        strVar2 = dtSource.Columns[iCol2].ColumnName;
                        iN1 = dv1.Length;
                        iN2 = dv2.Length;
                        iNd = dvDifference.Length;
                        dblMean1 = StatsFunctions.Mean(dv1.ToArray());
                        dblMean2 = StatsFunctions.Mean(dv1.ToArray());
                        dblMeanD = StatsFunctions.Mean(dvDifference.ToArray());
                        dblStd1 = StatsFunctions.StandardDeviation(dv1, BiasType.Unbiased);
                        dblStd2 = StatsFunctions.StandardDeviation(dv2, BiasType.Unbiased);
                        dblStdD = StatsFunctions.StandardDeviation(dvDifference, BiasType.Unbiased);
                        dblMin1 = StatsFunctions.MinValue(dv1);
                        dblMin2 = StatsFunctions.MinValue(dv2);
                        dblMinD = StatsFunctions.MinValue(dvDifference);
                        dblMax1 = StatsFunctions.MaxValue(dv1);
                        dblMax2 = StatsFunctions.MaxValue(dv2);
                        dblMaxD = StatsFunctions.MaxValue(dvDifference);
                        dblSeMean1 = dblStd1 / Math.Sqrt((double)iN1);
                        dblSeMean2 = dblStd2 / Math.Sqrt((double)iN2);
                        dblSeMeanD = dblStdD / Math.Sqrt((double)iNd);
                        dtStatistics.Rows.Add(new object[] { strVar1, iN1, Math.Round(dblMean1, m_iDecimalOuter), Math.Round(dblStd1, m_iDecimalOuter), dblMin1, dblMax1 });
                        dtStatistics.Rows.Add(new object[] { strVar2, iN2, Math.Round(dblMean2, m_iDecimalOuter), Math.Round(dblStd2, m_iDecimalOuter), dblMin2, dblMax2 });
                        dtStatistics.Rows.Add(new object[] { m_Difference, iNd, Math.Round(dblMeanD, m_iDecimalOuter), Math.Round(dblStdD, m_iDecimalOuter), dblMinD, dblMaxD });
                        dtStatistics.AcceptChanges();
                        #endregion

                        #region Testing Vaiables
                        if (double.IsNaN(oInputOption.oMethods.TestingDifference))
                            dblDifference_ = 0;
                        else
                            dblDifference_ = oInputOption.oMethods.TestingDifference;
                        #endregion

                        

                        #region Testing
                        TwoSamplePairedTTest oTest2P = new TwoSamplePairedTTest(dblMeanD - dblDifference_, dblStdD, iNd, dblAlpha, oType);
                        #endregion

                        #region Result Make

                        oReturn.P = Math.Round(oTest2P.P, m_iDecimalOuter);
                        oReturn.Statistic = Math.Round(oTest2P.Statistic, m_iDecimalOuter);
                        oReturn.StatisticName = "T-Value";
                        //oReturn.EstimateForDifference = dblMeanD;

                        oReturn.LeftCriticalValue = Math.Round(oTest2P.LeftCriticalValue, m_iDecimalOuter);
                        oReturn.RightCriticalValue = Math.Round(oTest2P.RightCriticalValue, m_iDecimalOuter);

                        #region CI
                        if (oType == HypothesisType.Right)
                            oReturn.LowerConfidenceLimit = Math.Round(oTest2P.LowerConfidenceLimit + dblDifference_, m_iDecimalOuter);
                        else if (oType == HypothesisType.Left)
                            oReturn.UpperConfidenceLimit = Math.Round(oTest2P.UpperConfidenceLimit + dblDifference_, m_iDecimalOuter);
                        else
                        {
                            oReturn.LowerConfidenceLimit = Math.Round(oTest2P.LowerConfidenceLimit + dblDifference_, m_iDecimalOuter);
                            oReturn.UpperConfidenceLimit = Math.Round(oTest2P.UpperConfidenceLimit + dblDifference_, m_iDecimalOuter);
                        }
                        #endregion                        

                        #endregion

                        break;

                    #endregion

                    #region Two Sample Unpaired
                    case TestingType.TwoSampleUnpaired:
                        #region DoubleVector Creator
                        dfClean = new DataFrame(dtSource);
                        dv1 = ((IDFColumn)dfClean[iCol1]).Clean().ToDoubleVector();
                        dv2 = ((IDFColumn)dfClean[iCol2]).Clean().ToDoubleVector();
                        if (dv1.Length < 1 || dv2.Length < 1)
                        {
                            oReturn.ErrorInformation = m_ErrorNotEnoughData;
                            return oReturn;
                        }
                        #endregion

                        #region Descriptive Statistics
                        strVar1 = dtSource.Columns[iCol1].ColumnName;
                        strVar2 = dtSource.Columns[iCol2].ColumnName;
                        iN1 = dv1.Length;
                        iN2 = dv2.Length;
                        dblMean1 = StatsFunctions.Mean(dv1.ToArray());
                        dblMean2 = StatsFunctions.Mean(dv2.ToArray());
                        dblStd1 = StatsFunctions.StandardDeviation(dv1, BiasType.Unbiased);
                        dblStd2 = StatsFunctions.StandardDeviation(dv2, BiasType.Unbiased);
                        dblMin1 = StatsFunctions.MinValue(dv1);
                        dblMin2 = StatsFunctions.MinValue(dv2);
                        dblMax1 = StatsFunctions.MaxValue(dv1);
                        dblMax2 = StatsFunctions.MaxValue(dv2);
                        dblSeMean1 = dblStd1 / Math.Sqrt((double)iN1);
                        dblSeMean2 = dblStd2 / Math.Sqrt((double)iN2);
                        dtStatistics.Rows.Add(new object[] { strVar1, iN1, Math.Round(dblMean1, m_iDecimalOuter), Math.Round(dblStd1, m_iDecimalOuter), dblMin1, dblMax1 });
                        dtStatistics.Rows.Add(new object[] { strVar2, iN2, Math.Round(dblMean2, m_iDecimalOuter), Math.Round(dblStd2, m_iDecimalOuter), dblMin2, dblMax2 });
                        dtStatistics.AcceptChanges();
                        #endregion

                        #region Testing Vaiables
                        if (double.IsNaN(oInputOption.oMethods.TestingDifference))
                            dblDifference_ = 0;
                        else
                            dblDifference_ = oInputOption.oMethods.TestingDifference;
                        #endregion                       

                        #region Testing
                        TwoSampleUnpairedTTest oTest2U = new TwoSampleUnpairedTTest(dblMean1, dblStd1, iN1, dblMean2 + dblDifference_, dblStd2, iN2, dblAlpha, oType);
                        #endregion

                        #region Result Make

                        oReturn.P = Math.Round(oTest2U.P, m_iDecimalOuter);
                        oReturn.Statistic = Math.Round(oTest2U.Statistic, m_iDecimalOuter);
                        oReturn.StatisticName = "T-Value";
                        oReturn.PooledStDev = Math.Round(oTest2U.SPooled, m_iDecimalOuter);
                        oReturn.EstimateForDifference = Math.Round(dblMean1 - dblMean2, m_iDecimalOuter);

                        oReturn.LeftCriticalValue = Math.Round(oTest2U.LeftCriticalValue, m_iDecimalOuter);
                        oReturn.RightCriticalValue = Math.Round(oTest2U.RightCriticalValue, m_iDecimalOuter);

                        #region CI
                        if (oType == HypothesisType.Right)
                            oReturn.LowerConfidenceLimit = Math.Round(oTest2U.LowerConfidenceLimit + dblDifference_, m_iDecimalOuter);
                        else if (oType == HypothesisType.Left)
                            oReturn.UpperConfidenceLimit = Math.Round(oTest2U.UpperConfidenceLimit + dblDifference_, m_iDecimalOuter);
                        else
                        {
                            oReturn.LowerConfidenceLimit = Math.Round(oTest2U.LowerConfidenceLimit + dblDifference_, m_iDecimalOuter);
                            oReturn.UpperConfidenceLimit = Math.Round(oTest2U.UpperConfidenceLimit + dblDifference_, m_iDecimalOuter);
                        }
                        #endregion
                        
                        #endregion
                        break;
                    #endregion

                    #region One Sample Z
                    default:  //case TestingType.OneSampleZ:
                        #region DoubleVector Creator
                        dfClean = (new DataFrame(dtSource)).CleanRows(iCol1);
                        dv1 = ((IDFColumn)dfClean[iCol1]).ToDoubleVector();
                        if (dv1.Length < 1)
                        {
                            oReturn.ErrorInformation = m_ErrorNotEnoughData;
                            return oReturn;
                        }
                        #endregion

                        
                        #region Descriptive Statistics
                        strVar1 = dtSource.Columns[iCol1].ColumnName;
                        iN1 = dv1.Length;
                        dblMean1 = StatsFunctions.Mean(dv1.ToArray());
                        dblStd1 = StatsFunctions.StandardDeviation(dv1, BiasType.Unbiased);
                        dblMin1 = StatsFunctions.MinValue(dv1);
                        dblMax1 = StatsFunctions.MaxValue(dv1);
                        dblSeMean1 = dblStd1 / Math.Sqrt((double)iN1);
                        dtStatistics.Rows.Add(new object[] { strVar1, iN1, Math.Round(dblMean1, m_iDecimalOuter), Math.Round(dblStd1, m_iDecimalOuter), dblMin1, dblMax1 });
                        dtStatistics.AcceptChanges();
                        #endregion

                        #region Testing Vaiables
                        if (double.IsNaN(oInputOption.oMethods.TestingMean))
                            dblMean_ = dblMean1;
                        else
                            dblMean_ = oInputOption.oMethods.TestingMean;
                        if (double.IsNaN(oInputOption.oMethods.TestingStd))
                            dblStandardDeviation_ = dblStd1;
                        else
                            dblStandardDeviation_ = oInputOption.oMethods.TestingStd;
                        #endregion

                        #region Testing
                        OneSampleZTest oTest1Z = new OneSampleZTest(dblMean1, iN1, dblMean_, dblStandardDeviation_, dblAlpha, oType);
                        #endregion

                        #region Result Make
                        oReturn.P = Math.Round(oTest1Z.P, m_iDecimalOuter);
                        oReturn.Statistic = Math.Round(oTest1Z.Statistic, m_iDecimalOuter);
                        oReturn.StatisticName = "Z";
                        oReturn.LeftCriticalValue = Math.Round(oTest1Z.LeftCriticalValue, m_iDecimalOuter);
                        oReturn.RightCriticalValue = Math.Round(oTest1Z.RightCriticalValue, m_iDecimalOuter);
                        #region CI
                        if (oType == HypothesisType.Right)
                            oReturn.LowerConfidenceLimit = Math.Round(oTest1Z.LowerConfidenceLimit, m_iDecimalOuter);
                        else if (oType == HypothesisType.Left)
                            oReturn.UpperConfidenceLimit = Math.Round(oTest1Z.UpperConfidenceLimit, m_iDecimalOuter);
                        else
                        {
                            oReturn.LowerConfidenceLimit = Math.Round(oTest1Z.LowerConfidenceLimit, m_iDecimalOuter);
                            oReturn.UpperConfidenceLimit = Math.Round(oTest1Z.UpperConfidenceLimit, m_iDecimalOuter);
                        }
                        #endregion
                        #endregion
                        break;
                    #endregion

                }                
                #endregion
                oReturn.Alpha = dblAlpha;
                oReturn.TestingMean = Math.Round(dblMean_, m_iDecimalOuter);
                oReturn.TestingStd = Math.Round(dblStandardDeviation_, m_iDecimalOuter);
                oReturn.TestingDifference = Math.Round(dblDifference_, m_iDecimalOuter);
                oReturn.dtDescriptiveStat = dtStatistics;
                oReturn.ErrorInformation = string.Empty;
                oReturn.VariableName1 = strVar1;
                oReturn.VariableName2 = strVar2;                
                return oReturn;
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
