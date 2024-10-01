using Application.GroupPermissions.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GroupPermissions.Commands
{
    public class CreateGroupPermissionCommand : IRequest<GroupPermissionDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();
        public List<int> AccountIds { get; set; } = new List<int>();
    }
}
