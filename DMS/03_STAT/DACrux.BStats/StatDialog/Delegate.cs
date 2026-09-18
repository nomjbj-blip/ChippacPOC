using System;
using System.Collections.Generic;
using System.Text;
using DACrux.BStats.StatisticsInput;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatDialog
{
    #region Descriptive
    public delegate void DSelectedDescriptiveStatistics(inputDescriptiveStatistics oSelected);
    public delegate void DSelectedGraph(SelectedGraphs oSelected);
    public delegate void DSelectedStatistics(SelectedStatistics oSelectedStatistics);
    #endregion

    #region Capability
    public delegate void DSelectedCapaContinuous(inputCapaContinuous oSelected);
    public delegate void DSelectedCapaContinuousOptions(SelectedCapaContinuousOptions oSelected);
    public delegate void DSelectedCapaContinuousEstimation(SelectedCapaContinuousEstimate oSelected);
    #endregion

    #region Corr
    public delegate void DSelectedCorrelation(inputCorrelation oSelected);
    #endregion

    #region Regression
    public delegate void DSelectedRegression(inputRegression oSelected);
    public delegate void DSelectedRegressionMethods(SelectedRegressionMethods oSelected);
    public delegate void DSelectedRegressionOptions(SelectedRegressionOptions oSelected);
    #endregion

    #region ANOVA
    public delegate void DSelectedAnoa(inputANOVA oSelected);
    public delegate void DSelectedAnovaGraph(SelectedAnovaGraph oSelected);
    #endregion

    #region Hypothesis Testing
    public delegate void DSelectedHypothesisTestingOptions(SelectedHypothesisTestingOptions oSelected);
    public delegate void DSelectedHypothesisTestingMethods(SelectedHypothesisTestingMethods oSelected);
    public delegate void DSelectedHypothesisTesting(inputHypothesisTesting oSelected);
    #endregion

    #region Taguchi
    public delegate void DSelectedTaguchi(inputTaguchi oSelected);
    #endregion
}
