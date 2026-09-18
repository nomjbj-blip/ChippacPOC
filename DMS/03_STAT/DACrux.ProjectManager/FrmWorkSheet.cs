using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DACrux.ProjectManager.UI;
using DACrux.BStats;

namespace DACrux.ProjectManager
{
    public partial class frmWorkSheet : Form
    {
        #region " MEMBER FIELD "

        private WorkSheet oWorkSheet;

        #endregion

        #region " PROPERTY "

        [Browsable(false)]
        public DataTable DataSource
        {
            get 
            {
                return uctlDataView.DataSource;
            }
            set
            {
                uctlDataView.DataSource = value;
            }
        }

        [Browsable(false)]
        public DataTable StatDataSource
        {
            get
            {
                return uctlDataView.StatDataSource;
            }
        }

        [Browsable(false)]
        public DataTable PureDataSource
        {
            get
            {
                return uctlDataView.PureDataSource;
            }
        }

        [Browsable(false)]
        public List<DACrux.ProjectManager.UI.DataView.ColumnInfo> CopiedValidColumnInfo
        {
            get
            {
                return uctlDataView.CopiedValidColumnInfoList;
            }
        }

        [Browsable(false)]
        public List<DACrux.ProjectManager.UI.DataView.ColumnInfo> StatColumnInfo
        {
            get
            {
                return uctlDataView.StatColumnInfoList;
            }
        }

        [Browsable(false)]
        public WorkSheet WorkSheet
        {
            get { return oWorkSheet; }
            set { oWorkSheet = value; }
        }

        #endregion

        #region " CREATOR "

        public frmWorkSheet(WorkSheet workSheet, DataTable dataSource)
        {
            InitializeComponent();

            this.oWorkSheet = workSheet;
            this.Text = workSheet.Name;

            if (dataSource != null && dataSource.Columns.Count > 0 && dataSource.Rows.Count > 0)
            {
                uctlDataView.IsSplited = true;
                uctlDataView.DataSource = dataSource;
            }
            else
            {
                uctlDataView.IsSplited = false;
            }
        }

        #endregion

        #region " EVENT "

        public event NewDataSourceAddedHandler OnNewDataSourceAdded;

        #endregion

        #region " METHOD "

        internal void ProcessDataViewContextMenu(string menuName)
        {
            uctlDataView.ProcessDataViewContextMenu(menuName);
        }

        private void OnWorkSheetRenamed(string name)
        {
            this.Text = name;
        }

        private void OnAnalysisAdded(Analysis analysis, bool bFocus)
        {
            if (analysis is StatAnalysis)
            {
                StatAnalysis statAnalysis = analysis as StatAnalysis;
                
                uctlAnalysisView.Add(statAnalysis.StatInfo);

                if(bFocus)
                {
                    uctlDataView.Visible = false;
                    uctlAnalysisView.Visible = true;
                    uctlGraphView.Visible = false;
                }
            }
            else if (analysis is GraphAnalysis)
            {
                GraphAnalysis graphAnalysis = analysis as GraphAnalysis;

                uctlGraphView.Add(graphAnalysis.GraphInfo);

                if(bFocus)
                {
                    uctlDataView.Visible = false;
                    uctlAnalysisView.Visible = false;
                    uctlGraphView.Visible = true;
                }
            }
        }

        internal void SetDefaultView()
        {
            uctlDataView.Visible = true;
            uctlAnalysisView.Visible = false;
            uctlGraphView.Visible = false;
        }

        internal void SetStatView()
        {
            uctlDataView.Visible = false;
            uctlAnalysisView.Visible = true;
            uctlGraphView.Visible = false;
        }

        internal void SetGraphView()
        {
            uctlDataView.Visible = false;
            uctlAnalysisView.Visible = false;
            uctlGraphView.Visible = true;
        }

        #endregion

        #region " EVENT_HANDLER "

        private void frmWorkSheet_Load(object sender, EventArgs e)
        {
            oWorkSheet.WorkSheetRenamed += new WorkSheetRenamedHandler(OnWorkSheetRenamed);
            oWorkSheet.AnalysisAdded += new AnalysisAddedHandler(OnAnalysisAdded);

            uctlDataView.NewDataSourceSplited += new NewDataSourceSplitedHandler(uctlDataView_NewDataSourceSplited);
            uctlGraphView.GraphUpdating += new GraphUpdatingHandler(uctlGraphView_GraphUpdating);
            uctlGraphView.GraphDeleted += new GraphDeletedHandler(uctlGraphView_GraphDeleted);
            uctlAnalysisView.StatChanged += new StatChangedHandler(uctlAnalysisView_StatChanged);
            uctlAnalysisView.StatDeleted += new StatDeletedHandler(uctlAnalysisView_StatDeleted);
            uctlAnalysisView.ImageClicked += new ImageClickedHandler(uctlAnalysisView_ImageClicked);
            uctlAnalysisView.ResultsRemoved += new ResultObjsRemovedHandler(uctlAnalysisView_ResultsRemoved);
            uctlGraphView.ImageClicked += new ImageClickedHandler(uctlGraphView_ImageClicked);
            uctlGraphView.ResultsRemoved += new ResultObjsRemovedHandler(uctlGraphView_ResultsRemoved);          
        }

        void uctlGraphView_ResultsRemoved(string[] imagePath)
        {
            uctlAnalysisView.RemoveAnalysis(imagePath);
        }

        void uctlAnalysisView_ResultsRemoved(string[] imagePath)
        {
            uctlGraphView.RemoveGraphs(imagePath);
        }


        void uctlAnalysisView_StatChanged(StatInformation statInfo)
        {
            //////////////
        }

        void uctlAnalysisView_StatDeleted(StatInformation statInfo)
        {
            try
            {
                foreach (Analysis analysis in oWorkSheet.lstAnalysis)
                {
                    if (analysis.AnalysisData is StatInformation && analysis.AnalysisData == statInfo)
                    {
                        oWorkSheet.RemoveAnalysis(analysis);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        void uctlAnalysisView_ImageClicked(string imagePath)
        {
            if(uctlGraphView.ActivateGraph(imagePath))
            {
                uctlDataView.Visible = false;
                uctlAnalysisView.Visible = false;
                uctlGraphView.Visible = true;
            }

        }

        void uctlGraphView_ImageClicked(string imagePath)
        {
            if (uctlAnalysisView.ActivateStat(imagePath))
            {
                uctlDataView.Visible = false;
                uctlAnalysisView.Visible = true;
                uctlGraphView.Visible = false;
            }
        }

        private void frmWorkSheet_Activated(object sender, EventArgs e)
        {
            uctlDataView.Visible = true;
            uctlAnalysisView.Visible = false;
            uctlGraphView.Visible = false;
        }

        private void btnDataView_Click(object sender, EventArgs e)
        {
            uctlDataView.Visible = true;
            uctlAnalysisView.Visible = false;
            uctlGraphView.Visible = false;
        }

        private void btnAnalysisView_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("You need a license.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            uctlDataView.Visible = false;
            uctlAnalysisView.Visible = true;
            uctlGraphView.Visible = false;
        }

        private void btnGraphView_Click(object sender, EventArgs e)
        {
            uctlDataView.Visible = false;
            uctlAnalysisView.Visible = false;
            uctlGraphView.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void uctlGraphView_GraphUpdating(GraphInformation graphInfo)
        {
            GraphAnalysisManager.UpdateGraphInformation(graphInfo);
        }

        void uctlGraphView_GraphDeleted(GraphInformation graphInfo)
        {
            try
            {
                foreach (Analysis analysis in oWorkSheet.lstAnalysis)
                {
                    if (analysis.AnalysisData is GraphInformation && analysis.AnalysisData == graphInfo)
                    {
                        oWorkSheet.RemoveAnalysis(analysis);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            	throw(ex);
            }
        }

        private void uctlDataView_NewDataSourceSplited(string name, DataTable dt)
        {
            if (OnNewDataSourceAdded != null)
                OnNewDataSourceAdded(name, dt);
        }

        #endregion
    }
}