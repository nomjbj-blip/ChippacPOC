using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DACrux.ProjectManager.UI;
using DACrux.BStats.Core;
using DACrux.BStats.StatisticsInput;

namespace DACrux.BStats.StatDialog
{
    

    public partial class DlgCorrelation : Form, iStatInformation
    {
        #region " MEMBER FIELD "

        public string FUNC_CODE = "F0302";

        #region [ LANGUAGE ]
        string m_Title = "Correlation";
        string m_Columns = "Columns";
        string m_Variables = "Variables";
        //string m_Regression = "&Regression equation";
        //string m_Pvalues = "&Display p-values";
        //string m_Scatter = "&Scatter plot";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";

        string m_ErrorTwoVar = "Please select 2 or more variables";
        string m_ErrorLength = "Length of selected variables doesn't equal.";

        #endregion

        #region [ EVENT ]
        public event DSelectedCorrelation On_SelectedCorrelation;
        #endregion

        private inputCorrelation m_Input;

        private System.Collections.Hashtable htColumns = null;

        private List<DACrux.ProjectManager.UI.DataView.ColumnInfo> m_lstValidColumnInfo;

        /// <summary>
        /// 선택 컬럼 옮길 곳
        /// </summary>
        private bool m_lvVariables = true;

        #endregion       

        #region " CREATOR "
        public DlgCorrelation(DataTable dtSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo, string strProject, string strWorkSheet, string strTitle, string strResultFilePath, string strUser)
        {
            InitializeComponent();
            m_lstValidColumnInfo = lstValidColumnInfo;
            m_Input = new inputCorrelation(strTitle, strResultFilePath);
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
            this.Text = m_Title;
            this.lblColumns.Text = m_Columns;
            this.lblVariables.Text = m_Variables;
            this.btnOk.Text = m_Ok;
            this.btnCancel.Text = m_Cancel;


            lvColumns.SmallImageList = imlColumnType;
            lvVariables.SmallImageList = imlColumnType;

            lvColumns.AllowDrop = true;
            lvVariables.AllowDrop = true;

            lvColumns.MultiSelect = true;
            lvVariables.MultiSelect = true;



            this.lvVariables.Enter += new EventHandler(lvVariables_Enter);


            this.btnRight_Var.Click += new EventHandler(btnRight_Var_Click);
            this.btnLeft_Var.Click += new EventHandler(btnLeft_Var_Click);

            this.lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);
            this.lvVariables.MouseDoubleClick += new MouseEventHandler(lvVariables_MouseDoubleClick);

            this.lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
            this.lvVariables.ItemDrag += new ItemDragEventHandler(lvVariables_ItemDrag);

            this.lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
            this.lvVariables.DragEnter += new DragEventHandler(lvVariables_DragEnter);

            this.lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
            this.lvVariables.DragDrop += new DragEventHandler(lvVariables_DragDrop);

            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        #region [ ListView Enter ]


        void lvVariables_Enter(object sender, EventArgs e)
        {
            m_lvVariables = true;
        }

        #endregion

        #region [ ListView DragDrop ]


        void lvVariables_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
                {
                    if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, (ListView)sender, (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
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
                    
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvVariables, (ListView)sender, (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListView Drag Enter ]

        
        void lvVariables_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (lvVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListViewItem Move Button ]

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

        #region [ Inquiry Option ]

        private void SetSetting()
        {
            MenuOption oMenuOption = null;
            inputCorrelation oOption = null;
            try
            {
                oMenuOption = new MenuOption();
                oOption = oMenuOption.GetCorrelation();
                m_Input.IsPvalue = oOption.IsPvalue;
                m_Input.IsRegression = oOption.IsRegression;
                m_Input.IsScatter = oOption.IsScatter;
                m_Input.IsSpearman = oOption.IsSpearman;
                chkPvalue.Checked = m_Input.IsPvalue;
                chkRegression.Checked = m_Input.IsRegression;
                chkScatter.Checked = m_Input.IsScatter;
                chkSpearman.Checked = m_Input.IsSpearman;
               
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
                oMenuOption.UpdateCorrelation(m_Input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ Set Columns ]
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

                    //if (lstColumnInfo[i].DataType == DataType.NUMBER)
                    //    lvItem.ImageKey = "NUMBER";
                    //else if (lstColumnInfo[i].DataType == DataType.TEXT)
                    //    lvItem.ImageKey = "TEXT";
                    //else if (lstColumnInfo[i].DataType == DataType.DATETIME)
                    //    lvItem.ImageKey = "DATETIME";  
                    // 숫자형 컬럼만 받도록 수정

                    if (lstColumnInfo[i].DataType == DataType.NUMBER)
                        lvItem.ImageKey = "NUMBER";
                    else
                    {
                        continue;
                    }

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
        #endregion

        #region [ Trim DataTable ]

        /// <summary>
        /// 선택한 컬럼으로만 DataTable정리하고 선택한 컬럼에 대한 정보 정리
        /// </summary>
        private void TrimInput()
        {
            int iColCnt;
            int iVarCnt;
            int iMapping;
            int iRowCnt;
            int iMaxRow;
            #region 현재 선택된 컬럼 인덱스 리스트
            int[] arrVar = null;
            #endregion
            #region 정리된 DataTable에서 현재 선택된 컬럼 인덱스 리스트
            int[] arrMappingVar = null;
            #endregion
            
            try
            {
                
                iVarCnt = lvVariables.Items.Count;

                #region Trim Valid Row

                iMaxRow = 0;
                for (int i = 0; i < iVarCnt; i++)
                {
                    if (iMaxRow < ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ValidRowIndex)
                        iMaxRow = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ValidRowIndex;
                }
                iRowCnt = m_Input.DataSource.Rows.Count;
                for (int i = iRowCnt - 1; i > -1; i--)
                {
                    if (i > iMaxRow)
                        m_Input.DataSource.Rows.RemoveAt(i);
                }
                m_Input.DataSource.AcceptChanges();
                
                #endregion

                arrVar = new int[iVarCnt];
                for (int i = 0; i < iVarCnt; i++)
                {
                    arrVar[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ColumnIndex;
                }               
                arrMappingVar = new int[iVarCnt];

                #region 선택한 컬럼만 남기고 나머지 컬럼삭제, 선택 인덱스 목록 맵핑
                iColCnt = m_Input.DataSource.Columns.Count;
                iMapping = iVarCnt-1;
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

        #region Check Length of Columns

        //private bool CheckColumns(List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo)
        private bool CheckColumns()
        {
            //int[] arrVar = null;
            int iVarCnt;
            int iTemp;
            try
            {
                iVarCnt = lvVariables.Items.Count;
                if (iVarCnt < 2)
                {
                    MessageBox.Show(m_ErrorTwoVar);
                    return false;
                }
                
                //arrVar = new int[iVarCnt];
                //iTemp = lstValidColumnInfo[((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[0]]).ColumnIndex].ValidRowIndex;
                iTemp = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[0]]).ValidRowIndex;
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

        #endregion

        #region " EVENT HANDLER "

        #region [ CLOSING ]

        private void btnOk_Click(object sender, EventArgs e)
        {
            #region m_Input 정리 및 컬럼길이 체크
            if (!CheckColumns())
            {
                //MessageBox.Show("Length of Selected Columns doesn't equal");
                return;
            }

            TrimInput();
            m_Input.IsPvalue = chkPvalue.Checked;
            m_Input.IsSpearman = chkSpearman.Checked;
            m_Input.IsRegression = chkRegression.Checked;
            m_Input.IsScatter = chkScatter.Checked;
            
            #endregion
            SaveSetting();
            On_SelectedCorrelation(m_Input);
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