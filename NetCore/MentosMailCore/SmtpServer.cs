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
        private IMessageMail Message { get; set; }
        private IMessageMail[] Messages { get; set; }
        
        
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

        private bool Send()
        {
           return Send(Message);
        }

        public override Task<bool> SendAsync(IMessageMail message, CancellationToken? token = null)
        {
            token?.ThrowIfCancellationRequested();
            Message = message;
            return new Task<bool>(Send);
        }

        public override Task<bool> SendAsync(CancellationToken? token = null, params IMessageMail[] messages)
        {
           token?.ThrowIfCancellationRequested();
            foreach (var messageMail in messages)
            {
                token?.ThrowIfCancellationRequested();
                Send(messageMail);
            }
            return new Task<bool>( () => true);
        }
    }
}