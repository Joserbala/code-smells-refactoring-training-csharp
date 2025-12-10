using BirthdayGreetingsKata2.Core;
using BirthdayGreetingsKata2.Infrastructure.Repositories;
using NUnit.Framework;


namespace BirthdayGreetingsKata2.Tests.Infrastructure.Repositories;

public class FileEmployeeRepositoryTest
{
    [Test]
    public void Loads_Employees_With_Valid_Path()
    {
        IEmployeesRepository employeesRepository =
            new FileEmployeesRepository(@"D:\code\personal\my-code-smells-refactoring-training-csharp\04-refactoring-to-hexagonal-architecture-2\BirthdayGreetingsKata2.Tests\Infrastructure\Repositories/employee_data1.txt");

        Assert.That(employeesRepository.GetAll().Count, Is.EqualTo(2));
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