using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace DACrux.BStats
{
    public class MenuOption
    {
        #region " MEMBER FIELD "

        private string m_Yes = "Y";
        private string m_No = "N";
        private string m_FilePath = Application.StartupPath + @"\MenuOption.xml";
        #endregion

        #region " CREATOR "
        public MenuOption()
        {
            if (!File.Exists(m_FilePath))
                InitializeOption();
        }

        #endregion

        #region " METHOD "

        #region [ Create XML File ]
        /// <summary>
        /// 
        /// </summary>
        private void CreateMenuOptionXMLFile()
        {
            DataSet dsXml = null;
            try
            {
                
                dsXml = new DataSet();

                #region STAT MENU
                dsXml.Tables.Add(new DataTable("STATMENU"));
                dsXml.Tables["STATMENU"].Columns.Add(new DataColumn("MAINMENU", System.Type.GetType("System.String")));
                dsXml.Tables["STATMENU"].Columns.Add(new DataColumn("DIALOG", System.Type.GetType("System.String")));
                dsXml.Tables["STATMENU"].Columns.Add(new DataColumn("DEPTH", System.Type.GetType("System.String")));
                dsXml.Tables["STATMENU"].Columns.Add(new DataColumn("ITEM", System.Type.GetType("System.String")));
                dsXml.Tables["STATMENU"].Columns.Add(new DataColumn("VALUE", System.Type.GetType("System.String")));
                #endregion

                dsXml.WriteXml(m_FilePath, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dsXml != null)
                    dsXml.Dispose();
                dsXml = null;
            }
        }

        #endregion
        
        #region [ Initialize Option ]

        private void InitializeOption()
        {
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4

            try
            {
                if (!File.Exists(m_FilePath))
                {
                    CreateMenuOptionXMLFile();
                }
                dsXml = new DataSet();
                dsXml.ReadXmlSchema(m_FilePath);

                #region Stat

                strMenuType = "STATMENU";
                

                #region Descriptive Statistics
                strMainMenu = "Descriptive Statistics";
                DACrux.BStats.StatisticsInput.inputDescriptiveStatistics oinputDescriptiveStatistics = new DACrux.BStats.StatisticsInput.inputDescriptiveStatistics(string.Empty);  //초기상태

                #region Statistics
                strDialog = "Statistics";
                strDepth = "1";

                strItem = "Mean";
                if (oinputDescriptiveStatistics.IsMean)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "SE of mean";
                if (oinputDescriptiveStatistics.IsMeanSE)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Standard Deviation";
                if (oinputDescriptiveStatistics.IsStandarddeviation)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Variance";
                if (oinputDescriptiveStatistics.IsVariance)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Coefficient of variation";
                if (oinputDescriptiveStatistics.IsCoefficientofvariation)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Trimmed mean";
                if (oinputDescriptiveStatistics.IsTrimmedMean)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "Sum";
                if (oinputDescriptiveStatistics.IsSum)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "Minimum";
                if (oinputDescriptiveStatistics.IsMin)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Maximum";
                if (oinputDescriptiveStatistics.IsMax)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Range";
                if (oinputDescriptiveStatistics.IsRange)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "N nonmissing";
                if (oinputDescriptiveStatistics.IsNnonmissing)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "N missing";
                if (oinputDescriptiveStatistics.IsNmissing)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "N total";
                if (oinputDescriptiveStatistics.IsNtotal)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Cumulative N";
                if (oinputDescriptiveStatistics.IsCumulativeN)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Percent";
                if (oinputDescriptiveStatistics.IsPercent)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Cumulative percent";
                if (oinputDescriptiveStatistics.IsCumulativePercent)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "First quartile";
                if (oinputDescriptiveStatistics.IsQ1)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Median";
                if (oinputDescriptiveStatistics.IsMedian)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Third quarile";
                if (oinputDescriptiveStatistics.IsQ3)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Interquartile range";
                if (oinputDescriptiveStatistics.IsInterquartileRange)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Mode";
                if (oinputDescriptiveStatistics.IsMode)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Sum of Squares";
                if (oinputDescriptiveStatistics.IsSumofSquares)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Skewness";
                if (oinputDescriptiveStatistics.IsSkewness)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Kurtosis";
                if (oinputDescriptiveStatistics.IsKurtosis)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "MSSD";
                if (oinputDescriptiveStatistics.IsMSSD)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });
                
                #endregion

                #region Graphs

                strDialog = "Graphs";
                strDepth = "1";

                strItem = "Histogram";
                if (oinputDescriptiveStatistics.IsHistogram)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "HistogramN with Normal Curve";
                if (oinputDescriptiveStatistics.IsHistogramNNormalCurve)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Box Plot";
                if (oinputDescriptiveStatistics.IsBoxPlot)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                #endregion

                #endregion

                #region ANOVA

                strMainMenu = "ANOVA";
                DACrux.BStats.StatisticsInput.inputANOVA oinputAnova = new DACrux.BStats.StatisticsInput.inputANOVA(string.Empty);

                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Is Single Column";
                if (oinputAnova.IsSingle)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                #endregion


                #region Graph

                strDialog = "Graph";
                strDepth = "1";


                strItem = "Histogram of residuals";
                if (oinputAnova.IsGraphHistogramResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Normal plot of residuals";
                if (oinputAnova.IsGraphProbabilityPlotResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Residuals versus fits";
                if (oinputAnova.IsGraphResidualsVsFittedValues)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Boxplots of data";
                if (oinputAnova.IsBoxPlot)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                #endregion

                #endregion

                #region Regression

                strMainMenu = "Regression";
                DACrux.BStats.StatisticsInput.inputRegression oinputRegression = new DACrux.BStats.StatisticsInput.inputRegression(string.Empty);

                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Fit intercept";
                if (oinputRegression.IsIntercept)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                #endregion

                #region Methods

                strDialog = "Methods";
                strDepth = "1";

                strItem = "Selection All";
                if (oinputRegression.ReAnalysisType == DACrux.BStats.Core.RegressionType.All)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Selection Stepwise";
                if (oinputRegression.ReAnalysisType == DACrux.BStats.Core.RegressionType.Stepwise)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Selection Forward";
                if (oinputRegression.ReAnalysisType == DACrux.BStats.Core.RegressionType.Forward)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "Selection Backward";
                if (oinputRegression.ReAnalysisType == DACrux.BStats.Core.RegressionType.Backward)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Criterion Alpha";
                if (oinputRegression.IsUseAlpha)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });



                strItem = "Stepwise value to enter";
                if (double.IsNaN(oinputRegression.stepwiseAdd))
                    strValue = string.Empty;
                else
                    strValue = oinputRegression.stepwiseAdd.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Stepwise value to remove";
                if (double.IsNaN(oinputRegression.stepwiseDrop))
                    strValue = string.Empty;
                else
                    strValue = oinputRegression.stepwiseDrop.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "Forward value to enter";
                if (double.IsNaN(oinputRegression.forwardAdd))
                    strValue = string.Empty;
                else
                    strValue = oinputRegression.forwardAdd.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Backward value to remove";
                if (double.IsNaN(oinputRegression.backwardDrop))
                    strValue = string.Empty;
                else
                    strValue = oinputRegression.backwardDrop.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                #endregion

                #region Options
                strDialog = "Options";
                strDepth = "1";


                strItem = "Histogram of residuals";
                if (oinputRegression.IsGraphHistogramResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Normal plot of residuals";
                if (oinputRegression.IsGraphProbabilityPlotResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Residuals versus fits";
                if (oinputRegression.IsGraphResidualsVsFittedValues)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Residuals versus order";
                if (oinputRegression.IsGraphResidualsVSOrder)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Table of coefficients";
                if (oinputRegression.IsTableCoefficients)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Analysis of Variance";
                if (oinputRegression.IsTableAnova)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "R squared";
                if (oinputRegression.IsTableR_Square)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Table of fits and residuals";
                if (oinputRegression.IsTableResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });
                #endregion

                #endregion

                #region Correlation
                strMainMenu = "Correlation";
                DACrux.BStats.StatisticsInput.inputCorrelation oinputCorrelation = new DACrux.BStats.StatisticsInput.inputCorrelation(string.Empty);

                #region Basic
                strDialog = "Basic";
                strDepth = "0";

                strItem = "P-value";
                if (oinputCorrelation.IsPvalue)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Spearman Rho";
                if (oinputCorrelation.IsSpearman)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Scatter plot";
                if (oinputCorrelation.IsScatter)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Regression equation";
                if (oinputCorrelation.IsRegression)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                #endregion

                #endregion

                #region Process Capability Continuous

                strMainMenu = "CapaContinuous";
                DACrux.BStats.StatisticsInput.inputCapaContinuous oinputCapaContinuous = new DACrux.BStats.StatisticsInput.inputCapaContinuous(string.Empty);

                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Use a constant for Subgroup size";
                if (oinputCapaContinuous.IsUseContantForSubgroupSize)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Constant for Subgroup size";
                strValue = oinputCapaContinuous.SubGroupSize.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "LSL";
                if (double.IsNaN(oinputCapaContinuous.LSL))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.LSL.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "USL";
                if (double.IsNaN(oinputCapaContinuous.USL))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.USL.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Historical mean";
                if (double.IsNaN(oinputCapaContinuous.HistoricalMean))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.HistoricalMean.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Historical Std Within subgroup";
                if (double.IsNaN(oinputCapaContinuous.HistoricalStdWithin))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.HistoricalStdWithin.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Historical Std Between subgroup";
                if (double.IsNaN(oinputCapaContinuous.HistoricalStdBetween))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.HistoricalStdBetween.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                #endregion

                #region Estimation

                strDialog = "Estimation";
                strDepth = "1";

                strItem = "Use unbiasing constants within";
                if (oinputCapaContinuous.IsUseUnbiasingConstantsWithin)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Rbar";
                if (oinputCapaContinuous.EstimationWithinSubgroup == DACrux.BStats.Core.EstimationWithin.Rbar)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Sbar";
                if (oinputCapaContinuous.EstimationWithinSubgroup == DACrux.BStats.Core.EstimationWithin.Sbar)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Pooled standard deviation";
                if (oinputCapaContinuous.EstimationWithinSubgroup == DACrux.BStats.Core.EstimationWithin.Pooled_standard_deviation)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "Use moving range of length";
                strValue = oinputCapaContinuous.MovingRangeofLength.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });



                strItem = "Average moving range";
                if (oinputCapaContinuous.EstimationBetweenSubgroup == DACrux.BStats.Core.EstimationBetween.Average_moving_range)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Median moving range";
                if (oinputCapaContinuous.EstimationBetweenSubgroup == DACrux.BStats.Core.EstimationBetween.Median_moving_range)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Square root of MSSD";
                if (oinputCapaContinuous.EstimationBetweenSubgroup == DACrux.BStats.Core.EstimationBetween.Square_root_of_MSSD)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "Use unbiasing constants overall";
                if (oinputCapaContinuous.IsUseUnbiasingConstantsOverall)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });



                #endregion

                #region Options

                strDialog = "Options";
                strDepth = "1";

                strItem = "Target";
                if (double.IsNaN(oinputCapaContinuous.Target))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.Target.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "K sigma";
                strValue = oinputCapaContinuous.Ksigma.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Between/within analysis";
                if (oinputCapaContinuous.IsBetweenWithinAnalysis)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Overall analysis";
                if (oinputCapaContinuous.IsOverallAnalysis)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Display - Parts per millon";
                if (oinputCapaContinuous.ResultDisplayType == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "Display - Percents";
                if (oinputCapaContinuous.ResultDisplayType == DACrux.BStats.Core.DisplayType.Percents)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Display - Capability stats";
                if (oinputCapaContinuous.ResultStatisticType == DACrux.BStats.Core.StatisticType.Capability_stats)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Display - Benchmark Z";
                if (oinputCapaContinuous.ResultStatisticType == DACrux.BStats.Core.StatisticType.Benchmark_Z)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Include confidence intervals";
                if (oinputCapaContinuous.IsIncludeConfidenceIntervals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Confidence level";
                if (double.IsNaN(oinputCapaContinuous.ConfidenceLevel))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.ConfidenceLevel.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Confidence intervals";
                if (oinputCapaContinuous.ResultConfidenceIntervalsType == DACrux.BStats.Core.ConfidenceIntervalsType.Two_Side)
                    strValue = "Two_Side";
                else if (oinputCapaContinuous.ResultConfidenceIntervalsType == DACrux.BStats.Core.ConfidenceIntervalsType.Upper)
                    strValue = "Upper";
                else
                    strValue = "Lower";
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "User Title";
                strValue = oinputCapaContinuous.UserTitle;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });
                #endregion

                #endregion

                #region Hypothesis Testing
                strMainMenu = "Hypothesis Testing";

                DACrux.BStats.StatisticsInput.inputHypothesisTesting oinputHypothesisTesting = new DACrux.BStats.StatisticsInput.inputHypothesisTesting(string.Empty);

                #region Basic
                strDialog = "Basic";
                strDepth = "0";

                strItem = "Is One Sample T";
                if (oinputHypothesisTesting.Testing == DACrux.BStats.Core.TestingType.OneSampleT)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "Is One Sample Z";
                if (oinputHypothesisTesting.Testing == DACrux.BStats.Core.TestingType.OneSampleZ)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Is Two Paired";
                if (oinputHypothesisTesting.Testing == DACrux.BStats.Core.TestingType.TwoSamplePaired)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Is Two Unpaired";
                if (oinputHypothesisTesting.Testing == DACrux.BStats.Core.TestingType.TwoSampleUnpaired)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                #endregion

                #region Options

                strDialog = "Options";
                strDepth = "1";

                strItem = "Descriptive statistics";
                if (oinputHypothesisTesting.IsDescriptiveStatistics == true)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });



                strItem = "Confidence interval";
                if (oinputHypothesisTesting.IsConfidenceInterval == true)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });


                strItem = "Critical value";
                if (oinputHypothesisTesting.IsCriticalValue == true)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                #endregion

                #region Methods

                strDialog = "Methods";
                strDepth = "1";


                strItem = "Alternative-Less";
                if (oinputHypothesisTesting.Alternative == DACrux.BStats.Core.AlternativeType.Less)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Alternative-NotEqual";
                if (oinputHypothesisTesting.Alternative == DACrux.BStats.Core.AlternativeType.NotEqual)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Alternative-Greater";
                if (oinputHypothesisTesting.Alternative == DACrux.BStats.Core.AlternativeType.Greater)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Confidence level";
                if (double.IsNaN(oinputHypothesisTesting.ConfidenceLevel))
                    strValue = string.Empty;
                else
                    strValue = oinputHypothesisTesting.ConfidenceLevel.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Testing mean";
                if (double.IsNaN(oinputHypothesisTesting.TestingMean))
                    strValue = string.Empty;
                else
                    strValue = oinputHypothesisTesting.TestingMean.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Testing standard deviation";
                if (double.IsNaN(oinputHypothesisTesting.TestingStd))
                    strValue = string.Empty;
                else
                    strValue = oinputHypothesisTesting.TestingStd.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });

                strItem = "Testing difference";
                if (double.IsNaN(oinputHypothesisTesting.TestingDifference))
                    strValue = string.Empty;
                else
                    strValue = oinputHypothesisTesting.TestingDifference.ToString();
                dsXml.Tables[strMenuType].Rows.Add(new object[] { strMainMenu, strDialog, strDepth, strItem, strValue });
                #endregion

                #endregion

                #endregion

                dsXml.WriteXml(m_FilePath);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ Update Info ]

        #region Stat - Descriptive Statistics

        public void UpdateDescriptiveStatistics(DACrux.BStats.StatisticsInput.inputDescriptiveStatistics oinputDescriptiveStatistics)
        {
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "Descriptive Statistics";

                #region Statistics
                strDialog = "Statistics";
                strDepth = "1";


                strItem = "Mean";
                if (oinputDescriptiveStatistics.IsMean)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);
                

                strItem = "SE of mean";
                if (oinputDescriptiveStatistics.IsMeanSE)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Standard Deviation";
                if (oinputDescriptiveStatistics.IsStandarddeviation)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Variance";
                if (oinputDescriptiveStatistics.IsVariance)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Coefficient of variation";
                if (oinputDescriptiveStatistics.IsCoefficientofvariation)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Trimmed mean";
                if (oinputDescriptiveStatistics.IsTrimmedMean)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "Sum";
                if (oinputDescriptiveStatistics.IsSum)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "Minimum";
                if (oinputDescriptiveStatistics.IsMin)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Maximum";
                if (oinputDescriptiveStatistics.IsMax)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Range";
                if (oinputDescriptiveStatistics.IsRange)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "N nonmissing";
                if (oinputDescriptiveStatistics.IsNnonmissing)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "N missing";
                if (oinputDescriptiveStatistics.IsNmissing)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "N total";
                if (oinputDescriptiveStatistics.IsNtotal)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Cumulative N";
                if (oinputDescriptiveStatistics.IsCumulativeN)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Percent";
                if (oinputDescriptiveStatistics.IsPercent)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Cumulative percent";
                if (oinputDescriptiveStatistics.IsCumulativePercent)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "First quartile";
                if (oinputDescriptiveStatistics.IsQ1)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Median";
                if (oinputDescriptiveStatistics.IsMedian)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Third quarile";
                if (oinputDescriptiveStatistics.IsQ3)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Interquartile range";
                if (oinputDescriptiveStatistics.IsInterquartileRange)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Mode";
                if (oinputDescriptiveStatistics.IsMode)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Sum of Squares";
                if (oinputDescriptiveStatistics.IsSumofSquares)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Skewness";
                if (oinputDescriptiveStatistics.IsSkewness)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Kurtosis";
                if (oinputDescriptiveStatistics.IsKurtosis)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "MSSD";
                if (oinputDescriptiveStatistics.IsMSSD)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                #endregion

                #region Graphs

                strDialog = "Graphs";
                strDepth = "1";

                strItem = "Histogram";
                if (oinputDescriptiveStatistics.IsHistogram)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "HistogramN with Normal Curve";
                if (oinputDescriptiveStatistics.IsHistogramNNormalCurve)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Box Plot";
                if (oinputDescriptiveStatistics.IsBoxPlot)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);
                #endregion

                dsXml.WriteXml(m_FilePath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Stat - ANOVA

        public void UpdateAnova(DACrux.BStats.StatisticsInput.inputANOVA oinputAnova)
        {
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "ANOVA";


                #region Basic

                strDialog = "Basic";
                strDepth = "0";


                strItem = "Is Single Column";

                if (oinputAnova.IsSingle)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                #endregion

                #region Graph

                strDialog = "Graph";
                strDepth = "1";


                strItem = "Histogram of residuals";

                if (oinputAnova.IsGraphHistogramResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);




                strItem = "Normal plot of residuals";

                if (oinputAnova.IsGraphProbabilityPlotResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Residuals versus fits";
                if (oinputAnova.IsGraphResidualsVsFittedValues)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Boxplots of data";
                if (oinputAnova.IsBoxPlot)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                #endregion

                dsXml.WriteXml(m_FilePath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dsXml != null)
                    dsXml.Dispose();
                dsXml = null;
            }
        }

        #endregion

        #region Stat - Regression
        public void UpdateRegression(DACrux.BStats.StatisticsInput.inputRegression oinputRegression)
        {
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "Regression";

                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Fit intercept";
                if (oinputRegression.IsIntercept)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                #endregion

                #region Methods
                strDialog = "Methods";
                strDepth = "1";

                strItem = "Selection All";
                if (oinputRegression.ReAnalysisType == DACrux.BStats.Core.RegressionType.All)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Selection Stepwise";
                if (oinputRegression.ReAnalysisType == DACrux.BStats.Core.RegressionType.Stepwise)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Selection Forward";
                if (oinputRegression.ReAnalysisType == DACrux.BStats.Core.RegressionType.Forward)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "Selection Backward";
                if (oinputRegression.ReAnalysisType == DACrux.BStats.Core.RegressionType.Backward)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Criterion Alpha";
                if (oinputRegression.IsUseAlpha)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);



                strItem = "Stepwise value to enter";
                if (double.IsNaN(oinputRegression.stepwiseAdd))
                    strValue = string.Empty;
                else
                    strValue = oinputRegression.stepwiseAdd.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Stepwise value to remove";
                if (double.IsNaN(oinputRegression.stepwiseDrop))
                    strValue = string.Empty;
                else
                    strValue = oinputRegression.stepwiseDrop.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "Forward value to enter";
                if (double.IsNaN(oinputRegression.forwardAdd))
                    strValue = string.Empty;
                else
                    strValue = oinputRegression.forwardAdd.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Backward value to remove";
                if (double.IsNaN(oinputRegression.backwardDrop))
                    strValue = string.Empty;
                else
                    strValue = oinputRegression.backwardDrop.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                #endregion

                #region Options
                strDialog = "Options";
                strDepth = "1";


                strItem = "Histogram of residuals";
                if (oinputRegression.IsGraphHistogramResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Normal plot of residuals";
                if (oinputRegression.IsGraphProbabilityPlotResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Residuals versus fits";
                if (oinputRegression.IsGraphResidualsVsFittedValues)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Residuals versus order";
                if (oinputRegression.IsGraphResidualsVSOrder)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Table of coefficients";
                if (oinputRegression.IsTableCoefficients)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Analysis of Variance";
                if (oinputRegression.IsTableAnova)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "R squared";
                if (oinputRegression.IsTableR_Square)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Table of fits and residuals";
                if (oinputRegression.IsTableResiduals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);
                #endregion

                dsXml.WriteXml(m_FilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Stat - Correlation
        public void UpdateCorrelation(DACrux.BStats.StatisticsInput.inputCorrelation oinputCorrelation)
        {
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "Correlation";

                strDialog = "Basic";
                strDepth = "0";

                strItem = "P-value";
                if (oinputCorrelation.IsPvalue)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Spearman Rho";
                if (oinputCorrelation.IsSpearman)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Scatter plot";
                if (oinputCorrelation.IsScatter)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Regression equation";
                if (oinputCorrelation.IsRegression)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                
                dsXml.WriteXml(m_FilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Stat - Process Capability Continuous

        public void UpdateCapaContinuous(DACrux.BStats.StatisticsInput.inputCapaContinuous oinputCapaContinuous)
        {
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "CapaContinuous";


                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Use a constant for Subgroup size";
                if (oinputCapaContinuous.IsUseContantForSubgroupSize)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Constant for Subgroup size";
                strValue = oinputCapaContinuous.SubGroupSize.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "LSL";
                if (double.IsNaN(oinputCapaContinuous.LSL))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.LSL.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "USL";
                if (double.IsNaN(oinputCapaContinuous.USL))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.USL.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Historical mean";
                if (double.IsNaN(oinputCapaContinuous.HistoricalMean))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.HistoricalMean.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Historical Std Within subgroup";
                if (double.IsNaN(oinputCapaContinuous.HistoricalStdWithin))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.HistoricalStdWithin.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Historical Std Between subgroup";
                if (double.IsNaN(oinputCapaContinuous.HistoricalStdBetween))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.HistoricalStdBetween.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                #endregion

                #region Estimation

                strDialog = "Estimation";
                strDepth = "1";

                strItem = "Use unbiasing constants within";
                if (oinputCapaContinuous.IsUseUnbiasingConstantsWithin)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Rbar";
                if (oinputCapaContinuous.EstimationWithinSubgroup == DACrux.BStats.Core.EstimationWithin.Rbar)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Sbar";
                if (oinputCapaContinuous.EstimationWithinSubgroup == DACrux.BStats.Core.EstimationWithin.Sbar)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Pooled standard deviation";
                if (oinputCapaContinuous.EstimationWithinSubgroup == DACrux.BStats.Core.EstimationWithin.Pooled_standard_deviation)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "Use moving range of length";
                strValue = oinputCapaContinuous.MovingRangeofLength.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);



                strItem = "Average moving range";
                if (oinputCapaContinuous.EstimationBetweenSubgroup == DACrux.BStats.Core.EstimationBetween.Average_moving_range)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Median moving range";
                if (oinputCapaContinuous.EstimationBetweenSubgroup == DACrux.BStats.Core.EstimationBetween.Median_moving_range)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Square root of MSSD";
                if (oinputCapaContinuous.EstimationBetweenSubgroup == DACrux.BStats.Core.EstimationBetween.Square_root_of_MSSD)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "Use unbiasing constants overall";
                if (oinputCapaContinuous.IsUseUnbiasingConstantsOverall)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);



                #endregion

                #region Options

                strDialog = "Options";
                strDepth = "1";

                strItem = "Target";
                if (double.IsNaN(oinputCapaContinuous.Target))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.Target.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "K sigma";
                strValue = oinputCapaContinuous.Ksigma.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Between/within analysis";
                if (oinputCapaContinuous.IsBetweenWithinAnalysis)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Overall analysis";
                if (oinputCapaContinuous.IsOverallAnalysis)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Display - Parts per millon";
                if (oinputCapaContinuous.ResultDisplayType == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "Display - Percents";
                if (oinputCapaContinuous.ResultDisplayType == DACrux.BStats.Core.DisplayType.Percents)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Display - Capability stats";
                if (oinputCapaContinuous.ResultStatisticType == DACrux.BStats.Core.StatisticType.Capability_stats)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Display - Benchmark Z";
                if (oinputCapaContinuous.ResultStatisticType == DACrux.BStats.Core.StatisticType.Benchmark_Z)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Include confidence intervals";
                if (oinputCapaContinuous.IsIncludeConfidenceIntervals)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Confidence level";
                if (double.IsNaN(oinputCapaContinuous.ConfidenceLevel))
                    strValue = string.Empty;
                else
                    strValue = oinputCapaContinuous.ConfidenceLevel.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Confidence intervals";
                if (oinputCapaContinuous.ResultConfidenceIntervalsType == DACrux.BStats.Core.ConfidenceIntervalsType.Two_Side)
                    strValue = "Two_Side";
                else if (oinputCapaContinuous.ResultConfidenceIntervalsType == DACrux.BStats.Core.ConfidenceIntervalsType.Upper)
                    strValue = "Upper";
                else
                    strValue = "Lower";
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "User Title";
                strValue = oinputCapaContinuous.UserTitle;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);
                #endregion

                dsXml.WriteXml(m_FilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Stat - Hypothesis Testing

        public void UpdateHypothesisTesting(DACrux.BStats.StatisticsInput.inputHypothesisTesting oInputHypothesisTesting)
        {
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "Hypothesis Testing";


                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Is One Sample T";
                if (oInputHypothesisTesting.Testing == DACrux.BStats.Core.TestingType.OneSampleT)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);
                

                strItem = "Is One Sample Z";
                if (oInputHypothesisTesting.Testing == DACrux.BStats.Core.TestingType.OneSampleZ)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Is Two Paired";
                if (oInputHypothesisTesting.Testing == DACrux.BStats.Core.TestingType.TwoSamplePaired)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Is Two Unpaired";
                if (oInputHypothesisTesting.Testing == DACrux.BStats.Core.TestingType.TwoSampleUnpaired)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                #endregion

                #region Options

                strDialog = "Options";
                strDepth = "1";

                strItem = "Descriptive statistics";
                if (oInputHypothesisTesting.IsDescriptiveStatistics == true)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                

                strItem = "Confidence interval";
                if (oInputHypothesisTesting.IsConfidenceInterval == true)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);


                strItem = "Critical value";
                if (oInputHypothesisTesting.IsCriticalValue == true)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                #endregion

                #region Methods

                strDialog = "Methods";
                strDepth = "1";


                strItem = "Alternative-Less";
                if (oInputHypothesisTesting.Alternative == DACrux.BStats.Core.AlternativeType.Less)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Alternative-NotEqual";
                if (oInputHypothesisTesting.Alternative == DACrux.BStats.Core.AlternativeType.NotEqual)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Alternative-Greater";
                if (oInputHypothesisTesting.Alternative == DACrux.BStats.Core.AlternativeType.Greater)
                    strValue = m_Yes;
                else
                    strValue = m_No;
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Confidence level";
                if (double.IsNaN(oInputHypothesisTesting.ConfidenceLevel))
                    strValue = string.Empty;
                else
                    strValue = oInputHypothesisTesting.ConfidenceLevel.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Testing mean";
                if (double.IsNaN(oInputHypothesisTesting.TestingMean))
                    strValue = string.Empty;
                else
                    strValue = oInputHypothesisTesting.TestingMean.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Testing standard deviation";
                if (double.IsNaN(oInputHypothesisTesting.TestingStd))
                    strValue = string.Empty;
                else
                    strValue = oInputHypothesisTesting.TestingStd.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);

                strItem = "Testing difference";
                if (double.IsNaN(oInputHypothesisTesting.TestingDifference))
                    strValue = string.Empty;
                else
                    strValue = oInputHypothesisTesting.TestingDifference.ToString();
                UpdateValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem, strValue);
                #endregion

                dsXml.WriteXml(m_FilePath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dsXml != null)
                    dsXml.Dispose();
                dsXml = null;
            }
        }

        #endregion


        #endregion

        #region [ Get Info ]

        #region Stat - Descriptive Statistics

        public DACrux.BStats.StatisticsInput.inputDescriptiveStatistics GetDescriptiveStatistics()
        {
            DACrux.BStats.StatisticsInput.inputDescriptiveStatistics oReturn = null;
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                oReturn = new DACrux.BStats.StatisticsInput.inputDescriptiveStatistics("");
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "Descriptive Statistics";

                if (!CheckValue(dsXml, strMenuType, strMainMenu))
                {
                    if (dsXml != null)
                        dsXml.Dispose();
                    dsXml = null;
                    InitializeOption();
                    dsXml = new DataSet();
                    dsXml.ReadXml(m_FilePath);
                }

                #region Statistics
                strDialog = "Statistics";
                strDepth = "1";


                strItem = "Mean";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsMean = true;
                else
                    oReturn.IsMean = false;

                strItem = "SE of mean";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsMeanSE = true;
                else
                    oReturn.IsMeanSE = false;

                strItem = "Standard Deviation";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsStandarddeviation = true;
                else
                    oReturn.IsStandarddeviation = false;

                strItem = "Variance";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsVariance = true;
                else
                    oReturn.IsVariance = false;

                strItem = "Coefficient of variation";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsCoefficientofvariation = true;
                else
                    oReturn.IsCoefficientofvariation = false;

                strItem = "Trimmed mean";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsTrimmedMean = true;
                else
                    oReturn.IsTrimmedMean = false;


                strItem = "Sum";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsSum = true;
                else
                    oReturn.IsSum = false;

                strItem = "Minimum";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsMin = true;
                else
                    oReturn.IsMin = false;


                strItem = "Maximum";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsMax = true;
                else
                    oReturn.IsMax = false;

                strItem = "Range";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsRange = true;
                else
                    oReturn.IsRange = false;

                strItem = "N nonmissing";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsNnonmissing = true;
                else
                    oReturn.IsNnonmissing = false;

                strItem = "N missing";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsNmissing = true;
                else
                    oReturn.IsNmissing = false;

                strItem = "N total";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsNtotal = true;
                else
                    oReturn.IsNtotal = false;


                strItem = "Cumulative N";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsCumulativeN = true;
                else
                    oReturn.IsCumulativeN = false;

                strItem = "Percent";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsPercent = true;
                else
                    oReturn.IsPercent = false;


                strItem = "Cumulative percent";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsCumulativePercent = true;
                else
                    oReturn.IsCumulativePercent = false;

                strItem = "First quartile";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsQ1 = true;
                else
                    oReturn.IsQ1 = false;

                strItem = "Median";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsMedian = true;
                else
                    oReturn.IsMedian = false;

                strItem = "Third quarile";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsQ3 = true;
                else
                    oReturn.IsQ3 = false;


                strItem = "Interquartile range";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsInterquartileRange = true;
                else
                    oReturn.IsInterquartileRange = false;

                strItem = "Mode";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsMode = true;
                else
                    oReturn.IsMode = false;

                strItem = "Sum of Squares";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsSumofSquares = true;
                else
                    oReturn.IsSumofSquares = false;

                strItem = "Skewness";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsSkewness = true;
                else
                    oReturn.IsSkewness = false;

                strItem = "Kurtosis";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsKurtosis = true;
                else
                    oReturn.IsKurtosis = false;

                strItem = "MSSD";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsMSSD = true;
                else
                    oReturn.IsMSSD = false;

                #endregion

                #region Graphs

                strDialog = "Graphs";
                strDepth = "1";

                strItem = "Histogram";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsHistogram = true;
                else
                    oReturn.IsHistogram = false;

                strItem = "HistogramN with Normal Curve";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsHistogramNNormalCurve = true;
                else
                    oReturn.IsHistogramNNormalCurve = false;

                strItem = "Box Plot";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsBoxPlot = true;
                else
                    oReturn.IsBoxPlot = false;

                #endregion

                return oReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Stat - ANOVA
        public DACrux.BStats.StatisticsInput.inputANOVA GetAnova()
        {
            DACrux.BStats.StatisticsInput.inputANOVA oReturn = null;
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                oReturn = new DACrux.BStats.StatisticsInput.inputANOVA("");
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "ANOVA";

                if (!CheckValue(dsXml, strMenuType, strMainMenu))
                {
                    if (dsXml != null)
                        dsXml.Dispose();
                    dsXml = null;
                    InitializeOption();
                    dsXml = new DataSet();
                    dsXml.ReadXml(m_FilePath);
                }
                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Is Single Column";

                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsSingle = true;
                else
                    oReturn.IsSingle = false;

                #endregion


                #region Graph
                strDialog = "Graph";
                strDepth = "1";


                strItem = "Histogram of residuals";

                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsGraphHistogramResiduals = true;
                else
                    oReturn.IsGraphHistogramResiduals = false;


                strItem = "Normal plot of residuals";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsGraphProbabilityPlotResiduals = true;
                else
                    oReturn.IsGraphProbabilityPlotResiduals = false;

                strItem = "Residuals versus fits";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsGraphResidualsVsFittedValues = true;
                else
                    oReturn.IsGraphResidualsVsFittedValues = false;

               

                strItem = "Boxplots of data";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsBoxPlot = true;
                else
                    oReturn.IsBoxPlot = false;

                

                #endregion

                return oReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Stat - Regression
        public DACrux.BStats.StatisticsInput.inputRegression GetRegression()
        {
            DACrux.BStats.StatisticsInput.inputRegression oReturn = null;
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                oReturn = new DACrux.BStats.StatisticsInput.inputRegression("");
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "Regression";

                if (!CheckValue(dsXml, strMenuType, strMainMenu))
                {
                    if(dsXml != null)
                        dsXml.Dispose();
                    dsXml = null;
                    InitializeOption();
                    dsXml = new DataSet();
                    dsXml.ReadXml(m_FilePath);
                }
                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Fit intercept";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsIntercept = true;
                else
                    oReturn.IsIntercept = false;
                
                #endregion

                #region Methods
                strDialog = "Methods";
                strDepth = "1";

                strItem = "Selection All";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.ReAnalysisType = DACrux.BStats.Core.RegressionType.All;


                strItem = "Selection Stepwise";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.ReAnalysisType = DACrux.BStats.Core.RegressionType.Stepwise;

                strItem = "Selection Forward";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.ReAnalysisType = DACrux.BStats.Core.RegressionType.Forward;


                strItem = "Selection Backward";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.ReAnalysisType = DACrux.BStats.Core.RegressionType.Backward;

                strItem = "Criterion Alpha";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsUseAlpha = true;
                else
                    oReturn.IsUseAlpha = false;


                strItem = "Stepwise value to enter";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.stepwiseAdd = double.NaN;
                else
                    oReturn.stepwiseAdd = Convert.ToDouble(strValue);


                strItem = "Stepwise value to remove";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.stepwiseDrop = double.NaN;
                else
                    oReturn.stepwiseDrop = Convert.ToDouble(strValue);


                strItem = "Forward value to enter";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.forwardAdd = double.NaN;
                else
                    oReturn.forwardAdd = Convert.ToDouble(strValue);

                strItem = "Backward value to remove";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.backwardDrop = double.NaN;
                else
                    oReturn.backwardDrop = Convert.ToDouble(strValue);



                #endregion


                #region Options
                strDialog = "Options";
                strDepth = "1";

                strItem = "Histogram of residuals";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsGraphHistogramResiduals = true;
                else
                    oReturn.IsGraphHistogramResiduals = false;


                strItem = "Normal plot of residuals";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsGraphProbabilityPlotResiduals = true;
                else
                    oReturn.IsGraphProbabilityPlotResiduals = false;

                strItem = "Residuals versus fits";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsGraphResidualsVsFittedValues = true;
                else
                    oReturn.IsGraphResidualsVsFittedValues = false;

                strItem = "Residuals versus order";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsGraphResidualsVSOrder = true;
                else
                    oReturn.IsGraphResidualsVSOrder = false;

                strItem = "Table of coefficients";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsTableCoefficients = true;
                else
                    oReturn.IsTableCoefficients = false;

                strItem = "Analysis of Variance";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsTableAnova = true;
                else
                    oReturn.IsTableAnova = false;

                strItem = "R squared";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsTableR_Square = true;
                else
                    oReturn.IsTableR_Square = false;

                strItem = "Table of fits and residuals";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsTableResiduals = true;
                else
                    oReturn.IsTableResiduals = false;

                #endregion

                return oReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Stat - Correlation
        public DACrux.BStats.StatisticsInput.inputCorrelation GetCorrelation()
        {
            DACrux.BStats.StatisticsInput.inputCorrelation oReturn = null;
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                oReturn = new DACrux.BStats.StatisticsInput.inputCorrelation("");
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "Correlation";

                if (!CheckValue(dsXml, strMenuType, strMainMenu))
                {
                    if (dsXml != null)
                        dsXml.Dispose();
                    dsXml = null;
                    InitializeOption();
                    dsXml = new DataSet();
                    dsXml.ReadXml(m_FilePath);
                }

                strDialog = "Basic";
                strDepth = "0";
                strItem = "P-value";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsPvalue = true;
                else
                    oReturn.IsPvalue = false;

                
                strItem = "Spearman Rho";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsSpearman = true;
                else
                    oReturn.IsSpearman = false;

                strItem = "Scatter plot";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsScatter = true;
                else
                    oReturn.IsScatter = false;

                strItem = "Regression equation";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsRegression = true;
                else
                    oReturn.IsRegression = false;

                return oReturn;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Stat - Process Capability Continuous

        public DACrux.BStats.StatisticsInput.inputCapaContinuous GetCapaContinuous()
        {
            DACrux.BStats.StatisticsInput.inputCapaContinuous oReturn = null;
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                oReturn = new DACrux.BStats.StatisticsInput.inputCapaContinuous("");
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "CapaContinuous";

                if (!CheckValue(dsXml, strMenuType, strMainMenu))
                {
                    if (dsXml != null)
                        dsXml.Dispose();
                    dsXml = null;
                    InitializeOption();
                    dsXml = new DataSet();
                    dsXml.ReadXml(m_FilePath);
                }


                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Use a constant for Subgroup size";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsUseContantForSubgroupSize = true;
                else
                    oReturn.IsUseContantForSubgroupSize = false;


                strItem = "Constant for Subgroup size";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                oReturn.SubGroupSize = Convert.ToInt32(strValue);


                strItem = "LSL";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.LSL = double.NaN;
                else
                    oReturn.LSL = Convert.ToDouble(strValue);

                strItem = "USL";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.USL = double.NaN;
                else
                    oReturn.USL = Convert.ToDouble(strValue);

                strItem = "Historical mean";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.HistoricalMean = double.NaN;
                else
                    oReturn.HistoricalMean = Convert.ToDouble(strValue);

                strItem = "Historical Std Within subgroup";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.HistoricalStdWithin = double.NaN;
                else
                    oReturn.HistoricalStdWithin = Convert.ToDouble(strValue);

                strItem = "Historical Std Between subgroup";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.HistoricalStdBetween = double.NaN;
                else
                    oReturn.HistoricalStdBetween = Convert.ToDouble(strValue);

                #endregion

                #region Estimation

                strDialog = "Estimation";
                strDepth = "1";

                strItem = "Use unbiasing constants within";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsUseUnbiasingConstantsWithin = true;
                else
                    oReturn.IsUseUnbiasingConstantsWithin = false;

                strItem = "Rbar";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.EstimationWithinSubgroup = DACrux.BStats.Core.EstimationWithin.Rbar;

                strItem = "Sbar";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.EstimationWithinSubgroup = DACrux.BStats.Core.EstimationWithin.Sbar;

                strItem = "Pooled standard deviation";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.EstimationWithinSubgroup = DACrux.BStats.Core.EstimationWithin.Pooled_standard_deviation;


                strItem = "Use moving range of length";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                oReturn.MovingRangeofLength = Convert.ToInt32(strValue);


                strItem = "Average moving range";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.EstimationBetweenSubgroup = DACrux.BStats.Core.EstimationBetween.Average_moving_range;

                strItem = "Median moving range";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.EstimationBetweenSubgroup = DACrux.BStats.Core.EstimationBetween.Median_moving_range;

                strItem = "Square root of MSSD";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.EstimationBetweenSubgroup = DACrux.BStats.Core.EstimationBetween.Square_root_of_MSSD;



                strItem = "Use unbiasing constants overall";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsUseUnbiasingConstantsOverall = true;
                else
                    oReturn.IsUseUnbiasingConstantsOverall = false;

                #endregion

                #region Options

                strDialog = "Options";
                strDepth = "1";

                strItem = "Target";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.Target = double.NaN;
                else
                    oReturn.Target = Convert.ToDouble(strValue);

                strItem = "K sigma";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                oReturn.Ksigma = Convert.ToDouble(strValue);


                strItem = "Between/within analysis";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsBetweenWithinAnalysis = true;
                else
                    oReturn.IsBetweenWithinAnalysis = false;

                strItem = "Overall analysis";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsOverallAnalysis = true;
                else
                    oReturn.IsOverallAnalysis = false;


                strItem = "Display - Parts per millon";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.ResultDisplayType = DACrux.BStats.Core.DisplayType.Parts_per_million;
                else
                    oReturn.ResultDisplayType = DACrux.BStats.Core.DisplayType.Percents;


                //strItem = "Display - Percents";  // Display - Parts per millon에서 구현

                strItem = "Display - Capability stats";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.ResultStatisticType = DACrux.BStats.Core.StatisticType.Capability_stats;
                else
                    oReturn.ResultStatisticType = DACrux.BStats.Core.StatisticType.Benchmark_Z;

                //strItem = "Display - Benchmark Z"; // Display - Capability stats에서 구현


                strItem = "Include confidence intervals";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsIncludeConfidenceIntervals = true;
                else
                    oReturn.IsIncludeConfidenceIntervals = false;


                strItem = "Confidence level";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.ConfidenceLevel = double.NaN;
                else
                    oReturn.ConfidenceLevel = Convert.ToDouble(strValue);

                strItem = "Confidence intervals";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == "Two_Side")
                    oReturn.ResultConfidenceIntervalsType = DACrux.BStats.Core.ConfidenceIntervalsType.Two_Side;
                else if (strValue == "Upper")
                    oReturn.ResultConfidenceIntervalsType = DACrux.BStats.Core.ConfidenceIntervalsType.Upper;
                else
                    oReturn.ResultConfidenceIntervalsType = DACrux.BStats.Core.ConfidenceIntervalsType.Lower;


                strItem = "User Title";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                oReturn.UserTitle = strValue;
                #endregion

                return oReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Stat - Hypothesis Testing
        public DACrux.BStats.StatisticsInput.inputHypothesisTesting GetHypothesisTesting()
        {
            DACrux.BStats.StatisticsInput.inputHypothesisTesting oReturn = null;
            DataSet dsXml = null;
            string strMenuType = string.Empty;  // DataTable Name
            string strMainMenu = string.Empty;  // Column0
            string strDialog = string.Empty;  // Column1
            string strDepth = string.Empty;  // Column2
            string strItem = string.Empty;  // Column3
            string strValue = string.Empty;  // Column4
            try
            {
                if (!File.Exists(m_FilePath))
                    InitializeOption();
                oReturn = new DACrux.BStats.StatisticsInput.inputHypothesisTesting("");
                dsXml = new DataSet();
                dsXml.ReadXml(m_FilePath);
                strMenuType = "STATMENU";
                strMainMenu = "Hypothesis Testing";

                if (!CheckValue(dsXml, strMenuType, strMainMenu))
                {
                    if (dsXml != null)
                        dsXml.Dispose();
                    dsXml = null;
                    InitializeOption();
                    dsXml = new DataSet();
                    dsXml.ReadXml(m_FilePath);
                }
                #region Basic

                strDialog = "Basic";
                strDepth = "0";

                strItem = "Is One Sample T";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.Testing = DACrux.BStats.Core.TestingType.OneSampleT;

                strItem = "Is One Sample Z";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.Testing = DACrux.BStats.Core.TestingType.OneSampleZ;

                strItem = "Is Two Paired";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.Testing = DACrux.BStats.Core.TestingType.TwoSamplePaired;

                strItem = "Is Two Unpaired";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.Testing = DACrux.BStats.Core.TestingType.TwoSampleUnpaired;

                #endregion

                #region Options

                strDialog = "Options";
                strDepth = "1";

                strItem = "Descriptive statistics";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsDescriptiveStatistics = true;
                else
                    oReturn.IsDescriptiveStatistics = false;

                strItem = "Confidence interval";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsConfidenceInterval = true;
                else
                    oReturn.IsConfidenceInterval = false;


                strItem = "Critical value";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.IsCriticalValue = true;
                else
                    oReturn.IsCriticalValue = false;

                #endregion

                #region Methods

                strDialog = "Methods";
                strDepth = "1";


                strItem = "Alternative-Less";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.Alternative = DACrux.BStats.Core.AlternativeType.Less;

                strItem = "Alternative-NotEqual";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.Alternative = DACrux.BStats.Core.AlternativeType.NotEqual;

                strItem = "Alternative-Greater";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == m_Yes)
                    oReturn.Alternative = DACrux.BStats.Core.AlternativeType.Greater;

                strItem = "Confidence level";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.ConfidenceLevel = double.NaN;
                else
                    oReturn.ConfidenceLevel = Convert.ToDouble(strValue);

                strItem = "Testing mean";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.TestingMean = double.NaN;
                else
                    oReturn.TestingMean = Convert.ToDouble(strValue);

                strItem = "Testing standard deviation";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.TestingStd = double.NaN;
                else
                    oReturn.TestingStd = Convert.ToDouble(strValue);

                strItem = "Testing difference";
                strValue = GetValue(dsXml, strMenuType, strMainMenu, strDialog, strDepth, strItem);
                if (strValue == string.Empty)
                    oReturn.TestingDifference = double.NaN;
                else
                    oReturn.TestingDifference = Convert.ToDouble(strValue);


                #endregion

                return oReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region [ Common Method ]

        #region Check Menu
        private bool CheckValue(DataSet dsXml, string strMenuType, string strMainMenu)
        {
            DataRow[] arrTemp = null;
            try
            {
                arrTemp = dsXml.Tables[strMenuType].Select(string.Format("MAINMENU = '{0}'", strMainMenu));
                if (arrTemp != null && arrTemp.Length > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Value
        private string GetValue(DataSet dsXml, string strMenuType, string strMainMenu, string strDialog, string strDepth, string strItem)
        {
            DataRow[] arrTemp = null;
            try
            {
                arrTemp = dsXml.Tables[strMenuType].Select(string.Format("MAINMENU = '{0}' AND DIALOG = '{1}' AND DEPTH = '{2}' AND ITEM = '{3}'", strMainMenu, strDialog, strDepth, strItem));
                if (arrTemp != null && arrTemp.Length > 0)
                    return arrTemp[0]["VALUE"].ToString();
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        #region Update Value
        private void UpdateValue(DataSet dsXml, string strMenuType, string strMainMenu, string strDialog, string strDepth, string strItem, string strValue)
        {
            try
            {
                dsXml.Tables[strMenuType].Select(string.Format("MAINMENU = '{0}' AND DIALOG = '{1}' AND DEPTH = '{2}' AND ITEM = '{3}'", strMainMenu, strDialog, strDepth, strItem))[0]["VALUE"] = strValue;
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete Xml File
        public void DeleteXmlFile()
        {
            try
            {
                if (File.Exists(m_FilePath))
                    File.Delete(m_FilePath);
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #endregion
    }
}
