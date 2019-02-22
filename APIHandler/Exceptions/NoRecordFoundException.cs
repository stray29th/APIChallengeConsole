using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHandler.Exceptions
{
    public class NoRecordFoundException : Exception 
    {
        public NoRecordFoundException(string message) : base(message)
        {

        }
    }
}
