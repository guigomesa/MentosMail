using System;
using System.Net.Mail;
using System.Text;
using MentosMailCore.MessageBox;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestsMentosMailCore
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBuildMessenger()
        {
            var msg = GetMailMessage();

            Assert.IsInstanceOfType(msg, typeof(MentosMailCore.MessageBox.MessageMail));
            
            Assert.AreEqual(msg.Att.Count,0);
            
            Assert.IsTrue(msg.To.Count==1,"Error in count To msg");
            Assert.AreEqual(msg.From.DisplayName,"MentosMailFrom");
            Assert.AreEqual(msg.From.Address,"from@mentosmailcore.com");
            
            Assert.AreEqual(msg.BodyMessage, "Hello World");
            Assert.AreEqual(msg.BodyMessage, "Subject Email");
            Assert.AreEqual(msg.MailEnconding, Encoding.UTF8);
            Assert.IsTrue(msg.IsBodyHtml);
            Assert.AreEqual(msg.Priority,MailPriority.High);
            
            Assert.AreEqual(msg.ReplyTo.DisplayName, "ReplyToName");
            Assert.AreEqual(msg.ReplyTo.Address, "reply@mentosmailcore.com");
            
            Assert.AreEqual(msg.Sender.DisplayName,"SenderName");
            Assert.AreEqual(msg.Sender.Address,"sender@mentosmailcore.com");
        }

        [TestMethod]
        public void TestTemplateGenerator()
        {
            const string htmlTestBase = @"<html>
                                    <head></head>
                                    <body>
	                                    <p> <strong>Name:</strong> <span>{0}</span> </p>
	                                    <p> <strong>AnotherField</strong> <span>{1}</span></p>
                                    </body>
                                    </html>";

            var htmlTest = String.Format(htmlTestBase, "MentosMail", "OtherField");
            
            var htmlSender = GenerateTemplate("MentosMail", "OtherField");
            
            Assert.AreEqual(htmlTest,htmlSender);
        }
        
        
        
        private static MentosMailCore.SmtpServerConf GetConfigSmtp()
        {
            return new MentosMailCore.SmtpServerConf
            {
                Host = "smtp.host.com",
                Username = "user",
                Password = "pass",
                Port = 587,
                UseDefaultCredential = false,
                UseSsl = false
            };
        }
        
        private static string GenerateTemplate(string name, string anotherField)
        {
            string htmlTemplate = @"<html>
                                    <head></head>
                                    <body>
	                                    <p> <strong>Name:</strong> <span>[Name]</span> </p>
	                                    <p> <strong>AnotherField</strong> <span>[AnotherField]</span></p>
                                    </body>
                                    </html>";

            var model = new
            {
                AnotherField = anotherField,
                Name = name
            };

            var templateService = new MentosMailCore.TemplateService(htmlTemplate);

            return templateService.GenerateTemplateFromAnonymous(model);

        }

        private static MessageMail GetMailMessage()
        {
            var msgBuilder = new MentosMailCore.MessageBox.MessageMailBuilder()
                .To(new MailAddress("to@mentosmailcore.com", "MentosMailName"))
                .From(new MailAddress("from@mentosmailcore.com", "MentosMailFrom"))
                .BodyMessage("Hello World") //send text
                .Subject("Subject Email")
                .Encoding(Encoding.UTF8)
                .IsBodyHtml()
                .Priority(MailPriority.High)
                .ReplyTo(new MailAddress("reply@mentosmailcore.com", "ReplyToName"))
                .Sender(new MailAddress("sender@mentosmailcore.com", "SenderName"));

            var msg = msgBuilder.Build();
            return msg;
        }
    }
}