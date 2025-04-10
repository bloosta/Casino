using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasinoConsoleApp
{
    [Table("clientsession")]
    public class ClientSession
    {

        [ForeignKey("clientid")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [ForeignKey("sessionid")]
        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
