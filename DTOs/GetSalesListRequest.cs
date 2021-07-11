using System;

namespace AVSSalesExplorer.DTOs
{
    public class GetSalesListRequest: PaginationData
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }        
    }
}