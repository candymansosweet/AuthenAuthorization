using Application.Accounts.Dto;
using MediatR;

namespace Application.Accounts.Commands
{
    public class CreateAccountCommand : IRequest<AccountDto>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public List<int> GroupPermissionIds { get; set; } = new List<int>();
    }
}

