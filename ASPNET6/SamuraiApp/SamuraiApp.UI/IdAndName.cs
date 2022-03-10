using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.UI
{
    public struct IdAndName
    {
        public IdAndName(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id;
        public string Name;
    }
}
