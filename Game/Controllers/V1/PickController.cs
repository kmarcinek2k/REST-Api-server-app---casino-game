using Game.Contracts.V1;
using Game.Domain;
using Game.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Controllers.V1
{
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="Admin,Casual")]
    public class PickController
    {

        public PickController()
        {
        }
        [HttpGet(ApiRoutes.Picks.Get)]
        public async Task<Pick> Get()
        {
            Pick p = new Pick();
            return p;
        }


    }
        
}
