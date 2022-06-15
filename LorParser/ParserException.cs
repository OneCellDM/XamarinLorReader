using System;
using System.Collections.Generic;
using System.Text;

namespace LorParser
{
    public class ParserException:Exception 
    {
        public ParserException() : base() { }
        public ParserException(string message) : base(message) { }
    }
}
