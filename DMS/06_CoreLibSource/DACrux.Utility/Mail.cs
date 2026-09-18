using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace DACrux.Utility
{
    public static class Mail
    {
        /// <summary>
        /// 메일 보내기
        /// </summary>
        /// <param name="subject">메일 제목</param>
        /// <param name="message">메일 내용</param>
        /// <param name="toList">받는 사람 메일 주소 목록,구분자(;)</param>
        public static void Send(string Host, string SenderAddress, string SenderName, string userName, string password, string subject, string message, string toList)
        {

            // 메세지를 만든다.
            MailMessage mail = new MailMessage();

            // 보내는 사람
            mail.From = new MailAddress(SenderAddress, SenderName);
            //mail.From = new MailAddress("hanamailmaster@hanamicron.co.kr", "Hana Mailing Master");

            String[] toArray = toList.Split(';');
            foreach (String to in toArray)
            {
                mail.To.Add(new MailAddress(to));
            }

            // 받는 사람
            //mail.To.Add(new MailAddress("house@hanamicron.co.kr", "김준용"));
            //mail.To.Add(new MailAddress("bjjung@hanamicron.co.kr", "정병주"));

            // 제목
            mail.Subject = subject;

            // 내용
            mail.Body = message;

            // encoding
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            // html 여부
            mail.IsBodyHtml = false;

            // 메일 우선 순위
            mail.Priority = MailPriority.High;

            // smtp 접속 후 메일 발송
            //SmtpClient smtpClient = new SmtpClient("smtp.hanamicron.co.kr");
            SmtpClient smtpClient = new SmtpClient(Host);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.Port = 25;

            //smtpClient.Credentials = new NetworkCredential("HanaMailMaster\\hmicron.com", "p@ssw0rd");
            smtpClient.Credentials = new NetworkCredential(userName, password);

            smtpClient.Send(mail);
        }

    }
}
