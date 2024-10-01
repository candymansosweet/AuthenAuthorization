using Application.Common.Mapping;
using Application.Permissions.Dto;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Permissions.Commands
{
    public class CreatePermissionCommand : IRequest<PermissionDto>, IMapTo<Permission>
    {
        public int? Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<int> GroupPermissionIds { get; set; } = new List<int>();
    }
}
