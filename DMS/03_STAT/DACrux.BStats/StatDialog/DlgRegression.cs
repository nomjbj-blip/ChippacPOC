using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DACrux.ProjectManager.UI;
using DACrux.BStats.StatDialog.DlgRegression_;
using DACrux.BStats.Core;
using DACrux.BStats.StatisticsInput;

namespace DACrux.BStats.StatDialog
{

    public partial class DlgRegression : Form, iStatInformation
    {
        #region " MEMBER FIELD "

        public string FUNC_CODE = "F0303";

        #region [ LANGUAGE ]
        string m_Title = "Regression";
        string m_Response = "Response";
        string m_Predictors = "Predictors";
        string m_Fitintercept = "Fi&t intercept";
        string m_Columns = "Columns";

        string m_Methods = "&Methods";
        string m_Options = "O&ptions";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";


        //MESSAGE
        string m_ErrorOneColumn = "Please select only one column.";
        string m_ErrorResponse = "Invalid response variable. \r\n Too few items.";
        string m_ErrorPredictor = "Invalid predictor(s). \r\n Too few items.";

        #endregion

        #region [ EVENT ]
        public event DSelectedRegression On_SelectedRegression;
        #endregion


        //Column Double Click시에 삽입할 곳...
        private bool m_lvRespense = true;

        private inputRegression m_Input;

        private System.Collections.Hashtable htColumns = null;

        #endregion

        #region " CREATOR "

        public DlgRegression(DataTable dtSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo, string strProject, string strWorkSheet, string strTitle, string strResultFilePath, string strUser)
        {
            InitializeComponent();
            m_Input = new inputRegression(strTitle, strResultFilePath);
            m_Input.DataSource = dtSource;
            m_Input.Title = strTitle;
            m_Input.Project = strProject;
            m_Input.WorkSheet = strWorkSheet;
            m_Input.User = strUser;
            SetColumnListView(lstValidColumnInfo);
            InitDialog();
            SetSetting();
        }

        #endregion

        #region " PROPERTY "

        /// <summary>
        /// 
        /// </summary>
        public StatInformation StatInfo
        {
            get
            {
                return m_Input.StatInfo;
            }
        }

        public GraphInformation[] GraphInformations
        {
            get
            {
                return m_Input.GraphInformations;
            }
            set
            {
                m_Input.GraphInformations = value;
            }
        }

        #endregion

        #region " METHOD "

        #region [ Inner Methods ]

        private void InitDialog()
        {
            try
            {
                this.Text = m_Title;
                lblResponse.Text = m_Response;
                lblPredictors.Text = m_Predictors;
                lblColumns.Text = m_Columns;
                chkIntercept.Text = m_Fitintercept;

                btnCancel.Text = m_Cancel;
                btnOk.Text = m_Ok;
                btnMethods.Text = m_Methods;
                btnOptions.Text = m_Options;


                lvColumns.SmallImageList = imlColumnType;
                lvRespense.SmallImageList = imlColumnType;
                lvPredictors.SmallImageList = imlColumnType;

                lvColumns.AllowDrop = true;
                lvRespense.AllowDrop = true;
                lvPredictors.AllowDrop = true;

                lvColumns.MultiSelect = true;
                lvRespense.MultiSelect = true;
                lvPredictors.MultiSelect = true;


                btnOk.Click += new EventHandler(btnOk_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
                btnMethods.Click += new EventHandler(btnMethods_Click);
                btnOptions.Click += new EventHandler(btnOptions_Click);
                btnLeft_Predictors.Click += new EventHandler(btnLeft_Predictors_Click);
                btnRight_Predictors.Click += new EventHandler(btnRight_Predictors_Click);
                btnLeft_Response.Click += new EventHandler(btnLeft_Response_Click);
                btnRight_Response.Click += new EventHandler(btnRight_Response_Click);

                this.lvPredictors.Enter += new EventHandler(lvPredictors_Enter);
                this.lvRespense.Enter += new EventHandler(lvRespense_Enter);

                this.lvPredictors.MouseDoubleClick += new MouseEventHandler(lvPredictors_MouseDoubleClick);
                this.lvRespense.MouseDoubleClick += new MouseEventHandler(lvRespense_MouseDoubleClick);
                this.lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);

                this.lvPredictors.ItemDrag += new ItemDragEventHandler(lvPredictors_ItemDrag);
                this.lvRespense.ItemDrag += new ItemDragEventHandler(lvRespense_ItemDrag);
                this.lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);

                this.lvPredictors.DragEnter += new DragEventHandler(lvPredictors_DragEnter);
                this.lvRespense.DragEnter += new DragEventHandler(lvRespense_DragEnter);
                this.lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);

                this.lvPredictors.DragDrop += new DragEventHandler(lvPredictors_DragDrop);
                this.lvRespense.DragDrop += new DragEventHandler(lvRespense_DragDrop);
                this.lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetColumnListView(List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstColumnInfo)
        {
            ListViewItem lvItem;
            int iColCNT;
            try
            {
                iColCNT = lstColumnInfo.Count;
                htColumns = new System.Collections.Hashtable();
                for (int i = 0; i < iColCNT; i++)
                {
                    lvItem = new ListViewItem("");

                    if (lstColumnInfo[i].DataType == DataType.NUMBER)
                        lvItem.ImageKey = "NUMBER";
                    else if (lstColumnInfo[i].DataType == DataType.TEXT)
                        lvItem.ImageKey = "TEXT";
                    else if (lstColumnInfo[i].DataType == DataType.DATETIME)
                        lvItem.ImageKey = "DATETIME";

                    lvItem.SubItems.Add(lstColumnInfo[i].ColumnID);
                    lvItem.SubItems.Add(lstColumnInfo[i].ColumnName);
                    if (lvItem.ImageKey == "NUMBER")
                        lvColumns.Items.Add(lvItem);
                    htColumns.Add(lvItem, lstColumnInfo[i]);
                    htColumns.Add(lstColumnInfo[i], lvItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemoveVariable(ListViewItem Item)
        {
            int[] arrIndex = null;
            try
            {
                arrIndex = new int[] { ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[Item]).ColumnIndex };
                if (m_Input.arrGraphVariablesVsResiduals != null && m_Input.arrGraphVariablesVsResiduals.Length > 0)
                    m_Input.arrGraphVariablesVsResiduals = RemoveFactor(m_Input.arrGraphVariablesVsResiduals, arrIndex);

                if (m_Input.arrIncludePredictors != null && m_Input.arrIncludePredictors.Length > 0)
                    m_Input.arrIncludePredictors = RemoveFactor(m_Input.arrIncludePredictors, arrIndex);

                if (m_Input.arrPredictorsInitialModel != null && m_Input.arrPredictorsInitialModel.Length > 0)
                    m_Input.arrPredictorsInitialModel = RemoveFactor(m_Input.arrPredictorsInitialModel, arrIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemoveVariables(IList LvItems)
        {
            int[] arrIndex = null;
            try
            {
                arrIndex = GetIndex(LvItems);
                if (m_Input.arrGraphVariablesVsResiduals != null && m_Input.arrGraphVariablesVsResiduals.Length > 0)
                    m_Input.arrGraphVariablesVsResiduals = RemoveFactor(m_Input.arrGraphVariablesVsResiduals, arrIndex);

                if (m_Input.arrIncludePredictors != null && m_Input.arrIncludePredictors.Length > 0)
                    m_Input.arrIncludePredictors = RemoveFactor(m_Input.arrIncludePredictors, arrIndex);

                if (m_Input.arrPredictorsInitialModel != null && m_Input.arrPredictorsInitialModel.Length > 0)
                    m_Input.arrPredictorsInitialModel = RemoveFactor(m_Input.arrPredictorsInitialModel, arrIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int[] GetIndex(IList LvItems)
        {
            int[] arrReturn = null;
            int iCnt;
            try
            {
                iCnt = LvItems.Count;
                arrReturn = new int[iCnt];
                for (int i = 0; i < iCnt; i++)
                {
                    arrReturn[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[(ListViewItem)LvItems[i]]).ColumnIndex;
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int[] RemoveFactor(int[] OrigSource, int[] arrRemoveFactor)
        {
            List<int> arrReturn = null;
            try
            {
                arrReturn = new List<int>();
                foreach (int i in OrigSource)
                {
                    if (Array.IndexOf(arrRemoveFactor, i) < 0)
                        arrReturn.Add(i);
                }
                return arrReturn.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //List<int> arrReturn = null;
            //try
            //{
            //    arrReturn = new List<int>(OrigSource);
            //    foreach (int i in arrRemoveFactor)
            //    {
            //        arrReturn.Remove(i);
            //    }
            //    return arrReturn.ToArray();
            //}catch(Exception ex)
            //{
            //    throw ex;
            //}
        }

        private bool CheckSelectedVar()
        {
            try
            {
                if (lvRespense.Items.Count != 1)
                {
                    MessageBox.Show(m_ErrorResponse);
                    return false;
                }

                if (lvPredictors.Items.Count < 1)
                {
                    MessageBox.Show(m_ErrorPredictor);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TrimInput()
        {
            int iColCnt;
            int iVarCnt;  //독립변수의 개수
            int iMapping;
            int iRowCnt;
            int iMaxRow;

            #region 현재 선택된 컬럼 인덱스 리스트
            int[] arrVar = null;
            #endregion

            #region 정리된 DataTable에서 현재 선택된 컬럼 인덱스 리스트
            int[] arrMappingVar = null;
            #endregion

            #region 내부대화상자에서 선택한 변수가 있는지..
            bool bGraph = false;
            bool bInclude = false;
            bool bInitial = false;
            int[] arrTempGraph = null;
            int[] arrTempInclude = null;
            int[] arrTempInitial = null;

            #endregion

            try
            {

                iVarCnt = lvPredictors.Items.Count;

                #region Trim Valid Row

                iMaxRow = 0;
                for (int i = 0; i < iVarCnt; i++)
                {
                    if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvPredictors.Items[i]]).ValidRowIndex)
                        iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvPredictors.Items[i]]).ValidRowIndex;
                }
                if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvRespense.Items[0]]).ValidRowIndex)
                    iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvRespense.Items[0]]).ValidRowIndex;

                iRowCnt = m_Input.DataSource.Rows.Count;

                for (int i = iRowCnt - 1; i > -1; i--)
                {
                    if (i > iMaxRow)
                        m_Input.DataSource.Rows.RemoveAt(i);
                }
                m_Input.DataSource.AcceptChanges();

                #endregion

                arrVar = new int[iVarCnt + 1];
                arrVar[0] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvRespense.Items[0]]).ColumnIndex;
                for (int i = 0; i < iVarCnt; i++)
                {
                    arrVar[i + 1] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvPredictors.Items[i]]).ColumnIndex;
                }
                arrMappingVar = new int[iVarCnt + 1];

                #region 선택한 컬럼만 남기고 나머지 컬럼삭제, 선택 인덱스 목록 맵핑
                iColCnt = m_Input.DataSource.Columns.Count;
                iMapping = iVarCnt;

                #region 내부대화상자 맵핑 임시 배열
                if (m_Input.arrGraphVariablesVsResiduals != null && m_Input.arrGraphVariablesVsResiduals.Length > 0)
                {
                    bGraph = true;
                    arrTempGraph = new int[m_Input.arrGraphVariablesVsResiduals.Length];
                }
                if (m_Input.arrIncludePredictors != null && m_Input.arrIncludePredictors.Length > 0)
                {
                    bInclude = true;
                    arrTempInclude = new int[m_Input.arrIncludePredictors.Length];
                }
                if (m_Input.arrPredictorsInitialModel != null && m_Input.arrPredictorsInitialModel.Length > 0)
                {
                    bInitial = true;
                    arrTempInitial = new int[m_Input.arrPredictorsInitialModel.Length];
                }


                #endregion

                for (int i = iColCnt - 1; i > -1; i--)
                {
                    int iIndex = -1;
                    iIndex = Array.IndexOf(arrVar, i);
                    if (iIndex != -1)
                    {
                        arrMappingVar[iIndex] = iMapping;

                        #region 내부 대화상자
                        if (bGraph)
                        {
                            iIndex = Array.IndexOf(m_Input.arrGraphVariablesVsResiduals, i);
                            if (iIndex != -1)
                                arrTempGraph[iIndex] = iMapping;
                        }
                        if (bInclude)
                        {
                            iIndex = Array.IndexOf(m_Input.arrIncludePredictors, i);
                            if (iIndex != -1)
                                arrTempInclude[iIndex] = iMapping;
                        }
                        if (bInitial)
                        {
                            iIndex = Array.IndexOf(m_Input.arrPredictorsInitialModel, i);
                            if (iIndex != -1)
                                arrTempInitial[iIndex] = iMapping;
                        }
                        #endregion

                        iMapping--;
                    }
                    else
                    {
                        m_Input.DataSource.Columns.RemoveAt(i);
                        m_Input.DataSource.AcceptChanges();
                    }
                }
                m_Input.iResponse = arrMappingVar[0];
                m_Input.arrPredictors = new int[iVarCnt];
                for (int i = 0; i < iVarCnt; i++)
                    m_Input.arrPredictors[i] = arrMappingVar[i + 1];
                m_Input.arrGraphVariablesVsResiduals = arrTempGraph;
                m_Input.arrIncludePredictors = arrTempInclude;
                m_Input.arrPredictorsInitialModel = arrTempInitial;

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrMappingVar = null;
                arrVar = null;
            }
        }

        #endregion

        #region [ Buton Click ]

        #region Left

        void btnLeft_Response_Click(object sender, EventArgs e)
        {
            if (lvRespense.SelectedItems.Count > 0)
            {
                RemoveVariables((IList)lvRespense.SelectedItems);
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvRespense, lvColumns, (IList)lvRespense.SelectedItems);
            }
        }

        void btnLeft_Predictors_Click(object sender, EventArgs e)
        {
            if (lvPredictors.SelectedItems.Count > 0)
            {
                RemoveVariables((IList)lvPredictors.SelectedItems);
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvPredictors, lvColumns, (IList)lvPredictors.SelectedItems);
            }
        }

        #endregion

        #region Right

        void btnRight_Response_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvColumns.SelectedItems.Count > 0)
                {
                    if (lvColumns.SelectedItems.Count == 1)
                    {
                        if (lvRespense.Items.Count > 0)
                        {
                            DACrux.ProjectManager.UI.Common.MoveListViewItem(lvRespense, lvColumns, lvRespense.Items[0]);
                        }
                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvRespense, lvColumns.SelectedItems[0]);
                        m_lvRespense = false;
                    }

                    else
                        MessageBox.Show(m_ErrorOneColumn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnRight_Predictors_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvColumns.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvPredictors, (IList)lvColumns.SelectedItems);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region [ ListView Enter ]

        void lvRespense_Enter(object sender, EventArgs e)
        {
            m_lvRespense = true;
        }

        void lvPredictors_Enter(object sender, EventArgs e)
        {
            m_lvRespense = false;
        }

        #endregion

        #region  [ ListView DragDrop ]

        void lvRespense_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
                {
                    if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    {
                        if (lvColumns.SelectedItems.Count > 0)
                        {
                            if (lvColumns.SelectedItems.Count == 1)
                            {
                                if (lvRespense.Items.Count > 0)
                                {
                                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvRespense, lvColumns, lvRespense.Items[0]);
                                }
                                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvRespense, lvColumns.SelectedItems[0]);
                                m_lvRespense = false;
                            }

                            else
                                MessageBox.Show(m_ErrorOneColumn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvPredictors_DragDrop(object sender, DragEventArgs e)
        {
            List<ListViewItem> filteredItems = null;
            try
            {
                if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
                {
                    if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    {
                        filteredItems = new List<ListViewItem>();
                        for (int i = 0; i < lvColumns.SelectedItems.Count; i++)
                        {
                            if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[i]]).DataType.Equals(DataType.NUMBER))
                                filteredItems.Add(lvColumns.SelectedItems[i]);
                        }
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvPredictors, (IList)filteredItems);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvColumns_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
                {
                    if (lvPredictors.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    {
                        RemoveVariables((IList)lvPredictors.SelectedItems);
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvPredictors, lvColumns, (IList)lvPredictors.SelectedItems);
                    }
                    else if (lvRespense.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    {
                        RemoveVariables((IList)lvRespense.SelectedItems);
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvRespense, lvColumns, (IList)lvRespense.SelectedItems);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListView DragEnter ]

        void lvRespense_DragEnter(object sender, DragEventArgs e)
        {
            if (lvRespense.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvPredictors.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
            {
                e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.Move;
        }

        void lvPredictors_DragEnter(object sender, DragEventArgs e)
        {
            if (lvRespense.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvPredictors.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
            {
                e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.Move;
        }

        void lvColumns_DragEnter(object sender, DragEventArgs e)
        {
            if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
            {
                e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.Move;
        }
        #endregion

        #region [ ListView ItemDrag ]

        void lvRespense_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvRespense.DoDragDrop(lvRespense.SelectedItems, DragDropEffects.Move);
        }

        void lvPredictors_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvPredictors.DoDragDrop(lvPredictors.SelectedItems, DragDropEffects.Move);
        }

        void lvColumns_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvColumns.DoDragDrop(lvColumns.SelectedItems, DragDropEffects.Move);
        }

        #endregion

        #region [ ListView DoubleClick ]

        void lvRespense_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvRespense.SelectedItems.Count > 0)
                {
                    RemoveVariable(lvRespense.SelectedItems[0]);
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvRespense, lvColumns, lvRespense.SelectedItems[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvPredictors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvPredictors.SelectedItems.Count > 0)
                {
                    RemoveVariable(lvPredictors.SelectedItems[0]);
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvPredictors, lvColumns, lvPredictors.SelectedItems[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvColumns_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                if (m_lvRespense)
                {
                    if (lvRespense.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvRespense, lvColumns, lvRespense.Items[0]);
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvRespense, lvColumns.SelectedItems[0]);
                    m_lvRespense = false;
                }
                else
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvPredictors, lvColumns.SelectedItems[0]);
                }
            }
        }

        #endregion

        #region [ Inquery Option ]

        private void SetSetting()
        {
            MenuOption oMenuOption = null;
            inputRegression oOption = null;
            try
            {
                oMenuOption = new MenuOption();
                oOption = oMenuOption.GetRegression();
                m_Input.backwardDrop = oOption.backwardDrop;
                m_Input.forwardAdd = oOption.forwardAdd;
                m_Input.IsGraphHistogramResiduals = oOption.IsGraphHistogramResiduals;
                m_Input.IsGraphProbabilityPlotResiduals = oOption.IsGraphProbabilityPlotResiduals;
                m_Input.IsGraphResidualsVsFittedValues = oOption.IsGraphResidualsVsFittedValues;
                m_Input.IsGraphResidualsVSOrder = oOption.IsGraphResidualsVSOrder;
                m_Input.IsIntercept = oOption.IsIntercept;
                m_Input.IsTableAnova = oOption.IsTableAnova;
                m_Input.IsTableCoefficients = oOption.IsTableCoefficients;
                m_Input.IsTableR_Square = oOption.IsTableR_Square;
                m_Input.IsTableResiduals = oOption.IsTableResiduals;
                m_Input.IsUseAlpha = oOption.IsUseAlpha;
                m_Input.ReAnalysisType = oOption.ReAnalysisType;
                m_Input.stepwiseAdd = oOption.stepwiseAdd;
                m_Input.stepwiseDrop = oOption.stepwiseDrop;
                chkIntercept.Checked = m_Input.IsIntercept;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// m_Input정리한 후에 실행할 것!!!
        /// </summary>
        private void SaveSetting()
        {
            MenuOption oMenuOption = null;
            try
            {
                oMenuOption = new MenuOption();
                oMenuOption.UpdateRegression(m_Input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region " EVENT HANDLER "

        #region [ OPTION BUTTON ]

        void btnOptions_Click(object sender, EventArgs e)
        {
            int iResponseItemCnt;
            int iPredictorsItemCnt;
            try
            {
                SelectedRegressionOptions oSelectedRegressionOptions = new SelectedRegressionOptions();
                //oSelectedRegressionOptions.Reset();

                oSelectedRegressionOptions.arrGraphVariablesVsResiduals = m_Input.arrGraphVariablesVsResiduals;
                oSelectedRegressionOptions.IsGraphHistogramResiduals = m_Input.IsGraphHistogramResiduals;
                oSelectedRegressionOptions.IsGraphProbabilityPlotResiduals = m_Input.IsGraphProbabilityPlotResiduals;
                oSelectedRegressionOptions.IsGraphResidualsVsFittedValues = m_Input.IsGraphResidualsVsFittedValues;
                oSelectedRegressionOptions.IsGraphResidualsVSOrder = m_Input.IsGraphResidualsVSOrder;
                oSelectedRegressionOptions.IsTableAnova = m_Input.IsTableAnova;
                oSelectedRegressionOptions.IsTableCoefficients = m_Input.IsTableCoefficients;
                oSelectedRegressionOptions.IsTableR_Square = m_Input.IsTableR_Square;
                oSelectedRegressionOptions.IsTableResiduals = m_Input.IsTableResiduals;

                iResponseItemCnt = lvRespense.Items.Count;
                iPredictorsItemCnt = lvPredictors.Items.Count;

                List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstColumnInfo = null;
                lstColumnInfo = new List<DACrux.ProjectManager.UI.DataView.ColumnInfo>(iResponseItemCnt + iPredictorsItemCnt);
                if (iResponseItemCnt == 1)
                    lstColumnInfo.Add(((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvRespense.Items[0]]));
                for (int i = 0; i < iPredictorsItemCnt; i++)
                    lstColumnInfo.Add(((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvPredictors.Items[i]]));
                DlgRegressionOptions oDlgRegressionOptions = new DlgRegressionOptions(oSelectedRegressionOptions, lstColumnInfo);
                oDlgRegressionOptions.On_SelectedOptions += new DSelectedRegressionOptions(oDlgRegressionOptions_On_SelectedOptions);
                oDlgRegressionOptions.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void oDlgRegressionOptions_On_SelectedOptions(SelectedRegressionOptions oSelected)
        {
            try
            {
                m_Input.arrGraphVariablesVsResiduals = oSelected.arrGraphVariablesVsResiduals;
                m_Input.IsGraphHistogramResiduals = oSelected.IsGraphHistogramResiduals;
                m_Input.IsGraphProbabilityPlotResiduals = oSelected.IsGraphProbabilityPlotResiduals;
                m_Input.IsGraphResidualsVsFittedValues = oSelected.IsGraphResidualsVsFittedValues;
                m_Input.IsGraphResidualsVSOrder = oSelected.IsGraphResidualsVSOrder;
                m_Input.IsTableAnova = oSelected.IsTableAnova;
                m_Input.IsTableCoefficients = oSelected.IsTableCoefficients;
                m_Input.IsTableR_Square = oSelected.IsTableR_Square;
                m_Input.IsTableResiduals = oSelected.IsTableResiduals;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnMethods_Click(object sender, EventArgs e)
        {
            int iItemCnt;
            try
            {
                SelectedRegressionMethods oSelectedRegressionMethods = new SelectedRegressionMethods();
                //oSelectedRegressionMethods.Reset();
                oSelectedRegressionMethods.arrIncludePredictors = m_Input.arrIncludePredictors;
                oSelectedRegressionMethods.arrPredictorsInitialModel = m_Input.arrPredictorsInitialModel;
                oSelectedRegressionMethods.backwardDrop = m_Input.backwardDrop;
                oSelectedRegressionMethods.forwardAdd = m_Input.forwardAdd;
                oSelectedRegressionMethods.IsUseAlpha = m_Input.IsUseAlpha;
                oSelectedRegressionMethods.ReAnalysisType = m_Input.ReAnalysisType;
                oSelectedRegressionMethods.stepwiseAdd = m_Input.stepwiseAdd;
                oSelectedRegressionMethods.stepwiseDrop = m_Input.stepwiseDrop;

                iItemCnt = lvPredictors.Items.Count;
                List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstColumnInfo = null;
                lstColumnInfo = new List<DACrux.ProjectManager.UI.DataView.ColumnInfo>(iItemCnt);
                for (int i = 0; i < iItemCnt; i++)
                    lstColumnInfo.Add(((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvPredictors.Items[i]]));
                DlgRegressionMethods oDlgRegressionMethods = new DlgRegressionMethods(oSelectedRegressionMethods, lstColumnInfo);
                oDlgRegressionMethods.On_SelectedMethod += new DSelectedRegressionMethods(oDlgRegressionMethods_On_SelectedMethod);
                oDlgRegressionMethods.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void oDlgRegressionMethods_On_SelectedMethod(SelectedRegressionMethods oSelected)
        {
            try
            {

                m_Input.IsUseAlpha = oSelected.IsUseAlpha;
                m_Input.ReAnalysisType = oSelected.ReAnalysisType;
                switch (m_Input.ReAnalysisType)
                {
                    case RegressionType.Stepwise:
                        m_Input.arrPredictorsInitialModel = oSelected.arrPredictorsInitialModel;
                        m_Input.stepwiseAdd = oSelected.stepwiseAdd;
                        m_Input.stepwiseDrop = oSelected.stepwiseDrop;
                        break;
                    case RegressionType.Backward:
                        m_Input.backwardDrop = oSelected.backwardDrop;
                        break;
                    case RegressionType.Forward:
                        m_Input.forwardAdd = oSelected.forwardAdd;
                        break;
                    default:
                        break;
                }
                m_Input.arrIncludePredictors = oSelected.arrIncludePredictors;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ CLOSING ]

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            #region m_Input 정리 및 체크

            if (!CheckSelectedVar())
                return;

            TrimInput();

            m_Input.IsIntercept = chkIntercept.Checked;
            #endregion
            SaveSetting();
            if (On_SelectedRegression != null)
                On_SelectedRegression(m_Input);
            this.DialogResult = DialogResult.OK;
        }
        #endregion

        #endregion


    }
}