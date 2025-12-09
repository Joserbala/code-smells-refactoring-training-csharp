using System;

namespace BirthdayGreetingsKata.Tests;

public static class OurDateFactory
{
    public static OurDate GetDateFromString(string stringDate)
    {
        var date = DateTime.ParseExact(stringDate, "yyyy/MM/dd", null);
        return new OurDate(date);
    }
}