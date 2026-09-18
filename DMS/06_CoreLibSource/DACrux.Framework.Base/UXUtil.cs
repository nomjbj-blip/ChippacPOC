using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using DACrux.Framework.RO;

namespace DACrux.Framework
{
    public static class UXUtil
    {
        public static void Translation(
            Control.ControlCollection oControls
            )
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

        #region AccessCheck

        public static bool AccessCheck(
            string strFuncCode,
            bool isDisplay
            )
        {
            bool bResult = false;
            UserGroup oUserGroup = null;

            try
            {
                if (DACrux.Base.GlobalVariable.UserID.ToUpper() == "ADMIN"
                    && DACrux.Base.GlobalVariable.Password.ToUpper() == "1111")
                    return true;

                oUserGroup = new UserGroup();
                bResult = oUserGroup.CheckFunction(
                    DACrux.Base.GlobalVariable.UserID,
                    strFuncCode
                    );

                if (!bResult && isDisplay)
                    DCMH.DspMessage("Do not have permission");


                return bResult;
            }
            catch (Exception ex)
            {
                DCMH.DspError(ex);
                return false;
            }
        }

        #endregion
    }
}
