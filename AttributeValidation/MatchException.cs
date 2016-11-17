using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeValidation
{
    public class MatchException : Exception
    {
        public MatchException() : base() { }
        public MatchException(string message) : base(message) { }
        public MatchException(string message, System.Exception inner) : base(message, inner) { }
    }
}
