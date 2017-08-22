using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MentosMail.MessageBox
{
    public class MessageMail : IMessageMail
    {
        public string BodyMessage { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
        public MailPriority Priority { get; set; }
        public Encoding MailEnconding { get; set; }
        public IList<MailAddress> To { get; set; } = new List<MailAddress>();
        public IList<MailAddress> Cc { get; set; } = new List<MailAddress>();
        public IList<MailAddress> Bcc { get; set; } = new List<MailAddress>();
        public MailAddress Sender { get; set; }
        public MailAddress ReplyTo { get; set; }
        public IList<Attachment> Att { get; set; } = new List<Attachment>();
    }
}
