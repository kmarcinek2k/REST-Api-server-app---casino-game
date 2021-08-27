using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Contracts.V1.Requests
{
    public class UserRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
