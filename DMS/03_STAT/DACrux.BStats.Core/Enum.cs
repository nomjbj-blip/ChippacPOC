using System;
namespace DACrux.BStats.Core
{
    #region " Capability "
    public enum EstimationWithin
    {
        Rbar,
        Sbar,
        Pooled_standard_deviation
    }

    public enum EstimationBetween
    {
        Average_moving_range,
        Median_moving_range,
        Square_root_of_MSSD
    }

    public enum DisplayType
    {
        Parts_per_million,
        Percents
    }

    public enum StatisticType
    {
        Capability_stats,
        Benchmark_Z
    }

    public enum ConfidenceIntervalsType
    {
        Two_Side,
        Upper,
        Lower
    }

    #endregion

    #region " Regression "
    public enum RegressionType
    {
        All, Forward, Backward, Stepwise
    }
    #endregion

    #region " Hypothesis Testing "
    public enum TestingType
    {
        OneSampleT, OneSampleZ, TwoSamplePaired, TwoSampleUnpaired
    }
    public enum AlternativeType
    {
        Less, NotEqual, Greater
    }
    #endregion 
}
