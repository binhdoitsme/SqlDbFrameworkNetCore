using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.TestModels
{
    class Phone
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public int ManufacturerId { get; set; }

        public override string ToString()
        {
            return $"{GetType().Name}:<{Name}, {Model}, {Year}>";
        }
    }
}
