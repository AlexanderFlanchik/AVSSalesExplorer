using AVSSalesExplorer.Models;
using System;
using System.Linq;

namespace AVSSalesExplorer.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public ItemCategory Category { get; set; }
        public string Description { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public string Comment { get; set; }

        public ItemSize[] Sizes { get; set; }
        public Sale[] Sales { get; set; }

        public int? SalesAmount => Sales?.Count();

        public static ItemViewModel MapFromItem(Item item) =>
            new ItemViewModel
            {
                Id = item.Id,
                Photo = item.Photo,
                Category = item.Category,
                Description = item.Description,
                PurchaseDate = item.PurchaseDate,
                Price = item.Price,
                InStock = item.InStock,
                Comment = item.Comment,
                Sizes = item.Sizes,
                Sales = item.Sales
            };
    }
}