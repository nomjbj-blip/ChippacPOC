using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DACrux.Utility
{
    public static class MenuHelper
    {
        /// <summary>
        /// 권한이 없는 Menu는 보이지 않게 한다
        /// </summary>ㅡ
        /// <param name="dtFunction">User 별로 권한이 있는 Menu List</param>
        /// <returns></returns>
        public static DataTable GetMenu(DataTable dtFunction)
        {
            DataTable dt = null;
            DataSet ds = null;

            DataTable dtTmp = null;
            DataTable dtTmpResult = null;

            string strMenuFile = Application.StartupPath + @"\MenuFile.mnu";

            try
            {
                if (File.Exists(strMenuFile))
                {
                    ds = new DataSet();
                    ds.ReadXmlSchema(strMenuFile);
                    ds.ReadXml(strMenuFile);

                    dtTmp = new DataTable();
                    dtTmpResult = new DataTable();

                    dtTmp = ds.Tables[0].Copy();

                    dtTmpResult.Columns.Add(new DataColumn("MENU_KEY", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("MENU_ID", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("POPUP_MENU", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("STOCK_TOOLBAR", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("CAPTION001", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("CAPTION002", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("CAPTION003", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("CAPTION004", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("CAPTION005", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("TOOL_TIP", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("ICON32", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("ICON16", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("FORM_TYPE", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("MODULE", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("ASSEMBLY", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("HELP_URL", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("MNU_ORDER", typeof(int)));
                    dtTmpResult.Columns.Add(new DataColumn("RESV_01", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("RESV_02", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("RESV_03", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("RESV_04", typeof(string)));
                    dtTmpResult.Columns.Add(new DataColumn("RESV_05", typeof(string)));


                    for (int i = 0; i < dtTmp.Rows.Count; i++)
                    {

                        if (dtTmp.Rows[i]["POPUP_MENU"].ToString().ToUpper() == "ROOT"
                            || dtTmp.Rows[i]["POPUP_MENU"].ToString().ToUpper() == "MNU_FILE"
                            || dtTmp.Rows[i]["POPUP_MENU"].ToString().ToUpper() == "MNU_STATISTICAL"
                            || dtTmp.Rows[i]["POPUP_MENU"].ToString().ToUpper() == "MNU_GRAPH_ANALYSIS"
                            || dtTmp.Rows[i]["RESV_05"].ToString().ToUpper() == "SUB_MENU"
                            )
                        {
                            dtTmpResult.ImportRow(dtTmp.Rows[i]);
                        }

                        for (int j = 0; j < dtFunction.Rows.Count; j++)
                        {
                            if (dtTmp.Rows[i]["CAPTION001"].ToString() == dtFunction.Rows[j]["HELP_URL"].ToString())
                            {
                                dtTmpResult.ImportRow(dtTmp.Rows[i]);
                            }
                        }
                        //if (dtTmp.Rows[i]["POPUO_MENU"].ToString() == "ROOT") 
                        //    dtTmpResult.ImportRow(dtTmp.Rows[i]);
                    }
                    int order = 0;
                    for (int x = 0; x < dtTmpResult.Rows.Count; x++)
                    {
                        if (dtTmpResult.Rows[x]["POPUP_MENU"].ToString() != "ROOT" && dtTmpResult.Rows[x]["POPUP_MENU"].ToString() != dtTmpResult.Rows[x - 1]["POPUP_MENU"].ToString())
                        {
                            order = 0;
                        }
                        dtTmpResult.Rows[x]["MNU_ORDER"] = order.ToString();
                        order++;
                    }

                    if (Regedit.LastUser.ToUpper() == "YSIM")
                    {
                        return dtTmp;
                    }
                    else
                    {
                        return dtTmpResult;
                    }
                }

                #region MENU(not found file)

                #region POPUP MNU
                dt = new DataTable();

                dt.Columns.Add(new DataColumn("MENU_KEY", typeof(string)));
                dt.Columns.Add(new DataColumn("MENU_ID", typeof(string)));
                dt.Columns.Add(new DataColumn("POPUP_MENU", typeof(string)));
                dt.Columns.Add(new DataColumn("STOCK_TOOLBAR", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION001", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION002", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION003", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION004", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION005", typeof(string)));
                dt.Columns.Add(new DataColumn("TOOL_TIP", typeof(string)));
                dt.Columns.Add(new DataColumn("ICON32", typeof(string)));
                dt.Columns.Add(new DataColumn("ICON16", typeof(string)));
                dt.Columns.Add(new DataColumn("FORM_TYPE", typeof(string)));
                dt.Columns.Add(new DataColumn("MODULE", typeof(string)));
                dt.Columns.Add(new DataColumn("ASSEMBLY", typeof(string)));
                dt.Columns.Add(new DataColumn("HELP_URL", typeof(string)));
                dt.Columns.Add(new DataColumn("MNU_ORDER", typeof(int)));
                dt.Columns.Add(new DataColumn("RESV_01", typeof(string)));
                dt.Columns.Add(new DataColumn("RESV_02", typeof(string)));
                dt.Columns.Add(new DataColumn("RESV_03", typeof(string)));
                dt.Columns.Add(new DataColumn("RESV_04", typeof(string)));
                dt.Columns.Add(new DataColumn("RESV_05", typeof(string)));

                #endregion

                #region ROOT MNU
                dt.Rows.Add(new object[] { "MNU_FILE", "MNU_FILE", "ROOT", ""
                                         , "File","","","","", "File", "","", "", "", ""
                                         , "",  0 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN", "MNU_ADMIN", "ROOT", ""
                                         , "Admin","","","","", "Administrator","", "", "", "", ""
                                         , "",  1 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_SETUP", "MNU_SETUP", "ROOT", ""
                                         , "Setup","","","","", "Setup & Configuration","", "", "", "", ""
                                         , "",  2 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_STATISTICAL", "MNU_STATISTICAL", "ROOT", ""
                                         , "Statistical","","","","", "Statistical","", "", "", "", ""
                                         , "",  3 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS", "MNU_GRAPH_ANALYSIS", "ROOT", ""
                                         , "Graph Analysis","","","","", "Graph Analysis","", "", "", "", ""
                                         , "",  4 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_REPORT", "MNU_REPORT", "ROOT", ""
                                         , "Report","","","","", "Report","", "", "", "", ""
                                         , "",  5 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_MONITOR", "MNU_MONITOR", "ROOT", ""
                                         , "Monitor","","","","", "Monitoring","", "", "", "", ""
                                         , "",  6 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_ANALYSIS", "MNU_ANALYSIS", "ROOT", ""
                                         , "Analysis","","","","", "Analysis", "","", "", "", ""
                                         , "",  7 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_PREDICTION", "MNU_PREDICTION", "ROOT",  ""
                                         , "Prediction", "","","","","Prediction & Modeling","", "", "", "", ""
                                         , "", 8 ,"" ,"" ,"" ,"" ,"" });
                #endregion

                #region Stat Default
                /// File
                dt.Rows.Add(new object[] { "MNU_FILE_NEW_PROJECT", "MNU_FILE_NEW_PROJECT", "MNU_FILE", "STAT"
                                         , "New Project","","","","", "New Project", "","", "", "", ""
                                         , "", 0 ,"FILE_NEW_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_OPEN_PROJECT", "MNU_FILE_OPEN_PROJECT", "MNU_FILE", "STAT"
                                         , "Open Project","","","","", "Open Project", "","", "", "", ""
                                         , "", 1 ,"FILE_OPEN_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_IMPORT_FILE", "MNU_FILE_IMPORT_FILE", "MNU_FILE", "STAT"
                                         , "Import File","","","","", "Import File","", "", "", "", ""
                                         , "", 2 ,"FILE_IMPORT_FILE" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_IMPORT_DATABASE", "MNU_FILE_IMPORT_DATABASE", "MNU_FILE", "STAT"
                                         , "Import Database", "","","","","Import Database", "","", "", "", ""
                                         , "", 3 ,"FILE_IMPORT_DATABASE_SOURCE" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_SAVE_PROJECT", "MNU_FILE_SAVE_PROJECT", "MNU_FILE", "STAT"
                                         , "Save Project","","","","", "Save Project", "","", "", "", ""
                                         , "", 4 ,"FILE_SAVE_PROJECT" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_FILE_SAVE_AS_PROJECT", "MNU_FILE_SAVE_AS_PROJECT", "MNU_FILE", "STAT"
                                         , "Save As Project","","","","", "Save As Project", "","", "", "", ""
                                         , "", 5 ,"FILE_SAVE_PROJECT_AS" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_RENAME_PROJECT", "MNU_FILE_OPEN_PROJECT", "MNU_FILE", "STAT"
                                         , "Rename Project","","","","", "Rename Project", "","", "", "", ""
                                         , "", 6 ,"FILE_RENAME_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_DELETE_PROJECT", "MNU_FILE_OPEN_PROJECT", "MNU_FILE", "STAT"
                                         , "Delete Project","","","","", "Delete Project", "","", "", "", ""
                                         , "", 7 ,"FILE_REMOVE_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_CLOSE_PROJECT", "MNU_FILE_OPEN_PROJECT", "MNU_FILE", "STAT"
                                         , "Close Project","","","","", "Close Project", "","", "", "", ""
                                         , "", 8 ,"FILE_CLOSE_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_NEW_WORK_SHEET", "MNU_FILE_NEW_WORK_SHEET", "MNU_FILE", "STAT"
                                         , "New Work Sheet","","","","", "New Work Sheet","", "", "", "", ""
                                         , "", 9 ,"FILE_NEW_WORKSHEET" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_RENAME_WORK_SHEET", "MNU_FILE_RENAME_WORK_SHEET", "MNU_FILE", "STAT"
                                         , "Rename Work Sheet","","","","", "Rename Work Sheet","", "", "", "", ""
                                         , "", 10 ,"FILE_RENAME_WORKSHEET" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_DELETE_WORK_SHEET", "MNU_FILE_DELETE_WORK_SHEET", "MNU_FILE", "STAT"
                                         , "Delete Work Sheet","","","","", "Delete Work Sheet", "","", "", "", ""
                                         , "", 11 ,"FILE_REMOVE_WORKSHEET" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_CLOSE_WORK_SHEET", "MNU_FILE_CLOSE_WORK_SHEET", "MNU_FILE", "STAT"
                                         , "Close Work Sheet","","","","", "Close Work Sheet", "","", "", "", ""
                                         , "", 12 ,"FILE_CLOSE_WORKSHEET" ,"" ,"" ,"" ,""});

                /// Statistical
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_DESC_ANALYSIS", "MNU_STATISTICAL_DESC_ANALYSIS", "MNU_STATISTICAL", "STAT"
                                         , "Descriptive Analysis","","","","", "Descriptive Analysis","", "", "", "", ""
                                         , "", 0 ,"STAT_DESC_ANALYSIS" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_HYPOTHESIS", "MNU_STATISTICAL_HYPOTHESIS", "MNU_STATISTICAL", "STAT"
                                         , "Hypothesis Analysis","","","","", "Hypothesis Analysis", "","", "", "", ""
                                         , "", 1 ,"STAT_HYPOTHESIS" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_CORRELATION", "MNU_STATISTICAL_CORRELATION", "MNU_STATISTICAL", "STAT"
                                         , "Correlation Analysis","","","","", "Correlation Analysis", "","", "", "", ""
                                         , "", 2 ,"STAT_CORRELATION" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_STAT_REGRESSION", "MNU_STATISTICAL_STAT_REGRESSION", "MNU_STATISTICAL", "STAT"
                                         , "Regression Analysis","","","","", "Regression Analysis", "","", "", "", ""
                                         , "", 3 ,"STAT_DESC_ANALYSIS" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_ANOVA", "MNU_STATISTICAL_ANOVA", "MNU_STATISTICAL", "STAT"
                                         , "ANOVA","","","","", "ANOVA","", "", "", "", ""
                                         , "", 4 ,"STAT_DESC_ANALYSIS" ,"" ,"" ,"" ,""});

                /// Graph
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_PIE", "MNU_GRAPH_ANALYSIS_GRAPH_PIE", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Pie","","","","", "Pie Chart", "","", "", "", ""
                                         , "", 0 ,"GRAPH_PIE" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_BAR", "MNU_GRAPH_ANALYSIS_GRAPH_BAR", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Bar","","","","", "Bar Chart", "","", "", "", ""
                                         , "", 1 ,"GRAPH_BAR" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_SCATTER", "MNU_GRAPH_ANALYSIS_GRAPH_SCATTER", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Scatter","","","","", "Scatter Chart","", "", "", "", ""
                                         , "", 2 ,"GRAPH_SCATTER" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_LINE", "MNU_GRAPH_ANALYSIS_GRAPH_LINE", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Line","","","","", "Line Chart", "","", "", "", ""
                                         , "", 3 ,"GRAPH_LINE" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_BOXPLOT", "MNU_GRAPH_ANALYSIS_GRAPH_BOXPLOT", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Boxplot","","","","", "Boxplot Chart","", "", "", "", ""
                                         , "", 4 ,"GRAPH_BOXPLOT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_PARETO", "MNU_GRAPH_ANALYSIS_GRAPH_PARETO", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Pareto", "","","","","Pareto Chart", "","", "", "", ""
                                         , "", 5 ,"GRAPH_PARETO" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_HISTOGRAM", "MNU_GRAPH_ANALYSIS_GRAPH_HISTOGRAM", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Histogram","","","","", "Histogram Chart","", "", "", "", ""
                                         , "", 6 ,"GRAPH_HISTOGRAM" ,"" ,"" ,"" ,""});

                #endregion

                #region Admin
                dt.Rows.Add(new object[] { "MNU_ADMIN_USER_MANAGEMENT", "MNU_FILE_CLOSE_WORK_SHEET", "MNU_ADMIN", ""
                                         , "User Management", "","","","","User Management", "","", "", "", ""
                                         , "", 0 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_SECURITY_GROUP_MANAGEMENT", "MNU_ADMIN_SECURITY_GROUP_MANAGEMENT", "MNU_ADMIN", ""
                                        , "Security Group Management","","","","", "Security Group Management","", "", "", "", ""
                                        , "", 1 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_FUNCTION_MANAGEMENT", "MNU_ADMIN_FUNCTION_MANAGEMENT", "MNU_ADMIN", ""
                                        , "Function Management","","","","", "Function Management", "","", "", "", ""
                                        , "", 2 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_USER_MONITORING", "MNU_ADMIN_USER_MONITORING", "MNU_ADMIN", ""
                                        , "User Monitoring","","","","", "User Monitoring","", "", "", "", ""
                                        , "", 3 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_MY_INFORMATION", "MNU_ADMIN_MY_INFORMATION", "MNU_ADMIN", ""
                                        , "My Information", "","","","","My Information","", "", "", "", ""
                                        , "", 4 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_SEMDMS_CONFIG", "MNU_ADMIN_SEMDMS_CONFIG", "MNU_ADMIN", "DMS"
                                        , "DMS Configuration","","","","", "DMS Configuration", "","", "MODAL_FROM", "DACrux.SEMDMS.Admin.dll", "DACrux.SEMDMS.Admin.frmConfiguration"
                                        , "", 5 ,"" ,"BEGINE" ,"" ,"" ,""});
                #endregion

                #region Setup
                //dt.Rows.Add(new object[] { "MNU_SETUP_CREATE_EQUIP", "MNU_SETUP_CREATE_EQUIP", "MNU_SETUP", "DMS"
                //                        , "DMS Equip Setup","","","","", "DMS Equip Setup", "","", "BASIC_FORM", "DACrux.SEMDMS.Admin.dll", "DACrux.SEMDMS.Admin.frmCreateEquip"
                //                        , "", 0 ,"" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_SETUP_SITE_OPTION", "MNU_SETUP_SITE_OPTION", "MNU_SETUP", "EMS"
                //                        , "Site Option", "","","","","Site Option", "","", "BASIC_FORM", "EMS.WB.ENGUI.dll", "EMS.WB.ENGUI.frmSiteOption"
                //                        , "", 1 ,"" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_SETUP_EQUIP_MODEL", "MNU_SETUP_EQUIP_MODEL", "MNU_SETUP", "EMS"
                //                        , "Equip Model", "","","","","Equip Model", "","", "BASIC_FORM", "EMS.WB.ENGUI.dll", "EMS.WB.ENGUI.frmEquipModelMgr"
                //                        , "", 2 ,"" ,"" ,"" ,"" ,""});

                dt.Rows.Add(new object[] {  "MNU_REPORT_RECIPE_MANAGEMENT", "MNU_REPORT_RECIPE_MANAGEMENT", "MNU_SETUP", "EMS"
                                        , "Recipe Management","","","","", "Recipe Management","", "", "BASIC_FORM", "EMS.WB.ENGUI.dll", "EMS.WB.ENGUI.frmRecipeManagement"
                                        , "", 0 ,"" ,"" ,"" ,"" ,""});
                #endregion

                #region Report
                dt.Rows.Add(new object[] { "MNU_REPORT_WBRECIPE_QUICK_VIEW", "MNU_REPORT_WBRECIPE_QUICK_VIEW", "MNU_REPORT", "EMS"
                                        , "Recipe Quick View","","","","", "EMS WB Recipe Quick View", "","", "BASIC_FORM", "EMS.WB.ENGUI.dll", "EMS.WB.ENGUI.frmRecipeQuickView"
                                       , "", 0 ,"" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_REPORT_RECIPE_MANAGEMENT", "MNU_REPORT_RECIPE_MANAGEMENT", "MNU_REPORT", "EMS"
                //                        , "Recipe Management","","","","", "Recipe Management","", "", "BASIC_FORM", "EMS.WB.ENGUI.dll", "EMS.WB.ENGUI.frmRecipeManagement"
                //, "", 1 ,"" ,"" ,"" ,"" ,""});
                #endregion

                #region Windows
                //dt.Rows.Add(new object[] { "MNU_WINDOWS_", "MNU_GRAPH_ANALYSIS_GRAPH_PIE", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Pie", "Pie Chart", "", "", "", ""
                //                         , "", 0 ,"GRAPH_PIE" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_BAR", "MNU_GRAPH_ANALYSIS_GRAPH_BAR", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Bar", "Bar Chart", "", "", "", ""
                //                         , "", 1 ,"GRAPH_BAR" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_SCATTER", "MNU_GRAPH_ANALYSIS_GRAPH_SCATTER", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Scatter", "Scatter Chart", "", "", "", ""
                //                         , "", 2 ,"GRAPH_SCATTER" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_LINE", "MNU_GRAPH_ANALYSIS_GRAPH_LINE", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Line", "Line Chart", "", "", "", ""
                //                         , "", 3 ,"GRAPH_LINE" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_BOXPLOT", "MNU_GRAPH_ANALYSIS_GRAPH_BOXPLOT", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Boxplot", "Boxplot Chart", "", "", "", ""
                //                         , "", 4 ,"GRAPH_BOXPLOT" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_PARETO", "MNU_GRAPH_ANALYSIS_GRAPH_PARETO", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Pareto", "Pareto Chart", "", "", "", ""
                //                         , "", 5 ,"GRAPH_PARETO" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_HISTOGRAM", "MNU_GRAPH_ANALYSIS_GRAPH_HISTOGRAM", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Histogram", "Histogram Chart", "", "", "", ""
                //                         , "", 6 ,"GRAPH_HISTOGRAM" ,"" ,"" ,"" ,""});
                #endregion

                #endregion

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 전제 Menu가 다 보인다.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMenu()
        {
            DataTable dt = null;
            DataSet ds = null;
            string strMenuFile = Application.StartupPath + @"\MenuFile.mnu";

            try
            {
                if (File.Exists(strMenuFile))
                {
                    ds = new DataSet();
                    ds.ReadXml(strMenuFile);
                    return ds.Tables[0];
                }

                #region POPUP MNU
                dt = new DataTable();
                //dt.Columns.Add(new DataColumn("MENU_KEY", typeof(string)));
                //dt.Columns.Add(new DataColumn("MENU_ID", typeof(string)));
                //dt.Columns.Add(new DataColumn("POPUP_MENU", typeof(string)));
                //dt.Columns.Add(new DataColumn("STOCK_TOOLBAR", typeof(string)));
                //dt.Columns.Add(new DataColumn("CAPTION", typeof(string)));
                //dt.Columns.Add(new DataColumn("TOOL_TIP", typeof(string)));
                //dt.Columns.Add(new DataColumn("ICON", typeof(string)));
                //dt.Columns.Add(new DataColumn("FORM_TYPE", typeof(string)));
                //dt.Columns.Add(new DataColumn("MODULE", typeof(string)));
                //dt.Columns.Add(new DataColumn("ASSEMBLY", typeof(string)));
                //dt.Columns.Add(new DataColumn("HELP_URL", typeof(string)));
                //dt.Columns.Add(new DataColumn("MNU_ORDER", typeof(int)));
                //dt.Columns.Add(new DataColumn("RESV_01", typeof(string)));
                //dt.Columns.Add(new DataColumn("RESV_02", typeof(string)));
                //dt.Columns.Add(new DataColumn("RESV_03", typeof(string)));
                //dt.Columns.Add(new DataColumn("RESV_04", typeof(string)));
                //dt.Columns.Add(new DataColumn("RESV_05", typeof(string)));


                dt.Columns.Add(new DataColumn("MENU_KEY", typeof(string)));
                dt.Columns.Add(new DataColumn("MENU_ID", typeof(string)));
                dt.Columns.Add(new DataColumn("POPUP_MENU", typeof(string)));
                dt.Columns.Add(new DataColumn("STOCK_TOOLBAR", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION001", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION002", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION003", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION004", typeof(string)));
                dt.Columns.Add(new DataColumn("CAPTION005", typeof(string)));
                dt.Columns.Add(new DataColumn("TOOL_TIP", typeof(string)));
                dt.Columns.Add(new DataColumn("ICON32", typeof(string)));
                dt.Columns.Add(new DataColumn("ICON16", typeof(string)));
                dt.Columns.Add(new DataColumn("FORM_TYPE", typeof(string)));
                dt.Columns.Add(new DataColumn("MODULE", typeof(string)));
                dt.Columns.Add(new DataColumn("ASSEMBLY", typeof(string)));
                dt.Columns.Add(new DataColumn("HELP_URL", typeof(string)));
                dt.Columns.Add(new DataColumn("MNU_ORDER", typeof(int)));
                dt.Columns.Add(new DataColumn("RESV_01", typeof(string)));
                dt.Columns.Add(new DataColumn("RESV_02", typeof(string)));
                dt.Columns.Add(new DataColumn("RESV_03", typeof(string)));
                dt.Columns.Add(new DataColumn("RESV_04", typeof(string)));
                dt.Columns.Add(new DataColumn("RESV_05", typeof(string)));

                #endregion

                #region Stat Default
                /*
                dt.Rows.Add(new object[] { "MNU_FILE", "MNU_FILE", "ROOT", ""
                                         , "File","","","","", "파일", "","", "", "", ""
                                         , "",  0 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN", "MNU_ADMIN", "ROOT", ""
                                         , "Admin","","","","", "Administrator","", "", "", "", ""
                                         , "",  1 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_SETUP", "MNU_SETUP", "ROOT", ""
                                         , "Setup","","","","", "Setup & Configuration","", "", "", "", ""
                                         , "",  2 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_STATISTICAL", "MNU_STATISTICAL", "ROOT", ""
                                         , "Statistical","","","","", "Statistical","", "", "", "", ""
                                         , "",  3 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS", "MNU_GRAPH_ANALYSIS", "ROOT", ""
                                         , "Graph Analysis","","","","", "Graph Analysis","", "", "", "", ""
                                         , "",  4 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_REPORT", "MNU_REPORT", "ROOT", ""
                                         , "Report","","","","", "Report","", "", "", "", ""
                                         , "",  5 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_MONITOR", "MNU_MONITOR", "ROOT", ""
                                         , "Monitor","","","","", "Monitoring","", "", "", "", ""
                                         , "",  6 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_ANALYSIS", "MNU_ANALYSIS", "ROOT", ""
                                         , "Analysis","","","","", "Analysis", "","", "", "", ""
                                         , "",  7 ,"" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_PREDICTION", "MNU_PREDICTION", "ROOT",  ""
                                         , "Prediction", "","","","","Prediction & Modeling","", "", "", "", ""
                                         , "", 8 ,"" ,"" ,"" ,"" ,"" });
                /// File
                dt.Rows.Add(new object[] { "MNU_FILE_NEW_PROJECT", "MNU_FILE_NEW_PROJECT", "MNU_FILE", "STAT"
                                         , "New Project","","","","", "New Project", "","", "", "", ""
                                         , "", 0 ,"FILE_NEW_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_OPEN_PROJECT", "MNU_FILE_OPEN_PROJECT", "MNU_FILE", "STAT"
                                         , "Open Project","","","","", "Open Project", "","", "", "", ""
                                         , "", 1 ,"FILE_OPEN_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_IMPORT_FILE", "MNU_FILE_IMPORT_FILE", "MNU_FILE", "STAT"
                                         , "Import File","","","","", "Import File","", "", "", "", ""
                                         , "", 2 ,"FILE_IMPORT_FILE" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_IMPORT_DATABASE", "MNU_FILE_IMPORT_DATABASE", "MNU_FILE", "STAT"
                                         , "Import Database", "","","","","Import Database", "","", "", "", ""
                                         , "", 3 ,"FILE_IMPORT_DATABASE_SOURCE" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_SAVE_PROJECT", "MNU_FILE_SAVE_PROJECT", "MNU_FILE", "STAT"
                                         , "Save Project","","","","", "Save Project", "","", "", "", ""
                                         , "", 4 ,"FILE_SAVE_PROJECT" ,"" ,"" ,"" ,"" });
                dt.Rows.Add(new object[] { "MNU_FILE_SAVE_AS_PROJECT", "MNU_FILE_SAVE_AS_PROJECT", "MNU_FILE", "STAT"
                                         , "Save As Project","","","","", "Save As Project", "","", "", "", ""
                                         , "", 5 ,"FILE_SAVE_PROJECT_AS" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_RENAME_PROJECT", "MNU_FILE_OPEN_PROJECT", "MNU_FILE", "STAT"
                                         , "Rename Project","","","","", "Rename Project", "","", "", "", ""
                                         , "", 6 ,"FILE_RENAME_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_DELETE_PROJECT", "MNU_FILE_OPEN_PROJECT", "MNU_FILE", "STAT"
                                         , "Delete Project","","","","", "Delete Project", "","", "", "", ""
                                         , "", 7 ,"FILE_REMOVE_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_CLOSE_PROJECT", "MNU_FILE_OPEN_PROJECT", "MNU_FILE", "STAT"
                                         , "Close Project","","","","", "Close Project", "","", "", "", ""
                                         , "", 8 ,"FILE_CLOSE_PROJECT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_NEW_WORK_SHEET", "MNU_FILE_NEW_WORK_SHEET", "MNU_FILE", "STAT"
                                         , "New Work Sheet","","","","", "New Work Sheet","", "", "", "", ""
                                         , "", 9 ,"FILE_NEW_WORKSHEET" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_RENAME_WORK_SHEET", "MNU_FILE_RENAME_WORK_SHEET", "MNU_FILE", "STAT"
                                         , "Rename Work Sheet","","","","", "Rename Work Sheet","", "", "", "", ""
                                         , "", 10 ,"FILE_RENAME_WORKSHEET" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_DELETE_WORK_SHEET", "MNU_FILE_DELETE_WORK_SHEET", "MNU_FILE", "STAT"
                                         , "Delete Work Sheet","","","","", "Delete Work Sheet", "","", "", "", ""
                                         , "", 11 ,"FILE_REMOVE_WORKSHEET" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_FILE_CLOSE_WORK_SHEET", "MNU_FILE_CLOSE_WORK_SHEET", "MNU_FILE", "STAT"
                                         , "Close Work Sheet","","","","", "Close Work Sheet", "","", "", "", ""
                                         , "", 12 ,"FILE_CLOSE_WORKSHEET" ,"" ,"" ,"" ,""});

                /// Statistical
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_DESC_ANALYSIS", "MNU_STATISTICAL_DESC_ANALYSIS", "MNU_STATISTICAL", "STAT"
                                         , "Descriptive Analysis","","","","", "Descriptive Analysis","", "", "", "", ""
                                         , "", 0 ,"STAT_DESC_ANALYSIS" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_HYPOTHESIS", "MNU_STATISTICAL_HYPOTHESIS", "MNU_STATISTICAL", "STAT"
                                         , "Hypothesis Analysis","","","","", "Hypothesis Analysis", "","", "", "", ""
                                         , "", 1 ,"STAT_HYPOTHESIS" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_CORRELATION", "MNU_STATISTICAL_CORRELATION", "MNU_STATISTICAL", "STAT"
                                         , "Correlation Analysis","","","","", "Correlation Analysis", "","", "", "", ""
                                         , "", 2 ,"STAT_CORRELATION" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_STAT_REGRESSION", "MNU_STATISTICAL_STAT_REGRESSION", "MNU_STATISTICAL", "STAT"
                                         , "Regression Analysis","","","","", "Regression Analysis", "","", "", "", ""
                                         , "", 3 ,"STAT_DESC_ANALYSIS" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_STATISTICAL_ANOVA", "MNU_STATISTICAL_ANOVA", "MNU_STATISTICAL", "STAT"
                                         , "ANOVA","","","","", "ANOVA","", "", "", "", ""
                                         , "", 4 ,"STAT_DESC_ANALYSIS" ,"" ,"" ,"" ,""});

                /// Graph
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_PIE", "MNU_GRAPH_ANALYSIS_GRAPH_PIE", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Pie","","","","", "Pie Chart", "","", "", "", ""
                                         , "", 0 ,"GRAPH_PIE" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_BAR", "MNU_GRAPH_ANALYSIS_GRAPH_BAR", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Bar","","","","", "Bar Chart", "","", "", "", ""
                                         , "", 1 ,"GRAPH_BAR" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_SCATTER", "MNU_GRAPH_ANALYSIS_GRAPH_SCATTER", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Scatter","","","","", "Scatter Chart","", "", "", "", ""
                                         , "", 2 ,"GRAPH_SCATTER" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_LINE", "MNU_GRAPH_ANALYSIS_GRAPH_LINE", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Line","","","","", "Line Chart", "","", "", "", ""
                                         , "", 3 ,"GRAPH_LINE" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_BOXPLOT", "MNU_GRAPH_ANALYSIS_GRAPH_BOXPLOT", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Boxplot","","","","", "Boxplot Chart","", "", "", "", ""
                                         , "", 4 ,"GRAPH_BOXPLOT" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_PARETO", "MNU_GRAPH_ANALYSIS_GRAPH_PARETO", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Pareto", "","","","","Pareto Chart", "","", "", "", ""
                                         , "", 5 ,"GRAPH_PARETO" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_HISTOGRAM", "MNU_GRAPH_ANALYSIS_GRAPH_HISTOGRAM", "MNU_GRAPH_ANALYSIS", "STAT"
                                         , "Histogram","","","","", "Histogram Chart","", "", "", "", ""
                                         , "", 6 ,"GRAPH_HISTOGRAM" ,"" ,"" ,"" ,""});

                #endregion

                #region Admin
                dt.Rows.Add(new object[] { "MNU_ADMIN_USER_MANAGEMENT", "MNU_FILE_CLOSE_WORK_SHEET", "MNU_ADMIN", ""
                                         , "User Management", "","","","","User Management", "","", "", "", ""
                                         , "", 0 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_SECURITY_GROUP_MANAGEMENT", "MNU_ADMIN_SECURITY_GROUP_MANAGEMENT", "MNU_ADMIN", ""
                                        , "Security Group Management","","","","", "Security Group Management","", "", "", "", ""
                                        , "", 1 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_FUNCTION_MANAGEMENT", "MNU_ADMIN_FUNCTION_MANAGEMENT", "MNU_ADMIN", ""
                                        , "Function Management","","","","", "Function Management", "","", "", "", ""
                                        , "", 2 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_USER_MONITORING", "MNU_ADMIN_USER_MONITORING", "MNU_ADMIN", ""
                                        , "User Monitoring","","","","", "User Monitoring","", "", "", "", ""
                                        , "", 3 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_MY_INFORMATION", "MNU_ADMIN_MY_INFORMATION", "MNU_ADMIN", ""
                                        , "My Information", "","","","","My Information","", "", "", "", ""
                                        , "", 4 ,"" ,"" ,"" ,"" ,""});
                dt.Rows.Add(new object[] { "MNU_ADMIN_SEMDMS_CONFIG", "MNU_ADMIN_SEMDMS_CONFIG", "MNU_ADMIN", "DMS"
                                        , "DMS Configuration","","","","", "DMS Configuration", "","", "MODAL_FROM", "DACrux.SEMDMS.Admin.dll", "DACrux.SEMDMS.Admin.frmConfiguration"
                                        , "", 5 ,"" ,"BEGINE" ,"" ,"" ,""});
                #endregion

                #region Setup
                dt.Rows.Add(new object[] { "MNU_SETUP_CREATE_EQUIP", "MNU_SETUP_CREATE_EQUIP", "MNU_SETUP", "DMS"
                                        , "DMS Equip Setup","","","","", "DMS Equip Setup", "","", "BASIC_FORM", "DACrux.SEMDMS.Admin.dll", "DACrux.SEMDMS.Admin.frmCreateEquip"
                                        , "", 0 ,"" ,"" ,"" ,"" ,""});

                dt.Rows.Add(new object[] { "MNU_SETUP_SITE_OPTION", "MNU_SETUP_SITE_OPTION", "MNU_SETUP", "EMS"
                                        , "Site Option", "","","","","Site Option", "","", "BASIC_FORM", "EMS.WB.Admin.dll", "EMS.WB.Admin.frmSiteOption"
                                        , "", 1 ,"" ,"" ,"" ,"" ,""});

                dt.Rows.Add(new object[] { "MNU_SETUP_EQUIP_MODEL", "MNU_SETUP_EQUIP_MODEL", "MNU_SETUP", "EMS"
                                        , "Equip Model", "","","","","Equip Model", "","", "BASIC_FORM", "EMS.WB.Admin.dll", "EMS.WB.Admin.frmEquipModelMgr"
                                        , "", 2 ,"" ,"" ,"" ,"" ,""});
                #endregion

                #region Report
                dt.Rows.Add(new object[] { "MNU_REPORT_WBRECIPE_QUICK_VIEW", "MNU_REPORT_WBRECIPE_QUICK_VIEW", "MNU_REPORT", "EMS"
                                        , "EMS WB Recipe Quick View","","","","", "EMS WB Recipe Quick View", "","", "BASIC_FORM", "EMS.WB.Admin.dll", "EMS.WB.Admin.frmWBRecipeQuickView"
                                        , "", 0 ,"" ,"" ,"" ,"" ,""});

                dt.Rows.Add(new object[] { "MNU_REPORT_RECIPE_VIEW", "MNU_REPORT_RECIPE_VIEW", "MNU_REPORT", "EMS"
                                        , "Recipe View","","","","", "Recipe View","", "", "BASIC_FORM", "EMS.WB.Admin.dll", "EMS.WB.Admin.frmRecipeView"
                                        , "", 1 ,"" ,"" ,"" ,"" ,""});
                #endregion

                #region Windows
                //dt.Rows.Add(new object[] { "MNU_WINDOWS_", "MNU_GRAPH_ANALYSIS_GRAPH_PIE", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Pie", "Pie Chart", "", "", "", ""
                //                         , "", 0 ,"GRAPH_PIE" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_BAR", "MNU_GRAPH_ANALYSIS_GRAPH_BAR", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Bar", "Bar Chart", "", "", "", ""
                //                         , "", 1 ,"GRAPH_BAR" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_SCATTER", "MNU_GRAPH_ANALYSIS_GRAPH_SCATTER", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Scatter", "Scatter Chart", "", "", "", ""
                //                         , "", 2 ,"GRAPH_SCATTER" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_LINE", "MNU_GRAPH_ANALYSIS_GRAPH_LINE", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Line", "Line Chart", "", "", "", ""
                //                         , "", 3 ,"GRAPH_LINE" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_BOXPLOT", "MNU_GRAPH_ANALYSIS_GRAPH_BOXPLOT", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Boxplot", "Boxplot Chart", "", "", "", ""
                //                         , "", 4 ,"GRAPH_BOXPLOT" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_PARETO", "MNU_GRAPH_ANALYSIS_GRAPH_PARETO", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Pareto", "Pareto Chart", "", "", "", ""
                //                         , "", 5 ,"GRAPH_PARETO" ,"" ,"" ,"" ,""});

                //dt.Rows.Add(new object[] { "MNU_GRAPH_ANALYSIS_GRAPH_HISTOGRAM", "MNU_GRAPH_ANALYSIS_GRAPH_HISTOGRAM", "MNU_GRAPH_ANALYSIS", "STAT"
                //                         , "Histogram", "Histogram Chart", "", "", "", ""
                //                         , "", 6 ,"GRAPH_HISTOGRAM" ,"" ,"" ,"" ,""});
                */
                #endregion

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private const string ICO_FILE_NEW_PROJECT = "89 50 4E 47 0D 0A 1A 0A 00 00 00 0D 49 48 44 52 00 00 00 10 00 00 00 10 08 06 00 00 00 1F F3 FF 61 00 00 00 20 63 48 52 4D 00 00 7A 25 00 00 80 83 00 00 F9 FF 00 00 80 E9 00 00 75 30 00 00 EA 60 00 00 3A 98 00 00 17 6F 92 5F C5 46 00 00 00 09 70 48 59 73 00 00 0B 12 00 00 0B 12 01 D2 DD 7E FC 00 00 03 6F 49 44 41 54 38 4F 45 93 8D 4F 53 57 18 C6 FB 8F 2C DB 1F 30 13 92 09 66 66 A5 F1 03 75 CC 8C 8D 00 99 88 1A 17 35 BA 39 70 C3 F1 B1 8D 52 18 14 28 1F 45 C6 00 EB 44 C0 C1 C6 47 29 E2 10 28 F2 31 D8 A4 40 41 08 60 22 2C 84 DA 7B DB AA D3 80 14 EC 17 FC 76 7A 31 D9 CD 79 73 9F 73 CF 3D CF 79 9F E7 7D 8F CA 21 7B 78 2A 79 90 65 19 D9 E9 C2 E5 92 14 AC D5 EA 94 D0 69 73 D1 EA 72 90 5C 32 2E D9 29 D6 DC 02 3B 95 70 49 6E 54 61 D0 7B 23 83 A6 CC 83 34 67 44 73 3B 43 4D 56 5E 31 BD 7D 56 7A 7A 7A F9 A3 A7 9F FE 9E 7B 64 EB 0C 34 67 A9 69 CC DA 4F 53 76 0C BD A6 EF 04 81 1C 26 90 A9 CF 88 61 7D DE C2 DA EC 6F 78 E7 5A 29 29 AF 63 DC 36 8D 6D 62 4A BC ED 4C 4E 3C 44 5F 5E CB E6 6C 0B 1B B3 BF B3 31 D7 49 7D 7A F4 2E 81 EC 96 F8 25 5D 8D CF BD C8 50 47 0D ED 5D 77 A9 6D 34 53 D7 60 A6 B6 FE D7 DD B8 D5 44 CD CD 66 3A BA EE 30 D8 FE 13 5B D2 0C 75 97 F7 28 B2 54 61 DD 37 D3 A2 59 5B 79 40 7B B7 15 76 C2 23 48 F8 11 F0 CD 3C 0C B6 95 6F 16 CB 1D 5E AD FC 8D 29 ED 83 37 04 C2 B0 EB 69 6A DC 8F FA C4 09 FD CA 8F AB 0E 27 4F 24 27 0D 0D 0D 94 18 2A D0 17 95 52 54 6C C0 50 5A 4E 65 D5 35 3C 8B BD 98 AE 44 0B D3 25 91 81 70 D5 94 AA 41 9A B4 D0 DA 75 8F 9D 9D 1D 3C 9E 67 62 43 19 F7 47 C6 08 06 36 09 6E 07 08 85 42 04 7C 7E 06 C7 FE A4 AA A2 5C 1C AA C1 2D BB C2 04 32 B5 97 35 4C 8C 5A 69 69 33 63 BC 56 C9 0D D3 2D 86 87 87 F1 85 82 CC 48 EB 4C AE BE C4 B2 E0 66 DA ED C5 E7 DB 62 68 78 0C 83 2E E3 FF 2A 58 AD 7D 74 76 76 62 AA 6F E4 E2 17 97 C8 C9 F9 9E C1 C1 01 0A 7F 2C 60 61 F1 31 C3 33 73 F4 2C AF 61 75 BE 66 6C D9 CD 56 70 13 BD BE 18 87 2C 24 4C D9 67 A8 AE 2C 27 29 29 89 33 67 4F 93 98 98 48 CA A9 33 24 27 27 93 FE F5 55 26 1E AD 30 EA 7C C5 88 E4 65 FC B9 9F 0E FB 63 82 A1 2D C5 17 B7 EB C9 AE 84 B2 AB 29 9C 38 71 92 C4 84 78 D2 BE BA C2 A9 94 64 E2 E3 3F 21 BF AC 82 BA BB 7F D1 34 FA 90 A9 17 AF 99 F7 06 E9 9A 5E 54 3C D1 17 95 29 9D A9 FA 26 3D 95 F8 4F E3 38 7C E8 00 17 CE 5D E2 F9 8B 7F A9 31 5D 47 A3 51 13 FB 71 1C E7 3E 3F CF EC 7A 80 65 AF 9F 85 35 2F F6 A9 7E 7C FE 0D 21 41 8F EC 72 A0 6A 6E BE CD 0F A9 67 39 78 F8 10 B1 C7 3F A2 BA BA 8A F3 17 2E A2 7E 7F 3F 05 FA 3C 6A 84 2F DD B6 79 BA 1F CC 61 1B EF 66 7B D3 83 75 60 10 63 61 B6 72 2F 54 E1 34 4A B5 5F B2 2F 32 8A C8 7D 51 44 BC BB 87 C8 C8 48 22 22 22 C8 CA FC 96 A7 2F 9F 11 08 F9 09 6C 6F E1 F5 6F 62 BD 3F C0 CF A5 B9 6C 9B DF A1 23 F7 08 AA 7F 96 57 39 9D 94 C0 91 98 03 9C 4C 88 15 9B DF E3 F8 31 0D 29 9F C5 B1 77 6F 14 9D AD 2D 14 E9 CB 84 E6 12 0A F3 4B A8 2A C8 64 C7 FC 16 98 DF A6 4D 27 08 9C A2 14 E1 4A D8 6C 36 EC F6 49 8E 1E FD 10 A3 D1 C8 D2 D2 12 43 43 23 A2 23 1D 4A CB 4A 92 4B B9 E6 92 48 BB 3D 2F 86 B6 DC 63 E4 17 6B F9 0F 5C 44 06 C8 F5 A5 AF E1 00 00 00 00 49 45 4E 44 AE 42 60 82";
        public static void InitMenu(ref Infragistics.Win.UltraWinToolbars.UltraToolbarsManager TargetToolBarManager, DataTable dtMenu, int LanguageNumber)
        {
            int iMnuOrder = 0;
            string strCaption = string.Format("CAPTION00{0}", LanguageNumber);
            try
            {
                DataRow[] drs = dtMenu.Select("POPUP_MENU = 'ROOT'", "MNU_ORDER");
                DataRow[] drs_sub = dtMenu.Select("POPUP_MENU <> 'ROOT'", "MNU_ORDER, MENU_KEY");
                foreach (DataRow dr in drs)
                {
                    iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

                    if (dr["MENU_KEY"].ToString().Length > 0 && TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString()) < 0)
                    {
                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupMenu = null;

                        pupMenu = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(dr["MENU_KEY"].ToString());
                        pupMenu.CustomizedCaption = dr[strCaption].ToString();
                        pupMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());

                        TargetToolBarManager.Tools.Add(pupMenu);
                        TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools.AddTool(dr["MENU_KEY"].ToString());
                    }
                }

                foreach (DataRow dr in drs_sub)
                {
                    iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

                    if (dr["MENU_KEY"].ToString().Length > 0 && TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString()) < 0)
                    {
                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupMenu = null;

                        Infragistics.Win.UltraWinToolbars.ButtonTool butMenu = new Infragistics.Win.UltraWinToolbars.ButtonTool(dr["MENU_KEY"].ToString());
                        butMenu.CustomizedCaption = dr[strCaption].ToString();

                        switch (dr["ICON32"].ToString())
                        {
                            case "ICO_FILE_NEW_PROJECT":
                                butMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(ICO_FILE_NEW_PROJECT);
                                break;
                            default:
                                if (dr["ICON32"].ToString().Length != 0)
                                    butMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
                                break;
                        }


                        TargetToolBarManager.Tools.Add(butMenu);

                        if (dr["STOCK_TOOLBAR"].ToString().Trim().Length > 0)
                        {
                            if (TargetToolBarManager.Toolbars.IndexOf(dr["STOCK_TOOLBAR"].ToString()) < 0)
                            {
                                TargetToolBarManager.Toolbars.AddToolbar(dr["STOCK_TOOLBAR"].ToString());
                            }

                            //if (dr["ICON32"].ToString().Length != 0)
                            //{
                            TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].Tools.AddTool(dr["MENU_KEY"].ToString());
                            TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].DockedRow = 0;
                            //}

                        }

                        pupMenu = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[dr["POPUP_MENU"].ToString()];
                        pupMenu.Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());
                        if (dr["RESV_02"].ToString() == "BEGINE")
                            pupMenu.Tools[dr["MENU_KEY"].ToString()].CustomizedIsFirstInGroup = Infragistics.Win.DefaultableBoolean.True;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void InitMenu(ref Infragistics.Win.UltraWinToolbars.UltraToolbarsManager TargetToolBarManager, DataTable dtMenu, int LanguageNumber, int type = 0)
        {
            if (type != 1)
            {
                InitMenu(ref TargetToolBarManager, dtMenu, LanguageNumber);
                return;
            }

            int iMnuOrder = 0;
            string strCaption = string.Format("CAPTION00{0}", LanguageNumber);
            try
            {
                DataRow[] drs = dtMenu.Select("POPUP_MENU = 'ROOT'", "MNU_ORDER");
                DataRow[] drs_sub = dtMenu.Select("RESV_05 = 'SUB_MENU'", "MNU_ORDER");
                DataRow[] drs_menu = dtMenu.Select("POPUP_MENU <> 'ROOT' and RESV_05 <> 'SUB_MENU'", "POPUP_MENU, MNU_ORDER");

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("MENU_KEY", typeof(string)));
                dt.Columns.Add(new DataColumn("POPUP_MENU", typeof(string)));
                dt.Columns.Add(new DataColumn("MNU_ORDER", typeof(string)));

                foreach (DataRow dr in drs)
                {
                    iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

                    if (dr["MENU_KEY"].ToString().Length > 0 && TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString()) < 0)
                    {
                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupMenu = null;

                        pupMenu = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(dr["MENU_KEY"].ToString());
                        pupMenu.CustomizedCaption = dr[strCaption].ToString();
                        pupMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());

                        TargetToolBarManager.Tools.Add(pupMenu);
                        TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());
                    }
                }

                foreach (DataRow dr in drs_menu)
                {
                    iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

                    if (dr["MENU_KEY"].ToString().Length > 0 && TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString()) < 0)
                    {
                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupMenu1 = null;

                        Infragistics.Win.UltraWinToolbars.ButtonTool butMenu = new Infragistics.Win.UltraWinToolbars.ButtonTool(dr["MENU_KEY"].ToString());
                        butMenu.CustomizedCaption = dr[strCaption].ToString();

                        switch (dr["ICON32"].ToString())
                        {
                            case "ICO_FILE_NEW_PROJECT":
                                butMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(ICO_FILE_NEW_PROJECT);
                                break;
                            default:
                                if (dr["ICON32"].ToString().Length != 0)
                                    butMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
                                break;
                        }

                        TargetToolBarManager.Tools.Add(butMenu);

                        if (dr["STOCK_TOOLBAR"].ToString().Trim().Length > 0)
                        {
                            if (TargetToolBarManager.Toolbars.IndexOf(dr["STOCK_TOOLBAR"].ToString()) < 0)
                            {
                                TargetToolBarManager.Toolbars.AddToolbar(dr["STOCK_TOOLBAR"].ToString());
                            }
                            TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].Tools.AddTool(dr["MENU_KEY"].ToString());
                            TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].DockedRow = 0;
                        }

                        int n = 0;

                        if (TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools.IndexOf(dr["POPUP_MENU"].ToString()) < 0)
                        {
                            foreach (DataRow drSub in drs_sub)
                            {
                                if (drSub["MENU_KEY"].ToString() == dr["POPUP_MENU"].ToString())
                                {
                                    if (TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[drSub["POPUP_MENU"].ToString()].ToolbarsManager.Tools.IndexOf(drSub["MENU_KEY"].ToString()) < 0)
                                    {

                                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupSubMenu = null;
                                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupSubMenu2 = null;

                                        pupSubMenu = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(drSub["MENU_KEY"].ToString());
                                        pupSubMenu.CustomizedCaption = drSub[strCaption].ToString();
                                        pupSubMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(drSub["ICON32"].ToString());

                                        TargetToolBarManager.Tools.Add(pupSubMenu);

                                        pupSubMenu2 = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[drSub["POPUP_MENU"].ToString()];
                                        pupSubMenu2.Tools.InsertTool(iMnuOrder, drSub["MENU_KEY"].ToString());
                                    }
                                    pupMenu1 = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[drSub["POPUP_MENU"].ToString()].ToolbarsManager.Tools[dr["POPUP_MENU"].ToString()];
                                    n = 1;
                                    break;
                                }
                            }
                        }
                        if (n == 0)
                            pupMenu1 = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[dr["POPUP_MENU"].ToString()];
                        pupMenu1.Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());

                        if (dr["RESV_02"].ToString() == "BEGINE")
                            pupMenu1.Tools[dr["MENU_KEY"].ToString()].CustomizedIsFirstInGroup = Infragistics.Win.DefaultableBoolean.True;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void InitMenuRibbon(ref Infragistics.Win.UltraWinToolbars.UltraToolbarsManager TargetToolBarManager, DataTable dtMenu, int LanguageNumber, int type = 0)
        {
            if (type != 1)
            {
                InitMenu(ref TargetToolBarManager, dtMenu, LanguageNumber);
                return;
            }

            int iMnuOrder = 0;
            string strCaption = string.Format("CAPTION00{0}", LanguageNumber);
            try
            {
                DataRow[] drs = dtMenu.Select("POPUP_MENU = 'ROOT'", "MNU_ORDER");
                DataRow[] drs_sub = dtMenu.Select("RESV_05 = 'SUB_MENU'", "MNU_ORDER");
                DataRow[] drs_menu = dtMenu.Select("POPUP_MENU <> 'ROOT' and RESV_05 <> 'SUB_MENU'", "POPUP_MENU, MNU_ORDER");

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("MENU_KEY", typeof(string)));
                dt.Columns.Add(new DataColumn("POPUP_MENU", typeof(string)));
                dt.Columns.Add(new DataColumn("MNU_ORDER", typeof(string)));

                foreach (DataRow dr in drs)
                {
                    iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

                    if (dr["MENU_KEY"].ToString().Length > 0 && TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString()) < 0)
                    {
                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupMenu = null;

                        pupMenu = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(dr["MENU_KEY"].ToString());
                        pupMenu.CustomizedCaption = dr[strCaption].ToString();
                        pupMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());

                        TargetToolBarManager.Tools.Add(pupMenu);
                        TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());

                        /// Ribbon
                        /// 
                        //TargetToolBarManager.Ribbon.Tabs.Add(dr["MENU_KEY"].ToString());
                        TargetToolBarManager.Ribbon.Tabs.Insert(iMnuOrder, dr["MENU_KEY"].ToString());
                        TargetToolBarManager.Ribbon.Tabs[dr["MENU_KEY"].ToString()].Caption = dr[strCaption].ToString();
                    }
                    //else if (dr["MENU_KEY"].ToString() == "MNU_FILE")
                    //{
                    //    /// Ribbon
                    //    /// 
                    //    TargetToolBarManager.Ribbon.Tabs.Insert(iMnuOrder, dr["MENU_KEY"].ToString());
                    //    TargetToolBarManager.Ribbon.Tabs[dr["MENU_KEY"].ToString()].Caption = dr[strCaption].ToString();
                    //}
                }

                foreach (DataRow dr in drs_menu)
                {
                    iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

                    if (dr["MENU_KEY"].ToString().Length > 0 && TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString()) < 0)
                    {
                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupMenu1 = null;
                        Infragistics.Win.UltraWinToolbars.RibbonGroup pupGroup1 = null;

                        Infragistics.Win.UltraWinToolbars.ButtonTool butMenu = new Infragistics.Win.UltraWinToolbars.ButtonTool(dr["MENU_KEY"].ToString());
                        butMenu.CustomizedCaption = dr[strCaption].ToString();

                        switch (dr["ICON16"].ToString())
                        {
                            case "ICO_FILE_NEW_PROJECT":
                                butMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(ICO_FILE_NEW_PROJECT);
                                break;
                            default:
                                if (dr["ICON32"].ToString().Length != 0)
                                    butMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());
                                break;
                        }

                        TargetToolBarManager.Tools.Add(butMenu);

                        if (dr["STOCK_TOOLBAR"].ToString().Trim().Length > 0)
                        {
                            if (TargetToolBarManager.Toolbars.IndexOf(dr["STOCK_TOOLBAR"].ToString()) < 0)
                            {
                                TargetToolBarManager.Toolbars.AddToolbar(dr["STOCK_TOOLBAR"].ToString());
                            }
                            TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].Tools.AddTool(dr["MENU_KEY"].ToString());
                            TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].DockedRow = 0;
                        }

                        int n = 0;

                        if (TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools.IndexOf(dr["POPUP_MENU"].ToString()) < 0)
                        {
                            foreach (DataRow drSub in drs_sub)
                            {
                                if (drSub["MENU_KEY"].ToString() == dr["POPUP_MENU"].ToString())
                                {
                                    if (TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[drSub["POPUP_MENU"].ToString()].ToolbarsManager.Tools.IndexOf(drSub["MENU_KEY"].ToString()) < 0)
                                    {

                                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupSubMenu = null;
                                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupSubMenu2 = null;

                                        pupSubMenu = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(drSub["MENU_KEY"].ToString());
                                        pupSubMenu.CustomizedCaption = drSub[strCaption].ToString();
                                        pupSubMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(drSub["ICON16"].ToString());

                                        TargetToolBarManager.Tools.Add(pupSubMenu);

                                        //Ribbon - Sub Popup Menu
                                        TargetToolBarManager.Ribbon.Tabs[drSub["POPUP_MENU"].ToString()].Groups.Add(drSub["MENU_KEY"].ToString());
                                        TargetToolBarManager.Ribbon.Tabs[drSub["POPUP_MENU"].ToString()].Groups[drSub["MENU_KEY"].ToString()].Caption = drSub[strCaption].ToString();

                                        pupSubMenu2 = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[drSub["POPUP_MENU"].ToString()];
                                        pupSubMenu2.Tools.InsertTool(iMnuOrder, drSub["MENU_KEY"].ToString());

                                    }
                                    pupMenu1 = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[drSub["POPUP_MENU"].ToString()].ToolbarsManager.Tools[dr["POPUP_MENU"].ToString()];

                                    //Ribbon - Sub Popup Menu
                                    //if (TargetToolBarManager.Ribbon.Tabs[dr["POPUP_MENU"].ToString()].Groups.IndexOf(drSub["MENU_KEY"].ToString()) < 0)
                                    //{
                                    //    TargetToolBarManager.Ribbon.Tabs[dr["POPUP_MENU"].ToString()].Groups.Add(drSub["MENU_KEY"].ToString());
                                    //}
                                    pupGroup1 = TargetToolBarManager.Ribbon.Tabs[drSub["POPUP_MENU"].ToString()].Groups[drSub["MENU_KEY"].ToString()];
                                    n = 1;
                                    break;
                                }
                            }
                        }
                        if (n == 0)
                        {
                            pupMenu1 = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[dr["POPUP_MENU"].ToString()];

                            if (TargetToolBarManager.Ribbon.Tabs[dr["POPUP_MENU"].ToString()].Groups.IndexOf(dr["POPUP_MENU"].ToString()) < 0)
                            {
                                TargetToolBarManager.Ribbon.Tabs[dr["POPUP_MENU"].ToString()].Groups.Add(dr["POPUP_MENU"].ToString());
                                TargetToolBarManager.Ribbon.Tabs[dr["POPUP_MENU"].ToString()].Groups[dr["POPUP_MENU"].ToString()].Caption = dr[strCaption].ToString();
                            }
                            pupGroup1 = TargetToolBarManager.Ribbon.Tabs[dr["POPUP_MENU"].ToString()].Groups[dr["POPUP_MENU"].ToString()];

                            ///// Ribbon->AddGroup
                            ///// 
                            //TargetToolBarManager.Ribbon.Tabs[dr["POPUP_MENU"].ToString()].Groups.Add(dr["MENU_KEY"].ToString());
                            //pupGroup1 = TargetToolBarManager.Ribbon.Tabs[dr["POPUP_MENU"].ToString()].Groups[dr["MENU_KEY"].ToString()];
                            //pupGroup1.Caption = dr[strCaption].ToString();
                        }

                        pupMenu1.Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());
                        pupGroup1.Tools.AddTool(dr["MENU_KEY"].ToString());

                        if (dr["RESV_02"].ToString() == "BEGINE")
                            pupMenu1.Tools[dr["MENU_KEY"].ToString()].CustomizedIsFirstInGroup = Infragistics.Win.DefaultableBoolean.True;
                        //}
                    }
                }
                TargetToolBarManager.Ribbon.IsMinimized = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
