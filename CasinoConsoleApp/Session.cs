using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasinoConsoleApp
{
    [Table("sessions")]
    public class Session
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("datetime")]
        public DateTime DateTime { get; set; }

        [Column("gametype")]
        public string GameType { get; set; }

        public ICollection<ClientSession> ClientSessions { get; set; }
    }
}
