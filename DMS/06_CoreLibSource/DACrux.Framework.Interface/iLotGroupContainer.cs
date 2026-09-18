using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    //2015-04-08-정병주 : Lot Group 관련 Interface
    public interface iLotGroupContainer
    {
        DataTable GetLotGroupAll();
        string[] GetGroupList();
        string[] GetListInGroup(string sGroup);
    }
}
