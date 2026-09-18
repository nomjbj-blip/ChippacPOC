using System;
using System.Collections;

namespace DACrux.BStats
{
    /// <summary>
    /// 클래스  명: Combination<br/>
    /// 클래스요약: Combination의 경우를 구하는 클래스<br/>
    /// 작  성  자: MiracomInc<br/>
    /// 최초작성일: 2005-08-01<br/>
    /// 최종수정자: MiracomInc<br/>
    /// 최종수정일: 2005-12-31<br/>
    /// 상세  설명: Combination의 경우를 구하는 클래스<br/>
    /// 변경  내용: <br/>
    /// </summary>
    public class Combination
    {
        /// <summary>
        /// 전체 데이터 수
        /// </summary>
        int iMaxNumber = 0;

        /// <summary>
        /// 선택할 데이터 수
        /// </summary>
        int iCombiNumber = 0;

        /// <summary>
        /// 전체 데이터
        /// </summary>
        ArrayList alData = null;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="maxNumber">전체 데이터 수</param>
        /// <param name="combiNumber">전에 데이터에서 뽑을 데이터 수</param>
        public Combination(int maxNumber, int combiNumber)
        {
            this.iMaxNumber = maxNumber;
            this.alData = new ArrayList();

            for (int i = 0; i < this.iMaxNumber; i++)
            {
                this.alData.Add(i.ToString());
            }

            this.iCombiNumber = combiNumber;
        }

        /// <summary>
        /// 재귀호출을 사용한 경우의수 생성 
        /// </summary>
        /// <returns>경우의 수</returns>
        public ArrayList GenerateCombiation()
        {
            ArrayList alResult = new ArrayList();

            // combiNumber가 1인 경우 
            // 재귀가 끝나는 조건이다.
            if (this.iCombiNumber < 2)
            {
                for (int i = 0; i < this.alData.Count; i++)
                {
                    alResult.Add(this.alData[i]);
                }

                return alResult;
            }

            // combiNumber가 2이상인 경우
            // 계속 재귀 호출이 된다.
            Combination combiBefore = new Combination(this.iMaxNumber, this.iCombiNumber - 1);
            ArrayList alBefore = combiBefore.GenerateCombiation();

            for (int i = 0; i < alBefore.Count; i++)
            {
                string strBefore = alBefore[i].ToString();

                string strIndex = string.Empty;
                string strMax = strBefore.Substring(0, 1);

                // strBefore에서 가장 큰값 가져오기
                for (int j = 0; j < strBefore.Length; j++)
                {
                    strIndex = strBefore.Substring(j, 1);
                    if (strMax.CompareTo(strIndex) < 0) strMax = strIndex;
                }

                int iMaxIndex = Convert.ToInt32(strMax);

                // 가장 큰값보다 큰나머지 배열하고 조합 마들기

                string strResult = string.Empty;

                for (int j = iMaxIndex + 1; j < this.alData.Count; j++)
                {
                    strResult = strBefore + this.alData[j].ToString();
                    alResult.Add(strResult);
                }
            }

            return alResult;
        }
    }
}
