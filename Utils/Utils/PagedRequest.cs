using System.Collections.Generic;

namespace Utils
{
    public class PagedRequest
    {
        public int CurrentPage { get; set; } = 1;
        public int RowsPerPage { get; set; } = 0;
        public List<Filter> Filters { get; set; } = new List<Filter>();
        public List<string> OrderByFields { get; set; } = new List<string>();
        public string FilterLogicOperator { get; set; } = " AND ";
        //table name
        public string Entity { get; set; }
        public bool Distinct { get; set; } = false;
    }
}
