using Application.Accounts.Dto;
using Application.Common.Mapping;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.Commands
{
    public class UpdateAccountCommand : IRequest<AccountDto>, IMapTo<Account>
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? PasswordNew { get; set; }
        public string? PasswordOld { get; set; }
        public List<int> GroupPermissionIds { get; set; } = new List<int>();
    }
}

