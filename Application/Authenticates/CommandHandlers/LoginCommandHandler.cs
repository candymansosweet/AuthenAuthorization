using Application.Accounts.Commands;
using Application.Accounts.Dto;
using Application.Authenticates.Commands;
using Application.Authenticates.Dto;
using AutoMapper;
using Common.Dtos;
using Common.Exceptions;
using Common.Services.JwtTokenService;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authenticates.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly AppDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        public LoginCommandHandler(
            AppDbContext context, 
            IJwtTokenService jwtTokenService
        )
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Account? acccount = await _context.Accounts         
                .Include(a => a.AccountGroupPermissions)
                    .ThenInclude(ag => ag.GroupPermission)
                    .ThenInclude(ass => ass.PermissionGroupPermissions)
                    .ThenInclude(per => per.Permission).Distinct()
                .FirstOrDefaultAsync(a => a.Name == request.Username, cancellationToken);
            if (acccount == null)
            {
                throw new AppException(ExceptionCode.Invalidate, "Username or password is incorrect");
            }
            // check password
            if (acccount == null || !BCrypt.Net.BCrypt.Verify(request.Password, acccount.PasswordHash))
                throw new AppException(ExceptionCode.Invalidate, "Username or password is incorrect");

            return _jwtTokenService.GenerateToken(
                new ClaimDto(
                    request.SecretString,
                    acccount.Id.ToString(),
                    acccount.Name,
                    acccount.AccountGroupPermissions
                        .SelectMany(ag => ag.GroupPermission.PermissionGroupPermissions)
                        .Select(ap => ap.Permission.Title)
                        .Distinct().ToList()
                )
            );
        }
    }
}
