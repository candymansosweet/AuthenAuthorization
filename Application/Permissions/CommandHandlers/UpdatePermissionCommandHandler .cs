using Application.Permissions.Commands;
using Application.Permissions.Dto;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Permissions.CommandHandlers
{
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, PermissionDto>
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePermissionCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PermissionDto> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            Permission? permission = await _context.Permissions
                .Include(p => p.AssignPermissions)
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (permission == null)
            {
                throw new AppException(ExceptionCode.Notfound, $"Không tìm thấy Permission {request.Title}",
                    new[] { new ErrorDetail(nameof(request.Title), request.Title) });
            }
            permission = _mapper.Map<Permission>(request);
            if(request.GroupPermissionIds?.Count > 0)
            {
                permission.AssignPermissions = request.GroupPermissionIds.Select(item => new AssignPermission()
                {
                    PermissionId = request.Id,
                    GroupPermissionId = item
                }).ToList();
            }
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PermissionDto>(permission);
        }
    }
}
