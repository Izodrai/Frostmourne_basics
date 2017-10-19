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

        public Symbol() { }

        public Symbol(int _id, string _name)
        {
            this.Id = _id;
            this.Name = _name;
        }
    }
}
