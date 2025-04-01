using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoConsoleApp
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string GameType { get; set; }
        public ICollection<ClientSession> ClientSessions { get; set; }
        public override string ToString()
        {
            return $"{Id} {DateTime} {GameType} {ClientSessions}";

        }
    }
}
