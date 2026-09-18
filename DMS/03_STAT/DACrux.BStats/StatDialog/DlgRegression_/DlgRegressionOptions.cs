using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using DACrux.BStats.Core;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.StatDialog.DlgRegression_
{
    
    public partial class DlgRegressionOptions : Form
    {
        #region " MEMBER FIELD "
        string m_Title = "Regression - Options";
        string m_Graph = "Graph";
        string m_Columns = "Columns";
        string m_Histogramofresiduals = "&Histogram of residuals";
        string m_Normalplotofresiduals = "&Normal plot of residuals";
        string m_ResidualsVSfits = "Residuals versus &fits";
        string m_ResidualsVSorder = "Residuals &versus order";
        string m_ResidualsVSthevariables = "Residuals VS the variables";
        string m_Results = "Results";
        string m_Tableofcoefficients = "Table of &coefficients";
        string m_AnalysisofVariance = "&Analysis of variance";
        string m_Rsquared = "&R-squared";
        string m_Tableoffitsandresiduals = "Table of fits &and residuals";

        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";

        #region Message
        #endregion

        private System.Collections.Hashtable htColumns = null;

        #endregion


        #region " CREATOR "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oSelected"></param>
        /// <param name="lstColumnInfo">종속변수와 설명변수로 선택된 모든 변수 목록</param>
        public DlgRegressionOptions(SelectedRegressionOptions oSelected, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstColumnInfo)
        {
            InitializeComponent();
            SetColumnListView(lstColumnInfo);
            InitDialog();
            SetSetting(oSelected);
        }
        #endregion

        #region " EVENT "
        public event DSelectedRegressionOptions On_SelectedOptions;
        #endregion

        #region " METHOD "

        private void InitDialog()
        {
            try
            {
                this.Text = m_Title;
                grpGraph.Text = m_Graph;
                grpResults.Text = m_Results;
                lblColumns.Text = m_Columns;
                lblGraphVariables.Text = m_ResidualsVSthevariables;
                chkAnova.Text = m_AnalysisofVariance;
                chkHistogramofResiduals.Text = m_Histogramofresiduals;
                chkNormalplot.Text = m_Normalplotofresiduals;
                chkResidualsVsFits.Text = m_ResidualsVSfits;
                chkResidualsVsOrder.Text = m_ResidualsVSorder;
                chkRsquared.Text = m_Rsquared;
                chkTableofCoefficients.Text = m_Tableofcoefficients;
                chkTableofResiduals.Text = m_Tableoffitsandresiduals;
                btnCancel.Text = m_Cancel;
                btnOk.Text = m_Ok;


                lvColumns.SmallImageList = imlColumnType;
                lvGraphVariables.SmallImageList = imlColumnType;

                lvColumns.AllowDrop = true;
                lvGraphVariables.AllowDrop = true;

                lvColumns.MultiSelect = true;
                lvGraphVariables.MultiSelect = true;


                btnOk.Click += new EventHandler(btnOk_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
                btnLeft_Graph.Click += new EventHandler(btnLeft_Graph_Click);
                btnRight_Graph.Click += new EventHandler(btnRight_Graph_Click);


                lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);
                lvGraphVariables.MouseDoubleClick += new MouseEventHandler(lvGraphVariables_MouseDoubleClick);

                lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
                lvGraphVariables.ItemDrag += new ItemDragEventHandler(lvGraphVariables_ItemDrag);

                lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
                lvGraphVariables.DragEnter += new DragEventHandler(lvGraphVariables_DragEnter);

                lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
                lvGraphVariables.DragDrop += new DragEventHandler(lvGraphVariables_DragDrop);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetSetting(SelectedRegressionOptions oSelected)
        {
            int iCnt;
            int iColumnCnt;
            try
            {
                chkAnova.Checked = oSelected.IsTableAnova;
                chkHistogramofResiduals.Checked = oSelected.IsGraphHistogramResiduals;
                chkNormalplot.Checked = oSelected.IsGraphProbabilityPlotResiduals;
                chkResidualsVsFits.Checked = oSelected.IsGraphResidualsVsFittedValues;
                chkResidualsVsOrder.Checked = oSelected.IsGraphResidualsVSOrder;
                chkRsquared.Checked = oSelected.IsTableR_Square;
                chkTableofCoefficients.Checked = oSelected.IsTableCoefficients;
                chkTableofResiduals.Checked = oSelected.IsTableResiduals;
                if (oSelected.arrGraphVariablesVsResiduals != null)
                {
                    iCnt = oSelected.arrGraphVariablesVsResiduals.Length;
                    
                    for (int i = 0; i < iCnt; i++)
                    {
                        iColumnCnt = lvColumns.Items.Count;
                        for (int j = 0; j < iColumnCnt; j++)
                        {
                            if (oSelected.arrGraphVariablesVsResiduals[i] == ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.Items[j]]).ColumnIndex)
                            {
                                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvGraphVariables, lvColumns.Items[j]);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SelectedRegressionOptions GetSetting()
        {
            SelectedRegressionOptions oReturn = new SelectedRegressionOptions();
            int iCnt;
            try
            {
                oReturn.IsGraphHistogramResiduals = chkHistogramofResiduals.Checked;
                oReturn.IsGraphProbabilityPlotResiduals = chkNormalplot.Checked;
                oReturn.IsGraphResidualsVsFittedValues = chkResidualsVsFits.Checked;
                oReturn.IsGraphResidualsVSOrder = chkResidualsVsOrder.Checked;
                oReturn.IsTableAnova = chkAnova.Checked;
                oReturn.IsTableCoefficients = chkTableofCoefficients.Checked;
                oReturn.IsTableR_Square = chkRsquared.Checked;
                oReturn.IsTableResiduals = chkTableofResiduals.Checked;

                iCnt = lvGraphVariables.Items.Count;
                if (iCnt > 0)
                    oReturn.arrGraphVariablesVsResiduals = new int[iCnt];
                for (int i = 0; i < iCnt; i++)
                    oReturn.arrGraphVariablesVsResiduals[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvGraphVariables.Items[i]]).ColumnIndex;
                return oReturn;
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

        #region [ Button Click ]

        void btnRight_Graph_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvGraphVariables, (IList)lvColumns.SelectedItems);
        }

        void btnLeft_Graph_Click(object sender, EventArgs e)
        {
            if (lvGraphVariables.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvGraphVariables, lvColumns, (IList)lvGraphVariables.SelectedItems);
        }

        #endregion       

        #region [ ListView MouseDoubleClick ]

        void lvGraphVariables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvGraphVariables.SelectedItems.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvGraphVariables, lvColumns, lvGraphVariables.SelectedItems[0]);
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
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvGraphVariables, lvColumns.SelectedItems[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListView ItemDrag ]

        void lvGraphVariables_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvGraphVariables.DoDragDrop(lvGraphVariables.SelectedItems, DragDropEffects.Move);
        }

        void lvColumns_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvColumns.DoDragDrop(lvColumns.SelectedItems, DragDropEffects.Move);
        }

        #endregion

        #region [ ListView DragEnter ]

        void lvGraphVariables_DragEnter(object sender, DragEventArgs e)
        {
            if (lvGraphVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
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

        #region [ ListView DragDrop ]

        void lvGraphVariables_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvGraphVariables, (IList)lvColumns.SelectedItems);
            }
        }

        void lvColumns_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                if (lvGraphVariables.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvGraphVariables, lvColumns, (IList)lvGraphVariables.SelectedItems);
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
            try
            {                
                if(On_SelectedOptions!=null) 
                    On_SelectedOptions(GetSetting());
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