using System.Text.Json;
using ARM.Core.Enums;
using ARM.Core.Models.UI;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ARM.WebApi.Infrastructure.ModelBinders;

/// <summary>
/// Парсер для <see cref="BaseListParams"/>
/// </summary>
public class BaseListParamsModelBinder : IModelBinder
{

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var queryPageNumber = bindingContext.ValueProvider.GetValue("PageNumber").FirstValue;
        var queryRowsCount = bindingContext.ValueProvider.GetValue("RowsCount").FirstValue;
        var queryOrderBy = (bindingContext.ValueProvider as IEnumerableValueProvider)!.GetKeysFromPrefix("OrderBy");
        var queryFilters = bindingContext.ValueProvider.GetValue("Filters").FirstValue;

        var pageNumber = Convert.ToInt32(queryPageNumber);
        var rowsCount = Convert.ToInt32(queryRowsCount);

        var orderBy = new Dictionary<string, bool>();
        if (queryOrderBy.Any())
            orderBy = queryOrderBy.ToDictionary(x => x.Key, x => Convert.ToBoolean(bindingContext.ValueProvider.GetValue(x.Value).FirstValue));

        var filters = new List<ComplexFilter>();
        if (queryFilters?.Length > 2)
            filters = JsonSerializer.Deserialize<List<ComplexFilter>>(queryFilters);

        // если передан массив, то преобразовываем его в соответствующий тип данных
        foreach (var filter in filters!)
        {
            if (filter.Operator != ComplexFilterOperators.In)
                continue;

            var value = (JsonElement)filter.Value;
            if (value.ValueKind != JsonValueKind.Array)
                throw new FormatException("Для оператора In передан неправильный тип данных.");
            
            filter.Value = ParseJsonElementToObjectArray(value);
        }

        var result = new BaseListParams() 
        {
            PageNumber = pageNumber, 
            RowsCount = rowsCount,
            OrderBy = orderBy,
            Filters = filters
        };

        bindingContext.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }
    
    private object[] ParseJsonElementToObjectArray(JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.Array)
        {
            return value.EnumerateArray()
                .Select(element => 
                {
                    // Если элемент - строка и её можно преобразовать в Guid, делаем это
                    if (element.ValueKind == JsonValueKind.String)
                    {
                        if (Guid.TryParse(element.GetString(), out var guid))
                        {
                            return (object)guid;
                        }
                    }

                    // В других случаях возвращаем как есть (например, для чисел, строк и проч.)
                    switch (element.ValueKind)
                    {
                        case JsonValueKind.Number:
                            return element.GetInt32();
                        case JsonValueKind.String:
                            return element.GetString();
                        case JsonValueKind.True:
                        case JsonValueKind.False:
                            return element.GetBoolean();
                        default:
                            throw new InvalidOperationException("Unsupported JSON element type.");
                    }
                })
                .ToArray()!;
        }

        throw new InvalidOperationException("Expected JsonElement to be an array.");
    }

}