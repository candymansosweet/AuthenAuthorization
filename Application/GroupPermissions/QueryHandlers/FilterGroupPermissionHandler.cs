using Application.GroupPermissions.Dto;
using Application.GroupPermissions.Queries;
using AutoMapper;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.GroupPermissions.QueryHandlers
{
    public class FilterGroupPermissionHandler : IRequestHandler<FilterGroupPermission, List<GroupPermissionDto>>
    {
        private readonly BanHangContext _context;
        private readonly IMapper _mapper;

        public FilterGroupPermissionHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GroupPermissionDto>> Handle(FilterGroupPermission request, CancellationToken cancellationToken)
        {
            var groupPermissions = await _context.GroupPermissions
                .Include(gp => gp.AssignPermissions)
                    .ThenInclude(ap => ap.Permission)
                .Include(gp => gp.AssignGroups)
                    .ThenInclude(ag => ag.Account)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<GroupPermissionDto>>(groupPermissions);
        }
    }
}

