using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using CenterSpace.NMath.Stats;
using CenterSpace.NMath.Matrix;
using CenterSpace.NMath.Core;

namespace DACrux.BStats
{
    /// <summary>
    /// 클래스  명: AnalysisFactorial<br/>
    /// 클래스요약: 요인분석의 데이터 분석 기능을 구현한 모듈.<br/>
    /// 작  성  자: MiracomInc<br/>
    /// 최초작성일: 2005-11-25<br/>
    /// 최종수정자: MiracomInc<br/>
    /// 최종수정일: 2005-11-25<br/>
    /// 상세  설명: 이수준 요인분석 기능을 구현하였다.<br/>
    /// 변경  내용: <br/>
    /// </summary>
    /// <remarks>
    /// Copyright 2005 MiracomInc All rights reserved.
    /// </remarks>
    public class AnalysisFactorial
    {
        /// <summary>
        /// 출력 결과
        /// </summary>
        private DataSet dsOutput = null; // 결과

        /// <summary>
        /// 요인분석의 입력 프로퍼티
        /// </summary>
        private DACrux.BStats.StatisticsInput.inputFactorAnalysis input = null; // Dlg 에 받을  분산분석 구조체

        /// <summary>
        /// 클래스 내에서 사용되는 데이터 테이블
        /// </summary>
        private DataTable dataTable = null;

        /// <summary>
        /// 초기값으로 받아들이는 데이터 프레임
        /// </summary>
        private DataFrame dfOrgin = null;

        /// <summary>
        /// 사용되지 않는 컬럼 삭제와 결측치 처리등의 처리를 하고 난후 데이터 프레임
        /// </summary>
        private DataFrame dfNew = null;

        /// <summary>
        /// 독립변수 인덱스 배열
        /// </summary>
        private int[] analysisParamsVarIndex = null;

        /// <summary>
        /// 분석변수 인덱스 배열
        /// </summary>
        private int[] analysisVarIndex = null;

        /// <summary>
        /// 데이터 컬럼 배열
        /// </summary>
        private DFColumn[] cols = null;

        /// <summary>
        /// 데이터 컬럼내 그룹간 인자 배열리스트
        /// </summary>
        private ArrayList[] alGroupFactors = null;

        /// <summary>
        /// 독립변수 배열리스트
        /// </summary>
        private ArrayList[] alIndepFactors = null;

        /// <summary>
        /// 풀링할 파라미터 리스트
        /// </summary>
        private ArrayList pooledParams = null;

        /// <summary>
        /// 풀링하지 않을 파라미터 리스트
        /// </summary>
        private ArrayList unPooledParams = null;

        /// <summary>
        /// 종속변수 배열
        /// </summary>
        private double[] dblValue = null;

        /// <summary>
        /// 교락 초기값
        /// </summary>
        private int indexOfConfoundCol = -1;


        /// <summary>
        /// 이인자 교호작용 수
        /// </summary>
        int TwoFactorInteraction = 0;

        /// <summary>
        /// 삼인자 교호작용 수
        /// </summary>
        int ThreeFactorInteraction = 0;

        /// <summary>
        /// 교호작용 카운트
        /// </summary>
        int interactionCount = 0;

        /// <summary>
        /// SST
        /// </summary>
        double TotalSumofSqures = 0.0;

        /// <summary>
        /// SSE
        /// </summary>
        double errorSumOfSqures = 0.0;

        /// <summary>
        /// Error의 자유도
        /// </summary>
        int errorDF = 0;

        /// <summary>
        /// 클래스내에서 사용되는 리스트
        /// </summary>
        ArrayList alCombi = null;

        /// <summary>
        /// 컴비네이션 클래스
        /// </summary>
        Combination combi = null;

        /// <summary>
        /// 정의값
        /// </summary>
        string defineNames = string.Empty;


        //string sAnalysisofVariance = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_TAB01");
        //string sModelInformation = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_TAB02");
        //string sMainEffect = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_TAB03");
        //string sInteractionEffect = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_TAB04");
        //string sSource = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL01");
        //string sDF = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL02");
        //string sSumofSquares = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL03");
        //string sMeanSquare = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL04");
        //string sFValue = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL05");
        //string sPValue = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL06");
        //string sRSquare = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL07");
        //string sAdjRSquare = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL08");
        //string sMSE = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL09");
        //string sFactor = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL10");

        //string sAnalysisofVariance = "AnalysisofVariance";
        //string sModelInformation = "ModelInformation";
        string sMainEffect = "MainEffect";
        string sInteractionEffect = "InteractionEffect";
        string sSource = "Source";
        string sDF = "DF";
        string sSumofSquares = "SumofSquares";
        string sMeanSquare = "MeanSquare";
        string sFValue = "FValue";
        string sPValue = "PValue";
        string sRSquare = "RSquare";
        string sAdjRSquare = "AdjRSquare";
        string sMSE = "MSE";
        string sFactor = "Factor";

        //string	sMainEffect                    = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL11"); 
        //string	sFactor                        = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL12"); 
        //string	sInteractionEffect             = Miracom.DACrux.Interface.ILanguages.getResourceString("STAT_DOE_FACTANAL_RESULT_LBL13"); 




        //int interactionIndex = -1;

        //결측치를 제거하기전 컬럼의 크기
        //private int columnLength           = 0;

        /// <summary>
        /// AnalysisFactorial의 생성자
        /// </summary>
        public AnalysisFactorial()
        {
            dsOutput = new DataSet("Anova Analysis");
        }

        /// <summary>
        /// AnalysisFactorial의 생성자
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="input"></param>
        public AnalysisFactorial(DataTable dataTable, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {
            this.dataTable = dataTable;
            this.input = input;
            this.dfOrgin = new DataFrame(dataTable);
            this.dsOutput = new DataSet("Descriptive Statistics");

            this.analysisParamsVarIndex = new int[input.analysisParams.Count];     // 독립변수
            this.analysisVarIndex = new int[input.analysisVariables.Count];	 // 종속변수		
            this.cols = new DFColumn[input.analysisVariables.Count + input.analysisParams.Count];

            this.defineNames += this.input.cols + "-" + this.input.rows;

            removeNotSeletedColumn();

            // ClassVariables를 셋팅한다.
            for (int i = 0; i < input.analysisParams.Count; i++)
            {
                int index = getIndex(input.analysisParams[i].ToString());
                analysisParamsVarIndex[i] = index;
            }

            // AnalysisVariables를 셋팅한다.
            for (int j = 0; j < input.analysisVariables.Count; j++)
            {
                int index = getIndex(input.analysisVariables[j].ToString());
                analysisVarIndex[j] = index;
            }
        }

        #region 사용하지 않는 변수 제거

        /// <summary>
        /// 독립변수와 종속변수로 선택된 컬럼이외의 컬럼은 제거한다.
        /// </summary>
        private void removeNotSeletedColumn()
        {

            int count = this.dfOrgin.Cols;

            int[] a = new int[this.dfOrgin.Cols];

            for (int k = 0; k < a.Length; k++)
            {
                a[k] = k;
            }

            // ClassVariables를 셋팅한다.
            for (int i = 0; i < input.analysisParams.Count; i++)
            {
                int index = getIndex(input.analysisParams[i].ToString());
                a[index] = -1;
            }
            // AnalysisVariables를 셋팅한다.
            for (int j = 0; j < input.analysisVariables.Count; j++)
            {
                int index = getIndex(input.analysisVariables[j].ToString());
                a[index] = -1;
            }

            int step2 = a.Length - 1;

            // 선택되지 않은 컬럼을 제거한다.

            while (step2 >= 0)
            {
                if (a[step2] > -1)
                {
                    this.dfOrgin.RemoveColumn(a[step2]);
                }

                step2--;
            }



        }

        #endregion

        #region 컬럼의 인덱스를 돌려준다.

        /// <summary>
        /// 컬럼명으로 해당 컬럼의 인덱스를 돌려준다.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int getIndex(string s)
        {
            return this.dfOrgin.IndexOfColumn(s);
        }

        private int getIndex(DataSet ds, string s)
        {
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                if (ds.Tables[0].Columns[i].ColumnName == s)
                    return i;
            }
            return -1;
        }

        private int getIndex(DataTable table, string s)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].ColumnName == s)
                    return i;
            }
            return -1;
        }

        private int getIndex(DataFrame df, string s)
        {
            return df.IndexOfColumn(s);
        }


        #endregion

        #region 초기화

        /// <summary>
        /// GetFactorialAnalysis 클래스의 생성자
        /// </summary>
        /// <returns></returns>
        public DataSet GetFactorialAnalysis()
        {
            dfNew = new DataFrame();

            string[] strFactor1 = null;			// 부품 변수의 내용			

            DFColumn[] columns = new DFColumn[dfOrgin.Cols];
            ArrayList[] alColumns = new ArrayList[dfOrgin.Cols];


            for (int i = 0; i < dfOrgin.Cols; i++)
            {
                columns[i] = (DFColumn)dfOrgin[i];
            }

            for (int i = 0; i < dfOrgin.Cols; i++)
                alColumns[i] = new ArrayList(columns[i].ToArray());

            int tempCount = columns[0].Count;

            for (int i = (tempCount - 1); i >= 0; i--)
            {
                bool missing = false;

                for (int k = 0; k < alColumns.Length; k++)
                {
                    if (alColumns[k][i].Equals(null) || alColumns[k][i].Equals(".") || Convert.ToInt32(alColumns[k][i]) == int.MinValue)
                        missing = true;
                }

                if (missing)
                {
                    for (int k = 0; k < alColumns.Length; k++)
                        alColumns[k].RemoveAt(i);
                }
            }


            for (int i = 0; i < this.analysisVarIndex.Length; i++)
            {
                //Object[] array = alColumns[this.analysisVarIndex[i] ].ToArray();
                Object[] array = alColumns[this.getIndex(dfOrgin, this.input.analysisVariables[i].ToString())].ToArray();

                dblValue = new Double[array.Length];

                for (int k = 0; k < array.Length; k++)
                    dblValue[k] = Convert.ToDouble(array[k]);

                CenterSpace.NMath.Stats.DFNumericColumn dfColValue = new DFNumericColumn(input.analysisVariables[i].ToString(), dblValue);
                dfNew.AddColumn(dfColValue);
            }

            //Console.Out.WriteLine("=============================================================");
            for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
            {
                //strFactor1 = (string[])alColumns[this.analysisParamsVarIndex[i] ].ToArray(System.Type.GetType("System.String"));

                object[] objFactor1 = alColumns[this.getIndex(dfOrgin, this.input.analysisParams[i].ToString())].ToArray();
                //bject [] objFactor1 = alColumns[this.analysisParamsVarIndex[i] ].ToArray();

                strFactor1 = new string[objFactor1.Length];

                for (int j = 0; j < strFactor1.Length; j++)
                    strFactor1[j] = objFactor1[j].ToString();

                CenterSpace.NMath.Stats.DFStringColumn dfColValue = new DFStringColumn(input.analysisParams[i].ToString(), strFactor1);
                dfNew.AddColumn(dfColValue);

                dfColValue = null;
            }
            //Console.Out.WriteLine("=============================================================");


            for (int i = 0; i < dfNew.Cols; i++)
            {
                columns[i] = (DFColumn)dfNew[i];
            }

            this.setIndexParameters();


            // 독립변수의 수준을 파악하여 ArrayList 배열에 정리			

            this.alGroupFactors = new ArrayList[this.analysisParamsVarIndex.Length];
            for (int i = 0; i < alGroupFactors.Length; i++)
                this.alGroupFactors[i] = this.CountLevelOfVar(((DFColumn)dfNew[this.analysisParamsVarIndex[i]]).ToStringArray());

            this.alIndepFactors = new ArrayList[this.analysisParamsVarIndex.Length];
            for (int i = 0; i < alIndepFactors.Length; i++)
                this.alIndepFactors[i] = new ArrayList(((DFColumn)dfNew[this.analysisParamsVarIndex[i]]).ToArray());


            //			if(!this.isValidate(dfNew))
            //			{
            //				MessageBox.Show("결측치나 데이터 형태가 적절하지 않습니다.");
            //				return null;
            //			}
            //
            //			if(!this.isValidateTwoWayOrthogonalArrays(dfNew))
            //			{
            //				MessageBox.Show("2수준 직교배열표가 아닙니다.");
            //				return null;
            //			}
            //
            //			if(!this.isValidateOrthogonalArrays(dfNew))
            //			{
            //				MessageBox.Show("데이터의 형태가 직교배열표가 아닙니다.");
            //				return null;
            //			}


            this.Run(dfNew, input);

            return dsOutput;
        }


        #endregion

        /// <summary>
        /// 실행함수
        /// </summary>
        /// <param name="dfNew"></param>
        /// <param name="input"></param>
        private void Run(DataFrame dfNew, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {

            if (this.input.bInteractionEffect)
                this.makeInterActionColumns(dfNew, input);

            if (this.input.bPooling)
                this.removePoolingData(dfNew, input);

            if (this.input.bAnovaAnalysis)
            {
                if (this.input.factorType == 2)
                    this.getAnovaTable(dfNew, input);
                else
                    this.getThreeFactorAnovaTable(dfNew, input);
            }

            this.getAnovaTableForPooling(dfNew, input);


            if (this.input.bDescriptiveStatistics)
                this.getModelInfoTable(dfNew, input);

            if (this.input.bMainEffect)
                this.getMainEffectTable(dfNew, input);

            if (this.input.bInteractionEffect)
                this.getInteractionEffectTable(dfNew, input);
        }


        #region 교호작용이 있을 경우 해당 교호작용 계산을 위해 새로운 컬럼을 데이터프레임에 추가한다.

        /// <summary>
        /// 교호작용이 있을 경우 해당 교호작용 계산을 위해 새로운 컬럼을 데이터 프레임에 추가한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="input"></param>
        private void makeInterActionColumns(DataFrame df, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {
            if (this.input.bInteractionEffect)
            {
                if (this.analysisParamsVarIndex.Length < 2)
                    return;

                combi = new Combination(this.analysisParamsVarIndex.Length, 2);

                alCombi = combi.GenerateCombiation();

                //Console.Out.WriteLine(alCombi.Count);

                for (int i = 0; i < alCombi.Count; i++)
                {
                    string str = alCombi[i].ToString();

                    int cols1 = this.analysisParamsVarIndex[Convert.ToInt32(str.Substring(0, 1))];
                    int cols2 = this.analysisParamsVarIndex[Convert.ToInt32(str.Substring(1, 1))];

                    if (cols1 != cols2)
                    {
                        if (this.input.factorType == 2)
                            this.makeInteractionColumn(df, cols1, cols2);
                        else
                            this.makeInteractionColumnForThird(df, cols1, cols2);
                    }
                }


                TwoFactorInteraction = alCombi.Count;



                // 만약 3인자 교호작용 이상이 나올 케이스의 경우는 아래와 같이 계산한다.
                // 설계 문서에 의해 4인자 이상의 교호작용은 무시하므로 생성하지 않는다.

                if (this.analysisParamsVarIndex.Length > 16)
                {
                    combi = new Combination(this.analysisParamsVarIndex.Length, 3);

                    alCombi = combi.GenerateCombiation();

                    ThreeFactorInteraction = alCombi.Count;

                    for (int i = 0; i < alCombi.Count; i++)
                    {
                        string str = alCombi[i].ToString();

                        int cols1 = this.analysisParamsVarIndex[Convert.ToInt32(str.Substring(0, 1))];
                        int cols2 = this.analysisParamsVarIndex[Convert.ToInt32(str.Substring(1, 1))];
                        int cols3 = this.analysisParamsVarIndex[Convert.ToInt32(str.Substring(2, 1))];

                        if (cols1 != cols2 && cols1 != cols3 && cols2 != cols3 && this.input.factorType == 2)
                            this.makeInteractionColumn(df, cols1, cols2, cols3);

                    }
                }

            }

        }

        #endregion

        #region 2수준 요인배치법의 Anova 테이블을 만든다.

        /// <summary>
        /// 2수준 요인배치법의 Anova 분석의 출력결과를 돌려준다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="input"></param>
        private void getAnovaTable(DataFrame df, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {

            DataTable anova = new DataTable("Analysis of Variance");

            TotalSumofSqures = this.getSumOfSqures((DFColumn)df[this.analysisVarIndex[0]]);

            double[] VarianceSumOfSqures = new double[this.analysisParamsVarIndex.Length];



            #region 각인자별 SumOfSqures를 계산한다.

            for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
                VarianceSumOfSqures[i] = this.getSumOfSqure_Variation(df, this.analysisParamsVarIndex[i]);

            #endregion

            #region 교호작용별 SumOfSqures를 계산한다.


            interactionCount = df.Cols - this.analysisParamsVarIndex.Length - this.analysisVarIndex.Length;

            double[] InteractionSumOfSqures = new double[interactionCount];


            if (this.input.bInteractionEffect)
            {
                Console.Out.WriteLine("교호작용별 SumOfSqures를 계산한다. length {0}", InteractionSumOfSqures.Length);


                for (int i = 0; i < interactionCount; i++)
                    InteractionSumOfSqures[i] = this.getSumOfSqure_Variation(df, this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i);
            }

            #endregion

            #region 오차의 자유도와 오차의 SumOfSqures를 계산한다 또한 P값을 계산한다.

            errorSumOfSqures = TotalSumofSqures - (StatsFunctions.NaNSum(VarianceSumOfSqures) + StatsFunctions.NaNSum(InteractionSumOfSqures));
            errorDF = dblValue.Length - VarianceSumOfSqures.Length - InteractionSumOfSqures.Length - 1;
            if (errorDF < 0) errorDF = 0;

            //Console.Out.WriteLine("errorSumOfSqures = {0}",errorSumOfSqures);

            //Console.Out.WriteLine("ErrorDF = {0}",errorDF);




            FDistribution fdist = null;

            if (errorDF > 0)
            {
                fdist = new FDistribution(1, errorDF);
            }

            #endregion

            try
            {
                anova.Columns.Add(this.sSource);
                anova.Columns.Add(this.sDF);
                anova.Columns.Add(this.sSumofSquares);
                anova.Columns.Add(this.sMeanSquare);
                anova.Columns.Add(this.sFValue);
                anova.Columns.Add(this.sPValue);

                object[] row = new object[6];

                for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
                {
                    // 요인 											

                    if (((DFColumn)df[this.analysisParamsVarIndex[i]]).Label.Equals(string.Empty))
                        row[0] = ((DFColumn)df[this.analysisParamsVarIndex[i]]).Name;
                    else
                        row[0] = ((DFColumn)df[this.analysisParamsVarIndex[i]]).Label;

                    row[1] = 1;					// 자유도

                    row[2] = Math.Round(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]), 4);			// 제곱합

                    row[3] = Math.Round(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]), 4);			// 평균제곱합

                    if (errorDF == 0.0 || errorSumOfSqures == 0)
                    {
                        row[4] = "*";
                        row[5] = "*";
                    }
                    else
                    {
                        row[4] = Math.Round(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]) / (errorSumOfSqures / errorDF), 4);					// F값
                        row[5] = 1 - Math.Round(fdist.CDF(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]) / (errorSumOfSqures / errorDF)), 4);				// P값
                    }

                    anova.Rows.Add(row);
                }


                for (int i = 0; i < interactionCount; i++)
                {
                    int index = this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i;
                    // 요인 
                    row[0] = ((DFColumn)df[index]).Name;						// 인자

                    if (((DFColumn)df[index]).Label.Equals(string.Empty))
                        row[0] = ((DFColumn)df[index]).Name;
                    else
                        row[0] = ((DFColumn)df[index]).Label;

                    row[1] = 1;					// 자유도

                    row[2] = Math.Round(this.getSumOfSqure_Variation(df, index), 4);			// 제곱합

                    row[3] = Math.Round(this.getSumOfSqure_Variation(df, index), 4);			// 평균제곱합

                    if (errorDF == 0.0 || errorSumOfSqures == 0)
                    {
                        row[4] = "*";
                        row[5] = "*";
                    }
                    else
                    {
                        row[4] = Math.Round(this.getSumOfSqure_Variation(df, index) / (errorSumOfSqures / errorDF), 4);					// F값
                        row[5] = 1 - Math.Round(fdist.CDF(this.getSumOfSqure_Variation(df, index) / (errorSumOfSqures / errorDF)), 4);				// P값
                    }

                    anova.Rows.Add(row);
                }


                if (errorDF > 0)
                {
                    // 오차 
                    row[0] = "Error";																// 오차

                    row[1] = errorDF;	// 자유도

                    row[2] = Math.Round(errorSumOfSqures, 4);													// 제곱합

                    if (errorSumOfSqures == 0)
                    {
                        row[3] = "*";											// 평균제곱합
                    }
                    else
                    {
                        row[3] = Math.Round(errorSumOfSqures / errorDF, 4);											// 평균제곱합
                    }

                    row[4] = "";																	// F값

                    row[5] = "";																	// P값

                    anova.Rows.Add(row);
                }

                // 전체 
                row[0] = "Total";																	// 전체

                row[1] = this.dblValue.Length - 1;													// 자유도

                row[2] = TotalSumofSqures;															// 제곱합

                row[3] = "";																	// 평균제곱합

                row[4] = "";																	// F값

                row[5] = "";																	// P값

                anova.Rows.Add(row);



                row = null;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                Console.Out.WriteLine(err);
                anova = null;

            }

            dsOutput.Tables.Add(anova);
            anova = null;


        }


        /// <summary>
        /// 2수준 요인배치법의 Anova분석결과를 통해 해당 기준에 유의하지 않은 변수를 오차항에 풀링시킨다음 해당 결과를 돌려준다.
        /// </summary>
        /// <param name="df"></param>
        /// <param naame="input"></param>
        private void getAnovaTableForPooling(DataFrame df, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {

            DataTable anova = new DataTable("Analysis of Variance For Pooling");

            TotalSumofSqures = this.getSumOfSqures((DFColumn)df[this.analysisVarIndex[0]]);

            double[] VarianceSumOfSqures = new double[this.analysisParamsVarIndex.Length];



            #region 각인자별 SumOfSqures를 계산한다.

            for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
                VarianceSumOfSqures[i] = this.getSumOfSqure_Variation(df, this.analysisParamsVarIndex[i]);

            #endregion

            #region 교호작용별 SumOfSqures를 계산한다.


            interactionCount = df.Cols - this.analysisParamsVarIndex.Length - this.analysisVarIndex.Length;

            double[] InteractionSumOfSqures = new double[interactionCount];


            if (this.input.bInteractionEffect)
            {
                Console.Out.WriteLine("교호작용별 SumOfSqures를 계산한다. length {0}", InteractionSumOfSqures.Length);


                for (int i = 0; i < interactionCount; i++)
                    InteractionSumOfSqures[i] = this.getSumOfSqure_Variation(df, this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i);
            }

            #endregion

            #region 오차의 자유도와 오차의 SumOfSqures를 계산한다 또한 P값을 계산한다.

            errorSumOfSqures = TotalSumofSqures - (StatsFunctions.NaNSum(VarianceSumOfSqures) + StatsFunctions.NaNSum(InteractionSumOfSqures));
            errorDF = dblValue.Length - VarianceSumOfSqures.Length - InteractionSumOfSqures.Length - 1;
            if (errorDF < 0) errorDF = 0;




            FDistribution fdist = null;

            if (errorDF > 0)
            {
                fdist = new FDistribution(1, errorDF);
            }

            #endregion

            try
            {
                anova.Columns.Add(this.sSource);
                anova.Columns.Add(this.sDF);
                anova.Columns.Add(this.sSumofSquares);
                anova.Columns.Add(this.sMeanSquare);
                anova.Columns.Add(this.sFValue);
                anova.Columns.Add(this.sPValue);

                object[] row = new object[6];

                for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
                {
                    // 요인 		








                    row[0] = ((DFColumn)df[this.analysisParamsVarIndex[i]]).Name;

                    row[1] = 1;					// 자유도

                    row[2] = Math.Round(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]), 4);			// 제곱합

                    row[3] = Math.Round(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]), 4);			// 평균제곱합

                    if (errorDF == 0.0 || errorSumOfSqures == 0)
                    {
                        row[4] = "*";
                        row[5] = "*";
                    }
                    else
                    {
                        row[4] = Math.Round(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]) / (errorSumOfSqures / errorDF), 4);					// F값
                        row[5] = 1 - Math.Round(fdist.CDF(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]) / (errorSumOfSqures / errorDF)), 4);				// P값
                    }

                    anova.Rows.Add(row);
                }


                for (int i = 0; i < interactionCount; i++)
                {
                    int index = this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i;
                    // 요인 
                    row[0] = ((DFColumn)df[index]).Name;						// 인자

                    if (((DFColumn)df[index]).Label.Equals(string.Empty))
                        row[0] = ((DFColumn)df[index]).Name;
                    else
                        row[0] = ((DFColumn)df[index]).Label;

                    row[1] = 1;					// 자유도

                    row[2] = Math.Round(this.getSumOfSqure_Variation(df, index), 4);			// 제곱합

                    row[3] = Math.Round(this.getSumOfSqure_Variation(df, index), 4);			// 평균제곱합

                    if (errorDF == 0.0 || errorSumOfSqures == 0)
                    {
                        row[4] = "*";
                        row[5] = "*";
                    }
                    else
                    {
                        row[4] = Math.Round(this.getSumOfSqure_Variation(df, index) / (errorSumOfSqures / errorDF), 4);					// F값
                        row[5] = 1 - Math.Round(fdist.CDF(this.getSumOfSqure_Variation(df, index) / (errorSumOfSqures / errorDF)), 4);				// P값
                    }

                    anova.Rows.Add(row);
                }


                if (errorDF > 0)
                {
                    // 오차 
                    row[0] = "Error";																// 오차

                    row[1] = errorDF;	// 자유도

                    row[2] = Math.Round(errorSumOfSqures, 4);													// 제곱합

                    if (errorSumOfSqures == 0)
                    {
                        row[3] = "*";											// 평균제곱합
                    }
                    else
                    {
                        row[3] = Math.Round(errorSumOfSqures / errorDF, 4);											// 평균제곱합
                    }

                    row[4] = "";																	// F값

                    row[5] = "";																	// P값

                    anova.Rows.Add(row);
                }

                // 전체 
                row[0] = "Total";																	// 전체

                row[1] = this.dblValue.Length - 1;													// 자유도

                row[2] = TotalSumofSqures;															// 제곱합

                row[3] = "";																	// 평균제곱합

                row[4] = "";																	// F값

                row[5] = "";																	// P값

                anova.Rows.Add(row);



                row = null;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                Console.Out.WriteLine(err);
                anova = null;
            }

            dsOutput.Tables.Add(anova);
            anova = null;

        }

        #endregion

        #region 3수준 요인배치법의 Anova 테이블을 만든다.

        /// <summary>
        /// 3수준 요인배치법의 Anova 분석의 출력결과를 돌려준다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="input"></param>
        private void getThreeFactorAnovaTable(DataFrame df, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {

            DataTable anova = new DataTable("Analysis of Variance");

            TotalSumofSqures = this.getSumOfSqures((DFColumn)df[this.analysisVarIndex[0]]);
            interactionCount = df.Cols - this.analysisParamsVarIndex.Length - this.analysisVarIndex.Length;

            double[] InteractionSumOfSqures = new double[interactionCount];
            double[] meanInteractionSumOfSqures = new double[interactionCount];
            double[] VarianceSumOfSqures = new double[this.analysisParamsVarIndex.Length];
            double[] meanVarianceSumOfSqures = new double[this.analysisParamsVarIndex.Length];

            int interDF = 4;
            int varDF = 2;


            #region 각인자별 SumOfSqures와 MeanSumOfSqures를 계산한다.

            for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
            {
                ArrayList alist = CountLevelOfVar(((DFColumn)df[this.analysisParamsVarIndex[i]]).ToStringArray());
                VarianceSumOfSqures[i] = this.getSumOfSquare_Variation(df, this.analysisParamsVarIndex[i], alist[0].ToString(), alist[1].ToString(), alist[2].ToString());
                meanVarianceSumOfSqures[i] = VarianceSumOfSqures[i] / varDF;
            }

            #endregion

            #region 교호작용별 SumOfSqures를 계산한다.


            if (this.input.bInteractionEffect)
            {
                for (int i = 0; i < interactionCount; i++)
                {
                    int interIndex = this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i;
                    ArrayList alist = CountLevelOfVar(((DFColumn)df[interIndex]).ToStringArray());

                    InteractionSumOfSqures[i] = this.getSumOfSquare_Variation(df, interIndex, alist[0].ToString(), alist[1].ToString(), alist[2].ToString());

                    meanInteractionSumOfSqures[i] = InteractionSumOfSqures[i] / interDF;
                }
            }

            #endregion

            #region 오차의 자유도와 오차의 SumOfSqures를 계산한다 또한 P값을 계산한다.

            errorSumOfSqures = TotalSumofSqures - (StatsFunctions.NaNSum(VarianceSumOfSqures) + StatsFunctions.NaNSum(InteractionSumOfSqures));
            errorDF = dblValue.Length - VarianceSumOfSqures.Length * 2 - InteractionSumOfSqures.Length * 4 - 1;
            if (errorDF < 0) errorDF = 0;

            FDistribution fdist = null;

            if (errorDF > 0)
            {
                fdist = new FDistribution(2, errorDF);
            }


            #endregion

            try
            {
                anova.Columns.Add(this.sSource);
                anova.Columns.Add(this.sDF);
                anova.Columns.Add(this.sSumofSquares);
                anova.Columns.Add(this.sMeanSquare);
                anova.Columns.Add(this.sFValue);
                anova.Columns.Add(this.sPValue);

                object[] row = new object[6];

                for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
                {
                    // 요인 											

                    if (((DFColumn)df[this.analysisParamsVarIndex[i]]).Label.Equals(string.Empty))
                        row[0] = ((DFColumn)df[this.analysisParamsVarIndex[i]]).Name;
                    else
                        row[0] = ((DFColumn)df[this.analysisParamsVarIndex[i]]).Label;

                    row[1] = varDF;					// 자유도

                    row[2] = Math.Round(VarianceSumOfSqures[i], 4);			// 제곱합

                    row[3] = Math.Round(meanVarianceSumOfSqures[i], 4);				// 평균제곱합

                    if (errorDF == 0.0 || errorSumOfSqures == 0)
                    {
                        row[4] = "*";
                        row[5] = "*";
                    }
                    else
                    {
                        double fvalue = meanVarianceSumOfSqures[i] / (errorSumOfSqures / errorDF);
                        row[4] = Math.Round(fvalue, 4);					// F값
                        row[5] = 1 - Math.Round(fdist.CDF(fvalue), 4);

                    }

                    anova.Rows.Add(row);
                }


                for (int i = 0; i < interactionCount; i++)
                {
                    int index = this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i;

                    // 요인 
                    row[0] = ((DFColumn)df[index]).Name;						// 인자

                    if (((DFColumn)df[index]).Label.Equals(string.Empty))
                        row[0] = ((DFColumn)df[index]).Name;
                    else
                        row[0] = ((DFColumn)df[index]).Label;

                    row[1] = interDF;					// 자유도

                    row[2] = Math.Round(InteractionSumOfSqures[i], 4);			// 제곱합

                    row[3] = Math.Round(meanInteractionSumOfSqures[i], 4);			// 평균제곱합

                    if (errorDF == 0.0 || errorSumOfSqures == 0)
                    {
                        row[4] = "*";
                        row[5] = "*";
                    }
                    else
                    {
                        row[4] = Math.Round(meanInteractionSumOfSqures[i] / (errorSumOfSqures / errorDF), 4);					// F값
                        row[5] = 1 - Math.Round(fdist.CDF(meanInteractionSumOfSqures[i] / (errorSumOfSqures / errorDF)), 4);				// P값
                    }

                    anova.Rows.Add(row);
                }


                if (errorDF > 0)
                {
                    // 오차 
                    row[0] = "Error";																// 오차

                    row[1] = errorDF;	// 자유도

                    row[2] = Math.Round(errorSumOfSqures, 4);													// 제곱합

                    if (errorSumOfSqures == 0)
                    {
                        row[3] = "*";											// 평균제곱합
                    }
                    else
                    {
                        row[3] = Math.Round(errorSumOfSqures / errorDF, 4);											// 평균제곱합
                    }

                    row[4] = "";																	// F값

                    row[5] = "";																	// P값

                    anova.Rows.Add(row);
                }

                // 전체 
                row[0] = "Total";																	// 전체

                row[1] = this.dblValue.Length - 1;													// 자유도

                row[2] = TotalSumofSqures;															// 제곱합

                row[3] = "";																	// 평균제곱합

                row[4] = "";																	// F값

                row[5] = "";																	// P값

                anova.Rows.Add(row);

                row = null;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                Console.Out.WriteLine(err);
                anova = null;

            }

            dsOutput.Tables.Add(anova);
            anova = null;

        }


        /// <summary>
        /// 3수준 요인배치법의 Anova분석결과를 통해 해당 기준에 유의하지 않은 변수를 오차항에 풀링시킨다음 해당 결과를 돌려준다.
        /// </summary>
        /// <param name="df"></param>
        /// <param naame="input"></param>
        private void getAnovaTableForPoolingForThird(DataFrame df, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {

            DataTable anova = new DataTable("Analysis of Variance For Pooling");

            TotalSumofSqures = this.getSumOfSqures((DFColumn)df[this.analysisVarIndex[0]]);

            double[] VarianceSumOfSqures = new double[this.analysisParamsVarIndex.Length];



            #region 각인자별 SumOfSqures를 계산한다.

            for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
                VarianceSumOfSqures[i] = this.getSumOfSqure_Variation(df, this.analysisParamsVarIndex[i]);

            #endregion

            #region 교호작용별 SumOfSqures를 계산한다.


            interactionCount = df.Cols - this.analysisParamsVarIndex.Length - this.analysisVarIndex.Length;

            double[] InteractionSumOfSqures = new double[interactionCount];


            if (this.input.bInteractionEffect)
            {
                Console.Out.WriteLine("교호작용별 SumOfSqures를 계산한다. length {0}", InteractionSumOfSqures.Length);


                for (int i = 0; i < interactionCount; i++)
                    InteractionSumOfSqures[i] = this.getSumOfSqure_Variation(df, this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i);
            }

            #endregion

            #region 오차의 자유도와 오차의 SumOfSqures를 계산한다 또한 P값을 계산한다.

            errorSumOfSqures = TotalSumofSqures - (StatsFunctions.NaNSum(VarianceSumOfSqures) + StatsFunctions.NaNSum(InteractionSumOfSqures));
            errorDF = dblValue.Length - VarianceSumOfSqures.Length - InteractionSumOfSqures.Length - 1;
            if (errorDF < 0) errorDF = 0;


            Console.Out.WriteLine("errorSumOfSqures = {0}", errorSumOfSqures);

            Console.Out.WriteLine("ErrorDF = {0}", errorDF);




            FDistribution fdist = null;

            if (errorDF > 0)
            {
                fdist = new FDistribution(1, errorDF);
            }

            #endregion

            try
            {
                anova.Columns.Add(this.sSource);
                anova.Columns.Add(this.sDF);
                anova.Columns.Add(this.sSumofSquares);
                anova.Columns.Add(this.sMeanSquare);
                anova.Columns.Add(this.sFValue);
                anova.Columns.Add(this.sPValue);

                object[] row = new object[6];

                for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
                {
                    // 요인 											

                    row[0] = ((DFColumn)df[this.analysisParamsVarIndex[i]]).Name;

                    row[1] = 1;					// 자유도

                    row[2] = Math.Round(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]), 4);			// 제곱합

                    row[3] = Math.Round(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]), 4);			// 평균제곱합

                    if (errorDF == 0.0 || errorSumOfSqures == 0)
                    {
                        row[4] = "*";
                        row[5] = "*";
                    }
                    else
                    {
                        row[4] = Math.Round(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]) / (errorSumOfSqures / errorDF), 4);					// F값
                        row[5] = 1 - Math.Round(fdist.CDF(this.getSumOfSqure_Variation(df, analysisParamsVarIndex[i]) / (errorSumOfSqures / errorDF)), 4);				// P값
                    }

                    anova.Rows.Add(row);
                }


                for (int i = 0; i < interactionCount; i++)
                {
                    int index = this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i;
                    // 요인 
                    row[0] = ((DFColumn)df[index]).Name;						// 인자

                    if (((DFColumn)df[index]).Label.Equals(string.Empty))
                        row[0] = ((DFColumn)df[index]).Name;
                    else
                        row[0] = ((DFColumn)df[index]).Label;

                    row[1] = 1;					// 자유도

                    row[2] = Math.Round(this.getSumOfSqure_Variation(df, index), 4);			// 제곱합

                    row[3] = Math.Round(this.getSumOfSqure_Variation(df, index), 4);			// 평균제곱합

                    if (errorDF == 0.0 || errorSumOfSqures == 0)
                    {
                        row[4] = "*";
                        row[5] = "*";
                    }
                    else
                    {
                        row[4] = Math.Round(this.getSumOfSqure_Variation(df, index) / (errorSumOfSqures / errorDF), 4);					// F값
                        row[5] = 1 - Math.Round(fdist.CDF(this.getSumOfSqure_Variation(df, index) / (errorSumOfSqures / errorDF)), 4);				// P값
                    }

                    anova.Rows.Add(row);
                }


                if (errorDF > 0)
                {
                    // 오차 
                    row[0] = "Error";																// 오차

                    row[1] = errorDF;	// 자유도

                    row[2] = Math.Round(errorSumOfSqures, 4);													// 제곱합

                    if (errorSumOfSqures == 0)
                    {
                        row[3] = "*";											// 평균제곱합
                    }
                    else
                    {
                        row[3] = Math.Round(errorSumOfSqures / errorDF, 4);											// 평균제곱합
                    }

                    row[4] = "";																	// F값

                    row[5] = "";																	// P값

                    anova.Rows.Add(row);
                }

                // 전체 
                row[0] = "Total";																	// 전체

                row[1] = this.dblValue.Length - 1;													// 자유도

                row[2] = TotalSumofSqures;															// 제곱합

                row[3] = "";																	// 평균제곱합

                row[4] = "";																	// F값

                row[5] = "";																	// P값

                anova.Rows.Add(row);



                row = null;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                Console.Out.WriteLine(err);
                anova = null;
            }

            dsOutput.Tables.Add(anova);
            anova = null;

        }

        #endregion

        #region 모형정보를 출력한다.

        /// <summary>
        /// 모형정보를 출력한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="input"></param>
        private void getModelInfoTable(DataFrame df, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {
            DataTable modelInfor = new DataTable("Model Information");

            modelInfor.Columns.Add(this.sRSquare);
            modelInfor.Columns.Add(this.sAdjRSquare);
            modelInfor.Columns.Add(this.sMSE);

            Object[] rows = new Object[3];

            if (errorDF == 0 || errorSumOfSqures == 0)
            {
                rows[0] = "1";
                rows[1] = "*";
                rows[2] = "*";
            }
            else
            {

                rows[0] = Math.Round(1 - (this.errorSumOfSqures / this.TotalSumofSqures), 3);
                rows[1] = Math.Round(1 - (this.dblValue.Length - 1) * ((this.errorSumOfSqures / this.errorDF) / this.TotalSumofSqures), 3);
                rows[2] = Math.Round(this.errorSumOfSqures / this.errorDF, 3);
            }

            modelInfor.Rows.Add(rows);

            this.dsOutput.Tables.Add(modelInfor);
            modelInfor = null;

        }

        #endregion

        #region 주효과/ 교호작용 테이블 만든다.


        /// <summary>
        /// 주효과를 출력한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="input"></param>
        /// 

        private void getMainEffectTable(DataFrame df, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {
            try
            {
                if (this.analysisParamsVarIndex.Length < 1)
                    return;

                DataTable table = new DataTable("Main Effect");

                table.Columns.Add(this.sFactor);
                table.Columns.Add(this.sMainEffect);

                object[] row = null;

                for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
                {
                    row = new object[2];

                    row[0] = ((DFColumn)df[this.analysisParamsVarIndex[i]]).Name;
                    row[1] = this.getMainEffect(df, this.analysisParamsVarIndex[i]);

                    table.Rows.Add(row);
                    row = null;
                }

                dsOutput.Tables.Add(table);
                table = null;

            }
            catch (CenterSpace.NMath.Core.InvalidArgumentException ex)
            {
                Console.Out.WriteLine(ex);

            }

        }


        /// <summary>
        /// 교호작용을 출력한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="input"></param>
        private void getInteractionEffectTable(DataFrame df, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {
            try
            {
                if (interactionCount < 1)
                    return;

                DataTable table = new DataTable("Interaction Effect");

                table.Columns.Add(this.sFactor);
                table.Columns.Add(this.sInteractionEffect);

                object[] row = null;

                for (int i = 0; i < this.interactionCount; i++)
                {
                    row = new object[2];

                    row[0] = ((DFColumn)df[this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i]).Name;
                    row[1] = this.getInteractionEffect(df, this.analysisParamsVarIndex.Length + this.analysisVarIndex.Length + i);

                    table.Rows.Add(row);
                    row = null;
                }

                dsOutput.Tables.Add(table);
                table = null;

            }
            catch (CenterSpace.NMath.Core.InvalidArgumentException ex)
            {
                Console.Out.WriteLine(ex);

            }

        }

        #endregion

        #region Anova 분석시에 데이터 조건이 적합한 여부를 판정한다.

        /// <summary>
        /// Anova 분석시에 데이터 조건이 적합한 여부를 판정한다.
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        private bool isValidate(DataFrame df)
        {
            bool isEqual = true;
            ArrayList alGroupName = null;		// 그룹별 이름
            ArrayList alGroupSize = null;		// 그룹별 갯수				

            for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
            {
                alGroupName = new ArrayList();
                alGroupSize = new ArrayList();

                DFColumn dfgCol = (DFColumn)df[this.input.analysisParams[i].ToString()];

                //Console.Out.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");

                //Console.Out.WriteLine(dfgCol.Name);

                string tempNameOld = dfgCol[0].ToString();
                string tempName = string.Empty;

                for (int k = 0; k < dfgCol.Count; k++)
                {
                    tempName = dfgCol[k].ToString();

                    //Console.Out.WriteLine(tempName);

                    // 서브그룹 변수에 널이 있는 경우,.
                    if (tempName == "." || tempName.Equals(string.Empty))
                        isEqual = false;

                    if (tempName.ToString().Equals("1") || tempName.ToString().Equals("0"))
                    {
                    }
                    else
                    {
                        isEqual = false;
                    }


                }

                //Console.Out.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");

            }

            return isEqual;
        }


        //직교배열표 형태로 존재하는지 여부를 체크한다.

        /// <summary>
        /// 직교배열표 형태로 데이터가 존재하는지 여부를 체크
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        private bool isValidateOrthogonalArrays(DataFrame df)
        {
            bool isEqual = true;
            ArrayList alGroupName = null;		// 그룹별 이름
            ArrayList alGroupSize = null;		// 그룹별 갯수				

            for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
            {
                alGroupName = new ArrayList();
                alGroupSize = new ArrayList();

                //df.SortRows(this.analysisParamsVarIndex[i]);


                DFColumn dfgCol = (DFColumn)df[this.input.analysisParams[i].ToString()];

                //DFColumn dfgCol =  (DFColumn)df[this.analysisParamsVarIndex[i]];	

                ArrayList distVar = CountLevelOfVar(dfgCol.ToStringArray());

                int[] countVars = new int[distVar.Count];

                string tempNameOld = dfgCol[0].ToString();
                string tempName = string.Empty;
                //int tempSize       = 0;

                for (int k = 0; k < dfgCol.Count; k++)
                {
                    tempName = dfgCol[k].ToString();

                    for (int j = 0; j < distVar.Count; j++)
                    {
                        if (distVar[j].ToString() == tempName)
                            countVars[j]++;
                    }

                }

                //Console.Out.WriteLine("컬럼의 구분의 크기들은 coutVars[0]= {0}",countVars[0]);
                //Console.Out.WriteLine("컬럼의 구분의 크기들은 coutVars[1]= {0}",countVars[1]);
                if (countVars[0] != countVars[1])
                {
                    isEqual = false;
                    return isEqual;
                }

            }

            return isEqual;
        }



        /// <summary>
        /// 직교배열표 형태로 존재하는지 여부를 체크한다.
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        private bool isValidateTwoWayOrthogonalArrays(DataFrame df)
        {
            bool isEqual = true;
            ArrayList alGroupName = null;		// 그룹별 이름
            ArrayList alGroupSize = null;		// 그룹별 갯수				

            for (int i = 0; i < this.analysisParamsVarIndex.Length; i++)
            {
                alGroupName = new ArrayList();
                alGroupSize = new ArrayList();

                //df.SortRows(this.analysisParamsVarIndex[i]);

                DFColumn dfgCol = (DFColumn)df[this.input.analysisParams[i].ToString()];

                //DFColumn dfgCol =  (DFColumn)df[this.analysisParamsVarIndex[i]];	

                ArrayList distVar = CountLevelOfVar(dfgCol.ToStringArray());

                dfgCol = null;

                if (distVar.Count == 2)
                {
                    isEqual = true;
                }
                else
                {
                    isEqual = false;
                    return isEqual;
                }


            }



            return isEqual;
        }



        #endregion

        #region 배열의 내용을 Distinct 하게 ArrayList로 반환한다.

        /// <summary>
        /// 배열의 내용을 Distinct하게 ArrayList로 반환한다.
        /// </summary>
        /// <param name="strVar"></param>
        /// <returns></returns>
        private ArrayList CountLevelOfVar(string[] strVar)
        {
            ArrayList alLevel = new ArrayList();
            string tempName = string.Empty;

            for (int i = 0; i < strVar.Length; i++)
            {
                tempName = strVar[i];

                // Missing값의 경우는 제외한다.
                if (tempName.Trim() == ".") continue;

                // 기존에 ArrayList에 있는 값이 아니면 추가
                if (alLevel.IndexOf(tempName) == -1)
                {
                    alLevel.Add(tempName);
                }
            }

            return alLevel;
        }

        #endregion

        #region 변수의 그룹별로 쪼개어 종속변수의 값을 가져온다.
        /// <summary>
        /// 변수의 그룹별로 쪼개어 종속변수의 값을 가져온다.
        /// </summary>
        /// <param name="factor1"></param>
        /// <param name="factor2"></param>
        /// <returns></returns>
        private double[] GetData(string factor1, string factor2)
        {
            ArrayList alData = new ArrayList();

            Object[] objFactor1 = this.alIndepFactors[0].ToArray();
            Object[] objFactor2 = null;

            if (input.analysisParams.Count > 1)
                objFactor2 = this.alIndepFactors[1].ToArray();

            bool bPart = false;
            bool bOper = false;

            // 두 인자가 모두 null인 경우 데이터를 조회할 수 없다.
            if (factor1 == null && factor2 == null) return null;

            for (int i = 0; i < dblValue.Length; i++)
            {
                // part와 매핑이 되는지
                if (objFactor1[i].ToString() == factor1)
                    bPart = true;
                else
                    bPart = false;

                // operator와 매핑이 되는지
                if (objFactor2 != null && objFactor2[i].ToString() == factor2)
                    bOper = true;
                else
                    bOper = false;

                // Operator만 조건인 경우
                if (factor1 == null && factor2 != null)
                {
                    if (bOper) alData.Add(this.dblValue[i]);
                }

                // Part만 조건인 경우
                if (factor1 != null && factor2 == null)
                {
                    if (bPart) alData.Add(this.dblValue[i]);
                }

                // Part와 Operator가 조건인 경우
                if (factor1 != null && factor2 != null)
                {
                    if (bPart && bOper) alData.Add(this.dblValue[i]);
                }
            }

            return (double[])alData.ToArray(System.Type.GetType("System.Double"));
        }




        private double[] GetData(int columnIndex1, string factor1, string factor2)
        {
            ArrayList alData = new ArrayList();

            Object[] objFactor1 = this.alIndepFactors[columnIndex1].ToArray();
            Object[] objFactor2 = null;

            bool bPart = false;
            bool bOper = false;

            // 두 인자가 모두 null인 경우 데이터를 조회할 수 없다.
            if (factor1 == null && factor2 == null) return null;

            for (int i = 0; i < dblValue.Length; i++)
            {
                // part와 매핑이 되는지
                if (objFactor1[i].ToString() == factor1)
                    bPart = true;
                else
                    bPart = false;

                // operator와 매핑이 되는지
                if (objFactor2 != null && objFactor2[i].ToString() == factor2)
                    bOper = true;
                else
                    bOper = false;

                // Operator만 조건인 경우
                if (factor1 == null && factor2 != null)
                {
                    if (bOper) alData.Add(this.dblValue[i]);
                }

                // Part만 조건인 경우
                if (factor1 != null && factor2 == null)
                {
                    if (bPart) alData.Add(this.dblValue[i]);
                }

                // Part와 Operator가 조건인 경우
                if (factor1 != null && factor2 != null)
                {
                    if (bPart && bOper) alData.Add(this.dblValue[i]);
                }
            }

            return (double[])alData.ToArray(System.Type.GetType("System.Double"));
        }



        private double[] GetData(DataFrame df, int columnIndex1, string factor1, string factor2)
        {
            ArrayList alData = new ArrayList();

            Object[] objFactor1 = (Object[])((DFColumn)df[columnIndex1]).ToStringArray();

            bool bPart = false;
            bool bOper = false;

            // 두 인자가 모두 null인 경우 데이터를 조회할 수 없다.
            if (factor1 == null && factor2 == null) return null;

            for (int i = 0; i < dblValue.Length; i++)
            {
                // part와 매핑이 되는지
                if (objFactor1[i].ToString().Equals(factor1))
                    bPart = true;
                else
                    bPart = false;

                // operator와 매핑이 되는지
                if (objFactor1[i].ToString().Equals(factor2))
                    bOper = true;
                else
                    bOper = false;

                // Operator만 조건인 경우
                if (factor1 == null && factor2 != null)
                {
                    if (bOper) alData.Add(this.dblValue[i]);
                }

                // Part만 조건인 경우
                if (factor1 != null && factor2 == null)
                {
                    if (bPart) alData.Add(this.dblValue[i]);
                }

                // Part와 Operator가 조건인 경우
                if (factor1 != null && factor2 != null)
                {
                    if (bPart && bOper) alData.Add(this.dblValue[i]);
                }
            }

            return (double[])alData.ToArray(System.Type.GetType("System.Double"));
        }




        private double[] GetData(int columnIndex1, int columnIndex2, string factor1, string factor2)
        {
            ArrayList alData = new ArrayList();

            Object[] objFactor1 = this.alIndepFactors[columnIndex1].ToArray();
            Object[] objFactor2 = this.alIndepFactors[columnIndex2].ToArray();

            bool bPart = false;
            bool bOper = false;

            // 두 인자가 모두 null인 경우 데이터를 조회할 수 없다.
            if (factor1 == null && factor2 == null) return null;

            for (int i = 0; i < dblValue.Length; i++)
            {
                // part와 매핑이 되는지
                if (objFactor1[i].ToString() == factor1)
                    bPart = true;
                else
                    bPart = false;

                // operator와 매핑이 되는지
                if (objFactor2 != null && objFactor2[i].ToString() == factor2)
                    bOper = true;
                else
                    bOper = false;

                // Operator만 조건인 경우
                if (factor1 == null && factor2 != null)
                {
                    if (bOper) alData.Add(this.dblValue[i]);
                }

                // Part만 조건인 경우
                if (factor1 != null && factor2 == null)
                {
                    if (bPart) alData.Add(this.dblValue[i]);
                }

                // Part와 Operator가 조건인 경우
                if (factor1 != null && factor2 != null)
                {
                    if (bPart && bOper) alData.Add(this.dblValue[i]);
                }

            }

            return (double[])alData.ToArray(System.Type.GetType("System.Double"));

        }




        #endregion

        #region SumOfSqures 관련 함수

        /// <summary>
        /// SumOfSquares 값을 돌려준다.
        /// </summary>
        /// <param name="dfCol"></param>
        /// <returns></returns>
        private double getSumOfSqures(DFColumn dfCol)
        {

            if (this.dblValue == null)
                return 0;

            DoubleVector variationY = dfCol.ToDoubleVector();
            //double sumOfsqureY = StatsFunctions.NaNSumOfSquares(variationY);
            double sumOfsqureY = StatsFunctions.SumOfSquaredErrors(variationY);

            return sumOfsqureY;

        }


        private double getSumOfSqure_Variation(DataFrame df, int columnIndex)
        {
            if (this.dblValue == null)
                return 0;

            string str1 = "0";
            string str2 = "1";

            double[] array1 = this.GetData(df, columnIndex, str1, null);
            double[] array2 = this.GetData(df, columnIndex, null, str2);

            double sum_str1 = StatsFunctions.NaNSum(array1);
            double sum_str2 = StatsFunctions.NaNSum(array2);

            double var = Math.Pow(sum_str1 - sum_str2, 2) / this.dblValue.Length;

            //Console.Out.WriteLine("SSR 계산. sum_str1 {0}, sum_str2 {1}",sum_str1,sum_str2);

            return var;

        }


        private double getSumOfSquare_Variation(DataFrame df, int columnIndex, string temp1, string temp2, string temp3)
        {
            if (this.dblValue == null)
                return 0;

            string str1 = temp1;
            string str2 = temp2;
            string str3 = temp3;

            double[] array1 = this.GetData(df, columnIndex, str1, null);
            double[] array2 = this.GetData(df, columnIndex, str2, null);
            double[] array3 = this.GetData(df, columnIndex, str3, null);

            double sum_str1 = StatsFunctions.NaNSum(array1);
            double sum_str2 = StatsFunctions.NaNSum(array2);
            double sum_str3 = StatsFunctions.NaNSum(array3);

            double value1 = Math.Pow(sum_str1, 2);
            double value2 = Math.Pow(sum_str2, 2);
            double value3 = Math.Pow(sum_str3, 2);

            double var = 3 * (value1 + value2 + value3) / this.dblValue.Length;
            double ct = Math.Pow(StatsFunctions.Sum(this.dblValue), 2) / this.dblValue.Length;

            return var - ct;
        }


        private double getSumOfSquare_Interaction(DataFrame df, int columnIndex, string temp1, string temp2, string temp3)
        {
            if (this.dblValue == null)
                return 0;

            string str1 = temp1;
            string str2 = temp2;
            string str3 = temp3;

            double[] array1 = this.GetData(df, columnIndex, str1, null);
            double[] array2 = this.GetData(df, columnIndex, str2, null);
            double[] array3 = this.GetData(df, columnIndex, str3, null);

            double sum_str1 = StatsFunctions.NaNSum(array1);
            double sum_str2 = StatsFunctions.NaNSum(array2);
            double sum_str3 = StatsFunctions.NaNSum(array3);

            double value1 = Math.Pow(sum_str1, 2);
            double value2 = Math.Pow(sum_str2, 2);
            double value3 = Math.Pow(sum_str3, 2);

            double var = 3 * (value1 + value2 + value3) / this.dblValue.Length;
            double ct = Math.Pow(StatsFunctions.Sum(this.dblValue), 2) / this.dblValue.Length;

            return var - ct;
        }





        #endregion

        #region 교호작용 컬럼을 생성한다.

        /// <summary>
        /// 두개인자의 교호작용 컬럼을 생성한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="columnIndex1"></param>
        /// <param name="columnIndex2"></param>
        private void makeInteractionColumn(DataFrame df, int columnIndex1, int columnIndex2)
        {

            Object[] cols1 = (Object[])((DFColumn)df[columnIndex1]).ToArray();
            Object[] cols2 = (Object[])((DFColumn)df[columnIndex2]).ToArray();

            int[] interactionCols = new int[cols1.Length];
            string[] strArray = new string[cols1.Length];


            for (int i = 0; i < interactionCols.Length; i++)
            {
                interactionCols[i] = (Convert.ToInt32(cols1[i].ToString()) + Convert.ToInt32(cols2[i].ToString())) % 2;
                strArray[i] = Convert.ToString((Convert.ToInt32(cols1[i].ToString()) + Convert.ToInt32(cols2[i].ToString())) % 2);
            }


            string[] colsName = new string[2];

            colsName[0] = ((DFColumn)df[columnIndex1]).Name;
            colsName[1] = ((DFColumn)df[columnIndex2]).Name;

            Array.Sort(colsName);

            string colName = colsName[0] + "*" + colsName[1];

            //Console.Out.WriteLine(colName);
            if (IsConfoundingInteraction(df, strArray))
            {
                string[] names = df.ColumnNames;

                int tmp = this.indexOfConfoundCol;
                if (tmp > -1)
                {
                    if (((DFColumn)df[tmp]).Label.Equals(string.Empty))
                        ((DFColumn)df[tmp]).Label = ((DFColumn)df[tmp]).Name;
                    else
                        ((DFColumn)df[tmp]).Label += "=" + colName;
                }


            }
            else
            {
                DFIntColumn numCol = new DFIntColumn(colName, interactionCols);
                numCol.Label = string.Empty;

                bool bPooled = false;
                for (int i = 0; i < this.input.pooledParams.Count; i++)
                {
                    if (this.input.pooledParams[i].ToString().Equals(numCol.Name))
                        bPooled = true;
                }

                if (!bPooled)
                    df.AddColumn(numCol);

            }

        }



        /// <summary>
        /// 3인자 경우 두개인자의 교호작용 컬럼을 생성한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="columnIndex1"></param>
        /// <param name="columnIndex2"></param>
        private void makeInteractionColumnForThird(DataFrame df, int columnIndex1, int columnIndex2)
        {

            Object[] cols1 = (Object[])((DFColumn)df[columnIndex1]).ToArray();
            Object[] cols2 = (Object[])((DFColumn)df[columnIndex2]).ToArray();

            int[] interactionCols = new int[cols1.Length];
            string[] strArray = new string[cols1.Length];


            for (int i = 0; i < interactionCols.Length; i++)
            {
                interactionCols[i] = (Convert.ToInt32(cols1[i].ToString()) + Convert.ToInt32(cols2[i].ToString())) % 3;
                strArray[i] = Convert.ToString((Convert.ToInt32(cols1[i].ToString()) + Convert.ToInt32(cols2[i].ToString())) % 3);
            }


            string[] colsName = new string[2];

            colsName[0] = ((DFColumn)df[columnIndex1]).Name;
            colsName[1] = ((DFColumn)df[columnIndex2]).Name;

            Array.Sort(colsName);

            string colName = colsName[0] + "*" + colsName[1];

            //Console.Out.WriteLine(colName);
            if (IsConfoundingInteraction(df, strArray))
            {
                string[] names = df.ColumnNames;

                int tmp = this.indexOfConfoundCol;
                if (tmp > -1)
                {
                    if (((DFColumn)df[tmp]).Label.Equals(string.Empty))
                        ((DFColumn)df[tmp]).Label = ((DFColumn)df[tmp]).Name;
                    else
                        ((DFColumn)df[tmp]).Label += "=" + colName;
                }


            }
            else
            {
                DFIntColumn numCol = new DFIntColumn(colName, interactionCols);
                numCol.Label = string.Empty;

                bool bPooled = false;
                for (int i = 0; i < this.input.pooledParams.Count; i++)
                {
                    if (this.input.pooledParams[i].ToString().Equals(numCol.Name))
                        bPooled = true;
                }

                if (!bPooled)
                    df.AddColumn(numCol);

            }

        }


        /// <summary>
        /// 세개 인자의 교호작용 컬럼을 사용한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="columnIndex1"></param>
        /// <param name="columnIndex2"></param>
        /// <param name="columnIndex3"></param>
        public void makeInteractionColumn(DataFrame df, int columnIndex1, int columnIndex2, int columnIndex3)
        {
            Object[] cols1 = (Object[])((DFColumn)df[columnIndex1]).ToArray();
            Object[] cols2 = (Object[])((DFColumn)df[columnIndex2]).ToArray();
            Object[] cols3 = (Object[])((DFColumn)df[columnIndex3]).ToArray();

            int[] interactionCols = new int[cols1.Length];
            string[] strArray = new string[cols1.Length];

            for (int i = 0; i < interactionCols.Length; i++)
            {
                interactionCols[i] = (Convert.ToInt32(cols1[i].ToString()) + Convert.ToInt32(cols2[i].ToString()) + Convert.ToInt32(cols3[i].ToString())) % 2;
                strArray[i] = Convert.ToString((Convert.ToInt32(cols1[i].ToString()) + Convert.ToInt32(cols2[i].ToString()) + Convert.ToInt32(cols3[i].ToString())) % 2);
            }


            string[] colsName = new string[3];

            colsName[0] = ((DFColumn)df[columnIndex1]).Name;
            colsName[1] = ((DFColumn)df[columnIndex2]).Name;
            colsName[2] = ((DFColumn)df[columnIndex3]).Name;

            Array.Sort(colsName);

            string colName = colsName[0] + "*" + colsName[1] + "*" + colsName[2];

            if (IsConfoundingInteraction(df, strArray))
            {
                string[] name = df.ColumnNames;
                int tmp = this.indexOfConfoundCol;

                if (tmp > -1)
                {
                    if (((DFColumn)df[tmp]).Label.Equals(string.Empty))
                        ((DFColumn)df[tmp]).Label = ((DFColumn)df[tmp]).Name;
                    //else
                    //((DFColumn)df[tmp]).Label +=  "=" +  colName;
                }
            }
            else
            {
                DFIntColumn numCol = new DFIntColumn(colName, interactionCols);
                numCol.Label = string.Empty;

                bool bPooled = false;

                for (int i = 0; i < this.input.pooledParams.Count; i++)
                {
                    if (this.input.pooledParams[i].ToString().Equals(numCol.Name))
                        bPooled = true;
                }

                if (!bPooled)
                    df.AddColumn(numCol);
            }
        }

        #endregion

        #region 주효과/ 교호작용을 구한다.

        /// <summary>
        /// 주효과를 구한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private double getMainEffect(DataFrame df, int columnIndex)
        {
            if (this.dblValue == null)
                return 0;

            string str1 = "0";
            string str2 = "1";

            double[] array1 = this.GetData(df, columnIndex, str1, null);
            double[] array2 = this.GetData(df, columnIndex, null, str2);

            double sum_str1 = StatsFunctions.NaNSum(array1);
            double sum_str2 = StatsFunctions.NaNSum(array2);

            double var = (sum_str2 - sum_str1) / (this.dblValue.Length / 2);

            return Convert.ToDouble(Math.Abs(var));

        }


        /// <summary>
        /// 교호작용을 구한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private double getInteractionEffect(DataFrame df, int columnIndex)
        {
            if (this.dblValue == null)
                return 0;

            string str1 = "0";
            string str2 = "1";

            double[] array1 = this.GetData(df, columnIndex, str1, null);
            double[] array2 = this.GetData(df, columnIndex, null, str2);

            double sum_str1 = StatsFunctions.NaNSum(array1);
            double sum_str2 = StatsFunctions.NaNSum(array2);

            double var = (sum_str1 - sum_str2) / (this.dblValue.Length / 2);

            return Convert.ToDouble(Math.Abs(var));

        }

        #endregion

        #region Anova 테이블중 유의하지 않은 변수를 자동적으로 아니면 사용자 정의형태로 풀링시킨다.

        /// <summary>
        /// 오차항에 풀링시킨다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="input"></param>

        private DataFrame removePoolingData(DataFrame df, DACrux.BStats.StatisticsInput.inputFactorAnalysis input)
        {
            pooledParams = (ArrayList)input.pooledParams;
            unPooledParams = (ArrayList)input.unPooledParams;

            if (pooledParams != null && pooledParams.Count > 0)
            {
                for (int i = 0; i < pooledParams.Count; i++)
                    df.RemoveColumn(pooledParams[i].ToString());
            }

            return df;
        }


        /// <summary>
        /// 인덱스를 셋팅한다.
        /// </summary>
        private void setIndexParameters()
        {
            // ClassVariables를 셋팅한다.
            for (int i = 0; i < input.analysisParams.Count; i++)
            {
                int index = getIndex(dfNew, input.analysisParams[i].ToString());
                this.analysisParamsVarIndex[i] = index;
            }

            // AnalysisVariables를 셋팅한다.
            for (int j = 0; j < input.analysisVariables.Count; j++)
            {
                int index = getIndex(dfNew, input.analysisVariables[j].ToString());
                this.analysisVarIndex[j] = index;
            }

        }




        #endregion

        /// <summary>
        /// 해당 문자열이 배열에 있을 경우 해당 인덱스를 반환한다.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private int getIndex_factor(string[] array, string str)
        {

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(str))
                    return i;
            }

            return -1;
        }

        #region 주효과 및 교호작용 차트를 그린다.

        /// <summary>
        /// 주효과 차트를 그린다.
        /// </summary>
        /// <returns></returns>
        public DataSet getMainEffectForChart()
        {

            DataSet resultDataSet = new DataSet();

            ArrayList[] groupFactors = new ArrayList[this.input.analysisParams.Count];

            for (int i = 0; i < groupFactors.Length; i++)
                groupFactors[i] = this.CountLevelOfVar(((DFColumn)dfNew[this.input.analysisParams[i].ToString()]).ToStringArray());

            for (int j = 0; j < this.input.analysisParams.Count; j++)
            {
                Object[] arGroup1 = groupFactors[j].ToArray();

                DataTable table = new DataTable("MainEffect" + "[" + input.analysisParams[j].ToString() + "]");
                table.Columns.Add("Kind");
                table.Columns.Add("Level");
                table.Columns.Add("Y");

                Object[] row = null;

                for (int k = 0; k < arGroup1.Length; k++)
                {
                    row = new Object[3];

                    string str1 = arGroup1[k].ToString();

                    row[0] = "MainEffect" + "_" + input.analysisParams[j].ToString();

                    row[1] = str1;

                    double[] array = this.GetData(j, str1, null);
                    row[2] = StatsFunctions.NaNMean(array);

                    table.Rows.Add(row);
                    row = null;
                }

                //this.dsOutput.Tables.Add(table);
                resultDataSet.Tables.Add(table);

                table = null;
            }

            return resultDataSet;

        }


        /// <summary>
        /// 교호작용 차트를 그린다.
        /// </summary>
        /// <returns></returns>
        public DataSet getInteractionForChart()
        {
            if (this.analysisParamsVarIndex.Length < 2)
                return null;

            DataSet resultDataSet = new DataSet();

            combi = new Combination(this.analysisParamsVarIndex.Length, 2);

            alCombi = combi.GenerateCombiation();

            for (int i = 0; i < alCombi.Count; i++)
            {
                string str = alCombi[i].ToString();

                int index1 = Convert.ToInt32(str.Substring(0, 1));
                int index2 = Convert.ToInt32(str.Substring(1, 1));

                Object[] arGroup1 = this.alGroupFactors[index1].ToArray();
                Object[] arGroup2 = this.alGroupFactors[index2].ToArray();

                string name1 = input.analysisParams[index1].ToString();
                string name2 = input.analysisParams[index2].ToString();

                DataTable table = new DataTable("interaction" + "[" + name1 + "*" + name2 + "]");
                table.Columns.Add("Kind");
                table.Columns.Add("Level");

                for (int j = 0; j < arGroup2.Length; j++)
                    table.Columns.Add(name2 + "_" + arGroup2[j].ToString());

                Object[] row = null;

                for (int k = 0; k < arGroup1.Length; k++)
                {
                    row = new Object[arGroup2.Length + 2];

                    string str1 = arGroup1[k].ToString();

                    row[0] = "Interaction " + name1 + "*" + name2;
                    row[1] = str1;

                    for (int p = 0; p < arGroup2.Length; p++)
                    {
                        string str2 = arGroup2[p].ToString();

                        double[] array = this.GetData(index1, index2, str1, str2);
                        row[p + 2] = StatsFunctions.NaNMean(array);
                    }

                    table.Rows.Add(row);
                    row = null;
                }

                resultDataSet.Tables.Add(table);
                table = null;

            }

            return resultDataSet;

        }


        #endregion

        #region 배열의 내용이 서로 같은지 여부를 판단한다.

        /// <summary>
        /// INT 배열의 내용이 서로 같은지 여부를 판단한다.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool IsEqualArray(int[] a, int[] b)
        {
            bool isEqaul = false;

            int length = a.Length;

            if (a.Length == b.Length)
            {
                bool check = true;
                for (int i = 0; i < length; i++)
                {
                    if (a[i] == b[i])
                    {

                    }
                    else
                    {
                        check = false;
                    }
                }
                isEqaul = check;
            }

            return isEqaul;
        }

        /// <summary>
        /// 문자열 배열의 내용이 서로 같은지 여부를 판단한다.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool IsEqualArray(string[] a, string[] b)
        {
            bool isEqaul = false;

            int length = a.Length;

            if (a.Length == b.Length)
            {
                bool check = true;
                //Console.Out.WriteLine("======================== start  Confound Job  ========================");
                for (int i = 0; i < length; i++)
                {
                    //Console.Out.WriteLine("{0} == {1}",a[i],b[i]);

                    if (a[i].Equals(b[i]))
                    {

                    }
                    else
                    {
                        check = false;
                    }
                }

                //Console.Out.WriteLine("======================== bool {0} ",check);

                isEqaul = check;
            }

            return isEqaul;
        }

        /// <summary>
        /// 해당 컬럼이 교락되어 있는지 여부를 판단한다.
        /// </summary>
        /// <param name="df"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool IsConfoundingInteraction(DataFrame df, string[] col)
        {
            bool isConfound = false;

            int length = df.Cols;

            string[] strArray = col;

            for (int i = 0; i < length; i++)
            {
                if (this.analysisVarIndex[0] == i)
                    continue;

                string[] array = ((DFColumn)df[i]).ToStringArray();

                if (this.IsEqualArray(array, strArray))
                {
                    isConfound = true;
                    this.indexOfConfoundCol = i;
                }

            }

            if (!isConfound)
            {
                string[] strArray1 = new string[col.Length];

                for (int i = 0; i < col.Length; i++)
                {
                    strArray1[i] = Convert.ToString((Convert.ToInt32(col[i]) + 1) % 2);
                }

                for (int i = 0; i < length; i++)
                {
                    if (this.analysisVarIndex[0] == i)
                        continue;

                    string[] array = ((DFColumn)df[i]).ToStringArray();

                    if (this.IsEqualArray(array, strArray1))
                    {
                        isConfound = true;
                        this.indexOfConfoundCol = i;
                    }

                }

            }

            return isConfound;
        }

        private bool IsConfoundingInteraction(DataFrame df, int[] col)
        {
            bool isConfound = false;

            int length = df.Cols;

            int[] iArray = col;

            for (int i = 0; i < length; i++)
            {
                int[] array = ((DFColumn)df[i]).ToIntArray();

                if (this.IsEqualArray(array, iArray))
                {
                    isConfound = true;
                    this.indexOfConfoundCol = i;
                }

            }

            return isConfound;
        }

        #endregion

    }
}
