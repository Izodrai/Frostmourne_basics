using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostmourne_basics
{
    public class Bid_by_date
    {
        public DateTime Bid_date { get; set; }

        public int Count { get; set; }

        public Bid_by_date(DateTime _bid_date, int _count)
        {
            this.Bid_date = _bid_date;
            this.Count = _count;
        }
    }
}
