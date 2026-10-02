using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// DACrux의 BASIC_FORM 메뉴에서 GDS Map 조회 화면(DB 기반, 읽기 전용)을 여는 호스트다.
    /// 설계: 문서/2026-10-02_GDS_Map_조회화면_설계.md 3.1절. 등록 화면 호스트(frmGdsMapSetup)와 같은 방식이다.
    /// 기존 ENGUI는 .NET 4.0이므로 .NET 4.8 GDS 프로젝트를 직접 참조하지 않고 실행 시 로드한다.
    /// </summary>
    public partial class frmGdsMapView : DACrux.Framework.Base.DACruxUXBasic01
    {
        private Form gdsForm;

        /// <summary>메뉴 로더가 요구하는 기본 생성자로 디자이너의 호스트 영역을 준비한다.</summary>
        public frmGdsMapView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// DACrux MDI 화면이 표시된 뒤 전용 폴더의 GDS 조회 화면을 자식 Form으로 붙인다.
        /// 전용 폴더를 사용해 기존 DACrux DLL과 GDS DLL의 버전 충돌을 피한다.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (gdsForm != null) return;

            try
            {
                string assemblyPath = Path.Combine(Application.StartupPath,
                    "GdsMap", "NexplantQMS.GdsMap.exe");
                if (!File.Exists(assemblyPath))
                    throw new FileNotFoundException("GDS Map 실행 파일을 찾을 수 없습니다.", assemblyPath);

                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                Type formType = assembly.GetType("NexplantQMS.GdsMap.GdsMapViewForm", true);
                gdsForm = Activator.CreateInstance(formType, new object[] { true }) as Form;
                if (gdsForm == null)
                    throw new InvalidOperationException("GDS Map 조회 화면을 Form으로 만들 수 없습니다.");

                gdsForm.TopLevel = false;
                gdsForm.FormBorderStyle = FormBorderStyle.None;
                gdsForm.Dock = DockStyle.Fill;
                pnlGdsMap.Controls.Add(gdsForm);
                gdsForm.Show();
            }
            catch (Exception ex)
            {
                Exception cause = ex is TargetInvocationException && ex.InnerException != null
                    ? ex.InnerException : ex;
                MessageBox.Show(this, cause.Message, "GDS Map 조회 화면 열기",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
