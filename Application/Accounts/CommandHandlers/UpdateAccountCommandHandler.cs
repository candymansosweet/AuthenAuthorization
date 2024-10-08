﻿using Application.Accounts.Commands;
using Application.Accounts.Dto;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Application.Accounts.CommandHandlers
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, AccountDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UpdateAccountCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDto> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Include(a => a.AssignGroup)
                .FirstOrDefaultAsync(a => a.Id == request.Id);

            if (account == null)
            {
                throw new AppException(ExceptionCode.Notfound, $"Không tìm thấy Account {request.Id}",
                    new[] { new ErrorDetail(nameof(request.Id), request.Id) });
            }
            if (request.Name.IsNullOrEmpty())
            {
                account.Name = request.Name;
            }
            if (request.PasswordNew != null)
            {
                if (BCrypt.Net.BCrypt.Verify(request.PasswordOld, account.PasswordHash))
                {
                    account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordNew);
                }
                else
                {
                    throw new AppException(ExceptionCode.Invalidate, $"Nhập sai mật khẩu cũ");
                }
            }
            if(request.GroupPermissionIds.Count > 0)
            {
                account.AssignGroup = request.GroupPermissionIds.Select(id => new AssignGroup
                {
                    AccountId = request.Id,
                    GroupPermissionId = id
                }).ToList();
            }
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<AccountDto>(account);
        }
    }
}

