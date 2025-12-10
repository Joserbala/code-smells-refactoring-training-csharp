using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BirthdayGreetingsKata2.Core;

namespace BirthdayGreetingsKata2.Infrastructure;

public class EmailGreetingSender(string smtpHost, int smtpPort, string sender) : IGreetingSender
{
    public void Send(List<GreetingMessage> greetings) => greetings.Select(MailMessageFrom).ToList().ForEach(Send);

    // made protected for testing :-(
    protected virtual void Send(MailMessage msg) => new SmtpClient(smtpHost, smtpPort).Send(msg);

    private MailMessage MailMessageFrom(GreetingMessage greeting)
        => new(sender, greeting.To(), greeting.Subject(), greeting.Text());
}