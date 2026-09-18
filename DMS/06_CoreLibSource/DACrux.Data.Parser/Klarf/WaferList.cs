using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using DACrux.Base;

namespace DACrux.Data.Parser.Klarf
{
    public class WaferList : List<Wafer>
    {
        public new void Add(Wafer wafer)
        {
            base.Add(wafer);
            Sort();
        }

        public Wafer GetWaferBySlot(int slot)
        {
            foreach (Wafer wafer in this)
            {
                if (wafer.Slot == slot)
                    return wafer;
            }

            return null;
        }

        public override string ToString()
        {
            if (Count == 0)
                return "(empty)";

            StringBuilder sb = new StringBuilder();
            sb.Append("(");

            for (int i = 0; i < Count; i++)
            {
                sb.Append(this[i].WaferID);

                if (i < Count - 1)
                    sb.Append(",");
            }

            sb.Append(")");

            return sb.ToString();
        }
    }

    public class Wafer : IComparable<Wafer>
    {
        public Wafer()
        {
            DefectList = new DefectList();
            ClassifedDefectList = new DefectList();
            ImageDefectList = new DefectList();
            TestList = new InspectionTestList();
            //SummaryList = new SummaryList();
        }

        /// <summary>
        /// Die Index를 변경합니다.
        /// </summary>
        public void UpdateDieIndex(DieIndexSort indexSort)
        {
            int addX = Int32.MaxValue;
            int addY = Int32.MaxValue;

            if (indexSort == DieIndexSort.CenterToLowerLeft)
            {
                // 기존에 저장된 Map 인지를 가져온다. 저장된 값이 있는 경우 그 값을 사용하고
                // 신규 Map인 경우 WaferDieCalculator 를 이용해서 Origin Die 인덱스를 계산한다. 2019.10.21 Taihi,Kim.
                Point originPt = Point.Empty;

                if (ParserKlarf.GetOriginDieIndex != null)
                    originPt = ParserKlarf.GetOriginDieIndex.Invoke(Parser.SetupID, Parser.StepID, Parser.SetupTimestamp);

                Parser.MapOriginFromDB = originPt != Point.Empty;

                if (Parser.MapOriginFromDB)
                {
                    addX = originPt.X;
                    addY = originPt.Y;

                    if (Parser.GetAngle() % 180 == 90)
                    {
                        int tmp = addX;
                        addX = addY;
                        addY = tmp;
                    }
                }
                else
                {
                    WaferDieCalculator calc = new WaferDieCalculator();
                    calc.WaferSize = Parser.SampleSize * 1000;
                    calc.DiePitchX = Parser.DiePitchX;
                    calc.DiePitchY = Parser.DiePitchY;
                    calc.OriginX = SampleCenterLocationX;
                    calc.OriginY = SampleCenterLocationY;
                    calc.Calculate();

                    addX = calc.OriginIndexX;
                    addY = calc.OriginIndexY;
                }
            }
            else // 좌표 조정. 가운데가 (0,0)이 되도록 변경한다.
            {
                addX = -(int)DieOriginX;
                addY = -(int)DieOriginY;
            }

            // Die Origin 업데이트
            DieOriginX = addX;
            DieOriginY = addY;

            AddIndex(addX, addY);
        }

        public void AddIndex(int addX, int addY)
        {
            if (addX != 0 || addY != 0)
            {
                // Die 정보 업데이트
                foreach (InspectionTest test in TestList)
                {
                    for (int i = 0; i < test.SampleTestPlan.Length; i++)
                    {
                        test.SampleTestPlan[i].X += addX;
                        test.SampleTestPlan[i].Y += addY;
                    }
                }

                // Defect 좌표 업데이트
                foreach (Defect d in DefectList)
                {
                    d.XINDEX += addX;
                    d.YINDEX += addY;
                }
            }
        }

        public void CopyTestList(InspectionTestList otestList)
        {
            TestList = otestList.Clone() as InspectionTestList;
        }

        public double DieOriginX { get; internal set; }
        public double DieOriginY { get; internal set; }
        public string WaferID { get; internal set; }
        public int Slot { get; internal set; }
        public double SampleCenterLocationX { get; internal set; }
        public double SampleCenterLocationY { get; internal set; }
        public Dictionary<int, string> ClassLookup { get; internal set; }
        public DefectList DefectList { get; internal set; }
        public DefectList ClassifedDefectList { get; internal set; }
        public DefectList ImageDefectList { get; internal set; }
        public InspectionTestList TestList { get; private set; }
        public ParserKlarf Parser { get; internal set; }
        public List<string> DefectHeaders { get; internal set; }

        public override string ToString()
        {
            return WaferID;
        }

        public void SetDefect(Defect[] defectArr)
        {
            if (defectArr == null)
                return;

            DefectList.Clear();

            // DefectList
            foreach (DACrux.Base.Defect d in defectArr)
            {
                Defect defect = d.Clone() as Defect;
                DefectList.Add(defect);

                if (defect.IMAGECOUNT > 0)
                    ImageDefectList.Add(defect);
            }
        }

        public int CompareTo(Wafer wafer)
        {
            return WaferID.CompareTo(wafer.WaferID);
        }
    }
}
