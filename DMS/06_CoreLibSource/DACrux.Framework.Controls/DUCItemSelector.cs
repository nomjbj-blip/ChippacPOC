/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : DUCItemSelector.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2013.01.14
--  Description     : DACrux Framework에서 사용하는 Default Control 
--  History         : Created by YSIM at 2013.01.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2015 년 DACrux V5 History Start
 * 2015-03-27 : YSIM -> Drop Down List에 Item이 없을경우 Dropdown List Box가 나타나지 않게 함
 ----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.Controls.Properties;
using System.Windows.Forms.Design;

namespace DACrux.Framework.Controls
{
    public delegate void ItemSelected(object sender, ListView.SelectedListViewItemCollection SelectedViewItems);

    //[Designer(typeof(FixedHeightDUCItemSelect),typeof(ControlDesigner))]
    public partial class DUCItemSelector : UserControl
    {
        #region [ MEMBER FIELD ]

        public event EventHandler DropDown;
        public event EventHandler TextChangeNotifyEvent;
        public event ItemSelected OnItemSelected;
        public new event EventHandler OnTextChanged;
        public frmDUCListBox slbx = new frmDUCListBox();


        private bool m_bUseAll = true;
        private bool m_bSearchVisible = true;
        private string m_strDefault = "";
        EntryModeCollection m_oEntry = EntryModeCollection.Normal;
        #endregion
        
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                lbl_title.Font = base.Font;
                txtSelect.Font = base.Font;
            }
        }

        public Color TextBoxBackColor
        {
            set
            {
                pnlSearch.BackColor = value;
            }
            get
            {
                return pnlSearch.BackColor;
            }
        }


        public CharacterCasing CharacterCasing { get { return txtSelect.CharacterCasing; } set { txtSelect.CharacterCasing = value; } }

        [Category("Label Setup")]
        public Font LabelFont
        {
            get { return lbl_title.Font; }
            set { lbl_title.Font = value; }
        }


        [Category("TextBox Setup")]
        [Browsable(true)]
        public new string Text
        {
            set
            {
                txtSelect.Text = value;
            }
            get { return txtSelect.Text; }
        }

        [Category("TextBox Setup")]
        [Browsable(true)]
        public string DefaultText
        {
            set
            {
                m_strDefault = value;
                if (slbx.lvList.SelectedItems == null || slbx.lvList.SelectedItems.Count == 0)
                    txtSelect.Text = m_strDefault;
            }
            get { return m_strDefault; }
        }


        [Category("TextBox Setup")]
        public bool SearchVisible
        {
            get { return m_bSearchVisible; }
            set
            {
                m_bSearchVisible = value;
                DUCItemSelector_VisibleChanged(null, null);
            }
        }

        [Category("TextBox Setup")]
        public bool ReadOnly
        {
            get {
                return txtSelect.ReadOnly; 
            }
            set {
                txtSelect.ReadOnly = value;
            }
        }


        [Category("TextBox Setup")]
        public Font EntryFont
        {
            get { return txtSelect.Font; }
            set { txtSelect.Font = value; }
        }


        [Category("TextBox Setup")]
        public new Image BackgroundImage
        {
            get { return butSearch.BackgroundImage; }
            set { butSearch.BackgroundImage = value; }
        }

        [Category("TextBox Setup")]
        public bool UseAll
        {
            get
            {
                return m_bUseAll;
            }
            set
            {
                m_bUseAll = value;
            }
        }

        [Browsable(false)]
        public ListView.SelectedListViewItemCollection SelectedItems
        {
            get
            {
                return slbx.lvList.SelectedItems;
            }
        }

        [Browsable(false)]
        public ListView.CheckedListViewItemCollection CheckedItems
        {
            get
            {
                return slbx.lvList.CheckedItems;
            }
        }

        [Browsable(false)]
        public ListView.SelectedIndexCollection SelectedIndices
        {
            get
            {
                return slbx.lvList.SelectedIndices;
            }
        }

        [Browsable(false)]
        public ListView.ListViewItemCollection Items
        {
            get
            {
                return slbx.lvList.Items;
            }
        }


        [Category("Data")]
        public DataTable DataSource
        {
            get 
            {
                if (slbx == null) return null;
                return slbx.DataSource;
            }
            set 
            {
                if (slbx != null)
                    slbx.DataSource = value;

                if (value == null) txtSelect.Text = m_strDefault;
            }
        }

        [Category("Data")]
        public bool MultiSelect
        {
            get 
            {
                return slbx.MultiSelect;
            }
            set 
            {
                slbx.MultiSelect = value; 
            }
        }


        [Browsable(false)]
        public string DisplayColumn
        {
            get
            {
                return slbx.DisplayColumn;
            }
            set
            {
                slbx.DisplayColumn = value;
            }
        }

        [Browsable(false)]
        public string ValueTextColumn
        {
            get
            {
                return slbx.ValueTextColumn;
            }
            set
            {
                slbx.ValueTextColumn = value;
            }
        }

        [Browsable(false)]
        public string ValueColumn
        {
            get
            {
                return slbx.ValueColumn;
            }
            set
            {
                slbx.ValueColumn = value;
            }
        }

        public string Label
        {
            set
            {
                lbl_title.Text = "     " + value;
            }
            get
            {
                return lbl_title.Text.Trim();
            }
        }

        public int LabelWidth
        {
            set
            {
                lbl_title.Width = value;
            }
            get
            {
                return lbl_title.Width;
            }
        }

        [Browsable(true)]
        public EntryModeCollection EntryMode
        {
            set
            {
                m_oEntry = value;
                SetImage();
            }
            get
            {
                return m_oEntry;
            }
        }

        #region [ Create ]

        public DUCItemSelector()
        {
            InitializeComponent();
        }

        #endregion

        private void DUCItemSelector_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            try
            {
                this.slbx.VisibleChanged +=new EventHandler(slbx_VisibleChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void slbx_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (slbx.Visible == false)
                {
                    this.Focus();

                    List<string> tmpSelected = new List<string>();
                    if (this.MultiSelect == false && slbx.lvList.SelectedItems != null && slbx.lvList.SelectedItems.Count > 0)
                    {
                        for (int i = 0; i < slbx.lvList.SelectedItems.Count; i++)
                            tmpSelected.Add(slbx.lvList.SelectedItems[i].SubItems[slbx.ValueTextColumn].Text);
                        this.Text = string.Join(",", tmpSelected.ToArray());
                    }
                    else if (this.MultiSelect == true && slbx.lvList.CheckedItems != null && slbx.lvList.CheckedItems.Count > 0)
                    {
                        for (int i = 0; i < slbx.lvList.CheckedItems.Count; i++)
                            tmpSelected.Add(slbx.lvList.CheckedItems[i].SubItems[slbx.ValueTextColumn].Text);
                        this.Text = string.Join(",", tmpSelected.ToArray());
                    }
                    else if (slbx.Clear)
                    {
                        this.Text = "";
                    }
                    else
                    {
                        this.Text = (this.Text.Length > 0) ? this.Text : m_strDefault;
                    }

                    if (OnItemSelected != null)
                    {
                        OnItemSelected(this, slbx.lvList.SelectedItems);
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


        private void butSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (DropDown != null)
                    DropDown(this, e);

                // 2015-03-27 : YSIM -> Drop Down List에 Item이 없을경우 Dropdown List Box가 나타나지 않게 함
                ////////////////////////////////////////////////////////////////////////////////////////////////////// 
                if (slbx.DataSource == null || slbx.lvList.Items.Count == 0)
                {
                    return;
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////// 

                this.Cursor = Cursors.WaitCursor;

                Point ptTopLeft = this.PointToScreen(new Point(pnlSearch.Left, pnlSearch.Top + pnlSearch.Height + 2));
                Point ptBottomLeft = this.PointToScreen(new Point(pnlSearch.Left, pnlSearch.Top + pnlSearch.Height + slbx.Height + 2));

                if (!Screen.FromPoint(ptTopLeft).WorkingArea.Contains(ptBottomLeft))
                {
                    slbx.Location = this.PointToScreen(new Point(pnlSearch.Left, pnlSearch.Top - slbx.Height - 2));
                }
                else
                {
                    slbx.Location = ptTopLeft;
                }

                slbx.CharacterCasing = txtSelect.CharacterCasing;
                slbx.Width = Math.Min(Math.Max(pnlSearch.Width, slbx.Width), (pnlSearch.Width * 2));
                slbx.Caption = this.Label;
                slbx.Show();
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


        private void SetImage()
        {
            try
            {
                switch (m_oEntry)
                {
                    case EntryModeCollection.Normal:
                        lbl_title.Image = Resources.StopHS;
                        break;
                    case EntryModeCollection.Key:
                        lbl_title.Image = Resources.Change_Password;
                        break;
                    case EntryModeCollection.Filter:
                        lbl_title.Image = Resources.Filter;
                        break;
                    case EntryModeCollection.Info:
                        lbl_title.Image = Resources.Information;
                        break;
                    case EntryModeCollection.Search:
                        lbl_title.Image = Resources.Search;
                        break;
                    case EntryModeCollection.Spot:
                        lbl_title.Image = Resources.RecordHS;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DUCItemSelector_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                lbl_title.Visible = true;
                pnlSearch.Visible = true;
                txtSelect.Visible = true;

                butSearch.Visible = m_bSearchVisible;
                if (txtSelect.Text.Trim().Length == 0)
                    txtSelect.Text = m_strDefault;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                if(TextChangeNotifyEvent != null)
                TextChangeNotifyEvent(sender, e);
                e.Handled = true;
            }
        }

        private void txtSelect_TextChanged(object sender, EventArgs e)
        {
            if (OnTextChanged != null) OnTextChanged(sender, e);
        }

    }

    public class FixedHeightDUCItemSelect : ControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
        }

        public override AccessibleObject AccessibilityObject
        {
            get
            {
                return base.AccessibilityObject;
            }
        }

        public override SelectionRules SelectionRules 
        {
            get
            {
                return SelectionRules.RightSizeable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable;
            }
        }
    }
}

