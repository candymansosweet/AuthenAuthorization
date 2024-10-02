using Application.Permissions.Dto;
using Application.Permissions.Queries;
using AutoMapper;
using Common.Models;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Permissions.QueryHandlers
{
    public class FilterPermissionHandler : IRequestHandler<FilterPermission, PaginatedList<PermissionDto>>
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public FilterPermissionHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<PermissionDto>> Handle(FilterPermission request, CancellationToken cancellationToken)
        {
            var query = _context.Permissions.AsQueryable();
            query = _context.Permissions
                .Include(p => p.AssignPermissions)
                    .ThenInclude(ap => ap.GroupPermission).AsQueryable();

            var mappedQuery = _mapper.ProjectTo<PermissionDto>(query);

            return await PaginatedList<PermissionDto>.CreateAsync(
                mappedQuery,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}
