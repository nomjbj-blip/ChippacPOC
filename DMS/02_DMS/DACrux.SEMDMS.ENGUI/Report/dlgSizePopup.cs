using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.Controls;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class dlgSizePopup : Form
    {
        public dlgSizePopup()
        {
            InitializeComponent();
        }

        public dlgSizePopup(
            double dMaxinum,
            double dMininum,
            int interval
            )
            : this()
        {
            Maxinum = dMaxinum;
            Mininum = dMininum;
            Interval = interval;
        }

        #region [ Event Handler ]
        private void dlgSizePopup_Load(
            object sender,
            EventArgs e
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("DESCRIPTION", typeof(string)));
            dt.Columns.Add(new DataColumn("MININUM", typeof(double)));
            dt.Columns.Add(new DataColumn("MAXINUM", typeof(double)));


            double dMininum = Mininum;
            double dDiff = (Maxinum - Mininum) / Interval;
            for (int idx = 0; idx < Interval; idx++)
            {
                DataRow row = dt.NewRow();
                row["DESCRIPTION"] = String.Format("{0} <= x < {1}", dMininum, dMininum + dDiff);
                row["MININUM"] = dMininum;
                row["MAXINUM"] = dMininum + dDiff;
                dt.Rows.Add(row);

                dMininum += dDiff;
            }
            dt.AcceptChanges();
            ducListBox1.DisplayMember = "DESCRIPTION";
            ducListBox1.ValueMember = "DESCRIPTION";
            ducListBox1.DataSource = dt;
        }
        #endregion [ Event Handler ]


        #region [ Method ]

        //----------------------------------------------------------------------------------------------------------

        private SizeOptionList GetSelectedValues(
            DUCListBox listBox
            )
        {
            DataTable dt = listBox.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
                return null;

            object[] arr = listBox.SelectedValues;
            SizeOptionList results = new SizeOptionList();
            if (arr == null || arr.Length == 0)
            {
                for (int idx = 0; idx < listBox.Items.Count; idx++)
                {
                    DataRowView view = listBox.Items[idx] as DataRowView;
                    if (view == null)
                        continue;

                    results.Add(
                        new SizeOption()
                        {
                            RANGENAME = view.Row[0].ToString(),
                            MININUM = Base.Convert.doubleParse(view.Row[1].ToString()),
                            MAXINUM = Base.Convert.doubleParse(view.Row[2].ToString())
                        });
                }
            }
            else
            {
                for (int idx = 0; idx < listBox.SelectedItems.Count; idx++)
                {
                    DataRowView view = listBox.SelectedItems[idx] as DataRowView;
                    if (view == null)
                        continue;

                    results.Add(
                        new SizeOption()
                        {
                            RANGENAME = view.Row[0].ToString(),
                            MININUM = Base.Convert.doubleParse(view.Row[1].ToString()),
                            MAXINUM = Base.Convert.doubleParse(view.Row[2].ToString())
                        });
                }
            }
            return results;
        }

        //----------------------------------------------------------------------------------------------------------

        #endregion [ Method ]

        #region [ Property ]
        public double Maxinum
        {
            get;
            private set;
        }

        //----------------------------------------------------------------------------------------------------------

        public double Mininum
        {
            get;
            private set;
        }

        //----------------------------------------------------------------------------------------------------------

        public int Interval
        {
            get;
            private set;
        }

        //----------------------------------------------------------------------------------------------------------

        internal SizeOptionList SeletedValues
        {
            get { return GetSelectedValues(ducListBox1); }
        }
        #endregion  [ Property ]
    }
}
