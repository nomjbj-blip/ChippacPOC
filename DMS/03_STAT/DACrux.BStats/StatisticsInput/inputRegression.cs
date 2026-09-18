using System;
using System.Collections.Generic;
using System.Text;
using DACrux.ProjectManager.UI;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatisticsInput
{
    public class inputRegression : inputDefault
    {

        #region " MEMBER FIELD "

        /// <summary>
		/// 종속 컬럼 인덱스
		/// </summary>
        public int iResponse = -1;

		/// <summary>
        /// 독립변수 변수 컬럼 인덱스 리스트
		/// </summary>
        public int[] arrPredictors;

        /// <summary>
        /// 절편유무
        /// </summary>
        public bool IsIntercept = true;
        
        #region Graph

        /// <summary>
        /// 잔차 대 예측변수 산점도 그릴 변수 목록
        /// </summary>
        public int[] arrGraphVariablesVsResiduals;

        /// <summary>
        /// 잔차의 정규 확률도
        /// </summary>
        public bool IsGraphProbabilityPlotResiduals = true;

        /// <summary>
        /// 잔차에 대한 히스토그램
        /// </summary>
        public bool IsGraphHistogramResiduals = true;

        /// <summary>
        /// 잔차 대 적합치
        /// </summary>
        public bool IsGraphResidualsVsFittedValues = true;

        /// <summary>
        /// 잔차 대 순서
        /// </summary>
        public bool IsGraphResidualsVSOrder = true;

        #endregion

        #region Result Option

        

        /// <summary>
        /// 회귀계수
        /// </summary>
        public bool IsTableCoefficients = true;

        /// <summary>
        /// 결정계수 관련 결과
        /// </summary>
        public bool IsTableR_Square = true;

        /// <summary>
        /// 분산분석표 여부
        /// </summary>
        public bool IsTableAnova = true;

        /// <summary>
        /// 잔차
        /// </summary>
        public bool IsTableResiduals = false;
       
        

        #endregion

        #region 변수 선택 관련

        /// <summary>
        /// 회귀분석형태 선택
        /// </summary>
        public DACrux.BStats.Core.RegressionType ReAnalysisType = DACrux.BStats.Core.RegressionType.All;

        /// <summary>
        /// 변수선택 기준
        /// </summary>
        public bool IsUseAlpha = true;

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
		/// </summary>
		public double stepwiseAdd = 0.25;

		/// <summary>
		/// 단계별 선택 - 후진기준
		/// </summary>
        public double stepwiseDrop = 0.1;

		/// <summary>
		/// 전진 선택 기준
		/// </summary>
        public double forwardAdd = 0.15;
		/// <summary>
		/// 후진 소거 기준
		/// </summary>
        public double backwardDrop = 0.15;

        #endregion

        #endregion

        #region " CREATOR "

        public inputRegression(string ResultFilePath)
            : this("Regression Analysis", ResultFilePath)
        {
        }

        /// <summary>
		/// REGRESSIONINPUT 클래스의 생성자
		/// </summary>
        public inputRegression(string Title, string ResultFilePath)
		{
            this.Type = StatType.RegressionAnalysis;
            this.Title = Title;
            this.ResultFilePath = ResultFilePath;
        }

        #endregion
    }
}
