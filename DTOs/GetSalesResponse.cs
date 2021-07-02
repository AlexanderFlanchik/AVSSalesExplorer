using AVSSalesExplorer.ViewModels;
using System;

namespace AVSSalesExplorer.DTOs
{
    public class GetSalesResponse
    {
        public SaleViewModel[] Sales { get; set; } = Array.Empty<SaleViewModel>();
    }
}