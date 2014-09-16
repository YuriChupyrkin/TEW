using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mail
{
    public class YandexMailSender: IEmailSender
    {
        private const string SMTP = "smtp.yandex.ru";
        private const int PORT = 587;

        public  void Send(string from, string password, 
            string mailto, string subject, string message)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(mailto);

                mail.Body = message;
                mail.Subject = subject;

                var client = new SmtpClient();
                client.Host = SMTP;
                client.Port = PORT;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from, password);
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

    }
}
