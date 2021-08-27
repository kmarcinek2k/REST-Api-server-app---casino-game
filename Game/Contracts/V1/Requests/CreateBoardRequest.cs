using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Contracts.V1.Requests
{
    public class CreateBoardRequest
    {
        public string Name { get; set; }
        public int MaxNumberOfPlayers { get; set; }
        public bool IsClosed { get; set; }
    }
}
