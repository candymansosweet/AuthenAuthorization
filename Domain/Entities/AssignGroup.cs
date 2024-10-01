using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities
{
    public class AssignGroup : BaseModel
    {
        public int AccountId { get; set; }
        public int GroupPermissionId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public virtual Account? Account { get; set; }
        [ForeignKey(nameof(GroupPermissionId))]
        public virtual GroupPermission? GroupPermission { get; set; }
    }
}