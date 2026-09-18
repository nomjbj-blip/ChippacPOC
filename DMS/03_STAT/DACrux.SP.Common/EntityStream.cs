using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace DACrux.SP.Common
{
    public class EntityStreamReader : StreamReader
    {
        #region " Member Field "

        private Regex _regex = null;

        private string _lineBuffer = null;
        private int _bufferIndex = 0;

        private long _startIndex = -1;
        private long _endIndex = -1;

        #endregion

        #region " Property "

        public new bool EndOfStream
        {
            get { return !BaseStream.CanRead || !(BaseStream.Position < _endIndex); }
        }

        #endregion

        #region " Creator "

        public EntityStreamReader(string regexString, string path) : this(regexString, path, -1, -1) { }

        public EntityStreamReader(string regexString, string path, long startAt, long endAt)
            : base(path)
        {
            try
            {
                if (string.IsNullOrEmpty(regexString))
                    throw new ArgumentNullException("regexString");

                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException("path");

                _regex = new Regex(regexString, RegexOptions.Compiled | RegexOptions.Multiline);
                //_regex = new Regex(regexString, RegexOptions.Compiled );
                _startIndex = (startAt < 0) ? 0 : startAt;
                _endIndex = (endAt < 0) ? BaseStream.Length - 1 : endAt;

                while (this.BaseStream.Position < _startIndex)
                    this.Read();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region " Method "

        public string ReadEntity()
        {
            if (EndOfStream)
                return null;

            while (!EndOfStream)
            {
                if (string.IsNullOrEmpty(_lineBuffer))
                {
                    _lineBuffer = ReadLine();
                    _bufferIndex = 0;
                }

                Match match = _regex.Match(_lineBuffer, _bufferIndex);

                // (\s*(\b\d+\b)\s*,\s*){2}((\s*(-?\b\d+(\.\d+)?\b)|(NA))(\s*,\s*)?)+?\n

                if (match.Length < 1)
                {
                    _lineBuffer = null;
                    continue;
                }
                else
                {
                    _bufferIndex = match.Index + match.Length;
                    return match.Value;
                }
            }

            return null;
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        
        public override void Close()
        {
            base.Close();
        }

        #endregion
    }
}
