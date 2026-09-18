using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDataMigrationRemotingService;
using NLog;
using System.Data;

namespace TestDataMigrationRawData
{
    class Program
    {
        public static Logger Log = LogManager.GetLogger("logfile");

        static void Main(string[] args)// factory tWaferSeq mWaferSeq tTableName
        {
            if (args == null || args.Length == 0)
                return;

            System.Threading.Thread.Sleep(10 * 1000);

            string factory = args[0];
            decimal tWaferSeq = Decimal.Parse(args[1]);
            decimal mWaferSeq = Decimal.Parse(args[2]);
            string tTableName = args[3];
            string mTableName = "TD_" + tTableName.Substring(1);

            TestData test = new TestData();
            MiracomTPS mira = new MiracomTPS();
            MiracomTPS.Factory = factory;

            try
            {
                // @TD_ 데이터 INSERT
                DataTable rawDt = test.GetTTableData(tTableName, new decimal[] { tWaferSeq });
                SplitXY(rawDt);

                rawDt.TableName = mTableName;

                if (rawDt != null && rawDt.Rows.Count > 0)
                {
                    foreach (DataRow row in rawDt.Rows)
                        row["WAFER_SEQ"] = mWaferSeq;

                    mira.InsertBulk(rawDt);
                    rawDt.Dispose();
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("ProcessRawData_async error wafSeq={0},NewWafSeq={1}", tWaferSeq, mWaferSeq);
                Log.Error(ex, message + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private static void SplitXY(DataTable dt)
        {
            if (!dt.Columns.Contains("XY"))
                return;

            dt.Columns.Add("X", typeof(decimal));
            dt.Columns.Add("Y", typeof(decimal));

            foreach (DataRow row in dt.Rows)
            {
                decimal xy = (decimal)row["XY"];

                row["X"] = (int)Math.Round(xy / 65536, 0);
                row["Y"] = xy % 65536;
            }

            dt.Columns.Remove("XY");
        }

        public static IMiracomTPS GetRemotingObject(string factory)
        {
            int port = TestDataMigrationRemotingService.MiracomTPS.GetPort(factory);

            IMiracomTPS obj = Activator.GetObject(typeof(IMiracomTPS),
                  String.Format("tcp://localhost:{0}/MiracomTPS", port)) as IMiracomTPS;

            if (obj == null)
                throw new Exception("Remoting object가 null 입니다.");

            return obj;
        }
    }
}
