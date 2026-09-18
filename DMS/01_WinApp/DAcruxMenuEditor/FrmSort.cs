using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACruxV5
{
    public partial class FrmSort : Form
    {
        public FrmSort()
        {
            InitializeComponent();
        }

        public SortType SortType
        {
            get { return rdoSortByTree.Checked ? SortType.SortByTree : SortType.SortByLevel; }
        }
    }

    public enum SortType
    {
        SortByTree,
        SortByLevel
    }
}
