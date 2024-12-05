using ARM.Core.Enums;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;

namespace ARM.Core.Extensions;

/// <summary>
/// Extension-методы для <see cref="BaseListParams"/>
/// </summary>
public static class BaseListParamsExtensions
{

    /// <summary>
    /// Применить и пагинацию
    /// </summary>
    public static BaseListParams WithPagination(this BaseListParams prms, int rowsCount = 100, int pageNumber = 1)
    {
        prms.RowsCount = rowsCount;
        prms.PageNumber = pageNumber;
        return prms;
    }

    /// <summary>
    /// Применить фильтр IsActual
    /// </summary>
    public static BaseListParams WithActualFilter(this BaseListParams prms) 
        => prms.WithFilter(nameof(IActualEntity.IsActual), ComplexFilterOperators.Equals, true);

    /// <summary>
    /// Применить фильтр IsActual и пагинацию
    /// </summary>
    public static BaseListParams WithActualPagination(this BaseListParams prms, int rowsCount = 100, int pageNumber = 1)
        => prms.WithActualFilter().WithPagination(rowsCount, pageNumber);

    /// <summary>
    /// Применить фильтр
    /// </summary>
    public static BaseListParams WithFilter(this BaseListParams prms, string field, ComplexFilterOperators operation, object value)
    {
        prms.SetFiltersListNotNull().Filters!.Add(new ComplexFilter(field, operation, value));
        return prms;
    }

    private static BaseListParams SetFiltersListNotNull(this BaseListParams prms)
    {
        prms.Filters ??= new List<ComplexFilter>();
        return prms;
    }

}