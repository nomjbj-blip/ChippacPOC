using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DACrux.ProjectManager.UI;
using DACrux.BStats.StatisticsInput;
using DACrux.BStats.StatDialog.DlgDescriptiveAnalysis_;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog
{
    

    public partial class DlgDescriptiveStatistics : Form, iStatInformation
    {
        #region " MEMBER FIELD "

        public string FUNC_CODE = "F0301";

        #region [ LANGUAGE ]
        string m_Title = "Display Descriptive Statistics"; 
        string m_Columns = "Columns";
        string m_Variables = "Variables";
        string m_ByVar = "Class Variables(optional)";
        string m_Statistics = "&Statistics...";
        string m_Graphs = "G&raphs...";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";

        string m_ErrorSelectVar = "Please select Variables";
        string m_ErrorLength = "Length of selected variables doesn't equal.";
        #endregion

        #region [ EVENT ]
        public event DSelectedDescriptiveStatistics On_SelectedDescriptiveStatistics;
        #endregion
        
        private inputDescriptiveStatistics m_Input;        
        
        private System.Collections.Hashtable htColumns = null;

        private List<DACrux.ProjectManager.UI.DataView.ColumnInfo> m_lstValidColumnInfo;

        /// <summary>
        /// 선택 컬럼 옮길 곳
        /// </summary>
        private bool m_lvVariables = true;

        #endregion       

        #region " CREATOR "
   
        public DlgDescriptiveStatistics(DataTable dtSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo, string strProject, string strWorkSheet, string strTitle, string strResultFilePath, string strUser)
        {
            InitializeComponent();
            m_lstValidColumnInfo = lstValidColumnInfo;
            m_Input = new inputDescriptiveStatistics(strTitle, strResultFilePath);
            m_Input.DataSource = dtSource;
            m_Input.Title = strTitle;
            m_Input.Project = strProject;
            m_Input.WorkSheet = strWorkSheet;
            m_Input.User = strUser;
            SetSetting();
            InitDialog();
            SetColumnListView(m_lstValidColumnInfo);
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

        private void InitDialog()
        {
            try
            {
                this.Text = m_Title;
                this.lblColumns.Text = m_Columns;
                this.lblVariables.Text = m_Variables;
                this.lblByVar.Text = m_ByVar;
                this.btnStatistics.Text = m_Statistics;
                this.btnGraph.Text = m_Graphs;
                this.btnOk.Text = m_Ok;
                this.btnCancel.Text = m_Cancel;


                lvColumns.SmallImageList = imlColumnType;
                lvVariables.SmallImageList = imlColumnType;
                lvByVar.SmallImageList = imlColumnType;

                lvColumns.AllowDrop = true;
                lvVariables.AllowDrop = true;
                lvByVar.AllowDrop = true;

                lvColumns.MultiSelect = true;
                lvVariables.MultiSelect = true;
                lvByVar.MultiSelect = true;



                this.lvVariables.Enter += new EventHandler(lvVariables_Enter);
                this.lvByVar.Enter += new EventHandler(lvByVar_Enter);


                this.btnRight_Var.Click += new EventHandler(btnRight_Var_Click);
                this.btnLeft_Var.Click += new EventHandler(btnLeft_Var_Click);
                this.btnRight_ByVal.Click += new EventHandler(btnRight_ByVal_Click);
                this.btnLeft_ByVal.Click += new EventHandler(btnLeft_ByVal_Click);

                this.lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);
                this.lvVariables.MouseDoubleClick += new MouseEventHandler(lvVariables_MouseDoubleClick);
                this.lvByVar.MouseDoubleClick += new MouseEventHandler(lvByVar_MouseDoubleClick);

                this.lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
                this.lvVariables.ItemDrag += new ItemDragEventHandler(lvVariables_ItemDrag);
                this.lvByVar.ItemDrag += new ItemDragEventHandler(lvByVar_ItemDrag);

                this.lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
                this.lvVariables.DragEnter += new DragEventHandler(lvVariables_DragEnter);
                this.lvByVar.DragEnter += new DragEventHandler(lvByVar_DragEnter);

                this.lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
                this.lvVariables.DragDrop += new DragEventHandler(lvVariables_DragDrop);
                this.lvByVar.DragDrop += new DragEventHandler(lvByVar_DragDrop);

                this.btnOk.Click += new EventHandler(btnOk_Click);
                this.btnCancel.Click += new EventHandler(btnCancel_Click);
                this.btnStatistics.Click += new EventHandler(btnStatistics_Click);
                this.btnGraph.Click += new EventHandler(btnGraph_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [ ListView Enter ]

        void lvByVar_Enter(object sender, EventArgs e)
        {
            m_lvVariables = false;
        }

        void lvVariables_Enter(object sender, EventArgs e)
        {            
            m_lvVariables = true;
        }

        #endregion

        #region [ ListView DragDrop ]

        void lvByVar_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
                {
                    if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvByVar, (IList)lvColumns.SelectedItems);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvVariables_DragDrop(object sender, DragEventArgs e)
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
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, (ListView)sender, (IList)filteredItems);
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
                    if (lvByVar.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    {
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvByVar, (ListView)sender, (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
                    }
                    else
                    {
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvVariables, (ListView)sender, (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListView Drag Enter ]

        void lvByVar_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (lvByVar.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    e.Effect = DragDropEffects.None;
                else
                    e.Effect = DragDropEffects.Move;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvVariables_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (lvByVar.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    e.Effect = DragDropEffects.None;
                else
                    e.Effect = DragDropEffects.Move;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvColumns_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    e.Effect = DragDropEffects.None;
                else
                    e.Effect = DragDropEffects.Move;
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListView Item Drag ]

        void lvByVar_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                lvByVar.DoDragDrop(lvByVar.SelectedItems, DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvVariables_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                lvVariables.DoDragDrop(lvVariables.SelectedItems, DragDropEffects.Move);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvColumns_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                lvColumns.DoDragDrop(lvColumns.SelectedItems, DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListView Mouse DoubleClick ]

        void lvByVar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvByVar.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvByVar, lvColumns, lvByVar.SelectedItems[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvVariables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvVariables.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvVariables, lvColumns, lvVariables.SelectedItems[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        void lvColumns_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvColumns.SelectedItems.Count > 0)
                {
                    if (m_lvVariables)
                    {
                        if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[0]]).DataType.Equals(DataType.NUMBER))
                            DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvVariables, lvColumns.SelectedItems[0]);
                    }
                    else
                    {
                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvByVar, lvColumns.SelectedItems[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListViewItem Move Button ]

        void btnLeft_ByVal_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvByVar.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvByVar, lvColumns, (IList)lvByVar.SelectedItems);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnRight_ByVal_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvColumns.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvByVar, (IList)lvColumns.SelectedItems);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnLeft_Var_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvVariables.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvVariables, lvColumns, (IList)lvVariables.SelectedItems);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnRight_Var_Click(object sender, EventArgs e)
        {
            List<ListViewItem> filteredItems = null;
            try
            {

                if (lvColumns.SelectedItems.Count > 0)
                {
                    filteredItems = new List<ListViewItem>();
                    for (int i = 0; i < lvColumns.SelectedItems.Count; i++)
                    {
                        if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[i]]).DataType.Equals(DataType.NUMBER))
                            filteredItems.Add(lvColumns.SelectedItems[i]);
                    }
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvVariables, (IList)filteredItems);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ Inquery Option ]

        private void SetSetting()
        {
            MenuOption oMenuOption = null;
            inputDescriptiveStatistics oOption = null;
            try
            {
                oMenuOption = new MenuOption();
                oOption = oMenuOption.GetDescriptiveStatistics();
                m_Input.IsBoxPlot = oOption.IsBoxPlot;
                m_Input.IsCoefficientofvariation = oOption.IsCoefficientofvariation;
                m_Input.IsCumulativeN = oOption.IsCumulativeN;
                m_Input.IsCumulativePercent = oOption.IsCumulativePercent;
                m_Input.IsHistogram = oOption.IsHistogram;
                m_Input.IsHistogramNNormalCurve = oOption.IsHistogramNNormalCurve;
                m_Input.IsInterquartileRange = oOption.IsInterquartileRange;
                m_Input.IsKurtosis = oOption.IsKurtosis;
                m_Input.IsMax = oOption.IsMax;
                m_Input.IsMean = oOption.IsMean;
                m_Input.IsMeanSE = oOption.IsMeanSE;
                m_Input.IsMedian = oOption.IsMedian;
                m_Input.IsMin = oOption.IsMin;
                m_Input.IsMode = oOption.IsMode;
                m_Input.IsMSSD = oOption.IsMSSD;
                m_Input.IsNmissing = oOption.IsNmissing;
                m_Input.IsNnonmissing = oOption.IsNnonmissing;
                m_Input.IsNtotal = oOption.IsNtotal;
                m_Input.IsPercent = oOption.IsPercent;
                m_Input.IsQ1 = oOption.IsQ1;
                m_Input.IsQ3 = oOption.IsQ3;
                m_Input.IsRange = oOption.IsRange;
                m_Input.IsSkewness = oOption.IsSkewness;
                m_Input.IsStandarddeviation = oOption.IsStandarddeviation;
                m_Input.IsSum = oOption.IsSum;
                m_Input.IsSumofSquares = oOption.IsSumofSquares;
                m_Input.IsTrimmedMean = oOption.IsTrimmedMean;
                m_Input.IsVariance = oOption.IsVariance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveSetting()
        {
            MenuOption oMenuOption = null;
            try
            {
                oMenuOption = new MenuOption();
                oMenuOption.UpdateDescriptiveStatistics(m_Input);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        #endregion

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

        /// <summary>
        /// 선택한 컬럼으로만 DataTable정리하고 선택한 컬럼에 대한 정보 정리
        /// </summary>
        /// 
        private void TrimInput()
        {
            int iColCnt;
            int iSelectedColCnt;
            int iVarCnt;
            int iByVarCnt;
            int iMapping;
            int iMaxRow;
            int iRowCnt;
            #region 현재 선택된 컬럼 인덱스 리스트
            int[] arrVar = null;
            int[] arrByVar = null;
            #endregion
            #region 정리된 DataTable에서 현재 선택된 컬럼 인덱스 리스트
            int[] arrMappingVar = null;
            int[] arrMappingByVar = null;
            #endregion
            #region DataTable의 선택 컬럼의 인덱스 리스트
            int[] arrTotal = null;
            #endregion
            try
            {
                iVarCnt = lvVariables.Items.Count;
                iByVarCnt = lvByVar.Items.Count;

                

                #region Trim Valid Row

                m_Input.arrValidRowIndices = new int[iVarCnt];

                iMaxRow = 0;
                for (int i = 0; i < iVarCnt; i++)
                {
                    m_Input.arrValidRowIndices[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ValidRowIndex;
                    if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ValidRowIndex)
                        iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ValidRowIndex;

                }
                for (int i = 0; i < iByVarCnt; i++)
                {
                    if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvByVar.Items[i]]).ValidRowIndex)
                        iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvByVar.Items[i]]).ValidRowIndex;
                }
                iRowCnt = m_Input.DataSource.Rows.Count;
                for (int i = iRowCnt - 1; i > -1; i--)
                {
                    if (i > iMaxRow)
                        m_Input.DataSource.Rows.RemoveAt(i);
                }
                m_Input.DataSource.AcceptChanges();

                #endregion

                iSelectedColCnt = iVarCnt + iByVarCnt;
                arrVar = new int[iVarCnt];
                arrByVar = new int[iByVarCnt];
                arrTotal = new int[iSelectedColCnt];
                for (int i = 0; i < iVarCnt; i++)
                {
                    arrVar[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ColumnIndex;
                    arrTotal[i] = arrVar[i];
                }
                for (int i = 0; i < iByVarCnt; i++)
                {
                    arrByVar[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvByVar.Items[i]]).ColumnIndex;
                    arrTotal[iVarCnt + i] = arrByVar[i];
                }
                arrMappingVar = new int[iVarCnt];
                arrMappingByVar = new int[iByVarCnt];

                #region 선택한 컬럼만 남기고 나머지 컬럼삭제, 선택 인덱스 목록 맵핑
                iColCnt = m_Input.DataSource.Columns.Count;
                iMapping = iSelectedColCnt - 1;
                for (int i = iColCnt - 1; i > -1; i--)
                {
                    int iIndex = -1;
                    iIndex = Array.IndexOf(arrTotal, i);

                    if (iIndex != -1)
                    {

                        iIndex = Array.IndexOf(arrVar, i);
                        if (iIndex != -1)
                        {
                            arrMappingVar[iIndex] = iMapping;
                            iMapping--;
                        }
                        else
                        {
                            iIndex = Array.IndexOf(arrByVar, i);
                            if (iIndex != -1)
                            {
                                arrMappingByVar[iIndex] = iMapping;
                                iMapping--;
                            }
                            else
                            {
#if DEBUG
                                MessageBox.Show("?????????????????????????????????????");
#endif
                            }
                        }
                    }
                    else
                    {
                        m_Input.DataSource.Columns.RemoveAt(i);
                        m_Input.DataSource.AcceptChanges();
                    }
                }
                m_Input.arrVariables = arrMappingVar;
                m_Input.arrByVariables = arrMappingByVar;
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrByVar = null;
                arrMappingByVar = null;
                arrMappingVar = null;
                arrTotal = null;
                arrVar = null;                
            }
        }

        #region Check Selected Variables

        #region Check Length of Columns

        private bool CheckColumns()
        {
            //int[] arrVar = null;
            int iVarCnt;
            int iTemp;
            try
            {
                iVarCnt = lvByVar.Items.Count;
                if (iVarCnt < 1)  //구분변수 없음
                    return true;
                iTemp = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvByVar.Items[0]]).ValidRowIndex;
                //iTemp = lstValidColumnInfo[((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvByVar.Items[0]]).ColumnIndex].ValidRowIndex;
                for (int i = 0; i < iVarCnt; i++)
                {
                    if (iTemp != ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvByVar.Items[i]]).ValidRowIndex)
                    {
                        MessageBox.Show(m_ErrorLength);
                        return false;
                    }
                }

                iVarCnt = lvVariables.Items.Count;
                if (iVarCnt < 1)
                {
                    MessageBox.Show(m_ErrorSelectVar);
                    return false;
                }

                for (int i = 0; i < iVarCnt; i++)
                {
                    //arrVar[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ColumnIndex;
                    if (iTemp != ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ValidRowIndex)
                    {
                        MessageBox.Show(m_ErrorLength);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private bool CheckSelectedVar()   
        {
            try
            {
                if (m_Input.arrVariables == null || m_Input.arrVariables.Length == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #endregion

        #region " EVENT HANDLER "

        #region [ OPTION ]

        private void btnGraph_Click(object sender, EventArgs e)
        {
            SelectedGraphs oSelectedGraphs = new SelectedGraphs();
            oSelectedGraphs.IsBoxPlot = m_Input.IsBoxPlot;
            oSelectedGraphs.IsHistogram = m_Input.IsHistogram;
            oSelectedGraphs.IsHistogramNNormalCurve = m_Input.IsHistogramNNormalCurve;
            oSelectedGraphs.IsRawDataPlot = m_Input.IsRawDataPlot;
            DlgGraph oDlgGraph = new DlgGraph(oSelectedGraphs);
            oDlgGraph.On_SelectedGraph += new DSelectedGraph(oDlgGraph_On_SelectedGraph);
            oDlgGraph.ShowDialog();
        }

        void oDlgGraph_On_SelectedGraph(SelectedGraphs oSelected)
        {
            m_Input.IsBoxPlot = oSelected.IsBoxPlot;
            m_Input.IsHistogram = oSelected.IsHistogram;
            m_Input.IsHistogramNNormalCurve = oSelected.IsHistogramNNormalCurve;
            m_Input.IsRawDataPlot = oSelected.IsRawDataPlot;
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {           
            SelectedStatistics oSelectedStatistics = new SelectedStatistics();
            oSelectedStatistics.IsCoefficientofvariation = m_Input.IsCoefficientofvariation;
            oSelectedStatistics.IsCumulativeN = m_Input.IsCumulativeN;
            oSelectedStatistics.IsCumulativePercent = m_Input.IsCumulativePercent;
            oSelectedStatistics.IsInterquartileRange = m_Input.IsInterquartileRange;
            oSelectedStatistics.IsKurtosis = m_Input.IsKurtosis;
            oSelectedStatistics.IsMax = m_Input.IsMax;
            oSelectedStatistics.IsMean = m_Input.IsMean;
            oSelectedStatistics.IsMeanSE = m_Input.IsCoefficientofvariation;
            oSelectedStatistics.IsMedian = m_Input.IsMedian;
            oSelectedStatistics.IsMin = m_Input.IsMin;
            oSelectedStatistics.IsMode = m_Input.IsMode;
            oSelectedStatistics.IsMSSD = m_Input.IsMSSD;
            oSelectedStatistics.IsNmissing = m_Input.IsNmissing;
            oSelectedStatistics.IsNnonmissing = m_Input.IsNnonmissing;
            oSelectedStatistics.IsNtotal = m_Input.IsNtotal;
            oSelectedStatistics.IsPercent = m_Input.IsPercent;
            oSelectedStatistics.IsQ1 = m_Input.IsQ1;
            oSelectedStatistics.IsQ3 = m_Input.IsQ3;
            oSelectedStatistics.IsRange = m_Input.IsRange;
            oSelectedStatistics.IsSkewness = m_Input.IsSkewness;
            oSelectedStatistics.IsStandarddeviation = m_Input.IsStandarddeviation;
            oSelectedStatistics.IsSum = m_Input.IsSum;
            oSelectedStatistics.IsSumofSquares = m_Input.IsSumofSquares;
            oSelectedStatistics.IsTrimmedMean = m_Input.IsTrimmedMean;
            oSelectedStatistics.IsVariance = m_Input.IsVariance;
                    
            DlgStatistics oDlgStatistics = new DlgStatistics(oSelectedStatistics);
            oDlgStatistics.On_SelectedStatistics += new DSelectedStatistics(oDlgStatistics_On_SelectedStatistics);
            oDlgStatistics.ShowDialog();

        }

        void oDlgStatistics_On_SelectedStatistics(SelectedStatistics oSelectedStatistics)
        {
            m_Input.IsCoefficientofvariation = oSelectedStatistics.IsCoefficientofvariation;
            m_Input.IsCumulativeN = oSelectedStatistics.IsCumulativeN;
            m_Input.IsCumulativePercent = oSelectedStatistics.IsCumulativePercent;
            m_Input.IsInterquartileRange = oSelectedStatistics.IsInterquartileRange;
            m_Input.IsKurtosis = oSelectedStatistics.IsKurtosis;
            m_Input.IsMax = oSelectedStatistics.IsMax;
            m_Input.IsMean = oSelectedStatistics.IsMean;
            m_Input.IsMeanSE = oSelectedStatistics.IsMeanSE;
            m_Input.IsMedian = oSelectedStatistics.IsMedian;
            m_Input.IsMin = oSelectedStatistics.IsMin;
            m_Input.IsMode = oSelectedStatistics.IsMode;
            m_Input.IsMSSD = oSelectedStatistics.IsMSSD;
            m_Input.IsNmissing = oSelectedStatistics.IsNmissing;
            m_Input.IsNnonmissing = oSelectedStatistics.IsNnonmissing;
            m_Input.IsNtotal = oSelectedStatistics.IsNtotal;
            m_Input.IsPercent = oSelectedStatistics.IsPercent;
            m_Input.IsQ1 = oSelectedStatistics.IsQ1;
            m_Input.IsQ3 = oSelectedStatistics.IsQ3;
            m_Input.IsRange = oSelectedStatistics.IsRange;
            m_Input.IsSkewness = oSelectedStatistics.IsSkewness;
            m_Input.IsStandarddeviation = oSelectedStatistics.IsStandarddeviation;
            m_Input.IsSum = oSelectedStatistics.IsSum;
            m_Input.IsSumofSquares = oSelectedStatistics.IsSumofSquares;
            m_Input.IsTrimmedMean = oSelectedStatistics.IsTrimmedMean;
            m_Input.IsVariance = oSelectedStatistics.IsVariance;
        }

        #endregion

        #region [ CLOSING ]

        private void btnOk_Click(object sender, EventArgs e)
        {
            #region m_Input 정리
            TrimInput();
            #endregion
            if (!CheckSelectedVar())
            {
                MessageBox.Show(m_ErrorSelectVar);
                return;
            }
            if (!CheckColumns())
                return;

            SaveSetting();
            On_SelectedDescriptiveStatistics(m_Input);
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion


        #endregion
    }
}