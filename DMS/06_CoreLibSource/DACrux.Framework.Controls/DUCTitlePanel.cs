using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.ComponentModel.Design;

namespace DACrux.Framework.Controls
{
    [Designer(typeof(DUCSearchPanelDesigner))]
    [Docking(DockingBehavior.Ask)]
    public partial class DUCTitlePanel : UserControl
    {
        #region [ PROPERTY ]

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        public Panel ControlRect
        {
            get { return pnlCtrlRect; }
        }

        [Category("Layout")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        #region [ Appearance ]

        [Category("Appearance")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                pnlCtrlRect.BackColor = base.BackColor;
            }
        }

        [Category("Appearance")]
        [Localizable(true)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set
            {
                base.BackgroundImage = value;
                pnlCtrlRect.BackgroundImage = base.BackgroundImage;
            }
        } 

        #endregion

        #region [ Title Setup ]

        [Category("Title Setup")]
        [DefaultValue("Form Title")]
        public string Title
        {
            get { return lblTitle.Text; }
            set
            {
                lblTitle.Text = value;

                Graphics g = lblTitle.CreateGraphics();
                pnlLabel.Width = lblIcon.Width + (int)g.MeasureString(lblTitle.Text, lblTitle.Font).Width + 10;
                g.Dispose();
                g = null;
            }
        }

        [Category("Title Setup")]
        [DefaultValue("●")]
        public string SpecialCharacter
        {
            get { return lblIcon.Text; }
            set { lblIcon.Text = value; }
        }

        [Category("Title Setup")]
        [DefaultValue(true)]
        public bool CollapseButton
        {
            get { return pnlOption.Visible; }
            set { pnlOption.Visible = value; }
        }

        [Category("Title Setup")]
        [DefaultValue(false)]
        public bool Collapse
        {
            get { return !pnlCtrlRect.Visible; }
            set
            {
                pnlCtrlRect.Visible = !value;

                if (pnlCtrlRect.Visible)
                {
                    this.Height = m_Heigth;
                    this.btnCollapse.Image = global::DACrux.Framework.Controls.Properties.Resources.Collapse_large;
                }
                else
                {
                    m_Heigth = this.Height;
                    this.Height = pnlTop.Height;
                    this.btnCollapse.Image = global::DACrux.Framework.Controls.Properties.Resources.Expand_large;
                }
            }
        }

        [Category("Title Setup")]
        [Localizable(true)]
        public Image SpecialCharacterImage
        {
            get { return lblIcon.Image; }
            set { lblIcon.Image = value; }
        }

        [Category("Title Setup")]
        public Font TitleFont
        {
            get { return lblTitle.Font; }
            set
            {
                lblTitle.Font = value;
                lblIcon.Font = lblTitle.Font;
            }
        }

        [Category("Title Setup")]
        public Color TitleForeColor
        {
            get { return lblTitle.ForeColor; }
            set
            {
                lblTitle.ForeColor = value;
                lblIcon.ForeColor = lblTitle.ForeColor;
            }
        }

        [Category("Title Setup")]
        public Color TitleBackColor
        {
            get { return pnlTop.BackColor; }
            set { pnlTop.BackColor = value; }
        }

        [Category("Title Setup")]
        public Image TitleBackgroundImage
        {
            get { return pnlTop.BackgroundImage; }
            set { pnlTop.BackgroundImage = value; }
        }

        [Category("Title Setup")]
        public int TitleHeight
        {
            get { return pnlTop.Height; }
            set
            {
                pnlTop.Height = value;

                if (pnlTop.Height <= btnCollapse.Height)
                {
                    btnCollapse.Location = new Point(btnCollapse.Location.X, pnlTop.Location.Y);
                }
                else
                {
                    int nYPos = (int)((pnlTop.Height - btnCollapse.Height) / 2);
                    if (((pnlTop.Height - btnCollapse.Height) % 2) == 1)
                        nYPos++;
                    btnCollapse.Location = new Point(btnCollapse.Location.X, pnlTop.Location.Y + nYPos);
                }
            }
        } 
        
        #endregion

        #region [ Button Setup ]
        [Category("Button Setup")]
        [DefaultValue(false)]
        public bool SearchButton
        {
            get { return btnSearch.Visible; }
            set { btnSearch.Visible = value; }
        }

        [Category("Button Setup")]
        [DefaultValue(false)]
        public bool CreateButton
        {
            get { return btnCreate.Visible; }
            set { btnCreate.Visible = value; }
        }

        [Category("Button Setup")]
        [DefaultValue(false)]
        public bool DeleteButton
        {
            get { return btnDelete.Visible; }
            set { btnDelete.Visible = value; }
        }

        [Category("Button Setup")]
        [DefaultValue(false)]
        public bool EditButton
        {
            get { return btnEdit.Visible; }
            set { btnEdit.Visible = value; }
        }

        [Category("Button Setup")]
        [DefaultValue(false)]
        public bool SaveButton
        {
            get { return btnSave.Visible; }
            set { btnSave.Visible = value; }
        }

        [Category("Button Setup")]
        [DefaultValue(false)]
        public bool OKButton
        {
            get { return btnOK.Visible; }
            set { btnOK.Visible = value; }
        }

        [Category("Button Setup")]
        [DefaultValue(false)]
        public bool CloseButton
        {
            get { return btnClose.Visible; }
            set { btnClose.Visible = value; }
        }
        #endregion

        #endregion

        #region [ MEMBER FIELD ]
        private int m_Heigth = 0;
        #endregion

        #region [ METHOD ]

        public DUCTitlePanel()
        {
            InitializeComponent();
        }

        private void DUCSearchPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            try
            {
                e.Control.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            try
            {
                this.Collapse = !this.Collapse;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region [ EVENT ]

        public event EventHandler SearchButton_Click = null;
        public event EventHandler CreateButton_Click = null;
        public event EventHandler EditButton_Click = null;
        public event EventHandler DeleteButton_Click = null;
        public event EventHandler SaveButton_Click = null;
        public event EventHandler OKButton_Click = null;
        public event EventHandler CloseButton_Click = null;

        #endregion

        private void Button_Click(object sender, EventArgs e)
        {
            try
            {
                switch (((Button)sender).Name.ToUpper())
                {
                    case "BTNSEARCH":
                        if (SearchButton_Click != null)
                            SearchButton_Click(sender, e);
                        break;

                    case "BTNCREATE":
                        if (CreateButton_Click != null)
                            CreateButton_Click(sender, e);
                        break;

                    case "BTNEDIT":
                        if (EditButton_Click != null)
                            EditButton_Click(sender, e);
                        break;

                    case "BTNDELETE":
                        if (DeleteButton_Click != null)
                            DeleteButton_Click(sender, e);
                        break;

                    case "BTNSAVE":
                        if (SaveButton_Click != null)
                            SaveButton_Click(sender, e);
                        break;

                    case "BTNOK":
                        if (OKButton_Click != null)
                            OKButton_Click(sender, e);
                        break;

                    case "BTNCLOSE":
                        if (CloseButton_Click != null)
                            CloseButton_Click(sender, e);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    internal class DUCSearchPanelDesigner : System.Windows.Forms.Design.ParentControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            DUCTitlePanel sp = component as DUCTitlePanel;
            this.EnableDesignMode(sp.ControlRect, "ControlRect");
        }

    }


}
