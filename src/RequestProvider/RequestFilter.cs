using System.Collections.Generic;

namespace MtdKey.Storage
{
    public class RequestFilter
    {
        public List<long> BunchIds { get; set; }
        public List<long> FieldIds { get; set; }
        public List<long> NodeIds { get; set; }
        public string SearchText { get; set; }
        public List<string> BunchNames { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public RequestFilter()
        {
            Page = 1;
            BunchIds = new();
            BunchNames = new();
            FieldIds = new();
            NodeIds = new();
            PageSize = 10;
        }
    }
}
