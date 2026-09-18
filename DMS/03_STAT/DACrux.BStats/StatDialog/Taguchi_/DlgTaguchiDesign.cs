using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.BStats.StatDialog.DlgTaguchi_
{
    public partial class DlgTaguchiDesign : Form
    {
        DataTable m_dtDesignList = null;
        string m_strRun = "";
        public DlgTaguchiDesign()
        {
            InitializeComponent();
            MakeDesignList();
        }

        public void SetDesignCondition(string Level = "2", int FCount = 2)
        {
            ViewList(Level, FCount);
        }

        private void ViewList(string Level, int FCount)
        {
            string strColumn = "LEVEL2";
            try
            {
                switch (Level)
                {
                    case "2":
                        strColumn = "LEVEL2";
                        break;
                    case "3":
                        strColumn = "LEVEL3";
                        break;
                    case "4":
                        strColumn = "LEVEL4";
                        break;
                    case "5":
                        strColumn = "LEVEL5";
                        break;
                    case "M":
                        strColumn = "LEVELM"; // 사용안함
                        break;
                }

                DataView dv = new DataView(m_dtDesignList);
                dv.RowFilter = string.Format("{0} >= {1}", strColumn, FCount);

                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("RUN", typeof(string));
                dtResult.Columns.Add(string.Format("{0} ** 열",Level,FCount) , typeof(string));

                for (int i = 0; i < dv.Count; i++)
                {
                    dtResult.Rows.Add(new object[] {dv[i]["DESIGN"].ToString() , string.Format("{0} ** {1}",Level,dv[i][strColumn].ToString()) });
                }

                dgDesignList.DataSource = dtResult;
                m_strRun = dgDesignList["RUN", 0].Value as string;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private DataTable MakeDesignList()
        {
            try
            {
                m_dtDesignList = new DataTable();
                m_dtDesignList.Columns.Add("TABLE", typeof(string));
                m_dtDesignList.Columns.Add("DESIGN", typeof(string));
                m_dtDesignList.Columns.Add("LEVEL2", typeof(int));
                m_dtDesignList.Columns.Add("LEVEL3", typeof(int));
                m_dtDesignList.Columns.Add("LEVEL4", typeof(int));
                m_dtDesignList.Columns.Add("LEVEL5", typeof(int));
                //dtDesignList.Columns.Add("LEVELM", typeof(int));  // 사용안함

                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L4", 3, -1, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L8", 7, -1, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L9", -1, 4, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L12", 11, -1, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L16", 15, -1, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L16", -1, -1, 5, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L25", -1, -1, -1, 6 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L27", -1, 13, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L32", 31, -1, -1, -1 });
                return m_dtDesignList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Run
        {
            get
            {
                return m_strRun;
            }
            set
            {
                m_strRun = value;
            }
        }

        private void dgDesignList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_strRun = dgDesignList["RUN", e.RowIndex].Value as string;
        }
    }
}
