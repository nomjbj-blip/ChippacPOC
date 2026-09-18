using System;
using System.Collections;

namespace DACrux.BStats
{
    #region 열거자

    /// <summary>
    /// 실험순서
    /// </summary>
    public enum eExcuteOrder
    {
        Random, Standard
    }

    /// <summary>
    /// 수준
    /// </summary>
    public enum eLevel
    {
        Level2, Level3
    }

    /// <summary>
    /// 요인분석의 설계타입
    /// </summary>
    public enum eDesignType
    {
        DesignType_2_2_Full,

        DesignType_2_3_Full,
        DesignType_2_3_Minus1,

        DesignType_2_4_Full,
        DesignType_2_4_Minus1,

        DesignType_2_5_Full,
        DesignType_2_5_Minus1,
        DesignType_2_5_Minus2,

        DesignType_2_6_Full,
        DesignType_2_6_Minus1,
        DesignType_2_6_Minus2,
        DesignType_2_6_Minus3,

        DesignType_2_7_Full,
        DesignType_2_7_Minus1,
        DesignType_2_7_Minus2,
        DesignType_2_7_Minus3,
        DesignType_2_7_Minus4,

        DesignType_2_8_Minus1,
        DesignType_2_8_Minus2,
        DesignType_2_8_Minus3,
        DesignType_2_8_Minus4,

        DesignType_2_9_Minus2,
        DesignType_2_9_Minus3,
        DesignType_2_9_Minus4,
        DesignType_2_9_Minus5,

        DesignType_2_10_Minus3,
        DesignType_2_10_Minus4,
        DesignType_2_10_Minus5,
        DesignType_2_10_Minus6,

        DesignType_2_11_Minus4,
        DesignType_2_11_Minus5,
        DesignType_2_11_Minus6,
        DesignType_2_11_Minus7,

        DesignType_2_12_Minus5,
        DesignType_2_12_Minus6,
        DesignType_2_12_Minus7,
        DesignType_2_12_Minus8,

        DesignType_2_13_Minus6,
        DesignType_2_13_Minus7,
        DesignType_2_13_Minus8,
        DesignType_2_13_Minus9,

        DesignType_2_14_Minus7,
        DesignType_2_14_Minus8,
        DesignType_2_14_Minus9,
        DesignType_2_14_Minus10,

        DesignType_2_15_Minus8,
        DesignType_2_15_Minus9,
        DesignType_2_15_Minus10,
        DesignType_2_15_Minus11,

        DesignType_3_2_Full,
        DesignType_3_3_Full,
        DesignType_3_4_Full,
        DesignType_3_5_Full
    }

    #endregion

    /// <summary>
    /// 클래스  명: DefinitionItem<br/>
    /// 클래스요약: 요인분석의 설계타입에 관한 정보 보유<br/>
    /// 작  성  자: MiracomInc<br/>
    /// 최초작성일: 2005-08-01<br/>
    /// 최종수정자: MiracomInc<br/>
    /// 최종수정일: 2005-12-31<br/>
    /// 상세  설명: 요인분석의 설계타입에 관한 정보 보유, Default로 2**(2) 타입의 값을 갖는다.<br/>
    /// 변경  내용: <br/>
    /// </summary>
    public class DefinitionItem
    {
        /// <summary>
        /// 실험수
        /// </summary>
        public string NumberOfExperiments = "4";

        /// <summary>
        /// 디자인 제목
        /// </summary>
        public string Title = "2**(2)";

        /// <summary>
        /// 해상도
        /// </summary>
        public string Resolution = "Full";

        /// <summary>
        /// 대비정의
        /// </summary>
        public string Definition = "";
    }


    /// <summary>
    /// 클래스  명: FactorialDesign<br/>
    /// 클래스요약: 요인분석의 디자인 타입에 따른 직교배열을 얻는다.<br/>
    /// 작  성  자: MiracomInc<br/>
    /// 최초작성일: 2005-08-01<br/>
    /// 최종수정자: MiracomInc<br/>
    /// 최종수정일: 2005-12-31<br/>
    /// 상세  설명: 요인분석의 디자인 타입에 따른 직교배열을 얻는다.<br/>
    /// 변경  내용: <br/>
    /// </summary>
    public class FactorialDesign
    {
        #region  반복수 : Repeat 

        private static int iRepeat = 1;

        /// <summary>
        /// 반복수
        /// </summary>
        public static int Repeat
        {
            set
            {
                if (value < 1)
                    FactorialDesign.iRepeat = 1;
                else
                    FactorialDesign.iRepeat = value;
            }

            get
            {
                return FactorialDesign.iRepeat;
            }
        }

        #endregion

        #region  반응변수의 수 : NumberOfResponse 

        private static int iNumberOfResponse = 1;

        /// <summary>
        /// 반응변수의 수
        /// </summary>
        public static int NumberOfResponse
        {
            set
            {
                if (value < 1)
                {
                    FactorialDesign.iNumberOfResponse = 1;
                }
                else
                {
                    FactorialDesign.iNumberOfResponse = value;
                }
            }

            get
            {
                return FactorialDesign.iNumberOfResponse;
            }
        }

        #endregion

        #region  실험순서 : ExcuteOrder 

        /// <summary>
        /// 실험순서
        /// </summary>
        public static eExcuteOrder ExcuteOrder = eExcuteOrder.Standard;

        #endregion

        #region  디자인 타입 : DesignType 

        /// <summary>
        /// 디자인 타입
        /// </summary>
        private static eDesignType designType = eDesignType.DesignType_2_2_Full;

        public static eDesignType DesignType
        {
            set
            {
                FactorialDesign.designType = value;

                string strType = FactorialDesign.designType.ToString();

                for (int i = 0; i < FactorialDesign.strDefinition.GetLength(0); i++)
                {
                    if (strType == FactorialDesign.strDefinition[i, 0])
                    {
                        FactorialDesign.definitionItem.NumberOfExperiments = FactorialDesign.strDefinition[i, 1];
                        FactorialDesign.definitionItem.Title = FactorialDesign.strDefinition[i, 2];
                        FactorialDesign.definitionItem.Resolution = FactorialDesign.strDefinition[i, 3];
                        FactorialDesign.definitionItem.Definition = FactorialDesign.strDefinition[i, 4];

                        break;
                    }
                }
            }

            get
            {
                return FactorialDesign.designType;
            }
        }

        #endregion

        #region  정의대비 : FactorialItem 

        private static DefinitionItem definitionItem = new DefinitionItem();

        /// <summary>
        /// 디자인타입의 정보
        /// </summary>
        public static DefinitionItem FactorialItem
        {
            get
            {
                return FactorialDesign.definitionItem;
            }
        }

        #endregion

        #region 직교배열 인자배치
        private static string[,] strDefinition = new string[,]{		
			{"DesignType_2_4" ,			"L4(2**3)"  ,			"a;b;ab","a;b;ab" },
			{"DesignType_2_8" ,			"L8(2**7)"  ,			"a;b;ab;c;ac;bc;abc","a;b;ab;c;ac;bc;abc" },
			{"DesignType_2_16",			"L16(2**15)",			"a;b;ab;c;ac;bc;abc;d;ad;bd;abd;cd;acd;bcd;abcd","a;b;ab;c;ac;bc;abc;d;ad;bd;abd;cd;acd;bcd;abcd" },
			{"DesignType_2_32",			"L32(2**31)",			"a;b;ab;c;ac;bc;abc;d;ad;bd;abd;cd;acd;bcd;abcd;e;ae;be;abe;ce;ace;bce;abce;de;ade;bde;abde;cde;acde;bcde;abcde","a;b;ab;c;ac;bc;abc;d;ad;bd;abd;cd;acd;bcd;abcd;e;ae;be;abe;ce;ace;bce;abce;de;ade;bde;abde;cde;acde;bcde;abcde" },
			{"DesignType_2_64",			"L64(2**63)",			"a;b;ab;c;ac;bc;abc;d;ad;bd;abd;cd;acd;bcd;abcd;e;ae;be;abe;ce;ace;bce;abce;de;ade;bde;abde;cde;acde;bcde;abcde;f;af;bf;abf;cf;acf;bcf;abcf;df;adf;bdf;abdf;cdf;acdf;bcdf;abcdf;ef;aef;bef;abef;cef;acef;bcef;abcef;def;adef;bdef;abdef;cdef;acdef;bcdef;abcdef" ,"a;b;ab;c;ac;bc;abc;d;ad;bd;abd;cd;acd;bcd;abcd;e;ae;be;abe;ce;ace;bce;abce;de;ade;bde;abde;cde;acde;bcde;abcde;f;af;bf;abf;cf;acf;bcf;abcf;df;adf;bdf;abdf;cdf;acdf;bcdf;abcdf;ef;aef;bef;abef;cef;acef;bcef;abcef;def;adef;bdef;abdef;cdef;acdef;bcdef;abcdef" },	
			
			{"DesignType_3_9" ,			"L9(3**4)"  ,			"a;b;ab;aab","a;b;ab;ab^2" },
			{"DesignType_3_27",			"L27(3**13)",			"a;b;ab;aab;c;ac;aac;bc;abc;aabc;bbc;aabcc;aabbc","a;b;ab;ab^2;c;ac;ac^2;bc;abc;ab^2c^2;bc^2;ab^2c;abc^2" },
			{"DesignType_3_81",			"L81(3**40)",			"a;b;ab;aab;c;ac;aac;bc;abc;aabc;bbc;aabcc;aabbc;d;ad;aad;bd;abd;aabd;bbd;aabdd;aabbd;cd;acd;aacd;bcd;abcd;aabcd;bbcd;aabccdd;aabcdd;ccd;aacdd;aaccd;bbcdd;aabbccd;aabbcd;bbccd;aabccd;aabbcdd" ,"a;b;ab;ab^2;c;ac;ac^2;bc;abc;ab^2c^2;bc^2;ab^2c;abc^2;d;ad;ad^2;bd;abd;ab^2d^2;bd^2;ab^2d;abd^2;cd;acd;ac^2d^2;bcd;abcd;ab^2c^2d^2;bc^2d^2;ab^2cd;ab^2c^2d;cd^2;ac^2d;acd^2;bc^2d;abcd^2;abc^2d^2;bcd^2;ab^2cd^2;abc^2d"	},

			{"DesignType_4_16",			"L16(4**5)"  ,			"a;b;ab;c;ac","a;b;ab;c;ac" },

			{"DesignType_5_25",			"L25(5**6)",			"a;b;ab;c;ac;bc;","a;b;ab;c;ac;bc;" }
															   };
        #endregion

        #region 정의대비값
        private static string[,] strDefinitionFactorial = new string[,] {
             // 타입,                   실험수,    제목,         해상도,  정의대비
			{"DesignType_2_2_Full",		"4",	"2**(2)",		"Full",	"" },

			{"DesignType_2_3_Full",		"8",	"2**(3)",		"Full",	"" },
			{"DesignType_2_3_Minus1",	"4",	"2**(3-1)",		"III",	"C=AB" },

			{"DesignType_2_4_Full",		"16",	"2**(4)",		"Full",	"" },
			{"DesignType_2_4_Minus1",	"8",	"2**(4-1)",		"IV",	"D=ABC" },

			{"DesignType_2_5_Full",		"32",	"2**(5)",		"Full",	"" },
			{"DesignType_2_5_Minus1",	"16",	"2**(5-1)",		"V",	"E=ABCD" },
			{"DesignType_2_5_Minus2",	"8",	"2**(5-2)",		"III",	"D=AB;E=AC" },

			{"DesignType_2_6_Full",		"64",	"2**(6)",		"Full",	"" },
			{"DesignType_2_6_Minus1",	"32",	"2**(6-1)",		"VI",	"F=ABCDE" },
			{"DesignType_2_6_Minus2",	"16",	"2**(6-2)",		"IV",	"E=ABC;F=BCD" },
			{"DesignType_2_6_Minus3",	"8",	"2**(6-3)",		"III",	"D=AB;E=AC;F=BC" },

			{"DesignType_2_7_Full",		"128",	"2**(7)",		"Full",	"" },
			{"DesignType_2_7_Minus1",	"64",	"2**(7-1)",		"VII",	"G=ABCDEF" },
			{"DesignType_2_7_Minus2",	"32",	"2**(7-2)",		"IV",	"F=ABCD;G=ABDE" },
			{"DesignType_2_7_Minus3",	"16",	"2**(7-3)",		"IV",	"E=ABC;F=BCD;G=ACD" },
			{"DesignType_2_7_Minus4",	"8",	"2**(7-4)",		"III",	"D=AB;E=AC;F=BC;G=ABC" },

			{"DesignType_2_8_Minus1",	"128",	"2**(8-1)",		"VIII",	"H=ABCDEFG" },
			{"DesignType_2_8_Minus2",	"64",	"2**(8-2)",		"V",	"G=ABCD;H=ABEF" },
			{"DesignType_2_8_Minus3",	"32",	"2**(8-3)",		"IV",	"F=ABC;G=ABD;H=BCDE" },
			{"DesignType_2_8_Minus4",	"16",	"2**(8-4)",		"IV",	"E=BCD;F=ACD;G=ABC;H=ABD" },

			{"DesignType_2_9_Minus2",	"128",	"2**(9-2)",		"VI",	"H=ACDFG;J=BCEFG" },
			{"DesignType_2_9_Minus3",	"64",	"2**(9-3)",		"IV",	"G=ABCD;H=ACEF;J=CDEF" },
			{"DesignType_2_9_Minus4",	"32",	"2**(9-4)",		"IV",	"F=BCDE;G=ACDE;H=ABDE;J=ABCE" },
			{"DesignType_2_9_Minus5",	"16",	"2**(9-5)",		"III",	"E=ABC;F=BCD;G=ACD;H=ABD;J=ABCD" },

			{"DesignType_2_10_Minus3",	"128",	"2**(10-3)",	"V",	"H=ABCG;J=BCDE;K=ACDF" },
			{"DesignType_2_10_Minus4",	"64",	"2**(10-4)",	"IV",	"G=BCDF;H=ACDF;J=ABDE;K=ABCE" },
			{"DesignType_2_10_Minus5",	"32",	"2**(10-5)",	"IV",	"F=ABCD;G=ABCE;H=ABDE;J=ACDE;K=BCDE" },
			{"DesignType_2_10_Minus6",	"16",	"2**(10-6)",	"III",	"E=ABC;F=BCD;G=ACD;H=ABD;J=ABCD;K=AB" },

			{"DesignType_2_11_Minus4",	"128",	"2**(11-4)",	"V",	"H=ABCG;J=BCDE;K=ACDF;L=ABCDEFG" },
			{"DesignType_2_11_Minus5",	"64",	"2**(11-5)",	"IV",	"G=CDE;H=ABCD;J=ABF;K=BDEF;L=ADEF" },
			{"DesignType_2_11_Minus6",	"32",	"2**(11-6)",	"IV",	"F=ABC;G=BCD;H=CDE;J=ACD;K=ADE;L=BDE" },
			{"DesignType_2_11_Minus7",	"16",	"2**(11-7)",	"III",	"E=ABC;F=BCD;G=ACD;H=ABD;J=ABCD;K=AB;L=AC" },

			{"DesignType_2_12_Minus5",	"128",	"2**(12-5)",	"IV",	"H=ACDG;J=ABCD;K=BCFG;L=ABDEFG;M=CDEF" },
			{"DesignType_2_12_Minus6",	"64",	"2**(12-6)",	"IV",	"G=DEF;H=ABC;J=BCDE;K=BCDF;L=ABEF;M=ACEF" },
			{"DesignType_2_12_Minus7",	"32",	"2**(12-7)",	"IV",	"F=ACE;G=ACD;H=ABD;J=ABE;K=CDE;L=ABCDE;M=ADE" },
			{"DesignType_2_12_Minus8",	"16",	"2**(12-8)",	"III",	"E=ABC;F=ABD;G=ACD;H=BCD;J=ABCD;K=AB;L=AC;M=AD" },

			{"DesignType_2_13_Minus6",	"128",	"2**(13-6)",	"IV",	"H=DEFG;J=BCEG;K=BCDFG;L=ABDEF;M=ACEF;N=ABC" },
			{"DesignType_2_13_Minus7",	"64",	"2**(13-7)",	"IV",	"G=ABC;H=DEF;J=BCDF;K=BCDE;L=ABEF;M=ACEF;N=BCEF" },
			{"DesignType_2_13_Minus8",	"32",	"2**(13-8)",	"IV",	"F=ACE;G=BCE;H=ABC;J=CDE;K=ABCDE;L=ABE;M=ACD;N=ADE" },
			{"DesignType_2_13_Minus9",	"16",	"2**(13-9)",	"III",	"E=ABC;F=ABD;G=ACD;H=BCD;J=ABCD;K=AB;L=AC;M=AD;N=BC" },

			{"DesignType_2_14_Minus7",	"128",	"2**(14-7)",	"IV",	"H=EFG;J=BCFG;K=BCEG;L=ABEF;M=ACEF;N=BCDEF;O=ABC" },
			{"DesignType_2_14_Minus8",	"64",	"2**(14-8)",	"IV",	"G=BEF;H=BCF;J=DEF;K=CEF;L=BCE;M=CDF;N=ABCD;O=BCDEF" },
			{"DesignType_2_14_Minus9",	"32",	"2**(14-9)",	"IV",	"F=ABC;G=ABD;H=ABE;J=ACD;K=ACE;L=ADE;M=BCD;N=BCE;O=BDE" },
			{"DesignType_2_14_Minus10",	"16",	"2**(14-10)",	"III",	"E=ABC;F=ABD;G=ACD;H=BCD;J=ABCD;K=AB;L=AC;M=AD;N=BC;O=BD" },

			{"DesignType_2_15_Minus8",	"128",	"2**(15-8)",	"IV",	"H=ABFG;J=ACDEF;K=BEF;L=ABCEG;M=CDFG;N=ACDEF;O=EFG;P=ABDEFG" },
			{"DesignType_2_15_Minus9",	"64",	"2**(15-9)",	"IV",	"G=ABC;H=ABD;J=ABE;K=ABF;L=ACD;M=ACE;N=ACF;O=ADE;P=ADF" },
			{"DesignType_2_15_Minus10",	"32",	"2**(15-10)",	"IV",	"F=ABC;G=ABD;H=ABE;J=ACD;K=ACE;L=ADE;M=BCD;N=BCE;O=BDE;P=CDE" },
			{"DesignType_2_15_Minus11",	"16",	"2**(15-11)",	"III",	"E=ABC;F=ABD;G=ACD;H=BCD;J=ABCD;K=AB;L=AC;M=AD;N=BC;O=BD;P=CD" },

			{"DesignType_3_2_Full",		"9",	"3**(2)",		"Full",	"" },
			{"DesignType_3_3_Full",		"27",	"3**(3)",		"Full",	"" },
			{"DesignType_3_4_Full",		"81",	"3**(4)",		"Full",	"" },
			{"DesignType_3_5_Full",		"243",	"3**(5)",		"Full",	"" }
																 };

        #endregion



        /// <summary>
        /// 요인분석(Factorial Analysis)의 직교배열을 얻는다.
        /// </summary>
        /// <returns>요인분석(Factorial Analysis)의 직교배열</returns>
        public static int[,] GetDefinitionArray()
        {
            // 분석타입의 타이틀
            string strTitle = FactorialDesign.definitionItem.Title;

            // 타이틀에서 타입의 요소 파악
            int iFactor = Convert.ToInt32(strTitle.Substring(0, 1));
            int iVariance;
            int iDefinition;

            int iPos1 = strTitle.IndexOf("(");
            int iPos2 = strTitle.IndexOf(")");
            string strTemp = strTitle.Substring(iPos1 + 1, (iPos2 - iPos1 - 1));

            // Full 모델인지 체크
            int iPos3 = strTitle.IndexOf("-");

            if (iPos3 == -1)
            {
                iVariance = Convert.ToInt32(strTemp);
                iDefinition = 0;
            }
            else
            {
                iVariance = Convert.ToInt32(strTitle.Substring(iPos1 + 1, (iPos3 - iPos1 - 1)));
                iDefinition = Convert.ToInt32(strTitle.Substring(iPos3 + 1, (iPos2 - iPos3 - 1)));
            }

            // 실험횟수와 정의대비 파악
            int iRows = Convert.ToInt32(FactorialDesign.definitionItem.NumberOfExperiments);
            string[] strDefi = null;

            if (FactorialDesign.definitionItem.Definition != "")
            {
                strDefi = FactorialDesign.definitionItem.Definition.Split(';');
            }

            // 디자인배열 생성
            int[,] strDesign = new int[iRows * FactorialDesign.iRepeat, iVariance + 1];

            // 행과 열의 위치
            int posRow;
            int posCol;

            // 계산에 필요한 변수들
            int iDevideNumber;
            int iQuotient;
            int iRemainder;

            // 반복
            for (int i = 0; i < FactorialDesign.iRepeat; i++)
            {
                // 규칙적인 값을 생성하는 부분
                for (int j = 1; j <= (iVariance - iDefinition); j++)
                {
                    posCol = j;

                    iDevideNumber = Convert.ToInt32(Math.Pow(iFactor, j - 1));

                    for (int k = 0; k < iRows; k++)
                    {
                        posRow = k + (i * iRows);

                        iQuotient = Convert.ToInt32(k / iDevideNumber);
                        iRemainder = iQuotient % iFactor;

                        strDesign[posRow, posCol] = iRemainder;
                    }

                }

                // 정의대비 식
                string strFormula = string.Empty;
                string strChar = string.Empty;
                int iColPos;
                int iValue;

                // 정의대비를 이용한 값을 생성하는 부분
                for (int j = 0; j < iDefinition; j++)
                {
                    posCol = (iVariance - iDefinition) + (j + 1);

                    // 정의대비를 구한다.
                    strFormula = strDefi[j];

                    for (int k = 0; k < iRows; k++)
                    {
                        posRow = k + (i * iRows);

                        //정의대비 계산
                        iValue = 1;

                        for (int l = 2; l < strFormula.Length; l++)
                        {
                            strChar = strFormula.Substring(l, 1);
                            iColPos = FactorialDesign.GetPosition(strChar);

                            if (strDesign[posRow, iColPos] == 1)
                            {
                                iValue *= 1;
                            }
                            else
                            {
                                iValue *= -1;
                            }

                            if (iValue == 1)
                            {
                                strDesign[posRow, posCol] = 1;
                            }
                            else
                            {
                                strDesign[posRow, posCol] = 0;
                            }
                        }

                    }
                }
            }

            // 실행순서
            if (ExcuteOrder == eExcuteOrder.Standard)	// 순차
            {
                for (int i = 0; i < strDesign.GetLength(0); i++)
                {
                    strDesign[i, 0] = i + 1;
                }
            }
            else										// 랜덤
            {
                // 랜덤숫자 생성
                Random MyRandom = new Random(unchecked((int)DateTime.Now.Ticks));
                int iRandomPosition;
                int iIndex;

                for (int i = iRows * FactorialDesign.iRepeat; i >= 1; i--)
                {
                    iRandomPosition = MyRandom.Next(0, i - 1);
                    iIndex = 0;

                    for (int j = 0; j <= iRandomPosition; j++)
                    {
                        if (j > 0)
                        {
                            iIndex++;
                        }

                        while (true)
                        {
                            if (strDesign[iIndex, 0] != 0)
                            {
                                iIndex++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    strDesign[iIndex, 0] = i;
                }
            }

            return strDesign;
        }



        private static string[] GetOthDefinition(string strType)
        {
            for (int i = 0; i < FactorialDesign.strDefinition.GetLength(0); i++)
            {
                if (strType == FactorialDesign.strDefinition[i, 0])
                {
                    return new string[] { FactorialDesign.strDefinition[i, 0], FactorialDesign.strDefinition[i, 1], FactorialDesign.strDefinition[i, 2] };
                }
            }
            return null;
        }

        /// <summary>
        /// 요인분석(Factorial Analysis)의 직교배열을 얻는다.
        /// </summary>
        /// <returns>요인분석(Factorial Analysis)의 직교배열</returns>
        public static int[,] GetDefinitionArray(int iLevel, int iFactor, int iRun, int iRepeat = 1)
        {
            #region 알고리즘을 구성할 기본 정보 생성

            string strKey = string.Format("DesignType_{0}_{1}", iLevel, iRun);
            string[] strDefineOth = GetOthDefinition(strKey);
            string strDefine = strDefineOth[1];


            string[] strDefi = null;

            int iPos0 = strDefine.IndexOf("L");
            int iPos1 = strDefine.IndexOf("(");
            int iPos2 = strDefine.IndexOf(")");
            int iPos3 = strDefine.IndexOf("**");

            string strCol = strDefine.Substring(iPos3 + 2, (iPos2 - (iPos3 + 2)));
            string strRow = strDefine.Substring(iPos0 + 1, iPos1 - 1);
            string strType = strDefine.Substring(iPos1 + 1, 1);

            //직교배열표의 열의 수
            int iColumns = Convert.ToInt32(iFactor);

            //직교배열표의 행의 수(실험수)
            int iRows = Convert.ToInt32(strRow);

            //직교배열표의 수준계
            int iType = Convert.ToInt32(strType);

            strDefi = strDefineOth[2].Split(';');

            #endregion

            #region 알고리즘 구성


            int defaultFactorCount = 0;

            for (int p = 0; p < strDefi.Length; p++)
            {
                if (strDefi[p].Length == 1)
                    defaultFactorCount++;
            }


            // 디자인배열 생성
            int[,] strDesign = new int[iRows, defaultFactorCount + 1];

            // 행과 열의 위치
            int posRow;
            int posCol;

            // 계산에 필요한 변수들
            int iDevideNumber;
            int iQuotient;
            int iRemainder;

            //반복수
            //int i =0;

            int count1 = 1;

            // 규칙적인 값을 생성하는 부분
            //for(int j=1; j<= defaultFactorCount; j++)
            for (int j = defaultFactorCount; j >= 1; j--)
            {
                posCol = count1;

                iDevideNumber = Convert.ToInt32(Math.Pow(iType, j - 1));

                for (int k = 0; k < iRows; k++)
                {
                    posRow = k;

                    iQuotient = Convert.ToInt32(k / iDevideNumber);

                    iRemainder = iQuotient % iType;

                    strDesign[posRow, posCol] = iRemainder;

                }

                count1++;

            }

            count1 = 1;



            // 디자인배열 생성
            int[,] resultDesignArray = new int[iRows, iColumns + 1];


            try
            {
                int count = 0;

                // 규칙적인 값을 생성하는 부분
                for (int j = 1; j <= iColumns; j++)
                {

                    posCol = j;

                    for (int k = 0; k < iRows; k++)
                    {
                        posRow = k;

                        int temp = 0;

                        //if (count == strDefi.Length) count = 0;
                        string str = strDefi[count];

                        for (int t = 0; t < str.Length; t++)
                        {
                            int innerIndex = GetPosition(str.Substring(t, 1));

                            temp += strDesign[posRow, innerIndex];

                        }
                        resultDesignArray[posRow, posCol] = (temp % iType);
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion

            #region 실험순서 생성

            if (ExcuteOrder == eExcuteOrder.Standard)	// 순차
            {
                for (int i = 0; i < resultDesignArray.GetLength(0); i++)
                {
                    resultDesignArray[i, 0] = i + 1;
                }
            }
            else										// 랜덤
            {
                // 랜덤숫자 생성
                Random MyRandom = new Random(unchecked((int)DateTime.Now.Ticks));
                int iRandomPosition;
                int iIndex;

                for (int i = iRows; i >= 1; i--)
                {
                    iRandomPosition = MyRandom.Next(0, i - 1);
                    iIndex = 0;

                    for (int j = 0; j <= iRandomPosition; j++)
                    {
                        if (j > 0)
                        {
                            iIndex++;
                        }

                        while (true)
                        {
                            if (resultDesignArray[iIndex, 0] != 0)
                            {
                                iIndex++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    resultDesignArray[iIndex, 0] = i;
                }
            }

            #endregion

            return resultDesignArray;
        }



        // 알파벳을 숫자로 치환시키준다.
        private static int GetPosition(string strTemp)
        {
            int iIndex;

            switch (strTemp.ToUpper())
            {
                case "A":
                    iIndex = 1;
                    break;

                case "B":
                    iIndex = 2;
                    break;

                case "C":
                    iIndex = 3;
                    break;

                case "D":
                    iIndex = 4;
                    break;

                case "E":
                    iIndex = 5;
                    break;

                case "F":
                    iIndex = 6;
                    break;

                case "G":
                    iIndex = 7;
                    break;

                default:
                    throw new Exception("잘못된 정의대비문자");
            }

            return iIndex;
        }

    }
}
