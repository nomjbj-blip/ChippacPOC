using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace DACrux.SP.Controls
{
    public class RichTextBoxEx : RichTextBox
    {
        //private string strRegex = string.Empty;

        //public string Regex
        //{
        //    get { return strRegex; }
        //    set
        //    {
        //        try
        //        {
        //            ClearSelection();

        //            if (string.IsNullOrEmpty(value))
        //                return;

        //                strRegex = value;
        //                SetSelection();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }
        //}

        //private void SetSelection()
        //{

        //    ClearSelection();

        //    Regex regex = new Regex(strRegex, RegexOptions.Singleline);

        //    foreach (Match match in regex.Matches(Text))
        //    {
        //        Select(match.Index, match.Length);
                 
        //    }
        //}
        //int iCurrPos = 0;
        //private void ClearSelection()
        //{
        //    iCurrPos = this.
        //    SelectionColor = SystemColors.ControlText;
        //    SelectAll();
        //    Select(0, 0);
        //    SelectionColor = Color.Blue;

        //}
    }
}
