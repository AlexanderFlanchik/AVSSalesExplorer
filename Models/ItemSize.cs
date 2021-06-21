namespace AVSSalesExplorer.Models
{
    public class ItemSize
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public ushort Size { get; set; }
        public virtual Item Item { get; set; }
        public bool InStock { get; set; }
        public ushort Amount { get; set; }
    }
}