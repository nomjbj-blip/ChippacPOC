using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

using DACrux.BStats;
using DACrux.ProjectManager.UI;
using DACrux.BStats.StatisticsInput;
//using DACrux.ProjectManager.Database;

namespace DACrux.ProjectManager
{
    /// <summary>
    /// 클래스 명 : ProjectManager <br/>
    /// 클래스요약: 프로젝트의 데이터와 결과들을 관리한다. <br/>
    /// 작  성  자: QMS 양형석<br/>
    /// 최초작성일: 2009-02-24<br/>
    /// 상세  설명: 프로젝트의 데이터와 결과들을 관리한다. <br/>
    /// 변경  내용: <br/>
    /// </summary>
    public sealed partial class ProjectManager : UserControl
    {
        #region " MEMBER FIELD "

        private static Form frmMainForm = null;

        private static Project oProject = null;
        private static WorkSheet oActiveWorkSheet = null;
        private static Analysis oActiveAnalysis = null;
        private static string strClickedMenu = string.Empty;

        private static string strLoggedUser = "Administrator";
        private static string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // string.Empty;

        private bool isInitiated = false;

        #endregion

        #region " CREATOR "

        public ProjectManager()
        {
            try
            {
                InitializeComponent();
                DeleteMenuOptionFile();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProjectManager_Load(object sender, EventArgs e)
        {    
            Initiate();
        }

        public void Initiate()
        {
            if (isInitiated)
                return;

            frmMainForm = this.ParentForm;

            if (!frmMainForm.IsMdiContainer)
                frmMainForm.IsMdiContainer = true;

            frmMainForm.MdiChildActivate += new EventHandler(frmMainForm_MdiChildActivate);
            frmMainForm.Disposed += new EventHandler(frmMainForm_Disposed);

            Project.OnProjectCreated += new ProjectCreatedHandler(OnProjectCreated);
            Project.OnProjectRenamed += new ProjectRenamedHandler(OnProjectRenamed);

            Project.OnWorkSheetAdded += new WorkSheetAddedHandler(OnWorkSheetAdded);
            Project.OnWorkSheetRemoved += new WorkSheetRemovedHandler(OnWorkSheetRemoved);

            SetupTempFolder(true);

            isInitiated = true;
        }
                
        #endregion

        #region " PROPERTY "

        internal WorkSheet ActiveWorkSheet
        {
            get { return oActiveWorkSheet; }
            set
            {
                oActiveWorkSheet = value;
            }
        }

        [Browsable(false)]
        internal Analysis ActiveAnalysis
        {
            get { return oActiveAnalysis; }
            set
            {
                oActiveAnalysis = value;
            }
        }

        public int DefaultDataSheetRowCount
        {
            get
            {
                return DACrux.ProjectManager.UI.DataView.DefaultRowCount;
            }
            set
            {
                DACrux.ProjectManager.UI.DataView.DefaultRowCount = value;
            }
        }

        public int DefaultDataSheetColumnCount
        {
            get
            {
                return DACrux.ProjectManager.UI.DataView.DefaultColumnCount;
            }
            set
            {
                DACrux.ProjectManager.UI.DataView.DefaultColumnCount = value;
            }
        }

        public MenuType ClickedMenu
        {
            set
            {
                ProcessClickedMenu(((MenuType)value).ToString());
            }
        }

        public static string SavePath
        {
            get { return ProjectManager.savePath; }
            set { ProjectManager.savePath = value; }
        }

        public static string LoggedUser
        {
            get { return ProjectManager.strLoggedUser; }
            set { ProjectManager.strLoggedUser = value; }
        }

        #endregion

        #region " METHOD "

        #region " Context Menu "

        private void cmsProject_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ProcessClickedMenu(e.ClickedItem.Name);
        }

        private void cmsWorkSheet_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ProcessClickedMenu(e.ClickedItem.Name);
        }

        private void cmsAnalysis_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ProcessClickedMenu(e.ClickedItem.Name);
        }

        private void tstMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ProcessClickedMenu(e.ClickedItem.Name);
        }

        private void ProcessClickedMenu(string clickedMenu)
        {
            try
            {
                if (!isInitiated)
                    Initiate();

                //////////////////////////////////////////////////////////////////////////
                // case "tsbXXX" : From ProjectManager Toolbar
                // case "cmiXXX" : From ContextMenu
                //////////////////////////////////////////////////////////////////////////

                Cursor = Cursors.WaitCursor;

                switch (clickedMenu)
                {
                    #region " Project "

                    case "FILE_NEW_PROJECT":
                    case "tsbNewProject":
                        {
                            #region Process new project
                            if (oProject == null)
                            {
                                NewProject();
                            }
                            else
                            {
                                switch (MessageBox.Show("Do you want save this project before closing?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
                                {
                                    case DialogResult.Yes:
                                        {
                                            SaveProject();
                                            CloseProject();
                                            NewProject();
                                        }
                                        break;
                                    case DialogResult.No:
                                        {
                                            CloseProject();
                                            NewProject();
                                        }
                                        break;
                                }
                            }
                            #endregion
                        }
                        break;

                    case "FILE_OPEN_PROJECT":
                    case "tsbOpenProject":
                        OpenProject();
                        break;

                    case "FILE_SAVE_PROJECT":
                        SaveProject();
                        break;

                    case "FILE_SAVE_PROJECT_AS":
                    case "cmiSaveProjectAs":
                        SaveProjectAs();
                        break;

                    case "FILE_RENAME_PROJECT":
                    case "cmiRenameProject":
                        tvExplorer.TopNode.BeginEdit();
                        break;

                    case "FILE_REMOVE_PROJECT":
                    case "cmiRemoveProject":
                        RemoveProject();
                        break;

                    case "FILE_CLOSE_PROJECT":
                    case "cmiCloseProject":
                        CloseProject();
                        break;

                    #endregion

                    #region " WorkSheet "

                    case "FILE_NEW_WORKSHEET":
                    case "tsbNewWorkSheet":
                    case "cmiAddWorkSheet":
                        NewWorkSheet();
                        break;

                    case "FILE_IMPORT_FILE":
                    case "tsbImportDataFile":
                    case "cmiImportDataFile":
                        ImportDataFile();
                        break;

                    case "FILE_IMPORT_DATABASE_SOURCE":
                        ImportDatabaseSource();
                        break;

                    case "FILE_RENAME_WORKSHEET":
                    case "cmiRenameWorkSheet":
                        tvExplorer.SelectedNode.BeginEdit();
                        break;

                    case "FILE_EXPORT_WORKSHEET":
                        ExportWorkSheet();
                        break;

                    case "FILE_REMOVE_WORKSHEET":
                    case "cmiRemoveWorkSheet":
                        RemoveWorkSheet();
                        break;

                    case "FILE_CLOSE_WORKSHEET":
                    case "cmiCloseWorkSheet":
                        CloseWorkSheet();
                        break;

                    case "FILE_EXIT":
                        break;

                    #endregion

                    #region " Data Manipulation "

                    case "DATA_COLUMN_SETTING":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiColumnSetting");
                        break;

                    case "DATA_INSERT":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiInsert");
                        break;

                    case "DATA_REMOVE":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiRemove");
                        break;

                    case "DATA_MOVE_COLUMN":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiMoveColumn");
                        break;

                    case "DATA_SORT":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiSort");
                        break;

                    case "DATA_SPLIT":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiSplit");
                        break;

                    case "DATA_SPLIT_SELECTED":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiSplitSelected");
                        break;

                    case "DATA_TRANSPOSE":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiTranspose");
                        break;

                    case "DATA_EXPORT_TO_EXCEL":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiExportToExcel");
                        break;

                    case "DATA_EXPORT_SELECTION_TO_EXCEL":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiExportSelectionToExcel");
                        break;

                    case "DATA_IMPORT_EXCEL":
                        if (oActiveWorkSheet != null)
                            oActiveWorkSheet.Form.ProcessDataViewContextMenu("cmiImportExcel");
                        break;

                    #endregion

                    #region " Statistical Analysis "

                    case "STAT_DESC_ANALYSIS":
                        NewStatAnalysis(StatType.DescriptiveAnalysis);
                        //MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_HYPOTHESIS":
                        NewStatAnalysis(StatType.HypothesisTesting);
                        break;

                    case "STAT_CORRELATION":
                        NewStatAnalysis(StatType.CorrelationAnalysis);
                        //MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_REGRESSION":
                        NewStatAnalysis(StatType.RegressionAnalysis);
                        //MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_ANOVA":
                        NewStatAnalysis(StatType.ANOVA);
                        //MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_CCONTINUOUS":
                        NewStatAnalysis(StatType.CpCpkAnalysis_Continuous);
                        //MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_ATTRIBUTE":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_DATA_DESIGN":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_GAGE_RR":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_RUN_CHART":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_LIN_ACC":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    //case "STAT_FACTOR_DESIGN":
                    //    NewStatDesign(StatType.DOE_FactorialDesign);
                    //    break;

                    case "STAT_FACTOR_ANALYSIS":
                        NewStatAnalysis(StatType.DOE_FactorialAnalysis);
                        break;


                    case "STAT_TAGUCHI_DESIGN":
                        NewStatDesign(StatType.DOE_TaguchiDesign);
                        break;

                    case "STAT_TAGUCHI_ANALYSIS":
                        NewStatAnalysis(StatType.DOE_TaguchiAnalysis);
                        break;


                    case "STAT_ORTHO_DESIGN":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_ORTHO_ANALYSIS":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_1_SAMPLE_Z":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_1_SAMPLE_T":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_1_PROPOSITION":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "STAT_2_PROPOSITION":
                        MessageBox.Show("You need license to perform this fuction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    #endregion

                    #region " Graph Analysis "

                    case "GRAPH_PIE":
                        NewGraphAnalysis(GraphType.Pie);
                        break;

                    case "GRAPH_BAR":
                        NewGraphAnalysis(GraphType.Bar);
                        break;

                    case "GRAPH_SCATTER":
                        NewGraphAnalysis(GraphType.Scatter);
                        break;

                    case "GRAPH_LINE":
                        NewGraphAnalysis(GraphType.Line);
                        break;

                    case "GRAPH_BOXPLOT":
                        NewGraphAnalysis(GraphType.BoxPlot);
                        break;

                    case "GRAPH_PARETO":
                        NewGraphAnalysis(GraphType.Pareto);
                        break;

                    case "GRAPH_HISTOGRAM":
                        NewGraphAnalysis(GraphType.Histogram);
                        break;

                    #endregion

                    #region " ProjectManager Toolbar "

                    case "tsbExpand":
                        foreach (TreeNode node in tvExplorer.Nodes)
                            node.ExpandAll();
                        break;


                    case "tsbCollapse":
                        foreach (TreeNode node in tvExplorer.Nodes)
                            node.Collapse();
                        break;

                    #endregion

                    #region " ProjectManager TreeView ContextMenu "

                    case "cmiExpandProject":
                    case "cmiExpandWorkSheet":
                        tvExplorer.SelectedNode.ExpandAll();
                        break;

                    case "cmiCollapseProject":
                    case "cmiCollapseWorkSheet":
                        tvExplorer.SelectedNode.Collapse();
                        break;

                    #endregion

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion



        private void NewStatDesign(StatType statAnalysisType)
        {

            try
            {
                Cursor = Cursors.WaitCursor;

                //if (oActiveWorkSheet == null)
                //    throw (new Exception("No Active WorkSheet exists."));

                switch (statAnalysisType)
                {
                    case StatType.DOE_FactorialDesign:
                        //DACrux.BStats.StatDialog.DlgFactorialDesign dlg = new BStats.StatDialog.DlgFactorialDesign();
                        //if (dlg.ShowDialog() == DialogResult.OK)
                        //{
                        //    NewWorkSheet(string.Format("DOE_{0}", DateTime.Now.ToString("MMddmmss")), dlg.FactorSheet,DataSourceType.External);
                        //}
                        break;
                    case StatType.DOE_TaguchiDesign:
                        DACrux.BStats.StatDialog.DlgTaguchi dlgTaguchi = new BStats.StatDialog.DlgTaguchi();
                        if (dlgTaguchi.ShowDialog() == DialogResult.OK)
                        {
                            FactorInfo[] oFactorInfos = dlgTaguchi.FactorIngos;
                            NewWorkSheet(string.Format("DOE_{0}", DateTime.Now.ToString("MMddmmss")), dlgTaguchi.TaguchiSheet, DataSourceType.External, oFactorInfos);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message == "MIRACOM")
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        #region " Context Menu Handler "

        private void NewProject()
        {
            try
            {
                oProject = Project.GetInstance();

                ActiveWorkSheet = null;
                ActiveAnalysis = null;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: ProjectManager.NewProject()"));
            }
        }

        private void NewProject(string name)
        {
            try
            {
                oProject = Project.GetInstance(name);

                ActiveWorkSheet = null;
                ActiveAnalysis = null;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: ProjectManager.NewProject(name)"));
            }
        }

        private void NewWorkSheet()
        {
            NewWorkSheet(string.Empty);
        }

        private void NewWorkSheet(string name)
        {
            NewWorkSheet(name, null);
        }

        public void NewWorkSheet(DataTable dataSource)
        {
            NewWorkSheet(string.Empty, dataSource);
        }

        public void NewWorkSheet(string name, DataTable dataSource)
        {
            NewWorkSheet(name, dataSource, DataSourceType.Internal);
        }

        public void NewWorkSheet(string name, DataTable dataSource, DataSourceType dataSourceType, FactorInfo[] TaguchiInfos = null)
        {
            WorkSheet workSheet;

            try
            {
                if (oProject == null)
                    oProject = Project.GetInstance();

                if (dataSource != null && dataSource.Rows.Count > 0)
                {
                    dataSource = DACrux.ProjectManager.UI.Common.ConvertToDataViewCompatibleDataTable(dataSource);

                    if (dataSourceType == DataSourceType.External)
                    {
                        dataSource.Rows.InsertAt(dataSource.NewRow(), 0);

                        for (int i = 0; i < dataSource.Columns.Count; i++)
                            dataSource.Rows[0][i] = dataSource.Columns[i].ColumnName;
                    }
                }

                workSheet = new WorkSheet(name, dataSource);
                oProject.AddWorkSheet(workSheet);

                // Taguchi를 위한 Factor 저장
                workSheet.TaguchiInfos = TaguchiInfos;

                workSheet.WorkSheetRenamed += new WorkSheetRenamedHandler(OnWorkSheetRenamed);
                workSheet.AnalysisAdded += new AnalysisAddedHandler(OnAnalysisAdded);
                workSheet.OnAnalysisRemoved += new AnalysisRemovedHandler(OnAnalysisRemoved);

                workSheet.OnNewDataSourceAdded += new NewDataSourceAddedHandler(NewWorkSheet);

                ActiveWorkSheet = workSheet;
                ActiveAnalysis = null;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message == "MIRACOM")
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw (new Exception(ex.Message + " \r\n\t: ProjectManager.NewWorkSheet()"));
                }

            }
        }

        private void NewAnalysis(Analysis analysis)
        {
            try
            {
                oActiveWorkSheet.AddAnalysis(analysis, true);
                ActiveAnalysis = analysis;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message == "MIRACOM")
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw ex;
                }
            }
        }

        private void NewStatAnalysis(StatType statAnalysisType)
        {
            Analysis analysis;
            Analysis graphAnalysis;
            DataTable dtDataSource;
            StatInformation statInfo;
            List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumn;
            string strName;
            string strProject;
            string strWorkSheet;

            try
            {
                Cursor = Cursors.WaitCursor;

                if (oActiveWorkSheet == null)
                    throw (new Exception("No Active WorkSheet exists."));

                dtDataSource = oActiveWorkSheet.StatDataSource;
                if (dtDataSource == null || dtDataSource.Rows.Count < 1)
                    throw (new Exception("No Data exists."));

                lstValidColumn = oActiveWorkSheet.CopiedValidColumnInfo;

                strName = StatAnalysis.GetNewInstanceName(statAnalysisType);

                strProject = oProject.Name;
                strWorkSheet = oActiveWorkSheet.Name;

                statInfo = StatAnalysisManager.GetStatAnalysis(statAnalysisType, dtDataSource, lstValidColumn, strProject, strWorkSheet, strName, strLoggedUser, oActiveWorkSheet.TaguchiInfos);

                if (statInfo != null)
                {
                    analysis = new StatAnalysis(oActiveWorkSheet, statInfo);

                    if (statInfo.GraphInformations != null)
                    {                 
                        foreach (GraphInformation graphInfo in statInfo.GraphInformations)
                        {
                            if (graphInfo.Type != GraphType.Line4Taguchi)
                            {
                                for (int i = 0; i < graphInfo.DataSource.Rows.Count; i++)
                                {
                                    foreach (DataColumn column in graphInfo.DataSource.Columns)
                                    {
                                        if (graphInfo.DataSource.Rows[i].IsNull(column) == true)
                                        {
                                            int nullValue = Convert.ToInt32(graphInfo.DataSource.Rows[i - 1][column]);
                                            graphInfo.DataSource.Rows[i][column] = nullValue;
                                        }
                                    }
                                }

                            }

                            if (graphInfo != null)
                            {
                                graphAnalysis = new GraphAnalysis(oActiveWorkSheet, graphInfo);

                                oActiveWorkSheet.AddAnalysis(graphAnalysis, false);
                            }
                        }
                    }

                    oActiveWorkSheet.AddAnalysis(analysis, true);
                    ActiveAnalysis = analysis;

                    ActivateWorkSheet(oActiveWorkSheet);
                    oActiveWorkSheet.Form.SetStatView();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message == "MIRACOM")
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void NewGraphAnalysis(GraphType graphAnalysisType)
        {
            Analysis analysis;
            DataTable dtDataSource;
            List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumn;
            GraphInformation graphInfo;

            try
            {
                if (oActiveWorkSheet == null)
                    throw (new Exception("No Active WorkSheet exists."));

                dtDataSource = oActiveWorkSheet.DataSource;
                if (dtDataSource == null || dtDataSource.Rows.Count < 1)
                    throw (new Exception("No Data exists."));

                lstValidColumn = oActiveWorkSheet.CopiedValidColumnInfo;
                if (lstValidColumn.Count < 1)
                    throw (new Exception("No Valid Data exists."));

                graphInfo = GraphAnalysisManager.GetGraphInformation(graphAnalysisType, dtDataSource, lstValidColumn);
                if (graphInfo != null)
                {
                    analysis = new GraphAnalysis(oActiveWorkSheet, graphInfo);

                    oActiveWorkSheet.AddAnalysis(analysis, true);
                    ActiveAnalysis = analysis;

                    ActivateWorkSheet(oActiveWorkSheet);
                    oActiveWorkSheet.Form.SetGraphView();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message == "MIRACOM")
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw ex;
                }
            }
        }

        private void OpenProject()
        {
            OpenFileDialog dlgOpenProject = null;

            try
            {
                dlgOpenProject = new OpenFileDialog();
                dlgOpenProject.InitialDirectory = savePath;
                dlgOpenProject.AddExtension = true;
                dlgOpenProject.Filter = "DACrux File (*.dpo)|*.dpo";

                if (dlgOpenProject.ShowDialog() == DialogResult.OK)
                {
                    if (oProject != null)
                        CloseProject();

                    ProcessOpenProject(new FileInfo(dlgOpenProject.FileName));
                }
            }
            catch
            {
                //ProcessCloseProject();
                MessageBox.Show("An error occured while opening the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dlgOpenProject != null)
                    dlgOpenProject.Dispose();
            }
        }

        private void ImportDataFile()
        {
            OpenFileDialog dlgImportDataFile = null;
            string extention = string.Empty;
            DataTable dataSource = null;

            try
            {
                dlgImportDataFile = new OpenFileDialog();
                dlgImportDataFile.InitialDirectory = savePath;
                dlgImportDataFile.DefaultExt = "xls";
                dlgImportDataFile.Filter = "Microsoft Excel File (*.xls)|*.xls|XML File (*.xml)|*.xml|CSV File (*.csv)|*.csv";

                if (dlgImportDataFile.ShowDialog() == DialogResult.OK)
                {
                    extention = Path.GetExtension(dlgImportDataFile.FileName).ToLower();

                    switch (extention)
                    {
                        case ".xls":
                            //dataSource = getDataFromXLS(dlgImportDataFile.FileName, " ");
                            dataSource = LoadExcel(dlgImportDataFile.FileName, true);
                            break;
                        case ".csv":
                            dataSource = LoadCSV(dlgImportDataFile.FileName, true);
                            break;
                        case ".xml":
                            dataSource = LoadXML(dlgImportDataFile.FileName);
                            break;
                    }
                    NewWorkSheet(dlgImportDataFile.FileName, dataSource, DataSourceType.External);

                    MessageBox.Show("Import successed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (dlgImportDataFile != null)
                    dlgImportDataFile.Dispose();
            }
        }

        private void ImportDatabaseSource()
        {
            //DataTable dt = null;
            //DlgImportDatabaseSource dlg;

            //try
            //{
            //    dlg = new DlgImportDatabaseSource();

            //    if (dlg.ShowDialog() == DialogResult.OK)
            //    {
            //        dt = dlg.ResultDatatable;
            //    }

            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        NewWorkSheet("", dt, DataSourceType.External);
            //        //dt = DACrux.ProjectManager.UI.Common.ConvertToDataViewCompatibleDataTable(dt);

            //        //NewWorkSheet("", dt);

            //        MessageBox.Show("Import successed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void SaveProject()
        {
            if (oProject == null)
            {
                MessageBox.Show("No Project exist.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!oProject.Saved)
            {
                SaveProjectAs();
            }
            else
            {
                ProcessSaveProject();
            }
        }

        private void SaveProjectAs()
        {
            SaveFileDialog dlgSaveProject = null;

            try
            {
                dlgSaveProject = new SaveFileDialog();
                dlgSaveProject.InitialDirectory = savePath;
                dlgSaveProject.AddExtension = true;
                dlgSaveProject.Filter = "DACrux File (*.dpo)|*.dpo";

                if (dlgSaveProject.ShowDialog() == DialogResult.OK)
                    ProcessSaveProject(new FileInfo(dlgSaveProject.FileName));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (dlgSaveProject != null)
                {
                    dlgSaveProject.Dispose();
                    dlgSaveProject = null;
                }
            }
        }

        private void ExportWorkSheet()
        {
            SaveFileDialog dlgExport = null;
            string extension = string.Empty;
            DataTable dataSource = null;

            try
            {
                if (oActiveWorkSheet == null)
                    return;

                dataSource = oActiveWorkSheet.StatDataSource;

                if (dataSource == null || dataSource.Rows.Count < 1)
                    return;

                dlgExport = new SaveFileDialog();
                dlgExport.DefaultExt = "xls";
                dlgExport.FileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), oActiveWorkSheet.Name);
                dlgExport.InitialDirectory = savePath;
                dlgExport.Filter = "Microsoft Excel File (*.xls)|*.xls|XML File (*.xml)|*.xml|CSV File (*.csv)|*.csv";

                if (dlgExport.ShowDialog() == DialogResult.OK)
                {
                    extension = Path.GetExtension(dlgExport.FileName).ToLower();

                    switch (extension)
                    {
                        case ".xls":
                            SaveExcel(dlgExport.FileName, dataSource);
                            break;
                        case ".csv":
                            SaveCSV(dlgExport.FileName, dataSource);
                            break;
                        case ".xml":
                            SaveXML(dlgExport.FileName, dataSource);
                            break;
                    }
                    
                    MessageBox.Show("Export successed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (dlgExport != null)
                    dlgExport.Dispose();
            }
        }

        private void CloseProject()
        {
            if (oProject == null)
                return;

            switch (MessageBox.Show("Do you want save current project?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
            {
                case DialogResult.Yes:
                    SaveProject();
                    ProcessCloseProject();
                    break;
                case DialogResult.No:
                    ProcessCloseProject();
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }

        private void CloseWorkSheet()
        {
            if (oActiveWorkSheet != null && !oActiveWorkSheet.Form.IsDisposed)
                oActiveWorkSheet.Form.Hide();
        }

        private void RemoveProject()
        {
            FileInfo fi;

            try
            {
                if (oProject == null)
                    return;

                if (oProject.FilePath.Trim() == string.Empty)
                {
                    CloseProject();
                    return;
                }

                fi = new FileInfo(oProject.FilePath);

                if (MessageBox.Show("Do you want remove current project permanently?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (fi.Exists)
                    {
                        fi.Delete();
                        ProcessCloseProject();
                    }
                    else
                    {
                        MessageBox.Show("Project file doesn't exist. Please check the file was removed or deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error occured during deleting current project. Please check file access authority.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RemoveWorkSheet()
        {
            try
            {
                if (oActiveWorkSheet == null)
                    return;

                if (MessageBox.Show("Do you want remove current WorkSheet permanently?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    oProject.RemoveWorkSheet(oActiveWorkSheet);
                }

                if (frmMainForm.ActiveMdiChild is frmWorkSheet)
                    oActiveWorkSheet = ((frmWorkSheet)frmMainForm.ActiveMdiChild).WorkSheet;
            }
            catch
            {
                MessageBox.Show("An error occured during removing current WorkSheet.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region " IO "

        private void ActivateWorkSheet(WorkSheet workSheet)
        {
            if (workSheet != null && workSheet.Form != null || frmMainForm.ActiveMdiChild != workSheet.Form)
            {
                try
                {
                    foreach (Form frm in frmMainForm.MdiChildren)
                    {
                        if (frm == workSheet.Form)
                        {
                            frm.Activate();
                            frm.Visible = true;
                            frm.Show();
                            return;
                        }
                    }

                    if (!workSheet.Form.IsDisposed)
                    {
                        workSheet.Form.MdiParent = frmMainForm;
                        workSheet.Form.Activate();
                        workSheet.Form.WindowState = FormWindowState.Maximized;
                        workSheet.Form.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show((ex.Message));
                }
            }
        }

        private void ProcessSaveProject()
        {
            if (oProject.FilePath == null || oProject.FilePath.Trim() == string.Empty)
                oProject.FilePath = savePath + @"\" + oProject.Name + ".dpo";

            FileInfo fi = new FileInfo(oProject.FilePath);

            ProcessSaveProject(fi);
        }

        private void ProcessSaveProject(FileInfo saveFileInfo)
        {
            XmlTextWriter xtw = null;
            string strEmpty = "NULL";
            byte[] arrByte = null;
            //object obj = null;

            try
            {
                xtw = new XmlTextWriter(saveFileInfo.FullName, Encoding.UTF8);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 5;

                xtw.WriteStartElement("PROJECT");
                xtw.WriteAttributeString("Name", oProject.Name);

                #region " Active Object Information "
                xtw.WriteStartElement("ACTIVE_OBJECT");

                if (oActiveWorkSheet != null)
                    xtw.WriteAttributeString("WORKSHEET", oActiveWorkSheet.Name);
                else
                    xtw.WriteAttributeString("WORKSHEET", strEmpty);

                if (oActiveAnalysis != null)
                    xtw.WriteAttributeString("ANALYSIS", oActiveAnalysis.Name);
                else
                    xtw.WriteAttributeString("ANALYSIS", strEmpty);

                xtw.WriteEndElement();
                #endregion

                #region " WorkSheet "
                foreach (WorkSheet workSheet in oProject.lstWorkSheet)
                {
                    xtw.WriteStartElement("WORKSHEET");
                    xtw.WriteAttributeString("Name", workSheet.Name);

                    #region " DataSource "
                    arrByte = GetSerializedData(workSheet.PureDataSource);
                    xtw.WriteStartElement("DATA");
                    xtw.WriteAttributeString("Length", arrByte.Length.ToString());
                    xtw.WriteBinHex(arrByte, 0, arrByte.Length);
                    xtw.WriteEndElement();
                    #endregion

                    #region " Analysis "
                    for (int j = 0; j < workSheet.ItemCount; j++)
                    {
                        xtw.WriteStartElement("ANALYSIS");

                        xtw.WriteAttributeString("Name", workSheet.GetAnalysisAt(j).Name);
                        xtw.WriteAttributeString("Type", workSheet.GetAnalysisAt(j).ModelType.ToString());

                        if (workSheet.GetAnalysisAt(j).Saving())
                        {
                            arrByte = GetSerializedData(workSheet.GetAnalysisAt(j).AnalysisData);
                            workSheet.GetAnalysisAt(j).Saved();
                        }

                        xtw.WriteStartElement("DATA");
                        xtw.WriteAttributeString("Length", arrByte.Length.ToString());
                        xtw.WriteBinHex(arrByte, 0, arrByte.Length);
                        xtw.WriteEndElement();

                        xtw.WriteEndElement();
                    }
                    #endregion

                    xtw.WriteEndElement();
                }
                #endregion

                xtw.WriteEndElement();
                xtw.Flush();

                oProject.Saved = true;
                oProject.FilePath = saveFileInfo.FullName;

                MessageBox.Show("Save Project successed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (xtw != null)
                {
                    xtw.Close();
                    xtw = null;
                }

                arrByte = null;
            }
        }

        private void ProcessOpenProject(FileInfo fileInfo)
        {
            #region " Local Variable "

            XmlDocument xmlDoc = null;
            XmlNodeList workSheetNodes = null;
            XmlNodeList analysisNodes = null;
            XmlNode node = null;
            XmlTextReader xtr = null;

            string strName = string.Empty;
            string strType = string.Empty;
            string strData = string.Empty;
            DataTable dtValue = null;
            object objTemp = null;
            Byte[] arrByte = null;

            WorkSheet workSheet = null;
            Analysis analysis = null;

            string strGraphType = ModelType.GraphAnalysis.ToString();
            string strStatType = ModelType.StatAnalysis.ToString();

            GraphInformation graphInfo = null;
            StatInformation statInfo = null;

            int byteLength = 0;

            #endregion

            try
            {
                #region " XML Loading "

                xmlDoc = new XmlDocument();
                xmlDoc.Load(fileInfo.FullName);

                #endregion

                #region " Project Loading "

                node = xmlDoc.SelectSingleNode("PROJECT");
                if (node != null)
                {
                    strName = node.Attributes["Name"].Value;
                    NewProject(strName);
                    oProject.Saved = true;
                }

                #endregion

                #region " WorkSheet Loading "

                workSheetNodes = node.SelectNodes("WORKSHEET");
                foreach (XmlNode workSheetNode in workSheetNodes)
                {
                    #region " WorkSheet Name "

                    strName = workSheetNode.Attributes["Name"].Value;

                    #endregion

                    #region " WorkSheet DataSource "

                    if (int.TryParse(workSheetNode.SelectSingleNode("DATA").Attributes["Length"].Value, out byteLength))
                    {
                        arrByte = new byte[byteLength];
                        xtr = new XmlTextReader(workSheetNode.SelectSingleNode("DATA").OuterXml, XmlNodeType.Element, null);
                        xtr.MoveToContent();
                        xtr.ReadBinHex(arrByte, 0, arrByte.Length);

                        objTemp = GetDeserializedData(arrByte);

                        if (objTemp is DataTable)
                            dtValue = objTemp as DataTable;
                        else
                            dtValue = null;
                    }
                    else
                    {
                        dtValue = null;
                    }

                    #endregion

                    #region " WorkSheet Event Setting "

                    workSheet = new WorkSheet(strName, dtValue);
                    oProject.AddWorkSheet(workSheet);
                    workSheet.WorkSheetRenamed += new WorkSheetRenamedHandler(OnWorkSheetRenamed);
                    workSheet.AnalysisAdded += new AnalysisAddedHandler(OnAnalysisAdded);
                    workSheet.OnAnalysisRemoved += new AnalysisRemovedHandler(OnAnalysisRemoved);
                    workSheet.OnNewDataSourceAdded += new NewDataSourceAddedHandler(NewWorkSheet);
                    oActiveWorkSheet = workSheet;

                    #endregion

                    #region " Analysis Loading "

                    analysisNodes = workSheetNode.SelectNodes("ANALYSIS");
                    foreach (XmlNode analysisNode in analysisNodes)
                    {
                        #region " Analysis Name & Type "

                        strName = analysisNode.Attributes["Name"].Value;
                        strType = analysisNode.Attributes["Type"].Value;

                        #endregion

                        #region " Analysis Data & Registration"

                        if (int.TryParse(analysisNode.SelectSingleNode("DATA").Attributes["Length"].Value, out byteLength))
                        {
                            arrByte = new byte[byteLength];
                            xtr = new XmlTextReader(analysisNode.SelectSingleNode("DATA").OuterXml, XmlNodeType.Element, null);
                            xtr.MoveToContent();
                            xtr.ReadBinHex(arrByte, 0, arrByte.Length);

                            objTemp = GetDeserializedData(arrByte);

                            if (strType == strGraphType)
                            {
                                if (objTemp is GraphInformation)
                                    graphInfo = objTemp as GraphInformation;
                                else
                                    graphInfo = new GraphInformation();

                                analysis = new GraphAnalysis(workSheet, graphInfo);
                            }
                            else if (strType == strStatType)
                            {
                                statInfo = objTemp as StatInformation;
                                //if (objTemp is StatInformation)
                                //    statInfo = objTemp as StatInformation;
                                //else
                                //    statInfo = new StatInformation();

                                analysis = new StatAnalysis(workSheet, statInfo);
                            }

                            workSheet.AddAnalysis(analysis, false);
                        }

                        #endregion
                    }

                    #endregion
                }

                #endregion

                #region " Active Object Loading "

                strName = node.SelectSingleNode("ACTIVE_OBJECT").Attributes["WORKSHEET"].Value;

                if (strName != null && strName != "NULL")
                {
                    oActiveWorkSheet = oProject.GetWorkSheet(strName);
                    ActivateWorkSheet(oActiveWorkSheet);
                }

                strName = node.SelectSingleNode("ACTIVE_OBJECT").Attributes["ANALYSIS"].Value;

                if (strName != null && strName != "NULL")
                {
                    oActiveAnalysis = oActiveWorkSheet.GetAnalysis(strName);
                }

                if (oActiveWorkSheet != null && oActiveWorkSheet.Form != null)
                {
                    oActiveWorkSheet.Form.SetDefaultView();
                }

                #endregion

                #region " File Path "

                oProject.FilePath = fileInfo.FullName;

                #endregion
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message == "MIRACOM")
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }
                else
                {
                    if (ex.InnerException != null && ex.InnerException.Message == "MIRACOM")
                    {
                        MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        throw (ex);
                    }
                }
            }
            finally
            {
                if (xmlDoc != null)
                    xmlDoc = null;

                if (xtr != null)
                    xtr = null;
            }
        }

        private void ProcessCloseProject()
        {
            oProject.Close();
            oProject = null;

            oActiveWorkSheet = null;
            oActiveAnalysis = null;

            tvExplorer.Nodes.Clear();
        }

        private byte[] GetSerializedData(object source)
        {
            BinaryFormatter bf;
            MemoryStream ms = null;
            byte[] arrByte = null;

            try
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream();

                if (source == null)
                    source = "NULL";
                bf.Serialize(ms, source);
                arrByte = ms.ToArray();

                ms.Seek(0, 0);
                object temp = bf.Deserialize(ms);

                return arrByte;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                bf = null;

                if (ms != null)
                {
                    ms.Close();
                    ms = null;
                }
            }
        }

        private object GetDeserializedData(byte[] arrByte)
        {
            BinaryFormatter bf;
            MemoryStream ms = null;
            object objReturn = null;

            try
            {
                if (arrByte == null || arrByte.Length < 1)
                    return null;

                bf = new BinaryFormatter();
                ms = new MemoryStream();
                ms.Write(arrByte, 0, arrByte.Length);
                ms.Seek(0, 0);

                objReturn = bf.Deserialize(ms);

                return objReturn;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                bf = null;

                if (ms != null)
                {
                    ms.Close();
                    ms = null;
                }
            }
        }

        private static byte[] StrToByteArray(string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        private DataTable LoadCSV(string fileName, bool isColumnHeader)
        {
            string path = Path.GetDirectoryName(fileName);
            string file = Path.GetFileName(fileName);

            DataTable dt = new DataTable();
            string conString = null;

            try
            {
                if (isColumnHeader)
                    conString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + @";Extended Properties=""Text;HDR=Yes;FMT=Delimited""";
                else
                    conString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + @";Extended Properties=""Text;HDR=No;FMT=Delimited""";

                using (OleDbConnection conn = new OleDbConnection(conString))
                {
                    string sql = " SELECT * FROM " + file;
                    conn.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);

                    da.Fill(dt);
                    conn.Close();
                }
            }
            catch
            {
                MessageBox.Show("Invalid csv format.");
                return null;
            }

            return dt;
        }

        private DataTable LoadExcel(string fileName, bool isColumnHeader)
        {
            string connectString;
            string SQL;

            DataTable dt = null;
            DataTable schema = null;

            OleDbConnection oleDbConnection = null;
            OleDbDataAdapter oleDbAdapter = null;

            try
            {
                if (isColumnHeader)
                    connectString = GetExcelConnection(fileName, "HDR=Yes;");
                else
                    connectString = GetExcelConnection(fileName, "HDR=No;");

                oleDbConnection = new OleDbConnection(connectString);
                oleDbConnection.Open();
                schema = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                if (schema == null)
                    return null;

                SQL = "SELECT * FROM [" + schema.Rows[0]["Table_name"].ToString() + "]";
                oleDbAdapter = new OleDbDataAdapter(SQL, oleDbConnection);

                dt = new DataTable();
                oleDbAdapter.Fill(dt);
                oleDbConnection.Close();

                DataRow dr = dt.NewRow();
                string colName = string.Empty;
                DataColumn column = null;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colName = dt.Columns[i].ColumnName;
                    column = dt.Columns.Add("TEMP_" + colName, typeof(string));

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j][i] != null)
                        {
                            dt.Rows[j]["TEMP_" + colName] = dt.Rows[j][i].ToString();
                        }
                    }

                    dt.Columns.RemoveAt(i);
                    dt.Columns["TEMP_" + colName].ColumnName = colName;
                    dt.Columns[colName].SetOrdinal(i);
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dr[i] = dt.Columns[i].ColumnName.Replace("TEMP_", "");
                }

                //dt.Rows.InsertAt(dr, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid excel format.(" + ex.Message + ")");
                return null;
            }

            return dt;
        }

        private DataTable getDataFromXLS(string path, string blad)
        {
            try
            {
                string strConnectionString = string.Empty;
                strConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + path + ";" +
                "Extended Properties=" + (char)34 + "Excel 8.0;HDR=Yes;IMEX=1;" + (char)34;

                OleDbConnection cnCSV = new OleDbConnection(strConnectionString);
                cnCSV.Open();
                OleDbCommand cmdSelect = new OleDbCommand(@"SELECT * FROM [" + blad + "]", cnCSV);
                OleDbDataAdapter daCSV = new OleDbDataAdapter();
                daCSV.SelectCommand = cmdSelect;
                DataTable dtCSV = new DataTable();
                daCSV.Fill(dtCSV);
                cnCSV.Close();
                daCSV = null;
                return dtCSV;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private DataTable LoadXML(string fileName)
        {
            System.Data.DataTable dt = null;

            try
            {
                dt = new DataTable();
                dt.ReadXml(fileName);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        private void SaveCSV(string fileName, DataTable dtSource)
        {
            // Create the CSV file to which grid data will be exported.
            StreamWriter sw = new StreamWriter(fileName, false);

            int iColCount = dtSource.Columns.Count;

            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dtSource.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }

            sw.Write(sw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dtSource.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }

            sw.Close();
        }

        private static int iRowCount;

        public static void ExportDataTableToExcel(DataTable dataTable, Microsoft.Office.Interop.Excel.Worksheet sheetToAddTo)
        {
            object[,] columnNames;

            columnNames = new object[1, dataTable.Columns.Count];

            //add the columns names from the datatable
            for (int index = 0; index < dataTable.Columns.Count; index++)
            {
                columnNames[0, index] = dataTable.Columns[index].Caption;
            }

            //get a range object that the columns will be added to
            Microsoft.Office.Interop.Excel.Range columnsNamesRange = sheetToAddTo.get_Range(sheetToAddTo.Cells[1, 1], sheetToAddTo.Cells[1, dataTable.Columns.Count]);

            //a simple assignement allows the data to be transferred quickly
            columnsNamesRange.Value2 = columnNames;
            columnsNamesRange.EntireRow.Font.Bold = true;

            //release the columsn range object now it is finished with
            System.Runtime.InteropServices.Marshal.ReleaseComObject(columnsNamesRange);
            columnsNamesRange = null;

            //create the object to store the dataTable data
            object[,] rowData;

            rowData = new object[dataTable.Rows.Count, dataTable.Columns.Count];

            iRowCount = dataTable.Rows.Count;

            //insert the data into the object[,]
            for (int iRow = 0; iRow < dataTable.Rows.Count; iRow++)
            {
                for (int iCol = 0; iCol < dataTable.Columns.Count; iCol++)
                {
                    rowData[iRow, iCol] = dataTable.Rows[iRow][iCol];
                }
            }

            //get a range to add the table data into 
            //it is one row down to avoid the previously added columns
            Microsoft.Office.Interop.Excel.Range dataCells = sheetToAddTo.get_Range(sheetToAddTo.Cells[2, 1],

            sheetToAddTo.Cells[dataTable.Rows.Count + 1, dataTable.Columns.Count]);

            // for formating the columns before polulating the data
            short colIndex = 1;
            string colType = string.Empty;
            foreach (System.Data.DataColumn dcol in dataTable.Columns)
            {
                colType = string.Empty;

                if (dcol.DataType.Equals(typeof(string))) // if data type is string 
                    colType = "@";
                else if (dcol.DataType.Equals(typeof(DateTime))) // if data type is datetime
                    colType = "dd-MM-yyyy";

                if (!string.IsNullOrEmpty(colType))
                    FormatColumn(dataCells, colIndex, colType);

                ++colIndex;
            }

            //assign data to worksheet
            dataCells.Value2 = rowData;

            //release range
            System.Runtime.InteropServices.Marshal.ReleaseComObject(dataCells);

            dataCells = null;
        }

        public static void FormatColumn(Microsoft.Office.Interop.Excel.Range excelRange, int col, string format)
        {
            ((Microsoft.Office.Interop.Excel.Range)excelRange.Cells[1, col]).EntireColumn.NumberFormat = format;
        }

        private void SaveExcel(string fileName, DataTable dtSource)
        {
            //Create excel sheet object
            Microsoft.Office.Interop.Excel.Application xlApp = null;
            Microsoft.Office.Interop.Excel.Workbook xlBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xlSheet = null;
            Object MissingValue = System.Reflection.Missing.Value;
            try
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlBook = xlApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[1];

                //To suppress the save as alert
                xlApp.DisplayAlerts = false;
                xlApp.AlertBeforeOverwriting = false;
                
                xlBook.SaveAs(fileName, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);

                //Call above export method m_dtWarrantyDetails - this is datatable
                ExportDataTableToExcel(dtSource, xlSheet);

                xlSheet.Cells.EntireColumn.AutoFit();
                
                //Save excel file.
                xlSheet.SaveAs(fileName, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Release the objects
                xlApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                xlSheet = null;
                xlBook = null;
                xlApp = null;
                System.GC.Collect();
            }
        }

        private void SaveXML(string fileName, DataTable dtSource)
        {
            dtSource.WriteXml(fileName, XmlWriteMode.WriteSchema);
        }

        private string GetExcelConnection(string strExcelFilename, string strHDR)
        {
            return @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strExcelFilename + ";Extended Properties=" + (char)34 + "Excel 12.0 Xml;" + strHDR + (char)34;
            //return
            //    @"Provider=Microsoft.Jet.OLEDB.4.0;" +
            //    @"Data Source=" + strExcelFilename + ";" +
            //    @"Extended Properties=" + Convert.ToChar(34).ToString() +
            //    @"Excel 8.0;" + strHDR + Convert.ToChar(34).ToString();
        }

        private string GetElementValue(XmlTextReader tr, string ElementName)
        {
            string ReturnValue = null;

            while (tr.Read())
            {
                if (tr.NodeType == XmlNodeType.Element && tr.Name == ElementName)
                {
                    // Attribute가 있으면 첫번째 값을 반환하고 Return
                    if (tr.HasAttributes)
                    {
                        ReturnValue = tr.GetAttribute(0);
                        break;
                    }

                    tr.Read();
                    ReturnValue = tr.Value;
                    break;
                }
            }

            return ReturnValue;
        }

        private void SetupTempFolder(bool bCreate)
        {
            try
            {
                if (bCreate)
                    new DirectoryInfo(DACrux.ProjectManager.UI.Common.TempPath).Create();
                else
                    new DirectoryInfo(DACrux.ProjectManager.UI.Common.TempPath).Delete(true);
            }
            catch
            {
            }
        }

        private void DeleteMenuOptionFile()
        {
            try
            {
                MenuOption oOption = new MenuOption();
                oOption.DeleteXmlFile();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region " EVENT HANDLER "

        #region " Data Model "

        private void OnProjectCreated(Project project)
        {
            DrawTreeViewNode(project);
            // DrawSomeOtherView(project);
        }

        private void OnWorkSheetAdded(WorkSheet workSheet)
        {
            DrawTreeViewNode(workSheet);

            workSheet.Form.MdiParent = frmMainForm;
            workSheet.Form.WindowState = FormWindowState.Maximized;
            workSheet.Form.Show();
        }

        private void OnAnalysisAdded(Analysis analysis, bool bFocus)
        {
        }

        private void OnWorkSheetRemoved(WorkSheet workSheet)
        {
            if (tvExplorer.SelectedNode != null && tvExplorer.SelectedNode.Level > 0)
                tvExplorer.SelectedNode.Remove();
        }

        private void OnAnalysisRemoved(Analysis analysis)
        {

        }

        private void OnProjectRenamed(string name)
        {
            tvExplorer.TopNode.Name = name;
            tvExplorer.TopNode.Text = name;
        }

        private void OnWorkSheetRenamed(string name)
        {
            tvExplorer.SelectedNode.Name = name;
            tvExplorer.SelectedNode.Text = name;
        }

        private void OnAnalysisRenamed(string name)
        {
            //tvExplorer.SelectedNode.Name = name;
        }

        #endregion

        #region " Main Form "

        void frmMainForm_MdiChildActivate(object sender, EventArgs e)
        {
            Form activatedForm = frmMainForm.ActiveMdiChild;
            if (activatedForm is frmWorkSheet)
            {
                frmWorkSheet frm = activatedForm as frmWorkSheet;

                if (ActiveWorkSheet != frm.WorkSheet)
                    ActiveWorkSheet = frm.WorkSheet;
            }
        }

        void frmMainForm_Disposed(object sender, EventArgs e)
        {
            SetupTempFolder(false);
            DeleteMenuOptionFile();
        }

        #endregion

        #region " TreeView "

        private void DrawTreeViewNode(ITreeViewDrawable drawable)
        {
            if (drawable.ModelType == ModelType.Project)
            {
                DrawTreeViewNode(-1, drawable);
            }
            else
            {
                TreeNode tnParent = tvExplorer.Nodes.Find(drawable.Parent, true)[0];
                int iParentNodeIndex = tnParent.Index;

                DrawTreeViewNode(iParentNodeIndex, drawable);
            }
        }

        private void DrawTreeViewNode(int iParentNodeIndex, ITreeViewDrawable drawable)
        {
            TreeNode tnNode;
            TreeNode tnParent;

            try
            {
                tvExplorer.BeginUpdate();

                tnNode = new TreeNode(drawable.Name);
                tnNode.Name = drawable.Name;
                tnNode.Tag = drawable;

                switch (drawable.ModelType)
                {
                    case ModelType.Project:
                        tnNode.SelectedImageIndex = 0;
                        tnNode.ImageIndex = 1;
                        tnNode.ContextMenuStrip = cmsProject;
                        break;
                    case ModelType.WorkSheet:
                        tnNode.SelectedImageIndex = 2;
                        tnNode.ImageIndex = 3;
                        tnNode.ContextMenuStrip = cmsWorkSheet;
                        break;
                    case ModelType.GraphAnalysis:
                        tnNode.SelectedImageIndex = 4;
                        tnNode.ImageIndex = 5;
                        tnNode.ContextMenuStrip = cmsAnalysis;
                        break;
                    case ModelType.StatAnalysis:
                        tnNode.SelectedImageIndex = 6;
                        tnNode.ImageIndex = 7;
                        tnNode.ContextMenuStrip = cmsAnalysis;
                        break;
                    case ModelType.Log:
                        tnNode.SelectedImageIndex = 8;
                        tnNode.ImageIndex = 9;
                        tnNode.ContextMenuStrip = cmsAnalysis;
                        break;
                    case ModelType.None:
                    default:
                        break;
                }

                if (iParentNodeIndex < 0)
                {
                    iParentNodeIndex = tvExplorer.Nodes.Add(tnNode);
                    tnParent = tnNode;
                }
                else
                {
                    tnParent = tvExplorer.Nodes[iParentNodeIndex];
                    tnParent.Nodes.Add(tnNode);
                }

                for (int i = 0; i < drawable.ItemCount; i++)
                {
                    DrawTreeViewNode(iParentNodeIndex, drawable.GetItemAt(i));
                }

                tnParent.Expand();
                tvExplorer.SelectedNode = tnNode;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message == "MIRACOM")
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw (new Exception(ex.Message + " \r\n\t: ProjectManager.DrawTreeViewNode(int iParentNodeIndex, ITreeViewDrawable drawable)"));
                }
            }
            finally
            {
                tvExplorer.EndUpdate();
            }
        }

        private void tvExplorer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch ((e.Node.Tag as ITreeViewDrawable).ModelType)
            {
                case ModelType.Project:
                    ActiveWorkSheet = null;
                    ActiveAnalysis = null;
                    break;
                case ModelType.WorkSheet:
                    ActiveWorkSheet = e.Node.Tag as WorkSheet;
                    ActiveAnalysis = null;
                    break;
                case ModelType.GraphAnalysis:
                case ModelType.StatAnalysis:
                case ModelType.Log:
                    ActiveAnalysis = e.Node.Tag as Analysis;
                    ActiveWorkSheet = oActiveAnalysis.oWorkSheet;
                    break;
                default:
                    break;
            }

            tvExplorer.SelectedNode = e.Node;
        }

        private void tvExplorer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e.Node.Tag as ITreeViewDrawable).ModelType == ModelType.WorkSheet)
                ActivateWorkSheet(oActiveWorkSheet);
        }

        private void tvExplorer_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            ModelType nodeType;
            string strNewLabel;
            int nIndex;

            try
            {
                if (e.Label == null)
                    return;
                else if (e.Label.Trim() == string.Empty)
                {
                    MessageBox.Show("Name cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    e.CancelEdit = true;
                    e.Node.BeginEdit();

                    return;
                }
                else if (e.Label.IndexOfAny(new char[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|' }) > -1)
                {
                    MessageBox.Show("Name cannot include special characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    e.CancelEdit = true;
                    e.Node.BeginEdit();

                    return;
                }

                nodeType = (e.Node.Tag as ITreeViewDrawable).ModelType;
                strNewLabel = e.Label.ToString();
                nIndex = 0;

                switch (nodeType)
                {
                    case ModelType.Project:
                        nIndex = -1;
                        break;
                    case ModelType.WorkSheet:
                        nIndex = oProject.GetWorkSheetIndex(strNewLabel);
                        break;
                    case ModelType.GraphAnalysis:
                    case ModelType.StatAnalysis:
                    case ModelType.Log:
                        nIndex = oActiveWorkSheet.GetAnalysisIndex(strNewLabel);
                        break;
                    default:
                        break;
                }

                if (nIndex > -1)
                {
                    MessageBox.Show("Typed name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.CancelEdit = true;
                    e.Node.BeginEdit();
                }
                else
                {
                    (e.Node.Tag as ITreeViewDrawable).Name = strNewLabel;
                }

            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: ProjectManager.tvExplorer_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)"));
            }
        }

        private void tvExplorer_KeyDown(object sender, KeyEventArgs e)
        {
            string strCode = e.KeyCode.ToString();

            if (strCode == "F2")
            {
                tvExplorer.SelectedNode.BeginEdit();
            }
        }

        #endregion

        #endregion
    }
}
