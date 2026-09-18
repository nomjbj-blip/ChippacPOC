using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using DACrux.BStats.StatisticsInput;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.Statistics
{
    public class HypothesisTesting
    {
        #region " MEMBER FIELD "

        /// <summary>
        /// Input Option
        /// </summary>
        private inputHypothesisTesting m_Input = null;

        private DACrux.BStats.Core.SelectedHypothesisTesting m_SelectedHypothesisTesting;
        private DACrux.BStats.Core.ResultHypothesisTesting[] m_Result;
        private DataTable m_DataSource;
        //private GraphInformation[] m_Graph = null;
        #endregion

        #region " CREATOR "
        public HypothesisTesting(inputHypothesisTesting oInput)
        {
            m_Input = oInput;
            m_SelectedHypothesisTesting = new DACrux.BStats.Core.SelectedHypothesisTesting();
            m_SelectedHypothesisTesting.Testing = oInput.Testing;
            m_SelectedHypothesisTesting.oMethods.Alternative = oInput.Alternative;
            m_SelectedHypothesisTesting.oMethods.ConfidenceLevel = oInput.ConfidenceLevel;
            m_SelectedHypothesisTesting.oMethods.TestingDifference = oInput.TestingDifference;
            m_SelectedHypothesisTesting.oMethods.TestingMean = oInput.TestingMean;
            m_SelectedHypothesisTesting.oMethods.TestingStd = oInput.TestingStd;
            m_SelectedHypothesisTesting.Options.IsConfidenceInterval = oInput.IsConfidenceInterval;
            m_SelectedHypothesisTesting.Options.IsDescriptiveStat = oInput.IsDescriptiveStatistics;
            m_SelectedHypothesisTesting.Options.IsCriticalValue = oInput.IsCriticalValue;
            m_DataSource = oInput.DataSource;
            m_Result = GetAnalysisResult(m_DataSource, m_Input.arrVariables, m_SelectedHypothesisTesting);
            MakeHtml(m_Result, m_SelectedHypothesisTesting);
        }
        #endregion

        #region " METHOD "

        #region  [Html]

        private void MakeHtml(DACrux.BStats.Core.ResultHypothesisTesting[] oResult, DACrux.BStats.Core.SelectedHypothesisTesting oTestingOption)
        {
            HtmlConverter oHtml = null;
            int iCnt;
            try
            {
                oHtml = new HtmlConverter(DACrux.ProjectManager.UI.Common.TempPath + m_Input.ResultFilePath);
                oHtml.MainTitle(m_Input.Project, m_Input.WorkSheet, m_Input.User, m_Input.Title);
                iCnt = oResult.Length;
                for (int i = 0; i < iCnt; i++)
                {
                    oHtml.BodyHypothesisTesting(oResult[i], oTestingOption.Testing, oTestingOption.oMethods.Alternative, oTestingOption.Options.IsDescriptiveStat, oTestingOption.Options.IsConfidenceInterval, oTestingOption.Options.IsCriticalValue);
                }
                oHtml.SaveHtml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private DACrux.BStats.Core.ResultHypothesisTesting[] GetAnalysisResult(DataTable dtSource, int[] arrSelectedCol, DACrux.BStats.Core.SelectedHypothesisTesting oOption)
        {
            DACrux.BStats.Core.ResultHypothesisTesting[] arrReturn = null;
            int iColCnt;
            int iUnitCnt;
            try
            {
                if (arrSelectedCol == null)
                    throw new Exception("Selected columns is null");
                iColCnt = arrSelectedCol.Length;
                if(iColCnt <1)
                    throw new Exception("Not enough selected columns");
                if (oOption.Testing == DACrux.BStats.Core.TestingType.OneSampleT || oOption.Testing == DACrux.BStats.Core.TestingType.OneSampleZ)
                {
                    iUnitCnt = iColCnt;
                    arrReturn = new DACrux.BStats.Core.ResultHypothesisTesting[iUnitCnt];
                    for (int i = 0; i < iUnitCnt; i++)
                    {
                        DACrux.BStats.Core.HypothesisTesting oTest = new DACrux.BStats.Core.HypothesisTesting(dtSource, arrSelectedCol[i], oOption);
                        arrReturn[i] = oTest.Result;
                    }
                }
                else
                {
                    if (arrSelectedCol.Length < 2)
                        throw new Exception("Not enough selected columns");
                    iUnitCnt = (iColCnt * iColCnt - iColCnt) / 2;
                    arrReturn = new DACrux.BStats.Core.ResultHypothesisTesting[iUnitCnt];
                    int iUnit = 0;
                    for (int i = 0; i < iColCnt-1; i++)
                    {
                        for (int j = i + 1; j < iColCnt; j++)
                        {
                            DACrux.BStats.Core.HypothesisTesting oTest = new DACrux.BStats.Core.HypothesisTesting(dtSource, arrSelectedCol[i], arrSelectedCol[j], oOption);
                            arrReturn[iUnit] =  oTest.Result;
                            iUnit++;
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

        #endregion

    }
}
