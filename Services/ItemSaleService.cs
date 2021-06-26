using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AVSSalesExplorer.Services
{
    public interface IItemSaleService
    {
        Task<int> CreateNewItemSale(NewItemSaleRequest request);
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
            var item = await _itemContext.Items.Include(i => i.Sales).Include(i => i.Sizes).FirstOrDefaultAsync();
            if (item is null)
            {
                return 0; // means unsuccessfull sale
            }

            var size = item.Sizes.FirstOrDefault(s => s.Size == request.Size);
            if (size is null)
            {
                return 0;
            }

            if (size.Amount > 1)
            {
                size.Amount--;
            }
            else
            {
                return 0;
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
    }
}