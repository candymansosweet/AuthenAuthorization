using Application.Accounts.Dto;
using Application.Accounts.Queries;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Accounts.QueryHandlers
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountById, AccountDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetAccountByIdHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDto?> Handle(GetAccountById request, CancellationToken cancellationToken)
        {
            //var account = await _context.Accounts
            //    .Include(a => a.AccountGroupPermissions)
            //        .ThenInclude(ag => ag.GroupPermission)
            //        .ThenInclude(gp => gp.PermissionGroupPermissions)
            //        .ThenInclude(pp => pp.Permission)
            //    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            IQueryable<Account> accountQuery = _context.Accounts.Where(a => a.Id == request.Id);

            AccountDto? account = _mapper.ProjectTo<AccountDto>(accountQuery).FirstOrDefault();
            if (account == null)
            {
                throw new AppException(ExceptionCode.Notfound, "Không tìm thấy Account");
            }
            return account;
        }
    }
}

