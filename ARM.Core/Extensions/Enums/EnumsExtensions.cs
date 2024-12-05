using ARM.Core.Enums;

namespace ARM.Core.Extensions.Enums;

public static class EnumsExtensions
{

    /// <summary>
    /// Возвращает строкове представление статуса задачи
    /// </summary>
    public static string TaskStatusToString(this TaskStatuses status)
    {
        return status switch
        {
            TaskStatuses.New => "Новый",
            TaskStatuses.AssemblingComponents => "Сбор комплектующих",
            TaskStatuses.Installation => "Монтаж",
            TaskStatuses.Testing => "Испытание",
            TaskStatuses.Closed => "Закрыт",
            _ => throw new NotImplementedException()
        };
    }
    
    /// <summary>
    /// Возвращает строкове представление роли пользователя
    /// </summary>
    public static string UsersRoleToString(this UsersRoles role)
    {
        return role switch
        {
            UsersRoles.Admin => "Администратор",
            UsersRoles.Engineer => "Инженер",
            UsersRoles.Draftsman => "Чертежник",
            UsersRoles.Storage => "Складовщик",
            _ => throw new NotImplementedException()
        };
    }
    
}