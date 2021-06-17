using System.ComponentModel;

namespace AVSSalesExplorer.Models
{
    /// <summary>
    /// Item category - clothes or bags
    /// </summary>
    public enum ItemCategory
    {
        [Description("Вещи")]
        Clothes,
        
        [Description("Сумки")]
        Bags
    }
}