
using Application.Accounts.Commands;
using Application.Accounts.Dto;
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

namespace Application.Accounts.CommandHandlers
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, AccountDto>
    {
        private BanHangContext _context;
        private readonly IMapper _mapper;

        public DeleteAccountCommandHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AccountDto> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            Account? acc = await _context.Accounts.FindAsync(request.Id) ??
                throw new AppException(ExceptionCode.Notfound, "Không tìm thấy Account");
            _context.Accounts.Remove(acc);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AccountDto>(acc);
        }
    }
}
