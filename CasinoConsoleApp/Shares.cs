using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CasinoConsoleApp
{
    public partial class Shares
    {
        public Shares()
        {
            Deals = new HashSet<Deals>();
        }

        public int Id { get; set; }
        public int? IssuerId { get; set; }
        public string Name { get; set; }
        public int? Nominal { get; set; }

        public virtual Issuer Issuer { get; set; }
        public virtual ICollection<Deals> Deals { get; set; }
    }
}
