using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using DACrux.BStats.StatisticsInput;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.Statistics
{
    public class CapaContinuous
    {
        #region " MEMBER FIELD "
        /// <summary>
        /// Input Option
        /// </summary>
        private inputCapaContinuous m_Input = null;


        #region Message
        private string m_ErrorArgumentEqual = "SingleDataColumnIndex equals SubgroupColumnIndex.";
        private string m_ErrorArgumentNoValueColumn = "Please select column for analysis.";
        private string m_ErrorArgumentContantForSubgroupSize = "Sub group size must be positive natural number.";
        private string m_ErrorSubgroupSizeContant = "Data rows Count must be greater than subgroup size contant.";
        #endregion

        ///// <summary>
        ///// Result For Html File
        ///// </summary>
        //private DACruxSet m_dsResult = null;

        ///// <summary>
        ///// Raw Data For Descriptive Statistics
        ///// </summary>
        //private DACruxSet[] m_RawData = null;

        #region [ CHART INFO ]
        //private GraphInformation[] m_Histogram = null;
        //private int m_DescriptionHistogramH = 300;
        //private int m_DescriptionHistogramW = 300;
        #endregion

        private DataTable m_DataSource;
        private int[] m_DataColumns;

        private DACrux.BStats.Core.SelectedCapaContinuous m_SelectedCapaContinuous;
        private DACrux.BStats.Core.ResultCapaContinuous m_ResultCapaContinuous;

        #endregion

        #region " CREATOR "

        public CapaContinuous(inputCapaContinuous oInput)
        {
            m_Input = oInput;
            m_SelectedCapaContinuous = new DACrux.BStats.Core.SelectedCapaContinuous();
            m_SelectedCapaContinuous.Reset();
            
            m_SelectedCapaContinuous.HistoricalBetweenStd = oInput.HistoricalStdBetween;
            m_SelectedCapaContinuous.HistoricalMean = oInput.HistoricalMean;
            m_SelectedCapaContinuous.HistoricalWithinStd = oInput.HistoricalStdWithin;
            //m_SelectedCapaContinuous.IsUseConstantForSubgroupSize = oInput.IsUseContantForSubgroupSize;
            m_SelectedCapaContinuous.LowerSpec = oInput.LSL;
            //m_SelectedCapaContinuous.SubGroupSize = oInput.SubGroupSize;
            m_SelectedCapaContinuous.UpperSpec = oInput.USL;           

            #region Options
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.ConfidenceIntervals = oInput.ResultConfidenceIntervalsType;
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.ConfidenceLevel = oInput.ConfidenceLevel;
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.IsBetweenWithinAnalysis = oInput.IsBetweenWithinAnalysis;
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.IsIncludeConfidenceIntervals = oInput.IsIncludeConfidenceIntervals;
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.IsOverallAnalysis = oInput.IsOverallAnalysis;
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.KsigmaForCapability = oInput.Ksigma;
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.ResultDisplayType = oInput.ResultDisplayType;
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.ResultStatisticType = oInput.ResultStatisticType;
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.Target = oInput.Target;
            m_SelectedCapaContinuous.SelectedCapaContinuousOptions.UserTitle = oInput.UserTitle;
            #endregion
            #region Estimate
            m_SelectedCapaContinuous.Estimate.EstimationBetweenSubgroup = oInput.EstimationBetweenSubgroup;
            m_SelectedCapaContinuous.Estimate.EstimationWithinSubgroup = oInput.EstimationWithinSubgroup;
            m_SelectedCapaContinuous.Estimate.IsUseUnbiasingOverall = oInput.IsUseUnbiasingConstantsOverall;
            m_SelectedCapaContinuous.Estimate.IsUseUnbiasingWithin = oInput.IsUseUnbiasingConstantsWithin;
            m_SelectedCapaContinuous.Estimate.MovingRangeofLength = oInput.MovingRangeofLength;
            #endregion

            if (oInput.UserTitle.Trim() != string.Empty) //사용자 제목있을 경우
                m_Input.Title = oInput.UserTitle;


            #region GetInpuDataTable
            if (oInput.arrVariables == null || oInput.arrVariables.Length == 0)
            {
                if (oInput.SingleColumnIndex < 0)
                    throw new ArgumentException(m_ErrorArgumentNoValueColumn, new Exception("MIRACOM"));  // Data Column으로 선택된 것이 아무것도 없음. 어떻게 하라고????
                if (oInput.IsUseContantForSubgroupSize)  //상수 사용
                {
                    if(oInput.SubGroupSize<1)
                        throw new ArgumentException(m_ErrorArgumentContantForSubgroupSize);  // 샘플 사이즈를 상수를 사용하는데....1보다 작다? 말이 안됨....
                    if (oInput.DataSource.Rows.Count < oInput.SubGroupSize)
                        throw new ArgumentException(m_ErrorSubgroupSizeContant, new Exception("MIRACOM"));
                    m_DataSource = GetInpuDataTable(oInput.DataSource, oInput.SingleColumnIndex, -1, oInput.SubGroupSize, null, out m_DataColumns);
                }
                else // 그룹 컬럼 사용
                {
                    if(oInput.SingleColumnIndex == oInput.SubgroupColumnIndex)
                        throw new ArgumentException(ErrorWrongParameter("SingleColumnIndex, SubgroupColumnIndex"), new Exception("MIRACOM"));  // 자료와 그룹 컬럼이 같을 순 없잖아????

                    m_DataSource = GetInpuDataTable(oInput.DataSource, oInput.SingleColumnIndex, oInput.SubgroupColumnIndex, -1, null, out m_DataColumns);
                }
            }
            else // 여러 컬럼이 모여서 하나의 서브그룹이 됨
            {
                m_DataSource = GetInpuDataTable(oInput.DataSource, -1, -1, -1, oInput.arrVariables, out m_DataColumns);
            }
            #endregion

            DACrux.BStats.Core.CapaContinuous oCapa = new DACrux.BStats.Core.CapaContinuous(m_DataSource, m_DataColumns, m_SelectedCapaContinuous);
            m_ResultCapaContinuous = oCapa.Result;
            m_Input.GraphInformations = GetHistogram(m_DataSource, m_DataColumns, "Histogram(" + m_Input.Title + ")", m_Input.Title, GetFileName(m_Input.ResultFilePath) + "Histogram" + DateTime.Now.ToString("yyyyMMddHHmmss") + "jpg", m_Input.LSL, m_Input.Target, m_Input.USL);
            MakeHtml(m_ResultCapaContinuous);

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

        #region Get Input DataTable
        /// <summary>
        /// 여러 형태의 DataTable을 여러 Value Columns형태의 DataTable로 돌려주고 해당 Index List를 out해준다
        /// </summary>
        /// <param name="dtOriginal"></param>
        /// <param name="iSingleDataColumnIndex">하나의 Column으로 되어 있을 경우(지정안된 경우 -1)</param>
        /// <param name="iSubgroupColumnIndex">서브그룹을 구별하는 컬럼(지정안된 경우 -1)</param>
        /// <param name="iSubGroupSize">서브그룹당 측정횟수(지정안된 경우 1보다 작은 수 )</param>
        /// <param name="arrVariables">하나의 서브그룹에 대한 측정값이 여러 컬럼으로 나눠져 있는 경우</param>
        /// <param name="arrDataColumns">return DataTable에서의 Data Column Index Array</param>
        /// <returns></returns>
        private DataTable GetInpuDataTable(DataTable dtOriginal, int iSingleDataColumnIndex, int iSubgroupColumnIndex, int iSubGroupSize, int[] arrVariables, out int[] arrDataColumns)
        {
            DataTable dtReturn = null;
            int iValueColumnsCnt;
            int iOrigRowCnt;
            int iOrigColCnt;
            //int[] arrSortedVariables = null;
            try
            {
                #region Input Check
                if (dtOriginal == null)
                    throw new ArgumentException(ErrorNullParameter("dtOriginal"));
                iOrigRowCnt = dtOriginal.Rows.Count;
                if(iOrigRowCnt<1)
                    throw new ArgumentException(ErrorNullParameter("dtOriginal"));
                if (iSingleDataColumnIndex < 0 && arrVariables == null)
                    throw new ArgumentException(ErrorNullParameter("arrVariables"));
                if (iSubgroupColumnIndex < 0 && iSubGroupSize < 1 && arrVariables == null)
                    throw new ArgumentException(ErrorNullParameter("arrVariables"));
                if (iSingleDataColumnIndex < 0 && iSubgroupColumnIndex < 0 && iSubGroupSize < 1 && arrVariables == null)
                    throw new ArgumentException(ErrorNullParameter("arrVariables"));
                if (arrVariables != null)
                {
                    iValueColumnsCnt = arrVariables.Length;
                    if (iSingleDataColumnIndex < 0 && iValueColumnsCnt < 1)
                        throw new ArgumentException(ErrorNullParameter("iSingleDataColumnIndex"));
                    if (iSubgroupColumnIndex < 0 && iSubGroupSize < 1 && iValueColumnsCnt < 1)
                        throw new ArgumentException(ErrorNullParameter("iSubgroupColumnIndex"));
                    if (iSingleDataColumnIndex < 0 && iSubgroupColumnIndex < 0 && iSubGroupSize < 1 && iValueColumnsCnt < 1)
                        throw new ArgumentException(ErrorNullParameter("iSubgroupColumnIndex"));
                }

                #endregion

                iOrigColCnt = dtOriginal.Columns.Count;
                if (arrVariables != null && arrVariables.Length > 0) //원본 DataTable에서 Data Columns이 지정된 경우
                {
                    iValueColumnsCnt = arrVariables.Length;
                    arrDataColumns = new int[iValueColumnsCnt];
                    dtOriginal.AcceptChanges();
                    dtReturn = dtOriginal.Copy();
                    //arrSortedVariables = Array.Sort(arrVariables);

                    int iMapping = iValueColumnsCnt - 1;
                    for (int i = iOrigColCnt - 1; i > -1; i--)
                    {
                        int iIndex = -1;
                        iIndex = Array.IndexOf(arrVariables, i);
                        if (iIndex != -1)
                        {
                            arrDataColumns[iIndex] = iMapping;
                            iMapping--;
                        }
                        else
                        {
                            dtReturn.Columns.RemoveAt(i);
                            dtReturn.AcceptChanges();
                        }
                    }                    
                }
                else // 하나의 Data Column을 서브그룹측정횟수(상수)나, 서브그룹에 대한 측정값이 여러 컬럼으로 나눠져 있는 경우
                {
                    if(iSingleDataColumnIndex<0)
                        throw new ArgumentException(ErrorNullParameter("iSingleDataColumnIndex"));  //Data Column이 어딘지 알수 없음

                    if (!(iSubgroupColumnIndex < 0) && iSubGroupSize > 0)
                        throw new ArgumentException(ErrorNullParameter("iSubgroupColumnIndex"));  //뭘 기준으로 하라고????
                    else if (iSubgroupColumnIndex < 0 && iSubGroupSize > 0)  // 측정횟수(상수) 기준으로
                    {
                        
                        arrDataColumns = new int[iSubGroupSize];
                        dtReturn = new DataTable();
                        for (int i = 0; i < iSubGroupSize; i++)
                        {
                            arrDataColumns[i] = i;
                            dtReturn.Columns.Add(i.ToString(), dtOriginal.Columns[iSingleDataColumnIndex].DataType);
                        }
                        int iRow = 0;
                        for (int i = 0; i < iOrigRowCnt; i++)
                        {
                            dtReturn.Rows.Add(dtReturn.NewRow());
                            for (int j = 0; j < iSubGroupSize; j++)
                            {
                                if (i < iOrigRowCnt)
                                {
                                    if (j != 0)
                                        i++;
                                    dtReturn.Rows[iRow][j] = dtOriginal.Rows[i][iSingleDataColumnIndex];
                                }
                            }
                            iRow++;
                        }
                    }
                    else // Subgroup Column 기준으로
                    {
                        if(iSingleDataColumnIndex == iSubgroupColumnIndex)
                            throw new ArgumentException(m_ErrorArgumentEqual, new Exception("MIRACOM"));  //뭘 기준으로 하라고????

                        arrDataColumns = null;
                        List<int> arrTempIndex = new List<int>();
                        int iTemp=0;
                        dtReturn = new DataTable();
                        while (dtOriginal.Rows.Count > 0)
                        {
                            string strValueColumName = dtOriginal.Rows[0][iSubgroupColumnIndex].ToString();
                            DataRow[] arrTemp = dtOriginal.Select("[" + dtOriginal.Columns[iSubgroupColumnIndex].ColumnName + "] = '" + strValueColumName + "'");
                            int iTempCnt = arrTemp.Length;
                            if (iTempCnt > 0)
                            {

                                #region 선택된 정보를 컬럼으로..
                                //dtReturn.Columns.Add(strValueColumName, dtOriginal.Columns[iSingleDataColumnIndex].DataType);
                                //for (int i = 0; i < iTempCnt; i++)
                                //{
                                //    if (!(i < dtReturn.Rows.Count))
                                //    {
                                //        dtReturn.Rows.Add(dtReturn.NewRow());
                                //    }
                                //    dtReturn.Rows[i][strValueColumName] = arrTemp[i][iSingleDataColumnIndex];
                                //    dtOriginal.Rows.Remove(arrTemp[i]);
                                //}
                                //arrTempIndex.Add(iTemp);
                                
                                #endregion

                                #region 선택된 정보를 행으로
                                if (dtReturn.Columns.Count < iTempCnt)
                                {
                                    for (int i = dtReturn.Columns.Count; i < iTempCnt; i++)
                                        dtReturn.Columns.Add(i.ToString(), dtOriginal.Columns[iSingleDataColumnIndex].DataType);
                                }
                                dtReturn.Rows.Add(dtReturn.NewRow());

                                for (int i = 0; i < iTempCnt; i++)
                                {
                                    dtReturn.Rows[iTemp][i] = arrTemp[i][iSingleDataColumnIndex];
                                    dtOriginal.Rows.Remove(arrTemp[i]);
                                }                                
                                #endregion

                                iTemp++; 
                            }
                            else
                            {
#if DEBUG
                                throw new Exception("??????????뭥미???????????????");
#endif
                            }

                        }
                        #region 선택된 정보를 컬럼으로..
                        //arrDataColumns = arrTempIndex.ToArray();
                        #endregion
                        #region 선택된 정보를 행으로
                        arrDataColumns = new int[dtReturn.Columns.Count];
                        for (int i = 0; i < arrDataColumns.Length; i++)
                            arrDataColumns[i] = i;
                        #endregion

                    }

                }

                for (int i = dtReturn.Rows.Count - 1; i > -1; i--)
                {
                    bool bNull = true;
                    for (int j = 0; j < arrDataColumns.Length; j++)
                    {
                        if (dtReturn.Rows[i][arrDataColumns[j]] != DBNull.Value)
                        {
                            bNull = false;
                            break;
                        }
                    }
                    if (!bNull)
                        break;
                    dtReturn.Rows.RemoveAt(i);
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

        #region Get Histogram
        private GraphInformation[] GetHistogram(DataTable dt, int[] arrDataColumns, string strGraphName, string strGraphTitle, string strImagePath, double LSL, double Target, double USL)
        {
            GraphInformation[] arrReturn = null;
            DataTable dtTemp = null;
            int iRowCnt;
            int iColCnt;
            try
            {
                iColCnt = arrDataColumns.Length;
                iRowCnt = dt.Rows.Count;
                dtTemp = new DataTable();
                dtTemp.Columns.Add();
                for (int i = 0; i < iRowCnt; i++)
                {
                    for (int j = 0; j < iColCnt; j++)
                    {
                        dtTemp.Rows.Add(new object[] { dt.Rows[i][j] });
                    }
                }
                dtTemp.AcceptChanges();


                arrReturn = new GraphInformation[1];
                arrReturn[0] = new GraphInformation(GraphType.Histogram);

                arrReturn[0].DataSource = dtTemp;
                arrReturn[0].DecimalPlaceX = DataTableUtil.GetDecimalPlace(dtTemp);
                arrReturn[0].NormalLine = true;
                arrReturn[0].AxisXForceZero = false;
                arrReturn[0].ForceZero = false;
                arrReturn[0].AddAxisY(new GraphInformation.ColumnInfoItem(0, null, "Data", dtTemp.Columns[0].DataType));
                arrReturn[0].Name = strGraphName;
                arrReturn[0].Title = strGraphTitle;
                arrReturn[0].ImagePath = strImagePath;
                arrReturn[0].ImageSize = new System.Drawing.Size(400, 300);
                arrReturn[0].DecimalPlace = 2;
                arrReturn[0].SpecLimit = true;
                arrReturn[0].LSL = LSL;
                arrReturn[0].Target = Target;
                arrReturn[0].USL = USL;
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Html

        private void MakeHtml(DACrux.BStats.Core.ResultCapaContinuous oResult)
        {
            HtmlConverter oHtml = null;
            try
            {
                oHtml = new HtmlConverter(DACrux.ProjectManager.UI.Common.TempPath + m_Input.ResultFilePath);
                oHtml.MainTitle(m_Input.Project, m_Input.WorkSheet, m_Input.User, m_Input.Title);
                oHtml.BodyCapaContinuous(oResult, m_Input.ResultDisplayType, m_Input.ResultStatisticType, m_Input.GraphInformations[0].ImagePath);
                oHtml.SaveHtml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

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
