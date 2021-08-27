using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Contracts.V1.Requests
{
    public class UpdateCurrentPlayersInBoard
    {
        public Guid Id { get; set; }
        public int Count { get; set; }
    }
}
