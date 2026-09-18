using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.BStats.StatisticsInput;

namespace DACrux.BStats.StatDialog
{
    public partial class DlgTaguchiAnalysis2 : DACrux.Framework.Base.DACruxUXBasic01, iStatInformation
    {


        #region [ LANGUAGE ]
        //string m_Title = "Taguchi Statistics";
        #endregion


        #region [ EVENT ]
        //public event DSelectedDescriptiveStatistics On_SelectedDescriptiveStatistics =  null;
        #endregion

        private inputTaguchi m_Input;

        //private System.Collections.Hashtable htColumns = null;

        private List<DACrux.ProjectManager.UI.DataView.ColumnInfo> m_lstValidColumnInfo;


        DlgTaguchi_.DlgTaguchiAnalysisGraph m_dlgGraph = null;
        DlgTaguchi_.DlgTaguchiAnalysisDetail m_dlgAnalysis = null;
        DlgTaguchi_.DlgTaguchiAnalysisOption m_dlgOption = null;
        public event DSelectedTaguchi On_SelectedTaguchi = null;

        public DlgTaguchiAnalysis2(DataTable dtSource
            , List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo
            , string strProject
            , string strWorkSheet
            , string strTitle
            , string strResultFilePath
            , string strUser
            , FactorInfo[] TaguchiInputInfomation)
        {
            InitializeComponent();
            
            this.FUNC_CODE = "F0301";

            m_Input = new inputTaguchi();

            m_lstValidColumnInfo = lstValidColumnInfo;
            m_Input.FactorInfos = TaguchiInputInfomation;

            if (TaguchiInputInfomation == null)
            {
                lbl_Factor.Enabled =
                butFactorDeSel.Enabled =
                butFactorSel.Enabled =
                dgFactorSelList.Enabled = true;
            }

            m_Input.Type = ProjectManager.UI.StatType.DOE_TaguchiAnalysis;
            m_Input.DataSource = dtSource;
            m_Input.Title = strTitle;
            m_Input.Project = strProject;
            m_Input.WorkSheet = strWorkSheet;
            m_Input.User = strUser;
            m_Input.ResultFilePath = strResultFilePath;
            //SetSetting();
            //InitDialog();
            SetColumnListView(m_lstValidColumnInfo);
        }

        void SetColumnListView(List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo)
        {
            try
            {
                dgAnalysisList.Rows.Clear();
                for (int i = 0; i < lstValidColumnInfo.Count; i++)
                {
                    dgAnalysisList.Rows.Add(new object[] { lstValidColumnInfo[i].ColumnID, lstValidColumnInfo[i].ColumnName });
                }

                /// 요인 리스트
                /// 
                if(m_Input.FactorInfos != null)
                {
                    for (int i = 0; i < m_Input.FactorInfos.Length; i++)
                    {
                        for (int j = 0; j < dgAnalysisList.Rows.Count; j++)
                        {
                            if (dgAnalysisList[1, j].Value.ToString() == m_Input.FactorInfos[i].Name.ToString())
                            {
                                dgAnalysisList.Rows.RemoveAt(j);
                            }
                        }
                        dgFactorSelList.Rows.Add(new object[] { m_Input.FactorInfos[i].ID, m_Input.FactorInfos[i].Name });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void butGraph_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dlgGraph == null)
                    m_dlgGraph = new DlgTaguchi_.DlgTaguchiAnalysisGraph();

                m_dlgGraph.chkAverage.Checked = m_Input.GraphAverage;
                m_dlgGraph.chkSNRatio.Checked = m_Input.GraphSNRatio;
                m_dlgGraph.chkStdev.Checked = m_Input.GraphSTDDev;

                if (m_dlgGraph.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_Input.GraphAverage = m_dlgGraph.chkAverage.Checked;
                    m_Input.GraphSNRatio = m_dlgGraph.chkSNRatio.Checked;
                    m_Input.GraphSTDDev = m_dlgGraph.chkStdev.Checked;
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dlgAnalysis == null)
                    m_dlgAnalysis = new DlgTaguchi_.DlgTaguchiAnalysisDetail();

                    m_dlgAnalysis.chkY_Average.Checked = m_Input.Analysis_Y_Average;
                    m_dlgAnalysis.chkY_SNRatio.Checked = m_Input.Analysis_Y_SNRatio;
                    m_dlgAnalysis.chkY_Stdev.Checked = m_Input.Analysis_Y_Stdev;

                    m_dlgAnalysis.chkL_Average.Checked = m_Input.Analysis_L_Average;
                    m_dlgAnalysis.chkL_SNRatio.Checked =  m_Input.Analysis_L_SNRatio;
                    m_dlgAnalysis.chkL_Stdev.Checked = m_Input.Analysis_L_Stdev;

                if (m_dlgAnalysis.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_Input.Analysis_Y_Average = m_dlgAnalysis.chkY_Average.Checked;
                    m_Input.Analysis_Y_SNRatio = m_dlgAnalysis.chkY_SNRatio.Checked;
                    m_Input.Analysis_Y_Stdev = m_dlgAnalysis.chkY_Stdev.Checked;

                    m_Input.Analysis_L_Average = m_dlgAnalysis.chkL_Average.Checked;
                    m_Input.Analysis_L_SNRatio = m_dlgAnalysis.chkL_SNRatio.Checked;
                    m_Input.Analysis_L_Stdev = m_dlgAnalysis.chkL_Stdev.Checked;
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butAnalysisDeSel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgAnalsysisSelList.SelectedRows.Count > 0)
                {
                    dgAnalysisList.Rows.Add(dgAnalsysisSelList.SelectedRows[0].Cells[0].Value, dgAnalsysisSelList.SelectedRows[0].Cells[1].Value);
                    dgAnalsysisSelList.Rows.Remove(dgAnalsysisSelList.SelectedRows[0]);
                }
            }
            catch (Exception ex)
            {
                DspError(ex);                
            }

        }
        private void butAnalysisSel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgAnalysisList.SelectedRows.Count > 0)
                {
                    dgAnalsysisSelList.Rows.Add(new object[] { dgAnalysisList.SelectedRows[0].Cells[0].Value, dgAnalysisList.SelectedRows[0].Cells[1].Value });
                    dgAnalysisList.Rows.Remove(dgAnalysisList.SelectedRows[0]);
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }


        private void butFactorDeSel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgFactorSelList.SelectedRows.Count > 0)
                {
                    dgAnalysisList.Rows.Add(dgFactorSelList.SelectedRows[0].Cells[0].Value, dgFactorSelList.SelectedRows[0].Cells[1].Value);
                    dgFactorSelList.Rows.Remove(dgFactorSelList.SelectedRows[0]);
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butFactorSel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgAnalysisList.SelectedRows.Count > 0)
                {
                    dgFactorSelList.Rows.Add(new object[] { dgAnalysisList.SelectedRows[0].Cells[0].Value, dgAnalysisList.SelectedRows[0].Cells[1].Value });
                    dgAnalysisList.Rows.Remove(dgAnalysisList.SelectedRows[0]);
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butOption_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dlgOption == null)
                    m_dlgOption = new DlgTaguchi_.DlgTaguchiAnalysisOption();

                m_dlgOption.chkTarget.Checked = m_Input.OptionCheckTarget;
                m_dlgOption.chkStddevIns.Checked = m_Input.OptionCheckStdDevIns;
                m_dlgOption.TaguchiRule = m_Input.tgcAnalysisRule;

                if (m_dlgOption.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_Input.OptionCheckTarget = m_dlgOption.chkTarget.Checked;
                    m_Input.OptionCheckStdDevIns = m_dlgOption.chkStddevIns.Checked;
                    m_Input.tgcAnalysisRule = m_dlgOption.TaguchiRule;
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_Input.AnalysisVariables == null) m_Input.AnalysisVariables = new System.Collections.ArrayList();
                
                m_Input.AnalysisVariables.Clear();
                if (m_Input.FactorInfos == null)
                {
                    List<FactorInfo> tmp = new List<FactorInfo>();
                    for (int i = 0; i < dgFactorSelList.RowCount; i++)
                        tmp.Add(new FactorInfo(dgFactorSelList[0, i].Value.ToString(), dgFactorSelList[1, i].Value.ToString(), "", -1, -1));

                    m_Input.FactorInfos = tmp.ToArray();
                }

                for (int i = 0; i < dgAnalsysisSelList.RowCount; i++)
                    m_Input.AnalysisVariables.Add(dgAnalsysisSelList[1, i].Value);

                if (On_SelectedTaguchi != null)
                    On_SelectedTaguchi(m_Input);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }


        public ProjectManager.UI.GraphInformation[] GraphInformations
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ProjectManager.UI.StatInformation StatInfo
        {
            get { return m_Input.StatInfo; }
        }
    }
}
