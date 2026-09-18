//#define SYNC
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using System.Threading;
using FabTwoToMigrationTools.Source;
using FabTwoToMigrationTools.Target;

namespace TestDataMigration
{
    public partial class FrmMain : Form
    {
        public static Logger _log = LogManager.GetLogger("logfile");
        public static string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static string DATE_FORMAT = "yyyy-MM-dd";
        public static readonly int THREAD_COUNT = 6;

        static object LockObject = new object();

        bool _exit;
        Semaphore _semaphore;
        CountdownEvent _countdown;

        public static Logger Log
        {
            get { return _log; }
        }

        public string Factory
        {
            get { return rdoFab1.Checked ? rdoFab1.Text : rdoFAB2.Text; }
        }

        public FrmMain()
        {
            InitializeComponent();

            dtStart.CustomFormat = DATETIME_FORMAT;
            dtEnd.CustomFormat = DATETIME_FORMAT;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            SettingData setting = new SettingData();

            if (setting.HasValue("START_DATE"))
                dtStart.Value = setting.GetValue<DateTime>("START_DATE");

            if (setting.HasValue("END_DATE"))
                dtEnd.Value = setting.GetValue<DateTime>("END_DATE");

            btnRun.Enabled = false;
            btnStop.Enabled = false;
            btnPROGRAM_PARM_DEF.Enabled = false;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("종료하시겠습니까?", "종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }


            _exit = true;

            SettingData setting = new SettingData();
            setting.SetValue("START_DATE", dtStart.Value.ToString(DATETIME_FORMAT));
            setting.SetValue("END_DATE", dtEnd.Value.ToString(DATETIME_FORMAT));
            setting.Save();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!(rdoFab1.Checked || rdoFAB2.Checked))
            {
                MessageBox.Show("Factory를 선택하세요.");
                return;
            }

            _test = new TestData();
            _mira = new MiracomTPS(Factory);

            string conn1 = _test.Sysdate();
            string conn2 = _mira.Sysdate();

            MessageBox.Show(String.Format("접속테스트: \r\n - LEGACY:{0}\r\n - DACRUX:{1}", conn1, conn2));

            if (!String.IsNullOrEmpty(conn1) && !String.IsNullOrEmpty(conn2))
            {
                btnRun.Enabled = true;
                btnPROGRAM_PARM_DEF.Enabled = true;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!(rdoFab1.Checked || rdoFAB2.Checked))
            {
                MessageBox.Show("Factory를 선택하세요.");
                return;
            }

            if (MessageBox.Show("실행하시겠습니까?",
                "실행", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                return;

            Text = String.Format("[{0}] {1}~{2}", Factory, dtStart.Value.ToString(DATE_FORMAT), dtEnd.Value.ToString(DATE_FORMAT));

            btnRun.Enabled = false;
            btnStop.Enabled = true;

            Process();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("중지하시겠습니까? 처리중인 날짜의 데이터는 모두 처리된 후 중지됩니다.",
                "중지", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                return;

            btnStop.Enabled = false;
        }

        TestData _test;
        MiracomTPS _mira;

        void WriteLog(string format, double val)
        {
            if (val > 0.5)
                Log.Info(format, val);
        }

        private void Process()
        {
            Log.Info("프로그램 실행 [{0}]", Factory);
            DateTime date = dtStart.Value;

            while (date < dtEnd.Value && btnStop.Enabled)
            {
                // LOT LIST
                DataTable lotDt = _test.GetLot(Factory, date, date.AddDays(1));
                string message = String.Format("Date = {0}, Lot Count = {1}", date.ToString(DATE_FORMAT), lotDt.Rows.Count);
                Log.Info(message);
                AppendInfo(message);

                foreach (DataRow lotRow in lotDt.Rows)
                {
                    // LOT 단위
                    string lotID = lotRow["LOT_ID"].ToString();
                    string program = lotRow["PROGRAM"].ToString();
                    decimal lotSeq = decimal.Parse(lotRow["LOT_SEQ"].ToString());
                    decimal oldLotSeq = Decimal.Parse(lotRow["OLD_LOT_SEQ"].ToString());

                    try
                    {
                        // WAFER LIST
                        DataTable wafDt = _test.GetWafer(lotID, lotSeq);
                        DataTable saveWafDt = wafDt.Clone(); // 테이블 구조 복제
                        Log.Info("Lot ID={0}, Wafer Cnt={1}", lotID, wafDt.Rows.Count);

                        List<RawDataInfo> rawDataInfoList = new List<RawDataInfo>();

                        foreach (DataRow wafRow in wafDt.Rows)
                        {
                            // WAFER 단위
                            decimal tWafSeq = Decimal.Parse(wafRow["OLD_WAFER_SEQ"].ToString());
                            wafRow["PROGRAM"] = program;
                            wafRow["PROGRAM_REV"] = "0";

                            // 최신 프로그램 파라미터 가져오기
                            //DataTable paraDt = _test.GetProgramParameter(Factory, program);
                            
                            //if (paraDt != null && paraDt.Rows.Count > 0)
                            //{
                            //    // XY 필드 없애고 X,Y필드로 분리
                            //    AppendXAndYRow(paraDt);
                            //}
                            //else
                            //{
                            //    Log.Error("파라미터가 없습니다. {0}", program);
                            //}
                            
                            //// 해당 PPD_SEQ에 대한 데이터가 MIRACOM DB에 있는지 확인
                            //if (!_mira.ExistsProgramParameter(program) && paraDt != null && paraDt.Rows.Count > 0)
                            //{
                            //    // 프로그램 별 파라미터 처리하기
                            //    paraDt.TableName = "TQP_PARASPEC";
                            //    _mira.InsertBulk(paraDt);
                            //}
                            
                            // @TD_테이블 필드 확인
                            DataTable tableDt = _test.GetProgramTable(program);
                            
                            //// @TQD_DATA_TABLES
                            //_mira.InsertProgramTable(Factory, program, ToArray(tableDt, 0));
                            decimal mWafSeq = _mira.NewWaferSeq();
                            
                            // RAW 데이터 처리
                            foreach (DataRow tableRow in tableDt.Rows)
                            {
                                rawDataInfoList.Add(new RawDataInfo(tWafSeq, mWafSeq, tableRow));
                            }
                            
                            // 새 WAFER_SEQ 로 업데이트
                            wafRow["WAFER_SEQ"] = mWafSeq;

                            saveWafDt.Rows.Add(wafRow.ItemArray);
                        }

                        wafDt.Dispose();

                        _semaphore = new Semaphore(THREAD_COUNT, THREAD_COUNT);
                        _countdown = new CountdownEvent(rawDataInfoList.Count);

                        // RAW 데이터 비동기 처리
                        foreach (RawDataInfo info in rawDataInfoList)
                        {
#if SYNC
                            ProcessRawData_async(info);
#else
                            Thread thread = new Thread(new ParameterizedThreadStart(ProcessRawData_async));
                            thread.IsBackground = true;
                            thread.Start(info);
#endif
                        }

                        // RAW 데이터 처리가 완료될때까지 대기
                        _countdown.Wait();
                        _countdown.Dispose();
                        _semaphore.Dispose();
                        Application.DoEvents();

                        // @TQP_PROGRAM :  프로그램 정보 없는 경우 등록
                        _mira.MergeIntoProgram(Factory, program, lotRow["TEST_AREA"].ToString(), lotRow["DEVICE"].ToString());

                        string maxPsSeq = _test.GetMaxPsSeq(program);
                        
                        /*// @TQP_BINDESC : 프로그램 별 BIN 기준정보 처리 (최신 PS_SEQ 기준)
                        //if (String.Compare(maxPsSeq, _mira.GetMaxPsSeq(program)) > 0)
                        //{
                        //    DataTable binDt = _test.GetBin(program);
                        //    DataTable miraBinDt = EmptyBinTable();

                        //    foreach (DataRow binRow in binDt.Rows)
                        //    {
                        //        decimal bin = (decimal)binRow["BIN"];
                        //        string name = binRow["BIN_LABEL"].ToString();
                        //        string color = binRow["BIN_COLOR"].ToString();
                        //        string gec = (decimal)binRow["GECBINS"] == 1 ? "Y" : "N";

                        //        miraBinDt.Rows.Add(program, bin, name, color, gec);
                        //    }

                        //    _mira.DeleteBin(program);
                        //    _mira.InsertBulk(miraBinDt);
                        //}*/

                        // CASE WHEN LENGTH(DEVICE) < 8 THEN SUBSTR(DEVICE,1,4) ELSE SUBSTR(DEVICE,5,4) END
                        string deviceAlias = (string)lotRow["DEVICE"];
                        deviceAlias = deviceAlias.Length >= 8 ? deviceAlias.Substring(4, 4) : deviceAlias.Substring(0, Math.Min(deviceAlias.Length, 4));
                        lotRow["DEVICE_ALIAS"] = deviceAlias;
                     
                        // @TQP_WAFER INSERT
                        saveWafDt.Columns.Remove("PPD_SEQ");
                        saveWafDt.TableName = "TQP_WAFER";
                        _mira.InsertBulk(saveWafDt);
                        saveWafDt.Dispose();

                        // @TQP_LOT 로우 단위로 INSERT
                        DataTable saveLotDt = lotDt.Clone();
                        saveLotDt.Rows.Add(lotRow.ItemArray);
                        saveLotDt.TableName = "TQP_LOT";
                        _mira.InsertBulk(saveLotDt);
                        saveLotDt.Dispose();
                        
                        // @TQP_LOT_SUM, @TQP_WAFER_SUM
                        Summary(lotSeq);
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        message = String.Format("Date={0}, Lot ID={1}", date.ToString(DATE_FORMAT), lotID);
                        Log.Error(ex, message + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                        AppendInfo("###### ERROR #####" + message + ex.Message);
                    }

                    Application.DoEvents();
                }

                lotDt.Dispose();

                if (_exit) return;

                // END
                Application.DoEvents();
                date = date.AddDays(1);
                GC.Collect();
            }

            btnRun.Enabled = true;
            btnStop.Enabled = false;
            Log.Info("프로그램 종료");
        }

        private void ProcessRawData_async(object parameter)
        {
            _semaphore.WaitOne();

            RawDataInfo info = parameter as RawDataInfo;

            try
            {
                string tTableName = info.TableRow[0].ToString();
                string mTableName = "TD_" + tTableName.Substring(1);

                List<string> paraList = new List<string>();

                DataTable paraDt = _test.GetParameterFromTable(tTableName);

                // 테이블 컬럼 가져오기
                foreach (DataRow paraRow in paraDt.Rows)
                    paraList.Add(paraRow[0].ToString());

                if (paraList.Contains("XY"))
                {
                    paraList.Remove("XY");
                    paraList.Add("X");
                    paraList.Add("Y");
                }

                paraDt.Dispose();

                lock (LockObject)
                {
                    // @TD_PROGRAM : 테이블 필드 체크하여 추가된 필드가 있는 경우 처리
                    _mira.UpdateTableField(mTableName, paraList);
                }

                paraList.Clear();

                // @TD_PROGRAM 데이터 INSERT
                DataTable rawDt = _test.GetTTableData(tTableName, new decimal[] { info.WafSeq });
                SplitXY(rawDt);
                rawDt.TableName = mTableName;

                if (rawDt != null && rawDt.Rows.Count > 0)
                {
                    foreach (DataRow row in rawDt.Rows)
                        row["WAFER_SEQ"] = info.MiracomWafSeq;

                    _mira.InsertBulk(rawDt);
                    rawDt.Dispose();
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("ProcessRawData_async error wafSeq={0},NewWafSeq={1}", info.WafSeq, info.MiracomWafSeq);
                Log.Error(ex, message + ex.Message + Environment.NewLine + ex.StackTrace);
                AppendInfo("###### ERROR #####" + message + ex.Message);
            }
            finally
            {
                _semaphore.Release();
                _countdown.Signal();
            }
        }

        private void AppendInfo(string info)
        {
            listBox1.SelectedIndex = listBox1.Items.Add(info);
        }

        // parameter에 xy 데이터 삭제 및 x, y 데이터 추가하기
        private void AppendXAndYRow(DataTable dt)
        {
            DataRow[] drParams = dt.Select("[PARAM_NAME] = 'XY'");
            foreach (DataRow row in drParams)
            {
                DataRow inRow = dt.NewRow();
                inRow["FACTORY"] = row["FACTORY"];
                inRow["PROGRAM"] = row["PROGRAM"];
                inRow["PPD_SEQ"] = row["PPD_SEQ"]; // WAFER 데이터 MAPPING을 위해 필요
                inRow["PROGRAM_REV"] = row["PROGRAM_REV"];
                inRow["PARAM_INDEX"] = -1;
                inRow["PARAM_NAME"] = "X";
                inRow["PARAM_TYPE"] = row["PARAM_TYPE"];
                inRow["PARAM_DESC"] = row["PARAM_DESC"];
                inRow["CREATE_TIME"] = row["CREATE_TIME"];
                inRow["CREATE_USER"] = row["CREATE_USER"];
                inRow["UPDATE_TIME"] = row["UPDATE_TIME"];
                inRow["UPDATE_USER"] = row["UPDATE_USER"];
                inRow["TABLE_NAME"] = row["TABLE_NAME"];
                inRow["DECIMAL_PLACES"] = row["DECIMAL_PLACES"];
                inRow["RUNTIME_DEFINED"] = row["RUNTIME_DEFINED"];
                inRow["UOM"] = row["UOM"];
                inRow["LSL"] = row["LSL"];
                inRow["TARGET"] = row["TARGET"];
                inRow["USL"] = row["USL"];
                inRow["LCL"] = row["LCL"];
                inRow["UCL"] = row["UCL"];
                inRow["TEST_LOW"] = row["TEST_LOW"];
                inRow["TEST_HIGH"] = row["TEST_HIGH"];
                dt.Rows.Add(inRow);

                inRow = dt.NewRow();
                inRow["FACTORY"] = row["FACTORY"];
                inRow["PROGRAM"] = row["PROGRAM"];
                inRow["PPD_SEQ"] = row["PPD_SEQ"]; // WAFER 데이터 MAPPING을 위해 필요
                inRow["PROGRAM_REV"] = row["PROGRAM_REV"];
                inRow["PARAM_INDEX"] = 0;
                inRow["PARAM_NAME"] = "Y";
                inRow["PARAM_TYPE"] = row["PARAM_TYPE"];
                inRow["PARAM_DESC"] = row["PARAM_DESC"];
                inRow["CREATE_TIME"] = row["CREATE_TIME"];
                inRow["CREATE_USER"] = row["CREATE_USER"];
                inRow["UPDATE_TIME"] = row["UPDATE_TIME"];
                inRow["UPDATE_USER"] = row["UPDATE_USER"];
                inRow["TABLE_NAME"] = row["TABLE_NAME"];
                inRow["DECIMAL_PLACES"] = row["DECIMAL_PLACES"];
                inRow["RUNTIME_DEFINED"] = row["RUNTIME_DEFINED"];
                inRow["UOM"] = row["UOM"];
                inRow["LSL"] = row["LSL"];
                inRow["TARGET"] = row["TARGET"];
                inRow["USL"] = row["USL"];
                inRow["LCL"] = row["LCL"];
                inRow["UCL"] = row["UCL"];
                inRow["TEST_LOW"] = row["TEST_LOW"];
                inRow["TEST_HIGH"] = row["TEST_HIGH"];
                dt.Rows.Add(inRow);

                dt.Rows.Remove(row);
            }
        }

        private DataTable EmptyBinTable()
        {
            DataTable dt = new DataTable("TQP_BINDESC");
            dt.Columns.Add("PROGRAM", typeof(string));
            dt.Columns.Add("BIN", typeof(decimal));
            dt.Columns.Add("BIN_NAME", typeof(string));
            dt.Columns.Add("COLOR", typeof(string));
            dt.Columns.Add("HIGH_GEC", typeof(string));
            return dt;
        }

        private DataTable EmptyLotSumTable()
        {
            DataTable dt = new DataTable("TQP_LOT_SUM");
            dt.Columns.Add("CUSTOMER", typeof(string));
            dt.Columns.Add("FACILITY", typeof(string));
            dt.Columns.Add("PRODUCT", typeof(string));
            dt.Columns.Add("DEVICE_ALIAS", typeof(string));
            dt.Columns.Add("TESTAREA", typeof(string));
            dt.Columns.Add("PROGRAM", typeof(string));
            dt.Columns.Add("MOTHER_LOT_ID", typeof(string));
            dt.Columns.Add("LOT_ID", typeof(string));
            dt.Columns.Add("LOT_SEQ", typeof(decimal));
            dt.Columns.Add("START_TIME", typeof(DateTime));
            dt.Columns.Add("END_TIME", typeof(DateTime));
            dt.Columns.Add("LOSS_DIE", typeof(decimal));
            dt.Columns.Add("TESTED_DIE", typeof(decimal));
            dt.Columns.Add("YIELD", typeof(decimal));
            dt.Columns.Add("FTA", typeof(decimal));
            dt.Columns.Add("BIN1", typeof(decimal));
            dt.Columns.Add("BIN2", typeof(decimal));
            dt.Columns.Add("BIN3", typeof(decimal));
            dt.Columns.Add("BIN4", typeof(decimal));
            dt.Columns.Add("BIN5", typeof(decimal));
            dt.Columns.Add("BIN6", typeof(decimal));
            dt.Columns.Add("BIN7", typeof(decimal));
            dt.Columns.Add("BIN8", typeof(decimal));
            dt.Columns.Add("BIN9", typeof(decimal));
            dt.Columns.Add("BIN10", typeof(decimal));
            dt.Columns.Add("BIN11", typeof(decimal));
            dt.Columns.Add("BIN12", typeof(decimal));
            dt.Columns.Add("BIN13", typeof(decimal));
            dt.Columns.Add("BIN14", typeof(decimal));
            dt.Columns.Add("BIN15", typeof(decimal));
            dt.Columns.Add("BIN16", typeof(decimal));
            dt.Columns.Add("BIN17", typeof(decimal));
            dt.Columns.Add("BIN18", typeof(decimal));
            dt.Columns.Add("BIN19", typeof(decimal));
            dt.Columns.Add("BIN20", typeof(decimal));
            dt.Columns.Add("BIN21", typeof(decimal));
            dt.Columns.Add("BIN22", typeof(decimal));
            dt.Columns.Add("BIN23", typeof(decimal));
            dt.Columns.Add("BIN24", typeof(decimal));
            dt.Columns.Add("BIN25", typeof(decimal));
            dt.Columns.Add("BIN26", typeof(decimal));
            dt.Columns.Add("BIN27", typeof(decimal));
            dt.Columns.Add("BIN28", typeof(decimal));
            dt.Columns.Add("BIN29", typeof(decimal));
            dt.Columns.Add("BIN30", typeof(decimal));
            dt.Columns.Add("BIN31", typeof(decimal));
            dt.Columns.Add("BIN32", typeof(decimal));
            dt.Columns.Add("BIN33", typeof(decimal));
            dt.Columns.Add("BIN34", typeof(decimal));
            dt.Columns.Add("BIN35", typeof(decimal));
            dt.Columns.Add("BIN36", typeof(decimal));
            dt.Columns.Add("BIN37", typeof(decimal));
            dt.Columns.Add("BIN38", typeof(decimal));
            dt.Columns.Add("BIN39", typeof(decimal));
            dt.Columns.Add("BIN40", typeof(decimal));
            dt.Columns.Add("BIN41", typeof(decimal));
            dt.Columns.Add("BIN42", typeof(decimal));
            dt.Columns.Add("BIN43", typeof(decimal));
            dt.Columns.Add("BIN44", typeof(decimal));
            dt.Columns.Add("BIN45", typeof(decimal));
            dt.Columns.Add("BIN46", typeof(decimal));
            dt.Columns.Add("BIN47", typeof(decimal));
            dt.Columns.Add("BIN48", typeof(decimal));
            dt.Columns.Add("BIN49", typeof(decimal));
            dt.Columns.Add("BIN50", typeof(decimal));
            dt.Columns.Add("BIN51", typeof(decimal));
            dt.Columns.Add("BIN52", typeof(decimal));
            dt.Columns.Add("BIN53", typeof(decimal));
            dt.Columns.Add("BIN54", typeof(decimal));
            dt.Columns.Add("BIN55", typeof(decimal));
            dt.Columns.Add("BIN56", typeof(decimal));
            dt.Columns.Add("BIN57", typeof(decimal));
            dt.Columns.Add("BIN58", typeof(decimal));
            dt.Columns.Add("BIN59", typeof(decimal));
            dt.Columns.Add("BIN60", typeof(decimal));
            dt.Columns.Add("BIN61", typeof(decimal));
            dt.Columns.Add("BIN62", typeof(decimal));
            dt.Columns.Add("BIN63", typeof(decimal));
            dt.Columns.Add("BIN64", typeof(decimal));
            dt.Columns.Add("BIN65", typeof(decimal));
            dt.Columns.Add("BIN66", typeof(decimal));
            dt.Columns.Add("BIN67", typeof(decimal));
            dt.Columns.Add("BIN68", typeof(decimal));
            dt.Columns.Add("BIN69", typeof(decimal));
            dt.Columns.Add("BIN70", typeof(decimal));
            dt.Columns.Add("BIN71", typeof(decimal));
            dt.Columns.Add("BIN72", typeof(decimal));
            dt.Columns.Add("BIN73", typeof(decimal));
            dt.Columns.Add("BIN74", typeof(decimal));
            dt.Columns.Add("BIN75", typeof(decimal));
            dt.Columns.Add("BIN76", typeof(decimal));
            dt.Columns.Add("BIN77", typeof(decimal));
            dt.Columns.Add("BIN78", typeof(decimal));
            dt.Columns.Add("BIN79", typeof(decimal));
            dt.Columns.Add("BIN80", typeof(decimal));
            dt.Columns.Add("BIN81", typeof(decimal));
            dt.Columns.Add("BIN82", typeof(decimal));
            dt.Columns.Add("BIN83", typeof(decimal));
            dt.Columns.Add("BIN84", typeof(decimal));
            dt.Columns.Add("BIN85", typeof(decimal));
            dt.Columns.Add("BIN86", typeof(decimal));
            dt.Columns.Add("BIN87", typeof(decimal));
            dt.Columns.Add("BIN88", typeof(decimal));
            dt.Columns.Add("BIN89", typeof(decimal));
            dt.Columns.Add("BIN90", typeof(decimal));
            dt.Columns.Add("BIN91", typeof(decimal));
            dt.Columns.Add("BIN92", typeof(decimal));
            dt.Columns.Add("BIN93", typeof(decimal));
            dt.Columns.Add("BIN94", typeof(decimal));
            dt.Columns.Add("BIN95", typeof(decimal));
            dt.Columns.Add("BIN96", typeof(decimal));
            dt.Columns.Add("BIN97", typeof(decimal));
            dt.Columns.Add("BIN98", typeof(decimal));
            dt.Columns.Add("BIN99", typeof(decimal));
            dt.Columns.Add("BIN100", typeof(decimal));
            dt.Columns.Add("GEC", typeof(decimal));
            dt.Columns.Add("WAFERS", typeof(decimal));
            dt.Columns.Add("BIN0", typeof(decimal));
            return dt;
        }

        private DataTable EmptyWaferSumTable()
        {
            DataTable dt = new DataTable("TQP_WAFER_SUM");
            dt.Columns.Add("CUSTOMER", typeof(string));
            dt.Columns.Add("FACILITY", typeof(string));
            dt.Columns.Add("PRODUCT", typeof(string));
            dt.Columns.Add("DEVICE_ALIAS", typeof(string));
            dt.Columns.Add("TESTAREA", typeof(string));
            dt.Columns.Add("PROGRAM", typeof(string));
            dt.Columns.Add("MOTHER_LOT_ID", typeof(string));
            dt.Columns.Add("LOT_ID", typeof(string));
            dt.Columns.Add("LOT_SEQ", typeof(decimal));
            dt.Columns.Add("WAFER_ID", typeof(string));
            dt.Columns.Add("WAFER_SEQ", typeof(decimal));
            dt.Columns.Add("TESTER", typeof(string));
            dt.Columns.Add("PROBE_CARD", typeof(string));
            dt.Columns.Add("OPERATOR", typeof(string));
            dt.Columns.Add("PROBE_CNT", typeof(decimal));
            dt.Columns.Add("NETDIE", typeof(decimal));
            dt.Columns.Add("START_TIME", typeof(DateTime));
            dt.Columns.Add("END_TIME", typeof(DateTime));
            dt.Columns.Add("WAFER_CAT", typeof(decimal));
            dt.Columns.Add("LOSS_DIE", typeof(decimal));
            dt.Columns.Add("TESTED_DIE", typeof(decimal));
            dt.Columns.Add("YIELD", typeof(decimal));
            dt.Columns.Add("FTA", typeof(decimal));
            dt.Columns.Add("BIN1", typeof(decimal));
            dt.Columns.Add("BIN2", typeof(decimal));
            dt.Columns.Add("BIN3", typeof(decimal));
            dt.Columns.Add("BIN4", typeof(decimal));
            dt.Columns.Add("BIN5", typeof(decimal));
            dt.Columns.Add("BIN6", typeof(decimal));
            dt.Columns.Add("BIN7", typeof(decimal));
            dt.Columns.Add("BIN8", typeof(decimal));
            dt.Columns.Add("BIN9", typeof(decimal));
            dt.Columns.Add("BIN10", typeof(decimal));
            dt.Columns.Add("BIN11", typeof(decimal));
            dt.Columns.Add("BIN12", typeof(decimal));
            dt.Columns.Add("BIN13", typeof(decimal));
            dt.Columns.Add("BIN14", typeof(decimal));
            dt.Columns.Add("BIN15", typeof(decimal));
            dt.Columns.Add("BIN16", typeof(decimal));
            dt.Columns.Add("BIN17", typeof(decimal));
            dt.Columns.Add("BIN18", typeof(decimal));
            dt.Columns.Add("BIN19", typeof(decimal));
            dt.Columns.Add("BIN20", typeof(decimal));
            dt.Columns.Add("BIN21", typeof(decimal));
            dt.Columns.Add("BIN22", typeof(decimal));
            dt.Columns.Add("BIN23", typeof(decimal));
            dt.Columns.Add("BIN24", typeof(decimal));
            dt.Columns.Add("BIN25", typeof(decimal));
            dt.Columns.Add("BIN26", typeof(decimal));
            dt.Columns.Add("BIN27", typeof(decimal));
            dt.Columns.Add("BIN28", typeof(decimal));
            dt.Columns.Add("BIN29", typeof(decimal));
            dt.Columns.Add("BIN30", typeof(decimal));
            dt.Columns.Add("BIN31", typeof(decimal));
            dt.Columns.Add("BIN32", typeof(decimal));
            dt.Columns.Add("BIN33", typeof(decimal));
            dt.Columns.Add("BIN34", typeof(decimal));
            dt.Columns.Add("BIN35", typeof(decimal));
            dt.Columns.Add("BIN36", typeof(decimal));
            dt.Columns.Add("BIN37", typeof(decimal));
            dt.Columns.Add("BIN38", typeof(decimal));
            dt.Columns.Add("BIN39", typeof(decimal));
            dt.Columns.Add("BIN40", typeof(decimal));
            dt.Columns.Add("BIN41", typeof(decimal));
            dt.Columns.Add("BIN42", typeof(decimal));
            dt.Columns.Add("BIN43", typeof(decimal));
            dt.Columns.Add("BIN44", typeof(decimal));
            dt.Columns.Add("BIN45", typeof(decimal));
            dt.Columns.Add("BIN46", typeof(decimal));
            dt.Columns.Add("BIN47", typeof(decimal));
            dt.Columns.Add("BIN48", typeof(decimal));
            dt.Columns.Add("BIN49", typeof(decimal));
            dt.Columns.Add("BIN50", typeof(decimal));
            dt.Columns.Add("BIN51", typeof(decimal));
            dt.Columns.Add("BIN52", typeof(decimal));
            dt.Columns.Add("BIN53", typeof(decimal));
            dt.Columns.Add("BIN54", typeof(decimal));
            dt.Columns.Add("BIN55", typeof(decimal));
            dt.Columns.Add("BIN56", typeof(decimal));
            dt.Columns.Add("BIN57", typeof(decimal));
            dt.Columns.Add("BIN58", typeof(decimal));
            dt.Columns.Add("BIN59", typeof(decimal));
            dt.Columns.Add("BIN60", typeof(decimal));
            dt.Columns.Add("BIN61", typeof(decimal));
            dt.Columns.Add("BIN62", typeof(decimal));
            dt.Columns.Add("BIN63", typeof(decimal));
            dt.Columns.Add("BIN64", typeof(decimal));
            dt.Columns.Add("BIN65", typeof(decimal));
            dt.Columns.Add("BIN66", typeof(decimal));
            dt.Columns.Add("BIN67", typeof(decimal));
            dt.Columns.Add("BIN68", typeof(decimal));
            dt.Columns.Add("BIN69", typeof(decimal));
            dt.Columns.Add("BIN70", typeof(decimal));
            dt.Columns.Add("BIN71", typeof(decimal));
            dt.Columns.Add("BIN72", typeof(decimal));
            dt.Columns.Add("BIN73", typeof(decimal));
            dt.Columns.Add("BIN74", typeof(decimal));
            dt.Columns.Add("BIN75", typeof(decimal));
            dt.Columns.Add("BIN76", typeof(decimal));
            dt.Columns.Add("BIN77", typeof(decimal));
            dt.Columns.Add("BIN78", typeof(decimal));
            dt.Columns.Add("BIN79", typeof(decimal));
            dt.Columns.Add("BIN80", typeof(decimal));
            dt.Columns.Add("BIN81", typeof(decimal));
            dt.Columns.Add("BIN82", typeof(decimal));
            dt.Columns.Add("BIN83", typeof(decimal));
            dt.Columns.Add("BIN84", typeof(decimal));
            dt.Columns.Add("BIN85", typeof(decimal));
            dt.Columns.Add("BIN86", typeof(decimal));
            dt.Columns.Add("BIN87", typeof(decimal));
            dt.Columns.Add("BIN88", typeof(decimal));
            dt.Columns.Add("BIN89", typeof(decimal));
            dt.Columns.Add("BIN90", typeof(decimal));
            dt.Columns.Add("BIN91", typeof(decimal));
            dt.Columns.Add("BIN92", typeof(decimal));
            dt.Columns.Add("BIN93", typeof(decimal));
            dt.Columns.Add("BIN94", typeof(decimal));
            dt.Columns.Add("BIN95", typeof(decimal));
            dt.Columns.Add("BIN96", typeof(decimal));
            dt.Columns.Add("BIN97", typeof(decimal));
            dt.Columns.Add("BIN98", typeof(decimal));
            dt.Columns.Add("BIN99", typeof(decimal));
            dt.Columns.Add("BIN100", typeof(decimal));
            dt.Columns.Add("GEC", typeof(decimal));
            dt.Columns.Add("BIN0", typeof(decimal));
            return dt;
        }

        private void SplitXY(DataTable dt)
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

        private void Summary(decimal lotSeq)
        {
            if (_mira.ExistsData(lotSeq))
                return;

            //MiracomTPS m = new MiracomTPS(GetSelectItem());
            DataTable waferDt = _mira.GetWaferSeq(lotSeq);

            DataTable lotSum = EmptyLotSumTable();
            DataTable wafSum = EmptyWaferSumTable();

            foreach (DataRow waferRow in waferDt.Rows)
            {
                decimal waferSeq = decimal.Parse(waferRow["WAFER_SEQ"].ToString());
                string ttable = _mira.GetBinTTable(waferSeq);
                Dictionary<string, decimal> dic = _mira.GetBinCount(ttable, waferSeq);
                DataTable wafSumDt = _mira.GetWaferSum(waferSeq);

                if (wafSumDt == null || wafSumDt.Rows.Count == 0)
                    continue;

                DataRow newWaf = wafSum.NewRow();
                newWaf["CUSTOMER"] = wafSumDt.Rows[0]["CUSTOMER"];
                newWaf["FACILITY"] = wafSumDt.Rows[0]["FACILITY"];
                newWaf["PRODUCT"] = wafSumDt.Rows[0]["PRODUCT"];
                newWaf["DEVICE_ALIAS"] = wafSumDt.Rows[0]["DEVICE_ALIAS"];
                newWaf["TESTAREA"] = wafSumDt.Rows[0]["TESTAREA"];
                newWaf["PROGRAM"] = wafSumDt.Rows[0]["PROGRAM"];
                newWaf["MOTHER_LOT_ID"] = wafSumDt.Rows[0]["MOTHER_LOT_ID"];
                newWaf["LOT_ID"] = wafSumDt.Rows[0]["LOT_ID"];
                newWaf["LOT_SEQ"] = wafSumDt.Rows[0]["LOT_SEQ"];
                newWaf["WAFER_ID"] = wafSumDt.Rows[0]["WAFER_ID"];
                newWaf["WAFER_SEQ"] = wafSumDt.Rows[0]["WAFER_SEQ"];
                newWaf["TESTER"] = wafSumDt.Rows[0]["TESTER"];
                newWaf["PROBE_CARD"] = wafSumDt.Rows[0]["PROBE_CARD"];
                newWaf["OPERATOR"] = wafSumDt.Rows[0]["OPERATOR"];
                newWaf["PROBE_CNT"] = wafSumDt.Rows[0]["PROBE_CNT"];
                newWaf["START_TIME"] = wafSumDt.Rows[0]["START_TIME"];
                newWaf["END_TIME"] = wafSumDt.Rows[0]["END_TIME"];
                newWaf["WAFER_CAT"] = wafSumDt.Rows[0]["WAFER_CAT"];

                if (dic != null)
                {
                    double sum = 0, good = 0;

                    foreach (var item in dic)
                    {
                        if (item.Key == "BIN1")
                            good += (int)item.Value;

                        newWaf[item.Key] = item.Value;
                        sum += (int)item.Value;
                    }

                    newWaf["TESTED_DIE"] = sum;
                    newWaf["YIELD"] = Math.Round(100 * good / sum, 2);
                }

                wafSum.Rows.Add(newWaf);
            }

            if (wafSum != null && waferDt.Rows.Count > 0)
            {
                _mira.InsertBulk(wafSum);
                wafSum.Dispose();
            }

            DataTable lotSumDt = _mira.GetLotSum(lotSeq);

            if (lotSumDt == null || lotSumDt.Rows.Count == 0)
                return;

            DataRow newLot = lotSum.NewRow();
            newLot["CUSTOMER"] = lotSumDt.Rows[0]["CUSTOMER"];
            newLot["FACILITY"] = lotSumDt.Rows[0]["FACILITY"];
            newLot["PRODUCT"] = lotSumDt.Rows[0]["PRODUCT"];
            newLot["DEVICE_ALIAS"] = lotSumDt.Rows[0]["DEVICE_ALIAS"];
            newLot["TESTAREA"] = lotSumDt.Rows[0]["TESTAREA"];
            newLot["PROGRAM"] = lotSumDt.Rows[0]["PROGRAM"];
            newLot["MOTHER_LOT_ID"] = lotSumDt.Rows[0]["MOTHER_LOT_ID"];
            newLot["LOT_ID"] = lotSumDt.Rows[0]["LOT_ID"];
            newLot["LOT_SEQ"] = lotSumDt.Rows[0]["LOT_SEQ"];
            newLot["START_TIME"] = lotSumDt.Rows[0]["START_TIME"];
            newLot["END_TIME"] = lotSumDt.Rows[0]["END_TIME"];
            newLot["LOSS_DIE"] = lotSumDt.Rows[0]["LOSS_DIE"];
            newLot["TESTED_DIE"] = lotSumDt.Rows[0]["TESTED_DIE"];
            newLot["YIELD"] = lotSumDt.Rows[0]["YIELD"];
            newLot["BIN1"] = lotSumDt.Rows[0]["BIN1"];
            newLot["GEC"] = lotSumDt.Rows[0]["GEC"];
            newLot["WAFERS"] = lotSumDt.Rows[0]["WAFERS"];

            for (int i = 0; i < 100; i++)
            {
                string name = String.Format("BIN{0}", i);
                newLot[name] = lotSumDt.Rows[0][name];
            }

            lotSum.Rows.Add(newLot);
            _mira.InsertBulk(lotSum);
            lotSumDt.Dispose();
        }

        private string[] ToArray(DataTable dt, int column)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < arr.Length; i++)
                arr[i] = dt.Rows[i][column].ToString();

            return arr;
        }

        class RawDataInfo
        {
            public RawDataInfo(decimal wafSeq, decimal mWafSeq, DataRow tableRow)
            {
                WafSeq = wafSeq;
                MiracomWafSeq = mWafSeq;
                TableRow = tableRow;
            }

            public decimal WafSeq;
            public decimal MiracomWafSeq;
            public DataRow TableRow;
        }

        class Watch
        {
            DateTime _dt;
            double _milli;
            public void Start() { _dt = DateTime.Now; }
            public void Stop() { _milli = DateTime.Now.Subtract(_dt).TotalSeconds; }
            public double StopAndStart() { double val = DateTime.Now.Subtract(_dt).TotalSeconds; Start(); return val; }
            public double TotalSeconds { get { return _milli; } }
        }

        private void btnPROGRAM_PARM_DEF_Click(object sender, EventArgs e)
        {
            DataTable dt = _test.GetProgramMaxPpdSeq(Factory);

            foreach (DataRow row in dt.Rows)
            {
                string ppdSeq = row["PPD_SEQ"].ToString();

                DataTable paraDt = _test.GetProgramParameterByPpdSeq(Factory, ppdSeq);

                // 프로그램 별 파라미터 처리하기
                paraDt.TableName = "TQP_PARASPEC";
                _mira.InsertBulk(paraDt);
                paraDt.Dispose();
            }
        }
    }
}
