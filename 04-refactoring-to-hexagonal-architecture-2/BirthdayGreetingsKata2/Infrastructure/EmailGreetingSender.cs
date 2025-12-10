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
            SendMessage(ConstructMessage(message));
        }
    }

    private MailMessage ConstructMessage(GreetingMessage message) {
        var msg = new MailMessage
        {
                From = new MailAddress(_sender),
                Subject = message.Subject(),
                Body = message.Text()
        };
        msg.To.Add(message.To());
        return msg;
    }

    private SmtpClient CreateMailSession() {
        return new SmtpClient(_smtpHost)
        {
                Port = _smtpPort
        };
    }

    // made protected for testing :-(
    protected virtual void SendMessage(MailMessage msg)
    {
        CreateMailSession().Send(msg);
    }
}