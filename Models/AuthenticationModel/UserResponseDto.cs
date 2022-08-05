using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AuthenticationModel
{
    public class UserResponseDto
    {
        public string Email { get; set; } = "";
        public string Token { get; set; } = "";
        public DateTime CookieExpiryDate { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
