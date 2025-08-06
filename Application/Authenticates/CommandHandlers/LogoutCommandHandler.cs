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
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutDto>
    {
        private readonly AppDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        public LogoutCommandHandler(
            AppDbContext context, 
            IJwtTokenService jwtTokenService
        )
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<LogoutDto> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            // Xoá token khỏi blacklist
            _jwtTokenService.RevokeToken(request.Token);
            LogoutDto logoutDto = new LogoutDto
            {
                Mess = "Logout successful"
            };
            // Trả về thông báo thành công
            return logoutDto;
        }
    }
}
