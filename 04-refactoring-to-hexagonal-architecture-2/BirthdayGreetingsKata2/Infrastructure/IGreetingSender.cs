using System.Collections.Generic;
using BirthdayGreetingsKata2.Core;

namespace BirthdayGreetingsKata2.Infrastructure;

public interface IGreetingSender {
    void Send(List<GreetingMessage> messages);
}