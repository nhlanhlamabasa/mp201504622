namespace HotelSystem.SharedKernel.Models
{
    using HotelSystem.SharedKernel.Interfaces;
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="EmailSender" />
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// The SendEmailPOP3
        /// </summary>
        /// <param name="Host">The Host<see cref="string"/></param>
        /// <param name="Port">The Port<see cref="int"/></param>
        /// <param name="Username">The Username<see cref="string"/></param>
        /// <param name="Password">The Password<see cref="string"/></param>
        /// <param name="email">The email<see cref="Email"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        public string SendEmailPOP3(string Host, int Port, string Username, string Password, Email email)
        {
            SmtpClient client = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = false
            };

            try
            {
                client.SendAsync(email.From, email.Recepient, email.Subject, email.Body, email.Token);
                return "Sent";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// The SendEmailSMTP
        /// </summary>
        /// <param name="Host">The Host<see cref="string"/></param>
        /// <param name="Port">The Port<see cref="int"/></param>
        /// <param name="Username">The Username<see cref="string"/></param>
        /// <param name="Password">The Password<see cref="string"/></param>
        /// <param name="email">The email<see cref="Email"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        public string SendEmailSMTP(string Host, int Port, string Username, string Password, Email email)
        {
            SmtpClient client = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = false
            };

            try
            {
                client.SendAsync(email.From, email.Recepient, email.Subject, email.Body, email.Token);
                return "Sent";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
