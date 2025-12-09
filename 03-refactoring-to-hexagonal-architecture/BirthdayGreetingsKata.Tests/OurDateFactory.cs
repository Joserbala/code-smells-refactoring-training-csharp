using System;

namespace BirthdayGreetingsKata.Tests;

public static class OurDateFactory
{
    public static OurDate GetDateFromString(string stringDate)
    {
        return new OurDate(DateTime.ParseExact(stringDate, "yyyy/MM/dd", null));
    }
}