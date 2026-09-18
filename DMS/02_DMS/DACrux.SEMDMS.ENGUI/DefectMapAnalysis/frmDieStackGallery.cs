using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.Base;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDieStackGallery : DACruxUXBasicDefectLink, iSEMControl, ISendDefect, IExportExcel, iFileControl
    {
        public frmDieStackGallery()
        {
            InitializeComponent();
        }

        bool _activated;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

        private void frmDieStackGallery_Load(object sender, EventArgs e)
        {
            if (DesignMode) 
                return;

            Application.DoEvents();

            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWaferRecipe(WaferList);
            }
            else if (ExistsDefectArray)
            {
                map.Draw(DefectList);
            }
        }

        public void DrawWafer(Base.DPWafer[] wafer)
        {
            map.SetData(wafer);
            map.Draw();

            this.DPWaferList = wafer;
        }

        public void DrawWaferRecipe(string[] strWafer)
        {
            DACrux.Base.DPWafer[] oWafer = new Base.DPWafer[strWafer.Length];
            for (int iWafer = 0; iWafer < strWafer.Length; iWafer++)
            {
                oWafer[iWafer].StepSeq = strWafer[iWafer];
            }

            DrawWafer(oWafer);

            Application.DoEvents();
        }

        public Base.Defect[] GetSelectedDefect()
        {
            return map.GetSelectedDefect();
        }

        public void ExportExcel()
        {
            map.ExportExcel();
        }

        public void DrawWafer(Base.DefectList defectList)
        {
            map.Draw(defectList);
        }

        private void frmDieStackGallery_FormClosing(object sender, FormClosingEventArgs e)
        {
            map.SaveSettings();
        }
    }
}
