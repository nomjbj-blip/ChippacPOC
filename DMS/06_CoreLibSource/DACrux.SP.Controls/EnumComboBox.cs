using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Controls
{
    public class EnumComboBox : ComboBox
    {
        Type type;

        public Type EnumType
        {
            get { return type; }
            set 
            {
                if (value == null)
                    return;

                if (type == null || type != value)
                {
                    type = value;
                    SetItem();
                }
            }
        }

        public object EnumValue
        {
            get
            {
                if (string.IsNullOrEmpty(Text))
                    return null;

                return Enum.Parse(type, Text);
            }
        }

        private void SetItem()
        {
            Items.Clear();

            
            foreach (string value in Enum.GetNames(type))
                this.Items.Add(value);
        }

        public EnumComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;

        }     
    }
}
