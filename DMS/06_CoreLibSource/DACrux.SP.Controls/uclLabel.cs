using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    /// <summary>
    /// Class Name : uclLabel<br/>
    /// Summary    : Label Control Class<br/>
    /// Author     : 미라콤 양형석<br/>
    /// First Date : 2008-11-12<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class uclLabel : System.Windows.Forms.Label, ISupportInitialize
    {
        #region " MEMBER FIELD "

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ImageList imlArrow;
        private bool isImageIncluded = false;

        #endregion

        #region " PROPERTY "

        /// <summary>
        /// Gets or sets whether show the Label image.
        /// </summary>
        [Category("Setup")]
        public bool ShowImage
        {
            get { return isImageIncluded; }
            set 
            { 
                isImageIncluded = value;

                if (isImageIncluded)
                {
                    this.Image = imlArrow.Images[0];
                    this.Text = "     " + this.Text.TrimStart(new char[] { ' ' });
                }
                else
                {
                    this.Image = null;
                    this.Text = this.Text.TrimStart(new char[] { ' ' });
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the caption is bold or normal.
        /// </summary>
        [Category("Setup")]
        public bool HighLight
        {
            get { return Font.Bold; }
            set { this.Font = new System.Drawing.Font(this.Font, value ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular); }
        }

        #endregion

        #region " CREATOR "

        /// <summary>
        /// Initialize.
        /// </summary>
        public uclLabel()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Initialize.
        /// </summary>
        public uclLabel(IContainer container)
        {
            InitializeComponent();
        }

        #endregion

        #region " METHOD "
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uclLabel));
            this.imlArrow = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imlArrow
            // 
            this.imlArrow.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlArrow.ImageStream")));
            this.imlArrow.TransparentColor = System.Drawing.Color.White;
            this.imlArrow.Images.SetKeyName(0, "spot1.gif");
            // 
            // uclLabel
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Location = new System.Drawing.Point(0, 1);
            this.Size = new System.Drawing.Size(80, 15);
            this.ResumeLayout(false);

        }

        #endregion

        #region " ISupportInitialize Implementation "

        public virtual void BeginInit()
        {
        }

        public virtual void EndInit()
        {
        }

        #endregion
    }
}
