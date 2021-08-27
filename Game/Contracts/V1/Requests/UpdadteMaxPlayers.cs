using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Contracts.V1.Requests
{
    public class UpdateMaxPlayers
    {
        public Guid Id { get; set; }
        public int MaxPlayers { get; set; }
    }
}
