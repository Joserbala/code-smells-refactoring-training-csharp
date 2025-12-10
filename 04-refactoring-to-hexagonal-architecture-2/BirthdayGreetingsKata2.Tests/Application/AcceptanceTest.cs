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
    private BirthdayService _service;
    private readonly Employee _john = new("John", "Doe", OurDate("1982/10/08"), "john.doe@foobar.com");
    private readonly Employee _mary = new("Mary", "Ann", OurDate("1975/03/11"), "mary.ann@foobar.com");
    private IGreetingSender _greetingSenderMock;

    [SetUp]
    public void SetUp()
    {
        _greetingSenderMock = Substitute.For<IGreetingSender>();

        var mock = Substitute.For<IEmployeesRepository>();

        mock.GetAll().Returns([_john, _mary]);
        _service = new BirthdayService(mock, _greetingSenderMock);
    }

    [Test]
    public void Base_Scenario()
    {
        var today = OurDate("2008/10/08");

        _service.SendGreetings(today);

        _greetingSenderMock.Received().Send(Arg.Is<List<GreetingMessage>>(x => x.Count == 1));
        _greetingSenderMock.Received().Send(Arg.Is<List<GreetingMessage>>(x =>
            x.Single().Equals(GreetingMessage.GenerateForSome(new List<Employee> { _john }).Single())));
    }

    [Test]
    public void Will_Not_Send_Emails_When_Nobodies_Birthday()
    {
        var today = OurDate("2008/01/01");

        _service.SendGreetings(today);

        _greetingSenderMock.Received().Send(Arg.Is<List<GreetingMessage>>(x => x.Count == 0));
    }
}