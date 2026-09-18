using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    #region " DELEGATE "

    /// <summary>
    /// Delegate
    /// </summary>
    //public delegate void TextChangedHandler(string strChanged);

    #endregion

    /// <summary>
    /// Class Name : SmartSelector<br/>
    /// Author     : Miracom Hyungsuk, Yang<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class SmartSelector2 : UserControl
    {
        #region " MEMBER FIELD "

        public SmartListbox2 slbx;
        private readonly static int MINUMUM_LISTBOX_WIDTH = 150;
        private readonly static int DEFAULT_CAPTION_WIDTH = 88;
        private Point position = new Point(0, 0);
        private bool bMultiSelect = true;

        public event EventHandler DropDown;
        public event EventHandler TextChangeNotifyEvent;

        List<int> lstSelectedIndices = new List<int>();

        #endregion

        #region " PROPERTY "

        private Color color;
     //   [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Color ColorProperty
        {
            get { return color; }
            set { color = value; }
        }
        private Bitmap bm;
      //  [Editor(typeof(System.Drawing.Design.BitmapEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Bitmap BitmapProperty
        {
            get { return bm; }
            set { bm = value; }
        }

        /// <summary>
        /// Gets or Sets title string for Listbox and Caption of this Selector.
        /// </summary>
        [Category("Setup")]
        public string Caption
        {
            get
            {
                return slbx.Caption;
            }
            set
            {
                lblTitle.Text = "     " + value;
                slbx.Caption = value;
            }
        }
        
        /// <summary>
        /// Gets or Sets the width of Caption.
        /// </summary>
        [Category("Setup")]
        public int CaptionWidth
        {
            get { return lblTitle.Width; }
            set
            {
                if (value < 1)
                    lblTitle.Width = DEFAULT_CAPTION_WIDTH;
                else
                    lblTitle.Width = value;
            }
        }
        
        /// <summary>
        /// Gets or Sets whether shows image or not.
        /// </summary>
        [Category("Setup")]
        public bool ShowImage
        {
            get { return lblTitle.ShowImage; }
            set { lblTitle.ShowImage = value; }
        }

        /// <summary>
        /// Gets or Sets Datasource of Listbox Items.
        /// </summary>
        [Category("Setup")]
        [Browsable(false)]
        public DataTable DataSource
        {
            get
            {
                return slbx.DataSource;
            }
            set
            {
                slbx.DataSource = value;
                if(bMultiSelect) txtContents.Text = "All";
            }
        }

        /// <summary>
        /// Gets or Sets whether user can select multi item or single item.
        /// </summary>
        [Category("Setup")]
        public bool MultiSelect
        {
            get
            {
                return bMultiSelect;
            }
            set
            {
                bMultiSelect = value;
                slbx.MultiSelect = value;

                if (value)
                    txtContents.BackColor = Color.Lavender;
                else
                    txtContents.BackColor = Color.White;
            }
        }

        [Browsable(false)]
        public string AllItems
        {
            get
            {
                bool bInialized = false;
                string strAllItems = string.Empty;

                try
                {
                    if (slbx.DataSource == null)
                    {
                        //slbx.DataSource = GetDataTableBy(oControlType);
                    }
                    else
                        bInialized = true;

                    strAllItems = slbx.GetAllItemString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (!bInialized)
                        slbx.DataSource = null;
                }
                return strAllItems;
            }
        }

        public new string Text
        {
            get
            {
                return txtContents.Text;
            }
            set
            {
                txtContents.Text = value;
            }
        }

        string strQuery = string.Empty;

        /// <summary>
        /// Gets Selected Item Indices
        /// </summary>
        [Browsable(false)]
        public List<int> SelectedIndices
        {
            get { return lstSelectedIndices; }
        }

        public bool ReadOnly
        {
            get { return txtContents.ReadOnly; }
            set { txtContents.ReadOnly = value; }
        }

        #endregion

        #region " CREATOR "

        /// <summary>
        /// Initalize Class
        /// </summary>
        public SmartSelector2()
        {
            InitializeComponent();

            slbx = new SmartListbox2();
        }

        #endregion

        #region " EVENT HANDLER "

        /// <summary>
        /// Show ListView For Selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>N/A</returns>
        private void btnListUp_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!slbx.Visible)
                {
                    if (DropDown != null)
                        DropDown(this, e);

                    if (slbx.DataSource != null && slbx.DataSource.Rows.Count > 0)
                    {
                        slbx.Location = this.PointToScreen(new Point(txtContents.Left, txtContents.Top + txtContents.Height + 2));
                        slbx.Width = (slbx.m_maxWidth < MINUMUM_LISTBOX_WIDTH) ? MINUMUM_LISTBOX_WIDTH + 10 : slbx.m_maxWidth + 10;
                        slbx.Visible = !(slbx.Visible);
                        
                    }
                    else
                    {
                        MessageBox.Show("No Data exists.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
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

        /// <summary>
        /// SmartSelector Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>N/A</returns>
        private void SmartSelector_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.ParentForm != null)
                    this.ParentForm.Cursor = Cursors.WaitCursor;
                else
                    this.Cursor = Cursors.WaitCursor;
                if (bMultiSelect)
                {
                    if (slbx.DataSource.Rows.Count != 0)
                    {
                        txtContents.Text = "All";
                    }
                    else
                    {
                        txtContents.Text = "";
                    }
                }
                else
                {
                    if (txtContents.Text == "All")
                    {
                        txtContents.Text = string.Empty;
                    }
                }

                    

                slbx.Visible = false;

                this.txtContents.TextChanged += new System.EventHandler(this.txtContents_TextChanged);
                this.slbx.VisibleChanged += new EventHandler(slbx_VisibleChanged);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.ParentForm != null)
                    this.ParentForm.Cursor = Cursors.Default;
                else
                    this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Make contents to All when It's empty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContents_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ParentForm != null)
                    this.ParentForm.Cursor = Cursors.WaitCursor;
                else
                    this.Cursor = Cursors.WaitCursor;
                if (txtContents.Text.Trim() == string.Empty && bMultiSelect)
                {
                    if (slbx.DataSource.Rows.Count != 0)
                    {
                        txtContents.Text = "All";
                    }
                    else
                    {
                        txtContents.Text = "";
                    }
                    //txtContents.Text = "All";
                }
                else if (txtContents.Text.Trim() == string.Empty)
                    txtContents.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.ParentForm != null)
                    this.ParentForm.Cursor = Cursors.Default;
                else
                    this.Cursor = Cursors.Default;
            }
        }

        void slbx_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (slbx.Visible == false)
                {
                    if (slbx.DataSource == null || slbx.DataSource.Rows.Count == 0)
                    {
                        if (bMultiSelect)
                        {
                            if (slbx.DataSource.Rows.Count != 0)
                            {
                                txtContents.Text = "All";
                            }
                            else
                            {
                                txtContents.Text = "";
                            }
                        }
                        else
                            txtContents.Text = string.Empty;

                        lstSelectedIndices.Clear();
                    }
                    else
                    {
                        txtContents.Text = slbx.GetSelectedString();
                        slbx.GetSelectedIndices(ref lstSelectedIndices);
                    }

                    if (TextChangeNotifyEvent != null)
                        TextChangeNotifyEvent(this, e);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion

        #region " METHOD "

        #region Reset

        /// <summary>
        /// Reset All Data
        /// </summary>
        public void Reset()
        {
            if (bMultiSelect)
                txtContents.Text = "All";
            else
                txtContents.Text = "";

            slbx.ClearItems();
            slbx.DataSource = null;
        }
        #endregion

        #endregion
    }
}