using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AuthenticationModel
{
    public class LoginRequestDto
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = string.Empty;
    }
}
