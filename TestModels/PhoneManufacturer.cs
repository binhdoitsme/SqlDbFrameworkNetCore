using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.TestModels
{
    public class PhoneManufacturer
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return $"{GetType().Name}:<{Id}>";
        }
    }
}
