using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.SP.Common
{
    public enum PatternModeItem
    {
        Single,
        Multi,
    }

    public enum ActiveTabItems
    {
        Entity,
        Section
    }
    public enum encoding
    {
       ASCII
       ,BigEndianUnicode
       ,Default
       ,Unicode
       ,UTF32
       ,UTF7
       ,UTF8
    }
    
    public enum TreeviewItemType
    {
        None = 0,
        Entity,
        Section
    }

    public enum TaskTypeItem
    {
        COM_PLUS,
        DATABASE,
        ENTITY_VALUE,
        ENTITY_TRAVERSE
    }

    public enum TaskRunningModeItem
    {
        Always = 0,
        Once
    }

    public enum LogTypeItem
    {
        Notice,
        Warning,
        Error,
    }

    public enum QueryTypeItem
    {
        Select,
        Insert,
        Update,
        Delete,
        Dynamic
    }

    public enum ResultUseTypeItem
    {
        General,
        Count,
        SeperatedValue,
        CharArray
    }

    public enum ScriptCommandTypeItem
    {
        REPEAT_START,
        REPEAT_END,
        RUN,
        SET_VALUE,
        SET_ARGS
    }

    public enum StreamModeItem { None, Stream }

    public enum FormatFileType { Formatter, Parser }

    public enum EntityString
    {
        bin, Wafer_ID, Product, FlatZone, X, Y, Total, Run_ID, Eow, BinCode, Wafer_Num
    }
}
