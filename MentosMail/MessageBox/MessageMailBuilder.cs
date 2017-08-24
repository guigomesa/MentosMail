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

        public MessageMailBuilder BodyMessage(string msg)
        {
            this.Message.BodyMessage = msg;
            return this;
        }

        public MessageMailBuilder Subject(string subject)
        {
            this.Message.Subject = subject;
            return this;
        }

        public MessageMailBuilder IsBodyHtml()
        {
            this.Message.IsBodyHtml = true;
            return this;
        }

        public MessageMailBuilder Priority(MailPriority priority = MailPriority.Normal)
        {
            this.Message.Priority = priority;
            return this;
        }

        public MessageMailBuilder Encoding(Encoding encode= null)
        {
            this.Message.MailEnconding = encode ?? System.Text.Encoding.UTF8;

            return this;
        }

        public MessageMailBuilder To(string email)
        {
            return this.To(new MailAddress(email));
        }

        public MessageMailBuilder To(string email, string displayName)
        {
            return this.To(new MailAddress(email, displayName));
        }


        public MessageMailBuilder To(MailAddress to)
        {
            this.Message.To.Add(to);
            return this;
        }

        public MessageMailBuilder To(params MailAddress[] tos)
        {
            foreach (var mailAddress in tos)
            {
                this.Message.To.Add(mailAddress);
            }
            return this;
        }

        public MessageMailBuilder Cc(string email)
        {
            return this.Cc(new MailAddress(email));
        }
        public MessageMailBuilder Cc(string email, string displayName)
        {
            return this.Cc(new MailAddress(email, displayName));
        }

        public MessageMailBuilder Cc(MailAddress cc)
        {
            this.Message.Cc.Add(cc);
            return this;
        }

        public MessageMailBuilder Cc(params MailAddress[] ccs)
        {
            foreach (var c in ccs)
            {
                this.Message.Cc.Add(c);
            }
            return this;
        }

        public MessageMailBuilder Bcc(string email)
        {
            return this.Bcc(new MailAddress(email));
        }

        public MessageMailBuilder Bcc(string email, string displayName)
        {
            return this.Bcc(new MailAddress(email, displayName));
        }

        public MessageMailBuilder Bcc(MailAddress bcc)
        {
            this.Message.Bcc.Add(bcc);
            return this;
        }

        public MessageMailBuilder Bcc(params MailAddress[] bccs)
        {
            foreach (var bcc in bccs)
            {
                this.Message.Bcc.Add(bcc);
            }
            return this;
        }

        public MessageMailBuilder Sender(string email)
        {
            return this.Sender(new MailAddress(email));
        }

        public MessageMailBuilder Sender(string email, string displayName)
        {
            return this.Sender(new MailAddress(email, displayName));
        }

        public MessageMailBuilder Sender(MailAddress sender)
        {
            this.Message.Sender = sender;
            return this;
        }

        public MessageMailBuilder ReplyTo(string email)
        {
            return this.ReplyTo(new MailAddress(email));
        }
        public MessageMailBuilder ReplyTo(string email, string displayName)
        {
            return this.ReplyTo(new MailAddress(email, displayName));
        }

        public MessageMailBuilder ReplyTo(MailAddress replyTo)
        {
            this.Message.ReplyTo = replyTo;
            return this;
        }

        public MessageMailBuilder Attachment(Attachment att)
        {
            this.Message.Att.Add(att);
            return this;
        }

        public MessageMailBuilder Attachment(params Attachment[] atts)
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
