using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Lớp đại diện cho nhóm quyền
    public class GroupPermission : BaseModel
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public virtual List<PermissionGroupPermission> PermissionGroupPermissions { get; set; } = new List<PermissionGroupPermission>();
        [JsonIgnore]
        public virtual List<AccountGroupPermission> AccountGroupPermissions { get; set; } = new List<AccountGroupPermission>();
    }
}