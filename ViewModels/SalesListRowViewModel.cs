using System;

namespace AVSSalesExplorer.ViewModels
{
    public class SalesListRowViewModel
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public DateTime SaleDate { get; set; }
        public ushort? Size { get; set; }
        public decimal Price { get; set; }
        public decimal Profit { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}