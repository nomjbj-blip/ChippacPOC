using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using DACrux.ProjectManager.UI;

namespace DACrux.BStats.GraphDialog
{
    public partial class DlgHistogramGraph : Form, iGraphAnalysis
    {
        #region " MEMBER FIELD "

        public string FUNC_CODE = "F0407";
        private GraphInformation graphInfo = null;
        private System.Collections.Hashtable htColumns = new System.Collections.Hashtable();

        #endregion

        #region " PROPERTY "

        public GraphInformation GraphInfo
        {
            get
            {
                return graphInfo;
            }
        }

        #endregion

        #region " CREATOR "

        public DlgHistogramGraph(DataTable dataSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo)
            : this(dataSource, lstValidColumnInfo, "Graph Analysis")
        {
        }

        public DlgHistogramGraph(DataTable dataSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo, string createdInformation)
        {
            InitializeComponent();

            graphInfo = new GraphInformation();
            graphInfo.Type = GraphType.Histogram;
            graphInfo.CreatedInformation = createdInformation;
            graphInfo.DataSource = dataSource;

            SetGraphDataInformation(lstValidColumnInfo);

            InitDialog();
        }

        public DlgHistogramGraph(GraphInformation graphInfo)
        {
            InitializeComponent();

            this.graphInfo = graphInfo;

            InitDialog();
        }

        #endregion

        #region " METHOD "

        private void InitDialog()
        {
            this.Text = graphInfo.Type.ToString() + " Graph";
            lblSample.Text = graphInfo.Type.ToString() + " Graph Sample";

            //2014-11-04-정병주 : Default 값을 1로 Setting 한다.(STEMCO 요청 사항)
            graphInfo.DecimalPlaceX = 1;

            lvColumns.SmallImageList = imlColumnType;
            lvSeries.SmallImageList = imlColumnType;

            lvColumns.AllowDrop = true;
            lvSeries.AllowDrop = true;

            lvColumns.MultiSelect = true;
            lvSeries.MultiSelect = true;

            
            nudLSL.Maximum = int.MaxValue;
            nudUSL.Maximum = int.MaxValue;

            SetColumnListView();
            SetSeriesListView();

            LoadCommonSetting(graphInfo);

            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            this.btnRight_Ser.Click += new EventHandler(btnRight_Ser_Click);
            this.btnLeft_Ser.Click += new EventHandler(btnLeft_Ser_Click);

            this.lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);
            this.lvSeries.MouseDoubleClick += new MouseEventHandler(lvSeries_MouseDoubleClick);

            this.lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
            this.lvSeries.ItemDrag += new ItemDragEventHandler(lvSeries_ItemDrag);

            this.lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
            this.lvSeries.DragEnter += new DragEventHandler(lvSeries_DragEnter);

            this.lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
            this.lvSeries.DragDrop += new DragEventHandler(lvSeries_DragDrop);
        }

        private void SetColumnListView()
        {
            ListViewItem lvItem;
            DataColumn column;
            for (int i = 0; i < graphInfo.DataSource.Columns.Count; i++)
            {
                column = graphInfo.DataSource.Columns[i];
                lvItem = new ListViewItem("");

                if (column.DataType == typeof(double))
                    lvItem.ImageKey = "NUMBER";
                else if (column.DataType == typeof(string))
                    lvItem.ImageKey = "TEXT";
                else if (column.DataType == typeof(DateTime))
                    lvItem.ImageKey = "DATETIME";

                lvItem.SubItems.Add(graphInfo.ColumnInfoItems[i].ColumnID);
                lvItem.SubItems.Add(graphInfo.ColumnInfoItems[i].ColumnName);

                lvColumns.Items.Add(lvItem);

                htColumns.Add(lvItem, graphInfo.ColumnInfoItems[i]);
                htColumns.Add(graphInfo.ColumnInfoItems[i], lvItem);
            }
        }

        private void SetSeriesListView()
        {
            GraphInformation.ColumnInfoItem[] arrAxisYItem = graphInfo.AxisY;

            GraphInformation.ColumnInfoItem axisYItem;
            for(int i=0; i<arrAxisYItem.Length; i++)
            {
                axisYItem = arrAxisYItem[i];

                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSeries, (ListViewItem)htColumns[axisYItem]);
            }
        }

        private void SetGraphDataInformation(List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo)
        {
            graphInfo.ColumnInfoItems.Clear();
            DACrux.ProjectManager.UI.DataView.ColumnInfo columnInfo;

            for (int i = 0; i < lstValidColumnInfo.Count; i++)
            {
                columnInfo = lstValidColumnInfo[i];
                graphInfo.ColumnInfoItems.Add(new GraphInformation.ColumnInfoItem(i, columnInfo.ColumnID, columnInfo.ColumnName, graphInfo.DataSource.Columns[i].DataType));
            }
        }

        private void LoadCommonSetting(GraphInformation graphInfo)
        {
            txtAxisXTitle.Text = graphInfo.AxisXTitle;
            txtAxisYTitle.Text = graphInfo.AxisYTitle;

            nudDecimalPlace.Value = Convert.ToDecimal(graphInfo.DecimalPlaceX);

            if (double.IsNaN(graphInfo.USL))
                nudUSL.Value = 0;
            else
                nudUSL.Value = Convert.ToDecimal(graphInfo.USL);
            if (double.IsNaN(graphInfo.LSL))
                nudLSL.Value = 0;
            else
                nudLSL.Value = Convert.ToDecimal(graphInfo.LSL);

            chkNormalLine.Checked = graphInfo.NormalLine;
            //chkSpecLimit.Checked = graphInfo.SpecLimit;
            chkSpecLimit.Checked = false;
            chk3SigmaLine.Checked = graphInfo.View3SigmaLine;
            chkFrequence.Checked = graphInfo.Frequence;
            chkGridLine.Checked = graphInfo.GridLine;
        }

        private void SaveCommonSetting(GraphInformation graphInfo)
        {
            graphInfo.AxisXTitle = txtAxisXTitle.Text;
            graphInfo.AxisYTitle = txtAxisYTitle.Text;

            graphInfo.DecimalPlaceX = Convert.ToInt32(nudDecimalPlace.Value);
            graphInfo.USL = Convert.ToDouble(nudUSL.Value);  
            //graphInfo.Target = //추가해줘야함. 김현태
            graphInfo.LSL = Convert.ToDouble(nudLSL.Value);

            graphInfo.NormalLine = chkNormalLine.Checked;
            graphInfo.SpecLimit = chkSpecLimit.Checked;
            graphInfo.View3SigmaLine = chk3SigmaLine.Checked;
            graphInfo.Frequence = chkFrequence.Checked;
            graphInfo.GridLine = chkGridLine.Checked;
        }

        #region " DEPLICATED : Auto Spec Calculation "
        //private void lvSeries_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (lvSeries.SelectedItems.Count > 0)
        //        {
        //            GraphInformation.ColumnInfoItem colInfo = (GraphInformation.ColumnInfoItem)htColumns[lvSeries.SelectedItems[0]];

        //            if (colInfo.ColumnType != typeof(double))
        //                return;

        //            int iColIndex = colInfo.ColumnIndex;
        //            int iRowCount = graphInfo.DataSource.Rows.Count;
        //            double nTemp = 0;
        //            double nMax = double.MinValue;
        //            double nMin = double.MaxValue;
        //            for (int i = 0; i < iRowCount; i++)
        //            {
        //                if (double.TryParse(graphInfo.DataSource.Rows[i][iColIndex].ToString(), out nTemp))
        //                {
        //                    if (nTemp > nMax)
        //                        nMax = nTemp;

        //                    if (nTemp < nMin)
        //                        nMin = nTemp;
        //                }
        //            }

        //            nudUSL.Value = Convert.ToDecimal(Math.Round(nMax - ((nMax - nMin) / 4), Convert.ToInt32(nudDecimalPlace.Value)));
        //            nudLSL.Value = Convert.ToDecimal(Math.Round(nMin + ((nMax - nMin) / 4), Convert.ToInt32(nudDecimalPlace.Value)));
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Invalid Data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
        #endregion

        #endregion

        #region " EVENT HANDLER "

        #region [ ListView Mouse DoubleClick ]

        void lvSeries_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvSeries.SelectedItems.Count > 0)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvSeries, lvColumns, lvSeries.SelectedItems[0]);
            }
        }

        void lvColumns_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                GraphInformation.ColumnInfoItem columnInfo = (GraphInformation.ColumnInfoItem)htColumns[lvColumns.SelectedItems[0]];

                if (columnInfo.ColumnType == typeof(double))
                {
                    if (lvSeries.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSeries, lvColumns, (IList)lvSeries.Items);

                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSeries, lvColumns.SelectedItems[0]);
                }
            }
        }

        #endregion

        #region [ ListViewItem Move Button ]

        private void btnRight_Ser_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                List<ListViewItem> filteredItems = new List<ListViewItem>();

                for (int i = 0; i < lvColumns.SelectedItems.Count; i++)
                {
                    if (((GraphInformation.ColumnInfoItem)htColumns[lvColumns.SelectedItems[i]]).ColumnType == typeof(double))
                        filteredItems.Add(lvColumns.SelectedItems[i]);
                }

                if (lvSeries.Items.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSeries, lvColumns, (IList)lvSeries.Items);

                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvSeries, (IList)filteredItems);
            }
        }

        private void btnLeft_Ser_Click(object sender, EventArgs e)
        {
            if (lvSeries.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSeries, lvColumns, (IList)lvSeries.SelectedItems);
        }

        #endregion

        #region [ Drag & Drop ]

        private ListView lvDragSource;

        void lvColumns_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvDragSource = (ListView)sender;
            lvDragSource.DoDragDrop(lvDragSource.SelectedItems, DragDropEffects.Move);
        }

        void lvSeries_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvDragSource = (ListView)sender;
            lvDragSource.DoDragDrop(lvDragSource.SelectedItems, DragDropEffects.Move);
        }

        void lvColumns_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvColumns)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void lvSeries_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvSeries)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void lvColumns_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvDragSource, (ListView)sender, (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
            }

            lvDragSource = null;
        }

        void lvSeries_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                IList items = (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));
                List<ListViewItem> filteredItems = new List<ListViewItem>();

                for (int i = 0; i < items.Count; i++)
                {
                    if (((GraphInformation.ColumnInfoItem)htColumns[(ListViewItem)items[i]]).ColumnType == typeof(double))
                        filteredItems.Add((ListViewItem)items[i]);
                }

                if(filteredItems.Count > 0)
                {
                    if (lvSeries.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSeries, lvColumns, (IList)lvSeries.Items);

                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvDragSource, (ListView)sender, filteredItems[0]);
                }
            }

            lvDragSource = null;
        }

        #endregion

        #region [ Closing ]

        private DateTimeFormat GetDateTimeFormat(string dateTimeFormatString)
        {
            DateTimeFormat format = DateTimeFormat.Full;

            if(dateTimeFormatString == DateTimeFormat.Full.ToString())
                return DateTimeFormat.Full;
            else if(dateTimeFormatString == DateTimeFormat.LongDate.ToString())
                return DateTimeFormat.LongDate;
            else if(dateTimeFormatString == DateTimeFormat.LongTime.ToString())
                return DateTimeFormat.LongTime;
            else if(dateTimeFormatString == DateTimeFormat.ShortDate.ToString())
                return DateTimeFormat.ShortDate;
            else if(dateTimeFormatString == DateTimeFormat.ShortTime.ToString())
                return DateTimeFormat.ShortTime;

            return format;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            #region [ Assertion ]

            string strMessage = string.Empty;

            if (lvSeries.Items.Count != 1)
                strMessage = "Only one column should be included.";

            for (int i = 0; i < lvSeries.Items.Count; i++)
            {
                if (((GraphInformation.ColumnInfoItem)htColumns[lvSeries.Items[i]]).ColumnType != typeof(double))
                {
                    strMessage = "Only number type column can be a series member.";
                    break;
                }
            }

            if (strMessage != string.Empty)
            {
                MessageBox.Show(strMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            #region [ Common Setting ]

            SaveCommonSetting(graphInfo);

            #endregion

            #region [ Data Setting ]

            graphInfo.ClearAxisY();
            foreach(ListViewItem lvItem in lvSeries.Items)
                graphInfo.AddAxisY((GraphInformation.ColumnInfoItem)htColumns[lvItem]);

            #endregion

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        private void pictureBoxSample_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void uclComboBox1_Load(object sender, EventArgs e)
        {

        }
    }
}