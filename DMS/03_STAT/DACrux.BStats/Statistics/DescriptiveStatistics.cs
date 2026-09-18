using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using DACrux.BStats.StatisticsInput;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.Statistics
{
    /// <summary>
    /// For DACrux Solution
    /// </summary>
    public class DescriptiveStatistics
    {
        #region " MEMBER FIELD "

        /// <summary>
        /// Input Option
        /// </summary>
        private inputDescriptiveStatistics m_Input = null;

        #region [ LANGUAGE ]
        string m_Variable = "Variable";

        string m_Mean = "Mean";
        string m_MeanSE = "SE Mean";
        string m_Standarddeviation = "StDev";
        string m_Variance = "Variance";
        string m_Coefficientofvariation = "CoefVar";
        string m_TrimmedMean = "TrMean";
        string m_Sum = "Sum";
        string m_Min = "Minimum";
        string m_Max = "Maximum";
        string m_Range = "Range";
        string m_Nnonmissing = "N";
        string m_Nmissing = "N*";
        string m_Ntotal = "Total Count";
        string m_CumulativeN = "CumN";
        string m_Percent = "Percent";
        string m_CumulativePercent = "CumPct";
        string m_Q1 = "Q1";
        string m_Median = "Median";
        string m_Q3 = "Q3";
        string m_InterquartileRange = "IQR";
        string m_Mode = "Mode";
        string m_ModeN = "N for Mode";
        string m_SumofSquares = "Sum of Squares";
        string m_Skewness = "Skewness";
        string m_Kurtosis = "Kurtosis";
        string m_MSSD = "MSSD";

        #endregion

        /// <summary>
        /// Result For Html File
        /// </summary>
        private DACruxSet m_dsResult = null;

        /// <summary>
        /// Raw Data For Descriptive Statistics
        /// </summary>
        private DACruxSet[] m_RawData = null;

        #region [ CHART INFO ]
        private GraphInformation[] m_BoxPlot = null;

        private GraphInformation[] m_Histogram = null;

        private GraphInformation[] m_RawDataPlot = null;

        private int m_DescriptionHistogramH = 300;
        private int m_DescriptionHistogramW = 400;
        private int m_DescriptionBoxplotW = 300;
        #endregion


        private DACrux.BStats.Core.SelectedStatistics m_SelectedStatistics;

        private const string m_Missing = "Not_assigned";
        #endregion

        #region " CREATOR "
        public DescriptiveStatistics(inputDescriptiveStatistics oInput)
        {
            #region Group column data type and missing value
            oInput.DataSource = GetNonMissingGroup(oInput.DataSource, oInput.arrByVariables);
            #endregion
            m_Input = oInput;
            m_SelectedStatistics = new DACrux.BStats.Core.SelectedStatistics();
            m_SelectedStatistics.Reset();
            m_SelectedStatistics.IsCoefficientofvariation = m_Input.IsCoefficientofvariation;
            m_SelectedStatistics.IsCumulativeN = m_Input.IsCumulativeN;
            m_SelectedStatistics.IsCumulativePercent = m_Input.IsCumulativePercent;
            m_SelectedStatistics.IsInterquartileRange = m_Input.IsInterquartileRange;
            m_SelectedStatistics.IsKurtosis = m_Input.IsKurtosis;
            m_SelectedStatistics.IsMax = m_Input.IsMax;
            m_SelectedStatistics.IsMean = m_Input.IsMean;
            m_SelectedStatistics.IsMeanSE = m_Input.IsMeanSE;
            m_SelectedStatistics.IsMedian = m_Input.IsMedian;
            m_SelectedStatistics.IsMin = m_Input.IsMin;
            m_SelectedStatistics.IsMode = m_Input.IsMode;
            m_SelectedStatistics.IsMSSD = m_Input.IsMSSD;
            m_SelectedStatistics.IsNmissing = m_Input.IsNmissing;
            m_SelectedStatistics.IsNnonmissing = m_Input.IsNnonmissing;
            m_SelectedStatistics.IsNtotal = m_Input.IsNtotal;
            m_SelectedStatistics.IsPercent = m_Input.IsPercent;
            m_SelectedStatistics.IsQ1 = m_Input.IsQ1;
            m_SelectedStatistics.IsQ3 = m_Input.IsQ3;
            m_SelectedStatistics.IsRange = m_Input.IsRange;
            m_SelectedStatistics.IsSkewness = m_Input.IsSkewness;
            m_SelectedStatistics.IsStandarddeviation = m_Input.IsStandarddeviation;
            m_SelectedStatistics.IsSum = m_Input.IsSum;
            m_SelectedStatistics.IsSumofSquares = m_Input.IsSumofSquares;
            m_SelectedStatistics.IsTrimmedMean = m_Input.IsTrimmedMean;
            m_SelectedStatistics.IsVariance = m_Input.IsVariance;
            m_RawData = GetSource();
            m_dsResult = GetAnalysisResult(m_RawData);
            if (m_Input.IsBoxPlot)
            {
                m_BoxPlot = DrawBoxPlot(MergeForBoxPlot(m_RawData));
                int idx = 0;
                for (int i = 0; i < m_dsResult.DACruxTable[0].DataTable.Rows.Count; i++)
                {
                    if (m_dsResult.DACruxTable[0].DataTable.Rows[i]["Variable"].ToString() != m_BoxPlot[idx].AxisYTitle) idx++;
                    m_BoxPlot[idx].USL = double.Parse(m_dsResult.DACruxTable[0].DataTable.Rows[i]["Q3"].ToString());
                    m_BoxPlot[idx].LSL = double.Parse(m_dsResult.DACruxTable[0].DataTable.Rows[i]["Q1"].ToString());
                }
            }

            if (m_Input.IsHistogram)
                m_Histogram = DrawHistogram(m_RawData, m_Input.IsHistogramNNormalCurve);
            if(m_Input.IsRawDataPlot)
                m_RawDataPlot = DrawRawDataPlot(m_RawData);

            m_Input.GraphInformations = MergeGraphInformation(m_Histogram, m_BoxPlot,m_RawDataPlot);
            MakeHtml(m_dsResult);
        }
        #endregion

        #region " METHOD "

        private GraphInformation[] MergeGraphInformation(GraphInformation[] arrHistogram, GraphInformation[] arrBoxPlot, GraphInformation[] arrRawDataPlot)
        {
            //int iHistogram = 0;
            //int iBoxplot = 0;
            //int iRawDataPlot = 0;
            //int iTotal;
            GraphInformation[] arrReturn = null;
            try
            {
                if(arrHistogram == null && arrBoxPlot == null && arrRawDataPlot == null)
                return null;


                arrReturn = new GraphInformation[arrHistogram.Length + arrBoxPlot.Length + arrRawDataPlot.Length];

                Array.Copy(arrHistogram, arrReturn, arrHistogram.Length);
                Array.Copy(arrBoxPlot,0 , arrReturn,arrHistogram.Length,arrBoxPlot.Length);
                Array.Copy(arrRawDataPlot, 0, arrReturn, (arrHistogram.Length + arrBoxPlot.Length), arrRawDataPlot.Length);

                //iHistogram=0;
                //iBoxplot=0;
                //iRawDataPlot = 0;

                //if (arrHistogram != null)
                //    iHistogram = arrHistogram.Length;
                //if (arrBoxPlot != null)
                //    iBoxplot = arrBoxPlot.Length;
                //if (arrRawDataPlot != null)
                //    iRawDataPlot = arrRawDataPlot.Length;

                //iTotal = iHistogram + iBoxplot + iRawDataPlot;
                //arrReturn = new GraphInformation[iTotal];
                //for (int i = 0; i < iTotal; i++)
                //{
                //    if (i < iHistogram)
                //        arrReturn[i] = arrHistogram[i];
                //    else
                //        arrReturn[i] = arrBoxPlot[i - iHistogram];
                //}
                return arrReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [ParentPath]
        /// <summary>
        /// 최종결과인 html file의 경로를 받아서 상위 폴더 경로를 돌려준다.
        /// </summary>
        /// <param name="strFullFilePath"></param>
        /// <returns></returns>
        private string GetParentPath(string strFullFilePath)
        {
            DirectoryInfo diFilePath = null;
            try
            {
                diFilePath = new DirectoryInfo(strFullFilePath);
                return diFilePath.Parent.FullName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [FileName]
        /// <summary>
        /// 최종결과인 html file의 경로를 받아서 경로와 확장자가 제거된 파일이름만 돌려준다.
        /// </summary>
        /// <param name="strFullFilePath"></param>
        /// <returns></returns>
        private string GetFileName(string strFullName)
        {
            FileInfo fi = new FileInfo(strFullName);
            try
            {
                return fi.Name.Remove(fi.Name.LastIndexOf(fi.Extension));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  [Html]

        private string SubTitle(string[] arrVar, string[] arrValue, string strEqual, string strSeparator)
        {            
            string strReturn = string.Empty;
            int iVarCnt;
            int iValueCnt;
            try
            {
                if (m_Input.arrByVariables.Length > 1)
                {
                    if(arrVar == null)
                        throw new ArgumentException(ErrorNullParameter("arrVar"));
                    if (arrValue == null)
                        throw new ArgumentException(ErrorNullParameter("arrValue"));
                    iVarCnt = arrVar.Length;
                    iValueCnt = arrValue.Length;
                    if (iVarCnt != iValueCnt)
                        throw new ArgumentException(ErrorNotEqualParameterLength("arrVar", "arrValue"));
                    strReturn = arrVar[0] + strEqual + arrValue[0];
                    if (iValueCnt > 1)
                    {
                        for (int i = 1; i < iValueCnt; i++)
                        {
                            strReturn += strSeparator + arrVar[i] + strEqual + arrValue[i];
                        }
                    }
                    
                    return strReturn;

                }
                else
                {
                    return string.Empty;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private string[] GetPath(GraphInformation[] arrGraph)
        {
            string[] arrReturn = null;
            int iLength;
            try
            {
                if (arrGraph == null)
                    return null;
                iLength = arrGraph.Length;
                arrReturn = new string[iLength];
                for (int i = 0; i < iLength; i++)
                {
                    arrReturn[i] = arrGraph[i].ImagePath;
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string[] GetPath(GraphInformation[] arrGraph, int iIndex)
        {
            try
            {
                if (arrGraph == null)
                    return null;
                if (arrGraph.Length - 1 < iIndex)
                {
#if DEBUG
                    MessageBox.Show("확인해봐라..김현태김현태");
#endif
                    return null;
                }               
                return new string[] { arrGraph[iIndex].ImagePath };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string[] GetPath(GraphInformation[] arrGraph, int iStart, int iEnd)
        {
            string[] arrReturn = null;
            int iLength;
            try
            {
                if (arrGraph == null)
                    return null;
                iLength = arrGraph.Length;
                arrReturn = new string[iEnd - iStart + 1];
                for (int i = iStart; i < iEnd+1; i++)
                {
                    arrReturn[i - iStart] = arrGraph[i].ImagePath;
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MakeHtml(DACruxSet dcsResult)
        {
            HtmlConverter oHtml = null;
            int iTableCnt;
            try
            {
                iTableCnt = dcsResult.DACruxTable.Count;
                oHtml = new HtmlConverter(DACrux.ProjectManager.UI.Common.TempPath + m_Input.ResultFilePath);
                oHtml.MainTitle(m_Input.Project, m_Input.WorkSheet, m_Input.User, m_Input.Title);
                
                if (m_Input.arrByVariables == null)  //분류변수 없음
                {
                    oHtml.SubTitle(m_Input.Title);
                    oHtml.BodyDescriptive(dcsResult.DACruxTable[0].DataTable, false, GetPath(m_Histogram), GetPath(m_BoxPlot),GetPath(m_RawDataPlot));
                }
                else
                {
                    if (m_Input.arrByVariables.Length == 0)  //분류변수 없음
                    {
                        oHtml.SubTitle(m_Input.Title);
                        oHtml.BodyDescriptive(dcsResult.DACruxTable[0].DataTable, false, GetPath(m_Histogram), GetPath(m_BoxPlot), GetPath(m_RawDataPlot));
                    }
                    else if (m_Input.arrByVariables.Length == 1)  //Series변수 하나만 존재
                    {
                        oHtml.SubTitle(m_Input.Title);
                        oHtml.BodyDescriptive(dcsResult.DACruxTable[0].DataTable, true, GetPath(m_Histogram), GetPath(m_BoxPlot), GetPath(m_RawDataPlot));
                    }
                    else  // Object Var과 Series Var모두 존재
                    {
                        int iHistogram = 0;
                        for (int i = 0; i < iTableCnt; i++)
                        {
                            oHtml.SubTitle(m_Input.Title + "[ " + SubTitle(dcsResult.DACruxTable[i].ObjVariable, dcsResult.DACruxTable[i].ObjVariableValue, " = ", ", ") + " ]");
                            oHtml.BodyDescriptive(dcsResult.DACruxTable[i].DataTable, true, GetPath(m_Histogram, iHistogram, iHistogram + dcsResult.DACruxTable[i].DataTable.Rows.Count-1), GetPath(m_BoxPlot, i),GetPath(m_RawDataPlot,i));
                            iHistogram += dcsResult.DACruxTable[i].DataTable.Rows.Count;
                        }
                    }
                }
                oHtml.SaveHtml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        
        #region [Chart]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iRowHeight"></param>
        /// <param name="iHistogramHeight"></param>
        /// <returns></returns>
        private int GetBoxPlotH(int iRowHeight, int iHistogramHeight)
        {
            int iRowCnt;
            int iReturn;
            try
            {
                iRowCnt = 0;
                if (m_Input.arrByVariables != null)
                {
                    if (m_Input.arrByVariables.Length > 0)
                    {
                        iRowCnt++;
                    }
                }
                if (m_Input.IsCoefficientofvariation)
                    iRowCnt++;
                if (m_Input.IsCumulativeN)
                    iRowCnt++;
                if (m_Input.IsCumulativePercent)
                    iRowCnt++;
                if (m_Input.IsInterquartileRange)
                    iRowCnt++;
                if (m_Input.IsKurtosis)
                    iRowCnt++;
                if (m_Input.IsMax)
                    iRowCnt++;
                if (m_Input.IsMean)
                    iRowCnt++;
                if (m_Input.IsMeanSE)
                    iRowCnt++;
                if (m_Input.IsMedian)
                    iRowCnt++;
                if (m_Input.IsMin)
                    iRowCnt++;
                if (m_Input.IsMode)
                {
                    iRowCnt++;//최빈수
                    iRowCnt++;//개수
                }
                if (m_Input.IsMSSD)
                    iRowCnt++;
                if (m_Input.IsNmissing)
                    iRowCnt++;
                if (m_Input.IsNnonmissing)
                    iRowCnt++;
                if (m_Input.IsNtotal)
                    iRowCnt++;
                if (m_Input.IsPercent)
                    iRowCnt++;
                if (m_Input.IsQ1)
                    iRowCnt++;
                if (m_Input.IsQ3)
                    iRowCnt++;
                if (m_Input.IsRange)
                    iRowCnt++;
                if (m_Input.IsSkewness)
                    iRowCnt++;
                if (m_Input.IsStandarddeviation)
                    iRowCnt++;
                if (m_Input.IsSum)
                    iRowCnt++;
                if (m_Input.IsSumofSquares)
                    iRowCnt++;
                if (m_Input.IsTrimmedMean)
                    iRowCnt++;
                if (m_Input.IsVariance)
                    iRowCnt++;

                iReturn = iRowCnt * iRowHeight;
                if (m_Input.IsHistogram)  //300에 해당하는만큼
                {
                    iReturn += iHistogramHeight;
                }
                return iReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private GraphInformation[] DrawHistogram(DACruxSet[] arrRowData, bool bNormalCurve)
        {
            ArrayList arrTemp = null;
            GraphInformation oGraph = null;
            int iObjCnt;  // arrRowData의 길이
            int iDataTableCnt;  // arrRowData내의 하나의 DACruxSet이 가진 Table 개수
            try
            {
                iObjCnt = arrRowData.Length;
                arrTemp = new ArrayList();
                for (int i = 0; i < iObjCnt; i++)
                {
                    iDataTableCnt = arrRowData[i].DACruxTable.Count;
                    for (int j = 0; j < iDataTableCnt; j++)
                    {
                        oGraph = new GraphInformation(GraphType.Histogram);
                        oGraph.ForceZero = false;
                        oGraph.AxisXForceZero = false;
                        oGraph.DataSource = arrRowData[i].DACruxTable[j].DataTable;
                        oGraph.DecimalPlaceX = DataTableUtil.GetDecimalPlace(arrRowData[i].DACruxTable[j].DataTable, 0);
                        oGraph.NormalLine = bNormalCurve;
                        oGraph.AddAxisY(new GraphInformation.ColumnInfoItem(0, null, arrRowData[i].DACruxTable[j].Variable, arrRowData[i].DACruxTable[j].DataTable.Columns[0].DataType));
                        oGraph.ImageSize = new System.Drawing.Size(m_DescriptionHistogramW, m_DescriptionHistogramH);
                        
                        if (m_Input.arrByVariables == null)
                        {
                            oGraph.Name = "Histogram(" + m_Input.Title + ")" + " : " + arrRowData[i].DACruxTable[j].Variable;  // 분석컬럼명만..
                            oGraph.Title = arrRowData[i].DACruxTable[j].Variable;  // 분석컬럼명만..
                            oGraph.ImagePath = GetFileName(m_Input.ResultFilePath) + "Histogram" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + i.ToString() +j.ToString() + ".jpg";
                        }                        
                        else
                        {
                            if (m_Input.arrByVariables.Length > 1)  // Obj Var와 Series Var모두 존재할때
                            {
                                oGraph.Name = "Histogram(" + m_Input.Title + ")" + " : " + SubTitle(arrRowData[i].DACruxTable[j].ObjVariable, arrRowData[i].DACruxTable[j].ObjVariableValue, "=", ", ") + " - " + SubTitle(arrRowData[i].DACruxTable[j].SeriesVariable, arrRowData[i].DACruxTable[j].SeriesVariableValue, "=", ", ") + " - " + arrRowData[i].DACruxTable[j].Variable;
                                oGraph.Title = SubTitle(arrRowData[i].DACruxTable[j].ObjVariable, arrRowData[i].DACruxTable[j].ObjVariableValue, "=", ", ");
                                oGraph.SubTitle = SubTitle(arrRowData[i].DACruxTable[j].SeriesVariable, arrRowData[i].DACruxTable[j].SeriesVariableValue, "=", ", ");
                                oGraph.ImagePath = GetFileName(m_Input.ResultFilePath) + "Histogram" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + i.ToString() +j.ToString() + ".jpg";
                            }
                            else if(m_Input.arrByVariables.Length == 0)
                            {                        
                            oGraph.Name = "Histogram(" + m_Input.Title + ")" + " : " + arrRowData[i].DACruxTable[j].Variable;  // 분석컬럼명만..
                            oGraph.Title = arrRowData[i].DACruxTable[j].Variable;  // 분석컬럼명만..
                            oGraph.ImagePath = GetFileName(m_Input.ResultFilePath) + "Histogram" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + i.ToString() +j.ToString() + ".jpg";
                            }                        
                            else if (m_Input.arrByVariables.Length == 1)
                            {
                                oGraph.Name = "Histogram(" + m_Input.Title + ")" + " : " + SubTitle(arrRowData[i].DACruxTable[j].SeriesVariable, arrRowData[i].DACruxTable[j].SeriesVariableValue, "=", ", ") + " - " + arrRowData[i].DACruxTable[j].Variable;
                                oGraph.Title = SubTitle(arrRowData[i].DACruxTable[j].SeriesVariable, arrRowData[i].DACruxTable[j].SeriesVariableValue, "=", ", ");
                                oGraph.SubTitle = arrRowData[i].DACruxTable[j].Variable;
                                oGraph.ImagePath = GetFileName(m_Input.ResultFilePath) + "Histogram" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + i.ToString() +j.ToString() + ".jpg";
                            }
                        }                       
                        arrTemp.Add(oGraph);
                    }
                }
                return (GraphInformation[])arrTemp.ToArray(typeof(GraphInformation));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrTemp.Clear();
                arrTemp = null;
            }
        }

        private GraphInformation[] DrawBoxPlot(DACruxSet dsRawData)
        {
            int iBoxPlotCnt;
            GraphInformation[] arrReturn = null;
            int iColCnt;
            try
            {
                iBoxPlotCnt = dsRawData.DACruxTable.Count;
                arrReturn = new GraphInformation[iBoxPlotCnt];
                for (int i = 0; i < iBoxPlotCnt; i++)
                {
                    arrReturn[i] = new GraphInformation(GraphType.BoxPlot);
                    arrReturn[i].ForceZero = false;
                    arrReturn[i].AxisXForceZero = false;
                    arrReturn[i].DataSource = dsRawData.DACruxTable[i].DataTable;
                    arrReturn[i].DecimalPlace = DataTableUtil.GetDecimalPlace(dsRawData.DACruxTable[i].DataTable);
                    arrReturn[i].ImagePath = GetFileName(m_Input.ResultFilePath) + "Boxplot" + DateTime.Now.ToString("yyyyMMddHHmmss") + "--" + i.ToString() + ".jpg";
                    arrReturn[i].ImageSize = new System.Drawing.Size(m_DescriptionBoxplotW, GetBoxPlotH(22, m_DescriptionHistogramH));
                    arrReturn[i].IsMeanBoxplot = false;

                    iColCnt = dsRawData.DACruxTable[i].DataTable.Columns.Count;

                    for (int j = 0; j < iColCnt; j++)
                    {
                        arrReturn[i].AddAxisY(new GraphInformation.ColumnInfoItem(j, null, dsRawData.DACruxTable[i].DataTable.Columns[j].ColumnName, dsRawData.DACruxTable[i].DataTable.Columns[j].DataType));
                    }

                                        
                    if (m_Input.arrByVariables == null)
                    {
                        arrReturn[i].Title = dsRawData.DACruxTable[i].Variable;  // 분석컬럼명만..
                        arrReturn[i].AxisYTitle = dsRawData.DACruxTable[i].Variable;
                        arrReturn[i].Name = "BoxPlot(" + m_Input.Title + ")";
                    }
                    else 
                    {
                        if (m_Input.arrByVariables.Length == 0) 
                        {
                            arrReturn[i].Title = dsRawData.DACruxTable[i].Variable;  // 분석컬럼명만..
                            arrReturn[i].Name = "BoxPlot(" + m_Input.Title + ")" + " : " + dsRawData.DACruxTable[i].Variable;
                            arrReturn[i].AxisYTitle = dsRawData.DACruxTable[i].Variable;
                        }
                        else if (m_Input.arrByVariables.Length > 1)  // Obj Var와 Series Var모두 존재할때
                        {
                            arrReturn[i].Title = SubTitle(dsRawData.DACruxTable[i].ObjVariable, dsRawData.DACruxTable[i].ObjVariableValue, "=", ", ");
                            arrReturn[i].SubTitle = SubTitle(dsRawData.DACruxTable[i].SeriesVariable, dsRawData.DACruxTable[i].SeriesVariableValue, "=", ", ");

                            arrReturn[i].Name = "BoxPlot(" + m_Input.Title + ")" + " : " + SubTitle(dsRawData.DACruxTable[i].ObjVariable, dsRawData.DACruxTable[i].ObjVariableValue, "=", ", ") + " - " + SubTitle(dsRawData.DACruxTable[i].SeriesVariable, dsRawData.DACruxTable[i].SeriesVariableValue, "=", ", ") + " - " + dsRawData.DACruxTable[i].Variable;
                            arrReturn[i].AxisXTitle = string.Join(", ",dsRawData.DACruxTable[i].SeriesVariable);
                            arrReturn[i].AxisYTitle = dsRawData.DACruxTable[i].Variable;
                        }
                        else
                        {
                            arrReturn[i].Title = SubTitle(dsRawData.DACruxTable[i].SeriesVariable, dsRawData.DACruxTable[i].SeriesVariableValue, "=", ", ");
                            arrReturn[i].SubTitle = dsRawData.DACruxTable[i].Variable;
                            arrReturn[i].Name = "BoxPlot(" + m_Input.Title + ")" + " : " + SubTitle(dsRawData.DACruxTable[i].SeriesVariable, dsRawData.DACruxTable[i].SeriesVariableValue, "=", ", ") + " - " + dsRawData.DACruxTable[i].Variable;
                            arrReturn[i].AxisXTitle = string.Join(", ", dsRawData.DACruxTable[i].SeriesVariable);
                            arrReturn[i].AxisYTitle = dsRawData.DACruxTable[i].Variable;
                        }
                    }

                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private GraphInformation[] DrawRawDataPlot(DACruxSet[] dsRawData)
        {
            int iRawDataPlotCnt;
            GraphInformation[] arrReturn = null;
            //int iColCnt;
            double yValue = double.NaN;
            double yLastValue = double.NaN;
            //System.Data.DataView dv = null;
            string strSortExpression = string.Empty;
            DataTable dtTemp = null;
            try
            {
                iRawDataPlotCnt = dsRawData[0].DACruxTable.Count;
                arrReturn = new GraphInformation[iRawDataPlotCnt];
                for (int i = 0; i < iRawDataPlotCnt; i++)
                {
                    string strXColName = dsRawData[0].DACruxTable[i].DataTable.Columns[0].ColumnName;
                    dtTemp = dsRawData[0].DACruxTable[i].DataTable.Copy();
                    dtTemp.Columns.Add("RANK", typeof(int));
                    int key = 0;
                    for (int j = 0; j < dtTemp.Rows.Count; j++)
                    {
                        yValue = double.NaN;
                        double.TryParse(dtTemp.Rows[j][strXColName].ToString(), out yValue);

                        if (yValue != yLastValue)
                        {
                            dtTemp.Rows[j]["RANK"] = ++key;
                        }
                        else
                        {
                            dtTemp.Rows[j]["RANK"] = key;
                        }
                    }

                    arrReturn[i] = new GraphInformation(GraphType.Scatter);
                    arrReturn[i].DataSource = dtTemp;
                    //arrReturn[i].Name = "RawTrend(" + m_Input.Title + ")";
                    arrReturn[i].Name = "RawTrend(" + m_Input.Title + ") : " + dtTemp.Columns[0].ColumnName;
                    arrReturn[i].AxisX = new GraphInformation.ColumnInfoItem(0, null, dtTemp.Columns[0].ColumnName, dtTemp.Columns[0].DataType);
                    arrReturn[i].AddAxisY(new GraphInformation.ColumnInfoItem(1, null, dtTemp.Columns[1].ColumnName, dtTemp.Columns[1].DataType));
                    arrReturn[i].AxisXTitle = dtTemp.Columns[0].ColumnName;
                    arrReturn[i].AxisYTitle = " ";
                    arrReturn[i].GridLine = false;
                    arrReturn[i].PointColor = System.Drawing.Color.Red;
                    arrReturn[i].ForceZero = false;
                    arrReturn[i].ViewProperty = true;
                    arrReturn[i].Title = dsRawData[0].DACruxTable[i].Variable;  // 분석컬럼명만..
                    arrReturn[i].DecimalPlace = DataTableUtil.GetDecimalPlace(dtTemp);
                    arrReturn[i].ImagePath = GetFileName(m_Input.ResultFilePath) + "RawDataPlot" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + i.ToString() + ".jpg";
                    arrReturn[i].ImageSize = new System.Drawing.Size(m_DescriptionBoxplotW, GetBoxPlotH(22, m_DescriptionHistogramH));

                    //OLD YSIM 2014-10-14
                    //arrReturn[i] = new GraphInformation(GraphType.Point);
                    //arrReturn[i].ForceZero = false;
                    //arrReturn[i].AxisXForceZero = true;
                    //arrReturn[i].DataSource = dsRawData[0].DACruxTable[i].DataTable;
                    //arrReturn[i].DecimalPlace = DataTableUtil.GetDecimalPlace(dsRawData[0].DACruxTable[i].DataTable);
                    //arrReturn[i].ImagePath = GetFileName(m_Input.ResultFilePath) + "RawDataPlot" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + i.ToString() + ".jpg";
                    //arrReturn[i].ImageSize = new System.Drawing.Size(m_DescriptionBoxplotW, GetBoxPlotH(22, m_DescriptionHistogramH));
                    //arrReturn[i].GridLine = true;
                    //arrReturn[i].PointColor = System.Drawing.Color.Red;

                    //iColCnt = dsRawData[0].DACruxTable[i].DataTable.Columns.Count;

                    //for (int j = 0; j < iColCnt; j++)
                    //{
                    //    arrReturn[i].AddAxisY(new GraphInformation.ColumnInfoItem(j, null, dsRawData[0].DACruxTable[i].DataTable.Columns[j].ColumnName, dsRawData[0].DACruxTable[i].DataTable.Columns[j].DataType));
                    //}

                    //arrReturn[i].Title = dsRawData[0].DACruxTable[i].Variable;  // 분석컬럼명만..
                    //arrReturn[i].AxisYTitle = dsRawData[0].DACruxTable[i].Variable;
                    //arrReturn[i].Name = "RawTrend(" + m_Input.Title + ")";
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DACruxSet MergeForBoxPlot(DACruxSet[] arrRowData)
        {
            DACruxSet dsReturn = null;
            DACruxTable dtTemp = null;
            int iObjCnt;
            int iDataTableCnt;
            string strVar;
            int iMaxRow;
            int iStartTable;
            int iColCnt;
            object[] arrTempRow = null;
            try                
            {
                iObjCnt = arrRowData.Length;

                dsReturn = new DACruxSet();
                dsReturn.ObjVariable = arrRowData[0].ObjVariable;
                dsReturn.SeriesVariable = arrRowData[0].SeriesVariable;
                
                for (int i = 0; i < iObjCnt; i++)
                {
                    iDataTableCnt = arrRowData[i].DACruxTable.Count;
                    iMaxRow = 0;
                    iColCnt = 0;
                    strVar = arrRowData[i].DACruxTable[0].Variable;

                    #region Return DataTable Name
                    dtTemp = new DACruxTable();
                    dtTemp.Variable = strVar;
                    dtTemp.ObjVariable = arrRowData[i].ObjVariable;
                    dtTemp.ObjVariableValue = arrRowData[i].ObjVariableValue;
                    
                    //if (m_Input.arrByVariables != null)
                    //{
                    //    if (m_Input.arrByVariables.Length > 1)// Obj Var와 Series Var모두 존재할때
                    //    {
                            
                    //    }
                    //    else if (m_Input.arrByVariables.Length == 1)
                    //    {
                    //        dtTemp = new DataTable(arrRowData[i].Tables[0].TableName);
                    //    }
                    //}
                    #endregion

                    iStartTable = 0;
                    iColCnt = 0;
                    for (int j = 0; j < iDataTableCnt; j++)
                    {
                        dtTemp.SeriesVariable = arrRowData[i].DACruxTable[j].SeriesVariable;
                        dtTemp.SeriesVariableValue = arrRowData[i].DACruxTable[j].SeriesVariableValue;
                        if (strVar == arrRowData[i].DACruxTable[j].Variable)
                        {
                            if (iMaxRow < arrRowData[i].DACruxTable[j].DataTable.Rows.Count)
                                iMaxRow = arrRowData[i].DACruxTable[j].DataTable.Rows.Count;

                            if (m_Input.arrByVariables == null)
                            {
                                dtTemp.DataTable.Columns.Add(arrRowData[i].DACruxTable[j].Variable, arrRowData[i].DACruxTable[j].DataTable.Columns[0].DataType);
                            }
                            else
                            {
                                if (m_Input.arrByVariables.Length == 0)
                                {
                                    dtTemp.DataTable.Columns.Add(arrRowData[i].DACruxTable[j].Variable, arrRowData[i].DACruxTable[j].DataTable.Columns[0].DataType);
                                }
                                else if (m_Input.arrByVariables.Length > 0)
                                {
                                    dtTemp.DataTable.Columns.Add(string.Join(", ",arrRowData[i].DACruxTable[j].SeriesVariableValue), arrRowData[i].DACruxTable[j].DataTable.Columns[0].DataType);
                                }                                
                            }
                            
                            iColCnt++;  //같은 Variable을 가진 Table수
                        }
                        else
                        {                           
                            // DataTable에 Row Add후에 결과 DataSet에 Add
                            dtTemp.DataTable.AcceptChanges();
                            for (int k = 0; k < iMaxRow; k++)  //가장 Rows Count가 많은 DataTable기준
                            {
                                arrTempRow = new object[iColCnt];
                                for (int l = iStartTable; l < j; l++)
                                {
                                    if(k<arrRowData[i].DACruxTable[l].DataTable.Rows.Count)
                                    {
                                        arrTempRow[l-iStartTable] = arrRowData[i].DACruxTable[l].DataTable.Rows[k][0];
                                    }
                                }
                                dtTemp.DataTable.Rows.Add(arrTempRow);
                                arrTempRow = null;
                            }

                            iStartTable = j;
                            // DataTable에 Row Add후에 결과 DataSet에 Add
                            strVar = arrRowData[i].DACruxTable[j].Variable;
                            iMaxRow = 0;
                            if (iMaxRow < arrRowData[i].DACruxTable[j].DataTable.Rows.Count)
                                iMaxRow = arrRowData[i].DACruxTable[j].DataTable.Rows.Count;
                            dsReturn.DACruxTable.Add(dtTemp);
                            dtTemp = null; 
                            iColCnt = 0;

                            #region Return DataTable Name
                            dtTemp = new DACruxTable();
                            dtTemp.Variable = strVar;
                            dtTemp.ObjVariable = arrRowData[i].ObjVariable;
                            dtTemp.ObjVariableValue = arrRowData[i].ObjVariableValue;
                            dtTemp.SeriesVariable = arrRowData[i].DACruxTable[j].SeriesVariable;
                            dtTemp.SeriesVariableValue = arrRowData[i].DACruxTable[j].SeriesVariableValue;

                            if (m_Input.arrByVariables == null)
                            {
                                dtTemp.DataTable.Columns.Add(arrRowData[i].DACruxTable[j].Variable, arrRowData[i].DACruxTable[j].DataTable.Columns[0].DataType);
                            }
                            else
                            {
                                if (m_Input.arrByVariables.Length == 0)
                                {
                                    dtTemp.DataTable.Columns.Add(arrRowData[i].DACruxTable[j].Variable, arrRowData[i].DACruxTable[j].DataTable.Columns[0].DataType);
                                }
                                else if (m_Input.arrByVariables.Length > 0)
                                {
                                    dtTemp.DataTable.Columns.Add(string.Join(", ", arrRowData[i].DACruxTable[j].SeriesVariableValue), arrRowData[i].DACruxTable[j].DataTable.Columns[0].DataType);
                                }
                            }
                            iColCnt++;

                            //if (m_Input.arrByVariables == null)
                            //{
                            //    dtTemp = new DataTable(strVar);  // 분석컬럼명만..
                            //}
                            //else
                            //{
                            //    if (m_Input.arrByVariables.Length > 1)// Obj Var와 Series Var모두 존재할때
                            //    {
                            //        dtTemp = new DataTable(SubTitle(arrRowData[i].DataSetName + "\r\n" + arrRowData[i].Tables[j].TableName, DataTableUtil.SEPARATOR) + DataTableUtil.SEPARATOR + arrRowData[i].Tables[j].TableName);
                            //    }
                            //    else
                            //    {
                            //        dtTemp = new DataTable(arrRowData[i].Tables[j].TableName);
                            //    }
                            //}
                            #endregion
                        }                        
                    }
                    // DataTable에 Row Add후에 결과 DataSet에 Add
                    dtTemp.DataTable.AcceptChanges();
                    for (int k = 0; k < iMaxRow; k++)  //가장 Rows Count가 많은 DataTable기준
                    {
                        arrTempRow = new object[iColCnt];
                        for (int l = iStartTable; l < iDataTableCnt; l++)
                        {
                            if (k < arrRowData[i].DACruxTable[l].DataTable.Rows.Count)
                            {
                                arrTempRow[l - iStartTable] = arrRowData[i].DACruxTable[l].DataTable.Rows[k][0];
                            }
                        }
                        dtTemp.DataTable.Rows.Add(arrTempRow);
                        arrTempRow = null;
                    }   

                    // DataTable에 Row Add후에 결과 DataSet에 Add
                    dsReturn.DACruxTable.Add(dtTemp);
                    dtTemp = null;
                    #region Return DataTable Name                    
                    #endregion
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
            }
        }
        
        #endregion

        #region [ Analysis ]

        private DACruxTable GetDACruxTable(DataTable dt, int[] arrByVar)
        {
            string[] arrObjVar = null;
            string[] arrSeriesVar = null;
            int iCnt;
            DACruxTable dctReturn = null;
            try
            {
                dctReturn = new DACruxTable();
                dt.AcceptChanges();
                dctReturn.DataTable = dt.Copy();
                if (arrByVar != null)
                {
                    iCnt = arrByVar.Length;
                    if (iCnt == 0)
                    {
                        return dctReturn;
                    }
                    else if (iCnt == 1)
                    {
                        dctReturn.SeriesVariable = new string[] { dt.Columns[arrByVar[0]].ColumnName };
                    }
                    else
                    {
                        arrSeriesVar = new string[1];
                        arrObjVar = new string[iCnt - 1];
                        for (int i = 0; i < iCnt - 1; i++)
                        {
                            arrObjVar[i] = dt.Columns[arrByVar[i]].ColumnName;
                        }
                        arrSeriesVar[0] = dt.Columns[arrByVar[iCnt - 1]].ColumnName;
                        dctReturn.ObjVariable = arrObjVar;
                        dctReturn.SeriesVariable = arrSeriesVar;
                        return dctReturn;
                    }
                }
                else
                {
                    return dctReturn;
                }
                return dctReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DACruxTable GetDACruxTable(DataTable dt, int[] arrByVar, int iVar)
        {
            string[] arrObjVar = null;
            string[] arrSeriesVar = null;
            int iCnt;
            DACruxTable dctReturn = null;
            try
            {
                dctReturn = new DACruxTable();
                dt.AcceptChanges();
                dctReturn.DataTable = dt.Copy();
                dctReturn.Variable = dt.Columns[iVar].ColumnName;
                if (arrByVar != null)
                {
                    iCnt = arrByVar.Length;
                    if (iCnt == 0)
                    {
                        return dctReturn;
                    }
                    else if (iCnt == 1)
                    {
                        dctReturn.SeriesVariable = new string[] { dt.Columns[arrByVar[0]].ColumnName };
                        return dctReturn;
                    }
                    else
                    {
                        arrSeriesVar = new string[1];
                        arrObjVar = new string[iCnt - 1];
                        for (int i = 0; i < iCnt-1; i++)
                        {
                            arrObjVar[i] = dt.Columns[arrByVar[i]].ColumnName;
                        }
                        arrSeriesVar[0] = dt.Columns[arrByVar[iCnt-1]].ColumnName;
                        dctReturn.ObjVariable = arrObjVar;
                        dctReturn.SeriesVariable = arrSeriesVar;
                        return dctReturn;
                    }
                }
                else
                {
                    return dctReturn;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DACruxSet[] GetSource()
        {
            DACruxSet[] arrReturn = null;
            int iByVarCnt;
            int[] arrObjByVar = null;
            try
            {
                if (m_Input.arrVariables == null)
                    throw new ArgumentException(ErrorNullParameter("Analysis Variables"));
                if(m_Input.DataSource == null)
                    throw new ArgumentException(ErrorNullParameter("DataSource is null"));

                if (m_Input.arrByVariables == null)
                {
                    arrReturn = Grouping(GetDACruxTable(m_Input.DataSource,m_Input.arrByVariables), m_Input.arrVariables, null, null);
                }
                else
                {
                    iByVarCnt = m_Input.arrByVariables.Length;
                    if (iByVarCnt == 0)
                    {
                        arrReturn = Grouping(GetDACruxTable(m_Input.DataSource, m_Input.arrByVariables), m_Input.arrVariables, null, null);
                    }
                    else if (iByVarCnt == 1)  //하나일 경우 SeriesByVar로...
                    {
                        arrReturn = Grouping(GetDACruxTable(m_Input.DataSource, m_Input.arrByVariables), m_Input.arrVariables, null, m_Input.arrByVariables);
                    }
                    else  // 둘이상일 경우 마지막 하나만 SeriesByVar로, 나머지는 ObjByVar로....
                    {
                        arrObjByVar = new int[iByVarCnt - 1];
                        for (int i = 0; i < iByVarCnt - 1; i++)
                            arrObjByVar[i] = m_Input.arrByVariables[i];


                        arrReturn = Grouping(GetDACruxTable(m_Input.DataSource, m_Input.arrByVariables), m_Input.arrVariables, arrObjByVar, new int[] { m_Input.arrByVariables[iByVarCnt - 1] });
                    }
                }
                return arrReturn;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DACruxSet GetAnalysisResult(DACruxSet[] Source)
        {
            DACruxSet dcsReturn = null;
            DACruxTable dctResult = null;
            DACruxTable dctUnitResult = null;
            int iDataSetCnt;
            int iDataTableCnt;
            int iUnitStartRows;
            string strObjByVar = string.Empty;  // Object구분 변수 = Source배열 안의 하나의 DataSet의 NameSpace
            string strVar = string.Empty;  // 분석 변수 = Source배열 안의 하나의 DataSet안의 하나의 DataTable의 NameSpace
            string strSeriesByVar = string.Empty; // Series구분 변수 = Source배열 안의 하나의 DataSet안의 하나의 DataTable의 TableName
            string strObjByVarValue = string.Empty; // Object구분 변수의 값 = Source배열 안의 하나의 DataSet의 DataSetName
            string strSeriesByVarValue = string.Empty; // Series구분 변수의 값 = Source배열 안의 하나의 DataSet안의 하나의 DataTable안의 하나의 Column Name
            int iCumulativeSum;
            int iTotal;
            try
            {
                iDataSetCnt = Source.Length;
                dcsReturn = new DACruxSet();
                for (int i = 0; i < iDataSetCnt; i++)
                {
                    iDataTableCnt = Source[i].DACruxTable.Count;
                    iTotal = 0;
                    iUnitStartRows = 0;
                    for (int j = 0; j < iDataTableCnt; j++)
                    {
                        dctUnitResult = GetUnitAnalysis(Source[i].DACruxTable[j]);
                        if (j == 0)
                        {
                            dctResult = dctUnitResult.Clone();
                            
                            strVar = dctUnitResult.DataTable.Rows[0][0].ToString();
                            dctResult.ObjVariable = Source[i].DACruxTable[0].ObjVariable;
                            dctResult.ObjVariableValue = Source[i].DACruxTable[0].ObjVariableValue;
                            dctResult.SeriesVariable = Source[i].DACruxTable[0].SeriesVariable;
                            dctResult.SeriesVariableValue = Source[i].DACruxTable[0].SeriesVariableValue;
                        }
                        dctResult.DataTable.Rows.Add(dctUnitResult.DataTable.Rows[0].ItemArray);
                        if (strVar != dctResult.DataTable.Rows[j][0].ToString())
                        {
                            iCumulativeSum = 0;
                            for (int k = iUnitStartRows; k < j; k++)
                            {
                                iCumulativeSum += Convert.ToInt32(dctResult.DataTable.Rows[k][m_Nnonmissing]);
                                if (m_Input.IsCumulativeN)
                                    dctResult.DataTable.Rows[k][m_CumulativeN] = iCumulativeSum;
                                if (m_Input.IsPercent)
                                    dctResult.DataTable.Rows[k][m_Percent] = Math.Round(Convert.ToDouble(dctResult.DataTable.Rows[k][m_Nnonmissing]) / (double)iTotal * 100, m_Input.DecimalOuter);
                                if (m_Input.IsCumulativePercent)
                                    dctResult.DataTable.Rows[k][m_CumulativePercent] = Math.Round((double)iCumulativeSum / (double)iTotal * 100, m_Input.DecimalOuter);
                            }
                            strVar = dctResult.DataTable.Rows[j][0].ToString();
                            iTotal = 0;
                            iUnitStartRows = j;
                        }

                        iTotal += Convert.ToInt32(dctResult.DataTable.Rows[j][m_Ntotal]);

                    }
                    iCumulativeSum = 0;
                    for (int k = iUnitStartRows; k < iDataTableCnt; k++)
                    {
                        iCumulativeSum += Convert.ToInt32(dctResult.DataTable.Rows[k][m_Nnonmissing]);
                        if (m_Input.IsCumulativeN)
                            dctResult.DataTable.Rows[k][m_CumulativeN] = iCumulativeSum;
                        if (m_Input.IsPercent)
                            dctResult.DataTable.Rows[k][m_Percent] = Math.Round(Convert.ToDouble(dctResult.DataTable.Rows[k][m_Nnonmissing]) / (double)iTotal * 100, m_Input.DecimalOuter);
                        if (m_Input.IsCumulativePercent)
                            dctResult.DataTable.Rows[k][m_CumulativePercent] = Math.Round((double)iCumulativeSum / (double)iTotal * 100, m_Input.DecimalOuter);
                    }
                    if (!m_Input.IsNnonmissing)
                        dctResult.DataTable.Columns.Remove(m_Nnonmissing);
                    if (!m_Input.IsNtotal)
                        dctResult.DataTable.Columns.Remove(m_Ntotal);
                    dcsReturn.DACruxTable.Add(dctResult);
                    //dcsReturn.DACruxTable.Add(dctResult.CopyDataTable());  // 에러나면 확인
                    dctResult = null;
                }

                // Cumulative Count, Percent, Cumulative Percent


                return dcsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        private DACruxTable GetUnitAnalysis(DACruxTable Source)
        {
            DACruxTable dctReturn = null;
            DACrux.BStats.Core.DescriptiveStatistics oStat = null;
            ArrayList arrRow = null;
            try
            {
                oStat = new DACrux.BStats.Core.DescriptiveStatistics(Source.CopyDataTable(), 0, m_SelectedStatistics);
                dctReturn = new DACruxTable();
                dctReturn.ObjVariable = Source.ObjVariable;
                dctReturn.ObjVariableValue = Source.ObjVariableValue;
                dctReturn.SeriesVariable = Source.SeriesVariable;
                dctReturn.SeriesVariableValue = Source.SeriesVariableValue;
                dctReturn.Variable = Source.Variable;
                arrRow = new ArrayList();
                dctReturn.DataTable = new DataTable();
                dctReturn.DataTable.Columns.Add(m_Variable, typeof(string));
                arrRow.Add(Source.Variable);  // 분석 변수 명
                if (Source.SeriesVariable != null)
                {
                    if (Source.SeriesVariable.Length != 0)
                    {
                        dctReturn.DataTable.Columns.Add(string.Join(",", Source.SeriesVariable), typeof(string));  // Series구분 변수명
                        arrRow.Add(string.Join(",", Source.SeriesVariableValue)); // Series구분 변수가 가지는 값
                    }
                }

                #region DataTable Form

                //누적합을 위해서 수정
                //if (m_SelectedStatistics.IsNtotal)
                //{
                dctReturn.DataTable.Columns.Add(m_Ntotal, typeof(int));
                    arrRow.Add(oStat.Result.Ntotal);
                //}
                //if (m_SelectedStatistics.IsNnonmissing)
                //{
                    dctReturn.DataTable.Columns.Add(m_Nnonmissing, typeof(int));
                    arrRow.Add(oStat.Result.Nnonmissing);
                //}
                if (m_SelectedStatistics.IsNmissing)
                {
                    dctReturn.DataTable.Columns.Add(m_Nmissing, typeof(int));
                    arrRow.Add(oStat.Result.Nmissing);
                }
                if (m_SelectedStatistics.IsCumulativeN)
                {
                    dctReturn.DataTable.Columns.Add(m_CumulativeN, typeof(int));
                    arrRow.Add(oStat.Result.CumulativeN);
                }
                if (m_SelectedStatistics.IsPercent)
                {
                    dctReturn.DataTable.Columns.Add(m_Percent, typeof(double));
                    arrRow.Add(oStat.Result.Percent);
                }
                if (m_SelectedStatistics.IsCumulativePercent)
                {
                    dctReturn.DataTable.Columns.Add(m_CumulativePercent, typeof(double));
                    arrRow.Add(oStat.Result.CumulativePercent);
                }
                if (m_SelectedStatistics.IsMean)
                {
                    dctReturn.DataTable.Columns.Add(m_Mean, typeof(double));
                    arrRow.Add(oStat.Result.Mean);
                }
                if (m_SelectedStatistics.IsMeanSE)
                {
                    dctReturn.DataTable.Columns.Add(m_MeanSE, typeof(double));
                    arrRow.Add(oStat.Result.MeanSe);
                }
                if (m_SelectedStatistics.IsTrimmedMean)
                {
                    dctReturn.DataTable.Columns.Add(m_TrimmedMean, typeof(double));
                    arrRow.Add(oStat.Result.TrimmedMean);
                }
                if (m_SelectedStatistics.IsStandarddeviation)
                {
                    dctReturn.DataTable.Columns.Add(m_Standarddeviation, typeof(double));
                    arrRow.Add(oStat.Result.Standarddeviation);
                }
                if (m_SelectedStatistics.IsVariance)
                {
                    dctReturn.DataTable.Columns.Add(m_Variance, typeof(double));
                    arrRow.Add(oStat.Result.Variance);
                }
                if (m_SelectedStatistics.IsCoefficientofvariation)
                {
                    dctReturn.DataTable.Columns.Add(m_Coefficientofvariation, typeof(double));
                    arrRow.Add(oStat.Result.Coefficientofvariation);
                }
                if (m_SelectedStatistics.IsSum)
                {
                    dctReturn.DataTable.Columns.Add(m_Sum, typeof(double));
                    arrRow.Add(oStat.Result.Sum);
                }
                if (m_SelectedStatistics.IsSumofSquares)
                {
                    dctReturn.DataTable.Columns.Add(m_SumofSquares, typeof(double));
                    arrRow.Add(oStat.Result.SumofSquares);
                }
                if (m_SelectedStatistics.IsMin)
                {
                    dctReturn.DataTable.Columns.Add(m_Min, typeof(double));
                    arrRow.Add(oStat.Result.Min);
                }
                if (m_SelectedStatistics.IsQ1)
                {
                    dctReturn.DataTable.Columns.Add(m_Q1, typeof(double));
                    arrRow.Add(oStat.Result.Q1);
                }
                if (m_SelectedStatistics.IsMedian)
                {
                    dctReturn.DataTable.Columns.Add(m_Median, typeof(double)); 
                    arrRow.Add(oStat.Result.Median);
                }
                if (m_SelectedStatistics.IsQ3)
                {
                    dctReturn.DataTable.Columns.Add(m_Q3, typeof(double));
                    arrRow.Add(oStat.Result.Q3);
                }
                if (m_SelectedStatistics.IsMax)
                {
                    dctReturn.DataTable.Columns.Add(m_Max, typeof(double));
                    arrRow.Add(oStat.Result.Max);
                }
                if (m_SelectedStatistics.IsRange)
                {
                    dctReturn.DataTable.Columns.Add(m_Range, typeof(double));
                    arrRow.Add(oStat.Result.Range);
                }
                if (m_SelectedStatistics.IsInterquartileRange)
                {
                    dctReturn.DataTable.Columns.Add(m_InterquartileRange, typeof(double));
                    arrRow.Add(oStat.Result.InterquartileRange);
                }
                if (m_SelectedStatistics.IsMode)
                {
                    dctReturn.DataTable.Columns.Add(m_Mode, typeof(double));
                    dctReturn.DataTable.Columns.Add(m_ModeN, typeof(int));
                    arrRow.Add(oStat.Result.Mode);
                    arrRow.Add(oStat.Result.ModeN);
                }
                if (m_SelectedStatistics.IsSkewness)
                {
                    dctReturn.DataTable.Columns.Add(m_Skewness, typeof(double));
                    arrRow.Add(oStat.Result.Skewness);
                }
                if (m_SelectedStatistics.IsKurtosis)
                {
                    dctReturn.DataTable.Columns.Add(m_Kurtosis, typeof(double));
                    arrRow.Add(oStat.Result.Kurtosis);
                }
                if (m_SelectedStatistics.IsMSSD)
                {
                    dctReturn.DataTable.Columns.Add(m_MSSD, typeof(double));
                    arrRow.Add(oStat.Result.MSSD);
                }
                #endregion

                //dctReturn.DataTable.AcceptChanges();
                dctReturn.DataTable.Rows.Add((object[])arrRow.ToArray(typeof(object)));
                //dctReturn.DataTable.AcceptChanges();
                return dctReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region [ Conversion ]
        ///// <summary>
        ///// Source DataTable에서 하나의 Column을 특정 Columns을 기준으로 Group으로 나누어서 여러 DataTable로 구성된 DataSet을 돌려준다.
        ///// </summary>
        ///// <param name="Source">Source DataTable</param>
        ///// <param name="SelectedColumn">Source DataTable내에서 최종적으로 선택할 Column Index</param>
        ///// <param name="GroupColumns">return될 DataTable에서 Column을 구분지을 Source DataTable에서의 Column Index</param>
        ///// <param name="Filter">filterExpression(행을 필터링하기 위한 조건)</param>
        ///// <returns>DataSet내의 Table에서 Column Name은 Group화하는 Columns 값 조합(없으면 SelectedColumn의 ColumnName)의 하나이며, DataTable TableName은 SelectedColumn의 ColumnName + m_Separator + Group화하는 Column Name + "=" + Group화하는 Column Value 의 조합, DataTable NameSpace는 SelectedColumn의 ColumnName</returns>


        private  DACruxSet Conversion(DACruxTable Source, int SelectedColumn, int ValidRowIndex, int[] GroupColumns, string Filter)
        {
            DACruxTable dctNewSource = null;
            DataTable dtDistinct = null;
            DACruxTable dctTemp = null;
            DACruxSet dcsReturn = null;
            int iDistinctCnt;
            int iDistinctColCnt;
            string[] arrDistinctColNames;
            string strVar = string.Empty;  // Var Column Name
            int iRowCnt;
            try
            {
                dctNewSource = DataTableUtil.SelectWhere(Source, Filter);
                if (GroupColumns == null)
                    GroupColumns = new int[0];
                iDistinctColCnt = GroupColumns.Length;
                if (iDistinctColCnt > 0)  // Group Columns(Series Var)가 있는 경우
                {
                    dtDistinct = DataTableUtil.SelectDistinctNoSort("DISTINCT", dctNewSource.CopyDataTable(), GroupColumns, true);
                    iDistinctCnt = dtDistinct.Rows.Count;
                    dcsReturn = new DACruxSet();

                    arrDistinctColNames = DataTableUtil.GetColumnName(dtDistinct);

                    strVar = dctNewSource.DataTable.Columns[SelectedColumn].ColumnName;


                    for (int i = 0; i < iDistinctCnt; i++)
                    {
                        //dctTemp = SelectWhere(strNameSpace + m_Separator + MakeString(arrDistinctColNames, dtDistinct.Rows[i].ItemArray, m_Separator), dtNewSource, SelectedColumn, MakeString(arrDistinctColNames, dtDistinct.Rows[i].ItemArray, " and "));
                        dctTemp = DataTableUtil.SelectWhere(dctNewSource, SelectedColumn, DataTableUtil.MakeString(arrDistinctColNames, dtDistinct.Rows[i].ItemArray, " and "));
                        dctTemp.Variable = strVar;
                        dctTemp.SeriesVariable = arrDistinctColNames;
                        dctTemp.SeriesVariableValue = DataTableUtil.GetStringArray(dtDistinct.Rows[i].ItemArray);
                        dctTemp.ObjVariable = dctNewSource.ObjVariable;
                        dctTemp.ObjVariableValue = dctNewSource.ObjVariableValue;
                        dctTemp.DataTable.Columns[0].ColumnName = DataTableUtil.MakeColumnName(dtDistinct.Rows[i].ItemArray, "--");
                        //여기서 dctTemp의 DataTable작업...null Trim
                        iRowCnt = dctTemp.DataTable.Rows.Count;
                        for (int j = iRowCnt - 1; j > -1; j--)
                        {
                            if (j > ValidRowIndex)
                            {
                                if (!dctTemp.DataTable.Rows[j][0].Equals(DBNull.Value))
                                    break;
                                dctTemp.DataTable.Rows.RemoveAt(j);
                            }
                        }
                        //여기서 dctTemp의 DataTable작업...null Trim
                        dcsReturn.DACruxTable.Add(dctTemp.Copy());
                        //dcsReturn.DACruxTable.Add(dctTemp);   // 확인할것
                        dctTemp = null;
                    }

                }
                else  // GroupColumns이 없을 경우
                {
                    dcsReturn = new DACruxSet();
                    strVar = dctNewSource.DataTable.Columns[SelectedColumn].ColumnName;
                    dctTemp = DataTableUtil.SelectWhere(dctNewSource, SelectedColumn, string.Empty);
                    dctTemp.Variable = strVar;
                    dctTemp.ObjVariable = dctNewSource.ObjVariable;
                    dctTemp.ObjVariableValue = dctNewSource.ObjVariableValue;
                    dctTemp.SeriesVariable = null;
                    dctTemp.SeriesVariableValue = null;
                    dcsReturn.ObjVariable = dctNewSource.ObjVariable;
                    dcsReturn.ObjVariableValue = dctNewSource.ObjVariableValue;
                    dcsReturn.SeriesVariable = null;
                    dcsReturn.SeriesVariableValue = null;
                    //여기서 dctTemp의 DataTable작업...null Trim
                    iRowCnt = dctTemp.DataTable.Rows.Count;
                    for (int j = iRowCnt - 1; j > -1; j--)
                    {
                        if (j > ValidRowIndex)
                        {
                            if (!dctTemp.DataTable.Rows[j][0].Equals(DBNull.Value))
                                break;
                            dctTemp.DataTable.Rows.RemoveAt(j);
                        }
                    }
                    //여기서 dctTemp의 DataTable작업...null Trim
                    dcsReturn.DACruxTable.Add(dctTemp);
                }
                return dcsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dcsReturn = null;
                dtDistinct = null;
                dctNewSource = null;
                dctTemp = null;
                arrDistinctColNames = null;
            }
        }
        #endregion

        #region [ Grouping ]
        ///// <summary>
        ///// Source DataTable을 받아서 arrObjByVar기준으로 나누어진 나눠어진 DataSet(arrVar별로 나누어진 DataTable(arrSeriesByVar기준으로 나누어진 Columns을 가진 DataTable)로 이루어진 DataSet)의 배열로 돌려준다
        ///// </summary>
        ///// <param name="Source">Source DataTable</param>
        ///// <param name="arrVar">return값의 하나의 DataSet내에서 DataTable을 나누는 기준이 되는 Source DataTable내의 Column Index List</param>
        ///// <param name="arrObjByVar">return값에서 DataSet을 나누는 기준이 되는 Source DataTable내의 Column Index List</param>
        ///// <param name="arrSeriesByVar">return값의 하나의 DataSet내의 하나의 DataTable의 Column을 나누는 기준이 되는 Column Index List</param>
        ///// <returns>DataSet으로 이루어진 배열</returns>

        private DACruxSet[] Grouping(DACruxTable Source, int[] arrVar, int[] arrObjByVar, int[] arrSeriesByVar)
        {
            DACruxSet[] arrDCS = null;
            DACruxSet dcsReturn = null;
            DACruxSet dcsUnit = null;
            DACruxTable dctTemp = null;  // 하나의 DataSet을 만들기 위한 내부 Source DataTable
            int iVarCnt; //arrVar의 길이(분석 변수의 개수)
            int iObjByVarCnt; //arrObjByVar의 길이(Object 분류 변수의 개수)
            int iSeriesByVarCnt; //arrSeriesByVar의 길이(Series 분류 변수의 개수)
            int iDistinctByObjCnt; //생성되는 Obj의 개수
            string[] arrDistinctByObjColNames; //
            string strDsFilter = string.Empty; // dtSource에서의 첫번째 조건(Object분리)
            DataTable dtDistinctByObj = null; // 차트나 분석의 출력 단위를 나누는 컬럼들의 유일한 조합의 모임
            int iColCntByObj;
            try
            {
                #region 분석변수에 대한 확인
                if (arrVar == null)
                    throw new ArgumentException(ErrorNullParameter("arrVar"));
                iVarCnt = arrVar.Length;
                if (iVarCnt == 0)
                    throw new ArgumentException(ErrorNullParameter("arrVar"));
                #endregion

                #region Object 분류 변수에 대한 확인
                if (arrObjByVar == null)
                    arrObjByVar = new int[0];
                iObjByVarCnt = arrObjByVar.Length;
                #endregion

                #region Series 분류 변수에 대한 확인
                if (arrSeriesByVar == null)
                    arrSeriesByVar = new int[0];
                iSeriesByVarCnt = arrSeriesByVar.Length;
                #endregion

                if (iObjByVarCnt > 0)  // Object 분류변수가 있는 경우
                {
                    dtDistinctByObj = DataTableUtil.SelectDistinctNoSort("Distinct", Source.DataTable, arrObjByVar, true);
                    iDistinctByObjCnt = dtDistinctByObj.Rows.Count;  //최종 결과물의 DACruxSet 개수
                    arrDCS = new DACruxSet[iDistinctByObjCnt];

                    iColCntByObj = dtDistinctByObj.Columns.Count;

                    #region 모든 Object 분류변수명 입력

                    arrDistinctByObjColNames = DataTableUtil.GetColumnName(dtDistinctByObj);  //어떤변수가 Obj Var인지.

                    #endregion

                    for (int i = 0; i < iDistinctByObjCnt; i++)
                    {
                        strDsFilter = DataTableUtil.MakeString(arrDistinctByObjColNames, dtDistinctByObj.Rows[i].ItemArray, " and ");  // Source에서 하나의 DataSet을 만들기위한 Source DataTable을 가져올 조건
                        //dcsReturn = new DACruxSet(MakeColumnName(dtDistinctByObj.Rows[i].ItemArray, m_Separator));
                        dcsReturn = new DACruxSet();
                        dcsReturn.ObjVariable = arrDistinctByObjColNames;
                        dctTemp = DataTableUtil.SelectWhere(Source, strDsFilter);
                        for (int j = 0; j < iVarCnt; j++)
                        {
                            dcsReturn.ObjVariableValue = DataTableUtil.GetStringArray(dtDistinctByObj.Rows[i].ItemArray);
                            dcsReturn.SeriesVariable = DataTableUtil.GetColumnName(Source.DataTable, arrSeriesByVar);
                            dctTemp.ObjVariableValue = dcsReturn.ObjVariableValue;
                            dcsUnit = Conversion(dctTemp, arrVar[j], m_Input.arrValidRowIndices[j], arrSeriesByVar, string.Empty);
                            for (int k = 0; k < dcsUnit.DACruxTable.Count; k++)
                            {
                                dcsReturn.DACruxTable.Add(dcsUnit.DACruxTable[k].Copy());
                            }
                            //dsReturn.AcceptChanges();
                            dcsUnit = null;
                        }
                        arrDCS[i] = dcsReturn;
                    }
                }
                else  // Object분류 변수가 없는 경우
                {
                    arrDCS = new DACruxSet[1];
                    dcsReturn = new DACruxSet();
                    dcsReturn.ObjVariable = null;
                    dcsReturn.ObjVariableValue = null;

                    for (int i = 0; i < iVarCnt; i++)
                    {
                        dcsUnit = Conversion(Source, arrVar[i], m_Input.arrValidRowIndices[i], arrSeriesByVar, string.Empty);
                        for (int j = 0; j < dcsUnit.DACruxTable.Count; j++)
                        {
                            dcsReturn.DACruxTable.Add(dcsUnit.DACruxTable[j].Copy());
                        }
                        //dsReturn.AcceptChanges();
                        dcsUnit = null;
                    }

                    arrDCS[0] = dcsReturn;
                }

                return arrDCS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrDistinctByObjColNames = null;
                dcsReturn = null;
                if (dtDistinctByObj != null)
                    dtDistinctByObj.Dispose();
                dtDistinctByObj = null;
                dcsUnit = null;
                dctTemp = null;
            }
        }

        #endregion


        #endregion

        private DataTable GetNonMissingGroup(DataTable dtSource, int[] arrIndex)
        {
            int iColCnt;
            int iRowCnt;
            DataRow[] arrTemp = null;
            DataTable dtReturn = null;
            try
            {
                if (dtSource == null)
                    return null;
                dtReturn = dtSource.Clone();
                if (arrIndex == null)
                    return dtReturn;
                iColCnt = arrIndex.Length;
                iRowCnt = dtSource.Rows.Count;
                for (int i = 0; i < iColCnt; i++)
                    dtReturn.Columns[arrIndex[i]].DataType = typeof(string);
                dtReturn.AcceptChanges();
                for (int i = 0; i < iRowCnt; i++)
                    dtReturn.Rows.Add(dtSource.Rows[i].ItemArray);
                
                for (int i = 0; i < iColCnt; i++)
                {
                    arrTemp = dtReturn.Select(string.Format("TRIM([" + dtSource.Columns[arrIndex[i]].ColumnName + "])" + " = '' OR [" + dtSource.Columns[arrIndex[i]].ColumnName + "] IS NULL"));
                    for (int j = 0; j < arrTemp.Length; j++)
                    {
                        arrTemp[j][arrIndex[i]] = m_Missing;
                    }
                    dtReturn.AcceptChanges();
                    arrTemp = null;
                }
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        #endregion
    }
}
