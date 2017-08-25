using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MentosMail.AttributesHelper;


namespace UsageExampleMentosMail
{
    class Program
    {
        static void Main(string[] args)
        {
            MentosMail.SmtpServer serverEmail = new MentosMail.SmtpServer(GetConfigSmtp());
            var msg = GetMessage();
            serverEmail.Send(msg);
            Console.WriteLine("Email enviado");
            Console.ReadLine();

        }

        static MentosMail.MessageBox.MessageMail GetMessage()
        {
            var msgBuilder = new MentosMail.MessageBox.MessageMailBuilder()
                .To(new MailAddress("to@email.com","to display name"))
                .From(new MailAddress("from@email.com","from name"))
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

            var templateService = new MentosMail.TemplateService(htmlTemplate);

            return templateService.GenerateTemplateFromViewModel(model);

        }


        static MentosMail.SmtpServerConf GetConfigSmtp()
        {
            return new MentosMail.SmtpServerConf
            {
                Host = "smtp.host.com",
                Username = "user",
                Password = "pass",
                Port = 587,
                UseDefaultCredential = false,
                UseSsl = false
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
