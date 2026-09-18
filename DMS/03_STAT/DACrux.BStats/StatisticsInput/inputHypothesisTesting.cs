using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.StatisticsInput
{
    public class inputHypothesisTesting : inputDefault
    {
        #region " MEMBER FIELD "

        /// <summary>
        /// 분석변수 리스트
        /// </summary>
        public int[] arrVariables = null;				// 분석변수 Index List
        
        /// <summary>
        /// 분석변수 리스트에 해당하는 유효한 Row Index
        /// </summary>
        public int[] arrValidRowIndices = null;  // 분석변수에 해당하는 컬럼의 유효한 Max Row Index


        public DACrux.BStats.Core.TestingType Testing = DACrux.BStats.Core.TestingType.OneSampleZ;


        #region [ Options ]

        public bool IsDescriptiveStatistics = true;

        public bool IsConfidenceInterval = true;

        public bool IsCriticalValue = false;

        #endregion

        #region [ Methods ]

        public DACrux.BStats.Core.AlternativeType Alternative = DACrux.BStats.Core.AlternativeType.NotEqual;

        public double ConfidenceLevel = 95;

        public double TestingMean = double.NaN;

        public double TestingStd = double.NaN;

        public double TestingDifference = double.NaN;

        #endregion


        #endregion

        #region " CREATOR "
        /// <summary>
        /// 비정형분석용
        /// </summary>
        /// <param name="ResultFilePath"></param>
        public inputHypothesisTesting(string ResultFilePath) : this("HypothesisTesting", ResultFilePath)
        {            
        }

        /// <summary>
        /// DACrux용(정형분석용)
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="ResultFilePath"></param>
        public inputHypothesisTesting(string Title, string ResultFilePath)
        {            
            this.Type = StatType.HypothesisTesting;
            this.Title = Title;
            this.ResultFilePath = ResultFilePath;
        }

        #endregion
    }
}
