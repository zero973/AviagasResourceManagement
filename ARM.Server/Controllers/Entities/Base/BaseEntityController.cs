using ARM.Core.Commands.Requests.Entities;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ARM.WebApi.Controllers.Entities.Base;

/// <summary>
/// Базовый контроллер для сущностей <see cref="IEntity"/>.
/// </summary>
public abstract class BaseEntityController<T> : ControllerBase
    where T : class, IEntity
{

    protected readonly ISender _sender;

    public BaseEntityController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    [Route("[action]")]
    public virtual async Task<JsonResult> Get(Guid id)
    {
        return new JsonResult(await _sender.Send(new GetDataRequest<T>(id)));
    }

    [HttpGet]
    [Route("[action]")]
    public virtual async Task<JsonResult> GetAll([FromQuery] BaseListParams baseParams)
    {
        return new JsonResult(await _sender.Send(new GetAllDataRequest<T>(baseParams)));
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<JsonResult> Add([FromBody] T entity)
    {
        return new JsonResult(await _sender.Send(new AddDataRequest<T>(entity)));
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<JsonResult> Update([FromBody] T entity)
    {
        return new JsonResult(await _sender.Send(new EditDataRequest<T>(entity)));
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<JsonResult> Delete([FromBody] T entity)
    {
        return new JsonResult(await _sender.Send(new DeleteDataRequest<T>(entity)));
    }
    
}