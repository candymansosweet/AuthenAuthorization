using Application.GroupPermissions.Dto;
using Application.GroupPermissions.Queries;
using AutoMapper;
using Common.Exceptions;
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
    public class GetGroupPermissionByIdHandler : IRequestHandler<GetGroupPermissionById, GroupPermissionDto>
    {
        private readonly BanHangContext _context;
        private readonly IMapper _mapper;

        public GetGroupPermissionByIdHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GroupPermissionDto> Handle(GetGroupPermissionById request, CancellationToken cancellationToken)
        {
            var groupPermission = await _context.GroupPermissions
                .Include(gp => gp.AssignPermissions)
                    .ThenInclude(ap => ap.Permission)
                .Include(gp => gp.AssignGroups)
                    .ThenInclude(ag => ag.Account)
                .FirstOrDefaultAsync(gp => gp.Id == request.Id, cancellationToken);

            if (groupPermission == null)
            {
                throw new AppException(ExceptionCode.Notfound, "Không tìm thấy GroupPermission");
            }

            return _mapper.Map<GroupPermissionDto>(groupPermission);
        }
    }
}

