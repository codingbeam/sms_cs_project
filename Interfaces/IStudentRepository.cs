using StudentManagement.ConsoleApp.Models;

public interface IStudentRepository
{
    /// <summary>
    /// Adds a new student to the repository.
    /// </summary>
    /// <param name="student">The student to add.</param>
    void Add(Student student);

    /// <summary>
    /// Retrieves a student by their unique identifier.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student.</param>
    /// <returns>The student with the specified ID, or null if not found.</returns>
    Student? GetById(int studentId);

    /// <summary>
    /// Retrieves all students from the repository.
    /// </summary>
    List<Student> GetAll();

    /// <summary>
    /// Updates an existing student's information in the repository.
    /// </summary>
    /// <param name="student">The student with updated information.</param>
    bool Update(Student student);

    /// <summary>
    /// Deletes a student from the repository by their unique identifier.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student to delete.</param>
    bool Delete(int studentId);
}