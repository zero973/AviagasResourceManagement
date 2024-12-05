using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using FluentValidation;

namespace ARM.Core.Helpers;

public static class CommandHandlersHelper
{

    /// <summary>
    /// Валидация сущности <paramref name="entity"/> с помощью соответствующего валидатора <paramref name="validator"/> 
    /// и возвратом сообщения с ошибками, если они есть.
    /// </summary>
    public static async Task<Result<T>> Validate<T>(T entity, IValidator<T> validator) where T : class
    {
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            return new Result<T>($"Данные содержат следующие ошибки: \n" 
                                 + string.Join($";\n", errors));
        }
        return new Result<T>();
    }

}