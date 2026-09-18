using System;
using System.Collections.Generic;
using System.Data;
using CenterSpace.NMath.Stats;

namespace DACrux.BStats.Core
{

    public delegate void Derror(Exception ex);

    public class CapaContinuous
    {
        #region " MEMBER FIELD "

        private DataTable m_Source;
        private int[] m_Columns;
        int m_iDecimalInner = 4;
        int m_iDecimalOuter = 4;
        SelectedCapaContinuous m_InputOption;
        ResultCapaContinuous m_Result;

        #region Message
        private string m_ErrorSpec_Target = "Target is out of spec.";
        private string m_ErrorSpec_LSLmoreUSL = "LSL cannot be greater than USL";
        private string m_ErrorLengthofMovingRange = "Length of moving range must be between 2 and 100.";
        private string m_ErrorKsigma = "K Sigma must be positive natural number.";
        private string m_ErrorSubgroupsize = "More than half of Subgroup should be the same size.";
        private string m_ErrorArgumentContantForSubgroupSize = "Sub group size must be positive natural number.";
        private string m_ErrorStd0 = "Standard deviation is 0. Cannot analysis.";

        #endregion


        private int m_iTempSubgrpSize = 0;
        private int m_iSubgrpSize = 0;

        ///// <summary>
        ///// array of all data
        ///// </summary>
        //private double[] m_arrAllData;        


        #region Event
        public event Derror On_Error;
        #endregion


        #endregion

        #region " CREATOR "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="DataColumns">하나의 Row가 하나의 Lot에 대한 Value임</param>
        /// <param name="InputOption"></param>
        public CapaContinuous(DataTable Source, int[] DataColumns, SelectedCapaContinuous InputOption)
        {
            m_Source = Source;
            m_Columns = DataColumns;
            m_InputOption = InputOption;
            m_Result = new ResultCapaContinuous();
            //m_arrAllData = GetAllData(m_Source, m_Columns, true);
            m_Result = Analysis(m_Source, m_Columns, m_InputOption);
        }
        #endregion

        #region " PROPERTY "
        public ResultCapaContinuous Result
        {
            get
            {
                return m_Result;
            }
        }


        public int DecimalInner
        {
            get { return m_iDecimalInner; }
            set { m_iDecimalInner = value; }
        }

        public int DecimalOuter
        {
            get { return m_iDecimalOuter; }
            set { m_iDecimalOuter = value; }
        }

        #endregion

        #region " METHOD "

        #region [ INNER ]

        // 모든 Data를 하나의 Double 배열로...

        //private double[] GetAllData(DataFrame dfSource, int[] DataColumns, bool bClean)
        //{
        //    double[] arrReturn;
        //    List<double> arrTemp = null;
        //    int iCnt;
        //    int iRow;
        //    try
        //    {
        //        if (DataColumns == null)
        //            throw new ArgumentException("Argument(parameter) is null.");

        //        iRow = dfSource.Rows;
        //        iCnt = DataColumns.Length;
        //        arrTemp = new List<double>();

        //        for (int i = 0; i < iCnt; i++)
        //        {
        //            if(bClean)
        //                arrTemp.AddRange(((IDFColumn)dfSource[DataColumns[i]]).Clean().ToDoubleArray());
        //            else
        //                arrTemp.AddRange(((IDFColumn)dfSource[DataColumns[i]]).ToDoubleArray());
        //        }
        //        arrReturn = arrTemp.ToArray();
        //        return arrReturn;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        #region [ ANALYSIS ]

        private ResultCapaContinuous Analysis(DataTable dtSource, int[] arrDataColumns, SelectedCapaContinuous oInputOption)
        {
            ResultCapaContinuous oReturn;
            bool bUSL = false;  //상한 스펙 있는지 여부
            bool bLSL = false;  //하한 스펙 있는지 여부
            bool bTarget = false;  //목표값 스펙 있는지 여부
            double dblUSL = double.NaN;
            double dblLSL = double.NaN;
            double dblTarget = double.NaN;
            double dblKsigma = double.NaN;
            double dblLengthofMovingRange = double.NaN;
            int iRowCnt;
            int iDataColCnt;
            //double dblAvgSubgroupSize;
            //double[] m_arrAllData;
            double[] arrAllCleanData;


            #region degree of freedom
            int idfOverall;

            //int idfBW;
            #endregion


            double dblMean = double.NaN;

            #region Sigma
            double dblSigmaWithin = double.NaN;
            double dblSigmaBetween = double.NaN;
            double dblSigmaBW = double.NaN;
            double dblSigmaOverAll = double.NaN;
            double dblSigmaxbar = double.NaN;
            #endregion

            #region Performance

            #region Observed Performance
            double dblObserved_PPMlessLSL = double.NaN;
            double dblObserved_PPMmoreUSL = double.NaN;
            double dblObserved_PPMtotal = double.NaN;
            #endregion

            #region Exp. Within Performance
            double dblWithin_PPMlessLSL = double.NaN;
            double dblWithin_PPMmoreUSL = double.NaN;
            double dblWithin_PPMtotal = double.NaN;
            #endregion

            #region Exp. B/W Performance
            double dblBW_PPMlessLSL = double.NaN;
            double dblBW_PPMmoreUSL = double.NaN;
            double dblBW_PPMtotal = double.NaN;
            #endregion

            #region Exp. Overall Performance
            double dblOverall_PPMlessLSL = double.NaN;
            double dblOverall_PPMmoreUSL = double.NaN;
            double dblOverall_PPMtotal = double.NaN;
            #endregion

            #endregion

            #region Capability

            #region Within Capability
            double dblWithin_CP = double.NaN;
            double dblWithin_Cpl = double.NaN;
            double dblWithin_Cpu = double.NaN;
            double dblWithin_CPK = double.NaN;
            double dblWithin_CCPK = double.NaN;
            #endregion

            #region B/W Capability
            double dblBW_CP = double.NaN;
            double dblBW_Cpl = double.NaN;
            double dblBW_Cpu = double.NaN;
            double dblBW_CPK = double.NaN;
            double dblBW_CCPK = double.NaN;
            #endregion

            #region Overall Capability
            double dblOverall_PP = double.NaN;
            double dblOverall_Ppl = double.NaN;
            double dblOverall_Ppu = double.NaN;
            double dblOverall_PPK = double.NaN;
            double dblOverall_Cpm = double.NaN;
            #endregion

            #endregion

            #region Z Bench

            double dblZlslWithin = double.NaN;
            double dblZuslWithin = double.NaN;
            double dblZbenchWithin = double.NaN;
            double dblZlslBW = double.NaN;
            double dblZuslBW = double.NaN;
            double dblZbenchBW = double.NaN;
            double dblZlslOverall = double.NaN;
            double dblZuslOverall = double.NaN;
            double dblZbenchOverall = double.NaN;

            #endregion

            try
            {
                #region Input Check

                if (dtSource == null)
                    throw new ArgumentException(ErrorNullParameter("dtSource"));
                iRowCnt = dtSource.Rows.Count;
                if (iRowCnt == 0)
                    throw new ArgumentException(ErrorNullParameter("dtSource"));

                if (arrDataColumns == null)
                    throw new ArgumentException(ErrorNullParameter("arrDataColumns"));

                iDataColCnt = arrDataColumns.Length;
                if (iDataColCnt == 0)
                    throw new ArgumentException(ErrorNullParameter("arrDataColumns"));

                //About Spec
                dblUSL = oInputOption.UpperSpec;
                dblLSL = oInputOption.LowerSpec;
                dblTarget = oInputOption.SelectedCapaContinuousOptions.Target;
                if (double.IsNaN(dblUSL) && double.IsNaN(dblLSL))
                {
                    if (On_Error != null)
                    {
                        On_Error(new ArgumentException(ErrorNullParameter("USL, LSL"), new Exception("MIRACOM")));
                    }
                    else
                    {
                        throw new ArgumentException(ErrorNullParameter("USL, LSL"), new Exception("MIRACOM"));
                    }
                }
                else
                {
                    if (!double.IsNaN(dblUSL) && !double.IsNaN(dblLSL))
                    {
                        bUSL = true;
                        bLSL = true;
                        if (dblUSL == dblLSL)
                        {
                            if (On_Error != null)
                                On_Error(new ArgumentException(ErrorWrongParameter("USL, LSL"), new Exception("MIRACOM")));
                            else
                                throw new ArgumentException(ErrorWrongParameter("USL, LSL"), new Exception("MIRACOM"));
                        }

                        if (dblUSL < dblLSL)
                        {
                            if (On_Error != null)
                                On_Error(new ArgumentException(m_ErrorSpec_LSLmoreUSL, new Exception("MIRACOM")));
                            else
                                throw new ArgumentException(m_ErrorSpec_LSLmoreUSL, new Exception("MIRACOM"));

                        }
                        if (!double.IsNaN(dblTarget))
                        {
                            bTarget = true;
                            //Target Check
                            if (dblTarget < dblLSL || dblTarget > dblUSL)
                            {
                                if (On_Error != null)
                                    On_Error(new ArgumentException(m_ErrorSpec_Target, new Exception("MIRACOM")));
                                else
                                    throw new ArgumentException(m_ErrorSpec_Target, new Exception("MIRACOM"));
                            }
                        }
                    }
                    else if (double.IsNaN(dblUSL) && !double.IsNaN(dblLSL))
                    {
                        bLSL = true;
                        if (!double.IsNaN(dblTarget))
                        {
                            bTarget = true;
                            //Target Check
                            if (dblTarget < dblLSL)
                            {
                                if (On_Error != null)
                                    On_Error(new ArgumentException(m_ErrorSpec_Target, new Exception("MIRACOM")));
                                else
                                    throw new ArgumentException(m_ErrorSpec_Target, new Exception("MIRACOM"));
                            }
                        }
                    }
                    else // if (!double.IsNaN(dblUSL) && double.IsNaN(dblLSL))
                    {
                        bUSL = true;
                        if (!double.IsNaN(dblTarget))
                        {
                            bTarget = true;
                            //Target Check
                            if (dblTarget > dblUSL)
                            {
                                if (On_Error != null)
                                    On_Error(new ArgumentException(m_ErrorSpec_Target, new Exception("MIRACOM")));
                                else
                                    throw new ArgumentException(m_ErrorSpec_Target, new Exception("MIRACOM"));
                            }
                        }
                    }

                }

                //이동 범위에 사용될 길이
                dblLengthofMovingRange = (double)oInputOption.Estimate.MovingRangeofLength;
                if (dblLengthofMovingRange < 2 || dblLengthofMovingRange > 100)
                {
                    if (On_Error != null)
                        On_Error(new ArgumentException(m_ErrorLengthofMovingRange, new Exception("MIRACOM")));
                    else
                        throw new ArgumentException(m_ErrorLengthofMovingRange, new Exception("MIRACOM"));
                }

                //K Sigma : default=6
                dblKsigma = oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability;
                if (dblKsigma <= 0)
                {
                    if (On_Error != null)
                        On_Error(new ArgumentException(m_ErrorKsigma, new Exception("MIRACOM")));
                    else
                        throw new ArgumentException(m_ErrorKsigma, new Exception("MIRACOM"));
                }

                #endregion

                oReturn = new ResultCapaContinuous();

                #region Sigma & Infomation

                oReturn.USL = dblUSL;
                oReturn.Target = dblTarget;
                oReturn.LSL = dblLSL;

                arrAllCleanData = GetAllData(dtSource, arrDataColumns, true);
                idfOverall = arrAllCleanData.Length;
                dblSigmaOverAll = GetSigmaOverall(arrAllCleanData, oInputOption.Estimate.IsUseUnbiasingOverall);
                if (double.IsNaN(dblSigmaOverAll) || dblSigmaOverAll == 0)
                {
                    if (On_Error != null)
                        On_Error(new Exception(m_ErrorStd0, new Exception("MIRACOM")));
                    else
                        throw new Exception(m_ErrorStd0, new Exception("MIRACOM"));
                    
                }
                if (iDataColCnt == 1)
                {

                    switch (oInputOption.Estimate.EstimationBetweenSubgroup)
                    {
                        //case EstimationBetween.Average_moving_range:
                        //    dblSigmaxbar = GetSigmaXbarAvg(dtSource, arrDataColumns, oInputOption.Estimate.MovingRangeofLength, oInputOption.Estimate.IsUseUnbiasingWithin);
                        //    break;
                        case EstimationBetween.Median_moving_range:
                            dblSigmaxbar = GetSigmaXbarMedian(dtSource, arrDataColumns, oInputOption.Estimate.MovingRangeofLength, oInputOption.Estimate.IsUseUnbiasingWithin, out m_iSubgrpSize);
                            break;
                        case EstimationBetween.Square_root_of_MSSD:
                            dblSigmaxbar = GetSigmaXbarSMSSD(dtSource, arrDataColumns, oInputOption.Estimate.IsUseUnbiasingWithin, out m_iSubgrpSize);
                            break;
                        default:
                            dblSigmaxbar = GetSigmaXbarAvg(dtSource, arrDataColumns, oInputOption.Estimate.MovingRangeofLength, oInputOption.Estimate.IsUseUnbiasingWithin, out m_iSubgrpSize);
                            break;
                    }
                    m_iSubgrpSize = 1;
                    dblSigmaWithin = dblSigmaxbar;

                }
                else // Subgroup Size가 1이상인 경우
                {
                    switch (oInputOption.Estimate.EstimationWithinSubgroup)
                    {
                        //case EstimationWithin.Pooled_standard_deviation:
                        //    {
                        //        dblSigmaWithin = GetSigmaWithin_Pooled(dtSource, arrDataColumns, oInputOption.Estimate.IsUseUnbiasingWithin);
                        //    }
                        //    break;
                        case EstimationWithin.Rbar:
                            dblSigmaWithin = GetSigmaWithin_Rbar(dtSource, arrDataColumns);
                            break;
                        case EstimationWithin.Sbar:
                            dblSigmaWithin = GetSigmaWithin_Sbar(dtSource, arrDataColumns, oInputOption.Estimate.IsUseUnbiasingWithin);
                            break;
                        default:
                            dblSigmaWithin = GetSigmaWithin_Pooled(dtSource, arrDataColumns, oInputOption.Estimate.IsUseUnbiasingWithin);
                            break;
                    }
                    switch (oInputOption.Estimate.EstimationBetweenSubgroup)
                    {
                        //case EstimationBetween.Average_moving_range:
                        //    dblSigmaxbar = GetSigmaXbarAvg(dtSource, arrDataColumns, oInputOption.Estimate.MovingRangeofLength, oInputOption.Estimate.IsUseUnbiasingWithin);  
                        //    break;
                        case EstimationBetween.Median_moving_range:
                            dblSigmaxbar = GetSigmaXbarMedian(dtSource, arrDataColumns, oInputOption.Estimate.MovingRangeofLength, oInputOption.Estimate.IsUseUnbiasingWithin, out m_iSubgrpSize);
                            break;
                        case EstimationBetween.Square_root_of_MSSD:
                            dblSigmaxbar = GetSigmaXbarSMSSD(dtSource, arrDataColumns, oInputOption.Estimate.IsUseUnbiasingWithin, out m_iSubgrpSize);
                            break;
                        default:
                            dblSigmaxbar = GetSigmaXbarAvg(dtSource, arrDataColumns, oInputOption.Estimate.MovingRangeofLength, oInputOption.Estimate.IsUseUnbiasingWithin, out m_iSubgrpSize);
                            break;
                    }
                    dblSigmaBetween = GetSigmaBetween(dblSigmaxbar, dblSigmaWithin, m_iSubgrpSize);
                    dblSigmaBW = GetSigmaBW(dblSigmaBetween, dblSigmaWithin);
                }

                #endregion

                #region Performance

                dblMean = StatsFunctions.Mean(arrAllCleanData);

                #region Observed

                if (bLSL && bUSL)
                {
                    dblObserved_PPMlessLSL = GetObslssLSL(arrAllCleanData, dblLSL);
                    dblObserved_PPMmoreUSL = GetObslssUSL(arrAllCleanData, dblUSL);
                    dblObserved_PPMtotal = dblObserved_PPMlessLSL + dblObserved_PPMmoreUSL;
                }
                else if (bLSL)
                {
                    dblObserved_PPMlessLSL = GetObslssLSL(arrAllCleanData, dblLSL);
                    dblObserved_PPMtotal = dblObserved_PPMlessLSL;
                }
                else //if (!double.IsNaN(dblLSL))
                {
                    dblObserved_PPMmoreUSL = GetObslssUSL(arrAllCleanData, dblUSL);
                    dblObserved_PPMtotal = dblObserved_PPMmoreUSL;
                }

                #endregion

                #region Within

                if (bLSL && bUSL)
                {
                    dblWithin_PPMlessLSL = GetPPMlessLSL(dblMean, dblLSL, dblSigmaWithin);
                    dblWithin_PPMmoreUSL = GetPPMmoreUSL(dblMean, dblUSL, dblSigmaWithin);
                    dblWithin_PPMtotal = dblWithin_PPMlessLSL + dblWithin_PPMmoreUSL;
                }
                else if (bLSL)
                {
                    dblWithin_PPMlessLSL = GetPPMlessLSL(dblMean, dblLSL, dblSigmaWithin);
                    dblWithin_PPMtotal = dblWithin_PPMlessLSL;
                }
                else //if (!double.IsNaN(dblLSL))
                {
                    dblWithin_PPMmoreUSL = GetPPMmoreUSL(dblMean, dblUSL, dblSigmaWithin);
                    dblWithin_PPMtotal = dblWithin_PPMmoreUSL;
                }

                #endregion

                #region BW
                if (iDataColCnt > 1)
                {
                    if (bLSL && bUSL)
                    {
                        dblBW_PPMlessLSL = GetPPMlessLSL(dblMean, dblLSL, dblSigmaBW);
                        dblBW_PPMmoreUSL = GetPPMmoreUSL(dblMean, dblUSL, dblSigmaBW);
                        dblBW_PPMtotal = dblBW_PPMlessLSL + dblBW_PPMmoreUSL;
                    }
                    else if (bLSL)
                    {
                        dblBW_PPMlessLSL = GetPPMlessLSL(dblMean, dblLSL, dblSigmaBW);
                        dblBW_PPMtotal = dblBW_PPMlessLSL;
                    }
                    else// if (bUSL)
                    {
                        dblBW_PPMmoreUSL = GetPPMmoreUSL(dblMean, dblUSL, dblSigmaBW);
                        dblBW_PPMtotal = dblBW_PPMmoreUSL;
                    }
                }
                #endregion

                #region Overall

                if (bLSL && bUSL)
                {
                    dblOverall_PPMlessLSL = GetPPMlessLSL(dblMean, dblLSL, dblSigmaOverAll);
                    dblOverall_PPMmoreUSL = GetPPMmoreUSL(dblMean, dblUSL, dblSigmaOverAll);
                    dblOverall_PPMtotal = dblOverall_PPMlessLSL + dblOverall_PPMmoreUSL;
                }
                else if (bLSL)
                {
                    dblOverall_PPMlessLSL = GetPPMlessLSL(dblMean, dblLSL, dblSigmaOverAll);
                    dblOverall_PPMtotal = dblOverall_PPMlessLSL;
                }
                else// if (bUSL)
                {
                    dblOverall_PPMmoreUSL = GetPPMmoreUSL(dblMean, dblUSL, dblSigmaOverAll);
                    dblOverall_PPMtotal = dblOverall_PPMmoreUSL;
                }

                #endregion

                #endregion

                #region Capability

                #region Within

                if (bLSL && bUSL)
                    dblWithin_CP = GetCp(dblUSL, dblLSL, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaWithin);
                if (bLSL)
                    dblWithin_Cpl = GetCPL(dblMean, dblLSL, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaWithin);
                if (bUSL)
                    dblWithin_Cpu = GetCPU(dblMean, dblUSL, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaWithin);

                dblWithin_CPK = GetCpk(dblWithin_Cpl, dblWithin_Cpu);
                dblWithin_CCPK = GetCCpk(dblLSL, dblUSL, dblTarget, dblMean, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaWithin);

                #endregion

                #region BW
                if (iDataColCnt > 1)
                {
                    if (bLSL && bUSL)
                        dblBW_CP = GetCp(dblUSL, dblLSL, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaBW);
                    if (bLSL)
                        dblBW_Cpl = GetCPL(dblMean, dblLSL, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaBW);
                    if (bUSL)
                        dblBW_Cpu = GetCPU(dblMean, dblUSL, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaBW);

                    dblBW_CPK = GetCpk(dblBW_Cpl, dblBW_Cpu);
                    dblBW_CCPK = GetCCpk(dblLSL, dblUSL, dblTarget, dblMean, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaBW);
                }
                #endregion

                #region Overall
                if (bLSL && bUSL)
                    dblOverall_PP = GetCp(dblUSL, dblLSL, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaOverAll);
                if (bLSL)
                    dblOverall_Ppl = GetCPL(dblMean, dblLSL, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaOverAll);
                if (bUSL)
                    dblOverall_Ppu = GetCPU(dblMean, dblUSL, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, dblSigmaOverAll);

                dblOverall_PPK = GetCpk(dblOverall_Ppl, dblOverall_Ppu);
                if (bTarget)
                    dblOverall_Cpm = GetCpm(dblLSL, dblUSL, dblTarget, oInputOption.SelectedCapaContinuousOptions.KsigmaForCapability, arrAllCleanData);
                #endregion

                #endregion

                #region Z bench
                if (oInputOption.SelectedCapaContinuousOptions.ResultStatisticType == StatisticType.Benchmark_Z)
                {
                    #region Within
                    if (bLSL)
                        dblZlslWithin = GetZlsl(dblMean, dblLSL, dblSigmaWithin);
                    if (bUSL)
                        dblZuslWithin = GetZusl(dblMean, dblUSL, dblSigmaWithin);
                    dblZbenchWithin = GetZbench(dblMean, dblZlslWithin, dblZuslWithin);
                    #endregion

                    #region B/W
                    if (iDataColCnt > 1)
                    {
                        if (bLSL)
                            dblZlslBW = GetZlsl(dblMean, dblLSL, dblSigmaBW);
                        if (bUSL)
                            dblZuslBW = GetZusl(dblMean, dblUSL, dblSigmaBW);
                        dblZbenchBW = GetZbench(dblMean, dblZlslBW, dblZuslBW);
                    }
                    #endregion

                    #region Overall
                    if (bLSL)
                        dblZlslOverall = GetZlsl(dblMean, dblLSL, dblSigmaOverAll);
                    if (bUSL)
                        dblZuslOverall = GetZusl(dblMean, dblUSL, dblSigmaOverAll);
                    dblZbenchOverall = GetZbench(dblMean, dblZlslOverall, dblZuslOverall);
                    #endregion

                }
                #endregion

                #region Order Return
                double dblTemp;
                if (oInputOption.SelectedCapaContinuousOptions.ResultDisplayType == DisplayType.Parts_per_million)
                    dblTemp = 1000000;
                else
                    dblTemp = 100;
                oReturn.BW_PPMlessLSL = Math.Round(dblBW_PPMlessLSL * dblTemp, m_iDecimalOuter);
                oReturn.BW_PPMmoreUSL = Math.Round(dblBW_PPMmoreUSL * dblTemp, m_iDecimalOuter);
                oReturn.BW_PPMtotal = Math.Round(dblBW_PPMtotal * dblTemp, m_iDecimalOuter);
                oReturn.CCpkBW = Math.Round(dblBW_CCPK, m_iDecimalOuter);
                oReturn.CCpkWithin = Math.Round(dblWithin_CCPK, m_iDecimalOuter);
                oReturn.CpBW = Math.Round(dblBW_CP, m_iDecimalOuter);
                oReturn.CpkBW = Math.Round(dblBW_CPK, m_iDecimalOuter);
                oReturn.CpkWithin = Math.Round(dblWithin_CPK, m_iDecimalOuter);
                oReturn.CPLBW = Math.Round(dblBW_Cpl, m_iDecimalOuter);
                oReturn.CPLWithin = Math.Round(dblWithin_Cpl, m_iDecimalOuter);
                oReturn.Cpm = Math.Round(dblOverall_Cpm, m_iDecimalOuter);
                oReturn.CPUBW = Math.Round(dblBW_Cpu, m_iDecimalOuter);
                oReturn.CPUWithin = Math.Round(dblWithin_Cpu, m_iDecimalOuter);
                oReturn.CpWithin = Math.Round(dblWithin_CP, m_iDecimalOuter);
                oReturn.LSL = dblLSL;
                oReturn.Mean = Math.Round(dblMean, m_iDecimalOuter);
                oReturn.N = idfOverall;
                oReturn.Observed_PPMlessLSL = Math.Round(dblObserved_PPMlessLSL * dblTemp, m_iDecimalOuter);
                oReturn.Observed_PPMmoreUSL = Math.Round(dblObserved_PPMmoreUSL * dblTemp, m_iDecimalOuter);
                oReturn.Observed_PPMtotal = Math.Round(dblObserved_PPMtotal * dblTemp, m_iDecimalOuter);
                oReturn.Overall_PPMlessLSL = Math.Round(dblOverall_PPMlessLSL * dblTemp, m_iDecimalOuter);
                oReturn.Overall_PPMmoreUSL = Math.Round(dblOverall_PPMmoreUSL * dblTemp, m_iDecimalOuter);
                oReturn.Overall_PPMtotal = Math.Round(dblOverall_PPMtotal * dblTemp, m_iDecimalOuter);
                oReturn.Pp = Math.Round(dblOverall_PP, m_iDecimalOuter);
                oReturn.Ppk = Math.Round(dblOverall_PPK, m_iDecimalOuter);
                oReturn.PPL = Math.Round(dblOverall_Ppl, m_iDecimalOuter);
                oReturn.PPU = Math.Round(dblOverall_Ppu, m_iDecimalOuter);
                oReturn.Std_Between = Math.Round(dblSigmaBetween, m_iDecimalOuter);
                oReturn.Std_BW = Math.Round(dblSigmaBW, m_iDecimalOuter);
                oReturn.Std_Overall = Math.Round(dblSigmaOverAll, m_iDecimalOuter);
                oReturn.Std_Within = Math.Round(dblSigmaWithin, m_iDecimalOuter);
                oReturn.Target = dblTarget;
                oReturn.USL = dblUSL;
                oReturn.Within_PPMlessLSL = Math.Round(dblWithin_PPMlessLSL * dblTemp, m_iDecimalOuter);
                oReturn.Within_PPMmoreUSL = Math.Round(dblWithin_PPMmoreUSL * dblTemp, m_iDecimalOuter);
                oReturn.Within_PPMtotal = Math.Round(dblWithin_PPMtotal * dblTemp, m_iDecimalOuter);
                oReturn.ZbenchBW = Math.Round(dblZbenchBW, m_iDecimalOuter);
                oReturn.ZbenchOverall = Math.Round(dblZbenchOverall, m_iDecimalOuter);
                oReturn.ZbenchWithin = Math.Round(dblZbenchWithin, m_iDecimalOuter);
                oReturn.ZlslBW = Math.Round(dblZlslBW, m_iDecimalOuter);
                oReturn.ZlslOverall = Math.Round(dblZlslOverall, m_iDecimalOuter);
                oReturn.ZlslWithin = Math.Round(dblZlslWithin, m_iDecimalOuter);
                oReturn.ZuslBW = Math.Round(dblZuslBW, m_iDecimalOuter);
                oReturn.ZuslOverall = Math.Round(dblZuslOverall, m_iDecimalOuter);
                oReturn.ZuslWithin = Math.Round(dblZuslWithin, m_iDecimalOuter);
                #endregion

                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ INFO ]

        private double GetSigma(double[] arrData)
        {
            try
            {
                return Math.Round(StatsFunctions.StandardDeviation(arrData, BiasType.Unbiased), m_iDecimalOuter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private double GetMean(double[] arrData)
        {
            try
            {
                return Math.Round(StatsFunctions.Mean(arrData), m_iDecimalOuter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int GetCount(double[] arrData)
        {
            try
            {
                return StatsFunctions.NaNCount(arrData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ SIGMA ]

        #region Sigma of Within

        #region Pooled Within Std
        private double GetSigmaWithin_Pooled(DataTable dt, int[] arrColumns, bool Unbiased)
        {
            double dblSp;
            double dblTotalSS;
            double dblDegrees;
            double dblC4d;
            int iRowCnt;
            int iColCnt;

            try
            {
                if (dt == null)
                    throw new ArgumentException(ErrorNullParameter("dt"));
                if (arrColumns == null)
                    throw new ArgumentException(ErrorNullParameter("arrColumns"));
                iColCnt = arrColumns.Length;
                iRowCnt = dt.Rows.Count;
                if (iColCnt < 2)
                    throw new ArgumentException(ErrorWrongParameter("arrColumns"));
                if (iRowCnt == 0)
                    throw new ArgumentException(ErrorWrongParameter("dt"));





                dblTotalSS = 0;  //각 부분군별 전체 오차 제곱합
                dblDegrees = 0; //자유도 계산을 위한 관측치의 누적합
                for (int i = 0; i < iRowCnt; i++)
                {
                    List<double> arrTemp = new List<double>();
                    //iNonNanColCnt = 0;
                    for (int j = 0; j < iColCnt; j++)
                    {
                        if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                        {
                            arrTemp.Add(Convert.ToDouble(dt.Rows[i][arrColumns[j]]));
                        }
                        //iNonNanColCnt++;
                    }
                    if (arrTemp.Count > 1)
                    {
                        dblTotalSS += StatsFunctions.SumOfSquaredErrors(arrTemp.ToArray());
                        //dblTotalSS += StatsFunctions.SumOfSquares(arrTemp.ToArray());  //이거 뭔 차이야???
                        dblDegrees += (arrTemp.Count - 1);
                    }
                    arrTemp = null;
                }
                dblSp = Math.Sqrt(dblTotalSS / dblDegrees);
                if (Unbiased)
                {
                    dblC4d = GetC4(1 + dblDegrees);
                    return dblSp / dblC4d;
                }
                else
                    return dblSp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Rbar Within Std
        private double GetSigmaWithin_Rbar(DataTable dt, int[] arrColumns)
        {
            double dblSr;
            double dblCumulative;
            double dblFcumulative;
            int iRowCnt;
            int iColCnt;
            int iModeSubgrpSize; //가장 많은 부분군 크기
            int iModeCount;  // 가장 많은 부분군 크기의 개수
            List<int> arrSubgrpSize;
            bool bSubgroupSizeSame = true;
            try
            {
                if (dt == null)
                    throw new ArgumentException(ErrorNullParameter("dt"));
                if (arrColumns == null)
                    throw new ArgumentException(ErrorNullParameter("arrColumns"));
                iColCnt = arrColumns.Length;
                iRowCnt = dt.Rows.Count;
                if (iColCnt < 2)
                    throw new ArgumentException(ErrorWrongParameter("arrColumns"));
                if (iRowCnt == 0)
                    throw new ArgumentException(ErrorWrongParameter("dt"));


                #region Mode SubGroup Size Check

                arrSubgrpSize = new List<int>();
                for (int i = 0; i < iRowCnt; i++)
                {
                    int iValueCnt = 0;
                    for (int j = 0; j < iColCnt; j++)
                    {
                        if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            iValueCnt++;
                    }
                    arrSubgrpSize.Add(iValueCnt);
                }
                List<int> arrDistinctCount = new List<int>();
                foreach (int oCount in arrSubgrpSize)
                {
                    if (!arrDistinctCount.Contains(oCount))
                        arrDistinctCount.Add(oCount);
                }
                iModeSubgrpSize = StatsFunctions.Mode(arrSubgrpSize.ToArray());
                if (iModeSubgrpSize < 1)
                {
                    if (On_Error != null)
                        On_Error(new Exception(m_ErrorArgumentContantForSubgroupSize, new Exception("MIRACOM")));
                    else
                        throw new Exception(m_ErrorArgumentContantForSubgroupSize, new Exception("MIRACOM"));
                }
                m_iTempSubgrpSize = iModeSubgrpSize;
                iModeCount = StatsFunctions.CountIf(arrSubgrpSize.ToArray(), new StatsFunctions.LogicalDoubleFunction(GetMode));


                foreach (int oCount in arrDistinctCount)
                {
                    if (oCount != iModeSubgrpSize)
                    {
                        m_iTempSubgrpSize = oCount;
                        if (iModeCount == StatsFunctions.CountIf(arrSubgrpSize.ToArray(), new StatsFunctions.LogicalDoubleFunction(GetMode)))
                        {
                            if (On_Error != null)
                                On_Error(new Exception(m_ErrorSubgroupsize, new Exception("MIRACOM")));
                            else
                                throw new Exception(m_ErrorSubgroupsize, new Exception("MIRACOM"));
                        }
                    }
                }
                #endregion

                if (iModeCount == iRowCnt)
                    bSubgroupSizeSame = true;
                else
                    bSubgroupSizeSame = false;
                dblSr = 0;


                if (bSubgroupSizeSame)
                {
                    double dblRangeTotal = 0;

                    for (int i = 0; i < iRowCnt; i++)
                    {
                        List<double> arrTemp = new List<double>();

                        for (int j = 0; j < iColCnt; j++)
                        {
                            if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                                arrTemp.Add(Convert.ToDouble(dt.Rows[i][arrColumns[j]]));
                        }
                        double[] arrDouble = arrTemp.ToArray();
                        double dblRange = StatsFunctions.MaxValue(arrDouble) - StatsFunctions.MinValue(arrDouble);
                        dblRangeTotal += dblRange;
                        arrTemp = null;
                    }
                    double dblRbar = dblRangeTotal / (double)iRowCnt;
                    double dbld2 = ConstantsTable.Getd2(iModeSubgrpSize);
                    dblSr = dblRbar / dbld2;
                }
                else
                {
                    dblCumulative = 0;
                    dblFcumulative = 0;
                    for (int i = 0; i < iRowCnt; i++)
                    {
                        List<double> arrTemp = new List<double>();

                        for (int j = 0; j < iColCnt; j++)
                        {
                            if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            {
                                arrTemp.Add(Convert.ToDouble(dt.Rows[i][arrColumns[j]]));
                            }
                        }
                        double[] arrDouble = arrTemp.ToArray();
                        int iDataCnt = arrDouble.Length;

                        if (iDataCnt < 2)
                            continue;

                        double dbld2 = ConstantsTable.Getd2(iDataCnt);
                        double dblF = Math.Pow(dbld2, 2) / Math.Pow(ConstantsTable.Getd3(iDataCnt), 2);
                        double dblRange = StatsFunctions.MaxValue(arrDouble) - StatsFunctions.MinValue(arrDouble);
                        dblCumulative += (dblF * dblRange / dbld2);
                        dblFcumulative += dblF;
                        arrTemp = null;
                    }
                    dblSr = dblCumulative / dblFcumulative;
                }
                return dblSr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Sbar Within Std

        private double GetSigmaWithin_Sbar(DataTable dt, int[] arrColumns, bool Unbiased)
        {

            double dblSs;
            double dblCumulative;
            double dblHcumulative;

            int iRowCnt;
            int iColCnt;
            try
            {
                if (dt == null)
                    throw new ArgumentException(ErrorNullParameter("dt"));
                if (arrColumns == null)
                    throw new ArgumentException(ErrorNullParameter("arrColumns"));
                iColCnt = arrColumns.Length;
                iRowCnt = dt.Rows.Count;
                if (iColCnt < 2)
                    throw new ArgumentException(ErrorWrongParameter("arrColumns"));
                if (iRowCnt == 0)
                    throw new ArgumentException(ErrorWrongParameter("dt"));

                dblSs = 0;

                dblCumulative = 0;
                dblHcumulative = 0;
                if (Unbiased)
                {
                    for (int i = 0; i < iRowCnt; i++)
                    {
                        List<double> arrTemp = new List<double>();

                        for (int j = 0; j < iColCnt; j++)
                        {
                            if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            {
                                arrTemp.Add(Convert.ToDouble(dt.Rows[i][arrColumns[j]]));
                            }
                        }
                        double[] arrDouble = arrTemp.ToArray();
                        int iDataCnt = arrDouble.Length;
                        if (iDataCnt < 2)
                            continue;
                        double dblC4 = GetC4((double)iDataCnt);
                        //double dblC4 = ControlChartCoefficient.Getc4(iDataCnt);
                        double dblH = (dblC4 * dblC4) / (1 - (dblC4 * dblC4));
                        double dblSubgroupStd = StatsFunctions.StandardDeviation(arrDouble, BiasType.Unbiased);

                        dblCumulative += (dblH * dblSubgroupStd / dblC4);
                        dblHcumulative += dblH;
                        arrTemp = null;
                    }
                    dblSs = dblCumulative / dblHcumulative;
                }
                else
                {
                    int iSubgrpCnt = 0;
                    for (int i = 0; i < iRowCnt; i++)
                    {
                        List<double> arrTemp = new List<double>();

                        for (int j = 0; j < iColCnt; j++)
                        {
                            if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            {
                                arrTemp.Add(Convert.ToDouble(dt.Rows[i][arrColumns[j]]));
                            }
                        }
                        double[] arrDouble = arrTemp.ToArray();
                        int iDataCnt = arrDouble.Length;
                        if (iDataCnt < 2)
                            continue;
                        else
                            iSubgrpCnt++;
                        dblCumulative += StatsFunctions.StandardDeviation(arrDouble, BiasType.Unbiased);
                        arrTemp = null;
                    }
                    dblSs = dblCumulative / (double)iSubgrpCnt;
                }
                return dblSs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Sigma of Between
        private double GetSigmaBetween(double dblSigmaXbar, double dblSigmaWithin, int iBatchSize)
        {
            double dblTemp;
            try
            {
                dblTemp = Math.Max(0, ((dblSigmaXbar * dblSigmaXbar) - ((dblSigmaWithin * dblSigmaWithin) / (double)iBatchSize)));
                return Math.Sqrt(dblTemp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Sigma of xbar

        #region Average of moving range

        private double GetSigmaXbarAvg(DataTable dt, int[] arrColumns, int iLengthMovingRange, bool Unbiased, out int iSampleSize)
        {
            double dblReturn;
            double dblCumulative;
            int iRowCnt;
            int iColCnt;
            int iModeSubgrpSize; //가장 많은 부분군 크기
            int iModeCount;  // 가장 많은 부분군 크기의 개수
            List<int> arrSubgrpSize;
            double[] arrAvg;
            try
            {
                #region 인자 확인
                if (dt == null)
                    throw new ArgumentException(ErrorNullParameter("dt"));
                if (arrColumns == null)
                    throw new ArgumentException(ErrorNullParameter("arrColumns"));
                iColCnt = arrColumns.Length;
                iRowCnt = dt.Rows.Count;
                if (iLengthMovingRange > iRowCnt)
                    throw new ArgumentException("Length of MovingRange is greater than Row Count"); // Exception처리해야하나...? 


                if (iColCnt < 1)
                    throw new ArgumentException(ErrorWrongParameter("arrColumns"));
                if (iRowCnt == 0)
                    throw new ArgumentException(ErrorWrongParameter("dt"));

                dt.AcceptChanges();
                #endregion


                #region Mode SubGroup Size Check

                arrSubgrpSize = new List<int>();
                for (int i = 0; i < iRowCnt; i++)
                {
                    int iValueCnt = 0;
                    for (int j = 0; j < iColCnt; j++)
                    {
                        if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            iValueCnt++;
                    }
                    arrSubgrpSize.Add(iValueCnt);
                }
                List<int> arrDistinctCount = new List<int>();
                foreach (int oCount in arrSubgrpSize)
                {
                    if (!arrDistinctCount.Contains(oCount))
                        arrDistinctCount.Add(oCount);
                }
                iModeSubgrpSize = StatsFunctions.Mode(arrSubgrpSize.ToArray());

                if (iModeSubgrpSize < 1)
                    throw new Exception(m_ErrorArgumentContantForSubgroupSize);
                m_iTempSubgrpSize = iModeSubgrpSize;
                iSampleSize = iModeSubgrpSize;
                iModeCount = StatsFunctions.CountIf(arrSubgrpSize.ToArray(), new StatsFunctions.LogicalDoubleFunction(GetMode));


                foreach (int oCount in arrDistinctCount)
                {
                    if (oCount != iModeSubgrpSize)
                    {
                        m_iTempSubgrpSize = oCount;
                        if (iModeCount == StatsFunctions.CountIf(arrSubgrpSize.ToArray(), new StatsFunctions.LogicalDoubleFunction(GetMode)))
                            throw new Exception(m_ErrorSubgroupsize);
                    }
                }
                #endregion


                #region 각 Row의 Mean가져오기(단 Sub Group Size가 Mode Sub Group Size인 것만)
                arrAvg = new double[iRowCnt];
                for (int i = 0; i < iRowCnt; i++)
                {
                    if (arrSubgrpSize[i] == iModeSubgrpSize)  // Sub Group Size가 Mode Sub Group Size면
                    {
                        double[] arrTemp = new double[iModeSubgrpSize];
                        int ii = 0;
                        for (int j = 0; j < iColCnt; j++)
                        {
                            if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            {
                                arrTemp[ii] = Convert.ToDouble(dt.Rows[i][arrColumns[j]]);
                                ii++;
                            }
                        }
                        arrAvg[i] = StatsFunctions.Mean(arrTemp);
                        arrTemp = null;
                    }
                    else  //서브 그룹 사이즈가 다르면!!
                    {
                        arrAvg[i] = double.NaN;
                    }
                }
                #endregion

                dblCumulative = 0;
                int iRealValueCnt = 0;
                for (int j = iLengthMovingRange - 1; j < iRowCnt; j++)
                {
                    List<double> arrTemp = new List<double>();
                    bool bNanValue = true;

                    for (int k = j; k > j - iLengthMovingRange; k--)
                    {
                        if (!double.IsNaN(arrAvg[k]))
                        {
                            arrTemp.Add(arrAvg[k]);
                            bNanValue = false;
                        }
                        else
                        {
                            bNanValue = true;
                            break;
                        }
                    }
                    if (!bNanValue)  // 같이 분석되는 것들이 모두 같은 Sub Group Size를 갖는 집단이라면.
                    {
                        double[] arrDouble = arrTemp.ToArray();
                        double dblMax = StatsFunctions.MaxValue(arrDouble);
                        double dblMin = StatsFunctions.MinValue(arrDouble);
                        double dblRange = dblMax - dblMin;
                        dblCumulative += dblRange;
                        iRealValueCnt++;
                    }
                    arrTemp = null;
                }

                //dblReturn = dblCumulative / (iValueCnt - iObservationsCnt + 1); 
                dblReturn = dblCumulative / (double)(iRealValueCnt);
                if (Unbiased)
                    return dblReturn / ConstantsTable.Getd2(iLengthMovingRange);
                else
                    return dblReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Median of moving range
        private double GetSigmaXbarMedian(DataTable dt, int[] arrColumns, int iLengthMovingRange, bool Unbiased, out int iSampleSize)
        {
            double dblReturn;
            int iRowCnt;
            int iColCnt;
            int iModeSubgrpSize; //가장 많은 부분군 크기
            int iModeCount;  // 가장 많은 부분군 크기의 개수
            List<int> arrSubgrpSize;
            double[] arrAvg;
            try
            {
                #region 인자 확인
                if (dt == null)
                    throw new ArgumentException(ErrorNullParameter("dt"));
                if (arrColumns == null)
                    throw new ArgumentException(ErrorNullParameter("arrColumns"));
                iColCnt = arrColumns.Length;
                iRowCnt = dt.Rows.Count;
                if (iLengthMovingRange > iRowCnt)
                    throw new ArgumentException("Length of MovingRange is greater than Row Count"); // Exception처리해야하나..
                if (iColCnt < 1)
                    throw new ArgumentException(ErrorWrongParameter("arrColumns"));
                if (iRowCnt == 0)
                    throw new ArgumentException(ErrorWrongParameter("dt"));
                dt.AcceptChanges();
                #endregion


                #region Mode SubGroup Size Check

                arrSubgrpSize = new List<int>();
                for (int i = 0; i < iRowCnt; i++)
                {
                    int iValueCnt = 0;
                    for (int j = 0; j < iColCnt; j++)
                    {
                        if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            iValueCnt++;
                    }
                    arrSubgrpSize.Add(iValueCnt);
                }
                List<int> arrDistinctCount = new List<int>();
                foreach (int oCount in arrSubgrpSize)
                {
                    if (!arrDistinctCount.Contains(oCount))
                        arrDistinctCount.Add(oCount);
                }
                iModeSubgrpSize = StatsFunctions.Mode(arrSubgrpSize.ToArray());
                if (iModeSubgrpSize < 1)
                {
                    if (On_Error != null)
                        On_Error(new Exception(m_ErrorArgumentContantForSubgroupSize, new Exception("MIRACOM")));
                    else
                        throw new Exception(m_ErrorArgumentContantForSubgroupSize, new Exception("MIRACOM"));
                }
                m_iTempSubgrpSize = iModeSubgrpSize;
                iSampleSize = iModeSubgrpSize;
                iModeCount = StatsFunctions.CountIf(arrSubgrpSize.ToArray(), new StatsFunctions.LogicalDoubleFunction(GetMode));


                foreach (int oCount in arrDistinctCount)
                {
                    if (oCount != iModeSubgrpSize)
                    {
                        m_iTempSubgrpSize = oCount;
                        if (iModeCount == StatsFunctions.CountIf(arrSubgrpSize.ToArray(), new StatsFunctions.LogicalDoubleFunction(GetMode)))
                            throw new Exception(m_ErrorSubgroupsize, new Exception("MIRACOM"));
                    }
                }
                #endregion


                #region 각 Row의 Mean가져오기(단 Sub Group Size가 Mode Sub Group Size인 것만)
                arrAvg = new double[iRowCnt];
                for (int i = 0; i < iRowCnt; i++)
                {
                    if (arrSubgrpSize[i] == iModeSubgrpSize)  // Sub Group Size가 Mode Sub Group Size면
                    {
                        double[] arrTemp = new double[iModeSubgrpSize];
                        int ii = 0;
                        for (int j = 0; j < iColCnt; j++)
                        {
                            if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            {
                                arrTemp[ii] = Convert.ToDouble(dt.Rows[i][arrColumns[j]]);
                                ii++;
                            }
                        }
                        arrAvg[i] = StatsFunctions.Mean(arrTemp);
                        arrTemp = null;
                    }
                    else  //서브 그룹 사이즈가 다르면!!
                    {
                        arrAvg[i] = double.NaN;
                    }
                }
                #endregion



                List<double> arrRange = new List<double>();
                for (int j = iLengthMovingRange - 1; j < iRowCnt; j++)
                {
                    List<double> arrTemp = new List<double>();
                    bool bNanValue = true;
                    for (int k = j; k > j - iLengthMovingRange; k--)
                    {
                        if (!double.IsNaN(arrAvg[k]))
                        {
                            arrTemp.Add(arrAvg[k]);
                            bNanValue = false;
                        }
                        else
                        {
                            bNanValue = true;
                            break;
                        }
                    }

                    if (!bNanValue)
                    {
                        double[] arrDouble = arrTemp.ToArray();
                        double dblMax = StatsFunctions.MaxValue(arrDouble);
                        double dblMin = StatsFunctions.MinValue(arrDouble);
                        double dblRange = dblMax - dblMin;
                        arrRange.Add(dblRange);
                    }
                    arrTemp = null;

                }
                dblReturn = StatsFunctions.Median(arrRange.ToArray());

                if (Unbiased)
                    return dblReturn / ConstantsTable.Getd4(iLengthMovingRange);
                else
                    return dblReturn;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Square root of MSSD(mean squared successive differences) Average of subgroup ranges
        private double GetSigmaXbarSMSSD(DataTable dt, int[] arrColumns, bool Unbiased, out int iSampleSize)
        {
            double dblReturn;
            double dblCumulative;
            int iRowCnt;
            int iColCnt;
            int iModeSubgrpSize; //가장 많은 부분군 크기
            int iModeCount;  // 가장 많은 부분군 크기의 개수
            List<int> arrSubgrpSize;
            double[] arrAvg;
            try
            {
                #region 인자 확인
                if (dt == null)
                    throw new ArgumentException(ErrorNullParameter("dt"));

                if (arrColumns == null)
                    throw new ArgumentException(ErrorNullParameter("arrColumns"));
                iColCnt = arrColumns.Length;
                iRowCnt = dt.Rows.Count;
                if (iColCnt < 1)
                    throw new ArgumentException(ErrorWrongParameter("arrColumns"));

                if (iRowCnt == 0)
                    throw new ArgumentException(ErrorWrongParameter("dt"));

                dt.AcceptChanges();
                #endregion


                #region Mode SubGroup Size Check

                arrSubgrpSize = new List<int>();
                for (int i = 0; i < iRowCnt; i++)
                {
                    int iValueCnt = 0;
                    for (int j = 0; j < iColCnt; j++)
                    {
                        if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            iValueCnt++;
                    }
                    arrSubgrpSize.Add(iValueCnt);
                }
                List<int> arrDistinctCount = new List<int>();
                foreach (int oCount in arrSubgrpSize)
                {
                    if (!arrDistinctCount.Contains(oCount))
                        arrDistinctCount.Add(oCount);
                }
                iModeSubgrpSize = StatsFunctions.Mode(arrSubgrpSize.ToArray());
                if (iModeSubgrpSize < 1)
                {
                    if (On_Error != null)
                        On_Error(new Exception(m_ErrorArgumentContantForSubgroupSize));
                    else
                        throw new Exception(m_ErrorArgumentContantForSubgroupSize);
                }
                m_iTempSubgrpSize = iModeSubgrpSize;
                iSampleSize = iModeSubgrpSize;
                iModeCount = StatsFunctions.CountIf(arrSubgrpSize.ToArray(), new StatsFunctions.LogicalDoubleFunction(GetMode));


                foreach (int oCount in arrDistinctCount)
                {
                    if (oCount != iModeSubgrpSize)
                    {
                        m_iTempSubgrpSize = oCount;
                        if (iModeCount == StatsFunctions.CountIf(arrSubgrpSize.ToArray(), new StatsFunctions.LogicalDoubleFunction(GetMode)))
                            throw new Exception(m_ErrorSubgroupsize, new Exception("MIRACOM"));
                    }
                }
                #endregion


                #region 각 Row의 Mean가져오기(단 Sub Group Size가 Mode Sub Group Size인 것만)
                arrAvg = new double[iRowCnt];
                for (int i = 0; i < iRowCnt; i++)
                {
                    if (arrSubgrpSize[i] == iModeSubgrpSize)  // Sub Group Size가 Mode Sub Group Size면
                    {
                        double[] arrTemp = new double[iModeSubgrpSize];
                        int ii = 0;
                        for (int j = 0; j < iColCnt; j++)
                        {
                            if (dt.Rows[i][arrColumns[j]] != DBNull.Value && dt.Rows[i][arrColumns[j]].ToString() != string.Empty && !double.IsNaN(Convert.ToDouble(dt.Rows[i][arrColumns[j]])))
                            {
                                arrTemp[ii] = Convert.ToDouble(dt.Rows[i][arrColumns[j]]);
                                ii++;
                            }
                        }
                        arrAvg[i] = StatsFunctions.Mean(arrTemp);
                        arrTemp = null;
                    }
                    else  //서브 그룹 사이즈가 다르면!!
                    {
                        arrAvg[i] = double.NaN;
                    }
                }
                #endregion


                dblCumulative = 0;
                int iRealValueCnt = 0;
                for (int i = 1; i < iRowCnt; i++)
                {
                    if (!double.IsNaN(arrAvg[i]) && !double.IsNaN(arrAvg[i - 1]))
                    {
                        dblCumulative += Math.Pow(arrAvg[i] - arrAvg[i - 1], 2);
                        iRealValueCnt++;
                    }
                }

                dblReturn = Math.Sqrt(dblCumulative / 2 / (double)(iRealValueCnt));


                if (Unbiased)
                    return dblReturn / ConstantsTable.Getc4_(iRealValueCnt + 1);  // 수치 맞춰줘야함..
                else
                    return dblReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Sigma of B/W
        private double GetSigmaBW(double dblSigmaBetween, double dblSigmaWithin)
        {
            try
            {
                return Math.Sqrt((dblSigmaBetween * dblSigmaBetween) + (dblSigmaWithin * dblSigmaWithin));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Sigma of overall
        private double GetSigmaOverall(DataTable dt, int[] arrColumns, bool Unbiased, out int idf)
        {
            List<double> arrTemp = null;
            int iColCnt;
            int iRowCnt;
            DataFrame dfSource;
            try
            {
                if (dt == null)
                    throw new ArgumentException(ErrorNullParameter("dt"));
                if (arrColumns == null)
                    throw new ArgumentException(ErrorNullParameter("arrColumns"));
                iColCnt = arrColumns.Length;
                iRowCnt = dt.Rows.Count;
                if (iColCnt < 1)
                    throw new ArgumentException(ErrorWrongParameter("arrColumns"));
                if (iRowCnt == 0)
                    throw new ArgumentException(ErrorWrongParameter("dt"));

                dfSource = new DataFrame(dt);
                arrTemp = new List<double>();
                for (int i = 0; i < iColCnt; i++)
                {
                    arrTemp.AddRange(((IDFColumn)dfSource[arrColumns[i]]).Clean().ToDoubleArray());
                }

                idf = arrTemp.Count;

                if (Unbiased)
                {
                    return StatsFunctions.StandardDeviation(arrTemp.ToArray(), BiasType.Unbiased) / GetC4(arrTemp.Count);
                }
                else
                    return StatsFunctions.StandardDeviation(arrTemp.ToArray(), BiasType.Unbiased);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetSigmaOverall(double[] arrCleanData, bool Unbiased)
        {
            try
            {
                if (arrCleanData == null)
                    throw new ArgumentException(ErrorNullParameter("arrCleanData"));
                if (Unbiased)
                {
                    return StatsFunctions.StandardDeviation(arrCleanData, BiasType.Unbiased) / GetC4(arrCleanData.Length);
                }
                else
                    return StatsFunctions.StandardDeviation(arrCleanData, BiasType.Unbiased);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region [ CAPABILITY ]

        private double GetCp(double USL, double LSL, double Ksigma, double Sigma)
        {
            try
            {
                return (USL - LSL) / (Ksigma * Sigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCPL(double Mean, double LSL, double Ksigma, double Sigma)
        {
            try
            {
                return (Mean - LSL) / (Ksigma / 2 * Sigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCPU(double Mean, double USL, double Ksigma, double Sigma)
        {
            try
            {
                return (USL - Mean) / (Ksigma / 2 * Sigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCpk(double CPL, double CPU)
        {
            try
            {
                if (double.IsNaN(CPU) && double.IsNaN(CPL))
                    return double.NaN;
                if (double.IsNaN(CPU))
                    return CPL;
                if (double.IsNaN(CPL))
                    return CPU;
                else
                    return Math.Min(CPU, CPL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCCpk(double LSL, double USL, double Target, double Mean, double Ksigma, double Sigma)
        {
            double Mu;
            try
            {
                if (double.IsNaN(USL) && double.IsNaN(LSL))
                    throw new ArgumentException("Argument(USL, LSL) is double.NaN");
                if (double.IsNaN(Ksigma))
                    throw new ArgumentException("Argument(Ksigma) is double.NaN");
                if (double.IsNaN(Sigma))
                    throw new ArgumentException("Argument(Sigma) is double.NaN");

                if (!double.IsNaN(Target))
                    Mu = Target;
                else
                {
                    if (!double.IsNaN(USL) && !double.IsNaN(LSL))
                        Mu = (USL + LSL) / 2;
                    else
                    {
                        if (!double.IsNaN(Mean))
                            Mu = Mean;
                        else
                            throw new ArgumentException("Argument(Mean) is double.NaN");
                    }
                }

                if (!double.IsNaN(USL) && !double.IsNaN(LSL))
                    return Math.Min(USL - Mu, Mu - LSL) / (Ksigma / 2 * Sigma);
                else if (double.IsNaN(USL) && !double.IsNaN(LSL))
                    return (Mu - LSL) / (Ksigma / 2 * Sigma);
                else //if (!double.IsNaN(USL) && double.IsNaN(LSL))
                    return (USL - Mu) / (Ksigma / 2 * Sigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="LSL"></param>
        /// <param name="USL"></param>
        /// <param name="Target"></param>
        /// <param name="Ksigma"></param>
        /// <param name="arrCleanData">double.Nan이 없는 배열</param>
        /// <returns></returns>
        private double GetCpm(double LSL, double USL, double Target, double Ksigma, double[] arrCleanData)
        {
            double dblm;
            int iDataCnt;
            double dblSigma_;
            double dblTemp;
            double dblReturn;
            try
            {
                if (double.IsNaN(USL) && double.IsNaN(LSL))
                    throw new ArgumentException("Argument(USL, LSL) is double.NaN");
                if (double.IsNaN(Ksigma))
                    throw new ArgumentException("Argument(Ksigma) is double.NaN");
                if (arrCleanData == null)
                    throw new ArgumentException("Argument(Data) is null");
                if (double.IsNaN(Target))
                    throw new ArgumentException("Argument(Target) is double.NaN");

                iDataCnt = arrCleanData.Length;
                if (iDataCnt == 0)
                    throw new ArgumentException("Length of Argument(Data) is 0");

                dblTemp = 0;
                for (int i = 0; i < iDataCnt; i++)
                    dblTemp += Math.Pow((arrCleanData[i] - Target), 2);
                dblSigma_ = Math.Sqrt(dblTemp / (iDataCnt - 1));

                if (!double.IsNaN(USL) && !double.IsNaN(LSL))
                {
                    dblm = (USL + LSL) / 2;
                    if (dblm == Target)
                    {
                        dblReturn = (USL - LSL) / (Ksigma * dblSigma_);
                    }
                    else
                    {
                        dblReturn = Math.Min((Target - LSL), (USL - Target)) / (Ksigma / 2 * dblSigma_);
                    }

                }
                else if (double.IsNaN(USL) && !double.IsNaN(LSL))
                {
                    dblReturn = (USL - Target) / (Ksigma / 2 * dblSigma_);
                }
                else //if (!double.IsNaN(USL) && double.IsNaN(LSL))
                {
                    dblReturn = (Target - LSL) / (Ksigma / 2 * dblSigma_);
                }
                return dblReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region [ Z.BENCH ]

        private double GetZlsl(double Mean, double LSL, double Sigma)
        {
            try
            {
                return (Mean - LSL) / Sigma;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetZusl(double Mean, double USL, double Sigma)
        {
            try
            {
                return (USL - Mean) / Sigma;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetZbench(double Mean, double Zlsl, double Zusl)
        {
            double dblP1 = double.NaN;
            double dblP2 = double.NaN;
            try
            {
                if (double.IsNaN(Mean))
                    throw new ArgumentException("Argument(Mean) is double.Nan");
                if (double.IsNaN(Zlsl) && double.IsNaN(Zusl))
                    throw new ArgumentException("Argument(Zlsl, Zusl) is double.Nan");

                NormalDistribution oNormal = new NormalDistribution(0, 1);

                if (!double.IsNaN(Zlsl))
                    dblP1 = 1 - oNormal.CDF(Zlsl);
                if (!double.IsNaN(Zusl))
                    dblP2 = 1 - oNormal.CDF(Zusl);
                if (!double.IsNaN(dblP1) && !double.IsNaN(dblP2))
                    return oNormal.InverseCDF(1 - dblP1 - dblP2);
                else
                {
                    if (!double.IsNaN(dblP1))
                        return oNormal.InverseCDF(1 - dblP1);
                    else if (!double.IsNaN(dblP2))
                        return oNormal.InverseCDF(1 - dblP2);
                    else
                    {
                        throw new Exception("Please Call Administrator");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ PERFORMANCE ]

        private double GetPPMlessLSL(double Mean, double LSL, double Sigma)
        {
            try
            {
                NormalDistribution oNormal = new NormalDistribution(0, 1);
                return 1 - (oNormal.CDF((Mean - LSL) / Sigma));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetPPMmoreUSL(double Mean, double USL, double Sigma)
        {
            try
            {
                NormalDistribution oNormal = new NormalDistribution(0, 1);
                return 1 - (oNormal.CDF((USL - Mean) / Sigma));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetObslssLSL(double[] arrCleanData, double LSL)
        {
            int iCnt;
            int iAccumulator;
            try
            {
                iCnt = arrCleanData.Length;
                iAccumulator = 0;
                for (int i = 0; i < iCnt; i++)
                {
                    if (arrCleanData[i] < LSL)
                        iAccumulator++;
                }
                return (double)iAccumulator / (double)iCnt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetObslssUSL(double[] arrCleanData, double USL)
        {
            int iCnt;
            int iAccumulator;
            try
            {
                iCnt = arrCleanData.Length;
                iAccumulator = 0;
                for (int i = 0; i < iCnt; i++)
                {
                    if (arrCleanData[i] > USL)
                        iAccumulator++;
                }
                return (double)iAccumulator / (double)iCnt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ CONFIDENCE INTERVAL ]  //신뢰구간 일단 보류(시간관계상-_-;)

        // 신뢰구간 부분 일단 보류(시간음따)       

        private double GetFnSbar(double AvgSampleSize)
        {
            int iSampleSize;
            try
            {
                iSampleSize = Convert.ToInt32(Math.Round(AvgSampleSize, 0));
                if (iSampleSize < 2)
                    throw new ArgumentException("AvgSampleSize < 2", new Exception("MIRACOM"));
                else if (iSampleSize == 2)
                    return 0.88;
                else if (iSampleSize == 3)
                    return 0.92;
                else if (iSampleSize == 4)
                    return 0.94;
                else if (iSampleSize == 5)
                    return 0.95;
                else if (iSampleSize == 6 || iSampleSize == 7)
                    return 0.96;
                else if (iSampleSize == 8 || iSampleSize == 9)
                    return 0.97;
                else if (iSampleSize > 9 && iSampleSize < 18)
                    return 0.98;
                else if (iSampleSize > 18 && iSampleSize < 65)
                    return 0.99;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Cp Interval

        private double GetCpBound(double Cp, double Probability, double v)
        {
            double dblReturn;
            try
            {
                if (Probability > 1 || Probability < 0)
                    throw new ArgumentException("Argument(Alpha) must be 0~1.");

                ChiSquareDistribution oChiSquare = new ChiSquareDistribution(v);
                dblReturn = Cp * (Math.Sqrt(oChiSquare.InverseCDF(Probability) / v));
                return dblReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private double GetCpLowerBound(double Cp, double Alpha, double v)
        {
            try
            {
                return GetCpBound(Cp, (Alpha / 2), v);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private double GetCpUpperBound(double Cp, double Alpha, double v)
        {
            try
            {
                return GetCpBound(Cp, 1 - (Alpha / 2), v);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Cpk Interval

        private double GetCpkBound(double Cpk, double Probability, double v, double Ntotal, double Ksigma)
        {
            double dblReturn;
            try
            {
                if (Probability > 1 || Probability < 0)
                    throw new ArgumentException("Argument(Alpha) must be 0~1.");
                NormalDistribution oNormal = new NormalDistribution(0, 1);
                dblReturn = Cpk + (oNormal.InverseCDF(Probability) * (Math.Sqrt((1 / (Ntotal * ((Ksigma / 2) * (Ksigma / 2)))) + ((Cpk * Cpk) / (2 * v)))));
                return dblReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private double GetCpkLowerBound(double Cpk, double Alpha, double v, double Ntotal, double Ksigma)
        {
            try
            {
                return GetCpkBound(Cpk, Alpha / 2, v, Ntotal, Ksigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetCpkUpperBound(double Cpk, double Alpha, double v, double Ntotal, double Ksigma)
        {
            try
            {
                return GetCpkBound(Cpk, 1 - (Alpha / 2), v, Ntotal, Ksigma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Z Interval
        private double GetZBound(double Probability)
        {
            try
            {
                if (Probability > 1 || Probability < 0)
                    throw new ArgumentException("Argument(Alpha) must be 0~1.");
                NormalDistribution oNormal = new NormalDistribution(0, 1);
                return Math.Abs(oNormal.InverseCDF(Probability));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetZLowerBound(double Zlsl, double Zusl, double Alpha, int iNtotal)
        {
            double dblP = double.NaN;
            double dblPl = double.NaN;
            double v1;
            double v2;
            double dblF;
            double dblTemp;
            try
            {
                NormalDistribution oNormal = new NormalDistribution(0, 1);
                if (double.IsNaN(Zlsl) && double.IsNaN(Zusl))
                    throw new ArgumentException("Argument(Zlsl, Zusl) is null.");
                else if (!double.IsNaN(Zlsl) && !double.IsNaN(Zusl))
                {
                    dblP = (1 - oNormal.CDF(Zlsl)) + (1 - oNormal.CDF(Zusl));
                }
                else if (!double.IsNaN(Zlsl) && double.IsNaN(Zusl))
                {
                    dblP = 1 - oNormal.CDF(Zlsl);
                }
                else //if (double.IsNaN(Zlsl) && !double.IsNaN(Zusl))
                {
                    dblP = 1 - oNormal.CDF(Zusl);
                }
                v1 = 2 * (double)iNtotal * dblP;
                v2 = 2 * ((double)iNtotal - dblP * (double)iNtotal + 1);

                FDistribution oF = new FDistribution(Convert.ToInt32(Math.Round(v1, 0)), Convert.ToInt32(Math.Round(v2, 0)));
                dblF = oF.InverseCDF(1 - (Alpha / 2));
                dblTemp = v1 * dblF;
                dblPl = dblTemp / (v2 + dblTemp);
                return GetZBound(dblPl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetZUpperBound(double Zlsl, double Zusl, double Alpha, int iNtotal)
        {
            double dblP = double.NaN;
            double dblPu = double.NaN;
            double v1;
            double v2;
            double dblF;
            double dblTemp;
            try
            {
                NormalDistribution oNormal = new NormalDistribution(0, 1);
                if (double.IsNaN(Zlsl) && double.IsNaN(Zusl))
                    throw new ArgumentException("Argument(Zlsl, Zusl) is null.");
                else if (!double.IsNaN(Zlsl) && !double.IsNaN(Zusl))
                {
                    dblP = (1 - oNormal.CDF(Zlsl)) + (1 - oNormal.CDF(Zusl));
                }
                else if (!double.IsNaN(Zlsl) && double.IsNaN(Zusl))
                {
                    dblP = 1 - oNormal.CDF(Zlsl);
                }
                else //if (double.IsNaN(Zlsl) && !double.IsNaN(Zusl))
                {
                    dblP = 1 - oNormal.CDF(Zusl);
                }

                v1 = 2 * ((double)iNtotal * dblP + 1);
                v2 = 2 * ((double)iNtotal - dblP * (double)iNtotal);

                FDistribution oF = new FDistribution(Convert.ToInt32(Math.Round(v1, 0)), Convert.ToInt32(Math.Round(v2, 0)));
                dblF = oF.InverseCDF(Alpha / 2);
                dblTemp = v1 * dblF;
                dblPu = dblTemp / (v2 + dblTemp);

                return GetZBound(dblPu);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region [ ETC ]

        private string ErrorNotEqualParameterLength(string strParameter1, string strParameter2)
        {
            try
            {
                return string.Format("Length of {0} doesn't equal length of {1}.", strParameter1, strParameter2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ErrorNullParameter(string strParameter)
        {
            try
            {
                if (strParameter == string.Empty)
                    strParameter = "parameter";

                return string.Format("Argument({0}) is null.", strParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ErrorWrongParameter(string strParameter)
        {
            try
            {
                if (strParameter == string.Empty)
                    strParameter = "parameter";

                return string.Format("Argument({0}) is wrong.", strParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool GetMode(double Value)
        {
            try
            {
                return (Value == (double)m_iTempSubgrpSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetC4(double N)
        {
            double dblGamma1;
            double dblGamma2;
            try
            {
                dblGamma1 = GammaFunction(N / 2);
                dblGamma2 = GammaFunction((N - 1) / 2);
                if (double.IsInfinity(dblGamma1) || double.IsInfinity(dblGamma2))
                    return 1;
                return Math.Sqrt(2 / (N - 1)) * dblGamma1 / dblGamma2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GammaFunction(double N)
        {
            try
            {
                return Math.Exp(StatsFunctions.GammaLn(N));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double[] GetAllData(DataTable Source, int[] DataColumns, bool bClean)
        {
            double[] arrReturn;
            List<double> arrTemp = null;
            int iCnt;
            int iRow;
            DataFrame dfSource;
            try
            {
                if (DataColumns == null)
                    throw new ArgumentException("Argument(parameter) is null.");
                if (Source == null)
                    throw new ArgumentException("Argument(Source) is null.");
                dfSource = new DataFrame(Source);
                iRow = dfSource.Rows;
                iCnt = DataColumns.Length;
                arrTemp = new List<double>(iRow * iCnt);

                for (int i = 0; i < iCnt; i++)
                {
                    if (bClean)
                        arrTemp.AddRange(((IDFColumn)dfSource[DataColumns[i]]).Clean().ToDoubleArray());
                    else
                        arrTemp.AddRange(((IDFColumn)dfSource[DataColumns[i]]).ToDoubleArray());
                }
                arrReturn = arrTemp.ToArray();
                return arrReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion
    }

    #region Constants Table

    public class ConstantsTable
    {

        private static double[] Constants_c4  // 100개
            = new double[] { double.NaN, 0.797885, 0.886227, 0.921318, 0.939986, 0.951533, 0.959369, 0.96503, 0.969311, 0.972659, 0.97535, 0.977559, 0.979406, 0.980971, 0.982316, 0.983484, 0.984506, 0.98541, 0.986214, 0.986934, 0.987583, 0.98817, 0.988705, 0.989193, 0.98964, 0.990052, 0.990433, 0.990786, 0.991113, 0.991418, 0.991703, 0.991969, 0.992219, 0.992454, 0.992675, 0.992884, 0.99308, 0.993267, 0.993443, 0.993611, 0.99377, 0.993922, 0.994066, 0.994203, 0.994335, 0.99446, 0.99458, 0.994695, 0.994806, 0.994911, 0.995013, 0.99511, 0.995204, 0.995294, 0.995381, 0.995465, 0.995546, 0.995624, 0.995699, 0.995772, 0.995842, 0.99591, 0.995976, 0.99604, 0.996102, 0.996161, 0.996219, 0.996276, 0.99633, 0.996383, 0.996435, 0.996485, 0.996534, 0.996581, 0.996627, 0.996672, 0.996716, 0.996759, 0.9968, 0.996841, 0.99688, 0.996918, 0.996956, 0.996993, 0.997028, 0.997063, 0.997097, 0.997131, 0.997163, 0.997195, 0.997226, 0.997257, 0.997286, 0.997315, 0.997344, 0.997372, 0.997399, 0.997426, 0.997452, 0.997478 };

        private static double[] Constants_c5  // 100개
            = new double[] { double.NaN, 0.60281, 0.463251, 0.38881, 0.341213, 0.307547, 0.282154, 0.262139, 0.245838, 0.232238, 0.220663, 0.210662, 0.201901, 0.194154, 0.187231, 0.180995, 0.175351, 0.170197, 0.165475, 0.161125, 0.157098, 0.153362, 0.149875, 0.146619, 0.143571, 0.140702, 0.137994, 0.135437, 0.133023, 0.13073, 0.12855, 0.126481, 0.124505, 0.122618, 0.120815, 0.119086, 0.11744, 0.115848, 0.114328, 0.112859, 0.11145, 0.110087, 0.108779, 0.107519, 0.106292, 0.105116, 0.103974, 0.102868, 0.101789, 0.100758, 0.099745, 0.098773, 0.097821, 0.096901, 0.096003, 0.095129, 0.094277, 0.09345, 0.092647, 0.091859, 0.091097, 0.090351, 0.08962, 0.088906, 0.088209, 0.08754, 0.086878, 0.086221, 0.085595, 0.084976, 0.084364, 0.083771, 0.083186, 0.082621, 0.082065, 0.081516, 0.080977, 0.080446, 0.079936, 0.079423, 0.078932, 0.078451, 0.077966, 0.077492, 0.07704, 0.076586, 0.076142, 0.075695, 0.075273, 0.074847, 0.074433, 0.074017, 0.073625, 0.073231, 0.072835, 0.072451, 0.072078, 0.071703, 0.071341, 0.070976 };

        private static double[] Constants_d2  // 50개
            = new double[] { 1, 1.128, 1.693, 2.059, 2.326, 2.534, 2.704, 2.847, 2.97, 3.078, 3.173, 3.258, 3.336, 3.407, 3.472, 3.532, 3.588, 3.64, 3.689, 3.735, 3.778, 3.819, 3.858, 3.895, 3.931, 3.965, 3.997, 4.028, 4.058, 4.086, 4.113, 4.139, 4.164, 4.189, 4.213, 4.236, 4.258, 4.28, 4.301, 4.322, 4.342, 4.361, 4.38, 4.398, 4.415, 4.432, 4.449, 4.466, 4.482, 4.498 };

        private static double[] Constants_d3  //25개
            = new double[] { 0.82, 0.8525, 0.8884, 0.8794, 0.8641, 0.848, 0.8332, 0.8198, 0.8078, 0.7971, 0.7873, 0.7785, 0.7704, 0.763, 0.7562, 0.7499, 0.7441, 0.7386, 0.7335, 0.7287, 0.7242, 0.7199, 0.7159, 0.7121, 0.7084 };

        private static double[] Constants_d4  //25개
            = new double[] { 1, 0.954, 1.588, 1.978, 2.257, 2.472, 2.645, 2.791, 2.915, 3.024, 3.121, 3.207, 3.285, 3.356, 3.422, 3.482, 3.538, 3.591, 3.64, 3.686, 3.73, 3.771, 3.811, 3.847, 3.883 };

        private static double[] Constants_c4_  //500개
            = new double[] { double.NaN, 0.79785, 0.87153, 0.905763, 0.925222, 0.937892, 0.946837, 0.953503, 0.958669, 0.962793, 0.966163, 0.968968, 0.971341, 0.973375, 0.975137, 0.976679, 0.978039, 0.979249, 0.980331, 0.981305, 0.982187, 0.982988, 0.98372, 0.984391, 0.985009, 0.985579, 0.986107, 0.986597, 0.987054, 0.98748, 0.987878, 0.988252, 0.988603, 0.988934, 0.989246, 0.98954, 0.989819, 0.990083, 0.990333, 0.990571, 0.990797, 0.991013, 0.991218, 0.991415, 0.991602, 0.991782, 0.991953, 0.992118, 0.992276, 0.992427, 0.992573, 0.992713, 0.992848, 0.992978, 0.993103, 0.993224, 0.99334, 0.993452, 0.993561, 0.993666, 0.993767, 0.993866, 0.993961, 0.994053, 0.994142, 0.994229, 0.994313, 0.994395, 0.994474, 0.994551, 0.994626, 0.994699, 0.994769, 0.994838, 0.994905, 0.99497, 0.995034, 0.995096, 0.995156, 0.995215, 0.995272, 0.995328, 0.995383, 0.995436, 0.995489, 0.995539, 0.995589, 0.995638, 0.995685, 0.995732, 0.995777, 0.995822, 0.995865, 0.995908, 0.995949, 0.99599, 0.99603, 0.996069, 0.996108, 0.996145, 0.996182, 0.996218, 0.996253, 0.996288, 0.996322, 0.996356, 0.996389, 0.996421, 0.996452, 0.996483, 0.996514, 0.996544, 0.996573, 0.996602, 0.996631, 0.996658, 0.996686, 0.996713, 0.996739, 0.996765, 0.996791, 0.996816, 0.996841, 0.996865, 0.996889, 0.996913, 0.996936, 0.996959, 0.996982, 0.997004, 0.997026, 0.997047, 0.997069, 0.997089, 0.99711, 0.99713, 0.99715, 0.99717, 0.997189, 0.997209, 0.997227, 0.997246, 0.997264, 0.997282, 0.9973, 0.997318, 0.997335, 0.997352, 0.997369, 0.997386, 0.997402, 0.997419, 0.997435, 0.99745, 0.997466, 0.997481, 0.997497, 0.997512, 0.997526, 0.997541, 0.997555, 0.99757, 0.997584, 0.997598, 0.997612, 0.997625, 0.997639, 0.997652, 0.997665, 0.997678, 0.997691, 0.997703, 0.997716, 0.997728, 0.997741, 0.997753, 0.997765, 0.997776, 0.997788, 0.9978, 0.997811, 0.997822, 0.997834, 0.997845, 0.997856, 0.997866, 0.997877, 0.997888, 0.997898, 0.997909, 0.997919, 0.997929, 0.997939, 0.997949, 0.997959, 0.997969, 0.997978, 0.997988, 0.997997, 0.998007, 0.998016, 0.998025, 0.998034, 0.998043, 0.998052, 0.998061, 0.99807, 0.998078, 0.998087, 0.998095, 0.998104, 0.998112, 0.99812, 0.998128, 0.998137, 0.998145, 0.998152, 0.99816, 0.998168, 0.998176, 0.998184, 0.998191, 0.998199, 0.998206, 0.998214, 0.998221, 0.998228, 0.998235, 0.998242, 0.99825, 0.998257, 0.998263, 0.99827, 0.998277, 0.998284, 0.998291, 0.998297, 0.998304, 0.998311, 0.998317, 0.998323, 0.99833, 0.998336, 0.998342, 0.998349, 0.998355, 0.998361, 0.998367, 0.998373, 0.998379, 0.998385, 0.998391, 0.998397, 0.998403, 0.998408, 0.998414, 0.99842, 0.998425, 0.998431, 0.998436, 0.998442, 0.998447, 0.998453, 0.998458, 0.998463, 0.998469, 0.998474, 0.998479, 0.998484, 0.998489, 0.998495, 0.9985, 0.998505, 0.99851, 0.998515, 0.998519, 0.998524, 0.998529, 0.998534, 0.998539, 0.998544, 0.998548, 0.998553, 0.998558, 0.998562, 0.998567, 0.998571, 0.998576, 0.99858, 0.998585, 0.998589, 0.998593, 0.998598, 0.998602, 0.998606, 0.998611, 0.998615, 0.998619, 0.998623, 0.998627, 0.998632, 0.998636, 0.99864, 0.998644, 0.998648, 0.998652, 0.998656, 0.99866, 0.998664, 0.998668, 0.998671, 0.998675, 0.998679, 0.998683, 0.998687, 0.99869, 0.998694, 0.998698, 0.998701, 0.998705, 0.998709, 0.998712, 0.998716, 0.99872, 0.998723, 0.998727, 0.99873, 0.998734, 0.998737, 0.99874, 0.998744, 0.998747, 0.998751, 0.998754, 0.998757, 0.998761, 0.998764, 0.998767, 0.99877, 0.998774, 0.998777, 0.99878, 0.998783, 0.998786, 0.99879, 0.998793, 0.998796, 0.998799, 0.998802, 0.998805, 0.998808, 0.998811, 0.998814, 0.998817, 0.99882, 0.998823, 0.998826, 0.998829, 0.998832, 0.998835, 0.998837, 0.99884, 0.998843, 0.998846, 0.998849, 0.998851, 0.998854, 0.998857, 0.99886, 0.998862, 0.998865, 0.998868, 0.998871, 0.998873, 0.998876, 0.998879, 0.998881, 0.998884, 0.998886, 0.998889, 0.998892, 0.998894, 0.998897, 0.998899, 0.998902, 0.998904, 0.998907, 0.998909, 0.998912, 0.998914, 0.998917, 0.998919, 0.998921, 0.998924, 0.998926, 0.998929, 0.998931, 0.998933, 0.998936, 0.998938, 0.99894, 0.998943, 0.998945, 0.998947, 0.99895, 0.998952, 0.998954, 0.998956, 0.998959, 0.998961, 0.998963, 0.998965, 0.998967, 0.99897, 0.998972, 0.998974, 0.998976, 0.998978, 0.99898, 0.998982, 0.998985, 0.998987, 0.998989, 0.998991, 0.998993, 0.998995, 0.998997, 0.998999, 0.999001, 0.999003, 0.999005, 0.999007, 0.999009, 0.999011, 0.999013, 0.999015, 0.999017, 0.999019, 0.999021, 0.999023, 0.999025, 0.999027, 0.999028, 0.99903, 0.999032, 0.999034, 0.999036, 0.999038, 0.99904, 0.999042, 0.999043, 0.999045, 0.999047, 0.999049, 0.999051, 0.999052, 0.999054, 0.999056, 0.999058, 0.99906, 0.999061, 0.999063, 0.999065, 0.999067, 0.999068, 0.99907, 0.999072, 0.999073, 0.999075, 0.999077, 0.999078, 0.99908, 0.999082, 0.999084, 0.999085, 0.999087, 0.999088, 0.99909, 0.999092, 0.999093, 0.999095, 0.999097, 0.999098, 0.9991, 0.999101, 0.999103, 0.999104, 0.999106, 0.999108, 0.999109, 0.999111, 0.999112, 0.999114, 0.999115, 0.999117, 0.999118, 0.99912, 0.999121, 0.999123, 0.999124 };




        public static double Getc4(int size)
        {
            if (size == 0)
                throw new ArgumentException("size is 0");
            if (size < 101)
                return Constants_c4[size - 1];
            else
                return 1;
        }

        public static double Getc5(int size)
        {
            if (size == 0)
                throw new ArgumentException("size is 0");
            if (size < 101)
                return Constants_c5[size - 1];
            else
                return Constants_c5[99]; //임시 
        }


        public static double Getd2(int size)
        {
            if (size == 0)
                throw new ArgumentException("size is 0");
            if (size < 51)
                return Constants_d2[size - 1];
            else
                return Constants_d2[49]; // 임시 
        }


        public static double Getd3(int size)
        {
            if (size == 0)
                throw new ArgumentException("size is 0");
            if (size < 26)
                return Constants_d2[size - 1];
            else
            {
                return 0.80818 - (0.0051871 * (double)size) + (0.00005098 * ((double)size * (double)size)) - (0.00000019 * ((double)size * (double)size * (double)size));
                //임시입니다. size 50까지는 맞지만 51부터도 맞는지는 확인 안되었습니다.
            }
        }

        public static double Getd4(int size)
        {
            if (size == 0)
                throw new ArgumentException("size is 0");
            if (size < 26)
                return Constants_d4[size - 1];
            else
            {
                return 2.88606 + (0.051313 * (double)size) - (0.00049243 * ((double)size * (double)size)) + (0.00000188 * ((double)size * (double)size * (double)size));
            }
        }

        public static double Getc4_(int size)
        {
            if (size == 0)
                throw new ArgumentException("size is 0");
            if (size < 501)
                return Constants_c4_[size - 1];
            else
                return 1;
        }
    }
    #endregion
}
