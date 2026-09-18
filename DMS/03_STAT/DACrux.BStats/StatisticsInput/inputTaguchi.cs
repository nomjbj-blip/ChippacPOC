using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.StatisticsInput
{
    /// </summary>
    public class inputTaguchi : inputDefault
    {
        /// <summary>
        /// 분석변수 리스트
        /// </summary>
        public ArrayList AnalysisVariables = null;
        /// <summary>
        /// 분석변수 인자
        /// </summary>
        public FactorInfo[] FactorInfos = null;

        /// <summary>
        /// 분석 공식
        /// </summary>
        public TaguchiAnalysisRule tgcAnalysisRule = TaguchiAnalysisRule.SN_NOMINAL_BEST_B;

        /// <summary>
        /// Option창에 Target
        /// </summary>
        public bool OptionCheckTarget = false;

        /// <summary>
        /// Option창에 표준편차 In(s)값 포함 여부
        /// </summary>
        public bool OptionCheckStdDevIns = false;

        /// <summary>
        /// 평균 Graph
        /// </summary>
        public bool GraphAverage = true;

        /// <summary>
        /// 신호대 잡음비 그래프
        /// </summary>
        public bool GraphSNRatio = true;

        /// <summary>
        /// 표준편차 그래프
        /// </summary>
        public bool GraphSTDDev = false;

        /// <summary>
        /// 분석창의 [반응 표 표시 대상 통계량] 
        /// </summary>
        public bool Analysis_Y_SNRatio = true;
        public bool Analysis_Y_Average = true;
        public bool Analysis_Y_Stdev = false;

        /// <summary>
        /// 분석창의 [선형 모형 적합 대상 통계량]
        /// </summary>
        public bool Analysis_L_SNRatio = false;
        public bool Analysis_L_Average = false;
        public bool Analysis_L_Stdev = false;

        /// <summary>
        /// 기술통계 출력결과 유무
        /// </summary>
        public bool bDescriptiveStatistics = false;

        /// <summary>
        ///  TaguchiINPUT 클래스의 생성자
        /// </summary>
        public inputTaguchi()
        {
            AnalysisVariables = null;
            FactorInfos = null;
        }


        #region " CREATOR "

        /// <summary>
        /// DACrux용(정형분석용)
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="ResultFilePath"></param>
        public inputTaguchi(string Title, string ResultFilePath, FactorInfo[] Factors)
        {
            this.FactorInfos = Factors;
            this.Type = StatType.DOE_TaguchiAnalysis;
            this.Title = Title;
            this.ResultFilePath = ResultFilePath;
        }

        #endregion
    }
}
