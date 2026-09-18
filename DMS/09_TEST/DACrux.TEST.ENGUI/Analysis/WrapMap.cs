using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.ENGUI
{
    public partial class WrapMap : UserControl
    {
        bool single = false;

        public delegate void EventHandlerSelected(WrapMap map);

        public event EventHandlerSelected OnSelected;
        public event EventHandlerSelected OnDblClick;
        public string TestWaferSeq = "";
        public string TestWaferID = "";

        bool bSelected = false;
        int mapId = -1;


        public bool Single
        {
            get
            {
                return single;
            }
            set
            {
                single = value;
            }
        }

        public bool Selected
        {
            get
            {
                return bSelected;
            }
            set
            {
                MapSelected(value);
            }
        }

        public int MapId
        {
            get
            {
                return mapId;
            }
            set
            {
                mapId = value;
            }
        }

        public DACrux.Map.WaferMap MapObj
        {
            get
            {
                return waferMap;
            }
        }


        public WrapMap()
        {
            InitializeComponent();
        }



        private void waferMap_MouseEnter(object sender, System.EventArgs e)
        {
            if (Single) return;
            if (!bSelected) this.BackColor = Color.Pink;
        }

        private void waferMap_MouseLeave(object sender, System.EventArgs e)
        {
            if (Single) return;
            if (!bSelected) this.BackColor = Color.White;
        }

        private void Map_MouseEnter(object sender, System.EventArgs e)
        {
            if (Single) return;
            if (!bSelected) this.BackColor = Color.Pink;
        }

        private void Map_MouseLeave(object sender, System.EventArgs e)
        {
            if (Single) return;
            if (!bSelected) this.BackColor = Color.White;
        }

        void MapSelected()
        {
            if (Single) return;
            if (!bSelected)
            {
                this.BackColor = Color.Blue;
            }
            else
            {
                this.BackColor = Color.White;
            }

            bSelected = !bSelected;
        }

        void MapSelected(bool selected)
        {
            if (Single) return;
            if (selected)
            {
                this.BackColor = Color.Blue;
            }
            else
            {
                this.BackColor = Color.White;
            }

            bSelected = selected;
        }

        private void waferMap_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) if (OnSelected != null) OnSelected(this);
        }

        private void waferMap_DoubleClick(object sender, System.EventArgs e)
        {
            if (OnDblClick != null) OnDblClick(this);
        }

        private void waferMap_Load(object sender, EventArgs e)
        {
        }
    }
}
