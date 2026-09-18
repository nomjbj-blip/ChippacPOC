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
    public partial class DlgHypothesisTesting : Form, iStatInformation
    {
        #region " MEMBER FIELD "
        string m_Title = "Hypothesis Testing";
        string m_OneSampleZText = "1-Sample &Z...";
        string m_OneSampleTText = "&1-Sample t...";
        string m_TwoSampleUnpaired = "&2-Sample t...";
        string m_TwoSamplePaired = "&Paired t...";
        string m_Columns = "Columns";
        string m_Variables = "Variables";

        string m_Methods = "&Methods";
        string m_Options = "O&ptions";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";

        //string m_ErrorDiff = "Use a single numeric constant(Test difference).";
        string m_ErrorOneMoreColumn = "Please select one or more column for analysis(1 sample)";
        string m_ErrorTwoMoreColumn = "Please select two or more column for analysis(2 sample)";
        string m_ErrorLengthPaired = "The following must be the same length";

        //string m_QuestionMean = "평균을 입력하지 않으면 Data로 부터 산출된 평균을 검정에 사용합니다.";
        //string m_QuestionMeanStd = "평균 또는 표준편차을 입력하지 않으면 Data로 부터 산출된 평균 또는 표준편차를 검정에 사용합니다.";
        //string m_QuestionDifference = "두 그룹간의 차이를 입력하지 않으면 차이를 0으로 두고 검정합니다.";
        string m_QuestionMean = "If you do not input the mean, it will be calculated from data.";
        string m_QuestionMeanStd = "If you do not input the mean or standard deviation, it will be calculated from data.";
        string m_QuestionDifference = "If you do not input the difference, the difference will be zero.";

        private inputHypothesisTesting m_Input;

        private System.Collections.Hashtable htColumns = null;

        #endregion


        #region " CREATOR "

        public DlgHypothesisTesting(DataTable dtSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo, string strProject, string strWorkSheet, string strTitle, string strResultFilePath, string strUser)
        {
            InitializeComponent();
            m_Input = new inputHypothesisTesting(strTitle, strResultFilePath);
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

        #region " EVENT "
        public event DSelectedHypothesisTesting On_Selected;
        #endregion


        #region " METHOD "

        private void InitDialog()
        {
            try
            {
                this.Text = m_Title;
                this.lblColumns.Text = m_Columns;
                this.lblVariables.Text = m_Variables;

                this.rdo1SampleT.Text = m_OneSampleTText;
                this.rdo1SampleZ.Text = m_OneSampleZText;
                this.rdo2SampleT.Text = m_TwoSampleUnpaired;
                this.rdoPairedT.Text = m_TwoSamplePaired;

                this.btnMethods.Text = m_Methods;
                this.btnOptions.Text = m_Options;
                this.btnOk.Text = m_Ok;
                this.btnCancel.Text = m_Cancel;

                lvColumns.SmallImageList = imlColumnType;
                lvVariables.SmallImageList = imlColumnType;

                lvColumns.AllowDrop = true;
                lvVariables.AllowDrop = true;

                lvColumns.MultiSelect = true;
                lvVariables.MultiSelect = true;


                this.lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);
                this.lvVariables.MouseDoubleClick += new MouseEventHandler(lvVariables_MouseDoubleClick);


                this.lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
                this.lvVariables.ItemDrag += new ItemDragEventHandler(lvVariables_ItemDrag);

                this.lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
                this.lvVariables.DragEnter += new DragEventHandler(lvVariables_DragEnter);


                this.lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
                this.lvVariables.DragDrop += new DragEventHandler(lvVariables_DragDrop);


                this.btnRight_Var.Click += new EventHandler(btnRight_Var_Click);
                this.btnLeft_Var.Click += new EventHandler(btnLeft_Var_Click);
                this.btnOk.Click +=new EventHandler(btnOk_Click);
                this.btnCancel.Click += new EventHandler(btnCancel_Click);
                this.btnMethods.Click +=new EventHandler(btnMethods_Click);
                this.btnOptions.Click+=new EventHandler(btnOptions_Click);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [ ListView DragDrop ]

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
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvVariables, (IList)filteredItems);
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
                if (lvVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvVariables, lvColumns, (IList)lvVariables.SelectedItems);
                }                
            }
        }

        #endregion

        #region [ ListView DragEnter ]

        void lvVariables_DragEnter(object sender, DragEventArgs e)
        {           
            if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
                e.Effect = DragDropEffects.None;           
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

        void lvVariables_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvVariables.DoDragDrop(lvVariables.SelectedItems, DragDropEffects.Move);
        }

        void lvColumns_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvColumns.DoDragDrop(lvColumns.SelectedItems, DragDropEffects.Move);
        }

        #endregion

        #region [ ListView MouseDoubleClick ]

        void lvVariables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvVariables.SelectedItems.Count > 0)
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvVariables, lvColumns, lvVariables.SelectedItems[0]);
                }
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
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvVariables, lvColumns.SelectedItems[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ Button Click ]

        void btnLeft_Var_Click(object sender, EventArgs e)
        {
            if (lvVariables.SelectedItems.Count > 0)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvVariables, lvColumns, (IList)lvVariables.SelectedItems);
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

        void btnOptions_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedHypothesisTestingOptions oSelectedHypothesisTestingOptions = new SelectedHypothesisTestingOptions();
                oSelectedHypothesisTestingOptions.IsConfidenceInterval = m_Input.IsConfidenceInterval;
                oSelectedHypothesisTestingOptions.IsDescriptiveStat = m_Input.IsDescriptiveStatistics;
                oSelectedHypothesisTestingOptions.IsCriticalValue = m_Input.IsCriticalValue;
                DlgHypothesisTesting_.DlgHypothesisOptions oDlg = new DACrux.BStats.StatDialog.DlgHypothesisTesting_.DlgHypothesisOptions(oSelectedHypothesisTestingOptions);
                oDlg.On_SelectedOptions += new DSelectedHypothesisTestingOptions(oDlg_On_SelectedOptions);
                oDlg.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void oDlg_On_SelectedOptions(SelectedHypothesisTestingOptions oSelected)
        {
            try
            {
                m_Input.IsConfidenceInterval = oSelected.IsConfidenceInterval;
                m_Input.IsDescriptiveStatistics = oSelected.IsDescriptiveStat;
                m_Input.IsCriticalValue = oSelected.IsCriticalValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnMethods_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedHypothesisTestingMethods oSelectedHypothesisTestingMethods = new SelectedHypothesisTestingMethods();
                oSelectedHypothesisTestingMethods.Alternative = m_Input.Alternative;
                oSelectedHypothesisTestingMethods.ConfidenceLevel = m_Input.ConfidenceLevel;
                oSelectedHypothesisTestingMethods.TestingDifference = m_Input.TestingDifference;
                oSelectedHypothesisTestingMethods.TestingMean = m_Input.TestingMean;
                oSelectedHypothesisTestingMethods.TestingStd = m_Input.TestingStd;

                if (rdo1SampleT.Checked)
                    m_Input.Testing = TestingType.OneSampleT;
                if (rdo1SampleZ.Checked)
                    m_Input.Testing = TestingType.OneSampleZ;
                if (rdo2SampleT.Checked)
                    m_Input.Testing = TestingType.TwoSampleUnpaired;
                if (rdoPairedT.Checked)
                    m_Input.Testing = TestingType.TwoSamplePaired;

                switch (m_Input.Testing)
                {
                    case TestingType.OneSampleT:
                        DlgHypothesisTesting_.DlgOneSampleT oDlgOneT = new DACrux.BStats.StatDialog.DlgHypothesisTesting_.DlgOneSampleT(oSelectedHypothesisTestingMethods);
                        oDlgOneT.On_SelectedMethods += new DSelectedHypothesisTestingMethods(oDlgOneT_On_SelectedMethods);
                        oDlgOneT.ShowDialog();
                        break;
                    case TestingType.TwoSamplePaired:
                        DlgHypothesisTesting_.DlgTwoSamplePaired oDlgPaired = new DACrux.BStats.StatDialog.DlgHypothesisTesting_.DlgTwoSamplePaired(oSelectedHypothesisTestingMethods);
                        oDlgPaired.On_SelectedMethods += new DSelectedHypothesisTestingMethods(oDlgPaired_On_SelectedMethods);
                        oDlgPaired.ShowDialog();
                        break;
                    case TestingType.TwoSampleUnpaired:
                        DlgHypothesisTesting_.DlgTwoSampleUnpaired oDlgUnpaired = new DACrux.BStats.StatDialog.DlgHypothesisTesting_.DlgTwoSampleUnpaired(oSelectedHypothesisTestingMethods);
                        oDlgUnpaired.On_SelectedMethods += new DSelectedHypothesisTestingMethods(oDlgUnpaired_On_SelectedMethods);
                        oDlgUnpaired.ShowDialog();
                        break;
                    default: //case TestingType.OneSampleZ:
                        DlgHypothesisTesting_.DlgOneSampleZ oDlgOneZ = new DACrux.BStats.StatDialog.DlgHypothesisTesting_.DlgOneSampleZ(oSelectedHypothesisTestingMethods);
                        oDlgOneZ.On_SelectedMethods += new DSelectedHypothesisTestingMethods(oDlgOneZ_On_SelectedMethods);
                        oDlgOneZ.ShowDialog();
                        break;
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void oDlgOneZ_On_SelectedMethods(SelectedHypothesisTestingMethods oSelected)
        {
            m_Input.Alternative = oSelected.Alternative;
            //m_Input.TestingDifference = oSelected.TestingDifference;
            m_Input.TestingMean = oSelected.TestingMean;
            m_Input.TestingStd = oSelected.TestingStd;
            m_Input.ConfidenceLevel = oSelected.ConfidenceLevel;
        }

        void oDlgUnpaired_On_SelectedMethods(SelectedHypothesisTestingMethods oSelected)
        {
            m_Input.Alternative = oSelected.Alternative;
            m_Input.TestingDifference = oSelected.TestingDifference;
            //m_Input.TestingMean = oSelected.TestingMean;
            //m_Input.TestingStd = oSelected.TestingStd;
            m_Input.ConfidenceLevel = oSelected.ConfidenceLevel;
        }

        void oDlgPaired_On_SelectedMethods(SelectedHypothesisTestingMethods oSelected)
        {
            m_Input.Alternative = oSelected.Alternative;
            m_Input.TestingDifference = oSelected.TestingDifference;
            //m_Input.TestingMean = oSelected.TestingMean;
            //m_Input.TestingStd = oSelected.TestingStd;
            m_Input.ConfidenceLevel = oSelected.ConfidenceLevel;
        }

        void oDlgOneT_On_SelectedMethods(SelectedHypothesisTestingMethods oSelected)
        {
            m_Input.Alternative = oSelected.Alternative;
            //m_Input.TestingDifference = oSelected.TestingDifference;
            m_Input.TestingMean = oSelected.TestingMean;
            //m_Input.TestingStd = oSelected.TestingStd;
            m_Input.ConfidenceLevel = oSelected.ConfidenceLevel;
        }

        #endregion

        #region [ Inquery Option ]

        private void SetSetting()
        {
            MenuOption oMenuOption = null;
            inputHypothesisTesting oOption = null;
            try
            {
                oMenuOption = new MenuOption();
                oOption = oMenuOption.GetHypothesisTesting();
                m_Input.Testing = oOption.Testing;
                switch (m_Input.Testing)
                {
                    case TestingType.OneSampleT:
                        rdo1SampleT.Checked = true;
                        break;
                    case TestingType.OneSampleZ:
                        rdo1SampleZ.Checked = true;
                        break;
                    case TestingType.TwoSampleUnpaired:
                        rdo2SampleT.Checked = true;
                        break;
                    case TestingType.TwoSamplePaired:
                        rdoPairedT.Checked = true;
                        break;
                }
                m_Input.Alternative = oOption.Alternative;
                m_Input.ConfidenceLevel = oOption.ConfidenceLevel;
                m_Input.IsConfidenceInterval = oOption.IsConfidenceInterval;
                m_Input.IsDescriptiveStatistics = oOption.IsDescriptiveStatistics;
                m_Input.IsCriticalValue = oOption.IsCriticalValue;
                m_Input.TestingDifference = oOption.TestingDifference;
                m_Input.TestingMean = oOption.TestingMean;
                m_Input.TestingStd = oOption.TestingStd;
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
                oMenuOption.UpdateHypothesisTesting(m_Input);
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

        private bool TrimInput()
        {
            int iColCnt;
            int iVarCnt;
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
            #endregion
            try
            {
                #region Trim Valid Row

                iMaxRow = 0;                
                iVarCnt = lvVariables.Items.Count;
                 iVarCnt = lvVariables.Items.Count;
                 if (iVarCnt >= 1)
                 {
                     if (iVarCnt == 1)
                     {
                         if (rdo2SampleT.Checked || rdoPairedT.Checked)
                         {
                             MessageBox.Show(m_ErrorTwoMoreColumn);
                             return false;
                         }
                     }
                 }
                 else
                 {
                     if (rdo2SampleT.Checked || rdoPairedT.Checked)
                     {
                         MessageBox.Show(m_ErrorTwoMoreColumn);
                         return false;
                     }
                     else
                     {
                         MessageBox.Show(m_ErrorOneMoreColumn);
                         return false;
                     }
                 }
                m_Input.arrValidRowIndices = new int[iVarCnt];
                for (int i = 0; i < iVarCnt; i++)
                {
                    m_Input.arrValidRowIndices[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ValidRowIndex;
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

                #region Testing별 Check

                if (rdo1SampleT.Checked)
                {
                    m_Input.Testing = TestingType.OneSampleT;
                    if (double.IsNaN(m_Input.TestingMean))
                    {
                        if (MessageBox.Show(m_QuestionMean, "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            btnMethods_Click(null, null);
                            return false;
                        }

                    }

                }
                if (rdo1SampleZ.Checked)
                {
                    m_Input.Testing = TestingType.OneSampleZ;
                    if (double.IsNaN(m_Input.TestingMean) || double.IsNaN(m_Input.TestingStd))
                    {
                        if (MessageBox.Show(m_QuestionMeanStd, "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            btnMethods_Click(null, null);
                            return false;
                        }
                    }
                }
                if (rdo2SampleT.Checked)
                {
                    m_Input.Testing = TestingType.TwoSampleUnpaired;
                    if (double.IsNaN(m_Input.TestingDifference))
                    {
                        if (MessageBox.Show(m_QuestionDifference, "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            btnMethods_Click(null, null);
                            return false;
                        }
                    }
                }
                if (rdoPairedT.Checked)
                {
                    m_Input.Testing = TestingType.TwoSamplePaired;
                    //선택한 컬럼의 길이가 모두 같은지 체크
                    int iTemp = m_Input.arrValidRowIndices[0];
                    for (int i = 0; i < m_Input.arrValidRowIndices.Length; i++)
                    {
                        if (iTemp != m_Input.arrValidRowIndices[i])
                        {
                            MessageBox.Show(m_ErrorLengthPaired);
                            return false;
                        }
                    }
                    //선택한 컬럼의 길이가 모두 같은지 체크
                    if (double.IsNaN(m_Input.TestingDifference))
                    {
                        if (MessageBox.Show(m_QuestionDifference, "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            btnMethods_Click(null, null);
                            return false;
                        }
                    }
                }

                #endregion

                iVarCnt = lvVariables.Items.Count;
                if (iVarCnt >= 1)
                {
                    if (iVarCnt == 1)
                    {
                        if (rdo2SampleT.Checked || rdoPairedT.Checked)
                        {
                            MessageBox.Show(m_ErrorTwoMoreColumn);
                            return false;
                        }
                    }
                    arrVar = new int[iVarCnt];
                    for (int i = 0; i < iVarCnt; i++)
                    {
                        arrVar[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvVariables.Items[i]]).ColumnIndex;
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
                    if (rdo2SampleT.Checked || rdoPairedT.Checked)
                    {
                        MessageBox.Show(m_ErrorTwoMoreColumn);
                        return false;
                    }
                    else
                    {
                        MessageBox.Show(m_ErrorOneMoreColumn);
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
                arrVar = null;
            }
        }

        #endregion 

        #endregion

        #region " EVENT HANDLER "
                
        #region [ CLOSING ]

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                #region m_Input 정리 및 체크
                if (!TrimInput())
                    return;
                #endregion
                SaveSetting();
                if (On_Selected != null)
                    On_Selected(m_Input);
                this.DialogResult = DialogResult.OK;
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