using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace DACrux.ProjectManager.UI
{
    public partial class GradientPanel : System.Windows.Forms.Panel
    {
        #region " ENUM "

        public enum GradientMode
        {
            Horizontal,
            Vertical,
            ForewardDiagonal,
            BackwardDiagonal,
        }

        #endregion

        #region " MEMBER FIELD "

        System.Drawing.Color gStartColor;
        System.Drawing.Color gEndColor;
        System.Drawing.Brush gBrush;
        GradientMode style = GradientMode.Horizontal;

        #endregion

        #region " CREATOR "

        public GradientPanel()
        {
            InitializeComponent();
            
            gStartColor = Color.AliceBlue;
            gEndColor = Color.Transparent;

            GradientStyle = GradientMode.Horizontal;
        }

        #endregion

        #region " PROPERTY "

        [Category("Gradient")]
        public GradientMode GradientStyle
        {
            get
            {
                return style;
            }
            set
            {
                style = value;

                UpdateBrush();
                PaintGradient();
            }
        }

        [Category("Gradient")]
        [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public System.Drawing.Color StartColor
        {
            get
            {
                return gStartColor;
            }
            set
            {
                gStartColor = value;

                UpdateBrush();
                PaintGradient();
            }
        }


        [Category("Gradient")]
        [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public System.Drawing.Color EndColor
        {
            get
            {
                return gEndColor;
            }
            set
            {
                gEndColor = value;

                UpdateBrush();
                PaintGradient();
            }
        }

        #endregion

        #region " METHOD "

        private void PaintGradient()
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(gBrush, new Rectangle(0, 0, this.Width, this.Height));
            this.BackgroundImage = bmp;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void UpdateBrush()
        {
            switch (style)
            {
                case GradientMode.Horizontal:
                    gBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), gStartColor, gEndColor, LinearGradientMode.Horizontal);
                    break;
                case GradientMode.Vertical:
                    gBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), gStartColor, gEndColor, LinearGradientMode.Vertical);
                    break;
                case GradientMode.BackwardDiagonal:
                    gBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), gStartColor, gEndColor, LinearGradientMode.BackwardDiagonal);
                    break;
                case GradientMode.ForewardDiagonal:
                    gBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), gStartColor, gEndColor, LinearGradientMode.ForwardDiagonal);
                    break;
            }
        }

        [System.Diagnostics.DebuggerStepThrough()]
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        #endregion

    }
}
