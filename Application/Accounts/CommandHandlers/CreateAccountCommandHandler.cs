using Application.Accounts.Commands;
using Application.Accounts.Dto;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using Infrastructure;
using MediatR;

namespace Application.Accounts.CommandHandlers
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDto>
    {
        private readonly BanHangContext _context;
        private readonly IMapper _mapper;

        public CreateAccountCommandHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            if (_context.Accounts.Any(x => x.Name == request.Name))
            {
                throw new AppException(ExceptionCode.Duplicate, $"Đã tồn tại Account {request.Name}",
                    new[] { new ErrorDetail(nameof(request.Name), request.Name) });
            }

            var account = new Account
            {
                Name = request.Name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                AssignGroup = request.GroupPermissionIds.Select(id => new AssignGroup
                {
                    GroupPermissionId = id
                }).ToList()
            };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AccountDto>(account);
        }
    }
}
