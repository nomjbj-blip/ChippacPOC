using System;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using DACrux.SP.Controls.Configuration;

namespace DACrux.SP.Controls
{
    /// <summary>
    /// Class Name : SmartListView<br/>
    /// Summary    : Dynamic Condition Selection ListView Control Class<br/>
    /// Author     : 미라콤 양형석<br/>
    /// First Date : 2008-11-12<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class SmartListView : System.Windows.Forms.ListView, ISupportInitialize, IConditionOwner
    {
        #region " INNER CLASS "

        public class SmartListViewItemSeletedEventArgs : EventArgs
        {
            public ListViewItem Item;
        };

        #endregion

        #region " MEMBER FIELD "

        private System.ComponentModel.IContainer components = null;
        private List<ListViewItem> checkedItem = new List<ListViewItem>();
        private System.Windows.Forms.ImageList imlCheckbox;

        ColumnHeader colCheck;
        ColumnHeader colCode = CreateColumnHeader("Code", 50, HorizontalAlignment.Left);
        ColumnHeader colDesc = CreateColumnHeader("Desc", 50, HorizontalAlignment.Left);
        DataTable dtDataSource = null;

        private static readonly string FACILITY = "FACILITY";
        private static readonly string FACILITY_DESC = "FACILITY_DESC";
        private static readonly string CORP_CODE = "CORP_CODE";
        private static readonly string CORP_DESC = "CORP_DESC";
        private static readonly string DEVICE = "DEVICE";
        private static readonly string DEVICE_DESC = "DEVICE_DESC";
        private static readonly string PRODUCT = "PRODUCT";
        private static readonly string VERSION = "VERSION";
        private static readonly string PRODUCT_DESC = "PRODUCT_DESC";
        private static readonly string C_PRODUCT = "C_PRODUCT";
        private static readonly string C_PRODUCT_DESC = "C_PRODUCT_DESC";
        private static readonly string FLOW = "FLOW";
        private static readonly string FLOW_DESC = "FLOW_DESC";
        private static readonly string OPER = "OPER";
        private static readonly string OPER_DESC = "OPER_DESC";
        private static readonly string RES_ID = "RES_ID";
        private static readonly string RES_DESC = "RES_DESC";
        private static readonly string PARA_ID = "PARA_ID";
        private static readonly string PARA_DESC = "PARA_DESC";
        private static readonly string DATA_TYPE = "DATA_TYPE";
        private static readonly string COLL_TYPE = "COLL_TYPE";
        private static readonly string SAMPLE_SIZE = "SAMPLE_SIZE";
        private static readonly string UNIT = "UNIT";
        private static readonly string PARA_GROUP_ID = "PARA_GROUP_ID";
        private static readonly string PARA_GROUP_DESC = "PARA_GROUP_DESC";
        private static readonly string DEFECT_FLAG = "DEFECT_FLAG";
        //private static readonly string USR_FLAG_1 = "USR_FLAG_1";
        private static readonly string CREATE_USER_ID = "CREATE_USER_ID";
        private static readonly string CREATE_TIME = "CREATE_TIME";
        private static readonly string UPDATE_USER_ID = "UPDATE_USER_ID";
        private static readonly string UPDATE_TIME = "UPDATE_TIME";
        private static readonly string USER_ID = "USER_ID";
        private static readonly string USER_NAME = "USER_NAME";
        private static readonly string USER_GROUP = "USER_GROUP";
        private static readonly string PHONE_OFFICE = "PHONE_OFFICE";
        private static readonly string PHONE_MOBILE = "PHONE_MOBILE";
        private static readonly string PHONE_HOME = "PHONE_HOME";
        private static readonly string PHONE_OTHER = "PHONE_OTHER";
        private static readonly string EMAIL_ID = "EMAIL_ID";

        private List<DynamicColumnItem> lstColumnItems = new List<DynamicColumnItem>();

        #endregion

        #region " PROPERTY "

        [Category("Setup")]
        public string DynamicColumnList
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (DynamicColumnItem item in lstColumnItems)
                {
                    if (item.DataTableHeader.Equals(item.ListViewHeader))
                        sb.AppendFormat(",{0}", item.DataTableHeader);
                    else
                        sb.AppendFormat(",{0}:{1}", item.DataTableHeader, item.ListViewHeader);
                }

                if (sb.Length > 0)
                    return sb.ToString(1, sb.Length - 1);
                else
                    return string.Empty;
            }
            set
            {
                lstColumnItems.Clear();

                try
                {
                    if (string.IsNullOrEmpty(value))
                        return;

                    string[] arrColumn = value.Split(',');
                    string[] arrColumnItem = null;

                    foreach (string column in arrColumn)
                    {
                        if (string.IsNullOrEmpty(column))
                            continue;

                        if (column.IndexOf(':') > 0)
                        {
                            arrColumnItem = column.Split(':');

                            if (arrColumnItem.Length < 2)
                                lstColumnItems.Add(new DynamicColumnItem(arrColumnItem[0], arrColumnItem[0]));
                            else
                                lstColumnItems.Add(new DynamicColumnItem(arrColumnItem[0], arrColumnItem[1]));
                        }
                    }

                    SetColumnHeader(helper.A_ConditionType);
                }
                catch
                {
                    throw new ArgumentException("Invalid Argument format : DynamicColumnList");
                }
            }
        }

        [Category("Setup"), Browsable(false)]
        public List<ListViewItem> CheckedItem
        {
            get { return checkedItem; }
        }

        [Category("Setup")]
        public new bool MultiSelect
        {
            get { return base.MultiSelect; }
            set
            {
                base.MultiSelect = value;

                if (!base.MultiSelect)
                    this.Columns[0].Width = 0;
                else
                    this.Columns[0].Width = this.SmallImageList.ImageSize.Width + 4;
            }
        }

        [Browsable(false)]
        public new IEnumerable<int> CheckedIndices
        {
            get
            {
                List<int> lstCheckedIndices = new List<int>();

                foreach (ListViewItem lvItem in this.checkedItem)
                {
                    lstCheckedIndices.Add(this.Items.IndexOf(lvItem));
                }
                lstCheckedIndices.Sort();

                return (IEnumerable<int>)lstCheckedIndices;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTable DataSource
        {
            get { return dtDataSource; }
            set
            {
                ClearItems();
                dtDataSource = value;

                if (dtDataSource != null && dtDataSource.Rows.Count > 0)
                {
                    SetColumnHeader(helper.A_ConditionType);
                    SetListViewItems();
                    SetColumnWidth();
                }
            }
        }

        #endregion

        #region " EVENT "

        public event EventHandler ItemSelected = null;

        #endregion

        #region " CREATOR "

        public SmartListView()
        {
           
            InitializeComponent();

            helper.ConditionTypeChanged += new ConditionTypeChangedHandler(SetColumnHeader);
        }

        public SmartListView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            helper.ConditionTypeChanged += new ConditionTypeChangedHandler(SetColumnHeader);
        }

        #endregion

        #region " EVENT HANDLER "

        void SmartListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (FocusedItem == null)
                return;

            if (MultiSelect)
            {
                if (FocusedItem.ImageIndex == 0)
                {
                    FocusedItem.ImageIndex = 1;

                    if (!checkedItem.Contains(FocusedItem))
                        checkedItem.Add(FocusedItem);
                }
                else
                {
                    FocusedItem.ImageIndex = 0;
                    checkedItem.Remove(FocusedItem);
                }
            }
            else
            {
                foreach (ListViewItem item in Items)
                    item.ImageIndex = 0;

                checkedItem.Clear();

                FocusedItem.ImageIndex = 1;
                checkedItem.Add(FocusedItem);
            }

            //if (FocusedItem.ImageIndex == 1)
            //{
            //    FocusedItem.ImageIndex = 0;
            //    checkedItem.Remove(FocusedItem);
            //}
            //else
            //{
            //    if (!MultiSelect)
            //    {
            //        CheckedItem.Clear();
            //        foreach (ListViewItem item in Items)
            //            item.ImageIndex = 0;
            //    }

            //    FocusedItem.ImageIndex = 1;

            //    if (!checkedItem.Contains(FocusedItem))
            //        checkedItem.Add(FocusedItem);
            //}

            if (ItemSelected != null)
            {
                SmartListViewItemSeletedEventArgs args = new SmartListViewItemSeletedEventArgs();
                args.Item = FocusedItem;

                ItemSelected(this, args);
            }

            helper.ConditionValueString = GetSelectedValueString();
            helper.ConditionValueObject = GetSelectedValueObject();
            helper.ConditionValueIndices = ((List<int>)CheckedIndices).ToArray();

            helper.UpdateRefToConditions();
        }

        #endregion

        #region " METHOD "

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmartListView));
            this.imlCheckbox = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // colCheck
            // 
            this.colCheck = new ColumnHeader();
            this.colCheck.Text = " ";
            this.colCheck.Width = 20;
            this.colCheck.TextAlign = HorizontalAlignment.Center;
            this.ResumeLayout(false);
            // 
            // imlCheckbox
            // 
            this.imlCheckbox.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlCheckbox.ImageStream")));
            this.imlCheckbox.TransparentColor = System.Drawing.Color.Transparent;
            this.imlCheckbox.Images.SetKeyName(0, "Unchecked");
            this.imlCheckbox.Images.SetKeyName(1, "Checked");
            this.ResumeLayout(false);
            // 
            // SmartListView
            // 
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideSelection = false;
            this.Size = new System.Drawing.Size(100, 200);
            this.Columns.Add(" ", 20, HorizontalAlignment.Center);
            this.FullRowSelect = true;
            this.View = System.Windows.Forms.View.Details;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MultiSelect = false;
            this.GridLines = false;
            this.HeaderStyle = ColumnHeaderStyle.None;
            this.SmallImageList = imlCheckbox;
            this.ResumeLayout(false);

            this.MouseUp += new MouseEventHandler(SmartListView_MouseUp);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Clear();

                if (this.SmallImageList != null)
                {
                    this.SmallImageList.Images.Clear();
                    this.SmallImageList.Dispose();
                    this.SmallImageList = null;
                }

                if (this.imlCheckbox != null)
                {
                    this.imlCheckbox.Images.Clear();
                    this.imlCheckbox.Dispose();
                    this.imlCheckbox = null;
                }

                if (!(components == null))
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        public void ClearItems()
        {
            Items.Clear();
            CheckedItem.Clear();
        }

        internal void ClearHeader()
        {
            this.Columns.Clear();
            this.Columns.Add(colCheck);
        }

        public void ClearSelectedItems()
        {
            foreach (ListViewItem item in Items)
                item.ImageIndex = 0;

            SelectedItems.Clear();
            CheckedItem.Clear();
        }

        public bool GetItemChecked(int i)
        {
            return (Items[i].ImageIndex == 0) ? false : true;
        }

        public void SetItemChecked(ListViewItem item, bool isCheck)
        {
            SetItemChecked(Items.IndexOf(item), isCheck);
        }

        public void SetItemChecked(int i, bool isCheck)
        {
            if (i < 0 || i >= Items.Count)
                return;

            if (isCheck == true)
            {
                Items[i].ImageIndex = 1;

                if (!CheckedItem.Contains(Items[i]))
                    checkedItem.Add(Items[i]);
            }
            else
            {
                Items[i].ImageIndex = 0;
                checkedItem.Remove(Items[i]);
            }
        }

        private static ColumnHeader CreateColumnHeader(string title, int width, HorizontalAlignment align)
        {
            ColumnHeader col = new ColumnHeader();
            col = new ColumnHeader();
            col.Text = title;
            col.Width = width;
            col.TextAlign = align;
            return col;
        }

        public DataTable GetSelectedValueObject()
        {
            if (dtDataSource == null || dtDataSource.Rows.Count < 1)
                return null;

            DataTable dt = null;

            switch (helper.A_ConditionType)
            {
                default:
                    dt = dtDataSource.Clone();
                    foreach (ListViewItem lvItem in CheckedItem)
                    {
                        if (lvItem.Index < 0)
                            checkedItem.Remove(lvItem);
                        else
                            dt.Rows.Add(dtDataSource.Rows[lvItem.Index].ItemArray);
                    }
                    break;
            }

            return dt;
        }

        public string GetSelectedValueString()
        {
            return GetSelectedValueString(1);
        }

        public string GetSelectedValueString(int columnIndex)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                ListViewItem lvItem = null;

                if (this.checkedItem.Count < 1 || columnIndex >= checkedItem[0].SubItems.Count)
                    return string.Empty;

                if (MultiSelect)
                {
                    foreach (object value in this.checkedItem)
                    {
                        lvItem = (ListViewItem)value;
                        sb.AppendFormat(",{0}", lvItem.SubItems[columnIndex].Text);
                    }

                    if (sb.Length > 1)
                        return sb.ToString(1, sb.Length - 1);
                    else
                        return string.Empty;
                }
                else
                {
                    if (FocusedItem == null)
                        return string.Empty;
                    else
                        return FocusedItem.SubItems[columnIndex].Text;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string GetSelectedValueString(string columnName)
        {
            int iColumnIndex = dtDataSource.Columns.IndexOf(columnName);

            if (iColumnIndex < 0)
                return string.Empty;
            else
                return GetSelectedValueString(iColumnIndex);
        }

        public virtual void BeginInit()
        {
        }

        public virtual void EndInit()
        {
        }

        private void SetColumnHeader(ConditionTypeItems oConditionType)
        {
            ClearHeader();

            switch (oConditionType)
            {
                case ConditionTypeItems.None:

                    if (lstColumnItems.Count > 0)
                    {
                        foreach (DynamicColumnItem column in lstColumnItems)
                            Columns.Add(CreateColumnHeader(column.ListViewHeader, 50, HorizontalAlignment.Left));
                    }
                    else
                        Columns.AddRange(new ColumnHeader[] { colCode, colDesc });

                    break;
                case ConditionTypeItems.Facility:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc });
                    break;
                case ConditionTypeItems.Customer:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc });
                    break;
                case ConditionTypeItems.Device:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc });
                    break;
                case ConditionTypeItems.Product:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc, CreateColumnHeader("Ver", 30, HorizontalAlignment.Left) });
                    break;
                case ConditionTypeItems.Flow:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc });
                    break;
                case ConditionTypeItems.Oper:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc });
                    break;
                case ConditionTypeItems.Resource:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc });
                    break;
                case ConditionTypeItems.Para:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc });
                    break;
                case ConditionTypeItems.ParaGroup:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc });
                    break;
                case ConditionTypeItems.User:
                    Columns.AddRange(new ColumnHeader[] { CreateColumnHeader(oConditionType.ToString(), 50, HorizontalAlignment.Left), colDesc });
                    break;
                default:
                    break;
            }
        }

        private void SetListViewItems()
        {
            ListViewItem lvItem;

            try
            {
                switch (helper.A_ConditionType)
                {
                    case ConditionTypeItems.None:

                        if (lstColumnItems.Count > 0)
                        {
                            for (int i = 0; i < dtDataSource.Rows.Count; i++)
                            {
                                lvItem = new ListViewItem(" ");

                                foreach (DynamicColumnItem column in lstColumnItems)
                                    lvItem.SubItems.Add(dtDataSource.Rows[i][column.DataTableHeader].ToString());

                                lvItem.ImageIndex = 0;

                                Items.Add(lvItem);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtDataSource.Rows.Count; i++)
                            {
                                lvItem = new ListViewItem(" ");

                                for (int j = 0; j < dtDataSource.Columns.Count; j++)
                                    lvItem.SubItems.Add(dtDataSource.Rows[i][j].ToString());

                                lvItem.ImageIndex = 0;
                                Items.Add(lvItem);
                            }
                        }

                        break;
                    case ConditionTypeItems.Facility:

                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][FACILITY].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][FACILITY_DESC].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }

                        break;
                    case ConditionTypeItems.Corporation:

                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CORP_CODE].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CORP_DESC].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.Customer:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CORP_CODE].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CORP_DESC].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.Vendor:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CORP_CODE].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CORP_DESC].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.Device:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][DEVICE].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][DEVICE_DESC].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.Product:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PRODUCT].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PRODUCT_DESC].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][VERSION].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][C_PRODUCT].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][C_PRODUCT_DESC].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.Flow:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");

                            lvItem.SubItems.Add(dtDataSource.Rows[i][FLOW].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][FLOW_DESC].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_TIME].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_TIME].ToString());

                            #region " ListViewItem.SubItem.Name이 ListViewItem.Clone()을 통해서는 복사되지 않기 때문에 의미가 없어진 비운의 코드 "
                            //(subItem = new ListViewItem.ListViewSubItem(lvItem, dtDataSource.Rows[i][FLOW].ToString())).Name = FLOW;
                            //lvItem.SubItems.Add(subItem);
                            //(subItem = new ListViewItem.ListViewSubItem(lvItem, dtDataSource.Rows[i][FLOW_DESC].ToString())).Name = FLOW_DESC;
                            //lvItem.SubItems.Add(subItem);
                            //(subItem = new ListViewItem.ListViewSubItem(lvItem, dtDataSource.Rows[i][CREATE_USER_ID].ToString())).Name = CREATE_USER_ID;
                            //lvItem.SubItems.Add(subItem);
                            //(subItem = new ListViewItem.ListViewSubItem(lvItem, dtDataSource.Rows[i][CREATE_TIME].ToString())).Name = CREATE_TIME;
                            //lvItem.SubItems.Add(subItem);
                            //(subItem = new ListViewItem.ListViewSubItem(lvItem, dtDataSource.Rows[i][UPDATE_USER_ID].ToString())).Name = UPDATE_USER_ID;
                            //lvItem.SubItems.Add(subItem);
                            //(subItem = new ListViewItem.ListViewSubItem(lvItem, dtDataSource.Rows[i][UPDATE_TIME].ToString())).Name = UPDATE_TIME;
                            //lvItem.SubItems.Add(subItem);
                            #endregion

                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.Oper:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][OPER].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][OPER_DESC].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_TIME].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_TIME].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.Resource:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][RES_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][RES_DESC].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_TIME].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_TIME].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.Para:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PARA_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PARA_DESC].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][DATA_TYPE].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][COLL_TYPE].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][SAMPLE_SIZE].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UNIT].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PARA_GROUP_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][DEFECT_FLAG].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_TIME].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_TIME].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.ParaGroup:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PARA_GROUP_DESC].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PARA_GROUP_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][CREATE_TIME].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][UPDATE_TIME].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                    case ConditionTypeItems.User:
                        for (int i = 0; i < dtDataSource.Rows.Count; i++)
                        {
                            lvItem = new ListViewItem(" ");
                            lvItem.SubItems.Add(dtDataSource.Rows[i][USER_ID].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][USER_NAME].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][USER_GROUP].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PHONE_OFFICE].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PHONE_MOBILE].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PHONE_HOME].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][PHONE_OTHER].ToString());
                            lvItem.SubItems.Add(dtDataSource.Rows[i][EMAIL_ID].ToString());
                            lvItem.ImageIndex = 0;

                            Items.Add(lvItem);
                        }
                        break;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                helper.InitConditionValue();
            }
        }

        private void SetColumnWidth()
        {
            if (dtDataSource == null || dtDataSource.Rows.Count < 1)
                return;

            int iMaxLength = 0;
            int iTemp = 0;
            //int iMaxIndex = -1;

            try
            {
                using (Graphics g = this.CreateGraphics())
                {
                    if (lstColumnItems.Count > 0)
                    {
                        for (int i = 1; i < this.Columns.Count; i++)
                        {
                            iMaxLength = (int)g.MeasureString(this.Columns[i].Text, this.Font).Width;

                            foreach (DataRow row in dtDataSource.Rows)
                            {
                                iTemp = (int)g.MeasureString(row[lstColumnItems[i - 1].DataTableHeader].ToString(), this.Font).Width;
                                if (iTemp > iMaxLength)
                                {
                                    iMaxLength = iTemp;
                                }
                            }

                            this.Columns[i].Width = iMaxLength + 15;
                        }
                    }
                    else
                    {
                        for (int i = 1; i < this.Columns.Count; i++)
                        {
                            iMaxLength = (int)g.MeasureString(this.Columns[i].Text, this.Font).Width;

                            foreach (DataRow row in dtDataSource.Rows)
                            {
                                iTemp = (int)g.MeasureString(row[i - 1].ToString(), this.Font).Width;
                                if (iTemp > iMaxLength)
                                {
                                    iMaxLength = iTemp;
                                }
                            }

                            this.Columns[i].Width = iMaxLength + 15;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region IConditionOwner Members

        private ConditionHelper helper = new ConditionHelper();

        [Category("Setup")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ICondition ConditionInfo
        {
            get { return helper; }
            //set { helper = (ConditionHelper)value; }
        }

        public void Reset()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                helper.ReadRefValues();
                DataSource = helper.GetDataTable(helper.A_ConditionType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion
    }

    public struct DynamicColumnItem
    {
        private string strListViewHeader;
        private string strDataTableHeader;

        public DynamicColumnItem(string dataTableHeader, string listViewHeader)
        {
            strListViewHeader = listViewHeader;
            strDataTableHeader = dataTableHeader;
        }

        public string ListViewHeader
        {
            get { return strListViewHeader; }
            set { strListViewHeader = value; }
        }

        public string DataTableHeader
        {
            get { return strDataTableHeader; }
            set { strDataTableHeader = value; }
        }

    }
}
