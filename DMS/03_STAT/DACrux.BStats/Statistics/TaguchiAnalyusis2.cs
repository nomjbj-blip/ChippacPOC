using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using DACrux.ProjectManager.UI;
using System.IO;

namespace DACrux.BStats.Statistics
{

    public class Descript
    {
        double dSum = double.NaN;
        double dAvg = double.NaN;
        double dStd = double.NaN;
        double dVar = double.NaN;
        double dSumOfSq = double.NaN;
        double dNSumOfSq = double.NaN;

        int dCntAll = 0;
        int dCntAva = 0;
        List<Double> m_dRawData = null;

        public double SUM { get { return dSum; } }
        public double AVG { get { return dAvg; } }
        public double STD { get { return dStd; } }
        public double VAR { get { return dVar; } }
        public double SOS { get { return dSumOfSq; } }
        public double NOS { get { return dNSumOfSq; } }
        public double ALLCount { get { return dCntAll; } }
        public double AVACount { get { return dCntAva; } }
        public double[] RAW { get { return m_dRawData.ToArray(); } }

        public Descript(List<Double> dRawData)
        {
            m_dRawData = dRawData;
            dCntAll = m_dRawData.Count;
            dCntAva = 0;
            List<Double> dRawData_avg = new List<double>();

            for (int i = 0; i < dCntAll; i++)
            {
                if (m_dRawData[i] != double.NaN)
                {
                    if (double.IsNaN(dSumOfSq)) dSumOfSq = 0;
                    if (double.IsNaN(dNSumOfSq)) dNSumOfSq = 0;
                    if (double.IsNaN(dSum)) dSum = 0;
                    dCntAva++;
                    dSumOfSq += Math.Pow(m_dRawData[i], 2);
                    dNSumOfSq += 1 / Math.Pow(m_dRawData[i], 2);
                    dSum += m_dRawData[i];
                }
            }

            //2. 평균
            dAvg = dSum / (double)dCntAva;

            //3. 분산
            dSum = 0;
            for (int i = 0; i < dCntAll; i++)
            {
                if (m_dRawData[i] != double.NaN)
                {
                    dRawData_avg.Add(m_dRawData[i] - dAvg);
                    dSum += Math.Pow(m_dRawData[i] - dAvg, 2);
                }
            }

            dVar = dSum / (dCntAva - 1);
            if (dVar == 0) dVar = double.NaN;

            //4. 표준편차
            dStd = Math.Sqrt(dVar);

        }
    }

    /// <summary>
    /// YSIM 2014-11-05
    /// </summary>
    public class TaguchiAnalyusis2
    {
        DataTable m_DataSource = null;
        DataTable m_dtDescResult = null;
        DataSet m_dsAnalysisResult = null;
        //DataTable m_dtResult = null;
        string[] m_strArrPara = null;
        string[] m_strArrAnalysis = null;
        DACrux.BStats.StatisticsInput.inputTaguchi m_Input = null;

        private GraphInformation[] m_Graph = null;

        private int m_GraphWidth = 500;
        private int m_GraphHeight = 370;

        public TaguchiAnalyusis2(DACrux.BStats.StatisticsInput.inputTaguchi oInput = null)
        {
            try
            {
                m_Input = oInput;
                m_DataSource = oInput.DataSource;

                // Sample
                if (m_DataSource == null)
                {
                    m_strArrPara = new string[] { "A", "B" };
                    m_strArrAnalysis = new string[] { "Y", "Z" };

                    m_DataSource = new DataTable();

                    m_DataSource.Columns.Add("A", typeof(double));
                    m_DataSource.Columns.Add("B", typeof(double));
                    m_DataSource.Columns.Add("Y", typeof(double));
                    m_DataSource.Columns.Add("Z", typeof(double));

                    m_DataSource.Rows.Add(new object[] { 1, 1, 5, 5 });
                    m_DataSource.Rows.Add(new object[] { 1, 2, 34, 3 });
                    m_DataSource.Rows.Add(new object[] { 2, 1, 3, 4 });
                    m_DataSource.Rows.Add(new object[] { 2, 2, 23, 2 });
                }
                else
                {
                    m_strArrPara = new string[oInput.FactorInfos.Length];
                    for (int i = 0; i < oInput.FactorInfos.Length; i++)
                    {
                        m_strArrPara[i] = oInput.FactorInfos[i].Name;
                    }

                    m_strArrAnalysis = new string[oInput.AnalysisVariables.Count];
                    for (int i = 0; i < oInput.AnalysisVariables.Count; i++)
                    {
                        m_strArrAnalysis[i] = oInput.AnalysisVariables[i].ToString();
                    }

                }

                m_DataSource.TableName = "RAWDATA";

                //1. 각 경우의 수별로 통계량값
                //A	B	z	Z2		avg	    var
                //-+--+---+------+-------+-----------
                //1	1	5	5		5	    0
                //1	2	34	3		18.5    480.5
                //2	1	3	4		3.5	    0.5
                //2	2	23	2		12.5	220.5
                m_dtDescResult = Calculation();
                m_dtDescResult.TableName = "SUMMARYDATA";


                //2. 각 요인별 평균, SN비
                if (m_dsAnalysisResult != null) m_dsAnalysisResult.Dispose();
                m_dsAnalysisResult = Analysis();
                m_dsAnalysisResult.Tables.Add(m_dtDescResult);
                m_dsAnalysisResult.Tables.Add(m_DataSource);

                m_Graph = GetGraphInfo();
                MakeHtml();
                m_Input.GraphInformations = m_Graph;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private GraphInformation[] GetGraphInfo()
        {
            #region Core의 Taguchi클래스 내의 Graph Title(클래스변수) 참고..
            //const string strAverage = "평균의 주 효과 플롯(데이터 평균)";
            //const string strSNRatio = "신호 대 잡음 비의 주 효과 플롯(데이터 평균)";
            //const string strStdev = "표준 편차의 주 효과 플롯(데이터 평균)";
            #endregion
            try
            {
                if (m_dsAnalysisResult == null || m_dsAnalysisResult.Tables.Count < 1)
                {
                    return null;
                }

                List<GraphInformation> lstGrpInfo = new List<GraphInformation>();

                if (m_Input.GraphAverage)
                {
                    GraphInformation gaReturn = new GraphInformation(GraphType.Line4Taguchi);
                    gaReturn.Name = "Taguchi(AVG:" + m_Input.Title + ")";
                    gaReturn.SubTitle = "TARGET_EFFECT_AVG";
                    gaReturn.AddAxisY(new GraphInformation.ColumnInfoItem(0, null, "평균의 평균", typeof(double)));
                    gaReturn.AxisX = new GraphInformation.ColumnInfoItem(1, null, "요인", typeof(double));
                    gaReturn.AxisXTitle = "TARGET_EFFECT_AVG";
                    gaReturn.DecimalPlace = DataTableUtil.GetDecimalPlace(m_dsAnalysisResult.Tables["RAWDATA"]);
                    gaReturn.GridLine = false;
                    gaReturn.PointColor = System.Drawing.Color.Red;
                    gaReturn.ForceZero = false;
                    gaReturn.AxisXForceZero = false;
                    gaReturn.DataSource4Taguchi = m_dsAnalysisResult;
                    gaReturn.Title = "평균의 주 효과 플롯(데이터 평균)";
                    gaReturn.Frequence = true;
                    gaReturn.IsImageWithTitles = true;
                    for (int i = 0; i < m_strArrPara.Length; i++)
                    {
                        gaReturn.ColumnInfoItems.Add(new GraphInformation.ColumnInfoItem(i, m_strArrPara[i], m_strArrPara[i], typeof(double)));
                    }
                    gaReturn.ImageSize = new System.Drawing.Size(m_GraphWidth, m_GraphHeight);
                    gaReturn.ImagePath = GetFileName(m_Input.ResultFilePath) + "AVG_" + DateTime.Now.ToString("yyyyMMddHHmmss")  + ".jpg";
                    lstGrpInfo.Add(gaReturn);
                }

                if (m_Input.GraphSNRatio)
                {
                    DataRow[] dr = m_dsAnalysisResult.Tables["DELTA"].Select(string.Format("VALUETYPE = '{0}' AND DELTA = 'NaN'",Enum.GetName(typeof(TaguchiAnalysisRule), m_Input.tgcAnalysisRule)));
                    if (dr == null || dr.Length == 0)
                    {

                        GraphInformation gsnReturn = new GraphInformation(GraphType.Line4Taguchi);
                        gsnReturn.Name = "Taguchi(S/N Ratio:" + m_Input.Title + ")"; ;
                        gsnReturn.SubTitle = Enum.GetName(typeof(TaguchiAnalysisRule), m_Input.tgcAnalysisRule);
                        gsnReturn.AddAxisY(new GraphInformation.ColumnInfoItem(0, null, "신호대 잡음비의 평균", typeof(double)));
                        gsnReturn.AxisX = new GraphInformation.ColumnInfoItem(1, null, "요인", typeof(double));
                        gsnReturn.AxisXTitle = Enum.GetName(typeof(TaguchiAnalysisRule), m_Input.tgcAnalysisRule);
                        gsnReturn.DecimalPlace = DataTableUtil.GetDecimalPlace(m_dsAnalysisResult.Tables["RAWDATA"]);
                        gsnReturn.GridLine = false;
                        gsnReturn.PointColor = System.Drawing.Color.Red;
                        gsnReturn.ForceZero = false;
                        gsnReturn.AxisXForceZero = false;
                        gsnReturn.DataSource4Taguchi = m_dsAnalysisResult;
                        gsnReturn.Title = "신호 대 잡음 비의 주 효과 플롯(데이터 평균)";
                        gsnReturn.Frequence = true;
                        gsnReturn.IsImageWithTitles = true;
                        for (int i = 0; i < m_strArrPara.Length; i++)
                        {
                            gsnReturn.ColumnInfoItems.Add(new GraphInformation.ColumnInfoItem(i, m_strArrPara[i], m_strArrPara[i], typeof(double)));
                        }
                        gsnReturn.ImageSize = new System.Drawing.Size(m_GraphWidth, m_GraphHeight);
                        gsnReturn.ImagePath = GetFileName(m_Input.ResultFilePath) + "SNR_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        lstGrpInfo.Add(gsnReturn);
                    }
                }

                if (m_Input.GraphSTDDev)
                {
                    GraphInformation gstdReturn = new GraphInformation(GraphType.Line4Taguchi);
                    gstdReturn.Name = "Taguchi(STDEV:" + m_Input.Title + ")";
                    gstdReturn.SubTitle = "STDEV_AVG";
                    gstdReturn.AddAxisY(new GraphInformation.ColumnInfoItem(0, null, "표준 편차의 평균", typeof(double)));
                    gstdReturn.AxisX = new GraphInformation.ColumnInfoItem(1, null, "요인", typeof(double));
                    gstdReturn.AxisXTitle = "STDEV_AVG";
                    gstdReturn.DecimalPlace = DataTableUtil.GetDecimalPlace(m_dsAnalysisResult.Tables["RAWDATA"]);
                    gstdReturn.GridLine = false;
                    gstdReturn.PointColor = System.Drawing.Color.Red;
                    gstdReturn.ForceZero = false;
                    gstdReturn.AxisXForceZero = false;
                    gstdReturn.DataSource4Taguchi = m_dsAnalysisResult;
                    gstdReturn.Title = "표준 편차의 주 효과 플롯(데이터 평균)";
                    gstdReturn.Frequence = true;
                    gstdReturn.IsImageWithTitles = true;
                    for (int i = 0; i < m_strArrPara.Length; i++)
                    {
                        gstdReturn.ColumnInfoItems.Add(new GraphInformation.ColumnInfoItem(i, m_strArrPara[i], m_strArrPara[i], typeof(double)));
                    }
                    gstdReturn.ImageSize = new System.Drawing.Size(m_GraphWidth, m_GraphHeight);
                    gstdReturn.ImagePath = GetFileName(m_Input.ResultFilePath) + "STD_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    lstGrpInfo.Add(gstdReturn);
                }

                return lstGrpInfo.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetFileName(string strFullName)
        {
            FileInfo fi = new FileInfo(strFullName);
            try
            {
                return fi.Name.Remove(fi.Name.LastIndexOf(fi.Extension));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MakeHtml()
        {
            HtmlConverter oHtml = null;
            try
            {
                oHtml = new HtmlConverter(DACrux.ProjectManager.UI.Common.TempPath + m_Input.ResultFilePath);
                oHtml.MainTitle(m_Input.Project, m_Input.WorkSheet, m_Input.User, m_Input.Title);
                oHtml.BodyTaguchi(m_dsAnalysisResult, m_Input.tgcAnalysisRule, m_Graph);
                oHtml.SaveHtml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Taguchi Analysis
        private DataSet Analysis()
        {

            DataSet ds = null;
            string filter = string.Empty;

            double tmp = double.NaN;

            double dTarAvg = double.NaN;
            double dTarMin = double.NaN;
            double dTarMax = double.NaN;
            double dTarSum = double.NaN;

            double dStdAvg = double.NaN;
            double dStdMax = double.NaN;
            double dStdMin = double.NaN;
            double dStdSum = double.NaN;
            int cnt = 0;
            int cnt_std = 0;

            //AVG
            double dSN_AVG_LB = double.NaN;
            double dSN_AVG_NN = double.NaN;
            double dSN_AVG_NB = double.NaN;
            double dSN_AVG_SB = double.NaN;

            //SUM
            double dSN_SUM_LB = double.NaN;
            double dSN_SUM_NN = double.NaN;
            double dSN_SUM_NB = double.NaN;
            double dSN_SUM_SB = double.NaN;

            //MIN
            double dSN_MIN_LB = double.NaN;
            double dSN_MIN_NN = double.NaN;
            double dSN_MIN_NB = double.NaN;
            double dSN_MIN_SB = double.NaN;

            //MAX
            double dSN_MAX_LB = double.NaN;
            double dSN_MAX_NN = double.NaN;
            double dSN_MAX_NB = double.NaN;
            double dSN_MAX_SB = double.NaN;

            //CNT
            int cnt_LB = 0;
            int cnt_NN = 0;
            int cnt_NB = 0;
            int cnt_SB = 0;

            try
            {
                ds = new DataSet();
                DataTable dt_delta = new DataTable("DELTA");
                dt_delta.Columns.Add("FACTOR", typeof(string));
                dt_delta.Columns.Add("VALUETYPE", typeof(string));
                dt_delta.Columns.Add("MAXVALUE", typeof(double));
                dt_delta.Columns.Add("MINVALUE", typeof(double));
                dt_delta.Columns.Add("DELTA", typeof(double));
                dt_delta.Columns.Add("RANK", typeof(double));

                for (int p = 0; p < m_strArrPara.Length; p++)
                {
                    DataTable dt = DACrux.Base.FilterEx.SelectGroupBy(string.Format("ANALYSIS_{0}", m_strArrPara[p]), m_dtDescResult, m_strArrPara[p]);
                    dt.Columns.Add("TARGET_EFFECT_AVG", typeof(double)); // 주효과 평균
                    dt.Columns.Add("STDEV_AVG", typeof(double)); // 표준편차 평균
                    dt.Columns.Add("SN_LARGER_BETTER", typeof(double));  // 신호대 잡음비 = -10 * log(sum(1/y**2)/N) , 클수록 좋음
                    dt.Columns.Add("SN_NOMINAL_BEST_N", typeof(double)); // 신호대 잡음비 = -10 * log(STD**2) , 목표 수준이 가장 좋음
                    dt.Columns.Add("SN_NOMINAL_BEST_B", typeof(double)); // 신호대 잡음비 = 10 * log((Y**2)/STD**2) , 목표 수준이 가장좋음
                    dt.Columns.Add("SN_SMALLER_BETTER", typeof(double)); // 신호대 잡음비 = -10 * LOG(SUM(y**2)/N)) , 작을 수록 좋음

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        filter = string.Format("{0}={1}", m_strArrPara[p], dt.Rows[i][m_strArrPara[p]].ToString());
                        DataRow[] drs = m_dtDescResult.Select(filter);


                        dTarAvg = 0;
                        dTarSum = 0;

                        dStdAvg = 0;
                        dStdSum = 0;


                        dSN_AVG_LB = double.NaN;
                        dSN_AVG_NN = double.NaN;
                        dSN_AVG_NB = double.NaN;
                        dSN_AVG_SB = double.NaN;

                        dSN_SUM_LB = 0;
                        dSN_SUM_NN = 0;
                        dSN_SUM_NB = 0;
                        dSN_SUM_SB = 0;



                        tmp = double.NaN;
                        cnt = 0;
                        cnt_std = 0;

                        cnt_LB = 0;
                        cnt_NN = 0;
                        cnt_NB = 0;
                        cnt_SB = 0;

                        foreach (DataRow dr in drs)
                        {
                            if (double.TryParse(dr["MIRA_AVG_COM"].ToString(), out tmp))
                            {
                                cnt++;
                                dTarSum += tmp;
                            }

                            ///1. SN_LARGER_BETTER
                            if (double.TryParse(dr["MIRA_NOS_COM"].ToString(), out tmp))
                            {
                                if (double.IsNaN(tmp) == false)
                                {
                                    int avacnt = int.Parse(dr["MIRA_AVACNT_COM"].ToString());
                                    cnt_LB++;
                                    dSN_SUM_LB += -10 * Math.Log10(tmp / avacnt);
                                }
                            }

                            ///2. SN_NOMINAL_BEST_N
                            if (double.TryParse(dr["MIRA_STD_COM"].ToString(), out tmp))
                            {
                                if (double.IsNaN(tmp) == false)
                                {
                                    cnt_NN++;
                                    dSN_SUM_NN += -10 * Math.Log10(Math.Pow(tmp, 2));

                                    cnt_std++;
                                    dStdSum += tmp;
                                }
                            }

                            ///3. SN_NOMINAL_BEST_B
                            if (double.TryParse(dr["MIRA_AVG_COM"].ToString(), out tmp))
                            {
                                double tmpstd = double.NaN;
                                if (double.TryParse(dr["MIRA_STD_COM"].ToString(), out tmpstd))
                                {
                                    if (double.IsNaN(tmp) == false && double.IsNaN(tmpstd) == false)
                                    {
                                        cnt_NB++;
                                        dSN_SUM_NB += 10 * Math.Log10(Math.Pow(tmp, 2) / Math.Pow(tmpstd, 2));
                                    }
                                }
                            }

                            ///4. SN_SMALLER_BETTER
                            if (double.TryParse(dr["MIRA_SOS_COM"].ToString(), out tmp))
                            {
                                if (double.IsNaN(tmp) == false)
                                {
                                    int avacnt = int.Parse(dr["MIRA_AVACNT_COM"].ToString());
                                    cnt_SB++;
                                    dSN_SUM_SB += -10 * Math.Log10(tmp / avacnt);
                                }
                            }

                        }



                        ///0. TARGET_EFFECT_AVG
                        dTarAvg = dTarSum / cnt;
                        dt.Rows[i]["TARGET_EFFECT_AVG"] = dTarAvg;

                        ///1. STDEV_AVG
                        dStdAvg = dStdSum / cnt_std;
                        dt.Rows[i]["STDEV_AVG"] = dStdAvg;

                        ///2. SN_LARGER_BETTER
                        dSN_AVG_LB = dSN_SUM_LB / cnt_LB;
                        dt.Rows[i]["SN_LARGER_BETTER"] = dSN_AVG_LB;

                        ///3. SN_NOMINAL_BEST_N
                        dSN_AVG_NN = dSN_SUM_NN / cnt_NN;
                        dt.Rows[i]["SN_NOMINAL_BEST_N"] = dSN_AVG_NN;

                        ///4. SN_NOMINAL_BEST_B
                        dSN_AVG_NB = dSN_SUM_NB / cnt_NB;
                        dt.Rows[i]["SN_NOMINAL_BEST_B"] = dSN_AVG_NB;

                        ///5. SN_SMALLER_BETTER
                        dSN_AVG_SB = dSN_SUM_SB / cnt_SB;
                        dt.Rows[i]["SN_SMALLER_BETTER"] = dSN_AVG_SB;

                        if (i == 0)
                        {
                            dTarMin = dTarAvg;
                            dTarMax = dTarAvg;
                            dStdMin = dStdAvg;
                            dStdMax = dStdAvg;

                            dSN_MIN_LB = dSN_AVG_LB;
                            dSN_MAX_LB = dSN_AVG_LB;

                            dSN_MIN_NN = dSN_AVG_NN;
                            dSN_MAX_NN = dSN_AVG_NN;

                            dSN_MIN_NB = dSN_AVG_NB;
                            dSN_MAX_NB = dSN_AVG_NB;

                            dSN_MIN_SB = dSN_AVG_SB;
                            dSN_MAX_SB = dSN_AVG_SB;
                        }
                        else
                        {
                            if (dTarMin > dTarAvg) dTarMin = dTarAvg;
                            if (dStdMin > dStdAvg) dStdMin = dStdAvg;
                            if (dSN_MIN_LB > dSN_AVG_LB) dSN_MIN_LB = dSN_AVG_LB;
                            if (dSN_MIN_NN > dSN_AVG_NN) dSN_MIN_NN = dSN_AVG_NN;
                            if (dSN_MIN_NB > dSN_AVG_NB) dSN_MIN_NB = dSN_AVG_NB;
                            if (dSN_MIN_SB > dSN_AVG_SB) dSN_MIN_SB = dSN_AVG_SB;

                            if (dTarMax < dTarAvg) dTarMax = dTarAvg;
                            if (dStdMax < dStdAvg) dStdMax = dStdAvg;
                            if (dSN_MAX_LB < dSN_AVG_LB) dSN_MAX_LB = dSN_AVG_LB;
                            if (dSN_MAX_NN < dSN_AVG_NN) dSN_MAX_NN = dSN_AVG_NN;
                            if (dSN_MAX_NB < dSN_AVG_NB) dSN_MAX_NB = dSN_AVG_NB;
                            if (dSN_MAX_SB < dSN_AVG_SB) dSN_MAX_SB = dSN_AVG_SB;
                        }
                    }

                    dt_delta.Rows.Add(new object[] { m_strArrPara[p], "TARGET_EFFECT_AVG", dTarMax, dTarMin, Math.Abs(dTarMax - dTarMin), 0 });
                    dt_delta.Rows.Add(new object[] { m_strArrPara[p], "STDEV_AVG", dStdMax, dStdMin, Math.Abs(dStdMax - dStdMin), 0 });
                    dt_delta.Rows.Add(new object[] { m_strArrPara[p], "SN_LARGER_BETTER", dSN_MAX_LB, dSN_MIN_LB, Math.Abs(dSN_MAX_LB - dSN_MIN_LB), 0 });
                    dt_delta.Rows.Add(new object[] { m_strArrPara[p], "SN_NOMINAL_BEST_N", dSN_MAX_NN, dSN_MIN_NN, Math.Abs(dSN_MAX_NN - dSN_MIN_NN), 0 });
                    dt_delta.Rows.Add(new object[] { m_strArrPara[p], "SN_NOMINAL_BEST_B", dSN_MAX_NB, dSN_MIN_NB, Math.Abs(dSN_MAX_NB - dSN_MIN_NB), 0 });
                    dt_delta.Rows.Add(new object[] { m_strArrPara[p], "SN_SMALLER_BETTER", dSN_MAX_SB, dSN_MIN_SB, Math.Abs(dSN_MAX_SB - dSN_MIN_SB), 0 });

                    ds.Tables.Add(dt);
                }
                DeltaRank("TARGET_EFFECT_AVG", ref dt_delta);
                DeltaRank("STDEV_AVG", ref dt_delta);
                DeltaRank("SN_LARGER_BETTER", ref dt_delta);
                DeltaRank("SN_NOMINAL_BEST_N", ref dt_delta);
                DeltaRank("SN_NOMINAL_BEST_B", ref dt_delta);
                DeltaRank("SN_SMALLER_BETTER", ref dt_delta);

                ds.Tables.Add(dt_delta);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void DeltaRank(string valueType , ref DataTable dt)
        {
            try
            {
                
                DataTable dtResult = DACrux.Base.FilterEx.SelectGroupBy(valueType, dt, "DELTA", string.Format("VALUETYPE='{0}' AND DELTA <> 'NaN'",valueType));

                DataRow[] drs = dtResult.Select(null, "DELTA DESC");

                int r = 1;
                for (int i = 0; i < drs.Length; i++)
                {
                    DataRow[] dataRows = dt.Select(string.Format("VALUETYPE='{0}' AND CONVERT(DELTA,System.Decimal) = CONVERT({1}, System.Decimal)", valueType, drs[i]["DELTA"]));

                    int icnt = int.Parse(drs[i]["COUNT"].ToString());
                    int sum = 0;
                    for (int j = 0; j < icnt; j++)
                    {
                        sum += r++;
                    }

                    foreach (DataRow dr in dataRows)
                    {
                        dr["RANK"] = (float)sum / (float)icnt;
                    }
                }

                dt.AcceptChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private DataTable Calculation(string ResultName = "DESCRIPT")
        {
            DataTable dt = null;
            try
            {
                dt = DACrux.Base.FilterEx.SelectDistinct(ResultName, m_DataSource, string.Join(",", m_strArrPara));
                dt.Columns.Add("MIRA_AVG_COM", typeof(double));
                dt.Columns.Add("MIRA_STD_COM", typeof(double));
                dt.Columns.Add("MIRA_VAR_COM", typeof(double));
                dt.Columns.Add("MIRA_SOS_COM", typeof(double));
                dt.Columns.Add("MIRA_NOS_COM", typeof(double));
                dt.Columns.Add("MIRA_AVACNT_COM", typeof(int));

                string[] ArrFilter = new string[m_strArrPara.Length];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int p = 0; p < m_strArrPara.Length; p++)
                    {
                        ArrFilter[p] = string.Format("{0}={1}", m_strArrPara[p], dt.Rows[i][m_strArrPara[p]].ToString());
                    }

                    string strFilter = string.Join(" AND ", ArrFilter);
                    DataRow[] dr = m_DataSource.Select(strFilter);
                    if (dr != null && dr.Length > 0)
                    {
                        Descript oDesc = SubCal(dr);
                        dt.Rows[i]["MIRA_AVG_COM"] = oDesc.AVG;
                        dt.Rows[i]["MIRA_STD_COM"] = oDesc.STD;
                        dt.Rows[i]["MIRA_VAR_COM"] = oDesc.VAR;
                        dt.Rows[i]["MIRA_SOS_COM"] = oDesc.SOS;
                        dt.Rows[i]["MIRA_NOS_COM"] = oDesc.NOS;
                        dt.Rows[i]["MIRA_AVACNT_COM"] = oDesc.AVACount;
                    }
                }

                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Descript SubCal(DataRow[] dr)
        {
            double tmp = double.NaN;
            List<Double> dRawData = new List<double>();
            try
            {

                for (int i = 0; i < dr.Length; i++)
                {
                    for (int a = 0; a < m_strArrAnalysis.Length; a++)
                    {
                        tmp = double.NaN;
                        if (double.TryParse(dr[i][m_strArrAnalysis[a]].ToString(), out tmp))
                        {
                            dRawData.Add(tmp);
                        }
                    }
                }
                Descript oDesc = new Descript(dRawData);
                return oDesc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }



}
