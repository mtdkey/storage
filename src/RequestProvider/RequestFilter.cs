using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public class RequestFilter
    {
        /// <summary>
        /// Ids of Table
        /// </summary>
        public List<long> Ids { get; set; }        
        public List<long> BunchIds { get; set; }
        public string SearchText { get; set; }
        public string BunchName { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public RequestFilter()
        {
            Page = 1;
            Ids = new();
            PageSize = 10;
            BunchIds = new();                        
        }
    }
}
