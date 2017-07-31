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

        public Symbol() { }

        public Symbol(int _id, string _name, string _description)
        {
            this.Id = _id;
            this.Name = _name;
            this.Description = _description;
            this.State = "unknown";
        }

        public Symbol(int _id, string _name, string _description, string _state)
        {
            this.Id = _id;
            this.Name = _name;
            this.Description = _description;
            this.State = _state;
        }
    }
}
