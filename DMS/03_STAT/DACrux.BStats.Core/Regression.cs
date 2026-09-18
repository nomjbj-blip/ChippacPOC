using System;
using System.Collections.Generic;
using System.Data;
using CenterSpace.NMath.Core;
using CenterSpace.NMath.Stats;
using System.IO;

namespace DACrux.BStats.Core
{
    public class Regression
    {
        #region " MEMBER FIELD "

        private DataTable m_dtSource;
        int m_iDecimalInner = 4;
        int m_iDecimalOuter = 4;
        SelectedRegression m_InputOption;
        ResultRegression m_Result;



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
        string m_T = "T";
        string m_Pvalue = "P-Value";
        string m_ObsNumber = "OBS-No";
        //string m_Response = "Response";
        string m_Predictors = "Predictors";
        string m_Fit = "Fit";
        string m_Residual = "Residual";
        string m_StudentizedResidual = "StudentizedResidual";
        string m_Model = "Model";
        string m_Error = "Error";
        string m_CorrectedTotal = "Corrected Total";
        #region 계수에 대한 출력
        string m_CoefficientsTitle = "Table of Coefficients";
        //string m_Constant = "Constant";
        string m_Coef = "Coef";
        string m_SECoef = "SE Coef";
        #endregion
        string m_AnovaTitle = "Analysis of Variance";
        string m_ResidualsTitle = "Table of Fits and Residuals";

        #region Graph Title
        string m_GraphHistogram = "Histogram of residuals";
        string m_GraphProbability = "Probability Plot of the Residuals";
        string m_GraphFit = "Residuals Versus the Fitted Values";
        string m_GraphOrder = "Residuals Versus Order";
        #endregion


        #region Message
        string m_ErrorData = "Source is null.";
        string m_ErrorDataLength = "Length of data is 0.";
        string m_ErrorResponse = "Invalid response variable. \r\n Too few items.";
        string m_ErrorPredictor = "Invalid predictor(s). \r\n Too few items.";
        string m_ErrorSelectedNo = "No variables entered or removed.";
        string m_ErrorDataNotEnough = "Not enough data in column.";

        string m_ErrorSystemIncludeNotSelected = "Include variables must be selected variables";
        #endregion

        #endregion

        #endregion

        #region " CREATOR "
        public Regression(DataTable dtSource, SelectedRegression InputOption)
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

        public ResultRegression Result
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

        private ResultRegression Analysis(DataTable dtSource, SelectedRegression InputOption)
        {
            //dtSource의 Rows Count
            int iRowCnt;
            //변수 선택법에 의해 선택된 변수 목록
            int[] arrSelected = null;
            //맵핑된 종속변수
            int iMapResponse;
            //맵핑된 독립변수
            int[] arrMapPredictors = null;
            //맵핑된 필수변수
            int[] arrMapIncludePredictors = null;
            //맵핑된 초기모형 변수
            int[] arrMapPredictorsInitialModel = null;
            //맵핑된 그래프 변수
            int[] arrMapGraphVariablesVsResiduals = null;
            //결과
            ResultRegression oReturn;
            //Trim된 dtSource에서 자료순번이 column 0번에 존재하는 DataTable
            DataTable dtObs = null;
            //Trim되기전의 원본 dtSource
            DataTable dtOrgin = null;
            try
            {
                if (dtSource == null)
                    throw new ArgumentException(m_ErrorData);
                iRowCnt = dtSource.Rows.Count;
                if (iRowCnt < 1)
                    throw new ArgumentException(m_ErrorDataLength, new Exception("MIRACOM"));
                if (InputOption.iResponse < 0)
                    throw new ArgumentException(m_ErrorResponse, new Exception("MIRACOM"));
                if (InputOption.arrPredictors == null || InputOption.arrPredictors.Length < 1)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));


                m_Result.Reset();


                #region dtSource 정리 및 Index List Mapping하기...꼭
                //꼭 dtSource복사해서 정리하고, 아래부분에는 복사해서 정리된 DataTable사용할 것!!!!---꼭 그래야하나...어디까지 영향을 주는 것인가..DataTable...
                TrimMapping(ref dtSource, out dtObs, out dtOrgin, InputOption.iResponse, InputOption.arrPredictors, InputOption.Methods.arrIncludePredictors, InputOption.Methods.arrPredictorsInitialModel, InputOption.Options.arrGraphVariablesVsResiduals, out iMapResponse, out arrMapPredictors, out arrMapIncludePredictors, out arrMapPredictorsInitialModel, out arrMapGraphVariablesVsResiduals);
                #endregion


                InputOption.arrPredictors = arrMapPredictors;
                InputOption.Methods.arrIncludePredictors = arrMapIncludePredictors;
                InputOption.Methods.arrPredictorsInitialModel = arrMapPredictorsInitialModel;
                InputOption.Options.arrGraphVariablesVsResiduals = arrMapGraphVariablesVsResiduals;

                #region 선택한 변수선택으로 변수 선택하기(독립변수만)
                arrSelected = GetSelectedVariables(dtSource, iMapResponse, arrMapPredictors, InputOption.Methods, InputOption.IsIntercept);
                if (arrSelected == null || arrSelected.Length < 1)
                {
                    oReturn = new ResultRegression();
                    oReturn.ErrorInformation = m_ErrorSelectedNo;
                    return oReturn;
                }
                #endregion

                #region Regression Analysis //옵션에 따라서...

                oReturn = RegressionAnalysis(dtSource, iMapResponse, arrSelected, arrMapGraphVariablesVsResiduals, InputOption.IsIntercept, InputOption.Options, dtObs, dtOrgin);

                #endregion


                //그래프 데이타 작성할때는 선택된 변수인지 확인하고 선택된 것들만 작성할 것!!! 김현태

                return oReturn;

            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == "MIRACOM")
                {
                    ResultRegression oExReturn = new ResultRegression();
                    oExReturn.ErrorInformation = ex.Message;
                    return oExReturn;
                }
                else
                    throw ex;
            }
        }

        #region Inner Util Methods

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

        private void TrimMapping(ref DataTable dtSource, out DataTable dtObs, out DataTable dtOrgin, int iResponese, int[] arrPredictors, int[] arrIncludePredictors, int[] arrPredictorsInitialModel, int[] arrGraph, out int iMapResponese, out int[] arrMapPredictors, out int[] arrMapIncludePredictors, out int[] arrMapPredictorsInitialModel, out int[] arrMapGraph)
        {
            int iColCnt;
            int iVarCnt;  //독립변수의 개수
            int iMapping;

            #region 현재 선택된 컬럼 인덱스 리스트
            int[] arrVar = null;
            #endregion

            #region 정리된 DataTable에서 현재 선택된 컬럼 인덱스 리스트
            int[] arrMappingVar = null;
            #endregion

            #region 내부대화상자에서 선택한 변수가 있는지..
            bool bGraph = false;
            bool bInclude = false;
            bool bInitial = false;

            #endregion

            try
            {
                dtOrgin = dtSource.Copy();
                #region 예외확인
                if (iResponese < 0)
                    throw new ArgumentException(m_ErrorResponse, new Exception("MIRACOM"));
                if (arrPredictors == null)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                iVarCnt = arrPredictors.Length;
                if (iVarCnt < 1)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                #endregion

                arrVar = new int[iVarCnt + 1];
                arrVar[0] = iResponese;

                for (int i = 0; i < iVarCnt; i++)
                {
                    arrVar[i + 1] = arrPredictors[i];
                }
                arrMappingVar = new int[iVarCnt + 1];

                #region 선택한 컬럼만 남기고 나머지 컬럼삭제, 선택 인덱스 목록 맵핑
                iColCnt = dtSource.Columns.Count;

                iMapping = iVarCnt;

                #region 내부대화상자 맵핑 임시 배열
                if (arrGraph != null && arrGraph.Length > 0)
                {
                    bGraph = true;
                    arrMapGraph = new int[arrGraph.Length];
                }
                else
                    arrMapGraph = arrGraph;
                if (arrIncludePredictors != null && arrIncludePredictors.Length > 0)
                {
                    bInclude = true;
                    arrMapIncludePredictors = new int[arrIncludePredictors.Length];
                }
                else
                    arrMapIncludePredictors = arrIncludePredictors;
                if (arrPredictorsInitialModel != null && arrPredictorsInitialModel.Length > 0)
                {
                    bInitial = true;
                    arrMapPredictorsInitialModel = new int[arrPredictorsInitialModel.Length];
                }
                else
                    arrMapPredictorsInitialModel = arrPredictorsInitialModel;


                #endregion

                for (int i = iColCnt - 1; i > -1; i--)
                {
                    int iIndex = -1;
                    iIndex = Array.IndexOf(arrVar, i);
                    if (iIndex != -1)
                    {
                        arrMappingVar[iIndex] = iMapping;

                        #region 내부 대화상자
                        if (bGraph)
                        {
                            iIndex = Array.IndexOf(arrGraph, i);
                            if (iIndex != -1)
                                arrMapGraph[iIndex] = iMapping;
                        }
                        if (bInclude)
                        {
                            iIndex = Array.IndexOf(arrIncludePredictors, i);
                            if (iIndex != -1)
                                arrMapIncludePredictors[iIndex] = iMapping;
                        }
                        if (bInitial)
                        {
                            iIndex = Array.IndexOf(arrPredictorsInitialModel, i);
                            if (iIndex != -1)
                                arrMapPredictorsInitialModel[iIndex] = iMapping;
                        }
                        #endregion

                        iMapping--;
                    }
                    else
                    {
                        dtSource.Columns.RemoveAt(i);
                        dtSource.AcceptChanges();
                    }
                }
                iMapResponese = arrMappingVar[0];
                arrMapPredictors = new int[iVarCnt];
                for (int i = 0; i < iVarCnt; i++)
                    arrMapPredictors[i] = arrMappingVar[i + 1];

                DataFrame df = new DataFrame(dtSource);
                dtSource = df.CleanRows().ToDataTable();
                dtObs = dtSource.Copy();
                dtSource.Columns.RemoveAt(0);
                dtSource.AcceptChanges();
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrMappingVar = null;
                arrVar = null;
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

        private int[] GetIndexInRegession(int[] arrPredictors, bool bIntercept)
        {
            int[] arrReturn;
            int[] arrSorted;
            int iCnt;
            try
            {
                if (arrPredictors == null)
                    throw new Exception(m_ErrorPredictor, new Exception("MIRACOM"));
                iCnt = arrPredictors.Length;
                if (iCnt < 1)
                    throw new Exception(m_ErrorPredictor, new Exception("MIRACOM"));
                arrSorted = new int[iCnt];
                arrPredictors.CopyTo(arrSorted, 0);

                Array.Sort(arrSorted);
                if (bIntercept)
                {
                    arrReturn = new int[iCnt + 1];
                    arrReturn[0] = 0;  //절편
                    for (int i = 0; i < iCnt; i++)
                    {
                        arrReturn[i + 1] = Array.IndexOf(arrSorted, arrPredictors[i]) + 1;
                    }
                }
                else
                {
                    arrReturn = new int[iCnt];
                    for (int i = 0; i < iCnt; i++)
                    {
                        arrReturn[i] = Array.IndexOf(arrSorted, arrPredictors[i]);
                    }
                }

                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string[] GetPredictorsName(DataTable dtSource, int[] arrPredictors, bool bIntercept)
        {
            int iCnt;
            string[] arrReturn = null;
            try
            {
                if (arrPredictors == null)
                    return null;
                iCnt = arrPredictors.Length;
                if (bIntercept)
                {
                    arrReturn = new string[iCnt + 1];
                    arrReturn[0] = "Intercept";
                    for (int i = 0; i < iCnt; i++)
                        arrReturn[i + 1] = dtSource.Columns[arrPredictors[i]].ColumnName;
                }
                else
                {
                    arrReturn = new string[iCnt];
                    for (int i = 0; i < iCnt; i++)
                        arrReturn[i] = dtSource.Columns[arrPredictors[i]].ColumnName;
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 분석결과

        private string GetRegressionEquation(LinearRegressionParameter[] oParaInfo, string strResponse, string[] arrPredictorsName, int[] arrIndexInRegression, int iDecimal)
        {
            string strReturn = string.Empty;
            int iCnt;
            try
            {
                if (oParaInfo.Length != arrPredictorsName.Length || oParaInfo.Length != arrIndexInRegression.Length)
                    throw new ArgumentException("Length of arguments does not equal.", new Exception("MIRACOM"));
                strReturn = strResponse + " = ";
                iCnt = oParaInfo.Length;
                for (int i = 0; i < iCnt; i++)
                {
                    if (i == 0 && arrPredictorsName[i] == "Intercept")
                    {

                        strReturn += ((double)(Math.Round(oParaInfo[arrIndexInRegression[i]].Value, iDecimal))).ToString();
                    }
                    else
                    {
                        if (oParaInfo[arrIndexInRegression[i]].Value > 0)
                        {
                            if (i > 0)
                                strReturn += " +" + ((double)(Math.Round(oParaInfo[arrIndexInRegression[i]].Value, iDecimal))).ToString() + "*" + arrPredictorsName[i];
                            else
                                strReturn += ((double)(Math.Round(oParaInfo[arrIndexInRegression[i]].Value, iDecimal))).ToString() + "*" + arrPredictorsName[i];
                        }
                        else
                            strReturn += " - " + ((double)(Math.Round(Math.Abs(oParaInfo[arrIndexInRegression[i]].Value), iDecimal))).ToString() + "*" + arrPredictorsName[i];

                    }
                }
                return strReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet GetTableforGraphs(DataTable dtSource, double[] arrResiduals, double[] arrFit, SelectedRegressionOptions oOption)
        {
            DataSet dsReturn = null;
            DataTable dtTemp = null;
            int iRowCnt;
            int iResidualsDecimal;
            double[] arrResidualsRound = null;
            try
            {
                dsReturn = new DataSet();
                iResidualsDecimal = GetDecimalPlace(dtSource) + 2;
                arrResidualsRound = new double[arrResiduals.Length];
                for (int i = 0; i < arrResiduals.Length; i++)
                    arrResidualsRound[i] = Math.Round(arrResiduals[i], iResidualsDecimal);

                if (oOption.IsGraphHistogramResiduals || oOption.IsGraphProbabilityPlotResiduals)
                {
                    dtTemp = new DataTable();
                    dtTemp.Columns.Add("Residual", typeof(System.Double));
                    iRowCnt = arrResiduals.Length;
                    for (int i = 0; i < iRowCnt; i++)
                        dtTemp.Rows.Add(new object[] { arrResidualsRound[i] });
                    
                    if (oOption.IsGraphHistogramResiduals)
                    {
                        dtTemp.TableName = m_GraphHistogram;
                        dtTemp.AcceptChanges();
                        dsReturn.Tables.Add(dtTemp.Copy());
                    }
                    if (oOption.IsGraphProbabilityPlotResiduals)
                    {
                        dtTemp.DefaultView.Sort = dtTemp.Columns[0].ColumnName;
                        dtTemp = dtTemp.DefaultView.ToTable(dtTemp.TableName, false, dtTemp.Columns[0].ColumnName);
                        dtTemp.Columns.Add("Percent", typeof(System.Double));
                        dtTemp.AcceptChanges();
                        for (int i = 0; i < iRowCnt; i++)
                            dtTemp.Rows[i][1] = Math.Round(Convert.ToDouble(((double)i+1) * 100 / (double)iRowCnt), m_iDecimalOuter);
                        dtTemp.TableName = m_GraphProbability;
                        dtTemp.AcceptChanges();
                        dsReturn.Tables.Add(dtTemp.Copy());
                    }
                    //dtTemp.Dispose();
                    dtTemp = null;
                }
                if (oOption.IsGraphResidualsVsFittedValues)
                {
                    dtTemp = new DataTable(m_GraphFit);
                    dtTemp.Columns.Add("Residual", typeof(System.Double));
                    dtTemp.Columns.Add("Fitted Value", typeof(System.Double));
                    if (arrResiduals.Length != arrFit.Length)
                        throw new ArgumentException("Length of residuals does not equal length of fitted values.", new Exception("MIRACOM"));
                    iRowCnt = arrResiduals.Length;
                    for (int i = 0; i < iRowCnt; i++)
                        dtTemp.Rows.Add(new object[] { arrResidualsRound[i], arrFit[i] });
                    dtTemp.AcceptChanges();
                    dsReturn.Tables.Add(dtTemp.Copy());
                }
                if (oOption.IsGraphResidualsVSOrder)
                {
                    dtTemp = new DataTable(m_GraphOrder);
                    dtTemp.Columns.Add("Residual", typeof(System.Double));
                    dtTemp.Columns.Add("Observation Order", typeof(System.Int32));
                    if (arrResiduals.Length != arrFit.Length)
                        throw new ArgumentException("Length of residuals does not equal length of fitted values.", new Exception("MIRACOM"));
                    iRowCnt = arrResiduals.Length;
                    for (int i = 0; i < iRowCnt; i++)
                        dtTemp.Rows.Add(new object[] { arrResidualsRound[i], i + 1 });
                    dtTemp.AcceptChanges();
                    dsReturn.Tables.Add(dtTemp.Copy());
                }


                if (oOption.arrGraphVariablesVsResiduals != null && oOption.arrGraphVariablesVsResiduals.Length > 0)
                {
                    if (dtSource.Rows.Count != arrResiduals.Length)
                        throw new ArgumentException("Source's row count does not equal length of residuals", new Exception("MIRACOM"));
                    string strColName;
                    for (int i = 0; i < oOption.arrGraphVariablesVsResiduals.Length; i++)
                    {
                        strColName = dtSource.Columns[oOption.arrGraphVariablesVsResiduals[i]].ColumnName;
                        dtTemp = new DataTable("Residuals Versus " + strColName);
                        dtTemp.Columns.Add("Residual", typeof(System.Double));
                        dtTemp.Columns.Add(strColName, dtSource.Columns[oOption.arrGraphVariablesVsResiduals[i]].DataType);
                        iRowCnt = arrResiduals.Length;
                        for (int j = 0; j < iRowCnt; j++)
                            dtTemp.Rows.Add(new object[] { arrResidualsRound[j], dtSource.Rows[j][oOption.arrGraphVariablesVsResiduals[i]] });
                        dtTemp.AcceptChanges();
                        dsReturn.Tables.Add(dtTemp.Copy());
                    }
                }


                //dsReturn.Tables.Add(dtTemp.Copy());
                return dsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetTableofAnova(LinearRegressionAnova oAnova)
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
                if (oAnova.ErrorDegreesOfFreedom == 0)
                {
                    // 모형 
                    oRow[0] = m_Model;
                    oRow[1] = oAnova.ModelDegreesOfFreedom;				// 자유도
                    oRow[2] = Math.Round(oAnova.RegressionSumOfSquares, m_iDecimalOuter);// 제곱합
                    oRow[3] = Math.Round(oAnova.MeanSquaredRegression, m_iDecimalOuter);	// 평균제곱합
                    oRow[4] = "*";			// F값
                    oRow[5] = "*";		// P값
                    dtReturn.Rows.Add(oRow);

                    // 오차 
                    oRow[0] = m_Error;
                    oRow[1] = oAnova.ErrorDegreesOfFreedom;	// 자유도
                    oRow[2] = "*";	// 제곱합
                    oRow[3] = "*";	// 평균제곱합
                    oRow[4] = "";										// F값
                    oRow[5] = "";										// P값
                    dtReturn.Rows.Add(oRow);
                    // 전체 
                    oRow[0] = m_CorrectedTotal;
                    oRow[1] = oAnova.ModelDegreesOfFreedom + oAnova.ErrorDegreesOfFreedom;	// 자유도
                    oRow[2] = Math.Round(oAnova.RegressionSumOfSquares, m_iDecimalOuter);	// 제곱합
                    oRow[3] = "";
                    oRow[4] = "";
                    oRow[5] = "";
                    dtReturn.Rows.Add(oRow);
                }
                else
                {
                    // 모형 
                    oRow[0] = m_Model;
                    oRow[1] = oAnova.ModelDegreesOfFreedom;				// 자유도
                    oRow[2] = Math.Round(oAnova.RegressionSumOfSquares, m_iDecimalOuter);// 제곱합
                    oRow[3] = Math.Round(oAnova.MeanSquaredRegression, m_iDecimalOuter);	// 평균제곱합
                    oRow[4] = Math.Round(oAnova.FStatistic, m_iDecimalOuter);			// F값
                    oRow[5] = Math.Round(oAnova.FStatisticPValue, m_iDecimalOuter);		// P값
                    dtReturn.Rows.Add(oRow);

                    // 오차 
                    oRow[0] = m_Error;
                    oRow[1] = oAnova.ErrorDegreesOfFreedom;	// 자유도
                    oRow[2] = Math.Round(oAnova.ResidualSumOfSquares, m_iDecimalOuter);	// 제곱합
                    oRow[3] = Math.Round(oAnova.MeanSquaredResidual, m_iDecimalOuter);	// 평균제곱합
                    oRow[4] = "";										// F값
                    oRow[5] = "";										// P값
                    dtReturn.Rows.Add(oRow);
                    // 전체 
                    oRow[0] = m_CorrectedTotal;
                    oRow[1] = oAnova.ModelDegreesOfFreedom + oAnova.ErrorDegreesOfFreedom;	// 자유도
                    oRow[2] = Math.Round(oAnova.RegressionSumOfSquares + oAnova.ResidualSumOfSquares, m_iDecimalOuter);	// 제곱합
                    oRow[3] = "";
                    oRow[4] = "";
                    oRow[5] = "";
                    dtReturn.Rows.Add(oRow);
                }

                
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetTableofCoefficients(LinearRegressionParameter[] arrParaInfo, string[] arrColumnName, int[] arrSortedIndex)
        {
            DataTable dtReturn;
            int iCnt;
            try
            {
                if (arrParaInfo == null || arrColumnName == null || arrSortedIndex == null)
                    throw new ArgumentException("Length of arguments(parameter info, column name, sorted index) does not equal.");
                if(arrParaInfo.Length != arrColumnName.Length || arrParaInfo.Length != arrSortedIndex.Length)
                    throw new ArgumentException("Length of arguments(parameter info, column name, sorted index) does not equal.");
                dtReturn = new DataTable(m_CoefficientsTitle);
                dtReturn.Columns.Add(m_Predictors);
                dtReturn.Columns.Add(m_Coef);
                dtReturn.Columns.Add(m_SECoef);
                dtReturn.Columns.Add(m_T);
                dtReturn.Columns.Add(m_Pvalue);

                iCnt = arrSortedIndex.Length;
                for (int i = 0; i < iCnt; i++)
                {
                    if (double.IsInfinity(arrParaInfo[arrSortedIndex[i]].StandardError))
                        dtReturn.Rows.Add(new object[] { arrColumnName[i], Math.Round(arrParaInfo[arrSortedIndex[i]].Value, m_iDecimalOuter), "*", "*", "*" });
                    else
                        dtReturn.Rows.Add(new object[] { arrColumnName[i], Math.Round(arrParaInfo[arrSortedIndex[i]].Value, m_iDecimalOuter), Math.Round(arrParaInfo[arrSortedIndex[i]].StandardError, m_iDecimalOuter), Math.Round(arrParaInfo[arrSortedIndex[i]].TStatistic(0.0), m_iDecimalOuter), Math.Round(arrParaInfo[arrSortedIndex[i]].TStatisticPValue(0.0), m_iDecimalOuter) });
                }
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetTableofRsquared(LinearRegressionAnova oAnova)
        {
            DataTable dtReturn = null;
            string strRs = string.Empty;
            string strAdjRs = string.Empty;
            string strMSR = string.Empty;

            try
            {
                dtReturn = new DataTable(m_RSquare);
                dtReturn.Columns.Add(m_RSquare);
                dtReturn.Columns.Add(m_AdjRSquare);
                dtReturn.Columns.Add(m_RootMse);
                if (double.IsInfinity(oAnova.RSquared))
                    strRs = "*";
                else
                    strRs = Convert.ToString(Math.Round(oAnova.RSquared, m_iDecimalOuter));
                if (double.IsInfinity(oAnova.AdjustedRsquared))
                    strAdjRs = "*";
                else
                    strAdjRs = Convert.ToString(Math.Round(oAnova.AdjustedRsquared, m_iDecimalOuter));
                if (double.IsInfinity(oAnova.MeanSquaredResidual))
                    strMSR = "*";
                else
                    strMSR = Convert.ToString(Math.Round(Math.Sqrt(oAnova.MeanSquaredResidual), m_iDecimalOuter));
                    //strMSR = Convert.ToString(Math.Round(oAnova.MeanSquaredResidual, m_iDecimalOuter));
                dtReturn.Rows.Add(new object[] {strRs, strAdjRs, strMSR });
                dtReturn.AcceptChanges();
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }    
        }

        private DataTable GetTableofFitsResiduals(DataTable dtOrgin, DataTable dtObs, LinearRegression lr, string strResponse, double[] arrFit, double[] arrResidual, double[] arrResponse)
        {
            int iRowCnt;
            double dblDeNumCoeff;
            DataTable dtReturn = null;
            int iObsRow;
            DataRow[] arrResult = null;
            int iDecimal;
            try
            {
                if (dtOrgin == null || dtObs == null || lr == null || strResponse == string.Empty || arrFit == null || arrResidual == null || arrResponse == null)
                    throw new ArgumentException("Argument is null.");
                if(arrFit.Length != arrResidual.Length || arrFit.Length != arrResponse.Length)
                    throw new ArgumentException("Length of arguments does not equal.");
                
                dblDeNumCoeff = Math.Sqrt(lr.Variance);
                dtReturn = new DataTable(m_ResidualsTitle);
                dtReturn.Columns.Add(m_ObsNumber);
                dtReturn.Columns.Add(strResponse);
                dtReturn.Columns.Add(m_Fit);
                dtReturn.Columns.Add(m_Residual);
                dtReturn.Columns.Add(m_StudentizedResidual);
                iRowCnt = dtOrgin.Rows.Count;
                iObsRow = 0;
                dtObs.Columns[0].ColumnName = "JINXIANTAI_OB";

                iDecimal = GetDecimalPlace(arrResidual);
                for (int i = 0; i < iRowCnt; i++)
                {
                    arrResult = dtObs.Select("[" + dtObs.Columns[0].ColumnName + "] = '" + ((int)(i + 1)).ToString() + "'");
                    if (arrResult.Length == 1)
                    {
                        //dtReturn.Rows.Add(new object[] { arrResult[0][0], arrResponse[iObsRow], Math.Round(Convert.ToDouble(arrFit[iObsRow]), m_iDecimalOuter), Math.Round(Convert.ToDouble(arrResidual[iObsRow]), m_iDecimalOuter), Math.Round(Convert.ToDouble(arrResidual[iObsRow]) / dblDeNumCoeff, m_iDecimalOuter) });
                        if (double.IsInfinity(dblDeNumCoeff))
                            dtReturn.Rows.Add(new object[] { arrResult[0][0], dtOrgin.Rows[i][strResponse], Convert.ToDouble(arrFit[iObsRow]), Convert.ToDouble(arrResidual[iObsRow]), "*" });
                        else
                            dtReturn.Rows.Add(new object[] { arrResult[0][0], dtOrgin.Rows[i][strResponse], Convert.ToDouble(arrFit[iObsRow]), Convert.ToDouble(arrResidual[iObsRow]), Math.Round(Convert.ToDouble(arrResidual[iObsRow]) / dblDeNumCoeff, iDecimal) });
                        iObsRow++;
                    }
                    else
                    {
                        if(dtOrgin.Rows[i][strResponse].ToString() == string.Empty)
                            dtReturn.Rows.Add(new object[] { (int)(i + 1), "*", "*", "*", "*" });
                        else
                            dtReturn.Rows.Add(new object[] { (int)(i + 1), dtOrgin.Rows[i][strResponse], "*", "*", "*" });

                        
                        arrResult = null;
                        continue;
                    }

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

        #endregion

        #region [ Variables selection method ]

        #region Vairables Selection


        /// <summary>
        /// 변수선택법에 따라서 주어진 환경에서 변수를 선택해서 인덱스 배열을 돌려준다.
        /// </summary>
        /// <param name="dtSource">Source DataTable</param>
        /// <param name="iResponse">종속변수의 컬럼 인덱스</param>
        /// <param name="arrPredictors">설명변수의 컬럼 인덱스 목록</param>
        /// <param name="oMethodsOption">변수선택 옵션</param>
        /// <param name="bIntercept">절편유무</param>
        /// <returns>선택된 컬럼 인덱스 목록</returns>
        private int[] GetSelectedVariables(DataTable dtSource, int iResponse, int[] arrPredictors, SelectedRegressionMethods oMethodsOption, bool bIntercept)
        {
            int[] arrReturn = null;
            try
            {
                switch (oMethodsOption.ReAnalysisType)
                {
                    case RegressionType.Stepwise:
                        arrReturn = GetStepwiseSelection(dtSource, iResponse, arrPredictors, oMethodsOption.arrIncludePredictors, oMethodsOption.arrPredictorsInitialModel, oMethodsOption.IsUseAlpha, oMethodsOption.stepwiseAdd, oMethodsOption.stepwiseDrop, bIntercept);
                        break;
                    case RegressionType.Forward:
                        arrReturn = GetForwardSelection(dtSource, iResponse, arrPredictors, oMethodsOption.arrIncludePredictors, oMethodsOption.IsUseAlpha, oMethodsOption.forwardAdd, bIntercept);
                        break;
                    case RegressionType.Backward:
                        arrReturn = GetBackwardSelection(dtSource, iResponse, arrPredictors, oMethodsOption.arrIncludePredictors, oMethodsOption.IsUseAlpha, oMethodsOption.backwardDrop, bIntercept);
                        break;
                    default:
                        arrReturn = arrPredictors;  //모두 선택
                        break;
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 컬럼이름으로....나중에 바꿔서 적용 예정
        private string[] GetSelectedVariables(DataTable dtSource, string strResponse, string[] arrPredictors, SelectedRegressionMethods oMethodsOption, bool bIntercept)
        {
            return null;
        }
        #endregion

        #endregion

        #region Stepwise

        private int[] GetStepwiseSelection(DataTable dtSource, int iResponse, int[] arrPredictors, int[] arrInclude, int[] arrInitial, bool bUseAlpha, double dblAdd, double dblDrop, bool bIntercept)
        {
            int[] arrReturn = null;
            int[] arrTemp = null;
            int iRangeCnt;
            int iPredictorsCnt;
            double dblStatisticsValue;
            int iBeforeEnterCnt;  //Add Method전의 Var Cnt
            int iAfterEnterCnt;  //Add Method후의 Var Cnt
            int iBeforeDropCnt;  //Drop Method전의 Var Cnt
            int iAfterDropCnt;  //Drop Method후의 Var Cnt
            bool bEnter = true;  //들어온 것 있는지
            bool bDrop = true;  //제거된 것 있는지
            try
            {
                arrReturn = MergeArray(arrInclude, arrInitial);
                iPredictorsCnt = arrPredictors.Length;

                do  //삭제 먼저..
                {
                    iRangeCnt = arrReturn.Length;  // Drop대상이 되는 변수 개수(모델내 변수)
                    if (arrInclude != null)
                        iRangeCnt -= arrInclude.Length;
                    if (bUseAlpha)
                    {
                        dblStatisticsValue = 0;
                        #region Drop
                        iBeforeDropCnt = arrReturn.Length;

                        for (int i = 0; i < iRangeCnt; i++)
                        {
                            arrTemp = DropLastVariable(dtSource, iResponse, arrPredictors, bUseAlpha, arrReturn, arrInclude, out dblStatisticsValue, bIntercept);
                            if (double.IsNaN(dblStatisticsValue))
                            {
                                bDrop = false;
                                arrReturn = arrTemp;
                                break;
                            }
                            if (dblStatisticsValue > 0 && dblStatisticsValue < dblDrop)
                            {
                                bDrop = false;
                                break;
                            }
                            arrReturn = arrTemp;
                        }
                        iAfterDropCnt = arrReturn.Length;
                        if (iBeforeDropCnt == iAfterDropCnt)
                            bDrop = false;
                        #endregion

                        #region Enter
                        dblStatisticsValue = 0;
                        iBeforeEnterCnt = arrReturn.Length;
                        iRangeCnt = arrPredictors.Length - iBeforeEnterCnt;

                        for (int i = 0; i < iRangeCnt; i++)
                        {
                            arrTemp = AddNextVariable(dtSource, iResponse, arrPredictors, bUseAlpha, arrReturn, out dblStatisticsValue, bIntercept);
                            if (dblStatisticsValue > dblAdd)
                            {
                                break;
                            }
                            arrReturn = arrTemp;
                        }
                        iAfterEnterCnt = arrReturn.Length;
                        if (iBeforeEnterCnt == iAfterEnterCnt)
                            bEnter = false;
                        #endregion
                    }
                    else
                    {
                        dblStatisticsValue = double.MaxValue;

                        #region Drop
                        iBeforeDropCnt = arrReturn.Length;

                        for (int i = 0; i < iRangeCnt; i++)
                        {
                            arrTemp = DropLastVariable(dtSource, iResponse, arrPredictors, bUseAlpha, arrReturn, arrInclude, out dblStatisticsValue, bIntercept);
                            if (double.IsNaN(dblStatisticsValue))
                            {
                                bDrop = false;
                                arrReturn = arrTemp;
                                break;
                            }
                            if (dblStatisticsValue > 0 && dblStatisticsValue > dblDrop)
                            {
                                bDrop = false;
                                break;
                            }
                            arrReturn = arrTemp;
                        }
                        iAfterDropCnt = arrReturn.Length;
                        if (iBeforeDropCnt == iAfterDropCnt)
                            bDrop = false;
                        #endregion

                        #region Enter
                        dblStatisticsValue = double.MaxValue;
                        iBeforeEnterCnt = arrReturn.Length;
                        iRangeCnt = arrPredictors.Length - iBeforeEnterCnt;

                        for (int i = 0; i < iRangeCnt; i++)
                        {
                            arrTemp = AddNextVariable(dtSource, iResponse, arrPredictors, bUseAlpha, arrReturn, out dblStatisticsValue, bIntercept);
                            if (dblStatisticsValue < dblAdd)
                                break;
                            arrReturn = arrTemp;
                        }
                        iAfterEnterCnt = arrReturn.Length;
                        if (iBeforeEnterCnt == iAfterEnterCnt)
                            bEnter = false;
                        #endregion
                    }

                }
                while (bEnter || bDrop);

                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Forward

        private int[] GetForwardSelection(DataTable dtSource, int iResponse, int[] arrPredictors, int[] arrInclude, bool bUseAlpha, double dblAdd, bool bIntercept)
        {
            int[] arrReturn = null;
            int[] arrTemp = null;
            int iRangeCnt;
            double dblStatisticsValue;
            try
            {
                arrReturn = arrInclude;
                iRangeCnt = arrPredictors.Length;
                if (arrInclude != null && arrInclude.Length != 0)
                    iRangeCnt -= arrInclude.Length;
                if (bUseAlpha)
                {
                    dblStatisticsValue = 0;

                    for (int i = 0; i < iRangeCnt; i++)
                    {
                        arrTemp = AddNextVariable(dtSource, iResponse, arrPredictors, bUseAlpha, arrReturn, out dblStatisticsValue, bIntercept);
                        if (dblStatisticsValue > dblAdd)
                            break;
                        arrReturn = arrTemp;
                    }
                }
                else
                {
                    dblStatisticsValue = double.MaxValue;
                    for (int i = 0; i < iRangeCnt; i++)
                    {
                        arrTemp = AddNextVariable(dtSource, iResponse, arrPredictors, bUseAlpha, arrReturn, out dblStatisticsValue, bIntercept);
                        if (dblStatisticsValue < dblAdd)
                            break;
                        arrReturn = arrTemp;
                    }
                }


                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        #endregion

        #region Backward

        private int[] GetBackwardSelection(DataTable dtSource, int iResponse, int[] arrPredictors, int[] arrInclude, bool bUseAlpha, double dblDrop, bool bIntercept)
        {
            int[] arrReturn = null;
            int[] arrTemp = null;
            int iRangeCnt;
            double dblStatisticsValue;
            try
            {
                arrReturn = arrPredictors;
                iRangeCnt = arrPredictors.Length;
                if (arrInclude != null && arrInclude.Length != 0)
                    iRangeCnt -= arrInclude.Length;
                if (bUseAlpha)
                {
                    dblStatisticsValue = 0;

                    for (int i = 0; i < iRangeCnt; i++)
                    {
                        arrTemp = DropLastVariable(dtSource, iResponse, arrPredictors, bUseAlpha, arrReturn, arrInclude, out dblStatisticsValue, bIntercept);
                        if (double.IsNaN(dblStatisticsValue))
                        {
                            arrReturn = arrTemp;
                            break;
                        }
                        if (dblStatisticsValue > 0 && dblStatisticsValue < dblDrop)
                            break;
                        arrReturn = arrTemp;
                    }
                }
                else
                {
                    dblStatisticsValue = double.MaxValue;
                    for (int i = 0; i < iRangeCnt; i++)
                    {
                        arrTemp = DropLastVariable(dtSource, iResponse, arrPredictors, bUseAlpha, arrReturn, arrInclude, out dblStatisticsValue, bIntercept);
                        if (double.IsNaN(dblStatisticsValue))
                        {
                            arrReturn = arrTemp;
                            break;
                        }
                        if (dblStatisticsValue > 0 && dblStatisticsValue > dblDrop)
                            break;
                        arrReturn = arrTemp;
                    }
                }

                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Add

        private int[] AddNextVariable(DataTable dtSource, int iResponse, int[] arrPredictors, bool bUseAlpha, int[] arrSelectedIndex, out double StatisticsValue, bool bIntercept)
        {
            //최종결과
            List<int> arrReturn = null;

            #region 변수선언
            DataFrame dfOld = null; //종속변수와 이전까지 선택된 변수로 이루어진 data
            DataFrame dfAllPredictors = null; //종속변수와 모든 독립변수가 포함된 data
            LinearRegression lrNormal = null; //회귀분석
            int iColCnt;  //Source's columns count
            int iPredictorsCnt;  // Predictors count
            int iSelectedCnt; // Selected index  count
            double dblMinOrMaxValue; //이번에 추가된 변수중의 가장 낮은 P value혹은 가장큰 F값, 마지막에는 이걸 out해줌
            int iSelectedVar; //이번에 선택된 변수
            int iNonVarCnt; //이미선택되어지지 않은 변수 개수
            List<int> arrRemoveCols = null; //선택되지 않은 열 번호..
            #endregion

            try
            {
                #region 예외처리
                if (dtSource == null)
                    throw new ArgumentException(m_ErrorData, new Exception("MIRACOM"));
                iColCnt = dtSource.Columns.Count;
                if (iColCnt == 0)
                    throw new ArgumentException(m_ErrorDataLength, new Exception("MIRACOM"));
                if (arrPredictors == null)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                iPredictorsCnt = arrPredictors.Length;
                if (iPredictorsCnt == 0)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                if (iResponse < 0)
                    throw new ArgumentException(m_ErrorResponse, new Exception("MIRACOM"));
                #endregion

                dfOld = new DataFrame(dtSource);
                dfAllPredictors = new DataFrame(dtSource);

                #region dfOld 작성 (종속변수와 이전에 선택된 변수로만 이루어진 data)
                if (arrSelectedIndex == null)
                    iSelectedCnt = 0;
                else
                    iSelectedCnt = arrSelectedIndex.Length;
                arrRemoveCols = new List<int>(iColCnt - (1 + iSelectedCnt));
                if (arrSelectedIndex == null)
                    arrSelectedIndex = new int[0];
                for (int i = 0; i < iColCnt; i++)
                {
                    if (Array.IndexOf<int>(arrSelectedIndex, i) < 0)
                    {
                        if (i != iResponse)
                            arrRemoveCols.Add(i);
                    }
                }
                Subset oSet = new Subset(arrRemoveCols.ToArray());

                dfOld.RemoveColumns(oSet);

                #endregion

                iNonVarCnt = arrRemoveCols.Count;  // 선택되어 지지 않은 변수 개수

                if (iSelectedCnt != 0)
                {
                    lrNormal = new LinearRegression(dfOld, dfOld.IndexOfColumn(dtSource.Columns[iResponse].ColumnName), bIntercept);
                    arrReturn = new List<int>(iSelectedCnt + 1);
                    arrReturn.AddRange(arrSelectedIndex);
                }
                else
                {
                    arrReturn = new List<int>(1);
                }

                iSelectedVar = -1;
                if (bUseAlpha)
                {
                    #region By alpha

                    dblMinOrMaxValue = 1;  //가장 작은 값 넣어 두는 곳..
                    LinearRegressionParameter oParameter;
                    if (iSelectedCnt != 0)
                    {
                        for (int i = 0; i < iNonVarCnt; i++)
                        {
                            lrNormal.AddPredictor(((IDFColumn)dfAllPredictors[arrRemoveCols[i]]).ToDoubleVector());
                            oParameter = new LinearRegressionParameter(lrNormal, lrNormal.NumberOfParameters - 1);
                            if (!double.IsInfinity(oParameter.StandardError))
                            {
                                if (dblMinOrMaxValue > oParameter.TStatisticPValue(0.0))
                                {
                                    dblMinOrMaxValue = oParameter.TStatisticPValue(0.0);
                                    iSelectedVar = arrRemoveCols[i];
                                }
                            }
                            else
                            {
                                throw new Exception(m_ErrorDataNotEnough, new Exception("MIRACOM"));
                            }
                            oParameter = null;
                            lrNormal.RemovePredictor(lrNormal.PredictorMatrix.Cols - 1);
                        }
                    }
                    else  //초기 모형에 아무것도 없는 경우
                    {
                        DoubleMatrix dm;
                        for (int i = 0; i < iNonVarCnt; i++)
                        {
                            dm = new DoubleMatrix(((IDFColumn)dfAllPredictors[arrRemoveCols[i]]).ToDoubleVector());
                            lrNormal = new LinearRegression(dm, ((IDFColumn)dfOld[0]).ToDoubleVector(), bIntercept);
                            oParameter = new LinearRegressionParameter(lrNormal, lrNormal.NumberOfParameters - 1);
                            if (!double.IsInfinity(oParameter.StandardError))
                            {
                                if (dblMinOrMaxValue > oParameter.TStatisticPValue(0.0))
                                {
                                    dblMinOrMaxValue = oParameter.TStatisticPValue(0.0);
                                    iSelectedVar = arrRemoveCols[i];
                                }
                            }
                            else
                            {
                                throw new Exception(m_ErrorDataNotEnough, new Exception("MIRACOM"));
                            }
                            dm = null;
                            oParameter = null;
                            lrNormal = null;
                        }
                    }

                    #endregion

                }
                else // F통계량 사용일 경우
                {
                    #region By F
                    dblMinOrMaxValue = 0;
                    if (iSelectedCnt != 0)
                    {
                        LinearRegressionAnova AnovaOld = new LinearRegressionAnova(lrNormal);
                        if (AnovaOld.ErrorDegreesOfFreedom == 0)
                            throw new Exception(m_ErrorDataNotEnough, new Exception("MIRACOM"));
                        LinearRegressionAnova AnovaNew;
                        for (int i = 0; i < iNonVarCnt; i++)
                        {
                            lrNormal.AddPredictor(((IDFColumn)dfAllPredictors[arrRemoveCols[i]]).ToDoubleVector());
                            AnovaNew = new LinearRegressionAnova(lrNormal);
                            if (AnovaNew.ErrorDegreesOfFreedom == 0)
                                throw new Exception(m_ErrorDataNotEnough, new Exception("MIRACOM"));
                            double dblF = (AnovaOld.ResidualSumOfSquares - AnovaNew.ResidualSumOfSquares) / AnovaNew.MeanSquaredResidual;
                            if (dblMinOrMaxValue < dblF)
                            {
                                dblMinOrMaxValue = dblF;
                                iSelectedVar = arrRemoveCols[i];
                            }
                            AnovaNew = null;
                            lrNormal.RemovePredictor(lrNormal.PredictorMatrix.Cols - 1);
                        }
                    }
                    else //초기 모형에 아무것도 없는 경우
                    {
                        LinearRegressionAnova AnovaNew;
                        DoubleMatrix dm;
                        for (int i = 0; i < iNonVarCnt; i++)
                        {
                            dm = new DoubleMatrix(((IDFColumn)dfAllPredictors[arrRemoveCols[i]]).ToDoubleVector());
                            lrNormal = new LinearRegression(dm, ((IDFColumn)dfOld[0]).ToDoubleVector(), bIntercept);
                            AnovaNew = new LinearRegressionAnova(lrNormal);
                            if (AnovaNew.ErrorDegreesOfFreedom == 0)
                                throw new Exception(m_ErrorDataNotEnough, new Exception("MIRACOM"));
                            double dblF = AnovaNew.FStatistic;
                            if (dblMinOrMaxValue < dblF)
                            {
                                dblMinOrMaxValue = dblF;
                                iSelectedVar = arrRemoveCols[i];
                            }
                            dm = null;
                            AnovaNew = null;
                            lrNormal = null;
                        }
                    }
                    #endregion
                }

                StatisticsValue = dblMinOrMaxValue;
                arrReturn.Add(iSelectedVar);  //이번에 선택된 변수 추가

                return arrReturn.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Drop

        private int[] DropLastVariable(DataTable dtSource, int iResponse, int[] arrPredictors, bool bUseAlpha, int[] arrSelectedIndex, int[] arrInclude, out double StatisticsValue, bool bIntercept)
        {
            //최종결과
            List<int> arrReturn = null;


            #region 변수선언
            //종속변수와 이전까지 선택된 변수로 이루어진 data
            DataFrame dfOld = null;
            //종속변수와 모든 독립변수가 포함된 data
            DataFrame dfAllPredictors = null;

            LinearRegression lrNormal = null; //회귀분석
            // LinearRegressionAnova anovaNormal = null;  //회귀분석에 대한 분산분석

            //new
            int iColCnt;  //Source's columns count
            int iPredictorsCnt;  // Predictors count
            int iSelectedCnt; // Selected index  count
            double dblMinOrMaxValue; //이번에 추가된 변수중의 가장 낮은 P value혹은 가장큰 F값, 마지막에는 이걸 out해줌
            int iSelectedVar; //이번에 선택된 변수
            int iNonVarCnt; //이미선택되어지지 않은 변수 개수
            int iIncludeCnt; //필수선택 변수 개수

            string strResponse;
            string[] arrIncludeName = null;
            //선택되지 않은 열 번호..
            List<int> arrRemoveCols = null;
            #endregion

            try
            {
                #region 예외처리
                if (dtSource == null)
                    throw new ArgumentException(m_ErrorData, new Exception("MIRACOM"));
                iColCnt = dtSource.Columns.Count;
                if (iColCnt == 0)
                    throw new ArgumentException(m_ErrorDataLength, new Exception("MIRACOM"));
                if (arrPredictors == null)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                iPredictorsCnt = arrPredictors.Length;
                if (iPredictorsCnt == 0)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                if (iResponse < 0)
                    throw new ArgumentException(m_ErrorResponse, new Exception("MIRACOM"));

                if (arrSelectedIndex == null)
                    iSelectedCnt = 0;
                else
                    iSelectedCnt = arrSelectedIndex.Length;

                if (arrInclude == null)
                    iIncludeCnt = 0;
                else
                    iIncludeCnt = arrInclude.Length;

                if (iSelectedCnt == 0)
                {
                    if (iIncludeCnt > 0)
                    {
                        throw new ArgumentException(m_ErrorSystemIncludeNotSelected);  //필수 변수가 있음에도 선택되지 않았음.  개발자 실수임...
                    }
                    else
                    {
                        StatisticsValue = double.NaN;
                        return null;  //선택된 것이 없기때문에 제거할 필요도 없이 리턴함..
                    }
                }
                else
                {
                    if (iSelectedCnt < iIncludeCnt)
                    {
                        throw new ArgumentException(m_ErrorSystemIncludeNotSelected);//필수 변수가 있음에도 선택되지 않았음.  개발자 실수임...
                    }
                    else if (iSelectedCnt == iIncludeCnt)
                    {
                        if (EqualArrayFactor(arrSelectedIndex, arrInclude))  //선택된 변수가 필수 변수와 같다면 그냥 리턴
                        {
                            StatisticsValue = double.NaN;
                            return arrSelectedIndex;
                        }
                    }
                }

                #endregion

                dfOld = new DataFrame(dtSource);
                dfAllPredictors = new DataFrame(dtSource);

                strResponse = dtSource.Columns[iResponse].ColumnName;
                if (iIncludeCnt > 0)
                {
                    arrIncludeName = new string[iIncludeCnt];
                    for (int i = 0; i < iIncludeCnt; i++)
                    {
                        arrIncludeName[i] = dtSource.Columns[arrInclude[i]].ColumnName;
                    }
                }

                #region dfOld 작성 (종속변수와 이전에 선택된 변수로만 이루어진 data)
                arrRemoveCols = new List<int>(iColCnt - (1 + iSelectedCnt));
                if (arrSelectedIndex == null)
                    arrSelectedIndex = new int[0];
                for (int i = 0; i < iColCnt; i++)
                {
                    if (Array.IndexOf<int>(arrSelectedIndex, i) < 0)
                    {
                        if (i != iResponse)
                            arrRemoveCols.Add(i);
                    }
                }
                Subset oSet = new Subset(arrRemoveCols.ToArray());

                dfOld.RemoveColumns(oSet);

                #endregion

                iNonVarCnt = arrRemoveCols.Count;  // 선택되어 지지 않은 변수 개수


                lrNormal = new LinearRegression(dfOld, dfOld.IndexOfColumn(strResponse), bIntercept);
                arrReturn = new List<int>(iSelectedCnt);
                arrReturn.AddRange(arrSelectedIndex);

                iSelectedVar = -1;

                if (bUseAlpha)
                {
                    #region By alpha

                    dblMinOrMaxValue = 0;  //가장 작은 값 넣어 두는 곳..
                    LinearRegressionParameter oParameter;
                    bool bResponse = false;
                    for (int i = 0; i < iSelectedCnt + 1; i++)  // dfOld의 컬럼을 돌면내서...
                    {
                        if (dfOld.IndexOfColumn(strResponse) != i) // 종속변수가 아닌지...
                        {
                            if (arrIncludeName != null)
                            {
                                if (Array.IndexOf<string>(arrIncludeName, ((IDFColumn)dfOld[i]).Name) > -1)
                                    continue;
                            }
                        }
                        else  //종속변수임...
                        {
                            bResponse = true;
                            continue;
                        }

                        if (bResponse)
                        {
                            if (bIntercept)
                                oParameter = new LinearRegressionParameter(lrNormal, i);
                            else
                                oParameter = new LinearRegressionParameter(lrNormal, i - 1);
                        }
                        else
                            if (bIntercept)
                                oParameter = new LinearRegressionParameter(lrNormal, i + 1);
                            else
                                oParameter = new LinearRegressionParameter(lrNormal, i);
                        if (!(dblMinOrMaxValue < 0))
                        {
                            if (double.IsInfinity(oParameter.StandardError))
                            {
                                throw new Exception(m_ErrorDataNotEnough, new Exception("MIRACOM"));
                            }
                            else
                            {
                                if (dblMinOrMaxValue < oParameter.TStatisticPValue(0.0))
                                {
                                    dblMinOrMaxValue = oParameter.TStatisticPValue(0.0);
                                    iSelectedVar = dtSource.Columns.IndexOf(((IDFColumn)dfOld[i]).Name);
                                }
                            }
                        }
                        oParameter = null;
                    }
                    #endregion

                }
                else // F통계량 사용일 경우
                {
                    #region By F
                    dblMinOrMaxValue = double.MaxValue;
                    bool bResponse = false;
                    LinearRegressionAnova AnovaOld = new LinearRegressionAnova(lrNormal);
                    if(AnovaOld.ErrorDegreesOfFreedom == 0)
                        throw new Exception(m_ErrorDataNotEnough, new Exception("MIRACOM"));

                    if (iSelectedCnt == 1)
                    {
                        for (int i = 0; i < iSelectedCnt + 1; i++)
                        {
                            if (dfOld.IndexOfColumn(strResponse) != i) // 종속변수아님
                            {
                                double dblF = AnovaOld.FStatistic;
                                if (dblMinOrMaxValue > dblF)
                                {
                                    dblMinOrMaxValue = dblF;
                                    iSelectedVar = dtSource.Columns.IndexOf(((IDFColumn)dfOld[i]).Name);
                                }
                            }
                        }
                    }
                    else
                    {
                        LinearRegressionAnova AnovaNew;

                        for (int i = 0; i < iSelectedCnt + 1; i++)
                        {
                            if (dfOld.IndexOfColumn(strResponse) != i) // 종속변수가 아닌지...
                            {
                                if (arrIncludeName != null)
                                {
                                    if (Array.IndexOf<string>(arrIncludeName, ((IDFColumn)dfOld[i]).Name) > -1)
                                        continue;
                                }
                            }
                            else  //종속변수임...
                            {
                                bResponse = true;
                                continue;
                            }

                            if (bResponse)
                                lrNormal.RemovePredictor(i - 1);
                            else
                                lrNormal.RemovePredictor(i);
                            AnovaNew = new LinearRegressionAnova(lrNormal);
                            if (AnovaNew.ErrorDegreesOfFreedom == 0)
                                throw new Exception(m_ErrorDataNotEnough, new Exception("MIRACOM"));
                            double dblF = (AnovaNew.ResidualSumOfSquares - AnovaOld.ResidualSumOfSquares) / AnovaOld.MeanSquaredResidual;
                            if (dblMinOrMaxValue > dblF)
                            {
                                dblMinOrMaxValue = dblF;
                                iSelectedVar = dtSource.Columns.IndexOf(((IDFColumn)dfOld[i]).Name);
                            }
                            AnovaNew = null;
                            lrNormal = null;
                            AnovaOld = null;
                            lrNormal = new LinearRegression(dfOld, dfOld.IndexOfColumn(strResponse), bIntercept);
                            AnovaOld = new LinearRegressionAnova(lrNormal);
                        }
                    }
                    #endregion
                }

                StatisticsValue = dblMinOrMaxValue;
                arrReturn.Remove(iSelectedVar);  //이번에 제거된 변수 제거

                return arrReturn.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region [ Regression analysis method ]

        private ResultRegression RegressionAnalysis(DataTable dtSource, int iResponse, int[] arrPredictors, int[] arrMappingGraph, bool bIntercept, SelectedRegressionOptions oOption, DataTable dtObs, DataTable dtOrgin)
        {
            DataFrame df = null;
            //DoubleMatrix dm = null;

            #region Result
            LinearRegression lrNormal = null; //회귀분석
            LinearRegressionAnova anovaNormal = null;  //회귀분석에 대한 분산분석
            LinearRegressionParameter[] arrParaInfo = null;  // 회귀분석 모형 내의 계수에 대한 정보
            double[] arrResponse = null;
            double[] arrResiduals = null;
            double[] arrFit = null;
            string[] arrPredictorsName = null;
            string strRegressionEquation = string.Empty;
            #endregion


            //new
            int iColCnt;  //Source's columns count
            int iPredictorsCnt;  // Predictors count
            List<int> arrRemoveCols = null;
            ResultRegression oReturn;

            int[] arrPredictorsInRegression = null;  //회귀모형속에서 종속변수의 위치
            int iResponseDecimal;
            int iResidualsDecimal;

            try
            {
                #region 예외처리
                if (dtSource == null)
                    throw new ArgumentException(m_ErrorData, new Exception("MIRACOM"));
                iColCnt = dtSource.Columns.Count;
                if (iColCnt == 0)
                    throw new ArgumentException(m_ErrorDataLength, new Exception("MIRACOM"));
                if (arrPredictors == null)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                iPredictorsCnt = arrPredictors.Length;
                if (iPredictorsCnt == 0)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                if (iResponse < 0)
                    throw new ArgumentException(m_ErrorResponse, new Exception("MIRACOM"));
                #endregion

                df = new DataFrame(dtSource);
                //dm = new DoubleMatrix(dtSource);

                #region 필요없는 열 삭제
                arrRemoveCols = new List<int>(iColCnt - (1 + iPredictorsCnt));
                for (int i = 0; i < iColCnt; i++)
                {
                    if (Array.IndexOf<int>(arrPredictors, i) < 0)
                    {
                        if (i != iResponse)
                            arrRemoveCols.Add(i);
                    }
                }
                Subset oSet = new Subset(arrRemoveCols.ToArray());
                df.RemoveColumns(oSet);
                #endregion


                iResponseDecimal = GetDecimalPlace(dtSource, iResponse);
                iResidualsDecimal = GetDecimalPlace(dtSource) + 2;


                lrNormal = new LinearRegression(df, df.IndexOfColumn(dtSource.Columns[iResponse].ColumnName), bIntercept);
                if(!lrNormal.IsGood)
                {
                    oReturn = new ResultRegression();
                    oReturn.ErrorInformation = lrNormal.ParameterCalculationErrorMessage;
                    return oReturn;
                }
                anovaNormal = new LinearRegressionAnova(lrNormal);
                arrPredictorsInRegression = GetIndexInRegession(arrPredictors, bIntercept);
                arrParaInfo = lrNormal.ParameterEstimates;
                arrResiduals = lrNormal.Residuals.ToArray();
                for (int i = 0; i < arrResiduals.Length; i++)
                    arrResiduals[i] = Math.Round(arrResiduals[i], iResidualsDecimal);
                arrPredictorsName = GetPredictorsName(dtSource, arrPredictors, bIntercept);
                arrResponse = lrNormal.Observations.ToArray();
                //arrFit = GetSumArray(arrResponse, arrResiduals);
                arrFit = lrNormal.PredictedObservations(lrNormal.PredictorMatrix).ToArray();  //김현태 윗줄과 비해 볼것!!
                //for (int i = 0; i < arrFit.Length; i++)
                //    arrFit[i] = Math.Round(arrFit[i], iResponseDecimal);


                oReturn = new ResultRegression();
                
                oReturn.RegressionEquation = GetRegressionEquation(arrParaInfo, dtSource.Columns[iResponse].ColumnName, arrPredictorsName, arrPredictorsInRegression, m_iDecimalOuter);
                oReturn.TableforGraphs = GetTableforGraphs(dtSource, arrResiduals, arrFit, oOption);
                if (oOption.IsTableAnova)
                    oReturn.TableofAnova = GetTableofAnova(anovaNormal);
                if (oOption.IsTableCoefficients)
                    oReturn.TableofCoefficients = GetTableofCoefficients(arrParaInfo, arrPredictorsName, arrPredictorsInRegression);
                if (oOption.IsTableR_Square)
                    oReturn.TableofRsquared = GetTableofRsquared(anovaNormal);
                if (oOption.IsTableResiduals)
                    oReturn.TableofFitsResiduals = GetTableofFitsResiduals(dtOrgin, dtObs, lrNormal, dtSource.Columns[iResponse].ColumnName, arrFit, arrResiduals, arrResponse);

                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 컬럼이름으로....나중에 바꿔서 적용 예정

        private void RegressionAnalysis(DataTable dtSource, string strResponse, string[] arrPredictors, bool bIntercept)
        {
            DataFrame df = null;
            //DoubleMatrix dm = null;

            LinearRegression lrNormal = null; //회귀분석
            LinearRegressionAnova anovaNormal = null;  //회귀분석에 대한 분산분석

            //new
            int iColCnt;  //Source's columns count
            int iPredictorsCnt;  // Predictors count
            List<int> arrRemoveCols = null;

            try
            {
                #region 예외처리
                if (dtSource == null)
                    throw new ArgumentException(m_ErrorData, new Exception("MIRACOM"));
                iColCnt = dtSource.Columns.Count;
                if (iColCnt == 0)
                    throw new ArgumentException(m_ErrorDataLength, new Exception("MIRACOM"));
                if (arrPredictors == null)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                iPredictorsCnt = arrPredictors.Length;
                if (iPredictorsCnt == 0)
                    throw new ArgumentException(m_ErrorPredictor, new Exception("MIRACOM"));
                if (strResponse == null || strResponse == string.Empty)
                    throw new ArgumentException(m_ErrorResponse, new Exception("MIRACOM"));
                #endregion

                df = new DataFrame(dtSource);
                //dm = new DoubleMatrix(dtSource);

                #region 필요없는 열 삭제
                arrRemoveCols = new List<int>(iColCnt - (1 + iPredictorsCnt));
                for (int i = 0; i < iColCnt; i++)
                {
                    if (Array.IndexOf<string>(arrPredictors, dtSource.Columns[i].ColumnName) < 0)
                    {
                        if (dtSource.Columns[i].ColumnName != strResponse)
                            arrRemoveCols.Add(i);
                    }
                }
                Subset oSet = new Subset(arrRemoveCols.ToArray());
                df.RemoveColumns(oSet);
                #endregion

                lrNormal = new LinearRegression(df, df.IndexOfColumn(strResponse), bIntercept);
                anovaNormal = new LinearRegressionAnova(lrNormal);
                #region TEST
                /*
                lrNormal.RemovePredictor(2);
                lrNormal.RecalculateParameters();
                lrNormal.AddPredictor(((IDFColumn)df[3]).ToDoubleVector());
                lrNormal.RecalculateParameters();
                 */
                #endregion

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
