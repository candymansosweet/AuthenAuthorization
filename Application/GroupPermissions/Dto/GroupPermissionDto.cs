using Application.Common.Mapping;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GroupPermissions.Dto
{
    public class GroupPermissionDto : IMapFrom<GroupPermission>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
        public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupPermission, GroupPermissionDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src =>
                    src.AssignPermissions.Select(ap => ap.Permission).ToList()))
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src =>
                    src.AssignGroups.Select(ag => ag.Account).ToList()));
        }
    }
}
