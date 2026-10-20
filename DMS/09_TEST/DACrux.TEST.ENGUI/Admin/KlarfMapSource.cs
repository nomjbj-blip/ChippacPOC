using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using DACrux.Data.Parser;
using DACrux.Data.Parser.Klarf;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// KLARF 파일 하나를 읽어 Map 생성에 필요한 값만 모은 결과다.
    /// 왜: File Map Setup 화면이 Parser 객체 전체를 들고 있지 않고, 검증된 값(µm 단위)만 쓰게 하려고 만들었다.
    /// 역할:
    /// 1. 기존 DMS 와 같은 방식으로 Parser 를 고른다 (FileAnalyzer -> ParserKlarf / ParserKlarf_18).
    /// 2. Parser 가 옮긴 인덱스(좌하단 1,1 기준)를 그대로 쓴다. 옮긴 양 = Wafer.DieOrigin.
    /// 3. 원점 조회 함수(originLookup)를 주면 DMS 업로드 서비스처럼 TQD_SETUP 저장값을 먼저 쓴다.
    /// 설계: 문서/설계/03_KLARF_Map/2026-10-06_KLARF_Map_Setup_설계.md 3~4절
    /// </summary>
    internal sealed class KlarfMapSource
    {
        /// <summary>
        /// ParserKlarf.GetOriginDieIndex 는 static 이라 다른 화면과 함께 쓰인다.
        /// 읽는 동안만 바꾸고 바로 되돌리며, 동시에 두 번 바꾸지 않도록 잠근다.
        /// </summary>
        private static readonly object OriginLookupLock = new object();

        private KlarfMapSource() { }

        public string FilePath { get; private set; }
        public string DeviceID { get; private set; }
        public string LotID { get; private set; }
        public string SetupID { get; private set; }
        public string StepID { get; private set; }
        public DateTime SetupTime { get; private set; }

        /// <summary>파일 SampleSize (mm). Map 저장값은 WaferSize (µm) 를 쓴다.</summary>
        public int SampleSize { get; private set; }

        /// <summary>Wafer 지름 (µm) = SampleSize * 1000.</summary>
        public double WaferSize { get; private set; }

        /// <summary>Die Pitch (µm).</summary>
        public double PitchX { get; private set; }
        public double PitchY { get; private set; }

        /// <summary>SampleOrientationMarkType (NOTCH / FLAT).</summary>
        public string MarkType { get; private set; }

        /// <summary>OrientationMarkLocation (DOWN / LEFT / UP / RIGHT).</summary>
        public string MarkLocation { get; private set; }

        /// <summary>Parser.GetAngle() 값. DOWN 0 / LEFT 90 / UP 180 / RIGHT 270 (KLARF File View 와 같음).</summary>
        public int NotchAngle { get; private set; }

        /// <summary>Flat 이면 true. 그 외(NOTCH / 없음)는 Notch 로 본다.</summary>
        public bool IsFlat { get; private set; }

        /// <summary>옮긴 양을 TQD_SETUP 저장값에서 가져왔으면 true, 계산기로 구했으면 false.</summary>
        public bool OriginFromDB { get; private set; }

        public ReadOnlyCollection<KlarfMapWafer> Wafers { get; private set; }

        /// <summary>
        /// 파일을 읽고 검사한다. 실패하면 예외를 던지며 일부만 채운 결과는 돌려주지 않는다.
        /// </summary>
        /// <param name="path">KLARF 파일 경로</param>
        /// <param name="originLookup">TQD_SETUP 원점 조회 함수. null 이면 계산기만 쓴다.</param>
        public static KlarfMapSource Load(string path, ParserKlarf.GetOriginDieIndexDelegate originLookup)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                throw new FileNotFoundException("KLARF 파일이 없습니다.", path);

            ParserKlarf parser;
            lock (OriginLookupLock)
            {
                ParserKlarf.GetOriginDieIndexDelegate previous = ParserKlarf.GetOriginDieIndex;
                ParserKlarf.GetOriginDieIndex = originLookup;
                try
                {
                    // 원점 조회는 Parser 생성자 안(Wafer.UpdateDieIndex)에서 일어난다.
                    FileAnalyzer analyzer = new FileAnalyzer(path);
                    parser = analyzer.FileType == FileType.KlarfFile_1_8 ? new ParserKlarf_18(path) : new ParserKlarf(path);
                }
                finally
                {
                    ParserKlarf.GetOriginDieIndex = previous;
                }
            }

            // Parser 는 예외 대신 ErrorFlag 로 실패를 알린다. 검사 순서는 KLARF File View(LoadFile) 와 같다.
            if (parser.ErrorFlag) throw new FormatException(parser.ErrorMessage);
            if (parser.Wafers == null || parser.Wafers.Count == 0)
                throw new FormatException("Wafer 정보가 없습니다. WaferID / Slot / DieOrigin 항목을 확인하세요.");
            if (parser.DiePitchX <= 0 || parser.DiePitchY <= 0)
                throw new FormatException("DiePitch 값이 없습니다.");
            if (parser.SampleSize <= 0)
                throw new FormatException("SampleSize (Wafer 크기) 값이 없습니다.");

            KlarfMapSource source = new KlarfMapSource();
            source.FilePath = path;
            source.DeviceID = parser.DeviceID ?? string.Empty;
            source.LotID = parser.LotID ?? string.Empty;
            source.SetupID = parser.SetupID ?? string.Empty;
            source.StepID = parser.StepID ?? string.Empty;
            source.SetupTime = parser.SetupTimestamp;
            source.SampleSize = parser.SampleSize;
            source.WaferSize = parser.SampleSize * 1000d;
            source.PitchX = parser.DiePitchX;
            source.PitchY = parser.DiePitchY;
            source.MarkType = parser.SampleOrientationMarkType ?? string.Empty;
            source.MarkLocation = parser.OrientationMarkLocation ?? string.Empty;
            source.NotchAngle = parser.GetAngle();
            source.IsFlat = source.MarkType.Trim().Equals("FLAT", StringComparison.OrdinalIgnoreCase);
            source.OriginFromDB = parser.MapOriginFromDB;

            List<KlarfMapWafer> wafers = new List<KlarfMapWafer>(parser.Wafers.Count);
            foreach (Wafer wafer in parser.Wafers)
                wafers.Add(KlarfMapWafer.Create(source, wafer));
            source.Wafers = wafers.AsReadOnly();
            return source;
        }
    }

    /// <summary>
    /// KLARF Wafer 하나의 Map 생성 값. Map 은 Wafer 공통이지만 SampleCenterLocation / 옮긴 양은 Wafer 별 값이다.
    /// 인덱스는 모두 LeftBottom (좌하단 1,1) 기준이다.
    /// </summary>
    internal sealed class KlarfMapWafer
    {
        private KlarfMapWafer() { }

        public string WaferID { get; private set; }
        public int Slot { get; private set; }

        /// <summary>SampleCenterLocation (µm). Map 의 OriginX / OriginY 로 쓴다.</summary>
        public double CenterX { get; private set; }
        public double CenterY { get; private set; }

        /// <summary>Parser 가 원본 인덱스에 더한 값 (Wafer.DieOrigin). 원본 인덱스 = Map 인덱스 - 이 값.</summary>
        public int ShiftX { get; private set; }
        public int ShiftY { get; private set; }

        /// <summary>Wafer 전체 Die 격자 범위 (LeftBottom).</summary>
        public KlarfMapGrid Grid { get; private set; }

        /// <summary>모든 InspectionTest 의 SampleTestPlan 합집합 (중복 제거, LeftBottom). [검사 Die 외 제거] 기준이다.</summary>
        public ReadOnlyCollection<Point> InspectedDies { get; private set; }

        /// <summary>Defect 수 (DefectList 기준).</summary>
        public int DefectCount { get; private set; }

        /// <summary>Probe / 정보 표시용 Defect 원본. 화면은 값을 바꾸지 않는다.</summary>
        public ReadOnlyCollection<DACrux.Base.Defect> Defects { get; private set; }

        /// <summary>
        /// Parser Wafer 에서 값을 옮기고 격자를 계산한다.
        /// 옮긴 양(DieOrigin)은 Parser 가 정수로 저장한 값이므로 정수로 바꿔 쓴다.
        /// </summary>
        internal static KlarfMapWafer Create(KlarfMapSource source, Wafer wafer)
        {
            KlarfMapWafer item = new KlarfMapWafer();
            item.WaferID = wafer.WaferID ?? string.Empty;
            item.Slot = wafer.Slot;
            item.CenterX = wafer.SampleCenterLocationX;
            item.CenterY = wafer.SampleCenterLocationY;
            item.ShiftX = (int)wafer.DieOriginX;
            item.ShiftY = (int)wafer.DieOriginY;
            item.Grid = KlarfMapRule.CalculateGrid(source.WaferSize, source.PitchX, source.PitchY,
                item.CenterX, item.CenterY, item.ShiftX, item.ShiftY);

            HashSet<Point> seen = new HashSet<Point>();
            List<Point> inspected = new List<Point>();
            if (wafer.TestList != null)
            {
                foreach (InspectionTest test in wafer.TestList)
                {
                    if (test.SampleTestPlan == null) continue;
                    foreach (Point point in test.SampleTestPlan)
                        if (seen.Add(point)) inspected.Add(point);
                }
            }
            item.InspectedDies = inspected.AsReadOnly();

            List<DACrux.Base.Defect> defects = new List<DACrux.Base.Defect>();
            if (wafer.DefectList != null) defects.AddRange(wafer.DefectList);
            item.Defects = defects.AsReadOnly();
            item.DefectCount = defects.Count;
            return item;
        }
    }
}
