using Application.Accounts.Dto;
using Application.Accounts.Queries;
using AutoMapper;
using Common.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.QueryHandlers
{
    public class FilterAccountHandler : IRequestHandler<FilterAccount, PaginatedList<AccountDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FilterAccountHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<AccountDto>> Handle(FilterAccount request, CancellationToken cancellationToken)
        {
            IQueryable<Account> accounts = _context.Accounts.OrderBy(e => e.Id).AsQueryable();
            

            return await PaginatedList<AccountDto>.CreateAsync(
                _mapper.ProjectTo<AccountDto>(accounts),
                request.PageNumber,
                request.PageSize
            );
        }
    }
}
