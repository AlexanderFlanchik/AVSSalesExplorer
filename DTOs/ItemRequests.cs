using AVSSalesExplorer.Common;
using System;

namespace AVSSalesExplorer.DTOs
{
    public abstract class ItemRequest
    {
        public decimal Price { get; set; }
        public ItemCategory Category { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Comment { get; set; }

        public ItemSizeRequest[] Sizes { get; set; }
    }

    public class ItemSizeRequest
    {        
        public ushort Size { get; set; }
        public bool InStock { get; set; }
    }

    public class AddNewItemRequest : ItemRequest
    { }

    public class UpdateItemRequest: ItemRequest
    {
        public int Id { get; set; }
    }
}