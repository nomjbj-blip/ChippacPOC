using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.BStats.StatDialog.DlgTaguchi_
{
    public partial class DlgCloseEffect : Form
    {
        FactorInfo[] m_FactorInfos = null;
        List<string> m_strListAll = null;

        public DlgCloseEffect()
        {
            InitializeComponent();
        }

        public string[] CloseEffect
        {
            get
            {
                if (lstSelCol.Items.Count == 0) return null;
                string[] tmpList = new string[lstSelCol.Items.Count];
                lstSelCol.Items.CopyTo(tmpList, 0);
                return tmpList;
            }
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            if (lstSelCol.Items.Count >= m_FactorInfos.Length)
            {
                MessageBox.Show(string.Format("너무 많은 상호작용을 선택했습니다. 최대 {0}개를 선택하십시오", m_FactorInfos.Length - 1));
                return;
            }
        }

        internal void SetFactorList(FactorInfo[] FactorInfos, string[] SelectedList = null)
        {
            m_FactorInfos = FactorInfos;
            try
            {
                lstFactor.Items.Clear();
                lstColList.Items.Clear();
                lstSelCol.Items.Clear();
                m_strListAll = new List<string>();
                for (int i = 0; i < m_FactorInfos.Length; i++)
                {
                    lstFactor.Items.Add(string.Format("{0}:{1}", m_FactorInfos[i].ID, m_FactorInfos[i].Name));
                    for (int j = 0; j < m_FactorInfos.Length; j++)
                    {
                        if (m_FactorInfos[i].ID == m_FactorInfos[j].ID) continue;
                        string tmpStr = string.Format("{0}{1}", m_FactorInfos[i].ID, m_FactorInfos[j].ID);
                        string tmpStr2 = string.Format("{0}{1}", m_FactorInfos[j].ID, m_FactorInfos[i].ID);

                        //전체 List
                        if (m_strListAll.IndexOf(tmpStr) < 0 && m_strListAll.IndexOf(tmpStr2) < 0)
                        {
                            m_strListAll.Add(tmpStr);

                            if (lstColList.Items.IndexOf(tmpStr) < 0)
                            {
                                //선택되지 않은 List
                                lstColList.Items.Add(tmpStr);
                            }
                            else
                            {
                                //선택된 List
                                lstSelCol.Items.Add(tmpStr);
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

        private void butSel_Click(object sender, EventArgs e)
        {
            if (lstColList.SelectedIndex < 0) return;
            lstSelCol.Items.Add(lstColList.Text);
            lstColList.Items.RemoveAt(lstColList.SelectedIndex);
        }

        private void butDeSel_Click(object sender, EventArgs e)
        {
            if (lstSelCol.SelectedIndex < 0) return;
            lstColList.Items.Add(lstSelCol.Text);
            lstSelCol.Items.RemoveAt(lstSelCol.SelectedIndex);
        }

        private void butSelAll_Click(object sender, EventArgs e)
        {
            lstColList.Items.Clear();
            lstSelCol.Items.Clear();

            lstSelCol.Items.AddRange(m_strListAll.ToArray());
        }

        private void butDeSelAll_Click(object sender, EventArgs e)
        {
            lstColList.Items.Clear();
            lstSelCol.Items.Clear();

            lstColList.Items.AddRange(m_strListAll.ToArray());
        }

        private void lstColList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            butSel_Click(null, null);
        }

        private void lstSelCol_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            butDeSel_Click(null, null);
        }
    }
}
