using System;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace DACrux.SEMDMS.DSL
{
    /// <summary>작업 모듈. 저장 함수가 어느 기능의 데이터를 바꾸는지 나타낸다. 지금은 모든 모듈에 같은 기간을 적용한다.</summary>
    public enum WorkModule { Dms, Gds, Klarf, ETest, WaferMap }

    /// <summary>작업 종류. Setup = 기준정보 저장 / 삭제, Upload = 데이터 적재. 조회는 확인하지 않는다.</summary>
    public enum WorkKind { Setup, Upload }

    /// <summary>
    /// 데이터 생성 / 수정 서비스 함수의 첫 단계에서 작업 가능 기간을 확인한다. (2026-10-23)
    /// - 기간 안이면 DB 일시(한국 시간)를 돌려주고, 기간 밖이면 예외를 던져 다음 단계(작업 키 발급 / 저장)로 가지 않게 한다.
    /// - 기간 값은 이 클래스 상수에만 둔다. 설정 파일 / Query XML / DB 테이블은 설치 현장에서 바꿀 수 있어 쓰지 않는다.
    /// - 시각은 DB 서버 시각을 쓴다. 클라이언트 / 서비스 PC 시간은 바꿀 수 있어 쓰지 않는다.
    ///   같은 이유로 일시 조회 SQL 도 Query XML 이 아닌 이 코드에 둔다.
    /// 설계: 문서/설계/06_환경_운영/2026-10-14_POC_License_설계.md
    /// </summary>
    public class WorkRule : Miracom.Middleware.QueryComponent
    {
        /// <summary>데이터 생성 / 수정이 가능한 마지막 날(한국 시간). 다음 날 0시부터 막는다. 기간을 바꿀 때는 이 값만 고쳐 서버에 다시 배포한다.</summary>
        static readonly DateTime LastWorkDay = new DateTime(2027, 1, 31);

        /// <summary>DB 일시가 이 날보다 앞서면 DB 서버 시각이 잘못된 것으로 보고 막는다.</summary>
        static readonly DateTime FirstValidDay = new DateTime(2026, 10, 1);

        /// <summary>
        /// DB 일시를 한국 시간으로 읽는다. DB OS 시간대가 UTC 여도 같은 값이 나오도록 UTC 로 바꾼 뒤 9시간을 더한다.
        /// SYSDATE 대신 SYSTIMESTAMP 를 쓰는 이유: FIXED_DATE 설정은 SYSDATE 만 고정한다.
        /// </summary>
        const string DbNowSql = "SELECT TO_CHAR(SYS_EXTRACT_UTC(SYSTIMESTAMP) + 9/24, 'YYYYMMDDHH24MISS') AS DB_TIME FROM DUAL";

        const string TimeErrorMessage = "서버 일시를 확인할 수 없어 저장할 수 없습니다.";

        readonly string connectID;

        public WorkRule()
        {
            connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
            if (string.IsNullOrEmpty(connectID))
                throw new Exception("The connect ID nothing. Please, check app.config.");
        }

        /// <summary>
        /// 저장 흐름의 1단계. 기간 안이면 DB 일시(한국 시간)를 돌려준다.
        /// 기간 밖이거나 DB 일시를 읽지 못하면 InvalidOperationException 을 던진다. 화면은 기존 오류 표시로 문장을 보여 준다.
        /// module / kind 는 기능별로 기간을 다르게 줄 때 쓰려고 받는다. 지금은 모두 같은 기간이다.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public DateTime Confirm(WorkModule module, WorkKind kind)
        {
            DateTime now = ReadDbNow();
            if (!IsOpen(now))
                throw new InvalidOperationException(ClosedMessage());
            return now;
        }

        /// <summary>
        /// 클라이언트 메뉴 안내용 한 행.
        /// 열: DB_TIME(yyyy-MM-dd HH:mm:ss) / LAST_DAY(yyyy-MM-dd) / WORK_OPEN(Y / N) / DAYS_LEFT(마지막 날까지 남은 일수, 오늘 포함. 지나면 0 이하)
        /// DB 일시를 읽지 못하면 예외. 클라이언트는 이때 메뉴를 바꾸지 않는다(저장은 Confirm 이 막는다).
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public DataTable GetState()
        {
            DateTime now = ReadDbNow();

            DataTable dt = new DataTable("WORK_STATE");
            dt.Columns.Add("DB_TIME", typeof(string));
            dt.Columns.Add("LAST_DAY", typeof(string));
            dt.Columns.Add("WORK_OPEN", typeof(string));
            dt.Columns.Add("DAYS_LEFT", typeof(int));
            dt.Rows.Add(
                now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                LastWorkDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                IsOpen(now) ? "Y" : "N",
                DaysLeft(now));
            return dt;
        }

        /// <summary>기간 판정. FirstValidDay 0시 이상, LastWorkDay 다음 날 0시 미만이면 작업 가능.</summary>
        internal static bool IsOpen(DateTime now)
        {
            return now >= FirstValidDay && now < LastWorkDay.AddDays(1);
        }

        /// <summary>마지막 날까지 남은 일수(오늘 포함). 마지막 날 당일 = 1, 다음 날 = 0.</summary>
        internal static int DaysLeft(DateTime now)
        {
            return (LastWorkDay.Date - now.Date).Days + 1;
        }

        static string ClosedMessage()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "POC 사용 기간({0:yyyy-MM-dd} 까지)이 지나 저장 / 수정 / 삭제할 수 없습니다. 조회 화면은 계속 사용할 수 있습니다.", LastWorkDay);
        }

        /// <summary>
        /// DB 일시(한국 시간)를 읽는다. 읽지 못하거나 FirstValidDay 보다 앞선 값이면 저장을 막는다.
        /// 서비스 PC 시간으로 대신하지 않는다(DB 가 안 되면 저장도 어차피 실패한다).
        /// NoInlining: Middleware 가 호출한 메서드 정보를 StackFrame 으로 읽으므로, 인라인되면 Probe(PowerShell) 호출에서 실패한다.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        DateTime ReadDbNow()
        {
            DateTime now;
            try
            {
                DataTable dt = this.GetDataTable(connectID, DbNowSql);
                string text = dt.Rows[0]["DB_TIME"].ToString();
                if (!DateTime.TryParseExact(text, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out now))
                    throw new FormatException("DB_TIME=" + text);
            }
            catch (Exception ex)
            {
                // Oracle 예외를 InnerException 으로 넘기면 Oracle DLL 이 없는 클라이언트에서 Remoting 역직렬화가 실패할 수 있어 문장만 붙인다
                throw new InvalidOperationException(TimeErrorMessage + " (" + ex.Message + ")");
            }

            if (now < FirstValidDay)
                throw new InvalidOperationException(TimeErrorMessage + " (DB 일시 " + now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + ")");
            return now;
        }
    }
}
