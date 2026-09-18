using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace QMS.WinControl
{
    /// <summary>
    /// Class Name : FlagEnumUITypeEditor<br/>
    /// Summary    : Flag Enum item Selection UI Type Control Class<br/>
    /// Author     : 미라콤 양형석<br/>
    /// First Date : 2009-11-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class FlagEnumUITypeEditor : UITypeEditor
    {
        #region " MEMBER FIELD "

        IWindowsFormsEditorService editorService = null;
        private FlagEnumCheckedListBox clb;

        #endregion

        #region " CREATOR "

        public FlagEnumUITypeEditor()
        {
            clb = new FlagEnumCheckedListBox();
            clb.BorderStyle = BorderStyle.None;
        }

        #endregion

        #region " METHOD "

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
                editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (editorService != null)
            {
                Enum e = (Enum)Convert.ChangeType(value, context.PropertyDescriptor.PropertyType);
                clb.EnumValue = e;
                editorService.DropDownControl(clb);

                return clb.EnumValue;
            }

            return null;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        #endregion
    }
}
