using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.TEST.RO;

namespace DACrux.TEST.ENGUI
{
    public partial class frmSetupMap : DACrux.Framework.Base.DACruxUXBasic01
    {
        private string m_strCurrentMapID = "UNKNOWN";
        private string m_strMapID = string.Empty;
        private bool m_isModified = false;

        DataTable m_dt = null;
        DataTable m_dsDies = null;

        public frmSetupMap()
        {
            InitializeComponent();
        }

        private void butPropertyMapDraw_Click(object sender, System.EventArgs e)
        {
            try
            {
                MapDraw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Map Drwing 정보가 올바르지 않습니다.[Err:{0}]", ex.Message));
            }
         
        }

        public void MapDraw()
        {
            double orgX = double.NaN;
            double orgY = double.NaN;

            try
            {
                /// Die Size Check
                /// 
                if (txtDieX.Text.Trim().Length == 0 || DACrux.Base.Convert.doubleParse(txtDieX.Text) <= 0)
                {
                    MessageBox.Show("Die X값이 0이거나 음수가 될 수 없습니다.");
                    return;
                }

                if (txtDieY.Text.Trim().Length == 0 || DACrux.Base.Convert.doubleParse(txtDieY.Text) <= 0)
                {
                    MessageBox.Show("Die Y값이 0이거나 음수가 될 수 없습니다.");
                    return;
                }

                /// Origin Check
                /// 
                if (txtOriginX.Text.Trim().Length == 0 || double.TryParse(txtOriginX.Text, out orgX) == false)
                {
                    MessageBox.Show("Origin X값이 없습니다.");
                    return;
                }

                if (txtOriginY.Text.Trim().Length == 0 || double.TryParse(txtOriginY.Text, out orgY) == false)
                {
                    MessageBox.Show("Origin Y값이 없습니다.");
                    return;
                }



                /// 1. Notch Type
                if (rdoNotch.Checked)
                {
                    m_wMap.NotchType = DACrux.Base.Notch.Notch;
                }
                else
                {
                    m_wMap.NotchType = DACrux.Base.Notch.Flat;
                }

                /// 2. Wafer Size
                switch (cmbWaferSize.Text.Replace("\"", ""))
                {
                    case "3":
                        m_wMap.WaferSize = 75.0d;
                        break;
                    case "4":
                        m_wMap.WaferSize = 100.0d;
                        break;
                    case "5":
                        m_wMap.WaferSize = 125.0d;
                        break;
                    case "6":
                        m_wMap.WaferSize = 150.0d;
                        break;
                    case "8":
                        m_wMap.WaferSize = 200.0d;
                        break;
                    case "12":
                        m_wMap.WaferSize = 200000.0d;
                        break;
                }

                /// 3. Edge Size
                m_wMap.EdgeSize = (double)numEdge_Real.Value;


                /// 4. Rotation Angle
                /// 
                int iAngle = m_wMap.NotchAngle;
                int.TryParse(cmbAngel.Text, out iAngle);
                m_wMap.NotchAngle = iAngle;

                /// 5. Notch Type
                m_wMap.NotchType = rdoFlat.Checked ? Notch.Flat : Notch.Notch;

                /// 6. XY Direction
                m_wMap.XYDirect = (DACrux.Base.XYDirection)Enum.Parse(typeof(DACrux.Base.XYDirection), cmbXYDirection.Text);

                /// 7. Die Size
                m_wMap.DieSizeX = DACrux.Base.Convert.doubleParse(txtDieX.Text);
                m_wMap.DieSizeY = DACrux.Base.Convert.doubleParse(txtDieY.Text);

                /// 8. Die Min/Max X, Y
                m_wMap.DieMinX = DACrux.Base.Convert.intParse(txtMinX.Text);
                m_wMap.DieMinY = DACrux.Base.Convert.intParse(txtMinY.Text);
                m_wMap.DieMaxX = DACrux.Base.Convert.intParse(txtMaxX.Text);
                m_wMap.DieMaxY = DACrux.Base.Convert.intParse(txtMaxY.Text);

                /// 9. Origin X, Y & Origin Index X, Y
                m_wMap.OriginX = orgX;
                m_wMap.OriginY = orgY;
                m_wMap.OriginIndexX = (int)numOriginIndexX.Value;
                m_wMap.OriginIndexY = (int)numOriginIndexY.Value;

                /// 10. First Index X, Y
                /// 
                /// 10.1 기존의 First 값으로 초기값 Set
                int iFirstX = m_wMap.DieMinX;
                int iFirstY = m_wMap.DieMinY;

                /// 10.2 기존의 First 값을 읽는다.
                int.TryParse(txtFirstX.Text, out iFirstX);
                int.TryParse(txtFirstY.Text, out iFirstY);

                /// 10.3 비교해서 값이 틀리면 작은 값을 취한다.
                if (m_wMap.DieMinX > iFirstX) iFirstX = m_wMap.DieMinX;
                if (m_wMap.DieMinY > iFirstY) iFirstY = m_wMap.DieMinY;
                txtFirstX.Text = iFirstX.ToString();
                txtFirstY.Text = iFirstY.ToString();

                /// 10.4 Map에도 작은 값으로 적용한다.
                m_wMap.FirstDieX = iFirstX;
                m_wMap.FirstDieY = iFirstY;

                m_wMap.DieCalculation(true);
                m_wMap.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void m_wMap_OnChangeCurrentDie(object sender, DACrux.Base.Die NewDie)
        {
            if (rdoIndex.Checked)
            {
                txtXIndex.Text = NewDie.IndexX.ToString();
                txtYIndex.Text = NewDie.IndexY.ToString();
            }
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

        private void rdoTop_CheckedChanged(object sender, System.EventArgs e)
        {
            m_wMap.NotchAngle = DACrux.Base.Convert.intParse(((RadioButton)sender).Text);
            m_wMap.Redraw();
        }

        private void chkToolTip_CheckedChanged(object sender, EventArgs e)
        {
            m_wMap.VisibleOffDie = chkToolTip.Checked;
        }



        public void LoadTSK(string FileName)
        {
            DataTable dt = null;
            DataTable dtDies = null;
            try
            {
                dt = GetTSKMapLoad(FileName, ref dtDies);
                DrawLoadingMap(dt, dtDies);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("TSK File [{0}]를 Load할 수 없습니다.[Err:{0}]", FileName, ex.Message));
            }

        }

        public void LoadMap(string MapID)
        {
            ProbeAdmin oAdmin = new ProbeAdmin();
            try
            {
                pnlRight.Enabled = false;

                m_dt = null;
                m_dsDies = null;
                txtMapID.Text = MapID;
                m_dt = oAdmin.GetMapDef(MapID);
                m_dsDies = oAdmin.GetDies(MapID);
                DrawLoadingMap(m_dt, m_dsDies);

                m_isModified = false;
                butCreate.Enabled = false;
                butUpdate.Enabled = false;
                butDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Map ID [{0}]를 Load할 수 없습니다.[Err:{0}]", MapID, ex.Message));
            }
            finally
            {
                pnlRight.Enabled = true;
                oAdmin = null;
            }
        }

        private void DrawLoadingMap(DataTable dt, DataTable dtDies)
        {
            try
            {
                /// Wafer Size Set
                /// 
                switch (DACrux.Base.Convert.intParse(dt.Rows[0]["WAFER_SIZE"].ToString()))
                {
                    case 100:
                        cmbWaferSize.Text = "4\"";
                        break;
                    case 125:
                        cmbWaferSize.Text = "5\"";
                        break;
                    case 150:
                        cmbWaferSize.Text = "6\"";
                        break;
                    case 200:
                        cmbWaferSize.Text = "8\"";
                        break;
                    case 300:
                        cmbWaferSize.Text = "12\"";
                        break;
                }
                m_wMap.WaferSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["WAFER_SIZE"].ToString());

                /// Notch Angle
                /// 
                m_wMap.NotchAngle = DACrux.Base.Convert.intParse(dt.Rows[0]["ANGLE"].ToString());
                cmbAngel.SelectedIndex = cmbAngel.Items.IndexOf(m_wMap.NotchAngle.ToString());


                /// Notch Type
                /// 
                switch (DACrux.Base.Convert.intParse(dt.Rows[0]["NOTCH_TYPE"].ToString()))
                {
                    case 0:
                        rdoNotch.Checked = true;
                        break;
                    case 1:
                        rdoFlat.Checked = true;
                        break;
                }
                m_wMap.NotchType = (DACrux.Base.Convert.intParse(dt.Rows[0]["NOTCH_TYPE"].ToString()) == 0) ? DACrux.Base.Notch.Notch : DACrux.Base.Notch.Flat;


                /// Edge Size
                /// 
                m_wMap.EdgeSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["EDGE_SIZE"].ToString());
                numEdge_Real.Value = (decimal)m_wMap.EdgeSize;


                /// Die Size x,y
                /// 
                txtDieX.Text = dt.Rows[0]["CHIP_SIZE_X"].ToString();
                txtDieY.Text = dt.Rows[0]["CHIP_SIZE_Y"].ToString();
                m_wMap.DieSizeX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["CHIP_SIZE_X"].ToString());
                m_wMap.DieSizeY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["CHIP_SIZE_Y"].ToString());

                /// Orgin x, y
                /// 
                m_wMap.OriginX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_MICRO_X"].ToString());
                m_wMap.OriginY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_MICRO_Y"].ToString());
                txtOriginX.Text = m_wMap.OriginX.ToString(); //string.Format("{0}", (m_wMap.OriginX - m_wMap.DieSizeX));
                txtOriginY.Text = m_wMap.OriginY.ToString();

                /// Min/Max XY
                /// 
                m_wMap.DieMinX = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MIN_X"].ToString());
                m_wMap.DieMaxX = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MAX_X"].ToString());
                m_wMap.DieMinY = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MIN_Y"].ToString());
                m_wMap.DieMaxY = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_INDEX_MAX_Y"].ToString());

                /// Orgin Index X,Y
                /// 
                m_wMap.OriginIndexX = DACrux.Base.Convert.intParse(dt.Rows[0]["ORIGIN_INDEX_X"].ToString());
                m_wMap.OriginIndexY = DACrux.Base.Convert.intParse(dt.Rows[0]["ORIGIN_INDEX_Y"].ToString());

                numOriginIndexX.Minimum = m_wMap.DieMinX;
                numOriginIndexY.Minimum = m_wMap.DieMinY;
                numOriginIndexX.Maximum = m_wMap.DieMaxX;
                numOriginIndexY.Maximum = m_wMap.DieMaxY;

                numOriginIndexX.Value = (decimal)dt.Rows[0]["ORIGIN_INDEX_X"];
                numOriginIndexY.Value = (decimal)dt.Rows[0]["ORIGIN_INDEX_Y"];

                /// First Die x, y
                /// 
                m_wMap.FirstDieX = DACrux.Base.Convert.intParse(dt.Rows[0]["FIRST_INDEX_X"].ToString());
                m_wMap.FirstDieY = DACrux.Base.Convert.intParse(dt.Rows[0]["FIRST_INDEX_Y"].ToString());
                txtFirstX.Text = dt.Rows[0]["FIRST_INDEX_X"].ToString();
                txtFirstY.Text = dt.Rows[0]["FIRST_INDEX_Y"].ToString();


                /// XY Direction
                /// 
                cmbXYDirection.Text = Enum.GetName(typeof(DACrux.Base.XYDirection),DACrux.Base.Convert.intParse(dt.Rows[0]["XY_DIRECTION"].ToString()));
                m_wMap.XYDirect = (DACrux.Base.XYDirection)Enum.Parse(typeof(DACrux.Base.XYDirection), dt.Rows[0]["XY_DIRECTION"].ToString());

                m_wMap.ReferenceDieSetting = DACrux.Base.Convert.intParse(dt.Rows[0]["REFERENCEDIE_SETTING"].ToString());

                m_wMap.DieCalculation(true);
                m_wMap.DieClear();

                for (int i = 0; i < dtDies.Rows.Count; i++)
                {
                    m_wMap.AddDie(new DACrux.Base.Die(DACrux.Base.Convert.intParse(dtDies.Rows[i]["X"].ToString())
                        , DACrux.Base.Convert.intParse(dtDies.Rows[i]["Y"].ToString())
                        , 0
                        , 0, 0, 0, 0, DACrux.Base.Convert.intParse(dtDies.Rows[i]["USECODE"].ToString())));
                }

                txtMinX.Text = m_wMap.DieMinX.ToString();
                txtMaxX.Text = m_wMap.DieMaxX.ToString();
                txtMinY.Text = m_wMap.DieMinY.ToString();
                txtMaxY.Text = m_wMap.DieMaxY.ToString();

                m_wMap.WaferDrawMode = Map.MapMode.Fit;
                m_wMap.CenterMark = true;
                m_wMap.Redraw();
                txtNetDie_Real.Text = m_wMap.NetDie.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetTSKMapLoad(string FileName, ref DataTable dtDies)
        {
            DataTable dt = null;

            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("MAPID", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("WAFER_SIZE", System.Type.GetType("System.Double")));
                dt.Columns.Add(new DataColumn("CHIP_SIZE_X", System.Type.GetType("System.Double")));
                dt.Columns.Add(new DataColumn("CHIP_SIZE_Y", System.Type.GetType("System.Double")));
                dt.Columns.Add(new DataColumn("ORIGIN_MICRO_X", System.Type.GetType("System.Double")));
                dt.Columns.Add(new DataColumn("ORIGIN_MICRO_Y", System.Type.GetType("System.Double")));
                dt.Columns.Add(new DataColumn("ORIGIN_INDEX_X", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("ORIGIN_INDEX_Y", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("FIRST_MICRO_X", System.Type.GetType("System.Double")));
                dt.Columns.Add(new DataColumn("FIRST_MICRO_Y", System.Type.GetType("System.Double")));
                dt.Columns.Add(new DataColumn("FIRST_INDEX_X", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("FIRST_INDEX_Y", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("EDGE_SIZE", System.Type.GetType("System.Double")));
                dt.Columns.Add(new DataColumn("ANGLE", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("NETDIE", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("NOTCH_TYPE", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("DIE_INDEX_MIN_X", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("DIE_INDEX_MAX_X", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("DIE_INDEX_MIN_Y", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("DIE_INDEX_MAX_Y", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("XY_DIRECTION", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("REFERENCEDIE_SETTING", System.Type.GetType("System.Int32")));

                dtDies = new DataTable();
                dtDies.Columns.Add(new DataColumn("X", System.Type.GetType("System.Int32")));
                dtDies.Columns.Add(new DataColumn("Y", System.Type.GetType("System.Int32")));
                dtDies.Columns.Add(new DataColumn("USECODE", System.Type.GetType("System.Int32")));

                FDll.TSKdll oTSKParse = new FDll.TSKdll();
                oTSKParse.ReadBinaryFile(FileName);

                int xyDirect = 0;

                if (oTSKParse.XDirection == 2)
                {
                    if (oTSKParse.YDirection == 1)
                    {
                        xyDirect = 0;
                    }
                    else if (oTSKParse.YDirection == 2)
                    {
                        xyDirect = 1;
                    }
                }
                else if (oTSKParse.XDirection == 1)
                {
                    if (oTSKParse.YDirection == 1)
                    {
                        xyDirect = 2;
                    }
                    else if (oTSKParse.YDirection == 2)
                    {
                        xyDirect = 3;
                    }
                }

                int iMinX = 999999;
                int iMaxX = -999999;
                int iMinY = 999999;
                int iMaxY = -999999;

                foreach (FDll.RowData oDie in oTSKParse.RowDatas)
                {
                    if (iMinX > oDie.DieX) iMinX = oDie.DieX;
                    if (iMinY > oDie.DieY) iMinY = oDie.DieY;
                    if (iMaxX < oDie.DieX) iMaxX = oDie.DieX;
                    if (iMaxY < oDie.DieY) iMaxY = oDie.DieY;
                }

                int defDieX = iMinX - 1;
                int defDieY = iMinY - 1;

                iMinX = 1;
                iMinY = 1;
                iMaxX = iMaxX - defDieX;
                iMaxY = iMaxY - defDieY;

                double dStartX = (oTSKParse.WaferSize - oTSKParse.ChipSizeX * (oTSKParse.XDies + 1)) / 2;
                double dStartY = (oTSKParse.WaferSize - oTSKParse.ChipSizeY * oTSKParse.YDies) / 2;

                switch (oTSKParse.ReferenceDieSetting)
                {
                    case 0:

                        dt.Rows.Add(new object[] {"TSKFILE",
															   oTSKParse.WaferSize,
															   oTSKParse.ChipSizeX,
															   oTSKParse.ChipSizeY,
															   oTSKParse.OriginX + (oTSKParse.WaferSize/2 - dStartX),
															   oTSKParse.OriginY - (oTSKParse.WaferSize/2 - dStartY),
															   iMinX,
															   iMinY,
															   oTSKParse.TargetDieX * oTSKParse.ChipSizeX,
															   oTSKParse.TargetDieY * oTSKParse.ChipSizeY,
															   oTSKParse.TargetDieX,
															   oTSKParse.TargetDieY,
															   oTSKParse.EdgeSize,
															   oTSKParse.Angle,
															   oTSKParse.TestDies,
															   oTSKParse.NotchType,
															   iMinX,
															   iMaxX,
															   iMinY,
															   iMaxY,
															   xyDirect,
															   oTSKParse.ReferenceDieSetting});
                        break;
                    case 1:
                        dt.Rows.Add(new object[] {"TSKFILE",
															   oTSKParse.WaferSize,
															   oTSKParse.ChipSizeX,
															   oTSKParse.ChipSizeY,
															   oTSKParse.OriginX + oTSKParse.ChipSizeX/2,
															   oTSKParse.OriginY + oTSKParse.ChipSizeY/2,
															   oTSKParse.OriginDieX,
															   oTSKParse.OriginDieY,
															   oTSKParse.TargetX,
															   oTSKParse.TargetY,
															   oTSKParse.TargetDieX * oTSKParse.ChipSizeX,
															   oTSKParse.TargetDieY * oTSKParse.ChipSizeY,
															   oTSKParse.EdgeSize,
															   oTSKParse.Angle,
															   oTSKParse.TestDies,
															   oTSKParse.NotchType,
															   iMinX,
															   iMaxX,
															   iMinY,
															   iMaxY,
															   xyDirect,
															   oTSKParse.ReferenceDieSetting});
                        break;
                    case 2:
                        if ((oTSKParse.OriginX + oTSKParse.OriginY + oTSKParse.OriginDieX + oTSKParse.OriginDieY) == 0)
                        {
                            dt.Rows.Add(new object[] {"TSKFILE",
																   oTSKParse.WaferSize,
																   oTSKParse.ChipSizeX,
																   oTSKParse.ChipSizeY,
																   oTSKParse.OriginX + (oTSKParse.WaferSize/2 - dStartX),
																   oTSKParse.OriginY - (oTSKParse.WaferSize/2 - dStartY),
																   oTSKParse.FirstDieX,
																   oTSKParse.FirstDieY,
																   oTSKParse.TargetDieX * oTSKParse.ChipSizeX,
																   oTSKParse.TargetDieY * oTSKParse.ChipSizeY,
																   oTSKParse.TargetDieX,
																   oTSKParse.TargetDieY,
																   oTSKParse.EdgeSize,
																   oTSKParse.Angle,
																   oTSKParse.TestDies,
																   oTSKParse.NotchType,
																   iMinX,
																   iMaxX,
																   iMinY,
																   iMaxY,
																   xyDirect,
																   oTSKParse.ReferenceDieSetting});
                        }
                        else if ((oTSKParse.DieIndexMinX + oTSKParse.DieIndexMaxX + oTSKParse.DieIndexMinY + oTSKParse.DieIndexMaxY) == 0)
                        {

                            dt.Rows.Add(new object[] {"TSKFILE",
																   oTSKParse.WaferSize,
																   oTSKParse.ChipSizeX,
																   oTSKParse.ChipSizeY,
																   oTSKParse.OriginX + oTSKParse.ChipSizeX/2,
																   oTSKParse.OriginY + oTSKParse.ChipSizeY/2,
																   oTSKParse.OriginDieX,
																   oTSKParse.OriginDieY,
																   oTSKParse.TargetX,
																   oTSKParse.TargetY,
																   oTSKParse.TargetDieX,
																   oTSKParse.TargetDieY,
																   oTSKParse.EdgeSize,
																   oTSKParse.Angle,
																   oTSKParse.TestDies,
																   oTSKParse.NotchType,
																   iMinX,
																   iMaxX,
																   iMinY,
																   iMaxY,
																   xyDirect,
																   oTSKParse.ReferenceDieSetting});
                        }
                        break;
                    case 3:
                        break;
                }

                foreach (FDll.RowData oDie in oTSKParse.RowDatas)
                {
                    dtDies.Rows.Add(new object[] { oDie.DieX - defDieX, oDie.DieY - defDieY, oDie.DieAttribute });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public void CreateMap(string MapID)
        {
            ProbeAdmin oAdmin;
            DataTable dt = null;
            try
            {
                oAdmin = new ProbeAdmin();
                /// Recipe 부분
                /// 
                oAdmin.SetMapDef(MapID
                    , m_wMap.WaferSize
                    , m_wMap.DieSizeX
                    , m_wMap.DieSizeY
                    , m_wMap.OriginX
                    , m_wMap.OriginY
                    , m_wMap.OriginIndexX
                    , m_wMap.OriginIndexY
                    , m_wMap.FirstDieX * m_wMap.DieSizeX
                    , m_wMap.FirstDieY * m_wMap.DieSizeY
                    , m_wMap.FirstDieX
                    , m_wMap.FirstDieY
                    , m_wMap.EdgeSize
                    , m_wMap.NotchAngle
                    , m_wMap.NetDie
                    , (m_wMap.NotchType == DACrux.Base.Notch.Notch) ? 0 : 1
                    , 0
                    , 0
                    , 0
                    , 0
                    , 0
                    , 0
                    , m_wMap.DieMinX
                    , m_wMap.DieMaxX
                    , m_wMap.DieMinY
                    , m_wMap.DieMaxY
                    , (int)m_wMap.XYDirect
                    , m_wMap.ReferenceDieSetting);

                /// Map 부분
                /// 
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("X", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("Y", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("USECODE", System.Type.GetType("System.Int32")));

                for (int i = 0; i < m_wMap.Dies.Count; i++)
                {
                    dt.Rows.Add(new object[] { m_wMap.Dies[i].IndexX, m_wMap.Dies[i].IndexY, m_wMap.Dies[i].DieProp });
                }

                oAdmin.CreateUseMap(DACrux.Base.GlobalVariable.Factory, MapID, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oAdmin = null;
            }
        }

        public void UpdateMap(string MapID)
        {
            ProbeAdmin oAdmin = new ProbeAdmin();
            try
            {
                if (DialogResult.OK == MessageBox.Show(string.Format("Map 정보를 수정하면 기존의 Data를 정상적으로 보실수 없을 수도 있습니다.\n\rLoad된 [{0}]의 정보를 Update하겠습니까?", MapID)
                    , "Update Map Infomation !!"
                    , MessageBoxButtons.OKCancel
                    , MessageBoxIcon.Question
                    , MessageBoxDefaultButton.Button1))
                {

                    switch(cmbXYDirection.SelectedItem.ToString())
                    {
                        case "LeftTop":
                            m_wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                            break;
                        case "LeftBottom" :
                            m_wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                            break;
                        case "RightBottom":
                            m_wMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                            break;
                        case "RightTop":
                            m_wMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                            break;
                    }

                    base.SetMainStatusBarProgress(20);
                    oAdmin.UpdateMapDef(
                         MapID
                        , m_wMap.WaferSize
                        , m_wMap.DieSizeX
                        , m_wMap.DieSizeY
                        , m_wMap.OriginX
                        , m_wMap.OriginY
                        , m_wMap.OriginIndexX
                        , m_wMap.OriginIndexY
                        , (m_wMap.FirstDieX - 1) * m_wMap.DieSizeX
                        , (m_wMap.FirstDieY - 1) * m_wMap.DieSizeY
                        , m_wMap.FirstDieX
                        , m_wMap.FirstDieY
                        , m_wMap.EdgeSize
                        , m_wMap.NotchAngle
                        , m_wMap.NetDie
                        , (m_wMap.NotchType == DACrux.Base.Notch.Notch) ? 0 : 1
                        , 0
                        , 0
                        , 0
                        , 0
                        , 0
                        , 0
                        , m_wMap.DieMinX
                        , m_wMap.DieMaxX
                        , m_wMap.DieMinY
                        , m_wMap.DieMaxY
                        , (int)m_wMap.XYDirect);
                    
                    base.SetMainStatusBarProgress(40);

                    CreateUseMap(MapID);
                    MessageBox.Show(string.Format("Map ID [{0}]이 정상으로 Update되었습니다.", MapID));

                    LoadMap(MapID);
                    base.SetMainStatusBarProgress(90);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oAdmin = null;
                base.SetMainStatusBarProgress(100);
            }
        }

        public void DeleteMap(string MapID)
        {
            ProbeAdmin oAdmin = new ProbeAdmin();
            try
            {
                if (DialogResult.OK == MessageBox.Show(string.Format("Map 정보를 삭제하면 기존의 Data를 정상적으로 보실수 없을 수도 있습니다.\n\rLoad된 [{0}]의 정보를 Delete하겠습니까?", MapID)
                    , "Delete Map !!"
                    , MessageBoxButtons.OKCancel
                    , MessageBoxIcon.Warning
                    , MessageBoxDefaultButton.Button1))
                {
                    oAdmin.DeleteMapDef(
                          MapID
                         ,DACrux.Base.GlobalVariable.UserID
                         );

                    MessageBox.Show(string.Format("Map ID [{0}]이 정상으로 Delete되었습니다.", MapID));
                    FillMapIDList();
                    butClear_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oAdmin = null;
            }
        }

        private void CreateUseMap(string MapID)
        {
            ProbeAdmin oMapDef = new ProbeAdmin();
            DataTable dt = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("X", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("Y", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("USECODE", System.Type.GetType("System.Int32")));

                base.SetMainStatusBarProgress(50);

                for (int i = 0; i < m_wMap.Dies.Count; i++)
                {
                    dt.Rows.Add(new object[] { m_wMap.Dies[i].IndexX, m_wMap.Dies[i].IndexY, m_wMap.Dies[i].DieProp });
                }

                base.SetMainStatusBarProgress(60);
                oMapDef.CreateUseMap(DACrux.Base.GlobalVariable.Factory, MapID, dt);
                base.SetMainStatusBarProgress(80);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMapDef = null;
            }
        }

        private void SaveAsMap()
        {
            frmNewMapID oNewMapID = null;
            try
            {
                oNewMapID = new frmNewMapID();

                if (oNewMapID.ShowDialog(this) == DialogResult.OK)
                {
                    SaveAsMap(oNewMapID.NewMapID);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oNewMapID != null) oNewMapID.Dispose();
            }
        }

        private void SaveAsMap(string NewMapID)
        {
            try
            {
                if (!ExistMapID(NewMapID))
                {
                    /// 중복되는 ID가 없을때
                    m_strCurrentMapID = NewMapID;
                    CreateMap(m_strCurrentMapID);
                    this.Text = this.Text.Replace("*", "");
                }
                else
                {
                    /// 이미 존재 할때
                    if (MessageBox.Show(this, string.Format("이미 ID:'{0}' 이 존재 합니다. Overrwite 하시겠습니까?", NewMapID)
                                    , "중복된 Map ID", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        m_strCurrentMapID = NewMapID;
                        UpdateMap(m_strCurrentMapID);
                        this.Text = this.Text.Replace("*", "");
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ExistMapID(string MapID)
        {
            ProbeAdmin oMapDef = null;
            try
            {
                oMapDef = new ProbeAdmin();
                return oMapDef.CheckDuplicateMapID(MapID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMapDef = null;
            }
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            frmSelectMapID oSelMap = null;
            try
            {
                switch (e.Tool.Key)
                {
                    case "OPEN_TSK":
                        if (openFileDialog2.ShowDialog(this) == DialogResult.OK)
                        {
                            LoadTSK(openFileDialog2.FileName);
                            m_strCurrentMapID = "UNKNOWN";
                        }
                        break;
                    case "LOADDEVICEMAP":
                        oSelMap = new frmSelectMapID();
                        if (oSelMap.ShowDialog(this) == DialogResult.OK)
                        {
                            m_strCurrentMapID = oSelMap.SelectedMapID();
                            LoadMap(m_strCurrentMapID);
                        }
                        break;
                    case "SAVE_MAP":
                        if (m_strCurrentMapID.Equals("UNKNOWN"))
                            SaveAsMap();
                        else
                            UpdateMap(m_strCurrentMapID);
                        break;
                    case "SAVE_AS_MAP":
                        SaveAsMap();
                        break;
                }
                this.Text = string.Format("Standard Map Definition-[{0}]", m_strCurrentMapID);
            }
            catch (Exception ex)
            {
                switch (e.Tool.Key)
                {
                    case "OPEN_TSK":
                        MessageBox.Show(string.Format("TSK File을 Load할 수 없습니다.[Err:{0}]", ex.Message));
                        break;
                    case "LOADDEVICEMAP":
                        MessageBox.Show(string.Format("DB에서 Map을 Load할 수 없습니다.[Err:{0}]", ex.Message));
                        break;
                    case "SAVE_MAP":
                    case "SAVE_AS_MAP":
                        MessageBox.Show(string.Format("저장하는 도중 오류가 발생하였습니다.[Err:{0}]", ex.Message));
                        break;
                }
            }
            finally
            {
                if (oSelMap != null) oSelMap.Dispose();
            }
        }


        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) ||
                char.IsSymbol(e.KeyChar) ||
                char.IsWhiteSpace(e.KeyChar) ||
                char.IsPunctuation(e.KeyChar))
                e.Handled = true;
            if (e.KeyChar == '-' && (sender as TextBox).Text.StartsWith("-"))
            {
                // - 허용안함
                e.Handled = true;
            }
                // . 을 허용하되 1개만 허용
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') < 0)
            {
                e.Handled = false;
            }
        }

        private void WithMinusIntTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) ||
                char.IsSymbol(e.KeyChar) ||
                char.IsWhiteSpace(e.KeyChar) ||
                char.IsPunctuation(e.KeyChar))
                e.Handled = true;
            
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length == 0)
            {
                // - 허용함
                e.Handled = false;
            }

            // Integer는 . 을 허용안함
            if (e.KeyChar == '.')
            {
                e.Handled = true;
            }
        }

        private void butVirtualCalculate_Click(object sender, EventArgs e)
        {
            DlgSetupMap dsm = null;

            int minX = 1;
            int minY = 1;
            int maxX = 1;
            int maxY = 1;

            try
            {
                int.TryParse(txtMinX.Text, out minX);
                int.TryParse(txtMinY.Text, out minY);
                int.TryParse(txtMaxX.Text, out maxX);
                int.TryParse(txtMaxY.Text, out maxY);

                dsm = new DlgSetupMap(minX,minY,maxX,maxY);
                dsm.OnWaferGridApply += new WaferGridApply(dsm_OnWaferGridApply);
                dsm.TopMost = true;
                dsm.ShowInTaskbar = false;
                dsm.Show();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        void dsm_OnWaferGridApply(int MinX, int MinY, int MaxX, int MaxY)
        {
            double wsize = m_wMap.WaferSize;
            double wmargin_tmp = m_wMap.WaferMargin;
            double wmargin = m_wMap.WaferMargin;
            double wedge = m_wMap.EdgeSize;
            int iFirstX = MinX;
            int iFirstY = MinY;

            try
            {
                txtMinX.Text = MinX.ToString();
                txtMinY.Text = MinY.ToString();
                txtMaxX.Text = MaxX.ToString();
                txtMaxY.Text = MaxY.ToString();

                int xcnt = MaxX - MinX +1;
                int ycnt = MaxY - MinY +1;

                if (double.TryParse(txtWaferMargin.Text, out wmargin_tmp)) wmargin = wmargin_tmp / 100d;
                wedge = (double)numEdge_Real.Value;

                double dDieSizeX = Math.Round(((wsize - wedge * 2) * wmargin) / xcnt, 5);
                double dDieSizeY = Math.Round(((wsize - wedge * 2) * wmargin) / ycnt, 5);

                txtDieX.Text = dDieSizeX.ToString();
                txtDieY.Text = dDieSizeY.ToString();

                txtXCnt.Text = xcnt.ToString();
                txtYCnt.Text = ycnt.ToString();


                numOriginIndexX.Minimum = MinX;
                numOriginIndexY.Minimum = MinY;
                numOriginIndexX.Maximum = MaxX;
                numOriginIndexY.Maximum = MaxY;

                numOriginIndexX.Value = MinX + (int)(xcnt / 2) - ((xcnt + 1) % 2);
                numOriginIndexY.Value = MinY + (int)(ycnt / 2) - ((ycnt + 1) % 2);

                if (cmbXYDirection.Text.IndexOf("Left") > -1)
                {
                    txtOriginX.Text = (-dDieSizeX * 0.5 * (xcnt % 2)).ToString();
                }
                else
                {
                    txtOriginX.Text = (dDieSizeX * 0.5 * (xcnt % 2)).ToString();
                }


                if (cmbXYDirection.Text.IndexOf("Bottom") > -1)
                {
                    txtOriginY.Text = (-dDieSizeY * 0.5 * (ycnt % 2)).ToString();
                }
                else
                {
                    txtOriginY.Text = (dDieSizeY * 0.5 * (ycnt % 2)).ToString();
                }

                int.TryParse(txtFirstX.Text, out iFirstX);
                int.TryParse(txtFirstY.Text, out iFirstY);
                if (MinX > iFirstX) iFirstX = MinX;
                if (MinY > iFirstY) iFirstY = MinY;
                txtFirstX.Text = iFirstX.ToString();
                txtFirstY.Text = iFirstY.ToString();

            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butSetOrigin_Click(object sender, EventArgs e)
        {
            DlgSetOrigin dso = null;
            try
            {
                double orgX = 0d;
                double orgY = 0d;

                double.TryParse(txtOriginX.Text , out orgX);
                double.TryParse(txtOriginY.Text , out orgY);

                dso = new DlgSetOrigin(orgX, orgY);
                dso.OnSetOrigin += new SetOrigin(dso_OnSetOrigin);
                dso.TopMost = true;
                dso.ShowInTaskbar = false;
                dso.Show();

            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        void dso_OnSetOrigin(double OriginX, double OriginY)
        {
            try
            {
                txtOriginX.Text = OriginX.ToString();
                txtOriginY.Text = OriginY.ToString();

                m_wMap.OriginX = OriginX;
                m_wMap.OriginY = OriginY;

                m_wMap.Redraw();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            try
            {
                /// Wafer Size Set
                /// 
                    // 변경없음
                /// Notch Angle
                /// 
                    // 변경없음
                /// Notch Type
                /// 
                    // 변경없음
                /// Edge Size
                /// 
                    // 변경없음
                /// Die Size x,y
                /// 

                m_wMap.Reset();
                m_wMap.Redraw();

                txtMapID.Text = "";
                txtDieX.Text = "";
                txtDieY.Text = "";

                /// Orgin x, y
                /// 
                txtOriginX.Text = "0"; //string.Format("{0}", (m_wMap.OriginX - m_wMap.DieSizeX));
                txtOriginY.Text = "0";


                /// Orgin Index X,Y
                /// 

                int MinX = (int)numOriginIndexX.Minimum;
                int MinY = (int)numOriginIndexY.Minimum;
                int MaxX = (int)numOriginIndexX.Maximum;
                int MaxY = (int)numOriginIndexY.Maximum;

                int xcnt = MaxX - MinX +1;
                int ycnt = MaxY - MinY +1;

                numOriginIndexX.Value = MinX + (int)(xcnt / 2) - ((xcnt + 1) % 2);
                numOriginIndexY.Value = MinY + (int)(ycnt / 2) - ((ycnt + 1) % 2);


                /// First Die x, y
                /// 
                m_wMap.FirstDieX = 0;
                m_wMap.FirstDieY = 0;


                /// XY Direction
                /// 
                    //변경없음
                /// Min/Max XY
                /// 

                txtMinX.Text = "";
                txtMaxX.Text = "";
                txtMinY.Text = "";
                txtMaxY.Text = "";

                txtNetDie_Real.Text = "";

            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }


        private void InitMap()
        {
            try
            {
                /// 1. Notch Type
                if (rdoNotch.Checked)
                {
                    m_wMap.NotchType = DACrux.Base.Notch.Notch;
                }
                else
                {
                    m_wMap.NotchType = DACrux.Base.Notch.Flat;
                }

                /// 2. Wafer Size
                switch (cmbWaferSize.Text.Replace("\"", ""))
                {
                    case "3":
                        m_wMap.WaferSize = 75.0d;
                        break;
                    case "4":
                        m_wMap.WaferSize = 100.0d;
                        break;
                    case "5":
                        m_wMap.WaferSize = 125.0d;
                        break;
                    case "6":
                        m_wMap.WaferSize = 150.0d;
                        break;
                    case "8":
                        m_wMap.WaferSize = 200.0d;
                        break;
                    case "12":
                        m_wMap.WaferSize = 200000.0d;
                        break;
                }

                /// 3. Edge Size
                m_wMap.EdgeSize = (double)numEdge_Real.Value;


                /// 4. Rotation Angle
                /// 
                if(string.IsNullOrEmpty(cmbAngel.Text) == false)
                m_wMap.NotchAngle = DACrux.Base.Convert.intParse(cmbAngel.Text);

                /// 5. XY Direction
                /// 
                if (string.IsNullOrEmpty(cmbXYDirection.Text) == false)
                m_wMap.XYDirect = (DACrux.Base.XYDirection)Enum.Parse(typeof(DACrux.Base.XYDirection), cmbXYDirection.Text);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void frmSetupMap_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try{
                FillMapIDList();
                InitMap();
            }catch(Exception ex)
            {
                DspError(ex);
            }
        }



        private void m_wMap_OnRedefineFirstDie(object sender, Die NewDie)
        {
            try
            {
                txtFirstX.Text = NewDie.IndexX.ToString();
                txtFirstY.Text = NewDie.IndexY.ToString();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void m_wMap_OnChangePosition(object sender, PointD Currpoint, PointD RealPoint)
        {
            if (rdoMicron.Checked)
            {
                txtXIndex.Text = Math.Round(Currpoint.X, 5).ToString();
                txtYIndex.Text = Math.Round(Currpoint.Y, 5).ToString();
            }
            else if (rdoMicronWafer.Checked)
            {
                txtXIndex.Text = Math.Round(RealPoint.X, 5).ToString();
                txtYIndex.Text = Math.Round(RealPoint.Y, 5).ToString();
            }
        }

        private void m_wMap_OnChangeDieProperty(object sender, Die NewDie)
        {
            txtNetDie_Real.Text = m_wMap.NetDie.ToString();
            m_isModified = true;
            ButtonCondition();
        }

        private void FillMapIDList()
        {
            RO.ProbeAdmin oProbe = null;
            lstMapID.Enabled = false;
            try
            {
                DataTable dt = null;
                oProbe = new RO.ProbeAdmin();

                dt = oProbe.GetMapIDList();

                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add(new DataColumn("MAPID", System.Type.GetType("System.String")));
                }

                lstMapID.DataSource = dt;
                lstMapID.DisplayMember = "MAPID";
                lstMapID.SelectedIndex = -1; 
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                lstMapID.Enabled = true;
            }
        }


        private void ButtonCondition()
        {
            try
            {
                if (string.IsNullOrEmpty(m_strCurrentMapID) && m_strCurrentMapID != txtMapID.Text)
                {
                    butCreate.Enabled = true;
                    butUpdate.Enabled = false;
                    butDelete.Enabled = false;
                    return;
                }

                if (m_strCurrentMapID == txtMapID.Text)
                {
                    double dWaferSize = 0d;
                    switch (cmbWaferSize.Text.Replace("\"", ""))
                    {
                        case "3":
                            dWaferSize = 75.0d;
                            break;
                        case "4":
                            dWaferSize = 100.0d;
                            break;
                        case "5":
                            dWaferSize = 125.0d;
                            break;
                        case "6":
                            dWaferSize = 150.0d;
                            break;
                        case "8":
                            dWaferSize = 200.0d;
                            break;
                        case "12":
                            dWaferSize = 200000.0d;
                            break;
                    }

                    //LeftTop = 0, LeftBottom = 1, RightBottom = 2, RightTop = 3 
                    int iXYDirection = (int)Enum.Parse(typeof(DACrux.Base.XYDirection), cmbXYDirection.Text);

                    /// Angle 0, 90, 180, 270
                    int iAngle = 0;
                    iAngle = DACrux.Base.Convert.intParse(cmbAngel.Text);

                    /// Notch Type Notch = 0, Flat = 1
                    int iNotchType = 0;
                    if (rdoNotch.Checked) iNotchType = 0;
                    else iNotchType = 1;

                    /// Die Size
                    double dChipSizeX = m_wMap.DieSizeX;
                    double dChipSizeY = m_wMap.DieSizeY;
                    double.TryParse(txtDieX.Text , out dChipSizeX);
                    double.TryParse(txtDieY.Text , out dChipSizeY);


                    /// Origin Index
                    int iOriginIndexX = (int)numOriginIndexX.Value;
                    int iOriginIndexY = (int)numOriginIndexY.Value;


                    /// Origin Posigion
                    double dOriginX = m_wMap.OriginX;
                    double dOriginY = m_wMap.OriginY;
                    double.TryParse(txtOriginX.Text, out dOriginX);
                    double.TryParse(txtOriginY.Text, out dOriginY);


                    /// First Index
                    int iFirstX = 1;
                    int iFirstY = 1;
                    int.TryParse(txtFirstX.Text, out iFirstX);
                    int.TryParse(txtFirstY.Text, out iFirstY);


                    if (DACrux.Base.Convert.doubleParse(m_dt.Rows[0]["WAFER_SIZE"].ToString()) != dWaferSize
                        || DACrux.Base.Convert.doubleParse(m_dt.Rows[0]["EDGE_SIZE"].ToString()) != (double)numEdge_Real.Value
                        || DACrux.Base.Convert.intParse(m_dt.Rows[0]["XY_DIRECTION"].ToString()) != iXYDirection
                        || DACrux.Base.Convert.intParse(m_dt.Rows[0]["ANGLE"].ToString()) != iAngle
                        || DACrux.Base.Convert.intParse(m_dt.Rows[0]["NOTCH_TYPE"].ToString()) != iNotchType
                        || DACrux.Base.Convert.doubleParse(m_dt.Rows[0]["CHIP_SIZE_X"].ToString()) != dChipSizeX
                        || DACrux.Base.Convert.doubleParse(m_dt.Rows[0]["CHIP_SIZE_Y"].ToString()) != dChipSizeY
                        || DACrux.Base.Convert.intParse(m_dt.Rows[0]["ORIGIN_INDEX_X"].ToString()) != iOriginIndexX
                        || DACrux.Base.Convert.intParse(m_dt.Rows[0]["ORIGIN_INDEX_Y"].ToString()) != iOriginIndexY
                        || DACrux.Base.Convert.doubleParse(m_dt.Rows[0]["ORIGIN_MICRO_X"].ToString()) != dOriginX
                        || DACrux.Base.Convert.doubleParse(m_dt.Rows[0]["ORIGIN_MICRO_Y"].ToString()) != dOriginY
                        || DACrux.Base.Convert.intParse(m_dt.Rows[0]["FIRST_INDEX_X"].ToString()) != iFirstX
                        || DACrux.Base.Convert.intParse(m_dt.Rows[0]["FIRST_INDEX_Y"].ToString()) != iFirstY
                        )
                    {
                        butUpdate.Enabled = true;
                        m_isModified = true;
                    }
                    else
                    {
                        butUpdate.Enabled = false;
                    }
                    butCreate.Enabled = false;
                    butDelete.Enabled = true;
                }
                else
                {
                    butCreate.Enabled = true;
                    butUpdate.Enabled = false;
                    butDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butCreate_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAsMap(txtMapID.Text);
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateMap(m_strCurrentMapID);
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteMap(m_strCurrentMapID);
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        private void lstMapID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstMapID.Enabled == false) return;

                if (m_isModified)
                {
                    DialogResult dr = MessageBox.Show("This map has been modified. Do you want to save?", "Modified Map", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (dr == System.Windows.Forms.DialogResult.Cancel)
                    {
                        lstMapID.Text = m_strCurrentMapID;
                        return;
                    }
                    else if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        UpdateMap(m_strCurrentMapID);
                    }
                    // dr == System.Windows.Forms.DialogResult.No 인경우 모든 작업 취소하고 새로 선택한 Map Loading
                }

                m_strCurrentMapID = lstMapID.Text;
                LoadMap(m_strCurrentMapID);
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        /// <summary>
        /// Change Event 시 Button 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmbWaferSize_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (cmbWaferSize.Text.Replace("\"", ""))
            {
                case "3":
                    m_wMap.WaferSize = 75.0d;
                    break;
                case "4":
                    m_wMap.WaferSize = 100.0d;
                    break;
                case "5":
                    m_wMap.WaferSize = 125.0d;
                    break;
                case "6":
                    m_wMap.WaferSize = 150.0d;
                    break;
                case "8":
                    m_wMap.WaferSize = 200.0d;
                    break;
                case "12":
                    m_wMap.WaferSize = 200000.0d;
                    break;
            }

            if (pnlRight.Enabled == false) return;
            try
            {
                ButtonCondition();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmbAngel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnlRight.Enabled == false) return;
            try
            {
                ButtonCondition();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void rdoNotch_CheckedChanged(object sender, System.EventArgs e)
        {
            m_wMap.NotchType = rdoNotch.Checked ? DACrux.Base.Notch.Notch : DACrux.Base.Notch.Flat;
            m_wMap.Redraw();
            if (pnlRight.Enabled == false) return;
            try
            {
                ButtonCondition();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void WaferPropertyTextChanged(object sender, EventArgs e)
        {
            if (pnlRight.Enabled == false) return;
            try
            {
                ButtonCondition();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void NumValueChanged(object sender, EventArgs e)
        {
            if (pnlRight.Enabled == false) return;
            try
            {
                ButtonCondition();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmbXYDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            double wsize = m_wMap.WaferSize;
            double wmargin = m_wMap.WaferMargin;
            int MinX = -1;
            int MinY = -1;
            int MaxX = -1;
            int MaxY = -1;

            double dDieSizeX = double.NaN;
            double dDieSizeY = double.NaN;

            try
            {
                ButtonCondition();
                if (int.TryParse(txtMinX.Text, out MinX) == false ||
                    int.TryParse(txtMinY.Text, out MinY) == false ||
                    int.TryParse(txtMaxX.Text, out MaxX) == false ||
                    int.TryParse(txtMaxY.Text, out MaxY) == false)
                    return;

                if (double.TryParse(txtDieX.Text, out dDieSizeX) == false ||
                    double.TryParse(txtDieY.Text, out dDieSizeY) == false)
                    return;

                int xcnt = MaxX - MinX + 1;
                int ycnt = MaxY - MinY + 1;

                if (cmbXYDirection.Text.IndexOf("Left") > -1)
                {
                    txtOriginX.Text = (-dDieSizeX * 0.5 * (xcnt % 2)).ToString();
                }
                else
                {
                    txtOriginX.Text = (dDieSizeX * 0.5 * (xcnt % 2)).ToString();
                }


                if (cmbXYDirection.Text.IndexOf("Bottom") > -1)
                {
                    txtOriginY.Text = (-dDieSizeY * 0.5 * (ycnt % 2)).ToString();
                }
                else
                {
                    txtOriginY.Text = (dDieSizeY * 0.5 * (ycnt % 2)).ToString();
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

    }
}
