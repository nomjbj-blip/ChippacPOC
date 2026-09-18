using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.Base
{
    #region Public Enum

    public enum Notch { Notch = 0, Flat = 1 };
    public enum XYDirection { LeftTop = 0, LeftBottom = 1, RightBottom = 2, RightTop = 3 };
    public enum Position { LeftTop = 0, LeftBottom = 1, RightTop = 2, RightBottom = 3 };
    public enum SeqType { Wafer = 0, Lot = 1 };
    public enum USEFLAG { ALL = 0, USE = 1, NOTUSE = 2 };
    public enum USEDIEFLAG { ALL = 0, USE = 1, NOTUSE = 2, EDGE = 3, OUT = 4 };
    public enum DIAGRAM_TYPE { Retangle = 0, Circle = 1, Close = 2 };
    public enum YieldType { Test, Fab, Cum };

    public enum ChartType { XR, XS, IMR, TREND }
    public enum LoggingType { LOG, ERROR, WARNING }
    public enum ParsingDataType { LOT, GLASS, QPANEL, PANEL }
    public enum ChartCategory { Control, Histogram, Probability }
    public enum ParsingResultType { Success, PreFail, MainFail }
    public enum UpdateErrorCode { NoError, MissingSubFile, BaseInformationError, HeaderError, DataError, UnknownError, TempDefectUploadError, PrevUploadError }

    public enum EXIT_MODE { EXIT, LOGOFF, RESTART };
    public enum MAP_TYPE { SIZE, CLASS, CLUSTER, FINEBIN, ROUGHBIN, DSA, REPEAT, OVERLAY, RD };

    #endregion
}
