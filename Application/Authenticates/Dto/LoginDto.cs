using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authenticates.Dto
{
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new List<string>();
        public string Token { get; set; } = string.Empty;
    }
}
