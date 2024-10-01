using Application.Accounts.Dto;
using MediatR;

namespace Application.Accounts.Queries
{
    public class FilterAccount : IRequest<List<AccountDto>>
    {
    }
}