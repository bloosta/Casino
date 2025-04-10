using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasinoConsoleApp
{
    [Table("clients")]
    public class Client
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public ICollection<ClientSession> ClientSessions { get; set; }
    }
}
