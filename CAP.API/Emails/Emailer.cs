using System;
using System.Net.Mail;

/// <summary>
/// Helper to send emails
/// Change the namespace later
/// </summary>

namespace CAP.API.Emailer
{
    public class EmailService
    {
        /// <summary>
        ///     Send an email
        ///     All Recipients will be able to view other recipients' emails
        /// </summary>
        /// <param name="to">Recipient, or list of emails as a string, comma seperated (,). All recipients will be able to view other recipients emails</param>
        /// <param name="subject">Subject for the email</param>
        /// <param name="body">Body for the email, can be HTML</param>
        public static void SendEmail(string to, string subject, string body)
        {
            string from = "no-reply@byu.edu";

            MailMessage message = new(from, to, subject, body);
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("gateway.byu.edu");

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in SendEmail(): {0}", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        ///     Send emails using BCC
        ///     Recipients will not be able to view other recipients emails
        /// </summary>
        /// <param name="to">Individual email, or list of emails seperated by comma, (,), none of the recipeients will be able to view other recipients emails</param>
        /// <param name="subject">Subject of email</param>
        /// <param name="body">Body of email, can be HTML</param>
        public static void SendBCCEmail(string to, string subject, string body)
        {
            string from = "no-reply@byu.edu";
            MailMessage message = new();
            message.IsBodyHtml = true;
            message.From = new MailAddress(from);
            message.Subject = subject;
            message.Body = body;
            message.Bcc.Add(to);


            SmtpClient client = new SmtpClient("gateway.byu.edu");

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in SendEmail(): {0}", ex.ToString());
                throw ex;
            }
        }
    }
}