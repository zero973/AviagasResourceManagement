using ARM.Core.Commands.Requests.Entities;
using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Models.Entities;
using ARM.Core.Models.Statistics;
using ARM.WebApi.Controllers.Entities.Base;
using FluentResults.Extensions.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ARM.WebApi.Controllers.Entities;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SystemTasksController : BaseActualEntityController<SystemTask>
{
    
    public SystemTasksController(ISender sender) : base(sender)
    {
        
    }
    
    /// <summary>
    /// Меняет статус задачи и автоматически ставит нужного исполнителя в зависимости от новго статуса.
    /// </summary>
    /// <returns>Возращает изменённый объект типа <see cref="SystemTask"/></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<SystemTask>> ChangeTaskStatus([FromBody] ChangeTaskStatus data)
    {
        return await _sender.Send(new ChangeTaskStatus(data.TaskId, data.NewStatus)).ToActionResult();
    }
    
    /// <summary>
    /// Меняет текущего исполнителя задачи
    /// </summary>
    /// <returns>Возращает изменённый объект типа <see cref="SystemTask"/></returns>
    [HttpPut]
    [Route("[action]")]
    public async Task<ActionResult<SystemTask>> ChangeTaskPerformer([FromBody] ChangeTaskPerformer data)
    {
        return await _sender.Send(data).ToActionResult();
    }
    
    /// <summary>
    /// Добавить сотрудника к задаче
    /// </summary>
    /// <remarks>Добавляет сотрудника в список <see cref="SystemTask.Employees"/>.</remarks>
    /// <returns>Возращает добавленный объект типа <see cref="TaskEmployee"/></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<TaskEmployee>> AddLinkedEmployee([FromBody] TaskEmployee employee)
    {
        return await _sender.Send(new AddDataRequest<TaskEmployee>(employee)).ToActionResult();
    }
    
    /// <summary>
    /// Удалить сотрудника из задачи
    /// </summary>
    /// <remarks>Удаляет сотрудника из списка <see cref="SystemTask.Employees"/>.</remarks>
    /// <returns>Возращает удалённый объект типа <see cref="TaskEmployee"/></returns>
    [HttpDelete]
    [Route("[action]")]
    public async Task<ActionResult<TaskEmployee>> RemoveLinkedEmployee([FromBody] Guid employeeId)
    {
        return await _sender.Send(new DeleteDataRequest<TaskEmployee>(employeeId)).ToActionResult();
    }
    
    /// <summary>
    /// Получение статистики по затратам на задачи в заданном месяце <paramref name="month"/>.
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<List<TasksStatistics>>> GetTasksStatistics([FromQuery] DateTime month)
    {
        return await _sender.Send(new GetTasksStatistics(month)).ToActionResult();
    }
    
}