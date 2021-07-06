using System.Collections.Generic;

namespace AVSSalesExplorer.Common
{
    public class CategoriesList : Dictionary<short, string>
    {
        public CategoriesList()
        {
            Add(-1, "Все");
            Add((short)ItemCategory.Clothes, "Вещи");
            Add((short)ItemCategory.Bags, "Сумки");
        }
    }
}