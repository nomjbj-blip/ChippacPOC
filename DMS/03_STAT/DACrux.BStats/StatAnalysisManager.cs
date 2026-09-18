using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Text;
using DACrux.ProjectManager.UI;
using DACrux.BStats.StatisticsInput;

namespace DACrux.BStats
{
    public static class StatAnalysisManager
    {
        public static StatInformation GetStatAnalysis(StatType type
            , DataTable dataSource
            , List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo
            , string strProject
            , string strWorkSheet
            , string strTitle
            , string strUser
            , FactorInfo[] TaguchiInfomation = null)
        {
            iStatInformation dlg = null;
            StatInformation statInfo = null;

            string resultFilePath = strProject + "-" + strWorkSheet + "-" + strTitle + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";

            try
            {
                switch (type)
                {
                    case StatType.DescriptiveAnalysis:
                        dlg = new StatDialog.DlgDescriptiveStatistics(dataSource, lstValidColumnInfo, strProject, strWorkSheet, strTitle, resultFilePath, strUser);
                        (dlg as StatDialog.DlgDescriptiveStatistics).On_SelectedDescriptiveStatistics += new DACrux.BStats.StatDialog.DSelectedDescriptiveStatistics(StatAnalysisManager_On_SelectedDescriptiveStatistics);
                        break;
                    case StatType.ANOVA:
                        dlg = new StatDialog.DlgANOVA(dataSource, lstValidColumnInfo, strProject, strWorkSheet, strTitle, resultFilePath, strUser);
                        (dlg as StatDialog.DlgANOVA).On_SelectedAnova += new DACrux.BStats.StatDialog.DSelectedAnoa(StatAnalysisManager_On_SelectedAnova);
                        break;
                    case StatType.CorrelationAnalysis:
                        dlg = new StatDialog.DlgCorrelation(dataSource, lstValidColumnInfo, strProject, strWorkSheet, strTitle, resultFilePath, strUser);
                        (dlg as StatDialog.DlgCorrelation).On_SelectedCorrelation += new DACrux.BStats.StatDialog.DSelectedCorrelation(StatAnalysisManager_On_SelectedCorrelation);
                        break;
                    case StatType.CpCpkAnalysis_Continuous:
                        dlg = new StatDialog.DlgCapaContinuous(dataSource, lstValidColumnInfo, strProject, strWorkSheet, strTitle, resultFilePath, strUser);
                        (dlg as StatDialog.DlgCapaContinuous).On_SelectedCapaContinuous += new DACrux.BStats.StatDialog.DSelectedCapaContinuous(StatAnalysisManager_On_SelectedCapaContinuous);
                        break;
                    case StatType.HypothesisTesting:
                        dlg = new StatDialog.DlgHypothesisTesting(dataSource, lstValidColumnInfo, strProject, strWorkSheet, strTitle, resultFilePath, strUser);
                        (dlg as StatDialog.DlgHypothesisTesting).On_Selected += new DACrux.BStats.StatDialog.DSelectedHypothesisTesting(StatAnalysisManager_On_Selected);
                        break;
                    case StatType.RegressionAnalysis:
                        dlg = new StatDialog.DlgRegression(dataSource, lstValidColumnInfo, strProject, strWorkSheet, strTitle, resultFilePath, strUser);
                        (dlg as StatDialog.DlgRegression).On_SelectedRegression += new DACrux.BStats.StatDialog.DSelectedRegression(StatAnalysisManager_On_SelectedRegression);
                        break;
                    //case StatType.DOE_FactorialDesign:
                    //    dlg = new StatDialog.DlgFactorialDesign();
                    //    break;
                    case StatType.DOE_TaguchiAnalysis:
                        dlg = new StatDialog.DlgTaguchiAnalysis2(dataSource, lstValidColumnInfo, strProject, strWorkSheet, strTitle, resultFilePath, strUser, TaguchiInfomation);
                        (dlg as StatDialog.DlgTaguchiAnalysis2).On_SelectedTaguchi += new DACrux.BStats.StatDialog.DSelectedTaguchi(StatAnalysisManager_On_SelectedTaguchi);
                        break;
                    default:
                        statInfo = null;
                        break;
                }
                if (dlg != null && (dlg as System.Windows.Forms.Form).ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    statInfo = dlg.StatInfo;
                }
                return statInfo;
            }
            catch(Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("An error occured during creating Stat Analysis.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                throw ex;
            }
        }


        #region [ Analysis Start ]

        #region Hypothesis testing

        static void StatAnalysisManager_On_Selected(DACrux.BStats.StatisticsInput.inputHypothesisTesting oSelected)
        {
            DACrux.BStats.Statistics.HypothesisTesting oHypothesisTesting = new DACrux.BStats.Statistics.HypothesisTesting(oSelected);
        }

        #endregion

        #region ANOVA

        static void StatAnalysisManager_On_SelectedAnova(DACrux.BStats.StatisticsInput.inputANOVA oSelected)
        {
            DACrux.BStats.Statistics.OneWayANOVA oOneWayANOVA = new DACrux.BStats.Statistics.OneWayANOVA(oSelected);
        }

        #endregion

        #region Regression Analysis

        static void StatAnalysisManager_On_SelectedRegression(DACrux.BStats.StatisticsInput.inputRegression oSelected)
        {
            DACrux.BStats.Statistics.Regression oRegression = new DACrux.BStats.Statistics.Regression(oSelected);
        }
        #endregion

        #region Process Capability Continuous

        static void StatAnalysisManager_On_SelectedCapaContinuous(DACrux.BStats.StatisticsInput.inputCapaContinuous oSelected)
        {
            DACrux.BStats.Statistics.CapaContinuous oCapaContinuous = new DACrux.BStats.Statistics.CapaContinuous(oSelected);
        }

        #endregion

        #region Correlation

        static void StatAnalysisManager_On_SelectedCorrelation(DACrux.BStats.StatisticsInput.inputCorrelation oSelected)
        {
            DACrux.BStats.Statistics.Correlation oCorrelation = new DACrux.BStats.Statistics.Correlation(oSelected);
        }

        #endregion

        #region Descriptive Statistics

        static void StatAnalysisManager_On_SelectedDescriptiveStatistics(DACrux.BStats.StatisticsInput.inputDescriptiveStatistics oSelected)
        {
            DACrux.BStats.Statistics.DescriptiveStatistics oDescriptiveStatistics = new DACrux.BStats.Statistics.DescriptiveStatistics(oSelected);
        }

        #endregion

        #endregion

        static void StatAnalysisManager_On_SelectedTaguchi(DACrux.BStats.StatisticsInput.inputTaguchi oSelected)
        {
            DACrux.BStats.Statistics.TaguchiAnalyusis2 oTaguchiStatistics = new DACrux.BStats.Statistics.TaguchiAnalyusis2(oSelected);
        }
    }
}
