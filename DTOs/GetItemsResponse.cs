using AVSSalesExplorer.ViewModels;
using System.Collections.Generic;

namespace AVSSalesExplorer.DTOs
{
    public class GetItemsResponse
    {
        public IEnumerable<ItemViewModel> Items { get; set; }
        public int Total { get; set; }
    }
}