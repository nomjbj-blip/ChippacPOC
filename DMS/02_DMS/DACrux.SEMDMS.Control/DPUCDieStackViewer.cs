using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DACrux.Map;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Linq;
using DACrux.Base;
using DACrux.DMSVFPD.Map;
using System.Collections.Generic;
using DACrux.Framework.Base;
using DACrux.Utility;
using DACrux.Common.RO;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// frmDefecMapAnalysis에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCDieStackViewer : System.Windows.Forms.UserControl, ISendDefect, IExportExcel
    {
        #region 멤버 변수
        
        #endregion

        #region 컨트롤 멤버 변수

        private SizeF m_dieSize;
        private DPUCRDSetup rdSetup;
        private DACrux.DMSVFPD.Map.MMGDefectMap_Simple mmgDefectMap;
        private System.ComponentModel.IContainer components;

        #endregion

        #region 생성자 및 Load 이벤트

        public DPUCDieStackViewer()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();
            //
            // TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
            //
        }

        private void DPUCDieStackMapViewer_Load(object sender, System.EventArgs e)
        {
            if (DesignMode)
                return;

            mmgDefectMap.MapType = MAP_TYPE.RD;
            mmgDefectMap.DefectSize = GetDefaultDefectSize();
        }

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();
            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : MMGDefectMap_Simple.DEFAULT_DEFECT_SIZE;
        }

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.rdSetup = new DACrux.SEMDMS.Control.DPUCRDSetup();
            this.mmgDefectMap = new DACrux.DMSVFPD.Map.MMGDefectMap_Simple();
            this.SuspendLayout();
            // 
            // rdSetup
            // 
            this.rdSetup.Dock = System.Windows.Forms.DockStyle.Right;
            this.rdSetup.Location = new System.Drawing.Point(715, 0);
            this.rdSetup.Name = "rdSetup";
            this.rdSetup.ShotArrayX = 1;
            this.rdSetup.ShotArrayY = 1;
            this.rdSetup.ShotStartX = 1;
            this.rdSetup.ShotStartY = 1;
            this.rdSetup.Size = new System.Drawing.Size(298, 600);
            this.rdSetup.TabIndex = 15;
            this.rdSetup.TargetControl = this.mmgDefectMap;
            this.rdSetup.UseOneByOne = true;
            this.rdSetup.ApplyButtonClick += new System.EventHandler(this.btnApply_Click);
            // 
            // mmgDefectMap
            // 
            this.mmgDefectMap.AngleOffSet = 0;
            this.mmgDefectMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mmgDefectMap.DieMinX = 0;
            this.mmgDefectMap.DieMinY = 0;
            this.mmgDefectMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mmgDefectMap.EdgeColor = System.Drawing.Color.Black;
            this.mmgDefectMap.Enabled_RotateNotch = false;
            this.mmgDefectMap.GlassBorderColor = System.Drawing.Color.Black;
            this.mmgDefectMap.GlassColor = System.Drawing.Color.Gray;
            this.mmgDefectMap.GlassDrawMode = DACrux.DMSVFPD.Map.MapMode.Free;
            this.mmgDefectMap.Location = new System.Drawing.Point(0, 0);
            this.mmgDefectMap.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.mmgDefectMap.Name = "mmgDefectMap";
            this.mmgDefectMap.PopupMenu = true;
            this.mmgDefectMap.RealSizeDefectDrawing = false;
            this.mmgDefectMap.ScaleMark = false;
            this.mmgDefectMap.ShowClassNumberInfo = false;
            this.mmgDefectMap.Size = new System.Drawing.Size(715, 600);
            this.mmgDefectMap.TabIndex = 0;
            this.mmgDefectMap.ViewAngle = 0;
            this.mmgDefectMap.VisibleDefectInRange = false;
            this.mmgDefectMap.VisibleImageMark = true;
            this.mmgDefectMap.VisibleInfomation = true;
            this.mmgDefectMap.VisibleSelectedModel = false;
            this.mmgDefectMap.VisibleShape = false;
            // 
            // DPUCDieStackViewer
            // 
            this.Controls.Add(this.mmgDefectMap);
            this.Controls.Add(this.rdSetup);
            this.Name = "DPUCDieStackViewer";
            this.Size = new System.Drawing.Size(1013, 600);
            this.Load += new System.EventHandler(this.DPUCDieStackMapViewer_Load);
            this.ResumeLayout(false);

        }
        #endregion

        #region [ Method ]

        public void Draw()
        {
            long[] lStepSeq = lStepSeq = new long[Wafer.Length];

            for (int i = 0; i < Wafer.Length; i++)
                lStepSeq[i] = DACrux.Base.Convert.longParse(Wafer[i].StepSeq);

            RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
            DataSet ds = obj.GetDefectMapViewer_Info(lStepSeq);

            if (ds == null || !ds.Tables.Contains("SETUP_INFO"))
                return;

            // Shot 정보 설정
            DmsWaferDieInfo info = DmsCache.Instance[lStepSeq[0]];
            rdSetup.ShotArrayX = info.StepInfo.ShotArrayX;
            rdSetup.ShotArrayY = info.StepInfo.ShotArrayY;
            rdSetup.ShotStartX = info.StepInfo.ShotStartX;
            rdSetup.ShotStartY = info.StepInfo.ShotStartY;

            DefectList defectList = new DefectList();
            defectList.AddRange(obj.GetDefectMapViewer_DefectArray(lStepSeq));
            
            Draw(defectList);
        }

        public void Draw(DefectList defectList)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                mmgDefectMap.SelectedDefect.Clear();
                
                long[] stepSeqArr = defectList.GetStepSeqArray();

                if (stepSeqArr == null || stepSeqArr.Length == 0)
                    return;

                mmgDefectMap.SetFunction(DACrux.DMSVFPD.Map.MMGDefectMap_Simple.Function.DefectSelect);
                CGlass glass = new CGlass();
                mmgDefectMap.SetGlass(glass);
                mmgDefectMap.GlassColor = Color.LightYellow;
                mmgDefectMap.ShowClassNumberInfo = true;
                mmgDefectMap.MapFitMode = DACrux.DMSVFPD.Map.MapFitMode.DisplayAll;
                mmgDefectMap.ClearShotInfo();
                // 첫번째 Map 기준정보로 설정
                DmsWaferDieInfo info = DmsCache.Instance[stepSeqArr[0]];

                m_dieSize = new SizeF((float)info.StepInfo.DiePitchX, (float)info.StepInfo.DiePitchY);
                mmgDefectMap.SetShotMap(true, m_dieSize, rdSetup.ShotArrayX, rdSetup.ShotArrayY, rdSetup.ShotStartX, rdSetup.ShotStartY);

                mmgDefectMap.DefaultDefectShape = DACrux.DMSVFPD.Map.DefectShape.Rectangle;
                mmgDefectMap.DefectClear();
                mmgDefectMap.Defects.AddRange(defectList);

                // Set Die Min/Max
                mmgDefectMap.DieMinX = mmgDefectMap.DieMinY = Int32.MaxValue;

                foreach (Point point in info.Dies)
                {
                    mmgDefectMap.DieMinX = Math.Min(mmgDefectMap.DieMinX, point.X);
                    mmgDefectMap.DieMinY = Math.Min(mmgDefectMap.DieMinY, point.Y);
                }

                mmgDefectMap.Information.Clear();

                string[] infoArr = DefectMapDraw.GetMapDescription(defectList);
                
                if (infoArr != null && infoArr.Length > 0)
                    mmgDefectMap.Information.AddRange(infoArr);
                
                mmgDefectMap.FitSize();

                //rdSetup.ClearChartData();
                //rdSetup.ApplyClick();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void CalcRD()
        {
            mmgDefectMap.SetShotMap(true, m_dieSize, rdSetup.ShotArrayX, rdSetup.ShotArrayY, rdSetup.ShotStartX, rdSetup.ShotStartY);

            long[] stepSeqArr = mmgDefectMap.Defects.GetStepSeqArray();

            if (stepSeqArr == null || stepSeqArr.Length == 0)
                return;

            RepeatedDefect rd = new RepeatedDefect();
            rd.DieXSize = mmgDefectMap.Glass.GLASS_X / mmgDefectMap.ShotArrayX;
            rd.DieYSize = mmgDefectMap.Glass.GLASS_Y / mmgDefectMap.ShotArrayY;
            rd.Tolerance = rdSetup.Tolerance;
            rd.RepeatCount = rdSetup.RepeatCount;
            rd.ShotStartX = rdSetup.ShotStartX;
            rd.ShotStartY = rdSetup.ShotStartY;
            rd.ShotArrayX = rdSetup.ShotArrayX;
            rd.ShotArrayY = rdSetup.ShotArrayY;
            rd.CalcBy = rdSetup.IsPointMode ? CalcBy.Point : CalcBy.Size;
            rd.Calculate(DmsCache.Instance[stepSeqArr[0]].Dies, mmgDefectMap.Defects);

            mmgDefectMap.FitSize();

            int rdCount = mmgDefectMap.Defects.GetRDCount();
            int nonRd = mmgDefectMap.Defects.Count - rdCount;
            rdSetup.SetChartData(nonRd, rdCount);
        }

        public Defect[] GetSelectedDefect()
        {
            if (mmgDefectMap.SelectedDefect.Count > 0)
                return mmgDefectMap.SelectedDefect.ToArray();
            else
                return mmgDefectMap.GetVisibleDefect().ToArray();
        }

        public void ExportExcel()
        {
            ExcelSheet sheet = new ExcelSheet();
            sheet.Add(mmgDefectMap);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet);

            ExcelExportManager.Export(e);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                CalcRD();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion[ Method ]

        #region [ Property ]

        public DACrux.Base.DPWafer[] Wafer
        {
            get;
            set;
        }

        public MMGDefectMap_Simple DefectMap
        {
            get { return mmgDefectMap; }
        }

        #endregion [ Property ]
    }
}
