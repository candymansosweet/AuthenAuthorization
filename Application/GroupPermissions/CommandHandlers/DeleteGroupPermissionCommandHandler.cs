using Application.GroupPermissions.Commands;
using Application.GroupPermissions.Dto;
using AutoMapper;
using Common.Exceptions;
using Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GroupPermissions.CommandHandlers
{
    public class DeleteGroupPermissionCommandHandler : IRequestHandler<DeleteGroupPermissionCommand, GroupPermissionDto>
    {
        private readonly BanHangContext _context;
        private readonly IMapper _mapper;

        public DeleteGroupPermissionCommandHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GroupPermissionDto> Handle(DeleteGroupPermissionCommand request, CancellationToken cancellationToken)
        {
            var groupPermission = await _context.GroupPermissions.FindAsync(request.Id);
            if (groupPermission == null)
            {
                throw new AppException(ExceptionCode.Notfound, "Không tìm thấy GroupPermission");
            }

            _context.GroupPermissions.Remove(groupPermission);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GroupPermissionDto>(groupPermission);
        }
    }
}
