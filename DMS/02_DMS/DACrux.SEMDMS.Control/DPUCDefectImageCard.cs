using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// Defect 이미지 카드. 이미지 1장과 글자 2줄(Defect / Die, Class / Bin / 크기)을 컨트롤 1개에 직접 그린다. (2026-10-20)
    /// 기존 DPUCDefectInfo 는 카드 1개에 컨트롤 24개(Label 18 / TrackBar 2 등)가 붙고 원본 이미지를 들고 늘려 그려서,
    /// 이미지가 많으면 창 핸들과 메모리가 크게 늘었다. 기존 카드는 다른 화면이 계속 쓰므로 그대로 두고 새로 만든다.
    /// 설계: 문서/설계/03_KLARF_Map/2026-10-20_Defect_Image_카드_단순화_설계.md
    ///  - 이미지는 원본 비율 그대로 그린다. 폭은 부르는 쪽이 정하고 높이는 카드가 폭과 이미지 비율로 정한다.
    ///  - 원본은 축소본(폭 최대 ThumbMaxWidth)을 만든 뒤 버리고, 큰 이미지 창용으로 파일 바이트만 보관한다.
    ///  - 이미지 읽기(FTP 다운로드 + 축소)는 모든 카드가 함께 쓰는 대기열(백그라운드 스레드 1개)에서 순서대로 한다.
    ///  - 한 번 클릭 = 선택 테두리, 더블클릭 = 기존 ImagePopUp(감마 / 밝기 막대). 카드에서 뺀 값은 큰 창 제목 줄에 모두 적는다.
    /// </summary>
    public class DPUCDefectImageCard : System.Windows.Forms.Control
    {
        /// <summary>이미지 읽기 상태</summary>
        public enum ImageState { Loading, Loaded, Failed }

        private const int Pad = 4;
        private const int LineHeight = 19;
        private const int TextHeight = LineHeight * 2 + 4;
        /// <summary>기본 축소본 최대 폭. Defect Map Viewer 이미지 목록 폭(약 390)보다 조금 크게 잡는다.</summary>
        private const int ThumbMaxWidth = 400;
        /// <summary>이미지를 읽기 전 높이 계산에 쓰는 비율(검사 장비 이미지 1808 x 1572).</summary>
        private const double DefaultRatio = 1572.0 / 1808.0;

        private static readonly Font BoldFont = new Font("맑은 고딕", 9F, FontStyle.Bold);
        private static readonly Font NormalFont = new Font("맑은 고딕", 9F);
        private static readonly Color TextColor = Color.FromArgb(32, 32, 32);
        private static readonly Color SubTextColor = Color.FromArgb(90, 90, 90);
        private static readonly Color BorderColor = Color.FromArgb(200, 200, 200);
        private static readonly Color SelectedColor = Color.FromArgb(0, 120, 215);
        private static readonly Color EmptyImageColor = Color.FromArgb(240, 240, 240);

        private readonly string m_ImagePath;
        /// <summary>이 카드의 축소본 최대 폭. 카드를 크게 쓰는 화면(상세보기 이미지 창)은 더 크게 받는다.</summary>
        private readonly int m_ThumbMaxWidth;
        private Bitmap m_Thumb;
        private byte[] m_ImageBytes;
        private double m_Ratio = DefaultRatio;
        private ImageState m_State = ImageState.Loading;
        private bool m_Selected;
        private volatile bool m_Closed;
        private bool m_Resizing;
        private ImagePopUp m_Popup;

        /// <summary>
        /// Defect 정보와 이미지 경로로 카드를 만든다. imagePath = TQD_IMAGES 의 IMAGE_PATH/IMAGE_FILENAME(FTP 상대 경로).
        /// 경로가 이 PC 에 있는 파일이면 FTP 를 거치지 않고 그대로 읽는다(확인용).
        /// </summary>
        public DPUCDefectImageCard(DACrux.Base.Defect defect, string imagePath)
            : this(defect == null ? 0 : defect.DEFECTID, imagePath)
        {
            if (defect == null)
                return;

            DieText = string.Format("Die ({0}, {1})", defect.XINDEX, defect.YINDEX);
            ClassText = string.Format("Class {0} / Bin {1}", defect.CLASSNUMBER, defect.ROUGHBINNUMBER);
            SizeText = Math.Abs(defect.XSIZE - defect.YSIZE) < 0.005
                ? Num(defect.XSIZE) + " µm"
                : Num(defect.XSIZE) + " x " + Num(defect.YSIZE) + " µm";
            PopupTitle = string.Format("Defect {0} / Die ({1}, {2}) / Class {3} / Bin {4} / Size {5} x {6} µm / REL ({7}, {8}) µm / Cluster {9}",
                defect.DEFECTID, defect.XINDEX, defect.YINDEX, defect.CLASSNUMBER, defect.ROUGHBINNUMBER,
                Num(defect.XSIZE), Num(defect.YSIZE), Num(defect.XREL), Num(defect.YREL), defect.CLUSTERNUMBER);
        }

        /// <summary>Defect ID 만 아는 경우(상세보기 이미지 창). 2줄째와 Die 는 비워 둔다.</summary>
        public DPUCDefectImageCard(long defectId, string imagePath)
            : this(defectId, imagePath, ThumbMaxWidth)
        {
        }

        /// <summary>
        /// 축소본 최대 폭을 정해 만든다(2026-10-22). 카드를 창 크기만큼 크게 그리는 화면은 기본 400 보다 크게 받아야 흐리지 않다.
        /// 읽기는 만들자마자 대기열에 넣으므로 폭은 생성자에서만 정한다.
        /// </summary>
        public DPUCDefectImageCard(long defectId, string imagePath, int thumbMaxWidth)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Name = "DPUCDefectImageCard";
            BackColor = Color.White;
            Margin = new Padding(4);
            Cursor = Cursors.Hand;

            DefectId = defectId;
            Text = "Defect " + defectId;
            DieText = string.Empty;
            ClassText = string.Empty;
            SizeText = string.Empty;
            PopupTitle = "Defect " + defectId;
            m_ImagePath = imagePath ?? string.Empty;
            m_ThumbMaxWidth = Math.Max(1, thumbMaxWidth);

            Size = new Size(220, HeightFor(220));
            ImageLoader.Enqueue(this);
        }

        #region [ Properties ]

        public long DefectId { get; private set; }
        /// <summary>1줄 오른쪽. 예: Die (6, 6)</summary>
        public string DieText { get; private set; }
        /// <summary>2줄 왼쪽. 예: Class 53 / Bin 53</summary>
        public string ClassText { get; private set; }
        /// <summary>2줄 오른쪽. X / Y 크기가 같으면 하나만. 예: 3.23 µm, 6.47 x 3.23 µm</summary>
        public string SizeText { get; private set; }
        /// <summary>더블클릭 큰 이미지 창 제목. 카드에서 뺀 REL / Cluster / X·Y 크기를 모두 적는다.</summary>
        public string PopupTitle { get; private set; }
        public string ImagePath { get { return m_ImagePath; } }
        public ImageState State { get { return m_State; } }

        /// <summary>파란 테두리 선택 표시. 한 번 클릭하면 같은 부모 안의 다른 카드는 해제된다.</summary>
        public bool Selected
        {
            get { return m_Selected; }
            set
            {
                if (m_Selected == value)
                    return;
                m_Selected = value;
                Invalidate();
            }
        }

        #endregion [ Properties ]

        #region [ Size ]

        /// <summary>폭에 맞는 카드 높이 = 이미지(폭 x 비율) + 위아래 여백 + 글자 2줄.</summary>
        private int HeightFor(int width)
        {
            int imageWidth = Math.Max(1, width - Pad * 2);
            return Pad * 2 + (int)Math.Round(imageWidth * m_Ratio) + TextHeight;
        }

        /// <summary>높이는 폭과 이미지 비율로 정한다. 부르는 쪽이 높이를 바꿔도 다시 맞춘다.</summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (m_Resizing)
                return;

            int height = HeightFor(Width);
            if (Height == height)
                return;

            m_Resizing = true;
            try { Height = height; }
            finally { m_Resizing = false; }
        }

        #endregion [ Size ]

        #region [ Paint / Mouse ]

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle image = new Rectangle(Pad, Pad, Width - Pad * 2, Height - Pad * 2 - TextHeight);

            if (m_Thumb != null)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.DrawImage(m_Thumb, image);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(EmptyImageColor))
                    g.FillRectangle(brush, image);
                string message = m_State == ImageState.Loading ? "불러오는 중..." : "이미지를 불러오지 못했습니다";
                TextRenderer.DrawText(g, message, NormalFont, image, Color.Gray,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
            }

            Rectangle line1 = new Rectangle(Pad + 2, image.Bottom + 4, Width - Pad * 2 - 4, LineHeight);
            Rectangle line2 = new Rectangle(line1.Left, line1.Bottom, line1.Width, LineHeight);
            const TextFormatFlags left = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
            const TextFormatFlags right = TextFormatFlags.Right | TextFormatFlags.VerticalCenter;

            TextRenderer.DrawText(g, Text, BoldFont, line1, TextColor, left);
            TextRenderer.DrawText(g, DieText, NormalFont, line1, TextColor, right);
            TextRenderer.DrawText(g, ClassText, NormalFont, line2, SubTextColor, left);
            TextRenderer.DrawText(g, SizeText, NormalFont, line2, SubTextColor, right);

            int border = m_Selected ? 2 : 1;
            using (Pen pen = new Pen(m_Selected ? SelectedColor : BorderColor, border))
            {
                pen.Alignment = PenAlignment.Inset;
                g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (Parent != null)
            {
                foreach (System.Windows.Forms.Control other in Parent.Controls)
                {
                    DPUCDefectImageCard card = other as DPUCDefectImageCard;
                    if (card != null && card != this)
                        card.Selected = false;
                }
            }
            Selected = true;
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            ShowPopup();
        }

        /// <summary>
        /// 큰 이미지 창(기존 ImagePopUp)을 연다. 보관한 파일 바이트로 이미지를 새로 만들어 넘기고, 창이 닫힐 때 해제한다.
        /// 카드가 지워져도(다른 Defect 선택) 열린 창은 그대로 둔다.
        /// </summary>
        public void ShowPopup()
        {
            if (m_ImageBytes == null)
                return;

            if (m_Popup != null && !m_Popup.IsDisposed)
            {
                m_Popup.Activate();
                return;
            }

            MemoryStream stream = new MemoryStream(m_ImageBytes);
            Image image = Image.FromStream(stream);
            ImagePopUp popup = new ImagePopUp(image);
            popup.Text = PopupTitle;
            popup.StartPosition = FormStartPosition.CenterParent;
            popup.FormClosed += delegate
            {
                image.Dispose();
                stream.Dispose();
            };
            m_Popup = popup;
            popup.Show(FindForm());
        }

        #endregion [ Paint / Mouse ]

        #region [ Image Load ]

        /// <summary>
        /// 대기열 스레드에서 부른다. 이미지 파일을 받아 축소본을 만들고 UI 스레드로 넘긴다.
        /// FTP 로 받은 임시 파일은 바이트를 읽은 뒤 지운다(기존 DPUCDefectInfo 와 같음).
        /// </summary>
        private void LoadImage()
        {
            byte[] bytes = null;
            Bitmap thumb = null;
            double ratio = DefaultRatio;
            FileInfo downloaded = null;
            try
            {
                if (m_ImagePath.Length > 0 && Path.IsPathRooted(m_ImagePath) && File.Exists(m_ImagePath))
                {
                    bytes = File.ReadAllBytes(m_ImagePath);
                }
                else
                {
                    downloaded = DefectMapDraw.fnFTPImageDownload(m_ImagePath);
                    if (downloaded != null && downloaded.Exists)
                        bytes = File.ReadAllBytes(downloaded.FullName);
                }

                if (bytes != null && !m_Closed)
                {
                    using (MemoryStream ms = new MemoryStream(bytes))
                    using (Image full = Image.FromStream(ms))
                    {
                        ratio = (double)full.Height / full.Width;
                        int width = Math.Min(m_ThumbMaxWidth, full.Width);
                        int height = Math.Max(1, (int)Math.Round(width * ratio));
                        thumb = new Bitmap(width, height);
                        using (Graphics g = Graphics.FromImage(thumb))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.DrawImage(full, new Rectangle(0, 0, width, height));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Defect Image 읽기 실패 ({0}): {1}", m_ImagePath, ex.Message);
                if (thumb != null)
                    thumb.Dispose();
                thumb = null;
                bytes = null;
            }
            finally
            {
                try
                {
                    if (downloaded != null && downloaded.Exists)
                        downloaded.Delete();
                }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }

            ShowLoaded(thumb, bytes, ratio);
        }

        // 화면에 붙기 전(창 핸들이 없을 때) 읽기가 끝난 결과. 핸들이 생길 때 붙인다.
        private readonly object m_PendingSync = new object();
        private Bitmap m_PendingThumb;
        private byte[] m_PendingBytes;
        private double m_PendingRatio;
        private bool m_PendingReady;

        /// <summary>
        /// 대기열 스레드에서 읽은 결과를 UI 스레드로 넘긴다. 카드가 이미 지워졌으면 축소본만 해제한다.
        /// 창 핸들이 아직 없으면 보관했다가 OnHandleCreated 에서 붙인다. 두 쪽이 동시에 돌 수 있어 잠금 안에서 판단한다.
        /// </summary>
        private void ShowLoaded(Bitmap thumb, byte[] bytes, double ratio)
        {
            lock (m_PendingSync)
            {
                if (m_Closed || IsDisposed)
                {
                    if (thumb != null)
                        thumb.Dispose();
                    return;
                }

                if (!IsHandleCreated)
                {
                    m_PendingThumb = thumb;
                    m_PendingBytes = bytes;
                    m_PendingRatio = ratio;
                    m_PendingReady = true;
                    return;
                }
            }

            try
            {
                BeginInvoke(new Action<Bitmap, byte[], double>(ApplyLoaded), thumb, bytes, ratio);
            }
            catch (InvalidOperationException)
            {
                // 창 핸들이 사라지는 중이다(목록이 지워지는 중).
                if (thumb != null)
                    thumb.Dispose();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            Bitmap thumb;
            byte[] bytes;
            double ratio;
            lock (m_PendingSync)
            {
                if (!m_PendingReady)
                    return;

                m_PendingReady = false;
                thumb = m_PendingThumb;
                bytes = m_PendingBytes;
                ratio = m_PendingRatio;
                m_PendingThumb = null;
                m_PendingBytes = null;
            }
            BeginInvoke(new Action<Bitmap, byte[], double>(ApplyLoaded), thumb, bytes, ratio);
        }

        private void ApplyLoaded(Bitmap thumb, byte[] bytes, double ratio)
        {
            if (m_Closed || IsDisposed)
            {
                if (thumb != null)
                    thumb.Dispose();
                return;
            }

            if (m_Thumb != null)
                m_Thumb.Dispose();
            m_Thumb = thumb;
            m_ImageBytes = thumb == null ? null : bytes;
            m_State = thumb == null ? ImageState.Failed : ImageState.Loaded;
            m_Ratio = thumb == null ? DefaultRatio : ratio;
            Height = HeightFor(Width);
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            m_Closed = true;
            if (disposing)
            {
                if (m_Thumb != null)
                    m_Thumb.Dispose();
                m_Thumb = null;
                m_ImageBytes = null;
                lock (m_PendingSync)
                {
                    if (m_PendingThumb != null)
                        m_PendingThumb.Dispose();
                    m_PendingThumb = null;
                    m_PendingBytes = null;
                    m_PendingReady = false;
                }
            }
            base.Dispose(disposing);
        }

        private static string Num(double value)
        {
            return value.ToString("0.##", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 모든 카드가 함께 쓰는 이미지 읽기 대기열. 백그라운드 스레드 1개가 들어온 순서대로 읽는다.
        /// 기존처럼 카드마다 스레드를 만들지 않고, FTP 접속(설정 조회 + 로그인)도 한 번에 하나씩만 한다.
        /// 지워진 카드는 건너뛴다. 앱 종료를 막지 않도록 IsBackground 로 둔다.
        /// </summary>
        private static class ImageLoader
        {
            private static readonly object Sync = new object();
            private static readonly Queue<DPUCDefectImageCard> Pending = new Queue<DPUCDefectImageCard>();
            private static Thread s_Worker;

            public static void Enqueue(DPUCDefectImageCard card)
            {
                lock (Sync)
                {
                    Pending.Enqueue(card);
                    if (s_Worker == null)
                    {
                        s_Worker = new Thread(Run);
                        s_Worker.IsBackground = true;
                        s_Worker.Name = "DefectImageLoader";
                        s_Worker.Start();
                    }
                    Monitor.Pulse(Sync);
                }
            }

            private static void Run()
            {
                while (true)
                {
                    DPUCDefectImageCard card;
                    lock (Sync)
                    {
                        while (Pending.Count == 0)
                            Monitor.Wait(Sync);
                        card = Pending.Dequeue();
                    }

                    if (card.m_Closed)
                        continue;

                    try
                    {
                        card.LoadImage();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("Defect Image 대기열 오류: {0}", ex.Message);
                    }
                }
            }
        }

        #endregion [ Image Load ]
    }
}
