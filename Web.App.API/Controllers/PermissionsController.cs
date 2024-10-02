using Application.Permissions.Commands;
using Application.Permissions.Dto;
using Application.Permissions.Queries;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.App.API.Controllers
{
    public class PermissionsController : ApiControllerBase
    {
        [HttpPost()]
        public async Task<ActionResult<PermissionDto>> Create(CreatePermissionCommand registerRequest)
        {
            return await Mediator.Send(registerRequest);
        }
        [HttpGet("filter")]
        public async Task<ActionResult<PaginatedList<PermissionDto>>> GetAll([FromQuery] FilterPermission data)
        {
            return await Mediator.Send(data);
        }
        [HttpPut]
        public async Task<ActionResult<PermissionDto>> Update(UpdatePermissionCommand updateRequest)
        {
            return await Mediator.Send(updateRequest);
        }
        [HttpDelete]
        public async Task<ActionResult<PermissionDto>> Delete([FromQuery] DeletePermissionCommand deletePermissionCommand)
        {
            return await Mediator.Send(deletePermissionCommand);
        }
        [HttpGet("get-by-id")]
        public async Task<ActionResult<PermissionDto>> Get([FromQuery] GetPermissionById getById)
        {
            return await Mediator.Send(getById);
        }
    }
}
