/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : TaguchiData.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.05
--  Description     : Taguch 분석에 필요한 Data 계산
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2015 년 DACrux V5 History Start
 ----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Stat.Core
{
    [Serializable]
    public class TaguchiData
    {
        DataTable m_DataSource = null;
        DataTable m_dtDescResult = null;
        DataSet m_dsAnalysisResult = null;
        string[] m_strArrPara = null;
        string[] m_strArrAnalysis = null;

        public DataSet TaguchiDataSet
        {
            get
            {
                return m_dsAnalysisResult;
            }
        }

        public TaguchiData(DataTable oInput = null)
        {
            try
            {
                m_DataSource = oInput;

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
            double dTarSum = double.NaN;
            double dStdAvg = double.NaN;
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

            //CNT
            int cnt_LB = 0;
            int cnt_NN = 0;
            int cnt_NB = 0;
            int cnt_SB = 0;

            try
            {
                ds = new DataSet();
                for (int p = 0; p < m_strArrPara.Length; p++)
                {
                    DataTable dt = FilterEx.SelectGroupBy(string.Format("ANALYSIS_{0}", m_strArrPara[p]), m_dtDescResult, m_strArrPara[p]);
                    dt.Columns.Add("TARGET_EFFECT_AVG", typeof(double)); // 주효과 평균
                    dt.Columns.Add("STDEV_AVG", typeof(double)); // 표준편차 평균
                    dt.Columns.Add("SN_LARGER_BETTER", typeof(double));  // 신호대 잡음비 = -10 * log(sum(1/y**2)/N)
                    dt.Columns.Add("SN_NOMINAL_BEST_N", typeof(double)); // 신호대 잡음비 = -10 * log(STD**2)
                    dt.Columns.Add("SN_NOMINAL_BEST_B", typeof(double)); // 신호대 잡음비 = 10 * log((Y**2)/STD**2)
                    dt.Columns.Add("SN_SMALLER_BETTER", typeof(double)); // 신호대 잡음비 = -10 * LOG(SUM(y**2)/N))

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
                    }

                    ds.Tables.Add(dt);
                }

                return ds;
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
                dt = FilterEx.SelectDistinct(ResultName, m_DataSource, string.Join(",", m_strArrPara));
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
                        Descriptive oDesc = SubCal(dr);
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

        private Descriptive SubCal(DataRow[] dr)
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
                Descriptive oDesc = new Descriptive(dRawData);
                return oDesc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
