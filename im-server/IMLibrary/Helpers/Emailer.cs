/**
 * This file defines a helper class for sending emails to application users
 * Written by Tanner Jorgensen, U of U School of Computing, Senior Capstone 2021
 **/

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Helpers
{
    public interface IEmailer
    {
        void Send(string to, string subject, string body, Stream attachment = null);
        void Send(List<string> to, string subject, string body, Stream attachment = null);
    }

    public class Emailer : IEmailer
    {
        private string host;
        private int port;
        private string email;
        private string password;
        private string webUrl;

        public Emailer(IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("SMTPConnection").Split(';');
            host = conn[0];
            port = int.Parse(conn[1]);
            email = conn[2];
            password = conn[3];
            webUrl = configuration.GetValue<string>("WebURL");
        }

        public void Send(string to, string subject, string body, Stream attachment = null)
        {
            Send(new List<string> { to }, subject, body, attachment);
        }

        public void Send(List<string> to, string subject, string body, Stream attachment = null)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("IM Client", email));
            foreach (var address in to)
            {
                mailMessage.To.Add(new MailboxAddress("", address));
            }
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart("plain")
            {
                Text = body.Replace("{webUrl}", webUrl)
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect(host, port, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(email, password);
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}
