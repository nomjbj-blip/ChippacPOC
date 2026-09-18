using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }

    public static class ControlExtensions
    {
        public static void Reset(this Control control)
        {
            ResetControl(control, null);
        }

        public static void Reset(this Control control, params Control [] exceptControlList)
        {
            ResetControl(control, exceptControlList);
        }

        private static void ResetControl(Control control, params Control[] exceptControlList)
        {
            foreach (Control ctl in control.Controls)
            {
                if (exceptControlList != null && exceptControlList.Contains(ctl))
                    continue;

                if (ctl.Controls.Count > 0)
                    ResetControl(ctl, exceptControlList);
                else
                {
                    if (ctl is TextBox)
                        ctl.Text = string.Empty;
                    else if (ctl is CheckBox)
                    {
                        if (!ctl.Name.ToString().Equals("chkStreamMode"))
                            (ctl as CheckBox).Checked = false;
                        else
                            (ctl as CheckBox).Checked = true;
                    }
                    else if (ctl is ComboBox)
                    {
                        (ctl as ComboBox).Items.Clear();
                        (ctl as ComboBox).Text = string.Empty;
                    }
                    else if (ctl is ListView)
                        (ctl as ListView).Items.Clear();
                }
            }
        }
    }
}
