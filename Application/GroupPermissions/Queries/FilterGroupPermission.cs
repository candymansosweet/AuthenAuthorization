using Application.GroupPermissions.Dto;
using Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GroupPermissions.Queries
{
    public class FilterGroupPermission : BasePaginatedQuery, IRequest<PaginatedList<GroupPermissionDto>>
    {
    }
}

