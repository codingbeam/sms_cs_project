using System.Text.Json;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Repositories;

/// <summary>
/// Stores student records in a JSON file.
/// </summary>
public class StudentRepository : IStudentRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _dataFilePath;
    private List<Student> _students;

    /// <summary>
    /// Initializes the repository and loads students from the JSON data file.
    /// Creates the data directory and an empty JSON array when the file does not exist.
    /// </summary>
    /// <exception cref="InvalidDataException">
    /// The data file contains invalid JSON or does not contain a student collection.
    /// </exception>
    public StudentRepository()
    {
        _dataFilePath = Path.GetFullPath(Path.Combine("Data", "students.json"));
        string? directory = Path.GetDirectoryName(_dataFilePath);
        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_dataFilePath))
        {
            _students = new List<Student>();
            SaveStudents(_students);
            return;
        }

        string json = File.ReadAllText(_dataFilePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            _students = new List<Student>();
            return;
        }

        List<Student?>? loadedStudents;
        try
        {
            loadedStudents = JsonSerializer.Deserialize<List<Student?>>(json, SerializerOptions);
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException(
                $"The student data file '{_dataFilePath}' contains invalid JSON. " +
                "The existing file was not changed.",
                exception);
        }

        if (loadedStudents is null)
        {
            throw new InvalidDataException(
                $"The student data file '{_dataFilePath}' must contain a JSON array.");
        }

        var studentIds = new HashSet<int>();
        foreach (Student? student in loadedStudents)
        {
            if (student is null)
            {
                throw new InvalidDataException(
                    $"The student data file '{_dataFilePath}' contains a null student entry.");
            }

            if (!studentIds.Add(student.StudentId))
            {
                throw new InvalidDataException(
                    $"The student data file '{_dataFilePath}' contains duplicate student ID {student.StudentId}.");
            }
        }

        _students = loadedStudents.Select(student => student!).ToList();
    }

    /// <summary>
    /// Adds a student and saves the updated collection to the JSON file.
    /// </summary>
    /// <param name="student">The student to add.</param>
    public void Add(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (_students.Exists(existingStudent => existingStudent.StudentId == student.StudentId))
        {
            throw new InvalidOperationException(
                $"A student with ID {student.StudentId} already exists.");
        }

        List<Student> updatedStudents = new(_students)
        {
            CopyStudent(student)
        };

        SaveStudents(updatedStudents);
        _students = updatedStudents;
    }

    /// <summary>
    /// Retrieves a copy of all students in the repository.
    /// </summary>
    /// <returns>A new list containing the repository's students.</returns>
    public List<Student> GetAll()
    {
        return _students.Select(CopyStudent).ToList();
    }

    /// <summary>
    /// Retrieves a student by their identifier.
    /// </summary>
    /// <param name="studentId">The identifier of the student to retrieve.</param>
    /// <returns>The matching student, or <see langword="null"/> if not found.</returns>
    public Student? GetById(int studentId)
    {
        Student? student = _students.Find(existingStudent => existingStudent.StudentId == studentId);
        return student is null ? null : CopyStudent(student);
    }

    /// <summary>
    /// Replaces an existing student and saves the updated collection to the JSON file.
    /// </summary>
    /// <param name="student">The student containing the updated information.</param>
    /// <returns><see langword="true"/> if the student was updated; otherwise, <see langword="false"/>.</returns>
    public bool Update(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        int index = _students.FindIndex(existingStudent => existingStudent.StudentId == student.StudentId);
        if (index < 0)
        {
            return false;
        }

        List<Student> updatedStudents = new(_students);
        updatedStudents[index] = CopyStudent(student);

        SaveStudents(updatedStudents);
        _students = updatedStudents;
        return true;
    }

    /// <summary>
    /// Removes a student and saves the updated collection to the JSON file.
    /// </summary>
    /// <param name="studentId">The identifier of the student to remove.</param>
    /// <returns><see langword="true"/> if the student was removed; otherwise, <see langword="false"/>.</returns>
    public bool Delete(int studentId)
    {
        int index = _students.FindIndex(student => student.StudentId == studentId);
        if (index < 0)
        {
            return false;
        }

        List<Student> updatedStudents = new(_students);
        updatedStudents.RemoveAt(index);

        SaveStudents(updatedStudents);
        _students = updatedStudents;
        return true;
    }

    private void SaveStudents(List<Student> students)
    {
        string json = JsonSerializer.Serialize(students, SerializerOptions);
        string directory = Path.GetDirectoryName(_dataFilePath)!;
        string temporaryFilePath = Path.Combine(
            directory,
            $"{Path.GetFileName(_dataFilePath)}.{Guid.NewGuid():N}.tmp");

        try
        {
            File.WriteAllText(temporaryFilePath, json);
            File.Move(temporaryFilePath, _dataFilePath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryFilePath))
            {
                File.Delete(temporaryFilePath);
            }
        }
    }

    private static Student CopyStudent(Student student)
    {
        return new Student
        {
            StudentId = student.StudentId,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Age = student.Age,
            Course = student.Course
        };
    }
}