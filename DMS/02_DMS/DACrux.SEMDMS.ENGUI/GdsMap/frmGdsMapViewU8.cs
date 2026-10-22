using System;
using System.Reflection;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// DACrux 의 BASIC_FORM 메뉴에서 GDS Map View U8 화면(같은 형태 Chain 일괄 생성)을 여는 호스트다 (2026-10-20).
    /// 설계: 문서/설계/02_GDS/2026-10-20_GDS_Map_View_U8_화면_설계.md 8절. GDS Map 조회 호스트(frmGdsMapView)와 같은 방식이다.
    /// 기존 ENGUI 는 .NET 4.0 이므로 .NET 4.8 GDS 프로젝트를 직접 참조하지 않고 실행 시 로드한다.
    /// </summary>
    public partial class frmGdsMapViewU8 : DACrux.Framework.Base.DACruxUXBasic01
    {
        private Form gdsForm;

        /// <summary>메뉴 로더가 요구하는 기본 생성자로 디자이너의 호스트 영역을 준비한다.</summary>
        public frmGdsMapViewU8()
        {
            InitializeComponent();
        }

        /// <summary>DACrux MDI 화면이 표시된 뒤 전용 폴더의 GDS Map View U8 화면을 자식 Form 으로 붙인다.</summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (gdsForm != null) return;

            try
            {
                using (DACrux.SEMDMS.Control.BusyCursor.Show(this))
                {
                    Assembly assembly = GdsMapAssembly.Load();
                    Type formType = assembly.GetType("NexplantQMS.GdsMap.GdsMapViewU8Form", true);
                    gdsForm = Activator.CreateInstance(formType, new object[] { true }) as Form;
                    if (gdsForm == null)
                        throw new InvalidOperationException("GDS Map View U8 화면을 Form으로 만들 수 없습니다.");

                    gdsForm.TopLevel = false;
                    gdsForm.FormBorderStyle = FormBorderStyle.None;
                    gdsForm.Dock = DockStyle.Fill;
                    pnlGdsMap.Controls.Add(gdsForm);
                    gdsForm.Show();
                }
            }
            catch (Exception ex)
            {
                Exception cause = ex is TargetInvocationException && ex.InnerException != null
                    ? ex.InnerException : ex;
                MessageBox.Show(this, cause.Message, "GDS Map View U8 화면 열기",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
