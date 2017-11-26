using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MentosMail
{
    public abstract class ServerBase
    {
        protected ISmtpServerConf _ServerConf { get; set; }

        protected ServerBase(ISmtpServerConf conf)
        {
            if (conf == null)
            {
                throw new ArgumentNullException(nameof(conf), "Config server is null");
            }
            if (!IsValidConfigServer(conf))
            {
                throw new ArgumentException("Config is valid", nameof(conf));
            }
            _ServerConf = conf;
        }


        protected virtual MailMessage CreateMessage(MentosMail.MessageBox.IMessageMail message)
        {
            var mail = new MailMessage();
            //add all to
            foreach (var mailAddress in message.To)
            {
                mail.To.Add(mailAddress);
            }
            mail.ReplyToList.Add(message.ReplyTo);

            //add all cc
            if (message.Cc != null && message.Cc.Any())
            {
                foreach (var mailAddress in message.Cc)
                {
                    mail.CC.Add(mailAddress);
                }
            }

            //sender
            mail.Sender = message.Sender;

            //subject
            mail.Subject = message.Subject;
            mail.SubjectEncoding = message.MailEnconding;

            //priority
            mail.Priority = message.Priority;

            //From
            mail.From = message.From;

            //body message
            mail.Body = message.BodyMessage;
            mail.BodyEncoding = message.MailEnconding;
            mail.IsBodyHtml = message.IsBodyHtml;
            //Get the credentials

            //attachments
            if (message.Att != null && message.Att.Any())
            {
                foreach (var att in message.Att)
                {
                    mail.Attachments.Add(att);
                }
            }

            return mail;
        }

        protected virtual SmtpClient GetSmtpClient()
        {
            var smtp = new SmtpClient(_ServerConf.Host, _ServerConf.Port)
            {
                UseDefaultCredentials = _ServerConf.UseDefaultCredential
            };
            if (!_ServerConf.UseDefaultCredential)
            {
                smtp.Credentials = GetCredential();
            }
            smtp.EnableSsl = _ServerConf.UseSsl;
            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

            return smtp;
        }

        protected virtual NetworkCredential GetCredential()
        {
            return new NetworkCredential(_ServerConf.Username, _ServerConf.Password);
        }

        protected virtual bool IsValidMessage(MentosMail.MessageBox.IMessageMail message)
        {
            if (message == null)
            {
                throw new ArgumentException("Message cannot be null", nameof(message));
            }
            if (message.To == null || !message.To.Any())
            {
                throw new ArgumentException("To cannot be null or no itens", nameof(message.To));
            }
            if (message.ReplyTo == null)
            {
                throw new ArgumentException("ReplyTo cannot be null", nameof(message.ReplyTo));
            }
            if (message.Sender == null)
            {
                throw new ArgumentException("Sender cannot be null", nameof(message.Sender));
            }
            if (message.MailEnconding == null)
            {
                throw new ArgumentException("BodyEncoding cannot be null", nameof(message.MailEnconding));
            }
            if (string.IsNullOrEmpty(message.Subject))
            {
                throw new ArgumentException("Subject cannot be null or empty", nameof(message.Subject));
            }
            if (message.From == null)
            {
                throw new ArgumentException("From cannot be null or empty", nameof(message.From));
            }
            return true;
        }

        protected virtual bool IsValidConfigServer(ISmtpServerConf conf)
        {
            if (string.IsNullOrEmpty(conf.Host))
            {
                throw new ArgumentException("Host cannot be is null or empty", nameof(conf.Host));
            }
            if (string.IsNullOrEmpty(conf.Password))
            {
                throw new ArgumentException("Password cannot be is null or empty", nameof(conf.Password));
            }
            if (conf.Port <= 0)
            {
                throw new ArgumentException("Port cannot be zero or less", nameof(conf.Port));
            }
            if (string.IsNullOrEmpty(conf.Username))
            {
                throw new ArgumentException("Username cannot is null or empty", nameof(conf.Username));
            }
            return true;
        }

        public abstract bool Send(MentosMail.MessageBox.IMessageMail message);
        public abstract bool Send(params MentosMail.MessageBox.IMessageMail[]  messages);
        public abstract Task<bool> SendAssync(MentosMail.MessageBox.IMessageMail message, CancellationToken? token = null);
        public abstract Task<bool> SendAssync(CancellationToken? token = null, params MentosMail.MessageBox.IMessageMail[] messages);
    }
}
