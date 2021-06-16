using System;

namespace AVSSalesExplorer.Models
{
    public class Sale
    {
        public int ItemId { get; set; }
        public decimal Price { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime SaleDate { get; set; }

        public virtual Item Item { get; set; }
        public virtual ItemSize Size { get; set; }
    }
}