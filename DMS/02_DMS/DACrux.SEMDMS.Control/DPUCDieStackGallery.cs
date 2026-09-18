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
using System.Collections.Generic;
using DACrux.DMSVFPD.Map;
using System.Threading;
using DACrux.Framework.Controls;
using DACrux.Framework.Base;
using DACrux.Utility;
using DACrux.Common.RO;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// frmDefecMapAnalysis에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCDieStackGallery : System.Windows.Forms.UserControl, ISendDefect, IExportExcel
    {
        #region 멤버 변수

        public static readonly int DEFAULT_CONTROL_WIDTH = 200;
        public static readonly int DEFAULT_COL_COUNT = 5;
        public static readonly float CONTROL_RATIO = 1.2f;
        public static readonly string NONE = "None";

        private SizeF m_dieSize;

        DACrux.Base.DPWafer[] m_wafer = null;
        DefectList m_defectList;

        #endregion

        #region 컨트롤 멤버 변수

        private Panel MapPanel;
        private Panel panel2;
        private Label label9;
        private Label label8;
        private ComboBox cboRow;
        private ComboBox cboColumn;
        private DACrux.Framework.Controls.DUCControlSpread spread;
        private Panel pnlSize;
        private TrackBar trkSize;
        private Label label11;
        private Panel pnlColCount;
        private Button btnCountApply;
        private NumericUpDown numRowCount;
        private Label label10;
        private NumericUpDown numColCount;
        private Label label12;
        private DPUCRDSetup rdSetup;
        private System.ComponentModel.IContainer components;

        #endregion

        #region 생성자 및 Load 이벤트

        public DPUCDieStackGallery()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();
            //
            // TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
            //
        }

        private void DPUCDieStackGallery_Load(object sender, System.EventArgs e)
        {
            trkSize.Value = DEFAULT_CONTROL_WIDTH;

            cboColumn.Items.AddRange(GalleryColumnItem.ToArray());
            cboRow.Items.AddRange(GalleryRowItem.ToArray());

            SettingData setting = new SettingData(GetType());
            cboColumn.SelectedIndex = setting.GetValue<int>(cboColumn.Name, 0);
            cboRow.SelectedIndex = setting.GetValue<int>(cboRow.Name, 0);
            numColCount.Value = setting.GetValue<decimal>(numColCount.Name, 3);
            numRowCount.Value = setting.GetValue<decimal>(numRowCount.Name, 3);
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
            this.MapPanel = new System.Windows.Forms.Panel();
            this.spread = new DACrux.Framework.Controls.DUCControlSpread();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlColCount = new System.Windows.Forms.Panel();
            this.btnCountApply = new System.Windows.Forms.Button();
            this.numRowCount = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numColCount = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.pnlSize = new System.Windows.Forms.Panel();
            this.trkSize = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cboRow = new System.Windows.Forms.ComboBox();
            this.cboColumn = new System.Windows.Forms.ComboBox();
            this.rdSetup = new DACrux.SEMDMS.Control.DPUCRDSetup();
            this.MapPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlColCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRowCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColCount)).BeginInit();
            this.pnlSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSize)).BeginInit();
            this.SuspendLayout();
            // 
            // MapPanel
            // 
            this.MapPanel.Controls.Add(this.rdSetup);
            this.MapPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.MapPanel.Location = new System.Drawing.Point(715, 0);
            this.MapPanel.Name = "MapPanel";
            this.MapPanel.Size = new System.Drawing.Size(298, 600);
            this.MapPanel.TabIndex = 15;
            // 
            // spread
            // 
            this.spread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spread.Location = new System.Drawing.Point(0, 32);
            this.spread.Name = "spread";
            this.spread.Size = new System.Drawing.Size(715, 568);
            this.spread.TabIndex = 16;
            this.spread.ControlRedraw += new System.EventHandler<DACrux.Framework.Controls.DUCControlSpreadRedrawArgs>(this.spread_ControlRedraw);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlColCount);
            this.panel2.Controls.Add(this.pnlSize);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.cboRow);
            this.panel2.Controls.Add(this.cboColumn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(715, 32);
            this.panel2.TabIndex = 0;
            // 
            // pnlColCount
            // 
            this.pnlColCount.Controls.Add(this.btnCountApply);
            this.pnlColCount.Controls.Add(this.numRowCount);
            this.pnlColCount.Controls.Add(this.label10);
            this.pnlColCount.Controls.Add(this.numColCount);
            this.pnlColCount.Controls.Add(this.label12);
            this.pnlColCount.Location = new System.Drawing.Point(425, 0);
            this.pnlColCount.Name = "pnlColCount";
            this.pnlColCount.Size = new System.Drawing.Size(230, 32);
            this.pnlColCount.TabIndex = 16;
            // 
            // btnCountApply
            // 
            this.btnCountApply.Location = new System.Drawing.Point(171, 4);
            this.btnCountApply.Name = "btnCountApply";
            this.btnCountApply.Size = new System.Drawing.Size(53, 23);
            this.btnCountApply.TabIndex = 15;
            this.btnCountApply.Text = "Apply";
            this.btnCountApply.UseVisualStyleBackColor = true;
            this.btnCountApply.Click += new System.EventHandler(this.btnCountApply_Click);
            // 
            // numRowCount
            // 
            this.numRowCount.Location = new System.Drawing.Point(119, 5);
            this.numRowCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numRowCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRowCount.Name = "numRowCount";
            this.numRowCount.Size = new System.Drawing.Size(47, 21);
            this.numRowCount.TabIndex = 17;
            this.numRowCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numRowCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(89, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 12);
            this.label10.TabIndex = 16;
            this.label10.Text = "Row";
            // 
            // numColCount
            // 
            this.numColCount.Location = new System.Drawing.Point(38, 5);
            this.numColCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numColCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numColCount.Name = "numColCount";
            this.numColCount.Size = new System.Drawing.Size(47, 21);
            this.numColCount.TabIndex = 15;
            this.numColCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numColCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 12);
            this.label12.TabIndex = 13;
            this.label12.Text = "Col";
            // 
            // pnlSize
            // 
            this.pnlSize.Controls.Add(this.trkSize);
            this.pnlSize.Controls.Add(this.label11);
            this.pnlSize.Location = new System.Drawing.Point(425, 0);
            this.pnlSize.Name = "pnlSize";
            this.pnlSize.Size = new System.Drawing.Size(200, 32);
            this.pnlSize.TabIndex = 15;
            // 
            // trkSize
            // 
            this.trkSize.LargeChange = 100;
            this.trkSize.Location = new System.Drawing.Point(44, 0);
            this.trkSize.Maximum = 1000;
            this.trkSize.Minimum = 100;
            this.trkSize.Name = "trkSize";
            this.trkSize.Size = new System.Drawing.Size(153, 45);
            this.trkSize.SmallChange = 100;
            this.trkSize.TabIndex = 11;
            this.trkSize.Value = 100;
            this.trkSize.ValueChanged += new System.EventHandler(this.trkSize_ValueChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(7, 2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 22);
            this.label11.TabIndex = 12;
            this.label11.Text = "Size";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(221, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 22);
            this.label9.TabIndex = 10;
            this.label9.Text = "Row";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(0, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 22);
            this.label8.TabIndex = 10;
            this.label8.Text = "Column";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboRow
            // 
            this.cboRow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRow.FormattingEnabled = true;
            this.cboRow.Location = new System.Drawing.Point(272, 6);
            this.cboRow.Name = "cboRow";
            this.cboRow.Size = new System.Drawing.Size(133, 20);
            this.cboRow.TabIndex = 0;
            this.cboRow.SelectedIndexChanged += new System.EventHandler(this.cboRow_SelectedIndexChanged);
            // 
            // cboColumn
            // 
            this.cboColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColumn.FormattingEnabled = true;
            this.cboColumn.Location = new System.Drawing.Point(66, 6);
            this.cboColumn.Name = "cboColumn";
            this.cboColumn.Size = new System.Drawing.Size(133, 20);
            this.cboColumn.TabIndex = 0;
            this.cboColumn.SelectedIndexChanged += new System.EventHandler(this.cboColumn_SelectedIndexChanged);
            // 
            // rdSetup
            // 
            this.rdSetup.Dock = System.Windows.Forms.DockStyle.Right;
            this.rdSetup.Location = new System.Drawing.Point(0, 0);
            this.rdSetup.Name = "rdSetup";
            this.rdSetup.ShotArrayX = 1;
            this.rdSetup.ShotArrayY = 1;
            this.rdSetup.ShotStartX = 1;
            this.rdSetup.ShotStartY = 1;
            this.rdSetup.Size = new System.Drawing.Size(298, 600);
            this.rdSetup.TabIndex = 16;
            this.rdSetup.TargetControl = this.spread;
            this.rdSetup.UseOneByOne = true;
            this.rdSetup.ApplyButtonClick += new System.EventHandler(this.btnApply_Click);
            // 
            // DPUCDieStackGallery
            // 
            this.Controls.Add(this.spread);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.MapPanel);
            this.Name = "DPUCDieStackGallery";
            this.Size = new System.Drawing.Size(1013, 600);
            this.Load += new System.EventHandler(this.DPUCDieStackGallery_Load);
            this.MapPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlColCount.ResumeLayout(false);
            this.pnlColCount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRowCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColCount)).EndInit();
            this.pnlSize.ResumeLayout(false);
            this.pnlSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSize)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region 사용자 정의 메서드

        public void SaveSettings()
        {
            SettingData setting = new SettingData(GetType());
            setting.SetValue(cboColumn.Name, cboColumn.SelectedIndex);
            setting.SetValue(cboRow.Name, cboRow.SelectedIndex);
            setting.SetValue(numColCount.Name, numColCount.Value);
            setting.SetValue(numRowCount.Name, numRowCount.Value);
            setting.Save();
        }

        public void Draw()
        {
            if (m_wafer == null || m_wafer.Length == 0)
                return;

            long[] stepSeqArr = new long[m_wafer.Length];

            for (int i = 0; i < m_wafer.Length; i++)
                stepSeqArr[i] = DACrux.Base.Convert.longParse(m_wafer[i].StepSeq);

            RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
            DataSet ds = obj.GetDefectMapViewer_Info(stepSeqArr);

            if (ds == null || !ds.Tables.Contains("SETUP_INFO"))
                return;

            DataTable defectDt = obj.GetDefectMapViewer_Defects(stepSeqArr);

            // Shot 정보 설정
            DmsWaferDieInfo info = DmsCache.Instance[stepSeqArr[0]];
            rdSetup.ShotArrayX = info.StepInfo.ShotArrayX;
            rdSetup.ShotArrayY = info.StepInfo.ShotArrayY;
            rdSetup.ShotStartX = info.StepInfo.ShotStartX;
            rdSetup.ShotStartY = info.StepInfo.ShotStartY;


            DefectList defectList = new DefectList();

            for (int i = 0; i < defectDt.Rows.Count; i++)
            {
                DataRow dr = defectDt.Rows[i];

                DACrux.Base.Defect df = new DACrux.Base.Defect();

                df.STEP_SEQ = DACrux.Base.Convert.intParse(dr["STEP_SEQ"].ToString());
                df.DEFECTID = DACrux.Base.Convert.intParse(dr["DEFECTID"].ToString());
                df.WAFER_SEQ = DACrux.Base.Convert.intParse(dr["WAFER_SEQ"].ToString());
                df.X = DACrux.Base.Convert.doubleParse(dr["X"].ToString());
                df.Y = DACrux.Base.Convert.doubleParse(dr["Y"].ToString());
                df.XREL = DACrux.Base.Convert.doubleParse(dr["XREL"].ToString());
                df.YREL = DACrux.Base.Convert.doubleParse(dr["YREL"].ToString());
                df.XINDEX = DACrux.Base.Convert.intParse(dr["XINDEX"].ToString());
                df.YINDEX = DACrux.Base.Convert.intParse(dr["YINDEX"].ToString());
                df.XSIZE = DACrux.Base.Convert.doubleParse(dr["XSIZE"].ToString());
                df.YSIZE = DACrux.Base.Convert.doubleParse(dr["YSIZE"].ToString());
                df.DEFECTAREA = DACrux.Base.Convert.doubleParse(dr["DEFECTAREA"].ToString());
                df.DSIZE = DACrux.Base.Convert.doubleParse(dr["DSIZE"].ToString());
                df.CLASSNUMBER = DACrux.Base.Convert.intParse(dr["CLASSNUMBER"].ToString());
                df.TEST = DACrux.Base.Convert.intParse(dr["TEST"].ToString());
                df.CLUSTERNUMBER = DACrux.Base.Convert.intParse(dr["CLUSTERNUMBER"].ToString());
                df.IMAGECOUNT = DACrux.Base.Convert.intParse(dr["IMAGECOUNT"].ToString());
                df.ADDER = DACrux.Base.Convert.intParse(dr["ADDER"].ToString());
                //df.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(dr["ROUGHBINNUMBER"].ToString());
                //df.FINEBINNUMBER = DACrux.Base.Convert.intParse(dr["FINEBINNUMBER"].ToString());
                //df.REVIEWSAMPLE = DACrux.Base.Convert.intParse(dr["REVIEWSAMPLE"].ToString());
                //df.FIRST_STEP = dr["FIRST_STEP"].ToString();
                //df.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["RETICLE_REPEAT_ID"].ToString());
                //df.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(dr["DIE_REPEAT_ID"].ToString());
                //df.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(dr["MAN_OPT_CLASS"].ToString());
                //df.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_OPT_CLASS"].ToString());
                //df.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(dr["MAN_SEM_CLASS"].ToString());
                //df.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(dr["AUTO_SEM_CLASS"].ToString());

                defectList.Add(df);
            }

            m_defectList = defectList;

            Draw(defectList);
        }

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();

            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : MMGDefectMap_Simple.DEFAULT_DEFECT_SIZE;
        }

        private Size GetControlSize()
        {
            if (pnlColCount.Visible)
                return spread.GetItemSize((int)numColCount.Value, (int)numRowCount.Value);
            else
                return new Size(trkSize.Value, (int)(trkSize.Value * CONTROL_RATIO));
        }

        public void Draw(DefectList defectList)
        {
            m_defectList = defectList;

            if (defectList == null || defectList.Count == 0)
            {
                spread.Clear();
                return;
            }

            float defectSize = GetDefaultDefectSize();
            Size controlSize = GetControlSize();

            GalleryColumnItem colItem = GalleryColumnItem.Parse(cboColumn.Text);
            GalleryRowItem rowItem = GalleryRowItem.Parse(cboRow.Text);

            Dictionary<DmsDataGroupBy.ColRow, DefectInfo> dic = DmsDataGroupBy.GroupBy(defectList, m_wafer, colItem, rowItem);

            // None / None 이면 지정된 컬럼 갯수와 로우 갯수에 맞게 보여준다.
            if (colItem == GalleryColumnItem.None && rowItem == GalleryRowItem.None && numColCount.Value > 0)
                dic = DmsDataGroupBy.GroupBy(dic, (int)numColCount.Value);

            DUCControlSpreadItemList controlList = new DUCControlSpreadItemList();

            foreach (var item in dic)
            {
                string col = item.Key.Col;
                string row = item.Key.Row;
                DefectList list = item.Value.DefectList;

                MMGDefectMap_Simple map = new MMGDefectMap_Simple();
                map.ForceLoadEvent();
                map.VisibleImageMark = rdSetup.VisibleImageMarker;
                map.MapType = MAP_TYPE.RD;
                map.Visible = true;
                map.Enabled = false;
                map.Size = controlSize;
                map.SetFunction(DACrux.DMSVFPD.Map.MMGDefectMap_Simple.Function.DefectSelect);
                map.VisibleStatusBar = false;
                map.MapType = MAP_TYPE.RD;

                CGlass glass = new CGlass();
                map.SetGlass(glass);
                map.GlassColor = Color.LightYellow;
                map.ShowClassNumberInfo = true;
                map.MapFitMode = DACrux.DMSVFPD.Map.MapFitMode.DisplayAll;

                DmsWaferDieInfo info = DmsCache.Instance[list[0].STEP_SEQ];

                m_dieSize = new System.Drawing.SizeF((float)info.StepInfo.DiePitchX, (float)info.StepInfo.DiePitchY);

                // Shot 정보 설정
                map.SetShotMap(true, m_dieSize, rdSetup.ShotArrayX, rdSetup.ShotArrayY, rdSetup.ShotStartX, rdSetup.ShotStartY);

                map.DefaultDefectShape = DACrux.DMSVFPD.Map.DefectShape.Rectangle;
                map.DefectClear();
                map.Defects.AddRange(list);

                map.DieMinX = map.DieMinY = Int32.MaxValue;
                map.DefectSize = defectSize;

                // Set Die Min/Max
                foreach (Point point in info.Dies)
                {
                    map.DieMinX = Math.Min(map.DieMinX, point.X);
                    map.DieMinY = Math.Min(map.DieMinY, point.Y);
                }

                map.Information.AddRange(DefectMapDraw.GetMapDescription(list));
                map.FitSize();
                
                controlList.Add(map, row, col);
            }
            
            Func<string[], string[]> colHeaderSorter = null;
            Func<string[], string[]> rowHeaderSorter = null;

            // Row가 Defect Class 인 경우 classnumber 로 정렬이 필요
            if (GalleryRowItem.Parse(cboRow.Text) == GalleryRowItem.DefectClass)
                rowHeaderSorter = SortDefectClass;

            spread.SetData(controlList, colHeaderSorter, rowHeaderSorter);
            spread.RedrawAll();

            //rdSetup.ClearChartData();
            //rdSetup.ApplyClick();
        }

        /// <summary>
        /// class 명에 대해 classnumber로 정렬합니다.
        /// </summary>
        private string[] SortDefectClass(string[] arr)
        {
            SortedList<int, string> list = new SortedList<int, string>();

            foreach (string name in arr)
            {
                int key = DmsCache.Instance.ClassLookup.GetKey(name);
                list.Add(key, name);
            }

            return list.Values.ToArray();
        }

        public void SetData(DACrux.Base.DPWafer[] wafer)
        {
            this.m_wafer = wafer;
        }

        public Defect[] GetSelectedDefect()
        {
            System.Windows.Forms.Control[] arr = spread.GetSelectedControls();

            if (arr == null || arr.Length == 0)
                return null;

            DefectList list = new DefectList();

            for (int i = 0; i < arr.Length; i++)
            {
                list.AddRange((arr[i] as MMGDefectMap_Simple).Defects.ToArray());
            }

            return list.ToArray();
        }

        private void CalcRD()
        {
            int rdCnt = 0;
            int nonRdCnt = 0;
            DUCControlSpreadItemList controlList = spread.ControlList;

            RepeatedDefect rd = new RepeatedDefect();
            rd.Tolerance = rdSetup.Tolerance;
            rd.RepeatCount = rdSetup.RepeatCount;
            rd.ShotStartX = rdSetup.ShotStartX;
            rd.ShotStartY = rdSetup.ShotStartY;
            rd.ShotArrayX = rdSetup.ShotArrayX;
            rd.ShotArrayY = rdSetup.ShotArrayY;
            rd.CalcBy = rdSetup.IsPointMode ? CalcBy.Point : CalcBy.Size;

            foreach (var item in controlList)
            {
                MMGDefectMap_Simple map = item.Control as MMGDefectMap_Simple;

                if (map == null)
                    continue;

                map.SetShotMap(true, m_dieSize, rdSetup.ShotArrayX, rdSetup.ShotArrayY, rdSetup.ShotStartX, rdSetup.ShotStartY);

                rd.DieXSize = map.Glass.GLASS_X / map.ShotArrayX;
                rd.DieYSize = map.Glass.GLASS_Y / map.ShotArrayY;
                rd.Calculate(DmsCache.Instance[m_defectList.GetStepSeqArray()[0]].Dies, map.Defects);

                map.FitSize();

                int rdCount = map.Defects.GetRDCount();
                int nonRd = map.Defects.Count - rdCount;

                rdCnt += rdCount;
                nonRdCnt += nonRd;
            }

            rdSetup.SetChartData(nonRdCnt, rdCnt);
        }

        private void CheckActiveControl()
        {
            //bool allNone = cboColumn.Text == NONE && cboRow.Text == NONE;
            //pnlColCount.Visible = allNone;
            //pnlSize.Visible = !allNone;
            pnlColCount.Visible = true;
            pnlSize.Visible = !pnlColCount.Visible;
        }

        public void ExportExcel()
        {
            ExcelSheet sheet = new ExcelSheet();
            sheet.Add(spread.Spread);

            ExcelExportArgs e = new ExcelExportArgs();
            e.SheetList.Add(sheet);

            ExcelExportManager.Export(e);
        }

        #endregion

        #region 이벤트 처리 메서드

        private void cboColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckActiveControl();
            Draw(m_defectList);
        }

        private void cboRow_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckActiveControl();
            Draw(m_defectList);
        }

        private void trkSize_ValueChanged(object sender, EventArgs e)
        {
            spread.ResizeControl((int)numColCount.Value, (int)numRowCount.Value);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            CalcRD();
            spread.RedrawAll();
        }

        private void spread_ControlRedraw(object sender, DUCControlSpreadRedrawArgs e)
        {
            (e.Control as MMGDefectMap_Simple).FitSize();
        }

        private void btnCountApply_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // None / None 이면 지정된 컬럼 갯수와 로우 갯수에 맞게 보여준다.
                if (GalleryColumnItem.Parse(cboColumn.Text) == GalleryColumnItem.None && GalleryRowItem.Parse(cboRow.Text) == GalleryRowItem.None && numColCount.Value > 0)
                    Draw(m_defectList);
                else
                    spread.ResizeControl((int)numColCount.Value, (int)numRowCount.Value);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}
