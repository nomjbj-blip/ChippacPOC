using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iDACruxFunctionManagement
    {
        DataTable LoadFunctionList();
        bool InsertFunction(string strFuncCode, string strFuncName, string strHelpUrl);
        bool UpdateFunction(string strFuncCode, string strFuncName, string strHelpUrl, string strOldFuncCode);
        bool UpdateGroupFunction(string strFuncCode,string strOldFuncCode);
        bool DeleteFunction(string strFuncCode);
        bool DeleteGroupFunction(string strFuncCode);
        DataTable CheckAttachedFunction(string strFuncCode);
        DataTable CheckFuncCodeExistence(string strFuncCode);
        DataTable CheckFuncNameExistence(string strFuncName);

    }
}
