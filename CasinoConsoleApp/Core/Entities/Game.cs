using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CasinoConsoleApp.Core.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public DateTime PlayedAt { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        public ICollection<Client> Players { get; set; } = new List<Client>();
    }
}