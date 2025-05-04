using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CasinoConsoleApp.Core.Entities
{
    public class Client 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}