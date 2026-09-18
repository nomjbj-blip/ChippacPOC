using System;
using System.Collections.Generic;
using System.Data;
using CenterSpace.NMath.Stats;

namespace DACrux.BStats.Core
{
    public class OneWayANOVA
    {
        #region " MEMBER FIELD "

        private DataTable m_dtSource;
        int m_iDecimalInner = 4;
        int m_iDecimalOuter = 4;
        SelectedAnova m_InputOption;
        ResultAnova m_Result;



        #region [ LANGUAGE ]

        string m_DF = "DF";
        string m_SumofSquares = "SumOfSquares";
        string m_MeanSquare = "MeanSquare";
        string m_F = "F";
        string m_RSquare = "R-Square";
        string m_AdjRSquare = "Adj.R-Square";
        string m_RootMse = " Root(MS Error)";
        string m_Source = "Source";
        //string m_Estimates = "Estimates";
        //string m_StandardError = "StandardError";
        string m_Pvalue = "P-Value";
        string m_FittedValue = "Fitted value";
        string m_Residual = "Residual";

        string m_N = "N";
        string m_Mean = "Mean";
        string m_Standarddeviation = "StDev";
        string m_Min = "Minimum";
        string m_Max = "Maximum";

        string m_Level = "Level";
        string m_Factor = "Group(Factor)";
        string m_Error = "Error";
        string m_Total = "Total";

        #region Graph Title
        string m_Statistics = "Basic statistics";
        string m_BoxPlot = "Boxplot of value by group";
        string m_GraphHistogram = "Histogram of residuals";
        string m_GraphProbability = "Probability Plot of the Residuals";
        string m_GraphFit = "Residuals Versus the Fitted Values";
        #endregion
        

        string m_AnovaTitle = "Analysis of Variance";

       

        #region Message
        string m_ErrorData = "Source is null.";
        string m_ErrorDataLength = "Length of data is 0.";
        //string m_ErrorDataNotEnough = "Not enough data in column.";
        string m_ErrorFewItems = "Too few items. \r\n Use 2 or more numeric columns.";
        string m_ErrorSystemValidRowIndices = "ValidRowIndices Info miss matching";
        string m_ErrorFewLevels = "There must be at least two levels.";
        string m_ErrorNotSelected = "Too few items.";
        string m_ErrorSameColumn = "Selected same columns.";
        #endregion

        #endregion

        #endregion

        #region " CREATOR "
        public OneWayANOVA(DataTable dtSource, SelectedAnova InputOption)
        {
            try
            {
                m_dtSource = dtSource;
                m_InputOption = InputOption;

                m_Result = Analysis(dtSource, InputOption);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region " PROPERTY "

        public ResultAnova Result
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

        private ResultAnova Analysis(DataTable dtSource, SelectedAnova InputOption)
        {
            //dtSource의 Rows Count
            int iRowCnt;
            ResultAnova oReturn = new ResultAnova();
            DataSet dsDes = null; //그룹별 기술통계를 위한...
            OneWayAnova oAnova;
            try
            {
                if (dtSource == null)
                    throw new ArgumentException(m_ErrorData);
                iRowCnt = dtSource.Rows.Count;
                if (iRowCnt < 1)
                    throw new ArgumentException(m_ErrorDataLength, new Exception("MIRACOM"));


                m_Result.Reset();

                if (InputOption.IsSingle)  // Sigle column
                {
                    DataTable dtCopy = dtSource.Copy();
                    if (InputOption.SingleColumn < 0 || InputOption.GroupColumn < 0)
                    {
                        throw new ArgumentException(m_ErrorNotSelected, new Exception("MIRACOM"));
                    }
                    if (InputOption.SingleColumn == InputOption.GroupColumn)
                    {
                        throw new ArgumentException(m_ErrorSameColumn, new Exception("MIRACOM"));
                    }
                    dsDes = Conversion(ref dtCopy, InputOption.SingleColumn, InputOption.GroupColumn);
                    if (dsDes == null || dsDes.Tables.Count < 2)
                    {
                        oReturn.ErrorInformation = m_ErrorFewLevels;
                        return oReturn;
                    }
                    oAnova = new OneWayAnova(new DataFrame(dtCopy), InputOption.GroupColumn, InputOption.SingleColumn);

                }
                else
                {
                    if (InputOption.arrVariables == null || InputOption.arrVariables.Length < 2)
                    {
                        throw new ArgumentException(m_ErrorFewItems, new Exception("MIRACOM"));
                    }
                    if (InputOption.arrValidRowIndices == null || InputOption.arrValidRowIndices.Length < 1)
                    {
                        throw new ArgumentException(m_ErrorSystemValidRowIndices);
                    }
                    if (InputOption.arrVariables.Length != InputOption.arrValidRowIndices.Length)
                    {
                        throw new ArgumentException(m_ErrorSystemValidRowIndices);
                    }
                   
                    dsDes = Conversion(dtSource, InputOption.arrVariables, InputOption.arrValidRowIndices);
                    oAnova = new OneWayAnova(new DataFrame(dtSource).ToDoubleMatrix());
                }
                

                oReturn.TableofAnova = GetTableofAnova(oAnova);
                oReturn.TableofRsquared = GetTableofRsquared(oAnova);
                oReturn.TableofStatistic = GetTableofStatistic(dsDes);
                oReturn.TableforGraphs = GetTableforGraphs(dsDes, InputOption.IsSingle, InputOption.oGraph.IsBoxPlot, InputOption.oGraph.IsGraphHistogramResiduals, InputOption.oGraph.IsGraphProbabilityPlotResiduals, InputOption.oGraph.IsGraphResidualsVsFittedValues);
                oReturn.GroupTitle = "One-way ANOVA: ";
                for (int i = 0; i < dsDes.Tables.Count; i++)
                {
                    if (i == 0)
                        oReturn.GroupTitle += dsDes.Tables[i].TableName;
                    else
                        oReturn.GroupTitle += ", " + dsDes.Tables[i].TableName ;
                }


                return oReturn;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message == "MIRACOM")
                    {
                        ResultAnova oExReturn = new ResultAnova();
                        oExReturn.ErrorInformation = ex.Message;
                        return oExReturn;
                    }
                    else
                        throw ex;
                }
                else
                    throw ex;
            }
        }


        #region 분석결과

        private DataTable GetTableofAnova(OneWayAnova oAnova)
        {
            DataTable dtReturn;
            try
            {
                dtReturn = new DataTable(m_AnovaTitle);

                dtReturn.Columns.Add(m_Source);
                dtReturn.Columns.Add(m_DF);
                dtReturn.Columns.Add(m_SumofSquares);
                dtReturn.Columns.Add(m_MeanSquare);
                dtReturn.Columns.Add(m_F);
                dtReturn.Columns.Add(m_Pvalue);

                object[] oRow = new object[6];

                // 모형 
                oRow[0] = m_Factor;
                oRow[1] = oAnova.AnovaTable.DegreesOfFreedomBetween;				// 자유도
                oRow[2] = Math.Round(oAnova.AnovaTable.SumOfSquaresBetween, m_iDecimalOuter);// 제곱합
                oRow[3] = Math.Round(oAnova.AnovaTable.MeanSquareBetween, m_iDecimalOuter);	// 평균제곱합
                oRow[4] = Math.Round(oAnova.AnovaTable.FStatistic, m_iDecimalOuter);			// F값
                oRow[5] = Math.Round(oAnova.AnovaTable.FStatisticPValue, m_iDecimalOuter);		// P값
                dtReturn.Rows.Add(oRow);

                // 오차 
                oRow[0] = m_Error;
                oRow[1] = oAnova.AnovaTable.DegreesOfFreedomWithin;	// 자유도
                oRow[2] = Math.Round(oAnova.AnovaTable.SumOfSquaresWithin, m_iDecimalOuter);	// 제곱합
                oRow[3] = Math.Round(oAnova.AnovaTable.MeanSquareWithin, m_iDecimalOuter);	// 평균제곱합
                oRow[4] = "";										// F값
                oRow[5] = "";										// P값
                dtReturn.Rows.Add(oRow);
                // 전체 
                oRow[0] = m_Total;
                oRow[1] = oAnova.AnovaTable.DegreesOfFreedomTotal;	// 자유도
                oRow[2] = Math.Round(oAnova.AnovaTable.SumOfSquaresTotal, m_iDecimalOuter);	// 제곱합
                oRow[3] = "";
                oRow[4] = "";
                oRow[5] = "";
                dtReturn.Rows.Add(oRow);
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsSource"></param>
        /// <param name="bSigleColumn">하나의 컬럼에 Value가 있는 형태인지.</param>
        /// <param name="arrSelectedColumns">여러 컬럼에 Value가 있는 형태인 경우에 그래프를 그리려고 선택한 컬럼명 목록</param>
        /// <param name="bBoxPlot"></param>
        /// <param name="bGraphHistogramResiduals"></param>
        /// <param name="bGraphProbabilityPlotResiduals"></param>
        /// <param name="bGraphResidualsVsFittedValues"></param>
        /// <param name="bGraphResidualsVSOrder"></param>
        /// <returns></returns>
        private DataSet GetTableforGraphs(DataSet dsSource, bool bSigleColumn, bool bBoxPlot, bool bGraphHistogramResiduals, bool bGraphProbabilityPlotResiduals, bool bGraphResidualsVsFittedValues)
        {
            DataSet dsReturn = null;
            DataTable dtTemp = null;
            List<double> arrResiduals;
            List<double> arrMean;  //Fit
            List<double> arrValue;
            int iRowCnt;
            int iMaxRowCnt;
            int iResidualsDecimal;
            int iTableCnt;
            int iMaxDecimal;
            try
            {
                
                dsReturn = new DataSet();
                iMaxDecimal = 0;
                iMaxRowCnt = 0;

                if (dsSource == null)
                    return null;
                iTableCnt = dsSource.Tables.Count;

                for (int i = 0; i < iTableCnt; i++)
                {
                    if (iMaxDecimal < GetDecimalPlace(dsSource.Tables[i]))
                        iMaxDecimal = GetDecimalPlace(dsSource.Tables[i]);
                }
                iResidualsDecimal = iMaxDecimal + 2;
                #region BoxPlot
                if (bBoxPlot)
                {
                    dtTemp = new DataTable(m_BoxPlot);
                    for (int i = 0; i < iTableCnt; i++)
                    {
                        dtTemp.Columns.Add(dsSource.Tables[i].TableName, dsSource.Tables[i].Columns[0].DataType);
                        if (dsSource.Tables[i].Rows.Count > iMaxRowCnt)
                            iMaxRowCnt = dsSource.Tables[i].Rows.Count;
                    }
                    for (int i = 0; i < iMaxRowCnt; i++)  //가장 Row Count가 많은 table기준만큼 Row를 넣고...
                    {
                        dtTemp.Rows.Add(dtTemp.NewRow());
                        for (int j = 0; j < iTableCnt; j++)
                        {
                            if (dsSource.Tables[j].Rows.Count > i)
                                dtTemp.Rows[i][j] = dsSource.Tables[j].Rows[i][0];
                        }
                    }
                    dtTemp.AcceptChanges();
                    dsReturn.Tables.Add(dtTemp.Copy());
                    dtTemp = null;
                }

                #endregion

                #region Histogram  or Probability or ResidualsVsFitted
                if (bGraphHistogramResiduals || bGraphProbabilityPlotResiduals || bGraphResidualsVsFittedValues )
                {
                    #region Residuals 계산
                    //Mean(Fit)계산
                    arrMean = new List<double>();
                    arrValue = new List<double>();
                    arrResiduals = new List<double>();
                    for (int i = 0; i < iTableCnt; i++)
                    {
                        iRowCnt = dsSource.Tables[i].Rows.Count;
                        double dblMean = Math.Round(StatsFunctions.NaNMean(((IDFColumn)(new DataFrame(dsSource.Tables[i])[0]))), iResidualsDecimal-2);
                        for (int j = 0; j < iRowCnt; j++)
                        {
                            
                            if (dsSource.Tables[i].Rows[j][0] != DBNull.Value && dsSource.Tables[i].Rows[j][0].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dsSource.Tables[i].Rows[j][0])))
                            {
                                arrMean.Add(dblMean);
                                arrValue.Add(Convert.ToDouble(dsSource.Tables[i].Rows[j][0]));
                                arrResiduals.Add(Math.Round(Convert.ToDouble(dsSource.Tables[i].Rows[j][0]) - dblMean, iResidualsDecimal));
                            }
                            else
                            {
                                //arrMean.Add(double.NaN);
                                //arrValue.Add(double.NaN);
                                //arrResiduals.Add(double.NaN);
                            }

                        }
                    }
                    #endregion

                    dtTemp = new DataTable();
                    dtTemp.Columns.Add(m_Residual, typeof(System.Double));
                    iRowCnt = arrResiduals.Count;
                    for (int i = 0; i < iRowCnt; i++)
                        dtTemp.Rows.Add(new object[] { arrResiduals[i] });

                    if (bGraphHistogramResiduals)
                    {
                        dtTemp.TableName = m_GraphHistogram;
                        dtTemp.AcceptChanges();
                        dsReturn.Tables.Add(dtTemp.Copy());
                    }
                    if (bGraphProbabilityPlotResiduals)
                    {
                        dtTemp.DefaultView.Sort = dtTemp.Columns[0].ColumnName;
                        dtTemp = dtTemp.DefaultView.ToTable(dtTemp.TableName, false, dtTemp.Columns[0].ColumnName);
                        dtTemp.Columns.Add("Percent", typeof(System.Double));
                        dtTemp.AcceptChanges();
                        for (int i = 0; i < iRowCnt; i++)
                            dtTemp.Rows[i][1] = Math.Round(Convert.ToDouble(((double)i + 1) * 100 / (double)iRowCnt), m_iDecimalOuter);
                        dtTemp.TableName = m_GraphProbability;
                        dtTemp.AcceptChanges();
                        dsReturn.Tables.Add(dtTemp.Copy());
                    }
                    dtTemp = null;

                    if (bGraphResidualsVsFittedValues)
                    {
                        dtTemp = new DataTable(m_GraphFit);
                        dtTemp.Columns.Add(m_Residual, typeof(System.Double));
                        dtTemp.Columns.Add(m_FittedValue, typeof(System.Double));
                        if (arrResiduals.Count != arrMean.Count)
                            throw new ArgumentException("Length of residuals does not equal length of mean values.", new Exception("MIRACOM"));
                        iRowCnt = arrResiduals.Count;
                        for (int i = 0; i < iRowCnt; i++)
                            dtTemp.Rows.Add(new object[] { arrResiduals[i], arrMean[i] });
                        dtTemp.AcceptChanges();
                        dsReturn.Tables.Add(dtTemp.Copy());
                        dtTemp = null;
                    }
                }
                #endregion


                return dsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetTableofRsquared(OneWayAnova oAnova)
        {
            DataTable dtReturn = null;
            double dblRs;
            double dblAdjRs;
            double dblMSR;
            string strRs = string.Empty;
            string strAdjRs = string.Empty;
            string strMSR = string.Empty;

            try
            {
                dtReturn = new DataTable(m_RSquare);
                dtReturn.Columns.Add(m_RSquare);
                dtReturn.Columns.Add(m_AdjRSquare);
                dtReturn.Columns.Add(m_RootMse);

                dblRs = (oAnova.AnovaTable.SumOfSquaresBetween / oAnova.AnovaTable.SumOfSquaresTotal);
                dblAdjRs = 1 - (oAnova.AnovaTable.MeanSquareWithin / (oAnova.AnovaTable.SumOfSquaresTotal / (double)oAnova.AnovaTable.DegreesOfFreedomTotal));
                dblMSR = Math.Sqrt(oAnova.AnovaTable.MeanSquareWithin);
                strRs = Convert.ToString(Math.Round(dblRs, m_iDecimalOuter));
                strAdjRs = Convert.ToString(Math.Round(dblAdjRs, m_iDecimalOuter));
                strMSR = Convert.ToString(Math.Round(dblMSR, m_iDecimalOuter));
                //if (double.IsInfinity(oAnova.RSquared))
                //    strRs = "*";
                //else
                //    strRs = Convert.ToString(Math.Round(oAnova.RSquared, m_iDecimalOuter));
                //if (double.IsInfinity(oAnova.AdjustedRsquared))
                //    strAdjRs = "*";
                //else
                //    strAdjRs = Convert.ToString(Math.Round(oAnova.AdjustedRsquared, m_iDecimalOuter));
                //if (double.IsInfinity(oAnova.MeanSquaredResidual))
                //    strMSR = "*";
                //else
                //    strMSR = Convert.ToString(Math.Round(oAnova.MeanSquaredResidual, m_iDecimalOuter));

                dtReturn.Rows.Add(new object[] { strRs, strAdjRs, strMSR });
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsSource">dsSource내의 DataTable은 Column(숫자형)이 하나만 존재해야함.</param>
        /// <returns></returns>
        private DataTable GetTableofStatistic(DataSet dsSource)
        {
            DataTable dtReturn = null;
            int iCnt;
            SelectedStatistics oStatistics;
            try
            {
                dtReturn = new DataTable(m_Statistics);
                dtReturn.Columns.Add(m_Level);
                dtReturn.Columns.Add(m_N);
                dtReturn.Columns.Add(m_Mean);
                dtReturn.Columns.Add(m_Standarddeviation);
                dtReturn.Columns.Add(m_Min);
                dtReturn.Columns.Add(m_Max);
                
                iCnt = dsSource.Tables.Count;

                oStatistics = new SelectedStatistics();
                oStatistics.Reset();
                oStatistics.IsMeanSE = false;
                oStatistics.IsNnonmissing = true;
                oStatistics.IsNtotal = false;
                oStatistics.IsNmissing = false;
                oStatistics.IsNtotal = true;
                oStatistics.IsQ1 = false;
                oStatistics.IsMedian = false;
                oStatistics.IsQ3 = false;               
                oStatistics.IsBiased = false;

                for (int i = 0; i < iCnt; i++)
                {
                    DescriptiveStatistics oCore = new DescriptiveStatistics(dsSource.Tables[i], 0, oStatistics);
                    if (double.IsNaN(oCore.Result.Standarddeviation))
                        dtReturn.Rows.Add(new object[] { dsSource.Tables[i].TableName, oCore.Result.Nnonmissing, oCore.Result.Mean, "*", oCore.Result.Min, oCore.Result.Max });
                    else
                        dtReturn.Rows.Add(new object[] { dsSource.Tables[i].TableName, oCore.Result.Nnonmissing, oCore.Result.Mean, oCore.Result.Standarddeviation, oCore.Result.Min, oCore.Result.Max });

                }
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        

        #region Conversion

        private DataTable GetUnitTable(DataTable dt, int iVar, int iValidRowIndices)
        {
            int iColCnt;
            int iRowCnt;
            DataTable dtReturn = null;
            try
            {
                dtReturn = new DataTable();
                dt.AcceptChanges();
                dtReturn = dt.Copy();
                dtReturn.TableName = dt.Columns[iVar].ColumnName;
                iColCnt = dtReturn.Columns.Count;
                for (int i = iColCnt-1; i > -1; i--)
                {
                    if (dtReturn.Columns[i].ColumnName != dtReturn.TableName)
                        dtReturn.Columns.RemoveAt(i);
                }
                //여기서 dtTemp의 DataTable작업...null Trim
                iRowCnt = dtReturn.Rows.Count;
                for (int i = iRowCnt - 1; i > -1; i--)
                {
                    if (i > iValidRowIndices)
                    {
                        if (!dtReturn.Rows[i][0].Equals(DBNull.Value))
                            break;
                        dtReturn.Rows.RemoveAt(i);
                    }
                }
                //여기서 dctTemp의 DataTable작업...null Trim
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet Conversion(DataTable dtSource, int[] arrValueColumns, int[] arrValidRowIndices)
        {
            DataTable dtTemp = null;
            DataSet dsReturn = null;
            int iCnt;
            try
            {                
                
                iCnt = arrValueColumns.Length;
                dsReturn = new DataSet();
                for (int i = 0; i < iCnt; i++)
                {
                    dtTemp = GetUnitTable(dtSource, arrValueColumns[i], arrValidRowIndices[i]);
                    dsReturn.Tables.Add(dtTemp.Copy());
                    dtTemp = null;
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsReturn = null;
                dtTemp = null;
            }
        }

        private DataSet Conversion(ref DataTable dtSource, int iValueColumn, int iGroupColumns)
        {
            DataTable dtDistinct = null;
            DataTable dtTemp = null;
            DataSet dsReturn = null;
            int iDistinctCnt;
            try
            {

                dtDistinct = SelectDistinct("DISTINCT", dtSource, new int[] { iGroupColumns } , true);
                iDistinctCnt = dtDistinct.Rows.Count;
                dsReturn = new DataSet();

                for (int i = 0; i < iDistinctCnt; i++)
                {
                    if (dtDistinct.Rows[i][0].ToString() == string.Empty)
                    {
                        DataRow[] arrTemp = dtSource.Select("[" + dtSource.Columns[iGroupColumns].ColumnName + "] = '" + dtDistinct.Rows[i][0].ToString() + "'");
                        foreach (DataRow dr in arrTemp)
                        {
                            dtSource.Rows.Remove(dr);
                        }
                        dtSource.AcceptChanges();
                    }
                    dtTemp = SelectWhere(dtSource, iValueColumn, "[" + dtSource.Columns[iGroupColumns].ColumnName + "] = '" + dtDistinct.Rows[i][0].ToString() + "'");
                    if (SelectWhere(dtTemp, 0, "[" + dtTemp.Columns[0].ColumnName + "]  is not null").Rows.Count < 1)
                    {
                        DataRow[] arrTemp = dtSource.Select("[" + dtSource.Columns[iGroupColumns].ColumnName + "] = '" + dtDistinct.Rows[i][0].ToString() + "'");
                        foreach (DataRow dr in arrTemp)
                        {
                            dtSource.Rows.Remove(dr);
                        }
                        dtSource.AcceptChanges();
                        dtTemp = null;
                        continue;
                    }
                    if (dtDistinct.Rows[i][0].ToString() == string.Empty)
                        dtTemp.TableName = "*";
                    else
                        dtTemp.TableName = dtDistinct.Rows[i][0].ToString();

                   
                    dsReturn.Tables.Add(dtTemp.Copy());
                    dtTemp = null;
                }                
                return dsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsReturn = null;
                dtDistinct = null;
                dtTemp = null;
            }
        }

        #endregion

        #region Inner Util Methods

        #region DataTable

        private DataTable SelectWhere( DataTable Source, string Filter)
        {
            DataTable dtReturn = null;
            DataRow[] arrDr = null;
            int iDrCnt;
            try
            {
                arrDr = Source.Select(Filter);
                iDrCnt = arrDr.Length;
                dtReturn = Source.Clone();
                dtReturn.AcceptChanges();
                for (int i = 0; i < iDrCnt; i++)
                {
                    dtReturn.Rows.Add(arrDr[i].ItemArray);
                }
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtReturn = null;
                arrDr = null;
            }
        }

        private DataTable SelectWhere(DataTable Source, int SelectedColumn, string Filter)
        {
            DataTable dtReturn = null;
            DataRow[] arrDr = null;
            int iDrCnt;
            try
            {

                dtReturn = new DataTable();
                dtReturn.Columns.Add(Source.Columns[SelectedColumn].ColumnName, Source.Columns[SelectedColumn].DataType);
                dtReturn.AcceptChanges();

                arrDr = Source.Select(Filter);
                iDrCnt = arrDr.Length;

                for (int i = 0; i < iDrCnt; i++)
                {
                    dtReturn.Rows.Add(new object[] { arrDr[i][SelectedColumn] });
                }
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtReturn = null;
                arrDr = null;
            }
        }

        private DataTable SelectDistinctNoSort(string TableName, DataTable SourceTable, int[] FieldIndex)
        {
            try
            {
                return SelectDistinct(TableName, SourceTable, FieldIndex, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable SelectDistinct(string TableName, DataTable SourceTable, int[] FieldIndex, bool bSort)
        {
            string[] arrColumnName = null;
            int iColumnCnt;
            try
            {
                if (FieldIndex == null)
                    throw new ArgumentException(ErrorParameter("FieldIndex"));
                if (SourceTable == null)
                    throw new ArgumentException(ErrorParameter("SourceTable"));
                iColumnCnt = FieldIndex.Length;
                arrColumnName = GetColumnName(SourceTable, FieldIndex);
                if (bSort)
                    SourceTable.DefaultView.Sort = string.Join(",", arrColumnName);
                return SourceTable.DefaultView.ToTable(TableName, true, arrColumnName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrColumnName = null;
            }
        }

        #endregion

        private string[] GetColumnName(DataTable dt, int[] arrColumnIndex)
        {
            string[] arrReturn = null;
            int iCnt;
            try
            {
                if (arrColumnIndex == null)
                    return null;
                if (dt == null)
                    throw new ArgumentException(ErrorParameter("dt"));
                iCnt = arrColumnIndex.Length;

                arrReturn = new string[iCnt];
                for (int i = 0; i < iCnt; i++)
                {
                    arrReturn[i] = dt.Columns[arrColumnIndex[i]].ColumnName;
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrReturn = null;
            }
        }

        private string ErrorParameter(string strParameter)
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

        private int GetDecimalPlace(DataTable dt, int iCol)
        {
            int iReturn;
            int iRowCnt;
            int iColCnt;
            double dblTemp;
            int iTemp;
            try
            {
                if (dt == null)
                    throw new ArgumentException("dt is null.");
                iRowCnt = dt.Rows.Count;
                iColCnt = dt.Columns.Count;
                if (iCol >= iColCnt)
                    throw new ArgumentException("dt dont has iCol.");
                iReturn = 0;
                for (int i = 0; i < iRowCnt; i++)
                {

                    if (double.TryParse(dt.Rows[i][iCol].ToString(), out dblTemp))
                    {
                        if (dblTemp.ToString().IndexOf('.') > -1)
                        {
                            iTemp = dblTemp.ToString().Length - (dblTemp.ToString().IndexOf('.') + 1);
                            if (iReturn < iTemp)
                                iReturn = iTemp;
                        }
                    }
                }
                return iReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetDecimalPlace(DataTable dt)
        {
            int iReturn;
            int iRowCnt;
            int iColCnt;
            double dblTemp;
            int iTemp;
            try
            {
                if (dt == null)
                    throw new ArgumentException("dt is null.");
                iRowCnt = dt.Rows.Count;
                iColCnt = dt.Columns.Count;
                iReturn = 0;
                for (int i = 0; i < iRowCnt; i++)
                {
                    for (int j = 0; j < iColCnt; j++)
                    {
                        if (double.TryParse(dt.Rows[i][j].ToString(), out dblTemp))
                        {
                            if (dblTemp.ToString().IndexOf('.') > -1)
                            {
                                iTemp = dblTemp.ToString().Length - (dblTemp.ToString().IndexOf('.') + 1);
                                if (iReturn < iTemp)
                                    iReturn = iTemp;
                            }
                        }
                    }
                }
                return iReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetDecimalPlace(double[] arrValues)
        {
            int iReturn;
            int iCnt;
            double dblTemp;
            int iTemp;
            try
            {
                if (arrValues == null)
                    throw new ArgumentException("arrValues is null.");
                iCnt = arrValues.Length;
                iReturn = 0;
                for (int i = 0; i < iCnt; i++)
                {
                    if (double.TryParse(arrValues[i].ToString(), out dblTemp))
                    {
                        if (dblTemp.ToString().IndexOf('.') > -1)
                        {
                            iTemp = dblTemp.ToString().Length - (dblTemp.ToString().IndexOf('.') + 1);
                            if (iReturn < iTemp)
                                iReturn = iTemp;
                        }
                    }
                }
                return iReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int[] MergeArray(int[] arrayA, int[] arrayB)
        {
            List<int> arrReturn;
            int iACnt;
            int iBCnt;
            try
            {
                if (arrayA == null)
                    iACnt = 0;
                else
                    iACnt = arrayA.Length;
                if (arrayB == null)
                    iBCnt = 0;
                else
                    iBCnt = arrayB.Length;
                arrReturn = new List<int>(iACnt + iBCnt);
                if (arrayA != null)
                    arrReturn.AddRange(arrayA);
                for (int i = 0; i < iBCnt; i++)
                {
                    if (!arrReturn.Contains(arrayB[i]))
                        arrReturn.Add(arrayB[i]);
                }
                //arrReturn.TrimExcess();
                arrReturn.Sort();
                return arrReturn.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrReturn = null;
            }
        }

        private bool EqualArrayFactor(int[] arrayA, int[] arrayB)
        {
            int iACnt;
            int iBCnt;
            try
            {

                if (arrayA == null)
                    iACnt = 0;
                else
                    iACnt = arrayA.Length;
                if (arrayB == null)
                    iBCnt = 0;
                else
                    iBCnt = arrayB.Length;

                if (iACnt == 0 && iBCnt == 0)
                    return true;

                if (iACnt != iBCnt)
                    return false;

                for (int i = 0; i < iACnt; i++)
                {
                    bool bTemp = false;
                    for (int j = 0; j < iBCnt; j++)
                    {
                        if (arrayA[i] == arrayB[j])
                        {
                            bTemp = true;
                            break;
                        }
                    }
                    if (!bTemp)
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

        private bool EqualArrayFactor(string[] arrayA, string[] arrayB)
        {
            int iACnt;
            int iBCnt;
            try
            {

                if (arrayA == null)
                    iACnt = 0;
                else
                    iACnt = arrayA.Length;
                if (arrayB == null)
                    iBCnt = 0;
                else
                    iBCnt = arrayB.Length;

                if (iACnt == 0 && iBCnt == 0)
                    return true;

                if (iACnt != iBCnt)
                    return false;

                for (int i = 0; i < iACnt; i++)
                {
                    bool bTemp = false;
                    for (int j = 0; j < iBCnt; j++)
                    {
                        if (arrayA[i] == arrayB[j])
                        {
                            bTemp = true;
                            break;
                        }
                    }
                    if (!bTemp)
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

        private double[] GetSumArray(double[] arrayA, double[] arrayB)
        {
            double[] arrReturn = null;
            int iACnt;
            int iBCnt;
            try
            {
                if (arrayA == null)
                    iACnt = 0;
                else
                    iACnt = arrayA.Length;

                if (arrayB == null)
                    iBCnt = 0;
                else
                    iBCnt = arrayB.Length;

                if (iACnt == 0 && iBCnt == 0)
                    return new double[0];
                if (iACnt != iBCnt)
                    throw new ArgumentException("Length of Arguments does not equal.");
                arrReturn = new double[iACnt];
                for (int i = 0; i < iACnt; i++)
                {
                    if(double.IsNaN(arrayA[i]) || double.IsNaN(arrayB[i]))
                        throw new ArgumentException("Arguments have double.NaN.");
                    arrReturn[i] = arrayA[i] + arrayB[i];
                }
                return arrReturn;
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
