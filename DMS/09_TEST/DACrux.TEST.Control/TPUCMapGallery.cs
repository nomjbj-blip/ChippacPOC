using System;
using DACrux.Framework.Base;
using System.Windows.Forms;
using System.Data;
using DACrux.Base;
using DACrux.TEST.RO;
using System.Diagnostics;
using System.Collections.Generic;
using DACrux.Utility;
using System.Drawing;
using DACrux.Common.RO;

namespace DACrux.MapAnalysis.Control
{
    /// <summary>
    /// TPUCMapGallery 에 대한 요약 설명입니다.
    /// </summary>
    public partial class TPUCMapGallery
        : DACruxCTLBasic01, IExportExcel
    {
        #region [ Data Field ]
        private enum ColumnIndex { WAFER_ID = 0, WAFER_SEQ, DIE_NUM, DIEPROBE_CNT, X, Y, BIN }
        private enum LimitType { SPECIFICATION = 0, CONTROL, TIGHTEN }

        public delegate void ParaAnalysis(object oWaferMap, string strPara);
        public event ParaAnalysis OnParaAnalysis;

        private int nDisplayFaltAngle = -1;
        private double gradationMaxValue = double.NaN;
        private double gradationMinValue = double.NaN;

        private string SelectedBin = string.Empty;
        private DataTable dtInfo = null;
        private DataTable dtInfoEnable = null;
        private DataTable dtMapOption = null;
        private readonly string DIENUM = "DIE_NUM";

        #endregion [ Data Field]

        #region [ Create & Close ]

        public TPUCMapGallery()
        {
            InitializeComponent();
        }

        #endregion

        #region [ Event Handler ]

        private void TPUCMapGallery_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;
        }

        private void btnParaAnalysis_Click(
            object sender,
            EventArgs e
            )
        {
            if (lsParaList.Items.Count <= 0 || lsParaList.SelectedItems.Count <= 0)
                return;

            if ((DataSource as DataSet) != null)
            {
                if (OnParaAnalysis != null)
                    OnParaAnalysis((DataSource as DataSet), lsParaList.SelectedItems[0].ToString());
            }
        }

        private void btnTestDataDraw_Click(
            object sender,
            EventArgs e
            )
        {
            if (lsParaList.SelectedItem == null)
                return;

            string selectedParameter = lsParaList.SelectedItem.ToString();
            long[] wafers = new long[WaferInfo.Length];
            for (int idx = 0; idx < WaferInfo.Length; idx++)
            {
                wafers[idx] = Base.Convert.longParse(WaferInfo[idx].WaferSeq);
            }

            ProbeMapAnalysis obj = new ProbeMapAnalysis();
            DataTable dt = obj.SelectWaferParaItem(wafers, selectedParameter, (int)numericUpDown1.Value);
            (DataSource as DataSet).Tables.Remove("MAPDATA");
            (DataSource as DataSet).AcceptChanges();

            (DataSource as DataSet).Tables.Add(dt);
            (DataSource as DataSet).AcceptChanges();

            FillData(dt);

            if (GetCutValue().Contains("ALL"))
            {
                SelectedBin = "ALL";
            }
            else
            {
                SelectedBin = GetCutValue();
            }

            DrawWafer(
                selectedParameter
                );

        }

        private void chkControlLimit_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            if (chkControlLimit.Checked)
                chkTightenLimit.Checked = chkSpecificationLimit.Checked = !chkControlLimit.Checked;

            if (lsParaList.SelectedItem == null)
                return;

            double dUCL = double.NaN;
            double dLCL = double.NaN;
            string selectedParameter = lsParaList.SelectedItem.ToString();

            if (!double.TryParse(txtUCL.Text, out dUCL))
                dUCL = double.NaN;

            if (!double.TryParse(txtLCL.Text, out dLCL))
                dLCL = double.NaN;

            if (chkControlLimit.Checked)
            {
                DrawWaferLimit(
                    selectedParameter,
                    dUCL,
                    dLCL
                    );
            }
            else
            {
                DrawWafer(
                    selectedParameter
                    );
            }
        }

        private void chkSpecificationLimit_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            if (chkSpecificationLimit.Checked)
                chkTightenLimit.Checked = chkControlLimit.Checked = !chkSpecificationLimit.Checked;


            if (lsParaList.SelectedItem == null)
                return;

            double dUSL = double.NaN;
            double dLSL = double.NaN;
            string selectedParameter = lsParaList.SelectedItem.ToString();

            if (!double.TryParse(txtUSL.Text, out dUSL))
                dUSL = double.NaN;

            if (!double.TryParse(txtLSL.Text, out dLSL))
                dLSL = double.NaN;

            if (chkSpecificationLimit.Checked)
            {
                DrawWaferLimit(
                    selectedParameter,
                    dUSL,
                    dLSL
                    );
            }
            else
            {
                DrawWafer(
                    selectedParameter
                    );
            }
        }

        private void chkToleranceLimit_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            if (chkTightenLimit.Checked)
                chkSpecificationLimit.Checked = chkControlLimit.Checked = !chkTightenLimit.Checked;


            if (lsParaList.SelectedItem == null)
                return;

            double dUtl = double.NaN;
            double dLtl = double.NaN;
            string selectedParameter = lsParaList.SelectedItem.ToString();

            if (!double.TryParse(txtUTL.Text, out dUtl))
                dUtl = double.NaN;

            if (!double.TryParse(txtLTL.Text, out dLtl))
                dLtl = double.NaN;

            if (chkTightenLimit.Checked)
            {
                DrawWaferLimit(
                    selectedParameter,
                    dUtl,
                    dLtl
                    );
            }
            else
            {
                DrawWafer(
                    selectedParameter
                    );
            }
        }


        private void clbParaCut_ItemCheck(
            object sender,
            ItemCheckEventArgs e
            )
        {
            if (e.Index == 0)
            {
                for (int i = 1; i < clbParaCut.Items.Count; i++)
                    clbParaCut.SetItemChecked(i, false);
            }
            else
            {
                clbParaCut.SetItemChecked(0, false);
            }
        }

        private void chkTotalPara_CheckedChanged(
            object sender,
            EventArgs e
            )
        {
            SetParameterBinding();
        }

        private void lsParaList_SelectedIndexChanged(
            object sender,
            EventArgs e
            )
        {
            SelectedParameter(sender);
            lsParaList.Focus();
        }

        private void lsParaList_KeyDown(
            object sender,
            KeyEventArgs e
            )
        {
            if (e.KeyCode == Keys.Up && ((sender as ListBox).SelectedIndex - 1) > -1)
            {
                (sender as ListBox).SelectedIndex--;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Down && ((sender as ListBox).SelectedIndex + 1) < (sender as ListBox).Items.Count)
            {
                (sender as ListBox).SelectedIndex++;
                e.Handled = true;
            }
        }

        private void mapContainer_Resize(
            object sender,
            EventArgs e
            )
        {
            if (mapContainer.Controls.Count <= 0)
                return;

            DrawResize();
        }

        private void numericUpDown1_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            btnTestDataDraw.PerformClick();
        }

        private void numericUpDown2_ValueChanged(
            object sender,
            EventArgs e
            )
        {
            DrawResize();
        }

        private void txtItemFilter_TextChanged(
            object sender,
            EventArgs e
            )
        {
            SetParameterBinding();
        }

        #endregion [ Event Handler ]

        #region [ Method ]

        private void ControlAdd(
            TPUCMapControl container
            )
        {
            if (mapContainer.InvokeRequired)
            {
                mapContainer.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        ControlAdd(container);
                    }));
            }
            else
            {
                if (container == null)
                    return;

                container.Height = container.Width = GetWidth();
                mapContainer.Controls.Add(container);
                mapContainer.ResumeLayout();
            }
        }

        public void Draw(
            int nAngle = -1
            )
        {
            ProbeMapAnalysis obj = new ProbeMapAnalysis();
            chkControlLimit.Checked = chkSpecificationLimit.Checked = chkTightenLimit.Checked = false;
            GetConfiguration();

            long[] wafers = new long[WaferInfo.Length];
            for (int idx = 0; idx < WaferInfo.Length; idx++)
            {
                wafers[idx] = Base.Convert.longParse(WaferInfo[idx].WaferSeq);
            }

            /// 컬럼 count 설정
            if (WaferInfo.Length == 1)
                numericUpDown2.Value = 1;
            else
                numericUpDown2.Value = 2;

            SelectedBin = "ALL";
            DataSource = obj.SelectWaferMapDrawData(wafers);
            if ((DataSource as DataSet) == null)
                return;

            SetParameterBinding();

            FillData(
                (DataSource as DataSet).Tables["MAPDATA"]
                );

            //DrawWafer(false, string.Empty, nAngle);
            mapContainer.Controls.Clear();
            mapContainer.Refresh();

            DataTable dt = null;
            DataRow[] rows = null;
            TPUCMapControl container = null;
            WaferRecipe recipe = new WaferRecipe();
            List<string> infor = null;
            string waferseq = string.Empty;
            string waferid = string.Empty;

            foreach (DataRow row in (DataSource as DataSet).Tables["WAFER_INFO"].Rows)
            {
                waferseq = row["WAFER_SEQ"].ToString();
                waferid = row["WAFER_ID"].ToString();
                container = new TPUCMapControl(waferid, waferseq);
                infor = new List<string>();
#if DEBUG
                Debug.WriteLine(String.Format("WAFER_SEQ = '{0}'", row["WAFER_SEQ"]));
#endif
                infor.Add(String.Format("WAFER ID: {0}", waferid));

                rows = (DataSource as DataSet).Tables["MAPDATA"].Select(String.Format("[WAFER_SEQ] = '{0}'", row["WAFER_SEQ"]));
                if (rows == null || rows.Length <= 0)
                    continue;

                dt = rows.CopyToDataTable<DataRow>();
                if (MakeWaferRecipe(dt, (DataSource as DataSet).Tables["RECIPE"].Rows[0], ref recipe, nAngle))
                {
                    container.WaferMap.DrawGradationDie = false;
                    container.WaferMap.FromGradationDieColor = Color.Lime;
                    container.WaferMap.ToGradationDieColor = Color.Red;
                    container.WaferMap.GradationInterval = Base.Convert.intParse(cbCutCnt.Text);
                    container.WaferMap.ParametricColumn = string.Empty; ;
                    container.WaferMap.GradationMaxValue = gradationMaxValue;
                    container.WaferMap.GradationMinValue = gradationMinValue;
                    container.WaferMap.DisplayValue = "PCMVALUE";
                    container.WaferMap.DisplayDieValue = DieDisplayValue.PCMValue;
                    container.WaferMap.VisibleDieValue = false;
                    container.WaferMap.SelecetedBin = SelectedBin;

                    container.WaferMap.ResetSelectedDie();
                    container.WaferMap.DieClear();

                    container.WaferMap.SetWaferRecipe(recipe);
                    container.WaferMap.DataSource = dt;

                    container.WaferMap.VisibleShot = false;
                    container.WaferMap.ViewAngle = (360 - (recipe.ANGLE - (recipe.ANGLE * 90))) % 360;

                    //=================================================================================================================================
                    //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                    //=================================================================================================================================
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
                                    container.WaferMap.WaferColor = crType;
                                    break;
                                //Wafer Border Line Color
                                case "WAFER_TEST_MAP_LINE":
                                    container.WaferMap.DieBorderColor = crType;
                                    break;
                            }
                        }
                    }

                    //--

                    // TQC_CONFIG_USER에 설정되어 있는 정보 Display
                    List<String> sInfo = new List<String>();
                    if (dtInfo != null && dtInfo.Rows.Count > 0)
                    {
                        foreach (DataRow drInfo in dtInfo.Rows)
                        {
                            if ((DataSource as DataSet).Tables["WAFER_INFO"].Columns.IndexOf(drInfo["NAME"].ToString()) > -1)
                                sInfo.Add(String.Format("{0}: {1}", drInfo["VALUE"], row[drInfo["NAME"].ToString()]));
                        }
                    }
                    else
                    {
                        // Default 
                        sInfo.Add(String.Format("{0}: {1}", "WAFER ID", row["WAFER_ID"]));
                    }

                    container.WaferMap.SetInfomation(sInfo.ToArray());

                    if (dtInfoEnable != null && dtInfoEnable.Rows.Count > 0)
                    {
                        if (dtInfoEnable.Rows[0]["VALUE"].ToString() == "N")
                            container.WaferMap.SetInfomation(null);
                    }

                    container.DrawWafer();

                    ControlAdd(container);
                }
            }
            mapContainer.Refresh();
        }

        private void DrawResize(
            )
        {
            TPUCMapControl container = null;
            for (int idx = 0; idx < mapContainer.Controls.Count; idx++)
            {
                container = mapContainer.Controls[idx] as TPUCMapControl;
                if (container == null)
                    continue;

                container.Height = GetWidth();
                container.Width = GetWidth();
            }
        }

        private void DrawWafer(
            string selectedParameter
            )
        {
            TPUCMapControl container = null;
            DataTable dt = null;
            WaferRecipe recipe = new WaferRecipe();
            List<string> infor = null;

            for (int idx = mapContainer.Controls.Count - 1; idx >= 0; idx--)
            {
                container = mapContainer.Controls[idx] as TPUCMapControl;
#if DEBUG
                Debug.WriteLine(String.Format("WAFER SEQ: {0}", container.WaferSeq));
#endif
                DataRow[] rows = (DataSource as DataSet).Tables["MAPDATA"].Select(String.Format("[WAFER_SEQ] = '{0}'", container.WaferSeq));
                if (rows.Length <= 0)
                    continue;

                dt = rows.CopyToDataTable<DataRow>();

                infor = new List<string>();
                infor.Add(String.Format("WAFER ID: {0}", container.WaferID));

                if (MakeWaferRecipe(dt, (DataSource as DataSet).Tables["RECIPE"].Rows[0], ref recipe, -1))
                {
                    container.WaferMap.DrawGradationDie = true;
                    container.WaferMap.FromGradationDieColor = Color.Lime;
                    container.WaferMap.ToGradationDieColor = Color.Red;
                    container.WaferMap.GradationInterval = Base.Convert.intParse(cbCutCnt.Text);
                    container.WaferMap.ParametricColumn = selectedParameter;
                    container.WaferMap.GradationMaxValue = gradationMaxValue;
                    container.WaferMap.GradationMinValue = gradationMinValue;
                    container.WaferMap.DisplayValue = "PCMVALUE";
                    container.WaferMap.DisplayDieValue = DieDisplayValue.PCMValue;
                    container.WaferMap.VisibleDieValue = true;
                    container.WaferMap.ParaLimit = false;
                    container.WaferMap.SelecetedBin = SelectedBin;

                    container.WaferMap.ResetSelectedDie();
                    container.WaferMap.DieClear();

                    container.WaferMap.SetWaferRecipe(recipe);
                    container.WaferMap.DataSource = dt;

                    container.WaferMap.VisibleShot = false;
                    container.WaferMap.ViewAngle = (360 - (recipe.ANGLE - (recipe.ANGLE * 90))) % 360;
                    container.DrawWafer();
                }
            }
        }

        private void DrawWaferLimit(
            string selectedParameter,
            double upperValue,
            double lowerValue
            )
        {
            TPUCMapControl container = null;
            DataTable dt = null;
            WaferRecipe recipe = new WaferRecipe();
            List<string> infor = null;

            for (int idx = mapContainer.Controls.Count - 1; idx >= 0; idx--)
            {
                container = mapContainer.Controls[idx] as TPUCMapControl;
#if DEBUG
                Debug.WriteLine(String.Format("WAFER SEQ: {0}", container.WaferSeq));
#endif
                DataRow[] rows = (DataSource as DataSet).Tables["MAPDATA"].Select(String.Format("[WAFER_SEQ] = '{0}'", container.WaferSeq));
                if (rows.Length <= 0)
                    continue;

                dt = rows.CopyToDataTable<DataRow>();

                infor = new List<string>();
                infor.Add(String.Format("WAFER ID: {0}", container.WaferID));

                if (MakeWaferRecipe(dt, (DataSource as DataSet).Tables["RECIPE"].Rows[0], ref recipe, -1))
                {
                    container.WaferMap.DrawGradationDie = true;
                    container.WaferMap.FromGradationDieColor = Color.Lime;
                    container.WaferMap.ToGradationDieColor = Color.Red;
                    container.WaferMap.GradationInterval = 2;
                    container.WaferMap.ParametricColumn = selectedParameter;
                    container.WaferMap.GradationMaxValue = upperValue;
                    container.WaferMap.GradationMinValue = lowerValue;
                    container.WaferMap.DisplayValue = "PCMVALUE";
                    container.WaferMap.DisplayDieValue = DieDisplayValue.PCMValue;
                    container.WaferMap.VisibleDieValue = true;
                    container.WaferMap.ParaLimit = true;
                    container.WaferMap.SelecetedBin = SelectedBin;

                    container.WaferMap.ResetSelectedDie();
                    container.WaferMap.DieClear();

                    container.WaferMap.SetWaferRecipe(recipe);
                    container.WaferMap.DataSource = dt;

                    container.WaferMap.VisibleShot = false;
                    container.WaferMap.ViewAngle = (360 - (recipe.ANGLE - (recipe.ANGLE * 90))) % 360;
                    container.DrawWafer();
                }
            }
        }

        public void ExportExcel(
            )
        {
            Utility.ExcelSheet sheet1 = new Utility.ExcelSheet();
            for (int idx = 0; idx < mapContainer.Controls.Count; idx++)
            {
                TPUCMapControl container = mapContainer.Controls[idx] as TPUCMapControl;
                sheet1.Add(container.WaferMap);
            }

            Utility.ExcelSheet sheet2 = new Utility.ExcelSheet();
            sheet2.Add(fpRawData);

            Utility.ExcelExportArgs e = new Utility.ExcelExportArgs();
            e.SheetList.Add(sheet1);
            e.SheetList.Add(sheet2);

            ExcelExportManager.Export(e);
        }

        private void FillData(
            DataTable dt
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpRawData);
            Utility.FPSpreadUtil.SetSpreadData(dt, fpRawData_Sheet1, 120, (int)numericUpDown1.Value);
            Utility.FPSpreadUtil.VisibleSpreadColumns(fpRawData_Sheet1, new int[] { (int)ColumnIndex.WAFER_SEQ }, false);
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpRawData_Sheet1);
        }

        /// <summary>
        /// DB에 설정되어 있는 Configuration 에 대한 정보를 가져온다.
        /// </summary>
        private void GetConfiguration(
            )
        {
            ComConfiguration obj = new ComConfiguration();
            dtInfo = obj.GetConfigurationUser(
                DACrux.Base.GlobalVariable.Factory,
                "TEST_OPTION_M",
                DACrux.Base.GlobalVariable.UserID
                );

            //Wafer Information 사용 여부
            dtInfoEnable = obj.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "WAFER_OPTION_M_ENABLE", DACrux.Base.GlobalVariable.UserID);

            dtMapOption = obj.SelectDefectMapConfig(
                DACrux.Base.GlobalVariable.Factory,
                DACrux.Base.GlobalVariable.UserID
                );
        }

        private int GetWidth()
        {
            return (mapContainer.Width - SystemInformation.VerticalScrollBarWidth) / (int)numericUpDown2.Value - mapContainer.Margin.Left * 2;
        }

        private string GetCutValue(
            )
        {
            string rValue = string.Empty;

            for (int idx = 0; idx < clbParaCut.Items.Count; idx++)
            {
                if (clbParaCut.GetItemChecked(idx))
                {
                    if (idx == 0)
                    {
                        rValue += "," + "ALL";
                    }
                    else
                    {
                        rValue += "," + (DACrux.Base.Convert.intParse(cbCutCnt.Text) - idx).ToString();
                    }
                }
            }

            return rValue;
        }

        private bool MakeWaferRecipe(
            DataTable dtMapData,
            DataRow drWaferInfo,
            ref WaferRecipe recipe,
            int nAngle
            )
        {
            int nRotationAngle = -1;
            double dMargin = 0.95d;

            if (int.TryParse(drWaferInfo["FIRST_INDEX_X"].ToString(), out recipe.FIRST_DIE_X) == false)
                recipe.FIRST_DIE_X = DACrux.Base.Convert.intParse(dtMapData.Compute("MIN([X])", "1=1").ToString());

            if (int.TryParse(drWaferInfo["FIRST_INDEX_Y"].ToString(), out recipe.FIRST_DIE_Y) == false)
                recipe.FIRST_DIE_Y = DACrux.Base.Convert.intParse(dtMapData.Compute("MIN([Y])", "1=1").ToString());

            if (int.TryParse(drWaferInfo["DIE_INDEX_MIN_X"].ToString(), out recipe.DIE_INDEX_MIN_X) == false)
                recipe.DIE_INDEX_MIN_X = recipe.FIRST_DIE_X;

            if (int.TryParse(drWaferInfo["DIE_INDEX_MIN_Y"].ToString(), out recipe.DIE_INDEX_MIN_Y) == false)
                recipe.DIE_INDEX_MIN_Y = recipe.FIRST_DIE_Y;

            if (int.TryParse(drWaferInfo["DIE_INDEX_MAX_X"].ToString(), out recipe.DIE_INDEX_MAX_X) == false)
                recipe.DIE_INDEX_MAX_X = DACrux.Base.Convert.intParse(dtMapData.Compute("MAX([X])", "1=1").ToString());

            if (int.TryParse(drWaferInfo["DIE_INDEX_MAX_Y"].ToString(), out recipe.DIE_INDEX_MAX_Y) == false)
                recipe.DIE_INDEX_MAX_Y = DACrux.Base.Convert.intParse(dtMapData.Compute("MAX([Y])", "1=1").ToString());

            if (Enum.TryParse(drWaferInfo["NOTCH_TYPE"].ToString(), out recipe.NOTCH_TYPE) == false)
                recipe.NOTCH_TYPE = DACrux.Base.Notch.Flat;

            if (int.TryParse(drWaferInfo["ANGLE"].ToString(), out recipe.ANGLE) == false)
                recipe.ANGLE = 180;

            if (Enum.TryParse(drWaferInfo["XY_DIRECTION"].ToString(), out recipe.XYDIR) == false)
                recipe.XYDIR = DACrux.Base.XYDirection.LeftBottom;

            recipe.WAFER_SIZE = DACrux.Base.Util.GetValue(drWaferInfo["WAFER_SIZE"].ToString(), 200000);

            if (double.TryParse(drWaferInfo["EDGE_SIZE"].ToString(), out recipe.EDGE_SIZE) == false)
                recipe.EDGE_SIZE = 3d;

            if (nAngle != -1)
                recipe.ANGLE = nAngle;

            nDisplayFaltAngle = recipe.ANGLE;

            //화면상에 보여 줄때 Bottom 으로 저장이 되어 있기 때문에 Rotation 각을 보고 Max 값을 치환 해준다.
            nRotationAngle = (360 - (180 - recipe.ANGLE)) % 360;

            if (nRotationAngle == 90 || nRotationAngle == 270)
            {
                int iTempMax = 0;
                iTempMax = recipe.DIE_INDEX_MAX_X;

                recipe.DIE_INDEX_MAX_X = recipe.DIE_INDEX_MAX_Y;
                recipe.DIE_INDEX_MAX_Y = iTempMax;

            }

            if (double.TryParse(drWaferInfo["CHIP_SIZE_X"].ToString(), out recipe.DIE_SIZE_X) == false)
            {
                recipe.DIE_SIZE_X = (recipe.WAFER_SIZE) / (double)(recipe.DIE_INDEX_MAX_X - recipe.DIE_INDEX_MIN_X) * dMargin;
            }

            if (double.TryParse(drWaferInfo["CHIP_SIZE_Y"].ToString(), out recipe.DIE_SIZE_Y) == false)
            {
                recipe.DIE_SIZE_Y = (recipe.WAFER_SIZE) / (double)(recipe.DIE_INDEX_MAX_Y - recipe.DIE_INDEX_MIN_Y) * dMargin;
            }

            if (double.TryParse(drWaferInfo["CHIP_SIZE_Y"].ToString(), out recipe.DIE_SIZE_Y) == false)
            {
                recipe.DIE_SIZE_Y = (recipe.WAFER_SIZE) / (double)(recipe.DIE_INDEX_MAX_Y - recipe.DIE_INDEX_MIN_Y) * dMargin;
            }

            if (double.TryParse(drWaferInfo["CHIP_SIZE_Y"].ToString(), out recipe.DIE_SIZE_Y) == false)
            {
                recipe.DIE_SIZE_Y = (recipe.WAFER_SIZE) / (double)(recipe.DIE_INDEX_MAX_Y - recipe.DIE_INDEX_MIN_Y) * dMargin;
            }

            if (int.TryParse(drWaferInfo["ORIGIN_INDEX_X"].ToString(), out recipe.ORIGIN_DIE_X) == false)
            {
                recipe.ORIGIN_DIE_X = recipe.DIE_INDEX_MIN_X + (int)Math.Floor((double)recipe.XDIES / 2.0d);
            }

            if (int.TryParse(drWaferInfo["ORIGIN_INDEX_Y"].ToString(), out recipe.ORIGIN_DIE_Y) == false)
            {
                recipe.ORIGIN_DIE_Y = recipe.DIE_INDEX_MIN_Y + (int)Math.Floor((double)recipe.YDIES / 2.0d);
            }

            if (double.TryParse(drWaferInfo["ORIGIN_MICRO_X"].ToString(), out recipe.ORIGIN_X) == false)
            {
                if (recipe.XDIES < 20)
                {
                    if (recipe.XDIES % 2 == 0)
                        recipe.ORIGIN_X = 0;
                    else
                        recipe.ORIGIN_X = recipe.DIE_SIZE_X / 2.0d;
                }
                else
                {
                    if (recipe.ANGLE == 90)
                        recipe.ORIGIN_X = recipe.DIE_SIZE_X / 2.0d;
                    else if (recipe.ANGLE == 270)
                        recipe.ORIGIN_X = -recipe.DIE_SIZE_X / 2.0d;
                    else
                        recipe.ORIGIN_X = 0;
                }
            }


            if (double.TryParse(drWaferInfo["ORIGIN_MICRO_Y"].ToString(), out recipe.ORIGIN_Y) == false)
            {
                if (recipe.YDIES < 20)
                {
                    if (recipe.YDIES % 2 == 0)
                        recipe.ORIGIN_Y = 0;
                    else
                        recipe.ORIGIN_Y = recipe.DIE_SIZE_Y / 2.0d;
                }
                else
                {
                    if (recipe.ANGLE == 90 || recipe.ANGLE == 270)
                        recipe.ORIGIN_Y = 0;
                    else if (recipe.ANGLE == 0)
                        recipe.ORIGIN_Y = -recipe.DIE_SIZE_Y / 2.0d;
                    else if (recipe.ANGLE == 180)
                        recipe.ORIGIN_Y = recipe.DIE_SIZE_Y / 2.0d;
                }
            }

            return true;
        }

        private void SelectedParameter(
            object sender
            )
        {
            if ((sender as ListBox).SelectedItem == null)
                return;

            string selectedParameter = (sender as ListBox).SelectedItem.ToString();
            DataRow[] drs = (DataSource as DataSet).Tables["PARA_LIST"].Select(
                String.Format("[PARAM_NAME] = '{0}'", selectedParameter)
                );

            if (!String.Equals(selectedParameter, DIENUM) && (drs == null || drs.Length <= 0))
                return;

            gradationMaxValue = Base.Convert.doubleParse(drs[0]["MAX"].ToString());
            gradationMinValue = Base.Convert.doubleParse(drs[0]["MIN"].ToString());

            drs = (DataSource as DataSet).Tables["PARA_ITEM"].Select(
                String.Format("[PARAM_NAME] = '{0}'", selectedParameter)
                );

            chkControlLimit.Checked = false;
            chkSpecificationLimit.Checked = false;
            chkTightenLimit.Checked = false;
            if (drs.Length > 0)
            {
                txtUSL.Text = drs[0]["USL"].ToString();
                txtLSL.Text = drs[0]["LSL"].ToString();
                txtUCL.Text = drs[0]["UCL"].ToString();
                txtLCL.Text = drs[0]["LCL"].ToString();
                txtUTL.Text = drs[0]["UTL"].ToString();
                txtLTL.Text = drs[0]["LTL"].ToString();
            }
            else
            {
                txtUSL.Text = string.Empty;
                txtLSL.Text = string.Empty;
                txtUCL.Text = string.Empty;
                txtLCL.Text = string.Empty;
                txtUTL.Text = string.Empty;
                txtLTL.Text = string.Empty;
            }

            SetCutValue();

            btnTestDataDraw.PerformClick();
        }
        private void SetParameterBinding(
            )
        {
            string[] strFilter = null;
            DataTable dt = null;
            DataTable dtTmp = null;
            if (chkTotalPara.Checked)
                dt = (DataSource as DataSet).Tables["PARA_ITEM"];
            else
                dt = (DataSource as DataSet).Tables["PARA_LIST"];

            lsParaList.Items.Clear();
            dtTmp = dt.Copy();
            if (!string.IsNullOrEmpty(txtItemFilter.Text))
            {
                dtTmp.Rows.Clear();
                strFilter = txtItemFilter.Text.Replace(",", ";").Replace("*", "%").Replace(" ", "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string val in strFilter)
                {
                    DataRow[] drs = dt.Select(String.Format("PARAM_NAME LIKE '{0}'", val));
                    if (drs.Length > 0)
                    {
                        foreach (DataRow drFilter in drs)
                        {
                            dtTmp.Rows.Add(drFilter.ItemArray);
                        }
                    }
                }
            }

            foreach (DataRow row in dtTmp.Rows)
            {
                lsParaList.Items.Add(row["PARAM_NAME"].ToString());
            }
        }

        private void SetCutValue(
            )
        {
            int nCutCnt = Base.Convert.intParse(cbCutCnt.Text);
            double dGap = (gradationMaxValue - gradationMinValue) / nCutCnt;

            clbParaCut.Items.Clear();
            if (!double.Equals(dGap, 0.0d))
            {
                for (int idx = 0; idx < nCutCnt; idx++)
                {
                    clbParaCut.Items.Insert(0, string.Format("{0:0.#######} ~ {1:0.#######}", gradationMinValue + (idx * dGap), gradationMinValue + ((idx + 1) * dGap)));
                    clbParaCut.SetItemChecked(0, true);
                }
            }
            clbParaCut.Items.Insert(0, "ALL");
            clbParaCut.SetItemChecked(0, true);
        }

        #endregion [ Method ]

        #region [ Property ]

        public Base.TPWafer[] WaferInfo
        {
            get;
            set;
        }

        public object DataSource
        {
            get;
            private set;
        }

        #endregion [ Property ]
    }
}
