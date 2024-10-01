using Application.Permissions.Commands;
using Application.Permissions.Dto;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permissions.CommandHandlers
{
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionDto>
    {
        private BanHangContext _context;
        private readonly IMapper _mapper;

        public CreatePermissionCommandHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PermissionDto> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            Permission permission = _mapper.Map<Permission>(request);
            if (_context.Permissions.Any(x => x.Title == request.Title))
            {
                throw new AppException(
                    ExceptionCode.Duplicate, 
                    "Đã tồn tại Permission " + request.Title, 
                    new[] { 
                        new ErrorDetail(
                            nameof(request.Title), 
                            request.Title)
                    }
                );
            }
            permission.AssignPermissions = request.GroupPermissionIds.Select(t => new AssignPermission()
            {
                GroupPermissionId = t
            }).ToList();

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<PermissionDto>(permission);
        }
    }
}
