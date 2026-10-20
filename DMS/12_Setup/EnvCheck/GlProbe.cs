using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SCK.EnvCheck
{
    /// <summary>
    /// GDS 화면을 쓸 수 있는 그래픽 환경인지 점검한다.
    /// GDS 화면(GdsMapControl)과 같은 방법으로 GLControl(기본 설정)을 만들고, 같은 #version 330 core 셰이더를
    /// 컴파일 / 링크해 본다. 원격 데스크톱 / VDI / 가상 PC 의 기본 디스플레이 드라이버는 OpenGL 1.1 만 제공해 여기서 실패한다.
    /// UI 스레드(STA)에서 불러야 한다.
    /// </summary>
    internal static class GlProbe
    {
        private const string Item = "그래픽(OpenGL, GDS 화면)";

        // GdsMapControl 과 같은 셰이더. GDS 화면이 실제로 컴파일하는 문법 수준을 그대로 확인한다.
        private const string VertexShaderCode = @"
            #version 330 core
            layout(location = 0) in vec2 aPos;
            layout(location = 1) in vec4 aColor;
            uniform mat4 uMatrix;
            out vec4 vColor;
            void main() {
                gl_Position = uMatrix * vec4(aPos, 0.0, 1.0);
                vColor = aColor;
            }
        ";

        private const string FragmentShaderCode = @"
            #version 330 core
            in vec4 vColor;
            out vec4 FragColor;
            void main() {
                FragColor = vColor;
            }
        ";

        /// <summary>
        /// 점검 결과를 돌려준다. OpenTK DLL 을 찾지 못하면 RunCore 를 부르는 순간 예외가 나므로 여기서 함께 받는다.
        /// </summary>
        public static CheckResult Run()
        {
            try
            {
                return RunCore();
            }
            catch (Exception ex)
            {
                return new CheckResult(Item, CheckLevel.Fail,
                    "OpenGL 장치를 만들 수 없음 (" + ex.GetBaseException().Message + ") - 그래픽 드라이버 설치 여부 / 원격 데스크톱 여부 확인");
            }
        }

        /// <summary>
        /// 화면 밖에 작은 창을 띄워 OpenGL 문맥을 만들고 버전 / 장치 이름 / 셰이더 컴파일 결과를 확인한다.
        /// 3.3 미만이면 셰이더 / VAO 함수를 부르지 않는다(함수 주소가 없어 프로세스가 종료될 수 있기 때문).
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static CheckResult RunCore()
        {
            using (var form = new Form())
            using (var gl = new GLControl())
            {
                Rectangle screen = SystemInformation.VirtualScreen;
                form.ShowInTaskbar = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(screen.Left - 400, screen.Top - 400);
                form.Size = new Size(64, 64);
                gl.Dock = DockStyle.Fill;
                form.Controls.Add(gl);
                form.Show();

                try
                {
                    gl.MakeCurrent();
                    string version = GL.GetString(StringName.Version) ?? string.Empty;
                    string renderer = GL.GetString(StringName.Renderer) ?? string.Empty;
                    string text = "OpenGL " + version + " / " + renderer;

                    Match m = Regex.Match(version, @"^(\d+)\.(\d+)");
                    int major = m.Success ? int.Parse(m.Groups[1].Value) : 0;
                    int minor = m.Success ? int.Parse(m.Groups[2].Value) : 0;
                    if (major < 3 || (major == 3 && minor < 3))
                    {
                        return new CheckResult(Item, CheckLevel.Fail,
                            text + " - 3.3 미만이라 GDS 화면을 쓸 수 없음. 그래픽 드라이버를 설치하거나 원격 데스크톱이 아닌 PC 에서 실행하세요");
                    }

                    string error = CompileShaders();
                    if (error != null)
                    {
                        return new CheckResult(Item, CheckLevel.Fail, text + " - GDS 화면 셰이더 컴파일 실패: " + error);
                    }
                    return new CheckResult(Item, CheckLevel.Ok, text + " / 셰이더 3.3 컴파일 정상");
                }
                finally
                {
                    form.Hide();
                }
            }
        }

        /// <summary>
        /// GDS 화면과 같은 셰이더를 컴파일 / 링크하고 VAO 를 하나 만들어 본다. 성공하면 null, 실패하면 오류 내용.
        /// </summary>
        private static string CompileShaders()
        {
            int vs = GL.CreateShader(ShaderType.VertexShader);
            int fs = GL.CreateShader(ShaderType.FragmentShader);
            int program = GL.CreateProgram();
            try
            {
                int status;
                GL.ShaderSource(vs, VertexShaderCode);
                GL.CompileShader(vs);
                GL.GetShader(vs, ShaderParameter.CompileStatus, out status);
                if (status == 0) return "Vertex " + GL.GetShaderInfoLog(vs).Trim();

                GL.ShaderSource(fs, FragmentShaderCode);
                GL.CompileShader(fs);
                GL.GetShader(fs, ShaderParameter.CompileStatus, out status);
                if (status == 0) return "Fragment " + GL.GetShaderInfoLog(fs).Trim();

                GL.AttachShader(program, vs);
                GL.AttachShader(program, fs);
                GL.LinkProgram(program);
                GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
                if (status == 0) return "Link " + GL.GetProgramInfoLog(program).Trim();

                int vao = GL.GenVertexArray();
                GL.DeleteVertexArray(vao);
                return null;
            }
            finally
            {
                GL.DeleteProgram(program);
                GL.DeleteShader(vs);
                GL.DeleteShader(fs);
            }
        }
    }
}
