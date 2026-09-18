using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.StatisticsInput
{
    public class inputCorrelation : inputDefault
    {
        
        #region " MEMBER FIELD "
                
        /// <summary>
        /// 분석변수 리스트
        /// </summary>
        public int[] arrVariables = null;				// 분석변수 Index List

        #region [ STATISTICS ]
        /// 유의확률
        /// </summary>
        public bool IsPvalue = true;
        /// <summary>
        /// 회귀식
        /// </summary>
        public bool IsRegression = true;
        /// <summary>
        /// Spearman
        /// </summary>
        public bool IsSpearman = false;


        #endregion

        #region [ GRAPHS ]
        /// <summary>
        /// 산점도
        /// </summary>
        public bool IsScatter = true;
        #endregion


        #endregion

        #region " CREATOR "
        public inputCorrelation(string ResultFilePath)
            : this("Coefficient of correlation", ResultFilePath)
        {
        }


        public inputCorrelation(string Title, string ResultFilePath)
        {
            this.Type = StatType.CorrelationAnalysis;
            this.Title = Title;
            this.ResultFilePath = ResultFilePath;
        }
        #endregion        

    }
}
