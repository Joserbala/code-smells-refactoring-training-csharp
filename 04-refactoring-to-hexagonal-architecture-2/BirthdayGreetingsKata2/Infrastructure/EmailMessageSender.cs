using System.Net.Mail;

namespace BirthdayGreetingsKata2.Infrastructure;

public class EmailMessageSender
{
    // made protected for testing :-(
    public virtual void SendMessage(MailMessage msg, SmtpClient smtpClient)
    {
        smtpClient.Send(msg);
    }

}