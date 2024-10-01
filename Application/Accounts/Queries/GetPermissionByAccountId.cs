using Application.Accounts.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts.Queries
{
    public class GetPermissionByAccountId : IRequest<AccountPermissionDto>
    {
        public int Id { get; set; }
    }
}