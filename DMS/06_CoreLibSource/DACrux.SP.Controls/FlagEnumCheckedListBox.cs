using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace QMS.WinControl
{
    /// <summary>
    /// Class Name : FlagEnumUITypeEditor<br/>
    /// Summary    : Flag Enum item Selection UI Type Control Class<br/>
    /// Author     : 미라콤 양형석<br/>
    /// First Date : 2009-11-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class FlagEnumCheckedListBox : CheckedListBox, IDisposable
    {
        #region " MEMBER FIELD "

        private System.ComponentModel.Container components = null;

        private Type enumType;
        private Enum enumValue;
        private bool isUpdatingCheckStates = false;

        #endregion

        #region " PROPERTY "

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Enum EnumValue
        {
            get
            {
                object e = Enum.ToObject(enumType, GetValue());
                return (Enum)e;
            }
            set
            {
                Items.Clear();
                enumValue = value;
                enumType = value.GetType();
                FillEnumMembers();
                ApplyEnumValue();
            }
        }

        #endregion

        #region " CREATOR "

        public FlagEnumCheckedListBox()
        {
            InitializeComponent();
        }

        #endregion

        #region " EVENT HANDLER "

        protected override void OnItemCheck(ItemCheckEventArgs e)
        {
            base.OnItemCheck(e);

            if (isUpdatingCheckStates)
                return;

            FlagEnumCheckedListBoxItem clickedItem = Items[e.Index] as FlagEnumCheckedListBoxItem;

            UpdateCheckedItems(clickedItem, e.NewValue);
        }

        #endregion

        #region " METHOD "

        private void InitializeComponent()
        {
            this.CheckOnClick = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        public FlagEnumCheckedListBoxItem Add(int value, string caption)
        {
            FlagEnumCheckedListBoxItem item = new FlagEnumCheckedListBoxItem(value, caption);
            Items.Add(item);
            return item;
        }

        public FlagEnumCheckedListBoxItem Add(FlagEnumCheckedListBoxItem item)
        {
            Items.Add(item);
            return item;
        }

        public void SetValue(string value)
        {
            string[] arrSplit = value.Split(',');

            int intVal = 0;
            object val = null;

            foreach (string split in arrSplit)
            {
                try
                {
                    val = Enum.Parse(enumType, split.Trim());
                    intVal += (int)Convert.ChangeType(val, typeof(int));
                }
                catch
                {
                    continue;
                }
            }

            SetValue(intVal);
        }

        public void SetValue(int value)
        {
            isUpdatingCheckStates = true;

            for (int i = 0; i < Items.Count; i++)
            {
                FlagEnumCheckedListBoxItem item = Items[i] as FlagEnumCheckedListBoxItem;

                if (item.Value == 0)
                    SetItemChecked(i, (value == 0));
                else
                    SetItemChecked(i, (value & item.Value) == item.Value);
            }

            isUpdatingCheckStates = false;
        }

        private void UpdateCheckedItems(FlagEnumCheckedListBoxItem clickedItem, CheckState checkState)
        {
            if (clickedItem.Value == 0)
                SetValue(0);

            int sum = GetValue();

            if (checkState == CheckState.Unchecked)
                sum = sum & (~clickedItem.Value);
            else
                sum |= clickedItem.Value;

            SetValue(sum);
        }

        public int GetValue()
        {
            int sum = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                FlagEnumCheckedListBoxItem item = Items[i] as FlagEnumCheckedListBoxItem;

                if (GetItemChecked(i))
                    sum |= item.Value;
            }

            return sum;
        }

        private void FillEnumMembers()
        {
            foreach (string name in Enum.GetNames(enumType))
            {
                object val = Enum.Parse(enumType, name);
                int intVal = (int)Convert.ChangeType(val, typeof(int));

                Add(intVal, name);
            }
        }

        private void ApplyEnumValue()
        {
            int intVal = (int)Convert.ChangeType(enumValue, typeof(int));
            SetValue(intVal);
        }

        #endregion
    }

    public sealed class FlagEnumCheckedListBoxItem
    {
        #region " MEMBER FIELD "

        private int iEnumValue;
        private string strCaption;

        #endregion

        #region " PROPERTY "

        public int Value
        {
            get { return iEnumValue; }
            set { iEnumValue = value; }
        }

        public string Caption
        {
            get { return strCaption; }
            set { strCaption = value; }
        }

        public bool IsFlagValue
        {
            get
            {
                return ((iEnumValue & (iEnumValue - 1)) == 0);
            }
        }

        #endregion

        #region " CREATOR "

        public FlagEnumCheckedListBoxItem(int value, string caption)
        {
            iEnumValue = value;
            strCaption = caption;
        }

        #endregion

        #region " METHOD "

        public override string ToString()
        {
            return strCaption;
        }

        public bool IsMemberFlagOf(FlagEnumCheckedListBoxItem composite)
        {
            return (IsFlagValue && ((composite.Value & iEnumValue) == iEnumValue));
        }

        #endregion
    }
}
