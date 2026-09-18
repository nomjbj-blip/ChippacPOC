using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Map;
using FarPoint.Win.Spread;
using System.Data;
using DACrux.Common.RO;
using System.Drawing;

namespace DACrux.SEMDMS.Control
{
    public class UserConfiguration
    {
        public static void SetDefectColor(DefectMap map)
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis obj = new DACrux.SEMDMS.RO.DefectMapAnalysis();

            DataTable sizeDt = obj.SelectColorByDefectSize(DACrux.Base.GlobalVariable.UserID);
            DataTable colorDt = obj.GetColorByDefectType();

            if (map != null)
            {
                map.SizeColor = sizeDt;
                map.TypeColor = colorDt;
            }
        }

        public static void SetMapInformation(DefectMap map, DataSet dsMapInfo)
        {
            // TQC_CONFIG_USER 테이블에 등록되어 있는 Information 정보를 표시하는 부분
            ComConfiguration obj = new ComConfiguration();
            DataTable dt = obj.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "WAFER_OPTION", DACrux.Base.GlobalVariable.UserID);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtList = null;
                List<string> sWaferInfo = new List<string>();
                List<string> lsList = null;
                foreach (DataRow dr in dt.Rows)
                {
                    lsList = new List<string>();
                    if (dsMapInfo.Tables["STEP_INFO"].Columns.Contains(dr["NAME"].ToString()))
                    {
                        dtList = dsMapInfo.Tables["STEP_INFO"].DefaultView.ToTable(true, dr["NAME"].ToString());
                        foreach (DataRow drList in dtList.Rows)
                        {
                            lsList.Add(drList[0].ToString());
                        }
                    }

                    if (lsList.Count > 0)
                        sWaferInfo.Add(string.Format("{0}: {1}", dr["VALUE"], string.Join(",", lsList.ToArray())));
                }
                map.SetInfomation(sWaferInfo.ToArray());
            }

            //--

            if (dsMapInfo.Tables.Contains("STEP_INFO") && dsMapInfo.Tables["STEP_INFO"].Rows.Count > 0 && dsMapInfo.Tables["STEP_INFO"].Columns.Contains("WAFER_ID"))
            {
                map.WaferID = dsMapInfo.Tables["STEP_INFO"].Rows[0]["WAFER_ID"].ToString();
            }

            //Wafer Information 사용 여부
            dt = obj.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "WAFER_OPTION_ENABLE", DACrux.Base.GlobalVariable.UserID);

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["VALUE"].ToString() == "N")
                    map.SetInfomation(null);
            }

            // Defect Size
            float defectSize;
            
            if (obj.TryDefaultDefectSize(out defectSize))
                 map.DefectSize = defectSize;
            else
                map.DefectSize = DefectMap.DEFAULT_DEFECT_SIZE;
            
        }

        public static void SetVirtualDie(DefectMap map)
        {
            //Virture Die 에 대한 Information Set
            ComConfiguration obj = new ComConfiguration();
            DataTable dtVir = obj.GetConfigUser(DACrux.Base.GlobalVariable.Factory, "VIRTUAL_OPTION", DACrux.Base.GlobalVariable.UserID);

            if (dtVir != null && dtVir.Rows.Count > 0)
            {
                foreach (DataRow dr in dtVir.Rows)
                {
                    if (dr["NAME"].ToString() == "COLOR")
                    {
                        map.VirtualDieColor = System.Drawing.ColorTranslator.FromHtml(dr["VALUE"].ToString());
                    }

                    if (dr["NAME"].ToString() == "VISIBLE")
                    {
                        if (dr["VALUE"].ToString() == "Y")
                            WaferMap.AppendVirtualDie(map);
                    }
                }
            }
        }

        public static void SetMapColor(DefectMap map)
        {
            //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
            ComConfiguration obj = new ComConfiguration();
            DataTable dt = obj.SelectDefectMapConfig(DACrux.Base.GlobalVariable.Factory, DACrux.Base.GlobalVariable.UserID);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string strType = dr["NAME"].ToString();
                    Color crType = ColorTranslator.FromHtml(dr["VALUE"].ToString());

                    switch (strType)
                    {
                        //Wafer Base Color
                        case "WAFER_MAP_BG":
                            map.WaferColor = crType;
                            break;
                        //Inspection Die Color
                        case "WAFER_MAP_INP":
                            map.DieBackgroundColor = crType;
                            break;
                        //Inspection Defect Die Color
                        case "WAFER_MAP_DEFECT":
                            map.DieDefectColor = crType;
                            break;
                        //Wafer Border Line Color
                        case "WAFER_MAP_LINE":
                            map.DieBorderColor = crType;
                            break;
                    }
                }
            }
        }
    }
}
