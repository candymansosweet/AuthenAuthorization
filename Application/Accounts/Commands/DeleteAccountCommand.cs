using Application.Accounts.Dto;
using MediatR;
namespace Application.Accounts.Commands
{
    public class DeleteAccountCommand : IRequest<AccountDto>
    {
        public int Id { get; set; }
    }
}

