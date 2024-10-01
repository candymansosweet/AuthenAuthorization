using Application.Common.Mapping;
using AutoMapper;
using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.Permissions.Dto
{
    public class PermissionDto: IMapFrom<Permission>
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<GroupPermissionDto> GroupPermissions { get; set; } = new List<GroupPermissionDto>();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.GroupPermissions, opt => opt.MapFrom(src =>
                    src.AssignPermissions.Select(ap => ap.GroupPermission).ToList()));
        }
    }
}
