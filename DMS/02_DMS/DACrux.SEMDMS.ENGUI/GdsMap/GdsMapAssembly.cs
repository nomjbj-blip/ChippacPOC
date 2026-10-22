using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// GDS 화면 DLL(100_ClientBin\GdsMap\NexplantQMS.GdsMap.dll, .NET 4.8)을 읽는다.
    /// ENGUI 는 .NET 4.0 이라 컴파일 때 참조하지 않고, GDS 호스트 화면(Map 설정 / 조회 / 조회 U8 / Die 상세)이 실행 시 이 DLL 하나를 같이 쓴다.
    /// 이미 읽힌 어셈블리가 있으면 그것을 쓴다. 상세보기 공유 도면 / 미리 읽기 캐시(static)가 하나만 있어야 하기 때문이다.
    /// 전용 폴더를 써서 DACrux DLL 과 GDS 전용 DLL(OpenTK 등)이 섞이지 않게 한다.
    /// 2026-10-22: EXE 대신 DLL 을 읽는다(문서/설계/05_연관분석/2026-10-22_Die상세_DLL로딩_Die목록_설계.md 4절).
    /// </summary>
    internal static class GdsMapAssembly
    {
        private const string AssemblyName = "NexplantQMS.GdsMap";

        /// <summary>GDS 화면 DLL 경로(실행 폴더 아래 GdsMap 폴더).</summary>
        public static string FilePath
        {
            get { return Path.Combine(Application.StartupPath, "GdsMap", AssemblyName + ".dll"); }
        }

        /// <summary>GDS 화면 어셈블리. 이미 읽혔으면 그것, 아니면 GdsMap 폴더 DLL 을 읽는다. 없으면 경로를 담은 예외.</summary>
        public static Assembly Load()
        {
            Assembly loaded = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == AssemblyName);
            if (loaded != null) return loaded;
            string path = FilePath;
            if (!File.Exists(path))
                throw new FileNotFoundException("GDS Map DLL 을 찾을 수 없습니다: " + path, path);
            return Assembly.LoadFrom(path);
        }
    }
}
