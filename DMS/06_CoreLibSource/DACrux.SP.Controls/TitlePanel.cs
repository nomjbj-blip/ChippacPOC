using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    public partial class TitlePanel : System.Windows.Forms.Panel
    {
        #region " CREATOR "

        public TitlePanel()
        {
            InitializeComponent();
        }

        #endregion

        #region " PROPERTY "

        [Category("Setup")]
        public GradientMode GradientStyle
        {
            get
            {
                return this.uclTitle1.GradientStyle;
            }
            set
            {
                this.uclTitle1.GradientStyle = value;
            }
        }

        [Category("Setup")]
        [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public System.Drawing.Color StartColor
        {
            get
            {
                return this.uclTitle1.StartColor;
            }
            set
            {
                this.uclTitle1.StartColor = value;
            }
        }


        [Category("Setup")]
        [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public System.Drawing.Color EndColor
        {
            get
            {
                return this.uclTitle1.EndColor;
            }
            set
            {
                this.uclTitle1.EndColor = value;
            }
        }

        [Category("Setup")]
        public string Title
        {
            get { return this.uclTitle1.Title; }
            set { this.uclTitle1.Title = value; }
        }

        #endregion
    }
}
