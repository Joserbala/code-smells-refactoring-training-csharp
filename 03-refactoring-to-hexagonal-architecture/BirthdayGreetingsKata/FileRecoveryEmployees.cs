using System.Collections.Generic;
using System.IO;

namespace BirthdayGreetingsKata;

public class FileRecoveryEmployees : RecoveryEmployees
{
    private readonly string _fileName;

    public FileRecoveryEmployees(string fileName)
    {
        _fileName = fileName;
    }

    public List<Employee> Get()
    {
        using var reader = new StreamReader(_fileName);
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
}