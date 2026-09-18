using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    public partial class frmProbeBinMapGallery : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl, DACrux.Framework.Base.IExportExcel
    {

        public frmProbeBinMapGallery()
        {
            InitializeComponent();
        }

        private void frmProbeBinMapGallery_Load(object sender, EventArgs e)
        {
            try
            {
                if (DesignMode) return;

                if (this.WaferList != null && this.WaferList.Length > 0)
                {
                    DrawWaferRecipe(WaferList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        public override void LoadParameter(object parameter)
        {
            // Pattern Search에서 전달한 TPWafer 배열 
            TPWafer[] waferArr = parameter as TPWafer[];

            if (waferArr == null || waferArr.Length == 0)
                return;

            DrawWafer(waferArr);
        }

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            string[] strProducts = null;
            string[] strPrograms = null;
            long[] strWaferSeqs = null;
            try
            {
                strProducts = new string[wafer.Length];
                strPrograms = new string[wafer.Length];
                strWaferSeqs = new long[wafer.Length];
                for (int i = 0; i < wafer.Length; i++)
                {
                    strProducts[i] = wafer[i].Product;
                    strPrograms[i] = wafer[i].Program;
                    strWaferSeqs[i] = DACrux.Base.Convert.longParse(wafer[i].WaferSeq);
                }
                tpucGallery1.Draw(strWaferSeqs);

                this.TPWaferList = wafer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        public void DrawWaferRecipe(string[] strWafer)
        {
            try
            {
                DACrux.Base.TPWafer[] oWafer = new Base.TPWafer[strWafer.Length];
                for (int iWafer = 0; iWafer < strWafer.Length; iWafer++)
                {
                    oWafer[iWafer].WaferSeq = strWafer[iWafer];
                }

                DrawWafer(oWafer);

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

         public void ExportExcel()
         {
             tpucGallery1.ExportExcel();
         }
    }
}
