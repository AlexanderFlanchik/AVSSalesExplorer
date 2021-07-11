using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Models;
using AVSSalesExplorer.ViewModels;

namespace AVSSalesExplorer.Services
{
    public interface ISalesListDataService
    {
        Task<GetSalesListResponse> GetAllSales(GetSalesListRequest request);
    }

    public class SalesListDataService : ISalesListDataService
    {
        private readonly ItemDbContext _itemContext;

        public SalesListDataService(ItemDbContext itemContext)
        {
            _itemContext = itemContext;
        }

        public async Task<GetSalesListResponse> GetAllSales(GetSalesListRequest request)
        {
            IQueryable<Sale> salesQuery = _itemContext.Sales.Include(s => s.Item).Include(s => s.Size)
                    .Where(s => s.SaleDate >= request.DateFrom && s.SaleDate <= request.DateTo)
                    .OrderByDescending(s => s.SaleDate);

            var totalSales = await salesQuery.CountAsync();
            var periodProfit = (await salesQuery.Select(s => s.Price - s.Item.Price).ToArrayAsync()).Sum();
            var salesData = await salesQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                    .Select(s => new SalesListRowViewModel
                                    {
                                        Id = s.Id,
                                        Photo = s.Item.Photo,
                                        Price = s.Price,
                                        Profit = s.Price - s.Item.Price,
                                        SaleDate = s.SaleDate,
                                        Size = s.Size != null ? s.Size.Size : null,
                                        Customer = s.Customer,
                                        Address = s.Address,
                                        Phone = s.Phone
                                    }
                    ).ToArrayAsync();

            var response = new GetSalesListResponse() 
                                    { 
                                        Sales = salesData, 
                                        TotalSales = totalSales,
                                        PeriodProfit = periodProfit
                                    };
            
            return response;
        }
    }
}