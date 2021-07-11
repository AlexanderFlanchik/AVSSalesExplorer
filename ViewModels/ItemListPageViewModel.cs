using AVSSalesExplorer.Common;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVSSalesExplorer.ViewModels
{
    public class ItemListPageViewModel : ViewModelBase
    {
        private readonly IItemService _itemService;

        private IList<ItemViewModel> _items;
        private int _pageNumber;
        private int _pageSize;
        private int _total;
        private int _totalPages;
        private short _categoryFilter;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private decimal? _priceFrom;
        private decimal? _priceTo;

        public static int[] PageSizes => new[] { 25, 50, 75, 100 };
        
        public ItemListPageViewModel(IItemService itemService)
        {
            _itemService = itemService;
            PageNumber = 1;
            PageSize = PageSizes[0];
            CategoryFilter = -1;
        }
       
        public IList<ItemViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(ItemsGridShown));
                OnPropertyChanged(nameof(NoItemsMessagesVisible));
            }
        }

        public bool ItemsGridShown => Items is not null && Items.Any();
        public bool NoItemsMessagesVisible => Items is null || !Items.Any();

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

        public short CategoryFilter
        {
            get => _categoryFilter;
            set
            {
                if (value != _categoryFilter)
                {
                    _categoryFilter = value;
                    OnPropertyChanged(nameof(CategoryFilter));
                }
            }
        }

        public DateTime? DateFrom
        {
            get => _dateFrom;
            set
            {
                if (value != _dateFrom)
                {
                    _dateFrom = value;
                    OnPropertyChanged(nameof(DateFrom));
                }
            }
        }
        public DateTime? DateTo
        {
            get => _dateTo;
            set
            {
                if (value != _dateTo)
                {
                    _dateTo = value;
                    OnPropertyChanged(nameof(DateTo));
                }
            }
        }

        public decimal? PriceFrom
        {
            get => _priceFrom;
            set
            {
                if (value != _priceFrom)
                {
                    _priceFrom = value;
                    OnPropertyChanged(nameof(PriceFrom));
                }
            }
        }

        public decimal? PriceTo
        {
            get => _priceTo;
            set
            {
                if (value != _priceTo)
                {
                    _priceTo = value;
                    OnPropertyChanged(nameof(PriceTo));
                }
            }
        }

        public async Task LoadData()
        {           
            var itemsRequest = new GetItemsRequest() 
                { 
                    PageNumber = PageNumber, 
                    PageSize = PageSize,
                    CategoryFilter = CategoryFilter,
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    PriceFrom = PriceFrom,
                    PriceTo = PriceTo
                };

            var itemsResponse = await _itemService.GetItems(itemsRequest);

            Total = itemsResponse.Total;
            TotalPages = (int)Math.Ceiling((decimal)Total / PageSize);
            
            Items = itemsResponse.Items.ToList();                        
        }

        public void ResetFilters()
        {
            CategoryFilter = -1;
            DateFrom = null;
            DateTo = null;
            PriceFrom = null;
            PriceTo = null;
        }

        public bool IsBackButtonShown => PageNumber > 1;
        public bool IsForwardButtonShown => PageNumber < TotalPages;        
        public Task DeleteItemById(int itemId) => _itemService.DeleteItem(itemId);
        public Task UpdateItemInStock(int itemId, bool inStock) => _itemService.UpdateItemInStock(new UpdateItemInStockRequest { Id = itemId, InStock = inStock });                
    }   
}