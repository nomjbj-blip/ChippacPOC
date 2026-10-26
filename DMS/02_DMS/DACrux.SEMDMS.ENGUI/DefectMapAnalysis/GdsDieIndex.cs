using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// Die Index = 고객 통합 Die# (DieCoordinateMapper.DieNumberOf). 분석이 E-Test 비교에 쓰는 것과 같은 Map(Factory + Product 의 TQP_USEMAP 사용 Die)으로
    /// 위 행부터(Y 큰 것부터), 같은 행은 왼쪽부터(X 작은 것부터) 1, 2, 3 ... 을 매긴다. 분석 결과 DIE 의 XINDEX / YINDEX 가 이 Map 의 시스템 좌표다.
    /// Simple 화면 Wafer Map 머리 / 종합 표와 상세보기 Die 목록이 같이 쓴다 (2026-10-22).
    /// Map 사용 Die 가 없거나 조회에 실패하면 null 이고, 화면은 Index 를 비운다.
    /// </summary>
    public static class GdsDieIndex
    {
        private static readonly object sync = new object();
        private static string lastKey;
        private static DieCoordinateMapper lastMapper;

        /// <summary>Map 사용 Die 조회 서비스. null 이면 원격 호출 RO 를 만든다. Smoke 검사는 같은 프로세스 BSL 을 붙인다.</summary>
        public static RO.GdsEtResult Service { get; set; }

        /// <summary>
        /// Map 을 새로 읽는다(Wafer 를 받을 때, 작업 스레드에서 불러도 된다). 읽은 결과는 Cached 가 다시 쓴다.
        /// 조회 실패도 null 로 기억한다(화면 스레드에서 같은 원격 호출을 또 기다리지 않게). 다음 Wafer 를 받을 때 다시 읽는다.
        /// </summary>
        public static DieCoordinateMapper Load(string factory, string product)
        {
            string key = Key(factory, product);
            if (key == null) return null;
            DieCoordinateMapper mapper = null;
            try
            {
                DataTable dies = (Service ?? new RO.GdsEtResult()).GetEtMapDieList(factory.Trim(), product.Trim());
                bool used = dies != null && dies.Rows.Cast<DataRow>().Any(r => r["USECODE"] != DBNull.Value
                    && Convert.ToInt32(r["USECODE"], CultureInfo.InvariantCulture) == DieCoordinateMapper.UsedDieCode);
                if (used) mapper = DieCoordinateMapper.FromDieTable(dies, new DiePoint(0, 0));
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Die Index Map 조회 실패 ({0} / {1}): {2}", factory, product, ex.Message);
            }
            lock (sync)
            {
                lastKey = key;
                lastMapper = mapper;
            }
            return mapper;
        }

        /// <summary>마지막으로 읽은 Map 이 같은 Factory + Product 면 그것을 쓰고, 아니면 새로 읽는다.</summary>
        public static DieCoordinateMapper Cached(string factory, string product)
        {
            string key = Key(factory, product);
            if (key == null) return null;
            lock (sync)
            {
                if (key == lastKey) return lastMapper;
            }
            return Load(factory, product);
        }

        /// <summary>Die Index. Map 이 없거나 사용 Die 가 아니면 0.</summary>
        public static int Of(DieCoordinateMapper mapper, int x, int y)
        {
            return mapper == null ? 0 : mapper.DieNumberOf(new DiePoint(x, y));
        }

        private static string Key(string factory, string product)
        {
            string f = (factory ?? string.Empty).Trim(), p = (product ?? string.Empty).Trim();
            return f.Length == 0 || p.Length == 0 ? null : (f + "\n" + p).ToUpperInvariant();
        }
    }
}
