using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Drawing;

namespace DACrux.SP.Controls
{
    public class SyntaxRichTextBox : System.Windows.Forms.RichTextBox
    {
        private SyntaxSettings m_settings = new SyntaxSettings();
        private static bool m_bPaint = true;
        private string m_strLine = "";
        private int m_nContentLength = 0;
        private int m_nLineLength = 0;
        private int m_nLineStart = 0;
        private int m_nLineEnd = 0;
        private string m_strKeywords = "";
        private int m_nCurSelection = 0;

        /// <summary>
        /// The settings.
        /// </summary>
        public SyntaxSettings Settings
        {
            get { return m_settings; }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x00f)
            {
                if (m_bPaint)
                    base.WndProc(ref m);
                else
                    m.Result = IntPtr.Zero;
            }
            else
                base.WndProc(ref m);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // Calculate shit here.
            m_nContentLength = this.TextLength;

            int nCurrentSelectionStart = SelectionStart;
            int nCurrentSelectionLength = SelectionLength;

            m_bPaint = false;

            // Find the start of the current line.
            m_nLineStart = nCurrentSelectionStart;
            while ((m_nLineStart > 0) && (Text[m_nLineStart - 1] != '\n'))
                m_nLineStart--;
            // Find the end of the current line.
            m_nLineEnd = nCurrentSelectionStart;
            while ((m_nLineEnd < Text.Length) && (Text[m_nLineEnd] != '\n'))
                m_nLineEnd++;
            // Calculate the length of the line.
            m_nLineLength = m_nLineEnd - m_nLineStart;
            // Get the current line.
            m_strLine = Text.Substring(m_nLineStart, m_nLineLength);

            // Process this line.
            ProcessLine();

            m_bPaint = true;
        }

        private void ProcessRegex(string strRegex, Color color)
        {
            Regex regKeywords = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match regMatch;

            for (regMatch = regKeywords.Match(m_strLine); regMatch.Success; regMatch = regMatch.NextMatch())
            {
                // Process the words
                int nStart = m_nLineStart + regMatch.Index;
                int nLenght = regMatch.Length;
                SelectionStart = nStart;
                SelectionLength = nLenght;
                SelectionColor = color;
            }
        }
        /// <summary>
        /// Compiles the keywords as a regular expression.
        /// </summary>
        public void CompileKeywords()
        {
            for (int i = 0; i < Settings.Keywords.Count; i++)
            {
                string strKeyword = "\\b" + Settings.Keywords[i] + "\\b";

                strKeyword = strKeyword.Replace("\\b:", ":");

                if (i == Settings.Keywords.Count - 1)
                    m_strKeywords += strKeyword;
                else
                    m_strKeywords += strKeyword + "|";
            }
        }

        public void ProcessAllLines()
        {
            m_bPaint = false;

            int nStartPos = 0;
            int i = 0;
            int nOriginalPos = SelectionStart;
            while (i < Lines.Length)
            {
                m_strLine = Lines[i];
                m_nLineStart = nStartPos;
                m_nLineEnd = m_nLineStart + m_strLine.Length;

                ProcessLine();
                i++;

                nStartPos += m_strLine.Length + 1;
            }

            m_bPaint = true;
        }
        /// <summary>
        /// Process a line.
        /// </summary>
        private void ProcessLine()
        {
            // Save the position and make the whole line black
            int nPosition = SelectionStart;
            SelectionStart = m_nLineStart;
            SelectionLength = m_nLineLength;
            SelectionColor = Color.Black;

            // Process the keywords
            ProcessRegex(m_strKeywords, Settings.KeywordColor);
            // Process numbers
            if (Settings.EnableIntegers)
                ProcessRegex("\\b(?:[0-9]*\\.)?[0-9]+\\b", Settings.IntegerColor);
            // Process strings
            if (Settings.EnableStrings)
                ProcessRegex("\"[^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*\"", Settings.StringColor);
            // Process comments
            if (Settings.EnableComments && !string.IsNullOrEmpty(Settings.Comment))
                ProcessRegex(Settings.Comment + ".*$", Settings.CommentColor);

            if (Settings.EnableDBParameter && !string.IsNullOrEmpty(Settings.DBParameter))
                ProcessRegex(Settings.DBParameter, Settings.DBParameterColor);

            if (Settings.EnableDBDynamicQuery && !string.IsNullOrEmpty(Settings.DBDynamicQuery))
                ProcessRegex(Settings.DBDynamicQuery, Settings.DBDynamicQueryColor);

            SelectionStart = nPosition;
            SelectionLength = 0;
            SelectionColor = Color.Black;

            m_nCurSelection = nPosition;
        }
    }

    /// <summary>
    /// Class to store syntax objects in.
    /// </summary>
    public class SyntaxList
    {
        public List<string> m_rgList = new List<string>();
        public Color m_color = new Color();
    }

    /// <summary>
    /// Settings for the keywords and colors.
    /// </summary>
    public class SyntaxSettings
    {
        SyntaxList m_rgKeywords = new SyntaxList();
        string m_strDBParameter = "";
        Color m_colorDBParameter = Color.AliceBlue;

        string m_strDBDynamicQuery = "";
        Color m_colorDBDynamicQuery = Color.DarkOrange;

        string m_strComment = "--";
        Color m_colorComment = Color.Green;
        Color m_colorString = Color.Gray;
        Color m_colorInteger = Color.Red;
        bool m_bEnableDBParameter = true;
        bool m_bEnableDBDynamicQuery = true;
        bool m_bEnableComments = true;
        bool m_bEnableIntegers = true;
        bool m_bEnableStrings = true;
        
        public string DBParameter
        {
            get { return m_strDBParameter; }
            set { m_strDBParameter = value; }
        }
        public Color DBParameterColor
        {
            get { return m_colorDBParameter; }
            set { m_colorDBParameter = value; }
        }
        
        public string DBDynamicQuery
        {
            get { return m_strDBDynamicQuery; }
            set { m_strDBDynamicQuery = value; }
        }
        public Color DBDynamicQueryColor
        {
            get { return m_colorDBDynamicQuery; }
            set { m_colorDBDynamicQuery = value; }
        }

        public List<string> Keywords
        {
            get { return m_rgKeywords.m_rgList; }
        }
        public Color KeywordColor
        {
            get { return m_rgKeywords.m_color; }
            set { m_rgKeywords.m_color = value; }
        }
        public string Comment
        {
            get { return m_strComment; }
            set { m_strComment = value; }
        }
        public Color CommentColor
        {
            get { return m_colorComment; }
            set { m_colorComment = value; }
        }
        public bool EnableDBParameter
        {
            get { return m_bEnableDBParameter; }
            set { m_bEnableDBParameter = value; }
        }
        public bool EnableDBDynamicQuery
        {
            get { return m_bEnableDBDynamicQuery; }
            set { m_bEnableDBDynamicQuery = value; }
        }
        public bool EnableComments
        {
            get { return m_bEnableComments; }
            set { m_bEnableComments = value; }
        }
        public bool EnableIntegers
        {
            get { return m_bEnableIntegers; }
            set { m_bEnableIntegers = value; }
        }
        public bool EnableStrings
        {
            get { return m_bEnableStrings; }
            set { m_bEnableStrings = value; }
        }
        public Color StringColor
        {
            get { return m_colorString; }
            set { m_colorString = value; }
        }
        public Color IntegerColor
        {
            get { return m_colorInteger; }
            set { m_colorInteger = value; }
        }
        
        
    }

    public static class RtfColoring
    {
        public static void ProcessRtfColoring(SyntaxRichTextBox richTextBox)
        {
            SyntaxSettings Settings = richTextBox.Settings;
            if (Settings.Keywords.Count < 1)
                return;

            // reset the text to get a clear rtf
            string strTextToAdd = richTextBox.Text;
            richTextBox.Clear();
            richTextBox.AppendText(strTextToAdd);
            string strRTF = richTextBox.Rtf;

            // find index of start of header
            int iRTFLoc = strRTF.IndexOf("\\rtf");

            // get index of where we'll insert the colour table
            // try finding opening bracket of first property of header first
            int iInsertLoc = strRTF.IndexOf('{', iRTFLoc);

            // if there is no property, we'll insert colour table
            // just before the end bracket of the header
            if (iInsertLoc == -1) iInsertLoc = strRTF.IndexOf('}', iRTFLoc) - 1;

            string strCommentColor = "\\red" + Settings.CommentColor.R.ToString() +
            "\\green" + Settings.CommentColor.G.ToString() +
            "\\blue" + Settings.CommentColor.B.ToString();

            string strStringColor = "\\red" + Settings.StringColor.R.ToString() +
            "\\green" + Settings.StringColor.G.ToString() +
            "\\blue" + Settings.StringColor.B.ToString();

            string strKeywordColor = "\\red" + Settings.KeywordColor.R.ToString() +
            "\\green" + Settings.KeywordColor.G.ToString() +
            "\\blue" + Settings.KeywordColor.B.ToString();

            string strIntegerColor = "\\red" + Settings.IntegerColor.R.ToString() +
            "\\green" + Settings.IntegerColor.G.ToString() +
            "\\blue" + Settings.IntegerColor.B.ToString();

            // insert the colour table at our chosen location
            strRTF = strRTF.Insert(iInsertLoc, "{\\colortbl ;" + strCommentColor + ";" + strStringColor + ";" + strKeywordColor + ";" + strIntegerColor + ";}");

            // build the keywords regex
            string strKeywords = String.Empty;
            foreach (string keyword in Settings.Keywords)
            {
                strKeywords += keyword + ",";
            }
            strKeywords = strKeywords.Substring(0, strKeywords.Length - 1);

            Regex r = new Regex(@", ?");
            strKeywords = @"\b(" + r.Replace(strKeywords, @"|") + @")\b";

            // start coloring
            // Keywords
            r = new Regex(strKeywords, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            strRTF = r.Replace(strRTF, new MatchEvaluator(MatchKeyword));

            // Integers
            r = new Regex("(\\b(?:[0-9]*\\.)?[0-9]+\\b)", RegexOptions.IgnoreCase);
            strRTF = r.Replace(strRTF, new MatchEvaluator(MatchInteger));

            // Comments
            r = new Regex("(" + Settings.Comment + ".*$)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            strRTF = r.Replace(strRTF, new MatchEvaluator(MatchComment));

            // Strings
            r = new Regex("(\"[^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*\"|\'[^\'\\\\\\r\\n]*(?:\\\\.[^\'\\\\\\r\\n]*)*\')", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            strRTF = r.Replace(strRTF, new MatchEvaluator(MatchString));

            richTextBox.Rtf = strRTF;
        }

        private static string MatchKeyword(Match match)
        {
            if (match.Groups[1].Success)
            {
                return @"\cf3 " + RemoveRtfColors(match.Value) + @"\cf0 ";
            }

            return String.Empty;
        }

        private static string MatchString(Match match)
        {
            if (match.Groups[1].Success)
            {
                return @"\cf2 " + RemoveRtfColors(match.Value) + @"\cf0 ";
            }

            return String.Empty;
        }

        private static string MatchComment(Match match)
        {
            if (match.Groups[1].Success)
            {
                return @"\cf1 " + RemoveRtfColors(match.Value) + @"\cf0 ";
            }

            return String.Empty;
        }

        private static string MatchInteger(Match match)
        {
            if (match.Groups[1].Success)
            {
                return @"\cf4 " + RemoveRtfColors(match.Value) + @"\cf0 ";
            }

            return String.Empty;
        }

        /// <summary> /// remove all rtf-colors from a string
        /// </summary> /// <param name="strText"></param> /// <returns>String</returns> 
        private static string RemoveRtfColors(string strText)
        {
            Regex r = new Regex(@"\\cf[0-9] ");
            strText = r.Replace(strText, "");

            return strText;
        }
    }
}