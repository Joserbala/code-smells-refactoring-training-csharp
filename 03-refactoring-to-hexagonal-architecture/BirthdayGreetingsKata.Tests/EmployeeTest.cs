using NUnit.Framework;

namespace BirthdayGreetingsKata.Tests;

public class EmployeeTest
{
    [Test]
    public void Test_Birthday()
    {
        var employee = new Employee("foo", "bar", OurDateFactory.GetDateFromString("1990/01/31"), "a@b.c");
        Assert.That(employee.IsBirthday(OurDateFactory.GetDateFromString("2008/01/30")),
            Is.False,
            "not his birthday");
        Assert.That(employee.IsBirthday(OurDateFactory.GetDateFromString("2008/01/31")),
            Is.True,
            "his birthday");
    }
}