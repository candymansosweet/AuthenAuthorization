using Application.Permissions.Dto;
using Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permissions.Queries
{
    public class FilterPermission : BasePaginatedQuery, IRequest<PaginatedList<PermissionDto>>
    {
        public string? Keyword { get; set; }
    }
}
