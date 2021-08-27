using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Contracts.V1.Requests
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string Token { get; set; }
    }
}
