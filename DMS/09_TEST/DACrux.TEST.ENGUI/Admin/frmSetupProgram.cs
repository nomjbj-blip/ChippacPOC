using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DACrux.TEST.RO;

namespace DACrux.TEST.ENGUI
{
    public partial class frmSetupProgram : DACrux.Framework.Base.DACruxUXBasic01
    {
        private DataSet m_DS = null;
        private string[] m_strNotUseFieldName = new string[] { "WAFER_SEQ", "DIEID", "X", "Y", "BIN", "HBIN", "CHARBIN", "SITE", "VISUALINSP", "AVI" };

        public frmSetupProgram()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            if (m_DS != null) m_DS.Dispose();
            m_DS = null;
            txtProgram.Text = "";
            txtVersion.Text = "";
            cmbCopyFromProgram.Text = "";

            m_DS = new DataSet();
            m_DS.Tables.Add("BIN");
            m_DS.Tables.Add("ITEM");

            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Soft Bin", System.Type.GetType("System.Int32")));
            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Hard Bin", System.Type.GetType("System.Int32")));
            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Bin Desc", System.Type.GetType("System.String")));
            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Map Symbol", System.Type.GetType("System.String")));
            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Is Gec", System.Type.GetType("System.String")));
            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Is Display", System.Type.GetType("System.String")));
            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Upper Limit", System.Type.GetType("System.Int32")));
            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Lower Limit", System.Type.GetType("System.Int32")));
            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Color", System.Type.GetType("System.String")));
            m_DS.Tables["BIN"].Columns.Add(new DataColumn("Description", System.Type.GetType("System.String")));

            m_DS.Tables["ITEM"].Columns.Add(new DataColumn("Item", System.Type.GetType("System.String")));
            m_DS.Tables["ITEM"].Columns.Add(new DataColumn("Description", System.Type.GetType("System.String")));
            m_DS.Tables["ITEM"].Columns.Add(new DataColumn("Lower Limit", System.Type.GetType("System.Double")));
            m_DS.Tables["ITEM"].Columns.Add(new DataColumn("Upper Limit", System.Type.GetType("System.Double")));
            m_DS.Tables["ITEM"].Columns.Add(new DataColumn("Lower Ctl Limit", System.Type.GetType("System.Double")));
            m_DS.Tables["ITEM"].Columns.Add(new DataColumn("Upper Ctl Limit", System.Type.GetType("System.Double")));
            m_DS.Tables["ITEM"].Columns.Add(new DataColumn("Unit", System.Type.GetType("System.String")));
            m_DS.Tables["ITEM"].Columns.Add(new DataColumn("Direction", System.Type.GetType("System.String")));
            m_DS.Tables["ITEM"].Columns.Add(new DataColumn("COMMENTS", System.Type.GetType("System.String")));


            fpBin_Sheet1.DataSource = m_DS.Tables["BIN"];
            fpItem_Sheet1.DataSource = m_DS.Tables["ITEM"];

            SetColumnStyle();

            FillDeviceList();
            FillArea();
            FillCopyProgram(
                string.Empty
                );
        }

        private void SetColumnStyle()
        {
            FarPoint.Win.Spread.CellType.NumberCellType numCell_INT = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType chkBoxCell_GEC = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType chkBoxCell_DISP = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType chkBoxCell_DIR = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType cmbBoxCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            cmbBoxCell.Items = new string[] { " ", "㎀", "㎁", "㎂", "㎃", "A", "㎴", "㎵", "㎷", "V", "㎱", "㎲", "㎳", "s", "Ω", "㏁", "㎐", "㎑", "㎒", "㎓", "㎊", "㎋", "㎌" };

            chkBoxCell_GEC.TextTrue = "Good Bin";
            chkBoxCell_GEC.TextFalse = "Fail Bin";
            chkBoxCell_DISP.TextTrue = "Enable";
            chkBoxCell_DISP.TextFalse = "Disable";
            chkBoxCell_DIR.TextTrue = "Out-Side";
            chkBoxCell_DIR.TextFalse = "In-Side";

            numCell_INT.DecimalPlaces = 0;

            fpBin_Sheet1.Columns[1].CellType = numCell_INT;
            fpBin_Sheet1.Columns[2].CellType = numCell_INT;
            fpBin_Sheet1.Columns[5].CellType = chkBoxCell_GEC;
            fpBin_Sheet1.Columns[6].CellType = chkBoxCell_DISP;
            fpBin_Sheet1.Columns[7].CellType = numCell_INT;
            fpBin_Sheet1.Columns[8].CellType = numCell_INT;

            fpBin_Sheet1.Columns[9].Locked = true;


            fpItem_Sheet1.Columns[7].CellType = cmbBoxCell;
            fpItem_Sheet1.Columns[8].CellType = chkBoxCell_DIR;


            ProbeAdmin oColor = new ProbeAdmin();

            for (int i = 0; i < fpBin_Sheet1.Rows.Count; i++)
            {
                if (fpBin_Sheet1.Cells[i, 9].Text == "")
                {
                    fpBin_Sheet1.Cells[i, 9].Text = oColor.ColorString(DACrux.Base.Convert.intParse(fpBin_Sheet1.Cells[i, 0].Text));
                }

                fpBin_Sheet1.Cells[i, 9].BackColor = ColorTranslator.FromHtml(fpBin_Sheet1.Cells[i, 9].Text.ToString());
            }

            chkBoxCell_GEC = null;
            chkBoxCell_DISP = null;
            chkBoxCell_DIR = null;
            cmbBoxCell = null;
        }

        private void butBinInsert_Click(object sender, System.EventArgs e)
        {
            ProbeAdmin oColor = new ProbeAdmin();
            DataRow[] drs = m_DS.Tables["BIN"].Select("", "Soft Bin DESC");
            int iMaxSBin = -1;
            int iMaxHBin = -1;
            string strColor = "255255255";
            if (drs.Length > 0)
            {
                iMaxSBin = DACrux.Base.Convert.intParse(drs[0]["Soft Bin"].ToString());
                iMaxHBin = DACrux.Base.Convert.intParse(drs[0]["Hard Bin"].ToString());
            }

            m_DS.Tables["BIN"].Rows.Add(m_DS.Tables["BIN"].NewRow());
            m_DS.Tables["BIN"].AcceptChanges();

            if (fpBin_Sheet1.Rows.Count == 1)
            {
                strColor = oColor.ColorString(1);
                fpBin_Sheet1.SetValue(0, 0, cmbArea.Text);
                fpBin_Sheet1.SetValue(0, 1, 1);
                fpBin_Sheet1.SetValue(0, 2, 1);
                fpBin_Sheet1.SetValue(0, 3, "GEC");
                fpBin_Sheet1.SetValue(0, 4, ".");
                fpBin_Sheet1.SetValue(0, 5, true);
                fpBin_Sheet1.SetValue(0, 6, true);
                fpBin_Sheet1.SetValue(0, 7, -1);
                fpBin_Sheet1.SetValue(0, 8, 10);
                fpBin_Sheet1.SetValue(0, 9, strColor);
                fpBin_Sheet1.SetValue(0, 10, "Good Electrical Chip");

                fpBin_Sheet1.Cells[0, 9].BackColor = ColorTranslator.FromHtml(strColor);
            }
            else
            {
                strColor = oColor.ColorString(iMaxSBin + 1);

                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 0, cmbArea.Text);
                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 1, iMaxSBin + 1);
                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 2, iMaxHBin + 1);
                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 3, string.Format("BIN{0:0#}", iMaxSBin + 1));
                //fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count-1,3,".");
                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 5, false);
                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 6, true);
                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 7, 10);
                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 8, 0);
                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 9, strColor);
                fpBin_Sheet1.SetValue(fpBin_Sheet1.Rows.Count - 1, 10, "Fail Chip");

                fpBin_Sheet1.Cells[fpBin_Sheet1.Rows.Count - 1, 9].BackColor = ColorTranslator.FromHtml(strColor);
            }
            SetColumnStyle();
        }

        private void butBinDelete_Click(object sender, System.EventArgs e)
        {
            if (fpBin_Sheet1.ActiveRowIndex < 0 || fpBin_Sheet1.Rows.Count == 0) return;
            m_DS.Tables["BIN"].Rows.RemoveAt(fpBin_Sheet1.ActiveRowIndex);
            m_DS.Tables["BIN"].AcceptChanges();
            SetColumnStyle();
        }

        private void butBinUp_Click(object sender, System.EventArgs e)
        {
            int iSel = -1;
            try
            {
                iSel = fpBin_Sheet1.ActiveRowIndex;
                if (iSel == 0) return;
            }
            catch
            {
                return;
            }

            DataRow dr = m_DS.Tables["BIN"].NewRow();
            dr.ItemArray = m_DS.Tables["BIN"].Rows[iSel].ItemArray;

            m_DS.Tables["BIN"].Rows.RemoveAt(iSel);
            m_DS.Tables["BIN"].AcceptChanges();

            m_DS.Tables["BIN"].Rows.InsertAt(dr, iSel - 1);
            m_DS.Tables["BIN"].AcceptChanges();

            fpBin_Sheet1.ActiveRowIndex = iSel - 1;
            SetColumnStyle();

        }

        private void butBinDown_Click(object sender, System.EventArgs e)
        {
            int iSel = -1;
            try
            {
                iSel = fpBin_Sheet1.ActiveRowIndex;
                if (iSel == fpBin_Sheet1.Rows.Count - 1) return;
            }
            catch
            {
                return;
            }

            DataRow dr = m_DS.Tables["BIN"].NewRow();
            dr.ItemArray = m_DS.Tables["BIN"].Rows[iSel].ItemArray;

            m_DS.Tables["BIN"].Rows.RemoveAt(iSel);
            m_DS.Tables["BIN"].AcceptChanges();

            m_DS.Tables["BIN"].Rows.InsertAt(dr, iSel + 1);
            m_DS.Tables["BIN"].AcceptChanges();

            fpBin_Sheet1.ActiveRowIndex = iSel + 1;
            SetColumnStyle();

        }

        private void butItemInsert_Click(object sender, System.EventArgs e)
        {
            m_DS.Tables["ITEM"].Rows.Add(m_DS.Tables["ITEM"].NewRow());
            m_DS.Tables["ITEM"].AcceptChanges();
            SetColumnStyle();
        }

        private void butDelete_Click(object sender, System.EventArgs e)
        {
            if (fpItem_Sheet1.ActiveRowIndex < 0 || fpItem_Sheet1.Rows.Count == 0) return;
            m_DS.Tables["ITEM"].Rows.RemoveAt(fpItem_Sheet1.ActiveRowIndex);
            m_DS.Tables["ITEM"].AcceptChanges();
            SetColumnStyle();
        }

        private void butUp_Click(object sender, System.EventArgs e)
        {
            int iSel = -1;
            try
            {
                iSel = fpItem_Sheet1.ActiveRowIndex;
                if (iSel == 0) return;
            }
            catch
            {
                return;
            }

            DataRow dr = m_DS.Tables["ITEM"].NewRow();
            dr.ItemArray = m_DS.Tables["ITEM"].Rows[iSel].ItemArray;

            m_DS.Tables["ITEM"].Rows.RemoveAt(iSel);
            m_DS.Tables["ITEM"].AcceptChanges();

            m_DS.Tables["ITEM"].Rows.InsertAt(dr, iSel - 1);
            m_DS.Tables["ITEM"].AcceptChanges();

            fpItem_Sheet1.ActiveRowIndex = iSel - 1;
            SetColumnStyle();
        }

        private void butDown_Click(object sender, System.EventArgs e)
        {
            int iSel = -1;
            try
            {
                iSel = fpItem_Sheet1.ActiveRowIndex;
                if (iSel == fpItem_Sheet1.Rows.Count - 1) return;
            }
            catch
            {
                return;
            }

            DataRow dr = m_DS.Tables["ITEM"].NewRow();
            dr.ItemArray = m_DS.Tables["ITEM"].Rows[iSel].ItemArray;

            m_DS.Tables["ITEM"].Rows.RemoveAt(iSel);
            m_DS.Tables["ITEM"].AcceptChanges();

            m_DS.Tables["ITEM"].Rows.InsertAt(dr, iSel + 1);
            m_DS.Tables["ITEM"].AcceptChanges();

            fpItem_Sheet1.ActiveRowIndex = iSel + 1;
            SetColumnStyle();
        }

        private void fpBin_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (fpBin_Sheet1.ColumnHeader.Columns[e.Column].Label.Equals("Color"))
            {
                colorDialog1.FullOpen = true;
                colorDialog1.AllowFullOpen = true;
                colorDialog1.Color = fpBin_Sheet1.Cells[e.Row, e.Column].BackColor;
                if (colorDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    fpBin_Sheet1.Cells[e.Row, e.Column].BackColor = colorDialog1.Color;
                    fpBin_Sheet1.Cells[e.Row, e.Column].Value = string.Format("{0:00#}{1:00#}{2:00#}", colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
                }
            }
        }

        private void FillDeviceList()
        {
            ProbeAdmin oDevDef = null;
            DataTable dt = null;
            try
            {
                oDevDef = new ProbeAdmin();
                dt = oDevDef.GetProductList();
                cmbDevice.DataSource = dt;
                cmbDevice.DisplayMember = "PRODUCT";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDevDef = null;
            }
        }

        private void FillArea()
        {
            ProbeAdmin oTPDef = null;
            DataTable dt = null;
            try
            {
                oTPDef = new ProbeAdmin();
                dt = oTPDef.GetTestAreaList();
                cmbArea.DataSource = dt;
                cmbArea.DisplayMember = "TESTAREA";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oTPDef = null;
            }
        }

        private void FillCopyProgram(
            string program
            )
        {
            ProbeAdmin oTPDef = null;
            DataTable dt = null;
            try
            {
                oTPDef = new ProbeAdmin();
                dt = oTPDef.GetTestProgramList(
                    DACrux.Base.GlobalVariable.Factory,
                    program
                    );
                DataRow drEnpty = dt.NewRow();
                dt.Rows.InsertAt(drEnpty, 0);
                cmbCopyFromProgram.DataSource = dt;
                cmbCopyFromProgram.DisplayMember = "PROGRAM";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oTPDef = null;
            }
        }

        private void butSave_Click(object sender, System.EventArgs e)
        {
            if (cmbArea.Text.Length == 0)
            {
                MessageBox.Show("Aere를 선택하거나 새로 입력해 주시기 바랍니다");
                cmbArea.Focus();
                return;
            }

            if (cmbDevice.Text.Length == 0)
            {
                MessageBox.Show("Device를 선택하여 주시기 바랍니다. \nDevice가 없을때는 Device를 먼저 Define해야 할 필요가 있습니다.");
                cmbDevice.Focus();
                return;
            }

            if (txtProgram.Text.Trim().Length == 0)
            {
                MessageBox.Show("Test Program Name은 반드시 설정하여 주시기 바랍니다.");
                txtProgram.Focus();
                return;
            }

            if (txtProgram.Text.IndexOf(" ") > -1)
            {
                MessageBox.Show("Test Program Name은 Space가 들어갈 수 없습니다.");
                txtProgram.Focus();
                return;
            }

            ProbeAdmin oTPDef = null;
            ProbeAdmin oBinDef = null;
            ProbeAdmin oPSpecDef = null;

            string[,] strBinInfo = null;
            string[,] strParaInfo = null;
            string strParaField = "";
            try
            {
                /// Check Program
                /// 


                /// Create Program
                /// 
                oTPDef = new ProbeAdmin();
                DataTable dt = oTPDef.GetProgramInfo(txtProgram.Text.ToUpper());
                DataRow[] drs = dt.Select("USE_FLAG='T'");
                if (drs.Length > 0)
                {
                    MessageBox.Show(string.Format("Program[{0}]은 이미 등록된 이름입니다.", txtProgram.Text));
                    return;
                }

                /// Create Para
                /// 

                m_DS.Tables["ITEM"].AcceptChanges();
                DataRow[] drsItem = m_DS.Tables["ITEM"].Select();
                if (drsItem.Length > 0)
                {
                    oPSpecDef = new ProbeAdmin();
                    strParaInfo = new string[drsItem.Length, 13];
                    for (int i = 0; i < drsItem.Length; i++)
                    {
                        strParaInfo[i, 0] = txtProgram.Text;
                        strParaInfo[i, 1] = cmbDevice.Text;
                        strParaInfo[i, 2] = cmbArea.Text;
                        strParaInfo[i, 3] = string.Format("{0}", i + 1);
                        strParaInfo[i, 4] = drsItem[i]["Item"].ToString();
                        strParaInfo[i, 5] = drsItem[i]["Description"].ToString();
                        strParaInfo[i, 6] = drsItem[i]["Lower Limit"].ToString();
                        strParaInfo[i, 7] = drsItem[i]["Upper Limit"].ToString();
                        strParaInfo[i, 8] = drsItem[i]["Lower Ctl Limit"].ToString();
                        strParaInfo[i, 9] = drsItem[i]["Upper Ctl Limit"].ToString();
                        strParaInfo[i, 10] = drsItem[i]["Unit"].ToString();
                        strParaInfo[i, 11] = drsItem[i]["Direction"].ToString();
                        strParaInfo[i, 12] = drsItem[i]["COMMENTS"].ToString();

                        strParaField = string.Format("{0},{1} Number", strParaField, drsItem[i]["Item"].ToString().Replace(" ", "_"));
                    }
                    oPSpecDef.CreateParaSpec(strParaInfo);
                }

                /// Create Bin
                /// 

                m_DS.Tables["BIN"].AcceptChanges();
                DataRow[] drsbin = m_DS.Tables["BIN"].Select();
                if (drsbin.Length > 0)
                {
                    oBinDef = new ProbeAdmin();
                    strBinInfo = new string[drsbin.Length, 13];
                    for (int i = 0; i < drsbin.Length; i++)
                    {
                        strBinInfo[i, 0] = txtProgram.Text;
                        strBinInfo[i, 1] = cmbDevice.Text;
                        strBinInfo[i, 2] = cmbArea.Text;
                        strBinInfo[i, 3] = drsbin[i]["Soft Bin"].ToString();
                        strBinInfo[i, 4] = drsbin[i]["Hard Bin"].ToString();
                        strBinInfo[i, 5] = drsbin[i]["Bin Desc"].ToString();
                        strBinInfo[i, 6] = drsbin[i]["Map Symbol"].ToString();
                        strBinInfo[i, 7] = drsbin[i]["Is Gec"].ToString();
                        strBinInfo[i, 8] = drsbin[i]["Is Display"].ToString();
                        strBinInfo[i, 9] = drsbin[i]["Upper Limit"].ToString();
                        strBinInfo[i, 10] = drsbin[i]["Lower Limit"].ToString();
                        strBinInfo[i, 11] = drsbin[i]["Color"].ToString();
                        strBinInfo[i, 12] = drsbin[i]["Description"].ToString();
                    }

                    oBinDef.CreateBins(strBinInfo);
                }

                try
                {
                    oTPDef.CreateProgram(cmbDevice.Text.ToUpper(), txtProgram.Text.ToUpper(), cmbArea.Text.ToUpper(), (double)numTagetYield.Value, txtVersion.Text, strParaField, txtSiteCount.Text);
                }
                catch
                {
                }

                SetColumnStyle();
                MessageBox.Show(string.Format("Program[{0}]을 새로 등록하였습니다.", txtProgram.Text));
                FillArea();
                FillCopyProgram(
                    txtProgram.Text
                    );

            }
            catch (Exception ex)
            {
                /// Rollback
                /// 
                if (oTPDef != null) oTPDef.DeleteProgramInfo(txtProgram.Text);
                if (oBinDef != null) oBinDef.DeleteProgramBin(txtProgram.Text);
                if (oPSpecDef != null) oPSpecDef.DeleteProgramParaSpec(txtProgram.Text);
                MessageBox.Show(string.Format("Program을 등록 할 수 없습니다.[err:{0}]", ex.Message));
            }
            finally
            {
                oTPDef = null;
                oBinDef = null;
                oPSpecDef = null;
                strBinInfo = null;
                strParaInfo = null;
            }
        }

        private void butCopyFrom_Click(object sender, System.EventArgs e)
        {
            if (cmbCopyFromProgram.Text.Length == 0) return;
            ProbeAdmin oTPDef = null;
            ProbeAdmin oBinDef = null;
            ProbeAdmin oPSpecDef = null;
            DataTable dt = null;
            try
            {
                oTPDef = new ProbeAdmin();
                oBinDef = new ProbeAdmin();
                oPSpecDef = new ProbeAdmin();

                dt = oTPDef.GetProgramInfo(cmbCopyFromProgram.Text);

                cmbArea.Text = dt.Rows[0]["TESTAREA"].ToString();
                cmbDevice.Text = dt.Rows[0]["PRODUCT"].ToString();
                txtProgram.Text = "Copy From " + dt.Rows[0]["PROGRAM"].ToString();
                txtVersion.Text = dt.Rows[0]["VERSION"].ToString();
                if (cmbArea.Text.Equals("PARAMETRIC"))
                {
                    txtSiteCount.Text = string.Format("{0}", dt.Rows[0]["SITE_CNT"]);
                }

                double dTarget = 90.0;
                if (double.TryParse(dt.Rows[0]["TARGET_YIELD"].ToString(), out  dTarget)) numTagetYield.Value = (Decimal)DACrux.Base.Convert.doubleParse(dt.Rows[0]["TARGET_YIELD"].ToString());
                else numTagetYield.Value = numTagetYield.Value;

                /// BIN
                /// ///////////////////////////////////////////////////////////////////////////////
                dt = oBinDef.GetBinListEditable(cmbCopyFromProgram.Text);
                dt.TableName = "BIN";
                m_DS.Tables.Remove("BIN");
                m_DS.Tables.Add(dt.Copy());

                fpBin_Sheet1.DataSource = m_DS.Tables["BIN"];
                ////////////////////////////////////////////////////////////////////////////////////

                /// ITEM
                /// ///////////////////////////////////////////////////////////////////////////////
                dt = oPSpecDef.GetParaSpecListEditable(
                    DACrux.Base.GlobalVariable.Factory,
                    cmbCopyFromProgram.Text
                    );
                dt.TableName = "ITEM";
                m_DS.Tables.Remove("ITEM");
                m_DS.Tables.Add(dt.Copy());

                fpItem_Sheet1.DataSource = m_DS.Tables["ITEM"];
                ////////////////////////////////////////////////////////////////////////////////////

                SetColumnStyle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Update를 할 수 없습니다.[Err:{0}]", ex.Message));
            }
            finally
            {
                oTPDef = null;
                oBinDef = null;
                oPSpecDef = null;
            }

        }

        private void butEditProgramBin_Click(object sender, System.EventArgs e)
        {
            if (cmbArea.Text.Length == 0)
            {
                MessageBox.Show("Aere를 선택하거나 새로 입력해 주시기 바랍니다");
                cmbArea.Focus();
                return;
            }

            if (cmbDevice.Text.Length == 0)
            {
                MessageBox.Show("Device를 선택하여 주시기 바랍니다. \nDevice가 없을때는 Device를 먼저 Define해야 할 필요가 있습니다.");
                cmbDevice.Focus();
                return;
            }


            ProbeAdmin oTPDef = null;
            ProbeAdmin oBinDef = null;
            ProbeAdmin oPSpecDef = null;

            string[,] strBinInfo = null;
            string[,] strParaInfo = null;
            try
            {
                /// Create Program
                /// 
                oTPDef = new ProbeAdmin();
                oTPDef.UpdateProgramInfo(txtProgram.Text.ToUpper(), cmbArea.Text.ToUpper(), cmbDevice.Text.ToUpper(), (double)numTagetYield.Value, txtVersion.Text);

                /// Create Bin
                /// 
                oBinDef = new ProbeAdmin();

                m_DS.Tables["BIN"].AcceptChanges();
                DataRow[] drsbin = m_DS.Tables["BIN"].Select();

                strBinInfo = new string[drsbin.Length, 13];
                for (int i = 0; i < drsbin.Length; i++)
                {
                    strBinInfo[i, 0] = cmbCopyFromProgram.Text;
                    strBinInfo[i, 1] = cmbDevice.Text;
                    strBinInfo[i, 2] = cmbArea.Text;
                    strBinInfo[i, 3] = drsbin[i]["Soft Bin"].ToString();
                    strBinInfo[i, 4] = drsbin[i]["Hard Bin"].ToString();
                    strBinInfo[i, 5] = drsbin[i]["Bin Desc"].ToString();
                    strBinInfo[i, 6] = drsbin[i]["Map Symbol"].ToString();
                    strBinInfo[i, 7] = drsbin[i]["Is Gec"].ToString();
                    strBinInfo[i, 8] = drsbin[i]["Is Display"].ToString();
                    strBinInfo[i, 9] = drsbin[i]["Upper Limit"].ToString();
                    strBinInfo[i, 10] = drsbin[i]["Lower Limit"].ToString();
                    strBinInfo[i, 11] = drsbin[i]["Color"].ToString();
                    strBinInfo[i, 12] = drsbin[i]["Description"].ToString();
                }
                oBinDef.UpdateProgramBin(cmbCopyFromProgram.Text, strBinInfo);

                /// Create Para
                /// 

                m_DS.Tables["ITEM"].AcceptChanges();
                DataRow[] drsItem = m_DS.Tables["ITEM"].Select();

                oPSpecDef = new ProbeAdmin();
                strParaInfo = new string[drsItem.Length, 13];
                for (int i = 0; i < drsItem.Length; i++)
                {
                    strParaInfo[i, 0] = cmbCopyFromProgram.Text;
                    strParaInfo[i, 1] = cmbDevice.Text;
                    strParaInfo[i, 2] = cmbArea.Text;
                    strParaInfo[i, 3] = string.Format("{0}", i + 1);
                    strParaInfo[i, 4] = drsItem[i]["Item"].ToString();
                    strParaInfo[i, 5] = drsItem[i]["Description"].ToString();
                    strParaInfo[i, 6] = drsItem[i]["Lower Limit"].ToString();
                    strParaInfo[i, 7] = drsItem[i]["Upper Limit"].ToString();
                    strParaInfo[i, 8] = drsItem[i]["Lower Ctl Limit"].ToString();
                    strParaInfo[i, 9] = drsItem[i]["Upper Ctl Limit"].ToString();
                    strParaInfo[i, 10] = drsItem[i]["Unit"].ToString();
                    strParaInfo[i, 11] = (drsItem[i]["Direction"].ToString().Length == 0) ? "0" : drsItem[i]["Direction"].ToString();
                    strParaInfo[i, 12] = drsItem[i]["COMMENTS"].ToString();
                }
                oPSpecDef.UpdateProgramParaSpec(cmbCopyFromProgram.Text.ToUpper(), strParaInfo);

                SetColumnStyle();
                MessageBox.Show(string.Format("Program[{0}]을 수정 하였습니다.", cmbCopyFromProgram.Text));

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Program을 Update 할 수 없습니다.[err:{0}]", ex.Message));
            }
            finally
            {
                oTPDef = null;
                oBinDef = null;
                strBinInfo = null;
            }
        }

        private void butDeleteProgramBin_Click(object sender, System.EventArgs e)
        {
            ProbeAdmin oTPDef = null;
            try
            {
                if (MessageBox.Show(string.Format("Program[{0}]을 삭제 하시겠습니까?", cmbCopyFromProgram.Text), "삭제확인", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    oTPDef = new ProbeAdmin();
                    oTPDef.UpdateUseFlag(cmbCopyFromProgram.Text, "F");
                    MessageBox.Show(string.Format("Program[{0}]을 삭제 하였습니다.", cmbCopyFromProgram.Text));
                    m_DS.Tables["BIN"].Clear();
                    m_DS.Tables["BIN"].AcceptChanges();
                    m_DS.Tables["ITEM"].Clear();
                    m_DS.Tables["ITEM"].AcceptChanges();
                    FillCopyProgram(
                        String.Empty
                        );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Program을 삭제 할 수 없습니다.[err:{0}]", ex.Message));
            }
            finally
            {
                oTPDef = null;
            }
        }

        private void butReset_Click(object sender, System.EventArgs e)
        {
            Initialize();
        }

        private void fpItem_EditModeOff(object sender, System.EventArgs e)
        {
            if (fpItem_Sheet1.ActiveColumnIndex == 0)
            {
                if (Array.IndexOf(m_strNotUseFieldName, fpItem_Sheet1.ActiveCell.Text.ToUpper()) > -1)
                {
                    MessageBox.Show(string.Format("Item Name [{0}]은 사용할 수 없습니다.", fpItem_Sheet1.ActiveCell.Text));
                    fpItem_Sheet1.ActiveCell.ResetValue();
                }
            }
        }

        private void TPUCProgramDefine_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;
            Initialize();
        }

        private void cmbArea_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.Text.Equals("PARAMETRIC"))
            {
                lblSiteCount.Enabled = true;
                txtSiteCount.Enabled = true;
            }
            else
            {
                lblSiteCount.Enabled = false;
                txtSiteCount.Text = "";
                txtSiteCount.Enabled = false;
            }
        }

        private void cmbArea_TextChanged(object sender, System.EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.Text.Equals("PARAMETRIC"))
            {
                lblSiteCount.Enabled = true;
                txtSiteCount.Enabled = true;
            }
            else
            {
                lblSiteCount.Enabled = false;
                txtSiteCount.Text = "";
                txtSiteCount.Enabled = false;
            }
        }
    }
}
