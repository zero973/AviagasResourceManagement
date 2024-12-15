using ARM.Core.Models.Entities;

namespace ARM.Core.Models.Statistics;

/// <summary>
/// Статистика планируемых расходов
/// </summary>
public class PlainExpensesStatistics
{
    
    /// <summary>
    /// Электрощит управления станцией
    /// </summary>
    public Cabinet Cabinet { get; set; }
    
    /// <summary>
    /// Кол-во шкафов
    /// </summary>
    public int Count { get; set; }
    
    /// <summary>
    /// Расход
    /// </summary>
    public decimal Expenses { get; set; }

    public PlainExpensesStatistics()
    {
        
    }

    public PlainExpensesStatistics(Cabinet cabinet, int count, decimal expenses)
    {
        Cabinet = cabinet;
        Count = count;
        Expenses = expenses;
    }
    
}