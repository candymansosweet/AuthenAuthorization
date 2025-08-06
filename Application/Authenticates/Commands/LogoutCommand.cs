using Application.Accounts.Dto;
using Application.Authenticates.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authenticates.Commands
{
    public class LogoutCommand: IRequest<LogoutDto>
    {
        public string Token { get; set; }
    }
}
