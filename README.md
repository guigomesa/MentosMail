# MentosMail
Send Email with .Net Framework


## Usage

To use instace one server 'SmtpServer'

``` csharp
   SmtpServer serverEmail = new SmtpServer(GetConfigSmtp());
   serverEmail.Send(GetMessage());
```

### The object to configure smtp sender is very simple

``` csharp
    var conf = new SmtpServerConf
            {
                Host = "smtp.gmail.com",
                Username = "email@gmail.com",
                Password = "your-password",
                Port = 465,
                UseDefaultCredential = false,
                UseSsl = true
            };
```

### There is support for an email message builder

``` csharp
    static MessageMail GetMessage()
        {
            var msgBuilder = new MessageMailBuilder()
                .AddTo(new MailAddress("to@email.com","to display name"))
                .CreateBodyMessage(GenerateTemplate()) //send text
                .CreateSubject("Subject Email")
                .SetEncoding(Encoding.UTF8)
                .SetIsBodyHtml(true)
                .SetPriority(MailPriority.High)
                .SetReplyTo(new MailAddress("reply@email.com","name for display"))
                .SetSender(new MailAddress("sender@email.com","sender display name"));

            return msgBuilder.Build();
        }
```

And also support models with attributes to mark the sending by email facilitating the substitution of terms in a template

See the example below:

``` csharp
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

            var templateService = new TemplateService(model, htmlTemplate);

            return templateService.GenerateTemplate();

        }
```


Check the "UsageExampleMentosMail" project to see it in action


# Future

In future support for the nuget package and other sender servers (MailGun etc).


## Is not supported yet:

* Send email assynchronous method
* Retrieve templates from file or url