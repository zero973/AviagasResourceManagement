using ARM.Core.Commands.Requests.Entities;
using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Models.Entities;
using ARM.WebApi.Controllers.Entities.Base;
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
    public async Task<JsonResult> ChangeTaskStatus([FromBody] ChangeTaskStatus data)
    {
        return new JsonResult(await _sender.Send(new ChangeTaskStatus(data.TaskId, data.NewStatus)));
    }
    
    /// <summary>
    /// Меняет текущего исполнителя задачи
    /// </summary>
    /// <returns>Возращает изменённый объект типа <see cref="SystemTask"/></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<JsonResult> ChangeTaskPerformer([FromBody] ChangeTaskPerformer data)
    {
        return new JsonResult(await _sender.Send(new ChangeTaskPerformer(data.TaskId, data.EmployeeId)));
    }
    
    /// <summary>
    /// Добавить сотрудника к задаче
    /// </summary>
    /// <remarks>Добавляет сотрудника в список <see cref="SystemTask.Employees"/>.</remarks>
    /// <returns>Возращает добавленный объект типа <see cref="TaskEmployee"/></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<JsonResult> AddLinkedEmployee([FromBody] TaskEmployee employee)
    {
        return new JsonResult(await _sender.Send(new AddDataRequest<TaskEmployee>(employee)));
    }
    
    /// <summary>
    /// Удалить сотрудника из задачи
    /// </summary>
    /// <remarks>Удаляет сотрудника из списка <see cref="SystemTask.Employees"/>.</remarks>
    /// <returns>Возращает удалённый объект типа <see cref="TaskEmployee"/></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<JsonResult> RemoveLinkedEmployee([FromBody] TaskEmployee employee)
    {
        return new JsonResult(await _sender.Send(new DeleteDataRequest<TaskEmployee>(employee)));
    }
    
    /// <summary>
    /// Получение статистики по затратам на задачи в заданном месяце <paramref name="month"/>.
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("[action]")]
    public async Task<JsonResult> GetTasksStatistics([FromQuery] DateTime month)
    {
        return new JsonResult(await _sender.Send(new GetTasksStatistics(month)));
    }
    
}