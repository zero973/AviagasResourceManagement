using ARM.Core.Enums;

namespace ARM.WebApi.Models;

/// <summary>
/// Модель для изменения статуса задачи
/// </summary>
public record ChangeTaskStatus(Guid TaskId, TaskStatuses NewStatus);

/// <summary>
/// Модель для смены текущего исполнителя задачи
/// </summary>
public record ChangeTaskPerformer(Guid TaskId, Guid EmployeeId);