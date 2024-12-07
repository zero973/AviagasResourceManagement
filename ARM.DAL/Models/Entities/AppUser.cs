using System.ComponentModel.DataAnnotations;
using ARM.Core.Enums;

namespace ARM.DAL.Models.Entities;

public class AppUser: BaseActualEntity
{
    
    [MaxLength(200)]
    public string Login { get; set; }
    
    [MaxLength(200)]
    public string PasswordHash { get; set; }
    
    public UsersRoles Role { get; set; }
    
    public Employee Employee { get; set; }
    
    public Guid EmployeeId { get; set; }

    public AppUser()
    {
        
    }
    
    public AppUser(string login, string passwordHash, UsersRoles role, Guid employeeId)
    {
        Login = login;
        PasswordHash = passwordHash;
        Role = role;
        EmployeeId = employeeId;
        Id = employeeId;
    }
    
}