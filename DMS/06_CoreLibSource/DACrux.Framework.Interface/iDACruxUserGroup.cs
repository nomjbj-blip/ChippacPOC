using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iDACruxUserGroup
    {
        DataTable LoadGroupList(string strGrpCode);
        DataTable LoadGroupInfo(string strSelectedGroup);
        DataTable LoadGroupFunction(string strSelectedGroup);
        DataTable LoadOutFunction(string strSelectedGroup);
        DataTable LoadMaxGroup();
        DataTable LoadAllFunction();
        bool InsertGroupInfo(string strGrpCode, string strGrpName, string strGrpCaption);
        bool InsertGroupFunction(string strGrpCode, string strFunCode);
        bool UpdateGroupInfo(string strGrpCode, string strGrpName, string strGrpCaption);
        bool DeleteGroupAllFunction(string strGrpCode);
        DataTable LoadGroupUser(string strSelectedGroup);
        bool DeleteGroup(string strGrpCode);
        DataTable CountByGroup();
        DataTable LoadJoinUserName();
        DataTable SearchGroupUser(string strSearchLike);
        DataTable SearchGroupList(string strSearchLike);
        bool CheckID(string strUserID);
        bool InsertGroupUser(string strGrpCode, string strUserID);
        bool DeleteGroupUser(string strID);
        DataTable LoadSecurityUser();
        bool InsertUser(string strUserID, string strUserName, string strPass, string strGrp, string strPnOffice, string strPnMobile, string strPnHome, string strPnOther, string strEmail);
        //bool InsertUser(string strUserID, string strUserName, string strPass, string strGrp, string strArea, string strPnOffice, string strPnMobile, string strPnHome, string strPnOther, string strEmail);
        DataTable LoadJoinAllGroup();
        bool DeleteUser(string strUserID);
        DataTable SearchDepartmentUser(string strSearchLike);
        DataTable SearchDepartmentList(string strSearchLike);
        DataTable CountByDepartment();
        DataTable LoadJoinUserName2();
        bool CheckFunction(string strUserID, string strFuncCode);
    }
}
