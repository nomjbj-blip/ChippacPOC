using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DACrux.Base;
using System.ComponentModel;
using DACrux.Map;

namespace DACrux.MapAnalysis.Control
{
    /// <summary>
    /// TPUCMapView 에 대한 요약 설명입니다.
    /// </summary>
    public partial class TPUCMapControl : UserControl
    {
        #region [ Create & Close ]

        public TPUCMapControl( 
            string waferid,
            string waferseq
            )
        {
            InitializeComponent();

            WaferID = waferid;
            WaferSeq = waferseq;
            map.WaferID = waferid;
        }

        #endregion

        #region [ Event Handler ]
        private void m_wMap_OnChangeCurrentDie(
            object sender,
            Base.Die NewDie
            )
        {
            txtX.Text = NewDie.IndexX.ToString();
            txtY.Text = NewDie.IndexY.ToString();
            txtBin.Text = NewDie.BinNumber.ToString();
            txtValue.Text = NewDie.ParametricValue.ToString();

            DataRow[] drs = (map.DataSource as DataTable).Select(String.Format("[X] = '{0}' AND [Y] = '{1}'", NewDie.IndexX, NewDie.IndexY));
            if (drs != null && drs.Length > 0)
                txtDieNum.Text = drs[0]["DIE_NUM"].ToString();
            else
                txtDieNum.Text = string.Empty;
        }
        #endregion [ Event Handler ]

        #region [ Method ]

        public void DrawWafer()
        {
            map.WaferDrawMode = DACrux.Map.MapMode.Fit;
            map.Focus();
        }
        #endregion [ Method ]

        #region [ Property ]

        public WaferMap WaferMap
        {
            get { return map; }
        }

        public string WaferID
        {
            get;
            private set;
        }

        public string WaferSeq
        {
            get;
            private set;
        }
        #endregion [ Property ]
    }
}
