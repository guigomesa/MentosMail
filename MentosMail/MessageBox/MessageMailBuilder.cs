using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MentosMail.MessageBox
{
    public class MessageMailBuilder
    {
        private MessageMail Message { get; set; } = new MessageMail();

        public MessageMailBuilder CreateBodyMessage(string msg)
        {
            this.Message.BodyMessage = msg;
            return this;
        }

        public MessageMailBuilder CreateSubject(string subject)
        {
            this.Message.Subject = subject;
            return this;
        }

        public MessageMailBuilder SetIsBodyHtml(bool isHtml)
        {
            this.Message.IsBodyHtml = isHtml;
            return this;
        }

        public MessageMailBuilder SetPriority(MailPriority priority = MailPriority.Normal)
        {
            this.Message.Priority = priority;
            return this;
        }

        public MessageMailBuilder SetEncoding(Encoding encode= null)
        {
            this.Message.MailEnconding = encode ?? Encoding.UTF8;

            return this;
        }

        public MessageMailBuilder AddTo(MailAddress to)
        {
            this.Message.To.Add(to);
            return this;
        }

        public MessageMailBuilder AddTo(params MailAddress[] tos)
        {
            foreach (var mailAddress in tos)
            {
                this.Message.To.Add(mailAddress);
            }
            return this;
        }

        public MessageMailBuilder AddCc(MailAddress cc)
        {
            this.Message.Cc.Add(cc);
            return this;
        }

        public MessageMailBuilder AddCc(params MailAddress[] ccs)
        {
            foreach (var c in ccs)
            {
                this.Message.Cc.Add(c);
            }
            return this;
        }

        public MessageMailBuilder AddBcc(MailAddress bcc)
        {
            this.Message.Bcc.Add(bcc);
            return this;
        }

        public MessageMailBuilder AddBcc(params MailAddress[] bccs)
        {
            foreach (var bcc in bccs)
            {
                this.Message.Bcc.Add(bcc);
            }
            return this;
        }

        public MessageMailBuilder SetSender(MailAddress sender)
        {
            this.Message.Sender = sender;
            return this;
        }

        public MessageMailBuilder SetReplyTo(MailAddress replyTo)
        {
            this.Message.ReplyTo = replyTo;
            return this;
        }

        public MessageMailBuilder AddAttachment(Attachment att)
        {
            this.Message.Att.Add(att);
            return this;
        }

        public MessageMailBuilder AddAttachment(params Attachment[] atts)
        {
            foreach (var att in atts)
            {
                this.Message.Att.Add(att);
            }
            return this;
        }

        public MessageMail Build()
        {
            return Message;
        }



    }
}
