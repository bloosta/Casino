using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CasinoConsoleApp
{
    public partial class NonUpdatableView
    {
        public int? DealId { get; set; }
        public string ShopName { get; set; }
        public string ShareName { get; set; }
        public string Type { get; set; }
        public int? Cost { get; set; }
        public int? Retail { get; set; }
        public int? Quantity { get; set; }
    }
}
