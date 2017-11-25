using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MentosMailCore.MessageBox;

namespace MentosMailCore
{
    public sealed class SmtpServer : ServerBase
    {
        public SmtpServer(ISmtpServerConf conf) : base(conf)
        {
        }

        public override bool Send(IMessageMail message)
        {
            SmtpClient smtpClient = null;
            MailMessage mail = null;
            try
            {
                if (!IsValidMessage(message))
                {
                    throw new ArgumentException("Message is invalid", nameof(message));
                }

                //Create mail message
                mail = CreateMessage(message);
                //Send message
                smtpClient = GetSmtpClient();
                smtpClient.Send(mail);

                return true;
            }
            finally
            {
                mail?.Dispose();
                smtpClient?.Dispose();
            }
        }

        public override bool Send(params IMessageMail[] messages)
        {
            return messages.Select(Send).All(m => m);
        }

        public override Task<bool> SendAssync(IMessageMail message, CancellationToken? token = null)
        {
            throw new NotImplementedException("In future version");
        }

        public override Task<bool> SendAssync(CancellationToken? token = null, params IMessageMail[] messages)
        {
            throw new NotImplementedException("In future version");
        }
    }
}