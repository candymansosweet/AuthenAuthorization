using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authenticates.Dto
{
    public class LoginDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
