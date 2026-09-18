using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Common.Interface;
using System.Drawing;
using System.Data;
using DACrux.Base;

namespace DACrux.Common.RO
{
    public class ComConfiguration
    {
        iComConfiguration m_OBJ;

        public ComConfiguration()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);

            m_OBJ = Activator.GetObject(typeof(DACrux.Common.Interface.iComConfiguration),
                        strUrl + "/DACrux.Common.BSL.ComConfiguration.bin") as iComConfiguration;
        }

        #region [ TQD_CONFIG ]

        public int AddConfiguration(
            String[,] parameters
            )
        {
            return m_OBJ.AddConfiguration(
                parameters
                );
        }

        public void AddConfiguration(
            string ValueCategory,
            string ValueName,
            string Value,
            string ValueType,
            string Description
            )
        {
            m_OBJ.AddConfiguration(
                ValueCategory,
                ValueName,
                Value,
                ValueType,
                Description
                );
        }

        public DataTable GetSEMDASConfigure()
        {
            return m_OBJ.GetSEMDASConfigure();
        }

        public DataTable GetConfigCategory(string strCategory)
        {
            return m_OBJ.GetConfigCategory(strCategory);
        }

        public DataTable GetDefectFTP()
        {
            return m_OBJ.GetDefectFTP();
        }

        public DataTable GetAVIImageFTPInfo()
        {
            return m_OBJ.GetAVIImageFTPInfo();
        }

        public DataTable GetAVIMapFTPInfo()
        {
            return m_OBJ.GetAVIMapFTPInfo();
        }

        public Dictionary<string, string> GetData(
            string category
            )
        {
            return m_OBJ.GetData(
                category
                );
        }
        #endregion [ TQD_CONFIG ]


        #region [ TQD_CONFIG_USER ]
        //TQD_CONFIG_USER
        public DataTable GetConfigUser(
            string factory,
            string category,
            string userid
            )
        {
            return m_OBJ.GetConfigUser(
                factory,
                category,
                userid
                );
        }

        //TQD_CONFIG_USER
        public DataTable GetConfigurationUser(
            string factory,
            string category,
            string userid
            )
        {
            return m_OBJ.GetConfigurationUser(
                factory,
                category,
                userid
                );
        }

        public Color GetConfigUserColor(
            string factory,
            string category,
            string name,
            string userid
            )
        {
            return m_OBJ.GetConfigUserColor(
                factory,
                category,
                name,
                userid
                );
        }

        public object GetConfigUserValue(
            string factory,
            string category,
            string name,
            string userid
            )
        {
            return m_OBJ.GetConfigUserValue(
                factory,
                category,
                name,
                userid
                );
        }

        //TQD_CONFIG_USER (delete -> insert)
        public int SetConfigUser(
            string factory,
            string[] category,
            string userid,
            string[,] parameters
            )
        {
            return m_OBJ.SetConfigUser(
                factory,
                category,
                userid,
                parameters
                );
        }

        //--


        public Color GetDieBackColor(string factory, string userID)
        {
            return m_OBJ.GetDieBackColor(factory, userID);
        }

        public DataTable SelectDefectMapConfig(string factory, string userID)
        {
            return m_OBJ.SelectDefectMapConfig(factory, userID);
        }

        #endregion [ TQD_CONFIG_USER]


        #region Config Value

        public string GetConfigValue(string category, string name)
        {
            return m_OBJ.GetConfigValue(GlobalVariable.Factory, category, name, GlobalVariable.UserID);
        }

        /// <summary>
        /// Default Defect Size를 가져옵니다.
        /// </summary>
        public bool TryDefaultDefectSize(out float defectSize)
        {
            defectSize = 0;

            string val = GetConfigValue("DEFECT_OPTION", "DEFAULT_DEFECT_SIZE");

            if (String.IsNullOrEmpty(val))
                return false;

            return float.TryParse(val, out defectSize);
        }

        #endregion

        #region Global Config

        public DataTable GetConfigListAll()
        {
            return m_OBJ.GetConfigListAll();
        }

        public DataTable GetConfigListDuple(string strCategory, string strName)
        {
            return m_OBJ.GetConfigListDuple(strCategory, strName);
        }

        public void UpdateConfiagData(string strCategory, string strName, string strValue, string strType, string strComment)
        {
            m_OBJ.UpdateConfiagData(strCategory, strName, strValue, strType, strComment);
        }

        public void DeleteConfiagData(string strCategory, string strName)
        {
            m_OBJ.DeleteConfiagData(strCategory, strName);
        }

        public void InsertConfiagData(string strCategory, string strName, string strValue, string strType, string strComment)
        {
            m_OBJ.InsertConfiagData(strCategory, strName, strValue, strType, strComment);
        }


        #endregion Global Config

        #region TQC_LOT_HIS

        public DataTable GetLotHisData(string strStartTime, string strEndTime)
        {
            return m_OBJ.GetLotHisData(strStartTime, strEndTime);
        }

        public DataTable GetLotHisDataGroup(string strItem, string strStartTime, string strEndTime)
        {
            return m_OBJ.GetLotHisDataGroup(strItem, strStartTime, strEndTime);
        }

        public DataSet GetCommonality(string[] strGoodLot, string[] strBadLot, string[] strItem, bool bTestArea)
        {
            return m_OBJ.GetCommonality(strGoodLot, strBadLot, strItem, bTestArea);
        }

        public DataSet GetCommonalityWafer(string[] strGoodLot, string[] strBadLot, string[] strItem, bool bTestArea)
        {
            return m_OBJ.GetCommonalityWafer(strGoodLot, strBadLot, strItem, bTestArea);
        }

        #endregion TQC_LOT_HIS
    }
}
