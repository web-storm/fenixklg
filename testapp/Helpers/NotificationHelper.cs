using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace testapp.Helpers
{
    public static class NotificationHelper
    {
        public static void NotifyByEmail(string mailTo, string body)
        {
            var from = new MailAddress("auto@fenixklg.ru", "Автоматическая рассылка fenixklg.ru");
            var to = new MailAddress(mailTo);

            var mail = new MailMessage(from, to);
            mail.Subject = "Сообщение от сайта fenixklg.ru";
            mail.Body = body;
            mail.IsBodyHtml = false;

            var client = new SmtpClient("mail.fenixklg.ru", 587);
            client.UseDefaultCredentials = true;
            var NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = "auto@fenixklg.ru";
            NetworkCred.Password = "9storm2!";
            client.Credentials = NetworkCred;
            client.EnableSsl = false;

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }
    }
}