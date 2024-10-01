using Application.GroupPermissions.Dto;
using MediatR;

namespace Application.GroupPermissions.Commands
{
    public class DeleteGroupPermissionCommand : IRequest<GroupPermissionDto>
    {
        public int Id { get; set; }
    }
}

