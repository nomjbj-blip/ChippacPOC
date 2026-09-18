using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Data;

namespace DACrux.Base
{
    public enum DieDisplayValue { Bin, BinChar, Shot, Site, ReProb, ReProbChar, PCMValue, Zone, Block };
    public enum DisplayFlatZone { None = -1, Top, Right, Bottom, Left };

    [Serializable]
    public struct ChangeDieBinNumberInfo
    {
        public int nPreDefectNumber;
        public int nDefectNumber;
    }

    [Serializable]
    public struct WaferRecipe
    {
        public WaferRecipe(double WaferSize)
        {
            ANGLE = 0;
            NOTCH_TYPE = Notch.Flat;
            DIE_SIZE_X = 0.01d;
            DIE_SIZE_Y = 0.01d;
            ORIGIN_DIE_X = 0;
            ORIGIN_DIE_Y = 0;
            ORIGIN_X = 0.0d;
            ORIGIN_Y = 0.0d;
            STREET_X = 0.0d;
            STREET_Y = 0.0d;
            DIE_INDEX_MIN_X = 65536;
            DIE_INDEX_MAX_X = -65536;
            DIE_INDEX_MIN_Y = 65536;
            DIE_INDEX_MAX_Y = -65536;
            EDGE_SIZE = 1.0d;
            NETDIE = 0;
            XYDIR = XYDirection.LeftTop;
            FIRST_DIE_X = 0;
            FIRST_DIE_Y = 0;
            REFERENCEDIE_SETTING = 0;

            SHOT_ARRAY_X = SHOT_ARRAY_Y = 1;
            SHOT_START_X = SHOT_START_Y = 1;

            _wafer_size = WaferSize;
            WAFER_RADIUS = WaferSize / 2;
            WAFER_SIZE = WaferSize;
        }

        private double _wafer_size;

        public int ANGLE;
        public Notch NOTCH_TYPE;
        public double DIE_SIZE_X;
        public double DIE_SIZE_Y;
        public int ORIGIN_DIE_X;
        public int ORIGIN_DIE_Y;
        public double ORIGIN_X;
        public double ORIGIN_Y;
        public double STREET_X;
        public double STREET_Y;

        public int DIE_INDEX_MIN_X;
        public int DIE_INDEX_MAX_X;
        public int DIE_INDEX_MIN_Y;
        public int DIE_INDEX_MAX_Y;

        public int FIRST_DIE_X;
        public int FIRST_DIE_Y;

        public XYDirection XYDIR;

        public int REFERENCEDIE_SETTING;

        public int XDIES
        {
            get
            {
                return DIE_INDEX_MAX_X - DIE_INDEX_MIN_X + 1;
            }
        }

        public int YDIES
        {
            get
            {
                return DIE_INDEX_MAX_Y - DIE_INDEX_MIN_Y + 1;
            }
        }

        public double WAFER_SIZE
        {
            get { return _wafer_size; }
            set { _wafer_size = value; WAFER_RADIUS = value / 2; }
        }

        public double WAFER_RADIUS;
        public double EDGE_SIZE;
        public int NETDIE;

        public int SHOT_ARRAY_X;
        public int SHOT_ARRAY_Y;
        public int SHOT_START_X;
        public int SHOT_START_Y;
    }

    [Serializable]
    public struct RectangleD
    {
        public RectangleD(double dX, double dY, double dWidth, double dHeight)
        {
            X = dX;
            Y = dY;
            Width = dWidth;
            Height = dHeight;
        }

        public double X;
        public double Y;
        public double Width;
        public double Height;

        public RectangleF ToRectangleF()
        {
            return new RectangleF((float)X, (float)Y, (float)Width, (float)Height);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }
    }

    [Serializable]
    public struct PointD
    {
        public PointD(double dX, double dY)
        {
            X = dX;
            Y = dY;
        }

        public double X;
        public double Y;

        public static readonly PointD Empty;

        public override string ToString()
        {
            return String.Format("{0}, {1}", X, Y);
        }
    }

    [Serializable]
    public struct P4PolygonD
    {
        public PointD P1;
        public PointD P2;
        public PointD P3;
        public PointD P4;
    }

    [Serializable]
    public class Die : IComparable<Die>
    {
        static Die()
        {
            Empty = new Die();
            Empty.IndexX = Int32.MaxValue;
            Empty.IndexY = Int32.MaxValue;
        }

        public Die()
        {
        }

        public Die(int idxX, int idxY, int Bin, int DieProperty)
        {
            IndexX = idxX;
            IndexY = idxY;

            DiePassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            DieProp = DieProperty;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;
            DefectCount = 0;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;

            ParametricValue = 0.0d;

            DieCood.X = 0;
            DieCood.Y = 0;
            DieCood.Width = 0;
            DieCood.Height = 0;
            Dummy = -1;

            ScopeImageLocal = string.Empty;
            ScopeImage = string.Empty;
            ScopeImagePath = string.Empty;
        }

        public Die(int idxX, int idxY, int Bin, double dX, double dY, double dWidth, double dHeight)
        {
            IndexX = idxX;
            IndexY = idxY;

            DiePassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            DieProp = 1;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;
            DefectCount = 0;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;

            ParametricValue = 0.0d;

            DieCood.X = dX;
            DieCood.Y = dY;
            DieCood.Width = dWidth;
            DieCood.Height = dHeight;
            Dummy = -1;

            ScopeImageLocal = string.Empty;
            ScopeImage = string.Empty;
            ScopeImagePath = string.Empty;
        }

        public Die(int idxX, int idxY, int Bin, RectangleD dDieCood)
        {
            IndexX = idxX;
            IndexY = idxY;

            DiePassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            DieProp = 1;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;
            DefectCount = 0;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;

            ParametricValue = 0.0d;

            DieCood = dDieCood;
            Dummy = -1;

            ScopeImageLocal = string.Empty;
            ScopeImage = string.Empty;
            ScopeImagePath = string.Empty;
        }

        public Die(int idxX, int idxY, int Bin, double dX, double dY, double dWidth, double dHeight, int DieProperty)
        {
            IndexX = idxX;
            IndexY = idxY;

            DiePassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            DieProp = DieProperty;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;
            DefectCount = 0;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;

            ParametricValue = 0.0d;

            DieCood.X = dX;
            DieCood.Y = dY;
            DieCood.Width = dWidth;
            DieCood.Height = dHeight;
            Dummy = -1;

            ScopeImageLocal = string.Empty;
            ScopeImage = string.Empty;
            ScopeImagePath = string.Empty;
        }

        public Die(int idxX, int idxY, int Bin, RectangleD dDieCood, int DieProperty)
        {
            IndexX = idxX;
            IndexY = idxY;

            DiePassFail = 0;
            Marking = 0;
            AVIFailNumber = 0;
            ReProbing = 0;
            NeddleInsp = 0;
            DieProp = DieProperty;
            SetNeddleInsp = 0;
            SiteNumber = 0;
            BlockArea = 0;
            BinNumber = Bin;
            DefectCount = 0;

            ZoneNumber = 0;
            ShotID = 0;
            VIFail = 0;

            ParametricValue = 0.0d;

            DieCood = dDieCood;

            Dummy = -1;

            ScopeImageLocal = string.Empty;
            ScopeImage = string.Empty;
            ScopeImagePath = string.Empty;
        }

        public static Point IntToPoint(int xy)
        {
            return new Point (xy >> 16, 0xffff & xy);
        }

        public static int PointToInt(Point pt)
        {
            return XYToInt(pt.X, pt.Y);
        }

        public static int XYToInt(int x, int y)
        {
            return (x << 16) + y;
        }

        public bool IsEmpty()
        {
            return IndexY == Empty.IndexX && IndexY == Empty.IndexY;
        }

        public RectangleD DieCood;
        public int IndexX;
        public int IndexY;

        public int DiePassFail;
        public int Marking;
        public int AVIFailNumber;
        public int ReProbing;
        public int NeddleInsp;
        public int DieProp;
        public int SetNeddleInsp;
        public int SiteNumber;
        public int BlockArea;
        public int BinNumber;
        public int DefectCount;

        public int ShotID;
        public int VIFail;
        public int ZoneNumber;
        public double ParametricValue;

        public int Dummy;

        public static readonly Die Empty;

        public string ScopeImageLocal;
        public string ScopeImage;
        public string ScopeImagePath;

        public override bool Equals(object obj)
        {
            return IndexX == ((Die)obj).IndexX && IndexY == ((Die)obj).IndexY;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("X:{0},Y:{1}", IndexX, IndexY);
        }

        public static bool operator ==(Die die1, Die die2)
        {
            return die1.Equals(die2);
        }

        public static bool operator !=(Die die1, Die die2)
        {
            return !die1.Equals(die2);
        }

        public int CompareTo(Die other)
        {
            if (IndexX == other.IndexX)
                return IndexY.CompareTo(other.IndexY);
            else
                return IndexX.CompareTo(other.IndexX);
        }
    }

    [Serializable]
    public class DieList : List<Die>
    {
        public int BinarySearch(int indexX, int indexY)
        {
            return base.BinarySearch(new Die() { IndexX = indexX, IndexY = indexY });
        }

        public int IndexOf(int indexX, int indexY)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].IndexX == indexX && this[i].IndexY == indexY)
                    return i;
            }

            return -1;
        }

        public void UpdateDie(int currIndex, Die die)
        {
            this[currIndex] = die;
        }
    }

    [Serializable]
    public struct Shot
    {
        public int MinX;
        public int MinY;
        public int MaxX;
        public int MaxY;
        public int IndexX;
        public int IndexY;
        public int ShotIndex;
        public RectangleD ShotCood;

        public int CountX
        {
            get
            {
                return MaxX - MinX + 1;
            }
        }
        public int CountY
        {
            get
            {
                return MaxY - MinY + 1;
            }
        }

        public void Offset(int ShotX, int ShotY)
        {
            MinX = MinX + (ShotX * CountX);
            MinY = MinY + (ShotY * CountY);
        }
    }

    [Serializable]
    public struct WaferInfo
    {
        public string Product;
        public string Program;
        public string WaferID;
        public string WaferSeq;
        public string LotID;
        public string LotSeq;
    }

    public class DmsStepInfo
    {
        public string Maker { get; set; }
        public string Model { get; set; }
        public string Equip { get; set; }
        public string ResultTimestamp { get; set; }
        public string LotID { get; set; }
        public int WaferSize { get; set; }
        public int SampleSize { get; set; }
        public string DeviceID { get; set; }
        public string SetupID { get; set; }
        public string SetupTimestamp { get; set; }
        public string StepID { get; set; }
        public string StepSeq { get; set; }
        public string SampleOrientationMarkType { get; set; }
        public double DiePitchX { get; set; }
        public double DiePitchY { get; set; }
        public int DieOriginX { get; set; }
        public int DieOriginY { get; set; }
        public string WaferID { get; set; }
        public int Slot { get; set; }
        public int Angle { get; set; }
        public double SampleCenterLocationX { get; set; }
        public double SampleCenterLocationY { get; set; }
        public double AreaPerTest { get; set; }
        public string Inspector { get; set; }
        public int ShotArrayX { get; set; }
        public int ShotArrayY { get; set; }
        public int ShotStartX { get; set; }
        public int ShotStartY { get; set; }
    }

    public class DmsWaferDieInfo
    {
        public DmsWaferDieInfo(long stepSeq, DmsStepInfo stepInfo, Point[] dies)
        {
            StepSeq = stepSeq;
            Dies = dies;
            StepInfo = stepInfo;
        }

        public long StepSeq
        {
            get;
            private set;
        }

        public Point[] Dies
        {
            get;
            private set;
        }

        public DmsStepInfo StepInfo
        {
            get;
            private set;
        }
    }

    [Serializable]
    public struct DEFECT_TAG
    {
        public int[] DValue;
        public int TEST;
        public string[] IMAGENAME;

        public long step_seq;
        public int defect_id;
        public double x;
        public double y;
        public double xrel;
        public double yrel;
        public int cell_num;
        public double index_x;
        public double index_y;
        public double xsize;
        public double ysize;
        public double defectarea;
        public double dsize;
        public int classnumber;
        public int test;
        public int clusternumber;
        public int roughbinnumber;
        public int finebinnumber;
        public int reviewsample;
        public int imagecount;
        public int adder;
        public string first_step;
        public int reticle_repeat_id;
        public int cell_repeat_id;
        public int man_opt_class;
        public int auto_opt_class;
        public int man_sem_class;
        public int auto_sem_class;
        public int address_d;
        public int address_g;
        public string coor_flag;
    }


    [Serializable]
    public class ItemCountColor
    {
        public int From;
        public int To;
        public Color Color;
    }

    [Serializable]
    public class ItemCountColorList : List<ItemCountColor>
    {
        public List<int> GetAllCount()
        {
            List<int> list = new List<int>();

            foreach (var item in this)
            {
                for (int i = item.From; i <= item.To; i++)
                {
                    int idx = list.BinarySearch(i);

                    if (idx < 0)
                        list.Insert(~idx, i);
                }
            }

            return list;
        }

        /// <summary>
        /// 모든 영역이 포함되는지를 가져옵니다.
        /// </summary>
        public bool IsAllRange()
        {
            if (Count == 0 || this[0].From != 0)
                return false;
            
            for (int i = 1; i < Count; i++)
            {
                // [i-1].To 와 [i].From 이 1 차이이면 연속
                if (this[i].From - this[i - 1].To != 1)
                    return false;
            }

            return true;
        }

        public Color GetColor(int count)
        {
            foreach (var item in this)
            {
                if (item.From <= count && item.To >= count)
                    return item.Color;
            }

            return Color.Empty;
        }
    }

    [Serializable]
    public class TestImageList : List<TestImage>
    {
        public void Add(Point Index, string LocalimageFullPath, string LocalimageNmae, string strBinNo)
        {
            Add(new TestImage() { XY = Index, LocalImagePath = LocalimageFullPath, LocalImageName = LocalimageNmae, BinNumber = strBinNo });
        }

        public void Remove(Point oDieIndex)
        {
            RemoveAll(p => p.XY == oDieIndex);
        }
    }

    [Serializable]
    public struct TestImage
    {
        public Point XY;
        public string IndexX;
        public string IndexY;
        public string LocalImagePath;
        public string LocalImageName;
        public string ServerImagePath;
        public string BinNumber;
    }

    #region [DMS Semi]



    /// DMS For SEMI
    /// 
    /// <Summary>
    /// 
    /// <b>■KLARF Structure</b><br>
    ///  
    /// - 작  성  자 : 미라콤 임영신<br>
    /// - 최초작성일 : 2004년 10월 07일<br>
    /// - 최종수정자 : 임영신<br>
    /// - 최종수정일 : 2004년 10월 07일<br>
    /// - 주요변경로그<br>
    /// 2004.10.07 생성<br>
    /// 
    /// </Summary>
    /// <Remarks>없음</Remarks>
    public struct DMS_IMAGE_TAG
    {
        public long STEP_SEQ;
        public int DEFECT_ID;
        public int IMAGE_ID;
        public int TEST;
        public long WAFER_SEQ;
        public string IMAGE_TYPE;
        public string IMAGE_PATH;
        public string IMAGE_SERVER;
        public string IMAGE_THUMB_PATH;
    }

    public struct DIEINFO_TAG
    {
        public int TEST;						//2004.09.21 DB Schema 변경시 추가 
        public int DX;
        public int DY;
    }

    public struct KLARF_HEADER_TAG
    {
        public float Version;				//	FileVersion 1 1;
        public string FileTimestamp;			//	FileTimestamp 03-19-02 16:18:20;
        public string TiffSpec;				//	TiffSpec 6.0 R NA;
        public string TiffFileName;			//	TiffFileName img-30021.tif;
        public string[] InspectionStationID;	// InspectionStationID "KLA_TENCOR" "KLA2139" "QPE1041";
        public string SampleType;				//	SampleType WAFER;
        public string ResultTimestamp;		//	ResultTimestamp 03-19-02 14:11:00;
        public string LotID;					//	LotID "Q0319_KLA";
        public int[] SampleSize;				//	SampleSize 1 200;
        public string DeviceID;
        public string SetupID;				//	SetupID "ETA25LXX_AAQC" 11-26-01 16:25:43;
        public string SetupTime;
        public string StepID;					//	StepID "AAQC";
        public string ResultID;
        public string SampleOrientationMarkType;	//SampleOrientationMarkType NOTCH;
        public string OrientationMarkLocation;	//OrientationMarkLocation RIGHT;
        public double DiePitchX;				//	DiePitch 2.1849500000e+04 2.1828200000e+04;
        public double DiePitchY;
        public double DieOriginX;				//DieOrigin 0.000000 0.000000;
        public double DieOriginY;
        public string WaferID;				//	WaferID "05";
        public int Slot;					//	Slot 4;
        public double SampleCenterLocationX;		//	SampleCenterLocation 2.3194300000e+04 2.1190300000e+04;
        public double SampleCenterLocationY;
        public int ClassLookup;			//	ClassLookup 256 
        public string[] DefectClass;
        public int[] DefectClusterSetup;
        public int InspectionTest;			//	InspectionTest 1
        public int SampleTestPlan;			//	SampleTestPlan 47
        public DIEINFO_TAG[] TestDieInfo;
        public double AreaPerTest;			//	AreaPerTest 4.2073100000e+09;
        public string[] TestParametersSpec;		//	TestParametersSpec 2 PIXELSIZE SAMPLEPERCENTAGE;
        public double[] TestParametersList;		// TestParametersList 0.390600 0.000000;
        public string[] DefectClusterSpec;
        public string[] DefectRecordSpec;		// DefectRecordSpec 16 DEFECTID XREL YREL XINDEX YINDEX XSIZE YSIZE DEFECTAREA DSIZE CLASSNUMBER TEST CLUSTERNUMBER ROUGHBINNUMBER FINEBINNUMBER REVIEWSAMPLE IMAGECOUNT ;
        public DEFECT_TAG[] Defects;
        public string[] SummarySpec;
        public double[] SummaryList;
    }

    public struct INSP_INFO
    {
        public long STEP_SEQ;
        public long WAFER_SEQ;
        public string INSPECTION_TIME;
        public string TECHNOLOGY;
        public string PRODUCT;
        public string LOT_ID;
        public string WAFER_ID;
        public string STEP_ID;
        public string SLOT_ID;
        public long SETUP_SEQ;
        public string LAST_UPDATE;
        public string STATUS;
        public string MAIN_EQ;
        public string INSPECTION_EQ;
        public string INSP_FILENAME;
        public string INSPECTION_PARAM;
        public int DEFECTS;
        public int CLASSIFIED_DEFECTS;
        public int KILLER_DEFECTS;
        public int RANDOM_DEFECTS;
        public int ADDER_KILLER_DEFECT;
        public int ADDER_RANDOM_DEFECT;
        public int CLUSTERS;
        public int ADDER_CLUSTERS;
        public double CLUSTER_AREA;
        public double DEFECT_DD;
        public double KILLER_DEFECT_DD;
        public double RANDOM_DEFECT_DD;
        public int INSPECTED_DIE;
        public int DEFECTIVE_DIE;
        public int ADDER_DEF_DIE;
        public int KILL_DEF_DIE;
        public int KILL_ADDER_DEF_DIE;
        public int IMAGES;
        public int SCAN_AREA;
        public int ADDER_DEFECTS;
        public int KILL_RND_DEFECTS;
    }
    #endregion

    public struct ARGUMENT_TAG
    {
        private string strResultFile;
        private string strResultPath;
        private string strBackupFile;
        private string strBackupPath;
        private string strProcessFile;
        private string strProcessPath;
        private string strErrorFile;
        private string strErrorPath;
        private string strServicePath;
        private string strServiceFile;
        private string strUploadComand;
        private string strLOGPath;
        private int iLogLevel;
        public string ResultType;
        private string otmDttm;
        private string otmPathDttm;
        private string otmDBDttm;
        private string strArchivePath;
        private string strResultEquip;
        private string strCSVFile;
        private string strFileFormat;
        public string SubProcessFile;
        /// <summary>
        /// Source File
        /// 처음 Write 된 위치의 File Name
        /// </summary>
        public string ResultFile
        {
            get
            {
                return strResultFile;
            }
            set
            {
                DateTime oDtNow = DateTime.Now;
                otmDttm = oDtNow.ToString("yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo);
                otmPathDttm = oDtNow.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                otmDBDttm = oDtNow.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);

                strResultFile = value;

                strResultPath = Path.GetDirectoryName(strResultFile);
                strProcessPath = strResultPath + @"\PROCESSING";
                strProcessFile = string.Format(@"{0}\{1}_{2}{3}", strProcessPath
                                                , Path.GetFileNameWithoutExtension(strResultFile)
                                                , otmDttm
                                                , Path.GetExtension(strResultFile));

                strCSVFile = string.Format(@"{0}\{1}_{2}.CSV", strProcessPath
                                            , Path.GetFileNameWithoutExtension(strResultFile)
                                            , otmDttm);

                strErrorPath = Path.GetDirectoryName(strResultPath) + @"\ERROR";
                strLOGPath = Path.GetDirectoryName(strResultPath) + @"\LOG";
                strResultEquip = Path.GetFileName(Path.GetDirectoryName(strResultPath));

                if (!Directory.Exists(strErrorPath)) Directory.CreateDirectory(strErrorPath);
                if (!Directory.Exists(strLOGPath)) Directory.CreateDirectory(strLOGPath);
                strErrorFile = string.Format(@"{0}\{1}_{2}{3}", strErrorPath
                                                , Path.GetFileNameWithoutExtension(strResultFile)
                                                , otmDttm
                                                , Path.GetExtension(strResultFile));

                iLogLevel = 5;
            }
        }

        public string FileFormat
        {
            get
            {
                return strFileFormat;
            }
            set
            {
                strFileFormat = value;
            }
        }

        public string CSVFile
        {
            get
            {
                return strCSVFile;
            }
        }

        public string ResultEquipID
        {
            get
            {
                return strResultEquip;
            }
            set
            {
                strResultEquip = value;
            }
        }

        public string KLARFFileName
        {
            get
            {
                return Path.GetDirectoryName(strResultFile);
            }
        }
        /// <summary>
        /// Result File이 In 되는 Directory
        /// </summary>
        public string ResultPath
        {
            get
            {
                return strResultPath;
            }
        }


        /// <summary>
        /// Backup File
        /// </summary>
        public string BackupFile
        {
            get
            {
                strBackupFile = string.Format(@"{0}\{1}_{2}{3}", BackupPath
                    , Path.GetFileNameWithoutExtension(strResultFile)
                    , otmDttm
                    , Path.GetExtension(strResultFile));
                return strBackupFile;
            }
        }

        /// <summary>
        /// File을 Parsing하기전 Backup을 받는 Directory
        /// </summary>
        public string BackupPath
        {
            get
            {
                return strBackupPath;
            }
            set
            {
                strBackupPath = value;
                strArchivePath = string.Format(@"{0}\{1}", strBackupPath, otmPathDttm);
            }
        }

        public string ArchivePath
        {
            get
            {
                return strArchivePath;
            }
        }

        /// <summary>
        /// Error File
        /// </summary>
        public string ErrorFile
        {
            get
            {
                return strErrorFile;
            }
            set
            {
                strErrorFile = value;
            }
        }

        /// <summary>
        /// File Parsing중 Error이 발생하면 이곳으로 옮김
        /// </summary>
        public string ErrorPath
        {
            get
            {
                return strErrorPath;
            }
            set
            {
                strErrorPath = value;
            }
        }


        /// <summary>
        /// Service File
        /// </summary>
        public string ServiceFile
        {
            get
            {
                return strServiceFile;
            }
            set
            {
                strServiceFile = value;
            }
        }

        /// <summary>
        /// File Parsing이 정상 종료후 Service를 할 Directory
        /// </summary>
        public string ServicePath
        {
            get
            {
                return strServicePath;
            }
            set
            {
                strServicePath = value;
                strServiceFile = string.Format(@"{0}\{1}_{2}{3}", strServicePath
                    , Path.GetFileNameWithoutExtension(strResultFile)
                    , otmDttm
                    , Path.GetExtension(strResultFile));
            }
        }


        /// <summary>
        /// Parsing할 KLARFFile Name
        /// </summary>
        public string ProcessFile
        {
            get
            {
                return strProcessFile;
            }
        }


        /// <summary>
        /// Parsing할 KLARFFile Name
        /// </summary>
        public string ProcessPath
        {
            get
            {
                return strProcessPath;
            }
        }


        /// <summary>
        /// Parsing할 KLARFFile Name
        /// </summary>
        public string UploadComand
        {
            get
            {
                return strUploadComand;
            }
            set
            {
                strUploadComand = value;
            }
        }


        /// <summary>
        /// TransTime Return;
        /// </summary>
        public string TransTime
        {
            get
            {
                return otmDttm;
            }
        }


        /// <summary>
        /// TransTime Return;
        /// Used DB
        /// </summary>
        public string TransTimeUseDB
        {
            get
            {
                return otmDBDttm;
            }
        }


        public string CommonServicePath
        {
            get
            {
                string strCmnService = string.Empty;
                strCmnService = string.Format(@"{0}\SERVICE", Path.GetDirectoryName(Path.GetDirectoryName(strResultPath)));
                return strCmnService;
            }
        }

        public string LogPath
        {
            get
            {
                return strLOGPath;
            }
            set
            {
                strLOGPath = value;
            }
        }

        public int LogLevel
        {
            get
            {
                return iLogLevel;
            }
            set
            {
                iLogLevel = value;
            }
        }
    }
}
