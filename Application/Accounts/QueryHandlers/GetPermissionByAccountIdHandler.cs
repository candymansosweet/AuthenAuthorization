using Application.Accounts.Dto;
using Application.Accounts.Queries;
using AutoMapper;
using Common.Exceptions;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace Application.Accounts.QueryHandlers
{
    public class GetPermissionByAccountIdHandler : IRequestHandler<GetPermissionByAccountId, AccountPermissionDto>
    {
        private readonly BanHangContext _context;
        private readonly IMapper _mapper;

        public GetPermissionByAccountIdHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountPermissionDto> Handle(GetPermissionByAccountId request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Include(a => a.AssignGroup)
                    .ThenInclude(ag => ag.GroupPermission)
                    .ThenInclude(ass => ass.AssignPermissions)
                    .ThenInclude(per => per.Permission).Distinct()
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (account == null)
            {
                throw new AppException(ExceptionCode.Notfound, "Không tìm thấy Account");
            }

            return _mapper.Map<AccountPermissionDto>(account);
        }
    }
}

