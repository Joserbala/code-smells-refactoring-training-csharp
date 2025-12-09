using NUnit.Framework;

namespace BirthdayGreetingsKata.Tests;

public class OurDateTest
{
    [Test]
    public void Is_Same_Date()
    {
        var ourDate = OurDateFactory.GetDateFromString("1789/01/24");
        var sameDay = OurDateFactory.GetDateFromString("2001/01/24");
        var notSameDay = OurDateFactory.GetDateFromString("1789/01/25");
        var notSameMonth = OurDateFactory.GetDateFromString("1789/02/24");

        Assert.That(ourDate.IsSameDay(sameDay), Is.True, "same");
        Assert.That(ourDate.IsSameDay(notSameDay), Is.False, "not same day");
        Assert.That(ourDate.IsSameDay(notSameMonth), Is.False, "not same month");
    }
}