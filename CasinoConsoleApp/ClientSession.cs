using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoConsoleApp
{
    public class ClientSession
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
