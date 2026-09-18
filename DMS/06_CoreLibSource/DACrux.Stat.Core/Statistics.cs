using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Stat.Core
{
    /// <summary>
    /// Class Name : Statistics<br/>
    /// Summary    : Descriptive Statistical Class<br/>
    /// Author     : Miracom Hyuntai, Kim<br/>
    /// First Date : 2007-09-17<br/>
    /// Description: Return Descriptive Statistical values.<br/>
    /// History    : 2011-01-22 : YSIM<br/>
    ///                         > Additional static Average function of double value array
    ///                         > Change to static all function type
    /// </summary>
    public class Statistics
    {
        public static double Average(double[] data)
        {  
            try
            {
                if (data == null || data.Length == 0) return double.NaN;
                double sum = 0d;
                for (int i = 0; i < data.Length; i++)
                {
                    sum += data[i];
                }

                return sum / data.Length;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double StandardDev(double[] data)
        {
            try
            {
                if (data == null || data.Length == 0) return double.NaN;

                double dAvg = Average(data);
                double sumOfDerivation = 0;

                for (int i = 0; i < data.Length; i++)
                {
                    sumOfDerivation += (dAvg - data[i]) * (dAvg - data[i]);
                }

                double sumOfDerivationAverage = sumOfDerivation / (data.Length - 1);
                return Math.Sqrt(sumOfDerivationAverage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double Median(double[] data)
        {
            
            double dMedian = double.NaN;
            try
            {

                if (data == null || data.Length == 0) return double.NaN;
                Array.Sort(data);
                if (data.Length % 2 == 0)
                {
                    dMedian = (data[data.Length / 2] + data[(data.Length / 2) - 1]) / 2d;
                }
                else
                {
                    dMedian = data[data.Length / 2];
                }

                return dMedian;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 관리도용 계수
        private static double A(int n)
        {
            double[] dblA = new double[] { double.NaN, double.NaN, 2.121, 1.732, 1.501, 1.342, 1.225, 1.134, 1.061, 1.000, 0.949, 0.905, 0.866, 0.832, 0.802, 0.775, 0.750, 0.728, 0.707, 0.688, 0.671 };
            return dblA[n];
        }
        private static double A2(int n)
        {
            double[] dblA2 = new double[] { double.NaN, double.NaN, 1.880, 1.023, 0.729, 0.577, 0.483, 0.419, 0.373, 0.337, 0.308, 0.285, 0.266, 0.249, 0.235, 0.223, 0.212, 0.203, 0.194, 0.187, 0.180 };
            return dblA2[n];
        }
        private static double A3(int n)
        {
            double[] dblA3 = new double[] { double.NaN, double.NaN, 2.659, 1.954, 1.628, 1.427, 1.287, 1.182, 1.099, 1.032, 0.975, 0.927, 0.886, 0.850, 0.817, 0.789, 0.763, 0.739, 0.718, 0.698, 0.680 };
            return dblA3[n];
        }

        private static double D(int n)
        {
            double[] dblD = new double[] { double.NaN, double.NaN, 0, 0, 0, 0, 0, 0.205, 0.387, 0.546, 0.687, 0.812, 0.924, 1.026, 1.131, 1.307, 1.385, 1.359, 1.426, 1.490, 1.548 };
            return dblD[n];
        }
        private static double D2(int n)
        {
            double[] dblD2 = new double[] { double.NaN, double.NaN, 3.686, 4.358, 4.698, 4.918, 5.078, 5.203, 5.307, 5.394, 5.469, 5.534, 5.592, 5.646, 5.693, 7.737, 5.779, 5.817, 5.854, 5.888, 5.922 };
            return dblD2[n];
        }
        private static double D3(int n)
        {
            double[] dblD3 = new double[] { double.NaN, double.NaN, 0, 0, 0, 0, 0, 0.076, 0.136, 0.184, 0.223, 0.256, 0.284, 0.308, 0.329, 0.348, 0.364, 0.379, 0.392, 0.404, 0.414 };
            return dblD3[n];
        }
        private static double D4(int n)
        {
            double[] dblD4 = new double[] { double.NaN, double.NaN, 3.267, 2.575, 2.282, 2.115, 2.004, 1.924, 1.864, 1.816, 1.777, 1.744, 1.719, 1.692, 1.671, 1.652, 1.636, 1.621, 1.608, 1.596, 1.586 };
            return dblD4[n];
        }
        #endregion

        #region Get Control Limit

        public static double GetControlLimit(double[] data, int iSampleCnt, string SType)
        {
            double d_rtn = double.NaN;

            try
            {
                switch (SType)
                {
                    case "UpperXBar":
                        d_rtn = A2(iSampleCnt) * Average(data);
                        break;
                    case "LowerXBar":
                        d_rtn = A2(iSampleCnt) * Average(data) * (-1);
                        break;
                    case "UpperRng":
                        if (iSampleCnt == 1)
                        {
                            //int iloc = 2;
                            //double[] tdata = new double[data.Length - 1];

                            //for (int i = 1; i < data.Length; i++)
                            //{
                            //    tdata[i - 1] = data[i];
                            //}

                            //d_rtn = D4(iloc) * Average(tdata);
                            d_rtn = D4(2) * Average(data);
                        }
                        else
                        {
                            d_rtn = D4(iSampleCnt) * Average(data);
                        }
                        break;
                    case "LowerRng":
                        if (iSampleCnt == 1)
                        {
                            //int iloc = 2;
                            //double[] tdata = new double[data.Length - 1];

                            //for (int i = 1; i < data.Length; i++)
                            //{
                            //    tdata[i - 1] = data[i];
                            //}

                            //d_rtn = D3(iloc) * Average(tdata);
                            d_rtn = D3(2) * Average(data);
                        }
                        else
                        {
                            d_rtn = D3(iSampleCnt) * Average(data);
                        }
                        break;
                }

                return d_rtn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Standard Deviation Original

        public static double GetStdvOrg(DataTable argdt, int[] arrValueIndex, string StdType)
        {
            double d_rtn = double.NaN;

            try
            {
                string[] arrValueString = new string[arrValueIndex.Length];

                for (int i = 0; i < arrValueIndex.Length; i++)
                {
                    arrValueString[i] = argdt.Columns[arrValueIndex[i]].ColumnName;
                }

                DataTable dt = argdt.DefaultView.ToTable(false, arrValueString);
                double[] dArrayList = null;

                switch (StdType)
                {
                    case "SINGLE":
                        dArrayList = new double[dt.Rows.Count * arrValueIndex.Length];
                        int k = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                dArrayList[k] = double.Parse(dt.Rows[i][j].ToString());
                                k++;
                            }
                        }

                        d_rtn = StandardDev(dArrayList);
                        break;
                    case "AVERAGE":
                        dArrayList = new double[dt.Columns.Count];
                        double SumofSquareStdDev = 0d;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                dArrayList[j] = double.Parse(dt.Rows[i][j].ToString());
                            }

                            double tStdDev = StandardDev(dArrayList);
                            SumofSquareStdDev += tStdDev * tStdDev;
                        }

                        d_rtn = Math.Sqrt(SumofSquareStdDev / dt.Rows.Count);
                        break;
                }

                return d_rtn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetStat
        /// <summary>
        /// 통계량을 계산해주는 메소드(Target이 중심이 아닐경우 Cpk에 Cpm을 넣어서 돌려주고 Cpm도 그대로 채워서 돌려줌.)
        /// 상세  설명: Data dt와 [USL, Target, LSL]string array을 받아서 [sigma, TotalCount , n, Min, Max, Avg, Cp, K,   Cpk, Cpu, Cpl, Cpm]string array 반환<br/>
        ///                                                                  0       1         2    3    4    5   6   7    8    9    10   11
        /// </summary>
        /// <param name="dr"> 2차원(직사각형)으로 된 그룹구분 없는 Data Rows</param>
        /// <param name="arrInput">[USL, Target, LSL]의 string Array</param>
        public static string[] GetStat(DataTable dt, string[] arrInput, int iStartCol, int iEndCol)
        {
            int iRowsCNT = 0;
            int iDataCNT = 0;

            string[] arrReturn = new string[12];

            decimal dblAcc = 0;
            decimal dblSum = 0;
            double dblMax = -99999999999999;
            double dblMin = 99999999999999;
            decimal dblSigma;

            decimal dblAvg;
            double dblCp;
            double dblK;
            double dblCpk;
            double dblCpu;
            double dblCpl;
            double dblCpm;
            double dblUSL;
            double dblTarget;
            double dblLSL;
            double dblM;
            decimal dblSigma_;

            try
            {
                for (int i = 0; i < 12; i++)
                    arrReturn[i] = string.Empty;

                iRowsCNT = dt.Rows.Count;

                for (int i = 0; i < iRowsCNT; i++)
                {
                    for (int j = iStartCol; j <= iEndCol; j++)
                    {
                        if (dt.Rows[i][j] != DBNull.Value)
                        {
                            if (dt.Rows[i][j].ToString().Trim() != "")
                            {
                                if (System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim()) > dblMax) dblMax = System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim());
                                if (System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim()) < dblMin) dblMin = System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim());

                                dblAcc += System.Convert.ToDecimal(dt.Rows[i][j].ToString().Trim());
                                dblSum += (decimal)Math.Pow(System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim()), 2);
                                iDataCNT++;
                            }
                        }
                    }
                }
                if (iDataCNT < 2) //자료수 부족
                {
                    return arrReturn;
                }

                arrReturn[1] = System.Convert.ToString(iRowsCNT * (iEndCol - iStartCol + 1));
                arrReturn[2] = System.Convert.ToString(iDataCNT);
                arrReturn[3] = System.Convert.ToString(dblMin);
                arrReturn[4] = System.Convert.ToString(dblMax);

                dblAvg = dblAcc / iDataCNT;

                arrReturn[5] = System.Convert.ToString(dblAvg);


                dblSigma = (decimal)Math.Sqrt((double)((dblSum / (iDataCNT - 1)) - (dblAvg * dblAvg * iDataCNT / (iDataCNT - 1))));

                arrReturn[0] = System.Convert.ToString(dblSigma);


                if (arrInput[2].Trim() != string.Empty && arrInput[0].Trim() != string.Empty) // 상한 하한 모두 존재
                {
                    dblUSL = System.Convert.ToDouble(arrInput[0].ToString().Trim());
                    dblLSL = System.Convert.ToDouble(arrInput[2].ToString().Trim());
                    //먼저 상한과 하한이 있는 경우 CP를 구함.
                    dblCp = (dblUSL - dblLSL) / (6 * (double)dblSigma);
                    arrReturn[6] = System.Convert.ToString(dblCp);

                    dblCpu = (dblUSL - (double)dblAvg) / (3 * (double)dblSigma);
                    dblCpl = ((double)dblAvg - dblLSL) / (3 * (double)dblSigma);

                    dblM = Math.Round(((dblUSL + dblLSL) / 2), 4);

                    dblK = Math.Abs(dblM - (double)dblAvg) / ((dblUSL - dblLSL) / 2);
                    arrReturn[7] = dblK.ToString();
                    if (arrInput[1] == null || arrInput[1].Trim() == string.Empty) //Target없을 때
                    {
                        dblTarget = (dblUSL + dblLSL) / 2;
                    }
                    else
                    {
                        dblTarget = System.Convert.ToDouble(arrInput[1].ToString().Trim());
                    }

                    if (dblM == dblTarget) //목표치가 스펙의 중심일 때
                    {
                        if (dblCpl < dblCpu)
                        {
                            dblCpk = dblCpl;
                        }
                        else
                        {
                            dblCpk = dblCpu;
                        }
                        arrReturn[8] = System.Convert.ToString(dblCpk);
                    }
                    else
                    {
                        dblSigma_ = (decimal)Math.Sqrt(Math.Pow((double)dblSigma, 2) + Math.Pow(((double)dblAvg - dblTarget), 2));
                        dblCpm = (dblUSL - dblLSL) / (6 * (double)dblSigma_);
                        arrReturn[11] = System.Convert.ToString(dblCpm);
                        arrReturn[8] = System.Convert.ToString(dblCpm);
                    }
                }
                else if (arrInput[0].Trim() != string.Empty) // USL만 존재하는 경우
                {
                    dblUSL = System.Convert.ToDouble(arrInput[0].ToString().Trim());
                    dblCpu = (dblUSL - (double)dblAvg) / (3 * (double)dblSigma);
                    arrReturn[8] = System.Convert.ToString(dblCpu);
                }
                else if (arrInput[2].Trim() != string.Empty) // LSL만 존재하는 경우
                {
                    dblLSL = System.Convert.ToDouble(arrInput[2].ToString().Trim());
                    dblCpl = ((double)dblAvg - dblLSL) / (3 * (double)dblSigma);
                    arrReturn[8] = System.Convert.ToString(dblCpl);
                }

                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetStat2
        /// <summary>
        /// 통계량을 계산해주는 메소드(Target이 중심이 아닐경우 Cpm만 돌려줌.)
        /// 상세  설명: Data dt와 [USL, Target, LSL]string array을 받아서 [sigma, TotalCount , n, Min, Max, Avg, Cp, K,   Cpk, Cpu, Cpl, Cpm, Ucl, Lcl]string array 반환<br/>
        ///                                                                  0       1         2    3    4    5   6   7    8    9    10   11  12    13
        /// </summary>
        /// <param name="dr"> 2차원(직사각형)으로 된 그룹구분 없는 Data Rows</param>
        /// <param name="arrInput">[USL, Target, LSL]의 string Array</param>
        public static string[] GetStat2(DataTable dt, string[] arrInput, int iStartCol, int iEndCol)
        {
            int iRowsCNT = 0;
            int iDataCNT = 0;

            string[] arrReturn = new string[14];

            decimal dblAcc = 0;
            decimal dblSum = 0;
            double dblMax = -99999999999999;
            double dblMin = 99999999999999;
            decimal dblSigma;

            decimal dblAccTmp = 0;
            decimal dblAcc2 = 0;
            decimal dblSum2 = 0;
            int iDataCNT2 = 0;
            decimal dblTemp;
            decimal dblSigma2;

            decimal dblAvg;
            double dblCp;
            double dblK;
            double dblCpk;
            double dblCpu;
            double dblCpl;
            double dblCpm;
            double dblUSL;
            double dblTarget;
            double dblLSL;
            double dblM;
            decimal dblSigma_;

            try
            {
                for (int i = 0; i < 14; i++)
                    arrReturn[i] = string.Empty;

                iRowsCNT = dt.Rows.Count;

                for (int i = 0; i < iRowsCNT; i++)
                {
                    dblAccTmp = 0;
                    iDataCNT2 = 0;
                    for (int j = iStartCol; j <= iEndCol; j++)
                    {
                        if (dt.Rows[i][j] != DBNull.Value)
                        {
                            if (dt.Rows[i][j].ToString().Trim() != "")
                            {
                                if (System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim()) > dblMax) dblMax = System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim());
                                if (System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim()) < dblMin) dblMin = System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim());

                                dblAcc += System.Convert.ToDecimal(dt.Rows[i][j].ToString().Trim());
                                dblSum += (decimal)Math.Pow(System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim()), 2);
                                iDataCNT++;

                                dblAccTmp += System.Convert.ToDecimal(dt.Rows[i][j].ToString().Trim());
                                iDataCNT2++;
                            }
                        }
                    }
                    dblTemp = dblAccTmp / iDataCNT2;
                    dblAcc2 += dblTemp;
                    dblSum2 += (decimal)Math.Pow(System.Convert.ToDouble(dblTemp), 2);
                }

                if (iRowsCNT < 2)  //자료수 부족
                {
                    return arrReturn;
                }
                if (iDataCNT < 2) //자료수 부족
                {
                    return arrReturn;
                }

                arrReturn[1] = System.Convert.ToString(iRowsCNT * (iEndCol - iStartCol + 1));
                arrReturn[2] = System.Convert.ToString(iDataCNT);
                arrReturn[3] = System.Convert.ToString(dblMin);
                arrReturn[4] = System.Convert.ToString(dblMax);

                dblAvg = dblAcc / iDataCNT;

                arrReturn[5] = System.Convert.ToString(dblAvg);

                dblSigma = (decimal)Math.Sqrt((double)((dblSum / (iDataCNT - 1)) - (dblAvg * dblAvg * iDataCNT / (iDataCNT - 1))));
                dblSigma2 = (decimal)Math.Sqrt((double)((dblSum2 / (iRowsCNT - 1)) - (dblAvg * dblAvg * iRowsCNT / (iRowsCNT - 1))));
                dblSigma2 = dblSigma2 / System.Convert.ToDecimal(Math.Sqrt(iDataCNT2));

                arrReturn[0] = System.Convert.ToString(dblSigma);
                arrReturn[12] = System.Convert.ToString((double)dblAvg + (3 * (double)dblSigma2));
                arrReturn[13] = System.Convert.ToString((double)dblAvg - (3 * (double)dblSigma2));


                if (arrInput[2].Trim() != string.Empty && arrInput[0].Trim() != string.Empty) // 상한 하한 모두 존재
                {
                    dblUSL = System.Convert.ToDouble(arrInput[0].ToString().Trim());
                    dblLSL = System.Convert.ToDouble(arrInput[2].ToString().Trim());
                    //먼저 상한과 하한이 있는 경우 CP를 구함.
                    dblCp = (dblUSL - dblLSL) / (6 * (double)dblSigma);
                    arrReturn[6] = System.Convert.ToString(dblCp);

                    dblCpu = (dblUSL - (double)dblAvg) / (3 * (double)dblSigma);
                    dblCpl = ((double)dblAvg - dblLSL) / (3 * (double)dblSigma);

                    dblM = Math.Round(((dblUSL + dblLSL) / 2), 4);

                    dblK = Math.Abs(dblM - (double)dblAvg) / ((dblUSL - dblLSL) / 2);
                    arrReturn[7] = dblK.ToString();
                    if (arrInput[1] == null || arrInput[1].Trim() == string.Empty) //Target없을 때
                    {
                        dblTarget = (dblUSL + dblLSL) / 2;
                    }
                    else
                    {
                        dblTarget = System.Convert.ToDouble(arrInput[1].ToString().Trim());
                    }

                    if (dblM == dblTarget) //목표치가 스펙의 중심일 때
                    {
                        if (dblCpl < dblCpu)
                        {
                            dblCpk = dblCpl;
                        }
                        else
                        {
                            dblCpk = dblCpu;
                        }
                        arrReturn[8] = System.Convert.ToString(dblCpk);
                    }
                    else
                    {
                        dblSigma_ = (decimal)Math.Sqrt(Math.Pow((double)dblSigma, 2) + Math.Pow(((double)dblAvg - dblTarget), 2));
                        dblCpm = (dblUSL - dblLSL) / (6 * (double)dblSigma_);
                        arrReturn[11] = System.Convert.ToString(dblCpm);
                    }
                }
                else if (arrInput[0].Trim() != string.Empty) // USL만 존재하는 경우
                {
                    dblUSL = System.Convert.ToDouble(arrInput[0].ToString().Trim());
                    dblCpu = (dblUSL - (double)dblAvg) / (3 * (double)dblSigma);
                    arrReturn[9] = System.Convert.ToString(dblCpu);
                }
                else if (arrInput[2].Trim() != string.Empty) // LSL만 존재하는 경우
                {
                    dblLSL = System.Convert.ToDouble(arrInput[2].ToString().Trim());
                    dblCpl = ((double)dblAvg - dblLSL) / (3 * (double)dblSigma);
                    arrReturn[10] = System.Convert.ToString(dblCpl);
                }

                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetStat2
        /// <summary>
        /// 2011-01-22 : YS Lim Create
        /// 통계량을 계산해주는 메소드(Target이 중심이 아닐경우 Cpm만 돌려줌.)
        /// 상세  설명: Data daData와 [USL, Target, LSL]string array을 받아서 [sigma, TotalCount , n, Min, Max, Avg, Cp, K,   Cpk, Cpu, Cpl, Cpm, Ucl, Lcl, Median ]string array 반환<br/>
        ///                                                                  0       1            2  3    4    5    6   7    8    9    10   11   12    13   14
        /// </summary>
        /// <param name="daData"> 2차원(직사각형)으로 된 그룹구분 없는 Data Rows</param>
        /// <param name="arrInput">[USL, Target, LSL]의 string Array</param>
        public static string[] GetStat2(double[] daData, string[] arrInput)
        {
            int iDataCNT = 0;

            string[] arrReturn = new string[15];

            decimal dblAcc = 0;
            decimal dblSum = 0;
            double dblMax = -99999999999999;
            double dblMin = 99999999999999;
            decimal dblSigma;
            //decimal dblSigma2;

            decimal dblAccTmp = 0;
            decimal dblTemp;

            decimal dblAvg;
            double dblCp;
            double dblK;
            double dblCpk;
            double dblCpu;
            double dblCpl;
            double dblCpm;
            double dblUSL;
            double dblTarget;
            double dblLSL;
            double dblM;
            decimal dblSigma_;

            try
            {
                for (int i = 0; i < 15; i++)
                    arrReturn[i] = string.Empty;

                dblAccTmp = 0;
                for (int j = 0; j < daData.Length; j++)
                {
                    if (daData[j] != double.NaN)
                    {
                        if (daData[j] > dblMax) dblMax = daData[j];
                        if (daData[j] < dblMin) dblMin = daData[j];

                        dblAcc += (decimal)daData[j];
                        dblSum += (decimal)Math.Pow(daData[j], 2);

                        dblAccTmp += (decimal)daData[j];
                        iDataCNT++;
                    }
                }
                dblTemp = dblAccTmp / iDataCNT;

                if (daData.Length < 2)  //자료수 부족
                {
                    return arrReturn;
                }
                if (iDataCNT < 2) //자료수 부족
                {
                    return arrReturn;
                }

                arrReturn[1] = System.Convert.ToString(daData.Length);
                arrReturn[2] = System.Convert.ToString(iDataCNT);
                arrReturn[3] = System.Convert.ToString(dblMin);
                arrReturn[4] = System.Convert.ToString(dblMax);

                dblAvg = dblAcc / iDataCNT;

                arrReturn[5] = System.Convert.ToString(dblAvg);

                dblSigma = (decimal)Math.Sqrt((double)((dblSum / (iDataCNT - 1)) - (dblAvg * dblAvg * iDataCNT / (iDataCNT - 1))));
                //dblSigma2 = dblSigma / System.Convert.ToDecimal(Math.Sqrt(iDataCNT));

                arrReturn[0] = System.Convert.ToString(dblSigma);
                arrReturn[12] = System.Convert.ToString((double)dblAvg + (3 * (double)dblSigma));
                arrReturn[13] = System.Convert.ToString((double)dblAvg - (3 * (double)dblSigma));

                if (arrInput[2].Trim() != string.Empty && arrInput[0].Trim() != string.Empty) // 상한 하한 모두 존재
                {
                    dblUSL = System.Convert.ToDouble(arrInput[0].ToString().Trim());
                    dblLSL = System.Convert.ToDouble(arrInput[2].ToString().Trim());
                    //먼저 상한과 하한이 있는 경우 CP를 구함.
                    dblCp = (dblUSL - dblLSL) / (6 * (double)dblSigma);
                    arrReturn[6] = System.Convert.ToString(dblCp);

                    dblCpu = (dblUSL - (double)dblAvg) / (3 * (double)dblSigma);
                    dblCpl = ((double)dblAvg - dblLSL) / (3 * (double)dblSigma);

                    dblM = Math.Round(((dblUSL + dblLSL) / 2), 4);

                    dblK = Math.Abs(dblM - (double)dblAvg) / ((dblUSL - dblLSL) / 2);
                    arrReturn[7] = dblK.ToString();
                    if (arrInput[1] == null || arrInput[1].Trim() == string.Empty) //Target없을 때
                    {
                        dblTarget = (dblUSL + dblLSL) / 2;
                    }
                    else
                    {
                        dblTarget = System.Convert.ToDouble(arrInput[1].ToString().Trim());
                    }

                    if (dblM == dblTarget) //목표치가 스펙의 중심일 때
                    {
                        if (dblCpl < dblCpu)
                        {
                            dblCpk = dblCpl;
                        }
                        else
                        {
                            dblCpk = dblCpu;
                        }
                        arrReturn[8] = System.Convert.ToString(dblCpk);
                    }
                    else
                    {
                        dblSigma_ = (decimal)Math.Sqrt(Math.Pow((double)dblSigma, 2) + Math.Pow(((double)dblAvg - dblTarget), 2));
                        dblCpm = (dblUSL - dblLSL) / (6 * (double)dblSigma_);
                        arrReturn[11] = System.Convert.ToString(dblCpm);
                    }
                }
                else if (arrInput[0].Trim() != string.Empty) // USL만 존재하는 경우
                {
                    dblUSL = System.Convert.ToDouble(arrInput[0].ToString().Trim());
                    dblCpu = (dblUSL - (double)dblAvg) / (3 * (double)dblSigma);
                    arrReturn[9] = System.Convert.ToString(dblCpu);
                }
                else if (arrInput[2].Trim() != string.Empty) // LSL만 존재하는 경우
                {
                    dblLSL = System.Convert.ToDouble(arrInput[2].ToString().Trim());
                    dblCpl = ((double)dblAvg - dblLSL) / (3 * (double)dblSigma);
                    arrReturn[10] = System.Convert.ToString(dblCpl);
                }

                arrReturn[14] = System.Convert.ToString(Median(daData));
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetStat2
        /// <summary>
        /// 2011-01-22 : YS Lim Create
        /// 통계량을 계산해주는 메소드(Target이 중심이 아닐경우 Cpm만 돌려줌.)
        /// 상세  설명: Data daData와 [USL, Target, LSL]string array을 받아서 [sigma, TotalCount , n, Min, Max, Avg, Cp, K, Cpk, Cpu, Cpl, Cpm, Ucl, Lcl, Median]string array 반환<br/>
        ///                                                                   0      1            2  3    4    5    6   7  8    9    10   11   12   13   14
        /// </summary>
        /// <param name="daData"> 1차원으로 된 그룹구분 없는 double data</param>
        public static string[] GetStat2(double[] daData)
        {
            int iDataCNT = 0;

            string[] arrReturn = new string[15];

            decimal dblAcc = 0;
            decimal dblSum = 0;
            double dblMax = -99999999999999;
            double dblMin = 99999999999999;
            decimal dblSigma;
            //decimal dblSigma2;

            decimal dblAccTmp = 0;
            decimal dblTemp;

            decimal dblAvg;
            double dblCp;
            double dblK;
            double dblCpk;
            double dblCpu;
            double dblCpl;
            double dblCpm;
            double dblUSL;
            double dblTarget;
            double dblLSL;
            double dblM;
            decimal dblSigma_;

            try
            {
                for (int i = 0; i < 15; i++)
                    arrReturn[i] = string.Empty;

                Array.Sort(daData);
                dblAccTmp = 0;
                for (int j = 0; j < daData.Length; j++)
                {
                    if (daData[j] != double.NaN)
                    {
                        if (daData[j] > dblMax) dblMax = daData[j];
                        if (daData[j] < dblMin) dblMin = daData[j];

                        dblAcc += (decimal)daData[j];
                        dblSum += (decimal)Math.Pow(daData[j], 2);

                        dblAccTmp += (decimal)daData[j];
                        iDataCNT++;
                    }
                }
                dblTemp = dblAccTmp / iDataCNT;

                if (daData.Length < 2)  //자료수 부족
                {
                    return arrReturn;
                }
                if (iDataCNT < 2) //자료수 부족
                {
                    return arrReturn;
                }

                arrReturn[1] = System.Convert.ToString(daData.Length);
                arrReturn[2] = System.Convert.ToString(iDataCNT);
                arrReturn[3] = System.Convert.ToString(dblMin);
                arrReturn[4] = System.Convert.ToString(dblMax);

                dblAvg = dblAcc / iDataCNT;

                arrReturn[5] = System.Convert.ToString(dblAvg);

                dblSigma = (decimal)Math.Sqrt((double)((dblSum / (iDataCNT - 1)) - (dblAvg * dblAvg * iDataCNT / (iDataCNT - 1))));
                //dblSigma2 = dblSigma / System.Convert.ToDecimal(Math.Sqrt(iDataCNT));

                arrReturn[0] = System.Convert.ToString(dblSigma);
                arrReturn[12] = System.Convert.ToString((double)dblAvg + (3 * (double)dblSigma));
                arrReturn[13] = System.Convert.ToString((double)dblAvg - (3 * (double)dblSigma));


                dblUSL = System.Convert.ToDouble(arrReturn[12].ToString().Trim());
                dblLSL = System.Convert.ToDouble(arrReturn[13].ToString().Trim());
                //먼저 상한과 하한이 있는 경우 CP를 구함.
                dblCp = (dblUSL - dblLSL) / (6 * (double)dblSigma);
                arrReturn[6] = System.Convert.ToString(dblCp);

                dblCpu = (dblUSL - (double)dblAvg) / (3 * (double)dblSigma);
                dblCpl = ((double)dblAvg - dblLSL) / (3 * (double)dblSigma);

                dblM = Math.Round(((dblUSL + dblLSL) / 2), 4);

                dblK = Math.Abs(dblM - (double)dblAvg) / ((dblUSL - dblLSL) / 2);
                arrReturn[7] = dblK.ToString();
                dblTarget = (dblUSL + dblLSL) / 2;

                if (dblM == dblTarget) //목표치가 스펙의 중심일 때
                {
                    if (dblCpl < dblCpu)
                    {
                        dblCpk = dblCpl;
                    }
                    else
                    {
                        dblCpk = dblCpu;
                    }
                    arrReturn[8] = System.Convert.ToString(dblCpk);
                }
                else
                {
                    dblSigma_ = (decimal)Math.Sqrt(Math.Pow((double)dblSigma, 2) + Math.Pow(((double)dblAvg - dblTarget), 2));
                    dblCpm = (dblUSL - dblLSL) / (6 * (double)dblSigma_);
                    arrReturn[11] = System.Convert.ToString(dblCpm);
                }
                arrReturn[14] = System.Convert.ToString(Median(daData));
                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetHistogram
        /// <summary>
        /// 2012-07-13 : YS Lim Create
        /// Histogram 각범위별 Count 계산 메소드
        /// </summary>
        /// <param name="daData"> 1차원으로 된 그룹구분 없는 double data</param>
        public static int[] GetHistogram(double[] daData, double dSlice)
        {
            string[] arrReturn = new string[15];

            double dblMax = -99999999999999;
            double dblMin = 99999999999999;

            int[] iValueStepCount = null;
            int iSliceCount = 0;
            try
            {

                Array.Sort(daData);

                dblMin = daData[0];
                dblMax = daData[daData.Length-1];
                iSliceCount = (int)Math.Round((dblMax - dblMin) / dSlice)+1;
                iValueStepCount = new int[iSliceCount];

                int idx = 0;
                for (int j = 0; j < daData.Length; j++)
                {
                    idx = (int)Math.Truncate((daData[j] - dblMin) / dSlice);
                    iValueStepCount[idx]++;
                }


                return iValueStepCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region GetStat2
        /// <summary>
        /// 통계량을 계산해주는 메소드(Target이 중심이 아닐경우 Cpm만 돌려줌.)
        /// 상세  설명: Data dt와 [USL, Target, LSL]string array을 받아서 [sigma, TotalCount , n, Min, Max, Avg, Cp, K,   Cpk, Cpu, Cpl, Cpm, Ucl, Lcl]string array 반환<br/>
        ///                                                                  0       1         2    3    4    5   6   7    8    9    10   11  12    13
        /// </summary>
        /// <param name="dr"> 2차원(직사각형)으로 된 그룹구분 없는 Data Rows</param>
        /// <param name="arrInput">[USL, Target, LSL]의 string Array</param>
        public static string[] GetStat2(DataRow[] drRows, string[] arrInput, int iStartCol, int iEndCol)
        {
            int iRowsCNT = 0;
            int iDataCNT = 0;

            string[] arrReturn = new string[14];

            decimal dblAcc = 0;
            decimal dblSum = 0;
            double dblMax = -99999999999999;
            double dblMin = 99999999999999;
            decimal dblSigma;

            decimal dblAccTmp = 0;
            decimal dblAcc2 = 0;
            decimal dblSum2 = 0;
            int iDataCNT2 = 0;
            decimal dblTemp;
            decimal dblSigma2;

            decimal dblAvg;
            double dblCp;
            double dblK;
            double dblCpk;
            double dblCpu;
            double dblCpl;
            double dblCpm;
            double dblUSL;
            double dblTarget;
            double dblLSL;
            double dblM;
            decimal dblSigma_;

            try
            {
                for (int i = 0; i < 14; i++)
                    arrReturn[i] = string.Empty;

                iRowsCNT = drRows.Length;

                for (int i = 0; i < iRowsCNT; i++)
                {
                    dblAccTmp = 0;
                    iDataCNT2 = 0;
                    for (int j = iStartCol; j <= iEndCol; j++)
                    {
                        if (drRows[i][j] != DBNull.Value)
                        {
                            if (drRows[i][j].ToString().Trim() != "")
                            {
                                if (System.Convert.ToDouble(drRows[i][j].ToString().Trim()) > dblMax) dblMax = System.Convert.ToDouble(drRows[i][j].ToString().Trim());
                                if (System.Convert.ToDouble(drRows[i][j].ToString().Trim()) < dblMin) dblMin = System.Convert.ToDouble(drRows[i][j].ToString().Trim());

                                dblAcc += System.Convert.ToDecimal(drRows[i][j].ToString().Trim());
                                dblSum += (decimal)Math.Pow(System.Convert.ToDouble(drRows[i][j].ToString().Trim()), 2);
                                iDataCNT++;

                                dblAccTmp += System.Convert.ToDecimal(drRows[i][j].ToString().Trim());
                                iDataCNT2++;
                            }
                        }
                    }
                    dblTemp = dblAccTmp / iDataCNT2;
                    dblAcc2 += dblTemp;
                    dblSum2 += (decimal)Math.Pow(System.Convert.ToDouble(dblTemp), 2);
                }

                if (iRowsCNT < 2)  //자료수 부족
                {
                    return arrReturn;
                }
                if (iDataCNT < 2) //자료수 부족
                {
                    return arrReturn;
                }

                arrReturn[1] = System.Convert.ToString(iRowsCNT * (iEndCol - iStartCol + 1));
                arrReturn[2] = System.Convert.ToString(iDataCNT);
                arrReturn[3] = System.Convert.ToString(dblMin);
                arrReturn[4] = System.Convert.ToString(dblMax);

                dblAvg = dblAcc / iDataCNT;

                arrReturn[5] = System.Convert.ToString(dblAvg);

                dblSigma = (decimal)Math.Sqrt((double)((dblSum / (iDataCNT - 1)) - (dblAvg * dblAvg * iDataCNT / (iDataCNT - 1))));
                dblSigma2 = (decimal)Math.Sqrt((double)((dblSum2 / (iRowsCNT - 1)) - (dblAvg * dblAvg * iRowsCNT / (iRowsCNT - 1))));
                dblSigma2 = dblSigma2 / System.Convert.ToDecimal(Math.Sqrt(iDataCNT2));

                arrReturn[0] = System.Convert.ToString(dblSigma);
                arrReturn[12] = System.Convert.ToString((double)dblAvg + (3 * (double)dblSigma2));
                arrReturn[13] = System.Convert.ToString((double)dblAvg - (3 * (double)dblSigma2));


                if (arrInput[2].Trim() != string.Empty && arrInput[0].Trim() != string.Empty) // 상한 하한 모두 존재
                {
                    dblUSL = System.Convert.ToDouble(arrInput[0].ToString().Trim());
                    dblLSL = System.Convert.ToDouble(arrInput[2].ToString().Trim());
                    //먼저 상한과 하한이 있는 경우 CP를 구함.
                    dblCp = (dblUSL - dblLSL) / (6 * (double)dblSigma);
                    arrReturn[6] = System.Convert.ToString(dblCp);

                    dblCpu = (dblUSL - (double)dblAvg) / (3 * (double)dblSigma);
                    dblCpl = ((double)dblAvg - dblLSL) / (3 * (double)dblSigma);

                    dblM = Math.Round(((dblUSL + dblLSL) / 2), 4);

                    dblK = Math.Abs(dblM - (double)dblAvg) / ((dblUSL - dblLSL) / 2);
                    arrReturn[7] = dblK.ToString();
                    if (arrInput[1] == null || arrInput[1].Trim() == string.Empty) //Target없을 때
                    {
                        dblTarget = (dblUSL + dblLSL) / 2;
                    }
                    else
                    {
                        dblTarget = System.Convert.ToDouble(arrInput[1].ToString().Trim());
                    }

                    if (dblM == dblTarget) //목표치가 스펙의 중심일 때
                    {
                        if (dblCpl < dblCpu)
                        {
                            dblCpk = dblCpl;
                        }
                        else
                        {
                            dblCpk = dblCpu;
                        }
                        arrReturn[8] = System.Convert.ToString(dblCpk);
                    }
                    else
                    {
                        dblSigma_ = (decimal)Math.Sqrt(Math.Pow((double)dblSigma, 2) + Math.Pow(((double)dblAvg - dblTarget), 2));
                        dblCpm = (dblUSL - dblLSL) / (6 * (double)dblSigma_);
                        arrReturn[11] = System.Convert.ToString(dblCpm);
                    }
                }
                else if (arrInput[0].Trim() != string.Empty) // USL만 존재하는 경우
                {
                    dblUSL = System.Convert.ToDouble(arrInput[0].ToString().Trim());
                    dblCpu = (dblUSL - (double)dblAvg) / (3 * (double)dblSigma);
                    arrReturn[9] = System.Convert.ToString(dblCpu);
                }
                else if (arrInput[2].Trim() != string.Empty) // LSL만 존재하는 경우
                {
                    dblLSL = System.Convert.ToDouble(arrInput[2].ToString().Trim());
                    dblCpl = ((double)dblAvg - dblLSL) / (3 * (double)dblSigma);
                    arrReturn[10] = System.Convert.ToString(dblCpl);
                }

                return arrReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetSigma
        // <summary>
        /// dt를 받아서 sigma를 계산해주는 메소드
        /// </summary>
        /// <param name="dt"> 2차원(직사각형)으로 된 그룹구분 없는 Data 테이블</param>
        public static double GetSigma(DataTable dt, int iStartCol, int iEndCol)
        {
            int iRowsCNT = 0;
            int iDataCNT = 0;

            decimal dblAcc = 0;
            decimal dblSum = 0;
            decimal dblSigma;
            decimal dblAvg;

            try
            {
                iRowsCNT = dt.Rows.Count;

                for (int i = 0; i < iRowsCNT; i++)
                {
                    for (int j = iStartCol; j <= iEndCol; j++)
                    {
                        if (dt.Rows[i][j] != DBNull.Value)
                        {
                            if (dt.Rows[i][j].ToString().Trim() != "")
                            {
                                dblAcc += System.Convert.ToDecimal(dt.Rows[i][j].ToString().Trim());
                                dblSum += (decimal)Math.Pow(System.Convert.ToDouble(dt.Rows[i][j].ToString().Trim()), 2);
                                iDataCNT++;
                            }
                        }
                    }
                }
                if (iDataCNT < 2) //자료수 부족
                {
                    return double.NaN;
                }
                dblAvg = dblAcc / iDataCNT;
                dblSigma = (decimal)Math.Sqrt((double)((dblSum / (iDataCNT - 1)) - (dblAvg * dblAvg * iDataCNT / (iDataCNT - 1))));

                return (double)dblSigma;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetRSigma
        //
        //		// <summary>
        //		/// 결과계 데이타를 받아서 sigma 값을 계산해주는 메소드
        //		/// </summary>
        //		private bool GetSigma(ref DataTable dt, bool bResultSystem)
        //		{
        //			bool bGrp = false;
        //			int iMember = 0;
        //			int iRowsCNT = 0;
        //			int iColsCNT = 0;
        //			int iValCNT;
        //			int iTotalValCNT=0;
        //			int iAVGidx;
        //			int iSGMidx;
        //			int iUCLidx;
        //			int iCLidx;
        //			int iLCLidx;
        //			int iRcnt;
        //
        //			double dblAccR;
        //			double dblTmp;
        //			double dblRowAcc;
        //			double dblAcc = 0;
        //			double dblSum = 0;            
        //			double dblAvg;            
        //			double[] arrSum = null;
        //
        //			int iStartValueIdx = 3;
        //
        //			try
        //			{
        //				if (bResultSystem)  //결과계
        //				{
        //					iRowsCNT = dt.Rows.Count;
        //					iColsCNT = dt.Columns.Count;
        //					arrSum = new double[iRowsCNT];
        //					iLCLidx = iColsCNT - 1;
        //					iCLidx = iColsCNT - 2;
        //					iUCLidx = iColsCNT - 3;
        //					iSGMidx = iColsCNT - 4;
        //					iAVGidx = iColsCNT - 5;
        //
        //					#region 한행의 자료수 판단
        //					for (int i = 0; i < iRowsCNT; i++)  //한행의 자료수가 2개이상인지.
        //					{
        //						if (iMember > 1)
        //						{
        //							bGrp = true;
        //							break;
        //						}
        //						iMember = 0;
        //						for (int j = iStartValueIdx; j<iColsCNT - 5; j++)
        //						{
        //							try
        //							{
        //								dblTmp = Convert.ToDouble(dt.Rows[i][j].ToString().Trim());
        //								iMember++;
        //							}
        //							catch
        //							{
        //								break;
        //							}                            
        //						}
        //					}
        //					#endregion 
        //
        //					#region 한행의 자료가 2개이상
        //
        //					if (bGrp) // 한행의 자료수가 2개이상일 경우
        //					{
        //						for (int i = 0; i < iRowsCNT; i++)
        //						{
        //							iValCNT = 0;
        //							dblRowAcc = 0;
        //							dblSum = 0;
        //
        //							for (int j = iStartValueIdx; j < iColsCNT - 5; j++)
        //							{
        //								if (dt.Rows[i][j] != DBNull.Value)
        //								{
        //									if (dt.Rows[i][j].ToString().Trim() != "")
        //									{
        //										try
        //										{
        //											dblTmp = Convert.ToDouble(dt.Rows[i][j].ToString().Trim());
        //										}
        //										catch
        //										{
        //											break;
        //										}
        //										dblRowAcc += dblTmp;
        //										dblAcc += dblTmp;
        //										dblSum += Math.Pow(dblTmp, 2);
        //										iValCNT++;
        //										iTotalValCNT++;
        //									}
        //								}
        //							}
        //							dt.Rows[i][iAVGidx] = dblRowAcc / iValCNT;
        //							dt.Rows[i][2] = iValCNT;
        //							arrSum[i] = dblSum;
        //						}
        //						dblAvg = dblAcc / iTotalValCNT;
        //
        //						dblTmp = 0;
        //
        //						for (int i = 0; i < iRowsCNT; i++)
        //						{
        //							dblTmp += arrSum[i];
        //							dblTmp -= (Convert.ToDouble(dt.Rows[i][2]) * Convert.ToDouble(dt.Rows[i][iAVGidx]) * Convert.ToDouble(dt.Rows[i][iAVGidx]));
        //						}
        //
        //						for (int i = 0; i < iRowsCNT; i++)
        //						{
        //							dt.Rows[i][iSGMidx] = Math.Sqrt(dblTmp / (iTotalValCNT - iRowsCNT) / (Convert.ToDouble(dt.Rows[i][2])));
        //							dt.Rows[i][iUCLidx] = dblAvg + (3 * Convert.ToDouble(dt.Rows[i][iSGMidx]));
        //							dt.Rows[i][iCLidx] = dblAvg;
        //							dt.Rows[i][iLCLidx] = dblAvg - (3 * Convert.ToDouble(dt.Rows[i][iSGMidx]));
        //						}
        //					}
        //						#endregion
        //					else // 한행의 자료수가 하나일 경우
        //					{
        //						dblAccR = 0;                       
        //						iRcnt = 0;
        //						for (int i = 0; i < iRowsCNT; i++)
        //						{
        //							if (dt.Rows[i][iStartValueIdx] != DBNull.Value)
        //							{
        //								if (dt.Rows[i][iStartValueIdx].ToString().Trim() != "")
        //								{
        //									dblAcc += Convert.ToDouble(dt.Rows[i][iStartValueIdx].ToString().Trim());
        //									iTotalValCNT++;
        //									if(i!=0)
        //									{
        //										dblAccR += Math.Abs((Convert.ToDouble(dt.Rows[i][iStartValueIdx].ToString().Trim()) - Convert.ToDouble(dt.Rows[i-1][iStartValueIdx].ToString().Trim()))); 
        //										iRcnt++;
        //									}
        //								}
        //							}
        //						}
        //						dblAvg = dblAcc / iTotalValCNT;
        //						for (int i = 0; i < iRowsCNT; i++)
        //						{
        //							dt.Rows[i][iCLidx] = dblAvg;
        //							dt.Rows[i][iAVGidx] = dblAvg;
        //							dt.Rows[i][iSGMidx] = dblAccR/iRcnt/1.128;
        //							dt.Rows[i][iUCLidx] = dblAvg + (3 * Convert.ToDouble(dt.Rows[i][iSGMidx].ToString()));
        //							dt.Rows[i][iLCLidx] = dblAvg - (3 * Convert.ToDouble(dt.Rows[i][iSGMidx].ToString()));
        //						}
        //					}
        //
        //					dt.AcceptChanges();
        //				}
        //				else //원인계
        //				{
        //				}
        //
        //				return true;
        //			}
        //			catch
        //			{
        //				return false;
        //			}
        //		}		
        //
        #endregion

        #region Rule Check

        /// <summary>
        /// 데이타 값과 스펙값을 받아서 Western Rule 을 체크하여 룰타입을 반환한다.
        /// 체크할 수 없거나 에러가 발생하면 X 를 반환한다.
        /// </summary>
        /// <param name="dt">데이터</param>
        /// <param name="dblSigma">시그마</param>
        /// <param name="dblUCL">관리상한</param>
        /// <param name="dblCL">중심</param>
        /// <param name="dblLCL">관리하한</param>
        /// <param name="strCheckChar">양측스펙, 혹은 상한/하한</param>
        /// <param name="iRuleB1"></param>
        /// <param name="iRuleC1"></param>
        /// <param name="iRuleD1"></param>
        /// <param name="iRuleE1"></param>
        /// <param name="iRuleE2"></param>
        /// <param name="iRuleF1"></param>
        /// <param name="iRuleF2"></param>
        /// <param name="iRuleG1"></param>
        /// <param name="iRuleH1"></param>
        /// <param name="bAVGOOC">평균으로 OOC를 계산할 것인지여부</param>
        /// <param name="iSampleSize"></param>
        /// <returns>Rule</returns>
        public static string RuleCheck(DataTable dt, string strSpecType, double dblSigma
            , double dblUSL, double dblTarget, double dblLSL
            , double dblUCL, double dblCL, double dblLCL
            , int iRuleB1, int iRuleC1, int iRuleD1, int iRuleE1, int iRuleE2, int iRuleF1
            , int iRuleF2, int iRuleG1, int iRuleH1, bool bAVGOOC, int iSampleSize, string strAlarm)
        {
            #region 선언

            double dblTemp;

            double dblAvg;
            double dblCur;
            double dblPrv;

            int iValueCount = 1;
            int i, j;

            int iRows = 0;

            int iPlus;
            int iMinus;

            bool bContinue;

            bool bIncrease;
            bool bSave;

            #endregion

            try
            {
                // 데이터 수

                iRows = dt.Rows.Count;

                // 데이터 개수가 부족한 경우 (N 반환)
                if (iRows < 2) return "N";

                iValueCount = System.Convert.ToInt16(dt.Rows[0]["VCOUNT"]);

                if (iSampleSize != -1)
                {
                    if (iValueCount != iSampleSize)
                        return "X";
                }

                #region Rule S Check

                if (strAlarm.Substring(0, 1).Equals("Y"))
                {
                    if (strSpecType == "5")
                    {
                        // 하한선만 있는 경우
                        for (j = 1; j <= iValueCount; j++)
                        {
                            dblTemp = System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);
                            if (dblTemp < dblLSL) return "S";
                        }
                    }
                    else if (strSpecType == "4")
                    {
                        // 상한선만 있는 경우
                        for (j = 1; j <= iValueCount; j++)
                        {
                            dblTemp = System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);
                            if (dblTemp > dblUSL) return "S";
                        }
                    }
                    else if (dblUSL <= dblLSL)
                    {
                        // 스펙값이 정상이 아닌 경우
                        return "X";
                    }
                    else if (strSpecType == "1" || strSpecType == "2" || strSpecType == "3")
                    {
                        // 양측 스펙인 경우
                        for (j = 1; j <= iValueCount; j++)
                        {
                            dblTemp = System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);
                            if (dblTemp > dblUSL || dblTemp < dblLSL) return "S";
                        }
                    }
                    else
                    {
                        // 스펙값이 없는 경우
                        return "X";
                    }
                }

                #endregion

                #region Rule A Check

                dblTemp = 0;

                if (strAlarm.Substring(1, 1).Equals("Y"))
                {
                    if (bAVGOOC)
                    {
                        // 평균으로 OOC 를 계산할 경우...
                        for (j = 1; j <= iValueCount; j++)
                            dblTemp += System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);

                        dblTemp = dblTemp / iValueCount;

                        switch (strSpecType)
                        {
                            case "1":
                            case "2":
                            case "3":
                                // 양측스펙
                                if (dblTemp > dblUCL || dblTemp < dblLCL) return "A";
                                break;
                            case "4":
                                // 최대치 관리

                                if (dblTemp > dblUCL) return "A";
                                break;
                            case "5":
                                // 최소치 관리

                                if (dblTemp < dblLCL) return "A";
                                break;
                        }
                    }
                    else
                    {
                        //각각의 값으로 OOC 를 계산할 경우...
                        for (j = 1; j <= iValueCount; j++)
                        {
                            dblTemp = System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);

                            switch (strSpecType)
                            {
                                case "1":
                                case "2":
                                case "3":
                                    // 양측스펙
                                    if (dblTemp > dblUCL || dblTemp < dblLCL) return "A";
                                    break;
                                case "4":
                                    // 최대치 관리

                                    if (dblTemp > dblUCL) return "A";
                                    break;
                                case "5":
                                    // 최소치 관리

                                    if (dblTemp < dblLCL) return "A";
                                    break;
                            }
                        }
                    }
                }

                #endregion

                #region Rule B Check = 6

                // 연속으로 n(6) 개가 증가하거나 연속으로 n(6) 개가 감소하는 경우
                if (strAlarm.Substring(2, 1).Equals("Y"))
                {
                    if (iRows >= iRuleB1)
                    {
                        bContinue = true;

                        iPlus = 0;
                        iMinus = 0;

                        dblCur = 0;

                        // 첫번째 행을 초기값으로 먼저 계산한다.
                        for (j = 1; j <= iValueCount; j++)
                            dblCur += System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);

                        dblCur = dblCur / iValueCount;

                        for (i = 1; i < iRuleB1; i++)
                        {
                            dblPrv = 0;

                            for (j = 1; j <= iValueCount; j++)
                                dblPrv += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                            dblPrv = dblPrv / iValueCount;

                            // 6개의 값중 연속으로 하나라도 서로 같으면 무조건 이 룰에 해당되지 않는다.
                            if (dblCur == dblPrv)
                            {
                                bContinue = false;
                                break;
                            }

                            if (dblCur > dblPrv)
                                iPlus++;
                            else
                                iMinus++;

                            // 미리체크한다. 증감의 변동이 있는지...
                            if (iPlus > 0 && iMinus > 0)
                            {
                                bContinue = false;
                                break;
                            }

                            dblCur = dblPrv;
                        }

                        if (bContinue)
                            if ((iPlus == iRuleB1) || (iMinus == iRuleB1))
                                return "B";
                    }
                }
                #endregion

                if (strSpecType == "1" || strSpecType == "2" || strSpecType == "3")
                {
                    // C 는 양측 스펙인 경우만 체크.

                    #region Rule C Check = 9

                    // 연속되는 9개의 점이 CL 을 중심으로 한쪽으로 치우친경우...
                    if (strAlarm.Substring(3, 1).Equals("Y"))
                    {
                        if (iRows >= iRuleC1)
                        {
                            bContinue = true;

                            iPlus = 0;
                            iMinus = 0;

                            for (i = 0; i < iRuleC1; i++)
                            {
                                dblAvg = 0;

                                for (j = 1; j <= iValueCount; j++)
                                    dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                                dblAvg = dblAvg / iValueCount;

                                // 하나라도 CL 값과 같으면 이 룰에 해당되지 않는다.
                                if (dblAvg == dblCL)
                                {
                                    bContinue = false;
                                    break;
                                }

                                if (dblAvg > dblCL)
                                    iPlus++;
                                else
                                    iMinus++;

                                if (iPlus > 0 && iMinus > 0)
                                {
                                    bContinue = false;
                                    break;
                                }
                            }

                            if (bContinue)
                                if ((iPlus > 0 && iMinus == 0) || (iMinus > 0 && iPlus == 0))
                                    return "C";
                        }
                    }

                    #endregion
                }

                #region Rule D Check = 14

                // 연속되는 14개의 점이 번갈아 가면서 증감이 반복되는 경우.
                if (strAlarm.Substring(4, 1).Equals("Y"))
                {
                    if (iRows >= iRuleD1)
                    {
                        bContinue = true;

                        dblCur = 0;
                        dblPrv = 0;

                        for (j = 1; j <= iValueCount; j++)
                            dblCur += System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);

                        dblCur = dblCur / iValueCount;

                        for (j = 1; j <= iValueCount; j++)
                            dblPrv += System.Convert.ToDouble(dt.Rows[1]["VALUE_" + j.ToString()]);

                        dblPrv = dblPrv / iValueCount;

                        bIncrease = false;

                        if (dblCur == dblPrv)
                            bContinue = false;
                        else if (dblCur > dblPrv)
                            bIncrease = true;
                        else
                            bIncrease = false;

                        dblCur = dblPrv;

                        if (bContinue)
                        {
                            bSave = !bIncrease;

                            for (i = 2; i < iRuleD1; i++)
                            {
                                dblPrv = 0;

                                for (j = 1; j <= iValueCount; j++)
                                    dblPrv += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                                dblPrv = dblPrv / iValueCount;

                                // 14중 연속되는 값이 하나라도 서로 같으면 무조건 이 룰에 해당되지 않는다.
                                if (dblCur == dblPrv)
                                {
                                    bContinue = false;
                                    break;
                                }

                                if (dblCur > dblPrv)
                                    bIncrease = true;
                                else
                                    bIncrease = false;

                                if (bSave == bIncrease)
                                {
                                    // 룰에 해당되지 않는다.
                                    bContinue = false;
                                    break;
                                }
                                else
                                    bSave = bIncrease;

                                dblCur = dblPrv;
                            }

                            if (bContinue) return "D";
                        }
                    }
                }
                #endregion

                if (strSpecType == "1" || strSpecType == "2" || strSpecType == "3")
                {
                    // E, F, G, H 는 양측 스펙인 경우만 체크.

                    #region Rule E Check = 3 , 2

                    // 연속되는 3개의 점중 2개 이상이 CL 을 중심으로 동일한 쪽으로 2시그마 이상 떨어진 경우.
                    if (strAlarm.Substring(5, 1).Equals("Y"))
                    {
                        if (iRows >= iRuleE1)
                        {
                            iPlus = 0;
                            iMinus = 0;

                            for (i = 0; i < iRuleE1; i++)
                            {
                                dblAvg = 0;

                                for (j = 1; j <= iValueCount; j++)
                                    dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                                dblAvg = dblAvg / iValueCount;

                                if (dblAvg >= (dblUCL - dblSigma))
                                    iPlus++;

                                if (dblAvg <= (dblLCL + dblSigma))
                                    iMinus++;
                            }

                            if (iPlus >= iRuleE2 || iMinus >= iRuleE2) return "E";
                        }
                    }
                    #endregion

                    #region Rule F Check = 5 , 4

                    // 연속되는 5개의 점중 4개 이상이 CL 로 부터 1시그마 이상 떨어진 경우
                    if (strAlarm.Substring(6, 1).Equals("Y"))
                    {
                        if (iRows >= iRuleF1)
                        {
                            iPlus = 0;
                            iMinus = 0;

                            for (i = 0; i < iRuleF1; i++)
                            {
                                dblAvg = 0;

                                for (j = 1; j <= iValueCount; j++)
                                    dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                                dblAvg = dblAvg / iValueCount;

                                if (dblAvg >= (dblCL + dblSigma))
                                    iPlus++;

                                if (dblAvg <= (dblCL - dblSigma))
                                    iMinus++;
                            }

                            if (iPlus >= iRuleF2 || iMinus >= iRuleF2) return "F";
                        }
                    }
                    #endregion

                    #region Rule G Check = 15

                    // 연속되는 15개의 점이 모두 CL 을 중심으로 1시그마 이내에 있는 경우
                    if (strAlarm.Substring(7, 1).Equals("Y"))
                    {
                        if (iRows >= iRuleG1)
                        {
                            bContinue = true;

                            for (i = 0; i < iRuleG1; i++)
                            {
                                dblAvg = 0;

                                for (j = 1; j <= iValueCount; j++)
                                    dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                                dblAvg = dblAvg / iValueCount;

                                if (dblAvg >= (dblCL + dblSigma) || dblAvg <= (dblCL - dblSigma))
                                {
                                    bContinue = false;
                                    break;
                                }
                            }

                            if (bContinue) return "G";
                        }
                    }
                    #endregion

                    #region Rule H Check = 8

                    // 연속되는 8개의 점이 CL을 중심으로 모두 1시그마 밖에 있는 경우.
                    if (strAlarm.Substring(8, 1).Equals("Y"))
                    {
                        if (iRows >= iRuleH1)
                        {
                            bContinue = true;

                            for (i = 0; i < iRuleH1; i++)
                            {
                                dblAvg = 0;

                                for (j = 1; j <= iValueCount; j++)
                                    dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                                dblAvg = dblAvg / iValueCount;

                                if (dblAvg < (dblCL + dblSigma) && dblAvg > (dblCL - dblSigma))
                                {
                                    bContinue = false;
                                    break;
                                }
                            }

                            if (bContinue) return "H";
                        }
                    }
                    #endregion
                }

                return "N";
            }
            catch
            {
                return "X";
            }
        }

        #endregion

        #region Rule Check

        /// <summary>
        /// 데이타 값과 스펙값을 받아서 Western Rule 을 체크하여 룰타입을 반환한다.
        /// 체크할 수 없거나 에러가 발생하면 X 를 반환한다.
        /// </summary>
        /// <param name="dt">데이터</param>
        /// <param name="dblSigma">시그마</param>
        /// <param name="dblUCL">관리상한</param>
        /// <param name="dblCL">중심</param>
        /// <param name="dblLCL">관리하한</param>
        /// <param name="strCheckChar">양측스펙, 혹은 상한/하한</param>
        /// <param name="iRuleB1"></param>
        /// <param name="iRuleC1"></param>
        /// <param name="iRuleD1"></param>
        /// <param name="iRuleE1"></param>
        /// <param name="iRuleE2"></param>
        /// <param name="iRuleF1"></param>
        /// <param name="iRuleF2"></param>
        /// <param name="iRuleG1"></param>
        /// <param name="iRuleH1"></param>
        /// <param name="bAVGOOC">평균으로 OOC를 계산할 것인지여부</param>
        /// <param name="iSampleSize"></param>
        /// <returns>Rule</returns>
        public static string RuleCheck(DataTable dt, string strSpecType, double dblSigma
            , double dblUSL, double dblTarget, double dblLSL
            , double dblUCL, double dblCL, double dblLCL
            , int iRuleB1, int iRuleC1, int iRuleD1, int iRuleE1, int iRuleE2, int iRuleF1
            , int iRuleF2, int iRuleG1, int iRuleH1, bool bAVGOOC, int iSampleSize)
        {
            #region 선언

            double dblTemp;

            double dblAvg;
            double dblCur;
            double dblPrv;

            int iValueCount = 1;
            int i, j;

            int iRows = 0;

            int iPlus;
            int iMinus;

            bool bContinue;

            bool bIncrease;
            bool bSave;

            #endregion

            try
            {
                // 데이터 수

                iRows = dt.Rows.Count;

                // 데이터 개수가 부족한 경우 (N 반환)
                if (iRows < 2) return "N";

                iValueCount = System.Convert.ToInt16(dt.Rows[0]["VALUE_COUNT"]);

                if (iSampleSize != -1)
                {
                    if (iValueCount != iSampleSize)
                        return "X";
                }

                #region Rule S Check

                if (strSpecType == "5")
                {
                    // 하한선만 있는 경우
                    for (j = 1; j <= iValueCount; j++)
                    {
                        dblTemp = System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);
                        if (dblTemp < dblLSL) return "S";
                    }
                }
                else if (strSpecType == "4")
                {
                    // 상한선만 있는 경우
                    for (j = 1; j <= iValueCount; j++)
                    {
                        dblTemp = System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);
                        if (dblTemp > dblUSL) return "S";
                    }
                }
                else if (dblUSL <= dblLSL)
                {
                    // 스펙값이 정상이 아닌 경우
                    return "X";
                }
                else if (strSpecType == "1" || strSpecType == "2" || strSpecType == "3")
                {
                    // 양측 스펙인 경우
                    for (j = 1; j <= iValueCount; j++)
                    {
                        dblTemp = System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);
                        if (dblTemp > dblUSL || dblTemp < dblLSL) return "S";
                    }
                }
                else
                {
                    // 스펙값이 없는 경우
                    return "X";
                }

                #endregion

                #region Rule A Check

                dblTemp = 0;

                if (bAVGOOC)
                {
                    // 평균으로 OOC 를 계산할 경우...
                    for (j = 1; j <= iValueCount; j++)
                        dblTemp += System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);

                    dblTemp = dblTemp / iValueCount;

                    switch (strSpecType)
                    {
                        case "1":
                        case "2":
                        case "3":
                            // 양측스펙
                            if (dblTemp > dblUCL || dblTemp < dblLCL) return "A";
                            break;
                        case "4":
                            // 최대치 관리

                            if (dblTemp > dblUCL) return "A";
                            break;
                        case "5":
                            // 최소치 관리

                            if (dblTemp < dblLCL) return "A";
                            break;
                    }
                }
                else
                {
                    //각각의 값으로 OOC 를 계산할 경우...
                    for (j = 1; j <= iValueCount; j++)
                    {
                        dblTemp = System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);

                        switch (strSpecType)
                        {
                            case "1":
                            case "2":
                            case "3":
                                // 양측스펙
                                if (dblTemp > dblUCL || dblTemp < dblLCL) return "A";
                                break;
                            case "4":
                                // 최대치 관리

                                if (dblTemp > dblUCL) return "A";
                                break;
                            case "5":
                                // 최소치 관리

                                if (dblTemp < dblLCL) return "A";
                                break;
                        }
                    }
                }

                #endregion

                #region Rule B Check

                // 연속으로 n(6) 개가 증가하거나 연속으로 n(6) 개가 감소하는 경우

                if (iRows >= iRuleB1)
                {
                    bContinue = true;

                    iPlus = 0;
                    iMinus = 0;

                    dblCur = 0;

                    // 첫번째 행을 초기값으로 먼저 계산한다.
                    for (j = 1; j <= iValueCount; j++)
                        dblCur += System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);

                    dblCur = dblCur / iValueCount;

                    for (i = 1; i < iRuleB1; i++)
                    {
                        dblPrv = 0;

                        for (j = 1; j <= iValueCount; j++)
                            dblPrv += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                        dblPrv = dblPrv / iValueCount;

                        // 6개의 값중 연속으로 하나라도 서로 같으면 무조건 이 룰에 해당되지 않는다.
                        if (dblCur == dblPrv)
                        {
                            bContinue = false;
                            break;
                        }

                        if (dblCur > dblPrv)
                            iPlus++;
                        else
                            iMinus++;

                        // 미리체크한다. 증감의 변동이 있는지...
                        if (iPlus > 0 && iMinus > 0)
                        {
                            bContinue = false;
                            break;
                        }

                        dblCur = dblPrv;
                    }

                    if (bContinue)
                        if ((iPlus == iRuleB1) || (iMinus == iRuleB1))
                            return "B";
                }

                #endregion

                if (strSpecType == "1" || strSpecType == "2" || strSpecType == "3")
                {
                    // C 는 양측 스펙인 경우만 체크.

                    #region Rule C Check

                    // 연속되는 9개의 점이 CL 을 중심으로 한쪽으로 치우친경우...

                    if (iRows >= iRuleC1)
                    {
                        bContinue = true;

                        iPlus = 0;
                        iMinus = 0;

                        for (i = 0; i < iRuleC1; i++)
                        {
                            dblAvg = 0;

                            for (j = 1; j <= iValueCount; j++)
                                dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                            dblAvg = dblAvg / iValueCount;

                            // 하나라도 CL 값과 같으면 이 룰에 해당되지 않는다.
                            if (dblAvg == dblCL)
                            {
                                bContinue = false;
                                break;
                            }

                            if (dblAvg > dblCL)
                                iPlus++;
                            else
                                iMinus++;

                            if (iPlus > 0 && iMinus > 0)
                            {
                                bContinue = false;
                                break;
                            }
                        }

                        if (bContinue)
                            if ((iPlus > 0 && iMinus == 0) || (iMinus > 0 && iPlus == 0))
                                return "C";
                    }

                    #endregion
                }

                #region Rule D Check

                // 연속되는 14개의 점이 번갈아 가면서 증감이 반복되는 경우.

                if (iRows >= iRuleD1)
                {
                    bContinue = true;

                    dblCur = 0;
                    dblPrv = 0;

                    for (j = 1; j <= iValueCount; j++)
                        dblCur += System.Convert.ToDouble(dt.Rows[0]["VALUE_" + j.ToString()]);

                    dblCur = dblCur / iValueCount;

                    for (j = 1; j <= iValueCount; j++)
                        dblPrv += System.Convert.ToDouble(dt.Rows[1]["VALUE_" + j.ToString()]);

                    dblPrv = dblPrv / iValueCount;

                    bIncrease = false;

                    if (dblCur == dblPrv)
                        bContinue = false;
                    else if (dblCur > dblPrv)
                        bIncrease = true;
                    else
                        bIncrease = false;

                    dblCur = dblPrv;

                    if (bContinue)
                    {
                        bSave = !bIncrease;

                        for (i = 2; i < iRuleD1; i++)
                        {
                            dblPrv = 0;

                            for (j = 1; j <= iValueCount; j++)
                                dblPrv += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                            dblPrv = dblPrv / iValueCount;

                            // 14중 연속되는 값이 하나라도 서로 같으면 무조건 이 룰에 해당되지 않는다.
                            if (dblCur == dblPrv)
                            {
                                bContinue = false;
                                break;
                            }

                            if (dblCur > dblPrv)
                                bIncrease = true;
                            else
                                bIncrease = false;

                            if (bSave == bIncrease)
                            {
                                // 룰에 해당되지 않는다.
                                bContinue = false;
                                break;
                            }
                            else
                                bSave = bIncrease;

                            dblCur = dblPrv;
                        }

                        if (bContinue) return "D";
                    }
                }

                #endregion

                if (strSpecType == "1" || strSpecType == "2" || strSpecType == "3")
                {
                    // E, F, G, H 는 양측 스펙인 경우만 체크.

                    #region Rule E Check

                    // 연속되는 3개의 점중 2개 이상이 CL 을 중심으로 동일한 쪽으로 2시그마 이상 떨어진 경우.

                    if (iRows >= iRuleE1)
                    {
                        iPlus = 0;
                        iMinus = 0;

                        for (i = 0; i < iRuleE1; i++)
                        {
                            dblAvg = 0;

                            for (j = 1; j <= iValueCount; j++)
                                dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                            dblAvg = dblAvg / iValueCount;

                            if (dblAvg >= (dblUCL - dblSigma))
                                iPlus++;

                            if (dblAvg <= (dblLCL + dblSigma))
                                iMinus++;
                        }

                        if (iPlus >= iRuleE2 || iMinus >= iRuleE2) return "E";
                    }

                    #endregion

                    #region Rule F Check

                    // 연속되는 5개의 점중 4개 이상이 CL 로 부터 1시그마 이상 떨어진 경우

                    if (iRows >= iRuleF1)
                    {
                        iPlus = 0;
                        iMinus = 0;

                        for (i = 0; i < iRuleF1; i++)
                        {
                            dblAvg = 0;

                            for (j = 1; j <= iValueCount; j++)
                                dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                            dblAvg = dblAvg / iValueCount;

                            if (dblAvg >= (dblCL + dblSigma))
                                iPlus++;

                            if (dblAvg <= (dblCL - dblSigma))
                                iMinus++;
                        }

                        if (iPlus >= iRuleF2 || iMinus >= iRuleF2) return "F";
                    }

                    #endregion

                    #region Rule G Check

                    // 연속되는 15개의 점이 모두 CL 을 중심으로 1시그마 이내에 있는 경우

                    if (iRows >= iRuleG1)
                    {
                        bContinue = true;

                        for (i = 0; i < iRuleG1; i++)
                        {
                            dblAvg = 0;

                            for (j = 1; j <= iValueCount; j++)
                                dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                            dblAvg = dblAvg / iValueCount;

                            if (dblAvg >= (dblCL + dblSigma) || dblAvg <= (dblCL - dblSigma))
                            {
                                bContinue = false;
                                break;
                            }
                        }

                        if (bContinue) return "G";
                    }

                    #endregion

                    #region Rule H Check

                    // 연속되는 8개의 점이 CL을 중심으로 모두 1시그마 밖에 있는 경우.

                    if (iRows >= iRuleH1)
                    {
                        bContinue = true;

                        for (i = 0; i < iRuleH1; i++)
                        {
                            dblAvg = 0;

                            for (j = 1; j <= iValueCount; j++)
                                dblAvg += System.Convert.ToDouble(dt.Rows[i]["VALUE_" + j.ToString()]);

                            dblAvg = dblAvg / iValueCount;

                            if (dblAvg < (dblCL + dblSigma) && dblAvg > (dblCL - dblSigma))
                            {
                                bContinue = false;
                                break;
                            }
                        }

                        if (bContinue) return "H";
                    }

                    #endregion
                }

                return "N";
            }
            catch
            {
                return "X";
            }
        }

        #endregion
    }
}
