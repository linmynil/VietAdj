using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public class PaginationInputBase
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
        public PaginationInputBase()
        {
            PageSize = 20;
        }
    }
}
