using System;
using BirthdayGreetingsKata2.Core;
using BirthdayGreetingsKata2.Infrastructure;
using BirthdayGreetingsKata2.Infrastructure.Repositories;

namespace BirthdayGreetingsKata2.Application;

public class App
{
    private const string EmployeesFilePath = "employee_data.txt";
    private const string SenderEmailAddress = "sender@here.com";
    private const string Host = "localhost";
    private const int SmtpPort = 25;

    static void Main(string[] args)
    {
        var service = new BirthdayService(
            new FileEmployeesRepository(EmployeesFilePath), new EmailMessageSender());
        try
        {
            var today = new OurDate(new DateTime());
            service.SendGreetings(today, Host, SmtpPort, SenderEmailAddress);
        }
        catch (Exception e)
        {
            Console.Write(e.StackTrace);
        }
    }
}