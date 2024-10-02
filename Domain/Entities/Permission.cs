using System.Text.Json.Serialization;

namespace Domain.Entities
{
    // Lớp đại diện cho quyền
    public class Permission : BaseModel
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public virtual List<AssignPermission> AssignPermissions { set; get; } = new List<AssignPermission>();
    }
}