using System;

namespace FDll
{
	[Serializable]
	public struct HeaderData
	{
		const int INITNO = -999999;
		const string INITSTR = "";

		public string	Operator;					
		public string	Device;
		public string 	TestProgram;
		public string 	TesterID;
		public int	StationNo;
		public string	HandlerProber;
		public string   ProberCard;
		public int	CassetteNo;
		public string	LotNo;
		public string	MotherLotNo;
		public int	SlotNo;
		public string	WaferID;
		public string	StartTime;
		public string	EndTime;

		public double	WaferSize;	// mm
		public int	Angle;			// {0, 90, 180, 270}
		public int	NotchType;		// {Notch = 0, Flat = 1}
		public double  EdgeSize;	// mm : Wafer 가장자리의 테두리 크기
		public int	TestDies;
		public int	PassDies;
		public int	FailDies;
		public double Pass;
		public double Fail;
		public int	XDies;			// Wafer X축의 Index 수
		public int	YDies;	
		public int DieIndexMinX;
		public int DieIndexMaxX;
		public int DieIndexMinY;
		public int DieIndexMaxY;		
		public double	ChipSizeX;
		public double	ChipSizeY;
		public double	OriginX;	// Wafer Center에서 Origin Die의 좌측 아래 모서리 까지의 X 거리
		public double	OriginY;
		public int	OriginDieX;		//Center Die Index X
		public int	OriginDieY;
		public double	TargetX;	// Wafer Center에서 기준 Die의 좌측 아래 모서리 까지의 X 거리
		public double	TargetY;	
		public int	TargetDieX;		// 기준 Die Index X
		public int	TargetDieY;
		public int	FirstDieX;		// 최초 Die의 Index X
		public int	FirstDieY;
		public int	XYDirection;	// Wafer Test 진행 방향 {LeftTop=0, LeftButtom=1, RightTop=2, RightButtom=3}
        public string MachineNo;    // 설비 명
        public string TestArea;
        public string MapProduct;
        public string Customer;
        public string Format;

        //GMS
        public int WaferNo;
        public int TotalDies;

        public string CardNo;
        public int WaferNoPlus;
        public int WaferNoMinus;
        public int WaferNoList;
        public string WaferName;
        public int LightingMode;
        public int StartPosition;
        public int MicroPosition;
        public string AlignmentAxis;
        public int AutoFocus;
        public int AlignmentX;
        public int AlignmentY;
        public double ProbeSize;
        public int TargetMode;
        public int TargetPositionX;
        public int TargetPositionY;
        public int StdPositionX;
        public int StdPositionY;
        public string OrientFlatSelect;
        public int NumOrientationFlat;
        public int OrientFlatPosition;
        public string ProbeAreaSel;
        public int InkerOffset;
        public int SampleProbeMode;
        public int SampleStep1X;
        public int SampleStep1Y;
        public int SampleStep2X;
        public int SampleStep2Y;
        public int SampleStep3X;
        public int SampleStep3Y;
        public int SampleStep4X;
        public int SampleStep4Y;
        public int SampleStep5X;
        public int SampleStep5Y;
        public int SampleStep6X;
        public int SampleStep6Y;
        public int SampleStep7X;
        public int SampleStep7Y;
        public int SampleStep8X;
        public int SampleStep8Y;
        public int SampleStep9X;
        public int SampleStep9Y;
        public int SampleStep10X;
        public int SampleStep10Y;
        public int MonitorDieX;
        public int MonitorDieY;
        public double MonitorDieSizeX;
        public double MonitorDieSizeY;
        public int MultiDieMode;
        public int MultiDieLocation;
        public int ContinuousFail;
        public int CheckBack;
        public int ContinuousFailCnt;
        public int SkipDieLine;
        public int CheckBackCnt;
        public int RejectWaferCnt;
        public int ChkBackNeedlePolish;
        public int ProbeNeedlePolish;
        public int ZCount;
        public int DieCount;
        public int WaferCount;
        public int Overdrive;
        public int ExecutionCnt;
        public int SettingTemperature;
        public int PresetAddressX;
        public int PresetAddressY;
        public int MarkingMode;
        public string LotStartTime;
        public string LotEndTime;
        public int CassetteSetInfo;
        public string ModelInfo;

        public string Records;
        //GMS

		//-- TSK
		public int	ProbingStartPosition;	// Prober 개시 방향 {LeftTop=1, RightTop=2, LeftButtom=3, RightButtom=4}
		public int	ProbingDirection;	// Probing 방향 {Left=1, Right=2, Top=3, Buttom=4}
		public int	XDirection;		// X축 증가 방향 {Left=1, Right=2}
		public int	YDirection;		// Y축 증가 방향 {Forword=1, Inword=2} 
		public int	TestCount;		// Test 횟수
		public string	WaferLoadingStartTime;
		public string	WaferUnloadingStartTime;
		public int	MachineNo1;
		public int	MachineNo2;
		public int	SpecialWord;
		public int	TestingFinishStatus;
		public int	ReferenceDieSetting;
		public int	AreaStartAdress;
		public int	LineCatData;
		public int	LineCatAddress;
		public int	MapFile;
		public int	MultiSite;
		public int	Category;
		public int	LastEditEquipKind;
		public int	MapVersion;
		public int	MapDataKind;
		//-- TSK

		//-- AMap
		public int	DContact;
		//-- AMap

		//-- Wmap_a
		public string Title;
		public int MC;
        //----------

        //-- Isort
        public string ReelID;
        //----------

        public int VIFail;
        public string Golden_Check;

        public string LotSeq;
        public string WaferSeq;
        public string CopyFlag;
        public int XRef;
        public int YRef;
        public string TestAreaGroup;
        public string TableName;

        public int iMinX;
        public int iMaxX;
        public int iMinY;
        public int iMaxY;

        public string[] WaferList;
	}

	public struct RowData
	{
		public int	DieX;
		public int	DieY;
		public int	Bin;
		public int  HBin;
		public string  CharBin;
		public int	DieTestResult;	// Die Test 결과 {Notest=0, Pass=1, Fail1=2, Fail2=3}
		public int	Marking;		// Marking 실행 유무 {무=0, 유=1}
		public int	DieAttribute;	// Die 속성 {Skip=0; Test=1, Mark=2}
		public int	TestSiteNo;

		//-- TSK
		public int	FailMarkInsp;		// {불가능=0, 가능=1}
		public int	ReProbing;			// Reprobing 결과
		public int	NeddleInspResult;	// {OK=0, NG=1}
		public int	NeddleInspTarget;	// {없음=0, 있음=1}
		public int	SamplingDie;
		public int  DummyData;
		public int	NotOutput;			//  {미측정=0, 측정 진행=1}
		public int	ProbingDie;
		public int	BlockArea;			// Block Area 판정
		//-- TSK

		//-- AVI
		public int  AviBin;
		//-- AVI

        //-- ISORT
        public int Reel;
        public int Position;
        //-- ISORT

        //-- Merge
        public int VisualInsp;
		//-- Merge
	}

    public struct BinDesc
    {
        public string Bin;
        public string InCharBin;
        public string OutCharBin;
    }
}
