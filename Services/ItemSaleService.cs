using AVSSalesExplorer.Common;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Models;
using AVSSalesExplorer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AVSSalesExplorer.Services
{
    public interface IItemSaleService
    {
        Task<int> CreateNewItemSale(NewItemSaleRequest request);
        Task<GetSalesResponse> GetItemSales(GetSalesRequest request);
    }

    public class ItemSaleService : IItemSaleService
    {
        private readonly ItemDbContext _itemContext;

        public ItemSaleService(ItemDbContext itemContext)
        {
            _itemContext = itemContext;
        }

        public async Task<int> CreateNewItemSale(NewItemSaleRequest request)
        {
            var item = await _itemContext.Items.Include(i => i.Sales).Include(i => i.Sizes)
                    .FirstOrDefaultAsync(it => it.Id == request.ItemId);

            if (item is null)
            {
                return 0; // means unsuccessfull sale
            }

            ItemSize size = null;
            if (item.Category != ItemCategory.Bags)
            {
                size = item.Sizes.FirstOrDefault(s => s.Size == request.Size);
                if (size is null)
                {
                    return 0;
                }

                if (size.Amount >= 1)
                {
                    size.Amount--;
                }
                else
                {
                    return 0;   // Cannot sale because the item size is out of stock.
                }

                item.InStock = item.Sizes.Any(s => s.Amount > 0);
            }
            
            var sale = new Sale()
            {
                Size = size,
                Price = request.Price,
                Phone = request.Phone,
                Address = request.Address,
                SaleDate = request.SaleDate,
                Customer = request.Customer
            };
                       
            item.Sales.Add(sale);            
            await _itemContext.SaveChangesAsync();

            return sale.Id;            
        }

        public async Task<GetSalesResponse> GetItemSales(GetSalesRequest request)
        {
            var item = await _itemContext.Items.Include(s => s.Sales).FirstOrDefaultAsync(i => i.Id == request.ItemId);
            if (item is null || item.Sales is null || !item.Sales.Any())
            {
                return new GetSalesResponse();
            }

            var sales = item.Sales.Select(s => new SaleViewModel 
                        { 
                            Id = s.Id, 
                            Size = s.Size?.Size,
                            Price = s.Price, 
                            Profit = s.Price - item.Price,
                            Address = s.Address, 
                            Customer = s.Customer,                             
                            Phone = s.Phone, 
                            SaleDate = s.SaleDate 
                        }
                ).ToArray();

            return new GetSalesResponse
            {
                Sales = sales
            };
        }
    }
}