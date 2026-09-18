using System;
using System.Windows.Forms;
using DACrux.SP.Common;
using System.Xml;
using System.Data;
using System.IO;

namespace SmartParser.Designer
{
    public partial class frmLang : Form
    {
        string formatfile = Application.StartupPath +"\\"+ "Lang.xml";
        #region " Member Field & Property "

        //private Analysis analysis = null;

        #endregion

        #region " Creator "

        public frmLang()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();
            

        }

        #endregion

        #region " Event Handler "

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(formatfile);
                if(!fi.Exists)
                    fi.Create();
                XmlDataDocument xmlDatadoc = new XmlDataDocument();

                xmlDatadoc.DataSet.ReadXml(formatfile);

                DataSet ds = new DataSet();

                ds = xmlDatadoc.DataSet;
                //ds.Tables[0].Rows.RemoveAt(ds.Tables[0].Rows.Count - 1);
                dataGridView1.DataSource = ds.Tables[0];
                //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                //dataGridView1.Columns[0].ReadOnly = true;
                //dataGridView1.DataMember = "Book";
                xmlDatadoc.DataSet.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     

        #endregion

        private void ColAdd_Click(object sender, EventArgs e)
        {
            DataGridViewColumn textcol = new DataGridViewColumn();
            textcol.HeaderText = textBox1.Text;
            textcol.CellTemplate = new DataGridViewTextBoxCell(); 
             dataGridView1.Columns.Add(textcol);
        }

        private void colDel_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedCells[0].ColumnIndex<=0)
            {
                MessageBox.Show("This column cannot be deleted.","Infomation",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
            }
            else
             dataGridView1.Columns.RemoveAt(dataGridView1.SelectedCells[0].ColumnIndex);
        }

        private void rowDel_Click(object sender, EventArgs e)
        {
           dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
        }

    
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.TableName = "XMLLang";

            for (int colCnt = 0; colCnt < dataGridView1.Columns.Count; colCnt++)
            {
                DataColumn dc = new DataColumn(dataGridView1.Columns[colCnt].HeaderText);
                dt.Columns.Add(dc);
            }

            DataRow dr;
            for (int rowCnt = 0; rowCnt < dataGridView1.Rows.Count; rowCnt++)
            {
                dr = dt.NewRow();
                for (int colCnt = 0; colCnt < dataGridView1.Columns.Count; colCnt++)
                {

                    dr[dataGridView1.Columns[colCnt].HeaderText] = dataGridView1.Rows[rowCnt].Cells[colCnt].Value;

                }
                dt.Rows.Add(dr);
            }



            if (dt == null)
            {
                MessageBox.Show("Data is Empty");
                return;
            }
            if (string.IsNullOrEmpty(dt.Rows[dt.Rows.Count - 1][0].ToString()))
            {
                dt.Rows[dt.Rows.Count - 1].Delete();
            }
            //ds.WriteXml(formatfile);
            dt.WriteXml(formatfile);
        }

     

        #region " Method "

        #endregion
    }
}
