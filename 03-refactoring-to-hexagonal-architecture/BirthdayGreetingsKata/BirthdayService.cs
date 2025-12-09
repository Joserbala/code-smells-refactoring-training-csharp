using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace BirthdayGreetingsKata;

public class BirthdayService
{
    public void SendGreetings(string fileName, OurDate ourDate,
        string smtpHost, int smtpPort)
    {
        var employees = GetEmployees(fileName);

        foreach (var employee in employees)
        {
            if (employee.IsBirthday(ourDate))
            {
                var recipient = employee.Email;
                var body = "Happy Birthday, dear %NAME%!".Replace("%NAME%",
                    employee.FirstName);
                var subject = "Happy Birthday!";
                SendMessage(smtpHost, smtpPort, "sender@here.com", subject,
                    body, recipient);
            }
        }
    }

    private List<Employee> GetEmployees(string fileName)
    {
        using var reader = new StreamReader(fileName);
        var str = "";
        str = reader.ReadLine(); // skip header
        
        var employees = new List<Employee>();
        
        while ((str = reader.ReadLine()) != null)
        {
            var employeeData = str.Split(", ");
            var employee = new Employee(employeeData[1], employeeData[0],
                employeeData[2], employeeData[3]);
            
            employees.Add(employee);
        }

        return employees;
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
        SendMessage(msg, smtpClient);
    }

    // made protected for testing :-(
    protected virtual void SendMessage(MailMessage msg, SmtpClient smtpClient)
    {
        smtpClient.Send(msg);
    }

    static void Main(string[] args)
    {
        var service = new BirthdayService();
        try
        {
            service.SendGreetings("employee_data.txt",
                new OurDate("2008/10/08"), "localhost", 25);
        }
        catch (Exception e)
        {
            Console.Write(e.StackTrace);
        }
    }
}