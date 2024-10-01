using Application.Common.Mapping;
using Application.Permissions.Dto;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts.Dto
{
    public class AccountDto : IMapFrom<Account>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GroupPermissionDto> GroupPermissions { get; set; } = new List<GroupPermissionDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.GroupPermissions, opt => opt.MapFrom(src =>
                    src.AssignGroup.Select(ag => ag.GroupPermission).ToList()));
        }
    }
}

