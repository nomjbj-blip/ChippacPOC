using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace DACrux.ProjectManager.UI
{
    [Serializable]
    public sealed class GraphInformation
    {
        #region " MEMBER FIELD "
        [Serializable]
        public struct ColumnInfoItem
        {
            public int ColumnIndex;
            public string ColumnID;
            public string ColumnName;
            public Type ColumnType;

            public ColumnInfoItem(int columnIndex, string columnID, string columnName, Type type)
            {
                ColumnIndex = columnIndex;
                ColumnID = columnID;
                ColumnName = columnName;
                ColumnType = type;
            }
        };

        public List<ColumnInfoItem> ColumnInfoItems = new List<ColumnInfoItem>();

        private string name = string.Empty;
        private GraphType type = GraphType.None;
        private DataTable dataSource = null;
        private DataSet dataSource4Taguchi = null;

        private ColumnInfoItem axisX;
        private ColumnInfoItem series;

        private List<ColumnInfoItem> axisY = new List<ColumnInfoItem>();

        private string axisXTitle = string.Empty;
        private string axisYTitle = string.Empty;

        private bool is3DMode = false;
        private bool isForceZero = true;
        private bool isAxisXForceZero = true;
        private bool isSerLegBox = false;
        private bool isPointLabel = false;
        private short labelAngle = 0;
        private short pointSize = 5;
        private Color pointColor = new Color();
        private short barSize = 10;
        private int decimalPlace = 2;
        private int decimalPlaceX = 0;

        private double usl = double.NaN;
        private double target = double.NaN;
        private double lsl = double.NaN;
        private bool isNormalLine = true;
        private bool isSpecLimit = true;
        private bool is3SigmaLine = true;
        private bool isFrequence = true;
        private bool isGridLine = true;
        private bool isMeanBoxplot = false;

        private bool isRegressionEquation = false;   // Scatter Chart의 SubTitle에 회귀식 표현유무

        private DateTimeFormat dateTimeFormat;
        private string createdInformation = string.Empty;

        private string imagePath = string.Empty;
        private Size imageSize = new Size(0, 0);
        private bool isDrawn = false;
        private bool isImageWithTitles = true;

        private string title = string.Empty;
        private string subTitle = string.Empty;

        private bool isPropertyBtnView = false;

        #endregion

        #region " PROPERTY "

        public bool ViewProperty
        {
            get { return isPropertyBtnView; }
            set { isPropertyBtnView = value; }
        }
        
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; }
        }

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        public Size ImageSize
        {
            get { return imageSize; }
            set { imageSize = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public GraphType Type
        {
            get { return type; }
            set { type = value; }
        }

        public DataTable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public DataSet DataSource4Taguchi
        {
            get { return dataSource4Taguchi; }
            set { dataSource4Taguchi = value; }
        }

        public bool PointLabel
        {
            get { return isPointLabel; }
            set { isPointLabel = value; }
        }

        public bool View3D
        {
            get { return is3DMode; }
            set { is3DMode = value; }
        }

        public bool ForceZero
        {
            get { return isForceZero; }
            set { isForceZero = value; }
        }

        public bool AxisXForceZero
        {
            get { return isAxisXForceZero; }
            set { isAxisXForceZero = value; }
        }

        public bool LegendBox
        {
            get { return isSerLegBox; }
            set { isSerLegBox = value; }
        }

        public int DecimalPlace
        {
            get { return decimalPlace; }
            set { decimalPlace = value; }
        }

        public int DecimalPlaceX
        {
            get { return decimalPlaceX; }
            set { decimalPlaceX = value; }
        }

        public double USL
        {
            get { return usl; }
            set { usl = value; }
        }

        public double Target
        {
            get { return target; }
            set { target = value; }
        }

        public double LSL
        {
            get { return lsl; }
            set { lsl = value; }
        }

        public bool NormalLine
        {
            get { return isNormalLine; }
            set { isNormalLine = value; }
        }

        public bool SpecLimit
        {
            get { return isSpecLimit; }
            set { isSpecLimit = value; }
        }

        public bool View3SigmaLine
        {
            get { return is3SigmaLine; }
            set { is3SigmaLine = value; }
        }

        public bool Frequence
        {
            get { return isFrequence; }
            set { isFrequence = value; }
        }

        public bool GridLine
        {
            get { return isGridLine; }
            set { isGridLine = value; }
        }

        public ColumnInfoItem AxisX
        {
            get { return axisX; }
            set { axisX = value; }
        }

        public ColumnInfoItem Series
        {
            get { return series; }
            set { series = value; }
        }

        public ColumnInfoItem[] AxisY
        {
            get { return axisY.ToArray(); }
            set
            {
                axisY.Clear();
                axisY.AddRange(value);
            }
        }

        public short LabelAngle
        {
            get { return labelAngle; }
            set { labelAngle = value; }
        }

        public string LabelAngleString
        {
            get 
            {
                string strAngle = string.Empty;

                switch(labelAngle)
                {
                    case 0:
                        strAngle = "Horizontal";
                        break;
                    case 45:
                        strAngle = "Diagonal";
                        break;
                    case 90:
                        strAngle = "Vertical";
                        break;
                }

                return strAngle; 
            }
            set 
            {
                string strAngle = value;

                switch(strAngle)
                {
                    case "Horizontal":
                        labelAngle = 0;
                        break;
                    case "Diagonal":
                        labelAngle = 45;
                        break;
                    case "Vertical":
                        labelAngle = 90;
                        break;
                }
            }
        }

        public short PointSize
        {
            get { return pointSize; }
            set { pointSize = value; }
        }

        public Color PointColor
        {
            get { return pointColor; }
            set { pointColor = value; }
        }

        public short BarSize
        {
            get { return barSize; }
            set { barSize = value; }
        }

        public string AxisXTitle
        {
            get { return axisXTitle; }
            set { axisXTitle = value; }
        }

        public string AxisYTitle
        {
            get { return axisYTitle; }
            set { axisYTitle = value; }
        }

        public string CreatedInformation
        {
            get { return createdInformation; }
            set { createdInformation = value; }
        }

        public string DateTimeFormatString
        {
            get { return Common.DateTimeFormats[(int)dateTimeFormat]; }
        }

        public DateTimeFormat DateTimeFormat
        {
            get { return dateTimeFormat; }
            set { dateTimeFormat = value; }
        }

        public bool RegressionEquation
        {
            get { return isRegressionEquation; }
            set { isRegressionEquation = value; }
        }

        public bool IsDrawn
        {
            get { return isDrawn; }
            set { isDrawn = value; }
        }

        public bool IsImageWithTitles
        {
            get { return isImageWithTitles; }
            set { isImageWithTitles = value; }
        }

        public bool IsMeanBoxplot
        {
            get { return isMeanBoxplot; }
            set { isMeanBoxplot = value; }
        }

        #endregion

        #region " CREATOR "

        public GraphInformation()
        {

        }

        public GraphInformation(GraphType oType)
        {
            type = oType;

            switch(type)
            {
                case GraphType.Bar:
                    break;
                case GraphType.Line:
                    break;
                case GraphType.Pie:
                    break;
                case GraphType.Scatter:
                    isForceZero = false;
                    isAxisXForceZero = false;
                    break;
                case GraphType.Pareto:
                    break;
                case GraphType.BoxPlot:
                    isForceZero = false;
                    break;
                case GraphType.Histogram:
                    isSpecLimit = false;
                    break;
            }
        }

        #endregion

        #region " METHOD "

        public void AddAxisY(ColumnInfoItem axisYItem)
        {
            if (!axisY.Contains(axisYItem))
            {
                axisY.Add(axisYItem);
            }
        }

        public void ClearAxisY()
        {
            axisY.Clear();
        }

        #endregion
    }
}
