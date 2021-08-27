using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Domain
{
    
    public class Pick
    {
        public int Number { get; set; }
        public string Color { get; set; }
        public Pick()
        {
            var rand = new Random();
            Number = rand.Next(37);
            if (Number == 0) Color = "Green";
            else if (Number % 2 == 0) Color = "Red";
            else Color = "Black";
        }

    }
}
