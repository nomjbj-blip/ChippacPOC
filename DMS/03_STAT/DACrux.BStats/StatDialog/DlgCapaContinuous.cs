using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using DACrux.ProjectManager.UI;
using DACrux.BStats.StatisticsInput;
using DACrux.BStats.StatDialog.DlgCapaContinuous_;
using DACrux.BStats.Core;


namespace DACrux.BStats.StatDialog
{
    

    public partial class DlgCapaContinuous : Form, iStatInformation
    {
        #region " MEMBER FIELD "

        #region [ LANGUAGE ]

        string m_Title = "Process Capability Analysis";
        string m_Columns = "Columns";
        string m_DataAreArranged = "Data are arranged as";
        string m_SingleColumn = "Single &column";
        string m_SubgroupSize = "Subgroup size";
        string m_IDcolumn = "ID column";
        string m_UseaConstant = "U&se a constant";
        string m_SubgroupsAcrossRows = "Su&bgroups across rows of";
        string m_LowerSpec = "&Lower spec";
        string m_UpperSpec = "&Upper spec";
        string m_HistoricalMean = "Historical &mean";
        string m_HistoricalStandardDeviations = "Historical standard deviations";
        string m_WithinSubgroup = "&Within subgroup";
        string m_BetweenSubgroups = "Betwee&n subgroups \r\n(or when subgroup size is 1)";
        string m_Optional = "(optional)";
        string m_Estimate = "&Estimate";
        string m_Options = "O&ptions";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";

        //MESSAGE
        string m_ErrorOneColumn = "Please select only one column.";
        string m_ErrorOneColumn2 = "Please select one column for ";
        string m_ErrorOneMoreColumn = "Please select one or more column for analysis.";
        string m_ErrorNumberColumn = "Please select number type column.";
        string m_ErrorNullSpec = "Please input any lower or upper specification limit.";
        string m_ErrorTarget = "Target is out of spec.";
        string m_ErrorUSLLSL = "USL must be greater than LSL.";
        string m_ErrorUSL = "Error USL.";
        string m_ErrorLSL = "Error LSL.";
        #endregion

        #region [ EVENT ]
        public event DSelectedCapaContinuous On_SelectedCapaContinuous;
        #endregion

        //lvColumns에서 Double Click 했을 때 가져다 놓을 곳...
        private bool m_lvSubgroupAcrossRows = false;
        private bool m_lvSingleVariables = true;
        private bool m_lvSubgroupColumn = false;


        private inputCapaContinuous m_Input;

        private System.Collections.Hashtable htColumns = null; 
        
        #endregion

        #region " CREATOR "
        public DlgCapaContinuous(DataTable dtSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo, string strProject, string strWorkSheet, string strTitle, string strResultFilePath, string strUser)
        {
            InitializeComponent();
            m_Input = new inputCapaContinuous(strTitle, strResultFilePath);
            m_Input.DataSource = dtSource;
            m_Input.Title = strTitle;
            m_Input.Project = strProject;
            m_Input.WorkSheet = strWorkSheet;
            m_Input.User = strUser;            
            InitDialog();
            SetSetting();
            SetColumnListView(lstValidColumnInfo);
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
                this.lblDataArrangedAs.Text = m_DataAreArranged;
                this.rdoSigleColumn.Text = m_SingleColumn;
                this.grpSubgroupSize.Text = m_SubgroupSize;
                this.lblIDcolumn.Text = m_IDcolumn;
                this.chkConstant.Text = m_UseaConstant;
                this.rdoSubgroupAcrossRows.Text = m_SubgroupsAcrossRows;
                this.lblLowerSpec.Text = m_LowerSpec;
                this.lblUpperSpec.Text = m_UpperSpec;
                this.lblHistoricalMean.Text = m_HistoricalMean;
                this.lblHistoricalStandardDeviations.Text = m_HistoricalStandardDeviations;
                this.lblBetweenSubgroups.Text = m_BetweenSubgroups;
                this.lblWithinSubgroup.Text = m_WithinSubgroup;
                this.lblHistoricalMeanOptional.Text = m_Optional;
                this.lblWithinOptional.Text = m_Optional;
                this.lblBetweenOptional.Text = m_Optional;

                this.btnEstimate.Text = m_Estimate;
                this.btnOptions.Text = m_Options;
                this.btnOk.Text = m_Ok;
                this.btnCancel.Text = m_Cancel;


                lvColumns.SmallImageList = imlColumnType;
                lvSingleVariables.SmallImageList = imlColumnType;
                lvSubgroupColumn.SmallImageList = imlColumnType;
                lvSubgroupAcrossRows.SmallImageList = imlColumnType;

                lvColumns.AllowDrop = true;
                lvSingleVariables.AllowDrop = true;
                lvSubgroupColumn.AllowDrop = true;
                lvSubgroupAcrossRows.AllowDrop = true;

                lvColumns.MultiSelect = true;
                lvSingleVariables.MultiSelect = true;
                lvSubgroupColumn.MultiSelect = true;
                lvSubgroupAcrossRows.MultiSelect = true;

                
                this.lvSubgroupAcrossRows.Enter += new EventHandler(lvSubgroupAcrossRows_Enter);
                this.lvSingleVariables.Enter += new EventHandler(lvSingleVariables_Enter);
                this.lvSubgroupColumn.Enter += new EventHandler(lvSubgroupColumn_Enter);


                this.btnRight_SingleVar.Click += new EventHandler(btnRight_SingleVar_Click);
                this.btnRight_SubgroupColumn.Click += new EventHandler(btnRight_SubgroupColumn_Click);
                this.btnRight_SubgroupsAcrossRows.Click += new EventHandler(btnRight_SubgroupsAcrossRows_Click);

                this.btnLeft_SingleVar.Click += new EventHandler(btnLeft_SingleVar_Click);
                this.btnLeft_SubgroupColumn.Click += new EventHandler(btnLeft_SubgroupColumn_Click);
                this.btnLeft_SubgroupsAcrossRows.Click += new EventHandler(btnLeft_SubgroupsAcrossRows_Click);


                this.lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);
                this.lvSingleVariables.MouseDoubleClick += new MouseEventHandler(lvSingleVariables_MouseDoubleClick);
                this.lvSubgroupAcrossRows.MouseDoubleClick += new MouseEventHandler(lvSubgroupAcrossRows_MouseDoubleClick);
                this.lvSubgroupColumn.MouseDoubleClick += new MouseEventHandler(lvSubgroupColumn_MouseDoubleClick);

                this.lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
                this.lvSingleVariables.ItemDrag += new ItemDragEventHandler(lvSingleVariables_ItemDrag);
                this.lvSubgroupAcrossRows.ItemDrag += new ItemDragEventHandler(lvSubgroupAcrossRows_ItemDrag);
                this.lvSubgroupColumn.ItemDrag += new ItemDragEventHandler(lvSubgroupColumn_ItemDrag);

                this.lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
                this.lvSingleVariables.DragEnter += new DragEventHandler(lvSingleVariables_DragEnter);
                this.lvSubgroupAcrossRows.DragEnter += new DragEventHandler(lvSubgroupAcrossRows_DragEnter);
                this.lvSubgroupColumn.DragEnter += new DragEventHandler(lvSubgroupColumn_DragEnter);

                this.lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
                this.lvSingleVariables.DragDrop += new DragEventHandler(lvSingleVariables_DragDrop);
                this.lvSubgroupAcrossRows.DragDrop += new DragEventHandler(lvSubgroupAcrossRows_DragDrop);
                this.lvSubgroupColumn.DragDrop += new DragEventHandler(lvSubgroupColumn_DragDrop);


                this.btnOk.Click += new EventHandler(btnOk_Click);
                this.btnOptions.Click += new EventHandler(btnOptions_Click);
                this.btnCancel.Click += new EventHandler(btnCancel_Click);
                this.btnEstimate.Click += new EventHandler(btnEstimate_Click);

                this.chkConstant.CheckedChanged += new EventHandler(chkConstant_CheckedChanged);

                this.rdoSigleColumn.CheckedChanged += new EventHandler(rdoSigleColumn_CheckedChanged);

                this.txtLowerSpec.Leave += new EventHandler(txtLowerSpec_Leave);
                this.txtUpperSpec.Leave += new EventHandler(txtUpperSpec_Leave);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        #region [ Button Click ]

        #region Left

        void btnLeft_SubgroupsAcrossRows_Click(object sender, EventArgs e)
        {
            if (lvSubgroupAcrossRows.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSubgroupAcrossRows, lvColumns, (IList)lvSubgroupAcrossRows.SelectedItems);
        }

        void btnLeft_SubgroupColumn_Click(object sender, EventArgs e)
        {
            //if (lvSubgroupColumn.SelectedItems.Count > 0)
            //    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSubgroupColumn, lvColumns, (IList)lvSubgroupColumn.SelectedItems);
            //선택 안되어 있어도 이동할수 있도록 변경. 하나밖에 없으므로..

            if(lvSubgroupColumn.Items.Count == 1)
                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSubgroupColumn, lvColumns, lvSubgroupColumn.Items[0]);
        }

        void btnLeft_SingleVar_Click(object sender, EventArgs e)
        {
            //if (lvSingleVariables.SelectedItems.Count > 0)
            //    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSingleVariables, lvColumns, (IList)lvSingleVariables.SelectedItems);
            //선택 안되어 있어도 이동할수 있도록 변경. 하나밖에 없으므로..
            if (lvSingleVariables.Items.Count == 1)
                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleVariables, lvColumns, lvSingleVariables.Items[0]);
        }

        #endregion

        #region Right

        void btnRight_SubgroupsAcrossRows_Click(object sender, EventArgs e)
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
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvSubgroupAcrossRows, (IList)filteredItems);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnRight_SubgroupColumn_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                if (lvColumns.SelectedItems.Count == 1)
                {
                    if (lvSubgroupColumn.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSubgroupColumn, lvColumns, lvSubgroupColumn.Items[0]);
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSubgroupColumn, lvColumns.SelectedItems[0]);
                }
                else
                {
                    MessageBox.Show(m_ErrorOneColumn);
                }
            }
        }

        void btnRight_SingleVar_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                if (lvColumns.SelectedItems.Count == 1)
                {
                    if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[0]]).DataType.Equals(DataType.NUMBER))
                    {
                        if (lvSingleVariables.Items.Count > 0)
                            DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleVariables, lvColumns, lvSingleVariables.Items[0]);
                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSingleVariables, lvColumns.SelectedItems[0]);
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

        #endregion

        #endregion

        #region [ ListView Enter ]

        void lvSubgroupColumn_Enter(object sender, EventArgs e)
        {
            m_lvSingleVariables = false;
            m_lvSubgroupAcrossRows = false;
            m_lvSubgroupColumn = true;
        }

        void lvSingleVariables_Enter(object sender, EventArgs e)
        {
            m_lvSingleVariables = true;
            m_lvSubgroupAcrossRows = false;
            m_lvSubgroupColumn = false;
        }

        void lvSubgroupAcrossRows_Enter(object sender, EventArgs e)
        {
            m_lvSingleVariables = false;
            m_lvSubgroupAcrossRows = true;
            m_lvSubgroupColumn = false;
        }

        #endregion

        #region [ ListView DragDrop ]

        void lvSubgroupColumn_DragDrop(object sender, DragEventArgs e)
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
                                if (lvSubgroupColumn.Items.Count > 0)
                                {
                                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSubgroupColumn, lvColumns, lvSubgroupColumn.Items[0]);
                                }
                                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSubgroupColumn, lvColumns.SelectedItems[0]);
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

        void lvSubgroupAcrossRows_DragDrop(object sender, DragEventArgs e)
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
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvSubgroupAcrossRows, (IList)filteredItems);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvSingleVariables_DragDrop(object sender, DragEventArgs e)
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
                                    if (lvSingleVariables.Items.Count > 0)
                                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleVariables, lvColumns, lvSingleVariables.Items[0]);
                                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSingleVariables, lvColumns.SelectedItems[0]);
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

        void lvColumns_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                if (lvSingleVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSingleVariables, lvColumns, (IList)lvSingleVariables.SelectedItems);
                }
                else if (lvSubgroupColumn.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSubgroupColumn, lvColumns, (IList)lvSubgroupColumn.SelectedItems);
                }
                else if (lvSubgroupAcrossRows.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSubgroupAcrossRows, lvColumns, (IList)lvSubgroupAcrossRows.SelectedItems);
                }
            }
        }

        #endregion

        #region [ ListView DragEnter ]

        void lvSubgroupColumn_DragEnter(object sender, DragEventArgs e)
        {
            if (lvSubgroupColumn.Enabled)
            {
                if (lvSubgroupColumn.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvSubgroupAcrossRows.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvSingleVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    e.Effect = DragDropEffects.None;
                }
                else
                    e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        void lvSubgroupAcrossRows_DragEnter(object sender, DragEventArgs e)
        {
            if (lvSubgroupColumn.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvSubgroupAcrossRows.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvSingleVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;
        }

        void lvSingleVariables_DragEnter(object sender, DragEventArgs e)
        {
            if (lvSubgroupColumn.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvSubgroupAcrossRows.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvSingleVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;
        }

        void lvColumns_DragEnter(object sender, DragEventArgs e)
        {
            if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;
        }

        #endregion

        #region [ ListView ItemDrag ]

        void lvSubgroupColumn_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvSubgroupColumn.DoDragDrop(lvSubgroupColumn.SelectedItems, DragDropEffects.Move);
        }

        void lvSubgroupAcrossRows_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvSubgroupAcrossRows.DoDragDrop(lvSubgroupAcrossRows.SelectedItems, DragDropEffects.Move);
        }

        void lvSingleVariables_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvSingleVariables.DoDragDrop(lvSingleVariables.SelectedItems, DragDropEffects.Move);
        }

        void lvColumns_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvColumns.DoDragDrop(lvColumns.SelectedItems, DragDropEffects.Move);
        }
        #endregion

        #region [ ListView MouseDoubleClick ]

        void lvSubgroupColumn_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvSubgroupColumn.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSubgroupColumn, lvColumns, lvSubgroupColumn.SelectedItems[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvSubgroupAcrossRows_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvSubgroupAcrossRows.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSubgroupAcrossRows, lvColumns, lvSubgroupAcrossRows.SelectedItems[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvSingleVariables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvSingleVariables.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleVariables, lvColumns, lvSingleVariables.SelectedItems[0]);
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
                    if (m_lvSubgroupColumn && !m_lvSubgroupAcrossRows && !m_lvSingleVariables)
                    {
                        if (lvSubgroupColumn.Enabled)
                        {
                            if (lvSubgroupColumn.Items.Count > 0)
                                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSubgroupColumn, lvColumns, lvSubgroupColumn.Items[0]);
                            DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSubgroupColumn, lvColumns.SelectedItems[0]);
                            m_lvSingleVariables = false;
                            m_lvSubgroupAcrossRows = true;
                            m_lvSubgroupColumn = false;
                        }
                        return;
                    }
                    else if (!m_lvSubgroupColumn && m_lvSubgroupAcrossRows && !m_lvSingleVariables)
                    {
                        if (lvSubgroupAcrossRows.Enabled)
                        {
                            if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[0]]).DataType.Equals(DataType.NUMBER))
                                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSubgroupAcrossRows, lvColumns.SelectedItems[0]);
                            else
                                MessageBox.Show(m_ErrorNumberColumn);
                        }
                        return;
                    }
                    else if (!m_lvSubgroupColumn && !m_lvSubgroupAcrossRows && m_lvSingleVariables)
                    {
                        if (((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.SelectedItems[0]]).DataType.Equals(DataType.NUMBER))
                        {
                            if (lvSingleVariables.Items.Count > 0)
                                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSingleVariables, lvColumns, lvSingleVariables.Items[0]);
                            DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSingleVariables, lvColumns.SelectedItems[0]);
                            m_lvSingleVariables = false;
                            m_lvSubgroupAcrossRows = false;
                            m_lvSubgroupColumn = true;
                        }
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
        
        #region [ Inquery Option ]

        private void SetSetting()
        {
            MenuOption oMenuOption = null;
            inputCapaContinuous oOption = null;
            try
            {
                oMenuOption = new MenuOption();
                oOption = oMenuOption.GetCapaContinuous();
                m_Input.ConfidenceLevel = oOption.ConfidenceLevel;
                m_Input.EstimationBetweenSubgroup = oOption.EstimationBetweenSubgroup;
                m_Input.EstimationWithinSubgroup = oOption.EstimationWithinSubgroup;
                m_Input.HistoricalMean = oOption.HistoricalMean;
                m_Input.HistoricalStdBetween = oOption.HistoricalStdBetween;
                m_Input.HistoricalStdWithin = oOption.HistoricalStdWithin;
                m_Input.IsBetweenWithinAnalysis = oOption.IsBetweenWithinAnalysis;
                m_Input.IsIncludeConfidenceIntervals = oOption.IsIncludeConfidenceIntervals;
                m_Input.IsOverallAnalysis = oOption.IsOverallAnalysis;
                m_Input.IsUseContantForSubgroupSize = oOption.IsUseContantForSubgroupSize;
                m_Input.IsUseUnbiasingConstantsOverall = oOption.IsUseUnbiasingConstantsOverall;
                m_Input.IsUseUnbiasingConstantsWithin = oOption.IsUseUnbiasingConstantsWithin;
                m_Input.Ksigma = oOption.Ksigma;
                m_Input.LSL = oOption.LSL;
                m_Input.MovingRangeofLength = oOption.MovingRangeofLength;
                m_Input.ResultConfidenceIntervalsType = oOption.ResultConfidenceIntervalsType;
                m_Input.ResultDisplayType = oOption.ResultDisplayType;
                m_Input.ResultStatisticType = oOption.ResultStatisticType;
                m_Input.SubGroupSize = oOption.SubGroupSize;
                m_Input.Target = oOption.Target;
                m_Input.UserTitle = oOption.UserTitle;
                m_Input.USL = oOption.USL;

                chkConstant.Checked = m_Input.IsUseContantForSubgroupSize;

                nudConstant.Enabled = m_Input.IsUseContantForSubgroupSize;
                btnLeft_SubgroupColumn.Enabled = !m_Input.IsUseContantForSubgroupSize;
                btnRight_SubgroupColumn.Enabled = !m_Input.IsUseContantForSubgroupSize;
                lvSubgroupColumn.Enabled = !m_Input.IsUseContantForSubgroupSize;

                nudConstant.Value = Convert.ToDecimal(m_Input.SubGroupSize);
                if (double.IsNaN(m_Input.LSL))
                    txtLowerSpec.Text = string.Empty;
                else
                    txtLowerSpec.Text = m_Input.LSL.ToString();
                if (double.IsNaN(m_Input.USL))
                    txtUpperSpec.Text = string.Empty;
                else
                    txtUpperSpec.Text = m_Input.USL.ToString();
                if (double.IsNaN(m_Input.HistoricalMean))
                    txtHistoricalMean.Text = string.Empty;
                else
                    txtHistoricalMean.Text = m_Input.HistoricalMean.ToString();
                if (double.IsNaN(m_Input.HistoricalStdWithin))
                    txtWithinSubgroup.Text = string.Empty;
                else
                    txtWithinSubgroup.Text = m_Input.HistoricalStdWithin.ToString();
                if (double.IsNaN(m_Input.HistoricalStdBetween))
                    txtBetweenSubgroups.Text = string.Empty;
                else
                    txtBetweenSubgroups.Text = m_Input.HistoricalStdBetween.ToString();

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
                oMenuOption.UpdateCapaContinuous(m_Input);
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

                m_Input.IsUseContantForSubgroupSize = chkConstant.Checked;

                if (rdoSigleColumn.Checked)
                {
                    if (lvSingleVariables.Items.Count > 0)
                    {
                        if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSingleVariables.Items[0]]).ValidRowIndex)
                            iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSingleVariables.Items[0]]).ValidRowIndex;
                        if (!m_Input.IsUseContantForSubgroupSize)
                        {
                            if (lvSubgroupColumn.Items.Count > 0)
                            {
                                if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSubgroupColumn.Items[0]]).ValidRowIndex)
                                    iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSubgroupColumn.Items[0]]).ValidRowIndex;
                            }
                            else
                            {
                                MessageBox.Show(m_ErrorOneColumn2 + "(sub group).");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(m_ErrorOneMoreColumn+"(value).");
                        return false;
                    }
                }
                else
                {
                    iVarCnt = lvSubgroupAcrossRows.Items.Count;
                    for (int i = 0; i < iVarCnt; i++)
                    {
                        if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSubgroupAcrossRows.Items[i]]).ValidRowIndex)
                            iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSubgroupAcrossRows.Items[i]]).ValidRowIndex;
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

                if (rdoSigleColumn.Checked)
                {

                    if (lvSingleVariables.Items.Count > 0)
                    {
                        m_Input.SingleColumnIndex = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSingleVariables.Items[0]]).ColumnIndex;
                        iSelectedColCnt++;
                    }
                    else
                    {
                        MessageBox.Show(m_ErrorOneColumn2 + "(value).");
                        return false;
                    }

                    
                    if (m_Input.IsUseContantForSubgroupSize)
                        m_Input.SubGroupSize = Convert.ToInt32(nudConstant.Value);
                    else
                    {
                        if (lvSubgroupColumn.Items.Count > 0)
                        {
                            m_Input.SubgroupColumnIndex = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSubgroupColumn.Items[0]]).ColumnIndex;
                            m_Input.IsSubgroupColumn = true;
                            iSelectedColCnt++;
                        }
                        else
                        {
                            m_Input.IsUseContantForSubgroupSize = true; //자동으로 1로 해주는 것으로 수정
                            m_Input.SubGroupSize = 1;  //자동으로 1로 해주는 것으로 수정
                            m_Input.IsSubgroupColumn = false;
                        }
                    }
                    arrTotal = new int[iSelectedColCnt];

                    #region arrTotal Make
                    if (lvSingleVariables.Items.Count > 0)
                    {
                        arrTotal[0] = m_Input.SingleColumnIndex;
                        if (!m_Input.IsUseContantForSubgroupSize)
                        {
                            if (lvSubgroupColumn.Items.Count > 0)
                            {
                                arrTotal[1] = m_Input.SubgroupColumnIndex;
                            }
                        }
                    }
                    else
                    {
                        if (!m_Input.IsUseContantForSubgroupSize)
                        {
                            if (lvSubgroupColumn.Items.Count > 0)
                            {
                                arrTotal[0] = m_Input.SubgroupColumnIndex;
                            }
                        }
                    }
                    #endregion

                    arrMappingVar = new int[iSelectedColCnt];                   

                    #region 선택한 컬럼만 남기고 나머지 컬럼삭제, 선택 인덱스 목록 맵핑
                    iColCnt = m_Input.DataSource.Columns.Count;
                    iMapping = iSelectedColCnt - 1;
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
                    if (lvSingleVariables.Items.Count > 0)
                    {
                        m_Input.SingleColumnIndex = arrMappingVar[0];
                        if (!m_Input.IsUseContantForSubgroupSize)
                        {
                            if (lvSubgroupColumn.Items.Count > 0)
                                m_Input.SubgroupColumnIndex = arrMappingVar[1];
                        }
                    }
                    else
                    {
                        if (!m_Input.IsUseContantForSubgroupSize)
                        {
                            if (lvSubgroupColumn.Items.Count > 0)
                                m_Input.SubgroupColumnIndex = arrMappingVar[0];
                        }
                    }
                    #endregion

                    #endregion

                }
                else
                {
                    iVarCnt = lvSubgroupAcrossRows.Items.Count;
                    if (iVarCnt > 0)
                    {
                        arrVar = new int[iVarCnt];
                        for (int i = 0; i < iVarCnt; i++)
                        {
                            arrVar[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvSubgroupAcrossRows.Items[i]]).ColumnIndex;
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

                //m_Input.DataSource.AcceptChanges();
                //int iRowCnt = m_Input.DataSource.Rows.Count;
                //for (int i = iRowCnt - 1; i > -1; i--)
                //{
                //    if(m_Input.DataSource.Rows[i].ItemArray
                //}

                #region etc

                if (txtLowerSpec.Text.Trim() != string.Empty)
                    m_Input.LSL = Convert.ToDouble(txtLowerSpec.Text.Trim());
                else
                    m_Input.LSL = double.NaN;

                if (txtUpperSpec.Text.Trim() != string.Empty)
                    m_Input.USL = Convert.ToDouble(txtUpperSpec.Text.Trim());
                else
                    m_Input.USL = double.NaN;

                if (txtHistoricalMean.Text.Trim() != string.Empty)
                    m_Input.HistoricalMean = Convert.ToDouble(txtHistoricalMean.Text.Trim());
                else
                    m_Input.HistoricalMean = double.NaN;

                if (txtWithinSubgroup.Text.Trim() != string.Empty)
                    m_Input.HistoricalStdWithin = Convert.ToDouble(txtWithinSubgroup.Text.Trim());
                else
                    m_Input.HistoricalStdWithin = double.NaN;

                if (txtBetweenSubgroups.Text.Trim() != string.Empty)
                    m_Input.HistoricalStdBetween = Convert.ToDouble(txtBetweenSubgroups.Text.Trim());
                else
                    m_Input.HistoricalStdBetween = double.NaN;

                #endregion

                #region Spec Check
                if (double.IsNaN(m_Input.USL) && double.IsNaN(m_Input.LSL))
                {
                    MessageBox.Show(m_ErrorNullSpec);
                    return false;
                }
                else
                {
                    if(double.IsNaN(m_Input.Target))
                    {
                        if (!double.IsNaN(m_Input.USL) && !double.IsNaN(m_Input.LSL))
                        {
                            if (m_Input.USL <= m_Input.LSL)
                            {
                                MessageBox.Show(m_ErrorUSLLSL);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (!double.IsNaN(m_Input.USL) && !double.IsNaN(m_Input.LSL))
                        {
                            if (m_Input.USL <= m_Input.LSL)
                            {
                                MessageBox.Show(m_ErrorUSLLSL);
                                return false;
                            }
                            if (m_Input.Target >= m_Input.USL || m_Input.Target <= m_Input.LSL)
                            {
                                MessageBox.Show(m_ErrorTarget);
                                return false;
                            }
                        }
                        else if (double.IsNaN(m_Input.USL) && !double.IsNaN(m_Input.LSL))
                        {
                            if (m_Input.Target <= m_Input.LSL)
                            {
                                MessageBox.Show(m_ErrorTarget);
                                return false;
                            }
                        }
                        else //if (!double.IsNaN(m_Input.USL) && double.IsNaN(m_Input.LSL))
                        {
                            if (m_Input.Target >= m_Input.USL)
                            {
                                MessageBox.Show(m_ErrorTarget);
                                return false;
                            }
                        }
                    }

                }               
                #endregion

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

        #endregion

        #region " EVENT HANDLER "

        #region [ SPEC INPUT ]

        void txtUpperSpec_Leave(object sender, EventArgs e)
        {
            string strText = string.Empty;
            double dblValue;
            try
            {
                strText = txtUpperSpec.Text.Trim();
                if (strText == string.Empty)
                    return;
                if (!double.TryParse(strText, out dblValue))
                {
                    MessageBox.Show(m_ErrorUSL);
                    txtUpperSpec.Focus();
                    txtUpperSpec.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void txtLowerSpec_Leave(object sender, EventArgs e)
        {
            string strText = string.Empty;
            double dblValue;
            try
            {
                strText = txtLowerSpec.Text.Trim();
                if (strText == string.Empty)
                    return;
                if (!double.TryParse(strText, out dblValue))
                {
                    MessageBox.Show(m_ErrorLSL);
                    txtLowerSpec.Focus();
                    txtLowerSpec.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        void rdoSigleColumn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoSigleColumn.Checked)
                {
                    lvSingleVariables.Enabled = true;
                    grpSubgroupSize.Enabled = true;
                    btnLeft_SingleVar.Enabled = true;
                    btnRight_SingleVar.Enabled = true;
                    lvSubgroupAcrossRows.Enabled = false;
                    btnLeft_SubgroupsAcrossRows.Enabled = false;
                    btnRight_SubgroupsAcrossRows.Enabled = false;
                    //if (m_lvSubgroupAcrossRows)
                    //{
                    m_lvSubgroupAcrossRows = false;
                    m_lvSingleVariables = true;
                    m_lvSubgroupColumn = false;
                    //}

                    if (lvSubgroupAcrossRows.Items.Count > 0)
                    {
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSubgroupAcrossRows, lvColumns, (IList)lvSubgroupAcrossRows.Items);
                    }
                }
                else
                {
                    lvSingleVariables.Enabled = false;
                    grpSubgroupSize.Enabled = false;
                    btnLeft_SingleVar.Enabled = false;
                    btnRight_SingleVar.Enabled = false;
                    lvSubgroupAcrossRows.Enabled = true;
                    btnLeft_SubgroupsAcrossRows.Enabled = true;
                    btnRight_SubgroupsAcrossRows.Enabled = true;
                    //if (m_lvSingleVariables || m_lvSubgroupColumn)
                    //{
                    m_lvSingleVariables = false;
                    m_lvSubgroupColumn = false;
                    m_lvSubgroupAcrossRows = true;
                    //}

                    if (lvSingleVariables.Items.Count > 0)
                    {
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSingleVariables, lvColumns, (IList)lvSingleVariables.Items);
                    }

                    if (lvSubgroupColumn.Items.Count > 0)
                    {
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSubgroupColumn, lvColumns, (IList)lvSubgroupColumn.Items);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }                

        #region [ USE CONSTANT ]
        void chkConstant_CheckedChanged(object sender, EventArgs e)
        {
            bool bCheck;
            try
            {
                bCheck = chkConstant.Checked;
                nudConstant.Enabled = bCheck;
                btnLeft_SubgroupColumn.Enabled = !bCheck;
                btnRight_SubgroupColumn.Enabled = !bCheck;
                lvSubgroupColumn.Enabled = !bCheck;
                if (bCheck)
                {
                    if (lvSubgroupColumn.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSubgroupColumn, lvColumns, lvSubgroupColumn.Items[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ OPTION BUTTON ]

        void btnEstimate_Click(object sender, EventArgs e)
        {
            SelectedCapaContinuousEstimate oSelectedCapaContinuousEstimate = new SelectedCapaContinuousEstimate();
            //oSelectedCapaContinuousEstimate.Reset();
            oSelectedCapaContinuousEstimate.EstimationBetweenSubgroup = m_Input.EstimationBetweenSubgroup;
            oSelectedCapaContinuousEstimate.EstimationWithinSubgroup = m_Input.EstimationWithinSubgroup;
            oSelectedCapaContinuousEstimate.IsUseUnbiasingOverall = m_Input.IsUseUnbiasingConstantsOverall;
            oSelectedCapaContinuousEstimate.IsUseUnbiasingWithin = m_Input.IsUseUnbiasingConstantsWithin;
            oSelectedCapaContinuousEstimate.MovingRangeofLength = m_Input.MovingRangeofLength;


            DlgEstimationofStandardDeviation oDlgEstimationofStandardDeviation = new DlgEstimationofStandardDeviation(oSelectedCapaContinuousEstimate);
            oDlgEstimationofStandardDeviation.On_CapaContinuousEstimation += new DSelectedCapaContinuousEstimation(oDlgEstimationofStandardDeviation_On_CapaContinuousEstimation);
            oDlgEstimationofStandardDeviation.ShowDialog();
        }

        void oDlgEstimationofStandardDeviation_On_CapaContinuousEstimation(SelectedCapaContinuousEstimate oSelected)
        {
            m_Input.EstimationBetweenSubgroup = oSelected.EstimationBetweenSubgroup;
            m_Input.EstimationWithinSubgroup = oSelected.EstimationWithinSubgroup;
            m_Input.IsUseUnbiasingConstantsOverall = oSelected.IsUseUnbiasingOverall;
            m_Input.IsUseUnbiasingConstantsWithin = oSelected.IsUseUnbiasingWithin;
            m_Input.MovingRangeofLength = oSelected.MovingRangeofLength;
        }

        void btnOptions_Click(object sender, EventArgs e)
        {
            SelectedCapaContinuousOptions oSelectedCapaContinuousOptions = new SelectedCapaContinuousOptions();
            //oSelectedCapaContinuousOptions.Reset();
            oSelectedCapaContinuousOptions.ConfidenceIntervals = m_Input.ResultConfidenceIntervalsType;
            oSelectedCapaContinuousOptions.ConfidenceLevel = m_Input.ConfidenceLevel;
            oSelectedCapaContinuousOptions.IsBetweenWithinAnalysis = m_Input.IsBetweenWithinAnalysis;
            oSelectedCapaContinuousOptions.IsIncludeConfidenceIntervals = m_Input.IsIncludeConfidenceIntervals;
            oSelectedCapaContinuousOptions.IsOverallAnalysis = m_Input.IsOverallAnalysis;
            oSelectedCapaContinuousOptions.KsigmaForCapability = m_Input.Ksigma;
            oSelectedCapaContinuousOptions.ResultDisplayType = m_Input.ResultDisplayType;
            oSelectedCapaContinuousOptions.ResultStatisticType = m_Input.ResultStatisticType;
            oSelectedCapaContinuousOptions.Target = m_Input.Target;
            oSelectedCapaContinuousOptions.UserTitle = m_Input.UserTitle;

            DlgCapaContinuousOptions oDlgCapaContinuousOptions = new DlgCapaContinuousOptions(oSelectedCapaContinuousOptions);
            oDlgCapaContinuousOptions.On_SelectedOptions += new DSelectedCapaContinuousOptions(oDlgCapaContinuousOptions_On_SelectedOptions);
            oDlgCapaContinuousOptions.ShowDialog();
        }

        void oDlgCapaContinuousOptions_On_SelectedOptions(SelectedCapaContinuousOptions oSelected)
        {
            m_Input.ResultConfidenceIntervalsType = oSelected.ConfidenceIntervals;
            m_Input.ConfidenceLevel = oSelected.ConfidenceLevel;
            m_Input.IsBetweenWithinAnalysis = oSelected.IsBetweenWithinAnalysis;
            m_Input.IsIncludeConfidenceIntervals = oSelected.IsIncludeConfidenceIntervals;
            m_Input.IsOverallAnalysis = oSelected.IsOverallAnalysis;
            m_Input.Ksigma = oSelected.KsigmaForCapability;
            m_Input.ResultDisplayType = oSelected.ResultDisplayType;
            m_Input.ResultStatisticType = oSelected.ResultStatisticType;
            m_Input.Target = oSelected.Target;
            m_Input.UserTitle = oSelected.UserTitle;
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
            if (!TrimInput())
                return;
            #endregion
            SaveSetting();
            if(On_SelectedCapaContinuous != null)
                On_SelectedCapaContinuous(m_Input);
            this.DialogResult = DialogResult.OK;
        }


        #endregion

        #endregion


    }
}