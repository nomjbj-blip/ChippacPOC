using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DACrux.BStats.StatisticsInput
{

    /// <summary>
    /// 클래스  명: FACTORINPUT<br/>
    /// 클래스요약: 요인분석 입력 클래스<br/>
    /// 작  성  자: MiracomInc<br/>
    /// 최초작성일: 2005-08-01<br/>
    /// 최종수정자: MiracomInc<br/>
    /// 최종수정일: 2005-12-31<br/>
    /// 상세  설명: 요인분석 입력 클래스<br/>
    /// 변경  내용: <br/>
    /// </summary>
    public class inputFactorAnalysis : inputDefault
    {
        /// <summary>
        /// 분석변수 리스트
        /// </summary>
        public ArrayList analysisVariables = null;
        /// <summary>
        /// 분석변수 인자
        /// </summary>
        public ArrayList analysisParams = null;

        /// <summary>
        /// 풀링되지 않을 변수 리스트
        /// </summary>
        public ArrayList unPooledParams = null;
        /// <summary>
        /// 풀링될 변수 리스트
        /// </summary>
        public ArrayList pooledParams = null;

        /// <summary>
        /// 기술통계 출력결과 유무
        /// </summary>
        public bool bDescriptiveStatistics = false;
        /// <summary>
        /// 분산분석표 여부
        /// </summary>
        public bool bAnovaAnalysis = false;
        /// <summary>
        /// 교호작용 포함 여부
        /// </summary>
        public bool bInteractionEffect = false;
        /// <summary>
        /// 주효과 포함 여부
        /// </summary>
        public bool bMainEffect = false;
        /// <summary>
        /// 추정식 포함 여부
        /// </summary>
        public bool bRegressionEquation = false;


        /// <summary>
        /// 교호작용 다이어그램 포함여부
        /// </summary>
        public bool bInteractionDiagram = false;
        /// <summary>
        /// 주효과 다이어그램 여부
        /// </summary>
        public bool bMainEffectDiagram = false;
        /// <summary>
        /// 잔차 플롯 여부
        /// </summary>
        public bool bResidualPlot = false;

        /// <summary>
        /// 컬럼수
        /// </summary>
        public int cols = 2;
        /// <summary>
        /// 행수
        /// </summary>
        public int rows = 4;
        /// <summary>
        /// 인자 수
        /// </summary>
        public int factorType = 2;

        /// <summary>
        /// 풀링기능 사용여부
        /// </summary>
        public bool bPooling = false;


        /// <summary>
        ///  FACTORINPUT 클래스의 생성자
        /// </summary>
        public inputFactorAnalysis()
        {
            analysisVariables = new ArrayList();
            analysisParams = new ArrayList();
            unPooledParams = new ArrayList();
            pooledParams = new ArrayList();
        }
    }

}
