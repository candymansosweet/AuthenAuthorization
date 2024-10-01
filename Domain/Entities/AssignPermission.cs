using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AssignPermission : BaseModel
    {
        public int PermissionId { get; set; }
        public int GroupPermissionId { get; set; }
        [ForeignKey(nameof(PermissionId))]
        public virtual Permission? Permission { get; set; }
        [ForeignKey(nameof(GroupPermissionId))]
        public virtual GroupPermission? GroupPermission { get; set; }
    }
}