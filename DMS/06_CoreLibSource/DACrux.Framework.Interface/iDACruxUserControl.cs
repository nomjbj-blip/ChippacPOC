using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iDACruxUserControl
    {
        DataTable GetFacilityList();
        DataTable GetDeviceList(string strFacility);
        DataTable GetProductList(string strFacility, string strPartID);
        DataTable GetProgramList(string strFacility, string strPartID, string strProduct);
        DataTable GetEquipList();
        DataTable GetHbinList(string strFacility, string strPartID, string strFullPartID);
        DataTable GetBinList(string strFacility, string strPartID, string strFullPartID, string strHbin);
        DataTable GetFlowList(string strFacility, string strProduct);        
        DataTable GetOperList(string strFacility, string strProduct, string strFlow);
        DataTable GetResList(string strFacility, string strProduct, string strFlow, string strOper, bool bProcess);
        DataTable GetResList(string strSummaryType);
        DataTable GetFlowListByRes(string strFacility, string strRes);
        DataTable GetOperListByResFlow(string strFacility, string strRes, string strFlow);
        DataTable GetParaList(string strFacility, string strProduct, string strFlow, string strOper, string strRes, string strTestProgram);
        DataTable GetLotTypeList(string strFacility);
        DataTable GetYmsLotList(string strStartDate, string strEndDate, string strFacility, string strPartID, string strProduct, string strFlow, string strOper, string strEq, string strLotType, bool bOnlyGlass);
        DataTable GetYmsUnitList(string strLotSeq);
        string GetFacility(string strMeasureEQ);

        DataTable GetEWSMatList(string strFacility);
        DataTable GetUserList();
        DataTable GetUserNameList();
        DataTable GetEWSTestMode(string strFacility);
        DataTable GetEWSStep(string strFacility);
        DataTable GetEWSTestGroup(string strFacility);
        DataTable GetEWSUse(string strFacility);
        DataTable GetEWSBoardType(string strFacility);

        DataTable GetProductListByNothing(string strFacility);
        DataTable GetParaListByNothing();

        DataTable GetCustomerList();
        DataTable GetYMSProductList(string strCustomer);
        DataTable GetYMSDeviceList(string strCustomer);
        DataTable GetYMSOperList();
        DataTable GetBinDesc(string strPartID);
        DataTable GetHandlerList();
    }
}
