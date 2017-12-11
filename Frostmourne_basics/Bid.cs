using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostmourne_basics
{
    public class Bid
    {
        public string Symbol { get; set; }

        public Int64 Bid_at { get; set; }

        public double Last_bid { get; set; }

        public Bid() { }

        public Bid(string _symbol, Int64 _bid_at, double _last_bid)
        {
            this.Symbol = _symbol;
            this.Bid_at = _bid_at;
            this.Last_bid = _last_bid;
        }
    }
}
