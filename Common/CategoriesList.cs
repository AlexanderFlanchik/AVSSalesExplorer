using System.Collections.Generic;

namespace AVSSalesExplorer.Common
{
    public class CategoriesList : Dictionary<int, string>
    {
        public CategoriesList()
        {
            Add(-1, "Все");
            Add((int)ItemCategory.Clothes, "Вещи");
            Add((int)ItemCategory.Bags, "Сумки");
        }
    }
}