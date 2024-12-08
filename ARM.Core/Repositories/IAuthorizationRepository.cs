using ARM.Core.Models.Entities;
using ARM.Core.Models.UI;

namespace ARM.Core.Repositories;

public interface IAuthorizationRepository
{

    /// <summary>
    /// Получить пользователя по логину и паролю
    /// </summary>
    Task<Result<EmployeeAccount>> GetUserByLoginAndPassword(string login, string passwordHash);

}