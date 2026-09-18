using System;
using System.Collections.Generic;
using System.Text;
using DACrux.ProjectManager.UI;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatisticsInput
{
    public class inputANOVA : inputDefault
    {
        #region " MEMBER FIELD "


        /// <summary>
        /// Value Column이 하나의 Column으로 된 형태인지..
        /// </summary>
        public bool IsSingle;

        /// <summary>
        /// 하나의 Column에 Value가 있을 경우
        /// </summary>
        public int SingleColumn = -1;

        /// <summary>
        /// Group Column Index
        /// </summary>
        public int GroupColumn = -1;

        /// <summary>
        /// Sub Group이 여러 Column에 나눠져 있는 경우
        /// </summary>
        public int[] arrVariables = null;

        /// <summary>
        /// 분석변수 리스트에 해당하는 유효한 Row Index
        /// </summary>
        public int[] arrValidRowIndices = null;

        #region Graph

        /// <summary>
        /// 잔차의 정규 확률도
        /// </summary>
        public bool IsGraphProbabilityPlotResiduals = false;

        /// <summary>
        /// 잔차에 대한 히스토그램
        /// </summary>
        public bool IsGraphHistogramResiduals = false;

        /// <summary>
        /// 잔차 대 적합치
        /// </summary>
        public bool IsGraphResidualsVsFittedValues = false;

        /// <summary>
        /// 잔차 대 순서
        /// </summary>
        public bool IsBoxPlot = true;

        #endregion

        #endregion

        #region " CREATOR "

        public inputANOVA(string ResultFilePath)
            : this("ANOVA", ResultFilePath)
        {
        }

        public inputANOVA(string Title, string ResultFilePath)
        {
            this.Type = StatType.ANOVA;
            this.Title = Title;
            this.ResultFilePath = ResultFilePath;
        }

        #endregion
    }
}
