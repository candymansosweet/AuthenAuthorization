using Application.Permissions.Dto;
using Application.Permissions.Queries;
using AutoMapper;
using Common.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Permissions.QueryHandlers
{
    public class GetPermissionByIdHandler : IRequestHandler<GetPermissionById, PermissionDto>
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public GetPermissionByIdHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PermissionDto> Handle(GetPermissionById request, CancellationToken cancellationToken)
        {
            var permission = await _context.Permissions
                .Include(p => p.AssignPermissions)
                    .ThenInclude(ap => ap.GroupPermission)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (permission == null)
            {
                throw new AppException(ExceptionCode.Notfound, "Không tìm thấy Permission");
            }

            return _mapper.Map<PermissionDto>(permission);
        }
    }
}
