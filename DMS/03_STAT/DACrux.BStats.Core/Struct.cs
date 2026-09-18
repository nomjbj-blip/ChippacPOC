using System.Data;

namespace DACrux.BStats.Core
{
    #region " Descriptive Statistics "

    public struct SelectedStatistics
    {
        public bool IsMean;
        public bool IsMeanSE;
        public bool IsStandarddeviation;
        public bool IsVariance;
        public bool IsCoefficientofvariation;
        public bool IsTrimmedMean;
        public bool IsSum;
        public bool IsMin;
        public bool IsMax;
        public bool IsRange;
        public bool IsNnonmissing;
        public bool IsNmissing;
        public bool IsNtotal;
        public bool IsCumulativeN;
        public bool IsPercent;
        public bool IsCumulativePercent;
        public bool IsQ1;
        public bool IsMedian;
        public bool IsQ3;
        public bool IsInterquartileRange;
        public bool IsMode;
        public bool IsSumofSquares;
        public bool IsSkewness;
        public bool IsKurtosis;
        public bool IsMSSD;
        public bool IsBiased; //편향
        /// <summary>
        /// 초기상태로 세팅
        /// </summary>
        public void Reset()
        {
            IsMean = true;
            IsMeanSE = true;
            IsStandarddeviation = true;
            IsVariance = false;
            IsCoefficientofvariation = false;
            IsTrimmedMean = false;
            IsSum = false;
            IsMin = true;
            IsMax = true;
            IsRange = false;
            IsNnonmissing = true;
            IsNmissing = true;
            IsNtotal = false;
            IsCumulativeN = false;
            IsPercent = false;
            IsCumulativePercent = false;
            IsQ1 = true;
            IsMedian = true;
            IsQ3 = true;
            IsInterquartileRange = false;
            IsMode = false;
            IsSumofSquares = false;
            IsSkewness = false;
            IsKurtosis = false;
            IsMSSD = false;
            IsBiased = false;
        }
    }

    public struct SelectedGraphs
    {
        public bool IsHistogram;
        public bool IsHistogramNNormalCurve;
        public bool IsBoxPlot;
        public bool IsRawDataPlot;
        /// <summary>
        /// 초기상태로 세팅
        /// </summary>
        public void Reset()
        {
            IsHistogram = true;
            IsHistogramNNormalCurve = true;
            IsBoxPlot = true;
            IsRawDataPlot = true;
        }
    } //Core에서 사용하는 것 아님

    public struct ResultStatistics
    {
        public double Mean;
        public double MeanSe;
        public double Standarddeviation;
        public double Variance;
        public double Coefficientofvariation;
        public double TrimmedMean;
        public double Sum;
        public double Min;
        public double Max;
        public double Range;
        public int Nnonmissing;
        public int Nmissing;
        public int Ntotal;
        public int CumulativeN;
        public double Percent;
        public double CumulativePercent;
        public double Q1;
        public double Median;
        public double Q3;
        public double InterquartileRange;
        public double Mode;
        public int ModeN;
        public double SumofSquares;
        public double Skewness;
        public double Kurtosis;
        public double MSSD;
    }

    #endregion

    #region " Correlation "

    public struct SelectedCorrelation
    {
        public bool IsPvalue;
        //public bool IsScatter;
        //public bool IsRegression;
        public bool IsSpearman;
        /// <summary>
        /// 초기상태로 세팅
        /// </summary>
        public void Reset()
        {
            IsPvalue = true;
            //IsScatter = true;
            //IsRegression = true;
            IsSpearman = false;
        }
    }

    public struct ResultCorrelation
    {
        public double Pvalue;
        public double CoefficientCorrelation;
        public string Xvar;
        public string Yvar;
        public double Spearman;
        public double SpearmanPvalue;
    }
    #endregion

    #region " Process Capability Continuous "

    public struct SelectedCapaContinuous
    {
        //public bool IsUseConstantForSubgroupSize;
        //public int SubGroupSize;
        public double LowerSpec;
        public double UpperSpec;
        public double HistoricalMean;
        public double HistoricalWithinStd;
        public double HistoricalBetweenStd;

        public SelectedCapaContinuousEstimate Estimate;

        public SelectedCapaContinuousOptions SelectedCapaContinuousOptions;

        /// <summary>
        /// 초기상태로 세팅
        /// </summary>
        public void Reset()
        {
            //IsUseConstantForSubgroupSize = false;
            //SubGroupSize = 0;
            LowerSpec = double.NaN;
            UpperSpec = double.NaN;
            HistoricalMean = double.NaN;
            HistoricalWithinStd = double.NaN;
            HistoricalBetweenStd = double.NaN;
            Estimate.Reset();
            SelectedCapaContinuousOptions.Reset();
        }

    }

    public struct SelectedCapaContinuousEstimate
    {
        public bool IsUseUnbiasingWithin;
        public EstimationWithin EstimationWithinSubgroup;
        public int MovingRangeofLength;
        public EstimationBetween EstimationBetweenSubgroup;
        public bool IsUseUnbiasingOverall;

        public void Reset()
        {
            IsUseUnbiasingWithin = true;
            EstimationWithinSubgroup = EstimationWithin.Pooled_standard_deviation;
            MovingRangeofLength = 2;
            EstimationBetweenSubgroup = EstimationBetween.Average_moving_range;
            IsUseUnbiasingOverall = false;
        }
    }

    public struct SelectedCapaContinuousOptions
    {
        public double Target;
        public double KsigmaForCapability;
        public bool IsBetweenWithinAnalysis;
        public bool IsOverallAnalysis;
        public DisplayType ResultDisplayType;
        public StatisticType ResultStatisticType;
        public bool IsIncludeConfidenceIntervals;
        public double ConfidenceLevel;
        public ConfidenceIntervalsType ConfidenceIntervals;
        public string UserTitle;

        /// <summary>
        /// 초기상태로 세팅
        /// </summary>
        public void Reset()
        {
            Target = double.NaN;
            KsigmaForCapability = 6;
            IsBetweenWithinAnalysis = true;
            IsOverallAnalysis = true;
            ResultDisplayType = DisplayType.Parts_per_million;
            ResultStatisticType = StatisticType.Capability_stats;
            IsIncludeConfidenceIntervals = false;
            ConfidenceLevel = 95;
            ConfidenceIntervals = ConfidenceIntervalsType.Two_Side;
            UserTitle = string.Empty;
        }
    }

    public struct ResultCapaContinuous
    {
        #region Process Data
        public double LSL;
        public double Target;
        public double USL;
        public double Mean;
        public int N;
        public double Std_Between;
        public double Std_Within;
        public double Std_BW;
        public double Std_Overall;
        #endregion

        #region Observed Performance
        public double Observed_PPMlessLSL;
        public double Observed_PPMmoreUSL;
        public double Observed_PPMtotal;
        #endregion

        #region Exp. Within Performance
        public double Within_PPMlessLSL;
        public double Within_PPMmoreUSL;
        public double Within_PPMtotal;
        #endregion

        #region Exp. B/W Performance
        public double BW_PPMlessLSL;
        public double BW_PPMmoreUSL;
        public double BW_PPMtotal;
        #endregion

        #region Exp. Overall Performance
        public double Overall_PPMlessLSL;
        public double Overall_PPMmoreUSL;
        public double Overall_PPMtotal;
        #endregion

        #region Within Capability
        public double CpWithin;
        //public double CpWithin_Lower;
        //public double CpWithin_Upper;
        public double CPLWithin;
        public double CPUWithin;

        public double ZbenchWithin;
        public double ZlslWithin;
        public double ZuslWithin;

        public double CpkWithin;
        //public double CpkWithin_Lower;
        //public double CpkWithin_Upper;
        public double CCpkWithin;
        #endregion

        #region  B/W Capability
        public double CpBW;
        //public double CpBW_Lower;
        //public double CpBW_Upper;
        public double CPLBW;
        public double CPUBW;

        public double ZbenchBW;
        public double ZlslBW;
        public double ZuslBW;


        public double CpkBW;
        //public double CpkBW_Lower;
        //public double CpkBW_Upper;
        public double CCpkBW;
        #endregion

        #region Overall Capability
        public double Pp;
        //public double Pp_Lower;
        //public double Pp_Upper;
        public double PPL;
        public double PPU;


        public double ZbenchOverall;
        public double ZlslOverall;
        public double ZuslOverall;


        public double Ppk;
        //public double Ppk_Lower;
        //public double Ppk_Upper;
        public double Cpm;
        //public double Cpm_Lower;
        #endregion

    }

    #endregion

    #region " Regression "

    public struct SelectedRegression
    {
        /// <summary>
        /// 종속 컬럼 인덱스
        /// </summary>
        public int iResponse;

        /// <summary>
        /// 독립변수 변수 컬럼 인덱스 리스트
        /// </summary>
        public int[] arrPredictors;

        /// <summary>
        /// 절편유무
        /// </summary>
        public bool IsIntercept;

        public SelectedRegressionMethods Methods;
        public SelectedRegressionOptions Options;

        /// <summary>
        /// 초기상태로 세팅
        /// </summary>
        public void Reset()
        {
            iResponse = -1;
            arrPredictors = null;
            IsIntercept = true;
            Methods.Reset();
            Options.Reset();
        }
    }

    public struct SelectedRegressionMethods
    {
        public RegressionType ReAnalysisType;
        /// <summary>
        /// 변수선택 기준
        /// </summary>
        public bool IsUseAlpha;

        /// <summary>
        /// 무조건 독립변수로 포함하는 변수 컬럼 인덱스 리스트
        /// </summary>
        public int[] arrIncludePredictors;

        /// <summary>
        /// Stepwise 초기 모델에서 포함하고 있는 변수 컬럼 인덱스 리스트
        /// </summary>
        public int[] arrPredictorsInitialModel;

        /// <summary>
        /// 단계별 선택 - 전진기준
        /// </summary>e
        public double stepwiseAdd;

        /// <summary>
        /// 단계별 선택 - 후진기준
        /// </summary>
        public double stepwiseDrop;

        /// <summary>
        /// 전진 선택 기준
        /// </summary>
        public double forwardAdd;
        /// <summary>
        /// 후진 소거 기준
        /// </summary>
        public double backwardDrop;

        /// <summary>
        /// 초기상태로 세팅
        /// </summary>
        public void Reset()
        {
            ReAnalysisType = RegressionType.All;
            IsUseAlpha = true;
            arrIncludePredictors = null;
            arrPredictorsInitialModel = null;
            stepwiseAdd = 0.25;
            stepwiseDrop = 0.1;
            forwardAdd = 0.15;
            backwardDrop = 0.15;
        }
    }

    public struct SelectedRegressionOptions
    {

        #region Graph

        /// <summary>
        /// 잔차 대 예측변수 산점도 그릴 변수 목록
        /// </summary>
        public int[] arrGraphVariablesVsResiduals;

        /// <summary>
        /// 잔차의 정규 확률도
        /// </summary>
        public bool IsGraphProbabilityPlotResiduals;

        /// <summary>
        /// 잔차에 대한 히스토그램
        /// </summary>
        public bool IsGraphHistogramResiduals;

        /// <summary>
        /// 잔차 대 적합치
        /// </summary>
        public bool IsGraphResidualsVsFittedValues;

        /// <summary>
        /// 잔차 대 순서
        /// </summary>
        public bool IsGraphResidualsVSOrder;

        #endregion

        #region Result Option


        /// <summary>
        /// 회귀계수
        /// </summary>
        public bool IsTableCoefficients;

        /// <summary>
        /// 분산분석표 여부
        /// </summary>
        public bool IsTableAnova;

        /// <summary>
        /// 잔차
        /// </summary>
        public bool IsTableResiduals;

        /// <summary>
        /// 결정계수 관련 결과
        /// </summary>
        public bool IsTableR_Square;

        #endregion

        /// <summary>
        /// 초기상태로 세팅
        /// </summary>
        public void Reset()
        {
            arrGraphVariablesVsResiduals = null;
            IsGraphProbabilityPlotResiduals = true;
            IsGraphHistogramResiduals = true;
            IsGraphResidualsVsFittedValues = true;
            IsGraphResidualsVSOrder = true;

            IsTableCoefficients = true;
            IsTableAnova = true;
            IsTableResiduals = true;
            IsTableR_Square = true;
        }
    }

    public struct ResultRegression
    {
        public string ErrorInformation;  //선택된 변수가 없을 경우나 자료가 없는경우 등의 이상 발생시..출력할때 가장 먼저 확인할 것!!
        public string RegressionEquation;  //회귀식
        public DataTable TableofAnova;  //분산분석표
        public DataTable TableofCoefficients; //계수에 대한 표
        public DataTable TableofFitsResiduals; // 적합치와 잔차
        public DataTable TableofRsquared;  // 설명력...
        public DataSet TableforGraphs; // 각 DataTable에는 두개의 Column이 존재하며 TableName에는 두 Column으로 만들어진 Graph Title이 존재함.

        public void Reset()
        {
            ErrorInformation = string.Empty;
            RegressionEquation = string.Empty;
            if (TableofAnova != null)
                TableofAnova.Dispose();
            TableofAnova = null;
            if (TableofCoefficients != null)
                TableofCoefficients.Dispose();
            TableofCoefficients = null;
            if (TableofFitsResiduals != null)
                TableofFitsResiduals.Dispose();
            TableofFitsResiduals = null;
            if (TableofRsquared != null)
                TableofRsquared.Dispose();
            TableofRsquared = null;
            if (TableforGraphs != null)
                TableforGraphs.Dispose();
            TableforGraphs = null;
        }
    }
    #endregion

    #region " ANOVA "

    public struct SelectedAnovaGraph
    {

        #region Graph

        /// <summary>
        /// 잔차의 정규 확률도
        /// </summary>
        public bool IsGraphProbabilityPlotResiduals;

        /// <summary>
        /// 잔차에 대한 히스토그램
        /// </summary>
        public bool IsGraphHistogramResiduals;

        /// <summary>
        /// 잔차 대 적합치
        /// </summary>
        public bool IsGraphResidualsVsFittedValues;

        /// <summary>
        /// Box Plot
        /// </summary>
        public bool IsBoxPlot;


        #endregion

        /// <summary>
        /// 초기상태로 세팅
        /// </summary>
        public void Reset()
        {
            IsBoxPlot = false;
            IsGraphProbabilityPlotResiduals = false;
            IsGraphHistogramResiduals = false;
            IsGraphResidualsVsFittedValues = false;
        }
    }

    public struct SelectedAnova
    {
        /// <summary>
        /// Value Column이 하나의 Column으로 된 형태인지..
        /// </summary>
        public bool IsSingle;

        /// <summary>
        /// 하나의 Column에 Value가 있을 경우
        /// </summary>
        public int SingleColumn;

        /// <summary>
        /// Group Column Index
        /// </summary>
        public int GroupColumn;

        /// <summary>
        /// Sub Group이 여러 Column에 나눠져 있는 경우
        /// </summary>
        public int[] arrVariables;

        /// <summary>
        /// 분석변수 리스트에 해당하는 유효한 Row Index
        /// </summary>
        public int[] arrValidRowIndices;

        public SelectedAnovaGraph oGraph;

        public void Reset()
        {
            SingleColumn = -1;
            GroupColumn = -1;
            arrVariables = null;
            arrValidRowIndices = null;
            oGraph.Reset();
        }
    }

    public struct ResultAnova
    {
        public string GroupTitle;
        public string ErrorInformation;  //선택된 변수가 없을 경우나 자료가 없는경우 등의 이상 발생시..출력할때 가장 먼저 확인할 것!!
        public DataTable TableofStatistic;  //그룹별 기술통계량
        public DataTable TableofAnova;  //분산분석표
        public DataTable TableofRsquared;  // 설명력...
        public DataSet TableforGraphs; // 각 DataTable에는 두개의 Column이 존재하며 TableName에는 두 Column으로 만들어진 Graph Title이 존재함.

        public void Reset()
        {
            GroupTitle = string.Empty;
            ErrorInformation = string.Empty;
            if (TableofAnova != null)
                TableofAnova.Dispose();
            TableofAnova = null;
            if (TableofRsquared != null)
                TableofRsquared.Dispose();
            TableofRsquared = null;
            if (TableforGraphs != null)
                TableforGraphs.Dispose();
            TableforGraphs = null;
        }
    }

    #endregion

    #region " Hypothesis Testing "

    public struct SelectedHypothesisTestingMethods
    {
        public DACrux.BStats.Core.AlternativeType Alternative;
        public double ConfidenceLevel;
        public double TestingMean;
        public double TestingStd;
        public double TestingDifference;

        public void Reset()
        {
            Alternative = DACrux.BStats.Core.AlternativeType.NotEqual;
            ConfidenceLevel = 95;
            TestingMean = double.NaN;
            TestingStd = double.NaN;
            TestingDifference = double.NaN;
        }
    }

    public struct SelectedHypothesisTestingOptions
    {
        public bool IsDescriptiveStat;
        public bool IsConfidenceInterval;
        public bool IsCriticalValue;
        public void Reset()
        {
            IsDescriptiveStat = true;
            IsConfidenceInterval = false;
            IsCriticalValue = false;
        }
        
    }

    public struct SelectedHypothesisTesting
    {
        public DACrux.BStats.Core.TestingType Testing;
        public SelectedHypothesisTestingMethods oMethods;
        public SelectedHypothesisTestingOptions Options;
        public void Reset()
        {
            Testing = TestingType.OneSampleT;
            oMethods.Reset();
            Options.Reset();
        }
    }

    public struct ResultHypothesisTesting
    {
        public string ErrorInformation;  //선택된 변수가 없을 경우나 자료가 없는경우 등의 이상 발생시..출력할때 가장 먼저 확인할 것!!
        public string VariableName1;
        public string VariableName2;
        public string StatisticName;
        public string ResultDescription;
        public double P;        
        public double Statistic;
        public double LeftCriticalValue;
        public double RightCriticalValue;
        public double LowerConfidenceLimit;
        public double UpperConfidenceLimit;
        public double Alpha;
        public double TestingMean;
        public double TestingStd;
        public double TestingDifference;
        public DataTable dtDescriptiveStat;
        public double EstimateForDifference;
        /// <summary>
        /// Unpaired Testing 인 경우에 출력되는 Pooled standard deviation
        /// </summary>
        public double PooledStDev;
        public void Reset()
        {
            ErrorInformation = string.Empty;
            VariableName1 =string.Empty;
            VariableName2 = string.Empty;
            StatisticName = string.Empty;
            LeftCriticalValue = double.NaN;
            RightCriticalValue = double.NaN;
            P = double.NaN;
            Statistic = double.NaN;
            LowerConfidenceLimit = double.NaN;
            UpperConfidenceLimit = double.NaN;
            Alpha = double.NaN;
            TestingMean = double.NaN;
            TestingStd = double.NaN;
            TestingDifference = double.NaN;
            dtDescriptiveStat = null;
            EstimateForDifference = double.NaN;
            PooledStDev = double.NaN;
        }
    }
    #endregion

}
