using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace DACrux.BStats.StatDialog
{
    public partial class DlgTaguchi : Form
    {
        
        DACrux.BStats.StatDialog.DlgTaguchi_.DlgTaguchiDesign m_oDesign = null;
        DACrux.BStats.StatDialog.DlgTaguchi_.DlgTaguchiFactor m_oFactor = null;

        DataTable m_dtDesignList = null;
        DataTable m_TaguchiSheet = null;
        FactorInfo[] m_FactorInfos = null;
        string m_strLevel = "2";
        string m_strRun = "L4";

        public DataTable TaguchiSheet
        {
            get
            {
                return m_TaguchiSheet;
            }
        }

        public FactorInfo[] FactorIngos
        {
            get
            {
                return m_FactorInfos;
            }
            set
            {
                m_FactorInfos = value;
            }
        }


        public DlgTaguchi()
        {
            InitializeComponent();
            MakeDesignList();
        }

        private void rdoDesignType_Click(object sender, EventArgs e)
        {
            int iMax = 31;
            int iLastValue = int.Parse(cmbFactor.Text);
            try
            {
                RadioButton oSel = (RadioButton)sender;

                switch (oSel.Name)
                {
                    case "rdoDesignType2": // 2~31
                        m_strLevel = "2";
                        iMax = 31;
                        m_strRun = "L4"; //default

                        break;
                    case "rdoDesignType3": // 2~13
                        m_strRun = "L9"; //default
                        m_strLevel = "3";
                        iMax = 13;
                        break;
                    case "rdoDesignType4": // 2~5
                        m_strRun = "L16"; //default
                        m_strLevel = "4";
                        iMax = 5;
                        break;
                    case "rdoDesignType5": // 2~6
                        m_strRun = "L25"; //default
                        m_strLevel = "5";
                        iMax = 6;
                        break;
                }

                cmbFactor.Items.Clear();
                for (int i = 2; i <= iMax; i++)
                {
                    cmbFactor.Items.Add(i);
                }

                int idx = cmbFactor.Items.IndexOf(iLastValue);
                if (idx > -1) cmbFactor.SelectedIndex = idx;
                else cmbFactor.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butDesignList_Click(object sender, EventArgs e)
        {
            try
            {
                DACrux.BStats.StatDialog.DlgTaguchi_.DlgTaguchiDesignList oDList = new DlgTaguchi_.DlgTaguchiDesignList();
                oDList.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butDesign_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_oDesign == null)
                {
                    m_oDesign = new DlgTaguchi_.DlgTaguchiDesign();
                }
                if (rdoDesignType2.Checked) m_strLevel = "2";
                if (rdoDesignType3.Checked) m_strLevel = "3";
                if (rdoDesignType4.Checked) m_strLevel = "4";
                if (rdoDesignType5.Checked) m_strLevel = "5";

                m_oDesign.SetDesignCondition(m_strLevel, int.Parse(cmbFactor.Text));
                if (m_oDesign.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_strRun = m_oDesign.Run;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butFactor_Click(object sender, EventArgs e)
        {
            try
            {
                List<FactorInfo> oTmpFactorInfo = new List<FactorInfo>();
                if (m_oFactor == null)
                {
                    m_oFactor = new DlgTaguchi_.DlgTaguchiFactor();
                }

                m_oFactor.SetFactorInfo(m_FactorInfos, m_strRun);

                if (m_oFactor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateFactorInfo()
        {
            int iFactors = int.Parse(cmbFactor.Text);
            try
            {
                List<FactorInfo> tmp = new List<FactorInfo>();

                for (int i = 0; i < iFactors; i++)
                {
                    string id = string.Format("{0}",MakeID(i));
                    FactorInfo oNew = new FactorInfo(id, id, MakeLevelValue(m_strLevel), i + 1, int.Parse(m_strLevel));
                    tmp.Add(oNew);
                }

                DataView dv = new DataView(m_dtDesignList);
                dv.RowFilter = string.Format("LEVEL{0} >= {1}", m_strLevel, tmp.Count);
                m_strRun = dv[0]["DESIGN"].ToString();

                m_FactorInfos = tmp.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string MakeLevelValue(string Level)
        {
            int iLevel = int.Parse(Level);
            string strLevelValue = "1 2";
            string[] strValues = new string[iLevel];
            for (int i = 1; i <= iLevel; i++)
                strValues[i-1] = i.ToString();
            strLevelValue = string.Join(" ", strValues);

            return strLevelValue;
        }

        private string MakeID(int idx)
        {
            string strID = "A";
            string ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            ///////////////           1    1    2    2
            /////////////// 0    5    0    5    0    5
  
            int p = 0;
            int m = 0;

            p = (int)(idx / ALPHA.Length);
            m = (int)(idx % ALPHA.Length);

            if(p == 0)
                strID = string.Format("{0}", ALPHA[m]);
            else
                strID = string.Format("{0}{1}", ALPHA[p], ALPHA[m]);
                

            return strID;
        }

        private void DlgTaguchi_Load(object sender, EventArgs e)
        {
            UpdateFactorInfo();
        }

        private void cmbFactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                UpdateFactorInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            CreateDataTable();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CreateDataTable()
        {
            //실험순서
 
            //this.selectDesignTable(this.comboBox1.Text);
			
            //DACrux.Interface.FactorialDesign.Repeat = 1;
            //DACrux.Interface.FactorialDesign.ExcuteOrder = Interface.eExcuteOrder.Standard;

            m_TaguchiSheet = new DataTable();
            int iLevel = int.Parse(m_strLevel);
            int iRun = int.Parse(m_strRun.Replace("L",""));

            // 비정규 Table
            if (iLevel == 2 && iRun == 12)
            {
                int[,] strL12 = new int[,] {{1,1,1,1,1,1,1,1,1,1,1}
                                            ,{1,1,1,1,1,2,2,2,2,2,2}
                                            ,{1,1,2,2,2,1,1,1,2,2,2}
                                            ,{1,2,1,2,2,1,2,2,1,1,2}
                                            ,{1,2,2,1,2,2,1,2,1,2,1}
                                            ,{1,2,2,2,1,2,2,1,2,1,1}
                                            ,{2,1,2,2,1,1,2,2,1,2,1}
                                            ,{2,1,2,1,2,2,2,1,1,1,2}
                                            ,{2,1,1,2,2,2,1,2,2,1,1}
                                            ,{2,2,2,1,1,1,1,2,2,1,2}
                                            ,{2,2,1,2,1,2,1,1,1,2,2}
                                            ,{2,2,1,1,2,1,2,1,2,2,1}};

                for (int i = 0; i < m_FactorInfos.Length; i++)
                {
                    string str = string.Empty;

                    if (m_FactorInfos[i].Name != null)
                        str = m_FactorInfos[i].Name.Trim();

                    if (str.Equals("") || str.Equals(null))
                        str = "e_" + i;

                    m_TaguchiSheet.Columns.Add(str);
                }

                for (int i = 0; i < 12; i++)
                {
                    ArrayList _rows = new ArrayList();
                    for (int j = 0; j < m_FactorInfos.Length; j++)
                    {
                        string[] strValues = m_FactorInfos[j].LevelValue.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        _rows.Add(strValues[strL12[i, j]-1]);
                    }
                    m_TaguchiSheet.Rows.Add(_rows.ToArray());
                }
                return;
            }


            int[,] FactorModel = DACrux.BStats.FactorialDesign.GetDefinitionArray(iLevel, m_FactorInfos.Length, iRun);

			ArrayList array = new ArrayList();

			for(int i =0; i < m_FactorInfos.Length; i++)
				array.Add(m_FactorInfos[i].Name );

			

			for(int i=0; i< array.Count; i++)
			{
				for(int j=i+1; j< array.Count; j++)
					if(array[i].ToString().Trim() != ""  && array[i].Equals(array[j])  )					
					{						
						MessageBox.Show("디자인 옵션을 잘못 지정하셨습니다.");
						return;
					}
			}
 

			//실험순서
            //m_TaguchiSheet.Columns.Add("Index");

            for (int i = 0; i < m_FactorInfos.Length; i++)
			{
				string str = string.Empty;

				if(m_FactorInfos[i].Name != null )
					str = m_FactorInfos[i].Name.Trim();
				
				if(str.Equals("") || str.Equals(null) )
					str = "e_"+i;

                m_TaguchiSheet.Columns.Add(str);				
			}

			// 요인 설계 테이블을 생성한다.
            for (int i = 0; i < FactorModel.GetLength(0); i++)
            {
                ArrayList _rows = new ArrayList();

                for (int j = 0; j < FactorModel.GetLength(1); j++)
                {
                    if (j > 0)
                    {
                        string[] strValues = m_FactorInfos[j - 1].LevelValue.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        _rows.Add(strValues[FactorModel[i, j]]);
                    }
                    //else
                    //{
                    //    _rows.Add(FactorModel[i, j].ToString());
                    //}
                }

                m_TaguchiSheet.Rows.Add(_rows.ToArray());
            }

            //for (int i = 0; i < 50 - m_FactorInfos.Count; i++)
            //{
            //    m_TaguchiSheet.Columns.Add();
            //}
        }


        private void MakeDesignList()
        {
            try
            {
                m_dtDesignList = new DataTable();
                m_dtDesignList.Columns.Add("TABLE", typeof(string));
                m_dtDesignList.Columns.Add("DESIGN", typeof(string));
                m_dtDesignList.Columns.Add("LEVEL2", typeof(int));
                m_dtDesignList.Columns.Add("LEVEL3", typeof(int));
                m_dtDesignList.Columns.Add("LEVEL4", typeof(int));
                m_dtDesignList.Columns.Add("LEVEL5", typeof(int));
                //dtDesignList.Columns.Add("LEVELM", typeof(int));  // 사용안함

                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L4", 3, -1, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L8", 7, -1, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L9", -1, 4, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L12", 11, -1, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L16", 15, -1, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L16", -1, -1, 5, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L25", -1, -1, -1, 6 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L27", -1, 13, -1, -1 });
                m_dtDesignList.Rows.Add(new object[] { "단일-수준", "L32", 31, -1, -1, -1 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DACrux.BStats.Statistics.TaguchiAnalyusis2 TEST = new DACrux.BStats.Statistics.TaguchiAnalyusis2();

        }

    }

}
