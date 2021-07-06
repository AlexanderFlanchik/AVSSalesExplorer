using System;

namespace AVSSalesExplorer.DTOs
{
    public class GetItemsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public short CategoryFilter { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }
}