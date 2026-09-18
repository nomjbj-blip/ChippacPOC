using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DACrux.BStats.Core;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.StatDialog.DlgRegression_
{
    
    public partial class DlgRegressionMethods : Form
    {
        #region " MEMBER FIELD "
        string m_Title = "Regression - Methods";
        string m_Columns = "Columns";
        string m_InEverymodel = "Predictors to include in every model";
        string m_InInitialmodel = "Predictors in initial model";
        string m_All = "A&ll";
        string m_ValueEnter = "Value to &enter";
        string m_ValueRemove = "Value to &remove";
        string m_Criterion = "Criterion";
        string m_UseAlpha = "Use &alpha values";
        string m_UseF = "&Use F values";
        string m_Stepwise = "&Stepwise (forward and backward)";
        string m_Forward = "&Forward selection";
        string m_Backward = "&Backward elimination";
        string m_Ok = "&Ok";
        string m_Cancel = "Cancel";

        #region Message

        string m_ErrorAlpha = "alpha to remove must be >= alpha to enter.";
        string m_ErrorF = "F to remove must be >= F to enter.";

        #endregion

        //Column Double Click시에 삽입할 곳...e
        private bool m_lvEverymodel = true;

        private System.Collections.Hashtable htColumns = null;

        #endregion

        #region " CREATOR "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oSelected"></param>
        /// <param name="lstColumnInfo">설명변수로 선택된 리스트만</param>
        public DlgRegressionMethods(SelectedRegressionMethods oSelected, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstColumnInfo)
        {
            InitializeComponent();
            SetColumnListView(lstColumnInfo);
            InitDialog();            
            SetSetting(oSelected);
        }
        #endregion

        #region " EVENT "
        public event DSelectedRegressionMethods On_SelectedMethod;
        #endregion

        #region " METHOD "

        #region [ Inner Method ]

        private void InitDialog()
        {
            try
            {
                this.Text = m_Title;
                lblColumns.Text = m_Columns;
                lblForwardValue.Text = m_ValueEnter;
                lblBackValue.Text = m_ValueRemove;
                lblStepValueE.Text = m_ValueEnter;
                lblStepValueR.Text = m_ValueRemove;
                lblPredictorsIncludeinEverymodel.Text = m_InEverymodel;
                lblPredictorsinInitialModel.Text = m_InInitialmodel;
                rdoAll.Text = m_All;
                rdoStepwise.Text = m_Stepwise;
                rdoForward.Text = m_Forward;
                rdoBackward.Text = m_Backward;
                rdoAlphavalue.Text = m_UseAlpha;
                rdoFvalue.Text = m_UseF;
                grpCriterion.Text = m_Criterion;
                btnCancel.Text = m_Cancel;
                btnOk.Text = m_Ok;



                lvColumns.SmallImageList = imlColumnType;
                lvPredictorsinEveryModel.SmallImageList = imlColumnType;
                lvPredictorsinInitialModel.SmallImageList = imlColumnType;

                lvColumns.AllowDrop = true;
                lvPredictorsinEveryModel.AllowDrop = true;
                lvPredictorsinInitialModel.AllowDrop = true;

                lvColumns.MultiSelect = true;
                lvPredictorsinEveryModel.MultiSelect = true;
                lvPredictorsinInitialModel.MultiSelect = true;


                rdoAlphavalue.CheckedChanged += new EventHandler(rdoAlphavalue_CheckedChanged);
                rdoAll.CheckedChanged += new EventHandler(rdoAll_CheckedChanged);
                rdoStepwise.CheckedChanged += new EventHandler(rdoStepwise_CheckedChanged);
                rdoForward.CheckedChanged += new EventHandler(rdoForward_CheckedChanged);
                rdoBackward.CheckedChanged += new EventHandler(rdoBackward_CheckedChanged);

                btnOk.Click += new EventHandler(btnOk_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
                btnRight_Everymodel.Click += new EventHandler(btnRight_Everymodel_Click);
                btnRight_Initialmodel.Click += new EventHandler(btnRight_Initialmodel_Click);
                btnLeft_Everymodel.Click += new EventHandler(btnLeft_Everymodel_Click);
                btnLeft_Initialmodel.Click += new EventHandler(btnLeft_Initialmodel_Click);



                lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);
                lvPredictorsinEveryModel.MouseDoubleClick += new MouseEventHandler(lvPredictorsinEveryModel_MouseDoubleClick);
                lvPredictorsinInitialModel.MouseDoubleClick += new MouseEventHandler(lvPredictorsinInitialModel_MouseDoubleClick);

                lvPredictorsinEveryModel.Enter += new EventHandler(lvPredictorsinEveryModel_Enter);
                lvPredictorsinInitialModel.Enter += new EventHandler(lvPredictorsinInitialModel_Enter);

                lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
                lvPredictorsinEveryModel.ItemDrag += new ItemDragEventHandler(lvPredictorsinEveryModel_ItemDrag);
                lvPredictorsinInitialModel.ItemDrag += new ItemDragEventHandler(lvPredictorsinInitialModel_ItemDrag);


                lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
                lvPredictorsinEveryModel.DragEnter += new DragEventHandler(lvPredictorsinEveryModel_DragEnter);
                lvPredictorsinInitialModel.DragEnter += new DragEventHandler(lvPredictorsinInitialModel_DragEnter);

                lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
                lvPredictorsinEveryModel.DragDrop += new DragEventHandler(lvPredictorsinEveryModel_DragDrop);
                lvPredictorsinInitialModel.DragDrop += new DragEventHandler(lvPredictorsinInitialModel_DragDrop);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetSetting(SelectedRegressionMethods oSelected)
        {
            int iCnt;
            int iColumnCnt;
            try
            {
                if (oSelected.arrIncludePredictors != null)
                {
                    iCnt = oSelected.arrIncludePredictors.Length;
                    iColumnCnt = lvColumns.Items.Count;  // 여기는 Move가 아닌 Copy이므로 항상 같음...
                    for (int i = 0; i < iCnt; i++)
                    {
                        for (int j = 0; j < iColumnCnt; j++)
                        {
                            if (oSelected.arrIncludePredictors[i] == ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.Items[j]]).ColumnIndex)
                            {
                                lvPredictorsinEveryModel.Items.Add((ListViewItem)lvColumns.Items[j].Clone());
                                break;
                            }
                        }                        
                    }
                }

                if (oSelected.IsUseAlpha)
                    rdoAlphavalue.Checked = true;
                else
                    rdoFvalue.Checked = true;
                switch (oSelected.ReAnalysisType)
                {
                    case RegressionType.Backward:
                        rdoBackward.Checked = true;
                        nudBackDrop.Value = Convert.ToDecimal(oSelected.backwardDrop);
                        break;
                    case RegressionType.Forward:
                        rdoForward.Checked = true;
                        nudForwardAdd.Value = Convert.ToDecimal(oSelected.forwardAdd);
                        break;
                    case RegressionType.Stepwise:
                        if (oSelected.arrPredictorsInitialModel != null)
                        {
                            iCnt = oSelected.arrPredictorsInitialModel.Length;
                            iColumnCnt = lvColumns.Items.Count;  // 여기는 Move가 아닌 Copy이므로 항상 같음...
                            for (int i = 0; i < iCnt; i++)
                            {
                                for (int j = 0; j < iColumnCnt; j++)
                                {
                                    if (oSelected.arrPredictorsInitialModel[i] == ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[lvColumns.Items[j]]).ColumnIndex)
                                    {
                                        lvPredictorsinInitialModel.Items.Add((ListViewItem)lvColumns.Items[j].Clone());
                                        break;
                                    }
                                }
                            }
                        }

                        rdoStepwise.Checked = true;
                        nudStepAdd.Value = Convert.ToDecimal(oSelected.stepwiseAdd);
                        nudStepDrop.Value = Convert.ToDecimal(oSelected.stepwiseDrop);
                        break;
                    default:
                        rdoAll.Checked = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SelectedRegressionMethods GetSetting()
        {
            SelectedRegressionMethods oReturn = new SelectedRegressionMethods();
            int iCnt;
            try
            {
                oReturn.Reset();                
                if (rdoAlphavalue.Checked)
                    oReturn.IsUseAlpha = true;
                else
                    oReturn.IsUseAlpha = false;

                if (rdoAll.Checked)
                {
                    oReturn.ReAnalysisType = RegressionType.All;
                }
                else if (rdoStepwise.Checked)
                {
                    oReturn.ReAnalysisType = RegressionType.Stepwise;
                    oReturn.stepwiseAdd = Convert.ToDouble(nudStepAdd.Value);
                    oReturn.stepwiseDrop = Convert.ToDouble(nudStepDrop.Value);
                }
                else if (rdoBackward.Checked)
                {
                    oReturn.ReAnalysisType = RegressionType.Backward;
                    oReturn.backwardDrop = Convert.ToDouble(nudBackDrop.Value);
                }
                else// if (rdoForward.Checked)
                {
                    oReturn.ReAnalysisType = RegressionType.Forward;
                    oReturn.forwardAdd = Convert.ToDouble(nudForwardAdd.Value);
                }

                iCnt = lvPredictorsinEveryModel.Items.Count;
                if (iCnt > 0)
                    oReturn.arrIncludePredictors = new int[iCnt];
                for (int i = 0; i < iCnt; i++)
                    oReturn.arrIncludePredictors[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[GetOrigItem(lvPredictorsinEveryModel.Items[i])]).ColumnIndex;

                iCnt = lvPredictorsinInitialModel.Items.Count;
                if (iCnt > 0)
                    oReturn.arrPredictorsInitialModel = new int[iCnt];
                for (int i = 0; i < iCnt; i++)
                    oReturn.arrPredictorsInitialModel[i] = ((DACrux.ProjectManager.UI.DataView.ColumnInfo)htColumns[GetOrigItem(lvPredictorsinInitialModel.Items[i])]).ColumnIndex;

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

        private ListViewItem GetOrigItem(ListViewItem CopiedItem)  //hash table에서 column Index찾기 위해서...사용.
        {
            int iCnt;
            try
            {
                iCnt = lvColumns.Items.Count;
                for (int i = 0; i < iCnt; i++)
                {
                    if (EqualItem(lvColumns.Items[i], CopiedItem))
                        return lvColumns.Items[i];
                    
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool EqualItem(ListViewItem Item1, ListViewItem Item2)
        {
            int iSubItemsCnt;
            try
            {
                iSubItemsCnt = Item1.SubItems.Count;
                if (iSubItemsCnt != Item2.SubItems.Count)
                    return false;
                if (Item1.ImageKey != Item2.ImageKey)
                    return false;
                for (int i = 0; i < iSubItemsCnt; i++)
                {
                    if (Item1.SubItems[i].Text != Item2.SubItems[i].Text)
                    {
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

        private bool IsItemInListView(ListView lv, ListViewItem lvItem)
        {
            try
            {
                for (int i = 0; i < lv.Items.Count; i++)
                {
                    if (EqualItem(lv.Items[i], lvItem))
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckValue(decimal AddValue, decimal DropValue, bool bAlpha)
        {
            try
            {
                if (bAlpha)
                {
                    if (DropValue < AddValue)
                        return false;
                    else
                        return true;
                }
                else
                {
                    if (AddValue < DropValue)
                        return false;
                    else
                        return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ Select Method Changed ]

        void rdoBackward_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoBackward.Checked)
                {
                    pnlStepwise.Enabled = false;
                    pnlBack.Enabled = true;
                    pnlForward.Enabled = false;
                    m_lvEverymodel = true;
                    btnLeft_Initialmodel.Enabled = false;
                    btnRight_Initialmodel.Enabled = false;
                    //btnLeft_Everymodel.Enabled = true;
                    //btnRight_Everymodel.Enabled = true;
                    //lblPredictorsIncludeinEverymodel.Enabled = true;
                    //lvPredictorsinEveryModel.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void rdoForward_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoForward.Checked)
                {
                    pnlStepwise.Enabled = false;
                    pnlBack.Enabled = false;
                    pnlForward.Enabled = true;
                    m_lvEverymodel = true;
                    btnLeft_Initialmodel.Enabled = false;
                    btnRight_Initialmodel.Enabled = false;
                    //btnLeft_Everymodel.Enabled = true;
                    //btnRight_Everymodel.Enabled = true;
                    //lblPredictorsIncludeinEverymodel.Enabled = true;
                    //lvPredictorsinEveryModel.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void rdoStepwise_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoStepwise.Checked)
                {
                    pnlStepwise.Enabled = true;
                    pnlForward.Enabled = false;
                    pnlBack.Enabled = false;
                    m_lvEverymodel = false;
                    btnLeft_Initialmodel.Enabled = true;
                    btnRight_Initialmodel.Enabled = true;
                    //btnLeft_Everymodel.Enabled = true;
                    //btnRight_Everymodel.Enabled = true;
                    //lblPredictorsIncludeinEverymodel.Enabled = true;
                    //lvPredictorsinEveryModel.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoAll.Checked)
                {
                    pnlStepwise.Enabled = false;
                    pnlForward.Enabled = false;
                    pnlBack.Enabled = false;
                    m_lvEverymodel = true;
                    btnLeft_Initialmodel.Enabled = false;
                    btnRight_Initialmodel.Enabled = false;
                    //btnLeft_Everymodel.Enabled = false;
                    //btnRight_Everymodel.Enabled = false;
                    //lblPredictorsIncludeinEverymodel.Enabled = false;
                    //lvPredictorsinEveryModel.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ Criterion Changed ]

        void rdoAlphavalue_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoAlphavalue.Checked)
                {
                    nudStepDrop.Minimum = 0;
                    nudStepAdd.Minimum = 0;
                    nudForwardAdd.Minimum = 0;
                    nudBackDrop.Minimum = 0;
                    nudStepDrop.Maximum = 0.9999m;
                    nudStepAdd.Maximum = 0.9999m;
                    nudForwardAdd.Maximum = 0.9999m;
                    nudBackDrop.Maximum = 0.9999m;
                    nudStepAdd.Value = 0.15m;
                    nudStepDrop.Value = 0.15m;
                    nudForwardAdd.Value = 0.25m;
                    nudBackDrop.Value = 0.1m;
                }
                else
                {
                    nudStepDrop.Minimum = 0;
                    nudStepAdd.Minimum = 0;
                    nudForwardAdd.Minimum = 0;
                    nudBackDrop.Minimum = 0;
                    nudStepDrop.Maximum = decimal.MaxValue;
                    nudStepAdd.Maximum = decimal.MaxValue;
                    nudForwardAdd.Maximum = decimal.MaxValue;
                    nudBackDrop.Maximum = decimal.MaxValue;
                    nudStepAdd.Value = 4;
                    nudStepDrop.Value = 4;
                    nudForwardAdd.Value = 4;
                    nudBackDrop.Value = 4;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ ListView DragDrop ]

        void lvPredictorsinInitialModel_DragDrop(object sender, DragEventArgs e)
        {
            int iCnt;
            try
            {
                if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Copy)
                {
                    if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    {
                        iCnt = lvColumns.SelectedItems.Count;
                        for (int i = 0; i < iCnt; i++)
                        {
                            if (!IsItemInListView(lvPredictorsinInitialModel, lvColumns.SelectedItems[i]))
                                lvPredictorsinInitialModel.Items.Add((ListViewItem)lvColumns.SelectedItems[i].Clone());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvPredictorsinEveryModel_DragDrop(object sender, DragEventArgs e)
        {
            int iCnt;
            try
            {
                if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Copy)
                {
                    if (lvColumns.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    {
                        iCnt = lvColumns.SelectedItems.Count;
                        for (int i = 0; i < iCnt; i++)
                        {
                            if (!IsItemInListView(lvPredictorsinEveryModel, lvColumns.SelectedItems[i]))
                                lvPredictorsinEveryModel.Items.Add((ListViewItem)lvColumns.SelectedItems[i].Clone());
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
            int iCnt;
            try
            {
                if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
                {
                    if (lvPredictorsinEveryModel.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    {
                        iCnt = lvPredictorsinEveryModel.SelectedItems.Count;
                        for (int i = iCnt - 1; i > -1; i--)
                            lvPredictorsinEveryModel.Items.RemoveAt(lvPredictorsinEveryModel.SelectedItems[i].Index);
                    }
                    else if (lvPredictorsinInitialModel.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                    {
                        iCnt = lvPredictorsinInitialModel.SelectedItems.Count;
                        for (int i = iCnt - 1; i > -1; i--)
                            lvPredictorsinInitialModel.Items.RemoveAt(lvPredictorsinInitialModel.SelectedItems[i].Index);
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

        void lvPredictorsinInitialModel_DragEnter(object sender, DragEventArgs e)
        {
            if (rdoStepwise.Checked)
            {
                if (lvPredictorsinInitialModel.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvPredictorsinEveryModel.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
                {
                    e.Effect = DragDropEffects.None;
                }
                else
                    e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        void lvPredictorsinEveryModel_DragEnter(object sender, DragEventArgs e)
        {
            if (lvPredictorsinInitialModel.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))) || lvPredictorsinEveryModel.SelectedItems.Equals(e.Data.GetData(typeof(ListView.SelectedListViewItemCollection))))
            {
                e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.Copy;
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

        void lvPredictorsinInitialModel_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvPredictorsinInitialModel.DoDragDrop(lvPredictorsinInitialModel.SelectedItems, DragDropEffects.Move);
        }

        void lvPredictorsinEveryModel_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvPredictorsinEveryModel.DoDragDrop(lvPredictorsinEveryModel.SelectedItems, DragDropEffects.Move);
        }

        void lvColumns_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvColumns.DoDragDrop(lvColumns.SelectedItems, DragDropEffects.Copy);
        }

        #endregion

        #region [ ListView Enter ]

        void lvPredictorsinInitialModel_Enter(object sender, EventArgs e)
        {
            m_lvEverymodel = false;
        }

        void lvPredictorsinEveryModel_Enter(object sender, EventArgs e)
        {
            m_lvEverymodel = true;
        }

        #endregion

        #region [ ListView DoubleClick ]

        void lvPredictorsinInitialModel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvPredictorsinInitialModel.SelectedItems.Count > 0)
                    lvPredictorsinInitialModel.Items.Remove(lvPredictorsinInitialModel.SelectedItems[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void lvPredictorsinEveryModel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvPredictorsinEveryModel.SelectedItems.Count > 0)
                    lvPredictorsinEveryModel.Items.Remove(lvPredictorsinEveryModel.SelectedItems[0]);
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
                    if (m_lvEverymodel)  // Every Model로...
                    {
                        if (!IsItemInListView(lvPredictorsinEveryModel, lvColumns.SelectedItems[0]))
                            lvPredictorsinEveryModel.Items.Add((ListViewItem)lvColumns.SelectedItems[0].Clone());
                    }
                    else  // Initial Model로...
                    {
                        if (!IsItemInListView(lvPredictorsinInitialModel, lvColumns.SelectedItems[0]))
                            lvPredictorsinInitialModel.Items.Add((ListViewItem)lvColumns.SelectedItems[0].Clone());
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ BUTTON CLICK ]

        #region Left

        void btnLeft_Initialmodel_Click(object sender, EventArgs e)
        {
            int iCnt;
            try
            {                
                iCnt= lvPredictorsinInitialModel.SelectedItems.Count;
                for (int i = iCnt - 1; i > -1; i--)
                    lvPredictorsinInitialModel.Items.RemoveAt(lvPredictorsinInitialModel.SelectedItems[i].Index);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        void btnLeft_Everymodel_Click(object sender, EventArgs e)
        {
            int iCnt;
            try
            {
                iCnt = lvPredictorsinEveryModel.SelectedItems.Count;
                for (int i = iCnt - 1; i > -1; i--)
                    lvPredictorsinEveryModel.Items.RemoveAt(lvPredictorsinEveryModel.SelectedItems[i].Index);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Right

        void btnRight_Initialmodel_Click(object sender, EventArgs e)
        {
            try
            {
                int iCnt = lvColumns.SelectedItems.Count;
                for (int i = 0; i < iCnt; i++)
                {
                    if (!IsItemInListView(lvPredictorsinInitialModel, lvColumns.SelectedItems[i]))
                        lvPredictorsinInitialModel.Items.Add((ListViewItem)lvColumns.SelectedItems[i].Clone());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnRight_Everymodel_Click(object sender, EventArgs e)
        {
            try
            {
                int iCnt = lvColumns.SelectedItems.Count;
                for (int i = 0; i < iCnt; i++)
                {
                    if (!IsItemInListView(lvPredictorsinEveryModel, lvColumns.SelectedItems[i]))
                        lvPredictorsinEveryModel.Items.Add((ListViewItem)lvColumns.SelectedItems[i].Clone());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region [ CLOSING ]

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void btnOk_Click(object sender, EventArgs e)
        {

            if (!CheckValue(nudStepAdd.Value, nudStepDrop.Value, rdoAlphavalue.Checked))
            {
                if (rdoAlphavalue.Checked)
                    throw new Exception(m_ErrorAlpha, new Exception("MIRACOM"));
                else
                    throw new Exception(m_ErrorF, new Exception("MIRACOM"));
            }
            if (On_SelectedMethod != null)
                On_SelectedMethod(GetSetting());
            this.DialogResult = DialogResult.OK;
        }

        #endregion

        #endregion








    }
}