using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.StatisticsInput
{
    public class inputDescriptiveStatistics : inputDefault
    {
        #region " MEMBER FIELD "
                
        /// <summary>
        /// 분석변수 리스트
        /// </summary>
        public int[] arrVariables = null;				// 분석변수 Index List
        /// <summary>
        /// 분류변수 리스트
        /// </summary>
        public int[] arrByVariables = null;				// 분류변수 Index List

        /// <summary>
        /// 분석변수 리스트에 해당하는 유효한 Row Index
        /// </summary>
        public int[] arrValidRowIndices = null;  // 분석변수에 해당하는 컬럼의 유효한 Max Row Index

        #region [ STATISTICS ]
        /// <summary>
        /// 평균
        /// </summary>
        public bool IsMean = true;
        /// <summary>
        /// 평균의 SE
        /// </summary>
        public bool IsMeanSE = true;
        /// <summary>
        /// 표준 편차
        /// </summary>
        public bool IsStandarddeviation = true;
        /// <summary>
        /// 분산
        /// </summary>
        public bool IsVariance = false;
        /// <summary>
        /// 변동 계수
        /// </summary>
        public bool IsCoefficientofvariation = false;
        /// <summary>
        /// 절사 평균
        /// </summary>
        public bool IsTrimmedMean = false;
        /// <summary>
        /// 합
        /// </summary>
        public bool IsSum = false;
        /// <summary>
        /// 최소값
        /// </summary>
        public bool IsMin = true;
        /// <summary>
        /// 최대값
        /// </summary>
        public bool IsMax = true;
        /// <summary>
        /// 범위
        /// </summary>
        public bool IsRange = false;
        /// <summary>
        /// 비결측값 개수
        /// </summary>
        public bool IsNnonmissing = true;
        /// <summary>
        /// 결측값 개수
        /// </summary>
        public bool IsNmissing = true;
        /// <summary>
        /// 관측치 개수
        /// </summary>
        public bool IsNtotal = false;
        /// <summary>
        /// 누적 개수
        /// </summary>
        public bool IsCumulativeN = false;
        /// <summary>
        /// 백분율
        /// </summary>
        public bool IsPercent = false;
        /// <summary>
        /// 누적 백분율
        /// </summary>
        public bool IsCumulativePercent = false;
        /// <summary>
        /// 제1사분위수
        /// </summary>
        public bool IsQ1 = true;
        /// <summary>
        /// 중위수
        /// </summary>
        public bool IsMedian = true;
        /// <summary>
        /// 제3사분위수
        /// </summary>
        public bool IsQ3 = true;
        /// <summary>
        /// 사분위간 범위
        /// </summary>
        public bool IsInterquartileRange = false;
        /// <summary>
        /// 최빈값
        /// </summary>
        public bool IsMode = false;
        /// <summary>
        /// 제곱합
        /// </summary>
        public bool IsSumofSquares = false;
        /// <summary>
        /// 왜도
        /// </summary>
        public bool IsSkewness = false;
        /// <summary>
        /// 첨도
        /// </summary>
        public bool IsKurtosis = false;
        /// <summary>
        /// MSSD
        /// </summary>
        public bool IsMSSD = false; //MSSD(연속 차이의 제곱 평균) : 분산의 추정치로서 사용합니다. 연속된 관측치의 차이를 제곱하여 합한 다음, 그 합의 평균을 2로 나눈 값입니다.

        #endregion

        #region [ GRAPHS ]

        /// <summary>
        /// 히스토그램
        /// </summary>
        public bool IsHistogram = true;
        /// <summary>
        /// 정규곡선이 있는 히스토그램
        /// </summary>
        public bool IsHistogramNNormalCurve = true;
        /// <summary>
        /// 박스그림
        /// </summary>
        public bool IsBoxPlot = true;

        /// <summary>
        /// Scatter Plot
        /// </summary>
        public bool IsRawDataPlot = true;

        #endregion


        #endregion

        #region " CREATOR "

        /// <summary>
        /// 비정형분석용
        /// </summary>
        /// <param name="ResultFilePath"></param>
        public inputDescriptiveStatistics(string ResultFilePath) : this("DescriptiveStatistics", ResultFilePath)
        {            
        }

        /// <summary>
        /// DACrux용(정형분석용)
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="ResultFilePath"></param>
        public inputDescriptiveStatistics(string Title, string ResultFilePath)
        {            
            this.Type = StatType.DescriptiveAnalysis;
            this.Title = Title;
            this.ResultFilePath = ResultFilePath;
        }

        #endregion        

    }
}
