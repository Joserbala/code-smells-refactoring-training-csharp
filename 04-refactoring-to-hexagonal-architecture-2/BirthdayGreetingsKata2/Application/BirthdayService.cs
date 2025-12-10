using System.Collections.Generic;
using System.Net.Mail;
using BirthdayGreetingsKata2.Core;
using BirthdayGreetingsKata2.Infrastructure;

namespace BirthdayGreetingsKata2.Application;

public class BirthdayService
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly IGreetingSender _greetingSender;

    public BirthdayService(IEmployeesRepository employeesRepository, IGreetingSender greetingSender)
    {
        _employeesRepository = employeesRepository;
        _greetingSender = greetingSender;
    }

    public void SendGreetings(OurDate date)
    {
        _greetingSender.Send(GreetingMessagesFor(EmployeesHavingBirthday(date)));
    }

    private static List<GreetingMessage> GreetingMessagesFor(IEnumerable<Employee> employees)
    {
        return GreetingMessage.GenerateForSome(employees);
    }

    private IEnumerable<Employee> EmployeesHavingBirthday(OurDate today)
    {
        return _employeesRepository.GetAll()
            .FindAll(employee => employee.IsBirthday(today));
    }
}