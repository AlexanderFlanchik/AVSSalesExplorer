using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Services;
using System.Threading.Tasks;

namespace AVSSalesExplorer.ViewModels
{
    public class ItemSalesViewModel
    {
        private readonly IItemSaleService _saleService;

        public ItemSalesViewModel(IItemSaleService saleService)
        {
            _saleService = saleService;
        }

        public async Task LoadData()
        {
            if (ItemId == default)
            {
                return;
            }

            var salesResponse = await _saleService.GetItemSales(new GetSalesRequest(ItemId));
            Sales = salesResponse.Sales;
        }

        public int ItemId { get; set; }
        public SaleViewModel[] Sales { get; set; }
    }
}