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
    
    // made protected for testing :-(
    protected virtual void Send(MailMessage msg) {
        new SmtpClient(_smtpHost, _smtpPort).Send(msg);
    }

    private MailMessage MailMessageFrom(GreetingMessage greeting) {
        return new MailMessage(_sender, greeting.To(), greeting.Subject(), greeting.Text());
    }
}