using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iDACruxConfig
    {
        DataTable GetGcmList();
        void InsertGcmDataSearchRule(string FACTORY, string SEARCH_ID);
        void InsertGcmDataFTPOption(string FACTORY, string FTP_IP, string FPT_PORT, string FTP_USER_ID, string FTP_PASSWORD, string GOLDEN, string EQUIPMENT_CMP, string MHS);
        void InsertGcmDataShiftZone(string FACTORY, string DAY, string SWING, string NIGHT);
        void InsertGcmDataCompareOption(string FACTORY, int INTERVAL, string START_TIME);
        void InsertGcmDataH101Config(string FACTORY, string H101SERVER_IP, string H101COM, string H101PORT, string H101CHANEL);

        void UpdateGcmDataSearchRule(string FACTORY, string SEARCH_ID);
        void UpdateGcmDataFTPOption(string FACTORY, string FTP_IP, string FPT_PORT, string FTP_USER_ID, string FTP_PASSWORD, string GOLDEN, string EQUIPMENT_CMP, string MHS);
        void UpdateGcmDataShiftZone(string FACTORY, string DAY, string SWING, string NIGHT);
        void UpdateGcmDataCompareOption(string FACTORY, int INTERVAL, string START_TIME);
        void UpdateGcmDataH101Config(string FACTORY, string H101SERVER_IP, string H101COM, string H101PORT, string H101CHANEL);
    }
}
