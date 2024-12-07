using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARM.DAL.Models.Entities;

public class Employee : BaseActualEntity
{
    
    /// <summary>
    /// Имя
    /// </summary>
    [MaxLength(50)]
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    [MaxLength(50)]
    public string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    [MaxLength(50)]
    public string Patronymic { get; set; }

    /// <summary>
    /// День рождения
    /// </summary>
    [Column(TypeName = "timestamp without time zone")]
    public DateTime Birthday { get; set; }
 
    /// <summary>
    /// Номер паспорта
    /// </summary>
    [MaxLength(10)]
    public string Passport { get; set; }
    
    /// <summary>
    /// Url картинки сотрудника
    /// </summary>
    [Column(TypeName = "text")]
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Связанный пользователь
    /// </summary>
    public AppUser? User { get; set; }
    
    /// <summary>
    /// Зарплата
    /// </summary>
    public EmployeeSalary? Salary { get; set; }
    
    public ICollection<SystemTask> Tasks { get; set; }
    
    public ICollection<TaskEmployee> TaskEmployees { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
    
    public ICollection<WorkedTime> WorkedTimes { get; set; }

    public Employee(string firstName, string lastName, string patronymic, DateTime birthday, string passport)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
        Birthday = birthday;
        Passport = passport;
    }
    
}