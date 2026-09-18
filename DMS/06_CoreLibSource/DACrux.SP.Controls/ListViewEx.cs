using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DACrux.SP.Controls
{
    public class ListViewEx : ListView
    {
        #region " Member Field "

        private ListViewItem item;

        string subItemText = "";
        int selectedSubItem = 0;

        private int X = 0;
        private int Y = 0;

        Dictionary<string, ComboBox> dicComboBox = new Dictionary<string, ComboBox>();

        #endregion

        #region " Creator "

        public ListViewEx()
        {
            this.Name = "listViewWithComboBox1";
            this.Size = new System.Drawing.Size(0, 0);
            this.TabIndex = 0;
            this.View = System.Windows.Forms.View.Details;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListViewMouseDown);
            this.DoubleClick += new System.EventHandler(this.ListViewDoubleClick);
            this.GridLines = true;
            this.FullRowSelect = true;
        }

        #endregion

        #region " Method "

        public void AddComboBox(string columnName, int width, ComboBox cbo)
        {
            try
            {
                if (string.IsNullOrEmpty(columnName))
                    throw new ArgumentException("Column Name is null.");

                if (cbo == null)
                    throw new ArgumentException("ComboBox is null.");

                cbo.KeyPress += ComboBoxKeyPressed;
                cbo.LostFocus += ComboBoxFocusExit;
                cbo.SelectedIndexChanged += ComboBoxSelected;
                cbo.Size = new System.Drawing.Size(0, 0);
                cbo.Location = new System.Drawing.Point(0, 0);
                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                cbo.Hide();

                this.Controls.AddRange(new System.Windows.Forms.Control[] { cbo });

                ColumnHeader columnHeader1 = new System.Windows.Forms.ColumnHeader();

                Columns.Add(columnHeader1);

                columnHeader1.Text = columnName;
                columnHeader1.Width = width;

                dicComboBox.Add(columnName, cbo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region " Event Handler "

        private void ComboBoxKeyPressed(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // 13 enter
            // 27 esc
            if (e.KeyChar == 13 || e.KeyChar == 27)
            {
                foreach (Control ctl in this.Controls)
                {
                    if (ctl is ComboBox)
                    {
                        ctl.Hide();
                        break;
                    }
                }
            }
        }

        private void ComboBoxSelected(object sender, System.EventArgs e)
        {
            ComboBox cbo = sender as ComboBox;

            int i = cbo.SelectedIndex;
            if (i >= 0)
            {
                string str = cbo.Items[i].ToString();
                item.SubItems[selectedSubItem].Text = str;
            }
        }

        private void ComboBoxFocusExit(object sender, System.EventArgs e)
        {
            (sender as ComboBox).Hide();
        }

        public void ListViewDoubleClick(object sender, System.EventArgs e)
        {
            // Check whether the subitem was clicked
            int start = X;
            int position = 0;
            int end = this.Columns[0].Width;
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (start > position && start < end)
                {
                    selectedSubItem = i;
                    break;
                }

                position = end;
                end += this.Columns[i].Width;
            }

            subItemText = item.SubItems[selectedSubItem].Text;

            string column = this.Columns[selectedSubItem].Text;

            Rectangle r = new Rectangle(position, item.Bounds.Top, end, item.Bounds.Bottom);
            dicComboBox[column].Size = new System.Drawing.Size(end - position, item.Bounds.Bottom - item.Bounds.Top);
            dicComboBox[column].Location = new System.Drawing.Point(position, item.Bounds.Y);
            dicComboBox[column].Show();
            dicComboBox[column].Text = subItemText;
            dicComboBox[column].SelectAll();
            dicComboBox[column].Focus();
        }

        public void ListViewMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            item = this.GetItemAt(e.X, e.Y);
            X = e.X;
            Y = e.Y;
        }

        #endregion
    }
}
