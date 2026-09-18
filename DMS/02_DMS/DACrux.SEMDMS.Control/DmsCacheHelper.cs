using System;
using System.Collections.Generic;
using System.Data;
using DACrux.Base;
using DACrux.SEMDMS.RO;
using System.Drawing;
using System.Reflection;
using DACrux.Data.Parser.Klarf;

namespace DACrux.SEMDMS.Control
{
    public class DmsCacheHelper
    {
        private static object LockObject = new object();

        public static void InitDmsCache()
        {
            DmsCache.Instance.ClassLookupDel += DefectMapAnalysis.GetClassLookup;
            DmsCache.Instance.WaferDieInfoDel += DefectMapAnalysis.GetWaferDieInfo;
        }

        /// <summary>
        /// Klarf 데이터를 로드합니다.
        /// </summary>
        public static void LoadKlarfData(ParserKlarf[] parserArr, out DefectList defectList)
        {
            defectList = new DefectList();

            foreach (ParserKlarf parser in parserArr)
            {
                foreach (Wafer wafer in parser.Wafers)
                {
                    DmsStepInfo step = new DmsStepInfo();
                    step.Maker = parser.Maker;
                    step.Model = parser.Model;
                    step.Equip = parser.Equip;
                    step.ResultTimestamp = parser.ResultTimestamp.ToString();
                    step.LotID = parser.LotID;
                    step.WaferSize = parser.SampleSize * 1000;
                    step.SampleSize = parser.SampleSize;
                    step.DeviceID = parser.DeviceID;
                    step.SetupID = parser.SetupID;
                    step.SetupTimestamp = parser.SetupTimestamp.ToString();
                    step.StepID = parser.StepID;
                    step.SampleOrientationMarkType = parser.SampleOrientationMarkType;
                    step.DiePitchX = parser.DiePitchX;
                    step.DiePitchY = parser.DiePitchY;
                    step.DieOriginX = (int)wafer.DieOriginX;
                    step.DieOriginY = (int)wafer.DieOriginY;
                    step.WaferID = wafer.WaferID;
                    step.Slot = wafer.Slot;
                    step.Angle = parser.GetAngle();
                    step.SampleCenterLocationX = wafer.SampleCenterLocationX;
                    step.SampleCenterLocationY = wafer.SampleCenterLocationY;
                    step.AreaPerTest = wafer.TestList[0].AreaPerTest;
                    step.Inspector = String.Empty;
                    step.ShotArrayX = step.ShotArrayY = step.ShotStartX = step.ShotStartY = 1;

                    long stepSeq = GetTempStepSeq();
                    DmsWaferDieInfo info = new DmsWaferDieInfo(stepSeq, step, wafer.TestList[0].SampleTestPlan);

                    DmsCache.Instance.WaferDieInfoList.Add(info);

                    foreach (Defect defect in wafer.DefectList)
                        defect.STEP_SEQ = stepSeq;

                    defectList.AddRange(wafer.DefectList);
                }
            }
        }

        private static long GetTempStepSeq()
        {
            DateTime t;

            lock (LockObject)
            {
                t = DateTime.Now;
                System.Threading.Thread.Sleep(1);
            }

            // temp stepSeq는 마이너스 값이 되도록 한다.
            return -long.Parse(String.Format("{0:00}{1:00}{2:00}{3:00}{4:0000}", t.Day, t.Hour, t.Minute, t.Second, t.Millisecond));
        }
    }
}
