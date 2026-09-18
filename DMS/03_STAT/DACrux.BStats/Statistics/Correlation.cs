using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using DACrux.BStats.StatisticsInput;
using DACrux.ProjectManager.UI;
using System.Runtime.Serialization.Formatters.Binary;

namespace DACrux.BStats.Statistics
{
    /// <summary>
    /// For DACrux Solution : 여러컬럼의 조합으로 상관분석을 실행하고 차트정보생성과, html file생성까지 하는 부분
    /// </summary>
    public class Correlation
    {
        #region " MEMBER FIELD "

        /// <summary>
        /// Input Option
        /// </summary>
        private inputCorrelation m_Input = null;


        #region [ LANGUAGE ]
        //string m_PearsonCorrelation = "Pearson correlation";
        //string m_Pvalue = "P-Value";
        //string m_Spearman = "Spearman's Rho";
        //string m_RegressionEquation = "Regression Equation";
        //string m_ScatterPlot = "ScatterPlot";

        #endregion

        #region [ CHART ]
        private int m_CorrelationScatterH = 300;
        private int m_CorrelationScatterW = 300;
        #endregion

        /// <summary>
        /// Result For Html File
        /// </summary>
        //private DACruxTable m_dctResult = null;

        /// <summary>
        /// Result For Html File
        /// </summary>
        private CorrelationResult[,] m_arrResult = null;

        /// <summary>
        /// Raw Data For Correlation
        /// </summary>
        private DACruxTable m_RawData = null;



        private DACrux.BStats.Core.SelectedCorrelation m_SelectedCorrelation = new DACrux.BStats.Core.SelectedCorrelation();  // Core Class에 넘겨주는 옵션값

        #endregion

        #region " CREATOR "
        public Correlation(inputCorrelation oInput)
        {
            m_Input = oInput;
            m_SelectedCorrelation.Reset();
            m_SelectedCorrelation.IsPvalue = m_Input.IsPvalue;
            m_SelectedCorrelation.IsSpearman = m_Input.IsSpearman;
            m_RawData = new DACruxTable();
            m_RawData.DataTable = m_Input.DataSource;
            m_RawData.SeriesVariable = DataTableUtil.GetColumnName(m_RawData.DataTable, m_Input.arrVariables);
            m_arrResult = GetAnalysisResult(m_RawData);
            MakeHtml(m_arrResult, m_RawData.SeriesVariable);
            m_Input.GraphInformations = GetGraphInfo();
        }
        #endregion

        #region " STRUCT "
        public struct CorrelationResult
        {
            public GraphInformation GraphInfo;
            public double PearsonCorrelation;
            public double Pvalue;
            public double SpearmanRho;
            public double SpearmanPvalue;
            public string X;
            public string Y;
        }
        #endregion

        #region " METHOD "

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

        private void MakeHtml(CorrelationResult[,] dctResult, string[] arrColumnNames)
        {
            HtmlConverter oHtml = null;
            try
            {
                oHtml = new HtmlConverter(DACrux.ProjectManager.UI.Common.TempPath + m_Input.ResultFilePath);
                oHtml.MainTitle(m_Input.Project, m_Input.WorkSheet, m_Input.User, m_Input.Title);
                oHtml.SubTitle(string.Join(", ", arrColumnNames));
                oHtml.BodyCorrelation(m_arrResult, arrColumnNames, m_Input.IsPvalue, m_Input.IsSpearman, m_Input.IsScatter);
                oHtml.SaveHtml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private CorrelationResult[,] GetAnalysisResult(DACruxTable RawData)
        {
            CorrelationResult[,] dctReturn = null;
            int iColumnCnt;
            try
            {
                iColumnCnt = m_Input.arrVariables.Length;
                dctReturn = new CorrelationResult[iColumnCnt, iColumnCnt];
                for (int i = 0; i < iColumnCnt; i++)  // Y축(Row)
                {
                    for (int j = 0; j < iColumnCnt; j++)  // X축(Column)
                    {
                        if (i == j)
                            continue;
                        CorrelationResult oCell = GetUnitAnalysis(i, j);
                        dctReturn[i, j] = oCell;
                    }
                }
                return dctReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CorrelationResult GetUnitAnalysis(int iY, int iX)
        {
            try
            {
                CorrelationResult oReturn = new CorrelationResult();
                DACrux.BStats.Core.Correlation oCorrelation = new DACrux.BStats.Core.Correlation(m_Input.DataSource, iX, iY, m_SelectedCorrelation);
                oReturn.PearsonCorrelation = oCorrelation.Result.CoefficientCorrelation;
                if (m_SelectedCorrelation.IsPvalue)
                    oReturn.Pvalue = oCorrelation.Result.Pvalue;
                if (m_SelectedCorrelation.IsSpearman)
                {
                    oReturn.SpearmanRho = oCorrelation.Result.Spearman;
                    if (m_SelectedCorrelation.IsPvalue)
                        oReturn.SpearmanPvalue = oCorrelation.Result.SpearmanPvalue;
                }
                oReturn.X = oCorrelation.Result.Xvar;
                oReturn.Y = oCorrelation.Result.Yvar;
                if (m_Input.IsScatter)
                {
                    oReturn.GraphInfo = new GraphInformation(GraphType.Scatter);
                    oReturn.GraphInfo.AxisXForceZero = false;
                    oReturn.GraphInfo.ForceZero = false;
                    oReturn.GraphInfo.DataSource = m_Input.DataSource;
                    oReturn.GraphInfo.AddAxisY(new GraphInformation.ColumnInfoItem(iY, null, m_Input.DataSource.Columns[iY].ColumnName, m_Input.DataSource.Columns[iY].DataType));
                    oReturn.GraphInfo.AxisX = new GraphInformation.ColumnInfoItem(iX, null, m_Input.DataSource.Columns[iX].ColumnName, m_Input.DataSource.Columns[iX].DataType);
                    oReturn.GraphInfo.DecimalPlace = DataTableUtil.GetDecimalPlace(m_Input.DataSource, iY);
                    oReturn.GraphInfo.DecimalPlaceX = DataTableUtil.GetDecimalPlace(m_Input.DataSource, iX);
                    oReturn.GraphInfo.Name = "ScatterPlot(" + m_Input.Title + ")" + " : " + oReturn.X + ", " + oReturn.Y;
                    oReturn.GraphInfo.Title = oReturn.Y + ", " + oReturn.X;  // 
                    oReturn.GraphInfo.ImagePath = GetFileName(m_Input.ResultFilePath) + "ScatterPlot" + DateTime.Now.ToString("yyyyMMddHHmmss") + oReturn.X + "-" + oReturn.Y + ".jpg";
                    oReturn.GraphInfo.AxisXTitle = oReturn.X;
                    oReturn.GraphInfo.AxisYTitle = oReturn.Y;
                    oReturn.GraphInfo.RegressionEquation = m_Input.IsRegression;
                    oReturn.GraphInfo.ImageSize = new System.Drawing.Size(m_CorrelationScatterW, m_CorrelationScatterH);
                }
                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private GraphInformation[] GetGraphInfo()
        {
            GraphInformation[] arrReturn = null;
            int iCnt;
            int iSelectedColumnCnt;
            int iGraph;
            try
            {
                iSelectedColumnCnt = m_Input.arrVariables.Length;
                iCnt = (iSelectedColumnCnt * iSelectedColumnCnt) - iSelectedColumnCnt;
                arrReturn = new GraphInformation[iCnt];
                iGraph = 0;
                for (int i = 0; i < iSelectedColumnCnt; i++)
                {
                    for (int j = 0; j < iSelectedColumnCnt; j++)
                    {
                        if (i == j)
                            continue;
                        arrReturn[iGraph] = m_arrResult[i,j].GraphInfo;
                        iGraph++;
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


    }
}
