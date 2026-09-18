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
using DACrux.SEMDMS.RO;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// frmDefecMapAnalysis에 대한 요약 설명입니다.
    /// </summary>
    public class DPUCDefectMapGallery : DACruxCTLBasic01, ISendDefect, IExportExcel
    {
        #region 멤버 변수

        public static readonly int DEFAULT_CONTROL_WIDTH = 200;
        public static readonly int DEFAULT_COL_COUNT = 5;
        public static readonly float CONTROL_RATIO = 1.2f;
        public static readonly string NONE = "None";

        DACrux.Base.DPWafer[] m_wafer = null;
        DefectList m_defectList;
        DataTable m_defectColor;

        private static readonly Color NoDieBackColor;
        private Color m_waferColor;

        #endregion

        #region 컨트롤 멤버 변수

        private Panel panel2;
        private Label label9;
        private Label label8;
        private ComboBox cboRow;
        private ComboBox cboColumn;
        private DACrux.Framework.Controls.DUCControlSpread spread;
        private TrackBar trkSize;
        private Label label10;
        private Panel pnlColCount;
        private Label label1;
        private Panel pnlSize;
        private NumericUpDown numRowCount;
        private Label label2;
        private NumericUpDown numColCount;
        private Button btnCountApply;
        private CheckBox chkShowDies;
        private CheckBox chkNewDefectsOnly;
        private CheckBox chkImageMark;
        private System.ComponentModel.IContainer components;

        #endregion

        #region 생성자 및 Load 이벤트

        public DPUCDefectMapGallery()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();
            //
            // TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
            //
            chkNewDefectsOnly.Enabled = false;
        }

        private void DPUCDefectMapGallery_Load(object sender, System.EventArgs e)
        {
            trkSize.Value = DEFAULT_CONTROL_WIDTH;

            cboColumn.Items.AddRange(GalleryColumnItem.ToArray());
            cboColumn.SelectedIndex = 0;

            cboRow.Items.AddRange(GalleryRowItem.ToArray());
            cboRow.SelectedIndex = 0;

            RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
            m_defectColor = obj.GetColorByDefectType();

            SettingData setting = new SettingData(GetType());
            cboColumn.SelectedIndex = setting.GetValue<int>(cboColumn.Name, 0);
            cboRow.SelectedIndex = setting.GetValue<int>(cboRow.Name, 0);
            numColCount.Value = setting.GetValue<decimal>(numColCount.Name, 3);
            numRowCount.Value = setting.GetValue<decimal>(numRowCount.Name, 3);
            chkShowDies.Checked = setting.GetValue<bool>(chkShowDies.Name, true);
            chkImageMark.Checked = setting.GetValue<bool>(chkImageMark.Name, true);
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkImageMark = new System.Windows.Forms.CheckBox();
            this.chkNewDefectsOnly = new System.Windows.Forms.CheckBox();
            this.chkShowDies = new System.Windows.Forms.CheckBox();
            this.pnlColCount = new System.Windows.Forms.Panel();
            this.btnCountApply = new System.Windows.Forms.Button();
            this.numRowCount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numColCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSize = new System.Windows.Forms.Panel();
            this.trkSize = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cboRow = new System.Windows.Forms.ComboBox();
            this.cboColumn = new System.Windows.Forms.ComboBox();
            this.spread = new DACrux.Framework.Controls.DUCControlSpread();
            this.panel2.SuspendLayout();
            this.pnlColCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRowCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColCount)).BeginInit();
            this.pnlSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSize)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkImageMark);
            this.panel2.Controls.Add(this.chkNewDefectsOnly);
            this.panel2.Controls.Add(this.chkShowDies);
            this.panel2.Controls.Add(this.pnlColCount);
            this.panel2.Controls.Add(this.pnlSize);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.cboRow);
            this.panel2.Controls.Add(this.cboColumn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1013, 32);
            this.panel2.TabIndex = 0;
            // 
            // chkImageMark
            // 
            this.chkImageMark.AutoSize = true;
            this.chkImageMark.Location = new System.Drawing.Point(856, 8);
            this.chkImageMark.Name = "chkImageMark";
            this.chkImageMark.Size = new System.Drawing.Size(91, 16);
            this.chkImageMark.TabIndex = 17;
            this.chkImageMark.Text = "Image Mark";
            this.chkImageMark.UseVisualStyleBackColor = true;
            this.chkImageMark.CheckedChanged += new System.EventHandler(this.chkImageMark_CheckedChanged);
            // 
            // chkNewDefectsOnly
            // 
            this.chkNewDefectsOnly.AutoSize = true;
            this.chkNewDefectsOnly.Location = new System.Drawing.Point(727, 8);
            this.chkNewDefectsOnly.Name = "chkNewDefectsOnly";
            this.chkNewDefectsOnly.Size = new System.Drawing.Size(123, 16);
            this.chkNewDefectsOnly.TabIndex = 16;
            this.chkNewDefectsOnly.Text = "New defects only";
            this.chkNewDefectsOnly.UseVisualStyleBackColor = true;
            this.chkNewDefectsOnly.CheckedChanged += new System.EventHandler(this.chkNewDefectsOnly_CheckedChanged);
            // 
            // chkShowDies
            // 
            this.chkShowDies.AutoSize = true;
            this.chkShowDies.Checked = true;
            this.chkShowDies.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowDies.Location = new System.Drawing.Point(637, 8);
            this.chkShowDies.Name = "chkShowDies";
            this.chkShowDies.Size = new System.Drawing.Size(84, 16);
            this.chkShowDies.TabIndex = 15;
            this.chkShowDies.Text = "Show dies";
            this.chkShowDies.UseVisualStyleBackColor = true;
            this.chkShowDies.CheckedChanged += new System.EventHandler(this.chkShowDies_CheckedChanged);
            // 
            // pnlColCount
            // 
            this.pnlColCount.Controls.Add(this.btnCountApply);
            this.pnlColCount.Controls.Add(this.numRowCount);
            this.pnlColCount.Controls.Add(this.label2);
            this.pnlColCount.Controls.Add(this.numColCount);
            this.pnlColCount.Controls.Add(this.label1);
            this.pnlColCount.Location = new System.Drawing.Point(401, 1);
            this.pnlColCount.Name = "pnlColCount";
            this.pnlColCount.Size = new System.Drawing.Size(230, 32);
            this.pnlColCount.TabIndex = 14;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "Row";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "Col";
            // 
            // pnlSize
            // 
            this.pnlSize.Controls.Add(this.trkSize);
            this.pnlSize.Controls.Add(this.label10);
            this.pnlSize.Location = new System.Drawing.Point(430, 0);
            this.pnlSize.Name = "pnlSize";
            this.pnlSize.Size = new System.Drawing.Size(200, 32);
            this.pnlSize.TabIndex = 13;
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
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(7, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 22);
            this.label10.TabIndex = 12;
            this.label10.Text = "Size";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(205, 6);
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
            this.cboRow.Location = new System.Drawing.Point(262, 7);
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
            // spread
            // 
            this.spread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spread.Location = new System.Drawing.Point(0, 32);
            this.spread.Name = "spread";
            this.spread.Size = new System.Drawing.Size(1013, 568);
            this.spread.TabIndex = 16;
            this.spread.ControlRedraw += new System.EventHandler<DACrux.Framework.Controls.DUCControlSpreadRedrawArgs>(this.spread_ControlRedraw);
            // 
            // DPUCDefectMapGallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.Controls.Add(this.spread);
            this.Controls.Add(this.panel2);
            this.Name = "DPUCDefectMapGallery";
            this.Size = new System.Drawing.Size(1013, 600);
            this.Load += new System.EventHandler(this.DPUCDefectMapGallery_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
            setting.SetValue(chkShowDies.Name, chkShowDies.Checked);
            setting.SetValue(chkImageMark.Name, chkImageMark.Checked);
            setting.Save();
        }

        public void Draw()
        {
            if (m_wafer == null || m_wafer.Length == 0)
                return;

            try
            {
                StatusMessage("데이터를 조회 중입니다.");

                long[] stepSeqArr = new long[m_wafer.Length];

                for (int i = 0; i < m_wafer.Length; i++)
                    stepSeqArr[i] = DACrux.Base.Convert.longParse(m_wafer[i].StepSeq);

                RO.DefectMapAnalysis obj = new RO.DefectMapAnalysis();
                DataSet ds = obj.GetDefectMapViewer_Info(stepSeqArr);

                if (ds == null || !ds.Tables.Contains("SETUP_INFO"))
                    return;

                // Wafer Info 설정
                foreach (DataRow stepRow in ds.Tables["STEP_INFO"].Rows)
                {
                    string stepSeq = stepRow["STEP_SEQ"].ToString();

                    for (int i = 0; i < m_wafer.Length; i++)
                    {
                        if (m_wafer[i].StepSeq == stepSeq)
                        {
                            m_wafer[i].LotID = stepRow["LOT_ID"].ToString();
                            m_wafer[i].Product = stepRow["PRODUCT"].ToString();
                            m_wafer[i].WaferID = stepRow["WAFER_ID"].ToString();
                            break;
                        }
                    }
                }

                DefectList defectList = new DefectList();
                Defect[] defectArr = obj.GetDefectMapViewer_DefectArray(stepSeqArr, chkNewDefectsOnly.Checked);

                // classname 설정
                for (int i = 0; i < defectArr.Length; i++)
                {
                    DefectEx defect = defectArr[i] as DefectEx;
                    defect.CLASSNAME = DmsCache.Instance.ClassLookup[defect.CLASSNUMBER];
                    defectList.Add(defect);
                }

                m_defectList = defectList;

                Draw(defectList);

                // Link 를 통하지 않고 직접 조회하는 경우만 New defects only 기능이 활성화 되도록 한다. 2019.12.21 Taihi,Kim.
                chkNewDefectsOnly.Enabled = true;
            }
            finally
            {
                StatusMessage(null);
            }
        }

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();

            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : DefectMap.DEFAULT_DEFECT_SIZE;
        }

        public void Draw(DefectList defectList)
        {
            m_defectList = defectList;

            if (defectList == null || defectList.Count == 0)
            {
                spread.Clear();
                return;
            }

            try
            {
                StatusMessage("Map을 그리고 있습니다.");

                float defectSize = GetDefaultDefectSize();
                Size controlSize = spread.GetItemSize((int)numColCount.Value, (int)numRowCount.Value);

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

                    DmsWaferDieInfo info = DmsCache.Instance[item.Value.StepSeq];

                    DPUCDefectMapGalleryItem galleryItem = new DPUCDefectMapGalleryItem();
                    galleryItem.Size = controlSize;

                    DefectMap map = galleryItem.DefectMap;
                    map.ForceLoadEvent();
                    map.WaferSize = info.StepInfo.WaferSize;
                    map.NotchType = DACrux.Base.Notch.Notch;
                    map.NotchAngle = 0;
                    map.AngleOffSet = 0;
                    map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                    map.NotchAngle = info.StepInfo.Angle;
                    map.DieSizeX = info.StepInfo.DiePitchX;
                    map.DieSizeY = info.StepInfo.DiePitchY;
                    map.OriginIndexX = info.StepInfo.DieOriginX;
                    map.OriginIndexY = info.StepInfo.DieOriginY;
                    map.OriginX = info.StepInfo.SampleCenterLocationX;
                    map.OriginY = info.StepInfo.SampleCenterLocationY;
                    map.DrawDefects = "ALL";
                    map.DefectSize = defectSize;
                    map.WaferID = info.StepInfo.WaferID;
                    map.EdgeColor = Color.DimGray;
                    map.VisibleImageMark = chkImageMark.Checked;
                    map.CenterMark = true;

                    //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                    ComConfiguration oComConfig = new ComConfiguration();
                    DataTable dtMapOption = oComConfig.SelectDefectMapConfig(DACrux.Base.GlobalVariable.Factory, DACrux.Base.GlobalVariable.UserID);
                    if (dtMapOption != null && dtMapOption.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtMapOption.Rows)
                        {
                            string strType = dr["NAME"].ToString();
                            Color crType = ColorTranslator.FromHtml(dr["VALUE"].ToString());

                            switch (strType)
                            {
                                //Wafer Base Color
                                case "WAFER_MAP_BG":
                                    map.WaferColor = crType;
                                    break;
                                //Inspection Die Color
                                case "WAFER_MAP_INP":
                                    map.DieBackgroundColor = crType;
                                    break;
                                //Inspection Defect Die Color
                                case "WAFER_MAP_DEFECT":
                                    map.DieDefectColor = crType;
                                    break;
                                //Wafer Border Line Color
                                case "WAFER_MAP_LINE":
                                    map.DieBorderColor = crType;
                                    break;

                            }
                        }
                    }

                    DataTable dt = oComConfig.GetConfigurationUser(
                        DACrux.Base.GlobalVariable.Factory,
                        "WAFER_OPTION_M",
                        DACrux.Base.GlobalVariable.UserID
                        );

                    List<string> lsfilete = new List<string>();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lsfilete.Add(dr["NAME"].ToString());
                        }
                    }

                    // 정보 표현을 위해 추가 2019.12.01 Taihi,Kim.
                    if (list.Count == 0)
                    {
                        Defect defect = new Defect();
                        defect.STEP_SEQ = item.Value.StepSeq;
                        list = new DefectList();
                        list.Add(defect);
                    }

                    //Wafer Information 사용 여부
                    dt = oComConfig.GetConfigurationUser(
                        DACrux.Base.GlobalVariable.Factory,
                        "WAFER_OPTION_M_ENABLE",
                        DACrux.Base.GlobalVariable.UserID
                        );

                    galleryItem.Information = DefectMapDraw.GetMapDescription(list, lsfilete.ToArray());

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["VALUE"].ToString() == "N")
                            galleryItem.Information = null;
                    }

                    if (m_defectColor != null)
                        map.TypeColor = m_defectColor;

                    //Virture Die 에 대한 Information Set
                    DataTable dtVir = oComConfig.GetConfigUser(
                        DACrux.Base.GlobalVariable.Factory,
                        "VIRTUAL_OPTION",
                        DACrux.Base.GlobalVariable.UserID
                        );

                    if (dtVir != null && dtVir.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtVir.Rows)
                        {
                            if (dr["NAME"].ToString() == "COLOR")
                            {
                                map.VirtualDieColor = System.Drawing.ColorTranslator.FromHtml(dr["VALUE"].ToString());
                            }

                            if (dr["NAME"].ToString() == "VISIBLE")
                            {
                                if (dr["VALUE"].ToString() == "Y")
                                    WaferMap.AppendVirtualDie(map);
                            }
                        }
                    }

                    foreach (Point pt in info.Dies)
                        map.AddDie(new Die(pt.X, pt.Y, 1, 1));

                    foreach (Defect d in item.Value.DefectList)
                        map.AddDefect(d);

                    controlList.Add(galleryItem, row, col);

                    map.WaferDrawMode = DACrux.Map.MapMode.Fit;

                    //Map 선택이 Defect Select 가 최우선 순위로 바꿔 준다.
                    map.DefectSelectMode();

                    m_waferColor = map.WaferColor;
                }

                Func<string[], string[]> colHeaderSorter = null;
                Func<string[], string[]> rowHeaderSorter = null;

                // Row가 Defect Class 인 경우 classnumber 로 정렬이 필요
                if (GalleryRowItem.Parse(cboRow.Text) == GalleryRowItem.DefectClass)
                    rowHeaderSorter = SortDefectClass;

                spread.SetData(controlList, colHeaderSorter, rowHeaderSorter);
                chkShowDies_CheckedChanged(chkShowDies, EventArgs.Empty);
            }
            finally
            {
                StatusMessage(null);
            }
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
                list.AddRange((arr[i] as DPUCDefectMapGalleryItem).DefectMap.Defects);
            }

            return list.ToArray();
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
            spread.ResizeControl(new Size(trkSize.Value, (int)(trkSize.Value * CONTROL_RATIO)));
        }

        private void btnCountApply_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // None / None 이면 지정된 컬럼 갯수와 로우 갯수에 데이터를 정렬하여 보여준다.
                if (GalleryColumnItem.Parse(cboColumn.Text) == GalleryColumnItem.None && GalleryRowItem.Parse(cboRow.Text) == GalleryRowItem.None && numColCount.Value > 0)
                    Draw(m_defectList);

                spread.ResizeControl((int)numColCount.Value, (int)numRowCount.Value);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void chkShowDies_CheckedChanged(object sender, EventArgs e)
        {
            if (spread.ControlList == null || spread.ControlList.Count == 0)
                return;

            if (chkShowDies.Checked)
            {
                foreach (var item in spread.ControlList)
                {
                    DefectMap map = (item.Control as DPUCDefectMapGalleryItem).DefectMap;
                    map.WaferColor = m_waferColor;
                    map.VisibleDie = true;
                }
            }
            else
            {
                foreach (var item in spread.ControlList)
                {
                    DefectMap map = (item.Control as DPUCDefectMapGalleryItem).DefectMap;
                    map.WaferColor = NoDieBackColor;
                    map.VisibleDie = false;
                }
            }

            spread.RedrawAll();
        }

        private void chkImageMark_CheckedChanged(object sender, EventArgs e)
        {
            if (spread.ControlList == null || spread.ControlList.Count == 0)
                return;

            foreach (var item in spread.ControlList)
            {
                DefectMap map = (item.Control as DPUCDefectMapGalleryItem).DefectMap;
                map.VisibleImageMark = (sender as CheckBox).Checked;
            }
            spread.RedrawAll();
        }

        private void chkNewDefectsOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (spread.ControlList == null || spread.ControlList.Count == 0)
                return;

            Draw();
        }

        private void spread_ControlRedraw(object sender, DUCControlSpreadRedrawArgs e)
        {
            DefectMap map = (e.Control as DPUCDefectMapGalleryItem).DefectMap;

            if (map == null)
                return;

            map.Redraw();
        }

        #endregion
    }
}
