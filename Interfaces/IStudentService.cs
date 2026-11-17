using StudentManagement.ConsoleApp.Models;

public interface IStudentService
{
    /// <summary>
    /// Adds a new student to the system.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <returns><see langword="true"/> if added; otherwise, <see langword="false"/> if the ID already exists.</returns>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    /// The student contains invalid data.
    /// </exception>
    bool AddStudent(Student student);

    /// <summary>
    /// Retrieves all students.
    /// </summary>
    /// <returns>A list containing all students.</returns>
    List<Student> GetAllStudents();

    /// <summary>
    /// Retrieves a student by their unique identifier.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student.</param>
    /// <returns>The student with the specified ID, or null if not found.</returns>
    Student? GetStudentById(int studentId);

    /// <summary>
    /// Updates an existing student's information.
    /// </summary>
    /// <param name="student">The student with updated information.</param>
    /// <returns><see langword="true"/> if updated; otherwise, <see langword="false"/> if not found.</returns>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
    /// The student contains invalid data.
    /// </exception>
    bool UpdateStudent(Student student);

    /// <summary>
    /// Deletes a student from the system by their unique identifier.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student to delete.</param>
    /// <returns><see langword="true"/> if deleted; otherwise, <see langword="false"/> if not found.</returns>
    bool DeleteStudent(int studentId);
}