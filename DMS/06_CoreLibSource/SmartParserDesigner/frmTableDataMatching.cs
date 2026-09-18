using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Resources;
using DACrux.SP.Controls;
using DACrux.SP.Common;

namespace SmartParser.Designer
{
    public partial class frmTableDataMatching : Form
    {
        #region " Member Field & Property "

        private Analysis analysis = null;
        private OracleConnector oc = null;

        #endregion

        #region " Creator "

        public frmTableDataMatching()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();

            ResourceManager rm = new ResourceManager("SmartParser.Controls.res", System.Reflection.Assembly.Load("SmartParser.Controls"));

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(15, 15);
            imgList.Images.Add((Image)rm.GetObject("table"));

            lvTable.SmallImageList = imgList;

            lvTable.Columns.Add("Name", 70);
            lvTable.Columns.Add("Type", 50);

            pnlTableList.Resize += new EventHandler(panel1_Resize);

            analysis = Analysis.GetInstance();

            analysis.Entities.ItemAdded += new ItemAddedEventHandler<RegexEntity>(EntitiesItemAdded);
            analysis.Entities.ItemRemoved += new ItemRemovedEventHandler<RegexEntity>(EntitiesItemRemoved);

            analysis.Sections.ItemAdded += new ItemAddedEventHandler<Section>(SectionsItemAdded);
            analysis.Sections.ItemRemoved += new ItemRemovedEventHandler<Section>(SectionsItemRemoved);
        }

        void panel1_Resize(object sender, EventArgs e)
        {
            try
            {
                if (pnlTableList.Width < 120)
                    return;

                lvTable.Columns[1].Width = 50;
                lvTable.Columns[0].Width = pnlTableList.Width - lvTable.Columns[1].Width - 20;
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region " Event Handler "

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                oc = new OracleConnector(analysis.ConnectionInfo);
                
                DataTable dt = oc.GetTableList();
                
                SetTableList(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetTableList(DataTable dtScheme)
        {
            DataTable dtTableList = DACrux.SP.Common.Utility.GetDistinctTable(dtScheme, DACrux.SP.Common.Utility.SortTypes.Asc, Literal.TABLE_NAME, Literal.TABLE_TYPE);

            DataTable dt;
            foreach (DataRow row in dtTableList.Rows)
            {
                dt = oc.GetTableSheme(row[Literal.TABLE_NAME].ToString());

                lvTable.Items.Add(GetTableListViewItem(row[Literal.TABLE_NAME].ToString(), row[Literal.TABLE_TYPE].ToString(), dt));
            }
        }

        private ListViewItem GetTableListViewItem(string tableName, string tableType, DataTable dtTable)
        {
            ListViewItem item = new ListViewItem();
            item.Text = item.Name = tableName;
            item.SubItems.Add(tableType);
            item.ImageIndex = 0;
            item.Tag = dtTable;

            return item;
        }

        void SectionsItemAdded(Section section)
        {
            try
            {
                section.SectionRenamed += new ItemRenamedEventHandler<Section>(SectionRenamed);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void SectionsItemRemoved(Section section)
        {
            try
            {
                section.SectionRenamed -= new ItemRenamedEventHandler<Section>(SectionRenamed);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void SectionRenamed(Section item, string prevName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void EntitiesItemAdded(RegexEntity entity)
        {
            try
            {
                entity.EntityRenamed += new ItemRenamedEventHandler<RegexEntity>(EntityRenamed);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void EntitiesItemRemoved(RegexEntity entity)
        {
            try
            {
                entity.EntityRenamed -= new ItemRenamedEventHandler<RegexEntity>(EntityRenamed);
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void EntityRenamed(RegexEntity item, string prevName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        void tabSetup_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                //if (tabSetup.Width < 100)
                //    return;

                //lvEntity.Columns[1].Width = 40;
                //lvEntity.Columns[0].Width = tabSetup.Width - lvEntity.Columns[1].Width - 20;

                //lvSection.Columns[1].Width = 40;
                //lvSection.Columns[0].Width = tabSetup.Width - lvSection.Columns[1].Width - 20;
            }
            catch (Exception ex)
            {
                DACrux.SP.Common.Utility.ShowMessageBox(ex.Message, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region " Method "


        
        #endregion
    }
}
