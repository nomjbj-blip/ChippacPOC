using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.Base
{
    #region [QMS addtion 12/02/15]

    #region DACruxStd - Date Format

    public static class DACruxStd
    {
        public const string DACruxDBTimeFormat = "YYYY/MM/DD HH24:MI:SS";
        public const string DACruxAPPTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public const string DACruxDBDateFormat = "YYYY/MM/DD";
        public const string DACruxAPPDateFormat = "yyyy/MM/dd";
    }

    #endregion

    #region Master Information

    public static class YmsSetting
    {
        public static readonly string AR_TEST_OPER = "5100";
        public static readonly string CF_TEST_OPER = "8040";
        public static readonly string CE_TEST_OPER = "3800";
        public static readonly string NONE = "9999";
    };

    #endregion

    public static class ConvertNumeric
    {
        public static int ConvertInt(string strInteger)
        {
            try
            {
                if (strInteger.Trim() == "")
                    return 0;

                return System.Convert.ToInt32(strInteger.Trim());
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public static double Convertdouble(string strDouble)
        {
            try
            {
                if (strDouble.Trim() == "")
                    return double.NaN;

                return System.Convert.ToDouble(strDouble.Trim());
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }

    [Serializable]
    public struct GlassRecipe
    {
        public GlassRecipe(double GlassSizeX, double GlassSizeY)
        {
            ANGLE = 0;
            GLASS_SIZE_X = GlassSizeX;
            GLASS_SIZE_Y = GlassSizeY;
            CELL_SIZE_X = 0.01d;
            CELL_SIZE_Y = 0.01d;

            ARRAY_SIZE_X = 0.01d;
            ARRAY_SIZE_Y = 0.01d;

            ORIGIN_CELL_X = 0;
            ORIGIN_CELL_Y = 0;
            ORIGIN_X = 0.0d;
            ORIGIN_Y = 0.0d;
            STREET_X = 0.0d;
            STREET_Y = 0.0d;

            CELL_INDEX_MIN_X = 0;
            CELL_INDEX_MAX_X = 0;
            CELL_INDEX_MIN_Y = 0;
            CELL_INDEX_MAX_Y = 0;

            ARRAY_INDEX_MIN_X = 0;
            ARRAY_INDEX_MAX_X = 0;
            ARRAY_INDEX_MIN_Y = 0;
            ARRAY_INDEX_MAX_Y = 0;

            EDGE_SIZE = 1.0d;
            NETCELL = 0;
            NETARRAY = 0;
            FIRST_CELL_X = 0;
            FIRST_CELL_Y = 0;
            REFERENCEDIE_SETTING = 0;
        }

        public int ANGLE;
        public double GLASS_SIZE_X;
        public double GLASS_SIZE_Y;
        public double CELL_SIZE_X;
        public double CELL_SIZE_Y;
        public double ARRAY_SIZE_X;
        public double ARRAY_SIZE_Y;

        public int ORIGIN_CELL_X;
        public int ORIGIN_CELL_Y;
        public double ORIGIN_X;
        public double ORIGIN_Y;
        public double STREET_X;
        public double STREET_Y;

        public int CELL_INDEX_MIN_X;
        public int CELL_INDEX_MAX_X;
        public int CELL_INDEX_MIN_Y;
        public int CELL_INDEX_MAX_Y;

        public int ARRAY_INDEX_MIN_X;
        public int ARRAY_INDEX_MAX_X;
        public int ARRAY_INDEX_MIN_Y;
        public int ARRAY_INDEX_MAX_Y;

        public int FIRST_CELL_X;
        public int FIRST_CELL_Y;

        public int REFERENCEDIE_SETTING;

        public double EDGE_SIZE;
        public int NETCELL;
        public int NETARRAY;

        public int XCELLS
        {
            get
            {
                return CELL_INDEX_MAX_X - CELL_INDEX_MIN_X + 1;
            }
        }

        public int YCELLS
        {
            get
            {
                return CELL_INDEX_MAX_Y - CELL_INDEX_MIN_Y + 1;
            }
        }

        public int XARRAYS
        {
            get
            {
                return ARRAY_INDEX_MAX_X - ARRAY_INDEX_MIN_X + 1;
            }
        }

        public int YARRAYS
        {
            get
            {
                return ARRAY_INDEX_MAX_Y - ARRAY_INDEX_MIN_Y + 1;
            }
        }
    }

    [Serializable]
    public struct Cell
    {

        public Cell(int idxX, int idxY, int Bin, int CellProperty)
        {
            IndexX = idxX;
            IndexY = idxY;

            CellPassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            CellProp = CellProperty;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;
            Dummy = 0;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;

            ParametricValue = 0.0d;

            CellCood.X = 0;
            CellCood.Y = 0;
            CellCood.Width = 0;
            CellCood.Height = 0;
        }

        public Cell(int idxX, int idxY, int Bin, double dX, double dY, double dWidth, double dHeight)
        {
            IndexX = idxX;
            IndexY = idxY;

            CellPassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            CellProp = 1;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;
            Dummy = 0;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;

            ParametricValue = 0.0d;

            CellCood.X = dX;
            CellCood.Y = dY;
            CellCood.Width = dWidth;
            CellCood.Height = dHeight;
        }

        public Cell(int idxX, int idxY, int Bin, RectangleD dCellCood)
        {
            IndexX = idxX;
            IndexY = idxY;

            CellPassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            CellProp = 1;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;
            Dummy = 0;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;

            ParametricValue = 0.0d;

            CellCood = dCellCood;
        }

        public Cell(int idxX, int idxY, int Bin, double dX, double dY, double dWidth, double dHeight, int CellProperty)
        {
            IndexX = idxX;
            IndexY = idxY;

            CellPassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            CellProp = CellProperty;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;
            Dummy = 0;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;

            ParametricValue = 0.0d;

            CellCood.X = dX;
            CellCood.Y = dY;
            CellCood.Width = dWidth;
            CellCood.Height = dHeight;
        }

        public Cell(int idxX, int idxY, int Bin, RectangleD dCellCood, int CellProperty)
        {
            IndexX = idxX;
            IndexY = idxY;

            CellPassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            CellProp = CellProperty;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;
            Dummy = 0;
            ParametricValue = 0.0d;

            CellCood = dCellCood;
        }

        public RectangleD CellCood;
        public int IndexX;
        public int IndexY;

        public int CellPassFail;
        public int Marking;
        public int AVIFailNumber;
        public int ReProbing;
        public int NeddleInsp;
        public int CellProp;
        public int SetNeddleInsp;
        public int SiteNumber;
        public int BlockArea;
        public int BinNumber;
        public int Dummy;

        public int ShotID;
        public int VIFail;
        public int ZoneNumber;
        public double ParametricValue;

    }

    public struct CellArray
    {
        public CellArray(int idxX, int idxY, int Bin)
        {
            IndexX = idxX;
            IndexY = idxY;

            CellNumber = 0;
            BinNumber = Bin;

            ZoneNumber = 0;
            ParametricValue = 0.0d;

            ArrayCood.X = 0;
            ArrayCood.Y = 0;
            ArrayCood.Width = 0;
            ArrayCood.Height = 0;
        }

        public CellArray(int idxX, int idxY, int Bin, double dX, double dY, double dWidth, double dHeight)
        {
            IndexX = idxX;
            IndexY = idxY;

            CellNumber = 0;
            BinNumber = Bin;

            ZoneNumber = 0;
            ParametricValue = 0.0d;

            ArrayCood.X = dX;
            ArrayCood.Y = dY;
            ArrayCood.Width = dWidth;
            ArrayCood.Height = dHeight;
        }

        public CellArray(int idxX, int idxY, int Bin, RectangleD dCellCood)
        {
            IndexX = idxX;
            IndexY = idxY;

            CellNumber = 0;
            BinNumber = Bin;

            ZoneNumber = 0;
            ParametricValue = 0.0d;

            ArrayCood = dCellCood;
        }
        public RectangleD ArrayCood;
        public int IndexX;
        public int IndexY;

        public int CellNumber;
        public int BinNumber;
        public int ZoneNumber;
        public double ParametricValue;
    }

    #endregion
}
