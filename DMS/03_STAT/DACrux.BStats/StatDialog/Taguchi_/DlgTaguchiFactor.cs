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
    public partial class DlgTaguchiFactor : Form
    {
        FactorInfo[] m_FactorInfos = null;
        string[] m_strCloseEffect = null;
        string m_strRun = "L2";

        #region
        string[] CHECK_KEY = new string [] {
                                            "2_2_4_Y"
                                            ,"2_3_4_N"
                                            ,"2_2_8_Y"
                                            ,"2_3_8_Y"
                                            ,"2_4_8_Y"
                                            ,"2_5_8_Y"
                                            ,"2_6_8_Y"
                                            ,"2_7_8_Y"
                                            ,"2_2_12_N"
                                            ,"2_3_12_N"
                                            ,"2_2_12_N"
                                            ,"2_3_12_N"
                                            ,"2_4_12_N"
                                            ,"2_5_12_N"
                                            ,"2_6_12_N"
                                            ,"2_7_12_N"
                                            ,"2_8_12_N"
                                            ,"2_9_12_N"
                                            ,"2_10_12_N"
                                            ,"2_11_12_N"
                                            ,"2_2_16_Y"
                                            ,"2_3_16_Y"
                                            ,"2_2_16_Y"
                                            ,"2_3_16_Y"
                                            ,"2_4_16_Y"
                                            ,"2_5_16_Y"
                                            ,"2_6_16_Y"
                                            ,"2_7_16_Y"
                                            ,"2_8_16_Y"
                                            ,"2_9_16_Y"
                                            ,"2_10_16_Y"
                                            ,"2_11_16_Y"
                                            ,"2_12_16_Y"
                                            ,"2_13_16_Y"
                                            ,"2_14_16_Y"
                                            ,"2_15_16_Y"
                                            ,"2_2_32_Y"
                                            ,"2_3_32_Y"
                                            ,"2_2_32_Y"
                                            ,"2_3_32_Y"
                                            ,"2_4_32_Y"
                                            ,"2_5_32_Y"
                                            ,"2_6_32_Y"
                                            ,"2_7_32_Y"
                                            ,"2_8_32_Y"
                                            ,"2_9_32_Y"
                                            ,"2_10_32_Y"
                                            ,"2_11_32_Y"
                                            ,"2_12_32_Y"
                                            ,"2_13_32_Y"
                                            ,"2_14_32_Y"
                                            ,"2_15_32_Y"
                                            ,"2_16_32_Y"
                                            ,"2_17_32_Y"
                                            ,"2_18_32_Y"
                                            ,"2_19_32_Y"
                                            ,"2_20_32_Y"
                                            ,"2_21_32_Y"
                                            ,"2_22_32_Y"
                                            ,"2_23_32_Y"
                                            ,"2_24_32_Y"
                                            ,"2_25_32_Y"
                                            ,"2_26_32_Y"
                                            ,"2_27_32_Y"
                                            ,"2_28_32_Y"
                                            ,"2_29_32_Y"
                                            ,"2_30_32_Y"
                                            ,"2_31_32_Y"
                                            ,"3_2_9_Y"
                                            ,"3_3_9_Y"
                                            ,"3_4_9_Y"
                                            ,"3_2_27_Y"
                                            ,"3_3_27_Y"
                                            ,"3_4_27_Y"
                                            ,"3_5_27_Y"
                                            ,"3_6_27_Y"
                                            ,"3_7_27_Y"
                                            ,"3_8_27_Y"
                                            ,"3_9_27_Y"
                                            ,"3_10_27_Y"
                                            ,"3_11_27_Y"
                                            ,"3_12_27_Y"
                                            ,"3_13_27_Y"
                                            ,"4_2_16_Y"
                                            ,"4_3_16_Y"
                                            ,"4_4_16_Y"
                                            ,"4_5_16_Y"
                                            ,"5_2_25_Y"
                                            ,"5_3_25_Y"
                                            ,"5_4_25_Y"
                                            ,"5_5_25_Y"
                                            ,"5_6_27_Y"
                                            };

        #endregion

        public DlgTaguchiFactor()
        {
            InitializeComponent();
        }

        public string[] CloseEffect
        {
            get
            {
                return m_strCloseEffect;
            }
        }

        public void SetFactorInfo(FactorInfo[] FactorInfos, string Run)
        {
            m_FactorInfos = FactorInfos;
            m_strRun = Run;
            ViewLiist();
        }

        private void ViewLiist()
        {
            string[] strColumnIDs = null;
            try
            {
                fpSpread1.ActiveSheet.Rows.Clear();
                if(m_FactorInfos == null || m_FactorInfos.Length == 0) return;

                fpSpread1.ActiveSheet.Rows.Count = m_FactorInfos.Length;
                strColumnIDs = new string[m_FactorInfos.Length];
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbCellType = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

                for (int i = 0; i < m_FactorInfos.Length; i++)
                {
                    fpSpread1.ActiveSheet.Cells[i, 0].Value = m_FactorInfos[i].Name;
                    fpSpread1.ActiveSheet.Cells[i, 1].Value = m_FactorInfos[i].LevelValue;
                    fpSpread1.ActiveSheet.Cells[i, 2].Value = m_FactorInfos[i].Column;
                    fpSpread1.ActiveSheet.Cells[i, 3].Value = m_FactorInfos[i].Level;
                    strColumnIDs[i] = m_FactorInfos[i].Column.ToString();
                }
                cmbCellType.Items = strColumnIDs;
                fpSpread1.ActiveSheet.Columns[2].CellType = cmbCellType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void butCloseEffect_Click(object sender, EventArgs e)
        {
            try
            {
                int iRun = int.Parse(m_strRun.Replace("L", ""));
                int iLev = m_FactorInfos[0].Level;
                int iFCnt = m_FactorInfos.Length;

                
                /// Check 규칙을 알수 없음.. Check Array는 나중에 Update
                /// LEVEL	PARA	RUN	
                string strKEY = string.Format("{0}_{1}_{2}_Y", iLev,iFCnt,iRun);

                if (Array.IndexOf(CHECK_KEY,strKEY) < 0)
                {
                    MessageBox.Show("선택된 배열로 어떤 상호작용도 추정할 수 없습니다.");
                    return;
                }

                DlgCloseEffect oCloseEff = new DlgCloseEffect();
                oCloseEff.SetFactorList(m_FactorInfos, m_strCloseEffect);
                if (oCloseEff.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_strCloseEffect = oCloseEff.CloseEffect;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rdoFactorCombine_CheckedChanged(object sender, EventArgs e)
        {
            butCloseEffect.Enabled = rdoFactorCombine.Checked;
            if (rdoFactorCombine.Checked) m_strCloseEffect = null;
        }


        private void fpSpread1_EditModeOff(object sender, EventArgs e)
        {
            try
            {
                int iRow = fpSpread1.ActiveSheet.ActiveRowIndex;
                int iCol = fpSpread1.ActiveSheet.ActiveColumnIndex;

                switch(iCol)
                {
                    case 0:
                        m_FactorInfos[iRow].Name = fpSpread1.ActiveSheet.Cells[iRow, iCol].Value.ToString();
                        break;
                    case 1:
                        string[] tmpValues = fpSpread1.ActiveSheet.Cells[iRow, iCol].Value.ToString().Split(' ');
                        if (tmpValues.Length != (int)fpSpread1.ActiveSheet.Cells[iRow, 3].Value)
                        {
                            MessageBox.Show("요인 수준 값의 수는 요인 수준의 수와 일치해야 합니다.");
                            fpSpread1.ActiveSheet.Cells[iRow, iCol].Value = m_FactorInfos[iRow].LevelValue;
                        }
                        m_FactorInfos[iRow].LevelValue = fpSpread1.ActiveSheet.Cells[iRow, iCol].Value.ToString();
                        break;
                    case 2:
                        m_FactorInfos[iRow].Column = int.Parse(fpSpread1.ActiveSheet.Cells[iRow, iCol].Value.ToString());
                        break;
                    case 3: // Read Only
                        //m_FactorInfos[iRow].Level = (int)fpSpread1.ActiveSheet.Cells[iRow, iCol].Value;
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }
}
