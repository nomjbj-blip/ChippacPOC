using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// DACrux의 BASIC_FORM 메뉴에서 GDS Map 화면을 여는 호스트다.
    /// 기존 ENGUI는 .NET 4.0이므로 .NET 4.8 GDS 프로젝트를 직접 참조하지 않고 실행 시 로드한다.
    /// </summary>
    public partial class frmGdsMapSetup : DACrux.Framework.Base.DACruxUXBasic01
    {
        private Form gdsForm;

        /// <summary>메뉴 로더가 요구하는 기본 생성자로 디자이너의 호스트 영역을 준비한다.</summary>
        public frmGdsMapSetup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// DACrux MDI 화면이 표시된 뒤 전용 폴더의 GDS 화면을 자식 Form으로 붙인다.
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
                Type formType = assembly.GetType("NexplantQMS.GdsMap.GdsMapForm", true);
                gdsForm = Activator.CreateInstance(formType, new object[] { true }) as Form;
                if (gdsForm == null)
                    throw new InvalidOperationException("GDS Map 화면을 Form으로 만들 수 없습니다.");

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
                MessageBox.Show(this, cause.Message, "GDS Map 화면 열기",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
