using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using DACrux.BStats.StatisticsInput;
using DACrux.BStats;

namespace DACrux.ProjectManager
{
    public sealed class WorkSheet : ITreeViewDrawable
    {
        #region " MEMBER FIELD "

        public static Project oProject = null;
        private frmWorkSheet oFrmWorkSheet;

        public List<Analysis> lstAnalysis = new List<Analysis>();

        private FactorInfo[] m_taguchiInfo = null;

        private string strName = string.Empty;

        private ModelType modelType = ModelType.WorkSheet;

        public DataTable DataSource
        {
            get
            {
                return oFrmWorkSheet.DataSource;
            }
            set
            {
                oFrmWorkSheet.DataSource = value; 
            }
        }

        public DataTable StatDataSource
        {
            get
            {
                return oFrmWorkSheet.StatDataSource;
            }
        }

        public DataTable PureDataSource
        {
            get
            {
                return oFrmWorkSheet.PureDataSource;
            }
        }

        public List<DACrux.ProjectManager.UI.DataView.ColumnInfo> CopiedValidColumnInfo
        {
            get
            {
                return oFrmWorkSheet.CopiedValidColumnInfo;
            }
        }

        public List<DACrux.ProjectManager.UI.DataView.ColumnInfo> StatColumnInfo
        {
            get
            {
                return oFrmWorkSheet.StatColumnInfo;
            }
        }

        public FactorInfo[] TaguchiInfos
        {
            get
            {
                return m_taguchiInfo;
            }
            set
            {
                m_taguchiInfo = value;
            }
        }

        #endregion

        #region " PROPERTY "

        public string Name
        {
            get { return strName; }
            set 
            { 
                strName = value;

                if(WorkSheetRenamed != null)
                    WorkSheetRenamed(strName);
            }
        }

        public string Parent
        {
            get { return oProject.Name; }
        }

        public ModelType ModelType
        {
            get { return modelType; }
        }

        public int ItemCount
        {
            get { return lstAnalysis.Count; }
        }

        public frmWorkSheet Form
        {
            get { return oFrmWorkSheet; }
        }
        
        #endregion

        #region " CREATOR "

        public WorkSheet() 
            : this(string.Empty)
        {
        }

        public WorkSheet(string name) 
            : this(name, null)
        {
        }

        public WorkSheet(string name, DataTable dataSource)
        {
            oProject = Project.GetInstance();

            if (name == string.Empty)
                strName = GetNewInstanceName();
            else
            {
                if (!oProject.CheckWorkSheetExist(name))
                    strName = name;
                else
                {
                    strName = GetNewInstanceName();

                    if (OnWorkSheetNameDuplicated != null)
                        OnWorkSheetNameDuplicated(name, strName);
                }
            }

            oFrmWorkSheet = new frmWorkSheet(this, dataSource);
            oFrmWorkSheet.OnNewDataSourceAdded += new NewDataSourceAddedHandler(oFrmWorkSheet_OnNewDataSourceAdded);

            oFrmWorkSheet.FormClosing += new System.Windows.Forms.FormClosingEventHandler(oFrmWorkSheet_FormClosing);
        }

        #endregion

        #region " EVENT "

        public event WorkSheetRenamedHandler WorkSheetRenamed;
        public event AnalysisAddedHandler AnalysisAdded;
        public event AnalysisRemovedHandler OnAnalysisRemoved;
        public event NewDataSourceAddedHandler OnNewDataSourceAdded;
        public event WorkSheetNameDuplicatedHandler OnWorkSheetNameDuplicated;

        #endregion

        #region " METHOD "

        public string GetNewInstanceName()
        {
            bool bDuplicate = true;
            string strNewInstanceName = string.Empty;
            int iTemp = 1;
            try
            {
                while(bDuplicate == true)
                {
                    strNewInstanceName = string.Format("WorkSheet{0}", iTemp++);
                    bDuplicate = oProject.CheckWorkSheetExist(strNewInstanceName);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: Project.GetNewInstanceName()"));
            }

            return strNewInstanceName;
        }

        public void AddAnalysis(Analysis analysis, bool bFocus)
        {
            try
            {
                lstAnalysis.Add(analysis);

                if (AnalysisAdded != null)
                    AnalysisAdded(analysis, bFocus);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: WorkSheet.AddAnalysis(Analysis Analysis)"));
            }
        }

        public void RemoveAnalysis(Analysis analysis)
        {
            try
            {
                if (OnAnalysisRemoved != null)
                    OnAnalysisRemoved(analysis);

                lstAnalysis.Remove(analysis);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: WorkSheet.RemoveWorkSheet(Analysis Analysis)"));
            }
        }

        public void RemoveAnalysisAt(int index)
        {
            Analysis analysis;

            try
            {
                analysis = lstAnalysis[index];

                if (OnAnalysisRemoved != null)
                    OnAnalysisRemoved(analysis);

                lstAnalysis.RemoveAt(index);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: WorkSheet.RemoveAnalysisAt(int index)"));
            }
        }

        public Analysis GetAnalysisAt(int index)
        {
            Analysis analysis;

            try
            {
                analysis = lstAnalysis[index];
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: WorkSheet.GetWorkSheetAt(int index)"));
            }

            return analysis;
        }

        public ITreeViewDrawable GetItemAt(int index)
        {
            ITreeViewDrawable drawable;

            try
            {
                drawable = lstAnalysis[index] as ITreeViewDrawable;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: WorkSheet.GetItemAt(int index)"));
            }

            return drawable;
        }

        public Analysis GetAnalysis(string name)
        {
            foreach (Analysis analysis in lstAnalysis)
            {
                if (analysis.Name == name)
                    return analysis;
            }
            return null;
        }

        public int GetAnalysisIndex(Analysis analysis)
        {
            return lstAnalysis.IndexOf(analysis);
        }

        public int GetAnalysisIndex(string name)
        {
            Analysis analysis = GetAnalysis(name);

            if (analysis != null)
                return lstAnalysis.IndexOf(analysis);
            else
                return -1;
        }
        
        internal void Close()
        {
            for(int i=0; i<lstAnalysis.Count; i++)
            {
                lstAnalysis[i].Close();
            }

            lstAnalysis.Clear();
            Form.Dispose();
        }

        #endregion

        #region " EVENT HANDLER "

        private void oFrmWorkSheet_OnNewDataSourceAdded(string name, DataTable dt)
        {
            if (OnNewDataSourceAdded != null)
                OnNewDataSourceAdded(name, dt);
        }

        void oFrmWorkSheet_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
            {
                e.Cancel = true;
                oFrmWorkSheet.Hide();
            }
        }

        #endregion
    }        
}
