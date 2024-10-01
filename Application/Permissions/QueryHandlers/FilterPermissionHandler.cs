using Application.Permissions.Dto;
using Application.Permissions.Queries;
using AutoMapper;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Permissions.QueryHandlers
{
    public class FilterPermissionHandler : IRequestHandler<FilterPermission, List<PermissionDto>>
    {
        private BanHangContext _context;
        private readonly IMapper _mapper;

        public FilterPermissionHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<PermissionDto>> Handle(FilterPermission request, CancellationToken cancellationToken)
        {
            var permissions = await _context.Permissions
                .Include(p => p.AssignPermissions)
                    .ThenInclude(ap => ap.GroupPermission)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<PermissionDto>>(permissions);
        }
    }
}
