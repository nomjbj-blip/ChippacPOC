using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DACrux.Base;
using DACrux.SEMDMS.RO;
using DACrux.Common.RO;
//using DMSPlus.DefectMapAnalysis.BDL;
//using TESTPlus.MapAnalysis.BDL;

namespace DACrux.SEMDMS.Control
{
	/// <summary>
	/// DPUCTestMapOverlay에 대한 요약 설명입니다.
	/// </summary>
	public class DPUCTestMapOverlay : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label6;
		private FarPoint.Win.Spread.FpSpread fpSpreadDefectMap;
		private FarPoint.Win.Spread.SheetView fpSpreadDefectMap_Sheet;
		private System.Windows.Forms.Panel panel2;
		private FarPoint.Win.Spread.FpSpread fpSpreadTestMap;
		private System.Windows.Forms.Label label1;
		private FarPoint.Win.Spread.SheetView fpSpreadTestMap_Sheet;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button buttonOverlay;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Splitter splitter2;
        private DACrux.Map.DefectMap defectMap;
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DPUCTestMapOverlay()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

		}

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DPUCTestMapOverlay));
			this.panel1 = new System.Windows.Forms.Panel();
			this.fpSpreadDefectMap = new FarPoint.Win.Spread.FpSpread();
			this.fpSpreadDefectMap_Sheet = new FarPoint.Win.Spread.SheetView();
			this.label6 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.fpSpreadTestMap = new FarPoint.Win.Spread.FpSpread();
			this.fpSpreadTestMap_Sheet = new FarPoint.Win.Spread.SheetView();
			this.label1 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.buttonOverlay = new System.Windows.Forms.Button();
			this.splitter2 = new System.Windows.Forms.Splitter();
            this.defectMap = new DACrux.Map.DefectMap();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefectMap)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefectMap_Sheet)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fpSpreadTestMap)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpreadTestMap_Sheet)).BeginInit();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.fpSpreadDefectMap);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 174);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(256, 266);
			this.panel1.TabIndex = 5;
			// 
			// fpSpreadDefectMap
			// 
			this.fpSpreadDefectMap.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
			this.fpSpreadDefectMap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fpSpreadDefectMap.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
			this.fpSpreadDefectMap.Location = new System.Drawing.Point(0, 23);
			this.fpSpreadDefectMap.Name = "fpSpreadDefectMap";
			this.fpSpreadDefectMap.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
			this.fpSpreadDefectMap.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
			this.fpSpreadDefectMap.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
																						   this.fpSpreadDefectMap_Sheet});
			this.fpSpreadDefectMap.Size = new System.Drawing.Size(256, 243);
			this.fpSpreadDefectMap.TabIndex = 26;
			this.fpSpreadDefectMap.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
			this.fpSpreadDefectMap.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpSpreadDefectMap_SelectionChanged);
			// 
			// fpSpreadDefectMap_Sheet
			// 
			this.fpSpreadDefectMap_Sheet.Reset();
			// Formulas and custom names must be loaded with R1C1 reference style
			this.fpSpreadDefectMap_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
			this.fpSpreadDefectMap_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
			this.fpSpreadDefectMap_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(107)), ((System.Byte)(105)), ((System.Byte)(107)));
			this.fpSpreadDefectMap_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
			this.fpSpreadDefectMap_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
			this.fpSpreadDefectMap_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
			this.fpSpreadDefectMap_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
			this.fpSpreadDefectMap_Sheet.DefaultStyle.Parent = "DataAreaDefault";
			this.fpSpreadDefectMap_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
			this.fpSpreadDefectMap_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(107)), ((System.Byte)(105)), ((System.Byte)(107)));
			this.fpSpreadDefectMap_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
			this.fpSpreadDefectMap_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
			this.fpSpreadDefectMap_Sheet.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
			this.fpSpreadDefectMap_Sheet.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
			this.fpSpreadDefectMap_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(107)), ((System.Byte)(105)), ((System.Byte)(107)));
			this.fpSpreadDefectMap_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
			this.fpSpreadDefectMap_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
			this.fpSpreadDefectMap_Sheet.SheetName = "Sheet1";
			this.fpSpreadDefectMap_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
			// 
			// label6
			// 
			this.label6.Dock = System.Windows.Forms.DockStyle.Top;
			this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
			this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label6.Location = new System.Drawing.Point(0, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(256, 23);
			this.label6.TabIndex = 25;
			this.label6.Text = "     Defect Map";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.fpSpreadTestMap);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(256, 168);
			this.panel2.TabIndex = 6;
			// 
			// fpSpreadTestMap
			// 
			this.fpSpreadTestMap.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
			this.fpSpreadTestMap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fpSpreadTestMap.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
			this.fpSpreadTestMap.Location = new System.Drawing.Point(0, 23);
			this.fpSpreadTestMap.Name = "fpSpreadTestMap";
			this.fpSpreadTestMap.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
			this.fpSpreadTestMap.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
			this.fpSpreadTestMap.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
																						 this.fpSpreadTestMap_Sheet});
			this.fpSpreadTestMap.Size = new System.Drawing.Size(256, 145);
			this.fpSpreadTestMap.TabIndex = 26;
			this.fpSpreadTestMap.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
			this.fpSpreadTestMap.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpSpreadTestMap_SelectionChanged);
			// 
			// fpSpreadTestMap_Sheet
			// 
			this.fpSpreadTestMap_Sheet.Reset();
			// Formulas and custom names must be loaded with R1C1 reference style
			this.fpSpreadTestMap_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
			this.fpSpreadTestMap_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
			this.fpSpreadTestMap_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(107)), ((System.Byte)(105)), ((System.Byte)(107)));
			this.fpSpreadTestMap_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
			this.fpSpreadTestMap_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
			this.fpSpreadTestMap_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
			this.fpSpreadTestMap_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
			this.fpSpreadTestMap_Sheet.DefaultStyle.Parent = "DataAreaDefault";
			this.fpSpreadTestMap_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
			this.fpSpreadTestMap_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(107)), ((System.Byte)(105)), ((System.Byte)(107)));
			this.fpSpreadTestMap_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
			this.fpSpreadTestMap_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
			this.fpSpreadTestMap_Sheet.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
			this.fpSpreadTestMap_Sheet.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
			this.fpSpreadTestMap_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(107)), ((System.Byte)(105)), ((System.Byte)(107)));
			this.fpSpreadTestMap_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
			this.fpSpreadTestMap_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
			this.fpSpreadTestMap_Sheet.SheetName = "Sheet1";
			this.fpSpreadTestMap_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
			this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(256, 23);
			this.label1.TabIndex = 25;
			this.label1.Text = "     Test Map";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.panel1);
			this.panel3.Controls.Add(this.splitter1);
			this.panel3.Controls.Add(this.buttonOverlay);
			this.panel3.Controls.Add(this.panel2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(256, 464);
			this.panel3.TabIndex = 7;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 168);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(256, 6);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// buttonOverlay
			// 
			this.buttonOverlay.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonOverlay.Location = new System.Drawing.Point(0, 440);
			this.buttonOverlay.Name = "buttonOverlay";
			this.buttonOverlay.Size = new System.Drawing.Size(256, 24);
			this.buttonOverlay.TabIndex = 0;
			this.buttonOverlay.Text = "Overlay";
			this.buttonOverlay.Click += new System.EventHandler(this.buttonOverlay_Click);
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point(256, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(6, 464);
			this.splitter2.TabIndex = 8;
			this.splitter2.TabStop = false;
			// 
			// defectMap
			// 
			this.defectMap.AngleOffSet = 0;
			this.defectMap.CenterMark = false;
			this.defectMap.Cursor = System.Windows.Forms.Cursors.Cross;
			this.defectMap.DataSource = null;
			this.defectMap.DieBackgroundColor = System.Drawing.Color.White;
			this.defectMap.DieBorderColor = System.Drawing.Color.LightGray;
			this.defectMap.DieMaxX = 0;
			this.defectMap.DieMaxY = 0;
			this.defectMap.DieMinX = 0;
			this.defectMap.DieMinY = 0;
			this.defectMap.DieSizeX = 0.01;
			this.defectMap.DieSizeY = 0.01;
			this.defectMap.DisplayValue = "BIN";
			this.defectMap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.defectMap.DrawFirstDie = true;
            this.defectMap.DrawMarkDie = false;
			this.defectMap.DrawOriginDie = true;
			this.defectMap.DrawSkipDie = true;
			this.defectMap.EdgeColor = System.Drawing.Color.LightGray;
			this.defectMap.EdgeSize = 1;
			this.defectMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
			this.defectMap.FirstDieX = 0;
			this.defectMap.FirstDieY = 0;
			this.defectMap.ForeColor = System.Drawing.Color.Red;
			this.defectMap.Location = new System.Drawing.Point(262, 0);
            this.defectMap.MapType = DACrux.Base.MAP_TYPE.OVERLAY;
			this.defectMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
			this.defectMap.Name = "defectMap";
			this.defectMap.NotchAngle = 0;
            this.defectMap.NotchType = DACrux.Base.Notch.Flat;
			this.defectMap.OriginDieBorder = System.Drawing.Color.Red;
			this.defectMap.OriginIndexX = 0;
			this.defectMap.OriginIndexY = 0;
			this.defectMap.OriginX = 0;
			this.defectMap.OriginY = 0;
			this.defectMap.PopupMenu = true;
			this.defectMap.ReferenceDieSetting = 0;
			this.defectMap.ScaleMark = false;
			this.defectMap.SelecetedBin = "ALL";
			this.defectMap.Size = new System.Drawing.Size(410, 464);
			this.defectMap.SkipDieColor = System.Drawing.Color.Yellow;
			this.defectMap.TabIndex = 9;
			this.defectMap.VisibleDieBorder = true;
			this.defectMap.VisibleDieValue = false;
			this.defectMap.VisibleInfomation = true;
			this.defectMap.VisibleProbeOverlay = false;
			this.defectMap.VisibleVIFail = false;
			this.defectMap.WaferBorderColor = System.Drawing.Color.LightGray;
			this.defectMap.WaferColor = System.Drawing.Color.Gray;
            this.defectMap.WaferDrawMode = DACrux.Map.MapMode.Free;
			this.defectMap.WaferSize = 200000;
            this.defectMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
			// 
			// DPUCTestMapOverlay
			// 
			this.Controls.Add(this.defectMap);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.panel3);
			this.Name = "DPUCTestMapOverlay";
			this.Size = new System.Drawing.Size(672, 464);
			this.Load += new System.EventHandler(this.DPUCTestMapOverlay_Load);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefectMap)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpreadDefectMap_Sheet)).EndInit();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fpSpreadTestMap)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpreadTestMap_Sheet)).EndInit();
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public void ListUpStepInfo(DPWafer [] wafer)
		{
			DefectMapAnalysis o = null;
			DataTable dt = null;
			try
			{
				string [] stepSeq = new string[wafer.Length];
				for(int i = 0; i < stepSeq.Length; i++) stepSeq[i] = wafer[i].StepSeq;
                o = new DefectMapAnalysis();
				dt = o.GetStepInfo(stepSeq);
                fpSpreadDefectMap_Sheet.DataSource = dt;
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadDefectMap_Sheet);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
				dt = null;

				o = null;
			}
		}

		public void ListUpWaferInfo(DACrux.Base.TPWafer [] wafer)
		{
            DefectMapAnalysis o = null;
			DataTable dt = null;
			try
			{
				string [] waferSeq = new string[wafer.Length];
                string[] program = new string[wafer.Length];
                for (int i = 0; i < waferSeq.Length; i++)
                {
                    waferSeq[i] = wafer[i].WaferSeq;
                    program[i] = wafer[i].Program;
                }
                o = new DefectMapAnalysis();
                dt = o.GetWaferDefectOverlay(program[0], waferSeq);
				fpSpreadTestMap_Sheet.DataSource = dt;
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadTestMap_Sheet);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
				dt = null;

				o = null;
			}
		}

		public void WaferInfo(string [] stepSeq)
		{
			DefectMapAnalysis o = null;
			DataTable dt = null;
			try
			{
				o = new DefectMapAnalysis();
                dt = o.GetStepInfo(stepSeq);
				fpSpreadDefectMap_Sheet.DataSource = dt;
/*
				FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
				numberCellType.FixedPoint = false;
				this.fpSpreadDefectMap_Sheet.Columns.Get(0).CellType = numberCellType;
				this.fpSpreadDefectMap_Sheet.Columns.Get(1).CellType = numberCellType;
*/
#if RELEASE
				fpSpreadDefectMap_Sheet.Columns[0].Visible = false;
#endif
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadDefectMap_Sheet);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(dt != null) dt.Dispose();
				dt = null;
				o = null;
			}
		}

		private void DPUCTestMapOverlay_Load(object sender, System.EventArgs e)
		{
			if(DesignMode) return;

            defectMap.DefectSize = GetDefaultDefectSize();
			/*
			try
			{
				string [] waferSeq = null;
				if(fpSpreadDefectMap_Sheet.DataSource == null)
				{
					waferSeq = new string[] {
														  "20151", "20190", "20210", "20211", "20230", "20250", "20291", "20293", "20310", "20330", "20350", "20370", "20371", "20390", "20450", "20510", "20530", "20550", "20570", "20590", "20610", "20650", "20651", "20652", "20653", "20654", "20670", "20691", "20710", "20711", "20712", "20713", "20714", "20110", "20130", "20150", "20716", "20717", "20718", "20719", "20720", "20721", "20722", "20723", "20730", "20752", "20792", "20793", "20810", "20830", "20731", "20732", "20750", "20751", "20871", "20873", "20890", "20891", "20910", "20911", "20930", "20950", "20970", "20990", "21030", "21050", "21051", "21070", "21091"
													  };
				}

				WaferInfo(waferSeq);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
			*/
		}

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();

            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : DACrux.Map.DefectMap.DEFAULT_DEFECT_SIZE;
        }

		string SelectedDefectColumn(string colName, int row)
		{
			DataView dv = fpSpreadDefectMap_Sheet.GetDataView(false);
			return string.Format("{0}", dv[row][colName]);
		}

		string SelectedTestColumn(string colName, int row)
		{
			DataView dv = fpSpreadTestMap_Sheet.GetDataView(false);
			return string.Format("{0}", dv[row][colName]);
		}

		private void fpSpreadDefectMap_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
		{
			/*
			TQP_WAFER_SUM o = null;
			DataTable dt = null;
			try
			{
				this.Cursor = Cursors.WaitCursor;

				string product = SelectedDefectColumn("PRODUCT", e.Range.Row);
				string lotId = SelectedDefectColumn("LOT_ID", e.Range.Row);
				string waferId = SelectedDefectColumn("WAFER_ID", e.Range.Row);

				o = new TQP_WAFER_SUM();
				dt = o.GetWaferInfo(product, lotId, waferId);
				fpSpreadTestMap_Sheet.DataSource = dt;

				fpSpreadTestMap_Sheet.Columns[0].Visible = false;

				Miracom.Common.Win.FPSpreadUtil.SpreadColumnFitSize(fpSpreadTestMap_Sheet);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
			finally
			{
				o = null;
				if(dt != null) dt.Dispose();
				dt = null;
				this.Cursor = Cursors.Default;
			}*/
		}

		private void buttonOverlay_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;

				FarPoint.Win.Spread.Model.CellRange cr = fpSpreadTestMap_Sheet.GetSelection(0);
				FarPoint.Win.Spread.Model.CellRange crDefect = fpSpreadDefectMap_Sheet.GetSelection(0);

				if(cr == null || cr.Row == -1) return;
				if(crDefect == null || crDefect.Row == -1) return;

				string product = SelectedTestColumn("PRODUCT", cr.Row);
				string program = SelectedTestColumn("PROGRAM", cr.Row);
				string waferSeq = SelectedTestColumn("WAFER_SEQ", cr.Row);
				string stepSeq = SelectedDefectColumn("STEP_SEQ", crDefect.Row);

                //TestMapDraw.DrawWaferMap(defectMap, product, program, waferSeq, stepSeq);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void fpSpreadTestMap_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
		{
            DefectMapAnalysis o = null;
			DataTable dt = null;
			try
			{
				this.Cursor = Cursors.WaitCursor;

				string lotId = SelectedTestColumn("LOT_ID", e.Range.Row);
				string waferId = SelectedTestColumn("WAFER_ID", e.Range.Row);

                o = new DefectMapAnalysis();
				dt = o.GetStepInfoOverlay(lotId, waferId);
				fpSpreadDefectMap_Sheet.DataSource = dt;
                DACrux.Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpreadDefectMap_Sheet);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source);
			}
			finally
			{
				o = null;
				if(dt != null) dt.Dispose();
				dt = null;
				this.Cursor = Cursors.Default;
			}
		}
	}
}
