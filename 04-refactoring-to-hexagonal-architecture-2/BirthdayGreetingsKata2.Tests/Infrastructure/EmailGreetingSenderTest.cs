using System.Collections.Generic;
using System.Net.Mail;
using BirthdayGreetingsKata2.Application;
using BirthdayGreetingsKata2.Core;
using BirthdayGreetingsKata2.Infrastructure;
using BirthdayGreetingsKata2.Tests.Application;
using BirthdayGreetingsKata2.Tests.helpers;
using NSubstitute;
using NUnit.Framework;

namespace BirthdayGreetingsKata2.Tests.Infrastructure;

public class EmailGreetingSenderTest
{
    private const int SmtpPort = 25;
    private const string SmtpHost = "localhost";
    private const string From = "sender@here.com";
    private List<MailMessage> _messagesSent;
    private GreetingSenderForTesting _messageSenderForTesting;
    private readonly Employee _john = new("John", "Doe", OurDateFactory.OurDate("1982/10/08"), "john.doe@foobar.com");
    private readonly Employee _mary = new("Mary", "Ann", OurDateFactory.OurDate("1975/03/11"), "mary.ann@foobar.com");

    private class GreetingSenderForTesting : EmailGreetingSender
    {
        private readonly List<MailMessage> _messagesSent;

        public GreetingSenderForTesting(List<MailMessage> messagesSent, string smtpHost, int smtpPort, string sender) :
            base(smtpHost, smtpPort, sender)
        {
            _messagesSent = messagesSent;
        }

        protected override void Send(MailMessage msg)
        {
            _messagesSent.Add(msg);
        }
    }

    [SetUp]
    public void SetUp()
    {
        _messagesSent = new List<MailMessage>();
        _messageSenderForTesting = new GreetingSenderForTesting(_messagesSent, SmtpHost, SmtpPort, From);
    }

    [Test]
    public void Send_Email()
    {
        var greetingMessage = GreetingMessage.GenerateForSome([_john]);

        _messageSenderForTesting.Send(greetingMessage);

        Assert.That(_messagesSent.Count, Is.EqualTo(1));

        var message = _messagesSent[0];
        Assert.That(message.Body, Is.EqualTo("Happy Birthday, dear John!"));
        Assert.That(message.Subject, Is.EqualTo("Happy Birthday!"));
        Assert.That(message.To, Has.Exactly(1).Items);
        Assert.That(message.To[0].Address, Is.EqualTo("john.doe@foobar.com"));
    }

    [Test]
    public void Send_Many_Emails()
    {
        var greetingMessage = GreetingMessage.GenerateForSome([_john, _mary]);

        _messageSenderForTesting.Send(greetingMessage);

        Assert.That(_messagesSent.Count, Is.EqualTo(2));

        var johnMessage = _messagesSent[0];
        Assert.That(johnMessage.Body, Is.EqualTo("Happy Birthday, dear John!"));
        Assert.That(johnMessage.Subject, Is.EqualTo("Happy Birthday!"));
        Assert.That(johnMessage.To, Has.Exactly(1).Items);
        Assert.That(johnMessage.To[0].Address, Is.EqualTo("john.doe@foobar.com"));
        
        var maryMessage = _messagesSent[1];
        Assert.That(maryMessage.Body, Is.EqualTo("Happy Birthday, dear Mary!"));
        Assert.That(maryMessage.Subject, Is.EqualTo("Happy Birthday!"));
        Assert.That(maryMessage.To, Has.Exactly(1).Items);
        Assert.That(maryMessage.To[0].Address, Is.EqualTo("mary.ann@foobar.com"));
    }

    [Test]
    public void Send_Zero_Emails()
    {
        _messageSenderForTesting.Send(GreetingMessage.GenerateForSome([]));

        Assert.That(_messagesSent.Count, Is.EqualTo(0));
    }
}