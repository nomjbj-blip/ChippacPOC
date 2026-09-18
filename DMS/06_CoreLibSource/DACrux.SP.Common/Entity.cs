using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DACrux.SP.Common;

namespace DACrux.SP.Common
{
    [Serializable]
    public abstract class Entity : ISectionItem
    {
        protected string name = string.Empty;
        protected string defaultvalue = string.Empty;
        protected string rownum = string.Empty;
        public virtual string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return; // throw new ArgumentException("Name cannot be empty.");

                if (name == value)
                    return;

                string prevName = name;
                name = value;
            }
        }
        public virtual string DefaultValue
        {
            get
            {
                return defaultvalue;
            }
            set
            {
                this.defaultvalue = value;
            }
        }

        public virtual string RowNum
        {
            get
            {
                return rownum;
            }
            set
            {
                this.rownum = value;
            }
        }

        public Section Parent { get; set; }

        public bool Force { get; set; }

        public abstract string GetValue();

        public override string ToString()
        {
            return Name;
        }
    }

    [Serializable]
    public class RegexEntity : Entity
    {
        public override string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return; // throw new ArgumentException("Name cannot be empty.");

                if (name == value)
                    return;

                string prevName = name;
                name = value;

                if (EntityRenamed != null)
                    EntityRenamed(this, prevName);
            }
        }
        public override string DefaultValue
        {
            get
            {
                return defaultvalue;
            }
            set
            {
                this.defaultvalue = value;
            }
        }
        public StreamModeItem StreamMode { get; set; }

        public string Label { get; set; }

        private string strRegex = string.Empty;

        public string RegexString
        {
            get { return strRegex; }
            set
            {
                try
                {
                    if (strRegex == value)
                        return;

                    strRegex = value;

                    if (RegexChanged != null)
                        RegexChanged(this);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        private RegexOptions regexOption = RegexOptions.None;
        //추가부분 인코딩
        public Encoding encoding = Encoding.Default;
       //Wafer 시작 좌표
        public int STP_X = 0;
        public int STP_Y = 0;
        public RegexOptions RegexOption
        {
            get { return regexOption; }
            set
            {
                try
                {
                    if (regexOption == value)
                        return;

                    regexOption = value;

                    if (RegexChanged != null)
                        RegexChanged(this);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string RegexValueGroupName { get; set; }
        public string RegexValueSeperator { get; set; }

        public Type ValueType { get; set; }
        public string strKey { get; set; }
        public override string GetValue()
        {
            return string.Empty;
        }

        public event RegexChangedEventHandler RegexChanged;

        public event ItemRenamedEventHandler<RegexEntity> EntityRenamed;
    }



    public delegate void ItemAddedEventHandler<T>(T item);
    public delegate void ItemRemovedEventHandler<T>(T item);
    public delegate void ItemRenamedEventHandler<T>(T item, string prevName);
    public delegate void RegexChangedEventHandler(RegexEntity entity);
}
