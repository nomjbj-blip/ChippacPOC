using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DACrux.BStats.StatisticsInput;
using DACrux.ProjectManager.UI;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog
{
    public partial class DlgANOVA : Form, iStatInformation
    {
        #region " MEMBER FIELD "

        public string FUNC_CODE = "F0304";

        #region [ LANGUAGE ]
        string m_Title = "ANOVA";
        string m_SingleColumn = "Data are arranged as";
        string m_Response = "Response";
        string m_Responses = "Responses(in separate columns):";
        string m_Factor = "Factor";
        string m_Columns = "Columns";

        string m_Graph = "G&raphs...";
        string m_Compare = "&Compare";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";


        //MESSAGE
        string m_ErrorOneColumn = "Please select only one column.";
        string m_ErrorOneColumn2 = "Please select one column for ";
        string m_ErrorNumberColumn = "Please select number type column.";
        string m_ErrorOneMoreColumn = "Please select one or more column for analysis";
        string m_ErrorLength = "The following must be the same length : factor variable and response variable.";
        
        //string m_ErrorOneColumn = "Please select only one column.";
        //string m_ErrorResponse = "Invalid response variable. \r\n Too few items.";
        //string m_ErrorFactor = "Invalid factor. \r\n Too few items.";

        bool m_bSingle = true; // Responses인지 Single인지
        bool m_bFactor = false; // Response인지 Factor인지
        #endregion

        #region [ EVENT ]
        public event DSelectedAnoa On_SelectedAnova;
        #endregion


        //Column Double Click시에 삽입할 곳...

        private inputANOVA m_Input;

        private System.Collections.Hashtable htColumns = null;

        #endregion


        #region " CREATOR "

        public DlgANOVA(DataTable dtSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo, string strProject, string strWorkSheet, string strTitle, string strResultFilePath, string strUser)
        {
            InitializeComponent();
            m_Input = new inputANOVA(strTitle, strResultFilePath);
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

        private void InitDialog()
        {
            try
            {
                this.Text = m_Title;
                this.lblColumns.Text = m_Columns;
                this.rdoSigleColumn.Text = m_SingleColumn;
                this.rdoSeparateCols.Text = m_Responses;
                this.lblResponse.Text = m_Response;
                this.lblFactor.Text = m_Factor;
                this.btnGraph.Text = m_Graph;
                this.btnOk.Text = m_Ok;
                this.btnCancel.Text = m_Cancel;
                this.btnComparisons.Text = m_Compare;

                lvColumns.SmallImageList = imlColumnType;
                lvSingleCol.SmallImageList = imlColumnType;
                lvFactorCol.SmallImageList = imlColumnType;
                lvSeparateCols.SmallImageList = imlColumnType;

                lvColumns.AllowDrop = true;
                lvSingleCol.AllowDrop = true;
                lvFactorCol.AllowDrop = true;
                lvSeparateCols.AllowDrop = true;

                lvColumns.MultiSelect = true;
                lvSingleCol.MultiSelect = true;
                lvFactorCol.MultiSelect = true;
                lvSeparateCols.MultiSelect = true;

                this.lvSingleCol.Enter += new EventHandler(lvSingleCol_Enter);
                this.lvFactorCol.Enter += new EventHandler(lvFactorCol_Enter);
                this.lvSeparateCols.Enter += new EventHandler(lvSeparateCols_Enter);

                this.btnRight_FactorCol.Click += new EventHandler(btnRight_FactorCol_Click);
                this.btnRight_SeparateCols.Click += new EventHandler(btnRight_SeparateCols_Click);
                this.btnRight_SingleCol.Click += new EventHandler(btnRight_SingleCol_Click);


                this.btnLeft_FactorCol.Click += new EventHandler(btnLeft_FactorCol_Click);
                this.btnLeft_SeparateCols.Click += new EventHandler(btnLeft_SeparateCols_Click);
                this.btnLeft_SingleCol.Click += new EventHandler(btnLeft_SingleCol_Click);

                this.lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);
                this.lvFactorCol.MouseDoubleClick += new MouseEventHandler(lvFactorCol_MouseDoubleClick);
                this.lvSeparateCols.MouseDoubleClick += new MouseEventHandler(lvSeparateCols_MouseDoubleClick);
                this.lvSingleCol.MouseDoubleClick += new MouseEventHandler(lvSingleCol_MouseDoubleClick);

                this.lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
                this.lvFactorCol.ItemDrag += new ItemDragEventHandler(lvFactorCol_ItemDrag);
                this.lvSeparateCols.ItemDrag += new ItemDragEventHandler(lvSeparateCols_ItemDrag);
                this.lvSingleCol.ItemDrag += new ItemDragEventHandler(lvSingleCol_ItemDrag);

                this.lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
                this.lvFactorCol.DragEnter += new DragEventHandler(lvFactorCol_DragEnter);
                this.lvSeparateCols.DragEnter += new DragEventHandler(lvSeparateCols_DragEnter);
                this.lvSingleCol.DragEnter += new DragEventHandler(lvSingleCol_DragEnter);

                this.lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
                this.lvFactorCol.DragDrop += new DragEventHandler(lvFactorCol_DragDrop);
                this.lvSeparateCols.DragDrop += new DragEventHandler(lvSeparateCols_DragDrop);
                this.lvSingleCol.DragDrop += new DragEventHandler(lvSingleCol_DragDrop);

                this.btnOk.Click += new EventHandler(btnOk_Click);
                this.btnGraph.Click += new EventHandler(btnGraph_Click);
                this.btnCancel.Click += new EventHandler(btnCancel_Click);

                this.rdoSigleColumn.CheckedChanged += new EventHandler(rdoSigleColumn_CheckedChanged);

                // Added By James kwon for STEMCO
                grpSingleCol.Enabled = false;
                lvSeparateCols.Enabled = true;
                btnLeft_SeparateCols.Enabled = true;
                btnRight_SeparateCols.Enabled = true;
                m_bSingle = false;
                m_bFactor = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void rdoSigleColumn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoSigleColumn.Checked)
                {
                    grpSingleCol.Enabled = true;
                    lvSeparateCols.Enabled = false;
                    btnLeft_SeparateCols.Enabled = false;
                    btnRight_SeparateCols.Enabled = false;
                    m_bSingle = true;
                    m_bFactor = false;
                }
                else
                {
                    grpSingleCol.Enabled = false;
                    lvSeparateCols.Enabled = true;
                    btnLeft_SeparateCols.Enabled = true;
                    btnRight_SeparateCols.Enabled = true;
                    m_bSingle = false;
                    m_bFactor = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [ CLOSING ]

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            #region m_Input 정리 및 체크
            if (!TrimInput())
                return;
            #endregion
            SaveSetting();
            if (On_SelectedAnova != null)
                On_SelectedAnova(m_Input);
            this.DialogResult = DialogResult.OK;
        }

        #endregion

        #region [ ListView DragDrop ]

        void lvSingleCol_DragDrop(object sender, DragEventArgs e)
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
                                if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[0]]).DataType.Equals(DataType.NUMBER))
                                {
                                    if (lvSingleCol.Items.Count > 0)
                                    {
                                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleCol, lvColumns, lvSingleCol.Items[0]);
                                    }
                                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSingleCol, lvColumns.SelectedItems[0]);
                                }
                                else
                                {
                                    MessageBox.Show(m_ErrorNumberColumn);
                                }
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

        void lvSeparateCols_DragDrop(object sender, DragEventArgs e)
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
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvSeparateCols, (IList)filteredItems);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvFactorCol_DragDrop(object sender, DragEventArgs e)
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
                                if (lvFactorCol.Items.Count > 0)
                                {
                                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvFactorCol, lvColumns, lvFactorCol.Items[0]);
                                }
                                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvFactorCol, lvColumns.SelectedItems[0]);
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

        void lvColumns_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                if (lvSingleCol.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSingleCol, lvColumns, (IList)lvSingleCol.SelectedItems);
                }
                else if (lvFactorCol.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvFactorCol, lvColumns, (IList)lvFactorCol.SelectedItems);
                }
                else if (lvSeparateCols.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSeparateCols, lvColumns, (IList)lvSeparateCols.SelectedItems);
                }
            }
        }

        #endregion

        #region [ ListView DragEnter ]

        void lvSingleCol_DragEnter(object sender, DragEventArgs e)
        {
            if (lvSingleCol.Enabled)
            {
                if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        void lvSeparateCols_DragEnter(object sender, DragEventArgs e)
        {
            if (lvSeparateCols.Enabled)
            {
                if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        void lvFactorCol_DragEnter(object sender, DragEventArgs e)
        {
            if (lvFactorCol.Enabled)
            {
                if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
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

        void lvSingleCol_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvSingleCol.DoDragDrop(lvSingleCol.SelectedItems, DragDropEffects.Move);
        }

        void lvSeparateCols_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvSeparateCols.DoDragDrop(lvSeparateCols.SelectedItems, DragDropEffects.Move);
        }

        void lvFactorCol_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvFactorCol.DoDragDrop(lvFactorCol.SelectedItems, DragDropEffects.Move);
        }

        void lvColumns_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvColumns.DoDragDrop(lvColumns.SelectedItems, DragDropEffects.Move);
        }

        #endregion

        #region [ ListView MouseDoubleClick ]

        void lvSingleCol_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvSingleCol.SelectedItems.Count > 0)
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleCol, lvColumns, lvSingleCol.SelectedItems[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvSeparateCols_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvSeparateCols.SelectedItems.Count > 0)
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSeparateCols, lvColumns, lvSeparateCols.SelectedItems[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvFactorCol_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvFactorCol.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvFactorCol, lvColumns, lvFactorCol.SelectedItems[0]);
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
                    if (m_bSingle && !m_bFactor)  // Single
                    {
                        if (lvSingleCol.Enabled)
                        {
                            if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[0]]).DataType.Equals(DataType.NUMBER))
                            {
                                if (lvSingleCol.Items.Count > 0)
                                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleCol, lvColumns, lvSingleCol.Items[0]);
                                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSingleCol, lvColumns.SelectedItems[0]);
                                m_bFactor = true;
                                m_bSingle = true;
                                return;
                            }
                        }
                    }
                    else if (m_bSingle && m_bFactor)  // Factor
                    {
                        if (lvFactorCol.Enabled)
                        {
                            if (lvFactorCol.Items.Count > 0)
                                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvFactorCol, lvColumns, lvFactorCol.Items[0]);
                            DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvFactorCol, lvColumns.SelectedItems[0]);
                            m_bFactor = false;
                            m_bSingle = true;
                            return;
                        }
                    }
                    else if (!m_bSingle)  // Responses
                    {
                        if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[0]]).DataType.Equals(DataType.NUMBER))
                            DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSeparateCols, lvColumns.SelectedItems[0]);
                        else
                            MessageBox.Show(m_ErrorNumberColumn);
                        return;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception("대체 어디로 넣어야 할지..김현태!!!!");
#endif
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListView Enter ]

        void lvSeparateCols_Enter(object sender, EventArgs e)
        {
            m_bSingle = false;
            m_bFactor = false;
        }

        void lvFactorCol_Enter(object sender, EventArgs e)
        {
            m_bSingle = true;
            m_bFactor = true;
        }

        void lvSingleCol_Enter(object sender, EventArgs e)
        {
            m_bSingle = true;
            m_bFactor = false;
        }

        #endregion

        #region [ Button Click ]

        #region Graph

        void btnGraph_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedAnovaGraph oSelectedAnovaGraph = new SelectedAnovaGraph();
                oSelectedAnovaGraph.IsBoxPlot = m_Input.IsBoxPlot;
                oSelectedAnovaGraph.IsGraphHistogramResiduals = m_Input.IsGraphHistogramResiduals;
                oSelectedAnovaGraph.IsGraphProbabilityPlotResiduals = m_Input.IsGraphProbabilityPlotResiduals;
                oSelectedAnovaGraph.IsGraphResidualsVsFittedValues = m_Input.IsGraphResidualsVsFittedValues;
                DlgANOVA_.DlgGraph oDlgGraph = new DACrux.BStats.StatDialog.DlgANOVA_.DlgGraph(oSelectedAnovaGraph);
                oDlgGraph.On_SelectedOptions += new DSelectedAnovaGraph(oDlgGraph_On_SelectedOptions);
                oDlgGraph.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void oDlgGraph_On_SelectedOptions(SelectedAnovaGraph oSelected)
        {
            try
            {
                m_Input.IsBoxPlot = oSelected.IsBoxPlot;
                m_Input.IsGraphHistogramResiduals = oSelected.IsGraphHistogramResiduals;
                m_Input.IsGraphProbabilityPlotResiduals = oSelected.IsGraphProbabilityPlotResiduals;
                m_Input.IsGraphResidualsVsFittedValues = oSelected.IsGraphResidualsVsFittedValues;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Left

        void btnLeft_SingleCol_Click(object sender, EventArgs e)
        {
            if (lvSingleCol.Items.Count > 0)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleCol, lvColumns, lvSingleCol.Items[0]);
            }
        }

        void btnLeft_SeparateCols_Click(object sender, EventArgs e)
        {
            if (lvSeparateCols.SelectedItems.Count > 0)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSeparateCols, lvColumns, (IList)lvSeparateCols.SelectedItems);
            }
        }

        void btnLeft_FactorCol_Click(object sender, EventArgs e)
        {
            if (lvFactorCol.Items.Count > 0)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvFactorCol, lvColumns, lvFactorCol.Items[0]);
            }
        }

        #endregion

        #region Right

        void btnRight_SingleCol_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                if (lvColumns.SelectedItems.Count == 1)
                {
                    if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[0]]).DataType.Equals(DataType.NUMBER))
                    {
                        if (lvSingleCol.Items.Count > 0)
                            DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleCol, lvColumns, lvSingleCol.Items[0]);
                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSingleCol, lvColumns.SelectedItems[0]);
                    }
                    else
                    {
                        MessageBox.Show(m_ErrorNumberColumn);
                    }

                    
                }
                else
                {
                    MessageBox.Show(m_ErrorOneColumn);
                }
            }
        }

        void btnRight_SeparateCols_Click(object sender, EventArgs e)
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
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvSeparateCols, (IList)filteredItems);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnRight_FactorCol_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                if (lvColumns.SelectedItems.Count == 1)
                {
                    if (lvFactorCol.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvFactorCol, lvColumns, lvFactorCol.Items[0]);
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvFactorCol, lvColumns.SelectedItems[0]);
                }
                else
                {
                    MessageBox.Show(m_ErrorOneColumn);
                }
            }
        }

        #endregion

        #endregion

        #region [ Inquery Option ]

        private void SetSetting()
        {
            MenuOption oMenuOption = null;
            inputANOVA oOption = null;
            try
            {
                oMenuOption = new MenuOption();
                oOption = oMenuOption.GetAnova();
                m_Input.IsSingle = oOption.IsSingle;
                rdoSigleColumn.Checked = m_Input.IsSingle;
                rdoSeparateCols.Checked = !m_Input.IsSingle;
                m_Input.IsBoxPlot = oOption.IsBoxPlot;
                m_Input.IsGraphHistogramResiduals = oOption.IsGraphHistogramResiduals;
                m_Input.IsGraphProbabilityPlotResiduals = oOption.IsGraphProbabilityPlotResiduals;
                m_Input.IsGraphResidualsVsFittedValues = oOption.IsGraphResidualsVsFittedValues;
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
                oMenuOption.UpdateAnova(m_Input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMenuOption = null;
            }
        }

        #endregion

        #region [ Inner Method ]

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

        private bool TrimInput()
        {
            int iColCnt;
            int iVarCnt;
            int iSelectedColCnt;
            int iMapping;
            int iMaxRow;
            int iRowCnt;
            #region 현재 선택된 컬럼 인덱스 리스트
            int[] arrVar = null;
            #endregion
            #region 정리된 DataTable에서 현재 선택된 컬럼 인덱스 리스트
            int[] arrMappingVar = null;
            #endregion
            #region DataTable의 선택 컬럼의 인덱스 리스트
            int[] arrTotal = null;
            #endregion
            try
            {

                #region Trim Valid Row

                iMaxRow = 0;
                
                m_Input.IsSingle = rdoSigleColumn.Checked;

                if (m_Input.IsSingle)
                {
                    if (lvSingleCol.Items.Count == 1)
                    {
                        if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSingleCol.Items[0]]).ValidRowIndex)
                            iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSingleCol.Items[0]]).ValidRowIndex;
                        if (lvFactorCol.Items.Count == 1)
                        {
                            if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvFactorCol.Items[0]]).ValidRowIndex)
                                iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvFactorCol.Items[0]]).ValidRowIndex;
                        }
                        else
                        {
                            MessageBox.Show(m_ErrorOneColumn2 + "(factor).");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(m_ErrorOneColumn2 + "(value).");
                        return false;
                    }
                }
                else
                {
                    iVarCnt = lvSeparateCols.Items.Count;
                    m_Input.arrValidRowIndices = new int[iVarCnt];
                    for (int i = 0; i < iVarCnt; i++)
                    {
                        m_Input.arrValidRowIndices[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSeparateCols.Items[i]]).ValidRowIndex;
                        if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSeparateCols.Items[i]]).ValidRowIndex)
                            iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSeparateCols.Items[i]]).ValidRowIndex;
                    }
                }

                iRowCnt = m_Input.DataSource.Rows.Count;
                for (int i = iRowCnt - 1; i > -1; i--)
                {
                    if (i > iMaxRow)
                        m_Input.DataSource.Rows.RemoveAt(i);
                }
                m_Input.DataSource.AcceptChanges();

                #endregion

                iSelectedColCnt = 0;

                if (m_Input.IsSingle)
                {

                    if (lvSingleCol.Items.Count==1)
                    {
                        m_Input.SingleColumn = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSingleCol.Items[0]]).ColumnIndex;
                        iSelectedColCnt++;
                    }
                    else
                    {
                        MessageBox.Show(m_ErrorOneColumn2 + "(value).");
                        return false;
                    }

                    if(lvFactorCol.Items.Count==1)
                    {
                        m_Input.GroupColumn = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvFactorCol.Items[0]]).ColumnIndex;
                        iSelectedColCnt++;
                    }
                    else
                    {
                        MessageBox.Show(m_ErrorOneColumn2 + "(factor).");
                        return false;
                    }
                    if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSingleCol.Items[0]]).ValidRowIndex != ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvFactorCol.Items[0]]).ValidRowIndex)
                    {
                        MessageBox.Show(m_ErrorLength);
                        return false;
                    }
                    arrTotal = new int[2];

                    #region arrTotal Make
                    arrTotal[0] = m_Input.SingleColumn;
                    arrTotal[1] = m_Input.GroupColumn;
                    #endregion

                    arrMappingVar = new int[2];

                    #region 선택한 컬럼만 남기고 나머지 컬럼삭제, 선택 인덱스 목록 맵핑
                    iColCnt = m_Input.DataSource.Columns.Count;
                    iMapping = 1;
                    for (int i = iColCnt - 1; i > -1; i--)
                    {
                        int iIndex = -1;
                        iIndex = Array.IndexOf(arrTotal, i);

                        if (iIndex != -1)
                        {
                            arrMappingVar[iIndex] = iMapping;
                            iMapping--;
                        }
                        else
                        {
                            m_Input.DataSource.Columns.RemoveAt(i);
                            m_Input.DataSource.AcceptChanges();
                        }
                    }

                    #region Mapping Value Input
                    m_Input.SingleColumn = arrMappingVar[0];
                    m_Input.GroupColumn = arrMappingVar[1];
                    #endregion

                    #endregion

                }
                else
                {
                    iVarCnt = lvSeparateCols.Items.Count;
                    if (iVarCnt > 0)
                    {
                        arrVar = new int[iVarCnt];
                        for (int i = 0; i < iVarCnt; i++)
                        {
                            arrVar[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSeparateCols.Items[i]]).ColumnIndex;
                        }
                        arrMappingVar = new int[iVarCnt];

                        #region 선택한 컬럼만 남기고 나머지 컬럼삭제, 선택 인덱스 목록 맵핑
                        iColCnt = m_Input.DataSource.Columns.Count;
                        iMapping = iVarCnt - 1;
                        for (int i = iColCnt - 1; i > -1; i--)
                        {
                            int iIndex = -1;
                            iIndex = Array.IndexOf(arrVar, i);

                            if (iIndex != -1)
                            {
                                arrMappingVar[iIndex] = iMapping;
                                iMapping--;
                            }
                            else
                            {
                                m_Input.DataSource.Columns.RemoveAt(i);
                                m_Input.DataSource.AcceptChanges();
                            }
                        }
                        m_Input.arrVariables = arrMappingVar;
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show(m_ErrorOneMoreColumn + "(value).");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                arrMappingVar = null;
                arrTotal = null;
                arrVar = null;
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
        }

        #endregion

        #endregion
    }
}