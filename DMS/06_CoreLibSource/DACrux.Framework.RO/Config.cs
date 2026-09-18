using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.Framework.Interface;

namespace DACrux.Framework.RO
{
    /// <summary>
    /// Class Name : Config<br/>
    /// Summary    : QMS Config Remoting Object Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2012-10-25<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class Config
    {
        #region Class Member

        DACrux.Framework.Interface.iDACruxConfig m_OBJ = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public Config()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            object obj = Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxCommon),
                strUrl + "/DACrux.Framework.BSL.Config.bin");
            m_OBJ = obj as DACrux.Framework.Interface.iDACruxConfig;
            System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }

        #endregion


        public DataTable GetGcmList()
        {
            try
            {
                return m_OBJ.GetGcmList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertGcmDataSearchRule(string FACTORY, string SEARCH_ID)
        {
            try
            {
                m_OBJ.InsertGcmDataSearchRule(FACTORY,SEARCH_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertGcmDataFTPOption(string FACTORY,string FTP_IP, string FPT_PORT, string FTP_USER_ID, string FTP_PASSWORD, string GOLDEN, string EQUIPMENT_CMP, string MHS)
        {
            

            try
            {
                m_OBJ.InsertGcmDataFTPOption(FACTORY
                            , FTP_IP
                            , FPT_PORT
                            , FTP_USER_ID
                            , FTP_PASSWORD
                            , GOLDEN
                            , EQUIPMENT_CMP
                            , MHS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertGcmDataShiftZone(string FACTORY, string DAY, string SWING, string NIGHT)
        {

            try
            {
                m_OBJ.InsertGcmDataShiftZone(FACTORY
                                            , DAY
                                            , SWING
                                            , NIGHT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertGcmDataCompareOption(string FACTORY ,int INTERVAL, string START_TIME)
        {
            try
            {
                m_OBJ.InsertGcmDataCompareOption(FACTORY
                                            , INTERVAL
                                            , START_TIME);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertGcmDataH101Config(string FACTORY, string H101SERVER_IP, string H101COM, string H101PORT, string H101CHANEL)
        {
            try
            {
                m_OBJ.InsertGcmDataH101Config(FACTORY
                                            , H101SERVER_IP
                                            , H101COM
                                            , H101PORT
                                            , H101CHANEL );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateGcmDataCompareOption(string FACTORY, int INTERVAL, string START_TIME)
        {
            try
            {
                m_OBJ.UpdateGcmDataCompareOption(FACTORY
                                            , INTERVAL
                                            , START_TIME);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateGcmDataH101Config(string FACTORY, string H101SERVER_IP, string H101COM, string H101PORT, string H101CHANEL)
        {
            try
            {
                m_OBJ.UpdateGcmDataH101Config(FACTORY
                                            , H101SERVER_IP
                                            , H101COM
                                            , H101PORT
                                            , H101CHANEL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public void UpdateGcmDataShiftZone(string FACTORY, string DAY, string SWING, string NIGHT)
        {
            try
            {
                m_OBJ.UpdateGcmDataShiftZone(FACTORY
                                            , DAY
                                            , SWING
                                            , NIGHT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateGcmDataFTPOption(string FACTORY, string FTP_IP, string FPT_PORT, string FTP_USER_ID, string FTP_PASSWORD, string GOLDEN, string EQUIPMENT_CMP, string MHS)
        {
            try
            {
                m_OBJ.UpdateGcmDataFTPOption(FACTORY
                                            , FTP_IP
                                            , FPT_PORT
                                            , FTP_USER_ID
                                            , FTP_PASSWORD
                                            , GOLDEN
                                            , EQUIPMENT_CMP
                                            , MHS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateGcmDataSearchRule(string FACTORY, string SEARCH_ID)
        {
            try
            {
                m_OBJ.UpdateGcmDataSearchRule(FACTORY
                                            , SEARCH_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectEquipmentType()
        {
            throw new NotImplementedException();
        }

        public void InsertEquipmentModel(string p, string p_2, string p_3, double dPAD, double dLEAD, double dDOWN, string p_4, string p_5, string p_6, string p_7, string p_8, string p_9, string p_10, string p_11, string p_12, string p_13, string p_14, string p_15, string p_16, string p_17, string p_18, string p_19, string p_20, string p_21, string p_22)
        {
            throw new NotImplementedException();
        }

        public void DeleteEquipmentModel(string p, string p_2)
        {
            throw new NotImplementedException();
        }

        public void UpdateEquipmentType(string p, string p_2, string p_3, double dPAD, double dLEAD, double dLEAD_2, double dDOWN, string p_4, string p_5, string p_6, string p_7, string p_8, string p_9, string p_10, string p_11, string p_12, string p_13, string p_14, string p_15, string p_16, string p_17, string p_18, string p_19, string p_20, string p_21, string p_22)
        {
            throw new NotImplementedException();
        }
    }
}
