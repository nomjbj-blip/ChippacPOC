using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.TEST.ENGUI
{
    public partial class frmAdminCustomer : DACrux.Framework.Base.DACruxUXBasic01
    {
        public frmAdminCustomer()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            FillCustomers();
        }

        public void FillCustomers()
        {
            RO.ProbeAdmin oAdmin = new RO.ProbeAdmin();
            DataTable dt = null;
            try
            {
                dt = oAdmin.GetCustomerList();
                fpMain.ActiveSheet.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Customer List를 조회할 수 없습니다.\n\r[{0}]", ex.Message));
            }
        }

        private void butSave_Click(object sender, System.EventArgs e)
        {
            RO.ProbeAdmin oAdmin = new RO.ProbeAdmin();
            try
            {
                //oAdmin.GetCustomerInfo
                oAdmin.CreateCustomer(txtCustomerID.Text
                                    , txtCustomer.Text
                                    , txtCustomerDesc.Text
                                    , chkDataService.Checked
                                    , (int)numUserCount.Value
                                    , dtExpire.Value.ToString("yyyy-MM-dd")
                                    , txtFTPSite.Text
                                    , txtFTPUser.Text
                                    , txtFTPPassword.Text
                                    , txtFTPPath.Text
                                    , txtSendTime.Text
                                    , txtAVIFormat.Text
                                    , txtEDSFormat.Text
                                    , txtInklessFormat.Text
                                    , txtRawDataFormat.Text
                                    , txtCustCode.Text
                                    , txtSprName.Text
                                    , txtMapRcvDir.Text
                                    , txtBackup.Text
                                    , txtError.Text
                                    , txtLog.Text
                                    );

                FillCustomers();

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Customer Create할 수 없습니다.\n\r[{0}]", ex.Message));
            }
        }

        private void butDelete_Click(object sender, System.EventArgs e)
        {
            RO.ProbeAdmin oAdmin = new RO.ProbeAdmin();
            try
            {
                oAdmin.DeleteCustomer(txtCustomerID.Text);
                FillCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Customer Delete 할 수 없습니다.\n\r[{0}]", ex.Message));
            }
        }

        private void butUpdate_Click(object sender, System.EventArgs e)
        {
            RO.ProbeAdmin oAdmin = new RO.ProbeAdmin();
            try
            {
                oAdmin.UpdateCustomer(
                    txtCustomer.Text
                    , chkDataService.Checked
                    , (int)numUserCount.Value
                    , dtExpire.Value.ToString("yyyy-MM-dd")
                    , txtCustomerDesc.Text
                    , txtFTPSite.Text
                    , txtFTPUser.Text
                    , txtFTPPassword.Text
                    , txtFTPPath.Text
                    , txtSendTime.Text
                    , txtAVIFormat.Text
                    , txtEDSFormat.Text
                    , txtInklessFormat.Text
                    , txtRawDataFormat.Text
                    , txtCustCode.Text
                    , txtSprName.Text
                    , txtMapRcvDir.Text
                    , txtBackup.Text
                    , txtError.Text
                    , txtLog.Text
                    , txtCustomerID.Text);

                FillCustomers();

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Customer를 Update 할 수 없습니다.\n\r[{0}]", ex.Message));
            }
        }

        private void fpMain_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            try
            {
                txtCustomerID.Text = fpMain.ActiveSheet.GetValue(e.Range.Row, 0).ToString();
                txtCustomer.Text = fpMain.ActiveSheet.GetValue(e.Range.Row, 1).ToString();
                txtCustomerDesc.Text = fpMain.ActiveSheet.GetValue(e.Range.Row, 2).ToString();
                chkDataService.Checked = fpMain.ActiveSheet.GetValue(e.Range.Row, 3).ToString().Equals("1");
                numUserCount.Value = Convert.ToDecimal(fpMain.ActiveSheet.GetValue(e.Range.Row, 4));
                dtExpire.Value = (DateTime)fpMain.ActiveSheet.GetValue(e.Range.Row, 5);
                txtFTPSite.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 6);
                txtFTPUser.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 7);
                txtFTPPassword.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 8);
                txtFTPPath.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 9);
                txtSendTime.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 10);
                txtAVIFormat.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 11);
                txtEDSFormat.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 12);
                txtInklessFormat.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 13);
                txtRawDataFormat.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 14);
                //txtCustCode.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 15);
                //txtSprName.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 16);
                //txtMapRcvDir.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 17);
                //txtBackup.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 18);
                //txtError.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 19);
                //txtLog.Text = fpMain.ActiveSheet.GetText(e.Range.Row, 20);
                butDelete.Enabled = true;
                butUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("선택한 것의 Data에 문제가 있습니다.\n\r[{0}]", ex.Message));
            }
        }

        private void txtCustCode_TextChanged(object sender, EventArgs e)
        {
            string sProbeDir = @"D:\BUMP\PROBE\";
            string cursPath = sProbeDir + @"MAP\" + txtCustCode.Text;
            string sExtension = cursPath.Substring(cursPath.LastIndexOf('\\') + 1);

            txtMapRcvDir.Text = cursPath;
            txtBackup.Text = sProbeDir + @"BACKUP\" + sExtension;
            txtError.Text = sProbeDir + @"ERROR\" + sExtension;
            txtLog.Text = sProbeDir + @"LOG\" + sExtension;

        }
    }
}
