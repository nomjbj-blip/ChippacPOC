using System;
using System.Globalization;

namespace DACrux.SEMDMS.Interface
{
    // =====================================================================================================
    //  Wafer Key 규칙 (2026-10-16)
    // -----------------------------------------------------------------------------------------------------
    //  목적
    //   KLARF Step / E-Test 파일이 같은 실물 Wafer 인지 판단하는 문자열(Wafer Key)을 만든다.
    //   통합 연관 분석 Ready 조회가 이 Key 로 같은 Wafer 의 KLARF Step 과 E-Test 파일을 모은다.
    //   설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 16.2절 (결정 B18)
    //
    //  규칙은 고객사와 협의해 정한다. 협의 전에는 아래 잠정 규칙(LOT_CORE_SLOT)을 쓰고 Confirmed = false 로 둔다.
    //   KLARF  : LOT_ID 를 '_' 로 나눠 3조각 이상이면 첫 조각(접두어)과 마지막 조각(Step 이름)을 뺀 가운데 = Lot 핵심.
    //            3조각 미만이면 LOT_ID 전체. Key = 대문자(Lot 핵심) + "-" + Slot 2자리.
    //            예) PFIC_RH-HS2615_C4Bump, Slot 2 -> RH-HS2615-02
    //   E-Test : Key = 대문자(LOT_ID) + "-" + Slot 2자리 (= TQP_GDS_SRC_FILE.WAFER_ID). 예) RH-HS2615, Slot 2 -> RH-HS2615-02
    //
    //  DB 후보는 SearchOf 로 넓게 거르고(Lot 글자 LIKE + Slot), 같은지는 FromKlarf / FromETest 결과를 비교한다.
    //  그래서 규칙이 바뀌어도 쿼리는 대부분 그대로다. 고객 규칙이 다른 컬럼(예: 원본 Wafer ID)으로 찾게 되면
    //  이 파일과 후보 쿼리(TQD_INSP_INFO.SELECT_KLARF_STEP_BY_WAFER / TQP_GDS_SRC_FILE.SELECT_ET_FILE_BY_WAFER) 조건만 바꾼다.
    //
    //  걷어내기 / 재활용: 이 파일 하나에 규칙이 모두 있다(System 만 사용). 서버(BSL)와 화면(ENGUI)이 같이 쓴다.
    // =====================================================================================================

    /// <summary>DB 후보 조건. LotLike = UPPER(LOT_ID) LIKE 값, Slot = 앞 0 을 뺀 Slot 번호 문자열.</summary>
    public sealed class WaferKeySearch
    {
        public string LotLike;
        public string Slot;
    }

    /// <summary>Wafer Key 규칙. 머리 주석 참고.</summary>
    public static class WaferKeyRule
    {
        /// <summary>규칙 이름. 고객 규칙으로 바꿀 때 함께 바꾼다.</summary>
        public const string RuleName = "LOT_CORE_SLOT";

        /// <summary>고객 협의 전 = false. 화면 상태줄 / [준비] 탭에 "규칙 잠정" 으로 보인다.</summary>
        public static readonly bool Confirmed = false;

        /// <summary>
        /// KLARF 1행(TQD_INSP_INFO + TQD_STEP.COMMENT 의 RAW_WAFER_ID)으로 Wafer Key 를 만든다. 만들 수 없으면 null.
        /// 잠정 규칙은 waferId / rawWaferId 를 쓰지 않는다(고객 규칙용 자리).
        /// </summary>
        public static string FromKlarf(string lotId, string waferId, string slotId, string rawWaferId)
        {
            string lot = (lotId ?? "").Trim();
            int slot;
            if (lot.Length == 0 || !TrySlot(slotId, out slot)) return null;
            string[] parts = lot.Split('_');
            string core = parts.Length >= 3 ? String.Join("_", parts, 1, parts.Length - 2) : lot;
            return Key(core, slot);
        }

        /// <summary>E-Test 파일 1행(TQP_GDS_SRC_FILE)으로 Wafer Key 를 만든다. 만들 수 없으면 null.</summary>
        public static string FromETest(string lotId, string slotNo, string lotWaferNo)
        {
            string lot = (lotId ?? "").Trim();
            int slot;
            if (lot.Length == 0 || !TrySlot(slotNo, out slot)) return null;
            return Key(lot, slot);
        }

        /// <summary>
        /// DB 후보 조건(넓게). Key 가 "Lot-NN" 모양이 아니면 null.
        /// LIKE 의 '_' 는 아무 글자 1개와도 맞아 후보가 조금 넓어질 뿐이다(같은지는 FromKlarf / FromETest 로 다시 본다).
        /// </summary>
        public static WaferKeySearch SearchOf(string waferKey)
        {
            string key = (waferKey ?? "").Trim();
            int dash = key.LastIndexOf('-');
            int slot;
            if (dash <= 0 || !TrySlot(key.Substring(dash + 1), out slot)) return null;
            return new WaferKeySearch { LotLike = "%" + key.Substring(0, dash).Replace("%", "").ToUpperInvariant() + "%", Slot = slot.ToString(CultureInfo.InvariantCulture) };
        }

        /// <summary>규칙 설명 (화면 / 로그 표시용).</summary>
        public static string Describe()
        {
            return "Lot 핵심(KLARF LOT_ID 의 '_' 가운데 조각) + Slot 2자리";
        }

        private static string Key(string lot, int slot)
        {
            return lot.ToUpperInvariant() + "-" + slot.ToString("00", CultureInfo.InvariantCulture);
        }

        private static bool TrySlot(string text, out int slot)
        {
            return int.TryParse((text ?? "").Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out slot) && slot > 0;
        }
    }
}
