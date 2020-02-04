using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Helpers
{
    public class SqlInjectionException : Exception
    {
        public SqlInjectionException() : base() { }
        public SqlInjectionException(string msg) : base(msg) { }
        public SqlInjectionException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
