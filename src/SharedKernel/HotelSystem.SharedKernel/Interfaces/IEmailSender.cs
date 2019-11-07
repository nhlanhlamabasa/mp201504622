namespace HotelSystem.SharedKernel.Interfaces
{
    using HotelSystem.SharedKernel.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IEmailSender" />
    /// </summary>
    public interface IEmailSender
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
        string SendEmailPOP3(string Host, int Port, string Username, string Password, Email email);

        /// <summary>
        /// The SendEmail
        /// </summary>
        /// <param name="Host">The Host<see cref="string"/></param>
        /// <param name="Port">The Port<see cref="int"/></param>
        /// <param name="Username">The Username<see cref="string"/></param>
        /// <param name="Password">The Password<see cref="string"/></param>
        /// <param name="email">The email<see cref="Email"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        string SendEmailSMTP(string Host, int Port, string Username, string Password, Email email);
    }
}
