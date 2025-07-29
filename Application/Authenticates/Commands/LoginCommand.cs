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
    public class LoginCommand: IRequest<string>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string SecretString { get; set; }
    }
}
