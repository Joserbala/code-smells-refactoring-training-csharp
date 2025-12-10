using System.Collections.Generic;
using System.Net.Mail;
using BirthdayGreetingsKata2.Core;
using BirthdayGreetingsKata2.Infrastructure;

namespace BirthdayGreetingsKata2.Application;

public class BirthdayService
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly EmailMessageSender _messageSender;

    public BirthdayService(IEmployeesRepository employeesRepository, EmailMessageSender messageSender)
    {
        _employeesRepository = employeesRepository;
        _messageSender = messageSender;
    }

    public void SendGreetings(OurDate date, string smtpHost, int smtpPort, string sender)
    {
        Send(GreetingMessagesFor(EmployeesHavingBirthday(date)),
            smtpHost, smtpPort, sender);
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

    private void Send(List<GreetingMessage> messages, string smtpHost, int smtpPort, string sender)
    {
        foreach (var message in messages)
        {
            var recipient = message.To();
            var body = message.Text();
            var subject = message.Subject();
            SendMessage(smtpHost, smtpPort, sender, subject, body, recipient);
        }
    }

    private void SendMessage(string smtpHost, int smtpPort, string sender,
        string subject, string body, string recipient)
    {
        // Create a mail session
        var smtpClient = new SmtpClient(smtpHost)
        {
            Port = smtpPort
        };

        // Construct the message
        var msg = new MailMessage
        {
            From = new MailAddress(sender),
            Subject = subject,
            Body = body
        };
        msg.To.Add(recipient);

        // Send the message
        _messageSender.SendMessage(msg, smtpClient);
    }
}