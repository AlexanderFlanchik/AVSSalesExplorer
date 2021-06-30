using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVSSalesExplorer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IItemService _itemService;

        private IList<ItemViewModel> _items;
        private int _pageNumber;
        private int _pageSize;

        public MainWindowViewModel(IItemService itemService)
        {
            _itemService = itemService;
        }

        public IList<ItemViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        
        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (_pageNumber != value)
                {
                    _pageNumber = value;
                    OnPropertyChanged(nameof(PageNumber));
                }
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value != _pageSize)
                {
                    _pageSize = value;
                    OnPropertyChanged(nameof(PageSize));
                }
            }
        }
        
        public async Task LoadData()
        {           
            var itemsRequest = new GetItemsRequest() { PageNumber = 1, PageSize = 50 };
            var itemsResponse = await _itemService.GetItems(itemsRequest);
            
            Items = itemsResponse.Items.ToList();                        
        }
        
        public Task DeleteItemById(int itemId) => _itemService.DeleteItem(itemId);

        public Task UpdateItemInStock(int itemId, bool inStock) => _itemService.UpdateItemInStock(new UpdateItemInStockRequest { Id = itemId, InStock = inStock });               
    }
}