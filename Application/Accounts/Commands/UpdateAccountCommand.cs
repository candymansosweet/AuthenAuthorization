using Application.Accounts.Dto;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.Commands
{
    public class UpdateAccountCommand : IRequest<AccountDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordOld { get; set; }
        public List<int> GroupPermissionIds { get; set; } = new List<int>();
    }
}

