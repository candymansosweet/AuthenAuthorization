using Application.GroupPermissions.Commands;
using Application.GroupPermissions.Dto;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using Infrastructure;
using MediatR;

namespace Application.GroupPermissions.CommandHandlers
{
    public class CreateGroupPermissionCommandHandler : IRequestHandler<CreateGroupPermissionCommand, GroupPermissionDto>
    {
        private readonly BanHangContext _context;
        private readonly IMapper _mapper;

        public CreateGroupPermissionCommandHandler(BanHangContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GroupPermissionDto> Handle(CreateGroupPermissionCommand request, CancellationToken cancellationToken)
        {
            if (_context.GroupPermissions.Any(x => x.Title == request.Title))
            {
                throw new AppException(ExceptionCode.Duplicate, $"Đã tồn tại GroupPermission {request.Title}",
                    new[] { new ErrorDetail(nameof(request.Title), request.Title) });
            }

            var groupPermission = new GroupPermission
            {
                Title = request.Title,
                Description = request.Description,
                AssignPermissions = request.PermissionIds.Select(id => new AssignPermission
                {
                    GroupPermissionId = id
                }).ToList(),
                AssignGroups = request.AccountIds.Select(id => new AssignGroup
                {
                    AccountId = id
                }).ToList()
            };

            _context.GroupPermissions.Add(groupPermission);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GroupPermissionDto>(groupPermission);
        }
    }
}

