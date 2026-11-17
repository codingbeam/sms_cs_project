using System.ComponentModel.DataAnnotations;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Services;

/// <summary>
/// Provides student-management operations and applies student validation rules.
/// </summary>
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudentService"/> class.
    /// </summary>
    /// <param name="studentRepository">The repository used to access student records.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="studentRepository"/> is <see langword="null"/>.
    /// </exception>
    public StudentService(IStudentRepository studentRepository)
    {
        ArgumentNullException.ThrowIfNull(studentRepository);
        _studentRepository = studentRepository;
    }

    /// <summary>
    /// Validates and adds a student unless their ID already exists.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <returns><see langword="true"/> if added; otherwise, <see langword="false"/> if the ID exists.</returns>
    /// <exception cref="ValidationException">The student contains invalid data.</exception>
    public bool AddStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        ValidateStudent(student);

        if (_studentRepository.GetById(student.StudentId) is not null)
        {
            return false;
        }

        _studentRepository.Add(student);
        return true;
    }

    /// <summary>
    /// Retrieves all students.
    /// </summary>
    /// <returns>A list containing all students.</returns>
    public List<Student> GetAllStudents()
    {
        return _studentRepository.GetAll();
    }

    /// <summary>
    /// Retrieves a student by their identifier.
    /// </summary>
    /// <param name="studentId">The identifier of the student to retrieve.</param>
    /// <returns>The matching student, or <see langword="null"/> if not found.</returns>
    public Student? GetStudentById(int studentId)
    {
        return _studentRepository.GetById(studentId);
    }

    /// <summary>
    /// Validates and updates an existing student's information.
    /// </summary>
    /// <param name="student">The student containing the updated information.</param>
    /// <returns><see langword="true"/> if updated; otherwise, <see langword="false"/> if not found.</returns>
    /// <exception cref="ValidationException">The student contains invalid data.</exception>
    public bool UpdateStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        ValidateStudent(student);

        if (_studentRepository.GetById(student.StudentId) is null)
        {
            return false;
        }

        return _studentRepository.Update(student);
    }

    /// <summary>
    /// Deletes a student by their identifier.
    /// </summary>
    /// <param name="studentId">The identifier of the student to delete.</param>
    /// <returns><see langword="true"/> if deleted; otherwise, <see langword="false"/> if not found.</returns>
    public bool DeleteStudent(int studentId)
    {
        return _studentRepository.Delete(studentId);
    }

    private static void ValidateStudent(Student student)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(student);

        if (!Validator.TryValidateObject(student, validationContext, validationResults, validateAllProperties: true))
        {
            string errorMessage = string.Join(
                Environment.NewLine,
                validationResults
                    .Where(result => !string.IsNullOrWhiteSpace(result.ErrorMessage))
                    .Select(result => result.ErrorMessage));

            throw new ValidationException(errorMessage);
        }
    }
}