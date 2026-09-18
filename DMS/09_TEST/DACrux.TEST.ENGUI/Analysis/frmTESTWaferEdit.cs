using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.TEST.RO;

namespace DACrux.TEST.ENGUI
{
    public partial class frmTESTWaferEdit : DACrux.Framework.Base.DACruxUXBasic01, DACrux.TEST.Interface.iTESTControl
    {
        private string m_TestAreaGroup = string.Empty;
        private string m_TestArea = string.Empty;
        private string m_Product = string.Empty;
        private string m_Program = string.Empty;
        private string m_LotID = string.Empty;
        private string m_WaferID = string.Empty;

        public frmTESTWaferEdit()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
        }

        private void frmTESTWaferEdit_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.WaferList != null && this.WaferList.Length > 0)
                {
                    DrawWaferRecipe(WaferList);
                }

            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        public void DrawWafer(DACrux.Base.TPWafer[] wafer)
        {
            DataSet dsData = null;
            DACrux.TEST.RO.ProbeMapAnalysis oTestMap = null;

            try
            {
                if (wafer.Length != 1)
                {
                    MessageBox.Show(this, "Set only One wafer.", this.Name);
                    return;
                }

                oTestMap = new ProbeMapAnalysis();

                dsData = oTestMap.SelectWaferMapBasic(DACrux.Base.Convert.longParse(wafer[0].WaferSeq));
                if (dsData == null || dsData.Tables.Count <= 0)
                    throw new Exception("Not Found Data");

                tpucMapEditor.DataSource = dsData;
                tpucMapEditor.Draw();

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

       
    }
}
