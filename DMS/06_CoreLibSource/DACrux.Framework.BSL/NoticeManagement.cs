using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DACrux.Framework.DSL;
using DACrux.Framework.Interface;
using Miracom.Middleware;

namespace DACrux.Framework.BSL
{
    public class NoticeManagement : BaseComponent, iNoticeManagement
    {/// <summary>
        /// 공지사항 데이터를 insert 합니다.
        /// </summary>
        public void InsertNoticeBoard(string titleKor, string titleChn, string titleEng,
            string bodyKor, string bodyChn, string bodyEng, string noticeStart, string noticeEnd, string useFlag, string createUserID)
        {
            TQC_NOTICE_BOARD obj = new TQC_NOTICE_BOARD();
            obj.InsertNoticeBoard(titleKor, titleChn, titleEng,
                bodyKor, bodyChn, bodyEng, noticeStart, noticeEnd, useFlag, createUserID);
        }

        /// <summary>
        /// 공지사항 데이터를 update 합니다.
        /// </summary>
        public void UpdateNoticeBoard(string noticeSeq, string titleKor, string titleChn, string titleEng,
            string bodyKor, string bodyChn, string bodyEng, string noticeStart, string noticeEnd, string useFlag, string updateUserID)
        {
            TQC_NOTICE_BOARD obj = new TQC_NOTICE_BOARD();
            obj.UpdateNoticeBoard(noticeSeq, titleKor, titleChn, titleEng,
                bodyKor, bodyChn, bodyEng, noticeStart, noticeEnd, useFlag, updateUserID);
        }

        /// <summary>
        /// 공지사항 데이터를 delete 합니다.
        /// </summary>
        public void DeleteNoticeBoard(string noticeSeq)
        {
            TQC_NOTICE_BOARD obj = new TQC_NOTICE_BOARD();
            obj.DeleteNoticeBoard(noticeSeq);
        }

        /// <summary>
        /// 현재 활성화된 공지사항을 가져옵니다.
        /// </summary>
        public DataTable SelectTodayNotice(string language)
        {
            TQC_NOTICE_BOARD obj = new TQC_NOTICE_BOARD();
            return obj.SelectTodayNotice(language);
        }

        /// <summary>
        /// 생성일 기준으로 공지사항을 가져옵니다.
        /// </summary>
        public DataTable SelectNoticeByCreateTime(string fromDate, string toDate)
        {
            TQC_NOTICE_BOARD obj = new TQC_NOTICE_BOARD();
            return obj.SelectNoticeByCreateTime(fromDate, toDate);
        }
    }
}
