using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BeautyMap.API.Settings
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator mediator;

        public ApiControllerBase(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
