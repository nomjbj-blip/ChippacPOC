using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DACrux.Common.Interface
{
    public interface iComConfiguration
    {
        #region [ TQC_CONFIG ]
        int AddConfiguration(String[,] parameters);
        void AddConfiguration(string ValueCategory, string ValueName, string Value, string ValueType, string Description);
        void UpdateConfiguration(string ValueCategory, string ValueName, string Value, string ValueType, string Description);
        DataTable GetConfigCategory(string strCategory);
        DataTable GetDefectFTP();
        DataTable GetSEMDASConfigure();

        DataTable GetAVIImageFTPInfo();
        DataTable GetAVIMapFTPInfo();
        Dictionary<string, string> GetData(string category);
        #endregion [TQC_CONFIG]

        //--

        #region [ TQC_CONFIG_USER ]
        DataTable GetConfigUser(string factory, string category, string userid);
        DataTable GetConfigurationUser(string factory, string category, string userid);
        Color GetConfigUserColor(string factory, string category, string name, string userid);
        object GetConfigUserValue(string factory, string category, string name, string userid);
        int SetConfigUser(string factory, string[] category, string userid, string[,] parameters);
        string GetConfigValue(string factory, string category, string name, string userId);

        //--

        Color GetDieBackColor(string factory, string userID);
        DataTable SelectDefectMapConfig(string factory, string userID);

        #endregion [ TQC_CONFIG_USER]

        #region Global Config

        DataTable GetConfigListAll();
        DataTable GetConfigListDuple(string strCategory, string strName);
        void UpdateConfiagData(string strCategory, string strName, string strValue, string strType, string strComment);
        void DeleteConfiagData(string strCategory, string strName);
        void InsertConfiagData(string strCategory, string strName, string strValue, string strType, string strComment);

        #endregion Global Config

        #region TQC_LOT_HIS

        DataTable GetLotHisData(string strStartTime, string strEndTime);
        DataTable GetLotHisDataGroup(string strItem, string strStartTime, string strEndTime);

        DataSet GetCommonality(string[] strGoodLot, string[] strBadLot, string[] strItem, bool bTestArea);
        DataSet GetCommonalityWafer(string[] strGoodLot, string[] strBadLot, string[] strItem, bool bTestArea);

        #endregion TQC_LOT_HIS
    }
}
