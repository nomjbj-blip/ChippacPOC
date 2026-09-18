#define SYNC

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
using TestDataMigrationRemotingService;
using System.Collections;

namespace TestDataMigration
{
    public partial class FrmMain2 : Form
    {
        public static Logger _log = LogManager.GetLogger("logfile");
        public static string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static string DATE_FORMAT = "yyyy-MM-dd";
        public static readonly int THREAD_COUNT = 5;

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

        public FrmMain2()
        {
            InitializeComponent();

            Text = String.Format("Thread Count = {0}", THREAD_COUNT);
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

#if SYNC
            Text = "!!!!!!!!!!!!!!!!!!!!!!!! SYNC MODE !!!!!!!!!!!!!!!!!!!!!!!!!!!!";
#endif

            btnRun.Enabled = btnStop.Enabled = pnlFunc.Enabled = false;
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
            _mira = new MiracomTPS();
            MiracomTPS.Factory = Factory;

            string conn1 = _test.Sysdate();
            string conn2 = _mira.Sysdate();
            //GetRemotingObject().Sysdate();

            MessageBox.Show(String.Format("접속테스트: \r\n - LEGACY:{0}\r\n - DACRUX:{1}", conn1, conn2));

            if (!String.IsNullOrEmpty(conn1) && !String.IsNullOrEmpty(conn2))
            {
                rdoFab1.Enabled = rdoFAB2.Enabled = false;
                btnRun.Enabled = true;
                pnlFunc.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!(rdoFab1.Checked || rdoFAB2.Checked))
            {
                MessageBox.Show("Factory를 선택하세요.");
                return;
            }

            if (MessageBox.Show("정말삭제?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                != System.Windows.Forms.DialogResult.Yes)
                return;

            Text = String.Format("[{0}] {1}~{2}", Factory, dtStart.Value.ToString(DATE_FORMAT), dtEnd.Value.ToString(DATE_FORMAT));

            Log.Info("프로그램 실행 [{0}]", Factory);
            DateTime date = dtStart.Value;

            // 날짜 별 처리
            while (date < dtEnd.Value)
            {
                DateTime date2 = date.AddDays(1) < dtEnd.Value ? date.AddDays(1) : dtEnd.Value;
                
                DataTable lotDt = _mira.GetLot(Factory, date, date2);

                string message = String.Format("Date = {0}, Lot Count = {1}", date.ToString(DATE_FORMAT), lotDt.Rows.Count);
                Log.Info(message);
                AppendInfo(message);

                foreach (DataRow lotRow in lotDt.Rows)
                {
                    // LOT 단위
                    string lotID = lotRow["LOT_ID"].ToString();
                    string program = lotRow["PROGRAM"].ToString();
                    string lotSeq = lotRow["LOT_SEQ"].ToString();

                    message = String.Format("Date = {0}, Lot Count = {1} ({2}/{1}) {3}", date.ToString(DATE_FORMAT), lotDt.Rows.Count, lotDt.Rows.IndexOf(lotRow) + 1, lotID);
                    UpdateLastItem(message);

                    if (program == "AVI" || program == "SCOPE")
                        continue;

                    // TQ 테이블에서 삭제
                    string[] tableNameArr = _mira.GetTableNameFormProgram(program);

                    if (tableNameArr != null)
                    {
                        foreach (string tableName in tableNameArr)
                            _mira.DeleteTdData(tableName, lotSeq);
                    }

                    // TQP_WAFER 에서 삭제
                    _mira.DeleteWaferByLotSeq(lotSeq);
                    // TQP_WAFER_SUM 에서 삭제
                    _mira.DeleteWaferSumByLotSeq(lotSeq);
                    // TQP_LOT 에서 삭제
                    _mira.DeleteLotByLotSeq(lotSeq);
                    // TQP_LOT_SUM 에서 삭제
                    _mira.DeleteLotSumByLotSeq(lotSeq);
                }

                // END
                date = date.AddDays(1);
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
            btnTest.Enabled = false;
            pnlFunc.Enabled = false;
            dtStart.Enabled = dtEnd.Enabled = false;

#if SYNC
            Process(null);
#else
            Thread thread = new Thread(new ParameterizedThreadStart(Process));
            thread.IsBackground = true;
            thread.Start();
#endif
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("중지하시겠습니까? 처리중인 날짜의 데이터는 모두 처리된 후 중지됩니다.",
                "중지", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                return;

            _exit = true;
            btnStop.Enabled = false;
        }

        TestData _test;
        MiracomTPS _mira;

        void WriteLog(string format, double val)
        {
            if (val > 0.5)
                Log.Info(format, val);
        }

        public IMiracomTPS GetRemotingObject()
        {
            int port = TestDataMigrationRemotingService.MiracomTPS.GetPort(Factory);
            IMiracomTPS obj = Activator.GetObject(typeof(IMiracomTPS),
                  String.Format("tcp://localhost:{0}/MiracomTPS", port)) as IMiracomTPS;

            if (obj == null)
                throw new Exception("Remoting object가 null 입니다.");

            return obj;
        }

        private void Process(object state)
        {
            Log.Info("프로그램 실행 [{0}]", Factory);
            DateTime date = dtStart.Value;

            // 날짜 별 처리
            while (date < dtEnd.Value && btnStop.Enabled)
            {
                DateTime date2 = date.AddDays(1) < dtEnd.Value ? date.AddDays(1) : dtEnd.Value;

                // LOT LIST
                DataTable lotDt = _test.GetLot(Factory, date, date2);

                string message = String.Format("Date = {0}, Lot Count = {1}", date.ToString(DATE_FORMAT), lotDt.Rows.Count);
                Log.Info(message);
                AppendInfo(message); 


                foreach (DataRow lotRow in lotDt.Rows)
                {
                    // LOT 단위
                    string lotID = lotRow["LOT_ID"].ToString();
                    string program = lotRow["PROGRAM"].ToString();
                    decimal tLotSeq = decimal.Parse(lotRow["OLD_LOT_SEQ"].ToString());
                    decimal mLotSeq = 0;

                    try
                    {
                        message = String.Format("Date = {0}, Lot Count = {1} ({2}/{1}) {3}", date.ToString(DATE_FORMAT), lotDt.Rows.Count, lotDt.Rows.IndexOf(lotRow) + 1, lotID);
                        UpdateLastItem(message);

                        // TQP_LOT에 있는 경우 PASS
                        if (_mira.ExistsLotData(tLotSeq))
                            continue;

#if !SYNC
                        mLotSeq = _mira.NewLotSeq();
                        lotRow["LOT_SEQ"] = mLotSeq;
#endif

                        // WAFER LIST
                        DataTable wafDt = _test.GetWafer(lotID, tLotSeq);
                        RemoveDuplicateWaferRow(wafDt);

                        DataTable saveWafDt = wafDt.Clone(); // 테이블 구조 복제
                        Log.Info("Lot ID={0}, Wafer Cnt={1}", lotID, wafDt.Rows.Count);

                        List<RawDataInfo> rawDataInfoList = new List<RawDataInfo>();

                        for (int i = 0; i < wafDt.Rows.Count; i++)
                        {
                            DataRow wafRow = wafDt.Rows[i];

                            // WAFER 단위
                            decimal tWafSeq = Decimal.Parse(wafRow["OLD_WAFER_SEQ"].ToString());
                            wafRow["PROGRAM"] = program;
                            wafRow["PROGRAM_REV"] = "0";
                            wafRow["LOT_SEQ"] = mLotSeq;

                            DataTable tableDt = _test.GetProgramTable(program);
                            decimal mWafSeq = _mira.NewWaferSeq();

                            //// Lot 당 최초 한번만 실행
                            //if (i == 0)
                            //{
                            //    // @TD_테이블 필드 확인
                            //    CheckTDTableField(tWafSeq);
                            //}

                            string[] tableArr = new string[tableDt.Rows.Count];
                            for (int j = 0; j < tableArr.Length; j++)
                                tableArr[j] = tableDt.Rows[j][0].ToString();

                            rawDataInfoList.Add(new RawDataInfo(tWafSeq, mWafSeq, tableArr));
                            
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
                            //ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessRawData_async), info);

                            Thread thread = new Thread(new ParameterizedThreadStart(ProcessRawData_async));
                            thread.IsBackground = true;
                            thread.Start(info);
#endif
                        }

                        // RAW 데이터 처리가 완료될때까지 대기
                        _countdown.Wait();
                        //Thread.Sleep(100);

                        _countdown.Dispose();
                        _semaphore.Dispose();

                        string deviceAlias = _mira.GetDeviceAlias(lotID);

                        if (String.IsNullOrEmpty(deviceAlias))
                            deviceAlias = "NONE";

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
                        Summary(mLotSeq);
                    }
                    catch (Exception ex)
                    {
                        message = String.Format("Date={0}, Lot ID={1}", date.ToString(DATE_FORMAT), lotID);
                        Log.Error(ex, message + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                        AppendInfo("###### ERROR #####" + message + ex.Message);
                    }
                }

                lotDt.Dispose();

                if (_exit) return;

                // END
                date = date.AddDays(1);
            }

            SetEnd();
        }

        /// <summary>
        /// 데이터 오류로 인한 중복 문제 해결
        /// </summary>
        private void RemoveDuplicateWaferRow(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            List<string> list = new List<string>();
            List<DataRow> removeList = new List<DataRow>();

            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dt.Rows[i];

                string waferID = row["WAFER_ID"].ToString();

                if (!list.Contains(waferID))
                    list.Add(waferID);
                else
                    removeList.Add(row);
            }

            if (removeList.Count > 0)
            {
                foreach (DataRow row in removeList)
                    dt.Rows.Remove(row);
            }
        }

        public void SetEnd()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(SetEnd));
            }
            else
            {
                btnRun.Enabled = true;
                btnStop.Enabled = false;
                btnTest.Enabled = true;
                dtStart.Enabled = dtEnd.Enabled = true;
                pnlFunc.Enabled = true;
                Log.Info("프로그램 종료");
            }
        }

        private void ProcessRawData_async(object parameter)
        {
#if !SYNC
            _semaphore.WaitOne();
#endif

            RawDataInfo info = parameter as RawDataInfo;

            try
            {
                string mTableName = ToMiracomTable(info.TTableName);

                DataTable schemaTable;
                List<object[]> dataList;
                
                // @TD_ 데이터 INSERT
                _test.GetTTableData(info.TTableName, new decimal[] { info.WafSeq }, out schemaTable, out dataList);

                if (schemaTable == null || dataList == null || dataList.Count == 0 || dataList[0].Length == 0)
                    return;
                
                // XY --> X, Y
                SplitXY(schemaTable, dataList);

                schemaTable.TableName = mTableName;

                int rowLength = dataList[0].Length;
                int waferSeqIndex = GetSchemaRowIndex(schemaTable, "WAFER_SEQ");

                for (int r = 0; r < rowLength; r++)
                {
                    dataList[waferSeqIndex][r] = info.MiracomWafSeq;
                }

                // 이전 wafer seq 데이터가 존재하는 경우 삭제
                _mira.DeleteOldTdData(mTableName, info.MiracomWafSeq);

                _mira.ExecuteTable(schemaTable, dataList);
                //GetRemotingObject().ExecuteTable(schemaTable, dataList);
                schemaTable.Dispose();
                dataList.Clear();
            }
            catch (Exception ex)
            {
                string message = String.Format("ProcessRawData_async error wafSeq={0},NewWafSeq={1}", info.WafSeq, info.MiracomWafSeq);
                Log.Error(ex, message + ex.Message + Environment.NewLine + ex.StackTrace);
                AppendInfo("###### ERROR #####" + message + ex.Message);

#if SYNC
                throw;
#endif
            }
            finally
            {
#if !SYNC
                _semaphore.Release();
                _countdown.Signal();
#endif
            }
        }

        private void AppendInfo(string info)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppendInfo), info);
                return;
            }

            listBox1.SelectedIndex = listBox1.Items.Add(info);
            Application.DoEvents();
        }

        private void UpdateLastItem(string info)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(UpdateLastItem), info);
                return;
            }

            if (listBox1.Items.Count == 0)
                return;

            int index = listBox1.Items.Count - 1;
            listBox1.Items[index] = info;
            listBox1.SelectedIndex = index;
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
                int x, y;

                if (MiracomTPS.TryXYtoXandY(row["XY"], out x, out y))
                {
                    row["X"] = x;
                    row["Y"] = y;
                }
            }

            dt.Columns.Remove("XY");
        }

        private void SplitXY(DataTable schemaTable, List<object[]> dataList)
        {   
            int xyIndex = GetSchemaRowIndex(schemaTable, "XY");

            if (xyIndex < 0)
                return;

            object[] itemArray = schemaTable.Rows[xyIndex].ItemArray;
            itemArray[schemaTable.Columns.IndexOf("ColumnName")] = "X";
            schemaTable.Rows.Add(itemArray);

            itemArray = schemaTable.Rows[xyIndex].ItemArray;
            itemArray[schemaTable.Columns.IndexOf("ColumnName")] = "Y";
            schemaTable.Rows.Add(itemArray);

            int dataCount = dataList[0].Length;
            int xIndex = dataList.Count;

            dataList.Add(new object[dataCount]);
            dataList.Add(new object[dataCount]);

            for (int r = 0; r < dataCount; r++)
            {
                int x, y;

                if (MiracomTPS.TryXYtoXandY(dataList[xyIndex][r], out x, out y))
                {
                    dataList[xIndex][r] = x;
                    dataList[xIndex + 1][r] = y;
                }
                else
                {
                    dataList[xIndex][r] = DBNull.Value;
                    dataList[xIndex + 1][r] = DBNull.Value;
                }
            }

            schemaTable.Rows.RemoveAt(xyIndex);
            dataList.RemoveAt(xyIndex);
        }

        private int GetSchemaRowIndex(DataTable dt, object value)
        {
            if (dt == null || dt.Rows.Count == 0)
                return -1;

            DataRow[] rows = dt.Select(String.Format("ColumnName = '{0}'", value));

            if (rows == null || rows.Length == 0)
                return -1;

            return dt.Rows.IndexOf(rows[0]);
        }

        private void Summary_async(object lotSeq)
        {
            try
            {
                _semaphore.WaitOne();

                Summary((decimal)lotSeq);
            }
            catch (Exception ex)
            {
                string message = String.Format("Summary_async error LotSeq={0}", lotSeq);
                Log.Error(ex, message + ex.Message + Environment.NewLine + ex.StackTrace);
                AppendInfo("###### ERROR #####" + message + ex.Message);
            }
            finally
            {
                _semaphore.Release();
                _countdown.Signal();
            }
        }

        private void Summary(decimal lotSeq)
        {
            DataTable waferDt = _mira.GetWaferSeq(lotSeq);

            DataTable lotSum = EmptyLotSumTable();
            DataTable wafSum = EmptyWaferSumTable();

            foreach (DataRow waferRow in waferDt.Rows)
            {
                decimal waferSeq = decimal.Parse(waferRow["WAFER_SEQ"].ToString());
                string ttable = _mira.GetBinTTable(waferSeq);

                Dictionary<string, decimal> dic = null;

                if (!String.IsNullOrEmpty(ttable))
                    dic = _mira.GetBinCount(ttable, waferSeq);

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

                    string[] goodBinNames = _mira.GetGoodBinNames(lotSeq);

                    foreach (var item in dic)
                    {
                        if (Array.IndexOf<string>(goodBinNames, item.Key) >= 0)
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

        private string[] ToArray(DataRow[] rows, int column)
        {
            if (rows.Length == 0)
                return null;

            string[] arr = new string[rows.Length];

            for (int i = 0; i < arr.Length; i++)
                arr[i] = rows[i][column].ToString();

            return arr;
        }

        class RawDataInfo
        {
            public RawDataInfo(decimal wafSeq, decimal mWafSeq, string[] tTableNameArr)
            {
                WafSeq = wafSeq;
                MiracomWafSeq = mWafSeq;
                TTableNameArr = tTableNameArr;
            }

            public decimal WafSeq;
            public decimal MiracomWafSeq;
            public string[] TTableNameArr;
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
            AppendInfo(String.Empty);
            DataTable programDt = _test.GetProgramList3(Factory);

            foreach (DataRow row in programDt.Rows)
            {
                string program = row[0].ToString();

                program = "KGY013CR";

                RegParaspecAsync(program);
                //ThreadPool.QueueUserWorkItem(RegParaspecAsync, program);
                //Thread thread = new Thread(new ParameterizedThreadStart(RegParaspecAsync));
                //thread.IsBackground = true;
                //thread.Start(program);
            }
        }

        private void RegParaspecAsync(object state)
        {
#if !SYNC
            //_semaphore.WaitOne();
#endif
            try
            {
                string program = state as string;
                DataTable paraDt = _test.GetProgramParameterByProgram(Factory, program);

                UpdateLastItem(program);

                DataRow[] rows = paraDt.Select("PARAM_NAME = 'XY'");

                if (rows != null && rows.Length == 1)
                {
                    DataRow xy = rows[0];

                    xy["PARAM_INDEX"] = -1;
                    xy["PARAM_NAME"] = "X";
                    object[] x = xy.ItemArray;

                    xy["PARAM_INDEX"] = 0;
                    xy["PARAM_NAME"] = "Y";
                    object[] y = xy.ItemArray;

                    paraDt.Rows.Remove(xy);

                    // 인덱스 재조정
                    for (int i = 0; i < paraDt.Rows.Count; i++)
                        paraDt.Rows[i]["PARAM_INDEX"] = i + 1;

                    paraDt.Rows.Add(x);
                    paraDt.Rows.Add(y);
                }

                Dictionary<string, object[]> dic = new Dictionary<string, object[]>();

                if (paraDt.Rows.Count == 0)
                    return;

                foreach (DataRow row in paraDt.Rows)
                    dic.Add(row["PARAM_NAME"].ToString(), row.ItemArray);

                string[] paraArr = _mira.GetParaList(program);

                if (paraArr != null)
                {
                    foreach (string para in paraArr)
                    {
                        if (dic.ContainsKey(para))
                            dic.Remove(para);
                        else
                            txtInsertWafSeq.AppendText(String.Format("{0}\t{1}\n", program, para));
                    }
                }
                else
                {
                    paraArr = null;
                }

                if (dic.Count == 0)
                    return;

                paraDt.TableName = "TQP_PARASPEC";

                paraDt.Rows.Clear();

                foreach (var item in dic)
                {
                    paraDt.Rows.Add(item.Value);
                }

                _mira.InsertBulk(paraDt);
                paraDt.Dispose();
                UpdateLastItem(program + " : " + dic.Count.ToString());
                AppendInfo(program);
            }
            finally
            {
               // _semaphore.Release();
            }
        }

        private void btnDATA_TABLES_Click(object sender, EventArgs e)
        {
            DataTable paraDt = _test.GetData_DATA_TABLES(Factory);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (DataRow row in paraDt.Rows)
            {
                dic.Add(ToMiracomTable(row["TABLE_NAME"].ToString()), row["PROGRAM"].ToString());
            }

            List<string> list = _mira.GetTableNames();

            foreach (string tableName in list)
            {
                if (dic.ContainsKey(tableName))
                    dic.Remove(tableName);
            }

            paraDt.Rows.Clear();

            foreach (var item in dic)
            {
                paraDt.Rows.Add(Factory, item.Value, item.Key);
            }

            // 프로그램 별 파라미터 처리하기
            paraDt.TableName = "TQP_DATA_TABLES";
            _mira.InsertBulk(paraDt);
            paraDt.Dispose();
        }

        private void btnCreateTdTable_Click(object sender, EventArgs e)
        {
            DataTable dt = _test.GetData_DATA_TABLES(Factory);

            if (dt == null || dt.Rows.Count == 0)
                return;

            foreach (DataRow row in dt.Rows)
            {
                string tTableName = row["TABLE_NAME"].ToString();
                string mTableName = ToMiracomTable(tTableName);

                // @TD_PROGRAM : 테이블 신규 추가
                if (!_mira.ExistsTable(mTableName))
                {
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

                    _mira.CreateTableAndView(mTableName, paraList);
                }
            }
        }

        private void btnRunSummary_Click(object sender, EventArgs e)
        {
#if SYNC
            ProcessSummary(null);
#else
            Thread thread = new Thread(new ParameterizedThreadStart(ProcessSummary));
            thread.IsBackground = true;
            thread.Start();

            //ThreadPool.QueueUserWorkItem(ProcessSummary);
#endif
        }

        private void ProcessSummary(object state)
        {
            btnStop.Enabled = true;
            btnRunSummary.Enabled = false;

            Log.Info("프로그램 실행 [{0}]", Factory);
            DateTime date = dtStart.Value;

            while (date < dtEnd.Value && btnStop.Enabled)
            {
                try
                {
                    // LOT LIST
                    DataTable lotDt = _mira.GetLot(Factory, date, date.AddDays(1));
                    string message = String.Format("Date = {0}, Lot Count = {1}", date.ToString(DATE_FORMAT), lotDt.Rows.Count);
                    Log.Info(message);
                    AppendInfo(message);

                    _semaphore = new Semaphore(THREAD_COUNT, THREAD_COUNT);
                    _countdown = new CountdownEvent(lotDt.Rows.Count);

                    foreach (DataRow lotRow in lotDt.Rows)
                    {
                        // LOT 단위
                        string lotID = lotRow["LOT_ID"].ToString();
                        string program = lotRow["PROGRAM"].ToString();
                        decimal tLotSeq = decimal.Parse(lotRow["OLD_LOT_SEQ"].ToString());
                        decimal mLotSeq = decimal.Parse(lotRow["LOT_SEQ"].ToString());
                        
#if SYNC
                            Summary_async(mLotSeq);
#else
                        //ThreadPool.QueueUserWorkItem(Summary_async, mLotSeq);

                        Thread thread = new Thread(new ParameterizedThreadStart(Summary_async));
                        thread.IsBackground = true;
                        thread.Start(mLotSeq);
#endif
                    }

                    // RAW 데이터 처리가 완료될때까지 대기
                    _countdown.Wait();

                    _countdown.Dispose();
                    _semaphore.Dispose();

                    lotDt.Dispose();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                    AppendInfo("###### ERROR #####" + ex.Message);
                }

                if (_exit) return;

                // END
                date = date.AddDays(1);
            }

            SetEnd();
        }

        private void btnRunTDTableFieldCheck_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtWafSeq_TDTableFieldCheck.Text))
                return;

            decimal tWafSeq;

            if (!decimal.TryParse(txtWafSeq_TDTableFieldCheck.Text, out tWafSeq))
                return;

            CheckTDTableField(tWafSeq);
        }

        /// <summary>
        /// TD 테이블에 필드가 맞는지 확인
        /// </summary>
        private void CheckTDTableField(decimal tWafSeq)
        {
            DataTable dt = _test.GetTTableName(tWafSeq.ToString());

            foreach (DataRow row in dt.Rows)
            {
                string tTableName = row["TABLE_NAME"].ToString();
                string mTableName = ToMiracomTable(tTableName);

                // LEGACY 컬럼
                DataTable tTblDt = _test.GetParameterFromTable(tTableName);

                List<string> colList = new List<string>();

                foreach (DataRow colRow in tTblDt.Rows)
                    colList.Add(colRow[0].ToString());

                DataTable mTblDt = _mira.GetParameterFromTable(mTableName);

                foreach (DataRow colRow in mTblDt.Rows)
                {
                    string col = colRow[0].ToString();

                    if (colList.Contains(col))
                        colList.Remove(col);
                }

                if (colList.Contains("XY"))
                    colList.Remove("XY");

                if (colList.Count > 0)
                    _mira.AppendColumnAtTDTable(mTableName, colList);
            }
        }

        private void btnRunTDTableFieldCheckALL_Click(object sender, EventArgs e)
        {
            DataTable dt = _mira.GetAllTableName();

            foreach (DataRow row in dt.Rows)
            {
                string mTableName = row["TABLE_NAME"].ToString();
                string tTableName = "T" + mTableName.Substring(3);

                // LEGACY 컬럼
                DataTable tTblDt = _test.GetParameterFromTable(tTableName);

                List<string> colList = new List<string>();

                foreach (DataRow colRow in tTblDt.Rows)
                    colList.Add(colRow[0].ToString());

                DataTable mTblDt = _mira.GetParameterFromTable(mTableName);

                foreach (DataRow colRow in mTblDt.Rows)
                {
                    string col = colRow[0].ToString();

                    if (colList.Contains(col))
                        colList.Remove(col);
                }

                if (colList.Contains("XY"))
                    colList.Remove("XY");

                if (colList.Count > 0)
                    _mira.AppendColumnAtTDTable(mTableName, colList);
            }
        }

        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            string date = dtpDeleteDate.Value.ToString(dtpDeleteDate.CustomFormat);

            if (MessageBox.Show(date + " 삭제?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                != System.Windows.Forms.DialogResult.Yes)
                return;

            // A.LOT_SEQ, C.TABLE_NAME, B.WAFER_SEQ 
            DataTable dt = _mira.GetData01(date);

            DataTable tableDt = dt.DefaultView.ToTable(true, "TABLE_NAME");

            foreach (DataRow tableRow in tableDt.Rows)
            {
                string tableName = tableRow[0].ToString();

                DataRow[] rows = dt.Select(String.Format("TABLE_NAME = '{0}'", tableName));
                string[] lotSeqArr = ToArray(rows, 0);
                
                _mira.DeleteTableData(tableName, lotSeqArr);
            }

            _mira.DeleteWaferAndLotData(date);

            MessageBox.Show("완료");
        }

        // XY --> X, Y 값 업데이트
        private void btnUpdateXY_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            btnStop.Enabled = true;
            btnTest.Enabled = false;
            pnlFunc.Enabled = false;
            dtStart.Enabled = dtEnd.Enabled = false;

#if SYNC
            UpdateXY(null);
#else
            Thread thread = new Thread(new ParameterizedThreadStart(UpdateXY));
            thread.IsBackground = true;
            thread.Start();
            
            //ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateXY), null);
#endif
        }

        private void UpdateXY(object state)

        {
            Log.Info("프로그램 실행 [{0}]", Factory);
            DateTime date = dtStart.Value;

            while (date < dtEnd.Value)
            {
                // LOT LIST
                DataTable lotDt = null;

                if (String.IsNullOrWhiteSpace(txtMLotSeq.Text))
                    lotDt = _test.GetLot(Factory, date, date.AddDays(1));
                else
                    lotDt = _mira.GetLot(txtMLotSeq.Text.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries));

                string message = String.Format("Date = {0}, Lot Count = {1}", date.ToString(DATE_FORMAT), lotDt.Rows.Count);
                Log.Info(message);
                AppendInfo(message);

                _semaphore = new Semaphore(THREAD_COUNT, THREAD_COUNT);
                _countdown = new CountdownEvent(lotDt.Rows.Count);

                foreach (DataRow lotRow in lotDt.Rows)
                {
#if SYNC
                    UpdateLotXY_async(new object[] { date, lotRow });
#else
                    Thread thread = new Thread(new ParameterizedThreadStart(UpdateLotXY_async));
                    thread.IsBackground = true;
                    thread.Start(new object[] { date, lotRow });

                    //ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateLotXY_async), new object[] { date, lotRow });
#endif
                }

                // RAW 데이터 처리가 완료될때까지 대기
                _countdown.Wait();

                _countdown.Dispose();
                _semaphore.Dispose();

                lotDt.Dispose();

                if (_exit)
                {
                    Log.Info("프로그램 중지 [{0}]", Factory);
                    return;
                }

                if (!String.IsNullOrWhiteSpace(txtMLotSeq.Text))
                    break;

                // END
                date = date.AddDays(1);
            }

            SetEnd();
        }

        private void UpdateLotXY_async(object state)
        {
            _semaphore.WaitOne();

            object[] arr = state as object[];
            DateTime date = (DateTime)arr[0];
            DataRow lotRow = arr[1] as DataRow;

            // LOT 단위
            string lotID = lotRow["LOT_ID"].ToString();
            string program = lotRow["PROGRAM"].ToString();
            decimal tLotSeq = decimal.Parse(lotRow["OLD_LOT_SEQ"].ToString());
            decimal mLotSeq = 0;

            try
            {
                mLotSeq = _mira.GetLotSeq(tLotSeq);

                // @TD_테이블 필드 확인
                DataTable tableDt = _test.GetProgramTable(program);

                // RAW 데이터 처리
                foreach (DataRow tableRow in tableDt.Rows)
                {
                    string tableName = tableRow[0].ToString();

                    if (!_test.ExsitsXYField(tableName))
                        continue;

                    // WAFER_SEQ = OLD_WAFER_SEQ
                    Dictionary<string, decimal> dic = _mira.GetWaferSeq02(mLotSeq);
                    List<string> waferSeqList = _test.GetWaferSeqList(tLotSeq);
                    
                    foreach (string waferSeq in waferSeqList)
                    {
                        // NULL WAFER_SEQ, OLD_WAFER_SEQ, DIE_NUM, X, Y
                        DataTable xyDt = _test.GetXYData(tableName, waferSeq);

                        if (xyDt == null || xyDt.Rows.Count == 0)
                            continue;

                        // 매핑
                        foreach (var item in dic)
                        {
                            foreach (DataRow row in xyDt.Select(String.Format("OLD_WAFER_SEQ = {0}", item.Key)))
                                row["WAFER_SEQ"] = item.Value;
                        }

                        for (int i = xyDt.Rows.Count - 1; i >= 0; i--)
                        {
                            if ((decimal)xyDt.Rows[i]["WAFER_SEQ"] == 0)
                                xyDt.Rows.RemoveAt(i);
                        }

                        xyDt.TableName = ToMiracomTable(tableName);

                        _mira.UpdateTable(xyDt,
                            new string[] { "X", "Y" },
                            new string[] { "WAFER_SEQ", "DIE_NUM" });

                        xyDt.Dispose();
                    }
                }

                tableDt.Dispose();
            }
            catch (Exception ex)
            {
                string message = String.Format("Summary_async error LotID={0}, LotSeq={1}", lotID, mLotSeq);
                Log.Error(ex, message + ex.Message + Environment.NewLine + ex.StackTrace);
                AppendInfo("###### ERROR #####" + message + ex.Message);
                AppendInfo(String.Empty); // for status
            }
            finally
            {
                _semaphore.Release();
                _countdown.Signal();

                string message = String.Format("Date = {0}, Lot Count = {1} ({2}/{1}) {3}", date.ToString(DATE_FORMAT), lotRow.Table.Rows.Count, _countdown.InitialCount - _countdown.CurrentCount, lotID);
                UpdateLastItem(message);
            }
        }

        private string ToMiracomTable(string tTableName)
        {
            return "TD_" + tTableName.Substring(1);
        }

        private void btnDeleteUnlinked_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정말삭제?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                != System.Windows.Forms.DialogResult.Yes)
                return;

            btnRun.Enabled = false;
            btnStop.Enabled = true;
            btnTest.Enabled = false;
            pnlFunc.Enabled = false;
            dtStart.Enabled = dtEnd.Enabled = false;

            Thread thread = new Thread(new ParameterizedThreadStart(DeleteUnlinked_async));
            thread.IsBackground = true;
            thread.Start();
        }

        private void DeleteUnlinked_async(object state)
        {
            AppendInfo(String.Format("연결 없는 데이터 삭제 실행 [{0}]", Factory));
            AppendInfo(String.Empty);
            List<string> tableNames = _mira.GetTableNames();

            for (int i = 0; i < tableNames.Count; i++)
            {
                UpdateLastItem(String.Format("({0} / {1})  {2}", i + 1, tableNames.Count, tableNames[i]));

                DataTable waferSeqDt = _mira.GetUnlinkedWaferSeq(tableNames[i]);

                if (waferSeqDt == null || waferSeqDt.Rows.Count == 0)
                    continue;

                waferSeqDt.TableName = tableNames[i];

                _mira.DeleteTable(waferSeqDt,
                        new string[] { "WAFER_SEQ" });

                if (_exit)
                {
                    AppendInfo("프로그램 종료");
                    break;
                }
            }
        }

        private void ProcessValid(object state)
        {
            Log.Info("프로그램 실행 [{0}]", Factory);
            DateTime date = dtStart.Value;

            List<decimal> erorWafList = new List<decimal>();
            List<decimal> notFoundLotList = new List<decimal>();
            List<DateTime> countErrDateList = new List<DateTime>();

            while (date < dtEnd.Value)
            {
                // LOT LIST
                Dictionary<decimal, int> tDic = _test.GetLotAndWafCnt(Factory, date, date.AddDays(1));
                Dictionary<decimal, int> mDic = _mira.GetLotAndWafCnt(date, date.AddDays(1));

                string message = String.Format("Date = {0}", date.ToString(DATE_FORMAT));
                Log.Info(message);
                AppendInfo(message);

                if (tDic.Count != mDic.Count)
                    countErrDateList.Add(date);

                foreach (var item in tDic)
                {
                    if (!mDic.ContainsKey(item.Key))
                    {
                        notFoundLotList.Add(item.Key);
                        continue;
                    }

                    if (mDic[item.Key] != item.Value)
                    {
                        erorWafList.Add(item.Key);
                    }
                }

                if (_exit) break;

                // END
                date = date.AddDays(1);
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("countErrDateList");
            foreach (DateTime dt in countErrDateList)
                sb.AppendLine(dt.ToString("yyyy-MM-dd"));

            Log.Info(sb.ToString());
            sb.Clear();

            sb.AppendLine("notFoundLotList");
            foreach (decimal val in notFoundLotList)
                sb.AppendLine(val.ToString());

            Log.Info(sb.ToString());
            sb.Clear();

            sb.AppendLine("erorWafList");
            foreach (decimal val in erorWafList)
                sb.AppendLine(val.ToString());

            Log.Info(sb.ToString());
            sb.Clear();

            SetEnd();
        }

        private void btnProcessValid_Click(object sender, EventArgs e)
        {
            ProcessValid(null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> list = _mira.GetTableNames();

            decimal val = 0;
            AppendInfo(String.Empty);

            foreach (string tableName in list)
            {
                val += _mira.GetDataCount(tableName);
                UpdateLastItem(String.Format("{0:N0}", val));
            }
        }

        private void btnSum_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSumLot.Text))
            {
                decimal lotSeq = _mira.GetLotSeq(txtSumLot.Text);
                Summary(lotSeq);
                txtSumLot.Clear();
            }
        }

        private void btnInsertWaferData_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtInsertWafSeq.Text))
            {
                MessageBox.Show("값 입력");
                return;
            }

            List<string> list = new List<string>();
            list.AddRange(txtInsertWafSeq.Lines);

            while (true)
            {
                if (list.Count == 0)
                    break;

                if (String.IsNullOrEmpty(list[0]))
                {
                    list.RemoveAt(0);
                    continue;
                }

                string[] arr = list[0].Split(',');
                decimal tWafSeq = Decimal.Parse(arr[0]);
                decimal mWafSeq = Decimal.Parse(arr[1]);

                AppendInfo(String.Format("{0}, {1}", tWafSeq, mWafSeq));

                string[] tables = _test.GetTableListFromWaferSeq(tWafSeq);

                AppendInfo(String.Format("table {0}개 찾음 {1}", tables.Length, String.Join(",", tables)));
                bool hasError = false;

                foreach (string table in tables)
                {
                    try
                    {
                        RawDataInfo info = new RawDataInfo(tWafSeq, mWafSeq, table);
                        ProcessRawData_async(info);
                        AppendInfo(String.Format("{0}: 처리 완료 {1}, {2}", table, tWafSeq, mWafSeq));
                    }
                    catch
                    {
                        hasError = true;
                        break;
                    }
                }

                if (hasError)
                {
                    txtInsertWafSeq.Clear();
                    foreach (string item in list)
                    {
                        txtInsertWafSeq.AppendText(item + Environment.NewLine);
                    }

                    break;
                }
                else
                {
                    list.RemoveAt(0);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string[] programArr = _mira.GetDistinctProgram();

            foreach (string program in programArr)
            {
                DataTable dt = _mira.GetDupColumn(program);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string dupField = row[1].ToString();

                        // 어떤 컬럼을 뺄것인지 결정
                        List<string> tableNameList = new List<string>();
                        tableNameList.AddRange(_mira.GetTableNameFormProgram(program));

                        string[] arr = _mira.GetTableWhereField(program, dupField);

                        if (arr != null)
                        {
                            foreach (string val in arr)
                                tableNameList.Remove(val);
                        }

                        foreach (string tableName in tableNameList)
                            txtInsertWafSeq.AppendText(String.Format("{0}\t{1}\r\n", tableName, dupField));

                        Application.DoEvents();
                    }
                }
            }
        }

        private void btnBin_Click(object sender, EventArgs e)
        {
            MiracomTPS m = new MiracomTPS(Factory);
            TestData t = new TestData();

            DataTable programDt = t.GetProgramList3(Factory);
            DataTable dt = EmptyBinTable();

            foreach (DataRow programRow in programDt.Rows)
            {
                string program = (string)programRow["PROGRAM"];

                DataTable binDt = t.GetBin(program);

                foreach (DataRow binRow in binDt.Rows)
                {
                    decimal bin = (decimal)binRow["BIN"];
                    string name = binRow["BIN_LABEL"].ToString();
                    string color = binRow["BIN_COLOR"].ToString().ToUpper();
                    string gec = (decimal)binRow["GECBINS"] == 1 ? "Y" : "N";

                    if (!String.IsNullOrEmpty(color) && color[0] != '#')
                        color = "#" + color;

                    dt.Rows.Add(program, bin, name, color, gec);
                }

                m.InsertBulk(dt);
                dt.Rows.Clear();
            }
        }
    }
}
