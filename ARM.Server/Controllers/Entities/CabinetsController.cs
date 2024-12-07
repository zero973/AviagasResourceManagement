using ARM.Core.Models.Entities;
using ARM.WebApi.Controllers.Entities.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ARM.WebApi.Controllers.Entities;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CabinetsController : BaseEntityController<Cabinet>
{
    public CabinetsController(ISender sender) : base(sender)
    {
        
    }
}