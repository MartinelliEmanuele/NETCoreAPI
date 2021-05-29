using System;
using System.Collections.Generic;

namespace Utils
{
    public class PagedResult<T>
    {
        public int CurrentPage { get; set; } = 0;
        public string NextUri { get; set; } = null;
        public string PreviousUri { get; set; } = null;
        public List<T> Results { get; set; } = new List<T>();
        public int TotalRows { get; set; } = 0;
    }
}
