using System.Collections.Generic;

namespace SCK.EnvCheck
{
    /// <summary>
    /// 점검 결과 등급.
    /// 실패: 프로그램 또는 주요 화면을 쓸 수 없음 / 경고: 쓸 수는 있으나 확인 필요 / 참고: 일부 부가 기능 안내.
    /// </summary>
    public enum CheckLevel
    {
        Ok,
        Warning,
        Fail,
        Info
    }

    /// <summary>
    /// 점검 항목 1개의 결과. 화면 목록 1행과 결과 파일 1행이 된다.
    /// 누락 파일 목록처럼 길게 남길 내용은 Lines 에 넣어 결과 파일 아래쪽 상세에만 쓴다.
    /// </summary>
    public sealed class CheckResult
    {
        public CheckResult(string item, CheckLevel level, string detail)
        {
            Item = item;
            Level = level;
            Detail = detail;
            Lines = new List<string>();
        }

        public string Item { get; private set; }

        public CheckLevel Level { get; private set; }

        public string Detail { get; private set; }

        public List<string> Lines { get; private set; }

        /// <summary>
        /// 화면 / 결과 파일에 표시할 등급 이름.
        /// </summary>
        public static string LevelText(CheckLevel level)
        {
            switch (level)
            {
                case CheckLevel.Ok: return "정상";
                case CheckLevel.Warning: return "경고";
                case CheckLevel.Fail: return "실패";
                default: return "참고";
            }
        }
    }
}
