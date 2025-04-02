using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoConsoleApp
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ClientSession> ClientSessions { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name}";

        }
    }
}
