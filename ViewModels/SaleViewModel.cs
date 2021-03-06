using System;

namespace AVSSalesExplorer.ViewModels
{
    public class SaleViewModel
    {
        public int Id { get; set; }
        public int? Size { get; set; }
        public decimal Price { get; set; }
        public decimal Profit {get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}