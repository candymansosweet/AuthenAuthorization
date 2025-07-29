using System.Text.Json.Serialization;

namespace Domain.Entities
{
    // Lớp đại diện cho tài khoản người dùng
    public class Account : BaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public virtual List<AccountGroupPermission> AccountGroupPermissions { get; set; } = new List<AccountGroupPermission>();
    }
}