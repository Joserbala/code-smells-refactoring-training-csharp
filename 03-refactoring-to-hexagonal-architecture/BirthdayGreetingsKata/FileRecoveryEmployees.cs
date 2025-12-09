using System;
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

    public List<Employee> GetAll()
    {
        using var reader = new StreamReader(_fileName);
        var str = "";
        str = reader.ReadLine(); // skip header
        
        var employees = new List<Employee>();
        
        while ((str = reader.ReadLine()) != null)
        {
            var employeeData = str.Split(", ");
            var employee = new Employee(employeeData[1], employeeData[0],
                CreateOurDate(employeeData), employeeData[3]);
            
            employees.Add(employee);
        }

        return employees;
    }

    private static OurDate CreateOurDate(string[] employeeData)
    {
        return new OurDate(DateTime.ParseExact(employeeData[2], "yyyy/MM/dd", null));
    }
}