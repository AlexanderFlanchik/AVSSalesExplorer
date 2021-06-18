using AVSSalesExplorer.Common;
using System;
using System.Collections.Generic;

namespace AVSSalesExplorer.Models
{
    /// <summary>
    /// Represent clothes item or bag.
    /// </summary>
    public class Item
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public ItemCategory Category { get; set; }
        public string Description { get; set; }        
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public string Comment { get; set; }

        public virtual ICollection<ItemSize> Sizes { get; set; } // only for clothes
        public virtual ICollection<Sale> Sales { get; set; }
    }
}