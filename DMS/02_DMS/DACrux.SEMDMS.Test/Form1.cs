using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnParseStart_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(TxtPath.Text.Trim()) == true)
                    TxtPath.Text = @"D:\HitekResultFile\A9078510________@TRET____05.smf";

                FileInfo ofile = null;
                ofile = new FileInfo(TxtPath.Text.Trim());
                if (ofile.Exists == false)
                    return;

                DACrux.SEMDMS.KtoD.DataParser parser = new KtoD.DataParser();

                List<string> lsItem = new List<string>();
                lsItem.Add("-K");
                //lsItem.Add(@"D:\HitekResultFile\KLA\A9078510________@TRET____05.smf");
                //lsItem.Add(@"D:\HitekResultFile\KLA\A9078510________@TRET____24.smf");
                lsItem.Add(TxtPath.Text.Trim());
                    
                lsItem.Add("-C");
                lsItem.Add("INSERT");
                lsItem.Add("-S");
                lsItem.Add("");
                lsItem.Add("-T");
                lsItem.Add("INSPECTION");
                lsItem.Add("-B");
                lsItem.Add(@"D:\HitekResultFile\Backup");
                lsItem.Add("-L");
                lsItem.Add("0");
                lsItem.Add("-F");
                lsItem.Add(ofile.Extension);
                parser.TestMain(lsItem.ToArray());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            TxtPath.Clear();
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtPath.Text = dlg.FileName;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BtnParseStart_Click(null, null);
        }
    }
}
