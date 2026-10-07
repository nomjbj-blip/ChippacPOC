using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// DACrux 호스트 정보. 2026-10-07 사용자 지시: Factory는 화면에서 고르지 않고 DACrux 로그인 Factory(DACrux.Base.GlobalVariable.Factory)를 쓴다.
    /// GDS 화면은 DACrux 메뉴(frmGdsMapSetup / frmGdsMapView)가 같은 프로세스에 올리므로 DACrux가 이미 로드한 DACrux.Base의 값을 그대로 읽는다.
    /// 단독 실행(DACrux.Base 없음)이면 빈 문자열을 돌려주고, 화면은 DB 기능을 막는다.
    /// </summary>
    internal static class DmsHostContext
    {
        /// <summary>DACrux 로그인 Factory. 없으면 빈 문자열.</summary>
        public static string Factory
        {
            get
            {
                try
                {
                    return ReadFactory();
                }
                catch (FileNotFoundException)
                {
                    return string.Empty;
                }
                catch (FileLoadException)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>DACrux.Base 로드 실패를 Factory 속성의 catch에서 잡도록 따로 둔다(JIT 시점에 어셈블리를 찾기 때문).</summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ReadFactory()
        {
            return (DACrux.Base.GlobalVariable.Factory ?? string.Empty).Trim();
        }
    }
}
