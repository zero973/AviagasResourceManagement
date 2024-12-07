using ARM.Core.Enums;
using ARM.Core.Models.Entities;
using ARM.Core.Models.Statistics;
using ARM.Core.Models.UI;
using MediatR;

namespace ARM.Core.Commands.Requests.Entities.Concrete;

/// <summary>
/// Команда для смены текущего статуса задачи
/// и автоматическую смену нужного исполнителя в зависимости от новго статуса.
/// </summary>
public record ChangeTaskStatus(Guid TaskId, TaskStatuses NewStatus) : IRequest<Result<SystemTask>>;

/// <summary>
/// Команда для смены текущего исполнителя задачи
/// </summary>
public record ChangeTaskPerformer(Guid TaskId, Guid EmployeeId) : IRequest<Result<SystemTask>>;

/// <summary>
/// Команда для получения статистики по затратам на задачи в заданном месяце
/// </summary>
/// <param name="Month">Месяц, за который выгружаются данные</param>
public record GetTasksStatistics(DateTime Month) : IRequest<Result<List<TasksStatistics>>>;