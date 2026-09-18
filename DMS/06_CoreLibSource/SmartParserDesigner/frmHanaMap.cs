using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Resources;
using DACrux.SP.Common;
using DACrux.SP.Controls;

namespace SmartParser.Designer
{
    public partial class frmHanaMap : Form
    {
        #region " Member Field & Property "

        //private Analysis analysis = null;
        static private string strHanaMap = null;
        private readonly string HEADER = @"{\rtf1\ansi\ansicpg1252\deff0{\fonttbl{\f0\fswiss\fcharset0 Consolas;}{\f1\fnil\fcharset129 \'b1\'bc\'b8\'b2\'c3\'bc;}{\f2\fbidi \fswiss\fcharset128\fprq1 Consolas;}{\f3\fbidi \fswiss\fcharset129\fprq1 Consolas{\*\panose 020b0609000101010101}\'b1\'bc\'b8\'b2\'c3\'bc;}}{\colortbl ;";
        private readonly string HEADER_COLOR = @"\red255\green255\blue255;\red0\green0\blue0;\red252\green124\blue124;";
        private readonly string HEADER_CLOSURE = @"}\viewkind4\uc1\pard\cf0\highlight1\lang1042\f0\fs18 ";
        private readonly string FOOTER = @"\cf0\highlight1\f1\par}";
        #endregion

        #region " Creator "

        public frmHanaMap(StringBuilder sb)
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();
            strHanaMap = sb.ToString();
            this.Load += new EventHandler(frmHanaMap_Load);
            
            this.FormClosed += new FormClosedEventHandler(frmHanaMap_FormClosed);

        }

       

       

        #endregion

        #region " Event Handler "
        void frmHanaMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Form pf = this.ParentForm;
                foreach (Form fr in pf.MdiChildren)
                {
                    if (fr.Name == "frmRegistration")
                    {
                        fr.WindowState = FormWindowState.Maximized;
                        fr.Select();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

                DACrux.SP.Common.Utility.ShowMessageBox(ex.ToString(), MessageBoxIcon.Error);
            }


        }
        void frmHanaMap_Load(object sender, EventArgs e)
        {
            try
            {

                this.WindowState = FormWindowState.Normal;
                //this.Size = new Size(400, 800);
                int a =Screen.PrimaryScreen.WorkingArea.Width;
                int b = this.ParentForm.Size.Width;
                this.Location = new Point(b-this.Size.Width-30, 0);
                HanaMapShow();
                this.Select();
                //this.ParentForm.LayoutMdi(MdiLayout.ArrangeIcons);
            }
            catch (Exception ex)
            {

                DACrux.SP.Common.Utility.ShowMessageBox(ex.ToString(), MessageBoxIcon.Error);
            }
        }
        #endregion

        #region " Method "

        void HanaMapShow()
        {
            try
            {
                richHana.Rtf = HEADER + HEADER_COLOR + HEADER_CLOSURE + strHanaMap.Replace("\n", "\\par\n").Replace("}", @"\}").Replace("{", @"\{").ToString() + FOOTER;
                //richHana.Text = strHanaMap;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
        }
        #endregion
    }
}
