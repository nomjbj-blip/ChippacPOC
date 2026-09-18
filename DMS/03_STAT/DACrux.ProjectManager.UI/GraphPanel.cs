using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DACrux.ProjectManager.UI
{
    public delegate void GraphUpdatingHandler(GraphInformation graphInfo);

    public sealed partial class GraphPanel : UserControl
    {
        #region " MEMBER FIELD "
        public bool IsCtrl = false;
        public event ImageClickedHandler ImageClicked;
        
        private GraphInformation graphInfo = new GraphInformation();
        private System.Windows.Forms.DataVisualization.Charting.BorderSkin simpleBorder = new System.Windows.Forms.DataVisualization.Charting.BorderSkin();
        private System.Windows.Forms.DataVisualization.Charting.BorderSkin solidBackground = new System.Windows.Forms.DataVisualization.Charting.BorderSkin();

        bool toolBar = true;


        bool propertybutton = true;
        #endregion

        #region " PROPERTY "

        public GraphInformation GraphInfo
        {
            get { return graphInfo; }
        }

        public bool ShowToolBar
        {
            get { return toolBar; }
            set
            {
                toolBar = value;
                toolStrip.Visible = value;
            }
        }

        public bool showpropertybutton
        {
            get { return propertybutton; }
            set
            {
                propertybutton = value;
                toolSepProperty.Visible = value;
                toolBtnProperty.Visible = value;
            }
        }

        public override System.Windows.Forms.Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                base.Cursor = value;
                SetCursor(this, value);

            }
        }

        
        #endregion

        #region " CREATOR "

        public GraphPanel()
        {
            InitializeComponent();
            simpleBorder.BorderDashStyle = ChartDashStyle.Solid;
            simpleBorder.BorderColor = Color.Gray;
            solidBackground.BackColor = Color.Gray;

            if (!DesignMode)
                InitPanel();             
        }

        #endregion

        #region " METHOD "

        public void InitPanel()
        {
            chart.ChartAreas.Clear();
            chart.Series.Clear();
      
            chart.UseWaitCursor = false;  
         
            toolSepLabelAngle.Visible = true;
            toolSepPointLabel.Visible = true;
            toolSepZoom.Visible = true;
            toolLblAngle.Visible = true;
            toolCboAngle.Visible = true;
            toolBtnPointLabel.Visible = true;
            toolBtnLegendBox.Visible = true;
            toolBtn3D.Visible = true;
            toolBtnZoom.Visible = true;

            spcHistogram.Visible = false;

            spcHistogram.MouseClick += new MouseEventHandler(spcHistogram_MouseClick);     
            chart.MouseClick += new MouseEventHandler(chart_MouseClick);

            chart.Refresh(); 
        }

        public void ChartSet(string chartReion)
        {
            chart.Titles.Clear();
            chart.BackColor = Color.WhiteSmoke;
            chart.ChartAreas[chartReion].BackColor = Color.White;    
            chart.ChartAreas[chartReion].BackGradientStyle = GradientStyle.DiagonalLeft;
            chart.ChartAreas[chartReion].BorderColor = Color.Black;
            chart.ChartAreas[chartReion].BorderDashStyle = ChartDashStyle.Solid;
            chart.ChartAreas[chartReion].BorderWidth = 1;
            chart.ChartAreas[chartReion].AxisX.LabelStyle.Angle = graphInfo.LabelAngle;

            chart.ChartAreas[chartReion].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart.ChartAreas[chartReion].AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.ChartAreas[chartReion].AxisX.LabelStyle.Font = new Font("Vedana", 9, FontStyle.Regular);
            chart.ChartAreas[chartReion].AxisY.LabelStyle.Font = new Font("Vedana", 9, FontStyle.Regular);

            chart.ChartAreas[chartReion].AxisX.TitleFont = new Font("Vedana", 9, FontStyle.Bold);
            chart.ChartAreas[chartReion].AxisY.TitleFont = new Font("Vedana", 9, FontStyle.Bold);

            
            chart.ChartAreas[chartReion].Area3DStyle.Enable3D = graphInfo.View3D;
            //chart.ChartAreas[chartReion].Area3DStyle.IsRightAngleAxes = false;
            //chart.ChartAreas[chartReion].Area3DStyle.Inclination = 10;
            //chart.ChartAreas[chartReion].Area3DStyle.Rotation = 20;
            //chart.ChartAreas[chartReion].Area3DStyle.LightStyle = LightStyle.Realistic;
            //chart.ChartAreas[chartReion].Area3DStyle.Perspective = 20;
            //chart.ChartAreas[chartReion].Area3DStyle.WallWidth = 1;
            //chart.ChartAreas[chartReion].Area3DStyle.IsClustered = true;

            //chart.ChartAreas[chartReion].AxisX.Interval = 1; //Default Is Auto

           
            chart.Titles.Add(graphInfo.Name);
            chart.Titles[0].Font = new Font("Vedana", 11, FontStyle.Bold);

           
         }
       
        void chart_MouseClick(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control || ModifierKeys == Keys.ControlKey)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (graphInfo.ImagePath.Trim() != string.Empty && ImageClicked != null)
                        ImageClicked(graphInfo.ImagePath);
                }
            }
        }
       
        void spcHistogram_MouseClick(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control || ModifierKeys == Keys.ControlKey)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (graphInfo.ImagePath.Trim() != string.Empty && ImageClicked != null)
                        ImageClicked(graphInfo.ImagePath);
                }
            }
        }
            
        public void DrawGraph(GraphInformation graphInfomation)
        {
            InitPanel();

            this.graphInfo = graphInfomation;

            try
            {
                Cursor = Cursors.WaitCursor;
                chart.Visible = false;
                if (graphInfomation.Type == GraphType.Line4Taguchi)
                {
                    DataBind4Taguchi(graphInfo);
                    SaveCaptureImage(graphInfo);
                    return;
                }

                if (graphInfo == null || graphInfo.DataSource == null || graphInfo.DataSource.Columns.Count < 1)
                   return;

                #region [ Histogram ]

                if (graphInfo.Type == GraphType.Histogram)
                {
                    DrawHistogram(graphInfo);
                    //return;
                }
                #endregion
                else            
                {
                    #region [ Data Binding "Line, Pie, Scatter, Pareto,BoxPlot"]

                    switch (graphInfo.Type)
                    {
                        case GraphType.Bar:
                        case GraphType.Line:
                            DataBindLineBar(graphInfo);
                            break;
                        case GraphType.Point:
                            DataBindPoint(graphInfo);
                            break;
                        case GraphType.Pie:
                            DataBindPie(graphInfo);
                            break;
                        case GraphType.Scatter:
                            DataBindScatter(graphInfo);
                            break;
                        case GraphType.Pareto:
                            DataBindPareto(graphInfo);
                            break;
                        case GraphType.BoxPlot:
                            //데이터 테이블에 컬럼의 길이가 다를 때
                            for (int i = 0; i < graphInfo.DataSource.Rows.Count; i++)
                            {
                                //foreach (DataColumn column in graphInfo.DataSource.Columns)
                                //{
                                //    if (graphInfo.DataSource.Rows[i].IsNull(column) == true)
                                //    {
                                //        int nullValue = Convert.ToInt32(graphInfo.DataSource.Rows[i - 1][column]);
                                //        graphInfo.DataSource.Rows[i][column] = nullValue;
                                //    }
                                //}
                            }
                            if (graphInfo.Name.IndexOf("ANOVA") > -1)
                            {
                                DrawBoxPlot_org(graphInfo);
                            }
                            else
                            {
                                DrawBoxPlot(graphInfo);
                            }
                            break;
                        default:
                            DataBindLineBar(graphInfo);
                            break;
                    }
                    #endregion

                    #region [ Apply Graph Setting ]

                    if (graphInfo.AxisXTitle != null && graphInfo.AxisXTitle != string.Empty)
                        chart.ChartAreas[0].AxisX.Title = graphInfo.AxisXTitle;

                    if (graphInfo.AxisYTitle != null && graphInfo.AxisYTitle != string.Empty)
                        chart.ChartAreas[0].AxisY.Title = graphInfo.AxisYTitle;


                    #endregion

                    #region [ Apply Toolbar Setting ]

                    toolCboAngle.ComboBox.SelectedItem = graphInfo.LabelAngleString ;
                    toolBtnPointLabel.CheckState = (graphInfo.PointLabel) ? CheckState.Checked : CheckState.Unchecked;
                    toolBtnLegendBox.CheckState = (graphInfo.LegendBox) ? CheckState.Checked : CheckState.Unchecked;
                    toolBtn3D.CheckState = (graphInfo.View3D) ? CheckState.Checked : CheckState.Unchecked;

                    toolSepProperty.Visible = toolBtnProperty.Visible = graphInfo.ViewProperty;
                    toolSepProperty.Visible = toolBtnProperty.Visible = (graphInfo.CreatedInformation.Equals("Graph Analysis")) ? true : false;

                    if (graphInfo.Type == GraphType.Pie)
                    {
                        toolSepLabelAngle.Visible = false;
                        toolLblAngle.Visible = false;
                        toolCboAngle.Visible = false;
                        toolBtnZoom.Visible = false;
                    }

                    if (graphInfo.Type == GraphType.BoxPlot)
                    {               
                         toolBtn3D.Visible = false;                    
                    }
                    #endregion
                
                }
                
                #region [ Save Capture Image && Titles ]

                if (graphInfo.IsImageWithTitles)
                {
                    SetTitles(graphInfo);
                    SaveCaptureImage(graphInfo);
                }
                else
                {
                    SaveCaptureImage(graphInfo);
                    SetTitles(graphInfo);
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                chart.Visible = true;
                Cursor = Cursors.Default;
            }
        }

        private bool DataBind4Taguchi(GraphInformation graphInfomation)
        {
            int iStartPosX = 5;
            int iStartPosY = 10;
            int iAreaWidth = 0;
            int iAreaHeight = 0;
            double dMax = double.MinValue;
            double dMin = double.MaxValue;
            double dSum = 0d;
            int iCnt = 0;
            try
            {
                this.graphInfo = graphInfomation;
                chart.Titles.Clear();
                chart.ChartAreas.Clear();
                iAreaWidth = (100 - iStartPosX * 2) / graphInfomation.ColumnInfoItems.Count;
                iAreaHeight = 100 - iStartPosY * 2;

                chart.Legends[0].Enabled = false;
                bool IsCanDraw = true;

                for (int i = 0; i < graphInfo.ColumnInfoItems.Count; i++)
                {
                    chart.ChartAreas.Add(graphInfo.ColumnInfoItems[i].ColumnName);
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY.LabelStyle.IsEndLabelVisible = false;

                    // Set the chart area position for the first chart area.
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].Position.X = iStartPosX + (i * iAreaWidth);
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].Position.Y = iStartPosY;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].Position.Width = iAreaWidth;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].Position.Height = iAreaHeight;

                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].InnerPlotPosition.Auto = false;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].InnerPlotPosition.X = 0;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].InnerPlotPosition.Y = 0;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].InnerPlotPosition.Width = 100;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].InnerPlotPosition.Height = 95;

                    chart.Series.Add(graphInfo.ColumnInfoItems[i].ColumnName);
                    chart.Series[graphInfo.ColumnInfoItems[i].ColumnName].ChartArea = graphInfo.ColumnInfoItems[i].ColumnName;

                    chart.Series[graphInfo.ColumnInfoItems[i].ColumnName].ChartType = SeriesChartType.Line;
                    chart.Series[graphInfo.ColumnInfoItems[i].ColumnName].MarkerStyle = MarkerStyle.Diamond;
                    chart.Series[graphInfo.ColumnInfoItems[i].ColumnName].MarkerSize = 5;
                    chart.Series[graphInfo.ColumnInfoItems[i].ColumnName].IsXValueIndexed = true;

                    string strTableName = string.Format("ANALYSIS_{0}", graphInfo.ColumnInfoItems[i].ColumnName);
                    for (int j = 0; j < graphInfo.DataSource4Taguchi.Tables[strTableName].Rows.Count; j++)
                    {
                        float xValue = float.Parse(graphInfo.DataSource4Taguchi.Tables[strTableName].Rows[j][graphInfo.ColumnInfoItems[i].ColumnName].ToString());
                        float yValue = float.Parse(graphInfo.DataSource4Taguchi.Tables[strTableName].Rows[j][graphInfo.SubTitle].ToString());
                        chart.Series[graphInfomation.ColumnInfoItems[i].ColumnName].Points.AddXY(xValue, yValue);

                        if (dMax < yValue) dMax = yValue;
                        if (dMin > yValue) dMin = yValue;

                        dSum += yValue;
                        iCnt++;
                    }

                    if (dMax == double.MinValue || dMin == double.MaxValue)
                    {
                        IsCanDraw = false;
                        break;
                    }
                    chart.Series[graphInfo.ColumnInfoItems[i].ColumnName].IsValueShownAsLabel = true;
                    chart.Series[graphInfo.ColumnInfoItems[i].ColumnName].LabelFormat = "0.00#";

                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].BorderDashStyle = ChartDashStyle.Solid;

                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX.MajorGrid.Enabled = false;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX.MinorGrid.Enabled = false;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX.MajorTickMark.Enabled = true;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX.MinorTickMark.Enabled = false;

                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY.MajorGrid.Enabled = false;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY.MinorGrid.Enabled = false;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY.MajorTickMark.Enabled = false;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY.MinorTickMark.Enabled = false;

                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX2.MajorGrid.Enabled = false;
                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX2.MinorGrid.Enabled = false;
                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX2.MajorTickMark.Enabled = false;
                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX2.MinorTickMark.Enabled = false;

                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY2.MajorGrid.Enabled = false;
                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY2.MinorGrid.Enabled = false;
                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY2.MajorTickMark.Enabled = false;
                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY2.MinorTickMark.Enabled = false;

                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX.Enabled = AxisEnabled.True;
                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX2.Enabled = AxisEnabled.False;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY.Enabled = AxisEnabled.False;
                    //chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisY2.Enabled = AxisEnabled.False;

                    // Set the alignment type
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AlignWithChartArea = graphInfo.ColumnInfoItems[0].ColumnName; //첫번째것으로 통일
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AlignmentStyle = AreaAlignmentStyles.All;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX.IsMarginVisible = true;
                    chart.ChartAreas[graphInfo.ColumnInfoItems[i].ColumnName].AxisX.IsStartedFromZero = false;
                    // Set Title
                    System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
                    title1.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
                    title1.ForeColor = Color.Black;
                    title1.Name = strTableName;
                    title1.Position.Auto = false;
                    title1.Position.Height = 5;
                    title1.Position.Width = iAreaWidth;
                    title1.Position.X = iStartPosX + (i * iAreaWidth);
                    title1.Position.Y = iStartPosY;
                    title1.Text = graphInfo.ColumnInfoItems[i].ColumnName;
                    chart.Titles.Add(title1);

                }

                if (dMax == dMin)
                {
                    dMax = dMax + dMax * 0.2;
                    dMin = dMin - dMin * 0.2;
                }

                if (IsCanDraw)
                {
                    dAvg = dSum / iCnt;

                    chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = true;
                    chart.ChartAreas[0].AxisY.Title = graphInfo.AxisYTitle;
                    chart.ChartAreas[0].AxisY2.MajorTickMark.Enabled = false;

                    if (dMax > dMin)
                    {
                        StripLine stripLine = new StripLine();
                        stripLine.BorderDashStyle = ChartDashStyle.Solid;
                        stripLine.BorderWidth = 2;
                        stripLine.IntervalOffset = dAvg;
                        for (int i = 0; i < chart.ChartAreas.Count; i++)
                        {
                            chart.ChartAreas[i].AxisY.Maximum = Math.Round(dMax + (dMax - dMin) * 0.2, 2);
                            chart.ChartAreas[i].AxisY.Minimum = Math.Round(dMin - (dMax - dMin) * 0.2, 2);
                            chart.ChartAreas[i].AxisY.StripLines.Add(stripLine);
                        }
                    }
                    chart.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].InnerPlotPosition.X = 15;
                    chart.ChartAreas[0].InnerPlotPosition.Y = 0;
                    chart.ChartAreas[0].InnerPlotPosition.Width = 85;
                    chart.ChartAreas[0].InnerPlotPosition.Height = 95;

                    //chart.ChartAreas[chart.ChartAreas.Count-1].AxisY2.Enabled = AxisEnabled.True;

                    chart.Titles.Add(new Title(graphInfo.Title, Docking.Top));
                }
                return IsCanDraw; // Graph 를 Drawing할 수 없을때= false, 결측치
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        double dAvg = 0;

        private void chart_PostPaint(object sender, ChartPaintEventArgs e)
        {
            try
            {
                if (graphInfo.Type == GraphType.BoxPlot && graphInfo.Name.IndexOf("ANOVA") < 0)
                {
                    if (e.ChartElement is Series)
                    {
                        Series series = (Series)e.ChartElement;
                        System.Drawing.PointF fLineMedian_Start = System.Drawing.PointF.Empty;
                        System.Drawing.PointF fLineMedian_End = System.Drawing.PointF.Empty;
                        System.Drawing.PointF fLineMean_Start = System.Drawing.PointF.Empty;
                        System.Drawing.PointF fLineMean_End = System.Drawing.PointF.Empty;

                        fLineMedian_Start = new PointF(70, 15);
                        fLineMedian_End = new PointF(80, 15);
                        fLineMean_Start = new PointF(70, 20);
                        fLineMean_End = new PointF(80, 20);

                        fLineMedian_Start = e.ChartGraphics.GetAbsolutePoint(fLineMedian_Start);
                        fLineMedian_End = e.ChartGraphics.GetAbsolutePoint(fLineMedian_End);
                        fLineMean_Start = e.ChartGraphics.GetAbsolutePoint(fLineMean_Start);
                        fLineMean_End = e.ChartGraphics.GetAbsolutePoint(fLineMean_End);


                        Pen p_Median = new Pen(Color.Black);
                        p_Median.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        p_Median.DashPattern = new float[] { 10f, 10f };

                        e.ChartGraphics.Graphics.DrawLine(p_Median, fLineMedian_Start, fLineMedian_End);
                        e.ChartGraphics.Graphics.DrawLine(new Pen(Color.Black), fLineMean_Start, fLineMean_End);

                        fLineMedian_End.Y = fLineMedian_End.Y - 8;
                        fLineMean_End.Y = fLineMean_End.Y - 8;

                        e.ChartGraphics.Graphics.DrawString("Median", this.Font, new SolidBrush(Color.Blue), fLineMedian_End);
                        e.ChartGraphics.Graphics.DrawString("Mean", this.Font, new SolidBrush(Color.Blue), fLineMean_End);
                    }
                }


                if (double.IsNaN(dAvg)) return; 
                if (graphInfo.Type == GraphType.Line4Taguchi)
                    DrawMarkLine(e, dAvg);
            }
            catch
            {
                MessageBox.Show("결측치입니다");
            }
        }

        public void DrawMarkLine(ChartPaintEventArgs e, double dAverage)
        {
            try
            {
                if (e.ChartElement is Series)
                {
                    Series series = (Series)e.ChartElement;
                    float avg_y = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.Y, dAverage);
                    float avg_x = (float)e.ChartGraphics.GetPositionFromAxis(series.ChartArea, AxisName.X, 0);
                    //float avg_x = chart.ChartAreas[series.ChartArea]..InnerPlotPosition.X;
                    float avg_x2 = chart.ChartAreas[series.ChartArea].Position.X + chart.ChartAreas[series.ChartArea].Position.Width;

                    PointF pos_1st = e.ChartGraphics.GetAbsolutePoint(new PointF(avg_x, avg_y));
                    PointF pos_2nd = e.ChartGraphics.GetAbsolutePoint(new PointF(avg_x2, avg_y));

                    e.ChartGraphics.Graphics.DrawLine(System.Drawing.Pens.Red, pos_1st, pos_2nd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void DataBindPoint(GraphInformation graphInfo)
        {

            string strXColName = string.Empty;
            string strLabel = string.Empty;

            List<string> lstLabel = new List<string>();
            List<double[]> lstValue = new List<double[]>();
            List<int> lstCount = new List<int>();

            DataTable dtTemp = graphInfo.DataSource.Copy();
            DataRow[] rows = null;
            DataRow tempRow = null;

            System.Data.DataView dv;


            int totCount = dtTemp.Rows.Count;
            double[] arrSumValue;

            object temp;
            double tempDouble = 0;

            try
            {
                if (graphInfo.AxisX.ColumnName != null && graphInfo.AxisX.ColumnName.Length > 0)
                {
                    strXColName = dtTemp.Columns[graphInfo.AxisX.ColumnIndex].ColumnName;
                }

                if (graphInfo.AxisX.ColumnType == typeof(string))
                {
                    #region [ String ]
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);
                        arrSumValue = new double[graphInfo.AxisY.Length];

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = '" + dtTemp.Rows[0][strXColName] + "'");
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        foreach (DataRow row in rows)
                        {

                            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                            {
                                temp = row[graphInfo.AxisY[i].ColumnIndex];

                                if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                    arrSumValue[i] += tempDouble;
                            }

                            row.Delete();
                        }

                        lstLabel.Add(strLabel);
                        lstValue.Add(arrSumValue);

                        dtTemp.AcceptChanges();
                    }
                    #endregion
                }
                else if (graphInfo.AxisX.ColumnType == typeof(DateTime))
                {
                    #region [ DateTime ]
                    DateTime dTemp;
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);

                        arrSumValue = new double[graphInfo.AxisY.Length];

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = '" + dtTemp.Rows[0][strXColName] + "'");
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        if (DateTime.TryParse(strLabel, out dTemp))
                            strLabel = dTemp.ToString(graphInfo.DateTimeFormatString);
                        //if (strLabel.Contains(" "))
                        //{
                        //    string[] splitPoint = strLabel.Split(' ');
                        //    strLabel = splitPoint[0].Trim();
                        //}
                        else
                            strLabel = string.Empty;

                        foreach (DataRow row in rows)
                        {
                            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                            {
                                temp = row[graphInfo.AxisY[i].ColumnIndex];

                                if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                    arrSumValue[i] += tempDouble;
                            }

                            row.Delete();
                        }

                        lstLabel.Add(strLabel);
                        lstValue.Add(arrSumValue);

                        dtTemp.AcceptChanges();
                    }
                    #endregion
                }
                else if (graphInfo.AxisX.ColumnType == null)
                {
                    for (int j = 0; j < graphInfo.AxisY.Length; j++)
                    {
                        arrSumValue = new double[totCount];
                        for (int i = 0; i < totCount; i++)
                        {
                            if (lstLabel.IndexOf(i.ToString()) < 0) lstLabel.Add(i.ToString());
                            temp = dtTemp.Rows[i][graphInfo.AxisY[j].ColumnIndex];

                            if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                arrSumValue[i] = tempDouble;
                        }
                        lstValue.Add(arrSumValue);
                    }
                }
                else
                {
                    #region [ Number ]
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);
                        arrSumValue = new double[graphInfo.AxisY.Length];

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "]" + " = " + dtTemp.Rows[0][strXColName]);
                        else
                            rows = dtTemp.Select("[" + strXColName + "]" + " is null");

                        foreach (DataRow row in rows)
                        {
                            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                            {
                                temp = row[graphInfo.AxisY[i].ColumnIndex];

                                if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                    arrSumValue[i] += tempDouble;
                            }

                            row.Delete();
                        }

                        lstLabel.Add(strLabel);
                        lstValue.Add(arrSumValue);

                        dtTemp.AcceptChanges();
                    }
                    #endregion
                }

                dtTemp = new DataTable();
                dtTemp.Columns.Add("Label", typeof(string));
                for (int i = 0; i < graphInfo.AxisY.Length; i++)
                {
                    dtTemp.Columns.Add(graphInfo.AxisY[i].ColumnName, graphInfo.AxisY[i].ColumnType);
                }

                if (graphInfo.AxisX.ColumnType != null)
                {
                    for (int i = 0; i < lstLabel.Count; i++)
                    {
                        tempRow = dtTemp.NewRow();
                        tempRow["Label"] = lstLabel[i];
                        for (int j = 0; j < graphInfo.AxisY.Length; j++)
                        {
                            tempRow[j + 1] = lstValue[i][j];
                        }
                        dtTemp.Rows.Add(tempRow);
                    }
                }


                dv = dtTemp.DefaultView;
                dv.Sort = "Label ASC";
                dtTemp = dv.ToTable();

              
                chart.ChartAreas.Clear();
                chart.Legends.Clear();
                chart.ChartAreas.Add("Point");
                ChartSet("Point");
                object maxVal;
                object minVal;
                object stdVal;

                double dMax = double.MinValue;
                double dMin = double.MaxValue;
                for (int j = 0; j < graphInfo.AxisY.Length; j++)
                {
                    string strSeriesName = (graphInfo.AxisY[j].ColumnName == string.Empty) ? graphInfo.AxisY[j].ColumnID : graphInfo.AxisY[j].ColumnName;
                    double[] dValues = new double[lstLabel.Count];

                    for (int i = 0; i < lstLabel.Count; i++)
                    {
                        dValues[i] = Convert.ToDouble(lstValue[j][i]);
                    }
                    chart.Series.Add(strSeriesName);
                    chart.Legends.Add(strSeriesName);
                    chart.Series[strSeriesName].Points.DataBindY(dValues);
                    chart.Series[strSeriesName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart.Series[strSeriesName].BorderWidth = 2;
                    chart.Series[strSeriesName].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[strSeriesName].MarkerSize = graphInfo.PointSize;
                    chart.Series[strSeriesName].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft;
                    chart.Series[strSeriesName].IsValueShownAsLabel = graphInfo.PointLabel;
                    chart.Series[strSeriesName].Font = new Font("Vedana", 8, FontStyle.Regular);
                    chart.Series[strSeriesName].IsVisibleInLegend = graphInfo.LegendBox;
                    chart.Series[strSeriesName].LabelFormat = "N" + graphInfo.DecimalPlace;

                    maxVal = graphInfo.DataSource.Compute("Max([" + strSeriesName + "])", "");
                    minVal = graphInfo.DataSource.Compute("Min([" + strSeriesName + "])", "");
                    stdVal = graphInfo.DataSource.Compute("StDev([" + strSeriesName + "])", "");

                    if (dMax < Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2))
                        dMax = Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2);
                    if (dMin > Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2))
                        dMin = Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2);

                }
                chart.ChartAreas["Point"].AxisY.Maximum = dMax;
                chart.ChartAreas["Point"].AxisY.Minimum = dMin;
                chart.ChartAreas["Point"].AxisX.IsMarginVisible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        private void DrawBoxPlot(GraphInformation graphInfo)
        {
             //( Q1-1.5 x IQR, Q3 + 1.5 x IQR ) , IQR=Q3-Q1 적용??
            double nTemp = 1.0;
            double[] nTempsY = new double[3];
            double[] nTempsX = new double[3];
            object avgVal;
            object maxVal;
            object minVal;
            object stdVal;

            double dMax = double.MinValue;
            double dMin = double.MaxValue;

            GraphInformation.ColumnInfoItem[] arrAxisYItem = graphInfo.AxisY;
            chart.ChartAreas.Clear();
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.ChartAreas.Add("BoxPlot");
            ChartSet("BoxPlot");
            chart.Series.Add("BoxPlotSeries");
            //chart.ChartAreas["BoxPlot"].AxisX.Interval = 1.0; // Default is Auto
            chart.ChartAreas["BoxPlot"].AxisX.Minimum = 0.0;

            try
            {
                string[] strBoxPlotSeries = new string[arrAxisYItem.Length];
                string[] strBoxPlotSeriesPoint = new string[arrAxisYItem.Length];
                string[] staticPoint = new string[arrAxisYItem.Length];

                for (int j = 0; j < arrAxisYItem.Length; j++)
                {
                    strBoxPlotSeries[j] = Convert.ToString(graphInfo.AxisY[j].ColumnIndex);
                    strBoxPlotSeriesPoint[j] = (arrAxisYItem[j].ColumnName == string.Empty) ? arrAxisYItem[j].ColumnID : arrAxisYItem[j].ColumnName;

                    chart.Series.Add(strBoxPlotSeries[j]);
                    chart.Series.Add(strBoxPlotSeriesPoint[j]);

                    staticPoint[j] = (graphInfo.CreatedInformation != string.Empty) ? arrAxisYItem[j].ColumnID : arrAxisYItem[j].ColumnName;

                    if (graphInfo.Name.Contains("DescriptiveAnalysis"))
                    {
                        staticPoint[j] = arrAxisYItem[j].ColumnName;
                    }
                    maxVal = graphInfo.DataSource.Compute("Max([" + staticPoint[j] + "])", "");
                    minVal = graphInfo.DataSource.Compute("Min([" + staticPoint[j] + "])", "");
                    avgVal = graphInfo.DataSource.Compute("Avg([" + staticPoint[j] + "])", "");
                    stdVal = graphInfo.DataSource.Compute("StDev([" + staticPoint[j] + "])", "");

                    double dUpperLimit = double.NaN;
                    double dLowerLimit = double.NaN;
                    if (double.IsNaN(graphInfo.USL) != true && double.IsNaN(graphInfo.LSL) != true)
                    {
                        dUpperLimit = (double)graphInfo.USL + (double)(graphInfo.USL - graphInfo.LSL) * 1.5;
                        dLowerLimit = (double)graphInfo.LSL - (double)(graphInfo.USL - graphInfo.LSL) * 1.5;
                    }
                    else
                    {
                        double NewCalQ1 = double.NaN;
                        double NewCalQ3 = double.NaN;
                        double[] dArray = new double[graphInfo.DataSource.Rows.Count];

                        System.Data.DataView dv = graphInfo.DataSource.DefaultView;
                        DataTable dt1 = null;
                        if (string.IsNullOrEmpty(arrAxisYItem[j].ColumnID) == true)
                        {
                            dv.Sort = arrAxisYItem[j].ColumnName + " ASC";
                            dt1 = dv.ToTable(false, arrAxisYItem[j].ColumnName);
                        }
                        else
                        {
                            dv.Sort = arrAxisYItem[j].ColumnID + " ASC";
                            dt1 = dv.ToTable(false, arrAxisYItem[j].ColumnID);
                        }

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dArray[i] = double.Parse(dt1.Rows[i][0].ToString());
                        }

                        // Calculation Q1, Q3
                        NewCalQ1 = calc_QUARTILE(dArray, 1);
                        NewCalQ3 = calc_QUARTILE(dArray, 3);

                        dUpperLimit = (double)NewCalQ3 + (double)(NewCalQ3 - NewCalQ1) * 1.5;
                        dLowerLimit = (double)NewCalQ1 - (double)(NewCalQ3 - NewCalQ1) * 1.5;
                    }

                    for (int i = 0; i < graphInfo.DataSource.Rows.Count; i++)
                    {
                        if (double.TryParse(graphInfo.DataSource.Rows[i][arrAxisYItem[j].ColumnIndex].ToString(), out nTemp))
                        {
                            if (nTemp <= dUpperLimit && nTemp >= dLowerLimit)
                            {
                                chart.Series[strBoxPlotSeries[j]].Points.AddXY((j + 1), nTemp);
                            }
                            else
                            {
                                chart.Series[strBoxPlotSeriesPoint[j]].Points.AddXY((j + 1), nTemp);
                            }
                        }
                        else
                        {
                            chart.Series[strBoxPlotSeries[j]].Points.AddY(double.NaN);
                        }
                    }
                    chart.Series[strBoxPlotSeries[j]].ChartType = SeriesChartType.Point;
                    chart.Series[strBoxPlotSeries[j]].MarkerSize = 0;
                    chart.Series[strBoxPlotSeries[j]].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[strBoxPlotSeries[j]].Enabled = true;

                    chart.Series[strBoxPlotSeriesPoint[j]].ChartType = SeriesChartType.Point;
                    chart.Series[strBoxPlotSeriesPoint[j]].MarkerSize = 15;
                    chart.Series[strBoxPlotSeriesPoint[j]].MarkerStyle = MarkerStyle.Cross;
                    chart.Series[strBoxPlotSeriesPoint[j]].Enabled = true;

                    if (graphInfo.PointLabel == true)
                    {
                        chart.Series[strBoxPlotSeriesPoint[j]].IsValueShownAsLabel = graphInfo.PointLabel;
                        chart.Series[strBoxPlotSeriesPoint[j]].LabelFormat = "N" + graphInfo.DecimalPlace;
                        chart.Series[strBoxPlotSeriesPoint[j]].Enabled = true;
                    }

                    if (dMax < Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2))
                        dMax = Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2);
                    if (dMin > Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2))
                        dMin = Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2);
                }

                chart.ChartAreas["BoxPlot"].AxisY.Maximum = dMax;
                chart.ChartAreas["BoxPlot"].AxisY.Minimum = dMin;

                chart.Legends.Add("BoxPlot");

                for (int i = 0; chart.Series.Count > i; i++)
                {
                    chart.Series[i].IsVisibleInLegend = false;
                    chart.Series[i].Font = new Font("Vedana", 7, FontStyle.Regular);
                }


                chart.Series["BoxPlotSeries"].ChartType = SeriesChartType.BoxPlot;
                chart.Series["BoxPlotSeries"]["BoxPlotSeries"] = string.Join(";", strBoxPlotSeries);
                chart.Series["BoxPlotSeries"]["BoxPlotShowMedian"] = "true";
                chart.Series["BoxPlotSeries"]["BoxPlotShowAverage"] = "true";
                chart.Series["BoxPlotSeries"]["BoxPlotShowMedian"] = "true";
                chart.Series["BoxPlotSeries"]["BoxPlotShowUnusualValues"] = "true";
                chart.Series["BoxPlotSeries"].BorderWidth = 2;
                chart.Series["BoxPlotSeries"].IsVisibleInLegend = false;
                chart.Series["BoxPlotSeries"].Font = new Font("Vedana", 8, FontStyle.Regular);

                chart.Series["BoxPlotSeries"]["BoxPlotWhiskerPercentile"] = "0";
                //chart.Series["BoxPlotSeries"]["BoxPlotPercentile"] = "25";
                //chart.Series["BoxPlotSeries"]["MaxPixelPointWidth"] = "10";
                //chart.Series["BoxPlotSeries"]["PixelPointWidth"] = "25";

                for (int i = 1; i < arrAxisYItem.Length + 1; i++)
                {
                    double minXaxes = i - 0.5;
                    double maxXaxes = i + 0.5;
                    string xAxesValue = strBoxPlotSeriesPoint[i - 1];
                    chart.ChartAreas["BoxPlot"].AxisX.CustomLabels.Add(minXaxes, maxXaxes, xAxesValue);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void DrawBoxPlot_old_2014_10_14(GraphInformation graphInfo)
        {
            double nTemp = 1.0;
            double[] nTempsY = new double[3];
            double[] nTempsX = new double[3];
            object avgVal;
            object maxVal;
            object minVal;

            object stdVal;
            double dMax = double.MinValue;
            double dMin = double.MaxValue;

            GraphInformation.ColumnInfoItem[] arrAxisYItem = graphInfo.AxisY;
            chart.ChartAreas.Clear();
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.ChartAreas.Add("BoxPlot");
            ChartSet("BoxPlot");
            chart.Series.Add("BoxPlotSeries");
            //chart.ChartAreas["BoxPlot"].AxisX.Interval = 1.0; // Default is Auto
            chart.ChartAreas["BoxPlot"].AxisX.Minimum = 0.0;

            try
            {
                string[] strBoxPlotSeries = new string[arrAxisYItem.Length];
                string[] strBoxPlotSeriesPoint = new string[arrAxisYItem.Length];
                string[] staticPoint = new string[arrAxisYItem.Length];



                for (int j = 0; j < arrAxisYItem.Length; j++)
                {
                    strBoxPlotSeries[j] = Convert.ToString(graphInfo.AxisY[j].ColumnIndex);
                    strBoxPlotSeriesPoint[j] = (arrAxisYItem[j].ColumnName == string.Empty) ? arrAxisYItem[j].ColumnID : arrAxisYItem[j].ColumnName;

                    chart.Series.Add(strBoxPlotSeries[j]);
                    chart.Series.Add(strBoxPlotSeriesPoint[j]);

                    maxVal = graphInfo.DataSource.Compute("Max([" + staticPoint[j] + "])", "");
                    minVal = graphInfo.DataSource.Compute("Min([" + staticPoint[j] + "])", "");
                    avgVal = graphInfo.DataSource.Compute("Avg([" + staticPoint[j] + "])", "");
                    stdVal = graphInfo.DataSource.Compute("StDev([" + staticPoint[j] + "])", "");

                    for (int i = 0; i < graphInfo.DataSource.Rows.Count; i++)
                    {
                        if (double.TryParse(graphInfo.DataSource.Rows[i][arrAxisYItem[j].ColumnIndex].ToString(), out nTemp))
                        {
                            chart.Series[strBoxPlotSeries[j]].Points.AddY(nTemp);
                            chart.Series[strBoxPlotSeriesPoint[j]].Points.AddXY((j + 1), nTemp);
                        }
                        else
                        {
                            chart.Series[strBoxPlotSeries[j]].Points.AddY(double.NaN);
                        }
                    }
                    chart.Series[strBoxPlotSeries[j]].Enabled = false;

                    //// BoxPlot의 최대, 최소, 평균값
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    nTempsX[i] = j + 1;
                    //}

                    staticPoint[j] = (graphInfo.CreatedInformation != string.Empty) ? arrAxisYItem[j].ColumnID : arrAxisYItem[j].ColumnName;

                    if (graphInfo.Name.Contains("DescriptiveAnalysis"))
                    {
                        staticPoint[j] = arrAxisYItem[j].ColumnName;
                    }



                    //nTempsY[0] = Math.Round(Convert.ToDouble(avgVal), 2);
                    //nTempsY[1] = Math.Round(Convert.ToDouble(maxVal), 2);
                    //nTempsY[2] = Math.Round(Convert.ToDouble(minVal), 2);

                    chart.Series[strBoxPlotSeriesPoint[j]].ChartType = SeriesChartType.Point;
                    chart.Series[strBoxPlotSeriesPoint[j]].MarkerSize = 2;
                    chart.Series[strBoxPlotSeriesPoint[j]].Enabled = false;


                    if (graphInfo.PointLabel == true)
                    {
                        chart.Series[strBoxPlotSeriesPoint[j]].IsValueShownAsLabel = graphInfo.PointLabel;
                        chart.Series[strBoxPlotSeriesPoint[j]].LabelFormat = "N" + graphInfo.DecimalPlace;
                        chart.Series[strBoxPlotSeriesPoint[j]].Enabled = true;
                    }

                    if (dMax < Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2))
                        dMax = Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2);
                    if (dMin > Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2))
                        dMin = Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2);
                }

                chart.ChartAreas["BoxPlot"].AxisY.Maximum = dMax;
                chart.ChartAreas["BoxPlot"].AxisY.Minimum = dMin;

                chart.Legends.Add("BoxPlot");

                for (int i = 0; chart.Series.Count > i; i++)
                {
                    chart.Series[i].IsVisibleInLegend = false;
                    chart.Series[i].Font = new Font("Vedana", 7, FontStyle.Regular);
                }


                chart.Series["BoxPlotSeries"].ChartType = SeriesChartType.BoxPlot;
                chart.Series["BoxPlotSeries"]["BoxPlotSeries"] = string.Join(";", strBoxPlotSeriesPoint);
                chart.Series["BoxPlotSeries"]["BoxPlotShowMedian"] = "true";
                chart.Series["BoxPlotSeries"]["BoxPlotShowAverage"] = "true";
                chart.Series["BoxPlotSeries"]["BoxPlotShowMedian"] = "true";
                chart.Series["BoxPlotSeries"]["BoxPlotShowUnusualValues"] = "true";
                chart.Series["BoxPlotSeries"].BorderWidth = 2;
                chart.Series["BoxPlotSeries"].IsVisibleInLegend = false;
                chart.Series["BoxPlotSeries"].Font = new Font("Vedana", 8, FontStyle.Regular);

                //chart.Series["BoxPlotSeries"]["BoxPlotWhiskerPercentile"] = "2";
                //chart.Series["BoxPlotSeries"]["BoxPlotPercentile"] = "25";
                //chart.Series["BoxPlotSeries"]["MaxPixelPointWidth"] = "10";
                //chart.Series["BoxPlotSeries"]["PixelPointWidth"] = "25";

                for (int i = 1; i < arrAxisYItem.Length + 1; i++)
                {
                    double minXaxes = i - 0.5;
                    double maxXaxes = i + 0.5;
                    string xAxesValue = strBoxPlotSeriesPoint[i - 1];
                    chart.ChartAreas["BoxPlot"].AxisX.CustomLabels.Add(minXaxes, maxXaxes, xAxesValue);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void DrawBoxPlot_org(GraphInformation graphInfo)
        {
            double nTemp = 1.0;
            double[] nTempsY = new double[3];
            double[] nTempsX = new double[3];
            object avgVal;
            object maxVal;
            object minVal;

            object stdVal;
            double dMax = double.MinValue;
            double dMin = double.MaxValue;

            GraphInformation.ColumnInfoItem[] arrAxisYItem = graphInfo.AxisY;
            chart.ChartAreas.Clear();
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.ChartAreas.Add("BoxPlot");
            ChartSet("BoxPlot");
            chart.Series.Add("BoxPlotSeries");
            //chart.ChartAreas["BoxPlot"].AxisX.Interval = 1.0; // Default is Auto
            chart.ChartAreas["BoxPlot"].AxisX.Minimum = 0.0;




            try
            {
                string[] strBoxPlotSeries = new string[arrAxisYItem.Length];
                string[] strBoxPlotSeriesPoint = new string[arrAxisYItem.Length];
                string[] staticPoint = new string[arrAxisYItem.Length];



                for (int j = 0; j < arrAxisYItem.Length; j++)
                {
                    strBoxPlotSeries[j] = Convert.ToString(graphInfo.AxisY[j].ColumnIndex);
                    strBoxPlotSeriesPoint[j] = (arrAxisYItem[j].ColumnName == string.Empty) ? arrAxisYItem[j].ColumnID : arrAxisYItem[j].ColumnName;

                    chart.Series.Add(strBoxPlotSeries[j]);
                    chart.Series.Add(strBoxPlotSeriesPoint[j]);

                    for (int i = 0; i < graphInfo.DataSource.Rows.Count; i++)
                    {
                        if (double.TryParse(graphInfo.DataSource.Rows[i][arrAxisYItem[j].ColumnIndex].ToString(), out nTemp))
                        {
                            chart.Series[strBoxPlotSeries[j]].Points.AddY(nTemp);
                        }
                        else
                        {
                            chart.Series[strBoxPlotSeries[j]].Points.AddY(double.NaN);
                        }
                    }
                    chart.Series[strBoxPlotSeries[j]].Enabled = false;

                    // BoxPlot의 최대, 최소, 평균값
                    for (int i = 0; i < 3; i++)
                    {
                        nTempsX[i] = j + 1;
                    }

                    staticPoint[j] = (graphInfo.CreatedInformation != string.Empty) ? arrAxisYItem[j].ColumnID : arrAxisYItem[j].ColumnName;

                    if (graphInfo.Name.Contains("DescriptiveAnalysis"))
                    {
                        staticPoint[j] = arrAxisYItem[j].ColumnName;
                    }

                    maxVal = graphInfo.DataSource.Compute("Max([" + staticPoint[j] + "])", "");
                    minVal = graphInfo.DataSource.Compute("Min([" + staticPoint[j] + "])", "");
                    avgVal = graphInfo.DataSource.Compute("Avg([" + staticPoint[j] + "])", "");
                    stdVal = graphInfo.DataSource.Compute("StDev([" + staticPoint[j] + "])", "");

                    nTempsY[0] = Math.Round(Convert.ToDouble(avgVal), 2);
                    nTempsY[1] = Math.Round(Convert.ToDouble(maxVal), 2);
                    nTempsY[2] = Math.Round(Convert.ToDouble(minVal), 2);

                    chart.Series[strBoxPlotSeriesPoint[j]].Points.DataBindXY(nTempsX, nTempsY);
                    chart.Series[strBoxPlotSeriesPoint[j]].ChartType = SeriesChartType.Point;
                    chart.Series[strBoxPlotSeriesPoint[j]].MarkerSize = 8;
                    chart.Series[strBoxPlotSeries[j]].Enabled = false;


                    if (graphInfo.PointLabel == true)
                    {
                        chart.Series[strBoxPlotSeriesPoint[j]].IsValueShownAsLabel = graphInfo.PointLabel;
                        chart.Series[strBoxPlotSeriesPoint[j]].LabelFormat = "N" + graphInfo.DecimalPlace;
                        chart.Series[strBoxPlotSeriesPoint[j]].Enabled = true;
                    }

                    if (dMax < Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2))
                        dMax = Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2);
                    if (dMin > Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2))
                        dMin = Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2);
                }

                chart.ChartAreas["BoxPlot"].AxisY.Maximum = dMax;
                chart.ChartAreas["BoxPlot"].AxisY.Minimum = dMin;

                chart.Legends.Add("BoxPlot");

                for (int i = 0; chart.Series.Count > i; i++)
                {
                    chart.Series[i].IsVisibleInLegend = false;
                    chart.Series[i].Font = new Font("Vedana", 9, FontStyle.Regular);
                }

                chart.Series["BoxPlotSeries"].ChartType = SeriesChartType.BoxPlot;
                chart.Series["BoxPlotSeries"]["BoxPlotSeries"] = string.Join(";", strBoxPlotSeries);
                chart.Series["BoxPlotSeries"]["BoxPlotShowMedian"] = "true";
                chart.Series["BoxPlotSeries"]["BoxPlotWhiskerPercentile"] = "0";
                chart.Series["BoxPlotSeries"]["BoxPlotPercentile"] = "25";
                chart.Series["BoxPlotSeries"]["BoxPlotShowAverage"] = "true";
                chart.Series["BoxPlotSeries"]["BoxPlotShowMedian"] = "true";
                chart.Series["BoxPlotSeries"]["BoxPlotShowUnusualValues"] = "true";
                chart.Series["BoxPlotSeries"]["MaxPixelPointWidth"] = "80";
                chart.Series["BoxPlotSeries"].BorderWidth = 2;
                chart.Series["BoxPlotSeries"].IsVisibleInLegend = false;
                chart.Series["BoxPlotSeries"]["PixelPointWidth"] = "25";
                chart.Series["BoxPlotSeries"].Font = new Font("Vedana", 9, FontStyle.Regular);

                for (int i = 1; i < arrAxisYItem.Length + 1; i++)
                {
                    double minXaxes = i - 0.5;
                    double maxXaxes = i + 0.5;
                    string xAxesValue = strBoxPlotSeriesPoint[i - 1];
                    chart.ChartAreas["BoxPlot"].AxisX.CustomLabels.Add(minXaxes, maxXaxes, xAxesValue);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        private void DrawHistogram(GraphInformation graphInfo)
        {
            try
            {
                chart.Visible = false;
                spcHistogram.Visible = false;
                spcHistogram.ResetChartData();

                if (graphInfo.AxisXTitle != null && graphInfo.AxisXTitle != string.Empty)
                    spcHistogram.AxisX.Title = graphInfo.AxisXTitle;

                if (graphInfo.AxisYTitle != null && graphInfo.AxisYTitle != string.Empty)
                    spcHistogram.AxisY.Title = graphInfo.AxisYTitle;

                if (double.IsNaN(graphInfo.USL) && double.IsNaN(graphInfo.Target) && double.IsNaN(graphInfo.LSL))
                    graphInfo.SpecLimit = false;
                else
                {
                    // Deleted By James Kwon 2014/10/16
                    //graphInfo.SpecLimit = true;
                    if (double.IsNaN(graphInfo.USL))
                        spcHistogram.USL = DACrux.Base.StatGlobalVariable.DOUBLE_NULL_DATA;
                    else
                        spcHistogram.USL = graphInfo.USL;

                    if (double.IsNaN(graphInfo.Target))
                        spcHistogram.Target = DACrux.Base.StatGlobalVariable.DOUBLE_NULL_DATA;
                    else
                        spcHistogram.Target = graphInfo.Target;

                    if (double.IsNaN(graphInfo.LSL))
                        spcHistogram.LSL = DACrux.Base.StatGlobalVariable.DOUBLE_NULL_DATA;
                    else
                        spcHistogram.LSL = graphInfo.LSL;
                }

                //spcHistogram.Precision = graphInfo.DecimalPlaceX;
                //spcHistogram.IsViewNormalLine = graphInfo.NormalLine;
                //spcHistogram.IsViewSpecLimit = graphInfo.SpecLimit;
                //spcHistogram.IsView3sLine = graphInfo.View3SigmaLine;
                //spcHistogram.IsViewGridLine = graphInfo.GridLine;
                //spcHistogram.IsViewBarFreqText = graphInfo.Frequence;
                spcHistogram.Font = new Font("Vedana", 7, FontStyle.Regular);

                if (graphInfo.Title == "")
                {
                    spcHistogram.MainTitle = "[ " + graphInfo.Name + " ]";
                }
                else
                {
                    spcHistogram.MainTitle = "[ " + graphInfo.Title + " ]";
                }


                int iRows = graphInfo.DataSource.Rows.Count;
                GraphInformation.ColumnInfoItem[] arrAxisYItem = graphInfo.AxisY;
                spcHistogram.DataSource = graphInfo.DataSource;
                spcHistogram.DrawHistogram(arrAxisYItem[0].ColumnName);

                toolSepLabelAngle.Visible = false;
                toolSepPointLabel.Visible = false;
                toolSepZoom.Visible = false;
                toolLblAngle.Visible = false;
                toolCboAngle.Visible = false;
                toolBtnPointLabel.Visible = false;
                toolBtnLegendBox.Visible = false;
                toolBtn3D.Visible = false;
                toolBtnZoom.Visible = false;

                if (graphInfo.IsImageWithTitles)
                {
                    SetTitles(graphInfo);
                    SaveCaptureImage(graphInfo);
                }
                else
                {
                    SaveCaptureImage(graphInfo);
                    SetTitles(graphInfo);
                }

                spcHistogram.Dock = DockStyle.Fill;
                spcHistogram.Visible = true;
                chart.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DataBindPareto(GraphInformation graphInfo)
        {
            string strXColName;
            string strYColName;
            string strLabel;

            DataTable dtTemp;
            DataTable dtPareto;
            DataRow[] rows;
            DataRow tempRow;

            System.Data.DataView dv;

            int totCount;
            double sumValue = 0;
            double cumSumValue = 0;
            double totValue = 0;
            double tempDouble = 0;

            DateTime dTemp;
            object temp;

            try
            {
                dtTemp = graphInfo.DataSource.Copy();
                totCount = dtTemp.Rows.Count;

                strXColName = dtTemp.Columns[graphInfo.AxisX.ColumnIndex].ColumnName;
                strYColName = dtTemp.Columns[graphInfo.AxisY[0].ColumnIndex].ColumnName;

                dtPareto = new DataTable();
                dtPareto.Columns.Add("Label", typeof(string));
                dtPareto.Columns.Add("Percent", typeof(double));
                dtPareto.Columns.Add("CumPercent", typeof(double));

                if (graphInfo.AxisX.ColumnType == typeof(string))
                {
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = '" + dtTemp.Rows[0][strXColName] + "'");
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        tempRow = dtPareto.NewRow();
                        tempRow["Label"] = strLabel;

                        foreach (DataRow row in rows)
                        {
                            temp = row[strYColName];

                            if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                            {
                                sumValue += tempDouble;
                                totValue += tempDouble;
                            }

                            row.Delete();
                        }

                        tempRow[1] = sumValue;

                        dtPareto.Rows.Add(tempRow);

                        sumValue = 0;

                        dtTemp.AcceptChanges();
                    }
                }
                else if (graphInfo.AxisX.ColumnType == typeof(DateTime))
                {
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = '" + dtTemp.Rows[0][strXColName] + "'");
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        tempRow = dtPareto.NewRow();

                        if (dtTemp.Rows[0][strXColName] != null && DateTime.TryParse(dtTemp.Rows[0][strXColName].ToString(), out dTemp))
                            tempRow["Label"] = dTemp.ToString(graphInfo.DateTimeFormatString);
                        else
                            tempRow["Label"] = string.Empty;

                        foreach (DataRow row in rows)
                        {
                            temp = row[strYColName];

                            if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                            {
                                sumValue += tempDouble;
                                totValue += tempDouble;
                            }

                            row.Delete();
                        }

                        tempRow[1] = sumValue;

                        dtPareto.Rows.Add(tempRow);

                        sumValue = 0;

                        dtTemp.AcceptChanges();
                    }
                }
                else
                {
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = " + dtTemp.Rows[0][strXColName]);
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        tempRow = dtPareto.NewRow();
                        tempRow["Label"] = strLabel;

                        foreach (DataRow row in rows)
                        {
                            temp = row[strYColName];

                            if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                            {
                                sumValue += tempDouble;
                                totValue += tempDouble;
                            }

                            row.Delete();
                        }

                        tempRow[1] = sumValue;

                        dtPareto.Rows.Add(tempRow);

                        sumValue = 0;

                        dtTemp.AcceptChanges();
                    }
                }

                dv = dtPareto.DefaultView;
                dv.Sort = "Percent DESC";
                dtPareto = dv.ToTable();

                foreach (DataRow row in dtPareto.Rows)
                {
                    sumValue = (double)row[1];
                    cumSumValue += sumValue;

                    row[1] = sumValue;
                    row[2] = Math.Round(cumSumValue / totValue * 100, graphInfo.DecimalPlace);
                }
                chart.ChartAreas.Clear();
                chart.Legends.Clear();
                chart.ChartAreas.Add("Pareto");
                ChartSet("Pareto");

                chart.Series.Add("Percent");
                chart.Series.Add("Cummulated");
                chart.Legends.Add("Percent");
                chart.Legends.Add("Cummulated");

                chart.Series["Percent"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                chart.Series["Percent"]["DrawingStyle"] = "Cylinder";
                chart.Series["Cummulated"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart.Series["Cummulated"].BorderWidth = 3;
                
                chart.Series["Cummulated"].IsValueShownAsLabel = true;
                chart.Series["Cummulated"].LabelFormat = "{0:0.0}%";

                chart.ChartAreas["Pareto"].AxisY.Maximum = totValue * 1.2;
                chart.ChartAreas["Pareto"].AxisY.LabelStyle.Format = "{0}";
               
                chart.Series["Cummulated"].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                chart.ChartAreas["Pareto"].AxisY2.LabelStyle.Format = "{0:0.0}%";
                chart.ChartAreas["Pareto"].AxisY2.MajorGrid.Enabled = false;
                //chart.ChartAreas["Pareto"].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chart.ChartAreas["Pareto"].AxisX.Interval = 1;
                chart.Series["Percent"].Points.DataBindXY(dtPareto.Rows, "Label", dtPareto.Rows, "Percent");
                chart.Series["Cummulated"].Points.DataBindXY(dtPareto.Rows, "Label", dtPareto.Rows, "CumPercent");
                chart.Series["Percent"].BackGradientStyle = GradientStyle.DiagonalLeft;             
                chart.Series["Percent"]["PixelPointWidth"] = Convert.ToString(graphInfo.BarSize*3);
                chart.Series["Percent"].Font = new Font("Vedana", 9, FontStyle.Bold);   
                chart.Series["Percent"].IsValueShownAsLabel = graphInfo.PointLabel;
                chart.Series["Percent"].LabelFormat = "N" + graphInfo.DecimalPlace;              
                chart.Series["Percent"].IsVisibleInLegend = graphInfo.LegendBox;
                chart.Series["Cummulated"].IsVisibleInLegend = graphInfo.LegendBox;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void DataBindScatter(GraphInformation graphInfo)
        {
            string strXColName = string.Empty;
            string strSColName = string.Empty;
            string strVColName = string.Empty;
            string strSeries = string.Empty;

            string strSortExpression = string.Empty;

            List<string> lstLabel = new List<string>();

            DataTable dtTemp = graphInfo.DataSource.Copy();
            //DataRow[] rows = null;
            //DataRow row = null;
            double tempY = 0.0;
            double sumY = 0.0;

            System.Data.DataView dv;

            int key = 0;

            //if (graphInfo.Series.ColumnIndex > -1)
            //{
            //    #region [ 나중에 기능 추가 : 시리즈가 지정되어 있고, Value가 하나만 지정되어 있는 경우 (CrossTabDataProvider) ]
            //    strXColName = dtTemp.Columns[graphInfo.AxisX.ColumnIndex].ColumnName;
            //    strSColName = dtTemp.Columns[graphInfo.Series.ColumnIndex].ColumnName;
            //    strVColName = dtTemp.Columns[graphInfo.AxisY[0].ColumnIndex].ColumnName;

            //    dtTemp.Columns.Add("KEY", typeof(int));
            //    dtTemp.Columns.Add(strXColName, typeof(string));
            //    dtTemp.Columns.Add(strSColName, typeof(string));
            //    dtTemp.Columns.Add(strVColName, typeof(double));

            //    while (dtTemp.Rows.Count > 0)
            //    {
            //        rows = dtTemp.Select(string.Format("{0} = '{1}'", strXColName, dtTemp.Rows[0][strXColName]));

            //        for (int i = rows.Length - 1; i >= 0; i--)
            //        {
            //            row = dtTemp.NewRow();
            //            row[strXColName] = rows[i][strXColName];
            //            row[strSColName] = rows[i][strSColName];
            //            row[strVColName] = rows[i][strVColName];

            //            dtTemp.Rows.Add(row);
            //            dtTemp.Rows.Remove(rows[i]);
            //        }
            //    }
            //    #endregion
            //}
            //else
            //{
            #region [ 시리즈가 미지정인 경우 ]

            #region [ Sort ]

            strXColName = dtTemp.Columns[graphInfo.AxisX.ColumnIndex].ColumnName;
            dv = dtTemp.DefaultView;
            strSortExpression = strXColName;

            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                strSortExpression += ", " + dtTemp.Columns[graphInfo.AxisY[i].ColumnIndex].ColumnName;

            strSortExpression += " ASC";

            dv.Sort = strSortExpression;

            dtTemp = dv.ToTable();
            dtTemp.AcceptChanges();

            dtTemp.Columns.Add("KEY", typeof(int));

            #endregion

            #region [ Key Column Setting ]

            if (graphInfo.AxisX.ColumnType == typeof(string))
            {
                string strTemp = string.Empty;
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (strTemp != (string)dtTemp.Rows[i][strXColName])
                    {
                        dtTemp.Rows[i]["KEY"] = ++key;
                        lstLabel.Add((string)dtTemp.Rows[i][strXColName]);
                    }
                    else
                    {
                        dtTemp.Rows[i]["KEY"] = key;
                    }

                    strTemp = (string)dtTemp.Rows[i][strXColName];
                }
            }
            else if (graphInfo.AxisX.ColumnType == typeof(DateTime))
            {
                string strTemp = string.Empty;
                string strLabel = string.Empty;
                DateTime dTemp;
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    strLabel = Convert.ToString(dtTemp.Rows[i][strXColName]);

                    if (DateTime.TryParse(strLabel, out dTemp))
                        strLabel = dTemp.ToString(graphInfo.DateTimeFormatString);
                    else
                        strLabel = string.Empty;

                    

                    if (strTemp != strLabel)
                    {
                        dtTemp.Rows[i]["KEY"] = ++key;

                        lstLabel.Add(strLabel);
                    }
                    else
                    {
                        dtTemp.Rows[i]["KEY"] = key;
                    }

                    strTemp = strLabel;
                }
            }
            else
            {
                string strTemp = string.Empty;
                string strLabel =string.Empty;
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    strLabel = Convert.ToString(dtTemp.Rows[i][strXColName]);               

                    if (strTemp != strLabel)
                    {
                        dtTemp.Rows[i]["KEY"] = ++key;

                        lstLabel.Add(strLabel);
                    }
                    else
                    {
                        dtTemp.Rows[i]["KEY"] = key;
                    }

                    strTemp = strLabel;
                }
            }
            

            #endregion

            #endregion
            if (graphInfo.AxisY[0].ColumnName == "Residual" || graphInfo.AxisY[0].ColumnName == "Percent" || graphInfo.AxisY[0].ColumnID == null)
            {
                chart.ChartAreas.Clear();
                chart.Legends.Clear();
                chart.ChartAreas.Add("Scatter");
                ChartSet("Scatter");

                string strSeriesName = graphInfo.AxisY[0].ColumnName;
                chart.Series.Add(strSeriesName);
                chart.Series.Add("strSeriesLine");
                chart.Series.Add("cumulative normal distribution");

                //적합도 0점 라인
                  if (graphInfo.AxisYTitle == "Residual")
                  {
                    chart.ChartAreas["Scatter"].AxisY.Crossing = 0;
                    chart.ChartAreas["Scatter"].AxisX.IsMarksNextToAxis = false;
                    chart.ChartAreas["Scatter"].AxisX.LineWidth = 4;
                    chart.ChartAreas["Scatter"].AxisX.LineColor = Color.CadetBlue;
                    //chart.ChartAreas["Scatter"].AxisX.Interval = 1.0;   // Default is Auto
                  }
                  
                //누적 확률 분포 곡선 
                  if (graphInfo.AxisXTitle == "Residual")
                  {

                      double coef = 1.0 / Math.Sqrt(2 * Math.PI);
                      double doubleX;

                      for (int x = -50; x <= 50; x++)
                      {
                          doubleX = x / 10.0;
                          tempY = coef * Math.Exp(doubleX * doubleX / -2) / 10;
                          sumY += tempY;
                          chart.Series["cumulative normal distribution"].Points.AddXY(doubleX - 0.05, sumY * 100);
                      }
                      chart.Series["cumulative normal distribution"].ChartType = SeriesChartType.Spline;
                      chart.Series["cumulative normal distribution"].Color = Color.CadetBlue;
                      chart.Series["cumulative normal distribution"].BorderWidth = 3;
                      chart.Invalidate();
                    
                      for (int i = 0; graphInfo.DataSource.Rows.Count > i; i++)
                      {
                       
                          if (i == graphInfo.DataSource.Rows.Count - 1 || !graphInfo.DataSource.Rows[i + 1]["Residual"].Equals(graphInfo.DataSource.Rows[i]["Residual"]))
                          {

                              double yVal = Convert.ToDouble(graphInfo.DataSource.Rows[i]["Percent"]);
                              double xVal = Convert.ToDouble(graphInfo.DataSource.Rows[i]["Residual"]);
                              chart.Series["strSeriesLine"].Points.AddXY(xVal, yVal);
                          }
                      }

                      chart.Series["strSeriesLine"].ChartType = SeriesChartType.Spline;
                      chart.Series["strSeriesLine"]["LineTension"] = "0.4";
                      chart.Series["strSeriesLine"].BorderWidth = 1;
                      chart.Series["strSeriesLine"].Color = Color.Red;
                      chart.Series["strSeriesLine"].IsVisibleInLegend = false;
                  }
             

                  chart.Series[strSeriesName].Points.DataBindXY(dtTemp.Rows, graphInfo.AxisX.ColumnName, dtTemp.Rows, graphInfo.AxisY[0].ColumnName);
                  chart.Series[strSeriesName].ChartType = SeriesChartType.Point;


                  chart.Legends.Add("Scatter");

                  chart.Series[strSeriesName].IsVisibleInLegend = graphInfo.LegendBox;
                  chart.Series["cumulative normal distribution"].IsVisibleInLegend = false;
                  chart.Series["strSeriesLine"].IsVisibleInLegend = false;
                  chart.Series[strSeriesName].BackGradientStyle = GradientStyle.DiagonalLeft;
                  chart.Series[strSeriesName].Font = new Font("Vedana", 8, FontStyle.Bold);
                  chart.Series[strSeriesName].IsValueShownAsLabel = graphInfo.PointLabel;
                  chart.Series[strSeriesName].SmartLabelStyle.Enabled = true;
                  chart.Series[strSeriesName].MarkerSize = graphInfo.PointSize;
                  chart.Series[strSeriesName].MarkerStyle = MarkerStyle .Circle;
                  chart.Series[strSeriesName].MarkerColor = Color.Red;
                  chart.Series[strSeriesName].XAxisType = AxisType.Primary;

                  // X축의 평균을 구해서 X축의 값을 설정
                  //double xAvg = Math.Round(Convert.ToDouble(dtTemp.Compute("Avg([" + graphInfo.AxisX.ColumnName + "])", "")), 2);
                  //chart.ChartAreas["Scatter"].AxisX.Minimum = xAvg - 5.0;
                  //chart.ChartAreas["Scatter"].AxisX.Maximum = xAvg + 5.0;
                
                  double xMin = Math.Round(Convert.ToDouble(dtTemp.Compute("Min([" + graphInfo.AxisX.ColumnName + "])", "")), 2);
                  double xMax = Math.Round(Convert.ToDouble(dtTemp.Compute("Max([" + graphInfo.AxisX.ColumnName + "])", "")), 2);
                  double xMargin = (xMax - xMin) * 0.05;
                  
                  chart.ChartAreas["Scatter"].AxisX.Minimum = xMin - xMargin;
                  chart.ChartAreas["Scatter"].AxisX.Maximum = xMax + xMargin;
                  //chart.ChartAreas["Scatter"].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
                  //chart.ChartAreas["Scatter"].AxisX.LabelStyle.Angle = 45;
                  //chart.ChartAreas["Scatter"].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;

                  if (graphInfo.AxisY.Length > 0 && graphInfo.AxisY[0].ColumnName != null)
                  {
                      double yMin = Math.Round(Convert.ToDouble(dtTemp.Compute("Min([" + graphInfo.AxisY[0].ColumnName + "])", "")), 2);
                      double yMax = Math.Round(Convert.ToDouble(dtTemp.Compute("Max([" + graphInfo.AxisY[0].ColumnName + "])", "")), 2);
                      double yMargin = (yMax - yMin) * 0.05;

                      if (graphInfo.Name.IndexOf("Descriptive") > -1)
                          yMargin = (yMax - yMin) * 1.1;

                      chart.ChartAreas["Scatter"].AxisY.Minimum = yMin - yMargin;
                      chart.ChartAreas["Scatter"].AxisY.Maximum = yMax + yMargin;

                      if(graphInfo.Name.IndexOf("Descriptive") > -1)
                        chart.ChartAreas["Scatter"].AxisY.Enabled = AxisEnabled.False;
                  }
                  chart.ChartAreas["Scatter"].AxisX.IsMarginVisible = true;
                  chart.ChartAreas["Scatter"].AxisY.IsMarginVisible = true;
                }// Graph View
                else
                {
                    chart.ChartAreas.Clear();
                    chart.Legends.Clear();
                    chart.ChartAreas.Add("Scatter");

                    if (graphInfo.AxisX.ColumnType == typeof(DateTime))
                    {
                        LabelStyle ls = new LabelStyle();
                        ls.Format = graphInfo.DateTimeFormatString;
                        chart.ChartAreas["Scatter"].AxisX.LabelStyle = ls;

                    }

                    ChartSet("Scatter");

                    double yMin = double.MaxValue;
                    double yMax = double.MinValue;

                    for (int j = 0; j < graphInfo.AxisY.Length; j++)
                    {
                        string strSeriesName = (graphInfo.ColumnInfoItems[j].ColumnName == string.Empty) ? graphInfo.AxisY[j].ColumnID : graphInfo.AxisY[j].ColumnName;

                        //X값이 문자, 시간으로 들어왔을 때 
                        string axisXColumnId = graphInfo.AxisX.ColumnID;
                        if (graphInfo.AxisX.ColumnType == typeof(DateTime) || graphInfo.AxisX.ColumnType == typeof(string))
                        {
                            string[] splitPoint = axisXColumnId.Split('-');
                            axisXColumnId = splitPoint[0].Trim();       
                        }
   
                        chart.Series.Add(strSeriesName);
                        chart.Legends.Add(strSeriesName);
                        chart.Series[strSeriesName].Points.DataBindXY(dtTemp.Rows, axisXColumnId, dtTemp.Rows, graphInfo.AxisY[j].ColumnID);
                        chart.Series[strSeriesName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                        chart.Series[strSeriesName].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft;
                       
                        chart.Series[strSeriesName].MarkerSize = graphInfo.PointSize;
                        chart.Series[strSeriesName].Font = new Font("Vedana", 8, FontStyle.Bold);
                        chart.Series[strSeriesName].IsValueShownAsLabel = graphInfo.PointLabel;
                        chart.Series[strSeriesName].IsVisibleInLegend = graphInfo.LegendBox;
                        chart.Series[strSeriesName].LabelFormat = "N" + graphInfo.DecimalPlace;

                        if(yMin > Math.Round(Convert.ToDouble(dtTemp.Compute("Min([" + graphInfo.AxisY[j].ColumnID + "])", "")), 2))
                            yMin = Math.Round(Convert.ToDouble(dtTemp.Compute("Min([" + graphInfo.AxisY[j].ColumnID + "])", "")), 2);

                        if(yMax < Math.Round(Convert.ToDouble(dtTemp.Compute("Max([" + graphInfo.AxisY[j].ColumnID + "])", "")), 2))
                            yMax = Math.Round(Convert.ToDouble(dtTemp.Compute("Max([" + graphInfo.AxisY[j].ColumnID + "])", "")), 2);
                    }

                    double yMargin = (yMax - yMin) * 0.05;
                    chart.ChartAreas["Scatter"].AxisY.Minimum = yMin - yMargin;
                    chart.ChartAreas["Scatter"].AxisY.Maximum = yMax + yMargin;
                    //chart.ChartAreas["Scatter"].AxisY.Enabled = AxisEnabled.False;
                    double xMin = Math.Round(Convert.ToDouble(dtTemp.Compute("Min([" + graphInfo.AxisX.ColumnID + "])", "")), 2);
                    double xMax = Math.Round(Convert.ToDouble(dtTemp.Compute("Max([" + graphInfo.AxisX.ColumnID + "])", "")), 2);
                    double xmargin = (xMax - xMin) * 0.05;

                    chart.ChartAreas["Scatter"].AxisX.Minimum = xMin - xmargin;
                    chart.ChartAreas["Scatter"].AxisX.Maximum = xMax + xmargin;

                    chart.ChartAreas["Scatter"].AxisY.IsMarginVisible = true;
                }
            }
      

            //#region [ Field Map Setting ]

            //chart.DataSourceSettings.Fields.Clear();

            //if (graphInfo.AxisX.ColumnType == typeof(string) || graphInfo.AxisX.ColumnType == typeof(DateTime))
            //    chart.DataSourceSettings.Fields.Add(new FieldMap("KEY", FieldUsage.XValue));
            //else
            //    chart.DataSourceSettings.Fields.Add(new FieldMap(strXColName, FieldUsage.XValue));

            //for (int i = 0; i < graphInfo.AxisY.Length; i++)
            //    chart.DataSourceSettings.Fields.Add(new FieldMap(dtTemp.Columns[graphInfo.AxisY[i].ColumnIndex].ColumnName, FieldUsage.Value));

            //#endregion

            //#region [ DataBind ]

            //chart.DataSourceSettings.DataSource = dtTemp;

            //#endregion

            //#region [ Label, Series Name Update ]

            //if (graphInfo.AxisX.ColumnType == typeof(string) || graphInfo.AxisX.ColumnType == typeof(DateTime))
            //{
            //    for (int i = 0; i < lstLabel.Count; i++)
            //        chart.AxisX.Labels[i + 1] = lstLabel[i];
            //}

            //for (int i = 0; i < graphInfo.AxisY.Length; i++)
            //    chart.Series[i].Text = graphInfo.AxisY[i].ColumnName;

            //#endregion
        


        private void DataBindPie(GraphInformation graphInfo)
        {
            string strXColName = string.Empty; 
            string strLabel = string.Empty; 

            List<string> lstLabel = new List<string>(); 
            List<double[]> lstValue = new List<double[]>(); 
            List<double[]> lstTotal = new List<double[]>();

            DataTable dtTemp = graphInfo.DataSource.Copy();
            DataRow[] rows = null; 
            int totCount = dtTemp.Rows.Count; 
            double[] arrTotValue = new double[graphInfo.AxisY.Length];
            double[] arrSumValue;
            double tempDouble = 0;

            object temp;

            try
            {
                strXColName = dtTemp.Columns[graphInfo.AxisX.ColumnIndex].ColumnName;

                if (graphInfo.AxisX.ColumnType == typeof(string))
                {
                    #region [ String ]
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);
                        arrSumValue = new double[graphInfo.AxisY.Length];

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = '" + dtTemp.Rows[0][strXColName] + "'");
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        foreach (DataRow row in rows)
                        {
                            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                            {
                                temp = row[graphInfo.AxisY[i].ColumnIndex];
                                if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                {
                                    arrSumValue[i] += tempDouble;
                                    arrTotValue[i] += tempDouble;
                                }
                            }

                            row.Delete();
                        }

                        lstLabel.Add(strLabel);
                        lstValue.Add(arrSumValue);

                        dtTemp.AcceptChanges();
                    }
                    #endregion
                }
                else if (graphInfo.AxisX.ColumnType == typeof(DateTime))
                {
                    #region [ DateTime ]
                    DateTime dTemp;
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = (dtTemp.Rows[0][strXColName]).ToString();

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = '" + dtTemp.Rows[0][strXColName] + "'");
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        if (DateTime.TryParse(strLabel, out dTemp))
                            strLabel = dTemp.ToString(graphInfo.DateTimeFormatString);
                        else
                            strLabel = string.Empty;

                        arrSumValue = new double[graphInfo.AxisY.Length];

                        foreach (DataRow row in rows)
                        {
                            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                            {
                                temp = row[graphInfo.AxisY[i].ColumnIndex];
                                if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                {
                                    arrSumValue[i] += tempDouble;
                                    arrTotValue[i] += tempDouble;
                                }
                            }

                            row.Delete();
                        }

                        lstLabel.Add(strLabel);
                        lstValue.Add(arrSumValue);

                        dtTemp.AcceptChanges();
                    }
                    #endregion
                }
                else
                {
                    #region [ Number ]
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);
                        arrSumValue = new double[graphInfo.AxisY.Length];

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = " + dtTemp.Rows[0][strXColName]);
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        foreach (DataRow row in rows)
                        {
                            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                            {
                                temp = row[graphInfo.AxisY[i].ColumnIndex];
                                if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                {
                                    arrSumValue[i] += tempDouble;
                                    arrTotValue[i] += tempDouble;
                                }
                            }

                            row.Delete();
                        }

                        lstLabel.Add(strLabel);
                        lstValue.Add(arrSumValue);

                        dtTemp.AcceptChanges();
                    }


                    #endregion
                }
                chart.ChartAreas.Clear();
                chart.Legends.Clear();
                chart.ChartAreas.Add("Pie");                        
                ChartSet("Pie");
        
                for (int j = 0; j < graphInfo.AxisY.Length; j++)
                {
                    string strSeriesName = (graphInfo.AxisY[j].ColumnName == string.Empty) ? graphInfo.AxisY[j].ColumnID : graphInfo.AxisY[j].ColumnName;
                    double[] dValues = new double[lstLabel.Count];
                    for (int i = 0; i < lstLabel.Count; i++)
                    {
                        dValues[i] = Convert.ToDouble(lstValue[i][j] / arrTotValue[j]);
                    }

                    chart.Series.Add(strSeriesName); 
                    chart.Legends.Add(strSeriesName);
                    chart.Series[strSeriesName].IsVisibleInLegend = graphInfo.LegendBox;

                    chart.Series[strSeriesName].Font = new Font("Vedana", 9, FontStyle.Bold);          
                    chart.Series[strSeriesName].ShadowOffset = 10;
                    chart.Series[strSeriesName].Points.DataBindXY(lstLabel, dValues);
                    chart.Series[strSeriesName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                    chart.Series[strSeriesName]["PieLabelStyle"] = "outside";
                    chart.Series[strSeriesName].LabelBackColor = Color.LightGray;
                    chart.Series[strSeriesName].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft;
                    chart.Series[strSeriesName].BackSecondaryColor = System.Drawing.Color.LightGray;
                    chart.Series[strSeriesName]["PieLineColor"] = "Black";
                    chart.Series[strSeriesName]["PieDrawingStyle"] = "Concave";
                    
                    chart.Series[strSeriesName].IsValueShownAsLabel = graphInfo.PointLabel;               
                    chart.Series[strSeriesName].LabelFormat = "P2";                 
                }
              
              

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void DataBindLineBar(GraphInformation graphInfo)
        {
           
            string strXColName = string.Empty; 
            string strLabel = string.Empty; 

            List<string> lstLabel = new List<string>();
            List<double[]> lstValue = new List<double[]>(); 
            List<int> lstCount = new List<int>(); 

            DataTable dtTemp = graphInfo.DataSource.Copy(); 
            DataRow[] rows = null; 
            DataRow tempRow = null;

            System.Data.DataView dv; 


            int totCount = dtTemp.Rows.Count;
            double[] arrSumValue;  

            object temp;
            double tempDouble = 0;

            try
            {
                if (graphInfo.AxisX.ColumnName != null && graphInfo.AxisX.ColumnName.Length > 0)
                {
                    strXColName = dtTemp.Columns[graphInfo.AxisX.ColumnIndex].ColumnName;
                }

                if (graphInfo.AxisX.ColumnType == typeof(string)) 
                {
                    #region [ String ]
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);
                        arrSumValue = new double[graphInfo.AxisY.Length];

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = '" + dtTemp.Rows[0][strXColName] + "'");
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        foreach (DataRow row in rows)
                        {

                            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                            {
                                temp = row[graphInfo.AxisY[i].ColumnIndex];

                                if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                    arrSumValue[i] += tempDouble;
                            }

                            row.Delete();
                        }

                        lstLabel.Add(strLabel);
                        lstValue.Add(arrSumValue);

                        dtTemp.AcceptChanges();
                    }
                    #endregion
                }
                else if (graphInfo.AxisX.ColumnType == typeof(DateTime))
                {
                    #region [ DateTime ]
                    DateTime dTemp;
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);
                        
                        arrSumValue = new double[graphInfo.AxisY.Length];

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "] = '" + dtTemp.Rows[0][strXColName] + "'");
                        else
                            rows = dtTemp.Select("[" + strXColName + "] is null");

                        if (DateTime.TryParse(strLabel, out dTemp))
                            strLabel = dTemp.ToString(graphInfo.DateTimeFormatString);
                            //if (strLabel.Contains(" "))
                            //{
                            //    string[] splitPoint = strLabel.Split(' ');
                            //    strLabel = splitPoint[0].Trim();
                            //}
                        else
                            strLabel = string.Empty;

                        foreach (DataRow row in rows)
                        {
                            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                            {
                                temp = row[graphInfo.AxisY[i].ColumnIndex];

                                if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                    arrSumValue[i] += tempDouble;
                            }

                            row.Delete();
                        }

                        lstLabel.Add(strLabel);
                        lstValue.Add(arrSumValue);

                        dtTemp.AcceptChanges();
                    }
                    #endregion
                }
                else if (graphInfo.AxisX.ColumnType == null)
                {
                    for (int j = 0; j < graphInfo.AxisY.Length; j++)
                    {
                        arrSumValue = new double[totCount];
                        for (int i = 0; i < totCount; i++)
                        {
                           if (lstLabel.IndexOf(i.ToString()) < 0) lstLabel.Add(i.ToString());
                                temp = dtTemp.Rows[i][graphInfo.AxisY[j].ColumnIndex];

                            if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                arrSumValue[i] = tempDouble;
                        }
                        lstValue.Add(arrSumValue);
                    }
                }
                else
                {
                    #region [ Number ]
                    while (dtTemp.Rows.Count > 0)
                    {
                        strLabel = Convert.ToString(dtTemp.Rows[0][strXColName]);
                        arrSumValue = new double[graphInfo.AxisY.Length];

                        if (strLabel != string.Empty)
                            rows = dtTemp.Select("[" + strXColName + "]" + " = " + dtTemp.Rows[0][strXColName]);
                        else
                            rows = dtTemp.Select("[" + strXColName + "]" + " is null");

                        foreach (DataRow row in rows)
                        {
                            for (int i = 0; i < graphInfo.AxisY.Length; i++)
                            {
                                temp = row[graphInfo.AxisY[i].ColumnIndex];

                                if (temp != null && double.TryParse(temp.ToString(), out tempDouble))
                                    arrSumValue[i] += tempDouble;
                            }

                            row.Delete();
                        }

                        lstLabel.Add(strLabel);
                        lstValue.Add(arrSumValue);

                        dtTemp.AcceptChanges();
                    }
                    #endregion
                }

                dtTemp = new DataTable();
                dtTemp.Columns.Add("Label", typeof(string));
                for (int i = 0; i < graphInfo.AxisY.Length; i++)
                {
                    dtTemp.Columns.Add(graphInfo.AxisY[i].ColumnName, graphInfo.AxisY[i].ColumnType);
                }

                if (graphInfo.AxisX.ColumnType != null)
                {
                    for (int i = 0; i < lstLabel.Count; i++)
                    {
                        tempRow = dtTemp.NewRow();
                        tempRow["Label"] = lstLabel[i];
                        for (int j = 0; j < graphInfo.AxisY.Length; j++)
                        {
                            tempRow[j + 1] = lstValue[i][j];
                        }
                        dtTemp.Rows.Add(tempRow);
                    }
                }


                dv = dtTemp.DefaultView;
                dv.Sort = "Label ASC";
                dtTemp = dv.ToTable();

                if (graphInfo.Type == GraphType.Bar)
                {
                    chart.ChartAreas.Clear();
                    chart.Legends.Clear();
                    chart.ChartAreas.Add("Bar");
                    ChartSet("Bar");

                    chart.ChartAreas["Bar"].Position.X = 5;
                    chart.ChartAreas["Bar"].Position.Y = 8;
                    if (graphInfo.ImageSize.Height == 0)
                        chart.ChartAreas["Bar"].Position.Height = 90;
                    else
                        chart.ChartAreas["Bar"].Position.Height = graphInfo.ImageSize.Height;

                    if (graphInfo.ImageSize.Width == 0)
                        chart.ChartAreas["Bar"].Position.Width = 85;
                    else
                        chart.ChartAreas["Bar"].Position.Width = graphInfo.ImageSize.Width;

                    for (int j = 0; j < graphInfo.AxisY.Length; j++)
                    {
                        string strSeriesName = (graphInfo.AxisY[j].ColumnName == string.Empty) ? graphInfo.AxisY[j].ColumnID : graphInfo.AxisY[j].ColumnName;                      
                        double[] dValues = new double[lstLabel.Count];

                        for (int i = 0; i < lstLabel.Count; i++)
                        {
                            dValues[i] = Convert.ToDouble(lstValue[i][j]);
                        }
                        
                        chart.Series.Add(strSeriesName);
                        chart.Legends.Add(strSeriesName);
                        chart.Series[strSeriesName].Points.DataBindXY(lstLabel, dValues);
                        chart.Series[strSeriesName].ChartType = SeriesChartType.Column;
                        chart.Series[strSeriesName]["DrawingStyle"] = "Cylinder";
                        chart.Series[strSeriesName].BackGradientStyle = GradientStyle.DiagonalLeft;
                        chart.Series[strSeriesName].IsValueShownAsLabel = graphInfo.PointLabel;
                        chart.Series[strSeriesName].Font = new Font("Vedana", 9, FontStyle.Regular);               
                        chart.Series[strSeriesName].IsVisibleInLegend = graphInfo.LegendBox;
                        chart.Series[strSeriesName]["PixelPointWidth"] = Convert.ToString(graphInfo.BarSize * 10);
                        chart.Series[strSeriesName].LabelFormat = "N"+ graphInfo.DecimalPlace;
                                   
                    }
                }
                else if (graphInfo.Type == GraphType.Line)
                {
                    chart.ChartAreas.Clear();
                    chart.Legends.Clear();
                    chart.ChartAreas.Add("Line");
                    ChartSet("Line");
                    object maxVal;
                    object minVal;
                    object stdVal;

                    double dMax = double.MinValue;
                    double dMin = double.MaxValue;
                    for (int j = 0; j < graphInfo.AxisY.Length; j++)
                    {
                        string strSeriesName = (graphInfo.AxisY[j].ColumnName == string.Empty) ? graphInfo.AxisY[j].ColumnID : graphInfo.AxisY[j].ColumnName;
                        double[] dValues = new double[lstLabel.Count];

                        for (int i = 0; i < lstLabel.Count; i++)
                        {
                            // Changed By James Kwon
                            //if (graphInfo.Name.IndexOf("RegressionAnalysis") > -1)
                            //    dValues[i] = Convert.ToDouble(lstValue[j][i]);
                            //else
                                dValues[i] = Convert.ToDouble(lstValue[i][j]);
                        }
                        chart.Series.Add(strSeriesName);
                        chart.Legends.Add(strSeriesName);
                        chart.Series[strSeriesName].Points.DataBindY(dValues);
                        chart.Series[strSeriesName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        chart.Series[strSeriesName].BorderWidth = 2;
                        chart.Series[strSeriesName].MarkerStyle = MarkerStyle.Circle;
                        chart.Series[strSeriesName].MarkerSize = graphInfo.PointSize;
                        chart.Series[strSeriesName].MarkerColor = graphInfo.PointColor;
                        chart.Series[strSeriesName].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft;
                        chart.Series[strSeriesName].IsValueShownAsLabel = graphInfo.PointLabel;
                        chart.Series[strSeriesName].Font = new Font("Vedana", 9, FontStyle.Regular); 
                        chart.Series[strSeriesName].IsVisibleInLegend = graphInfo.LegendBox;
                        chart.Series[strSeriesName].LabelFormat = "N" + graphInfo.DecimalPlace;
                        chart.ChartAreas["Line"].AxisY.Crossing = 0;
                        chart.ChartAreas["Line"].AxisX.IsMarksNextToAxis = false;
                        chart.ChartAreas["Line"].AxisX.LineWidth = 4;
                        chart.ChartAreas["Line"].AxisX.LineColor = Color.CadetBlue;

                        //maxVal = graphInfo.DataSource.Compute("Max([" + strSeriesName + "])", "");
                        //minVal = graphInfo.DataSource.Compute("Min([" + strSeriesName + "])", "");
                        //stdVal = graphInfo.DataSource.Compute("StDev([" + strSeriesName + "])", "");

                        maxVal = dtTemp.Compute("Max([" + strSeriesName + "])", "");
                        minVal = dtTemp.Compute("Min([" + strSeriesName + "])", "");
                        stdVal = dtTemp.Compute("StDev([" + strSeriesName + "])", "");

                        if (dMax < Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2))
                            dMax = Math.Round(Convert.ToDouble(maxVal) + Convert.ToDouble(stdVal), 2);
                        if (dMin > Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2))
                            dMin = Math.Round(Convert.ToDouble(minVal) - Convert.ToDouble(stdVal), 2);

                    }
                    chart.ChartAreas["Line"].AxisY.Maximum = dMax;
                    chart.ChartAreas["Line"].AxisY.Minimum = dMin;
                    //chart.ChartAreas["Line"].AxisX.IsMarginVisible = true;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            } 
        }

        private void DataBindGeneral(GraphInformation graphInfo)
        {
            //double nTemp;
            //object objTemp;
            //DateTime dTemp;
            //string strSeriesName;

            //GraphInformation.ColumnInfoItem[] arrAxisYItem = graphInfo.AxisY;

            //chart.Data.Series = arrAxisYItem.Length;
            //chart.Data.Points = graphInfo.DataSource.Rows.Count;

            //if (graphInfo.AxisX.ColumnType == typeof(DateTime))
            //{
            //    #region DATETIME
            //    for (int i = 0; i < chart.Data.Points; i++)
            //    {
            //        objTemp = graphInfo.DataSource.Rows[i][graphInfo.AxisX.ColumnIndex];

            //        if (objTemp != null && DateTime.TryParse(objTemp.ToString(), out dTemp))
            //            chart.Data.Labels[i] = dTemp.ToString(graphInfo.DateTimeFormatString);
            //        else
            //            chart.Data.Labels[i] = string.Empty;

            //        for (int j = 0; j < chart.Data.Series; j++)
            //        {
            //            if (double.TryParse(graphInfo.DataSource.Rows[i][arrAxisYItem[j].ColumnIndex].ToString(), out nTemp))
            //                chart.Data[j, i] = nTemp;
            //            else
            //                chart.Data[j, i] = ChartFX.WinForms.DataValues.Hidden;
            //        }
            //    }
            //    #endregion
            //}
            //else
            //{
            //    for (int i = 0; i < chart.Data.Points; i++)
            //    {
            //        objTemp = graphInfo.DataSource.Rows[i][graphInfo.AxisX.ColumnIndex];

            //        if (objTemp != null)
            //            chart.Data.Labels[i] = objTemp.ToString();
            //        else
            //            chart.Data.Labels[i] = string.Empty;

            //        for (int j = 0; j < chart.Data.Series; j++)
            //        {
            //            if (double.TryParse(graphInfo.DataSource.Rows[i][arrAxisYItem[j].ColumnIndex].ToString(), out nTemp))
            //                chart.Data[j, i] = nTemp;
            //            else
            //                chart.Data[j, i] = ChartFX.WinForms.DataValues.Hidden;
            //        }
            //    }
            //}

            //for (int i = 0; i < chart.Data.Series; i++)
            //{
            //    strSeriesName = arrAxisYItem[i].ColumnName;

            //    if (strSeriesName == string.Empty)
            //        strSeriesName = arrAxisYItem[i].ColumnID;

            //    chart.Series[i].Text = strSeriesName;
            //}
        }

        private void SetTitles(GraphInformation graphInfo)
        {
            try
            {
                if (graphInfo.Type == GraphType.Histogram)
                {
                    if (graphInfo.Title == null || graphInfo.Title == string.Empty)
                        return;

                    spcHistogram.MainTitle = graphInfo.Title;

                    if (graphInfo.SubTitle == null || graphInfo.SubTitle == string.Empty)
                        return;

                    spcHistogram.MainTitle += (char)10 + graphInfo.SubTitle;
                }

                else if (graphInfo.Type == GraphType.Scatter)
                {
                   
                }
                //    if (graphInfo.RegressionEquation)
                //    {
                //        #region Regression Info
                //        StatisticFormula statRegression = new StatisticFormula();
                //        statRegression.Chart = chart;
                //        statRegression.LegendBox.Visible = false;
                //        //statRegression.Studies.Add(Analysis.PearsonCorrelationCoefficient);  //상관계수
                //        statRegression.Studies.Add(Analysis.RegressionLine);
                //        statRegression.Studies.Add(Analysis.LinearRegressionM);
                //        statRegression.Studies.Add(Analysis.LinearRegressionB);
                //        #region 회귀선
                //        StudyLine oRegline = (StudyLine)statRegression.Studies.Find(StudyType.Analysis, (int)Analysis.RegressionLine);
                //        oRegline.Line.Color = Color.DarkGreen;
                //        oRegline.Line.Width = 2;
                //        oRegline.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                //        oRegline.Visible = true;
                //        #endregion
                //        double dblCoefficient = Math.Round(statRegression.Calculators[0].Get(Analysis.LinearRegressionM), 4);
                //        double dblIntercept = Math.Round(statRegression.Calculators[0].Get(Analysis.LinearRegressionB), 4);
                //        //double dblCorrelationCoefficient = statistics1.Calculators[0].Get(Analysis.PearsonCorrelationCoefficient);  //상관계수
                //        string strRegressionEquation = chart.AxisY.Title.Text + " = " + dblIntercept.ToString() + ((dblCoefficient < 0) ? dblCoefficient.ToString() : "+" + dblCoefficient.ToString()) + chart.AxisX.Title.Text;
                //        #endregion




                //        if (graphInfo.Title != null && graphInfo.Title != string.Empty)
                //        {
                //            chart.Titles.Add(new TitleDockable());
                //            chart.Titles[0].Text = graphInfo.Title;
                //            chart.Titles[0].Font = new Font("Tahoma", 12);
                //            chart.Titles[0].Alignment = StringAlignment.Center;
                //        }

                //        if (graphInfo.SubTitle != null && graphInfo.SubTitle != string.Empty)
                //        {
                //            if (graphInfo.Title == null || graphInfo.Title == string.Empty)
                //            {
                //                chart.Titles.Add(new TitleDockable());
                //                chart.Titles[0].Text = graphInfo.SubTitle;
                //                chart.Titles[0].Font = new Font("Tahoma", 12);
                //                chart.Titles[0].Alignment = StringAlignment.Center;
                //            }
                //            else
                //            {
                //                chart.Titles.Add(new TitleDockable());
                //                chart.Titles[1].Text = graphInfo.SubTitle;
                //                chart.Titles[1].Font = new Font("Tahoma", 12);
                //                chart.Titles[1].Alignment = StringAlignment.Center;
                //            }
                //        }
                //        if (graphInfo.RegressionEquation)
                //        {
                //            #region Regression Info
                //            Statistics statRegression = new Statistics();
                //            statRegression.Chart = chart;
                //            statRegression.LegendBox.Visible = false;
                //            //statRegression.Studies.Add(Analysis.PearsonCorrelationCoefficient);  //상관계수
                //            statRegression.Studies.Add(Analysis.RegressionLine);
                //            statRegression.Studies.Add(Analysis.LinearRegressionM);
                //            statRegression.Studies.Add(Analysis.LinearRegressionB);
                //            #region 회귀선
                //            StudyLine oRegline = (StudyLine)statRegression.Studies.Find(StudyType.Analysis, (int)Analysis.RegressionLine);
                //            oRegline.Line.Color = Color.DarkGreen;
                //            oRegline.Line.Width = 2;
                //            oRegline.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                //            oRegline.Visible = true;
                //            #endregion
                //            double dblCoefficient = Math.Round(statRegression.Calculators[0].Get(Analysis.LinearRegressionM), 4);
                //            double dblIntercept = Math.Round(statRegression.Calculators[0].Get(Analysis.LinearRegressionB), 4);
                //            //double dblCorrelationCoefficient = statistics1.Calculators[0].Get(Analysis.PearsonCorrelationCoefficient);  //상관계수
                //            string strRegressionEquation = chart.AxisY.Title.Text + " = " + dblIntercept.ToString() + ((dblCoefficient < 0) ? dblCoefficient.ToString() : "+" + dblCoefficient.ToString()) + chart.AxisX.Title.Text;
                //            #endregion

                //            if (graphInfo.Title == null || graphInfo.Title == string.Empty)
                //            {
                //                if (graphInfo.SubTitle == null || graphInfo.SubTitle == string.Empty)
                //                {
                //                    chart.Titles.Add(new TitleDockable());
                //                    chart.Titles[0].Text = strRegressionEquation;
                //                    chart.Titles[0].Font = new Font("Tahoma", 12);
                //                    chart.Titles[0].Alignment = StringAlignment.Center;
                //                }
                //                else
                //                {
                //                    chart.Titles.Add(new TitleDockable());
                //                    chart.Titles[1].Text = strRegressionEquation;
                //                    chart.Titles[1].Font = new Font("Tahoma", 12);
                //                    chart.Titles[1].Alignment = StringAlignment.Center;
                //                }
                //            }
                //            else
                //            {
                //                if (graphInfo.SubTitle == null || graphInfo.SubTitle == string.Empty)
                //                {
                //                    chart.Titles.Add(new TitleDockable());
                //                    chart.Titles[1].Text = strRegressionEquation;
                //                    chart.Titles[1].Font = new Font("Tahoma", 12);
                //                    chart.Titles[1].Alignment = StringAlignment.Center;
                //                }
                //                else
                //                {
                //                    chart.Titles.Add(new TitleDockable());
                //                    chart.Titles[2].Text = strRegressionEquation;
                //                    chart.Titles[2].Font = new Font("Tahoma", 12);
                //                    chart.Titles[2].Alignment = StringAlignment.Center;
                //                }
                //            }
                //        }
                //    }
                //}
            }
        
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SaveCaptureImage(GraphInformation graphInfo)
        {
            Bitmap bmp = null;
            Rectangle rect;
            Graphics g = null;

            try
            {
                if (graphInfo.IsDrawn == true || graphInfo.ImagePath == null || graphInfo.ImagePath == string.Empty)
                    return;

                if (graphInfo.ImageSize.IsEmpty)
                    return;

                if (graphInfo.Type == GraphType.Histogram)
                {
                    
                    spcHistogram.Font = new Font("Vedana", 7, FontStyle.Regular);
                    spcHistogram.SetBounds(0, 0, graphInfo.ImageSize.Width,graphInfo.ImageSize.Height);
                    spcHistogram.Update();
                    //spcHistogram.IsXAxisAutoInterval = true;
                    

                    rect = (spcHistogram.DisplayRectangle);
                    bmp = new Bitmap(rect.Width, rect.Height);

                    spcHistogram.DrawToBitmap(bmp, rect);
                    bmp = ResizeBitmap(bmp, graphInfo.ImageSize.Width, graphInfo.ImageSize.Height);
                    bmp.Save(DACrux.ProjectManager.UI.Common.TempPath + graphInfo.ImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    graphInfo.IsDrawn = true;
                }
                else
                {

                    chart.Titles[0].Font = new Font("Vedana", 8, FontStyle.Bold);

                    chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Vedana", 8, FontStyle.Regular);
                    chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Vedana", 8, FontStyle.Regular);

                    for (int i = 0; i < chart.Series.Count; i++)
                    {
                        chart.Series[i].Font = new Font("Vedana", 8, FontStyle.Regular);
                    }

                    chart.SetBounds(0, 0, graphInfo.ImageSize.Width, graphInfo.ImageSize.Height);
                    chart.Update();
                    
                    rect = chart.DisplayRectangle;
                    bmp = new Bitmap(rect.Width, rect.Height);

                    chart.DrawToBitmap(bmp, rect);
                    bmp = ResizeBitmap(bmp, graphInfo.ImageSize.Width, graphInfo.ImageSize.Height);
                    bmp.Save(DACrux.ProjectManager.UI.Common.TempPath + graphInfo.ImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    graphInfo.IsDrawn = true;

                }
                if(bmp != null)
                    bmp.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (g != null)
                    g.Dispose();
            }
        }

        private Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((Image)result))
                g.DrawImage(b, 0, 0, nWidth, nHeight);
            return result;
        }
 
        private void SetCursor(Control oControl, System.Windows.Forms.Cursor oCursor)
        {
            foreach (Control oSubCtrl in oControl.Controls)
            {
                SetCursor(oSubCtrl, oCursor);
                oSubCtrl.Cursor = oCursor;
            }
            //oControl.Cursor = oCursor;
        }

        //#endregion

        #region " EVENT & DELEGATE "

        public event GraphUpdatingHandler GraphUpdating;

        #endregion

        #region " EVENT HANDLER "

        private void toolBtnImageCopy_Click(object sender, EventArgs e)
        
        {
            //클립보드에 저장
            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //chart.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            //Bitmap bmp = new Bitmap(stream);
            //Clipboard.SetDataObject(bmp);
            //MessageBox.Show("Image Copy On Clpboard!");


            //파일경로 저장
            FolderBrowserDialog dialog = new FolderBrowserDialog();
              
            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string selected = dialog.SelectedPath;
                    string chartName = (graphInfo.Name == null) ? graphInfo.Title : graphInfo.Name;
                    string filePath = selected + "\\" + chartName.Split(':')[0] + "-" + DateTime.Now.ToString("yy년MM월dd일-HH시mm분ss초") + ".jpg";
                    if (graphInfo.Type == GraphType.Histogram)
                    {
                        spcHistogram.SaveImage(filePath);
                    }
                    else
                    {
                        chart.SaveImage(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    MessageBox.Show("Image Copy Success!");
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolCboAngle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolCboAngle.Text == string.Empty)
                return;
         
            graphInfo.LabelAngleString = toolCboAngle.Text;
       
            // X축의 경사도를 지정합니다

            if (graphInfo.Type == GraphType.Histogram)
               return;

            chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.LabelStyle.Angle = graphInfo.LabelAngle;

            //toolBtn3D.CheckState = (graphInfo.View3D) ? CheckState.Checked : CheckState.Unchecked;

            //// LabelPoint의 경사도를 지정합니다.
            //if (graphInfo.Type == GraphType.Pareto)
            //{
            //    chart.Series["Percent"].SmartLabelStyle.Enabled = false;
            //    chart.Series["Percent"].LabelAngle = angle;
            //}
            //else if (graphInfo.Type == GraphType.BoxPlot)
            //{
            //    for (int i = 2; chart.Series.Count > i; i++)
            //    {
            //        chart.Series[i].SmartLabelStyle.Enabled = false;
            //        chart.Series[i].LabelAngle = angle;
            //    }
            //}
            //else
            //{
            //    {
            //        for (int j = 0; j < graphInfo.AxisY.Length; j++)
            //        {
            //            string strSeriesName = (graphInfo.AxisY[j].ColumnName == string.Empty) ? graphInfo.AxisY[j].ColumnID : graphInfo.AxisY[j].ColumnName;
            //            chart.Series[j].SmartLabelStyle.Enabled = false;
            //            chart.Series[j].LabelAngle = angle;
            //        }
            //    }
            //}    
        }

        private void toolBtnLegendBox_Click(object sender, EventArgs e)
        {
            graphInfo.LegendBox = !graphInfo.LegendBox;
            if (graphInfo.Type == GraphType.Pareto)
            {
                chart.Series["Percent"].IsVisibleInLegend = graphInfo.LegendBox;
                chart.Series["Cummulated"].IsVisibleInLegend = graphInfo.LegendBox;
            }
            else if (graphInfo.Type == GraphType.BoxPlot)
            {                  
                    chart.Series[0].IsVisibleInLegend = graphInfo.LegendBox;                      
            }
            else
            {
                for (int j = 0; j < graphInfo.AxisY.Length; j++)
                {
                    if (graphInfo.AxisX.ColumnName == "Fitted value")
                    {
                        chart.Series[1].IsVisibleInLegend = false;
                        chart.Series[0].IsVisibleInLegend = graphInfo.LegendBox;
                        break;
                    }
                    else
                    {
                        string strSeriesName = (graphInfo.AxisY[j].ColumnName == string.Empty) ? graphInfo.AxisY[j].ColumnID : graphInfo.AxisY[j].ColumnName;

                        chart.Series[j].IsVisibleInLegend = graphInfo.LegendBox;
                    }
                  
                
                }
            }               
            toolBtnLegendBox.CheckState = (graphInfo.LegendBox) ? CheckState.Checked : CheckState.Unchecked;
        }

        private void toolBtn3D_Click(object sender, EventArgs e)
        {          
            graphInfo.View3D = !graphInfo.View3D;
            chart.ChartAreas[Convert.ToString(graphInfo.Type)].Area3DStyle.Enable3D = graphInfo.View3D;
            toolBtn3D.CheckState = (graphInfo.View3D) ? CheckState.Checked : CheckState.Unchecked;
            
        }

        private void toolBtnZoom_Click(object sender, EventArgs e)
        {      
            toolBtnZoom.Checked = !toolBtnZoom.Checked;

            if (toolBtnZoom.Checked)
            {
                //if (graphInfo.Type == GraphType.BoxPlot)
                //{

                //}
                //else
                //{
                    chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.Zoom(0,2);
                    chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.Zoomable = true;
                    chart.ChartAreas[Convert.ToString(graphInfo.Type)].CursorX.AutoScroll = true;
                    chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScrollBar.LineColor = Color.Black;
                    chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScrollBar.Size = 17;                    
                    chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.SmallScrollSize = double.NaN;

               // }

            }
            else
            {
                chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.ZoomReset();
            }    
        }
    

        private void toolBtnPointLabel_Click(object sender, EventArgs e)
        {
            graphInfo.PointLabel = !graphInfo.PointLabel;

            if (graphInfo.Type == GraphType.Pareto)
            {
                chart.Series["Percent"].IsValueShownAsLabel = graphInfo.PointLabel;
            }
            else if(graphInfo.Type == GraphType.BoxPlot)
            {
                for (int i = 2; chart.Series.Count > i; i++)
                {
                    chart.Series[i].IsValueShownAsLabel = graphInfo.PointLabel;
                }
                   
            }
            else 
            {
                for (int j = 0; j < graphInfo.AxisY.Length; j++)
                {
                    chart.Series[j].IsValueShownAsLabel = graphInfo.PointLabel;
                }
            }
          
            toolBtnPointLabel.CheckState = (graphInfo.PointLabel) ? CheckState.Checked : CheckState.Unchecked;
        }

        private void toolBtnProperty_Click(object sender, EventArgs e)
        {
            toolBtnProperty.CheckState = CheckState.Unchecked;

            if (GraphUpdating != null)
            {
                GraphUpdating(graphInfo);
               DrawGraph(graphInfo);
            }
        }

        #endregion


        //protected override bool ProcessDialogKey(Keys keyData)
        //{
        //    // Avoid dialog processing of arrow keys
        //    if (keyData == Keys.Left || keyData == Keys.Right)
        //    {
        //        return false;
        //    }
        //    return base.ProcessDialogKey(keyData);
        //}

        //private void chart_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Right)
        //    {
        //        chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.Scroll(ScrollType.SmallIncrement);
        //    }
        //    else if (e.KeyCode == Keys.Left)
        //    {
        //        chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.Scroll(ScrollType.SmallDecrement);
        //    }
        //    else if (e.KeyCode == Keys.PageDown)
        //    {
        //        chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.Scroll(ScrollType.LargeIncrement);
        //    }
        //    else if (e.KeyCode == Keys.PageUp)
        //    {
        //        chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.Scroll(ScrollType.LargeDecrement);
        //    }
        //    else if (e.KeyCode == Keys.Home)
        //    {
        //        chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.Scroll(ScrollType.First);
        //    }
        //    else if (e.KeyCode == Keys.End)
        //    {
        //        chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.Scroll(ScrollType.Last);
        //    }          
        //}

        //private void chart_Click(object sender, EventArgs e)
        //{
        //    chart.Focus();
        //}

        //Point ptStartPint;

        //private void chart_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == System.Windows.Forms.MouseButtons.Left)
        //    {
        //        ptStartPint = e.Location;
        //    }
        //}

        //private void chart_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == System.Windows.Forms.MouseButtons.Left)
        //    {
        //        Point ptEndPoint = e.Location;

        //        if (toolBtnZoom.Checked)
        //        {
        //            chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisX.ScaleView.Zoom(ptStartPint.X, ptEndPoint.X);
        //            chart.ChartAreas[Convert.ToString(graphInfo.Type)].AxisY.ScaleView.Zoom(ptStartPint.Y, ptEndPoint.Y);
        //        }
        //    }
        //}

        private double calc_QUARTILE(double[] array, int iquart)
        {
            double d_quart_value = double.NaN;

            try
            {
                int ihp = 0;
                double dMid = double.NaN;

                if (array.Length % 2 == 0)
                {
                    ihp = array.Length / 2;
                    dMid = (double)(ihp + 0.5);
                }
                else
                {
                    ihp = (array.Length - 1) / 2;
                    dMid = (double)(ihp + 1);
                }

                switch (iquart)
                {
                    case 1:
                        if (ihp % 2 == 0)
                        {
                            d_quart_value = array[ihp / 2 - 1] + (array[ihp / 2] - array[ihp / 2 - 1]) / 4;
                        }
                        else
                        {
                            d_quart_value = array[(ihp + 1) / 2 - 1];
                        }
                        break;
                    case 2:
                        if(dMid == Math.Ceiling(dMid))  // 홀수
                        {
                            d_quart_value = array[(int)dMid - 1];
                        } else  // 짝수
                        {
                            d_quart_value = array[(int)Math.Truncate(dMid) - 1] + (array[(int)Math.Ceiling(dMid) - 1] - array[(int)Math.Truncate(dMid) - 1]) / 2;
                        }
                        break;
                    case 3 :
                        if(dMid == Math.Ceiling(dMid)) // 홀수
                        {
                            if (ihp % 2 == 0)
                            {
                                d_quart_value = array[((int)dMid + ihp / 2) - 1] + array[(int)dMid + ihp / 2] - (array[((int)dMid + ihp / 2) - 1]) / 4;
                            }
                            else
                            {
                                d_quart_value = array[((int)dMid + (ihp + 1) / 2) - 1];
                            }
                        }
                        else  // 짝수
                        {
                            if (ihp % 2 == 0)
                            {
                                d_quart_value = array[((int)Math.Truncate(dMid) + ihp / 2) - 1] + (array[(int)Math.Truncate(dMid) + ihp / 2] - array[((int)Math.Truncate(dMid) + ihp / 2) - 1]) * 3 / 4;
                            }
                            else
                            {
                                d_quart_value = array[((int)Math.Truncate(dMid) + (ihp + 1) / 2) - 1];
                            }
                        }
                        break;
                    default:
                        break;
                }
                
                return d_quart_value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
      #endregion