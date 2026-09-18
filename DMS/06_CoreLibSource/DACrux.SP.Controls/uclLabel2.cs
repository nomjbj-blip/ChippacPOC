using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    public class uclLabel2 : System.Windows.Forms.Label, ISupportInitialize
    {
        #region " MEMBER FIELD "

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ImageList imlArrow;
        private bool isImageIncluded = false;

        #endregion

        #region " PROPERTY "

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

        #endregion

        #region " EVENT "

        #endregion

        #region " CREATOR "

        public uclLabel2()
        {
            InitializeComponent();

        }

        public uclLabel2(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region " EVENT HANDLER "

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uclLabel2));
            this.imlArrow = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imlArrow
            // 
            this.imlArrow.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlArrow.ImageStream")));
            this.imlArrow.TransparentColor = System.Drawing.Color.White;
            this.imlArrow.Images.SetKeyName(0, "spot1.gif");
            // 
            // uclLabel2
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
