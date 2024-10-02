using Application.Accounts.Dto;
using Common.Models;
using MediatR;

namespace Application.Accounts.Queries
{
    public class FilterAccount : BasePaginatedQuery, IRequest<PaginatedList<AccountDto>>
    {
    }
}