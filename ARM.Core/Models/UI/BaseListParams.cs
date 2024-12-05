namespace ARM.Core.Models.UI;

/// <summary>
/// Базовые параметры для операции получения данных
/// </summary>
public class BaseListParams
{

    /// <summary>
    /// Номер страницы
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Кол-во строк в странице
    /// </summary>
    public int RowsCount { get; set; } = 1;

    /// <summary>
    /// Параметры сортировки. Ключ - название поля; Значение - метод сортировки (IsAsc) (true - asc / false - desc)
    /// </summary>
    public IDictionary<string, bool>? OrderBy { get; set; }

    /// <summary>
    /// Параметры фильтрации
    /// </summary>
    public IList<ComplexFilter>? Filters { get; set; }

    /// <summary>
    /// Получить значение фильтра по <paramref name="fieldName"/>
    /// </summary>
    /// <param name="fieldName">Название поля</param>
    public object? GetFilterValue(string fieldName)
    {
        fieldName = fieldName.ToLower();
        if (Filters == null || !Filters.Any(x => x.Field.ToUpper() == fieldName.ToUpper()))
            return null;
        return Filters.First(x => x.Field.ToUpper() == fieldName.ToUpper()).Value;
    }

    public override string ToString()
    {
        return $"PageNumber: {PageNumber} RowsCount: {RowsCount} OrderBy: {OrderBy?.Count ?? 0} Filters: {Filters?.Count ?? 0}";
    }

}