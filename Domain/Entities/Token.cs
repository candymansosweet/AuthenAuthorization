using System.Text.Json.Serialization;

namespace Domain.Entities
{
    // Lớp đại diện cho tài khoản người dùng
    public class Token : BaseModel
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
        public string SecretString { get; set; }
    }
}