using System.ComponentModel.DataAnnotations.Schema;

namespace ARM.DAL.Models.Entities;

/// <summary>
/// Зарплата сотрудника в час
/// </summary>
public class EmployeeSalary : BaseActualEntity
{
    
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Employee Employee { get; set; }
    
    /// <summary>
    /// Текущий исполнитель задачи - Employee.Id
    /// </summary>
    public Guid EmployeeId { get; set; }
    
    /// <summary>
    /// Зарплата сотрудника в час
    /// </summary>
    public decimal SalaryForOneHour { get; set; }
    
    /// <summary>
    /// Дата начала действия зарплаты
    /// </summary>
    [Column(TypeName = "timestamp without time zone")]
    public DateTime Start { get; set; }
    
    /// <summary>
    /// Дата окончания действия зарплаты
    /// </summary>
    [Column(TypeName = "timestamp without time zone")]
    public DateTime End { get; set; }

    public EmployeeSalary()
    {
        
    }

    public EmployeeSalary(Guid employeeId, decimal salaryForOneHour, DateTime start, DateTime end)
    {
        EmployeeId = employeeId;
        SalaryForOneHour = salaryForOneHour;
        Start = start;
        End = end;
    }
    
}