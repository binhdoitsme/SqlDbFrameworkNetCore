using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq.Relational
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ManyToManyAttribute : Attribute
    {
        public Type ImmediateType { get; private set; }

        public ManyToManyAttribute(Type immediateType)
        {
            ImmediateType = immediateType;
        }
    }
}
