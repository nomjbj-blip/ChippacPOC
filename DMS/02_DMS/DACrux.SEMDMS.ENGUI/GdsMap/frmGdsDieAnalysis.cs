using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// Die 상세 분석 화면(화면 B) 호스트. 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 7.3절 (CLAUDE-053)
    /// 기존 frmGdsMapView 와 같은 방식으로 .NET 4.8 GDS EXE 를 실행 시 읽어 NexplantQMS.GdsMap.GdsDieAnalysisForm 을 자식 Form 으로 붙인다.
    /// 연관 분석 화면(frmGdsKlarfAnalysis)이 직접 만들어 연다(2026-10-12: ShowForm 은 ENGUI 를 다른 어셈블리 사본으로 읽어 중복 생성되므로 쓰지 않음),
    /// 매개변수 object[] { 분석 결과 DataSet, Die X, Die Y } 를 LoadParameter / ShowDie 로 넘긴다.
    /// 두 프로젝트가 같이 아는 형식(System.Data / System.Action)만 넘긴다.
    /// </summary>
    public partial class frmGdsDieAnalysis : DACrux.Framework.Base.DACruxUXBasic01
    {
        private Form gdsForm;
        private MethodInfo showDie;
        private object[] pending;

        public frmGdsDieAnalysis()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 화면이 표시된 뒤 GDS 상세 Form 을 붙인다. 붙이기 전에 받은 매개변수가 있으면 바로 넘긴다.
        /// 전용 폴더(100_ClientBin\GdsMap)를 써서 DACrux DLL 과 GDS DLL 의 버전 충돌을 피한다(frmGdsMapView 와 같음).
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (gdsForm != null) return;
            try
            {
                string assemblyPath = Path.Combine(Application.StartupPath, "GdsMap", "NexplantQMS.GdsMap.exe");
                if (!File.Exists(assemblyPath))
                    throw new FileNotFoundException("GDS Map 실행 파일을 찾을 수 없습니다.", assemblyPath);

                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                Type formType = assembly.GetType("NexplantQMS.GdsMap.GdsDieAnalysisForm", true);
                gdsForm = Activator.CreateInstance(formType, new object[] { true }) as Form;
                if (gdsForm == null)
                    throw new InvalidOperationException("Die 상세 분석 화면을 Form 으로 만들 수 없습니다.");
                showDie = formType.GetMethod("ShowDie", new[] { typeof(DataSet), typeof(int), typeof(int) });
                if (showDie == null)
                    throw new MissingMethodException("NexplantQMS.GdsMap.GdsDieAnalysisForm", "ShowDie (GDS EXE 버전 확인 필요)");
                PropertyInfo imageRequest = formType.GetProperty("DefectImageRequested");
                if (imageRequest != null)
                    imageRequest.SetValue(gdsForm, new Action<long, long>(ShowDefectImages), null);

                gdsForm.TopLevel = false;
                gdsForm.FormBorderStyle = FormBorderStyle.None;
                gdsForm.Dock = DockStyle.Fill;
                pnlGdsMap.Controls.Add(gdsForm);
                gdsForm.Show();

                if (pending != null)
                {
                    object[] p = pending;
                    pending = null;
                    ShowDie((DataSet)p[0], (int)p[1], (int)p[2]);
                }
            }
            catch (Exception ex)
            {
                Exception cause = ex is TargetInvocationException && ex.InnerException != null ? ex.InnerException : ex;
                MessageBox.Show(this, cause.Message, "Die 상세 분석 화면 열기", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        /// <summary>ShowForm 으로 열렸을 때 Activated 에서 불린다(DACruxUXBasic01). 매개변수: object[] { DataSet, int X, int Y }.</summary>
        public override void LoadParameter(object parameter)
        {
            var p = parameter as object[];
            if (p == null || p.Length < 3 || !(p[0] is DataSet)) return;
            ShowDie((DataSet)p[0], Convert.ToInt32(p[1]), Convert.ToInt32(p[2]));
        }

        /// <summary>
        /// 연관 분석 화면이 직접 부른다. GDS Form 이 아직 없으면 붙인 뒤 넘긴다.
        /// 탭 제목 = "상세보기 (X, Y)" (2026-10-12 사용자 지시 "Tab 상단에는 상세보기(좌표정보)", 설계 14.4절). 상세 화면은 이 Chip 1개만 분석한다.
        /// </summary>
        public void ShowDie(DataSet analysis, int xIndex, int yIndex)
        {
            Text = String.Format("상세보기 ({0}, {1})", xIndex, yIndex);
            if (gdsForm == null || showDie == null)
            {
                pending = new object[] { analysis, xIndex, yIndex };
                return;
            }
            showDie.Invoke(gdsForm, new object[] { analysis, xIndex, yIndex });
        }

        /// <summary>
        /// GDS 상세 화면의 이미지 보기 요청. 기존 Defect Map Viewer 와 같은 경로(TQD_IMAGES -> DPUCDefectInfo 썸네일 / FTP 다운로드)를 쓴다.
        /// 썸네일을 더블클릭하면 기존 ImagePopUp 이 열린다.
        /// </summary>
        private void ShowDefectImages(long stepSeq, long defectId)
        {
            DataTable paths = new RO.DefectMapAnalysis().GetDefectImagePath01(new[] { stepSeq });
            var popup = new Form
            {
                Text = "Defect #" + defectId + " 이미지",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(720, 460),
                ShowInTaskbar = false
            };
            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };
            popup.Controls.Add(flow);
            foreach (DataRow row in paths.Rows)
            {
                if (Convert.ToInt64(row["DEFECTID"]) != defectId) continue;
                string path = String.Format("{0}/{1}", row["IMAGE_PATH"].ToString().Trim(), row["IMAGE_FILENAME"].ToString().Trim());
                flow.Controls.Add(new DACrux.SEMDMS.Control.DPUCDefectInfo(defectId.ToString(), path, "", "", "", "", "", "", "", ""));
            }
            if (flow.Controls.Count == 0)
            {
                popup.Dispose();
                MessageBox.Show(this, "Defect #" + defectId + " 이미지가 없습니다(TQD_IMAGES).", "Defect 이미지", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            popup.Show(this);
        }
    }
}
