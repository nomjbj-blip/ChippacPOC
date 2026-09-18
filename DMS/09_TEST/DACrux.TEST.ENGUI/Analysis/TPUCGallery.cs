using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using DACrux.Utility;
using Excel = Microsoft.Office.Interop.Excel;
using DACrux.Common.RO; 

namespace DACrux.TEST.ENGUI
{
    public partial class TPUCGallery : UserControl, DACrux.Framework.Base.IExportExcel
    {
        public delegate void EventHandlerClose();
        public event EventHandlerClose OnClose;

        public bool isFromMapView = false;

        long[] m_WaferSeqs = null;

        bool threadStop = false;
        bool endCreateMap = true;
        bool endDrawMap = true;
        bool endLayout = true;

        delegate void EvnetHandlerMapCreate();
        event EvnetHandlerMapCreate OnMapCreate;

        DACrux.TEST.ENGUI.WrapMap lastSelectedMap = null;
        bool isDrawing = false;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonLast;
        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonFirst;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonRedraw;
        private System.Windows.Forms.ComboBox comboBoxCol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStop;
        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Panel panelSingle;

        public TPUCGallery()
        {
            InitializeComponent();
            this.OnMapCreate += new EvnetHandlerMapCreate(Gallery_OnMapCreate);
        }

        public bool CloseButtonVisiable
        {
            get
            {
                return buttonClose.Visible;
            }
            set
            {
                buttonClose.Visible = value;
            }
        }

        void CreateMap()
        {

            if (comboBoxCol.Text.Length == 0) return;

            try
            {
                isDrawing = true;

                labelStatus.Text = "맵을 생성합니다.";
                ultraProgressBar.Value = 0;
                ultraProgressBar.Maximum = m_WaferSeqs.Length;

                panelMap.SuspendLayout();
                for (int i = 0; i < m_WaferSeqs.Length; i++)
                {
                    CheckForIllegalCrossThreadCalls = false;

                    Thread.Sleep(50);

                    Invoke(OnMapCreate);

                    ultraProgressBar.Value = i + 1;

                    if (threadStop) break;
                }
                panelMap.ResumeLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isDrawing = false;
                threadStop = false;
                endCreateMap = true;
            }
        }

        void CreateMapNoInvoke()
        {
            if (comboBoxCol.Text.Length == 0) return;

            try
            {
                isDrawing = true;

                labelStatus.Text = "맵을 생성합니다.";
                ultraProgressBar.Value = 0;
                ultraProgressBar.Maximum = m_WaferSeqs.Length;

                panelMap.SuspendLayout();
                for (int i = 0; i < m_WaferSeqs.Length; i++)
                {
                    //Invoke(OnMapCreate);
                    Gallery_OnMapCreate();

                    ultraProgressBar.Value = i + 1;

                    if (threadStop) break;
                }
                panelMap.ResumeLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isDrawing = false;
                threadStop = false;
                endCreateMap = true;
            }
        }

        private void Gallery_Load(object sender, System.EventArgs e)
        {
            //if(!DesignMode) timer.Enabled = true;

            if (DesignMode) return;

            panelMap.Dock = DockStyle.Fill;
            panelSingle.Dock = DockStyle.Fill;

            panelMap.BringToFront();

            DACrux.TEST.ENGUI.WrapMap map = new DACrux.TEST.ENGUI.WrapMap();
            map.Single = true;
            map.OnDblClick += new DACrux.TEST.ENGUI.WrapMap.EventHandlerSelected(map_OnDblClick);
            map.Dock = DockStyle.Fill;
            panelSingle.Controls.Add(map);
        }

        public void Draw(long[] WaferSeqs)
        {
            try
            {
                if (!endCreateMap) return;
                if (!endDrawMap) return;

                endCreateMap = false;
                endDrawMap = false;

                //panelMap.Controls.Clear();
                DACrux.TEST.ENGUI.WrapMap map = null;
                for (int i = panelMap.Controls.Count - 1; i >= 0; i--)
                {
                    map = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i];
                    map.Dispose();
                }

                this.m_WaferSeqs = WaferSeqs;

                System.Threading.Thread thread = new System.Threading.Thread(
                    new System.Threading.ThreadStart(CreateMap));
                thread.Name = "Create map.";
                CheckForIllegalCrossThreadCalls = false;
                thread.Start();

                //CreateMapNoInvoke();

                System.Threading.Thread threadDraw = new System.Threading.Thread(
                    new System.Threading.ThreadStart(DrawWaferMapThread));
                threadDraw.Name = "Draw map";
                threadDraw.Start();

                //DrawWaferMapThread();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            if (!DesignMode)
            {
                timer.Enabled = false;

                //CreateMap();

                //				System.Threading.Thread thread = new System.Threading.Thread(
                //					new System.Threading.ThreadStart(CreateMap));
                //				thread.Name = "맵을 그립니다.";
                //				thread.Start();
            }
        }

        void MapLayout()
        {
            if (panelMap.Controls.Count == 0) return;

            labelStatus.Text = "Arrange the Map";
            ultraProgressBar.Value = 0;
            ultraProgressBar.Maximum = panelMap.Controls.Count;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                int colCnt = (panelMap.Width - 20) / panelMap.Controls[0].Width;
                int curCol = 0;
                int curRow = 0;
                // map의 배열을 다시 한다
                panelMap.SuspendLayout();
                for (int i = 0; i < panelMap.Controls.Count; i++)
                {
                    panelMap.Controls[i].Location =
                        new Point(curCol * panelMap.Controls[0].Width + panelMap.DisplayRectangle.Left,
                        curRow * panelMap.Controls[0].Height + panelMap.DisplayRectangle.Top);
                    curCol++;
                    if (curCol == colCnt)
                    {
                        curCol = 0;
                        curRow++;
                    }

                    ultraProgressBar.Value = i + 1;
                }
                panelMap.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                endLayout = true;
            }
        }

        private void panelMap_SizeChanged(object sender, System.EventArgs e)
        {
            if (DesignMode) return;
            if (isDrawing) return;
            if (!endDrawMap) return;
            if (!endLayout) return;
            Panel panel = (Panel)sender;
            if (panel.Controls.Count == 0) return;

            endLayout = false;

            System.Threading.Thread thread = new System.Threading.Thread(
                new System.Threading.ThreadStart(MapLayout));
            thread.Name = "Rearrange the Map.";
            thread.Start();
        }

        void Redraw()
        {
            if (comboBoxCol.Text.Length == 0) return;

            isDrawing = true;

            labelStatus.Text = "Draw Map";
            ultraProgressBar.Value = 0;
            ultraProgressBar.Maximum = panelMap.Controls.Count;
            this.Cursor = Cursors.WaitCursor;

            int colCnt = DACrux.Base.Convert.intParse(comboBoxCol.Text);
            int size = (panelMap.Width - 20) / colCnt;
            int row = 0;
            int col = 0;
            DACrux.TEST.ENGUI.WrapMap map = null;
            panelMap.SuspendLayout();
            for (int i = 0; i < panelMap.Controls.Count; i++)
            {
                map = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i];
                map.Location = new Point(col * size, row * size);
                map.Size = new Size(size, size);

                col++;
                if (col % colCnt == 0)
                {
                    row++;
                    col = 0;
                }

                ultraProgressBar.Value = i + 1;
            }
            panelMap.ResumeLayout();
            this.Cursor = Cursors.Default;
            isDrawing = false;
        }

        private void buttonRedraw_Click(object sender, System.EventArgs e)
        {
            if (!endCreateMap) return;
            if (!endDrawMap) return;

            if (m_WaferSeqs == null || m_WaferSeqs.Length <= 0)
                return;

            panelMap.BringToFront();
            System.Threading.Thread thread = new System.Threading.Thread(
                new System.Threading.ThreadStart(Redraw));
            thread.Name = "Draw Map";
            thread.Start();
        }

        private void map_OnSelected(DACrux.TEST.ENGUI.WrapMap map)
        {
            DACrux.TEST.ENGUI.WrapMap m = null;
            if (ModifierKeys == Keys.Control || ModifierKeys == Keys.ControlKey)
            {
                map.Selected = !map.Selected;
                lastSelectedMap = map;
            }
            else if (ModifierKeys == Keys.Shift || ModifierKeys == Keys.ShiftKey)
            {
                int mapIdx1 = 0;
                int mapIdx2 = 0;
                for (int i = 0; i < this.panelMap.Controls.Count; i++)
                {
                    m = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i];
                    if (lastSelectedMap != null)
                    {
                        if (m.MapId == lastSelectedMap.MapId) mapIdx1 = i;
                        if (m.MapId == map.MapId) mapIdx2 = i;
                    }
                    else
                    {
                        if (m.MapId == map.MapId)
                        {
                            mapIdx1 = i;
                            mapIdx2 = i;
                            break;
                        }
                    }
                }

                int startIdx = 0;
                int endIdx = 0;
                if (mapIdx1 < mapIdx2)
                {
                    startIdx = mapIdx1;
                    endIdx = mapIdx2;
                }
                else
                {
                    startIdx = mapIdx2;
                    endIdx = mapIdx1;
                }

                for (int i = 0; i < this.panelMap.Controls.Count; i++)
                {
                    m = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i];
                    m.Selected = false;
                }

                for (int i = 0; i < this.panelMap.Controls.Count; i++)
                {
                    m = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i];
                    if (m.MapId >= startIdx && m.MapId <= endIdx) m.Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < this.panelMap.Controls.Count; i++)
                {
                    m = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i];
                    m.Selected = false;
                }
                map.Selected = true;
                lastSelectedMap = map;
            }
        }

        private void panelMap_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DACrux.TEST.ENGUI.WrapMap m = null;
            for (int i = 0; i < this.panelMap.Controls.Count; i++)
            {
                m = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i];
                m.Selected = false;
            }
            lastSelectedMap = null;
        }

        private void Gallery_OnMapCreate()
        {
            try
            {
                int colCnt = DACrux.Base.Convert.intParse(comboBoxCol.Text);
                int size = (panelMap.Width - 20) / colCnt;

                int row = panelMap.Controls.Count / colCnt;
                int col = panelMap.Controls.Count % colCnt;

                DACrux.TEST.ENGUI.WrapMap map = new DACrux.TEST.ENGUI.WrapMap();
                map.OnSelected += new DACrux.TEST.ENGUI.WrapMap.EventHandlerSelected(map_OnSelected);
                map.OnDblClick += new DACrux.TEST.ENGUI.WrapMap.EventHandlerSelected(map_OnDblClick);
                map.MapId = panelMap.Controls.Count;
                map.Size = new Size(size, size);
                map.Location = new Point(size * col, size * row);
                panelMap.Controls.Add(map);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            threadStop = true;
            Button button = (Button)sender;
            button.Enabled = false;
        }

        void DrawWaferMapThread()
        {
            while (!endCreateMap)
            {
                System.Threading.Thread.Sleep(100);
            }

            try
            {
                buttonStop.Enabled = true;
                labelStatus.Text = "맵을 그립니다.";
                ultraProgressBar.Value = 0;
                ultraProgressBar.Maximum = panelMap.Controls.Count;

                DACrux.TEST.ENGUI.WrapMap map = null;
                for (int i = 0; i < panelMap.Controls.Count; i++)
                {
                    map = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i];
                    DrawWaferMap(map, m_WaferSeqs[map.MapId]);

                    ultraProgressBar.Value = i + 1;
                    if (threadStop) break;
                }

                threadStop = false;
                endDrawMap = true;
            }
            catch (Exception ex) { }
        }

        void DrawWaferMap(DACrux.TEST.ENGUI.WrapMap map, long WaferSeqs)
        {
            DACrux.TEST.RO.ProbeMapAnalysis oPRBMapAnalysis = null;
            ComConfiguration oComConfig = null;
            DataSet ds = null;
            DACrux.Map.WaferMap wMap = map.MapObj;
            try
            {
                oPRBMapAnalysis = new RO.ProbeMapAnalysis();
                ds = oPRBMapAnalysis.SelectWaferMapDrawData(WaferSeqs);
                if (ds == null) return;

                map.TestWaferSeq = WaferSeqs.ToString();

                Color tmpColor = Color.White;
                for (int i = 0; i < ds.Tables["BINSUM"].Rows.Count; i++)
                {
                    if (!ds.Tables["BINSUM"].Rows[i]["COLOR"].ToString().Equals("#FFFFFF"))
                    {
                        tmpColor = ColorTranslator.FromHtml(ds.Tables["BINSUM"].Rows[i]["COLOR"].ToString());
                        wMap.SetColor(DACrux.Base.Convert.intParse(ds.Tables["BINSUM"].Rows[i]["BIN"].ToString()), tmpColor);
                        //m_wcZonal.SetColor(DACrux.Base.Convert.intParse(ds.Tables[0].Rows[i]["BIN"].ToString()), chart1.Series[0].Points[i].Color);
                    }
                }

                wMap.WaferSize = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["WAFER_SIZE"].ToString());
                wMap.DieSizeX = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_X"].ToString());
                wMap.DieSizeY = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["CHIP_SIZE_Y"].ToString());
                wMap.OriginIndexX = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_X"].ToString());
                wMap.OriginIndexY = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_INDEX_Y"].ToString());
                wMap.OriginX = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_X"].ToString());
                wMap.OriginY = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["ORIGIN_MICRO_Y"].ToString());
                wMap.FirstDieX = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_X"].ToString());
                wMap.FirstDieY = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["FIRST_INDEX_Y"].ToString());
                wMap.NotchAngle = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["ANGLE"].ToString());
                wMap.EdgeSize = DACrux.Base.Convert.doubleParse(ds.Tables["RECIPE"].Rows[0]["EDGE_SIZE"].ToString());
                wMap.NotchType = DACrux.Base.Notch.Notch; //ds.Tables["RECIPE"].Rows[0]["NOTCH_TYPE"].ToString().Equals("0") ? DACrux.Base.Notch.Flat : DACrux.Base.Notch.Notch;

                int iXYDir = DACrux.Base.Convert.intParse(ds.Tables["RECIPE"].Rows[0]["XY_DIRECTION"].ToString());
                switch (iXYDir)
                {
                    case 0:
                        wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
                        break;
                    case 1:
                        wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
                        break;
                    case 2:
                        wMap.XYDirect = DACrux.Base.XYDirection.RightBottom;
                        break;
                    case 3:
                        wMap.XYDirect = DACrux.Base.XYDirection.RightTop;
                        break;
                }

                wMap.DataSource = ds.Tables["MAPDATA"];

                /// Map Info Set=================================================================
                /// 
                string[] strInfo = null;
                if (isFromMapView)
                {
                    strInfo = new string[1];
                    strInfo[0] = string.Format("WAFER_ID:{0}",  ds.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString());
                }
                else
                {
                    strInfo = new string[5];
                    strInfo[0] = string.Format("DEVICE  :{0}", ds.Tables["WAFER_INFO"].Rows[0]["PRODUCT"].ToString());
                    strInfo[1] = string.Format("PROGRAM :{0}", ds.Tables["WAFER_INFO"].Rows[0]["PROGRAM"].ToString());
                    strInfo[2] = string.Format("WAFER_ID:{0}", ds.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString());
                    //strInfo[3] = string.Format("YIELD   :{0:0.0#} %", ds.Tables["WAFER_INFO"].Rows[0]["YIELD"].ToString());

                    float fTemp = 0;
                    if (float.TryParse(ds.Tables["WAFER_INFO"].Rows[0]["YIELD"].ToString(), out fTemp) == false)
                        fTemp = 0;

                    strInfo[3] = string.Format("YIELD   :{0:#.#0} %", fTemp);
                    strInfo[4] = string.Format("TESTER  :{0}", ds.Tables["WAFER_INFO"].Rows[0]["TESTER"].ToString());
                }

                if (ds.Tables.Contains("WAFER_INFO") && ds.Tables["WAFER_INFO"].Rows.Count > 0 && ds.Tables["WAFER_INFO"].Columns.Contains("WAFER_ID"))
                {
                    wMap.WaferID = ds.Tables["WAFER_INFO"].Rows[0]["WAFER_ID"].ToString();
                    map.TestWaferID = wMap.WaferID;
                }

                wMap.SetInfomation(strInfo);

                //=================================================================================================================================
                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
                //=================================================================================================================================
                oComConfig = new ComConfiguration();
                DataTable dtInfo = oComConfig.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "TEST_OPTION_M", DACrux.Base.GlobalVariable.UserID);
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    List<string> sWaferInfo = new List<string>();

                    foreach (DataRow drInfo in dtInfo.Rows)
                    {
                        if (ds.Tables["WAFER_INFO"].Columns.IndexOf(drInfo["NAME"].ToString()) > -1)
                            sWaferInfo.Add(string.Format("{0}  :{1}", drInfo["VALUE"], ds.Tables["WAFER_INFO"].Rows[0][drInfo["NAME"].ToString()].ToString()));
                    }

                    wMap.SetInfomation(sWaferInfo.ToArray());
                }

                //Wafer Information 사용 여부
                dtInfo = oComConfig.GetConfigurationUser(DACrux.Base.GlobalVariable.Factory, "WAFER_OPTION_M_ENABLE", DACrux.Base.GlobalVariable.UserID);
                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    if (dtInfo.Rows[0]["VALUE"].ToString() == "N")
                        wMap.SetInfomation(null);
                }

                //사용자 별로 정의된 색상 및 Wafer 정보를 출력 한다.
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
                            case "WAFER_TEST_MAP_BG":
                                wMap.WaferColor = crType;
                                break;
                            //Wafer Border Line Color
                            case "WAFER_TEST_MAP_LINE":
                                wMap.DieBorderColor = crType;
                                break;

                        }
                    }
                }

                wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
                wMap.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ControlEnableSingle(bool en)
        {
            buttonFirst.Enabled = en;
            buttonLeft.Enabled = en;
            buttonRight.Enabled = en;
            buttonLast.Enabled = en;
        }

        private void map_OnDblClick(DACrux.TEST.ENGUI.WrapMap map)
        {
            if (map.Parent.Name.Equals("panelMap"))
            {
                this.Cursor = Cursors.WaitCursor;
                DACrux.TEST.ENGUI.WrapMap tMap = (DACrux.TEST.ENGUI.WrapMap)panelSingle.Controls[0];

                if (chMapView.Checked)
                {
                    //Single Map View 로 조회 한다.
                    if (string.IsNullOrEmpty(map.TestWaferSeq) == false)
                    {
                        DACrux.TEST.ENGUI.frmSingleMapView oTestMapView = new frmSingleMapView(map.TestWaferSeq);
                        oTestMapView.Name = string.Format("{0} ({1})", oTestMapView.Name, map.TestWaferID);
                        oTestMapView.MdiParent = this.ParentForm.ParentForm;
                        oTestMapView.Dock = DockStyle.Fill;
                        oTestMapView.Show();
                    }
                }
                else
                {
                    map.MapObj.Copy(tMap.MapObj);

                    tMap.MapId = map.MapId;
                    panelSingle.BringToFront();
                    this.Cursor = Cursors.Default;
                    ControlEnableSingle(true);
                }
            }
            else if (map.Parent.Name.Equals("panelSingle"))
            {
                panelMap.BringToFront();
                //ControlEnableSingle(false);
            }
        }

        void SingleMapDraw(int idx)
        {
            this.Cursor = Cursors.WaitCursor;
            DACrux.TEST.ENGUI.WrapMap m = (DACrux.TEST.ENGUI.WrapMap)panelSingle.Controls[0];
            DACrux.TEST.ENGUI.WrapMap sMap = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[idx];
            sMap.MapObj.Copy(m.MapObj);
            m.MapId = idx;
            this.Cursor = Cursors.Default;
        }

        private void buttonLeft_Click(object sender, System.EventArgs e)
        {
            DACrux.TEST.ENGUI.WrapMap m = (DACrux.TEST.ENGUI.WrapMap)panelSingle.Controls[0];
            int idx = m.MapId;
            if (idx == 0) return;
            idx--;
            SingleMapDraw(idx);
        }

        private void buttonRight_Click(object sender, System.EventArgs e)
        {
            DACrux.TEST.ENGUI.WrapMap m = (DACrux.TEST.ENGUI.WrapMap)panelSingle.Controls[0];
            int idx = m.MapId;
            if (idx == panelMap.Controls.Count - 1) return;
            idx++;
            SingleMapDraw(idx);
        }

        private void buttonFirst_Click(object sender, System.EventArgs e)
        {
            DACrux.TEST.ENGUI.WrapMap m = (DACrux.TEST.ENGUI.WrapMap)panelSingle.Controls[0];
            if (m.MapId == 0) return;
            SingleMapDraw(0);
        }

        private void buttonLast_Click(object sender, System.EventArgs e)
        {
            DACrux.TEST.ENGUI.WrapMap m = (DACrux.TEST.ENGUI.WrapMap)panelSingle.Controls[0];
            if (m.MapId == panelMap.Controls.Count - 1) return;
            SingleMapDraw(panelMap.Controls.Count - 1);
        }

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            if (OnClose != null) OnClose();
        }

        public string[] SaveTempBmp()
        {
            ArrayList arrMapFile = new ArrayList();
            Bitmap bmpWaferMap = null;
            string tmpFile = string.Format(@"{0}\tmpMap.bmp", Application.StartupPath);
            DACrux.TEST.ENGUI.WrapMap map = null;

            for (int i = 0; i < panelMap.Controls.Count; i++)
            {
                map = (DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i];
                tmpFile = string.Format(@"{0}\tmpMap_{1}.bmp", Application.StartupPath, i);
                bmpWaferMap = map.MapObj.GetMapImage();
                bmpWaferMap.Save(tmpFile);
                arrMapFile.Add(tmpFile);
            }

            return (string[])arrMapFile.ToArray(typeof(string));
        }

        #region Excel Export

        public void ExportExcel()
        {
            DACrux.Utility.ExcelExportArgs e = new DACrux.Utility.ExcelExportArgs();
            DACrux.Utility.ExcelSheet sheet = new DACrux.Utility.ExcelSheet();

            ////int colCnt = DACrux.Base.Convert.intParse(comboBoxCol.Text);
            //List<Image> lsImages = new List<Image>();
            //Bitmap bmp = null;
            //Rectangle rect;
            //for (int i = 0; i < panelMap.Controls.Count; i++)
            //{
            //    rect = ((DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i]).DisplayRectangle;
            //    bmp = new Bitmap(rect.Width, rect.Height);
            //    ((DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i]).DrawToBitmap(bmp, new Rectangle(0, 0, rect.Width, rect.Height));

            //    lsImages.Add(bmp);
            //}

            //if (lsImages.Count <= 0)
            //    return;

            //sheet.Add(lsImages.ToArray());

            for (int i = 0; i < panelMap.Controls.Count; i++)
            {
                sheet.Add(((DACrux.TEST.ENGUI.WrapMap)panelMap.Controls[i]).MapObj);
                sheet.Add();
            }

            e.SheetList.Add(sheet);
            e.SheetList[e.SheetList.Count - 1].SheetName = "Map";

            DACrux.Utility.ExcelExportManager.Export(e);
        }

        #endregion 
    }
}
