using Application.GroupPermissions.Commands;
using Application.GroupPermissions.Dto;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using System.Security;

namespace Application.GroupPermissions.CommandHandlers
{
    public class CreateGroupPermissionCommandHandler : IRequestHandler<CreateGroupPermissionCommand, GroupPermissionDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateGroupPermissionCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GroupPermissionDto> Handle(CreateGroupPermissionCommand request, CancellationToken cancellationToken)
        {
            if (_context.GroupPermissions.Any(x => x.Title == request.Title))
            {
                throw new AppException(ExceptionCode.Duplicate, $"Đã tồn tại GroupPermission {request.Title}",
                    new[] { new ErrorDetail(nameof(request.Title), request.Title) });
            }
            GroupPermission groupPermission = _mapper.Map<GroupPermission>(request);

            _context.GroupPermissions.Add(groupPermission);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GroupPermissionDto>(groupPermission);
        }
    }
}

