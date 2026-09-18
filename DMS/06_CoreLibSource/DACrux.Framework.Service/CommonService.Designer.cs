namespace DACrux.Framework.Service
{
    partial class CommonService
    {
        #region Class Member

        private System.ComponentModel.IContainer components = null;

        #endregion

        #region Dispose

        /// <summary>
        /// Release All using resource
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "DACruxCore";
        }

        #endregion
    }
}
