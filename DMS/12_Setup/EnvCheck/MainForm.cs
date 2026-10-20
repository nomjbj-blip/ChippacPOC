using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCK.EnvCheck
{
    /// <summary>
    /// 점검 결과 화면. 열리면 바로 점검하고, 결과를 목록으로 보여 준 뒤 결과 파일을 쓴다.
    /// Setup 이 설치 끝에 /setup 인자로 띄우고, 사용자는 시작 메뉴 "설치 점검"으로 언제든 다시 띄운다.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly EnvChecker _checker;
        private readonly bool _fromSetup;
        private string _reportPath;

        public MainForm(string appDir, bool fromSetup)
        {
            InitializeComponent();
            _checker = new EnvChecker(appDir);
            _fromSetup = fromSetup;
            if (_fromSetup) Text = "DACrux SCK POC 설치 점검 - 설치가 끝났습니다. 결과를 확인한 뒤 닫으세요";
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            RunChecks();
        }

        private void btnRecheck_Click(object sender, EventArgs e)
        {
            RunChecks();
        }

        /// <summary>
        /// 결과 파일을 메모장으로 연다. 원격 지원 때 이 파일을 보내 달라고 안내한다.
        /// </summary>
        private void btnOpenReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_reportPath) || !File.Exists(_reportPath)) return;
            Process.Start("notepad.exe", "\"" + _reportPath + "\"");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 점검 실행 흐름: OpenGL(UI 스레드) -> 나머지(작업 스레드, 네트워크 대기 중에도 화면이 멈추지 않게) -> 목록 표시 -> 결과 파일.
        /// </summary>
        private async void RunChecks()
        {
            btnRecheck.Enabled = false;
            btnOpenReport.Enabled = false;
            lstResult.Items.Clear();
            lblSummary.ForeColor = SystemColors.ControlText;
            lblSummary.Text = "점검 중입니다. 서버가 응답하지 않으면 몇 초 걸릴 수 있습니다...";
            lblInfo.Text = string.Format("설치 폴더 : {0}\r\n프로그램 버전 : {1}\r\n서버 주소 : {2}",
                _checker.AppDir, _checker.ReadVersionLabel(), _checker.ReadServerIp());

            try
            {
                CheckResult gl = GlProbe.Run();
                List<CheckResult> results = await Task.Run(() => _checker.RunAll(gl));
                ShowResults(results);
                _reportPath = _checker.WriteReport(results);
                lblSummary.Text += "\r\n결과 파일 : " + _reportPath;
                btnOpenReport.Enabled = true;
            }
            catch (Exception ex)
            {
                lblSummary.ForeColor = Color.Firebrick;
                lblSummary.Text = "점검 중 오류: " + ex.GetBaseException().Message;
            }
            finally
            {
                btnRecheck.Enabled = true;
            }
        }

        private void ShowResults(List<CheckResult> results)
        {
            bool hasFail = false;
            bool hasWarning = false;
            lstResult.BeginUpdate();
            foreach (CheckResult r in results)
            {
                var row = new ListViewItem(CheckResult.LevelText(r.Level));
                row.SubItems.Add(r.Item);
                row.SubItems.Add(r.Detail);
                row.ForeColor = LevelColor(r.Level);
                row.ToolTipText = r.Detail;
                lstResult.Items.Add(row);
                hasFail |= r.Level == CheckLevel.Fail;
                hasWarning |= r.Level == CheckLevel.Warning;
            }
            lstResult.EndUpdate();
            lstResult.ShowItemToolTips = true;

            string summary = EnvChecker.Summary(results);
            if (hasFail)
            {
                lblSummary.ForeColor = Color.Firebrick;
                lblSummary.Text = summary + " - 실패 항목을 해결해야 사용할 수 있습니다";
            }
            else if (hasWarning)
            {
                lblSummary.ForeColor = Color.DarkOrange;
                lblSummary.Text = summary + " - 경고 항목을 확인하세요";
            }
            else
            {
                lblSummary.ForeColor = Color.ForestGreen;
                lblSummary.Text = summary + " - 사용할 준비가 되었습니다";
            }
        }

        private static Color LevelColor(CheckLevel level)
        {
            switch (level)
            {
                case CheckLevel.Ok: return Color.ForestGreen;
                case CheckLevel.Warning: return Color.DarkOrange;
                case CheckLevel.Fail: return Color.Firebrick;
                default: return Color.SteelBlue;
            }
        }
    }
}
