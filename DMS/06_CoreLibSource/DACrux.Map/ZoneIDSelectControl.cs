using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Map
{
    public partial class ZoneIDSelectControl : UserControl
    {
        private DefectMap _map;
        private bool _isBinding;

        public ZoneIDSelectControl()
        {
            InitializeComponent();
        }

        public void BindingList()
        {
            lstZoneItem.Items.Clear();
            lstZoneItem.Items.AddRange(Map.ZoneConfig.ZoneItemList.ToZoneIDArray());

            chkAll.Checked = false;
            chkAll.Checked = true;
        }

        private void ZoneIDListControl_Load(object sender, EventArgs e)
        {
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            _isBinding = true;

            try
            {
                for (int i = 0; i < lstZoneItem.Items.Count; i++)
                {
                    lstZoneItem.SetItemChecked(i, chkAll.Checked);
                    Map.ZoneConfig.ZoneItemList[i].Visible = chkAll.Checked;
                }

                Map.ClearSelectedDefect();
                Map.Redraw();
            }
            finally
            {
                _isBinding = false;
            }
            
        }

        private void chkShowZoneID_CheckedChanged(object sender, EventArgs e)
        {
            if (Map != null)
            {
                Map.VisibleZoneID = chkShowZoneID.Checked;
                Map.ClearSelectedDefect();
                Map.Redraw();
            }
        }

        private void lstZoneItem_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_isBinding && Map != null)
            {
                Map.ZoneConfig.ZoneItemList[lstZoneItem.Items[e.Index].ToString()].Visible = e.NewValue == CheckState.Checked;
                Map.ClearSelectedDefect();
                Map.Redraw();
            }
        }

        private void Map_ZoneItemListChanged(object sender, EventArgs e)
        {
            BindingList();
        }

        [Browsable(false)]
        public DefectMap Map
        {
            get 
            {
                return _map; 
            }
            set
            {
                if (_map != null && _map != value)
                    _map.ZoneConfig.ZoneItemListChanged -= Map_ZoneItemListChanged;

                if (value != null && _map != value)
                {
                    _map = value;
                    value.ZoneConfig.ZoneItemListChanged += Map_ZoneItemListChanged;
                    BindingList();
                }

                _map = value;
            }
        }
    }
}
