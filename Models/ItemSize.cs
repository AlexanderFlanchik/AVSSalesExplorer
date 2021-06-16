namespace AVSSalesExplorer.Models
{
    public class ItemSize
    {
        public int ItemId { get; set; }
        public ushort Size { get; set; }
        public virtual Item Item { get; set; }
    }
}