using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CasinoConsoleApp
{
    public partial class StockRecords
    {
        public int? Quantity { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }
}
