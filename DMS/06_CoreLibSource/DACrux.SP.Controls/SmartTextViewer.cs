using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DACrux.SP.Controls
{
    public partial class SmartTextViewer : UserControl
    {
        #region " Member Field "

        private readonly int DEFAULT_LINE_COUNT_PER_PAGE = 500;

        private readonly string HEADER = @"{\rtf1\ansi\ansicpg1252\deff0{\fonttbl{\f0\fswiss\fcharset0 Consolas;}{\f1\fnil\fcharset129 \'b1\'bc\'b8\'b2\'c3\'bc;}{\f2\fbidi \fswiss\fcharset128\fprq1 Consolas;}{\f3\fbidi \fswiss\fcharset129\fprq1 Consolas{\*\panose 020b0609000101010101}\'b1\'bc\'b8\'b2\'c3\'bc;}{\f4\fbidi \fmodern\fcharset0\fprq1 GulimChe Western;}}{\colortbl ;";
        private readonly string HEADER_COLOR = @"\red255\green255\blue255;\red0\green0\blue0;\red252\green124\blue124;";
        private readonly string HEADER_CLOSURE = @"}\viewkind4\uc1\pard\cf0\highlight1\lang1042\f0\fs18 ";

        private readonly string SELECTION_START = @"\f4\cf2\highlight3 ";
        private readonly string SELECTION_END = @"\cf0\highlight1 ";

        private readonly string BACKSLASH_SINGLE = @"\";
        private readonly string BACKSLASH_DOUBLE = @"\\";

        private readonly string NEWLINE = "\n";
        private readonly string NEWLINE_RTF = "\\par\n";

        private readonly string FOOTER = @"\cf0\highlight1\f1\par}";

        private string[] arrLines = null;
        private string strTotalContent = string.Empty;

        private Dictionary<int, List<int[]>> dicPageSelection = new Dictionary<int, List<int[]>>();
        private Dictionary<int, string> dicPageContent = new Dictionary<int, string>();
        private Dictionary<int, int[]> dicPageLength = new Dictionary<int, int[]>();

        #endregion

        #region " Property "

        public event PageLister.PageChangeHandler PageChanged
        {
            add
            {
                pageLister.PageChanged += value;
            }
            remove
            {
                pageLister.PageChanged -= value;
            }
        }

        public RichTextBox TextBox
        {
            get { return txtContent; }
        }

        public string TotalContent
        {
            get { return strTotalContent; }
            set
            {
                strTotalContent = value;

                Reset();
            }
        }

        #endregion

        #region " Creator "

        public SmartTextViewer()
        {
            InitializeComponent();

            pageLister.PagesCount = 0;
            pageLister.PageChanged += pageNo => ShowPage(pageNo);

            //txtContent.ReadOnly = true;
        }

        #endregion

        #region " Method "

        private void Reset()
        {
            try
            {
                dicPageLength.Clear();

                txtContent.Text = string.Empty;
                pageLister.PagesCount = 0;
                arrLines = null;

                if (strTotalContent.IsNullOrEmpty())
                    return;
               
                arrLines = strTotalContent.Split(new string[] { NEWLINE }, StringSplitOptions.None);

                int iDiv = (int)(arrLines.Length / DEFAULT_LINE_COUNT_PER_PAGE);
                pageLister.PagesCount = arrLines.Length % DEFAULT_LINE_COUNT_PER_PAGE > 0 ? iDiv + 1 : iDiv;
                pageLister.CurrentPage = 1;

                for (int i = 1; i <= pageLister.PagesCount; i++)
                {
                    int maxLine = (i == pageLister.PagesCount) ? arrLines.Length : i * DEFAULT_LINE_COUNT_PER_PAGE;
                    dicPageContent[i] = string.Join(NEWLINE, arrLines, (i - 1) * DEFAULT_LINE_COUNT_PER_PAGE, maxLine - (i - 1) * DEFAULT_LINE_COUNT_PER_PAGE);

                    if (i == 1)
                        dicPageLength.Add(i, new int[] { 0, dicPageContent[i].Length + 1 });
                    else
                        dicPageLength.Add(i, new int[] { dicPageLength.Sum(kv => kv.Value[1]), dicPageContent[i].Length + 1 });
                }

                ShowPage(pageLister.CurrentPage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowPage(int pageNo)
        {
            try
            {
                if (strTotalContent.IsNullOrEmpty())
                    return;

                SetText(pageNo);

                uclLineNo.LineOffset = (pageNo - 1) * DEFAULT_LINE_COUNT_PER_PAGE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Current Page Select Text from index, length
        /// </summary>
        /// <param name="selection">[0]:index, [1]:length</param>
        public void CaculatePageSelection(List<int[]> selection)
        {
            try
            {
                dicPageSelection.Clear();

                if (string.IsNullOrEmpty(strTotalContent))
                    return;

                if (selection == null || selection.Count < 1)
                    return;

                int[] arrPagePos = null;
                foreach (int[] pos in selection)
                {
                    if (pos[1] == 0)
                        continue;

                    arrPagePos = GetRelativePos(pos[0]); //Return Value(arrPagePos) = [0]:pageNo, [1]:position in the page relatively

                    if (!dicPageSelection.ContainsKey(arrPagePos[0]))
                        dicPageSelection.Add(arrPagePos[0], new List<int[]>());

                    if ((dicPageLength[arrPagePos[0]][0] + dicPageLength[arrPagePos[0]][1]) < pos[0] + pos[1]) // 페이지 사이에 선택영역이 걸쳐서 존재할 때
                    {
                        if (!dicPageSelection.ContainsKey(arrPagePos[0] + 1))
                            dicPageSelection.Add(arrPagePos[0] + 1, new List<int[]>());

                        int iBeforeLength = dicPageLength[arrPagePos[0]][0] + dicPageLength[arrPagePos[0]][1] - pos[0] - 1; // 현재 페이지 끝까지
                        int iAfterLength = pos[1] - iBeforeLength - 1; // 선택구간 다음 페이지 나머지까지

                        dicPageSelection[arrPagePos[0]].Add(new int[] { arrPagePos[1], iBeforeLength });
                        dicPageSelection[arrPagePos[0] + 1].Insert(0, new int[] { 0, iAfterLength });
                    }
                    else
                        dicPageSelection[arrPagePos[0]].Add(new int[] { arrPagePos[1], pos[1] }); // arrPagePos[1]:position in the page, pos[1]:length of the selection
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int[] GetRelativePos(int pos)
        {
            int iRetPage = 1;

            for (int i = 1; i <= dicPageLength.Count; i++)
            {
                if (pos >= dicPageLength[i][0] && pos < dicPageLength[i][0] + dicPageLength[i][1])
                {
                    iRetPage = i;
                    break;
                }
            }

            if (iRetPage == 1)
                return new int[] { iRetPage, pos };
            else
                return new int[] { iRetPage, pos - dicPageLength[iRetPage][0] };
        }

        private void ResetSelectionColor()
        {
            txtContent.SuspendLayout();

            txtContent.SelectAll();
            txtContent.SelectionBackColor = Color.White;
            txtContent.DeselectAll();

            txtContent.ResumeLayout();
        }

        /// <summary>
        /// Current Page Select Text from index, length
        /// </summary>
        /// <param name="selection">[0]:index, [1]:length</param>
        public void SelectText(List<int[]> selection)
        {
            try
            {
                CaculatePageSelection(selection);
                ResetSelectionColor();
                //SetText(pageLister.CurrentPage);

                if (dicPageSelection.Count > 0)
                {
                    var firstSelectedPage = (from a in dicPageSelection
                                             select a.Key).Min();

                    SetText(firstSelectedPage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SelectText(int start, int length)
        {
            List<int[]> lst = new List<int[]>();
            lst.Add(new int[] { start, length });

            SelectText(lst);
        }

        private void SetText(int pageNo)
        {
            int pt = 0;
            int[] pos;

            string strCurrentPageText = string.Empty;
            StringBuilder sb = null;

            try
            {
                if (!dicPageContent.ContainsKey(pageNo))
                    return;

                if (dicPageSelection == null || dicPageSelection.Count < 1 || !dicPageSelection.ContainsKey(pageNo) || dicPageSelection[pageNo].Count < 1)
                {
                    txtContent.Text = dicPageContent[pageNo].ToString();
                    txtContent.Select(0, 0);
                    //txtContent.SelectAll();
                    //txtContent.Font = new Font("Arial", 9);
                    txtContent.ScrollToCaret();
                    return;
                }

                strCurrentPageText = dicPageContent[pageNo].ToString();
                sb = new StringBuilder(strCurrentPageText.Length * 2);
                int i = 0;

                try
                {
                    for (; i < dicPageSelection[pageNo].Count; i++)
                    {
                        pos = dicPageSelection[pageNo][i];
                        //txtContent.Select(pos[0], pos[1]);
                        //txtContent.SelectionBackColor = Color.FromArgb(0, 252, 124, 124);
                        sb.Append(strCurrentPageText.Substring(pt, pos[0] - pt).Replace(BACKSLASH_SINGLE, BACKSLASH_DOUBLE)); // 처음 Match 전까지 문자열 추가
                        sb.Append(SELECTION_START + strCurrentPageText.Substring(pos[0], pos[1]).Replace(BACKSLASH_SINGLE, BACKSLASH_DOUBLE) + SELECTION_END); // 처음 Match 문자열 추가
                        pt = pos[0] + pos[1]; // 다음 위치로 포인터 이동
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                sb.Append(strCurrentPageText.Substring(pt, strCurrentPageText.Length - pt).Replace(BACKSLASH_SINGLE, BACKSLASH_DOUBLE));
                txtContent.Rtf = HEADER + HEADER_COLOR + HEADER_CLOSURE + sb.Replace(NEWLINE, NEWLINE_RTF).Replace("}", @"\}").Replace("{", @"\{").ToString() + FOOTER;

                //txtContent.Rtf = "";
                //txtContent.Rtf = HEADER + HEADER_COLOR + HEADER_CLOSURE + sb.Replace(NEWLINE, NEWLINE_RTF).ToString() + FOOTER;
                //txtContent.SelectAll();
                //txtContent.Font = new Font("Arial", 9);
                txtContent.Select(dicPageSelection[pageNo][0][0], 0);
                txtContent.ScrollToCaret();
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
