using BirthdayGreetingsKata2.Core;
using BirthdayGreetingsKata2.Infrastructure.Repositories;
using NUnit.Framework;


namespace BirthdayGreetingsKata2.Tests.Infrastructure.Repositories;

public class FileEmployeeRepositoryTest
{
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