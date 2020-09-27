using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ecommerce.UI.Email
{
    public class GmailStmp : IEmail
    {
        private string _host;
        private int _port;
        private bool _ssl;
        private string _myemail;
        private string _password;



        public GmailStmp(string host, int port, bool ssl, string myemail, string password)
        {
            this._host = host;
            this._port = port;
            this._ssl = ssl;
            this._myemail = myemail;
            this._password = password;
        }

        public Task SendEmailAsync(string email, string subject, string text)
        {
            var client = new SmtpClient(this._host, this._port)
            {
                Credentials = new NetworkCredential(_myemail, _password),
                EnableSsl = this._ssl
            };

            return client.SendMailAsync(
                new MailMessage(this._myemail, email, subject, text)
                {
                    IsBodyHtml = true
                }
             );
        }
    }
}
