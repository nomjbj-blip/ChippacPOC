using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DACrux.SEMDMS.RO;
using DACrux.Base;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectColorDefine_Property
        : DACrux.Framework.Base.DACruxUXBasic01
    {
        #region [ Constructor ]
        private enum ColumnIndex { SIZE_SEQ = 0, SIZE_FROM, SIZE_TO, COLOR }
        private readonly string ConfigUserID = "ADMIN";

        public frmDefectColorDefine_Property()
        {
            InitializeComponent();
        }

        #endregion [ Constructor ]

        //------------------------------------------------------------------------------------

        #region [ Event Handler ]

        //--

        private void btnSearch_Click(
            object sender,
            EventArgs e
            )
        {
            ViewColorListBySize(
                //DACrux.Base.GlobalVariable.UserID
                ConfigUserID
                );
        }

        //--

        private void frmDefectColorDefine_Property_Load(
            object sender,
            EventArgs e
            )
        {
            if (DesignMode)
                return;

            ViewColorListBySize(
                //DACrux.Base.GlobalVariable.UserID
                ConfigUserID
                );

        }

        //--

        private void grid_CommandButtonClick(
            object sender,
            Framework.PropertyGrid.CommandEventArgs e
            )
        {
            RO.SEMConfiguration oSemConfig = new SEMConfiguration();
            string sizeSeq = grid.GetValue("SIZE_SEQ").ToString();
            string strColor = grid.GetValue("COLOR").ToString();

            bool exists = oSemConfig.ExistsDefectSizeByColor(
                ConfigUserID,
                sizeSeq
                );

            DialogResult result;
            switch (e.Mode)
            {
                case Framework.PropertyGrid.CommandMode.Insert:
                    if (exists)
                    {
                        MessageBox.Show("해당 데이터가 이미 존재합니다.");
                        e.Cancel = true;
                        return;
                    }

                    //--

                    result = MessageBox.Show("저장하시겠습니까?", "입력", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        oSemConfig.InsertDefectSizeByColor(
                            //DACrux.Base.GlobalVariable.UserID,
                            ConfigUserID,
                            sizeSeq,
                            grid.GetValue("SIZE_FROM").ToString(),
                            grid.GetValue("SIZE_TO").ToString(),
                            strColor //ColorTranslator.ToHtml(color)
                            );
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;

                case Framework.PropertyGrid.CommandMode.Update:

                    if (!exists)
                    {
                        MessageBox.Show("업데이트할 데이터가 없습니다.");
                        e.Cancel = true;
                        return;
                    }

                    //--

                    result = MessageBox.Show("변경하시겠습니까", "수정", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        oSemConfig.UpdateDefectSizeByColor(
                            //DACrux.Base.GlobalVariable.UserID,
                            ConfigUserID,
                            sizeSeq,
                            grid.GetValue("SIZE_FROM").ToString(),
                            grid.GetValue("SIZE_TO").ToString(),
                            strColor     //ColorTranslator.ToHtml(color)
                            );
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;

                case Framework.PropertyGrid.CommandMode.Delete:

                    if (!exists)
                    {
                        MessageBox.Show("삭제할 데이터가 없습니다.");
                        e.Cancel = true;
                        return;
                    }

                    //--

                    result = MessageBox.Show("삭제하시겠습니까", "삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        oSemConfig.DeleteDefectSizeByColor(
                            //DACrux.Base.GlobalVariable.UserID,
                            ConfigUserID,
                            sizeSeq
                            );
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;

                case Framework.PropertyGrid.CommandMode.None:
                default:
                    break;
            }
        }

        //--

        private void grid_CommandComplete(
            object sender,
            Framework.PropertyGrid.CommandEventArgs e
            )
        {
            btnSearch.PerformClick();
        }

        //--

        private void grid_StateChanged(
            object sender,
            Framework.PropertyGrid.StateChangedEventArgs e
            )
        {
            if (e.State == Framework.PropertyGrid.State.Insert)
            {
                grid.SetValue("SIZE_SEQ", (long)fpSpread_Size_Sheet.Rows.Count + 1);
            }
        }

        //--

        #endregion [ Event Handler ]

        //------------------------------------------------------------------------------------

        #region [ Method ]

        //--

        public void SpreadBackColor(
            FarPoint.Win.Spread.SheetView sv,
            int colNo
            )
        {
            string color = string.Empty;
            for (int i = 0; i < sv.RowCount; i++)
            {
                color = (string)sv.Cells[i, colNo].Value;
                sv.Cells[i, colNo].BackColor = ColorTranslator.FromHtml(color);
            }
        }

        //--

        private void ViewColorListBySize(
            string userid
            )
        {
            Utility.FPSpreadUtil.InitSpread(fpSpread_Size);

            SEMConfiguration oSemConfig = new SEMConfiguration();
            DataTable dt = oSemConfig.SelectColorListByDftSize(userid);

            Utility.FPSpreadUtil.SetSpreadData(dt, fpSpread_Size_Sheet, 100, (int)decimalPlaces.Value);
            Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread_Size_Sheet);
            NumberCellType numberCell = new NumberCellType();
            numberCell.DecimalPlaces = (int)decimalPlaces.Value;
            fpSpread_Size_Sheet.Columns[(int)ColumnIndex.SIZE_FROM].CellType = numberCell;
            fpSpread_Size_Sheet.Columns[(int)ColumnIndex.SIZE_TO].CellType = numberCell;

            grid.DataSource = dt;

            SpreadBackColor(fpSpread_Size_Sheet, (int)ColumnIndex.COLOR);
        }

        //--

        #endregion [ Method ]

        //------------------------------------------------------------------------------------
    }
}
