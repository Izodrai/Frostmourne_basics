using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostmourne_basics
{
    public class Symbol
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string State { get; set; }

        public double Lot_max_size { get; set; }

        public double Lot_min_size { get; set; }

        public Symbol() { }

        public Symbol(int _id, string _name, string _description)
        {
            this.Id = _id;
            this.Name = _name;
            this.Description = _description;
            this.State = "unknown";
        }

        public Symbol(int _id, string _name, string _description, string _state, double _lot_min_size, double _lot_max_size)
        {
            this.Id = _id;
            this.Name = _name;
            this.Description = _description;
            this.State = _state;
            this.Lot_max_size = _lot_max_size;
            this.Lot_min_size = _lot_min_size;
        }
    }
}
