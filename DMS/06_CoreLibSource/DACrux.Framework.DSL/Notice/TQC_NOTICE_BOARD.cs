using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miracom.Middleware;
using System.Data;

namespace DACrux.Framework.DSL
{
    public class TQC_NOTICE_BOARD : QueryComponent
    {
        public TQC_NOTICE_BOARD()
        {
            string connectID = string.Empty;

            connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
            if (String.IsNullOrEmpty(connectID))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "TQC_NOTICE_BOARD.xml");
        }

        public void InsertNoticeBoard(string titleKor, string titleChn, string titleEng,
            string bodyKor, string bodyChn, string bodyEng, string noticeStart, string noticeEnd, string useFlag, string createUserID)
        {
            Execute("INSERT_NOTICE_BOARD", null, new string[]
            { 
                titleKor, titleChn, titleEng,
                bodyKor, bodyChn, bodyEng, 
                noticeStart, noticeEnd, useFlag, createUserID}
            );
        }

        public void UpdateNoticeBoard(string noticeSeq, string titleKor, string titleChn, string titleEng,
            string bodyKor, string bodyChn, string bodyEng, string noticeStart, string noticeEnd, string useFlag, string updateUserID)
        {
            Execute("UPDATE_NOTICE_BOARD", null, new string[] 
            {
                noticeSeq,  titleKor, titleChn, titleEng,
                bodyKor, bodyChn, bodyEng, 
                noticeStart, noticeEnd, useFlag, updateUserID
            });
        }

        public void DeleteNoticeBoard(string noticeSeq)
        {
            Execute("DELETE_NOTICE_BOARD", null, new string[] { noticeSeq });
        }

        public DataTable SelectTodayNotice(string language)
        {
            return GetDataTable("SELECT_TODAY_NOTICE", null, new string[] { language });
        }

        public DataTable SelectNoticeByCreateTime(string fromDate, string toDate)
        {
            return GetDataTable("SELECT_NOTICE_BOARD_01", null, new string[] { fromDate, toDate });
        }
    }
}
