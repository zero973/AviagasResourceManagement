using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using FluentResults.Extensions.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ARM.WebApi.Controllers.Entities.Base;

/// <summary>
/// Базовый контроллер для сущностей <see cref="IActualEntity"/>.
/// </summary>
public class BaseActualEntityController<T> : ControllerBase
    where T : class, IActualEntity
{

    protected readonly ISender _sender;

    public BaseActualEntityController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    [Route("[action]")]
    public virtual async Task<ActionResult<T>> Get(Guid id)
    {
        return await _sender.Send(new GetActualDataRequest<T>(id)).ToActionResult();
    }

    [HttpGet]
    [Route("[action]")]
    public virtual async Task<ActionResult<List<T>>> GetAll([FromQuery] BaseListParams baseParams)
    {
        return await _sender.Send(new GetActualAllDataRequest<T>(baseParams)).ToActionResult();
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<ActionResult<T>> Add([FromBody] T entity)
    {
        return await _sender.Send(new AddActualDataRequest<T>(entity)).ToActionResult();
    }

    [HttpPut]
    [Route("[action]")]
    public virtual async Task<ActionResult<T>> Update([FromBody] T entity)
    {
        return await _sender.Send(new EditActualDataRequest<T>(entity)).ToActionResult();
    }

    [HttpDelete]
    [Route("[action]")]
    public virtual async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        return await _sender.Send(new DeleteActualDataRequest<T>(id)).ToActionResult();
    }
    
}