using ARM.Core.Models.Statistics;
using ARM.Core.Models.UI;

namespace ARM.Core.Repositories;

/// <summary>
/// Репозиторий для получения статистики
/// </summary>
public interface IStatisticsRepository
{

    /// <summary>
    /// Получение статистики по затратам на задачи в заданном месяце <paramref name="month"/>.
    /// Возвращает только те задачи, где есть списание времени в месяце (т.е. по ним велись работы в заданном месяце).
    /// </summary>
    /// <param name="month">Месяц выгрузки</param>
    /// <returns>Статистика за месяц</returns>
    Task<Result<List<TasksStatistics>>> GetTasksStatisticsForMonth(DateTime month);

}