using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSSalesExplorer.ViewModels
{
    public class SalesListViewModel : ViewModelBase
    {
        private readonly ISalesListDataService _salesService;
        
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private SalesListRowViewModel[] _sales;
        private decimal _periodProfit;
        private int _pageNumber;
        private int _pageSize;
        private int _totalSales;
        private int _totalPages;

        public SalesListViewModel(ISalesListDataService salesService)
        {
            _salesService = salesService;
            PageSize = 25;
            PageNumber = 1;
        }

        public int[] PageSizes => new[] { 10, 25, 50, 100 };

        public SalesListRowViewModel[] Sales
        {
            get => _sales;
            set
            {
                _sales = value;
                OnPropertyChanged(nameof(Sales));
            }
        }

        public DateTime DateFrom
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

        public DateTime DateTo
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

        public decimal PeriodProfit
        {
            get => _periodProfit;
            set
            {
                if (value != _periodProfit)
                {
                    _periodProfit = value;
                    OnPropertyChanged(nameof(PeriodProfit));
                }
            }
        }

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (value != _pageNumber)
                {
                    _pageNumber = value;
                    OnPropertyChanged(nameof(PageNumber));
                    OnPropertyChanged(nameof(IsPageBackEnabled));
                    OnPropertyChanged(nameof(IsPageForwardEnabled));
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

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                if (value != _totalPages)
                {
                    _totalPages = value;
                    OnPropertyChanged(nameof(TotalPages));
                    OnPropertyChanged(nameof(IsPageForwardEnabled));
                }
            }
        }

        public int TotalSales
        {
            get => _totalSales;
            set
            {
                _totalSales = value;
                OnPropertyChanged(nameof(TotalSales));
            }
        }

        public bool IsPageBackEnabled => PageNumber > 1;
        public bool IsPageForwardEnabled => PageNumber < TotalPages;

        public async Task LoadData()
        {
            var salesRequest = new GetSalesListRequest()
            {
                DateFrom = DateFrom,
                DateTo = DateTo,
                PageNumber = PageNumber,
                PageSize = PageSize
            };

            var response = await _salesService.GetAllSales(salesRequest);
            TotalPages = (int)Math.Ceiling((decimal)response.TotalSales / PageSize);
            TotalSales = response.TotalSales;

            Sales = response.Sales;
            PeriodProfit = response.PeriodProfit;
        }
    }
}