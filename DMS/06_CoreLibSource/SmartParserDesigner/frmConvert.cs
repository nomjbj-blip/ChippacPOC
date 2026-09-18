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

    public partial class frmConvert : Form
    {
        #region " Member Field & Property "

        static private Analysis analysis = null;
        static private RegexEntity entitys;
        static private string strFilename;
        public RegexEntity entity
        {
            get { return entitys; }
            set { entitys = value; }
        }
        public string Filename
        {
            get { return strFilename; }
            set { strFilename = value; }
        }
        public Analysis analy
        {
            get { return analysis; }
            set { analysis = value; }
        }
        #endregion

        #region " Creator "

        public frmConvert()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();
            funclang.CheckLang();
            InitializeComponent();
            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
            this.FormClosed += new FormClosedEventHandler(frmConvert_FormClosed);
        }

        #endregion

        #region " Event Handler "
        void frmConvert_FormClosed(object sender, FormClosedEventArgs e)
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
        void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                    return;

                BinCountCheck(int.Parse(textBox1.Text));
            }
            catch (FormatException)
            {
                textBox1.Text = "1";
                DACrux.SP.Common.Utility.ShowMessageBox("the value is not correct", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.ToString(), MessageBoxIcon.Error);
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Normal;
                BinCountCheck(1);
                //this.TopMost = true;
                //this.Size = new Size(400, 500);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void btnCount_Click(object sender, EventArgs e)
        {


            if (BinCheckList.CheckedItems.Count <= 0)
                return;

            try
            {

                string[] strCountValue = new string[BinCheckList.CheckedItems.Count];
                for (int listcount = 0; listcount < BinCheckList.CheckedItems.Count; listcount++)
                {
                    strCountValue[listcount] = BinCheckList.CheckedItems[listcount].ToString();
                }
                var lst = from DACrux.SP.Common.Token mt in analysis.Navigators[strFilename].EntityCaptureCollection[entitys]
                          select new int[] { mt.Index, mt.Length };

                String strTempText = null;
                List<int[]> list = new List<int[]>();
                list = lst.ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("Bin");
                dt.Columns.Add("Count", typeof(int));
                int totalcount = 0;
                for (int CountValue = 0; CountValue < strCountValue.Length; CountValue++)
                {
                    DataRow dr = dt.NewRow();
                    int ValueLength = strCountValue[CountValue].Length;
                    int BinCount = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        int[] test = list[i];
                        strTempText = analysis.SampleFiles[strFilename].ToString().Substring(test[0], test[1]);
                        for (int strCount = 0; strCount < strTempText.Length; strCount += ValueLength)
                        {
                            if (strTempText.Substring(strCount, ValueLength) == strCountValue[CountValue])
                                BinCount += 1;
                        }

                    }
                    dr["Bin"] = strCountValue[CountValue].ToString();
                    dr["Count"] = BinCount;
                    totalcount += BinCount;
                    dt.Rows.Add(dr);

                }
                DataRow drs = dt.NewRow();
                drs["Bin"] = "Sum";
                drs["Count"] = totalcount;
                dt.Rows.Add(drs);
                BinCountView.DataSource = dt;
                //MessageBox.Show(string.Format("BinCount {0}", BinCount.ToString()));
            }
            catch (Exception ex)
            {

                DACrux.SP.Common.Utility.ShowMessageBox(ex.ToString(), MessageBoxIcon.Error);
            }

        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                string goodbin = textBox2.Text;
                string nullbin = textBox3.Text;
                string failbin = textBox4.Text;
                if (!string.IsNullOrEmpty(goodbin))
                    frmRegistration.GoodBin = goodbin;
                if (!string.IsNullOrEmpty(nullbin))
                    frmRegistration.NullBin = nullbin;
                if (!string.IsNullOrEmpty(failbin))
                    frmRegistration.FailBin = failbin;
                if (!string.IsNullOrEmpty(txtTop.Text))
                    frmRegistration.strTop = txtTop.Text;
                if (!string.IsNullOrEmpty(txtBottom.Text))
                    frmRegistration.strBottom = txtBottom.Text;
                if (!string.IsNullOrEmpty(txtLeft.Text))
                    frmRegistration.strLeft = txtLeft.Text;
                if (!string.IsNullOrEmpty(txtRight.Text))
                    frmRegistration.strRight = txtRight.Text;
                this.Close();
            }
            catch (Exception ex)
            {

                DACrux.SP.Common.Utility.ShowMessageBox(ex.ToString(), MessageBoxIcon.Error);
            }

        }

        #region " Method "
        private void BinCountCheck(int length)
        {
            try
            {
                BinCheckList.Items.Clear();
                var lst = from DACrux.SP.Common.Token mt in analysis.Navigators[strFilename].EntityCaptureCollection[entitys]
                          select new int[] { mt.Index, mt.Length };

                String strTempText = null;
                List<int[]> list = new List<int[]>();
                list = lst.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    int[] test = list[i];
                    strTempText = analysis.SampleFiles[strFilename].ToString().Substring(test[0], test[1]);
                    if (strTempText.Length % length > 0)
                    {
                        MessageBox.Show(string.Format("This map is not divided by {0}.", length.ToString()), "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    analysis.CharCount = int.Parse(textBox1.Text);
                    for (int strCount = 0; strCount < strTempText.Length; strCount += length)
                    {
                        if (strCount + length < strTempText.Length)
                        {
                            if (BinCheckList.Items.Count <= 0)
                            {
                                if (!(strTempText.Substring(strCount, length) == "\n"))
                                    BinCheckList.Items.Add(strTempText.Substring(strCount, length));
                            }
                            else
                            {
                                bool checkadd = true;
                                for (int j = 0; j < BinCheckList.Items.Count; j++)
                                {
                                    if (BinCheckList.Items[j].ToString().Equals(strTempText.Substring(strCount, length)))
                                    {
                                        checkadd = false;
                                        break;
                                    }
                                }
                                if (checkadd)
                                    BinCheckList.Items.Add(strTempText.Substring(strCount, length));

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
