﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSSalesExplorer.DTOs
{
    public class GetItemsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}