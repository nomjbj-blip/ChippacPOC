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
    public partial class frmExportToKlarf : DACruxUXBasicDefectLink, iSEMControl, ISendDefect
    {
        public frmExportToKlarf()
        {
            InitializeComponent();
        }

        private void frmExportToKlarf_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            // 링크를 통해 호출 하는 경우
            if (ExistsDefectArray)
            {
                ctl.DefectList.AddRange(DefectList);
                ctl.SetWaferInfo(DefectList.GetStepSeqArray());
            }
        }

        public Base.Defect[] GetSelectedDefect()
        {
            return ctl.GetSelectedDefect();
        }

        // 왼쪽의 조회 컨트롤에서 호출 하는 경우
        public void DrawWafer(Base.DPWafer[] waferArr)
        {
            if (waferArr == null || waferArr.Length == 0)
                return;

            long[] stepSeqArr = new long[waferArr.Length];

            for (int i = 0; i < waferArr.Length; i++)
                stepSeqArr[i] = Convert.ToInt64(waferArr[i].StepSeq);

            RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
            ctl.DefectList.Clear();
            ctl.DefectList.AddRange(obj.GetDefectData(stepSeqArr));
            ctl.SetWaferInfo(stepSeqArr);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ctl.Save();
        }
    }
}
