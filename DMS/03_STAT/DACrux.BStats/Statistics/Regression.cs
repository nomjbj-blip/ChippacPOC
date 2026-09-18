using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using DACrux.BStats.StatisticsInput;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.Statistics
{
    public class Regression
    {
        #region " MEMBER FIELD "
        /// <summary>
        /// Input Option
        /// </summary>
        private inputRegression m_Input = null;


        #region Message
        //private string m_ErrorArgumentEqual = "SingleDataColumnIndex equals SubgroupColumnIndex.";
        //private string m_ErrorArgumentNoValueColumn = "Please select column for analysis.";
        //private string m_ErrorArgumentContantForSubgroupSize = "Sub group size must be positive natural number.";
        //private string m_ErrorSubgroupSizeContant = "Data rows Count must be greater than subgroup size contant.";
        #endregion

        private DACrux.BStats.Core.SelectedRegression m_SelectedRegression;
        private DACrux.BStats.Core.ResultRegression m_ResultRegression;
        private DataTable m_DataSource;
        private GraphInformation[] m_Graph = null;
        private string m_Response;  //종속변수 이름
        private string[] m_Predictors;//독립변수 이름

        private int m_GraphWidth = 370;
        private int m_GraphHeight = 370;
        #endregion

        #region " CREATOR "

        public Regression(inputRegression oInput)
        {
            m_Input = oInput;
            m_SelectedRegression = new DACrux.BStats.Core.SelectedRegression();
            m_SelectedRegression.arrPredictors = oInput.arrPredictors;
            m_SelectedRegression.iResponse = oInput.iResponse;
            m_SelectedRegression.IsIntercept = oInput.IsIntercept;
            m_SelectedRegression.Methods.arrIncludePredictors = oInput.arrIncludePredictors;
            m_SelectedRegression.Methods.arrPredictorsInitialModel = oInput.arrPredictorsInitialModel;
            m_SelectedRegression.Methods.backwardDrop = oInput.backwardDrop;
            m_SelectedRegression.Methods.forwardAdd = oInput.forwardAdd;
            m_SelectedRegression.Methods.IsUseAlpha = oInput.IsUseAlpha;
            m_SelectedRegression.Methods.ReAnalysisType = oInput.ReAnalysisType;
            m_SelectedRegression.Methods.stepwiseAdd = oInput.stepwiseAdd;
            m_SelectedRegression.Methods.stepwiseDrop = oInput.stepwiseDrop;
            m_SelectedRegression.Options.arrGraphVariablesVsResiduals = oInput.arrGraphVariablesVsResiduals;
            m_SelectedRegression.Options.IsGraphHistogramResiduals = oInput.IsGraphHistogramResiduals;
            m_SelectedRegression.Options.IsGraphProbabilityPlotResiduals = oInput.IsGraphProbabilityPlotResiduals;
            m_SelectedRegression.Options.IsGraphResidualsVsFittedValues = oInput.IsGraphResidualsVsFittedValues;
            m_SelectedRegression.Options.IsGraphResidualsVSOrder = oInput.IsGraphResidualsVSOrder;
            m_SelectedRegression.Options.IsTableAnova = oInput.IsTableAnova;
            m_SelectedRegression.Options.IsTableCoefficients = oInput.IsTableCoefficients;
            m_SelectedRegression.Options.IsTableR_Square = oInput.IsTableR_Square;
            m_SelectedRegression.Options.IsTableResiduals = oInput.IsTableResiduals;
            m_DataSource = oInput.DataSource;
            m_Response = m_DataSource.Columns[m_SelectedRegression.iResponse].ColumnName;
            m_Predictors = new string[m_SelectedRegression.arrPredictors.Length];
            for (int i = 0; i < m_SelectedRegression.arrPredictors.Length; i++)
                m_Predictors[i] = m_DataSource.Columns[m_SelectedRegression.arrPredictors[i]].ColumnName;
            DACrux.BStats.Core.Regression oRegression = new DACrux.BStats.Core.Regression(m_DataSource, m_SelectedRegression);
            m_ResultRegression = oRegression.Result;

            m_Graph = GetGraphInfo(m_ResultRegression.TableforGraphs);
            MakeHtml(m_ResultRegression);            
            m_Input.GraphInformations = m_Graph;
        }

        #endregion

        #region " METHOD "

        #region [ FileName ]
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

        #region [ Html ]

        private void MakeHtml(DACrux.BStats.Core.ResultRegression oResult)
        {
            HtmlConverter oHtml = null;
            try
            {
                oHtml = new HtmlConverter(DACrux.ProjectManager.UI.Common.TempPath + m_Input.ResultFilePath);
                oHtml.MainTitle(m_Input.Project, m_Input.WorkSheet, m_Input.User, m_Input.Title);
                oHtml.BodyRegression(oResult, m_Input.ReAnalysisType, m_Graph, m_Response, m_Predictors);
                oHtml.SaveHtml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ Graph ]

        private GraphInformation[] GetGraphInfo(DataSet dsSource)
        {
            GraphInformation[] arrReturn = null;
            int iCnt;
            #region Core의 Regression클래스 내의 Graph Title(클래스변수) 참고..
            const string strHistogram = "Histogram of residuals";
            const string strProbability = "Probability Plot of the Residuals";
            const string strFit = "Residuals Versus the Fitted Values";
            const string strOrder = "Residuals Versus Order";
            #endregion
            try
            {
                if (dsSource == null || dsSource.Tables.Count < 1)
                {
                    return null;
                }
                iCnt = dsSource.Tables.Count;
                arrReturn = new GraphInformation[iCnt];
                for (int i = 0; i < iCnt; i++)
                {

                    switch (dsSource.Tables[i].TableName)
                    {
                        case(strHistogram):
                            arrReturn[i] = new GraphInformation(GraphType.Histogram);
                            arrReturn[i].Name = "Histogram(" + m_Input.Title + ")" + " : " + strHistogram;
                            arrReturn[i].AddAxisY(new GraphInformation.ColumnInfoItem(0, null, dsSource.Tables[i].Columns[0].ColumnName, dsSource.Tables[i].Columns[0].DataType));
                            arrReturn[i].NormalLine = true;
                            arrReturn[i].DecimalPlaceX = DataTableUtil.GetDecimalPlace(dsSource.Tables[i], 0);
                            arrReturn[i].ForceZero = true;
                            break;
                        case (strProbability):
                            arrReturn[i] = new GraphInformation(GraphType.Scatter);
                            arrReturn[i].Name = "Probability(" + m_Input.Title + ")" + " : " + strProbability;
                            
                            arrReturn[i].AxisX = new GraphInformation.ColumnInfoItem(0, null, dsSource.Tables[i].Columns[0].ColumnName, dsSource.Tables[i].Columns[0].DataType);
                            arrReturn[i].AddAxisY(new GraphInformation.ColumnInfoItem(1, null, dsSource.Tables[i].Columns[1].ColumnName, dsSource.Tables[i].Columns[1].DataType));
                            arrReturn[i].AxisXTitle = dsSource.Tables[i].Columns[0].ColumnName;
                            arrReturn[i].AxisYTitle = dsSource.Tables[i].Columns[1].ColumnName;
                            arrReturn[i].DecimalPlaceX = DataTableUtil.GetDecimalPlace(dsSource.Tables[i], 0);
                            arrReturn[i].DecimalPlace = DataTableUtil.GetDecimalPlace(dsSource.Tables[i], 1);
                            arrReturn[i].PointColor = System.Drawing.Color.Red;
                            arrReturn[i].ForceZero = false;
                            break;
                        case (strFit):
                            arrReturn[i] = new GraphInformation(GraphType.Scatter);
                            arrReturn[i].Name = "Scatter(" + m_Input.Title + ")" + " : " + strFit;
                            arrReturn[i].AddAxisY(new GraphInformation.ColumnInfoItem(0, null, dsSource.Tables[i].Columns[0].ColumnName, dsSource.Tables[i].Columns[0].DataType));
                            arrReturn[i].AxisX = new GraphInformation.ColumnInfoItem(1, null, dsSource.Tables[i].Columns[1].ColumnName, dsSource.Tables[i].Columns[1].DataType);
                            arrReturn[i].AxisXTitle = dsSource.Tables[i].Columns[1].ColumnName;
                            arrReturn[i].AxisYTitle = dsSource.Tables[i].Columns[0].ColumnName;
                            arrReturn[i].DecimalPlaceX = DataTableUtil.GetDecimalPlace(dsSource.Tables[i], 1);
                            arrReturn[i].DecimalPlace = DataTableUtil.GetDecimalPlace(dsSource.Tables[i], 0);
                            arrReturn[i].GridLine = false;
                            arrReturn[i].PointColor = System.Drawing.Color.Red;
                            arrReturn[i].ForceZero = false;
                            break;
                        case (strOrder):
                            //arrReturn[i] = new GraphInformation(GraphType.Scatter);
                            arrReturn[i] = new GraphInformation(GraphType.Line);
                            arrReturn[i].Name = "Line(" + m_Input.Title + ")" + " : " + strOrder;
                            arrReturn[i].AddAxisY(new GraphInformation.ColumnInfoItem(0, null, dsSource.Tables[i].Columns[0].ColumnName, dsSource.Tables[i].Columns[0].DataType));
                            arrReturn[i].AxisX = new GraphInformation.ColumnInfoItem(1, null, dsSource.Tables[i].Columns[1].ColumnName, dsSource.Tables[i].Columns[1].DataType);
                            arrReturn[i].AxisXTitle = dsSource.Tables[i].Columns[1].ColumnName;
                            //arrReturn[i].AxisYTitle = dsSource.Tables[i].Columns[0].ColumnName;
                            //arrReturn[i].DecimalPlaceX = DataTableUtil.GetDecimalPlace(dsSource.Tables[i], 1);
                            arrReturn[i].DecimalPlace = DataTableUtil.GetDecimalPlace(dsSource.Tables[i]);
                            arrReturn[i].GridLine = false;
                            arrReturn[i].PointColor = System.Drawing.Color.Red;
                            arrReturn[i].ForceZero = false;


                            break;
                        default:  //기타 변수들과의 산점도
                            arrReturn[i] = new GraphInformation(GraphType.Scatter);
                            arrReturn[i].Name = "Scatter(" + m_Input.Title + ")" + " : " + dsSource.Tables[i].TableName;
                            arrReturn[i].AddAxisY(new GraphInformation.ColumnInfoItem(0, null, dsSource.Tables[i].Columns[0].ColumnName, dsSource.Tables[i].Columns[0].DataType));
                            arrReturn[i].AxisX = new GraphInformation.ColumnInfoItem(1, null, dsSource.Tables[i].Columns[1].ColumnName, dsSource.Tables[i].Columns[1].DataType);
                            arrReturn[i].AxisXTitle = dsSource.Tables[i].Columns[1].ColumnName;
                            arrReturn[i].AxisYTitle = dsSource.Tables[i].Columns[0].ColumnName;
                            arrReturn[i].DecimalPlaceX = DataTableUtil.GetDecimalPlace(dsSource.Tables[i], 1);
                            arrReturn[i].DecimalPlace = DataTableUtil.GetDecimalPlace(dsSource.Tables[i], 0);
                            arrReturn[i].GridLine = false;
                            arrReturn[i].PointColor = System.Drawing.Color.Red;
                            arrReturn[i].ForceZero = false;
                            break;

                    }
                    arrReturn[i].AxisXForceZero = false;
                    arrReturn[i].DataSource = dsSource.Tables[i];
                    arrReturn[i].Title = dsSource.Tables[i].TableName;
                    arrReturn[i].Frequence = true;
                    
                    arrReturn[i].IsImageWithTitles = true;
                    arrReturn[i].ImageSize = new System.Drawing.Size(m_GraphWidth, m_GraphHeight);
                    arrReturn[i].ImagePath = GetFileName(m_Input.ResultFilePath) + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + i.ToString()  + ".jpg";
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
