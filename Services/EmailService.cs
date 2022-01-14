using System.Net;
using System.Net.Mail;

namespace Blog.Services
{
    public class EmailService
    {
        public bool send(
        string ToName,
        string ToEmail,
        string Subject,
        string Body,
        string FromName = "Matheus, Developer",
        string FromEmail = "meuEmail")
        {
            var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

            smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtpClient.EnableSsl = true;

            var mail = new MailMessage();
            mail.From = new MailAddress(FromEmail, FromName);
            mail.To.Add(new MailAddress(ToEmail, ToName));
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
