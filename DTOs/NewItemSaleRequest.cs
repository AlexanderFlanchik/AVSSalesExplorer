using System;

namespace AVSSalesExplorer.DTOs
{
    public class NewItemSaleRequest
    {
        public int ItemId { get; set; }
        public ushort Size { get; set; }
        public decimal Price { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime SaleDate { get; set; }
    }
}