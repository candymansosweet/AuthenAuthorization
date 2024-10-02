using Application.Common.Mapping;
using Application.GroupPermissions.Dto;
using Domain.Entities;
using MediatR;

namespace Application.GroupPermissions.Commands
{
    public class CreateGroupPermissionCommand : IRequest<GroupPermissionDto>, IMapTo<GroupPermission>
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
    }
}