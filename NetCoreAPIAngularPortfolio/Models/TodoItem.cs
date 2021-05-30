using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPIAngularPortfolio.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = "";
        public DateTime? Due { get; set; } = null;
        public bool IsComplete { get; set; }
    }
}
