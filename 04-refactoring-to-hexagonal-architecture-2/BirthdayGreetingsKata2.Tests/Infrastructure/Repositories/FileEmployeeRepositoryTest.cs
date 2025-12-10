using System.Linq;
using BirthdayGreetingsKata2.Core;
using BirthdayGreetingsKata2.Infrastructure.Repositories;
using BirthdayGreetingsKata2.Tests.helpers;
using NUnit.Framework;


namespace BirthdayGreetingsKata2.Tests.Infrastructure.Repositories;

public class FileEmployeeRepositoryTest
{
    [Test]
    public void Loads_Employees_With_Valid_Path()
    {
        IEmployeesRepository employeesRepository =
            new FileEmployeesRepository("Infrastructure/Repositories/employee_data.txt");

        var employees = employeesRepository.GetAll();
        Assert.That(employees.Count, Is.EqualTo(2));

        var john = new Employee("John", "Doe", OurDateFactory.OurDate("1982/10/08"), "john.doe@foobar.com");
        Assert.That(employees.Any(e => e.Equals(john)));

        var mary = new Employee("Mary", "Ann", OurDateFactory.OurDate("1975/03/11"), "mary.ann@foobar.com");
        Assert.That(employees.Any(e => e.Equals(mary)));
    }

    [Test]
    public void Fails_When_The_File_Does_Not_Exist()
    {
        IEmployeesRepository employeesRepository = new FileEmployeesRepository("non-existing.file");

        Assert.Throws<CannotReadEmployeesException>(() => employeesRepository.GetAll());
    }

    [Test]
    public void Fails_When_The_File_Contains_A_Wrongly_Formatted_Birthdate()
    {
        IEmployeesRepository employeesRepository = new FileEmployeesRepository(
            "Infrastructure/Repositories/contains-employee-with-wrongly-formatted-birthdate.csv"
        );

        Assert.Throws<CannotReadEmployeesException>(() => employeesRepository.GetAll());
    }

    [Test]
    public void Fails_When_The_File_Is_Missing_Columns()
    {
        IEmployeesRepository employeesRepository = new FileEmployeesRepository(
            "Infrastructure/Repositories/contains-not-enough-fields.csv"
        );

        Assert.Throws<CannotReadEmployeesException>(() => employeesRepository.GetAll());
    }
}