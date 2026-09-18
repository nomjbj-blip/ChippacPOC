using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iNoticeManagement
    {
        void InsertNoticeBoard(string titleKor, string titleChn, string titleEng,
            string bodyKor, string bodyChn, string bodyEng, string noticeStart, string noticeEnd, string useFlag, string createUserID);

        void UpdateNoticeBoard(string noticeSeq, string titleKor, string titleChn, string titleEng,
            string bodyKor, string bodyChn, string bodyEng, string noticeStart, string noticeEnd, string useFlag, string updateUserID);

        void DeleteNoticeBoard(string noticeSeq);

        DataTable SelectTodayNotice(string language);

        DataTable SelectNoticeByCreateTime(string fromDate, string toDate);
    }
}
