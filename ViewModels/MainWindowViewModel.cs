using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Services;
using System;
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
        private int _total;
        private int _totalPages;

        public int[] PageSizes => new[] { 10, 50, 75, 100 };

        public MainWindowViewModel(IItemService itemService)
        {
            _itemService = itemService;
            PageNumber = 1;
            PageSize = PageSizes[0];
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
                    OnPropertyChanged(nameof(IsForwardButtonShown));
                    OnPropertyChanged(nameof(IsBackButtonShown));
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

        public int Total
        {
            get => _total;
            set
            {
                if (value != _total)
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                if (value != _totalPages)
                {
                    _totalPages = value;
                    OnPropertyChanged(nameof(TotalPages));
                    OnPropertyChanged(nameof(IsForwardButtonShown));                    
                }
            }
        }
        
        public async Task LoadData()
        {           
            var itemsRequest = new GetItemsRequest() { PageNumber = PageNumber, PageSize = PageSize };
            var itemsResponse = await _itemService.GetItems(itemsRequest);

            Total = itemsResponse.Total;
            TotalPages = (int)Math.Ceiling((decimal)Total / PageSize);

            Items = itemsResponse.Items.ToList();                        
        }

        public bool IsBackButtonShown => PageNumber > 1;
        public bool IsForwardButtonShown
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }

        public Task DeleteItemById(int itemId) => _itemService.DeleteItem(itemId);

        public Task UpdateItemInStock(int itemId, bool inStock) => _itemService.UpdateItemInStock(new UpdateItemInStockRequest { Id = itemId, InStock = inStock });               
    }
}