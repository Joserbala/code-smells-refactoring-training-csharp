using System.Collections.Generic;
using System.Net.Mail;
using BirthdayGreetingsKata2.Core;

namespace BirthdayGreetingsKata2.Infrastructure;

public class EmailGreetingSender : IGreetingSender {
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _sender;

    public EmailGreetingSender(string smtpHost, int smtpPort, string sender) {
        _smtpHost = smtpHost;
        _smtpPort = smtpPort;
        _sender = sender;
    }

    public void Send(List<GreetingMessage> messages) {
        foreach (var message in messages)
        {
            var recipient = message.To();
            var body = message.Text();
            var subject = message.Subject();
            this.SendMessage(subject, body, recipient);
        }
    }

    private void SendMessage(string subject, string body, string recipient)
    {
        // Create a mail session
        var smtpClient = new SmtpClient(_smtpHost)
        {
            Port = _smtpPort
        };

        // Construct the message
        var msg = new MailMessage
        {
            From = new MailAddress(_sender),
            Subject = subject,
            Body = body
        };
        msg.To.Add(recipient);

        // Send the message
        SendMessage(msg, smtpClient);
    }

    // made protected for testing :-(

    protected virtual void SendMessage(MailMessage msg, SmtpClient smtpClient)
    {
        smtpClient.Send(msg);
    }
}