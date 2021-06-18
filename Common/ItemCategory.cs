using System.ComponentModel;

namespace AVSSalesExplorer.Common
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