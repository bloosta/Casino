using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CasinoConsoleApp
{
    public partial class Deals
    {
        public int Id { get; set; }
        public int? ShopId { get; set; }
        public int? ShareId { get; set; }
        public string Type { get; set; }
        public int? Cost { get; set; }
        public int? Retail { get; set; }
        public int? Quantity { get; set; }

        public virtual Shares Share { get; set; }
        public virtual Shops Shop { get; set; }
    }
}
