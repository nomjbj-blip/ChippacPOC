using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.ENGUI
{
    public partial class frmSelectMapID : DACrux.Framework.Base.DACruxUXBasic01
    {
        private string m_strSelectedMapID = string.Empty;

        public string SelectedMapID()
        {
            return m_strSelectedMapID;
        }

        public frmSelectMapID()
        {
            InitializeComponent();
            Load_MapID_List();
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void lstMapID_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Load_MapID_List()
        {
            RO.ProbeAdmin oProbe = null;
            try
            {
                DataTable dt = null;
                oProbe = new RO.ProbeAdmin();

                dt = oProbe.GetMapIDList();

                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add(new DataColumn("MAPID", System.Type.GetType("System.String")));
                }

                lstMapID.DataSource = dt;
                lstMapID.DisplayMember = "MAPID";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void lstMapID_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_strSelectedMapID = lstMapID.Text;
        }
        
    }
}
