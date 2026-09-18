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

namespace DACrux.SEMDMS.ENGUI
{
    public partial class frmDefectColoring : DACrux.Framework.Base.DACruxUXBasic01
    {
        public frmDefectColoring()
        {
            InitializeComponent();
        }


        #region  "ColorBySize_Load"

        private void frmDefectColoring_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            try
            {
                ViewColorListBySize(DACrux.Base.GlobalVariable.UserID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }


        #endregion

        #region  "Color"

        public void SpreadBackColor(FarPoint.Win.Spread.SheetView sv, int colNo)
        {
            string color = string.Empty;
            int r = 0;
            int g = 0;
            int b = 0;
            for (int i = 0; i < sv.RowCount; i++)
            {
                color = (string)sv.Cells[i, colNo].Value;
                r = DACrux.Base.Convert.intParse(color.Substring(0, 3));
                g = DACrux.Base.Convert.intParse(color.Substring(3, 3));
                b = DACrux.Base.Convert.intParse(color.Substring(6, 3));
                sv.Cells[i, colNo].BackColor = Color.FromArgb(r, g, b);
            }
        }

        #endregion

        #region  "Color Box"

        // Color Box
        Color ColorDialog(Color orig)
        {
            ColorDialog cd = null;
            try
            {
                cd = new ColorDialog();
                cd.Color = picBoxSizeColor.BackColor;
                if (cd.ShowDialog(this) != DialogResult.OK) return orig;
                return cd.Color;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cd != null) cd.Dispose();
                cd = null;
            }
        }

        #endregion

        #region  "Color To RGB"

        //  Color To RGB
        string ColorToRgbString(Color c)
        {
            return string.Format("{0}{1}{2}"
                , string.Format("{0}", c.R).PadLeft(3, '0')
                , string.Format("{0}", c.G).PadLeft(3, '0')
                , string.Format("{0}", c.B).PadLeft(3, '0')
                );
        }

        #endregion

        #region "Color Box (Size) Click"

        private void picBoxSizeColor_Click(object sender, EventArgs e)
        {
            try
            {
                picBoxSizeColor.BackColor = ColorDialog(picBoxSizeColor.BackColor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        #endregion

        #region  "View Color List By Size"

        void ViewColorListBySize(string user)
        {
            SEMConfiguration oSEMConfiguration = null;
            DataTable dataList = null;

            try
            {
                oSEMConfiguration = new SEMConfiguration();

                dataList = oSEMConfiguration.SelectColorListByDftSize(user);

                if (dataList.Rows.Count == 0)
                {
                    dataList.Dispose();
                    dataList = null;
                    dataList = oSEMConfiguration.SelectColorListByDftSize("admin");
                }

                Utility.FPSpreadUtil.InitSpread(fpSpread_Size);
                DACrux.Utility.FPSpreadUtil.SetSpreadData(dataList, fpSpread_Size_Sheet);
                //fpSpread_Size_Sheet.DataSource = dataList;
                //fpSpread_Size_Sheet.ColumnHeaderAutoText.ToString();
                Utility.FPSpreadUtil.SetAutoColumnWidth(fpSpread_Size_Sheet);

                SpreadBackColor(fpSpread_Size_Sheet, 2);
          }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                if (dataList != null) dataList.Dispose();
                dataList = null;
            }
        }

        #endregion

        #region  "Create Click"

        //Create Click
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSizeFrom.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input From Size.", this.Name);
                    return;
                }
                if (txtSizeTo.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please input To Size.", this.Name);
                    return;
                }
                int itemp = 0;
                if (int.TryParse(txtSizeFrom.Text, out itemp) == false)
                {
                    MessageBox.Show(this, "Please input only number.(Size From)", this.Name);
                    return;
                }
                if (int.TryParse(txtSizeTo.Text, out itemp) == false)
                {
                    MessageBox.Show(this, "Please input only number.(Size To)", this.Name);
                    return;
                }

                fpSpread_Size_Sheet.Rows.Add(fpSpread_Size_Sheet.RowCount, 1);
                fpSpread_Size_Sheet.Cells[fpSpread_Size_Sheet.RowCount - 1, 0].Value = decimal.Parse(txtSizeFrom.Text);
                fpSpread_Size_Sheet.Cells[fpSpread_Size_Sheet.RowCount - 1, 1].Value = decimal.Parse(txtSizeTo.Text);
                fpSpread_Size_Sheet.Cells[fpSpread_Size_Sheet.RowCount - 1, 2].Value = string.Format("{0}{1}{2}"
                    , string.Format("{0}", picBoxSizeColor.BackColor.R).PadLeft(3, '0')
                    , string.Format("{0}", picBoxSizeColor.BackColor.G).PadLeft(3, '0')
                    , string.Format("{0}", picBoxSizeColor.BackColor.B).PadLeft(3, '0')
                    );
                fpSpread_Size_Sheet.Cells[fpSpread_Size_Sheet.RowCount - 1, 2].BackColor = picBoxSizeColor.BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        #endregion

        #region  "Update Click"

        //  Update Click
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                FarPoint.Win.Spread.Model.CellRange cr = fpSpread_Size_Sheet.GetSelection(0);
                if (cr.Row == -1) return;

                fpSpread_Size_Sheet.Cells[cr.Row, 0].Value = decimal.Parse(txtSizeFrom.Text);
                fpSpread_Size_Sheet.Cells[cr.Row, 1].Value = decimal.Parse(txtSizeTo.Text);
                fpSpread_Size_Sheet.Cells[cr.Row, 2].Value = string.Format("{0}{1}{2}"
                    , string.Format("{0}", picBoxSizeColor.BackColor.R).PadLeft(3, '0')
                    , string.Format("{0}", picBoxSizeColor.BackColor.G).PadLeft(3, '0')
                    , string.Format("{0}", picBoxSizeColor.BackColor.B).PadLeft(3, '0')
                    );
                fpSpread_Size_Sheet.Cells[cr.Row, 2].BackColor = picBoxSizeColor.BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        #endregion

        #region "Delete Click"

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Remove?", this.Name, MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                FarPoint.Win.Spread.Model.CellRange cr = fpSpread_Size_Sheet.GetSelection(0);
                if (cr.Row == -1) return;
                fpSpread_Size_Sheet.Rows.Remove(cr.Row, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        #endregion

        #region  "Size Save Click"

        //  Size Save Click
        private void btnSizeSave_Click(object sender, EventArgs e)
        {
            DACrux.SEMDMS.RO.SEMConfiguration oConfigure = null;
            int[] seq = null;
            int[] from = null;
            int[] to = null;
            Color[] color = null;

            try
            {
                if (MessageBox.Show(this, "Save?", this.Name, MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                seq = new int[fpSpread_Size_Sheet.RowCount];
                from = new int[fpSpread_Size_Sheet.RowCount];
                to = new int[fpSpread_Size_Sheet.RowCount];
                color = new Color[fpSpread_Size_Sheet.RowCount];

                for (int i = 0; i < fpSpread_Size_Sheet.RowCount; i++)
                {
                    seq[i] = i + 1;
                    from[i] = System.Convert.ToInt32(fpSpread_Size_Sheet.Cells[i, 0].Value);
                    to[i] = System.Convert.ToInt32(fpSpread_Size_Sheet.Cells[i, 1].Value);
                    //from[i] = (int)(decimal)fpSpread_Size_Sheet.Cells[i, 0].Value;
                    //to[i] = (int)(decimal)fpSpread_Size_Sheet.Cells[i, 1].Value;
                    color[i] = fpSpread_Size_Sheet.Cells[i, 2].BackColor;
                }

                oConfigure = new DACrux.SEMDMS.RO.SEMConfiguration();
                oConfigure.DeleteColorByDftSize(DACrux.Base.GlobalVariable.UserID);

                if (fpSpread_Size_Sheet.RowCount == 0) return;
                oConfigure.CreateColorByDftSize(DACrux.Base.GlobalVariable.UserID, seq, from, to, color);

                MessageBox.Show(this, "Save.", this.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
            finally
            {
                oConfigure = null;
                seq = null;
                from = null;
                to = null;
                color = null;
            }
        }

        #endregion


        #region  "fpSpread_Size_SelectionChanged"

        //  fpSpread_Size_SelectionChanged
        private void fpSpread_Size_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            if (e.Range.Row == -1) return;
            try
            {
                txtSizeFrom.Text = string.Format("{0}", fpSpread_Size_Sheet.Cells[e.Range.Row, 0].Value);
                txtSizeTo.Text = string.Format("{0}", fpSpread_Size_Sheet.Cells[e.Range.Row, 1].Value);
                picBoxSizeColor.BackColor = fpSpread_Size_Sheet.Cells[e.Range.Row, 2].BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }

            if (e.Range.Column != 2) return;
            try
            {
                fpSpread_Size_Sheet.Cells[e.Range.Row, e.Range.Column].BackColor = ColorDialog(fpSpread_Size_Sheet.Cells[e.Range.Row, e.Range.Column].BackColor);
                picBoxSizeColor.BackColor = fpSpread_Size_Sheet.Cells[e.Range.Row, e.Range.Column].BackColor;
                fpSpread_Size_Sheet.Cells[e.Range.Row, e.Range.Column].Value = ColorToRgbString(picBoxSizeColor.BackColor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Name);
            }
        }

        #endregion

        private void btnView_Click(object sender, EventArgs e)
        {
            ViewColorListBySize(DACrux.Base.GlobalVariable.UserID);
        }
    }
}
