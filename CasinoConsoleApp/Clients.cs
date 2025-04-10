using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CasinoConsoleApp
{
    public partial class Clients
    {
        public Clients()
        {
            Clientsession = new HashSet<Clientsession>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Clientsession> Clientsession { get; set; }
    }
}
