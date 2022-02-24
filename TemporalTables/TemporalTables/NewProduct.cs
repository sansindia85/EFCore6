using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalTables
{
    public class NewProduct
    {
        public NewProduct(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public Guid Id { get; private set; }
        public string Name { get; init; }

        //[Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
