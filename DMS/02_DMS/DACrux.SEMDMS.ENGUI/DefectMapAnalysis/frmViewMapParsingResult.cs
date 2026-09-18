using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmViewMapParsingResult : DACrux.Framework.Base.DACruxUXBasic01
    {
        public frmViewMapParsingResult()
        {
            InitializeComponent();
        }

        private void frmViewMapParsingResult_Load(object sender, EventArgs e)
        {
            ViewResultList();
        }

        #region  "View Map Parsing Result"

         public void ViewResultList()
        {
            RO.DefectMapAnalysis oDMapAnalysis = null;
            DataTable dataList = null;

            try
            {
                oDMapAnalysis = new RO.DefectMapAnalysis();

                dataList = oDMapAnalysis.SelectMapParsingResultList();
                fpSpread_Result_Sheet.DataSource = dataList;
                fpSpread_Result_Sheet.ColumnHeaderAutoText.ToString();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                if (dataList != null) dataList.Dispose();
                dataList = null;
            }
        }

        #endregion

         #region  "View Map Parsing Result Lot List"

         public DataTable ViewResultLotList()
         {
             RO.DefectMapAnalysis oDMapAnalysis = null;
             DataTable dataList = null;
             try
             {
                 oDMapAnalysis = new RO.DefectMapAnalysis();
                 dataList = oDMapAnalysis.SelectMapParsingResultLotList();
             }
             catch (Exception ex)
             {
                 DspError(ex);
             }

             return dataList;
         }

         #endregion

         #region  "View Map Parsing Result Wafer List"

         public DataTable ViewResultWaferList()
         {
             RO.DefectMapAnalysis oDMapAnalysis = null;
             DataTable dataList = null;

             try
             {
                 oDMapAnalysis = new RO.DefectMapAnalysis();
                 dataList = oDMapAnalysis.SelectMapParsingResultWaferList();
             }
             catch (Exception ex)
             {
                 DspError(ex);
             }

             return dataList;
         }

         #endregion

         #region  "View Map Parsing Result Step List"

         public DataTable ViewResultStepList()
         {
             RO.DefectMapAnalysis oDMapAnalysis = null;
             DataTable dataList = null;

             try
             {
                 oDMapAnalysis = new RO.DefectMapAnalysis();
                 dataList = oDMapAnalysis.SelectMapParsingResultStepList();
             }
             catch (Exception ex)
             {
                 DspError(ex);
             }

             return dataList;
         }

         #endregion

         #region  "View Map Parsing Result Inspection Eq List"

         public DataTable ViewResultInspEqList()
         {
             RO.DefectMapAnalysis oDMapAnalysis = null;
             DataTable dataList = null;

             try
             {
                 oDMapAnalysis = new RO.DefectMapAnalysis();
                 dataList = oDMapAnalysis.SelectMapParsingResultInspEqList();
             }
             catch (Exception ex)
             {
                 DspError(ex);
             }

             return dataList;
         }

         #endregion

         #region  "View Map Parsing Result By Item"

         public void ViewResultByItem(string lotId, string waferId, string stepId, string inspEq)
         {
             RO.DefectMapAnalysis oDMapAnalysis = null;
             DataTable dataList = null;

             try
             {
                 oDMapAnalysis = new RO.DefectMapAnalysis();
                 dataList = oDMapAnalysis.SelectMapParsingResultByItem(lotId, waferId, stepId, inspEq);

                 fpSpread_Result_Sheet.DataSource = dataList;
                 //DACrux.Utility.FPSpreadUtil.SpreadColumnFitSize(fpSpread_Result_Sheet);
                 //DACrux.Utility.Component.SetSpreadData(dataList, fpSpread_Result_Sheet, 100);
             }
             catch (Exception ex)
             {
                 DspError(ex);
             }
             finally
             {
                 if (dataList != null) dataList.Dispose();
                 dataList = null;
             }
         }

         #endregion

         private void cmbLotId_Click(object sender, EventArgs e)
         {             
             DataTable dt = ViewResultLotList();
             cmbLotId.DataSource = dt;
             cmbLotId.DisplayMember = "LOT_ID";
         }

         private void cmbWaferId_Click(object sender, EventArgs e)
         {
             DataTable dt = ViewResultWaferList();
             cmbWaferId.DataSource = dt;
             cmbWaferId.DisplayMember = "WAFER_ID";
         }

         private void cmbStepId_Click(object sender, EventArgs e)
         {
             DataTable dt = ViewResultStepList();
             cmbStepId.DataSource = dt;
             cmbStepId.DisplayMember = "STEP_ID";
         }

         private void cmbInspEq_Click(object sender, EventArgs e)
         {
             DataTable dt = ViewResultInspEqList();
             cmbInspEq.DataSource = dt;
             cmbInspEq.DisplayMember = "INSPECTION_EQ";
         }

         private void btnView_Click(object sender, EventArgs e)
         {
             string lotId = "";
             string waferId = "";
             string stepId = "";
             string inspEq = "";

             if (cmbLotId.Text.Length < 1 || cmbLotId.Text.ToString() == " ")
             {
                 lotId = "%";
             } else {
                 lotId = cmbLotId.Text;
             }

             if (cmbWaferId.Text.Length < 1 || cmbWaferId.Text.ToString() == " ")
             {
                 waferId = "%";
             }
             else
             {
                 waferId = cmbWaferId.Text;
             }

             if (cmbStepId.Text.Length < 1 || cmbStepId.Text.ToString() == " ")
             {
                 stepId = "%";
             }
             else
             {
                 stepId = cmbStepId.Text;
             }

             if (cmbInspEq.Text.Length < 1 || cmbInspEq.Text.ToString() == " ")
             {
                 inspEq = "%";
             }
             else
             {
                 inspEq = cmbInspEq.Text;
             }
             
             ViewResultByItem(lotId, waferId, stepId, inspEq);
         }

         private void cmbLotId_MouseClick(object sender, MouseEventArgs e)
         {
             cmbLotId.Text = "";
         }

         private void cmbStepId_MouseClick(object sender, MouseEventArgs e)
         {
             cmbStepId.Text = "";
         }

         private void cmbWaferId_MouseClick(object sender, MouseEventArgs e)
         {
             cmbWaferId.Text = "";
         }

         private void cmbInspEq_MouseClick(object sender, MouseEventArgs e)
         {
             cmbInspEq.Text = "";
         } 
    }
}
