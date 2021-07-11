using AVSSalesExplorer.ViewModels;

namespace AVSSalesExplorer.DTOs
{
    public class GetSalesListResponse
    {
        public SalesListRowViewModel[] Sales { get; set; }
        public int TotalSales { get; set; }
        public decimal PeriodProfit { get; set; }
    }
}