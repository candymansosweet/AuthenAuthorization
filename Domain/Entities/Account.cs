using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Account : BaseModel
    {
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public virtual List<AssignGroup> AssignGroup { get; set; } = new List<AssignGroup>();
    }
}