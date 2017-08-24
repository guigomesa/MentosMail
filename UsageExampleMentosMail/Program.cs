using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MentosMail;
using MentosMail.AttributesHelper;
using MentosMail.MessageBox;

namespace UsageExampleMentosMail
{
    class Program
    {
        static void Main(string[] args)
        {
            SmtpServer serverEmail = new SmtpServer(GetConfigSmtp());

            serverEmail.Send(GetMessage());

        }

        static MessageMail GetMessage()
        {
            var msgBuilder = new MessageMailBuilder()
                .To(new MailAddress("to@email.com","to display name"))
                .BodyMessage(GenerateTemplate()) //send text
                .Subject("Subject Email")
                .Encoding(Encoding.UTF8)
                .IsBodyHtml()
                .Priority(MailPriority.High)
                .ReplyTo(new MailAddress("reply@email.com","name for display"))
                .Sender(new MailAddress("sender@email.com","sender display name"));

            return msgBuilder.Build();
        }

        static string GenerateTemplate()
        {
            string htmlTemplate = @"<html>
                                    <head></head>
                                    <body>
	                                    <p> <strong>Name:</strong> <span>[Name]</span> </p>
	                                    <p> <strong>AnotherField</strong> <span>[(phone)]</span></p>
                                    </body>
                                    </html>";

            var model = new ModelExample
            {
                AnotherField = "This is a field",
                Name = "Guilherme Almeida (guigomesa)"
            };

            var templateService = new TemplateService(htmlTemplate);

            return templateService.GenerateTemplateFromViewModel(model);

        }


        static SmtpServerConf GetConfigSmtp()
        {
            return new SmtpServerConf
            {
                Host = "smtp.gmail.com",
                Username = "email@gmail.com",
                Password = "your-password",
                Port = 465,
                UseDefaultCredential = false,
                UseSsl = true
            };
        }
    }

    internal class ModelExample
    {
        //usage:
        //name of field in []
        // [Name]
        //for replace in template string
        [SenderFieldInMail] 
        public string Name { get; set; }


        //usage:
        //name in property in attribute
        //in this example [(phone)]
        //for replace in template string
        [SenderFieldInMail("[(phone)]")]
        public string AnotherField { get; set; }
    }
    
}
