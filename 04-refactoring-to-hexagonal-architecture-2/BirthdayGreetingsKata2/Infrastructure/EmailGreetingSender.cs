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

    public void Send(List<GreetingMessage> greetings) {
        foreach (var greeting in greetings)
        {
            Send(MailMessageFrom(greeting));
        }
    }

    private MailMessage MailMessageFrom(GreetingMessage greeting) {
        var msg = new MailMessage
        {
                From = new MailAddress(_sender),
                Subject = greeting.Subject(),
                Body = greeting.Text()
        };
        msg.To.Add(greeting.To());
        return msg;
    }

    // made protected for testing :-(
    protected virtual void Send(MailMessage msg)
    {
        new SmtpClient(_smtpHost, _smtpPort).Send(msg);
    }
}