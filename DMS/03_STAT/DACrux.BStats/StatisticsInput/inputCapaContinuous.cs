using System;
using System.Collections.Generic;
using System.Text;
using DACrux.ProjectManager.UI;
using DACrux.BStats.Core;

namespace DACrux.BStats.StatisticsInput
{
    public class inputCapaContinuous : inputDefault
    {
        #region " MEMBER FIELD "

        /// <summary>
        /// 하나의 Column에 Value가 있을 경우
        /// </summary>
        private int m_iSingleColumn;


        /// <summary>
        /// 부분군 ID Column 선택여부
        /// </summary>
        private bool m_bSubgroupColumn = false;
        /// <summary>
        /// 부분군 ID Column
        /// </summary>
        private int m_iSubgroupColumn;

        /// <summary>
        /// 부분군 크기를 상수로 입력될 것인지
        /// </summary>
        public bool IsUseContantForSubgroupSize = false;

        /// <summary>
        /// 부분군 크기(상수)
        /// </summary>
        private int m_iSubGroupSize = 1;


                
        /// <summary>
        /// Sub Group이 여러 Column에 나눠져 있는 경우
        /// </summary>
        public int[] arrVariables = null;				// 분석변수 Index List
        


        /// <summary>
        /// 규격 하한
        /// </summary>
        private double m_dblLSL = double.NaN;

        /// <summary>
        /// 규격 상한
        /// </summary>
        private double m_dblUSL = double.NaN;

        /// <summary>
        /// 역사적 평균
        /// </summary>
        private double m_dblHistoricalMean = double.NaN;

        /// <summary>
        /// 역사적 부분군 내부 표준 편차
        /// </summary>
        private double m_dblHistoricalStdWithin = double.NaN;

        /// <summary>
        /// 역사적 부분군 사이 표준 편차
        /// </summary>
        private double m_dblHistoricalStdBetween = double.NaN;


        #region [ Estimate ]

        /// <summary>
        /// 부분군 내부 불편화 상수 사용
        /// </summary>
        public bool IsUseUnbiasingConstantsWithin = true;
        /// <summary>
        /// 부분군 내부 표준 편차 추정치
        /// </summary>
        public EstimationWithin EstimationWithinSubgroup = EstimationWithin.Pooled_standard_deviation;
        /// <summary>
        /// 이동 범위에 사용될 길이
        /// </summary>
        private int m_iMovingRangeLength = 2;
        /// <summary>
        /// 부분군 사이 표준 편차 추정치
        /// </summary>
        public EstimationBetween EstimationBetweenSubgroup = EstimationBetween.Average_moving_range; 
        /// <summary>
        /// 전체 표준 편차를 계산할 때 불편화 상수 사용
        /// </summary>
        public bool IsUseUnbiasingConstantsOverall = true;
        

        #endregion

        #region [ Options ]

        private double m_dblTarget = double.NaN;

        private double m_iKsigma = 6;

        public bool IsBetweenWithinAnalysis = true;

        public bool IsOverallAnalysis = true;

        public DisplayType ResultDisplayType = DisplayType.Parts_per_million;

        public StatisticType ResultStatisticType = StatisticType.Capability_stats;

        public bool IsIncludeConfidenceIntervals = false;

        private double m_dblConfidenceLevel = 95.0;

        public ConfidenceIntervalsType ResultConfidenceIntervalsType = ConfidenceIntervalsType.Two_Side;

        private string m_strUserTitle = string.Empty;


        #endregion

        #endregion

        #region " PROPERTY "

        
        public int SubGroupSize
        {
            get
            {
                return m_iSubGroupSize;
            }
            set
            {
                m_iSubGroupSize = value;
            }
        }


        public bool IsSubgroupColumn
        {
            get
            {
                return m_bSubgroupColumn;
            }
            set
            {
                m_bSubgroupColumn = value;
            }
        }

        public int SubgroupColumnIndex
        {
            get
            {
                return m_iSubgroupColumn;
            }
            set
            {
                m_iSubgroupColumn = value;
            }
        }

        public int SingleColumnIndex
        {
            get
            {
                return m_iSingleColumn;
            }
            set
            {
                m_iSingleColumn = value;
            }
        }

        

        public int MovingRangeofLength
        {
            get
            {
                return m_iMovingRangeLength;
            }
            set
            {
                m_iMovingRangeLength = value;
            }
        }

        public string UserTitle
        {
            get
            {
                return m_strUserTitle;
            }
            set
            {
                m_strUserTitle = value;
            }
        }

        public double ConfidenceLevel
        {
            get
            {
                return m_dblConfidenceLevel;
            }
            set
            {
                m_dblConfidenceLevel = value;
            }
        }

        public double Ksigma
        {
            get
            {
                return m_iKsigma;
            }
            set
            {
                m_iKsigma = value;
            }
        }

        public double Target
        {
            get
            {
                return m_dblTarget;
            }
            set
            {
                m_dblTarget = value;
            }
        }

        public double LSL
        {
            get
            {
                return m_dblLSL;
            }
            set
            {
                m_dblLSL = value;
            }
        }
        public double USL
        {
            get
            {
                return m_dblUSL;
            }
            set
            {
                m_dblUSL = value;
            }
        }
        public double HistoricalMean
        {
            get
            {
                return m_dblHistoricalMean;
            }
            set
            {
                m_dblHistoricalMean = value;
            }
        }
        public double HistoricalStdWithin
        {
            get
            {
                return m_dblHistoricalStdWithin;
            }
            set
            {
                m_dblHistoricalStdWithin = value;
            }
        }
        public double HistoricalStdBetween
        {
            get
            {
                return m_dblHistoricalStdBetween;
            }
            set
            {
                m_dblHistoricalStdBetween = value;
            }
        }

        #endregion

        #region " CREATOR "

        public inputCapaContinuous(string ResultFilePath) : this("Process Capability Continuous", ResultFilePath)
        {
        }

        public inputCapaContinuous(string Title, string ResultFilePath)
        {
            this.Type = StatType.CpCpkAnalysis_Continuous;
            this.Title = Title;
            this.ResultFilePath = ResultFilePath;
        }

       

        #endregion        
    }
}
