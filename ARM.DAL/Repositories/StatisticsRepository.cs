using ARM.Core.Extensions;
using ARM.Core.Models.Statistics;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using ARM.DAL.ApplicationContexts;
using ARM.DAL.Models.Entities;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ARM.DAL.Extensions;

namespace ARM.DAL.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    
    private readonly AppDbContext _context;
    private readonly ILogger<StatisticsRepository> _logger;
    
    public StatisticsRepository(AppDbContext context, ILogger<StatisticsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Result<List<TasksStatistics>>> GetTasksStatisticsForMonth(DateTime month)
    {
        var start = month.WithFirstDayMonth();
        var end = start.WithLastDayMonth();
        try
        {
            var tasks = _context.Model.FindEntityType(typeof(SystemTask))!.GetSchemaTableName();
            var workedTimes = _context.Model.FindEntityType(typeof(WorkedTime))!.GetSchemaTableName();
            var employeeSalaries = _context.Model.FindEntityType(typeof(EmployeeSalary))!.GetSchemaTableName();
            var cabinetPartCounts = _context.Model.FindEntityType(typeof(CabinetPartCounts))!.GetSchemaTableName();
            var cabinetParts = _context.Model.FindEntityType(typeof(CabinetPart))!.GetSchemaTableName();
            
            var (connection, transaction) = _context.GetConnection();
            
            var sql = $@"
                WITH worked_times AS (
                    SELECT wt.""{nameof(WorkedTime.TaskId)}"",
		                SUM(wt.""{nameof(WorkedTime.Hours)}"") AS ""{nameof(TasksStatistics.RealWorkedHours)}"",
                        -- умножаем з/п за 1 час на работы на кол-во часов работы и на 1.5, если это переработка
                        SUM(sl.""{nameof(EmployeeSalary.SalaryForOneHour)}""
                            * wt.""{nameof(WorkedTime.Hours)}""
                            * CASE WHEN wt.""{nameof(WorkedTime.IsOverwork)}"" = true THEN 1.5 ELSE 1 END) AS ""{nameof(TasksStatistics.TotalSalary)}""
	                FROM {workedTimes} wt
		                JOIN {employeeSalaries} sl ON sl.""{nameof(EmployeeSalary.EmployeeId)}"" = wt.""{nameof(WorkedTime.EmployeeId)}""
                            AND sl.""{nameof(EmployeeSalary.IsActual)}""
                            AND (sl.""{nameof(EmployeeSalary.Start)}"", sl.""{nameof(EmployeeSalary.End)}"") OVERLAPS (@{nameof(start)}, @{nameof(end)})
	                WHERE wt.""{nameof(WorkedTime.IsActual)}"" = true
                        AND wt.""{nameof(WorkedTime.Date)}"" BETWEEN @{nameof(start)} AND @{nameof(end)}
	                GROUP BY wt.""{nameof(WorkedTime.TaskId)}""
                ),
                parts_counts AS (
                    SELECT pc.""{nameof(CabinetPartCounts.TaskId)}"",
                        SUM(pc.""{nameof(CabinetPartCounts.Count)}"") AS ""{nameof(TasksStatistics.TotalCabinetPartCount)}"",
                        SUM(cp.""{nameof(CabinetPart.Cost)}"" * pc.""{nameof(CabinetPartCounts.Count)}"") AS ""{nameof(TasksStatistics.TotalCabinetPartsCost)}""
                    FROM {cabinetPartCounts} pc
                        JOIN {cabinetParts} cp ON cp.""Id"" = pc.""{nameof(CabinetPartCounts.CabinetPartId)}""
                    WHERE pc.""{nameof(CabinetPartCounts.IsActual)}""
                        -- в выгрузку добавляем детали только за тот месяц, когда их добавили или изменили
                        AND (pc.""{nameof(CabinetPartCounts.CreateDate)}"" BETWEEN @{nameof(start)} AND @{nameof(end)}
                            OR pc.""{nameof(CabinetPartCounts.UpdateDate)}"" BETWEEN @{nameof(start)} AND @{nameof(end)})
                    GROUP BY pc.""{nameof(CabinetPartCounts.TaskId)}""
                )
                SELECT t.""{nameof(SystemTask.Id)}"" AS ""{nameof(TasksStatistics.TaskId)}"",
                    t.""{nameof(SystemTask.Name)}"" AS ""{nameof(TasksStatistics.TaskName)}"",
                    t.""{nameof(SystemTask.Status)}"",
                    t.""{nameof(SystemTask.EstimatedWorkHours)}"",
                    COALESCE(wt.""{nameof(TasksStatistics.RealWorkedHours)}"", 0) AS ""{nameof(TasksStatistics.RealWorkedHours)}"",
                    COALESCE(wt.""{nameof(TasksStatistics.TotalSalary)}"", 0) AS ""{nameof(TasksStatistics.TotalSalary)}"",
                    COALESCE(pc.""{nameof(TasksStatistics.TotalCabinetPartCount)}"", 0) AS ""{nameof(TasksStatistics.TotalCabinetPartCount)}"",
                    COALESCE(pc.""{nameof(TasksStatistics.TotalCabinetPartsCost)}"", 0) AS ""{nameof(TasksStatistics.TotalCabinetPartsCost)}"",
                    COALESCE(wt.""{nameof(TasksStatistics.TotalSalary)}"", 0) + COALESCE(pc.""{nameof(TasksStatistics.TotalCabinetPartsCost)}"", 0) AS ""{nameof(TasksStatistics.TotalCost)}""
                FROM {tasks} t
                    LEFT JOIN worked_times wt ON wt.""{nameof(WorkedTime.TaskId)}"" = t.""Id""
                    LEFT JOIN parts_counts pc ON pc.""{nameof(CabinetPartCounts.TaskId)}"" = t.""Id""
                WHERE t.""{nameof(SystemTask.IsActual)}"" = true
                    AND COALESCE(t.""{nameof(SystemTask.FinishDate)}"", 'infinity'::timestamp) >= @{nameof(start)}
                ORDER BY t.""{nameof(SystemTask.Status)}"", t.""{nameof(SystemTask.Name)}""";
            var result = (await connection.QueryAsync<TasksStatistics>(sql, new { start, end }, transaction))
                .AsList();
            return new Result<List<TasksStatistics>>(true, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении объекта по Id");
            return new Result<List<TasksStatistics>>("Произошла ошибка при попытке сформировать статистику");
        }
    }
    
}