using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats
{
    #region " INTERFACE "

    public interface iGraphAnalysis
    {
        GraphInformation GraphInfo { get; }
    }
    public interface iStatInformation
    {
        GraphInformation[] GraphInformations { get; set; }
        StatInformation StatInfo { get; }
    }

    #endregion

    public class FactorInfo
    {
        public string ID = "";
        public string Name = "";
        public string LevelValue = "";
        public int Column = -1;
        public int Level = 2;

        public FactorInfo(string FID, string FName, string FLevelValue, int FCol, int FLevel)
        {
            ID = FID;
            Name = FName;
            LevelValue = FLevelValue;
            Column = FCol;
            Level = FLevel;
        }
    }

    public enum TaguchiAnalysisRule
    {
        SN_LARGER_BETTER, SN_NOMINAL_BEST_N, SN_NOMINAL_BEST_B, SN_SMALLER_BETTER
    }

}
