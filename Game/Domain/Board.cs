using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Domain
{
    public class Board
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AdminUserId { get; set; }
        public int MaxNumberOfPlayers { get; set; }
        public int CurrentNumberOfPlayers { get; set; }
        public bool IsClosed { get; set; }
        public bool HasEnded { get; set; }
        public int CurrentPool { get; set; }

        [ForeignKey(nameof(AdminUserId))]
        public IdentityUser User { get; set; }
    }
}
