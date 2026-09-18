using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.SP.Common
{
    [Serializable]
    public class Section : ISectionItem
    {
        private string name = string.Empty;
        private string defaultvalue = string.Empty;
        /// <summary>
        /// Logical name of the Section
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return; // throw new ArgumentException("Section Name cannot be empty.");

                if (name == value)
                    return;

                string prevName = name;
                name = value;

                if (SectionRenamed != null)
                    SectionRenamed(this, prevName);
            }
        }
        public virtual string DefaultValue
        {
            get
            {
                return defaultvalue;
            }
            set
            {
                this.defaultvalue = value;
            }
        }
        public int LineStart { get; private set; }
        public int LineEnd { get; private set; }
        public int ColumnStart { get; private set; }
        public int ColumnEnd { get; private set; }

        public Section Parent { get; set; }

        public bool Force { get; set; }

        public ISectionItemCollection Items { get; private set; }

        protected Section()
        {
            Items = new ISectionItemCollection();

            Items.ItemAdded += new ItemAddedEventHandler<ISectionItem>(Items_ListItemAdded);
            Items.ItemRemoved += new ItemRemovedEventHandler<ISectionItem>(Items_ListItemRemoved);
        }

        void Items_ListItemRemoved(ISectionItem item)
        {
            item.Parent = null;
        }

        void Items_ListItemAdded(ISectionItem item)
        {
            item.Parent = this;
        }

        public override string ToString()
        {
            return Name;
        }

        public event ItemRenamedEventHandler<Section> SectionRenamed;
    }

    [Serializable]
    public class RegexSection : Section
    {
        RegexEntity startEntity;
        RegexEntity endEntity;

        public RegexEntity StartEntity
        {
            get { return startEntity; }
            set
            {
                try
                {
                    if (startEntity == value)
                        return;

                    startEntity = value;
                    startEntity.RegexChanged -= Entity_RegexChanged; // 기존에 추가했던 녀석일지도 모름
                    startEntity.RegexChanged += Entity_RegexChanged;

                    if (SectionBoundaryEntityChanged != null)
                        SectionBoundaryEntityChanged(this);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public RegexEntity EndEntity
        {
            get { return endEntity; }
            set 
            {
                try
                {
                    if (endEntity == value)
                        return;

                    endEntity = value;
                    endEntity.RegexChanged -= Entity_RegexChanged; // 기존에 추가했던 녀석일지도 모름
                    endEntity.RegexChanged += Entity_RegexChanged;

                    if (SectionBoundaryEntityChanged != null)
                        SectionBoundaryEntityChanged(this);
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }
            }
        }

        void Entity_RegexChanged(RegexEntity entity)
        {
            if (entity != startEntity && entity != endEntity)
                return;

            if (SectionBoundaryEntityChanged != null)
                SectionBoundaryEntityChanged(this);
        }

        public event SectionBoundaryEntityChangedEventHandler SectionBoundaryEntityChanged;

        //public virtual RegexEntity StartEntity { get; set; }
        //public virtual RegexEntity EndEntity { get; set; }
    }

    [Serializable]
    public class FixedWidthTableSection : Section
    {
        public int ColumnInterval { get; set; }
        public int RowInterval { get; set; }
    }

    public delegate void SectionBoundaryEntityChangedEventHandler(RegexSection section);
}
