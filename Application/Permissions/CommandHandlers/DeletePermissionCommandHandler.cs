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
    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand,PermissionDto>
    {
        private BanHangContext _context;
        private readonly IMapper _mapper;

        public DeletePermissionCommandHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PermissionDto> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            Permission permission = new Permission();
            if(request.Id != null)
            {
                permission = await _context.Permissions.FindAsync(request.Id);
            }
            else if(request.Code != null) {
                permission = await _context.Permissions.FindAsync(request.Code);
            }
            if (permission == null)
            {
                throw new AppException(ExceptionCode.Notfound, "Không tìm thấy Permission");
            }
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PermissionDto>(permission);
        }
    }
}
