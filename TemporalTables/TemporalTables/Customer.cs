using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalTables
{
    public class Customer
    {
        public Customer(string name)
        {
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; init; }

        public List<Order> Orders { get; } = new List<Order>();
    }
}
