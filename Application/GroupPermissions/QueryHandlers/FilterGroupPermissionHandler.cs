using Application.GroupPermissions.Dto;
using Application.GroupPermissions.Queries;
using Application.Permissions.Dto;
using AutoMapper;
using Common.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.GroupPermissions.QueryHandlers
{
    public class FilterGroupPermissionHandler : IRequestHandler<FilterGroupPermission, PaginatedList<GroupPermissionDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FilterGroupPermissionHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<GroupPermissionDto>> Handle(FilterGroupPermission request, CancellationToken cancellationToken)
        {
            var query = _context.GroupPermissions.AsQueryable();
            query = _context.GroupPermissions
                .Include(gp => gp.AssignPermissions)
                    .ThenInclude(ap => ap.Permission)
                .Include(gp => gp.AssignGroups)
                    .ThenInclude(ag => ag.Account);

            return await PaginatedList<GroupPermissionDto>.CreateAsync(
                _mapper.ProjectTo<GroupPermissionDto>(query),
                request.PageNumber,
                request.PageSize
            );
        }
    }
}

