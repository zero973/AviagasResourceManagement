namespace ARM.Core.Enums;

/// <summary>
/// Операции фильтрации
/// </summary>
public enum ComplexFilterOperators
{
    All = 0,
    Equals = 1,
    NotEquals = 2,
    GreaterThan = 3,
    LessThan = 4,
    GreaterThanOrEqual = 5,
    LessThanOrEqual = 6,
    Contains = 7,
    StartsWith = 8,
    EndsWith = 9,
    
    /// <summary>
    /// Значение поля находится внутри какого-то массива.
    /// </summary>
    /// <remarks>Операция только для массивов</remarks>
    In = 10
}