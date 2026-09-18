using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    public partial class frmShotMapInformation : DACrux.Framework.Base.DACruxUXBasic01
    {
        private string _insertMapID;
        private int _deleteSelectedIndex;

        public frmShotMapInformation()
        {
            InitializeComponent();
        }

        #region [ Method ]

        private void InitProperty()
        {
            grid.PropertyList.Clear();

            DACrux.Framework.PropertyGrid.PropertyItem pi = null;

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "MAPID";
            pi.DisplayName = "Map ID";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "CHIP_SIZE_X";
            pi.DisplayName = "Chip Size X";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "CHIP_SIZE_Y";
            pi.DisplayName = "Chip Size Y";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "ORIGIN_MICRO_X";
            pi.DisplayName = "Origin Micro X";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "ORIGIN_MICRO_Y";
            pi.DisplayName = "Origin Micro Y";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "ORIGIN_INDEX_X";
            pi.DisplayName = "Origin Index X";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "ORIGIN_INDEX_Y";
            pi.DisplayName = "Origin Index Y";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "FIRST_MICRO_X";
            pi.DisplayName = "First Micro X";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "FIRST_MICRO_Y";
            pi.DisplayName = "First Micro Y";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "EDGE_SIZE";
            pi.DisplayName = "Edge Size";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "ANGLE";
            pi.DisplayName = "ANGLE";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "NETDIE";
            pi.DisplayName = "Net Die";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "DIE_INDEX_MIN_X";
            pi.DisplayName = "Wafer Min X";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "DIE_INDEX_MAX_X";
            pi.DisplayName = "Wafer Max X";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "DIE_INDEX_MIN_Y";
            pi.DisplayName = "Wafer Min Y";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "DIE_INDEX_MAX_Y";
            pi.DisplayName = "Wafer Max Y";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "NETSHOT";
            pi.DisplayName = "Net Shot";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "ST_XCNT";
            pi.DisplayName = "Shot X Count";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "ST_YCNT";
            pi.DisplayName = "Shot Y Count";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "ST_START_X";
            pi.DisplayName = "Shot Start X Index";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "ST_START_Y";
            pi.DisplayName = "Shot Start Y Index";
            grid.PropertyList.Add(pi);

            pi = new Framework.PropertyGrid.PropertyItem();
            pi.Category = "[01] General";
            pi.ColumnName = "DELETE_FLAG";
            pi.DisplayName = "Delete Flag";
            grid.PropertyList.Add(pi);

        }

        private void SetWaferInfo(string strMapID)
        {
            DataTable dtMap = null;

            map.DieClear();
            map.WaferDrawMode = DACrux.Map.MapMode.Fit;

            RO.ProbeAdmin obj = new RO.ProbeAdmin();
            DataTable dt = obj.GetMapDef(strMapID);

            grid.DataSource = null;
            grid.DataSource = dt;

            if (dt == null || dt.Rows.Count == 0)
                return;

            DataRow row = dt.Rows[0];

            map.WaferSize = DACrux.Base.Convert.doubleParse(row["WAFER_SIZE"].ToString());
            map.NotchType = DACrux.Base.Notch.Notch;
            map.AngleOffSet = 0;
            map.XYDirect = DACrux.Base.XYDirection.LeftBottom;

            // 사용자 등록 MAP 과 I/F MAP 파일 구분하는 기준은 DELETE_FLAG 값에 따라 구분된다.
            if (row["DELETE_FLAG"].ToString() == "N")
            {
                map.VisibleShot = true;
                map.NotchAngle = DACrux.Base.Convert.intParse(row["ANGLE"].ToString());

                map.DieSizeX = DACrux.Base.Convert.doubleParse(row["CHIP_SIZE_X"].ToString());
                map.DieSizeY = DACrux.Base.Convert.doubleParse(row["CHIP_SIZE_Y"].ToString());

                map.OriginIndexX = DACrux.Base.Convert.intParse(row["ORIGIN_INDEX_X"].ToString());
                map.OriginIndexY = DACrux.Base.Convert.intParse(row["ORIGIN_INDEX_Y"].ToString());

                map.OriginX = DACrux.Base.Convert.doubleParse(row["ORIGIN_MICRO_X"].ToString());
                map.OriginY = DACrux.Base.Convert.doubleParse(row["ORIGIN_MICRO_Y"].ToString());

                map.ShotArrayX = DACrux.Base.Convert.intParse(row["ST_XCNT"].ToString());
                map.ShotArrayY = DACrux.Base.Convert.intParse(row["ST_YCNT"].ToString());

                map.ShotStartX = DACrux.Base.Convert.intParse(row["ST_START_X"].ToString());
                map.ShotStartY = DACrux.Base.Convert.intParse(row["ST_START_Y"].ToString());

                //m_wMap.DieCalculation(true);

                DACrux.TEST.RO.ProbeMapAnalysis oMap = new DACrux.TEST.RO.ProbeMapAnalysis();

                dtMap = oMap.SelectWaferShotMap(strMapID);
                if (dtMap != null && dtMap.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMap.Rows)
                    {
                        int x = DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString());
                        int y = DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString());
                        map.AddDie(new DACrux.Base.Die(x, y, 1, 1));
                    }
                }
            }
            else // 사용자가 등록한 MAP 인 경우
            {
                map.VisibleShot = false;
                map.NotchAngle = 0;

                map.DieSizeX = DACrux.Base.Convert.doubleParse(row["CHIP_SIZE_X"].ToString());
                map.DieSizeY = DACrux.Base.Convert.doubleParse(row["CHIP_SIZE_Y"].ToString());

                map.OriginX = DACrux.Base.Convert.doubleParse(row["ORIGIN_MICRO_X"].ToString());
                map.OriginY = DACrux.Base.Convert.doubleParse(row["ORIGIN_MICRO_Y"].ToString());

                map.OriginIndexX = DACrux.Base.Convert.intParse(row["ORIGIN_INDEX_X"].ToString());
                map.OriginIndexY = DACrux.Base.Convert.intParse(row["ORIGIN_INDEX_Y"].ToString());

                map.DieMinX = DACrux.Base.Convert.intParse(row["DIE_INDEX_MIN_X"].ToString());
                map.DieMinY = DACrux.Base.Convert.intParse(row["DIE_INDEX_MIN_Y"].ToString());

                map.DieMaxX = DACrux.Base.Convert.intParse(row["DIE_INDEX_MAX_X"].ToString());
                map.DieMaxY = DACrux.Base.Convert.intParse(row["DIE_INDEX_MAX_Y"].ToString());

                DACrux.Base.WaferDieCalculator calc = new Base.WaferDieCalculator();
                calc.WaferSize = map.WaferSize;
                calc.DiePitchX = map.DieSizeX;
                calc.DiePitchY = map.DieSizeY;
                calc.OriginX = map.OriginX;
                calc.OriginY = map.OriginY;
                calc.WaferMargin = DACrux.Base.Convert.doubleParse(row["EDGE_SIZE"].ToString()) * 1000;
                calc.Calculate();

                foreach (Point pt in calc.GetDiePointArray())
                    map.AddDie(new Base.Die(pt.X, pt.Y, 1, 1));
            }

            SetMapConfig();

            map.CenterMark = chkCenterLine.Checked;
            map.VisibleShotAlignPoint = chkScopePoint.Checked;

            map.WaferDrawMode = DACrux.Map.MapMode.Fit;
        }

        private void BindingMap(DACrux.Base.WaferDieCalculator calc)
        {
            map.DieClear();
            map.VisibleShot = false;
            map.WaferDrawMode = DACrux.Map.MapMode.Fit;

            map.NotchAngle = 0;

            map.DieSizeX = calc.DiePitchX;
            map.DieSizeY = calc.DiePitchY;

            map.OriginX = calc.OriginX;
            map.OriginY = calc.OriginY;

            map.OriginIndexX = calc.OriginIndexX;
            map.OriginIndexY = calc.OriginIndexY;

            map.DieMinX = calc.IndexXMin;
            map.DieMinY = calc.IndexYMin;

            map.DieMaxX = calc.IndexXMax;
            map.DieMaxY = calc.IndexYMax;
            
            foreach (Point pt in calc.GetDiePointArray())
                map.AddDie(new Base.Die(pt.X, pt.Y, 1, 1));

            map.CenterMark = chkCenterLine.Checked;
            map.VisibleShotAlignPoint = chkScopePoint.Checked;

            map.WaferDrawMode = DACrux.Map.MapMode.Fit;
        }

        private void BindingMapList()
        {
            DACrux.TEST.RO.ProbeAdmin oProbe = new DACrux.TEST.RO.ProbeAdmin();
            DataTable dt = oProbe.GetMapIDListAll();

            if (dt != null && dt.Rows.Count > 0)
            {
                dlbDeviceList.DisplayMember = "DISP_MAPID";
                dlbDeviceList.ValueMember = "MAPID";
                dlbDeviceList.DataSource = dt;
            }
        }

        private double? GetDouble(object obj, double? defaultValue = null)
        {
            if (obj == null)
                return defaultValue;

            double val;

            if (!double.TryParse(obj.ToString(), out val))
                return defaultValue;

            return val;
        }

        private void SetMapConfig()
        {
            DACrux.Common.RO.ComConfiguration oComConfig = new DACrux.Common.RO.ComConfiguration();

            //=================================================================================================================================
            //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
            //=================================================================================================================================

            //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
            DataTable dtMapOption = oComConfig.SelectDefectMapConfig(DACrux.Base.GlobalVariable.Factory, DACrux.Base.GlobalVariable.UserID);
            if (dtMapOption != null && dtMapOption.Rows.Count > 0)
            {
                foreach (DataRow dr in dtMapOption.Rows)
                {
                    string strType = dr["NAME"].ToString();
                    Color crType = ColorTranslator.FromHtml(dr["VALUE"].ToString());

                    switch (strType)
                    {
                        //Wafer Base Color
                        case "WAFER_TEST_MAP_BG":
                            map.WaferColor = crType;
                            break;
                        //Wafer Border Line Color
                        case "WAFER_TEST_MAP_LINE":
                            map.DieBorderColor = crType;
                            break;

                    }
                }
            }
        }
        #endregion [ Method ]

        #region [ Event Handler ]

        private void frmShotMapInformation_Load(object sender, EventArgs e)
        {
            BindingMapList();
        }

        private void dlbDeviceList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                object[] arr = dlbDeviceList.SelectedValues;

                if (arr.Length > 0)
                {
                    map.VisibleShotAlignPoint = false;
                    MainForm.SetStatusMessage("Map 을 조회 합니다.");
                    SetWaferInfo(arr[0].ToString());
                }
            }
            finally
            {
                MainForm.SetStatusMessage(null);
            }
        }

        private void m_wMap_OnChangeCurrentDie(object sender, Base.Die NewDie)
        {
            txtXIndex.Text = NewDie.IndexX.ToString();
            txtYIndex.Text = NewDie.IndexY.ToString();
        }

        private void chkCenterLine_CheckedChanged(object sender, EventArgs e)
        {
            map.CenterMark = chkCenterLine.Checked;
            map.Redraw();
        }

        private void chkScopePoint_CheckedChanged(object sender, EventArgs e)
        {
            map.VisibleShotAlignPoint = chkScopePoint.Checked;
        }

        private void gridProperty_PropertyDataChanged(object sender, EventArgs e)
        {
            bool userReg = (grid.GetValue("DELETE_FLAG").ToString() == "Y");
            grid.EnableEditButton = userReg;
            grid.EnableDeleteButton = userReg;
        }

        private void gridProperty_CommandButtonClick(object sender, Framework.PropertyGrid.CommandEventArgs e)
        {
            string mapID = grid.GetValue("MAPID") as string;
            RO.ProbeAdmin obj = new RO.ProbeAdmin();
            bool exists = obj.ExistsMapID(GlobalVariable.Factory, mapID);

            if (e.Mode == Framework.PropertyGrid.CommandMode.Insert)
            {
                if (exists)
                {
                    MessageBox.Show("해당 데이터가 이미 존재합니다.");
                    e.Cancel = true;
                    return;
                }

                DialogResult result = MessageBox.Show("저장하시겠습니까?", "Insert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    obj.InsertMapData(grid.GetDictionaryValue(), GlobalVariable.UserID);
                    _insertMapID = mapID;
                }
                else
                    e.Cancel = true;
            }
            else if (e.Mode == Framework.PropertyGrid.CommandMode.Update)
            {
                if (!exists)
                {
                    MessageBox.Show("업데이트할 데이터가 없습니다.");
                    e.Cancel = true;
                    return;
                }

                DialogResult result = MessageBox.Show("업데이트 하시겠습니까?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == System.Windows.Forms.DialogResult.OK)
                    obj.UpdateMapData(grid.GetDictionaryValue(), GlobalVariable.UserID);
                else
                    e.Cancel = true;
            }
            else if (e.Mode == Framework.PropertyGrid.CommandMode.Delete)
            {
                if (!exists)
                {
                    MessageBox.Show("삭제할 데이터가 없습니다.");
                    e.Cancel = true;
                    return;
                }

                DialogResult result = MessageBox.Show("삭제하시겠습니까?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    obj.DeleteMapData(GlobalVariable.Factory, mapID);
                    _deleteSelectedIndex = dlbDeviceList.SelectedIndex;
                }
                else
                    e.Cancel = true;
            }
        }

        private void grid_ValueChanged(object sender, Framework.PropertyGrid.ValueChangedEventArgs e)
        {
            string[] checkFields = new string[] { "CHIP_SIZE_X", "CHIP_SIZE_Y", "ORIGIN_MICRO_X", "ORIGIN_MICRO_Y", "EDGE_SIZE" };

            if (checkFields.Contains(e.Item.ColumnName))
            {
                double? wafersize = GetDouble(grid.GetValue("WAFER_SIZE"));
                double? diePitchX = GetDouble(grid.GetValue("CHIP_SIZE_X"));
                double? diePitchY = GetDouble(grid.GetValue("CHIP_SIZE_Y"));
                double? originX = GetDouble(grid.GetValue("ORIGIN_MICRO_X"), 0);
                double? originY = GetDouble(grid.GetValue("ORIGIN_MICRO_Y"), 0);
                double? edgeSize = GetDouble(grid.GetValue("EDGE_SIZE"), 0);

                if (diePitchX == null || diePitchY == null || originX == null || originY == null)
                    return;

                WaferDieCalculator calc = new WaferDieCalculator();
                calc.WaferSize = wafersize.Value;
                calc.DiePitchX = diePitchX.Value;
                calc.DiePitchY = diePitchY.Value;
                calc.OriginX = originX.Value;
                calc.OriginY = originY.Value;
                calc.WaferMargin = edgeSize.Value * 1000;
                calc.Calculate();

                grid.SetValue("DIE_INDEX_MIN_X", calc.IndexXMin);
                grid.SetValue("DIE_INDEX_MIN_Y", calc.IndexYMin);
                grid.SetValue("DIE_INDEX_MAX_X", calc.IndexXMax);
                grid.SetValue("DIE_INDEX_MAX_Y", calc.IndexYMax);
                grid.SetValue("ORIGIN_INDEX_X", calc.OriginIndexX);
                grid.SetValue("ORIGIN_INDEX_Y", calc.OriginIndexY);

                BindingMap(calc);
            }
        }

        private void gridProperty_CommandComplete(object sender, Framework.PropertyGrid.CommandEventArgs e)
        {
            // INSERT 시 목록 갱신 후 추가된 DEVICE 선택 되도록 한다.
            if (e.Mode == Framework.PropertyGrid.CommandMode.Insert)
            {
                BindingMapList();
                dlbDeviceList.SelectedValue = _insertMapID;
            } // DELETE 시 목록 갱신 후 이전 선택한 인덱스 순서가 되도록 한다.
            else if (e.Mode == Framework.PropertyGrid.CommandMode.Delete)
            {
                BindingMapList();
                dlbDeviceList.SelectedIndex = Math.Min(_deleteSelectedIndex, dlbDeviceList.Items.Count - 1);
            }
        }

        #endregion [ Event Handler ]
    }
}

