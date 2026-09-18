using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using DACrux.BStats.StatisticsInput;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.Statistics
{
    public class OneWayANOVA
    {
        #region " MEMBER FIELD "
        /// <summary>
        /// Input Option
        /// </summary>
        private inputANOVA m_Input = null;

        private DACrux.BStats.Core.SelectedAnova m_SelectedAnova;
        private DACrux.BStats.Core.ResultAnova m_Result;
        private DataTable m_DataSource;
        private GraphInformation[] m_Graph = null;

        private int m_GraphWidth = 370;
        private int m_GraphHeight = 370;

        private const string m_Missing = "Not_assigned";

        #endregion

        #region " CREATOR "

        public OneWayANOVA(inputANOVA oInput)
        {
            if (oInput.IsSingle)
            {
                if (oInput.GroupColumn > -1)
                    oInput.DataSource = GetNonMissingGroup(oInput.DataSource, new int[] { oInput.GroupColumn });
                else
                {
#if DEBUG
                    throw new Exception("머니? 그룹컬럼없음!!");
#endif
                }
            }
            m_Input = oInput;
            m_SelectedAnova = new DACrux.BStats.Core.SelectedAnova();
            m_SelectedAnova.IsSingle = oInput.IsSingle;
            m_SelectedAnova.arrValidRowIndices = oInput.arrValidRowIndices;
            m_SelectedAnova.arrVariables = oInput.arrVariables;
            m_SelectedAnova.GroupColumn = oInput.GroupColumn;
            m_SelectedAnova.SingleColumn = oInput.SingleColumn;
            m_SelectedAnova.oGraph.IsBoxPlot = oInput.IsBoxPlot;
            m_SelectedAnova.oGraph.IsGraphHistogramResiduals = oInput.IsGraphHistogramResiduals;
            m_SelectedAnova.oGraph.IsGraphProbabilityPlotResiduals = oInput.IsGraphProbabilityPlotResiduals;
            m_SelectedAnova.oGraph.IsGraphResidualsVsFittedValues = oInput.IsGraphResidualsVsFittedValues;
            m_DataSource = oInput.DataSource;
            DACrux.BStats.Core.OneWayANOVA oOneWayANOVA = new DACrux.BStats.Core.OneWayANOVA(m_DataSource, m_SelectedAnova);
            m_Result = oOneWayANOVA.Result;

            m_Graph = GetGraphInfo(m_Result.TableforGraphs);
            MakeHtml(m_Result, m_Graph);            
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

        private void MakeHtml(DACrux.BStats.Core.ResultAnova oResult, DACrux.ProjectManager.UI.GraphInformation[] oGraph)
        {
            HtmlConverter oHtml = null;
            try
            {
                oHtml = new HtmlConverter(DACrux.ProjectManager.UI.Common.TempPath + m_Input.ResultFilePath);
                oHtml.MainTitle(m_Input.Project, m_Input.WorkSheet, m_Input.User, m_Input.Title);
                oHtml.SubTitle(oResult.GroupTitle);
                oHtml.BodyOneWayANOVA(oResult, oGraph);
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
            #region Core의 OneWayANOVA클래스 내의 Graph Title(클래스변수) 참고..
            const string strBoxPlot = "Boxplot of value by group";
            const string strHistogram = "Histogram of residuals";
            const string strProbability = "Probability Plot of the Residuals";
            const string strFit = "Residuals Versus the Fitted Values";
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
                        case(strBoxPlot):
                            arrReturn[i] = new GraphInformation(GraphType.BoxPlot);
                            arrReturn[i].Name = "BoxPlot(" + m_Input.Title + ")" + " : " + strBoxPlot;
                            arrReturn[i].IsMeanBoxplot = true;
                            for (int j = 0; j < dsSource.Tables[i].Columns.Count; j++)
                            arrReturn[i].AddAxisY(new GraphInformation.ColumnInfoItem(j, null, dsSource.Tables[i].Columns[j].ColumnName, dsSource.Tables[i].Columns[j].DataType));
                            arrReturn[i].DecimalPlaceX = DataTableUtil.GetDecimalPlace(dsSource.Tables[i], 0);
                            break;
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
                        
                    }
                    arrReturn[i].AxisXForceZero = false;
                    arrReturn[i].DataSource = dsSource.Tables[i];
                    arrReturn[i].Title = dsSource.Tables[i].TableName;
                    arrReturn[i].Frequence = true;
                    
                    arrReturn[i].IsImageWithTitles = true;
                    arrReturn[i].ImageSize = new System.Drawing.Size(m_GraphWidth, m_GraphHeight);
                    arrReturn[i].ImagePath = GetFileName(m_Input.ResultFilePath) + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + i.ToString()  + ".jpg";
                    



                    
                    //김현태 구현!!! 김현태김현태김현태김현태김현태김현태김현태
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
        #endregion
    }
}
