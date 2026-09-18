using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Data;
using DACrux.Base;
using DACrux.Framework.Base;
using DACrux.SEMDMS.Interface;
using DACrux.SEMDMS.RO;
using System.Windows.Forms;
using DACrux.Framework;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectImageGallery
        : DACruxUXBasicDefectLink, iSEMControl, ISendDefect, IExportExcel
    {
        //--

        #region [ Constructor ]

        //--

        public frmDefectImageGallery(
            )
        {
            InitializeComponent();
        }

        //--

        #endregion [ Constructor ]

        //--

        #region [ Event Handler ]

        //--

        private void frmDefectImageGallery_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            Application.DoEvents();

            SettingData setData = new SettingData(this.Name);
            this.dpucImageGallery1.ColCount = setData.GetValue<decimal>("COLCOUNT", 1);
            this.dpucImageGallery1.RowCount = setData.GetValue<decimal>("ROWCOUNT", 1);
            this.dpucImageGallery1.NONE = setData.GetValue<bool>("NONE", false);
            this.dpucImageGallery1.Display = setData.GetValue<bool>("DISPLAY", false);
            this.dpucImageGallery1.Zoomable = setData.GetValue<bool>("ZOOMABLE", false);
            if (this.WaferList != null && this.WaferList.Length > 0)
            {
                DrawWafer(
                    WaferList
                    );
            }
            else if (ExistsDefectArray)
            {
                Dictionary<long, int[]> dicDefects = new Dictionary<long, int[]>();
                DefectMapAnalysis obj = new DefectMapAnalysis();
                List<int> lTmp = null;
                DefectList defects = new DefectList();
                foreach (Defect d in DefectList)
                {
                    if (d.IMAGECOUNT <= 0) continue;

                    if (!dicDefects.ContainsKey((long)d.STEP_SEQ))
                    {
                        dicDefects.Add((long)d.STEP_SEQ, new int[] { d.DEFECTID });
                    }
                    else
                    {
                        lTmp = dicDefects[(long)d.STEP_SEQ].ToList<int>();
                        lTmp.Add(d.DEFECTID);
                        dicDefects[(long)d.STEP_SEQ] = lTmp.ToArray<int>();
                    }
                }

                //--

                foreach (KeyValuePair<long, int[]> pv in dicDefects)
                {
                    DataTable dt = obj.GetDefectImage(pv.Key, pv.Value);
                    defects.AddRange(obj.GetDefectImages(dt));
                }

                dpucImageGallery1.DefectDataSource = defects;
                dpucImageGallery1.ImageDataSource = defects;
                dpucImageGallery1.DataBinding();
            }
        }

        //--

        private void frmDefectImageGallery_FormClosed(
            object sender,
            System.Windows.Forms.FormClosedEventArgs e
            )
        {
            SettingData settingData = new SettingData(this.Name);
            settingData.SetValue("COLCOUNT", dpucImageGallery1.ColCount);
            settingData.SetValue("ROWCOUNT", dpucImageGallery1.RowCount);
            settingData.SetValue("NONE", dpucImageGallery1.NONE);
            settingData.SetValue("DISPLAY", dpucImageGallery1.Display);
            settingData.SetValue("ZOOMABLE", dpucImageGallery1.Zoomable);
            settingData.Save();

            dpucImageGallery1.Closed();
        }

        //--

        #endregion [ Event Handler ]

        //--

        #region [ Method ]

        //--

        private void DrawWafer(
            string[] steps
            )
        {
            DPWafer[] oWafers = new DPWafer[steps.Length];
            for (int idx = 0; idx < oWafers.Length; idx++)
            {
                oWafers[idx].StepSeq = steps[idx];
            }

            DrawWafer(
                oWafers
                );
        }

        //--

        public void DrawWafer(
            DPWafer[] wafers
            )
        {
            Wafer = wafers;
            long[] steps = new long[wafers.Length];
            for (int idx = 0; idx < wafers.Length; idx++)
            {
                steps[idx] = DACrux.Base.Convert.longParse(wafers[idx].StepSeq);
            }

            //--

            DrawWafer(
                steps
                );
        }

        //--

        public void DrawWafer(
            long[] steps
            )
        {
            DefectMapAnalysis obj = new DefectMapAnalysis();
            dpucImageGallery1.Wafer = Wafer;
            dpucImageGallery1.DefectDataSource = obj.GetDefectDataToObjectArray_Comp(steps);
            dpucImageGallery1.ImageDataSource = obj.GetDefectImages(steps);
            dpucImageGallery1.DataBinding();
        }

        //--

        public Defect[] GetSelectedDefect()
        {
            return dpucImageGallery1.GetSelectedDefect();
        }

        #endregion [ Method ]

        //--

        #region [ Properties ]
        public DPWafer[] Wafer
        {
            get;
            private set;
        }
        #endregion [ Properties ]

        public void ExportExcel()
        {
            dpucImageGallery1.ExportExcel();
        }
    }
}
