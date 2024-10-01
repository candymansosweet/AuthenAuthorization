using Application.Accounts.Dto;
using Application.Accounts.Queries;
using AutoMapper;
using Common.Exceptions;
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

        public async Task<AccountDto> Handle(GetAccountById request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Include(a => a.AssignGroup)
                    .ThenInclude(ag => ag.GroupPermission)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (account == null)
            {
                throw new AppException(ExceptionCode.Notfound, "Không tìm thấy Account");
            }

            return _mapper.Map<AccountDto>(account);
        }
    }
}

