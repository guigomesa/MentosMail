using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MentosMail.MessageBox
{
    public interface IMessageMail
    {
        string BodyMessage { get; set; }
        string Subject { get; set; }
        bool IsBodyHtml { get; set; }
        MailPriority Priority { get; set; }
        Encoding MailEnconding { get; set; }
        IList<MailAddress> To { get; set; }
        IList<MailAddress> Cc { get; set; }
        IList<MailAddress> Bcc { get; set; }
        MailAddress Sender { get; set; }
        MailAddress ReplyTo { get; set; }
        MailAddress From { get; set; }
        IList<Attachment> Att { get; set; }
    }
}
