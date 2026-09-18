using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DACrux.SP.Common;
using System.Resources;

namespace DACrux.SP.Controls
{
    public partial class RegexSectionControl : UserControl
    {
        #region " Member Field & Property "

        enum ListViewType { Entity, Section, Origin };

        private RegexSection section = null;

        private Analysis analysis = Analysis.GetInstance();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RegexSection SectionContent
        {
            get
            {
                try
                {
                    if (section == null)
                    {
                        section = new RegexSection();
                        section.SectionRenamed += section_SectionRenamed;
                    }

                    section.Name = txtName.Text;
                    section.StartEntity = cboFirstEntity.SelectedItem as RegexEntity;
                    section.EndEntity = cboLastEntity.SelectedItem as RegexEntity;
                    //section.Parent = txtParent.Tag as Section;

                    foreach (ListViewItem item in lvItems.Items)
                        section.Items.Add(item.Tag as ISectionItem);

                    return section;
                }
                catch (Exception ex)
                {
                    Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
                    return null;
                }
                finally
                {
                    section = null;
                }
            }
            set
            {
                try
                {
                    SetListView(ListViewType.Origin);

                    if (value == null)
                    {
                        ResetControls();
                        section = null;
                        return;
                    }

                    txtName.Text = value.Name;
                    txtParent.Text = (value.Parent == null) ? string.Empty : value.Parent.Name;
                    //txtParent.Tag = (value.Parent == null) ? null : value.Parent;

                    SetItemListView(value.Items);
                    RefreshPositionEntityItem();

                    cboFirstEntity.SelectedItem = value.StartEntity;
                    cboLastEntity.SelectedItem = value.EndEntity;

                    section = value;
                }
                catch (Exception ex)
                {
                    Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
                    ResetControls();
                    section = null;
                }
            }
        }

      
        public string name
        {
            get
            {
                return this.lblName.Text;
            }
            set
            {
                    this.lblName.Text = value;
            }
        }

        public string parentsection
        {
            get
            {
                return this.label5.Text;
            }
            set
            {
                     this.label5.Text = value;
            }
        }

        public string entrylist
        {
            get
            {
                return this.label1.Text;
            }
            set
            {
                    this.label1.Text = value;
            }
        }

        public string sectionlist
        {
            get
            {
                    return this.label2.Text;
            }
            set
            {
                    this.label2.Text = value;
            }
        }

        public string itemlist
        {
            get
            {
                    return this.label3.Text;
            }
            set
            {
                    this.label3.Text = value;
            }
        }

        public string firstentry
        {
            get
            {
                    return this.label6.Text;
            }
            set
            {
                    this.label6.Text = value;
            }
        }

        public string endentry
        {
            get
            {
                    return this.label7.Text;
            }
            set
            {
                this.label7.Text = value;
            }
        }

        public string mustsection
        {
            get
            {
                    return this.chkMustExists.Text;
            }
            set
            {
                    this.chkMustExists.Text = value;
            }
        }

        public string save
        {
            get
            {
                    return this.btnSave.Text;
            }
            set
            {
                    this.btnSave.Text = value;
            }
        }
        
        public string remove
        {
            get
            {
                    return this.btnRemove.Text;
            }
            set
            {
                    this.btnRemove.Text = value;
            }
        }
       
        public string find
        {
            get
            {
                    return this.btnFind.Text;
            }
            set
            {
                    this.btnFind.Text = value;
            }
        }


        #endregion

        #region " Creator "

        public RegexSectionControl()
        {
            //DACrux.SP.Common.MutiLang funclang = new MutiLang();

            //funclang.CheckLnag();
            InitializeComponent();

            btnSave.Click += new EventHandler(btnSave_Click);
            btnRemove.Click += new EventHandler(btnRemove_Click);
            btnFind.Click += new EventHandler(btnFind_Click);

            ResourceManager rm = new ResourceManager("DACrux.SP.Controls.res", System.Reflection.Assembly.Load("DACrux.SP.Controls"));

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(15, 15);
            imgList.Images.Add((Image)rm.GetObject("entity"));
            imgList.Images.Add((Image)rm.GetObject("section"));

            lvEntity.SmallImageList = imgList;
            lvSection.SmallImageList = imgList;
            lvItems.SmallImageList = imgList;

            analysis.Sections.ItemAdded += new ItemAddedEventHandler<Section>(Sections_ItemAdded);
            analysis.Sections.ItemRemoved += new ItemRemovedEventHandler<Section>(Sections_ItemRemoved);
            analysis.Entities.ItemAdded += new ItemAddedEventHandler<RegexEntity>(Entities_ItemAdded);
            analysis.Entities.ItemRemoved += new ItemRemovedEventHandler<RegexEntity>(Entities_ItemRemoved);

            SizeChanged += new EventHandler(Control_SizeChanged);

            InitListView();
        }

        #endregion

        #region " Event Handler "

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                SetListView(ListViewType.Origin);
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void Entities_ItemRemoved(RegexEntity entity)
        {
            try
            {
                if (!(entity.Parent == null))
                    return;

                lvEntity.Items.RemoveByKey(entity.Name);
                lvItems.Items.RemoveByKey(entity.Name);
                entity.EntityRenamed -= entity_EntityRenamed;

                RefreshPositionEntityItem();
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void Entities_ItemAdded(RegexEntity entity)
        {
            try
            {
                if (!(entity.Parent == null))
                    return;

                lvEntity.Items.Add(GetEntityListViewItem(entity));
                entity.EntityRenamed += entity_EntityRenamed;
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void Sections_ItemRemoved(Section item)
        {
            try
            {
                if (!(item is RegexSection) || !(item.Parent == null))
                    return;

                lvSection.Items.RemoveByKey(item.Name);
                lvItems.Items.RemoveByKey(item.Name); // 만약 lvItems에 같은 이름의 Entity가 있다면?
                item.SectionRenamed -= section_SectionRenamed;

                RefreshPositionEntityItem();
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void Sections_ItemAdded(Section item)
        {
            try
            {
                if (!(item is RegexSection) || !(item.Parent == null))
                    return;

                lvSection.Items.Add(GetSectionListViewItem(item));
                item.SectionRenamed += section_SectionRenamed;
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (FindRequested != null)
                    FindRequested((RegexEntity)cboFirstEntity.SelectedItem, (RegexEntity)cboLastEntity.SelectedItem);
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboFirstEntity.SelectedItem == null)
                {
                    Utility.ShowMessageBox("First Entity must exists.", MessageBoxIcon.Information);
                    return;
                }

                if (string.IsNullOrEmpty(txtName.Text))
                {
                    Utility.ShowMessageBox("Please input the section name.", MessageBoxIcon.Information);
                    txtName.Focus();
                    return;
                }

                if (SaveRequested != null)
                {
                    SaveRequested(SectionContent);
                    ResetControls();
                    section = null;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (RemoveRequested != null)
                {
                    RemoveRequested(SectionContent);
                    ResetControls();
                    section = null;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }

        }

        void section_SectionRenamed(Section item, string prevName)
        {
            ListViewItem lvItem;

            try
            {
                lvItem = lvSection.Items[prevName];

                if (lvItem != null)
                    lvItem.Name = lvItem.Text = item.Name;

                lvItem = lvItems.Items[prevName];

                if (lvItem != null)
                    lvItem.Name = lvItem.Text = item.Name;
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void entity_EntityRenamed(Entity item, string prevName)
        {
            ListViewItem lvItem;

            try
            {
                lvItem = lvEntity.Items[prevName];

                if (lvItem != null)
                    lvItem.Name = lvItem.Text = item.Name;

                lvItem = lvItems.Items[prevName];

                if (lvItem != null)
                    lvItem.Name = lvItem.Text = item.Name;

                RefreshPositionEntityItem();
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void Control_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (Width < 100)
                    return;

                lvEntity.Columns[0].Width = lvEntity.Width - 20;
                lvSection.Columns[0].Width = lvSection.Width - 20;
                lvItems.Columns[0].Width = lvItems.Width - 20;
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        #region [ ListView DragDrop ]

        private ListView lvDragSource;

        void ListViewItem_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) && e.Effect == DragDropEffects.Move)
            {
                Utility.MoveListViewItems(lvDragSource, (ListView)sender, (IList)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));

                if (lvDragSource == lvItems || (ListView)sender == lvItems)
                    RefreshPositionEntityItem();
            }

            lvDragSource = null;
        }

        void TargetListView_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource != lvItems)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void OriginListView_DragEnter(object sender, DragEventArgs e)
        {
            if (lvDragSource != null && lvDragSource == lvItems)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void ListViewItem_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvDragSource = (ListView)sender;
            lvDragSource.DoDragDrop(lvDragSource.SelectedItems, DragDropEffects.Move);
        }

        void OriginListViewItem_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView lvSource = sender as ListView;

            if (lvSource.SelectedItems.Count > 0 && e.Button == MouseButtons.Left)
                Utility.MoveListViewItem(lvSource, lvItems, lvSource.SelectedItems[0]);

            RefreshPositionEntityItem();
        }

        #endregion

        #endregion

        #region " Method "

        #region [ Origin ListView ]

        private void SetListView(ListViewType listViewType)
        {
            switch (listViewType)
            {
                case ListViewType.Entity:

                    lvEntity.Items.Clear();

                    var entityList = from Entity entity in analysis.Entities
                                     where entity is RegexEntity && entity.Parent == null
                                     select entity;

                    foreach (RegexEntity entity in entityList)
                    {
                        lvEntity.Items.Add(GetEntityListViewItem(entity));
                        entity.EntityRenamed -= entity_EntityRenamed;
                        entity.EntityRenamed += entity_EntityRenamed;
                    }

                    break;
                case ListViewType.Section:

                    lvSection.Items.Clear();

                    var sectionList = from Section section in analysis.Sections
                                      where section is RegexSection && section.Parent == null
                                      select section;

                    foreach (RegexSection section in sectionList)
                    {
                        lvSection.Items.Add(GetSectionListViewItem(section));
                        section.SectionRenamed -= section_SectionRenamed;
                        section.SectionRenamed += section_SectionRenamed;
                    }

                    break;
                case ListViewType.Origin:
                    SetListView(ListViewType.Entity);
                    SetListView(ListViewType.Section);
                    break;
            }
        }

        private ListViewItem GetEntityListViewItem(Entity entity)
        {
            ListViewItem item = new ListViewItem();
            item.Text = item.Name = entity.Name;
            item.ImageIndex = 0;
            item.Tag = entity;

            return item;
        }

        private ListViewItem GetSectionListViewItem(Section section)
        {
            ListViewItem item = new ListViewItem();
            item.Text = item.Name = section.Name;
            item.ImageIndex = 1;
            item.Tag = section;

            return item;
        }

        private ListViewItem GetSectionItemListViewItem(ISectionItem item)
        {
            ListViewItem lvItem = new ListViewItem();
            lvItem.Text = lvItem.Name = item.Name;
            lvItem.ImageIndex = (item is Entity) ? 0 : 1;
            lvItem.Tag = item;

            return lvItem;
        }

        #endregion

        #region [ Target ListView (lvItems) ]

        private void RefreshPositionEntityItem()
        {
            var validItemList = from ListViewItem item in lvItems.Items
                                where item.Tag is RegexEntity
                                select item.Tag as RegexEntity;

            RegexEntity entityFirst = cboFirstEntity.SelectedItem as RegexEntity;
            RegexEntity entityLast = cboLastEntity.SelectedItem as RegexEntity;

            cboFirstEntity.Items.Clear();
            cboLastEntity.Items.Clear();

            cboFirstEntity.Items.AddRange(validItemList.ToArray());
            cboLastEntity.Items.AddRange(validItemList.ToArray());

            if (entityFirst != null && validItemList.Contains(entityFirst))
                cboFirstEntity.SelectedItem = entityFirst;

            if (entityLast != null && validItemList.Contains(entityLast))
                cboLastEntity.SelectedItem = entityLast;
        }

        private void SetItemListView(ISectionItemCollection sectionItemList)
        {
            ListViewItem lvItem = null;

            lvItems.Items.Clear();

            var itemList = from ISectionItem item in sectionItemList
                           where item.Parent == null
                           select item;

            foreach (ISectionItem sectionItem in sectionItemList)
            {
                lvItem = new ListViewItem();
                lvItem.Name = lvItem.Text = sectionItem.Name;
                lvItem.Tag = sectionItem;
                lvItem.ImageIndex = (sectionItem is RegexEntity) ? 0 : 1;
                lvItems.Items.Add(lvItem);
            }
        }

        private void InitListView()
        {
            ListView[] arrLv = new ListView[] { lvEntity, lvSection, lvItems };

            foreach (ListView lv in arrLv)
            {
                lv.HeaderStyle = ColumnHeaderStyle.None;
                lv.Columns.Add("Name", lv.Width - 20);
                lv.AllowDrop = true;
            }

            lvEntity.MultiSelect = true;
            lvSection.MultiSelect = true;
            lvItems.MultiSelect = false;

            this.lvEntity.MouseDoubleClick += new MouseEventHandler(OriginListViewItem_MouseDoubleClick);
            this.lvSection.MouseDoubleClick += new MouseEventHandler(OriginListViewItem_MouseDoubleClick);

            this.lvEntity.ItemDrag += new ItemDragEventHandler(ListViewItem_ItemDrag);
            this.lvSection.ItemDrag += new ItemDragEventHandler(ListViewItem_ItemDrag);
            this.lvItems.ItemDrag += new ItemDragEventHandler(ListViewItem_ItemDrag);

            this.lvEntity.DragEnter += new DragEventHandler(OriginListView_DragEnter);
            this.lvSection.DragEnter += new DragEventHandler(OriginListView_DragEnter);
            this.lvItems.DragEnter += new DragEventHandler(TargetListView_DragEnter);

            this.lvEntity.DragDrop += new DragEventHandler(ListViewItem_DragDrop);
            this.lvSection.DragDrop += new DragEventHandler(ListViewItem_DragDrop);
            this.lvItems.DragDrop += new DragEventHandler(ListViewItem_DragDrop);
        }

        #endregion

        #region [ Etc ]

        private void ResetControls()
        {
            this.Reset();
            SetListView(ListViewType.Origin);
        }

        #endregion

        #endregion

        #region " Event "

        public event SectionFindRequestedEventHandler FindRequested;
        public event RegexSectionSaveRequestedEventHandler SaveRequested;
        public event RegexSectionRemoveRequestedEventHandler RemoveRequested;

        #endregion

    }

    public delegate void RegexSectionSaveRequestedEventHandler(RegexSection entity);
    public delegate void RegexSectionRemoveRequestedEventHandler(RegexSection entity);
}
