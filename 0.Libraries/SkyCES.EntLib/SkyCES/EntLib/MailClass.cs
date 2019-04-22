namespace SkyCES.EntLib
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    public sealed class MailClass
    {
        public static void SendEmail(MailInfo mail)
        {
            MailMessage message = new MailMessage {
                BodyEncoding = Encoding.Default,
                From = new MailAddress(mail.UserName)
            };
            try
            {
                message.To.Add(mail.ToEmail);
            }
            catch
            {
            }
            message.Subject = mail.Title;
            message.Body = mail.Content;
            message.IsBodyHtml = mail.IsBodyHtml;
            try
            {
                new SmtpClient(mail.Server) { Port = mail.ServerPort, UseDefaultCredentials = false, Credentials = new NetworkCredential(mail.UserName, mail.Password), DeliveryMethod = SmtpDeliveryMethod.Network }.Send(message);
            }
            catch
            {
                throw new Exception("邮件配置错误，请检查");
            }
        }
    }
}

