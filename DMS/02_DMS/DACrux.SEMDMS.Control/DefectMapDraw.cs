using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.Common.RO;
using DACrux.Map;
using DACrux.SEMDMS.RO;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// DefectMapDraw에 대한 요약 설명입니다.
    /// </summary>
    public static class DefectMapDraw
    {
        public static readonly string FAB1 = "FAB1";
        public static readonly string FAB2 = "FAB2";

        public static void Draw(DefectMap map, long stepSeq)
        {
            try
            {
                if (map.InvokeRequired)
                {
                    map.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        Draw(map, stepSeq);
                    }
                    ));
                }
                else
                {
                    long setupSeq = -1;

                    setupSeq = SetupSeq(stepSeq);
                    if (setupSeq == -1) return;
                    Recipe(setupSeq, map);
                    SetupMap(stepSeq, map);
                    DefectData(stepSeq, map);
                    AddInfo(stepSeq, map);
                    map.WaferDrawMode = DACrux.Map.MapMode.Fit;

                    //Map 선택이 Defect Select 가 최우선 순위로 바꿔 준다.
                    map.DefectSelectMode();

                    map.Redraw();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void DrawDefect(DefectMap map, long stepSeq, ref List<Defect> oDefect)
        {
            try
            {
                long setupSeq = -1;

                setupSeq = SetupSeq(stepSeq);
                if (setupSeq == -1) return;
                Recipe(setupSeq, map);
                //ScanSample(stepSeq, map);
                SetupMap(stepSeq, map);
                DefectData(stepSeq, map);
                oDefect = map.Defects;

                AddInfo(stepSeq, map);
                map.WaferDrawMode = DACrux.Map.MapMode.Fit;

                //Map 선택이 Defect Select 가 최우선 순위로 바꿔 준다.
                map.DefectSelectMode();

                map.Redraw();

                oDefect = map.Defects;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AddInfo(long stepSeq, DefectMap map)
        {
            DefectMapAnalysis oDMapAnalysis = null;
            DataTable dt = null;
            try
            {
                oDMapAnalysis = new DefectMapAnalysis();
                dt = oDMapAnalysis.GetStepInfo(new long[] { stepSeq });
                DataRow[] drs = dt.Select();
                map.AddInfomation(string.Format("LID:{0}", drs[0]["LOT_ID"].ToString()));
                map.AddInfomation(string.Format("WID:{0}", drs[0]["WAFER_ID"].ToString().Replace(drs[0]["LOT_ID"].ToString(), "").Replace("-", "")));
                map.AddInfomation(string.Format("LAYER:{0}", drs[0]["STEP_ID"].ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
                oDMapAnalysis = null;
            }
        }

        public static void Draw(DefectMap map, long[] stepSeq, int tolerance, ref DataTable dtDefect)
        {
            try
            {
                long setupSeq = -1;

                if (stepSeq.Length == 1) setupSeq = SetupSeq(stepSeq[0]);
                else setupSeq = SetupSeq(stepSeq[1]);
                Recipe(setupSeq, map);
                //if (stepSeq.Length == 1) ScanSample(stepSeq[0], map);
                //else ScanSample(stepSeq[1], map);

                if (stepSeq.Length == 1) SetupMap(stepSeq[0], map);
                else SetupMap(stepSeq[1], map);

                DefectData(stepSeq, map, tolerance, ref dtDefect);
                map.WaferDrawMode = DACrux.Map.MapMode.Fit;

                //Map 선택이 Defect Select 가 최우선 순위로 바꿔 준다.
                map.DefectSelectMode();

                map.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Draw(DefectMap map, long stepSeq, int tolerance, ref DataTable defect
            , bool isShot, string product, int dieSizeX, int dieSizeY)
        {
            try
            {
                long setupSeq = -1;
                setupSeq = SetupSeq(stepSeq);
                if (setupSeq == -1) return;
                Recipe(setupSeq, map);
                //ScanSample(stepSeq, map);
                SetupMap(stepSeq, map);
                DefectData(stepSeq, map, tolerance, ref defect, isShot, product, dieSizeX, dieSizeY);
                map.WaferDrawMode = DACrux.Map.MapMode.Fit;

                //Map 선택이 Defect Select 가 최우선 순위로 바꿔 준다.
                map.DefectSelectMode();

                map.Redraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DrawNoRedraw(DefectMap map, long stepSeq, ref DataTable defect)
        {
            try
            {
                long setupSeq = -1;

                setupSeq = SetupSeq(stepSeq);
                if (setupSeq == -1) return;
                Recipe(setupSeq, map);
                //ScanSample(stepSeq, map);
                SetupMap(stepSeq, map);
                DefectData(stepSeq, map, ref defect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static long SetupSeq(long stepSeq)
        {
            DefectMapAnalysis oDMapAnalysis = null;
            DataTable dt = null;
            try
            {
                oDMapAnalysis = new DefectMapAnalysis();
                dt = oDMapAnalysis.GetStepInfo(new long[] { stepSeq });

                if (dt.Rows.Count == 0) return -1;
                return DACrux.Base.Convert.longParse(dt.Rows[0]["SETUP_SEQ"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// ScanSample 대체 한다. Setup Seq 별로 Map 정의
        /// </summary>
        public static void SetupMap(long StepSeq, DefectMap map)
        {
            DefectMapAnalysis oDMapAnalysis = null;
            ComConfiguration oComConfig = null;
            DataTable dt = null;
            try
            {
                oDMapAnalysis = new DefectMapAnalysis();
                oComConfig = new ComConfiguration();

                long setupSeq = -1;
                setupSeq = SetupSeq(StepSeq);
                if (setupSeq == -1) return;

                dt = oDMapAnalysis.GetSetupMap(setupSeq);
                DataRow[] drs = dt.Select();

                map.DieClear();

                //Virture Die 에 대한 Information Set
                DataTable dtVir = oComConfig.GetConfigUser(
                  DACrux.Base.GlobalVariable.Factory,
                  "VIRTUAL_OPTION",
                  DACrux.Base.GlobalVariable.UserID
                  );

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

                foreach (DataRow dr in drs)
                {
                    map.AddDie(new Die(DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString())
                        , DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString())
                        , DACrux.Base.Convert.intParse(dr["TEST"].ToString())
                        , 1));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static void Recipe(long setupSeq, DefectMap map)
        {
            DefectMapAnalysis oDMapAnalysis = null;
            DataTable dt = null;
            try
            {
                oDMapAnalysis = new DefectMapAnalysis();
                dt = oDMapAnalysis.GetSetupRecipe(setupSeq);

                map.WaferSize = DACrux.Base.Convert.doubleParse(dt.Rows[0]["WAFER_SIZE"].ToString());
                map.NotchType = DACrux.Base.Notch.Notch; //dt.Rows[0]["NOTCH_TYPE"].ToString().Equals("F") ? Notch.Flat : Notch.Notch;
                map.AngleOffSet = 0;
                map.XYDirect = XYDirection.LeftBottom;
                map.NotchAngle = 0;// DOWN으로 저장하여 보여주므로 0으로 설정 DACrux.Base.Convert.intParse(dt.Rows[0]["ANGLE"].ToString());

                map.DieSizeX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["DIE_PITCH_X"].ToString());
                map.DieSizeY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["DIE_PITCH_Y"].ToString());
                map.OriginIndexX = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_ORIGIN_X"].ToString());
                map.OriginIndexY = DACrux.Base.Convert.intParse(dt.Rows[0]["DIE_ORIGIN_Y"].ToString());
                map.OriginX = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_X"].ToString());
                map.OriginY = DACrux.Base.Convert.doubleParse(dt.Rows[0]["ORIGIN_Y"].ToString());
                map.DieCalculation(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static void DefectData(long StepSeq, DefectMap map)
        {
            DefectMapAnalysis oDMapAnalysis = null;
            DataTable dt = null;
            try
            {
                oDMapAnalysis = new DefectMapAnalysis();
                dt = oDMapAnalysis.GetDefectData(StepSeq);

                map.DefectClear();
                foreach (DataRow dr in dt.Rows)
                {
                    Defect df = new Defect();
                    df.STEP_SEQ = DACrux.Base.Convert.intParse(dr["STEP_SEQ"].ToString());
                    df.DEFECTID = DACrux.Base.Convert.intParse(dr["DEFECTID"].ToString());
                    df.WAFER_SEQ = DACrux.Base.Convert.intParse(dr["WAFER_SEQ"].ToString());
                    df.X = DACrux.Base.Convert.doubleParse(dr["X"].ToString());
                    df.Y = DACrux.Base.Convert.doubleParse(dr["Y"].ToString());
                    df.XREL = DACrux.Base.Convert.doubleParse(dr["XREL"].ToString());
                    df.YREL = DACrux.Base.Convert.doubleParse(dr["YREL"].ToString());
                    df.XINDEX = DACrux.Base.Convert.intParse(dr["XINDEX"].ToString());
                    df.YINDEX = DACrux.Base.Convert.intParse(dr["YINDEX"].ToString());
                    df.XSIZE = DACrux.Base.Convert.doubleParse(dr["XSIZE"].ToString());
                    df.YSIZE = DACrux.Base.Convert.doubleParse(dr["YSIZE"].ToString());
                    df.DEFECTAREA = DACrux.Base.Convert.doubleParse(dr["DEFECTAREA"].ToString());
                    df.DSIZE = DACrux.Base.Convert.doubleParse(dr["DSIZE"].ToString());
                    df.CLASSNUMBER = DACrux.Base.Convert.intParse(dr["CLASSNUMBER"].ToString());
                    df.TEST = DACrux.Base.Convert.intParse(dr["TEST"].ToString());
                    df.CLUSTERNUMBER = DACrux.Base.Convert.intParse(dr["CLUSTERNUMBER"].ToString());
                    df.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(dr["ROUGHBINNUMBER"].ToString());
                    df.FINEBINNUMBER = DACrux.Base.Convert.intParse(dr["FINEBINNUMBER"].ToString());
                    df.REVIEWSAMPLE = DACrux.Base.Convert.intParse(dr["REVIEWSAMPLE"].ToString());
                    df.IMAGECOUNT = DACrux.Base.Convert.intParse(dr["IMAGECOUNT"].ToString());
                    df.ADDER = DACrux.Base.Convert.intParse(dr["ADDER"].ToString());
                    df.FIRST_STEP = dr["FIRST_STEP"].ToString();
                    df.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["RETICLE_REPEAT_ID"].ToString());
                    df.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["DIE_REPEAT_ID"].ToString());
                    df.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(dr["MAN_OPT_CLASS"].ToString());
                    df.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_OPT_CLASS"].ToString());
                    df.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(dr["MAN_SEM_CLASS"].ToString());
                    df.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_SEM_CLASS"].ToString());

                    if (dr.Table.Columns.IndexOf("IMAGE_PATH") > -1 && string.IsNullOrEmpty(dr["IMAGE_PATH"].ToString()) == false)
                    {
                        df.IMAGEURL = string.Format("{0}/{1}", dr["IMAGE_PATH"].ToString().Trim(), dr["IMAGE_FILENAME"].ToString().Trim()).Trim();
                    }

                    map.AddDefect(df);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        static void DefectData(long StepSeq, DefectMap map, bool isImage)
        {
            DefectMapAnalysis oDMapAnalysis = null;
            DataTable dt = null;
            try
            {
                oDMapAnalysis = new DefectMapAnalysis();
                dt = oDMapAnalysis.GetDefectData(StepSeq);

                map.DefectClear();
                foreach (DataRow dr in dt.Rows)
                {
                    Defect df = new Defect();
                    df.STEP_SEQ = DACrux.Base.Convert.intParse(dr["STEP_SEQ"].ToString());
                    df.DEFECTID = DACrux.Base.Convert.intParse(dr["DEFECTID"].ToString());
                    df.WAFER_SEQ = DACrux.Base.Convert.intParse(dr["WAFER_SEQ"].ToString());
                    df.X = DACrux.Base.Convert.doubleParse(dr["X"].ToString());
                    df.Y = DACrux.Base.Convert.doubleParse(dr["Y"].ToString());
                    df.XREL = DACrux.Base.Convert.doubleParse(dr["XREL"].ToString());
                    df.YREL = DACrux.Base.Convert.doubleParse(dr["YREL"].ToString());
                    df.XINDEX = DACrux.Base.Convert.intParse(dr["XINDEX"].ToString());
                    df.YINDEX = DACrux.Base.Convert.intParse(dr["YINDEX"].ToString());
                    df.XSIZE = DACrux.Base.Convert.doubleParse(dr["XSIZE"].ToString());
                    df.YSIZE = DACrux.Base.Convert.doubleParse(dr["YSIZE"].ToString());
                    df.DEFECTAREA = DACrux.Base.Convert.doubleParse(dr["DEFECTAREA"].ToString());
                    df.DSIZE = DACrux.Base.Convert.doubleParse(dr["DSIZE"].ToString());
                    df.CLASSNUMBER = DACrux.Base.Convert.intParse(dr["CLASSNUMBER"].ToString());
                    df.TEST = DACrux.Base.Convert.intParse(dr["TEST"].ToString());
                    df.CLUSTERNUMBER = DACrux.Base.Convert.intParse(dr["CLUSTERNUMBER"].ToString());
                    df.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(dr["ROUGHBINNUMBER"].ToString());
                    df.FINEBINNUMBER = DACrux.Base.Convert.intParse(dr["FINEBINNUMBER"].ToString());
                    df.REVIEWSAMPLE = DACrux.Base.Convert.intParse(dr["REVIEWSAMPLE"].ToString());
                    df.IMAGECOUNT = isImage ? DACrux.Base.Convert.intParse(dr["IMAGECOUNT"].ToString()) : 0;
                    df.ADDER = DACrux.Base.Convert.intParse(dr["ADDER"].ToString());
                    df.FIRST_STEP = dr["FIRST_STEP"].ToString();
                    df.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["RETICLE_REPEAT_ID"].ToString());
                    df.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["DIE_REPEAT_ID"].ToString());
                    df.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(dr["MAN_OPT_CLASS"].ToString());
                    df.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_OPT_CLASS"].ToString());
                    df.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(dr["MAN_SEM_CLASS"].ToString());
                    df.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_SEM_CLASS"].ToString());

                    if (dr.Table.Columns.IndexOf("IMAGE_PATH") > -1 && string.IsNullOrEmpty(dr["IMAGE_PATH"].ToString()) == false)
                    {
                        df.IMAGEURL = string.Format("{0}/{1}", dr["IMAGE_PATH"].ToString().Trim(), dr["IMAGE_FILENAME"].ToString().Trim()).Trim();
                    }

                    map.AddDefect(df);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        static void DefectData(long[] stepSeq, DefectMap map, int tolerance, ref DataTable dtDefect)
        {
            DefectMapAnalysis oDMapAnalysis = null;
            DataTable dt = null;
            try
            {
                oDMapAnalysis = new DefectMapAnalysis();
                if (stepSeq.Length == 1) dt = oDMapAnalysis.GetFirstStep(stepSeq[0]);
                else dt = oDMapAnalysis.GetDsa(stepSeq[0], stepSeq[1], tolerance);
                dtDefect = dt;

                map.DefectClear();
                foreach (DataRow dr in dtDefect.Rows)
                {
                    Defect df = new Defect();
                    df.STEP_SEQ = DACrux.Base.Convert.intParse(dr["STEP_SEQ"].ToString());
                    df.DEFECTID = DACrux.Base.Convert.intParse(dr["DEFECTID"].ToString());
                    df.WAFER_SEQ = DACrux.Base.Convert.intParse(dr["WAFER_SEQ"].ToString());
                    df.X = DACrux.Base.Convert.doubleParse(dr["X"].ToString());
                    df.Y = DACrux.Base.Convert.doubleParse(dr["Y"].ToString());
                    df.XREL = DACrux.Base.Convert.doubleParse(dr["XREL"].ToString());
                    df.YREL = DACrux.Base.Convert.doubleParse(dr["YREL"].ToString());
                    df.XINDEX = DACrux.Base.Convert.intParse(dr["XINDEX"].ToString());
                    df.YINDEX = DACrux.Base.Convert.intParse(dr["YINDEX"].ToString());
                    df.XSIZE = DACrux.Base.Convert.doubleParse(dr["XSIZE"].ToString());
                    df.YSIZE = DACrux.Base.Convert.doubleParse(dr["YSIZE"].ToString());
                    df.DEFECTAREA = DACrux.Base.Convert.doubleParse(dr["DEFECTAREA"].ToString());
                    df.DSIZE = DACrux.Base.Convert.doubleParse(dr["DSIZE"].ToString());
                    df.CLASSNUMBER = DACrux.Base.Convert.intParse(dr["CLASSNUMBER"].ToString());
                    df.TEST = DACrux.Base.Convert.intParse(dr["TEST"].ToString());
                    df.CLUSTERNUMBER = DACrux.Base.Convert.intParse(dr["CLUSTERNUMBER"].ToString());
                    df.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(dr["ROUGHBINNUMBER"].ToString());
                    df.FINEBINNUMBER = DACrux.Base.Convert.intParse(dr["FINEBINNUMBER"].ToString());
                    df.REVIEWSAMPLE = DACrux.Base.Convert.intParse(dr["REVIEWSAMPLE"].ToString());
                    df.IMAGECOUNT = DACrux.Base.Convert.intParse(dr["IMAGECOUNT"].ToString());
                    df.ADDER = DACrux.Base.Convert.intParse(dr["ADDER"].ToString());
                    df.FIRST_STEP = dr["FIRST_STEP"].ToString();
                    df.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["RETICLE_REPEAT_ID"].ToString());
                    df.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["DIE_REPEAT_ID"].ToString());
                    df.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(dr["MAN_OPT_CLASS"].ToString());
                    df.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_OPT_CLASS"].ToString());
                    df.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(dr["MAN_SEM_CLASS"].ToString());
                    df.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_SEM_CLASS"].ToString());
                    df.DSA = dr["NEW_CARRYOVER_MISSING"].ToString();

                    if (dr.Table.Columns.IndexOf("IMAGE_PATH") > -1 && string.IsNullOrEmpty(dr["IMAGE_PATH"].ToString()) == false)
                    {
                        df.IMAGEURL = string.Format("{0}/{1}", dr["IMAGE_PATH"].ToString().Trim(), dr["IMAGE_FILENAME"].ToString().Trim()).Trim();
                    }

                    map.AddDefect(df);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        static void DefectData(long StepSeq, DefectMap map, int tolerance, ref DataTable defect, bool isShot, string product, int dieSizeX, int dieSizeY)
        {
            DefectMapAnalysis oDMapAnalysis = null;
            DataTable dt = null;
            try
            {
                oDMapAnalysis = new DefectMapAnalysis();
                if (isShot)
                    dt = oDMapAnalysis.GetShotRepeatDefect(StepSeq, product, dieSizeX, dieSizeY, tolerance);
                else
                    dt = oDMapAnalysis.GetDieRepeatDefect(StepSeq, tolerance);
                defect = dt;

                map.DefectClear();
                foreach (DataRow dr in defect.Rows)
                {
                    Defect df = new Defect();
                    df.STEP_SEQ = DACrux.Base.Convert.intParse(dr["STEP_SEQ"].ToString());
                    df.DEFECTID = DACrux.Base.Convert.intParse(dr["DEFECTID"].ToString());
                    df.WAFER_SEQ = DACrux.Base.Convert.intParse(dr["WAFER_SEQ"].ToString());
                    df.X = DACrux.Base.Convert.doubleParse(dr["X"].ToString());
                    df.Y = DACrux.Base.Convert.doubleParse(dr["Y"].ToString());
                    df.XREL = DACrux.Base.Convert.doubleParse(dr["XREL"].ToString());
                    df.YREL = DACrux.Base.Convert.doubleParse(dr["YREL"].ToString());
                    df.XINDEX = DACrux.Base.Convert.intParse(dr["XINDEX"].ToString());
                    df.YINDEX = DACrux.Base.Convert.intParse(dr["YINDEX"].ToString());
                    df.XSIZE = DACrux.Base.Convert.doubleParse(dr["XSIZE"].ToString());
                    df.YSIZE = DACrux.Base.Convert.doubleParse(dr["YSIZE"].ToString());
                    df.DEFECTAREA = DACrux.Base.Convert.doubleParse(dr["DEFECTAREA"].ToString());
                    df.DSIZE = DACrux.Base.Convert.doubleParse(dr["DSIZE"].ToString());
                    df.CLASSNUMBER = DACrux.Base.Convert.intParse(dr["CLASSNUMBER"].ToString());
                    df.TEST = DACrux.Base.Convert.intParse(dr["TEST"].ToString());
                    df.CLUSTERNUMBER = DACrux.Base.Convert.intParse(dr["CLUSTERNUMBER"].ToString());
                    df.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(dr["ROUGHBINNUMBER"].ToString());
                    df.FINEBINNUMBER = DACrux.Base.Convert.intParse(dr["FINEBINNUMBER"].ToString());
                    df.REVIEWSAMPLE = DACrux.Base.Convert.intParse(dr["REVIEWSAMPLE"].ToString());
                    df.IMAGECOUNT = DACrux.Base.Convert.intParse(dr["IMAGECOUNT"].ToString());
                    df.ADDER = DACrux.Base.Convert.intParse(dr["ADDER"].ToString());
                    df.FIRST_STEP = dr["FIRST_STEP"].ToString();
                    df.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["RETICLE_REPEAT_ID"].ToString());
                    df.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["DIE_REPEAT_ID"].ToString());
                    df.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(dr["MAN_OPT_CLASS"].ToString());
                    df.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_OPT_CLASS"].ToString());
                    df.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(dr["MAN_SEM_CLASS"].ToString());
                    df.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_SEM_CLASS"].ToString());
                    df.REPEAT_XREL = DACrux.Base.Convert.intParse(dr["REPEAT_XREL"].ToString());
                    df.REPEAT_YREL = DACrux.Base.Convert.intParse(dr["REPEAT_YREL"].ToString());

                    if (dr.Table.Columns.IndexOf("IMAGE_PATH") > -1 && string.IsNullOrEmpty(dr["IMAGE_PATH"].ToString()) == false)
                    {
                        df.IMAGEURL = string.Format("{0}/{1}", dr["IMAGE_PATH"].ToString().Trim(), dr["IMAGE_FILENAME"].ToString().Trim()).Trim();
                    }

                    map.AddDefect(df);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        static void DefectData(long StepSeq, DefectMap map, ref DataTable defect)
        {
            DefectMapAnalysis oDMapAnalysis = null;
            DataTable dt = null;
            try
            {
                if (map.MapType != DACrux.Base.MAP_TYPE.REPEAT)
                {
                    oDMapAnalysis = new DefectMapAnalysis();
                    dt = oDMapAnalysis.GetDefectData(StepSeq);

                    defect = dt;
                }

                // defect 들을 넘겨주기만한다
                //dt = defect;en

                map.DefectClear();
                foreach (DataRow dr in defect.Rows)
                {
                    Defect df = new Defect();
                    df.STEP_SEQ = DACrux.Base.Convert.intParse(dr["STEP_SEQ"].ToString());
                    df.DEFECTID = DACrux.Base.Convert.intParse(dr["DEFECTID"].ToString());
                    df.WAFER_SEQ = DACrux.Base.Convert.intParse(dr["WAFER_SEQ"].ToString());
                    df.X = DACrux.Base.Convert.doubleParse(dr["X"].ToString());
                    df.Y = DACrux.Base.Convert.doubleParse(dr["Y"].ToString());
                    df.XREL = DACrux.Base.Convert.doubleParse(dr["XREL"].ToString());
                    df.YREL = DACrux.Base.Convert.doubleParse(dr["YREL"].ToString());
                    df.XINDEX = DACrux.Base.Convert.intParse(dr["XINDEX"].ToString());
                    df.YINDEX = DACrux.Base.Convert.intParse(dr["YINDEX"].ToString());
                    df.XSIZE = DACrux.Base.Convert.doubleParse(dr["XSIZE"].ToString());
                    df.YSIZE = DACrux.Base.Convert.doubleParse(dr["YSIZE"].ToString());
                    df.DEFECTAREA = DACrux.Base.Convert.doubleParse(dr["DEFECTAREA"].ToString());
                    df.DSIZE = DACrux.Base.Convert.doubleParse(dr["DSIZE"].ToString());
                    df.CLASSNUMBER = DACrux.Base.Convert.intParse(dr["CLASSNUMBER"].ToString());
                    df.TEST = DACrux.Base.Convert.intParse(dr["TEST"].ToString());
                    df.CLUSTERNUMBER = DACrux.Base.Convert.intParse(dr["CLUSTERNUMBER"].ToString());
                    df.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(dr["ROUGHBINNUMBER"].ToString());
                    df.FINEBINNUMBER = DACrux.Base.Convert.intParse(dr["FINEBINNUMBER"].ToString());
                    df.REVIEWSAMPLE = DACrux.Base.Convert.intParse(dr["REVIEWSAMPLE"].ToString());
                    df.IMAGECOUNT = DACrux.Base.Convert.intParse(dr["IMAGECOUNT"].ToString());
                    df.ADDER = DACrux.Base.Convert.intParse(dr["ADDER"].ToString());
                    df.FIRST_STEP = dr["FIRST_STEP"].ToString();
                    df.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["RETICLE_REPEAT_ID"].ToString());
                    df.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["DIE_REPEAT_ID"].ToString());
                    df.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(dr["MAN_OPT_CLASS"].ToString());
                    df.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_OPT_CLASS"].ToString());
                    df.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(dr["MAN_SEM_CLASS"].ToString());
                    df.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_SEM_CLASS"].ToString());

                    //try catch 로 하면 속도가 너무 느리다.. 
                    if (dr.Table.Columns.IndexOf("REPEAT_XREL") > -1)
                        df.REPEAT_XREL = DACrux.Base.Convert.intParse(dr["REPEAT_XREL"].ToString());

                    if (dr.Table.Columns.IndexOf("REPEAT_YREL") > -1)
                        df.REPEAT_YREL = DACrux.Base.Convert.intParse(dr["REPEAT_YREL"].ToString());

                    if (dr.Table.Columns.IndexOf("IMAGE_PATH") > -1 && string.IsNullOrEmpty(dr["IMAGE_PATH"].ToString()) == false)
                    {
                        df.IMAGEURL = string.Format("{0}/{1}", dr["IMAGE_PATH"].ToString().Trim(), dr["IMAGE_FILENAME"].ToString().Trim()).Trim();
                    }

                    map.AddDefect(df);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public static void Density(long StepSeq, DefectMap map, string SelDefects = "ALL")
        {
            DefectMapAnalysis oDMapAnalysis = null;
            DACrux.Common.RO.ComConfiguration oComConfig = null;
            DataTable dt = null;
            try
            {
                oDMapAnalysis = new DefectMapAnalysis();
                oComConfig = new DACrux.Common.RO.ComConfiguration();

                dt = oDMapAnalysis.GetDensity(new long[] { StepSeq }, SelDefects);
                DataRow[] drs = dt.Select();

                map.DieClear();

                //Virture Die 에 대한 Information Set
                DataTable dtVir = oComConfig.GetConfigUser(
                  DACrux.Base.GlobalVariable.Factory,
                  "VIRTUAL_OPTION",
                  DACrux.Base.GlobalVariable.UserID
                  );

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

                foreach (DataRow dr in drs)
                {
                    map.AddDie(new Die(DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString())
                        , DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString())
                        , DACrux.Base.Convert.intParse(dr["DEFECT_CNT"].ToString())
                        , 3));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
                oDMapAnalysis = null;
            }
        }

        public static string[] GetMapDescription(DefectList defectList, string[] strViewItem = null)
        {
            if (defectList == null || strViewItem == null || strViewItem.Length <= 0)
                return new string[] { };

            List<string> lotList = new List<string>();
            List<string> wafList = new List<string>();
            List<string> arrinspection_eq = new List<string>();
            List<string> arrproduct = new List<string>();
            List<string> arrslot = new List<string>();
            List<string> arrstepid = new List<string>();
            List<string> arrstrSetup = new List<string>();
            List<string> arrDefectiveDiePercentages = new List<string>();
            string inspectionTime = null;

            foreach (Defect d in defectList)
            {
                string lotID = DmsCache.Instance[d.STEP_SEQ].StepInfo.LotID;
                string waferID = DmsCache.Instance[d.STEP_SEQ].StepInfo.WaferID;
                string time = DmsCache.Instance[d.STEP_SEQ].StepInfo.ResultTimestamp;
                string inspection_eq = DmsCache.Instance[d.STEP_SEQ].StepInfo.Equip;
                string product = DmsCache.Instance[d.STEP_SEQ].StepInfo.DeviceID;
                string slot = DmsCache.Instance[d.STEP_SEQ].StepInfo.Slot.ToString();
                string stepid = DmsCache.Instance[d.STEP_SEQ].StepInfo.StepID;
                string strSetup = DmsCache.Instance[d.STEP_SEQ].StepInfo.SetupID;


                if (!lotList.Contains(lotID))
                    lotList.Add(lotID);

                if (!wafList.Contains(waferID))
                    wafList.Add(waferID);

                if (String.Compare(inspectionTime, time) < 0)
                    inspectionTime = time;

                if (!arrinspection_eq.Contains(inspection_eq))
                    arrinspection_eq.Add(inspection_eq);

                if (!arrproduct.Contains(product))
                    arrproduct.Add(product);

                if (!arrslot.Contains(slot))
                    arrslot.Add(slot);

                if (!arrstepid.Contains(stepid))
                    arrstepid.Add(stepid);

                if (!arrstrSetup.Contains(strSetup))
                    arrstrSetup.Add(strSetup);
            }

            List<string> result = new List<string>();

            if (Array.IndexOf(strViewItem, "LOT_ID") > -1)
            {
                if (lotList.Count > 0)
                    result.Add(String.Format("Lot ID: {0}", String.Join(", ", lotList.ToArray())));
            }

            if (Array.IndexOf(strViewItem, "WAFER_ID") > -1)
            {
                if (wafList.Count > 0) 
                    result.Add(String.Format("Wafer ID: {0}", String.Join(", ", wafList.ToArray())));
            }

            if (Array.IndexOf(strViewItem, "RESULTTIMESTAMP") > -1)
            {
                result.Add(String.Format("Time: {0}", inspectionTime));
            }

            if (Array.IndexOf(strViewItem, "INSPECTION_EQ") > -1)
            {
                if (arrinspection_eq.Count > 0)
                    result.Add(String.Format("Equip : {0}", String.Join(", ", arrinspection_eq.ToArray())));
            }

            if (Array.IndexOf(strViewItem, "PRODUCT") > -1)
            {
                if (arrproduct.Count > 0)
                    result.Add(String.Format("Device : {0}", String.Join(", ", arrproduct.ToArray())));
            }

            if (Array.IndexOf(strViewItem, "SLOT_ID") > -1)
            {
                if (arrslot.Count > 0)
                    result.Add(String.Format("Slot : {0}", String.Join(", ", arrslot.ToArray())));
            }

            if (Array.IndexOf(strViewItem, "STEP_ID") > -1)
            {
                if (arrstepid.Count > 0)
                    result.Add(String.Format("StepID : {0}", String.Join(", ", arrstepid.ToArray())));
            }

            if (Array.IndexOf(strViewItem, "SETUP_ID") > -1)
            {
                if (arrstrSetup.Count > 0)
                    result.Add(String.Format("SetupID : {0}", String.Join(", ", arrstrSetup.ToArray())));
            }

            return result.ToArray();
        }

        /// <summary>
        /// FTP 서버 정보를 나타냅니다.
        /// </summary>
        public static class FtpServerInfo
        {
            public static bool HasValue()
            {
                return !String.IsNullOrEmpty(IP);
            }

            public static string IP;
            public static string Port;
            public static string UserID;
            public static string Password;
            public static string SubPath;
        }

        /// <summary>
        /// FTP에서 파일을 Stream으로 가져옵니다.
        /// </summary>
        public static Stream fnFTPGetStream(string strFTPDirectory)
        {
            if (!FtpServerInfo.HasValue())
            {
                ComConfiguration obj = new ComConfiguration();
                DataTable dtConfig = obj.GetDefectFTP();

                if (dtConfig == null || dtConfig.Rows.Count == 0)
                    throw new Exception("Not found FTP Server Info");

                FtpServerInfo.IP = dtConfig.Rows[0]["IP"].ToString();
                FtpServerInfo.Port = dtConfig.Rows[0]["PORT"].ToString();
                FtpServerInfo.UserID = dtConfig.Rows[0]["ID"].ToString();
                FtpServerInfo.Password = dtConfig.Rows[0]["PASS"].ToString();
                FtpServerInfo.SubPath = dtConfig.Rows[0]["MAIN_PATH"].ToString();
            }
            /* 260223 Chris */
            //string fullPath = String.Format("ftp://{0}:{1}/{2}/{3}", FtpServerInfo.IP, FtpServerInfo.Port, FtpServerInfo.SubPath.Trim('/'), strFTPDirectory.Trim('/'));
            string fullPath = strFTPDirectory.Trim('/');
            return DownloadToStream(fullPath, FtpServerInfo.UserID, FtpServerInfo.Password);
        }

        private static Stream DownloadToStream(string ftpPath, string id, string password)
        {

            WebRequest req = WebRequest.Create(ftpPath);

            if (req is HttpWebRequest)
                req.Method = WebRequestMethods.Http.Get;
            else if (req is FtpWebRequest)
                req.Method = WebRequestMethods.Ftp.DownloadFile;
            else if (req is FileWebRequest)
                req.Method = WebRequestMethods.File.DownloadFile;

            if (!String.IsNullOrWhiteSpace(id))
                req.Credentials = new NetworkCredential(id, password);

            try
            {
                WebResponse rep = req.GetResponse();
                return rep.GetResponseStream();
            }
            catch
            {
                return null;
            }
        }

        public static FileInfo fnFTPImageDownload(string strFTPDirctory)
        {
            //DACrux.Utility.ServerCommunicationFtp oFTP = null;
            DACrux.Utility.HFtpClient oFTP = null;
            string strLocalFullPath = string.Empty;

            string strFTPFullPath = string.Empty;
            string strLocalPathName = string.Empty;

            string strFTPIP = string.Empty;
            string strFTPPort = string.Empty;
            string strFTPID = string.Empty;
            string strFTPPass = string.Empty;
            string strFTPPath = "/%2f/images";

            FileInfo oDefectFile = null;
            DirectoryInfo oDirectory = null;

            ComConfiguration oConfig = null;

            try
            {
                oConfig = new ComConfiguration();
                DataTable dtConfig = oConfig.GetDefectFTP();
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                    return null;

                strFTPIP = dtConfig.Rows[0]["IP"].ToString();
                strFTPPort = dtConfig.Rows[0]["PORT"].ToString();
                strFTPID = dtConfig.Rows[0]["ID"].ToString();
                strFTPPass = dtConfig.Rows[0]["PASS"].ToString();
                strFTPPath = dtConfig.Rows[0]["MAIN_PATH"].ToString();

                string[] ArrImage = strFTPDirctory.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                //oFTP = new Utility.ServerCommunicationFtp();
                //oFTP.Server = strFTPIP;
                //oFTP.Port = DACrux.Base.Convert.intParse(strFTPPort);
                //oFTP.UserID = strFTPID;
                //oFTP.Password = strFTPPass;
                //oFTP.ChmodValue = 777;

                oFTP = new DACrux.Utility.HFtpClient(strFTPIP, DACrux.Base.Convert.intParse(strFTPPort), strFTPID, strFTPPass);
                bool IsFtpConnect = oFTP.LoginTest();
                if (IsFtpConnect == false)
                    throw new Exception("FTP에 접속할 수 없습니다.");

                strLocalPathName = System.IO.Path.Combine(Environment.CurrentDirectory, "DEFECT_IMAGE");
                oDirectory = new DirectoryInfo(strLocalPathName);
                if (!oDirectory.Exists)
                    oDirectory.Create();

                strLocalFullPath = System.IO.Path.Combine(strLocalPathName, ArrImage[ArrImage.Length - 1].ToString());
                strFTPFullPath = System.IO.Path.Combine(strFTPPath, strFTPDirctory).Replace(@"\", "/");
                oFTP.Down(strFTPFullPath, strLocalFullPath);
                //oFTP.ReceiveFile(strFTPFullPath, strLocalFullPath);

                oDefectFile = new FileInfo(strLocalFullPath);
                if (!oDefectFile.Exists)
                    throw new Exception("Defect Image File을 Download 하였으나 존재 하지 않습니다.");

                return oDefectFile;

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {

            }

        }

        public static MemoryStream fnFTPImageDownloadStream(string strFTPDirctory)
        {
            string strLocalFullPath = string.Empty;

            string strFTPFullPath = string.Empty;
            string strLocalPathName = string.Empty;

            string strFTPIP = string.Empty;
            string strFTPPort = string.Empty;
            string strFTPID = string.Empty;
            string strFTPPass = string.Empty;
            string strFTPPath = "/%2f/images";

            ComConfiguration oConfig = null;

            MemoryStream MSResult = null;

            try
            {
                oConfig = new ComConfiguration();
                DataTable dtConfig = oConfig.GetDefectFTP();
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                    return null;

                strFTPIP = dtConfig.Rows[0]["IP"].ToString();
                strFTPPort = dtConfig.Rows[0]["PORT"].ToString();
                strFTPID = dtConfig.Rows[0]["ID"].ToString();
                strFTPPass = dtConfig.Rows[0]["PASS"].ToString();
                strFTPPath = dtConfig.Rows[0]["MAIN_PATH"].ToString();

                string[] ArrImage = strFTPDirctory.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                using (DACrux.Utility.ServerCommunicationFtp oFTP = new Utility.ServerCommunicationFtp())
                {
                    oFTP.Server = strFTPIP;
                    oFTP.Port = DACrux.Base.Convert.intParse(strFTPPort);
                    oFTP.UserID = strFTPID;
                    oFTP.Password = strFTPPass;
                    oFTP.ChmodValue = 777;

                    MSResult = oFTP.ReceiveStream(strFTPDirctory);
                }

                return MSResult;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {

            }
        }

        public static void fnFTPImageDownloadClear()
        {
            try
            {
                DirectoryInfo oDirectory = new DirectoryInfo(System.IO.Path.Combine(Environment.CurrentDirectory, "DEFECT_IMAGE"));
                if (oDirectory.Exists)
                    oDirectory.Delete(true);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Shot의 시작 인덱스를 Die Min X,Y를 고려하여 재조정합니다.
        /// </summary>
        public static void ReadjustShotStartIndex(WaferMap map)
        {
            int x = Int32.MaxValue;
            int y = Int32.MaxValue;

            foreach (Die die in map.Dies)
            {
                if (die.DieProp == WaferMap.DIE_PROP_VIRTUAL_DIE)
                    continue;

                x = Math.Min(x, die.IndexX);
                y = Math.Min(y, die.IndexY);
            }

            // 1보다 크면 Shot Start 위치를 조정
            if (x > 1)
                map.ShotStartX += (x - 1);

            if (y > 1)
                map.ShotStartY += (y - 1);
        }
    }
}
