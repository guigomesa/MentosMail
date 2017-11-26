# MentosMail
Send Email with .Net Framework.

Simple usage.

## Build

### NetCore Build

![Travis Build](https://api.travis-ci.org/guigomesa/MentosMail.svg?branch=master)

### NetStandart

n/a


## Install

Use nuget package to install

``` powershell
    Install-Package MentosMail
```

## Usage

To use instace one server 'SmtpServer'

``` csharp
   MentosMail.SmtpServer serverEmail = new MentosMail.SmtpServer(GetConfigSmtp());
   serverEmail.Send(GetMessage());
```

### The object to configure smtp sender is very simple

``` csharp
    static MentosMail.SmtpServerConf GetConfigSmtp()
        {
            return new MentosMail.SmtpServerConf
            {
                Host = "smtp.gmail.com",
                Username = "email@gmail.com",
                Password = "your-password",
                Port = 465,
                UseDefaultCredential = false,
                UseSsl = true
            };
        }
```

### There is support for an email message builder

``` csharp
    static MessageMail GetMessage()
        static MentosMail.MessageBox.MessageMail GetMessage()
        {
            var msgBuilder = new MentosMail.MessageBox.MessageMailBuilder()
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
        var templateService = new MentosMail.TemplateService(htmlTemplate);

        return templateService.GenerateTemplateFromViewModel(model);
    }
```

Check the "UsageExampleMentosMail" project to see it in action


## Future

In future support for sender servers (MailGun etc) and assync methods to send email.


## Is not supported yet:

* Send email assynchronous method (throw exception if invoke methods)
* Retrieve templates from file or url (throw exception if invoke methods)


## Thanks

@miltonfilho thanks for comments (:
