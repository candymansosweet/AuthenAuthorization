using Application.Accounts.Dto;
using Application.Accounts.Queries;
using AutoMapper;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.QueryHandlers
{
    public class FilterAccountHandler : IRequestHandler<FilterAccount, List<AccountDto>>
    {
        private readonly BanHangContext _context;
        private readonly IMapper _mapper;

        public FilterAccountHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AccountDto>> Handle(FilterAccount request, CancellationToken cancellationToken)
        {
            var accounts = await _context.Accounts
                .Include(a => a.AssignGroup)
                    .ThenInclude(ag => ag.GroupPermission)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<AccountDto>>(accounts);
        }
    }
}
