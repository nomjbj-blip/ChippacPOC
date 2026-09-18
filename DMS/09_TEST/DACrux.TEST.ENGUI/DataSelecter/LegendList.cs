using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace DACrux.TEST.ENGUI.DataSelecter
{
    public class LegendList : UserControl
    {
        private CheckBox chkVisibleAll;
        private Panel pnlList;
        List<LegendItem> _list = new List<LegendItem>();
        private CheckBox chkShowLabelAll;
        private Label label1;
        private Label label2;
        bool _cancel;

        public LegendList()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.chkVisibleAll = new System.Windows.Forms.CheckBox();
            this.pnlList = new System.Windows.Forms.Panel();
            this.chkShowLabelAll = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkVisibleAll
            // 
            this.chkVisibleAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkVisibleAll.Location = new System.Drawing.Point(3, 3);
            this.chkVisibleAll.Name = "chkVisibleAll";
            this.chkVisibleAll.Size = new System.Drawing.Size(13, 22);
            this.chkVisibleAll.TabIndex = 5;
            this.chkVisibleAll.UseVisualStyleBackColor = true;
            this.chkVisibleAll.CheckedChanged += new System.EventHandler(this.chkCheckAll_CheckedChanged);
            // 
            // pnlList
            // 
            this.pnlList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlList.AutoScroll = true;
            this.pnlList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlList.Location = new System.Drawing.Point(1, 44);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(101, 459);
            this.pnlList.TabIndex = 6;
            // 
            // chkShowLabelAll
            // 
            this.chkShowLabelAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowLabelAll.Location = new System.Drawing.Point(3, 20);
            this.chkShowLabelAll.Name = "chkShowLabelAll";
            this.chkShowLabelAll.Size = new System.Drawing.Size(13, 22);
            this.chkShowLabelAll.TabIndex = 7;
            this.chkShowLabelAll.UseVisualStyleBackColor = true;
            this.chkShowLabelAll.CheckedChanged += new System.EventHandler(this.chkShowLabelAll_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "Item Visible";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Label Visible";
            // 
            // LegendList
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkShowLabelAll);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.chkVisibleAll);
            this.Name = "LegendList";
            this.Size = new System.Drawing.Size(103, 506);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public event ItemCheckedChangedEventHandler VisibleCheckedChanged;
        public event ItemCheckedChangedEventHandler ShowLabelCheckedChanged;
        public delegate void ItemCheckedChangedEventHandler(object sender, ItemCheckedChangedEventArgs e);

        protected virtual void OnVisibleCheckedChanged(ItemCheckedChangedEventArgs e)
        {
            if (VisibleCheckedChanged != null)
                VisibleCheckedChanged(this, e);
        }

        protected virtual void OnShowLabelCheckedChanged(ItemCheckedChangedEventArgs e)
        {
            if (ShowLabelCheckedChanged != null)
                ShowLabelCheckedChanged(this, e);
        }

        public void Add(string itemName, Color itemColor)
        {
            Add(new LegendItem() { ItemName = itemName, ItemColor = itemColor });
        }

        private void Add(LegendItem item)
        {
            _cancel = true;

            chkVisibleAll.Checked = true;
            chkShowLabelAll.Checked = false;
            item.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            item.VisibleCheckedChanged += new EventHandler(item_VisibleCheckedChanged);
            item.ShowLabelCheckedChanged += new EventHandler(item_ShowLabelCheckedChanged);
            item.Location = new Point(0, _list.Count * item.Height);

            _list.Add(item);
            pnlList.Controls.Add(item);

            _cancel = false;
        }

        public void Clear()
        {
            pnlList.Controls.Clear();

            foreach (LegendItem item in _list)
            {
                item.VisibleCheckedChanged -= new EventHandler(item_VisibleCheckedChanged);
                item.ShowLabelCheckedChanged -= new EventHandler(item_ShowLabelCheckedChanged);
            }

            _list.Clear();
        }

        private void item_VisibleCheckedChanged(object sender, EventArgs e)
        {
            if (_cancel)
                return;

            OnVisibleCheckedChanged(new ItemCheckedChangedEventArgs(GetVisibleCheckedItemNames()));
        }

        private void item_ShowLabelCheckedChanged(object sender, EventArgs e)
        {
            if (_cancel)
                return;

            OnShowLabelCheckedChanged(new ItemCheckedChangedEventArgs(GetShowLabelCheckedItemNames()));
        }

        private void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_cancel || _list.Count == 0)
                return;

            _cancel = true;

            foreach (LegendItem item in _list)
                item.IsVisibleChecked = chkVisibleAll.Checked;

            _cancel = false;

            OnVisibleCheckedChanged(new ItemCheckedChangedEventArgs(GetVisibleCheckedItemNames()));
        }

        private void chkShowLabelAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_cancel || _list.Count == 0)
                return;

            _cancel = true;

            foreach (LegendItem item in _list)
                item.IsShowLabelChecked = chkShowLabelAll.Checked;

            _cancel = false;

            OnShowLabelCheckedChanged(new ItemCheckedChangedEventArgs(GetShowLabelCheckedItemNames()));
        }

        private string[] GetVisibleCheckedItemNames()
        {
            List<string> list = new List<string>();

            foreach (LegendItem item in _list)
            {
                if (item.IsVisibleChecked)
                    list.Add(item.ItemName);
            }

            return list.ToArray();
        }

        private string[] GetShowLabelCheckedItemNames()
        {
            List<string> list = new List<string>();

            foreach (LegendItem item in _list)
            {
                if (item.IsShowLabelChecked)
                    list.Add(item.ItemName);
            }

            return list.ToArray();
        }
    }

    public class ItemCheckedChangedEventArgs : EventArgs
    {
        public ItemCheckedChangedEventArgs(string[] itemNameArray)
        {
            ItemNameArray = itemNameArray;
        }

        public string[] ItemNameArray
        {
            get;
            private set;
        }
    }

    public class ItemMouseMoveEventArgs : EventArgs
    {
        public ItemMouseMoveEventArgs(LegendItem item)
        {
            LegendItem = item;
        }

        public LegendItem LegendItem { get; private set; }
    }
}
