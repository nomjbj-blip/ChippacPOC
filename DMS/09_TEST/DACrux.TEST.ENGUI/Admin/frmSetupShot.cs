/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : frmSetupShot.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2013.01.14
--  Description     : DACrux/SPC EDC DataSource Setup UI 
--  History         : Created by YSIM at 2013.01.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2015 년 DACrux V5 History Start
 * 2015-04-22 : YSLEE
    1. Create
----------------------------------------------------------------------------------------------------------*/
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
    public partial class frmSetupShot : DACrux.Framework.Base.DACruxUXBasic01
    {
        private bool m_bFillList = false;

        public frmSetupShot()
        {
            InitializeComponent();
        }



        private void butLoad_Click(object sender, EventArgs e)
        {
            if (m_bFillList) return;

            try
            {
                SetMap();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetMap()
        {
            DataTable dt = null;
            DataTable dtUseMap = null;
            DACrux.TEST.RO.ProbeAdmin oAdmin = null;

            try
            {
                oAdmin = new RO.ProbeAdmin();
                dt = oAdmin.GetMapDef(cmbDevice.SelectedValue.ToString());
                dtUseMap = oAdmin.GetInDies(cmbDevice.SelectedValue.ToString());

                shotMap1.WaferDrawMode = DACrux.Map.MapMode.Fit;
                shotMap1.WaferSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["WAFER_SIZE"].ToString());

                shotMap1.DieSizeX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["CHIP_SIZE_X"].ToString());
                shotMap1.DieSizeY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["CHIP_SIZE_Y"].ToString());

                shotMap1.OriginIndexX = DACrux.Base.Convert.intParse(dt.Rows[0]["ORIGIN_INDEX_X"].ToString());
                shotMap1.OriginIndexY = DACrux.Base.Convert.intParse(dt.Rows[0]["ORIGIN_INDEX_Y"].ToString());

                shotMap1.OriginX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_MICRO_X"].ToString());
                shotMap1.OriginY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_MICRO_Y"].ToString());

                shotMap1.FirstDieX = DACrux.Base.Convert.intParse(dt.Rows[0]["FIRST_INDEX_X"].ToString());
                shotMap1.FirstDieY = DACrux.Base.Convert.intParse(dt.Rows[0]["FIRST_INDEX_Y"].ToString());

                shotMap1.NotchAngle = DACrux.Base.Convert.intParse(dt.Rows[0]["ANGLE"].ToString());
                shotMap1.EdgeSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["EDGE_SIZE"].ToString());
                shotMap1.NotchType = dt.Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                int iXYDir = DACrux.Base.Convert.intParse(dt.Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        shotMap1.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        shotMap1.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        shotMap1.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        shotMap1.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }

                shotMap1.DieClear();

                for (int i = 0; i < dtUseMap.Rows.Count; i++)
                {
                    shotMap1.AddDie(new DACrux.Base.Die(DACrux.Base.Convert.intParse(dtUseMap.Rows[i]["X"].ToString())
                        , DACrux.Base.Convert.intParse(dtUseMap.Rows[i]["Y"].ToString())
                        , 0
                        , 0, 0, 0, 0, 1));

                }

                shotMap1.WaferDrawMode = DACrux.Map.MapMode.Fit;
                shotMap1.CenterMark = true;
                shotMap1.Redraw();

                dt = oAdmin.SelectShotOrigin(cmbDevice.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    shotMap1.SetShotMap(DACrux.Base.Convert.intParse(dt.Rows[0]["MINX"].ToString())
                        , DACrux.Base.Convert.intParse(dt.Rows[0]["MINY"].ToString())
                        , DACrux.Base.Convert.intParse(dt.Rows[0]["MAXX"].ToString())
                        , DACrux.Base.Convert.intParse(dt.Rows[0]["MAXY"].ToString()));
                }
            }
            catch
            {
                MessageBox.Show(string.Format("Device define check. [{0}]'s map doesn't exist", cmbDevice.Text));
            }
            finally
            {
                oAdmin = null;
                if (dt != null) dt.Dispose();
                dt = null;
                if (dtUseMap != null) dtUseMap.Dispose();
                dtUseMap = null;
            }
        }

        private void FillDeviceList()
        {
            RO.ProbeAdmin oAdmin = new RO.ProbeAdmin();
            DataTable dt = new DataTable();
            try
            {
                m_bFillList = true;
                dt = oAdmin.GetProductList();
                dt.Rows.InsertAt(dt.NewRow(), 0);
                dt.AcceptChanges();
                cmbDevice.DataSource = dt;
                cmbDevice.DisplayMember = "PRODUCT";
                cmbDevice.ValueMember = "MAPID";
                m_bFillList = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            DataTable dt = shotMap1.GetDataSource().Tables[0];

            RO.ProbeAdmin oAdmin = new RO.ProbeAdmin();

            if (dt.Rows.Count > 0)
            {
                oAdmin.UpdateShot(cmbDevice.Text, ref dt);
            }
        }

        

        private void frmSetupShot_Load(object sender, EventArgs e)
        {
            FillDeviceList();
        }

        private void cmbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_bFillList) return;

            try
            {
                SetMap();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
