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
using DACrux.SP.Common;
using DACrux.SP.Controls;

namespace SmartParser.Designer
{
    public partial class frmDefaultTemplate : Form
    {
        #region " Member Field & Property "

        private Analysis analysis = null;

        #endregion

        #region " Creator "

        public frmDefaultTemplate()
        {
            DACrux.SP.Common.MultiLang funclang = new MultiLang();

            funclang.CheckLang();
            InitializeComponent();

            ResourceManager rm = new ResourceManager("SmartParser.Controls.res", System.Reflection.Assembly.Load("SmartParser.Controls"));

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(15, 15);
            imgList.Images.Add((Image)rm.GetObject("entity"));
            imgList.Images.Add((Image)rm.GetObject("section"));

            analysis = Analysis.GetInstance();

            analysis.Entities.ItemAdded += new ItemAddedEventHandler<RegexEntity>(EntitiesItemAdded);
            analysis.Entities.ItemRemoved += new ItemRemovedEventHandler<RegexEntity>(EntitiesItemRemoved);

            analysis.Sections.ItemAdded += new ItemAddedEventHandler<Section>(SectionsItemAdded);
            analysis.Sections.ItemRemoved += new ItemRemovedEventHandler<Section>(SectionsItemRemoved);
        }

        #endregion

        #region " Event Handler "

        protected override void OnLoad(EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
