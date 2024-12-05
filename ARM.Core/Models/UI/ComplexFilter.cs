using ARM.Core.Enums;

namespace ARM.Core.Models.UI;

/// <summary>
/// Фильтр данных
/// </summary>
public class ComplexFilter
{

    /// <summary>
    /// Анализируемое поле
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Операция
    /// </summary>
    public ComplexFilterOperators Operator { get; set; }

    /// <summary>
    /// Сравниваемое значение
    /// </summary>
    public object Value { get; set; }

    public ComplexFilter()
    {

    }

    public ComplexFilter(string field, ComplexFilterOperators operation, object value)
    {
        Field = field;
        Operator = operation;
        Value = value;
    }

    public override string ToString()
    {
        return $"Field: {Field}, Operator: {Operator}, Value: {Value}";
    }

}