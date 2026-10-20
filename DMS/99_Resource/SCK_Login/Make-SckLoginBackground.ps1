# SCK POC 로그인 화면 배경 이미지 생성 (2026-10-14)
#
# frmLoginDACrux 배경(550 x 350, 컨트롤 위치는 원래 Designer 기준)을 만든다.
# 칩 패키지(기판 / 범프층 / 상부 다이)를 간단한 레이 트레이싱으로 그리고, 상부 다이에 로고를 레이저 마킹처럼 넣는다.
# 색은 DACrux 화면 톤(Office2007 Blue 계열 연하늘 / 슬레이트 남색)에 맞춘다.
# 글자는 넣지 않는다. 왼쪽 위 남색 띠(시스템 이름 / 버전 Label 자리)와 오른쪽 아래 남색 판(로그인 컨트롤 자리)만 그린다.
#   - 실행 폴더의 CustomLogin.rs 로 복사하면 frmLoginDACrux_Load 가 BackgroundImage 로 읽는다(소스 수정 없음).
# 예) powershell -NoProfile -ExecutionPolicy Bypass -File Make-SckLoginBackground.ps1 -Logo .\STATSChipPAC_logo.png -OutDir .
#     (-OkImg / -CancelImg 에 기존 버튼 이미지를 주면 컨트롤과 제안 Label 을 얹은 SCK_LoginPreview.png 도 만든다)
param([string]$Logo, [string]$OutDir, [string]$OkImg, [string]$CancelImg)
$code = @'
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;

public struct V3
{
    public double X, Y, Z;
    public V3(double x, double y, double z) { X = x; Y = y; Z = z; }
    public static V3 operator +(V3 a, V3 b) { return new V3(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }
    public static V3 operator -(V3 a, V3 b) { return new V3(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }
    public static V3 operator *(V3 a, double s) { return new V3(a.X * s, a.Y * s, a.Z * s); }
    public static double Dot(V3 a, V3 b) { return a.X * b.X + a.Y * b.Y + a.Z * b.Z; }
    public static V3 Cross(V3 a, V3 b) { return new V3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X); }
    public double Len() { return Math.Sqrt(X * X + Y * Y + Z * Z); }
    public V3 N() { double l = Len(); return new V3(X / l, Y / l, Z / l); }
}

public class Tex
{
    public int W, H; public float[] R, G, B;
    public Tex(Bitmap bmp)
    {
        W = bmp.Width; H = bmp.Height; R = new float[W * H]; G = new float[W * H]; B = new float[W * H];
        BitmapData d = bmp.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        int[] buf = new int[W * H]; Marshal.Copy(d.Scan0, buf, 0, W * H); bmp.UnlockBits(d);
        for (int i = 0; i < W * H; i++) { int c = buf[i]; R[i] = ((c >> 16) & 255) / 255f; G[i] = ((c >> 8) & 255) / 255f; B[i] = (c & 255) / 255f; }
    }
    public void Sample(double u, double v, bool wrap, out double r, out double g, out double b)
    {
        double x = u * W - 0.5, y = v * H - 0.5;
        int x0 = (int)Math.Floor(x), y0 = (int)Math.Floor(y); double fx = x - x0, fy = y - y0;
        r = g = b = 0;
        for (int j = 0; j < 2; j++)
            for (int i = 0; i < 2; i++)
            {
                int xi = x0 + i, yi = y0 + j;
                if (wrap) { xi = ((xi % W) + W) % W; yi = ((yi % H) + H) % H; }
                else { xi = Math.Max(0, Math.Min(W - 1, xi)); yi = Math.Max(0, Math.Min(H - 1, yi)); }
                double w = (i == 0 ? 1 - fx : fx) * (j == 0 ? 1 - fy : fy); int k = yi * W + xi;
                r += R[k] * w; g += G[k] * w; b += B[k] * w;
            }
    }
}

public class Box
{
    public double X0, X1, Y0, Y1, Z0, Z1; public Tex Top; public int Kind;
    public Box(double x0, double x1, double y0, double y1, double z0, double z1, Tex top, int kind)
    { X0 = x0; X1 = x1; Y0 = y0; Y1 = y1; Z0 = z0; Z1 = z1; Top = top; Kind = kind; }
}

public static class ChipArt
{
    static Color C(string h) { return ColorTranslator.FromHtml(h); }
    static readonly string DOT = ((char)0xB7).ToString();
    static double Cl(double v) { return v < 0 ? 0 : (v > 1 ? 1 : v); }
    static double Smooth(double a, double b, double x) { double t = Cl((x - a) / (b - a)); return t * t * (3 - 2 * t); }
    static double Knee(double v) { return v < 0.82 ? v : 0.82 + (1 - Math.Exp(-(v - 0.82) * 5.5)) * 0.18; }

    // ---------- logo ----------
    public static Bitmap LogoAlpha(string path)
    {
        using (Bitmap src = new Bitmap(path))
        {
            Rectangle crop = new Rectangle(18, 20, 297, 65);
            Bitmap dst = new Bitmap(crop.Width, crop.Height, PixelFormat.Format32bppArgb);
            for (int y = 0; y < crop.Height; y++)
                for (int x = 0; x < crop.Width; x++)
                {
                    Color c = src.GetPixel(crop.X + x, crop.Y + y);
                    int m = Math.Max(255 - c.R, Math.Max(255 - c.G, 255 - c.B));
                    if (m <= 3) { dst.SetPixel(x, y, Color.Transparent); continue; }
                    double a = m / 255.0;
                    Func<int, int> un = v => Math.Max(0, Math.Min(255, (int)Math.Round(255 - (255 - v) / a)));
                    dst.SetPixel(x, y, Color.FromArgb(m, un(c.R), un(c.G), un(c.B)));
                }
            return dst;
        }
    }

    static void DrawImageFit(Graphics g, Image img, RectangleF box, ColorMatrix cm)
    {
        float s = Math.Min(box.Width / img.Width, box.Height / img.Height);
        float w = img.Width * s, h = img.Height * s;
        RectangleF r = new RectangleF(box.X + (box.Width - w) / 2, box.Y + (box.Height - h) / 2, w, h);
        using (ImageAttributes ia = new ImageAttributes())
        {
            if (cm != null) ia.SetColorMatrix(cm);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(img, new PointF[] { r.Location, new PointF(r.Right, r.Top), new PointF(r.Left, r.Bottom) },
                new RectangleF(0, 0, img.Width, img.Height), GraphicsUnit.Pixel, ia);
            g.PixelOffsetMode = PixelOffsetMode.Default;
        }
    }

    // ---------- textures ----------
    static void GoldDot(Graphics g, float x, float y, float r, Brush dark, Brush mid, Brush hi)
    {
        g.FillEllipse(dark, x - r, y - r, 2 * r, 2 * r);
        g.FillEllipse(mid, x - r * 0.78f - r * 0.12f, y - r * 0.78f - r * 0.12f, r * 1.56f, r * 1.56f);
        g.FillEllipse(hi, x - r * 0.55f, y - r * 0.6f, r * 0.55f, r * 0.5f);
    }

    public static Bitmap Substrate(int S)
    {
        Bitmap b = new Bitmap(S, S, PixelFormat.Format32bppArgb);
        Random rnd = new Random(7);
        float sc = S / 100f;
        using (Graphics g = Graphics.FromImage(b))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(C("#34435A"));
            using (SolidBrush n = new SolidBrush(Color.FromArgb(9, 255, 255, 255)))
                for (int i = 0; i < 6000; i++) g.FillRectangle(n, rnd.Next(S), rnd.Next(S), 2, 2);
            using (Pen p = new Pen(C("#4A5B74"), 5)) g.DrawRectangle(p, 2.5f, 2.5f, S - 6, S - 6);

            float f0 = (50 - 27) * sc, f1 = (50 + 27) * sc, cx = S / 2f;
            // fan-out traces + edge pads, 4 sides by rotation
            using (Pen tp = new Pen(Color.FromArgb(235, C("#A8843C")), 2.0f))
            using (SolidBrush pad = new SolidBrush(C("#C9A04A")))
            {
                for (int side = 0; side < 4; side++)
                {
                    GraphicsState st = g.Save();
                    g.TranslateTransform(cx, cx); g.RotateTransform(90 * side); g.TranslateTransform(-cx, -cx);
                    int n = 30;
                    for (int i = 0; i < n; i++)
                    {
                        float t = (i + 0.5f) / n;
                        float sx = f0 + 18 + (f1 - f0 - 36) * t;
                        float ex = cx + (sx - cx) * 1.42f;
                        float y1 = f0 - 2.2f * sc, y2 = y1 - Math.Abs(ex - sx), ye = 6.5f * sc;
                        g.DrawLines(tp, new PointF[] { new PointF(sx, f0), new PointF(sx, y1), new PointF(ex, y2), new PointF(ex, ye) });
                        g.FillRectangle(pad, ex - 4, ye - 14, 8, 15);
                    }
                    g.Restore(st);
                }
            }
            // corner passives
            using (SolidBrush body = new SolidBrush(C("#243044"))) using (SolidBrush term = new SolidBrush(C("#B9B3A3")))
            {
                foreach (PointF cc in new[] { new PointF(9, 9), new PointF(91, 9), new PointF(9, 91), new PointF(91, 91) })
                    for (int k = 0; k < 4; k++)
                    {
                        float x = (cc.X + (k % 2) * (cc.X < 50 ? 3.2f : -3.2f)) * sc, y = (cc.Y + (k / 2) * (cc.Y < 50 ? 2.6f : -2.6f)) * sc;
                        g.FillRectangle(body, x - 10, y - 6, 20, 12); g.FillRectangle(term, x - 10, y - 6, 5, 12); g.FillRectangle(term, x + 5, y - 6, 5, 12);
                    }
            }
            // bump field
            using (SolidBrush dk = new SolidBrush(C("#6E5222"))) using (SolidBrush md = new SolidBrush(C("#C6A04F"))) using (SolidBrush hi = new SolidBrush(C("#FFF1C2")))
            {
                float pitch = 1.5f * sc;
                for (float y = f0 + pitch / 2; y < f1; y += pitch)
                    for (float x = f0 + pitch / 2; x < f1; x += pitch) GoldDot(g, x, y, 4.6f, dk, md, hi);
            }
            using (Pen fp = new Pen(C("#C9A04A"), 2f))
                foreach (PointF cc in new[] { new PointF(4, 4), new PointF(96, 4), new PointF(4, 96), new PointF(96, 96) })
                    g.DrawEllipse(fp, cc.X * sc - 7, cc.Y * sc - 7, 14, 14);
        }
        return b;
    }

    public static Bitmap Interposer(int S)
    {
        Bitmap b = new Bitmap(S, S, PixelFormat.Format32bppArgb);
        Random rnd = new Random(11);
        using (Graphics g = Graphics.FromImage(b))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(C("#26324A"));
            using (Pen p = new Pen(C("#3F5070"), 3)) g.DrawRectangle(p, 14, 14, S - 29, S - 29);
            SolidBrush[] shades = new SolidBrush[6];
            for (int i = 0; i < 6; i++) { double k = i / 5.0; shades[i] = new SolidBrush(Color.FromArgb(255, (int)(140 + 80 * k), (int)(150 + 75 * k), (int)(165 + 70 * k))); }
            using (SolidBrush sp = new SolidBrush(C("#FFF4D6")))
            {
                for (int y = 34; y < S - 30; y += 10)
                    for (int x = 34; x < S - 30; x += 10)
                    {
                        if (((x - 34) / 10) % 22 == 21 || ((y - 34) / 10) % 22 == 21) continue;
                        double r = rnd.NextDouble();
                        if (r < 0.05) g.FillEllipse(sp, x - 2.8f, y - 2.8f, 5.6f, 5.6f);
                        else g.FillEllipse(shades[rnd.Next(6)], x - 2.3f, y - 2.3f, 4.6f, 4.6f);
                    }
            }
            foreach (SolidBrush s in shades) s.Dispose();
        }
        return b;
    }

    public static Bitmap TopDie(int S, Bitmap logo)
    {
        Bitmap b = new Bitmap(S, S, PixelFormat.Format32bppArgb);
        Random rnd = new Random(3);
        using (Graphics g = Graphics.FromImage(b))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            using (LinearGradientBrush lg = new LinearGradientBrush(new Point(0, 0), new Point(S, S), C("#5A6C86"), C("#36455C")))
                g.FillRectangle(lg, 0, 0, S, S);
            using (Pen p = new Pen(Color.FromArgb(10, 255, 255, 255), 1))
                for (int i = 0; i < 500; i++) { float y = (float)(rnd.NextDouble() * S); g.DrawLine(p, 0, y, S, y + (float)(rnd.NextDouble() * 4 - 2)); }
            using (Pen p = new Pen(C("#73859F"), 6)) g.DrawRectangle(p, 3, 3, S - 7, S - 7);
            ColorMatrix cm = new ColorMatrix(new float[][] {
                new float[] { 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 0.62f, 0 }, new float[] { 0.84f, 0.88f, 0.94f, 0, 1 } });
            DrawImageFit(g, logo, new RectangleF(S * 0.14f, S * 0.24f, S * 0.72f, S * 0.18f), cm);
            using (Font f1 = new Font("Consolas", S * 0.044f, FontStyle.Bold, GraphicsUnit.Pixel))
            using (Font f2 = new Font("Consolas", S * 0.034f, FontStyle.Regular, GraphicsUnit.Pixel))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(160, 0xC4, 0xD0, 0xE0)))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center })
            {
            }
            using (SolidBrush pin = new SolidBrush(C("#2A3648"))) g.FillEllipse(pin, S * 0.07f, S * 0.07f, S * 0.05f, S * 0.05f);
            using (Pen pr = new Pen(Color.FromArgb(60, 255, 255, 255), 2)) g.DrawEllipse(pr, S * 0.07f, S * 0.07f, S * 0.05f, S * 0.05f);
        }
        return b;
    }

    public static Bitmap Board(int S)
    {
        Bitmap b = new Bitmap(S, S, PixelFormat.Format32bppArgb);
        Random rnd = new Random(21);
        using (Graphics g = Graphics.FromImage(b))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(C("#CCD9E8"));
            using (SolidBrush fillA = new SolidBrush(C("#C3D1E3")))
                for (int i = 0; i < 26; i++) g.FillRectangle(fillA, rnd.Next(S), rnd.Next(S), 80 + rnd.Next(260), 60 + rnd.Next(200));
            using (Pen comp = new Pen(C("#9FB1C8"), 2f)) using (SolidBrush cf = new SolidBrush(C("#C0CEE0"))) using (SolidBrush pd = new SolidBrush(C("#8C9DB5")))
                for (int i = 0; i < 70; i++)
                {
                    int w = 24 + rnd.Next(110), h = 18 + rnd.Next(90), x = rnd.Next(S), y = rnd.Next(S);
                    g.FillRectangle(cf, x, y, w, h); g.DrawRectangle(comp, x, y, w, h);
                    for (int k = 4; k < w - 4; k += 8) { g.FillRectangle(pd, x + k, y - 5, 4, 6); g.FillRectangle(pd, x + k, y + h - 1, 4, 6); }
                }
            using (Pen tr = new Pen(C("#A3B4CB"), 1.7f)) using (Pen via = new Pen(C("#8EA2BD"), 1.4f))
            {
                for (int bnd = 0; bnd < 110; bnd++)
                {
                    int lines = 3 + rnd.Next(9); float sp = 7f;
                    float x = rnd.Next(S), y = rnd.Next(S);
                    int dir = rnd.Next(4); float dx = dir == 0 ? 1 : (dir == 1 ? -1 : 0), dy = dir == 2 ? 1 : (dir == 3 ? -1 : 0);
                    float l1 = 120 + rnd.Next(520), l2 = 40 + rnd.Next(140), l3 = 80 + rnd.Next(420);
                    int turn = rnd.Next(2) == 0 ? 1 : -1;
                    float tx = dx - dy * turn, ty = dy + dx * turn;
                    for (int k = 0; k < lines; k++)
                    {
                        float ox = -dy * k * sp, oy = dx * k * sp;
                        float kk = k * sp * 0.41f;
                        PointF p0 = new PointF(x + ox, y + oy);
                        PointF p1 = new PointF(p0.X + dx * (l1 + kk * turn), p0.Y + dy * (l1 + kk * turn));
                        PointF p2 = new PointF(p1.X + tx * l2 * 0.7071f, p1.Y + ty * l2 * 0.7071f);
                        PointF p3 = new PointF(p2.X + dx * l3, p2.Y + dy * l3);
                        g.DrawLines(tr, new PointF[] { p0, p1, p2, p3 });
                        g.DrawEllipse(via, p3.X - 3, p3.Y - 3, 6, 6);
                    }
                }
            }
        }
        return b;
    }

    public static Bitmap Blur(Bitmap src, int factor)
    {
        Bitmap small = new Bitmap(src.Width / factor, src.Height / factor, PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage(small)) { g.InterpolationMode = InterpolationMode.HighQualityBicubic; g.DrawImage(src, 0, 0, small.Width, small.Height); }
        Bitmap big = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage(big)) { g.InterpolationMode = InterpolationMode.HighQualityBicubic; g.DrawImage(small, 0, 0, big.Width, big.Height); }
        small.Dispose();
        return big;
    }

    // ---------- ray tracing ----------
    static bool Axis(double o, double d, double lo, double hi, int fLo, ref double tmin, ref double tmax, ref int fmin)
    {
        if (Math.Abs(d) < 1e-12) return o >= lo && o <= hi;
        double t1 = (lo - o) / d, t2 = (hi - o) / d; int f1 = fLo, f2 = fLo + 1;
        if (t1 > t2) { double tt = t1; t1 = t2; t2 = tt; int ff = f1; f1 = f2; f2 = ff; }
        if (t1 > tmin) { tmin = t1; fmin = f1; }
        if (t2 < tmax) tmax = t2;
        return tmin <= tmax;
    }
    static bool HitBox(V3 o, V3 d, Box b, out double t, out int face)
    {
        double tmin = -1e18, tmax = 1e18; int f = -1; t = 0; face = -1;
        if (!Axis(o.X, d.X, b.X0, b.X1, 0, ref tmin, ref tmax, ref f)) return false;
        if (!Axis(o.Y, d.Y, b.Y0, b.Y1, 2, ref tmin, ref tmax, ref f)) return false;
        if (!Axis(o.Z, d.Z, b.Z0, b.Z1, 4, ref tmin, ref tmax, ref f)) return false;
        if (tmin < 1e-6) return false;
        t = tmin; face = f; return true;
    }
    static readonly V3[] Normals = { new V3(-1, 0, 0), new V3(1, 0, 0), new V3(0, -1, 0), new V3(0, 1, 0), new V3(0, 0, -1), new V3(0, 0, 1) };

    static Box[] boxes;
    static Tex board, boardBlur;
    static V3 L = new V3(-0.42, 0.55, 0.72).N();
    static V3[] Ls;

    static double Shadow(V3 p, Box self)
    {
        int occ = 0;
        foreach (V3 l in Ls)
        {
            foreach (Box b in boxes)
            {
                if (b == self || b.Z1 <= p.Z + 1e-6) continue;
                double t; int f;
                if (HitBox(p, l, b, out t, out f)) { occ++; break; }
            }
        }
        return 1.0 - 0.62 * occ / Ls.Length;
    }

    static void Shade(V3 o, V3 d, out double r, out double g, out double bl, out double depth)
    {
        double best = 1e18; int bf = -1; Box bb = null;
        foreach (Box b in boxes) { double t; int f; if (HitBox(o, d, b, out t, out f) && t < best) { best = t; bf = f; bb = b; } }
        bool hitBoard = false;
        if (d.Z < 0) { double tb = -o.Z / d.Z; if (tb < best) { best = tb; hitBoard = true; bb = null; } }
        if (bb == null && !hitBoard) { r = 0.88; g = 0.92; bl = 0.96; depth = 1e6; return; }
        V3 p = o + d * best; depth = best;
        V3 h = (L - d).N();
        if (hitBoard)
        {
            double r1, g1, b1, r2, g2, b2;
            double u = (p.X + 300) / 600.0, v = (300 - p.Y) / 600.0;
            board.Sample(u, v, true, out r1, out g1, out b1);
            boardBlur.Sample(u, v, true, out r2, out g2, out b2);
            double k = Cl((best - 170) / 200);
            r = r1 + (r2 - r1) * k; g = g1 + (g2 - g1) * k; bl = b1 + (b2 - b1) * k;
            double lum = (r + g + bl) / 3;
            double lit = 0.42 + 0.7 * L.Z;
            double spec = Math.Pow(Math.Max(0, h.Z), 30) * 0.10;
            double dx = Math.Max(Math.Abs(p.X) - 50, 0), dy = Math.Max(Math.Abs(p.Y) - 50, 0);
            double ao = 0.45 + 0.55 * Cl(Math.Sqrt(dx * dx + dy * dy) / 16);
            double sh = Shadow(p + new V3(0, 0, 0.01), null);
            double m = lit * ao * sh;
            r = r * m + spec; g = g * m + spec; bl = bl * m + spec * 1.1;
        }
        else
        {
            V3 n = Normals[bf];
            if (bf == 5)
            {
                double u = (p.X - bb.X0) / (bb.X1 - bb.X0), v = (bb.Y1 - p.Y) / (bb.Y1 - bb.Y0);
                bb.Top.Sample(u, v, false, out r, out g, out bl);
                double lum = (r + g + bl) / 3, metal = Cl((r - bl) * 3 + (lum - 0.45) * 1.2);
                double sh = Shadow(p + new V3(0, 0, 0.01), bb);
                double lit = (0.42 + 0.72 * L.Z) * sh * (bb.Kind == 2 ? 0.95 : 1.0);
                double nh = Math.Max(0, h.Z);
                double spec;
                if (bb.Kind == 2) spec = (Math.Pow(nh, 90) * 0.6 + Math.Pow(nh, 7) * 0.045) * sh;
                else spec = (Math.Pow(nh, 45) * (0.12 + metal * 1.4) + Math.Pow(nh, 6) * 0.04) * sh;
                V3 R = d - n * (2 * V3.Dot(d, n));
                double env = bb.Kind == 2 ? 0.02 + 0.17 * Smooth(0.35, 0.95, R.Y) : 0.0;
                double e = Math.Min(Math.Min(p.X - bb.X0, bb.X1 - p.X), Math.Min(p.Y - bb.Y0, bb.Y1 - p.Y));
                double edge = e < 0.45 ? (0.45 - e) / 0.45 * (0.18 + 0.25 * Cl((p.Y - bb.Y0) / (bb.Y1 - bb.Y0))) : 0;
                r = r * lit + spec * 1.0 + env * 0.9 + edge; g = g * lit + spec * 0.97 + env * 0.95 + edge; bl = bl * lit + spec * 0.9 + env + edge;
            }
            else
            {
                double fz = (p.Z - bb.Z0) / (bb.Z1 - bb.Z0);
                double br, bgc, bbc;
                if (bb.Kind == 0)
                {
                    br = 0.30; bgc = 0.36; bbc = 0.45;
                    double s = (fz * 4) % 1.0;
                    if (s < 0.18) { br = 0.62; bgc = 0.52; bbc = 0.32; }
                }
                else if (bb.Kind == 1) { br = 0.20; bgc = 0.25; bbc = 0.33; }
                else { br = 0.27; bgc = 0.33; bbc = 0.42; }
                double dif = Math.Max(0, V3.Dot(n, L));
                double lit = (0.32 + 0.85 * dif) * (0.72 + 0.4 * fz);
                double spec = Math.Pow(Math.Max(0, V3.Dot(n, h)), 25) * 0.25;
                double rim = fz > 0.86 ? 0.10 : 0;
                r = br * lit + spec + rim; g = bgc * lit + spec + rim; bl = bbc * lit + spec * 1.05 + rim;
            }
        }
        double fog = 1 - Math.Exp(-Math.Max(0, best - 165) * 0.0050);
        r += (0.86 - r) * fog * 0.92; g += (0.90 - g) * fog * 0.92; bl += (0.95 - bl) * fog * 0.92;
    }

    static void BoxBlur(float[] src, float[] dst, int W, int H, int rad)
    {
        float[] tmp = new float[W * H];
        for (int y = 0; y < H; y++)
        {
            double acc = 0; int cnt = 0;
            for (int x = -rad; x <= rad; x++) { if (x >= 0 && x < W) { acc += src[y * W + x]; cnt++; } }
            for (int x = 0; x < W; x++)
            {
                tmp[y * W + x] = (float)(acc / cnt);
                int xo = x - rad, xi = x + rad + 1;
                if (xo >= 0) { acc -= src[y * W + xo]; cnt--; }
                if (xi < W) { acc += src[y * W + xi]; cnt++; }
            }
        }
        for (int x = 0; x < W; x++)
        {
            double acc = 0; int cnt = 0;
            for (int y = -rad; y <= rad; y++) { if (y >= 0 && y < H) { acc += tmp[y * W + x]; cnt++; } }
            for (int y = 0; y < H; y++)
            {
                dst[y * W + x] = (float)(acc / cnt);
                int yo = y - rad, yi = y + rad + 1;
                if (yo >= 0) { acc -= tmp[yo * W + x]; cnt--; }
                if (yi < H) { acc += tmp[yi * W + x]; cnt++; }
            }
        }
    }
    static float[] Gauss(float[] src, int W, int H, int rad)
    {
        float[] a = new float[W * H], b = new float[W * H];
        BoxBlur(src, a, W, H, rad); BoxBlur(a, b, W, H, rad); BoxBlur(b, a, W, H, rad);
        return a;
    }

    public static Bitmap Render(Tex sub, Tex inter, Tex top, Tex brd, Tex brdBlur, int RW, int RH, double fl, double ppx, double ppy)
    {
        board = brd; boardBlur = brdBlur;
        boxes = new Box[] {
            new Box(-50, 50, -50, 50, 0, 3, sub, 0),
            new Box(-33, 33, -33, 33, 20, 22.5, inter, 1),
            new Box(-31, 31, -31, 31, 37, 43, top, 2) };
        Ls = new V3[] { L, (L + new V3(0.07, 0, 0)).N(), (L + new V3(-0.07, 0.03, 0)).N(), (L + new V3(0, -0.07, 0.02)).N(), (L + new V3(0.03, 0.06, -0.03)).N() };

        V3 cam = new V3(-58, -178, 120), tgt = new V3(0, 4, 18);
        V3 f = (tgt - cam).N(), rr = V3.Cross(f, new V3(0, 0, 1)).N(), uu = V3.Cross(rr, f);
        float[] R = new float[RW * RH], G = new float[RW * RH], B = new float[RW * RH], D = new float[RW * RH];
        double[] ox = { 0.25, 0.75, 0.25, 0.75 }, oy = { 0.25, 0.25, 0.75, 0.75 };
        for (int y = 0; y < RH; y++)
            for (int x = 0; x < RW; x++)
            {
                double sr = 0, sg = 0, sb = 0, sd = 0;
                for (int s = 0; s < 4; s++)
                {
                    V3 dir = (f * fl + rr * (x + ox[s] - ppx) + uu * (ppy - (y + oy[s]))).N();
                    double r, g, b, dep; Shade(cam, dir, out r, out g, out b, out dep);
                    sr += r; sg += g; sb += b; sd += Math.Min(dep, 2000);
                }
                int i = y * RW + x; R[i] = (float)(sr / 4); G[i] = (float)(sg / 4); B[i] = (float)(sb / 4); D[i] = (float)(sd / 4);
            }

        // depth of field
        float[] Rb = Gauss(R, RW, RH, 2), Gb = Gauss(G, RW, RH, 2), Bb = Gauss(B, RW, RH, 2);
        double focus = (new V3(0, 0, 30) - cam).Len();
        for (int i = 0; i < RW * RH; i++)
        {
            double k = Cl(Math.Abs(D[i] - focus) / 110.0);
            R[i] += (float)((Rb[i] - R[i]) * k); G[i] += (float)((Gb[i] - G[i]) * k); B[i] += (float)((Bb[i] - B[i]) * k);
        }

        // gold particles between layers (bumps in transit)
        Random rnd = new Random(5);
        for (int n = 0; n < 1800; n++)
        {
            double px = Math.Round((rnd.NextDouble() * 58 - 29) / 1.5) * 1.5, py = Math.Round((rnd.NextDouble() * 58 - 29) / 1.5) * 1.5;
            double band = rnd.NextDouble();
            double pz = band < 0.55 ? 3.6 + Math.Pow(rnd.NextDouble(), 1.6) * 15.8 : 23.2 + Math.Pow(rnd.NextDouble(), 1.4) * 13.3;
            V3 P = new V3(px, py, pz), Dv = P - cam;
            double dep = V3.Dot(Dv, f); if (dep <= 1) continue;
            double sx = ppx + fl * V3.Dot(Dv, rr) / dep, sy = ppy - fl * V3.Dot(Dv, uu) / dep;
            int ix = (int)sx, iy = (int)sy; if (ix < 2 || iy < 2 || ix >= RW - 2 || iy >= RH - 2) continue;
            double dist = Dv.Len(); if (dist > D[iy * RW + ix] + 0.8) continue;
            double rad = 0.9 + 230.0 / dep, sig = rad * 0.55, bright = 0.35 + 0.65 * rnd.NextDouble();
            int w = (int)Math.Ceiling(rad * 2.2);
            for (int yy = -w; yy <= w; yy++)
                for (int xx = -w; xx <= w; xx++)
                {
                    int qx = ix + xx, qy = iy + yy; if (qx < 0 || qy < 0 || qx >= RW || qy >= RH) continue;
                    double dd = (qx + 0.5 - sx) * (qx + 0.5 - sx) + (qy + 0.5 - sy) * (qy + 0.5 - sy);
                    double a = Math.Exp(-dd / (2 * sig * sig)) * bright;
                    int q = qy * RW + qx;
                    R[q] += (float)(a * 0.95); G[q] += (float)(a * 0.76); B[q] += (float)(a * 0.40);
                }
        }

        // bloom
        float[] Rh = new float[RW * RH], Gh = new float[RW * RH], Bh = new float[RW * RH];
        for (int i = 0; i < RW * RH; i++) { Rh[i] = Math.Max(0, R[i] - 1.0f); Gh[i] = Math.Max(0, G[i] - 1.0f); Bh[i] = Math.Max(0, B[i] - 1.0f); }
        Rh = Gauss(Rh, RW, RH, 7); Gh = Gauss(Gh, RW, RH, 7); Bh = Gauss(Bh, RW, RH, 7);
        for (int i = 0; i < RW * RH; i++) { R[i] += Rh[i] * 0.6f; G[i] += Gh[i] * 0.6f; B[i] += Bh[i] * 0.6f; }

        // downscale 2x and vignette
        int OW = RW / 2, OH = RH / 2;
        Bitmap outb = new Bitmap(OW, OH, PixelFormat.Format32bppArgb);
        int[] px32 = new int[OW * OH];
        for (int y = 0; y < OH; y++)
            for (int x = 0; x < OW; x++)
            {
                double r = 0, g = 0, b = 0;
                for (int j = 0; j < 2; j++) for (int i = 0; i < 2; i++) { int k = (y * 2 + j) * RW + x * 2 + i; r += R[k]; g += G[k]; b += B[k]; }
                r /= 4; g /= 4; b /= 4;
                double vx = (x - OW * 0.36) / (OW * 0.75), vy = (y - OH * 0.45) / (OH * 0.85);
                double vig = 1 - 0.16 * Smooth(0.45, 1.3, Math.Sqrt(vx * vx + vy * vy));
                r *= vig; g *= vig; b *= vig;
                r = Knee(r); g = Knee(g); b = Knee(b);
                int ir = (int)(Cl(r) * 255 + 0.5), ig = (int)(Cl(g) * 255 + 0.5), ib = (int)(Cl(b) * 255 + 0.5);
                px32[y * OW + x] = (255 << 24) | (ir << 16) | (ig << 8) | ib;
            }
        BitmapData bd = outb.LockBits(new Rectangle(0, 0, OW, OH), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        Marshal.Copy(px32, 0, bd.Scan0, OW * OH); outb.UnlockBits(bd);
        return outb;
    }

    // ---------- overlay: header / logo / login panel ----------
    public static Bitmap Compose(Bitmap render, Bitmap logoWhite)
    {
        Bitmap bmp = new Bitmap(550, 350, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.DrawImage(render, 0, 0, 550, 350);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            using (Font fT = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (Font fS = new Font("Segoe UI Semibold", 8.5f))
            using (Font fM = new Font("Consolas", 6.5f))
            using (Font fMB = new Font("Consolas", 7f, FontStyle.Bold))
            {
                using (LinearGradientBrush haze = new LinearGradientBrush(new Rectangle(0, -2, 550, 96), Color.FromArgb(150, 244, 247, 252), Color.FromArgb(0, 244, 247, 252), 90f))
                    g.FillRectangle(haze, 0, 0, 550, 90);
                using (LinearGradientBrush band = new LinearGradientBrush(new Rectangle(-2, 0, 304, 44), C("#34486A"), C("#52657E"), 0f))
                    g.FillRectangle(band, 0, 0, 300, 44);
                using (SolidBrush ln = new SolidBrush(C("#466EAA"))) g.FillRectangle(ln, 0, 44, 300, 2);
            }
            DrawImageFit(g, logoWhite, new RectangleF(318, 12, 220, 48), null);


            RectangleF lb = new RectangleF(222, 224, 320, 120);
            using (LinearGradientBrush pnl = new LinearGradientBrush(new PointF(0, lb.Y), new PointF(0, lb.Bottom), Color.FromArgb(238, 0x3A, 0x50, 0x72), Color.FromArgb(238, 0x2A, 0x3A, 0x55)))
                g.FillRectangle(pnl, lb);
            using (SolidBrush acc = new SolidBrush(C("#7FA3D8"))) g.FillRectangle(acc, lb.X, lb.Y, lb.Width, 2);
            using (Pen pl = new Pen(Color.FromArgb(70, 200, 216, 240), 1f))
            {
                g.DrawRectangle(pl, lb.X + 0.5f, lb.Y + 0.5f, lb.Width - 1, lb.Height - 1);
                g.DrawLine(pl, lb.X + 8, 299.5f, lb.Right - 8, 299.5f);
            }
            using (Font fM = new Font("Consolas", 6.5f))
            using (SolidBrush l = new SolidBrush(C("#9FB4D2")))
            {
            }
        }
        return bmp;
    }

    public static Bitmap Preview(Bitmap bg, string okPath, string cancelPath)
    {
        Bitmap p = new Bitmap(bg.Width, bg.Height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(p))
        {
            g.DrawImage(bg, 0, 0, bg.Width, bg.Height);
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            Font fLbl = new Font("Gulim", 9f, FontStyle.Bold), fVer = new Font("Malgun Gothic", 8.25f, FontStyle.Bold),
                 fL2 = new Font("Malgun Gothic", 9.75f, FontStyle.Bold), fTb = new Font("Gulim", 9f);
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center })
            {
                g.DrawString("User ID", fLbl, Brushes.White, new RectangleF(229, 237, 122, 20), sf);
                g.DrawString("Password", fLbl, Brushes.White, new RectangleF(229, 263, 122, 20), sf);
            }
            foreach (Rectangle r in new[] { new Rectangle(357, 237, 181, 21), new Rectangle(357, 262, 181, 21) })
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(64, 64, 64))) g.FillRectangle(b, r);
                g.DrawRectangle(Pens.DimGray, r.X, r.Y, r.Width - 1, r.Height - 1);
            }
            g.DrawString("ENG01", fTb, Brushes.White, 360, 241);
            g.DrawString("******", fTb, Brushes.White, 360, 267);
            using (Image ok = Image.FromFile(okPath)) g.DrawImage(ok, 388, 315, 72, 25);
            using (Image cc = Image.FromFile(cancelPath)) g.DrawImage(cc, 466, 315, 72, 25);
            g.DrawString("V5.014.1", fVer, Brushes.White, 117, 22);
            using (Font fA = new Font("Segoe UI", 14.25f, FontStyle.Bold)) g.DrawString("SCK LAB", fA, Brushes.White, 12, 7);
            using (Font fB = new Font("Segoe UI", 9f, FontStyle.Bold)) using (SolidBrush nb = new SolidBrush(Color.FromArgb(31, 58, 104))) g.DrawString("Chain " + DOT + " Defect " + DOT + " E-Test Correlation", fB, nb, 12, 52);
        }
        return p;
    }
}
'@
Add-Type -TypeDefinition $code -ReferencedAssemblies System.Drawing
$png = [System.Drawing.Imaging.ImageFormat]::Png
$sw = [Diagnostics.Stopwatch]::StartNew()
$la = [ChipArt]::LogoAlpha($Logo)
$subB = [ChipArt]::Substrate(1024)
$intB = [ChipArt]::Interposer(1024)
$topB = [ChipArt]::TopDie(1024, $la)
$brdB = [ChipArt]::Board(2048)
$brdBl = [ChipArt]::Blur($brdB, 4)
$render = [ChipArt]::Render((New-Object Tex $subB), (New-Object Tex $intB), (New-Object Tex $topB), (New-Object Tex $brdB), (New-Object Tex $brdBl), 1100, 700, 1010.0, 372.0, 322.0)
$bg = [ChipArt]::Compose($render, $la)
$bg.Save((Join-Path $OutDir 'SCK_LoginBackground.png'), $png)
if ($OkImg -and $CancelImg -and (Test-Path $OkImg) -and (Test-Path $CancelImg)) {
    $pv = [ChipArt]::Preview($bg, $OkImg, $CancelImg)
    $pv.Save((Join-Path $OutDir 'SCK_LoginPreview.png'), $png)
}
"OK {0:N1}s" -f $sw.Elapsed.TotalSeconds
