using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.ConsoleApp.Models;
using StudentManagement.ConsoleApp.Repositories;
using StudentManagement.ConsoleApp.Services;

var services = new ServiceCollection();
services.AddSingleton<IStudentRepository, StudentRepository>();
services.AddSingleton<IStudentService, StudentService>();

using var serviceProvider = services.BuildServiceProvider();
var studentService = serviceProvider.GetRequiredService<IStudentService>();

Console.WriteLine("Student Management System");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Register a student");
    Console.WriteLine("2. Display all students");
    Console.WriteLine("3. Search for a student");
    Console.WriteLine("4. Update a student");
    Console.WriteLine("5. Delete a student");
    Console.WriteLine("0. Exit");
    Console.Write("Select an option: ");

    string? selection = Console.ReadLine();
    if (selection is null || selection == "0")
    {
        Console.WriteLine("Goodbye.");
        break;
    }

    switch (selection)
    {
        case "1":
            RegisterStudent();
            break;
        case "2":
            DisplayAllStudents();
            break;
        case "3":
            SearchStudent();
            break;
        case "4":
            UpdateStudent();
            break;
        case "5":
            DeleteStudent();
            break;
        default:
            Console.WriteLine("Invalid option. Select a number from the menu.");
            break;
    }
}

void RegisterStudent()
{
    Console.WriteLine("\nRegister a student");

    int? studentId = ReadInteger("Student ID: ");
    string? firstName = ReadInput("First name: ");
    string? lastName = ReadInput("Last name: ");
    string? email = ReadInput("Email: ");
    int? age = ReadInteger("Age: ");
    string? course = ReadInput("Course: ");

    if (studentId is null || firstName is null || lastName is null ||
        email is null || age is null || course is null)
    {
        Console.WriteLine("Registration cancelled because input ended.");
        return;
    }

    var student = new Student
    {
        StudentId = studentId.Value,
        FirstName = firstName,
        LastName = lastName,
        Email = email,
        Age = age.Value,
        Course = course
    };

    try
    {
        Console.WriteLine(studentService.AddStudent(student)
            ? $"Student {student.StudentId} was registered successfully."
            : $"Student ID {student.StudentId} is already in use.");
    }
    catch (ValidationException exception)
    {
        Console.WriteLine($"Student information is invalid: {exception.Message}");
    }
}

void DisplayAllStudents()
{
    List<Student> students = studentService.GetAllStudents();
    if (students.Count == 0)
    {
        Console.WriteLine("There are no students to display.");
        return;
    }

    Console.WriteLine("\nStudent records");
    foreach (Student student in students)
    {
        DisplayStudent(student);
    }
}

void SearchStudent()
{
    int? studentId = ReadInteger("Enter the student ID to search for: ");
    if (studentId is null)
    {
        Console.WriteLine("Search cancelled because input ended.");
        return;
    }

    Student? student = studentService.GetStudentById(studentId.Value);
    if (student is null)
    {
        Console.WriteLine($"No student was found with ID {studentId.Value}.");
        return;
    }

    DisplayStudent(student);
}

void UpdateStudent()
{
    int? studentId = ReadInteger("Enter the student ID to update: ");
    if (studentId is null)
    {
        Console.WriteLine("Update cancelled because input ended.");
        return;
    }

    Student? existingStudent = studentService.GetStudentById(studentId.Value);
    if (existingStudent is null)
    {
        Console.WriteLine($"No student was found with ID {studentId.Value}.");
        return;
    }

    Console.WriteLine("Press Enter to keep the current value.");
    string? firstName = ReadOptionalText($"First name [{existingStudent.FirstName}]: ", existingStudent.FirstName);
    string? lastName = ReadOptionalText($"Last name [{existingStudent.LastName}]: ", existingStudent.LastName);
    string? email = ReadOptionalText($"Email [{existingStudent.Email}]: ", existingStudent.Email);
    int? age = ReadOptionalInteger($"Age [{existingStudent.Age}]: ", existingStudent.Age);
    string? course = ReadOptionalText($"Course [{existingStudent.Course}]: ", existingStudent.Course);

    if (firstName is null || lastName is null || email is null || age is null || course is null)
    {
        Console.WriteLine("Update cancelled because input ended.");
        return;
    }

    var updatedStudent = new Student
    {
        StudentId = existingStudent.StudentId,
        FirstName = firstName,
        LastName = lastName,
        Email = email,
        Age = age.Value,
        Course = course
    };

    try
    {
        Console.WriteLine(studentService.UpdateStudent(updatedStudent)
            ? $"Student {updatedStudent.StudentId} was updated successfully."
            : $"Student ID {updatedStudent.StudentId} no longer exists.");
    }
    catch (ValidationException exception)
    {
        Console.WriteLine($"Student information is invalid: {exception.Message}");
    }
}

void DeleteStudent()
{
    int? studentId = ReadInteger("Enter the student ID to delete: ");
    if (studentId is null)
    {
        Console.WriteLine("Deletion cancelled because input ended.");
        return;
    }

    Student? student = studentService.GetStudentById(studentId.Value);
    if (student is null)
    {
        Console.WriteLine($"No student was found with ID {studentId.Value}.");
        return;
    }

    DisplayStudent(student);
    string? confirmation = ReadInput("Delete this student? (y/n): ");
    if (!string.Equals(confirmation, "y", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Deletion cancelled.");
        return;
    }

    Console.WriteLine(studentService.DeleteStudent(studentId.Value)
        ? $"Student {studentId.Value} was deleted successfully."
        : $"Student ID {studentId.Value} no longer exists.");
}

static void DisplayStudent(Student student)
{
    Console.WriteLine(
        $"ID: {student.StudentId} | Name: {student.FirstName} {student.LastName} | " +
        $"Email: {student.Email} | Age: {student.Age} | Course: {student.Course}");
}

static string? ReadInput(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine();
}

static int? ReadInteger(string prompt)
{
    while (true)
    {
        string? input = ReadInput(prompt);
        if (input is null)
        {
            return null;
        }

        if (int.TryParse(input, out int value))
        {
            return value;
        }

        Console.WriteLine("Please enter a valid whole number.");
    }
}

static string? ReadOptionalText(string prompt, string currentValue)
{
    string? input = ReadInput(prompt);
    return input is null ? null : string.IsNullOrWhiteSpace(input) ? currentValue : input;
}

static int? ReadOptionalInteger(string prompt, int currentValue)
{
    while (true)
    {
        string? input = ReadInput(prompt);
        if (input is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            return currentValue;
        }

        if (int.TryParse(input, out int value))
        {
            return value;
        }

        Console.WriteLine("Please enter a valid whole number, or press Enter to keep the current value.");
    }
}
