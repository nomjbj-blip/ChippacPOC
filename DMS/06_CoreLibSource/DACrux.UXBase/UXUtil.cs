using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.UXBase
{
    internal static class UXUtil
    {
        public static void Translation(Control.ControlCollection oControls)
        {
            try
            {
                foreach (Control ct in oControls)
                {
                    if (ct.Controls != null && ct.Controls.Count > 0)
                    {
                        Translation(ct.Controls);
                    }

                    if (ct.GetType() == typeof(Label)
                        || ct.GetType() == typeof(Button)
                        || ct.GetType() == typeof(CheckBox)
                        || ct.GetType() == typeof(RadioButton))
                    {
                        if (ct.Text != null)
                        {
                            ct.Text = DACrux.Base.MultiLanguage.Translation(ct.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
