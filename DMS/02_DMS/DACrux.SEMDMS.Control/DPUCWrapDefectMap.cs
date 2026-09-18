using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DACrux.Map;
using DACrux.Common.RO;

namespace DACrux.SEMDMS.Control
{
	/// <summary>
	/// DPUCWrapDefectMap에 대한 요약 설명입니다.
	/// </summary>
	public class DPUCWrapDefectMap : System.Windows.Forms.UserControl
	{
		public delegate void EventDefectMap(DPUCWrapDefectMap map);
		public event EventDefectMap OnDblClk;
		public event EventDefectMap OnSelectedChange;

		bool selected = false;
		int mapNo = -1;

		private System.Windows.Forms.Panel panel1;
        private DACrux.Map.DefectMap map;
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DPUCWrapDefectMap()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.
            map.DefectSize = GetDefaultDefectSize();
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
            this.map = new DACrux.Map.DefectMap();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DieBackgroundColor = System.Drawing.Color.Black;
            this.map.DieBorderColor = System.Drawing.Color.LightGray;
            this.map.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.map.DieMaxX = 0;
            this.map.DieMaxY = 0;
            this.map.DieMinX = 0;
            this.map.DieMinY = 0;
            this.map.DieSizeX = 0.01D;
            this.map.DieSizeY = 0.01D;
            this.map.DisplayValue = "BIN";
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.DrawDefects = "ALL";
            this.map.DrawFirstDie = true;
            this.map.DrawMarkDie = false;
            this.map.DrawOriginDie = true;
            this.map.DrawSkipDie = true;
            this.map.EdgeColor = System.Drawing.Color.LightGray;
            this.map.EdgeSize = 1D;
            this.map.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.map.FirstDieX = 0;
            this.map.FirstDieY = 0;
            this.map.ForeColor = System.Drawing.Color.Red;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.map.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.map.Name = "map";
            this.map.NotchAngle = 0;
            this.map.NotchType = DACrux.Base.Notch.Flat;
            this.map.OriginDieBorder = System.Drawing.Color.Red;
            this.map.OriginIndexX = 0;
            this.map.OriginIndexY = 0;
            this.map.OriginX = 0D;
            this.map.OriginY = 0D;
            this.map.PickupDieAlpha = 96;
            this.map.PickupedDieColor = System.Drawing.Color.Transparent;
            this.map.PopupMenu = true;
            this.map.ReferenceDieSetting = 0;
            this.map.ScaleMark = false;
            this.map.SelecetedBin = "ALL";
            this.map.Size = new System.Drawing.Size(460, 396);
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 0;
            this.map.TransParent = 255;
            this.map.ViewAngle = 0;
            this.map.VisibleDieBorder = true;
            this.map.VisibleDieValue = false;
            this.map.VisibleFocusDie = false;
            this.map.VisibleInfomation = true;
            this.map.VisibleOffDie = false;
            this.map.VisibleProbeOverlay = false;
            this.map.VisibleStringBin = false;
            this.map.VisibleVIFail = false;
            this.map.VisibleXY = false;
            this.map.WaferBorderColor = System.Drawing.Color.LightGray;
            this.map.WaferColor = System.Drawing.Color.Gray;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.map.WaferMargin = 0.95D;
            this.map.WaferSize = 200000D;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.map.Click += new System.EventHandler(this.map_Click);
            this.map.DoubleClick += new System.EventHandler(this.map_DoubleClick);
            this.map.MouseEnter += new System.EventHandler(this.map_MouseEnter);
            this.map.MouseLeave += new System.EventHandler(this.map_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.map);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 396);
            this.panel1.TabIndex = 1;
            // 
            // DPUCWrapDefectMap
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Name = "DPUCWrapDefectMap";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(464, 400);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public bool Selected
		{
			get
			{
				return this.selected;
			}
			set
			{
				if(value) this.BackColor = Color.Blue;
				else this.BackColor = Color.White;
				this.selected = value;
			}
		}

		public int MapNo
		{
			get
			{
				return this.mapNo;
			}
			set
			{
				this.mapNo = value;
			}
		}

        private float GetDefaultDefectSize()
        {
            float defectSize;
            ComConfiguration obj = new ComConfiguration();

            return obj.TryDefaultDefectSize(out defectSize) ? defectSize : DefectMap.DEFAULT_DEFECT_SIZE;
        }

		private void map_MouseEnter(object sender, System.EventArgs e)
		{
			this.BackColor = Color.Blue;
		}

		private void map_MouseLeave(object sender, System.EventArgs e)
		{
			if(!selected) this.BackColor = Color.White;
		}

		public void Draw(long stepSeq, bool ImageMark)
		{
			try
			{
                if (map.Parent.Disposing)
                {
                    return;
                }
                if (map.InvokeRequired)
                {
                    map.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            Draw(stepSeq, ImageMark);
                        }
                    ));
                }
                else
                {
                    DrawDefect(stepSeq, ImageMark);
                }
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        public void DrawDefect(long stepSeq, bool ImageMark)
        {
            DACrux.SEMDMS.RO.DefectMapAnalysis oDMapAnalysis = null;
            //RO.SEMConfiguration oSemConfig = null;
            ComConfiguration oComConfig = null;
            DataSet dsDefect = null;
            DataTable dt = null;

            try
            {
                dsDefect = new DataSet();

                oDMapAnalysis = new RO.DefectMapAnalysis();
                oComConfig = new ComConfiguration();

                //=================================================================================================================================
                //Setup Seq 정보를 가져 온다.
                //=================================================================================================================================
                dsDefect = oDMapAnalysis.GetDefectMapViewer_Info( new long[] { stepSeq });
                
                if (dsDefect.Tables.IndexOf("SETUP_INFO") < 0)
                    throw new Exception("정의된 Setup 정보가 없습니다. TQD_SETUP Empty");

                map.WaferSize = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["WAFER_SIZE"].ToString());
                map.NotchType = DACrux.Base.Notch.Notch; //dsDefect.Tables["SETUP_INFO"].Rows[0]["NOTCH_TYPE"].ToString().Equals("F") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;
                map.AngleOffSet = 0;
                map.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                map.NotchAngle = 0;// DOWN으로 저장하여 보여주므로 0으로 설정 DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ANGLE"].ToString());

                map.DieSizeX = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_X"].ToString());
                map.DieSizeY = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_PITCH_Y"].ToString());
                map.OriginIndexX = DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_X"].ToString());
                map.OriginIndexY = DACrux.Base.Convert.intParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["DIE_ORIGIN_Y"].ToString());
                map.OriginX = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_X"].ToString());
                map.OriginY = DACrux.Base.Convert.doubleParse(dsDefect.Tables["SETUP_INFO"].Rows[0]["ORIGIN_Y"].ToString());
                map.DieCalculation(true);
                map.DrawDefects = "ALL";


                //=================================================================================================================================
                //Setup Map 정보를 가져 온다.
                //=================================================================================================================================
                if (dsDefect.Tables.IndexOf("SETUP_MAP") < 0)
                    throw new Exception("정의된 Setup 정보가 없습니다. TQD_SETUP Empty");

                map.DieClear();

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

                DataTable defectDt = oDMapAnalysis.GetDefectMapViewer_Defects(new long[] { stepSeq });

                foreach (DataRow dr in defectDt.Rows)
                {
                    map.AddDie(new DACrux.Base.Die(DACrux.Base.Convert.intParse(dr["INDEX_X"].ToString())
                        , DACrux.Base.Convert.intParse(dr["INDEX_Y"].ToString())
                        , DACrux.Base.Convert.intParse(dr["TEST"].ToString())
                        , 1));
                }


                //=================================================================================================================================
                //Defect 정보를 가져 온다.
                //=================================================================================================================================
                if (dsDefect.Tables.IndexOf("DEFECT_INFO") < 0)
                    throw new Exception("정의된 Defect 정보가 없습니다. TQD_SETUP Empty");

                map.DefectClear();

                foreach (DataRow dr in dsDefect.Tables["DEFECT_INFO"].Rows)
                {
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

                    if (dr.Table.Columns.IndexOf("IMAGE_PATH") > -1 && string.IsNullOrEmpty(dr["IMAGE_PATH"].ToString()) == false)
                    {
                        df.IMAGEURL = string.Format("{0}/{1}", dr["IMAGE_PATH"].ToString().Trim(), dr["IMAGE_FILENAME"].ToString().Trim()).Trim();
                    }

                    //try catch 로 하면 속도가 너무 느리다.. 
                    if (dr.Table.Columns.IndexOf("REPEAT_XREL") > -1)
                        df.REPEAT_XREL = DACrux.Base.Convert.intParse(dr["REPEAT_XREL"].ToString());

                    if (dr.Table.Columns.IndexOf("REPEAT_YREL") > -1)
                        df.REPEAT_YREL = DACrux.Base.Convert.intParse(dr["REPEAT_YREL"].ToString());

                    map.AddDefect(df);
                }

                //=================================================================================================================================
                // TQD_CONFIG_USER 테이블의 Global Option 설정되어 있는 항목으로 변경
                //=================================================================================================================================
                // map.AddInfomation(string.Format("LID:{0}", dsDefect.Tables["STEP_INFO"].Rows[0]["LOT_ID"].ToString()));
                // map.AddInfomation(string.Format("WID:{0}", dsDefect.Tables["STEP_INFO"].Rows[0]["WAFER_ID"].ToString()));
                // map.AddInfomation(string.Format("LAYER:{0}", dsDefect.Tables["STEP_INFO"].Rows[0]["STEP_ID"].ToString()));
                dt = oComConfig.GetConfigUser(
                    Base.GlobalVariable.Factory,
                    "WAFER_OPTION",
                    Base.GlobalVariable.UserID
                    );

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        map.AddInfomation(String.Format("{0}: {1}", dr["VALUE"], dsDefect.Tables["STEP_INFO"].Rows[0][dr["NAME"].ToString()].ToString()));
                    }
                }
                else
                {
                    map.AddInfomation(string.Format("LID:{0}", dsDefect.Tables["STEP_INFO"].Rows[0]["LOT_ID"].ToString()));
                    map.AddInfomation(string.Format("WID:{0}", dsDefect.Tables["STEP_INFO"].Rows[0]["SLOT_ID"].ToString()));
                    map.AddInfomation(string.Format("LAYER:{0}", dsDefect.Tables["STEP_INFO"].Rows[0]["STEP_ID"].ToString()));
                }

                //Wafer Information 사용 여부
                dt = oComConfig.GetConfigurationUser(
                 DACrux.Base.GlobalVariable.Factory,
                 "WAFER_OPTION_ENABLE",
                 DACrux.Base.GlobalVariable.UserID
                 );

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["VALUE"].ToString() == "N")
                        map.SetInfomation(null);
                }

                map.VisibleInfomation = true;
                map.VisibleImageMark = ImageMark;

                //=================================================================================================================================
                //Data 기준으로 Map 을 Draw 한다. 
                //=================================================================================================================================
                map.WaferDrawMode = DACrux.Map.MapMode.Fit;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

		public void Copy(DefectMap targetMap)
		{
            ComConfiguration oComConfig = new ComConfiguration();
			try
			{
                targetMap.SetDefaultColors();
				targetMap.DieClear();
				targetMap.DefectClear();

                targetMap.WaferSize = map.WaferSize;
                targetMap.NotchType = DACrux.Base.Notch.Notch; //map.NotchType;
                targetMap.AngleOffSet = map.AngleOffSet;
                targetMap.NotchAngle = map.NotchAngle;
                targetMap.XYDirect = map.XYDirect;

                targetMap.DieSizeX = map.DieSizeX;
                targetMap.DieSizeY = map.DieSizeY;
                targetMap.OriginIndexX = map.OriginIndexX;
                targetMap.OriginIndexY = map.OriginIndexY;
                targetMap.OriginX = map.OriginX;
                targetMap.OriginY = map.OriginY;
                targetMap.DieCalculation(true);
                targetMap.WaferDrawMode = MapMode.Fit;

				this.map.Copy(targetMap);

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

                for (int i = 0; i < map.Dies.Count; i++)
                {
                    targetMap.AddDie(map.Dies[i]);
                    //targetMap.Dies .AddDie(die[i].IndexX, die[i].IndexY, die[i].BinNumber);
                }

                for (int i = 0; i < map.Defects.Count; i++)
                    targetMap.AddDefect(map.Defects[i]);

                targetMap.Refresh();
				targetMap.Redraw();

			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private void map_Click(object sender, System.EventArgs e)
		{
			if(OnSelectedChange != null) OnSelectedChange(this);
		}

		private void map_DoubleClick(object sender, System.EventArgs e)
		{
			if(OnDblClk != null) OnDblClk(this);
		}

		public Bitmap GetMapImage()
		{
			try
			{
				return map.GetMapImage();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        public DefectMap DefectMap
        {
            get { return map; }
        }
	}
}
