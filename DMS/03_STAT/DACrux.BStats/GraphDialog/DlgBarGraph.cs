using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using DACrux.ProjectManager.UI;

namespace DACrux.BStats.GraphDialog
{
    public partial class DlgBarGraph : Form, iGraphAnalysis
    {
        #region " MEMBER FIELD "

        public string FUNC_CODE = "F0402";

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

        public DlgBarGraph(DataTable dataSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo)
            : this(dataSource, lstValidColumnInfo, "Graph Analysis")
        {
        }

        public DlgBarGraph(DataTable dataSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo, string createdInformation)
        {
            InitializeComponent();

            graphInfo = new GraphInformation();
            graphInfo.Type = GraphType.Bar;
            graphInfo.CreatedInformation = createdInformation;
            graphInfo.DataSource = dataSource;

            graphInfo.View3D = false;
            // Changed By James Kwon 2014/10/16
            //graphInfo.PointLabel = true;
            graphInfo.PointLabel = false;
            graphInfo.LabelAngle = 0;

            SetGraphDataInformation(lstValidColumnInfo);

            InitDialog();
        }

        public DlgBarGraph(GraphInformation graphInfo)
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

            cboDateTimeFormat.SelectedIndex = 0;
            cboLabelAngle.SelectedIndex = 0;

            lvColumns.SmallImageList = imlColumnType;
            lvSeries.SmallImageList = imlColumnType;
            lvLabel.SmallImageList = imlColumnType;

            lvColumns.AllowDrop = true;
            lvSeries.AllowDrop = true;
            lvLabel.AllowDrop = true;

            lvColumns.MultiSelect = true;
            lvSeries.MultiSelect = true;
            lvLabel.MultiSelect = true;

            SetColumnListView();
            SetSeriesListView();
            SetLabelListView();

            LoadCommonSetting(graphInfo);

            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            this.btnRight_Ser.Click += new EventHandler(btnRight_Ser_Click);
            this.btnLeft_Ser.Click += new EventHandler(btnLeft_Ser_Click);
            this.btnRight_Leg.Click += new EventHandler(btnRight_Leg_Click);
            this.btnLeft_Leg.Click += new EventHandler(btnLeft_Leg_Click);

            this.lvColumns.MouseDoubleClick += new MouseEventHandler(lvColumns_MouseDoubleClick);
            this.lvSeries.MouseDoubleClick += new MouseEventHandler(lvSeries_MouseDoubleClick);
            this.lvLabel.MouseDoubleClick += new MouseEventHandler(lvLabel_MouseDoubleClick);

            this.lvColumns.ItemDrag += new ItemDragEventHandler(lvColumns_ItemDrag);
            this.lvSeries.ItemDrag += new ItemDragEventHandler(lvSeries_ItemDrag);
            this.lvLabel.ItemDrag += new ItemDragEventHandler(lvLabel_ItemDrag);

            this.lvColumns.DragEnter += new DragEventHandler(lvColumns_DragEnter);
            this.lvSeries.DragEnter += new DragEventHandler(lvSeries_DragEnter);
            this.lvLabel.DragEnter += new DragEventHandler(lvLabel_DragEnter);

            this.lvColumns.DragDrop += new DragEventHandler(lvColumns_DragDrop);
            this.lvSeries.DragDrop += new DragEventHandler(lvSeries_DragDrop);
            this.lvLabel.DragDrop += new DragEventHandler(lvLabel_DragDrop);
        }

        private void LoadCommonSetting(GraphInformation graphInfo)
        {
            txtAxisXTitle.Text = graphInfo.AxisXTitle;
            txtAxisYTitle.Text = graphInfo.AxisYTitle;
            cboDateTimeFormat.Text = graphInfo.DateTimeFormat.ToString();
            nudBarSize.Value = Convert.ToDecimal(graphInfo.BarSize);
            nudDecimalPlace.Value = Convert.ToDecimal(graphInfo.DecimalPlace);
            cboLabelAngle.Text = graphInfo.LabelAngleString;
            chkView3D.Checked = graphInfo.View3D;
            //chkSerLegBox.Checked = graphInfo.LegendBox;
            chkSerLegBox.Checked = true;
            chkPointLabel.Checked = graphInfo.PointLabel;
        }

        private void SaveCommonSetting(GraphInformation graphInfo)
        {
            graphInfo.AxisXTitle = txtAxisXTitle.Text;
            graphInfo.AxisYTitle = txtAxisYTitle.Text;
            graphInfo.DateTimeFormat = GetDateTimeFormat(cboDateTimeFormat.Text);
            graphInfo.BarSize = Convert.ToInt16(nudBarSize.Value);
            graphInfo.DecimalPlace = Convert.ToInt32(nudDecimalPlace.Value);
            graphInfo.LabelAngleString = cboLabelAngle.Text;
            graphInfo.View3D = chkView3D.Checked;
            graphInfo.LegendBox = chkSerLegBox.Checked;
            graphInfo.PointLabel = chkPointLabel.Checked;
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
            for (int i = 0; i < arrAxisYItem.Length; i++)
            {
                axisYItem = arrAxisYItem[i];

                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSeries, (ListViewItem)htColumns[axisYItem]);
            }
        }

        private void SetLabelListView()
        {
            foreach (ListViewItem lvColumnItem in lvColumns.Items)
            {
                if (((GraphInformation.ColumnInfoItem)htColumns[lvColumnItem]).Equals(graphInfo.AxisX))
                {
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvLabel, lvColumnItem);
                    break;
                }
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

        private DateTimeFormat GetDateTimeFormat(string dateTimeFormatString)
        {
            DateTimeFormat format = DateTimeFormat.Full;

            if (dateTimeFormatString == DateTimeFormat.Full.ToString())
                return DateTimeFormat.Full;
            else if (dateTimeFormatString == DateTimeFormat.LongDate.ToString())
                return DateTimeFormat.LongDate;
            else if (dateTimeFormatString == DateTimeFormat.LongTime.ToString())
                return DateTimeFormat.LongTime;
            else if (dateTimeFormatString == DateTimeFormat.ShortDate.ToString())
                return DateTimeFormat.ShortDate;
            else if (dateTimeFormatString == DateTimeFormat.ShortTime.ToString())
                return DateTimeFormat.ShortTime;

            return format;
        }

        #endregion

        #region " EVENT HANDLER "

        #region [ ListView Mouse DoubleClick ]

        void lvLabel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvLabel.SelectedItems.Count > 0)
            {
                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvLabel, lvColumns, lvLabel.SelectedItems[0]);
            }
        }

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
                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvSeries, lvColumns.SelectedItems[0]);
                else
                {
                    if (lvLabel.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvLabel, lvColumns, (IList)lvLabel.Items);

                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvLabel, lvColumns.SelectedItems[0]);
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

                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvColumns, lvSeries, (IList)filteredItems);
            }
        }

        private void btnLeft_Ser_Click(object sender, EventArgs e)
        {
            if (lvSeries.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSeries, lvColumns, (IList)lvSeries.SelectedItems);
        }

        private void btnRight_Leg_Click(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                if (lvLabel.Items.Count > 0)
                    DACrux.ProjectManager.UI.Common.MoveListViewItems(lvLabel, lvColumns, (IList)lvLabel.Items);

                DACrux.ProjectManager.UI.Common.MoveListViewItem(lvColumns, lvLabel, lvColumns.SelectedItems[0]);
            }
        }

        private void btnLeft_Leg_Click(object sender, EventArgs e)
        {
            if (lvLabel.SelectedItems.Count > 0)
                DACrux.ProjectManager.UI.Common.MoveListViewItems(lvLabel, lvColumns, (IList)lvLabel.SelectedItems);
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

        void lvLabel_ItemDrag(object sender, ItemDragEventArgs e)
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

        void lvLabel_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvLabel)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void lvColumns_DragDrop(object sender, DragEventArgs e)
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

                if (items.Count > 0)
                {
                    if (lvSeries.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvSeries, lvColumns, (IList)lvSeries.Items);

                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvDragSource, lvSeries, (ListViewItem)items[0]);
                }
            }

            lvDragSource = null;
        }

        void lvLabel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                IList items = (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));

                if (items.Count > 0)
                {
                    if (lvLabel.Items.Count > 0)
                        DACrux.ProjectManager.UI.Common.MoveListViewItems(lvLabel, lvColumns, (IList)lvLabel.Items);

                    DACrux.ProjectManager.UI.Common.MoveListViewItem(lvDragSource, lvLabel, (ListViewItem)items[0]);
                }
            }

            lvDragSource = null;
        }

        #endregion

        #region [ Closing ]

        private void btnOk_Click(object sender, EventArgs e)
        {
            #region [ Assertion ]

            string strMessage = string.Empty;

            if (lvSeries.Items.Count < 1)
                strMessage = "At least one column should be selected.";

            if (lvLabel.Items.Count != 1)
                strMessage = "Specify a column for Axis X Label";

            for (int i = 0; i < lvSeries.Items.Count; i++)
            {
                if (((GraphInformation.ColumnInfoItem)htColumns[lvSeries.Items[i]]).ColumnType != typeof(double))
                {
                    strMessage = "Only number type columns can be value.";
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
            graphInfo.AxisX = (GraphInformation.ColumnInfoItem)htColumns[lvLabel.Items[0]];
            foreach (ListViewItem lvItem in lvSeries.Items)
                graphInfo.AddAxisY((GraphInformation.ColumnInfoItem)htColumns[lvItem]);

            #endregion

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion
        private void btnLeft_Ser_Click_1(object sender, EventArgs e)
        {

        }

        #endregion
    }
}