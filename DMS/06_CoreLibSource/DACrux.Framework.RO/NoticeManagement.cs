using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Framework.Interface;
using System.Data;

namespace DACrux.Framework.RO
{
    public class NoticeManagement
    {
        #region [ Class Member ]
        private iNoticeManagement m_OBJ = null;
        #endregion [ Class Member ]

        #region [ Constructor ]
        public NoticeManagement()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            object obj = Activator.GetObject(typeof(iNoticeManagement),
                strUrl + "/DACrux.Framework.BSL.NoticeManagement.bin");
            m_OBJ = obj as iNoticeManagement;
            System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }
        #endregion [ Constructor ] 


        public void InsertNoticeBoard(string titleKor, string titleChn, string titleEng,
            string bodyKor, string bodyChn, string bodyEng, string noticeStart, string noticeEnd, string useFlag, string createUserID)
        {
            m_OBJ.InsertNoticeBoard(titleKor, titleChn, titleEng,
                bodyKor, bodyChn, bodyEng, noticeStart, noticeEnd, useFlag, createUserID);
        }

        public void UpdateNoticeBoard(string noticeSeq, string titleKor, string titleChn, string titleEng,
            string bodyKor, string bodyChn, string bodyEng, string noticeStart, string noticeEnd, string useFlag, string updateUserID)
        {
            m_OBJ.UpdateNoticeBoard(noticeSeq, titleKor, titleChn, titleEng,
                bodyKor, bodyChn, bodyEng, noticeStart, noticeEnd, useFlag, updateUserID);
        }

        public void DeleteNoticeBoard(string noticeSeq)
        {
            m_OBJ.DeleteNoticeBoard(noticeSeq);
        }

        public DataTable SelectTodayNotice(string language)
        {
            return m_OBJ.SelectTodayNotice(language);
        }

        public DataTable SelectNoticeByCreateTime(string fromDate, string toDate)
        {
            return m_OBJ.SelectNoticeByCreateTime(fromDate, toDate);
        }
    }
}
