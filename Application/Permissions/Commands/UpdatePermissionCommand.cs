using Application.Common.Mapping;
using Application.Permissions.Dto;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permissions.Commands
{
    public class UpdatePermissionCommand : IRequest<PermissionDto>, IBasicMapTo<Permission>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public List<int> GroupPermissionIds { get; set; } = new List<int>();
    }
}
