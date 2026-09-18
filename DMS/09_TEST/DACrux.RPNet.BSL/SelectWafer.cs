using System;
using System.Data;
using DACrux.RPNet.DSL;
using RPNet.Server.RI;

namespace DACrux.RPNet.BSL
{
    public class SelectWafer : Miracom.Middleware.QueryComponent, ISelectWafer
    {
        public DataTable GetProgramSetup(string Program, string lotSeq)
        {
            PROGRAM_SETUP_REV programSetupRev = new PROGRAM_SETUP_REV();
            return programSetupRev.GetProgramSetup(Program, lotSeq);
        }

        public DataTable GetWaferList(string StartTime, string EndTime, string[] strFields, string[] strWhere)
        {
            LOT_WAFER lotWafer = new LOT_WAFER();
            return lotWafer.GetWaferList(StartTime, EndTime, strFields, strWhere);
        }

        public DataTable GetMapData(string PROGRAM, string WaferSeq)
        {
            PGMTABLE pgmtable = new PGMTABLE();
            return pgmtable.GetMapData(PROGRAM, WaferSeq);
        }

        public DataTable GetWaferInfo(string WaferSeq)
        {
            LOT_WAFER lotWafer = new LOT_WAFER();
            return lotWafer.GetWaferInfo(WaferSeq);
        }

        public DataTable GetBinDistribution(string PROGRAM, string WaferSeq)
        {
            PGMTABLE pgmtable = new PGMTABLE();
            return pgmtable.GetBinDistribution(PROGRAM, WaferSeq);
        }

        public DataTable GetBinDistribution(string PROGRAM, string[] WaferSeq)
        {
            PGMTABLE pgmtable = new PGMTABLE();
            return pgmtable.GetBinDistribution(PROGRAM, WaferSeq);
        }

        public DataTable GetProgramLotSeq(string Device, string Lot)
        {
            LOT_WAFER lotWafer = new LOT_WAFER();
            return lotWafer.GetProgramLotSeq(Device, Lot);
        }

        public DataTable GetWaferSeq(string Lot, string WaferId, string ProbeCnt)
        {
            LOT_WAFER lotWafer = new LOT_WAFER();
            return lotWafer.GetWaferSeq(Lot, WaferId, ProbeCnt);
        }

        public DataTable GetDistTestArea()
        {
            LOT_WAFER lotWafer = new LOT_WAFER();
            return lotWafer.GetDistTestArea();
        }

        public DataTable GetDistProgram(string TESTAREA, string WHERE)
        {
            LOT_WAFER lotWafer = new LOT_WAFER();
            return lotWafer.GetDistProgram(TESTAREA, WHERE);
        }

        public DataTable GetDistLot(string TESTAREA, string PROGRAM, string WHERE)
        {
            LOT_WAFER lotWafer = new LOT_WAFER();
            return lotWafer.GetDistLot(TESTAREA, PROGRAM, WHERE);
        }

        public DataTable GetDistWafer(string TESTAREA, string PROGRAM, string LOT, string WHERE)
        {
            LOT_WAFER lotWafer = new LOT_WAFER();
            return lotWafer.GetDistWafer(TESTAREA, PROGRAM, LOT, WHERE);
        }

        public int SampleFunction(int ggg)
        {
            return 0;
        }

        public void SaveMapShift(string device, string xShift, string yShift)
        {
            LOT_WAFER lotWafer = new LOT_WAFER();
            lotWafer.SaveMapShift(device, xShift, yShift);
        }
    }
}
