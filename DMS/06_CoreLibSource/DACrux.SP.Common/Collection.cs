using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DACrux.SP.Common;

namespace DACrux.SP.Common
{
    [Serializable]
    public sealed class EntityCollection : List<RegexEntity>
    {
        public bool Exists(string name)
        {
            return this.Exists(entity => entity.Name.Equals(name));
        }

        public RegexEntity Find(string name)
        {
            return this.Find(entity => entity.Name.Equals(name));
        }

        public RegexEntity this[string name]
        {
            get { return Find(name); }
        }

        public new void Add(RegexEntity entity)
        {
            if (Exists(entity.Name))
                return;
           
            else
            {

                base.Add(entity);

                if (ItemAdded != null)
                    ItemAdded(entity);
            }
        }

        public new void AddRange(IEnumerable<RegexEntity> entities)
        {
            foreach (RegexEntity entity in entities)
                Add(entity);
        }

        public new bool Remove(RegexEntity entity)
        {
            bool bResult = base.Remove(this[entity.Name]);

            if (ItemRemoved != null)
                ItemRemoved(entity);

            return bResult;
        }

        public new void RemoveAll(Predicate<RegexEntity> match)
        {
            var list = from RegexEntity e in base.ToArray()
                       where match(e)
                       select e;

            foreach (RegexEntity entity in list)
                Remove(entity);
        }

        public new void RemoveAt(int index)
        {
            Remove(base[index]);
        }

        public new void RemoveRange(int index, int count)
        {
            for (int i = index + count; i >= index; i--)
                RemoveAt(i);
        }

        public event ItemAddedEventHandler<RegexEntity> ItemAdded;
        public event ItemRemovedEventHandler<RegexEntity> ItemRemoved;
    }

    /// <summary>
    /// Analysis의 Sections
    /// </summary>
    [Serializable]
    public sealed class SectionCollection : List<Section>
    {
        public bool Exists(string name)
        {
            return this.Exists(entity => entity.Name.Equals(name));
        }

        public Section Find(string name)
        {
            return this.Find(entity => entity.Name.Equals(name));
        }

        public Section this[string name]
        {
            get { return Find(name); }
        }

        public new void Add(Section section)
        {
            if (Exists(section.Name))
                return;
            else
            {

                base.Add(section);

                if (ItemAdded != null)
                    ItemAdded(section);

            }
        }

        public new void AddRange(IEnumerable<Section> sections)
        {
            foreach (Section section in sections)
                Add(section);
        }

        public new bool Remove(Section section)
        {
            bool bResult = base.Remove(this[section.Name]);

            foreach (ISectionItem child in section.Items)
                child.Parent = null;

            if (ItemRemoved != null)
                ItemRemoved(section);

            return bResult;
        }

        public new void RemoveAll(Predicate<Section> match)
        {
            var list = from Section e in base.ToArray()
                       where match(e)
                       select e;

            foreach (Section section in list)
                Remove(section);
        }

        public new void RemoveAt(int index)
        {
            Remove(base[index]);
        }

        public new void RemoveRange(int index, int count)
        {
            for (int i = index + count; i >= index; i--)
                RemoveAt(i);
        }

        public event ItemAddedEventHandler<Section> ItemAdded;
        public event ItemRemovedEventHandler<Section> ItemRemoved;
    }

    /// <summary>
    /// Section의 Items
    /// </summary>
    [Serializable]
    public sealed class ISectionItemCollection : List<ISectionItem>
    {
        public bool Exists(string name)
        {
            return this.Exists(item => item.Name.Equals(name));
        }

        public ISectionItem Find(string name)
        {
            return this.Find(item => item.Name.Equals(name));
        }

        public ISectionItem this[string name]
        {
            get { return Find(name); }
        }

        public new void Add(ISectionItem item)
        {
            if (Exists(item.Name))
                return;
            else
            {
                base.Add(item);

                if (ItemAdded != null)
                    ItemAdded(item);
            }
        }

        public new void AddRange(IEnumerable<ISectionItem> items)
        {
            foreach (ISectionItem item in items)
                Add(item);
        }

        public new bool Remove(ISectionItem item)
        {
            bool bResult = base.Remove(this[item.Name]);

            //foreach (ISectionItem child in this)
            //    child.Parent = null;

            if (ItemRemoved != null)
                ItemRemoved(item);

            return bResult;
        }

        public new void RemoveAll(Predicate<ISectionItem> match)
        {
            var list = from ISectionItem item in base.ToArray()
                       where match(item)
                       select item;

            foreach (ISectionItem item in list)
                Remove(item);
        }

        public new void RemoveAt(int index)
        {
            Remove(base[index]);
        }

        public new void RemoveRange(int index, int count)
        {
            for (int i = index + count; i >= index; i--)
                RemoveAt(i);
        }

        public event ItemAddedEventHandler<ISectionItem> ItemAdded;
        public event ItemRemovedEventHandler<ISectionItem> ItemRemoved;
    }

    [Serializable]
    public sealed class ITaskCollection : List<ITask>
    {
        public bool Exists(string name)
        {
            return this.Exists(task => task.Name.Equals(name));
        }

        public ITask Find(string name)
        {
            return this.Find(task => task.Name.Equals(name));
        }

        public ITask this[string name]
        {
            get { return Find(name); }
        }

        public new void Add(ITask task)
        {
            if (Exists(task.Name))
                return;
            else
            {
                base.Add(task);

                if (ItemAdded != null)
                    ItemAdded(task);
            }
        }

        public new void AddRange(IEnumerable<ITask> tasks)
        {
            foreach (ITask task in tasks)
                Add(task);
        }

        public new bool Remove(ITask task)
        {
            bool bResult = base.Remove(this[task.Name]);

            if (ItemRemoved != null)
                ItemRemoved(task);

            return bResult;
        }

        //public new void RemoveAll(Predicate<ITask> match)
        //{
        //    var list = from Task e in base.ToArray()
        //               where match(e)
        //               select e;

        //    foreach (ITask task in list)
        //        Remove(task);
        //}

        public new void RemoveAt(int index)
        {
            Remove(base[index]);
        }

        public new void RemoveRange(int index, int count)
        {
            for (int i = index + count; i >= index; i--)
                RemoveAt(i);
        }

        public event ItemAddedEventHandler<ITask> ItemAdded;
        public event ItemRemovedEventHandler<ITask> ItemRemoved;
    }
}
