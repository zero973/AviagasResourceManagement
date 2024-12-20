using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
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
    public virtual async Task<JsonResult> Get(Guid id)
    {
        return new JsonResult(await _sender.Send(new GetActualDataRequest<T>(id)));
    }

    [HttpGet]
    [Route("[action]")]
    public virtual async Task<JsonResult> GetAll([FromQuery] BaseListParams baseParams)
    {
        return new JsonResult(await _sender.Send(new GetActualAllDataRequest<T>(baseParams)));
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<JsonResult> Add([FromBody] T entity)
    {
        return new JsonResult(await _sender.Send(new AddActualDataRequest<T>(entity)));
    }

    [HttpPut]
    [Route("[action]")]
    public virtual async Task<JsonResult> Update([FromBody] T entity)
    {
        return new JsonResult(await _sender.Send(new EditActualDataRequest<T>(entity)));
    }

    [HttpDelete]
    [Route("[action]")]
    public virtual async Task<JsonResult> Delete([FromQuery] Guid id)
    {
        return new JsonResult(await _sender.Send(new DeleteActualDataRequest<T>(id)));
    }
    
}