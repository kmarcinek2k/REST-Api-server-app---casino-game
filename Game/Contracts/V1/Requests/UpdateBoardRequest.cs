using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Contracts.V1.Requests
{
    public class UpdateBoardRequest
    {
        public Guid Id { get; set; }
        public bool IsClosed { get; set; }
    }
}
