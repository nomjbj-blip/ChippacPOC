using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Framework.DSL;
using System.Data;

namespace DACrux.Framework.BSL
{
    public class Config : Miracom.Middleware.BaseComponent, DACrux.Framework.Interface.iDACruxConfig
    {
        public Config()
        {
        }

        public DataTable GetGcmList()
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                return oGcmTblDat.GetGcm();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void InsertGcmDataSearchRule(string FACTORY, string SEARCH_ID)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.InsertGcmDataSearchRule(FACTORY,SEARCH_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }



        public void InsertGcmDataFTPOption(string FACTORY, string FTP_IP, string FPT_PORT, string FTP_USER_ID, string FTP_PASSWORD, string GOLDEN, string EQUIPMENT_CMP, string MHS)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.InsertGcmDataFTPOption(FACTORY, FTP_IP, FPT_PORT, FTP_USER_ID, FTP_PASSWORD, GOLDEN, EQUIPMENT_CMP, MHS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        public void InsertGcmDataShiftZone(string FACTORY, string DAY, string SWING, string NIGHT)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.InsertGcmDataFTPOption(FACTORY, DAY, SWING, NIGHT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        public void InsertGcmDataCompareOption(string FACTORY, int INTERVAL, string START_TIME)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.InsertGcmDataCompareOption(FACTORY, INTERVAL, START_TIME);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        public void InsertGcmDataH101Config(string FACTORY, string H101SERVER_IP, string H101COM, string H101PORT, string H101CHANEL)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.InsertGcmDataH101Config(FACTORY, H101SERVER_IP, H101COM, H101PORT, H101CHANEL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        public void UpdateGcmDataH101Config(string FACTORY, string H101SERVER_IP, string H101COM, string H101PORT, string H101CHANEL)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.UpdateGcmDataH101Config(FACTORY, H101SERVER_IP, H101COM, H101PORT, H101CHANEL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void UpdateGcmDataCompareOption(string FACTORY, int INTERVAL, string START_TIME)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.UpdateGcmDataCompareOption(FACTORY, INTERVAL, START_TIME);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        public void UpdateGcmDataSearchRule(string FACTORY, string SEARCH_ID)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.UpdateGcmDataSearchRule(FACTORY, SEARCH_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void UpdateGcmDataFTPOption(string FACTORY, string FTP_IP, string FPT_PORT, string FTP_USER_ID, string FTP_PASSWORD, string GOLDEN, string EQUIPMENT_CMP, string MHS)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.UpdateGcmDataFTPOption( FACTORY,  FTP_IP,  FPT_PORT,  FTP_USER_ID,  FTP_PASSWORD,  GOLDEN,  EQUIPMENT_CMP,  MHS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void UpdateGcmDataShiftZone(string FACTORY, string DAY, string SWING, string NIGHT)
        {
            TQC_GCMTBLDAT oGcmTblDat = null;
            try
            {
                oGcmTblDat = new TQC_GCMTBLDAT();
                oGcmTblDat.UpdateGcmDataShiftZone(FACTORY, DAY, SWING, NIGHT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
    }
}
