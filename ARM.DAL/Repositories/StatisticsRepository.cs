using ARM.Core.Extensions;
using ARM.Core.Models.Statistics;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using ARM.DAL.ApplicationContexts;
using ARM.DAL.Models.Entities;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<StatisticsRepository> _logger;
    
    public StatisticsRepository(AppDbContext context, IMapper mapper, ILogger<StatisticsRepository> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<Result<List<TasksStatistics>>> GetTasksStatisticsForMonth(DateTime month)
    {
        var start = month.WithFirstDayMonth();
        var end = start.WithLastDayMonth();
        try
        {
            var (connection, transaction) = _context.GetConnection();
            var sql = $@"
                SELECT t.""{nameof(SystemTask.Id)}"",
                    t.""{nameof(SystemTask.Name)}"",
                    t.""{nameof(SystemTask.Status)}"",
                    t.""{nameof(SystemTask.EstimatedWorkHours)}"",
                    SUM(wt.""{nameof(WorkedTime.Hours)}"") AS ""{nameof(TasksStatistics.RealWorkedHours)}"",
                    SUM(sl.""{nameof(EmployeeSalary.SalaryForOneHour)}"") AS ""{nameof(TasksStatistics.TotalSalary)}"",
                    SUM(pc.""{nameof(CabinetPartCounts.Count)}"") AS ""{nameof(TasksStatistics.TotalCabinetPartCount)}"",
                    SUM(cp.""{nameof(CabinetPart.Cost)}"" * pc.""{nameof(CabinetPartCounts.Count)}"") AS ""{nameof(TasksStatistics.TotalCabinetPartsCost)}"",
                    SUM(sl.""{nameof(EmployeeSalary.SalaryForOneHour)}"") + SUM(cp.""{nameof(CabinetPart.Cost)}"" * pc.""{nameof(CabinetPartCounts.Count)}"") AS ""{nameof(TasksStatistics.TotalCost)}""
                FROM ""{nameof(_context.Taks)}"" t
                    JOIN ""{nameof(_context.WorkedTimes)}"" wt ON wt.""{nameof(WorkedTime.TaskId)}"" = t.""Id"" 
                        AND wt.""{nameof(WorkedTime.IsActual)}"" = true
                        -- берём только те задачи, где есть списание времени в месяце
                        AND wt.""{nameof(WorkedTime.Date)}"" BETWEEN @{nameof(start)} AND @{nameof(end)}
                    JOIN ""{nameof(_context.EmployeeSalaries)}"" sl ON sl.""{nameof(EmployeeSalary.EmployeeId)}"" = wt.""{nameof(WorkedTime.EmployeeId)}""
                        AND sl.""{nameof(EmployeeSalary.IsActual)}""
                        AND (sl.""{nameof(EmployeeSalary.Start)}"", sl.""{nameof(EmployeeSalary.End)}"") OVERLAPS (@{nameof(start)}, @{nameof(end)})
                    JOIN ""{nameof(_context.CabinetPartCounts)}"" pc ON pc.""{nameof(CabinetPartCounts.TaskId)}"" = t.""Id""
                        AND pc.""{nameof(CabinetPartCounts.IsActual)}""
                    JOIN ""{nameof(_context.CabinetParts)}"" cp ON pc.""Id"" = pc.""{nameof(CabinetPartCounts.CabinetPartId)}""
                WHERE t.""{nameof(SystemTask.IsActual)}"" = true
                GROUP BY t.""{nameof(SystemTask.Id)}"", 
                    t.""{nameof(SystemTask.Name)}"",
                    t.""{nameof(SystemTask.Status)}"",
                    t.""{nameof(SystemTask.EstimatedWorkHours)}""
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