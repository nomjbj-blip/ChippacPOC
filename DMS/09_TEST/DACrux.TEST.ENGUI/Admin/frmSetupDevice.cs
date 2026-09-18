/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : frmSetupDevice.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2013.01.14
--  Description     : DACrux/SPC EDC DataSource Setup UI 
--  History         : Created by YSIM at 2013.01.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2015 년 DACrux V5 History Start
 * 2015-04-21 : YSLEE
    1. butCreate_Click() : 신규생성, 신규 Device(Product) Map 등록
    2. butUpdate_Click() : 신규생성, 존재하는 Device Map 정보 업데이트
    3. butDelete_Click() : 신규생성, 존재하는 Device Map 정보 삭제, 실제 데이터 삭제가 아닌 DELETE_FLAG = 'Y' 변경
    4. ducDeviceName_OnItemSelected() : Device 리스트 출력, 기존 Device 선택 시 해당 기준 Map 화면 출력, 선택하지 않은 경우 빈 Map 출력
    5. ducDeviceName_OnTextChanged() : Device 직접 입력시, txtCustDevice.Text에 동일한 값을 저장
----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.TEST.RO;

namespace DACrux.TEST.ENGUI
{
    public partial class frmSetupDevice : DACrux.Framework.Base.DACruxUXBasic01
    {
        public frmSetupDevice()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            txtCustDevice.Text = "";
            ducDeviceName.Text = "";
            txtDieSizeX.Text = "";
            txtDieSizeY.Text = "";
            txtDiesX.Text = "";
            txtDiesY.Text = "";
            txtEdgeSize.Text = "";
            txtFacility.Text = "";
            txtNetDie.Text = "";
            txtNotchAngle.Text = "";
            txtNotchType.Text = "";
            txtOrgingIndexX.Text = "";
            txtOrgingIndexY.Text = "";
            txtOriginX.Text = "";
            txtOriginY.Text = "";
            txtWaferSize.Text = "";

            FillMapIDList();
            FillCustomerIDList();

            m_wMap.Reset();
            m_wMap.Redraw();
        }

        private void chkCenterMark_CheckedChanged(object sender, System.EventArgs e)
        {
            m_wMap.CenterMark = chkCenterMark.Checked;
            m_wMap.Redraw();
        }

        private void chkScale_CheckedChanged(object sender, System.EventArgs e)
        {
            m_wMap.ScaleMark = chkScale.Checked;
            m_wMap.Redraw();
        }

        private void FillMapIDList()
        {
            DataTable dt = null;
            RO.ProbeAdmin oProbe = new RO.ProbeAdmin();
            try
            {
                dt = oProbe.GetMapIDList();
                DataRow[] drs = dt.Select("");
                cmbMapIDs.Items.Clear();
                foreach (DataRow dr in drs)
                {
                    cmbMapIDs.Items.Add(dr["MAPID"].ToString());
                }
                cmbMapIDs.Text = "";
            }
            catch
            {
                MessageBox.Show("MapID List를 가져올 수 없습니다");
            }
        }

        private void FillCustomerIDList()
        {
            DataTable dt = null;
            RO.ProbeAdmin oProbe = new RO.ProbeAdmin();
            try
            {
                dt = oProbe.GetCustomerList();
                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "CUSTOMER_ID";
                cmbCustomer.ValueMember = "CUSTOMER_NAME";
                cmbCustomer.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Customer List를 가져올 수 없습니다[ERR:{0}]", ex.Message));
            }
        }

        private void cmbMapIDs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DataTable dt = null;
            ProbeMapAnalysis oProbe = new ProbeMapAnalysis();
            ProbeAdmin oAdmin = new ProbeAdmin();

            try
            {
                dt = oAdmin.GetMapDef(cmbMapIDs.Text);
                txtWaferSize.Text = dt.Rows[0]["WAFER_SIZE"].ToString();
                txtDieSizeX.Text = dt.Rows[0]["CHIP_SIZE_X"].ToString();
                txtDieSizeY.Text = dt.Rows[0]["CHIP_SIZE_Y"].ToString();
                txtOrgingIndexX.Text = dt.Rows[0]["FIRST_INDEX_X"].ToString();
                txtOrgingIndexY.Text = dt.Rows[0]["FIRST_INDEX_Y"].ToString();
                txtOriginX.Text = dt.Rows[0]["FIRST_MICRO_X"].ToString();
                txtOriginY.Text = dt.Rows[0]["FIRST_MICRO_Y"].ToString();
                txtDiesX.Text = string.Format("{0}", DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MAX_X"].ToString())
                                    - DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MIN_X"].ToString()) + 1);
                txtDiesY.Text = string.Format("{0}", DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MAX_Y"].ToString())
                                    - DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MIN_Y"].ToString()) + 1);
                txtEdgeSize.Text = dt.Rows[0]["EDGE_SIZE"].ToString();
                txtNetDie.Text = dt.Rows[0]["NETDIE"].ToString();
                txtNotchType.Text = dt.Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? "Flat" : "Notch";
                txtNotchAngle.Text = dt.Rows[0]["ANGLE"].ToString();

                m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                m_wMap.WaferSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["WAFER_SIZE"].ToString());

                m_wMap.DieSizeX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["CHIP_SIZE_X"].ToString());
                m_wMap.DieSizeY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["CHIP_SIZE_Y"].ToString());

                m_wMap.OriginIndexX = DACrux.Base.Convert.intParse(dt.Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_wMap.OriginIndexY = DACrux.Base.Convert.intParse(dt.Rows[0]["ORIGIN_INDEX_Y"].ToString());

                m_wMap.OriginX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_wMap.OriginY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_MICRO_Y"].ToString());

                m_wMap.FirstDieX = DACrux.Base.Convert.intParse(dt.Rows[0]["FIRST_INDEX_X"].ToString());
                m_wMap.FirstDieY = DACrux.Base.Convert.intParse(dt.Rows[0]["FIRST_INDEX_Y"].ToString());

                m_wMap.NotchAngle = DACrux.Base.Convert.intParse(dt.Rows[0]["ANGLE"].ToString());
                m_wMap.EdgeSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["EDGE_SIZE"].ToString());
                m_wMap.NotchType = DACrux.Base.Notch.Notch; //dt.Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Notch : DACrux.Base.Notch.Flat;

                int iXYDir = DACrux.Base.Convert.intParse(dt.Rows[0]["XY_DIRECTION"].ToString());

                //LeftTop = 0, LeftBottom = 1, RightBottom = 2, RightTop = 3
                switch (iXYDir)
                {
                    case 0:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        m_wMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }

                dt = oAdmin.GetDies(cmbMapIDs.Text);
                m_wMap.DieClear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    m_wMap.AddDie(new DACrux.Base.Die(DACrux.Base.Convert.intParse(dt.Rows[i]["X"].ToString())
                        , DACrux.Base.Convert.intParse(dt.Rows[i]["Y"].ToString())
                        , 0
                        , 0, 0, 0, 0, DACrux.Base.Convert.intParse(dt.Rows[i]["USECODE"].ToString())));

                }

                m_wMap.Width = m_wMap.Width + 1; /// Size를 자동으로 조절하게 하는 Trip
                m_wMap.Redraw();

            }
            catch
            {
                MessageBox.Show(string.Format("MapID [{0}] 정보를 가져올 수 없습니다", cmbMapIDs.Text));
            }
        }

        private void ducDeviceName_TextChanged(object sender, System.EventArgs e)
        {
            txtCustDevice.Text = ducDeviceName.Text;
        }

        private void butCancel_Click(object sender, System.EventArgs e)
        {
            Initialize();
        }

        private void TPUCDevice_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;
            Initialize();
        }

        private void ducDeviceName_DropDown(object sender, EventArgs e)
        {
            RO.ProbeAdmin oProbe = new RO.ProbeAdmin();
            DataTable dt = null;
            try
            {
                oProbe = new ProbeAdmin();
                dt = oProbe.GetProductList();
                ducDeviceName.DataSource = dt;
                ducDeviceName.DisplayColumn = "PRODUCT";
                ducDeviceName.ValueColumn = "PRODUCT";
                ducDeviceName.ValueTextColumn = "PRODUCT";
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void ducDeviceName_OnItemSelected(object sender, ListView.SelectedListViewItemCollection SelectedViewItems)
        {
            RO.ProbeAdmin oProbe = new RO.ProbeAdmin();
            DataTable dt = null;
            try
            {
                oProbe = new RO.ProbeAdmin();
                dt = oProbe.SelectProductInfo01(ducDeviceName.Text);
                if (dt.Rows.Count > 0)
                {
                    txtCustDevice.Text = dt.Rows[0]["CUSTOMER_PROD"].ToString();
                    txtFacility.Text = dt.Rows[0]["FACTORY"].ToString();
                    cmbCustomer.Text = dt.Rows[0]["CUSTOMER_ID"].ToString();
                    cmbMapIDs.Text = dt.Rows[0]["MAPID"].ToString();
                }
                else
                {
                    Initialize();
                }

            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void ducDeviceName_OnTextChanged(object sender, EventArgs e)
        {
            txtCustDevice.Text = ducDeviceName.Text;
        }

        private void butCreate_Click(object sender, EventArgs e)
        {
            RO.ProbeAdmin oProbe = new RO.ProbeAdmin();
            DataTable dt = new DataTable();
            try
            {
                if (txtFacility.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Facility를 입력하여 주세요");
                    txtFacility.Focus();
                    return;
                }

                if (ducDeviceName.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Device 를 입력하여 주세요");
                    ducDeviceName.Focus();
                    return;
                }

                if (cmbCustomer.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Customer등록을 먼저 실행하여 주세요");
                    return;
                }

                if (txtCustDevice.Text.Trim().Length == 0)
                {
                    txtCustDevice.Text = ducDeviceName.Text;
                }

                if (cmbMapIDs.Text.Trim().Length == 0)
                {
                    MessageBox.Show("기준 Map이 선택되지 않았습니다.");
                    return;
                }

                dt = oProbe.SelectProductInfo01(ducDeviceName.Text);
                if (dt.Rows.Count== 0)
                {
                    oProbe.SetDeviceDef(txtFacility.Text.Trim()
                        , ducDeviceName.Text.Trim()
                        , cmbCustomer.Text.Trim()
                        , cmbCustomer.SelectedValue.ToString().Trim()
                        , txtCustDevice.Text.Trim()
                        , cmbMapIDs.Text.Trim());

                    oProbe.CreateAVITable(ducDeviceName.Text.Trim());

                    MessageBox.Show(string.Format("Device [{0}]을 정상등록 하였습니다.", ducDeviceName.Text));
                }
                else
                {
                    //oProbe.UpdateProductInfo01(txtFacility.Text, cmbCustomer.Text, cmbCustomer.Text, txtCustDevice.Text, cmbMapIDs.Text, ducDeviceName.Text);
                    MessageBox.Show(string.Format("동일한 Device [{0}]가 이미 존재합니다.", ducDeviceName.Text));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Device [{0}]을 등록 할 수 없습니다[err:{1}]", ducDeviceName.Text, ex.Message));
            }
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            RO.ProbeAdmin oProbe = new RO.ProbeAdmin();
            DataTable dt = new DataTable();
            try
            {
                dt = oProbe.SelectProductInfo01(ducDeviceName.Text);
                if (dt.Rows.Count > 0)
                {
                    oProbe.UpdateProductInfo01(txtFacility.Text, cmbCustomer.Text, cmbCustomer.Text, txtCustDevice.Text, cmbMapIDs.Text, ducDeviceName.Text);
                    MessageBox.Show(string.Format("Device [{0}]을 수정하였습니다.", ducDeviceName.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Device [{0}]을 수정 할 수 없습니다[err:{1}]", ducDeviceName.Text, ex.Message));
            }
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            RO.ProbeAdmin oProbe = new RO.ProbeAdmin();
            DataTable dt = new DataTable();
            try
            {
                dt = oProbe.SelectProductInfo01(ducDeviceName.Text);
                if (dt.Rows.Count > 0)
                {
                    oProbe.UpdateDeleteFlag("Y", ducDeviceName.Text);
                    MessageBox.Show(string.Format("Device [{0}]을 삭제하였습니다.", ducDeviceName.Text));
                    Initialize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Device [{0}]을 삭제 할 수 없습니다[err:{1}]", ducDeviceName.Text, ex.Message));
            }
        }

        

    }
}
