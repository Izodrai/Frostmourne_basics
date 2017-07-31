using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostmourne_basics
{
    public class Error
    {
        public bool IsAnError { get; set; }
        public string MessageError { get; set; }

        public Error() { }

        public Error(bool _isAnError, string _messageError)
        {
            this.IsAnError = _isAnError;
            this.MessageError = _messageError;
        }
    }
}
