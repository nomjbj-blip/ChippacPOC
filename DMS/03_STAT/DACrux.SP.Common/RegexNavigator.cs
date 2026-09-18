using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DACrux.SP.Common
{
    public class RegexNavigator
    {
        #region " Member Field "

        string strTargetContent = string.Empty;

        private Dictionary<RegexEntity, List<Token>> dicEntityCapture = new Dictionary<RegexEntity, List<Token>>();
        private Dictionary<RegexSection, List<Token>> dicSectionCapture = new Dictionary<RegexSection, List<Token>>();

        #endregion

        #region " Property "

        public Dictionary<RegexEntity, List<DACrux.SP.Common.Token>> EntityCaptureCollection
        {
            get { return dicEntityCapture; }
        }

        public Dictionary<RegexSection, List<DACrux.SP.Common.Token>> SectionCaptureCollection
        {
            get { return dicSectionCapture; }
        }

        public string TargetText
        {
            get { return strTargetContent; }
            set { strTargetContent = value; }
        }

        #endregion

        #region " Creator "

        public RegexNavigator()
        {
        }

        public RegexNavigator(string targetContent)
        {
            strTargetContent = targetContent;
        }

        #endregion

        #region " Event Handler "

        void section_SectionBoundaryEntityChanged(RegexSection section)
        {
            if (!dicSectionCapture.ContainsKey(section))
                return;

            if (string.IsNullOrEmpty(strTargetContent))
                return;

            string strRegex = GetSectionRegexString(section);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // GetCaptures(RegexEntity entity) 메서드를 써도 되지만 추후 필요시 변경예정
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var captures = from Match match in Regex.Matches(strTargetContent, strRegex, section.StartEntity.RegexOption)
                           select new Token() { Index = match.Index, Length = match.Length };

            dicSectionCapture[section] = captures.ToList();
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        void entity_RegexChanged(RegexEntity entity)
        {
            if (!dicEntityCapture.ContainsKey(entity))
                return;

            if (string.IsNullOrEmpty(strTargetContent))
                return;

            dicEntityCapture[entity] = GetCaptures(entity);
        }

        #endregion

        #region " Method "

        public List<Token> GetCaptures(RegexEntity entity, string valueGroupName)
        {
            List<Token> lstReturn = GetCaptures(entity.RegexString, valueGroupName, entity.RegexOption);
            return lstReturn;
        }

        public List<Token> GetCaptures(RegexEntity entity)
        {
            List<Token> lstReturn = GetCaptures(entity.RegexString, entity.RegexValueGroupName, entity.RegexOption);
            return lstReturn;
        }

        public List<Token> GetCaptures(string strRegex, string strGroupName, RegexOptions regexOptions)
        {
            List<Token> lstReturn = GetCaptures(ref strTargetContent, strRegex, strGroupName, regexOptions);
            return lstReturn;
        }
        //정규식 구문
        public static List<Token> GetCaptures(ref string strTargetContent, string strRegex, string strGroupName, RegexOptions regexOptions)
        {
            List<Token> lstCapture = new List<Token>();

            Regex regex = new Regex(strRegex, regexOptions | RegexOptions.Compiled );

            if (!string.IsNullOrEmpty(strGroupName))
                lstCapture = (from Match match in regex.Matches(strTargetContent)
                              select new Token() { Index = match.Groups[strGroupName].Index, Length = match.Groups[strGroupName].Length }).ToList();
            else
            {
                MatchCollection matches = regex.Matches(strTargetContent);

                for (int i = 0; i < matches.Count; i++)
                    lstCapture.Add(new Token() { Index = matches[i].Index, Length = matches[i].Length });
            }

            return lstCapture;
        }

        public List<int[]> GetPositionInfo(RegexEntity entity)
        {
            try
            {
                if (!dicEntityCapture.ContainsKey(entity))
                    return null;

                var lst = from Token mt in dicEntityCapture[entity]
                          select new int[] { mt.Index, mt.Length };

                return lst.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<int[]> GetPositionInfo(RegexSection section)
        {
            try
            {
                if (!dicSectionCapture.ContainsKey(section))
                    return null;

                var lst = from Token mt in dicSectionCapture[section]
                          select new int[] { mt.Index, mt.Length };

                return lst.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetEntity(RegexEntity entity)
        {
            if (!dicEntityCapture.ContainsKey(entity))
                dicEntityCapture.Add(entity, null);

            if (string.IsNullOrEmpty(strTargetContent))
                return;

            dicEntityCapture[entity] = GetCaptures(entity);

            entity.RegexChanged += new RegexChangedEventHandler(entity_RegexChanged);
        }

        public void SetSection(RegexSection section)
        {
            if (!dicSectionCapture.ContainsKey(section))
                dicSectionCapture.Add(section, null);

            if (string.IsNullOrEmpty(strTargetContent))
                return;

            string strRegex = GetSectionRegexString(section);

            dicSectionCapture[section] = GetCaptures(strRegex, string.Empty, section.StartEntity.RegexOption);

            section.SectionBoundaryEntityChanged += section_SectionBoundaryEntityChanged;
        }

        private string GetSectionRegexString(RegexSection section)
        {
            string strRegex = section.StartEntity.RegexString;

            if (section.EndEntity != null)
                strRegex += ".*?" + "(" + section.EndEntity.RegexString + "){1}?";
            else
                strRegex = "(" + strRegex + ")+?";

            return strRegex;
        }

        public List<Token> GetCaptures(ISectionItem regexItem)
        {
            if (regexItem is RegexEntity && dicEntityCapture.ContainsKey(regexItem as RegexEntity))
                return dicEntityCapture[regexItem as RegexEntity];
            else if (regexItem is RegexSection && dicSectionCapture.ContainsKey(regexItem as RegexSection))
                return dicSectionCapture[regexItem as RegexSection];
            else
                return null;
        }

        public Token GetCapture(RegexEntity entity, int index)
        {
            try
            {
                if (!dicEntityCapture.ContainsKey(entity))
                    return null;

                if (index < 0 || dicEntityCapture[entity].Count <= index)
                    return null;

                return dicEntityCapture[entity][index];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Token GetCapture(RegexSection section, int index)
        {
            try
            {
                if (!dicSectionCapture.ContainsKey(section))
                    return null;

                if (index < 0 || dicSectionCapture[section].Count <= index)
                    return null;

                return dicSectionCapture[section][index];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public void RefreshSelection(RegexEntity entity)
        {
            try
            {
                if (dicEntityCapture.ContainsKey(entity))
                    dicEntityCapture[entity] = null;

                if (string.IsNullOrEmpty(strTargetContent))
                    return;

                dicEntityCapture[entity] = GetCaptures(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
