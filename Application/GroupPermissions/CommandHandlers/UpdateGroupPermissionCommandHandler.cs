using Application.GroupPermissions.Commands;
using Application.GroupPermissions.Dto;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Application.GroupPermissions.CommandHandlers
{
    public class UpdateGroupPermissionCommandHandler : IRequestHandler<UpdateGroupPermissionCommand, GroupPermissionDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UpdateGroupPermissionCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GroupPermissionDto> Handle(UpdateGroupPermissionCommand request, CancellationToken cancellationToken)
        {
            var groupPermission = await _context.GroupPermissions
                .Include(gp => gp.PermissionGroupPermissions)
                .Include(gp => gp.AccountGroupPermissions)
                .FirstOrDefaultAsync(gp => gp.Id == request.Id);

            if (groupPermission == null)
            {
                throw new AppException(ExceptionCode.Notfound, $"Không tìm thấy GroupPermission {request.Id}",
                    new[] { new ErrorDetail(nameof(request.Id), request.Id) });
            }
            groupPermission = _mapper.Map<GroupPermission>(request);
            if (request.PermissionIds?.Count > 0)
            {
                groupPermission.PermissionGroupPermissions = request.PermissionIds.Select(id => new PermissionGroupPermission
                {
                    GroupPermissionId = request.Id,
                    PermissionId = id
                }).ToList();

            }
            if (request.AccountIds?.Count > 0)
            {
                groupPermission.AccountGroupPermissions = request.AccountIds.Select(id => new AccountGroupPermission
                {
                    GroupPermissionId = request.Id,
                    AccountId = id
                }).ToList();
            }

            _context.GroupPermissions.Update(groupPermission);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GroupPermissionDto>(groupPermission);
        }
    }
}
