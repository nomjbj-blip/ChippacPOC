using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace DACrux.BStats
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlConverter
    {

        #region " MEMBER FIELD "

        #region [ LANGUAGE ]
        string m_Space1 = "&nbsp;";
        string m_Space2 = "&nbsp;&nbsp;";
        //string m_Space3 = "&nbsp;&nbsp;&nbsp;";
        //string m_Space4 = "&nbsp;&nbsp;&nbsp;&nbsp;";

        private string m_ErrorColNameNotEqual = "Length of ColumnNames doesn't equal";

        #region Correlation
        string m_PearsonCorrelation = "Pearson correlation";
        //string m_Pvalue = "P-Value";
        string m_Spearman = "Spearman's Rho";
        //string m_RegressionEquation = "Regression Equation";
        //string m_ScatterPlot = "ScatterPlot";
        #endregion

        #region CapaContinuous
        string m_ProcessData = "Process Data";
        string m_LSL = "LSL";
        string m_Target = "Target";
        string m_USL = "USL";
        string m_SampleMean = "Sample Mean";
        string m_SampleN = "Sample N";
        string m_SigmaBetween = "StDev(Between)";
        string m_SigmaWithin = "StDev(Within)";
        string m_SigmaBW = "StDev(B/W)";
        string m_SigmaOverall = "StDev(Overall)";

        string m_ObsPerformance = "Observed Performance";
        string m_WithinPerformance = "Exp. Within Performance";
        string m_BWPerformance = "Exp. B/W Performance";
        string m_OverallPerformance = "Exp. Overall Performance";

        string m_PPMlessLSL = "PPK < LSL";
        string m_PPMmoreUSL = "PPK > USL";
        string m_PPMtotal = "PPM Total";

        string m_PPMlessLSLper = "% < LSL";
        string m_PPMmoreUSLper = "% > USL";
        string m_PPMtotalper = "% Total";

        string m_WithinCapability = "Within Capability";
        string m_BWCapability = "B/W Capability";
        string m_OverallCapability = "Overall Capability";

        string m_Cp = "Cp";
        string m_CPL = "CPL";
        string m_CPU = "CPU";
        string m_Cpk = "Cpk";
        string m_CCpk = "CCpk";
        string m_Zbench = "Z.Bench";
        string m_Zlsl = "Z.LSL";
        string m_Zusl = "Z.USL";

        string m_Pp = "Pp";
        string m_PPL = "PPL";
        string m_PPU = "PPU";
        string m_Ppk = "Ppk";
        string m_Cpm = "Cpm";

        #endregion

        #region Regression
        string m_RegressionEquation = "The regression equation";
        string m_ErrorRegressionData = "Check data. this data can not analysis";
        #endregion

        #region Anova
        string m_ErrorAnovaData = "Check data. this data can not analysis";
        #endregion

        #region Hypothesis testing
        string m_Hypothesis = "Hypothesis";
        string m_CI = "Confidence interval";
        string m_CriticalValue = "Critical value";
        string m_TestResult = "Test result";
        #endregion

        #endregion

        /// <summary>
        /// 파일명
        /// </summary>
        private string m_Filename;

        /// <summary>
        /// Stream Writer
        /// </summary>
        private StreamWriter m_Sw;

        

        #endregion

        #region " CREATOR "
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="FullFileName">html파일이 저장될 경로+파일명</param>
        public HtmlConverter(string FullFileName)
        {
            try
            {
                m_Filename = FullFileName;
                DirectoryInfo oDI = new DirectoryInfo(m_Filename);
                if (!oDI.Parent.Exists)
                {
                    oDI.Parent.Create();
                    oDI.Parent.Attributes = FileAttributes.Hidden;
                }               
                m_Sw = new StreamWriter(m_Filename, false, System.Text.Encoding.UTF8);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region " METHOD "

        #region [ Header & Main Title ]
        /// <summary>
        /// Header & Main Title Print
        /// </summary>
        /// <param name="Project">Project Name</param>
        /// <param name="WorkSheet">WorkSheet Name</param>
        /// <param name="User">User Name</param>
        /// <param name="Title">Main Title</param>
        public void MainTitle(string Project, string WorkSheet, string User, string Title)
        {
            try
            {
                m_Sw.WriteLine("<html>");
                m_Sw.WriteLine("<head>");
                m_Sw.WriteLine("<meta http-equiv='Content-Type' content='text/html; charset=gb2312'>");
                m_Sw.WriteLine("<title></title>");
                m_Sw.WriteLine("<HR size='1'>");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<style type='text/css'>");
                m_Sw.WriteLine("SELECT  {background-color : #ffffff;}");
                m_Sw.WriteLine("body, table, tr, td, select, textarea, input{ ");
                m_Sw.WriteLine(" 		font-family: Tahoma, seoul, Tahoma, helvetica;");
                m_Sw.WriteLine(" 		font-size: 12px;");
                m_Sw.WriteLine(" 		color: #000000;");
                m_Sw.WriteLine("		scrollbar-3dlight-color:595959;");
                m_Sw.WriteLine("	        scrollbar-arrow-color:ffffff;");
                m_Sw.WriteLine("	        scrollbar-base-color:CFCFCF;");
                m_Sw.WriteLine("	        scrollbar-darkshadow-color:FFFFFF;");
                m_Sw.WriteLine("	        scrollbar-face-color:CFCFCF;");
                m_Sw.WriteLine("	        scrollbar-highlight-color:FFFFF;");
                m_Sw.WriteLine("	        scrollbar-shadow-color:595959");
                m_Sw.WriteLine(" 	}");
                m_Sw.WriteLine(".copy	{");
                m_Sw.WriteLine("	font-size: 8pt;");
                m_Sw.WriteLine("	color : #888887;");
                m_Sw.WriteLine("	font-family : Tahoma;");
                m_Sw.WriteLine("}");
                m_Sw.WriteLine(".HEIGHT	{font-size:9pt;line-height:13pt;}");
                m_Sw.WriteLine(".SMALL	{font-size:8pt;}");
                m_Sw.WriteLine("TD	{");
                m_Sw.WriteLine("	color: #000000;");
                m_Sw.WriteLine("	font-size:9pt;");
                m_Sw.WriteLine("	font-family : Tahoma;");
                m_Sw.WriteLine("}");
                m_Sw.WriteLine("A	{");
                m_Sw.WriteLine("	color: #000000;");
                m_Sw.WriteLine("	TEXT-DECORATION: none");
                m_Sw.WriteLine("}");
                m_Sw.WriteLine("A:hover		{");
                m_Sw.WriteLine("	color: #003366;");
                m_Sw.WriteLine("	TEXT-DECORATION: underline");
                m_Sw.WriteLine("}");
                m_Sw.WriteLine("DIV	{");
                m_Sw.WriteLine("	color: #000000;");
                m_Sw.WriteLine("	font-size:9pt;");
                m_Sw.WriteLine("	font-family : Tahoma;");
                m_Sw.WriteLine("}");
                m_Sw.WriteLine("TTL {");
                m_Sw.WriteLine("	color: #ffffff;");
                m_Sw.WriteLine("	font-size:9pt;");
                m_Sw.WriteLine("	font-family : Tahoma;");
                m_Sw.WriteLine("}");

                m_Sw.WriteLine("</STYLE>");

                m_Sw.WriteLine("</head>");
                m_Sw.WriteLine("<body>");

                m_Sw.WriteLine("<!----------------------------------- Header ---------------------------------->");

                m_Sw.WriteLine("<table width='100%' border='0' cellspacing='0'>");
                m_Sw.WriteLine("	<tr height='25'> ");
                m_Sw.WriteLine("		<td width='100' align=left><B>● Project</B></td>");
                m_Sw.WriteLine("		<td align='left'>:&nbsp;" + Project + "</td>");
                m_Sw.WriteLine("	</tr>");
                m_Sw.WriteLine("	<tr height='25'> ");
                m_Sw.WriteLine("		<td width='100'><B>● WorkSheet</B></td>");
                m_Sw.WriteLine("		<td align='left'>:&nbsp;" + WorkSheet + "</td>");
                m_Sw.WriteLine("	</tr>");
                m_Sw.WriteLine("	<tr height='25'> ");
                m_Sw.WriteLine("		<td width='100'><B>● User Name</B></td>");
                m_Sw.WriteLine("		<td align='left'>:&nbsp;" + User + "</td>");
                m_Sw.WriteLine("	</tr>");
                m_Sw.WriteLine("	<tr height='25'> ");
                m_Sw.WriteLine("		<td width='100'><B>● Analysis Date</B></td>");
                m_Sw.WriteLine("		<td align='left'>:&nbsp;" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                m_Sw.WriteLine("	</tr>");
                m_Sw.WriteLine("	<tr height='25'> ");
                m_Sw.WriteLine("		<td width='100'><B>● Title</B></td>");
                m_Sw.WriteLine("		<td align='left'>:&nbsp;" + Title + "</td>");
                m_Sw.WriteLine("	</tr>");
                m_Sw.WriteLine("</table>");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<HR size='1'>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ Sub Title ]
        /// <summary>
        /// 부제목(표제목) 출력
        /// </summary>
        /// <param name="SubTitle">부제목(표제목)</param>
        public void SubTitle(string SubTitle)
        {
            try
            {
                m_Sw.WriteLine("<!----------------------------------- Sub Title -------------------------------->");
                m_Sw.WriteLine("<table width='100%' border='0' cellspacing='1' cellpadding='1'>");
                m_Sw.WriteLine("	<tr> ");
                m_Sw.WriteLine("		<td height='40' align='left' bgcolor='#FFFFFF'><b>" + SubTitle + "</b></td>");
                m_Sw.WriteLine("	</tr>");
                m_Sw.WriteLine("</table>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ Body ]

        #region Descriptive

        public void BodyDescriptive(DataTable dtSource, bool IsSeries, string[] arrHistogramPath, string[] arrBoxPlotPath, string[] arrRawDataPlot)
        {
            int iRowCnt;
            int iColCnt;
            string strVar = string.Empty;
            int iRowS;  // 한가지 Var Rows Start Index
            int iRowE;  // 한가지 Var Rows End Index
            int iBoxPlot;
            try
            {
                if (dtSource == null)
                    throw new ArgumentException(ErrorParameter("dtSource"));
                iRowCnt = dtSource.Rows.Count;
                if(iRowCnt <1)
                    throw new ArgumentException(ErrorParameter("dtSource"));
                iColCnt = dtSource.Columns.Count;
                if (iColCnt < 1)
                    throw new ArgumentException(ErrorParameter("dtSource"));

                strVar = dtSource.Rows[0][0].ToString();
                iRowS = 0;
                iBoxPlot = 0;
                for (int i = 0; i < iRowCnt; i++)
                {
                    if (strVar != dtSource.Rows[i][0].ToString())
                    {
                        strVar = dtSource.Rows[i][0].ToString();
                        iRowE = i - 1;
                        if (arrBoxPlotPath != null)
                        {
                            UnitTable(dtSource, IsSeries, iRowS, iRowE, arrHistogramPath, arrBoxPlotPath[iBoxPlot],arrRawDataPlot[iBoxPlot]);
                        }
                        else
                        {
                            UnitTable(dtSource, IsSeries, iRowS, iRowE, arrHistogramPath, string.Empty,string.Empty);
                        }
                        iBoxPlot++;
                        iRowS = i;
                    }
                }
                iRowE = iRowCnt - 1;
                if (arrBoxPlotPath != null)
                {
                    UnitTable(dtSource, IsSeries, iRowS, iRowE, arrHistogramPath, arrBoxPlotPath[iBoxPlot], arrRawDataPlot[iBoxPlot]);
                }
                else
                {
                    UnitTable(dtSource, IsSeries, iRowS, iRowE, arrHistogramPath, string.Empty, string.Empty);
                }
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<br>");
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 같은  Variables을 가진 Rows를 하나의 Table로 그려주는 코드를 추가한다.
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="IsSeries">Series구분 변수가 있는지</param>
        /// <param name="iStartRow"></param>
        /// <param name="iEndRow"></param>
        private void UnitTable(DataTable dtSource, bool IsSeries, int iStartRow, int iEndRow, string[] arrHistogramPath, string strBoxPlotPath, string strRawDataPlotPath)
        {
            int iColCnt;
            try
            {
                iColCnt = dtSource.Columns.Count;

                m_Sw.WriteLine("<!----------------------------- Contents Format 1  ----------------------------->");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<table border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");




                if (IsSeries)
                {
                    for (int i = 0; i < iColCnt; i++)
                    {

                        m_Sw.WriteLine("	<tr align='center' height='22'>");
                        if (i == 0)
                        {
                            m_Sw.WriteLine("		<td colspan =" + Convert.ToString(iEndRow - iStartRow+2) + " align='center' valign='center' bgcolor = '#6699cc' bordercolordark='#6699cc'><font color='#ffffff'>" + dtSource.Rows[iStartRow][0].ToString() + "</font></td>");
                            if (strBoxPlotPath != null)
                            {
                                if (strBoxPlotPath != string.Empty)
                                {
                                    m_Sw.WriteLine("   		<td align='center' valign='center' bgcolor = '#6699cc' bordercolordark='#6699cc'><font color='#ffffff'>BoxPlot</font></td>");
                                    m_Sw.WriteLine("   		<td align='center' valign='center' bgcolor = '#6699cc' bordercolordark='#6699cc'><font color='#ffffff'>RawDataPlot</font></td>");
                                }
                            }
                            m_Sw.WriteLine("	</tr>");
                            continue;                           
                        }
                        m_Sw.WriteLine("<td align='center' bgcolor = '#eeeeee' bordercolordark='#eeeeee' width='70'><font color='#000000'>" + dtSource.Columns[i].ColumnName + "</font></td>");
                        for (int j = iStartRow; j < iEndRow + 1; j++)
                        {
                            if (i == 1)
                            {
                                m_Sw.WriteLine("		<td align='center' bgcolor = '#eeeeeg'>" + dtSource.Rows[j][1].ToString() + "&nbsp;</td>");
                                if (j == iEndRow)
                                {
                                    if (strBoxPlotPath != null && strBoxPlotPath != string.Empty)
                                    {
                                        if (arrHistogramPath != null)
                                        {
                                            if (arrHistogramPath.Length != 0)
                                            {
                                                m_Sw.WriteLine("		<td align='center' rowspan=" + iColCnt.ToString() + " ><img src= " + @"""" + strBoxPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                                m_Sw.WriteLine("		<td align='center' rowspan=" + iColCnt.ToString() + " ><img src= " + @"""" + strRawDataPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                            }
                                            else
                                            {
                                                m_Sw.WriteLine("		<td align='center' rowspan=" + ((int)iColCnt + 1).ToString() + " ><img src= " + @"""" + strBoxPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                                m_Sw.WriteLine("		<td align='center' rowspan=" + ((int)iColCnt + 1).ToString() + " ><img src= " + @"""" + strRawDataPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                            }
                                        }
                                        else
                                        {
                                            m_Sw.WriteLine("		<td align='center' rowspan=" + ((int)iColCnt + 1).ToString() + " ><img src= " + @"""" + strBoxPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                            m_Sw.WriteLine("		<td align='center' rowspan=" + ((int)iColCnt + 1).ToString() + " ><img src= " + @"""" + strRawDataPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                        }
                                     }
                                }
                            }
                            else
                            {
                                if (double.IsNaN(Convert.ToDouble(dtSource.Rows[j][i])))
                                    m_Sw.WriteLine("		<td align='right'>*&nbsp;</td>");
                                else
                                    m_Sw.WriteLine("		<td align='right'>" + dtSource.Rows[j][i].ToString() + "&nbsp;</td>");
                            }
                        }
                        m_Sw.WriteLine("	</tr>");
                        if (i == 1)  //Histogram
                        {
                            if (arrHistogramPath != null && arrHistogramPath.Length != 0)
                            {
                                m_Sw.WriteLine("	<tr align='center' height='22'>");
                                m_Sw.WriteLine("<td align='center' bgcolor = '#eeeeee' bordercolordark='#eeeeee'><font color='#000000'>Histogram</font></td>");
                                for (int j = iStartRow; j < iEndRow + 1; j++)
                                {
                                    m_Sw.WriteLine("<td align='center'><img src= " + @"""" + arrHistogramPath[j].Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + ">&nbsp;</td>");
                                }
                                m_Sw.WriteLine("	</tr>");
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < iColCnt; i++)
                    {

                        m_Sw.WriteLine("	<tr align='center' height='22'>");
                        if (i == 0)
                        {
                            #region 첫번째 컬럼
                            m_Sw.WriteLine("		<td colspan =" + Convert.ToString(iEndRow - iStartRow+2) + " align='center' valign='center' bgcolor = '#6699cc' bordercolordark='#6699cc'><font color='#ffffff'>" + dtSource.Rows[iStartRow][0].ToString() + "</font></td>");
                            if (strBoxPlotPath != null)
                            {
                                if (strBoxPlotPath != string.Empty)
                                {
                                    m_Sw.WriteLine("   		<td align='center' valign='center' bgcolor = '#6699cc' bordercolordark='#6699cc'><font color='#ffffff'>BoxPlot</font></td>");
                                    m_Sw.WriteLine("   		<td align='center' valign='center' bgcolor = '#6699cc' bordercolordark='#6699cc'><font color='#ffffff'>RawDataPlot</font></td>");
                                }
                            }
                            m_Sw.WriteLine("	</tr>");

                            //Histogram
                            if (arrHistogramPath != null)
                            {
                                if (arrHistogramPath.Length != 0)
                                {
                                    m_Sw.WriteLine("	<tr align='center' height='22'>");
                                    m_Sw.WriteLine("<td align='center' bgcolor = '#eeeeee' bordercolordark='#eeeeee'" + "><font color='#000000'>Histogram</font></td>");
                                    for (int j = iStartRow; j < iEndRow + 1; j++)
                                    {
                                        m_Sw.WriteLine("<td align='center'><img src= " + @"""" + arrHistogramPath[j].Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + ">&nbsp;</td>");
                                    }

                                    if (strBoxPlotPath != null)
                                    {
                                        if (strBoxPlotPath != string.Empty)
                                        {
                                            m_Sw.WriteLine("		<td align='center' rowspan=" + iColCnt.ToString() + "><img src= " + @"""" + strBoxPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                            m_Sw.WriteLine("		<td align='center' rowspan=" + iColCnt.ToString() + "><img src= " + @"""" + strRawDataPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                        }
                                    }

                                    m_Sw.WriteLine("	</tr>");
                                }                                
                            }                            
                            continue;
                            #endregion
                        }
                            
                        m_Sw.WriteLine("<td align='center' bgcolor = '#eeeeee' bordercolordark='#eeeeee' width='70'><font color='#000000'>" + dtSource.Columns[i].ColumnName + "</font></td>");
                        for (int j = iStartRow; j < iEndRow + 1; j++)
                        {
                            m_Sw.WriteLine("		<td align='right'>" + dtSource.Rows[j][i].ToString() + "&nbsp;</td>");
                            if (i == 1)
                            {
                                if (arrHistogramPath == null)
                                {
                                    if (strBoxPlotPath != null && strBoxPlotPath != string.Empty)
                                    {
                                        m_Sw.WriteLine("		<td align='center' rowspan=" + ((int)iColCnt + 1).ToString() + " ><img src= " + @"""" + strBoxPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                        m_Sw.WriteLine("		<td align='center' rowspan=" + ((int)iColCnt + 1).ToString() + " ><img src= " + @"""" + strRawDataPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                    }
                                    
                                }
                                else
                                {
                                    if (arrHistogramPath.Length == 0)
                                    {
                                        if (strBoxPlotPath != null && strBoxPlotPath != string.Empty)
                                        {
                                            m_Sw.WriteLine("		<td align='center' rowspan=" + ((int)iColCnt + 1).ToString() + " ><img src= " + @"""" + strBoxPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                            m_Sw.WriteLine("		<td align='center' rowspan=" + ((int)iColCnt + 1).ToString() + " ><img src= " + @"""" + strRawDataPlotPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</td>");
                                        }
                                    }
                                }
                            }
                        }
                        m_Sw.WriteLine("	</tr>");
                        
                    }
                }
                
                m_Sw.WriteLine("</table>");
               
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Correlation

        public void BodyCorrelation(DACrux.BStats.Statistics.Correlation.CorrelationResult[,] dctResult, string[] arrColumnNames, bool bPvalue, bool bSpearman, bool bScatter)
        {
            int iColCnt;  //컬럼이름 개수
            int iTemp;  // 배열의 길이 임시 저장
            try
            {
                if(dctResult == null)
                    throw new ArgumentException(ErrorParameter("dctResult"));
                if (arrColumnNames == null)
                    throw new ArgumentException(ErrorParameter("arrColumnNames "));
                iColCnt = arrColumnNames.Length;
                iTemp = dctResult.Length;
                if (iTemp != iColCnt * iColCnt)
                    throw new ArgumentException(m_ErrorColNameNotEqual);

                m_Sw.WriteLine("<!----------------------------- Contents Format 1  ----------------------------->");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<table border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");


                for (int i = 0; i < iColCnt; i++)  // Row
                {
                    #region 컬럼명 작성
                    if (i == 0)  //ColumnName
                    {
                        m_Sw.WriteLine("	<tr align='center' height='22'>");
                        m_Sw.WriteLine("<td bgcolor = '#6699cc' bordercolordark='#6699cc'></td>");
                        for (int j = 0; j < iColCnt; j++)
                        {
                            m_Sw.WriteLine("<td align='center' bgcolor = '#6699cc' bordercolordark='#6699cc'><font color='#000000'>" + arrColumnNames[j] + "</font></td>");
                        }
                        m_Sw.WriteLine("	</tr>");
                    }
                    #endregion

                    m_Sw.WriteLine("	<tr align='center' height='22'>");
                    #region Row Name 
                    m_Sw.WriteLine("<td align='center' bgcolor = '#6699cc' bordercolordark='#6699cc' width='70'><font color='#000000'>" + arrColumnNames[i] + "</font></td>");
                    #endregion
                    for (int j = 0; j < iColCnt; j++)  // Col
                    {
                        if (i == j)
                        {
                            m_Sw.WriteLine("<td bgcolor = '#eeeeee' bordercolordark='#eeeeee'></td>");
                        }
                        else  //하나의 셀에 들어갈 내용
                        {
                            m_Sw.WriteLine("<td align='left' bgcolor = '#eeeeee' bordercolordark='#eeeeee'>");
                            m_Sw.WriteLine("&nbsp;" + m_PearsonCorrelation + " : " + dctResult[i, j].PearsonCorrelation.ToString());
                            if (bPvalue)
                            {
                                m_Sw.WriteLine("(" + dctResult[i, j].Pvalue.ToString() + ")");
                            }
                            
                            if (bSpearman)
                            {
                                m_Sw.WriteLine("<br>" + "&nbsp;" + m_Spearman + " : " + dctResult[i, j].SpearmanRho.ToString());
                                if (bPvalue)
                                {
                                    m_Sw.WriteLine("(" + dctResult[i, j].SpearmanPvalue.ToString() + ")");
                                }
                            }
                            if(bScatter)
                                m_Sw.WriteLine("<br><img src= " + @"""" + dctResult[i,j].GraphInfo.ImagePath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;");

                            m_Sw.WriteLine("</td>");
                        }
                    }
                    m_Sw.WriteLine("	</tr>");
                }
                m_Sw.WriteLine("</table>");

                m_Sw.WriteLine("<!----------------------------------- Description -------------------------------->");
                m_Sw.WriteLine("<table width='100%' border='0' cellspacing='1' cellpadding='1'>");
                m_Sw.WriteLine("	<tr> ");
                m_Sw.WriteLine("		<td height='22' align='left' bgcolor='#FFFFFF'><b>&nbsp;&nbsp;※ Cell Contents : Coefficient of correlation(P-Value)</b></td>");
                m_Sw.WriteLine("	</tr>");
                m_Sw.WriteLine("</table>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CapaContinuous
        public void BodyCapaContinuous(DACrux.BStats.Core.ResultCapaContinuous oResult, DACrux.BStats.Core.DisplayType oPPM, DACrux.BStats.Core.StatisticType oCapa, string strHistogramPath)
        {
            string strSpace = m_Space2;
            int iProcessSubTitleWidth = 200 ;
            int iPerformanceSubTitleWidth = 90;
            int iPerformanceValueWidth = 100;
            int iCapaSubTitleWidth = 100;
            int iCapaValueWidth = 160;
            try
            {
                m_Sw.WriteLine("<!----------------------------- Contents Format 1  ----------------------------->");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<TABLE border='0' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");

                m_Sw.WriteLine("<TR>");
                m_Sw.WriteLine("	<TD>");
                m_Sw.WriteLine("		<TABLE border=0 cellpadding=0 cellspacing=0>");
                m_Sw.WriteLine("			<TR>");
                m_Sw.WriteLine("				<!-------------------------------- Histogram -------------------------------------->");
                m_Sw.WriteLine("				<td width=400 height=300 bgcolor=#eeeeee align=center valign=middle><img src= " + @"""" + strHistogramPath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</TD>");
                m_Sw.WriteLine("				<!---------------------------------------------------------------------------------->");
                m_Sw.WriteLine("				<TD width=10></TD>");
                m_Sw.WriteLine("				<TD width=390 height=300 valign=top>");

                #region PROCESS DATA
                m_Sw.WriteLine("					<TABLE border=0 cellpadding=0 cellspacing=0>");
                m_Sw.WriteLine("						<TR>");
                m_Sw.WriteLine("							<TD colspan=3 valign=top>");
                m_Sw.WriteLine("								<TABLE border='1' cellspacing='0' width=100% bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD colspan=2 align=center bgcolor = '#ff99aa' bordercolordark='#ff99aa'>" + m_ProcessData + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iProcessSubTitleWidth.ToString() + ">" + m_LSL + "</TD>");
                if (double.IsNaN(oResult.LSL))
                    m_Sw.WriteLine("										<TD align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right>" + oResult.LSL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iProcessSubTitleWidth.ToString() + ">" + m_Target + "</TD>");
                if (double.IsNaN(oResult.Target))
                    m_Sw.WriteLine("										<TD align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right>" + oResult.Target.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iProcessSubTitleWidth.ToString() + ">" + m_USL + "</TD>");
                if (double.IsNaN(oResult.USL))
                    m_Sw.WriteLine("										<TD align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right>" + oResult.USL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iProcessSubTitleWidth.ToString() + ">" + m_SampleMean + "</TD>");
                if (double.IsNaN(oResult.Mean))
                    m_Sw.WriteLine("										<TD align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right>" + oResult.Mean.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iProcessSubTitleWidth.ToString() + ">" + m_SampleN + "</TD>");
                m_Sw.WriteLine("										<TD align=right>" + oResult.N.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iProcessSubTitleWidth.ToString() + ">" + m_SigmaBetween + "</TD>");
                if (double.IsNaN(oResult.Std_Between))
                    m_Sw.WriteLine("										<TD align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right>" + oResult.Std_Between.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iProcessSubTitleWidth.ToString() + ">" + m_SigmaWithin + "</TD>");
                if (double.IsNaN(oResult.Std_Within))
                    m_Sw.WriteLine("										<TD align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right>" + oResult.Std_Within.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iProcessSubTitleWidth.ToString() + ">" + m_SigmaBW + "</TD>");
                if (double.IsNaN(oResult.Std_BW))
                    m_Sw.WriteLine("										<TD align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right>" + oResult.Std_BW.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iProcessSubTitleWidth.ToString() + ">" + m_SigmaOverall + "</TD>");
                if (double.IsNaN(oResult.Std_Overall))
                    m_Sw.WriteLine("										<TD align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right>" + oResult.Std_Overall.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");

                m_Sw.WriteLine("								</TABLE>");
                #endregion
                
                m_Sw.WriteLine("							</TD>");
                m_Sw.WriteLine("						</TR>");
                m_Sw.WriteLine("						<TR height=10>");
                m_Sw.WriteLine("							<TD colspan=3></TD>");
                m_Sw.WriteLine("						</TR>");
                m_Sw.WriteLine("						<TR height=70>");
                m_Sw.WriteLine("							<TD width=190 valign=top>");

                #region Obs Performance

                m_Sw.WriteLine("								<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD colspan=2 align=center bgcolor = '#6699cc' bordercolordark='#6699cc'>"+m_ObsPerformance+"</TD>");

                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                if (oPPM == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMlessLSL + "</TD>");
                else
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMlessLSLper + "</TD>");
                if (double.IsNaN(oResult.Observed_PPMlessLSL))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.Observed_PPMlessLSL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                if (oPPM == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMmoreUSL + "</TD>");
                else
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMmoreUSLper + "</TD>");
                if (double.IsNaN(oResult.Observed_PPMmoreUSL))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.Observed_PPMmoreUSL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");

                

                m_Sw.WriteLine("									<TR>");
                if (oPPM == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMtotal + "</TD>");
                else
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMtotalper + "</TD>");
                if (double.IsNaN(oResult.Observed_PPMtotal))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.Observed_PPMtotal.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");

                m_Sw.WriteLine("								</TABLE>");

                #endregion

                m_Sw.WriteLine("							</TD>");
                m_Sw.WriteLine("							<TD width=10></TD>");
                m_Sw.WriteLine("							<TD width=190 valign=top>");

                #region Within Performance

                m_Sw.WriteLine("								<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD colspan=2 align=center bgcolor = '#6699cc' bordercolordark='#6699cc'>" + m_WithinPerformance + "</TD>");

                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                if (oPPM == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMlessLSL + "</TD>");
                else
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMlessLSLper + "</TD>");
                if (double.IsNaN(oResult.Within_PPMlessLSL))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.Within_PPMlessLSL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                if (oPPM == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMmoreUSL + "</TD>");
                else
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMmoreUSLper + "</TD>");
                if (double.IsNaN(oResult.Within_PPMmoreUSL))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.Within_PPMmoreUSL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");

                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMtotal + "</TD>");
                if (double.IsNaN(oResult.Within_PPMtotal))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.Within_PPMtotal.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");

                m_Sw.WriteLine("								</TABLE>");

                #endregion

                m_Sw.WriteLine("							</TD>");
                m_Sw.WriteLine("						</TR>");
                m_Sw.WriteLine("						<TR height=10>");
                m_Sw.WriteLine("							<TD colspan=3></TD>");
                m_Sw.WriteLine("						</TR>");
                m_Sw.WriteLine("						<TR height=70>");
                m_Sw.WriteLine("							<TD width=190 valign=top>");

                #region BW Performance

                m_Sw.WriteLine("								<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD colspan=2 align=center bgcolor = '#6699cc' bordercolordark='#6699cc'>" + m_BWPerformance + "</TD>");

                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                if (oPPM == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMlessLSL + "</TD>");
                else
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMlessLSLper + "</TD>");
                if (double.IsNaN(oResult.BW_PPMlessLSL))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.BW_PPMlessLSL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                if (oPPM == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMmoreUSL + "</TD>");
                else
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMmoreUSLper + "</TD>");
                if (double.IsNaN(oResult.BW_PPMmoreUSL))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.BW_PPMmoreUSL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");

                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMtotal + "</TD>");
                if (double.IsNaN(oResult.BW_PPMtotal))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.BW_PPMtotal.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");

                m_Sw.WriteLine("								</TABLE>");

                #endregion

                m_Sw.WriteLine("							</TD>");
                m_Sw.WriteLine("							<TD width=10></TD>");
                m_Sw.WriteLine("							<TD width=190 valign=top>");

                #region Overall Performance

                m_Sw.WriteLine("								<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");
                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD colspan=2 align=center bgcolor = '#6699cc' bordercolordark='#6699cc'>" + m_OverallPerformance + "</TD>");

                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                if (oPPM == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMlessLSL + "</TD>");
                else
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMlessLSLper + "</TD>");
                if (double.IsNaN(oResult.Overall_PPMlessLSL))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.Overall_PPMlessLSL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");
                m_Sw.WriteLine("									<TR>");
                if (oPPM == DACrux.BStats.Core.DisplayType.Parts_per_million)
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMmoreUSL + "</TD>");
                else
                    m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMmoreUSLper + "</TD>");
                if (double.IsNaN(oResult.Overall_PPMmoreUSL))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.Overall_PPMmoreUSL.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");

                m_Sw.WriteLine("									<TR>");
                m_Sw.WriteLine("										<TD bgcolor=#dddddd  bordercolordark='#dddddd' width=" + iPerformanceSubTitleWidth + " align=center>" + m_PPMtotal + "</TD>");
                if (double.IsNaN(oResult.Overall_PPMtotal))
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("										<TD align=right width=" + iPerformanceValueWidth.ToString() + ">" + oResult.Overall_PPMtotal.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("									</TR>");

                m_Sw.WriteLine("								</TABLE>");

                #endregion


                m_Sw.WriteLine("							</TD>");
                m_Sw.WriteLine("						</TR>");
                m_Sw.WriteLine("					</TABLE>");
                m_Sw.WriteLine("				</TD>");
                m_Sw.WriteLine("			</TR>");
                m_Sw.WriteLine("		</TABLE>");
                m_Sw.WriteLine("	</TD>");
                m_Sw.WriteLine("</TR>");
                m_Sw.WriteLine("<TR height=10><TD></TD></TR>");
                m_Sw.WriteLine("<TR>");
                m_Sw.WriteLine("	<TD>");
                m_Sw.WriteLine("		<TABLE border=0 cellpadding=0 cellspacing=0 height=200>");
                m_Sw.WriteLine("		<TR>");
                m_Sw.WriteLine("			<TD width=260 valign=top>");

                #region Within Capability
                m_Sw.WriteLine("				<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");
                m_Sw.WriteLine("					<TR>");
                m_Sw.WriteLine("						<TD colspan=2 align=center bgcolor = '#33CC99' bordercolordark='#33CC99'>" + m_WithinCapability + "</TD>");
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                if (oCapa == DACrux.BStats.Core.StatisticType.Capability_stats)
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Cp + "</TD>");
                    if (double.IsNaN(oResult.CpWithin) || double.IsInfinity(oResult.CpWithin))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CpWithin.ToString() + strSpace + "</TD>");
                }
                else
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Zbench + "</TD>");
                    if (double.IsNaN(oResult.ZbenchWithin) || double.IsInfinity(oResult.ZbenchWithin))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.ZbenchWithin.ToString() + strSpace + "</TD>");
                }
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                if (oCapa == DACrux.BStats.Core.StatisticType.Capability_stats)
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_CPL + "</TD>");
                    if (double.IsNaN(oResult.CPLWithin) || double.IsInfinity(oResult.CPLWithin))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CPLWithin.ToString() + strSpace + "</TD>");
                }
                else
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Zlsl + "</TD>");
                    if (double.IsNaN(oResult.ZlslWithin) || double.IsInfinity(oResult.ZlslWithin))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.ZlslWithin.ToString() + strSpace + "</TD>");
                }
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                if (oCapa == DACrux.BStats.Core.StatisticType.Capability_stats)
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_CPU + "</TD>");
                    if (double.IsNaN(oResult.CPUWithin) || double.IsInfinity(oResult.CPUWithin))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CPUWithin.ToString() + strSpace + "</TD>");
                }
                else
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Zusl + "</TD>");
                    if (double.IsNaN(oResult.ZuslWithin) || double.IsInfinity(oResult.ZuslWithin))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.ZuslWithin.ToString() + strSpace + "</TD>");

                }
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Cpk + "</TD>");
                if (double.IsNaN(oResult.CpkWithin) || double.IsInfinity(oResult.CpkWithin))
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CpkWithin.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_CCpk + "</TD>");
                if (double.IsNaN(oResult.CCpkWithin) || double.IsInfinity(oResult.CCpkWithin))
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CCpkWithin.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("				</TABLE>");

                #endregion

                m_Sw.WriteLine("			</TD>");
                m_Sw.WriteLine("			<TD width=10></TD>");
                m_Sw.WriteLine("			<TD width=260 valign=top>");

                #region BW Capability


                m_Sw.WriteLine("				<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");
                m_Sw.WriteLine("					<TR>");
                m_Sw.WriteLine("						<TD colspan=2 align=center bgcolor = '#33CC99' bordercolordark='#33CC99'>" + m_BWCapability + "</TD>");
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                if (oCapa == DACrux.BStats.Core.StatisticType.Capability_stats)
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Cp + "</TD>");
                    if (double.IsNaN(oResult.CpBW) || double.IsInfinity(oResult.CpBW))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CpBW.ToString() + strSpace + "</TD>");
                }
                else
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Zbench + "</TD>");
                    if (double.IsNaN(oResult.ZbenchBW) || double.IsInfinity(oResult.ZbenchBW))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.ZbenchBW.ToString() + strSpace + "</TD>");
                }
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                if (oCapa == DACrux.BStats.Core.StatisticType.Capability_stats)
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_CPL + "</TD>");
                    if (double.IsNaN(oResult.CPLBW) || double.IsInfinity(oResult.CPLBW))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CPLBW.ToString() + strSpace + "</TD>");
                }
                else
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Zlsl + "</TD>");
                    if (double.IsNaN(oResult.ZlslBW) || double.IsInfinity(oResult.ZlslBW))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.ZlslBW.ToString() + strSpace + "</TD>");
                }
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                if (oCapa == DACrux.BStats.Core.StatisticType.Capability_stats)
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_CPU + "</TD>");
                    if (double.IsNaN(oResult.CPUBW) || double.IsInfinity(oResult.CPUBW))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CPUBW.ToString() + strSpace + "</TD>");
                }
                else
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Zusl + "</TD>");
                    if (double.IsNaN(oResult.ZuslBW) || double.IsInfinity(oResult.ZuslBW))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.ZuslBW.ToString() + strSpace + "</TD>");
                }
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                
                m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Cpk + "</TD>");
                if (double.IsNaN(oResult.CpkBW) || double.IsInfinity(oResult.CpkBW))
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CpkBW.ToString() + strSpace + "</TD>");

                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_CCpk + "</TD>");
                if (double.IsNaN(oResult.CCpkBW) || double.IsInfinity(oResult.CCpkBW))
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.CCpkBW.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("				</TABLE>");

                #endregion

                m_Sw.WriteLine("			</TD>");
                m_Sw.WriteLine("			<TD width=10></TD>");
                m_Sw.WriteLine("			<TD width=260 valign=top>");

                #region Overall Capability

                m_Sw.WriteLine("				<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");
                m_Sw.WriteLine("					<TR>");
                m_Sw.WriteLine("						<TD colspan=2 align=center bgcolor = '#33CC99' bordercolordark='#33CC99'>" + m_OverallCapability + "</TD>");
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                if (oCapa == DACrux.BStats.Core.StatisticType.Capability_stats)
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Pp + "</TD>");
                    if (double.IsNaN(oResult.Pp) || double.IsInfinity(oResult.Pp))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.Pp.ToString() + strSpace + "</TD>");
                }
                else
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Zbench + "</TD>");
                    if (double.IsNaN(oResult.ZbenchOverall) || double.IsInfinity(oResult.ZbenchOverall))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.ZbenchOverall.ToString() + strSpace + "</TD>");
                }
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                if (oCapa == DACrux.BStats.Core.StatisticType.Capability_stats)
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_PPL + "</TD>");
                    if (double.IsNaN(oResult.PPL) || double.IsInfinity(oResult.PPL))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.PPL.ToString() + strSpace + "</TD>");
                }
                else
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Zlsl + "</TD>");
                    if (double.IsNaN(oResult.ZlslOverall) || double.IsInfinity(oResult.ZlslOverall))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.ZlslOverall.ToString() + strSpace + "</TD>");
                }
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                if (oCapa == DACrux.BStats.Core.StatisticType.Capability_stats)
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_PPU + "</TD>");
                    if (double.IsNaN(oResult.PPU) || double.IsInfinity(oResult.PPU))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.PPU.ToString() + strSpace + "</TD>");
                }
                else
                {
                    m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Zusl + "</TD>");
                    if (double.IsNaN(oResult.ZuslOverall) || double.IsInfinity(oResult.ZuslOverall))
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                    else
                        m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.ZuslOverall.ToString() + strSpace + "</TD>");
                }
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Ppk + "</TD>");
                if (double.IsNaN(oResult.Ppk) || double.IsInfinity(oResult.Ppk))
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.Ppk.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("					<TR>");
                m_Sw.WriteLine("						<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center width=" + iCapaSubTitleWidth.ToString() + ">" + m_Cpm + "</TD>");
                if (double.IsNaN(oResult.Cpm) || double.IsInfinity(oResult.Cpm))
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>*" + strSpace + "</TD>");
                else
                    m_Sw.WriteLine("						<TD width=" + iCapaValueWidth.ToString() + " align=right>" + oResult.Cpm.ToString() + strSpace + "</TD>");
                m_Sw.WriteLine("					</TR>");

                m_Sw.WriteLine("				</TABLE>");

                

                #endregion


                m_Sw.WriteLine("			</TD>");
                m_Sw.WriteLine("		</TR>");
                m_Sw.WriteLine("		</TABLE>");
                m_Sw.WriteLine("	</TD>");
                m_Sw.WriteLine("</TR>");
                m_Sw.WriteLine("</TABLE>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Regression

        private string GetRsquared(DataTable dtRsquared)
        {
            string strReturn = string.Empty;
            try
            {
                if (dtRsquared == null)
                    throw new ArgumentException("dtRsquared is null.");
                for (int i = 0; i < dtRsquared.Columns.Count; i++)
                {
                    if (dtRsquared.Columns[i].ColumnName == "R-Square" || dtRsquared.Columns[i].ColumnName == "Adj.R-Square")
                    {
                        float tmpVal = float.Parse(dtRsquared.Rows[0][i].ToString())  * 100;
                        strReturn += dtRsquared.Columns[i].ColumnName + " = " + string.Format("{0:#0.00}%",tmpVal) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                    else
                    {
                        strReturn += dtRsquared.Columns[i].ColumnName + " = " + dtRsquared.Rows[0][i].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                }
                return strReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BodyRegression(DACrux.BStats.Core.ResultRegression oResult, DACrux.BStats.Core.RegressionType oType, DACrux.ProjectManager.UI.GraphInformation[] oGraph, string strResponse, string[] arrPredictors)
        {
            string strSubTitle;
            try
            {
                switch (oType)
                {
                    //case DACrux.BStats.Core.RegressionType.All:
                    //    break;
                    case DACrux.BStats.Core.RegressionType.Stepwise:
                        strSubTitle = "Regression(stepwise) : ";
                        break;
                    case DACrux.BStats.Core.RegressionType.Forward:
                        strSubTitle = "Regression(forward) : ";
                        break;
                    case DACrux.BStats.Core.RegressionType.Backward:
                        strSubTitle = "Regression(backward) : ";
                        break;
                    default:
                        strSubTitle = "Regression Analysis : ";
                        break;
                }
                strSubTitle += strResponse + " versus " + string.Join(", ", arrPredictors);

                m_Sw.WriteLine("<!----------------------------- Contents Format 1  ----------------------------->");
                m_Sw.WriteLine("<br>");
                m_Sw.WriteLine("<B>" + strSubTitle + "</B><BR><BR>");
                m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' cellpadding = 10 width=700>");
                m_Sw.WriteLine("	<TR>");
                m_Sw.WriteLine("		<TD align=left bgcolor = '#eeeeee' bordercolordark='#eeeeee'>");
                if (oResult.ErrorInformation == null ||oResult.ErrorInformation == string.Empty)
                {
                    m_Sw.WriteLine("		<B>[" + m_RegressionEquation + "]</B><BR>");
                    m_Sw.WriteLine("        " + oResult.RegressionEquation);
                    if (oResult.TableofRsquared != null)
                    {
                        m_Sw.WriteLine("		<BR><BR>");
                        m_Sw.WriteLine("		<B>[" + oResult.TableofRsquared.TableName + "]</B><BR>");

                        m_Sw.WriteLine("        " + GetRsquared(oResult.TableofRsquared)) ;
                    }
                }
                else
                {
                    m_Sw.WriteLine("		<B>" + m_ErrorRegressionData + "</B>" +"("+ oResult.ErrorInformation+")");
                }
                m_Sw.WriteLine("		</TD>");
                m_Sw.WriteLine("	</TR>");
                m_Sw.WriteLine("</TABLE>");
                m_Sw.WriteLine("<BR><BR>");

                if (oResult.TableofCoefficients != null)
                {
                    m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' width=700>");
                    m_Sw.WriteLine("	<TR>");
                    m_Sw.WriteLine("		<TD colspan=5 align=center bgcolor = '#336699' bordercolordark='#336699'><FONT COLOR='#ffffff'><B>" + oResult.TableofCoefficients .TableName+ "</B></FONT></TD>");
                    m_Sw.WriteLine("	</TR>");
                    for (int i = -1; i < oResult.TableofCoefficients.Rows.Count; i++)
                    {
                        if (i == -1)
                        {
                            m_Sw.WriteLine("	<TR bgcolor='#6699cc' bordercolordark='#6699cc' align=center>");
                            for(int j=0; j<oResult.TableofCoefficients.Columns.Count; j++)
                                m_Sw.WriteLine("		<TD width=138><FONT COLOR='#ffffff'><B>" + oResult.TableofCoefficients.Columns[j].ColumnName+ "</B></FONT></TD>");
                            m_Sw.WriteLine("	</TR>");
                        }
                        else
                        {
                            m_Sw.WriteLine("	<TR>");
                            for (int j = 0; j < oResult.TableofCoefficients.Columns.Count; j++)
                            {
                                if (j == 0)
                                    m_Sw.WriteLine("		<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center>" + oResult.TableofCoefficients.Rows[i][j].ToString() + "</TD>");
                                else
                                    m_Sw.WriteLine("		<TD align=right>" + oResult.TableofCoefficients.Rows[i][j].ToString() + "&nbsp;&nbsp;</TD>");  //   
                            }
                            m_Sw.WriteLine("	</TR>");
                        }
                    }
                    m_Sw.WriteLine("</TABLE>");
                    m_Sw.WriteLine("<BR><BR>");
                }


                if (oResult.TableofAnova != null)
                {
                    m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' width=700>");
                    m_Sw.WriteLine("	<TR>");
                    m_Sw.WriteLine("		<TD colspan=6 align=center bgcolor = '#336699' bordercolordark='#336699'><FONT COLOR='#ffffff'><B>" + oResult.TableofAnova.TableName + "</B></FONT></TD>");
                    m_Sw.WriteLine("	</TR>");
                    for (int i = -1; i < oResult.TableofAnova.Rows.Count; i++)
                    {
                        if (i == -1)
                        {
                            m_Sw.WriteLine("	<TR bgcolor='#6699cc' bordercolordark='#6699cc' align=center>");
                            for (int j = 0; j < oResult.TableofAnova.Columns.Count; j++)
                                m_Sw.WriteLine("		<TD width=110><FONT COLOR='#ffffff'><B>" + oResult.TableofAnova.Columns[j].ColumnName + "</B></FONT></TD>");
                            m_Sw.WriteLine("	</TR>");
                        }
                        else
                        {
                            m_Sw.WriteLine("	<TR>");
                            for (int j = 0; j < oResult.TableofAnova.Columns.Count; j++)
                            {
                                if (j == 0)
                                    m_Sw.WriteLine("		<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center>" + oResult.TableofAnova.Rows[i][j].ToString() + "</TD>");
                                else
                                    m_Sw.WriteLine("		<TD align=right>" + oResult.TableofAnova.Rows[i][j].ToString() + "&nbsp;&nbsp;</TD>");  //   
                            }
                            m_Sw.WriteLine("	</TR>");
                        }
                    }
                    m_Sw.WriteLine("</TABLE>");
                    m_Sw.WriteLine("<BR><BR>");
                }


                if (oResult.TableofFitsResiduals != null)
                {
                    m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' width=700>");
                    m_Sw.WriteLine("	<TR>");
                    m_Sw.WriteLine("		<TD colspan=5 align=center bgcolor = '#336699' bordercolordark='#336699'><FONT COLOR='#ffffff'><B>" + oResult.TableofFitsResiduals.TableName + "</B></FONT></TD>");
                    m_Sw.WriteLine("	</TR>");
                    for (int i = -1; i < oResult.TableofFitsResiduals.Rows.Count; i++)
                    {
                        if (i == -1)
                        {
                            m_Sw.WriteLine("	<TR bgcolor='#6699cc' bordercolordark='#6699cc' align=center>");
                            for (int j = 0; j < oResult.TableofFitsResiduals.Columns.Count; j++)
                                m_Sw.WriteLine("		<TD width=138><FONT COLOR='#ffffff'><B>" + oResult.TableofFitsResiduals.Columns[j].ColumnName + "</B></FONT></TD>");
                            m_Sw.WriteLine("	</TR>");
                        }
                        else
                        {
                            m_Sw.WriteLine("	<TR>");
                            for (int j = 0; j < oResult.TableofFitsResiduals.Columns.Count; j++)
                            {
                                if (j == 0)
                                    m_Sw.WriteLine("		<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center>" + oResult.TableofFitsResiduals.Rows[i][j].ToString() + "</TD>");
                                else
                                    m_Sw.WriteLine("		<TD align=right>" + oResult.TableofFitsResiduals.Rows[i][j].ToString() + "&nbsp;&nbsp;</TD>");  //   
                            }
                            m_Sw.WriteLine("	</TR>");
                        }
                    }
                    m_Sw.WriteLine("</TABLE>");
                    m_Sw.WriteLine("<BR><BR>");
                }
                if (oGraph != null && oGraph.Length > 0)
                {
                    m_Sw.WriteLine("<TABLE border='0' cellspacing='00' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' cellpadding=0 width=700>");
                    for (int i = 0; i < oGraph.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            m_Sw.WriteLine("	<TR height=370>");
                            m_Sw.WriteLine("		<TD width=10></TD>");
                            m_Sw.WriteLine("		<TD align=center bgcolor = '#eeeeee' bordercolordark='#eeeeee' width=370 valign=middle><img src= " + @"""" + oGraph[i].ImagePath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</TD>");
                        }
                        else
                        {
                            m_Sw.WriteLine("		<TD width=10></TD>");
                            m_Sw.WriteLine("		<TD align=center bgcolor = '#eeeeee' bordercolordark='#eeeeee' width=370 valign=middle><img src= " + @"""" + oGraph[i].ImagePath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</TD>");
                            m_Sw.WriteLine("		<TD width=10></TD>");
                            m_Sw.WriteLine("	</TR>");
                        }
                    }
                    if (oGraph.Length % 2 == 1)
                    {
                        m_Sw.WriteLine("		<TD width=10></TD>");
                        m_Sw.WriteLine("		<TD align=center bgcolor = '#eeeeee' bordercolordark='#eeeeee' width=370 valign=middle><FONT SIZE=7 COLOR=#dddddd><B>No Graph</B></FONT></TD> ");
                        m_Sw.WriteLine("		<TD width=10></TD>");
                        m_Sw.WriteLine("	</TR>");
                    }
                    m_Sw.WriteLine("</TABLE>");
                    m_Sw.WriteLine("<BR>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ANOVA

        public void BodyOneWayANOVA(DACrux.BStats.Core.ResultAnova oResult, DACrux.ProjectManager.UI.GraphInformation[] oGraph)
        {
            //oResult.TableforGraphs;
            //oResult.TableofAnova;
            //oResult.TableofRsquared;
            //oResult.TableofStatistic;
            //oResult.ErrorInformation;

            m_Sw.WriteLine("<!----------------------------- Contents Format 1  ----------------------------->");
            m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' cellpadding = 10 width=700>");
            m_Sw.WriteLine("	<TR>");
            m_Sw.WriteLine("		<TD align=left bgcolor = '#eeeeee' bordercolordark='#eeeeee'>");
            if (oResult.ErrorInformation == null || oResult.ErrorInformation == string.Empty)
            {               
                if (oResult.TableofRsquared != null)
                {
                    m_Sw.WriteLine("		<B>[" + oResult.TableofRsquared.TableName + "]</B><BR>");

                    m_Sw.WriteLine("        " + GetRsquared(oResult.TableofRsquared));
                }
            }
            else
            {
                m_Sw.WriteLine("		<B>" + m_ErrorAnovaData + "</B>" + "(" + oResult.ErrorInformation + ")");
            }
            m_Sw.WriteLine("		</TD>");
            m_Sw.WriteLine("	</TR>");
            m_Sw.WriteLine("</TABLE>");
            m_Sw.WriteLine("<BR><BR>");

            #region Statistic

            if (oResult.TableofStatistic != null)
            {
                m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' width=700>");
                m_Sw.WriteLine("	<TR>");
                m_Sw.WriteLine("		<TD colspan=6 align=center bgcolor = '#336699' bordercolordark='#336699'><FONT COLOR='#ffffff'><B>" + oResult.TableofStatistic.TableName + "</B></FONT></TD>");
                m_Sw.WriteLine("	</TR>");
                for (int i = -1; i < oResult.TableofStatistic.Rows.Count; i++)
                {
                    if (i == -1)
                    {
                        m_Sw.WriteLine("	<TR bgcolor='#6699cc' bordercolordark='#6699cc' align=center>");
                        for (int j = 0; j < oResult.TableofStatistic.Columns.Count; j++)
                            m_Sw.WriteLine("		<TD width=138><FONT COLOR='#ffffff'><B>" + oResult.TableofStatistic.Columns[j].ColumnName + "</B></FONT></TD>");
                        m_Sw.WriteLine("	</TR>");
                    }
                    else
                    {
                        m_Sw.WriteLine("	<TR>");
                        for (int j = 0; j < oResult.TableofStatistic.Columns.Count; j++)
                        {
                            if (j == 0)
                                m_Sw.WriteLine("		<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center>" + oResult.TableofStatistic.Rows[i][j].ToString() + "</TD>");
                            else
                                m_Sw.WriteLine("		<TD align=right>" + oResult.TableofStatistic.Rows[i][j].ToString() + "&nbsp;&nbsp;</TD>");  //   
                        }
                        m_Sw.WriteLine("	</TR>");
                    }
                }
                m_Sw.WriteLine("</TABLE>");
                m_Sw.WriteLine("<BR><BR>");
            }

            #endregion

            #region Anova

            if (oResult.TableofAnova != null)
            {
                m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' width=700>");
                m_Sw.WriteLine("	<TR>");
                m_Sw.WriteLine("		<TD colspan=6 align=center bgcolor = '#336699' bordercolordark='#336699'><FONT COLOR='#ffffff'><B>" + oResult.TableofAnova.TableName + "</B></FONT></TD>");
                m_Sw.WriteLine("	</TR>");
                for (int i = -1; i < oResult.TableofAnova.Rows.Count; i++)
                {
                    if (i == -1)
                    {
                        m_Sw.WriteLine("	<TR bgcolor='#6699cc' bordercolordark='#6699cc' align=center>");
                        for (int j = 0; j < oResult.TableofAnova.Columns.Count; j++)
                            m_Sw.WriteLine("		<TD width=110><FONT COLOR='#ffffff'><B>" + oResult.TableofAnova.Columns[j].ColumnName + "</B></FONT></TD>");
                        m_Sw.WriteLine("	</TR>");
                    }
                    else
                    {
                        m_Sw.WriteLine("	<TR>");
                        for (int j = 0; j < oResult.TableofAnova.Columns.Count; j++)
                        {
                            if (j == 0)
                                m_Sw.WriteLine("		<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center>" + oResult.TableofAnova.Rows[i][j].ToString() + "</TD>");
                            else
                                m_Sw.WriteLine("		<TD align=right>" + oResult.TableofAnova.Rows[i][j].ToString() + "&nbsp;&nbsp;</TD>");  //   
                        }
                        m_Sw.WriteLine("	</TR>");
                    }
                }
                m_Sw.WriteLine("</TABLE>");
                m_Sw.WriteLine("<BR><BR>");
            }

            #endregion

            #region Graph

            if (oGraph != null && oGraph.Length > 0)
            {
                m_Sw.WriteLine("<TABLE border='0' cellspacing='00' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' cellpadding=0 width=700>");
                for (int i = 0; i < oGraph.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        m_Sw.WriteLine("	<TR height=370>");
                        m_Sw.WriteLine("		<TD width=10></TD>");
                        m_Sw.WriteLine("		<TD align=center bgcolor = '#eeeeee' bordercolordark='#eeeeee' width=370 valign=middle><img src= " + @"""" + oGraph[i].ImagePath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</TD>");
                    }
                    else
                    {
                        m_Sw.WriteLine("		<TD width=10></TD>");
                        m_Sw.WriteLine("		<TD align=center bgcolor = '#eeeeee' bordercolordark='#eeeeee' width=370 valign=middle><img src= " + @"""" + oGraph[i].ImagePath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</TD>");
                        m_Sw.WriteLine("		<TD width=10></TD>");
                        m_Sw.WriteLine("	</TR>");
                    }
                }
                if (oGraph.Length % 2 == 1)
                {
                    m_Sw.WriteLine("		<TD width=10></TD>");
                    m_Sw.WriteLine("		<TD align=center bgcolor = '#eeeeee' bordercolordark='#eeeeee' width=370 valign=middle><FONT SIZE=7 COLOR=#dddddd><B>No Graph</B></FONT></TD> ");
                    m_Sw.WriteLine("		<TD width=10></TD>");
                    m_Sw.WriteLine("	</TR>");
                }
                m_Sw.WriteLine("</TABLE>");
                m_Sw.WriteLine("<BR>");
            }

            #endregion

        }

        #endregion

        #region Hypothesis testing
        public void BodyHypothesisTesting(DACrux.BStats.Core.ResultHypothesisTesting oResult, DACrux.BStats.Core.TestingType oType, DACrux.BStats.Core.AlternativeType oAlternativeType ,bool bStatistics, bool bConfidenceInterval, bool bCriticalValue)
        {
            string strSubTitle;
            string strHypothesis;
            string strEqual;
            string strCI;
            string strCriticalValue;
            string strTestResult;
            string strLevel;
            try
            {
                #region SubTitle
                switch (oType)
                {
                    case DACrux.BStats.Core.TestingType.OneSampleT:
                        strSubTitle = "One sample T test : " + oResult.VariableName1;
                        break;
                    case DACrux.BStats.Core.TestingType.TwoSamplePaired:
                        strSubTitle = "Paired two sample T test : " + oResult.VariableName1 + ", " + oResult.VariableName2;
                        break;
                    case DACrux.BStats.Core.TestingType.TwoSampleUnpaired:
                        strSubTitle = "Unpaired two sample T test : " + oResult.VariableName1 + ", " + oResult.VariableName2;
                        break;
                    default: //case DACrux.BStats.Core.TestingType.OneSampleZ:
                        strSubTitle = "One sample Z test : " + oResult.VariableName1;
                        break;
                }
                SubTitle(strSubTitle);
                #endregion

                #region Level Make
                switch (oAlternativeType)
                {
                    case DACrux.BStats.Core.AlternativeType.Greater:
                        strLevel = Convert.ToString((1 - oResult.Alpha) * 100);                        
                        break;
                    case DACrux.BStats.Core.AlternativeType.Less:
                        strLevel = Convert.ToString((1 - oResult.Alpha) * 100);
                        break;
                    default://case DACrux.BStats.Core.AlternativeType.NotEqual:
                        strLevel = Convert.ToString((1 - (oResult.Alpha / 2)) * 100);
                        break;
                }
                #endregion

                #region Equal Make && CI && CriticalValue
                switch (oAlternativeType)
                {
                    case DACrux.BStats.Core.AlternativeType.Greater:
                        strCI = "Lower(" + strLevel + "%) = " + oResult.LowerConfidenceLimit.ToString();
                        strCriticalValue = "Left = " + oResult.LeftCriticalValue.ToString();
                        strEqual = ">";
                        break;
                    case DACrux.BStats.Core.AlternativeType.Less:
                        strCI = "Upper(" + strLevel + "%) = " + oResult.UpperConfidenceLimit.ToString();
                        strCriticalValue = "Right = " + oResult.RightCriticalValue.ToString();
                        strEqual = "<";
                        break;
                    default://case DACrux.BStats.Core.AlternativeType.NotEqual:
                        strCI = "Lower(" + strLevel + "%) = " + oResult.LowerConfidenceLimit.ToString() + "      Upper(" + strLevel + "%) = " + oResult.UpperConfidenceLimit.ToString();
                        strCriticalValue = "Left = " + oResult.LeftCriticalValue.ToString() + "      Right = " + oResult.RightCriticalValue.ToString();
                        strEqual = "not =";
                        break;
                }
                #endregion

                //Result
                strTestResult = oResult.StatisticName + " = " + oResult.Statistic.ToString() + "      " + "P-Value = " + oResult.P.ToString();


                m_Sw.WriteLine("<!----------------------------- Contents Format 1  ----------------------------->");
                m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' cellpadding = 10 width=700>");
                m_Sw.WriteLine("	<TR>");
                m_Sw.WriteLine("		<TD align=left bgcolor = '#eeeeee' bordercolordark='#eeeeee'>");
                if (oResult.ErrorInformation == null || oResult.ErrorInformation == string.Empty)
                {
                    //귀무가설  Mean, Std, Difference를 이용해서
                    //검정 결과 Pvalue와 통계량을 이용해서
                    //신뢰구간
                    //임계값  CriticalValue
                    // Alpha
                    //oResult.EstimateForDifference
                    //oResult.PooledStDev
                    switch (oType)
                    {
                        #region 1T
                        case DACrux.BStats.Core.TestingType.OneSampleT:
                            #region Hypothesis
                            strHypothesis = "mu = " + oResult.TestingMean.ToString() + " VS mu " + strEqual + " " + oResult.TestingMean.ToString();
                            
                            #endregion                                                    
                            break;
                        #endregion

                        #region 2P
                        case DACrux.BStats.Core.TestingType.TwoSamplePaired:
                            #region Hypothesis
                            strHypothesis = "difference = " + oResult.TestingDifference.ToString() + " VS difference " + strEqual + " " + oResult.TestingDifference.ToString();
                            #endregion
                            break;
                        #endregion

                        #region 2U
                        case DACrux.BStats.Core.TestingType.TwoSampleUnpaired:
                            #region Hypothesis
                            strHypothesis = "difference = " + oResult.TestingDifference.ToString() + " VS difference " + strEqual + " " + oResult.TestingDifference.ToString();
                            #endregion
                            break;
                        #endregion

                        #region 1Z
                        default: //case DACrux.BStats.Core.TestingType.OneSampleZ:
                            #region Hypothesis
                            strHypothesis = "mu = " + oResult.TestingMean.ToString() + " VS mu " + strEqual + " " + oResult.TestingMean.ToString() + "            The assumed standard deviation = " + oResult.TestingStd.ToString();
                            #endregion
                            break;
                        #endregion
                    }
                    m_Sw.WriteLine("		<B>[" + m_Hypothesis + "]</B><BR>");
                    m_Sw.WriteLine("        " + strHypothesis.Replace(" ", m_Space1) + "<BR>");
                    m_Sw.WriteLine("		<BR><B>[" + m_TestResult + "]</B><BR>");
                    m_Sw.WriteLine("        " + strTestResult.Replace(" ", m_Space1) + "<BR>");
                    if (bConfidenceInterval)
                    {
                        m_Sw.WriteLine("		<BR><B>[" + m_CI + "]</B><BR>");
                        m_Sw.WriteLine("        " + strCI.Replace(" ", m_Space1) + "<BR>");
                    }
                    if (bCriticalValue)
                    {
                        m_Sw.WriteLine("		<BR><B>[" + m_CriticalValue + "]</B><BR>");
                        m_Sw.WriteLine("        " + strCriticalValue.Replace(" ", m_Space1) + "<BR>");
                    }
                }
                else
                {
                    m_Sw.WriteLine("		<B>" + m_ErrorAnovaData + "</B>" + "(" + oResult.ErrorInformation + ")");
                }
                
          
                m_Sw.WriteLine("		</TD>");
                m_Sw.WriteLine("	</TR>");
                m_Sw.WriteLine("</TABLE>");
                m_Sw.WriteLine("<BR><BR>");

                #region Statistic
                if (bStatistics)
                {
                    if (oResult.dtDescriptiveStat != null)
                    {
                        m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' width=700>");
                        m_Sw.WriteLine("	<TR>");
                        m_Sw.WriteLine("		<TD colspan=6 align=center bgcolor = '#336699' bordercolordark='#336699'><FONT COLOR='#ffffff'><B>" + oResult.dtDescriptiveStat.TableName + "</B></FONT></TD>");
                        m_Sw.WriteLine("	</TR>");
                        for (int i = -1; i < oResult.dtDescriptiveStat.Rows.Count; i++)
                        {
                            if (i == -1)
                            {
                                m_Sw.WriteLine("	<TR bgcolor='#6699cc' bordercolordark='#6699cc' align=center>");
                                for (int j = 0; j < oResult.dtDescriptiveStat.Columns.Count; j++)
                                    m_Sw.WriteLine("		<TD width=138><FONT COLOR='#ffffff'><B>" + oResult.dtDescriptiveStat.Columns[j].ColumnName + "</B></FONT></TD>");
                                m_Sw.WriteLine("	</TR>");
                            }
                            else
                            {
                                m_Sw.WriteLine("	<TR>");
                                for (int j = 0; j < oResult.dtDescriptiveStat.Columns.Count; j++)
                                {
                                    if (j == 0)
                                        m_Sw.WriteLine("		<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center>" + oResult.dtDescriptiveStat.Rows[i][j].ToString() + "</TD>");
                                    else
                                        m_Sw.WriteLine("		<TD align=right>" + oResult.dtDescriptiveStat.Rows[i][j].ToString() + "&nbsp;&nbsp;</TD>");  //   
                                }
                                m_Sw.WriteLine("	</TR>");
                            }
                        }
                        m_Sw.WriteLine("</TABLE>");
                        m_Sw.WriteLine("<BR><BR>");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Taguchi
        public void BodyTaguchi(DataSet oResult, TaguchiAnalysisRule oRule, DACrux.ProjectManager.UI.GraphInformation[] oGraph)
        {
            //string strSubTitle;
            //string strHypothesis;
            //string strEqual;
            //string strCI;
            //string strCriticalValue;
            //string strTestResult;
            //string strLevel;

            List<string> lstFactors = new List<string>();
            int iLevels = 0;
            try
            {

                // SubTitle
                m_Sw.WriteLine("<!----------------------------- Contents Format 1  ----------------------------->");
                m_Sw.WriteLine("<TABLE border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999' cellpadding = 10 >");

                for (int g = 0; g < oGraph.Length; g++)
                {
                    m_Sw.WriteLine("	<TR>");
                    m_Sw.WriteLine("		<TD colspan=5 align=center bgcolor = '#336699' bordercolordark='#336699'><FONT COLOR='#ffffff'><B>");
                    m_Sw.WriteLine("		    <B>");
                    m_Sw.WriteLine(oGraph[g].Title);
                    m_Sw.WriteLine("		    </B></FONT></TD>");
                    m_Sw.WriteLine("	</TR>");

                    lstFactors.Clear();

                    /// Header
                    m_Sw.WriteLine("	<TR>");
                    m_Sw.WriteLine("	    <TD>");
                    m_Sw.WriteLine("	        <TABLE width='600' border='1' cellspacing='0' bordercolor='#999999' bordercolordark='white' bordercolorlight='#999999'>");
                    m_Sw.WriteLine("	            <TR bgcolor='#6699cc' bordercolordark='#6699cc' align=center>");
                    m_Sw.WriteLine("		            <TD width=200><FONT COLOR='#ffffff'><B>수준</TD>");
                    foreach (DataTable dt in oResult.Tables)
                    {
                        string strTableName = dt.TableName;
                        if (strTableName.StartsWith("ANALYSIS_"))
                        {
                            if(iLevels == 0) iLevels = oResult.Tables[strTableName].Rows.Count;
                            lstFactors.Add(strTableName.Replace("ANALYSIS_", ""));
                            m_Sw.WriteLine(string.Format("		<TD width=200><FONT COLOR='#ffffff'><B>{0}</TD>", strTableName.Replace("ANALYSIS_", "")));
                        }
                    }
                    m_Sw.WriteLine("	    </TR>");


                    /// Value
                    for (int v = 0; v < iLevels; v++)
                    {
                        m_Sw.WriteLine("	    <TR>");
                        for (int i = 0; i < lstFactors.Count; i++)
                        {
                            DataTable dt = oResult.Tables["ANALYSIS_" + lstFactors[i]];
                            if (i == 0)
                            {
                                m_Sw.WriteLine(string.Format("		        <TD bgcolor=#dddddd bordercolordark='#dddddd' align=center>{0}</TD>", dt.Rows[v][lstFactors[i]].ToString()));
                            }

                            double tmpValue = double.NaN;
                            if (double.TryParse(dt.Rows[v][oGraph[g].AxisXTitle].ToString(), out tmpValue) == true)
                            {
                                tmpValue = Math.Round(tmpValue, 3);
                                m_Sw.WriteLine(string.Format("		        <TD align=right>{0}</TD>", tmpValue));
                            }
                            else
                            {
                                m_Sw.WriteLine("		        <TD align=right>NaN</TD>");
                            }
                        }
                        m_Sw.WriteLine("	    </TR>");
                    }

                    /// Delta
                    DataView dv = oResult.Tables["DELTA"].DefaultView;
                    m_Sw.WriteLine("	<TR>");
                    m_Sw.WriteLine("		<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center>DELTA</TD>");
                    string Rank = "		<TD bgcolor=#dddddd bordercolordark='#dddddd' align=center>RANK</TD>";
                    for (int i = 0; i < lstFactors.Count; i++)
                    {
                        dv.RowFilter = string.Format("FACTOR = '{0}' AND VALUETYPE='{1}'", lstFactors[i], oGraph[g].AxisXTitle);

                        double tmpDelta = double.NaN;
                        if (double.TryParse(dv[0]["DELTA"].ToString(), out tmpDelta) == true)
                        {
                            tmpDelta = Math.Round(tmpDelta, 3);
                            m_Sw.WriteLine(string.Format("		        <TD align=right>{0}</TD>", tmpDelta));
                        }
                        else
                        {
                            m_Sw.WriteLine("		        <TD align=right>NaN</TD>");
                        }

                        Rank = Rank + string.Format("		<TD align=right>{0}</TD>", dv[0]["RANK"].ToString());
                    }
                    m_Sw.WriteLine("	</TR>");
                    m_Sw.WriteLine("	<TR>");
                    m_Sw.WriteLine(Rank);
                    m_Sw.WriteLine("	</TR>");
                    m_Sw.WriteLine("	</TABLE>");
                    m_Sw.WriteLine("	</TD>");


                    /// Graph
                    m_Sw.WriteLine("	<TR>");
                    m_Sw.WriteLine("		<TD align=center colspan='3' bgcolor = '#ffffff' bordercolordark='#eeeeee' valign=middle><img src= " + @"""" + oGraph[g].ImagePath.Replace(@"\", "/") + @"""" + @"alt = ""Ctrl + Click""" + " >&nbsp;</TD>");
                    m_Sw.WriteLine("	</TR>");
                }
                m_Sw.WriteLine("</TABLE>");
                m_Sw.WriteLine("<BR><BR>");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #endregion

        #region [ Save ]
        /// <summary>
        /// 구성된 내용의 저장을 마무리한다.
        /// </summary>
        public void SaveHtml()
        {
            try
            {
                m_Sw.WriteLine("</body>");
                m_Sw.WriteLine("</html>");
                m_Sw.Close();
                m_Sw.Dispose();
                m_Sw = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private string ErrorParameter(string strParameter)
        {
            try
            {
                if (strParameter == string.Empty)
                    strParameter = "parameter";

                return string.Format("Argument({0}) is null.", strParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
