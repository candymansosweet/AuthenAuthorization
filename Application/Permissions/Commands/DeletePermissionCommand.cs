using Application.Common.Mapping;
using Application.Permissions.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permissions.Commands
{
    public class DeletePermissionCommand : IRequest<PermissionDto>
    {
        public int? Id { get; set; }
        public int? Code { get; set; }
    }
}
