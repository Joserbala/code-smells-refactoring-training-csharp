using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BirthdayGreetingsKata2.Application;
using BirthdayGreetingsKata2.Core;
using BirthdayGreetingsKata2.Infrastructure;
using NSubstitute;
using NUnit.Framework;
using static BirthdayGreetingsKata2.Tests.helpers.OurDateFactory;

namespace BirthdayGreetingsKata2.Tests.Application;

public class AcceptanceTest
{
    private const int SmtpPort = 25;
    private const string SmtpHost = "localhost";
    private const string From = "sender@here.com";
    private List<MailMessage> _messagesSent;
    private BirthdayService _service;
    private readonly Employee _john = new("John", "Doe", OurDate("1982/10/08"), "john.doe@foobar.com");
    private readonly Employee _mary = new("Mary", "Ann", OurDate("1975/03/11"), "mary.ann@foobar.com");
    private IGreetingSender _greetingServiceForTesting;
    private const string EmployeesFilePath = "Application/employee_data.txt";

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
        var messageSenderForTesting = new GreetingSenderForTesting(_messagesSent, SmtpHost, SmtpPort, From);
        _greetingServiceForTesting = Substitute.For<IGreetingSender>();

        var mock = Substitute.For<IEmployeesRepository>();

        mock.GetAll().Returns([_john, _mary]);
        _service = new BirthdayService(mock, _greetingServiceForTesting);
    }

    [Test]
    public void Base_Scenario()
    {
        var today = OurDate("2008/10/08");

        _service.SendGreetings(today);

        _greetingServiceForTesting.Received().Send(Arg.Is<List<GreetingMessage>>(x => x.Count == 1));
        _greetingServiceForTesting.Received().Send(Arg.Is<List<GreetingMessage>>(x => x.Single().Equals(GreetingMessage.GenerateForSome(new List<Employee> {_john}).Single())));
    }

    [Test]
    public void Will_Not_Send_Emails_When_Nobodies_Birthday()
    {
        var today = OurDate("2008/01/01");

        _service.SendGreetings(today);

        Assert.That(_messagesSent, Is.Empty);
    }
}